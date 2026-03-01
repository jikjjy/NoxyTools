using System;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public class ConfirmUpload : ConfirmUploadDialog
    {
        public string Auther { get; private set; } = string.Empty;
        public string Comment { get; private set; } = string.Empty;
        private readonly Model.ConfigService mConfig = Services.Config.Value;

        public ConfirmUpload() : base()
        {
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            Auther
                = txbAuther.Text
                = mConfig.NoxypediaAuther;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            txbAuther.Validating += txbAuther_Validating;
            txbComment.Validating += txbComment_Validating;

            btnOk.Click += btnOk_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void form_Load(object sender, EventArgs e)
        {
        }

        private void form_Shown(object sender, EventArgs e)
        {
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void txbAuther_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mConfig.NoxypediaAuther = txbAuther.Text;
            Auther = txbAuther.Text;
        }

        private void txbComment_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Comment = txbComment.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
