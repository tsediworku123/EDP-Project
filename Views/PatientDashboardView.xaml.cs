using System.Windows.Controls;

namespace ClinicAppointmentSystem.Views
{
    public partial class PatientDashboardView : UserControl
    {
        public PatientDashboardView()
        {
            InitializeComponent();
            var vm = new ViewModels.PatientDashboardViewModel();
            this.DataContext = vm;
            vm.Initialize();
        }
    }
}
