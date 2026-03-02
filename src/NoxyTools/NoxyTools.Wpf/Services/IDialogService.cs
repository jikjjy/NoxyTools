using Noxypedia.Model;

namespace NoxyTools.Wpf.Services;

/// <summary>
/// 파일/폴더 다이얼로그 및 메시지 박스를 ViewModel에서 처리하기 위한 추상화.
/// </summary>
public interface IDialogService
{
    /// <summary>파일 열기 다이얼로그. 선택된 경로 또는 null 반환.</summary>
    string? ShowOpenFileDialog(string title = "파일 열기", string filter = "All Files|*.*");

    /// <summary>파일 저장 다이얼로그. 선택된 경로 또는 null 반환.</summary>
    string? ShowSaveFileDialog(string title = "파일 저장", string filter = "All Files|*.*", string defaultFileName = "");

    /// <summary>폴더 선택 다이얼로그. 선택된 경로 또는 null 반환.</summary>
    string? ShowOpenFolderDialog(string title = "폴더 선택", string? initialPath = null);

    /// <summary>메시지 박스 표시.</summary>
    bool ShowMessageBox(string message, string title = "알림", bool isYesNo = false);

    /// <summary>
    /// 좌/우 목록 편집 다이얼로그를 표시합니다.
    /// 확인 시 오른쪽 목록의 원본 데이터를 반환하고, 취소 시 null을 반환합니다.
    /// </summary>
    /// <typeparam name="TData">목록 항목의 원본 데이터 타입. class 타입이어야 합니다.</typeparam>
    /// <param name="leftData">왼쪽(전체) 목록 데이터.</param>
    /// <param name="rightData">오른쪽(선택) 목록 초기 데이터.</param>
    /// <param name="leftTitle">왼쪽 목록 제목.</param>
    /// <param name="rightTitle">오른쪽 목록 제목.</param>
    /// <param name="displayName">항목 표시 이름 변환 함수. null이면 ToString() 사용.</param>
    /// <param name="maxCount">오른쪽 목록 최대 개수. 0이면 제한 없음.</param>
    IReadOnlyList<TData>? ShowListEditorDialog<TData>(
        IEnumerable<TData> leftData,
        IEnumerable<TData> rightData,
        string leftTitle = "전체 목록",
        string rightTitle = "선택 목록",
        Func<TData, string>? displayName = null,
        int maxCount = 0) where TData : class;

    /// <summary>
    /// 아이템 선택 다이얼로그를 표시합니다.
    /// 확인 시 선택된 아이템을 반환하고, 취소 시 null을 반환합니다.
    /// </summary>
    ItemSet? ShowSelectItemDialog(IEnumerable<ItemSet> items);
}
