using HMS.Core.Persistence;
using HMS.Core.Persistence.Repositories;
using HMS.Core.Persistence.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Domain.Enums;
using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace HMS.Core.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        private string _username;
        public string Username { get => _username; set => SetProperty(ref _username, value); }

        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        private bool _hasError;
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            var unitOfWork = new UnitOfWork(DatabaseFactory.CreateContext());
            _authService = new AuthService(unitOfWork);
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        public void ExecuteLoginWithPassword(string password)
        {
            HasError     = false;
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Username))
            {
                ShowError("Please enter your username.");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter your password.");
                return;
            }

            IsLoading = true;
            var user  = _authService.Login(Username, password);
            IsLoading = false;

            if (user == null)
            {
                ShowError("Invalid username or password. Please try again.");
                return;
            }

            DataManager.EnsureLoaded();
            var doctor  = user.Role == UserRole.Doctor.ToString()  ? DataManager.Doctors.FirstOrDefault(d => d.Id == user.DoctorId) : null;
            var patient = user.Role == UserRole.Patient.ToString() ? DataManager.Patients.FirstOrDefault(p => p.Id == user.PatientId) : null;
            CurrentSession.Instance.StartSession(user, doctor, patient);

            OpenShellForRole(user.Role);
        }

        private void ExecuteLogin() { }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError     = true;
        }

        private void OpenShellForRole(string role)
        {
            try 
            {
                Window shell = null;
                switch (role)
                {
                    case "Admin":
                    case "Receptionist":
                        shell = new Views.AdminShellView(); 
                        break;
                    case "Doctor":
                        shell = new Views.DoctorShellView();
                        break;
                    case "Patient":
                        shell = new Views.PatientShellView();
                        break;
                    default:
                        ShowError("Unknown role.");
                        return;
                }

                shell.Show();
                shell.Activate();
                shell.Focus();
                System.Windows.Application.Current.MainWindow = shell;

                foreach (Window w in System.Windows.Application.Current.Windows)
                {
                    if (w is Views.Auth.LoginView)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"LOAD ERROR: Could not open {role} portal.\n\nDetails: {ex.Message}", "Portal Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
