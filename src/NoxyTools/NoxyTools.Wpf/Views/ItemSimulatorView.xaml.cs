using NoxyTools.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NoxyTools.Wpf.Views;

public partial class ItemSimulatorView : UserControl
{
    private ItemSimulatorViewModel? _vm;

    public ItemSimulatorView()
    {
        InitializeComponent();

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
        if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            _vm.SearchItemVM.ToggleFavoriteCommand?.Execute(null);
            e.Handled = true;
        }
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        _vm = DataContext as ItemSimulatorViewModel;
    }

    private void AnySlots_RightClick(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is not DependencyObject dep) return;
        var btn = FindParent<Button>(dep);
        if (btn?.Tag is not SlotViewModel slotVm) return;
        var ic = FindParent<ItemsControl>(btn);
        if (ic?.Tag is SimulatorSetViewModel setVm)
        {
            setVm.SlotUnmountCommand.Execute(slotVm);
            e.Handled = true;
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