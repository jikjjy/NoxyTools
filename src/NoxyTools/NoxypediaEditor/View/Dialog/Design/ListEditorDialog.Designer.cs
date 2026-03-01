namespace NoxypediaEditor
{
    partial class ListEditorDialog
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
            this.pnlBaseLeftList = new System.Windows.Forms.Panel();
            this.pnlBaseLeftFilter = new System.Windows.Forms.Panel();
            this.txbLeftFilter = new System.Windows.Forms.TextBox();
            this.lblSubTitleLeftFilter = new System.Windows.Forms.Label();
            this.grdLeft = new System.Windows.Forms.DataGridView();
            this.pnlBaseRightList = new System.Windows.Forms.Panel();
            this.pnlBaseRightFilter = new System.Windows.Forms.Panel();
            this.txbRightFilter = new System.Windows.Forms.TextBox();
            this.lblSubTitleRightFilter = new System.Windows.Forms.Label();
            this.grdRight = new System.Windows.Forms.DataGridView();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlTitleLeftList = new System.Windows.Forms.Panel();
            this.lblTitleLeft = new System.Windows.Forms.Label();
            this.pnlTitleRightList = new System.Windows.Forms.Panel();
            this.lblTitleRight = new System.Windows.Forms.Label();
            this.pnlBase.SuspendLayout();
            this.pnlBaseLeftList.SuspendLayout();
            this.pnlBaseLeftFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLeft)).BeginInit();
            this.pnlBaseRightList.SuspendLayout();
            this.pnlBaseRightFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRight)).BeginInit();
            this.pnlMenu.SuspendLayout();
            this.pnlTitleLeftList.SuspendLayout();
            this.pnlTitleRightList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.ColumnCount = 3;
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.pnlBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBase.Controls.Add(this.pnlBaseLeftList, 0, 1);
            this.pnlBase.Controls.Add(this.pnlBaseRightList, 2, 1);
            this.pnlBase.Controls.Add(this.pnlMenu, 0, 3);
            this.pnlBase.Controls.Add(this.btnAdd, 1, 1);
            this.pnlBase.Controls.Add(this.btnDelete, 1, 2);
            this.pnlBase.Controls.Add(this.pnlTitleLeftList, 0, 0);
            this.pnlBase.Controls.Add(this.pnlTitleRightList, 2, 0);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.RowCount = 4;
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.pnlBase.Size = new System.Drawing.Size(643, 479);
            this.pnlBase.TabIndex = 0;
            // 
            // pnlBaseLeftList
            // 
            this.pnlBaseLeftList.Controls.Add(this.pnlBaseLeftFilter);
            this.pnlBaseLeftList.Controls.Add(this.grdLeft);
            this.pnlBaseLeftList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBaseLeftList.Location = new System.Drawing.Point(3, 53);
            this.pnlBaseLeftList.Name = "pnlBaseLeftList";
            this.pnlBase.SetRowSpan(this.pnlBaseLeftList, 2);
            this.pnlBaseLeftList.Size = new System.Drawing.Size(290, 372);
            this.pnlBaseLeftList.TabIndex = 0;
            // 
            // pnlBaseLeftFilter
            // 
            this.pnlBaseLeftFilter.Controls.Add(this.txbLeftFilter);
            this.pnlBaseLeftFilter.Controls.Add(this.lblSubTitleLeftFilter);
            this.pnlBaseLeftFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBaseLeftFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlBaseLeftFilter.Name = "pnlBaseLeftFilter";
            this.pnlBaseLeftFilter.Size = new System.Drawing.Size(290, 30);
            this.pnlBaseLeftFilter.TabIndex = 1;
            // 
            // txbLeftFilter
            // 
            this.txbLeftFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLeftFilter.Location = new System.Drawing.Point(56, 3);
            this.txbLeftFilter.Name = "txbLeftFilter";
            this.txbLeftFilter.Size = new System.Drawing.Size(232, 23);
            this.txbLeftFilter.TabIndex = 1;
            // 
            // lblSubTitleLeftFilter
            // 
            this.lblSubTitleLeftFilter.AutoSize = true;
            this.lblSubTitleLeftFilter.Location = new System.Drawing.Point(3, 6);
            this.lblSubTitleLeftFilter.Name = "lblSubTitleLeftFilter";
            this.lblSubTitleLeftFilter.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleLeftFilter.TabIndex = 0;
            this.lblSubTitleLeftFilter.Text = "필터";
            // 
            // grdLeft
            // 
            this.grdLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLeft.Location = new System.Drawing.Point(0, 32);
            this.grdLeft.Name = "grdLeft";
            this.grdLeft.RowTemplate.Height = 23;
            this.grdLeft.Size = new System.Drawing.Size(290, 340);
            this.grdLeft.TabIndex = 0;
            // 
            // pnlBaseRightList
            // 
            this.pnlBaseRightList.Controls.Add(this.pnlBaseRightFilter);
            this.pnlBaseRightList.Controls.Add(this.grdRight);
            this.pnlBaseRightList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBaseRightList.Location = new System.Drawing.Point(349, 53);
            this.pnlBaseRightList.Name = "pnlBaseRightList";
            this.pnlBase.SetRowSpan(this.pnlBaseRightList, 2);
            this.pnlBaseRightList.Size = new System.Drawing.Size(291, 372);
            this.pnlBaseRightList.TabIndex = 1;
            // 
            // pnlBaseRightFilter
            // 
            this.pnlBaseRightFilter.Controls.Add(this.txbRightFilter);
            this.pnlBaseRightFilter.Controls.Add(this.lblSubTitleRightFilter);
            this.pnlBaseRightFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBaseRightFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlBaseRightFilter.Name = "pnlBaseRightFilter";
            this.pnlBaseRightFilter.Size = new System.Drawing.Size(291, 30);
            this.pnlBaseRightFilter.TabIndex = 2;
            // 
            // txbRightFilter
            // 
            this.txbRightFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbRightFilter.Location = new System.Drawing.Point(56, 3);
            this.txbRightFilter.Name = "txbRightFilter";
            this.txbRightFilter.Size = new System.Drawing.Size(232, 23);
            this.txbRightFilter.TabIndex = 1;
            // 
            // lblSubTitleRightFilter
            // 
            this.lblSubTitleRightFilter.AutoSize = true;
            this.lblSubTitleRightFilter.Location = new System.Drawing.Point(3, 6);
            this.lblSubTitleRightFilter.Name = "lblSubTitleRightFilter";
            this.lblSubTitleRightFilter.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleRightFilter.TabIndex = 0;
            this.lblSubTitleRightFilter.Text = "필터";
            // 
            // grdRight
            // 
            this.grdRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRight.Location = new System.Drawing.Point(0, 32);
            this.grdRight.Name = "grdRight";
            this.grdRight.RowTemplate.Height = 23;
            this.grdRight.Size = new System.Drawing.Size(291, 340);
            this.grdRight.TabIndex = 1;
            // 
            // pnlMenu
            // 
            this.pnlBase.SetColumnSpan(this.pnlMenu, 3);
            this.pnlMenu.Controls.Add(this.btnOk);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenu.Location = new System.Drawing.Point(3, 431);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(637, 45);
            this.pnlMenu.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(637, 45);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnAdd.Location = new System.Drawing.Point(299, 53);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 183);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "─▶";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.ForeColor = System.Drawing.Color.DarkRed;
            this.btnDelete.Location = new System.Drawing.Point(299, 242);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 183);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "◀─";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // pnlTitleLeftList
            // 
            this.pnlTitleLeftList.Controls.Add(this.lblTitleLeft);
            this.pnlTitleLeftList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitleLeftList.Location = new System.Drawing.Point(3, 3);
            this.pnlTitleLeftList.Name = "pnlTitleLeftList";
            this.pnlTitleLeftList.Size = new System.Drawing.Size(290, 44);
            this.pnlTitleLeftList.TabIndex = 6;
            // 
            // lblTitleLeft
            // 
            this.lblTitleLeft.AutoSize = true;
            this.lblTitleLeft.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleLeft.Location = new System.Drawing.Point(49, 14);
            this.lblTitleLeft.Name = "lblTitleLeft";
            this.lblTitleLeft.Size = new System.Drawing.Size(42, 19);
            this.lblTitleLeft.TabIndex = 0;
            this.lblTitleLeft.Text = "TITLE";
            // 
            // pnlTitleRightList
            // 
            this.pnlTitleRightList.Controls.Add(this.lblTitleRight);
            this.pnlTitleRightList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitleRightList.Location = new System.Drawing.Point(349, 3);
            this.pnlTitleRightList.Name = "pnlTitleRightList";
            this.pnlTitleRightList.Size = new System.Drawing.Size(291, 44);
            this.pnlTitleRightList.TabIndex = 7;
            // 
            // lblTitleRight
            // 
            this.lblTitleRight.AutoSize = true;
            this.lblTitleRight.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleRight.Location = new System.Drawing.Point(49, 14);
            this.lblTitleRight.Name = "lblTitleRight";
            this.lblTitleRight.Size = new System.Drawing.Size(42, 19);
            this.lblTitleRight.TabIndex = 1;
            this.lblTitleRight.Text = "TITLE";
            // 
            // ListEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(643, 479);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ListEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Noxypedia Editor";
            this.pnlBase.ResumeLayout(false);
            this.pnlBaseLeftList.ResumeLayout(false);
            this.pnlBaseLeftFilter.ResumeLayout(false);
            this.pnlBaseLeftFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLeft)).EndInit();
            this.pnlBaseRightList.ResumeLayout(false);
            this.pnlBaseRightFilter.ResumeLayout(false);
            this.pnlBaseRightFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRight)).EndInit();
            this.pnlMenu.ResumeLayout(false);
            this.pnlTitleLeftList.ResumeLayout(false);
            this.pnlTitleLeftList.PerformLayout();
            this.pnlTitleRightList.ResumeLayout(false);
            this.pnlTitleRightList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.TableLayoutPanel pnlBase;
        protected System.Windows.Forms.Panel pnlBaseLeftList;
        protected System.Windows.Forms.Panel pnlBaseRightList;
        protected System.Windows.Forms.Panel pnlMenu;
        protected System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.Button btnDelete;
        protected System.Windows.Forms.Panel pnlTitleLeftList;
        protected System.Windows.Forms.Panel pnlTitleRightList;
        protected System.Windows.Forms.Button btnOk;
        protected System.Windows.Forms.Label lblTitleLeft;
        protected System.Windows.Forms.Label lblTitleRight;
        protected System.Windows.Forms.DataGridView grdLeft;
        protected System.Windows.Forms.DataGridView grdRight;
        protected System.Windows.Forms.Panel pnlBaseLeftFilter;
        protected System.Windows.Forms.TextBox txbLeftFilter;
        protected System.Windows.Forms.Label lblSubTitleLeftFilter;
        protected System.Windows.Forms.Panel pnlBaseRightFilter;
        protected System.Windows.Forms.TextBox txbRightFilter;
        protected System.Windows.Forms.Label lblSubTitleRightFilter;
    }
}