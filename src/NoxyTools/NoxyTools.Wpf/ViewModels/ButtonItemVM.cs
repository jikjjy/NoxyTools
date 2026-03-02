using CommunityToolkit.Mvvm.ComponentModel;
using Noxypedia.Model;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// NoxypediaSearchView 내 동적 버튼 하나(재료/지역/테크)에 해당하는 경량 VM
/// </summary>
public partial class ButtonItemVM : ObservableObject
{
    public ItemSet Item { get; }
    public string Text { get; }

    /// <summary>이미지 URL — CacheService.RequestImage() 호출 후 ImageReady 이벤트로 갱신됩니다.</summary>
    public string? ImageUrl { get; }

    /// <summary>로드 완료 후 자동 갱신되는 이미지. null이면 아직 로딩 중입니다.</summary>
    [ObservableProperty] private BitmapSource? _image;

    [ObservableProperty] private bool _isSelected;

    public ButtonItemVM(ItemSet item, string text, string? imageUrl = null, BitmapSource? image = null)
    {
        Item = item;
        Text = text;
        ImageUrl = imageUrl;
        _image = image;
    }
}
