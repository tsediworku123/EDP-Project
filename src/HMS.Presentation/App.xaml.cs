using System.Windows;

namespace HMS.Core
{
    public partial class App : System.Windows.Application
    {
        public App()
        {
            InitializeComponent();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var msg = $"FATAL DISPATCHER ERROR:\n{e.Exception.Message}\n\nStack: {e.Exception.StackTrace}\n\nInner: {e.Exception.InnerException?.Message}";
            try { System.IO.File.WriteAllText("crashlog_dispatcher.txt", msg); } catch { }
            MessageBox.Show(msg, "System Crash Protection", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            // System.Environment.Exit(1); // Disabled to prevent fatal shutdown during debugging
        }
    }
}
