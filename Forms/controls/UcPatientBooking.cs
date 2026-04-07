using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientBooking : UserControl
    {
        private int _patientId;
        
        // Selection State
        private string _selectedSpec = "All Specialties";
        private int _selectedDoctorId = 0;
        private DateTime _selectedDate = DateTime.Today.AddDays(1);
        private DateTime _selectedTime = DateTime.MinValue;
        private string _selectedShift = "All"; 

        // UI Components
        private FlowLayoutPanel pnlSpecChips;
        private FlowLayoutPanel pnlDoctorGrid;
        private TextBox txtSearch;
        private Panel pnlDateTimePanel; // Sidebar for time selection
        private MonthCalendar calDate;
        private FlowLayoutPanel pnlShifts;
        private FlowLayoutPanel pnlSlots;
        private Button btnConfirm;
        private Label lblSelectedDoctorBrief;

        public UcPatientBooking(int patientId)
        {
            this._patientId = patientId;
            this.Dock = DockStyle.Fill;
            this.BackColor = PatientTheme.Background;
            InitializeAdvancedUI();
        }

        private void InitializeAdvancedUI()
        {
            this.Controls.Clear();
            
            // ── HEADER ──────────────────────────────────────────────────
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = PatientTheme.Surface, Padding = new Padding(40, 25, 40, 0) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            
            Label lblTitle = new Label { Text = "SCHEDULE A VISIT", Font = PatientTheme.TitleMedium, ForeColor = PatientTheme.TextPrimary, AutoSize = true, Location = new Point(40, 25) };
            Label lblSub = new Label { Text = "Choose your preferred specialty, doctor, and time slot.", Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, AutoSize = true, Location = new Point(41, 60) };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });
            this.Controls.Add(pnlHeader);

            // ── MAIN BODY (FLOW) ────────────────────────────────────────
            Panel pnlMain = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 40, 40) };
            this.Controls.Add(pnlMain);
            pnlMain.BringToFront();

            // 1. SELECT SPECIALTY (TOP SECTION)
            AddStepLabel(pnlMain, "1. SELECT SPECIALTY", new Point(40, 10));
            pnlSpecChips = new FlowLayoutPanel { Location = new Point(40, 40), Size = new Size(1150, 60), WrapContents = false, AutoScroll = true };
            PopulateSpecialtyChips();
            pnlMain.Controls.Add(pnlSpecChips);

            // 2. SEARCH & DOCTOR LIST
            AddStepLabel(pnlMain, "2. CHOOSE YOUR DOCTOR", new Point(40, 115));
            txtSearch = new TextBox { 
                Location = new Point(40, 150), 
                Width = 400, 
                Height = 40, 
                Font = PatientTheme.BodyRegular, 
                Text = "Search doctor name...", 
                ForeColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle 
            };
            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text == "Search doctor name...") {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = PatientTheme.TextPrimary;
                }
            };
            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) {
                    txtSearch.Text = "Search doctor name...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, e) => {
                if (txtSearch.Text != "Search doctor name...") UpdateDoctorGrid();
            };
            pnlMain.Controls.Add(txtSearch);

            pnlDoctorGrid = new FlowLayoutPanel { Location = new Point(40, 205), Size = new Size(1150, 500), AutoScroll = true, Padding = new Padding(0, 0, 20, 0) };
            pnlMain.Controls.Add(pnlDoctorGrid);

            // ── DATE/TIME SIDE PANEL (INITIALY HIDDEN) ──────────────────
            pnlDateTimePanel = new Panel { Dock = DockStyle.Right, Width = 450, BackColor = PatientTheme.Surface, Visible = false };
            pnlDateTimePanel.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, 0, 0, pnlDateTimePanel.Height);
            this.Controls.Add(pnlDateTimePanel);
            pnlDateTimePanel.BringToFront();

            // Setup Sidebar Components
            Button btnCloseSidebar = new Button { Text = "\u2715", Size = new Size(40, 40), Location = new Point(390, 20), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnCloseSidebar.FlatAppearance.BorderSize = 0;
            btnCloseSidebar.Click += (s, e) => pnlDateTimePanel.Visible = false;
            pnlDateTimePanel.Controls.Add(btnCloseSidebar);

            lblSelectedDoctorBrief = new Label { Text = "Doctor Name", Font = PatientTheme.TitleMedium, ForeColor = PatientTheme.TextPrimary, Location = new Point(35, 30), AutoSize = true };
            pnlDateTimePanel.Controls.Add(lblSelectedDoctorBrief);

            Label l3 = new Label { Text = "3. PREFERRED DATE", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.Primary, Location = new Point(35, 85), AutoSize = true };
            pnlDateTimePanel.Controls.Add(l3);

            calDate = new MonthCalendar { Location = new Point(35, 115), MaxSelectionCount = 1, MinDate = DateTime.Today.AddDays(1) };
            calDate.DateSelected += (s, e) => { _selectedDate = e.Start; RefreshSlots(); };
            pnlDateTimePanel.Controls.Add(calDate);

            pnlShifts = new FlowLayoutPanel { Location = new Point(35, 300), Size = new Size(380, 45), WrapContents = false };
            AddShiftButton("ALL", "All");
            AddShiftButton("MORNING", "Morning");
            AddShiftButton("AFTERNOON", "Afternoon");
            pnlDateTimePanel.Controls.Add(pnlShifts);

            pnlSlots = new FlowLayoutPanel { Location = new Point(35, 360), Size = new Size(380, 300), AutoScroll = true };
            pnlDateTimePanel.Controls.Add(pnlSlots);

            btnConfirm = new Button { 
                Text = "CONFIRM APPOINTMENT", 
                Dock = DockStyle.Bottom, 
                Height = 80, 
                BackColor = PatientTheme.TextMuted, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                Enabled = false 
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;
            pnlDateTimePanel.Controls.Add(btnConfirm);

            UpdateDoctorGrid();
        }

        private void AddStepLabel(Control p, string t, Point loc) => p.Controls.Add(new Label { Text = t, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = PatientTheme.Primary, Location = loc, AutoSize = true });

        private void PopulateSpecialtyChips()
        {
            pnlSpecChips.Controls.Clear();
            var specs = new List<string> { "All Specialties" };
            specs.AddRange(DataManager.Departments);
            
            foreach (var s in specs) {
                bool isSel = (_selectedSpec == s);
                Button btn = new Button { 
                    Text = s.ToUpper(), 
                    AutoSize = true, 
                    Padding = new Padding(15, 8, 15, 8), 
                    Margin = new Padding(0, 0, 15, 0),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = isSel ? PatientTheme.Primary : Color.FromArgb(241, 245, 249),
                    ForeColor = isSel ? Color.White : PatientTheme.TextSecondary,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (sender, x) => { _selectedSpec = s; PopulateSpecialtyChips(); UpdateDoctorGrid(); };
                pnlSpecChips.Controls.Add(btn);
            }
        }

        private void UpdateDoctorGrid()
        {
            pnlDoctorGrid.Controls.Clear();
            var doctors = DataManager.Doctors.Where(d => d.IsActive && !d.IsOnLeave);
            if (_selectedSpec != "All Specialties") doctors = doctors.Where(d => d.Department == _selectedSpec);
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text != "Search doctor name...") 
                doctors = doctors.Where(d => d.FullName.ToLower().Contains(txtSearch.Text.ToLower()));

            foreach (var doc in doctors) {
                pnlDoctorGrid.Controls.Add(CreateDoctorCard(doc));
            }
        }

        private Panel CreateDoctorCard(Doctor d)
        {
            Panel card = new Panel { Size = new Size(350, 160), BackColor = Color.White, Margin = new Padding(0, 0, 30, 30), Cursor = Cursors.Hand };
            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen p = new Pen(PatientTheme.Border, 1)) {
                    GraphicsPath path = GetRoundedPath(card.ClientRectangle, 10);
                    e.Graphics.DrawPath(p, path);
                }
            };

            // Availability status calculation
            var todaySlots = DataManager.GetAvailableTimeSlots(d.Id, DateTime.Today);
            bool isAvailableToday = todaySlots.Count > 0;
            
            Panel pnlStatus = new Panel { Size = new Size(10, 10), Location = new Point(25, 25), BackColor = isAvailableToday ? PatientTheme.Success : PatientTheme.TextMuted };
            pnlStatus.Paint += (s, e) => e.Graphics.FillEllipse(new SolidBrush(pnlStatus.BackColor), 0, 0, 10, 10);
            
            Label lStatus = new Label { Text = isAvailableToday ? "Available Today" : "Next Available: Tomorrow", Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = isAvailableToday ? PatientTheme.Success : PatientTheme.TextSecondary, Location = new Point(40, 23), AutoSize = true };
            Label lName = new Label { Text = "DR. " + d.FullName.ToUpper(), Font = new Font("Segoe UI Semibold", 12), ForeColor = PatientTheme.TextPrimary, Location = new Point(23, 45), AutoSize = true };
            Label lSpec = new Label { Text = d.Specialty, Font = PatientTheme.Subtitle, ForeColor = PatientTheme.Primary, Location = new Point(25, 75), AutoSize = true };
            Label lDays = new Label { Text = "WORKING DAYS: " + d.WorkingDays, Font = new Font("Segoe UI", 8), ForeColor = PatientTheme.TextSecondary, Location = new Point(25, 110), AutoSize = true };

            card.Controls.AddRange(new Control[] { pnlStatus, lStatus, lName, lSpec, lDays });
            
            EventHandler onSelect = (s, e) => {
                _selectedDoctorId = d.Id;
                lblSelectedDoctorBrief.Text = "DR. " + d.FullName.ToUpper();
                pnlDateTimePanel.Visible = true;
                RefreshSlots();
            };
            
            foreach (Control c in card.Controls) c.Click += onSelect;
            card.Click += onSelect;

            return card;
        }

        private GraphicsPath GetRoundedPath(Rectangle r, int rad)
        {
            GraphicsPath p = new GraphicsPath();
            float d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void AddShiftButton(string label, string value)
        {
            Button btn = new Button { 
                Text = label, 
                Size = new Size(110, 38), 
                Margin = new Padding(0, 0, 12, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = (_selectedShift == value) ? PatientTheme.TextPrimary : PatientTheme.Surface,
                ForeColor = (_selectedShift == value) ? Color.White : PatientTheme.TextMuted,
                Font = PatientTheme.LabelBold,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = PatientTheme.Border;
            btn.Click += (s, e) => { _selectedShift = value; RefreshSlots(); };
            pnlShifts.Controls.Add(btn);
        }

        private void RefreshSlots()
        {
            pnlSlots.Controls.Clear();
            _selectedTime = DateTime.MinValue;
            btnConfirm.Enabled = false;
            btnConfirm.BackColor = PatientTheme.TextMuted;
            
            foreach (Control c in pnlShifts.Controls) {
                if (c is Button b) {
                    bool isActive = (c.Text == _selectedShift.ToUpper()) || (c.Text == "ALL" && _selectedShift == "All");
                    b.BackColor = isActive ? PatientTheme.TextPrimary : PatientTheme.Surface;
                    b.ForeColor = isActive ? Color.White : PatientTheme.TextMuted;
                }
            }

            if (_selectedDoctorId == 0) return;
            var slots = DataManager.GetAvailableTimeSlots(_selectedDoctorId, _selectedDate);
            if (_selectedShift != "All") slots = slots.Where(s => GetTimeShift(s.TimeOfDay) == _selectedShift).ToList();

            if (!slots.Any()) {
                pnlSlots.Controls.Add(new Label { Text = "No slots available on this date.", Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = PatientTheme.Danger, AutoSize = true });
                return;
            }

            foreach (var slot in slots) {
                Button btn = new Button { 
                    Text = slot.ToString("hh:mm tt"), Size = new Size(160, 50), Margin = new Padding(0, 0, 15, 15),
                    FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(248, 250, 252), ForeColor = PatientTheme.TextPrimary,
                    Font = PatientTheme.LabelBold, Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = PatientTheme.Border;
                btn.Click += (s, e) => {
                    _selectedTime = slot;
                    foreach (Control c in pnlSlots.Controls) { c.BackColor = Color.FromArgb(248, 250, 252); c.ForeColor = PatientTheme.TextPrimary; }
                    btn.BackColor = PatientTheme.Primary;
                    btn.ForeColor = Color.White;
                    btnConfirm.Enabled = true;
                    btnConfirm.BackColor = PatientTheme.Success;
                };
                pnlSlots.Controls.Add(btn);
            }
        }

        private string GetTimeShift(TimeSpan t) => t.Hours < 12 ? "Morning" : (t.Hours < 17 ? "Afternoon" : "Evening");

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _selectedDoctorId);
            string msg = $"Confirm appointment with Dr. {doc?.FullName} on {_selectedDate:D} at {_selectedTime:hh:mm tt}?";
            if (MessageBox.Show(msg, "Confirm Visit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                DataManager.AddAppointment(new Appointment { PatientId = _patientId, DoctorId = _selectedDoctorId, AppointmentDate = _selectedTime, Status = "Scheduled" });
                MessageBox.Show("Appointment successfully booked!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.ParentForm is PatientDashboard dash) dash.LoadHome();
            }
        }
    }
}
