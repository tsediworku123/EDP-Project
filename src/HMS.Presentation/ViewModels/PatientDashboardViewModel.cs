using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PatientDashboardViewModel : ObservableObject
    {
        private int _patientId;
        private string _welcomeMessage;
        private Appointment _nextAppointment;
        private int _totalVisits;
        private int _upcomingCount;
        private string _bloodGroup;
        private int _pendingBillsCount;
        private int _pendingPrescriptionsCount;
        private int _medicalRecordsCount;
        private string _allergies;
        private string _currentMedications;

        public string WelcomeMessage { get => _welcomeMessage; set => SetProperty(ref _welcomeMessage, value); }
        public Appointment NextAppointment { get => _nextAppointment; set => SetProperty(ref _nextAppointment, value); }
        public int TotalVisits { get => _totalVisits; set => SetProperty(ref _totalVisits, value); }
        public int UpcomingCount { get => _upcomingCount; set => SetProperty(ref _upcomingCount, value); }
        public int MedicalRecordsCount { get => _medicalRecordsCount; set => SetProperty(ref _medicalRecordsCount, value); }
        public int PendingBillsCount { get => _pendingBillsCount; set => SetProperty(ref _pendingBillsCount, value); }
        public int PendingPrescriptionsCount { get => _pendingPrescriptionsCount; set => SetProperty(ref _pendingPrescriptionsCount, value); }
        public string Allergies { get => _allergies; set => SetProperty(ref _allergies, value); }
        public string CurrentMedications { get => _currentMedications; set => SetProperty(ref _currentMedications, value); }
        public string BloodGroup { get => _bloodGroup; set => SetProperty(ref _bloodGroup, value); }

        public ObservableCollection<DashboardActivity> RecentActivity { get; } = new ObservableCollection<DashboardActivity>();

        public PatientDashboardViewModel()
        {
            var patient = DataManager.GetCurrentPatient();
            if (patient != null)
            {
                _patientId = patient.Id;
                WelcomeMessage = $"Welcome back, {patient.FullName?.Split(' ').FirstOrDefault() ?? "Patient"}";
                Allergies = patient.AllergiesOrChronicConditions ?? "No allergies recorded";
                CurrentMedications = patient.CurrentMedications ?? "None";
                BloodGroup = patient.BloodGroup ?? "Unknown";
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
            PendingBillsCount = DataManager.GetPatientBills(_patientId).Count(b => b.Status != "Paid");
            PendingPrescriptionsCount = DataManager.GetPatientPrescriptions(_patientId).Count(p => p.Status == "Prescribed");

            NextAppointment = appointments
                .Where(a => a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();

            RecentActivity.Clear();
            var activities = new System.Collections.Generic.List<DashboardActivity>();

            // Add Medical Records
            foreach (var r in DataManager.GetPatientMedicalRecords(_patientId).OrderByDescending(r => r.Date).Take(2))
            {
                activities.Add(new DashboardActivity { 
                    Title = "Diagnosis Updated", 
                    Summary = r.Diagnosis, 
                    Date = r.Date,
                    Icon = "MessageMedical",
                    Color = "#3B82F6"
                });
            }

            // Add Lab Results
            foreach (var l in DataManager.GetPatientLabTests(_patientId).Where(t => t.Status == "Completed").OrderByDescending(t => t.CompletedDate).Take(2))
            {
                activities.Add(new DashboardActivity { 
                    Title = "Lab Results Ready", 
                    Summary = $"{l.TestName}: {l.NumericalValue} {l.Unit}", 
                    Date = l.CompletedDate ?? DateTime.Now,
                    Icon = "Flask",
                    Color = "#8B5CF6"
                });
            }

            // Add Prescriptions
            foreach (var p in DataManager.GetPatientPrescriptions(_patientId).OrderByDescending(p => p.PrescribedDate).Take(2))
            {
                activities.Add(new DashboardActivity { 
                    Title = p.Status == "Dispensed" ? "Medication Dispensed" : "New Prescription", 
                    Summary = $"Visit on {p.PrescribedDate:MMM dd}", 
                    Date = p.PrescribedDate,
                    Icon = "Pill",
                    Color = "#10B981"
                });
            }

            // Sort and take top 5
            foreach (var act in activities.OrderByDescending(a => a.Date).Take(5))
            {
                RecentActivity.Add(act);
            }
        }
    }

    public class DashboardActivity
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }
}
