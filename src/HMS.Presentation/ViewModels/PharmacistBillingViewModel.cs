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
            
            // Dummy bills for UI presentation
            Bills.Add(new PharmacyBillDTO { Id = 1001, PatientName = "John Doe", Date = DateTime.Now.ToString("MMM dd, yyyy"), TotalAmount = 45.50m, Status = "Paid" });
            Bills.Add(new PharmacyBillDTO { Id = 1002, PatientName = "Jane Smith", Date = DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy"), TotalAmount = 120.00m, Status = "Unpaid" });
            Bills.Add(new PharmacyBillDTO { Id = 1003, PatientName = "Michael Scott", Date = DateTime.Now.AddDays(-2).ToString("MMM dd, yyyy"), TotalAmount = 15.75m, Status = "Paid" });
        }

        private void GenerateNewBill()
        {
            System.Windows.MessageBox.Show("Open Generate Pharmacy Bill Dialog", "Generate Bill", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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
