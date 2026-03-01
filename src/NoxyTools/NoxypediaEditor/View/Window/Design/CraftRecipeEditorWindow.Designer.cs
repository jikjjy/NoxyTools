namespace NoxypediaEditor
{
    partial class CraftRecipeEditorWindow
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
            this.lblTitleList = new System.Windows.Forms.Label();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.pnlBaseCraft = new System.Windows.Forms.TableLayoutPanel();
            this.txbMaterials = new System.Windows.Forms.TextBox();
            this.lblSubTitleMaterials = new System.Windows.Forms.Label();
            this.lblSubTitleSubstituteMaterials = new System.Windows.Forms.Label();
            this.txbSubstituteMaterial = new System.Windows.Forms.TextBox();
            this.cbbLocation = new System.Windows.Forms.ComboBox();
            this.txbDescription = new System.Windows.Forms.TextBox();
            this.lblSubTitleCraft = new System.Windows.Forms.Label();
            this.lblSubTitleLocation = new System.Windows.Forms.Label();
            this.lblSubTitleDescription = new System.Windows.Forms.Label();
            this.txbSuccessProbability = new System.Windows.Forms.TextBox();
            this.txbName = new System.Windows.Forms.TextBox();
            this.lblSubTitleSuccessProbability = new System.Windows.Forms.Label();
            this.lblSubTitleName = new System.Windows.Forms.Label();
            this.lblTitleEditor = new System.Windows.Forms.Label();
            this.pnlEditMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditClipImage = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnEditRename = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).BeginInit();
            this.pnlBase.Panel1.SuspendLayout();
            this.pnlBase.Panel2.SuspendLayout();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlBaseCraft.SuspendLayout();
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
            this.pnlBase.Panel1.Controls.Add(this.lblTitleList);
            // 
            // pnlBase.Panel2
            // 
            this.pnlBase.Panel2.Controls.Add(this.btnEditRename);
            this.pnlBase.Panel2.Controls.Add(this.lblVersionInfo);
            this.pnlBase.Panel2.Controls.Add(this.pnlBaseCraft);
            this.pnlBase.Panel2.Controls.Add(this.cbbLocation);
            this.pnlBase.Panel2.Controls.Add(this.txbDescription);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleCraft);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleLocation);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleDescription);
            this.pnlBase.Panel2.Controls.Add(this.txbSuccessProbability);
            this.pnlBase.Panel2.Controls.Add(this.txbName);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleSuccessProbability);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleName);
            this.pnlBase.Panel2.Controls.Add(this.lblTitleEditor);
            this.pnlBase.Panel2.Controls.Add(this.pnlEditMenu);
            this.pnlBase.Size = new System.Drawing.Size(1234, 634);
            this.pnlBase.SplitterDistance = 411;
            this.pnlBase.TabIndex = 4;
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
            // lblTitleList
            // 
            this.lblTitleList.AutoSize = true;
            this.lblTitleList.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleList.Location = new System.Drawing.Point(47, 22);
            this.lblTitleList.Name = "lblTitleList";
            this.lblTitleList.Size = new System.Drawing.Size(83, 19);
            this.lblTitleList.TabIndex = 0;
            this.lblTitleList.Text = "조합 리스트";
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
            // pnlBaseCraft
            // 
            this.pnlBaseCraft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBaseCraft.ColumnCount = 2;
            this.pnlBaseCraft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBaseCraft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBaseCraft.Controls.Add(this.txbMaterials, 0, 1);
            this.pnlBaseCraft.Controls.Add(this.lblSubTitleMaterials, 0, 0);
            this.pnlBaseCraft.Controls.Add(this.lblSubTitleSubstituteMaterials, 1, 0);
            this.pnlBaseCraft.Controls.Add(this.txbSubstituteMaterial, 1, 1);
            this.pnlBaseCraft.Location = new System.Drawing.Point(49, 384);
            this.pnlBaseCraft.Name = "pnlBaseCraft";
            this.pnlBaseCraft.RowCount = 2;
            this.pnlBaseCraft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlBaseCraft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBaseCraft.Size = new System.Drawing.Size(744, 220);
            this.pnlBaseCraft.TabIndex = 17;
            // 
            // txbMaterials
            // 
            this.txbMaterials.BackColor = System.Drawing.Color.DarkGray;
            this.txbMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbMaterials.Location = new System.Drawing.Point(5, 45);
            this.txbMaterials.Margin = new System.Windows.Forms.Padding(5);
            this.txbMaterials.Multiline = true;
            this.txbMaterials.Name = "txbMaterials";
            this.txbMaterials.ReadOnly = true;
            this.txbMaterials.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbMaterials.Size = new System.Drawing.Size(362, 170);
            this.txbMaterials.TabIndex = 0;
            // 
            // lblSubTitleMaterials
            // 
            this.lblSubTitleMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTitleMaterials.Location = new System.Drawing.Point(3, 3);
            this.lblSubTitleMaterials.Margin = new System.Windows.Forms.Padding(3);
            this.lblSubTitleMaterials.Name = "lblSubTitleMaterials";
            this.lblSubTitleMaterials.Size = new System.Drawing.Size(366, 34);
            this.lblSubTitleMaterials.TabIndex = 14;
            this.lblSubTitleMaterials.Text = "  # 재료 리스트";
            this.lblSubTitleMaterials.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSubTitleSubstituteMaterials
            // 
            this.lblSubTitleSubstituteMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTitleSubstituteMaterials.Location = new System.Drawing.Point(375, 3);
            this.lblSubTitleSubstituteMaterials.Margin = new System.Windows.Forms.Padding(3);
            this.lblSubTitleSubstituteMaterials.Name = "lblSubTitleSubstituteMaterials";
            this.lblSubTitleSubstituteMaterials.Size = new System.Drawing.Size(366, 34);
            this.lblSubTitleSubstituteMaterials.TabIndex = 15;
            this.lblSubTitleSubstituteMaterials.Text = "  # 대체 가능한 재료";
            this.lblSubTitleSubstituteMaterials.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txbSubstituteMaterial
            // 
            this.txbSubstituteMaterial.BackColor = System.Drawing.Color.DarkGray;
            this.txbSubstituteMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbSubstituteMaterial.Location = new System.Drawing.Point(377, 45);
            this.txbSubstituteMaterial.Margin = new System.Windows.Forms.Padding(5);
            this.txbSubstituteMaterial.Multiline = true;
            this.txbSubstituteMaterial.Name = "txbSubstituteMaterial";
            this.txbSubstituteMaterial.ReadOnly = true;
            this.txbSubstituteMaterial.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbSubstituteMaterial.Size = new System.Drawing.Size(362, 170);
            this.txbSubstituteMaterial.TabIndex = 16;
            // 
            // cbbLocation
            // 
            this.cbbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLocation.FormattingEnabled = true;
            this.cbbLocation.Location = new System.Drawing.Point(49, 308);
            this.cbbLocation.Name = "cbbLocation";
            this.cbbLocation.Size = new System.Drawing.Size(745, 25);
            this.cbbLocation.TabIndex = 16;
            // 
            // txbDescription
            // 
            this.txbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDescription.BackColor = System.Drawing.Color.DarkGray;
            this.txbDescription.Location = new System.Drawing.Point(49, 202);
            this.txbDescription.Multiline = true;
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbDescription.Size = new System.Drawing.Size(746, 55);
            this.txbDescription.TabIndex = 15;
            // 
            // lblSubTitleCraft
            // 
            this.lblSubTitleCraft.AutoSize = true;
            this.lblSubTitleCraft.Location = new System.Drawing.Point(27, 353);
            this.lblSubTitleCraft.Name = "lblSubTitleCraft";
            this.lblSubTitleCraft.Size = new System.Drawing.Size(47, 17);
            this.lblSubTitleCraft.TabIndex = 14;
            this.lblSubTitleCraft.Text = "조합식";
            // 
            // lblSubTitleLocation
            // 
            this.lblSubTitleLocation.AutoSize = true;
            this.lblSubTitleLocation.Location = new System.Drawing.Point(27, 276);
            this.lblSubTitleLocation.Name = "lblSubTitleLocation";
            this.lblSubTitleLocation.Size = new System.Drawing.Size(64, 17);
            this.lblSubTitleLocation.TabIndex = 14;
            this.lblSubTitleLocation.Text = "조합 장소";
            // 
            // lblSubTitleDescription
            // 
            this.lblSubTitleDescription.AutoSize = true;
            this.lblSubTitleDescription.Location = new System.Drawing.Point(27, 171);
            this.lblSubTitleDescription.Name = "lblSubTitleDescription";
            this.lblSubTitleDescription.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleDescription.TabIndex = 14;
            this.lblSubTitleDescription.Text = "설명";
            // 
            // txbSuccessProbability
            // 
            this.txbSuccessProbability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSuccessProbability.BackColor = System.Drawing.Color.DarkGray;
            this.txbSuccessProbability.Location = new System.Drawing.Point(686, 350);
            this.txbSuccessProbability.Name = "txbSuccessProbability";
            this.txbSuccessProbability.Size = new System.Drawing.Size(107, 23);
            this.txbSuccessProbability.TabIndex = 2;
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
            // lblSubTitleSuccessProbability
            // 
            this.lblSubTitleSuccessProbability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubTitleSuccessProbability.AutoSize = true;
            this.lblSubTitleSuccessProbability.Location = new System.Drawing.Point(565, 353);
            this.lblSubTitleSuccessProbability.Name = "lblSubTitleSuccessProbability";
            this.lblSubTitleSuccessProbability.Size = new System.Drawing.Size(103, 17);
            this.lblSubTitleSuccessProbability.TabIndex = 1;
            this.lblSubTitleSuccessProbability.Text = "#조합 성공 확률";
            // 
            // lblSubTitleName
            // 
            this.lblSubTitleName.AutoSize = true;
            this.lblSubTitleName.Location = new System.Drawing.Point(27, 100);
            this.lblSubTitleName.Name = "lblSubTitleName";
            this.lblSubTitleName.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleName.TabIndex = 1;
            this.lblSubTitleName.Text = "이름";
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
            this.pnlEditMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlEditMenu.Size = new System.Drawing.Size(746, 31);
            this.pnlEditMenu.TabIndex = 18;
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
            // btnEditRename
            // 
            this.btnEditRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnEditRename.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.btnEditRename.Location = new System.Drawing.Point(697, 127);
            this.btnEditRename.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditRename.Name = "btnEditRename";
            this.btnEditRename.Size = new System.Drawing.Size(98, 31);
            this.btnEditRename.TabIndex = 22;
            this.btnEditRename.Text = "이름 변경";
            this.btnEditRename.UseVisualStyleBackColor = false;
            // 
            // CraftRecipeEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1234, 634);
            this.Controls.Add(this.pnlBase);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Name = "CraftRecipeEditorWindow";
            this.pnlBase.Panel1.ResumeLayout(false);
            this.pnlBase.Panel1.PerformLayout();
            this.pnlBase.Panel2.ResumeLayout(false);
            this.pnlBase.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).EndInit();
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlBaseCraft.ResumeLayout(false);
            this.pnlBaseCraft.PerformLayout();
            this.pnlEditMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.SplitContainer pnlBase;
        protected System.Windows.Forms.Label lblSubTitleFilter;
        protected System.Windows.Forms.TextBox txbFilter;
        protected System.Windows.Forms.DataGridView grdList;
        protected System.Windows.Forms.Label lblTitleList;
        protected System.Windows.Forms.TextBox txbDescription;
        protected System.Windows.Forms.Label lblSubTitleDescription;
        protected System.Windows.Forms.Button btnDelete;
        protected System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.Button btnEdit;
        protected System.Windows.Forms.TextBox txbName;
        protected System.Windows.Forms.Label lblSubTitleName;
        protected System.Windows.Forms.Label lblTitleEditor;
        protected System.Windows.Forms.ComboBox cbbLocation;
        protected System.Windows.Forms.Label lblSubTitleLocation;
        private System.Windows.Forms.TableLayoutPanel pnlBaseCraft;
        protected System.Windows.Forms.Label lblSubTitleCraft;
        protected System.Windows.Forms.TextBox txbMaterials;
        protected System.Windows.Forms.Label lblSubTitleMaterials;
        private System.Windows.Forms.TableLayoutPanel pnlEditMenu;
        protected System.Windows.Forms.Button btnEditClipImage;
        protected System.Windows.Forms.Label lblVersionInfo;
        protected System.Windows.Forms.TextBox txbSuccessProbability;
        protected System.Windows.Forms.Label lblSubTitleSuccessProbability;
        protected System.Windows.Forms.Label lblSubTitleSubstituteMaterials;
        protected System.Windows.Forms.TextBox txbSubstituteMaterial;
        protected System.Windows.Forms.Button btnEditRename;
    }
}