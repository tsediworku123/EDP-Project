using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ClinicAppointmentSystem.ViewModels
{
    public class PatientDashboardViewModel : ObservableObject
    {
        private int _patientId;
        private string _welcomeMessage;
        private Appointment _nextAppointment;
        private int _totalVisits;
        private int _upcomingCount;
        private int _medicalRecordsCount;

        public string WelcomeMessage { get => _welcomeMessage; set => SetProperty(ref _welcomeMessage, value); }
        public Appointment NextAppointment { get => _nextAppointment; set => SetProperty(ref _nextAppointment, value); }
        public int TotalVisits { get => _totalVisits; set => SetProperty(ref _totalVisits, value); }
        public int UpcomingCount { get => _upcomingCount; set => SetProperty(ref _upcomingCount, value); }
        public int MedicalRecordsCount { get => _medicalRecordsCount; set => SetProperty(ref _medicalRecordsCount, value); }

        public ObservableCollection<DashboardActivity> RecentActivity { get; } = new ObservableCollection<DashboardActivity>();

        public PatientDashboardViewModel()
        {
            var patient = DataManager.GetCurrentPatient();
            if (patient != null)
            {
                _patientId = patient.Id;
                WelcomeMessage = $"Welcome back, {patient.FullName?.Split(' ').FirstOrDefault() ?? "Patient"}";
            }
            else
            {
                WelcomeMessage = "Welcome to the Clinic Portal";
            }
        }

        public void Initialize()
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            DataManager.EnsureLoaded();
            var appointments = DataManager.GetPatientAppointments(_patientId);
            
            TotalVisits = appointments.Count(a => a.Status == "Completed");
            UpcomingCount = appointments.Count(a => a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now);
            MedicalRecordsCount = DataManager.GetPatientMedicalRecords(_patientId).Count;

            NextAppointment = appointments
                .Where(a => a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();

            RecentActivity.Clear();
            var recentRecords = DataManager.GetPatientMedicalRecords(_patientId).OrderByDescending(r => r.Date).Take(3);
            foreach (var r in recentRecords) 
            {
                RecentActivity.Add(new DashboardActivity { 
                    Title = "Medical Record Added", 
                    Summary = r.Diagnosis, 
                    Date = r.Date,
                    Icon = "MessageMedical"
                });
            }
        }
    }
}
