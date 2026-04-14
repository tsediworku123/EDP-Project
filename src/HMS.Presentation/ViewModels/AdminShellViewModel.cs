using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "DASHBOARD";
        private bool _isSidebarCollapsed = false;

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavDoctorsCommand { get; }
        public ICommand NavUsersCommand { get; }
        public ICommand NavPatientsCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminShellViewModel()
        {
            // Set initial view
            ActivePageTitle = "DASHBOARD";
            CurrentView = new AdminDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "DASHBOARD";
                CurrentView = new AdminDashboardView();
            });

            NavDoctorsCommand = new RelayCommand(() => {
                ActivePageTitle = "MANAGE DOCTORS";
                // CurrentView = new ManageDoctorsView();
            });

            NavUsersCommand = new RelayCommand(() => {
                ActivePageTitle = "USER ACCOUNTS";
                // CurrentView = new ManageUsersView();
            });

            NavPatientsCommand = new RelayCommand(() => {
                ActivePageTitle = "PATIENT LIST";
                // CurrentView = new PatientListView();
            });

            LogoutCommand = new RelayCommand(() => {
                CurrentSession.Instance.EndSession();
                var login = new LoginView();
                login.Show();
                
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is AdminShellView)
                    {
                        w.Close();
                        break;
                    }
                }
            });
        }
    }
}
