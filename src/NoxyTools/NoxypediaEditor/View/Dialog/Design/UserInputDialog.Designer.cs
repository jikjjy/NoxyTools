
namespace NoxypediaEditor
{
    partial class UserInputDialog
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
            this.pnlBaseMenu = new System.Windows.Forms.Panel();
            this.pnlMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnCencel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitleInput = new System.Windows.Forms.Label();
            this.txbInput = new System.Windows.Forms.TextBox();
            this.pnlBaseMenu.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBaseMenu
            // 
            this.pnlBaseMenu.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBaseMenu.Controls.Add(this.pnlMenu);
            this.pnlBaseMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBaseMenu.Location = new System.Drawing.Point(0, 106);
            this.pnlBaseMenu.Name = "pnlBaseMenu";
            this.pnlBaseMenu.Size = new System.Drawing.Size(411, 47);
            this.pnlBaseMenu.TabIndex = 0;
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlMenu.ColumnCount = 3;
            this.pnlMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlMenu.Controls.Add(this.btnCencel, 2, 0);
            this.pnlMenu.Controls.Add(this.btnOk, 1, 0);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.RowCount = 1;
            this.pnlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMenu.Size = new System.Drawing.Size(411, 47);
            this.pnlMenu.TabIndex = 0;
            // 
            // btnCencel
            // 
            this.btnCencel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnCencel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCencel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCencel.Location = new System.Drawing.Point(277, 6);
            this.btnCencel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnCencel.Name = "btnCencel";
            this.btnCencel.Size = new System.Drawing.Size(131, 35);
            this.btnCencel.TabIndex = 0;
            this.btnCencel.Text = "취소";
            this.btnCencel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(140, 6);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(131, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(39, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(42, 19);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "TITLE";
            // 
            // lblSubTitleInput
            // 
            this.lblSubTitleInput.AutoSize = true;
            this.lblSubTitleInput.Location = new System.Drawing.Point(11, 44);
            this.lblSubTitleInput.Name = "lblSubTitleInput";
            this.lblSubTitleInput.Size = new System.Drawing.Size(34, 17);
            this.lblSubTitleInput.TabIndex = 2;
            this.lblSubTitleInput.Text = "입력";
            // 
            // txbInput
            // 
            this.txbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbInput.BackColor = System.Drawing.Color.DarkGray;
            this.txbInput.Location = new System.Drawing.Point(30, 69);
            this.txbInput.Name = "txbInput";
            this.txbInput.Size = new System.Drawing.Size(369, 23);
            this.txbInput.TabIndex = 3;
            // 
            // UserInputDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.CancelButton = this.btnCencel;
            this.ClientSize = new System.Drawing.Size(411, 153);
            this.ControlBox = false;
            this.Controls.Add(this.txbInput);
            this.Controls.Add(this.lblSubTitleInput);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlBaseMenu);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Noxypedia Editor";
            this.pnlBaseMenu.ResumeLayout(false);
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBaseMenu;
        private System.Windows.Forms.TableLayoutPanel pnlMenu;
        protected System.Windows.Forms.Button btnCencel;
        protected System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblSubTitleInput;
        protected System.Windows.Forms.Label lblTitle;
        protected System.Windows.Forms.TextBox txbInput;
    }
}