namespace BeSmartMRP.DatabaseForms
{
    partial class frmBudChart
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
            frmBudChart.pmClearInstanse();
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
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtFooter = new DevExpress.XtraEditors.MemoEdit();
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtQnBudChart = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcBudChart = new DevExpress.XtraEditors.ButtonEdit();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.grcQcAcChart = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcQnAcChart = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.txtQnBudType = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcBudType = new DevExpress.XtraEditors.ButtonEdit();
            this.cmbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtRemark = new DevExpress.XtraEditors.ButtonEdit();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.txtCode = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBudChart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBudChart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcAcChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnAcChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBudType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBudType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMain});
            this.barMainEdit.Controller = this.barAndDockingController1;
            this.barMainEdit.DockControls.Add(this.barDockControlTop);
            this.barMainEdit.DockControls.Add(this.barDockControlBottom);
            this.barMainEdit.DockControls.Add(this.barDockControlLeft);
            this.barMainEdit.DockControls.Add(this.barDockControlRight);
            this.barMainEdit.Form = this;
            this.barMainEdit.MaxItemId = 0;
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            // 
            // barMain
            // 
            this.barMain.BarName = "Custom 1";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.DisableCustomization = true;
            this.barMain.OptionsBar.DrawDragBorder = false;
            this.barMain.OptionsBar.UseWholeRow = true;
            this.barMain.Text = "Custom 1";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.toolTipController1.SetSuperTip(this.barDockControlTop, null);
            // 
            // barDockControlBottom
            // 
            this.toolTipController1.SetSuperTip(this.barDockControlBottom, null);
            // 
            // barDockControlLeft
            // 
            this.toolTipController1.SetSuperTip(this.barDockControlLeft, null);
            // 
            // barDockControlRight
            // 
            this.toolTipController1.SetSuperTip(this.barDockControlRight, null);
            // 
            // toolTipController1
            // 
            this.toolTipController1.Rounded = true;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(16, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 23);
            this.toolTipController1.SetSuperTip(this.label7, null);
            this.label7.TabIndex = 30;
            this.label7.Text = "ประเภทงบประมาณ :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(59, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 23);
            this.toolTipController1.SetSuperTip(this.label4, null);
            this.label4.TabIndex = 13;
            this.label4.Text = "ชนิด :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(20, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 23);
            this.toolTipController1.SetSuperTip(this.label3, null);
            this.label3.TabIndex = 6;
            this.label3.Text = "หมายเหตุ :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(19, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 23);
            this.toolTipController1.SetSuperTip(this.label2, null);
            this.label2.TabIndex = 6;
            this.label2.Text = "ชื่อ (ภาษาอังกฤษ) :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(23, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 23);
            this.toolTipController1.SetSuperTip(this.label1, null);
            this.label1.TabIndex = 6;
            this.label1.Text = "ชื่อ (ภาษาไทย) :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(48, 72);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(146, 23);
            this.toolTipController1.SetSuperTip(this.lblCode, null);
            this.lblCode.TabIndex = 5;
            this.lblCode.Text = "รหัสผังงบประมาณ :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(16, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 23);
            this.toolTipController1.SetSuperTip(this.label5, null);
            this.label5.TabIndex = 34;
            this.label5.Text = "รหัสงบประมาณคุม :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 23);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(741, 499);
            this.pgfMainEdit.TabIndex = 7;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(741, 475);
            this.xtraTabPage1.Text = "Browse";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(6, 395);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(728, 71);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Name = "";
            this.grdBrowView.Location = new System.Drawing.Point(6, 6);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.grdBrowView.Size = new System.Drawing.Size(728, 383);
            this.grdBrowView.TabIndex = 0;
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
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.gridView1.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView1_ColumnWidthChanged);
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtQnBudChart);
            this.xtraTabPage2.Controls.Add(this.txtQcBudChart);
            this.xtraTabPage2.Controls.Add(this.label5);
            this.xtraTabPage2.Controls.Add(this.grdTemPd);
            this.xtraTabPage2.Controls.Add(this.txtQnBudType);
            this.xtraTabPage2.Controls.Add(this.txtQcBudType);
            this.xtraTabPage2.Controls.Add(this.label7);
            this.xtraTabPage2.Controls.Add(this.cmbType);
            this.xtraTabPage2.Controls.Add(this.label4);
            this.xtraTabPage2.Controls.Add(this.txtRemark);
            this.xtraTabPage2.Controls.Add(this.label3);
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(741, 475);
            this.xtraTabPage2.Text = "รายละเอียด";
            // 
            // txtQnBudChart
            // 
            this.txtQnBudChart.Enabled = false;
            this.txtQnBudChart.EnterMoveNextControl = true;
            this.txtQnBudChart.Location = new System.Drawing.Point(340, 48);
            this.txtQnBudChart.Name = "txtQnBudChart";
            this.txtQnBudChart.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnBudChart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnBudChart.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnBudChart.Properties.Appearance.Options.UseFont = true;
            this.txtQnBudChart.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnBudChart.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnBudChart.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnBudChart.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnBudChart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnBudChart.Size = new System.Drawing.Size(304, 22);
            this.txtQnBudChart.TabIndex = 3;
            // 
            // txtQcBudChart
            // 
            this.txtQcBudChart.EnterMoveNextControl = true;
            this.txtQcBudChart.Location = new System.Drawing.Point(195, 48);
            this.txtQcBudChart.Name = "txtQcBudChart";
            this.txtQcBudChart.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBudChart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBudChart.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBudChart.Properties.Appearance.Options.UseFont = true;
            this.txtQcBudChart.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBudChart.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBudChart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQcBudChart.Size = new System.Drawing.Size(137, 22);
            this.txtQcBudChart.TabIndex = 2;
            this.txtQcBudChart.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcBudChart.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcBudChart_Validating);
            this.txtQcBudChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // grdTemPd
            // 
            this.grdTemPd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTemPd.EmbeddedNavigator.Name = "";
            this.grdTemPd.Location = new System.Drawing.Point(12, 203);
            this.grdTemPd.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPd.MainView = this.gridView2;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcRemark,
            this.grcQcAcChart,
            this.grcQnAcChart});
            this.grdTemPd.Size = new System.Drawing.Size(717, 260);
            this.grdTemPd.TabIndex = 9;
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
            this.gridView2.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsMenu.EnableColumnMenu = false;
            this.gridView2.OptionsMenu.EnableFooterMenu = false;
            this.gridView2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView2.OptionsNavigation.AutoFocusNewRow = true;
            this.gridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView2.OptionsNavigation.UseTabKey = false;
            this.gridView2.OptionsSelection.InvertSelection = true;
            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            // 
            // grcRemark
            // 
            this.grcRemark.AutoHeight = false;
            this.grcRemark.Name = "grcRemark";
            // 
            // grcQcAcChart
            // 
            this.grcQcAcChart.AutoHeight = false;
            this.grcQcAcChart.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.SearchInCol)});
            this.grcQcAcChart.Name = "grcQcAcChart";
            this.grcQcAcChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcAcChart_KeyDown);
            this.grcQcAcChart.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcAcChart_ButtonClick);
            // 
            // grcQnAcChart
            // 
            this.grcQnAcChart.AutoHeight = false;
            this.grcQnAcChart.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.SearchInCol)});
            this.grcQnAcChart.Name = "grcQnAcChart";
            this.grcQnAcChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcAcChart_KeyDown);
            this.grcQnAcChart.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcAcChart_ButtonClick);
            // 
            // txtQnBudType
            // 
            this.txtQnBudType.Enabled = false;
            this.txtQnBudType.EnterMoveNextControl = true;
            this.txtQnBudType.Location = new System.Drawing.Point(340, 24);
            this.txtQnBudType.Name = "txtQnBudType";
            this.txtQnBudType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnBudType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnBudType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnBudType.Properties.Appearance.Options.UseFont = true;
            this.txtQnBudType.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnBudType.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnBudType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnBudType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnBudType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnBudType.Size = new System.Drawing.Size(304, 22);
            this.txtQnBudType.TabIndex = 1;
            this.txtQnBudType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnBudType.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcBudType_Validating);
            this.txtQnBudType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQcBudType
            // 
            this.txtQcBudType.EnterMoveNextControl = true;
            this.txtQcBudType.Location = new System.Drawing.Point(195, 24);
            this.txtQcBudType.Name = "txtQcBudType";
            this.txtQcBudType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBudType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBudType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBudType.Properties.Appearance.Options.UseFont = true;
            this.txtQcBudType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBudType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBudType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQcBudType.Size = new System.Drawing.Size(137, 22);
            this.txtQcBudType.TabIndex = 0;
            this.txtQcBudType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcBudType.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcBudType_Validating);
            this.txtQcBudType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // cmbType
            // 
            this.cmbType.EnterMoveNextControl = true;
            this.cmbType.Location = new System.Drawing.Point(195, 143);
            this.cmbType.Name = "cmbType";
            this.cmbType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbType.Properties.Appearance.Options.UseBackColor = true;
            this.cmbType.Properties.Appearance.Options.UseFont = true;
            this.cmbType.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbType.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.cmbType.Properties.PopupSizeable = true;
            this.cmbType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbType.Size = new System.Drawing.Size(137, 22);
            this.cmbType.TabIndex = 7;
            // 
            // txtRemark
            // 
            this.txtRemark.EnterMoveNextControl = true;
            this.txtRemark.Location = new System.Drawing.Point(195, 167);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtRemark.Properties.Appearance.Options.UseBackColor = true;
            this.txtRemark.Properties.Appearance.Options.UseFont = true;
            this.txtRemark.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtRemark.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtRemark.Size = new System.Drawing.Size(449, 22);
            this.txtRemark.TabIndex = 8;
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(195, 120);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Size = new System.Drawing.Size(339, 22);
            this.txtName2.TabIndex = 6;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(195, 96);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.Appearance.Options.UseFont = true;
            this.txtName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName.Size = new System.Drawing.Size(339, 22);
            this.txtName.TabIndex = 5;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(195, 72);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.txtCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCode.Size = new System.Drawing.Size(137, 22);
            this.txtCode.TabIndex = 4;
            // 
            // frmBudChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 522);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmBudChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.toolTipController1.SetSuperTip(this, null);
            this.Text = "ข้อมูลผังงบประมาณ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPdCateg_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBudChart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBudChart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcAcChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnAcChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBudType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBudType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.Utils.ToolTipController toolTipController1;
        private DevExpress.XtraTab.XtraTabControl pgfMainEdit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.MemoEdit txtFooter;
        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtQnBudType;
        private DevExpress.XtraEditors.ButtonEdit txtQcBudType;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ButtonEdit txtRemark;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit grcRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcAcChart;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnAcChart;
        private DevExpress.XtraEditors.ButtonEdit txtQnBudChart;
        private DevExpress.XtraEditors.ButtonEdit txtQcBudChart;
        private System.Windows.Forms.Label label5;
    }
}