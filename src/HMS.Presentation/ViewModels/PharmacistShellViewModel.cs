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
        public ICommand NavChangePasswordCommand { get; }
        public ICommand NavNotificationsCommand { get; }
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
                var email = CurrentSession.Instance.LoggedInUser?.Email ?? "PHARMACIST USER";
                PharmacistName = email.Split('@')[0].ToUpper();
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

            NavChangePasswordCommand = new RelayCommand(async () => {
                var view = new ChangePasswordDialog { DataContext = new ChangePasswordViewModel() };
                await DialogHost.Show(view, "MainDialogHost");
            });

            NavNotificationsCommand = new RelayCommand(async () => {
                var view = new NotificationCenterView { DataContext = new NotificationCenterViewModel() };
                await DialogHost.Show(view, "MainDialogHost");
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
