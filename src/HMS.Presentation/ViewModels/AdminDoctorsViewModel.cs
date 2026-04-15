using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminDoctorsViewModel : ObservableObject
    {
        private ObservableCollection<Doctor> _doctors;
        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set => SetProperty(ref _doctors, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterDoctors();
                }
            }
        }

        public int TotalActiveDoctors => Doctors?.Count(d => d.IsActive) ?? 0;
        public int TotalSpecialties => DataManager.Doctors?.Select(d => d.Specialization).Distinct().Count() ?? 0;

        public ICommand AddDoctorCommand { get; }
        public ICommand EditDoctorCommand { get; }
        public ICommand DeleteDoctorCommand { get; }
        public ICommand RefreshCommand { get; }

        public AdminDoctorsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshData();

            AddDoctorCommand = new RelayCommand(AddDoctor);
            EditDoctorCommand = new RelayCommand<Doctor>(EditDoctor);
            DeleteDoctorCommand = new RelayCommand<Doctor>(DeleteDoctor);
            RefreshCommand = new RelayCommand(RefreshData);
        }

        private void RefreshData()
        {
            if (DataManager.Doctors != null)
                Doctors = new ObservableCollection<Doctor>(DataManager.Doctors);
        }

        private void FilterDoctors()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                RefreshData();
                return;
            }

            var filtered = DataManager.Doctors?.Where(d => 
                d.FullName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) || 
                d.Specialization.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)
            ).ToList();

            Doctors = new ObservableCollection<Doctor>(filtered ?? new List<Doctor>());
        }

        private void AddDoctor()
        {
            MessageBox.Show("The 'Add Doctor' modern dialog is being prepared. For now, you can add data directly to the doctors.json file.", "Module in Progress", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditDoctor(Doctor doctor)
        {
            if (doctor == null) return;
            MessageBox.Show($"Editing profile for {doctor.FullName}. Changes will be saved to the database in the next build.", "Edit Doctor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteDoctor(Doctor doctor)
        {
            if (doctor == null) return;

            var result = MessageBox.Show($"Are you sure you want to remove {doctor.FullName} from the system?", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.OK)
            {
                DataManager.Doctors.Remove(doctor);
                DataManager.SaveData();
                RefreshData();
                OnPropertyChanged(nameof(TotalActiveDoctors));
            }
        }
    }
}
