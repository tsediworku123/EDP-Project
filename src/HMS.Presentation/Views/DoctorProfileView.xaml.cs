using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class DoctorProfileView : UserControl
    {
        public DoctorProfileView()
        {
            InitializeComponent();
        }

        private DoctorProfileViewModel VM => DataContext as DoctorProfileViewModel;

        private void CurrentPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.CurrentPassword = ((PasswordBox)sender).Password;
        }

        private void NewPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.NewPassword = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
