using Noxypedia;
using Noxypedia.Model;
using System.Data;
using System.Text;

namespace NoxypediaEditor
{
    public class CraftRecipeEditor : CraftRecipeEditorWindow
    {
        private static class GridColumn
        {
            public static readonly string NAME = "Name";
            public static readonly string RECIPE_PREVIEW = "RecipePreview";
            public static readonly string RAWDATA = "Rawdata";
            public static readonly string FILTERING_SOURCE = "FilteringSource";
        }
        private readonly Model.ConfigService mConfig = Services.Config.Value;
        private readonly NoxypediaService mNoxpedia = Services.Noxypedia.Value;
        private CraftRecipeSet mSelectItem = new CraftRecipeSet();
        private DataTable mDataTable = new DataTable();

        public CraftRecipeEditor() : base()
        {
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            cbbLocation.Items.Clear();
            cbbLocation.Items.AddRange(mNoxpedia.Data.Locations.Select(item => item.Name).ToArray());

            mDataTable.Clear();
            mDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mDataTable.Columns.Add(GridColumn.RECIPE_PREVIEW, typeof(string));
            mDataTable.Columns.Add(GridColumn.RAWDATA, typeof(CraftRecipeSet));
            mDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));

            Utils.InitializeGridView(grdList);
            grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdList.MultiSelect = false;
            grdList.ReadOnly = true;

            grdList.DataSource = mDataTable;
            grdList.Columns[GridColumn.NAME].HeaderText = "이름 (ID)";
            grdList.Columns[GridColumn.RECIPE_PREVIEW].HeaderText = "레시피";
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
            txbSuccessProbability.KeyPress += Utils.CheckKeyPressNumberOnly;

            txbName.Validating += txbName_Validating;
            txbDescription.Validating += txbDescription_Validating;
            txbSuccessProbability.Validating += txbSuccessProbability_Validating;
            cbbLocation.SelectedIndexChanged += cbbLocation_SelectedIndexChanged;
            txbMaterials.Click += txbMaterials_Click;
            txbSubstituteMaterial.Click += txbSubstituteMaterial_Click;

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
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as CraftRecipeSet).DeepClone();
            }
            updateEditUi();
        }

        private void txbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mSelectItem.Name = txbName.Text;
            updateEditUi();
        }

        private void txbDescription_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mSelectItem.Description = txbDescription.Text;
            updateEditUi();
        }

        private void txbSuccessProbability_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            float getValue = 0f;
            if (string.IsNullOrWhiteSpace(txbSuccessProbability.Text) == false)
            {
                getValue = Convert.ToSingle(txbSuccessProbability.Text);
            }
            mSelectItem.SuccessProbability = getValue <= 0f || getValue >= 100f ? null : (float?)getValue;
            updateEditUi();
        }

        private void cbbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectItem.Location = mNoxpedia.Data.Locations.FirstOrDefault(item => item.Name == cbbLocation.Text);
        }

        private void txbMaterials_Click(object sender, EventArgs e)
        {
            int maxCount = 5 - mSelectItem.SubstituteMaterials.Count;
            ListEditor<ItemSet>.ShowDialog(mNoxpedia.Data.Items.ToList(), mSelectItem.Materials, "재료 리스트 편집", string.Empty, maxCount);
            updateEditUi();
        }

        private void txbSubstituteMaterial_Click(object sender, EventArgs e)
        {
            if (mSelectItem.SubstituteMaterials.Count == 0)
            {
                mSelectItem.SubstituteMaterials.Add(new List<ItemSet>());
            }
            ListEditor<ItemSet>.ShowDialog(mNoxpedia.Data.Items.ToList(), mSelectItem.SubstituteMaterials.First(), "대체 가능한 재료 리스트 편집", string.Empty);
            if (mSelectItem.SubstituteMaterials.First().Count == 0)
            {
                mSelectItem.SubstituteMaterials.RemoveAt(0);
            }
            updateEditUi();
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
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as CraftRecipeSet).DeepClone();
            }
            updateEditUi();
        }

        private void commandModifyItem()
        {
            if (mNoxpedia.Data.ModifyCraftRecipe(mSelectItem) == false)
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
            if (mNoxpedia.Data.CraftRecipes.Any(item => item.Name == mSelectItem.Name) == false)
            {
                MessageBox.Show("일치하는 정보가 없어서 삭제 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("데이터를 삭제하면 다른 데이터에 영향을 줄 수 있습니다.\n삭제 하시겠습니까?", "Noxypedia Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            mNoxpedia.Data.DeleteCraftRecipe(mSelectItem);
            updateList();
            mNoxpedia.RasieDataChangedEvent();
        }

        private void commandAddItem()
        {
            if (mNoxpedia.Data.AddCraftRecipe(mSelectItem) == false)
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

            if (mNoxpedia.Data.CraftRecipes.Any(item => item.Name == mSelectItem.Name) == true)
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
            pnlBase.SplitterDistance = mConfig.CraftRecipeEditor.WindowSplitterDistance;
        }

        private void saveUiSetting()
        {
            mConfig.CraftRecipeEditor.WindowSplitterDistance = pnlBase.SplitterDistance;
        }

        private void updateEditUi()
        {
            cbbLocation.Items.Clear();
            cbbLocation.Items.AddRange(mNoxpedia.Data.Locations.Select(item => item.Name).ToArray());

            StringBuilder sb = new StringBuilder();
            lblVersionInfo.Text = mSelectItem.ToHistoryString();
            txbName.Text = mSelectItem.Name;
            txbDescription.Text = mSelectItem.Description;
            txbSuccessProbability.Text = mSelectItem.SuccessProbability.HasValue ? mSelectItem.SuccessProbability.Value.ToString() : string.Empty;
            sb.Clear();
            foreach (var item in mSelectItem.Materials)
            {
                sb.AppendLine($"[{item.Name}]");
            }
            txbMaterials.Text = sb.ToString();
            cbbLocation.SelectedItem = mSelectItem.Location?.Name;
            txbSubstituteMaterial.Visible = mSelectItem.Materials.Count != 5;
            if (
                mSelectItem.SubstituteMaterials != null
                && mSelectItem.SubstituteMaterials.FirstOrDefault() != null
                )
            {
                txbSubstituteMaterial.Text = string.Join($"{Environment.NewLine}or{Environment.NewLine}", mSelectItem.SubstituteMaterials.FirstOrDefault().Select(item => $"[{item.Name}]"));
            }
            else
            {
                txbSubstituteMaterial.Text = string.Empty;
            }

            bool existItem = mNoxpedia.Data.CraftRecipes.Any(item => item.Name == mSelectItem.Name);
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
            StringBuilder sb = new StringBuilder();
            foreach (var item in mNoxpedia.Data.CraftRecipes)
            {
                sb.Clear();
                for (int i = 0; i < item.Materials.Count; i++)
                {
                    if (i != 0)
                    {
                        sb.Append($" + ");
                    }
                    sb.Append($"{item.Materials[i].Name}");
                }
                mDataTable.Rows.Add(item.Name, sb.ToString(), item, item.ToFilteringSource());
            }

            grdList.Columns[GridColumn.NAME].Width = 120;

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
