using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientHome : UserControl
    {
        private int _patientId;
        private Patient _patient;
        private FlowLayoutPanel flpMain;
        
        public UcPatientHome(int patientId)
        {
            this._patientId = patientId;
            this._patient = DataManager.Patients.FirstOrDefault(p => p.Id == patientId);
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = PatientTheme.Background;
            BuildModernUI();
            RefreshData();
        }

        private void BuildModernUI()
        {
            this.Controls.Clear();
            
            flpMain = new FlowLayoutPanel { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                Padding = new Padding(40, 30, 40, 40),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpMain);

            // ── HEADER ──
            Panel pnlHeader = new Panel { Size = new Size(1000, 110), Margin = new Padding(0, 0, 0, 30) };
            Label lWelcome = new Label { Text = $"Welcome, {(_patient?.FullName?.Split(' ').FirstOrDefault() ?? "Patient")}", Font = PatientTheme.TitleLarge, ForeColor = PatientTheme.TextPrimary, Location = new Point(-2, 10), AutoSize = true };
            Label lSubtitle = new Label { Text = "Here is your clinical summary and upcoming schedule.", Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(0, 55), AutoSize = true };
            Label lStatus = new Label { Text = "PREMIUM MEMBER", Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = Color.White, BackColor = PatientTheme.Primary, Padding = new Padding(12, 6, 12, 6), Location = new Point(0, 85), AutoSize = true };
            pnlHeader.Controls.AddRange(new Control[] { lWelcome, lSubtitle, lStatus });
            flpMain.Controls.Add(pnlHeader);

            // ── HERO & ANALYTICS ──
            TableLayoutPanel tlpGrid = new TableLayoutPanel { Size = new Size(1000, 260), ColumnCount = 2, Margin = new Padding(0, 0, 0, 40) };
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            
            Panel pnlHero = new Panel { Name = "pnlHero", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 20, 0) };
            pnlHero.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = PatientTheme.GetBrandGradient(pnlHero.ClientRectangle)) {
                    GraphicsPath path = GetRoundedPath(pnlHero.ClientRectangle, 12);
                    e.Graphics.FillPath(brush, path);
                }
            };
            tlpGrid.Controls.Add(pnlHero, 0, 0);

            FlowLayoutPanel flpStats = new FlowLayoutPanel { Name = "flpStats", Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            tlpGrid.Controls.Add(flpStats, 1, 0);
            flpMain.Controls.Add(tlpGrid);

            // ── QUICK ACTIONS ──
            Label lActionTitle = new Label { Text = "QUICK LINKS", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.TextMuted, Margin = new Padding(0, 10, 0, 15), AutoSize = true };
            flpMain.Controls.Add(lActionTitle);

            FlowLayoutPanel flpActions = new FlowLayoutPanel { Size = new Size(1000, 90), Margin = new Padding(0, 0, 0, 40) };
            flpActions.Controls.Add(CreateActionCard("Book Appointment", PatientTheme.Primary, (s, e) => (this.FindForm() as PatientDashboard)?.LoadBooking()));
            flpActions.Controls.Add(CreateActionCard("Medical Records", Color.FromArgb(71, 85, 105), (s, e) => (this.FindForm() as PatientDashboard)?.LoadHistory()));
            flpActions.Controls.Add(CreateActionCard("Latest Feedback", PatientTheme.Amber, (s, e) => (this.FindForm() as PatientDashboard)?.LoadRateVisits()));
            flpMain.Controls.Add(flpActions);

            // ── SCHEDULE FEED ──
            Label lScheduleTitle = new Label { Text = "UPCOMING VISITS", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.TextMuted, Margin = new Padding(0, 10, 0, 15), AutoSize = true };
            flpMain.Controls.Add(lScheduleTitle);

            FlowLayoutPanel flpUpcoming = new FlowLayoutPanel { Name = "flpUpcoming", Size = new Size(1100, 500), FlowDirection = FlowDirection.TopDown, WrapContents = false };
            flpMain.Controls.Add(flpUpcoming);
        }

        private Button CreateActionCard(string t, Color c, EventHandler click)
        {
            Button btn = new Button { Text = t, Size = new Size(230, 60), BackColor = Color.White, ForeColor = c, FlatStyle = FlatStyle.Flat, Font = PatientTheme.ButtonFont, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 20, 0) };
            btn.FlatAppearance.BorderColor = c;
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += click;
            btn.MouseEnter += (s, e) => { btn.BackColor = c; btn.ForeColor = Color.White; };
            btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; btn.ForeColor = c; };
            return btn;
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

        public void RefreshData()
        {
            DataManager.EnsureLoaded();
            var pnlHero = this.Controls.Find("pnlHero", true).FirstOrDefault() as Panel;
            var flpStats = this.Controls.Find("flpStats", true).FirstOrDefault() as FlowLayoutPanel;
            var flpUpcoming = this.Controls.Find("flpUpcoming", true).FirstOrDefault() as FlowLayoutPanel;
            if (pnlHero == null || flpStats == null || flpUpcoming == null) return;

            pnlHero.Controls.Clear();
            flpStats.Controls.Clear();
            flpUpcoming.Controls.Clear();

            var all = DataManager.Appointments.Where(a => a.PatientId == _patientId).OrderByDescending(a => a.AppointmentDate).ToList();
            var next = all.Where(a => a.AppointmentDate >= DateTime.Now && a.Status != "Cancelled").OrderBy(a => a.AppointmentDate).FirstOrDefault();
            
            // Build Hero
            BuildHeroContent(pnlHero, next);

            // Build Stats
            AddStatMini(flpStats, "SESSION COUNT", all.Count(a => a.Status == "Completed").ToString(), PatientTheme.Success);
            AddStatMini(flpStats, "DOCUMENTS", all.Count(a => !string.IsNullOrEmpty(a.Diagnosis)).ToString(), Color.FromArgb(71, 85, 105));
            AddStatMini(flpStats, "ACTIVE QUORES", "3", PatientTheme.Primary);

            // Build List
            foreach (var a in all.Take(4)) flpUpcoming.Controls.Add(CreateScheduleCard(a));
        }

        private void BuildHeroContent(Panel p, Appointment a)
        {
            if (a == null) {
                p.Controls.Add(new Label { Text = "Welcome to Alpha Clinic. Book your first visit today!", Font = PatientTheme.Subtitle, ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter });
                return;
            }
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            Label lH = new Label { Text = "YOUR NEXT APPOINTMENT", Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = Color.FromArgb(200, 255, 255, 255), Location = new Point(30, 30), AutoSize = true };
            Label lD = new Label { Text = a.AppointmentDate.ToString("MMMM dd, yyyy"), Font = PatientTheme.TitleLarge, ForeColor = Color.White, Location = new Point(27, 50), AutoSize = true };
            Label lT = new Label { Text = a.AppointmentDate.ToString("hh:mm tt"), Font = PatientTheme.Subtitle, ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
            Label lDoc = new Label { Text = $"Dr. {doc?.FullName ?? "Physician"}", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(30, 135), AutoSize = true };
            Label lSpec = new Label { Text = doc?.Specialty ?? "General", Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(200, 255, 255, 255), Location = new Point(32, 165), AutoSize = true };
            p.Controls.AddRange(new Control[] { lH, lD, lT, lDoc, lSpec });
        }

        private void AddStatMini(FlowLayoutPanel p, string t, string v, Color c)
        {
            Panel card = new Panel { Size = new Size(p.Width - 10, 75), BackColor = Color.White, Margin = new Padding(0, 0, 0, 15) };
            card.Paint += (s, e) => {
                using (Pen bp = new Pen(PatientTheme.Border, 1)) e.Graphics.DrawRectangle(bp, 0,0, card.Width-1, card.Height-1);
                e.Graphics.FillRectangle(new SolidBrush(c), 0, 0, 5, card.Height);
            };
            Label tl = new Label { Text = t, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = PatientTheme.TextMuted, Location = new Point(20, 15), AutoSize = true };
            Label vl = new Label { Text = v, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(18, 32), AutoSize = true };
            card.Controls.AddRange(new Control[] { tl, vl });
            p.Controls.Add(card);
        }

        private Panel CreateScheduleCard(Appointment a)
        {
            Panel p = new Panel { Size = new Size(1000, 100), BackColor = Color.White, Margin = new Padding(0, 0, 0, 12) };
            p.Paint += (s, e) => { using (Pen bp = new Pen(PatientTheme.Border, 1)) e.Graphics.DrawRectangle(bp, 0, 0, p.Width-1, p.Height-1); };
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            Label lD = new Label { Text = a.AppointmentDate.ToString("MMM dd"), Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(25, 20), AutoSize = true };
            Label lT = new Label { Text = a.AppointmentDate.ToString("hh:mm tt"), Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(25, 50), AutoSize = true };
            Label lDoc = new Label { Text = $"Dr. {doc?.FullName}", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(200, 25), AutoSize = true };
            Label lSt = new Label { Text = a.Status.ToUpper(), Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = a.Status == "Completed" ? PatientTheme.Success : PatientTheme.Primary, Location = new Point(200, 55), AutoSize = true };
            p.Controls.AddRange(new Control[] { lD, lT, lDoc, lSt });
            return p;
        }
    }
}
