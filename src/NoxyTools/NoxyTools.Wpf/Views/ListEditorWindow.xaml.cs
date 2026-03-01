using NoxyTools.Wpf.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoxyTools.Wpf.Views;

public partial class ListEditorWindow : Window
{
    public ListEditorWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        KeyDown += (_, e) =>
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        };
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ListEditorViewModel oldVm)
            oldVm.CloseRequested -= OnCloseRequested;
        if (e.NewValue is ListEditorViewModel newVm)
            newVm.CloseRequested += OnCloseRequested;
    }

    private void OnCloseRequested(object? sender, EventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    /// <summary>왼쪽 목록 항목 더블클릭 → 추가</summary>
    private void LeftItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not ListEditorViewModel vm) return;
        if (sender is ListBoxItem { DataContext: ListEditorEntry entry })
            vm.AddCommand.Execute(entry);
    }

    /// <summary>오른쪽 목록 항목 더블클릭 → 삭제</summary>
    private void RightItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not ListEditorViewModel vm) return;
        if (sender is ListBoxItem { DataContext: ListEditorEntry entry })
            vm.RemoveCommand.Execute(entry);
    }
}
