using System.Windows;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AddNewUserWindow : Window
    {
        public AddNewUserWindow()
        {
            InitializeComponent();
            DataContext = new AddNewUserViewModel(this);
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
