using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    public partial class MainContainer : Form
    {
        private bool isLoggedIn = false;
        private string userRole = "";
        private string userName = "";

        public MainContainer()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.MainMenuStrip = menuStrip;
            ShowHomePage();
        }

        private void ShowHomePage()
        {
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            HomePage homePage = new HomePage();
            homePage.MdiParent = this;
            homePage.WindowState = FormWindowState.Maximized;
            homePage.Show();
            UpdateMenuForRole();
        }

        public void OpenForm(Form form)
        {
            if (!isLoggedIn && !(form is HomePage) && !(form is Login) && !(form is Register))
            {
                MessageBox.Show("Please login first.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        public void UpdateMenuForRole()
        {
            menuStrip.Items.Clear();

            if (!isLoggedIn)
            {
                ToolStripMenuItem homeItem = new ToolStripMenuItem("Home");
                homeItem.Click += (s, e) => ShowHomePage();
                menuStrip.Items.Add(homeItem);
                return;
            }

            // ===== FILE MENU (common for both) =====
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

            ToolStripMenuItem dashboardItem = new ToolStripMenuItem("Dashboard");
            dashboardItem.Click += (s, e) => {
                if (userRole == "Admin")
                    OpenForm(new AdminDashboard());
                else
                    OpenForm(new UserDashboard());
            };
            fileMenu.DropDownItems.Add(dashboardItem);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitItem);
            menuStrip.Items.Add(fileMenu);

            // ===== HOME MENU (common for both) =====
            ToolStripMenuItem homeMenuItem = new ToolStripMenuItem("Home");
            homeMenuItem.Click += (s, e) => ShowHomePage();
            menuStrip.Items.Add(homeMenuItem);

            // ===== ROLE-SPECIFIC MENUS =====
            if (userRole == "Admin")
            {
                // ADMIN MENUS
                ToolStripMenuItem doctorsItem = new ToolStripMenuItem("Doctors");
                doctorsItem.Click += (s, e) => OpenForm(new DoctorsForm());
                menuStrip.Items.Add(doctorsItem);

                ToolStripMenuItem patientsItem = new ToolStripMenuItem("Patients");
                patientsItem.Click += (s, e) => OpenForm(new PatientForm());
                menuStrip.Items.Add(patientsItem);

                ToolStripMenuItem appointmentsItem = new ToolStripMenuItem("Appointments");
                appointmentsItem.Click += (s, e) => OpenForm(new BookAppointmentForm());
                menuStrip.Items.Add(appointmentsItem);

                ToolStripMenuItem reportsItem = new ToolStripMenuItem("Reports");
                reportsItem.Click += (s, e) => OpenForm(new ReportsForm());
                menuStrip.Items.Add(reportsItem);
            }
            else
            {
                // PATIENT/USER MENUS - These are what patients should see

                // Appointments Menu (with dropdown)
                ToolStripMenuItem appointmentsItem = new ToolStripMenuItem("Appointments");
                appointmentsItem.DropDownItems.Add("Book Appointment", null, (s, e) => OpenForm(new BookAppointmentForm()));
                appointmentsItem.DropDownItems.Add("My Appointments", null, (s, e) => OpenForm(new MyAppointmentsForm()));
                menuStrip.Items.Add(appointmentsItem);

                // Doctors Menu (view only)
                ToolStripMenuItem doctorsItem = new ToolStripMenuItem("Doctors");
                doctorsItem.Click += (s, e) => OpenForm(new ViewDoctorsForm());
                menuStrip.Items.Add(doctorsItem);

                // Medical Records Menu
                ToolStripMenuItem medicalItem = new ToolStripMenuItem("Medical Records");
                medicalItem.DropDownItems.Add("Medical History", null, (s, e) => OpenForm(new MedicalHistoryForm()));
                medicalItem.DropDownItems.Add("My Profile", null, (s, e) => OpenForm(new UserProfileForm()));
                menuStrip.Items.Add(medicalItem);

                // Feedback Menu
                ToolStripMenuItem feedbackItem = new ToolStripMenuItem("Feedback");
                feedbackItem.DropDownItems.Add("Give Feedback", null, (s, e) => OpenForm(new GiveFeedbackForm()));
                feedbackItem.DropDownItems.Add("Notifications", null, (s, e) => OpenForm(new NotificationsForm()));
                menuStrip.Items.Add(feedbackItem);
            }

            // ===== LOGOUT MENU (common for both) =====
            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Logout");
            logoutItem.Click += LogoutMenuItem_Click;
            menuStrip.Items.Add(logoutItem);

            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            if (isLoggedIn)
            {
                lblUser.Text = $"Logged in as: {userName} ({userRole})";
                lblStatus.Text = "Ready";
            }
            else
            {
                lblUser.Text = "Not logged in";
                lblStatus.Text = "Ready";
            }
        }

        public void LoginSuccess(string role, string name)
        {
            isLoggedIn = true;
            userRole = role;
            userName = name;
            UpdateMenuForRole();
            UpdateStatusBar();

            if (role == "Admin")
            {
                OpenForm(new AdminDashboard());
            }
            else
            {
                OpenForm(new UserDashboard());
            }
        }

        private void LogoutMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                isLoggedIn = false;
                userRole = "";
                userName = "";
                DataManager.CurrentUser = null;
                UpdateMenuForRole();
                UpdateStatusBar();
                ShowHomePage();
            }
        }
    }
}