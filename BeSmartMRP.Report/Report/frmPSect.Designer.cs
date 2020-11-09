namespace mBudget.Report
{
    partial class frmPSect
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
            frmPSect.pmClearInstanse();
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
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlRngCode = new System.Windows.Forms.Panel();
            this.txtBegCode = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndCode = new DevExpress.XtraEditors.ButtonEdit();
            this.pnlTagCode = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTagCode = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.cmbWRng = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPOrderBy = new DevExpress.XtraEditors.ComboBoxEdit();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTagDept = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlRngCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndCode.Properties)).BeginInit();
            this.pnlTagCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWRng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPOrderBy.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagDept.Properties)).BeginInit();
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
            this.pnlTitle.Location = new System.Drawing.Point(-3, -4);
            this.pnlTitle.LookAndFeel.SkinName = "Liquid Sky";
            this.pnlTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlTitle.LookAndFeel.UseWindowsXPTheme = false;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(561, 73);
            this.pnlTitle.TabIndex = 16;
            this.pnlTitle.Text = "pnlTitle";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(71, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(310, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "พิมพ์ทะเบียนแผนก";
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
            this.panelControl1.Size = new System.Drawing.Size(561, 24);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Text = "panelControl1";
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(71, 2);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "รหัสรายงาน : PSECTLST";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::mBudget.Properties.Resources.printer2;
            this.pictureBox1.Location = new System.Drawing.Point(15, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pnlRngCode
            // 
            this.pnlRngCode.Controls.Add(this.txtBegCode);
            this.pnlRngCode.Controls.Add(this.label1);
            this.pnlRngCode.Controls.Add(this.label2);
            this.pnlRngCode.Controls.Add(this.txtEndCode);
            this.pnlRngCode.Location = new System.Drawing.Point(12, 196);
            this.pnlRngCode.Name = "pnlRngCode";
            this.pnlRngCode.Size = new System.Drawing.Size(423, 51);
            this.pnlRngCode.TabIndex = 3;
            // 
            // txtBegCode
            // 
            this.txtBegCode.EnterMoveNextControl = true;
            this.txtBegCode.Location = new System.Drawing.Point(105, 0);
            this.txtBegCode.Name = "txtBegCode";
            this.txtBegCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegCode.Properties.Appearance.Options.UseFont = true;
            this.txtBegCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegCode.Size = new System.Drawing.Size(291, 22);
            this.txtBegCode.TabIndex = 0;
            this.txtBegCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrGrp_Validating);
            this.txtBegCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(23, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "ตั้งแต่ :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(23, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "ถึง :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEndCode
            // 
            this.txtEndCode.EnterMoveNextControl = true;
            this.txtEndCode.Location = new System.Drawing.Point(105, 28);
            this.txtEndCode.Name = "txtEndCode";
            this.txtEndCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndCode.Properties.Appearance.Options.UseFont = true;
            this.txtEndCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndCode.Size = new System.Drawing.Size(291, 22);
            this.txtEndCode.TabIndex = 1;
            this.txtEndCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrGrp_Validating);
            this.txtEndCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // pnlTagCode
            // 
            this.pnlTagCode.Controls.Add(this.label5);
            this.pnlTagCode.Controls.Add(this.txtTagCode);
            this.pnlTagCode.Location = new System.Drawing.Point(12, 253);
            this.pnlTagCode.Name = "pnlTagCode";
            this.pnlTagCode.Size = new System.Drawing.Size(423, 49);
            this.pnlTagCode.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(102, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "ระบุเป็นรายตัว :";
            // 
            // txtTagCode
            // 
            this.txtTagCode.EnterMoveNextControl = true;
            this.txtTagCode.Location = new System.Drawing.Point(104, 26);
            this.txtTagCode.Name = "txtTagCode";
            this.txtTagCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagCode.Properties.Appearance.Options.UseFont = true;
            this.txtTagCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtTagCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtTagCode.Properties.ReadOnly = true;
            this.txtTagCode.Size = new System.Drawing.Size(291, 22);
            this.txtTagCode.TabIndex = 0;
            this.txtTagCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::mBudget.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(184, 327);
            this.btnCancel.LookAndFeel.SkinName = "Money Twins";
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.LookAndFeel.UseWindowsXPTheme = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::mBudget.Properties.Resources.Print2;
            this.btnPrint.Location = new System.Drawing.Point(116, 327);
            this.btnPrint.LookAndFeel.SkinName = "Money Twins";
            this.btnPrint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnPrint.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPrint.LookAndFeel.UseWindowsXPTheme = false;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // cmbWRng
            // 
            this.cmbWRng.EnterMoveNextControl = true;
            this.cmbWRng.Location = new System.Drawing.Point(240, 168);
            this.cmbWRng.Name = "cmbWRng";
            this.cmbWRng.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbWRng.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbWRng.Properties.Appearance.Options.UseBackColor = true;
            this.cmbWRng.Properties.Appearance.Options.UseFont = true;
            this.cmbWRng.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbWRng.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbWRng.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbWRng.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbWRng.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbWRng.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbWRng.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.cmbWRng.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.cmbWRng.Properties.PopupSizeable = true;
            this.cmbWRng.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbWRng.Size = new System.Drawing.Size(168, 22);
            this.cmbWRng.TabIndex = 2;
            this.cmbWRng.SelectedIndexChanged += new System.EventHandler(this.cmbWRng_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(35, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 23);
            this.label6.TabIndex = 39;
            this.label6.Text = "การระบุช่วงข้อมูล :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(35, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 23);
            this.label3.TabIndex = 38;
            this.label3.Text = "เลือกช่วงแผนกเรียงตาม :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPOrderBy
            // 
            this.cmbPOrderBy.EnterMoveNextControl = true;
            this.cmbPOrderBy.Location = new System.Drawing.Point(240, 90);
            this.cmbPOrderBy.Name = "cmbPOrderBy";
            this.cmbPOrderBy.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbPOrderBy.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbPOrderBy.Properties.Appearance.Options.UseBackColor = true;
            this.cmbPOrderBy.Properties.Appearance.Options.UseFont = true;
            this.cmbPOrderBy.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbPOrderBy.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbPOrderBy.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbPOrderBy.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbPOrderBy.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbPOrderBy.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbPOrderBy.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.cmbPOrderBy.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.cmbPOrderBy.Properties.PopupSizeable = true;
            this.cmbPOrderBy.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbPOrderBy.Size = new System.Drawing.Size(168, 22);
            this.cmbPOrderBy.TabIndex = 0;
            this.cmbPOrderBy.SelectedIndexChanged += new System.EventHandler(this.cmbPOrderBy_SelectedIndexChanged);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            this.defaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtTagDept);
            this.panel1.Location = new System.Drawing.Point(12, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 49);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(102, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "ระบุฝ่าย :";
            // 
            // txtTagDept
            // 
            this.txtTagDept.EnterMoveNextControl = true;
            this.txtTagDept.Location = new System.Drawing.Point(104, 26);
            this.txtTagDept.Name = "txtTagDept";
            this.txtTagDept.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagDept.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagDept.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagDept.Properties.Appearance.Options.UseFont = true;
            this.txtTagDept.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagDept.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagDept.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtTagDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtTagDept.Properties.ReadOnly = true;
            this.txtTagDept.Size = new System.Drawing.Size(291, 22);
            this.txtTagDept.TabIndex = 0;
            this.txtTagDept.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagDept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // frmPSect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(554, 405);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlRngCode);
            this.Controls.Add(this.pnlTagCode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cmbWRng);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbPOrderBy);
            this.Controls.Add(this.pnlTitle);
            this.Name = "frmPSect";
            this.Text = "PSECTLST";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlRngCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBegCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndCode.Properties)).EndInit();
            this.pnlTagCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTagCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWRng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPOrderBy.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTagDept.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlRngCode;
        private DevExpress.XtraEditors.ButtonEdit txtBegCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndCode;
        private System.Windows.Forms.Panel pnlTagCode;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ButtonEdit txtTagCode;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.ComboBoxEdit cmbWRng;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPOrderBy;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit txtTagDept;
    }
}