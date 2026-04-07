using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorFeedback : UserControl
    {
        private int _doctorId;

        public UcDoctorFeedback(int doctorId)
        {
            InitializeComponent();
            this.Controls.Clear();
            _doctorId = doctorId;
            LoadFeedback();
        }

        private void LoadFeedback()
        {
            this.Controls.Clear();
            DataManager.EnsureLoaded();

            var ratedAppts = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId && a.PatientRating > 0)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            double avg = ratedAppts.Count > 0 ? ratedAppts.Average(a => a.PatientRating) : 0;

            // Header Section
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 150, BackColor = Color.White, Padding = new Padding(30) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 149, pnlHeader.Width, 149);

            Label lAvg = new Label { Text = avg.ToString("F1"), Font = new Font("Segoe UI", 48, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(30, 20), AutoSize = true };
            Label lStars = new Label { Text = new String('★', (int)Math.Round(avg)) + new String('☆', 5 - (int)Math.Round(avg)), Font = new Font("Segoe UI", 18), ForeColor = Color.FromArgb(245, 158, 11), Location = new Point(180, 40), AutoSize = true };
            Label lCount = new Label { Text = $"Based on {ratedAppts.Count} patient reviews", Font = new Font("Segoe UI", 10), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(180, 80), AutoSize = true };

            pnlHeader.Controls.AddRange(new Control[] { lAvg, lStars, lCount });
            this.Controls.Add(pnlHeader);

            // Feed Section
            FlowLayoutPanel pnlFeed = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(30), FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.FromArgb(249, 250, 251) };
            
            foreach (var a in ratedAppts) {
                var pat = DataManager.Patients.FirstOrDefault(x => x.Id == a.PatientId);
                Panel r = new Panel { Size = new Size(800, 100), BackColor = Color.White, Margin = new Padding(0, 0, 0, 15) };
                r.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, r.Width - 1, r.Height - 1);

                Label rStars = new Label { Text = new String('★', a.PatientRating), ForeColor = Color.FromArgb(245, 158, 11), Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
                Label rPat = new Label { Text = pat?.FullName ?? "Verified Patient", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(20, 40), AutoSize = true };
                Label rDate = new Label { Text = a.AppointmentDate.ToString("MMM dd, yyyy"), Font = new Font("Segoe UI", 8), ForeColor = Color.FromArgb(148, 163, 184), Location = new Point(700, 15), AutoSize = true };
                Label rText = new Label { Text = string.IsNullOrWhiteSpace(a.PatientFeedback) ? "No comment left." : "\"" + a.PatientFeedback + "\"", Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(20, 65), AutoSize = true, Width = 750 };

                r.Controls.AddRange(new Control[] { rStars, rPat, rDate, rText });
                pnlFeed.Controls.Add(r);
            }

            this.Controls.Add(pnlFeed);
            pnlFeed.BringToFront();
        }
    }
}
