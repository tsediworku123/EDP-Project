using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System;
using System.Windows;
using System.Windows.Input;

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
        public ICommand NavDoctorsCommand { get; }
        public ICommand NavUsersCommand { get; }
        public ICommand NavPatientsCommand { get; }
        public ICommand NavAppointmentsCommand { get; }
        public ICommand NavReportsCommand { get; }
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

            NavDoctorsCommand = new RelayCommand(() => {
                ActivePageTitle = "MANAGE DOCTORS";
                CurrentView = new AdminDoctorsView(); 
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
