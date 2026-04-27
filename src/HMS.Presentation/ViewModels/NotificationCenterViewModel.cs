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

            // In a real app we'd fetch from DB, here we mock some for the UI
            Notifications.Add(new Notification { Title = "Welcome", Message = $"Hello {user.Email}, welcome back to the HMS portal.", Timestamp = System.DateTime.Now.AddMinutes(-5) });
            
            if (user.Role == "Doctor")
            {
                Notifications.Add(new Notification { Title = "Lab Result Ready", Message = "Urgent: Complete Blood Count results for John Doe are now available.", Timestamp = System.DateTime.Now.AddMinutes(-20) });
            }
            else if (user.Role == "LabTechnician")
            {
                Notifications.Add(new Notification { Title = "New Test Priority", Message = "A new urgent Cardiology test has been requested for Room 402.", Timestamp = System.DateTime.Now.AddMinutes(-2) });
            }

            OnPropertyChanged(nameof(StatusText));
        }
    }
}
