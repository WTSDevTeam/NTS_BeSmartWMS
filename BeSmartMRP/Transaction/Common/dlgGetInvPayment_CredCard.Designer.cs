namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetInvPayment_CredCard
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
            this.pnlBToolbar = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtPayType = new DevExpress.XtraEditors.ButtonEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInvAmt = new DevExpress.XtraEditors.SpinEdit();
            this.txtRecvAmt = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCardNo = new DevExpress.XtraEditors.ButtonEdit();
            this.btn_Pym2 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Pym1 = new DevExpress.XtraEditors.SimpleButton();
            this.pnlBToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBToolbar
            // 
            this.pnlBToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBToolbar.BackColor = System.Drawing.Color.Turquoise;
            this.pnlBToolbar.Controls.Add(this.btnCancel);
            this.pnlBToolbar.Controls.Add(this.btnOK);
            this.pnlBToolbar.Location = new System.Drawing.Point(-6, 512);
            this.pnlBToolbar.Name = "pnlBToolbar";
            this.pnlBToolbar.Size = new System.Drawing.Size(846, 138);
            this.pnlBToolbar.TabIndex = 115;
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
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseBackColor = true;
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(659, 9);
            this.btnOK.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(162, 112);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&ตกลง";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtPayType
            // 
            this.txtPayType.Enabled = false;
            this.txtPayType.EnterMoveNextControl = true;
            this.txtPayType.Location = new System.Drawing.Point(280, 73);
            this.txtPayType.Name = "txtPayType";
            this.txtPayType.Properties.AllowFocused = false;
            this.txtPayType.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtPayType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtPayType.Properties.Appearance.Options.UseBackColor = true;
            this.txtPayType.Properties.Appearance.Options.UseFont = true;
            this.txtPayType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtPayType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtPayType.Properties.ReadOnly = true;
            this.txtPayType.Size = new System.Drawing.Size(340, 46);
            this.txtPayType.TabIndex = 1;
            this.txtPayType.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 39);
            this.label4.TabIndex = 116;
            this.label4.Text = "ประเภท :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtInvAmt.Location = new System.Drawing.Point(280, 21);
            this.txtInvAmt.Name = "txtInvAmt";
            this.txtInvAmt.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.txtInvAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
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
            this.txtInvAmt.Size = new System.Drawing.Size(340, 46);
            this.txtInvAmt.TabIndex = 0;
            // 
            // txtRecvAmt
            // 
            this.txtRecvAmt.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtRecvAmt.EnterMoveNextControl = true;
            this.txtRecvAmt.Location = new System.Drawing.Point(280, 177);
            this.txtRecvAmt.Name = "txtRecvAmt";
            this.txtRecvAmt.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtRecvAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtRecvAmt.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtRecvAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtRecvAmt.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtRecvAmt.Size = new System.Drawing.Size(340, 46);
            this.txtRecvAmt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 39);
            this.label2.TabIndex = 121;
            this.label2.Text = "ยอดเงิน :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(6, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 39);
            this.label1.TabIndex = 120;
            this.label1.Text = "รับเงิน :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(6, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 39);
            this.label3.TabIndex = 116;
            this.label3.Text = "เลขบัตร :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNo
            // 
            this.txtCardNo.EnterMoveNextControl = true;
            this.txtCardNo.Location = new System.Drawing.Point(280, 125);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtCardNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCardNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtCardNo.Properties.Appearance.Options.UseFont = true;
            this.txtCardNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCardNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCardNo.Size = new System.Drawing.Size(340, 46);
            this.txtCardNo.TabIndex = 2;
            // 
            // btn_Pym2
            // 
            this.btn_Pym2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Pym2.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btn_Pym2.Appearance.Options.UseFont = true;
            this.btn_Pym2.Image = global::BeSmartMRP.Properties.Resources.master_card;
            this.btn_Pym2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_Pym2.Location = new System.Drawing.Point(655, 138);
            this.btn_Pym2.Name = "btn_Pym2";
            this.btn_Pym2.Size = new System.Drawing.Size(169, 110);
            this.btn_Pym2.TabIndex = 6;
            this.btn_Pym2.TabStop = false;
            this.btn_Pym2.Tag = "CRED_MC";
            this.btn_Pym2.Text = "MASTER CARD";
            this.btn_Pym2.Click += new System.EventHandler(this.btn_Pym2_Click);
            // 
            // btn_Pym1
            // 
            this.btn_Pym1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Pym1.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btn_Pym1.Appearance.Options.UseFont = true;
            this.btn_Pym1.Image = global::BeSmartMRP.Properties.Resources.visa_2;
            this.btn_Pym1.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_Pym1.Location = new System.Drawing.Point(655, 22);
            this.btn_Pym1.Name = "btn_Pym1";
            this.btn_Pym1.Size = new System.Drawing.Size(169, 110);
            this.btn_Pym1.TabIndex = 5;
            this.btn_Pym1.TabStop = false;
            this.btn_Pym1.Tag = "CRED_VISA";
            this.btn_Pym1.Text = "VISA";
            this.btn_Pym1.Click += new System.EventHandler(this.btn_Pym1_Click);
            // 
            // dlgGetInvPayment_CredCard
            // 
            this.AcceptButton = this.btnOK;
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(836, 643);
            this.ControlBox = false;
            this.Controls.Add(this.txtInvAmt);
            this.Controls.Add(this.txtRecvAmt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCardNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPayType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlBToolbar);
            this.Controls.Add(this.btn_Pym2);
            this.Controls.Add(this.btn_Pym1);
            this.Name = "dlgGetInvPayment_CredCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credit Card Detail";
            this.pnlBToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPayType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Pym2;
        private DevExpress.XtraEditors.SimpleButton btn_Pym1;
        private System.Windows.Forms.Panel pnlBToolbar;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.ButtonEdit txtPayType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SpinEdit txtInvAmt;
        public DevExpress.XtraEditors.SpinEdit txtRecvAmt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ButtonEdit txtCardNo;
    }
}