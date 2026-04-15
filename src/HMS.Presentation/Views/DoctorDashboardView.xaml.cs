using System.Windows.Controls;
using System.Windows;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class DoctorDashboardView : UserControl
    {
        public DoctorDashboardView()
        {
            InitializeComponent();
            var vm = new ViewModels.DoctorDashboardViewModel();
            
            // Connect navigation request to shell ViewModel if loaded in shell
            vm.RequestViewChange = (view, title) => 
            {
                var window = Window.GetWindow(this);
                if (window?.DataContext is DoctorShellViewModel shellVm)
                {
                    shellVm.ActivePageTitle = title;
                    shellVm.CurrentView = view;
                }
            };
            
            DataContext = vm;
        }
    }
}
