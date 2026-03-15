using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class UserDashboard : Form
    {
        private Timer refreshTimer;
        private Patient currentPatient;

        public UserDashboard()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            LoadPatientData();
            LoadDashboardData();
            StartAutoRefresh();
            SetupIcon();
        }

        private void SetupIcon()
        {
            this.picPatientIcon.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.White, 3))
                {
                    e.Graphics.DrawEllipse(pen, 20, 10, 20, 20);
                    e.Graphics.DrawLine(pen, 30, 30, 30, 50);
                    e.Graphics.DrawLine(pen, 30, 35, 15, 45);
                    e.Graphics.DrawLine(pen, 30, 35, 45, 45);
                }
            };
            this.picPatientIcon.Invalidate();
        }

        private void LoadPatientData()
        {
            currentPatient = DataManager.GetCurrentPatient();
            if (currentPatient != null)
            {
                lblWelcome.Text = $"Welcome, {currentPatient.FullName}!";
                lblPatientName.Text = currentPatient.FullName;
            }
            else
            {
                lblWelcome.Text = "Welcome, Patient!";
                lblPatientName.Text = "Patient";
            }
        }

        private void LoadDashboardData()
        {
            if (currentPatient == null) return;

            // Upcoming Appointments
            var upcoming = DataManager.Appointments
                .Where(a => a.PatientId == currentPatient.Id && a.AppointmentDate >= DateTime.Today && a.Status != "Cancelled")
                .OrderBy(a => a.AppointmentDate)
                .Take(3)
                .ToList();

            lvUpcoming.Items.Clear();
            foreach (var apt in upcoming)
            {
                string doctorName = DataManager.Doctors.FirstOrDefault(d => d.Id == apt.DoctorId)?.FullName ?? "Unknown";
                ListViewItem item = new ListViewItem(apt.AppointmentDate.ToString("MMM dd, yyyy"));
                item.SubItems.Add(doctorName);
                item.SubItems.Add(apt.Status);
                item.Tag = apt;
                lvUpcoming.Items.Add(item);
            }

            if (upcoming.Count == 0)
            {
                ListViewItem item = new ListViewItem("No upcoming appointments");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.ForeColor = Color.Gray;
                lvUpcoming.Items.Add(item);
            }

            // Notifications Count
            int unreadCount = DataManager.Notifications
                .Where(n => n.PatientId == currentPatient.Id && !n.IsRead)
                .Count();
            lblNotifications.Text = unreadCount.ToString();

            // Appointments Count
            int totalAppts = DataManager.Appointments.Count(a => a.PatientId == currentPatient.Id);
            lblTotalAppointments.Text = totalAppts.ToString();

            // Medical Records Count
            int recordsCount = DataManager.MedicalRecords.Count(m => m.PatientId == currentPatient.Id);
            lblMedicalRecords.Text = recordsCount.ToString();
        }

        private void StartAutoRefresh()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 30000;
            refreshTimer.Tick += (s, e) => LoadDashboardData();
            refreshTimer.Start();
        }

        // Navigation Methods - These open forms through MainContainer
        private void btnViewDoctors_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new ViewDoctorsForm());
            else
                new ViewDoctorsForm().ShowDialog();
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new BookAppointmentForm());
            else
                new BookAppointmentForm().ShowDialog();
            LoadDashboardData();
        }

        private void btnMyAppointments_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new MyAppointmentsForm());
            else
                new MyAppointmentsForm().ShowDialog();
            LoadDashboardData();
        }

        private void btnMedicalHistory_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new MedicalHistoryForm());
            else
                new MedicalHistoryForm().ShowDialog();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new UserProfileForm());
            else
                new UserProfileForm().ShowDialog();
            LoadPatientData();
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new NotificationsForm());
            else
                new NotificationsForm().ShowDialog();
            LoadDashboardData();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
                Program.MainForm.OpenForm(new GiveFeedbackForm());
            else
                new GiveFeedbackForm().ShowDialog();
        }
    }
}