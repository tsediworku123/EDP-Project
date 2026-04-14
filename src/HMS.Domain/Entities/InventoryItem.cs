using System;

namespace HMS.Core.Domain.Entities
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // Medicine, Medical Supply, Equipment, Consumable, Stationery
        public string SKU { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public int StockQuantity { get; set; } = 0;
        public int ReorderLevel { get; set; } = 10;
        public decimal PurchaseUnitPrice { get; set; } = 0;
        public decimal SellingUnitPrice { get; set; } = 0;
        public string StorageLocation { get; set; } // Self A1, Shelf B2, etc.
        public DateTime? ExpiryDate { get; set; }
        public DateTime LastStockUpdate { get; set; } = DateTime.Now;
        public string SupplierName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
