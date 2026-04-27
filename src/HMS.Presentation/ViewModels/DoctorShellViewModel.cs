using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System.Windows;
using System.Windows.Input;
using HMS.Core.ViewModels.Auth;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.ViewModels
{
    public class DoctorShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "DASHBOARD";
        private string _doctorName;
        private bool _isSidebarCollapsed = false;

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public string DoctorName { get => _doctorName; set => SetProperty(ref _doctorName, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavAppointmentsCommand { get; }
        public ICommand NavPatientsCommand { get; }
        public ICommand NavRecordsCommand { get; }
        public ICommand NavProfileCommand { get; }
        public ICommand NavChangePasswordCommand { get; }
        public ICommand NavNotificationsCommand { get; }
        public ICommand LogoutCommand { get; }

        public DoctorShellViewModel()
        {
            DataManager.EnsureLoaded();
            var doctor = CurrentSession.Instance.LoggedInDoctor;
            DoctorName = doctor != null ? doctor.FullName.ToUpper() : "DOCTOR";

            // Set initial view
            ActivePageTitle = "DASHBOARD";
            CurrentView = new DoctorDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "DASHBOARD";
                CurrentView = new DoctorDashboardView();
            });

            NavAppointmentsCommand = new RelayCommand(() => {
                ActivePageTitle = "APPOINTMENTS";
                CurrentView = new DoctorAppointmentsView();
            });

            NavPatientsCommand = new RelayCommand(() => {
                ActivePageTitle = "MY PATIENTS";
                CurrentView = new DoctorPatientsView();
            });

            NavRecordsCommand = new RelayCommand(() => {
                ActivePageTitle = "MEDICAL RECORDS";
                CurrentView = new DoctorRecordsView();
            });

            NavProfileCommand = new RelayCommand(() => {
                ActivePageTitle = "MY PROFILE";
                CurrentView = new DoctorProfileView();
            });

            NavChangePasswordCommand = new RelayCommand(async () => {
                var view = new ChangePasswordDialog { DataContext = new ChangePasswordViewModel() };
                await DialogHost.Show(view, "MainDialogHost");
            });

            NavNotificationsCommand = new RelayCommand(async () => {
                var view = new NotificationCenterView { DataContext = new NotificationCenterViewModel() };
                await DialogHost.Show(view, "MainDialogHost");
            });

            LogoutCommand = new RelayCommand(() => {
                CurrentSession.Instance.EndSession();
                var login = new LoginView();
                login.Show();
                
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is DoctorShellView)
                    {
                        w.Close();
                        break;
                    }
                }
            });
        }
    }
}
