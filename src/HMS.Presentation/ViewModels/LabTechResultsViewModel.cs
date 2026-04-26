using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class LabTechResultsViewModel : ObservableObject
    {
        private LabTest _test;
        public LabTest Test { get => _test; set => SetProperty(ref _test, value); }

        private string _resultSummary;
        public string ResultSummary { get => _resultSummary; set => SetProperty(ref _resultSummary, value); }

        private string _resultDetails;
        public string ResultDetails { get => _resultDetails; set => SetProperty(ref _resultDetails, value); }

        private string _clinicalNotes;
        public string ClinicalNotes { get => _clinicalNotes; set => SetProperty(ref _clinicalNotes, value); }
        
        private string _referenceRange;
        public string ReferenceRange { get => _referenceRange; set => SetProperty(ref _referenceRange, value); }

        private string _numericalValue;
        public string NumericalValue { get => _numericalValue; set => SetProperty(ref _numericalValue, value); }

        private string _unit;
        public string Unit { get => _unit; set => SetProperty(ref _unit, value); }

        private bool _isCriticalValue;
        public bool IsCriticalValue { get => _isCriticalValue; set => SetProperty(ref _isCriticalValue, value); }

        private string _criticalValueNote;
        public string CriticalValueNote { get => _criticalValueNote; set => SetProperty(ref _criticalValueNote, value); }

        public ICommand SaveResultsCommand { get; }

        public LabTechResultsViewModel(LabTest test)
        {
            Test = test;
            ResultSummary = test.ResultSummary;
            ResultDetails = test.ResultDetails;
            ClinicalNotes = test.ClinicalNotes;
            ReferenceRange = test.ReferenceRange;
            NumericalValue = test.NumericalValue;
            Unit = test.Unit;
            IsCriticalValue = test.IsCriticalValue;
            CriticalValueNote = test.CriticalValueNote;
            SaveResultsCommand = new RelayCommand(ExecuteSaveResults);
        }

        private void ExecuteSaveResults()
        {
            if (string.IsNullOrWhiteSpace(ResultSummary)) return;

            Test.ResultSummary = ResultSummary;
            Test.ResultDetails = ResultDetails;
            Test.ClinicalNotes = ClinicalNotes;
            Test.ReferenceRange = ReferenceRange;
            Test.NumericalValue = NumericalValue;
            Test.Unit = Unit;
            Test.IsCriticalValue = IsCriticalValue;
            Test.CriticalValueNote = CriticalValueNote;
            Test.Status = "Completed";
            Test.CompletedDate = DateTime.Now;
            if (CurrentSession.Instance.LoggedInLabTechnician != null)
            {
                Test.LabTechnicianName = CurrentSession.Instance.LoggedInLabTechnician.FullName;
            }

            DataManager.SaveLabTests();
            DataManager.LogAudit(CurrentSession.Instance.LoggedInUser?.Username, $"Uploaded results for test: {Test.TestName}", "Laboratory");
            
            // Send notification to Patient
            DataManager.Notifications.Add(new Notification
            {
                PatientId = Test.PatientId,
                Title = "Lab Results Available",
                Message = $"Your results for {Test.TestName} are now available.",
                Type = "Result"
            });

            // Send notification to Doctor
            DataManager.Notifications.Add(new Notification
            {
                DoctorId = Test.DoctorId,
                Title = "Lab Results Ready",
                Message = $"Results for patient {Test.PatientName} ({Test.TestName}) are ready.",
                Type = "Laboratory"
            });

            // If critical, send urgent notification
            if (IsCriticalValue)
            {
                DataManager.Notifications.Add(new Notification
                {
                    DoctorId = Test.DoctorId,
                    Title = "URGENT: Critical Lab Value",
                    Message = $"CRITICAL VALUE ALERT for {Test.PatientName}: {Test.TestName}. {CriticalValueNote}",
                    Type = "Laboratory"
                });
                DataManager.LogAudit("Laboratory", $"CRITICAL VALUE ALERT sent for test: {Test.TestName}, Patient: {Test.PatientName}", "Alert");
            }

            DataManager.SaveNotifications();
            
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(true, null);
        }
    }
}
