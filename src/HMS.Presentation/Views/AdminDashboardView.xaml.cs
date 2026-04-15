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
            
            try 
            {
                DataManager.EnsureLoaded();
                
                DataContext = new {
                    TotalDoctors = DataManager.Doctors?.Count ?? 0,
                    TotalPatients = DataManager.Patients?.Count ?? 0,
                    TotalAppointments = DataManager.Appointments?.Count ?? 0,
                    RecentUsers = DataManager.Users?.Take(5).ToList() ?? new List<User>()
                };
            }
            catch (System.Exception ex)
            {
                // Fallback for visual stability if data fails to load
                DataContext = new {
                    TotalDoctors = 0,
                    TotalPatients = 0,
                    TotalAppointments = 0,
                    RecentUsers = new List<User>()
                };
            }
        }
    }
}
