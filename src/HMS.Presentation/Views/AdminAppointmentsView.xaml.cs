using System.Windows.Controls;
using HMS.Core.AppLogic.Services;

namespace HMS.Core.Views
{
    public partial class AdminAppointmentsView : UserControl
    {
        public AdminAppointmentsView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
            DataContext = new { Appointments = DataManager.Appointments };
        }
    }
}
