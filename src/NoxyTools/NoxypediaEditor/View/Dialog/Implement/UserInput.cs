using System;
using System.Windows.Forms;

namespace NoxypediaEditor
{
    public class UserInput : UserInputDialog
    {
        public string Title { get; private set; } = string.Empty;
        public string UserInputText { get; private set; } = string.Empty;

        public UserInput(string title, string initializeText) : base()
        {
            Title = title;
            UserInputText = initializeText;
            initializeComponent();
            registEvent();
        }

        private void initializeComponent()
        {
            lblTitle.Text = Title;
            txbInput.Text = UserInputText;
        }

        private void registEvent()
        {
            Load += form_Load;
            Shown += form_Shown;
            FormClosing += form_FormClosing;

            txbInput.TextChanged += txbInput_TextChanged;
            btnOk.Click += btnOk_Click;
            btnCencel.Click += btnCencel_Click;
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

        private void txbInput_TextChanged(object sender, EventArgs e)
        {
            UserInputText = txbInput.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCencel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
