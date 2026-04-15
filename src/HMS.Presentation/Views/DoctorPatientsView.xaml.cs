using System.Windows.Controls;
using HMS.Core.AppLogic.Services;
using System.Linq;

namespace HMS.Core.Views
{
    public partial class DoctorPatientsView : UserControl
    {
        public DoctorPatientsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.DoctorPatientsViewModel();
        }
    }
}
