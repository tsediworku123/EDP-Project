using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClinicAppointmentSystem.Utils
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return b ? Visibility.Visible : Visibility.Collapsed;
            if (value != null && parameter != null) return value.ToString() == parameter.ToString() ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b ? Visibility.Visible : Visibility.Collapsed;
            
            // Handle Equality check (e.g. Count == 0)
            if (value != null && parameter != null)
            {
                if (value.ToString() == parameter.ToString()) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            return Visibility.Visible; // Default to visible for null/empty checks
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
