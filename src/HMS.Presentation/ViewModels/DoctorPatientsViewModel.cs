using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorPatientsViewModel : ObservableObject
    {
        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set
            {
                if (SetProperty(ref _patients, value))
                {
                    OnPropertyChanged(nameof(TotalPatientsCount));
                    OnPropertyChanged(nameof(ActivePatientsCount));
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterPatients();
                }
            }
        }

        public int TotalPatientsCount => Patients?.Count ?? 0;
        public int ActivePatientsCount => Patients?.Count(p => p.IsActive) ?? 0;

        public ICommand RefreshCommand { get; }

        public DoctorPatientsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshData();

            RefreshCommand = new RelayCommand(RefreshData);
        }

        private void RefreshData()
        {
            if (DataManager.Patients != null)
            {
                var currentDoctor = CurrentSession.Instance.LoggedInDoctor;
                if (currentDoctor != null)
                {
                    var patientIds = DataManager.Appointments
                        .Where(a => a.DoctorId == currentDoctor.Id)
                        .Select(a => a.PatientId)
                        .Distinct();
                    
                    var myPatients = DataManager.Patients.Where(p => patientIds.Contains(p.Id)).ToList();
                    Patients = new ObservableCollection<Patient>(myPatients);
                }
                else
                {
                    Patients = new ObservableCollection<Patient>(DataManager.Patients);
                }
            }
        }

        private void FilterPatients()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                RefreshData();
                return;
            }

            var currentDoctor = CurrentSession.Instance.LoggedInDoctor;
            var source = DataManager.Patients.AsEnumerable();
            
            if (currentDoctor != null)
            {
                var patientIds = DataManager.Appointments
                    .Where(a => a.DoctorId == currentDoctor.Id)
                    .Select(a => a.PatientId)
                    .Distinct();
                source = source.Where(p => patientIds.Contains(p.Id));
            }

            var query = SearchText.ToLower();
            var filtered = source.Where(p => 
                (p.FullName != null && p.FullName.ToLower().Contains(query)) || 
                (p.PatientCode != null && p.PatientCode.ToLower().Contains(query))
            ).ToList();

            Patients = new ObservableCollection<Patient>(filtered ?? new List<Patient>());
        }
    }
}
