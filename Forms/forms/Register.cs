using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter full name!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter phone number!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (!long.TryParse(txtPhone.Text, out _))
            {
                MessageBox.Show("Phone must contain only numbers!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter address!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            if (!rbtnMale.Checked && !rbtnFemale.Checked)
            {
                MessageBox.Show("Please select gender!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter username!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (DataManager.Users.Any(u => u.Username == txtUsername.Text.Trim()))
            {
                MessageBox.Show("Username already exists!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            string email = txtEmail.Text.Trim();

            try
            {
                // Create new patient
                Patient newPatient = new Patient
                {
                    Id = DataManager.Patients.Count > 0 ? DataManager.Patients.Max(p => p.Id) + 1 : 1,
                    FullName = txtFullName.Text.Trim(),
                    DateOfBirth = dtpDateOfBirth.Value,
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Gender = rbtnMale.Checked ? "Male" : "Female"
                };
                DataManager.Patients.Add(newPatient);

                // Create new user
                User newUser = new User
                {
                    Id = DataManager.Users.Count > 0 ? DataManager.Users.Max(u => u.Id) + 1 : 1,
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    Role = "Patient", // IMPORTANT: Set role to Patient
                    PatientId = newPatient.Id,
                    Email = email
                };
                DataManager.Users.Add(newUser);

                MessageBox.Show("Registration successful! Please login with your new account.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();

                // Open login page
                if (Program.MainForm != null)
                {
                    Login loginForm = new Login(Program.MainForm);
                    loginForm.Show();
                }
                else
                {
                    Login loginForm = new Login();
                    loginForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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