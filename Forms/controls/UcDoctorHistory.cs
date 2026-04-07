using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorHistory : UserControl
    {
        private int _doctorId;

        public UcDoctorHistory(int doctorId)
        {
            this._doctorId = doctorId;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            SetupKPIs();
            RefreshData();
            
            // Ensure Search Header is on top
            this.pnlHeader.BringToFront();
        }

        private void SetupKPIs()
        {
            pnlStats.Controls.Clear();
            var history = DataManager.Appointments.Where(a => a.DoctorId == _doctorId).ToList();
            
            AddStatCard("TOTAL RECORDS", history.Count.ToString(), "\u231B", Color.FromArgb(59, 130, 246));
            AddStatCard("COMPLETED", history.Count(a => a.Status == "Completed").ToString(), "\u2705", Color.FromArgb(16, 185, 129));
            AddStatCard("PENDING", history.Count(a => a.Status == "Pending").ToString(), "\u23F3", Color.FromArgb(245, 158, 11));
        }

        private void AddStatCard(string title, string value, string icon, Color color)
        {
            Panel card = new Panel { Size = new Size(220, 80), BackColor = Color.White, Margin = new Padding(0, 0, 20, 0) };
            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(new SolidBrush(color), 0, 0, 5, card.Height); // Side accent
            };

            Label lIcon = new Label { Text = icon, Font = new Font("Segoe UI", 16), ForeColor = color, Location = new Point(15, 25), AutoSize = true };
            Label lTitle = new Label { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(148, 163, 184), Location = new Point(50, 15), AutoSize = true };
            Label lVal = new Label { Text = value, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(50, 35), AutoSize = true };

            card.Controls.AddRange(new Control[] { lIcon, lTitle, lVal });
            pnlStats.Controls.Add(card);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvHistory.Columns[e.ColumnIndex].Name == "ActionView") {
                MessageBox.Show("Historical notes feature coming soon.");
            }
        }

        public void RefreshData()
        {
            dgvHistory.Rows.Clear();
            string filter = txtSearch.Text.ToLower().Trim();
            string statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All Statuses";
            
            var history = DataManager.Appointments
                .Where(a => a.DoctorId == _doctorId)
                .OrderByDescending(a => a.AppointmentDate).ToList();

            foreach (var a in history) {
                if (statusFilter != "All Statuses" && a.Status != statusFilter) continue;

                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                
                // Unified Search (Patient ID, Name, Diagnosis, or Date)
                bool matches = string.IsNullOrEmpty(filter) || 
                               (pat != null && pat.Id.ToString() == filter) ||
                               (pat != null && pat.FullName.ToLower().Contains(filter)) ||
                               (a.Diagnosis != null && a.Diagnosis.ToLower().Contains(filter)) ||
                               (a.Reason != null && a.Reason.ToLower().Contains(filter)) ||
                               (a.AppointmentDate.ToString("yyyy-MM-dd").Contains(filter));

                if (!matches) continue;
                
                dgvHistory.Rows.Add(a.AppointmentDate.ToString("yyyy-MM-dd"), pat?.FullName ?? "Unknown", a.Diagnosis ?? "N/A", a.Status);
            }
        }
    }
}
