using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class LabTechHistoryView : UserControl
    {
        public LabTechHistoryView()
        {
            InitializeComponent();
            DataContext = new LabTechHistoryViewModel();
        }
    }
}
