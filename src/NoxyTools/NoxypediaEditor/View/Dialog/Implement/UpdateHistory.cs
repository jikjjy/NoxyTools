using Noxypedia;
using Noxypedia.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public class UpdateHistory : UpdateHistoryDialog
    {
        private static class GridColumn
        {
            public static readonly string VERSION = "Version";
            public static readonly string AUTHER = "Auther";
            public static readonly string UPDATE_TIME = "UpdateTime";
            public static readonly string RAWDATA = "Rawdata";
        }
        private readonly NoxypediaService mNoxpedia = Services.Noxypedia.Value;
        private DataTable mDataTable = new DataTable();
        private NoxypediaHistory mSelectItem = new NoxypediaHistory();
        private static NoxypediaHistory[] mHistoryData;

        public UpdateHistory() : base()
        {
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            mDataTable.Clear();
            mDataTable.Columns.Add(GridColumn.VERSION, typeof(string));
            mDataTable.Columns.Add(GridColumn.AUTHER, typeof(string));
            mDataTable.Columns.Add(GridColumn.UPDATE_TIME, typeof(DateTime));
            mDataTable.Columns.Add(GridColumn.RAWDATA, typeof(NoxypediaHistory));

            Utils.InitializeGridView(grdList);
            grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdList.MultiSelect = false;
            grdList.ReadOnly = true;

            grdList.DataSource = mDataTable;
            grdList.Columns[GridColumn.VERSION].HeaderText = "버전";
            grdList.Columns[GridColumn.AUTHER].HeaderText = "수정자";
            grdList.Columns[GridColumn.UPDATE_TIME].HeaderText = "수정 시간";
            grdList.Columns[GridColumn.RAWDATA].Visible = false;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            grdList.SelectionChanged += grdList_SelectionChanged;
        }

        private void form_Load(object sender, EventArgs e)
        {
        }

        private void form_Shown(object sender, EventArgs e)
        {
            // DynamoDB 제거 — 히스토리 데이터 미사용
            if (mHistoryData == null)
            {
                mHistoryData = new NoxypediaHistory[0];
            }
            updateList();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void grdList_SelectionChanged(object sender, EventArgs e)
        {
            if (grdList.SelectedRows.Count > 0)
            {
                mSelectItem = grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as NoxypediaHistory;
            }
            updateEditUi();
        }

        private void updateEditUi()
        {
            if (mSelectItem is null)
            {
                return;
            }

            lblVersion.Text = mSelectItem.Range;
            lblAuther.Text = mSelectItem.Auther;
            lblDate.Text = mSelectItem.UpdateDate.ToLocalTime().ToString();
            txbComment.Text = mSelectItem.Comment;
        }

        private void updateList()
        {
            mDataTable.Rows.Clear();
            foreach (var item in mHistoryData)
            {
                mDataTable.Rows.Add(item.Range, item.Auther, item.UpdateDate, item);
            }

            if (grdList.Rows.Count == 0)
            {
                updateEditUi();
            }

            grdList.Columns[GridColumn.VERSION].Width = 60;
        }
    }
}
