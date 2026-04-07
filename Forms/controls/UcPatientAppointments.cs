using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using ClinicAppointmentSystem;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientAppointments : UserControl
    {
        private int _patientId;
        private string _viewType = "Upcoming"; // Default view
        private string _currentSearch = "";
        private string _currentSort = "Date (Newest)";

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        
        // Modern UI Components
        private Panel pnlHeader;
        private Panel pnlTabContainer;
        private Panel pnlEmptyState;
        private FlowLayoutPanel flpCards;
        private Label lblUpcomingCount;
        private Button btnTabUpcoming, btnTabPast, btnTabCancelled;
        private ComboBox cmbSort;

        public UcPatientAppointments(int patientId, string viewType)
        {
            this._patientId = patientId;
            this._viewType = string.IsNullOrEmpty(viewType) ? "Upcoming" : viewType;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = PatientTheme.Background;
            
            SetupModernUI();
            RefreshData();
        }

        private void SetupModernUI()
        {
            this.Controls.Clear();

            // 1. Header Section
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = PatientTheme.Surface, Padding = new Padding(40, 25, 40, 0) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            
            Label lblTitle = new Label { Text = "MY APPOINTMENTS", Font = PatientTheme.TitleMedium, ForeColor = PatientTheme.TextPrimary, Location = new Point(40, 25), AutoSize = true };
            
            // Stats Mini Card in Header
            Panel pnlStats = new Panel { Size = new Size(200, 65), Location = new Point(40, 65), BackColor = PatientTheme.PrimaryLight };
            pnlStats.Paint += (s, e) => {
                using (Pen p = new Pen(PatientTheme.Primary, 1)) e.Graphics.DrawRectangle(p, 0, 0, pnlStats.Width - 1, pnlStats.Height - 1);
            };
            Label lblStatTitle = new Label { Text = "UPCOMING VISITS", Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = PatientTheme.Primary, Location = new Point(12, 12), AutoSize = true };
            lblUpcomingCount = new Label { Text = "0", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = PatientTheme.Primary, Location = new Point(10, 28), AutoSize = true };
            pnlStats.Controls.AddRange(new Control[] { lblStatTitle, lblUpcomingCount });
            
            // Search & Sort Bar
            txtSearch = new TextBox { Width = 250, Location = new Point(pnlHeader.Width - 550, 75), Font = PatientTheme.BodyRegular, BorderStyle = BorderStyle.FixedSingle, BackColor = PatientTheme.Background };
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.TextChanged += (s, e) => { _currentSearch = txtSearch.Text.ToLower(); RefreshData(); };
            try { SendMessage(txtSearch.Handle, EM_SETCUEBANNER, 0, "Search by doctor or specialty..."); } catch { }

            cmbSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, Location = new Point(pnlHeader.Width - 280, 75), Font = PatientTheme.BodyRegular, FlatStyle = FlatStyle.Flat };
            cmbSort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbSort.Items.AddRange(new object[] { "Date (Newest)", "Date (Oldest)", "Physician Name" });
            cmbSort.SelectedItem = _currentSort;
            cmbSort.SelectedIndexChanged += (s, e) => { _currentSort = cmbSort.SelectedItem.ToString(); RefreshData(); };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, pnlStats, txtSearch, cmbSort });
            this.Controls.Add(pnlHeader);

            // 2. Tab Navigation
            pnlTabContainer = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = PatientTheme.Surface, Padding = new Padding(40, 0, 40, 0) };
            
            btnTabUpcoming = CreateTabButton("UPCOMING", _viewType == "Upcoming");
            btnTabUpcoming.Location = new Point(40, 10);
            btnTabUpcoming.Click += (s, e) => { _viewType = "Upcoming"; UpdateTabs(); RefreshData(); };

            btnTabPast = CreateTabButton("PAST VISITS", _viewType == "Past");
            btnTabPast.Location = new Point(160, 10);
            btnTabPast.Click += (s, e) => { _viewType = "Past"; UpdateTabs(); RefreshData(); };

            btnTabCancelled = CreateTabButton("CANCELLED", _viewType == "Cancelled");
            btnTabCancelled.Location = new Point(290, 10);
            btnTabCancelled.Click += (s, e) => { _viewType = "Cancelled"; UpdateTabs(); RefreshData(); };

            Button btnBook = new Button { 
                Text = "+ BOOK NEW VISIT", 
                Size = new Size(200, 40), 
                Location = new Point(pnlTabContainer.Width - 240, 10), 
                BackColor = PatientTheme.Success, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = PatientTheme.LabelBold, 
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnBook.FlatAppearance.BorderSize = 0;
            btnBook.Click += (s, e) => (this.FindForm() as PatientDashboard)?.LoadBooking();

            pnlTabContainer.Controls.AddRange(new Control[] { btnTabUpcoming, btnTabPast, btnTabCancelled, btnBook });
            this.Controls.Add(pnlTabContainer);

            // 3. Main Card Flow
            flpCards = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(40, 30, 40, 40),
                BackColor = PatientTheme.Background,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpCards);
            flpCards.BringToFront();

            // 4. Empty State
            pnlEmptyState = new Panel { Dock = DockStyle.Fill, BackColor = PatientTheme.Background, Visible = false };
            Label lblEmpty = new Label { Text = "No appointments found.", Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextMuted, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            pnlEmptyState.Controls.Add(lblEmpty);
            this.Controls.Add(pnlEmptyState);
        }

        private Button CreateTabButton(string text, bool isActive)
        {
            Button btn = new Button {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 40),
                Font = PatientTheme.LabelBold,
                ForeColor = isActive ? PatientTheme.Primary : PatientTheme.TextMuted,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            return btn;
        }

        private void UpdateTabs()
        {
            btnTabUpcoming.ForeColor = _viewType == "Upcoming" ? PatientTheme.Primary : PatientTheme.TextMuted;
            btnTabPast.ForeColor = _viewType == "Past" ? PatientTheme.Primary : PatientTheme.TextMuted;
            btnTabCancelled.ForeColor = _viewType == "Cancelled" ? PatientTheme.Primary : PatientTheme.TextMuted;
        }

        public void RefreshData()
        {
            if (flpCards != null) flpCards.Controls.Clear();
            
            var all = DataManager.Appointments.Where(a => a.PatientId == _patientId).ToList();
            
            // Update Stats
            int upCount = all.Count(a => a.AppointmentDate >= DateTime.Now && a.Status == "Scheduled");
            if (lblUpcomingCount != null) lblUpcomingCount.Text = upCount.ToString();

            List<Appointment> filtered;
            if (_viewType == "Upcoming")
                filtered = all.Where(a => a.AppointmentDate >= DateTime.Now && a.Status != "Completed" && a.Status != "Cancelled").OrderBy(a => a.AppointmentDate).ToList();
            else if (_viewType == "Cancelled")
                filtered = all.Where(a => a.Status == "Cancelled").OrderByDescending(a => a.AppointmentDate).ToList();
            else // Past
                filtered = all.Where(a => a.AppointmentDate < DateTime.Now || a.Status == "Completed").Where(a => a.Status != "Cancelled").OrderByDescending(a => a.AppointmentDate).ToList();

            if (!string.IsNullOrEmpty(_currentSearch))
            {
                filtered = filtered.Where(a => {
                    var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                    return (doc?.FullName?.ToLower().Contains(_currentSearch) ?? false) || 
                           (doc?.Specialty?.ToLower().Contains(_currentSearch) ?? false);
                }).ToList();
            }

            // Sorting
            if (_currentSort == "Date (Newest)") filtered = filtered.OrderByDescending(a => a.AppointmentDate).ToList();
            else if (_currentSort == "Date (Oldest)") filtered = filtered.OrderBy(a => a.AppointmentDate).ToList();
            else if (_currentSort == "Physician Name") filtered = filtered.OrderBy(a => DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName).ToList();

            pnlEmptyState.Visible = (filtered.Count == 0);
            if (flpCards != null) foreach (var a in filtered) flpCards.Controls.Add(CreateAppointmentCard(a));
        }

        private Panel CreateAppointmentCard(Appointment a)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            Panel p = new Panel { Size = new Size(950, 130), BackColor = PatientTheme.Surface, Margin = new Padding(0, 0, 0, 20) };
            p.Paint += (s, e) => {
                using (Pen bp = new Pen(PatientTheme.Border, 1)) e.Graphics.DrawRectangle(bp, 0, 0, p.Width - 1, p.Height - 1);
            };
            
            // Date Block
            Label lDate = new Label { Text = a.AppointmentDate.ToString("MMM dd, yyyy"), Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(30, 25), AutoSize = true };
            Label lTime = new Label { Text = a.AppointmentDate.ToString("hh:mm tt"), Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(30, 65), AutoSize = true };
            
            // Doctor Block
            Panel pLine = new Panel { Size = new Size(1, 70), BackColor = PatientTheme.Border, Location = new Point(220, 30) };
            Label lDoc = new Label { Text = $"Dr. {doc?.FullName ?? "Physician"}", Font = new Font("Segoe UI", 13, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(250, 30), AutoSize = true };
            Label lSpec = new Label { Text = $"{doc?.Specialty ?? "General"} | {a.AppointmentType ?? "Regular"}", Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(250, 62), AutoSize = true };
            
            // Status Badge
            Color sb, sf;
            string ss = a.Status.ToUpper();
            if (ss == "COMPLETED") { sb = PatientTheme.SuccessLight; sf = PatientTheme.Success; }
            else if (ss == "CANCELLED") { sb = PatientTheme.DangerLight; sf = PatientTheme.Danger; }
            else { sb = PatientTheme.PrimaryLight; sf = PatientTheme.Primary; }

            Label lStatus = new Label { 
                Text = " " + ss + " ",
                Font = PatientTheme.LabelBold,
                ForeColor = sf,
                BackColor = sb,
                Location = new Point(600, 50),
                AutoSize = true,
                Padding = new Padding(10, 5, 10, 5)
            };

            // Action Button
            Button btnAction = new Button { Size = new Size(180, 50), Location = new Point(740, 40), BackColor = PatientTheme.Background, ForeColor = PatientTheme.TextSecondary, FlatStyle = FlatStyle.Flat, Font = PatientTheme.LabelBold, Cursor = Cursors.Hand };
            btnAction.FlatAppearance.BorderSize = 0;
            
            if (a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now)
            {
                btnAction.Text = "CANCEL VISIT";
                btnAction.BackColor = PatientTheme.DangerLight;
                btnAction.ForeColor = PatientTheme.Danger;
                btnAction.Click += (s, e) => {
                    if (MessageBox.Show("Are you sure you want to cancel this visit?", "Cancel Appointment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                        a.Status = "Cancelled";
                        DataManager.SaveAppointments();
                        RefreshData();
                    }
                };
            }
            else if (a.Status == "Completed" && a.PatientRating == 0)
            {
                btnAction.Text = "★ RATE SERVICE";
                btnAction.BackColor = Color.FromArgb(254, 243, 199); // Amber 100
                btnAction.ForeColor = Color.FromArgb(146, 64, 14); // Amber 800
                btnAction.Click += (s, e) => ShowRatingDialog(a);
            }
            else
            {
                btnAction.Text = "VIEW DETAILS";
                btnAction.Click += (s, e) => new AppointmentDetailForm(a.Id).ShowDialog();
            }

            p.Controls.AddRange(new Control[] { lDate, lTime, pLine, lDoc, lSpec, lStatus, btnAction });
            return p;
        }

        private void ShowRatingDialog(Appointment a)
        {
            using (var rateForm = new RateDoctorForm(a))
                if (rateForm.ShowDialog() == DialogResult.OK) RefreshData();
        }
    }
}
