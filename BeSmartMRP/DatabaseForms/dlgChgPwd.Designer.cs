namespace BeSmartMRP.DatabaseForms
{
    partial class dlgChgPwd
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPwd2 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtPwd1 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtCurrPwd = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrPwd.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::BeSmartMRP.Properties.Resources.Exit01;
            this.btnCancel.Location = new System.Drawing.Point(253, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "ยกเลิก";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Image = global::BeSmartMRP.Properties.Resources.text_rich;
            this.btnOK.Location = new System.Drawing.Point(185, 133);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&ตกลง";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(29, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "รหัสผ่านใหม่-อีกครั้ง :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(29, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "รหัสผ่านใหม่ :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPwd2
            // 
            this.txtPwd2.EnterMoveNextControl = true;
            this.txtPwd2.Location = new System.Drawing.Point(185, 91);
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtPwd2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtPwd2.Properties.Appearance.Options.UseBackColor = true;
            this.txtPwd2.Properties.Appearance.Options.UseFont = true;
            this.txtPwd2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtPwd2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtPwd2.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPwd2.Properties.MaxLength = 10;
            this.txtPwd2.Properties.PasswordChar = '*';
            this.txtPwd2.Size = new System.Drawing.Size(140, 22);
            this.txtPwd2.TabIndex = 2;
            // 
            // txtPwd1
            // 
            this.txtPwd1.EnterMoveNextControl = true;
            this.txtPwd1.Location = new System.Drawing.Point(185, 63);
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtPwd1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtPwd1.Properties.Appearance.Options.UseBackColor = true;
            this.txtPwd1.Properties.Appearance.Options.UseFont = true;
            this.txtPwd1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtPwd1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtPwd1.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPwd1.Properties.MaxLength = 10;
            this.txtPwd1.Properties.PasswordChar = '*';
            this.txtPwd1.Size = new System.Drawing.Size(140, 22);
            this.txtPwd1.TabIndex = 1;
            // 
            // txtCurrPwd
            // 
            this.txtCurrPwd.EnterMoveNextControl = true;
            this.txtCurrPwd.Location = new System.Drawing.Point(185, 35);
            this.txtCurrPwd.Name = "txtCurrPwd";
            this.txtCurrPwd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtCurrPwd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtCurrPwd.Properties.Appearance.Options.UseBackColor = true;
            this.txtCurrPwd.Properties.Appearance.Options.UseFont = true;
            this.txtCurrPwd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtCurrPwd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtCurrPwd.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCurrPwd.Properties.MaxLength = 10;
            this.txtCurrPwd.Properties.PasswordChar = '*';
            this.txtCurrPwd.Size = new System.Drawing.Size(140, 22);
            this.txtCurrPwd.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(29, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "รหัสผ่านปัจจุบัน :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(446, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dlgChgPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(446, 261);
            this.ControlBox = false;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPwd2);
            this.Controls.Add(this.txtCurrPwd);
            this.Controls.Add(this.txtPwd1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgChgPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "แก้ไขรหัสผ่าน";
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrPwd.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit txtPwd2;
        private DevExpress.XtraEditors.ButtonEdit txtPwd1;
        private DevExpress.XtraEditors.ButtonEdit txtCurrPwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}