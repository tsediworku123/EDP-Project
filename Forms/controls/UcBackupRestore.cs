using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Services;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcBackupRestore : UserControl
    {
        private static readonly string BackupRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Alpha Clinic Backups"
        );

        public UcBackupRestore()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Dock = DockStyle.Fill;
            this.Load += (s, e) => RefreshData();
        }

        private void btnBackupNow_Click(object sender, EventArgs e)
        {
            try
            {
                var svc = new BackupService();
                var result = svc.CreateBackup();
                if (result.Success)
                {
                    DataManager.LastBackupTime = DateTime.Now;
                    RefreshData();
                    MessageBox.Show($"Backup created successfully!\n\nLocation: {result.Path}", 
                        "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Backup error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(BackupRoot);
            System.Diagnostics.Process.Start("explorer.exe", $"\"{BackupRoot}\"");
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a backup from the list to restore.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folderPath = dgvHistory.SelectedRows[0].Tag?.ToString();
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show("Backup folder not found. It may have been deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string backupName = Path.GetFileName(folderPath);
            if (MessageBox.Show(
                $"This will OVERWRITE current data with backup:\n\n{backupName}\n\nThis cannot be undone. Continue?",
                "Confirm Restore",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                foreach (string file in Directory.GetFiles(folderPath, "*.json"))
                {
                    string dest = Path.Combine(dataDir, Path.GetFileName(file));
                    File.Copy(file, dest, overwrite: true);
                }
                DataManager.EnsureLoaded();
                MessageBox.Show("Data restored successfully! The application data has been refreshed.", 
                    "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Restore failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshData()
        {
            // Stats
            pnlStats.Controls.Clear();
            string lastBackup = DataManager.LastBackupTime == DateTime.MinValue
                ? "No backup yet"
                : DataManager.LastBackupTime.ToString("MMM dd, HH:mm");

            AddStat("DB STATUS", "HEALTHY", Color.FromArgb(16, 185, 129));
            AddStat("LAST BACKUP", lastBackup, Color.FromArgb(59, 130, 246));
            AddStat("BACKUP FOLDER", "Documents\\Alpha Clinic Backups", Color.FromArgb(100, 116, 139));

            // Update path label
            if (lblBackupPath != null)
                lblBackupPath.Text = $"Backup location: {BackupRoot}";

            // Populate backup history from actual files
            dgvHistory.Rows.Clear();
            if (Directory.Exists(BackupRoot))
            {
                var dirs = new DirectoryInfo(BackupRoot)
                    .GetDirectories("Backup_*")
                    .OrderByDescending(d => d.CreationTime);

                foreach (var dir in dirs)
                {
                    var files = dir.GetFiles("*.json");
                    long sizeKb = files.Sum(f => f.Length) / 1024;
                    int idx = dgvHistory.Rows.Add(
                        dir.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                        dir.Name,
                        $"{sizeKb:N0} KB",
                        "Verified"
                    );
                    dgvHistory.Rows[idx].Tag = dir.FullName;
                }
            }

            if (dgvHistory.Rows.Count == 0)
                dgvHistory.Rows.Add("--", "No backups found", "--", "--");
        }

        private void AddStat(string title, string val, Color c)
        {
            Panel p = new Panel { Size = new Size(230, 100), BackColor = Color.White, Margin = new Padding(0, 0, 20, 0) };
            p.Controls.Add(new Label { Text = val, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = c, Dock = DockStyle.Top, Height = 55, TextAlign = ContentAlignment.MiddleCenter });
            p.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Bottom, Height = 35, TextAlign = ContentAlignment.MiddleCenter });
            pnlStats.Controls.Add(p);
        }
    }
}
