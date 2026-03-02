using Noxypedia.Model;
using System.Data;

namespace NoxypediaEditor
{
    public class ListEditor<TDataSet> : ListEditorDialog
        where TDataSet : BaseModel
    {
        private static class GridColumn
        {
            public static readonly string NAME = "Name";
            public static readonly string RAWDATA = "Rawdata";
            public static readonly string FILTERING_SOURCE = "FilteringSource";
        }
        private List<TDataSet> mLeftList;
        private List<TDataSet> mRightList;
        private DataTable mLeftDataTable = new DataTable();
        private DataTable mRightDataTable = new DataTable();
        private int mMaxCount;

        private ListEditor() : base()
        {
            initializeComponent();
            registEvent();
        }

        public static DialogResult ShowDialog(List<TDataSet> leftData, List<TDataSet> rightData, string leftTitle, string rightTitle, int maxCount = 0)
        {
            using (var instance = new ListEditor<TDataSet>())
            {
                instance.mMaxCount = maxCount;
                instance.mLeftList = leftData;
                instance.mRightList = rightData;
                instance.lblTitleLeft.Text = leftTitle;
                instance.lblTitleRight.Text = rightTitle;
                return instance.ShowDialog();
            }
        }

        private void initializeComponent()
        {
            mLeftDataTable.Clear();
            mLeftDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mLeftDataTable.Columns.Add(GridColumn.RAWDATA, typeof(TDataSet));
            mLeftDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));
            mRightDataTable.Clear();
            mRightDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mRightDataTable.Columns.Add(GridColumn.RAWDATA, typeof(TDataSet));
            mRightDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));

            Utils.InitializeGridView(grdLeft);
            grdLeft.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdLeft.MultiSelect = false;
            grdLeft.ReadOnly = true;
            Utils.InitializeGridView(grdRight);
            grdRight.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdRight.MultiSelect = false;
            grdRight.ReadOnly = true;

            grdLeft.DataSource = mLeftDataTable;
            grdLeft.Columns[GridColumn.NAME].HeaderText = "이름 (ID)";
            grdLeft.Columns[GridColumn.RAWDATA].Visible = false;
            grdLeft.Columns[GridColumn.FILTERING_SOURCE].Visible = false;
            grdRight.DataSource = mRightDataTable;
            grdRight.Columns[GridColumn.NAME].HeaderText = "이름 (ID)";
            grdRight.Columns[GridColumn.RAWDATA].Visible = false;
            grdRight.Columns[GridColumn.FILTERING_SOURCE].Visible = false;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            txbLeftFilter.TextChanged += txbLeftFilter_TextChanged;
            txbRightFilter.TextChanged += txbRightFilter_TextChanged;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnOk.Click += btnOk_Click;
        }

        private void form_Load(object sender, EventArgs e)
        {
        }

        private void form_Shown(object sender, EventArgs e)
        {
            updateList();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void txbLeftFilter_TextChanged(object sender, EventArgs e)
        {
            mLeftDataTable.DefaultView.RowFilter = $"[{GridColumn.FILTERING_SOURCE}] LIKE '%{txbLeftFilter.Text}%'";
        }

        private void txbRightFilter_TextChanged(object sender, EventArgs e)
        {
            mRightDataTable.DefaultView.RowFilter = $"[{GridColumn.FILTERING_SOURCE}] LIKE '%{txbRightFilter.Text}%'";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (grdLeft.SelectedRows.Count == 0)
            {
                MessageBox.Show("추가 할 항목을 선택 해주세요.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            TDataSet addItem = grdLeft.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as TDataSet;
            if (mRightList.Any(item => item.Name == addItem.Name) == true)
            {
                MessageBox.Show("선택한 항목이 이미 리스트에 존재합니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (
                mMaxCount > 0
                && mRightList.Count + 1 > mMaxCount
                )
            {
                MessageBox.Show($"{mMaxCount}개 이상 추가 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mRightList.Add(addItem);
            updateList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdRight.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제 할 항목을 선택 해주세요.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            TDataSet removeItem = grdRight.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as TDataSet;
            int removeIndex = mRightList.FindIndex(item => item.Name == removeItem.Name);
            if (removeIndex < 0)
            {
                MessageBox.Show("항목이 이미 제거됐습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mRightList.RemoveAt(removeIndex);
            updateList();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void updateList()
        {
            using (var recoverLeft = new Utils.DataGridViewSelectionRecovery(grdLeft))
            using (var recoverRight = new Utils.DataGridViewSelectionRecovery(grdRight))
            {
                mRightDataTable.Rows.Clear();
                foreach (var item in mRightList)
                {
                    mRightDataTable.Rows.Add(item.Name, item, item.ToFilteringSource());
                }
                mLeftDataTable.Rows.Clear();
                foreach (var item in mLeftList)
                {
                    if (mRightList.Any(right => right.Name == item.Name) == true)
                    {
                        continue;
                    }
                    mLeftDataTable.Rows.Add(item.Name, item, item.ToFilteringSource());
                }
            }
        }
    }
}
