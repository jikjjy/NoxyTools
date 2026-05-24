using NoxyTools.Wpf.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NoxyTools.Wpf.Views;

public partial class FarmingSimulatorView : UserControl
{
    private NoxypediaSearchViewModel? _detailVm;

    public FarmingSimulatorView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_detailVm != null)
            _detailVm.PropertyChanged -= OnDetailVmPropertyChanged;

        _detailVm = (DataContext as FarmingSimulatorViewModel)?.DetailVM;

        if (_detailVm != null)
            _detailVm.PropertyChanged += OnDetailVmPropertyChanged;

        SyncDocuments();
    }

    private void OnDetailVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(NoxypediaSearchViewModel.InfoDocument):
                InfoRichTextBox.Document = _detailVm?.InfoDocument ?? new FlowDocument();
                break;
            case nameof(NoxypediaSearchViewModel.DropCreepsDocument):
                DropCreepsRichTextBox.Document = _detailVm?.DropCreepsDocument ?? new FlowDocument();
                break;
        }
    }

    private void SyncDocuments()
    {
        if (InfoRichTextBox != null)
            InfoRichTextBox.Document = _detailVm?.InfoDocument ?? new FlowDocument();
        if (DropCreepsRichTextBox != null)
            DropCreepsRichTextBox.Document = _detailVm?.DropCreepsDocument ?? new FlowDocument();
    }
}
