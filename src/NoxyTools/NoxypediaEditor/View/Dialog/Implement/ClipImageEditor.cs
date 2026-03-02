using Noxypedia.Model;
using System.Data;

namespace NoxypediaEditor
{
    public class ClipImageEditor : ClipImageEditorDialog
    {
        public Dictionary<string, ClipImageSet> Result { get; private set; }
        private static class GridColumn
        {
            public static readonly string NAME = "Name";
            public static readonly string RAWDATA = "Rawdata";
            public static readonly string FILTERING_SOURCE = "FilteringSource";
        }
        private ClipImageSet mSelectItem = new ClipImageSet();
        private DataTable mDataTable = new DataTable();
        private MemoryStream mImageStream = new MemoryStream();
        private string[] mKeys;

        private ClipImageEditor() : base()
        {
            initializeComponent();
            registEvent();
        }

        public static DialogResult ShowDialog(Dictionary<string, ClipImageSet> src, string[] keys)
        {
            using (var dialog = new ClipImageEditor())
            {
                dialog.Result = src;
                dialog.mKeys = keys;
                return dialog.ShowDialog();
            }
        }

        private void initializeComponent()
        {
            mDataTable.Clear();
            mDataTable.Columns.Add(GridColumn.NAME, typeof(string));
            mDataTable.Columns.Add(GridColumn.RAWDATA, typeof(ClipImageSet));
            mDataTable.Columns.Add(GridColumn.FILTERING_SOURCE, typeof(string));

            Utils.InitializeGridView(grdList);
            grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdList.MultiSelect = false;
            grdList.ReadOnly = true;

            grdList.DataSource = mDataTable;
            grdList.Columns[GridColumn.NAME].HeaderText = "키";
            grdList.Columns[GridColumn.RAWDATA].Visible = false;
            grdList.Columns[GridColumn.FILTERING_SOURCE].Visible = false;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            grdList.SelectionChanged += grdList_SelectionChanged;

            txbSourceUrl.TextChanged += txbSourceUrl_TextChanged;
            txbDescription.TextChanged += txbDescription_TextChanged;

            btnRefresh.Click += btnRefresh_Click;
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

        private void grdList_SelectionChanged(object sender, EventArgs e)
        {
            if (grdList.SelectedRows.Count > 0)
            {
                mSelectItem = grdList.SelectedRows[0].Cells[GridColumn.RAWDATA].Value as ClipImageSet;
            }
            updateEditUi();
        }

        private void txbSourceUrl_TextChanged(object sender, EventArgs e)
        {
            mSelectItem.SourceURL = txbSourceUrl.Text;
        }

        private void txbDescription_TextChanged(object sender, EventArgs e)
        {
            mSelectItem.Description = txbDescription.Text;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            updateImagePreview();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void updateEditUi()
        {
            if (mSelectItem is null)
            {
                return;
            }

            txbSourceUrl.Text = mSelectItem.SourceURL;
            txbDescription.Text = mSelectItem.Description;
        }

        private void updateImagePreview()
        {
            mImageStream.Seek(0, SeekOrigin.Begin);
            mImageStream.SetLength(0);
            try
            {
                var oldImage = picPreview.Image;
                oldImage?.Dispose();
                picPreview.Image = null;
                if (Utils.DownloadRemoteImageFile(mSelectItem.SourceURL, mImageStream) == false)
                {
                    MessageBox.Show("이미지 다운로드에 실패 했습니다. 이미지 URL을 확인해 주세요.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                picPreview.Image = Image.FromStream(mImageStream);
            }
            catch
            {
                MessageBox.Show("이미지 다운로드에 실패 했습니다. 이미지 URL을 확인해 주세요.", "Noxypedia Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateList()
        {
            mDataTable.Rows.Clear();
            foreach (var key in mKeys)
            {
                if (Result.ContainsKey(key) == false)
                {
                    Result[key] = new ClipImageSet();
                }
                mDataTable.Rows.Add(key, Result[key], key);
            }

            if (grdList.Rows.Count == 0)
            {
                updateEditUi();
            }
        }
    }
}
