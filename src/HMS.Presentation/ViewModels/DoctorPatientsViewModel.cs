using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using HMS.Core.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

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
        public ICommand AddPatientCommand { get; }
        public ICommand EditPatientCommand { get; }
        public ICommand DeletePatientCommand { get; }

        public DoctorPatientsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshData();

            RefreshCommand = new RelayCommand(RefreshData);
            DeletePatientCommand = new RelayCommand<Patient>(DeletePatient);
            AddPatientCommand = new RelayCommand(AddPatient);
            EditPatientCommand = new RelayCommand<Patient>(EditPatient);
        }

        private async void AddPatient()
        {
            var view = new AddPatientDialog { DataContext = new AddPatientViewModel() };
            await DialogHost.Show(view, "MainDialogHost");
            RefreshData();
        }

        private async void EditPatient(Patient patient)
        {
            if (patient == null) return;
            var view = new AddPatientDialog { DataContext = new AddPatientViewModel(patient) };
            await DialogHost.Show(view, "MainDialogHost");
            RefreshData();
        }

        private void RefreshData()
        {
            if (DataManager.Patients != null)
                Patients = new ObservableCollection<Patient>(DataManager.Patients);
        }

        private void FilterPatients()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                RefreshData();
                return;
            }

            var query = SearchText.ToLower();
            var filtered = DataManager.Patients?.Where(p => 
                (p.FullName != null && p.FullName.ToLower().Contains(query)) || 
                (p.PatientCode != null && p.PatientCode.ToLower().Contains(query))
            ).ToList();

            Patients = new ObservableCollection<Patient>(filtered ?? new List<Patient>());
        }

        private void DeletePatient(Patient patient)
        {
            if (patient == null) return;
            var res = MessageBox.Show($"Delete records for {patient.FullName}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                DataManager.Patients.Remove(patient);
                DataManager.SaveAllData();
                RefreshData();
            }
        }
    }
}
