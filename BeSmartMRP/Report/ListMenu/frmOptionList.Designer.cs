﻿namespace BeSmartMRP.Report.ListMenu
{
    partial class frmOptionList
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
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblText1 = new System.Windows.Forms.Label();
            this.txtBegQnProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEndQnProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndQcProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQcProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnProj1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnProj1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj1.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.pnlTitle.Location = new System.Drawing.Point(-7, -2);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(598, 76);
            this.pnlTitle.TabIndex = 82;
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
            this.lblTitle.Size = new System.Drawing.Size(515, 31);
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
            this.panelControl1.Size = new System.Drawing.Size(599, 24);
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
            this.lblTaskName.Text = "TASKNAME : EBUDALOCATE";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.lblText1);
            this.panel1.Controls.Add(this.txtBegQnProj1);
            this.panel1.Controls.Add(this.txtEndQnProj1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtEndQcProj1);
            this.panel1.Controls.Add(this.txtBegQcProj1);
            this.panel1.Location = new System.Drawing.Point(8, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 89);
            this.panel1.TabIndex = 83;
            // 
            // lblText1
            // 
            this.lblText1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblText1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblText1.Location = new System.Drawing.Point(71, 0);
            this.lblText1.Name = "lblText1";
            this.lblText1.Size = new System.Drawing.Size(340, 23);
            this.lblText1.TabIndex = 85;
            this.lblText1.Text = "เลือกช่วงผลผลิต";
            this.lblText1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBegQnProj1
            // 
            this.txtBegQnProj1.Enabled = false;
            this.txtBegQnProj1.EnterMoveNextControl = true;
            this.txtBegQnProj1.Location = new System.Drawing.Point(215, 25);
            this.txtBegQnProj1.Name = "txtBegQnProj1";
            this.txtBegQnProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQnProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQnProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQnProj1.Properties.Appearance.Options.UseFont = true;
            this.txtBegQnProj1.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBegQnProj1.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBegQnProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQnProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQnProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtBegQnProj1.Size = new System.Drawing.Size(277, 22);
            this.txtBegQnProj1.TabIndex = 1;
            // 
            // txtEndQnProj1
            // 
            this.txtEndQnProj1.Enabled = false;
            this.txtEndQnProj1.EnterMoveNextControl = true;
            this.txtEndQnProj1.Location = new System.Drawing.Point(215, 49);
            this.txtEndQnProj1.Name = "txtEndQnProj1";
            this.txtEndQnProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQnProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQnProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQnProj1.Properties.Appearance.Options.UseFont = true;
            this.txtEndQnProj1.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtEndQnProj1.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtEndQnProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQnProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQnProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtEndQnProj1.Size = new System.Drawing.Size(277, 22);
            this.txtEndQnProj1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(7, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 23);
            this.label1.TabIndex = 82;
            this.label1.Text = "ถึง :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 85;
            this.label2.Text = "ตั้งแต่ :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndQcProj1
            // 
            this.txtEndQcProj1.EnterMoveNextControl = true;
            this.txtEndQcProj1.Location = new System.Drawing.Point(74, 49);
            this.txtEndQcProj1.Name = "txtEndQcProj1";
            this.txtEndQcProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcProj1.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtEndQcProj1.Size = new System.Drawing.Size(138, 22);
            this.txtEndQcProj1.TabIndex = 2;
            this.txtEndQcProj1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcProj1.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj1_Validating);
            this.txtEndQcProj1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQcProj1
            // 
            this.txtBegQcProj1.EnterMoveNextControl = true;
            this.txtBegQcProj1.Location = new System.Drawing.Point(74, 25);
            this.txtBegQcProj1.Name = "txtBegQcProj1";
            this.txtBegQcProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcProj1.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBegQcProj1.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtBegQcProj1.Size = new System.Drawing.Size(138, 22);
            this.txtBegQcProj1.TabIndex = 0;
            this.txtBegQcProj1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcProj1.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj1_Validating);
            this.txtBegQcProj1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(150, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 85;
            this.btnCancel.Text = "ย&กเลิก";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnPrint.Location = new System.Drawing.Point(82, 200);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 84;
            this.btnPrint.Text = "&ตกลง";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmOptionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTitle);
            this.Name = "frmOptionList";
            this.Text = "Option Report";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnProj1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnProj1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblText1;
        private DevExpress.XtraEditors.ButtonEdit txtBegQnProj1;
        private DevExpress.XtraEditors.ButtonEdit txtEndQnProj1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcProj1;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcProj1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
    }
}