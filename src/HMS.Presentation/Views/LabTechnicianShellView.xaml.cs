using System.Windows;
using System.Windows.Input;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class LabTechnicianShellView : Window
    {
        public LabTechnicianShellView()
        {
            InitializeComponent();
            DataContext = new LabTechnicianShellViewModel();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
