using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class RescheduleAppointmentDialog : Form
    {
        private Appointment appointment;

        public RescheduleAppointmentDialog(Appointment apt)
        {
            appointment = apt;
            InitializeComponent();
            LoadAvailableSlots();
        }

        private void LoadAvailableSlots()
        {
            dtpNewDate.MinDate = DateTime.Today;
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

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            if (lstTimeSlots.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a new time slot.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedTime = lstTimeSlots.SelectedItems[0].Text;
            DateTime newDateTime = dtpNewDate.Value.Date.Add(DateTime.Parse(selectedTime).TimeOfDay);

            DataManager.RescheduleAppointment(appointment.Id, newDateTime);
            MessageBox.Show("Appointment rescheduled successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}