namespace BeSmartMRP
{
    partial class frmMainMenuPOS1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainMenuPOS1));
            this.navStmove_TR = new DevExpress.XtraNavBar.NavBarItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.lblVersionDate = new System.Windows.Forms.Label();
            this.lblAnnouce = new DevExpress.XtraEditors.LabelControl();
            this.navStmove_GD = new DevExpress.XtraNavBar.NavBarItem();
            this.navStmove_FR = new DevExpress.XtraNavBar.NavBarItem();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navStmove_WR = new DevExpress.XtraNavBar.NavBarItem();
            this.navStock_Bal1 = new DevExpress.XtraNavBar.NavBarItem();
            this.pbxCusLOGO = new DevExpress.XtraEditors.PictureEdit();
            this.navBarGroup4 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroup5 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.navOrder_SO = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navOrder_PO = new DevExpress.XtraNavBar.NavBarItem();
            this.pnlAbout = new DevExpress.XtraEditors.PanelControl();
            this.pnlCustomer = new DevExpress.XtraEditors.PanelControl();
            this.imgBG = new DevExpress.XtraEditors.PictureEdit();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCusLOGO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAbout)).BeginInit();
            this.pnlAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCustomer)).BeginInit();
            this.pnlCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBG.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // navStmove_TR
            // 
            this.navStmove_TR.Caption = "ใบขอเบิก";
            this.navStmove_TR.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.lorry;
            this.navStmove_TR.Name = "navStmove_TR";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // lblVersionDate
            // 
            this.lblVersionDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblVersionDate.Location = new System.Drawing.Point(5, 216);
            this.lblVersionDate.Name = "lblVersionDate";
            this.lblVersionDate.Size = new System.Drawing.Size(228, 23);
            this.lblVersionDate.TabIndex = 1;
            this.lblVersionDate.Text = "Version Date :";
            this.lblVersionDate.Visible = false;
            // 
            // lblAnnouce
            // 
            this.lblAnnouce.AllowHtmlString = true;
            this.lblAnnouce.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnnouce.Appearance.Options.UseFont = true;
            this.lblAnnouce.Appearance.Options.UseTextOptions = true;
            this.lblAnnouce.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblAnnouce.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAnnouce.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lblAnnouce.Location = new System.Drawing.Point(144, 5);
            this.lblAnnouce.Name = "lblAnnouce";
            this.lblAnnouce.Size = new System.Drawing.Size(255, 208);
            this.lblAnnouce.TabIndex = 2;
            // 
            // navStmove_GD
            // 
            this.navStmove_GD.Caption = "Goods Delivery Note";
            this.navStmove_GD.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.clipboard_invoice;
            this.navStmove_GD.Name = "navStmove_GD";
            // 
            // navStmove_FR
            // 
            this.navStmove_FR.Caption = "Goods Recieve";
            this.navStmove_FR.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.document_GR;
            this.navStmove_FR.ImageOptions.SmallImage = global::BeSmartMRP.Properties.Resources.document_index;
            this.navStmove_FR.Name = "navStmove_FR";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.splitterControl1.Appearance.Options.UseBackColor = true;
            this.splitterControl1.Location = new System.Drawing.Point(150, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(4, 505);
            this.splitterControl1.TabIndex = 5;
            this.splitterControl1.TabStop = false;
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "WareHouse";
            this.navBarGroup3.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsList;
            this.navBarGroup3.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.house_one;
            this.navBarGroup3.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navStmove_FR),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navStmove_TR),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navStmove_GD),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navStmove_WR),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navStock_Bal1)});
            this.navBarGroup3.Name = "navBarGroup3";
            this.navBarGroup3.Visible = false;
            // 
            // navStmove_WR
            // 
            this.navStmove_WR.Caption = "Raw Materials Pick Slip";
            this.navStmove_WR.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.document_WR;
            this.navStmove_WR.Name = "navStmove_WR";
            // 
            // navStock_Bal1
            // 
            this.navStock_Bal1.Caption = "Stock Inquiry";
            this.navStock_Bal1.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.box_front;
            this.navStock_Bal1.Name = "navStock_Bal1";
            // 
            // pbxCusLOGO
            // 
            this.pbxCusLOGO.Location = new System.Drawing.Point(5, 5);
            this.pbxCusLOGO.Name = "pbxCusLOGO";
            this.pbxCusLOGO.Properties.ShowMenu = false;
            this.pbxCusLOGO.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pbxCusLOGO.Size = new System.Drawing.Size(133, 111);
            this.pbxCusLOGO.TabIndex = 0;
            // 
            // navBarGroup4
            // 
            this.navBarGroup4.Caption = "Reporting";
            this.navBarGroup4.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroup4.GroupClientHeight = 80;
            this.navBarGroup4.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup4.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.chart_pie_alternative;
            this.navBarGroup4.Name = "navBarGroup4";
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(150, 223);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // navBarGroup5
            // 
            this.navBarGroup5.Caption = "Configuration";
            this.navBarGroup5.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.setting_tools;
            this.navBarGroup5.Name = "navBarGroup5";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "Sales";
            this.navBarGroup1.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.cash_terminal;
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // navOrder_SO
            // 
            this.navOrder_SO.Caption = "Sale Order";
            this.navOrder_SO.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.document_SO;
            this.navOrder_SO.Name = "navOrder_SO";
            this.navOrder_SO.Visible = false;
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "Point of Sale";
            this.navBarGroup2.Expanded = true;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsList;
            this.navBarGroup2.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.cash_register_2;
            this.navBarGroup2.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navOrder_PO),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navOrder_SO)});
            this.navBarGroup2.Name = "navBarGroup2";
            // 
            // navOrder_PO
            // 
            this.navOrder_PO.Caption = "ขายหน้าร้าน";
            this.navOrder_PO.ImageOptions.LargeImage = global::BeSmartMRP.Properties.Resources.cash_register_2;
            this.navOrder_PO.Name = "navOrder_PO";
            // 
            // pnlAbout
            // 
            this.pnlAbout.Appearance.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlAbout.Appearance.BorderColor = System.Drawing.Color.Navy;
            this.pnlAbout.Appearance.Options.UseBackColor = true;
            this.pnlAbout.Appearance.Options.UseBorderColor = true;
            this.pnlAbout.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlAbout.Controls.Add(this.splitterControl1);
            this.pnlAbout.Controls.Add(this.pnlCustomer);
            this.pnlAbout.Controls.Add(this.imgBG);
            this.pnlAbout.Controls.Add(this.navBarControl1);
            this.pnlAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAbout.Location = new System.Drawing.Point(0, 25);
            this.pnlAbout.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.pnlAbout.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlAbout.Name = "pnlAbout";
            this.pnlAbout.Size = new System.Drawing.Size(699, 505);
            this.pnlAbout.TabIndex = 11;
            this.pnlAbout.SizeChanged += new System.EventHandler(this.pnlAbout_SizeChanged);
            // 
            // pnlCustomer
            // 
            this.pnlCustomer.Appearance.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.pnlCustomer.Appearance.Options.UseBackColor = true;
            this.pnlCustomer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.pnlCustomer.Controls.Add(this.lblVersionDate);
            this.pnlCustomer.Controls.Add(this.lblAnnouce);
            this.pnlCustomer.Controls.Add(this.pbxCusLOGO);
            this.pnlCustomer.Location = new System.Drawing.Point(186, 192);
            this.pnlCustomer.Name = "pnlCustomer";
            this.pnlCustomer.Size = new System.Drawing.Size(404, 245);
            this.pnlCustomer.TabIndex = 1;
            // 
            // imgBG
            // 
            this.imgBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgBG.Location = new System.Drawing.Point(150, 0);
            this.imgBG.Name = "imgBG";
            this.imgBG.Properties.AllowFocused = false;
            this.imgBG.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.imgBG.Size = new System.Drawing.Size(549, 505);
            this.imgBG.TabIndex = 0;
            this.imgBG.Visible = false;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup2;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup3,
            this.navBarGroup2,
            this.navBarGroup1,
            this.navBarGroup4,
            this.navBarGroup5});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navStmove_FR,
            this.navStmove_WR,
            this.navStmove_TR,
            this.navOrder_PO,
            this.navStmove_GD,
            this.navOrder_SO,
            this.navStock_Bal1});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 150;
            this.navBarControl1.Size = new System.Drawing.Size(150, 505);
            this.navBarControl1.TabIndex = 4;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinExplorerBarViewInfoRegistrator("DevExpress Style");
            this.navBarControl1.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarControl1_LinkClicked);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Production Capacity";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "DEMO01";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 25);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 527);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(699, 25);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 527);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Production Planning";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Tag = "DEMO02";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "&Application";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "ออกจากระบบ";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageOptions.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "EXIT";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DisableClose = true;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
            this.barManager1.UseF10KeyForMenu = false;
            this.barManager1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barManager1_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(699, 25);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 552);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(699, 0);
            // 
            // dockManager1
            // 
            this.dockManager1.Controller = this.barAndDockingController1;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar"});
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 530);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(699, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(684, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.xtraTabbedMdiManager1.AppearancePage.HeaderActive.Options.UseFont = true;
            this.xtraTabbedMdiManager1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.xtraTabbedMdiManager1.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.xtraTabbedMdiManager1.Controller = this.barAndDockingController1;
            this.xtraTabbedMdiManager1.HeaderButtons = ((DevExpress.XtraTab.TabButtons)(((DevExpress.XtraTab.TabButtons.Prev | DevExpress.XtraTab.TabButtons.Next) 
            | DevExpress.XtraTab.TabButtons.Close)));
            this.xtraTabbedMdiManager1.HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Always;
            this.xtraTabbedMdiManager1.MdiParent = this;
            this.xtraTabbedMdiManager1.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabbedMdiManager1.PageAdded += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageAdded);
            this.xtraTabbedMdiManager1.PageRemoved += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageRemoved);
            // 
            // imgToolbar
            // 
            this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
            this.imgToolbar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgToolbar.Images.SetKeyName(0, "data_table.png");
            this.imgToolbar.Images.SetKeyName(1, "Home.gif");
            this.imgToolbar.Images.SetKeyName(2, "scroll.png");
            this.imgToolbar.Images.SetKeyName(3, "Mapping1.gif");
            this.imgToolbar.Images.SetKeyName(4, "Exit01.gif");
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // frmMainMenuPOS1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 552);
            this.Controls.Add(this.pnlAbout);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("frmMainMenuPOS1.IconOptions.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "frmMainMenuPOS1";
            this.Text = "BeSmart Point of Sale";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainMenuPOS1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMainMenuPOS1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCusLOGO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAbout)).EndInit();
            this.pnlAbout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlCustomer)).EndInit();
            this.pnlCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgBG.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarItem navStmove_TR;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private System.Windows.Forms.Label lblVersionDate;
        private DevExpress.XtraEditors.LabelControl lblAnnouce;
        private DevExpress.XtraNavBar.NavBarItem navStmove_GD;
        private DevExpress.XtraNavBar.NavBarItem navStmove_FR;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup3;
        private DevExpress.XtraNavBar.NavBarItem navStmove_WR;
        private DevExpress.XtraNavBar.NavBarItem navStock_Bal1;
        private DevExpress.XtraEditors.PictureEdit pbxCusLOGO;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup4;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup5;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraNavBar.NavBarItem navOrder_SO;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup2;
        private DevExpress.XtraNavBar.NavBarItem navOrder_PO;
        private DevExpress.XtraEditors.PanelControl pnlAbout;
        private DevExpress.XtraEditors.PanelControl pnlCustomer;
        private DevExpress.XtraEditors.PictureEdit imgBG;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private System.Windows.Forms.ImageList imgToolbar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}