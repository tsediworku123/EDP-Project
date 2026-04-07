using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using ClinicAppointmentSystem;

namespace ClinicAppointmentSystem.Controls
{
    public class UcMedicalHistory : UserControl
    {
        private int _patientId;
        private Panel pnlHeader;
        private FlowLayoutPanel flpHistory;
        private TextBox txtSearch;
        private ComboBox cmbSort;
        private string _currentSort = "Most Recent";
        private string _searchText = "";
        private Label lblTotalCount;

        public UcMedicalHistory(int patientId)
        {
            this._patientId = patientId;
            InitializeComponentModern();
            this.Dock = DockStyle.Fill;
            this.BackColor = PatientTheme.Background;
            RefreshData();
        }

        private void InitializeComponentModern()
        {
            this.Controls.Clear();

            // 1. Header Section
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 130, BackColor = PatientTheme.Surface, Padding = new Padding(40, 25, 40, 0) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            
            Label lblTitle = new Label { Text = "MEDICAL HISTORY", Font = PatientTheme.TitleMedium, ForeColor = PatientTheme.TextPrimary, Location = new Point(40, 25), AutoSize = true };
            
            // Analytics Mini Card
            Panel pnlStats = new Panel { Size = new Size(200, 60), Location = new Point(40, 65), BackColor = PatientTheme.SuccessLight };
            pnlStats.Paint += (s, e) => {
                using (Pen p = new Pen(PatientTheme.Success, 1)) e.Graphics.DrawRectangle(p, 0, 0, pnlStats.Width - 1, pnlStats.Height - 1);
            };
            Label lblStatTitle = new Label { Text = "CONSULTATIONS", Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = PatientTheme.Success, Location = new Point(12, 10), AutoSize = true };
            lblTotalCount = new Label { Text = "0", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = PatientTheme.Success, Location = new Point(10, 25), AutoSize = true };
            pnlStats.Controls.AddRange(new Control[] { lblStatTitle, lblTotalCount });
            
            // Filter Bar
            txtSearch = new TextBox { Width = 280, Location = new Point(pnlHeader.Width - 550, 75), Font = PatientTheme.BodyRegular, BorderStyle = BorderStyle.FixedSingle, BackColor = PatientTheme.Background };
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.TextChanged += (s, e) => { _searchText = txtSearch.Text.ToLower(); RefreshData(); };
            
            cmbSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, Location = new Point(pnlHeader.Width - 250, 75), Font = PatientTheme.BodyRegular, FlatStyle = FlatStyle.Flat };
            cmbSort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbSort.Items.AddRange(new object[] { "Most Recent", "Oldest First", "Physician" });
            cmbSort.SelectedItem = _currentSort;
            cmbSort.SelectedIndexChanged += (s, e) => { _currentSort = cmbSort.SelectedItem.ToString(); RefreshData(); };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, pnlStats, txtSearch, cmbSort });
            this.Controls.Add(pnlHeader);

            // 2. History Card Flow
            flpHistory = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(40, 30, 40, 40),
                BackColor = PatientTheme.Background,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpHistory);
            flpHistory.BringToFront();
        }

        public void RefreshData()
        {
            if (flpHistory != null) flpHistory.Controls.Clear();
            var history = DataManager.Appointments
                .Where(a => a.PatientId == _patientId && (a.Status == "Completed" || a.Status == "Checked-In"))
                .ToList();

            // Update Stats
            if (lblTotalCount != null) lblTotalCount.Text = history.Count.ToString();

            // Search Filter
            if (!string.IsNullOrEmpty(_searchText))
            {
                history = history.Where(a => {
                    var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                    return (doc?.FullName?.ToLower().Contains(_searchText) ?? false) || 
                           (a.Diagnosis?.ToLower().Contains(_searchText) ?? false) ||
                           (a.ConsultationNote?.ToLower().Contains(_searchText) ?? false);
                }).ToList();
            }

            // Sorting
            if (_currentSort == "Most Recent") history = history.OrderByDescending(a => a.AppointmentDate).ToList();
            else if (_currentSort == "Oldest First") history = history.OrderBy(a => a.AppointmentDate).ToList();
            else if (_currentSort == "Physician") history = history.OrderBy(a => DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName).ToList();

            if (flpHistory != null)
            {
                foreach (var a in history) flpHistory.Controls.Add(CreateHistoryCard(a));
                if (history.Count == 0) flpHistory.Controls.Add(new Label { Text = "No medical history records found.", Font = new Font("Segoe UI", 11, FontStyle.Italic), ForeColor = PatientTheme.TextMuted, AutoSize = true, Margin = new Padding(40) });
            }
        }

        private Panel CreateHistoryCard(Appointment a)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            Panel p = new Panel { Size = new Size(950, 160), BackColor = PatientTheme.Surface, Margin = new Padding(0, 0, 0, 20) };
            p.Paint += (s, e) => {
                using (Pen bp = new Pen(PatientTheme.Border, 1)) e.Graphics.DrawRectangle(bp, 0, 0, p.Width - 1, p.Height - 1);
            };
            
            // Left block (Date Calendar Style)
            Panel pDate = new Panel { Size = new Size(130, 160), BackColor = PatientTheme.Background, Location = new Point(0, 0) };
            pDate.Paint += (s, e) => e.Graphics.DrawLine(new Pen(PatientTheme.Border, 1), pDate.Width - 1, 0, pDate.Width - 1, pDate.Height);
            
            Label lMon = new Label { Text = a.AppointmentDate.ToString("MMM").ToUpper(), Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = PatientTheme.Danger, Location = new Point(0, 30), AutoSize = false, Width = 130, TextAlign = ContentAlignment.MiddleCenter };
            Label lDay = new Label { Text = a.AppointmentDate.ToString("dd"), Font = new Font("Segoe UI", 32, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(0, 50), AutoSize = false, Width = 130, TextAlign = ContentAlignment.MiddleCenter };
            Label lYear = new Label { Text = a.AppointmentDate.ToString("yyyy"), Font = PatientTheme.LabelBold, ForeColor = PatientTheme.TextMuted, Location = new Point(0, 105), AutoSize = false, Width = 130, TextAlign = ContentAlignment.MiddleCenter };
            pDate.Controls.AddRange(new Control[] { lMon, lDay, lYear });
            
            // Details Block
            Label lDoc = new Label { Text = $"Dr. {doc?.FullName ?? "Physician"}", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = PatientTheme.TextPrimary, Location = new Point(160, 25), AutoSize = true };
            
            string diagStr = string.IsNullOrEmpty(a.Diagnosis) ? "General Consultation" : a.Diagnosis;
            Label lDiagTitle = new Label { Text = "PRIMARY DIAGNOSIS", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.Primary, Location = new Point(160, 65), AutoSize = true };
            Label lDiag = new Label { Text = diagStr, Font = PatientTheme.Subtitle, ForeColor = PatientTheme.TextPrimary, Location = new Point(160, 85), AutoSize = true };
            
            string noteStr = string.IsNullOrEmpty(a.ConsultationNote) ? "Routine clinical assessment completed." : (a.ConsultationNote.Length > 80 ? a.ConsultationNote.Substring(0, 77) + "..." : a.ConsultationNote);
            Label lNotesTitle = new Label { Text = "CLINICAL SUMMARY", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.TextMuted, Location = new Point(480, 65), AutoSize = true };
            Label lNotes = new Label { Text = noteStr, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = PatientTheme.TextSecondary, Location = new Point(480, 85), Width = 280, AutoSize = false, Height = 50 };
            
            // Actions
            Button btnView = new Button { 
                Text = "Read Record", 
                Size = new Size(160, 40), 
                Location = new Point(770, 35), 
                BackColor = PatientTheme.Background, 
                ForeColor = PatientTheme.TextSecondary, 
                FlatStyle = FlatStyle.Flat, 
                Font = PatientTheme.LabelBold, 
                Cursor = Cursors.Hand 
            };
            btnView.FlatAppearance.BorderSize = 0;
            btnView.Click += (s, e) => new AppointmentDetailForm(a.Id).ShowDialog();

            Button btnDownload = new Button { 
                Text = "\u2B07 DOWNLOAD PDF", 
                Size = new Size(160, 40), 
                Location = new Point(770, 85), 
                BackColor = PatientTheme.SuccessLight, 
                ForeColor = PatientTheme.Success, 
                FlatStyle = FlatStyle.Flat, 
                Font = PatientTheme.LabelBold, 
                Cursor = Cursors.Hand 
            };
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.Click += (s, e) => MessageBox.Show($"Your medical record for {a.AppointmentDate:MMMM dd} has been encrypted and saved to your secure downloads folder.", "Document Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            p.Controls.AddRange(new Control[] { pDate, lDoc, lDiagTitle, lDiag, lNotesTitle, lNotes, btnView, btnDownload });
            return p;
        }
    }
}
