using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class NurseShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "DASHBOARD";
        private string _nurseName;
        private bool _isSidebarCollapsed = false;

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public string NurseName { get => _nurseName; set => SetProperty(ref _nurseName, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavVitalsCommand { get; }
        public ICommand NavPatientsCommand { get; }
        public ICommand NavAdmissionsCommand { get; }
        public ICommand NavLogoutCommand { get; }

        public NurseShellViewModel()
        {
            DataManager.EnsureLoaded();
            
            if (CurrentSession.Instance.LoggedInNurse != null)
            {
                NurseName = CurrentSession.Instance.LoggedInNurse.FullName.ToUpper();
            }
            else
            {
                NurseName = "NURSE USER";
            }

            // Set initial view
            ActivePageTitle = "DASHBOARD";
            CurrentView = new NurseDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "DASHBOARD";
                CurrentView = new NurseDashboardView();
            });

            NavVitalsCommand = new RelayCommand(() => {
                ActivePageTitle = "PATIENT VITALS MONITOR";
                CurrentView = new NurseVitalsView();
            });

            NavPatientsCommand = new RelayCommand(() => {
                ActivePageTitle = "PATIENT RECORDS";
                // CurrentView = new NursePatientsView();
            });

            NavAdmissionsCommand = new RelayCommand(() => {
                ActivePageTitle = "ADMISSIONS & DISCHARGES";
                // CurrentView = new NurseAdmissionsView();
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
