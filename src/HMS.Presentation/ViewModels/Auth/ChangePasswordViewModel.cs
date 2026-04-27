using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using MaterialDesignThemes.Wpf;
using HMS.Core.Persistence;
using HMS.Core.Persistence.Repositories;
using HMS.Core.Persistence.Services;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels.Auth
{
    public class ChangePasswordViewModel : ObservableObject
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ICommand ChangePasswordCommand { get; }
        public ICommand CancelCommand { get; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new RelayCommand(ChangePassword);
            CancelCommand = new RelayCommand(() => DialogHost.Close("MainDialogHost"));
        }

        private void ChangePassword()
        {
            string cleanOld = OldPassword?.Trim();
            string cleanNew = NewPassword?.Trim();
            string cleanConfirm = ConfirmPassword?.Trim();

            if (string.IsNullOrWhiteSpace(cleanOld) || string.IsNullOrWhiteSpace(cleanNew))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cleanNew != cleanConfirm)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var sessionUser = CurrentSession.Instance.LoggedInUser;
            if (sessionUser == null)
            {
                MessageBox.Show("No active session found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var uow = new UnitOfWork(DatabaseFactory.CreateContext()))
            {
                var authService = new AuthService(uow);
                bool success = authService.ChangePassword(sessionUser.Id, cleanOld, cleanNew);

                if (success)
                {
                    // Update session user (hashed)
                    string newHashedPassword = PasswordHasher.HashPassword(cleanNew);
                    sessionUser.Password = newHashedPassword;

                    // Refresh DataManager's user list from DB to get the newly saved password
                    DataManager.ReloadUsers();
                    
                    // Also update the session object specifically
                    sessionUser.Password = newHashedPassword;
                    
                    // And update DataManager.CurrentUser if it exists
                    if (DataManager.CurrentUser != null && DataManager.CurrentUser.Id == sessionUser.Id)
                    {
                        DataManager.CurrentUser.Password = newHashedPassword;
                    }

                    // Force a save through DataManager as well to ensure total synchronization
                    DataManager.SaveUsers();

                    MessageBox.Show("Password changed successfully! Please use your new password next time you log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogHost.Close("MainDialogHost");
                }
                else
                {
                    MessageBox.Show("Could not change password. Please verify your current password is correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
