namespace mBudget.Report.MISReport
{
    partial class XRREPORT04
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
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.pnlRngCode = new System.Windows.Forms.Panel();
            this.txtBegQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEndQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEndQnProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtBegQnProd = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            this.pnlRngCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(137, 194);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Size = new System.Drawing.Size(125, 22);
            this.txtEndDate.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(55, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 23);
            this.label3.TabIndex = 43;
            this.label3.Text = "ถึงวันที่ :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label88.Location = new System.Drawing.Point(55, 167);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(76, 23);
            this.label88.TabIndex = 42;
            this.label88.Text = "ตั้งแต่วันที่ :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(23, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "ถึง :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBegDate
            // 
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(137, 167);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Size = new System.Drawing.Size(125, 22);
            this.txtBegDate.TabIndex = 37;
            // 
            // pnlRngCode
            // 
            this.pnlRngCode.Controls.Add(this.txtBegQcProd);
            this.pnlRngCode.Controls.Add(this.txtEndQcProd);
            this.pnlRngCode.Controls.Add(this.txtEndQnProd);
            this.pnlRngCode.Controls.Add(this.txtBegQnProd);
            this.pnlRngCode.Controls.Add(this.label1);
            this.pnlRngCode.Controls.Add(this.label2);
            this.pnlRngCode.Location = new System.Drawing.Point(32, 113);
            this.pnlRngCode.Name = "pnlRngCode";
            this.pnlRngCode.Size = new System.Drawing.Size(423, 51);
            this.pnlRngCode.TabIndex = 35;
            // 
            // txtBegQcProd
            // 
            this.txtBegQcProd.EnterMoveNextControl = true;
            this.txtBegQcProd.Location = new System.Drawing.Point(105, 0);
            this.txtBegQcProd.Name = "txtBegQcProd";
            this.txtBegQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcProd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQcProd.Size = new System.Drawing.Size(99, 22);
            this.txtBegQcProd.TabIndex = 0;
            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProd_Validating);
            this.txtBegQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtEndQcProd
            // 
            this.txtEndQcProd.EnterMoveNextControl = true;
            this.txtEndQcProd.Location = new System.Drawing.Point(105, 26);
            this.txtEndQcProd.Name = "txtEndQcProd";
            this.txtEndQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcProd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQcProd.Size = new System.Drawing.Size(99, 22);
            this.txtEndQcProd.TabIndex = 2;
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProd_Validating);
            this.txtEndQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtEndQnProd
            // 
            this.txtEndQnProd.EnterMoveNextControl = true;
            this.txtEndQnProd.Location = new System.Drawing.Point(210, 26);
            this.txtEndQnProd.Name = "txtEndQnProd";
            this.txtEndQnProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQnProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQnProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQnProd.Properties.Appearance.Options.UseFont = true;
            this.txtEndQnProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQnProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQnProd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndQnProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtEndQnProd.Size = new System.Drawing.Size(210, 22);
            this.txtEndQnProd.TabIndex = 3;
            this.txtEndQnProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQnProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProd_Validating);
            this.txtEndQnProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtBegQnProd
            // 
            this.txtBegQnProd.EnterMoveNextControl = true;
            this.txtBegQnProd.Location = new System.Drawing.Point(210, 0);
            this.txtBegQnProd.Name = "txtBegQnProd";
            this.txtBegQnProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQnProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQnProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQnProd.Properties.Appearance.Options.UseFont = true;
            this.txtBegQnProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQnProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQnProd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegQnProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtBegQnProd.Size = new System.Drawing.Size(210, 22);
            this.txtBegQnProd.TabIndex = 1;
            this.txtBegQnProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQnProd.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProd_Validating);
            this.txtBegQnProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(23, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "ตั้งแต่ :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 276);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(558, 117);
            this.dataGridView1.TabIndex = 44;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.Visible = false;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            this.defaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::mBudget.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(205, 243);
            this.btnCancel.LookAndFeel.SkinName = "Money Twins";
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.LookAndFeel.UseWindowsXPTheme = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::mBudget.Properties.Resources.Print2;
            this.btnPrint.Location = new System.Drawing.Point(137, 243);
            this.btnPrint.LookAndFeel.SkinName = "Money Twins";
            this.btnPrint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnPrint.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPrint.LookAndFeel.UseWindowsXPTheme = false;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 39;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(58, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(246, 23);
            this.label6.TabIndex = 41;
            this.label6.Text = "การระบุช่วงสินค้า :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(71, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(310, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "ข้อมูลยอดขายและ Stock";
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.lblTaskName);
            this.panelControl1.Location = new System.Drawing.Point(-2, 46);
            this.panelControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.LookAndFeel.UseWindowsXPTheme = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(589, 24);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Text = "panelControl1";
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(79, 3);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "รหัสรายงาน : XRREPORT04";
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
            this.pnlTitle.Location = new System.Drawing.Point(-3, -3);
            this.pnlTitle.LookAndFeel.SkinName = "Liquid Sky";
            this.pnlTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlTitle.LookAndFeel.UseWindowsXPTheme = false;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(589, 73);
            this.pnlTitle.TabIndex = 36;
            this.pnlTitle.Text = "pnlTitle";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::mBudget.Properties.Resources.chart;
            this.pictureBox1.Location = new System.Drawing.Point(15, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // XRREPORT04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.pnlRngCode);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pnlTitle);
            this.Name = "XRREPORT04";
            this.Text = "XRREPORT04";
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            this.pnlRngCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQnProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQnProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.Panel pnlRngCode;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcProd;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcProd;
        private DevExpress.XtraEditors.ButtonEdit txtEndQnProd;
        private DevExpress.XtraEditors.ButtonEdit txtBegQnProd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}