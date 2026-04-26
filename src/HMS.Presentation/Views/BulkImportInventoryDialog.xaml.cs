using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HMS.Core.Views
{
    public class BulkImportRow
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; } = "Medicine";
        public string UnitType { get; set; } = "Tablets";
        public int StockQuantity { get; set; }
        public decimal SellingUnitPrice { get; set; }
        public string ExpiryDateStr { get; set; } = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
    }

    public partial class BulkImportInventoryDialog : UserControl
    {
        private ObservableCollection<BulkImportRow> _rows = new ObservableCollection<BulkImportRow>();
        private string _csvFilePath;

        public BulkImportInventoryDialog()
        {
            InitializeComponent();
            // Pre-fill with a few empty rows
            _rows.Add(new BulkImportRow());
            _rows.Add(new BulkImportRow());
            _rows.Add(new BulkImportRow());
            BulkGrid.ItemsSource = _rows;
        }

        private void BrowseCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select CSV File",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = ".csv"
            };

            if (dlg.ShowDialog() == true)
            {
                _csvFilePath = dlg.FileName;
                ImportCsvBtn.IsEnabled = true;
                StatusText.Text = $"Selected: {System.IO.Path.GetFileName(_csvFilePath)}";
                StatusText.Visibility = Visibility.Visible;
            }
        }

        private void ImportCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_csvFilePath) || !File.Exists(_csvFilePath))
            {
                MessageBox.Show("Please select a valid CSV file first.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var lines = File.ReadAllLines(_csvFilePath);
                int imported = 0;
                _rows.Clear();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Skip header row
                    var trimmed = line.Trim().ToLower();
                    if (trimmed.StartsWith("name") || trimmed.StartsWith("#")) continue;

                    var parts = line.Split(',');
                    if (parts.Length < 2) continue;

                    var row = new BulkImportRow
                    {
                        Name = parts[0].Trim(),
                        SKU = parts.Length > 1 ? parts[1].Trim() : "",
                        Category = parts.Length > 2 ? parts[2].Trim() : "Medicine",
                        UnitType = parts.Length > 3 ? parts[3].Trim() : "Tablets",
                        ExpiryDateStr = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd")
                    };

                    if (parts.Length > 4 && int.TryParse(parts[4].Trim(), out int qty))
                        row.StockQuantity = qty;

                    if (parts.Length > 5 && decimal.TryParse(parts[5].Trim(), out decimal price))
                        row.SellingUnitPrice = price;

                    if (parts.Length > 6)
                        row.ExpiryDateStr = parts[6].Trim();

                    _rows.Add(row);
                    imported++;
                }

                StatusText.Text = $"✓ Loaded {imported} items from CSV. Review and click SAVE ALL.";
                StatusText.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading CSV: {ex.Message}", "Import Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Gather valid rows from the grid
                var validRows = _rows.Where(r => !string.IsNullOrWhiteSpace(r.Name)).ToList();

                if (!validRows.Any())
                {
                    MessageBox.Show("No valid items to save. Please enter at least one item with a Name.", 
                        "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int savedCount = 0;
                foreach (var row in validRows)
                {
                    DateTime? expiry = null;
                    if (DateTime.TryParse(row.ExpiryDateStr, out var dt))
                        expiry = dt;

                    var item = new InventoryItem
                    {
                        Id = 0,
                        Name = row.Name.Trim(),
                        SKU = string.IsNullOrWhiteSpace(row.SKU) ? "SKU-" + DateTime.Now.Ticks.ToString().Substring(10) + savedCount : row.SKU.Trim(),
                        Category = string.IsNullOrWhiteSpace(row.Category) ? "Medicine" : row.Category.Trim(),
                        UnitType = string.IsNullOrWhiteSpace(row.UnitType) ? "Tablets" : row.UnitType.Trim(),
                        StockQuantity = row.StockQuantity,
                        SellingUnitPrice = row.SellingUnitPrice,
                        ExpiryDate = expiry,
                        LastStockUpdate = DateTime.Now,
                        IsActive = true
                    };

                    DataManager.AddInventoryItem(item);
                    savedCount++;
                }

                MessageBox.Show($"Successfully imported {savedCount} item(s) to inventory!", "Import Complete",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Close dialog with success
                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute("True", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving items: {ex.Message}", "Save Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
