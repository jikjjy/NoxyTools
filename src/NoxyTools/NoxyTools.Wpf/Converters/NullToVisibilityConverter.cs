using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NoxyTools.Wpf.Converters;

/// <summary>
/// null → Collapsed, 非null → Visible (Invert=true にすると逆転)
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public class NullToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isNull = value is null;
        bool show = Invert ? isNull : !isNull;
        return show ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
