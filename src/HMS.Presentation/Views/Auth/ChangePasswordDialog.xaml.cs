using System.Windows;
using System.Windows.Controls;
using HMS.Core.ViewModels.Auth;

namespace HMS.Core.Views.Auth
{
    public partial class ChangePasswordDialog : UserControl
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ChangePasswordViewModel)DataContext;
            vm.OldPassword = OldPasswordBox.Password;
            vm.NewPassword = NewPasswordBox.Password;
            vm.ConfirmPassword = ConfirmPasswordBox.Password;
            vm.ChangePasswordCommand.Execute(null);
        }
    }
}
