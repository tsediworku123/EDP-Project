using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class MedicalHistoryForm : Form
    {
        private Patient currentPatient;
        private MedicalRecord selectedRecord;

        public MedicalHistoryForm()
        {
            InitializeComponent();
            currentPatient = DataManager.GetCurrentPatient();
            LoadMedicalHistory();
            SetupPlaceholders();
        }

        private void SetupPlaceholders()
        {
            // Set placeholder text for empty fields
            txtDiagnosis.Text = "Select a record to view diagnosis";
            txtDiagnosis.ForeColor = Color.Gray;

            txtPrescription.Text = "Select a record to view prescription";
            txtPrescription.ForeColor = Color.Gray;

            txtNotes.Text = "Select a record to view notes";
            txtNotes.ForeColor = Color.Gray;
        }

        private void LoadMedicalHistory(string filter = "")
        {
            lvHistory.Items.Clear();

            var allRecords = DataManager.MedicalRecords
                .Where(m => m.PatientId == currentPatient.Id)
                .OrderByDescending(m => m.VisitDate)
                .ToList();

            var records = allRecords;

            if (!string.IsNullOrEmpty(filter))
            {
                records = allRecords.Where(m =>
                    m.DoctorName.ToLower().Contains(filter.ToLower()) ||
                    m.Diagnosis.ToLower().Contains(filter.ToLower()))
                    .ToList();
            }

            foreach (var record in records)
            {
                ListViewItem item = new ListViewItem(record.VisitDate.ToString("MMM dd, yyyy"));
                item.SubItems.Add(record.DoctorName);
                item.SubItems.Add(record.Diagnosis);
                item.SubItems.Add(record.Prescription?.Length > 30 ? record.Prescription.Substring(0, 27) + "..." : record.Prescription ?? "-");
                item.SubItems.Add(record.Notes?.Length > 30 ? record.Notes.Substring(0, 27) + "..." : record.Notes ?? "-");
                item.Tag = record;

                // Alternate row colors
                if (lvHistory.Items.Count % 2 == 0)
                    item.BackColor = Color.FromArgb(250, 250, 255);

                lvHistory.Items.Add(item);
            }

            lblStatus.Text = $"Found {lvHistory.Items.Count} medical records";

            if (lvHistory.Items.Count > 0)
            {
                lvHistory.Items[0].Selected = true;
            }
        }

        private void lvHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count > 0)
            {
                selectedRecord = (MedicalRecord)lvHistory.SelectedItems[0].Tag;
                DisplayRecordDetails(selectedRecord);

                // Highlight selected row
                foreach (ListViewItem item in lvHistory.Items)
                {
                    int index = lvHistory.Items.IndexOf(item);
                    item.BackColor = index % 2 == 0 ?
                        Color.FromArgb(250, 250, 255) : Color.White;
                }
                lvHistory.SelectedItems[0].BackColor = Color.FromArgb(200, 230, 255);
            }
        }

        private void DisplayRecordDetails(MedicalRecord record)
        {
            // Diagnosis
            txtDiagnosis.Text = record.Diagnosis;
            txtDiagnosis.ForeColor = Color.Black;

            // Prescription
            txtPrescription.Text = record.Prescription ?? "No prescription recorded";
            txtPrescription.ForeColor = Color.Black;

            // Notes
            txtNotes.Text = record.Notes ?? "No additional notes";
            txtNotes.ForeColor = Color.Black;

            // Update visit info
            lblVisitInfo.Text = $"Visit on {record.VisitDate:MMMM dd, yyyy} with {record.DoctorName}";
            lblVisitInfo.ForeColor = Color.FromArgb(0, 105, 148);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadMedicalHistory(searchTerm);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                LoadMedicalHistory();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadMedicalHistory();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lvHistory.Items.Count == 0)
            {
                MessageBox.Show("No records to export.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt";
            saveDialog.FileName = $"MedicalHistory_{DateTime.Now:yyyyMMdd}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Create CSV content
                var lines = new System.Collections.Generic.List<string>();
                lines.Add("Date,Doctor,Diagnosis,Prescription,Notes");

                foreach (ListViewItem item in lvHistory.Items)
                {
                    var record = (MedicalRecord)item.Tag;
                    lines.Add($"\"{record.VisitDate:yyyy-MM-dd}\",\"{record.DoctorName}\",\"{record.Diagnosis}\",\"{record.Prescription}\",\"{record.Notes}\"");
                }

                System.IO.File.WriteAllLines(saveDialog.FileName, lines);
                MessageBox.Show("Medical history exported successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (selectedRecord == null)
            {
                MessageBox.Show("Please select a record to print.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Simple print simulation
            string printContent = $"MEDICAL RECORD\n\n" +
                $"Date: {selectedRecord.VisitDate:MMMM dd, yyyy}\n" +
                $"Doctor: {selectedRecord.DoctorName}\n" +
                $"Diagnosis: {selectedRecord.Diagnosis}\n" +
                $"Prescription: {selectedRecord.Prescription}\n" +
                $"Notes: {selectedRecord.Notes}\n\n" +
                $"--- End of Record ---";

            MessageBox.Show(printContent, "Print Preview",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}