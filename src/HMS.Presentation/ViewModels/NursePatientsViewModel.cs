using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

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

        public NursePatientsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshCommand = new RelayCommand(LoadPatients);
            LoadPatients();
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
