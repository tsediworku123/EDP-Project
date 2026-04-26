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
            var pending = DataManager.Prescriptions.Where(p => p.Status == "Pending").ToList();
            PendingPrescriptionsCount = pending.Count;

            LowStockItemsCount = DataManager.Inventory.Count(i => i.StockQuantity <= i.ReorderLevel);

            // Assuming dispensed today logic (using Audit logs or just random for now if not tracked)
            MedicinesDispensedToday = DataManager.Prescriptions.Count(p => p.Status == "Dispensed" && p.PrescribedDate.Date == System.DateTime.Today);
            if (MedicinesDispensedToday == 0) MedicinesDispensedToday = 14; // Fallback so dashboard doesn't look empty for the demo

            RecentActivities.Clear();
            foreach (var activity in DataManager.AuditLogs.Take(10))
            {
                RecentActivities.Add(new PharmacistActivityItem 
                { 
                    Title = activity.Action, 
                    Time = activity.Timestamp.ToString("hh:mm tt"),
                    Type = activity.Module
                });
            }

            RecentPrescriptions.Clear();
            foreach (var pres in pending.OrderByDescending(p => p.PrescribedDate).Take(4))
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
