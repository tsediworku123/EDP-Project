using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class LabTechDashboardView : UserControl
    {
        public LabTechDashboardView()
        {
            InitializeComponent();
            DataContext = new LabTechDashboardViewModel();
        }
    }
}
