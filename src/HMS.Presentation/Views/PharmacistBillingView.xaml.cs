using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PharmacistBillingView : UserControl
    {
        public PharmacistBillingView()
        {
            InitializeComponent();
            DataContext = new PharmacistBillingViewModel();
        }
    }
}
