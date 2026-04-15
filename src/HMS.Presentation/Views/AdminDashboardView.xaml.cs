using System.Linq;
using System.Windows.Controls;
using HMS.Core.AppLogic.Services;
using System.Collections.Generic;
using HMS.Core.Domain.Entities;

namespace HMS.Core.Views
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminDashboardViewModel();
        }
    }
}
