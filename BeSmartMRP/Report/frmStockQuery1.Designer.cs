namespace BeSmartMRP.Report
{
    partial class frmStockQuery1
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
            DevExpress.XtraPivotGrid.PivotGridGroup pivotGridGroup1 = new DevExpress.XtraPivotGrid.PivotGridGroup();
            this.pivotGridControl1 = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.dtspstk01 = new BeSmartMRP.Report.LocalDataSet.DTSPSTK01();
            this.fieldCIOTYPE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCLOT = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCFRWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCPDGRP = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCPDTYPE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCPROD = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCSECT = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCSUBCOOR = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCTOWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQCWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQN2SUBCOOR = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNFRWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNPDGRP = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNPDTYPE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNPROD = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNSECT = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNSUBCOOR = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNTOWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNUM = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCQNWHOUSE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCREFNO = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCSERIALNO = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldCWHTYPE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldDDATE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldNQTY = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldNQTYIN = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldNQTYOUT = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldQCWHLOCA = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldQNWHLOCA = new DevExpress.XtraPivotGrid.PivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtspstk01)).BeginInit();
            this.SuspendLayout();
            // 
            // pivotGridControl1
            // 
            this.pivotGridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pivotGridControl1.DataMember = "XRPSTMOVE";
            this.pivotGridControl1.DataSource = this.dtspstk01;
            this.pivotGridControl1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.fieldCIOTYPE,
            this.fieldCLOT,
            this.fieldCQCFRWHOUSE,
            this.fieldCQCPDGRP,
            this.fieldCQCPDTYPE,
            this.fieldCQCPROD,
            this.fieldCQCSECT,
            this.fieldCQCSUBCOOR,
            this.fieldCQCTOWHOUSE,
            this.fieldCQCWHOUSE,
            this.fieldCQN2SUBCOOR,
            this.fieldCQNFRWHOUSE,
            this.fieldCQNPDGRP,
            this.fieldCQNPDTYPE,
            this.fieldCQNPROD,
            this.fieldCQNSECT,
            this.fieldCQNSUBCOOR,
            this.fieldCQNTOWHOUSE,
            this.fieldCQNUM,
            this.fieldCQNWHOUSE,
            this.fieldCREFNO,
            this.fieldCSERIALNO,
            this.fieldCWHTYPE,
            this.fieldDDATE,
            this.fieldNQTY,
            this.fieldNQTYIN,
            this.fieldNQTYOUT,
            this.fieldQCWHLOCA,
            this.fieldQNWHLOCA});
            pivotGridGroup1.Fields.Add(this.fieldCQCPROD);
            pivotGridGroup1.Fields.Add(this.fieldCQNPROD);
            pivotGridGroup1.Hierarchy = null;
            pivotGridGroup1.ShowNewValues = true;
            this.pivotGridControl1.Groups.AddRange(new DevExpress.XtraPivotGrid.PivotGridGroup[] {
            pivotGridGroup1});
            this.pivotGridControl1.Location = new System.Drawing.Point(12, 61);
            this.pivotGridControl1.Name = "pivotGridControl1";
            this.pivotGridControl1.Size = new System.Drawing.Size(741, 281);
            this.pivotGridControl1.TabIndex = 0;
            // 
            // dtspstk01
            // 
            this.dtspstk01.DataSetName = "DTSPSTK01";
            this.dtspstk01.Locale = new System.Globalization.CultureInfo("en-US");
            this.dtspstk01.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fieldCIOTYPE
            // 
            this.fieldCIOTYPE.AreaIndex = 28;
            this.fieldCIOTYPE.Caption = "IO";
            this.fieldCIOTYPE.FieldName = "CIOTYPE";
            this.fieldCIOTYPE.Name = "fieldCIOTYPE";
            // 
            // fieldCLOT
            // 
            this.fieldCLOT.AreaIndex = 0;
            this.fieldCLOT.Caption = "Lot";
            this.fieldCLOT.FieldName = "CLOT";
            this.fieldCLOT.Name = "fieldCLOT";
            // 
            // fieldCQCFRWHOUSE
            // 
            this.fieldCQCFRWHOUSE.AreaIndex = 1;
            this.fieldCQCFRWHOUSE.Caption = "รหัสคลังสินค้าต้นทาง";
            this.fieldCQCFRWHOUSE.FieldName = "CQCFRWHOUSE";
            this.fieldCQCFRWHOUSE.Name = "fieldCQCFRWHOUSE";
            // 
            // fieldCQCPDGRP
            // 
            this.fieldCQCPDGRP.AreaIndex = 2;
            this.fieldCQCPDGRP.Caption = "รหัสกลุ่มสินค้า";
            this.fieldCQCPDGRP.FieldName = "CQCPDGRP";
            this.fieldCQCPDGRP.Name = "fieldCQCPDGRP";
            // 
            // fieldCQCPDTYPE
            // 
            this.fieldCQCPDTYPE.AreaIndex = 3;
            this.fieldCQCPDTYPE.Caption = "รหัสชนิดสินค้า";
            this.fieldCQCPDTYPE.FieldName = "CQCPDTYPE";
            this.fieldCQCPDTYPE.Name = "fieldCQCPDTYPE";
            // 
            // fieldCQCPROD
            // 
            this.fieldCQCPROD.AreaIndex = 4;
            this.fieldCQCPROD.Caption = "รหัสสินค้า";
            this.fieldCQCPROD.FieldName = "CQCPROD";
            this.fieldCQCPROD.Name = "fieldCQCPROD";
            // 
            // fieldCQCSECT
            // 
            this.fieldCQCSECT.AreaIndex = 6;
            this.fieldCQCSECT.Caption = "รหัสแผนก";
            this.fieldCQCSECT.FieldName = "CQCSECT";
            this.fieldCQCSECT.Name = "fieldCQCSECT";
            // 
            // fieldCQCSUBCOOR
            // 
            this.fieldCQCSUBCOOR.AreaIndex = 7;
            this.fieldCQCSUBCOOR.FieldName = "CQCSUBCOOR";
            this.fieldCQCSUBCOOR.Name = "fieldCQCSUBCOOR";
            // 
            // fieldCQCTOWHOUSE
            // 
            this.fieldCQCTOWHOUSE.AreaIndex = 8;
            this.fieldCQCTOWHOUSE.Caption = "รหัสคลังปลายทาง";
            this.fieldCQCTOWHOUSE.FieldName = "CQCTOWHOUSE";
            this.fieldCQCTOWHOUSE.Name = "fieldCQCTOWHOUSE";
            // 
            // fieldCQCWHOUSE
            // 
            this.fieldCQCWHOUSE.AreaIndex = 9;
            this.fieldCQCWHOUSE.Caption = "รหัสคลังสินค้า";
            this.fieldCQCWHOUSE.FieldName = "CQCWHOUSE";
            this.fieldCQCWHOUSE.Name = "fieldCQCWHOUSE";
            // 
            // fieldCQN2SUBCOOR
            // 
            this.fieldCQN2SUBCOOR.AreaIndex = 10;
            this.fieldCQN2SUBCOOR.FieldName = "CQN2SUBCOOR";
            this.fieldCQN2SUBCOOR.Name = "fieldCQN2SUBCOOR";
            // 
            // fieldCQNFRWHOUSE
            // 
            this.fieldCQNFRWHOUSE.AreaIndex = 11;
            this.fieldCQNFRWHOUSE.Caption = "ชื่อคลังสินค้าต้นทาง";
            this.fieldCQNFRWHOUSE.FieldName = "CQNFRWHOUSE";
            this.fieldCQNFRWHOUSE.Name = "fieldCQNFRWHOUSE";
            // 
            // fieldCQNPDGRP
            // 
            this.fieldCQNPDGRP.AreaIndex = 12;
            this.fieldCQNPDGRP.Caption = "ชื่อกลุ่มสินค้า";
            this.fieldCQNPDGRP.FieldName = "CQNPDGRP";
            this.fieldCQNPDGRP.Name = "fieldCQNPDGRP";
            // 
            // fieldCQNPDTYPE
            // 
            this.fieldCQNPDTYPE.AreaIndex = 13;
            this.fieldCQNPDTYPE.Caption = "ชื่อประเภทสินค้า";
            this.fieldCQNPDTYPE.FieldName = "CQNPDTYPE";
            this.fieldCQNPDTYPE.Name = "fieldCQNPDTYPE";
            // 
            // fieldCQNPROD
            // 
            this.fieldCQNPROD.AreaIndex = 5;
            this.fieldCQNPROD.Caption = "ชื่อสินค้า";
            this.fieldCQNPROD.FieldName = "CQNPROD";
            this.fieldCQNPROD.Name = "fieldCQNPROD";
            // 
            // fieldCQNSECT
            // 
            this.fieldCQNSECT.AreaIndex = 14;
            this.fieldCQNSECT.Caption = "ชื่อแผนก";
            this.fieldCQNSECT.FieldName = "CQNSECT";
            this.fieldCQNSECT.Name = "fieldCQNSECT";
            // 
            // fieldCQNSUBCOOR
            // 
            this.fieldCQNSUBCOOR.AreaIndex = 15;
            this.fieldCQNSUBCOOR.FieldName = "CQNSUBCOOR";
            this.fieldCQNSUBCOOR.Name = "fieldCQNSUBCOOR";
            // 
            // fieldCQNTOWHOUSE
            // 
            this.fieldCQNTOWHOUSE.AreaIndex = 16;
            this.fieldCQNTOWHOUSE.Caption = "ชื่อคลังสินค้าปลายทาง";
            this.fieldCQNTOWHOUSE.FieldName = "CQNTOWHOUSE";
            this.fieldCQNTOWHOUSE.Name = "fieldCQNTOWHOUSE";
            // 
            // fieldCQNUM
            // 
            this.fieldCQNUM.AreaIndex = 17;
            this.fieldCQNUM.Caption = "ชื่อหน่วยนับ";
            this.fieldCQNUM.FieldName = "CQNUM";
            this.fieldCQNUM.Name = "fieldCQNUM";
            // 
            // fieldCQNWHOUSE
            // 
            this.fieldCQNWHOUSE.AreaIndex = 18;
            this.fieldCQNWHOUSE.Caption = "ชื่อคลังสินค้า";
            this.fieldCQNWHOUSE.FieldName = "CQNWHOUSE";
            this.fieldCQNWHOUSE.Name = "fieldCQNWHOUSE";
            // 
            // fieldCREFNO
            // 
            this.fieldCREFNO.AreaIndex = 19;
            this.fieldCREFNO.Caption = "เลขที่อ้างอิง";
            this.fieldCREFNO.FieldName = "CREFNO";
            this.fieldCREFNO.Name = "fieldCREFNO";
            // 
            // fieldCSERIALNO
            // 
            this.fieldCSERIALNO.AreaIndex = 20;
            this.fieldCSERIALNO.Caption = "SERIAL NO.";
            this.fieldCSERIALNO.FieldName = "CSERIALNO";
            this.fieldCSERIALNO.Name = "fieldCSERIALNO";
            // 
            // fieldCWHTYPE
            // 
            this.fieldCWHTYPE.AreaIndex = 21;
            this.fieldCWHTYPE.Caption = "ประเภทคลังสินค้า";
            this.fieldCWHTYPE.FieldName = "CWHTYPE";
            this.fieldCWHTYPE.Name = "fieldCWHTYPE";
            // 
            // fieldDDATE
            // 
            this.fieldDDATE.AreaIndex = 22;
            this.fieldDDATE.Caption = "วันที่เอกสาร";
            this.fieldDDATE.FieldName = "DDATE";
            this.fieldDDATE.Name = "fieldDDATE";
            // 
            // fieldNQTY
            // 
            this.fieldNQTY.AreaIndex = 23;
            this.fieldNQTY.Caption = "จำนวน";
            this.fieldNQTY.FieldName = "NQTY";
            this.fieldNQTY.Name = "fieldNQTY";
            // 
            // fieldNQTYIN
            // 
            this.fieldNQTYIN.AreaIndex = 24;
            this.fieldNQTYIN.Caption = "จำนวน รับ";
            this.fieldNQTYIN.FieldName = "NQTY_IN";
            this.fieldNQTYIN.Name = "fieldNQTYIN";
            // 
            // fieldNQTYOUT
            // 
            this.fieldNQTYOUT.AreaIndex = 25;
            this.fieldNQTYOUT.Caption = "จำนวนจ่าย";
            this.fieldNQTYOUT.FieldName = "NQTY_OUT";
            this.fieldNQTYOUT.Name = "fieldNQTYOUT";
            // 
            // fieldQCWHLOCA
            // 
            this.fieldQCWHLOCA.AreaIndex = 26;
            this.fieldQCWHLOCA.Caption = "รหัสที่เก็บสินค้า";
            this.fieldQCWHLOCA.FieldName = "QCWHLOCA";
            this.fieldQCWHLOCA.Name = "fieldQCWHLOCA";
            // 
            // fieldQNWHLOCA
            // 
            this.fieldQNWHLOCA.AreaIndex = 27;
            this.fieldQNWHLOCA.Caption = "ชื่อที่เก็บสินค้า";
            this.fieldQNWHLOCA.FieldName = "QNWHLOCA";
            this.fieldQNWHLOCA.Name = "fieldQNWHLOCA";
            // 
            // frmStockQuery1
            // 
            this.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 370);
            this.Controls.Add(this.pivotGridControl1);
            this.Name = "frmStockQuery1";
            this.Text = "STOCK INQUIRY";
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtspstk01)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl1;
        private LocalDataSet.DTSPSTK01 dtspstk01;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCIOTYPE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCLOT;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCFRWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCPDGRP;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCPDTYPE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCPROD;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCSECT;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCSUBCOOR;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCTOWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQCWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQN2SUBCOOR;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNFRWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNPDGRP;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNPDTYPE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNPROD;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNSECT;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNSUBCOOR;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNTOWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNUM;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCQNWHOUSE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCREFNO;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCSERIALNO;
        private DevExpress.XtraPivotGrid.PivotGridField fieldCWHTYPE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldDDATE;
        private DevExpress.XtraPivotGrid.PivotGridField fieldNQTY;
        private DevExpress.XtraPivotGrid.PivotGridField fieldNQTYIN;
        private DevExpress.XtraPivotGrid.PivotGridField fieldNQTYOUT;
        private DevExpress.XtraPivotGrid.PivotGridField fieldQCWHLOCA;
        private DevExpress.XtraPivotGrid.PivotGridField fieldQNWHLOCA;
    }
}