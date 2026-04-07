using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;
using System.Windows.Input;
using System;
using System.Windows;

namespace ClinicAppointmentSystem.ViewModels
{
    public class PatientProfileViewModel : ObservableObject
    {
        private Patient _patient;
        public Patient Patient { get => _patient; set => SetProperty(ref _patient, value); }

        public ICommand UpdateCommand { get; }

        public PatientProfileViewModel()
        {
            LoadData();
            UpdateCommand = new RelayCommand(UpdateInfo);
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            Patient = DataManager.GetCurrentPatient();
        }

        private void UpdateInfo()
        {
            DataManager.SavePatients();
            MessageBox.Show("Clinical profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
