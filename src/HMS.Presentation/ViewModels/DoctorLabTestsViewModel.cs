using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorLabTestsViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private AppointmentItem _selectedAppointment;
        private string _testName;
        private string _selectedCategory;
        private string _clinicalNotes;
        private string _statusMsg;
        private bool _hasStatusMsg;
        private bool _hasError;

        public ObservableCollection<AppointmentItem> TodayAppointments { get; } = new ObservableCollection<AppointmentItem>();
        public ObservableCollection<LabTestItem> RequestedTests { get; } = new ObservableCollection<LabTestItem>();
        public string TodayDateDisplay => DateTime.Today.ToString("dddd, MMMM d yyyy");
        public bool HasNoAppointments => TodayAppointments.Count == 0;
        public bool HasTests => RequestedTests.Count > 0;

        public ObservableCollection<string> TestCategories { get; } = new ObservableCollection<string>
        {
            "Hematology", "Biochemistry", "Microbiology", "Immunology",
            "Radiology – X-Ray", "Radiology – CT Scan", "Radiology – MRI",
            "Radiology – Ultrasound", "Cardiology – ECG", "Pathology", "Urine Analysis", "Other"
        };

        public ObservableCollection<string> CommonTests { get; } = new ObservableCollection<string>
        {
            "Complete Blood Count (CBC)", "Blood Glucose (Fasting)", "HbA1c",
            "Lipid Panel", "Liver Function Tests (LFT)", "Kidney Function Tests (KFT)",
            "Thyroid Function (TSH)", "Urinalysis", "Chest X-Ray",
            "ECG (Electrocardiogram)", "Blood Culture", "Coagulation Profile"
        };

        public AppointmentItem SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                SetProperty(ref _selectedAppointment, value);
                OnPropertyChanged(nameof(HasSelection));
                OnPropertyChanged(nameof(SelectedPatientName));
                OnPropertyChanged(nameof(SelectedAppointmentInfo));
                RequestedTests.Clear();
                StatusMsg = string.Empty;
                HasStatusMsg = false;
            }
        }

        public bool HasSelection => _selectedAppointment != null;
        public string SelectedPatientName => _selectedAppointment?.PatientName ?? string.Empty;
        public string SelectedAppointmentInfo => _selectedAppointment != null
            ? $"Appointment #{_selectedAppointment.Source.Id} · {_selectedAppointment.TimeDisplay}"
            : string.Empty;

        public string TestName { get => _testName; set => SetProperty(ref _testName, value); }
        public string SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }
        public string ClinicalNotes { get => _clinicalNotes; set => SetProperty(ref _clinicalNotes, value); }
        public string StatusMsg { get => _statusMsg; set => SetProperty(ref _statusMsg, value); }
        public bool HasStatusMsg { get => _hasStatusMsg; set => SetProperty(ref _hasStatusMsg, value); }
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }

        public ICommand AddTestCommand { get; }
        public ICommand RemoveTestCommand { get; }
        public ICommand QuickAddTestCommand { get; }
        public ICommand SubmitLabOrderCommand { get; }

        public DoctorLabTestsViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            SelectedCategory = TestCategories[0];
            LoadAppointments();

            AddTestCommand = new RelayCommand(AddTest);
            RemoveTestCommand = new RelayCommand<LabTestItem>(t => { if (t != null) { RequestedTests.Remove(t); OnPropertyChanged(nameof(HasTests)); } });
            QuickAddTestCommand = new RelayCommand<string>(QuickAddTest);
            SubmitLabOrderCommand = new RelayCommand(SubmitLabOrder);
        }

        private void LoadAppointments()
        {
            TodayAppointments.Clear();
            if (_doctor == null) return;

            var appts = DataManager.Appointments
                .Where(a => a.DoctorId == _doctor.Id
                         && a.AppointmentDate.Date == DateTime.Today
                         && a.Status != "Cancelled")
                .OrderBy(a => a.AppointmentDate);

            foreach (var a in appts)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId);
                TodayAppointments.Add(new AppointmentItem
                {
                    Source = a,
                    PatientName = patient?.FullName ?? "Unknown Patient",
                    TimeDisplay = a.AppointmentDate.ToString("hh:mm tt"),
                    Reason = a.Reason
                });
            }
            OnPropertyChanged(nameof(HasNoAppointments));
        }

        private void AddTest()
        {
            if (string.IsNullOrWhiteSpace(TestName))
            {
                SetStatus("Please enter a test name.", true);
                return;
            }
            RequestedTests.Add(new LabTestItem
            {
                TestName = TestName.Trim(),
                Category = SelectedCategory
            });
            TestName = string.Empty;
            OnPropertyChanged(nameof(HasTests));
        }

        private void QuickAddTest(string test)
        {
            if (string.IsNullOrWhiteSpace(test)) return;
            if (RequestedTests.Any(t => t.TestName == test)) return;
            RequestedTests.Add(new LabTestItem { TestName = test, Category = SelectedCategory });
            OnPropertyChanged(nameof(HasTests));
        }

        private void SubmitLabOrder()
        {
            if (_selectedAppointment == null) { SetStatus("Please select an appointment.", true); return; }
            if (RequestedTests.Count == 0) { SetStatus("Add at least one test to the order.", true); return; }

            foreach (var item in RequestedTests)
            {
                DataManager.LabTests.Add(new LabTest
                {
                    Id = DataManager.LabTests.Any() ? DataManager.LabTests.Max(t => t.Id) + 1 : 1,
                    PatientId = _selectedAppointment.Source.PatientId,
                    DoctorId = _doctor.Id,
                    AppointmentId = _selectedAppointment.Source.Id,
                    TestName = item.TestName,
                    TestCategory = item.Category,
                    RequestedDate = DateTime.Now,
                    Status = "Requested",
                    ClinicalNotes = ClinicalNotes?.Trim() ?? string.Empty
                });
            }

            DataManager.SaveLabTests();
            int count = RequestedTests.Count;
            RequestedTests.Clear();
            ClinicalNotes = string.Empty;
            OnPropertyChanged(nameof(HasTests));
            SetStatus($"{count} lab test(s) ordered for {_selectedAppointment.PatientName}!", false);
        }

        private void SetStatus(string msg, bool isError)
        {
            StatusMsg = msg;
            HasError = isError;
            HasStatusMsg = true;
        }
    }

    public class LabTestItem
    {
        public string TestName { get; set; }
        public string Category { get; set; }
    }
}
