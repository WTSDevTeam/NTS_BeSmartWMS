namespace BeSmartMRP.Report
{
    partial class frmPBGStat
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndQcProj = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQcProj = new DevExpress.XtraEditors.ButtonEdit();
            this.txtYear = new DevExpress.XtraEditors.ButtonEdit();
            this.label88 = new System.Windows.Forms.Label();
            this.txtQcSect = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnSect = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(70, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(511, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "รายงานตรวจสอบสถานะการตั้งงบประมาณ";
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
            this.pnlTitle.Location = new System.Drawing.Point(-1, -1);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(594, 76);
            this.pnlTitle.TabIndex = 113;
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
            this.panelControl1.Size = new System.Drawing.Size(595, 24);
            this.panelControl1.TabIndex = 1;
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(77, 4);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 17);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "TASKNAME : PBGSTAT01";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BeSmartMRP.Properties.Resources.printer2;
            this.pictureBox1.Location = new System.Drawing.Point(15, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 383);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(582, 22);
            this.statusStrip1.TabIndex = 114;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(255, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "ย&กเลิก";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnPrint.Location = new System.Drawing.Point(187, 211);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "&ตกลง";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(73, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 119;
            this.label1.Text = "ถึงโครงการรหัส :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(14, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 23);
            this.label2.TabIndex = 120;
            this.label2.Text = "พิมพ์โครงการตั้งแต่รหัส :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndQcProj
            // 
            this.txtEndQcProj.EnterMoveNextControl = true;
            this.txtEndQcProj.Location = new System.Drawing.Point(187, 171);
            this.txtEndQcProj.Name = "txtEndQcProj";
            this.txtEndQcProj.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcProj.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcProj.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcProj.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcProj.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcProj.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcProj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtEndQcProj.Size = new System.Drawing.Size(196, 22);
            this.txtEndQcProj.TabIndex = 4;
            this.txtEndQcProj.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcProj.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj_Validating);
            this.txtEndQcProj.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQcProj
            // 
            this.txtBegQcProj.EnterMoveNextControl = true;
            this.txtBegQcProj.Location = new System.Drawing.Point(187, 147);
            this.txtBegQcProj.Name = "txtBegQcProj";
            this.txtBegQcProj.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcProj.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcProj.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcProj.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcProj.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBegQcProj.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBegQcProj.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcProj.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcProj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtBegQcProj.Size = new System.Drawing.Size(196, 22);
            this.txtBegQcProj.TabIndex = 3;
            this.txtBegQcProj.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcProj.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj_Validating);
            this.txtBegQcProj.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtYear
            // 
            this.txtYear.EnterMoveNextControl = true;
            this.txtYear.Location = new System.Drawing.Point(187, 120);
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
            this.txtYear.TabIndex = 2;
            this.txtYear.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtYear.Validating += new System.ComponentModel.CancelEventHandler(this.txtYear_Validating);
            this.txtYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label88.Location = new System.Drawing.Point(74, 119);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(112, 23);
            this.label88.TabIndex = 125;
            this.label88.Text = "ปีงบประมาณ :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcSect
            // 
            this.txtQcSect.EnterMoveNextControl = true;
            this.txtQcSect.Location = new System.Drawing.Point(187, 95);
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
            this.txtQcSect.TabIndex = 0;
            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcSect.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcSect_Validating);
            this.txtQcSect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQnSect
            // 
            this.txtQnSect.Enabled = false;
            this.txtQnSect.EnterMoveNextControl = true;
            this.txtQnSect.Location = new System.Drawing.Point(316, 95);
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
            this.txtQnSect.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(74, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 23);
            this.label3.TabIndex = 124;
            this.label3.Text = "ระบุแผนก :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmPBGStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.txtQcSect);
            this.Controls.Add(this.txtQnSect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEndQcProj);
            this.Controls.Add(this.txtBegQcProj);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmPBGStat";
            this.Text = "PBGSTAT01";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcProj;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcProj;
        private DevExpress.XtraEditors.ButtonEdit txtYear;
        private System.Windows.Forms.Label label88;
        private DevExpress.XtraEditors.ButtonEdit txtQcSect;
        private DevExpress.XtraEditors.ButtonEdit txtQnSect;
        private System.Windows.Forms.Label label3;
    }
}