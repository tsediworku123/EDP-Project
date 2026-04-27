using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class PatientLabResultsViewModel : ObservableObject
    {
        public ObservableCollection<TestResultItem> TestResults { get; } = new ObservableCollection<TestResultItem>();

        public PatientLabResultsViewModel()
        {
            LoadTestResults();
        }

        private void LoadTestResults()
        {
            DataManager.EnsureLoaded();
            var patient = DataManager.GetCurrentPatient();
            if (patient == null) return;

            var allTests = DataManager.GetPatientLabTests(patient.Id)
                .Where(t => t.Status == "Completed" || t.Status == "Results Ready")
                .OrderByDescending(t => t.RequestedDate)
                .ToList();

            TestResults.Clear();
            foreach (var test in allTests)
            {
                var doctor = DataManager.GetDoctorById(test.DoctorId);
                
                TestResults.Add(new TestResultItem
                {
                    TestId = test.Id,
                    TestName = test.TestName,
                    DoctorName = doctor != null ? $"Dr. {doctor.FullName}" : "Unknown",
                    RequestDate = test.RequestedDate,
                    CompletionDate = test.CompletedDate ?? test.RequestedDate,
                    ResultValue = $"{test.NumericalValue} {test.Unit}" ?? "Pending",
                    ReferenceRange = test.ReferenceRange ?? "N/A",
                    Notes = test.ClinicalNotes ?? "No additional notes"
                });
            }
        }
    }

    public class TestResultItem
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string DoctorName { get; set; }
        public System.DateTime RequestDate { get; set; }
        public System.DateTime CompletionDate { get; set; }
        public string ResultValue { get; set; }
        public string ReferenceRange { get; set; }
        public string Notes { get; set; }
    }
}
