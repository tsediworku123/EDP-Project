using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PatientPrescriptionsView : UserControl
    {
        public PatientPrescriptionsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.PatientPrescriptionsViewModel();
        }
    }
}
