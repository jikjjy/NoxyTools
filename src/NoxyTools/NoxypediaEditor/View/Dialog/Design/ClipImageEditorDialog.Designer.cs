namespace NoxypediaEditor
{
    partial class ClipImageEditorDialog
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
            this.pnlBase = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSubTitleImageList = new System.Windows.Forms.Label();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.SplitContainer();
            this.pnlBaseInformation = new System.Windows.Forms.Panel();
            this.pnlBaseData = new System.Windows.Forms.SplitContainer();
            this.lblSubTitleSourceUrl = new System.Windows.Forms.Label();
            this.txbSourceUrl = new System.Windows.Forms.TextBox();
            this.lblSubTitleDescription = new System.Windows.Forms.Label();
            this.txbDescription = new System.Windows.Forms.TextBox();
            this.lblSubTitleData = new System.Windows.Forms.Label();
            this.pnlBaseImagePreview = new System.Windows.Forms.Panel();
            this.picPreview = new Cyotek.Windows.Forms.ImageBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSubTitlePreview = new System.Windows.Forms.Label();
            this.pnlBase.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();
            this.pnlContent.Panel1.SuspendLayout();
            this.pnlContent.Panel2.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlBaseInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBaseData)).BeginInit();
            this.pnlBaseData.Panel1.SuspendLayout();
            this.pnlBaseData.Panel2.SuspendLayout();
            this.pnlBaseData.SuspendLayout();
            this.pnlBaseImagePreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.ColumnCount = 2;
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBase.Controls.Add(this.panel3, 0, 1);
            this.pnlBase.Controls.Add(this.pnlTitle, 0, 0);
            this.pnlBase.Controls.Add(this.pnlMenu, 0, 2);
            this.pnlBase.Controls.Add(this.pnlContent, 1, 1);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.RowCount = 3;
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlBase.Size = new System.Drawing.Size(1121, 589);
            this.pnlBase.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblSubTitleImageList);
            this.panel3.Controls.Add(this.grdList);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(194, 503);
            this.panel3.TabIndex = 5;
            // 
            // lblSubTitleImageList
            // 
            this.lblSubTitleImageList.AutoSize = true;
            this.lblSubTitleImageList.Location = new System.Drawing.Point(25, 14);
            this.lblSubTitleImageList.Name = "lblSubTitleImageList";
            this.lblSubTitleImageList.Size = new System.Drawing.Size(90, 17);
            this.lblSubTitleImageList.TabIndex = 0;
            this.lblSubTitleImageList.Text = "이미지 리스트";
            // 
            // grdList
            // 
            this.grdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.Location = new System.Drawing.Point(5, 38);
            this.grdList.Name = "grdList";
            this.grdList.RowTemplate.Height = 23;
            this.grdList.Size = new System.Drawing.Size(184, 462);
            this.grdList.TabIndex = 1;
            // 
            // pnlTitle
            // 
            this.pnlBase.SetColumnSpan(this.pnlTitle, 2);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(3, 3);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1115, 34);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(48, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(129, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "이미지 리스트 편집";
            // 
            // pnlMenu
            // 
            this.pnlBase.SetColumnSpan(this.pnlMenu, 2);
            this.pnlMenu.Controls.Add(this.btnOk);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenu.Location = new System.Drawing.Point(3, 552);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(1115, 34);
            this.pnlMenu.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(1115, 34);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(203, 43);
            this.pnlContent.Name = "pnlContent";
            // 
            // pnlContent.Panel1
            // 
            this.pnlContent.Panel1.Controls.Add(this.pnlBaseInformation);
            // 
            // pnlContent.Panel2
            // 
            this.pnlContent.Panel2.Controls.Add(this.pnlBaseImagePreview);
            this.pnlContent.Size = new System.Drawing.Size(915, 503);
            this.pnlContent.SplitterDistance = 305;
            this.pnlContent.TabIndex = 6;
            // 
            // pnlBaseInformation
            // 
            this.pnlBaseInformation.Controls.Add(this.pnlBaseData);
            this.pnlBaseInformation.Controls.Add(this.lblSubTitleData);
            this.pnlBaseInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBaseInformation.Location = new System.Drawing.Point(0, 0);
            this.pnlBaseInformation.Name = "pnlBaseInformation";
            this.pnlBaseInformation.Size = new System.Drawing.Size(305, 503);
            this.pnlBaseInformation.TabIndex = 3;
            // 
            // pnlBaseData
            // 
            this.pnlBaseData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBaseData.Location = new System.Drawing.Point(1, 38);
            this.pnlBaseData.Name = "pnlBaseData";
            this.pnlBaseData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlBaseData.Panel1
            // 
            this.pnlBaseData.Panel1.Controls.Add(this.lblSubTitleSourceUrl);
            this.pnlBaseData.Panel1.Controls.Add(this.txbSourceUrl);
            // 
            // pnlBaseData.Panel2
            // 
            this.pnlBaseData.Panel2.Controls.Add(this.lblSubTitleDescription);
            this.pnlBaseData.Panel2.Controls.Add(this.txbDescription);
            this.pnlBaseData.Size = new System.Drawing.Size(301, 462);
            this.pnlBaseData.SplitterDistance = 221;
            this.pnlBaseData.TabIndex = 4;
            // 
            // lblSubTitleSourceUrl
            // 
            this.lblSubTitleSourceUrl.AutoSize = true;
            this.lblSubTitleSourceUrl.Location = new System.Drawing.Point(3, 4);
            this.lblSubTitleSourceUrl.Name = "lblSubTitleSourceUrl";
            this.lblSubTitleSourceUrl.Size = new System.Drawing.Size(103, 17);
            this.lblSubTitleSourceUrl.TabIndex = 2;
            this.lblSubTitleSourceUrl.Text = "이미지 소스 URL";
            // 
            // txbSourceUrl
            // 
            this.txbSourceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSourceUrl.BackColor = System.Drawing.Color.DarkGray;
            this.txbSourceUrl.Location = new System.Drawing.Point(3, 24);
            this.txbSourceUrl.Multiline = true;
            this.txbSourceUrl.Name = "txbSourceUrl";
            this.txbSourceUrl.Size = new System.Drawing.Size(295, 188);
            this.txbSourceUrl.TabIndex = 3;
            // 
            // lblSubTitleDescription
            // 
            this.lblSubTitleDescription.AutoSize = true;
            this.lblSubTitleDescription.Location = new System.Drawing.Point(3, 4);
            this.lblSubTitleDescription.Name = "lblSubTitleDescription";
            this.lblSubTitleDescription.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleDescription.TabIndex = 2;
            this.lblSubTitleDescription.Text = "설명";
            // 
            // txbDescription
            // 
            this.txbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDescription.BackColor = System.Drawing.Color.DarkGray;
            this.txbDescription.Location = new System.Drawing.Point(3, 24);
            this.txbDescription.Multiline = true;
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.Size = new System.Drawing.Size(295, 207);
            this.txbDescription.TabIndex = 3;
            // 
            // lblSubTitleData
            // 
            this.lblSubTitleData.AutoSize = true;
            this.lblSubTitleData.Location = new System.Drawing.Point(25, 14);
            this.lblSubTitleData.Name = "lblSubTitleData";
            this.lblSubTitleData.Size = new System.Drawing.Size(107, 17);
            this.lblSubTitleData.TabIndex = 1;
            this.lblSubTitleData.Text = "선택 이미지 정보";
            // 
            // pnlBaseImagePreview
            // 
            this.pnlBaseImagePreview.Controls.Add(this.picPreview);
            this.pnlBaseImagePreview.Controls.Add(this.btnRefresh);
            this.pnlBaseImagePreview.Controls.Add(this.lblSubTitlePreview);
            this.pnlBaseImagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBaseImagePreview.Location = new System.Drawing.Point(0, 0);
            this.pnlBaseImagePreview.Name = "pnlBaseImagePreview";
            this.pnlBaseImagePreview.Size = new System.Drawing.Size(606, 503);
            this.pnlBaseImagePreview.TabIndex = 4;
            // 
            // picPreview
            // 
            this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.picPreview.Location = new System.Drawing.Point(5, 42);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(596, 418);
            this.picPreview.TabIndex = 4;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnRefresh.Location = new System.Drawing.Point(5, 466);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(596, 34);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "이미지 새로고침";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblSubTitlePreview
            // 
            this.lblSubTitlePreview.AutoSize = true;
            this.lblSubTitlePreview.Location = new System.Drawing.Point(25, 14);
            this.lblSubTitlePreview.Name = "lblSubTitlePreview";
            this.lblSubTitlePreview.Size = new System.Drawing.Size(90, 17);
            this.lblSubTitlePreview.TabIndex = 1;
            this.lblSubTitlePreview.Text = "이미지 프리뷰";
            // 
            // ClipImageEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1121, 589);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ClipImageEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Noxypedia Editor";
            this.pnlBase.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlMenu.ResumeLayout(false);
            this.pnlContent.Panel1.ResumeLayout(false);
            this.pnlContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.pnlBaseInformation.ResumeLayout(false);
            this.pnlBaseInformation.PerformLayout();
            this.pnlBaseData.Panel1.ResumeLayout(false);
            this.pnlBaseData.Panel1.PerformLayout();
            this.pnlBaseData.Panel2.ResumeLayout(false);
            this.pnlBaseData.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBaseData)).EndInit();
            this.pnlBaseData.ResumeLayout(false);
            this.pnlBaseImagePreview.ResumeLayout(false);
            this.pnlBaseImagePreview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel pnlBase;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSubTitleImageList;
        protected System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMenu;
        protected System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlBaseInformation;
        private System.Windows.Forms.Label lblSubTitleData;
        private System.Windows.Forms.Label lblSubTitleDescription;
        private System.Windows.Forms.Label lblSubTitleSourceUrl;
        protected System.Windows.Forms.TextBox txbDescription;
        protected System.Windows.Forms.TextBox txbSourceUrl;
        private System.Windows.Forms.SplitContainer pnlBaseData;
        private System.Windows.Forms.Panel pnlBaseImagePreview;
        protected System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblSubTitlePreview;
        protected System.Windows.Forms.SplitContainer pnlContent;
        protected Cyotek.Windows.Forms.ImageBox picPreview;
    }
}