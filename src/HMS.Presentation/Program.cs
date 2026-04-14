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
            app.InitializeComponent();
            
            // Start with the LoginView
            var mainWindow = new HMS.Core.Views.Auth.LoginView();
            app.Run(mainWindow);
        }
    }
}
