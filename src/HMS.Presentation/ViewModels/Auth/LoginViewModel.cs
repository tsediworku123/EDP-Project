using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Enums;
using HMS.Core.Domain.Entities;
using HMS.Core.Infrastructure.Repositories.Json;
using HMS.Core.Common.Utils;
using HMS.Core.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace HMS.Core.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService    _authService;
        private readonly JsonDataService _dataService;

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
            _dataService = new JsonDataService();
            _authService = new AuthService(_dataService);
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

            var doctor  = user.Role == UserRole.Doctor.ToString()  ? _dataService.LoadDoctors().Find(d => d.Id == user.DoctorId) : null;
            var patient = user.Role == UserRole.Patient.ToString() ? _dataService.LoadPatients().Find(p => p.Id == user.PatientId) : null;
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
                    ShowError("Unknown role. Contact the administrator.");
                    return;
            }

            shell.Show();
            foreach (Window w in System.Windows.Application.Current.Windows)
            {
                if (w is Views.Auth.LoginView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
