using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 장비 슬롯 하나의 ViewModel.
/// </summary>
public partial class SlotViewModel : ObservableObject
{
    public int Index { get; }

    [ObservableProperty] private string  _text           = "(비어있음)";
    [ObservableProperty] private BitmapSource? _image;
    [ObservableProperty] private SolidColorBrush _foreground = new(Color.FromRgb(241, 241, 241));
    [ObservableProperty] private bool   _isHighlighted;

    /// <summary>현재 표시 중인 이미지의 URL. OnImageReady 에서 일치 여부 확인용.</summary>
    public string? ImageUrl { get; set; }

    public SlotViewModel(int index) => Index = index;
}

/// <summary>
/// 수치 + 색상 쌍. 장비 시뮬레이터 요약 스탯 표시용.
/// </summary>
public partial class StatValueVM : ObservableObject
{
    [ObservableProperty] private string _text = "-";
    [ObservableProperty] private SolidColorBrush _foreground = new(Color.FromRgb(241, 241, 241));

    private static readonly SolidColorBrush PlusColor =
        new SolidColorBrush(Colors.LimeGreen).Apply(b => b.Freeze());
    private static readonly SolidColorBrush MinusColor =
        new SolidColorBrush(Colors.Crimson).Apply(b => b.Freeze());
    private static readonly SolidColorBrush NullColor =
        new SolidColorBrush(Color.FromRgb(241, 241, 241)).Apply(b => b.Freeze());

    public void Update(int? value)
    {
        Text       = value.HasValue ? $"{value.Value:+#,0;-#,0;}" : "-";
        Foreground = value.HasValue
            ? (value.Value >= 0 ? PlusColor : MinusColor)
            : NullColor;
    }

    public void UpdateDouble(double? value)
    {
        Text       = value.HasValue ? $"{value.Value:+#,0.##;-#,0.##;}" : "-";
        Foreground = value.HasValue
            ? (value.Value >= 0 ? PlusColor : MinusColor)
            : NullColor;
    }
}

file static class BrushExt
{
    public static T Apply<T>(this T obj, Action<T> action) { action(obj); return obj; }
}
