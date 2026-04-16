using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace HMS.Core.ViewModels
{
    public class DoctorProfileViewModel : ObservableObject
    {
        private Doctor _doctor;
        private User _user;

        // ---------- Editable Profile Fields ----------
        private string _fullName;
        private string _phoneNumber;
        private string _specialization;
        private string _department;
        private string _email;
        private string _profilePhotoPath;
        private BitmapImage _profilePhoto;

        // ---------- Password Change Fields ----------
        private string _currentPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _passwordStatusMessage;
        private bool _passwordStatusIsError;

        // ---------- General Status ----------
        private string _statusMessage;
        private bool _statusIsError;
        private bool _hasUnsavedChanges;

        // ==================== Properties ====================

        public string FullName
        {
            get => _fullName;
            set { SetProperty(ref _fullName, value); HasUnsavedChanges = true; }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { SetProperty(ref _phoneNumber, value); HasUnsavedChanges = true; }
        }

        public string Specialization
        {
            get => _specialization;
            set { SetProperty(ref _specialization, value); HasUnsavedChanges = true; }
        }

        public string Department
        {
            get => _department;
            set { SetProperty(ref _department, value); HasUnsavedChanges = true; }
        }

        public string Email
        {
            get => _email;
            set { SetProperty(ref _email, value); HasUnsavedChanges = true; }
        }

        public string ProfilePhotoPath
        {
            get => _profilePhotoPath;
            set { SetProperty(ref _profilePhotoPath, value); LoadPhoto(value); }
        }

        public BitmapImage ProfilePhoto
        {
            get => _profilePhoto;
            set
            {
                SetProperty(ref _profilePhoto, value);
                OnPropertyChanged(nameof(HasPhoto));
            }
        }

        public bool HasPhoto => _profilePhoto != null;

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string PasswordStatusMessage
        {
            get => _passwordStatusMessage;
            set
            {
                SetProperty(ref _passwordStatusMessage, value);
                OnPropertyChanged(nameof(HasPasswordStatusMessage));
            }
        }

        public bool HasPasswordStatusMessage => !string.IsNullOrEmpty(_passwordStatusMessage);

        public bool PasswordStatusIsError
        {
            get => _passwordStatusIsError;
            set => SetProperty(ref _passwordStatusIsError, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                SetProperty(ref _statusMessage, value);
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }

        public bool HasStatusMessage => !string.IsNullOrEmpty(_statusMessage);

        public bool StatusIsError
        {
            get => _statusIsError;
            set => SetProperty(ref _statusIsError, value);
        }

        public bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set => SetProperty(ref _hasUnsavedChanges, value);
        }

        // Derived display
        public string DoctorInitials => GetInitials(FullName);
        public string DoctorRoleLabel => _doctor != null ? $"{_doctor.Specialization} · {_doctor.Department}" : "Medical Professional";

        // ==================== Commands ====================
        public ICommand SaveProfileCommand { get; }
        public ICommand ChangePhotoCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        // ==================== Constructor ====================
        public DoctorProfileViewModel()
        {
            DataManager.EnsureLoaded();
            LoadDoctorData();

            SaveProfileCommand    = new RelayCommand(SaveProfile);
            ChangePhotoCommand    = new RelayCommand(ChangePhoto);
            ChangePasswordCommand = new RelayCommand(ChangePassword);
        }

        // ==================== Data Loading ====================
        private void LoadDoctorData()
        {
            _doctor = CurrentSession.Instance.LoggedInDoctor;
            _user   = CurrentSession.Instance.LoggedInUser;

            if (_doctor == null) return;

            _fullName      = _doctor.FullName;
            _phoneNumber   = _doctor.PhoneNumber;
            _specialization = _doctor.Specialization;
            _department    = _doctor.Department;
            _email         = _doctor.Email;
            _profilePhotoPath = _doctor.PhotoPath;

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Specialization));
            OnPropertyChanged(nameof(Department));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(DoctorInitials));
            OnPropertyChanged(nameof(DoctorRoleLabel));

            LoadPhoto(_profilePhotoPath);
            HasUnsavedChanges = false;
        }

        private void LoadPhoto(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource    = new Uri(path, UriKind.Absolute);
                    bmp.CacheOption  = BitmapCacheOption.OnLoad;
                    bmp.DecodePixelWidth = 200;
                    bmp.EndInit();
                    bmp.Freeze();
                    ProfilePhoto = bmp;
                }
                else
                {
                    ProfilePhoto = null;
                }
            }
            catch { ProfilePhoto = null; }
        }

        // ==================== Command Handlers ====================

        private void ChangePhoto()
        {
            var dlg = new OpenFileDialog
            {
                Title  = "Select Profile Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp"
            };

            if (dlg.ShowDialog() == true)
            {
                ProfilePhotoPath  = dlg.FileName;
                HasUnsavedChanges = true;
                ShowStatus("Photo selected. Click 'Save Changes' to apply.", false);
            }
        }

        private void SaveProfile()
        {
            if (_doctor == null)
            {
                ShowStatus("No doctor session found.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(FullName))
            {
                ShowStatus("Full name cannot be empty.", true);
                return;
            }

            _doctor.FullName     = FullName.Trim();
            _doctor.PhoneNumber  = PhoneNumber?.Trim();
            _doctor.Specialization = Specialization?.Trim();
            _doctor.Department   = Department?.Trim();
            _doctor.Email        = Email?.Trim();
            _doctor.PhotoPath    = ProfilePhotoPath;

            // Persist to JSON
            DataManager.SaveDoctors();

            // Update User email if linked
            if (_user != null && !string.IsNullOrWhiteSpace(Email))
            {
                _user.Email = Email.Trim();
                DataManager.SaveUsers();
            }

            HasUnsavedChanges = false;
            OnPropertyChanged(nameof(DoctorInitials));
            OnPropertyChanged(nameof(DoctorRoleLabel));

            ShowStatus("Profile updated successfully!", false);
        }

        private void ChangePassword()
        {
            if (_user == null)
            {
                ShowPasswordStatus("No user account found.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ShowPasswordStatus("Please enter your current password.", true);
                return;
            }

            if (!PasswordHasher.VerifyPassword(CurrentPassword, _user.Password))
            {
                ShowPasswordStatus("Current password is incorrect.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 6)
            {
                ShowPasswordStatus("New password must be at least 6 characters.", true);
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ShowPasswordStatus("Passwords do not match. Please re-enter.", true);
                return;
            }

            _user.Password = PasswordHasher.HashPassword(NewPassword);
            DataManager.SaveUsers();

            // Clear fields
            CurrentPassword = string.Empty;
            NewPassword     = string.Empty;
            ConfirmPassword = string.Empty;

            ShowPasswordStatus("Password changed successfully!", false);
        }

        // ==================== Helpers ====================

        private void ShowStatus(string message, bool isError)
        {
            StatusMessage = message;
            StatusIsError = isError;
        }

        private void ShowPasswordStatus(string message, bool isError)
        {
            PasswordStatusMessage  = message;
            PasswordStatusIsError  = isError;
        }

        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "DR";
            var cleaned = name.ToLowerInvariant().StartsWith("dr.") ? name.Substring(3) : name;
            var parts = cleaned.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return string.Concat(parts[0][0], parts[parts.Length - 1][0]).ToUpper();
        }
    }
}
