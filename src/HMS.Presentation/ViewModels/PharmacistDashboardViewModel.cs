using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class PharmacistDashboardViewModel : ObservableObject
    {
        private int _pendingPrescriptionsCount;
        public int PendingPrescriptionsCount { get => _pendingPrescriptionsCount; set => SetProperty(ref _pendingPrescriptionsCount, value); }

        private int _lowStockItemsCount;
        public int LowStockItemsCount { get => _lowStockItemsCount; set => SetProperty(ref _lowStockItemsCount, value); }

        private int _expiredItemsCount;
        public int ExpiredItemsCount { get => _expiredItemsCount; set => SetProperty(ref _expiredItemsCount, value); }

        private int _expiringSoonCount;
        public int ExpiringSoonCount { get => _expiringSoonCount; set => SetProperty(ref _expiringSoonCount, value); }

        private int _medicinesDispensedToday;
        public int MedicinesDispensedToday { get => _medicinesDispensedToday; set => SetProperty(ref _medicinesDispensedToday, value); }
        
        public ObservableCollection<PharmacistActivityItem> RecentActivities { get; } = new ObservableCollection<PharmacistActivityItem>();
        public ObservableCollection<RecentPrescriptionItem> RecentPrescriptions { get; } = new ObservableCollection<RecentPrescriptionItem>();
 
        public PharmacistDashboardViewModel()
        {
            DataManager.EnsureLoaded();
            LoadStats();
        }
 
        private void LoadStats()
        {
            DataManager.ReloadAllData();

            var pending = DataManager.Prescriptions.Where(p => p.Status == "Pending" || p.Status == "Partial").ToList();
            PendingPrescriptionsCount = pending.Count;

            LowStockItemsCount = DataManager.Inventory.Count(i => i.StockQuantity <= i.ReorderLevel && i.IsActive);
            ExpiredItemsCount = DataManager.Inventory.Count(i => i.IsExpired && i.IsActive);
            ExpiringSoonCount = DataManager.Inventory.Count(i => i.IsExpiringSoon && i.IsActive);

            MedicinesDispensedToday = DataManager.Prescriptions.Count(p => 
                p.DispensedDate.HasValue && p.DispensedDate.Value.Date == System.DateTime.Today);

            RecentActivities.Clear();
            var logs = DataManager.AuditLogs
                .Where(l => l.Module == "Pharmacy" || l.Module == "Inventory" || l.Module == "Laboratory")
                .OrderByDescending(l => l.Timestamp)
                .Take(8);

            foreach (var activity in logs)
            {
                RecentActivities.Add(new PharmacistActivityItem 
                { 
                    Title = activity.Action, 
                    Time = activity.Timestamp.ToString("hh:mm tt"),
                    Type = activity.Module
                });
            }

            RecentPrescriptions.Clear();
            foreach (var pres in pending.OrderByDescending(p => p.PrescribedDate).Take(5))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == pres.PatientId);
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == pres.DoctorId);
                var itemsCount = pres.Items?.Count ?? 0;

                RecentPrescriptions.Add(new RecentPrescriptionItem
                {
                    Description = $"Prescription #{pres.Id} ({itemsCount} items) - Patient: {patient?.FullName ?? "Unknown"}",
                    SubDescription = $"Prescribed by {doctor?.FullName ?? "Unknown"} • {pres.PrescribedDate:MMM dd, hh:mm tt}"
                });
            }
        }
    }
 
    public class PharmacistActivityItem
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }

    public class RecentPrescriptionItem
    {
        public string Description { get; set; }
        public string SubDescription { get; set; }
    }
}
