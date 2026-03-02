using Noxypedia;
using Noxypedia.Utils;
using System.Diagnostics;
using System.Reflection;

namespace NoxypediaEditor
{
    public class Main : MainWindow
    {
        private readonly Model.ConfigService mConfig = Services.Config.Value;
        private readonly NoxypediaService mNoxpedia = Services.Noxypedia.Value;
        private readonly Dictionary<EView, Form> mSubForms = new Dictionary<EView, Form>();
        private enum EView
        {
            ItemGradeEditor,
            UniqueOptionEditor,
            ItemEditor,
            RegionEditor,
            CreepEditor,
            LocationEditor,
            CraftRecipeEditor
        }

        public Main() : base()
        {
            initializeControl();
            registTag();
            registEvent();
        }

        private void initializeControl()
        {
            btnSelectItemGrade.Checked = true;
            TopMost
                = chkTopMost.Checked
                = mConfig.TopMost;
        }

        private void registTag()
        {
            btnSelectItemGrade.Tag = EView.ItemGradeEditor;
            btnSelectUniqueOption.Tag = EView.UniqueOptionEditor;
            btnSelectItem.Tag = EView.ItemEditor;
            btnSelectRegion.Tag = EView.RegionEditor;
            btnSelectCreep.Tag = EView.CreepEditor;
            btnSelectLocation.Tag = EView.LocationEditor;
            btnSelectCraftRecipe.Tag = EView.CraftRecipeEditor;
        }

        private void registEvent()
        {
            Load += new EventHandler(form_Load);
            FormClosing += new FormClosingEventHandler(form_FormClosing);

            btnSelectItemGrade.Click += btnSelect_Click;
            btnSelectUniqueOption.Click += btnSelect_Click;
            btnSelectItem.Click += btnSelect_Click;
            btnSelectRegion.Click += btnSelect_Click;
            btnSelectCreep.Click += btnSelect_Click;
            btnSelectLocation.Click += btnSelect_Click;
            btnSelectCraftRecipe.Click += btnSelect_Click;

            btnDownload.Click += btnDownload_Click;
            btnExport.Click += btnExport_Click;

            chkTopMost.CheckedChanged += chkTopMost_CheckedChanged;

            mNoxpedia.InformationChanged += noxpedia_InformationChanged;

            btnTest.Click += btnTest_Click;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var dic = new Dictionary<string, Noxypedia.Model.ClipImageSet>();
            var keys = new string[]
            {
                "ClipImage",
                "FullImage",
                "PathImage"
            };
            ClipImageEditor.ShowDialog(dic, keys);
        }

        private static T getAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            object[] attributes =
                assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if ((attributes == null) || (attributes.Length == 0))
            {
                return null;
            }

            // Convert the first attribute value into
            // the desired type and return it.
            return (T)attributes[0];
        }

        private void switchingView(EView index)
        {
            if (mSubForms.ContainsKey(index) == false)
            {
                switch (index)
                {
                    case EView.ItemGradeEditor:
                        {
                            mSubForms[index] = new ItemGradeEditor();
                        }
                        break;
                    case EView.UniqueOptionEditor:
                        {
                            mSubForms[index] = new UniqueOptionEditor();
                        }
                        break;
                    case EView.ItemEditor:
                        {
                            mSubForms[index] = new ItemEditor();
                        }
                        break;
                    case EView.RegionEditor:
                        {
                            mSubForms[index] = new RegionEditor();
                        }
                        break;
                    case EView.CreepEditor:
                        {
                            mSubForms[index] = new CreepEditor();
                        }
                        break;
                    case EView.LocationEditor:
                        {
                            mSubForms[index] = new LocationEditor();
                        }
                        break;
                    case EView.CraftRecipeEditor:
                        {
                            mSubForms[index] = new CraftRecipeEditor();
                        }
                        break;
                    default:
                        {
                            Debug.Assert(false);
                        }
                        break;
                }
                mSubForms[index].Owner = this;
                mSubForms[index].TopLevel = false;
                mSubForms[index].Dock = DockStyle.Fill;
                pnlContent.Controls.Add(mSubForms[index]);
            }

            foreach (var item in mSubForms)
            {
                Form subForm = item.Value;
                subForm.Visible = (item.Key == index);
            }
        }

        private void form_Load(object sender, EventArgs e)
        {
            Location = mConfig.MainWindowLocation;
            Size = mConfig.MainWindowSize;

            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyFileVersionAttribute fileVersionAttr = getAssemblyAttribute<AssemblyFileVersionAttribute>(asm);
            if (fileVersionAttr != null)
            {
                Text += $"　[Ver{fileVersionAttr.Version}]";
            }

            lblStatus.Text = "데이터 없음 — 파일에서 불러오기를 사용하세요";
            switchingView(EView.ItemGradeEditor);
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                mConfig.MainWindowLocation = Location;
                mConfig.MainWindowSize = Size;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null)
            {
                return;
            }

            switchingView((EView)c.Tag);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (mNoxpedia.Data != null && mNoxpedia.Data.Items.Count > 0)
            {
                if (MessageBox.Show("현재 로드된 데이터가 있습니다.\n파일을 불러오면 수정된 내용을 잊을 수 있습니다.\n계속하겠습니까?",
                    "Noxypedia Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            using (var ofd = new OpenFileDialog
            {
                Title = "noxypedia.dat 열기",
                Filter = "Noxypedia 데이터 파일 (*.dat)|*.dat",
                DefaultExt = "dat"
            })
            {
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                if (!mNoxpedia.LoadFromFile(ofd.FileName))
                {
                    MessageBox.Show("파일 로드에 실패했습니다.", "Noxypedia Editor",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (mNoxpedia.Data == null)
            {
                MessageBox.Show("데이터가 로드되지 않았습니다.", "Noxypedia Editor",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "noxypedia.dat 내보내기 위치 선택",
                FileName = "noxypedia.dat",
                Filter = "Noxypedia 데이터 파일 (*.dat)|*.dat",
                DefaultExt = "dat"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                NoxypediaDataFile.Save(mNoxpedia.Data, mNoxpedia.Version + 1, sfd.FileName);
                mNoxpedia.LoadFromFile(sfd.FileName);
                MessageBox.Show(
                    $"noxypedia.dat 내보내기 성공\n\n저장 위치: {sfd.FileName}\n데이터 버전: {mNoxpedia.Version}",
                    "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"내보내기 실패: {ex.Message}", "Noxypedia Editor",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            TopMost
                = mConfig.TopMost
                = chkTopMost.Checked;
        }

        private void noxpedia_InformationChanged(object sender, EventArgs e)
        {
            lblStatus.Text = $"info> {mNoxpedia.UpdateDate.ToLocalTime()} / {mNoxpedia.Auther} / {mNoxpedia.Version}";
        }
    }
}
