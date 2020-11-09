namespace BeSmartMRP.Transaction.Common
{
    partial class dlgHisOpt1
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
            this.pnlProd = new System.Windows.Forms.Panel();
            this.txtEndQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.lblCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlProd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlProd
            // 
            this.pnlProd.BackColor = System.Drawing.Color.Transparent;
            this.pnlProd.Controls.Add(this.txtEndQcProd);
            this.pnlProd.Controls.Add(this.txtBegQcProd);
            this.pnlProd.Controls.Add(this.label3);
            this.pnlProd.Controls.Add(this.label2);
            this.pnlProd.Location = new System.Drawing.Point(12, 26);
            this.pnlProd.Name = "pnlProd";
            this.pnlProd.Size = new System.Drawing.Size(378, 66);
            this.pnlProd.TabIndex = 0;
            // 
            // txtEndQcProd
            // 
            this.txtEndQcProd.EnterMoveNextControl = true;
            this.txtEndQcProd.Location = new System.Drawing.Point(132, 36);
            this.txtEndQcProd.Name = "txtEndQcProd";
            this.txtEndQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtEndQcProd.Size = new System.Drawing.Size(229, 22);
            this.txtEndQcProd.TabIndex = 1;
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtBegQcProd_WsOnValidating);
            this.txtEndQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQcProd
            // 
            this.txtBegQcProd.EnterMoveNextControl = true;
            this.txtBegQcProd.Location = new System.Drawing.Point(132, 13);
            this.txtBegQcProd.Name = "txtBegQcProd";
            this.txtBegQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtBegQcProd.Size = new System.Drawing.Size(229, 22);
            this.txtBegQcProd.TabIndex = 0;
            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtBegQcProd_WsOnValidating);
            this.txtBegQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "ถึง :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(3, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "รหัสสินค้าตั้งแต่ :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(212, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "C&ancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_ok;
            this.btnOK.Location = new System.Drawing.Point(144, 166);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            // 
            // txtBegDate
            // 
            this.txtBegDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(144, 98);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtBegDate.Size = new System.Drawing.Size(135, 22);
            this.txtBegDate.TabIndex = 1;
            // 
            // txtEndDate
            // 
            this.txtEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(144, 126);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtEndDate.Size = new System.Drawing.Size(135, 22);
            this.txtEndDate.TabIndex = 2;
            // 
            // lblCode
            // 
            this.lblCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(42, 97);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(97, 23);
            this.lblCode.TabIndex = 7;
            this.lblCode.Text = "ระบุช่วงวันที่ :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(42, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "ถึง :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgHisOpt1
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 227);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlProd);
            this.Name = "dlgHisOpt1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Form";
            this.pnlProd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlProd;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private DevExpress.XtraEditors.DateEdit txtEndDate;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcProd;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcProd;
        private System.Windows.Forms.Label label3;
    }
}