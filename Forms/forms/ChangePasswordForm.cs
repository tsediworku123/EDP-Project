using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class ChangePasswordForm : Form
    {
        private User currentUser;

        public ChangePasswordForm()
        {
            InitializeComponent();
            currentUser = DataManager.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("You must be logged in to change password.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            SetupLockIcon();
            this.Resize += ChangePasswordForm_Resize;
        }

        private void ChangePasswordForm_Resize(object sender, EventArgs e)
        {
            CenterForm();
        }

        private void CenterForm()
        {
            // Center the panel in the form
            if (panelContent != null)
            {
                panelContent.Location = new Point(
                    (this.ClientSize.Width - panelContent.Width) / 2,
                    (this.ClientSize.Height - panelContent.Height) / 2
                );
            }
        }

        private void SetupLockIcon()
        {
            // Draw lock icon on the picture box
            this.picLockIcon.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.White, 3))
                {
                    // Lock body
                    e.Graphics.DrawRectangle(pen, 15, 25, 30, 20);
                    // Lock handle (arc)
                    e.Graphics.DrawArc(pen, 22, 15, 16, 16, 0, 180);
                }
            };
            this.picLockIcon.Invalidate();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string currentPass = txtCurrentPassword.Text;
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            // Validation
            if (string.IsNullOrEmpty(currentPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Please fill all fields!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentPass != currentUser.Password)
            {
                MessageBox.Show("Current password is incorrect!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass.Length < 4)
            {
                MessageBox.Show("New password must be at least 4 characters!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("New password and confirm password do not match!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update password
            currentUser.Password = newPass;
            DataManager.SaveUsers();

            MessageBox.Show("Password changed successfully! Please login with your new password.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Logout the user
            DataManager.CurrentUser = null;
            this.Close();

            // Return to login page
            Login loginForm = new Login(Program.MainForm);
            loginForm.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}