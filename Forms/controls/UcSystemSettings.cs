using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClinicAppointmentSystem;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcSystemSettings : UserControl
    {
        private Timer _autoBackupTimer;

        public UcSystemSettings()
        {
            InitializeComponent();
            LoadValues();
        }

        private void LoadValues()
        {
            cmbStart.Items.Clear();
            cmbEnd.Items.Clear();
            for (int i = 0; i < 24; i++) {
                string t = FormatHour(i);
                cmbStart.Items.Add(t);
                cmbEnd.Items.Add(t);
            }
            cmbStart.Text = FormatHour(DataManager.ClinicOpStart.Hours);
            cmbEnd.Text   = FormatHour(DataManager.ClinicOpEnd.Hours);

            chkDouble.Checked = !DataManager.AllowDoubleBooking;
            nSlots.Value = DataManager.DefaultSlotDuration;

            var s = LoadSettings();
            txtClinicName.Text    = s.GetVal("Name",    "Alpha Clinic");
            txtClinicAddr.Text    = s.GetVal("Address", "");
            txtClinicPhone.Text   = s.GetVal("Phone",   "");
            chkAutoBackup.Checked = s.GetVal("AutoBackup", "false") == "true";
            chkDarkMode.Checked   = s.GetVal("DarkMode",   "false") == "true";
        }

        private static string FormatHour(int h) =>
            h < 12 ? (h == 0 ? "12 AM" : h + " AM") : (h == 12 ? "12 PM" : (h - 12) + " PM");

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int startH = ParseHour(cmbStart.Text);
                int endH   = ParseHour(cmbEnd.Text);

                if (startH >= endH) {
                    MessageBox.Show("Clinic open time must be earlier than close time.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataManager.ClinicOpStart       = new TimeSpan(startH, 0, 0);
                DataManager.ClinicOpEnd         = new TimeSpan(endH,   0, 0);
                DataManager.DefaultSlotDuration = (int)nSlots.Value;
                DataManager.AllowDoubleBooking  = !chkDouble.Checked;

                var s = new Dictionary<string, string> {
                    ["Name"]        = txtClinicName.Text.Trim(),
                    ["Address"]     = txtClinicAddr.Text.Trim(),
                    ["Phone"]       = txtClinicPhone.Text.Trim(),
                    ["AutoBackup"]  = chkAutoBackup.Checked ? "true" : "false",
                    ["DarkMode"]    = chkDarkMode.Checked   ? "true" : "false"
                };
                SaveSettings(s);

                ApplyAutoBackup(chkAutoBackup.Checked);
                ApplyTheme(chkDarkMode.Checked);

                DataManager.LogAudit(DataManager.CurrentUser?.Username, "Updated System Settings");
                MessageBox.Show("All settings saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyTheme(bool dark)
        {
            Form f = this.FindForm();
            if (f == null) return;
            Color bg = dark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(248, 250, 252);
            Color sb = dark ? Color.FromArgb(7,  15, 35) : Color.FromArgb(30, 41, 59);
            f.BackColor = bg;
            foreach (Control c in f.Controls) {
                if (c.Name == "sidebarPanel") c.BackColor = sb;
                if (c.Name == "mainPanel")    c.BackColor = bg;
            }
        }

        private void ApplyAutoBackup(bool enabled)
        {
            _autoBackupTimer?.Stop();
            _autoBackupTimer?.Dispose();
            _autoBackupTimer = null;
            if (!enabled) return;
            _autoBackupTimer = new Timer { Interval = 60 * 60 * 1000 };
            _autoBackupTimer.Tick += (s, e2) => {
                var svc = new Services.BackupService();
                var r = svc.CreateBackup();
                if (r.Success) DataManager.LastBackupTime = DateTime.Now;
            };
            _autoBackupTimer.Start();
        }

        private static int ParseHour(string s)
        {
            if (s.Contains("AM")) { int h = int.Parse(s.Replace(" AM", "")); return h == 12 ? 0 : h; }
            else                  { int h = int.Parse(s.Replace(" PM", "")); return h == 12 ? 12 : h + 12; }
        }

        // ── Simple key=value settings file ──────────────────────────────────
        private static readonly string SettingsFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "clinic_settings.ini");

        private Dictionary<string, string> LoadSettings()
        {
            var d = new Dictionary<string, string>();
            try {
                if (File.Exists(SettingsFile))
                    foreach (var line in File.ReadAllLines(SettingsFile)) {
                        int eq = line.IndexOf('=');
                        if (eq > 0) d[line.Substring(0, eq).Trim()] = line.Substring(eq + 1).Trim();
                    }
            } catch { }
            return d;
        }

        private void SaveSettings(Dictionary<string, string> d)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
            var lines = new List<string>();
            foreach (var kv in d) lines.Add($"{kv.Key}={kv.Value}");
            File.WriteAllLines(SettingsFile, lines);
        }
    }

    // Extension helper on Dictionary
    internal static class DictExtensions
    {
        public static string GetVal(this Dictionary<string, string> d, string key, string def)
            => d.TryGetValue(key, out var v) ? v : def;
    }
}
