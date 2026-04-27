using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Text;

namespace HMS.Core.ViewModels
{
    public class NursePatientsViewModel : ObservableObject
    {
        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); LoadPatients(); }
        }

        public int TotalPatientsCount => DataManager.Patients.Count;
        public ICommand RefreshCommand { get; }
        public ICommand ViewMedicalHistoryCommand { get; }

        public NursePatientsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshCommand = new RelayCommand(LoadPatients);
            ViewMedicalHistoryCommand = new RelayCommand<Patient>(ExecuteViewMedicalHistory);
            LoadPatients();
        }

        private void ExecuteViewMedicalHistory(Patient patient)
        {
            if (patient == null) return;

            var records = DataManager.MedicalRecords.Where(r => r.PatientId == patient.Id).ToList();
            if (!records.Any())
            {
                MessageBox.Show($"No medical records found for {patient.FullName}.", "Medical History", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Medical History for {patient.FullName}:");
            sb.AppendLine("------------------------------------");
            foreach (var r in records.OrderByDescending(r => r.Date))
            {
                sb.AppendLine($"[{r.Date:MMM dd, yyyy}] {r.Diagnosis}");
                sb.AppendLine($"Treatment: {r.Treatment}");
                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString(), "Medical History", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadPatients()
        {
            Patients.Clear();
            var query = DataManager.Patients.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var s = SearchText.ToLower();
                query = query.Where(p => p.FullName.ToLower().Contains(s) ||
                                         (p.PatientCode != null && p.PatientCode.ToLower().Contains(s)));
            }
            foreach (var p in query.OrderBy(p => p.FullName))
                Patients.Add(p);
            OnPropertyChanged(nameof(TotalPatientsCount));
        }
    }
}
