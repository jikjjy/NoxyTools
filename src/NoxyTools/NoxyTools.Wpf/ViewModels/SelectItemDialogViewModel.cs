using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 선택 다이얼로그 ViewModel.
/// IDialogService.ShowSelectItemDialog() 에서 생성됩니다.
/// </summary>
public partial class SelectItemDialogViewModel : ViewModelBase
{
    // ── 아이템 목록 ──────────────────────────────────────────────

    public ObservableCollection<ButtonItemVM> Items { get; } = new();

    // ── 선택 결과 ────────────────────────────────────────────────

    /// <summary>사용자가 선택한 아이템. 확인 후에 유효합니다.</summary>
    public ItemSet? SelectedItem { get; private set; }

    /// <summary>다이얼로그를 닫도록 View에 요청하는 이벤트.</summary>
    public event EventHandler? CloseRequested;

    // ── 내부 필드 ────────────────────────────────────────────────

    private CacheService? _cache;

    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// 아이템 목록을 초기화합니다.
    /// </summary>
    /// <param name="items">표시할 아이템 목록.</param>
    /// <param name="cache">이미지 로딩에 사용할 CacheService. null이면 이미지 없이 텍스트만 표시.</param>
    public void Initialize(IEnumerable<ItemSet> items, CacheService? cache = null)
    {
        // 이전 구독 해제
        if (_cache != null) _cache.ImageReady -= OnImageReady;
        _cache = cache;

        Items.Clear();
        foreach (var item in items)
        {
            string? imageUrl = null;
            if (cache != null)
            {
                try
                {
                    var clipImages = item.ClipImages;
                    if (clipImages.ContainsKey(ClipImageKeys.Item.MainImage))
                        imageUrl = clipImages[ClipImageKeys.Item.MainImage].SourceURL;
                }
                catch { /* 이미지 URL 조회 실패 시 무시 */ }
            }

            var displayName = $"[{item.Grade.Name}]{item.Name}";
            // 이미지 URL이 있으면 백그라운드 로드 요청
            if (imageUrl != null) cache!.RequestImage(imageUrl);
            Items.Add(new ButtonItemVM(item, displayName,
                imageUrl: imageUrl, image: imageUrl != null ? cache!.GetImage(imageUrl) : null));
        }

        // 새 구독: 이미지 도착 시 해당 목록 항목 갱신
        if (_cache != null) _cache.ImageReady += OnImageReady;
    }

    private void OnImageReady(object? sender, string url)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
        {
            foreach (var vm in Items)
            {
                if (vm.ImageUrl == url)
                    vm.Image = _cache?.GetImage(url);
            }
        });
    }

    // ── 커맨드 ───────────────────────────────────────────────────

    [RelayCommand]
    private void SelectItem(ButtonItemVM buttonVm)
    {
        SelectedItem = buttonVm.Item;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
