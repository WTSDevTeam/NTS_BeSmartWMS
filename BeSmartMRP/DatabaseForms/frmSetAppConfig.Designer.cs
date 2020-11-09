namespace BeSmartMRP.DatabaseForms
{
    partial class frmSetAppConfig
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
            this.barMainEdit = new DevExpress.XtraBars.BarManager(this.components);
            this.barMain = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.cmbRound = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblRound = new System.Windows.Forms.Label();
            this.lblScrap = new System.Windows.Forms.Label();
            this.cmbCalScrap = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRound.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalScrap.Properties)).BeginInit();
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
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.UseWholeRow = true;
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
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.xtraTabControl1.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 23);
            this.xtraTabControl1.LookAndFeel.SkinName = "Office 2007 Blue";
            this.xtraTabControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(685, 423);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xtraTabPage1.Controls.Add(this.cmbCalScrap);
            this.xtraTabPage1.Controls.Add(this.lblScrap);
            this.xtraTabPage1.Controls.Add(this.cmbRound);
            this.xtraTabPage1.Controls.Add(this.lblRound);
            this.xtraTabPage1.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(607, 414);
            this.xtraTabPage1.Text = "General";
            // 
            // cmbRound
            // 
            this.cmbRound.EnterMoveNextControl = true;
            this.cmbRound.Location = new System.Drawing.Point(225, 77);
            this.cmbRound.Name = "cmbRound";
            this.cmbRound.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbRound.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbRound.Properties.Appearance.Options.UseBackColor = true;
            this.cmbRound.Properties.Appearance.Options.UseFont = true;
            this.cmbRound.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbRound.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.cmbRound.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbRound.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbRound.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbRound.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbRound.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbRound.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbRound.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.cmbRound.Properties.PopupSizeable = true;
            this.cmbRound.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbRound.Size = new System.Drawing.Size(216, 22);
            this.cmbRound.TabIndex = 1;
            // 
            // lblRound
            // 
            this.lblRound.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblRound.Location = new System.Drawing.Point(23, 76);
            this.lblRound.Name = "lblRound";
            this.lblRound.Size = new System.Drawing.Size(196, 23);
            this.lblRound.TabIndex = 17;
            this.lblRound.Text = "การปัดเศษ :";
            this.lblRound.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScrap
            // 
            this.lblScrap.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblScrap.Location = new System.Drawing.Point(23, 48);
            this.lblScrap.Name = "lblScrap";
            this.lblScrap.Size = new System.Drawing.Size(196, 23);
            this.lblScrap.TabIndex = 17;
            this.lblScrap.Text = "การคำนวณ Scrap :";
            this.lblScrap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCalScrap
            // 
            this.cmbCalScrap.EnterMoveNextControl = true;
            this.cmbCalScrap.Location = new System.Drawing.Point(225, 49);
            this.cmbCalScrap.Name = "cmbCalScrap";
            this.cmbCalScrap.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbCalScrap.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbCalScrap.Properties.Appearance.Options.UseBackColor = true;
            this.cmbCalScrap.Properties.Appearance.Options.UseFont = true;
            this.cmbCalScrap.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbCalScrap.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.cmbCalScrap.Properties.AppearanceDropDown.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbCalScrap.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmbCalScrap.Properties.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbCalScrap.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbCalScrap.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.cmbCalScrap.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmbCalScrap.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.cmbCalScrap.Properties.PopupSizeable = true;
            this.cmbCalScrap.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbCalScrap.Size = new System.Drawing.Size(216, 22);
            this.cmbCalScrap.TabIndex = 0;
            // 
            // frmSetAppConfig
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 467);
            this.ControlBox = false;
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmSetAppConfig";
            this.Text = "Application Preferences";
            ((System.ComponentModel.ISupportInitialize)(this.barMainEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbRound.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalScrap.Properties)).EndInit();
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
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbRound;
        private System.Windows.Forms.Label lblRound;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCalScrap;
        private System.Windows.Forms.Label lblScrap;
    }
}