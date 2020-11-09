namespace BeSmartMRP.Transaction.Common
{
    partial class dlgGetQty
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
            this.lblQty11 = new System.Windows.Forms.Label();
            this.txtQty = new DevExpress.XtraEditors.SpinEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.lblQty12 = new System.Windows.Forms.Label();
            this.txtQnUM = new DevExpress.XtraEditors.ButtonEdit();
            this.txtQnUM2 = new DevExpress.XtraEditors.ButtonEdit();
            this.lblQty13 = new System.Windows.Forms.Label();
            this.txtQnStdUM = new DevExpress.XtraEditors.ButtonEdit();
            this.txtUMQty = new DevExpress.XtraEditors.SpinEdit();
            this.pnlStQty = new System.Windows.Forms.Panel();
            this.lblQty21 = new System.Windows.Forms.Label();
            this.txtQnStdStUM = new DevExpress.XtraEditors.ButtonEdit();
            this.txtStQty = new DevExpress.XtraEditors.SpinEdit();
            this.txtQnStUM2 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtStUMQty = new DevExpress.XtraEditors.SpinEdit();
            this.txtQnStUM = new DevExpress.XtraEditors.ButtonEdit();
            this.lblQty22 = new System.Windows.Forms.Label();
            this.lblQty23 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnUM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnUM2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStdUM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUMQty.Properties)).BeginInit();
            this.pnlStQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStdStUM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStUM2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStUMQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStUM.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblQty11
            // 
            this.lblQty11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty11.Location = new System.Drawing.Point(14, 15);
            this.lblQty11.Name = "lblQty11";
            this.lblQty11.Size = new System.Drawing.Size(183, 23);
            this.lblQty11.TabIndex = 8;
            this.lblQty11.Text = "จำนวนหน่วยนับมาตรฐาน :";
            this.lblQty11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQty
            // 
            this.txtQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtQty.EnterMoveNextControl = true;
            this.txtQty.Location = new System.Drawing.Point(203, 15);
            this.txtQty.Name = "txtQty";
            this.txtQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtQty.Properties.Appearance.Options.UseFont = true;
            this.txtQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtQty.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtQty.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtQty.Size = new System.Drawing.Size(93, 22);
            this.txtQty.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 111);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(683, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(271, 74);
            this.btnCancel.LookAndFeel.SkinName = "Money Twins";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "C&ancel";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnOK.Location = new System.Drawing.Point(203, 74);
            this.btnOK.LookAndFeel.SkinName = "Money Twins";
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblQty12
            // 
            this.lblQty12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty12.Location = new System.Drawing.Point(377, 15);
            this.lblQty12.Name = "lblQty12";
            this.lblQty12.Size = new System.Drawing.Size(19, 23);
            this.lblQty12.TabIndex = 8;
            this.lblQty12.Text = "1";
            this.lblQty12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnUM
            // 
            this.txtQnUM.EnterMoveNextControl = true;
            this.txtQnUM.Location = new System.Drawing.Point(302, 15);
            this.txtQnUM.Name = "txtQnUM";
            this.txtQnUM.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnUM.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnUM.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnUM.Properties.Appearance.Options.UseFont = true;
            this.txtQnUM.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnUM.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnUM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQnUM.Size = new System.Drawing.Size(69, 22);
            this.txtQnUM.TabIndex = 1;
            this.txtQnUM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnUM.Validating += new System.ComponentModel.CancelEventHandler(this.txtQnUM_Validating);
            this.txtQnUM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // txtQnUM2
            // 
            this.txtQnUM2.Enabled = false;
            this.txtQnUM2.EnterMoveNextControl = true;
            this.txtQnUM2.Location = new System.Drawing.Point(402, 15);
            this.txtQnUM2.Name = "txtQnUM2";
            this.txtQnUM2.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnUM2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnUM2.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnUM2.Properties.Appearance.Options.UseFont = true;
            this.txtQnUM2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnUM2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnUM2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnUM2.Size = new System.Drawing.Size(58, 22);
            this.txtQnUM2.TabIndex = 101;
            // 
            // lblQty13
            // 
            this.lblQty13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty13.Location = new System.Drawing.Point(466, 15);
            this.lblQty13.Name = "lblQty13";
            this.lblQty13.Size = new System.Drawing.Size(52, 23);
            this.lblQty13.TabIndex = 8;
            this.lblQty13.Text = "เท่ากับ";
            this.lblQty13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQnStdUM
            // 
            this.txtQnStdUM.Enabled = false;
            this.txtQnStdUM.EnterMoveNextControl = true;
            this.txtQnStdUM.Location = new System.Drawing.Point(583, 15);
            this.txtQnStdUM.Name = "txtQnStdUM";
            this.txtQnStdUM.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnStdUM.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnStdUM.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnStdUM.Properties.Appearance.Options.UseFont = true;
            this.txtQnStdUM.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnStdUM.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnStdUM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnStdUM.Size = new System.Drawing.Size(58, 22);
            this.txtQnStdUM.TabIndex = 101;
            // 
            // txtUMQty
            // 
            this.txtUMQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtUMQty.EnterMoveNextControl = true;
            this.txtUMQty.Location = new System.Drawing.Point(513, 15);
            this.txtUMQty.Name = "txtUMQty";
            this.txtUMQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtUMQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtUMQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtUMQty.Properties.Appearance.Options.UseFont = true;
            this.txtUMQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtUMQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtUMQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtUMQty.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtUMQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUMQty.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtUMQty.Size = new System.Drawing.Size(64, 22);
            this.txtUMQty.TabIndex = 2;
            // 
            // pnlStQty
            // 
            this.pnlStQty.BackColor = System.Drawing.Color.Transparent;
            this.pnlStQty.Controls.Add(this.lblQty21);
            this.pnlStQty.Controls.Add(this.txtQnStdStUM);
            this.pnlStQty.Controls.Add(this.txtStQty);
            this.pnlStQty.Controls.Add(this.txtQnStUM2);
            this.pnlStQty.Controls.Add(this.txtStUMQty);
            this.pnlStQty.Controls.Add(this.txtQnStUM);
            this.pnlStQty.Controls.Add(this.lblQty22);
            this.pnlStQty.Controls.Add(this.lblQty23);
            this.pnlStQty.Location = new System.Drawing.Point(7, 39);
            this.pnlStQty.Name = "pnlStQty";
            this.pnlStQty.Size = new System.Drawing.Size(664, 24);
            this.pnlStQty.TabIndex = 3;
            // 
            // lblQty21
            // 
            this.lblQty21.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty21.Location = new System.Drawing.Point(7, 1);
            this.lblQty21.Name = "lblQty21";
            this.lblQty21.Size = new System.Drawing.Size(183, 23);
            this.lblQty21.TabIndex = 8;
            this.lblQty21.Text = "จำนวนหน่วยคุมสต็อค :";
            this.lblQty21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQnStdStUM
            // 
            this.txtQnStdStUM.Enabled = false;
            this.txtQnStdStUM.EnterMoveNextControl = true;
            this.txtQnStdStUM.Location = new System.Drawing.Point(576, 1);
            this.txtQnStdStUM.Name = "txtQnStdStUM";
            this.txtQnStdStUM.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnStdStUM.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnStdStUM.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnStdStUM.Properties.Appearance.Options.UseFont = true;
            this.txtQnStdStUM.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnStdStUM.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnStdStUM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnStdStUM.Size = new System.Drawing.Size(58, 22);
            this.txtQnStdStUM.TabIndex = 101;
            // 
            // txtStQty
            // 
            this.txtStQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtStQty.EnterMoveNextControl = true;
            this.txtStQty.Location = new System.Drawing.Point(196, 1);
            this.txtStQty.Name = "txtStQty";
            this.txtStQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtStQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtStQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtStQty.Properties.Appearance.Options.UseFont = true;
            this.txtStQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtStQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtStQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtStQty.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtStQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStQty.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtStQty.Size = new System.Drawing.Size(93, 22);
            this.txtStQty.TabIndex = 0;
            // 
            // txtQnStUM2
            // 
            this.txtQnStUM2.Enabled = false;
            this.txtQnStUM2.EnterMoveNextControl = true;
            this.txtQnStUM2.Location = new System.Drawing.Point(395, 1);
            this.txtQnStUM2.Name = "txtQnStUM2";
            this.txtQnStUM2.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txtQnStUM2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnStUM2.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnStUM2.Properties.Appearance.Options.UseFont = true;
            this.txtQnStUM2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnStUM2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnStUM2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", 12, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::BeSmartMRP.Properties.Resources.view)});
            this.txtQnStUM2.Size = new System.Drawing.Size(58, 22);
            this.txtQnStUM2.TabIndex = 101;
            // 
            // txtStUMQty
            // 
            this.txtStUMQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtStUMQty.EnterMoveNextControl = true;
            this.txtStUMQty.Location = new System.Drawing.Point(506, 1);
            this.txtStUMQty.Name = "txtStUMQty";
            this.txtStUMQty.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtStUMQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtStUMQty.Properties.Appearance.Options.UseBackColor = true;
            this.txtStUMQty.Properties.Appearance.Options.UseFont = true;
            this.txtStUMQty.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtStUMQty.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtStUMQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtStUMQty.Properties.DisplayFormat.FormatString = "#,##0.00";
            this.txtStUMQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStUMQty.Properties.Mask.EditMask = "#,###,###,##0.00";
            this.txtStUMQty.Size = new System.Drawing.Size(64, 22);
            this.txtStUMQty.TabIndex = 2;
            // 
            // txtQnStUM
            // 
            this.txtQnStUM.EnterMoveNextControl = true;
            this.txtQnStUM.Location = new System.Drawing.Point(295, 1);
            this.txtQnStUM.Name = "txtQnStUM";
            this.txtQnStUM.Properties.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtQnStUM.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtQnStUM.Properties.Appearance.Options.UseBackColor = true;
            this.txtQnStUM.Properties.Appearance.Options.UseFont = true;
            this.txtQnStUM.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.txtQnStUM.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtQnStUM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::BeSmartMRP.Properties.Resources.view, null)});
            this.txtQnStUM.Size = new System.Drawing.Size(69, 22);
            this.txtQnStUM.TabIndex = 1;
            this.txtQnStUM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtQnStUM.Validating += new System.ComponentModel.CancelEventHandler(this.txtQnStUM_Validating);
            this.txtQnStUM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);
            // 
            // lblQty22
            // 
            this.lblQty22.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty22.Location = new System.Drawing.Point(370, 1);
            this.lblQty22.Name = "lblQty22";
            this.lblQty22.Size = new System.Drawing.Size(19, 23);
            this.lblQty22.TabIndex = 8;
            this.lblQty22.Text = "1";
            this.lblQty22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblQty23
            // 
            this.lblQty23.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblQty23.Location = new System.Drawing.Point(459, 1);
            this.lblQty23.Name = "lblQty23";
            this.lblQty23.Size = new System.Drawing.Size(52, 23);
            this.lblQty23.TabIndex = 8;
            this.lblQty23.Text = "เท่ากับ";
            this.lblQty23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgGetQty
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(683, 133);
            this.ControlBox = false;
            this.Controls.Add(this.pnlStQty);
            this.Controls.Add(this.txtQnStdUM);
            this.Controls.Add(this.txtQnUM2);
            this.Controls.Add(this.txtQnUM);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblQty12);
            this.Controls.Add(this.lblQty11);
            this.Controls.Add(this.txtUMQty);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.lblQty13);
            this.Name = "dlgGetQty";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ระบุจำนวนสินค้า และ หน่วยนับ";
            ((System.ComponentModel.ISupportInitialize)(this.txtQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnUM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnUM2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStdUM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUMQty.Properties)).EndInit();
            this.pnlStQty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStdStUM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStUM2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStUMQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQnStUM.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQty11;
        private DevExpress.XtraEditors.SpinEdit txtQty;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.Label lblQty12;
        private DevExpress.XtraEditors.ButtonEdit txtQnUM;
        private DevExpress.XtraEditors.ButtonEdit txtQnUM2;
        private System.Windows.Forms.Label lblQty13;
        private DevExpress.XtraEditors.ButtonEdit txtQnStdUM;
        private DevExpress.XtraEditors.SpinEdit txtUMQty;
        private System.Windows.Forms.Panel pnlStQty;
        private System.Windows.Forms.Label lblQty21;
        private DevExpress.XtraEditors.ButtonEdit txtQnStdStUM;
        private DevExpress.XtraEditors.SpinEdit txtStQty;
        private DevExpress.XtraEditors.ButtonEdit txtQnStUM2;
        private DevExpress.XtraEditors.SpinEdit txtStUMQty;
        private DevExpress.XtraEditors.ButtonEdit txtQnStUM;
        private System.Windows.Forms.Label lblQty22;
        private System.Windows.Forms.Label lblQty23;
    }
}