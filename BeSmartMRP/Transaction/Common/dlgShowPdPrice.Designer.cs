namespace BeSmartMRP.Transaction.Common
{
    partial class dlgShowPdPrice
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtTotPdQty = new DevExpress.XtraEditors.SpinEdit();
            this.txtQcProd = new DevExpress.XtraEditors.ButtonEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotPdQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(224, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "C&ancel";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnOK.Location = new System.Drawing.Point(156, 143);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "&OK";
            // 
            // txtTotPdQty
            // 
            this.txtTotPdQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTotPdQty.Enabled = false;
            this.txtTotPdQty.EnterMoveNextControl = true;
            this.txtTotPdQty.Location = new System.Drawing.Point(156, 76);
            this.txtTotPdQty.Name = "txtTotPdQty";
            this.txtTotPdQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTotPdQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtTotPdQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotPdQty.Properties.Appearance.Options.UseFont = true;
            this.txtTotPdQty.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotPdQty.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotPdQty.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtTotPdQty.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotPdQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtTotPdQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtTotPdQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtTotPdQty.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtTotPdQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtTotPdQty.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtTotPdQty.Size = new System.Drawing.Size(178, 30);
            this.txtTotPdQty.TabIndex = 28;
            // 
            // txtQcProd
            // 
            this.txtQcProd.Location = new System.Drawing.Point(156, 37);
            this.txtQcProd.Name = "txtQcProd";
            this.txtQcProd.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQcProd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQcProd.Properties.Appearance.Options.UseBackColor = true;
            this.txtQcProd.Properties.Appearance.Options.UseFont = true;
            this.txtQcProd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQcProd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQcProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtQcProd.Size = new System.Drawing.Size(326, 30);
            this.txtQcProd.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(42, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 30;
            this.label2.Text = "จำนวน :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(42, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 23);
            this.label9.TabIndex = 29;
            this.label9.Text = "จำนวนสินค้า :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgShowPdPrice
            // 
            this.AcceptButton = this.btnOK;
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(593, 351);
            this.Controls.Add(this.txtTotPdQty);
            this.Controls.Add(this.txtQcProd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "dlgShowPdPrice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "แสดงราคาสินค้า";
            ((System.ComponentModel.ISupportInitialize)(this.txtTotPdQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcProd.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SpinEdit txtTotPdQty;
        private DevExpress.XtraEditors.ButtonEdit txtQcProd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
    }
}