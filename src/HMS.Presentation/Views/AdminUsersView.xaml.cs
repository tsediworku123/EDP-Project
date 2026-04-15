using System.Windows.Controls;
using HMS.Core.AppLogic.Services;

namespace HMS.Core.Views
{
    public partial class AdminUsersView : UserControl
    {
        public AdminUsersView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
            DataContext = new { Users = DataManager.Users };
        }
    }
}
