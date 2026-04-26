using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;

namespace HMS.Core.ViewModels
{
    public class AddInventoryItemViewModel : ObservableObject
    {
        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _sku;
        public string SKU { get => _sku; set => SetProperty(ref _sku, value); }

        private string _category = "Medicine";
        public string Category { get => _category; set => SetProperty(ref _category, value); }

        private decimal _sellingUnitPrice;
        public decimal SellingUnitPrice { get => _sellingUnitPrice; set => SetProperty(ref _sellingUnitPrice, value); }

        private int _stockQuantity;
        public int StockQuantity { get => _stockQuantity; set => SetProperty(ref _stockQuantity, value); }

        private string _unitType = "Pills";
        public string UnitType { get => _unitType; set => SetProperty(ref _unitType, value); }

        private DateTime? _expiryDate = DateTime.Now.AddMonths(12);
        public DateTime? ExpiryDate { get => _expiryDate; set => SetProperty(ref _expiryDate, value); }

        public InventoryItem GetNewItem()
        {
            return new InventoryItem
            {
                Name = this.Name,
                SKU = this.SKU,
                Category = this.Category,
                SellingUnitPrice = this.SellingUnitPrice,
                StockQuantity = this.StockQuantity,
                UnitType = this.UnitType,
                ExpiryDate = this.ExpiryDate,
                LastStockUpdate = DateTime.Now
            };
        }
    }
}
