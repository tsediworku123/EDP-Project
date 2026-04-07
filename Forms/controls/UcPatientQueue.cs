using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientQueue : UserControl
    {
        private int _doctorId;
        private Panel pnlContainer;
        private Label lblRefreshed;
        
        public UcPatientQueue(int doctorId)
        {
            this._doctorId = doctorId;
            InitializeUI();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.Resize += (s, e) => {
                if (pnlContainer != null) UpdateCardWidths();
            };
            RefreshQueue();
        }

        private void InitializeUI()
        {
            this.Controls.Clear();

            Panel topBar = new Panel { Dock = DockStyle.Top, Height = 100, Padding = new Padding(40, 40, 40, 0) };
            Label lTitle = new Label { Text = "LIVE PATIENT FLOW", Font = new Font("Segoe UI", 18, FontStyle.Bold), Dock = DockStyle.Left, AutoSize = true };
            lblRefreshed = new Label { Text = "Refreshed at --:--", Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Dock = DockStyle.Right, TextAlign = ContentAlignment.BottomRight, AutoSize = true };
            
            topBar.Controls.Add(lTitle);
            topBar.Controls.Add(lblRefreshed);
            this.Controls.Add(topBar);

            pnlContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(40, 10, 40, 40) };
            this.Controls.Add(pnlContainer);
        }

        private void UpdateCardWidths()
        {
            foreach (Control c in pnlContainer.Controls)
            {
                if (c is Panel p)
                {
                    p.Width = pnlContainer.Width - 85; 
                }
            }
        }

        public void RefreshQueue()
        {
            pnlContainer.Controls.Clear();
            DataManager.EnsureLoaded();
            lblRefreshed.Text = $"Refreshed at {DateTime.Now:hh:mm tt}";

            var today = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == DateTime.Today && 
                           (a.Status == "Scheduled" || a.Status == "In Progress" || a.Status == "Checked-In"))
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            if (today.Count == 0) {
                RenderEmptyState();
                return;
            }

            var current = today.FirstOrDefault(a => a.Status == "In Progress");
            var next = today.FirstOrDefault(a => a.Status == "Checked-In" || a.Status == "Scheduled");
            var others = today.Where(a => a != current && a != next).OrderBy(a => a.AppointmentDate).ToList();

            int y = 0;
            if (current != null) AddQueueItem("CURRENTLY IN CONSULTATION", current, ref y, Color.FromArgb(163, 113, 247), "\u2695"); // Medical icon
            if (next != null) AddQueueItem("NEXT PATIENT", next, ref y, Color.FromArgb(59, 130, 246), "\u25B6"); // Play/Next icon
            
            foreach (var a in others)
            {
                AddQueueItem("UPCOMING", a, ref y, Color.FromArgb(107, 114, 128), "\u231A"); // Clock icon
            }
            
            UpdateCardWidths();
        }

        private void RenderEmptyState()
        {
            Panel p = new Panel { Size = new Size(pnlContainer.Width - 85, 200), Location = new Point(0, 20), BackColor = Color.White };
            p.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);
            
            Label l = new Label { Text = "\u2714 No more patients scheduled for today.", Font = new Font("Segoe UI Semibold", 14), ForeColor = Color.FromArgb(16, 185, 129), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            p.Controls.Add(l);
            pnlContainer.Controls.Add(p);
        }

        private void AddQueueItem(string title, Appointment a, ref int y, Color accent, string icon)
        {
            Panel p = new Panel { 
                Size = new Size(pnlContainer.Width - 85, 160), 
                Location = new Point(0, y), 
                BackColor = Color.White,
                Cursor = Cursors.Default
            };
            
            // Premium accent bar
            p.Paint += (s, e) => e.Graphics.DrawLine(new Pen(accent, 8), 0, 0, 0, p.Height);
            p.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);

            Label lIcon = new Label { Text = icon, Font = new Font("Segoe UI", 16), ForeColor = accent, Location = new Point(25, 20), AutoSize = true };
            Label lTitle = new Label { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(60, 25), AutoSize = true };
            
            var pat = DataManager.Patients.FirstOrDefault(x => x.Id == a.PatientId);
            Label lName = new Label { Text = pat?.FullName ?? "Unknown", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(25, 65), AutoSize = true };
            
            TimeSpan wait = DateTime.Now - a.AppointmentDate;
            string waitStr = wait.TotalMinutes > 5 ? $"{ (int)wait.TotalMinutes } mins wait" : "On time";
            Label lInfo = new Label { Text = $"{a.AppointmentDate:hh:mm tt}  |  {a.Reason.ToUpper()}  |  {waitStr}", Font = new Font("Segoe UI", 10), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(25, 115), AutoSize = true };
            
            if (title == "NEXT PATIENT" || a.Status == "Checked-In" || a.Status == "Scheduled") {
                Button btnCall = new Button { 
                    Text = "CALL PATIENT", 
                    Anchor = AnchorStyles.Right | AnchorStyles.Top, 
                    Location = new Point(p.Width - 200, 55), 
                    Size = new Size(170, 55), 
                    BackColor = accent, 
                    ForeColor = Color.White, 
                    FlatStyle = FlatStyle.Flat, 
                    Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                    Cursor = Cursors.Hand 
                };
                btnCall.FlatAppearance.BorderSize = 0;
                btnCall.Click += (s, e) => {
                    a.Status = "In Progress";
                    DataManager.SaveAppointments();
                    (this.FindForm() as DoctorDashboard)?.OpenConsultation(a.Id);
                };
                p.Controls.Add(btnCall);
            }

            p.Controls.AddRange(new Control[] { lIcon, lTitle, lName, lInfo });
            pnlContainer.Controls.Add(p);
            y += 175; // Height + spacing
        }
    }
}
