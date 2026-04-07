using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;

namespace ClinicAppointmentSystem.ViewModels
{
    public class MyAppointmentsViewModel : ObservableObject
    {
        private string _searchText = "";
        public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) RefreshFilteredLists(); } }

        public ObservableCollection<Appointment> UpcomingAppointments { get; } = new ObservableCollection<Appointment>();
        public ObservableCollection<Appointment> PastAppointments { get; } = new ObservableCollection<Appointment>();
        public ObservableCollection<Appointment> CancelledAppointments { get; } = new ObservableCollection<Appointment>();

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
                query = query.Where(a => {
                    var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                    return doc?.FullName?.ToLower().Contains(SearchText.ToLower()) ?? false;
                });

            foreach (var a in query.Where(x => x.Status == "Scheduled" && x.AppointmentDate >= DateTime.Now).OrderBy(x => x.AppointmentDate))
                UpcomingAppointments.Add(a);

            foreach (var a in query.Where(x => x.Status == "Completed" || (x.Status == "Scheduled" && x.AppointmentDate < DateTime.Now)).OrderByDescending(x => x.AppointmentDate))
                PastAppointments.Add(a);

            foreach (var a in query.Where(x => x.Status == "Cancelled").OrderByDescending(x => x.AppointmentDate))
                CancelledAppointments.Add(a);
        }
    }
}
