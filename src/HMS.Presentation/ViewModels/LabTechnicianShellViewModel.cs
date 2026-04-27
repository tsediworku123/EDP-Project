using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class LabTechnicianShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "LAB DASHBOARD";
        private string _labTechName;
        private bool _isSidebarCollapsed = false;

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public string LabTechName { get => _labTechName; set => SetProperty(ref _labTechName, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }
        public NotificationCenterViewModel NotificationCenterVM { get; } = new NotificationCenterViewModel();

        public ICommand NavDashboardCommand { get; }
        public ICommand NavQueueCommand { get; }
        public ICommand NavHistoryCommand { get; }
        public ICommand NavLogoutCommand { get; }

        public LabTechnicianShellViewModel()
        {
            DataManager.EnsureLoaded();
            
            if (CurrentSession.Instance.LoggedInLabTechnician != null)
            {
                LabTechName = CurrentSession.Instance.LoggedInLabTechnician.FullName.ToUpper();
            }
            else
            {
                LabTechName = "LAB TECHNICIAN";
            }

            // Set initial view
            ActivePageTitle = "LAB DASHBOARD";
            CurrentView = new LabTechDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "LAB DASHBOARD";
                CurrentView = new LabTechDashboardView();
            });

            NavQueueCommand = new RelayCommand(() => {
                ActivePageTitle = "TEST QUEUE";
                CurrentView = new LabTechTestQueueView();
            });

            NavHistoryCommand = new RelayCommand(() => {
                ActivePageTitle = "TEST HISTORY";
                CurrentView = new LabTechHistoryView();
            });

            NavLogoutCommand = new RelayCommand(() => {
                CurrentSession.Instance.EndSession();
                var login = new LoginView();
                login.Show();
                
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            });
        }
    }
}
