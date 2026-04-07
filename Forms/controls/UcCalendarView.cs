using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcCalendarView : UserControl
    {
        public UcCalendarView()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            // Defer data load until the control is actually visible on screen
            this.Load += (s, e) => RefreshCalendar();
        }

        public void RefreshCalendar()
        {
            if (dgvDaily == null || dgvDaily.IsDisposed) return;
            if (dgvDaily.Columns.Count == 0) return; // Not ready yet

            dgvDaily.SuspendLayout();
            dgvDaily.Rows.Clear();
            DateTime targetDate = dtpNavDate.Value.Date;
            var appts = DataManager.Appointments
                .Where(a => a.AppointmentDate.Date == targetDate)
                .OrderBy(a => a.AppointmentDate).ToList();

            foreach (var a in appts)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                dgvDaily.Rows.Add(
                    a.AppointmentDate.ToString("hh:mm tt"),
                    patient?.FullName ?? "Unknown",
                    a.Reason ?? "--",
                    a.Status
                );
            }

            if (dgvDaily.Rows.Count == 0)
            {
                dgvDaily.Rows.Add("--", "No appointments for this date", "--", "--");
            }
            dgvDaily.ResumeLayout();
        }

        private void dtpNavDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshCalendar();
        }

        private void cmbColorBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCalendar();
        }
    }
}
