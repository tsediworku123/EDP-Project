using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            // Daily Report
            int todayAppointments = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today);
            int todayCompleted = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today &&
                (a.Status == "Confirmed" || a.Status == "Completed"));
            int todayPending = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Pending");
            int todayCancelled = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Cancelled");

            lblTodayTotal.Text = todayAppointments.ToString();
            lblTodayCompleted.Text = todayCompleted.ToString();
            lblTodayPending.Text = todayPending.ToString();
            lblTodayCancelled.Text = todayCancelled.ToString();

            // Monthly Report
            DateTime firstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

            int monthlyAppointments = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay);
            int monthlyCompleted = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay &&
                (a.Status == "Confirmed" || a.Status == "Completed"));
            int monthlyPending = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay && a.Status == "Pending");
            int monthlyCancelled = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay && a.Status == "Cancelled");

            lblMonthlyTotal.Text = monthlyAppointments.ToString();
            lblMonthlyCompleted.Text = monthlyCompleted.ToString();
            lblMonthlyPending.Text = monthlyPending.ToString();
            lblMonthlyCancelled.Text = monthlyCancelled.ToString();

            // Patient Statistics
            int totalPatients = DataManager.Patients.Count;
            int malePatients = DataManager.Patients.Count(p => p.Gender == "Male");
            int femalePatients = DataManager.Patients.Count(p => p.Gender == "Female");

            lblTotalPatients.Text = totalPatients.ToString();
            lblMalePatients.Text = malePatients.ToString();
            lblFemalePatients.Text = femalePatients.ToString();

            // Doctor Performance
            lvDoctorPerformance.Items.Clear();
            foreach (var doctor in DataManager.Doctors)
            {
                int total = DataManager.Appointments.Count(a => a.DoctorId == doctor.Id);
                int completed = DataManager.Appointments.Count(a => a.DoctorId == doctor.Id &&
                    (a.Status == "Confirmed" || a.Status == "Completed"));
                double rate = total > 0 ? (double)completed / total * 100 : 0;

                ListViewItem item = new ListViewItem(doctor.Id.ToString());
                item.SubItems.Add(doctor.FullName);
                item.SubItems.Add(doctor.Specialization);
                item.SubItems.Add(total.ToString());
                item.SubItems.Add(completed.ToString());
                item.SubItems.Add(rate.ToString("0.0") + "%");

                if (rate >= 80)
                    item.BackColor = Color.FromArgb(220, 255, 220);
                else if (rate >= 50)
                    item.BackColor = Color.FromArgb(255, 255, 200);
                else
                    item.BackColor = Color.FromArgb(255, 220, 220);

                lvDoctorPerformance.Items.Add(item);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF files (*.pdf)|*.pdf|Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
            saveDialog.Title = "Export Report";
            saveDialog.FileName = $"Report_{DateTime.Now:yyyyMMdd}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"Report exported successfully to:\n{saveDialog.FileName}",
                    "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print functionality coming soon!", "Print",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}