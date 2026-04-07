using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnDepartments;
        private System.Windows.Forms.Button btnBookAppt;
        private System.Windows.Forms.Button btnTodayQueue;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnCalendar;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnSecurity;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnAudit;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnToggleSidebar;

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblBadge;
        private System.Windows.Forms.Timer clockTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.btnToggleSidebar = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnDepartments = new System.Windows.Forms.Button();
            this.btnPatients = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnSecurity = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnAudit = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblLoggedInUser = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.sidebarPanel.SuspendLayout();
            this.topBar.SuspendLayout();
            this.SuspendLayout();

            // CLOCK
            this.clockTimer.Interval = 1000;
            this.clockTimer.Tick += new System.EventHandler(this.ClockTimer_Tick);
            this.clockTimer.Start();

            // SIDEBAR
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Size = new System.Drawing.Size(240, 700);
            this.sidebarPanel.Padding = new System.Windows.Forms.Padding(0, 20, 0, 10);
            this.sidebarPanel.AutoScroll = true;

            // Toggle
            this.btnToggleSidebar.Text = "\u2630";
            this.btnToggleSidebar.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnToggleSidebar.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidebar.FlatStyle = FlatStyle.Flat;
            this.btnToggleSidebar.FlatAppearance.BorderSize = 0;
            this.btnToggleSidebar.Size = new System.Drawing.Size(40, 40);
            this.btnToggleSidebar.Location = new System.Drawing.Point(10, 10);
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);
            this.sidebarPanel.Controls.Add(this.btnToggleSidebar);

            // Logo & Badge
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLogo.Text = " ALPHA CLINIC";
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(55, 15);
            this.lblLogo.Size = new System.Drawing.Size(180, 40);
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLogo.Name = "lblLogo";

            this.lblBadge = new System.Windows.Forms.Label();
            this.lblBadge.Text = "OVERSIGHT";
            this.lblBadge.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold);
            this.lblBadge.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblBadge.Location = new System.Drawing.Point(90, 45);
            this.lblBadge.Size = new System.Drawing.Size(100, 20);
            this.lblBadge.Name = "lblBadge";
            
            this.sidebarPanel.Controls.Add(this.lblLogo);
            this.sidebarPanel.Controls.Add(this.lblBadge);

            // Sidebar Buttons
            this.btnDashboard.Text = "  \u25A0  Dashboard";
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDashboard.Location = new System.Drawing.Point(5, 90);
            this.btnDashboard.Size = new System.Drawing.Size(230, 42);
            this.btnDashboard.FlatStyle = FlatStyle.Flat;
            this.btnDashboard.ForeColor = Color.White;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            this.btnDashboard.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnDashboard);

            this.btnUsers.Text = "  \u263A  Users";
            this.btnUsers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUsers.Location = new System.Drawing.Point(5, 134);
            this.btnUsers.Size = new System.Drawing.Size(230, 42);
            this.btnUsers.FlatStyle = FlatStyle.Flat;
            this.btnUsers.ForeColor = Color.White;
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.TextAlign = ContentAlignment.MiddleLeft;
            this.btnUsers.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnUsers);

            this.btnDoctors.Text = "  \u2695  Doctors";
            this.btnDoctors.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDoctors.Location = new System.Drawing.Point(5, 178);
            this.btnDoctors.Size = new System.Drawing.Size(230, 42);
            this.btnDoctors.FlatStyle = FlatStyle.Flat;
            this.btnDoctors.ForeColor = Color.White;
            this.btnDoctors.FlatAppearance.BorderSize = 0;
            this.btnDoctors.TextAlign = ContentAlignment.MiddleLeft;
            this.btnDoctors.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnDoctors);

            this.btnDepartments.Text = "  \u25A6  Departments";
            this.btnDepartments.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDepartments.Location = new System.Drawing.Point(5, 222);
            this.btnDepartments.Size = new System.Drawing.Size(230, 42);
            this.btnDepartments.FlatStyle = FlatStyle.Flat;
            this.btnDepartments.ForeColor = Color.White;
            this.btnDepartments.FlatAppearance.BorderSize = 0;
            this.btnDepartments.TextAlign = ContentAlignment.MiddleLeft;
            this.btnDepartments.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnDepartments);

            this.btnPatients.Text = "  \u2764  Patients";
            this.btnPatients.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPatients.Location = new System.Drawing.Point(5, 266);
            this.btnPatients.Size = new System.Drawing.Size(230, 42);
            this.btnPatients.FlatStyle = FlatStyle.Flat;
            this.btnPatients.ForeColor = Color.White;
            this.btnPatients.FlatAppearance.BorderSize = 0;
            this.btnPatients.TextAlign = ContentAlignment.MiddleLeft;
            this.btnPatients.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnPatients);

            this.btnBookAppt = new System.Windows.Forms.Button();
            this.btnBookAppt.Text = "  \u271A  Book Appointment";
            this.btnBookAppt.Tag = new string[] { "  \u271A  Book Appointment", "\u271A" };
            this.btnBookAppt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBookAppt.Location = new System.Drawing.Point(5, 310);
            this.btnBookAppt.Size = new System.Drawing.Size(230, 42);
            this.btnBookAppt.FlatStyle = FlatStyle.Flat;
            this.btnBookAppt.ForeColor = Color.White;
            this.btnBookAppt.FlatAppearance.BorderSize = 0;
            this.btnBookAppt.TextAlign = ContentAlignment.MiddleLeft;
            this.btnBookAppt.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnBookAppt);

            this.btnTodayQueue = new System.Windows.Forms.Button();
            this.btnTodayQueue.Text = "  \u231A  Today\'s Queue";
            this.btnTodayQueue.Tag = new string[] { "  \u231A  Today\'s Queue", "\u231A" };
            this.btnTodayQueue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnTodayQueue.Location = new System.Drawing.Point(5, 354);
            this.btnTodayQueue.Size = new System.Drawing.Size(230, 42);
            this.btnTodayQueue.FlatStyle = FlatStyle.Flat;
            this.btnTodayQueue.ForeColor = Color.White;
            this.btnTodayQueue.FlatAppearance.BorderSize = 0;
            this.btnTodayQueue.TextAlign = ContentAlignment.MiddleLeft;
            this.btnTodayQueue.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnTodayQueue);

            this.btnAppointments.Text = "  \u2637  Appointments";
            this.btnAppointments.Tag = new string[] { "  \u2637  Appointments", "\u2637" };
            this.btnAppointments.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAppointments.Location = new System.Drawing.Point(5, 398);
            this.btnAppointments.Size = new System.Drawing.Size(230, 42);
            this.btnAppointments.FlatStyle = FlatStyle.Flat;
            this.btnAppointments.ForeColor = Color.White;
            this.btnAppointments.FlatAppearance.BorderSize = 0;
            this.btnAppointments.TextAlign = ContentAlignment.MiddleLeft;
            this.btnAppointments.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnAppointments);

            this.btnCalendar.Text = "  \u25CB  Calendar";
            this.btnCalendar.Tag = new string[] { "  \u25CB  Calendar", "\u25CB" };
            this.btnCalendar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCalendar.Location = new System.Drawing.Point(5, 442);
            this.btnCalendar.Size = new System.Drawing.Size(230, 42);
            this.btnCalendar.FlatStyle = FlatStyle.Flat;
            this.btnCalendar.ForeColor = Color.White;
            this.btnCalendar.FlatAppearance.BorderSize = 0;
            this.btnCalendar.TextAlign = ContentAlignment.MiddleLeft;
            this.btnCalendar.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnCalendar);

            this.btnReports.Text = "  \u2261  Reports";
            this.btnReports.Tag = new string[] { "  \u2261  Reports", "\u2261" };
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReports.Location = new System.Drawing.Point(5, 486);
            this.btnReports.Size = new System.Drawing.Size(230, 42);
            this.btnReports.FlatStyle = FlatStyle.Flat;
            this.btnReports.ForeColor = Color.White;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.TextAlign = ContentAlignment.MiddleLeft;
            this.btnReports.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnReports);

            this.btnSettings.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnSettings);
            
            this.btnSecurity.Text = "  \u26BF  Security";
            this.btnSecurity.Tag = new string[] { "  \u26BF  Security", "\u26BF" };
            this.btnSecurity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSecurity.Location = new System.Drawing.Point(5, 574);
            this.btnSecurity.Size = new System.Drawing.Size(230, 42);
            this.btnSecurity.FlatStyle = FlatStyle.Flat;
            this.btnSecurity.ForeColor = Color.White;
            this.btnSecurity.FlatAppearance.BorderSize = 0;
            this.btnSecurity.TextAlign = ContentAlignment.MiddleLeft;
            this.btnSecurity.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnSecurity);

            this.btnBackup.Text = "  \u2193  Backup";
            this.btnBackup.Tag = new string[] { "  \u2193  Backup", "\u2193" };
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBackup.Location = new System.Drawing.Point(5, 618);
            this.btnBackup.Size = new System.Drawing.Size(230, 42);
            this.btnBackup.FlatStyle = FlatStyle.Flat;
            this.btnBackup.ForeColor = Color.White;
            this.btnBackup.FlatAppearance.BorderSize = 0;
            this.btnBackup.TextAlign = ContentAlignment.MiddleLeft;
            this.btnBackup.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnBackup);

            this.btnAudit.Text = "  \u2611  Audit";
            this.btnAudit.Tag = new string[] { "  \u2611  Audit", "\u2611" };
            this.btnAudit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAudit.Location = new System.Drawing.Point(5, 662);
            this.btnAudit.Size = new System.Drawing.Size(230, 42);
            this.btnAudit.FlatStyle = FlatStyle.Flat;
            this.btnAudit.ForeColor = Color.White;
            this.btnAudit.FlatAppearance.BorderSize = 0;
            this.btnAudit.TextAlign = ContentAlignment.MiddleLeft;
            this.btnAudit.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnAudit);

            this.btnLogout.Text = "  \u2716  Logout";
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Size = new System.Drawing.Size(230, 42);
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            this.btnLogout.Click += new System.EventHandler(this.Logout_Click);
            this.sidebarPanel.Controls.Add(this.btnLogout);

            // TOPBAR
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.Height = 60;
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblPageTitle.Visible = false;

            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTime.ForeColor = System.Drawing.Color.Gray;
            this.lblDateTime.Location = new System.Drawing.Point(1100, 35);
            this.lblDateTime.Size = new System.Drawing.Size(180, 15);
            this.lblDateTime.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.topBar.Controls.Add(this.lblPageTitle);
            this.topBar.Controls.Add(this.lblDateTime);

            // CONTENT
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.AutoScroll = true;

            // MAIN PANEL (Nesting container for stability)
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.topBar);

            // FORM ASSEMBLY
            this.Text = "Alpha Clinic - Administrative Oversight Engine";
            this.ClientSize = new System.Drawing.Size(1300, 750);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.sidebarPanel);
            
            this.sidebarPanel.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
