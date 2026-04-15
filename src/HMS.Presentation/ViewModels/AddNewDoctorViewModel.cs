using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using HMS.Core.AppLogic.Services;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.ViewModels
{
    public class AddNewDoctorViewModel : ObservableObject
    {
        private readonly Doctor _existingDoctor;
        private readonly bool _isEditing;

        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _specialty;
        public string Specialty { get => _specialty; set => SetProperty(ref _specialty, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _department;
        public string Department { get => _department; set => SetProperty(ref _department, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _assignedShift = "Morning";
        public string AssignedShift { get => _assignedShift; set => SetProperty(ref _assignedShift, value); }

        public string DialogTitle => _isEditing ? $"EDIT: {FullName}" : "ADD NEW MEDICAL STAFF";
        public string ActionButtonText => _isEditing ? "SAVE CHANGES" : "REGISTER DOCTOR";

        public List<string> Specialties { get; } = new List<string> { "Cardiology", "Neurology", "Pediatrics", "Orthopedics", "Dermatology", "General Medicine", "Oncology", "Psychiatry" };
        public List<string> Departments => DataManager.Departments;

        public ICommand SaveCommand { get; }

        public AddNewDoctorViewModel(Doctor doctor = null)
        {
            if (doctor != null)
            {
                _existingDoctor = doctor;
                _isEditing = true;
                
                FullName = doctor.FullName;
                Specialty = doctor.Specialization;
                Phone = doctor.PhoneNumber;
                Email = doctor.Email;
                Department = doctor.Department;
                AssignedShift = doctor.AssignedShift;
                Address = doctor.Address;
            }

            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Specialty))
            {
                MessageBox.Show("Name and Specialization are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEditing && _existingDoctor != null)
            {
                // Update Existing
                _existingDoctor.FullName = FullName;
                _existingDoctor.Specialization = Specialty;
                _existingDoctor.PhoneNumber = Phone;
                _existingDoctor.Email = Email;
                _existingDoctor.Department = Department;
                _existingDoctor.AssignedShift = AssignedShift;
                _existingDoctor.Address = Address;
            }
            else
            {
                // Create New
                var newDoctor = new Doctor
                {
                    Id = DataManager.Doctors.Any() ? DataManager.Doctors.Max(d => d.Id) + 1 : 1,
                    FullName = FullName,
                    Specialization = Specialty,
                    PhoneNumber = Phone,
                    Email = Email,
                    Department = Department,
                    AssignedShift = AssignedShift,
                    Address = Address,
                    IsActive = true
                };
                DataManager.Doctors.Add(newDoctor);
            }

            DataManager.SaveAllData();
            DialogHost.CloseDialogCommand.Execute(true, null);
            
            MessageBox.Show($"Staff records for {FullName} have been updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
