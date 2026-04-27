using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace HMS.Core.ViewModels
{
    public class PatientBillingViewModel : ObservableObject
    {
        public ObservableCollection<Bill> Bills { get; } = new ObservableCollection<Bill>();
        public ICommand PayBillCommand { get; }

        public PatientBillingViewModel()
        {
            PayBillCommand = new RelayCommand<Bill>(PayBill);
            LoadBills();
        }

        private void PayBill(Bill bill)
        {
            if (bill == null || bill.Status == "Paid") return;

            var result = MessageBox.Show($"Are you sure you want to pay {bill.TotalAmount:C} for {bill.InvoiceNumber}?", 
                                         "Confirm Payment", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                bill.Status = "Paid";
                bill.PaidAmount = bill.TotalAmount;
                
                DataManager.UpdateBill(bill);
                MessageBox.Show("Payment successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBills();
            }
        }

        private void LoadBills()
        {
            DataManager.EnsureLoaded();
            var patient = DataManager.GetCurrentPatient();
            if (patient == null) return;

            var patientBills = DataManager.GetPatientBills(patient.Id)
                .OrderByDescending(b => b.BillDate)
                .ToList();

            Bills.Clear();
            foreach (var bill in patientBills)
            {
                Bills.Add(bill);
            }
        }
    }
}
