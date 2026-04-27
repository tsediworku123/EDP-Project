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
    public class BulkPatientRow
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; } = "Male";
        public int Age { get; set; } = 25;
        public string Address { get; set; }
        public string BloodType { get; set; } = "Unknown";
    }

    public partial class BulkImportPatientsDialog : UserControl
    {
        private ObservableCollection<BulkPatientRow> _rows = new ObservableCollection<BulkPatientRow>();
        private string _csvFilePath;

        public BulkImportPatientsDialog()
        {
            InitializeComponent();
            _rows.Add(new BulkPatientRow());
            _rows.Add(new BulkPatientRow());
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
                    var row = new BulkPatientRow
                    {
                        FullName = p[0].Trim(),
                        Phone = p.Length > 1 ? p[1].Trim() : "",
                        Gender = p.Length > 2 ? p[2].Trim() : "Male",
                        Address = p.Length > 4 ? p[4].Trim() : "",
                        BloodType = p.Length > 5 ? p[5].Trim() : "Unknown"
                    };
                    if (p.Length > 3 && int.TryParse(p[3].Trim(), out int age)) row.Age = age;
                    _rows.Add(row);
                    count++;
                }
                StatusText.Text = $"✓ Loaded {count} patients from CSV.";
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
                    MessageBox.Show("Enter at least one patient name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int saved = 0;
                foreach (var row in valid)
                {
                    var patient = new Patient
                    {
                        FullName = row.FullName.Trim(),
                        Phone = string.IsNullOrWhiteSpace(row.Phone) ? "000-" + DateTime.Now.Ticks.ToString().Substring(12) : row.Phone.Trim(),
                        Gender = row.Gender?.Trim() ?? "Male",
                        DateOfBirth = DateTime.Now.AddYears(-row.Age),
                        Address = row.Address?.Trim(),
                        BloodGroup = row.BloodType?.Trim() ?? "Unknown",
                        PatientCode = "PAT-PENDING",
                        IsActive = true
                    };

                    DataManager.RegisterPatient(patient);
                    saved++;
                }
                MessageBox.Show($"Successfully registered {saved} patient(s)!", "Import Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute("True", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
