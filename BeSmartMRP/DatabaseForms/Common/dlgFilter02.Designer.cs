namespace BeSmartMRP.DatabaseForms.Common
{
    partial class dlgFilter02
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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.cmbQcRefType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtQnRefType = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbQcRefType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnRefType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(16, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 54;
            this.label1.Text = "ระบุประเภทเอกสาร :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 287);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(493, 22);
            this.statusStrip1.TabIndex = 59;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(227, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(157, 145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&ตกลง";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTitle.Location = new System.Drawing.Point(70, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(298, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "เล่มเอกสาร";
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
            this.pnlTitle.Location = new System.Drawing.Point(-10, -2);
            this.pnlTitle.LookAndFeel.SkinName = "Liquid Sky";
            this.pnlTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(512, 76);
            this.pnlTitle.TabIndex = 51;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.lblTaskName);
            this.panelControl1.Location = new System.Drawing.Point(-2, 50);
            this.panelControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(512, 24);
            this.panelControl1.TabIndex = 1;
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(77, 2);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "TASKNAME : EBGBOOK";
            // 
            // cmbQcRefType
            // 
            this.cmbQcRefType.EnterMoveNextControl = true;
            this.cmbQcRefType.Location = new System.Drawing.Point(159, 93);
            this.cmbQcRefType.Name = "cmbQcRefType";
            this.cmbQcRefType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbQcRefType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbQcRefType.Properties.Appearance.Options.UseBackColor = true;
            this.cmbQcRefType.Properties.Appearance.Options.UseFont = true;
            this.cmbQcRefType.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbQcRefType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbQcRefType.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbQcRefType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbQcRefType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbQcRefType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbQcRefType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.cmbQcRefType.Properties.Items.AddRange(new object[] {
            "B1"});
            this.cmbQcRefType.Properties.PopupSizeable = true;
            this.cmbQcRefType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbQcRefType.Size = new System.Drawing.Size(62, 22);
            this.cmbQcRefType.TabIndex = 0;
            this.cmbQcRefType.SelectedIndexChanged += new System.EventHandler(this.cmbQcRefType_SelectedIndexChanged);
            // 
            // txtQnRefType
            // 
            this.txtQnRefType.Enabled = false;
            this.txtQnRefType.EnterMoveNextControl = true;
            this.txtQnRefType.Location = new System.Drawing.Point(227, 92);
            this.txtQnRefType.Name = "txtQnRefType";
            this.txtQnRefType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnRefType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnRefType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnRefType.Properties.Appearance.Options.UseFont = true;
            this.txtQnRefType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnRefType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnRefType.Size = new System.Drawing.Size(224, 22);
            this.txtQnRefType.TabIndex = 1;
            // 
            // dlgFilter02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(493, 309);
            this.ControlBox = false;
            this.Controls.Add(this.cmbQcRefType);
            this.Controls.Add(this.txtQnRefType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgFilter02";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ระบุ Option ในการแสดงผล";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbQcRefType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnRefType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private DevExpress.XtraEditors.ComboBoxEdit cmbQcRefType;
        private DevExpress.XtraEditors.ButtonEdit txtQnRefType;
    }
}