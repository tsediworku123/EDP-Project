using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Text.RegularExpressions;

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

        // Login credential fields — only required for new registration, not editing
        private string _loginEmail;
        public string LoginEmail { get => _loginEmail; set => SetProperty(ref _loginEmail, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }

        // Controls whether credential section is visible in the dialog
        public bool ShowCredentialSection => !_isEditing;

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

            // For new patients, validate login credentials
            if (!_isEditing)
            {
                if (string.IsNullOrWhiteSpace(LoginEmail))
                {
                    MessageBox.Show("Login Email is required for new patient registration.\nThis will be used as the patient's login credential.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(LoginEmail.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 4)
                {
                    MessageBox.Show("Password is required and must be at least 4 characters.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check if email is already taken
                if (DataManager.Users.Any(u => u.Email != null && u.Email.Equals(LoginEmail.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("This email is already associated with an existing user account.\nPlease use a different email.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
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

                DataManager.SavePatients();
            }
            else
            {
                var newPatient = new Patient
                {
                    FullName = FullName,
                    GrandfatherName = GrandfatherName,
                    DateOfBirth = DateOfBirth,
                    Phone = Phone,
                    Address = Address,
                    Gender = Gender,
                    Email = LoginEmail?.Trim(),
                    AllergiesOrChronicConditions = AllergiesOrChronicConditions,
                    EmergencyContactName = EmergencyName,
                    EmergencyContactPhone = EmergencyPhone,
                    EmergencyContact = EmergencyContact,
                    MedicalNotes = MedicalNotes,
                    BloodGroup = BloodGroup,
                    CurrentMedications = CurrentMedications,
                    ChronicConditions = ChronicConditions,
                    PreferredLanguage = PreferredLanguage,
                    IsActive = true
                };
                
                // RegisterPatient saves the patient and auto-creates the linked User account
                DataManager.RegisterPatientWithCredentials(newPatient, LoginEmail.Trim(), Password);
            }

            DialogHost.CloseDialogCommand.Execute(true, null);
            MessageBox.Show($"Registry records for {FullName} have been processed.\n" +
                          (!_isEditing ? $"Login account created with email: {LoginEmail?.Trim()}" : ""), 
                          "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
