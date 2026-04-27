using System.Windows;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AddNewUserWindow : Window
    {
        public AddNewUserWindow()
        {
            InitializeComponent();
            var vm = new AddNewUserViewModel(this);
            vm.PropertyChanged += Vm_PropertyChanged;
            DataContext = vm;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Password")
            {
                var vm = (AddNewUserViewModel)sender;
                PassBox.Password = vm.Password;
                ConfirmPassBox.Password = vm.ConfirmPassword;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (AddNewUserViewModel)DataContext;
            vm.Password = PassBox.Password;
            vm.ConfirmPassword = ConfirmPassBox.Password;
            vm.SaveCommand.Execute(null);
        }
    }
}
