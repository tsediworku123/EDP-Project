using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PatientBillingView : UserControl
    {
        public PatientBillingView()
        {
            InitializeComponent();
            DataContext = new ViewModels.PatientBillingViewModel();
        }
    }
}
