using HMS.Core.ViewModels.Auth;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.Views.Auth
{
    public partial class LoginView : Window
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
        }

        // Passes the PasswordBox value to the ViewModel (security — can't bind passwords in XAML)
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.ExecuteLoginWithPassword(PasswordBox.Password);
        }

        // Allow pressing Enter in the PasswordBox to trigger login
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ViewModel?.ExecuteLoginWithPassword(PasswordBox.Password);
        }

        // Custom drag-to-move (no window chrome)
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
