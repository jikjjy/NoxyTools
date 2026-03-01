using NoxyTools.Wpf.ViewModels;
using System;
using System.Windows;

namespace NoxyTools.Wpf.Views;

public partial class SelectItemDialogWindow : Window
{
    public SelectItemDialogWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        KeyDown += (_, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        };
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is SelectItemDialogViewModel oldVm)
            oldVm.CloseRequested -= OnCloseRequested;
        if (e.NewValue is SelectItemDialogViewModel newVm)
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
}
