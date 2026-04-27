using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace HMS.Core.ViewModels
{
    public class LabTechTestQueueViewModel : ObservableObject
    {
        private ObservableCollection<LabTest> _pendingTests;
        public ObservableCollection<LabTest> PendingTests 
        { 
            get => _pendingTests; 
            set => SetProperty(ref _pendingTests, value); 
        }

        private LabTest _selectedTest;
        public LabTest SelectedTest 
        { 
            get => _selectedTest; 
            set => SetProperty(ref _selectedTest, value); 
        }

        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories { get => _categories; set => SetProperty(ref _categories, value); }

        private string _selectedCategory;
        public string SelectedCategory 
        { 
            get => _selectedCategory; 
            set {
                if (SetProperty(ref _selectedCategory, value))
                    LoadQueue();
            }
        }

        public ICommand CollectSampleCommand { get; }
        public ICommand StartTestingCommand { get; }
        public ICommand CompleteTestCommand { get; }
        public ICommand RejectSampleCommand { get; }

        public LabTechTestQueueViewModel()
        {
            Categories = new ObservableCollection<string> { "All Categories", "Hematology", "Biochemistry", "Microbiology", "Immunology", "Other" };
            SelectedCategory = "All Categories";
            
            LoadQueue();
            CollectSampleCommand = new RelayCommand(ExecuteCollectSample);
            StartTestingCommand = new RelayCommand(ExecuteStartTesting);
            CompleteTestCommand = new RelayCommand(ExecuteCompleteTest);
            RejectSampleCommand = new RelayCommand(ExecuteRejectSample);
        }

        private void LoadQueue()
        {
            var tests = DataManager.LabTests
                .Where(t => t.Status != "Completed" && t.Status != "Cancelled" && t.Status != "Rejected")
                .ToList();

            // Resolve Patient Names
            foreach (var test in tests)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == test.PatientId);
                test.PatientName = patient?.FullName ?? "Unknown Patient";
            }

            var query = tests.AsEnumerable();

            if (SelectedCategory != "All Categories" && !string.IsNullOrEmpty(SelectedCategory))
            {
                query = query.Where(t => t.TestCategory == SelectedCategory);
            }

            var queue = query
                .OrderByDescending(t => t.IsUrgent)
                .ThenBy(t => t.RequestedDate)
                .ToList();
            
            PendingTests = new ObservableCollection<LabTest>(queue);
        }

        private async void ExecuteRejectSample()
        {
            if (SelectedTest == null) return;
            
            var view = new Views.RejectionReasonDialog();
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "MainDialogHost");

            if (result is string reason && !string.IsNullOrWhiteSpace(reason))
            {
                SelectedTest.Status = "Rejected";
                SelectedTest.RejectionReason = reason;
                
                // Notify Doctor
                DataManager.Notifications.Add(new Notification
                {
                    DoctorId = SelectedTest.DoctorId,
                    Title = "LAB TEST REJECTED",
                    Message = $"Sample for patient {SelectedTest.PatientName} ({SelectedTest.TestName}) was rejected. Reason: {reason}",
                    Type = "Laboratory"
                });

                DataManager.SaveLabTests();
                DataManager.SaveNotifications();
                DataManager.LogAudit(CurrentSession.Instance.LoggedInUser?.Email, $"Rejected sample for {SelectedTest.TestName}. Reason: {reason}", "Laboratory");
                LoadQueue();
            }
        }

        private void ExecuteCollectSample()
        {
            if (SelectedTest == null) return;
            SelectedTest.Status = "Sample Collected";
            DataManager.SaveLabTests();
            LoadQueue();
        }

        private void ExecuteStartTesting()
        {
            if (SelectedTest == null) return;
            SelectedTest.Status = "In Progress";
            DataManager.SaveLabTests();
            LoadQueue();
        }

        private async void ExecuteCompleteTest()
        {
            if (SelectedTest == null) return;
            
            var view = new Views.LabTechResultsView(SelectedTest);
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "MainDialogHost");
            
            if (result is bool b && b)
            {
                LoadQueue();
            }
        }
    }
}
