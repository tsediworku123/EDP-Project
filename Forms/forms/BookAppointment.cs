using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class BookAppointmentForm : Form
    {
        private Patient currentPatient;
        private Doctor selectedDoctor;

        public BookAppointmentForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            currentPatient = DataManager.GetCurrentPatient();
            LoadDoctors();
            LoadAvailableSlots();
        }

        private void LoadDoctors()
        {
            cmbDoctor.Items.Clear();
            foreach (var doctor in DataManager.Doctors)
            {
                cmbDoctor.Items.Add($"{doctor.FullName} - {doctor.Specialization}");
            }
            if (cmbDoctor.Items.Count > 0) cmbDoctor.SelectedIndex = 0;
        }

        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDoctor.SelectedItem != null)
            {
                string selected = cmbDoctor.SelectedItem.ToString();
                string doctorName = selected.Split('-')[0].Trim();
                selectedDoctor = DataManager.Doctors.FirstOrDefault(d => d.FullName == doctorName);
            }
        }

        private void LoadAvailableSlots()
        {
            lstTimeSlots.Items.Clear();
            string[] slots = { "09:00 AM", "10:00 AM", "11:00 AM", "02:00 PM", "03:00 PM", "04:00 PM" };

            foreach (string slot in slots)
            {
                ListViewItem item = new ListViewItem(slot);
                item.SubItems.Add("Available");
                item.ForeColor = Color.Green;
                lstTimeSlots.Items.Add(item);
            }
        }

        private void btnCheckAvailability_Click(object sender, EventArgs e)
        {
            if (selectedDoctor == null && cmbDoctor.SelectedItem != null)
            {
                string selected = cmbDoctor.SelectedItem.ToString();
                string doctorName = selected.Split('-')[0].Trim();
                selectedDoctor = DataManager.Doctors.FirstOrDefault(d => d.FullName == doctorName);
            }

            LoadAvailableSlots();
            MessageBox.Show($"Available slots for {dtpDate.Value:MMM dd, yyyy} are shown below.",
                "Availability", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (selectedDoctor == null && cmbDoctor.SelectedItem != null)
            {
                string selected = cmbDoctor.SelectedItem.ToString();
                string doctorName = selected.Split('-')[0].Trim();
                selectedDoctor = DataManager.Doctors.FirstOrDefault(d => d.FullName == doctorName);
            }

            if (selectedDoctor == null)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstTimeSlots.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a time slot.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Please enter a reason for the appointment.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string selectedTime = lstTimeSlots.SelectedItems[0].Text;
                DateTime appointmentDateTime = dtpDate.Value.Date.Add(DateTime.Parse(selectedTime).TimeOfDay);

                Appointment newAppointment = new Appointment
                {
                    Id = DataManager.Appointments.Count > 0 ? DataManager.Appointments.Max(a => a.Id) + 1 : 1,
                    PatientId = currentPatient?.Id ?? 0,
                    DoctorId = selectedDoctor.Id,
                    AppointmentDate = appointmentDateTime,
                    Reason = txtReason.Text.Trim(),
                    Status = "Pending"
                };

                DataManager.AddAppointment(newAppointment);
                MessageBox.Show("Appointment booked successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form after successful booking
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking appointment: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}