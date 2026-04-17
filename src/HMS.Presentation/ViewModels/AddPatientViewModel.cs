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

        private string _grandfatherName;
        public string GrandfatherName { get => _grandfatherName; set => SetProperty(ref _grandfatherName, value); }

        private DateTime _dateOfBirth = DateTime.Now.AddYears(-30);
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _gender = "Male";
        public string Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _allergiesOrChronicConditions;
        public string AllergiesOrChronicConditions { get => _allergiesOrChronicConditions; set => SetProperty(ref _allergiesOrChronicConditions, value); }

        private string _emergencyName;
        public string EmergencyName { get => _emergencyName; set => SetProperty(ref _emergencyName, value); }

        private string _emergencyPhone;
        public string EmergencyPhone { get => _emergencyPhone; set => SetProperty(ref _emergencyPhone, value); }

        private string _emergencyContact;
        public string EmergencyContact { get => _emergencyContact; set => SetProperty(ref _emergencyContact, value); }

        private string _medicalNotes;
        public string MedicalNotes { get => _medicalNotes; set => SetProperty(ref _medicalNotes, value); }

        private string _bloodGroup = "Unknown";
        public string BloodGroup { get => _bloodGroup; set => SetProperty(ref _bloodGroup, value); }

        private string _currentMedications = "None";
        public string CurrentMedications { get => _currentMedications; set => SetProperty(ref _currentMedications, value); }

        private string _chronicConditions = "None";
        public string ChronicConditions { get => _chronicConditions; set => SetProperty(ref _chronicConditions, value); }

        private string _preferredLanguage;
        public string PreferredLanguage { get => _preferredLanguage; set => SetProperty(ref _preferredLanguage, value); }


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
                GrandfatherName = patient.GrandfatherName;
                DateOfBirth = patient.DateOfBirth;
                Phone = patient.Phone;
                Address = patient.Address;
                Gender = patient.Gender;
                Email = patient.Email;
                AllergiesOrChronicConditions = patient.AllergiesOrChronicConditions;
                EmergencyName = patient.EmergencyContactName;
                EmergencyPhone = patient.EmergencyContactPhone;
                EmergencyContact = patient.EmergencyContact;
                MedicalNotes = patient.MedicalNotes;
                BloodGroup = patient.BloodGroup;
                CurrentMedications = patient.CurrentMedications;
                ChronicConditions = patient.ChronicConditions;
                PreferredLanguage = patient.PreferredLanguage;
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
                _existingPatient.GrandfatherName = GrandfatherName;
                _existingPatient.DateOfBirth = DateOfBirth;
                _existingPatient.Phone = Phone;
                _existingPatient.Address = Address;
                _existingPatient.Gender = Gender;
                _existingPatient.Email = Email;
                _existingPatient.AllergiesOrChronicConditions = AllergiesOrChronicConditions;
                _existingPatient.EmergencyContactName = EmergencyName;
                _existingPatient.EmergencyContactPhone = EmergencyPhone;
                _existingPatient.EmergencyContact = EmergencyContact;
                _existingPatient.MedicalNotes = MedicalNotes;
                _existingPatient.BloodGroup = BloodGroup;
                _existingPatient.CurrentMedications = CurrentMedications;
                _existingPatient.ChronicConditions = ChronicConditions;
                _existingPatient.PreferredLanguage = PreferredLanguage;
            }
            else
            {
                var newId = DataManager.Patients.Any() ? DataManager.Patients.Max(p => p.Id) + 1 : 1;
                var newPatient = new Patient
                {
                    Id = newId,
                    FullName = FullName,
                    GrandfatherName = GrandfatherName,
                    DateOfBirth = DateOfBirth,
                    Phone = Phone,
                    Address = Address,
                    Gender = Gender,
                    Email = Email,
                    AllergiesOrChronicConditions = AllergiesOrChronicConditions,
                    EmergencyContactName = EmergencyName,
                    EmergencyContactPhone = EmergencyPhone,
                    EmergencyContact = EmergencyContact,
                    MedicalNotes = MedicalNotes,
                    BloodGroup = BloodGroup,
                    CurrentMedications = CurrentMedications,
                    ChronicConditions = ChronicConditions,
                    PreferredLanguage = PreferredLanguage,
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
