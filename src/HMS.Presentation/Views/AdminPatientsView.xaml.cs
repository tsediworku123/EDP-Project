using System.Windows.Controls;
using HMS.Core.AppLogic.Services;

namespace HMS.Core.Views
{
    public partial class AdminPatientsView : UserControl
    {
        public AdminPatientsView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
            DataContext = new { Patients = DataManager.Patients };
        }
    }
}
