using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PatientShellViewModel : ObservableObject
    {
        private object _currentView;
        private bool _isSidebarCollapsed = false;
        private string _activePageTitle = "Dashboard";

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavBookingCommand { get; }
        public ICommand NavAppointmentsCommand { get; }
        public ICommand NavHistoryCommand { get; }
        public ICommand NavProfileCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ToggleSidebarCommand { get; }

        public PatientShellViewModel()
        {
            CurrentView = new PatientDashboardView();
            ActivePageTitle = "DASHBOARD";

            NavDashboardCommand = new RelayCommand(() => {
                CurrentView = new PatientDashboardView();
                ActivePageTitle = "DASHBOARD";
            });
            NavBookingCommand = new RelayCommand(() => {
                CurrentView = new BookVisitView();
                ActivePageTitle = "BOOK NEW VISIT";
            });
            NavAppointmentsCommand = new RelayCommand(() => {
                CurrentView = new MyAppointmentsView();
                ActivePageTitle = "MY APPOINTMENTS";
            });
            NavHistoryCommand = new RelayCommand(() => {
                CurrentView = new MedicalHistoryView();
                ActivePageTitle = "MEDICAL RECORDS";
            });
            NavProfileCommand = new RelayCommand(() => {
                CurrentView = new MyProfileView();
                ActivePageTitle = "MY PROFILE";
            });
            LogoutCommand = new RelayCommand(() => {
                CurrentSession.Instance.EndSession();
                var login = new HMS.Core.Views.Auth.LoginView();
                login.Show();
                foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
                {
                    if (w is PatientShellView)
                    {
                        w.Close();
                        break;
                    }
                }
            });
            ToggleSidebarCommand = new RelayCommand(() => IsSidebarCollapsed = !IsSidebarCollapsed);
        }
    }
}
