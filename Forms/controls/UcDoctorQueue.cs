using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorQueue : UserControl
    {
        private int _doctorId;
        private System.Windows.Forms.Timer refreshTimer;

        public UcDoctorQueue(int doctorId)
        {
            this._doctorId = doctorId;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            refreshTimer = new System.Windows.Forms.Timer { Interval = 30000 }; // 30s auto-refresh
            refreshTimer.Tick += (s, e) => RefreshData();
            refreshTimer.Start();
            
            RefreshData();
        }

        private void dtpFilter_ValueChanged(object sender, EventArgs e)
        {
            lblTitle.Text = dtpFilter.Value.Date == DateTime.Today ? "LIVE PATIENT QUEUE (TODAY)" : $"SCHEDULE FOR {dtpFilter.Value:MMM dd, yyyy}";
            RefreshData();
        }

        private void btnRefresh_Click(object sender, EventArgs e) { RefreshData(); }
        private void btnCallNext_Click(object sender, EventArgs e) { CallNextPatient(); }
        private void btnNoShow_Click(object sender, EventArgs e) { MarkNoShow(); }

        private void DgvQueue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvQueue.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Scheduled") e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139);
                else if (status == "Checked-In") e.CellStyle.ForeColor = Color.FromArgb(59, 130, 246);
                else if (status == "In Progress") e.CellStyle.ForeColor = Color.FromArgb(163, 113, 247);
                else if (status == "Completed") e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                else if (status == "Cancelled") e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void DgvQueue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgvQueue.Columns[e.ColumnIndex].Name;
            int apptId = (int)dgvQueue.Rows[e.RowIndex].Cells["Id"].Value;
            var appt = DataManager.Appointments.FirstOrDefault(a => a.Id == apptId);

            if (col == "ActionStart")
            {
                if (appt != null) {
                    appt.Status = "In Progress";
                    RefreshData();
                    (this.FindForm() as AdminDashboard)?.OpenConsultation(apptId);
                }
            }
            else if (col == "ActionSkip")
            {
                if (appt != null) {
                    appt.Status = "Scheduled"; // Reset from checked-in or just move it
                    MessageBox.Show("Patient skipped. They will remain in the schedule.", "Queue Updated");
                    RefreshData();
                }
            }
            else if (col == "ActionDone")
            {
                if (appt != null && MessageBox.Show("Mark this appointment as Completed?", "Quick Action", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    appt.Status = "Completed";
                    appt.CompletionTime = DateTime.Now;
                    RefreshData();
                }
            }
        }

        public void RefreshData()
        {
            dgvQueue.Rows.Clear();
            var appts = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == dtpFilter.Value.Date)
                .OrderBy(a => a.AppointmentDate).ToList();

            foreach (var a in appts)
            {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                string time = a.AppointmentDate.ToString("HH:mm");
                dgvQueue.Rows.Add(a.Id, time, pat?.FullName ?? "Unknown", "General", a.Reason, a.Status);
            }

            int waiting = appts.Count(a => a.Status == "Checked-In");
            lblCount.Text = $"Total Today: {appts.Count} | Waiting: {waiting}";
        }

        private void CallNextPatient()
        {
            var next = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId && a.AppointmentDate.Date == DateTime.Today && a.Status == "Checked-In")
                .OrderBy(a => a.AppointmentDate).FirstOrDefault();

            if (next != null)
            {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == next.PatientId);
                MessageBox.Show($"Next Patient: {pat?.FullName ?? "Unknown"}\nTime: {next.AppointmentDate:HH:mm}", "Calling Patient", MessageBoxButtons.OK, MessageBoxIcon.Information);
                next.Status = "In Progress";
                RefreshData();
                (this.FindForm() as AdminDashboard)?.OpenConsultation(next.Id);
            }
            else
            {
                MessageBox.Show("No patients currently waiting in the queue.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MarkNoShow()
        {
            if (dgvQueue.SelectedRows.Count == 0) {
                MessageBox.Show("Please select a patient row from the queue.");
                return;
            }

            int apptId = (int)dgvQueue.SelectedRows[0].Cells["Id"].Value;
            var appt = DataManager.Appointments.FirstOrDefault(a => a.Id == apptId);
            if (appt != null) {
                string patName = dgvQueue.SelectedRows[0].Cells["Patient"].Value.ToString();
                if (MessageBox.Show($"Mark {patName} as NO-SHOW?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    appt.Status = "No-Show";
                    RefreshData();
                }
            }
        }
    }
}
