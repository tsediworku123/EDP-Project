using System.Windows;
using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AddStaffDialog : UserControl
    {
        public AddStaffDialog()
        {
            InitializeComponent();
            DataContext = new AddNewStaffViewModel();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (AddNewStaffViewModel)DataContext;
            vm.Password = PassBox.Password;
            vm.SaveCommand.Execute(null);
        }
    }
}
