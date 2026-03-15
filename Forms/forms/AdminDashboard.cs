using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadStatistics();
            LoadRecentAppointments();
        }

        private void LoadStatistics()
        {
            lblTotalPatients.Text = DataManager.Patients.Count.ToString();
            lblTotalDoctors.Text = DataManager.Doctors.Count.ToString();
            lblTotalAppointments.Text = DataManager.Appointments.Count.ToString();
            lblTodayAppointments.Text = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today).ToString();
        }

        private void LoadRecentAppointments()
        {
            lvRecentAppointments.Items.Clear();
            var recent = DataManager.Appointments.OrderByDescending(a => a.AppointmentDate).Take(5);

            foreach (var apt in recent)
            {
                string patient = DataManager.Patients.FirstOrDefault(p => p.Id == apt.PatientId)?.FullName ?? "Unknown";
                string doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == apt.DoctorId)?.FullName ?? "Unknown";

                ListViewItem item = new ListViewItem(apt.Id.ToString());
                item.SubItems.Add(patient);
                item.SubItems.Add(doctor);
                item.SubItems.Add(apt.AppointmentDate.ToString("MMM dd, yyyy hh:mm tt"));
                item.SubItems.Add(apt.Status);
                lvRecentAppointments.Items.Add(item);
            }
        }
    }
}