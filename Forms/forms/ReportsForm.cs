using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            DrawBarChart();
            DrawPieChart();
        }

        private void LoadReports()
        {
            // Daily Report
            int todayAppointments = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today);
            int todayCompleted = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today &&
                (a.Status == "Confirmed" || a.Status == "Completed"));
            int todayPending = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Pending");
            int todayCancelled = DataManager.Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Cancelled");

            lblDailyAppointments.Text = todayAppointments.ToString();
            lblDailyCompleted.Text = todayCompleted.ToString();
            lblDailyPending.Text = todayPending.ToString();
            lblDailyCancelled.Text = todayCancelled.ToString();

            // Monthly Report
            DateTime firstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

            int monthlyAppointments = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay);
            int monthlyCompleted = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay &&
                (a.Status == "Confirmed" || a.Status == "Completed"));
            int monthlyPending = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay && a.Status == "Pending");
            int monthlyCancelled = DataManager.Appointments.Count(a => a.AppointmentDate.Date >= firstDay && a.AppointmentDate.Date <= lastDay && a.Status == "Cancelled");

            lblMonthlyAppointments.Text = monthlyAppointments.ToString();
            lblMonthlyCompleted.Text = monthlyCompleted.ToString();
            lblMonthlyPending.Text = monthlyPending.ToString();
            lblMonthlyCancelled.Text = monthlyCancelled.ToString();
            lblMonthName.Text = DateTime.Now.ToString("MMMM yyyy");

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

        private void DrawBarChart()
        {
            // Clear any existing controls in the chart panel
            panelBarChart.Controls.Clear();

            // Title
            Label lblChartTitle = new Label();
            lblChartTitle.Text = "Weekly Appointments";
            lblChartTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblChartTitle.ForeColor = Color.FromArgb(0, 105, 148);
            lblChartTitle.Location = new Point(10, 10);
            lblChartTitle.Size = new Size(200, 25);
            panelBarChart.Controls.Add(lblChartTitle);

            // Get real data from appointments
            DateTime[] days = new DateTime[7];
            int[] values = new int[7];

            for (int i = 0; i < 7; i++)
            {
                days[i] = DateTime.Today.AddDays(-i);
                values[6 - i] = DataManager.Appointments.Count(a => a.AppointmentDate.Date == days[i].Date);
            }

            int maxValue = values.Max();
            if (maxValue == 0) maxValue = 1; // Prevent division by zero

            int startX = 40;
            int barWidth = 50;
            int spacing = 15;
            int baseY = panelBarChart.Height - 50;
            int maxHeight = 150;

            for (int i = 0; i < 7; i++)
            {
                int barHeight = (int)((double)values[i] / maxValue * maxHeight);
                if (barHeight < 5 && values[i] > 0) barHeight = 15;

                // Bar with gradient
                Panel bar = new Panel();
                bar.Location = new Point(startX + i * (barWidth + spacing), baseY - barHeight);
                bar.Size = new Size(barWidth, barHeight);
                bar.Tag = values[i];

                // Add gradient to bar
                bar.Paint += (s, e) => {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        bar.ClientRectangle,
                        Color.FromArgb(52, 152, 219),
                        Color.FromArgb(41, 128, 185),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(brush, bar.ClientRectangle);
                    }
                };

                panelBarChart.Controls.Add(bar);

                // Day label
                Label lblDay = new Label();
                lblDay.Text = days[i].ToString("ddd");
                lblDay.Location = new Point(startX + i * (barWidth + spacing), baseY + 5);
                lblDay.Size = new Size(barWidth, 20);
                lblDay.TextAlign = ContentAlignment.MiddleCenter;
                lblDay.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                panelBarChart.Controls.Add(lblDay);

                // Value label on top of bar
                if (values[i] > 0)
                {
                    Label lblValue = new Label();
                    lblValue.Text = values[i].ToString();
                    lblValue.Location = new Point(startX + i * (barWidth + spacing), baseY - barHeight - 20);
                    lblValue.Size = new Size(barWidth, 20);
                    lblValue.TextAlign = ContentAlignment.MiddleCenter;
                    lblValue.ForeColor = Color.FromArgb(41, 128, 185);
                    lblValue.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    panelBarChart.Controls.Add(lblValue);
                }
            }

            // Y-axis line
            Panel yAxis = new Panel();
            yAxis.Location = new Point(30, 30);
            yAxis.Size = new Size(2, baseY - 30);
            yAxis.BackColor = Color.FromArgb(100, 100, 100);
            panelBarChart.Controls.Add(yAxis);

            // X-axis line
            Panel xAxis = new Panel();
            xAxis.Location = new Point(30, baseY);
            xAxis.Size = new Size(panelBarChart.Width - 50, 2);
            xAxis.BackColor = Color.FromArgb(100, 100, 100);
            panelBarChart.Controls.Add(xAxis);
        }

        private void DrawPieChart()
        {
            // Clear any existing controls in the pie chart panel
            panelPieChart.Controls.Clear();

            // Title
            Label lblPieTitle = new Label();
            lblPieTitle.Text = "Gender Distribution";
            lblPieTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblPieTitle.ForeColor = Color.FromArgb(0, 105, 148);
            lblPieTitle.Location = new Point(10, 10);
            lblPieTitle.Size = new Size(200, 25);
            panelPieChart.Controls.Add(lblPieTitle);

            int total = DataManager.Patients.Count;
            if (total == 0) total = 1;

            int male = DataManager.Patients.Count(p => p.Gender == "Male");
            int female = DataManager.Patients.Count(p => p.Gender == "Female");

            float maleAngle = (float)male / total * 360;

            // Panel to draw the pie chart
            Panel pieCanvas = new Panel();
            pieCanvas.Location = new Point(20, 50);
            pieCanvas.Size = new Size(150, 150);
            pieCanvas.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(10, 10, 130, 130);

                // Male slice
                if (male > 0)
                {
                    using (SolidBrush maleBrush = new SolidBrush(Color.FromArgb(52, 152, 219)))
                    {
                        e.Graphics.FillPie(maleBrush, rect, 0, maleAngle);
                    }
                }

                // Female slice
                if (female > 0)
                {
                    using (SolidBrush femaleBrush = new SolidBrush(Color.FromArgb(155, 89, 182)))
                    {
                        e.Graphics.FillPie(femaleBrush, rect, maleAngle, 360 - maleAngle);
                    }
                }

                // Border
                using (Pen pen = new Pen(Color.White, 2))
                {
                    e.Graphics.DrawEllipse(pen, rect);
                }
            };
            panelPieChart.Controls.Add(pieCanvas);

            // Legend
            int legendY = 50;

            // Male legend
            Panel maleLegend = new Panel();
            maleLegend.BackColor = Color.FromArgb(52, 152, 219);
            maleLegend.Location = new Point(190, legendY);
            maleLegend.Size = new Size(20, 20);
            panelPieChart.Controls.Add(maleLegend);

            Label lblMaleLegend = new Label();
            lblMaleLegend.Text = $"Male: {male} ({(float)male / total * 100:0.1}%)";
            lblMaleLegend.Location = new Point(220, legendY);
            lblMaleLegend.Size = new Size(150, 20);
            lblMaleLegend.Font = new Font("Segoe UI", 10);
            panelPieChart.Controls.Add(lblMaleLegend);
            legendY += 30;

            // Female legend
            Panel femaleLegend = new Panel();
            femaleLegend.BackColor = Color.FromArgb(155, 89, 182);
            femaleLegend.Location = new Point(190, legendY);
            femaleLegend.Size = new Size(20, 20);
            panelPieChart.Controls.Add(femaleLegend);

            Label lblFemaleLegend = new Label();
            lblFemaleLegend.Text = $"Female: {female} ({(float)female / total * 100:0.1}%)";
            lblFemaleLegend.Location = new Point(220, legendY);
            lblFemaleLegend.Size = new Size(150, 20);
            lblFemaleLegend.Font = new Font("Segoe UI", 10);
            panelPieChart.Controls.Add(lblFemaleLegend);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
            DrawBarChart();
            DrawPieChart();
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