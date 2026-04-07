using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcBookingForm : UserControl
    {
        public UcBookingForm()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            
            LoadInitialData();
            cmbShift.Items.AddRange(new string[] { 
                "Morning Shift (08:00 AM - 02:00 PM)", 
                "Afternoon Shift (02:00 PM - 08:00 PM)", 
                "Evening Shift (06:00 PM - 11:00 PM)" 
            });
            cmbDuration.SelectedIndex = 1; // Default 20 min
            cmbType.SelectedIndex = 0;
        }

        private void HandleCancel_Click(object sender, EventArgs e)
        {
            if (this.ParentForm is AdminDashboard admin) admin.LoadDashboardHome();
            else if (this.ParentForm is DoctorDashboard doc) doc.LoadHome();
            else if (this.ParentForm is PatientDashboard pat) pat.LoadHome();
            else if (this.ParentForm is ReceptionDashboardForm recep) recep.LoadHome();
            else this.ParentForm?.Close();
        }

        public void PreselectPatient(int patientId)
        {
            var p = DataManager.Patients.FirstOrDefault(x => x.Id == patientId);
            if (p != null)
            {
                cmbPatient.Text = p.FullName;
            }
        }

        private void LoadInitialData()
        {
            cmbPatient.Items.Clear();
            foreach(var p in DataManager.Patients) cmbPatient.Items.Add(p.FullName);
            
            cmbDoctor.Items.Clear();
            foreach(var d in DataManager.Doctors.Where(doc => doc.IsActive)) 
                cmbDoctor.Items.Add($"{d.FullName} - {d.Specialty}");
        }

        private void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbShift.SelectedIndex < 0) return;

            // Set constraints for Start Time Picker
            DateTime date = dtpDate.Value.Date;
            switch (cmbShift.SelectedIndex)
            {
                case 0: // Morning 8-2
                    dtpStartTime.Value = date.AddHours(8);
                    break;
                case 1: // Afternoon 2-8
                    dtpStartTime.Value = date.AddHours(14);
                    break;
                case 2: // Evening 6-11
                    dtpStartTime.Value = date.AddHours(18);
                    break;
            }
            TimingChanged(null, null);
        }

        private void TimingChanged(object sender, EventArgs e)
        {
            if (cmbDuration.SelectedItem == null) return;
            
            int duration = int.Parse(cmbDuration.SelectedItem.ToString());
            DateTime start = dtpStartTime.Value;
            DateTime end = start.AddMinutes(duration);

            lblEndTimeDisplay.Text = end.ToString("hh:mm tt");
            
            // Check shift boundaries
            ValidateShiftBoundaries(start, end);
        }

        private bool ValidateShiftBoundaries(DateTime start, DateTime end)
        {
            if (cmbShift.SelectedIndex < 0) return true;

            TimeSpan s = start.TimeOfDay;
            TimeSpan e = end.TimeOfDay;

            bool valid = false;
            switch (cmbShift.SelectedIndex)
            {
                case 0: valid = (s >= TimeSpan.FromHours(8) && e <= TimeSpan.FromHours(14)); break;
                case 1: valid = (s >= TimeSpan.FromHours(14) && e <= TimeSpan.FromHours(20)); break;
                case 2: valid = (s >= TimeSpan.FromHours(18) && e <= TimeSpan.FromHours(23)); break;
            }

            lblEndTimeDisplay.ForeColor = valid ? Color.FromArgb(59, 130, 246) : Color.Red;
            return valid;
        }

        private void btnNewPatient_Click(object sender, EventArgs e)
        {
            using (var reg = new RegisterPatientForm())
            {
                if (reg.ShowDialog() == DialogResult.OK)
                {
                    LoadInitialData();
                    cmbPatient.SelectedIndex = cmbPatient.Items.Count - 1;
                }
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var patName = cmbPatient.Text.Trim();
            var pat = DataManager.Patients.FirstOrDefault(p => p.FullName.Equals(patName, StringComparison.OrdinalIgnoreCase));
            
            var docName = cmbDoctor.SelectedItem.ToString().Split('-')[0].Trim();
            var doc = DataManager.Doctors.FirstOrDefault(d => d.FullName == docName);

            if (pat == null) {
                MessageBox.Show("Patient not found. Please select a valid patient or register a new one.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime start = dtpStartTime.Value;
            int duration = int.Parse(cmbDuration.SelectedItem.ToString());
            DateTime end = start.AddMinutes(duration);

            if (!ValidateShiftBoundaries(start, end)) {
                MessageBox.Show("Appointment time falls outside the selected shift hours.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // OVERLAP CHECK
            var existingAppts = DataManager.Appointments.Where(a => 
                a.DoctorId == doc.Id && 
                a.AppointmentDate.Date == dtpDate.Value.Date &&
                (a.Status == "Pending" || a.Status == "Confirmed")
            ).ToList();

            foreach (var a in existingAppts)
            {
                DateTime aStart = a.AppointmentDate;
                DateTime aEnd = aStart.AddMinutes(30); // Default duration if not specified in model, should ideally be property

                // Overlap exists if (StartA < EndB) and (EndA > StartB)
                if (start < aEnd && end > aStart)
                {
                    MessageBox.Show($"OVERLAP DETECTED!\n\nThis doctor already has an appointment from {aStart:hh:mm tt} to {aEnd:hh:mm tt}.\n\nPlease choose a different time.", "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var appt = new Appointment {
                Id = DataManager.Appointments.Count > 0 ? DataManager.Appointments.Max(a => a.Id) + 1 : 1,
                PatientId = pat.Id,
                DoctorId = doc.Id,
                AppointmentDate = start,
                Reason = string.IsNullOrWhiteSpace(txtReason.Text) ? cmbType.SelectedItem.ToString() : txtReason.Text.Trim(),
                ConsultationFee = nudFee.Value,
                IsPaid = chkPaid.Checked,
                Status = "Confirmed"
            };

            DataManager.Appointments.Add(appt);
            DataManager.SaveAppointments();

            MessageBox.Show("Appointment booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            HandleCancel_Click(null, null); // Return to home
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(cmbPatient.Text) || cmbDoctor.SelectedIndex < 0 || cmbShift.SelectedIndex < 0 || cmbDuration.SelectedIndex < 0)
            {
                MessageBox.Show("Please fill in all required fields (Patient, Doctor, Shift, and Duration).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
