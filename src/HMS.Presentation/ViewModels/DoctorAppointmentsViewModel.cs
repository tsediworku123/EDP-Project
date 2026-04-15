using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class DoctorAppointmentsViewModel : ObservableObject
    {
        public ObservableCollection<object> Appointments { get; } = new ObservableCollection<object>();

        public DoctorAppointmentsViewModel()
        {
            DataManager.EnsureLoaded();
            var docId = CurrentSession.Instance.LoggedInDoctor?.Id ?? 0;
            var apps = DataManager.Appointments.Where(a => a.DoctorId == docId).OrderByDescending(a => a.AppointmentDate);
            
            foreach (var app in apps)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == app.PatientId);
                Appointments.Add(new { 
                    app.AppointmentDate, 
                    PatientName = patient?.FullName ?? "Unknown", 
                    app.Reason, 
                    app.Status 
                });
            }
        }
    }
}
