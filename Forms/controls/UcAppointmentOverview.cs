using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class UcAppointmentOverview : UserControl
    {
        public UcAppointmentOverview()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Dock = DockStyle.Fill;
            
            cmbDoctor.Items.Add("All Providers");
            foreach(var d in DataManager.Doctors) cmbDoctor.Items.Add(d.FullName);
            cmbDoctor.SelectedIndex = 0;
            
            cmbStatus.SelectedIndex = 0;
            
            RefreshData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbDoctor.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            RefreshData();
        }

        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            dgvAll.Rows.Clear();
            var query = DataManager.Appointments.AsEnumerable();
            
            if (cmbDoctor.SelectedIndex > 0) {
                string docName = cmbDoctor.SelectedItem.ToString();
                query = query.Where(a => (DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName ?? "") == docName);
            }
            
            if (cmbStatus.SelectedIndex > 0) {
                query = query.Where(a => a.Status == cmbStatus.SelectedItem.ToString());
            }

            if (dtpDate.Checked) {
                query = query.Where(a => a.AppointmentDate.Date == dtpDate.Value.Date);
            }

            foreach (var a in query.OrderByDescending(x => x.AppointmentDate)) {
                var pat = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                dgvAll.Rows.Add(
                    a.AppointmentDate.ToString("yyyy-MM-dd"),
                    a.AppointmentDate.ToString("HH:mm"),
                    pat?.FullName ?? "New Patient",
                    doc?.FullName ?? "Unassigned",
                    a.Reason,
                    a.Status
                );
            }
            lblCount.Text = $"Showing {dgvAll.Rows.Count} appointments matching criteria.";
        }
    }
}
