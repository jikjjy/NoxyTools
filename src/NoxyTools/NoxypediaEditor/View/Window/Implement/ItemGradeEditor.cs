using Noxypedia;
using Noxypedia.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public class ItemGradeEditor : ItemGradeEditorWindow
    {
        private static class GridColumn
        {
            public static readonly string COLOR_PREVIEW = "ColorPreview";
            public static readonly string GRADE = "Grade";
            public static readonly string NAME = "Name";
            public static readonly string RAWDATA = "Rawdata";
            public static readonly string FILTERING_SOURCE = "FilteringSource";
        }
        private readonly Model.ConfigService mConfig = Services.Config.Value;
        private readonly NoxypediaService mNoxpedia = Services.Noxypedia.Value;
        private ItemGradeSet mSelectItem = new ItemGradeSet();
        private DataTable mDataTable = new DataTable();

        public ItemGradeEditor() : base()
        {
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            mDataTable.Clear();
            mDataTable.Columns.Add(GridColumn.COLOR_PREVIEW, typeof(Color));
            mDataTable.Columns.Add(GridColumn.GRADE, typeof(string));
            mDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mDataTable.Columns.Add(GridColumn.RAWDATA, typeof(ItemGradeSet));
            mDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));

            Utils.InitializeGridView(grdList);
            grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdList.MultiSelect = false;
            grdList.ReadOnly = true;

            grdList.DataSource = mDataTable;
            grdList.Columns[GridColumn.COLOR_PREVIEW].HeaderText = string.Empty;
            grdList.Columns[GridColumn.GRADE].HeaderText = "등급";
            grdList.Columns[GridColumn.NAME].HeaderText = "이름 (ID)";
            grdList.Columns[GridColumn.RAWDATA].Visible = false;
            grdList.Columns[GridColumn.FILTERING_SOURCE].Visible = false;

            btnAdd.ForeColor = COLOR_ADD_BUTTON;
            btnDelete.ForeColor = COLOR_REMOVE_BUTTON;
            btnEdit.ForeColor = COLOR_MODIFY_BUTTON;
            btnEditClipImage.Visible = false;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            txbFilter.KeyPress += Utils.CheckKeyPressFilteringForbiddenCharacters;
            txbFilter.TextChanged += txbFilter_TextChanged;
            grdList.SelectionChanged += grdItemGrade_SelectionChanged;
            grdList.CellPainting += grdItemGrade_CellPainting;

            txbName.Validating += txbName_Validating;
            txbGradeIndex.Validating += txbGradeIndex_Validating;
            txbGradeIndex.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbColorCode.Validating += txbColorCode_Validating;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnEdit.Click += btnEdit_Click;
            btnEditRename.Click += btnEditRename_Click;

            mNoxpedia.InformationChanged += noxpedia_InformationChanged;
            mNoxpedia.DataChanged += noxpedia_DataChanged;
        }

        private void form_Load(object sender, EventArgs e)
        {
        }

        private void form_Shown(object sender, EventArgs e)
        {
            loadUiSetting();
            updateList();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveUiSetting();
        }

        private void txbFilter_TextChanged(object sender, EventArgs e)
        {
            mDataTable.DefaultView.RowFilter = $"[{GridColumn.FILTERING_SOURCE}] LIKE '%{txbFilter.Text}%'";
        }

        private void grdItemGrade_SelectionChanged(object sender, EventArgs e)
        {
            if (grdList.SelectedRows.Count > 0)
            {
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as ItemGradeSet).DeepClone();
            }
            updateEditUi();
        }

        private void grdItemGrade_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.Value is Color)
            {
                Color setColor = Color.FromArgb(0xFF, (Color)e.Value);
                e.CellStyle.ForeColor
                    = e.CellStyle.BackColor
                    = e.CellStyle.SelectionBackColor
                    = e.CellStyle.SelectionForeColor
                    = setColor;
            }
        }

        private void txbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mSelectItem.Name = txbName.Text;
            updateEditUi();
        }

        private void txbGradeIndex_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                int result = Convert.ToInt32(txbGradeIndex.Text);
                mSelectItem.GradeOrder = result;
                updateEditUi();
            }
            catch
            {
                MessageBox.Show("값을 숫자로 변환 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txbColorCode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                int result = Convert.ToInt32(txbColorCode.Text, 16);
                mSelectItem.Color = Color.FromArgb(result);
                updateEditUi();
            }
            catch
            {
                MessageBox.Show("색깔로 변환 할 수 없습니다.\n(세이브 코드에 있는 색깔 코드를 입력해 주세요.)", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            commandAddItem();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            commandDeleteItem();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            commandModifyItem();
        }

        private void btnEditRename_Click(object sender, EventArgs e)
        {
            commandRenameItem();
        }

        private void noxpedia_InformationChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void noxpedia_DataChanged(object sender, EventArgs e)
        {
            if (grdList.SelectedRows.Count > 0)
            {
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as ItemGradeSet).DeepClone();
            }
            updateEditUi();
        }

        private void commandModifyItem()
        {
            if (mNoxpedia.Data.ModifyItemGrade(mSelectItem) == false)
            {
                txbName.Focus();
                txbName.SelectAll();
                MessageBox.Show("이름이 존재하지 않아 변경 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            updateList();
            mNoxpedia.RasieDataChangedEvent();
        }

        private void commandDeleteItem()
        {
            if (mNoxpedia.Data.ItemGrades.Any(item => item.Name == mSelectItem.Name) == false)
            {
                MessageBox.Show("일치하는 정보가 없어서 삭제 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("데이터를 삭제하면 다른 데이터에 영향을 줄 수 있습니다.\n삭제 하시겠습니까?", "Noxypedia Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            mNoxpedia.Data.DeleteItemGrade(mSelectItem);
            updateList();
            mNoxpedia.RasieDataChangedEvent();
        }

        private void commandAddItem()
        {
            if (mNoxpedia.Data.AddItemGrade(mSelectItem) == false)
            {
                txbName.Focus();
                txbName.SelectAll();
                MessageBox.Show("중복된 이름이 존재하여 추가 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            updateList();
            mNoxpedia.RasieDataChangedEvent();
        }

        private void commandRenameItem()
        {
            if (grdList.SelectedRows.Count == 0)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(mSelectItem.Name) == true)
            {
                return;
            }

            if (mNoxpedia.Data.ItemGrades.Any(item => item.Name == mSelectItem.Name) == true)
            {
                MessageBox.Show("중복된 이름이 존재하여 변경 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var originalData = grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as BaseModel;
            if (MessageBox.Show($"이름(ID)을 변경하시겠습니까?{Environment.NewLine}{Environment.NewLine}'{originalData.Name}' → '{mSelectItem.Name}' ", "Noxypedia Editor", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            mNoxpedia.Data.RenameData(originalData, mSelectItem.Name);

            updateList(mSelectItem.Name);
            mNoxpedia.RasieDataChangedEvent();
        }

        private void loadUiSetting()
        {
            pnlBase.SplitterDistance = mConfig.ItemGradeEditor.WindowSplitterDistance;
        }

        private void saveUiSetting()
        {
            mConfig.ItemGradeEditor.WindowSplitterDistance = pnlBase.SplitterDistance;
        }

        private void updateEditUi()
        {
            lblVersionInfo.Text = mSelectItem.ToHistoryString();
            txbName.Text = mSelectItem.Name;
            txbGradeIndex.Text = mSelectItem.GradeOrder.ToString();
            txbColorCode.Text = $"{mSelectItem.Color.ToArgb():X8}";
            pnlColorCodePreview.BackColor = Color.FromArgb(0xFF, mSelectItem.Color);

            bool existItem = mNoxpedia.Data.ItemGrades.Any(item => item.Name == mSelectItem.Name);
            btnAdd.Enabled = existItem == false;
            btnDelete.Enabled = existItem == true;
            btnEdit.Enabled = existItem == true;
        }

        private void updateList(string recoveryItemName = "")
        {
            string lastSelectItemName = mSelectItem.Name;
            if (string.IsNullOrEmpty(recoveryItemName) == false)
            {
                lastSelectItemName = recoveryItemName;
            }
            int lastFirstDisplayedScrollingRowIndex = grdList.FirstDisplayedScrollingRowIndex;

            mDataTable.Rows.Clear();
            foreach (var item in mNoxpedia.Data.ItemGrades)
            {
                mDataTable.Rows.Add(item.Color, item.GradeOrder, item.Name, item, item.ToFilteringSource());
            }

            grdList.Columns[GridColumn.COLOR_PREVIEW].Width = 45;
            grdList.Columns[GridColumn.GRADE].Width = 80;

            if (grdList.Rows.Count == 0)
            {
                updateEditUi();
            }

            foreach (DataGridViewRow row in grdList.Rows)
            {
                if (Convert.ToString(row.Cells[GridColumn.NAME].Value) == lastSelectItemName)
                {
                    grdList.FirstDisplayedScrollingRowIndex = lastFirstDisplayedScrollingRowIndex < 0 ? row.Index : lastFirstDisplayedScrollingRowIndex;
                    row.Selected = true;
                    break;
                }
            }
        }
    }
}
