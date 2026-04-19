using System;
using System.Windows;
using HMS.Core.Views;

namespace HMS.Core
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try 
            {
                // Initialize Data and Database
                HMS.Core.AppLogic.Services.DataManager.EnsureLoaded();

                var app = new App();
                app.ShutdownMode = ShutdownMode.OnLastWindowClose;
                
                // Start with the LoginView
                var loginView = new HMS.Core.Views.Auth.LoginView();
                app.Run(loginView);
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("crashlog.txt", $"CRITICAL STARTUP ERROR:\n{ex.Message}\n\n{ex.StackTrace}\n\nInner: {ex.InnerException?.Message}");
                MessageBox.Show($"The system encountered a fatal error during startup and cannot continue. Details have been saved to crashlog.txt.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
