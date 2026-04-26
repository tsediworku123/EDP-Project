using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class PharmacistDashboardViewModel : ObservableObject
    {
        public int PendingPrescriptionsCount { get; set; }
        public int LowStockItemsCount { get; set; }
        public int MedicinesDispensedToday { get; set; }
        
        public ObservableCollection<PharmacistActivityItem> RecentActivities { get; } = new ObservableCollection<PharmacistActivityItem>();
 
        public PharmacistDashboardViewModel()
        {
            DataManager.EnsureLoaded();
            LoadStats();
        }
 
        private void LoadStats()
        {
            // Dummy logic for now, in a real app this would query inventory and prescriptions
            PendingPrescriptionsCount = 12; 
            LowStockItemsCount = 5;
            MedicinesDispensedToday = 45;
 
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
        }
    }
 
    public class PharmacistActivityItem
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}
