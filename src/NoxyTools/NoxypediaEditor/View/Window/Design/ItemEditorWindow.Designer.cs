namespace NoxypediaEditor
{
    partial class ItemEditorWindow
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
            this.pnlEditMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditClipImage = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.pnlBaseCraft = new System.Windows.Forms.TableLayoutPanel();
            this.lblCraftRecipePreview = new System.Windows.Forms.Label();
            this.lblSubTitleCraftResult = new System.Windows.Forms.Label();
            this.lblSubTitleCraftRecipe = new System.Windows.Forms.Label();
            this.txbCraftResult = new System.Windows.Forms.TextBox();
            this.cbbCraftRecipe = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInformationBase = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbbItemPart = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlContainerWearableClass = new System.Windows.Forms.FlowLayoutPanel();
            this.chkClassKnight = new System.Windows.Forms.CheckBox();
            this.chkClassWizard = new System.Windows.Forms.CheckBox();
            this.chkClassPriest = new System.Windows.Forms.CheckBox();
            this.chkClassArcher = new System.Windows.Forms.CheckBox();
            this.chkClassDruid = new System.Windows.Forms.CheckBox();
            this.chkClassSummoner = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbbItemGrade = new System.Windows.Forms.ComboBox();
            this.pnlItemOptions = new System.Windows.Forms.TableLayoutPanel();
            this.txbInteligence = new System.Windows.Forms.TextBox();
            this.txbAgility = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txbStrength = new System.Windows.Forms.TextBox();
            this.txbHP = new System.Windows.Forms.TextBox();
            this.txbMP = new System.Windows.Forms.TextBox();
            this.txbAttack = new System.Windows.Forms.TextBox();
            this.txbArmor = new System.Windows.Forms.TextBox();
            this.txbUniqueOptions = new System.Windows.Forms.TextBox();
            this.chkUnidentifiedItem = new System.Windows.Forms.CheckBox();
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
            this.pnlBaseCraft.SuspendLayout();
            this.pnlInformationBase.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlContainerWearableClass.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlItemOptions.SuspendLayout();
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
            this.pnlBase.Panel2.Controls.Add(this.pnlEditMenu);
            this.pnlBase.Panel2.Controls.Add(this.pnlBaseCraft);
            this.pnlBase.Panel2.Controls.Add(this.label9);
            this.pnlBase.Panel2.Controls.Add(this.label14);
            this.pnlBase.Panel2.Controls.Add(this.label1);
            this.pnlBase.Panel2.Controls.Add(this.pnlInformationBase);
            this.pnlBase.Panel2.Controls.Add(this.pnlItemOptions);
            this.pnlBase.Panel2.Controls.Add(this.chkUnidentifiedItem);
            this.pnlBase.Panel2.Controls.Add(this.txbName);
            this.pnlBase.Panel2.Controls.Add(this.lblSubTitleName);
            this.pnlBase.Panel2.Controls.Add(this.lblTitleEditor);
            this.pnlBase.Size = new System.Drawing.Size(1234, 634);
            this.pnlBase.SplitterDistance = 411;
            this.pnlBase.TabIndex = 1;
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
            this.lblTitleList.Size = new System.Drawing.Size(97, 19);
            this.lblTitleList.TabIndex = 0;
            this.lblTitleList.Text = "아이템 리스트";
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblVersionInfo.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionInfo.Location = new System.Drawing.Point(0, 607);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(819, 27);
            this.lblVersionInfo.TabIndex = 20;
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
            // pnlBaseCraft
            // 
            this.pnlBaseCraft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBaseCraft.ColumnCount = 3;
            this.pnlBaseCraft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlBaseCraft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.pnlBaseCraft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBaseCraft.Controls.Add(this.lblCraftRecipePreview, 2, 0);
            this.pnlBaseCraft.Controls.Add(this.lblSubTitleCraftResult, 0, 1);
            this.pnlBaseCraft.Controls.Add(this.lblSubTitleCraftRecipe, 0, 0);
            this.pnlBaseCraft.Controls.Add(this.txbCraftResult, 1, 1);
            this.pnlBaseCraft.Controls.Add(this.cbbCraftRecipe, 1, 0);
            this.pnlBaseCraft.Location = new System.Drawing.Point(49, 300);
            this.pnlBaseCraft.Name = "pnlBaseCraft";
            this.pnlBaseCraft.RowCount = 2;
            this.pnlBaseCraft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlBaseCraft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBaseCraft.Size = new System.Drawing.Size(746, 91);
            this.pnlBaseCraft.TabIndex = 16;
            // 
            // lblCraftRecipePreview
            // 
            this.lblCraftRecipePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCraftRecipePreview.Location = new System.Drawing.Point(283, 3);
            this.lblCraftRecipePreview.Margin = new System.Windows.Forms.Padding(3);
            this.lblCraftRecipePreview.Name = "lblCraftRecipePreview";
            this.lblCraftRecipePreview.Size = new System.Drawing.Size(460, 29);
            this.lblCraftRecipePreview.TabIndex = 21;
            this.lblCraftRecipePreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSubTitleCraftResult
            // 
            this.lblSubTitleCraftResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTitleCraftResult.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSubTitleCraftResult.Location = new System.Drawing.Point(3, 38);
            this.lblSubTitleCraftResult.Margin = new System.Windows.Forms.Padding(3);
            this.lblSubTitleCraftResult.Name = "lblSubTitleCraftResult";
            this.lblSubTitleCraftResult.Size = new System.Drawing.Size(94, 50);
            this.lblSubTitleCraftResult.TabIndex = 18;
            this.lblSubTitleCraftResult.Text = "# 조합 결과";
            this.lblSubTitleCraftResult.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSubTitleCraftRecipe
            // 
            this.lblSubTitleCraftRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTitleCraftRecipe.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSubTitleCraftRecipe.Location = new System.Drawing.Point(3, 3);
            this.lblSubTitleCraftRecipe.Margin = new System.Windows.Forms.Padding(3);
            this.lblSubTitleCraftRecipe.Name = "lblSubTitleCraftRecipe";
            this.lblSubTitleCraftRecipe.Size = new System.Drawing.Size(94, 29);
            this.lblSubTitleCraftRecipe.TabIndex = 17;
            this.lblSubTitleCraftRecipe.Text = "# 레시피";
            this.lblSubTitleCraftRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbCraftResult
            // 
            this.txbCraftResult.BackColor = System.Drawing.Color.DarkGray;
            this.pnlBaseCraft.SetColumnSpan(this.txbCraftResult, 2);
            this.txbCraftResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbCraftResult.Location = new System.Drawing.Point(103, 38);
            this.txbCraftResult.Multiline = true;
            this.txbCraftResult.Name = "txbCraftResult";
            this.txbCraftResult.ReadOnly = true;
            this.txbCraftResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbCraftResult.Size = new System.Drawing.Size(640, 50);
            this.txbCraftResult.TabIndex = 19;
            // 
            // cbbCraftRecipe
            // 
            this.cbbCraftRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCraftRecipe.FormattingEnabled = true;
            this.cbbCraftRecipe.Location = new System.Drawing.Point(103, 6);
            this.cbbCraftRecipe.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cbbCraftRecipe.Name = "cbbCraftRecipe";
            this.cbbCraftRecipe.Size = new System.Drawing.Size(174, 25);
            this.cbbCraftRecipe.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 403);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "옵션";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 272);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 17);
            this.label14.TabIndex = 14;
            this.label14.Text = "조합 정보";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "정보";
            // 
            // pnlInformationBase
            // 
            this.pnlInformationBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInformationBase.ColumnCount = 4;
            this.pnlInformationBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlInformationBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlInformationBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlInformationBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlInformationBase.Controls.Add(this.panel2, 3, 0);
            this.pnlInformationBase.Controls.Add(this.label4, 0, 1);
            this.pnlInformationBase.Controls.Add(this.label2, 0, 0);
            this.pnlInformationBase.Controls.Add(this.label3, 2, 0);
            this.pnlInformationBase.Controls.Add(this.pnlContainerWearableClass, 1, 1);
            this.pnlInformationBase.Controls.Add(this.panel1, 1, 0);
            this.pnlInformationBase.Location = new System.Drawing.Point(49, 191);
            this.pnlInformationBase.Name = "pnlInformationBase";
            this.pnlInformationBase.RowCount = 2;
            this.pnlInformationBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlInformationBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlInformationBase.Size = new System.Drawing.Size(746, 69);
            this.pnlInformationBase.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbbItemPart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(476, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(267, 28);
            this.panel2.TabIndex = 20;
            // 
            // cbbItemPart
            // 
            this.cbbItemPart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbbItemPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbItemPart.FormattingEnabled = true;
            this.cbbItemPart.Location = new System.Drawing.Point(0, 3);
            this.cbbItemPart.Name = "cbbItemPart";
            this.cbbItemPart.Size = new System.Drawing.Size(267, 25);
            this.cbbItemPart.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label4.Location = new System.Drawing.Point(3, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 29);
            this.label4.TabIndex = 17;
            this.label4.Text = "# 착용 가능";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 28);
            this.label2.TabIndex = 15;
            this.label2.Text = "# 등급";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Location = new System.Drawing.Point(376, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 28);
            this.label3.TabIndex = 16;
            this.label3.Text = "# 부위";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlContainerWearableClass
            // 
            this.pnlContainerWearableClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInformationBase.SetColumnSpan(this.pnlContainerWearableClass, 3);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassKnight);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassWizard);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassPriest);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassArcher);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassDruid);
            this.pnlContainerWearableClass.Controls.Add(this.chkClassSummoner);
            this.pnlContainerWearableClass.Location = new System.Drawing.Point(103, 37);
            this.pnlContainerWearableClass.Name = "pnlContainerWearableClass";
            this.pnlContainerWearableClass.Size = new System.Drawing.Size(640, 29);
            this.pnlContainerWearableClass.TabIndex = 18;
            // 
            // chkClassKnight
            // 
            this.chkClassKnight.AutoSize = true;
            this.chkClassKnight.Location = new System.Drawing.Point(3, 6);
            this.chkClassKnight.Margin = new System.Windows.Forms.Padding(3, 6, 6, 6);
            this.chkClassKnight.Name = "chkClassKnight";
            this.chkClassKnight.Size = new System.Drawing.Size(53, 21);
            this.chkClassKnight.TabIndex = 0;
            this.chkClassKnight.Text = "기사";
            this.chkClassKnight.UseVisualStyleBackColor = true;
            // 
            // chkClassWizard
            // 
            this.chkClassWizard.AutoSize = true;
            this.chkClassWizard.Location = new System.Drawing.Point(68, 6);
            this.chkClassWizard.Margin = new System.Windows.Forms.Padding(6);
            this.chkClassWizard.Name = "chkClassWizard";
            this.chkClassWizard.Size = new System.Drawing.Size(66, 21);
            this.chkClassWizard.TabIndex = 1;
            this.chkClassWizard.Text = "마법사";
            this.chkClassWizard.UseVisualStyleBackColor = true;
            // 
            // chkClassPriest
            // 
            this.chkClassPriest.AutoSize = true;
            this.chkClassPriest.Location = new System.Drawing.Point(146, 6);
            this.chkClassPriest.Margin = new System.Windows.Forms.Padding(6);
            this.chkClassPriest.Name = "chkClassPriest";
            this.chkClassPriest.Size = new System.Drawing.Size(66, 21);
            this.chkClassPriest.TabIndex = 2;
            this.chkClassPriest.Text = "성직자";
            this.chkClassPriest.UseVisualStyleBackColor = true;
            // 
            // chkClassArcher
            // 
            this.chkClassArcher.AutoSize = true;
            this.chkClassArcher.Location = new System.Drawing.Point(224, 6);
            this.chkClassArcher.Margin = new System.Windows.Forms.Padding(6);
            this.chkClassArcher.Name = "chkClassArcher";
            this.chkClassArcher.Size = new System.Drawing.Size(53, 21);
            this.chkClassArcher.TabIndex = 3;
            this.chkClassArcher.Text = "궁수";
            this.chkClassArcher.UseVisualStyleBackColor = true;
            // 
            // chkClassDruid
            // 
            this.chkClassDruid.AutoSize = true;
            this.chkClassDruid.Location = new System.Drawing.Point(289, 6);
            this.chkClassDruid.Margin = new System.Windows.Forms.Padding(6);
            this.chkClassDruid.Name = "chkClassDruid";
            this.chkClassDruid.Size = new System.Drawing.Size(79, 21);
            this.chkClassDruid.TabIndex = 4;
            this.chkClassDruid.Text = "드루이드";
            this.chkClassDruid.UseVisualStyleBackColor = true;
            // 
            // chkClassSummoner
            // 
            this.chkClassSummoner.AutoSize = true;
            this.chkClassSummoner.Location = new System.Drawing.Point(380, 6);
            this.chkClassSummoner.Margin = new System.Windows.Forms.Padding(6);
            this.chkClassSummoner.Name = "chkClassSummoner";
            this.chkClassSummoner.Size = new System.Drawing.Size(66, 21);
            this.chkClassSummoner.TabIndex = 5;
            this.chkClassSummoner.Text = "용술사";
            this.chkClassSummoner.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbbItemGrade);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(103, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 28);
            this.panel1.TabIndex = 19;
            // 
            // cbbItemGrade
            // 
            this.cbbItemGrade.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbbItemGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbItemGrade.FormattingEnabled = true;
            this.cbbItemGrade.Location = new System.Drawing.Point(0, 3);
            this.cbbItemGrade.Name = "cbbItemGrade";
            this.cbbItemGrade.Size = new System.Drawing.Size(267, 25);
            this.cbbItemGrade.TabIndex = 0;
            // 
            // pnlItemOptions
            // 
            this.pnlItemOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlItemOptions.ColumnCount = 8;
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlItemOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlItemOptions.Controls.Add(this.txbInteligence, 5, 0);
            this.pnlItemOptions.Controls.Add(this.txbAgility, 3, 0);
            this.pnlItemOptions.Controls.Add(this.label7, 0, 2);
            this.pnlItemOptions.Controls.Add(this.label5, 0, 0);
            this.pnlItemOptions.Controls.Add(this.label6, 2, 0);
            this.pnlItemOptions.Controls.Add(this.label8, 4, 0);
            this.pnlItemOptions.Controls.Add(this.label10, 0, 1);
            this.pnlItemOptions.Controls.Add(this.label11, 2, 1);
            this.pnlItemOptions.Controls.Add(this.label12, 4, 1);
            this.pnlItemOptions.Controls.Add(this.label13, 6, 1);
            this.pnlItemOptions.Controls.Add(this.txbStrength, 1, 0);
            this.pnlItemOptions.Controls.Add(this.txbHP, 1, 1);
            this.pnlItemOptions.Controls.Add(this.txbMP, 3, 1);
            this.pnlItemOptions.Controls.Add(this.txbAttack, 5, 1);
            this.pnlItemOptions.Controls.Add(this.txbArmor, 7, 1);
            this.pnlItemOptions.Controls.Add(this.txbUniqueOptions, 1, 2);
            this.pnlItemOptions.Location = new System.Drawing.Point(49, 456);
            this.pnlItemOptions.Name = "pnlItemOptions";
            this.pnlItemOptions.RowCount = 3;
            this.pnlItemOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlItemOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlItemOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlItemOptions.Size = new System.Drawing.Size(746, 148);
            this.pnlItemOptions.TabIndex = 12;
            // 
            // txbInteligence
            // 
            this.txbInteligence.BackColor = System.Drawing.Color.DarkGray;
            this.txbInteligence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbInteligence.Location = new System.Drawing.Point(415, 3);
            this.txbInteligence.Name = "txbInteligence";
            this.txbInteligence.Size = new System.Drawing.Size(140, 23);
            this.txbInteligence.TabIndex = 28;
            // 
            // txbAgility
            // 
            this.txbAgility.BackColor = System.Drawing.Color.DarkGray;
            this.txbAgility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbAgility.Location = new System.Drawing.Point(229, 3);
            this.txbAgility.Name = "txbAgility";
            this.txbAgility.Size = new System.Drawing.Size(140, 23);
            this.txbAgility.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 82);
            this.label7.TabIndex = 18;
            this.label7.Text = "#\r\n고\r\n유\r\n옵\r\n션";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "# 힘";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(189, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "# 민";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(375, 3);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 24);
            this.label8.TabIndex = 19;
            this.label8.Text = "# 지";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 33);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 24);
            this.label10.TabIndex = 21;
            this.label10.Text = "# 체";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(189, 33);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 24);
            this.label11.TabIndex = 22;
            this.label11.Text = "# 마";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(375, 33);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 24);
            this.label12.TabIndex = 23;
            this.label12.Text = "# 공";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(561, 33);
            this.label13.Margin = new System.Windows.Forms.Padding(3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 24);
            this.label13.TabIndex = 24;
            this.label13.Text = "# 방";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbStrength
            // 
            this.txbStrength.BackColor = System.Drawing.Color.DarkGray;
            this.txbStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbStrength.Location = new System.Drawing.Point(43, 3);
            this.txbStrength.Name = "txbStrength";
            this.txbStrength.Size = new System.Drawing.Size(140, 23);
            this.txbStrength.TabIndex = 26;
            // 
            // txbHP
            // 
            this.txbHP.BackColor = System.Drawing.Color.DarkGray;
            this.txbHP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbHP.Location = new System.Drawing.Point(43, 33);
            this.txbHP.Name = "txbHP";
            this.txbHP.Size = new System.Drawing.Size(140, 23);
            this.txbHP.TabIndex = 29;
            // 
            // txbMP
            // 
            this.txbMP.BackColor = System.Drawing.Color.DarkGray;
            this.txbMP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbMP.Location = new System.Drawing.Point(229, 33);
            this.txbMP.Name = "txbMP";
            this.txbMP.Size = new System.Drawing.Size(140, 23);
            this.txbMP.TabIndex = 30;
            // 
            // txbAttack
            // 
            this.txbAttack.BackColor = System.Drawing.Color.DarkGray;
            this.txbAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbAttack.Location = new System.Drawing.Point(415, 33);
            this.txbAttack.Name = "txbAttack";
            this.txbAttack.Size = new System.Drawing.Size(140, 23);
            this.txbAttack.TabIndex = 31;
            // 
            // txbArmor
            // 
            this.txbArmor.BackColor = System.Drawing.Color.DarkGray;
            this.txbArmor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbArmor.Location = new System.Drawing.Point(601, 33);
            this.txbArmor.Name = "txbArmor";
            this.txbArmor.Size = new System.Drawing.Size(142, 23);
            this.txbArmor.TabIndex = 32;
            // 
            // txbUniqueOptions
            // 
            this.txbUniqueOptions.BackColor = System.Drawing.Color.DarkGray;
            this.pnlItemOptions.SetColumnSpan(this.txbUniqueOptions, 7);
            this.txbUniqueOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbUniqueOptions.Location = new System.Drawing.Point(43, 63);
            this.txbUniqueOptions.Multiline = true;
            this.txbUniqueOptions.Name = "txbUniqueOptions";
            this.txbUniqueOptions.ReadOnly = true;
            this.txbUniqueOptions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbUniqueOptions.Size = new System.Drawing.Size(700, 82);
            this.txbUniqueOptions.TabIndex = 33;
            // 
            // chkUnidentifiedItem
            // 
            this.chkUnidentifiedItem.AutoSize = true;
            this.chkUnidentifiedItem.Location = new System.Drawing.Point(49, 426);
            this.chkUnidentifiedItem.Name = "chkUnidentifiedItem";
            this.chkUnidentifiedItem.Size = new System.Drawing.Size(109, 21);
            this.chkUnidentifiedItem.TabIndex = 11;
            this.chkUnidentifiedItem.Text = "미확인 아이템";
            this.chkUnidentifiedItem.UseVisualStyleBackColor = true;
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
            // ItemEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1234, 634);
            this.Controls.Add(this.pnlBase);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Name = "ItemEditorWindow";
            this.pnlBase.Panel1.ResumeLayout(false);
            this.pnlBase.Panel1.PerformLayout();
            this.pnlBase.Panel2.ResumeLayout(false);
            this.pnlBase.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).EndInit();
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlEditMenu.ResumeLayout(false);
            this.pnlBaseCraft.ResumeLayout(false);
            this.pnlBaseCraft.PerformLayout();
            this.pnlInformationBase.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlContainerWearableClass.ResumeLayout(false);
            this.pnlContainerWearableClass.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlItemOptions.ResumeLayout(false);
            this.pnlItemOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.SplitContainer pnlBase;
        protected System.Windows.Forms.Label lblSubTitleFilter;
        protected System.Windows.Forms.TextBox txbFilter;
        protected System.Windows.Forms.DataGridView grdList;
        protected System.Windows.Forms.Label lblTitleList;
        protected System.Windows.Forms.TextBox txbName;
        protected System.Windows.Forms.Label lblSubTitleName;
        protected System.Windows.Forms.Label lblTitleEditor;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel pnlInformationBase;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel pnlContainerWearableClass;
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.ComboBox cbbItemPart;
        protected System.Windows.Forms.CheckBox chkClassKnight;
        protected System.Windows.Forms.CheckBox chkClassWizard;
        protected System.Windows.Forms.CheckBox chkClassPriest;
        protected System.Windows.Forms.CheckBox chkClassArcher;
        protected System.Windows.Forms.CheckBox chkClassDruid;
        protected System.Windows.Forms.CheckBox chkClassSummoner;
        protected System.Windows.Forms.ComboBox cbbItemGrade;
        protected System.Windows.Forms.CheckBox chkUnidentifiedItem;
        protected System.Windows.Forms.TableLayoutPanel pnlItemOptions;
        protected System.Windows.Forms.Label label7;
        protected System.Windows.Forms.Label label5;
        protected System.Windows.Forms.Label label6;
        protected System.Windows.Forms.Label label8;
        protected System.Windows.Forms.Label label10;
        protected System.Windows.Forms.Label label11;
        protected System.Windows.Forms.Label label12;
        protected System.Windows.Forms.Label label13;
        protected System.Windows.Forms.TextBox txbStrength;
        protected System.Windows.Forms.TextBox txbInteligence;
        protected System.Windows.Forms.TextBox txbAgility;
        protected System.Windows.Forms.TextBox txbHP;
        protected System.Windows.Forms.TextBox txbMP;
        protected System.Windows.Forms.TextBox txbAttack;
        protected System.Windows.Forms.TextBox txbArmor;
        protected System.Windows.Forms.Label label9;
        protected System.Windows.Forms.TextBox txbUniqueOptions;
        private System.Windows.Forms.TableLayoutPanel pnlBaseCraft;
        protected System.Windows.Forms.Label label14;
        protected System.Windows.Forms.Label lblSubTitleCraftResult;
        protected System.Windows.Forms.Label lblSubTitleCraftRecipe;
        protected System.Windows.Forms.Label lblCraftRecipePreview;
        protected System.Windows.Forms.TextBox txbCraftResult;
        protected System.Windows.Forms.ComboBox cbbCraftRecipe;
        private System.Windows.Forms.TableLayoutPanel pnlEditMenu;
        protected System.Windows.Forms.Button btnEditClipImage;
        protected System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.Button btnDelete;
        protected System.Windows.Forms.Button btnEdit;
        protected System.Windows.Forms.Label lblVersionInfo;
        protected System.Windows.Forms.Button btnEditRename;
    }
}