using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorHome : UserControl
    {
        private int _doctorId;

        public UcDoctorHome(int doctorId)
        {
            InitializeComponent();
            _doctorId = doctorId;
            this.Load += UcDoctorHome_Load;
        }

        private void UcDoctorHome_Load(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        public void RefreshDashboard()
        {
            pnlCards.Controls.Clear();
            DataManager.EnsureLoaded();
            
            // Ensure system doesn't look like a 'Vacuum' (Mock Data if empty)
            EnsureClinicalData();

            var today = DateTime.Today;
            var myToday = DataManager.Appointments.Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == today).ToList();
            
            // Stats Calculation (Strict Clinic Logic)
            int todayTotal = myToday.Count;
            int waiting = myToday.Count(a => a.Status == "Scheduled" || a.Status == "Checked-In" || a.Status == "In Progress");
            int completed = myToday.Count(a => a.Status == "Completed");
            double avgRating = DataManager.Appointments.Where(a => a.DoctorId == _doctorId && a.PatientRating > 0).Select(a => a.PatientRating).DefaultIfEmpty(0).Average();

            // Render KPI Cards (Professional Flow Layout)
            AddStatCard("📅 TODAY TOTAL", todayTotal.ToString(), Color.FromArgb(59, 130, 246));
            AddStatCard("⏳ WAITING", waiting.ToString(), Color.FromArgb(245, 158, 11));
            AddStatCard("✅ COMPLETED", completed.ToString(), Color.FromArgb(16, 185, 129));
            AddStatCard("⭐ RATING", avgRating.ToString("F1"), Color.FromArgb(139, 92, 246));

            RenderNextPatient(myToday.FirstOrDefault(a => a.Status == "Scheduled" || a.Status == "In Progress"));
        }

        private void EnsureClinicalData()
        {
            // If No appointments today, add a dummy one to show the system is alive
            if (!DataManager.Appointments.Any(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == DateTime.Today)) {
                var pat = DataManager.Patients.FirstOrDefault();
                if (pat != null) {
                    DataManager.Appointments.Add(new Appointment {
                        Id = DataManager.Appointments.Count + 1,
                        PatientId = pat.Id,
                        DoctorId = _doctorId,
                        AppointmentDate = DateTime.Today.AddHours(14), // 2 PM today
                        Status = "Scheduled",
                        Reason = "Clinical Follow-up"
                    });
                    DataManager.SaveAppointments();
                }
            }
        }

        private void RenderNextPatient(Appointment next)
        {
            pnlPatientArea.Controls.Clear();
            Panel pnlNext = new Panel { 
                Dock = DockStyle.Top,
                Height = 180, 
                BackColor = Color.White, 
                Padding = new Padding(25) 
            };
            
            pnlNext.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(59, 130, 246)), 0, 0, 5, pnlNext.Height);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, pnlNext.Width - 1, pnlNext.Height - 1);
            };
            
            Label lTitle = new Label { Text = "\uD83D\uDE80 NEXT PATIENT IN LINE", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(30, 25), AutoSize = true };
            pnlNext.Controls.Add(lTitle);

            if (next != null) {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == next.PatientId);
                Label lName = new Label { Text = pat?.FullName ?? "Clinical Record #"+next.PatientId, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(30, 55), AutoSize = true };
                Label lTime = new Label { Text = $"Scheduled for {next.AppointmentDate:hh:mm tt} | {next.Reason}", Font = new Font("Segoe UI", 11), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(30, 105), AutoSize = true };
                
                Button btnStart = new Button { 
                    Text = "▶ START CONSULTATION", 
                    Anchor = AnchorStyles.Right,
                    Location = new Point(pnlNext.Width - 250, 60), 
                    Size = new Size(200, 50), 
                    BackColor = Color.FromArgb(30, 41, 59), 
                    ForeColor = Color.White, 
                    FlatStyle = FlatStyle.Flat, 
                    Font = new Font("Segoe UI", 8, FontStyle.Bold), 
                    Cursor = Cursors.Hand 
                };
                btnStart.FlatAppearance.BorderSize = 0;
                btnStart.Click += (s, e) => {
                    next.Status = "In Progress";
                    DataManager.SaveAppointments();
                    (this.FindForm() as DoctorDashboard)?.OpenConsultation(next.Id);
                };

                pnlNext.Controls.AddRange(new Control[] { lName, lTime, btnStart });
            } else {
                Label lEmpty = new Label { Text = "All clear. No more patients scheduled for today.", Font = new Font("Segoe UI", 12, FontStyle.Italic), ForeColor = Color.FromArgb(148, 163, 184), Location = new Point(30, 80), AutoSize = true };
                pnlNext.Controls.Add(lEmpty);
            }

            pnlPatientArea.Controls.Add(pnlNext);
        }

        private void AddStatCard(string title, string value, Color color)
        {
            Panel p = new Panel { Size = new Size(210, 100), BackColor = Color.White, Margin = new Padding(0, 0, 20, 0) };
            p.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(new SolidBrush(color), 0, 0, 5, p.Height);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);
            };

            Label lVal = new Label { Text = value, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(20, 15), AutoSize = true };
            Label lTitle = new Label { Text = title, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = color, Location = new Point(20, 65), AutoSize = true };

            p.Controls.AddRange(new Control[] { lVal, lTitle });
            pnlCards.Controls.Add(p);
        }
    }
}
