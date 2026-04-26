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
    public class BulkDoctorRow
    {
        public string FullName { get; set; }
        public string Specialization { get; set; } = "General Medicine";
        public string Department { get; set; } = "General Medicine";
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public partial class BulkImportDoctorsDialog : UserControl
    {
        private ObservableCollection<BulkDoctorRow> _rows = new ObservableCollection<BulkDoctorRow>();
        private string _csvFilePath;

        public BulkImportDoctorsDialog()
        {
            InitializeComponent();
            _rows.Add(new BulkDoctorRow());
            _rows.Add(new BulkDoctorRow());
            BulkGrid.ItemsSource = _rows;
        }

        private void BrowseCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Title = "Select CSV", Filter = "CSV files (*.csv)|*.csv|All (*.*)|*.*" };
            if (dlg.ShowDialog() == true)
            {
                _csvFilePath = dlg.FileName;
                ImportCsvBtn.IsEnabled = true;
                StatusText.Text = $"Selected: {Path.GetFileName(_csvFilePath)}";
                StatusText.Visibility = Visibility.Visible;
            }
        }

        private void ImportCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_csvFilePath) || !File.Exists(_csvFilePath)) return;
            try
            {
                var lines = File.ReadAllLines(_csvFilePath);
                _rows.Clear();
                int count = 0;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var t = line.Trim().ToLower();
                    if (t.StartsWith("fullname") || t.StartsWith("name") || t.StartsWith("#")) continue;
                    var p = line.Split(',');
                    if (p.Length < 1) continue;
                    _rows.Add(new BulkDoctorRow
                    {
                        FullName = p[0].Trim(),
                        Specialization = p.Length > 1 ? p[1].Trim() : "General Medicine",
                        Department = p.Length > 2 ? p[2].Trim() : "General Medicine",
                        Phone = p.Length > 3 ? p[3].Trim() : "",
                        Email = p.Length > 4 ? p[4].Trim() : ""
                    });
                    count++;
                }
                StatusText.Text = $"✓ Loaded {count} doctors from CSV.";
                StatusText.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var valid = _rows.Where(r => !string.IsNullOrWhiteSpace(r.FullName)).ToList();
                if (!valid.Any())
                {
                    MessageBox.Show("Enter at least one doctor name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int saved = 0;
                foreach (var row in valid)
                {
                    var doc = new Doctor
                    {
                        FullName = row.FullName.Trim(),
                        Specialization = string.IsNullOrWhiteSpace(row.Specialization) ? "General Medicine" : row.Specialization.Trim(),
                        Department = string.IsNullOrWhiteSpace(row.Department) ? "General Medicine" : row.Department.Trim(),
                        PhoneNumber = row.Phone?.Trim(),
                        Email = row.Email?.Trim(),
                        IsActive = true,
                        WorkingDays = "Mon,Tue,Wed,Thu,Fri",
                        WorkingHoursStart = new TimeSpan(8, 0, 0),
                        WorkingHoursEnd = new TimeSpan(16, 0, 0)
                    };
                    DataManager.RegisterDoctor(doc);
                    saved++;
                }
                MessageBox.Show($"Successfully imported {saved} doctor(s)!", "Import Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute("True", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
