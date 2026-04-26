using System.Windows.Controls;
using HMS.Core.Domain.Entities;

namespace HMS.Core.Views
{
    public partial class LabTestDetailDialog : UserControl
    {
        public LabTestDetailDialog(LabTest test)
        {
            InitializeComponent();
            DataContext = new { Test = test };
        }
    }
}
