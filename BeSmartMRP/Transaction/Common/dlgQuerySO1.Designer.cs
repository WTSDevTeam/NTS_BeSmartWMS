namespace BeSmartMRP.Transaction.Common
{
    partial class dlgQuerySO1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgQuerySO1));
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcIsOut = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grcApprove = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAPVStat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcIsPost = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.pgfMainEdit = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.pnlTitle1 = new System.Windows.Forms.Panel();
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.pnlTitle2 = new System.Windows.Forms.Panel();
            this.lblTitle2 = new System.Windows.Forms.Label();
            this.txtTotMfgQty = new DevExpress.XtraEditors.SpinEdit();
            this.lblTotMfgQty = new System.Windows.Forms.Label();
            this.txtQnProd = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.lblProd = new System.Windows.Forms.Label();
            this.txtQcCoor = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCoor = new System.Windows.Forms.Label();
            this.grdTemPd = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.chkTag = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.pnlTitle1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.pnlTitle2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotMfgQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
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
            this.grcIsPost});
            this.grdBrowView.Size = new System.Drawing.Size(683, 333);
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
            this.gridView1.OptionsBehavior.Editable = false;
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
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 25);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.pgfMainEdit.Size = new System.Drawing.Size(705, 424);
            this.pgfMainEdit.TabIndex = 2;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage1.Controls.Add(this.pnlTitle1);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(705, 402);
            this.xtraTabPage1.Text = "Step 1";
            // 
            // pnlTitle1
            // 
            this.pnlTitle1.BackColor = System.Drawing.Color.White;
            this.pnlTitle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTitle1.Controls.Add(this.lblTitle1);
            this.pnlTitle1.Location = new System.Drawing.Point(-2, -1);
            this.pnlTitle1.Name = "pnlTitle1";
            this.pnlTitle1.Size = new System.Drawing.Size(708, 54);
            this.pnlTitle1.TabIndex = 129;
            // 
            // lblTitle1
            // 
            this.lblTitle1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle1.ForeColor = System.Drawing.Color.Navy;
            this.lblTitle1.Location = new System.Drawing.Point(14, 13);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(597, 23);
            this.lblTitle1.TabIndex = 119;
            this.lblTitle1.Text = "รายการสินค้าตาม S/O";
            this.lblTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage2.Controls.Add(this.pnlTitle2);
            this.xtraTabPage2.Controls.Add(this.txtTotMfgQty);
            this.xtraTabPage2.Controls.Add(this.lblTotMfgQty);
            this.xtraTabPage2.Controls.Add(this.txtQnProd);
            this.xtraTabPage2.Controls.Add(this.txtQnCoor);
            this.xtraTabPage2.Controls.Add(this.txtQcProd);
            this.xtraTabPage2.Controls.Add(this.lblProd);
            this.xtraTabPage2.Controls.Add(this.txtQcCoor);
            this.xtraTabPage2.Controls.Add(this.lblCoor);
            this.xtraTabPage2.Controls.Add(this.grdTemPd);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(705, 402);
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
            this.lblTitle2.Text = "รายการสินค้าตาม S/O";
            this.lblTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTotMfgQty
            // 
            this.txtTotMfgQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTotMfgQty.Enabled = false;
            this.txtTotMfgQty.EnterMoveNextControl = true;
            this.txtTotMfgQty.Location = new System.Drawing.Point(170, 119);
            this.txtTotMfgQty.Name = "txtTotMfgQty";
            this.txtTotMfgQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTotMfgQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTotMfgQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotMfgQty.Properties.Appearance.Options.UseFont = true;
            this.txtTotMfgQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTotMfgQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTotMfgQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtTotMfgQty.Properties.DisplayFormat.FormatString = "n2";
            this.txtTotMfgQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtTotMfgQty.Properties.EditFormat.FormatString = "n2";
            this.txtTotMfgQty.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtTotMfgQty.Size = new System.Drawing.Size(167, 22);
            this.txtTotMfgQty.TabIndex = 124;
            // 
            // lblTotMfgQty
            // 
            this.lblTotMfgQty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTotMfgQty.Location = new System.Drawing.Point(13, 119);
            this.lblTotMfgQty.Name = "lblTotMfgQty";
            this.lblTotMfgQty.Size = new System.Drawing.Size(158, 23);
            this.lblTotMfgQty.TabIndex = 127;
            this.lblTotMfgQty.Text = "รวมจำนวนที่สั่งผลิต :";
            this.lblTotMfgQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnProd
            // 
            this.txtQnProd.Enabled = false;
            this.txtQnProd.EnterMoveNextControl = true;
            this.txtQnProd.Location = new System.Drawing.Point(343, 94);
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnProd.Properties.Buttons"))))});
            this.txtQnProd.Size = new System.Drawing.Size(268, 22);
            this.txtQnProd.TabIndex = 123;
            // 
            // txtQnCoor
            // 
            this.txtQnCoor.Enabled = false;
            this.txtQnCoor.EnterMoveNextControl = true;
            this.txtQnCoor.Location = new System.Drawing.Point(343, 68);
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQnCoor.Properties.Buttons"))))});
            this.txtQnCoor.Size = new System.Drawing.Size(268, 22);
            this.txtQnCoor.TabIndex = 121;
            // 
            // txtQcProd
            // 
            this.txtQcProd.Enabled = false;
            this.txtQcProd.EnterMoveNextControl = true;
            this.txtQcProd.Location = new System.Drawing.Point(170, 94);
            this.txtQcProd.Name = "txtQcProd";
            this.txtQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcProd.Properties.Buttons"))))});
            this.txtQcProd.Size = new System.Drawing.Size(167, 22);
            this.txtQcProd.TabIndex = 122;
            // 
            // lblProd
            // 
            this.lblProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblProd.Location = new System.Drawing.Point(13, 93);
            this.lblProd.Name = "lblProd";
            this.lblProd.Size = new System.Drawing.Size(158, 23);
            this.lblProd.TabIndex = 126;
            this.lblProd.Text = "สินค้า :";
            this.lblProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcCoor
            // 
            this.txtQcCoor.Enabled = false;
            this.txtQcCoor.EnterMoveNextControl = true;
            this.txtQcCoor.Location = new System.Drawing.Point(170, 68);
            this.txtQcCoor.Name = "txtQcCoor";
            this.txtQcCoor.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcCoor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcCoor.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Appearance.Options.UseFont = true;
            this.txtQcCoor.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcCoor.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcCoor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("txtQcCoor.Properties.Buttons"))))});
            this.txtQcCoor.Size = new System.Drawing.Size(167, 22);
            this.txtQcCoor.TabIndex = 120;
            // 
            // lblCoor
            // 
            this.lblCoor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCoor.Location = new System.Drawing.Point(13, 67);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(158, 23);
            this.lblCoor.TabIndex = 125;
            this.lblCoor.Text = "ลูกค้า :";
            this.lblCoor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.grdTemPd.Location = new System.Drawing.Point(10, 151);
            this.grdTemPd.MainView = this.gridView2;
            this.grdTemPd.Name = "grdTemPd";
            this.grdTemPd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkTag});
            this.grdTemPd.Size = new System.Drawing.Size(683, 233);
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
            this.gridView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView2_MouseDown);
            this.gridView2.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView2_ColumnWidthChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // chkTag
            // 
            this.chkTag.AutoHeight = false;
            this.chkTag.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkTag.Name = "chkTag";
            this.chkTag.CheckedChanged += new System.EventHandler(this.chkTag_CheckedChanged);
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
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "F10-OK";
            this.barButtonItem1.Glyph = global::BeSmartMRP.Properties.Resources.text_ok;
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "OK";
            // 
            // dlgQuerySO1
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
            this.Name = "dlgQuerySO1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Sale Order";
            this.Resize += new System.EventHandler(this.dlgQuerySO1_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgQuerySO1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.pnlTitle1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.pnlTitle2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTotMfgQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcCoor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcApprove;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcAPVStat;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsPost;
        private DevExpress.XtraTab.XtraTabControl pgfMainEdit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem btnNext;
        private DevExpress.XtraBars.BarButtonItem btnPrev;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl grdTemPd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkTag;
        private System.Windows.Forms.Label lblTitle2;
        private DevExpress.XtraEditors.SpinEdit txtTotMfgQty;
        private System.Windows.Forms.Label lblTotMfgQty;
        private DevExpress.XtraEditors.ButtonEdit txtQnProd;
        private DevExpress.XtraEditors.ButtonEdit txtQnCoor;
        private DevExpress.XtraEditors.ButtonEdit txtQcProd;
        private System.Windows.Forms.Label lblProd;
        private DevExpress.XtraEditors.ButtonEdit txtQcCoor;
        private System.Windows.Forms.Label lblCoor;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem btnOK2;
        private System.Windows.Forms.Panel pnlTitle1;
        private System.Windows.Forms.Label lblTitle1;
        private System.Windows.Forms.Panel pnlTitle2;
    }
}