using HMS.Core.Common.Utils;
using HMS.Core.Infrastructure.Repositories.Json;
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
        private readonly JsonDataService _dataService;
        private readonly Window _window;

        private string _username;
        public string Username { get => _username; set => SetProperty(ref _username, value); }

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
        public Doctor SelectedDoctor { get => _selectedDoctor; set => SetProperty(ref _selectedDoctor, value); }

        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string> { "Admin", "Receptionist", "Doctor" };
        public ObservableCollection<Doctor> Doctors { get; } = new ObservableCollection<Doctor>();

        private bool _isDoctorRoleSelected;
        public bool IsDoctorRoleSelected { get => _isDoctorRoleSelected; set => SetProperty(ref _isDoctorRoleSelected, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddNewUserViewModel(Window window)
        {
            _window = window;
            _dataService = new JsonDataService();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            LoadDoctors();
        }

        private void OnSelectedRoleChanged(string value)
        {
            IsDoctorRoleSelected = value == "Doctor";
        }

        private void LoadDoctors()
        {
            var docs = _dataService.LoadDoctors();
            foreach (var doc in docs) Doctors.Add(doc);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(SelectedRole))
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

            var users = _dataService.LoadUsers();
            if (users.Any(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Username already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int nextUserId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            var newUser = new User
            {
                Id = nextUserId,
                Username = Username,
                Password = PasswordHasher.HashPassword(Password),
                Role = SelectedRole,
                DoctorId = SelectedRole == "Doctor" ? SelectedDoctor.Id : (int?)null,
                Email = Email,
                IsActive = true
            };

            users.Add(newUser);
            _dataService.SaveUsers(users);

            MessageBox.Show($"User '{Username}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
