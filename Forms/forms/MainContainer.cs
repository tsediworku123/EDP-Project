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

            // FILE Menu
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

            // HOME Menu
            ToolStripMenuItem homeMenuItem = new ToolStripMenuItem("Home");
            homeMenuItem.Click += (s, e) => ShowHomePage();
            menuStrip.Items.Add(homeMenuItem);

            // DOCTORS Menu
            ToolStripMenuItem doctorsItem = new ToolStripMenuItem("Doctors");
            doctorsItem.Click += (s, e) => OpenForm(new DoctorsForm());
            menuStrip.Items.Add(doctorsItem);

            // PATIENTS Menu
            ToolStripMenuItem patientsItem = new ToolStripMenuItem("Patients");
            patientsItem.Click += (s, e) => OpenForm(new PatientForm());
            menuStrip.Items.Add(patientsItem);

            // APPOINTMENTS Menu
            ToolStripMenuItem appointmentsItem = new ToolStripMenuItem("Appointments");
            appointmentsItem.Click += (s, e) => OpenForm(new BookAppointmentForm());
            menuStrip.Items.Add(appointmentsItem);

            // REPORTS Menu
            ToolStripMenuItem reportsItem = new ToolStripMenuItem("Reports");
            reportsItem.Click += (s, e) => OpenForm(new ReportsForm());
            menuStrip.Items.Add(reportsItem);

            // LOGOUT Menu
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

            // Update the menu first
            UpdateMenuForRole();
            UpdateStatusBar();

            // Close any open forms
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            // Open the appropriate dashboard based on role
            if (role == "Admin")
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.MdiParent = this;
                adminDashboard.WindowState = FormWindowState.Maximized;
                adminDashboard.Show();
            }
            else
            {
                UserDashboard userDashboard = new UserDashboard();
                userDashboard.MdiParent = this;
                userDashboard.WindowState = FormWindowState.Maximized;
                userDashboard.Show();
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