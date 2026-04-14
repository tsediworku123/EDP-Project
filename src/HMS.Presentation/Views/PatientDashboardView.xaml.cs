using System.Windows.Controls;

namespace HMS.Core.Views
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
