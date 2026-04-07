using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcAuditLog : UserControl
    {
        public UcAuditLog()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Dock = DockStyle.Fill;
            cmbModule.SelectedIndex = 0;
            RefreshLogs();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshLogs(txtSearch.Text);
        }

        private void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLogs(txtSearch.Text);
        }

        private void RefreshLogs(string filter = "")
        {
            dgvLogs.Rows.Clear();
            
            if (!DataManager.AuditLogs.Any())
            {
                // Seed with initial system logs if empty
                DataManager.LogAudit("System", "Application Started", "Security");
                DataManager.LogAudit("admin", "Initialization Sync Complete", "Settings");
            }

            var logs = DataManager.AuditLogs.AsEnumerable();

            // Filter by Search Text
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                string f = txtSearch.Text.ToLower();
                logs = logs.Where(l => l.Username.ToLower().Contains(f) || l.Action.ToLower().Contains(f));
            }

            // Filter by Module
            if (cmbModule.SelectedIndex > 0)
            {
                string selectedMod = cmbModule.SelectedItem.ToString();
                logs = logs.Where(l => l.Module == selectedMod);
            }

            foreach (var log in logs.OrderByDescending(l => l.Timestamp))
            {
                int idx = dgvLogs.Rows.Add(
                    log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.Username,
                    log.Action,
                    log.Module,
                    log.IpAddress
                );

                // Styling row based on module
                if (log.Module == "Security") dgvLogs.Rows[idx].Cells[3].Style.ForeColor = Color.FromArgb(239, 68, 68);
                else if (log.Module == "Clinical") dgvLogs.Rows[idx].Cells[3].Style.ForeColor = Color.FromArgb(16, 185, 129);
            }
        }


    }
}
