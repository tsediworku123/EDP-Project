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

        public PharmacistInventoryViewModel()
        {
            DataManager.EnsureLoaded();
            
            RefreshCommand = new RelayCommand(LoadInventory);
            AddItemCommand = new RelayCommand(AddNewItem);

            LoadInventory();
        }

        private void LoadInventory()
        {
            Inventory.Clear();
            
            var query = DataManager.Inventory.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerSearch = SearchText.ToLower();
                query = query.Where(i => i.Name.ToLower().Contains(lowerSearch) || 
                                         i.SKU.ToLower().Contains(lowerSearch) || 
                                         i.Category.ToLower().Contains(lowerSearch));
            }

            foreach (var item in query.OrderBy(i => i.Name))
            {
                Inventory.Add(item);
            }
        }

        private void AddNewItem()
        {
            // Placeholder for opening an "Add Item" dialog
            System.Windows.MessageBox.Show("Open Add Inventory Item Dialog", "Add Item", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
