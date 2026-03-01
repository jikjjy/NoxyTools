using Microsoft.Win32;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels;
using NoxyTools.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NoxyTools.Wpf.Services;

public class DialogService : IDialogService
{
    private readonly CacheService _cache;

    public DialogService(CacheService cache)
    {
        _cache = cache;
    }
    public string? ShowOpenFileDialog(string title = "파일 열기", string filter = "All Files|*.*")
    {
        var dlg = new OpenFileDialog
        {
            Title = title,
            Filter = filter
        };
        return dlg.ShowDialog() == true ? dlg.FileName : null;
    }

    public string? ShowSaveFileDialog(string title = "파일 저장", string filter = "All Files|*.*", string defaultFileName = "")
    {
        var dlg = new SaveFileDialog
        {
            Title = title,
            Filter = filter,
            FileName = defaultFileName
        };
        return dlg.ShowDialog() == true ? dlg.FileName : null;
    }

    public string? ShowOpenFolderDialog(string title = "폴더 선택", string? initialPath = null)
    {
        var dlg = new OpenFolderDialog
        {
            Title = title
        };
        if (!string.IsNullOrEmpty(initialPath) && System.IO.Directory.Exists(initialPath))
            dlg.InitialDirectory = initialPath;
        return dlg.ShowDialog() == true ? dlg.FolderName : null;
    }

    public bool ShowMessageBox(string message, string title = "알림", bool isYesNo = false)
    {
        var button = isYesNo ? MessageBoxButton.YesNo : MessageBoxButton.OK;
        var result = MessageBox.Show(message, title, button, MessageBoxImage.Information);
        return result == MessageBoxResult.OK || result == MessageBoxResult.Yes;
    }

    public IReadOnlyList<TData>? ShowListEditorDialog<TData>(
        IEnumerable<TData> leftData,
        IEnumerable<TData> rightData,
        string leftTitle = "전체 목록",
        string rightTitle = "선택 목록",
        Func<TData, string>? displayName = null,
        int maxCount = 0) where TData : class
    {
        var vm = new ListEditorViewModel
        {
            LeftTitle  = leftTitle,
            RightTitle = rightTitle
        };
        vm.Configure(maxCount);

        // 오른쪽 목록에 있는 이름 집합 (왼쪽에서 제외)
        var rightNames = new HashSet<string>(rightData
            .Select(item => displayName?.Invoke(item) ?? item.ToString() ?? ""));

        foreach (var item in rightData)
        {
            var name = displayName?.Invoke(item) ?? item.ToString() ?? "";
            vm.RightItems.Add(new ListEditorEntry(name, item));
        }
        foreach (var item in leftData)
        {
            var name = displayName?.Invoke(item) ?? item.ToString() ?? "";
            if (!rightNames.Contains(name))
                vm.LeftItems.Add(new ListEditorEntry(name, item));
        }

        var window = new ListEditorWindow { DataContext = vm };

        bool? result = window.ShowDialog();
        if (result != true) return null;

        return vm.GetRightData<TData>().ToList();
    }

    public ItemSet? ShowSelectItemDialog(IEnumerable<ItemSet> items)
    {
        var vm = new SelectItemDialogViewModel();
        vm.Initialize(items, _cache);

        var window = new SelectItemDialogWindow { DataContext = vm };
        bool? result = window.ShowDialog();
        if (result != true) return null;

        return vm.SelectedItem;
    }
}
