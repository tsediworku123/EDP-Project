using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class ViewDoctorsForm : Form
    {
        public ViewDoctorsForm()
        {
            InitializeComponent();
            LoadDoctors();
        }

        private void LoadDoctors(string searchTerm = "")
        {
            lvDoctors.Items.Clear();
            var doctors = string.IsNullOrEmpty(searchTerm)
                ? DataManager.Doctors
                : DataManager.Doctors.Where(d =>
                    d.FullName.ToLower().Contains(searchTerm.ToLower()) ||
                    d.Specialization.ToLower().Contains(searchTerm.ToLower()));

            foreach (var doctor in doctors)
            {
                double rating = DataManager.GetDoctorAverageRating(doctor.Id);
                ListViewItem item = new ListViewItem(doctor.Id.ToString());
                item.SubItems.Add(doctor.FullName);
                item.SubItems.Add(doctor.Specialization);
                item.SubItems.Add(doctor.Gender);
                item.SubItems.Add(doctor.Phone);
                item.SubItems.Add(rating.ToString("0.0") + " ★");
                item.Tag = doctor;
                lvDoctors.Items.Add(item);
            }
            lblStatus.Text = $"Found {lvDoctors.Items.Count} doctors";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadDoctors(txtSearch.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDoctors(txtSearch.Text);
        }

        private void btnViewSchedule_Click(object sender, EventArgs e)
        {
            if (lvDoctors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a doctor to view schedule.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Doctor selectedDoctor = (Doctor)lvDoctors.SelectedItems[0].Tag;
            MessageBox.Show($"Dr. {selectedDoctor.FullName}'s schedule:\n\n" +
                $"Monday - Friday: 9:00 AM - 5:00 PM\n" +
                $"Saturday: 9:00 AM - 1:00 PM\n" +
                $"Sunday: Closed", "Doctor Schedule",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}