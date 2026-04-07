using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorAvailability : UserControl
    {
        private int _doctorId;

        public UcDoctorAvailability(int doctorId)
        {
            InitializeComponent();
            _doctorId = doctorId;
            
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            cmbShift.SelectedIndexChanged += cmbShift_SelectedIndexChanged;
            btnAddBlock.Click += btnAddBlock_Click;
            btnToggleStatus.Click += btnToggleStatus_Click;
            dgvBlocked.CellContentClick += dgvBlocked_CellContentClick;
            
            // Precision Controls
            btnDecrease.Click += (s, e) => NudgeDuration(-1);
            btnIncrease.Click += (s, e) => NudgeDuration(1);
            
            RefreshUI();
            
            // Force High-Precision UI
            this.cmbShift.Width = 220;
            this.btnAddBlock.Left = 715;
            
            cmbShift.SelectedIndex = 0;
            ApplyShiftDefaults();
        }

        private void RefreshUI()
        {
            DataManager.EnsureLoaded();
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _doctorId);
            if (doc == null) return;

            // Update Status Toggle
            bool onLeave = doc.CurrentStatus == "On Leave";
            btnToggleStatus.Text = onLeave ? "🔴 CURRENTLY ON LEAVE" : "🟢 AVAILABLE FOR BOOKING";
            btnToggleStatus.BackColor = onLeave ? Color.FromArgb(254, 242, 242) : Color.FromArgb(236, 253, 245);
            btnToggleStatus.ForeColor = onLeave ? Color.FromArgb(220, 38, 38) : Color.FromArgb(5, 150, 105);
            btnToggleStatus.FlatAppearance.BorderColor = btnToggleStatus.ForeColor;

            // Refresh Blocked Grid
            dgvBlocked.Rows.Clear();
            if (doc.BlockedTimes != null) {
                foreach (var t in doc.BlockedTimes.Where(x => x >= DateTime.Today).OrderBy(x => x)) {
                    dgvBlocked.Rows.Add(t.ToString("dddd, MMM dd | hh:mm tt"));
                }
            }
        }

        private void cmbShift_SelectedIndexChanged(object sender, EventArgs e) => ApplyShiftDefaults();

        private void ApplyShiftDefaults()
        {
            switch (cmbShift.SelectedIndex) {
                case 0: txtStartTime.Text = "08:00"; txtEndTime.Text = "08:30"; break;
                case 1: txtStartTime.Text = "14:00"; txtEndTime.Text = "14:30"; break;
                case 2: txtStartTime.Text = "18:00"; txtEndTime.Text = "18:30"; break;
            }
        }

        private void btnAddBlock_Click(object sender, EventArgs e)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _doctorId);
            if (doc == null) return;

            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan start) || 
                !TimeSpan.TryParse(txtEndTime.Text, out TimeSpan end)) {
                MessageBox.Show("Please enter valid times (e.g., 08:30).");
                return;
            }

            if (end <= start) {
                MessageBox.Show("End time must be after start time.");
                return;
            }

            if (doc.BlockedTimes == null) doc.BlockedTimes = new List<DateTime>();

            // Block range
            DateTime current = dtpDate.Value.Date.Add(start);
            DateTime limit = dtpDate.Value.Date.Add(end);

            while (current < limit) {
                if (!doc.BlockedTimes.Any(t => t == current)) {
                    doc.BlockedTimes.Add(current);
                }
                current = current.AddMinutes(15);
            }

            DataManager.SaveDoctors();
            RefreshUI();
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _doctorId);
            if (doc == null) return;

            doc.CurrentStatus = doc.CurrentStatus == "On Leave" ? "Available" : "On Leave";
            doc.IsOnLeave = (doc.CurrentStatus == "On Leave");
            DataManager.SaveDoctors();
            RefreshUI();
        }

        private void dgvBlocked_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 1) return;

            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _doctorId);
            if (doc == null || doc.BlockedTimes == null) return;

            string timeStr = dgvBlocked.Rows[e.RowIndex].Cells[0].Value.ToString();
            var target = doc.BlockedTimes.FirstOrDefault(t => t.ToString("dddd, MMM dd | hh:mm tt") == timeStr);
            
            if (target != default) {
                doc.BlockedTimes.Remove(target);
                DataManager.SaveDoctors();
                RefreshUI();
            }
        }

        private void NudgeDuration(int direction)
        {
            if (TimeSpan.TryParse(txtEndTime.Text, out TimeSpan current)) {
                var next = current.Add(TimeSpan.FromMinutes(direction * 15));
                if (next.TotalHours >= 0 && next.TotalHours < 24) {
                    txtEndTime.Text = $"{(int)next.TotalHours:D2}:{next.Minutes:D2}";
                }
            }
        }
    }
}
