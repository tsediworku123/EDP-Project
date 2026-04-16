using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AdminReportsView : UserControl
    {
        public AdminReportsView()
        {
            InitializeComponent();
            DataContext = new AdminReportsViewModel();
        }
    }
}
