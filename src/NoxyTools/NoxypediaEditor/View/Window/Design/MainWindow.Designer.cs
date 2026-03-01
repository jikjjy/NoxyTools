namespace NoxypediaEditor
{
    partial class MainWindow
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.pnlBase = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTopMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelectItemGrade = new System.Windows.Forms.RadioButton();
            this.btnSelectUniqueOption = new System.Windows.Forms.RadioButton();
            this.btnSelectItem = new System.Windows.Forms.RadioButton();
            this.btnSelectCreep = new System.Windows.Forms.RadioButton();
            this.btnSelectLocation = new System.Windows.Forms.RadioButton();
            this.btnSelectRegion = new System.Windows.Forms.RadioButton();
            this.btnSelectCraftRecipe = new System.Windows.Forms.RadioButton();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBottomMenu = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBase.SuspendLayout();
            this.pnlTopMenu.SuspendLayout();
            this.pnlBottomMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBase.ColumnCount = 1;
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBase.Controls.Add(this.pnlTopMenu, 0, 0);
            this.pnlBase.Controls.Add(this.pnlContent, 0, 1);
            this.pnlBase.Controls.Add(this.pnlBottomMenu, 0, 2);
            this.pnlBase.Location = new System.Drawing.Point(-1, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.RowCount = 3;
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlBase.Size = new System.Drawing.Size(1116, 539);
            this.pnlBase.TabIndex = 1;
            // 
            // pnlTopMenu
            // 
            this.pnlTopMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlTopMenu.Controls.Add(this.btnSelectItemGrade);
            this.pnlTopMenu.Controls.Add(this.btnSelectUniqueOption);
            this.pnlTopMenu.Controls.Add(this.btnSelectItem);
            this.pnlTopMenu.Controls.Add(this.btnSelectCreep);
            this.pnlTopMenu.Controls.Add(this.btnSelectLocation);
            this.pnlTopMenu.Controls.Add(this.btnSelectRegion);
            this.pnlTopMenu.Controls.Add(this.btnSelectCraftRecipe);
            this.pnlTopMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlTopMenu.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTopMenu.Name = "pnlTopMenu";
            this.pnlTopMenu.Size = new System.Drawing.Size(1116, 40);
            this.pnlTopMenu.TabIndex = 2;
            // 
            // btnSelectItemGrade
            // 
            this.btnSelectItemGrade.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectItemGrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectItemGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectItemGrade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectItemGrade.Location = new System.Drawing.Point(3, 5);
            this.btnSelectItemGrade.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectItemGrade.Name = "btnSelectItemGrade";
            this.btnSelectItemGrade.Size = new System.Drawing.Size(119, 32);
            this.btnSelectItemGrade.TabIndex = 0;
            this.btnSelectItemGrade.TabStop = true;
            this.btnSelectItemGrade.Text = "아이템 등급";
            this.btnSelectItemGrade.UseVisualStyleBackColor = false;
            // 
            // btnSelectUniqueOption
            // 
            this.btnSelectUniqueOption.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectUniqueOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectUniqueOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectUniqueOption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectUniqueOption.Location = new System.Drawing.Point(128, 5);
            this.btnSelectUniqueOption.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectUniqueOption.Name = "btnSelectUniqueOption";
            this.btnSelectUniqueOption.Size = new System.Drawing.Size(119, 32);
            this.btnSelectUniqueOption.TabIndex = 1;
            this.btnSelectUniqueOption.TabStop = true;
            this.btnSelectUniqueOption.Text = "고유 옵션";
            this.btnSelectUniqueOption.UseVisualStyleBackColor = false;
            // 
            // btnSelectItem
            // 
            this.btnSelectItem.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectItem.Location = new System.Drawing.Point(253, 5);
            this.btnSelectItem.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectItem.Name = "btnSelectItem";
            this.btnSelectItem.Size = new System.Drawing.Size(119, 32);
            this.btnSelectItem.TabIndex = 2;
            this.btnSelectItem.TabStop = true;
            this.btnSelectItem.Text = "아이템";
            this.btnSelectItem.UseVisualStyleBackColor = false;
            // 
            // btnSelectCreep
            // 
            this.btnSelectCreep.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectCreep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectCreep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCreep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectCreep.Location = new System.Drawing.Point(378, 5);
            this.btnSelectCreep.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectCreep.Name = "btnSelectCreep";
            this.btnSelectCreep.Size = new System.Drawing.Size(119, 32);
            this.btnSelectCreep.TabIndex = 6;
            this.btnSelectCreep.TabStop = true;
            this.btnSelectCreep.Text = "크리쳐";
            this.btnSelectCreep.UseVisualStyleBackColor = false;
            // 
            // btnSelectLocation
            // 
            this.btnSelectLocation.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectLocation.Location = new System.Drawing.Point(503, 5);
            this.btnSelectLocation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectLocation.Name = "btnSelectLocation";
            this.btnSelectLocation.Size = new System.Drawing.Size(119, 32);
            this.btnSelectLocation.TabIndex = 4;
            this.btnSelectLocation.TabStop = true;
            this.btnSelectLocation.Text = "장소";
            this.btnSelectLocation.UseVisualStyleBackColor = false;
            // 
            // btnSelectRegion
            // 
            this.btnSelectRegion.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectRegion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectRegion.Location = new System.Drawing.Point(628, 5);
            this.btnSelectRegion.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectRegion.Name = "btnSelectRegion";
            this.btnSelectRegion.Size = new System.Drawing.Size(119, 32);
            this.btnSelectRegion.TabIndex = 3;
            this.btnSelectRegion.TabStop = true;
            this.btnSelectRegion.Text = "지역";
            this.btnSelectRegion.UseVisualStyleBackColor = false;
            // 
            // btnSelectCraftRecipe
            // 
            this.btnSelectCraftRecipe.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectCraftRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSelectCraftRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCraftRecipe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnSelectCraftRecipe.Location = new System.Drawing.Point(753, 5);
            this.btnSelectCraftRecipe.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnSelectCraftRecipe.Name = "btnSelectCraftRecipe";
            this.btnSelectCraftRecipe.Size = new System.Drawing.Size(119, 32);
            this.btnSelectCraftRecipe.TabIndex = 5;
            this.btnSelectCraftRecipe.TabStop = true;
            this.btnSelectCraftRecipe.Text = "조합식";
            this.btnSelectCraftRecipe.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 40);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1116, 459);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlBottomMenu
            // 
            this.pnlBottomMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlBottomMenu.Controls.Add(this.btnTest);
            this.pnlBottomMenu.Controls.Add(this.chkTopMost);
            this.pnlBottomMenu.Controls.Add(this.btnExport);
            this.pnlBottomMenu.Controls.Add(this.btnDownload);
            this.pnlBottomMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.pnlBottomMenu.Location = new System.Drawing.Point(0, 499);
            this.pnlBottomMenu.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottomMenu.Name = "pnlBottomMenu";
            this.pnlBottomMenu.Size = new System.Drawing.Size(1116, 40);
            this.pnlBottomMenu.TabIndex = 2;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnTest.Location = new System.Drawing.Point(128, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(175, 34);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Function Test";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(13, 11);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(113, 21);
            this.chkTopMost.TabIndex = 2;
            this.chkTopMost.Text = "항상 위에 표시";
            this.chkTopMost.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(85)))), ((int)(((byte)(37)))));
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(207)))), ((int)(((byte)(226)))));
            this.btnExport.Location = new System.Drawing.Point(437, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(218, 34);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "파일로 내보내기 (.dat)";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnDownload.Location = new System.Drawing.Point(661, 3);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(218, 34);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "파일에서 불러오기";
            this.btnDownload.UseVisualStyleBackColor = false;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1114, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(12, 17);
            this.lblStatus.Text = "-";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1114, 562);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Noxypedia Editor";
            this.pnlBase.ResumeLayout(false);
            this.pnlTopMenu.ResumeLayout(false);
            this.pnlBottomMenu.ResumeLayout(false);
            this.pnlBottomMenu.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel pnlBase;
        protected System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.FlowLayoutPanel pnlTopMenu;
        protected System.Windows.Forms.RadioButton btnSelectItemGrade;
        protected System.Windows.Forms.RadioButton btnSelectUniqueOption;
        protected System.Windows.Forms.RadioButton btnSelectItem;
        protected System.Windows.Forms.RadioButton btnSelectRegion;
        protected System.Windows.Forms.RadioButton btnSelectLocation;
        protected System.Windows.Forms.RadioButton btnSelectCraftRecipe;
        protected System.Windows.Forms.RadioButton btnSelectCreep;
        protected System.Windows.Forms.StatusStrip statusStrip;
        protected System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel pnlBottomMenu;
        protected System.Windows.Forms.CheckBox chkTopMost;
        protected System.Windows.Forms.Button btnExport;
        protected System.Windows.Forms.Button btnDownload;
        protected System.Windows.Forms.Button btnTest;
    }
}

