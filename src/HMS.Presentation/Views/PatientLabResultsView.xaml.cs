using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class PatientLabResultsView : UserControl
    {
        public PatientLabResultsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.PatientLabResultsViewModel();
        }
    }
}
