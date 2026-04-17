using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorTestResultsViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private LabTestResultItem _selectedResult;
        private string _searchText;
        private string _filterStatus;

        public ObservableCollection<LabTestResultItem> AllResults { get; } = new ObservableCollection<LabTestResultItem>();
        public ObservableCollection<LabTestResultItem> FilteredResults { get; } = new ObservableCollection<LabTestResultItem>();

        public ObservableCollection<string> StatusFilters { get; } = new ObservableCollection<string>
        {
            "All", "Requested", "Sample Collected", "In Progress", "Completed", "Cancelled"
        };

        public LabTestResultItem SelectedResult
        {
            get => _selectedResult;
            set { SetProperty(ref _selectedResult, value); OnPropertyChanged(nameof(HasSelection)); }
        }

        public bool HasSelection => _selectedResult != null;
        public bool HasNoResults => FilteredResults.Count == 0;

        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public string FilterStatus
        {
            get => _filterStatus;
            set { SetProperty(ref _filterStatus, value); ApplyFilter(); }
        }

        public int TotalCount => AllResults.Count;
        public int CompletedCount => AllResults.Count(r => r.Source.Status == "Completed");
        public int PendingCount => AllResults.Count(r => r.Source.Status == "Requested" || r.Source.Status == "In Progress");

        public ICommand RefreshCommand { get; }
        public ICommand MarkCompletedCommand { get; }

        public DoctorTestResultsViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            FilterStatus = "All";
            RefreshCommand = new RelayCommand(LoadResults);
            MarkCompletedCommand = new RelayCommand(MarkCompleted);
            LoadResults();
        }

        private void LoadResults()
        {
            AllResults.Clear();
            if (_doctor == null) return;

            var tests = DataManager.LabTests
                .Where(t => t.DoctorId == _doctor.Id)
                .OrderByDescending(t => t.RequestedDate);

            foreach (var t in tests)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == t.PatientId);
                AllResults.Add(new LabTestResultItem
                {
                    Source = t,
                    PatientName = patient?.FullName ?? "Unknown",
                    RequestedDateDisplay = t.RequestedDate.ToString("dd MMM yyyy HH:mm"),
                    CompletedDateDisplay = t.CompletedDate.HasValue
                        ? t.CompletedDate.Value.ToString("dd MMM yyyy HH:mm") : "—"
                });
            }

            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(PendingCount));
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredResults.Clear();
            var query = AllResults.AsEnumerable();

            if (FilterStatus != "All")
                query = query.Where(r => r.Source.Status == FilterStatus);

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(r =>
                    r.PatientName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    r.Source.TestName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var item in query) FilteredResults.Add(item);
            OnPropertyChanged(nameof(HasNoResults));
        }

        private void MarkCompleted()
        {
            if (_selectedResult == null) return;
            _selectedResult.Source.Status = "Completed";
            _selectedResult.Source.CompletedDate = DateTime.Now;
            DataManager.SaveLabTests();
            LoadResults();
        }
    }

    public class LabTestResultItem
    {
        public LabTest Source { get; set; }
        public string PatientName { get; set; }
        public string RequestedDateDisplay { get; set; }
        public string CompletedDateDisplay { get; set; }
    }
}
