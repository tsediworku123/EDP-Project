using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcConsultationPanel : UserControl
    {
        private int _apptId;
        private Appointment _appt;
        private Patient _patient;

        public UcConsultationPanel(int apptId)
        {
            this._apptId = apptId;
            this._appt = DataManager.Appointments.FirstOrDefault(a => a.Id == apptId);
            this._patient = DataManager.Patients.FirstOrDefault(p => p.Id == _appt?.PatientId);
            
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // Enable Scrolling for long clinical notes
            this.pnlRight.AutoScroll = true;
            
            // Connect Follow-up Logic
            this.chkFollowUp.CheckedChanged += (s, e) => this.dtpFollowUp.Enabled = this.chkFollowUp.Checked;
            
            // Connect Save/Finalize Button
            this.btnSave.Click += btnSave_Click;
            
            LoadClinicalData();
        }

        private void LoadClinicalData()
        {
            if (_appt == null || _patient == null) return;

            // Populate Patient Info
            this.lblPatientName.Text = _patient.FullName;
            int age = DateTime.Today.Year - _patient.DateOfBirth.Year;
            this.lblPatientAge.Text = $"Age: {age} | Gender: {_patient.Gender} | Phone: {_patient.Phone}";
            this.lblReason.Text = $"CHIEF COMPLAINT:\n{_appt.Reason}";

            // Populate Clinical Fields
            this.txtNotes.Text = _appt.ClinicalNotes;
            this.txtDiagnosis.Text = _appt.Diagnosis;
            this.txtPrescription.Text = _appt.Prescription;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_appt == null) return;

            // Validate
            if (string.IsNullOrWhiteSpace(this.txtDiagnosis.Text)) {
                MessageBox.Show("Please provide a diagnosis before completing the consultation.", "Clinical Requirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save Data
            _appt.ClinicalNotes = this.txtNotes.Text;
            _appt.Diagnosis = this.txtDiagnosis.Text;
            _appt.Prescription = this.txtPrescription.Text;
            _appt.Status = "Completed";
            _appt.CompletionTime = DateTime.Now;

            DataManager.SaveAppointments();
            MessageBox.Show("Clinical Consultation Finalized Successfully.", "Clinic System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Return to Schedule
            (this.FindForm() as DoctorDashboard)?.LoadTodayPatients();
        }
    }
}
