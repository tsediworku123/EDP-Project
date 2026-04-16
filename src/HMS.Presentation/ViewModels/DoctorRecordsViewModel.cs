using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorRecordsViewModel : ObservableObject
    {
        // ─── Private State ──────────────────────────────────────────────────────
        private readonly Doctor _doctor;

        private string _searchText = string.Empty;
        private string _filterPatient = "All Patients";
        private string _filterDiagnosis = "All Diagnoses";
        private string _statusMsg;
        private bool _hasStatusMsg;
        private bool _isSuccess;
        private bool _showForm;

        // Form fields
        private RecordItem _selectedRecord;
        private string _formTitle;
        private string _formDiagnosis;
        private string _formTreatment;
        private string _formPrescription;
        private string _formTestResults;
        private string _formNotes;
        private Patient _formPatient;
        private DateTime _formDate = DateTime.Today;
        private bool _isEditing;

        // ─── Collections ────────────────────────────────────────────────────────
        public ObservableCollection<RecordItem> AllRecords    { get; } = new ObservableCollection<RecordItem>();
        public ObservableCollection<RecordItem> FilteredRecords { get; } = new ObservableCollection<RecordItem>();
        public ObservableCollection<string>     PatientFilter { get; } = new ObservableCollection<string>();
        public ObservableCollection<string>     DiagnosisFilter { get; } = new ObservableCollection<string>();
        public ObservableCollection<Patient>    Patients      { get; } = new ObservableCollection<Patient>();

        // ─── Filter / Search Properties ─────────────────────────────────────────
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilters(); }
        }

        public string FilterPatient
        {
            get => _filterPatient;
            set { SetProperty(ref _filterPatient, value); ApplyFilters(); }
        }

        public string FilterDiagnosis
        {
            get => _filterDiagnosis;
            set { SetProperty(ref _filterDiagnosis, value); ApplyFilters(); }
        }

        // ─── Stats ──────────────────────────────────────────────────────────────
        public int TotalRecords   => AllRecords.Count;
        public int TotalPatients  => AllRecords.Select(r => r.Source.PatientId).Distinct().Count();
        public string LatestDate  => AllRecords.Count > 0
            ? AllRecords.OrderByDescending(r => r.Source.Date).First().DateDisplay
            : "—";

        // ─── Status Banner ───────────────────────────────────────────────────────
        public string StatusMsg       { get => _statusMsg;    set => SetProperty(ref _statusMsg,    value); }
        public bool   HasStatusMsg    { get => _hasStatusMsg; set => SetProperty(ref _hasStatusMsg, value); }
        public bool   IsSuccess       { get => _isSuccess;    set => SetProperty(ref _isSuccess,    value); }

        // ─── Form State ─────────────────────────────────────────────────────────
        public bool ShowForm          { get => _showForm;     set => SetProperty(ref _showForm,     value); }
        public bool IsEditing         { get => _isEditing;    set { SetProperty(ref _isEditing, value); OnPropertyChanged(nameof(FormHeader)); } }
        public string FormHeader      => _isEditing ? "Edit Medical Record" : "Add New Medical Record";

        public RecordItem SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                SetProperty(ref _selectedRecord, value);
                OnPropertyChanged(nameof(HasSelection));
                StatusMsg = string.Empty;
                HasStatusMsg = false;
            }
        }
        public bool HasSelection => _selectedRecord != null;

        // Form fields
        public string   FormTitle       { get => _formTitle;       set => SetProperty(ref _formTitle,       value); }
        public string   FormDiagnosis   { get => _formDiagnosis;   set => SetProperty(ref _formDiagnosis,   value); }
        public string   FormTreatment   { get => _formTreatment;   set => SetProperty(ref _formTreatment,   value); }
        public string   FormPrescription{ get => _formPrescription; set => SetProperty(ref _formPrescription, value); }
        public string   FormTestResults { get => _formTestResults;  set => SetProperty(ref _formTestResults,  value); }
        public string   FormNotes       { get => _formNotes;       set => SetProperty(ref _formNotes,       value); }
        public Patient  FormPatient     { get => _formPatient;     set => SetProperty(ref _formPatient,     value); }
        public DateTime FormDate        { get => _formDate;        set => SetProperty(ref _formDate,        value); }

        // ─── Commands ────────────────────────────────────────────────────────────
        public ICommand AddNewCommand    { get; }
        public ICommand EditCommand      { get; }
        public ICommand DeleteCommand    { get; }
        public ICommand SaveFormCommand  { get; }
        public ICommand CancelFormCommand{ get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand DismissBannerCommand { get; }

        // ─── Constructor ────────────────────────────────────────────────────────
        public DoctorRecordsViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;

            AddNewCommand       = new RelayCommand(OpenAddForm);
            EditCommand         = new RelayCommand(OpenEditForm,  () => HasSelection);
            DeleteCommand       = new RelayCommand(DeleteRecord,  () => HasSelection);
            SaveFormCommand     = new RelayCommand(SaveForm);
            CancelFormCommand   = new RelayCommand(CloseForm);
            ClearSearchCommand  = new RelayCommand(() => SearchText = string.Empty);
            DismissBannerCommand = new RelayCommand(() => { HasStatusMsg = false; StatusMsg = string.Empty; });

            LoadRecords();
        }

        // ─── Data Loading ───────────────────────────────────────────────────────
        private void LoadRecords()
        {
            AllRecords.Clear();
            PatientFilter.Clear();
            DiagnosisFilter.Clear();
            Patients.Clear();

            // Populate patient dropdown
            Patients.Add(null); // null = placeholder "Select patient"
            foreach (var p in DataManager.Patients.OrderBy(p => p.FullName))
                Patients.Add(p);

            // Build records for this doctor (or all if doctor is null – admin context)
            var records = _doctor != null
                ? DataManager.MedicalRecords.Where(r => r.DoctorId == _doctor.Id)
                : DataManager.MedicalRecords.AsEnumerable();

            foreach (var r in records.OrderByDescending(r => r.Date))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == r.PatientId);
                var doctor  = DataManager.Doctors.FirstOrDefault(d => d.Id == r.DoctorId);
                AllRecords.Add(new RecordItem
                {
                    Source      = r,
                    PatientName = patient?.FullName ?? $"Patient #{r.PatientId}",
                    DoctorName  = r.DoctorName ?? doctor?.FullName ?? $"Doctor #{r.DoctorId}",
                    DateDisplay = r.Date.ToString("dd MMM yyyy"),
                });
            }

            // Build filter lists
            PatientFilter.Add("All Patients");
            foreach (var name in AllRecords.Select(r => r.PatientName).Distinct().OrderBy(n => n))
                PatientFilter.Add(name);

            DiagnosisFilter.Add("All Diagnoses");
            foreach (var d in AllRecords.Select(r => r.Source.Diagnosis).Where(d => !string.IsNullOrWhiteSpace(d)).Distinct().OrderBy(d => d))
                DiagnosisFilter.Add(d);

            ApplyFilters();
            RefreshStats();
        }

        private void ApplyFilters()
        {
            FilteredRecords.Clear();
            var q = AllRecords.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var kw = _searchText.Trim().ToLower();
                q = q.Where(r =>
                    (r.PatientName?.ToLower().Contains(kw) == true) ||
                    (r.Source.Title?.ToLower().Contains(kw) == true) ||
                    (r.Source.Diagnosis?.ToLower().Contains(kw) == true) ||
                    (r.Source.Prescription?.ToLower().Contains(kw) == true));
            }

            if (_filterPatient != "All Patients" && !string.IsNullOrEmpty(_filterPatient))
                q = q.Where(r => r.PatientName == _filterPatient);

            if (_filterDiagnosis != "All Diagnoses" && !string.IsNullOrEmpty(_filterDiagnosis))
                q = q.Where(r => r.Source.Diagnosis == _filterDiagnosis);

            foreach (var r in q)
                FilteredRecords.Add(r);
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalRecords));
            OnPropertyChanged(nameof(TotalPatients));
            OnPropertyChanged(nameof(LatestDate));
        }

        // ─── Form Logic ─────────────────────────────────────────────────────────
        private void OpenAddForm()
        {
            IsEditing        = false;
            FormTitle        = string.Empty;
            FormDiagnosis    = string.Empty;
            FormTreatment    = string.Empty;
            FormPrescription = string.Empty;
            FormTestResults  = string.Empty;
            FormNotes        = string.Empty;
            FormPatient      = null;
            FormDate         = DateTime.Today;
            HasStatusMsg     = false;
            ShowForm         = true;
        }

        private void OpenEditForm()
        {
            if (_selectedRecord == null) return;
            var r = _selectedRecord.Source;
            IsEditing        = true;
            FormTitle        = r.Title;
            FormDiagnosis    = r.Diagnosis;
            FormTreatment    = r.Treatment;
            FormPrescription = r.Prescription;
            FormTestResults  = r.TestResults;
            FormNotes        = r.MedicalNotes;
            FormPatient      = DataManager.Patients.FirstOrDefault(p => p.Id == r.PatientId);
            FormDate         = r.Date;
            HasStatusMsg     = false;
            ShowForm         = true;
        }

        private void SaveForm()
        {
            if (string.IsNullOrWhiteSpace(FormTitle) || string.IsNullOrWhiteSpace(FormDiagnosis))
            {
                ShowBanner("Title and Diagnosis are required.", success: false);
                return;
            }

            if (!_isEditing && FormPatient == null)
            {
                ShowBanner("Please select a patient.", success: false);
                return;
            }

            if (_isEditing && _selectedRecord != null)
            {
                var r = _selectedRecord.Source;
                r.Title        = FormTitle.Trim();
                r.Diagnosis    = FormDiagnosis.Trim();
                r.Treatment    = FormTreatment?.Trim();
                r.Prescription = FormPrescription?.Trim();
                r.TestResults  = FormTestResults?.Trim() ?? "None";
                r.MedicalNotes = FormNotes?.Trim();
                r.Date         = FormDate;
                DataManager.SaveMedicalRecords();
                CloseForm();
                LoadRecords();
                ShowBanner("Record updated successfully.", success: true);
            }
            else
            {
                var newRecord = new MedicalRecord
                {
                    Id           = DataManager.MedicalRecords.Any() ? DataManager.MedicalRecords.Max(x => x.Id) + 1 : 1,
                    PatientId    = FormPatient.Id,
                    DoctorId     = _doctor?.Id ?? 0,
                    Title        = FormTitle.Trim(),
                    Diagnosis    = FormDiagnosis.Trim(),
                    Treatment    = FormTreatment?.Trim(),
                    Prescription = FormPrescription?.Trim(),
                    TestResults  = FormTestResults?.Trim() ?? "None",
                    MedicalNotes = FormNotes?.Trim(),
                    Date         = FormDate,
                    DoctorName   = _doctor?.FullName,
                };
                DataManager.MedicalRecords.Add(newRecord);
                DataManager.SaveMedicalRecords();
                CloseForm();
                LoadRecords();
                ShowBanner($"Record added for {FormPatient.FullName}.", success: true);
            }
        }

        private void DeleteRecord()
        {
            if (_selectedRecord == null) return;
            var r = _selectedRecord.Source;
            DataManager.MedicalRecords.Remove(r);
            DataManager.SaveMedicalRecords();
            AllRecords.Remove(_selectedRecord);
            FilteredRecords.Remove(_selectedRecord);
            SelectedRecord = null;
            RefreshStats();
            ShowBanner("Record deleted successfully.", success: true);
        }

        private void CloseForm() => ShowForm = false;

        private void ShowBanner(string msg, bool success)
        {
            StatusMsg    = msg;
            IsSuccess    = success;
            HasStatusMsg = true;
        }
    }

    // ─── DTO ─────────────────────────────────────────────────────────────────────
    public class RecordItem
    {
        public MedicalRecord Source      { get; set; }
        public string        PatientName { get; set; }
        public string        DoctorName  { get; set; }
        public string        DateDisplay { get; set; }
    }
}
