using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class DoctorAppointmentsView : UserControl
    {
        public DoctorAppointmentsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.DoctorAppointmentsViewModel();
        }
    }
}
