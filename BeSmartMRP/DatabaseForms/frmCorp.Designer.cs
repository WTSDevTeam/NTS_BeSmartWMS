namespace BeSmartMRP.DatabaseForms
{
    partial class frmCorp
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
            frmCorp.pmClearInstanse();
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
            this.txtCalDate = new DevExpress.XtraEditors.DateEdit();
            this.txtTradeNo = new DevExpress.XtraEditors.ButtonEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.txtHolder = new DevExpress.XtraEditors.ButtonEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFax = new DevExpress.XtraEditors.ButtonEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTel = new DevExpress.XtraEditors.ButtonEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAddr22 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtAddr21 = new DevExpress.XtraEditors.ButtonEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName2 = new DevExpress.XtraEditors.ButtonEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddr12 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtAddr11 = new DevExpress.XtraEditors.ButtonEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtCalDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCalDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTradeNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr22.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr21.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr12.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr11.Properties)).BeginInit();
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
            this.pgfMainEdit.Size = new System.Drawing.Size(677, 448);
            this.pgfMainEdit.TabIndex = 8;
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
            this.xtraTabPage1.Size = new System.Drawing.Size(677, 424);
            this.xtraTabPage1.Text = "Browse";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.EnterMoveNextControl = true;
            this.txtFooter.Location = new System.Drawing.Point(10, 341);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtFooter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFooter.Properties.Appearance.Options.UseBackColor = true;
            this.txtFooter.Properties.Appearance.Options.UseFont = true;
            this.txtFooter.Properties.LookAndFeel.SkinName = "The Asphalt World";
            this.txtFooter.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtFooter.Properties.ReadOnly = true;
            this.txtFooter.Size = new System.Drawing.Size(658, 71);
            this.txtFooter.TabIndex = 1;
            this.txtFooter.TabStop = false;
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Name = "";
            this.grdBrowView.Location = new System.Drawing.Point(10, 6);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.Size = new System.Drawing.Size(658, 329);
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
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtCalDate);
            this.xtraTabPage2.Controls.Add(this.txtTradeNo);
            this.xtraTabPage2.Controls.Add(this.label11);
            this.xtraTabPage2.Controls.Add(this.txtHolder);
            this.xtraTabPage2.Controls.Add(this.label10);
            this.xtraTabPage2.Controls.Add(this.label9);
            this.xtraTabPage2.Controls.Add(this.txtFax);
            this.xtraTabPage2.Controls.Add(this.label8);
            this.xtraTabPage2.Controls.Add(this.txtTel);
            this.xtraTabPage2.Controls.Add(this.label7);
            this.xtraTabPage2.Controls.Add(this.txtAddr22);
            this.xtraTabPage2.Controls.Add(this.txtAddr21);
            this.xtraTabPage2.Controls.Add(this.label5);
            this.xtraTabPage2.Controls.Add(this.txtName2);
            this.xtraTabPage2.Controls.Add(this.label4);
            this.xtraTabPage2.Controls.Add(this.txtAddr12);
            this.xtraTabPage2.Controls.Add(this.txtAddr11);
            this.xtraTabPage2.Controls.Add(this.label2);
            this.xtraTabPage2.Controls.Add(this.txtName);
            this.xtraTabPage2.Controls.Add(this.label1);
            this.xtraTabPage2.Controls.Add(this.txtCode);
            this.xtraTabPage2.Controls.Add(this.lblCode);
            this.xtraTabPage2.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(677, 424);
            this.xtraTabPage2.Text = "รายละเอียด";
            // 
            // txtCalDate
            // 
            this.txtCalDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtCalDate.EnterMoveNextControl = true;
            this.txtCalDate.Location = new System.Drawing.Point(197, 253);
            this.txtCalDate.Name = "txtCalDate";
            this.txtCalDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtCalDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCalDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCalDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtCalDate.Properties.Appearance.Options.UseFont = true;
            this.txtCalDate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCalDate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCalDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtCalDate.Properties.DisplayFormat.FormatString = "dd/MM/yy";
            this.txtCalDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCalDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtCalDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCalDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtCalDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtCalDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.txtCalDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCalDate.Size = new System.Drawing.Size(137, 22);
            this.txtCalDate.TabIndex = 9;
            // 
            // txtTradeNo
            // 
            this.txtTradeNo.EnterMoveNextControl = true;
            this.txtTradeNo.Location = new System.Drawing.Point(197, 303);
            this.txtTradeNo.Name = "txtTradeNo";
            this.txtTradeNo.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTradeNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTradeNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtTradeNo.Properties.Appearance.Options.UseFont = true;
            this.txtTradeNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTradeNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTradeNo.Size = new System.Drawing.Size(236, 22);
            this.txtTradeNo.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label11.Location = new System.Drawing.Point(61, 303);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 23);
            this.label11.TabIndex = 0;
            this.label11.Text = "เลขทะเบียนการค้า :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHolder
            // 
            this.txtHolder.EnterMoveNextControl = true;
            this.txtHolder.Location = new System.Drawing.Point(197, 278);
            this.txtHolder.Name = "txtHolder";
            this.txtHolder.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtHolder.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtHolder.Properties.Appearance.Options.UseBackColor = true;
            this.txtHolder.Properties.Appearance.Options.UseFont = true;
            this.txtHolder.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtHolder.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtHolder.Size = new System.Drawing.Size(236, 22);
            this.txtHolder.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(61, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 23);
            this.label10.TabIndex = 0;
            this.label10.Text = "ชื่อผู้ประกอบการ :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(61, 256);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "วันที่เริ่มใช้ระบบ :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFax
            // 
            this.txtFax.EnterMoveNextControl = true;
            this.txtFax.Location = new System.Drawing.Point(197, 228);
            this.txtFax.Name = "txtFax";
            this.txtFax.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtFax.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtFax.Properties.Appearance.Options.UseBackColor = true;
            this.txtFax.Properties.Appearance.Options.UseFont = true;
            this.txtFax.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtFax.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtFax.Size = new System.Drawing.Size(236, 22);
            this.txtFax.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(61, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "เบอร์แฟกซ์ :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTel
            // 
            this.txtTel.EnterMoveNextControl = true;
            this.txtTel.Location = new System.Drawing.Point(197, 204);
            this.txtTel.Name = "txtTel";
            this.txtTel.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTel.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTel.Properties.Appearance.Options.UseBackColor = true;
            this.txtTel.Properties.Appearance.Options.UseFont = true;
            this.txtTel.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTel.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTel.Size = new System.Drawing.Size(236, 22);
            this.txtTel.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(61, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = "เบอร์โทร :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAddr22
            // 
            this.txtAddr22.EnterMoveNextControl = true;
            this.txtAddr22.Location = new System.Drawing.Point(197, 179);
            this.txtAddr22.Name = "txtAddr22";
            this.txtAddr22.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtAddr22.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtAddr22.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr22.Properties.Appearance.Options.UseFont = true;
            this.txtAddr22.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtAddr22.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAddr22.Size = new System.Drawing.Size(339, 22);
            this.txtAddr22.TabIndex = 6;
            // 
            // txtAddr21
            // 
            this.txtAddr21.EnterMoveNextControl = true;
            this.txtAddr21.Location = new System.Drawing.Point(197, 155);
            this.txtAddr21.Name = "txtAddr21";
            this.txtAddr21.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtAddr21.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtAddr21.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr21.Properties.Appearance.Options.UseFont = true;
            this.txtAddr21.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtAddr21.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAddr21.Size = new System.Drawing.Size(339, 22);
            this.txtAddr21.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(61, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "ที่อยู่ภาษาอังกฤษ :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName2
            // 
            this.txtName2.EnterMoveNextControl = true;
            this.txtName2.Location = new System.Drawing.Point(197, 130);
            this.txtName2.Name = "txtName2";
            this.txtName2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtName2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtName2.Properties.Appearance.Options.UseBackColor = true;
            this.txtName2.Properties.Appearance.Options.UseFont = true;
            this.txtName2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtName2.Size = new System.Drawing.Size(339, 22);
            this.txtName2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(61, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "ชื่อภาษาอังกฤษ :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAddr12
            // 
            this.txtAddr12.EnterMoveNextControl = true;
            this.txtAddr12.Location = new System.Drawing.Point(197, 104);
            this.txtAddr12.Name = "txtAddr12";
            this.txtAddr12.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtAddr12.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtAddr12.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr12.Properties.Appearance.Options.UseFont = true;
            this.txtAddr12.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtAddr12.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAddr12.Size = new System.Drawing.Size(339, 22);
            this.txtAddr12.TabIndex = 3;
            // 
            // txtAddr11
            // 
            this.txtAddr11.EnterMoveNextControl = true;
            this.txtAddr11.Location = new System.Drawing.Point(197, 79);
            this.txtAddr11.Name = "txtAddr11";
            this.txtAddr11.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtAddr11.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtAddr11.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr11.Properties.Appearance.Options.UseFont = true;
            this.txtAddr11.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtAddr11.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAddr11.Size = new System.Drawing.Size(339, 22);
            this.txtAddr11.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(61, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "ที่อยู่ภาษาไทย :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtName.Size = new System.Drawing.Size(339, 22);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(55, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ชื่อภาษาไทย :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtCode.Size = new System.Drawing.Size(137, 22);
            this.txtCode.TabIndex = 0;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblCode.Location = new System.Drawing.Point(21, 31);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(175, 23);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "รหัสบริษัท :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmCorp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 471);
            this.Controls.Add(this.pgfMainEdit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmCorp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "เพิ่ม/แก้ไขรายชื่อบริษัท";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgfMainEdit)).EndInit();
            this.pgfMainEdit.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFooter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCalDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCalDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTradeNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr22.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr21.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr12.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
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
        private DevExpress.XtraEditors.ButtonEdit txtAddr11;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtCode;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.ButtonEdit txtTradeNo;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.ButtonEdit txtHolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ButtonEdit txtFax;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ButtonEdit txtTel;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ButtonEdit txtAddr22;
        private DevExpress.XtraEditors.ButtonEdit txtAddr21;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ButtonEdit txtName2;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit txtAddr12;
        private DevExpress.XtraEditors.DateEdit txtCalDate;
    }
}