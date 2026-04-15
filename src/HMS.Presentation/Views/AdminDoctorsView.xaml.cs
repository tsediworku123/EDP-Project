using System.Windows.Controls;
using HMS.Core.AppLogic.Services;

namespace HMS.Core.Views
{
    public partial class AdminDoctorsView : UserControl
    {
        public AdminDoctorsView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
            DataContext = new { Doctors = DataManager.Doctors };
        }
    }
}
