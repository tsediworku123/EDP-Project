using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorPrescribeViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private AppointmentItem _selectedAppointment;
        private string _medicineName;
        private string _dosage;
        private string _frequency;
        private int _durationDays = 7;
        private string _instructions;
        private string _prescriptionNotes;
        private string _statusMsg;
        private bool _hasStatusMsg;
        private bool _hasError;

        public ObservableCollection<AppointmentItem> TodayAppointments { get; } = new ObservableCollection<AppointmentItem>();
        public ObservableCollection<PrescriptionItem> MedicineItems { get; } = new ObservableCollection<PrescriptionItem>();
        public string TodayDateDisplay => DateTime.Today.ToString("dddd, MMMM d yyyy");
        public bool HasNoAppointments => TodayAppointments.Count == 0;

        // Frequency options
        public ObservableCollection<string> FrequencyOptions { get; } = new ObservableCollection<string>
        {
            "Once daily", "Twice daily", "Three times daily", "Four times daily",
            "Every 8 hours", "Every 12 hours", "As needed (PRN)", "Once weekly", "Before meals", "After meals"
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
                MedicineItems.Clear();
                StatusMsg = string.Empty;
                HasStatusMsg = false;
            }
        }

        public bool HasSelection => _selectedAppointment != null;
        public string SelectedPatientName => _selectedAppointment?.PatientName ?? string.Empty;
        public string SelectedAppointmentInfo => _selectedAppointment != null
            ? $"Appointment #{_selectedAppointment.Source.Id} · {_selectedAppointment.TimeDisplay}"
            : string.Empty;
        public bool HasMedicines => MedicineItems.Count > 0;

        public string MedicineName { get => _medicineName; set => SetProperty(ref _medicineName, value); }
        public string Dosage { get => _dosage; set => SetProperty(ref _dosage, value); }
        public string Frequency { get => _frequency; set => SetProperty(ref _frequency, value); }
        public int DurationDays { get => _durationDays; set => SetProperty(ref _durationDays, value); }
        public string Instructions { get => _instructions; set => SetProperty(ref _instructions, value); }
        public string PrescriptionNotes { get => _prescriptionNotes; set => SetProperty(ref _prescriptionNotes, value); }
        public string StatusMsg { get => _statusMsg; set => SetProperty(ref _statusMsg, value); }
        public bool HasStatusMsg { get => _hasStatusMsg; set => SetProperty(ref _hasStatusMsg, value); }
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }

        public ICommand AddMedicineCommand { get; }
        public ICommand RemoveMedicineCommand { get; }
        public ICommand SavePrescriptionCommand { get; }

        public DoctorPrescribeViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            Frequency = FrequencyOptions[0];
            LoadAppointments();
            AddMedicineCommand = new RelayCommand(AddMedicine);
            RemoveMedicineCommand = new RelayCommand<PrescriptionItem>(RemoveMedicine);
            SavePrescriptionCommand = new RelayCommand(SavePrescription);
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

        private void AddMedicine()
        {
            if (string.IsNullOrWhiteSpace(MedicineName))
            {
                SetStatus("Please enter a medicine name.", true);
                return;
            }
            MedicineItems.Add(new PrescriptionItem
            {
                MedicineName = MedicineName.Trim(),
                Dosage = Dosage?.Trim() ?? "As directed",
                Frequency = Frequency ?? "Once daily",
                DurationDays = DurationDays,
                Instructions = Instructions?.Trim() ?? string.Empty
            });
            // Clear fields
            MedicineName = string.Empty;
            Dosage = string.Empty;
            Instructions = string.Empty;
            DurationDays = 7;
            OnPropertyChanged(nameof(HasMedicines));
        }

        private void RemoveMedicine(PrescriptionItem item)
        {
            if (item != null) MedicineItems.Remove(item);
            OnPropertyChanged(nameof(HasMedicines));
        }

        private void SavePrescription()
        {
            if (_selectedAppointment == null)
            {
                SetStatus("Please select a patient appointment.", true);
                return;
            }
            if (MedicineItems.Count == 0)
            {
                SetStatus("Add at least one medicine before saving.", true);
                return;
            }

            var prescription = new Prescription
            {
                Id = DataManager.Prescriptions.Any() ? DataManager.Prescriptions.Max(p => p.Id) + 1 : 1,
                PatientId = _selectedAppointment.Source.PatientId,
                DoctorId = _doctor.Id,
                AppointmentId = _selectedAppointment.Source.Id,
                PrescribedDate = DateTime.Now,
                Notes = PrescriptionNotes?.Trim() ?? string.Empty,
                Status = "Pending",
                Items = MedicineItems.ToList()
            };

            // Also update the appointment prescription field
            _selectedAppointment.Source.Prescription = string.Join("; ",
                MedicineItems.Select(m => $"{m.MedicineName} {m.Dosage} – {m.Frequency}"));

            DataManager.Prescriptions.Add(prescription);
            DataManager.SavePrescriptions();
            DataManager.SaveAppointments();

            MedicineItems.Clear();
            PrescriptionNotes = string.Empty;
            OnPropertyChanged(nameof(HasMedicines));
            SetStatus($"Prescription saved for {_selectedAppointment.PatientName}!", false);
        }

        private void SetStatus(string msg, bool isError)
        {
            StatusMsg = msg;
            HasError = isError;
            HasStatusMsg = true;
        }
    }
}
