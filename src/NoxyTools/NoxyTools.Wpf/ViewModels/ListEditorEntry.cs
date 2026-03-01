namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// ListEditorViewModel에서 사용하는 항목 래퍼.
/// DisplayName은 화면에 표시되는 텍스트, RawData는 원본 객체입니다.
/// </summary>
public sealed record ListEditorEntry(string DisplayName, object RawData);
