using NoxyTools.Wpf.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace NoxyTools.Wpf.Views;

public partial class NoxypediaSearchView : UserControl
{
    private NoxypediaSearchViewModel? _vm;

    public NoxypediaSearchView()
    {
        InitializeComponent();

        // キーバインド (F1/F2/F3 탭 전환, Alt+Left/Right 이전/다음)
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => SetTab(0)), new KeyGesture(Key.F1)));
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => SetTab(1)), new KeyGesture(Key.F2)));
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => SetTab(2)), new KeyGesture(Key.F3)));
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => _vm?.NavigatePreviousCommand.Execute(null)), new KeyGesture(Key.Left, ModifierKeys.Alt)));
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => _vm?.NavigateNextCommand.Execute(null)), new KeyGesture(Key.Right, ModifierKeys.Alt)));

        DataContextChanged += OnDataContextChanged;
        Loaded += OnLoaded;
    }

    private void SetTab(int index)
    {
        if (_vm != null) _vm.SelectedTabIndex = index;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Ctrl+L / Alt+D to focus filter box (forwarded to SearchItemVM)
        var window = Window.GetWindow(this);
        if (window != null)
        {
            window.KeyDown += OnWindowKeyDown;
        }
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
        if (_vm != null)
            _vm.PropertyChanged -= OnVmPropertyChanged;

        _vm = DataContext as NoxypediaSearchViewModel;
        if (_vm != null)
            _vm.PropertyChanged += OnVmPropertyChanged;

        SyncAllDocuments();
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(NoxypediaSearchViewModel.InfoDocument):
                SyncDocument(InfoRichTextBox, _vm?.InfoDocument);
                break;
            case nameof(NoxypediaSearchViewModel.DropCreepsDocument):
                SyncDocument(DropCreepsRichTextBox, _vm?.DropCreepsDocument);
                break;
            case nameof(NoxypediaSearchViewModel.TechDocument):
                SyncDocument(TechRichTextBox, _vm?.TechDocument);
                break;
            case nameof(NoxypediaSearchViewModel.TechGradeDocument):
                SyncDocument(TechGradeRichTextBox, _vm?.TechGradeDocument);
                break;
        }
    }

    private void SyncAllDocuments()
    {
        SyncDocument(InfoRichTextBox, _vm?.InfoDocument);
        SyncDocument(DropCreepsRichTextBox, _vm?.DropCreepsDocument);
        SyncDocument(TechRichTextBox, _vm?.TechDocument);
        SyncDocument(TechGradeRichTextBox, _vm?.TechGradeDocument);
    }

    private static void SyncDocument(RichTextBox rtb, FlowDocument? doc)
    {
        rtb.Document = doc ?? new FlowDocument();
    }
}

/// <summary>간단한 인라인 RelayCommand (코드비하인드 전용)</summary>
file sealed class RelayCommand(Action<object?> execute) : ICommand
{
    public event EventHandler? CanExecuteChanged { add { } remove { } }
    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter) => execute(parameter);
}
