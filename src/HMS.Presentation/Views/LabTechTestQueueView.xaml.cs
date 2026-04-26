using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class LabTechTestQueueView : UserControl
    {
        public LabTechTestQueueView()
        {
            InitializeComponent();
            DataContext = new LabTechTestQueueViewModel();
        }
    }
}
