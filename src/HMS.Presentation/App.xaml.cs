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
            MessageBox.Show($"FATAL ERROR: {e.Exception.Message}\n\nStack Trace: {e.Exception.StackTrace}", "System Crash Protection", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            System.Environment.Exit(1);
        }
    }
}
