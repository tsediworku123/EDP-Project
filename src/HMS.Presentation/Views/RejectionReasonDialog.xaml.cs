using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace HMS.Core.Views
{
    public partial class RejectionReasonDialog : UserControl
    {
        public RejectionReasonDialog()
        {
            InitializeComponent();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                // Optionally show error, but for now we just close if it's empty and let the VM handle it or prevent it
                return;
            }

            DialogHost.CloseDialogCommand.Execute(ReasonTextBox.Text, this);
        }
    }
}
