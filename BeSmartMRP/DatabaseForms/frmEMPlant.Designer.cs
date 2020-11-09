namespace BeSmartMRP.DatabaseForms
{
    partial class frmEMPlant
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEMPlant));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
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
            this.txtQcStdWorkHr = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcHoliday = new DevExpress.XtraEditors.ButtonEdit();
            this.lblStdWorkHr = new System.Windows.Forms.Label();
            this.lblHoliday = new System.Windows.Forms.Label();
            this.cmbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcStdWorkHr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcHoliday.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
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
            // 
            // barMain
            // 
            this.barMain.BarName = "Tools";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.Text = "Tools";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barMainEdit;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.barDockControlTop.Size = new System.Drawing.Size(1320, 20);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 964);
            this.barDockControlBottom.Manager = this.barMainEdit;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.barDockControlBottom.Size = new System.Drawing.Size(1320, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 20);
            this.barDockControlLeft.Manager = this.barMainEdit;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 944);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1320, 20);
            this.barDockControlRight.Manager = this.barMainEdit;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 944);
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 20);
            this.pgfMainEdit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(1320, 944);
            this.pgfMainEdit.TabIndex = 13;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.ImageOptions.Image = global::BeSmartMRP.Properties.Resources.data_table;
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1316, 893);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(20, 723);
            this.txtFooter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(1282, 147);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdBrowView.Location = new System.Drawing.Point(20, 12);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.Size = new System.Drawing.Size(1282, 699);
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
            this.gridView1.DetailHeight = 727;
            this.gridView1.FixedLineWidth = 4;
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
            this.xtraTabPage2.Controls.Add(this.txtQcStdWorkHr);
            this.xtraTabPage2.Controls.Add(this.txtQcHoliday);
            this.xtraTabPage2.Controls.Add(this.lblStdWorkHr);
            this.xtraTabPage2.Controls.Add(this.lblHoliday);
            this.xtraTabPage2.Controls.Add(this.cmbType);
            this.xtraTabPage2.Controls.Add(this.label4);
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.ImageOptions.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1316, 893);
            this.xtraTabPage2.Text = "General Detail";
            // 
            // txtQcStdWorkHr
            // 
            this.txtQcStdWorkHr.EnterMoveNextControl = true;
            this.txtQcStdWorkHr.Location = new System.Drawing.Point(394, 382);
            this.txtQcStdWorkHr.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtQcStdWorkHr.Name = "txtQcStdWorkHr";
            this.txtQcStdWorkHr.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcStdWorkHr.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcStdWorkHr.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcStdWorkHr.Properties.Appearance.Options.UseFont = true;
            this.txtQcStdWorkHr.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcStdWorkHr.Properties.AppearanceFocused.Options.UseBackColor = true;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.txtQcStdWorkHr.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtQcStdWorkHr.Size = new System.Drawing.Size(272, 46);
            this.txtQcStdWorkHr.TabIndex = 4;
            // 
            // txtQcHoliday
            // 
            this.txtQcHoliday.EnterMoveNextControl = true;
            this.txtQcHoliday.Location = new System.Drawing.Point(394, 280);
            this.txtQcHoliday.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtQcHoliday.Name = "txtQcHoliday";
            this.txtQcHoliday.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcHoliday.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcHoliday.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcHoliday.Properties.Appearance.Options.UseFont = true;
            this.txtQcHoliday.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcHoliday.Properties.AppearanceFocused.Options.UseBackColor = true;
            editorButtonImageOptions2.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions2.Image")));
            this.txtQcHoliday.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtQcHoliday.Size = new System.Drawing.Size(272, 46);
            this.txtQcHoliday.TabIndex = 3;
            // 
            // lblStdWorkHr
            // 
            this.lblStdWorkHr.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblStdWorkHr.Location = new System.Drawing.Point(388, 332);
            this.lblStdWorkHr.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblStdWorkHr.Name = "lblStdWorkHr";
            this.lblStdWorkHr.Size = new System.Drawing.Size(796, 48);
            this.lblStdWorkHr.TabIndex = 248;
            this.lblStdWorkHr.Text = "ระบุฐานข้อมูลวันหยุด :";
            this.lblStdWorkHr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHoliday
            // 
            this.lblHoliday.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblHoliday.Location = new System.Drawing.Point(388, 226);
            this.lblHoliday.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHoliday.Name = "lblHoliday";
            this.lblHoliday.Size = new System.Drawing.Size(824, 48);
            this.lblHoliday.TabIndex = 247;
            this.lblHoliday.Text = "ระบุฐานข้อมูลวันหยุด :";
            this.lblHoliday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbType
            // 
            this.cmbType.EnterMoveNextControl = true;
            this.cmbType.Location = new System.Drawing.Point(394, 492);
            this.cmbType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbType.Name = "cmbType";
            this.cmbType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbType.Properties.Appearance.Options.UseBackColor = true;
            this.cmbType.Properties.Appearance.Options.UseFont = true;
            this.cmbType.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbType.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.cmbType.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbType.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbType.Properties.AppearanceFocused.Options.UseBackColor = true;
            editorButtonImageOptions3.EnableTransparency = false;
            this.cmbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.cmbType.Properties.PopupSizeable = true;
            this.cmbType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbType.Size = new System.Drawing.Size(274, 46);
            this.cmbType.TabIndex = 14;
            this.cmbType.Visible = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(122, 490);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(270, 48);
            this.label4.TabIndex = 15;
            this.label4.Text = "ชนิด :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(394, 164);
            this.txtName2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Size = new System.Drawing.Size(678, 46);
            this.txtName2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(54, 164);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 48);
            this.label2.TabIndex = 0;
            this.label2.Text = "Plant Name 2 :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(394, 114);
            this.txtName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.Appearance.Options.UseFont = true;
            this.txtName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName.Size = new System.Drawing.Size(678, 46);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(48, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plant Name :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(394, 64);
            this.txtCode.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.BackColor = System.Drawing.Color.Moccasin;
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.txtCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCode.Size = new System.Drawing.Size(360, 46);
            this.txtCode.TabIndex = 0;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(42, 64);
            this.lblCode.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(350, 48);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Plant Code :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmEMPlant
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 964);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("frmEMPlant.IconOptions.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "frmEMPlant";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Plant Items";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQcStdWorkHr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcHoliday.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private DevExpress.XtraEditors.ComboBoxEdit cmbType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.ButtonEdit txtQcStdWorkHr;
        private DevExpress.XtraEditors.ButtonEdit txtQcHoliday;
        private System.Windows.Forms.Label lblStdWorkHr;
        private System.Windows.Forms.Label lblHoliday;
    }
}