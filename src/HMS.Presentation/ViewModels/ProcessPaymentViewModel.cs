using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class ProcessPaymentViewModel : ObservableObject
    {
        private Bill _bill;
        public Bill Bill
        {
            get => _bill;
            set => SetProperty(ref _bill, value);
        }

        private decimal _paymentAmount;
        public decimal PaymentAmount
        {
            get => _paymentAmount;
            set => SetProperty(ref _paymentAmount, value);
        }

        private string _paymentMethod = "Cash";
        public string PaymentMethod
        {
            get => _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }

        public decimal RemainingBalance => Bill.TotalAmount - Bill.PaidAmount;

        public ICommand ConfirmPaymentCommand { get; }

        public ProcessPaymentViewModel(Bill bill)
        {
            Bill = bill;
            PaymentAmount = RemainingBalance;
            ConfirmPaymentCommand = new RelayCommand(ExecuteConfirmPayment, CanConfirmPayment);
        }

        private bool CanConfirmPayment() => PaymentAmount > 0 && PaymentAmount <= RemainingBalance;

        private void ExecuteConfirmPayment()
        {
            var payment = new Payment
            {
                BillId = Bill.Id,
                PatientId = Bill.PatientId,
                Amount = PaymentAmount,
                PaymentMethod = PaymentMethod,
                PaymentDate = DateTime.Now,
                TransactionId = $"TXN-{DateTime.Now:yyyyMMddHHmmss}",
                Status = "Successful",
                HandledBy = CurrentSession.Instance.LoggedInBillingStaff?.FullName ?? "System"
            };

            DataManager.Payments.Add(payment);
            DataManager.SavePayments();

            // Update Bill
            Bill.PaidAmount += PaymentAmount;
            if (Bill.PaidAmount >= Bill.TotalAmount)
                Bill.Status = "Paid";
            else
                Bill.Status = "Partially Paid";

            DataManager.SaveBills();

            MaterialDesignThemes.Wpf.DialogHost.Close("BillingDialogHost");
        }
    }
}
