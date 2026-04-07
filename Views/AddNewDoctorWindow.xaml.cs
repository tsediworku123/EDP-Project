using System.Windows;
using ClinicAppointmentSystem.ViewModels;

namespace ClinicAppointmentSystem.Views
{
    public partial class AddNewDoctorWindow : Window
    {
        public AddNewDoctorWindow()
        {
            InitializeComponent();
            DataContext = new AddNewDoctorViewModel(this);
        }

        // Bridge to get Password from PasswordBox securely
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (AddNewDoctorViewModel)DataContext;
            vm.Password = PassBox.Password;
            vm.ConfirmPassword = ConfirmPassBox.Password;
            vm.SaveCommand.Execute(null);
        }
    }
}
