namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetXLS2
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDir = new DevExpress.XtraEditors.ButtonEdit();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.dlgSaveFile1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lsbOption = new DevExpress.XtraEditors.ListBoxControl();
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcIsOut = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grcApprove = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAPVStat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcIsPost = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.chkIsTagOrd = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grdBrowView2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(77, 528);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(9, 528);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&ตกลง";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "ระบุ Folder ที่ต้องการ Import :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDir
            // 
            this.txtDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDir.EnterMoveNextControl = true;
            this.txtDir.Location = new System.Drawing.Point(9, 35);
            this.txtDir.Name = "txtDir";
            this.txtDir.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtDir.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtDir.Properties.Appearance.Options.UseBackColor = true;
            this.txtDir.Properties.Appearance.Options.UseFont = true;
            this.txtDir.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtDir.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDir.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDir.Properties.ReadOnly = true;
            this.txtDir.Size = new System.Drawing.Size(710, 22);
            this.txtDir.TabIndex = 9;
            this.txtDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDir_ButtonClick);
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.DefaultExt = "*.XML";
            this.dlgSaveFile.FileName = "WBS.XML";
            this.dlgSaveFile.Filter = "Excel Files|*.XLS";
            // 
            // lsbOption
            // 
            this.lsbOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsbOption.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbOption.Appearance.Options.UseFont = true;
            this.lsbOption.Location = new System.Drawing.Point(9, 64);
            this.lsbOption.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.lsbOption.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lsbOption.Name = "lsbOption";
            this.lsbOption.Size = new System.Drawing.Size(267, 458);
            this.lsbOption.TabIndex = 27;
            this.lsbOption.SelectedIndexChanged += new System.EventHandler(this.lsbOption_SelectedIndexChanged);
            // 
            // grdBrowView
            // 
            this.grdBrowView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdBrowView.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdBrowView.Location = new System.Drawing.Point(287, 64);
            this.grdBrowView.MainView = this.gridView1;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcIsOut,
            this.grcApprove,
            this.grcAPVStat,
            this.grcIsPost,
            this.chkIsTagOrd});
            this.grdBrowView.Size = new System.Drawing.Size(432, 129);
            this.grdBrowView.TabIndex = 28;
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
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
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
            // chkIsTagOrd
            // 
            this.chkIsTagOrd.AutoHeight = false;
            this.chkIsTagOrd.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsTagOrd.Name = "chkIsTagOrd";
            // 
            // grdBrowView2
            // 
            this.grdBrowView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBrowView2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdBrowView2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdBrowView2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdBrowView2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdBrowView2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdBrowView2.Location = new System.Drawing.Point(287, 230);
            this.grdBrowView2.MainView = this.gridView2;
            this.grdBrowView2.Name = "grdBrowView2";
            this.grdBrowView2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemCheckEdit2,
            this.repositoryItemCheckEdit3});
            this.grdBrowView2.Size = new System.Drawing.Size(432, 294);
            this.grdBrowView2.TabIndex = 28;
            this.grdBrowView2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
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
            this.gridView2.GridControl = this.grdBrowView2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView2.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            " ",
            "/",
            "R"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            " ",
            "/",
            "R"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            this.repositoryItemComboBox2.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
            this.repositoryItemComboBox2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(287, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "รายการสินค้า";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgGetXLS2
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 569);
            this.ControlBox = false;
            this.Controls.Add(this.grdBrowView2);
            this.Controls.Add(this.grdBrowView);
            this.Controls.Add(this.lsbOption);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "dlgGetXLS2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Scaner File";
            ((System.ComponentModel.ISupportInitialize)(this.txtDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit txtDir;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
        private System.Windows.Forms.FolderBrowserDialog dlgSaveFile1;
        public DevExpress.XtraEditors.ListBoxControl lsbOption;
        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcApprove;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcAPVStat;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsPost;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkIsTagOrd;
        private DevExpress.XtraGrid.GridControl grdBrowView2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit3;
        private System.Windows.Forms.Label label2;
    }
}