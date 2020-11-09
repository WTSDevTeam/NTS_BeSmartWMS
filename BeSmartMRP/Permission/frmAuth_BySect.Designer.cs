namespace BeSmartMRP.Permission
{
    partial class frmAuth_BySect
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
            frmAuth_BySect.pmClearInstanse();
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
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.grcQcSect = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcQnSect = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQnEmpl = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcAppLogin = new DevExpress.XtraEditors.ButtonEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcSect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnSect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnEmpl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAppLogin.Properties)).BeginInit();
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
            this.barMainEdit.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3});
            this.barMainEdit.MaxItemId = 3;
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            // 
            // barMain
            // 
            this.barMain.BarName = "Tools";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barMain.Text = "Tools";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Print";
            this.barButtonItem3.Glyph = global::BeSmartMRP.Properties.Resources.Print2;
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Tag = "Print";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Save";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.Save;
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10);
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "Save";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Exit";
            this.barButtonItem2.Glyph = global::BeSmartMRP.Properties.Resources.Exit01;
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "Exit";
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 25);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(681, 448);
            this.pgfMainEdit.TabIndex = 5;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.grdTemPd);
            this.xtraTabPage1.Controls.Add(this.label6);
            this.xtraTabPage1.Controls.Add(this.txtQnEmpl);
            this.xtraTabPage1.Controls.Add(this.txtQcAppLogin);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(681, 424);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // grdTemPd
            // 
            this.grdTemPd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grdTemPd.EmbeddedNavigator.Name = "";
            this.grdTemPd.Location = new System.Drawing.Point(45, 52);
            this.grdTemPd.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPd.MainView = this.gridView2;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcRemark,
            this.grcQcSect,
            this.grcQnSect});
            this.grdTemPd.Size = new System.Drawing.Size(548, 283);
            this.grdTemPd.TabIndex = 10;
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
            // grcQcSect
            // 
            this.grcQcSect.AutoHeight = false;
            this.grcQcSect.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.SearchInCol)});
            this.grcQcSect.Name = "grcQcSect";
            this.grcQcSect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcAcChart_KeyDown);
            this.grcQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcAcChart_ButtonClick);
            // 
            // grcQnSect
            // 
            this.grcQnSect.AutoHeight = false;
            this.grcQnSect.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.SearchInCol)});
            this.grcQnSect.Name = "grcQnSect";
            this.grcQnSect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcAcChart_KeyDown);
            this.grcQnSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcAcChart_ButtonClick);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(12, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "ระบุ Login ในระบบ :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnEmpl
            // 
            this.txtQnEmpl.Enabled = false;
            this.txtQnEmpl.EnterMoveNextControl = true;
            this.txtQnEmpl.Location = new System.Drawing.Point(332, 24);
            this.txtQnEmpl.Name = "txtQnEmpl";
            this.txtQnEmpl.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnEmpl.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnEmpl.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnEmpl.Properties.Appearance.Options.UseFont = true;
            this.txtQnEmpl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnEmpl.Properties.ReadOnly = true;
            this.txtQnEmpl.Size = new System.Drawing.Size(261, 22);
            this.txtQnEmpl.TabIndex = 1;
            this.txtQnEmpl.Visible = false;
            // 
            // txtQcAppLogin
            // 
            this.txtQcAppLogin.EnterMoveNextControl = true;
            this.txtQcAppLogin.Location = new System.Drawing.Point(182, 24);
            this.txtQcAppLogin.Name = "txtQcAppLogin";
            this.txtQcAppLogin.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcAppLogin.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcAppLogin.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcAppLogin.Properties.Appearance.Options.UseFont = true;
            this.txtQcAppLogin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQcAppLogin.Size = new System.Drawing.Size(144, 22);
            this.txtQcAppLogin.TabIndex = 0;
            this.txtQcAppLogin.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQcAppLogin.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcAppLogin_Validating);
            this.txtQcAppLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.PageVisible = false;
            this.xtraTabPage2.Size = new System.Drawing.Size(681, 424);
            this.xtraTabPage2.Text = "Detail Page";
            // 
            // frmAuth_BySect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 473);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmAuth_BySect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "กำหนดสิทธิ์การทำงานของ USER ตามแผนก";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAuth_BySect_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcSect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnSect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnEmpl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcAppLogin.Properties)).EndInit();
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
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ButtonEdit txtQnEmpl;
        private DevExpress.XtraEditors.ButtonEdit txtQcAppLogin;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit grcRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcSect;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnSect;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
    }
}