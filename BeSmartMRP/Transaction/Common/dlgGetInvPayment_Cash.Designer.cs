namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetInvPayment_Cash
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtInvAmt = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChangeAmt = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRecvAmt = new DevExpress.XtraEditors.SpinEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBToolbar = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChangeAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).BeginInit();
            this.pnlBToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseBackColor = true;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(27, 9);
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(162, 112);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // txtInvAmt
            // 
            this.txtInvAmt.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtInvAmt.Enabled = false;
            this.txtInvAmt.EnterMoveNextControl = true;
            this.txtInvAmt.Location = new System.Drawing.Point(243, 37);
            this.txtInvAmt.Name = "txtInvAmt";
            this.txtInvAmt.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.txtInvAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtInvAmt.Properties.Appearance.ForeColor = System.Drawing.Color.Yellow;
            this.txtInvAmt.Properties.Appearance.Options.UseBackColor = true;
            this.txtInvAmt.Properties.Appearance.Options.UseFont = true;
            this.txtInvAmt.Properties.Appearance.Options.UseForeColor = true;
            this.txtInvAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtInvAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtInvAmt.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtInvAmt.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtInvAmt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtInvAmt.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtInvAmt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtInvAmt.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtInvAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtInvAmt.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtInvAmt.Size = new System.Drawing.Size(289, 48);
            this.txtInvAmt.TabIndex = 124;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(38, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 39);
            this.label2.TabIndex = 125;
            this.label2.Text = "ยอดเงิน :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtChangeAmt
            // 
            this.txtChangeAmt.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtChangeAmt.Enabled = false;
            this.txtChangeAmt.EnterMoveNextControl = true;
            this.txtChangeAmt.Location = new System.Drawing.Point(243, 145);
            this.txtChangeAmt.Name = "txtChangeAmt";
            this.txtChangeAmt.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.txtChangeAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtChangeAmt.Properties.Appearance.ForeColor = System.Drawing.Color.Yellow;
            this.txtChangeAmt.Properties.Appearance.Options.UseBackColor = true;
            this.txtChangeAmt.Properties.Appearance.Options.UseFont = true;
            this.txtChangeAmt.Properties.Appearance.Options.UseForeColor = true;
            this.txtChangeAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtChangeAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtChangeAmt.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtChangeAmt.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtChangeAmt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtChangeAmt.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtChangeAmt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtChangeAmt.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtChangeAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtChangeAmt.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtChangeAmt.Size = new System.Drawing.Size(289, 48);
            this.txtChangeAmt.TabIndex = 120;
            this.txtChangeAmt.Visible = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(38, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 39);
            this.label3.TabIndex = 123;
            this.label3.Text = "เงินทอน :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Visible = false;
            // 
            // txtRecvAmt
            // 
            this.txtRecvAmt.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtRecvAmt.EnterMoveNextControl = true;
            this.txtRecvAmt.Location = new System.Drawing.Point(243, 91);
            this.txtRecvAmt.Name = "txtRecvAmt";
            this.txtRecvAmt.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtRecvAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtRecvAmt.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtRecvAmt.Properties.Appearance.Options.UseBackColor = true;
            this.txtRecvAmt.Properties.Appearance.Options.UseFont = true;
            this.txtRecvAmt.Properties.Appearance.Options.UseForeColor = true;
            this.txtRecvAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtRecvAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtRecvAmt.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtRecvAmt.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtRecvAmt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtRecvAmt.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtRecvAmt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.txtRecvAmt.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtRecvAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtRecvAmt.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtRecvAmt.Size = new System.Drawing.Size(289, 48);
            this.txtRecvAmt.TabIndex = 0;
            this.txtRecvAmt.ValueChanged += new System.EventHandler(this.txtRecvAmt_Validated);
            this.txtRecvAmt.EditValueChanged += new System.EventHandler(this.txtRecvAmt_ValueChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(38, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 39);
            this.label1.TabIndex = 121;
            this.label1.Text = "รับเงิน :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlBToolbar
            // 
            this.pnlBToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBToolbar.BackColor = System.Drawing.Color.Turquoise;
            this.pnlBToolbar.Controls.Add(this.btnCancel);
            this.pnlBToolbar.Controls.Add(this.btnOK);
            this.pnlBToolbar.Location = new System.Drawing.Point(-9, 348);
            this.pnlBToolbar.Name = "pnlBToolbar";
            this.pnlBToolbar.Size = new System.Drawing.Size(746, 138);
            this.pnlBToolbar.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseBackColor = true;
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(547, 9);
            this.btnOK.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(174, 112);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&ตกลง";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dlgGetInvPayment_Cash
            // 
            this.AcceptButton = this.btnOK;
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(725, 475);
            this.ControlBox = false;
            this.Controls.Add(this.txtInvAmt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtChangeAmt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRecvAmt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlBToolbar);
            this.Name = "dlgGetInvPayment_Cash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "เงินสด";
            ((System.ComponentModel.ISupportInitialize)(this.txtInvAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChangeAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).EndInit();
            this.pnlBToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBToolbar;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        public DevExpress.XtraEditors.SpinEdit txtChangeAmt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit txtInvAmt;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SpinEdit txtRecvAmt;
    }
}