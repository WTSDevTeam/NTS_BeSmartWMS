namespace BeSmartMRP.DatabaseForms
{
    partial class frmBudTran
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
            frmBudTran.pmClearInstanse(this.mstrBudgetStep);
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
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.grbAPVStat = new DevExpress.XtraEditors.GroupControl();
            this.pnlOut = new System.Windows.Forms.Panel();
            this.chkIsOut = new DevExpress.XtraEditors.CheckEdit();
            this.txtAPVRemark = new DevExpress.XtraEditors.MemoEdit();
            this.rdoAPVStat = new DevExpress.XtraEditors.RadioGroup();
            this.label3 = new System.Windows.Forms.Label();
            this.grbApprove = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtIsApprove = new DevExpress.XtraEditors.ButtonEdit();
            this.lblApprove = new System.Windows.Forms.Label();
            this.txtDApprove = new DevExpress.XtraEditors.DateEdit();
            this.txtApproveBy = new DevExpress.XtraEditors.ButtonEdit();
            this.lblDApprove = new System.Windows.Forms.Label();
            this.txtLastUpd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtStat = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnLastUpd = new DevExpress.XtraEditors.ButtonEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pnlGridControl = new System.Windows.Forms.Panel();
            this.btnMoveRowDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnMoveRowUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnRemoveRow = new DevExpress.XtraEditors.SimpleButton();
            this.btnInsertRow = new DevExpress.XtraEditors.SimpleButton();
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.tbrType = new DevExpress.XtraBars.BarEditItem();
            this.tbcFilter = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.tbrTXCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtFooter = new DevExpress.XtraEditors.MemoEdit();
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcIsOut = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grcApprove = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAPVStat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcIsPost = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtTotAmt = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnEmpl = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcEmpl = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnJob = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcJob = new DevExpress.XtraEditors.ButtonEdit();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.grcQcMachine = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcQnMachine = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcQcDept = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcAllocPcn = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQty = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQnUM = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcAmt = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.txtRevise = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnSect = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcSect = new DevExpress.XtraEditors.ButtonEdit();
            this.txtYear = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.grbAPVStat)).BeginInit();
            this.grbAPVStat.SuspendLayout();
            this.pnlOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAPVRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoAPVStat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbApprove)).BeginInit();
            this.grbApprove.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIsApprove.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDApprove.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDApprove.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApproveBy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastUpd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnLastUpd.Properties)).BeginInit();
            this.pnlGridControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnEmpl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcEmpl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAllocPcn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnUM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRevise.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTipController1
            // 
            this.toolTipController1.Rounded = true;
            // 
            // grbAPVStat
            // 
            this.grbAPVStat.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grbAPVStat.Appearance.Options.UseBackColor = true;
            this.grbAPVStat.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.grbAPVStat.AppearanceCaption.Options.UseFont = true;
            this.grbAPVStat.AppearanceCaption.Options.UseTextOptions = true;
            this.grbAPVStat.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.grbAPVStat.Controls.Add(this.pnlOut);
            this.grbAPVStat.Controls.Add(this.txtAPVRemark);
            this.grbAPVStat.Controls.Add(this.rdoAPVStat);
            this.grbAPVStat.Controls.Add(this.label3);
            this.grbAPVStat.Location = new System.Drawing.Point(521, 15);
            this.grbAPVStat.LookAndFeel.SkinName = "Blue";
            this.grbAPVStat.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbAPVStat.Name = "grbAPVStat";
            this.grbAPVStat.Size = new System.Drawing.Size(242, 210);
            this.grbAPVStat.TabIndex = 6;
            this.grbAPVStat.Text = "สถานะการพิจารณา";
            // 
            // pnlOut
            // 
            this.pnlOut.BackColor = System.Drawing.Color.Transparent;
            this.pnlOut.Controls.Add(this.chkIsOut);
            this.pnlOut.Location = new System.Drawing.Point(8, 159);
            this.pnlOut.Name = "pnlOut";
            this.pnlOut.Size = new System.Drawing.Size(229, 23);
            this.pnlOut.TabIndex = 38;
            // 
            // chkIsOut
            // 
            this.chkIsOut.Location = new System.Drawing.Point(25, 0);
            this.chkIsOut.Name = "chkIsOut";
            this.chkIsOut.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.chkIsOut.Properties.Appearance.Options.UseFont = true;
            this.chkIsOut.Properties.Caption = "ผ่านหน่วยงานภายนอก :";
            this.chkIsOut.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsOut.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkIsOut.Size = new System.Drawing.Size(169, 22);
            this.chkIsOut.TabIndex = 38;
            this.chkIsOut.TabStop = false;
            // 
            // txtAPVRemark
            // 
            this.txtAPVRemark.Location = new System.Drawing.Point(60, 85);
            this.txtAPVRemark.Name = "txtAPVRemark";
            this.txtAPVRemark.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtAPVRemark.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtAPVRemark.Size = new System.Drawing.Size(177, 68);
            this.txtAPVRemark.TabIndex = 1;
            this.txtAPVRemark.TabStop = false;
            // 
            // rdoAPVStat
            // 
            this.rdoAPVStat.Location = new System.Drawing.Point(8, 29);
            this.rdoAPVStat.Name = "rdoAPVStat";
            this.rdoAPVStat.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoAPVStat.Properties.Appearance.Options.UseBackColor = true;
            this.rdoAPVStat.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("R", "พิจารณาไม่ผ่าน (REJECT)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("P", "พิจารณาผ่าน (PASS)")});
            this.rdoAPVStat.Properties.EditValueChanged += new System.EventHandler(this.rdoAPVStat_Properties_EditValueChanged);
            this.rdoAPVStat.Size = new System.Drawing.Size(229, 50);
            this.rdoAPVStat.TabIndex = 0;
            this.rdoAPVStat.DoubleClick += new System.EventHandler(this.rdoAPVStat_DoubleClick);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(5, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 34;
            this.label3.Text = "สาเหตุ :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grbApprove
            // 
            this.grbApprove.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grbApprove.Appearance.Options.UseBackColor = true;
            this.grbApprove.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.grbApprove.AppearanceCaption.Options.UseFont = true;
            this.grbApprove.AppearanceCaption.Options.UseTextOptions = true;
            this.grbApprove.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.grbApprove.Controls.Add(this.panel1);
            this.grbApprove.Controls.Add(this.txtLastUpd);
            this.grbApprove.Controls.Add(this.txtStat);
            this.grbApprove.Controls.Add(this.txtQnLastUpd);
            this.grbApprove.Controls.Add(this.label10);
            this.grbApprove.Controls.Add(this.label4);
            this.grbApprove.Controls.Add(this.label9);
            this.grbApprove.Location = new System.Drawing.Point(10, 115);
            this.grbApprove.LookAndFeel.SkinName = "Blue";
            this.grbApprove.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbApprove.Name = "grbApprove";
            this.grbApprove.Size = new System.Drawing.Size(505, 110);
            this.grbApprove.TabIndex = 4;
            this.grbApprove.Text = "ผู้อนุมัติเบื้องต้น (หัวหน้าหน่วยงานผู้เสนองบประมาณ)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.txtIsApprove);
            this.panel1.Controls.Add(this.lblApprove);
            this.panel1.Controls.Add(this.txtDApprove);
            this.panel1.Controls.Add(this.txtApproveBy);
            this.panel1.Controls.Add(this.lblDApprove);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 25);
            this.panel1.TabIndex = 0;
            // 
            // txtIsApprove
            // 
            this.txtIsApprove.EnterMoveNextControl = true;
            this.txtIsApprove.Location = new System.Drawing.Point(103, 1);
            this.txtIsApprove.Name = "txtIsApprove";
            this.txtIsApprove.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtIsApprove.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtIsApprove.Properties.Appearance.Options.UseBackColor = true;
            this.txtIsApprove.Properties.Appearance.Options.UseFont = true;
            this.txtIsApprove.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.Control;
            this.txtIsApprove.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtIsApprove.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtIsApprove.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtIsApprove.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtIsApprove.Properties.MaxLength = 1;
            this.txtIsApprove.Size = new System.Drawing.Size(26, 22);
            this.txtIsApprove.TabIndex = 0;
            this.txtIsApprove.EditValueChanged += new System.EventHandler(this.txtIsApprove_EditValueChanged);
            this.txtIsApprove.DoubleClick += new System.EventHandler(this.txtIsApprove_Click);
            this.txtIsApprove.Enter += new System.EventHandler(this.txtIsApprove_Enter);
            // 
            // lblApprove
            // 
            this.lblApprove.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblApprove.Location = new System.Drawing.Point(-11, 1);
            this.lblApprove.Name = "lblApprove";
            this.lblApprove.Size = new System.Drawing.Size(115, 23);
            this.lblApprove.TabIndex = 33;
            this.lblApprove.Text = "อนุมัติ :";
            this.lblApprove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDApprove
            // 
            this.txtDApprove.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtDApprove.Enabled = false;
            this.txtDApprove.EnterMoveNextControl = true;
            this.txtDApprove.Location = new System.Drawing.Point(324, 1);
            this.txtDApprove.Name = "txtDApprove";
            this.txtDApprove.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtDApprove.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtDApprove.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtDApprove.Properties.Appearance.Options.UseBackColor = true;
            this.txtDApprove.Properties.Appearance.Options.UseFont = true;
            this.txtDApprove.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtDApprove.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtDApprove.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtDApprove.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtDApprove.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtDApprove.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtDApprove.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtDApprove.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtDApprove.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDApprove.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDApprove.Size = new System.Drawing.Size(118, 22);
            this.txtDApprove.TabIndex = 1;
            // 
            // txtApproveBy
            // 
            this.txtApproveBy.Enabled = false;
            this.txtApproveBy.EnterMoveNextControl = true;
            this.txtApproveBy.Location = new System.Drawing.Point(135, 1);
            this.txtApproveBy.Name = "txtApproveBy";
            this.txtApproveBy.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtApproveBy.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtApproveBy.Properties.Appearance.Options.UseBackColor = true;
            this.txtApproveBy.Properties.Appearance.Options.UseFont = true;
            this.txtApproveBy.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtApproveBy.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtApproveBy.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtApproveBy.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtApproveBy.Size = new System.Drawing.Size(131, 22);
            this.txtApproveBy.TabIndex = 0;
            // 
            // lblDApprove
            // 
            this.lblDApprove.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblDApprove.Location = new System.Drawing.Point(241, 1);
            this.lblDApprove.Name = "lblDApprove";
            this.lblDApprove.Size = new System.Drawing.Size(84, 23);
            this.lblDApprove.TabIndex = 5;
            this.lblDApprove.Text = "วันที่ :";
            this.lblDApprove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastUpd
            // 
            this.txtLastUpd.Enabled = false;
            this.txtLastUpd.EnterMoveNextControl = true;
            this.txtLastUpd.Location = new System.Drawing.Point(115, 77);
            this.txtLastUpd.Name = "txtLastUpd";
            this.txtLastUpd.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtLastUpd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtLastUpd.Properties.Appearance.Options.UseBackColor = true;
            this.txtLastUpd.Properties.Appearance.Options.UseFont = true;
            this.txtLastUpd.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtLastUpd.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtLastUpd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtLastUpd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtLastUpd.Size = new System.Drawing.Size(163, 22);
            this.txtLastUpd.TabIndex = 0;
            // 
            // txtStat
            // 
            this.txtStat.Enabled = false;
            this.txtStat.EnterMoveNextControl = true;
            this.txtStat.Location = new System.Drawing.Point(115, 53);
            this.txtStat.Name = "txtStat";
            this.txtStat.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtStat.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtStat.Properties.Appearance.Options.UseBackColor = true;
            this.txtStat.Properties.Appearance.Options.UseFont = true;
            this.txtStat.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtStat.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtStat.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtStat.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtStat.Size = new System.Drawing.Size(163, 22);
            this.txtStat.TabIndex = 0;
            // 
            // txtQnLastUpd
            // 
            this.txtQnLastUpd.Enabled = false;
            this.txtQnLastUpd.EnterMoveNextControl = true;
            this.txtQnLastUpd.Location = new System.Drawing.Point(361, 78);
            this.txtQnLastUpd.Name = "txtQnLastUpd";
            this.txtQnLastUpd.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnLastUpd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnLastUpd.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnLastUpd.Properties.Appearance.Options.UseFont = true;
            this.txtQnLastUpd.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtQnLastUpd.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtQnLastUpd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnLastUpd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnLastUpd.Size = new System.Drawing.Size(93, 22);
            this.txtQnLastUpd.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(1, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 23);
            this.label10.TabIndex = 33;
            this.label10.Text = "แก้ไขล่าสุด :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(1, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 23);
            this.label4.TabIndex = 33;
            this.label4.Text = "สถานะ :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(268, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 23);
            this.label9.TabIndex = 33;
            this.label9.Text = "โดย User :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(15, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 23);
            this.label5.TabIndex = 33;
            this.label5.Text = "งบประมาณรวม :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(-16, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 23);
            this.label2.TabIndex = 33;
            this.label2.Text = "ผู้เสนองบประมาณ :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(-16, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 23);
            this.label1.TabIndex = 33;
            this.label1.Text = "โครงการ :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(236, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "REVISE :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(-16, 15);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(142, 23);
            this.lblCode.TabIndex = 5;
            this.lblCode.Text = "ปีงบประมาณ :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(-16, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 23);
            this.label7.TabIndex = 33;
            this.label7.Text = "แผนก :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 474);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pnlGridControl
            // 
            this.pnlGridControl.BackColor = System.Drawing.Color.Transparent;
            this.pnlGridControl.Controls.Add(this.btnMoveRowDown);
            this.pnlGridControl.Controls.Add(this.btnMoveRowUp);
            this.pnlGridControl.Controls.Add(this.btnRemoveRow);
            this.pnlGridControl.Controls.Add(this.btnInsertRow);
            this.pnlGridControl.Location = new System.Drawing.Point(294, 231);
            this.pnlGridControl.Name = "pnlGridControl";
            this.pnlGridControl.Size = new System.Drawing.Size(96, 25);
            this.pnlGridControl.TabIndex = 34;
            // 
            // btnMoveRowDown
            // 
            this.btnMoveRowDown.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnMoveRowDown.Appearance.Options.UseFont = true;
            this.btnMoveRowDown.Image = global::BeSmartMRP.Properties.Resources.arrow_down_green;
            this.btnMoveRowDown.Location = new System.Drawing.Point(66, 0);
            this.btnMoveRowDown.Name = "btnMoveRowDown";
            this.btnMoveRowDown.Size = new System.Drawing.Size(23, 25);
            this.btnMoveRowDown.TabIndex = 3;
            this.btnMoveRowDown.TabStop = false;
            this.btnMoveRowDown.Visible = false;
            this.btnMoveRowDown.Click += new System.EventHandler(this.btnMoveRowDown_Click);
            // 
            // btnMoveRowUp
            // 
            this.btnMoveRowUp.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnMoveRowUp.Appearance.Options.UseFont = true;
            this.btnMoveRowUp.Image = global::BeSmartMRP.Properties.Resources.arrow_up_green;
            this.btnMoveRowUp.Location = new System.Drawing.Point(44, 0);
            this.btnMoveRowUp.Name = "btnMoveRowUp";
            this.btnMoveRowUp.Size = new System.Drawing.Size(23, 25);
            this.btnMoveRowUp.TabIndex = 3;
            this.btnMoveRowUp.TabStop = false;
            this.btnMoveRowUp.Visible = false;
            this.btnMoveRowUp.Click += new System.EventHandler(this.btnMoveRowUp_Click);
            // 
            // btnRemoveRow
            // 
            this.btnRemoveRow.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnRemoveRow.Appearance.Options.UseFont = true;
            this.btnRemoveRow.Image = global::BeSmartMRP.Properties.Resources.RemoveRow;
            this.btnRemoveRow.Location = new System.Drawing.Point(22, 0);
            this.btnRemoveRow.Name = "btnRemoveRow";
            this.btnRemoveRow.Size = new System.Drawing.Size(23, 25);
            this.btnRemoveRow.TabIndex = 3;
            this.btnRemoveRow.TabStop = false;
            this.btnRemoveRow.ToolTip = "ลบบรรทัด";
            this.btnRemoveRow.Click += new System.EventHandler(this.btnRemoveRow_Click);
            // 
            // btnInsertRow
            // 
            this.btnInsertRow.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnInsertRow.Appearance.Options.UseFont = true;
            this.btnInsertRow.Image = global::BeSmartMRP.Properties.Resources.AddRow;
            this.btnInsertRow.Location = new System.Drawing.Point(0, 0);
            this.btnInsertRow.Name = "btnInsertRow";
            this.btnInsertRow.Size = new System.Drawing.Size(23, 25);
            this.btnInsertRow.TabIndex = 3;
            this.btnInsertRow.TabStop = false;
            this.btnInsertRow.ToolTip = "แทรกบรรทัด";
            this.btnInsertRow.Click += new System.EventHandler(this.btnInsertRow_Click);
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMain,
            this.bar1});
            this.barMainEdit.Controller = this.barAndDockingController1;
            this.barMainEdit.DockControls.Add(this.barDockControlTop);
            this.barMainEdit.DockControls.Add(this.barDockControlBottom);
            this.barMainEdit.DockControls.Add(this.barDockControlLeft);
            this.barMainEdit.DockControls.Add(this.barDockControlRight);
            this.barMainEdit.Form = this;
            this.barMainEdit.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.tbrTXCopy,
            this.tbrType,
            this.barButtonItem1});
            this.barMainEdit.MaxItemId = 3;
            this.barMainEdit.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.tbcFilter});
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            // 
            // barMain
            // 
            this.barMain.BarName = "Custom 1";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.AllowRename = true;
            this.barMain.OptionsBar.DisableCustomization = true;
            this.barMain.OptionsBar.DrawSizeGrip = true;
            this.barMain.Text = "Standard";
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 1;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.tbrType),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tbrTXCopy, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Offset = 23;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.Text = "Option";
            // 
            // tbrType
            // 
            this.tbrType.Caption = "สถานะ";
            this.tbrType.Edit = this.tbcFilter;
            this.tbrType.Id = 1;
            this.tbrType.Name = "tbrType";
            this.tbrType.Width = 120;
            // 
            // tbcFilter
            // 
            this.tbcFilter.AutoHeight = false;
            this.tbcFilter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tbcFilter.Items.AddRange(new object[] {
            "",
            "WAIT",
            "APPROVE",
            "REJECT"});
            this.tbcFilter.Name = "tbcFilter";
            this.tbcFilter.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tbcFilter.SelectedValueChanged += new System.EventHandler(this.tbcFilter_SelectedValueChanged);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "พิมพ์";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.Print2;
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "PRINT";
            // 
            // tbrTXCopy
            // 
            this.tbrTXCopy.Caption = "Copy";
            this.tbrTXCopy.Glyph = global::BeSmartMRP.Properties.Resources.Export;
            this.tbrTXCopy.Id = 0;
            this.tbrTXCopy.Name = "tbrTXCopy";
            this.tbrTXCopy.Tag = "TXCOPY";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 25);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(784, 471);
            this.pgfMainEdit.TabIndex = 9;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.data_table;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(784, 447);
            this.xtraTabPage1.Text = "Browse";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(7, 350);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(771, 68);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdBrowView.Location = new System.Drawing.Point(7, 5);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcIsOut,
            this.grcApprove,
            this.grcAPVStat,
            this.grcIsPost});
            this.grdBrowView.Size = new System.Drawing.Size(770, 341);
            this.grdBrowView.TabIndex = 0;
            this.grdBrowView.UseEmbeddedNavigator = true;
            this.grdBrowView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.grdBrowView.Resize += new System.EventHandler(this.grdBrowView_Resize);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.gridView1.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.FocusedCell.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grdBrowView;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.EndSorting += new System.EventHandler(this.gridView1_EndSorting);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gridView1_CustomRowFilter);
            this.gridView1.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView1_ColumnWidthChanged);
            // 
            // grcIsOut
            // 
            this.grcIsOut.AutoHeight = false;
            this.grcIsOut.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.grcIsOut.Name = "grcIsOut";
            this.grcIsOut.CheckedChanged += new System.EventHandler(this.grcIsOut_CheckedChanged);
            // 
            // grcApprove
            // 
            this.grcApprove.AutoHeight = false;
            this.grcApprove.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grcApprove.Items.AddRange(new object[] {
            " ",
            "/",
            "R"});
            this.grcApprove.Name = "grcApprove";
            this.grcApprove.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
            this.grcApprove.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.grcApprove.SelectedValueChanged += new System.EventHandler(this.grcApprove_SelectedValueChanged);
            // 
            // grcAPVStat
            // 
            this.grcAPVStat.AutoHeight = false;
            this.grcAPVStat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grcAPVStat.Items.AddRange(new object[] {
            " ",
            "/",
            "R"});
            this.grcAPVStat.Name = "grcAPVStat";
            this.grcAPVStat.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
            this.grcAPVStat.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.grcAPVStat.SelectedValueChanged += new System.EventHandler(this.grcAPVStat_SelectedValueChanged);
            // 
            // grcIsPost
            // 
            this.grcIsPost.AutoHeight = false;
            this.grcIsPost.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.grcIsPost.Name = "grcIsPost";
            this.grcIsPost.CheckedChanged += new System.EventHandler(this.grcIsPost_CheckedChanged);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage2.Controls.Add(this.pnlGridControl);
            this.xtraTabPage2.Controls.Add(this.grbAPVStat);
            this.xtraTabPage2.Controls.Add(this.grbApprove);
            this.xtraTabPage2.Controls.Add(this.txtTotAmt);
            this.xtraTabPage2.Controls.Add(this.label5);
            this.xtraTabPage2.Controls.Add(this.txtQnEmpl);
            this.xtraTabPage2.Controls.Add(this.txtQcEmpl);
            this.xtraTabPage2.Controls.Add(this.txtQnJob);
            this.xtraTabPage2.Controls.Add(this.txtQcJob);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.grdTemPd);
            this.xtraTabPage2.Controls.Add(this.txtRevise);
            this.xtraTabPage2.Controls.Add(this.label8);
            this.xtraTabPage2.Controls.Add(this.txtQnSect);
            this.xtraTabPage2.Controls.Add(this.txtQcSect);
            this.xtraTabPage2.Controls.Add(this.txtYear);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Controls.Add(this.label7);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(784, 447);
            this.xtraTabPage2.Text = "รายละเอียด";
            // 
            // txtTotAmt
            // 
            this.txtTotAmt.AllowDrop = true;
            this.txtTotAmt.EditValue = "";
            this.txtTotAmt.Enabled = false;
            this.txtTotAmt.EnterMoveNextControl = true;
            this.txtTotAmt.Location = new System.Drawing.Point(125, 231);
            this.txtTotAmt.Name = "txtTotAmt";
            this.txtTotAmt.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtTotAmt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTotAmt.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotAmt.Properties.Appearance.Options.UseFont = true;
            this.txtTotAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtTotAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtTotAmt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTotAmt.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTotAmt.Size = new System.Drawing.Size(163, 22);
            this.txtTotAmt.TabIndex = 0;
            // 
            // txtQnEmpl
            // 
            this.txtQnEmpl.Enabled = false;
            this.txtQnEmpl.EnterMoveNextControl = true;
            this.txtQnEmpl.Location = new System.Drawing.Point(276, 87);
            this.txtQnEmpl.Name = "txtQnEmpl";
            this.txtQnEmpl.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnEmpl.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnEmpl.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnEmpl.Properties.Appearance.Options.UseFont = true;
            this.txtQnEmpl.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnEmpl.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtQnEmpl.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnEmpl.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtQnEmpl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnEmpl.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnEmpl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnEmpl.Size = new System.Drawing.Size(239, 22);
            this.txtQnEmpl.TabIndex = 3;
            this.txtQnEmpl.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnEmpl.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmpl_Validating);
            this.txtQnEmpl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQcEmpl
            // 
            this.txtQcEmpl.EnterMoveNextControl = true;
            this.txtQcEmpl.Location = new System.Drawing.Point(123, 87);
            this.txtQcEmpl.Name = "txtQcEmpl";
            this.txtQcEmpl.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcEmpl.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcEmpl.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcEmpl.Properties.Appearance.Options.UseFont = true;
            this.txtQcEmpl.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.Control;
            this.txtQcEmpl.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQcEmpl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcEmpl.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcEmpl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcEmpl.Size = new System.Drawing.Size(147, 22);
            this.txtQcEmpl.TabIndex = 2;
            this.txtQcEmpl.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcEmpl.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcEmpl_Validating);
            this.txtQcEmpl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQnJob
            // 
            this.txtQnJob.Enabled = false;
            this.txtQnJob.EnterMoveNextControl = true;
            this.txtQnJob.Location = new System.Drawing.Point(276, 63);
            this.txtQnJob.Name = "txtQnJob";
            this.txtQnJob.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnJob.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnJob.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnJob.Properties.Appearance.Options.UseFont = true;
            this.txtQnJob.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnJob.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtQnJob.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnJob.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtQnJob.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnJob.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnJob.Size = new System.Drawing.Size(239, 22);
            this.txtQnJob.TabIndex = 1;
            this.txtQnJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnJob.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcJob_Validating);
            this.txtQnJob.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQcJob
            // 
            this.txtQcJob.EnterMoveNextControl = true;
            this.txtQcJob.Location = new System.Drawing.Point(123, 63);
            this.txtQcJob.Name = "txtQcJob";
            this.txtQcJob.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcJob.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcJob.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcJob.Properties.Appearance.Options.UseFont = true;
            this.txtQcJob.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.Control;
            this.txtQcJob.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQcJob.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcJob.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcJob.Size = new System.Drawing.Size(147, 22);
            this.txtQcJob.TabIndex = 0;
            this.txtQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcJob.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcJob_Validating);
            this.txtQcJob.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // grdTemPd
            // 
            this.grdTemPd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTemPd.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemPd.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemPd.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemPd.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemPd.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemPd.Location = new System.Drawing.Point(10, 259);
            this.grdTemPd.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPd.MainView = this.gridView2;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcRemark,
            this.grcQcMachine,
            this.grcQnMachine,
            this.grcType,
            this.grcQcDept,
            this.grcAllocPcn,
            this.grcQty,
            this.grcQnUM,
            this.grcAmt});
            this.grdTemPd.Size = new System.Drawing.Size(753, 158);
            this.grdTemPd.TabIndex = 5;
            this.grdTemPd.UseEmbeddedNavigator = true;
            this.grdTemPd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView2.GridControl = this.grdTemPd;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsMenu.EnableColumnMenu = false;
            this.gridView2.OptionsMenu.EnableFooterMenu = false;
            this.gridView2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView2.OptionsNavigation.AutoFocusNewRow = true;
            this.gridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView2.OptionsNavigation.UseTabKey = false;
            this.gridView2.OptionsSelection.InvertSelection = true;
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            // 
            // grcRemark
            // 
            this.grcRemark.AutoHeight = false;
            this.grcRemark.Name = "grcRemark";
            // 
            // grcQcMachine
            // 
            this.grcQcMachine.AutoHeight = false;
            this.grcQcMachine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.SearchInCol, null)});
            this.grcQcMachine.Name = "grcQcMachine";
            this.grcQcMachine.ValidateOnEnterKey = true;
            // 
            // grcQnMachine
            // 
            this.grcQnMachine.AutoHeight = false;
            this.grcQnMachine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.SearchInCol, null)});
            this.grcQnMachine.Name = "grcQnMachine";
            this.grcQnMachine.ValidateOnEnterKey = true;
            // 
            // grcType
            // 
            this.grcType.AutoHeight = false;
            this.grcType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grcType.Items.AddRange(new object[] {
            " ",
            "A"});
            this.grcType.Name = "grcType";
            this.grcType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // grcQcDept
            // 
            this.grcQcDept.AutoHeight = false;
            this.grcQcDept.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.SearchInCol, null)});
            this.grcQcDept.Name = "grcQcDept";
            // 
            // grcAllocPcn
            // 
            this.grcAllocPcn.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.grcAllocPcn.AutoHeight = false;
            this.grcAllocPcn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcAllocPcn.DisplayFormat.FormatString = "#,###.00";
            this.grcAllocPcn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAllocPcn.EditFormat.FormatString = "#,###.00";
            this.grcAllocPcn.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAllocPcn.Mask.EditMask = "#,###.00";
            this.grcAllocPcn.Name = "grcAllocPcn";
            // 
            // grcQty
            // 
            this.grcQty.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.grcQty.AutoHeight = false;
            this.grcQty.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcQty.DisplayFormat.FormatString = "#,###.00";
            this.grcQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcQty.EditFormat.FormatString = "#,###.00";
            this.grcQty.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcQty.Mask.EditMask = "#,###.00";
            this.grcQty.Name = "grcQty";
            // 
            // grcQnUM
            // 
            this.grcQnUM.AutoHeight = false;
            this.grcQnUM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.SearchInCol, null)});
            this.grcQnUM.Name = "grcQnUM";
            // 
            // grcAmt
            // 
            this.grcAmt.AutoHeight = false;
            this.grcAmt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.grcAmt.DisplayFormat.FormatString = "##,###,###.00";
            this.grcAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.EditFormat.FormatString = "##,###,###.00";
            this.grcAmt.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.Mask.EditMask = "##,###,###.00";
            this.grcAmt.Name = "grcAmt";
            this.grcAmt.Leave += new System.EventHandler(this.grcAmt_Leave);
            // 
            // txtRevise
            // 
            this.txtRevise.Enabled = false;
            this.txtRevise.EnterMoveNextControl = true;
            this.txtRevise.Location = new System.Drawing.Point(323, 15);
            this.txtRevise.Name = "txtRevise";
            this.txtRevise.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtRevise.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtRevise.Properties.Appearance.Options.UseBackColor = true;
            this.txtRevise.Properties.Appearance.Options.UseFont = true;
            this.txtRevise.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtRevise.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtRevise.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtRevise.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtRevise.Size = new System.Drawing.Size(73, 22);
            this.txtRevise.TabIndex = 4;
            // 
            // txtQnSect
            // 
            this.txtQnSect.Enabled = false;
            this.txtQnSect.EnterMoveNextControl = true;
            this.txtQnSect.Location = new System.Drawing.Point(276, 39);
            this.txtQnSect.Name = "txtQnSect";
            this.txtQnSect.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnSect.Properties.Appearance.Options.UseFont = true;
            this.txtQnSect.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtQnSect.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtQnSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnSect.Size = new System.Drawing.Size(239, 22);
            this.txtQnSect.TabIndex = 0;
            // 
            // txtQcSect
            // 
            this.txtQcSect.Enabled = false;
            this.txtQcSect.EnterMoveNextControl = true;
            this.txtQcSect.Location = new System.Drawing.Point(123, 39);
            this.txtQcSect.Name = "txtQcSect";
            this.txtQcSect.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQcSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcSect.Properties.Appearance.Options.UseFont = true;
            this.txtQcSect.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtQcSect.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtQcSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcSect.Size = new System.Drawing.Size(147, 22);
            this.txtQcSect.TabIndex = 0;
            // 
            // txtYear
            // 
            this.txtYear.Enabled = false;
            this.txtYear.EnterMoveNextControl = true;
            this.txtYear.Location = new System.Drawing.Point(123, 15);
            this.txtYear.Name = "txtYear";
            this.txtYear.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtYear.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtYear.Properties.Appearance.Options.UseBackColor = true;
            this.txtYear.Properties.Appearance.Options.UseFont = true;
            this.txtYear.Properties.AppearanceDisabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtYear.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtYear.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtYear.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtYear.Size = new System.Drawing.Size(107, 22);
            this.txtYear.TabIndex = 0;
            // 
            // frmBudTran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 496);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmBudTran";
            this.Text = "บันทึกงบประมาณก่อนการอนุมัติ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBudTran_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grbAPVStat)).EndInit();
            this.grbAPVStat.ResumeLayout(false);
            this.pnlOut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAPVRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoAPVStat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbApprove)).EndInit();
            this.grbApprove.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtIsApprove.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDApprove.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDApprove.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApproveBy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastUpd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnLastUpd.Properties)).EndInit();
            this.pnlGridControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTotAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnEmpl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcEmpl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAllocPcn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnUM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRevise.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController1;
        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTab.XtraTabControl pgfMainEdit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.MemoEdit txtFooter;
        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.DateEdit txtDApprove;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit grcRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcMachine;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnMachine;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcType;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcDept;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAllocPcn;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnUM;
        private DevExpress.XtraEditors.ButtonEdit txtRevise;
        private System.Windows.Forms.Label lblDApprove;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ButtonEdit txtYear;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ButtonEdit txtQnEmpl;
        private DevExpress.XtraEditors.ButtonEdit txtQcEmpl;
        private DevExpress.XtraEditors.ButtonEdit txtQnJob;
        private DevExpress.XtraEditors.ButtonEdit txtQcJob;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtIsApprove;
        private DevExpress.XtraEditors.ButtonEdit txtApproveBy;
        private System.Windows.Forms.Label lblApprove;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.GroupControl grbApprove;
        private DevExpress.XtraEditors.ButtonEdit txtTotAmt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ButtonEdit txtStat;
        private DevExpress.XtraEditors.ButtonEdit txtQnLastUpd;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit txtLastUpd;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ButtonEdit txtQnSect;
        private DevExpress.XtraEditors.ButtonEdit txtQcSect;
        private DevExpress.XtraEditors.GroupControl grbAPVStat;
        private DevExpress.XtraEditors.MemoEdit txtAPVRemark;
        private DevExpress.XtraEditors.RadioGroup rdoAPVStat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAmt;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsOut;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem tbrTXCopy;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcApprove;
        private DevExpress.XtraBars.BarEditItem tbrType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox tbcFilter;
        private DevExpress.XtraEditors.CheckEdit chkIsOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcAPVStat;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsPost;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private System.Windows.Forms.Panel pnlGridControl;
        private DevExpress.XtraEditors.SimpleButton btnInsertRow;
        private DevExpress.XtraEditors.SimpleButton btnMoveRowDown;
        private DevExpress.XtraEditors.SimpleButton btnMoveRowUp;
        private DevExpress.XtraEditors.SimpleButton btnRemoveRow;
    }
}