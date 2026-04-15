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
            var app = new App();
            app.ShutdownMode = ShutdownMode.OnLastWindowClose;
            app.InitializeComponent();
            
            // Start with the LoginView
            var loginView = new HMS.Core.Views.Auth.LoginView();
            app.Run(loginView);
        }
    }
}
