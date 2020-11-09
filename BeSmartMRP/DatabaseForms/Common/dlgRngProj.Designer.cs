namespace BeSmartMRP.DatabaseForms.Common
{
    partial class dlgRngProj
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndQcProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtBegQcProj1 = new DevExpress.XtraEditors.ButtonEdit();
            this.chkIsAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(42, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 23);
            this.label1.TabIndex = 88;
            this.label1.Text = "ถึง :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrint
            // 
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPrint.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnPrint.Location = new System.Drawing.Point(109, 122);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(64, 27);
            this.btnPrint.TabIndex = 89;
            this.btnPrint.Text = "&ตกลง";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(42, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 90;
            this.label2.Text = "ตั้งแต่ :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndQcProj1
            // 
            this.txtEndQcProj1.EnterMoveNextControl = true;
            this.txtEndQcProj1.Location = new System.Drawing.Point(109, 54);
            this.txtEndQcProj1.Name = "txtEndQcProj1";
            this.txtEndQcProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtEndQcProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtEndQcProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtEndQcProj1.Properties.Appearance.Options.UseFont = true;
            this.txtEndQcProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtEndQcProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEndQcProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtEndQcProj1.Size = new System.Drawing.Size(196, 22);
            this.txtEndQcProj1.TabIndex = 87;
            this.txtEndQcProj1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtEndQcProj1.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj1_Validating);
            this.txtEndQcProj1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(177, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 27);
            this.btnCancel.TabIndex = 91;
            this.btnCancel.Text = "ย&กเลิก";
            // 
            // txtBegQcProj1
            // 
            this.txtBegQcProj1.EnterMoveNextControl = true;
            this.txtBegQcProj1.Location = new System.Drawing.Point(109, 30);
            this.txtBegQcProj1.Name = "txtBegQcProj1";
            this.txtBegQcProj1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtBegQcProj1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtBegQcProj1.Properties.Appearance.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.Appearance.Options.UseFont = true;
            this.txtBegQcProj1.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBegQcProj1.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtBegQcProj1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBegQcProj1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, global::BeSmartMRP.Properties.Resources.view)});
            this.txtBegQcProj1.Size = new System.Drawing.Size(196, 22);
            this.txtBegQcProj1.TabIndex = 86;
            this.txtBegQcProj1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtBegQcProj1.Validating += new System.ComponentModel.CancelEventHandler(this.txtQcProj1_Validating);
            this.txtBegQcProj1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // chkIsAll
            // 
            this.chkIsAll.Location = new System.Drawing.Point(107, 82);
            this.chkIsAll.Name = "chkIsAll";
            this.chkIsAll.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.chkIsAll.Properties.Appearance.Options.UseFont = true;
            this.chkIsAll.Properties.Caption = "พิมพ์ทุกแผนก ?";
            this.chkIsAll.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.chkIsAll.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.chkIsAll.Size = new System.Drawing.Size(119, 22);
            this.chkIsAll.TabIndex = 92;
            this.chkIsAll.Visible = false;
            // 
            // dlgRngProj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(337, 179);
            this.ControlBox = false;
            this.Controls.Add(this.chkIsAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEndQcProj1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtBegQcProj1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgRngProj";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ระบุช่วงโครงการ";
            ((System.ComponentModel.ISupportInitialize)(this.txtEndQcProj1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBegQcProj1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAll.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtEndQcProj1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.ButtonEdit txtBegQcProj1;
        private DevExpress.XtraEditors.CheckEdit chkIsAll;
    }
}