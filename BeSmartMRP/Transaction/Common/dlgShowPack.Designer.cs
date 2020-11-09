namespace BeSmartMRP.Transaction.Common
{
    partial class dlgShowPack
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgShowPack));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
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
            this.grdTemPack = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcQty = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQcPack = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcShowPack = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.txtQnProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCoor = new System.Windows.Forms.Label();
            this.txtSumQty = new DevExpress.XtraEditors.ButtonEdit();
            this.lblQty = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShowPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumQty.Properties)).BeginInit();
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelSO, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            this.barDockControlTop.Size = new System.Drawing.Size(719, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 454);
            this.barDockControlBottom.Size = new System.Drawing.Size(719, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 425);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(719, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 425);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 432);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(719, 22);
            this.statusStrip1.TabIndex = 114;
            this.statusStrip1.Text = "statusStrip1";
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
            this.grdTemPack.Location = new System.Drawing.Point(12, 128);
            this.grdTemPack.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemPack.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemPack.MainView = this.gridView1;
            this.grdTemPack.Name = "grdTemPack";
            this.grdTemPack.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcQty,
            this.grcQcPack,
            this.grcShowPack});
            this.grdTemPack.Size = new System.Drawing.Size(678, 282);
            this.grdTemPack.TabIndex = 136;
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
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
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
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
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
            this.grcShowPack.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcShowPack_ButtonClick);
            // 
            // lblTitle1
            // 
            this.lblTitle1.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle1.Location = new System.Drawing.Point(154, 102);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(489, 23);
            this.lblTitle1.TabIndex = 139;
            this.lblTitle1.Text = "ระบุรูปแบบการจัดเก็บ";
            this.lblTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnProd
            // 
            this.txtQnProd.Enabled = false;
            this.txtQnProd.EnterMoveNextControl = true;
            this.txtQnProd.Location = new System.Drawing.Point(330, 51);
            this.txtQnProd.Name = "txtQnProd";
            this.txtQnProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnProd.Properties.Appearance.Options.UseFont = true;
            this.txtQnProd.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnProd.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnProd.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.txtQnProd.Size = new System.Drawing.Size(268, 22);
            this.txtQnProd.TabIndex = 135;
            // 
            // txtQcProd
            // 
            this.txtQcProd.Enabled = false;
            this.txtQcProd.EnterMoveNextControl = true;
            this.txtQcProd.Location = new System.Drawing.Point(157, 51);
            this.txtQcProd.Name = "txtQcProd";
            this.txtQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcProd.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, false)});
            this.txtQcProd.Size = new System.Drawing.Size(167, 22);
            this.txtQcProd.TabIndex = 134;
            // 
            // lblCoor
            // 
            this.lblCoor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCoor.Location = new System.Drawing.Point(0, 50);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(158, 23);
            this.lblCoor.TabIndex = 137;
            this.lblCoor.Text = "สินค้า :";
            this.lblCoor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSumQty
            // 
            this.txtSumQty.Enabled = false;
            this.txtSumQty.EnterMoveNextControl = true;
            this.txtSumQty.Location = new System.Drawing.Point(157, 77);
            this.txtSumQty.Name = "txtSumQty";
            this.txtSumQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSumQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtSumQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtSumQty.Properties.Appearance.Options.UseFont = true;
            this.txtSumQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtSumQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtSumQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("buttonEdit1.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, false)});
            this.txtSumQty.Size = new System.Drawing.Size(167, 22);
            this.txtSumQty.TabIndex = 134;
            // 
            // lblQty
            // 
            this.lblQty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty.Location = new System.Drawing.Point(0, 76);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(158, 23);
            this.lblQty.TabIndex = 137;
            this.lblQty.Text = "จำนวน :";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgShowPack
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 454);
            this.Controls.Add(this.grdTemPack);
            this.Controls.Add(this.lblTitle1);
            this.Controls.Add(this.txtQnProd);
            this.Controls.Add(this.txtSumQty);
            this.Controls.Add(this.txtQcProd);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.lblCoor);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "dlgShowPack";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Storage";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShowPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumQty.Properties)).EndInit();
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
        private DevExpress.XtraGrid.GridControl grdTemPack;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcQty;
        private System.Windows.Forms.Label lblTitle1;
        private DevExpress.XtraEditors.ButtonEdit txtQnProd;
        private DevExpress.XtraEditors.ButtonEdit txtQcProd;
        private System.Windows.Forms.Label lblCoor;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcPack;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcShowPack;
        private DevExpress.XtraEditors.ButtonEdit txtSumQty;
        private System.Windows.Forms.Label lblQty;
    }
}