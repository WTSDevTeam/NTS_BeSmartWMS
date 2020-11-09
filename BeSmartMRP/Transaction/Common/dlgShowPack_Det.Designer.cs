namespace BeSmartMRP.Transaction.Common
{
    partial class dlgShowPack_Det
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgShowPack_Det));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelSO = new DevExpress.XtraBars.BarButtonItem();
            this.grdTemPack = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcQty = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQcPack = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcShowPack = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.txtQnWHPack = new DevExpress.XtraEditors.ButtonEdit();
            this.txtSumQty = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcWHPack = new DevExpress.XtraEditors.ButtonEdit();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblCoor = new System.Windows.Forms.Label();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShowPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnWHPack.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcWHPack.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 373);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(670, 22);
            this.statusStrip1.TabIndex = 115;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barMainEdit.DockControls.Add(this.barDockControlTop);
            this.barMainEdit.DockControls.Add(this.barDockControlBottom);
            this.barMainEdit.DockControls.Add(this.barDockControlLeft);
            this.barMainEdit.DockControls.Add(this.barDockControlRight);
            this.barMainEdit.Form = this;
            this.barMainEdit.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.btnDelSO,
            this.barButtonItem4});
            this.barMainEdit.MaxItemId = 5;
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "F10-OK";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.text_ok;
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10);
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "OK";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Esc-Close";
            this.barButtonItem3.Glyph = global::BeSmartMRP.Properties.Resources.Exit01;
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Tag = "EXIT";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(670, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 395);
            this.barDockControlBottom.Size = new System.Drawing.Size(670, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 366);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(670, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 366);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "F8-Load P/O";
            this.barButtonItem2.Glyph = global::BeSmartMRP.Properties.Resources.Search;
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8);
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "LOADSO";
            // 
            // btnDelSO
            // 
            this.btnDelSO.Caption = "F4-Clear P/O";
            this.btnDelSO.Glyph = global::BeSmartMRP.Properties.Resources.DelItem;
            this.btnDelSO.Id = 3;
            this.btnDelSO.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.btnDelSO.Name = "btnDelSO";
            this.btnDelSO.Tag = "CLEARSO";
            // 
            // grdTemPack
            // 
            this.grdTemPack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTemPack.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemPack.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemPack.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemPack.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemPack.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemPack.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemPack.Location = new System.Drawing.Point(9, 125);
            this.grdTemPack.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPack.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPack.MainView = this.gridView1;
            this.grdTemPack.Name = "grdTemPack";
            this.grdTemPack.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcQty,
            this.grcQcPack,
            this.grcShowPack});
            this.grdTemPack.Size = new System.Drawing.Size(649, 232);
            this.grdTemPack.TabIndex = 143;
            this.grdTemPack.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grdTemPack;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.CacheValuesOnRowUpdating = DevExpress.Data.CacheRowValuesMode.Disabled;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsNavigation.AutoFocusNewRow = true;
            this.gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView1.OptionsNavigation.UseTabKey = false;
            this.gridView1.OptionsSelection.InvertSelection = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // grcQty
            // 
            this.grcQty.AutoHeight = false;
            this.grcQty.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcQty.DisplayFormat.FormatString = "n2";
            this.grcQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcQty.EditFormat.FormatString = "n2";
            this.grcQty.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcQty.Name = "grcQty";
            // 
            // grcQcPack
            // 
            this.grcQcPack.AutoHeight = false;
            this.grcQcPack.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.SearchInCol, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.grcQcPack.Name = "grcQcPack";
            // 
            // grcShowPack
            // 
            this.grcShowPack.AutoHeight = false;
            this.grcShowPack.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.SearchInCol, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.grcShowPack.Name = "grcShowPack";
            // 
            // lblTitle1
            // 
            this.lblTitle1.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle1.Location = new System.Drawing.Point(151, 99);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(489, 23);
            this.lblTitle1.TabIndex = 146;
            this.lblTitle1.Text = "ระบุรูปแบบการจัดเก็บ";
            this.lblTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnWHPack
            // 
            this.txtQnWHPack.Enabled = false;
            this.txtQnWHPack.EnterMoveNextControl = true;
            this.txtQnWHPack.Location = new System.Drawing.Point(327, 48);
            this.txtQnWHPack.Name = "txtQnWHPack";
            this.txtQnWHPack.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnWHPack.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnWHPack.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnWHPack.Properties.Appearance.Options.UseFont = true;
            this.txtQnWHPack.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnWHPack.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnWHPack.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnWHPack.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnWHPack.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnWHPack.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.txtQnWHPack.Size = new System.Drawing.Size(268, 22);
            this.txtQnWHPack.TabIndex = 142;
            // 
            // txtSumQty
            // 
            this.txtSumQty.Enabled = false;
            this.txtSumQty.EnterMoveNextControl = true;
            this.txtSumQty.Location = new System.Drawing.Point(154, 74);
            this.txtSumQty.Name = "txtSumQty";
            this.txtSumQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSumQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtSumQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtSumQty.Properties.Appearance.Options.UseFont = true;
            this.txtSumQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtSumQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtSumQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtSumQty.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, false)});
            this.txtSumQty.Size = new System.Drawing.Size(167, 22);
            this.txtSumQty.TabIndex = 141;
            // 
            // txtQcWHPack
            // 
            this.txtQcWHPack.Enabled = false;
            this.txtQcWHPack.EnterMoveNextControl = true;
            this.txtQcWHPack.Location = new System.Drawing.Point(154, 48);
            this.txtQcWHPack.Name = "txtQcWHPack";
            this.txtQcWHPack.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcWHPack.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcWHPack.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcWHPack.Properties.Appearance.Options.UseFont = true;
            this.txtQcWHPack.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcWHPack.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcWHPack.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcWHPack.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, false)});
            this.txtQcWHPack.Size = new System.Drawing.Size(167, 22);
            this.txtQcWHPack.TabIndex = 140;
            // 
            // lblQty
            // 
            this.lblQty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty.Location = new System.Drawing.Point(-3, 73);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(158, 23);
            this.lblQty.TabIndex = 145;
            this.lblQty.Text = "Storage Size :";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCoor
            // 
            this.lblCoor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCoor.Location = new System.Drawing.Point(-3, 47);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(158, 23);
            this.lblCoor.TabIndex = 144;
            this.lblCoor.Text = "Storage :";
            this.lblCoor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Print Barcode";
            this.barButtonItem4.Glyph = global::BeSmartMRP.Properties.Resources.barcode;
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.Tag = "PRN_BARCODE";
            // 
            // dlgShowPack_Det
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 395);
            this.Controls.Add(this.grdTemPack);
            this.Controls.Add(this.lblTitle1);
            this.Controls.Add(this.txtQnWHPack);
            this.Controls.Add(this.txtSumQty);
            this.Controls.Add(this.txtQcWHPack);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.lblCoor);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "dlgShowPack_Det";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pack Detail";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShowPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnWHPack.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcWHPack.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem btnDelSO;
        private DevExpress.XtraGrid.GridControl grdTemPack;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcPack;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcShowPack;
        private System.Windows.Forms.Label lblTitle1;
        private DevExpress.XtraEditors.ButtonEdit txtQnWHPack;
        private DevExpress.XtraEditors.ButtonEdit txtSumQty;
        private DevExpress.XtraEditors.ButtonEdit txtQcWHPack;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label lblCoor;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
    }
}