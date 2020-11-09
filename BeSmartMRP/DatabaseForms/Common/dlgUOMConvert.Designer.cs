namespace BeSmartMRP.DatabaseForms.Common
{
    partial class dlgUOMConvert
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
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lblUnit1 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.txtUOMQty = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUOMQty.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(117, 95);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(184, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // lblUnit1
            // 
            this.lblUnit1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblUnit1.Location = new System.Drawing.Point(12, 58);
            this.lblUnit1.Name = "lblUnit1";
            this.lblUnit1.Size = new System.Drawing.Size(99, 23);
            this.lblUnit1.TabIndex = 34;
            this.lblUnit1.Text = "1 PCS = ";
            this.lblUnit1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblTitle.Location = new System.Drawing.Point(22, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(356, 23);
            this.lblTitle.TabIndex = 35;
            this.lblTitle.Text = "หน่วยนับมาตรฐาน = PCS";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnit2
            // 
            this.lblUnit2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblUnit2.Location = new System.Drawing.Point(236, 57);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(138, 23);
            this.lblUnit2.TabIndex = 36;
            this.lblUnit2.Text = "KGS";
            this.lblUnit2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUOMQty
            // 
            this.txtUOMQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtUOMQty.EnterMoveNextControl = true;
            this.txtUOMQty.Location = new System.Drawing.Point(117, 58);
            this.txtUOMQty.Name = "txtUOMQty";
            this.txtUOMQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtUOMQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtUOMQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtUOMQty.Properties.Appearance.Options.UseFont = true;
            this.txtUOMQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtUOMQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtUOMQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtUOMQty.Properties.DisplayFormat.FormatString = "n4";
            this.txtUOMQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUOMQty.Properties.EditFormat.FormatString = "n4";
            this.txtUOMQty.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUOMQty.Size = new System.Drawing.Size(113, 22);
            this.txtUOMQty.TabIndex = 0;
            // 
            // dlgUOMConvert
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(386, 155);
            this.Controls.Add(this.txtUOMQty);
            this.Controls.Add(this.lblUnit2);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUnit1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgUOMConvert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "อัตราแปลงหน่วย";
            ((System.ComponentModel.ISupportInitialize)(this.txtUOMQty.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Label lblUnit1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUnit2;
        private DevExpress.XtraEditors.SpinEdit txtUOMQty;
    }
}