using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using HMS.Core.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AppointmentDisplayItem
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientName { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string AppointmentType { get; set; }
        public string Priority { get; set; }
        public bool IsEmergency { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string ConsultationNote { get; set; }
        public string Recommendation { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsPaid { get; set; }
        public string ClinicalNotes { get; set; }
        public DateTime? CompletionTime { get; set; }
        public int PatientRating { get; set; }
        public string PatientFeedback { get; set; }

        // Derived display values
        public string DateDisplay => AppointmentDate.ToString("dd MMM yyyy");
        public string TimeDisplay => AppointmentDate.ToString("hh:mm tt");
        public string StatusBadge => Status ?? "Pending";
        public string EmergencyLabel => IsEmergency ? "🚨 EMERGENCY" : "";
        public string TypeDisplay => $"{AppointmentType ?? "Regular"} · {Priority ?? "Normal"}";
    }

    public class DoctorAppointmentsViewModel : ObservableObject
    {
        private List<AppointmentDisplayItem> _allAppointments = new List<AppointmentDisplayItem>();

        private ObservableCollection<AppointmentDisplayItem> _appointments;
        public ObservableCollection<AppointmentDisplayItem> Appointments
        {
            get => _appointments;
            set { SetProperty(ref _appointments, value); OnPropertyChanged(nameof(AppointmentCount)); }
        }

        private AppointmentDisplayItem _selectedAppointment;
        public AppointmentDisplayItem SelectedAppointment
        {
            get => _selectedAppointment;
            set { SetProperty(ref _selectedAppointment, value); OnPropertyChanged(nameof(HasSelection)); }
        }

        public bool HasSelection => SelectedAppointment != null;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) ApplyFilters(); }
        }

        private string _selectedStatus = "All";
        public string SelectedStatus
        {
            get => _selectedStatus;
            set { if (SetProperty(ref _selectedStatus, value)) ApplyFilters(); }
        }

        public int AppointmentCount => Appointments?.Count ?? 0;

        public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string>
        {
            "All", "Pending", "Confirmed", "Completed", "Cancelled"
        };

        public ICommand RefreshCommand { get; }
        public ICommand ClearSelectionCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand EditAppointmentCommand { get; }
        public ICommand DeleteAppointmentCommand { get; }

        public DoctorAppointmentsViewModel()
        {
            DataManager.EnsureLoaded();
            LoadAllAppointments();
            ApplyFilters();

            RefreshCommand = new RelayCommand(() => { DataManager.EnsureLoaded(); LoadAllAppointments(); ApplyFilters(); });
            ClearSelectionCommand = new RelayCommand(() => SelectedAppointment = null);
            UpdateStatusCommand = new RelayCommand<string>(UpdateStatus);
            EditAppointmentCommand = new RelayCommand<AppointmentDisplayItem>(EditAppointment);
            DeleteAppointmentCommand = new RelayCommand<AppointmentDisplayItem>(DeleteAppointment);
        }

        private async void EditAppointment(AppointmentDisplayItem item)
        {
            if (item == null) return;
            var real = DataManager.Appointments.FirstOrDefault(a => a.Id == item.Id);
            if (real == null) return;

            var vm = new EditAppointmentViewModel(real);
            var dialog = new EditAppointmentDialog { DataContext = vm };
            await DialogHost.Show(dialog, "MainDialogHost");

            LoadAllAppointments();
            ApplyFilters();
            SelectedAppointment = null;
        }

        private void DeleteAppointment(AppointmentDisplayItem item)
        {
            if (item == null) return;
            var result = MessageBox.Show(
                $"Are you sure you want to delete the appointment for {item.PatientName} on {item.DateDisplay}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var real = DataManager.Appointments.FirstOrDefault(a => a.Id == item.Id);
                if (real != null)
                {
                    DataManager.Appointments.Remove(real);
                    DataManager.SaveAllData();
                    LoadAllAppointments();
                    ApplyFilters();
                    SelectedAppointment = null;
                    MessageBox.Show("Appointment deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void LoadAllAppointments()
        {
            _allAppointments.Clear();
            var docId = CurrentSession.Instance.LoggedInDoctor?.Id ?? 0;

            // If docId == 0 (no doctor logged in, e.g. admin testing), show all appointments
            var query = docId > 0
                ? DataManager.Appointments.Where(a => a.DoctorId == docId)
                : DataManager.Appointments.AsEnumerable();

            foreach (var app in query.OrderByDescending(a => a.AppointmentDate))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == app.PatientId);
                _allAppointments.Add(new AppointmentDisplayItem
                {
                    Id = app.Id,
                    PatientId = app.PatientId,
                    DoctorId = app.DoctorId,
                    AppointmentDate = app.AppointmentDate,
                    PatientName = patient?.FullName ?? $"Patient #{app.PatientId}",
                    Reason = app.Reason,
                    Status = app.Status,
                    AppointmentType = app.AppointmentType,
                    Priority = app.Priority,
                    IsEmergency = app.IsEmergency,
                    Diagnosis = app.Diagnosis,
                    Prescription = app.Prescription,
                    ConsultationNote = app.ConsultationNote,
                    Recommendation = app.Recommendation,
                    ConsultationFee = app.ConsultationFee,
                    IsPaid = app.IsPaid,
                    ClinicalNotes = app.ClinicalNotes,
                    CompletionTime = app.CompletionTime,
                    PatientRating = app.PatientRating,
                    PatientFeedback = app.PatientFeedback
                });
            }
        }

        private void ApplyFilters()
        {
            var filtered = _allAppointments.AsEnumerable();

            if (SelectedStatus != "All")
                filtered = filtered.Where(a => a.Status == SelectedStatus);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var q = SearchText.ToLower();
                filtered = filtered.Where(a =>
                    (a.PatientName != null && a.PatientName.ToLower().Contains(q)) ||
                    (a.Reason != null && a.Reason.ToLower().Contains(q)) ||
                    a.Id.ToString().Contains(q));
            }

            Appointments = new ObservableCollection<AppointmentDisplayItem>(filtered);
        }

        private void UpdateStatus(string newStatus)
        {
            if (SelectedAppointment == null || string.IsNullOrEmpty(newStatus)) return;

            // Capture values BEFORE refreshing (ApplyFilters replaces the collection,
            // which causes the DataGrid TwoWay binding to null out SelectedAppointment).
            int appointmentId = SelectedAppointment.Id;
            string patientName = SelectedAppointment.PatientName;

            var real = DataManager.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (real == null) return;

            real.Status = newStatus;
            if (newStatus == "Completed") real.CompletionTime = DateTime.Now;
            DataManager.SaveAllData();

            LoadAllAppointments();
            ApplyFilters();
            SelectedAppointment = null;

            MessageBox.Show($"Appointment #{appointmentId} for {patientName} updated to '{newStatus}'.",
                "Status Updated", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
