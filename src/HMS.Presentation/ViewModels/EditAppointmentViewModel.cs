using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class EditAppointmentViewModel : ObservableObject
    {
        private readonly Appointment _appointment;

        private DateTime _appointmentDate;
        public DateTime AppointmentDate { get => _appointmentDate; set => SetProperty(ref _appointmentDate, value); }

        private string _appointmentTime;
        public string AppointmentTime { get => _appointmentTime; set => SetProperty(ref _appointmentTime, value); }

        private string _reason;
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }

        private string _status;
        public string Status { get => _status; set => SetProperty(ref _status, value); }

        private string _appointmentType;
        public string AppointmentType { get => _appointmentType; set => SetProperty(ref _appointmentType, value); }

        private string _priority;
        public string Priority { get => _priority; set => SetProperty(ref _priority, value); }

        private string _diagnosis;
        public string Diagnosis { get => _diagnosis; set => SetProperty(ref _diagnosis, value); }

        private string _prescription;
        public string Prescription { get => _prescription; set => SetProperty(ref _prescription, value); }

        private string _consultationNote;
        public string ConsultationNote { get => _consultationNote; set => SetProperty(ref _consultationNote, value); }

        private string _recommendation;
        public string Recommendation { get => _recommendation; set => SetProperty(ref _recommendation, value); }

        private string _clinicalNotes;
        public string ClinicalNotes { get => _clinicalNotes; set => SetProperty(ref _clinicalNotes, value); }

        public ICommand SaveCommand { get; }

        public EditAppointmentViewModel(Appointment appointment)
        {
            _appointment = appointment;

            AppointmentDate = appointment.AppointmentDate.Date;
            AppointmentTime = appointment.AppointmentDate.ToString("hh:mm tt");
            Reason = appointment.Reason;
            Status = appointment.Status;
            AppointmentType = appointment.AppointmentType;
            Priority = appointment.Priority;
            Diagnosis = appointment.Diagnosis;
            Prescription = appointment.Prescription;
            ConsultationNote = appointment.ConsultationNote;
            Recommendation = appointment.Recommendation;
            ClinicalNotes = appointment.ClinicalNotes;

            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Reason))
            {
                MessageBox.Show("Reason is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Try to parse time from the text entry
            DateTime combinedDate = AppointmentDate;
            if (TimeSpan.TryParse(AppointmentTime, out TimeSpan parsedTime))
            {
                combinedDate = AppointmentDate.Date + parsedTime;
            }
            else if (DateTime.TryParse(AppointmentTime, out DateTime parsedDt))
            {
                combinedDate = AppointmentDate.Date + parsedDt.TimeOfDay;
            }

            _appointment.AppointmentDate = combinedDate;
            _appointment.Reason = Reason;
            _appointment.Status = Status;
            _appointment.AppointmentType = AppointmentType;
            _appointment.Priority = Priority;
            _appointment.Diagnosis = Diagnosis;
            _appointment.Prescription = Prescription;
            _appointment.ConsultationNote = ConsultationNote;
            _appointment.Recommendation = Recommendation;
            _appointment.ClinicalNotes = ClinicalNotes;

            if (Status == "Completed" && _appointment.CompletionTime == null)
                _appointment.CompletionTime = DateTime.Now;

            DataManager.SaveAllData();
            DialogHost.CloseDialogCommand.Execute(true, null);
            MessageBox.Show("Appointment updated successfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
