using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AddAppointmentViewModel : ObservableObject
    {
        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        private DateTime _appointmentDate = DateTime.Today;
        public DateTime AppointmentDate
        {
            get => _appointmentDate;
            set => SetProperty(ref _appointmentDate, value);
        }

        private DateTime _appointmentTime = DateTime.Now;
        public DateTime AppointmentTime
        {
            get => _appointmentTime;
            set => SetProperty(ref _appointmentTime, value);
        }

        private string _reason;
        public string Reason
        {
            get => _reason;
            set => SetProperty(ref _reason, value);
        }

        private string _priority = "Normal";
        public string Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        public ObservableCollection<string> PriorityOptions { get; } = new ObservableCollection<string>
        {
            "Normal", "Urgent", "Follow-up"
        };

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddAppointmentViewModel()
        {
            DataManager.EnsureLoaded();
            Patients = new ObservableCollection<Patient>(DataManager.Patients.OrderBy(p => p.FullName));
            
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return SelectedPatient != null && !string.IsNullOrWhiteSpace(Reason);
        }

        private void Save()
        {
            var doctor = CurrentSession.Instance.LoggedInDoctor;
            if (doctor == null)
            {
                MessageBox.Show("Doctor session not found. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var combinedDateTime = AppointmentDate.Date.Add(AppointmentTime.TimeOfDay);

            var newAppointment = new Appointment
            {
                PatientId = SelectedPatient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = combinedDateTime,
                Reason = Reason,
                Priority = Priority,
                Status = "Scheduled",
                AppointmentType = "Consultation"
            };

            bool success = DataManager.AddAppointment(newAppointment);

            if (success)
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
                MessageBox.Show($"Appointment scheduled for {SelectedPatient.FullName} on {combinedDateTime:f}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add appointment. Please check if the date is in the past.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
    }
}
