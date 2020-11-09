﻿namespace BeSmartMRP.DatabaseForms
{
    partial class frmMFResource
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
            pmClearInstanse(this.mResourceType);
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
            this.grbDetail = new DevExpress.XtraEditors.GroupControl();
            this.txtInstall = new DevExpress.XtraEditors.DateEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.txtModel = new DevExpress.XtraEditors.ButtonEdit();
            this.txtSerialNo = new DevExpress.XtraEditors.ButtonEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.grbCapacity = new DevExpress.XtraEditors.GroupControl();
            this.txtUnderLoad = new DevExpress.XtraEditors.SpinEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOverLoad = new DevExpress.XtraEditors.SpinEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCapacity = new DevExpress.XtraEditors.SpinEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtQnJob = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnSect = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnMCType = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQcJob = new DevExpress.XtraEditors.ButtonEdit();
            this.lblJob = new System.Windows.Forms.Label();
            this.txtQcSect = new DevExpress.XtraEditors.ButtonEdit();
            this.lblSect = new System.Windows.Forms.Label();
            this.txtQcMCType = new DevExpress.XtraEditors.ButtonEdit();
            this.lblMCType = new System.Windows.Forms.Label();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.lblName2 = new System.Windows.Forms.Label();
            this.txtName = new DevExpress.XtraEditors.ButtonEdit();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.ButtonEdit();
            this.lblCode = new System.Windows.Forms.Label();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.grdTemCost = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcCostBy = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAmt = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).BeginInit();
            this.pgfMainEdit.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbDetail)).BeginInit();
            this.grbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInstall.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInstall.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerialNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbCapacity)).BeginInit();
            this.grbCapacity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnderLoad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverLoad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCapacity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnMCType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcMCType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcCostBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).BeginInit();
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
            this.barMain.Text = "Tools";
            // 
            // pgfMainEdit
            // 
            this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
            this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pgfMainEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgfMainEdit.Location = new System.Drawing.Point(0, 23);
            this.pgfMainEdit.Name = "pgfMainEdit";
            this.pgfMainEdit.SelectedTabPage = this.xtraTabPage1;
            this.pgfMainEdit.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.False;
            this.pgfMainEdit.Size = new System.Drawing.Size(775, 543);
            this.pgfMainEdit.TabIndex = 16;
            this.pgfMainEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            this.pgfMainEdit.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage1.Controls.Add(this.txtFooter);
            this.xtraTabPage1.Controls.Add(this.grdBrowView);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.data_table;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(775, 519);
            this.xtraTabPage1.Text = "Browse Page";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(10, 437);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(756, 71);
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
            this.grdBrowView.Size = new System.Drawing.Size(756, 425);
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
            this.xtraTabPage2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage2.Controls.Add(this.grbDetail);
            this.xtraTabPage2.Controls.Add(this.grbCapacity);
            this.xtraTabPage2.Controls.Add(this.txtQnJob);
            this.xtraTabPage2.Controls.Add(this.txtQnSect);
            this.xtraTabPage2.Controls.Add(this.txtQnMCType);
            this.xtraTabPage2.Controls.Add(this.txtQcJob);
            this.xtraTabPage2.Controls.Add(this.lblJob);
            this.xtraTabPage2.Controls.Add(this.txtQcSect);
            this.xtraTabPage2.Controls.Add(this.lblSect);
            this.xtraTabPage2.Controls.Add(this.txtQcMCType);
            this.xtraTabPage2.Controls.Add(this.lblMCType);
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.lblName2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.lblName);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(775, 519);
            this.xtraTabPage2.Text = "General Detail";
            // 
            // grbDetail
            // 
            this.grbDetail.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbDetail.AppearanceCaption.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grbDetail.AppearanceCaption.Options.UseFont = true;
            this.grbDetail.AppearanceCaption.Options.UseForeColor = true;
            this.grbDetail.Controls.Add(this.txtInstall);
            this.grbDetail.Controls.Add(this.label12);
            this.grbDetail.Controls.Add(this.txtModel);
            this.grbDetail.Controls.Add(this.txtSerialNo);
            this.grbDetail.Controls.Add(this.label10);
            this.grbDetail.Controls.Add(this.label11);
            this.grbDetail.Location = new System.Drawing.Point(420, 182);
            this.grbDetail.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.grbDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbDetail.LookAndFeel.UseWindowsXPTheme = true;
            this.grbDetail.Name = "grbDetail";
            this.grbDetail.Size = new System.Drawing.Size(327, 143);
            this.grbDetail.TabIndex = 7;
            this.grbDetail.Text = "Detail";
            // 
            // txtInstall
            // 
            this.txtInstall.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtInstall.EnterMoveNextControl = true;
            this.txtInstall.Location = new System.Drawing.Point(129, 44);
            this.txtInstall.Name = "txtInstall";
            this.txtInstall.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtInstall.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtInstall.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtInstall.Properties.Appearance.Options.UseBackColor = true;
            this.txtInstall.Properties.Appearance.Options.UseFont = true;
            this.txtInstall.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtInstall.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtInstall.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtInstall.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtInstall.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtInstall.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtInstall.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtInstall.Properties.NullText = "N/A";
            this.txtInstall.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtInstall.Size = new System.Drawing.Size(135, 22);
            this.txtInstall.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label12.Location = new System.Drawing.Point(9, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 23);
            this.label12.TabIndex = 37;
            this.label12.Text = "Installation :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            this.txtModel.EnterMoveNextControl = true;
            this.txtModel.Location = new System.Drawing.Point(129, 97);
            this.txtModel.Name = "txtModel";
            this.txtModel.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtModel.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtModel.Properties.Appearance.Options.UseBackColor = true;
            this.txtModel.Properties.Appearance.Options.UseFont = true;
            this.txtModel.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtModel.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtModel.Size = new System.Drawing.Size(180, 22);
            this.txtModel.TabIndex = 2;
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.EnterMoveNextControl = true;
            this.txtSerialNo.Location = new System.Drawing.Point(129, 70);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSerialNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtSerialNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtSerialNo.Properties.Appearance.Options.UseFont = true;
            this.txtSerialNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtSerialNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtSerialNo.Size = new System.Drawing.Size(180, 22);
            this.txtSerialNo.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(9, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 23);
            this.label10.TabIndex = 37;
            this.label10.Text = "Model :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label11.Location = new System.Drawing.Point(9, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 23);
            this.label11.TabIndex = 37;
            this.label11.Text = "Serial No. :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grbCapacity
            // 
            this.grbCapacity.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbCapacity.AppearanceCaption.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grbCapacity.AppearanceCaption.Options.UseFont = true;
            this.grbCapacity.AppearanceCaption.Options.UseForeColor = true;
            this.grbCapacity.Controls.Add(this.txtUnderLoad);
            this.grbCapacity.Controls.Add(this.label8);
            this.grbCapacity.Controls.Add(this.txtOverLoad);
            this.grbCapacity.Controls.Add(this.label6);
            this.grbCapacity.Controls.Add(this.txtCapacity);
            this.grbCapacity.Controls.Add(this.label4);
            this.grbCapacity.Controls.Add(this.label9);
            this.grbCapacity.Location = new System.Drawing.Point(21, 182);
            this.grbCapacity.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.grbCapacity.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbCapacity.LookAndFeel.UseWindowsXPTheme = true;
            this.grbCapacity.Name = "grbCapacity";
            this.grbCapacity.Size = new System.Drawing.Size(393, 143);
            this.grbCapacity.TabIndex = 6;
            this.grbCapacity.Text = "Capacity";
            // 
            // txtUnderLoad
            // 
            this.txtUnderLoad.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtUnderLoad.EnterMoveNextControl = true;
            this.txtUnderLoad.Location = new System.Drawing.Point(176, 95);
            this.txtUnderLoad.Name = "txtUnderLoad";
            this.txtUnderLoad.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtUnderLoad.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtUnderLoad.Properties.Appearance.Options.UseBackColor = true;
            this.txtUnderLoad.Properties.Appearance.Options.UseFont = true;
            this.txtUnderLoad.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtUnderLoad.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtUnderLoad.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtUnderLoad.Properties.DisplayFormat.FormatString = "n2";
            this.txtUnderLoad.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUnderLoad.Properties.EditFormat.FormatString = "n2";
            this.txtUnderLoad.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUnderLoad.Size = new System.Drawing.Size(113, 22);
            this.txtUnderLoad.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(56, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 23);
            this.label8.TabIndex = 37;
            this.label8.Text = "% Under Load :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOverLoad
            // 
            this.txtOverLoad.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtOverLoad.EnterMoveNextControl = true;
            this.txtOverLoad.Location = new System.Drawing.Point(176, 69);
            this.txtOverLoad.Name = "txtOverLoad";
            this.txtOverLoad.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtOverLoad.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtOverLoad.Properties.Appearance.Options.UseBackColor = true;
            this.txtOverLoad.Properties.Appearance.Options.UseFont = true;
            this.txtOverLoad.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtOverLoad.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtOverLoad.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtOverLoad.Properties.DisplayFormat.FormatString = "n2";
            this.txtOverLoad.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtOverLoad.Properties.EditFormat.FormatString = "n2";
            this.txtOverLoad.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtOverLoad.Size = new System.Drawing.Size(113, 22);
            this.txtOverLoad.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(56, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 23);
            this.label6.TabIndex = 37;
            this.label6.Text = "% Over Load :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCapacity
            // 
            this.txtCapacity.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtCapacity.EnterMoveNextControl = true;
            this.txtCapacity.Location = new System.Drawing.Point(176, 43);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCapacity.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCapacity.Properties.Appearance.Options.UseBackColor = true;
            this.txtCapacity.Properties.Appearance.Options.UseFont = true;
            this.txtCapacity.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCapacity.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCapacity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCapacity.Properties.DisplayFormat.FormatString = "n2";
            this.txtCapacity.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCapacity.Properties.EditFormat.FormatString = "n2";
            this.txtCapacity.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCapacity.Size = new System.Drawing.Size(113, 22);
            this.txtCapacity.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(56, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 23);
            this.label4.TabIndex = 37;
            this.label4.Text = "Capacity :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(296, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 23);
            this.label9.TabIndex = 37;
            this.label9.Text = "Units/Hour";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnJob
            // 
            this.txtQnJob.Enabled = false;
            this.txtQnJob.EnterMoveNextControl = true;
            this.txtQnJob.Location = new System.Drawing.Point(383, 154);
            this.txtQnJob.Name = "txtQnJob";
            this.txtQnJob.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnJob.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnJob.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnJob.Properties.Appearance.Options.UseFont = true;
            this.txtQnJob.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnJob.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnJob.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnJob.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnJob.Size = new System.Drawing.Size(265, 22);
            this.txtQnJob.TabIndex = 35;
            // 
            // txtQnSect
            // 
            this.txtQnSect.Enabled = false;
            this.txtQnSect.EnterMoveNextControl = true;
            this.txtQnSect.Location = new System.Drawing.Point(383, 129);
            this.txtQnSect.Name = "txtQnSect";
            this.txtQnSect.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnSect.Properties.Appearance.Options.UseFont = true;
            this.txtQnSect.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnSect.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnSect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnSect.Size = new System.Drawing.Size(265, 22);
            this.txtQnSect.TabIndex = 35;
            // 
            // txtQnMCType
            // 
            this.txtQnMCType.Enabled = false;
            this.txtQnMCType.EnterMoveNextControl = true;
            this.txtQnMCType.Location = new System.Drawing.Point(383, 104);
            this.txtQnMCType.Name = "txtQnMCType";
            this.txtQnMCType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnMCType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnMCType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnMCType.Properties.Appearance.Options.UseFont = true;
            this.txtQnMCType.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtQnMCType.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtQnMCType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnMCType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnMCType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnMCType.Size = new System.Drawing.Size(265, 22);
            this.txtQnMCType.TabIndex = 35;
            // 
            // txtQcJob
            // 
            this.txtQcJob.EnterMoveNextControl = true;
            this.txtQcJob.Location = new System.Drawing.Point(197, 154);
            this.txtQcJob.Name = "txtQcJob";
            this.txtQcJob.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcJob.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcJob.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcJob.Properties.Appearance.Options.UseFont = true;
            this.txtQcJob.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcJob.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcJob.Size = new System.Drawing.Size(180, 22);
            this.txtQcJob.TabIndex = 5;
            // 
            // lblJob
            // 
            this.lblJob.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblJob.Location = new System.Drawing.Point(18, 154);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(178, 23);
            this.lblJob.TabIndex = 36;
            this.lblJob.Text = "โครงการ :";
            this.lblJob.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcSect
            // 
            this.txtQcSect.EnterMoveNextControl = true;
            this.txtQcSect.Location = new System.Drawing.Point(197, 129);
            this.txtQcSect.Name = "txtQcSect";
            this.txtQcSect.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcSect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcSect.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcSect.Properties.Appearance.Options.UseFont = true;
            this.txtQcSect.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcSect.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcSect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcSect.Size = new System.Drawing.Size(180, 22);
            this.txtQcSect.TabIndex = 4;
            // 
            // lblSect
            // 
            this.lblSect.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblSect.Location = new System.Drawing.Point(18, 129);
            this.lblSect.Name = "lblSect";
            this.lblSect.Size = new System.Drawing.Size(178, 23);
            this.lblSect.TabIndex = 36;
            this.lblSect.Text = "แผนก :";
            this.lblSect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQcMCType
            // 
            this.txtQcMCType.EnterMoveNextControl = true;
            this.txtQcMCType.Location = new System.Drawing.Point(197, 104);
            this.txtQcMCType.Name = "txtQcMCType";
            this.txtQcMCType.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcMCType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcMCType.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcMCType.Properties.Appearance.Options.UseFont = true;
            this.txtQcMCType.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcMCType.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcMCType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQcMCType.Size = new System.Drawing.Size(180, 22);
            this.txtQcMCType.TabIndex = 3;
            // 
            // lblMCType
            // 
            this.lblMCType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblMCType.Location = new System.Drawing.Point(18, 104);
            this.lblMCType.Name = "lblMCType";
            this.lblMCType.Size = new System.Drawing.Size(178, 23);
            this.lblMCType.TabIndex = 36;
            this.lblMCType.Text = "Machine Type :";
            this.lblMCType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(197, 79);
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
            this.lblName2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName2.Location = new System.Drawing.Point(27, 79);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(169, 23);
            this.lblName2.TabIndex = 0;
            this.lblName2.Text = "OPR. Name 2 :";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(197, 55);
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
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblName.Location = new System.Drawing.Point(24, 55);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(172, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "OPR. Name :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(197, 31);
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
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(21, 31);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(175, 23);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "OPR. Code :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage3.Controls.Add(this.grdTemCost);
            this.xtraTabPage3.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(775, 519);
            this.xtraTabPage3.TabPageWidth = 75;
            this.xtraTabPage3.Text = "Costing";
            // 
            // grdTemCost
            // 
            this.grdTemCost.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdTemCost.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdTemCost.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdTemCost.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdTemCost.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdTemCost.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "RowInsert"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 10, true, true, "", "RowDelete"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 8, true, true, "", "RowMoveUp"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 16, true, true, "", "RowMoveDown")});
            this.grdTemCost.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.grdTemPd_EmbeddedNavigator_ButtonClick);
            this.grdTemCost.Location = new System.Drawing.Point(11, 26);
            this.grdTemCost.LookAndFeel.SkinName = "Office 2007 Blue";
            this.grdTemCost.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.grdTemCost.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTemCost.MainView = this.gridView2;
            this.grdTemCost.Name = "grdTemCost";
            this.grdTemCost.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcCostBy,
            this.grcAmt});
            this.grdTemCost.Size = new System.Drawing.Size(636, 374);
            this.grdTemCost.TabIndex = 8;
            this.grdTemCost.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.FocusedCell.BackColor = System.Drawing.Color.Yellow;
            this.gridView2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView2.GridControl = this.grdTemCost;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
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
            // frmMFResource
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 566);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmMFResource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Resource Items";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbDetail)).EndInit();
            this.grbDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtInstall.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInstall.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerialNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbCapacity)).EndInit();
            this.grbCapacity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUnderLoad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverLoad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCapacity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnMCType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcSect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcMCType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTemCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcCostBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAmt)).EndInit();
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
        private DevExpress.XtraEditors.ButtonEdit txtQnJob;
        private DevExpress.XtraEditors.ButtonEdit txtQnSect;
        private DevExpress.XtraEditors.ButtonEdit txtQnMCType;
        private DevExpress.XtraEditors.ButtonEdit txtQcJob;
        private System.Windows.Forms.Label lblJob;
        private DevExpress.XtraEditors.ButtonEdit txtQcSect;
        private System.Windows.Forms.Label lblSect;
        private DevExpress.XtraEditors.ButtonEdit txtQcMCType;
        private System.Windows.Forms.Label lblMCType;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label lblName2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label lblName;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.GroupControl grbCapacity;
        private DevExpress.XtraEditors.SpinEdit txtCapacity;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SpinEdit txtUnderLoad;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.SpinEdit txtOverLoad;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.GroupControl grbDetail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.ButtonEdit txtModel;
        private DevExpress.XtraEditors.ButtonEdit txtSerialNo;
        private DevExpress.XtraEditors.DateEdit txtInstall;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraGrid.GridControl grdTemCost;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcCostBy;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit grcAmt;
    }
}