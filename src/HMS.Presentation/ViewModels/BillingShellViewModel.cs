using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class BillingShellViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public string StaffName => CurrentSession.Instance.LoggedInBillingStaff?.FullName ?? "Billing Staff";
        public string StaffPosition => CurrentSession.Instance.LoggedInBillingStaff?.Position ?? "Accounts Department";

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowInvoicesCommand { get; }
        public ICommand ShowPaymentsCommand { get; }
        public ICommand ShowInsuranceCommand { get; }
        public ICommand LogoutCommand { get; }

        public BillingShellViewModel()
        {
            ShowDashboardCommand = new RelayCommand(() => CurrentView = new BillingDashboardViewModel());
            ShowInvoicesCommand = new RelayCommand(() => CurrentView = new BillingInvoicesViewModel());
            ShowPaymentsCommand = new RelayCommand(() => CurrentView = new BillingPaymentsViewModel());
            ShowInsuranceCommand = new RelayCommand(() => CurrentView = new BillingInsuranceViewModel());
            LogoutCommand = new RelayCommand(ExecuteLogout);

            CurrentView = new BillingDashboardViewModel();
        }

        private void ExecuteLogout()
        {
            CurrentSession.Instance.EndSession();
            // In a real app, this would trigger a window change to login
        }
    }
}
