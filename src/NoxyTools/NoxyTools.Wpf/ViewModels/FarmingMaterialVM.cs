using CommunityToolkit.Mvvm.ComponentModel;
using Noxypedia.Model;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 파밍 시뮬레이터의 필요 재료 요약 한 행.
/// </summary>
public partial class FarmingMaterialVM : ObservableObject
{
    public ItemSet Item { get; }
    public string? ImageUrl { get; }

    [ObservableProperty] private BitmapSource? _image;
    [ObservableProperty] private string _materialName = "";
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private bool _isHighlighted;

    /// <summary>등급 이름 (표시용)</summary>
    public string GradeName { get; }

    /// <summary>
    /// '보조-도박' 등급 아이템의 도박 재료 이름.
    /// 해당 등급이 아니면 null.
    /// </summary>
    public string? GambleMaterialName { get; }

    public FarmingMaterialVM(ItemSet item, string? imageUrl, int count)
    {
        Item          = item;
        ImageUrl      = imageUrl;
        _materialName = item.Name;
        _totalCount   = count;
        GradeName     = item.Grade.Name;

        // 보조-도박 등급이면 BeforeItems[0]이 '재료-도박' 아이템
        GambleMaterialName = (item.Grade.Name == "보조-도박" && item.BeforeItems.Count > 0)
            ? item.BeforeItems[0].Name
            : null;
    }
}
