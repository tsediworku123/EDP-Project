using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using System.Collections.Generic;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorAppointments : UserControl
    {
        private int _doctorId;

        public UcDoctorAppointments(int doctorId)
        {
            this._doctorId = doctorId;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            // Connect Designer Controls
            this.txtSearch.TextChanged += (s, e) => RefreshData();
            this.dtpFilter.ValueChanged += (s, e) => RefreshData();
            this.dgvQueue.CellContentClick += dgvQueue_CellContentClick;

            SetupGrid();
            RefreshData();
            
            // Ensure Search Header is on top
            this.pnlHeader.BringToFront();
        }

        private void SetupGrid()
        {
            dgvQueue.Columns.Clear();
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "TIME", Width = 80 });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "Patient", HeaderText = "PATIENT NAME", MinimumWidth = 200 });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "Reason", HeaderText = "REASON / TYPE", MinimumWidth = 150 });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "STATUS", Width = 120 });
            
            dgvQueue.Columns.Add(new DataGridViewButtonColumn { 
                Name = "ActionStart", HeaderText = "", Text = "START", UseColumnTextForButtonValue = true, Width = 100 
            });
            dgvQueue.Columns.Add(new DataGridViewButtonColumn { 
                Name = "ActionNoShow", HeaderText = "", Text = "NO-SHOW", UseColumnTextForButtonValue = true, Width = 100 
            });

            // Enable Interactive Header Sorting
            foreach (DataGridViewColumn col in dgvQueue.Columns) {
                if (!(col is DataGridViewButtonColumn)) col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            // Status Styling
            dgvQueue.CellFormatting += (s, e) => {
                if (dgvQueue.Columns[e.ColumnIndex].Name == "Status" && e.Value != null) {
                    string st = e.Value.ToString();
                    if (st == "In Progress") e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                    else if (st == "Completed") e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                    else if (st == "No-Show") e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    else e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                }
            };
        }

        private void SetupKPIs(List<Appointment> apps)
        {
            pnlStats.Controls.Clear();
            AddStatCard("TODAY TOTAL", apps.Count.ToString(), "\u231A", Color.FromArgb(59, 130, 246));
            AddStatCard("WAITING", apps.Count(a => a.Status == "Checked-In" || a.Status == "Scheduled").ToString(), "\u23F3", Color.FromArgb(245, 158, 11));
            AddStatCard("COMPLETED", apps.Count(a => a.Status == "Completed").ToString(), "\u2705", Color.FromArgb(16, 185, 129));
            AddStatCard("NO-SHOWS", apps.Count(a => a.Status == "No-Show").ToString(), "\u274C", Color.FromArgb(239, 68, 68));
        }

        private void AddStatCard(string title, string value, string icon, Color color)
        {
            Panel card = new Panel { Size = new Size(180, 75), BackColor = Color.White, Margin = new Padding(0, 0, 15, 0) };
            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(new SolidBrush(color), 0, 0, 5, card.Height);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, card.Width - 1, card.Height - 1);
            };

            Label lIcon = new Label { Text = icon, Font = new Font("Segoe UI", 14), ForeColor = color, Location = new Point(12, 22), AutoSize = true };
            Label lTitle = new Label { Text = title, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = Color.FromArgb(148, 163, 184), Location = new Point(40, 15), AutoSize = true };
            Label lVal = new Label { Text = value, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(40, 32), AutoSize = true };

            card.Controls.AddRange(new Control[] { lIcon, lTitle, lVal });
            pnlStats.Controls.Add(card);
        }

        public void RefreshData()
        {
            dgvQueue.Rows.Clear();
            DataManager.EnsureLoaded();
            
            DateTime targetDate = dtpFilter.Value.Date;
            string search = txtSearch.Text.ToLower().Trim();

            var appointments = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == targetDate)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            // Ensure Clinical Activity
            if (appointments.Count == 0 && targetDate == DateTime.Today) {
                var pat = DataManager.Patients.FirstOrDefault();
                if (pat != null) {
                    DataManager.Appointments.Add(new Appointment {
                        Id = DataManager.Appointments.Count + 1, PatientId = pat.Id, DoctorId = _doctorId,
                        AppointmentDate = DateTime.Today.AddHours(14), Status = "Scheduled", Reason = "Follow-up"
                    });
                    DataManager.SaveAppointments();
                    appointments = DataManager.Appointments.Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == targetDate).ToList();
                }
            }

            SetupKPIs(appointments);

            foreach (var a in appointments) {
                var pat = DataManager.Patients.FirstOrDefault(x => x.Id == a.PatientId);
                string filter = search.ToLower().Trim();
                
                bool matches = string.IsNullOrEmpty(filter) || 
                               (pat != null && pat.FullName.ToLower().Contains(filter)) || 
                               (pat != null && pat.Id.ToString() == filter) || 
                               (a.Reason != null && a.Reason.ToLower().Contains(filter));

                if (!matches) continue;

                int rowIndex = dgvQueue.Rows.Add(
                    a.AppointmentDate.ToString("HH:mm"),
                    pat?.FullName ?? "Unknown",
                    a.Reason ?? "Consultation",
                    a.Status
                );
                dgvQueue.Rows[rowIndex].Tag = a.Id;
                
                if (a.Status == "Completed" || a.Status == "Cancelled" || a.Status == "No-Show") {
                    dgvQueue.Rows[rowIndex].Cells["ActionStart"].ReadOnly = true;
                    dgvQueue.Rows[rowIndex].Cells["ActionNoShow"].ReadOnly = true;
                }
            }
        }

        private void dgvQueue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int apptId = (int)dgvQueue.Rows[e.RowIndex].Tag;
            var appt = DataManager.Appointments.FirstOrDefault(a => a.Id == apptId);
            if (appt == null) return;

            if (dgvQueue.Columns[e.ColumnIndex].Name == "ActionStart") {
                appt.Status = "In Progress";
                DataManager.SaveAppointments();
                (this.FindForm() as DoctorDashboard)?.OpenConsultation(appt.Id);
            }
            else if (dgvQueue.Columns[e.ColumnIndex].Name == "ActionNoShow") {
                if (MessageBox.Show("Mark as No-Show?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    appt.Status = "No-Show";
                    DataManager.SaveAppointments();
                    RefreshData();
                }
            }
        }
    }
}
