using System.Windows;
using System.Windows.Input;
using HMS.Core.ViewModels;

namespace HMS.Core.Views
{
    public partial class AdminShellView : Window
    {
        public AdminShellView()
        {
            InitializeComponent();
            DataContext = new AdminShellViewModel();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Close();
    }
}
