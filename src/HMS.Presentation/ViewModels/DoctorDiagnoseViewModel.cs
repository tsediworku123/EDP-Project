using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorDiagnoseViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private AppointmentItem _selectedAppointment;
        private string _chiefComplaint;
        private string _diagnosis;
        private string _recommendation;
        private string _clinicalNotes;
        private string _statusMsg;
        private bool _hasStatusMsg;

        public ObservableCollection<AppointmentItem> TodayAppointments { get; } = new ObservableCollection<AppointmentItem>();
        public string TodayDateDisplay => DateTime.Today.ToString("dddd, MMMM d yyyy");
        public bool HasNoAppointments => TodayAppointments.Count == 0;

        public AppointmentItem SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                SetProperty(ref _selectedAppointment, value);
                OnPropertyChanged(nameof(HasSelection));
                OnPropertyChanged(nameof(SelectedPatientName));
                OnPropertyChanged(nameof(SelectedAppointmentInfo));
                // Pre-fill existing diagnosis if any
                if (value != null)
                {
                    ChiefComplaint = value.Source.Reason ?? string.Empty;
                    Diagnosis = value.Source.Diagnosis ?? string.Empty;
                    Recommendation = value.Source.Recommendation ?? string.Empty;
                    ClinicalNotes = value.Source.ClinicalNotes ?? string.Empty;
                }
                StatusMsg = string.Empty;
                HasStatusMsg = false;
            }
        }

        public bool HasSelection => _selectedAppointment != null;
        public string SelectedPatientName => _selectedAppointment?.PatientName ?? string.Empty;
        public string SelectedAppointmentInfo => _selectedAppointment != null
            ? $"Appointment #{_selectedAppointment.Source.Id} · {_selectedAppointment.TimeDisplay}"
            : string.Empty;

        public string ChiefComplaint { get => _chiefComplaint; set => SetProperty(ref _chiefComplaint, value); }
        public string Diagnosis { get => _diagnosis; set => SetProperty(ref _diagnosis, value); }
        public string Recommendation { get => _recommendation; set => SetProperty(ref _recommendation, value); }
        public string ClinicalNotes { get => _clinicalNotes; set => SetProperty(ref _clinicalNotes, value); }
        public string StatusMsg { get => _statusMsg; set => SetProperty(ref _statusMsg, value); }
        public bool HasStatusMsg { get => _hasStatusMsg; set => SetProperty(ref _hasStatusMsg, value); }

        public ICommand SaveDiagnosisCommand { get; }

        public DoctorDiagnoseViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            LoadAppointments();
            SaveDiagnosisCommand = new RelayCommand(SaveDiagnosis);
        }

        private void LoadAppointments()
        {
            TodayAppointments.Clear();
            if (_doctor == null) return;

            var appts = DataManager.Appointments
                .Where(a => a.DoctorId == _doctor.Id
                         && a.AppointmentDate.Date == DateTime.Today
                         && a.Status != "Cancelled")
                .OrderBy(a => a.AppointmentDate);

            foreach (var a in appts)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                TodayAppointments.Add(new AppointmentItem
                {
                    Source = a,
                    PatientName = patient?.FullName ?? "Unknown Patient",
                    TimeDisplay = a.AppointmentDate.ToString("hh:mm tt"),
                    Reason = a.Reason
                });
            }
            OnPropertyChanged(nameof(HasNoAppointments));
        }

        private void SaveDiagnosis()
        {
            if (_selectedAppointment == null || string.IsNullOrWhiteSpace(Diagnosis))
            {
                StatusMsg = "Please select a patient and enter a diagnosis.";
                HasStatusMsg = true;
                return;
            }

            var appt = _selectedAppointment.Source;
            appt.Diagnosis = Diagnosis.Trim();
            appt.Recommendation = Recommendation?.Trim() ?? string.Empty;
            appt.ClinicalNotes = ClinicalNotes?.Trim() ?? string.Empty;
            if (appt.Status == "Scheduled" || appt.Status == "Confirmed")
                appt.Status = "Completed";

            // Also create / update MedicalRecord
            var existing = DataManager.MedicalRecords
                .FirstOrDefault(r => r.PatientId == appt.PatientId && r.DoctorId == appt.DoctorId
                                  && r.Date.Date == DateTime.Today);
            if (existing != null)
            {
                existing.Diagnosis = Diagnosis.Trim();
                existing.Treatment = Recommendation?.Trim() ?? string.Empty;
                existing.MedicalNotes = ClinicalNotes?.Trim() ?? string.Empty;
            }
            else
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
                DataManager.MedicalRecords.Add(new MedicalRecord
                {
                    Id = DataManager.MedicalRecords.Any() ? DataManager.MedicalRecords.Max(r => r.Id) + 1 : 1,
                    PatientId = appt.PatientId,
                    DoctorId = _doctor.Id,
                    Title = $"Diagnosis – {DateTime.Today:dd MMM yyyy}",
                    Date = DateTime.Now,
                    Diagnosis = Diagnosis.Trim(),
                    Treatment = Recommendation?.Trim() ?? string.Empty,
                    MedicalNotes = ClinicalNotes?.Trim() ?? string.Empty,
                    DoctorName = _doctor.FullName
                });
            }

            DataManager.SaveAppointments();
            DataManager.SaveMedicalRecords();

            StatusMsg = $"Diagnosis saved for {_selectedAppointment.PatientName} successfully!";
            HasStatusMsg = true;
        }
    }

    // Shared DTO used by multiple clinical ViewModels
    public class AppointmentItem
    {
        public Appointment Source { get; set; }
        public string PatientName { get; set; }
        public string TimeDisplay { get; set; }
        public string Reason { get; set; }
    }
}
