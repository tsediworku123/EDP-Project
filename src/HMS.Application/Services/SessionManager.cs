using System;
using System.IO;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    public class SessionManager : IMessageFilter
    {
        private static SessionManager _instance;
        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        public bool RememberMe { get; set; }
        public bool AutoLogin { get; set; }
        public string SavedUsername { get; set; } = "";
        public string SavedPassword { get; set; } = "";

        private string settingsFilePath;
        private Timer sessionTimer;
        public event EventHandler OnSessionTimeout;

        // Inactivity timeout in minutes
        public int TimeoutMinutes { get; set; } = 30;

        private SessionManager()
        {
            try
            {
                var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClinicAppointmentSystem");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                
                settingsFilePath = Path.Combine(folder, "UserSettings.conf");
                LoadSettings();
            }
            catch { }

            sessionTimer = new Timer();
            sessionTimer.Tick += SessionTimer_Tick;
            
            // 30 min default interval
            sessionTimer.Interval = TimeoutMinutes * 60 * 1000;
        }

        public void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                try {
                    var lines = File.ReadAllLines(settingsFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var val = parts[1].Trim();
                            if (key == "RememberMe") RememberMe = bool.TryParse(val, out var b) ? b : false;
                            if (key == "AutoLogin") AutoLogin = bool.TryParse(val, out var a) ? a : false;
                            if (key == "SavedUsername") SavedUsername = val;
                            if (key == "SavedPassword") SavedPassword = val;
                        }
                    }
                }
                catch { }
            }
        }

        public void SaveSettings()
        {
            try
            {
                var content = $"RememberMe={RememberMe}\r\n" +
                              $"AutoLogin={AutoLogin}\r\n" +
                              $"SavedUsername={SavedUsername}\r\n" +
                              $"SavedPassword={SavedPassword}";
                File.WriteAllText(settingsFilePath, content);
            }
            catch { }
        }

        public void StartSessionTracking()
        {
            sessionTimer.Interval = TimeoutMinutes * 60 * 1000;
            sessionTimer.Start();
            Application.AddMessageFilter(this);
        }

        public void StopSessionTracking()
        {
            sessionTimer.Stop();
            Application.RemoveMessageFilter(this);
        }

        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            sessionTimer.Stop(); // Stop to prevent multiple triggers
            OnSessionTimeout?.Invoke(this, EventArgs.Empty);
        }

        // IMessageFilter implementation
        public bool PreFilterMessage(ref Message m)
        {
            // Windows Messages for Mouse/Keyboard Activity
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_MBUTTONDOWN = 0x0207;
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;

            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP)
            {
                // Reset timer on activity
                if (sessionTimer.Enabled)
                {
                    sessionTimer.Stop();
                    sessionTimer.Start();
                }
            }

            return false;
        }
    }
}
