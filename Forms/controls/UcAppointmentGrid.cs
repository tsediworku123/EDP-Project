using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcAppointmentGrid : UserControl
    {
        private bool _isAdminMode = false;
        private Timer _refreshTimer;
        private FlowLayoutPanel _flpWaiting;

        public bool IsAdminMode {
            get => _isAdminMode;
            set { _isAdminMode = value; UpdateModeUI(); }
        }

        public UcAppointmentGrid()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            SetupLiveQueueUI();

            _refreshTimer = new Timer { Interval = 30000 }; // 30s auto refresh
            _refreshTimer.Tick += (s, e) => RefreshData();
            _refreshTimer.Start();
            
            this.Disposed += (s, e) => {
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();
            };
            
            RefreshData();
        }

        private void SetupLiveQueueUI()
        {
            // Reconfigure Dropdowns
            cmbDoctor.Items.Clear();
            cmbDoctor.Items.Add("All Doctors");
            foreach(var d in DataManager.Doctors) cmbDoctor.Items.Add(d.FullName);
            cmbDoctor.SelectedIndex = 0;
            
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "All Statuses", "Pending", "Confirmed", "In Progress", "Completed", "No-Show", "Cancelled" });
            cmbStatus.SelectedIndex = 0;

            // Reconfigure Grid Columns
            apptGrid.Columns.Clear();
            apptGrid.Columns.Add("Id", "Id");
            apptGrid.Columns["Id"].Visible = false;
            apptGrid.Columns.Add("Time", "Time");
            apptGrid.Columns.Add("Patient", "Patient Name");
            apptGrid.Columns.Add("Phone", "Phone");
            apptGrid.Columns.Add("Doctor", "Doctor");
            apptGrid.Columns.Add("Reason", "Reason");
            apptGrid.Columns.Add("Status", "Status");

            DataGridViewButtonColumn btnCheckIn = new DataGridViewButtonColumn { Name = "ActionCheckIn", HeaderText = "Action", Text = "Check-In", UseColumnTextForButtonValue = true };
            DataGridViewButtonColumn btnComplete = new DataGridViewButtonColumn { Name = "ActionComplete", HeaderText = "", Text = "Complete", UseColumnTextForButtonValue = true };
            DataGridViewButtonColumn btnCancel = new DataGridViewButtonColumn { Name = "ActionCancel", HeaderText = "", Text = "Cancel", UseColumnTextForButtonValue = true };
            
            apptGrid.Columns.AddRange(new DataGridViewColumn[] { btnCheckIn, btnComplete, btnCancel });
            apptGrid.CellContentClick += ApptGrid_CellContentClick;

            // Build Waiting List Side Panel
            Panel pnlRight = new Panel { Dock = DockStyle.Right, Width = 300, BackColor = Color.White, Padding = new Padding(15) };
            Label lblWaitInfo = new Label { Text = "LIVE WAITING LIST", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Top, Height = 40 };
            
            _flpWaiting = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            pnlRight.Controls.Add(_flpWaiting);
            pnlRight.Controls.Add(lblWaitInfo);
            
            // Add right panel to container first to ensure Dock.Fill takes remaining space
            this.Controls.Add(pnlRight);
            apptGrid.SendToBack(); // Push grid behind the side panel docking context
            pnlRight.BringToFront(); 
        }

        private void ApptGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isAdminMode) return;

            string colName = apptGrid.Columns[e.ColumnIndex].Name;
            int id = (int)apptGrid.Rows[e.RowIndex].Cells["Id"].Value;
            var appt = DataManager.Appointments.FirstOrDefault(a => a.Id == id);
            
            if (appt == null) return;

            if (colName == "ActionCheckIn") appt.Status = "In Progress";
            else if (colName == "ActionComplete") appt.Status = "Completed";
            else if (colName == "ActionCancel") appt.Status = "Cancelled";
            else return;

            DataManager.SaveAppointments();
            RefreshData();
        }

        private void UpdateModeUI()
        {
            if (apptGrid.Columns.Contains("ActionCheckIn"))
            {
                apptGrid.Columns["ActionCheckIn"].Visible = !_isAdminMode;
                apptGrid.Columns["ActionComplete"].Visible = !_isAdminMode;
                apptGrid.Columns["ActionCancel"].Visible = !_isAdminMode;
            }
        }

        public void RefreshData()
        {
            if (this.IsDisposed || apptGrid == null || apptGrid.IsDisposed || apptGrid.Columns.Count == 0) return;

            apptGrid.Rows.Clear();
            _flpWaiting?.Controls.Clear();

            var list = DataManager.Appointments.Where(a => a.AppointmentDate.Date == dtpDate.Value.Date).ToList();
            
            // Build Waiting List (In Progress)
            foreach(var w in list.Where(a => a.Status == "In Progress").OrderBy(a => a.AppointmentDate))
            {
                var p = DataManager.Patients.FirstOrDefault(x => x.Id == w.PatientId);
                var d = DataManager.Doctors.FirstOrDefault(x => x.Id == w.DoctorId);
                AddWaitingCard(p?.FullName ?? "Unknown", w.AppointmentDate.ToString("HH:mm"), d?.FullName ?? "Dr.", w.Reason);
            }

            // Apply Filters for Grid
            if (cmbDoctor.SelectedIndex > 0)
                list = list.Where(a => DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName == cmbDoctor.SelectedItem.ToString()).ToList();
            
            if (cmbStatus.SelectedIndex > 0)
                list = list.Where(a => a.Status == cmbStatus.SelectedItem.ToString()).ToList();

            string search = txtSearch.Text.ToLower();
            
            foreach (var a in list.OrderBy(x => x.AppointmentDate)) {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                if (!string.IsNullOrEmpty(search) && pat != null && !pat.FullName.ToLower().Contains(search) && !(pat.Phone?.Contains(search) ?? false)) continue;
                
                var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                
                int r = apptGrid.Rows.Add(a.Id, a.AppointmentDate.ToString("HH:mm"), pat?.FullName ?? "Unknown", pat?.Phone ?? "---", doc?.FullName ?? "---", a.Reason, a.Status);
                
                // Color code statuses
                if (a.Status == "In Progress") apptGrid.Rows[r].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199); // Light yellow
                else if (a.Status == "Completed") apptGrid.Rows[r].DefaultCellStyle.BackColor = Color.FromArgb(209, 250, 229); // Light green
                else if (a.Status == "No-Show" || a.Status == "Cancelled") apptGrid.Rows[r].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226); // Light red
            }
            lblResults.Text = $"Found {apptGrid.Rows.Count} appointments matching criteria.";
        }

        private void AddWaitingCard(string patName, string time, string docName, string reason)
        {
            Panel p = new Panel { Width = 260, Height = 90, BackColor = Color.FromArgb(248, 250, 252), Margin = new Padding(5, 5, 5, 10) };
            p.Paint += (s, e) => { e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240)), 0, 0, p.Width - 1, p.Height - 1); };

            Label lTime = new Label { Text = time, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(245, 158, 11), Location = new Point(10, 10), AutoSize = true };
            Label lPat = new Label { Text = patName, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(60, 8), AutoSize = true };
            Label lDoc = new Label { Text = $"For: {docName}", Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(10, 35), AutoSize = true };
            Label lReason = new Label { Text = reason, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(10, 60), AutoSize = true };

            p.Controls.AddRange(new Control[] { lTime, lPat, lDoc, lReason });
            _flpWaiting?.Controls.Add(p);
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e) { RefreshData(); }
        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e) { RefreshData(); }
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) { RefreshData(); }
        private void txtSearch_TextChanged(object sender, EventArgs e) { RefreshData(); }
    }
}
