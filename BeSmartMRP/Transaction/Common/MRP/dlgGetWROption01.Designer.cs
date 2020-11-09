namespace BeSmartMRP.Transaction.Common.MRP
{
    partial class dlgGetWROption01
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
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.txtQnCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnBook = new DevExpress.XtraEditors.ButtonEdit();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCoor = new System.Windows.Forms.Label();
            this.lblBranch = new System.Windows.Forms.Label();
            this.txtQcCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtQcBook = new DevExpress.XtraEditors.ButtonEdit();
            this.lblBook = new System.Windows.Forms.Label();
            this.chkIsValidStk = new DevExpress.XtraEditors.CheckEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBook.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBook.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsValidStk.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBegDate
            // 
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(127, 147);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtBegDate.Size = new System.Drawing.Size(136, 22);
            this.txtBegDate.TabIndex = 132;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblDate.Location = new System.Drawing.Point(-8, 147);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(136, 23);
            this.lblDate.TabIndex = 140;
            this.lblDate.Text = "วันที่เอกสาร :";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(77, 4);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 17);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "TASKNAME : EBUDALOCATE";
            this.lblTaskName.Visible = false;
            // 
            // txtQnCoor
            // 
            this.txtQnCoor.Enabled = false;
            this.txtQnCoor.EnterMoveNextControl = true;
            this.txtQnCoor.Location = new System.Drawing.Point(269, 259);
            this.txtQnCoor.Name = "txtQnCoor";
            this.txtQnCoor.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnCoor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnCoor.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnCoor.Properties.Appearance.Options.UseFont = true;
            this.txtQnCoor.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnCoor.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnCoor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnCoor.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnCoor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnCoor.Size = new System.Drawing.Size(275, 22);
            this.txtQnCoor.TabIndex = 131;
            this.txtQnCoor.Visible = false;
            // 
            // txtQnBranch
            // 
            this.txtQnBranch.Enabled = false;
            this.txtQnBranch.EnterMoveNextControl = true;
            this.txtQnBranch.Location = new System.Drawing.Point(269, 92);
            this.txtQnBranch.Name = "txtQnBranch";
            this.txtQnBranch.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnBranch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnBranch.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnBranch.Properties.Appearance.Options.UseFont = true;
            this.txtQnBranch.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnBranch.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnBranch.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnBranch.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnBranch.Size = new System.Drawing.Size(275, 22);
            this.txtQnBranch.TabIndex = 127;
            // 
            // txtQnBook
            // 
            this.txtQnBook.Enabled = false;
            this.txtQnBook.EnterMoveNextControl = true;
            this.txtQnBook.Location = new System.Drawing.Point(269, 119);
            this.txtQnBook.Name = "txtQnBook";
            this.txtQnBook.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnBook.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnBook.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnBook.Properties.Appearance.Options.UseFont = true;
            this.txtQnBook.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnBook.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnBook.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnBook.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnBook.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnBook.Size = new System.Drawing.Size(275, 22);
            this.txtQnBook.TabIndex = 129;
            // 
            // pnlTitle
            // 
            this.pnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnlTitle.Appearance.BackColor2 = System.Drawing.Color.LightSkyBlue;
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.panelControl1);
            this.pnlTitle.Controls.Add(this.pictureBox1);
            this.pnlTitle.Location = new System.Drawing.Point(-8, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(599, 76);
            this.pnlTitle.TabIndex = 135;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(70, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(298, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "บันทึกการปันส่วนงบประมาณ";
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.lblTaskName);
            this.panelControl1.Location = new System.Drawing.Point(-1, 51);
            this.panelControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(597, 24);
            this.panelControl1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BeSmartMRP.Properties.Resources.clipboard;
            this.pictureBox1.Location = new System.Drawing.Point(15, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblCoor
            // 
            this.lblCoor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCoor.Location = new System.Drawing.Point(-9, 258);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(136, 23);
            this.lblCoor.TabIndex = 139;
            this.lblCoor.Text = "ระบุผู้จำหน่าย :";
            this.lblCoor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCoor.Visible = false;
            // 
            // lblBranch
            // 
            this.lblBranch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblBranch.Location = new System.Drawing.Point(-9, 91);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(136, 23);
            this.lblBranch.TabIndex = 138;
            this.lblBranch.Text = "ระบุสาขา :";
            this.lblBranch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcCoor
            // 
            this.txtQcCoor.EnterMoveNextControl = true;
            this.txtQcCoor.Location = new System.Drawing.Point(127, 259);
            this.txtQcCoor.Name = "txtQcCoor";
            this.txtQcCoor.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcCoor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcCoor.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Appearance.Options.UseFont = true;
            this.txtQcCoor.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQcCoor.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQcCoor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcCoor.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcCoor.Size = new System.Drawing.Size(136, 22);
            this.txtQcCoor.TabIndex = 130;
            this.txtQcCoor.Visible = false;
            // 
            // txtQcBranch
            // 
            this.txtQcBranch.Enabled = false;
            this.txtQcBranch.EnterMoveNextControl = true;
            this.txtQcBranch.Location = new System.Drawing.Point(127, 92);
            this.txtQcBranch.Name = "txtQcBranch";
            this.txtQcBranch.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBranch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBranch.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Appearance.Options.UseFont = true;
            this.txtQcBranch.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQcBranch.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQcBranch.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBranch.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcBranch.Size = new System.Drawing.Size(136, 22);
            this.txtQcBranch.TabIndex = 126;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 336);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(582, 22);
            this.statusStrip1.TabIndex = 137;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtQcBook
            // 
            this.txtQcBook.EnterMoveNextControl = true;
            this.txtQcBook.Location = new System.Drawing.Point(127, 119);
            this.txtQcBook.Name = "txtQcBook";
            this.txtQcBook.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBook.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBook.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBook.Properties.Appearance.Options.UseFont = true;
            this.txtQcBook.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBook.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBook.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcBook.Size = new System.Drawing.Size(136, 22);
            this.txtQcBook.TabIndex = 128;
            // 
            // lblBook
            // 
            this.lblBook.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblBook.Location = new System.Drawing.Point(-9, 118);
            this.lblBook.Name = "lblBook";
            this.lblBook.Size = new System.Drawing.Size(136, 23);
            this.lblBook.TabIndex = 136;
            this.lblBook.Text = "ระบุเล่มเอกสาร :";
            this.lblBook.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIsValidStk
            // 
            this.chkIsValidStk.Location = new System.Drawing.Point(124, 180);
            this.chkIsValidStk.Name = "chkIsValidStk";
            this.chkIsValidStk.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.chkIsValidStk.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.chkIsValidStk.Properties.Appearance.Options.UseBackColor = true;
            this.chkIsValidStk.Properties.Appearance.Options.UseFont = true;
            this.chkIsValidStk.Properties.Caption = "Gen Issue slip items. If stock available only ?";
            this.chkIsValidStk.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsValidStk.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.chkIsValidStk.Size = new System.Drawing.Size(323, 22);
            this.chkIsValidStk.TabIndex = 141;
            this.chkIsValidStk.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(195, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 134;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(127, 217);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 133;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dlgGetWROption01
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 358);
            this.ControlBox = false;
            this.Controls.Add(this.chkIsValidStk);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtQnCoor);
            this.Controls.Add(this.txtQnBranch);
            this.Controls.Add(this.txtQnBook);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.lblCoor);
            this.Controls.Add(this.lblBranch);
            this.Controls.Add(this.txtQcCoor);
            this.Controls.Add(this.txtQcBranch);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtQcBook);
            this.Controls.Add(this.lblBook);
            this.Name = "dlgGetWROption01";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Option";
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBook.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBook.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsValidStk.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTaskName;
        private DevExpress.XtraEditors.ButtonEdit txtQnCoor;
        private DevExpress.XtraEditors.ButtonEdit txtQnBranch;
        private DevExpress.XtraEditors.ButtonEdit txtQnBook;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCoor;
        private System.Windows.Forms.Label lblBranch;
        private DevExpress.XtraEditors.ButtonEdit txtQcCoor;
        private DevExpress.XtraEditors.ButtonEdit txtQcBranch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.ButtonEdit txtQcBook;
        private System.Windows.Forms.Label lblBook;
        private DevExpress.XtraEditors.CheckEdit chkIsValidStk;
    }
}