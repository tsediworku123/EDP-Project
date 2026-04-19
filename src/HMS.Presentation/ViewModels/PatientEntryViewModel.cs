using HMS.Core.Common.Utils;
using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public partial class PatientEntryViewModel : ObservableObject
    {
        private readonly Window _window;
        private int _patientId = 0;

        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private DateTime _dateOfBirth = DateTime.Today.AddYears(-30);
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private string _gender = "Male";
        public string Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _allergies;
        public string Allergies { get => _allergies; set => SetProperty(ref _allergies, value); }

        private string _medicalNotes;
        public string MedicalNotes { get => _medicalNotes; set => SetProperty(ref _medicalNotes, value); }

        private string _emergencyName;
        public string EmergencyName { get => _emergencyName; set => SetProperty(ref _emergencyName, value); }

        private string _emergencyPhone;
        public string EmergencyPhone { get => _emergencyPhone; set => SetProperty(ref _emergencyPhone, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public PatientEntryViewModel(Window window)
        {
            _window = window;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void LoadPatient(Patient patient)
        {
            _patientId = patient.Id;
            FullName = patient.FullName;
            DateOfBirth = patient.DateOfBirth;
            Gender = patient.Gender;
            Phone = patient.Phone;
            Email = patient.Email;
            Address = patient.Address;
            Allergies = patient.AllergiesOrChronicConditions;
            MedicalNotes = patient.MedicalNotes;
            EmergencyName = patient.EmergencyContactName;
            EmergencyPhone = patient.EmergencyContactPhone;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Full Name and Phone are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataManager.EnsureLoaded();
            if (_patientId == 0) // New Patient
            {
                var newPatient = new Patient
                {
                    FullName = FullName,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Phone = Phone,
                    Email = Email,
                    Address = Address,
                    AllergiesOrChronicConditions = Allergies,
                    MedicalNotes = MedicalNotes,
                    EmergencyContactName = EmergencyName,
                    EmergencyContactPhone = EmergencyPhone,
                    IsActive = true
                };
                DataManager.RegisterPatient(newPatient); // DataManager handles ID and saving
            }
            else // Edit Existing
            {
                var p = DataManager.Patients.FirstOrDefault(x => x.Id == _patientId);
                if (p != null)
                {
                    p.FullName = FullName;
                    p.DateOfBirth = DateOfBirth;
                    p.Gender = Gender;
                    p.Phone = Phone;
                    p.Email = Email;
                    p.Address = Address;
                    p.AllergiesOrChronicConditions = Allergies;
                    p.MedicalNotes = MedicalNotes;
                    p.EmergencyContactName = EmergencyName;
                    p.EmergencyContactPhone = EmergencyPhone;
                    DataManager.SavePatients();
                }
            }
            MessageBox.Show("Patient record saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _window.DialogResult = true;
            _window.Close();
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}
