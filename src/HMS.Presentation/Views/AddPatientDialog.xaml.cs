using System.Windows;
using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AddPatientDialog : UserControl
    {
        public AddPatientDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Transfers password values from PasswordBox controls to the ViewModel
        /// before the Save command executes. PasswordBox cannot be bound directly
        /// due to WPF security restrictions.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddPatientViewModel vm && vm.ShowCredentialSection)
            {
                vm.Password = PasswordBox.Password;
                vm.ConfirmPassword = ConfirmPasswordBox.Password;
            }
        }
    }
}
