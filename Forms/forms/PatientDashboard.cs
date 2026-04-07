using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Controls;
using ClinicAppointmentSystem.Utils;
using System.Collections.Generic;

namespace ClinicAppointmentSystem
{
    public class PatientDashboard : Form
    {
        private int _patientId;
        private Patient _currentPatient;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel pnlActiveIndicator;
        
        // Flexibility Components
        private bool isSidebarCollapsed = false;
        private const int ExpandedWidth = 260;
        private const int CollapsedWidth = 72;
        private Timer sidebarTimer;
        private Button btnToggleSidebar;

        public PatientDashboard(int patientId)
        {
            this._patientId = patientId;
            this._currentPatient = DataManager.Patients.FirstOrDefault(p => p.Id == patientId);
            
            sidebarTimer = new Timer { Interval = 8 }; // Faster interval for smoothness
            sidebarTimer.Tick += SidebarTimer_Tick;

            InitializeUI();
            
            Timer refreshTimer = new Timer { Interval = 60000 };
            refreshTimer.Tick += (s, e) => RefreshCurrentlyViewedControl();
            refreshTimer.Start();

            LoadHome();
        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            int step = 20; 
            if (isSidebarCollapsed)
            {
                if (sidebarPanel.Width > CollapsedWidth) {
                    sidebarPanel.Width -= step;
                    UpdateSidebarUI();
                }
                else { 
                    sidebarPanel.Width = CollapsedWidth; 
                    sidebarTimer.Stop(); 
                    UpdateSidebarUI(); 
                }
            }
            else
            {
                if (sidebarPanel.Width < ExpandedWidth) {
                    sidebarPanel.Width += step;
                    UpdateSidebarUI();
                }
                else { 
                    sidebarPanel.Width = ExpandedWidth; 
                    sidebarTimer.Stop(); 
                    UpdateSidebarUI(); 
                }
            }
        }

        private void UpdateSidebarUI()
        {
            Panel pnlNav = sidebarPanel.Controls.OfType<Panel>().FirstOrDefault();
            if (pnlNav == null) return;

            foreach (Control c in pnlNav.Controls)
            {
                if (c is Button btn)
                {
                    string[] tags = btn.Tag as string[];
                    if (tags == null) continue;

                    string fullText = tags[0];
                    string icon = tags[1];

                    if (sidebarPanel.Width < 150) {
                        btn.Text = icon;
                        btn.TextAlign = ContentAlignment.MiddleCenter;
                        btn.Padding = new Padding(0);
                        btn.Font = new Font("Segoe UI", 14);
                    } else {
                        btn.Text = $"   {icon}   {fullText}";
                        btn.TextAlign = ContentAlignment.MiddleLeft;
                        btn.Padding = new Padding(20, 0, 0, 0);
                        btn.Font = new Font("Segoe UI Semibold", 10);
                    }
                }
            }
        }

        private void RefreshCurrentlyViewedControl()
        {
            if (contentPanel.Controls.Count > 0)
            {
                var ctrl = contentPanel.Controls[0];
                if (ctrl is UcPatientHome home) home.RefreshData();
                else if (ctrl is UcPatientAppointments apps) apps.RefreshData();
                else if (ctrl is UcMedicalHistory hist) hist.RefreshData();
                else if (ctrl is UcPatientRateVisits rate) rate.RefreshData();
            }
            UpdateSidebarBadges();
        }

        private void InitializeUI()
        {
            this.Text = "ALPHA CLINIC | Patient Portal";
            this.ClientSize = new Size(1300, 850);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimizeBox = true;
            this.MaximizeBox = true;
            this.BackColor = PatientTheme.Background;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = PatientTheme.BodyRegular;

            // ── TOP BAR ────────────────────────────────────────────────
            Panel topBar = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            topBar.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
            this.Controls.Add(topBar);

            btnToggleSidebar = new Button {
                Text = "\u2261", 
                Size = new Size(50, 50),
                Location = new Point(12, 12),
                FlatStyle = FlatStyle.Flat,
                ForeColor = PatientTheme.Primary,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.Click += (s, e) => { isSidebarCollapsed = !isSidebarCollapsed; sidebarTimer.Start(); };
            topBar.Controls.Add(btnToggleSidebar);

            Label lblLogo = new Label { 
                Text = "ALPHA CLINIC", 
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold), 
                ForeColor = PatientTheme.Primary, 
                Location = new Point(75, 22), 
                AutoSize = true 
            };
            topBar.Controls.Add(lblLogo);

            Panel pnlBadge = new Panel { Size = new Size(280, 45), Location = new Point(topBar.Width - 420, 15), BackColor = PatientTheme.Background };
            pnlBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            Label lblIdentity = new Label { 
                Text = $"{_currentPatient?.FullName?.ToUpper() ?? "PATIENT"} | {_currentPatient?.PatientCode ?? "P-0000"}",
                Font = PatientTheme.LabelBold,
                ForeColor = PatientTheme.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBadge.Controls.Add(lblIdentity);
            topBar.Controls.Add(pnlBadge);

            Button btnLogout = new Button {
                Text = "LOGOUT",
                Size = new Size(100, 35),
                Location = new Point(topBar.Width - 130, 20),
                BackColor = PatientTheme.DangerLight,
                ForeColor = PatientTheme.Danger,
                FlatStyle = FlatStyle.Flat,
                Font = PatientTheme.LabelBold,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => Program.MainForm.ForceLogout(false);
            topBar.Controls.Add(btnLogout);

            // ── SIDEBAR ────────────────────────────────────────────────
            sidebarPanel = new Panel { Dock = DockStyle.Left, Width = ExpandedWidth, BackColor = PatientTheme.SidebarBg, MinimumSize = new Size(CollapsedWidth, 0) };
            this.Controls.Add(sidebarPanel);

            // Resizable Splitter
            Splitter split = new Splitter { Dock = DockStyle.Left, Width = 3, BackColor = PatientTheme.Border };
            this.Controls.Add(split);

            pnlActiveIndicator = new Panel { Size = new Size(6, 55), BackColor = Color.White, Visible = false };
            sidebarPanel.Controls.Add(pnlActiveIndicator);

            // Container for buttons to manage docking order
            Panel pnlNav = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            sidebarPanel.Controls.Add(pnlNav);

            AddSidebarButton(pnlNav, "PROFILE", "\uD83D\uDC64", (s, e) => LoadProfile());
            AddSidebarButton(pnlNav, "RECORDS", "\uD83D\uDCC4", (s, e) => LoadHistory());
            AddSidebarButton(pnlNav, "BOOK VISIT", "\u2728", (s, e) => LoadBooking());
            AddSidebarButton(pnlNav, "FEEDBACK", "\u2B50", (s, e) => LoadRateVisits());
            AddSidebarButton(pnlNav, "APPOINTMENTS", "\uD83D\uDCC5", (s, e) => LoadUpcoming());
            AddSidebarButton(pnlNav, "DASHBOARD", "\uD83C\uDFE0", (s, e) => LoadHome());

            this.Resize += (s, e) => {
                if (this.Width < 1100 && !isSidebarCollapsed) { isSidebarCollapsed = true; sidebarTimer.Start(); }
            };

            UpdateSidebarBadges();

            // ── CONTENT AREA ───────────────────────────────────────────
            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = PatientTheme.Background, AutoScroll = true };
            this.Controls.Add(contentPanel);
            contentPanel.BringToFront();
        }

        private void AddSidebarButton(Panel container, string text, string icon, EventHandler onClick)
        {
            Button btn = new Button {
                Text = $"   {icon}   {text}",
                Dock = DockStyle.Top,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Padding = new Padding(20, 0, 0, 0),
                Tag = new string[] { text, icon }
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = PatientTheme.Primary;
            btn.FlatAppearance.MouseOverBackColor = PatientTheme.SidebarHover;
            
            btn.Click += (s, e) => {
                // RESET ALL BUTTONS
                foreach (Control c in container.Controls) {
                    if (c is Button b) {
                        b.BackColor = Color.Transparent;
                        b.ForeColor = Color.FromArgb(148, 163, 184);
                    }
                }
                // HIGHLIGHT ACTIVE
                btn.BackColor = PatientTheme.Primary;
                btn.ForeColor = Color.White;
                
                pnlActiveIndicator.Location = new Point(0, sidebarPanel.PointToClient(btn.Parent.PointToScreen(btn.Location)).Y);
                pnlActiveIndicator.Height = btn.Height;
                pnlActiveIndicator.Visible = true;
                pnlActiveIndicator.BringToFront();
                onClick(s, e);
            };
            container.Controls.Add(btn);
        }

        private void UpdateSidebarBadges()
        {
            var pnlNav = sidebarPanel.Controls.OfType<Panel>().FirstOrDefault();
            if (pnlNav == null) return;

            var rateBtn = pnlNav.Controls.OfType<Button>().FirstOrDefault(b => (b.Tag as string[])?[0] == "FEEDBACK");
            if (rateBtn != null)
            {
                int unrated = DataManager.Appointments.Count(a => a.PatientId == _patientId && a.Status == "Completed" && a.PatientRating == 0);
                if (unrated > 0) rateBtn.ForeColor = PatientTheme.Amber;
            }
        }

        public void LoadHome() => ShowControl(new UcPatientHome(_patientId));
        public void LoadUpcoming() => ShowControl(new UcPatientAppointments(_patientId, "Upcoming"));
        public void LoadRateVisits() => ShowControl(new UcPatientRateVisits(_patientId));
        public void LoadHistory() => ShowControl(new UcMedicalHistory(_patientId));
        public void LoadProfile() => ShowControl(new UcPatientProfile(_patientId));
        public void LoadBooking() => ShowControl(new UcPatientBooking(_patientId));

        public void ShowControl(Control uc)
        {
            contentPanel.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(uc);
        }
    }
}
