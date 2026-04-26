using HMS.Core.Common.Utils;
using System;

namespace HMS.Core.ViewModels
{
    public class GenerateBillViewModel : ObservableObject
    {
        private string _patientName;
        public string PatientName { get => _patientName; set => SetProperty(ref _patientName, value); }

        private decimal _totalAmount;
        public decimal TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }

        private string _status = "Unpaid";
        public string Status { get => _status; set => SetProperty(ref _status, value); }

        public PharmacyBillDTO GetNewBill()
        {
            return new PharmacyBillDTO
            {
                PatientName = this.PatientName,
                TotalAmount = this.TotalAmount,
                Status = this.Status,
                Date = DateTime.Now.ToString("MMM dd, yyyy")
            };
        }
    }
}
