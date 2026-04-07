using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ClinicAppointmentSystem.ViewModels
{
    public class BookVisitViewModel : ObservableObject
    {
        private string _selectedSpecialty = "All Specialties";
        private string _searchText = "";
        private Doctor _selectedDoctor;
        private DateTime _selectedDate = DateTime.Today.AddDays(1);
        private DateTime _selectedTime;
        private string _reason;

        public ObservableCollection<string> Specialties { get; } = new ObservableCollection<string>();
        public ObservableCollection<Doctor> FilteredDoctors { get; } = new ObservableCollection<Doctor>();
        public ObservableCollection<TimeSlot> AvailableSlots { get; } = new ObservableCollection<TimeSlot>();

        public string SelectedSpecialty { get => _selectedSpecialty; set { if (SetProperty(ref _selectedSpecialty, value)) UpdateDoctors(); } }
        public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) UpdateDoctors(); } }
        public Doctor SelectedDoctor { get => _selectedDoctor; set { if (SetProperty(ref _selectedDoctor, value)) LoadSlots(); } }
        public DateTime SelectedDate { get => _selectedDate; set { if (SetProperty(ref _selectedDate, value)) LoadSlots(); } }
        public DateTime SelectedTime { get => _selectedTime; set => SetProperty(ref _selectedTime, value); }
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }

        public ICommand ConfirmCommand { get; }

        public BookVisitViewModel()
        {
            ConfirmCommand = new RelayCommand(ConfirmBooking, () => SelectedDoctor != null && SelectedTime != DateTime.MinValue);
            InitialLoad();
        }

        private void InitialLoad()
        {
            DataManager.EnsureLoaded();
            Specialties.Add("All Specialties");
            foreach (var s in DataManager.Departments) Specialties.Add(s);
            UpdateDoctors();
        }

        private void UpdateDoctors()
        {
            FilteredDoctors.Clear();
            var query = DataManager.Doctors.Where(d => d.IsActive && !d.IsOnLeave);
            if (SelectedSpecialty != "All Specialties") query = query.Where(d => d.Department == SelectedSpecialty);
            if (!string.IsNullOrWhiteSpace(SearchText)) query = query.Where(d => d.FullName.ToLower().Contains(SearchText.ToLower()));
            
            foreach (var d in query) FilteredDoctors.Add(d);
        }

        private void LoadSlots()
        {
            AvailableSlots.Clear();
            SelectedTime = DateTime.MinValue;
            if (SelectedDoctor == null) return;
            var slots = DataManager.GetAvailableTimeSlots(SelectedDoctor.Id, SelectedDate);
            foreach (var s in slots) 
            {
                var slot = new TimeSlot { Time = s };
                slot.SelectionChanged += (obj, isSelected) => {
                    if (isSelected) {
                        SelectedTime = slot.Time;
                        // Deselect others
                        foreach (var other in AvailableSlots) if (other != slot) other.IsSelected = false;
                    }
                };
                AvailableSlots.Add(slot);
            }
        }

        private void ConfirmBooking()
        {
            var patient = DataManager.GetCurrentPatient();
            if (patient == null) return;

            var success = DataManager.AddAppointment(new Appointment {
                PatientId = patient.Id,
                DoctorId = SelectedDoctor.Id,
                AppointmentDate = SelectedTime,
                Status = "Scheduled",
                Reason = Reason
            });

            if (success) {
                MessageBox.Show("Visit successfully scheduled!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } else {
                MessageBox.Show("Could not schedule visit. Please try another slot.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class TimeSlot : ObservableObject
    {
        private DateTime _time;
        private bool _isSelected;

        public DateTime Time { get => _time; set => SetProperty(ref _time, value); }
        public bool IsSelected 
        { 
            get => _isSelected; 
            set { 
                if (SetProperty(ref _isSelected, value)) 
                    SelectionChanged?.Invoke(this, value); 
            } 
        }

        public event Action<TimeSlot, bool> SelectionChanged;
    }
}
