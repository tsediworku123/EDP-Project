using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class AppointmentForm : Form
    {
        public AppointmentForm()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            cmbPatient.Items.Clear();
            foreach (var p in DataManager.Patients)
            {
                cmbPatient.Items.Add($"{p.Id} - {p.FullName}");
            }

            cmbDoctor.Items.Clear();
            foreach (var d in DataManager.Doctors)
            {
                cmbDoctor.Items.Add($"{d.Id} - {d.FullName} ({d.Specialization})");
            }

            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            if (cmbDoctor.Items.Count > 0) cmbDoctor.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedItem == null || cmbDoctor.SelectedItem == null)
            {
                MessageBox.Show("Please select patient and doctor!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTime.Text))
            {
                MessageBox.Show("Please enter time!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string patientText = cmbPatient.SelectedItem.ToString();
                string doctorText = cmbDoctor.SelectedItem.ToString();

                int patientId = int.Parse(patientText.Split('-')[0].Trim());
                int doctorId = int.Parse(doctorText.Split('-')[0].Trim());

                DateTime appointmentDate = dtpDate.Value.Date;
                TimeSpan time = DateTime.Parse(txtTime.Text).TimeOfDay;
                DateTime dateTime = appointmentDate.Add(time);

                Appointment appointment = new Appointment
                {
                    Id = DataManager.Appointments.Count > 0 ? DataManager.Appointments.Max(a => a.Id) + 1 : 1,
                    PatientId = patientId,
                    DoctorId = doctorId,
                    AppointmentDate = dateTime,
                    Reason = txtReason.Text.Trim(),
                    Status = cmbStatus.SelectedItem.ToString()
                };

                DataManager.Appointments.Add(appointment);
                DataManager.SaveAppointments();

                MessageBox.Show("Appointment scheduled successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTime_TextChanged(object sender, EventArgs e)
        {

        }
    }
}