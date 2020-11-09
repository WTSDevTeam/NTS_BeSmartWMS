namespace BeSmartMRP.Permission
{
    partial class frmAuth_Add1
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
            this.txtAuth1 = new DevExpress.XtraEditors.ToggleSwitch();
            this.lblAuth1 = new System.Windows.Forms.Label();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuth1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(339, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 32);
            this.btnCancel.TabIndex = 93;
            this.btnCancel.Text = "C&ancel";
            // 
            // txtAuth1
            // 
            this.txtAuth1.Location = new System.Drawing.Point(260, 62);
            this.txtAuth1.Name = "txtAuth1";
            this.txtAuth1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtAuth1.Properties.OffText = "No";
            this.txtAuth1.Properties.OnText = "Yes";
            this.txtAuth1.Size = new System.Drawing.Size(98, 24);
            this.txtAuth1.TabIndex = 96;
            this.txtAuth1.TabStop = false;
            // 
            // lblAuth1
            // 
            this.lblAuth1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblAuth1.Location = new System.Drawing.Point(38, 59);
            this.lblAuth1.Name = "lblAuth1";
            this.lblAuth1.Size = new System.Drawing.Size(207, 30);
            this.lblAuth1.TabIndex = 95;
            this.lblAuth1.Text = "อนุญาติให้แก้วันที่ Transaction :";
            this.lblAuth1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnOK.Location = new System.Drawing.Point(260, 226);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(73, 32);
            this.btnOK.TabIndex = 92;
            this.btnOK.Text = "&OK";
            // 
            // frmAuth_Add1
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(582, 293);
            this.ControlBox = false;
            this.Controls.Add(this.txtAuth1);
            this.Controls.Add(this.lblAuth1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "frmAuth_Add1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "กำหนดสิทธิ์เพิ่มเติม";
            ((System.ComponentModel.ISupportInitialize)(this.txtAuth1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label lblAuth1;
        public DevExpress.XtraEditors.ToggleSwitch txtAuth1;
    }
}