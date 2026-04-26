using System.Windows.Controls;
using HMS.Core.ViewModels;
using HMS.Core.Domain.Entities;

namespace HMS.Core.Views
{
    public partial class LabTechResultsView : UserControl
    {
        public LabTechResultsView(LabTest test)
        {
            InitializeComponent();
            DataContext = new LabTechResultsViewModel(test);
        }
    }
}
