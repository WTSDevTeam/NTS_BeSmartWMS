namespace mBudget.Report.GenReport
{
    partial class GENSALESUM
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
            this.txtEndDate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.txtBegDate = new DevExpress.XtraEditors.DateEdit();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(137, 118);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtEndDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndDate.Properties.Appearance.Options.UseFont = true;
            this.txtEndDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtEndDate.Properties.DisplayFormat.FormatString = "MM/yy";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Size = new System.Drawing.Size(125, 22);
            this.txtEndDate.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(-3, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 23);
            this.label3.TabIndex = 43;
            this.label3.Text = "จนถึงเดือน/ปี :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label88.Location = new System.Drawing.Point(12, 91);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(119, 23);
            this.label88.TabIndex = 42;
            this.label88.Text = "ตั้งแต่เดือน/ปี :";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBegDate
            // 
            this.txtBegDate.EditValue = new System.DateTime(2007, 11, 16, 0, 0, 0, 0);
            this.txtBegDate.EnterMoveNextControl = true;
            this.txtBegDate.Location = new System.Drawing.Point(137, 91);
            this.txtBegDate.Name = "txtBegDate";
            this.txtBegDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtBegDate.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegDate.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegDate.Properties.Appearance.Options.UseFont = true;
            this.txtBegDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtBegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 17, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.txtBegDate.Properties.DisplayFormat.FormatString = "MM/yy";
            this.txtBegDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtBegDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtBegDate.Properties.Mask.EditMask = "MM/yyyy";
            this.txtBegDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtBegDate.Size = new System.Drawing.Size(125, 22);
            this.txtBegDate.TabIndex = 37;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 200);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(558, 194);
            this.dataGridView1.TabIndex = 44;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.Visible = false;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            this.defaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::mBudget.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(205, 157);
            this.btnCancel.LookAndFeel.SkinName = "Money Twins";
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.LookAndFeel.UseWindowsXPTheme = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.Image = global::mBudget.Properties.Resources.Print2;
            this.btnPrint.Location = new System.Drawing.Point(137, 157);
            this.btnPrint.LookAndFeel.SkinName = "Money Twins";
            this.btnPrint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.btnPrint.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPrint.LookAndFeel.UseWindowsXPTheme = false;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 39;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("JasmineUPC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(71, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(310, 31);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "คำนวณยอดขาย";
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.lblTaskName);
            this.panelControl1.Location = new System.Drawing.Point(-2, 46);
            this.panelControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.LookAndFeel.UseWindowsXPTheme = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(589, 24);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Text = "panelControl1";
            // 
            // lblTaskName
            // 
            this.lblTaskName.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTaskName.Location = new System.Drawing.Point(79, 3);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(310, 20);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "รหัสรายงาน : GENSALESUM01";
            // 
            // pnlTitle
            // 
            this.pnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnlTitle.Appearance.BackColor2 = System.Drawing.Color.LightSkyBlue;
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.panelControl1);
            this.pnlTitle.Controls.Add(this.pictureBox1);
            this.pnlTitle.Location = new System.Drawing.Point(-3, -2);
            this.pnlTitle.LookAndFeel.SkinName = "Liquid Sky";
            this.pnlTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlTitle.LookAndFeel.UseWindowsXPTheme = false;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(589, 73);
            this.pnlTitle.TabIndex = 36;
            this.pnlTitle.Text = "pnlTitle";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::mBudget.Properties.Resources.chart;
            this.pictureBox1.Location = new System.Drawing.Point(15, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 46);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // GENSALESUM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 405);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label88);
            this.Controls.Add(this.txtBegDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.pnlTitle);
            this.Name = "GENSALESUM";
            this.Text = "GENSALESUM";
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label88;
        private DevExpress.XtraEditors.DateEdit txtBegDate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTaskName;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}