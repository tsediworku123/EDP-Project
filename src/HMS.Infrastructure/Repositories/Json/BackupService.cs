using System;
using System.IO;

namespace HMS.Core.Infrastructure.Repositories.Json
{
    public class BackupService
    {
        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string BaseBackupDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
            "Alpha Clinic Backups"
        );

        public (bool Success, string Path, string Message) CreateBackup()
        {
            try
            {
                if (!Directory.Exists(DataDirectory))
                    return (false, string.Empty, "Data directory not found.");

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
                string backupFolderPath = Path.Combine(BaseBackupDirectory, $"Backup_{timestamp}");

                if (!Directory.Exists(backupFolderPath))
                    Directory.CreateDirectory(backupFolderPath);

                string[] jsonFiles = Directory.GetFiles(DataDirectory, "*.json");
                if (jsonFiles.Length == 0)
                    return (false, string.Empty, "No JSON data files found to backup.");

                foreach (string file in jsonFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(backupFolderPath, fileName);
                    File.Copy(file, destFile, true);
                }

                return (true, backupFolderPath, $"Backup created successfully at {backupFolderPath}");
            }
            catch (Exception ex)
            {
                return (false, string.Empty, $"Backup failed: {ex.Message}");
            }
        }
    }
}
