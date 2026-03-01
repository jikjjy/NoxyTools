namespace NoxypediaEditor
{
    partial class ItemGradeEditorWindow
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
            this.pnlBase = new System.Windows.Forms.SplitContainer();
            this.lblSubTitleFilter = new System.Windows.Forms.Label();
            this.txbFilter = new System.Windows.Forms.TextBox();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.lblTitleGradeList = new System.Windows.Forms.Label();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.pnlEditMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditClipImage = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.txbGradeIndex = new System.Windows.Forms.TextBox();
            this.lblSubTitleGradeIndex = new System.Windows.Forms.Label();
            this.pnlColorCodePreview = new System.Windows.Forms.Panel();
            this.txbColorCode = new System.Windows.Forms.TextBox();
            this.lblSubTitleColorCode = new System.Windows.Forms.Label();
            this.txbName = new System.Windows.Forms.TextBox();
            this.lblSubTitleName = new System.Windows.Forms.Label();
            this.lblTitleEditor = new System.Windows.Forms.Label();
            this.btnEditRename = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).BeginInit();
            this.pnlBase.Panel1.SuspendLayout();
            this.pnlBase.Panel2.SuspendLayout();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlEditMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Name = "pnlBase";
            // 
            // pnlBase.Panel1
            // 
            this.pnlBase.Panel1.Controls.Add(this.lblSubTitleFilter);
            this.pnlBase.Panel1.Controls.Add(this.txbFilter);
            this.pnlBase.Panel1.Controls.Add(this.grdList);
            this.pnlBase.Panel1.Controls.Add(this.lblTitleGradeList);
            // 
            // pnlBase.Panel2
            // 
            this.pnlBase.Panel2.Controls.Add(this.btnEditRename);
            this.pnlBase.Panel2.Controls.Add(this.lblVersionInfo);
            this.pnlBase.Panel2.Controls.Add(this.pnlEditMenu);
            this.pnlBase.Panel2.Controls.Add(this.txbGradeIndex);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleGradeIndex);
            this.pnlBase.Panel2.Controls.Add(this.pnlColorCodePreview);
            this.pnlBase.Panel2.Controls.Add(this.txbColorCode);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleColorCode);
            this.pnlBase.Panel2.Controls.Add(this.txbName);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleName);
            this.pnlBase.Panel2.Controls.Add(this.lblTitleEditor);
            this.pnlBase.Size = new System.Drawing.Size(1234, 634);
            this.pnlBase.SplitterDistance = 411;
            this.pnlBase.TabIndex = 0;
            // 
            // lblSubTitleFilter
            // 
            this.lblSubTitleFilter.AutoSize = true;
            this.lblSubTitleFilter.Location = new System.Drawing.Point(12, 63);
            this.lblSubTitleFilter.Name = "lblSubTitleFilter";
            this.lblSubTitleFilter.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleFilter.TabIndex = 3;
            this.lblSubTitleFilter.Text = "필터";
            // 
            // txbFilter
            // 
            this.txbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbFilter.BackColor = System.Drawing.Color.DarkGray;
            this.txbFilter.Location = new System.Drawing.Point(68, 61);
            this.txbFilter.Name = "txbFilter";
            this.txbFilter.Size = new System.Drawing.Size(331, 23);
            this.txbFilter.TabIndex = 2;
            // 
            // grdList
            // 
            this.grdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.Location = new System.Drawing.Point(12, 90);
            this.grdList.Name = "grdList";
            this.grdList.RowTemplate.Height = 23;
            this.grdList.Size = new System.Drawing.Size(387, 532);
            this.grdList.TabIndex = 1;
            // 
            // lblTitleGradeList
            // 
            this.lblTitleGradeList.AutoSize = true;
            this.lblTitleGradeList.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleGradeList.Location = new System.Drawing.Point(47, 22);
            this.lblTitleGradeList.Name = "lblTitleGradeList";
            this.lblTitleGradeList.Size = new System.Drawing.Size(129, 19);
            this.lblTitleGradeList.TabIndex = 0;
            this.lblTitleGradeList.Text = "아이템 등급 리스트";
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblVersionInfo.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionInfo.Location = new System.Drawing.Point(0, 607);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(819, 27);
            this.lblVersionInfo.TabIndex = 21;
            this.lblVersionInfo.Text = "Version Information";
            this.lblVersionInfo.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pnlEditMenu
            // 
            this.pnlEditMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditMenu.ColumnCount = 4;
            this.pnlEditMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlEditMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlEditMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlEditMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlEditMenu.Controls.Add(this.btnEditClipImage, 0, 0);
            this.pnlEditMenu.Controls.Add(this.btnAdd, 1, 0);
            this.pnlEditMenu.Controls.Add(this.btnDelete, 2, 0);
            this.pnlEditMenu.Controls.Add(this.btnEdit, 3, 0);
            this.pnlEditMenu.Location = new System.Drawing.Point(49, 57);
            this.pnlEditMenu.Name = "pnlEditMenu";
            this.pnlEditMenu.RowCount = 1;
            this.pnlEditMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlEditMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlEditMenu.Size = new System.Drawing.Size(746, 31);
            this.pnlEditMenu.TabIndex = 19;
            // 
            // btnEditClipImage
            // 
            this.btnEditClipImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnEditClipImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditClipImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.btnEditClipImage.Location = new System.Drawing.Point(0, 0);
            this.btnEditClipImage.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditClipImage.Name = "btnEditClipImage";
            this.btnEditClipImage.Size = new System.Drawing.Size(186, 31);
            this.btnEditClipImage.TabIndex = 11;
            this.btnEditClipImage.Text = "클립 이미지 편집";
            this.btnEditClipImage.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.btnAdd.Location = new System.Drawing.Point(192, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(180, 31);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.btnDelete.Location = new System.Drawing.Point(378, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(180, 31);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Location = new System.Drawing.Point(564, 0);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(182, 31);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "수정";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // txbGradeIndex
            // 
            this.txbGradeIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbGradeIndex.BackColor = System.Drawing.Color.DarkGray;
            this.txbGradeIndex.Location = new System.Drawing.Point(49, 267);
            this.txbGradeIndex.Name = "txbGradeIndex";
            this.txbGradeIndex.Size = new System.Drawing.Size(746, 23);
            this.txbGradeIndex.TabIndex = 7;
            // 
            // lblSubTitleGradeIndex
            // 
            this.lblSubTitleGradeIndex.AutoSize = true;
            this.lblSubTitleGradeIndex.Location = new System.Drawing.Point(27, 236);
            this.lblSubTitleGradeIndex.Name = "lblSubTitleGradeIndex";
            this.lblSubTitleGradeIndex.Size = new System.Drawing.Size(64, 17);
            this.lblSubTitleGradeIndex.TabIndex = 6;
            this.lblSubTitleGradeIndex.Text = "등급 번호";
            // 
            // pnlColorCodePreview
            // 
            this.pnlColorCodePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlColorCodePreview.BackColor = System.Drawing.Color.Black;
            this.pnlColorCodePreview.Location = new System.Drawing.Point(736, 199);
            this.pnlColorCodePreview.Name = "pnlColorCodePreview";
            this.pnlColorCodePreview.Size = new System.Drawing.Size(59, 23);
            this.pnlColorCodePreview.TabIndex = 5;
            // 
            // txbColorCode
            // 
            this.txbColorCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbColorCode.BackColor = System.Drawing.Color.DarkGray;
            this.txbColorCode.Location = new System.Drawing.Point(49, 199);
            this.txbColorCode.Name = "txbColorCode";
            this.txbColorCode.Size = new System.Drawing.Size(681, 23);
            this.txbColorCode.TabIndex = 4;
            // 
            // lblSubTitleColorCode
            // 
            this.lblSubTitleColorCode.AutoSize = true;
            this.lblSubTitleColorCode.Location = new System.Drawing.Point(27, 168);
            this.lblSubTitleColorCode.Name = "lblSubTitleColorCode";
            this.lblSubTitleColorCode.Size = new System.Drawing.Size(88, 17);
            this.lblSubTitleColorCode.TabIndex = 3;
            this.lblSubTitleColorCode.Text = "색 코드 (HEX)";
            // 
            // txbName
            // 
            this.txbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbName.BackColor = System.Drawing.Color.DarkGray;
            this.txbName.Location = new System.Drawing.Point(49, 131);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(645, 23);
            this.txbName.TabIndex = 2;
            // 
            // lblSubTitleName
            // 
            this.lblSubTitleName.AutoSize = true;
            this.lblSubTitleName.Location = new System.Drawing.Point(27, 100);
            this.lblSubTitleName.Name = "lblSubTitleName";
            this.lblSubTitleName.Size = new System.Drawing.Size(60, 17);
            this.lblSubTitleName.TabIndex = 1;
            this.lblSubTitleName.Text = "이름 (ID)";
            // 
            // lblTitleEditor
            // 
            this.lblTitleEditor.AutoSize = true;
            this.lblTitleEditor.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleEditor.Location = new System.Drawing.Point(49, 22);
            this.lblTitleEditor.Name = "lblTitleEditor";
            this.lblTitleEditor.Size = new System.Drawing.Size(37, 19);
            this.lblTitleEditor.TabIndex = 0;
            this.lblTitleEditor.Text = "편집";
            // 
            // btnEditRename
            // 
            this.btnEditRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnEditRename.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.btnEditRename.Location = new System.Drawing.Point(697, 127);
            this.btnEditRename.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditRename.Name = "btnEditRename";
            this.btnEditRename.Size = new System.Drawing.Size(98, 31);
            this.btnEditRename.TabIndex = 23;
            this.btnEditRename.Text = "이름 변경";
            this.btnEditRename.UseVisualStyleBackColor = false;
            // 
            // ItemGradeEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1234, 634);
            this.Controls.Add(this.pnlBase);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Name = "ItemGradeEditorWindow";
            this.pnlBase.Panel1.ResumeLayout(false);
            this.pnlBase.Panel1.PerformLayout();
            this.pnlBase.Panel2.ResumeLayout(false);
            this.pnlBase.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).EndInit();
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlEditMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.SplitContainer pnlBase;
        protected System.Windows.Forms.TextBox txbGradeIndex;
        protected System.Windows.Forms.Label lblSubTitleGradeIndex;
        protected System.Windows.Forms.Label lblTitleGradeList;
        protected System.Windows.Forms.Panel pnlColorCodePreview;
        protected System.Windows.Forms.TextBox txbColorCode;
        protected System.Windows.Forms.Label lblSubTitleColorCode;
        protected System.Windows.Forms.TextBox txbName;
        protected System.Windows.Forms.Label lblSubTitleName;
        protected System.Windows.Forms.Label lblTitleEditor;
        protected System.Windows.Forms.DataGridView grdList;
        protected System.Windows.Forms.TextBox txbFilter;
        protected System.Windows.Forms.Label lblSubTitleFilter;
        private System.Windows.Forms.TableLayoutPanel pnlEditMenu;
        protected System.Windows.Forms.Button btnEditClipImage;
        protected System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.Button btnDelete;
        protected System.Windows.Forms.Button btnEdit;
        protected System.Windows.Forms.Label lblVersionInfo;
        protected System.Windows.Forms.Button btnEditRename;
    }
}