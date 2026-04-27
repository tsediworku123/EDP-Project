using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class BillingInvoicesViewModel : ObservableObject
    {
        private ObservableCollection<Bill> _invoices;
        public ObservableCollection<Bill> Invoices
        {
            get => _invoices;
            set => SetProperty(ref _invoices, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set 
            { 
                if (SetProperty(ref _searchText, value))
                    LoadInvoices();
            }
        }

        public ICommand CreateInvoiceCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ProcessPaymentCommand { get; }

        public BillingInvoicesViewModel()
        {
            LoadInvoices();
            CreateInvoiceCommand = new RelayCommand(ExecuteCreateInvoice);
            ViewDetailsCommand = new RelayCommand<Bill>(ExecuteViewDetails);
            ProcessPaymentCommand = new RelayCommand<Bill>(ExecuteProcessPayment);
        }

        private void LoadInvoices()
        {
            var query = DataManager.Bills.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(b => b.InvoiceNumber.Contains(SearchText) || b.PatientId.ToString().Contains(SearchText));
            }
            Invoices = new ObservableCollection<Bill>(query.OrderByDescending(b => b.BillDate));
        }

        private async void ExecuteCreateInvoice()
        {
            var view = new Views.CreateInvoiceView { DataContext = new CreateInvoiceViewModel() };
            await MaterialDesignThemes.Wpf.DialogHost.Show(view, "BillingDialogHost");
            LoadInvoices();
        }

        private void ExecuteViewDetails(Bill bill)
        {
            if (bill == null) return;
            // Logic to show details
        }

        private async void ExecuteProcessPayment(Bill bill)
        {
            if (bill == null || bill.Status == "Paid") return;
            var view = new Views.ProcessPaymentView { DataContext = new ProcessPaymentViewModel(bill) };
            await MaterialDesignThemes.Wpf.DialogHost.Show(view, "BillingDialogHost");
            LoadInvoices();
        }
    }
}
