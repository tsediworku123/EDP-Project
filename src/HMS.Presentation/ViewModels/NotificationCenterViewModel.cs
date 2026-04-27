using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class NotificationCenterViewModel : ObservableObject
    {
        public ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();
        public string StatusText => Notifications.Any() ? "" : "Empty";

        public NotificationCenterViewModel()
        {
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            Notifications.Clear();
            var user = CurrentSession.Instance.LoggedInUser;
            if (user == null) return;

            DataManager.EnsureLoaded();
            
            // Get relevant notifications for this user
            var relevant = DataManager.Notifications
                .Where(n => n.TargetRole == user.Role 
                          || n.UserId == user.Id
                          || (user.Role == User.Doctor && n.DoctorId == CurrentSession.Instance.LoggedInDoctor?.Id)
                          || (user.Role == User.Patient && n.PatientId == CurrentSession.Instance.LoggedInPatient?.Id)
                          || (user.Role == User.Nurse && n.UserId == user.Id) // Extend as needed
                          )
                .OrderByDescending(n => n.Timestamp)
                .ToList();

            foreach (var n in relevant)
            {
                Notifications.Add(n);
            }
            
            OnPropertyChanged(nameof(StatusText));
        }
    }
}
