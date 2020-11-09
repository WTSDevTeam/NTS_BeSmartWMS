namespace BeSmartMRP.Report
{
    partial class frmXRVATSale
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtQnBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.lblBranch = new System.Windows.Forms.Label();
            this.txtQcBranch = new DevExpress.XtraEditors.ButtonEdit();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.lblToDate = new System.Windows.Forms.Label();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.pnlVATType = new System.Windows.Forms.Panel();
            this.lblRngProd = new System.Windows.Forms.Label();
            this.lblToProd = new System.Windows.Forms.Label();
            this.lblFrProd = new System.Windows.Forms.Label();
            this.txtEndVat = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegVat = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).BeginInit();
            this.pnlDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            this.pnlVATType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndVat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegVat.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.panelControl1);
            this.pnlTitle.Controls.Add(this.pictureBox1);
            this.pnlTitle.Location = new System.Drawing.Point(-5, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(724, 76);
            this.pnlTitle.TabIndex = 295;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(70, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(641, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "พิมพ์รายงานการทำงานแต่ละวัน";
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
            this.panelControl1.Size = new System.Drawing.Size(725, 24);
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
            this.lblTaskName.Text = "TASKNAME : PMOSTAT";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BeSmartMRP.Properties.Resources.ico_lrg_9100;
            this.pictureBox1.Location = new System.Drawing.Point(15, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 48);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // txtQnBranch
            // 
            this.txtQnBranch.Enabled = false;
            this.txtQnBranch.EnterMoveNextControl = true;
            this.txtQnBranch.Location = new System.Drawing.Point(359, 103);
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtQnBranch.Size = new System.Drawing.Size(277, 22);
            this.txtQnBranch.TabIndex = 1;
            // 
            // lblBranch
            // 
            this.lblBranch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblBranch.Location = new System.Drawing.Point(48, 102);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(163, 23);
            this.lblBranch.TabIndex = 300;
            this.lblBranch.Text = "ระบุสาขา :";
            this.lblBranch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcBranch
            // 
            this.txtQcBranch.EnterMoveNextControl = true;
            this.txtQcBranch.Location = new System.Drawing.Point(217, 103);
            this.txtQcBranch.Name = "txtQcBranch";
            this.txtQcBranch.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBranch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBranch.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Appearance.Options.UseFont = true;
            this.txtQcBranch.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQcBranch.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQcBranch.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBranch.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.txtQcBranch.Size = new System.Drawing.Size(136, 22);
            this.txtQcBranch.TabIndex = 0;
            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcBranch_Validating);
            // 
            // pnlDate
            // 
            this.pnlDate.BackColor = System.Drawing.Color.Transparent;
            this.pnlDate.Controls.Add(this.label1);
            this.pnlDate.Controls.Add(this.txtBegDate);
            this.pnlDate.Controls.Add(this.lblDate);
            this.pnlDate.Controls.Add(this.txtEndDate);
            this.pnlDate.Controls.Add(this.lblToDate);
            this.pnlDate.Location = new System.Drawing.Point(51, 212);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(609, 73);
            this.pnlDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(163, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 23);
            this.label1.TabIndex = 85;
            this.label1.Text = "ใส่ช่วงเดือนที่ต้องยื่นภาษี :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBegDate
            // 
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(164, 31);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.txtBegDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "MM/yyyy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "MM/yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Size = new System.Drawing.Size(137, 22);
            this.txtBegDate.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblDate.Location = new System.Drawing.Point(-5, 30);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(163, 25);
            this.lblDate.TabIndex = 249;
            this.lblDate.Text = "วันที่ออกรายงาน :";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(386, 31);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, false)});
            this.txtEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "MM/yyyy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "MM/yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Size = new System.Drawing.Size(137, 22);
            this.txtEndDate.TabIndex = 1;
            // 
            // lblToDate
            // 
            this.lblToDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblToDate.Location = new System.Drawing.Point(307, 30);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(73, 22);
            this.lblToDate.TabIndex = 248;
            this.lblToDate.Text = "ถึงเดือน :";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnPrint.Location = new System.Drawing.Point(215, 310);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "&ตกลง";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(283, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "ย&กเลิก";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlVATType
            // 
            this.pnlVATType.Controls.Add(this.lblRngProd);
            this.pnlVATType.Controls.Add(this.lblToProd);
            this.pnlVATType.Controls.Add(this.lblFrProd);
            this.pnlVATType.Controls.Add(this.txtEndVat);
            this.pnlVATType.Controls.Add(this.txtBegVat);
            this.pnlVATType.Location = new System.Drawing.Point(49, 129);
            this.pnlVATType.Name = "pnlVATType";
            this.pnlVATType.Size = new System.Drawing.Size(610, 77);
            this.pnlVATType.TabIndex = 2;
            // 
            // lblRngProd
            // 
            this.lblRngProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblRngProd.Location = new System.Drawing.Point(164, 4);
            this.lblRngProd.Name = "lblRngProd";
            this.lblRngProd.Size = new System.Drawing.Size(340, 23);
            this.lblRngProd.TabIndex = 85;
            this.lblRngProd.Text = "ระบุช่วงประเภท VAT :";
            this.lblRngProd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblToProd
            // 
            this.lblToProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblToProd.Location = new System.Drawing.Point(44, 51);
            this.lblToProd.Name = "lblToProd";
            this.lblToProd.Size = new System.Drawing.Size(122, 23);
            this.lblToProd.TabIndex = 82;
            this.lblToProd.Text = "ถึง :";
            this.lblToProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrProd
            // 
            this.lblFrProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblFrProd.Location = new System.Drawing.Point(44, 27);
            this.lblFrProd.Name = "lblFrProd";
            this.lblFrProd.Size = new System.Drawing.Size(123, 23);
            this.lblFrProd.TabIndex = 85;
            this.lblFrProd.Text = "ตั้งแต่ :";
            this.lblFrProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndVat
            // 
            this.txtEndVat.EnterMoveNextControl = true;
            this.txtEndVat.Location = new System.Drawing.Point(167, 52);
            this.txtEndVat.Name = "txtEndVat";
            this.txtEndVat.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndVat.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndVat.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndVat.Properties.Appearance.Options.UseFont = true;
            this.txtEndVat.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndVat.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndVat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, true)});
            this.txtEndVat.Size = new System.Drawing.Size(181, 22);
            this.txtEndVat.TabIndex = 2;
            this.txtEndVat.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndVat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            this.txtEndVat.Validating += new System.ComponentModel.CancelEventHandler(this.txtVat_Validating);
            // 
            // txtBegVat
            // 
            this.txtBegVat.EnterMoveNextControl = true;
            this.txtBegVat.Location = new System.Drawing.Point(167, 28);
            this.txtBegVat.Name = "txtBegVat";
            this.txtBegVat.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegVat.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegVat.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegVat.Properties.Appearance.Options.UseFont = true;
            this.txtBegVat.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBegVat.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBegVat.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegVat.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegVat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, null, true)});
            this.txtBegVat.Size = new System.Drawing.Size(181, 22);
            this.txtBegVat.TabIndex = 0;
            this.txtBegVat.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegVat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            this.txtBegVat.Validating += new System.ComponentModel.CancelEventHandler(this.txtVat_Validating);
            // 
            // frmXRVATSale
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(712, 454);
            this.Controls.Add(this.pnlVATType);
            this.Controls.Add(this.pnlDate);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtQnBranch);
            this.Controls.Add(this.lblBranch);
            this.Controls.Add(this.txtQcBranch);
            this.Controls.Add(this.pnlTitle);
            this.Name = "frmXRVATSale";
            this.Text = "XRPVATSALE";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBranch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBranch.Properties)).EndInit();
            this.pnlDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            this.pnlVATType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndVat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegVat.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.ButtonEdit txtQnBranch;
        private System.Windows.Forms.Label lblBranch;
        private DevExpress.XtraEditors.ButtonEdit txtQcBranch;
        private System.Windows.Forms.Panel pnlDate;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraEditors.DateEdit txtEndDate;
        private System.Windows.Forms.Label lblToDate;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Panel pnlVATType;
        private System.Windows.Forms.Label lblRngProd;
        private System.Windows.Forms.Label lblToProd;
        private System.Windows.Forms.Label lblFrProd;
        private DevExpress.XtraEditors.ButtonEdit txtEndVat;
        private DevExpress.XtraEditors.ButtonEdit txtBegVat;
        private System.Windows.Forms.Label label1;
    }
}