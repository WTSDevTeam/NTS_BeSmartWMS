
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction
{

    public partial class frmInvoice : UIHelper.frmBase
    {

        public static string TASKNAME = "SALE_ORDER";
        public const string xd_ALIAS_TEMREFTO = "TemRefTo";
        public const string xd_Alias_TemRefToProd = "TemRefToReqProd";
        public const string xd_Alias_Tem1PdSer = "Tem1PdSer";

        public const string xd_ALIAS_BROW_REFTO = "TemBrowRefTo";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private int intActiveRow = -1;
        private string mstrBrowViewAlias = "Brow_Alias";
        //private DBS.Business.Entity.Book mobjTabRefer = new Book(App.ERPConnectionString, App.DatabaseReside);
        private DataSet dtsDataEnv = new DataSet();
        private bool mbllIsLoadPageEdit = false;
        private bool mbllFilterResult = false;
        private bool bllIsInitGrid = false;
        //private bool mbllPopUpResult = false;
        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrSortKey = "FCCODE";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrBranchID = "";
        private string mstrBookID = "";
        private string mstrQcBook = "";
        private string mstrRefType = "SO";
        private DocumentType mDocType = DocumentType.SO;
        private string mstrBookPrefix = "";
        private bool mbllOrderOnly = false;

        private string mstrRfType = "S";
        private string mstrRefTypeName = "Sale Order";
        private string mstrPdType = "";
        private string mstrSaleOrBuy = "S";
        private string mstrSaleOrBuyForPdSer = "S";
        private string mstrIOType = "O";

        private BeSmartMRP.Business.Entity.CoorType mCoorType = CoorType.Customer;

        private DateTime mdttBrowBegDate = DateTime.Now;
        private DateTime mdttBrowEndDate = DateTime.Now;

        private string mstrTemPd = StockAgent.xd_Alias_TemPd;
        private string mstrHTable = MapTable.Table.GLRef;
        private string mstrITable = MapTable.Table.RefProd;
        private string mstrITable2 = "XAREFPROD";

        //รายละเอียดเอกสารอ้างอิง
        private string mstrRefToRefType = "PR";
        private DocumentType mRefToDocType = DocumentType.PR;
        private DocumentType mDefaRefToDocType = DocumentType.PR;

        private string mstrRefToTab = MapTable.Table.GLRef;
        private string mstrRefToBook = "";
        private string mstrRefToRowID = "";
        private decimal mdecRefToAmt = 0;
        private string mstrOldRefToRowID = "";
        private bool mbllIsCrDrChgQty = true;

        //private string mstrDefaLot = "                    ";
        private string mstrDefaWHouse = "";
        private string mstrDefaWHouseTypeList = " ";//SysDef.gc_WAREHOUSE_TYPE_NORMAL;
        private string mstrDefaVATType = "";

        //private bool mbllWaitForApprove = false;

        private bool mbllWaitForApprove = true;
        private string mstrStep = SysDef.gc_REF_STEP_CUT_STOCK;
        //private string mstrDOStep = SysDef.gc_REF_STEP_CUT_STOCK;
        private string mstrAtStep = "";

        private DateTime mdttLastLock = DBEnum.NullDate;
        private DateTime mdttBookLastLock = DBEnum.NullDate;
        private DateTime mdttAccBookLastLock = DBEnum.NullDate;
        private DateTime mdttLastClose = DBEnum.NullDate;

        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        //private string mstrVatDue = "";

        private string mstrLastQcCoor = "";
        private string mstrLastQsCoor = "";
        private string mstrLastQcWHouse = "";	//คลังสินค้า

        private string mstrDivision = "";	//ฝ่าย
        private string mstrJob = "";
        private string mstrProj = "";
        private string mstrCurrSign = "";

        private decimal mdecVatRate = 0;

        private decimal mdecAmtStd = 0;
        private decimal mdecDiscAmtStd = 0;
        private decimal mdecVATAmtStd = 0;

        private string mstrVatDue = "";
        private string mstrCoorType = "";
        private string mstrDetailMsg = "";
        private string mstrRefNoMsg = "";
        private bool mbllIsInv = false;
        private bool mbllIsCrNote = false;
        private bool mbllIsDrNote = false;
        private bool mbllIsServ = false;
        private bool mbllIsGetDebt = false;
        private bool mbllCanGetTotAmt = false;
        private bool mbllChkCoorBal = false;
        private int mintUpdBalSign = 1;
        private int mintUpdStockSign = 1;
        private string mstrRefToRefTypeList = "";
        private string mstrDefaRefToRefType = "";
        private string mstrCoorMsg = "";
        private DateTime mdttReceDate = DateTime.Now;


        #region "รายละเอียดหน้าอื่น ๆ "

        //หน้าที่ 1
        private string mstrSEmpl = "";
        private decimal mdecSaleComm = 0;
        private int mintCredTerm = 0;
        private DateTime mdttDueDate = DateTime.Now;
        private int mintPromote = 0;
        private DateTime mdttSendDate = DateTime.Now;
        private string mstrWHouse = "";
        private string mstrDeliCoor = "";
        //private string mstrEmpl = "";
        private string mstrRecvMan = "";
        private string mstrDebtCode = "";
        private DateTime mdttDebtDate = DateTime.Now;
        private string mstrVatCoor = "";
        private string mstrInvDetail = "";
        private string mstrOldVatSeq = "";
        private string mstrVatSeq = "";
        private string mstrTradeTrm = "";


        //หน้าที่ 2
        private string mstrRemarkH1 = "";
        private string mstrRemarkH2 = "";
        private string mstrRemarkH3 = "";
        private string mstrRemarkH4 = "";
        private string mstrRemarkH5 = "";
        private string mstrRemarkH6 = "";
        private string mstrRemarkH7 = "";
        private string mstrRemarkH8 = "";
        private string mstrRemarkH9 = "";
        private string mstrRemarkH10 = "";

        #endregion

        private string mstrOldCode = "";
        private string mstrOldRefNo = "";
        private int mintSaveBrowViewRowIndex = 0;
        private bool mbllRecalTotPd = false;
        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;

        private DialogForms.dlgGetCoor pofrmGetCoor = null;
        //private DialogForms.dlgGetJob pofrmGetJob = null;
        private DialogForms.dlgGetDept pofrmGetDept = null;
        private DialogForms.dlgGetProdType pofrmGetPdType = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetVatType pofrmGetVatType = null;
        private DialogForms.dlgGetCurrency pofrmGetCurrency = null;
        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;
        //private DialogForms.dlgGetFormula pofrmGetFormula = null;

        //private PdSer mPdSer = new PdSer();
        //private Common.cShowPdHistory oShowPdHistory = null;
        private StockAgent mStockAgent = new StockAgent(App.ERPConnectionString, App.ConnectionString, App.DatabaseReside);

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private bool mbllIsPrintGroup = true;
        private string mstrCoorDiscStr = "";

        public frmInvoice(string inRefType)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.mDocType = BusinessEnum.GetDocEnum(this.mstrRefType);

            this.pmInitForm();
        }

        private bool pmChkReferItem(string inEditType)
        {
            bool bllResult = false;

            string strMsg1 = "แก้ไข";
            string strMsg2 = "";
            switch (inEditType.ToUpper())
            {
                case "E":
                    strMsg1 = "แก้ไข";
                    break;
                case "D":
                    strMsg1 = "ลบ";
                    break;
            }

            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            if (dtrTemPd["cStep"].ToString() == SysDef.gc_REF_STEP_PAY)
            {
                switch (this.mDocType)
                {
                    case DocumentType.SO:
                    case DocumentType.PO:
                        strMsg2 = "ส่งของครบแล้ว";
                        break;
                    case DocumentType.QS:
                        strMsg2 = "จัดทำเอกสาร SO ";
                        break;
                    case DocumentType.PR:
                        strMsg2 = "จัดทำเอกสาร PO ";
                        break;
                    default:
                        strMsg2 = "ส่งของครบแล้ว";
                        break;
                }
                MessageBox.Show("ไม่สามารถ" + strMsg1 + "รายการได้เนื่องจาก" + strMsg2 + "ครบแล้ว", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bllResult = true;
            }
            return bllResult;
        }

        private void pmInitForm()
        {
            this.pmInitializeComponent();
            this.pmFilterForm();
            this.pmSetBrowView();
            this.pmInitGridProp();

            //if (this.gridView1.RowCount > 0)
            //    this.gridView1.FocusedRowHandle = this.gridView1.RowCount - 1;

            //this.gridView1.MoveFirst();
            //this.gridView1.MoveLast();
            this.gridView1.MoveLastVisible();
            
            //Message msg = new Message();
            //this.ProcessCmdKey(ref msg, Keys.Up);
            
            this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_BROWSE;
            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
        }

        private void pmInitializeComponent()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefType", "REFTYPE", "select * from RefType where fcSkid = ?", ref strErrorMsg);
            DataRow dtrRefType = this.dtsDataEnv.Tables["QRefType"].Rows[0];

            this.mstrRefTypeName = dtrRefType["fcName"].ToString();
            this.mstrRfType = dtrRefType["fcRfType"].ToString();
            this.mstrIsCash = dtrRefType["fcIsCash"].ToString();
            this.mstrVatDue = dtrRefType["fcVatDue"].ToString();
            this.mstrCoorType = dtrRefType["fcCoorType"].ToString();
            this.mbllIsInv = (SysDef.gc_RFTYPE_INV_BUY + SysDef.gc_RFTYPE_INV_SELL).IndexOf(dtrRefType["fcRfType"].ToString()) > -1;
            this.mbllIsCrNote = (SysDef.gc_RFTYPE_CR_BUY + SysDef.gc_RFTYPE_CR_SELL).IndexOf(dtrRefType["fcRfType"].ToString()) > -1;
            this.mbllIsDrNote = (SysDef.gc_RFTYPE_DR_BUY + SysDef.gc_RFTYPE_DR_SELL).IndexOf(dtrRefType["fcRfType"].ToString()) > -1;
            this.mbllIsServ = (dtrRefType["fcIsServic"].ToString() == "Y");
            this.mbllIsGetDebt = (SysDef.gc_REFTYPE_S_INV_Q + "," + SysDef.gc_REFTYPE_S_INV_Y + "," + SysDef.gc_REFTYPE_P_INV_Q + "," + SysDef.gc_REFTYPE_P_INV_Y + ",").IndexOf(dtrRefType["fcCode"].ToString()) > -1;

            this.mbllCanGetTotAmt = (this.mbllIsCrNote || this.mbllIsDrNote);

            this.mStockAgent.CorpID = App.ActiveCorp.RowID;
            this.mStockAgent.IsCrSell = this.mbllIsCrNote;

            if (this.mstrVatDue == "Y")
            {
                this.mstrDetailMsg = "รายการในรายงานภาษี";
                this.mstrRefNoMsg = "เลขใบกำกับภาษี";
            }
            else
            {
                this.mstrDetailMsg = "รายละเอียด";
                this.mstrRefNoMsg = "เลขที่อ้างอิง";
            }

            this.mbllChkCoorBal = (this.mstrCoorType == SysDef.gc_CUST_TYPE && this.mstrIsCash != "Y");
            this.mintUpdBalSign = (this.mbllIsCrNote == false ? 1 : -1);	// ยอดหนี้เป็น + เสมอ
            this.mCoorType = (this.mstrCoorType == SysDef.gc_CUST_TYPE ? CoorType.Customer : CoorType.Supplier);

            this.pnlRefTo.Visible = false;
            if (this.mCoorType == CoorType.Customer)
            {
                this.mstrCoorMsg = "ลูกค้า";
                this.mstrSaleOrBuy = "S";
                this.mstrSaleOrBuyForPdSer = "S";
                this.mstrIOType = "O";
                this.mintUpdStockSign = (this.mbllIsCrNote == false ? -1 : 1);
                //gocstStkUtilPara.plpropCrSell = ( this.mbllIsCrNote == true );
            }
            else
            {
                this.mstrCoorMsg = "ผู้จำหน่าย";
                this.mstrSaleOrBuy = "P";
                this.mstrSaleOrBuyForPdSer = (this.mbllIsCrNote == false ? "P" : "S");
                this.mstrIOType = "I";
                this.mintUpdStockSign = (this.mbllIsCrNote == false ? 1 : -1);
            }

            //this.mstrRefToRefTypeList = this.pmSplitToSQLStr(this.pmGetRefToRefTypeList());
            //this.mstrDefaRefToRefType = this.pmSplitToSQLStr(this.pmGetDefaRefToRefType());

            switch (this.mDocType)
            {
                case DocumentType.BR:
                case DocumentType.BI:
                    this.lblRefTo.Text = "ค้นใบ PO :";
                    this.mstrRefToRefType = DocumentType.PO.ToString();
                    this.mstrDefaRefToRefType = this.mstrRefToRefType;
                    this.mstrRefToTab = MapTable.Table.OrderH;
                    this.mstrRefToRefTypeList = this.mstrDefaRefToRefType;
                    break;
                case DocumentType.SR:
                case DocumentType.SI:
                    this.pnlRefTo.Visible = true;
                    this.lblRefTo.Text = "ค้นใบ SO :";
                    this.mstrRefToRefType = DocumentType.SO.ToString();
                    this.mstrDefaRefToRefType = this.mstrRefToRefType;
                    this.mstrRefToTab = MapTable.Table.OrderH;
                    this.mstrRefToRefTypeList = this.mstrDefaRefToRefType;
                    break;
                default:
                    this.lblRefTo.Text = "ค้นใบ Inv# :";
                    this.mstrDefaRefToRefType = this.pmGetDefaRefToRefType();
                    this.mstrRefToRefType = this.mstrDefaRefToRefType;
                    this.mstrRefToTab = MapTable.Table.GLRef;
                    this.mstrRefToRefTypeList = this.pmGetRefToRefTypeList();
                    break;
            }

            this.mstrPdType = this.pmSplitToSQLStr(this.pmGetRefProdType());
            this.mDefaRefToDocType = BusinessEnum.GetDocEnum(this.mstrDefaRefToRefType);
            this.mRefToDocType = BusinessEnum.GetDocEnum(this.mstrDefaRefToRefType);

            //this.pgfMainEdit.ShowTabs = false;
            TASKNAME = "INV_" + this.mstrRefType.TrimEnd();
            this.Text = this.mstrRefTypeName.TrimEnd();
            //this.lblTitle.Text = this.mstrRefTypeName.TrimEnd();
            this.lblQcCoor.Text = "รหัส" + this.mstrCoorMsg + " :";
            this.lblQnCoor.Text = "ชื่อย่อ" + this.mstrCoorMsg + " :";

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.barMain.BarName = TASKNAME;
            //this.pmCreateInvToolBar(this.WsLocale, ref this.uiInvBar);

            this.txtCode.Properties.MaxLength = 7;
            this.txtRefNo.Properties.MaxLength = 15;
            this.txtQcCoor.Properties.MaxLength = 20;
            this.txtQsCoor.Properties.MaxLength = 70;
            this.txtVatType.Properties.MaxLength = 1;
            this.txtVatIsOut.Properties.MaxLength = 1;
            this.txtDiscount.Properties.MaxLength = 20;

            this.pmSetToolbarState(xd_PAGE_BROWSE);
            this.pmCreateTem();
            this.pmSetPropGrdTemPd();

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

        }

        private string pmGetRefToRefTypeList()
        {
            string strRef2RefType = "";
            if (this.mCoorType == CoorType.Customer)
            {
                if (this.mbllIsInv)
                {
                    strRef2RefType = SysDef.gc_REFTYPE_S_ORDER + ",";
                }
                else
                {
                    strRef2RefType = (this.mbllIsServ ? SysDef.gc_GRP_REFTYPE_S_INV_SERVICE : SysDef.gc_GRP_REFTYPE_S_INV_PRODUCT);
                }
            }
            else
            {
                if (this.mbllIsInv)
                {
                    strRef2RefType = SysDef.gc_REFTYPE_P_ORDER + ",";
                }
                else
                {
                    strRef2RefType = (this.mbllIsServ ? SysDef.gc_GRP_REFTYPE_P_INV_SERVICE : SysDef.gc_GRP_REFTYPE_P_INV_PRODUCT);
                }
            }
            return strRef2RefType;
        }

        private string pmGetDefaRefToRefType()
        {
            string strRefType = "";
            if (this.mbllIsCrNote || this.mbllIsDrNote)
            {
                //ลดหนี้/เพิ่มหนี้ฝั่งขาย
                if (this.mCoorType == CoorType.Customer)
                {
                    //งานบริการ
                    if (this.mbllIsServ)
                    {
                        if ((SysDef.gc_REFTYPE_S_CR_A + "," + SysDef.gc_REFTYPE_S_DR_B).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_S_INV_U;
                        }
                        else if ((SysDef.gc_REFTYPE_S_CR_N + "," + SysDef.gc_REFTYPE_S_DR_Z).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_S_INV_V;
                        }
                    }
                    else
                    {
                        if ((SysDef.gc_REFTYPE_S_CR_C + "," + SysDef.gc_REFTYPE_S_DR_D).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_S_INV_R;
                        }
                        else if ((SysDef.gc_REFTYPE_S_CR_M + "," + SysDef.gc_REFTYPE_S_DR_X).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_S_INV_I;
                        }
                    }
                }
                //ลดหนี้/เพิ่มหนี้ฝั่งซื้อ
                else
                {

                    if (this.mbllIsServ)
                    {
                        if ((SysDef.gc_REFTYPE_P_CR_A + "," + SysDef.gc_REFTYPE_P_DR_B).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_P_INV_U;
                        }
                        else if ((SysDef.gc_REFTYPE_P_CR_N + "," + SysDef.gc_REFTYPE_P_DR_Z).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_P_INV_V;
                        }
                    }
                    else
                    {
                        if ((SysDef.gc_REFTYPE_P_CR_C + "," + SysDef.gc_REFTYPE_P_DR_D).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_P_INV_R;
                        }
                        else if ((SysDef.gc_REFTYPE_P_CR_M + "," + SysDef.gc_REFTYPE_P_DR_X).IndexOf(this.mstrRefType) > -1)
                        {
                            strRefType = SysDef.gc_REFTYPE_P_INV_I;
                        }
                    }

                }
            }
            return strRefType;
        }


        private string pmGetRefProdType()
        {
            string strResult = "";
            if (this.mstrSaleOrBuy == "S" && this.mbllIsServ)
            {
                strResult = SysDef.gc_PROD_TYPE_EXPENSE + "," + SysDef.gc_PROD_TYPE_INCOME + "," + SysDef.gc_PROD_TYPE_PLEDGE;
            }
            else if (this.mstrSaleOrBuy == "S" && this.mbllIsServ == false)
            {
                strResult = SysDef.gc_PROD_TYPE_FINISH + "," + SysDef.gc_PROD_TYPE_RAW_MAT + "," + SysDef.gc_PROD_TYPE_SEMI + "," + SysDef.gc_PROD_TYPE_ASSET + "," + SysDef.gc_PROD_TYPE_INCOME + ","
                    + SysDef.gc_PROD_TYPE_COMPO + "," + SysDef.gc_PROD_TYPE_SPARE + "," + SysDef.gc_PROD_TYPE_LABEL + "," + SysDef.gc_PROD_TYPE_PACKAGE + "," + SysDef.gc_PROD_TYPE_LAND + ","
                    + SysDef.gc_PROD_TYPE_BUILDING + "," + SysDef.gc_PROD_TYPE_OFFICE_EQUIP + "," + SysDef.gc_PROD_TYPE_MACHINE + ","
                    + SysDef.gc_PROD_TYPE_SUBCONTRAC + "," + SysDef.gc_PROD_TYPE_PLEDGE + "," + SysDef.gc_PROD_TYPE_TOOLS;
            }
            else if (this.mstrSaleOrBuy != "S")
            {
                strResult = SysDef.gc_PROD_TYPE_FINISH + "," + SysDef.gc_PROD_TYPE_RAW_MAT + "," + SysDef.gc_PROD_TYPE_OFFICE_SUPPLY + "," + SysDef.gc_PROD_TYPE_SEMI + "," + SysDef.gc_PROD_TYPE_ASSET + "," + SysDef.gc_PROD_TYPE_OTHERS + ","
                    + SysDef.gc_PROD_TYPE_COMPO + "," + SysDef.gc_PROD_TYPE_SPARE + "," + SysDef.gc_PROD_TYPE_LABEL + "," + SysDef.gc_PROD_TYPE_PACKAGE + "," + SysDef.gc_PROD_TYPE_LAND + ","
                    + SysDef.gc_PROD_TYPE_BUILDING + "," + SysDef.gc_PROD_TYPE_OFFICE_EQUIP + "," + SysDef.gc_PROD_TYPE_MACHINE + ","
                    + SysDef.gc_PROD_TYPE_SUBCONTRAC + "," + SysDef.gc_PROD_TYPE_EXPENSE + "," + SysDef.gc_PROD_TYPE_PLEDGE + "," + SysDef.gc_PROD_TYPE_TOOLS;
            }
            strResult += "," + SysDef.gc_PROD_TYPE_REMARK;
            return strResult;
        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
                base.OnClosing(ce);
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "ต้องการที่จะปิดหน้าจอการทำงานหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void pmSetToolbarState(int inActivePage)
        {
            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = (this.mFormActiveMode == FormActiveMode.PopUp ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);

            this.barMainEdit.Items[WsToolBar.Print.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            if (this.mFormActiveMode == FormActiveMode.Report)
            {
                this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Update.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Print.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);

            this.barMainEdit.Items["INSPD"].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items["DELPD"].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items["EOTH"].Enabled = (inActivePage == 0 ? false : true);

        }

        //private void pmCreateInvToolBar(enuWsLocale inLocale, ref UICommandBar Sender)
        //{
        //    Sender.Text = "Other";
        //    UICommand cmdOth = new UICommand("OTH", WsWinUtil.GetStringfromCulture(inLocale, new string[] { "แก้ไขอื่น ๆ", "Other" }), 13, Shortcut.F9);
        //    UICommand cmdDet1 = new UICommand("DET1", WsWinUtil.GetStringfromCulture(inLocale, new string[] { "รายละเอียดส/ค", "Show Detail" }), 13, Shortcut.F7);
        //    //UICommand cmdHistory = new UICommand("HISTORY", WsWinUtil.GetStringfromCulture(inLocale, new string[] { "ประวัติอื่น ๆ", "Show History" }), 13);

        //    UICommand cmdHis_Coor = new UICommand("HIS_COOR", WsWinUtil.GetStringfromCulture(inLocale, new string[] { "แสดงประวัติสินค้า", "Show Product History" }));
        //    UICommand cmdHis_Prod = new UICommand("HIS_PROD", WsWinUtil.GetStringfromCulture(inLocale, new string[] { "แสดงประวัติ" + this.mstrCoorMsg, "Show Coor History" }));
        //    //UICommand cmdCopy = new UICommand("COPY", WsWinUtil.GetStringfromCulture(inLocale, new string[] {"คัดลอก", "Copy"}), 13, Shortcut.ShiftIns);

        //    cmdOth.ToolTipText = WsWinUtil.GetStringfromCulture(inLocale, new string[] { "กรอกข้อมูลอื่น ๆ", "Enter Other Detail" });
        //    cmdDet1.ToolTipText = WsWinUtil.GetStringfromCulture(inLocale, new string[] { "แสดงรายละเอียดสินค้า", "Show Product Detail" });
        //    cmdHistory.ToolTipText = WsWinUtil.GetStringfromCulture(inLocale, new string[] { "แสดงประวัติสินค้า", "Show Product History" });
        //    //cmdCopy.ToolTipText = WsWinUtil.GetStringfromCulture(inLocale, new string[] {"คัดลอกเอกสาร", "Copy from Other Document"});

        //    cmdHis_Coor.Click += new CommandEventHandler(cmdHis_Coor_Click);
        //    cmdHis_Prod.Click += new CommandEventHandler(cmdHis_Prod_Click);
        //    cmdHistory.Commands.AddRange(new UICommand[] { cmdHis_Coor, cmdHis_Prod });
        //    Sender.Commands.AddRange(new UICommand[] { cmdDet1, cmdOth, cmdHistory });
        //}

        private void pmSetBrowView()
        {
            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind();
            string strErrorMsg = "";
            string strSQLExec;
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strStep = this.pmGenOrderStep();

            string[] staPara = new string[] { MapTable.Table.GLRef, MapTable.Table.Coor, MapTable.Table.Currency, MapTable.Table.EmplR };

            string strAppFld = "GLREF.FTDATETIME, GLREF.FTLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD";

            strSQLExec = "select GLREF.FCSKID, GLREF.FCSTAT, GLREF.FCATSTEP, " + strStep + ", GLREF.FCCODE, GLREF.FCREFNO, GLREF.FDDATE, COOR.FCSNAME as CQNCOOR, CURRENCY.FCSIGN, GLREF.FNAMTKE+GLREF.FNVATAMTKE as NAMT, GLREF.FCLAYH , "+strAppFld+" from {0} GLREF";
            strSQLExec = strSQLExec + " left join {1} COOR on (GLREF.FCCOOR = COOR.FCSKID) ";
            strSQLExec = strSQLExec + " left join {2} CURRENCY on (GLREF.FCCURRENCY = CURRENCY.FCSKID) ";
            strSQLExec = strSQLExec + " left join {3} EM1 ON EM1.FCSKID = GLREF.FCCREATEBY ";
            strSQLExec = strSQLExec + " left join {3} EM2 ON EM2.FCSKID = GLREF.FCCORRECTB ";
            strSQLExec = strSQLExec + " where GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCREFTYPE = ? and GLREF.FCBOOK = ? and GLREF.FDDATE between ? and ?";
            pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrBookID, this.mdttBrowBegDate, this.mdttBrowEndDate };

            strSQLExec = String.Format(strSQLExec, staPara);
            //pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrHTable, strSQLExec, ref strErrorMsg);
            DataColumn dtcRefTo = new DataColumn("CREFTO", System.Type.GetType("System.String"));
            dtcRefTo.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRefTo);
            //dlg.WaitClear();
        }

        private string pmGenOrderStep()
        {
            string strStep = "";
            string[] staPara = new string[] { MapTable.Table.GLRef, SysDef.gc_REF_STEP_PAY, SysDef.gc_REF_STEP_CLOSED, SysDef.gc_REF_STEP_WAIT_APPROVE, SysDef.gc_REF_STEP_CUT_STOCK };

            switch (this.mDocType)
            {
                case DocumentType.PR:
                    if (this.mbllWaitForApprove)
                    {
                        strStep = "FCSTEP = case ";
                        strStep += " when {0}.FCSTEP = '{1}' then 'PO' ";
                        strStep += " when {0}.FCSTEP = '{2}' then 'CLOSE' ";
                        strStep += " when {0}.FCSTEP = '{3}' then 'WAIT' ";
                        strStep += " when {0}.FCSTEP = '{4}' and {0}.FCAPPROVEB = '' then '' ";
                        strStep += " when {0}.FCSTEP = '{4}' and {0}.FCAPPROVEB <> '' then 'APPROVE' ";
                        strStep += " end ";
                    }
                    else
                    {
                        strStep = "FCSTEP = case {0}.FCSTEP when '{1}' then 'PO' when '{2}' then 'CLOSE' when '{4}' then ' ' end";
                    }
                    break;
                case DocumentType.QS:
                    strStep = "FCSTEP = case {0}.FCSTEP when '{1}' then 'SO' when '{2}' then 'CLOSE' when '{4}' then ' ' end";
                    break;
                case DocumentType.PO:
                    if (this.mbllWaitForApprove)
                    {
                        strStep = "FCSTEP = case ";
                        strStep += " when {0}.FCSTEP = '{1}' then 'DLVR' ";
                        strStep += " when {0}.FCSTEP = '{2}' then 'CLOSE' ";
                        strStep += " when {0}.FCSTEP = '{3}' then 'WAIT' ";
                        strStep += " when {0}.FCSTEP = '{4}' and {0}.FCAPPROVEB = '' then '' ";
                        strStep += " when {0}.FCSTEP = '{4}' and {0}.FCAPPROVEB <> '' then 'APPROVE' ";
                        strStep += " end ";
                    }
                    else
                    {
                        strStep = "FCSTEP = case {0}.FCSTEP when '{1}' then 'DLVR' when '{2}' then 'CLOSE' when '{4}' then ' ' end";
                    }
                    break;
                case DocumentType.SO:
                    strStep = "FCSTEP = case {0}.FCSTEP when '{1}' then 'DLVR' when '{2}' then 'CLOSE' when '{4}' then ' ' end";
                    break;
                default:
                    strStep = "{0}.FCSTEP";
                    break;
            }
            return String.Format(strStep, staPara);
        }


        private void pmInitGridProp()
        {

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            
            this.gridView1.Columns["FCSKID"].Visible = false;
            this.gridView1.Columns["FCLAYH"].Visible = false;
            this.gridView1.Columns["FCSIGN"].Visible = false;
            this.gridView1.Columns["CLOGIN_ADD"].Visible = false;
            this.gridView1.Columns["CLOGIN_UPD"].Visible = false;
            this.gridView1.Columns["FTDATETIME"].Visible = false;
            this.gridView1.Columns["FTLASTUPD"].Visible = false;

            this.gridView1.Columns["FCSTAT"].VisibleIndex = 0;
            this.gridView1.Columns["FCATSTEP"].VisibleIndex = 1;
            this.gridView1.Columns["FCSTEP"].VisibleIndex = 2;
            this.gridView1.Columns["FCCODE"].VisibleIndex = 3;
            this.gridView1.Columns["FCREFNO"].VisibleIndex = 4;
            this.gridView1.Columns["FDDATE"].VisibleIndex = 5;
            this.gridView1.Columns["CQNCOOR"].VisibleIndex = 6;
            this.gridView1.Columns["NAMT"].VisibleIndex = 7;
            this.gridView1.Columns["CREFTO"].VisibleIndex = 8;

            this.gridView1.Columns["FCSTAT"].Caption = "C";
            this.gridView1.Columns["FCATSTEP"].Caption = "P";
            this.gridView1.Columns["FCSTEP"].Caption = "";
            this.gridView1.Columns["FCCODE"].Caption = "เลขที่ภายใน";
            this.gridView1.Columns["FCREFNO"].Caption = "เลขที่อ้างอิง";
            this.gridView1.Columns["FDDATE"].Caption = "วันที่";
            this.gridView1.Columns["CQNCOOR"].Caption = (this.mCoorType == CoorType.Customer ? "ชื่อย่อลูกค้า" : "ชื่อย่อผู้จำหน่าย");
            this.gridView1.Columns["FCSIGN"].Caption = "สกุลเงิน";
            this.gridView1.Columns["NAMT"].Caption = "มูลค่า";
            this.gridView1.Columns["CREFTO"].Caption = "";

            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["FCSTAT"].Width = 30;
            //this.gridView1.Columns["FCSTAT"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCSTAT"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["FCATSTEP"].Width = 30;
            //this.gridView1.Columns["FCATSTEP"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCATSTEP"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
            this.gridView1.Columns["FCSTEP"].Width = 30;
            //this.gridView1.Columns["FCSTEP"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCSTEP"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["FCCODE"].Width = 80;
            this.gridView1.Columns["FCREFNO"].Width = 80;
            this.gridView1.Columns["FDDATE"].Width = 80;
            this.gridView1.Columns["NAMT"].Width = 100;
            this.gridView1.Columns["CREFTO"].Width = 10;

            this.gridView1.Columns["NAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NAMT"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["FDDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["FDDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.pmSetSortKey("fcCode", true);
            this.bllIsInitGrid = true;
            this.pmCalcColWidth();
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["FCSTAT"].Width
                                    + this.gridView1.Columns["FCATSTEP"].Width
                                    + this.gridView1.Columns["FCSTEP"].Width
                                    + this.gridView1.Columns["FCCODE"].Width
                                    + this.gridView1.Columns["FCREFNO"].Width
                                    + this.gridView1.Columns["FDDATE"].Width
                                    + this.gridView1.Columns["NAMT"].Width
                                    + this.gridView1.Columns["CREFTO"].Width;

            int intNewWidth = this.grdBrowView.Width - intColWidth - 35;
            this.gridView1.Columns["CQNCOOR"].Width = (intNewWidth > 0 ? intNewWidth : 70);
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            this.pmSetSortKey(this.gridView1.SortedColumns[0].FieldName, false);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["ftDateTime"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["ftLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
                //this.txtFooter.Text = "วันที่สร้าง : " + Convert.ToDateTime(dtrBrow["ftDateTime"]).ToString("dd/MM/yy hh:mm:ss");
                //this.txtFooter.Text += "\r\nแก้ไขล่าสุดวันที่ : " + Convert.ToDateTime(dtrBrow["ftLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
            }
            else
            {
                this.txtFooter.Text = "";
            }
        }

        private void frmInvoice_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.RefreshData();
            }

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
            }

            this.mstrSortKey = inColumn.ToUpper();
            this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
            this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
        }

        private void pmRefreshBrowView()
        {
            this.mintSaveBrowViewRowIndex = this.gridView1.FocusedRowHandle;
            this.pmSetBrowView();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            if (this.gridView1.RowCount > 0)
                this.gridView1.FocusedRowHandle = 0;

        }

        private void pmLoadEditPage()
        {

            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[xd_PAGE_BROWSE].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
            
            this.mbllCanEdit = true;
            this.mbllCanEditHeadOnly = false;
            try
            {
                this.pmBlankFormData();
                if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {

                    this.pmLoadFormData();
                    this.mbllCanEdit = this.pmCanEdit(this.dtsDataEnv.Tables[this.mstrHTable].Rows[0], "E");
                }
                this.txtCode.Focus();
            }
            catch (Exception ex)
            {
#if DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }
        }

        private void pmBlankFormData()
        {
            DataRow dtrTemRow = null;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mstrEditRowID = "";
            this.mstrRefToRowID = "";
            this.mstrOldRefToRowID = "";

            this.mbllIsCrDrChgQty = true;

            //Page Edit 1
            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcProd"];
            this.mstrStep = (this.mbllWaitForApprove == true ? SysDef.gc_REF_STEP_WAIT_APPROVE : SysDef.gc_REF_STEP_CUT_STOCK);
            //this.mstrDOStep = SysDef.gc_REF_STEP_CUT_STOCK;
            this.mstrAtStep = SysDef.gc_ATSTEP_VOUCHER_WAIT;

            this.mstrLastQcCoor = "";
            this.mstrLastQsCoor = "";

            this.txtQcCoor.Tag = "";
            this.txtQcCoor.Text = "";
            this.txtQsCoor.Text = "";
            this.mstrCoorDiscStr = "";

            this.mstrLastQcCoor = this.txtQcCoor.Text;
            this.mstrLastQsCoor = this.txtQsCoor.Text;

            this.txtRefTo.Text = "";
            this.txtCode.Text = "";
            this.txtRefNo.Text = "";
            this.txtDate.DateTime = DateTime.Now;

            this.txtVatType.Tag = "";
            this.txtVatType.Text = "";
            this.txtVatRate.Text = "";
            this.txtVatIsOut.Text = App.ActiveCorp.SaleVATIsOut;

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where 0=1", ref strErrorMsg);

            pobjSQLUtil.SetPara(new object[1] { this.mstrDefaVATType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVatType", "VatType", "select fcCode,fnRate from VatType where fcCode = ?", ref strErrorMsg))
            {
                dtrTemRow = this.dtsDataEnv.Tables["QVatType"].Rows[0];
                decimal decVatRate = Convert.ToDecimal(dtrTemRow["fnRate"]);
                this.txtVatType.Tag = this.mstrDefaVATType;
                this.mdecVatRate = decVatRate;
                this.txtVatType.Text = dtrTemRow["fcCode"].ToString().TrimEnd();
                this.txtVatRate.Text = decVatRate.ToString("###.00") + "%";
            }

            this.txtQcDept.Tag = App.ActiveCorp.DefaultSectID;
            this.txtQcDept.Text = "";
            this.mstrDivision = "";
            pobjSQLUtil.SetPara(new object[1] { this.txtQcDept.Tag });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode, fcName, fcDept from Sect where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemRow = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtQcDept.Text = dtrTemRow["fcCode"].ToString().TrimEnd();
                this.mstrDivision = dtrTemRow["fcDept"].ToString();

                //this.dtsDataEnv.Tables[this.mstrTemPd].Columns["cDept"].DefaultValue = this.txtQcDept.Tag;
                //this.dtsDataEnv.Tables[this.mstrTemPd].Columns["cQcDept"].DefaultValue = this.txtQcDept.Text.TrimEnd();
            }

            this.txtQcJob.Tag = App.ActiveCorp.DefaultJobID;
            this.txtQcJob.Text = "";
            this.mstrJob = App.ActiveCorp.DefaultJobID;
            this.mstrProj = App.ActiveCorp.DefaultProjectID;
            pobjSQLUtil.SetPara(new object[1] { this.mstrJob });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode, fcName, fcProj from JOB where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemRow = this.dtsDataEnv.Tables["QJob"].Rows[0];
                this.txtQcJob.Tag = this.mstrJob;
                this.txtQcJob.Text = dtrTemRow["fcCode"].ToString().TrimEnd();
                this.mstrProj = dtrTemRow["fcProj"].ToString();

                //this.dtsDataEnv.Tables[this.mstrTemPd].Columns["cJob"].DefaultValue = this.mstrJob;
                //this.dtsDataEnv.Tables[this.mstrTemPd].Columns["cQcJob"].DefaultValue = this.txtQcJob.Text.TrimEnd();
            }

            this.txtQcCurrency.Tag = "";
            this.txtQcCurrency.Text = "";
            this.txtQnCurrency.Text = "";
            this.txtXRate.Value = 1;
            this.txtEditDet1.Text = "N";
            //this.cmbVatIsOut.SelectedIndex = (App.ActiveCorp.SaleVATIsOut == "Y" ? 0 : 1);

            this.txtTotPdQty.Value = 0;
            this.txtTotPdAmt.Value = 0;
            this.txtDiscount.Text = "";
            this.txtDiscountAmt.Value = 0;
            this.txtAmt.Value = 0;
            this.txtVatAmt.Value = 0;
            this.txtGrossAmt.Value = 0;
            this.txtDiscountAmtKe.Value = 0;
            this.txtAmtKe.Value = 0;
            this.txtVatAmtKe.Value = 0;
            this.txtGrossAmtKe.Value = 0;

            this.mstrSEmpl = "";
            this.mdecSaleComm = 0;
            this.mintCredTerm = 0;
            this.mdttDueDate = DBEnum.NullDate;
            this.mstrRecvMan = "";
            this.mintPromote = 0;
            this.mstrDebtCode = "";
            this.mdttDebtDate = DBEnum.NullDate;
            this.mstrVatCoor = "";
            this.mstrInvDetail = "";
            this.mstrHasReturn = "Y";
            this.mdttReceDate = DateTime.Now;
            this.mstrOldVatSeq = "";
            this.mstrVatSeq = "";
            this.mstrDeliCoor = "";
            this.mstrWHouse = this.mstrDefaWHouse;
            this.mstrTradeTrm = "";

            this.mstrRemarkH1 = "";
            this.mstrRemarkH2 = "";
            this.mstrRemarkH3 = "";
            this.mstrRemarkH4 = "";
            this.mstrRemarkH5 = "";
            this.mstrRemarkH6 = "";
            this.mstrRemarkH7 = "";
            this.mstrRemarkH8 = "";
            this.mstrRemarkH9 = "";
            this.mstrRemarkH10 = "";

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Clear();

        }

        private void pmLoadFormData()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {

                this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

                object[] pAPara = new object[1] { this.mstrEditRowID };
                pobjSQLUtil.SetPara(pAPara);
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where fcSkid=?", ref strErrorMsg))
                {

                    DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];

                    this.mstrStep = dtrGLRef["fcStep"].ToString();
                    this.mstrAtStep = dtrGLRef["fcAtStep"].ToString();

                    this.txtCode.Text = dtrGLRef["fcCode"].ToString().TrimEnd();
                    this.txtRefNo.Text = dtrGLRef["fcRefNo"].ToString().TrimEnd();
                    this.txtDate.DateTime = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;
                    this.txtQcDept.Tag = dtrGLRef["fcSect"].ToString().TrimEnd();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcDept.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from " + MapTable.Table.Department + " where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcDept.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrGLRef["fcJob"].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcJob.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from " + MapTable.Table.Job + " where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcCoor.Tag = dtrGLRef["fcCoor"].ToString();
                    this.mstrDeliCoor = dtrGLRef["fcDeliCoor"].ToString();
                    this.mstrVatCoor = dtrGLRef["fcVatCoor"].ToString();

                    pobjSQLUtil.SetPara(new object[1] { this.txtQcCoor.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.txtQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                        this.txtQsCoor.Text = dtrCoor["fcSName"].ToString().TrimEnd();

                        this.mstrLastQcCoor = this.txtQcCoor.Text;
                        this.mstrLastQsCoor = this.txtQsCoor.Text;
                    
                    }
                    if (this.mstrVatCoor == string.Empty)
                    {
                        this.mstrVatCoor = dtrGLRef["fcCoor"].ToString();
                    }
                    if (this.mstrDeliCoor == string.Empty)
                    {
                        this.mstrDeliCoor = dtrGLRef["fcCoor"].ToString();
                    }

                    decimal decVatRate = Convert.ToDecimal(dtrGLRef["fnVatRate"]);
                    this.txtVatType.Tag = dtrGLRef["fcVatType"].ToString();
                    this.mdecVatRate = decVatRate;
                    this.txtVatType.Text = dtrGLRef["fcVatType"].ToString();
                    this.txtVatRate.Text = decVatRate.ToString("###.00") + "%";

                    this.txtVatIsOut.Text = dtrGLRef["fcVatIsOut"].ToString();

                    this.mdecAmtStd = Convert.ToDecimal(dtrGLRef["fnAmt"]);
                    this.mdecDiscAmtStd = Convert.ToDecimal(dtrGLRef["fnDiscAmt1"]);
                    this.mdecVATAmtStd = Convert.ToDecimal(dtrGLRef["fnVatAmt"]);

                    this.txtTotPdAmt.Value = 0;
                    this.txtDiscount.Text = dtrGLRef["fcDiscStr"].ToString().TrimEnd();
                    this.txtDiscountAmt.Value = Convert.ToDecimal(dtrGLRef["fnDiscAmtK"]);
                    this.txtAmt.Value = Convert.ToDecimal(dtrGLRef["fnAmtKe"]);
                    this.txtVatAmt.Value = Convert.ToDecimal(dtrGLRef["fnVatAmtKe"]);

                    this.mstrSEmpl = dtrGLRef["fcEmpl"].ToString();
                    this.mintCredTerm = Convert.ToInt32(dtrGLRef["fnCredTerm"]);
                    this.mdttDueDate = (Convert.IsDBNull(dtrGLRef["fdDueDate"]) ? DBEnum.NullDate : Convert.ToDateTime(dtrGLRef["fdDueDate"]));
                    this.mdttReceDate = (Convert.IsDBNull(dtrGLRef["fdRecedate"]) ? this.txtDate.DateTime : Convert.ToDateTime(dtrGLRef["fdRecedate"]));
                    this.mdttDebtDate = (Convert.IsDBNull(dtrGLRef["fdDebtDate"]) ? DBEnum.NullDate : Convert.ToDateTime(dtrGLRef["fdDebtDate"]));
                    this.mstrDebtCode = dtrGLRef["fcDebtCode"].ToString().TrimEnd();
                    this.mstrRecvMan = dtrGLRef["fcRecvMan"].ToString().TrimEnd();
                    this.mintPromote = Convert.ToInt32(dtrGLRef["fcPromote"].ToString().TrimEnd() == string.Empty ? "0" : dtrGLRef["fcPromote"].ToString().TrimEnd());
                    this.mstrHasReturn = dtrGLRef["fcHasRet"].ToString();
                    this.mstrVatSeq = dtrGLRef["fcVatSeq"].ToString().TrimEnd();
                    this.mstrOldVatSeq = this.mstrVatSeq;
                    this.mstrTradeTrm = dtrGLRef["fcTradeTrm"].ToString();

                    this.pmLoadRemark(dtrGLRef);
                    this.pmLoadProdTran();
                    this.pmSetRefToCodeList();

                    //Default RefTo Document
                    if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count > 0)
                    {
                        DataRow dtrTemRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows[0];
                        this.mstrRefToRefType = dtrTemRefTo["cRefType"].ToString();
                        this.mstrRefToBook = dtrTemRefTo["cBook"].ToString();
                        this.mstrRefToRowID = dtrTemRefTo["cRowID"].ToString();
                        this.mRefToDocType = BusinessEnum.GetDocEnum(this.mstrRefToRefType);
                    }

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldRefNo = this.txtRefNo.Text;
        }

        private void pmLoadRemark(DataRow inSource)
        {
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData2"]) ? "" : inSource["fmMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData3"]) ? "" : inSource["fmMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData4"]) ? "" : inSource["fmMemData4"].ToString().TrimEnd());
            if (inSource["fmMemData5"] != null)
                strRemark += (Convert.IsDBNull(inSource["fmMemData5"]) ? "" : inSource["fmMemData5"].ToString().TrimEnd());

            this.mstrInvDetail = BizRule.GetMemData(strRemark, "Det");
            this.mstrRemarkH1 = BizRule.GetMemData(strRemark, "Rem");
            this.mstrRemarkH2 = BizRule.GetMemData(strRemark, "Rm2");
            this.mstrRemarkH3 = BizRule.GetMemData(strRemark, "Rm3");
            this.mstrRemarkH4 = BizRule.GetMemData(strRemark, "Rm4");
            this.mstrRemarkH5 = BizRule.GetMemData(strRemark, "Rm5");
            this.mstrRemarkH6 = BizRule.GetMemData(strRemark, "Rm6");
            this.mstrRemarkH7 = BizRule.GetMemData(strRemark, "Rm7");
            this.mstrRemarkH8 = BizRule.GetMemData(strRemark, "Rm8");
            this.mstrRemarkH9 = BizRule.GetMemData(strRemark, "Rm9");
            this.mstrRemarkH10 = BizRule.GetMemData(strRemark, "RmA");
        }

        private void pmLoadProdTran()
        {
            int intRecNo = 0;
            string strErrorMsg = "";
            string strSQLStrRefProd = "select * from RefProd where RefProd.fcGLRef = ? order by RefProd.fcSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", strSQLStrRefProd, ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    intRecNo++;
                    if (this.mdecSaleComm == 0)
                    {
                        this.mdecSaleComm = Convert.ToDecimal(dtrRefProd["fnCommissi"]);
                    }
                    this.pmRepl1RecTemRefProd(intRecNo, dtrRefProd);
                }
                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();
                this.gridView2.MoveFirst();
            }
        }

        private void pmRepl1RecTemRefProd(int inRecNo, DataRow inRefProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

            //dtrTemPd["cStep"] = inRefProd["fcStep"].ToString();
            dtrTemPd["cRowID"] = inRefProd["fcSkid"].ToString();
            //dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cProd"] = inRefProd["fcProd"].ToString();
            dtrTemPd["cLastProd"] = inRefProd["fcProd"].ToString();
            dtrTemPd["cRefPdType"] = inRefProd["fcRefPdTyp"].ToString();
            dtrTemPd["cLastRefPdType"] = inRefProd["fcRefPdTyp"].ToString();
            dtrTemPd["cFormula"] = inRefProd["fcFormulas"].ToString();
            dtrTemPd["cLastFormula"] = inRefProd["fcFormulas"].ToString();
            dtrTemPd["cPFormula"] = inRefProd["fcPFormula"].ToString();
            dtrTemPd["cRootSeq"] = inRefProd["fcRootSeq"].ToString();
            dtrTemPd["cPdType"] = inRefProd["fcProdType"].ToString();
            dtrTemPd["cLastPdType"] = inRefProd["fcProdType"].ToString();
            dtrTemPd["cDept"] = inRefProd["fcSect"].ToString();
            dtrTemPd["cDivision"] = inRefProd["fcDept"].ToString();
            dtrTemPd["cJob"] = inRefProd["fcJob"].ToString();
            dtrTemPd["cProject"] = inRefProd["fcProj"].ToString();
            dtrTemPd["cRemark1"] = (Convert.IsDBNull(inRefProd["fmReMark"]) ? "" : inRefProd["fmReMark"].ToString().TrimEnd());
            dtrTemPd["cRemark2"] = (Convert.IsDBNull(inRefProd["fmReMark2"]) ? "" : inRefProd["fmReMark2"].ToString().TrimEnd());
            dtrTemPd["cRemark3"] = (Convert.IsDBNull(inRefProd["fmReMark3"]) ? "" : inRefProd["fmReMark3"].ToString().TrimEnd());
            dtrTemPd["cRemark4"] = (Convert.IsDBNull(inRefProd["fmReMark4"]) ? "" : inRefProd["fmReMark4"].ToString().TrimEnd());
            dtrTemPd["cRemark5"] = (Convert.IsDBNull(inRefProd["fmReMark5"]) ? "" : inRefProd["fmReMark5"].ToString().TrimEnd());
            dtrTemPd["cRemark6"] = (Convert.IsDBNull(inRefProd["fmReMark6"]) ? "" : inRefProd["fmReMark6"].ToString().TrimEnd());
            dtrTemPd["cRemark7"] = (Convert.IsDBNull(inRefProd["fmReMark7"]) ? "" : inRefProd["fmReMark7"].ToString().TrimEnd());
            dtrTemPd["cRemark8"] = (Convert.IsDBNull(inRefProd["fmReMark8"]) ? "" : inRefProd["fmReMark8"].ToString().TrimEnd());
            dtrTemPd["cRemark9"] = (Convert.IsDBNull(inRefProd["fmReMark9"]) ? "" : inRefProd["fmReMark9"].ToString().TrimEnd());
            dtrTemPd["cRemark10"] = (Convert.IsDBNull(inRefProd["fmReMark10"]) ? "" : inRefProd["fmReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inRefProd["fcUm"].ToString();
            dtrTemPd["nQty"] = Convert.ToDecimal(inRefProd["fnQty"]);
            dtrTemPd["nStQty"] = Convert.ToDecimal(inRefProd["fnStQty"]);
            dtrTemPd["nUOMQty"] = Convert.ToDecimal(inRefProd["fnUmQty"]);
            dtrTemPd["nStUOMQty"] = Convert.ToDecimal(inRefProd["fnStUmQty"]);
            dtrTemPd["nPrice"] = Convert.ToDecimal(inRefProd["fnPriceKe"]);
            dtrTemPd["nLastPrice"] = Convert.ToDecimal(inRefProd["fnPriceKe"]);
            dtrTemPd["cDiscStr"] = inRefProd["fcDiscStr"].ToString().TrimEnd();
            dtrTemPd["nDiscAmt"] = Convert.ToDecimal(inRefProd["fnDiscAmt"]);
            dtrTemPd["nQtyPerMFM"] = 1;

            dtrTemPd["nLastQty"] = Convert.ToDecimal(inRefProd["fnQty"]);

            dtrTemPd["nOQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
            dtrTemPd["nOUMQty"] = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
            dtrTemPd["nOStQty"] = Convert.ToDecimal(dtrTemPd["nStQty"]);
            dtrTemPd["nOStUMQty"] = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);

            dtrTemPd["cLot"] = inRefProd["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inRefProd["fcWHouse"].ToString();

            dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);

            if (dtrTemPd["cPFormula"].ToString().TrimEnd() != "")
            {
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cPFormula"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QFormula", "FORMULA", "select FCCODE,FCNAME,FCUM from FORMULAS where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcPFormula"] = this.dtsDataEnv.Tables["QFormula"].Rows[0]["fcCode"].ToString().TrimEnd();
                }
            }

            if (dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_FORMULA)
            {
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cFormula"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QFormula", "FORMULA", "select FCCODE,FCNAME,FCUM from FORMULAS where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QFormula"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QFormula"].Rows[0]["fcName"].ToString().TrimEnd();
                    dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                    dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();

                    if (BizRule.IsRootFormula(dtrTemPd["cPFormula"].ToString()))
                    {
                        dtrTemPd["cQcPFormula"] = this.dtsDataEnv.Tables["QFormula"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                }
            }
            else
            {
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM,FCCTRLSTOC from PROD where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cCtrlStoc"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCtrlStoc"].ToString();
                    dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                    dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                    dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
                    dtrTemPd["cStUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
                }
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHouse"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select FCCODE,FCNAME from WHouse where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            this.mstrWHouse = dtrTemPd["cWHouse"].ToString();
            //this.mstrLastQcWHouse = this.txtQcWHouse.Text;

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cDept"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select FCCODE,FCNAME from SECT where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cJob"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select FCCODE,FCNAME from JOB where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cRowID"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QXaRefProd", "xaRefProd", "select cWHLoca from xaRefProd where cRefProd = ?", ref strErrorMsg))
            {
                dtrTemPd["cWHLoca"] = this.dtsDataEnv.Tables["QXaRefProd"].Rows[0]["cWHLoca"].ToString();
            }

            //Load NoteCut
            string strNoteCut = "select * from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?";
            if ((pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID"].ToString() })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg)))
            {
                //this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Clear();
                DataRow[] dtaRefToRow = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Select("cRowID = '" + this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildH"].ToString() + "'");
                if (dtaRefToRow.Length == 0)
                {
                    //string strFldList = "{0}.fcRefNo as fcRefNo, {0}.fdDate as fdDate";
                    string strFldList = "{0}.fcRefNo as fcRefNo, {0}.fcCode as fcCode, {0}.fdDate as fdDate";
                    strFldList += ", Book.fcSkid as fcBook, Book.fcRefType as fcRefType, Book.fcCode as fcQcBook";

                    string strSQLRefToStr = string.Format("select " + strFldList + " from {0}, Book where {0}.fcSkid = ? and {0}.fcBook = Book.fcSkid ", new object[] { this.mstrRefToTab });
                    string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildH"].ToString();
                    if ((pobjSQLUtil.SetPara(new object[] { strRefToHRowID })
                        && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, strSQLRefToStr, ref strErrorMsg)))
                    {
                        DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        DataRow dtrTemRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].NewRow();

                        this.mstrRefToRefType = dtrRefTo["fcRefType"].ToString();
                        this.mRefToDocType = BusinessEnum.GetDocEnum(this.mstrRefToRefType);

                        dtrTemRefTo["cRowID"] = strRefToHRowID;
                        dtrTemRefTo["cRefType"] = this.mstrRefToRefType;
                        dtrTemRefTo["cBook"] = dtrRefTo["fcBook"].ToString();
                        dtrTemRefTo["cQcBook"] = dtrRefTo["fcQcBook"].ToString();
                        //dtrTemRefTo["cCode"] = dtrRefTo["fcRefNo"].ToString();
                        dtrTemRefTo["cCode"] = dtrRefTo["fcCode"].ToString();
                        dtrTemRefTo["dDate"] = Convert.ToDateTime(dtrRefTo["fdDate"]);
                        this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Add(dtrTemRefTo);
                    }
                }

                dtrTemPd["cXRefToProd"] = dtrTemPd["cProd"].ToString();
                dtrTemPd["cXRefToRefType"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildTyp"].ToString();
                dtrTemPd["cXRefToRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildI"].ToString();
                dtrTemPd["cXRefToHRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildH"].ToString();
                dtrTemPd["cRefToRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildI"].ToString();
                dtrTemPd["cRefToHRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["fcChildH"].ToString();
                dtrTemPd["cRefToCode"] = this.pmGetRefToCode(dtrTemPd["cRefToHRowID"].ToString());
            }

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);

        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private bool pmCanEdit(DataRow inGLRef, string inEditType)
        {
            string strMsg1 = "แก้ไข";
            switch (inEditType.ToUpper())
            {
                case "E":
                    strMsg1 = "แก้ไข";
                    break;
                case "D":
                    strMsg1 = "ลบ";
                    break;
            }
            bool bllResult = true;
            if (inGLRef["fcStep"].ToString() == SysDef.gc_REF_STEP_PAY
                && this.mstrIsCash != "Y"
                && (Convert.ToDecimal(inGLRef["fnAmt"]) + Convert.ToDecimal(inGLRef["fnVatAmt"])) != 0)
            {
                MessageBox.Show("ไม่อนุญาตให้" + strMsg1 + "เนื่องจากใบแจ้งหนี้ฉบับนี้ชำระเงินครบแล้ว", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bllResult = false;
            }
            else if (inGLRef["fcAtStep"].ToString() != SysDef.gc_ATSTEP_VOUCHER_WAIT)
            {
                //MessageBox.Show("ไม่อนุญาตให้"+strMsg1+"เนื่องจากใบแจ้งหนี้ฉบับนี้ลงรายวันแล้ว" + "\nหากต้องการแก้ให้เข้าไปแก้ไขที่ระบบ Forma", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show("ไม่อนุญาตให้" + strMsg1 + "รายการสินค้า และจำนวนเงินเนื่องจากใบแจ้งหนี้ฉบับนี้ลงรายวันแล้ว" + "\nหากต้องการแก้ให้เข้าไปแก้ไขที่ระบบ Forma", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.mbllCanEditHeadOnly = true;
                bllResult = true;
            }
            return bllResult;
        }

        private string pmGetRefToCode(string inRefToHRowID)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLRefToStr = "select fcRefNo from " + this.mstrRefToTab + " where fcSkid = ?";
            string strRefToCode = "";
            if ((pobjSQLUtil.SetPara(new object[] { inRefToHRowID })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, strSQLRefToStr, ref strErrorMsg)))
            {
                DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                strRefToCode = dtrRefTo["fcRefNo"].ToString();
            }
            return strRefToCode;
        }

        private void pmCreateTem()
        {
            //รายการสินค้า
            this.dtsDataEnv.CaseSensitive = true;

            DataTable dtbTemPd = new DataTable(this.mstrTemPd);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cStep", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark3", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark4", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark5", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark6", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark7", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark8", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark9", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark10", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cDiscStr", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDiscAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nCostAmt", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("cDiscStr1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDiscAmt1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastDiscStr1", System.Type.GetType("System.String"));

            //dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"), "((nPrice-nDiscAmt1) * nQty) - nDiscAmt");
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDivision", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProject", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRefType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackDOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nDOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefTo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIsDelete", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns["nActRow"].AutoIncrement = true;

            //Save ค่า Qty ไว้ตอน Check Stock
            dtbTemPd.Columns.Add("cCtrlStoc", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOUmQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStUmQty", System.Type.GetType("System.Decimal"));

            //เรื่อง ชุดสินค้า
            dtbTemPd.Columns.Add("cRootSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQtyPerMFm", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastRefPdType", System.Type.GetType("System.String"));

            dtbTemPd.Columns["cRootSeq"].DefaultValue = "";
            dtbTemPd.Columns["cQcPFormula"].DefaultValue = "";
            dtbTemPd.Columns["cPFormula"].DefaultValue = "";
            dtbTemPd.Columns["nQtyPerMFm"].DefaultValue = 0;
            dtbTemPd.Columns["cLastFormula"].DefaultValue = "";
            dtbTemPd.Columns["cLastRefPdType"].DefaultValue = "";


            dtbTemPd.Columns["nRecNo"].DefaultValue = 0;
            dtbTemPd.Columns["cCtrlStoc"].DefaultValue = "0";
            dtbTemPd.Columns["cRefPdType"].DefaultValue = SysDef.gc_REFPD_TYPE_PRODUCT;
            dtbTemPd.Columns["cStep"].DefaultValue = SysDef.gc_STEP_CREATED;
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStUOMQty"].DefaultValue = 0;
            dtbTemPd.Columns["nPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nLastPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cDiscStr"].DefaultValue = "";
            dtbTemPd.Columns["nDiscAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cDiscStr1"].DefaultValue = "";
            dtbTemPd.Columns["nDiscAmt1"].DefaultValue = 0;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPd.Columns["cUOMStd"].DefaultValue = "";
            dtbTemPd.Columns["cRemark1"].DefaultValue = "";
            dtbTemPd.Columns["cRemark2"].DefaultValue = "";
            dtbTemPd.Columns["cRemark3"].DefaultValue = "";
            dtbTemPd.Columns["cRemark4"].DefaultValue = "";
            dtbTemPd.Columns["cRemark5"].DefaultValue = "";
            dtbTemPd.Columns["cRemark6"].DefaultValue = "";
            dtbTemPd.Columns["cRemark7"].DefaultValue = "";
            dtbTemPd.Columns["cRemark8"].DefaultValue = "";
            dtbTemPd.Columns["cRemark9"].DefaultValue = "";
            dtbTemPd.Columns["cRemark10"].DefaultValue = "";
            dtbTemPd.Columns["cLot"].DefaultValue = "";
            dtbTemPd.Columns["cWHouse"].DefaultValue = "";
            dtbTemPd.Columns["cQcWHouse"].DefaultValue = "";
            dtbTemPd.Columns["cQnWHouse"].DefaultValue = "";
            dtbTemPd.Columns["nBackQty"].DefaultValue = 0;
            dtbTemPd.Columns["nBackDOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nDOQty"].DefaultValue = 0;
            dtbTemPd.Columns["cDept"].DefaultValue = "";
            dtbTemPd.Columns["cQcDept"].DefaultValue = "";
            dtbTemPd.Columns["cQnDept"].DefaultValue = "";
            dtbTemPd.Columns["cDivision"].DefaultValue = "";
            dtbTemPd.Columns["cJob"].DefaultValue = "";
            dtbTemPd.Columns["cQcJob"].DefaultValue = "";
            dtbTemPd.Columns["cQnJob"].DefaultValue = "";
            dtbTemPd.Columns["cProject"].DefaultValue = "";
            dtbTemPd.Columns["cRefToCode"].DefaultValue = "";
            dtbTemPd.Columns["cRefToRowID"].DefaultValue = "";
            dtbTemPd.Columns["cRefToHRowID"].DefaultValue = "";
            dtbTemPd.Columns["cXRefToProd"].DefaultValue = "";
            dtbTemPd.Columns["cXRefToRowID"].DefaultValue = "";
            dtbTemPd.Columns["cXRefToHRowID"].DefaultValue = "";
            dtbTemPd.Columns["cXRefToRefType"].DefaultValue = "";
            dtbTemPd.Columns["cLastPdType"].DefaultValue = "";
            dtbTemPd.Columns["cLastProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQnProd"].DefaultValue = "";
            dtbTemPd.Columns["cWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["nOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOUmQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStUmQty"].DefaultValue = 0;
            dtbTemPd.Columns["nCostAmt"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemRefTo = new DataTable(xd_ALIAS_TEMREFTO);
            dtbTemRefTo.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cRefType", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("IsDelete", System.Type.GetType("System.Boolean"));
            dtbTemRefTo.Columns.Add("cStep", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cPlant", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cQcPlant", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cBranch", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cQcBranch", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cBook", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cQcBook", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemRefTo.Columns.Add("fcQnCoor", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));

            dtbTemRefTo.Columns["IsDelete"].DefaultValue = false;
            this.dtsDataEnv.Tables.Add(dtbTemRefTo);

            DataTable dtbTemBrowRefTo = new DataTable(xd_ALIAS_BROW_REFTO);
            dtbTemBrowRefTo.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("cRefType", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("cStat", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemBrowRefTo.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("cSign", System.Type.GetType("System.String"));
            dtbTemBrowRefTo.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemBrowRefTo.Columns.Add("nPayAmt", System.Type.GetType("System.Decimal"));

            this.dtsDataEnv.Tables.Add(dtbTemBrowRefTo);

            this.dtsDataEnv.Tables[this.mstrTemPd].TableNewRow += new DataTableNewRowEventHandler(frmInvoice_TableNewRow);
            //this.dtsDataEnv.Tables[this.mstrTemPd].ColumnChanged += new DataColumnChangeEventHandler(frmInvoice_ColumnChanged);

        }

        private void pmSetPropGrdTemPd()
        {

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;
            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            this.gridView2.Columns["nRecNo"].VisibleIndex = 0;
            //this.gridView2.Columns["cRefPdType"].VisibleIndex = 1;
            //this.gridView2.Columns["cPdType"].VisibleIndex = 1;
            this.gridView2.Columns["cQcProd"].VisibleIndex = 2;
            this.gridView2.Columns["cQnProd"].VisibleIndex = 3;
            this.gridView2.Columns["cRemark1"].VisibleIndex = 4;
            //this.gridView2.Columns["cLot"].VisibleIndex = 5;
            this.gridView2.Columns["nQty"].VisibleIndex = 6;
            this.gridView2.Columns["cQnUOM"].VisibleIndex = 7;
            this.gridView2.Columns["nPrice"].VisibleIndex = 8;
            this.gridView2.Columns["cDiscStr"].VisibleIndex = 9;
            this.gridView2.Columns["nAmt"].VisibleIndex = 10;

            this.gridView2.Columns["nRecNo"].Visible = true;
            //this.gridView2.Columns["cRefPdType"].Visible = true;
            //this.gridView2.Columns["cPdType"].Visible = true;
            this.gridView2.Columns["cQcProd"].Visible = true;
            this.gridView2.Columns["cRemark1"].Visible = true;
            //this.gridView2.Columns["cLot"].Visible = true;
            this.gridView2.Columns["nQty"].Visible = true;
            this.gridView2.Columns["cQnUOM"].Visible = true;
            this.gridView2.Columns["nPrice"].Visible = true;
            this.gridView2.Columns["cDiscStr"].Visible = true;
            this.gridView2.Columns["nAmt"].Visible = true;
            this.gridView2.Columns["cQnProd"].Visible = true;
            this.gridView2.Columns["cQcDept"].Visible = false;	//(this.mstrSaleOrBuy == "P");
            this.gridView2.Columns["cQcJob"].Visible = false;	//(this.mstrSaleOrBuy == "P");
            //this.gridView2.Columns["cRefToCode"].Visible = ("SO,PO".IndexOf(this.mstrRefType) > -1);
            this.gridView2.Columns["cRefTo"].Visible = false;

            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.SystemColors.Control;
            this.gridView2.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nAmt"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cRefToCode"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;

            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nAmt"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cRefToCode"].OptionsColumn.AllowFocus = false;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cRefPdType"].Caption = "P/F";
            this.gridView2.Columns["cPdType"].Caption = "T";
            this.gridView2.Columns["cQcProd"].Caption = "รหัสสินค้า";
            this.gridView2.Columns["cQnProd"].Caption = "ชื่อสินค้า";
            this.gridView2.Columns["cRemark1"].Caption = "หมายเหตุ";
            this.gridView2.Columns["cLot"].Caption = "Lot";
            this.gridView2.Columns["nQty"].Caption = "จำนวน";
            this.gridView2.Columns["cQnUOM"].Caption = "หน่วย";
            this.gridView2.Columns["nPrice"].Caption = "ราคา/หน่วย";
            this.gridView2.Columns["cDiscStr1"].Caption = "ส่วนลด/หน่วย";
            this.gridView2.Columns["cDiscStr"].Caption = "ส่วนลด";
            this.gridView2.Columns["nAmt"].Caption = "มูลค่า";
            this.gridView2.Columns["cQcDept"].Caption = "แผนก";
            this.gridView2.Columns["cQcJob"].Caption = "Job";
            this.gridView2.Columns["cRefToCode"].Caption = "#เอกสารอ้างอิง";
            this.gridView2.Columns["cRefTo"].Caption = "";
            //this.grdTemPd.FrozenColumns = 3;

            this.gridView2.Columns["nRecNo"].Width = 5;
            this.gridView2.Columns["cRefPdType"].Width = 5;
            this.gridView2.Columns["cPdType"].Width = 5;
            this.gridView2.Columns["cQcProd"].Width = 35;
            this.gridView2.Columns["cRemark1"].Width = 30;
            this.gridView2.Columns["cLot"].Width = 20;
            this.gridView2.Columns["nQty"].Width = 30;
            this.gridView2.Columns["cQnUOM"].Width = 30;
            this.gridView2.Columns["nPrice"].Width = 35;
            this.gridView2.Columns["cDiscStr1"].Width = 30;
            this.gridView2.Columns["cDiscStr"].Width = 30;
            this.gridView2.Columns["nAmt"].Width = 30;
            this.gridView2.Columns["cQcDept"].Width = 20;
            this.gridView2.Columns["cQcJob"].Width = 20;
            this.gridView2.Columns["cRefToCode"].Width = 40;
            this.gridView2.Columns["cRefTo"].Width = 10;

            this.gridView2.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nPrice"].DisplayFormat.FormatString = "#,###,###.00";
            this.gridView2.Columns["nAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nAmt"].DisplayFormat.FormatString = "#,###,###.00";

            this.grcPdType.MaxLength = 1;
            this.grcQcProd.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_CODE;
            this.grcQnProd.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_NAME;

            this.gridView2.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView2.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView2.Columns["cQnProd"].ColumnEdit = this.grcQnProd;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark;
            this.gridView2.Columns["cLot"].ColumnEdit = this.grcLotWh;
            this.gridView2.Columns["nQty"].ColumnEdit = this.grcQty;

        }

        private void frmInvoice_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            DataRow dtrTemPd = e.Row;
            dtrTemPd["nRecNo"] = e.Row.Table.Rows.Count + 1;
            dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);
        }

        private void pmGotoBrowPage()
        {
            this.pmBlankFormData();

            this.pmSetPageStatus(false);
            this.pgfMainEdit.TabPages[xd_PAGE_BROWSE].PageEnabled = true;
            this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_BROWSE;
            this.grdBrowView.Focus();
        
        }

        private void pmFilterForm()
        {
            this.pmInitPopUpDialog("FILTER");
        }

        private bool pmInitPopUpDialog(string inDialogName)
        {
            return this.pmInitPopUpDialogAll(inDialogName, false, false);
        }

        private bool pmInitPopUpDialog(string inDialogName, bool inPara1)
        {
            return this.pmInitPopUpDialogAll(inDialogName, inPara1, false);
        }

        private bool pmInitPopUpDialogAll(string inDialogName, bool inPara1, bool inPara2)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        //this.pofrmGetCoor = new DialogForms.dlgGetCoor(this.mCoorType);
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor();
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                //case "JOB":
                //    if (this.pofrmGetJob == null)
                //    {
                //        this.pofrmGetJob = new DialogForms.dlgGetJob();
                //        this.pofrmGetJob.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetJob.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                //    }
                //    break;
                case "DEPT":
                    if (this.pofrmGetDept == null)
                    {
                        this.pofrmGetDept = new DialogForms.dlgGetDept();
                        this.pofrmGetDept.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDept.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WAREHOUSE":
                    if (this.pofrmGetWareHouse == null)
                    {
                        this.pofrmGetWareHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWareHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWareHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "FILTER":
                    using (Transaction.Common.dlgFilterOption1 dlgFilter = new Transaction.Common.dlgFilterOption1(this.mstrRefType))
                    {
                        if (this.mstrBranchID != string.Empty)
                            dlgFilter.BranchID = this.mstrBranchID;
                        if (this.mstrBookID != string.Empty)
                            dlgFilter.BookID = this.mstrBookID;

                        dlgFilter.BeginDate = this.mdttBrowBegDate;
                        dlgFilter.EndDate = this.mdttBrowEndDate;

                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;
                            this.mstrBranchID = dlgFilter.BranchID;
                            this.mstrBookID = dlgFilter.BookID;
                            this.mstrQcBook = dlgFilter.QcBook;
                            this.mdttBrowBegDate = dlgFilter.BeginDate.Date;
                            this.mdttBrowEndDate = dlgFilter.EndDate.Date;
                            
                            //this.lblTitle.Text = this.mstrRefTypeName.TrimEnd();

                            //this.mstrRefTypeName = dlgFilter.RefTypeName;
                            string strErrorMsg = "";
                            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                            pobjSQLUtil.SetPara(new object[1] { this.mstrBranchID });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select * from Branch where fcSkid = ?", ref strErrorMsg))
                            {
                                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                                string strWHField = (this.mstrSaleOrBuy == "S" ? "fcWHSale" : "fcWHBuy");
                                this.mstrDefaWHouse = dtrBranch[strWHField].ToString();
                                this.mdttLastClose = (!Convert.IsDBNull(dtrBranch["fdLastClos"]) ? Convert.ToDateTime(dtrBranch["fdLastClos"]) : DBEnum.NullDate);
                                this.mstrDefaVATType = App.ActiveCorp.SaleVATType;

                                pobjSQLUtil.SetPara(new object[1] { this.mstrBookID });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select * from Book where fcSkid = ?", ref strErrorMsg))
                                {
                                    DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];

                                    if (dtrBook["fcPrefix"].ToString().Trim() != string.Empty)
                                    {
                                        this.mstrBookPrefix = dtrBook["fcPrefix"].ToString().TrimEnd();
                                    }
                                    else
                                    {
                                        this.mstrBookPrefix = this.mstrRefType + this.mstrQcBook + "/";
                                    }

                                    this.mbllOrderOnly = dtrBook["fcCtrlOrd"].ToString() == "1";

                                    if (this.mbllOrderOnly)
                                    {
                                        MessageBox.Show("เล่มเอกสารนี้จะต้อง ค้นหาจาก Order เท่านั้น", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        this.gridView2.Columns["cQcProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
                                        this.gridView2.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;

                                        this.gridView2.Columns["cQcProd"].OptionsColumn.AllowFocus = false;
                                        this.gridView2.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
                                        this.gridView2.OptionsView.NewItemRowPosition= DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;

                                        this.barMainEdit.Items["INSPD"].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                                    }
                                    else
                                    {
                                        this.gridView2.Columns["cQcProd"].AppearanceCell.BackColor = System.Drawing.Color.White;
                                        this.gridView2.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.White;

                                        this.gridView2.Columns["cQcProd"].OptionsColumn.AllowFocus = true;
                                        this.gridView2.Columns["cQnProd"].OptionsColumn.AllowFocus = true;
                                        this.gridView2.OptionsView.NewItemRowPosition= DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
                                        this.barMainEdit.Items["INSPD"].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                                    }

                                    //string strVATType = dtrBook["fcVatType"].ToString();
                                    //if (strVATType.TrimEnd() != string.Empty)
                                    //	this.mstrDefaVATType = strVATType;

                                    //this.mdttBookLastLock = (!Convert.IsDBNull(dtrBook["fdLocked"]) ? Convert.ToDateTime(dtrBook["fdLocked"]) : DBEnum.NullDate);
                                    //pobjSQLUtil.SetPara(new object[1] {this.mstrBookID});
                                    //if ((dtrBook["fdLocked"].ToString().TrimEnd() != string.Empty)
                                    //	&& pobjSQLUtil.SetPara(new object[1] {dtrBook["fcAccBook"].ToString()})
                                    //	&& pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAccBook", "ACCBOOK", "select * from AccBook where fcSkid = ?", ref strErrorMsg))
                                    //{
                                    //	DataRow dtrAccBook = this.dtsDataEnv.Tables["QAccBook"].Rows[0];
                                    //	this.mdttAccBookLastLock = (!Convert.IsDBNull(dtrAccBook["fdLocked"]) ? Convert.ToDateTime(dtrAccBook["fdLocked"]) : DBEnum.NullDate);
                                    //}
                                }
                            }

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();
                        }
                    }
                    break;
                case "REFTO":
                    if (this.mbllIsInv)
                        this.pmPopGetRefTo();
                    else
                        this.pmRefToReqGLRef();
                    break;
                case "PDTYPE":
                    if (this.pofrmGetPdType == null)
                    {
                        this.pofrmGetPdType = new DialogForms.dlgGetProdType();
                        this.pofrmGetPdType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PRODUCT":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                //case "FORMULA":
                //    if (this.pofrmGetFormula == null)
                //    {
                //        this.pofrmGetFormula = new DialogForms.dlgGetFormula();
                //        this.pofrmGetFormula.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetFormula.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                //    }
                //    break;
                case "VATTYPE":
                    if (this.pofrmGetVatType == null)
                    {
                        this.pofrmGetVatType = new DialogForms.dlgGetVatType();
                        this.pofrmGetVatType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetVatType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "CURRENCY":
                    if (this.pofrmGetCurrency == null)
                    {
                        this.pofrmGetCurrency = new DialogForms.dlgGetCurrency();
                        this.pofrmGetCurrency.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCurrency.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "QTY_ITEM":
                    using (Transaction.Common.dlgGetQty dlg = new Transaction.Common.dlgGetQty())
                    {
                        DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                        decimal decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                        decimal decUOMQty = (Convert.IsDBNull(dtrTemPd["nUOMQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nUOMQty"]));
                        decimal decStkQty = (Convert.IsDBNull(dtrTemPd["nStQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nStQty"]));
                        decimal decStkUOMQty = (Convert.IsDBNull(dtrTemPd["nStUOMQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nStUOMQty"]));

                        string strUOM = (Convert.IsDBNull(dtrTemPd["cUOM"]) ? "" : dtrTemPd["cUOM"].ToString());
                        string strUOMStd = (Convert.IsDBNull(dtrTemPd["cUOMStd"]) ? "" : dtrTemPd["cUOMStd"].ToString());
                        string strStkUOM = (Convert.IsDBNull(dtrTemPd["cStUOM"]) ? "" : dtrTemPd["cStUOM"].ToString());
                        string strStkUOMStd = (Convert.IsDBNull(dtrTemPd["cStUOMStd"]) ? "" : dtrTemPd["cStUOMStd"].ToString());

                        dlg.Row = dtrTemPd;
                        //TODO: INV1
                        //dlg.SetParentForm(this);

                        dlg.Qty = decQty;
                        dlg.UMQty = decUOMQty;
                        dlg.UM = strUOM;
                        dlg.StdUM = strUOMStd;
                        dlg.StQty = decStkQty;
                        dlg.StUMQty = decUOMQty;
                        dlg.StUM = strUOM;
                        dlg.StStdUM = strUOMStd;
                        //กรณีลดหนี้/เพิ่มหนี้ที่เลือก Option ไม่ลด/เพิ่มจำนวน
                        dlg.IsFixUMQty = !this.mbllIsCrDrChgQty;

                        string strErrorMsg = "";
                        bool bllResult = false;
                        if (inPara1)
                        {
                            dlg.ShowDialog();
                            bllResult = (dlg.DialogResult == DialogResult.OK);
                        }
                        else
                        {
                            bllResult = this.ValidateQty(true, ref strErrorMsg);
                        }
                        //dlg.ShowDialog();
                        //if (dlg.DialogResult == DialogResult.OK)

                        if (bllResult)
                        {

                            dtrTemPd["nQty"] = dlg.Qty;
                            dtrTemPd["cUOM"] = dlg.UM;
                            dtrTemPd["nLastQty"] = dlg.Qty;
                            dtrTemPd["cQnUOM"] = dlg.UMName;
                            dtrTemPd["nUOMQty"] = dlg.UMQty;
                            dtrTemPd["nStQty"] = dlg.StQty;
                            dtrTemPd["cStUOM"] = dlg.StUM;
                            dtrTemPd["cStQnUOM"] = dlg.StUMName;
                            dtrTemPd["nStUOMQty"] = dlg.StUMQty;

                            dtrTemPd["nBackQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                            dtrTemPd["nPrice"] = (Convert.IsDBNull(dtrTemPd["nPrice"]) ? this.pmGetPrice() : dtrTemPd["nPrice"]);
                            dtrTemPd["nLastPrice"] = Convert.ToDecimal(dtrTemPd["nPrice"]);

                            if (dtrTemPd["cPFormula"].ToString().TrimEnd() != string.Empty)
                            {
                                if (BizRule.IsRootFormula(dtrTemPd["cPFormula"].ToString()))
                                {
                                    this.pmChgIQty(dtrTemPd, dtrTemPd["cRootSeq"].ToString());
                                    this.pmethChgMFMPrice(dtrTemPd["cRootSeq"].ToString(), false);
                                }
                                else
                                {
                                    if (dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
                                    {
                                        this.pmethChgMFMPrice(dtrTemPd["cRootSeq"].ToString(), true);
                                    }
                                }
                            }


                        }
                        else
                        {
                            dtrTemPd["nQty"] = Convert.ToDecimal(dtrTemPd["nLastQty"]);
                            return false;
                        }
                    }
                    break;
                //case "LOT_ITEM":
                //    using (Transaction.Common.dlgGetLotWHouse dlg = new Transaction.Common.dlgGetLotWHouse())
                //    {
                //        DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                //        string strWHouse = (dtrTemPd["cWHouse"].ToString().TrimEnd() == string.Empty ? this.mstrDefaWHouse : dtrTemPd["cWHouse"].ToString());
                //        string strWHLoca = dtrTemPd["cWHLoca"].ToString();
                //        int intActRow = Convert.ToInt32(dtrTemPd["nActRow"]);

                //        dlg.SetParentForm(this);
                //        dlg.RefType = this.mstrRefType;
                //        dlg.IsQInsertPdSer = ((SysDef.gc_GRP_REFTYPE_P_INV_PRODUCT + SysDef.gc_GRP_REFTYPE_P_CR_PRODUCT + SysDef.gc_GRP_REFTYPE_P_DR_PRODUCT).IndexOf(this.mstrRefType) > -1);
                //        //int intChk = (SysDef.gc_GRP_REFTYPE_P_INV_PRODUCT + SysDef.gc_GRP_REFTYPE_P_CR_PRODUCT + SysDef.gc_GRP_REFTYPE_P_DR_PRODUCT).IndexOf(this.mstrRefType);
                //        dlg.IsGenLot = ((SysDef.gc_GRP_REFTYPE_P_INV_PRODUCT + SysDef.gc_GRP_REFTYPE_P_CR_PRODUCT + SysDef.gc_GRP_REFTYPE_P_DR_PRODUCT).IndexOf(this.mstrRefType) > -1);
                //        dlg.IsShowDefaLot = (SysDef.gc_GRP_REFTYPE_S_INV_PRODUCT.IndexOf(this.mstrRefType) > -1);
                //        dlg.DocDate = this.txtDate.DateTime;
                //        dlg.ProdID = dtrTemPd["cProd"].ToString();
                //        dlg.ProdCode = dtrTemPd["cQcProd"].ToString().TrimEnd();

                //        dlg.SaleOrBuy = this.mstrSaleOrBuy;
                //        //dlg.BindData(this.dtsDataEnv, this.mPdSer, intActRow, this.mstrBranchID, dtrTemPd["cLot"].ToString(), strWHouse, strWHLoca);
                //        dlg.BindData(this.dtsDataEnv, null, intActRow, this.mstrBranchID, dtrTemPd["cLot"].ToString(), strWHouse, strWHLoca);
                //        dlg.StdUOM = dtrTemPd["cUOMStd"].ToString();

                //        dlg.ShowDialog();
                //        if (dlg.DialogResult == DialogResult.OK)
                //        {
                //            dtrTemPd["cLot"] = dlg.Lot;
                //            dtrTemPd["cWHouse"] = dlg.WareHouseID;

                //            }
                //        }
                //    }
                //    break;
                case "REMARK_ITEM":
                    //TODO: INVP1
                    //using (Transaction.Common.dlgGetItemRemark dlg = new Transaction.Common.dlgGetItemRemark())
                    //{
                    //    DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                    //    dlg.SetTitle("หมายเหตุรายการสินค้า");
                    //    dlg.Remark1 = dtrTemPd["cRemark1"].ToString();
                    //    dlg.Remark2 = dtrTemPd["cRemark2"].ToString();
                    //    dlg.Remark3 = dtrTemPd["cRemark3"].ToString();
                    //    dlg.Remark4 = dtrTemPd["cRemark4"].ToString();
                    //    dlg.Remark5 = dtrTemPd["cRemark5"].ToString();
                    //    dlg.Remark6 = dtrTemPd["cRemark6"].ToString();
                    //    dlg.Remark7 = dtrTemPd["cRemark7"].ToString();
                    //    dlg.Remark8 = dtrTemPd["cRemark8"].ToString();
                    //    dlg.Remark9 = dtrTemPd["cRemark9"].ToString();
                    //    dlg.Remark10 = dtrTemPd["cRemark10"].ToString();
                    //    dlg.ShowDialog();
                    //    if (dlg.DialogResult == DialogResult.OK)
                    //    {
                    //        dtrTemPd["cRemark1"] = dlg.Remark1;
                    //        dtrTemPd["cRemark2"] = dlg.Remark2;
                    //        dtrTemPd["cRemark3"] = dlg.Remark3;
                    //        dtrTemPd["cRemark4"] = dlg.Remark4;
                    //        dtrTemPd["cRemark5"] = dlg.Remark5;
                    //        dtrTemPd["cRemark6"] = dlg.Remark6;
                    //        dtrTemPd["cRemark7"] = dlg.Remark7;
                    //        dtrTemPd["cRemark8"] = dlg.Remark8;
                    //        dtrTemPd["cRemark9"] = dlg.Remark9;
                    //        dtrTemPd["cRemark10"] = dlg.Remark10;
                    //    }
                    //}
                   break;

                //case "SHOW_REFTO":
                //    using (Transaction.Common.dlgShowRefTo dlg = new Transaction.Common.dlgShowRefTo(this.mCoorType))
                //    {
                //        this.pmQueryRefTo();
                //        dlg.BindData(this.dtsDataEnv, xd_ALIAS_BROW_REFTO);
                //        dlg.ShowDialog();
                //    }
                //    break;
                case "GET_OTH":
                    using (Transaction.Common.Invoice.dlgGetInvOther dlg = new Transaction.Common.Invoice.dlgGetInvOther(this.mFormEditMode, this.mDocType, this.mstrSaleOrBuy))
                    {
                        dlg.BranchID = this.mstrBranchID;
                        dlg.InvoiceDate = this.txtDate.DateTime;
                        dlg.SEmpl = this.mstrSEmpl;
                        dlg.SaleCommission = this.mdecSaleComm;
                        dlg.CreditTerm = this.mintCredTerm;
                        dlg.DueDate = this.mdttDueDate;
                        dlg.RecvMan = this.mstrRecvMan;
                        dlg.WHouse = this.mstrWHouse;
                        dlg.PromoteCode = this.mintPromote;
                        dlg.DebtCode = this.mstrDebtCode;
                        dlg.DebtDate = this.mdttDebtDate;
                        dlg.VatCoor = this.mstrVatCoor;
                        dlg.InvoiceDetail = this.mstrInvDetail;
                        dlg.HasVatReturn = this.mstrHasReturn;
                        dlg.VatDueDate = this.mdttReceDate;
                        dlg.VatSeq = this.mstrVatSeq;
                        dlg.DeliCoor = this.mstrDeliCoor;
                        dlg.VatType = this.txtVatType.Tag.ToString();

                        dlg.Remark1 = this.mstrRemarkH1;
                        dlg.Remark2 = this.mstrRemarkH2;
                        dlg.Remark3 = this.mstrRemarkH3;
                        dlg.Remark4 = this.mstrRemarkH4;
                        dlg.Remark5 = this.mstrRemarkH5;
                        dlg.Remark6 = this.mstrRemarkH6;
                        dlg.Remark7 = this.mstrRemarkH7;
                        dlg.Remark8 = this.mstrRemarkH8;
                        dlg.Remark9 = this.mstrRemarkH9;
                        dlg.Remark10 = this.mstrRemarkH10;

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {

                            this.mstrSEmpl = dlg.SEmpl;
                            this.mdecSaleComm = dlg.SaleCommission;
                            this.mintCredTerm = dlg.CreditTerm;
                            if (dlg.HasDueDate)
                            {
                                this.mdttDueDate = dlg.DueDate;
                            }
                            else
                            {
                                this.mdttDueDate = DBEnum.NullDate;
                            }
                            this.mstrRecvMan = dlg.RecvMan;
                            this.mintPromote = dlg.PromoteCode;
                            this.mstrDebtCode = dlg.DebtCode;
                            if (dlg.HasDebtDate)
                            {
                                this.mdttDebtDate = dlg.DebtDate;
                            }
                            else
                            {
                                this.mdttDebtDate = DBEnum.NullDate;
                            }
                            this.mstrVatCoor = dlg.VatCoor;
                            this.mstrInvDetail = dlg.InvoiceDetail;
                            this.mstrHasReturn = dlg.HasVatReturn;
                            this.mdttReceDate = dlg.VatDueDate;
                            this.mstrVatSeq = dlg.VatSeq;
                            this.mstrDeliCoor = dlg.DeliCoor;

                            if (this.mstrWHouse != dlg.WHouse)
                            {
                                this.mstrWHouse = dlg.WHouse;
                                this.pmChangeWHouseItem();
                            }

                            this.mstrRemarkH1 = dlg.Remark1;
                            this.mstrRemarkH2 = dlg.Remark2;
                            this.mstrRemarkH3 = dlg.Remark3;
                            this.mstrRemarkH4 = dlg.Remark4;
                            this.mstrRemarkH5 = dlg.Remark5;
                            this.mstrRemarkH6 = dlg.Remark6;
                            this.mstrRemarkH7 = dlg.Remark7;
                            this.mstrRemarkH8 = dlg.Remark8;
                            this.mstrRemarkH9 = dlg.Remark9;
                            this.mstrRemarkH10 = dlg.Remark10;
                        }
                        this.txtEditDet1.Text = "N";
                        this.txtEditDet1.Focus();
                    }
                    break;
            }
            return true;
        }

        private void pmQueryRefTo()
        {
            this.pmQueryRefDoc();
        }

        private void pmQueryRefDoc()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

            this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].Rows.Clear();
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", "select * from GLREF where FCSKID = ?", ref strErrorMsg);
            DataRow dtrQGLRef = this.dtsDataEnv.Tables["QGLRef"].Rows[0];


            //Load PR
            string strNoteCut_PR = "select fcChildTyp, fcChildH from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? group by fcChildTyp, fcChildH";
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut_PR, ref strErrorMsg);
            foreach (DataRow dtrNoteCut in this.dtsDataEnv.Tables["QNoteCut"].Rows)
            {
                string strRefType = dtrNoteCut["fcChildTyp"].ToString();
                DocumentType oDocType = BusinessEnum.GetDocEnum(strRefType);

                switch (oDocType)
                {
                    case DocumentType.QS:
                    case DocumentType.PR:
                    case DocumentType.PO:
                    case DocumentType.SO:
                        this.pmQueryOrder(dtrNoteCut["fcChildH"].ToString());
                        break;
                }
            }

            //กรณีเป็น Invoice Load ใบลดหนี้/เพิ่มหนี้
            string strNoteCut = "select fcMasterTy, fcMasterH from NOTECUT where FCCHILDTYP = ? and FCCHILDH = ? group by fcMasterTy, fcMasterH";
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNoteCut2", "NoteCut", strNoteCut, ref strErrorMsg);
            foreach (DataRow dtrNoteCut2 in this.dtsDataEnv.Tables["QNoteCut2"].Rows)
            {
                string strRefType = dtrNoteCut2["fcMasterTy"].ToString();
                DocumentType oDocType = BusinessEnum.GetDocEnum(strRefType);
                switch (oDocType)
                {
                    case DocumentType.BR:
                    case DocumentType.BI:
                    case DocumentType.BC:
                    case DocumentType.BM:
                    case DocumentType.SR:
                    case DocumentType.SI:
                    case DocumentType.SC:
                    case DocumentType.SM:
                        this.pmQueryInvoice(dtrNoteCut2["fcMasterH"].ToString());
                        break;
                }
            }

            //กรณีเป็น ใบลดหนี้/เพิ่มหนี้ Load Invoice
            strNoteCut = "select FCCHILDTYP, FCCHILDH from NOTECUT where fcMasterTy = ? and fcMasterH = ? group by FCCHILDTYP, FCCHILDH";
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNoteCut2", "NoteCut", strNoteCut, ref strErrorMsg);
            foreach (DataRow dtrNoteCut2 in this.dtsDataEnv.Tables["QNoteCut2"].Rows)
            {
                string strRefType = dtrNoteCut2["fcChildTyp"].ToString();
                DocumentType oDocType = BusinessEnum.GetDocEnum(strRefType);
                switch (oDocType)
                {
                    case DocumentType.BR:
                    case DocumentType.BI:
                    case DocumentType.BC:
                    case DocumentType.BM:
                    case DocumentType.SR:
                    case DocumentType.SI:
                    case DocumentType.SC:
                    case DocumentType.SM:
                        this.pmQueryInvoice(dtrNoteCut2["fcChildH"].ToString());
                        break;
                }
            }

            string strLayH = dtrBrow["fcLayH"].ToString();
            if (strLayH != null)
                this.pmQueryLayH(strLayH, dtrQGLRef);

            //Load Bill
            string strSQLCmdRefPay = "select RefPay.fcChildGLR , RefPay.fcGLRef , RefPay.fnPayAmtKe from RefPay where RefPay.fcChildGLR = ?";
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefPay", "RefPay", strSQLCmdRefPay, ref strErrorMsg);
            foreach (DataRow dtrRefPay in this.dtsDataEnv.Tables["QRefPay"].Rows)
            {
                decimal decAmt = Convert.ToDecimal(dtrBrow["nAmt"]);
                this.pmQueryBill(dtrRefPay["fcGLRef"].ToString(), decAmt, dtrRefPay);
            }

            this.mstrEditRowID = "";
        }

        private void pmQueryOrder(string inRowID)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec_ORDERH = "select ORDERH.FCSKID, ORDERH.FCREFTYPE, ORDERH.FCSTAT, ORDERH.FCSTEP, ORDERH.FCCODE, ORDERH.FCREFNO, ORDERH.FDDATE, ORDERH.FNAMTKE+ORDERH.FNVATAMTKE as NAMT from ORDERH";
            strSQLExec_ORDERH += " where ORDERH.FCSKID = ?";

            pobjSQLUtil.SetPara(new object[] { inRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "pmQueryOrder_ORDERH", "ORDERH", strSQLExec_ORDERH, ref strErrorMsg))
            {
                DataRow dtrSource = this.dtsDataEnv.Tables["pmQueryOrder_ORDERH"].Rows[0];
                DataRow dtrDest = this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].NewRow();
                dtrDest["cRowID"] = dtrSource["fcSkid"].ToString();
                dtrDest["cRefType"] = dtrSource["fcRefType"].ToString();
                dtrDest["cStat"] = dtrSource["fcStat"].ToString();
                dtrDest["cRefNo"] = dtrSource["fcRefNo"].ToString();
                dtrDest["dDate"] = Convert.ToDateTime(dtrSource["fdDate"]);
                //dtrDest["cQnCoor"] = dtrSource["cQnCoor"].ToString();
                //dtrDest["cSign"] = dtrSource["fcSign"].ToString();
                dtrDest["nAmt"] = Convert.ToDecimal(dtrSource["nAmt"]);

                this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].Rows.Add(dtrDest);

            }
        }

        private void pmQueryInvoice(string inRowID)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec_GLREF = "select GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FCSTAT, GLREF.FCSTEP, GLREF.FCCODE, GLREF.FCREFNO, GLREF.FDDATE, GLREF.FNAMTKE+GLREF.FNVATAMTKE as NAMT from GLREF";
            strSQLExec_GLREF += " where GLREF.FCSKID = ?";

            pobjSQLUtil.SetPara(new object[] { inRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "pmQueryInvoice_GLRef", "GLREF", strSQLExec_GLREF, ref strErrorMsg))
            {
                DataRow dtrSource = this.dtsDataEnv.Tables["pmQueryInvoice_GLRef"].Rows[0];
                DataRow dtrDest = this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].NewRow();
                dtrDest["cRowID"] = dtrSource["fcSkid"].ToString();
                dtrDest["cRefType"] = dtrSource["fcRefType"].ToString();
                dtrDest["cStat"] = dtrSource["fcStat"].ToString();
                dtrDest["cRefNo"] = dtrSource["fcRefNo"].ToString();
                dtrDest["dDate"] = Convert.ToDateTime(dtrSource["fdDate"]);
                //dtrDest["cQnCoor"] = dtrSource["cQnCoor"].ToString();
                //dtrDest["cSign"] = dtrSource["fcSign"].ToString();
                dtrDest["nAmt"] = Convert.ToDecimal(dtrSource["nAmt"]);

                this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].Rows.Add(dtrDest);

            }

        }

        private void pmQueryLayH(string inRowID, DataRow inGLRef)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec_LAYH = "select * from LAYH where LAYH.FCSKID = ?";

            pobjSQLUtil.SetPara(new object[] { inRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "pmQueryOrder_LAYH", "LAYH", strSQLExec_LAYH, ref strErrorMsg))
            {
                DataRow dtrSource = this.dtsDataEnv.Tables["pmQueryOrder_LAYH"].Rows[0];
                DataRow dtrDest = this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].NewRow();

                decimal decXRate = Convert.ToDecimal(inGLRef["fnXRate"]);

                decimal decAmt2 = Math.Round(Convert.ToDecimal(inGLRef["fnAmt2"]) / (decXRate == 0 ? 1 : decXRate), 4, MidpointRounding.AwayFromZero);
                decimal decSign = (BusinessEnum.gc_RFTYPE_CR_BUY + BusinessEnum.gc_RFTYPE_CR_SELL).IndexOf(inGLRef["fcRfType"].ToString()) > -1 ? -1 : 1;
                decimal decPayAmtKe = Math.Round(Convert.ToDecimal(inGLRef["fnPayAmtKe"]) - decAmt2, 4, MidpointRounding.AwayFromZero) * decSign;

                dtrDest["cRowID"] = dtrSource["fcSkid"].ToString();
                dtrDest["cRefType"] = dtrSource["fcRefType"].ToString();
                dtrDest["cStat"] = dtrSource["fcStat"].ToString();
                dtrDest["cRefNo"] = dtrSource["fcRefNo"].ToString();
                dtrDest["dDate"] = Convert.ToDateTime(dtrSource["fdDate"]);
                //dtrDest["cQnCoor"] = dtrSource["cQnCoor"].ToString();
                //dtrDest["cSign"] = dtrSource["fcSign"].ToString();
                dtrDest["nAmt"] = Convert.ToDecimal(dtrSource["fnAmt"]);
                dtrDest["nPayAmt"] = decPayAmtKe;

                this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].Rows.Add(dtrDest);

            }
        }

        private void pmQueryBill(string inRowID, decimal inAmt, DataRow inRefPay)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec_GLREF = "select * from GLREF where GLREF.FCSKID = ?";

            pobjSQLUtil.SetPara(new object[] { inRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "pmQueryInvoice_GLRef", "GLREF", strSQLExec_GLREF, ref strErrorMsg))
            {
                DataRow dtrSource = this.dtsDataEnv.Tables["pmQueryInvoice_GLRef"].Rows[0];
                DataRow dtrDest = this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].NewRow();

                decimal decSign = (BusinessEnum.gc_RFTYPE_CR_BUY + BusinessEnum.gc_RFTYPE_CR_SELL).IndexOf(dtrSource["fcRfType"].ToString()) > -1 ? -1 : 1;
                decimal decPayAmtKe = Math.Round(Convert.ToDecimal(inRefPay["fnPayAmtKe"]), 4, MidpointRounding.AwayFromZero) * decSign;

                dtrDest["cRowID"] = dtrSource["fcSkid"].ToString();
                dtrDest["cRefType"] = dtrSource["fcRefType"].ToString();
                dtrDest["cStat"] = dtrSource["fcStat"].ToString();
                dtrDest["cRefNo"] = dtrSource["fcRefNo"].ToString();
                dtrDest["dDate"] = Convert.ToDateTime(dtrSource["fdDate"]);
                //dtrDest["cQnCoor"] = dtrSource["cQnCoor"].ToString();
                //dtrDest["cSign"] = dtrSource["fcSign"].ToString();
                dtrDest["nAmt"] = inAmt;
                dtrDest["nPayAmt"] = Convert.ToDecimal(decPayAmtKe);

                this.dtsDataEnv.Tables[xd_ALIAS_BROW_REFTO].Rows.Add(dtrDest);

            }

        }

        private void pmPopGetRefTo()
        {
            //if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count == 0)
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count > 0)
            {
                DataRow dtrTemRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows[0];
                this.mstrRefToBook = dtrTemRefTo["cBook"].ToString();
                this.mstrRefToRowID = dtrTemRefTo["cRowID"].ToString();
            }
            if (this.mstrRefToRowID == "")
            {
                this.pmRefToReqOrder();

                //using (Common.dlgGetWhRef dlgRefTo = new Common.dlgGetWhRef())
                //{
                //    dlgRefTo.ShowDialog();
                //    if (dlgRefTo.DialogResult == DialogResult.OK)
                //    {
                //        switch (dlgRefTo.SelectedIndex)
                //        {
                //            case 0:
                //                this.pmRefToReqProd();
                //                break;
                //            case 1:
                //                this.pmRefToReqOrder();
                //                break;
                //            case 2:
                //                this.pmRefToReqCoor();
                //                break;
                //        }
                //    }
                //}
            }
            else
            {
                //Show RefTo List
                this.pmRefToReqOrder();
            }
        }

        //ค้นโดยระบุเป็นผู้จำหน่าย
        private void pmRefToReqCoor()
        {
            this.pmRefToItem("COOR");
        }

        //ค้นโดยระบุเป็นสินค้า
        private void pmRefToReqProd()
        {
            this.pmRefToItem("PROD");
        }

        private void pmRefToItem(string inType)
        {

            this.pmQueryRefToProd();
            this.pmLoadRefToProd();
            this.pmSetRefToCodeList();
            this.mbllRecalTotPd = true;
            this.pmRecalTotPd();

            //TODO: XXX
            //using (Transaction.Common.dlgReqOrderToProd dlgRefTo = new Transaction.Common.dlgReqOrderToProd())
            //{
            //    this.pmQueryRefToProd();
            //    dlgRefTo.IsSearchByProduct = (inType.ToUpper().TrimEnd() == "PROD");
            //    dlgRefTo.StartPosition = FormStartPosition.Manual;
            //    dlgRefTo.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlgRefTo.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //    dlgRefTo.BindData(this.dtsDataEnv);
            //    dlgRefTo.ShowDialog();
            //    if (dlgRefTo.DialogResult == DialogResult.OK)
            //    {
            //        this.pmLoadRefToProd();
            //        this.pmSetRefToCodeList();
            //        this.mbllRecalTotPd = true;
            //        this.pmRecalTotPd();
            //    }
            //}
        }

        private void pmQueryRefToProd()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            if (this.txtQcCoor.Tag.ToString() != string.Empty)
            {
                strSQLStr = "select OrderH.fdDate ,OrderI.fcProd , OrderI.fcSect , OrderI.fcDept , OrderI.fcJob , OrderI.fcProj , OrderI.fnBackQty * OrderI.fnUMQty as fnBackQty, OrderI.fnUmQty , OrderI.fcSkid , OrderI.fcOrderH , OrderH.fcRefType , OrderH.fcBook , OrderH.fcCoor , OrderH.fdDate , OrderH.fcRefNo as fcPRCode , Prod.fcCode , Prod.fcName ";
                strSQLStr += " , Coor.fcCode as fcQcCoor, Coor.fcSName as fcQnCoor ";
                strSQLStr += " from OrderI , OrderH , Prod , Coor ";
                strSQLStr += " where OrderH.fcSkid = OrderI.fcOrderH";
                strSQLStr += " and OrderI.fcProd = Prod.fcSkid ";
                strSQLStr += " and OrderI.fcCoor = Coor.fcSkid ";
                strSQLStr += " and OrderH.fcCorp = ?";
                strSQLStr += " and OrderH.fcBranch = ?";
                strSQLStr += " and OrderH.fcRefType = ?";
                strSQLStr += " and OrderH.fcStat = ?";
                strSQLStr += " and OrderH.fcStep = ?";
                strSQLStr += " and OrderI.fcStat = ?";
                strSQLStr += " and OrderI.fcStep = ?";
                strSQLStr += " and OrderI.fcRefPdTyp = ?";
                strSQLStr += " and OrderH.fcCoor = ?";
                strSQLStr += " order by Prod.fcCode";

                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefToRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, SysDef.gc_REFPD_TYPE_PRODUCT, this.txtQcCoor.Tag });
            }
            else
            {
                strSQLStr = "select OrderH.fdDate ,OrderI.fcProd , OrderI.fcSect , OrderI.fcDept , OrderI.fcJob , OrderI.fcProj , OrderI.fnBackQty * OrderI.fnUMQty as fnBackQty , OrderI.fnUmQty , OrderI.fcSkid , OrderI.fcOrderH , OrderH.fcRefType , OrderH.fcBook , OrderH.fcCoor , OrderH.fdDate , OrderH.fcRefNo as fcPRCode , Prod.fcCode , Prod.fcName ";
                strSQLStr += " , Coor.fcCode as fcQcCoor, Coor.fcSName as fcQnCoor ";
                strSQLStr += " from OrderI , OrderH , Prod , Coor ";
                strSQLStr += " where OrderH.fcSkid = OrderI.fcOrderH";
                strSQLStr += " and OrderI.fcProd = Prod.fcSkid ";
                strSQLStr += " and OrderI.fcCoor = Coor.fcSkid ";
                strSQLStr += " and OrderH.fcCorp = ?";
                strSQLStr += " and OrderH.fcBranch = ?";
                strSQLStr += " and OrderH.fcRefType = ?";
                strSQLStr += " and OrderH.fcStat = ?";
                strSQLStr += " and OrderH.fcStep = ?";
                strSQLStr += " and OrderI.fcStat = ?";
                strSQLStr += " and OrderI.fcStep = ?";
                strSQLStr += " and OrderI.fcRefPdTyp = ?";
                strSQLStr += " order by Prod.fcCode";

                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefToRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, SysDef.gc_REFPD_TYPE_PRODUCT });
            }
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, xd_Alias_TemRefToProd, "REFTO", strSQLStr, ref strErrorMsg);
            DataColumn dtcCheck = new DataColumn("IsCheck", System.Type.GetType("System.Boolean"));
            dtcCheck.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck);

            DataColumn dtcCheck2 = new DataColumn("IsCheck2", System.Type.GetType("System.Boolean"));
            dtcCheck2.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck2);

        }

        private void pmLoadRefToProd()
        {

            bool bllsFirst = true;
            foreach (DataRow dtrRefTo in this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Rows)
            {
                DataRow[] dtaRefToRow = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cRefToRowID = '" + dtrRefTo["fcSkid"].ToString() + "'");
                //if (Convert.ToBoolean(dtrRefTo["IsCheck2"]) == false)
                if (false)
                {
                    for (int intDel = 0; intDel < dtaRefToRow.Length; intDel++)
                    {
                        //this.pmClrRefTo(dtrRefTo["fcSkid"].ToString());
                        //dtaRefToRow[intDel]["cRefToCode"] = "";
                        //dtaRefToRow[intDel]["cRefToRowID"] = "";
                        //dtaRefToRow[intDel]["cRefToHRowID"] = "";
                    }
                }
                else
                {
                    if (dtaRefToRow.Length == 0)
                    {
                        string strRefFld = "";
                        if (this.mbllIsInv)
                        {
                            //Invoice ค้น Order
                            this.pmLoadRefTo1Prod(dtrRefTo, bllsFirst);
                            strRefFld = "fcOrderH";
                            bllsFirst = false;
                        }
                        else
                        {
                            //ใบลดหนี้/เพิ่มหนี้ ค้น Invoice
                            this.pmLoadRefTo1Prod_RefProd(dtrRefTo);
                            strRefFld = "fcGLRef";
                        }
                        DataRow[] dtaRefToHRow = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Select("cRowID = '" + dtrRefTo[strRefFld].ToString() + "'");
                        if (dtaRefToHRow.Length == 0)
                        {
                            DataRow dtrNewRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].NewRow();
                            dtrNewRefTo["cRowID"] = dtrRefTo[strRefFld].ToString();
                            dtrNewRefTo["cRefType"] = this.mstrRefToRefType;
                            dtrNewRefTo["cBook"] = dtrRefTo["fcBook"].ToString();
                            //dtrNewRefTo["cCode"] = dtrRefTo["fcPRCode"].ToString();
                            dtrNewRefTo["cCode"] = dtrRefTo["fcPRCode2"].ToString();
                            this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Add(dtrNewRefTo);
                        }
                    }
                }
            }
        }

        //TODO: Load Detail of Order
        private void pmLoadHDetail(string inOrderH)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderH", "ORDERH", "select * from ORDERH where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrOrderH = this.dtsDataEnv.Tables["QOrderH"].Rows[0];

                decimal decXRate = Convert.ToDecimal(dtrOrderH["fnXRate"]);
                this.txtXRate.Value = (decXRate != 0 ? decXRate : 1);
                this.txtQcCurrency.Tag = dtrOrderH["fcCurrency"].ToString();
                pobjSQLUtil.SetPara(new object[1] { this.txtQcCurrency.Tag });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCurrency", "CURRENCY", "select fcCode, fcName, fcSign from " + MapTable.Table.Currency + " where fcSkid = ?", ref strErrorMsg))
                {
                    DataRow dtrCurr = this.dtsDataEnv.Tables["QCurrency"].Rows[0];
                    this.txtQcCurrency.Text = dtrCurr["fcCode"].ToString().TrimEnd();
                    this.txtQnCurrency.Text = dtrCurr["fcName"].ToString().TrimEnd();
                    this.mstrCurrSign = dtrCurr["fcSign"].ToString();
                }

                this.pmLoadRemark(dtrOrderH);

                pobjSQLUtil.SetPara(new object[1] { dtrOrderH["fcCoor"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcCode, fcName, fcSName from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                {
                    DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                    this.txtQcCoor.Tag = dtrOrderH["fcCoor"].ToString();
                    this.txtQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                    this.txtQsCoor.Text = dtrCoor["fcSName"].ToString().TrimEnd();

                    this.mstrLastQcCoor = this.txtQcCoor.Text;
                    this.mstrLastQsCoor = this.txtQsCoor.Text;
                    
                    this.mstrInvDetail = dtrCoor["fcName"].ToString().TrimEnd();
                }

                if (this.mstrVatCoor == string.Empty)
                {
                    this.mstrVatCoor = dtrOrderH["fcCoor"].ToString();
                }
                if (this.mstrDeliCoor == string.Empty)
                {
                    this.mstrDeliCoor = dtrOrderH["fcCoor"].ToString();
                }

                this.mstrSEmpl = dtrOrderH["fcEmpl"].ToString();
                this.mintCredTerm = Convert.ToInt32(dtrOrderH["fnCredTerm"]);
                this.pmCalDueDate();
                this.mstrTradeTrm = dtrOrderH["fcTradeTrm"].ToString();

            }
        }

        private void pmLoadRefTo1Prod(DataRow inOrderI, bool inFirst)
        {
            int intRecNo = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            if (inFirst)
            {
                this.pmLoadHDetail(inOrderI["fcOrderH"].ToString());
            }

            pobjSQLUtil.SetPara(new object[] { inOrderI["fcProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
            {

                intRecNo++;
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.pmRetrieveProductVal(ref dtrTemPd, dtrProd);

                //dtrTemPd["nRecNo"] = intRecNo;
                dtrTemPd["cXRefToProd"] = inOrderI["fcProd"].ToString();
                dtrTemPd["cXRefToRefType"] = inOrderI["fcRefType"].ToString();
                dtrTemPd["cXRefToRowID"] = inOrderI["fcSkid"].ToString();
                dtrTemPd["cXRefToHRowID"] = inOrderI["fcOrderH"].ToString();
                dtrTemPd["cRefToRowID"] = inOrderI["fcSkid"].ToString();
                dtrTemPd["cRefToHRowID"] = inOrderI["fcOrderH"].ToString();
                //dtrTemPd["cRefToCode"] = inOrderI["fcPRCode"].ToString();
                dtrTemPd["cRefToCode"] = inOrderI["fcPRCode2"].ToString();

                dtrTemPd["cCtrlStoc"] = dtrProd["fcCtrlStoc"].ToString();
                dtrTemPd["nQty"] = Math.Round(Convert.ToDecimal(inOrderI["fnBackQty"]) / Convert.ToDecimal(dtrTemPd["nUOMQty"]), 2, MidpointRounding.AwayFromZero);
                dtrTemPd["nBackQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                dtrTemPd["nLastQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);

                DataRow dtrOrderI = null;
                pobjSQLUtil.SetPara(new object[1] { inOrderI["fcSkid"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "ORDERI", "select * from ORDERI where fcSkid = ?", ref strErrorMsg))
                {
                    dtrOrderI = this.dtsDataEnv.Tables["QOrderI"].Rows[0];
                    dtrTemPd["nPrice"] = Convert.ToDecimal(dtrOrderI["fnPriceKe"]);
                    dtrTemPd["nLastPrice"] = Convert.ToDecimal(dtrOrderI["fnPriceKe"]);
                    dtrTemPd["cDiscStr"] = dtrOrderI["fcDiscStr"].ToString().TrimEnd();
                    dtrTemPd["nDiscAmt"] = Convert.ToDecimal(dtrOrderI["fnDiscAmt"]);

                    dtrTemPd["cRemark1"] = (Convert.IsDBNull(dtrOrderI["fmReMark"]) ? "" : dtrOrderI["fmReMark"].ToString().TrimEnd());
                    dtrTemPd["cRemark2"] = (Convert.IsDBNull(dtrOrderI["fmReMark2"]) ? "" : dtrOrderI["fmReMark2"].ToString().TrimEnd());
                    dtrTemPd["cRemark3"] = (Convert.IsDBNull(dtrOrderI["fmReMark3"]) ? "" : dtrOrderI["fmReMark3"].ToString().TrimEnd());
                    dtrTemPd["cRemark4"] = (Convert.IsDBNull(dtrOrderI["fmReMark4"]) ? "" : dtrOrderI["fmReMark4"].ToString().TrimEnd());
                    dtrTemPd["cRemark5"] = (Convert.IsDBNull(dtrOrderI["fmReMark5"]) ? "" : dtrOrderI["fmReMark5"].ToString().TrimEnd());
                    dtrTemPd["cRemark6"] = (Convert.IsDBNull(dtrOrderI["fmReMark6"]) ? "" : dtrOrderI["fmReMark6"].ToString().TrimEnd());
                    dtrTemPd["cRemark7"] = (Convert.IsDBNull(dtrOrderI["fmReMark7"]) ? "" : dtrOrderI["fmReMark7"].ToString().TrimEnd());
                    dtrTemPd["cRemark8"] = (Convert.IsDBNull(dtrOrderI["fmReMark8"]) ? "" : dtrOrderI["fmReMark8"].ToString().TrimEnd());
                    dtrTemPd["cRemark9"] = (Convert.IsDBNull(dtrOrderI["fmReMark9"]) ? "" : dtrOrderI["fmReMark9"].ToString().TrimEnd());
                    dtrTemPd["cRemark10"] = (Convert.IsDBNull(dtrOrderI["fmReMark10"]) ? "" : dtrOrderI["fmReMark10"].ToString().TrimEnd());
                
                }

                dtrTemPd["cDept"] = inOrderI["fcSect"].ToString();
                dtrTemPd["cDivision"] = inOrderI["fcDept"].ToString();
                dtrTemPd["cJob"] = inOrderI["fcJob"].ToString();
                dtrTemPd["cProject"] = inOrderI["fcProj"].ToString();
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cDept"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select FCCODE,FCNAME from SECT where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();
                }

                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cJob"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select FCCODE,FCNAME from JOB where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();
                }

                dtrTemPd["cWHouse"] = dtrOrderI["fcWHouse"].ToString();
                dtrTemPd["cLot"] = dtrOrderI["fcLot"].ToString().TrimEnd();

                dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);

                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }
        }

        private void pmLoadRefTo1Prod_RefProd(DataRow inRefProd)
        {
            int intRecNo = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inRefProd["fcProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
            {

                intRecNo++;
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.pmRetrieveProductVal(ref dtrTemPd, dtrProd);

                if (this.txtQcCoor.Text.TrimEnd() == string.Empty)
                {
                    this.txtQcCoor.Tag = inRefProd["fcCoor"].ToString();
                    this.txtQcCoor.Text = inRefProd["fcQcCoor"].ToString().TrimEnd();
                    this.txtQsCoor.Text = inRefProd["fcQnCoor"].ToString().TrimEnd();

                    this.mstrLastQcCoor = this.txtQcCoor.Text;
                    this.mstrLastQsCoor = this.txtQsCoor.Text;
                
                }

                //dtrTemPd["nRecNo"] = intRecNo;
                dtrTemPd["cXRefToProd"] = inRefProd["fcProd"].ToString();
                dtrTemPd["cXRefToRefType"] = inRefProd["fcRefType"].ToString();
                dtrTemPd["cXRefToRowID"] = inRefProd["fcSkid"].ToString();
                dtrTemPd["cXRefToHRowID"] = inRefProd["fcGLRef"].ToString();
                dtrTemPd["cRefToRowID"] = inRefProd["fcSkid"].ToString();
                dtrTemPd["cRefToHRowID"] = inRefProd["fcGLRef"].ToString();
                dtrTemPd["cRefToCode"] = inRefProd["fcPRCode"].ToString();

                if (!this.mbllIsCrDrChgQty)
                    dtrTemPd["nUOMQty"] = 0;

                decimal decUmQty = (Convert.ToDecimal(dtrTemPd["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(dtrTemPd["nUOMQty"]));
                decimal decRefQty = this.pmSumCrNoteQty(this.mstrRefToRefType, inRefProd["fcGLRef"].ToString(), inRefProd["fcSkid"].ToString(), decUmQty);
                decimal decQty = Math.Round(Convert.ToDecimal(inRefProd["fnBackQty"]) / decUmQty, 2, MidpointRounding.AwayFromZero) - decRefQty;

                dtrTemPd["cCtrlStoc"] = dtrProd["fcCtrlStoc"].ToString();
                dtrTemPd["nQty"] = (decQty >= 0 ? decQty : 0);
                dtrTemPd["nBackQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                dtrTemPd["nLastQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);

                DataRow dtrRefProd = null;
                pobjSQLUtil.SetPara(new object[1] { inRefProd["fcSkid"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from REFPROD where fcSkid = ?", ref strErrorMsg))
                {
                    dtrRefProd = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                    dtrTemPd["nPrice"] = Convert.ToDecimal(dtrRefProd["fnPriceKe"]);
                    dtrTemPd["nLastPrice"] = Convert.ToDecimal(dtrRefProd["fnPriceKe"]);
                    dtrTemPd["cDiscStr"] = dtrRefProd["fcDiscStr"].ToString().TrimEnd();
                    dtrTemPd["nDiscAmt"] = Convert.ToDecimal(dtrRefProd["fnDiscAmt"]);
                }

                dtrTemPd["cDept"] = inRefProd["fcSect"].ToString();
                dtrTemPd["cDivision"] = inRefProd["fcDept"].ToString();
                dtrTemPd["cJob"] = inRefProd["fcJob"].ToString();
                dtrTemPd["cProject"] = inRefProd["fcProj"].ToString();
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cDept"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select FCCODE,FCNAME from SECT where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();
                }

                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cJob"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select FCCODE,FCNAME from JOB where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    dtrTemPd["cQnJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();
                }

                dtrTemPd["cWHouse"] = dtrRefProd["fcWHouse"].ToString();
                dtrTemPd["cLot"] = dtrRefProd["fcLot"].ToString().TrimEnd();

                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }
        }

        //ค้นโดยระบุเป็นใบ
        private void pmRefToReqOrder()
        {
            using (Common.Order.dlgOrdGetRefTo dlgRefTo = new Common.Order.dlgOrdGetRefTo(this.mRefToDocType, this.mstrRefToRefTypeList))
            {

                dlgRefTo.RefDocumentType = this.mRefToDocType;
                dlgRefTo.CoorType = this.mCoorType;
                dlgRefTo.BranchID = this.mstrBranchID;
                dlgRefTo.CoorID = this.txtQcCoor.Tag.ToString();
                dlgRefTo.BookID = this.mstrRefToBook;
                dlgRefTo.RefToDocumentID = this.mstrRefToRowID;

                dlgRefTo.ShowDialog();
                if (dlgRefTo.DialogResult == DialogResult.OK)
                {
                    if (dlgRefTo.RefToDocumentID != string.Empty)
                    {
                        this.mRefToDocType = dlgRefTo.RefDocumentType;
                        if (dlgRefTo.RefToDocumentID != this.mstrRefToRowID)
                            this.pmRefToItemByOrder(dlgRefTo.RefToDocumentID);
                    }
                    else
                    {
                        this.pmClearRefTo();
                    }
                }
            }
        }

        //ค้นโดยระบุเป็นใบจาก GLRef
        private void pmRefToReqGLRef()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count > 0)
            {
                DataRow dtrTemRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows[0];
                this.mstrRefToBook = dtrTemRefTo["cBook"].ToString();
                this.mstrRefToRowID = dtrTemRefTo["cRowID"].ToString();
            }

            //TODO: XXX
            //using (Common.Invoice.dlgInvGetRefTo dlgRefTo = new Common.Invoice.dlgInvGetRefTo(this.mRefToDocType, this.mstrRefToRefTypeList))
            //{

            //    dlgRefTo.CoorType = this.mCoorType;
            //    dlgRefTo.BranchID = this.mstrBranchID;
            //    dlgRefTo.CoorID = this.txtQcCoor.Tag;
            //    dlgRefTo.BookID = this.mstrRefToBook;
            //    dlgRefTo.RefToDocumentID = this.mstrRefToRowID;

            //    dlgRefTo.ShowDialog();
            //    if (dlgRefTo.DialogResult == DialogResult.OK)
            //    {
            //        if (dlgRefTo.RefToDocumentID != string.Empty)
            //        {
            //            if (dlgRefTo.RefToDocumentID != this.mstrRefToRowID)
            //            {
            //                string strMsg = "";
            //                if (this.mbllIsCrNote)
            //                    strMsg = "ต้องการลดจำนวน ?";
            //                else if (this.mbllIsDrNote)
            //                    strMsg = "ต้องการเพิ่มจำนวน ?";

            //                this.mstrRefToRefType = dlgRefTo.RefDocumentType.ToString();
            //                this.mRefToDocType = BusinessEnum.GetDocEnum(this.mstrRefToRefType);
            //                this.mstrRefToRowID = dlgRefTo.RefToDocumentID;
            //                this.mstrRefToBook = dlgRefTo.BookID;

            //                this.mbllIsCrDrChgQty = (MessageBox.Show(this, strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
            //                this.pmRefToItemByRefProd(dlgRefTo.RefToDocumentID);
            //            }
            //        }
            //        else
            //        {
            //            this.pmClearRefTo();
            //        }
            //    }
            //}
        }

        private void pmRefToItemByRefProd(string inGLRef)
        {
            //TODO: XXX
            //using (Transaction.Common.dlgReqOrderToProd dlgRefTo = new Transaction.Common.dlgReqOrderToProd())
            //{
            //    this.pmQueryRefToProdByRefProd(inGLRef);
            //    dlgRefTo.StartPosition = FormStartPosition.Manual;
            //    dlgRefTo.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlgRefTo.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //    dlgRefTo.BindData(this.dtsDataEnv);
            //    dlgRefTo.ShowDialog();
            //    if (dlgRefTo.DialogResult == DialogResult.OK)
            //    {
            //        this.pmLoadRefToProd();
            //        this.pmSetRefToCodeList();
            //        this.mbllRecalTotPd = true;
            //        this.pmRecalTotPd();
            //    }
            //}
        }

        private void pmRefToItemByOrder(string inOrderH)
        {
            this.pmQueryRefToProdByOrder(inOrderH);
            this.pmLoadRefToProd();
            this.pmSetRefToCodeList();
            this.mbllRecalTotPd = true;
            this.pmRecalTotPd();

            //TODO: XXX
            //using (Transaction.Common.dlgReqOrderToProd dlgRefTo = new Transaction.Common.dlgReqOrderToProd())
            //{
            //    this.pmQueryRefToProdByOrder(inOrderH);
            //    //dlgRefTo.IsSearchByProduct = true;
            //    dlgRefTo.StartPosition = FormStartPosition.Manual;
            //    dlgRefTo.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlgRefTo.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //    dlgRefTo.BindData(this.dtsDataEnv);
            //    dlgRefTo.ShowDialog();
            //    if (dlgRefTo.DialogResult == DialogResult.OK)
            //    {
            //        this.pmLoadRefToProd();
            //        this.pmSetRefToCodeList();
            //        this.mbllRecalTotPd = true;
            //        this.pmRecalTotPd();
            //    }
            //}
        }

        private void pmQueryRefToProdByOrder(string inOrderH)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select OrderH.fdDate ,OrderI.fcProd , OrderI.fcSect , OrderI.fcDept , OrderI.fcJob , OrderI.fcProj , OrderI.fnBackQty * OrderI.fnUMQty as fnBackQty , OrderI.fnUmQty , OrderI.fcSkid , OrderI.fcOrderH , OrderH.fcRefType , OrderH.fcBook , OrderH.fcCoor , OrderH.fdDate , OrderH.fcRefNo as fcPRCode , OrderH.fcCode as fcPRCode2 , Prod.fcCode , Prod.fcName ";
            strSQLStr += " , Coor.fcCode as fcQcCoor, Coor.fcSName as fcQnCoor ";
            strSQLStr += " from OrderI , OrderH , Prod , Coor ";
            strSQLStr += " where OrderH.fcSkid = OrderI.fcOrderH";
            strSQLStr += " and OrderI.fcProd = Prod.fcSkid ";
            strSQLStr += " and OrderI.fcCoor = Coor.fcSkid ";
            strSQLStr += " and OrderH.fcSkid = ?";
            strSQLStr += " and OrderI.fcStat = ?";
            strSQLStr += " and OrderI.fcStep = ?";
            strSQLStr += " and OrderI.fcRefPdTyp = ?";
            strSQLStr += " order by OrderI.fcSeq";

            pobjSQLUtil.SetPara(new object[] { inOrderH, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, SysDef.gc_REFPD_TYPE_PRODUCT });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, xd_Alias_TemRefToProd, "REFTO", strSQLStr, ref strErrorMsg);
            DataColumn dtcCheck = new DataColumn("IsCheck", System.Type.GetType("System.Boolean"));
            dtcCheck.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck);

            DataColumn dtcCheck2 = new DataColumn("IsCheck2", System.Type.GetType("System.Boolean"));
            dtcCheck2.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck2);

        }

        private void pmQueryRefToProdByRefProd(string inGLRef)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select GLRef.fdDate ,RefProd.fcProd , RefProd.fcSect , RefProd.fcDept , RefProd.fcJob , RefProd.fcProj , RefProd.fnQty * RefProd.fnUMQty as fnBackQty , RefProd.fnUmQty , RefProd.fcSkid , RefProd.fcGLRef , GLRef.fcRefType , GLRef.fcBook , GLRef.fcCoor , GLRef.fdDate , GLRef.fcRefNo as fcPRCode , Prod.fcCode , Prod.fcName ";
            strSQLStr += " , Coor.fcCode as fcQcCoor, Coor.fcSName as fcQnCoor ";
            strSQLStr += " from RefProd , GLRef , Prod , Coor ";
            strSQLStr += " where GLRef.fcSkid = RefProd.fcGLRef";
            strSQLStr += " and RefProd.fcProd = Prod.fcSkid ";
            strSQLStr += " and RefProd.fcCoor = Coor.fcSkid ";
            strSQLStr += " and GLRef.fcSkid = ?";
            strSQLStr += " and RefProd.fcStat = ?";
            strSQLStr += " and RefProd.fcRefPdTyp = ?";
            strSQLStr += " order by RefProd.fcSeq";

            pobjSQLUtil.SetPara(new object[] { inGLRef, SysDef.gc_STAT_NORMAL, SysDef.gc_REFPD_TYPE_PRODUCT });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, xd_Alias_TemRefToProd, "REFTO", strSQLStr, ref strErrorMsg);
            DataColumn dtcCheck = new DataColumn("IsCheck", System.Type.GetType("System.Boolean"));
            dtcCheck.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck);

            DataColumn dtcCheck2 = new DataColumn("IsCheck2", System.Type.GetType("System.Boolean"));
            dtcCheck2.DefaultValue = false;
            this.dtsDataEnv.Tables[xd_Alias_TemRefToProd].Columns.Add(dtcCheck2);

        }

        private void pmClearRefTo()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.mdecRefToAmt = 0;
            this.txtRefTo.Text = "";
            foreach (DataRow dtrTCut in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                dtrTCut["cRefToRowID"] = "";
                dtrTCut["cRefToCode"] = "";
            }
        }

        private void pmLoadRefToOrder(string inOrderH)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QLoadRefTo", this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrRefTo = this.dtsDataEnv.Tables["QLoadRefTo"].Rows[0];

                this.pmLoadRemark(dtrRefTo);

                pobjSQLUtil.SetPara(new object[] { dtrRefTo["fcCoor"].ToString() });
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from COOR where fcSkid = ?", ref strErrorMsg);
                this.pmRetrieveCoorVal(this.dtsDataEnv.Tables["QCoor"].Rows[0]);
                string strDeliCoor = dtrRefTo["fcDeliCoor"].ToString();
                if (this.mstrDeliCoor != strDeliCoor)
                {
                    pobjSQLUtil.SetPara(new object[] { strDeliCoor });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from COOR where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrRefToCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.mstrDeliCoor = dtrRefToCoor["fcSkid"].ToString();
                    }
                }

                this.txtQcDept.Tag = dtrRefTo["fcSect"].ToString();
                pobjSQLUtil.SetPara(new object[1] { this.txtQcDept.Tag });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode, fcDept from Sect where fcSkid = ?", ref strErrorMsg))
                {
                    this.txtQcDept.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    this.mstrDivision = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString().TrimEnd();
                }

                this.txtQcJob.Tag = dtrRefTo["fcJob"].ToString();
                pobjSQLUtil.SetPara(new object[1] { this.txtQcJob.Tag });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode, fcProj from Job where fcSkid = ?", ref strErrorMsg))
                {
                    this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    this.mstrProj = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString().TrimEnd();
                }

                this.mstrSEmpl = dtrRefTo["fcEmpl"].ToString();
                this.mstrTradeTrm = dtrRefTo["fcTradeTrm"].ToString();

                this.mintCredTerm = Convert.ToInt32(dtrRefTo["fnCredTerm"]);
                this.pmCalDueDate();

                decimal decVatRate = Convert.ToDecimal(dtrRefTo["fnVatRate"]);
                this.txtVatType.Tag = dtrRefTo["fcVatType"].ToString();
                this.mdecVatRate = decVatRate;
                this.txtVatType.Text = dtrRefTo["fcVatType"].ToString();
                this.txtVatRate.Text = decVatRate.ToString("###.00") + "%";

                decimal decXRate = Convert.ToDecimal(dtrRefTo["fnXRate"]);
                this.txtXRate.Value = (decXRate != 0 ? decXRate : 1);
                this.txtQcCurrency.Tag = dtrRefTo["fcCurrency"].ToString();
                pobjSQLUtil.SetPara(new object[1] { this.txtQcCurrency.Tag });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCurrency", "CURRENCY", "select fcCode, fcName from Currency where fcSkid = ?", ref strErrorMsg))
                {
                    DataRow dtrCurr = this.dtsDataEnv.Tables["QCurrency"].Rows[0];
                    this.txtQcCurrency.Text = dtrCurr["fcCode"].ToString().TrimEnd();
                    this.txtQnCurrency.Text = dtrCurr["fcName"].ToString().TrimEnd();
                }

                this.txtVatIsOut.Text = dtrRefTo["fcVatIsOut"].ToString().TrimEnd();
                this.txtTotPdAmt.Value = 0;
                this.txtDiscount.Text = dtrRefTo["fcDiscStr"].ToString().TrimEnd();
                this.txtDiscountAmt.Value = Convert.ToDecimal(dtrRefTo["fnDiscAmtK"]);
                this.txtAmt.Value = Convert.ToDecimal(dtrRefTo["fnAmtKe"]);
                this.txtVatAmt.Value = Convert.ToDecimal(dtrRefTo["fnVatAmtKe"]);

                this.pmLoadRefToOrderI(dtrRefTo["fcSkid"].ToString());
            }
        }

        private void pmLoadRefToOrderI(string inOrderH)
        {
            int intRecNo = 0;
            string strErrorMsg = "";
            string strFldList = "fcSkid, fcStep, fcMStep, fcProdType, fcProd, fcLot, fcWHouse, fmReMark, fmReMark2, fmReMark3, fmReMark4, fmReMark5, fmReMark6, fmReMark7, fmReMark8, fmReMark9, fmReMark10";
            strFldList += ",fcUm, fnQty, fnUmQty, fnBackQty, fnPlanQty, fnPriceKe, fcDiscStr, fnDiscAmt, cEngDraw, cPackStyle ";
            //string strSQLStrOrderI = "select "+strFldList+" from OrderI where OrderI.fcCorp = ? and OrderI.fcBranch = ? and OrderI.fcOrderH = ? order by OrderI.fcSeq , OrderI.fcIOType";
            string strSQLStrOrderI = "select * from OrderI where OrderI.fcCorp = ? and OrderI.fcBranch = ? and OrderI.fcOrderH = ? order by OrderI.fcSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[3] { App.ActiveCorp.RowID, this.mstrBranchID, inOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "OrderI", strSQLStrOrderI, ref strErrorMsg))
            {
                foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    intRecNo++;
                    if (dtrOrderI["fcStep"].ToString() == SysDef.gc_REF_STEP_CLOSED)
                        continue;
                    else if (Convert.ToDecimal(dtrOrderI["fnBackQty"]) == 0)
                        continue;
                    else
                        this.pmRepl1RecTemOrderI2(intRecNo, dtrOrderI);
                }
                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();
                this.gridView2.MoveFirst();
            }
        }

        private void pmRepl1RecTemOrderI2(int inRecNo, DataRow inOrderI)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

            decimal decBackQty = Convert.ToDecimal(inOrderI["fnBackQty"]);

            dtrTemPd["cRowID"] = "";
            //dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cLastProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cLastRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cLastPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cDept"] = inOrderI["fcSect"].ToString();
            dtrTemPd["cDivision"] = inOrderI["fcDept"].ToString();
            dtrTemPd["cJob"] = inOrderI["fcJob"].ToString();
            dtrTemPd["cProject"] = inOrderI["fcProj"].ToString();
            dtrTemPd["cRemark1"] = (Convert.IsDBNull(inOrderI["fmReMark"]) ? "" : inOrderI["fmReMark"].ToString().TrimEnd());
            dtrTemPd["cRemark2"] = (Convert.IsDBNull(inOrderI["fmReMark2"]) ? "" : inOrderI["fmReMark2"].ToString().TrimEnd());
            dtrTemPd["cRemark3"] = (Convert.IsDBNull(inOrderI["fmReMark3"]) ? "" : inOrderI["fmReMark3"].ToString().TrimEnd());
            dtrTemPd["cRemark4"] = (Convert.IsDBNull(inOrderI["fmReMark4"]) ? "" : inOrderI["fmReMark4"].ToString().TrimEnd());
            dtrTemPd["cRemark5"] = (Convert.IsDBNull(inOrderI["fmReMark5"]) ? "" : inOrderI["fmReMark5"].ToString().TrimEnd());
            dtrTemPd["cRemark6"] = (Convert.IsDBNull(inOrderI["fmReMark6"]) ? "" : inOrderI["fmReMark6"].ToString().TrimEnd());
            dtrTemPd["cRemark7"] = (Convert.IsDBNull(inOrderI["fmReMark7"]) ? "" : inOrderI["fmReMark7"].ToString().TrimEnd());
            dtrTemPd["cRemark8"] = (Convert.IsDBNull(inOrderI["fmReMark8"]) ? "" : inOrderI["fmReMark8"].ToString().TrimEnd());
            dtrTemPd["cRemark9"] = (Convert.IsDBNull(inOrderI["fmReMark9"]) ? "" : inOrderI["fmReMark9"].ToString().TrimEnd());
            dtrTemPd["cRemark10"] = (Convert.IsDBNull(inOrderI["fmReMark10"]) ? "" : inOrderI["fmReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inOrderI["fcUm"].ToString();
            dtrTemPd["nQty"] = decBackQty;
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inOrderI["fnUmQty"]) == 0 ? 0 : Convert.ToDecimal(inOrderI["fnUmQty"]));
            dtrTemPd["nBackQty"] = decBackQty;
            dtrTemPd["nBackDOQty"] = decBackQty;
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            dtrTemPd["nPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);
            dtrTemPd["nLastPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);
            dtrTemPd["cDiscStr"] = inOrderI["fcDiscStr"].ToString().TrimEnd();
            dtrTemPd["cLot"] = inOrderI["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inOrderI["fcWHouse"].ToString();
            dtrTemPd["nDiscAmt"] = Convert.ToDecimal(inOrderI["fnDiscAmt"]);

            dtrTemPd["cDiscStr1"] = inOrderI["cDiscStr1"].ToString().TrimEnd();
            dtrTemPd["nDiscAmt1"] = Convert.ToDecimal(inOrderI["nDiscAmt1"]);
            dtrTemPd["cLastDiscStr1"] = inOrderI["cDiscStr1"].ToString().TrimEnd();

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
                dtrTemPd["cStUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHouse"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select FCCODE,FCNAME from WHouse where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cDept"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select FCCODE,FCNAME from SECT where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnDept"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cJob"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select FCCODE,FCNAME from JOB where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnJob"] = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            string strRefToHRowID = inOrderI["fcOrderH"].ToString();
            DataRow[] dtaRefToRow = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Select("cRowID = '" + strRefToHRowID + "'");
            if (dtaRefToRow.Length == 0)
            {
                if ((pobjSQLUtil.SetPara(new object[] { strRefToHRowID })
                    && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select fdDate, fcBook, fcCode, fcRefNo from OrderH where fcSkid = ?", ref strErrorMsg)))
                {
                    DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                    DataRow dtrTemRefTo = this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].NewRow();
                    dtrTemRefTo["cRowID"] = strRefToHRowID;
                    dtrTemRefTo["cRefType"] = this.mstrRefToRefType;
                    dtrTemRefTo["cBook"] = dtrRefTo["fcBook"].ToString();
                    //dtrTemRefTo["cCode"] = dtrRefTo["fcRefNo"].ToString();
                    dtrTemRefTo["cCode"] = dtrRefTo["fcCode"].ToString();
                    dtrTemRefTo["dDate"] = Convert.ToDateTime(dtrRefTo["fdDate"]);
                    this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Add(dtrTemRefTo);
                }
            }
            dtrTemPd["cXRefToProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cXRefToRefType"] = inOrderI["fcRefType"].ToString();
            dtrTemPd["cXRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cXRefToHRowID"] = inOrderI["fcOrderH"].ToString();
            dtrTemPd["cRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cRefToHRowID"] = inOrderI["fcOrderH"].ToString();
            dtrTemPd["cRefToCode"] = this.pmGetRefToCode(inOrderI["fcOrderH"].ToString());

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);

        }

        private void pmSetRefToCodeList()
        {
            string strRefToList = "";
            if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count > 0)
            {
                foreach (DataRow dtrRefToRow in this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows)
                {
                    //if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                    //    strRefToList += dtrRefToRow["cCode"].ToString() + ", ";
                    if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                        strRefToList += "SO" + dtrRefToRow["cQcBook"].ToString().Trim() +"/"+ dtrRefToRow["cCode"].ToString() + ", ";
                }
            }
            if (strRefToList != string.Empty)
                this.txtRefTo.Text = AppUtil.StringHelper.Left(strRefToList, StringHelper.RAt(",", strRefToList) - 1);
        }

        private void pmSetPageStatus(bool inIsEnable)
        {
            for (int intCnt = 0; intCnt < this.pgfMainEdit.TabPages.Count; intCnt++)
            {
                this.pgfMainEdit.TabPages[intCnt].PageEnabled = inIsEnable;
            }
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "INSPD":
                        this.pmInsertGridRow();
                        break;
                    case "DELPD":
                        this.pmClr1TemPd();
                        this.mbllRecalTotPd = true;
                        this.pmRecalTotPd();
                        break;
                    case "EOTH":
                        this.pmInitPopUpDialog("GET_OTH");
                        break;
                    default:
                        WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                        switch (oToolButton)
                        {
                            case WsToolBar.Enter:
                                this.pmEnterForm();
                                break;
                            case WsToolBar.Insert:
                                if (App.PermissionManager.CheckPermission(AuthenType.CanInsert, App.AppUserID, TASKNAME))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Insert;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการเพิ่มข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Update:
                                if (App.PermissionManager.CheckPermission(AuthenType.CanEdit, App.AppUserID, TASKNAME))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Edit;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Delete:
                                if (App.PermissionManager.CheckPermission(AuthenType.CanDelete, App.AppUserID, TASKNAME))
                                {
                                    this.pmDeleteData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการลบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Print:
                                this.pmPrintData();
                                break;

                            case WsToolBar.Search:
                                if (this.gridView1.OptionsView.ShowAutoFilterRow)
                                {
                                    this.gridView1.OptionsView.ShowAutoFilterRow = false;
                                    this.gridView1.ClearColumnsFilter();
                                }
                                else
                                {
                                    this.gridView1.OptionsView.ShowAutoFilterRow = true;
                                }
                                break;
                            case WsToolBar.Undo:
                                if (MessageBox.Show("ยกเลิกการแก้ไข ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                                    this.pmGotoBrowPage();
                                break;
                            case WsToolBar.Save:
                                this.pmSaveData();
                                break;
                            case WsToolBar.Refresh:
                                this.pmRefreshBrowView();
                                break;
                        }
                        break;
                }

            }
        }

        private void pmInsertGridRow()
        {
            if (this.gridView2.FocusedRowHandle == this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count)
                return;

            int intSaveRowHd = this.gridView2.FocusedRowHandle;
            DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            DataRow dtrOldRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            for (int intCnt = this.gridView2.FocusedRowHandle; intCnt < this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count; intCnt++)
            {
                DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[intCnt];
                dtrNewRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]);
                dtrCurrRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]) + 1;

                DataSetHelper.CopyDataRow(dtrCurrRow, ref dtrOldRow);
                DataSetHelper.CopyDataRow(dtrNewRow, ref dtrCurrRow);
                DataSetHelper.CopyDataRow(dtrOldRow, ref dtrNewRow);
            }
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);
            this.gridView2.FocusedRowHandle = intSaveRowHd;
            //this.grdTemPd.Refetch();
            //this.grdTemPd.Refresh();
        }


        private void pmPrintData()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null)
                return;

            //this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

            string strCode = dtrBrow["fcCode"].ToString().TrimEnd();
            using (Transaction.Common.dlgPRange dlg = new Transaction.Common.dlgPRange())
            {
                dlg.BeginCode = strCode;
                dlg.EndCode = strCode;

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    string strRPTFileName = "";
                    switch (this.mDocType)
                    {
                        case DocumentType.SM:
                            strRPTFileName = "PFORM_S_INV_M.RPT";
                            break;
                        case DocumentType.SI:
                            strRPTFileName = "PFORM_S_INV_I.RPT";
                            break;
                    }

                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, Application.StartupPath + "\\RPT\\" + strRPTFileName);
                }
            }
        }

        ArrayList pAHideFMItem = new ArrayList();
        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";

            BeSmartMRP.Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new BeSmartMRP.Report.LocalDataSet.FORM2PRINT();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select ";
            strSQLStr += "Coor.fcCode as QcCoor , Coor.fcName  as QnCoor , Coor.fcSName as QsCoor";
            strSQLStr += ", Coor.fmMemData as CrMemData, Coor.fmMemData2 as CrMemData2, Coor.fmMemData3 as CrMemData3, Coor.fmMemData4 as CrMemData4, Coor.fmMemData5 as CrMemData5, Coor.fcZip, Coor.fcTel ";
            strSQLStr += ", GLRef.fcSkid, GLRef.fcRfType, GLRef.fcRefType, GLRef.fcCode, GLRef.fcRefNo, GLRef.fdDate, GLRef.fnCredTerm, GLRef.fdDueDate, GLRef.fdReceDate, GLRef.fcVatIsOut ";
            strSQLStr += ", GLRef.fmMemData, GLRef.fmMemData2, GLRef.fmMemData3, GLRef.fmMemData4, GLRef.fmMemData5 ";
            strSQLStr += ", GLRef.fnAmt, GLRef.fnVatAmt, GLRef.fnDiscAmt1 ";
            strSQLStr += ", Empl.fcCode as QcSEmpl, Empl.fcName as QnSEmpl ";
            strSQLStr += ", CRZone.fcCode as QcCrZone, CRZone.fcName as QnCrZone ";
            strSQLStr += ", Prod.fcCode as QcProd, Prod.fcName as QnProd, Prod.fcSName as QsProd ";
            strSQLStr += ", PdGrp.fcCode as QcPdGrp, PdGrp.fcName as QnPdGrp ";
            strSQLStr += ", TradeTrm.fcCode as QcTradeTrm, TradeTrm.fcName as QnTradeTrm ";
            strSQLStr += ", RefProd.fnQty, RefProd.fnPriceKe, RefProd.fcDiscStr, RefProd.fnDiscAmt, RefProd.fnXRate ";

            strSQLStr += "from GLRef ";
            strSQLStr += "left join RefProd on RefProd.fcGLRef = GLRef.fcSkid ";
            strSQLStr += "left join Coor on Coor.fcSkid = GLRef.fcCoor ";
            strSQLStr += "left join CrZone on CrZone.fcSkid = Coor.fcCrZone ";
            strSQLStr += "left join Empl on Empl.fcSkid = GLRef.fcEmpl ";
            strSQLStr += "left join Prod on Prod.fcSkid = RefProd.fcProd ";
            strSQLStr += "left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            strSQLStr += "left join TradeTrm on TradeTrm.fcSkid = GLRef.fcTradeTrm ";
            strSQLStr += " where GLRef.fcCorp = ? and GLRef.fcBranch = ? and GLRef.fcRefType = ? and GLRef.fcBook = ? and GLRef.fcCode between ? and ? order by GLRef.fcCode";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrBookID, inBegCode, inEndCode });

            string strRPTFileName = inRPTFileName;
            if (strRPTFileName != string.Empty)
            {
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, Report.Agents.PrintField.xd_PRN_FORM_H, this.mstrHTable, strSQLStr, ref strErrorMsg))
                {

                    string strCurrCode = "";
                    decimal decSumQty = 0;
                    string strRefToCode = ""; string strRefToDate = ""; string strRefToReceDate = "";

                    foreach (DataRow dtrPFormH in this.dtsDataEnv.Tables[Report.Agents.PrintField.xd_PRN_FORM_H].Rows)
                    {

                        DataRow dtrPrnData = dtsPrintPreview.FORMOBJ.NewRow();

                        string strRemark = (Convert.IsDBNull(dtrPFormH["CrMemData"]) ? "" : dtrPFormH["CrMemData"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["CrMemData2"]) ? "" : dtrPFormH["CrMemData2"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["CrMemData3"]) ? "" : dtrPFormH["CrMemData3"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["CrMemData4"]) ? "" : dtrPFormH["CrMemData4"].ToString().TrimEnd());
                        if (dtrPFormH["CrMemData5"] != null)
                            strRemark += (Convert.IsDBNull(dtrPFormH["CrMemData5"]) ? "" : dtrPFormH["CrMemData5"].ToString().TrimEnd());

                        //ลูกค้า
                        dtrPrnData["F1020"] = dtrPFormH["QnCoor"].ToString();
                        dtrPrnData["F1021"] = dtrPFormH["QcCoor"].ToString();

                        string strRetVal = BizRule.GetMemData(strRemark, "A11");
                        string stRemark2 = BizRule.GetMemData(strRemark, "A21");
                        if (dtrPFormH["fcZip"].ToString().TrimEnd() != string.Empty && stRemark2.TrimEnd() == string.Empty)
                        {
                            strRetVal = strRetVal.Trim() + " " + dtrPFormH["fcZip"].ToString().TrimEnd();
                        }


                        dtrPrnData["F1023"] = this.pmPTextField2(strRetVal);

                        strRetVal = BizRule.GetMemData(strRemark, "A21");
                        string stRemark3 = BizRule.GetMemData(strRemark, "A31");
                        if (dtrPFormH["fcZip"].ToString().TrimEnd() != string.Empty && stRemark3.TrimEnd() == string.Empty)
                        {
                            strRetVal = strRetVal.Trim() + " " + dtrPFormH["fcZip"].ToString().TrimEnd();
                        }

                        dtrPrnData["F1024"] = this.pmPTextField2(strRetVal);

                        string strTel = BizRule.GetMemData(strRemark, "Tel");
                        if (strTel.Trim() == "")
                        {
                            dtrPrnData["F1025"] = this.pmPTextField2(dtrPFormH["fcTel"].ToString().TrimEnd());
                        }
                        else
                        {
                            dtrPrnData["F1025"] = this.pmPTextField2(strTel);
                        }
                        
                        dtrPrnData["F1026"] = this.pmPTextField2(BizRule.GetMemData(strRemark, "Fax"));

                        dtrPrnData["F1160"] = this.pmPTextField2(BizRule.GetMemData(strRemark, "C11"));
                        dtrPrnData["F1161"] = this.pmPTextField2(BizRule.GetMemData(strRemark, "C21"));
                        dtrPrnData["F1162"] = this.pmPTextField2(BizRule.GetMemData(strRemark, "C31"));

                        strRemark = (Convert.IsDBNull(dtrPFormH["fmMemData"]) ? "" : dtrPFormH["fmMemData"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["fmMemData2"]) ? "" : dtrPFormH["fmMemData2"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["fmMemData3"]) ? "" : dtrPFormH["fmMemData3"].ToString().TrimEnd());
                        strRemark += (Convert.IsDBNull(dtrPFormH["fmMemData4"]) ? "" : dtrPFormH["fmMemData4"].ToString().TrimEnd());
                        if (dtrPFormH["fmMemData5"] != null)
                            strRemark += (Convert.IsDBNull(dtrPFormH["fmMemData5"]) ? "" : dtrPFormH["fmMemData5"].ToString().TrimEnd());

                        string strRemark1 = BizRule.GetMemData(strRemark, "Rem");
                        string strRemark2 = BizRule.GetMemData(strRemark, "Rm2");
                        string strRemark3 = BizRule.GetMemData(strRemark, "Rm3");
                        string strRemark4 = BizRule.GetMemData(strRemark, "Rm4");
                        string strRemark5 = BizRule.GetMemData(strRemark, "Rm5");
                        string strRemark6 = BizRule.GetMemData(strRemark, "Rm6");
                        string strRemark7 = BizRule.GetMemData(strRemark, "Rm7");
                        string strRemark8 = BizRule.GetMemData(strRemark, "Rm8");
                        string strRemark9 = BizRule.GetMemData(strRemark, "Rm9");
                        string strRemark10 = BizRule.GetMemData(strRemark, "RmA");

                        dtrPrnData["F1040"] = Convert.ToDateTime(dtrPFormH["fdDate"]);
                        dtrPrnData["F1045"] = dtrPFormH["fnCredTerm"].ToString();
                        dtrPrnData["F1046"] = (Convert.IsDBNull(dtrPFormH["fdDueDate"]) ? DateTime.MinValue : Convert.ToDateTime(dtrPFormH["fdDueDate"]));
                        dtrPrnData["F1047"] = this.pmPTextField2(strRemark1);
                        //dtrPrnData["F1049"] = (Convert.IsDBNull(dtrPFormH["fdReceDate"]) ? DateTime.MinValue : Convert.ToDateTime(dtrPFormH["fdReceDate"]));
                        dtrPrnData["F1050"] = dtrPFormH["fcRefNo"].ToString().TrimEnd();
                        dtrPrnData["F1051"] = dtrPFormH["QcSEmpl"].ToString().TrimEnd();
                        dtrPrnData["F1052"] = dtrPFormH["QnSEmpl"].ToString().TrimEnd();
                        dtrPrnData["F1090"] = 0;

                        dtrPrnData["F1191"] = dtrPFormH["QnCrZone"].ToString().TrimEnd();

                        decimal decSumAmt = 0;
                        if (dtrPFormH["fcVatIsOut"].ToString() == "Y")
                        {
                            decSumAmt = Convert.ToDecimal(dtrPFormH["fnAmt"]) + Convert.ToDecimal(dtrPFormH["fnDiscAmt1"]);
                        }
                        else
                        {
                            decSumAmt = Convert.ToDecimal(dtrPFormH["fnAmt"]) + Convert.ToDecimal(dtrPFormH["fnVatAmt"]) + Convert.ToDecimal(dtrPFormH["fnDiscAmt1"]);
                        }
                        dtrPrnData["F1210"] = decSumAmt;
                        //ส่วนลด : จำนวนเงิน ส่วนลดรวมทั้งใบ
                        dtrPrnData["F1220"] = Convert.ToDecimal(dtrPFormH["fnDiscAmt1"]);

                        decimal decSumAmt_F1250 = 0;
                        switch (dtrPFormH["fcRfType"].ToString())
                        {
                            case "XXXX":
                                break;
                            default:
                                decSumAmt_F1250 = Convert.ToDecimal(dtrPFormH["fnAmt"]) + Convert.ToDecimal(dtrPFormH["fnVATAmt"]);
                                break;
                        }

                        dtrPrnData["F1250"] = decSumAmt_F1250;
                        //จำนวนเงิน : จำนวนเงินสุทธิภาษาไทย เช่น เก้าสิบบาท
                        string strAmtText = "";
                        try
                        {
                            strAmtText = Report.Agents.N2Alpha.ConvNumberToText(Convert.ToDouble(decSumAmt_F1250));
                        }
                        catch (Exception ex)
                        {
                            strAmtText = "";
                        }
                        dtrPrnData["F1251"] = strAmtText;

                        dtrPrnData["I2021"] = dtrPFormH["QcProd"].ToString().TrimEnd();
                        dtrPrnData["I2023"] = dtrPFormH["QnProd"].ToString().TrimEnd();
                        dtrPrnData["I2026"] = dtrPFormH["QsProd"].ToString().TrimEnd();
                        dtrPrnData["I2036"] = dtrPFormH["QcPdGrp"].ToString().TrimEnd();

                        decimal decQty = 0;
                        decQty = Convert.ToDecimal(dtrPFormH["fnQty"]);
                        //จำนวน : จำนวนสินค้า
                        dtrPrnData["I2041"] = decQty;
                        
                        decimal decPrice = 0;
                        decPrice = Convert.ToDecimal(dtrPFormH["fnPriceKe"]) *Convert.ToDecimal(dtrPFormH["fnXRate"]);
                        dtrPrnData["I2042"] = decPrice;

                        //ส่วนลด : ส่วนลด % แต่ละรายการสินค้า
                        string strDiscStr = dtrPFormH["fcDiscStr"].ToString().Trim();
                        if ("%".IndexOf(strDiscStr) > -1 && "+".IndexOf(strDiscStr) < 0)
                        {
                            dtrPrnData["I2050"] = strDiscStr;
                        }
                        //ส่วนลด : ส่วนลด ตามที่ป้อน แต่ละรายการ
                        dtrPrnData["I2052"] = strDiscStr;

                        //มูลค่า : มูลค่า (ราคา X จำนวน)
                        decimal decVal_2060 = Math.Round(Convert.ToDecimal(dtrPFormH["fnQty"]) * Convert.ToDecimal(dtrPFormH["fnPriceKe"]) * Convert.ToDecimal(dtrPFormH["fnXRate"]) - Convert.ToDecimal(dtrPFormH["fnDiscAmt"]), 2, MidpointRounding.AwayFromZero);
                        dtrPrnData["I2060"] = decVal_2060;

                        dtrPrnData["F3113"] = this.pmPTextField2(strRemark2);
                        //รายละเอียดเอกสาร : หมายเหตุ 3 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3114"] = this.pmPTextField2(strRemark3);
                        //รายละเอียดเอกสาร : หมายเหตุ 4 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3115"] = this.pmPTextField2(strRemark4);
                        //รายละเอียดเอกสาร : หมายเหตุ 5 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3116"] = this.pmPTextField2(strRemark5);
                        //รายละเอียดเอกสาร : หมายเหตุ 6 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3117"] = this.pmPTextField2(strRemark6);
                        //รายละเอียดเอกสาร : หมายเหตุ 7 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3118"] = this.pmPTextField2(strRemark7);
                        //รายละเอียดเอกสาร : หมายเหตุ 8 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3119"] = this.pmPTextField2(strRemark8);
                        //รายละเอียดเอกสาร : หมายเหตุ 9 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3120"] = this.pmPTextField2(strRemark9);
                        //รายละเอียดเอกสาร : หมายเหตุ10 , รายละเอียดที่หัวเอกสาร
                        dtrPrnData["F3121"] = this.pmPTextField2(strRemark10);

                        if (strCurrCode != dtrPFormH["fcCode"].ToString())
                        {
                            strCurrCode = dtrPFormH["fcCode"].ToString();

                            //จำนวนสินค้า : จำนวนรวมสินค้าทุกตัวของทั้งใบ INVOICE
                            decSumQty = this.lpfSumRefQty("F3109", dtrPFormH["fcSkid"].ToString());
                            //รายละเอียดเอกสาร : อ้างอิงเลขที่ใบสั่งซื้อ/ใบขอโอนสินค้า
                            this.pmGetRefToCodeH(dtrPFormH["fcRefType"].ToString(), dtrPFormH["fcSkid"].ToString(), ref strRefToCode, ref strRefToDate, ref strRefToReceDate);
                        }

                        //จำนวนสินค้า : จำนวนรวมสินค้าทุกตัวของทั้งใบ INVOICE
                        dtrPrnData["F3109"] = decSumQty;
                        //รายละเอียดเอกสาร : อ้างอิงเลขที่ใบสั่งซื้อ/ใบขอโอนสินค้า
                        dtrPrnData["F3122"] = strRefToCode;
                        dtrPrnData["F3141"] = strRefToDate;
                        dtrPrnData["F1049_2"] = strRefToReceDate;

                        dtrPrnData["F7119"] = dtrPFormH["QnTradeTrm"].ToString().TrimEnd();

                        dtsPrintPreview.FORMOBJ.Rows.Add(dtrPrnData);

                    }

                    if (dtsPrintPreview.FORMOBJ.Rows.Count > 0)
                        this.pmPreviewReport(dtsPrintPreview, strRPTFileName);

                }
            }
        }

        private string pmGetRefToCodeH(string inRefType, string inMasterH, ref string ioRefCode, ref string ioRefDate, ref string ioRefReceDate)
        {
            string strErrorMsg = "";

            string strRefToCode = "";
            string strRefToRefNo = "";
            string strRefToDate = "";
            string strRefToReceDate = "";

            WS.Data.Agents.cDBMSAgent poSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strNoteCut = "select fcChildH from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? group by fcChildH";
            poSQLHelper.SetPara(new object[] { inRefType, inMasterH });
            if (poSQLHelper.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg))
            {
                string strSQLRefToStr = "select BOOK.FCCODE as QCBOOK, ORDERH.FCCODE,ORDERH.FCREFNO,ORDERH.FDDATE,ORDERH.FDRECEDATE from ORDERH left join BOOK on BOOK.FCSKID = ORDERH.FCBOOK where ORDERH.FCSKID = ?";
                for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count; intCnt++)
                {
                    string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[intCnt]["fcChildH"].ToString();
                    if ((poSQLHelper.SetPara(new object[] { strRefToHRowID })
                        && poSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", strSQLRefToStr, ref strErrorMsg)))
                    {
                        DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        DateTime dttDate = Convert.ToDateTime(dtrRefTo["fdDate"]);
                        int intYear = dttDate.Year+543;

                        DateTime dttReceDate = Convert.ToDateTime(dtrRefTo["fdReceDate"]);
                        int intYear2 = dttReceDate.Year+543;

                        string strDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right(intYear.ToString("####"), 2);
                        string strReceDate = dttReceDate.ToString("dd/MM/") + AppUtil.StringHelper.Right(intYear2.ToString("####"), 2);

                        strRefToCode += "SO" + dtrRefTo["QcBook"].ToString().TrimEnd() + "/"+dtrRefTo["fcCode"].ToString() + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                        strRefToRefNo += dtrRefTo["fcRefNo"].ToString() + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                        strRefToDate += strDate + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                        strRefToReceDate += strReceDate + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                    }
                }
            }
            ioRefCode = strRefToCode;
            ioRefDate = strRefToDate;
            ioRefReceDate = strRefToReceDate;
            return strRefToRefNo;
        }

        private decimal lpfSumRefQty(string inFieldNo, string tcinRowID)
        {
            string strErrorMsg = "";
            decimal lnSumQty = 0;
            WS.Data.Agents.cDBMSAgent poSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            poSQLHelper.SetPara(new object[] { tcinRowID });
            if (poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_SumRefQty", "REFPROD", "select fcRfType, fcRefPdTyp, fcIOType, fnQty, fnUmQty, fnStQty from REFPROD where fcGLRef = ? order by fcSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["Q1_SumRefQty"].Rows)
                {
                    if ("T,W,G".IndexOf(dtrRefProd["fcRfType"].ToString()) > -1)
                    {
                        if (dtrRefProd["fcRefPdTyp"].ToString() == "P"
                            && dtrRefProd["fcIOType"].ToString() == "O")
                        {
                            switch (inFieldNo)
                            {
                                case "F3139":
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnStQty"]);
                                    break;
                                default:
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnQty"]) * (inFieldNo == "F3138" ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (dtrRefProd["fcRefPdTyp"].ToString() == "P")
                        {
                            switch (inFieldNo)
                            {
                                case "F3139":
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnStQty"]) * (dtrRefProd["fcIOType"].ToString() == "O" ? -1 : 1);
                                    break;
                                default:
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnQty"]) * (inFieldNo == "F3138" ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"])) * (dtrRefProd["fcIOType"].ToString() == "O" ? -1 : 1);
                                    break;
                            }
                        }
                    }
                }
            }
            return lnSumQty;
        }

        private void pmPrintRangeData3(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";
            string strSQLStrOrderI = "select * from " + this.mstrITable + " where fcGLRef = ? order by fcSeq";


            BeSmartMRP.Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new BeSmartMRP.Report.LocalDataSet.FORM2PRINT();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //object[] pAPara = new object[3] { App.ActiveCorp.CorpID, inBegCode, inEndCode};

            Report.Agents.PrintField oPrintField = new Report.Agents.PrintField(pobjSQLUtil, App.ActiveCorp);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrHTable + " where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode between ? and ? and fcStat <> 'C' order by fcCode";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrBookID, inBegCode, inEndCode });

            string strRPTFileName = inRPTFileName;
            if (strRPTFileName != string.Empty)
            {
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, Report.Agents.PrintField.xd_PRN_FORM_H, this.mstrHTable, strSQLStr, ref strErrorMsg))
                {
                    this.pAHideFMItem.Clear();
                    foreach (DataRow dtrPFormH in this.dtsDataEnv.Tables[Report.Agents.PrintField.xd_PRN_FORM_H].Rows)
                    {

                        string strLastPrnPrefixValue = "";
                        int intRowCount = 0;
                        DataRow dtrLastRow = null;

                        pobjSQLUtil.SetPara(new object[] { dtrPFormH["fcSkid"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, Report.Agents.PrintField.xd_PRN_FORM_I, "OrderI", strSQLStrOrderI, ref strErrorMsg))
                        {

                            string strShowCompo = "";
                            foreach (DataRow dtrPFormI in this.dtsDataEnv.Tables[Report.Agents.PrintField.xd_PRN_FORM_I].Rows)
                            {

                                intRowCount++;

                                DataRow dtrPrnData = dtsPrintPreview.FORMOBJ.NewRow();
                                oPrintField.LoadFieldValue(Report.PrintFieldType.GLRefField, dtrPFormH, ref dtrPrnData);

                                strShowCompo = "";
                                if (dtrPFormI["fcRefPdTyp"].ToString() == SysDef.gc_REFPD_TYPE_FORMULA
                                    && dtrPFormI["fcFormulas"].ToString().TrimEnd() != string.Empty
                                    && pobjSQLUtil.SetPara(new object[] { dtrPFormI["fcFormulas"].ToString() })
                                    && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QFormula", "FORMULA", "select fcSkid,fcShowComp from FORMULAS where FCSKID = ?", ref strErrorMsg))
                                {
                                    DataRow dtrFormula = this.dtsDataEnv.Tables["QFormula"].Rows[0];
                                    strShowCompo = (dtrFormula["fcShowComp"].ToString() == "0" || dtrFormula["fcShowComp"].ToString().TrimEnd() == "" ? App.ActiveCorp.ShowFormulaCompo : dtrFormula["fcShowComp"].ToString());
                                    if (strShowCompo == "2" && this.pAHideFMItem.IndexOf(dtrFormula["fcSkid"].ToString(), 0) < 0)
                                    {
                                        this.pAHideFMItem.Add(dtrFormula["fcSkid"].ToString());
                                    }
                                }

                                if (dtrPFormI["fcPFormula"].ToString().TrimEnd() != ""
                                    && this.pAHideFMItem.IndexOf(dtrPFormI["fcPFormula"].ToString(), 0) > -1)
                                {
                                    continue;
                                }

                                oPrintField.LoadFieldValue(Report.PrintFieldType.RefProdField, dtrPFormI, ref dtrPrnData);
                                //dtsPrintPreview.FORMOBJ.Rows.Add(dtrPrnData);

                                string strQcProd = dtrPrnData["I2021"].ToString();
                                string strQcUM = dtrPrnData["I2031"].ToString();
                                if (this.mbllIsPrintGroup)
                                {
                                    if (strLastPrnPrefixValue == (strQcProd + "," + strQcUM))
                                    {
                                        //จำนวน
                                        dtrLastRow["I2041"] = Convert.ToDecimal(dtrLastRow["I2041"]) + Convert.ToDecimal(dtrPrnData["I2041"]);
                                        //มูลค่า
                                        dtrLastRow["I2060"] = Convert.ToDecimal(dtrLastRow["I2060"]) + Convert.ToDecimal(dtrPrnData["I2060"]);
                                    }
                                    else
                                    {
                                        strLastPrnPrefixValue = (strQcProd + "," + strQcUM);

                                        if (dtrLastRow != null)
                                            dtsPrintPreview.FORMOBJ.Rows.Add(dtrLastRow);

                                        dtrLastRow = dtsPrintPreview.FORMOBJ.NewRow();
                                        DataSetHelper.CopyDataRow(dtrPrnData, ref dtrLastRow);
                                    }

                                    if (intRowCount == this.dtsDataEnv.Tables[Report.Agents.PrintField.xd_PRN_FORM_I].Rows.Count)
                                    {
                                        if (dtrLastRow != null)
                                            dtsPrintPreview.FORMOBJ.Rows.Add(dtrLastRow);
                                    }
                                }
                                else
                                {
                                    dtsPrintPreview.FORMOBJ.Rows.Add(dtrPrnData);
                                }


                            }
                        }
                    }

                    if (dtsPrintPreview.FORMOBJ.Rows.Count > 0)
                        this.pmPreviewReport(dtsPrintPreview, strRPTFileName);

                }
            }
        }


        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName)
        {
            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(inRPTFileName))
            {
                MessageBox.Show("File not found " + inRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rptPreviewReport.Load(inRPTFileName);
            rptPreviewReport.SetDataSource(inData);

            App.PreviewReport(this, false, rptPreviewReport);
        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = dtrBrow["fcCode"].ToString().TrimEnd();

            bool bllCanDelete = false;
            bllCanDelete = (!this.pmCheckHasUsed(ref strErrorMsg))
                && this.pmChkStockBeforeCancel(this.mstrEditRowID, ref strErrorMsg);

            if (bllCanDelete)
            {
                if (MessageBox.Show("ยืนยันการลบเอกสารเลขที่ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {

                    this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                    this.mSaveDBAgent.AppID = App.AppID;
                    this.mdbConn = this.mSaveDBAgent.GetDBConnection();

                    try
                    {

                        this.mdbConn.Open();
                        this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                        //Update Stock
                        this.pmChgItemStat(this.mstrEditRowID, ref strErrorMsg);

                        //Save NoteCut ไว้เพื่อไล่ Update Order Step ในภายหลัง
                        pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", "select fcChildH from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? group by fcChildH", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        //Delete Note Cut
                        pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                        this.mSaveDBAgent.BatchSQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        if (this.mbllIsInv)
                        {
                            //ต้องลบ NoteCut ออกก่อนจึงค่อยวนลูป Update Step จึงจะถูกต้อง
                            foreach (DataRow dtrChildH in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                            {
                                this.pmUpdateOrderHStep(dtrChildH["fcChildH"].ToString());
                            }
                        }

                        pAPara = new object[1] { this.mstrEditRowID };
                        if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", "select * from " + this.mstrHTable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                        {
                            DataRow dtrGLRef = this.dtsDataEnv.Tables["QGLRef"].Rows[0];
                            if (this.mstrIsCash != "Y")
                                this.pmUpdateCoorBal(dtrGLRef, -1);

                        }

                        // ลบ RefProd
                        pAPara = new object[1] { this.mstrEditRowID };
                        this.mSaveDBAgent.BatchSQLExec("delete from RefProd where fcGLRef = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        // ลบ GLRef
                        pAPara = new object[1] { this.mstrEditRowID };
                        this.mSaveDBAgent.BatchSQLExec("delete from GLRef where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID2(this.mstrBrowViewAlias, "fcSkid", this.mstrEditRowID));

                        this.mdbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.mdbTran.Rollback();
                        App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
						MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
                    }
                    finally
                    {
                        this.mdbConn.Close();
                    }

                }
            }
            else
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.mstrEditRowID = "";
        }

        private bool pmCheckHasUsed(ref string ioErrorMsg)
        {

            bool bllResult = false;
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null)
                return false;

            string strRowID = dtrBrow["fcSkid"].ToString();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { strRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrHTable, "select * from " + this.mstrHTable + " where fcSkid = ?", ref strErrorMsg))
            {
                bllResult = (this.pmCanEdit(this.dtsDataEnv.Tables["QHasUsed"].Rows[0], "E") == false);
                bllResult = bllResult || (this.pmCheckIsReferItem(strRowID) == true);
            }
            else
                bllResult = false;

            return bllResult;
        }

        private bool pmCheckIsReferItem(string inGLRef)
        {
            string strErrorMsg = "";
            string strSQLStr = "select RefProd.fcStep, RefProd.fcProd, RefProd.fnQty, RefProd.fnBackQty";
            strSQLStr += ", Prod.fcCode as fcQcProd, Prod.fcName as fcQnProd ";
            strSQLStr += " from RefProd left join Prod on Prod.fcSkid = RefProd.fcProd where RefProd.fcGLRef = ?";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inGLRef });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrITable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QHasUsed"].Rows)
                {
                    string strProdMsg = "(" + dtrRefProd["fcQcProd"].ToString().TrimEnd() + ") " + dtrRefProd["fcQnProd"].ToString().TrimEnd();
                    if (dtrRefProd["fcStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                    {
                        MessageBox.Show("สินค้า " + strProdMsg + " ส่งของครบแล้ว", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    else if (Convert.ToDecimal(dtrRefProd["fnBackQty"]) < Convert.ToDecimal(dtrRefProd["fnQty"]))
                    {
                        MessageBox.Show("สินค้า " + strProdMsg + " ส่งของแล้วบางส่วน", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
            }
            return false;
        }

        private void pmSaveData()
        {
            string strErrorMsg = "";
            this.txtCode.Focus();
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                if (this.pmReplRecord(ref strErrorMsg))
                {
                    //dlgWaitWind dlg = new dlgWaitWind();
                    //dlg.WaitWind("บันทึกเรียบร้อย", 500);
                    ////MessageBox.Show("บันทึกเรียบร้อย", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                        //this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_BROWSE;
                        this.pmGotoBrowPage();
                    else
                        this.pmInsertLoop();
                    //dlg.WaitClear();
                }
            }
            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool pmValidBeforeSave(ref string ioErrorMsg)
        {
            bool bllResult = true;

            //Force Update TemPd Before Save
            this.gridView2.UpdateCurrentRow();
            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                if (MessageBox.Show("ยังไม่ได้ระบุเลขที่เอกสารต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }
            DateTime dttCorpStartDate = App.ActiveCorp.StartAppDate;
            if (this.mbllCanEdit == false)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "เอกสารไม่อนุญาตให้แก้ไข";
                this.txtCode.Focus();
                return false;
            }
            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "ยังไม่ได้ระบุเอกสาร";
                this.txtCode.Focus();
                return false;
            }
            if (this.txtQcCoor.Text.Trim() == string.Empty)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "ยังไม่ได้ชื่อลูกค้า";
                this.txtQcCoor.Focus();
                return false;
            }
            else if (dttCorpStartDate.Date.CompareTo(this.txtDate.DateTime.Date) > 0)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "วันที่เอกสาร" + this.mstrRefTypeName.TrimEnd() + " ไม่ควรเป็นวันที่ก่อนเริ่มใช้ระบบ";
                this.txtDate.Focus();
                return false;
            }
            else if (this.txtDate.DateTime.Date.CompareTo(this.mdttDueDate.Date) > 0)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "วันที่ยืนราคา ต้องมากกว่าวันที่เอกสาร";
                this.txtDate.Focus();
                return false;
            }
            else if (this.txtDate.DateTime.Date.CompareTo(this.mdttSendDate.Date) > 0)
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "วันที่ส่งของ ต้องมากกว่าวันที่เอกสาร";
                this.txtDate.Focus();
                return false;
            }
            else if (!this.pmChkItemOK(ref ioErrorMsg))
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                this.grdTemPd.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && this.pmChkDupCode())
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "เลขที่เอกสารซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldRefNo.TrimEnd() != this.txtRefNo.Text.TrimEnd())
                && this.pmChkDupRefNo())
            {
                this.pgfMainEdit.SelectedTabPageIndex = xd_PAGE_EDIT1;
                ioErrorMsg = "เลขที่อ้างอิงซ้ำ";
                this.txtRefNo.Focus();
                return false;
            }
            else
            {

                string strVatSeqPrefix = BizRule.GetVatSeqPrefix(this.mdttReceDate);
                if (this.txtVatType.Tag.ToString() != string.Empty
                    && this.txtVatType.Tag.ToString() != SysDef.gc_VAT_NOT_PRN
                    && this.mstrVatDue == "Y"
                    && this.mstrSaleOrBuy == "P")
                {

                    string strErrorMsg = "";

                    WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                    object[] pAPara = null;
                    if (this.mstrVatSeq.TrimEnd() == string.Empty)
                    {
                        pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.txtVatType.Tag };
                        this.mstrVatSeq = BizRule.RunVatSeq(pobjSQLUtil, pAPara, strVatSeqPrefix, ref strErrorMsg);
                    }

                    //MessageBox.Show(strErrorMsg + "\n" + "VQ = " + this.mstrVatSeq);

                    if (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldVatSeq != this.mstrVatSeq)
                    {
                        string strRefNo = "";
                        pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.txtVatType.Tag, this.mstrVatSeq };
                        if (BizRule.ChkDupVatSeq(pobjSQLUtil, pAPara, ref strRefNo))
                        {

                            if (this.mstrVatSeq != string.Empty)
                            {
                                ioErrorMsg = "ลำดับที่ [" + this.mstrVatSeq.Substring(6, 5) + "] เพื่อออกรายงานภาษีซื้อ\n";
                                ioErrorMsg += "ซ้ำกับของเอกสารเลขที่ " + strRefNo + "\n";
                                ioErrorMsg += "กรุณาลองกดปุ่ม F10 อีกครั้งเพื่อทำการ Save";
                            }
                            bllResult = false;
                        }
                    }
                }
                else
                {
                    bllResult = true;
                }

            }

            return bllResult;
        }

        private bool pmChkItemOK(ref string ioErrorMsg)
        {
            bool bllResult = false;
            bool bllHasWhouse = true;
            bool bllHasWHLoca = true;

            DataRow dtrErrRow = null;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                if (dtrTemPd["cProd"].ToString() != string.Empty && dtrTemPd["cWHouse"].ToString().TrimEnd() == string.Empty)
                {
                    bllHasWhouse = false;
                    dtrErrRow = dtrTemPd;
                    break;
                }
            }

            if (!bllHasWhouse)
            {
                ioErrorMsg = "รายการที่ " + Convert.ToInt32(dtrErrRow["nRecNo"]).ToString("###") + " ยังไม่ได้ระบุคลัง save ไม่ได้";
            }
            else
            {
                bllResult = true;
            }

            return bllResult;
        }

        //		private bool pmValidDueDate()
        //		{
        //			bool bllResult = true;
        //			if (this.mdttDueDate.Date.CompareTo(this.txtDate.Value.Date) > 0)
        //			{
        //				bllResult = false;
        //			}
        //			return bllResult;
        //		}

        private bool pmReplRecord(ref string ioErrorMsg)
        {
            bool bllResult = true;
            bool bllIsNewRow = false;
            string strErrorMsg = "";
            string strRowID = "";

            DataRow dtrCurrRow = null;
            DataRow dtrGLRef = null;

            if (this.mFormEditMode == UIHelper.AppFormState.Insert)
            {
                strRowID = App.mRunRowID(this.mstrHTable);
                this.mstrEditRowID = strRowID;
                dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].NewRow();
                bllIsNewRow = true;
                dtrGLRef["fcCreateAp"] = App.AppID;
                dtrGLRef["fcCreateBy"] = App.FMAppUserID;
            }
            else
            {
                dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
                bllIsNewRow = false;
            }

            string gcTemStr01 = BizRule.SetMemData(this.mstrRemarkH1.TrimEnd(), "Rem")
                + BizRule.SetMemData(this.mstrInvDetail, "Det")
                + BizRule.SetMemData(this.mstrRemarkH2.TrimEnd(), "Rm2")
                + BizRule.SetMemData(this.mstrRemarkH3.TrimEnd(), "Rm3")
                + BizRule.SetMemData(this.mstrRemarkH4.TrimEnd(), "Rm4")
                + BizRule.SetMemData(this.mstrRemarkH5.TrimEnd(), "Rm5")
                + BizRule.SetMemData(this.mstrRemarkH6.TrimEnd(), "Rm6")
                + BizRule.SetMemData(this.mstrRemarkH7.TrimEnd(), "Rm7")
                + BizRule.SetMemData(this.mstrRemarkH8.TrimEnd(), "Rm8")
                + BizRule.SetMemData(this.mstrRemarkH9.TrimEnd(), "Rm9")
                + BizRule.SetMemData(this.mstrRemarkH10.TrimEnd(), "RmA");

            string strRefNo = this.txtRefNo.Text;
            if (strRefNo.TrimEnd() == string.Empty)
            {
                //strRefNo = this.mstrRefType + this.mstrQcBook.TrimEnd() + "/" + this.txtCode.Text.TrimEnd();
                strRefNo = this.mstrBookPrefix + this.txtCode.Text.TrimEnd();
            }

            //string strVatSeq = "";
            decimal decVATRate = Convert.ToDecimal(StringHelper.Left(this.txtVatRate.Text, this.txtVatRate.Text.Length - 1));

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            strRowID = this.mstrEditRowID;

            dtrGLRef["fcSkid"] = this.mstrEditRowID;
            dtrGLRef["fcCorp"] = App.ActiveCorp.RowID;
            dtrGLRef["fcBranch"] = this.mstrBranchID;
            dtrGLRef["fcStep"] = this.mstrStep;
            dtrGLRef["fcAtStep"] = this.mstrAtStep;
            dtrGLRef["fcRefType"] = this.mstrRefType;
            dtrGLRef["fcRfType"] = this.mstrRfType;
            dtrGLRef["fcBook"] = this.mstrBookID;
            dtrGLRef["fcCode"] = this.txtCode.Text.TrimEnd();
            dtrGLRef["fcRefNo"] = strRefNo;
            dtrGLRef["fdDate"] = this.txtDate.DateTime.Date;
            dtrGLRef["fdReceDate"] = this.mdttReceDate;
            dtrGLRef["fdDueDate"] = this.mdttDueDate;
            dtrGLRef["fcSect"] = this.txtQcDept.Tag;
            dtrGLRef["fcDept"] = this.mstrDivision;
            dtrGLRef["fcJob"] = this.txtQcJob.Tag;
            dtrGLRef["fcProj"] = this.mstrProj;
            dtrGLRef["fcVatIsOut"] = this.txtVatIsOut.Text;
            dtrGLRef["fcVatType"] = this.txtVatType.Tag;
            dtrGLRef["fnVatrate"] = decVATRate;
            dtrGLRef["fcCoor"] = this.txtQcCoor.Tag;
            dtrGLRef["fcEmpl"] = this.mstrSEmpl;
            dtrGLRef["fcRecvMan"] = this.mstrRecvMan;
            dtrGLRef["fcDeliCoor"] = this.mstrDeliCoor;
            dtrGLRef["fcVatCoor"] = this.mstrVatCoor;
            dtrGLRef["fnCredTerm"] = this.mintCredTerm;
            dtrGLRef["fcIsCash"] = this.mstrIsCash;
            dtrGLRef["fcHasRet"] = this.mstrHasReturn;
            dtrGLRef["fcVatDue"] = this.mstrVatDue;

            dtrGLRef["fcTradeTrm"] = this.mstrTradeTrm;

            dtrGLRef["fcDiscStr"] = this.txtDiscount.Text.TrimEnd();
            dtrGLRef["fcCurrency"] = this.txtQcCurrency.Tag;
            dtrGLRef["fnXRate"] = this.txtXRate.Value;
            dtrGLRef["fnAmtKe"] = this.txtAmt.Value;
            dtrGLRef["fnVatAmtKe"] = this.txtVatAmt.Value;
            dtrGLRef["fnDiscAmtK"] = this.txtDiscountAmt.Value;
            dtrGLRef["fnAmt"] = Convert.ToDecimal(this.txtAmt.Value) * Convert.ToDecimal(this.txtXRate.Value);
            dtrGLRef["fnVatAmt"] = Convert.ToDecimal(this.txtVatAmt.Value) * Convert.ToDecimal(this.txtXRate.Value);
            dtrGLRef["fnDiscAmt1"] = Convert.ToDecimal(this.txtDiscountAmt.Value) * Convert.ToDecimal(this.txtXRate.Value);
            dtrGLRef["fnAmt2"] = 0;	//fnAmt2
            //dtrGLRef["fcCreateBy"] = "";	//fcCreateBy
            //dtrGLRef["fcCorrectB"] = "";	//fcCorrectB
            dtrGLRef["fcCorrectB"] = App.FMAppUserID;
            dtrGLRef["fcApproveB"] = ""; //fcApproveB
            dtrGLRef["fcLUpdApp"] = App.AppID;	//fcLUpdApp
            dtrGLRef["fcDataser"] = "";	//fcDataser
            dtrGLRef["fcEAfterR"] = "E";

            dtrGLRef["fcDebtCode"] = this.mstrDebtCode;
            dtrGLRef["fdDebtDate"] = this.mdttDebtDate;
            dtrGLRef["fcVatSeq"] = this.mstrVatSeq;

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrGLRef["fmMemData"] = gcTemStr02;
            if (DataSetHelper.HasField("fmMemData2", dtrGLRef))
            {
                dtrGLRef["fmMemData2"] = gcTemStr03;
                dtrGLRef["fmMemData3"] = gcTemStr04;
                dtrGLRef["fmMemData4"] = gcTemStr05;
            }

            if (DataSetHelper.HasField("fmMemData5", dtrGLRef))
            {
                dtrGLRef["fmMemData5"] = gcTemStr06;
            }

            if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                this.dtsDataEnv.Tables[this.mstrHTable].Rows.Add(dtrGLRef);

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent2.AppID = App.AppID;
            this.mdbConn2 = this.mSaveDBAgent2.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.mdbConn2.Open();
                this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

                if (this.mFormEditMode == UIHelper.AppFormState.Edit && this.mstrIsCash != "Y")
                {
                    this.pmUpdateCoorBal(dtrGLRef, -1);
                }

                //Clear Old Ref To Order
                this.pmUpdateDeleteTemRefTo();

                decimal decDiscAmtI = 0;
                this.pmSaveRefProd(this.mstrEditRowID, ref decDiscAmtI);

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    this.pmUpdateStockForInsert();
                }

                dtrGLRef["fnDiscAmtI"] = decDiscAmtI;

                if (this.mstrIsCash != "Y")
                {
                    this.pmUpdateCoorBal(dtrGLRef, 1);
                }
                else
                {
                    decimal decHAmt = Convert.ToDecimal(dtrGLRef["fnAmt"]) + Convert.ToDecimal(dtrGLRef["fnVATAmt"]);
                    this.mstrStep = SysDef.gc_REF_STEP_PAY;
                    dtrGLRef["fcStep"] = SysDef.gc_REF_STEP_PAY;
                    dtrGLRef["fnPayAmt"] = decHAmt;
                }

                //Update GLRef
                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrGLRef, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                //pobjSQLUtil.SetPara(pAPara);
                //bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr, ref strErrorMsg);

                //this.mDbCommand = this.mSaveDBAgent.GetDBCommand(strSQLUpdateStr, this.mdbConn, this.mdbTran);
                //this.mSaveDBAgent.SetPara(pAPara, ref this.mDbCommand);
                //int intResult = this.mDbCommand.ExecuteNonQuery();

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //UPDATE REFTO DOC STEP
                this.pmSaveTemRefTo();

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                this.mdbTran2.Rollback();
                App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }
            finally
            {
                this.mdbConn.Close();
                this.mdbConn2.Close();
            }

            //Update BrowseView Page
            if (bllResult)
            {
                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                    dtrCurrRow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();
                else
                    dtrCurrRow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[this.pmGetRowID2(this.mstrBrowViewAlias, "FCSKID", this.mstrEditRowID)];

                string strStep = this.pmGetOrderStepName(this.mstrStep);

                dtrCurrRow["fcSkid"] = this.mstrEditRowID;
                dtrCurrRow["fcStep"] = strStep;
                dtrCurrRow["fcAtStep"] = this.mstrAtStep;
                dtrCurrRow["fcCode"] = this.txtCode.Text.TrimEnd();
                dtrCurrRow["fcRefNo"] = strRefNo;
                dtrCurrRow["fdDate"] = this.txtDate.DateTime.Date;
                dtrCurrRow["cQnCoor"] = StringHelper.PadR(this.txtQsCoor.Text, this.txtQsCoor.Properties.MaxLength);
                dtrCurrRow["fcSign"] = this.txtQcCurrency.Text.TrimEnd();
                dtrCurrRow["nAmt"] = this.txtGrossAmt.Value;
                dtrCurrRow["ftDateTime"] = DateTime.Now;
                dtrCurrRow["ftLastUpd"] = DateTime.Now;
                dtrCurrRow["CLOGIN_ADD"] = App.AppUserName;
                dtrCurrRow["CLOGIN_UPD"] = App.AppUserName;
                
                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrCurrRow);
                    this.gridView1.MoveLast();
                }
            }
            return bllResult;
        }

        private void pmUpdateCoorBal(DataRow inGLRef, int inUpdSign)
        {
            bool bllIsNewRow = false;
            string strErrorMsg = "";
            DataRow dtrCoorBal;

            object[] pAPara = new object[] { inGLRef["fcCorp"].ToString(), inGLRef["fcBranch"].ToString(), this.mstrCoorType, inGLRef["fcCoor"].ToString() };
            if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "CoorBal", "COORBAL", "select * from CoorBal where fcCorp=? and fcBranch = ? and fcCoorType = ? and fcCoor = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                dtrCoorBal = this.dtsDataEnv.Tables["CoorBal"].NewRow();
                bllIsNewRow = true;
                string strRowID = App.mRunRowID("COORBAL");
                dtrCoorBal["fcSkid"] = strRowID;
                dtrCoorBal["fcCorp"] = inGLRef["fcCorp"].ToString();
                dtrCoorBal["fcBranch"] = inGLRef["fcBranch"].ToString();
                dtrCoorBal["fcCoorType"] = this.mstrCoorType;
                dtrCoorBal["fcCoor"] = inGLRef["fcCoor"].ToString();
                dtrCoorBal["fcCreateAp"] = App.AppID;
                dtrCoorBal["fnAmt"] = 0;
            }
            else
            {
                bllIsNewRow = false;
                dtrCoorBal = this.dtsDataEnv.Tables["CoorBal"].Rows[0];
            }
            dtrCoorBal["fnAmt"] = Convert.ToDecimal(dtrCoorBal["fnAmt"]) + (Convert.ToDecimal(inGLRef["fnAmt"]) + Convert.ToDecimal(inGLRef["fnVatAmt"])) * 1 * inUpdSign;
            dtrCoorBal["fcLUpdApp"] = App.AppID;
            dtrCoorBal["fcEAfterR"] = "E";

            //Update CoorBal
            string strSQLUpdateStr = "";
            pAPara = null;
            cDBMSAgent.GenUpdateSQLString(dtrCoorBal, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
            this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

        }

        private string pmGetOrderStepName(string inStep)
        {
            string strStep = inStep;
            switch (this.mDocType)
            {
                case DocumentType.PR:
                    if (this.mbllWaitForApprove)
                    {
                        if (inStep == SysDef.gc_REF_STEP_PAY)
                        {
                            strStep = "PO";
                        }
                        else if (inStep == SysDef.gc_REF_STEP_CLOSED)
                        {
                            strStep = "CLOSE";
                        }
                        else if (inStep == SysDef.gc_REF_STEP_WAIT_APPROVE)
                        {
                            strStep = "WAIT";
                        }
                        else
                            strStep = "";
                    }
                    else
                        strStep = (inStep == SysDef.gc_REF_STEP_PAY ? "PO" : (inStep == SysDef.gc_REF_STEP_CLOSED ? "CLOSE" : ""));
                    break;
                case DocumentType.QS:
                    strStep = (inStep == SysDef.gc_REF_STEP_PAY ? "SO" : (inStep == SysDef.gc_REF_STEP_CLOSED ? "CLOSE" : ""));
                    break;
                case DocumentType.PO:
                    if (this.mbllWaitForApprove)
                    {
                        if (inStep == SysDef.gc_REF_STEP_PAY)
                        {
                            strStep = "DLVR";
                        }
                        else if (inStep == SysDef.gc_REF_STEP_CLOSED)
                        {
                            strStep = "CLOSE";
                        }
                        else if (inStep == SysDef.gc_REF_STEP_WAIT_APPROVE)
                        {
                            strStep = "WAIT";
                        }
                        else
                            strStep = "";
                    }
                    else
                        strStep = (inStep == SysDef.gc_REF_STEP_PAY ? "DLVR" : (inStep == SysDef.gc_REF_STEP_CLOSED ? "CLOSE" : ""));
                    break;
                case DocumentType.SO:
                    strStep = (inStep == SysDef.gc_REF_STEP_PAY ? "DLVR" : (inStep == SysDef.gc_REF_STEP_CLOSED ? "CLOSE" : ""));
                    break;
                default:
                    strStep = " ";
                    break;
            }
            return strStep;
        }

        private void pmSaveTemRefTo()
        {
            if (this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows.Count > 0)
            {
                foreach (DataRow dtrRefToRow in this.dtsDataEnv.Tables[xd_ALIAS_TEMREFTO].Rows)
                {
                    if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                    {
                        if (this.mbllIsInv)
                            this.pmUpdateOrderHStep(dtrRefToRow["cRowID"].ToString());
                    }
                }
            }
        }

        private void pmUpdateDeleteTemRefTo() { }

        private bool pmSaveRefProd(string ioErrorMsg, ref decimal ioDiscAmtI)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                bool bllIsNewRow = false;

                //if (dtrTemPd["cStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                //	continue;
                //else if (dtrTemPd["cDOStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                //	continue;

                DataRow dtrRefProd = null;
                if (dtrTemPd["cFormula"].ToString().TrimEnd() != string.Empty
                    || dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                //&& (Convert.ToDecimal(dtrTemPd["nQty"]) != 0 ? true : MessageBox.Show("สินค้า " + dtrTemPd["cQcProd"].ToString().TrimEnd() + "\nยังไม่ได้ระบุจำนวนต้องการ Save ด้วยหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    //if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                    //	|| (pobjSQLUtil.SetPara( new object[1] { dtrTemPd["cRowID"].ToString() } ) 
                    //	&& !pobjSQLUtil.SQLExec(ref this.dtsDataEnv,"QChkRefProd", "RefProd", "select fcSkid from RefProd where fcSkid = ?", ref strErrorMsg)))
                    //{

                    decimal decQty = 0;
                    decimal decUMQty = 0;
                    decimal decStQty = 0;
                    decimal decStUmQty = (Convert.ToDecimal(dtrTemPd["nStUOMQty"]) == 0 ? 1 : Convert.ToDecimal(dtrTemPd["nStUOMQty"]));
                    decimal lnStUmQty = (Convert.ToDecimal(dtrTemPd["nStUOMQty"]) == 0 ? 1 : Convert.ToDecimal(dtrTemPd["nStUOMQty"]));

                    decimal lnCostAmt = Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nUOMQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"]) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);

                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID("RefProd");
                        bllIsNewRow = true;
                        dtrRefProd["fcCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrRefProd["fcSkid"] = dtrTemPd["cRowID"].ToString();

                        decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                        decUMQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                        decStQty = Convert.ToDecimal(dtrTemPd["nStQty"]);
                        decStUmQty = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);

                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                        DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
                        if (((BusinessEnum.gc_RFTYPE_INV_SELL + BusinessEnum.gc_RFTYPE_CR_BUY).IndexOf(dtrGLRef["fcRfType"].ToString()) > -1) == false)
                        {
                            bool llChangeLotAndWh = (dtrRefProd["fcLot"].ToString() != dtrTemPd["cLot"].ToString()) || (dtrRefProd["fcWHouse"].ToString() != dtrTemPd["cWHouse"].ToString());

                            if (dtrTemPd["cProd"].ToString() + dtrTemPd["cWHouse"].ToString() + dtrTemPd["cLot"].ToString().TrimEnd()
                                == dtrRefProd["fcLot"].ToString() + dtrRefProd["fcWHouse"].ToString() + dtrRefProd["fcLot"].ToString().TrimEnd())
                            {
                                decQty = ((Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nUOMQty"])) - (Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]))) / Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                                decUMQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);

                                decStQty = ((Convert.ToDecimal(dtrTemPd["nStQty"]) * lnStUmQty) - (Convert.ToDecimal(dtrRefProd["fnStQty"]) * Convert.ToDecimal(dtrRefProd["fnStUmQty"]))) / lnStUmQty;
                                lnCostAmt = Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nUOMQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"]) - Convert.ToDecimal(dtrTemPd["nDiscAmt"])
                                    - Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * Convert.ToDecimal(dtrRefProd["fnPrice"]) - Convert.ToDecimal(dtrRefProd["fnDiscAmt"]);
                            }
                            else
                            {
                                decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                                decUMQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                                decStQty = Convert.ToDecimal(dtrTemPd["nStQty"]);

                                //TODO: Test Update Stock
                                //bllResult = this.pmRetStock(dtrRefProd, dtrTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                            }

                        }
                        else
                        {
                            decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                            decUMQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                            decStQty = Convert.ToDecimal(dtrTemPd["nStQty"]);

                            //TODO: Test Update Stock
                            //bllResult = this.pmRetStock(dtrRefProd, dtrTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                        }

                    }

                    this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, decQty, decUMQty, decStQty, decStUmQty, lnCostAmt);
                    ioDiscAmtI += Convert.ToDecimal(dtrRefProd["fnDiscAmt"]);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    //Save NoteCut
                    this.pmSaveNoteCut(dtrTemPd);

                }
                else
                {
                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[1] { dtrTemPd["cRowID"].ToString() };
                        if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefProd", this.mstrITable, "select * from " + this.mstrITable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                        {
                            DataRow dtrRefProd2 = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                            this.pmRetStock(dtrRefProd2, dtrTemPd["cWHLoca"].ToString(), false, false, "", ref strErrorMsg);
                        }

                        //Delete Note Cut and RefProd
                        pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from RefProd where FCSKID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        //pobjSQLUtil.SetPara(new object[] {this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID"].ToString()});
                        //pobjSQLUtil.SQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", ref strErrorMsg);
                        //pobjSQLUtil.SetPara(new object[] {dtrTemPd["cRowID"].ToString()});
                        //bllResult = pobjSQLUtil.SQLExec("delete from RefProd where FCSKID = ?", ref strErrorMsg);
                    }
                }
            }
            return true;
        }

        private void pmSaveNoteCut(DataRow inTemPd)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != inTemPd["cXRefToRowID"].ToString().TrimEnd()
                && inTemPd["cXRefToRowID"].ToString().TrimEnd() != string.Empty)
            {
                //Delete Note Cut
                //string strDelNoteCut = "delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ? and FCCHILDTYP = ? and FCCHILDH = ? and FCCHILDI = ?";
                //pobjSQLUtil.SetPara( new object[] {this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString()});
                //pobjSQLUtil.SQLExec(strDelNoteCut, ref strErrorMsg);

                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString() };
                this.mSaveDBAgent.BatchSQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ? and FCCHILDTYP = ? and FCCHILDH = ? and FCCHILDI = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != string.Empty
                && Convert.ToDecimal(inTemPd["nQty"]) != 0)
            {
                bool bllIsNewRow_NoteCut = false;
                DataRow dtrNoteCut = null;
                string strRowID = "";
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), this.mstrRefToRefType, inTemPd["cRefToHRowID"].ToString(), inTemPd["cRefToRowID"].ToString() };
                string strNoteCut = "select * from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ? and FCCHILDTYP = ? and FCCHILDH = ? and FCCHILDI = ?";
                if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.NoteCut, MapTable.Table.NoteCut, strNoteCut, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    strRowID = App.mRunRowID(MapTable.Table.NoteCut);
                    bllIsNewRow_NoteCut = true;
                    dtrNoteCut = this.dtsDataEnv.Tables[MapTable.Table.NoteCut].NewRow();
                    dtrNoteCut["fcSkid"] = strRowID;
                    dtrNoteCut["fcCreateAp"] = App.AppID;
                }
                else
                {
                    strRowID = inTemPd["cRefToRowID"].ToString();
                    dtrNoteCut = this.dtsDataEnv.Tables[MapTable.Table.NoteCut].Rows[0];
                }
                dtrNoteCut["fcMasterty"] = this.mstrRefType;
                dtrNoteCut["fcMasterH"] = this.mstrEditRowID;
                dtrNoteCut["fcMasterI"] = inTemPd["cRowID"].ToString();
                dtrNoteCut["fcCorp"] = App.ActiveCorp.RowID;
                dtrNoteCut["fcBranch"] = this.mstrBranchID;
                dtrNoteCut["fcChildTyp"] = this.mstrRefToRefType;
                dtrNoteCut["fcChildH"] = inTemPd["cRefToHRowID"].ToString();
                dtrNoteCut["fcChildI"] = inTemPd["cRefToRowID"].ToString();
                dtrNoteCut["fnQty"] = Convert.ToDecimal(inTemPd["nQty"]);
                dtrNoteCut["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
                dtrNoteCut["fclUpdApp"] = App.AppID;
                dtrNoteCut["fcEAfterR"] = "E";

                string strSQLUpdateStr_NoteCut = "";
                cDBMSAgent.GenUpdateSQLString(dtrNoteCut, "FCSKID", bllIsNewRow_NoteCut, ref strSQLUpdateStr_NoteCut, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr_NoteCut, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //pobjSQLUtil.SetPara(pAPara);
                //pobjSQLUtil.SQLExec(strSQLUpdateStr_NoteCut, ref strErrorMsg);
            }
            else
            {
                //Delete Note Cut
                //pobjSQLUtil.SetPara(new object[] {this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString()});
                //pobjSQLUtil.SQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", ref strErrorMsg);

                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString() };
                this.mSaveDBAgent.BatchSQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }
        }

        private bool pmReplRecordRefProd(bool inState, DataRow inTemPd, ref DataRow ioRefProd, decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, decimal tninCostAmt)
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            bool llSucc = true;
            DataRow dtrRefProd = ioRefProd;

            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];

            string strRemark1 = inTemPd["cRemark1"].ToString().TrimEnd();
            string strRemark2 = inTemPd["cRemark2"].ToString().TrimEnd();
            string strRemark3 = inTemPd["cRemark3"].ToString().TrimEnd();
            string strRemark4 = inTemPd["cRemark4"].ToString().TrimEnd();
            string strRemark5 = inTemPd["cRemark5"].ToString().TrimEnd();
            string strRemark6 = inTemPd["cRemark6"].ToString().TrimEnd();
            string strRemark7 = inTemPd["cRemark7"].ToString().TrimEnd();
            string strRemark8 = inTemPd["cRemark8"].ToString().TrimEnd();
            string strRemark9 = inTemPd["cRemark9"].ToString().TrimEnd();
            string strRemark10 = inTemPd["cRemark10"].ToString().TrimEnd();

            //dtrRefProd["fcStep"] = inTemPd["cStep"].ToString().TrimEnd();
            dtrRefProd["fcCorp"] = App.ActiveCorp.RowID;
            dtrRefProd["fcBranch"] = this.mstrBranchID;
            dtrRefProd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
            dtrRefProd["fcRefType"] = dtrGLRef["fcRefType"].ToString();
            dtrRefProd["fcRfType"] = dtrGLRef["fcRfType"].ToString();
            dtrRefProd["fcStat"] = dtrGLRef["fcStat"].ToString();
            dtrRefProd["fdDate"] = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;
            dtrRefProd["fcCoor"] = dtrGLRef["fcCoor"].ToString();
            dtrRefProd["fcRefPdTyp"] = inTemPd["cRefPdType"].ToString().TrimEnd();
            dtrRefProd["fcProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrRefProd["fcRootSeq"] = inTemPd["cRootSeq"].ToString().TrimEnd();
            dtrRefProd["fcShowComp"] = "";
            dtrRefProd["fcPformula"] = inTemPd["cPFormula"].ToString();
            dtrRefProd["fcFormulas"] = inTemPd["cFormula"].ToString();
            dtrRefProd["fcProd"] = inTemPd["cProd"].ToString();
            dtrRefProd["fmRemark"] = strRemark1;
            dtrRefProd["fmRemark2"] = strRemark2;
            dtrRefProd["fmRemark3"] = strRemark3;
            dtrRefProd["fmRemark4"] = strRemark4;
            dtrRefProd["fmRemark5"] = strRemark5;
            dtrRefProd["fmRemark6"] = strRemark6;
            dtrRefProd["fmRemark7"] = strRemark7;
            dtrRefProd["fmRemark8"] = strRemark8;
            dtrRefProd["fmRemark9"] = strRemark9;
            dtrRefProd["fmRemark10"] = strRemark10;
            dtrRefProd["fcUm"] = inTemPd["cUOM"].ToString();
            dtrRefProd["fcLot"] = inTemPd["cLot"].ToString().TrimEnd();
            dtrRefProd["fcWhouse"] = inTemPd["cWHouse"].ToString();
            dtrRefProd["fcSect"] = (inTemPd["cDept"].ToString() != string.Empty ? inTemPd["cDept"].ToString() : dtrGLRef["fcSect"].ToString());
            dtrRefProd["fcDept"] = (inTemPd["cDivision"].ToString() != string.Empty ? inTemPd["cDivision"].ToString() : dtrGLRef["fcDept"].ToString());
            dtrRefProd["fcJob"] = (inTemPd["cJob"].ToString() != string.Empty ? inTemPd["cJob"].ToString() : dtrGLRef["fcJob"].ToString());
            dtrRefProd["fcProj"] = (inTemPd["cProject"].ToString() != string.Empty ? inTemPd["cProject"].ToString() : dtrGLRef["fcProj"].ToString());
            dtrRefProd["fcIoType"] = this.mstrIOType;
            dtrRefProd["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrRefProd["fnDiscAmtK"] = Convert.ToDecimal(inTemPd["nDiscAmt"]);
            dtrRefProd["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]) * Convert.ToDecimal(this.txtXRate.Value);
            dtrRefProd["fnDiscAmt"] = Convert.ToDecimal(inTemPd["nDiscAmt"]) * Convert.ToDecimal(this.txtXRate.Value);
            dtrRefProd["fnXRate"] = Convert.ToDecimal(this.txtXRate.Value);
            dtrRefProd["fcDiscStr"] = inTemPd["cDiscStr"].ToString().TrimEnd();
            dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                if (!bllIsNewRec)
                {
                    if (((BusinessEnum.gc_RFTYPE_INV_SELL + BusinessEnum.gc_RFTYPE_CR_BUY).IndexOf(dtrGLRef["fcRfType"].ToString()) > -1) == false)
                    {
                        llSucc = this.pmUpdateStock(true, tninQty, tninUmQty, tninStQty, tninStUmQty, bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd);
                    }
                    else
                    {
                        llSucc = this.pmUpdateStock(false, tninQty, tninUmQty, tninStQty, tninStUmQty, bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd);
                    }

                    llSucc = this.pmRetStock(dtrRefProd, inTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                }

                //8/11/50 แก้เพราะถ้าเป็นตอน F2 แล้วมา Add Item
                //ทำให้ refProd.fnQty เป็น Null แล้ว pmUpdateStock เกิด Error ขึ้น
                //				if (((BusinessEnum.gc_RFTYPE_INV_SELL + BusinessEnum.gc_RFTYPE_CR_BUY).IndexOf(dtrGLRef["fcRfType"].ToString()) > -1) == false)
                //				{
                //					llSucc = this.pmUpdateStock( true , tninQty , tninUmQty , tninStQty , tninStUmQty , bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd );
                //				}
                //				else
                //				{
                //					llSucc = this.pmUpdateStock( false , tninQty , tninUmQty , tninStQty , tninStUmQty , bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd );
                //				}
                //
                //				if (!bllIsNewRec)
                //					llSucc = this.pmRetStock(dtrRefProd, inTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);

            }

            dtrRefProd["fcLUpdApp"] = App.AppID;
            dtrRefProd["fcDataser"] = "";
            dtrRefProd["fcEAfterR"] = 'E';

            dtrRefProd["fnQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrRefProd["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrRefProd["fcUmStd"] = inTemPd["cUOMStd"].ToString();

            dtrRefProd["fnStQty"] = Convert.ToDecimal(inTemPd["nStQty"]);
            dtrRefProd["fcStUm"] = inTemPd["cUOMStd"].ToString();
            dtrRefProd["fnStUmQty"] = Convert.ToDecimal(inTemPd["nStUOMQty"]);
            dtrRefProd["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

            decimal decVatInAmt = (dtrGLRef["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrGLRef["fnVatAmt"]) : 0);
            decimal decItemAmt = 0;
            decimal decItemVAT = 0;
            decimal decDiscAmt2 = (Convert.IsDBNull(dtrGLRef["fnDiscAmt2"]) ? 0 : Convert.ToDecimal(dtrGLRef["fnDiscAmt2"]));
            if ((Convert.ToDecimal(dtrGLRef["fnAmt"]) + decVatInAmt + Convert.ToDecimal(dtrGLRef["fnDiscAmt1"]) + decDiscAmt2) == 0)
            {
                decItemAmt = (Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"])
                    - Convert.ToDecimal(dtrRefProd["fnDiscAmt"]));
            }
            else
            {
                decItemAmt = (Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]) - Convert.ToDecimal(dtrRefProd["fnDiscAmt"]))
                    * ((Convert.ToDecimal(dtrGLRef["fnAmt"]) + decVatInAmt) / (Convert.ToDecimal(dtrGLRef["fnAmt"]) + decVatInAmt + Convert.ToDecimal(dtrGLRef["fnDiscAmt1"]) + decDiscAmt2));
            }
            dtrRefProd["fcVatIsOut"] = dtrGLRef["fcVatIsOut"].ToString();
            dtrRefProd["fcVatType"] = dtrGLRef["fcVatType"].ToString();
            dtrRefProd["fnVatRate"] = Convert.ToDecimal(dtrGLRef["fnVatRate"]);
            decItemVAT = decItemAmt * Convert.ToDecimal(dtrGLRef["fnVatRate"]) / (100 + (dtrGLRef["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrGLRef["fnVatRate"]) : 0));

            dtrRefProd["fnVatAmt"] = Math.Round(decItemVAT, 4, MidpointRounding.AwayFromZero);

            //8/11/50 แก้เพราะถ้าเป็นตอน F2 แล้วมา Add Item จะได้ Update Stock
            if (this.mFormEditMode == UIHelper.AppFormState.Edit && bllIsNewRec)
            {
                if (((BusinessEnum.gc_RFTYPE_INV_SELL + BusinessEnum.gc_RFTYPE_CR_BUY).IndexOf(dtrGLRef["fcRfType"].ToString()) > -1) == false)
                {
                    llSucc = this.pmUpdateStock(true, tninQty, tninUmQty, tninStQty, tninStUmQty, bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd);
                }
                else
                {
                    llSucc = this.pmUpdateStock(false, tninQty, tninUmQty, tninStQty, tninStUmQty, bllIsNewRec, ref dtrGLRef, ref dtrRefProd, inTemPd);
                }

            }

            return true;
        }

        private bool pmUpdateStockForInsert()
        {
            string strErrorMsg = "";
            string strSQLUpdateStr = "";
            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
            DataRow dtrRefProd = null;
            bool llSucc = true;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {

                dtrRefProd = null;
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;
                    llSucc = this.pmUpdateStock(false, Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), true, ref dtrGLRef, ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }
            }
            return llSucc;
        }

        private Decimal pmToDecimal(object inConvert)
        {
            Decimal decRetVal = 0;
            //decRetVal = (Convert.IsDBNull(inConvert) ? 0 : Convert.ToDecimal(inConvert));
            if (Convert.IsDBNull(inConvert))
            {
                decRetVal = 0;
            }
            else
            {
                decRetVal = Convert.ToDecimal(inConvert);
            }
            return decRetVal;
        }

        private bool pmUpdateStock(bool tlinNotUpdCostAmt, decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, bool tlinNewItem, ref DataRow ioGLRef, ref DataRow ioRefProd, DataRow inTemPd)
        {
            string strErrorMsg = "";
            decimal lnQty = tninQty;
            decimal lnUmQty = tninUmQty;
            decimal lnStQty2 = tninStQty;
            decimal lnStUmQty2 = tninStUmQty;

            DataRow dtrGLRef = ioGLRef;
            DataRow dtrRefProd = ioRefProd;
            DataRow dtrTemPd = inTemPd;

            dtrGLRef["fnDiscAmt2"] = (Convert.IsDBNull(dtrGLRef["fnDiscAmt2"]) ? 0 : Convert.ToDecimal(dtrGLRef["fnDiscAmt2"]));
            dtrRefProd["fnCostAmt"] = (Convert.IsDBNull(dtrRefProd["fnCostAmt"]) ? 0 : Convert.ToDecimal(dtrRefProd["fnCostAmt"]));

            decimal lnVatInAmt = (dtrGLRef["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrGLRef["fnVatAmt"]) : 0);
            decimal lnItemAmt = 0;
            if (Convert.ToDecimal(dtrGLRef["fnAmt"]) + lnVatInAmt + Convert.ToDecimal(dtrGLRef["fnDiscAmt1"]) + Convert.ToDecimal(dtrGLRef["fnDiscAmt2"]) == 0)
            {
                lnItemAmt = (Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]) - Convert.ToDecimal(dtrRefProd["fnDiscAmt"]));
            }
            else
            {
                //lnItemAmt = Math.Round(( Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]) - Convert.ToDecimal(dtrRefProd["fnDiscAmt"]) ), App.ActiveCorp.RoundAmtAt) 
                //	* ((Convert.ToDecimal(dtrGLRef["fnAmt"]) + lnVatInAmt) / (Convert.ToDecimal(dtrGLRef["fnAmt"]) + lnVatInAmt + Convert.ToDecimal(dtrGLRef["fnDiscAmt1"]) + Convert.ToDecimal(dtrGLRef["fnDiscAmt2"])));


                lnItemAmt = Math.Round((this.pmToDecimal(dtrRefProd["fnQty"]) * this.pmToDecimal(dtrRefProd["fnPriceKe"]) * this.pmToDecimal(dtrRefProd["fnXRate"]) - this.pmToDecimal(dtrRefProd["fnDiscAmt"])), App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero)
                    * ((this.pmToDecimal(dtrGLRef["fnAmt"]) + lnVatInAmt) / (this.pmToDecimal(dtrGLRef["fnAmt"]) + lnVatInAmt + this.pmToDecimal(dtrGLRef["fnDiscAmt1"]) + this.pmToDecimal(dtrGLRef["fnDiscAmt2"])));


                lnItemAmt = Math.Round(lnItemAmt, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
            }

            bool llForceCalCost = false;
            decimal lnCostAmt = 0;
            if (Convert.ToDecimal(dtrRefProd["fnQty"]) == 0)
            {
                llForceCalCost = false;
                lnCostAmt = 0;
            }
            else if (this.mstrSaleOrBuy == "S"
                && !this.mbllIsCrNote
                && Convert.ToDecimal(dtrRefProd["fnQty"]) > 0)
            {
                llForceCalCost = false;
                lnCostAmt = 0;
            }
            else if (this.mstrSaleOrBuy == "S"
                && this.mbllIsCrNote)
            {
                llForceCalCost = true;
                lnCostAmt = 0;
                if (Convert.ToDecimal(dtrTemPd["nCostAmt"]) > 0)
                {
                    lnCostAmt = Convert.ToDecimal(dtrTemPd["nCostAmt"]);
                }
                else
                {
                    if (Convert.ToDecimal(dtrRefProd["fnQty"]) != 0 && Convert.ToDecimal(dtrRefProd["fnUmQty"]) != 0)
                    {
                        decimal lnIVatInAmt = (dtrGLRef["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrRefProd["fnVatAmt"]) : 0);
                        lnCostAmt = Math.Round(lnItemAmt - lnIVatInAmt, App.ActiveCorp.RoundPriceAt, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        lnCostAmt = Math.Round(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]), App.ActiveCorp.RoundPriceAt, MidpointRounding.AwayFromZero);
                    }
                }
            }
            else
            {
                llForceCalCost = true; //ลดหนี้ซื้อก็คิดต้นทุนตามที่ระบุที่ราคา RefProd.fnPrice
                if (Convert.ToDecimal(dtrRefProd["fnQty"]) != 0 && Convert.ToDecimal(dtrRefProd["fnUmQty"]) != 0)
                {
                    decimal lnIVatInAmt = (dtrGLRef["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrRefProd["fnVatAmt"]) : 0);
                    lnCostAmt = Math.Round(lnItemAmt - lnIVatInAmt, App.ActiveCorp.RoundPriceAt, MidpointRounding.AwayFromZero);
                }
                else
                {
                    lnCostAmt = Math.Round(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]), App.ActiveCorp.RoundPriceAt, MidpointRounding.AwayFromZero);
                }
            }

            decimal lnUpdCostAmt = lnCostAmt - Convert.ToDecimal(dtrRefProd["fnCostAmt"]);	// กรณีแก้ไข เพิ่ม/ลด จำนวน จะ Update เฉพาะจำนวนที่ เพิ่ม/ลด เท่านั้น เหมือน Qty
            decimal lnAvgCost = 0;
            decimal lnStQty = 0;
            string lcCtrlStock = dtrTemPd["cCtrlStoc"].ToString();
            string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	//ระดับหน่วยคุมสต็อค

            decimal decOutStockQty = 0;
            decimal decOutWHLocaQty = 0;

            this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
            this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
            bool llSucc = this.mStockAgent.UpdateStock
                (
            #region "UpdateStock Parameter"
                  this.mintUpdStockSign
                , this.mstrBranchID
                , dtrRefProd["fcProdType"].ToString()
                , dtrRefProd["fcProd"].ToString()
                , dtrRefProd["fcWhouse"].ToString()
                , dtrTemPd["cWHLoca"].ToString()
                , dtrRefProd["fcLot"].ToString()
                , lcCtrlStock
                , lnQty
                , lnUmQty
                , dtrTemPd["cUOM"].ToString()
                , dtrTemPd["cQnUOM"].ToString()
                , lnUpdCostAmt
                , ref lnAvgCost
                , lnStQty2 * lnStUmQty2
                , ref decOutStockQty
                , ref decOutWHLocaQty
                , false
                , false
                , ""
                , llForceCalCost
                , 0
                , false
                , false
                , ""
                , ref strErrorMsg
            #endregion
);

            if (tlinNotUpdCostAmt == false)
            {
                decimal lnReplCost = 0;
                if (this.mstrSaleOrBuy == "S" && Convert.ToDecimal(dtrRefProd["fnQty"]) > 0)
                {
                    if (llForceCalCost)
                        lnReplCost = (lnCostAmt > 0 ? lnCostAmt : lnAvgCost * Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                    else
                        lnReplCost = lnAvgCost * Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]);
                }
                else
                {
                    lnReplCost = lnCostAmt;
                }
                dtrRefProd["fnCostAmt"] = Math.Round(lnReplCost, App.ActiveCorp.RoundPriceAt, MidpointRounding.AwayFromZero);
            }

            return llSucc;
        }

        private bool pmRetStock(DataRow inRefProd, string inWHLoca, bool inForEdit, bool inIsAlert, string inCtrlStock, ref string ioErrorMsg)
        {
            string strErrorMsg = "";
            bool llSucc = false;
            if (inRefProd["fcProd"].ToString() != "")
            {
                decimal lnCostAmt = Convert.ToDecimal(inRefProd["fnCostAmt"]);
                string lcCtrlStock = "";
                if (inCtrlStock != string.Empty)
                {
                    lcCtrlStock = inCtrlStock;
                }
                else
                {
                    lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, inRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);
                }

                decimal decAvgCost = 0;
                decimal decOutStockQty = 0;
                decimal decOutWHLocaQty = 0;
                string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	// ระดับหน่วยคุมสต็อค

                this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                llSucc = this.mStockAgent.UpdateStock
                    (
                #region "UpdateStock Parameter"
-1 * this.mintUpdStockSign
                    , this.mstrBranchID
                    , inRefProd["fcProdType"].ToString()
                    , inRefProd["fcProd"].ToString()
                    , inRefProd["fcWhouse"].ToString()
                    , inWHLoca
                    , inRefProd["fcLot"].ToString()
                    , lcCtrlStock
                    , Convert.ToDecimal(inRefProd["fnQty"])
                    , Convert.ToDecimal(inRefProd["fnUmQty"])
                    , inRefProd["fcUm"].ToString()
                    , ""
                    , lnCostAmt
                    , ref decAvgCost
                    , Convert.ToDecimal(inRefProd["fnStQty"]) * Convert.ToDecimal(inRefProd["fnStUmQty"])
                    , ref decOutStockQty
                    , ref decOutWHLocaQty
                    , false
                    , inIsAlert
                    , "ยอดในสต๊อคมีไม่พอสำหรับการคืนค่าเดิม จำเป็นต้องให้ยอดติดลบ"
                    , true
                    , 0
                    , false
                    , false
                    , ""
                    , ref strErrorMsg
                #endregion
                    );

                ioErrorMsg = strErrorMsg;
            }

            return llSucc;
        }

        private bool pmChgItemStat(string inGLRef, ref string ioErrorMsg)
        {
            string strErrorMsg = "";
            decimal decMinQty = 0;
            decimal decStMinQty = 0;
            bool bllQtySucc = false;
            bool bllStQtySucc = false;
            bool llSucc = false;

            decimal lnUpdSign = this.mintUpdStockSign * -1;
            string strSQLStrRefProd = "select * from RefProd where RefProd.fcGLRef = ? order by RefProd.fcSeq ";
            this.mSaveDBAgent.SetPara(new object[] { inGLRef });
            if (this.mSaveDBAgent.SQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", strSQLStrRefProd, ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    if (dtrRefProd["fcProd"].ToString() == string.Empty)
                    {
                        continue;
                    }

                    string strWHLoca = "";

                    string lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);
                    string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	// ระดับหน่วยคุมสต็อค

                    string strUM = "";
                    string strQnUM = "";
                    string strStUM = "";
                    string strStQnUM = "";

                    this.mSaveDBAgent.SetPara(new object[] { dtrRefProd["fcUM"].ToString() });
                    if (this.mSaveDBAgent.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                    {
                        strUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                        strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString();
                    }
                    this.mSaveDBAgent.SetPara(new object[] { dtrRefProd["fcStUM"].ToString() });
                    if (this.mSaveDBAgent.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                    {
                        strStUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                        strStQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString();
                    }

                    decimal lnCostAmt = Convert.ToDecimal(dtrRefProd["fnCostAmt"]);
                    decimal lnAvgCost = 0;
                    decimal decOutStockQty = 0;
                    decimal decOutWHLocaQty = 0;

                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    llSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
lnUpdSign
                        , this.mstrBranchID
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , Convert.ToDecimal(dtrRefProd["fnQty"])
                        , Convert.ToDecimal(dtrRefProd["fnUmQty"])
                        , dtrRefProd["fcUM"].ToString()
                        , ""
                        , lnCostAmt
                        , ref lnAvgCost
                        , Convert.ToDecimal(dtrRefProd["fnStQty"]) * Convert.ToDecimal(dtrRefProd["fnStUmQty"])
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , true
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
);

                }
            }
            return llSucc;
        }

        private bool pmChkStockBeforeCancel(string inGLRef, ref string ioErrorMsg)
        {

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            string strErrorMsg = "";
            decimal decMinQty = 0;
            decimal decStMinQty = 0;
            bool bllQtySucc = false;
            bool bllStQtySucc = false;
            bool llSucc = true;

            decimal lnUpdSign = this.mintUpdStockSign * -1;
            string strSQLStrRefProd = "select * from RefProd where RefProd.fcGLRef = ? order by RefProd.fcSeq ";
            pobjSQLUtil.SetPara(new object[] { inGLRef });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", strSQLStrRefProd, ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    if (dtrRefProd["fcProd"].ToString() == string.Empty)
                    {
                        continue;
                    }

                    string strWHLoca = "";

                    string lcCtrlStock = BizRule.GetProdCtrlStock(pobjSQLUtil, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);
                    string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	// ระดับหน่วยคุมสต็อค

                    if (dtrRefProd["fcIoType"].ToString() == "I"
                        && Convert.ToDecimal(dtrRefProd["fnQty"]) > 0
                        && dtrRefProd["fcProd"].ToString() != string.Empty
                        && lcCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE
                        && pobjSQLUtil.SetPara(new object[] { dtrRefProd["fcWHouse"].ToString() })
                        && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select * from WHouse where fcSkid = ?", ref strErrorMsg)
                        && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcType"].ToString()) > -1) == false)
                    {

                        string strUM = "";
                        string strQnUM = "";
                        pobjSQLUtil.SetPara(new object[] { dtrRefProd["fcUM"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                        {
                            strUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString();
                        }

                        if (Convert.ToDecimal(dtrRefProd["fnStQty"]) == 0)
                        {

                            llSucc = this.mStockAgent.TestUpdateStock
                                (
                            #region "Stock Parameter"
lnUpdSign
                                , this.mstrBranchID
                                , dtrRefProd["fcProdType"].ToString()
                                , dtrRefProd["fcProd"].ToString()
                                , dtrRefProd["fcWHouse"].ToString()
                                , strWHLoca
                                , dtrRefProd["fcLot"].ToString()
                                , lcCtrlStock
                                , Convert.ToDecimal(dtrRefProd["fnQty"])
                                , Convert.ToDecimal(dtrRefProd["fnUMQty"])
                                , 0
                                , 0
                                , strQnUM
                                , ref decMinQty
                                , ref bllQtySucc
                                , lcStCtrlStock
                                , 0
                                , 1
                                , 0
                                , 1
                                , ""
                                , ref decStMinQty
                                , ref bllStQtySucc
                                , false
                                , false
                                , false
                                , ref ioErrorMsg
                            #endregion
);

                        }
                        else
                        {
                            string strStUM = "";
                            string strStQnUM = "";
                            pobjSQLUtil.SetPara(new object[] { dtrRefProd["fcStUM"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strStUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                                strStQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString();
                            }

                            llSucc = this.mStockAgent.TestUpdateStock
                                (
                            #region "Stock Parameter"
lnUpdSign
                                , this.mstrBranchID
                                , dtrRefProd["fcProdType"].ToString()
                                , dtrRefProd["fcProd"].ToString()
                                , dtrRefProd["fcWHouse"].ToString()
                                , strWHLoca
                                , dtrRefProd["fcLot"].ToString()
                                , lcCtrlStock
                                , Convert.ToDecimal(dtrRefProd["fnQty"])
                                , Convert.ToDecimal(dtrRefProd["fnUMQty"])
                                , 0
                                , 0
                                , strStQnUM
                                , ref decMinQty
                                , ref bllQtySucc
                                , lcStCtrlStock
                                , Convert.ToDecimal(dtrRefProd["fnStQty"])
                                , Convert.ToDecimal(dtrRefProd["fnStUmQty"])
                                , 0
                                , 1
                                , ""
                                , ref decStMinQty
                                , ref bllStQtySucc
                                , false
                                , false
                                , false
                                , ref ioErrorMsg
                            #endregion
);

                        }
                    }

                }
            }

            return llSucc;
        }

        private bool pmRunCode()
        {
            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            long intRunCode = 1;
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            pAPara = new object[4] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrBookID };
            strSQLStr = "select fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["fcCode"].ToString().Trim();
                try
                {
                    intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
            }
            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : this.txtCode.Properties.MaxLength);
            this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            if (this.txtRefNo.Text.TrimEnd() == string.Empty)
            {
                string strRefNo = this.mstrBookPrefix + this.txtCode.Text.TrimEnd();
                this.txtRefNo.Text = strRefNo;
                //this.txtRefNo.Text = this.mstrRefType + this.mstrQcBook + "/" + this.txtCode.Text.TrimEnd();
            }
            return true;
        }

        private void pgfMainEdit_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            this.pmSetToolbarState(pgfMainEdit.SelectedTabPageIndex);

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                if (pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
                {
                    this.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                }
                else
                {
                    this.Location = new Point(Convert.ToInt16((AppUtil.CommonHelper.SysMetric(1) - this.Width) / 2), Convert.ToInt16((AppUtil.CommonHelper.SysMetric(2) - this.Height) / 2));
                }
            }

        }

        private void pmEnterForm()
        {
            if (this.mFormActiveMode == FormActiveMode.PopUp
                && this.pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
            {
                //this.mbllPopUpResult = true;
                this.Hide();
            }
        }

        private void frmInvoice_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.pmEnterForm();
                    break;
                //case Keys.PageUp:
                //	if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                //		this.pgfMainEdit.SelectedTabPageIndex = ( this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_EDIT1 : this.pgfMainEdit.SelectedTabPageIndex + 1);
                //	break;
                //case Keys.PageDown:
                //	if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                //		this.pgfMainEdit.SelectedTabPageIndex = ( this.pgfMainEdit.SelectedTabPageIndex - 1 <= xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);
                //	break;
                case Keys.Escape:
                    if (this.pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
                    {
                        if (MessageBox.Show("ยกเลิกการแก้ไข ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.pmGotoBrowPage();
                        }
                    }
                    else
                    {
                        if (this.mFormActiveMode == FormActiveMode.Edit)
                        {
                            //this.pmFilterForm();
                            //this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
                            //if (this.DialogResult == DialogResult.Cancel)
                            this.Close();
                        }
                        else
                            this.Hide();
                    }
                    break;
            }
        }

        private void pmPopUpButtonClick(string inTextbox, string inPara1)
        {
            //string strPopUpResult = "";
            switch (inTextbox)
            {
                case "TXTQCCOOR":
                case "TXTQSCOOR":
                case "TXTQCDELICOOR":
                    this.pmInitPopUpDialog("COOR");
                    this.pofrmGetCoor.ValidateField("", (inTextbox == "TXTQCCOOR" || inTextbox == "TXTQCDELICOOR" ? "FCCODE" : "FCSNAME"), true);
                    if (this.pofrmGetCoor.PopUpResult)
                    {
                        if (inTextbox == "TXTQCCOOR" || inTextbox == "TXTQSCOOR")
                            this.pmRetrievePopUpVal("COOR");
                        else
                            this.pmRetrievePopUpVal("DELICOOR");
                    }
                    break;
                case "TXTREFTO":
                    this.pmInitPopUpDialog("REFTO");
                    break;
                case "TXTQCDEPT":
                    this.pmInitPopUpDialog("DEPT");
                    this.pofrmGetDept.ValidateField("", "FCCODE", true);
                    if (this.pofrmGetDept.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("DEPT");
                    }
                    break;
                case "TXTVATTYPE":
                    this.pmInitPopUpDialog("VATTYPE");
                    this.pofrmGetVatType.ValidateField("", "FCCODE", true);
                    if (this.pofrmGetVatType.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("VATTYPE");
                    }
                    break;
                case "TXTQCCURRENCY":
                    this.pmInitPopUpDialog("CURRENCY");
                    this.pofrmGetCurrency.ValidateField("", "FCCODE", true);
                    if (this.pofrmGetCurrency.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("CURRENCY");
                    }
                    break;
            }
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

        private string pmRetrievePopUpVal(string inPopupForm)
        {

            string strRetValue = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = null;
            DataRow dtrTemPd = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "COOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrPopup = this.pofrmGetCoor.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.pmRetrieveCoorVal(dtrPopup);
                        }
                        else
                        {
                            this.txtQcCoor.Tag = "";
                            this.txtQcCoor.Text = "";
                            this.txtQsCoor.Text = "";

                            this.mstrSEmpl = "";
                            this.mstrTradeTrm = "";

                            this.mstrLastQcCoor = this.txtQcCoor.Text;
                            this.mstrLastQsCoor = this.txtQsCoor.Text;
                        }
                    }
                    break;
                case "PDTYPE":
                    if (this.pofrmGetPdType != null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.intActiveRow];
                        DataRow dtrPdType = this.pofrmGetPdType.RetrieveValue();

                        if (dtrPdType == null)
                        {
                        }
                        else
                        {
                            dtrTemPd["cPdType"] = dtrPdType["fcCode"].ToString().TrimEnd();
                            if (dtrTemPd["cLastPdType"].ToString() != dtrPdType["fcCode"].ToString().TrimEnd())
                            {
                                this.pmClr1TemPd();

                                dtrTemPd["cLastPdType"] = dtrPdType["fcCode"].ToString().TrimEnd();
                                dtrTemPd["cPdType"] = dtrPdType["fcCode"].ToString().TrimEnd();
                                strRetValue = dtrPdType["fcCode"].ToString().TrimEnd();

                            }
                        }
                    }
                    break;
                case "CQCPROD":
                case "CQNPROD":
                    if (this.pofrmGetProd != null)
                    {
                        dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                        DataRow dtrProd = this.pofrmGetProd.RetrieveValue();

                        if (dtrProd == null)
                        {
                            this.pmClr1TemPd();
                            //this.grdTemPd.SetValue("cPdType", dtrTemPd["cPdType"].ToString().TrimEnd());
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "cPdType", dtrTemPd["cPdType"].ToString().TrimEnd());
                        }
                        else
                        {
                            //if (dtrTemPd["cLastProd"].ToString() != dtrProd["fcSkid"].ToString())
                            //{
                            dtrTemPd["cLastProd"] = dtrProd["fcSkid"].ToString();
                            dtrTemPd["cLastQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                            dtrTemPd["cLastQnProd"] = dtrProd["fcName"].ToString().TrimEnd();
                            this.pmRetrieveProductVal(ref dtrTemPd, dtrProd);
                            strRetValue = dtrProd[(inPopupForm.TrimEnd().ToUpper() == "CQCPROD" ? "fcCode" : "fcName")].ToString().TrimEnd();
                            //}
                        }
                        if (dtrTemPd["cProd"].ToString().TrimEnd() != dtrTemPd["cXRefToProd"].ToString().TrimEnd())
                        {
                            dtrTemPd["cRefToRowID"] = "";
                            dtrTemPd["cRefToCode"] = "";
                        }
                    }
                    break;

                case "DEPT":
                    if (this.pofrmGetDept != null)
                    {
                        DataRow dtrPopup = this.pofrmGetDept.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtQcDept.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcDept.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcDept.Tag = "";
                            this.txtQcDept.Text = "";
                        }
                    }
                    break;
                case "VATTYPE":
                    if (this.pofrmGetVatType != null)
                    {
                        DataRow dtrVatType = this.pofrmGetVatType.RetrieveValue();
                        if (dtrVatType != null)
                        {
                            decimal decVatRate = Convert.ToDecimal(dtrVatType["fnRate"]);
                            this.mdecVatRate = decVatRate;
                            this.txtVatType.Tag = dtrVatType["fcCode"].ToString().TrimEnd();
                            this.txtVatType.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                            this.txtVatRate.Text = decVatRate.ToString("###.00") + "%";
                            this.pmRecalTotPd();
                        }
                        else
                        {
                            this.txtVatType.Tag = "";
                            this.txtVatType.Text = "";
                        }
                    }
                    break;
                case "CURRENCY":
                    if (this.pofrmGetCurrency != null)
                    {
                        DataRow dtrPopup = this.pofrmGetCurrency.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            decimal decRate = Convert.ToDecimal(dtrPopup["fnRate"]);
                            this.txtQcCurrency.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcCurrency.Text = dtrPopup["fcCode"].ToString();
                            this.txtQnCurrency.Text = dtrPopup["fcName"].ToString();
                            this.txtXRate.Value = (decRate != 0 ? decRate : 1);
                            this.mstrCurrSign = dtrPopup["fcSign"].ToString();
                        }
                        else
                        {
                            this.txtQcCurrency.Tag = "";
                            this.txtQcCurrency.Text = "";
                            this.txtQnCurrency.Text = "";
                            this.txtXRate.Value = 1;
                            this.mstrCurrSign = "";
                        }
                    }
                    break;

            }

            return strRetValue;
        }

        private void pmRetrieveProductVal(ref DataRow inTemPd, DataRow inProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strUOM = "";
            string strStkUOM = "";
            decimal decUOMQty = 1;
            decimal decStkUOMQty = 1;

            switch (this.mstrSaleOrBuy)
            {
                case "S":
                    strUOM = (inProd["fcUm2"].ToString().TrimEnd() != string.Empty ? inProd["fcUm2"].ToString() : inProd["fcUm"].ToString());
                    strStkUOM = (inProd["fcUm2"].ToString().TrimEnd() != string.Empty ? inProd["fcStUm2"].ToString() : inProd["fcUm"].ToString());
                    decUOMQty = Convert.ToDecimal(inProd["fnUmQty2"]);
                    decStkUOMQty = Convert.ToDecimal(inProd["fnStUmQty2"]);
                    break;
                case "P":
                    strUOM = (inProd["fcUm1"].ToString().TrimEnd() != string.Empty ? inProd["fcUm1"].ToString() : inProd["fcUm"].ToString());
                    strStkUOM = (inProd["fcUm1"].ToString().TrimEnd() != string.Empty ? inProd["fcStUm1"].ToString() : inProd["fcUm"].ToString());
                    decUOMQty = Convert.ToDecimal(inProd["fnUmQty1"]);
                    decStkUOMQty = Convert.ToDecimal(inProd["fnStUmQty1"]);
                    break;
            }

            inTemPd["cProd"] = inProd["fcSkid"].ToString();
            inTemPd["cFormula"] = "";
            inTemPd["cRefPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
            inTemPd["cLastRefPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
            inTemPd["cCtrlStoc"] = BizRule.GetProdCtrlStock(inProd["fcCtrlStoc"].ToString(), App.ActiveCorp.SCtrlStock);
            inTemPd["cPdType"] = inProd["fcType"].ToString().TrimEnd();
            inTemPd["cLastPdType"] = inProd["fcType"].ToString().TrimEnd();
            inTemPd["cQcProd"] = inProd["fcCode"].ToString().TrimEnd();
            inTemPd["cQnProd"] = inProd["fcName"].ToString().TrimEnd();
            inTemPd["cLastQcProd"] = inTemPd["cQcProd"].ToString();
            inTemPd["cLastQnProd"] = inTemPd["cQnProd"].ToString();
            inTemPd["cUOM"] = strUOM;
            inTemPd["cUOMStd"] = inProd["fcUm"].ToString();
            inTemPd["nUOMQty"] = decUOMQty;
            inTemPd["cStUOM"] = strStkUOM;
            inTemPd["cStUOMStd"] = inProd["fcStUm"].ToString();
            inTemPd["nStUOMQty"] = decUOMQty;
            //inTemPd["nPrice"] = this.pmGetPrice();
            inTemPd["cWHouse"] = this.mstrDefaWHouse;
            inTemPd["cDiscStr"] = this.mstrCoorDiscStr;

            //decimal decAmt = Convert.ToDecimal(this.grdTemPd.GetValue("nQty")) * Convert.ToDecimal(this.grdTemPd.GetValue("nPrice"));
            //decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(strValue, decAmt, 0, 0);
            //if (decDiscAmt <= decAmt)
            //{
            //    this.grdTemPd.SetValue("nDiscAmt", decDiscAmt);
            //}

            if (this.mstrSaleOrBuy == "S")
            {
                inTemPd["nPrice"] = Convert.ToDecimal(inProd["fnPrice"]);
            }


            //inTemPd["cWHouse"] = this.txtQcWHouse.Tag;

            if (inTemPd["cUOM"].ToString() != string.Empty
                && pobjSQLUtil.SetPara(new object[] { inTemPd["cUOM"].ToString() })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
            {
                inTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            else
            {
                inTemPd["cQnUOM"] = "";
            }

            if (inProd["fcSkid"].ToString() != string.Empty
                && pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inProd["fcSkid"].ToString() })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProdX4", "PRODX4", "select fmMemData, fmMemData2, fmMemData3, fmMemData4, fmMemData5 from ProdX4 where fcCorp = ? and fcProd = ?", ref strErrorMsg))
            {
                DataRow dtrProdX4 = this.dtsDataEnv.Tables["QProdX4"].Rows[0];

                string strRemark = (Convert.IsDBNull(dtrProdX4["fmMemData"]) ? "" : dtrProdX4["fmMemData"].ToString().TrimEnd());
                strRemark += (Convert.IsDBNull(dtrProdX4["fmMemData2"]) ? "" : dtrProdX4["fmMemData2"].ToString().TrimEnd());
                strRemark += (Convert.IsDBNull(dtrProdX4["fmMemData3"]) ? "" : dtrProdX4["fmMemData3"].ToString().TrimEnd());
                strRemark += (Convert.IsDBNull(dtrProdX4["fmMemData4"]) ? "" : dtrProdX4["fmMemData4"].ToString().TrimEnd());
                strRemark += (Convert.IsDBNull(dtrProdX4["fmMemData5"]) ? "" : dtrProdX4["fmMemData5"].ToString().TrimEnd());
                
                inTemPd["cRemark1"] = BizRule.GetMemData(strRemark, "Rem");
                inTemPd["cRemark2"] = BizRule.GetMemData(strRemark, "Rm2");
                inTemPd["cRemark3"] = BizRule.GetMemData(strRemark, "Rm3");
                inTemPd["cRemark4"] = BizRule.GetMemData(strRemark, "Rm4");
                inTemPd["cRemark5"] = BizRule.GetMemData(strRemark, "Rm5");
                inTemPd["cRemark6"] = BizRule.GetMemData(strRemark, "Rm6");
                inTemPd["cRemark7"] = BizRule.GetMemData(strRemark, "Rm7");
                inTemPd["cRemark8"] = BizRule.GetMemData(strRemark, "Rm8");
                inTemPd["cRemark9"] = BizRule.GetMemData(strRemark, "Rm9");
                inTemPd["cRemark10"] = BizRule.GetMemData(strRemark, "RmA");
            
            }

            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "cQcProd", inProd["fcCode"].ToString().TrimEnd());
            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "cQnProd", inProd["fcName"].ToString().TrimEnd());
            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "cQnUOM", inProd["cQnUOM"].ToString().TrimEnd());
            //this.gridView2.UpdateCurrentRow();
        }

        private void pmRetrieveFormulaVal(ref DataRow inTemPd, DataRow inProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            inTemPd["cProd"] = "";
            inTemPd["cFormula"] = inProd["fcSkid"].ToString();
            inTemPd["cRefPdType"] = SysDef.gc_REFPD_TYPE_FORMULA;
            //inTemPd["cCtrlStoc"] = "";
            inTemPd["cCtrlStoc"] = BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT;
            inTemPd["cPdType"] = inProd["fcProdType"].ToString().TrimEnd();
            inTemPd["cLastPdType"] = inProd["fcProdType"].ToString().TrimEnd();
            inTemPd["cQcProd"] = inProd["fcCode"].ToString().TrimEnd();
            inTemPd["cQnProd"] = inProd["fcName"].ToString().TrimEnd();
            inTemPd["cLastQcProd"] = inTemPd["cQcProd"].ToString();
            inTemPd["cLastQnProd"] = inTemPd["cQnProd"].ToString();
            inTemPd["cUOM"] = inProd["fcUm"].ToString();
            inTemPd["cUOMStd"] = inProd["fcUm"].ToString();
            inTemPd["nUOMQty"] = 1;
            inTemPd["cStUOM"] = 1;
            inTemPd["cStUOMStd"] = inProd["fcUm"].ToString();
            inTemPd["nStUOMQty"] = 1;
            //inTemPd["nPrice"] = this.pmGetPrice();
            inTemPd["cWHouse"] = this.mstrDefaWHouse;
            //inTemPd["cWHouse"] = this.txtQcWHouse.Tag;

            if (inTemPd["cUOM"].ToString() != string.Empty
                && pobjSQLUtil.SetPara(new object[] { inTemPd["cUOM"].ToString() })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
            {
                inTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            else
            {
                inTemPd["cQnUOM"] = "";
            }

        }

        private void pmLoadFormulaComponent(string inFormula, string inRootSeq, int inRecNo)
        {
            //int intRecNo = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count;
            int intRecNo = inRecNo;

            string strErrorMsg = "";
            bool bllIsNewRow = false;
            QFormulaCollection mFormula = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            mFormula = BizRule.GetItemofFormula(pobjSQLUtil, inFormula);
            if (mFormula.Count > 0)
            {
                for (int intCnt = 0; intCnt < mFormula.Count; intCnt++)
                {
                    FormulaInfo mInfo = mFormula[intCnt];

                    DataRow dtrTemPd = null;

                    pobjSQLUtil.SetPara(new object[] { mInfo.PdOrFM });
                    if (mInfo.RefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
                    {
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ? ", ref strErrorMsg))
                        {
                            //dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                            dtrTemPd = this.pmNewItem(intRecNo, ref bllIsNewRow);
                            DataRow dtrQProd = this.dtsDataEnv.Tables["QProd"].Rows[0];

                            intRecNo++;

                            dtrTemPd["cRootSeq"] = inRootSeq;
                            dtrTemPd["cPFormula"] = mInfo.ParentFM;
                            dtrTemPd["cQcPFormula"] = mInfo.ParentQcFM;
                            dtrTemPd["nQtyPerMFm"] = mInfo.Qty;

                            //dtrTemPd["nRecNo"] = intRecNo;
                            dtrTemPd["cLastProd"] = dtrQProd["fcSkid"].ToString();
                            dtrTemPd["cLastQcProd"] = dtrQProd["fcCode"].ToString().TrimEnd();
                            dtrTemPd["cLastQnProd"] = dtrQProd["fcName"].ToString().TrimEnd();

                            this.pmRetrieveProductVal(ref dtrTemPd, dtrQProd);
                            if (bllIsNewRow)
                                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                        }
                    }
                    else
                    {
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QFormula", "FORMULAS", "select * from FORMULAS where fcSkid = ? ", ref strErrorMsg))
                        {
                            DataRow dtrQFM = this.dtsDataEnv.Tables["QFormula"].Rows[0];
                            //dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                            dtrTemPd = this.pmNewItem(intRecNo, ref bllIsNewRow);

                            intRecNo++;

                            dtrTemPd["cRootSeq"] = inRootSeq;
                            dtrTemPd["cPFormula"] = mInfo.ParentFM;
                            dtrTemPd["nQtyPerMFm"] = mInfo.Qty;

                            //dtrTemPd["nRecNo"] = intRecNo;
                            dtrTemPd["cLastFormula"] = dtrQFM["fcSkid"].ToString();
                            dtrTemPd["cLastQcProd"] = dtrQFM["fcCode"].ToString().TrimEnd();
                            dtrTemPd["cLastQnProd"] = dtrQFM["fcName"].ToString().TrimEnd();

                            this.pmRetrieveProductVal(ref dtrTemPd, dtrQFM);
                            if (bllIsNewRow)
                                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                        }
                    }

                }
            }
        }

        private void pmRetrieveCoorVal(DataRow inCoor)
        {
            DataRow dtrTemRow = null;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.txtQcCoor.Tag = inCoor["fcSkid"].ToString();
            this.txtQcCoor.Text = inCoor["fcCode"].ToString().TrimEnd();
            this.txtQsCoor.Text = inCoor["fcSName"].ToString().TrimEnd();
            this.mstrSEmpl = inCoor["fcEmpl"].ToString();
            this.mstrDeliCoor = inCoor["fcDeliCoor"].ToString();
            this.mstrVatCoor = inCoor["fcVatCoor"].ToString();

            this.mintCredTerm = Convert.ToInt32(inCoor["fnCredTerm"]);

            this.mstrLastQcCoor = this.txtQcCoor.Text;
            this.mstrLastQsCoor = this.txtQsCoor.Text;

            if (this.mstrDeliCoor.TrimEnd() == string.Empty)
            {
                this.mstrDeliCoor = this.txtQcCoor.Tag.ToString();
            }
            if (this.mstrVatCoor.TrimEnd() == string.Empty)
            {
                this.mstrVatCoor = this.txtQcCoor.Tag.ToString();
            }

            if (DataSetHelper.HasField("fcDiscStr", inCoor))
            {
                this.txtDiscount.Text = (inCoor["fcDiscStr"].ToString().TrimEnd() != string.Empty ? inCoor["fcDiscStr"].ToString().TrimEnd() : (Convert.ToDecimal(inCoor["fnDiscPcn"]) != 0 ? Convert.ToDecimal(inCoor["fnDiscPcn"]).ToString("#####.00") + "%" : ""));
            }
            else
            {
                this.txtDiscount.Text = (Convert.ToDecimal(inCoor["fnDiscPcn"]) != 0 ? Convert.ToDecimal(inCoor["fnDiscPcn"]).ToString("#####.00") + "%" : "");
            }

            this.pmCalDueDate();

            if (this.mstrSaleOrBuy == "S" && this.mstrSEmpl.TrimEnd() != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[] { this.mstrSEmpl });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmpl", "EMPL", "select * from " + MapTable.Table.Employee + " where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdecSaleComm = Convert.ToDecimal(this.dtsDataEnv.Tables["QEmpl"].Rows[0]["fnCommissi"]);
                }

                this.mstrCoorDiscStr = "";
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, "A", this.txtQcCoor.Tag.ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPrice", "PRICE", "select FCDISCSTR from PRICE where FCCORP = ? and FCSYSTEM = ? and FCCOOR = ? ", ref strErrorMsg))
                {
                    this.mstrCoorDiscStr = this.dtsDataEnv.Tables["QPrice"].Rows[0]["fcDiscStr"].ToString().TrimEnd();
                }

            }

            if (this.mstrVatCoor.TrimEnd() != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[] { this.mstrVatCoor });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcName,fcName2 from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemRow = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                    string strName = dtrTemRow["fcName"].ToString().TrimEnd();
                    string strName2 = dtrTemRow["fcName2"].ToString().TrimEnd();

                    this.mstrInvDetail = strName;
                    if (strName2 != string.Empty
                        && MessageBox.Show(this, "ชื่อในรายงานภาษีจะใช้ชื่อภาษาไทย ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        this.mstrInvDetail = strName2;
                    }
                }
            }

            if (DataSetHelper.HasField("fcVatType", inCoor)
                & inCoor["fcVatType"].ToString().TrimEnd() != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[1] { inCoor["fcVatType"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVatType", "VatType", "select fcCode,fnRate from VatType where fcCode = ?", ref strErrorMsg))
                {
                    dtrTemRow = this.dtsDataEnv.Tables["QVatType"].Rows[0];
                    decimal decVatRate = Convert.ToDecimal(dtrTemRow["fnRate"]);
                    this.txtVatType.Tag = inCoor["fcVatType"].ToString();
                    this.mdecVatRate = decVatRate;
                    this.txtVatType.Text = dtrTemRow["fcCode"].ToString().TrimEnd();
                    this.txtVatRate.Text = decVatRate.ToString("###.00") + "%";
                }
                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();

            }


        }

        private void pmCalDueDate()
        {
            DateTime dttDueDate = this.txtDate.DateTime.Date.AddDays(this.mintCredTerm);
            if (dttDueDate.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("วันที่ครบกำหนดตรงกับวันอาทิตย์ จะปรับให้ตรงกับวันจันทร์ ", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dttDueDate = dttDueDate.Date.AddDays(1);
            }
            this.mdttDueDate = dttDueDate.Date;
        }

        private void pmRecalTotPd()
        {
            decimal decSumQty = 0;
            decimal decSumAmtKe = 0;
            //decimal decSumAmtStd = 0;
            //decimal decXRate = 1;
            if (this.mbllRecalTotPd)
            {
                foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
                {
                    //decXRate = (Convert.ToDecimal(dtrTemPd["nXRate"]) == 0 ? 1 : Convert.ToDecimal(dtrTemPd["nXRate"]));

                    //decSumQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                    //decSumAmtKe += Convert.ToDecimal(dtrTemPd["nAmt"]);

                    if (dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
                    {
                        decSumQty += (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                        decSumAmtKe += Math.Round((Convert.IsDBNull(dtrTemPd["nAmt"]) ? 0 : Convert.ToDecimal(dtrTemPd["nAmt"])), App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    }

                    //decSumAmtStd += Convert.ToDecimal(dtrTemPd["nAmt"]) * decXRate;
                }
                this.mbllRecalTotPd = false;
                this.txtTotPdQty.Value = decSumQty;
                this.txtTotPdAmt.Value = decSumAmtKe;

                decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(this.txtDiscount.Text, Convert.ToDecimal(this.txtTotPdAmt.Value), 0, 0);
                if (decDiscAmt != Convert.ToDecimal(this.txtDiscountAmt.Value))
                    this.txtDiscountAmt.Value = decDiscAmt;
            }
            if (this.txtVatIsOut.Text == "Y")
            {
                this.txtAmt.Value = Convert.ToDecimal(this.txtTotPdAmt.Value) - Convert.ToDecimal(this.txtDiscountAmt.Value);
                this.txtVatAmt.Value = Convert.ToDecimal(this.txtAmt.Value) * this.mdecVatRate / 100;
                this.txtGrossAmt.Value = Convert.ToDecimal(this.txtAmt.Value) + Convert.ToDecimal(this.txtVatAmt.Value);
            }
            else
            {
                this.txtGrossAmt.Value = Convert.ToDecimal(this.txtTotPdAmt.Value) - Convert.ToDecimal(this.txtDiscountAmt.Value);
                this.txtVatAmt.Value = Convert.ToDecimal(this.txtGrossAmt.Value) * (this.mdecVatRate / (100 + this.mdecVatRate));
                this.txtAmt.Value = Convert.ToDecimal(this.txtGrossAmt.Value) - Convert.ToDecimal(this.txtVatAmt.Value);
            }
        }

        private void txtQcDept_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "fcCode";
            if (txtPopup.Text == "")
            {
                this.txtQcDept.Tag = "";
                this.txtQcDept.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("DEPT");
                e.Cancel = !this.pofrmGetDept.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetDept.PopUpResult)
                {
                    this.pmRetrievePopUpVal("DEPT");
                }
            }
        }

        //private void txtQcJob_Validating(object sender, CancelEventArgs e)
        //{
        //    DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
        //    string strOrderBy = "fcCode";
        //    if (txtPopup.Text == "")
        //    {
        //        this.txtQcJob.Tag = "";
        //        this.txtQcJob.Text = "";
        //    }
        //    else
        //    {
        //        this.pmInitPopUpDialog("JOB");
        //        e.Cancel = !this.pofrmGetJob.ValidateField(txtPopup.Text, strOrderBy, false);
        //        if (this.pofrmGetJob.PopUpResult)
        //        {
        //            this.pmRetrievePopUpVal("JOB");
        //        }
        //    }
        //}

        private void txtVatType_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";
            this.pmInitPopUpDialog("VATTYPE");
            e.Cancel = !this.pofrmGetVatType.ValidateField(txtPopup.Text, strOrderBy, false);
            if (this.pofrmGetVatType.PopUpResult)
            {
                this.pmRetrievePopUpVal("VATTYPE");
            }
        }

        private void txtQsCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;

            if (txtPopup.Name.ToUpper().TrimEnd() == "TXTQCCOOR")
            {
                if (txtPopup.Text == this.mstrLastQcCoor) return;
            }
            else
            {
                if (txtPopup.Text == this.mstrLastQsCoor) return;
            }

            string strOrderBy = (txtPopup.Name.ToUpper().TrimEnd() == "TXTQCCOOR" ? "FCCODE" : "FCSNAME");
            if (txtPopup.Text == "")
            {
                this.txtQcCoor.Tag = "";
                this.txtQcCoor.Text = "";
                this.txtQsCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal("COOR");
                }
            }
        }

        private void txtQcCurrency_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";
            if (txtPopup.Text == "")
            {
                this.txtQcCurrency.Tag = "";
                this.txtQcCurrency.Text = "";
                this.txtQnCurrency.Text = "";
                this.txtXRate.Value = 1;
            }
            else
            {
                this.pmInitPopUpDialog("CURRENCY");
                e.Cancel = !this.pofrmGetCurrency.ValidateField(txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetCurrency.PopUpResult)
                {
                    this.pmRetrievePopUpVal("CURRENCY");
                }
            }
        }

        private void txtVatIsOut_TextChanged(object sender, System.EventArgs e)
        {
            this.pmRecalTotPd();
            this.txtVatIsOut.SelectAll();
        }

        private void txtDiscount_Validated(object sender, System.EventArgs e)
        {
            decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(this.txtDiscount.Text, Convert.ToDecimal(this.txtTotPdAmt.Value), 0, 0);
            if (decDiscAmt != Convert.ToDecimal(this.txtDiscountAmt.Value))
                this.txtDiscountAmt.Value = decDiscAmt;
            this.pmRecalTotPd();
        }

        private void txtDiscountAmt_Validated(object sender, System.EventArgs e)
        {
            this.pmRecalTotPd();
        }

        private void grdTemPd_Enter(object sender, System.EventArgs e)
        {
            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcProd"];
        }

        //TODO: grdTemPd Valid Method
        //private void grdTemPd_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        //{
        //    this.intActiveRow = this.grdTemPd.Row;

        //    string strValue = "";
        //    this.grdTemPd.SetValue("nDummyFld", 0);	//Force to Update in TemPd
        //    this.grdTemPd.SetValue("nRecNo", this.grdTemPd.Row + 1);	//Force to Update in TemPd
        //    this.grdTemPd.UpdateData();
        //    switch (e.Column.DataMember.ToUpper())
        //    {
        //        case "CQCPROD":
        //        case "CQNPROD":

        //            string strRefPdType = (this.grdTemPd.GetValue("cRefPdType") == null || Convert.IsDBNull(this.grdTemPd.GetValue("cRefPdType").ToString()) ? SysDef.gc_REFPD_TYPE_PRODUCT : this.grdTemPd.GetValue("cRefPdType").ToString());
        //            string strSearchBy = (e.Column.DataMember.ToUpper() == "CQCPROD" ? "FCCODE" : "FCNAME");
        //            string strPdType = (this.grdTemPd.GetValue("cPdType") == null || Convert.IsDBNull(this.grdTemPd.GetValue("cPdType").ToString()) || (this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() == string.Empty) ? this.mstrPdType : "'" + this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() + "'");
        //            strValue = (this.grdTemPd.GetValue(e.Column.DataMember) == null || Convert.IsDBNull(this.grdTemPd.GetValue(e.Column.DataMember).ToString()) ? "" : this.grdTemPd.GetValue(e.Column.DataMember).ToString());
        //            strPdType = (strPdType != string.Empty ? strPdType : this.mstrPdType);
        //            if (strValue.TrimEnd() == string.Empty)
        //            {
        //                this.grdTemPd.SetValue("cLastProd", "");
        //                this.grdTemPd.UpdateData();
        //            }

        //            if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
        //            {
        //                this.pmInitPopUpDialog("PRODUCT");
        //                if (strValue.TrimEnd() == string.Empty)
        //                {
        //                    this.grdTemPd.SetValue("cLastProd", "");
        //                    this.grdTemPd.UpdateData();
        //                }

        //                this.pofrmGetProd.ValidateField(strPdType, "", strSearchBy, true);
        //                if (this.pofrmGetProd.PopUpResult)
        //                {
        //                    this.pmRetrievePopUpVal("PRODUCT");
        //                }
        //            }
        //            else
        //            {

        //                this.pmInitPopUpDialog("FORMULA");
        //                if (strValue.TrimEnd() == string.Empty)
        //                {
        //                    this.grdTemPd.SetValue("cLastProd", "");
        //                    this.grdTemPd.UpdateData();
        //                }

        //                this.pofrmGetFormula.ValidateField(strPdType, "", strSearchBy, true);
        //                if (this.pofrmGetFormula.PopUpResult)
        //                {
        //                    this.pmRetrievePopUpVal("FORMULA");
        //                }
        //            }

        //            break;
        //        case "CREMARK1":
        //            this.pmInitPopUpDialog("REMARK_ITEM");
        //            break;
        //        case "CLOT":
        //            this.pmInitPopUpDialog("LOT_ITEM");
        //            break;
        //        case "NQTY":
        //            this.pmInitPopUpDialog("QTY_ITEM");
        //            break;
        //        case "CDISCSTR1":
        //            this.pmInitPopUpDialog("DISC_ITEM");
        //            break;
        //        case "CQCDEPT":
        //            this.pmInitPopUpDialog("DEPT");
        //            strValue = (this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()) == null || Convert.IsDBNull(this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()).ToString()) ? "" : this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()).ToString());
        //            this.pofrmGetDept.ValidateField(strValue, Department.FIELD_CODE, true);
        //            if (this.pofrmGetDept.PopUpResult)
        //                this.pmRetrievePopUpVal("DEPT_ITEM");

        //            break;
        //        case "CQCJOB":
        //            this.pmInitPopUpDialog("JOB");
        //            strValue = (this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()) == null || Convert.IsDBNull(this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()).ToString()) ? "" : this.grdTemPd.GetValue(e.Column.DataMember.ToUpper()).ToString());
        //            this.pofrmGetJob.ValidateField(strValue, Job.FIELD_CODE, true);
        //            if (this.pofrmGetJob.PopUpResult)
        //                this.pmRetrievePopUpVal("JOB_ITEM");

        //            break;
        //        case "CREFTO":
        //            this.pmInitPopUpDialog("QUERYSTOCK");
        //            break;
        //    }
        //}

        //private void grdTemPd_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    if (e.Alt | e.Control | e.Shift)
        //    {
        //        if (e.Shift && e.KeyCode == Keys.Enter)
        //        {
        //        }
        //        else if (e.Control && e.KeyCode == Keys.Enter)
        //        {
        //            //MessageBox.Show("Ctrl+Enter");
        //        }
        //        else
        //            return;
        //    }

        //    switch (e.KeyCode)
        //    {
        //        case Keys.F3:
        //        case Keys.End:
        //            string strValue = "";
        //            this.grdTemPd.SetValue("nDummyFld", 0);	//Force to Update in TemPd
        //            this.grdTemPd.SetValue("nRecNo", this.grdTemPd.Row + 1);	//Force to Update in TemPd
        //            this.grdTemPd.UpdateData();
        //            this.intActiveRow = this.grdTemPd.Row;
        //            string strDataMember = this.grdTemPd.RootTable.Columns[this.grdTemPd.Col].DataMember.ToUpper();
        //            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row];

        //            switch (strDataMember)
        //            {
        //                case "CQCPROD":
        //                case "CQNPROD":
        //                    if (this.pmChkReferItem("E"))
        //                        return;

        //                    string strRefPdType = (this.grdTemPd.GetValue("cRefPdType") == null || Convert.IsDBNull(this.grdTemPd.GetValue("cRefPdType").ToString()) ? SysDef.gc_REFPD_TYPE_PRODUCT : this.grdTemPd.GetValue("cRefPdType").ToString());
        //                    string strSearchBy = (strDataMember == "CQCPROD" ? "FCCODE" : "FCNAME");
        //                    string strPdType = (this.grdTemPd.GetValue("cPdType") == null || Convert.IsDBNull(this.grdTemPd.GetValue("cPdType").ToString()) || (this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() == string.Empty) ? this.mstrPdType : "'" + this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() + "'");
        //                    strValue = (this.grdTemPd.GetValue(strDataMember) == null || Convert.IsDBNull(this.grdTemPd.GetValue(strDataMember).ToString()) ? "" : this.grdTemPd.GetValue(strDataMember).ToString());
        //                    strPdType = (strPdType != string.Empty ? strPdType : this.mstrPdType);

        //                    if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
        //                    {
        //                        this.pmInitPopUpDialog("PRODUCT");
        //                        this.pofrmGetProd.ValidateField(strPdType, "", strSearchBy, true);
        //                        if (this.pofrmGetProd.PopUpResult)
        //                        {
        //                            this.pmRetrievePopUpVal("PRODUCT");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.pmInitPopUpDialog("FORMULA");
        //                        this.pofrmGetFormula.ValidateField(strPdType, "", strSearchBy, true);
        //                        if (this.pofrmGetFormula.PopUpResult)
        //                        {
        //                            this.pmRetrievePopUpVal("FORMULA");
        //                        }
        //                    }

        //                    break;
        //                case "CREMARK1":
        //                    this.pmInitPopUpDialog("REMARK_ITEM");
        //                    break;
        //                case "CLOT":
        //                    this.pmInitPopUpDialog("LOT_ITEM");
        //                    break;
        //                case "NQTY":
        //                    if (this.pmChkReferItem("E"))
        //                        return;

        //                    this.pmInitPopUpDialog("QTY_ITEM");
        //                    break;
        //                case "CQCDEPT":
        //                    if (this.pmChkReferItem("E"))
        //                        return;

        //                    this.pmInitPopUpDialog("DEPT");
        //                    strValue = (this.grdTemPd.GetValue(strDataMember) == null || Convert.IsDBNull(this.grdTemPd.GetValue(strDataMember).ToString()) ? "" : this.grdTemPd.GetValue(strDataMember).ToString());
        //                    this.pofrmGetDept.ValidateField(strValue, Department.FIELD_CODE, true);
        //                    if (this.pofrmGetDept.PopUpResult)
        //                        this.pmRetrievePopUpVal("DEPT_ITEM");

        //                    break;
        //                case "CQCJOB":
        //                    if (this.pmChkReferItem("E"))
        //                        return;

        //                    this.pmInitPopUpDialog("JOB");
        //                    strValue = (this.grdTemPd.GetValue(strDataMember) == null || Convert.IsDBNull(this.grdTemPd.GetValue(strDataMember).ToString()) ? "" : this.grdTemPd.GetValue(strDataMember).ToString());
        //                    this.pofrmGetJob.ValidateField(strValue, Job.FIELD_CODE, true);
        //                    if (this.pofrmGetJob.PopUpResult)
        //                        this.pmRetrievePopUpVal("JOB_ITEM");

        //                    break;
        //                case "CDISCSTR1":
        //                    this.pmInitPopUpDialog("DISC_ITEM");
        //                    break;
        //            }
        //            break;
        //    }

        //}

        //private void grdTemPd_CurrentCellChanging(object sender, Janus.Windows.GridEX.CurrentCellChangingEventArgs e)
        //{
        //    if (e.Column == null || e.Row == null || this.grdTemPd.Col < 1 || this.grdTemPd.GetValue("nDummyFld") == null)
        //        return;

        //    this.grdTemPd.SetValue("nDummyFld", 0);	//Force to Update in TemPd
        //    this.grdTemPd.SetValue("nRecNo", this.grdTemPd.Row + 1);	//Force to Update in TemPd
        //    this.grdTemPd.UpdateData();
        //    this.intActiveRow = this.grdTemPd.Row;
        //    if (this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count > this.grdTemPd.Row)
        //    {
        //        if (this.grdTemPd.GetValue(this.grdTemPd.RootTable.Columns[this.grdTemPd.Col].DataMember) == null)
        //            return;

        //        string strRefPdType = this.grdTemPd.GetValue("cRefPdType").ToString();

        //        string strProdID = this.grdTemPd.GetValue((strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT ? "cProd" : "cFormula")).ToString();

        //        string strValue = this.grdTemPd.GetValue(this.grdTemPd.RootTable.Columns[this.grdTemPd.Col].DataMember).ToString();
        //        //string strPdType = this.grdTemPd.GetValue("cPdType").ToString().TrimEnd();
        //        string strPdType = (this.grdTemPd.GetValue("cPdType") == null || Convert.IsDBNull(this.grdTemPd.GetValue("cPdType").ToString()) || (this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() == string.Empty) ? this.mstrPdType : "'" + this.grdTemPd.GetValue("cPdType").ToString().TrimEnd() + "'");
        //        strPdType = (strPdType != string.Empty ? strPdType : this.mstrPdType);

        //        //string strValue = Convert.ToString(this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row][this.grdTemPd.RootTable.Columns[this.grdTemPd.Col].DataMember]);
        //        switch (this.grdTemPd.RootTable.Columns[this.grdTemPd.Col].DataMember.ToUpper())
        //        {
        //            case "CREFPDTYPE":
        //                string strLastRefPdType = this.grdTemPd.GetValue("cLastRefPdType").ToString().TrimEnd();
        //                if (strValue != strLastRefPdType)
        //                {
        //                    //this.pmClr1TemPd();
        //                    if (strValue.TrimEnd() != string.Empty
        //                        && "PF".IndexOf(strValue) < 0)
        //                    {
        //                        MessageBox.Show("ต้องระบุ P = สินค้า , F = ชุดสินค้า เท่านั้น", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.grdTemPd.SetValue("cRefPdType", strLastRefPdType);
        //                    }
        //                    else
        //                    {
        //                        this.grdTemPd.SetValue("cLastRefPdType", strValue);
        //                    }
        //                }
        //                break;
        //            case "CPDTYPE":
        //                string strLastPdType = this.grdTemPd.GetValue("cLastPdType").ToString().TrimEnd();
        //                if (strValue != strLastPdType)
        //                {
        //                    //if (this.pmChkReferItem("E"))
        //                    //	return;

        //                    e.Cancel = !this.pmValidPdTypeCol(strValue, "FCCODE");
        //                }

        //                break;
        //            case "CQCPROD":
        //                string strLastQcProd = this.grdTemPd.GetValue("cLastQcProd").ToString().TrimEnd();

        //                if (strValue != strLastQcProd)
        //                {
        //                    if (this.pmChkReferItem("E"))
        //                    {
        //                        this.grdTemPd.SetValue("cQcProd", strLastQcProd);
        //                        return;
        //                    }
        //                    e.Cancel = !this.pmValidProdCol(strPdType, strValue, "FCCODE", false);
        //                }
        //                break;
        //            case "CQNPROD":
        //                string strLastQnProd = this.grdTemPd.GetValue("cLastQnProd").ToString().TrimEnd();
        //                if (strValue != strLastQnProd)
        //                {
        //                    if (this.pmChkReferItem("E"))
        //                    {
        //                        this.grdTemPd.SetValue("cQnProd", strLastQnProd);
        //                        return;
        //                    }

        //                    e.Cancel = !this.pmValidProdCol(strPdType, strValue, "FCNAME", false);
        //                }
        //                break;
        //            case "NQTY":
        //                strValue = (strValue != string.Empty ? strValue : "0");
        //                decimal decQty = Convert.ToDecimal(strValue);
        //                decimal decLastQty = Convert.ToDecimal(this.grdTemPd.GetValue("nLastQty"));

        //                if (decQty != decLastQty)
        //                {

        //                    if (this.pmChkReferItem("E"))
        //                    {
        //                        this.grdTemPd.SetValue("nQty", decLastQty);
        //                        return;
        //                    }

        //                    if (decQty > 0 && strProdID == string.Empty)
        //                    {
        //                        MessageBox.Show("กรุณาระบุสินค้าก่อนระบุจำนวนสินค้า/บริการ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.grdTemPd.SetValue("nQty", 0);
        //                        this.grdTemPd.SetValue("nBackQty", 0);
        //                        this.grdTemPd.SetValue("nBackDOQty", 0);
        //                        this.grdTemPd.SetValue("nDOQty", 0);
        //                        //e.Cancel = true;
        //                    }
        //                    else
        //                    {
        //                        if (this.pmValidQtyCol())
        //                            this.mbllRecalTotPd = true;
        //                        else
        //                            e.Cancel = false;
        //                    }
        //                    this.mbllRecalTotPd = true;
        //                }
        //                break;
        //            case "NPRICE":
        //                strValue = (strValue != string.Empty ? strValue : "0");
        //                decimal decPrice = Convert.ToDecimal(strValue);
        //                decimal decLastPrice = Convert.ToDecimal(this.grdTemPd.GetValue("nLastPrice"));

        //                if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
        //                {
        //                    if (decPrice > 0 && strProdID == string.Empty)
        //                    {
        //                        MessageBox.Show("กรุณาระบุสินค้าก่อนระบุราคาสินค้า/บริการ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.grdTemPd.SetValue("nPrice", 0);
        //                        //e.Cancel = true;
        //                    }
        //                    else
        //                        e.Cancel = !this.pmValidPriceCol();

        //                    this.mbllRecalTotPd = true;
        //                }
        //                else
        //                {
        //                    if (decPrice > 0 && decPrice != decLastPrice)
        //                    {
        //                        MessageBox.Show("แก้ไขราคาของชุดสินค้าไม่ได้", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.grdTemPd.SetValue("nPrice", decLastPrice);
        //                    }
        //                }
        //                break;
        //            case "CDISCSTR":
        //                //string decLastAmt = Convert.ToDecimal(this.grdTemPd.GetValue("nAmt"));
        //                decimal decAmt = Convert.ToDecimal(this.grdTemPd.GetValue("nQty")) * Convert.ToDecimal(this.grdTemPd.GetValue("nPrice"));
        //                decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(strValue, decAmt, 0, 0);
        //                if (decDiscAmt <= decAmt)
        //                {
        //                    this.grdTemPd.SetValue("nDiscAmt", decDiscAmt);
        //                    this.grdTemPd.SetValue("nAmt", decAmt - decDiscAmt);
        //                    this.pmValidDiscountCol();

        //                    this.mbllRecalTotPd = true;
        //                }
        //                else
        //                {
        //                    e.Cancel = true;
        //                    MessageBox.Show("ส่วนลดมากกว่ามูลค่าสินค้า !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    //this.grdTemPd.SetValue("cDiscStr", "");
        //                }
        //                break;
        //            //					case "CQCDEPT":
        //            //						e.Cancel = ! this.pmValidDeptCol(strValue, Department.FIELD_CODE);
        //            //						break;
        //            //					case "CQCJOB":
        //            //						e.Cancel = ! this.pmValidJobCol(strValue, Job.FIELD_CODE);
        //            //						break;
        //            default:
        //                break;
        //        }
        //        if (this.mbllRecalTotPd)
        //            this.pmRecalTotPd();
        //    }
        //}

        private bool pmValidQtyCol()
        {
            return this.pmInitPopUpDialog("QTY_ITEM");
        }

        private decimal pmGetPrice()
        {
            //TODO: return ราคาซื้อ/ขาย
            return 0;
        }

        private bool pmValidPriceCol()
        {
            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.intActiveRow];
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            dtrTemPd["nQty"] = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : dtrTemPd["nQty"]);
            dtrTemPd["nPrice"] = (Convert.IsDBNull(dtrTemPd["nPrice"]) ? 0 : dtrTemPd["nPrice"]);
            if (dtrTemPd["nQty"] == null)
                return true;

            if (dtrTemPd["cPFormula"].ToString().TrimEnd() != string.Empty
                && dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
            {
                this.pmethChgMFMPrice(dtrTemPd["cRootSeq"].ToString(), false);
            }

            return true;
        }

        private bool pmValidDiscountCol()
        {
            //this.pmInitPopUpDialog("DISC_ITEM");

            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.intActiveRow];
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            if (dtrTemPd["cPFormula"].ToString().TrimEnd() != string.Empty
                && dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
            {
                this.pmethChgMFMPrice(dtrTemPd["cRootSeq"].ToString(), false);
            }

            return true;
        }

        private bool pmValidPdTypeCol(string inSeekVal, string inOrderBy)
        {
            bool bllResult = true;
            string strValue = inSeekVal;
            string strOrderBy = inOrderBy;
            if (strValue.TrimEnd() == string.Empty)
            {

                //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle];
                DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                string strRefPdType = (Convert.IsDBNull(dtrTemPd["cRefPdType"]) ? SysDef.gc_REFPD_TYPE_PRODUCT : dtrTemPd["cRefPdType"].ToString());
                if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
                {

                    dtrTemPd["cProd"] = "";
                    dtrTemPd["cFormula"] = "";
                    dtrTemPd["cLastFormula"] = "";
                    dtrTemPd["cPFormula"] = "";
                    dtrTemPd["cQcPFormula"] = "";
                    dtrTemPd["cPdType"] = "";
                    dtrTemPd["cQcProd"] = "";
                    dtrTemPd["cQnProd"] = "";
                    dtrTemPd["cUOM"] = "";
                    dtrTemPd["cQnUOM"] = "";
                    dtrTemPd["nQty"] = 0;
                    dtrTemPd["nBackQty"] = 0;
                    dtrTemPd["nBackDOQty"] = 0;
                    dtrTemPd["nDOQty"] = 0;
                    dtrTemPd["nPrice"] = 0;
                    dtrTemPd["nLastPrice"] = 0;
                    dtrTemPd["cDiscStr"] = "";
                    dtrTemPd["nAmt"] = 0;

                }
                else
                {
                    //TODO: Clear FM Item
                    this.pmClrTemPd_Formula(dtrTemPd["cLastQcProd"].ToString());
                    this.pmClr1TemPd();
                }

                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();

            }
            else
            {
                this.pmInitPopUpDialog("PDTYPE");
                bllResult = this.pofrmGetPdType.ValidateField(this.mstrPdType, strValue, strOrderBy, false);
                if (this.pofrmGetPdType.PopUpResult)
                {
                    this.pmRetrievePopUpVal("PDTYPE");
                }
                //this.mbllIsShowProdDialog = this.pofrmGetPdType.bllIsShowDialog;
            }
            return bllResult;
        }

        private bool pmValidProdCol(string inPdType, string inSeekVal, string inOrderBy, bool inForceBrow)
        {
            bool bllResult = true;

            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            string strValue = inSeekVal;
            string strPdType = inPdType;
            string strOrderBy = inOrderBy;
            string strRefPdType = (Convert.IsDBNull(dtrTemPd["cRefPdType"]) ? SysDef.gc_REFPD_TYPE_PRODUCT : dtrTemPd["cRefPdType"].ToString());

            if (strValue.TrimEnd() == string.Empty)
            {
                if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
                {

                    //dtrTemPd["cPdType"] = "";
                    dtrTemPd["cProd"] = "";
                    dtrTemPd["cFormula"] = "";
                    dtrTemPd["cLastFormula"] = "";
                    dtrTemPd["cPFormula"] = "";
                    dtrTemPd["cQcPFormula"] = "";
                    dtrTemPd["cLastProd"] = "";
                    dtrTemPd["cQcProd"] = "";
                    dtrTemPd["cQnProd"] = "";
                    dtrTemPd["cLastQcProd"] = "";
                    dtrTemPd["cLastQnProd"] = "";
                    dtrTemPd["cUOM"] = "";
                    dtrTemPd["cQnUOM"] = "";
                    dtrTemPd["nQty"] = 0;
                    dtrTemPd["nPrice"] = 0;
                    dtrTemPd["nLastPrice"] = 0;
                    dtrTemPd["cDiscStr"] = "";
                    dtrTemPd["nAmt"] = 0;

                }
                else
                {
                    //TODO: Clear FM Item
                    this.pmClrTemPd_Formula(dtrTemPd["cLastQcProd"].ToString());
                    this.pmClr1TemPd();
                }

                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();

            }
            else
            {

                strPdType = (strPdType != string.Empty ? strPdType : this.mstrPdType);

                if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
                {
                    this.pmInitPopUpDialog("PRODUCT");
                    bllResult = this.pofrmGetProd.ValidateField(strPdType, strValue, strOrderBy, inForceBrow);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("PRODUCT");
                    }
                }
                else
                {
                    //this.pmInitPopUpDialog("FORMULA");
                    //bllResult = this.pofrmGetFormula.ValidateField(strPdType, strValue, strOrderBy, inForceBrow);
                    //if (this.pofrmGetFormula.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal("FORMULA");
                    //}
                }

                //this.mbllIsShowProdDialog = this.pofrmGetProd.bllIsShowDialog;
            }
            return bllResult;
        }

        //		private bool pmValidDeptCol(string inSeekVal, string inOrderBy)
        //		{
        //			bool bllResult = true;
        //			string strValue = inSeekVal;
        //			string strOrderBy = inOrderBy;
        //			if (strValue.TrimEnd() == string.Empty)
        //			{
        //				DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row];
        //				dtrTemPd["cDept"] = "";
        //				dtrTemPd["cQcDept"] = "";
        //				dtrTemPd["cQnDept"] = "";
        //			}
        //			else
        //			{
        //				this.pmInitPopUpDialog("DEPT");
        //				bllResult = this.pofrmGetDept.ValidateField(strValue, strOrderBy, false);
        //				if (this.pofrmGetDept.PopUpResult)
        //					this.pmRetrievePopUpVal("DEPT_ITEM");
        //
        //			}
        //			return bllResult;
        //		}

        //		private bool pmValidJobCol(string inSeekVal, string inOrderBy)
        //		{
        //			bool bllResult = true;
        //			string strValue = inSeekVal;
        //			string strOrderBy = inOrderBy;
        //			if (strValue.TrimEnd() == string.Empty)
        //			{
        //				DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row];
        //				dtrTemPd["cJob"] = "";
        //				dtrTemPd["cQcJob"] = "";
        //				dtrTemPd["cQnJob"] = "";
        //			}
        //			else
        //			{
        //				this.pmInitPopUpDialog("JOB");
        //				bllResult = this.pofrmGetJob.ValidateField(strValue, strOrderBy, false);
        //				if (this.pofrmGetJob.PopUpResult)
        //					this.pmRetrievePopUpVal("JOB_ITEM");
        //
        //			}
        //			return bllResult;
        //		}

        private void pmClrTemPd_Formula(string inPFormula)
        {

            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row];
            //DataRow[] dtaFM = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cQcPFormula = '" + dtrTemPd["cLastQcProd"].ToString() + "'");
            DataRow[] dtaFM = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cQcPFormula = '" + inPFormula + "'");
            if (dtaFM.Length > 0)
            {
                for (int intCnt = 0; intCnt < dtaFM.Length; intCnt++)
                {
                    //if (dtaFM[intCnt]["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_FORMULA)
                    //{
                    //	this.pmClrTemPd_Formula(dtaFM[intCnt]["cLastQcProd"].ToString());
                    //	this.pmClr1TemPd(dtaFM[intCnt]);
                    //}
                    //else
                    //{
                    this.pmClr1TemPd(dtaFM[intCnt]);
                    dtaFM[intCnt]["cRootSeq"] = "";
                    //}
                }
            }

        }

        private void pmClr1TemPd(DataRow inTemPd)
        {
            inTemPd["cRefPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
            inTemPd["cProd"] = "";
            inTemPd["cLastProd"] = "";
            inTemPd["cFormula"] = "";
            inTemPd["cLastFormula"] = "";
            inTemPd["cPFormula"] = "";
            inTemPd["cQcPFormula"] = "";
            inTemPd["cPdType"] = "";
            inTemPd["cQcProd"] = "";
            inTemPd["cLastQcProd"] = "";
            inTemPd["cLastQnProd"] = "";
            inTemPd["cQnProd"] = "";
            inTemPd["cUOM"] = "";
            inTemPd["cQnUOM"] = "";
            inTemPd["nQty"] = 0;
            inTemPd["nBackQty"] = 0;
            inTemPd["nBackDOQty"] = 0;
            inTemPd["nDOQty"] = 0;
            //inTemPd["nPlanQty"] = 0;
            inTemPd["nPrice"] = 0;
            inTemPd["nLastPrice"] = 0;
            inTemPd["cDiscStr"] = "";
            //inTemPd["nAmt"] = 0;

            inTemPd["cRefToRowID"] = "";
            inTemPd["cRefToCode"] = "";

            this.mbllRecalTotPd = true;
            //this.pmRecalTotPd();
        }

        private void pmClr1TemPd()
        {
            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle];
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            dtrTemPd["cProd"] = "";
            dtrTemPd["cLastProd"] = "";
            dtrTemPd["cFormula"] = "";
            dtrTemPd["cLastFormula"] = "";
            dtrTemPd["cPFormula"] = "";
            dtrTemPd["cQcPFormula"] = "";
            dtrTemPd["cPdType"] = "";
            dtrTemPd["cQcProd"] = "";
            dtrTemPd["cLastQcProd"] = "";
            dtrTemPd["cLastQnProd"] = "";
            dtrTemPd["cQnProd"] = "";
            dtrTemPd["cUOM"] = "";
            dtrTemPd["cQnUOM"] = "";
            dtrTemPd["nQty"] = 0;
            dtrTemPd["nBackQty"] = 0;
            dtrTemPd["nBackDOQty"] = 0;
            dtrTemPd["nDOQty"] = 0;
            dtrTemPd["nPrice"] = 0;
            dtrTemPd["cDiscStr"] = "";
            dtrTemPd["nAmt"] = 0;

            dtrTemPd["cRefToRowID"] = "";
            dtrTemPd["cRefToCode"] = "";

            this.mbllRecalTotPd = true;
            //this.pmRecalTotPd();
        }

        private void txtCode_Validating(object sender, CancelEventArgs e)
        {
            if (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
            {
                if (this.pmChkDupCode())
                {
                    MessageBox.Show("เลขที่เอกสารซ้ำ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else
                e.Cancel = false;
        }

        private void txtRefNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldRefNo.TrimEnd() != this.txtRefNo.Text.TrimEnd())
            {
                if (this.pmChkDupRefNo())
                {
                    MessageBox.Show("เลขที่อ้างอิงซ้ำ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else
                e.Cancel = false;
        }

        private void txtDate_Validated(object sender, System.EventArgs e)
        {
            this.pmCalDueDate();
        }

        private void txtDate_Validating(object sender, CancelEventArgs e)
        {
            DateTime dttCorpStartDate = App.ActiveCorp.StartAppDate;
            int intCmp = dttCorpStartDate.CompareTo(this.txtDate.DateTime.Date);
            if (intCmp > 0)
            {
                MessageBox.Show("วันที่เอกสาร" + this.mstrRefTypeName.TrimEnd() + " ไม่ควรเป็นวันที่ก่อนเริ่มใช้ระบบ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private bool pmChkDupCode()
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrBookID, this.txtCode.Text };
            strSQLStr = "select fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                bllResult = true;
            }
            return bllResult;
        }

        private bool pmChkDupRefNo()
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.txtRefNo.Text };
            strSQLStr = "select fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcRefNo = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                bllResult = true;
            }
            return bllResult;
        }

        private int pmGetRowID2(string inAlias, string inSearchField, string inSearchValue)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[inAlias].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[inAlias].Rows[intCnt][inSearchField].ToString().TrimEnd() == inSearchValue)
                    return intCnt;
            }
            return -1;
        }

        private void pmUpdateOrderHStep(string inOrderH)
        {
            string strErrorMsg = "";
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFoxSQLStrOrderI = "select * from OrderI where OrderI.fcOrderH = ?";

            string strHStep = SysDef.gc_REF_STEP_PAY;
            //pobjSQLUtil.SetPara(new object[] { inOrderH });
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefToI", "ORDERI", strFoxSQLStrOrderI, new object[] { inOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefToI"].Rows)
                {
                    decimal decCutQty = 0;
                    decimal decQty = Convert.ToDecimal(dtrOrderI["fnQty"]);
                    //Sum Cut Qty
                    decCutQty = this.pmSumCutQty(dtrOrderI["fcRefType"].ToString(), inOrderH, dtrOrderI["fcSkid"].ToString(), Convert.ToDecimal(dtrOrderI["fnUMQty"]));
                    string strOrderIStep = SysDef.gc_REF_STEP_PAY;
                    decimal decBackQty = 0;
                    if (decCutQty != 0 && decCutQty >= decQty)
                    {
                        if (decQty < 0) { decBackQty = decQty - decCutQty; }
                    }
                    else
                    {
                        if (dtrOrderI["fcRefPdTyp"].ToString() == "P") { strHStep = SysDef.gc_REF_STEP_CUT_STOCK; }
                        strOrderIStep = SysDef.gc_REF_STEP_CUT_STOCK;
                        decBackQty = decQty - decCutQty;
                    }
                    //Update RefTo OrderI step
                    string strSQLUpdate_OrderI = "update ORDERI set fcStep = ?, fnBackQty = ? where fcSkid = ?";
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdate_OrderI, new object[] { strOrderIStep, decBackQty, dtrOrderI["fcSkid"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    //pobjSQLUtil.SetPara(new object[] { strOrderIStep, decBackQty, dtrOrderI["fcSkid"].ToString() });
                    //pobjSQLUtil.SQLExec(strSQLUpdate_OrderI, ref strErrorMsg);
                }
                //Update RefTo OrderH step
                this.mSaveDBAgent.BatchSQLExec("update ORDERH set fcStep = ? where fcSkid = ?", new object[] { strHStep, inOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //pobjSQLUtil.SetPara(new object[] { strHStep, inOrderH});
                //pobjSQLUtil.SQLExec("update ORDERH set fcStep = ? where fcSkid = ?", ref strErrorMsg);

            }
        }

        private decimal pmSumCutQty(string inRefType, string inChildH, string inChildI, decimal inUOMQty)
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            decimal decCutQty = 0;
            string strFoxSQLStrNoteCut = "select * from NoteCut where NoteCut.fcChildtyp = ? and NoteCut.fcChildH = ? and NoteCut.fcChildI = ?";
            pAPara = new object[] { inRefType, inChildH, inChildI };
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut", "NOTECUT", strFoxSQLStrNoteCut, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrNoteCut in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                {
                    //pobjSQLUtil.SetPara(new object[] { dtrNoteCut["fcMasterH"].ToString() });
                    pAPara = new object[] { dtrNoteCut["fcMasterH"].ToString() };
                    if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select fcStat from GLREF where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        if (this.dtsDataEnv.Tables["QRefTo"].Rows[0]["fcStat"].ToString() != "C")
                            decCutQty += Convert.ToDecimal(dtrNoteCut["fnQty"]) * Convert.ToDecimal(dtrNoteCut["fnUmQty"]) / (inUOMQty != 0 ? inUOMQty : 1);
                    }
                }
            }
            return decCutQty;
        }

        private decimal pmSumCrNoteQty(string inRefType, string inChildH, string inChildI, decimal inUOMQty)
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            decimal decCutQty = 0;
            string strRefType = (this.mstrSaleOrBuy == "S" ? "'SC', 'SM'" : "'BC', 'BM'");
            string strFoxSQLStrNoteCut = "select * from NoteCut where NoteCut.fcChildtyp = ? and NoteCut.fcChildH = ? and NoteCut.fcMasterTy in (" + strRefType + ") ";
            pAPara = new object[] { inRefType, inChildH };
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NOTECUT", strFoxSQLStrNoteCut, ref strErrorMsg))
            {
                foreach (DataRow dtrNoteCut in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                {
                    //pobjSQLUtil.SetPara(new object[] { dtrNoteCut["fcMasterH"].ToString() });
                    pAPara = new object[] { dtrNoteCut["fcMasterH"].ToString() };
                    pobjSQLUtil.SetPara(pAPara);
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select fcStat from GLREF where fcSkid = ?", ref strErrorMsg))
                    {
                        if (this.dtsDataEnv.Tables["QRefTo"].Rows[0]["fcStat"].ToString() != "C")
                            decCutQty += Convert.ToDecimal(dtrNoteCut["fnQty"]);
                        //decCutQty +=Convert.ToDecimal(dtrNoteCut["fnQty"]) * Convert.ToDecimal(dtrNoteCut["fnUmQty"]) / (inUOMQty != 0 ? inUOMQty : 1);
                    }
                }
            }
            return decCutQty;
        }

        //private void grdBrowView_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        //{
        //    switch (e.Column.DataMember.ToUpper())
        //    {
        //        case "CREFTO":
        //            this.pmInitPopUpDialog("SHOW_REFTO");
        //            break;
        //    }
        //}

        private void pmChangeWHouseItem()
        {
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                dtrTemPd["cWHouse"] = this.mstrWHouse;
            }
        }

        private string pmSplitToSQLStr(string inStr)
        {
            string strResult = "";
            string[] staSQL = inStr.Split(",".ToCharArray());
            if (staSQL.Length > 0)
            {
                for (int intCnt = 0; intCnt < staSQL.Length; intCnt++)
                {
                    strResult += "'" + staSQL[intCnt] + (intCnt == staSQL.Length - 1 ? "'" : "',");
                }
            }
            return strResult;
        }

        public bool ValidateQty(bool inSetError, ref string ioErrorMsg)
        {
            return this.pmValidQty(inSetError, ref ioErrorMsg);
        }

        public decimal GetStockBalance(string inProd, string inWHouse, string inWHLoca, string inLot)
        {
            return this.mStockAgent.GetStockBalance(this.mstrBranchID, inProd, inWHouse, inWHLoca, inLot);
        }

        public DataRow NewTemPdRow(string inProd)
        {

            DataRow dtrTemPd = null;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.pmRetrieveProductVal(ref dtrTemPd, dtrProd);
                dtrTemPd["nRecNo"] = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count;
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }

            return dtrTemPd;

        }

        public void InsertLoop(ref bool inFirstRow, ref DataRow inTemPd, string inProd, string inWHouse, string inWHLoca, string inLot)
        {
            DataRow dtrTemPd = null;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            if (inFirstRow)
            {
                dtrTemPd = inTemPd;
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { inProd });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
                {
                    if (inTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        this.pmRetrieveProductVal(ref dtrTemPd, dtrProd);
                        dtrTemPd["nRecNo"] = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count;
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }
                    else
                    {
                        dtrTemPd = inTemPd;
                    }
                }
            }

            dtrTemPd["cLot"] = inLot;
            dtrTemPd["cWHouse"] = inWHouse;
            dtrTemPd["cWHLoca"] = inWHLoca;

            inTemPd = dtrTemPd;
            inFirstRow = false;
        }

        public void QueryTemLot(string inProd, string inWHouse, string inWHLoca)
        {
            if (this.dtsDataEnv.Tables[StockAgent.xd_Alias_TemLot] != null)
            {
                this.dtsDataEnv.Tables.Remove(StockAgent.xd_Alias_TemLot);
            }
            this.dtsDataEnv.Tables.Add(this.mStockAgent.GetLotTable(this.mstrBranchID, inProd, inWHouse, inWHLoca));
        }

        private bool pmValidQty(bool inSetError, ref string ioErrorMsg)
        {
            bool bllResult = true;

            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle];
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            decimal decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
            string lcCtrlStock = BizRule.GetProdCtrlStock(dtrTemPd["cCtrlStoc"].ToString(), App.ActiveCorp.SCtrlStock);

            if (lcCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE && decQty > 0)
            {
                ioErrorMsg = "จำนวนต้องไม่เท่ากับ 0";
                if (Convert.ToDecimal(dtrTemPd["nStQty"]) == 0)
                {
                    decimal decMinQty = 0;
                    decimal decStMinQty = 0;
                    //decimal decRetStQty = 0;
                    bool bllQtySucc = false;
                    bool bllStQtySucc = false;

                    this.mStockAgent.TestUpdateStock
                        (
                    #region "Stock Parameter"
this.mintUpdStockSign
                        , this.mstrBranchID
                        , dtrTemPd["cPdType"].ToString()
                        , dtrTemPd["cProd"].ToString()
                        , dtrTemPd["cWHouse"].ToString()
                        , dtrTemPd["cWHLoca"].ToString()
                        , dtrTemPd["cLot"].ToString()
                        , lcCtrlStock
                        , Convert.ToDecimal(dtrTemPd["nQty"])
                        , Convert.ToDecimal(dtrTemPd["nUOMQty"])
                        , Convert.ToDecimal(dtrTemPd["nOQty"])
                        , Convert.ToDecimal(dtrTemPd["nOUMQty"])
                        , dtrTemPd["cQnUOM"].ToString()
                        , ref decMinQty
                        , ref bllQtySucc
                        , App.ActiveCorp.SCtrlStock
                        , Convert.ToDecimal(dtrTemPd["nStQty"])
                        , Convert.ToDecimal(dtrTemPd["nStUOMQty"])
                        , Convert.ToDecimal(dtrTemPd["nOStQty"])
                        , Convert.ToDecimal(dtrTemPd["nOStUMQty"])
                        , App.ActiveCorp.SStCtrlStock
                        , ref decStMinQty
                        , ref bllStQtySucc
                        , inSetError
                        , false
                        , false
                        , ref ioErrorMsg
                    #endregion
);

                    if ((!bllQtySucc || !bllStQtySucc)
                        && (ioErrorMsg != string.Empty))
                    {
                        MessageBox.Show(ioErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!bllQtySucc)
                    {
                        bllResult = false;
                        dtrTemPd["nQty"] = decMinQty;
                    }
                    if (!bllStQtySucc)
                    {
                        bllResult = false;
                        dtrTemPd["nStQty"] = decStMinQty;
                    }

                }
                else
                {
                }
                return bllResult;
            }
            else
            {
                bllResult = true;
            }
            return bllResult;
        }

        DataRow mdtvActiveRow = null;
        private void pmGetGridActiveRow()
        {
            this.mdtvActiveRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
        }

        private void pmShowDetail()
        {
            this.pmGetGridActiveRow();
            if (this.mdtvActiveRow != null && this.mdtvActiveRow["cProd"].ToString().TrimEnd() != string.Empty)
            {
                string strMsg = "";
                strMsg = "สินค้า : " + this.mdtvActiveRow["cQcProd"].ToString().TrimEnd() + "[" + this.mdtvActiveRow["cQnProd"].ToString().TrimEnd() + this.mdtvActiveRow["cRemark1"].ToString().TrimEnd() + "]";
                strMsg += "\r\nหน่วยนับ [" + this.mdtvActiveRow["cQnUom"].ToString().TrimEnd() + "]" + AppUtil.StringHelper.Space(15);
                strMsg += "\r\nLot [" + this.mdtvActiveRow["cLot"].ToString().TrimEnd() + "]";
                strMsg += "\r\nจำนวน [" + Convert.ToDecimal(this.mdtvActiveRow["nQty"]).ToString("#,###,###,##0.00") + "]";
                strMsg += "   ราคา [" + Convert.ToDecimal(this.mdtvActiveRow["nPrice"]).ToString("#,###,###,##0.00") + "]";
                strMsg += "   ส่วนลด [" + this.mdtvActiveRow["cDiscStr"].ToString().TrimEnd() + "]";
                strMsg += "   เป็นเงิน [" + Convert.ToDecimal(this.mdtvActiveRow["nAmt"]).ToString("#,###,###,##0.00") + "]";

                string strSaleOrBuy = (this.mstrSaleOrBuy == "S" ? "ราคาขาย" : "ราคาซื้อ");
                string strMaxRef = "";
                string strMinRef = "";
                string strLastRef = "";
                decimal decMaxAmt = 0;
                decimal decMinAmt = Convert.ToDecimal(999999999.99);
                DateTime dttLastDate = DateTime.MinValue;
                DateTime dttCurDate = DateTime.Now;

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                string strProd = this.mdtvActiveRow["cProd"].ToString();
                if (this.mstrSaleOrBuy == "S" && this.txtQcCoor.Tag.ToString().TrimEnd() != string.Empty)
                {
                    pobjSQLUtil.SetPara(new object[] { this.mstrBranchID, this.txtQcCoor.Tag, strProd });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from RefProd where fcBranch = ? and fcCoor = ? and fcProd = ?", ref strErrorMsg))
                    {
                        foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QRefProd"].Rows)
                        {
                            if ((SysDef.gc_RFTYPE_INV_SELL + SysDef.gc_RFTYPE_DR_SELL).IndexOf(dtrRefProd["fcRfType"].ToString()) > -1)
                            {
                                #region "Cal Min&Max Price"
                                decimal decUmQty = (Convert.ToDecimal(dtrRefProd["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                                if (Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty >= decMaxAmt)
                                {
                                    //ราคาขายสูงสุด
                                    decMaxAmt = Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty;
                                    strMaxRef = dtrRefProd["fcSkid"].ToString();
                                }
                                if (Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty <= decMinAmt && Convert.ToDecimal(dtrRefProd["fnPrice"]) != 0)
                                {
                                    //ราคาขายต่ำสุด
                                    decMinAmt = Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty;
                                    strMinRef = dtrRefProd["fcSkid"].ToString();
                                }

                                if (Convert.ToDateTime(dtrRefProd["fdDate"]).Date.CompareTo(dttLastDate.Date) > 0
                                    || (Convert.ToDateTime(dtrRefProd["fdDate"]).Date.CompareTo(dttLastDate.Date) == 0 && Convert.ToDateTime(dtrRefProd["ftDateTime"]).CompareTo(dttCurDate) > 0)
                                    && Convert.ToDecimal(dtrRefProd["fnPrice"]) != 0)
                                {
                                    dttLastDate = Convert.ToDateTime(dtrRefProd["fdDate"]);
                                    dttCurDate = Convert.ToDateTime(dtrRefProd["ftDateTime"]);
                                    strLastRef = dtrRefProd["fcSkid"].ToString();
                                }
                                #endregion
                            }//if 3
                        }//foreach
                    }//if 2
                }//if 1
                else if (strProd != "" && this.mstrSaleOrBuy != "S")
                {
                    pobjSQLUtil.SetPara(new object[] { strProd, this.mstrBranchID });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from RefProd where fcProd = ? and fcBranch = ? ", ref strErrorMsg))
                    {
                        foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QRefProd"].Rows)
                        {
                            if ((SysDef.gc_RFTYPE_INV_BUY + SysDef.gc_RFTYPE_DR_BUY).IndexOf(dtrRefProd["fcRfType"].ToString()) > -1)
                            {
                                decimal decUmQty = (Convert.ToDecimal(dtrRefProd["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                                #region "Cal Min&Max Buy Price"
                                if (Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty >= decMaxAmt)
                                {
                                    //ราคาซื้อสูงสุด
                                    decMaxAmt = Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty;
                                    strMaxRef = dtrRefProd["fcSkid"].ToString();
                                }
                                if (Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty <= decMinAmt && Convert.ToDecimal(dtrRefProd["fnPrice"]) != 0)
                                {
                                    //ราคาซื้อต่ำสุด
                                    decMinAmt = Convert.ToDecimal(dtrRefProd["fnPrice"]) / decUmQty;
                                    strMinRef = dtrRefProd["fcSkid"].ToString();
                                }

                                if (Convert.ToDateTime(dtrRefProd["fdDate"]).Date.CompareTo(dttLastDate.Date) > 0
                                    || (Convert.ToDateTime(dtrRefProd["fdDate"]).Date.CompareTo(dttLastDate.Date) == 0 && Convert.ToDateTime(dtrRefProd["ftDateTime"]).CompareTo(dttCurDate) > 0)
                                    && Convert.ToDecimal(dtrRefProd["fnPrice"]) != 0)
                                {
                                    dttLastDate = Convert.ToDateTime(dtrRefProd["fdDate"]);
                                    dttCurDate = Convert.ToDateTime(dtrRefProd["ftDateTime"]);
                                    strLastRef = dtrRefProd["fcSkid"].ToString();
                                }
                                #endregion
                            }
                        }
                    }
                }

                string strDate = "วันที่ [";
                string strLastAmt = "";
                string strMaxAmt = "";
                string strMinAmt = "";
                string strRefProdCmd = "select REFPROD.FDDATE, REFPROD.FNPRICE, UM.FCNAME as QnUM, CURRENCY.FCNAME as QnCurrency from REFPROD ";
                strRefProdCmd += " left join UM on UM.FCSKID = REFPROD.FCUM ";
                strRefProdCmd += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
                strRefProdCmd += " left join CURRENCY on CURRENCY.FCSKID = GLREF.FCCURRENCY ";
                strRefProdCmd += " where REFPROD.FCSKID = ?";
                DataRow dtrTem = null;

                if (strLastRef != string.Empty)
                {
                    pobjSQLUtil.SetPara(new object[] { strLastRef });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                    {
                        dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                        string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                        strLastAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                    }
                }

                if (strMaxRef != string.Empty)
                {
                    pobjSQLUtil.SetPara(new object[] { strMaxRef });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                    {
                        dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                        string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                        strMaxAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                    }
                }

                if (strMinRef != string.Empty)
                {
                    pobjSQLUtil.SetPara(new object[] { strMinRef });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                    {
                        dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                        string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                        strMinAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                    }
                }
                strMsg += "\r\n" + strSaleOrBuy + "ล่าสุด [" + strLastAmt + "]";
                strMsg += "\r\n" + strSaleOrBuy + "สูงสุด [" + strMaxAmt + "]";
                strMsg += "\r\n" + strSaleOrBuy + "ต่ำสุด [" + strMinAmt + "]";

                MessageBox.Show(this, strMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void pmShowHistory()
        {
            this.pmGetGridActiveRow();
            //TODO:XXX
            //using (Common.dlgGetWhHType dlg = new Common.dlgGetWhHType(this.mstrSaleOrBuy))
            //{
            //    dlg.ShowDialog();
            //    if (dlg.DialogResult == DialogResult.OK)
            //    {

            //        //TODO:XXX
            //        //if (this.oShowPdHistory == null)
            //        //{
            //        //    this.oShowPdHistory = new Common.cShowPdHistory(this.mstrSaleOrBuy);
            //        //}
            //        switch (dlg.SelectedIndex)
            //        {
            //            case 0:
            //                if (this.txtQcCoor.Tag != string.Empty)
            //                    this.oShowPdHistory.ShowCoorHistory(this.mstrBranchID, this.txtQcCoor.Tag);
            //                break;
            //            case 1:
            //                if (this.mdtvActiveRow != null)
            //                    //TODO:XXX
            //                    //this.oShowPdHistory.ShowProdHistory(this.mstrBranchID, this.mdtvActiveRow["cProd"].ToString());
            //                break;
            //        }

            //    }
            //}
        }

        private void pmShowHistory2(string inType)
        {

            //TODO:XXX
            //this.pmGetGridActiveRow();

            //if (this.oShowPdHistory == null)
            //{
            //    this.oShowPdHistory = new Common.cShowPdHistory(this.mstrSaleOrBuy);
            //}
            //switch (inType)
            //{
            //    case "PROD":
            //        if (this.mdtvActiveRow != null)
            //            this.oShowPdHistory.ShowProdHistory(this.mstrBranchID, this.mdtvActiveRow["cProd"].ToString());
            //        break;
            //    case "COOR":
            //        if (this.txtQcCoor.Tag != string.Empty)
            //            this.oShowPdHistory.ShowCoorHistory(this.mstrBranchID, this.txtQcCoor.Tag);
            //        break;
            //}

        }

        //private void cmdHis_Coor_Click(object sender, CommandEventArgs e)
        //{
        //    this.pmShowHistory2("PROD");
        //}

        //private void cmdHis_Prod_Click(object sender, CommandEventArgs e)
        //{
        //    this.pmShowHistory2("COOR");
        //}

        private DataRow pmNewItem(int inCurrRow, ref bool ioIsNewRow)
        {
            DataRow dtrNewRow = null;
            bool bllHasEmptyRow = false;
            //foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            for (int intCnt = inCurrRow; intCnt < this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count; intCnt++)
            {
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[intCnt];
                if (dtrTemPd["cQcProd"].ToString().TrimEnd() == string.Empty && Convert.ToDecimal(dtrTemPd["nQty"]) == 0)
                {
                    dtrNewRow = dtrTemPd;
                    bllHasEmptyRow = true;
                    ioIsNewRow = false;
                    break;
                }
            }
            if (!bllHasEmptyRow)
            {
                dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                ioIsNewRow = true;
            }
            return dtrNewRow;
        }

        private void pmChgIQty(DataRow inTemPd, string inRootSeq)
        {
            decimal decQty = 0;
            decimal decDiscAmt = 0;
            decimal decAmt = 0;
            decimal decPrice = 0;
            decimal decMQtyStd = Convert.ToDecimal(inTemPd["nQty"]) * Convert.ToDecimal(inTemPd["nUOMQty"]);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {

                if (dtrTemPd["cRootSeq"].ToString() == inRootSeq)
                {
                    if (BizRule.IsRootFormula(dtrTemPd["cPFormula"].ToString()))
                    {
                        decQty = Convert.ToDecimal(inTemPd["nQty"]);
                    }
                    else
                    {
                        decQty = Convert.ToDecimal(dtrTemPd["nQtyPerMFm"]) * decMQtyStd;
                    }

                    decAmt = decQty * Convert.ToDecimal(dtrTemPd["nPrice"]);
                    decDiscAmt = BizRule.CalDiscAmtFromDiscStr(dtrTemPd["cDiscStr"].ToString(), decAmt, decQty, 0);
                    decPrice = BizRule.LimitPrcCost(Convert.ToDecimal(dtrTemPd["nPrice"]));

                    dtrTemPd["nQty"] = decQty;
                    dtrTemPd["nBackQty"] = decQty;
                    dtrTemPd["nLastQty"] = decQty;
                    dtrTemPd["nPrice"] = decPrice;
                    dtrTemPd["nLastPrice"] = decPrice;
                    dtrTemPd["nDiscAmt"] = decDiscAmt;
                }
            }
        }

        private void pmethChgMFMPrice(string tcinRootSeq, bool inChgQtyPerMfm)
        {

            DataRow[] dtaFM = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cRootSeq = '" + tcinRootSeq + "'");
            if (dtaFM.Length > 0)
            {
                DataRow[] dtaPFM = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cRefPdType = 'F' and cQcProd = '" + dtaFM[0]["cQcPFormula"].ToString() + "'");
                decimal decAmt = 0;
                decimal decDiscAmt = 0;
                decimal decSumAmt = 0;
                decimal decMFMQty = Convert.ToDecimal(dtaPFM[0]["nQty"]);
                //Sum Item Amt
                for (int intCnt = 0; intCnt < dtaFM.Length; intCnt++)
                {

                    if (dtaFM[intCnt]["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
                    {
                        //decSumAmt = decSumAmt + Math.Abs((Convert.ToDecimal(dtaFM[intCnt]["nQty"]) *Convert.ToDecimal(dtaFM[intCnt]["nPrice"])));
                        decAmt = Math.Abs((Convert.ToDecimal(dtaFM[intCnt]["nQty"]) * Convert.ToDecimal(dtaFM[intCnt]["nPrice"])));
                        decDiscAmt = BizRule.CalDiscAmtFromDiscStr(dtaFM[intCnt]["cDiscStr"].ToString(), decAmt, 0, 0);
                        decSumAmt += (decAmt - decDiscAmt);
                    }

                }

                if (decMFMQty != 0)
                {
                    dtaPFM[0]["nPrice"] = decSumAmt / decMFMQty;
                    dtaPFM[0]["nLastPrice"] = decSumAmt / decMFMQty;
                }

            }


        }

        private void btnLoopIns_Click(object sender, System.EventArgs e)
        {
            this.pmInitPopUpDialog("LOOPINSERT");
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            //ColumnView view = this.grdTemPd.MainView as ColumnView;

            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nDummyFld", 0);
            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nRecNo", this.gridView2.FocusedRowHandle+1);
            //this.gridView2.UpdateCurrentRow();

            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            string strValue = e.Value.ToString();
            string strRefPdType = dtrTemPd["cRefPdType"].ToString();
            string strProdID = dtrTemPd["cProd"].ToString();

            if (strCol.ToUpper() == "CQCPROD"
                && strValue == dtrTemPd["cQcProd"].ToString())
            {
                return;
            }
            else if (strCol.ToUpper() == "CQNPROD"
                && strValue == dtrTemPd["cQnProd"].ToString())
            {
                return;
            }

            switch (strCol.ToUpper())
            {

                case "CPDTYPE":
                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cPdType"] = "";
                        this.pmClr1TemPd();
                    }
                    else
                    {
                        this.pmInitPopUpDialog("PDTYPE");
                        string strOrderBy = (strCol.ToUpper() == "CPDTYPE" ? "FCCODE" : "FCNAME");
                        e.Valid = !this.pofrmGetPdType.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetPdType.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CPDTYPE" ? dtrTemPd["cPdType"].ToString().TrimEnd() : dtrTemPd["cPdType"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cPdType"] = "";
                            this.pmClr1TemPd();
                        }
                    }
                    break;
                case "CQCPROD":
                case "CQNPROD":

                    if (strValue.Trim() != string.Empty)
                    {
                        this.pmInitPopUpDialog("PRODUCT");

                        e.Valid = !this.pofrmGetProd.ValidateField(strValue, strCol.ToUpper() == "CQCPROD" ? "FCCODE" : "FCNAME", false);

                        if (this.pofrmGetProd.PopUpResult)
                        {
                            string strRetVal = this.pmRetrievePopUpVal(strCol.ToUpper());
                            e.Value = strRetVal;
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.gridView2.FocusedRowHandle = this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count - 1;
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd[strCol] = "";
                            this.pmClr1TemPd(dtrTemPd);
                        }
                    }
                    else
                    {
                        e.Value = "";
                        dtrTemPd[strCol] = "";
                        this.pmClr1TemPd(dtrTemPd);
                    }

                    break;
                case "NQTY":
                    strValue = (strValue != string.Empty ? strValue : "0");
                    decimal decQty = Convert.ToDecimal(strValue);
                    decimal decLastQty = Convert.ToDecimal(dtrTemPd["nLastQty"]);

                    if (decQty != decLastQty)
                    {

                        if (this.pmChkReferItem("E"))
                        {
                            this.gridView2.SetFocusedValue(decLastQty);
                            return;
                        }

                        if (decQty > 0 && strProdID == string.Empty)
                        {
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nQty", 0);
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nBackQty", 0);
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nBackDOQty", 0);
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nDOQty", 0);
                            this.gridView2.UpdateCurrentRow();
                            MessageBox.Show("กรุณาระบุสินค้าก่อนระบุจำนวนสินค้า/บริการ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            this.gridView2.SetFocusedValue(decQty);
                            if (this.pmValidQtyCol())
                                this.mbllRecalTotPd = true;
                            else
                            {
                                this.gridView2.SetFocusedValue(decLastQty);
                                e.Value = decLastQty;
                                //e.Valid = false;
                            }
                        }
                        this.mbllRecalTotPd = true;
                    }
                    break;
                case "NPRICE":
                    strValue = (strValue != string.Empty ? strValue : "0");
                    decimal decPrice = Convert.ToDecimal(strValue);
                    decimal decLastPrice = Convert.ToDecimal(dtrTemPd["nLastPrice"]);

                    if (strRefPdType == SysDef.gc_REFPD_TYPE_PRODUCT)
                    {
                        if (decPrice > 0 && strProdID == string.Empty)
                        {
                            this.gridView2.SetFocusedValue(decLastPrice);
                            e.Value = decLastPrice;
                            MessageBox.Show("กรุณาระบุสินค้าก่อนระบุราคาสินค้า/บริการ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nPrice", 0);
                            //this.gridView2.UpdateCurrentRow();
                            //this.grdTemPd.SetValue("nPrice", 0);
                            //e.Cancel = true;
                        }
                        else
                        {
                            //this.gridView2.SetFocusedValue(decPrice);
                            //e.Value = decPrice;
                            //decimal decAmt1 = Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"]);
                            //decimal decDiscAmt1 = Convert.ToDecimal(dtrTemPd["nDiscAmt"]);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nAmt", decAmt1 - decDiscAmt1);
                            this.pmValidPriceCol();
                            this.mbllRecalTotPd = true;
                        }
                    }
                    else
                    {
                        if (decPrice > 0 && decPrice != decLastPrice)
                        {
                            this.gridView2.SetFocusedValue(decLastPrice);
                            e.Value = decLastPrice;
                            MessageBox.Show("แก้ไขราคาของชุดสินค้าไม่ได้", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //this.grdTemPd.SetValue("nPrice", decLastPrice);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nPrice", decLastPrice);
                        }
                    }
                    break;
                case "CDISCSTR":
                    //string decLastAmt = Convert.ToDecimal(this.grdTemPd.GetValue("nAmt"));
                    decimal decAmt = Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"]);
                    decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(strValue, decAmt, 0, 0);
                    if (decDiscAmt <= decAmt)
                    {
                        this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nDiscAmt", decDiscAmt);
                        this.pmValidDiscountCol();

                        this.mbllRecalTotPd = true;
                    }
                    else
                    {
                        e.Valid = true;
                        e.Value = "";
                        MessageBox.Show("ส่วนลดมากกว่ามูลค่าสินค้า !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //this.grdTemPd.SetValue("cDiscStr", "");
                    }
                    break;
            }
            if (this.mbllRecalTotPd)
                this.pmRecalTotPd();
        }

        private void grcRemark_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            if (dtrTemPd == null)
            {
                dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }

            this.pmInitPopUpDialog("REMARK_ITEM");
            //this.pofrmGetUM.ValidateField("", "FCNAME", true);
            //if (this.pofrmGetUM.PopUpResult)
            //{
            //    //this.gridView2.UpdateCurrentRow();
            //    this.pmRetrievePopUpVal("CQNUM");
            //}
        }

        private void grcQty_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            if (dtrTemPd == null)
            {
                dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }

            this.pmInitPopUpDialog("QTY_ITEM", true);
        }

        private void grcColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    //this.pmInitPopUpDialog("UM");
                    //this.pofrmGetUM.ValidateField("", "FCNAME", true);
                    //if (this.pofrmGetUM.PopUpResult)
                    //{
                    //    //this.gridView2.UpdateCurrentRow();
                    //    DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                    //    if (dtrTemPd == null)
                    //    {
                    //        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPdVer].NewRow();
                    //        this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows.Add(dtrTemPd);
                    //    }
                    //    this.pmRetrievePopUpVal("CQNUM");
                    //}
                    break;
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[e.RowHandle];
                if (Convert.IsDBNull(dtrTemPd["nQty"]))
                {
                    dtrTemPd["nQty"] = 0;
                }
                if (Convert.IsDBNull(dtrTemPd["nPrice"]))
                {
                    dtrTemPd["nPrice"] = 0;
                }
                if (Convert.IsDBNull(dtrTemPd["nDiscAmt"]))
                {
                    dtrTemPd["nDiscAmt"] = 0;
                }
                dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);
                //dtrTemPd["nRecNo"] = gridView1.RowCount;

            }
        }

        private void gridView2_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(e.RowHandle);
            dtrTemPd["nQty"] = 0;
            dtrTemPd["nPrice"] = 0;
            dtrTemPd["nDiscAmt"] = 0;
        }

        private void txtEditDet1_Enter(object sender, EventArgs e)
        {
            this.txtEditDet1.SelectAll();
        }

        private void txtEditDet1_TextChanged(object sender, EventArgs e)
        {
            this.grdTemPd.Focus();
        }

        private void txtEditDet1_Validated(object sender, EventArgs e)
        {
            if (this.txtEditDet1.Text != "N")
                this.pmInitPopUpDialog("GET_OTH");
        }

        private void txtVatAmt_Validated(object sender, EventArgs e)
        {

        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;
        }

        private string pmPTextField2(string inText)
        {
            return AppUtil.StringHelper.ChrTran(inText, "|", Convert.ToChar(13).ToString());
        }

    }

}
