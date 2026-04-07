using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class Login : Form
    {
        private MainContainer mainForm;
        private bool isAuthenticating = false;

        public Login(MainContainer main = null)
        {
            mainForm = main;
            InitializeComponent();
            SetupLogo();
            LoadSettings();
        }

        private void LoadSettings()
        {
            var sm = SessionManager.Instance;
            if (sm.RememberMe)
            {
                chkRememberMe.Checked = true;
                txtUsername.Text = sm.SavedUsername;
                txtPassword.Text = sm.SavedPassword;
            }

            if (sm.AutoLogin)
            {
                chkAutoLogin.Checked = true;
                this.Shown += (s, e) => {
                    if (MainContainer.isTransitioning) return;
                    btnLogin_Click(null, null);
                };
            }
        }

        private void chkRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            chkAutoLogin.Visible = chkRememberMe.Checked;
            if (!chkRememberMe.Checked)
            {
                chkAutoLogin.Checked = false;
            }
        }

        private void SetupLogo()
        {
            this.pictureBoxIcon.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color medicalBlue = Color.FromArgb(0, 174, 219);
                using (Brush brush = new SolidBrush(medicalBlue))
                {
                    // Draw a modern medical cross
                    e.Graphics.FillRectangle(brush, 22, 10, 16, 40);
                    e.Graphics.FillRectangle(brush, 10, 22, 40, 16);
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

            if (isAuthenticating || MainContainer.isTransitioning) return;
            isAuthenticating = true;
            btnLogin.Enabled = false;

            var user = DataManager.AuthenticateUser(username, password);

            if (user != null)
            {
                string name = user.Role == "Admin" ? "Admin" :
                    DataManager.Patients.FirstOrDefault(p => p.Id == user.PatientId)?.FullName ?? username;

                DataManager.CurrentUser = user;

                var sm = SessionManager.Instance;
                if (chkRememberMe.Checked)
                {
                    sm.RememberMe = true;
                    sm.SavedUsername = username;
                    sm.SavedPassword = password;
                    sm.AutoLogin = chkAutoLogin.Checked;
                }
                else
                {
                    sm.RememberMe = false;
                    sm.SavedUsername = "";
                    sm.SavedPassword = "";
                    sm.AutoLogin = false;
                }
                sm.SaveSettings();
                
                sm.StartSessionTracking();

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
                        OpenAdminDashboard();
                    }
                    else if (user.Role == "Receptionist")
                    {
                        ReceptionDashboardForm receptionDashboard = new ReceptionDashboardForm();
                        receptionDashboard.ShowDialog();
                    }
                    else if (user.Role == "Doctor")
                    {
                        DoctorDashboard doctorDashboard = new DoctorDashboard(user.DoctorId);
                        doctorDashboard.ShowDialog();
                    }
                    else if (user.Role == "Patient")
                    {
                        PatientDashboard patientDashboard = new PatientDashboard(user.PatientId);
                        patientDashboard.ShowDialog();
                    }
                    this.Close();
                }
            }
            else
            {
                isAuthenticating = false;
                btnLogin.Enabled = true;
                MessageBox.Show("Invalid username or password!", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenAdminDashboard()
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ShowDialog();
        }



        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            RegisterPatientForm regForm = new RegisterPatientForm();
            regForm.ShowDialog();
            this.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
