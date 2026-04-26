using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class NurseDashboardView : UserControl
    {
        public NurseDashboardView()
        {
            InitializeComponent();
            DataContext = new NurseDashboardViewModel();
        }
    }
}
