using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views;
using HMS.Core.Views.Auth;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PharmacistShellViewModel : ObservableObject
    {
        private object _currentView;
        private string _activePageTitle = "DASHBOARD";
        private string _pharmacistName;
        private bool _isSidebarCollapsed = false;

        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }
        public string ActivePageTitle { get => _activePageTitle; set => SetProperty(ref _activePageTitle, value); }
        public string PharmacistName { get => _pharmacistName; set => SetProperty(ref _pharmacistName, value); }
        public bool IsSidebarCollapsed { get => _isSidebarCollapsed; set => SetProperty(ref _isSidebarCollapsed, value); }

        public ICommand NavDashboardCommand { get; }
        public ICommand NavPrescriptionsCommand { get; }
        public ICommand NavInventoryCommand { get; }
        public ICommand NavBillingCommand { get; }
        public ICommand NavLogoutCommand { get; }

        public PharmacistShellViewModel()
        {
            DataManager.EnsureLoaded();
            
            if (CurrentSession.Instance.LoggedInPharmacist != null)
            {
                PharmacistName = CurrentSession.Instance.LoggedInPharmacist.FullName.ToUpper();
            }
            else
            {
                PharmacistName = "PHARMACIST USER";
            }

            // Set initial view
            ActivePageTitle = "DASHBOARD";
            CurrentView = new PharmacistDashboardView();
            
            NavDashboardCommand = new RelayCommand(() => {
                ActivePageTitle = "DASHBOARD";
                CurrentView = new PharmacistDashboardView();
            });

            NavPrescriptionsCommand = new RelayCommand(() => {
                ActivePageTitle = "PRESCRIPTIONS & DISPENSING";
                CurrentView = new PharmacistPrescriptionsView();
            });

            NavInventoryCommand = new RelayCommand(() => {
                ActivePageTitle = "DRUG INVENTORY";
                CurrentView = new PharmacistInventoryView();
            });

            NavBillingCommand = new RelayCommand(() => {
                ActivePageTitle = "PHARMACY BILLING";
                CurrentView = new PharmacistBillingView();
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
