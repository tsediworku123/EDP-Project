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
    public class UcPatientRateVisits : UserControl
    {
        private int _patientId;
        private Panel pnlHeader;
        private FlowLayoutPanel flpVisits;
        private Label lblPendingCount;

        public UcPatientRateVisits(int patientId)
        {
            this._patientId = patientId;
            InitializeModernUI();
            this.Dock = DockStyle.Fill;
            this.BackColor = PatientTheme.Background;
            RefreshData();
        }

        private void InitializeModernUI()
        {
            this.Controls.Clear();

            // 1. Header
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = PatientTheme.Surface, Padding = new Padding(40, 30, 40, 0) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            
            Label lblTitle = new Label { Text = "SHARE YOUR FEEDBACK", Font = PatientTheme.TitleMedium, ForeColor = PatientTheme.TextPrimary, Location = new Point(40, 25), AutoSize = true };
            Label lblSubtitle = new Label { Text = "Help us improve by rating your recent clinical experiences.", Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(41, 62), AutoSize = true };
            
            Panel pnlStats = new Panel { Size = new Size(200, 45), Location = new Point(40, 95), BackColor = Color.FromArgb(254, 243, 199) }; // Amber 100
            pnlStats.Paint += (s, e) => {
                using (Pen p = new Pen(Color.FromArgb(251, 191, 36), 1)) e.Graphics.DrawRectangle(p, 0, 0, pnlStats.Width - 1, pnlStats.Height - 1);
            };
            lblPendingCount = new Label { Text = "0 PENDING RATINGS", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(146, 64, 14), Location = new Point(0, 0), AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            pnlStats.Controls.Add(lblPendingCount);

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, pnlStats });
            this.Controls.Add(pnlHeader);

            // 2. Visit Flow
            flpVisits = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(40, 30, 40, 40),
                BackColor = PatientTheme.Background,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpVisits);
        }

        public void RefreshData()
        {
            if (flpVisits == null) return;
            flpVisits.Controls.Clear();

            var unratedAppts = DataManager.Appointments
                .Where(a => a.PatientId == _patientId && a.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase) && a.PatientRating == 0)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            if (lblPendingCount != null) lblPendingCount.Text = $"{unratedAppts.Count} PENDING RATINGS";

            if (unratedAppts.Count == 0)
            {
                Label lblEmpty = new Label {
                    Text = "You have no pending consultations to rate. Thank you!",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    ForeColor = PatientTheme.TextMuted,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Size = new Size(flpVisits.Width - 80, 200)
                };
                flpVisits.Controls.Add(lblEmpty);
            }
            else
            {
                foreach (var a in unratedAppts)
                {
                    flpVisits.Controls.Add(CreateRatingCard(a));
                }
            }
        }

        private Panel CreateRatingCard(Appointment a)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            Panel p = new Panel { Size = new Size(flpVisits.Width > 100 ? flpVisits.Width - 100 : 900, 100), BackColor = PatientTheme.Surface, Margin = new Padding(0, 0, 0, 15) };
            p.Paint += (s, e) => {
                using (Pen bp = new Pen(PatientTheme.Border, 1)) e.Graphics.DrawRectangle(bp, 0, 0, p.Width - 1, p.Height - 1);
            };

            Label lDate = new Label { Text = a.AppointmentDate.ToString("MMM dd, yyyy"), Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(25, 25), AutoSize = true };
            Label lDoc = new Label { Text = "Dr. " + (doc?.FullName ?? "Physician"), Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextSecondary, Location = new Point(25, 52), AutoSize = true };
            
            Label lReason = new Label { Text = a.Reason ?? "General Consultation", Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = PatientTheme.TextSecondary, Location = new Point(300, 40), AutoSize = true };

            Button btnRate = new Button {
                Text = "★ RATE NOW",
                Size = new Size(160, 45),
                Location = new Point(p.Width - 200, 28),
                BackColor = Color.FromArgb(254, 243, 199), // Amber 100
                ForeColor = Color.FromArgb(146, 64, 14), // Amber 800
                FlatStyle = FlatStyle.Flat,
                Font = PatientTheme.LabelBold,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnRate.FlatAppearance.BorderSize = 0;
            btnRate.Click += (s, e) => {
                var rateForm = new RateDoctorForm(a);
                if (rateForm.ShowDialog() == DialogResult.OK) {
                    RefreshData();
                    (this.FindForm() as PatientDashboard)?.Refresh();
                }
            };

            p.Controls.AddRange(new Control[] { lDate, lDoc, lReason, btnRate });
            return p;
        }
    }
}
