using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMS.Core.Common.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            if (IsInverted || (parameter != null && parameter.ToString() == "Inverse"))
            {
                val = !val;
            }

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
