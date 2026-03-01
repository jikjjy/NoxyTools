
namespace NoxypediaEditor
{
    partial class UpdateHistoryDialog
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
            this.grdList = new System.Windows.Forms.DataGridView();
            this.pnlData = new System.Windows.Forms.Panel();
            this.txbComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblAuther = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdList
            // 
            this.grdList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.Location = new System.Drawing.Point(12, 51);
            this.grdList.Name = "grdList";
            this.grdList.RowTemplate.Height = 23;
            this.grdList.Size = new System.Drawing.Size(377, 457);
            this.grdList.TabIndex = 0;
            // 
            // pnlData
            // 
            this.pnlData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlData.Controls.Add(this.txbComment);
            this.pnlData.Controls.Add(this.lblComment);
            this.pnlData.Controls.Add(this.lblDate);
            this.pnlData.Controls.Add(this.lblAuther);
            this.pnlData.Controls.Add(this.label6);
            this.pnlData.Controls.Add(this.label3);
            this.pnlData.Controls.Add(this.lblVersion);
            this.pnlData.Controls.Add(this.label1);
            this.pnlData.Location = new System.Drawing.Point(395, 12);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(383, 496);
            this.pnlData.TabIndex = 1;
            // 
            // txbComment
            // 
            this.txbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbComment.BackColor = System.Drawing.Color.DarkGray;
            this.txbComment.Location = new System.Drawing.Point(39, 250);
            this.txbComment.Multiline = true;
            this.txbComment.Name = "txbComment";
            this.txbComment.ReadOnly = true;
            this.txbComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbComment.Size = new System.Drawing.Size(344, 246);
            this.txbComment.TabIndex = 1;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(18, 221);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(60, 17);
            this.lblComment.TabIndex = 0;
            this.lblComment.Text = "# 코맨트";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(36, 179);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(12, 17);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "-";
            // 
            // lblAuther
            // 
            this.lblAuther.AutoSize = true;
            this.lblAuther.Location = new System.Drawing.Point(36, 114);
            this.lblAuther.Name = "lblAuther";
            this.lblAuther.Size = new System.Drawing.Size(12, 17);
            this.lblAuther.TabIndex = 0;
            this.lblAuther.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "# 날짜";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "# 수정자";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(36, 48);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(12, 17);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "# 버전";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "버전 히스토리 조회";
            // 
            // UpdateHistoryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(790, 520);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.grdList);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Name = "UpdateHistoryDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Noxypedia Editor";
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.DataGridView grdList;
        protected System.Windows.Forms.TextBox txbComment;
        protected System.Windows.Forms.Label lblDate;
        protected System.Windows.Forms.Label lblAuther;
        protected System.Windows.Forms.Label lblVersion;
    }
}