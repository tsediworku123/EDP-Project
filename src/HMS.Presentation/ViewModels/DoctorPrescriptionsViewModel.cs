using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorPrescriptionsViewModel : ObservableObject
    {
        private readonly Doctor _doctor;
        private PrescriptionItemDto _selectedPrescription;
        private string _searchText;

        public ObservableCollection<PrescriptionItemDto> Prescriptions { get; } = new ObservableCollection<PrescriptionItemDto>();
        public ObservableCollection<PrescriptionItemDto> FilteredPrescriptions { get; } = new ObservableCollection<PrescriptionItemDto>();

        public PrescriptionItemDto SelectedPrescription
        {
            get => _selectedPrescription;
            set { SetProperty(ref _selectedPrescription, value); OnPropertyChanged(nameof(HasSelection)); }
        }

        public bool HasSelection => _selectedPrescription != null;
        public bool HasNoResults => FilteredPrescriptions.Count == 0;

        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        public DoctorPrescriptionsViewModel()
        {
            DataManager.EnsureLoaded();
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            RefreshCommand = new RelayCommand(LoadPrescriptions);
            ViewDetailsCommand = new RelayCommand(ExecuteViewDetails);
            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            Prescriptions.Clear();
            if (_doctor == null) return;

            var items = DataManager.Prescriptions
                .Where(p => p.DoctorId == _doctor.Id)
                .OrderByDescending(p => p.PrescribedDate);

            foreach (var p in items)
            {
                var patient = DataManager.Patients.FirstOrDefault(pat => pat.Id == p.PatientId);
                Prescriptions.Add(new PrescriptionItemDto
                {
                    Source = p,
                    PatientName = patient?.FullName ?? "Unknown",
                    DateDisplay = p.PrescribedDate.ToString("dd MMM yyyy"),
                    MedicineSummary = string.Join(", ", p.Items.Select(i => i.MedicineName)),
                    Status = p.Status
                });
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredPrescriptions.Clear();
            var query = Prescriptions.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(p => 
                    p.PatientName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.MedicineSummary.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var item in query) FilteredPrescriptions.Add(item);
            OnPropertyChanged(nameof(HasNoResults));
        }

        private void ExecuteViewDetails()
        {
            if (SelectedPrescription == null) return;
            // Optionally open a dialog showing items details
            System.Windows.MessageBox.Show(
                $"Prescription for {SelectedPrescription.PatientName}\n" +
                $"Medicines: {SelectedPrescription.MedicineSummary}\n" +
                $"Status: {SelectedPrescription.Status}",
                "Prescription Details", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }

    public class PrescriptionItemDto
    {
        public Prescription Source { get; set; }
        public string PatientName { get; set; }
        public string DateDisplay { get; set; }
        public string MedicineSummary { get; set; }
        public string Status { get; set; }
    }
}
