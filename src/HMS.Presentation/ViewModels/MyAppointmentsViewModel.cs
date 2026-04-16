using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;

namespace HMS.Core.ViewModels
{
    public class MyAppointmentsViewModel : ObservableObject
    {
        private string _searchText = "";
        public string SearchText 
        { 
            get => _searchText; 
            set { if (SetProperty(ref _searchText, value)) RefreshFilteredLists(); } 
        }

        public ObservableCollection<AppointmentDisplay> UpcomingAppointments { get; } = new ObservableCollection<AppointmentDisplay>();
        public ObservableCollection<AppointmentDisplay> PastAppointments { get; } = new ObservableCollection<AppointmentDisplay>();
        public ObservableCollection<AppointmentDisplay> CancelledAppointments { get; } = new ObservableCollection<AppointmentDisplay>();

        private List<Appointment> _allAppointments = new List<Appointment>();

        public MyAppointmentsViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            var patient = DataManager.GetCurrentPatient();
            if (patient != null)
            {
                _allAppointments = DataManager.GetPatientAppointments(patient.Id);
                RefreshFilteredLists();
            }
        }

        private void RefreshFilteredLists()
        {
            UpcomingAppointments.Clear();
            PastAppointments.Clear();
            CancelledAppointments.Clear();

            var query = _allAppointments.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(a => {
                    var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                    return doc?.FullName?.ToLower().Contains(SearchText.ToLower()) ?? false;
                });
            }

            foreach (var a in query.Where(x => x.Status == "Scheduled" && x.AppointmentDate >= DateTime.Now).OrderBy(x => x.AppointmentDate))
                UpcomingAppointments.Add(MapToDisplay(a));
            
            foreach (var a in query.Where(x => x.Status == "Completed" || (x.Status == "Scheduled" && x.AppointmentDate < DateTime.Now)).OrderByDescending(x => x.AppointmentDate))
                PastAppointments.Add(MapToDisplay(a));
            
            foreach (var a in query.Where(x => x.Status == "Cancelled").OrderByDescending(x => x.AppointmentDate))
                CancelledAppointments.Add(MapToDisplay(a));
        }

        private AppointmentDisplay MapToDisplay(Appointment a)
        {
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            return new AppointmentDisplay
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status,
                DoctorName = doc?.FullName ?? "Unknown Doctor",
                DoctorSpecialty = doc?.Specialization ?? "General Practice",
                StatusColor = GetStatusColor(a.Status)
            };
        }

        private string GetStatusColor(string status)
        {
            switch (status)
            {
                case "Scheduled": return "#3B82F6";
                case "Completed": return "#10B981";
                case "Cancelled": return "#EF4444";
                default: return "#64748B";
            }
        }
    }

    public class AppointmentDisplay
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialty { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
    }
}
