using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;

namespace HMS.Core.ViewModels
{
    public class PharmacistBillingViewModel : ObservableObject
    {
        private static System.Collections.Generic.List<PharmacyBillDTO> _allBills = new System.Collections.Generic.List<PharmacyBillDTO>
        {
            new PharmacyBillDTO { Id = 1001, PatientName = "John Doe", Date = DateTime.Now.ToString("MMM dd, yyyy"), TotalAmount = 45.50m, Status = "Paid" },
            new PharmacyBillDTO { Id = 1002, PatientName = "Jane Smith", Date = DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy"), TotalAmount = 120.00m, Status = "Unpaid" },
            new PharmacyBillDTO { Id = 1003, PatientName = "Michael Scott", Date = DateTime.Now.AddDays(-2).ToString("MMM dd, yyyy"), TotalAmount = 15.75m, Status = "Paid" }
        };

        public ObservableCollection<PharmacyBillDTO> Bills { get; } = new ObservableCollection<PharmacyBillDTO>();
        
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LoadBills();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand GenerateBillCommand { get; }

        public PharmacistBillingViewModel()
        {
            DataManager.EnsureLoaded();
            
            RefreshCommand = new RelayCommand(LoadBills);
            GenerateBillCommand = new RelayCommand(GenerateNewBill);

            LoadBills();
        }

        private void LoadBills()
        {
            Bills.Clear();
            
            var query = _allBills.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerSearch = SearchText.ToLower();
                query = query.Where(b => b.PatientName.ToLower().Contains(lowerSearch) || 
                                         b.InvoiceNumber.ToLower().Contains(lowerSearch));
            }

            foreach (var bill in query.OrderByDescending(b => b.Id))
            {
                Bills.Add(bill);
            }
        }

        private async void GenerateNewBill()
        {
            var vm = new GenerateBillViewModel();
            var dialog = new HMS.Core.Views.GenerateBillDialog { DataContext = vm };
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");

            if (result is bool success && success)
            {
                if (string.IsNullOrWhiteSpace(vm.PatientName))
                {
                    System.Windows.MessageBox.Show("Patient Name is required.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                var newBill = vm.GetNewBill();
                newBill.Id = _allBills.Count > 0 ? _allBills.Max(b => b.Id) + 1 : 1001;
                
                _allBills.Add(newBill);
                LoadBills();
            }
        }
    }

    public class PharmacyBillDTO
    {
        public int Id { get; set; }
        public string InvoiceNumber => $"INV-{Id}";
        public string PatientName { get; set; }
        public string Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
