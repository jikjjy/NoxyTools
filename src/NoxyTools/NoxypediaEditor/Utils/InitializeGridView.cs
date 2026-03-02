namespace NoxypediaEditor
{
    public static partial class Utils
    {
        /// <summary>
        /// 그리드 뷰 기본 스타일 초기화
        /// 설명 : 그리드 뷰 생성 시 기본 제약 조건으로 사용합니다. 해당 부분이 수정되면 다른 부분에도 영향이 갑니다.
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        public static bool InitializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 배경색
                objGridView.BackgroundColor = Color.DimGray;
                // 그리드 뷰 칼럼 사이즈 조정
                objGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // 그리드 뷰 행, 열 사이즈 유저 조정 막음
                objGridView.AllowUserToResizeRows = false;
                objGridView.AllowUserToResizeColumns = false;
                // 그리드 뷰 행 머리글 없앰
                objGridView.RowHeadersVisible = false;
                // 그리드 뷰 홀수행 색 변경
                objGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(51, 51, 55);
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
                // 마지막 행 제거
                objGridView.AllowUserToAddRows = false;
                // 그리드 선택 색 변경
                objGridView.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                objGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
                objGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 101, 179);
                objGridView.UserDeletingRow += ObjGridView_UserDeletingRow;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        private static void ObjGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
