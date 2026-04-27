using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PharmacistInventoryViewModel : ObservableObject
    {
        public ObservableCollection<InventoryItem> Inventory { get; } = new ObservableCollection<InventoryItem>();
        
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LoadInventory();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand BulkImportCommand { get; }

        public PharmacistInventoryViewModel()
        {
            DataManager.EnsureLoaded();
            
            RefreshCommand = new RelayCommand(LoadInventory);
            AddItemCommand = new RelayCommand(AddNewItem);
            BulkImportCommand = new RelayCommand(BulkImport);

            LoadInventory();
        }

        private void LoadInventory()
        {
            try
            {
                Inventory.Clear();

                // Reload fresh from DB every time
                DataManager.ReloadInventory();

                var query = DataManager.Inventory.AsQueryable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var lowerSearch = SearchText.ToLower();
                    // Null-safe search to avoid NullReferenceException
                    query = query.Where(i =>
                        (i.Name != null && i.Name.ToLower().Contains(lowerSearch)) ||
                        (i.SKU != null && i.SKU.ToLower().Contains(lowerSearch)) ||
                        (i.Category != null && i.Category.ToLower().Contains(lowerSearch)));
                }

                foreach (var item in query.OrderBy(i => i.Name ?? ""))
                {
                    Inventory.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading inventory: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void AddNewItem()
        {
            try
            {
                var vm = new AddInventoryItemViewModel();
                var dialog = new HMS.Core.Views.AddInventoryItemDialog { DataContext = vm };
                var result = await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");

                if (result is bool success && success)
                {
                    if (string.IsNullOrWhiteSpace(vm.Name))
                    {
                        System.Windows.MessageBox.Show("Item Name is required.", "Validation Error",
                            System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return;
                    }

                    // Auto-generate SKU if empty
                    if (string.IsNullOrWhiteSpace(vm.SKU))
                        vm.SKU = "SKU-" + DateTime.Now.Ticks.ToString().Substring(10);

                    // Default category if none selected
                    if (string.IsNullOrWhiteSpace(vm.Category))
                        vm.Category = "Medicine";

                    var newItem = vm.GetNewItem();
                    newItem.Id = 0;

                    DataManager.AddInventoryItem(newItem);

                    // Reload from DB to ensure UI shows the persisted item
                    LoadInventory();

                    System.Windows.MessageBox.Show($"'{newItem.Name}' has been added to inventory.", "Success",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to save item: {ex.Message}", "Save Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void BulkImport()
        {
            try
            {
                var dialog = new HMS.Core.Views.BulkImportInventoryDialog();
                await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");
                // After dialog closes, reload inventory
                LoadInventory();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Bulk import error: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
