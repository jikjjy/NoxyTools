using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NoxyTools.Wpf.ViewModels;

namespace NoxyTools.Wpf.Views;

public partial class ItemSimulatorView : UserControl
{
    private ItemSimulatorViewModel? _vm;

    public ItemSimulatorView()
    {
        InitializeComponent();

        // Alt+Left/Right 키바인딩
        InputBindings.Add(new KeyBinding(
            new RelayCmd(_ => _vm?.NavigatePreviousCommand.Execute(null)),
            new KeyGesture(Key.Left, ModifierKeys.Alt)));
        InputBindings.Add(new KeyBinding(
            new RelayCmd(_ => _vm?.NavigateNextCommand.Execute(null)),
            new KeyGesture(Key.Right, ModifierKeys.Alt)));

        DataContextChanged += OnDataContextChanged;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window != null)
            window.KeyDown += OnWindowKeyDown;
    }

    private void OnWindowKeyDown(object sender, KeyEventArgs e)
    {
        if (_vm == null) return;
        bool ctrlB = e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) != 0;
        if (ctrlB)
        {
            _vm.SearchItemVM.ToggleFavoriteCommand?.Execute(null);
            e.Handled = true;
        }
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_vm != null) _vm.PropertyChanged -= OnVmPropertyChanged;
        _vm = DataContext as ItemSimulatorViewModel;
        if (_vm != null) _vm.PropertyChanged += OnVmPropertyChanged;
        SyncAllDocuments();
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ItemSimulatorViewModel.InfoDocument):
                SyncDoc(InfoDetailRichTextBox, _vm?.InfoDocument);
                break;
            case nameof(ItemSimulatorViewModel.UniqueOptionsDocument):
                SyncDoc(UniqueOptionsRichTextBox, _vm?.UniqueOptionsDocument);
                break;
        }
    }

    private void SyncAllDocuments()
    {
        SyncDoc(InfoDetailRichTextBox,   _vm?.InfoDocument);
        SyncDoc(UniqueOptionsRichTextBox, _vm?.UniqueOptionsDocument);
    }

    private static void SyncDoc(RichTextBox rtb, FlowDocument? doc) =>
        rtb.Document = doc ?? new FlowDocument();

    // ─── 슬롯 우클릭 → 해제 ───────────────────────────────────────────
    private void SlotsItemsControl_RightClick(object sender, MouseButtonEventArgs e)
    {
        if (_vm == null) return;

        // 클릭된 요소에서 SlotViewModel을 찾아 해제
        if (e.OriginalSource is DependencyObject dep)
        {
            var btn = FindParent<Button>(dep);
            if (btn?.Tag is NoxyTools.Wpf.ViewModels.SlotViewModel slotVm)
            {
                _vm.SlotUnmountCommand.Execute(slotVm);
                e.Handled = true;
            }
        }
    }

    private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        var current = VisualTreeHelper.GetParent(child);
        while (current != null)
        {
            if (current is T typed) return typed;
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }
}

file sealed class RelayCmd(Action<object?> execute) : ICommand
{
    public event EventHandler? CanExecuteChanged { add { } remove { } }
    public bool CanExecute(object? p) => true;
    public void Execute(object? p) => execute(p);
}

