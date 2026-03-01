using NoxyTools.Wpf.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NoxyTools.Wpf.Views;

public partial class MakeValidReportView : UserControl
{
    public MakeValidReportView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is MakeValidReportViewModel oldVm)
        {
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            oldVm.CopyRequested   -= OnCopyRequested;
        }

        if (e.NewValue is MakeValidReportViewModel newVm)
        {
            newVm.PropertyChanged += OnViewModelPropertyChanged;
            newVm.CopyRequested   += OnCopyRequested;
            newVm.RequestDocumentRefresh();
        }
    }

    private void OnCopyRequested(object? sender, EventArgs e)
    {
        if (sender is not MakeValidReportViewModel vm) return;

        // Step 1: RichTextBox 내장 Copy() → RTF + PlainText 클립보드 등록
        PreviewRichTextBox.SelectAll();
        PreviewRichTextBox.Copy();

        // Step 2: CF_HTML 형식 추가 (네이버 카페 등 웹 에디터는 HTML 형식 우선 사용)
        var data = new DataObject();
        object? rtf = Clipboard.GetData(DataFormats.Rtf);
        if (rtf != null) data.SetData(DataFormats.Rtf, rtf);
        if (Clipboard.ContainsText()) data.SetText(Clipboard.GetText());
        data.SetData(DataFormats.Html, vm.GetClipboardHtml());
        Clipboard.SetDataObject(data, true);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MakeValidReportViewModel.PreviewDocument)
            && sender is MakeValidReportViewModel vm
            && vm.PreviewDocument != null)
        {
            PreviewRichTextBox.Document = vm.PreviewDocument;
        }
    }

    private void SavePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox tb) return;
        tb.CaretIndex = tb.Text.Length;
        if (tb.Template?.FindName("PART_ContentHost", tb) is ScrollViewer sv)
            sv.ScrollToRightEnd();
    }

    private void CafeHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }
}

