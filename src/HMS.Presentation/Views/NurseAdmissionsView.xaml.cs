using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class NurseAdmissionsView : UserControl
    {
        public NurseAdmissionsView()
        {
            InitializeComponent();
            DataContext = new NurseAdmissionsViewModel();
        }
    }
}
