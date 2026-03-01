using System.Windows;

namespace NoxyTools.Wpf.Services;

public class ClipboardService : IClipboardService
{
    public void SetText(string text)
    {
        Clipboard.SetText(text);
    }

    public void SetRtf(string rtf)
    {
        // SetData(DataFormats.Rtf, string)은 클립보드에 RTF 형식으로 등록되지 않음.
        // SetText + TextDataFormat.Rtf 를 사용해야 메모장 등에서 붙여넣기가 활성화됨.
        Clipboard.SetText(rtf, TextDataFormat.Rtf);
    }
}
