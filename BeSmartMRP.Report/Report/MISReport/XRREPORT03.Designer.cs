namespace mBudget.Report.MISReport
{
    partial class XRREPORT03
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
            this.txtBegQnCrZone = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQcPdGrp = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQcCrZone = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEndQnCrZone = new DevExpress.XtraEditors.ButtonEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEndQcCrZone = new DevExpress.XtraEditors.ButtonEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.label88 = new System.Windows.Forms.Label();
            this.txtBegQnPdGrp = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEndQnPdGrp = new DevExpress.XtraEditors.ButtonEdit();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pnlRngCode = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndQcPdGrp = new DevExpress.XtraEditors.ButtonEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnCrZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcPdGrp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcCrZone.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnCrZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcCrZone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnPdGrp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnPdGrp.Properties)).BeginInit();
            this.pnlRngCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcPdGrp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBegQnCrZone
            // 
            this.txtBegQnCrZone.EnterMoveNextControl = true;
            this.txtBegQnCrZone.Location = new System.Drawing.Point(210, 0);
            this.txtBegQnCrZone.Name = "txtBegQnCrZone";
            this.txtBegQnCrZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQnCrZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQnCrZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQnCrZone.Properties.Appearance.Options.UseFont = true;
            this.txtBegQnCrZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQnCrZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQnCrZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQnCrZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQnCrZone.Size = new System.Drawing.Size(210, 22);
            this.txtBegQnCrZone.TabIndex = 1;
            this.txtBegQnCrZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQnCrZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrZone_Validating);
            this.txtBegQnCrZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQcPdGrp
            // 
            this.txtBegQcPdGrp.EnterMoveNextControl = true;
            this.txtBegQcPdGrp.Location = new System.Drawing.Point(105, 1);
            this.txtBegQcPdGrp.Name = "txtBegQcPdGrp";
            this.txtBegQcPdGrp.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcPdGrp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcPdGrp.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcPdGrp.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcPdGrp.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcPdGrp.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcPdGrp.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQcPdGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQcPdGrp.Size = new System.Drawing.Size(99, 22);
            this.txtBegQcPdGrp.TabIndex = 0;
            this.txtBegQcPdGrp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcPdGrp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQcCrZone
            // 
            this.txtBegQcCrZone.EnterMoveNextControl = true;
            this.txtBegQcCrZone.Location = new System.Drawing.Point(105, 1);
            this.txtBegQcCrZone.Name = "txtBegQcCrZone";
            this.txtBegQcCrZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcCrZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcCrZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcCrZone.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcCrZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcCrZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcCrZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQcCrZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQcCrZone.Size = new System.Drawing.Size(99, 22);
            this.txtBegQcCrZone.TabIndex = 0;
            this.txtBegQcCrZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcCrZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrZone_Validating);
            this.txtBegQcCrZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.txtEndQnCrZone);
            this.panel1.Controls.Add(this.txtBegQnCrZone);
            this.panel1.Controls.Add(this.txtBegQcCrZone);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtEndQcCrZone);
            this.panel1.Location = new System.Drawing.Point(48, 117);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 51);
            this.panel1.TabIndex = 66;
            // 
            // txtEndQnCrZone
            // 
            this.txtEndQnCrZone.EnterMoveNextControl = true;
            this.txtEndQnCrZone.Location = new System.Drawing.Point(210, 26);
            this.txtEndQnCrZone.Name = "txtEndQnCrZone";
            this.txtEndQnCrZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQnCrZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQnCrZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQnCrZone.Properties.Appearance.Options.UseFont = true;
            this.txtEndQnCrZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQnCrZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQnCrZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQnCrZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQnCrZone.Size = new System.Drawing.Size(210, 22);
            this.txtEndQnCrZone.TabIndex = 3;
            this.txtEndQnCrZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQnCrZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrZone_Validating);
            this.txtEndQnCrZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(23, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "ตั้งแต่ :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(23, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "ถึง :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndQcCrZone
            // 
            this.txtEndQcCrZone.EnterMoveNextControl = true;
            this.txtEndQcCrZone.Location = new System.Drawing.Point(105, 27);
            this.txtEndQcCrZone.Name = "txtEndQcCrZone";
            this.txtEndQcCrZone.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcCrZone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcCrZone.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcCrZone.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcCrZone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcCrZone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcCrZone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQcCrZone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQcCrZone.Size = new System.Drawing.Size(99, 22);
            this.txtEndQcCrZone.TabIndex = 2;
            this.txtEndQcCrZone.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcCrZone.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcCrZone_Validating);
            this.txtEndQcCrZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(71, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 23);
            this.label7.TabIndex = 75;
            this.label7.Text = "การระบุช่วงเขตการขาย :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::mBudget.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(221, 225);
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
            this.btnPrint.Location = new System.Drawing.Point(153, 225);
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
            this.txtBegDate.Location = new System.Drawing.Point(153, 183);
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
            this.label88.Location = new System.Drawing.Point(11, 183);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(136, 23);
            this.label88.TabIndex = 74;
            this.label88.Text = "ระบุปีที่ออกรายงาน :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBegQnPdGrp
            // 
            this.txtBegQnPdGrp.EnterMoveNextControl = true;
            this.txtBegQnPdGrp.Location = new System.Drawing.Point(210, 0);
            this.txtBegQnPdGrp.Name = "txtBegQnPdGrp";
            this.txtBegQnPdGrp.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQnPdGrp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQnPdGrp.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQnPdGrp.Properties.Appearance.Options.UseFont = true;
            this.txtBegQnPdGrp.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQnPdGrp.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQnPdGrp.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQnPdGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQnPdGrp.Size = new System.Drawing.Size(210, 22);
            this.txtBegQnPdGrp.TabIndex = 1;
            this.txtBegQnPdGrp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQnPdGrp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtEndQnPdGrp
            // 
            this.txtEndQnPdGrp.EnterMoveNextControl = true;
            this.txtEndQnPdGrp.Location = new System.Drawing.Point(210, 26);
            this.txtEndQnPdGrp.Name = "txtEndQnPdGrp";
            this.txtEndQnPdGrp.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQnPdGrp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQnPdGrp.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQnPdGrp.Properties.Appearance.Options.UseFont = true;
            this.txtEndQnPdGrp.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQnPdGrp.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQnPdGrp.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQnPdGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQnPdGrp.Size = new System.Drawing.Size(210, 22);
            this.txtEndQnPdGrp.TabIndex = 3;
            this.txtEndQnPdGrp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQnPdGrp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(79, 4);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "รหัสรายงาน : XRREPORT03";
            // 
            // pnlRngCode
            // 
            this.pnlRngCode.Controls.Add(this.txtEndQnPdGrp);
            this.pnlRngCode.Controls.Add(this.txtBegQnPdGrp);
            this.pnlRngCode.Controls.Add(this.txtBegQcPdGrp);
            this.pnlRngCode.Controls.Add(this.label1);
            this.pnlRngCode.Controls.Add(this.label2);
            this.pnlRngCode.Controls.Add(this.txtEndQcPdGrp);
            this.pnlRngCode.Location = new System.Drawing.Point(48, 284);
            this.pnlRngCode.Name = "pnlRngCode";
            this.pnlRngCode.Size = new System.Drawing.Size(423, 51);
            this.pnlRngCode.TabIndex = 65;
            this.pnlRngCode.Visible = false;
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
            // txtEndQcPdGrp
            // 
            this.txtEndQcPdGrp.EnterMoveNextControl = true;
            this.txtEndQcPdGrp.Location = new System.Drawing.Point(105, 27);
            this.txtEndQcPdGrp.Name = "txtEndQcPdGrp";
            this.txtEndQcPdGrp.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcPdGrp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcPdGrp.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcPdGrp.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcPdGrp.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcPdGrp.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcPdGrp.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQcPdGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQcPdGrp.Size = new System.Drawing.Size(99, 22);
            this.txtEndQcPdGrp.TabIndex = 2;
            this.txtEndQcPdGrp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcPdGrp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
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
            this.lblTitle.Text = "ยอดขายลูกค้าเก่า/ใหม่ทั่วประเทศ";
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
            this.label3.Location = new System.Drawing.Point(267, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 23);
            this.label3.TabIndex = 72;
            this.label3.Text = "ถึง :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(313, 184);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "MM/yyyy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Size = new System.Drawing.Size(99, 22);
            this.txtEndDate.TabIndex = 68;
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
            this.label6.Location = new System.Drawing.Point(71, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 23);
            this.label6.TabIndex = 73;
            this.label6.Text = "การระบุช่วงทีมขาย :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Visible = false;
            // 
            // XRREPORT03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.pnlRngCode);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label6);
            this.Name = "XRREPORT03";
            this.Text = "XRREPORT03";
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnCrZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcPdGrp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcCrZone.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnCrZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcCrZone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnPdGrp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnPdGrp.Properties)).EndInit();
            this.pnlRngCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcPdGrp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtBegQnCrZone;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcPdGrp;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcCrZone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.ButtonEdit txtEndQnCrZone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcCrZone;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.Label label88;
        private DevExpress.XtraEditors.ButtonEdit txtBegQnPdGrp;
        private DevExpress.XtraEditors.ButtonEdit txtEndQnPdGrp;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Panel pnlRngCode;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcPdGrp;
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