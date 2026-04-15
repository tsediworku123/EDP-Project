using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.ViewModels
{
    public class AddPatientViewModel : ObservableObject
    {
        private readonly Patient _existingPatient;
        private readonly bool _isEditing;

        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private DateTime _dateOfBirth = DateTime.Now.AddYears(-30);
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _gender = "Male";
        public string Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private string _bloodGroup = "Unknown";
        public string BloodGroup { get => _bloodGroup; set => SetProperty(ref _bloodGroup, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _emergencyName;
        public string EmergencyName { get => _emergencyName; set => SetProperty(ref _emergencyName, value); }

        private string _emergencyPhone;
        public string EmergencyPhone { get => _emergencyPhone; set => SetProperty(ref _emergencyPhone, value); }

        private string _medicalNotes;
        public string MedicalNotes { get => _medicalNotes; set => SetProperty(ref _medicalNotes, value); }

        public string DialogTitle => _isEditing ? $"UPDATE RECORD: {FullName}" : "NEW PATIENT REGISTRATION";
        public string ActionButtonText => _isEditing ? "SAVE UPDATES" : "REGISTER PATIENT";

        public ICommand SaveCommand { get; }

        public AddPatientViewModel(Patient patient = null)
        {
            if (patient != null)
            {
                _existingPatient = patient;
                _isEditing = true;
                
                FullName = patient.FullName;
                DateOfBirth = patient.DateOfBirth;
                Phone = patient.Phone;
                Gender = patient.Gender;
                BloodGroup = patient.BloodGroup;
                Address = patient.Address;
                EmergencyName = patient.EmergencyContactName;
                EmergencyPhone = patient.EmergencyContactPhone;
                MedicalNotes = patient.MedicalNotes;
            }

            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Full Name and Phone Number are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEditing && _existingPatient != null)
            {
                _existingPatient.FullName = FullName;
                _existingPatient.DateOfBirth = DateOfBirth;
                _existingPatient.Phone = Phone;
                _existingPatient.Gender = Gender;
                _existingPatient.BloodGroup = BloodGroup;
                _existingPatient.Address = Address;
                _existingPatient.EmergencyContactName = EmergencyName;
                _existingPatient.EmergencyContactPhone = EmergencyPhone;
                _existingPatient.MedicalNotes = MedicalNotes;
            }
            else
            {
                var newId = DataManager.Patients.Any() ? DataManager.Patients.Max(p => p.Id) + 1 : 1;
                var newPatient = new Patient
                {
                    Id = newId,
                    FullName = FullName,
                    DateOfBirth = DateOfBirth,
                    Phone = Phone,
                    Gender = Gender,
                    BloodGroup = BloodGroup,
                    Address = Address,
                    EmergencyContactName = EmergencyName,
                    EmergencyContactPhone = EmergencyPhone,
                    MedicalNotes = MedicalNotes,
                    PatientCode = $"PAT-{newId:D5}",
                    IsActive = true
                };
                DataManager.Patients.Add(newPatient);
            }

            DataManager.SaveAllData();
            DialogHost.CloseDialogCommand.Execute(true, null);
            MessageBox.Show($"Registry records for {FullName} have been processed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
