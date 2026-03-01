using Noxypedia;
using Noxypedia.Model;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public class ItemEditor : ItemEditorWindow
    {
        private static class GridColumn
        {
            public static readonly string GRADE = "Grade";
            public static readonly string NAME = "Name";
            public static readonly string RAWDATA = "Rawdata";
            public static readonly string FILTERING_SOURCE = "FilteringSource";
        }
        private readonly Model.ConfigService mConfig = Services.Config.Value;
        private readonly NoxypediaService mNoxpedia = Services.Noxypedia.Value;
        private ItemSet mSelectItem = new ItemSet();
        private DataTable mDataTable = new DataTable();

        public ItemEditor() : base()
        {
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            cbbItemGrade.Items.Clear();
            cbbItemGrade.Items.AddRange(mNoxpedia.Data.ItemGrades.Select(item => item.Name).ToArray());
            cbbCraftRecipe.Items.Clear();
            cbbCraftRecipe.Items.AddRange(mNoxpedia.Data.CraftRecipes.Select(item => item.Name).ToArray());
            cbbItemPart.Items.Clear();
            cbbItemPart.Items.AddRange(Enum.GetNames(typeof(EItemWearingPart)).ToArray());

            chkClassKnight.Tag = EClassFlags.Knight;
            chkClassWizard.Tag = EClassFlags.Wizard;
            chkClassPriest.Tag = EClassFlags.Priest;
            chkClassArcher.Tag = EClassFlags.Archer;
            chkClassDruid.Tag = EClassFlags.Druid;
            chkClassSummoner.Tag = EClassFlags.Summoner;

            mDataTable.Clear();
            mDataTable.Columns.Add(GridColumn.GRADE, typeof(string));
            mDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mDataTable.Columns.Add(GridColumn.RAWDATA, typeof(ItemSet));
            mDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));

            Utils.InitializeGridView(grdList);
            grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdList.MultiSelect = false;
            grdList.ReadOnly = true;

            grdList.DataSource = mDataTable;
            grdList.Columns[GridColumn.GRADE].HeaderText = "등급";
            grdList.Columns[GridColumn.NAME].HeaderText = "이름 (ID)";
            grdList.Columns[GridColumn.RAWDATA].Visible = false;
            grdList.Columns[GridColumn.FILTERING_SOURCE].Visible = false;

            btnAdd.ForeColor = COLOR_ADD_BUTTON;
            btnDelete.ForeColor = COLOR_REMOVE_BUTTON;
            btnEdit.ForeColor = COLOR_MODIFY_BUTTON;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            txbFilter.KeyPress += Utils.CheckKeyPressFilteringForbiddenCharacters;
            txbFilter.TextChanged += txbFilter_TextChanged;
            grdList.SelectionChanged += grdItemGrade_SelectionChanged;

            txbStrength.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbAgility.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbInteligence.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbHP.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbMP.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbAttack.KeyPress += Utils.CheckKeyPressNumberOnly;
            txbArmor.KeyPress += Utils.CheckKeyPressNumberOnly;

            txbName.Validating += txbName_Validating;
            txbStrength.Validating += txbStrength_Validating;
            txbAgility.Validating += txbAgility_Validating;
            txbInteligence.Validating += txbInteligence_Validating;
            txbHP.Validating += txbHP_Validating;
            txbMP.Validating += txbMP_Validating;
            txbAttack.Validating += txbAttack_Validating;
            txbArmor.Validating += txbArmor_Validating;
            txbUniqueOptions.Click += txbUniqueOptions_Click;
            txbCraftResult.Click += txbCraftResult_Click;

            cbbItemPart.SelectedIndexChanged += cbbItemPart_SelectedIndexChanged;
            cbbItemGrade.SelectedIndexChanged += cbbItemGrade_SelectedIndexChanged;
            cbbCraftRecipe.SelectedIndexChanged += cbbCraftRecipe_SelectedIndexChanged;

            chkUnidentifiedItem.CheckedChanged += chkUnidentifiedItem_CheckedChanged;
            chkClassKnight.CheckedChanged += chkWearableClass_CheckedChanged;
            chkClassWizard.CheckedChanged += chkWearableClass_CheckedChanged;
            chkClassPriest.CheckedChanged += chkWearableClass_CheckedChanged;
            chkClassArcher.CheckedChanged += chkWearableClass_CheckedChanged;
            chkClassDruid.CheckedChanged += chkWearableClass_CheckedChanged;
            chkClassSummoner.CheckedChanged += chkWearableClass_CheckedChanged;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnEdit.Click += btnEdit_Click;
            btnEditClipImage.Click += btnEditClipImage_Click;
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
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as ItemSet).DeepClone();
            }
            updateEditUi();
        }

        private void txbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mSelectItem.Name = txbName.Text;
            updateEditUi();
        }

        private void txbStrength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.Strength = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbAgility_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.Agility = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbInteligence_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.Inteligence = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbHP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.HP = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbMP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.MP = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbAttack_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.Attack = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbArmor_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }
            mSelectItem.Armor = string.IsNullOrWhiteSpace(control.Text) ? null : (int?)Convert.ToInt32(control.Text);
            updateEditUi();
        }

        private void txbUniqueOptions_Click(object sender, EventArgs e)
        {
            ListEditor<UniqueOptionSet>.ShowDialog(mNoxpedia.Data.UniqueOptions, mSelectItem.UniqueOptions, "고유 옵션 리스트 편집", string.Empty);
            updateEditUi();
        }

        private void txbCraftResult_Click(object sender, EventArgs e)
        {
            ListEditor<ItemSet>.ShowDialog(mNoxpedia.Data.Items.Where(item => item.Name != mSelectItem.Name).ToList(), mSelectItem.CraftDestinations, "조합 결과 리스트 편집", string.Empty);
            updateEditUi();
        }

        private void cbbItemPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectItem.Part = (EItemWearingPart)Enum.Parse(typeof(EItemWearingPart), cbbItemPart.Text);
        }

        private void cbbItemGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectItem.Grade = mNoxpedia.Data.ItemGrades.FirstOrDefault(item => item.Name == cbbItemGrade.Text);
        }

        private void cbbCraftRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectItem.CraftRecipe = mNoxpedia.Data.CraftRecipes.FirstOrDefault(item => item.Name == cbbCraftRecipe.Text);
        }

        private void chkUnidentifiedItem_CheckedChanged(object sender, EventArgs e)
        {
            mSelectItem.IsUnidentified = chkUnidentifiedItem.Checked;
            updateEditUi();
        }

        private void chkWearableClass_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            if (chk == null)
            {
                return;
            }
            var flag = (EClassFlags)chk.Tag;
            if (chk.Checked)
            {
                mSelectItem.WearableClass |= flag;
            }
            else
            {
                EClassFlags mask = ~flag;
                mSelectItem.WearableClass &= mask;
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

        private void btnEditClipImage_Click(object sender, EventArgs e)
        {
            ClipImageEditor.ShowDialog(mSelectItem.ClipImages, ClipImageKeys.Item.AllKeys);
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
                mSelectItem = (grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as ItemSet).DeepClone();
            }
            updateEditUi();
        }

        private void commandModifyItem()
        {
            using (var dialog = new UserInput("정보 확인 버전 입력", mSelectItem.CheckVersion))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                mSelectItem.CheckVersion = dialog.UserInputText;
            }
            if (mNoxpedia.Data.ModifyItem(mSelectItem) == false)
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
            if (mNoxpedia.Data.Items.Any(item => item.Name == mSelectItem.Name) == false)
            {
                MessageBox.Show("일치하는 정보가 없어서 삭제 할 수 없습니다.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("데이터를 삭제하면 다른 데이터에 영향을 줄 수 있습니다.\n삭제 하시겠습니까?", "Noxypedia Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            mNoxpedia.Data.DeleteItem(mSelectItem);
            updateList();
            mNoxpedia.RasieDataChangedEvent();
        }

        private void commandAddItem()
        {
            using (var dialog = new UserInput("정보 확인 버전 입력", mSelectItem.CheckVersion))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                mSelectItem.CheckVersion = dialog.UserInputText;
            }
            if (mNoxpedia.Data.AddItem(mSelectItem) == false)
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

            if (mNoxpedia.Data.Items.Any(item => item.Name == mSelectItem.Name) == true)
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
            pnlBase.SplitterDistance = mConfig.ItemEditor.WindowSplitterDistance;
        }

        private void saveUiSetting()
        {
            mConfig.ItemEditor.WindowSplitterDistance = pnlBase.SplitterDistance;
        }

        private void updateEditUi()
        {
            StringBuilder sb = new();
            cbbItemGrade.Items.Clear();
            cbbItemGrade.Items.AddRange(mNoxpedia.Data.ItemGrades.Select(item => item.Name).ToArray());
            cbbCraftRecipe.Items.Clear();
            cbbCraftRecipe.Items.AddRange(mNoxpedia.Data.CraftRecipes.Select(item => item.Name).ToArray());

            lblVersionInfo.Text = mSelectItem.ToHistoryString();
            txbName.Text = mSelectItem.Name;
            cbbItemPart.SelectedIndex = (int)mSelectItem.Part;
            cbbItemGrade.SelectedItem = mSelectItem.Grade?.Name;
            cbbCraftRecipe.SelectedItem = mSelectItem.CraftRecipe?.Name;
            txbStrength.Text = mSelectItem.Strength.HasValue ? mSelectItem.Strength.Value.ToString() : string.Empty;
            txbAgility.Text = mSelectItem.Agility.HasValue ? mSelectItem.Agility.Value.ToString() : string.Empty;
            txbInteligence.Text = mSelectItem.Inteligence.HasValue ? mSelectItem.Inteligence.Value.ToString() : string.Empty;
            txbHP.Text = mSelectItem.HP.HasValue ? mSelectItem.HP.Value.ToString() : string.Empty;
            txbMP.Text = mSelectItem.MP.HasValue ? mSelectItem.MP.Value.ToString() : string.Empty;
            txbAttack.Text = mSelectItem.Attack.HasValue ? mSelectItem.Attack.Value.ToString() : string.Empty;
            txbArmor.Text = mSelectItem.Armor.HasValue ? mSelectItem.Armor.Value.ToString() : string.Empty;
            chkClassKnight.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Knight);
            chkClassWizard.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Wizard);
            chkClassPriest.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Priest);
            chkClassArcher.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Archer);
            chkClassDruid.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Druid);
            chkClassSummoner.Checked = mSelectItem.WearableClass.HasFlag(EClassFlags.Summoner);
            chkUnidentifiedItem.Checked = mSelectItem.IsUnidentified;
            sb.Clear();
            foreach (var item in mSelectItem.UniqueOptions)
            {
                sb.AppendLine($"[{item.Name}]  {item.EffectDescription}");
            }
            txbUniqueOptions.Text = sb.ToString();
            sb.Clear();
            foreach (var item in mSelectItem.CraftDestinations)
            {
                sb.AppendLine($"[{item.Grade.Name}]  {item.Name}");
            }
            txbCraftResult.Text = sb.ToString();
            sb.Clear();
            if (mSelectItem.CraftRecipe == null)
            {
                mSelectItem.CraftRecipe = new CraftRecipeSet();
            }
            for (int i = 0; i < mSelectItem.CraftRecipe.Materials.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append($" + ");
                }
                sb.Append($"{mSelectItem.CraftRecipe.Materials[i].Name}");
            }
            lblCraftRecipePreview.Text = sb.ToString();

            bool existItem = mNoxpedia.Data.Items.Any(item => item.Name == mSelectItem.Name);
            btnAdd.Enabled = existItem == false;
            btnDelete.Enabled = existItem == true;
            btnEdit.Enabled = existItem == true;

            pnlItemOptions.Visible = !mSelectItem.IsUnidentified;
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
            foreach (var item in mNoxpedia.Data.Items)
            {
                string gradeName = string.Empty;
                if (item.Grade != null)
                {
                    gradeName = item.Grade.Name;
                }
                mDataTable.Rows.Add(gradeName, item.Name, item, item.ToFilteringSource());
            }

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
