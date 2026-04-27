using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class NurseDashboardViewModel : ObservableObject
    {
        public int TotalPatientsInWard { get; set; }
        public int PendingVitalsCount { get; set; }
        public int MedsToAdministerCount { get; set; }
        
        public ObservableCollection<NurseActivityItem> RecentActivities { get; } = new ObservableCollection<NurseActivityItem>();
 
        public NurseDashboardViewModel()
        {
            DataManager.EnsureLoaded();
            LoadStats();
        }
 
        private void LoadStats()
        {
            // Dummy logic for now, in a real app this would query active admissions
            TotalPatientsInWard = 14; 
            PendingVitalsCount = 4;
            MedsToAdministerCount = 8;
 
            RecentActivities.Clear();
            foreach (var activity in DataManager.AuditLogs.Take(10))
            {
                RecentActivities.Add(new NurseActivityItem 
                { 
                    Title = activity.Action, 
                    Time = activity.Timestamp.ToString("hh:mm tt"),
                    Type = activity.Module
                });
            }
        }
    }
 
    public class NurseActivityItem
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}
