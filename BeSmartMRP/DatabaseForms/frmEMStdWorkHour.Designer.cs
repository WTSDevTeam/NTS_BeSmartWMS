namespace BeSmartMRP.DatabaseForms
{
    partial class frmEMStdWorkHour
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
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
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
            this.btnGrpUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.txtSumOTWkHour = new DevExpress.XtraEditors.SpinEdit();
            this.txtSumWkHour = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOTWKHour = new System.Windows.Forms.Label();
            this.lblWkHour = new System.Windows.Forms.Label();
            this.lsbDayList = new DevExpress.XtraEditors.ListBoxControl();
            this.grdTemOTHour = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.grcBegTime2 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.grcEndTime2 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.grdTemWorkHour = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcCostBy = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAmt = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.grcBegDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.grcBegTime = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.grcEndTime = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.lblName2 = new System.Windows.Forms.Label();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.lblName = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSumOTWkHour.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumWkHour.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbDayList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemOTHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegTime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcEndTime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemWorkHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcCostBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barMainEdit
            // 
            this.barMainEdit.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMain,
            this.bar3});
            this.barMainEdit.DockControls.Add(this.barDockControlTop);
            this.barMainEdit.DockControls.Add(this.barDockControlBottom);
            this.barMainEdit.DockControls.Add(this.barDockControlLeft);
            this.barMainEdit.DockControls.Add(this.barDockControlRight);
            this.barMainEdit.Form = this;
            this.barMainEdit.MaxItemId = 0;
            this.barMainEdit.StatusBar = this.bar3;
            // 
            // barMain
            // 
            this.barMain.BarName = "Tools";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.Text = "Tools";
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
            this.barDockControlTop.Size = new System.Drawing.Size(711, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 493);
            this.barDockControlBottom.Size = new System.Drawing.Size(711, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 464);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(711, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 464);
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
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 29);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.PaintStyleName = "PropertyView";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(711, 464);
            this.pgfMainEdit.TabIndex = 17;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.data_table;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(709, 440);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(8, 364);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(690, 71);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.Location = new System.Drawing.Point(10, 6);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.Size = new System.Drawing.Size(690, 352);
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
            this.xtraTabPage2.Controls.Add(this.btnGrpUpdate);
            this.xtraTabPage2.Controls.Add(this.txtSumOTWkHour);
            this.xtraTabPage2.Controls.Add(this.txtSumWkHour);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.label3);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.lblOTWKHour);
            this.xtraTabPage2.Controls.Add(this.lblWkHour);
            this.xtraTabPage2.Controls.Add(this.lsbDayList);
            this.xtraTabPage2.Controls.Add(this.grdTemOTHour);
            this.xtraTabPage2.Controls.Add(this.grdTemWorkHour);
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.lblName2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.lblName);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(709, 444);
            this.xtraTabPage2.Text = "General Detail";
            // 
            // btnGrpUpdate
            // 
            this.btnGrpUpdate.AllowFocus = false;
            this.btnGrpUpdate.Location = new System.Drawing.Point(543, 96);
            this.btnGrpUpdate.Name = "btnGrpUpdate";
            this.btnGrpUpdate.Size = new System.Drawing.Size(77, 21);
            this.btnGrpUpdate.TabIndex = 40;
            this.btnGrpUpdate.TabStop = false;
            this.btnGrpUpdate.Text = "Group Update";
            this.btnGrpUpdate.Visible = false;
            this.btnGrpUpdate.Click += new System.EventHandler(this.btnGrpUpdate_Click);
            // 
            // txtSumOTWkHour
            // 
            this.txtSumOTWkHour.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSumOTWkHour.EnterMoveNextControl = true;
            this.txtSumOTWkHour.Location = new System.Drawing.Point(551, 412);
            this.txtSumOTWkHour.Name = "txtSumOTWkHour";
            this.txtSumOTWkHour.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSumOTWkHour.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtSumOTWkHour.Properties.Appearance.Options.UseBackColor = true;
            this.txtSumOTWkHour.Properties.Appearance.Options.UseFont = true;
            this.txtSumOTWkHour.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtSumOTWkHour.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtSumOTWkHour.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSumOTWkHour.Properties.DisplayFormat.FormatString = "n2";
            this.txtSumOTWkHour.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtSumOTWkHour.Properties.EditFormat.FormatString = "n2";
            this.txtSumOTWkHour.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtSumOTWkHour.Properties.ReadOnly = true;
            this.txtSumOTWkHour.Size = new System.Drawing.Size(69, 22);
            this.txtSumOTWkHour.TabIndex = 38;
            // 
            // txtSumWkHour
            // 
            this.txtSumWkHour.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSumWkHour.EnterMoveNextControl = true;
            this.txtSumWkHour.Location = new System.Drawing.Point(326, 413);
            this.txtSumWkHour.Name = "txtSumWkHour";
            this.txtSumWkHour.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSumWkHour.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtSumWkHour.Properties.Appearance.Options.UseBackColor = true;
            this.txtSumWkHour.Properties.Appearance.Options.UseFont = true;
            this.txtSumWkHour.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtSumWkHour.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtSumWkHour.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSumWkHour.Properties.DisplayFormat.FormatString = "n2";
            this.txtSumWkHour.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtSumWkHour.Properties.EditFormat.FormatString = "n2";
            this.txtSumWkHour.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtSumWkHour.Properties.ReadOnly = true;
            this.txtSumWkHour.Size = new System.Drawing.Size(69, 22);
            this.txtSumWkHour.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                            | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(326, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 23);
            this.label2.TabIndex = 39;
            this.label2.Text = "OT Hour";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                            | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(170, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 23);
            this.label3.TabIndex = 39;
            this.label3.Text = "Work Day";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                            | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(331, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 39;
            this.label1.Text = "Work Hour";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOTWKHour
            // 
            this.lblOTWKHour.BackColor = System.Drawing.Color.Transparent;
            this.lblOTWKHour.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblOTWKHour.Location = new System.Drawing.Point(454, 412);
            this.lblOTWKHour.Name = "lblOTWKHour";
            this.lblOTWKHour.Size = new System.Drawing.Size(105, 23);
            this.lblOTWKHour.TabIndex = 39;
            this.lblOTWKHour.Text = "Total OT Hour";
            this.lblOTWKHour.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWkHour
            // 
            this.lblWkHour.BackColor = System.Drawing.Color.Transparent;
            this.lblWkHour.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblWkHour.Location = new System.Drawing.Point(178, 413);
            this.lblWkHour.Name = "lblWkHour";
            this.lblWkHour.Size = new System.Drawing.Size(142, 23);
            this.lblWkHour.TabIndex = 39;
            this.lblWkHour.Text = "Total Work Hour";
            this.lblWkHour.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lsbDayList
            // 
            this.lsbDayList.Location = new System.Drawing.Point(169, 120);
            this.lsbDayList.Name = "lsbDayList";
            this.lsbDayList.Size = new System.Drawing.Size(151, 282);
            this.lsbDayList.TabIndex = 3;
            this.lsbDayList.SelectedIndexChanged += new System.EventHandler(this.lsbDayList_SelectedIndexChanged);
            // 
            // grdTemOTHour
            // 
            this.grdTemOTHour.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemOTHour.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemOTHour.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemOTHour.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemOTHour.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemOTHour.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemOTHour.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.grdTemPd_EmbeddedNavigator_ButtonClick);
            this.grdTemOTHour.Location = new System.Drawing.Point(326, 292);
            this.grdTemOTHour.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemOTHour.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.grdTemOTHour.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemOTHour.MainView = this.gridView3;
            this.grdTemOTHour.Name = "grdTemOTHour";
            this.grdTemOTHour.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemSpinEdit1,
            this.repositoryItemDateEdit1,
            this.grcBegTime2,
            this.grcEndTime2});
            this.grdTemOTHour.Size = new System.Drawing.Size(294, 110);
            this.grdTemOTHour.TabIndex = 4;
            this.grdTemOTHour.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView3.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView3.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView3.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView3.GridControl = this.grdTemOTHour;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.OptionsBehavior.CacheValuesOnRowUpdating = DevExpress.Data.CacheRowValuesMode.Disabled;
            this.gridView3.OptionsCustomization.AllowFilter = false;
            this.gridView3.OptionsCustomization.AllowSort = false;
            this.gridView3.OptionsMenu.EnableColumnMenu = false;
            this.gridView3.OptionsMenu.EnableFooterMenu = false;
            this.gridView3.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView3.OptionsNavigation.AutoFocusNewRow = true;
            this.gridView3.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView3.OptionsNavigation.UseTabKey = false;
            this.gridView3.OptionsSelection.InvertSelection = true;
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView3.OptionsView.ShowFooter = true;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsView.ShowIndicator = false;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "-",
            "SEC",
            "MIN",
            "HOUR",
            "DAY"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.DisplayFormat.FormatString = "n2";
            this.repositoryItemSpinEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repositoryItemSpinEdit1.EditFormat.FormatString = "n2";
            this.repositoryItemSpinEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repositoryItemSpinEdit1.Mask.EditMask = "n2";
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.DisplayFormat.FormatString = "dd/MM";
            this.repositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.EditFormat.FormatString = "dd/MM";
            this.repositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.Mask.EditMask = "dd/MM";
            this.repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // grcBegTime2
            // 
            this.grcBegTime2.AutoHeight = false;
            this.grcBegTime2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcBegTime2.Mask.EditMask = "HH:mm";
            this.grcBegTime2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.grcBegTime2.Name = "grcBegTime2";
            // 
            // grcEndTime2
            // 
            this.grcEndTime2.AutoHeight = false;
            this.grcEndTime2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcEndTime2.Mask.EditMask = "HH:mm";
            this.grcEndTime2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.grcEndTime2.Name = "grcEndTime2";
            // 
            // grdTemWorkHour
            // 
            this.grdTemWorkHour.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemWorkHour.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemWorkHour.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemWorkHour.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemWorkHour.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemWorkHour.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemWorkHour.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.grdTemPd_EmbeddedNavigator_ButtonClick);
            this.grdTemWorkHour.Location = new System.Drawing.Point(326, 120);
            this.grdTemWorkHour.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemWorkHour.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.grdTemWorkHour.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemWorkHour.MainView = this.gridView2;
            this.grdTemWorkHour.Name = "grdTemWorkHour";
            this.grdTemWorkHour.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcCostBy,
            this.grcAmt,
            this.grcBegDate,
            this.grcBegTime,
            this.grcEndTime});
            this.grdTemWorkHour.Size = new System.Drawing.Size(294, 146);
            this.grdTemWorkHour.TabIndex = 4;
            this.grdTemWorkHour.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView2.GridControl = this.grdTemWorkHour;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView2.OptionsBehavior.CacheValuesOnRowUpdating = DevExpress.Data.CacheRowValuesMode.Disabled;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsMenu.EnableColumnMenu = false;
            this.gridView2.OptionsMenu.EnableFooterMenu = false;
            this.gridView2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView2.OptionsNavigation.AutoFocusNewRow = true;
            this.gridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView2.OptionsNavigation.UseTabKey = false;
            this.gridView2.OptionsSelection.InvertSelection = true;
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView2.OptionsView.ShowFooter = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowIndicator = false;
            // 
            // grcCostBy
            // 
            this.grcCostBy.AutoHeight = false;
            this.grcCostBy.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grcCostBy.Items.AddRange(new object[] {
            "-",
            "SEC",
            "MIN",
            "HOUR",
            "DAY"});
            this.grcCostBy.Name = "grcCostBy";
            this.grcCostBy.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // grcAmt
            // 
            this.grcAmt.AutoHeight = false;
            this.grcAmt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcAmt.DisplayFormat.FormatString = "n2";
            this.grcAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.EditFormat.FormatString = "n2";
            this.grcAmt.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.grcAmt.Mask.EditMask = "n2";
            this.grcAmt.Name = "grcAmt";
            // 
            // grcBegDate
            // 
            this.grcBegDate.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.grcBegDate.AutoHeight = false;
            this.grcBegDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grcBegDate.DisplayFormat.FormatString = "dd/MM";
            this.grcBegDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grcBegDate.EditFormat.FormatString = "dd/MM";
            this.grcBegDate.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grcBegDate.Mask.EditMask = "dd/MM";
            this.grcBegDate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.grcBegDate.Name = "grcBegDate";
            this.grcBegDate.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // grcBegTime
            // 
            this.grcBegTime.AutoHeight = false;
            this.grcBegTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcBegTime.Mask.EditMask = "HH:mm";
            this.grcBegTime.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.grcBegTime.Name = "grcBegTime";
            // 
            // grcEndTime
            // 
            this.grcEndTime.AutoHeight = false;
            this.grcEndTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.grcEndTime.Mask.EditMask = "HH:mm";
            this.grcEndTime.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.grcEndTime.Name = "grcEndTime";
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(170, 68);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Size = new System.Drawing.Size(451, 22);
            this.txtName2.TabIndex = 2;
            // 
            // lblName2
            // 
            this.lblName2.BackColor = System.Drawing.Color.Transparent;
            this.lblName2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName2.Location = new System.Drawing.Point(0, 68);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(169, 23);
            this.lblName2.TabIndex = 0;
            this.lblName2.Text = "OPR. Name 2 :";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(170, 44);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.Appearance.Options.UseFont = true;
            this.txtName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName.Size = new System.Drawing.Size(451, 22);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName.Location = new System.Drawing.Point(-3, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(172, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "OPR. Name :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(170, 20);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.txtCode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCode.Size = new System.Drawing.Size(180, 22);
            this.txtCode.TabIndex = 0;
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(-6, 20);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(175, 23);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "OPR. Code :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmEMStdWorkHour
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 516);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmEMStdWorkHour";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmEMStdWorkHour";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSumOTWkHour.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumWkHour.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbDayList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemOTHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegTime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcEndTime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemWorkHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcCostBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcBegTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barMainEdit;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.Bar bar3;
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
        private DevExpress.XtraGrid.GridControl grdTemWorkHour;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcCostBy;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAmt;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit grcBegDate;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label lblName2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label lblName;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.ListBoxControl lsbDayList;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit grcBegTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit grcEndTime;
        private DevExpress.XtraGrid.GridControl grdTemOTHour;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit grcBegTime2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit grcEndTime2;
        private DevExpress.XtraEditors.SpinEdit txtSumWkHour;
        private System.Windows.Forms.Label lblWkHour;
        private DevExpress.XtraEditors.SpinEdit txtSumOTWkHour;
        private System.Windows.Forms.Label lblOTWKHour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton btnGrpUpdate;
    }
}