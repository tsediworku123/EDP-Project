using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class BillingPaymentsViewModel : ObservableObject
    {
        private ObservableCollection<Payment> _payments;
        public ObservableCollection<Payment> Payments
        {
            get => _payments;
            set => SetProperty(ref _payments, value);
        }

        public BillingPaymentsViewModel()
        {
            LoadPayments();
        }

        private void LoadPayments()
        {
            Payments = new ObservableCollection<Payment>(DataManager.Payments.OrderByDescending(p => p.PaymentDate));
        }
    }
}
