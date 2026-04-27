using HMS.Core.Common.Utils;
using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public partial class AddNewUserViewModel : ObservableObject
    {
        private readonly Window _window;



        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (SetProperty(ref _selectedRole, value))
                    OnSelectedRoleChanged(value);
            }
        }

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor 
        { 
            get => _selectedDoctor; 
            set 
            {
                if (SetProperty(ref _selectedDoctor, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient 
        { 
            get => _selectedPatient; 
            set 
            {
                if (SetProperty(ref _selectedPatient, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        private Nurse _selectedNurse;
        public Nurse SelectedNurse 
        { 
            get => _selectedNurse; 
            set 
            {
                if (SetProperty(ref _selectedNurse, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        private Pharmacist _selectedPharmacist;
        public Pharmacist SelectedPharmacist 
        { 
            get => _selectedPharmacist; 
            set 
            {
                if (SetProperty(ref _selectedPharmacist, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        private LabTechnician _selectedLabTech;
        public LabTechnician SelectedLabTech 
        { 
            get => _selectedLabTech; 
            set 
            {
                if (SetProperty(ref _selectedLabTech, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        private BillingStaff _selectedBillingStaff;
        public BillingStaff SelectedBillingStaff 
        { 
            get => _selectedBillingStaff; 
            set 
            {
                if (SetProperty(ref _selectedBillingStaff, value))
                    Email = (value != null && !string.IsNullOrWhiteSpace(value.Email)) ? value.Email : string.Empty;
            } 
        }

        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string> 
        { 
            User.Admin, 
            User.Doctor, 
            User.Nurse, 
            User.Pharmacist, 
            User.LabTechnician, 
            User.Receptionist, 
            User.Patient 
        };
        
        public ObservableCollection<Doctor> Doctors { get; } = new ObservableCollection<Doctor>();
        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();
        public ObservableCollection<Nurse> Nurses { get; } = new ObservableCollection<Nurse>();
        public ObservableCollection<Pharmacist> Pharmacists { get; } = new ObservableCollection<Pharmacist>();
        public ObservableCollection<LabTechnician> LabTechs { get; } = new ObservableCollection<LabTechnician>();
        public ObservableCollection<BillingStaff> BillingStaffs { get; } = new ObservableCollection<BillingStaff>();

        private bool _isDoctorRoleSelected;
        public bool IsDoctorRoleSelected { get => _isDoctorRoleSelected; set => SetProperty(ref _isDoctorRoleSelected, value); }

        private bool _isPatientRoleSelected;
        public bool IsPatientRoleSelected { get => _isPatientRoleSelected; set => SetProperty(ref _isPatientRoleSelected, value); }

        private bool _isNurseRoleSelected;
        public bool IsNurseRoleSelected { get => _isNurseRoleSelected; set => SetProperty(ref _isNurseRoleSelected, value); }

        private bool _isPharmacistRoleSelected;
        public bool IsPharmacistRoleSelected { get => _isPharmacistRoleSelected; set => SetProperty(ref _isPharmacistRoleSelected, value); }

        private bool _isLabTechRoleSelected;
        public bool IsLabTechRoleSelected { get => _isLabTechRoleSelected; set => SetProperty(ref _isLabTechRoleSelected, value); }

        private bool _isReceptionistRoleSelected;
        public bool IsReceptionistRoleSelected { get => _isReceptionistRoleSelected; set => SetProperty(ref _isReceptionistRoleSelected, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddNewUserViewModel(Window window)
        {
            _window = window;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            LoadData();
        }

        private void OnSelectedRoleChanged(string value)
        {
            IsDoctorRoleSelected = value == User.Doctor;
            IsPatientRoleSelected = value == User.Patient;
            IsNurseRoleSelected = value == User.Nurse;
            IsPharmacistRoleSelected = value == User.Pharmacist;
            IsLabTechRoleSelected = value == User.LabTechnician;
            IsReceptionistRoleSelected = value == User.Receptionist;
            
            // Clear selections and email when role changes
            _selectedDoctor = null;
            _selectedPatient = null;
            _selectedNurse = null;
            _selectedPharmacist = null;
            _selectedLabTech = null;
            _selectedBillingStaff = null;
            
            // Notify property changes for the backing fields we just cleared manually
            OnPropertyChanged(nameof(SelectedDoctor));
            OnPropertyChanged(nameof(SelectedPatient));
            OnPropertyChanged(nameof(SelectedNurse));
            OnPropertyChanged(nameof(SelectedPharmacist));
            OnPropertyChanged(nameof(SelectedLabTech));
            OnPropertyChanged(nameof(SelectedBillingStaff));
            
            Email = string.Empty;
            
            // Clear passwords when role changes
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            
            Doctors.Clear();
            foreach (var doc in DataManager.Doctors) Doctors.Add(doc);
            
            Patients.Clear();
            foreach (var pat in DataManager.Patients) Patients.Add(pat);
            
            Nurses.Clear();
            foreach (var nurse in DataManager.Nurses) Nurses.Add(nurse);
            
            Pharmacists.Clear();
            foreach (var pharm in DataManager.Pharmacists) Pharmacists.Add(pharm);
            
            LabTechs.Clear();
            foreach (var tech in DataManager.LabTechnicians) LabTechs.Add(tech);
            
            BillingStaffs.Clear();
            foreach (var staff in DataManager.BillingStaff) BillingStaffs.Add(staff);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(SelectedRole))
            {
                MessageBox.Show("Please fill all required fields (*).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedRole == "Doctor" && SelectedDoctor == null)
            {
                MessageBox.Show("Please select a doctor to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedRole == "Patient" && SelectedPatient == null)
            {
                MessageBox.Show("Please select a patient to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedRole == "Nurse" && SelectedNurse == null)
            {
                MessageBox.Show("Please select a nurse to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedRole == "Pharmacist" && SelectedPharmacist == null)
            {
                MessageBox.Show("Please select a pharmacist to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedRole == "LabTechnician" && SelectedLabTech == null)
            {
                MessageBox.Show("Please select a lab technician to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedRole == "Receptionist" && SelectedBillingStaff == null)
            {
                MessageBox.Show("Please select a billing staff / receptionist to link this user account to.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var users = DataManager.Users;
            
            // Check if this specific profile already has an account linked to it
            User existingLink = null;
            if (SelectedRole == "Doctor") existingLink = users.FirstOrDefault(u => u.DoctorId == SelectedDoctor.Id);
            else if (SelectedRole == "Nurse") existingLink = users.FirstOrDefault(u => u.NurseId == SelectedNurse.Id);
            else if (SelectedRole == "Pharmacist") existingLink = users.FirstOrDefault(u => u.PharmacistId == SelectedPharmacist.Id);
            else if (SelectedRole == "LabTechnician") existingLink = users.FirstOrDefault(u => u.LabTechnicianId == SelectedLabTech.Id);
            else if (SelectedRole == "Receptionist") existingLink = users.FirstOrDefault(u => u.BillingStaffId == SelectedBillingStaff.Id);
            else if (SelectedRole == "Patient") existingLink = users.FirstOrDefault(u => u.PatientId == SelectedPatient.Id);

            if (existingLink != null)
            {
                MessageBox.Show($"This {SelectedRole} already has a user account linked to them (Email: {existingLink.Email}).\n\nYou don't need to create a new one.", "Duplicate Account", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if the email is used by someone else
            if (users.Any(u => u.Email != null && u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"The email '{Email}' is already in use by another account. Please use a different email or check the existing accounts.", "Email Already Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new User
            {
                Password = PasswordHasher.HashPassword(Password),
                Role = SelectedRole,
                DoctorId = SelectedRole == "Doctor" ? SelectedDoctor.Id : (int?)null,
                PatientId = SelectedRole == "Patient" ? SelectedPatient.Id : (int?)null,
                NurseId = SelectedRole == "Nurse" ? SelectedNurse.Id : (int?)null,
                PharmacistId = SelectedRole == "Pharmacist" ? SelectedPharmacist.Id : (int?)null,
                LabTechnicianId = SelectedRole == "LabTechnician" ? SelectedLabTech.Id : (int?)null,
                BillingStaffId = SelectedRole == "Receptionist" ? SelectedBillingStaff.Id : (int?)null,
                Email = Email,
                IsActive = true
            };

            users.Add(newUser);
            DataManager.SaveUsers();

            MessageBox.Show($"User with email '{Email}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
