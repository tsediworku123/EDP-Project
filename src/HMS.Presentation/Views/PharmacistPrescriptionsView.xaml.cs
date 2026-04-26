using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PharmacistPrescriptionsView : UserControl
    {
        public PharmacistPrescriptionsView()
        {
            InitializeComponent();
            DataContext = new PharmacistPrescriptionsViewModel();
        }
    }
}
