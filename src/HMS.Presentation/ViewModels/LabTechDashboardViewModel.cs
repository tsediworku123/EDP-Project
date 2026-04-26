using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class LabTechDashboardViewModel : ObservableObject
    {
        public int PendingTestsCount => DataManager.LabTests.Count(t => t.Status == "Requested" || t.Status == "Sample Collected");
        public int CompletedTodayCount => DataManager.LabTests.Count(t => t.Status == "Completed" && t.CompletedDate?.Date == System.DateTime.Today);
        public int TotalTestsCount => DataManager.LabTests.Count;

        private ObservableCollection<LabTest> _recentTests;
        public ObservableCollection<LabTest> RecentTests 
        { 
            get => _recentTests; 
            set => SetProperty(ref _recentTests, value); 
        }

        public LabTechDashboardViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var tests = DataManager.LabTests
                .OrderByDescending(t => t.RequestedDate)
                .Take(10)
                .ToList();

            foreach (var test in tests)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == test.PatientId);
                test.PatientName = patient?.FullName ?? "Unknown Patient";
            }
            
            RecentTests = new ObservableCollection<LabTest>(tests);
        }
    }
}
