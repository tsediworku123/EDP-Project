using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class BillingDashboardViewModel : ObservableObject
    {
        public decimal DailyRevenue => DataManager.Payments
            .Where(p => p.PaymentDate.Date == DateTime.Today && p.Status == "Successful")
            .Sum(p => p.Amount);

        public decimal MonthlyRevenue => DataManager.Payments
            .Where(p => p.PaymentDate.Month == DateTime.Today.Month && p.PaymentDate.Year == DateTime.Today.Year && p.Status == "Successful")
            .Sum(p => p.Amount);

        public int PendingInvoicesCount => DataManager.Bills.Count(b => b.Status == "Unpaid" || b.Status == "Partially Paid");
        public int PendingClaimsCount => DataManager.InsuranceClaims.Count(c => c.Status == "Pending");

        public ObservableCollection<double> RevenuePoints { get; set; } = new ObservableCollection<double> { 45, 60, 55, 80, 75, 95, 85, 110, 100, 120, 115, 140 };

        private ObservableCollection<Payment> _recentPayments;
        public ObservableCollection<Payment> RecentPayments
        {
            get => _recentPayments;
            set => SetProperty(ref _recentPayments, value);
        }

        private ObservableCollection<Bill> _unpaidBills;
        public ObservableCollection<Bill> UnpaidBills
        {
            get => _unpaidBills;
            set => SetProperty(ref _unpaidBills, value);
        }

        public BillingDashboardViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var payments = DataManager.Payments
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .ToList();
            RecentPayments = new ObservableCollection<Payment>(payments);

            var bills = DataManager.Bills
                .Where(b => b.Status == "Unpaid" || b.Status == "Partially Paid")
                .OrderByDescending(b => b.BillDate)
                .Take(5)
                .ToList();
            UnpaidBills = new ObservableCollection<Bill>(bills);
        }
    }
}
