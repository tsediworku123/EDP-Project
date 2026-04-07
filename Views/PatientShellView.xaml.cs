using System.Windows;
using System.Windows.Input;

namespace ClinicAppointmentSystem.Views
{
    public partial class PatientShellView : Window
    {
        public PatientShellView()
        {
            InitializeComponent();
            var vm = new ViewModels.PatientShellViewModel();
            this.DataContext = vm;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Close();
    }
}
