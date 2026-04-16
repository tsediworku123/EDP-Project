using System.Windows.Controls;
using HMS.Core.AppLogic.Services;

namespace HMS.Core.Views
{
    public partial class AdminPatientsView : UserControl
    {
        public AdminPatientsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminPatientsViewModel();
        }
    }
}
