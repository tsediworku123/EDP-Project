using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class MyAppointmentsForm : Form
    {
        private Patient currentPatient;
        private Appointment selectedAppointment;

        public MyAppointmentsForm()
        {
            InitializeComponent();
            currentPatient = DataManager.GetCurrentPatient();

            if (currentPatient == null)
            {
                MessageBox.Show("You must be logged in to view appointments.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadAppointments();
        }

        private void LoadAppointments(string filter = "All")
        {
            lvAppointments.Items.Clear();
            var appointments = DataManager.GetPatientAppointments(currentPatient.Id);

            if (filter == "Upcoming")
                appointments = appointments.Where(a => a.AppointmentDate >= DateTime.Today && a.Status != "Cancelled").ToList();
            else if (filter == "Past")
                appointments = appointments.Where(a => a.AppointmentDate < DateTime.Today).ToList();
            else if (filter == "Cancelled")
                appointments = appointments.Where(a => a.Status == "Cancelled").ToList();

            foreach (var apt in appointments)
            {
                string doctorName = DataManager.Doctors.FirstOrDefault(d => d.Id == apt.DoctorId)?.FullName ?? "Unknown";

                ListViewItem item = new ListViewItem(apt.Id.ToString());
                item.SubItems.Add(doctorName);
                item.SubItems.Add(apt.AppointmentDate.ToString("MMM dd, yyyy"));
                item.SubItems.Add(apt.AppointmentDate.ToString("hh:mm tt"));
                item.SubItems.Add(apt.Reason);
                item.SubItems.Add(apt.Status);
                item.Tag = apt;

                // Color code based on status
                if (apt.Status == "Confirmed")
                    item.BackColor = Color.FromArgb(220, 255, 220);
                else if (apt.Status == "Pending")
                    item.BackColor = Color.FromArgb(255, 255, 200);
                else if (apt.Status == "Cancelled")
                    item.BackColor = Color.FromArgb(255, 200, 200);
                else if (apt.Status == "Completed")
                    item.BackColor = Color.FromArgb(200, 230, 255);

                lvAppointments.Items.Add(item);
            }

            lblStatus.Text = $"Found {lvAppointments.Items.Count} appointments";
        }

        private void lvAppointments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAppointments.SelectedItems.Count > 0)
            {
                selectedAppointment = (Appointment)lvAppointments.SelectedItems[0].Tag;
                btnReschedule.Enabled = selectedAppointment.Status != "Cancelled" &&
                                        selectedAppointment.Status != "Completed" &&
                                        selectedAppointment.AppointmentDate >= DateTime.Today;
                btnCancel.Enabled = selectedAppointment.Status != "Cancelled" &&
                                    selectedAppointment.Status != "Completed";
            }
            else
            {
                btnReschedule.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (selectedAppointment == null) return;

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this appointment?",
                "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataManager.CancelAppointment(selectedAppointment.Id);
                LoadAppointments(cmbFilter.Text);
                MessageBox.Show("Appointment cancelled successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            if (selectedAppointment == null) return;

            using (RescheduleAppointmentDialog dialog = new RescheduleAppointmentDialog(selectedAppointment))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadAppointments(cmbFilter.Text);
                }
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAppointments(cmbFilter.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments(cmbFilter.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}