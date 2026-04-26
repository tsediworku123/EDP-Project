using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.Views.Auth
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || 
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Please fill out all required fields (Name, Email, Password).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newPatient = new Patient
                {
                    FullName = NameBox.Text.Trim(),
                    Email = EmailBox.Text.Trim(),
                    Phone = PhoneBox.Text.Trim(),
                    Password = PasswordBox.Password,
                    DateOfBirth = DateTime.Now.AddYears(-20), // Default
                    Gender = "Male", // Default
                    Address = "Not Provided",
                    IsActive = true
                };

                DataManager.RegisterPatient(newPatient);

                MessageBox.Show("Account successfully created! You can now sign in with your email and password.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                var loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not register account. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
