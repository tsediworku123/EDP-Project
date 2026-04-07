using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;
using System.Collections.Generic;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDashboardHome : UserControl
    {
        public UcDashboardHome()
        {
            InitializeComponent();
            SetupGrid();
            RefreshStats();
            SetupTimer();
            
            this.Resize += (s, e) => {
                pnlCards.Width = this.Width - 40;
                pnlQuickLinks.Width = this.Width - 40;
                pnlLiveGrid.Width = this.Width - 40;
                
                // Adjust card sizes to spread across width if possible
                UpdateCardSpacing();
            };
        }

        private void UpdateCardSpacing()
        {
            // Optional: Adjust margins or padding based on width
        }

        private void SetupGrid()
        {
            dgvRecent.Columns.Clear();
            dgvRecent.Columns.Add("Time", "Time");
            dgvRecent.Columns.Add("Patient", "Patient Name");
            dgvRecent.Columns.Add("Doctor", "Doctor / Specialty");
            dgvRecent.Columns.Add("Reason", "Reason for Visit");
            dgvRecent.Columns.Add("Status", "Status");

            dgvRecent.Columns[0].Width = 100;
            dgvRecent.Columns[1].Width = 250;
            dgvRecent.Columns[2].Width = 250;
            dgvRecent.Columns[3].Width = 250;
            dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            dgvRecent.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgvRecent.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            dgvRecent.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRecent.EnableHeadersVisualStyles = false;
        }

        private void SetupTimer()
        {
            timerRefresh.Interval = 60000; // 60 seconds
            timerRefresh.Tick += (s, e) => RefreshStats();
            timerRefresh.Start();
        }

        public void RefreshStats()
        {
            if (this.IsDisposed || dgvRecent == null || dgvRecent.IsDisposed || dgvRecent.Columns.Count == 0) return;

            DataManager.EnsureLoaded();
            
            pnlCards.Controls.Clear();
            pnlQuickLinks.Controls.Clear();
            
            string role = DataManager.CurrentUser?.Role;
            bool isRec = (role == "Receptionist");

            // KPI Stats Calculation
            int todayAppts = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today);
            int pending = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && (a.Status == "Pending" || a.Status == "Confirmed"));
            int waiting = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "In Progress"); // or Checked-In
            int completed = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Completed");
            int noShow = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "No-Show");

            if (isRec)
            {
                // Receptionist KPIs
                AddStatCard("TODAY'S APPTS", todayAppts.ToString(), Color.FromArgb(59, 130, 246));
                AddStatCard("PENDING/CONFIRMED", pending.ToString(), Color.FromArgb(139, 92, 246));
                AddStatCard("WAITING NOW", waiting.ToString(), Color.FromArgb(245, 158, 11));
                AddStatCard("COMPLETED", completed.ToString(), Color.FromArgb(16, 185, 129));
                AddStatCard("NO-SHOW TODAY", noShow.ToString(), Color.FromArgb(239, 68, 68));

                // Receptionist Quick Links
                AddQuickLink(" \u271A Book Appointment", "Schedule standard visit", (s, e) => {
                    if (this.ParentForm is AdminDashboard admin) admin.ShowControl(new UcBookingForm());
                });

                AddQuickLink(" \u2795 New Patient", "Register to directory", (s, e) => {
                    var form = new RegisterPatientForm();
                    form.ShowDialog();
                    RefreshStats();
                });

                AddQuickLink(" \uD83C\uDFC3 Walk-in (Urgent)", "Immediate placement", (s, e) => {
                    // Quick walkin flow could be just opening booking form with current time
                    if (this.ParentForm is AdminDashboard admin) admin.ShowControl(new UcBookingForm());
                });
            }
            else
            {
                // Admin KPIs
                int totalPatients = DataManager.Patients.Count;
                int monthAppts = DataManager.Appointments.Count(a => a.AppointmentDate.Month == DateTime.Today.Month && a.AppointmentDate.Year == DateTime.Today.Year);
                int activeDocs = DataManager.Doctors.Count(d => d.IsActive);
                string noShowRate = todayAppts > 0 ? $"{(noShow / (double)todayAppts) * 100:0}%" : "0%";

                AddStatCard("TOTAL PATIENTS", totalPatients.ToString(), Color.FromArgb(59, 130, 246));
                AddStatCard("TODAY'S FLOW", todayAppts.ToString(), Color.FromArgb(16, 185, 129));
                AddStatCard("ACTIVE DOCTORS", activeDocs.ToString(), Color.FromArgb(139, 92, 246));
                AddStatCard("NO-SHOW RATE", noShowRate, Color.FromArgb(245, 158, 11));
                AddStatCard("MONTHLY VOL.", monthAppts.ToString(), Color.FromArgb(100, 116, 139));

                // Admin Quick Links

                AddQuickLink(" \u2695 New Doctor", "Clinical Profile", (s, e) => {
                    if (this.ParentForm is AdminDashboard admin) admin.ShowControl(new UcDoctorManagement());
                });
                
                AddQuickLink(" \u2193 Backup Data", "Full Snapshot", (s, e) => {
                    var backupService = new ClinicAppointmentSystem.Services.BackupService();
                    var result = backupService.CreateBackup();
                    
                    if (result.Success)
                    {
                        var msg = $"Backup created successfully at:\n{result.Path}\n\nWould you like to open the folder?";
                        if (MessageBox.Show(msg, "System Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", result.Path);
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

                AddQuickLink(" \u2261 Full Reports", "Analytics", (s, e) => {
                    if (this.ParentForm is AdminDashboard admin) admin.ShowControl(new UcClinicalReports());
                });
            }

            // Refresh Grid
            dgvRecent.Rows.Clear();
            foreach (var appt in DataManager.Appointments.Where(a => a.AppointmentDate.Date == DateTime.Today).OrderBy(a => a.AppointmentDate))
            {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
                var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == appt.DoctorId);
                dgvRecent.Rows.Add(
                    appt.AppointmentDate.ToString("HH:mm"),
                    pat?.FullName ?? "Unknown",
                    $"{doc?.FullName ?? "---"} ({doc?.Specialty ?? "GP"})",
                    appt.Reason,
                    appt.Status
                );
            }
        }

        private void AddStatCard(string title, string value, Color iconColor)
        {
            Panel p = new Panel { Size = new Size(195, 115), BackColor = Color.White, Margin = new Padding(10) };
            
            // Premium side accent (Status color)
            p.Paint += (s, e) => {
                e.Graphics.DrawLine(new Pen(iconColor, 6), 0, 0, 0, p.Height);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);
            };

            Label lVal = new Label { 
                Text = value, 
                Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold), 
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Top, 
                Height = 65, 
                TextAlign = ContentAlignment.BottomCenter 
            };
            Label lTitle = new Label { 
                Text = title, 
                Font = new Font("Segoe UI", 7, FontStyle.Bold), 
                ForeColor = iconColor, // Use the color for the title text for better distinction
                Dock = DockStyle.Bottom, 
                Height = 45, 
                TextAlign = ContentAlignment.TopCenter 
            };
            p.Controls.AddRange(new Control[] { lVal, lTitle });
            pnlCards.Controls.Add(p);
        }

        private void AddQuickLink(string title, string desc, EventHandler click)
        {
            Panel p = new Panel { Size = new Size(245, 105), BackColor = Color.White, Margin = new Padding(12), Cursor = Cursors.Hand };
            p.Click += click;
            p.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);

            Label lTitle = new Label { 
                Text = title, 
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold), 
                ForeColor = Color.FromArgb(59, 130, 246), // Secondary Brand Blue
                Location = new Point(15, 20),
                AutoSize = true
            };
            lTitle.Click += click;
            
            Label lDesc = new Label { 
                Text = desc, 
                Font = new Font("Segoe UI", 8), 
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(18, 55),
                AutoSize = true
            };
            lDesc.Click += click;
            
            p.Controls.AddRange(new Control[] { lTitle, lDesc });
            
            p.MouseEnter += (s, e) => p.BackColor = Color.FromArgb(243, 244, 246);
            p.MouseLeave += (s, e) => p.BackColor = Color.White;
            
            pnlQuickLinks.Controls.Add(p);
        }
    }
}
