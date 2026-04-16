using System.Windows;
using System.Windows.Controls;
using HMS.Core.AppLogic.Services;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AdminUsersView : UserControl
    {
        public AdminUsersView()
        {
            InitializeComponent();
            DataContext = new AdminUsersViewModel();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddNewUserWindow();
            addUserWindow.ShowDialog();
            
            // Refresh data after window closes
            if (DataContext is AdminUsersViewModel vm)
            {
                vm.RefreshCommand.Execute(null);
            }
        }
    }
}
