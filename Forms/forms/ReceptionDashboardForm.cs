using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class ReceptionDashboardForm : Form
    {
        public ReceptionDashboardForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += ReceptionDashboardForm_KeyDown;
            RefreshDashboard();
            if (txtGlobalSearch != null)
            {
                txtGlobalSearch.TextChanged += (s, e) => RefreshDashboard();
            }
        }

        private void RefreshDashboard()
        {
            // 1. CALCULATE STATISTICS
            var today = DataManager.Appointments.Where(a => a.AppointmentDate.Date == DateTime.Today).ToList();
            int total = today.Count;
            int checkedIn = today.Count(a => a.Status == "Checked-In");
            int completed = today.Count(a => a.Status == "Completed");
            int cancelled = today.Count(a => a.Status == "Cancelled");
            int pending = today.Count(a => a.Status == "Scheduled");

            // 2. POPULATE STATS PANEL (Clear first to avoid duplicates on refresh)
            pnlStats.Controls.Clear();
            int cardWidth = (pnlStats.Width - 60) / 5;
            if (cardWidth < 180) cardWidth = 180;

            pnlStats.Controls.Add(CreateStatCard("TOTAL VISITS", total.ToString(), "", Color.FromArgb(52, 152, 219), cardWidth));
            pnlStats.Controls.Add(CreateStatCard("CHECKED IN", checkedIn.ToString(), "", Color.FromArgb(155, 89, 182), cardWidth));
            pnlStats.Controls.Add(CreateStatCard("PENDING", pending.ToString(), "", Color.FromArgb(243, 156, 18), cardWidth));
            pnlStats.Controls.Add(CreateStatCard("COMPLETED", completed.ToString(), "", Color.FromArgb(46, 204, 113), cardWidth));
            pnlStats.Controls.Add(CreateStatCard("CANCELLED", cancelled.ToString(), "", Color.FromArgb(192, 57, 43), cardWidth));

            // 3. FILL THE QUEUE
            string search = txtGlobalSearch?.Text.Trim().ToLower() ?? "";
            dgvQueue.Rows.Clear();
            var sortedToday = today.OrderBy(a => a.AppointmentDate).ToList();

            foreach (var a in sortedToday)
            {
                string patientName = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId)?.FullName ?? "Unknown";
                string phone = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId)?.Phone ?? "";
                string doctorName = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName ?? "Doc";
                string docDisplay = doctorName.StartsWith("Dr.") ? doctorName : "Dr. " + doctorName;

                if (!string.IsNullOrEmpty(search))
                {
                    if (!patientName.ToLower().Contains(search) && !phone.Contains(search))
                        continue;
                }

                int idx = dgvQueue.Rows.Add(
                    a.AppointmentDate.ToString("hh:mm tt"),
                    patientName.ToUpper(),
                    docDisplay,
                    a.Status,
                    "Check-In", "Complete", "Cancel"
                );
                
                dgvQueue.Rows[idx].Tag = a.Id;

                // Color Status
                StyleRowByStatus(dgvQueue.Rows[idx], a.Status);
            }
        }

        private Panel CreateStatCard(string title, string value, string icon, Color color, int width)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Size = new Size(width, 100),
                Margin = new Padding(0, 0, 12, 0),
                BorderStyle = BorderStyle.None
            };

            // Add subtle bottom border for accent
            Panel accent = new Panel { Dock = DockStyle.Bottom, Height = 4, BackColor = color };
            card.Controls.Add(accent);

            Label lblValue = new Label { Text = value, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(15, 10), AutoSize = true };
            Label lblTitle = new Label { Text = title, Font = new Font("Segoe UI Semibold", 9F), ForeColor = Color.Gray, Location = new Point(17, 50), AutoSize = true };
            Label lblIcon = new Label { Text = icon, Font = new Font("Segoe UI", 24F), ForeColor = color, Location = new Point(width - 55, 18), AutoSize = true };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblIcon);

            return card;
        }

        private void StyleRowByStatus(DataGridViewRow row, string status)
        {
            row.Cells["Status"].Style.Font = new Font(dgvQueue.Font, FontStyle.Bold);
            switch (status)
            {
                case "Scheduled": row.Cells["Status"].Style.ForeColor = Color.FromArgb(41, 128, 185); break;
                case "Checked-In": row.Cells["Status"].Style.ForeColor = Color.FromArgb(142, 68, 173); row.DefaultCellStyle.BackColor = Color.FromArgb(250, 245, 255); break;
                case "Completed": row.Cells["Status"].Style.ForeColor = Color.FromArgb(39, 174, 96); break;
                case "Cancelled": row.Cells["Status"].Style.ForeColor = Color.Gray; break;
            }
        }

        private void HandleCheckIn_Click(object sender, EventArgs e) => HandleContextAction("Checked-In");
        private void HandleComplete_Click(object sender, EventArgs e) => HandleContextAction("Completed");
        private void HandleCancel_Click(object sender, EventArgs e) => HandleContextAction("Cancelled");

        private void HandleContextAction(string newStatus)
        {
            if (dgvQueue.SelectedRows.Count > 0 && dgvQueue.SelectedRows[0].Tag != null)
            {
                int apptId = (int)dgvQueue.SelectedRows[0].Tag;
                var a = DataManager.Appointments.FirstOrDefault(x => x.Id == apptId);
                if (a != null)
                {
                    a.Status = newStatus;
                    RefreshDashboard();
                }
            }
        }

        private void dgvQueue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvQueue.Rows[e.RowIndex].Tag == null) return;
            int apptId = (int)dgvQueue.Rows[e.RowIndex].Tag;
            var a = DataManager.Appointments.FirstOrDefault(x => x.Id == apptId);
            if (a == null) return;

            if (dgvQueue.Columns[e.ColumnIndex].Name == "CheckIn") a.Status = "Checked-In";
            else if (dgvQueue.Columns[e.ColumnIndex].Name == "Complete") a.Status = "Completed";
            else if (dgvQueue.Columns[e.ColumnIndex].Name == "Cancel") a.Status = "Cancelled";

            RefreshDashboard();
        }

        private void btnNavFindPatient_Click(object sender, EventArgs e) { MessageBox.Show("Please use the global search or modern sidebar."); RefreshDashboard(); }
        private void btnNavNewBooking_Click(object sender, EventArgs e) { MessageBox.Show("Please use the modern sidebar for new bookings."); RefreshDashboard(); }
        private void btnNavSchedule_Click(object sender, EventArgs e) { MessageBox.Show("Please use the modern sidebar to view schedules."); RefreshDashboard(); }
        private void btnNavDoctorAvailability_Click(object sender, EventArgs e) { MessageBox.Show("Please use the modern sidebar for doctor availability."); RefreshDashboard(); }

        private void ReceptionDashboardForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) { e.Handled = true; btnNavFindPatient_Click(this, EventArgs.Empty); }
            else if (e.KeyCode == Keys.F2) { e.Handled = true; btnNavNewBooking_Click(this, EventArgs.Empty); }
            else if (e.KeyCode == Keys.F3) { e.Handled = true; btnNavSchedule_Click(this, EventArgs.Empty); }
            else if (e.KeyCode == Keys.F4) { e.Handled = true; btnNavDoctorAvailability_Click(this, EventArgs.Empty); }
            else if (e.Control && e.KeyCode == Keys.F) 
            { 
                e.Handled = true; 
                if (txtGlobalSearch != null) { txtGlobalSearch.Focus(); txtGlobalSearch.SelectAll(); }
            }
        }

        public void LoadHome()
        {
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(pnlStats);
            pnlContent.Controls.Add(pnlQueue);
            RefreshDashboard();
        }

        public void ShowControl(Control uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.MainForm.ForceLogout(false);
            }
        }
    }
}
