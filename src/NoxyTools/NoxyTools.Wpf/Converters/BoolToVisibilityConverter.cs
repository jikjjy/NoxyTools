using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NoxyTools.Wpf.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class BoolToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool bVal = value is bool b && b;
        if (Invert) bVal = !bVal;
        return bVal ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool bVal = value is Visibility v && v == Visibility.Visible;
        if (Invert) bVal = !bVal;
        return bVal;
    }
}
