using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminDashboardViewModel();
        }
    }
}
