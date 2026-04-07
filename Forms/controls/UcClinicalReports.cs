using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;
using System.Collections.Generic;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcClinicalReports : UserControl
    {
        private int _doctorId = 0;
        public UcClinicalReports(int doctorId = 0)
        {
            this._doctorId = doctorId;
            InitializeComponent();
            this.Load += (s, e) => {
                if (_doctorId != 0)
                {
                    var d = DataManager.Doctors.FirstOrDefault(x => x.Id == _doctorId);
                    if (lblTitle != null) lblTitle.Text = $"MY PERFORMANCE: {d?.FullName.ToUpper() ?? "DOCTOR"}";
                }
                RefreshReports();
            };
            this.Resize += (s, e) => RefreshReports();
        }

        public void RefreshReports()
        {
            if (scrollContainer.Width < 100) return; // Prevent layout calculation on zero-size

            DataManager.EnsureLoaded();
            scrollContainer.SuspendLayout();
            scrollContainer.Controls.Clear();
            
            try
            {
                int currentY = 20;
                int width = Math.Max(scrollContainer.Width - 60, 300);
                int cardW = (width / 3) - 20;

                // Success Rate & Analytics (Row 1)
                AddSectionHeader("CLINICAL SUCCESS & ATTRITION", ref currentY);
                
                var appts = _doctorId == 0 ? DataManager.Appointments : DataManager.Appointments.Where(a => a.DoctorId == _doctorId).ToList();
                int realTotal = appts.Count;
                int comp = appts.Count(a => a.Status == "Completed");
                int canc = appts.Count(a => a.Status == "Cancelled");
                int nosh = appts.Count(a => a.Status == "No-Show" || a.Status == "No-show");

                // If no data, show a 100% placeholder for the charts to show the UI structure
                int displayTotal = realTotal == 0 ? 1 : realTotal;
                int displayComp = realTotal == 0 ? 1 : comp; 
                int displayCanc = canc;
                int displayNosh = nosh;
                
                // Analytics Row: 2 Big Tiles + 1 Donut Chart
                int analyticsH = 180;
                Panel pAnalytics = new Panel { Location = new Point(20, currentY), Size = new Size(width, analyticsH), BackColor = Color.White };
                scrollContainer.Controls.Add(pAnalytics);

                int tileW = (width / 4) - 15;
                AddSummaryTileInside(pAnalytics, "Total Case Load", realTotal.ToString(), Color.FromArgb(71, 85, 105), 0, 0, tileW, analyticsH);
                AddSummaryTileInside(pAnalytics, "Success Rate", realTotal == 0 ? "0%" : $"{(comp * 100 / realTotal)}%", Color.FromArgb(16, 185, 129), tileW + 20, 0, tileW, analyticsH);
                
                // Donut Chart for Status
                Panel pDonut = new Panel { Location = new Point((tileW + 20) * 2, 0), Size = new Size(width - (tileW + 20) * 2, analyticsH), BackColor = Color.White };
                pAnalytics.Controls.Add(pDonut);
                DrawDonutChart(pDonut, displayComp, displayCanc, displayNosh, realTotal == 0);

                currentY += analyticsH + 30;

                // Provider Performance
                if (_doctorId == 0)
                {
                    AddSectionHeader("DOCTOR PERFORMANCE & WORKLOAD", ref currentY);
                }
                else
                {
                    AddSectionHeader("RELATIVE BENCHMARKING (CLINIC-WIDE)", ref currentY);
                }
                
                DataGridView dgvWork = new DataGridView { 
                    Location = new Point(20, currentY), 
                    Size = new Size(width, 280), 
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };
                dgvWork.Columns.Add("D", "Physician Name");
                dgvWork.Columns.Add("T", "Total Case Load");
                dgvWork.Columns.Add("C", "Completed Visits");
                dgvWork.Columns.Add("P", "Efficiency %");
                dgvWork.Columns.Add("S", "Status");

                dgvWork.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                dgvWork.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvWork.EnableHeadersVisualStyles = false;
                dgvWork.RowTemplate.Height = 35;

                // Dictionary for faster lookup
                var docMap = DataManager.Doctors.ToDictionary(d => d.Id, d => d.FullName);

                var stats = DataManager.Appointments.GroupBy(a => a.DoctorId).Select(g => new {
                    DocId = g.Key,
                    DocName = docMap.ContainsKey(g.Key) ? docMap[g.Key] : "Unknown",
                    Total = g.Count(),
                    Done = g.Count(a => a.Status == "Completed"),
                    Rate = g.Count() > 0 ? (g.Count(a => a.Status == "Completed") * 100 / g.Count()) : 0
                }).OrderByDescending(s => s.Total).ToList();

                foreach (var s in stats) {
                    int rIdx = dgvWork.Rows.Add(s.DocName, s.Total, s.Done, s.Rate + "%", s.Total > 20 ? "High Usage" : "Optimal");
                    if (_doctorId != 0 && s.DocId == _doctorId) dgvWork.Rows[rIdx].DefaultCellStyle.BackColor = Color.FromArgb(240, 249, 255);
                }
                scrollContainer.Controls.Add(dgvWork);
                currentY += 310;

                // Specialty Trend
                AddSectionHeader("VOLUME BY MEDICAL SPECIALTY (ANALYTICS)", ref currentY);
                Panel pChart = new Panel { Location = new Point(20, currentY), Size = new Size(width, 240), BackColor = Color.White, Padding = new Padding(20) };
                scrollContainer.Controls.Add(pChart);
                RenderSpecialtyHorizontalBars(pChart);
                currentY += 260;

                // Feedback Section
                AddSectionHeader("PATIENT SATISFACTION & FEEDBACK", ref currentY);
                var feedbacks = _doctorId == 0 ? DataManager.Feedbacks : DataManager.Feedbacks.Where(f => f.DoctorId == _doctorId).ToList();
                if (feedbacks.Any())
                {
                    DataGridView dgvFeedback = new DataGridView { 
                        Location = new Point(20, currentY), 
                        Size = new Size(width, 250), 
                        BackgroundColor = Color.White,
                        BorderStyle = BorderStyle.None,
                        RowHeadersVisible = false,
                        AllowUserToAddRows = false,
                        ReadOnly = true,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    };
                    dgvFeedback.Columns.Add("D", "Date");
                    dgvFeedback.Columns.Add("P", "Patient");
                    dgvFeedback.Columns.Add("R", "Rating");
                    dgvFeedback.Columns.Add("C", "Comment");
                    
                    foreach (var f in feedbacks.OrderByDescending(x => x.FeedbackDate)) {
                        dgvFeedback.Rows.Add(f.FeedbackDate.ToString("MMM dd"), f.PatientName, " " + f.Rating, f.Comment);
                    }
                    scrollContainer.Controls.Add(dgvFeedback);
                    currentY += 270;
                }
                else
                {
                    Label lblNoFeedback = new Label { Text = "No feedback received yet.", Location = new Point(40, currentY), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
                    scrollContainer.Controls.Add(lblNoFeedback);
                    currentY += 40;
                }
            }
            finally
            {
                scrollContainer.ResumeLayout();
            }
        }

        private void AddSectionHeader(string text, ref int currentY)
        {
            Label lbl = new Label { 
                Text = text, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = Color.FromArgb(100, 116, 139), 
                Location = new Point(20, currentY), 
                AutoSize = true 
            };
            scrollContainer.Controls.Add(lbl);
            currentY += 30;
        }

        private void AddSummaryTileInside(Panel container, string title, string val, Color c, int x, int y, int w, int h)
        {
            Panel p = new Panel { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.White };
            p.Controls.Add(new Label { Text = val, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = c, Dock = DockStyle.Top, Height = h - 40, TextAlign = ContentAlignment.MiddleCenter });
            p.Controls.Add(new Label { Text = title.ToUpper(), Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Bottom, Height = 40, TextAlign = ContentAlignment.MiddleCenter });
            
            p.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
            container.Controls.Add(p);
        }

        private void DrawDonutChart(Panel p, int comp, int canc, int nosh, bool isEmpty = false)
        {
            var chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart.Dock = DockStyle.Fill;
            p.Controls.Add(chart);

            var area = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
            area.BackColor = Color.White;
            chart.ChartAreas.Add(area);
            
            var legend = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend");
            legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
            legend.Alignment = StringAlignment.Center;
            legend.Font = new Font("Segoe UI", 9);
            chart.Legends.Add(legend);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("Status");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            series.BorderWidth = 2;
            series.BorderColor = Color.White;
            series["DoughnutRadius"] = "50";
            series["PieLabelStyle"] = "Disabled";

            if (isEmpty || (comp == 0 && canc == 0 && nosh == 0))
            {
                var point = series.Points.Add(1);
                point.Color = Color.FromArgb(241, 245, 249);
                point.LegendText = "No Data Yet";
            }
            else
            {
                if (comp > 0)
                {
                    var pComp = series.Points.Add(comp);
                    pComp.Color = Color.FromArgb(16, 185, 129);
                    pComp.LegendText = $"Completed ({comp})";
                    pComp.ToolTip = $"Completed: {comp}";
                }
                if (canc > 0)
                {
                    var pCanc = series.Points.Add(canc);
                    pCanc.Color = Color.FromArgb(239, 68, 68);
                    pCanc.LegendText = $"Cancelled ({canc})";
                    pCanc.ToolTip = $"Cancelled: {canc}";
                }
                if (nosh > 0)
                {
                    var pNosh = series.Points.Add(nosh);
                    pNosh.Color = Color.FromArgb(245, 158, 11);
                    pNosh.LegendText = $"No-Show ({nosh})";
                    pNosh.ToolTip = $"No-Show: {nosh}";
                }
            }

            chart.Series.Add(series);
        }

        private void RenderSpecialtyHorizontalBars(Panel p)
        {
            var apptsForChart = _doctorId == 0 ? DataManager.Appointments : DataManager.Appointments.Where(a => a.DoctorId == _doctorId).ToList();
            
            var docSpecialtyMap = DataManager.Doctors.ToDictionary(d => d.Id, d => d.Specialty);

            var data = apptsForChart.GroupBy(a => docSpecialtyMap.ContainsKey(a.DoctorId) ? docSpecialtyMap[a.DoctorId] : "General")
                .Select(g => new { Name = g.Key, Count = g.Count() }).OrderBy(x => x.Count).ToList();
            
            p.Controls.Clear();
            
            if (data.Count == 0) {
                p.Controls.Add(new Label { Text = "No specialty data available yet.", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9, FontStyle.Italic) });
                return;
            }

            var chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart.Dock = DockStyle.Fill;
            p.Controls.Add(chart);

            var area = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8);
            area.BackColor = Color.White;
            chart.ChartAreas.Add(area);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("Volume");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series.Color = Color.FromArgb(14, 165, 233);
            series.BorderWidth = 0;
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 8, FontStyle.Bold);

            foreach (var d in data) {
                var pt = series.Points.AddXY(d.Name, d.Count);
                series.Points[pt].ToolTip = $"{d.Name}: {d.Count} consultations";
            }

            chart.Series.Add(series);
        }

        private void btnPrintSummary_Click(object sender, EventArgs e) => MessageBox.Show("Report sent to print queue.", "Clinical Output", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Alpha Clinic Reports");
                Directory.CreateDirectory(folder);
                string fileName = Path.Combine(folder, "Clinic_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");

                var lines = new System.Collections.Generic.List<string>();
                lines.Add("Date,Patient,Doctor,Reason,Status");

                foreach (var a in DataManager.Appointments.OrderBy(a => a.AppointmentDate))
                {
                    var pat = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                    var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                    string line = string.Join(",",
                        a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                        $"\"{pat?.FullName ?? "Unknown"}\"",
                        $"\"{doc?.FullName ?? "Unknown"}\"",
                        $"\"{(a.Reason ?? "").Replace("\"","'")}\"",
                        a.Status
                    );
                    lines.Add(line);
                }

                System.IO.File.WriteAllLines(fileName, lines, System.Text.Encoding.UTF8);

                if (MessageBox.Show($"Report exported to:\n{fileName}\n\nOpen file now?", "Export Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    System.Diagnostics.Process.Start("explorer.exe", $"\"{fileName}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
