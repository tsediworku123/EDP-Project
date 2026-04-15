using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AppointmentItem
    {
        public int Id { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string PatientName { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
    }

    public class DoctorAppointmentsViewModel : ObservableObject
    {
        public ObservableCollection<AppointmentItem> Appointments { get; } = new ObservableCollection<AppointmentItem>();

        public ICommand ViewDetailsCommand { get; }

        public DoctorAppointmentsViewModel()
        {
            DataManager.EnsureLoaded();
            var docId = CurrentSession.Instance.LoggedInDoctor?.Id ?? 0;
            var apps = DataManager.Appointments.Where(a => a.DoctorId == docId).OrderByDescending(a => a.AppointmentDate);
            
            foreach (var app in apps)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == app.PatientId);
                string diagnosis = string.IsNullOrEmpty(app.Diagnosis) ? "Not diagnosed yet" : app.Diagnosis;
                string detailsText = $"Patient: {patient?.FullName ?? "Unknown"}\nDate: {app.AppointmentDate}\nReason: {app.Reason}\nStatus: {app.Status}\nDiagnosis: {diagnosis}\nNotes: {app.Recommendation}";

                Appointments.Add(new AppointmentItem { 
                    Id = app.Id,
                    AppointmentDate = app.AppointmentDate, 
                    PatientName = patient?.FullName ?? "Unknown", 
                    Reason = app.Reason, 
                    Status = app.Status,
                    Details = detailsText
                });
            }

            ViewDetailsCommand = new RelayCommand<AppointmentItem>((appItem) => 
            {
                if (appItem != null)
                {
                    MessageBox.Show(appItem.Details, $"Appointment Details - ID: {appItem.Id}", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
        }
    }
}
