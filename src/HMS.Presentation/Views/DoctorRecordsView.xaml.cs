using System.Windows.Controls;
using HMS.Core.AppLogic.Services;
using System;
using System.Linq;

namespace HMS.Core.Views
{
    public partial class DoctorRecordsView : UserControl
    {
        public DoctorRecordsView()
        {
            InitializeComponent();
            DataManager.EnsureLoaded();
            DataContext = new { Records = DataManager.MedicalRecords.Take(10).ToList() };
        }
    }
}
