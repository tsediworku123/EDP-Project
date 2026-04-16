using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminUsersViewModel : ObservableObject
    {
        private ObservableCollection<User> _filteredUsers;
        private string _searchText;
        private string _selectedRole = "All";
        private int _totalUsers;
        private int _activeStaff;
        private int _deactivatedUsers;

        public ObservableCollection<User> FilteredUsers 
        { 
            get => _filteredUsers; 
            set => SetProperty(ref _filteredUsers, value); 
        }

        public string SearchText 
        { 
            get => _searchText; 
            set 
            { 
                if (SetProperty(ref _searchText, value))
                    ApplyFilters();
            } 
        }

        public string SelectedRole 
        { 
            get => _selectedRole; 
            set 
            { 
                if (SetProperty(ref _selectedRole, value))
                    ApplyFilters();
            } 
        }

        public List<string> RoleOptions { get; } = new List<string> { "All", User.Admin, User.Doctor, User.Receptionist, User.Patient };

        public int TotalUsers { get => _totalUsers; set => SetProperty(ref _totalUsers, value); }
        public int ActiveStaff { get => _activeStaff; set => SetProperty(ref _activeStaff, value); }
        public int DeactivatedUsers { get => _deactivatedUsers; set => SetProperty(ref _deactivatedUsers, value); }

        public ICommand RefreshCommand { get; }
        public ICommand ToggleStatusCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand AddUserCommand { get; }

        public AdminUsersViewModel()
        {
            RefreshCommand = new RelayCommand(LoadData);
            ToggleStatusCommand = new RelayCommand<User>(ToggleUserStatus);
            ResetPasswordCommand = new RelayCommand<User>(ResetUserPassword);
            AddUserCommand = new RelayCommand(AddNewUser);
            
            LoadData();
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            UpdateStats();
            ApplyFilters();
        }

        private void UpdateStats()
        {
            var users = DataManager.Users;
            TotalUsers = users.Count;
            ActiveStaff = users.Count(u => u.IsActive && u.Role != User.Patient);
            DeactivatedUsers = users.Count(u => !u.IsActive);
        }

        private void ApplyFilters()
        {
            var query = DataManager.Users.AsEnumerable();

            if (SelectedRole != "All")
                query = query.Where(u => u.Role == SelectedRole);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string search = SearchText.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(search) || (u.Email != null && u.Email.ToLower().Contains(search)));
            }

            FilteredUsers = new ObservableCollection<User>(query.OrderBy(u => u.Username));
        }

        private void ToggleUserStatus(User user)
        {
            if (user == null) return;
            
            string action = user.IsActive ? "deactivate" : "activate";
            var result = MessageBox.Show($"Are you sure you want to {action} user '{user.Username}'?", 
                                       "Confirm Action", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                user.IsActive = !user.IsActive;
                DataManager.SaveUsers();
                LoadData();
            }
        }

        private void ResetUserPassword(User user)
        {
            if (user == null) return;

            var result = MessageBox.Show($"Are you sure you want to reset the password for '{user.Username}' to 'Welcome123'?", 
                                       "Confirm Password Reset", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                user.Password = PasswordHasher.HashPassword("Welcome123");
                DataManager.SaveUsers();
                MessageBox.Show($"Password for '{user.Username}' has been reset to: Welcome123", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddNewUser()
        {
            // Trigger logic for opening AddNewUserWindow
            // In a real MVVM app, we'd use a service, but for now we'll assume the view handles triggering this 
            // or we can instantiate the window here if needed.
            // But since this is a UI improvement task, we'll focus on the VM logic.
        }
    }
}
