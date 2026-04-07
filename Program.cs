using System;
using System.Windows;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    internal static class Program
    {
        public static MainContainer MainForm { get; private set; }

        [STAThread]
        static void Main()
        {
            // Initialize WPF resources for the hybrid app
            var app = new App();
            app.InitializeComponent();
            app.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainContainer();
            System.Windows.Forms.Application.Run(MainForm);
        }
    }
}
