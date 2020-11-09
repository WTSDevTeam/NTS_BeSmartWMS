
//#define xd_DEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common.Invoice
{
    public partial class dlgGetInvOther : UIHelper.frmBase
    {

        public dlgGetInvOther(UIHelper.AppFormState inEditMode, DocumentType inDocType, string inSaleOrBuy)
        {
            InitializeComponent();

            this.mFormEditMode = inEditMode;
            this.mDocType = inDocType;
            this.mstrSaleOrBuy = inSaleOrBuy;
            this.pmInitForm();
        
        }

        public int xd_PAGE_BROWSE = 0;
        public int xd_PAGE_EDIT1 = 1;
        public int xd_PAGE_EDIT2 = 2;
        public int xd_PAGE_EDIT3 = 0;

        private UIHelper.AppFormState mFormEditMode;
        private DocumentType mDocType = DocumentType.SO;
        private string mstrSaleOrBuy = "S";
        private BeSmartMRP.Business.Entity.CoorType mCoorType = CoorType.Customer;
        private DataSet dtsDataEnv = new DataSet();
        private DateTime mdttInvDate = DateTime.Now;
        private DateTime mdttLastVatDue = DateTime.Now;
        private DateTime mdttVatDue = DateTime.Now;

        private string mstrVatType = "";
        private string mstrOldVatSeq = "";
        private string mstrVatSeq = "";

        private string mstrBranchID = "";
        private string mstrWHouseType = "('" + SysDef.gc_WHOUSE_TYPE_NORMAL + "')";

        private DialogForms.dlgGetWHouse pofrmGetWHouse = null;
        private DialogForms.dlgGetEmplSM pofrmGetSEmpl = null;
        private DialogForms.dlgGetEmplSM pofrmGetEmpl = null;
        //private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DatabaseForms.frmCust pofrmGetCoor = null;

        #region "Public Field"

        public string BranchID
        {
            get { return this.mstrBranchID; }
            set { this.mstrBranchID = value; }
        }

        /// <summary>
        /// วันที่ของเอกสาร Invoice
        /// </summary>
        public DateTime InvoiceDate
        {
            set { this.mdttInvDate = value; }
        }

        /// <summary>
        /// พนักงานขาย
        /// </summary>
        public string SEmpl
        {
            get { return this.txtQcSEmpl.Tag.ToString(); }
            set
            {
                this.txtQcSEmpl.Tag = value;
                string strCode = ""; string strName = "";
                this.pmSeekVal(this.txtQcSEmpl.Tag.ToString(), MapTable.Table.Employee, ref strCode, ref strName);
                this.txtQcSEmpl.Text = strCode;
                this.txtQnSEmpl.Text = strName;
            }
        }

        /// <summary>
        /// Sale Commission
        /// </summary>
        public decimal SaleCommission
        {
            get { return Convert.ToDecimal(this.txtCommPcn.Value); }
            set { this.txtCommPcn.Value = value; }
        }

        /// <summary>
        /// เครดิตเทอม
        /// </summary>
        public int CreditTerm
        {
            get { return Convert.ToInt32(this.txtCredTerm.Value); }
            set { this.txtCredTerm.Value = value; }
        }

        /// <summary>
        /// วันที่ส่งของ
        /// </summary>
        public DateTime ReceDate
        {
            get { return this.txtReceDate.DateTime; }
            set
            {
                DateTime dttValue = Convert.ToDateTime(value);
                if (dttValue.CompareTo(DBEnum.NullDate) == 0)
                {
                    this.txtReceDate.EditValue = null;
                }
                else
                {
                    this.txtReceDate.DateTime = dttValue;
                }
            }
        }

        /// <summary>
        /// วันที่ยืนราคา, วันที่ Due
        /// </summary>
        public DateTime DueDate
        {
            get { return this.txtDueDate.DateTime; }
            set
            {
                DateTime dttValue = Convert.ToDateTime(value);
                if (dttValue.CompareTo(DBEnum.NullDate) == 0)
                {
                    this.txtDueDate.EditValue = null;
                }
                else
                {
                    this.txtDueDate.DateTime = dttValue;
                }
            }
        }

        /// <summary>
        /// มีวันที่ใบแจ้งหนี้ ?
        /// </summary>
        public bool HasDueDate
        {
            get { return !(this.txtDueDate.EditValue == null); }
        }

        /// <summary>
        /// ผู้รับของ
        /// </summary>
        public string RecvMan
        {
            get { return this.txtRecvMan.Text; }
            set { this.txtRecvMan.Text = value; }
        }

        /// <summary>
        /// คลังสินค้า
        /// </summary>
        public string WHouse
        {
            get { return this.txtQcWHouse.Tag.ToString(); }
            set
            {
                this.txtQcWHouse.Tag = value;
                string strCode = ""; string strName = "";
                this.pmSeekVal(this.txtQcWHouse.Tag.ToString(), MapTable.Table.WHouse, ref strCode, ref strName);
                this.txtQcWHouse.Text = strCode;
                this.txtQnWHouse.Text = strName;
            }
        }

        /// <summary>
        /// รหัสโปรโมท
        /// </summary>
        public int PromoteCode
        {
            get { return Convert.ToInt32(this.txtPromote.Value); }
            set { this.txtPromote.Value = value; }
        }

        /// <summary>
        /// เลขที่ใบแจ้งหนี้
        /// </summary>
        public string DebtCode
        {
            get { return this.txtInvCode.Text; }
            set { this.txtInvCode.Text = value; }
        }

        /// <summary>
        /// วันที่ใบแจ้งหนี้
        /// </summary>
        public DateTime DebtDate
        {
            get { return this.txtInvDate.DateTime; }
            set
            {
                DateTime dttValue = Convert.ToDateTime(value);
                if (dttValue.CompareTo(DBEnum.NullDate) == 0)
                {
                    this.txtInvDate.DateTime = dttValue;
                }
                else
                {
                    this.txtInvDate.EditValue = null;
                }
            }
        }

        /// <summary>
        /// มีวันที่ใบแจ้งหนี้ ?
        /// </summary>
        public bool HasDebtDate
        {
            get { return !(this.txtInvDate.EditValue == null); }
        }

        /// <summary>
        /// ลูกค้าที่ใช้ในใบกำกับ
        /// </summary>
        public string VatCoor
        {
            get { return this.txtQcVatCoor.Tag.ToString(); }
            set
            {
                this.txtQcVatCoor.Tag = value;
                string strCode = ""; string strName = "";
                this.pmSeekVal(this.txtQcVatCoor.Tag.ToString(), MapTable.Table.Coor, ref strCode, ref strName);
                this.txtQcVatCoor.Text = strCode;
                this.txtQnVatCoor.Text = strName;
            }
        }

        /// <summary>
        /// รายละเอียดที่ออกในรายงานภาษี
        /// </summary>
        public string InvoiceDetail
        {
            get { return this.txtVatDet.Text.TrimEnd(); }
            set { this.txtVatDet.Text = value; }
        }

        /// <summary>
        /// ขอคืนได้หรือไม่ ?
        /// </summary>
        public string HasVatReturn
        {
            get { return (this.cmbHasRet.SelectedIndex == 0 ? "Y" : "N"); }
            set 
            {
                this.cmbHasRet.SelectedIndex = (value == "Y" ? 0 : 1); 
            }
        }

        /// <summary>
        /// วันที่ยื่นภาษี
        /// </summary>
        public DateTime VatDueDate
        {
            get { return new DateTime(this.txtVatDue.DateTime.Year, this.txtVatDue.DateTime.Month, 1); }
            set
            {
                this.txtVatDue.DateTime = value;
                //this.mdttLastVatDue = this.txtVatDue.Value;
                //this.mdttVatDue = this.txtVatDue.Value;
            }
        }

        /// <summary>
        /// VatType
        /// </summary>
        public string VatType
        {
            set { this.mstrVatType = value; }
        }

        /// <summary>
        /// Vat Sequence
        /// </summary>
        public string VatSeq
        {
            get { return this.mstrVatSeq; }
            set
            {
                this.mstrVatSeq = value;
                this.mstrVatSeq = (this.mstrVatSeq == string.Empty ? AppUtil.StringHelper.Replicate(" ", 11) : this.mstrVatSeq);
                this.txtVatSeq.Text = this.mstrVatSeq.Substring(6, 5);
                this.mstrOldVatSeq = this.mstrVatSeq;
            }
        }

        /// <summary>
        /// ลูกค้าที่ใช้ส่งของ
        /// </summary>
        public string DeliCoor
        {
            get { return this.txtQcDeliCoor.Tag.ToString(); }
            set
            {
                this.txtQcDeliCoor.Tag = value;
                string strCode = ""; string strName = "";
                this.pmSeekVal(this.txtQcDeliCoor.Tag.ToString(), MapTable.Table.Coor, ref strCode, ref strName);
                this.txtQcDeliCoor.Text = strCode;
                this.txtQnDeliCoor.Text = strName;
            }
        }

        #region "Public Remark Field"
        /// <summary>
        /// หมายเหตุ 1
        /// </summary>
        public string Remark1
        {
            get { return this.txtRemark1.Text; }
            set { this.txtRemark1.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 2
        /// </summary>
        public string Remark2
        {
            get { return this.txtRemark2.Text; }
            set { this.txtRemark2.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 3
        /// </summary>
        public string Remark3
        {
            get { return this.txtRemark3.Text; }
            set { this.txtRemark3.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 4
        /// </summary>
        public string Remark4
        {
            get { return this.txtRemark4.Text; }
            set { this.txtRemark4.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 5
        /// </summary>
        public string Remark5
        {
            get { return this.txtRemark5.Text; }
            set { this.txtRemark5.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 6
        /// </summary>
        public string Remark6
        {
            get { return this.txtRemark6.Text; }
            set { this.txtRemark6.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 7
        /// </summary>
        public string Remark7
        {
            get { return this.txtRemark7.Text; }
            set { this.txtRemark7.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 8
        /// </summary>
        public string Remark8
        {
            get { return this.txtRemark8.Text; }
            set { this.txtRemark8.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 9
        /// </summary>
        public string Remark9
        {
            get { return this.txtRemark9.Text; }
            set { this.txtRemark9.Text = value; }
        }

        /// <summary>
        /// หมายเหตุ 10
        /// </summary>
        public string Remark10
        {
            get { return this.txtRemark10.Text; }
            set { this.txtRemark10.Text = value; }
        }

        #endregion

        #endregion


        private void pmInitForm()
        {
            this.pmInitializeComponent();
        }

        private void pmInitializeComponent()
        {

            this.cmbHasRet.Properties.Items.Clear();
            this.cmbHasRet.Properties.Items.AddRange(new object[] { "Y=ได้", "N = ไม่ได้" });

            if (this.mstrSaleOrBuy == "S")
            {
                this.mCoorType = CoorType.Customer;
                this.lblWHouse.Text = "คลังขายหลัก :";
                this.pnlVatSeq.Visible = false;
            }
            else
            {
                this.mCoorType = CoorType.Supplier;
                this.lblWHouse.Text = "คลังซื้อหลัก :";
                this.pnlEmpl.Visible = false;
                this.pnlVatCoor.Visible = false;
            }

            this.txtRecvMan.Properties.MaxLength = 20;
            this.txtInvCode.Properties.MaxLength = 25;
            this.txtVatDet.Properties.MaxLength = 100;
            this.txtVatSeq.Properties.MaxLength = 5;

            this.txtQcSEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_CODE;
            this.txtQnSEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_NAME;

            this.txtQcWHouse.Properties.MaxLength = DialogForms.dlgGetWHouse.MAXLENGTH_CODE;
            this.txtQnWHouse.Properties.MaxLength = DialogForms.dlgGetWHouse.MAXLENGTH_NAME;

            this.txtQcVatCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_CODE;
            this.txtQnVatCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_NAME;

            this.txtQcDeliCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_CODE;
            this.txtQnDeliCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_NAME;

            this.txtQcSEmpl.Focus();

        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                switch (e.Item.Tag.ToString().ToUpper())
                {
                    case "OK":
                        if (this.pmValidForm())
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                        break;
                    case "CANCEL":
                        this.DialogResult = DialogResult.Cancel;
                        break;
                    default:
                        break;
                }
            }
        }

        private bool pmValidForm()
        {
            string strErrorMsg = "";
            bool bllResult = false;
            if (!this.pmValidDueDate())
            {
                strErrorMsg = "วันที่ครบกำหนดต้องมากกว่าวันที่เอกสาร";
                this.txtDueDate.Focus();
            }
            else if (!this.pmChkVatSeq(ref strErrorMsg))
            {
                this.txtVatSeq.Focus();
            }
            else if (this.txtQcWHouse.Text.Trim() == string.Empty)
            {
                strErrorMsg = "กรุณาระบุคลังสินค้า";
                this.txtQcWHouse.Focus();
            }
            else
            {
                bllResult = true;
            }
            if (!bllResult && strErrorMsg != string.Empty)
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return bllResult;
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        //this.pofrmGetCoor = new DialogForms.dlgGetCoor();
                        this.pofrmGetCoor = new DatabaseForms.frmCust(FormActiveMode.PopUp);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WHOUSE":
                    if (this.pofrmGetWHouse == null)
                    {
                        this.pofrmGetWHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "SEMPL":
                    if (this.pofrmGetSEmpl == null)
                    {
                        this.pofrmGetSEmpl = new DialogForms.dlgGetEmplSM("S");
                        this.pofrmGetSEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMPL":
                    if (this.pofrmGetEmpl == null)
                    {
                        this.pofrmGetEmpl = new DialogForms.dlgGetEmplSM("X");
                        this.pofrmGetEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "WHOUSE":
                    if (this.pofrmGetWHouse != null)
                    {
                        DataRow dtrPopup = this.pofrmGetWHouse.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtQcWHouse.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcWHouse.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtQnWHouse.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcWHouse.Tag = "";
                            this.txtQcWHouse.Text = "";
                            this.txtQnWHouse.Text = "";
                        }
                    }
                    break;
                case "SEMPL":
                    if (this.pofrmGetSEmpl != null)
                    {
                        DataRow dtrPopup = this.pofrmGetSEmpl.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtQcSEmpl.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcSEmpl.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtQnSEmpl.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcSEmpl.Tag = "";
                            this.txtQcSEmpl.Text = "";
                            this.txtQnSEmpl.Text = "";
                        }
                    }
                    break;
                case "VATCOOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrPopup = this.pofrmGetCoor.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtQcVatCoor.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcVatCoor.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtQnVatCoor.Text = dtrPopup["fcName"].ToString().TrimEnd();
                            this.txtVatDet.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcVatCoor.Tag = "";
                            this.txtQcVatCoor.Text = "";
                            this.txtQnVatCoor.Text = "";
                        }
                    }
                    break;
                case "DELICOOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrPopup = this.pofrmGetCoor.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtQcDeliCoor.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcDeliCoor.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtQnDeliCoor.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcDeliCoor.Tag = "";
                            this.txtQcDeliCoor.Text = "";
                            this.txtQnDeliCoor.Text = "";
                        }
                    }
                    break;
            }
            this.Focus();
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonClick(string inTextbox, string inValue1)
        {
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCWHOUSE":
                case "TXTQNWHOUSE":
                    strOrderBy = (inTextbox.ToUpper() == "TXTQCWHOUSE" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("WHOUSE");
                    this.pofrmGetWHouse.ValidateField(this.mstrBranchID, "", strOrderBy, true);
                    if (this.pofrmGetWHouse.PopUpResult)
                        this.pmRetrievePopUpVal("WHOUSE");
                    break;
                case "TXTQCSEMPL":
                case "TXTQNSEMPL":
                    strOrderBy = (inTextbox.ToUpper() == "TXTQCSEMPL" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("SEMPL");
                    this.pofrmGetSEmpl.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetSEmpl.PopUpResult)
                        this.pmRetrievePopUpVal("SEMPL");
                    break;
                case "TXTQCEMPL":
                case "TXTQNEMPL":
                    strOrderBy = (inTextbox.ToUpper() == "TXTQCEMPL" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("EMPL");
                    this.pofrmGetEmpl.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetEmpl.PopUpResult)
                        this.pmRetrievePopUpVal("EMPL");
                    break;
                case "TXTQCVATCOOR":
                case "TXTQNVATCOOR":
                    strOrderBy = (inTextbox.ToUpper() == "TXTQCVATCOOR" ? "CCODE" : "CSNAME");
                    this.pmInitPopUpDialog("COOR");
                    this.pofrmGetCoor.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetCoor.PopUpResult)
                        this.pmRetrievePopUpVal("VATCOOR");
                    break;
                case "TXTQCDELICOOR":
                case "TXTQNDELICOOR":
                    strOrderBy = (inTextbox.ToUpper() == "TXTQCDELICOOR" ? "CCODE" : "CSNAME");
                    this.pmInitPopUpDialog("COOR");
                    this.pofrmGetCoor.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetCoor.PopUpResult)
                        this.pmRetrievePopUpVal("DELICOOR");
                    break;
            }
        }

        private void txtWHouse_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopup.Name.ToUpper() == "TXTQCWHOUSE" ? "FCCODE" : "FCNAME");
            if (txtPopup.Text == "")
            {
                this.txtQcWHouse.Tag = "";
                this.txtQcWHouse.Text = "";
                this.txtQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WHOUSE");
                e.Cancel = !this.pofrmGetWHouse.ValidateField(this.mstrBranchID, txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetWHouse.PopUpResult)
                {
                    this.pmRetrievePopUpVal("WHOUSE");
                }
            }
        }

        private void txtSEmpl_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopup.Name.ToUpper() == "TXTQCSEMPL" ? "FCCODE" : "FCNAME");
            if (txtPopup.Text == "")
            {
                this.txtQcSEmpl.Tag = "";
                this.txtQcSEmpl.Text = "";
                this.txtQnSEmpl.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("SEMPL");
                e.Cancel = !this.pofrmGetSEmpl.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetSEmpl.PopUpResult)
                {
                    this.pmRetrievePopUpVal("SEMPL");
                }
            }
        }

        private void txtVatCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopup.Name.ToUpper() == "TXTQCVATCOOR" ? "CCODE" : "CSNAME");
            if (txtPopup.Text == "")
            {
                this.txtQcVatCoor.Tag = "";
                this.txtQcVatCoor.Text = "";
                this.txtQnVatCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal("VATCOOR");
                }
            }
        }

        private void txtDeliCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopup.Name.ToUpper() == "TXTQCDELICOOR" ? "CCODE" : "CSNAME");
            if (txtPopup.Text == "")
            {
                this.txtQcDeliCoor.Tag = "";
                this.txtQcDeliCoor.Text = "";
                this.txtQnDeliCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal("DELICOOR");
                }
            }
        }

        public void pmSeekVal(string inRowID, string inTabName, ref string ioCode, ref string ioName)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { inRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSeekVal", inTabName, "select FCCODE,FCNAME from " + inTabName + " where fcSkid = ?", ref strErrorMsg))
            {
                ioCode = this.dtsDataEnv.Tables["QSeekVal"].Rows[0]["fcCode"].ToString().TrimEnd();
                ioName = this.dtsDataEnv.Tables["QSeekVal"].Rows[0]["fcName"].ToString().TrimEnd();
            }
        }

        private void dlgGetInvOther_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex - 1 < xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);
                    break;
                case Keys.PageDown:
                    this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_BROWSE : this.pgfMainEdit.SelectedTabPageIndex + 1);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("ยกเลิกการแก้ไข ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    break;
            }
        }

        private void txtCredTerm_Validating(object sender, CancelEventArgs e)
        {
            this.pmCalDueDate();
        }

        private void pmCalDueDate()
        {
            DateTime dttDueDate = this.mdttInvDate.Date.AddDays(Convert.ToInt32(this.txtCredTerm.Value));
            if (dttDueDate.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("วันที่ครบกำหนดตรงกับวันอาทิตย์ จะปรับให้ตรงกับวันจันทร์ ", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dttDueDate = dttDueDate.Date.AddDays(1);
            }
            this.txtDueDate.DateTime = dttDueDate;
        }

        private void txtDueDate_Validating(object sender, CancelEventArgs e)
        {
            if (!this.pmValidDueDate())
            {
                e.Cancel = true;
                MessageBox.Show("วันที่ยืนราคาต้องมากกว่าวันที่เอกสาร", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool pmValidDueDate()
        {
            bool bllResult = true;
            if (this.mdttInvDate.Date.CompareTo(this.txtDueDate.DateTime.Date) > 0)
            {
                bllResult = false;
            }
            return bllResult;
        }

        private void txtVatDue_Validating(object sender, CancelEventArgs e)
        {
            if (this.mdttLastVatDue.CompareTo(this.txtVatDue.DateTime) != 0)
            {
                this.mdttLastVatDue = this.txtVatDue.DateTime;
                string strVatSeq = this.mstrVatSeq;
                DateTime dttDate = this.mdttVatDue;
                this.mstrVatSeq = BizRule.GetVatSeqPrefix(this.txtVatDue.DateTime) + this.txtVatSeq.Text;
                this.pmValidVatSeq();
                this.txtVatSeq.Text = this.mstrVatSeq.Substring(6, 5);
                this.mstrVatSeq = strVatSeq;
                this.mdttVatDue = dttDate;
            }
        }

        private bool pmValidVatSeq()
        {
            string strErrorMsg = "";
            string strRefNo = "";
            bool bllResult = true;
            if (this.mstrVatSeq.TrimEnd() != string.Empty)
            {
                if (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldVatSeq != this.mstrVatSeq)
                {
                    string strMthYr = BizRule.GetVatSeqPrefix(mdttVatDue);

                    WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                    object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrVatType, this.mstrVatSeq };

                    if (BizRule.ChkDupVatSeq(pobjSQLUtil, pAPara, ref strRefNo))
                    {
                        strErrorMsg = "";
                        pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrVatType };
                        string strNewVatSeq = BizRule.RunVatSeq(pobjSQLUtil, pAPara, strMthYr, ref strErrorMsg);

                        strErrorMsg = "ลำดับที่ [" + this.mstrVatSeq + "] เพื่อออกรายงานภาษีซื้อ\n";
                        strErrorMsg += "ซ้ำกับของเอกสารใบอื่น\n";
                        strErrorMsg += "เปลี่ยนเป็นลำดับที่ [" + strNewVatSeq.Substring(strMthYr.Length, 5) + "]";
                        MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        this.mstrVatSeq = strNewVatSeq;
                        bllResult = false;
                    }
                }
            }
            return bllResult;
        }

        private bool pmChkVatSeq(ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strRefNo = "";
            string strMthYr = BizRule.GetVatSeqPrefix(this.txtVatDue.DateTime);
            string strVatSeq = strMthYr + this.txtVatSeq.Text.TrimEnd();
            if (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldVatSeq != strVatSeq)
            {

                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrVatType, strVatSeq };

                if (BizRule.ChkDupVatSeq(pobjSQLUtil, pAPara, ref strRefNo))
                {

                    ioErrorMsg = "ลำดับที่ [" + this.txtVatSeq.Text.TrimEnd() + "] เพื่อออกรายงานภาษีซื้อ\n";
                    ioErrorMsg += "ซ้ำกับของเอกสารเลขที่ " + strRefNo + "\n";
                    //MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    bllResult = false;
                }
            }
            else
            {
                bllResult = true;
                this.mstrVatSeq = strMthYr + this.txtVatSeq.Text.TrimEnd();
            }
            return bllResult;
        }

        private void txtVatSeq_Validating(object sender, CancelEventArgs e)
        {
            string strErrorMsg = "";
            if (!this.pmChkVatSeq(ref strErrorMsg))
            {
                e.Cancel = true;
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
