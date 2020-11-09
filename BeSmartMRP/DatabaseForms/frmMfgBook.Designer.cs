namespace BeSmartMRP.DatabaseForms
{
    partial class frmMfgBook
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
            pmClearInstanse();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMfgBook));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
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
            this.lblName2 = new System.Windows.Forms.Label();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.lblName = new System.Windows.Forms.Label();
            this.txtQnRefType = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcRefType = new DevExpress.XtraEditors.ButtonEdit();
            this.lblRefType = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCode = new System.Windows.Forms.Label();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.pnlWHouse_FG = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWHouse_FG = new System.Windows.Forms.Label();
            this.txtTagWHouse_RM = new DevExpress.XtraEditors.ButtonEdit();
            this.txtTagWHouse_FG = new DevExpress.XtraEditors.ButtonEdit();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.grbMO = new DevExpress.XtraEditors.GroupControl();
            this.chkIsGenDupPR = new DevExpress.XtraEditors.CheckEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTagBook_PO = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTagBook_PR = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnRefType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcRefType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            this.pnlWHouse_FG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagWHouse_RM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagWHouse_FG.Properties)).BeginInit();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbMO)).BeginInit();
            this.grbMO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsGenDupPR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagBook_PO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagBook_PR.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMain});
            this.barMainEdit.DockControls.Add(this.barDockControlTop);
            this.barMainEdit.DockControls.Add(this.barDockControlBottom);
            this.barMainEdit.DockControls.Add(this.barDockControlLeft);
            this.barMainEdit.DockControls.Add(this.barDockControlRight);
            this.barMainEdit.Form = this;
            this.barMainEdit.MaxItemId = 0;
            // 
            // barMain
            // 
            this.barMain.BarName = "Tools";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.DisableCustomization = true;
            this.barMain.OptionsBar.UseWholeRow = true;
            this.barMain.Text = "Tools";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(645, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 453);
            this.barDockControlBottom.Size = new System.Drawing.Size(645, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 424);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(645, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 424);
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.White;
            this.pgfMainEdit.AppearancePage.HeaderActive.BackColor2 = System.Drawing.Color.SkyBlue;
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseBackColor = true;
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.AppearancePage.PageClient.BackColor = System.Drawing.Color.SkyBlue;
            this.pgfMainEdit.AppearancePage.PageClient.Options.UseBackColor = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 29);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.PaintStyleName = "PropertyView";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(645, 424);
            this.pgfMainEdit.TabIndex = 9;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage4});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.data_table;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(645, 401);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(8, 322);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(630, 71);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.Location = new System.Drawing.Point(7, 6);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.Size = new System.Drawing.Size(631, 311);
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
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grdBrowView;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.lblName2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.lblName);
            this.xtraTabPage2.Controls.Add(this.txtQnRefType);
            this.xtraTabPage2.Controls.Add(this.txtQcRefType);
            this.xtraTabPage2.Controls.Add(this.lblRefType);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(645, 407);
            this.xtraTabPage2.Text = "Detail Page";
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(188, 103);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Size = new System.Drawing.Size(339, 22);
            this.txtName2.TabIndex = 2;
            // 
            // lblName2
            // 
            this.lblName2.BackColor = System.Drawing.Color.Transparent;
            this.lblName2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName2.Location = new System.Drawing.Point(52, 103);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(135, 23);
            this.lblName2.TabIndex = 0;
            this.lblName2.Text = "ชื่อ (ภาษา 2) :";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(188, 77);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.Appearance.Options.UseFont = true;
            this.txtName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName.Size = new System.Drawing.Size(339, 22);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName.Location = new System.Drawing.Point(46, 77);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(141, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "ชื่อ (ภาษาไทย) :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnRefType
            // 
            this.txtQnRefType.Enabled = false;
            this.txtQnRefType.EnterMoveNextControl = true;
            this.txtQnRefType.Location = new System.Drawing.Point(240, 28);
            this.txtQnRefType.Name = "txtQnRefType";
            this.txtQnRefType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnRefType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnRefType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnRefType.Properties.Appearance.Options.UseFont = true;
            this.txtQnRefType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnRefType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnRefType.Size = new System.Drawing.Size(287, 22);
            this.txtQnRefType.TabIndex = 4;
            // 
            // txtQcRefType
            // 
            this.txtQcRefType.Enabled = false;
            this.txtQcRefType.EnterMoveNextControl = true;
            this.txtQcRefType.Location = new System.Drawing.Point(188, 28);
            this.txtQcRefType.Name = "txtQcRefType";
            this.txtQcRefType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcRefType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcRefType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcRefType.Properties.Appearance.Options.UseFont = true;
            this.txtQcRefType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcRefType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcRefType.Size = new System.Drawing.Size(46, 22);
            this.txtQcRefType.TabIndex = 3;
            // 
            // lblRefType
            // 
            this.lblRefType.BackColor = System.Drawing.Color.Transparent;
            this.lblRefType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblRefType.Location = new System.Drawing.Point(12, 28);
            this.lblRefType.Name = "lblRefType";
            this.lblRefType.Size = new System.Drawing.Size(175, 23);
            this.lblRefType.TabIndex = 0;
            this.lblRefType.Text = "ประเภทเอกสาร :";
            this.lblRefType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(188, 53);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.txtCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCode.Size = new System.Drawing.Size(82, 22);
            this.txtCode.TabIndex = 0;
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(12, 53);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(175, 23);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "รหัสเล่มเอกสาร :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.pnlWHouse_FG);
            this.xtraTabPage3.Image = global::BeSmartMRP.Properties.Resources.box_closed1;
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(645, 407);
            this.xtraTabPage3.Text = "Config Warehouse";
            // 
            // pnlWHouse_FG
            // 
            this.pnlWHouse_FG.BackColor = System.Drawing.Color.Transparent;
            this.pnlWHouse_FG.Controls.Add(this.label1);
            this.pnlWHouse_FG.Controls.Add(this.lblWHouse_FG);
            this.pnlWHouse_FG.Controls.Add(this.txtTagWHouse_RM);
            this.pnlWHouse_FG.Controls.Add(this.txtTagWHouse_FG);
            this.pnlWHouse_FG.Location = new System.Drawing.Point(21, 35);
            this.pnlWHouse_FG.Name = "pnlWHouse_FG";
            this.pnlWHouse_FG.Size = new System.Drawing.Size(542, 115);
            this.pnlWHouse_FG.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(144, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 23);
            this.label1.TabIndex = 38;
            this.label1.Text = "Rawmat Available Stock :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWHouse_FG
            // 
            this.lblWHouse_FG.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblWHouse_FG.Location = new System.Drawing.Point(144, 0);
            this.lblWHouse_FG.Name = "lblWHouse_FG";
            this.lblWHouse_FG.Size = new System.Drawing.Size(342, 23);
            this.lblWHouse_FG.TabIndex = 38;
            this.lblWHouse_FG.Text = "Finish Goods Available Stock :";
            this.lblWHouse_FG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTagWHouse_RM
            // 
            this.txtTagWHouse_RM.EnterMoveNextControl = true;
            this.txtTagWHouse_RM.Location = new System.Drawing.Point(147, 83);
            this.txtTagWHouse_RM.Name = "txtTagWHouse_RM";
            this.txtTagWHouse_RM.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagWHouse_RM.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagWHouse_RM.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagWHouse_RM.Properties.Appearance.Options.UseFont = true;
            this.txtTagWHouse_RM.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagWHouse_RM.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagWHouse_RM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtTagWHouse_RM.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.txtTagWHouse_RM.Size = new System.Drawing.Size(339, 22);
            this.txtTagWHouse_RM.TabIndex = 37;
            this.txtTagWHouse_RM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagWHouse_RM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtTagWHouse_FG
            // 
            this.txtTagWHouse_FG.EnterMoveNextControl = true;
            this.txtTagWHouse_FG.Location = new System.Drawing.Point(147, 26);
            this.txtTagWHouse_FG.Name = "txtTagWHouse_FG";
            this.txtTagWHouse_FG.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagWHouse_FG.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagWHouse_FG.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagWHouse_FG.Properties.Appearance.Options.UseFont = true;
            this.txtTagWHouse_FG.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagWHouse_FG.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagWHouse_FG.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtTagWHouse_FG.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.txtTagWHouse_FG.Size = new System.Drawing.Size(339, 22);
            this.txtTagWHouse_FG.TabIndex = 37;
            this.txtTagWHouse_FG.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagWHouse_FG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.grbMO);
            this.xtraTabPage4.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(645, 401);
            this.xtraTabPage4.Text = "Gen Document";
            // 
            // grbMO
            // 
            this.grbMO.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.grbMO.Appearance.Options.UseBackColor = true;
            this.grbMO.Controls.Add(this.chkIsGenDupPR);
            this.grbMO.Controls.Add(this.label2);
            this.grbMO.Controls.Add(this.txtTagBook_PO);
            this.grbMO.Controls.Add(this.label3);
            this.grbMO.Controls.Add(this.txtTagBook_PR);
            this.grbMO.Location = new System.Drawing.Point(36, 17);
            this.grbMO.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.grbMO.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbMO.Name = "grbMO";
            this.grbMO.Size = new System.Drawing.Size(531, 221);
            this.grbMO.TabIndex = 0;
            this.grbMO.Text = "Generate PR Config";
            // 
            // chkIsGenDupPR
            // 
            this.chkIsGenDupPR.Location = new System.Drawing.Point(40, 161);
            this.chkIsGenDupPR.Name = "chkIsGenDupPR";
            this.chkIsGenDupPR.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.chkIsGenDupPR.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.chkIsGenDupPR.Properties.Appearance.Options.UseBackColor = true;
            this.chkIsGenDupPR.Properties.Appearance.Options.UseFont = true;
            this.chkIsGenDupPR.Properties.Caption = "Gen Duplicate PR ?";
            this.chkIsGenDupPR.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsGenDupPR.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.chkIsGenDupPR.Size = new System.Drawing.Size(169, 22);
            this.chkIsGenDupPR.TabIndex = 40;
            this.chkIsGenDupPR.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(39, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(342, 23);
            this.label2.TabIndex = 38;
            this.label2.Text = "Check Outstanding PO :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTagBook_PO
            // 
            this.txtTagBook_PO.EnterMoveNextControl = true;
            this.txtTagBook_PO.Location = new System.Drawing.Point(42, 122);
            this.txtTagBook_PO.Name = "txtTagBook_PO";
            this.txtTagBook_PO.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagBook_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagBook_PO.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagBook_PO.Properties.Appearance.Options.UseFont = true;
            this.txtTagBook_PO.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagBook_PO.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagBook_PO.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtTagBook_PO.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.txtTagBook_PO.Size = new System.Drawing.Size(339, 22);
            this.txtTagBook_PO.TabIndex = 1;
            this.txtTagBook_PO.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagBook_PO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(39, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(342, 23);
            this.label3.TabIndex = 38;
            this.label3.Text = "Check Outstanding PR :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTagBook_PR
            // 
            this.txtTagBook_PR.EnterMoveNextControl = true;
            this.txtTagBook_PR.Location = new System.Drawing.Point(42, 58);
            this.txtTagBook_PR.Name = "txtTagBook_PR";
            this.txtTagBook_PR.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTagBook_PR.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTagBook_PR.Properties.Appearance.Options.UseBackColor = true;
            this.txtTagBook_PR.Properties.Appearance.Options.UseFont = true;
            this.txtTagBook_PR.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTagBook_PR.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTagBook_PR.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtTagBook_PR.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.txtTagBook_PR.Size = new System.Drawing.Size(339, 22);
            this.txtTagBook_PR.TabIndex = 0;
            this.txtTagBook_PR.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtTagBook_PR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // frmMfgBook
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 453);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmMfgBook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMfgBook";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnRefType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcRefType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            this.pnlWHouse_FG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTagWHouse_RM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagWHouse_FG.Properties)).EndInit();
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbMO)).EndInit();
            this.grbMO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIsGenDupPR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagBook_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTagBook_PR.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
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
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label lblName2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label lblName;
        private DevExpress.XtraEditors.ButtonEdit txtQnRefType;
        private DevExpress.XtraEditors.ButtonEdit txtQcRefType;
        private System.Windows.Forms.Label lblRefType;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private System.Windows.Forms.Panel pnlWHouse_FG;
        private System.Windows.Forms.Label lblWHouse_FG;
        private DevExpress.XtraEditors.ButtonEdit txtTagWHouse_FG;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtTagWHouse_RM;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private DevExpress.XtraEditors.GroupControl grbMO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ButtonEdit txtTagBook_PO;
        private DevExpress.XtraEditors.ButtonEdit txtTagBook_PR;
        private DevExpress.XtraEditors.CheckEdit chkIsGenDupPR;
    }
}