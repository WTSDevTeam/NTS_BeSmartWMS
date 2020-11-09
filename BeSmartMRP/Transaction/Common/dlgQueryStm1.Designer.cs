namespace BeSmartMRP.Transaction.Common
{
    partial class dlgQueryStm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgQueryStm1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnOK2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrev = new DevExpress.XtraBars.BarButtonItem();
            this.btnNext = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcIsOut = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grcApprove = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAPVStat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcIsPost = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.chkIsTagOrd = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.pnlTitle2 = new System.Windows.Forms.Panel();
            this.lblTitle2 = new System.Windows.Forms.Label();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkTag = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            this.pnlTitle2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTag)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.btnNext,
            this.btnPrev,
            this.btnOK2});
            this.barManager1.MaxItemId = 5;
            this.barManager1.StatusBar = this.bar3;
            this.barManager1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barManager1_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOK2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnPrev, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnNext, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnOK2
            // 
            this.btnOK2.Caption = "F10-OK";
            this.btnOK2.Glyph = global::BeSmartMRP.Properties.Resources.text_ok;
            this.btnOK2.Id = 4;
            this.btnOK2.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10);
            this.btnOK2.Name = "btnOK2";
            this.btnOK2.Tag = "ENTER";
            // 
            // btnPrev
            // 
            this.btnPrev.Caption = "Previous";
            this.btnPrev.Glyph = global::BeSmartMRP.Properties.Resources.Nv2;
            this.btnPrev.Id = 3;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Tag = "PREV";
            // 
            // btnNext
            // 
            this.btnNext.Caption = "Next";
            this.btnNext.Glyph = global::BeSmartMRP.Properties.Resources.Nv3;
            this.btnNext.Id = 2;
            this.btnNext.Name = "btnNext";
            this.btnNext.Tag = "NEXT";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Esc-Cancel";
            this.barButtonItem2.Glyph = global::BeSmartMRP.Properties.Resources.Exit01;
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "EXIT";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(705, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 449);
            this.barDockControlBottom.Size = new System.Drawing.Size(705, 21);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 420);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(705, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 420);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "F10-OK";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.text_ok;
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "OK";
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 29);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.pgfMainEdit.Size = new System.Drawing.Size(705, 420);
            this.pgfMainEdit.TabIndex = 131;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(699, 392);
            this.xtraTabPage1.Text = "Step 1";
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
            this.grdBrowView.Location = new System.Drawing.Point(11, 59);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcIsOut,
            this.grcApprove,
            this.grcAPVStat,
            this.grcIsPost,
            this.chkIsTagOrd});
            this.grdBrowView.Size = new System.Drawing.Size(683, 329);
            this.grdBrowView.TabIndex = 1;
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
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // grcIsOut
            // 
            this.grcIsOut.AutoHeight = false;
            this.grcIsOut.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.grcIsOut.Name = "grcIsOut";
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
            // 
            // grcIsPost
            // 
            this.grcIsPost.AutoHeight = false;
            this.grcIsPost.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.grcIsPost.Name = "grcIsPost";
            // 
            // chkIsTagOrd
            // 
            this.chkIsTagOrd.AutoHeight = false;
            this.chkIsTagOrd.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsTagOrd.Name = "chkIsTagOrd";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.pnlTitle2);
            this.xtraTabPage2.Controls.Add(this.grdTemPd);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(699, 392);
            this.xtraTabPage2.Text = "Step 2";
            // 
            // pnlTitle2
            // 
            this.pnlTitle2.BackColor = System.Drawing.Color.White;
            this.pnlTitle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTitle2.Controls.Add(this.lblTitle2);
            this.pnlTitle2.Location = new System.Drawing.Point(-2, -1);
            this.pnlTitle2.Name = "pnlTitle2";
            this.pnlTitle2.Size = new System.Drawing.Size(708, 54);
            this.pnlTitle2.TabIndex = 128;
            // 
            // lblTitle2
            // 
            this.lblTitle2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle2.ForeColor = System.Drawing.Color.Navy;
            this.lblTitle2.Location = new System.Drawing.Point(14, 13);
            this.lblTitle2.Name = "lblTitle2";
            this.lblTitle2.Size = new System.Drawing.Size(597, 23);
            this.lblTitle2.TabIndex = 119;
            this.lblTitle2.Text = "รายการสินค้าตาม P/O";
            this.lblTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.grdTemPd.Location = new System.Drawing.Point(10, 59);
            this.grdTemPd.MainView = this.gridView2;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkTag});
            this.grdTemPd.Size = new System.Drawing.Size(683, 321);
            this.grdTemPd.TabIndex = 0;
            this.grdTemPd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.FocusedCell.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.gridView2.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.Yellow;
            this.gridView2.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.FocusedCell.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gridView2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView2.Appearance.FocusedCell.Options.UseFont = true;
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView2.GridControl = this.grdTemPd;
            this.gridView2.Images = this.imageList1;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // chkTag
            // 
            this.chkTag.AutoHeight = false;
            this.chkTag.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkTag.Name = "chkTag";
            // 
            // dlgQueryStm1
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 470);
            this.ControlBox = false;
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "dlgQueryStm1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.pnlTitle2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnOK2;
        private DevExpress.XtraBars.BarButtonItem btnPrev;
        private DevExpress.XtraBars.BarButtonItem btnNext;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraTab.XtraTabControl pgfMainEdit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcApprove;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcAPVStat;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsPost;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkIsTagOrd;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.Panel pnlTitle2;
        private System.Windows.Forms.Label lblTitle2;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkTag;
    }
}