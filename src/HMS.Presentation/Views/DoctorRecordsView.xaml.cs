using System.Windows.Controls;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class DoctorRecordsView : UserControl
    {
        public DoctorRecordsView()
        {
            InitializeComponent();
            DataContext = new DoctorRecordsViewModel();
        }
    }
}
