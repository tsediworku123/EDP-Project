using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class Login : Form
    {
        private MainContainer mainForm;

        public Login(MainContainer main = null)
        {
            mainForm = main;
            InitializeComponent();
            SetupLogo();
        }

        private void SetupLogo()
        {
            this.pictureBoxIcon.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(0, 191, 255), 8))
                {
                    e.Graphics.DrawLine(pen, 15, 30, 45, 30);
                    e.Graphics.DrawLine(pen, 30, 15, 30, 45);
                }
            };
            this.pictureBoxIcon.Invalidate();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = DataManager.AuthenticateUser(username, password);

            if (user != null)
            {
                string name = user.Role == "Admin" ? "Admin" :
                    DataManager.Patients.FirstOrDefault(p => p.Id == user.PatientId)?.FullName ?? username;

                DataManager.CurrentUser = user;

                if (mainForm != null)
                {
                    // This will open the appropriate dashboard based on role
                    mainForm.LoginSuccess(user.Role, name);
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Welcome {name}!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    // Check role and open appropriate dashboard
                    if (user.Role == "Admin")
                    {
                        AdminDashboard adminDashboard = new AdminDashboard();
                        adminDashboard.ShowDialog();
                    }
                    else
                    {
                        UserDashboard userDashboard = new UserDashboard();
                        userDashboard.ShowDialog();
                    }
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register regForm = new Register();
            regForm.ShowDialog();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}