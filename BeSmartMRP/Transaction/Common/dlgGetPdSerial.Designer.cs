namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetPdSerial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgGetPdSerial));
            this.grdBrowView = new DevExpress.XtraGrid.GridControl();
            this.grdTemPdSer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcIsOut = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grcApprove = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcAPVStat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.grcIsPost = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.chkIsTagOrd = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPdSer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).BeginInit();
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
            this.grdBrowView.Location = new System.Drawing.Point(12, 12);
            this.grdBrowView.MainView = this.grdTemPdSer;
            this.grdBrowView.Name = "grdBrowView";
            this.grdBrowView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grcIsOut,
            this.grcApprove,
            this.grcAPVStat,
            this.grcIsPost,
            this.chkIsTagOrd});
            this.grdBrowView.Size = new System.Drawing.Size(576, 441);
            this.grdBrowView.TabIndex = 95;
            this.grdBrowView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdTemPdSer});
            // 
            // grdTemPdSer
            // 
            this.grdTemPdSer.Appearance.FocusedCell.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.grdTemPdSer.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.Yellow;
            this.grdTemPdSer.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.grdTemPdSer.Appearance.FocusedCell.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdTemPdSer.Appearance.FocusedCell.Options.UseBackColor = true;
            this.grdTemPdSer.Appearance.FocusedCell.Options.UseFont = true;
            this.grdTemPdSer.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.grdTemPdSer.Appearance.HeaderPanel.Options.UseFont = true;
            this.grdTemPdSer.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.grdTemPdSer.GridControl = this.grdBrowView;
            this.grdTemPdSer.Name = "grdTemPdSer";
            this.grdTemPdSer.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdTemPdSer.OptionsCustomization.AllowFilter = false;
            this.grdTemPdSer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.grdTemPdSer.OptionsView.ShowGroupPanel = false;
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancel.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseBackColor = true;
            this.btnCancel.Appearance.Options.UseBorderColor = true;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(116, 459);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 40);
            this.btnCancel.TabIndex = 97;
            this.btnCancel.Text = "ยกเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnOK.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseBackColor = true;
            this.btnOK.Appearance.Options.UseBorderColor = true;
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnOK.Location = new System.Drawing.Point(12, 459);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(98, 40);
            this.btnOK.TabIndex = 96;
            this.btnOK.Text = "ตกลง";
            // 
            // dlgGetPdSerial
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(600, 512);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdBrowView);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("dlgGetPdSerial.IconOptions.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgGetPdSerial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "บันทึกรายละเอียด Serial #";
            ((System.ComponentModel.ISupportInitialize)(this.grdBrowView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemPdSer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcApprove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcAPVStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcIsPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTagOrd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private DevExpress.XtraGrid.GridControl grdBrowView;
        private DevExpress.XtraGrid.Views.Grid.GridView grdTemPdSer;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsOut;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcApprove;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox grcAPVStat;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit grcIsPost;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkIsTagOrd;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}