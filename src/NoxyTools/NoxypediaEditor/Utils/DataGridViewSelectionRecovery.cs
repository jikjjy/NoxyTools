using System;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public static partial class Utils
    {
        public class DataGridViewSelectionRecovery : IDisposable
        {
            private bool disposedValue;
            private readonly DataGridView mDataGridView;
            private readonly int mLastDisplayRowIndex;
            private readonly int mLastSelectRowIndex;

            public DataGridViewSelectionRecovery(DataGridView dataGridView)
            {
                mDataGridView = dataGridView;
                mLastDisplayRowIndex = mDataGridView.FirstDisplayedScrollingRowIndex;
                mLastSelectRowIndex = mDataGridView.SelectedRows.Count > 0 ? mDataGridView.SelectedRows[0].Index : -1;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        gridSelectionRecovery(mDataGridView, mLastSelectRowIndex, mLastDisplayRowIndex);
                    }

                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            private void gridSelectionRecovery(DataGridView grid, int lastIndex, int lastDisplayIndex)
            {
                int rowCount = grid.Rows.Count;
                if (rowCount == 0)
                {
                    return;
                }
                if (lastIndex < 0)
                {
                    return;
                }
                if (lastIndex >= rowCount)
                {
                    lastIndex = rowCount - 1;
                }
                grid.Rows[lastIndex].Selected = true;
                grid.FirstDisplayedScrollingRowIndex = lastDisplayIndex;
            }
        }
    }
}
