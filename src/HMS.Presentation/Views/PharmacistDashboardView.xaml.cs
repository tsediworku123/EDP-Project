using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PharmacistDashboardView : UserControl
    {
        public PharmacistDashboardView()
        {
            InitializeComponent();
            DataContext = new PharmacistDashboardViewModel();
        }
    }
}
