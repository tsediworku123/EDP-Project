using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class DoctorDashboardViewModel : ObservableObject
    {
        public ObservableCollection<object> TodayAppointments { get; } = new ObservableCollection<object>();
        public int TotalPatientCount => DataManager.Patients.Count;
        public int AppointmentsTodayCount { get; private set; }

        public DoctorDashboardViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            var currentDoctor = CurrentSession.Instance.LoggedInDoctor;
            if (currentDoctor == null) return;

            var appointments = DataManager.Appointments
                .Where(a => a.DoctorId == currentDoctor.Id && a.AppointmentDate.Date == DateTime.Today)
                .OrderBy(a => a.AppointmentDate);

            AppointmentsTodayCount = appointments.Count();

            foreach (var app in appointments)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == app.PatientId);
                TodayAppointments.Add(new
                {
                    PatientName = patient?.FullName ?? "Unknown",
                    Reason = app.Reason,
                    Time = app.AppointmentDate.ToString("hh:mm tt")
                });
            }
        }
    }
}
