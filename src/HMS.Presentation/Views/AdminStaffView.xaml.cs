using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AdminStaffView : UserControl
    {
        public AdminStaffView()
        {
            InitializeComponent();
            DataContext = new AdminStaffViewModel();
        }
    }
}
