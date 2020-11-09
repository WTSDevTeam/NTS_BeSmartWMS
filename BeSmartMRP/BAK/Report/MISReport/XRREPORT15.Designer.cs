namespace mBudget.Report.MISReport
{
    partial class XRREPORT15
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
            this.components = new System.ComponentModel.Container();
            this.txtBegQcEmZone = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.label88 = new System.Windows.Forms.Label();
            this.txtBegQnEmZone = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEndQnEmZone = new DevExpress.XtraEditors.ButtonEdit();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pnlRngCode = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndQcEmZone = new DevExpress.XtraEditors.ButtonEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcEmZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnEmZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnEmZone.Properties)).BeginInit();
            this.pnlRngCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcEmZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBegQcEmZone
            // 
            this.txtBegQcEmZone.EnterMoveNextControl = true;
            this.txtBegQcEmZone.Location = new System.Drawing.Point(105, 1);
            this.txtBegQcEmZone.Name = "txtBegQcEmZone";
            this.txtBegQcEmZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcEmZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcEmZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcEmZone.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcEmZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcEmZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcEmZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQcEmZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQcEmZone.Size = new System.Drawing.Size(99, 22);
            this.txtBegQcEmZone.TabIndex = 0;
            this.txtBegQcEmZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcEmZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmZone_Validating);
            this.txtBegQcEmZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(23, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "ตั้งแต่ :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::mBudget.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(213, 215);
            this.btnCancel.LookAndFeel.SkinName = "Money Twins";
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.LookAndFeel.UseWindowsXPTheme = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 70;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::mBudget.Properties.Resources.Print2;
            this.btnPrint.Location = new System.Drawing.Point(145, 215);
            this.btnPrint.LookAndFeel.SkinName = "Money Twins";
            this.btnPrint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnPrint.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPrint.LookAndFeel.UseWindowsXPTheme = false;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 69;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtBegDate
            // 
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(145, 173);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "MM/yyyy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Size = new System.Drawing.Size(99, 22);
            this.txtBegDate.TabIndex = 67;
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label88.Location = new System.Drawing.Point(3, 173);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(136, 23);
            this.label88.TabIndex = 74;
            this.label88.Text = "ระบุปีที่ออกรายงาน :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBegQnEmZone
            // 
            this.txtBegQnEmZone.EnterMoveNextControl = true;
            this.txtBegQnEmZone.Location = new System.Drawing.Point(210, 0);
            this.txtBegQnEmZone.Name = "txtBegQnEmZone";
            this.txtBegQnEmZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQnEmZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQnEmZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQnEmZone.Properties.Appearance.Options.UseFont = true;
            this.txtBegQnEmZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQnEmZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQnEmZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQnEmZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQnEmZone.Size = new System.Drawing.Size(210, 22);
            this.txtBegQnEmZone.TabIndex = 1;
            this.txtBegQnEmZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQnEmZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmZone_Validating);
            this.txtBegQnEmZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtEndQnEmZone
            // 
            this.txtEndQnEmZone.EnterMoveNextControl = true;
            this.txtEndQnEmZone.Location = new System.Drawing.Point(210, 26);
            this.txtEndQnEmZone.Name = "txtEndQnEmZone";
            this.txtEndQnEmZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQnEmZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQnEmZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQnEmZone.Properties.Appearance.Options.UseFont = true;
            this.txtEndQnEmZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQnEmZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQnEmZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQnEmZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQnEmZone.Size = new System.Drawing.Size(210, 22);
            this.txtEndQnEmZone.TabIndex = 3;
            this.txtEndQnEmZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQnEmZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmZone_Validating);
            this.txtEndQnEmZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(79, 4);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "รหัสรายงาน : XRREPORT15";
            // 
            // pnlRngCode
            // 
            this.pnlRngCode.Controls.Add(this.txtEndQnEmZone);
            this.pnlRngCode.Controls.Add(this.txtBegQnEmZone);
            this.pnlRngCode.Controls.Add(this.txtBegQcEmZone);
            this.pnlRngCode.Controls.Add(this.label1);
            this.pnlRngCode.Controls.Add(this.label2);
            this.pnlRngCode.Controls.Add(this.txtEndQcEmZone);
            this.pnlRngCode.Location = new System.Drawing.Point(40, 116);
            this.pnlRngCode.Name = "pnlRngCode";
            this.pnlRngCode.Size = new System.Drawing.Size(423, 51);
            this.pnlRngCode.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(23, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "ถึง :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndQcEmZone
            // 
            this.txtEndQcEmZone.EnterMoveNextControl = true;
            this.txtEndQcEmZone.Location = new System.Drawing.Point(105, 27);
            this.txtEndQcEmZone.Name = "txtEndQcEmZone";
            this.txtEndQcEmZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcEmZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcEmZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcEmZone.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcEmZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcEmZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcEmZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQcEmZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQcEmZone.Size = new System.Drawing.Size(99, 22);
            this.txtEndQcEmZone.TabIndex = 2;
            this.txtEndQcEmZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcEmZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmZone_Validating);
            this.txtEndQcEmZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.lblTaskName);
            this.panelControl1.Location = new System.Drawing.Point(-2, 47);
            this.panelControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.LookAndFeel.UseWindowsXPTheme = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(589, 24);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Text = "panelControl1";
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
            this.pnlTitle.Location = new System.Drawing.Point(-3, -2);
            this.pnlTitle.LookAndFeel.SkinName = "Liquid Sky";
            this.pnlTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlTitle.LookAndFeel.UseWindowsXPTheme = false;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(589, 73);
            this.pnlTitle.TabIndex = 71;
            this.pnlTitle.Text = "panelControl1";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(71, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(512, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "รายงานการรับคืนแยกตามพนักงานขาย";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::mBudget.Properties.Resources.chart;
            this.pictureBox1.Location = new System.Drawing.Point(15, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(259, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 23);
            this.label3.TabIndex = 72;
            this.label3.Text = "ถึง :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(305, 174);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "yyyy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Size = new System.Drawing.Size(99, 22);
            this.txtEndDate.TabIndex = 68;
            this.txtEndDate.Visible = false;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            this.defaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(63, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 23);
            this.label6.TabIndex = 73;
            this.label6.Text = "การระบุช่วงทีมขาย :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // XRREPORT15
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.pnlRngCode);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label6);
            this.Name = "XRREPORT15";
            this.Text = "XRREPORT15";
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcEmZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnEmZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnEmZone.Properties)).EndInit();
            this.pnlRngCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcEmZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtBegQcEmZone;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.Label label88;
        private DevExpress.XtraEditors.ButtonEdit txtBegQnEmZone;
        private DevExpress.XtraEditors.ButtonEdit txtEndQnEmZone;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Panel pnlRngCode;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcEmZone;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.DateEdit txtEndDate;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.Label label6;
    }
}