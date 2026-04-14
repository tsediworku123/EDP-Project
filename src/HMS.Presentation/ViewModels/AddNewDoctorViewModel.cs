using HMS.Core.Common.Utils;
using HMS.Core.Infrastructure.Repositories.Json;
using HMS.Core.Domain.Entities;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;

namespace HMS.Core.ViewModels
{
    public partial class AddNewDoctorViewModel : ObservableObject
    {
        private readonly JsonDataService _dataService;
        private readonly Window _window;

        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _specialty;
        public string Specialty { get => _specialty; set => SetProperty(ref _specialty, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private TimeSpan _workingHoursStart = new TimeSpan(9, 0, 0);
        public TimeSpan WorkingHoursStart { get => _workingHoursStart; set => SetProperty(ref _workingHoursStart, value); }

        private TimeSpan _workingHoursEnd = new TimeSpan(17, 0, 0);
        public TimeSpan WorkingHoursEnd { get => _workingHoursEnd; set => SetProperty(ref _workingHoursEnd, value); }

        private string _workingDays = "Mon, Tue, Wed, Thu, Fri";
        public string WorkingDays { get => _workingDays; set => SetProperty(ref _workingDays, value); }

        private int _defaultSlotDuration = 30;
        public int DefaultSlotDuration { get => _defaultSlotDuration; set => SetProperty(ref _defaultSlotDuration, value); }

        private int _bufferTime = 5;
        public int BufferTime { get => _bufferTime; set => SetProperty(ref _bufferTime, value); }

        private string _calendarColor = "#2196F3";
        public string CalendarColor { get => _calendarColor; set => SetProperty(ref _calendarColor, value); }

        private string _username;
        public string Username { get => _username; set => SetProperty(ref _username, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddNewDoctorViewModel(Window window)
        {
            _window = window;
            _dataService = new JsonDataService();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Specialty) || 
                string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please fill all required fields (*).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var users = _dataService.LoadUsers();
            if (users.Any(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Username already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var doctors = _dataService.LoadDoctors();
            int nextDoctorId = doctors.Any() ? doctors.Max(d => d.Id) + 1 : 1;
            int nextUserId = users.Any() ? users.Max(u => u.Id) + 1 : 1;

            // 1. Create Doctor Record
            var newDoctor = new Doctor
            {
                Id = nextDoctorId,
                FullName = FullName,
                Specialty = Specialty,
                Phone = Phone,
                WorkingHoursStart = WorkingHoursStart,
                WorkingHoursEnd = WorkingHoursEnd,
                WorkingDays = WorkingDays,
                SlotDurationMinutes = DefaultSlotDuration,
                BufferMinutes = BufferTime,
                CalendarColor = CalendarColor,
                Email = Email,
                IsActive = true
            };

            // 2. Create User Record
            var newUser = new User
            {
                Id = nextUserId,
                Username = Username,
                Password = PasswordHasher.HashPassword(Password), 
                Role = "Doctor",
                DoctorId = nextDoctorId,
                Email = Email,
                IsActive = true
            };

            doctors.Add(newDoctor);
            users.Add(newUser);

            _dataService.SaveDoctors(doctors);
            _dataService.SaveUsers(users);

            MessageBox.Show($"Doctor '{FullName}' and account '{Username}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
