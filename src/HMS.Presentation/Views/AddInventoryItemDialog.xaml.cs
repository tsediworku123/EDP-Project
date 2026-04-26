using HMS.Core.ViewModels;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public partial class AddInventoryItemDialog : UserControl
    {
        public AddInventoryItemDialog()
        {
            InitializeComponent();

            // Pre-select defaults when loaded
            Loaded += (s, e) =>
            {
                CategoryCombo.SelectedIndex = 0;
                UnitTypeCombo.SelectedIndex = 0;
            };
        }

        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AddInventoryItemViewModel vm && CategoryCombo.SelectedItem is ComboBoxItem item)
                vm.Category = item.Content?.ToString();
        }

        private void UnitTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AddInventoryItemViewModel vm && UnitTypeCombo.SelectedItem is ComboBoxItem item)
                vm.UnitType = item.Content?.ToString();
        }
    }
}
