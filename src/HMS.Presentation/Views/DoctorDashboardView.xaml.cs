using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class DoctorDashboardView : UserControl
    {
        public DoctorDashboardView()
        {
            InitializeComponent();
            DataContext = new ViewModels.DoctorDashboardViewModel();
        }
    }
}
