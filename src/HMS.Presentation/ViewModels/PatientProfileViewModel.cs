using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System.Windows.Input;
using System;
using System.Windows;

namespace HMS.Core.ViewModels
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
