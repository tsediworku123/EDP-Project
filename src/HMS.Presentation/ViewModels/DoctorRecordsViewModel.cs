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
        private RecordItem _selectedRecord;

        // ─── Collections ────────────────────────────────────────────────────────
        public ObservableCollection<RecordItem> AllRecords    { get; } = new ObservableCollection<RecordItem>();
        public ObservableCollection<RecordItem> FilteredRecords { get; } = new ObservableCollection<RecordItem>();
        public ObservableCollection<string>     PatientFilter { get; } = new ObservableCollection<string>();
        public ObservableCollection<string>     DiagnosisFilter { get; } = new ObservableCollection<string>();

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

        public RecordItem SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                SetProperty(ref _selectedRecord, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }
        public bool HasSelection => _selectedRecord != null;

        // ─── Commands ────────────────────────────────────────────────────────────
        public ICommand AddNewCommand    { get; }
        public ICommand EditCommand      { get; }
        public ICommand DeleteCommand    { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        // ─── Constructor ────────────────────────────────────────────────────────
        public DoctorRecordsViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;

            AddNewCommand       = new RelayCommand(OpenAddForm);
            EditCommand         = new RelayCommand<RecordItem>(OpenEditForm);
            DeleteCommand       = new RelayCommand<RecordItem>(DeleteRecord);
            ClearSearchCommand  = new RelayCommand(() => SearchText = string.Empty);
            ClearSelectionCommand = new RelayCommand(() => SelectedRecord = null);

            LoadRecords();
        }

        // ─── Data Loading ───────────────────────────────────────────────────────
        private void LoadRecords()
        {
            AllRecords.Clear();
            PatientFilter.Clear();
            DiagnosisFilter.Clear();

            // Build records for this doctor
            var records = _doctor != null
                ? DataManager.MedicalRecords.Where(r => r.DoctorId == _doctor.Id)
                : DataManager.MedicalRecords.AsEnumerable();

            foreach (var r in records.OrderByDescending(r => r.Date))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == r.PatientId);
                AllRecords.Add(new RecordItem
                {
                    Source      = r,
                    PatientName = patient?.FullName ?? $"Patient #{r.PatientId}",
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
                    (r.Source.Diagnosis?.ToLower().Contains(kw) == true));
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

        // ─── Dialog Logic ────────────────────────────────────────────────────────
        private async void OpenAddForm()
        {
            var vm = new AddMedicalRecordViewModel();
            var view = new HMS.Core.Views.AddMedicalRecordDialog { DataContext = vm };

            vm.RequestClose += success => {
                MaterialDesignThemes.Wpf.DialogHost.Close("MainDialogHost");
                if (success) LoadRecords();
            };

            await MaterialDesignThemes.Wpf.DialogHost.Show(view, "MainDialogHost");
        }

        private async void OpenEditForm(RecordItem item)
        {
            var target = item ?? _selectedRecord;
            if (target == null) return;

            var vm = new AddMedicalRecordViewModel(target.Source);
            var view = new HMS.Core.Views.AddMedicalRecordDialog { DataContext = vm };

            vm.RequestClose += success => {
                MaterialDesignThemes.Wpf.DialogHost.Close("MainDialogHost");
                if (success) LoadRecords();
            };

            await MaterialDesignThemes.Wpf.DialogHost.Show(view, "MainDialogHost");
        }

        private void DeleteRecord(RecordItem item)
        {
            var target = item ?? _selectedRecord;
            if (target == null) return;

            DataManager.MedicalRecords.Remove(target.Source);
            DataManager.SaveMedicalRecords();
            LoadRecords();
            SelectedRecord = null;
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
