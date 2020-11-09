namespace BeSmartMRP.DatabaseForms.Common
{
    partial class dlgFilter03
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.label88 = new System.Windows.Forms.Label();
            this.txtQcSect = new DevExpress.XtraEditors.ButtonEdit();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtQnSect = new DevExpress.XtraEditors.ButtonEdit();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQnBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.txtYear = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 287);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(493, 22);
            this.statusStrip1.TabIndex = 60;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(191, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(123, 179);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&ตกลง";
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label88.Location = new System.Drawing.Point(10, 136);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(112, 23);
            this.label88.TabIndex = 59;
            this.label88.Text = "ปีงบประมาณ :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcSect
            // 
            this.txtQcSect.EnterMoveNextControl = true;
            this.txtQcSect.Location = new System.Drawing.Point(123, 112);
            this.txtQcSect.Name = "txtQcSect";
            this.txtQcSect.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcSect.Properties.Appearance.Options.UseFont = true;
            this.txtQcSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcSect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQcSect.Size = new System.Drawing.Size(123, 22);
            this.txtQcSect.TabIndex = 2;
            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcSect.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcSect_Validating);
            this.txtQcSect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(77, 5);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 16);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "TASKNAME : EBUDALOCATE";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(70, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(414, 31);
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
            this.panelControl1.Size = new System.Drawing.Size(510, 24);
            this.panelControl1.TabIndex = 1;
            // 
            // txtQnSect
            // 
            this.txtQnSect.Enabled = false;
            this.txtQnSect.EnterMoveNextControl = true;
            this.txtQnSect.Location = new System.Drawing.Point(252, 112);
            this.txtQnSect.Name = "txtQnSect";
            this.txtQnSect.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnSect.Properties.Appearance.Options.UseFont = true;
            this.txtQnSect.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnSect.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnSect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnSect.Size = new System.Drawing.Size(215, 22);
            this.txtQnSect.TabIndex = 3;
            this.txtQnSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnSect.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcSect_Validating);
            this.txtQnSect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
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
            this.pnlTitle.Location = new System.Drawing.Point(-3, -4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(512, 76);
            this.pnlTitle.TabIndex = 56;
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(10, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 57;
            this.label1.Text = "ระบุแผนก :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(10, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 57;
            this.label2.Text = "ระบุสาขา :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnBranch
            // 
            this.txtQnBranch.Enabled = false;
            this.txtQnBranch.EnterMoveNextControl = true;
            this.txtQnBranch.Location = new System.Drawing.Point(252, 87);
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnBranch.Size = new System.Drawing.Size(215, 22);
            this.txtQnBranch.TabIndex = 1;
            // 
            // txtQcBranch
            // 
            this.txtQcBranch.Enabled = false;
            this.txtQcBranch.EnterMoveNextControl = true;
            this.txtQcBranch.Location = new System.Drawing.Point(123, 87);
            this.txtQcBranch.Name = "txtQcBranch";
            this.txtQcBranch.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBranch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBranch.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Appearance.Options.UseFont = true;
            this.txtQcBranch.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBranch.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQcBranch.Size = new System.Drawing.Size(123, 22);
            this.txtQcBranch.TabIndex = 0;
            // 
            // txtYear
            // 
            this.txtYear.EnterMoveNextControl = true;
            this.txtYear.Location = new System.Drawing.Point(123, 137);
            this.txtYear.Name = "txtYear";
            this.txtYear.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtYear.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtYear.Properties.Appearance.Options.UseBackColor = true;
            this.txtYear.Properties.Appearance.Options.UseFont = true;
            this.txtYear.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtYear.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtYear.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtYear.Size = new System.Drawing.Size(123, 22);
            this.txtYear.TabIndex = 4;
            this.txtYear.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtYear.Validating += new System.ComponentModel.CancelEventHandler(this.txtYear_Validating);
            this.txtYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // dlgFilter03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(493, 309);
            this.ControlBox = false;
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.txtQcBranch);
            this.Controls.Add(this.txtQcSect);
            this.Controls.Add(this.txtQnBranch);
            this.Controls.Add(this.txtQnSect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.label1);
            this.Name = "dlgFilter03";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ระบุ Option";
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label label88;
        private DevExpress.XtraEditors.ButtonEdit txtQcSect;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtQnSect;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtQnBranch;
        private DevExpress.XtraEditors.ButtonEdit txtQcBranch;
        private DevExpress.XtraEditors.ButtonEdit txtYear;
    }
}