using System;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // Sidebar buttons
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnToggleSidebar;

        // Other controls
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblPageTitle;

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
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.btnToggleSidebar = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnPatients = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblLoggedInUser = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.sidebarPanel.SuspendLayout();
            this.topBar.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.SuspendLayout();

            // Form properties
            this.Text = "Clinic Management System - Admin Dashboard";
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.MaximizeBox = true;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Name = "AdminDashboard";

            // Sidebar Panel
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Size = new System.Drawing.Size(250, 700);
            this.sidebarPanel.MinimumSize = new System.Drawing.Size(60, 0);
            this.sidebarPanel.Padding = new System.Windows.Forms.Padding(0, 20, 0, 10);
            this.sidebarPanel.TabIndex = 0;

            // Toggle Button (always visible)
            this.btnToggleSidebar.Text = "☰";
            this.btnToggleSidebar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnToggleSidebar.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidebar.BackColor = System.Drawing.Color.Transparent;
            this.btnToggleSidebar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSidebar.FlatAppearance.BorderSize = 0;
            this.btnToggleSidebar.Size = new System.Drawing.Size(40, 40);
            this.btnToggleSidebar.Location = new System.Drawing.Point(10, 10);
            this.btnToggleSidebar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleSidebar.Name = "btnToggleSidebar";
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);
            this.sidebarPanel.Controls.Add(this.btnToggleSidebar);

            // Logo (will be hidden when collapsed)
            System.Windows.Forms.Label lblLogo = new System.Windows.Forms.Label();
            lblLogo.Text = "🏥 CLINIC MS";
            lblLogo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblLogo.ForeColor = System.Drawing.Color.White;
            lblLogo.Location = new System.Drawing.Point(60, 15);
            lblLogo.Size = new System.Drawing.Size(180, 40);
            lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblLogo.Name = "lblLogo";
            this.sidebarPanel.Controls.Add(lblLogo);

            // Admin Badge (will be hidden when collapsed)
            System.Windows.Forms.Label lblAdminBadge = new System.Windows.Forms.Label();
            lblAdminBadge.Text = "ADMIN";
            lblAdminBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            lblAdminBadge.ForeColor = System.Drawing.Color.White;
            lblAdminBadge.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            lblAdminBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblAdminBadge.Location = new System.Drawing.Point(180, 25);
            lblAdminBadge.Size = new System.Drawing.Size(60, 20);
            lblAdminBadge.Name = "lblAdminBadge";
            this.sidebarPanel.Controls.Add(lblAdminBadge);

            // Separator
            System.Windows.Forms.Label lblSeparator = new System.Windows.Forms.Label();
            lblSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSeparator.Location = new System.Drawing.Point(20, 70);
            lblSeparator.Size = new System.Drawing.Size(210, 2);
            lblSeparator.Name = "lblSeparator";
            this.sidebarPanel.Controls.Add(lblSeparator);

            // Dashboard Button
            this.btnDashboard.Text = "     📊 DASHBOARD";
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Size = new System.Drawing.Size(230, 45);
            this.btnDashboard.Location = new System.Drawing.Point(10, 90);
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnDashboard);

            // Doctors Button
            this.btnDoctors.Text = "     👨‍⚕️ DOCTORS";
            this.btnDoctors.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.btnDoctors.ForeColor = System.Drawing.Color.White;
            this.btnDoctors.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnDoctors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoctors.FlatAppearance.BorderSize = 0;
            this.btnDoctors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDoctors.Size = new System.Drawing.Size(230, 45);
            this.btnDoctors.Location = new System.Drawing.Point(10, 140);
            this.btnDoctors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDoctors.Name = "btnDoctors";
            this.btnDoctors.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnDoctors);

            // Patients Button
            this.btnPatients.Text = "     👤 PATIENTS";
            this.btnPatients.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.btnPatients.ForeColor = System.Drawing.Color.White;
            this.btnPatients.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnPatients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPatients.FlatAppearance.BorderSize = 0;
            this.btnPatients.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPatients.Size = new System.Drawing.Size(230, 45);
            this.btnPatients.Location = new System.Drawing.Point(10, 190);
            this.btnPatients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPatients.Name = "btnPatients";
            this.btnPatients.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnPatients);

            // Appointments Button
            this.btnAppointments.Text = "     📅 APPOINTMENTS";
            this.btnAppointments.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.btnAppointments.ForeColor = System.Drawing.Color.White;
            this.btnAppointments.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppointments.FlatAppearance.BorderSize = 0;
            this.btnAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppointments.Size = new System.Drawing.Size(230, 45);
            this.btnAppointments.Location = new System.Drawing.Point(10, 240);
            this.btnAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnAppointments);

            // Reports Button
            this.btnReports.Text = "     📈 REPORTS";
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.Size = new System.Drawing.Size(230, 45);
            this.btnReports.Location = new System.Drawing.Point(10, 290);
            this.btnReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReports.Name = "btnReports";
            this.btnReports.Click += new System.EventHandler(this.SidebarButton_Click);
            this.sidebarPanel.Controls.Add(this.btnReports);

            // Separator before logout
            System.Windows.Forms.Label lblSeparator2 = new System.Windows.Forms.Label();
            lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSeparator2.Location = new System.Drawing.Point(20, 350);
            lblSeparator2.Size = new System.Drawing.Size(210, 2);
            lblSeparator2.Name = "lblSeparator2";
            this.sidebarPanel.Controls.Add(lblSeparator2);

            // Logout Button
            this.btnLogout.Text = "     🚪 LOGOUT";
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Size = new System.Drawing.Size(230, 45);
            this.btnLogout.Location = new System.Drawing.Point(10, 370);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Click += new System.EventHandler(this.Logout_Click);
            this.sidebarPanel.Controls.Add(this.btnLogout);

            // Version
            System.Windows.Forms.Label lblVersion = new System.Windows.Forms.Label();
            lblVersion.Text = "Version 2.0";
            lblVersion.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblVersion.ForeColor = System.Drawing.Color.FromArgb(149, 165, 166);
            lblVersion.Location = new System.Drawing.Point(20, 660);
            lblVersion.Size = new System.Drawing.Size(210, 20);
            lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblVersion.Name = "lblVersion";
            this.sidebarPanel.Controls.Add(lblVersion);

            // ========== TOP BAR - NO DASHBOARD TEXT ==========
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.Height = 60;
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBar.TabIndex = 1;

            // Page Title - HIDDEN (no dashboard text)
            this.lblPageTitle.Text = "";
            this.lblPageTitle.Visible = false;
            this.topBar.Controls.Add(this.lblPageTitle);

            // Search
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(700, 15);
            this.txtSearch.Size = new System.Drawing.Size(350, 25);
            this.txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.txtSearch.Text = "Search patients, doctors...";
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            this.topBar.Controls.Add(this.txtSearch);

            // User
            this.lblLoggedInUser.Text = "👤 Admin";
            this.lblLoggedInUser.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLoggedInUser.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblLoggedInUser.Location = new System.Drawing.Point(1100, 10);
            this.lblLoggedInUser.Size = new System.Drawing.Size(100, 25);
            this.lblLoggedInUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblLoggedInUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.topBar.Controls.Add(this.lblLoggedInUser);

            // DateTime
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblDateTime.Text = DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.lblDateTime.Location = new System.Drawing.Point(1100, 35);
            this.lblDateTime.Size = new System.Drawing.Size(180, 15);
            this.lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.topBar.Controls.Add(this.lblDateTime);

            // Timer for clock
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => this.lblDateTime.Text = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt");
            timer.Start();

            // Footer Panel
            this.footerPanel.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.footerPanel.Height = 30;
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.TabIndex = 2;

            System.Windows.Forms.Label lblFooter = new System.Windows.Forms.Label();
            lblFooter.Text = "© 2024 Clinic Management System | Logged in as: Admin";
            lblFooter.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblFooter.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            lblFooter.Location = new System.Drawing.Point(270, 5);
            lblFooter.Size = new System.Drawing.Size(400, 20);
            lblFooter.Anchor = AnchorStyles.Left;
            this.footerPanel.Controls.Add(lblFooter);

            System.Windows.Forms.Label lblStatus = new System.Windows.Forms.Label();
            lblStatus.Text = "● Online";
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblStatus.ForeColor = System.Drawing.Color.Green;
            lblStatus.Location = new System.Drawing.Point(1200, 5);
            lblStatus.Size = new System.Drawing.Size(70, 20);
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.footerPanel.Controls.Add(lblStatus);

            // Content Panel
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.contentPanel.Location = new System.Drawing.Point(250, 60);
            this.contentPanel.Size = new System.Drawing.Size(1050, 610);
            this.contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.contentPanel.AutoScroll = true;
            this.contentPanel.TabIndex = 3;

            // Add controls to form
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.footerPanel);

            this.sidebarPanel.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}