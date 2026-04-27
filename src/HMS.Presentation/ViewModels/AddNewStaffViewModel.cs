using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.ViewModels
{
    public class AddNewStaffViewModel : ObservableObject
    {
        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (SetProperty(ref _selectedRole, value))
                    UpdateRoleVisibility();
            }
        }

        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>
        {
            "Doctor", "Nurse", "Pharmacist", "Lab Technician", "Billing Staff"
        };

        // Role specific fields
        private string _specialization;
        public string Specialization { get => _specialization; set => SetProperty(ref _specialization, value); }

        private string _department;
        public string Department { get => _department; set => SetProperty(ref _department, value); }

        private string _licenseNumber;
        public string LicenseNumber { get => _licenseNumber; set => SetProperty(ref _licenseNumber, value); }

        private string _employeeCode;
        public string EmployeeCode { get => _employeeCode; set => SetProperty(ref _employeeCode, value); }

        private string _position;
        public string Position { get => _position; set => SetProperty(ref _position, value); }

        // Visibility properties
        private bool _isDoctorFieldsVisible;
        public bool IsDoctorFieldsVisible { get => _isDoctorFieldsVisible; set => SetProperty(ref _isDoctorFieldsVisible, value); }

        private bool _isNurseFieldsVisible;
        public bool IsNurseFieldsVisible { get => _isNurseFieldsVisible; set => SetProperty(ref _isNurseFieldsVisible, value); }

        private bool _isPharmacistFieldsVisible;
        public bool IsPharmacistFieldsVisible { get => _isPharmacistFieldsVisible; set => SetProperty(ref _isPharmacistFieldsVisible, value); }

        private bool _isLabTechFieldsVisible;
        public bool IsLabTechFieldsVisible { get => _isLabTechFieldsVisible; set => SetProperty(ref _isLabTechFieldsVisible, value); }

        private bool _isBillingStaffFieldsVisible;
        public bool IsBillingStaffFieldsVisible { get => _isBillingStaffFieldsVisible; set => SetProperty(ref _isBillingStaffFieldsVisible, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddNewStaffViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => DialogHost.Close("MainDialogHost", false));
            
            SelectedRole = Roles[0]; // Default to Doctor
        }

        private void UpdateRoleVisibility()
        {
            IsDoctorFieldsVisible = SelectedRole == "Doctor";
            IsNurseFieldsVisible = SelectedRole == "Nurse";
            IsPharmacistFieldsVisible = SelectedRole == "Pharmacist";
            IsLabTechFieldsVisible = SelectedRole == "Lab Technician";
            IsBillingStaffFieldsVisible = SelectedRole == "Billing Staff";
            
            // Set some defaults
            if (IsDoctorFieldsVisible || IsNurseFieldsVisible) Department = "General Medicine";
            if (IsLabTechFieldsVisible) Department = "Laboratory";
            if (IsBillingStaffFieldsVisible) Department = "Finance/Billing";
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Please fill required fields (Name and Email).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                switch (SelectedRole)
                {
                    case "Doctor":
                        var doc = new Doctor { FullName = FullName, Email = Email, PhoneNumber = Phone, Specialization = Specialization, Department = Department, IsActive = true };
                        DataManager.RegisterDoctor(doc, Password);
                        break;
                    case "Nurse":
                        var nurse = new Nurse { FullName = FullName, Email = Email, Phone = Phone, Specialization = Specialization, Department = Department, IsActive = true };
                        DataManager.RegisterNurse(nurse, Password);
                        break;
                    case "Pharmacist":
                        var pharm = new Pharmacist { FullName = FullName, Email = Email, Phone = Phone, LicenseNumber = LicenseNumber, IsActive = true };
                        DataManager.RegisterPharmacist(pharm, Password);
                        break;
                    case "Lab Technician":
                        var tech = new LabTechnician { FullName = FullName, Email = Email, Phone = Phone, Specialization = Specialization, EmployeeCode = EmployeeCode, Department = Department, IsActive = true };
                        DataManager.RegisterLabTechnician(tech, Password);
                        break;
                    case "Billing Staff":
                        var staff = new BillingStaff { FullName = FullName, Email = Email, Phone = Phone, Position = Position, EmployeeCode = EmployeeCode, Department = Department, IsActive = true };
                        DataManager.RegisterBillingStaff(staff, Password);
                        break;
                }

                MessageBox.Show($"{SelectedRole} registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogHost.Close("MainDialogHost", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
