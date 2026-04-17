using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorDischargeViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private AppointmentItem _selectedAppointment;
        private string _dischargeSummary;
        private string _followUpInstructions;
        private string _prescriptionSummary;
        private string _dischargeCondition;
        private string _statusMsg;
        private bool _hasStatusMsg;
        private bool _hasError;

        public ObservableCollection<AppointmentItem> CompletedAppointments { get; } = new ObservableCollection<AppointmentItem>();
        public bool HasNoAppointments => CompletedAppointments.Count == 0;

        public ObservableCollection<string> ConditionOptions { get; } = new ObservableCollection<string>
        {
            "Stable", "Improved", "Critical – Referred", "Unchanged", "Recovered", "Deceased"
        };

        public AppointmentItem SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                SetProperty(ref _selectedAppointment, value);
                OnPropertyChanged(nameof(HasSelection));
                OnPropertyChanged(nameof(SelectedPatientName));
                OnPropertyChanged(nameof(SelectedAppointmentInfo));
                if (value != null) PreFillFromAppointment(value);
                StatusMsg = string.Empty;
                HasStatusMsg = false;
            }
        }

        public bool HasSelection => _selectedAppointment != null;
        public string SelectedPatientName => _selectedAppointment?.PatientName ?? string.Empty;
        public string SelectedAppointmentInfo => _selectedAppointment != null
            ? $"Appointment #{_selectedAppointment.Source.Id} · {_selectedAppointment.Source.AppointmentDate:dd MMM yyyy}"
            : string.Empty;

        public string DischargeSummary { get => _dischargeSummary; set => SetProperty(ref _dischargeSummary, value); }
        public string FollowUpInstructions { get => _followUpInstructions; set => SetProperty(ref _followUpInstructions, value); }
        public string PrescriptionSummary { get => _prescriptionSummary; set => SetProperty(ref _prescriptionSummary, value); }
        public string DischargeCondition { get => _dischargeCondition; set => SetProperty(ref _dischargeCondition, value); }
        public string StatusMsg { get => _statusMsg; set => SetProperty(ref _statusMsg, value); }
        public bool HasStatusMsg { get => _hasStatusMsg; set => SetProperty(ref _hasStatusMsg, value); }
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }

        public ICommand SaveDischargeCommand { get; }

        public DoctorDischargeViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            DischargeCondition = ConditionOptions[0];
            LoadAppointments();
            SaveDischargeCommand = new RelayCommand(SaveDischarge);
        }

        private void LoadAppointments()
        {
            CompletedAppointments.Clear();
            if (_doctor == null) return;

            // Show today's + recent completed appointments
            var appts = DataManager.Appointments
                .Where(a => a.DoctorId == _doctor.Id
                         && (a.Status == "Completed" || a.AppointmentDate.Date == DateTime.Today)
                         && a.Status != "Cancelled"
                         && a.AppointmentDate >= DateTime.Today.AddDays(-7))
                .OrderByDescending(a => a.AppointmentDate);

            foreach (var a in appts)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                CompletedAppointments.Add(new AppointmentItem
                {
                    Source = a,
                    PatientName = patient?.FullName ?? "Unknown Patient",
                    TimeDisplay = a.AppointmentDate.ToString("dd MMM · hh:mm tt"),
                    Reason = a.Reason
                });
            }
            OnPropertyChanged(nameof(HasNoAppointments));
        }

        private void PreFillFromAppointment(AppointmentItem item)
        {
            var appt = item.Source;
            DischargeSummary = appt.Diagnosis ?? string.Empty;
            FollowUpInstructions = appt.Recommendation ?? string.Empty;
            PrescriptionSummary = appt.Prescription ?? string.Empty;
        }

        private void SaveDischarge()
        {
            if (_selectedAppointment == null)
            {
                SetStatus("Please select a patient appointment.", true);
                return;
            }
            if (string.IsNullOrWhiteSpace(DischargeSummary))
            {
                SetStatus("Please enter a discharge summary.", true);
                return;
            }

            var appt = _selectedAppointment.Source;
            appt.Recommendation = FollowUpInstructions?.Trim() ?? appt.Recommendation;
            appt.Prescription = PrescriptionSummary?.Trim() ?? appt.Prescription;
            if (appt.Status != "Completed")
                appt.Status = "Completed";
            appt.CompletionTime = DateTime.Now;

            // Create a medical record as the discharge document
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
            DataManager.MedicalRecords.Add(new MedicalRecord
            {
                Id = DataManager.MedicalRecords.Any() ? DataManager.MedicalRecords.Max(r => r.Id) + 1 : 1,
                PatientId = appt.PatientId,
                DoctorId = _doctor.Id,
                Title = $"Discharge Summary – {DateTime.Today:dd MMM yyyy}",
                Date = DateTime.Now,
                Diagnosis = DischargeSummary.Trim(),
                Treatment = FollowUpInstructions?.Trim() ?? string.Empty,
                Prescription = PrescriptionSummary?.Trim() ?? string.Empty,
                MedicalNotes = $"Discharge condition: {DischargeCondition}",
                DoctorName = _doctor.FullName
            });

            DataManager.SaveAppointments();
            DataManager.SaveMedicalRecords();

            SetStatus($"Discharge summary saved for {_selectedAppointment.PatientName}!", false);
            LoadAppointments();
        }

        private void SetStatus(string msg, bool isError)
        {
            StatusMsg = msg;
            HasError = isError;
            HasStatusMsg = true;
        }
    }
}
