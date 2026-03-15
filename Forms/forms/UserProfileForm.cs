using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class UserProfileForm : Form
    {
        private Patient currentPatient;
        private User currentUser;

        public UserProfileForm()
        {
            InitializeComponent();
            LoadUserData();
            SetupAvatar();
        }

        private void SetupAvatar()
        {
            // Draw avatar with person icon and first letter of name
            this.picAvatar.Paint += (s, e) => {
                // Draw circle background
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 191, 255)))
                {
                    e.Graphics.FillEllipse(brush, 5, 5, 70, 70);
                }

                // Draw first letter of name
                if (currentPatient != null && !string.IsNullOrEmpty(currentPatient.FullName))
                {
                    string firstLetter = currentPatient.FullName.Substring(0, 1).ToUpper();
                    using (Font font = new Font("Segoe UI", 24, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        // Center the letter
                        SizeF textSize = e.Graphics.MeasureString(firstLetter, font);
                        float x = (80 - textSize.Width) / 2;
                        float y = (80 - textSize.Height) / 2;
                        e.Graphics.DrawString(firstLetter, font, brush, x, y);
                    }
                }
            };
            this.picAvatar.Invalidate();
        }

        private void LoadUserData()
        {
            currentPatient = DataManager.GetCurrentPatient();
            currentUser = DataManager.CurrentUser;

            if (currentPatient != null)
            {
                txtFullName.Text = currentPatient.FullName;
                txtPhone.Text = currentPatient.Phone;
                txtAddress.Text = currentPatient.Address;
                dtpDOB.Value = currentPatient.DateOfBirth;

                if (currentPatient.Gender == "Male")
                    rbtnMale.Checked = true;
                else
                    rbtnFemale.Checked = true;

                txtUsername.Text = currentUser?.Username ?? "";
                txtEmail.Text = currentUser?.Email ?? "";

                // Set avatar letter
                SetupAvatar();
            }
            else
            {
                MessageBox.Show("No patient data found!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter full name!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter phone number!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(txtPhone.Text, out _))
            {
                MessageBox.Show("Phone must contain only numbers!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!rbtnMale.Checked && !rbtnFemale.Checked)
            {
                MessageBox.Show("Please select gender!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Update patient information
                currentPatient.FullName = txtFullName.Text.Trim();
                currentPatient.Phone = txtPhone.Text.Trim();
                currentPatient.Address = txtAddress.Text.Trim();
                currentPatient.DateOfBirth = dtpDOB.Value;
                currentPatient.Gender = rbtnMale.Checked ? "Male" : "Female";

                // Update user email if changed
                if (currentUser != null)
                {
                    currentUser.Email = txtEmail.Text.Trim();
                }

                DataManager.SavePatients();
                DataManager.SaveUsers();

                MessageBox.Show("Profile updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm();
            changePasswordForm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}