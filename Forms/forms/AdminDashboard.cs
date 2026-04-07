using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Controls;

namespace ClinicAppointmentSystem
{
    public partial class AdminDashboard : Form
    {
        private bool isSidebarCollapsed = false;
        private int sidebarExpandedWidth = 240;
        private int sidebarCollapsedWidth = 70;

        public AdminDashboard()
        {
            InitializeComponent();
            InitSidebarButtonTags();   // Save original labels before any collapse
            SyncWithDataManager();
            ApplyRolePermissions();
            SetupButtonHoverEffects();
            LoadDashboardHome();
        }

        // Save each sidebar button's full text AND its icon character
        private void InitSidebarButtonTags()
        {
            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnToggleSidebar) {
                    string full = b.Text;
                    // Extract icon: text format is "  ICON  Label"
                    string icon = "";
                    string trimmed = full.TrimStart();
                    if (trimmed.Length >= 1) {
                        // The icon is the first non-space character
                        icon = trimmed.Substring(0, 1);
                    }
                    b.Tag = new string[] { full, icon };
                }
            }
        }

        private void ApplyRolePermissions()
        {
            var user = DataManager.CurrentUser;
            if (user == null) return;

            string role = user.Role;

            // Admin: Full access to admin tools, hide receptionist specific tools for cleaner UI
            if (role == "Admin")
            {
                btnBookAppt.Visible = false;
                btnTodayQueue.Visible = false;
                return;
            }

            // Receptionist: Show receptionist tools, hide Admin and Doctor tools
            if (role == "Receptionist")
            {
                btnBookAppt.Visible = true;
                btnTodayQueue.Visible = true;

                btnDoctors.Visible = false;
                btnUsers.Visible = false;
                btnSettings.Visible = false;
                btnSecurity.Visible = true;
                btnBackup.Visible = false;
                btnAudit.Visible = false;
            }
            // Doctor: Hide almost everything except schedule and dashboard
            else if (role == "Doctor")
            {
                btnBookAppt.Visible = false;
                btnTodayQueue.Visible = false;

                btnDoctors.Visible = false;
                btnUsers.Visible = false;
                btnPatients.Visible = false;
                btnBackup.Visible = false;
                btnAudit.Visible = false;
                
                // Keep btnReports and btnSettings visible but rebrand them
                btnReports.Visible = true;
                btnSettings.Visible = true;
            }
            
            RelayoutSidebar();
        }

        private void RelayoutSidebar()
        {
            int yPos = 90;
            var navButtons = new Button[] {
                btnDashboard, btnUsers, btnDoctors, btnDepartments, btnPatients, 
                btnBookAppt, btnTodayQueue, btnAppointments, btnCalendar, 
                btnReports, btnSettings, btnSecurity, btnBackup, btnAudit 
            };

            var role = DataManager.CurrentUser?.Role;

            foreach(Button b in navButtons)
            {
                bool isVisibleForRole = true;
                if (role == "Admin") {
                    if (b == btnBookAppt || b == btnTodayQueue) isVisibleForRole = false;
                }
                else if (role == "Receptionist") {
                    if (b == btnDoctors || b == btnUsers || b == btnDepartments || b == btnSettings || b == btnBackup || b == btnAudit) isVisibleForRole = false;
                }
                else if (role == "Doctor") {
                    if (b != btnDashboard && b != btnAppointments && b != btnCalendar && b != btnReports && b != btnSettings && b != btnSecurity) isVisibleForRole = false;
                }

                if (isVisibleForRole)
                {
                    b.Location = new Point(5, yPos);
                    yPos += 44; // 42 height + 2 padding
                }
            }
        }

        private void SyncWithDataManager()
        {
            DataManager.EnsureLoaded();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            if (!this.IsDisposed && lblDateTime != null)
                lblDateTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm:ss tt");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N)) { LoadBookingView(); return true; }
            if (keyData == (Keys.Control | Keys.T)) { LoadTodayQueueView(); return true; }
            if (keyData == (Keys.Control | Keys.P)) { LoadPatientsView(); return true; }
            
            // Legacy admin shortcuts
            if (keyData == (Keys.Control | Keys.Shift | Keys.N)) { LoadDoctorsView(); return true; }
            if (keyData == (Keys.Control | Keys.Shift | Keys.U)) { LoadStaffView(); return true; }
            if (keyData == (Keys.Control | Keys.B)) { LoadBackupView(); return true; }
            if (keyData == Keys.F5) { SyncWithDataManager(); LoadDashboardHome(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ShowControl(UserControl uc)
        {
            contentPanel.SuspendLayout();
            
            // Fix memory stacking: explicitly dispose old controls before clearing
            foreach (Control c in contentPanel.Controls) {
                c.Dispose();
            }
            contentPanel.Controls.Clear();
            
            uc.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(uc);
            contentPanel.ResumeLayout();
            
            // Set Page Title if lblPageTitle exists (handled by specific Load methods usually)
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            isSidebarCollapsed = !isSidebarCollapsed;
            
            this.SuspendLayout();
            sidebarPanel.Width = isSidebarCollapsed ? sidebarCollapsedWidth : sidebarExpandedWidth;
            btnToggleSidebar.Text = isSidebarCollapsed ? "\u25B6" : "\u2630";

            // Hide/Show Logo Branding
            if (lblLogo != null) lblLogo.Visible = !isSidebarCollapsed;
            if (lblBadge != null) lblBadge.Visible = !isSidebarCollapsed;

            // Rename Doctor buttons if needed
            if (DataManager.CurrentUser?.Role == "Doctor")
            {
                btnReports.Tag = new string[] { "  \u2605  Ratings & Feedback", "\u2605" };
                btnSettings.Tag = new string[] { "  \u23F2  Availability", "\u23F2" };
            }

            // Update all nav buttons: show icon-only when collapsed, full text when expanded
            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnToggleSidebar) {
                    b.Width = sidebarPanel.Width - 10;
                    if (b.Tag is string[] tags && tags.Length >= 2) {
                        if (isSidebarCollapsed) {
                            b.Text = tags[1]; // Just the icon character
                            b.TextAlign = ContentAlignment.MiddleCenter;
                        } else {
                            b.Text = tags[0]; // Full "  ICON  Label" text
                            b.TextAlign = ContentAlignment.MiddleLeft;
                        }
                    }
                }
            }
            
            this.ResumeLayout();
        }


        private void SidebarButton_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            if (clicked == btnLogout) { Logout_Click(sender, e); return; }

            ResetSidebarButtons();
            clicked.BackColor = Color.FromArgb(59, 130, 246); // Primary Blue Highlight

            if (clicked == btnDashboard) LoadDashboardHome();
            else if (clicked == btnUsers) LoadStaffView();
            else if (clicked == btnDoctors) LoadDoctorsView();
            else if (clicked == btnDepartments) LoadDepartmentsView();
            else if (clicked == btnPatients) LoadPatientsView();
            else if (clicked == btnBookAppt) LoadBookingView();
            else if (clicked == btnTodayQueue) LoadTodayQueueView();
            else if (clicked == btnAppointments) LoadAppointmentsView();
            else if (clicked == btnCalendar) LoadCalendarView();
            else if (clicked == btnReports) LoadReportsView();
            else if (clicked == btnSettings) LoadSettingsView();
            else if (clicked == btnSecurity) ShowControl(new UcSecurityManager());
            else if (clicked == btnBackup) LoadBackupView();
            else if (clicked == btnAudit) LoadAuditView();
        }

        private void ResetSidebarButtons()
        {
            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnLogout && b != btnToggleSidebar) 
                    b.BackColor = Color.Transparent;
            }
        }

        private void SetupButtonHoverEffects()
        {
            // Standardizing colors to Slate/Blue theme
            Color hoverColor = Color.FromArgb(51, 65, 85); // Slate 700
            Color activeColor = Color.FromArgb(59, 130, 246); // Blue 500

            Action<Button> apply = (b) => {
                b.MouseEnter += (s, ev) => { if (b.BackColor != activeColor) b.BackColor = hoverColor; };
                b.MouseLeave += (s, ev) => { if (b.BackColor != activeColor) b.BackColor = Color.Transparent; };
            };

            apply(btnDashboard); apply(btnUsers); apply(btnDoctors); apply(btnPatients);
            apply(btnBookAppt); apply(btnTodayQueue);
            apply(btnAppointments); apply(btnCalendar); apply(btnReports); apply(btnSettings); apply(btnSecurity);
            apply(btnBackup); apply(btnAudit);
        }

        // Module Loading Methods (Shared Suite)
        public void LoadDashboardHome() 
        {
            if (DataManager.CurrentUser?.Role == "Doctor")
            {
                int docId = DataManager.CurrentUser.DoctorId;
                ShowControl(new UcDoctorHome(docId));
            }
            else
            {
                ShowControl(new UcDashboardHome());
            }
        }
        private void LoadStaffView() => ShowControl(new UcStaffManagement());
        private void LoadDoctorsView() => ShowControl(new UcDoctorManagement());
        private void LoadDepartmentsView() => ShowControl(new UcDepartmentManager());
        private void LoadPatientsView() => ShowControl(new UcPatientDirectory());
        private void LoadBookingView() => ShowControl(new UcBookingForm());
        private void LoadTodayQueueView() => ShowControl(new UcAppointmentGrid { IsAdminMode = false });
        private void LoadAppointmentsView()
        {
            if (DataManager.CurrentUser?.Role == "Doctor")
                ShowControl(new UcDoctorAppointments(DataManager.CurrentUser.DoctorId));
            else
                ShowControl(new UcAppointmentGrid { IsAdminMode = DataManager.CurrentUser?.Role == "Admin" });
        }
        private void LoadCalendarView() => ShowControl(new UcCalendarView());
        private void LoadReportsView()
        {
            if (DataManager.CurrentUser?.Role == "Doctor")
                ShowControl(new UcDoctorFeedback(DataManager.CurrentUser.DoctorId));
            else
                ShowControl(new UcClinicalReports());
        }
        private void LoadSettingsView()
        {
            if (DataManager.CurrentUser?.Role == "Doctor")
                ShowControl(new UcDoctorAvailability(DataManager.CurrentUser.DoctorId));
            else
                ShowControl(new UcSystemSettings());
        }
        private void LoadBackupView() => ShowControl(new UcBackupRestore());
        private void LoadAuditView() => ShowControl(new UcAuditLog());

        public void OpenConsultation(int apptId)
        {
            ShowControl(new Controls.UcConsultationPanel(apptId));
        }

        public void LoadTodayPatients()
        {
            if (DataManager.CurrentUser?.Role == "Doctor")
                ShowControl(new Controls.UcDoctorQueue(DataManager.CurrentUser.DoctorId));
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Finalize administrative logout?", "Security Compliance", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.BeginInvoke(new Action(() => Program.MainForm.ForceLogout(false)));
            }
        }
    }
}
