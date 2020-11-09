namespace BeSmartMRP.DatabaseForms
{
    partial class frmPdGrp
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
            frmPdGrp.pmClearInstanse(); 
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
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.barMainEdit = new DevExpress.XtraBars.BarManager();
            this.barMain = new DevExpress.XtraBars.Bar();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtFooter = new DevExpress.XtraEditors.MemoEdit();
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQnAccItem = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcAccItem = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnAccSCost = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcAccSCost = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnAccSCred = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcAccSCred = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnAccSCash = new DevExpress.XtraEditors.ButtonEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtQcAccSCash = new DevExpress.XtraEditors.ButtonEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtQnAccBCred = new DevExpress.XtraEditors.ButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQcAccBCred = new DevExpress.XtraEditors.ButtonEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtQnAccBCash = new DevExpress.XtraEditors.ButtonEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQcAccBCash = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccItem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccItem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCred.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCred.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccBCred.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccBCred.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccBCash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccBCash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTipController1
            // 
            this.toolTipController1.Rounded = true;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            this.defaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = false;
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
            this.barMainEdit.MainMenu = this.barMain;
            this.barMainEdit.MaxItemId = 0;
            this.barMainEdit.ToolTipController = this.toolTipController1;
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            // 
            // barMain
            // 
            this.barMain.BarName = "barMain";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.DrawDragBorder = false;
            this.barMain.OptionsBar.MultiLine = true;
            this.barMain.OptionsBar.UseWholeRow = true;
            this.barMain.Text = "Standard";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.LookAndFeel.SkinName = "The Asphalt World";
            this.barAndDockingController1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.LookAndFeel.UseWindowsXPTheme = false;
            this.barAndDockingController1.PaintStyleName = "Skin";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 24);
            this.pgfMainEdit.LookAndFeel.SkinName = "Liquid Sky";
            this.pgfMainEdit.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.pgfMainEdit.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pgfMainEdit.LookAndFeel.UseWindowsXPTheme = false;
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(627, 419);
            this.pgfMainEdit.TabIndex = 0;
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
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(627, 395);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(3, 321);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.LookAndFeel.UseWindowsXPTheme = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(621, 71);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Name = "";
            this.grdBrowView.Location = new System.Drawing.Point(3, 6);
            this.grdBrowView.LookAndFeel.SkinName = "The Asphalt World";
            this.grdBrowView.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.grdBrowView.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdBrowView.LookAndFeel.UseWindowsXPTheme = false;
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.Size = new System.Drawing.Size(621, 309);
            this.grdBrowView.TabIndex = 0;
            this.grdBrowView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
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
            this.gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grdBrowView;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ShowFilterPanel = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.gridView1.EndSorting += new System.EventHandler(this.gridView1_EndSorting);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.txtQnAccItem);
            this.xtraTabPage2.Controls.Add(this.txtQcAccItem);
            this.xtraTabPage2.Controls.Add(this.txtQnAccSCost);
            this.xtraTabPage2.Controls.Add(this.txtQcAccSCost);
            this.xtraTabPage2.Controls.Add(this.txtQnAccSCred);
            this.xtraTabPage2.Controls.Add(this.txtQcAccSCred);
            this.xtraTabPage2.Controls.Add(this.txtQnAccSCash);
            this.xtraTabPage2.Controls.Add(this.label8);
            this.xtraTabPage2.Controls.Add(this.txtQcAccSCash);
            this.xtraTabPage2.Controls.Add(this.label7);
            this.xtraTabPage2.Controls.Add(this.txtQnAccBCred);
            this.xtraTabPage2.Controls.Add(this.label6);
            this.xtraTabPage2.Controls.Add(this.txtQcAccBCred);
            this.xtraTabPage2.Controls.Add(this.label5);
            this.xtraTabPage2.Controls.Add(this.txtQnAccBCash);
            this.xtraTabPage2.Controls.Add(this.label4);
            this.xtraTabPage2.Controls.Add(this.txtQcAccBCash);
            this.xtraTabPage2.Controls.Add(this.label3);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::mBudget.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(627, 395);
            this.xtraTabPage2.Text = "Detail Page";
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(182, 79);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtName2.Size = new System.Drawing.Size(339, 22);
            this.txtName2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(35, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "ชื่อกลุ่มสินค้าภาษา 2 :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(182, 56);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.Appearance.Options.UseFont = true;
            this.txtName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtName.Size = new System.Drawing.Size(339, 22);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(74, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "ชื่อกลุ่มสินค้า :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnAccItem
            // 
            this.txtQnAccItem.EnterMoveNextControl = true;
            this.txtQnAccItem.Location = new System.Drawing.Point(328, 220);
            this.txtQnAccItem.Name = "txtQnAccItem";
            this.txtQnAccItem.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccItem.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccItem.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccItem.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccItem.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccItem.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccItem.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccItem.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccItem.TabIndex = 14;
            // 
            // txtQcAccItem
            // 
            this.txtQcAccItem.EnterMoveNextControl = true;
            this.txtQcAccItem.Location = new System.Drawing.Point(182, 220);
            this.txtQcAccItem.Name = "txtQcAccItem";
            this.txtQcAccItem.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccItem.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccItem.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccItem.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccItem.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccItem.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccItem.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccItem.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccItem.TabIndex = 13;
            // 
            // txtQnAccSCost
            // 
            this.txtQnAccSCost.EnterMoveNextControl = true;
            this.txtQnAccSCost.Location = new System.Drawing.Point(328, 197);
            this.txtQnAccSCost.Name = "txtQnAccSCost";
            this.txtQnAccSCost.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccSCost.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccSCost.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccSCost.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccSCost.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccSCost.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccSCost.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccSCost.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccSCost.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccSCost.TabIndex = 12;
            // 
            // txtQcAccSCost
            // 
            this.txtQcAccSCost.EnterMoveNextControl = true;
            this.txtQcAccSCost.Location = new System.Drawing.Point(182, 197);
            this.txtQcAccSCost.Name = "txtQcAccSCost";
            this.txtQcAccSCost.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccSCost.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccSCost.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccSCost.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccSCost.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccSCost.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccSCost.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccSCost.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccSCost.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccSCost.TabIndex = 11;
            // 
            // txtQnAccSCred
            // 
            this.txtQnAccSCred.EnterMoveNextControl = true;
            this.txtQnAccSCred.Location = new System.Drawing.Point(328, 174);
            this.txtQnAccSCred.Name = "txtQnAccSCred";
            this.txtQnAccSCred.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccSCred.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccSCred.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccSCred.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccSCred.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccSCred.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccSCred.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccSCred.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccSCred.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccSCred.TabIndex = 10;
            // 
            // txtQcAccSCred
            // 
            this.txtQcAccSCred.EnterMoveNextControl = true;
            this.txtQcAccSCred.Location = new System.Drawing.Point(182, 174);
            this.txtQcAccSCred.Name = "txtQcAccSCred";
            this.txtQcAccSCred.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccSCred.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccSCred.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccSCred.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccSCred.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccSCred.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccSCred.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccSCred.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccSCred.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccSCred.TabIndex = 9;
            // 
            // txtQnAccSCash
            // 
            this.txtQnAccSCash.EnterMoveNextControl = true;
            this.txtQnAccSCash.Location = new System.Drawing.Point(328, 151);
            this.txtQnAccSCash.Name = "txtQnAccSCash";
            this.txtQnAccSCash.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccSCash.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccSCash.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccSCash.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccSCash.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccSCash.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccSCash.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccSCash.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccSCash.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccSCash.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(26, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(155, 23);
            this.label8.TabIndex = 23;
            this.label8.Text = "รหัสบัญชีสินค้าคงเหลือ :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcAccSCash
            // 
            this.txtQcAccSCash.EnterMoveNextControl = true;
            this.txtQcAccSCash.Location = new System.Drawing.Point(182, 151);
            this.txtQcAccSCash.Name = "txtQcAccSCash";
            this.txtQcAccSCash.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccSCash.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccSCash.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccSCash.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccSCash.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccSCash.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccSCash.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccSCash.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccSCash.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccSCash.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(38, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 23);
            this.label7.TabIndex = 22;
            this.label7.Text = "รหัสบัญชีต้นทุน :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnAccBCred
            // 
            this.txtQnAccBCred.EnterMoveNextControl = true;
            this.txtQnAccBCred.Location = new System.Drawing.Point(328, 128);
            this.txtQnAccBCred.Name = "txtQnAccBCred";
            this.txtQnAccBCred.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccBCred.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccBCred.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccBCred.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccBCred.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccBCred.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccBCred.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccBCred.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccBCred.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccBCred.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(56, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 23);
            this.label6.TabIndex = 21;
            this.label6.Text = "รหัสบัญชีขายเชื่อ :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcAccBCred
            // 
            this.txtQcAccBCred.EnterMoveNextControl = true;
            this.txtQcAccBCred.Location = new System.Drawing.Point(182, 128);
            this.txtQcAccBCred.Name = "txtQcAccBCred";
            this.txtQcAccBCred.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccBCred.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccBCred.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccBCred.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccBCred.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccBCred.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccBCred.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccBCred.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccBCred.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccBCred.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(56, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 23);
            this.label5.TabIndex = 20;
            this.label5.Text = "รหัสบัญชีขายสด :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnAccBCash
            // 
            this.txtQnAccBCash.EnterMoveNextControl = true;
            this.txtQnAccBCash.Location = new System.Drawing.Point(328, 105);
            this.txtQnAccBCash.Name = "txtQnAccBCash";
            this.txtQnAccBCash.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnAccBCash.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnAccBCash.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnAccBCash.Properties.Appearance.Options.UseFont = true;
            this.txtQnAccBCash.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnAccBCash.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnAccBCash.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQnAccBCash.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQnAccBCash.Size = new System.Drawing.Size(264, 22);
            this.txtQnAccBCash.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(53, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "รหัสบัญชีซื้อเชื่อ :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcAccBCash
            // 
            this.txtQcAccBCash.EnterMoveNextControl = true;
            this.txtQcAccBCash.Location = new System.Drawing.Point(182, 105);
            this.txtQcAccBCash.Name = "txtQcAccBCash";
            this.txtQcAccBCash.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAccBCash.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAccBCash.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAccBCash.Properties.Appearance.Options.UseFont = true;
            this.txtQcAccBCash.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcAccBCash.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcAccBCash.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtQcAccBCash.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::mBudget.Properties.Resources.view)});
            this.txtQcAccBCash.Size = new System.Drawing.Size(140, 22);
            this.txtQcAccBCash.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(74, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "รหัสบัญชีซื้อสด :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(182, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.txtCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtCode.Size = new System.Drawing.Size(137, 22);
            this.txtCode.TabIndex = 0;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(74, 33);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(107, 23);
            this.lblCode.TabIndex = 15;
            this.lblCode.Text = "รหัสกลุ่มสินค้า :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmPdGrp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 443);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmPdGrp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ฐานข้อมูลกลุ่มสินค้า";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPdGrp_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccItem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccItem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCred.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCred.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccSCash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccSCash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccBCred.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccBCred.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnAccBCash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAccBCash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraTab.XtraTabControl pgfMainEdit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.MemoEdit txtFooter;
        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccBCash;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccBCash;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccItem;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccItem;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccSCost;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccSCost;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccSCred;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccSCred;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccSCash;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccSCash;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ButtonEdit txtQnAccBCred;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ButtonEdit txtQcAccBCred;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}