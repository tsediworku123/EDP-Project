using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class NurseVitalsView : UserControl
    {
        public NurseVitalsView()
        {
            InitializeComponent();
            DataContext = new NurseVitalsViewModel();
        }
    }
}
