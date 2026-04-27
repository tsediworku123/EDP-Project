using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System;
using System.Windows;
using System.Windows.Input;
using HMS.Core.ViewModels.Auth;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.ViewModels
{
    public class AdminShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "DASHBOARD";
        private string _todayDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public string TodayDate { get => _todayDate; set => SetProperty(ref _todayDate, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavStaffCommand { get; }
        public ICommand NavUsersCommand { get; }
        public ICommand NavPatientsCommand { get; }
        public ICommand NavAppointmentsCommand { get; }
        public ICommand NavAvailabilityCommand { get; }
        public ICommand NavReportsCommand { get; }
        public ICommand NavChangePasswordCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminShellViewModel()
        {
            DataManager.EnsureLoaded();
            
            // Set initial view
            ActivePageTitle = "DASHBOARD";
            CurrentView = new AdminDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "DASHBOARD";
                CurrentView = new AdminDashboardView();
            });

            NavStaffCommand = new RelayCommand(() => {
                ActivePageTitle = "MANAGE STAFF";
                CurrentView = new AdminStaffView(); 
            });

            NavAvailabilityCommand = new RelayCommand(() => {
                ActivePageTitle = "STAFF AVAILABILITY";
                CurrentView = new AdminAvailabilityView();
            });

            NavUsersCommand = new RelayCommand(() => {
                ActivePageTitle = "USER ACCOUNTS";
                CurrentView = new AdminUsersView();
            });

            NavPatientsCommand = new RelayCommand(() => {
                ActivePageTitle = "PATIENT DIRECTORY";
                CurrentView = new AdminPatientsView();
            });

            NavAppointmentsCommand = new RelayCommand(() => {
                ActivePageTitle = "APPOINTMENTS";
                CurrentView = new AdminAppointmentsView();
            });

            NavReportsCommand = new RelayCommand(() => {
                ActivePageTitle = "REPORTS & EXPORT";
                CurrentView = new AdminReportsView();
            });

            NavChangePasswordCommand = new RelayCommand(async () => {
                var view = new ChangePasswordDialog { DataContext = new ChangePasswordViewModel() };
                await DialogHost.Show(view, "MainDialogHost");
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
