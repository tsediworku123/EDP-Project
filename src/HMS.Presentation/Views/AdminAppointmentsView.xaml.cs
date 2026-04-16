using System.Windows.Controls;
using HMS.Core.AppLogic.Services;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AdminAppointmentsView : UserControl
    {
        public AdminAppointmentsView()
        {
            InitializeComponent();
            DataContext = new AdminAppointmentsViewModel();
        }
    }
}
