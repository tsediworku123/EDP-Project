using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class NurseDashboardViewModel : ObservableObject
    {
        public int TotalPatientsInWard { get; set; }
        public int PendingVitalsCount { get; set; }
        public int MedsToAdministerCount { get; set; }
        
        public ObservableCollection<NurseActivityItem> RecentActivities { get; } = new ObservableCollection<NurseActivityItem>();
        public ObservableCollection<ScheduleItem> CareSchedule { get; } = new ObservableCollection<ScheduleItem>();
 
        public NurseDashboardViewModel()
        {
            DataManager.EnsureLoaded();
            LoadStats();
        }
 
        private void LoadStats()
        {
            DataManager.EnsureLoaded();
            
            // Real counts from DataManager
            TotalPatientsInWard = DataManager.Patients.Count;
            
            // Count appointments for today that might need vitals or meds
            var today = DateTime.Today;
            var todayAppointments = DataManager.Appointments
                .Where(a => a.AppointmentDate.Date == today && a.Status == "Scheduled")
                .ToList();
                
            PendingVitalsCount = todayAppointments.Count;
            MedsToAdministerCount = todayAppointments.Count / 2; // Approximation

            RecentActivities.Clear();
            // Show latest 10 audit logs as activities
            var logs = DataManager.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(10);

            foreach (var activity in logs)
            {
                RecentActivities.Add(new NurseActivityItem 
                { 
                    Title = $"{activity.Action} in {activity.Module}", 
                    Time = activity.Timestamp.ToString("hh:mm tt"),
                    Type = activity.Module
                });
            }

            CareSchedule.Clear();
            var appointments = DataManager.Appointments
                .Where(a => a.AppointmentDate.Date == today && a.Status == "Scheduled")
                .OrderBy(a => a.AppointmentDate)
                .Take(5);

            foreach (var appt in appointments)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
                CareSchedule.Add(new ScheduleItem
                {
                    PatientName = patient?.FullName ?? "Unknown Patient",
                    Time = appt.AppointmentDate.ToString("hh:mm tt"),
                    Description = $"Appointment: {appt.Reason}",
                    TaskType = "Vitals/Checkup"
                });
            }
        }
    }

    public class ScheduleItem : ObservableObject
    {
        private string _patientName;
        private string _time;
        private string _description;
        private string _taskType;

        public string PatientName { get => _patientName; set => SetProperty(ref _patientName, value); }
        public string Time { get => _time; set => SetProperty(ref _time, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string TaskType { get => _taskType; set => SetProperty(ref _taskType, value); }
    }

    public class NurseActivityItem : ObservableObject
    {
        private string _title;
        private string _time;
        private string _type;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Time { get => _time; set => SetProperty(ref _time, value); }
        public string Type { get => _type; set => SetProperty(ref _type, value); }
    }
}
