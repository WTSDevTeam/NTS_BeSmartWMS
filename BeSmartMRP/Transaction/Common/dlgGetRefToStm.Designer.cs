namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetRefToStm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgGetRefToStm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelSO = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcQty = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.txtQnCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnBook = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCoor = new System.Windows.Forms.Label();
            this.txtQcBook = new DevExpress.XtraEditors.ButtonEdit();
            this.lblBook = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBook.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBook.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.btnDelSO});
            this.barMainEdit.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelSO, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "F8-Load";
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
            this.barDockControlTop.Size = new System.Drawing.Size(669, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 405);
            this.barDockControlBottom.Size = new System.Drawing.Size(669, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 376);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(669, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 376);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 383);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(669, 22);
            this.statusStrip1.TabIndex = 114;
            this.statusStrip1.Text = "statusStrip1";
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
            this.grdTemPd.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemPd.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.grdTemPd_EmbeddedNavigator_ButtonClick);
            this.grdTemPd.Location = new System.Drawing.Point(25, 123);
            this.grdTemPd.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPd.MainView = this.gridView1;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcQty});
            this.grdTemPd.Size = new System.Drawing.Size(631, 238);
            this.grdTemPd.TabIndex = 136;
            this.grdTemPd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grdTemPd;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.CacheValuesOnRowUpdating = DevExpress.Data.CacheRowValuesMode.Disabled;
            this.gridView1.OptionsCustomization.AllowFilter = false;
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
            // lblTitle1
            // 
            this.lblTitle1.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle1.Location = new System.Drawing.Point(167, 97);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(489, 23);
            this.lblTitle1.TabIndex = 139;
            this.lblTitle1.Text = "รายการสินค้า";
            this.lblTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnCoor
            // 
            this.txtQnCoor.Enabled = false;
            this.txtQnCoor.EnterMoveNextControl = true;
            this.txtQnCoor.Location = new System.Drawing.Point(343, 71);
            this.txtQnCoor.Name = "txtQnCoor";
            this.txtQnCoor.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnCoor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnCoor.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnCoor.Properties.Appearance.Options.UseFont = true;
            this.txtQnCoor.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnCoor.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnCoor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnCoor.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnCoor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnCoor.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtQnCoor.Size = new System.Drawing.Size(268, 22);
            this.txtQnCoor.TabIndex = 135;
            // 
            // txtQnBook
            // 
            this.txtQnBook.Enabled = false;
            this.txtQnBook.EnterMoveNextControl = true;
            this.txtQnBook.Location = new System.Drawing.Point(283, 44);
            this.txtQnBook.Name = "txtQnBook";
            this.txtQnBook.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnBook.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnBook.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnBook.Properties.Appearance.Options.UseFont = true;
            this.txtQnBook.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnBook.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnBook.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnBook.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnBook.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnBook.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtQnBook.Size = new System.Drawing.Size(328, 22);
            this.txtQnBook.TabIndex = 133;
            // 
            // txtQcCoor
            // 
            this.txtQcCoor.Enabled = false;
            this.txtQcCoor.EnterMoveNextControl = true;
            this.txtQcCoor.Location = new System.Drawing.Point(170, 71);
            this.txtQcCoor.Name = "txtQcCoor";
            this.txtQcCoor.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcCoor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcCoor.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Appearance.Options.UseFont = true;
            this.txtQcCoor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcCoor.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcCoor.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.txtQcCoor.Size = new System.Drawing.Size(167, 22);
            this.txtQcCoor.TabIndex = 134;
            // 
            // lblCoor
            // 
            this.lblCoor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCoor.Location = new System.Drawing.Point(13, 70);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(158, 23);
            this.lblCoor.TabIndex = 137;
            this.lblCoor.Text = "ลูกค้า :";
            this.lblCoor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcBook
            // 
            this.txtQcBook.EnterMoveNextControl = true;
            this.txtQcBook.Location = new System.Drawing.Point(170, 44);
            this.txtQcBook.Name = "txtQcBook";
            this.txtQcBook.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcBook.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcBook.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcBook.Properties.Appearance.Options.UseFont = true;
            this.txtQcBook.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcBook.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcBook.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcBook.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.txtQcBook.Size = new System.Drawing.Size(107, 22);
            this.txtQcBook.TabIndex = 132;
            // 
            // lblBook
            // 
            this.lblBook.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblBook.Location = new System.Drawing.Point(13, 43);
            this.lblBook.Name = "lblBook";
            this.lblBook.Size = new System.Drawing.Size(158, 23);
            this.lblBook.TabIndex = 138;
            this.lblBook.Text = "ระบุเล่มเอกสาร :";
            this.lblBook.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgGetRefToStm
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 405);
            this.ControlBox = false;
            this.Controls.Add(this.grdTemPd);
            this.Controls.Add(this.lblTitle1);
            this.Controls.Add(this.txtQnCoor);
            this.Controls.Add(this.txtQnBook);
            this.Controls.Add(this.txtQcCoor);
            this.Controls.Add(this.lblCoor);
            this.Controls.Add(this.txtQcBook);
            this.Controls.Add(this.lblBook);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "dlgGetRefToStm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "อ้างอิงใบขอเบิก";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnBook.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcBook.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem btnDelSO;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcQty;
        private System.Windows.Forms.Label lblTitle1;
        private DevExpress.XtraEditors.ButtonEdit txtQnCoor;
        private DevExpress.XtraEditors.ButtonEdit txtQnBook;
        private DevExpress.XtraEditors.ButtonEdit txtQcCoor;
        private System.Windows.Forms.Label lblCoor;
        private DevExpress.XtraEditors.ButtonEdit txtQcBook;
        private System.Windows.Forms.Label lblBook;
    }
}