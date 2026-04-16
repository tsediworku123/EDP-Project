using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminDashboardViewModel : ObservableObject
    {
        public int TotalDoctors => DataManager.Doctors?.Count ?? 0;
        public int TotalPatients => DataManager.Patients?.Count ?? 0;
        public int TotalAppointments => DataManager.Appointments?.Count ?? 0;
        public List<User> RecentUsers => DataManager.Users?.Take(5).ToList() ?? new List<User>();

        public ICommand BackupCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ManagePermissionsCommand { get; }
        public ICommand ViewAuditTrailsCommand { get; }

        public AdminDashboardViewModel()
        {
            DataManager.EnsureLoaded();

            BackupCommand = new RelayCommand(() =>
            {
                try 
                {
                    DataManager.BackupData();
                    MessageBox.Show("System Database (JSON) has been backed up successfully to the Archives directory.", "Backup Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Backup failed: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            ExportCommand = new RelayCommand(() =>
            {
                if (DataManager.Patients != null)
                {
                    CSVExporter.ExportToCSV(DataManager.Patients, $"Global_Patient_Report_{DateTime.Now:yyyyMMdd}.csv");
                }
            });

            ManagePermissionsCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Access Control List (ACL) modifications are currently locked for stability. Use the 'User Accounts' tab for basic role management.", "Security Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
            });

            ViewAuditTrailsCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Live System Audit logs are currently being recorded in the background. A visual log viewer is scheduled for the next release.", "Feature Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }
}
