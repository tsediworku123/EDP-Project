using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PharmacistInventoryView : UserControl
    {
        public PharmacistInventoryView()
        {
            InitializeComponent();
            DataContext = new PharmacistInventoryViewModel();
        }
    }
}
