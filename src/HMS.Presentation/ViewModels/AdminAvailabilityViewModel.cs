using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminAvailabilityViewModel : ObservableObject
    {
        private Doctor _selectedDoctor;
        private string _searchText;
        private bool _isEditing;

        public ObservableCollection<Doctor> Doctors { get; } = new ObservableCollection<Doctor>();
        public ObservableCollection<Doctor> FilteredDoctors { get; } = new ObservableCollection<Doctor>();

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                if (SetProperty(ref _selectedDoctor, value))
                {
                    IsEditing = value != null;
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterDoctors();
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand ResetCommand { get; }

        public AdminAvailabilityViewModel()
        {
            LoadDoctors();
            SaveCommand = new RelayCommand(SaveAvailability);
            ResetCommand = new RelayCommand(() => SelectedDoctor = null);
        }

        private void LoadDoctors()
        {
            DataManager.EnsureLoaded();
            Doctors.Clear();
            foreach (var d in DataManager.Doctors) Doctors.Add(d);
            FilterDoctors();
        }

        private void FilterDoctors()
        {
            FilteredDoctors.Clear();
            var query = Doctors.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(d => d.FullName.ToLower().Contains(SearchText.ToLower()) || 
                                       d.Specialization.ToLower().Contains(SearchText.ToLower()));
            }
            foreach (var d in query) FilteredDoctors.Add(d);
        }

        private void SaveAvailability()
        {
            if (SelectedDoctor == null) return;

            try
            {
                DataManager.SaveDoctors();
                MessageBox.Show($"Availability settings for {SelectedDoctor.FullName} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                IsEditing = false;
                SelectedDoctor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
