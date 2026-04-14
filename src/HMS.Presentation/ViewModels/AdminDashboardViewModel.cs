using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class AdminDashboardViewModel : ObservableObject
    {
        public int TotalDoctors => DataManager.Doctors.Count;
        public int TotalPatients => DataManager.Patients.Count;
        public int TotalAppointments => DataManager.Appointments.Count;
        public int TotalUsers => DataManager.Users.Count;

        public AdminDashboardViewModel()
        {
            DataManager.EnsureLoaded();
        }
    }
}
