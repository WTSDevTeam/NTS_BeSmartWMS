namespace BeSmartMRP.DatabaseForms
{
    partial class frmWkCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWkCalendar));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.grdTemWkHour = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.grcQcMachine = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcQnMachine = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcQcDept = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcAllocPcn = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQty = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcQnUM = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grcAmt = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemWkHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAllocPcn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnUM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMain,
            this.bar1});
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
            // 
            // barMain
            // 
            this.barMain.BarName = "Tools";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barMain.Text = "Tools";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "F2-Edit";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.Open;
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "EDIT";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "F10-Save";
            this.barButtonItem2.Glyph = global::BeSmartMRP.Properties.Resources.Save;
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10);
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "SAVE";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Esc-Exit";
            this.barButtonItem3.Glyph = global::BeSmartMRP.Properties.Resources.Exit01;
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Tag = "EXIT";
            // 
            // bar1
            // 
            this.bar1.BarName = "statusbar";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "statusbar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(749, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 487);
            this.barDockControlBottom.Size = new System.Drawing.Size(749, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 458);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(749, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 458);
            // 
            // grdTemWkHour
            // 
            this.grdTemWkHour.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTemWkHour.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemWkHour.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemWkHour.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemWkHour.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemWkHour.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemWkHour.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemWkHour.Location = new System.Drawing.Point(12, 96);
            this.grdTemWkHour.MainView = this.gridView1;
            this.grdTemWkHour.Name = "grdTemWkHour";
            this.grdTemWkHour.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcRemark,
            this.grcQcMachine,
            this.grcQnMachine,
            this.grcType,
            this.grcQcDept,
            this.grcAllocPcn,
            this.grcQty,
            this.grcQnUM,
            this.grcAmt});
            this.grdTemWkHour.Size = new System.Drawing.Size(720, 385);
            this.grdTemWkHour.TabIndex = 23;
            this.grdTemWkHour.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.grdTemWkHour;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.CacheValuesOnRowUpdating = DevExpress.Data.CacheRowValuesMode.Disabled;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
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
            // grcRemark
            // 
            this.grcRemark.AutoHeight = false;
            this.grcRemark.Name = "grcRemark";
            // 
            // grcQcMachine
            // 
            this.grcQcMachine.AutoHeight = false;
            this.grcQcMachine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("grcQcMachine.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.grcQcMachine.Name = "grcQcMachine";
            this.grcQcMachine.ValidateOnEnterKey = true;
            // 
            // grcQnMachine
            // 
            this.grcQnMachine.AutoHeight = false;
            this.grcQnMachine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("grcQnMachine.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.grcQnMachine.Name = "grcQnMachine";
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("grcQcDept.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("grcQnUM.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.grcQnUM.Name = "grcQnUM";
            // 
            // grcAmt
            // 
            this.grcAmt.AutoHeight = false;
            this.grcAmt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, false)});
            this.grcAmt.DisplayFormat.FormatString = "##,###,###.00";
            this.grcAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.EditFormat.FormatString = "##,###,###.00";
            this.grcAmt.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.Mask.EditMask = "##,###,###.00";
            this.grcAmt.Name = "grcAmt";
            // 
            // pnlTitle
            // 
            this.pnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.pictureBox1);
            this.pnlTitle.Location = new System.Drawing.Point(-13, 27);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(775, 63);
            this.pnlTitle.TabIndex = 242;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(83, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(589, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "WORK CALENDAR ENTRY";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BeSmartMRP.Properties.Resources.ico_lrg_1084;
            this.pictureBox1.Location = new System.Drawing.Point(15, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 49);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 344);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(378, 128);
            this.dataGridView1.TabIndex = 247;
            this.dataGridView1.Visible = false;
            // 
            // frmWkCalendar
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 514);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.grdTemWkHour);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmWkCalendar";
            this.Text = "Work Calendar Entry";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemWkHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQcDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAllocPcn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcQnUM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl grdTemWkHour;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit grcRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcMachine;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnMachine;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcType;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQcDept;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAllocPcn;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit grcQnUM;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAmt;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}