using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class BookVisitViewModel : ObservableObject
    {
        private string _selectedSpecialty = "All Specialties";
        private string _searchText = "";
        private DoctorAvailabilityInfo _selectedDoctorInfo;
        private DateTime _selectedDate = DateTime.Today.AddDays(1);
        private DateTime _selectedTime;
        private string _reason;

        public ObservableCollection<string> Specialties { get; } = new ObservableCollection<string>();
        public ObservableCollection<DoctorAvailabilityInfo> FilteredDoctors { get; } = new ObservableCollection<DoctorAvailabilityInfo>();
        public ObservableCollection<TimeSlot> AvailableSlots { get; } = new ObservableCollection<TimeSlot>();

        public string SelectedSpecialty { get => _selectedSpecialty; set { if (SetProperty(ref _selectedSpecialty, value)) UpdateDoctors(); } }
        public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) UpdateDoctors(); } }
        public DoctorAvailabilityInfo SelectedDoctorInfo
        {
            get => _selectedDoctorInfo;
            set { if (SetProperty(ref _selectedDoctorInfo, value)) LoadSlots(); }
        }
        public DateTime SelectedDate { get => _selectedDate; set { if (SetProperty(ref _selectedDate, value)) { UpdateDoctors(); LoadSlots(); } } }
        public DateTime SelectedTime { get => _selectedTime; set => SetProperty(ref _selectedTime, value); }
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }

        public ICommand ConfirmCommand { get; }

        public BookVisitViewModel()
        {
            ConfirmCommand = new RelayCommand(ConfirmBooking, () => SelectedDoctorInfo != null && SelectedTime != DateTime.MinValue);
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

            foreach (var d in query)
            {
                // Check if doctor works on selected date
                var dayName = SelectedDate.DayOfWeek.ToString().Substring(0, 3);
                bool worksToday = d.WorkingDays != null && d.WorkingDays.Contains(dayName);

                // Calculate total possible slots for the day
                var totalMinutes = (d.WorkingHoursEnd - d.WorkingHoursStart).TotalMinutes;
                var breakMinutes = (d.BreakTimeEnd - d.BreakTimeStart).TotalMinutes;
                int baseSlots = (int)((totalMinutes - breakMinutes) / (d.SlotDurationMinutes > 0 ? d.SlotDurationMinutes : 30));
                if (baseSlots < 1) baseSlots = 1;
                
                // Total capacity = slots * patients per slot
                int totalCapacity = baseSlots * (d.MaxPatientsPerSlot > 0 ? d.MaxPatientsPerSlot : 1);

                // Count booked appointments for this doctor on selected date
                int currentlyBooked = DataManager.Appointments.Count(a =>
                    a.DoctorId == d.Id &&
                    a.AppointmentDate.Date == SelectedDate.Date &&
                    a.Status != "Cancelled" && a.Status != "No Show");

                int slotsRemaining = Math.Max(0, totalCapacity - currentlyBooked);

                // Next available date if fully booked or not working today
                string nextAvailDate = "";
                if (!worksToday || slotsRemaining == 0)
                {
                    var checkDate = SelectedDate.AddDays(1);
                    for (int i = 0; i < 30; i++)
                    {
                        var dn = checkDate.DayOfWeek.ToString().Substring(0, 3);
                        if (d.WorkingDays != null && d.WorkingDays.Contains(dn))
                        {
                            int booked = DataManager.Appointments.Count(a =>
                                a.DoctorId == d.Id &&
                                a.AppointmentDate.Date == checkDate.Date &&
                                a.Status != "Cancelled" && a.Status != "No Show");
                            if (booked < totalCapacity)
                            {
                                nextAvailDate = checkDate.ToString("MMM dd, yyyy");
                                break;
                            }
                        }
                        checkDate = checkDate.AddDays(1);
                    }
                }

                FilteredDoctors.Add(new DoctorAvailabilityInfo
                {
                    Doctor = d,
                    FullName = d.FullName,
                    Specialty = d.Specialization,
                    WorkingHours = $"{d.WorkingHoursStart:hh\\:mm} - {d.WorkingHoursEnd:hh\\:mm}",
                    WorksOnSelectedDate = worksToday,
                    BookedSlots = currentlyBooked,
                    TotalSlots = totalCapacity,
                    SlotsRemaining = slotsRemaining,
                    IsFull = worksToday && slotsRemaining == 0,
                    NextAvailableDate = nextAvailDate,
                    AvailabilityText = !worksToday ? "Not working this day"
                                     : slotsRemaining == 0 ? "FULLY BOOKED"
                                     : $"{slotsRemaining} spots left"
                });
            }
        }

        private void LoadSlots()
        {
            AvailableSlots.Clear();
            SelectedTime = DateTime.MinValue;
            if (SelectedDoctorInfo == null) return;
            var slots = DataManager.GetAvailableTimeSlots(SelectedDoctorInfo.Doctor.Id, SelectedDate);
            foreach (var s in slots)
            {
                var slot = new TimeSlot { Time = s };
                slot.SelectionChanged += (obj, isSelected) => {
                    if (isSelected) {
                        SelectedTime = slot.Time;
                        foreach (var other in AvailableSlots) if (other != slot) other.IsSelected = false;
                    }
                };
                AvailableSlots.Add(slot);
            }
        }

        private void ConfirmBooking()
        {
            if (SelectedDoctorInfo == null) {
                MessageBox.Show("Please select a physician first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedTime == DateTime.MinValue) {
                MessageBox.Show("Please select an available timeslot.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Reason)) {
                MessageBox.Show("Please provide a reason for your visit.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var patient = DataManager.GetCurrentPatient();
            if (patient == null) {
                MessageBox.Show("User session expired. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var success = DataManager.AddAppointment(new Appointment {
                PatientId = patient.Id,
                DoctorId = SelectedDoctorInfo.Doctor.Id,
                AppointmentDate = SelectedTime,
                Status = "Scheduled",
                Reason = Reason
            });

            if (success) {
                MessageBox.Show($"Appointment secured with {SelectedDoctorInfo.FullName} for {SelectedTime:MMM dd, hh:mm tt}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Reason = ""; // Clear reason
                UpdateDoctors(); 
                LoadSlots();
            } else {
                MessageBox.Show("This slot has just been taken. Please select another one.", "Slot Unavailable", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadSlots(); // Refresh slots
            }
        }
    }

    public class DoctorAvailabilityInfo
    {
        public Doctor Doctor { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string WorkingHours { get; set; }
        public bool WorksOnSelectedDate { get; set; }
        public int BookedSlots { get; set; }
        public int TotalSlots { get; set; }
        public int SlotsRemaining { get; set; }
        public bool IsFull { get; set; }
        public string NextAvailableDate { get; set; }
        public string AvailabilityText { get; set; }
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
