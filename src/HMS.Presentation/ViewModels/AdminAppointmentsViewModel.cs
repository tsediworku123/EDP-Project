using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DisplayAppointment : ObservableObject
    {
        public Appointment BaseAppointment { get; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Department { get; set; }
        public string TimeString => BaseAppointment.AppointmentDate.ToString("HH:mm");
        public string DateString => BaseAppointment.AppointmentDate.ToString("dd MMM yyyy");
        
        public string Status => BaseAppointment.Status;
        public string Priority => BaseAppointment.Priority;
        public string Reason => BaseAppointment.Reason;
        public int Id => BaseAppointment.Id;

        public DisplayAppointment(Appointment appointment)
        {
            BaseAppointment = appointment;
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == appointment.PatientId);
            var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
            
            PatientName = patient?.FullName ?? $"Unknown (ID: {appointment.PatientId})";
            DoctorName = doctor?.FullName ?? $"Unknown (ID: {appointment.DoctorId})";
            Department = doctor?.Department ?? "N/A";
        }
    }

    public class AdminAppointmentsViewModel : ObservableObject
    {
        private ObservableCollection<DisplayAppointment> _filteredAppointments;
        private string _searchText;
        private string _selectedStatus = "All";
        private int _totalAppointments;
        private int _todayAppointments;
        private int _pendingAppointments;

        public ObservableCollection<DisplayAppointment> FilteredAppointments 
        { 
            get => _filteredAppointments; 
            set => SetProperty(ref _filteredAppointments, value); 
        }

        public string SearchText 
        { 
            get => _searchText; 
            set 
            { 
                if (SetProperty(ref _searchText, value))
                    ApplyFilters();
            } 
        }

        public string SelectedStatus 
        { 
            get => _selectedStatus; 
            set 
            { 
                if (SetProperty(ref _selectedStatus, value))
                    ApplyFilters();
            } 
        }

        public List<string> StatusOptions { get; } = new List<string> { "All", "Scheduled", "Completed", "Cancelled" };

        public int TotalAppointments { get => _totalAppointments; set => SetProperty(ref _totalAppointments, value); }
        public int TodayAppointments { get => _todayAppointments; set => SetProperty(ref _todayAppointments, value); }
        public int PendingAppointments { get => _pendingAppointments; set => SetProperty(ref _pendingAppointments, value); }

        public ICommand RefreshCommand { get; }
        public ICommand CancelAppointmentCommand { get; }
        public ICommand CompleteAppointmentCommand { get; }
        public ICommand ConfirmAppointmentCommand { get; }
        public ICommand ReactivateCommand { get; }

        public AdminAppointmentsViewModel()
        {
            RefreshCommand = new RelayCommand(LoadData);
            CancelAppointmentCommand = new RelayCommand<DisplayAppointment>(a => UpdateStatus(a, "Cancelled", "cancel this appointment?"));
            CompleteAppointmentCommand = new RelayCommand<DisplayAppointment>(a => UpdateStatus(a, "Completed", "mark this appointment as completed?"));
            ConfirmAppointmentCommand = new RelayCommand<DisplayAppointment>(a => UpdateStatus(a, "Confirmed", "confirm this appointment?"));
            ReactivateCommand = new RelayCommand<DisplayAppointment>(a => UpdateStatus(a, "Scheduled", "re-activate this cancelled appointment?"));
            
            LoadData();
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            UpdateStats();
            ApplyFilters();
        }

        private void UpdateStats()
        {
            var apps = DataManager.Appointments;
            TotalAppointments = apps.Count;
            TodayAppointments = apps.Count(a => a.AppointmentDate.Date == DateTime.Today);
            PendingAppointments = apps.Count(a => (a.Status == "Scheduled" || a.Status == "Confirmed") && a.AppointmentDate >= DateTime.Today);
        }

        private void ApplyFilters()
        {
            var query = DataManager.Appointments.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string search = SearchText.ToLower();
                var appsWithNames = query.Select(a => new DisplayAppointment(a));
                appsWithNames = appsWithNames.Where(a => 
                    a.PatientName.ToLower().Contains(search) || 
                    a.DoctorName.ToLower().Contains(search) || 
                    a.Reason.ToLower().Contains(search));
                
                if (SelectedStatus != "All")
                    appsWithNames = appsWithNames.Where(a => a.Status == SelectedStatus);

                FilteredAppointments = new ObservableCollection<DisplayAppointment>(appsWithNames.OrderByDescending(a => a.BaseAppointment.AppointmentDate));
            }
            else
            {
                if (SelectedStatus != "All")
                    query = query.Where(a => a.Status == SelectedStatus);

                FilteredAppointments = new ObservableCollection<DisplayAppointment>(
                    query.Select(a => new DisplayAppointment(a))
                         .OrderByDescending(a => a.BaseAppointment.AppointmentDate)
                );
            }
        }

        private void UpdateStatus(DisplayAppointment displayApp, string newStatus, string actionText)
        {
            if (displayApp == null) return;

            // Basic Confirmation Dialog using standard MessageBox for simplicity in this turn, 
            // but we can integrate with DialogHost if you prefer.
            var result = MessageBox.Show($"Are you sure you want to {actionText}", 
                                       "Confirm Action", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var app = DataManager.Appointments.FirstOrDefault(a => a.Id == displayApp.Id);
                if (app != null)
                {
                    app.Status = newStatus;
                    if (newStatus == "Completed") app.CompletionTime = DateTime.Now;
                    
                    DataManager.SaveAppointments();
                    LoadData();
                }
            }
        }
    }
}
