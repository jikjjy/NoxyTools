using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NoxyTools.Wpf.Utils;

/// <summary>
/// RichTextBox.Document를 XAML 바인딩으로 교체할 수 있게 해 주는 첨부 속성입니다.
/// WPF의 RichTextBox.Document는 DependencyProperty가 아니어서 직접 바인딩이 불가합니다.
/// </summary>
public static class RichTextBoxHelper
{
    public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.RegisterAttached(
            "Document",
            typeof(FlowDocument),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(null, OnDocumentChanged));

    public static FlowDocument GetDocument(DependencyObject obj) =>
        (FlowDocument)obj.GetValue(DocumentProperty);

    public static void SetDocument(DependencyObject obj, FlowDocument value) =>
        obj.SetValue(DocumentProperty, value);

    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RichTextBox rtb)
            rtb.Document = e.NewValue as FlowDocument ?? new FlowDocument();
    }
}
