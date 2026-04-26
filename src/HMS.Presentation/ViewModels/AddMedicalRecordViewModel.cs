using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AddMedicalRecordViewModel : ObservableObject
    {
        private Patient _selectedPatient;
        private string _title;
        private string _diagnosis;
        private string _treatment;
        private string _prescription;
        private string _testResults;
        private string _clinicalNotes;
        private DateTime _date = DateTime.Today;
        private bool _isEditing;
        private readonly int _recordId;

        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();

        public Patient SelectedPatient { get => _selectedPatient; set => SetProperty(ref _selectedPatient, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Diagnosis { get => _diagnosis; set => SetProperty(ref _diagnosis, value); }
        public string Treatment { get => _treatment; set => SetProperty(ref _treatment, value); }
        public string Prescription { get => _prescription; set => SetProperty(ref _prescription, value); }
        public string TestResults { get => _testResults; set => SetProperty(ref _testResults, value); }
        public string ClinicalNotes { get => _clinicalNotes; set => SetProperty(ref _clinicalNotes, value); }
        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }
        public bool IsEditing { get => _isEditing; set { SetProperty(ref _isEditing, value); OnPropertyChanged(nameof(FormHeader)); } }
        public string FormHeader => IsEditing ? "EDIT MEDICAL RECORD" : "ADD NEW MEDICAL RECORD";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool> RequestClose;

        public AddMedicalRecordViewModel(MedicalRecord recordToEdit = null)
        {
            DataManager.EnsureLoaded();
            foreach (var p in DataManager.Patients.OrderBy(p => p.FullName))
                Patients.Add(p);

            if (recordToEdit != null)
            {
                IsEditing = true;
                _recordId = recordToEdit.Id;
                SelectedPatient = Patients.FirstOrDefault(p => p.Id == recordToEdit.PatientId);
                Title = recordToEdit.Title;
                Diagnosis = recordToEdit.Diagnosis;
                Treatment = recordToEdit.Treatment;
                Prescription = recordToEdit.Prescription;
                TestResults = recordToEdit.TestResults;
                ClinicalNotes = recordToEdit.MedicalNotes;
                Date = recordToEdit.Date;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
        }

        private void Save()
        {
            if (SelectedPatient == null || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Diagnosis))
                return;

            var doctor = CurrentSession.Instance.LoggedInDoctor;

            if (IsEditing)
            {
                var record = DataManager.MedicalRecords.FirstOrDefault(r => r.Id == _recordId);
                if (record != null)
                {
                    record.PatientId = SelectedPatient.Id;
                    record.Title = Title.Trim();
                    record.Diagnosis = Diagnosis.Trim();
                    record.Treatment = Treatment?.Trim();
                    record.Prescription = Prescription?.Trim();
                    record.TestResults = TestResults?.Trim() ?? "None";
                    record.MedicalNotes = ClinicalNotes?.Trim();
                    record.Date = Date;
                }
            }
            else
            {
                var newRecord = new MedicalRecord
                {
                    PatientId = SelectedPatient.Id,
                    DoctorId = doctor?.Id ?? 0,
                    Title = Title.Trim(),
                    Diagnosis = Diagnosis.Trim(),
                    Treatment = Treatment?.Trim(),
                    Prescription = Prescription?.Trim(),
                    TestResults = TestResults?.Trim() ?? "None",
                    MedicalNotes = ClinicalNotes?.Trim(),
                    Date = Date,
                    DoctorName = doctor?.FullName
                };
                DataManager.MedicalRecords.Add(newRecord);
            }

            DataManager.SaveMedicalRecords();
            RequestClose?.Invoke(true);
        }
    }
}
