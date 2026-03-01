namespace NoxypediaEditor
{
    partial class ConfirmUploadDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.lblSubTitleAuther = new System.Windows.Forms.Label();
            this.lblSubTitleComment = new System.Windows.Forms.Label();
            this.txbComment = new System.Windows.Forms.TextBox();
            this.txbAuther = new System.Windows.Forms.TextBox();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(46, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(83, 19);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "업로드 확인";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(35, 55);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(266, 17);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "변경된 내용을 서버에 업로드 하시겠습니까?";
            // 
            // lblSubTitleAuther
            // 
            this.lblSubTitleAuther.AutoSize = true;
            this.lblSubTitleAuther.Location = new System.Drawing.Point(22, 92);
            this.lblSubTitleAuther.Name = "lblSubTitleAuther";
            this.lblSubTitleAuther.Size = new System.Drawing.Size(47, 17);
            this.lblSubTitleAuther.TabIndex = 2;
            this.lblSubTitleAuther.Text = "수정자";
            // 
            // lblSubTitleComment
            // 
            this.lblSubTitleComment.AutoSize = true;
            this.lblSubTitleComment.Location = new System.Drawing.Point(22, 146);
            this.lblSubTitleComment.Name = "lblSubTitleComment";
            this.lblSubTitleComment.Size = new System.Drawing.Size(77, 17);
            this.lblSubTitleComment.TabIndex = 2;
            this.lblSubTitleComment.Text = "수정 코멘트";
            // 
            // txbComment
            // 
            this.txbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbComment.BackColor = System.Drawing.Color.DarkGray;
            this.txbComment.Location = new System.Drawing.Point(38, 169);
            this.txbComment.Multiline = true;
            this.txbComment.Name = "txbComment";
            this.txbComment.Size = new System.Drawing.Size(534, 86);
            this.txbComment.TabIndex = 3;
            // 
            // txbAuther
            // 
            this.txbAuther.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbAuther.BackColor = System.Drawing.Color.DarkGray;
            this.txbAuther.Location = new System.Drawing.Point(38, 116);
            this.txbAuther.Name = "txbAuther";
            this.txbAuther.Size = new System.Drawing.Size(534, 23);
            this.txbAuther.TabIndex = 4;
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlMenu.Controls.Add(this.btnCancel);
            this.pnlMenu.Controls.Add(this.btnOk);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMenu.Location = new System.Drawing.Point(0, 266);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(584, 50);
            this.pnlMenu.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(431, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 38);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnOk.Location = new System.Drawing.Point(284, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(141, 38);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // ConfirmUploadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 316);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.txbAuther);
            this.Controls.Add(this.txbComment);
            this.Controls.Add(this.lblSubTitleComment);
            this.Controls.Add(this.lblSubTitleAuther);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfirmUploadDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Noxypedia Editor";
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSubTitleAuther;
        private System.Windows.Forms.Label lblSubTitleComment;
        private System.Windows.Forms.Panel pnlMenu;
        protected System.Windows.Forms.Label lblText;
        protected System.Windows.Forms.TextBox txbComment;
        protected System.Windows.Forms.TextBox txbAuther;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnOk;
        protected System.Windows.Forms.Label lblTitle;
    }
}