
//TODO: ยังไม่เสร็จเรื่อง แก้สินค้า A=>B จะ Update Stock ผิด

//#define xd_RUNMODE_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using DevExpress.XtraGrid.Views.Base;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.Report.Agents;

namespace BeSmartMRP.Transaction
{

    public partial class frmStmove : UIHelper.frmBase, IfrmStockBase
    {

        public static string TASKNAME = "ESTMOVE_";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrHTable = QMFStmoveHDInfo.TableName;
        private string mstrRefTable = QMFStmoveHDInfo.TableName;
        private string mstrITable = MapTable.Table.RefProd;

        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrCanEditMsg = "";
        private bool mbllIsGetPdSer = true;

        private string mstrBookPrefix = "";
        private string mstrBookRunCodeType = "";
        private int mintBookRunCodeLen = 4;
        private string mstrRunCodeStyle = "";

        private string mstrDefaWHouse = "";
        private string mstrDefaWHouseTypeList = " ";//SysDef.gc_WAREHOUSE_TYPE_NORMAL;
        //private WHLocaInfo mDefaWHLoca = new WHLocaInfo();

        private bool mbllWaitForApprove = false;
        private string mstrStep = SysDef.gc_STEP_IGNORE;
        private string mstrAtStep = "";

        private DateTime mdttLastLock = DBEnum.NullDate;
        private DateTime mdttBookLastLock = DBEnum.NullDate;
        private DateTime mdttAccBookLastLock = DBEnum.NullDate;
        private DateTime mdttLastClose = DBEnum.NullDate;

        private string mstrLastQcWHouse = "";	//คลังสินค้า
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrActiveOP = "";
        private string mstrSortKey = QMFStmoveHDInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";
        private decimal mdecCurrQty = 0;

        private ArrayList pATagCode = new ArrayList();

        private string mstrFrWhType = "";
        private string mstrToWhType = "";

        private string mstrFrWhType2 = "";
        private string mstrToWhType2 = "";

        private string mstrSaleOrBuyForPdSer = "P";
        private CoorType mCoorType = CoorType.Customer;

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrTemFG = "TemFG";
        private string mstrTemPd = "TemPd";
        private string mstrTemOP = "TemOP";
        private string mstrTemPdX1 = "TemPdX1";
        private string mstrTemRoute = "TemRoute";
        private string mstrTemRefTo = "TemRefTo";

        private string mstrTemRefTo_Pack = "TemRefTo_Pack";
        private string mstrTemRefTo_Pack_Det = "TemRefTo_Pack_Det";

        public const string xd_Alias_Tem1PdSer = "Tem1PdSer";

        private string mstrTemPd_GenPR = "TemPd_GenPR";
        private string mstrGenPR_Branch = "";
        private string mstrGenPR_Book = "";
        private string mstrGenPR_Coor = "";
        private string mstrGenPR_Code = "";
        private string mstrGenPR_RefNo = "";
        private DateTime mdttGenPR_Date = DateTime.Now;
        private int mintGenPR_CredTerm = 0;
        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private string mstrTemGenFG = "TemGenFG";
        private string mstrTemGenPd = "TemGenPd";
        private string mstrTemGenOP = "TemGenOP";

        private string mstrEditRowID = "";
        private string mstrSaveRowID = "";
        private string mstrEditRowID_PR = "";
        private DocumentType mDocType = DocumentType.WR;
        private string mstrRefType = DocumentType.WR.ToString();
        private string mstrRefTypeName = "";
        private string mstrRfType = "O";
        private DocumentType mRefType = DocumentType.WR;
        private string mstrPDocCode = "";
        private string mstrPlant = "";
        private string mstrBranch = "";
        private string mstrBook = "";
        private string mstrQcBook = "";
        private string mstrPdType = "";
        private string mstrStdUM = "";
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private string mstrFrWkCtrH = "";
        private string mstrToWkCtrH = "";

        //รายละเอียดเอกสารอ้างอิง
        private string mstrRefToRefType = DocumentType.PO.ToString();
        private string mstrRefToTab = "ORDERH";
        private string mstrRefToBook = "";
        private string mstrRefToRowID = "";
        private decimal mdecRefToAmt = 0;
        private string mstrRefToMOrderOP = "";
        private string mstrRefToWOrderI = "";
        private string mstrOldRefToRowID = "";
        private string mstrOldRefToMOrderOP = "";

        private decimal mdecSumMOQty = 0;
        private decimal mdecSumGoodQty = 0;
        private decimal mdecSumWasteQty = 0;
        private decimal mdecSumLossQty = 0;

        private string mstrDefaFGWHouse = "";
        private string mstrDefaWRWHouse = "";
        private string mstrDefaRWWHouse = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrDefaSect = "";
        private string mstrDefaJob = "";

        private bool bllWaitApprove = false;

        private decimal mdecPdStOutput = 1;
        private decimal mdecLastMfgQty = 0;
        private decimal mdecLastTotMfgQty = 0;
        private decimal mdecScrapQty = 0;
        private string mstrLastScrap = "";
        private string mstrBatchNo = "";
        private string mstrBatchRun = "";
        private string mstrLevel = "";
        private string mstrMasterWO = "";
        private string mstrParentWO = "";

        private string mstrFormMenuName = UIBase.GetAppUIText(new string[] { "BOM", "BOM" });

        private UIHelper.AppFormState mFormEditMode;

        private MfgResourceType mResourceType = MfgResourceType.Machine;

        private string mstrOldCode = "";
        private string mstrOldRefNo = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private bool mbllRecalTotPd = false;

        private decimal mdecUMQty = 0;

        private PdSer mPdSer = new PdSer();
        private StockAgent mStockAgent = new StockAgent(App.ERPConnectionString, App.ConnectionString, App.DatabaseReside);

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private bool mbllIsPrintGroup = false;
        private string mstrPFormulaStype = "2";

        private DatabaseForms.frmMFResType pofrmGetResType = null;
        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DatabaseForms.frmMFWorkCenter pofrmGetWkCtr = null;
        private DatabaseForms.frmMFStdOpr pofrmGetStdOP = null;
        private DatabaseForms.frmMFResource pofrmGetResource = null;

        private DialogForms.dlgGetProdType pofrmGetProdType = null;
        //private DialogForms.dlgGetProd pofrmGetProd = null;
        private DatabaseForms.frmProd pofrmGetProd = null;
        private DialogForms.dlgGetUM pofrmGetUM = null;
        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;

        private Common.MRP.dlgGetRefToRoute pofrmGetOPSeq = null;
        //private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DatabaseForms.frmEMSuppl pofrmGetSuppl = null;
        private DatabaseForms.frmCust pofrmGetCust = null;
        private DialogForms.dlgGetBOM pofrmGetBOM = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;

        private UIHelper.IfrmDBBase pofrmGetWHLoca = null;

        #region "Grid Action Method"

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.Tag != null)
            {
                string strMenuName = e.Button.Tag.ToString().ToUpper();
                WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                switch (oToolButton)
                {
                    case WsToolBar.RowInsert:
                        this.pmInsertGridRow();
                        break;
                    case WsToolBar.RowDelete:
                        this.pmClr1TemPd();
                        break;
                    case WsToolBar.RowMoveUp:
                        this.pmMoveGridRow(1);
                        break;
                    case WsToolBar.RowMoveDown:
                        this.pmMoveGridRow(-1);
                        break;
                }
            }
        }

        private void pmInsertGridRow()
        {
            if (this.mActiveGrid.FocusedRowHandle < 0
                || this.mActiveGrid.FocusedRowHandle == this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Count)
                return;

            int intFocus = this.mActiveGrid.FocusedRowHandle;
            DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
            DataRow dtrOldRow = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
            for (int intCnt = this.mActiveGrid.FocusedRowHandle; intCnt < this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Count; intCnt++)
            {
                DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrActiveTem].Rows[intCnt];
                dtrNewRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]);
                dtrCurrRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]) + 1;

                DataSetHelper.CopyDataRow(dtrCurrRow, ref dtrOldRow);
                DataSetHelper.CopyDataRow(dtrNewRow, ref dtrCurrRow);
                DataSetHelper.CopyDataRow(dtrOldRow, ref dtrNewRow);
            }
            this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrNewRow);
            this.mActiveGrid.FocusedRowHandle = intFocus;
            //this.mActiveGrid.Refetch();
            //this.mActiveGrid.Refresh();
        }

        private void pmMoveGridRow(int inDirection)
        {
            if ((inDirection == 1 && this.mActiveGrid.FocusedRowHandle == 0)
                || inDirection == -1 && this.mActiveGrid.FocusedRowHandle == this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Count - 1)
                return;

            DataRow dtrSwapRow = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
            DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrActiveTem].Rows[this.mActiveGrid.FocusedRowHandle];
            DataRow dtrUpRow = this.dtsDataEnv.Tables[this.mstrActiveTem].Rows[this.mActiveGrid.FocusedRowHandle - inDirection];

            int intSwapRecNo = Convert.ToInt32(dtrCurrRow["nRecNo"]);
            dtrCurrRow["nRecNo"] = Convert.ToInt32(dtrUpRow["nRecNo"]);
            dtrUpRow["nRecNo"] = intSwapRecNo;

            DataSetHelper.CopyDataRow(dtrCurrRow, ref dtrSwapRow);
            DataSetHelper.CopyDataRow(dtrUpRow, ref dtrCurrRow);
            DataSetHelper.CopyDataRow(dtrSwapRow, ref dtrUpRow);

            this.mActiveGrid.FocusedRowHandle = this.mActiveGrid.FocusedRowHandle - inDirection;
            //this.mActiveGrid.Refetch();
            //this.mActiveGrid.Refresh();
        }
        #endregion

        public frmStmove(string inRefType)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.mDocType = BusinessEnum.GetDocEnum(this.mstrRefType);
            this.pmInitForm();
        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
            {
                base.OnClosing(ce);
                App.ActivateMainScreen();
            }
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "Do you want to close this form ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        }
        
        private static frmStmove mInstanse_1 = null;
        private static frmStmove mInstanse_2 = null;
        private static frmStmove mInstanse_3 = null;
        private static frmStmove mInstanse_4 = null;
        private static frmStmove mInstanse_5 = null;
        private static frmStmove mInstanse_6 = null;
        private static frmStmove mInstanse_7 = null;

        public static frmStmove GetInstanse(DocumentType inRefType)
        {

            switch (inRefType)
            {
                case DocumentType.WR:
                    if (mInstanse_1 == null)
                    {
                        mInstanse_1 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_1;
                    break;
                case DocumentType.WX:
                    if (mInstanse_2 == null)
                    {
                        mInstanse_2 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_2;
                    break;
                case DocumentType.RW:
                    if (mInstanse_3 == null)
                    {
                        mInstanse_3 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_3;
                    break;
                case DocumentType.RX:
                    if (mInstanse_4 == null)
                    {
                        mInstanse_4 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_4;
                    break;
                case DocumentType.FR:
                    if (mInstanse_5 == null)
                    {
                        mInstanse_5 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_5;
                    break;
                case DocumentType.TR:
                    if (mInstanse_6 == null)
                    {
                        mInstanse_6 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_6;
                    break;
                case DocumentType.GD:
                    if (mInstanse_7 == null)
                    {
                        mInstanse_7 = new frmStmove(inRefType.ToString());
                    }
                    return mInstanse_7;
                    break;
            }
            return null;
        }

        private static void pmClearInstanse(DocumentType inRefType)
        {

            switch (inRefType)
            {
                case DocumentType.WR:
                    if (mInstanse_1 != null)
                    {
                        mInstanse_1 = null;
                    }
                    break;
                case DocumentType.WX:
                    if (mInstanse_2 != null)
                    {
                        mInstanse_2 = null;
                    }
                    break;
                case DocumentType.RW:
                    if (mInstanse_3 != null)
                    {
                        mInstanse_3 = null;
                    }
                    break;
                case DocumentType.RX:
                    if (mInstanse_4 != null)
                    {
                        mInstanse_4 = null;
                    }
                    break;
                case DocumentType.FR:
                    if (mInstanse_5 != null)
                    {
                        mInstanse_5 = null;
                    }
                    break;
                case DocumentType.TR:
                    if (mInstanse_6 != null)
                    {
                        mInstanse_6 = null;
                    }
                    break;
                case DocumentType.GD:
                    if (mInstanse_7 != null)
                    {
                        mInstanse_7 = null;
                    }
                    break;
            }
        
        }

        private void frmStmove_Load(object sender, EventArgs e)
        {
            this.grdBrowView.RefreshDataSource();
            this.grdBrowView.ForceInitialize();
            this.gridView1.MoveLastVisible();
        }

        public FormActiveMode ActiveMode
        {
            get { return this.mFormActiveMode; }
            set { this.mFormActiveMode = value; }
        }

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public MfgResourceType ResourceType
        {
            get { return this.mResourceType; }
            set { this.mResourceType = value; }
        }

        public string PlantID
        {
            get { return this.mstrPlant; }
            set { this.mstrPlant = value; }
        }

        public void ReInitForm()
        {
            this.pmSetFormUI();
            this.pmInitGridProp();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmCreateTem();
            this.pmInitGridProp_TemPd();
            this.pmInitGridProp_TemStore();

            this.mbllFilterResult = false;
            
            this.pmInitializeComponent();
            this.pmFilterForm();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();

            if (this.mFormActiveMode == FormActiveMode.Edit)
            {
                this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
                if (!this.mbllFilterResult)
                {
                    this.Close();
                    //frmBudAllocate.pmClearInstanse();
                }
            }
        
        }

        private void pmInitializeComponent()
        {

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }


            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefType", "REFTYPE", "select * from RefType where fcSkid = ?", ref strErrorMsg);
            DataRow dtrRefType = this.dtsDataEnv.Tables["QRefType"].Rows[0];
            this.mstrRefTypeName = UIBase.GetAppUIText(new string[] { dtrRefType["fcName"].ToString().TrimEnd(), dtrRefType["fcName2"].ToString().TrimEnd() });
            this.mstrRfType = dtrRefType["fcRfType"].ToString();

            this.mStockAgent.CorpID = App.ActiveCorp.RowID;

            this.pnlRefTo.Visible = false;
            this.pnlLoadRefDoc.Visible = false;
            this.mCoorType = CoorType.Supplier;
            switch (this.mDocType)
            {
                case DocumentType.WR:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_WIP);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1055;
                    this.mCoorType = CoorType.Customer;
                    break;
                case DocumentType.GD:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_WIP);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1055;
                    this.mCoorType = CoorType.Customer;
                    this.pnlLoadRefDoc.Visible = true;
                    this.mstrRefToRefType = "Q1";
                    break;
                case DocumentType.WX:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1055;
                    this.mCoorType = CoorType.Customer;
                    break;
                case DocumentType.RW:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_WIP);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1055;
                    this.mCoorType = CoorType.Customer;

                    this.pnlLoadRefDoc.Visible = true;
                    lblRefTo2.Visible = false;
                    txtRefTo2.Visible = false;
                    //this.pnlLoadRefDoc.Size = new Size(758, 33);

                    break;
                case DocumentType.RX:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1055;
                    this.mCoorType = CoorType.Customer;
                    break;
                case DocumentType.FR:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_WIP);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrSaleOrBuyForPdSer = "P";
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1026;
                    this.pnlLoadRefDoc.Visible = true;
                    this.mCoorType = CoorType.Supplier;

                    break;
                case DocumentType.TR:
                    //this.mstrFrWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_WIP);
                    //this.mstrToWhType = this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL);
                    this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    this.mstrSaleOrBuyForPdSer = "S";
                    this.pnlRefTo.Visible = false;
                    this.imgFormLogo.BackgroundImage = global::BeSmartMRP.Properties.Resources.ico_lrg_1026;
                    break;
            }

            this.pnlRefTo.Visible = false;
            this.ribbonControl1.Visible = false;

            //if (this.pnlLoadRefDoc.Visible == false)
            if (this.mDocType == DocumentType.FR || this.mDocType == DocumentType.GD || this.mDocType == DocumentType.RW)
            {
                if (this.mDocType == DocumentType.GD)
                {
                    //this.txtRefTo2.Visible = false;
                    //this.lblRefTo2.Visible = false;
                }

            }
            else
            {
                pnlBar1.Location = this.pnlLoadRefDoc.Location;
                this.pgfEditItem.Location = new Point(-1, pnlBar1.Location.Y + pnlBar1.Height + 5);
                this.pgfEditItem.Height = this.pgfEditItem.Height + pnlBar1.Height - 15;
            }

            TASKNAME = "ESTMOVE_" + this.mstrRefType.TrimEnd();
            this.Text = this.mstrRefTypeName.TrimEnd();
            this.lblTitle.Text = this.mstrRefTypeName.TrimEnd();

            UIHelper.UIBase.CreateTransactionToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();
            this.pmSetFormUI();

            this.mstrPdType = this.pmSplitToSQLStr(this.pmGetRefProdType());

            UIBase.SetDefaultChildAppreance(this);
            //UIBase.SetDefaultGridViewAppreance(this.gridView2);

            this.txtDate.Enabled = App.AppAllowChangeDate;

        }

        private string pmGetRefProdType()
        {
            string strResult = "";
            strResult = SysDef.gc_PROD_TYPE_FINISH + "," + SysDef.gc_PROD_TYPE_RAW_MAT + "," + SysDef.gc_PROD_TYPE_SEMI + "," + SysDef.gc_PROD_TYPE_ASSET + "," + SysDef.gc_PROD_TYPE_INCOME + ","
                + SysDef.gc_PROD_TYPE_COMPO + "," + SysDef.gc_PROD_TYPE_SPARE + "," + SysDef.gc_PROD_TYPE_LABEL + "," + SysDef.gc_PROD_TYPE_PACKAGE + "," + SysDef.gc_PROD_TYPE_LAND + ","
                + SysDef.gc_PROD_TYPE_BUILDING + "," + SysDef.gc_PROD_TYPE_OFFICE_EQUIP + "," + SysDef.gc_PROD_TYPE_MACHINE + ","
                + SysDef.gc_PROD_TYPE_SUBCONTRAC + "," + SysDef.gc_PROD_TYPE_PLEDGE + "," + SysDef.gc_PROD_TYPE_TOOLS;

            return strResult;
        }

        private void pmSetFormUI()
        {

            this.mstrFormMenuName = this.mstrRefTypeName;
            this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            this.lblCode.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Doc. Code" }) });
            this.lblRefNo.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." }) });
            this.lblDate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Doc. Date" }) });

            this.lblRefer.Text = UIBase.GetAppUIText(new string[] { "#อ้างอิง M/O :", "#Refer M/O :" });
            this.lblFrOP.Text = UIBase.GetAppUIText(new string[] { "OP Seq. :", "OP. Seq :" });

            this.lblFrQcWHouse.Text = UIBase.GetAppUIText(new string[] { "จากคลัง รหัส :", "From Warehouse :" });
            this.lblFrQnWHouse.Text = UIBase.GetAppUIText(new string[] { "ชื่อคลัง :", "Name :" });
            this.lblToQcWHouse.Text = UIBase.GetAppUIText(new string[] { "เข้าคลัง รหัส :", "To Warehouse :" });
            this.lblToQnWHouse.Text = UIBase.GetAppUIText(new string[] { "ชื่อคลัง :", "Name :" });
            this.lblRemark.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ :", "Remark :" });

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });

            this.btnGetOPSeq.Text = UIBase.GetAppUIText(new string[] { "ระบุขั้นตอนการผลิต", "Select OP. Seq" });

            this.btnGetOPSeq.Text = UIBase.GetAppUIText(new string[] { "ระบุขั้นตอนการผลิต", "Select OP. Seq" });

            switch (this.mDocType)
            {
                case DocumentType.FR:
                    this.lblCoor.Text = UIBase.GetAppUIText(new string[] { "ผู้จำหน่าย :", "Supplier :" });
                    break;
                case DocumentType.GD:
                case DocumentType.RW:
                    this.lblCoor.Text = UIBase.GetAppUIText(new string[] { "ลูกค้า :", "Customer :" });
                    this.lblRefTo2.Text = UIBase.GetAppUIText(new string[] { "เอกสารอ้างอิง :", "Refer# :" });
                    break;
            }

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFStmoveHDInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFStmoveHDInfo.Field.RefNo);


        }

        private void pmMapEvent()
        {

            this.Resize += new EventHandler(frmStmove_Resize);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.grdBrowView.Resize += new EventHandler(grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new ColumnEventHandler(gridView1_ColumnWidthChanged);

            this.grdTemPd.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView2.ColumnWidthChanged += new ColumnEventHandler(gridView2_ColumnWidthChanged);
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            this.gridView2.GotFocus += new EventHandler(gridView2_GotFocus);
            this.gridView2.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView2_FocusedRowChanged);
            this.gridView2.CellValueChanged += new CellValueChangedEventHandler(this.gridView2_CellValueChanged);

            this.grcQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcRemark.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcLot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcLot.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcWHLoca.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcWHLoca.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQty.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcQty_ButtonClick);

            this.txtRefTo.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtRefTo.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            
            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);

            this.txtFrOPSeq.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtFrOPSeq.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtFrOPSeq.Validating += new CancelEventHandler(txtFrOPSeq_Validating);

            this.txtFrQcWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtFrQcWHouse.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtFrQcWHouse.Validating += new CancelEventHandler(txtFrQcWHouse_Validating);

            this.txtFrQnWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtFrQnWHouse.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtFrQnWHouse.Validating += new CancelEventHandler(txtFrQcWHouse_Validating);

            this.txtToQcWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtToQcWHouse.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtToQcWHouse.Validating += new CancelEventHandler(txtToQcWHouse_Validating);

            this.txtToQnWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtToQnWHouse.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtToQnWHouse.Validating += new CancelEventHandler(txtToQcWHouse_Validating);

            this.txtQcCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcCoor.Validating += new CancelEventHandler(txtCoor_Validating);

            this.txtRefTo2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtRefTo2.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);

            this.txtRemark.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtRemark.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);

            this.pgfEditItem.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfEditItem_SelectedPageChanged);

        }

        private void frmStmove_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
            this.pmCalcColWidth2();
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec = "";
            strSQLExec = "select GLREF.FCSKID, GLREF.FCCODE, GLREF.FDDATE, GLREF.FCSTAT, GLREF.FCATSTEP, GLREF.FCSTEPX1, GLREF.FCREFNO, GLREF.FCTRANWHY, GLREF.FMMEMDATA ";
            strSQLExec += " , FRWHOUSE.FCCODE AS QCFRWHOUSE , TOWHOUSE.FCCODE AS QCTOWHOUSE ";
            strSQLExec += " , SECT.FCCODE AS QCSECT, JOB.FCCODE AS QCJOB ";
            strSQLExec += " , GLREF.FTDATETIME as DCREATE, GLREF.FTLASTUPD as DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} GLREF ";
            strSQLExec += " left join WHOUSE FRWHOUSE on FRWHOUSE.FCSKID = GLREF.FCFRWHOUSE ";
            strSQLExec += " left join WHOUSE TOWHOUSE on TOWHOUSE.FCSKID = GLREF.FCTOWHOUSE ";
            strSQLExec += " left join SECT on SECT.FCSKID = GLREF.FCSECT ";
            strSQLExec += " left join JOB on JOB.FCSKID = GLREF.FCJOB ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = GLREF.FCCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = GLREF.FCCORRECTB ";
            strSQLExec += " where GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCREFTYPE = ? and GLREF.FCBOOK = ? and GLREF.FDDATE between ? and ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "GLREF", strSQLExec, ref strErrorMsg);

            DataColumn dtcRemark = new DataColumn("CREMARK", System.Type.GetType("System.String"));
            dtcRemark.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRemark);

            DataColumn dtcRefTo_RefNo = new DataColumn("CREFTO_REFNO", System.Type.GetType("System.String"));
            dtcRefTo_RefNo.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRefTo_RefNo);

            DataColumn dtcRefTo_OPSeq = new DataColumn("CREFTO_OP", System.Type.GetType("System.String"));
            dtcRefTo_OPSeq.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRefTo_OPSeq);

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                string strRefTo_RefNo = ""; string strRefTo_OP = "";
                this.pmLoadRefTo2(dtrBrow["fcSkid"].ToString(), ref strRefTo_RefNo, ref strRefTo_OP);

                dtrBrow["CREFTO_REFNO"] = strRefTo_RefNo;
                dtrBrow["CREFTO_OP"] = strRefTo_OP;
                dtrBrow["cRemark"] = this.pmLoadRemark2(dtrBrow);
            
            }

        }

        private string[] pARemark = new String[9] { "", "", "", "", "", "", "", "", "" };

        private void pmLoadRemark(DataRow inSource)
        {
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData2"]) ? "" : inSource["fmMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData3"]) ? "" : inSource["fmMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData4"]) ? "" : inSource["fmMemData4"].ToString().TrimEnd());
            if (inSource["fmMemData5"] != null)
                strRemark += (Convert.IsDBNull(inSource["fmMemData5"]) ? "" : inSource["fmMemData5"].ToString().TrimEnd());

            this.txtRemark.Text = BizRule.GetMemData(strRemark, "Rem");
            pARemark[0] = BizRule.GetMemData(strRemark, "Rm2");
            pARemark[1] = BizRule.GetMemData(strRemark, "Rm3");
            pARemark[2] = BizRule.GetMemData(strRemark, "Rm4");
            pARemark[3] = BizRule.GetMemData(strRemark, "Rm5");
            pARemark[4] = BizRule.GetMemData(strRemark, "Rm6");
            pARemark[5] = BizRule.GetMemData(strRemark, "Rm7");
            pARemark[6] = BizRule.GetMemData(strRemark, "Rm8");
            pARemark[7] = BizRule.GetMemData(strRemark, "Rm9");
            pARemark[8] = BizRule.GetMemData(strRemark, "RmA");
        
        }

        private string pmLoadRemark2(DataRow inSource)
        {
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            return BizRule.GetMemData(strRemark, "Rem");
        }

        private string pmGenOrderStep()
        {
            string strStep = "";
            strStep = "CSTEP = case WCTRANH.CSTEP when 'P' then 'FINISH' when 'L' then 'CLOSE' when '1' then ' ' end";
            return strStep;
        }

        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView2.Columns["cPdType"].VisibleIndex = i++;
            this.gridView2.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView2.Columns["cQnProd"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView2.Columns["cRefPdType"].VisibleIndex = i++;
            this.gridView2.Columns["cPdType"].VisibleIndex = i++;
            this.gridView2.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView2.Columns["cQnProd"].VisibleIndex = i++;
            this.gridView2.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView2.Columns["cLot"].VisibleIndex = i++;
            this.gridView2.Columns["cQcWHLoca"].VisibleIndex = i++;

            if (this.mDocType == DocumentType.IM)
            {
                this.gridView2.Columns["cToQcWHLoca"].VisibleIndex = i++;
            }

            //if (this.mDocType != DocumentType.TR)
            //{
            //    this.gridView2.Columns["nMOQty"].VisibleIndex = i++;
            //    this.gridView2.Columns["nStMoveQty"].VisibleIndex = i++;
            //}

            this.gridView2.Columns["nQty"].VisibleIndex = i++;
            this.gridView2.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView2.Columns["nPrice"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cRefPdType"].Caption = "P/F";
            this.gridView2.Columns["cPdType"].Caption = "T";
            this.gridView2.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView2.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView2.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView2.Columns["cLot"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ล๊อต", "Lot" });
            this.gridView2.Columns["cQcWHLoca"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ที่เก็บ", "Location" });
            this.gridView2.Columns["cToQcWHLoca"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "เข้าที่เก็บ", "To Location" });
            //this.gridView2.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#MO Qty.", "#MO Qty." });

            switch (this.mDocType)
            { 
                case DocumentType.WR:
                case DocumentType.WX:
                case DocumentType.RW:
                case DocumentType.RX:
                    this.gridView2.Columns["nStMoveQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "Issue Qty.", "Issue Qty." });
                    break;
                case DocumentType.FR:
                    this.gridView2.Columns["nStMoveQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "FG Qty.", "FG Qty." });
                    break;
                case DocumentType.TR:
                    this.gridView2.Columns["nStMoveQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "Tranfer Qty.", "Tranfer Qty." });
                    break;
            }

            this.gridView2.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView2.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView2.Columns["nPrice"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ราคาทุน", "Cost" });

            this.gridView2.Columns["nRecNo"].Width = 40;
            this.gridView2.Columns["cRefPdType"].Width = 30;
            this.gridView2.Columns["cPdType"].Width = 50;
            this.gridView2.Columns["cQcProd"].Width = 130;
            this.gridView2.Columns["nMOQty"].Width = 80;
            this.gridView2.Columns["nStMoveQty"].Width = 80;
            this.gridView2.Columns["nQty"].Width = 80;
            this.gridView2.Columns["cQnUOM"].Width = 80;
            this.gridView2.Columns["cRemark1"].Width = 80;
            this.gridView2.Columns["cLot"].Width = 80;
            this.gridView2.Columns["cQcWHLoca"].Width = 80;
            this.gridView2.Columns["cToQcWHLoca"].Width = 80;
            this.gridView2.Columns["nPrice"].Width = 80;

            this.gridView2.Columns["nMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nMOQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView2.Columns["nStMoveQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nStMoveQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView2.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView2.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nPrice"].DisplayFormat.FormatString = "#,###,###.00";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW2_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW2_CPDTYPE";
            this.grcRemark.Buttons[0].Tag = "GRDVIEW2_CREMARK1";
            this.grcLot.Buttons[0].Tag = "GRDVIEW2_CLOT";
            this.grcQty.Buttons[0].Tag = "GRDVIEW2_NQTY";
            this.grcQcWHLoca.Buttons[0].Tag = "GRDVIEW2_CQCWHLOCA";
            //this.grcQcWHLoca.Buttons[0].Tag = "GRDVIEW2_CQCWHLOCA";

            this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");

            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["nMOQty"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.gridView2.Columns["nMOQty"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nMOQty"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["nStMoveQty"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.gridView2.Columns["nStMoveQty"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nStMoveQty"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView2.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView2.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark;
            this.gridView2.Columns["cLot"].ColumnEdit = this.grcLot;
            this.gridView2.Columns["cQcWHLoca"].ColumnEdit = this.grcQcWHLoca;
            this.gridView2.Columns["cToQcWHLoca"].ColumnEdit = this.grcQcWHLoca;

            this.gridView2.Columns["nMOQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView2.Columns["nMOQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView2.Columns["nStMoveQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView2.Columns["nStMoveQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView2.Columns["nQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView2.Columns["nQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.pmCalcColWidth2();
        }

        private void pmInitGridProp_TemStore()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPack].DefaultView;

            this.grdTemStore.DataSource = this.dtsDataEnv.Tables[this.mstrTemPack];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
                this.gridView3.Columns[intCnt].OptionsColumn.AllowFocus = true;
                this.gridView3.Columns[intCnt].OptionsColumn.ReadOnly = true;
            }

            int i = 0;

            this.gridView3.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView3.Columns["cRefPdType"].VisibleIndex = i++;
            //this.gridView3.Columns["cPdType"].VisibleIndex = i++;
            this.gridView3.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView3.Columns["cQnProd"].VisibleIndex = i++;
            //this.gridView3.Columns["cLot"].VisibleIndex = i++;
            this.gridView3.Columns["cQcWHLoca"].VisibleIndex = i++;

            this.gridView3.Columns["nQty"].VisibleIndex = i++;
            this.gridView3.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView3.Columns["cShowPack"].VisibleIndex = i++;
            
            this.gridView3.Columns["nRecNo"].Caption = "No.";
            //this.gridView3.Columns["cRefPdType"].Caption = "P/F";
            //this.gridView3.Columns["cPdType"].Caption = "T";
            this.gridView3.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView3.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            //this.gridView3.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView3.Columns["cLot"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ล๊อต", "Lot" });
            this.gridView3.Columns["cQcWHLoca"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ที่เก็บ", "Location" });
            //this.gridView3.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#MO Qty.", "#MO Qty." });

            this.gridView3.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView3.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });

            this.gridView3.Columns["cShowPack"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "Pack", "Pack" });
            //this.gridView3.Columns["nPrice"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ราคาทุน", "Cost" });

            this.gridView3.Columns["nRecNo"].Width = 40;
            //this.gridView3.Columns["cRefPdType"].Width = 30;
            //this.gridView3.Columns["cPdType"].Width = 50;
            this.gridView3.Columns["cQcProd"].Width = 130;
            this.gridView3.Columns["nQty"].Width = 80;
            this.gridView3.Columns["cQnUOM"].Width = 80;
            this.gridView3.Columns["cLot"].Width = 80;
            this.gridView3.Columns["cQcWHLoca"].Width = 80;

            this.gridView3.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView3.Columns["cShowPack"].ColumnEdit = this.grcShowPack;

            this.grcShowPack.Buttons[0].Tag = "GRDVIEW3_CSHOWPACK";

            //this.pmCalcColWidth2();
        }

        private void pmCalcColWidth1()
        {

            int intColWidth = this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Width
                                    + this.gridView1.Columns["QCFRWHOUSE"].Width
                                    + this.gridView1.Columns["QCTOWHOUSE"].Width
                                    + this.gridView1.Columns["QCSECT"].Width
                                    + this.gridView1.Columns["QCJOB"].Width;

            //int intNewWidth = this.Width - intColWidth - 60;
            //this.gridView2.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView2.Columns["nRecNo"].Width
                                    //+ this.gridView2.Columns["cRefPdType"].Width
                                    + this.gridView2.Columns["cPdType"].Width
                                    + this.gridView2.Columns["cQcProd"].Width
                                    + this.gridView2.Columns["cRemark1"].Width
                                    + this.gridView2.Columns["cLot"].Width
                                    + this.gridView2.Columns["nQty"].Width
                                    + this.gridView2.Columns["nPrice"].Width
                                    + this.gridView2.Columns["cQnUOM"].Width;

            if (this.mDocType != DocumentType.TR)
            {
                intColWidth += this.gridView2.Columns["nMOQty"].Width + this.gridView2.Columns["nStMoveQty"].Width;
            }

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView2.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFStmoveHDInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].VisibleIndex = i++;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].VisibleIndex = i++;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].VisibleIndex = i++;
            this.gridView1.Columns["QCFRWHOUSE"].VisibleIndex = i++;
            this.gridView1.Columns["QCTOWHOUSE"].VisibleIndex = i++;
            this.gridView1.Columns["QCSECT"].VisibleIndex = i++;
            this.gridView1.Columns["QCJOB"].VisibleIndex = i++;
            this.gridView1.Columns["CREMARK"].VisibleIndex = i++;
            //this.gridView1.Columns["CREFTO_REFNO"].VisibleIndex = i++;
            //this.gridView1.Columns["CREFTO_OP"].VisibleIndex = i++;

            this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Width = 30;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Width = 90;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Width = 100;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Width = 80;
            this.gridView1.Columns["QCFRWHOUSE"].Width = 80;
            this.gridView1.Columns["QCTOWHOUSE"].Width = 80;
            this.gridView1.Columns["QCSECT"].Width = 50;
            this.gridView1.Columns["QCJOB"].Width = 50;
            this.gridView1.Columns["CREMARK"].Width = 100;
            this.gridView1.Columns["CREFTO_REFNO"].Width = 125;
            this.gridView1.Columns["CREFTO_OP"].Width = 80;

            this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Caption = "C";
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Caption = UIBase.GetAppUIText(new string[] {"เลขที่ภายใน","DOC. CODE"});
            this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            //this.gridView1.Columns["REFNO_WO"].Caption = UIBase.GetAppUIText(new string[] { "#MO", "#MO" });
            //this.gridView1.Columns[QMFStmoveHDInfo.Field.FrOPSeq].Caption = UIBase.GetAppUIText(new string[] { "OP Seq.", "OP Seq." });
            //this.gridView1.Columns["QNFRMOPR"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อ OP", "OP Name" });
            this.gridView1.Columns["QCFRWHOUSE"].Caption = UIBase.GetAppUIText(new string[] { "จากคลัง", "From Warehouse" });
            this.gridView1.Columns["QCTOWHOUSE"].Caption = UIBase.GetAppUIText(new string[] { "เข้าคลัง", "To Warehouse" });
            this.gridView1.Columns["QCSECT"].Caption = UIBase.GetAppUIText(new string[] { "แผนก", "Section" });
            this.gridView1.Columns["QCJOB"].Caption = UIBase.GetAppUIText(new string[] { "โครงการ", "Job" });
            this.gridView1.Columns["CREMARK"].Caption = UIBase.GetAppUIText(new string[] { "หมายเหตุ", "Remark" });
            this.gridView1.Columns["CREFTO_REFNO"].Caption = UIBase.GetAppUIText(new string[] { "#MO", "#MO" });
            this.gridView1.Columns["CREFTO_OP"].Caption = UIBase.GetAppUIText(new string[] { "OP. Seq", "OP. Seq" }); ;

            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].DisplayFormat.FormatString = "dd/MM/yy";

            this.pmSetSortKey(QMFStmoveHDInfo.Field.Code, true);
        
        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void grdTemPd_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth2();
        }

        private void gridView2_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth2();
        }

        private void gridView2_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemPd;
            this.mActiveGrid = this.gridView2;
            //this.gridView2.FocusedColumn = this.gridView2.Columns["cOPSeq"];
        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.RefreshData();
            }

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count;intCnt++ )
            {
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
            }
            this.mstrSortKey = inColumn.ToUpper();
            this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
            this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
        }

        private void pgfMainEdit_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            this.pmSetToolbarState(pgfMainEdit.SelectedTabPageIndex);

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                if (pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
                {
                    this.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.Width - 20, AppUtil.CommonHelper.SysMetric(9) + 5);
                }
                else 
                {
                    this.Location = new Point(Convert.ToInt16((AppUtil.CommonHelper.SysMetric(1) - this.Width) / 2), Convert.ToInt16((AppUtil.CommonHelper.SysMetric(2) - this.Height) / 2));
                }
            }

        }

        private void pgfEditItem_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (pgfEditItem.SelectedTabPageIndex == xd_PAGE_BROWSE)
            {
                //this.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.Width - 20, AppUtil.CommonHelper.SysMetric(9) + 5);
            }
            else
            {
                //Reload TemPack
                //EnumerableRowCollection<DataRow> queryYear = from p in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                //                group p by new{p.Field<string>("QcProd"),p.Field<string>("cLot")} into g
                //                select g;

                foreach (DataRow dtrTemPk1 in this.dtsDataEnv.Tables[this.mstrTemPack].Rows)
                {
                    dtrTemPk1["nQty"] = 0;
                }

                foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
                {
                    //ให้ช้ามLot ได้
                    //DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPack].Select("cQcProd = '" + dtrTemPd["cQcProd"].ToString() + "' and cLot = '" + dtrTemPd["cLot"].ToString() + "' and cQcWHLoca = '" + dtrTemPd["cQcWHLoca"].ToString() + "'");
                    DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPack].Select("cQcProd = '" + dtrTemPd["cQcProd"].ToString() + "' and cQcWHLoca = '" + dtrTemPd["cQcWHLoca"].ToString() + "'");
                    if (da.Length == 0)
                    {
                        DataRow dtrTemPack = this.dtsDataEnv.Tables[this.mstrTemPack].NewRow();
                        dtrTemPack["cProd"] = dtrTemPd["cProd"].ToString();
                        dtrTemPack["cQcProd"] = dtrTemPd["cQcProd"].ToString();
                        dtrTemPack["cQnProd"] = dtrTemPd["cQnProd"].ToString();
                        dtrTemPack["cQnUOM"] = dtrTemPd["cQnUOM"].ToString();
                        dtrTemPack["cLot"] = dtrTemPd["cLot"].ToString();
                        dtrTemPack["cWHLoca"] = dtrTemPd["cWHLoca"].ToString();
                        dtrTemPack["cQcWHLoca"] = dtrTemPd["cQcWHLoca"].ToString();
                        dtrTemPack["nQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                        this.dtsDataEnv.Tables[this.mstrTemPack].Rows.Add(dtrTemPack);
                    }
                    else
                    {
                        da[0]["nQty"] = Convert.ToDecimal(da[0]["nQty"]) + Convert.ToDecimal(dtrTemPd["nQty"]);
                    }
                }
            }

        }

        private void pmSetToolbarState(int inActivePage)
        {
            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = (this.mFormActiveMode == FormActiveMode.PopUp ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);

            if (this.mFormActiveMode == FormActiveMode.Report)
            {
                this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Update.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            this.barMainEdit.Items[WsToolBar.Close.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMainEdit.Items[WsToolBar.Print.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Cancel.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Print.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            //this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);

            this.barMainEdit.Items["barPrnBarcode"].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items["barImpXLS1"].Enabled = (inActivePage == 0 ? false : true);
            
            //this.barMainEdit.Items["barPrnBarcode"].Enabled = (inActivePage == 0 ? true : false);
            //barOnKB
        }

        private void pmFilterForm()
        {
            this.pmInitPopUpDialog("FILTER");
        }

        private void pmGotoBrowPage()
        {
            this.pmBlankFormData();
            this.pmRefreshBrowView();

            this.pmSetPageStatus(false);
            this.pgfMainEdit.TabPages[0].PageEnabled = true;
            this.pgfMainEdit.SelectedTabPageIndex = 0;
            this.grdBrowView.Focus();
        }

        private void pmRefreshBrowView()
        {
            this.mintSaveBrowViewRowIndex = this.gridView1.FocusedRowHandle;
            this.pmSetBrowView();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            if (this.gridView1.RowCount > this.mintSaveBrowViewRowIndex)
                this.gridView1.FocusedRowHandle = this.mintSaveBrowViewRowIndex;

        }

        private void pmSetPageStatus(bool inIsEnable)
        {
            for (int intCnt = 0; intCnt < this.pgfMainEdit.TabPages.Count; intCnt++)
            {
                this.pgfMainEdit.TabPages[intCnt].PageEnabled = inIsEnable;
            }
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

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            DataRow dtrGetVal = null;
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Transaction.Common.dlgFilterOption1 dlgFilter = new Transaction.Common.dlgFilterOption1(this.mstrRefType))
                    {
                        if (this.mstrBranch != string.Empty)
                            dlgFilter.BranchID = this.mstrBranch;
                        if (this.mstrBook != string.Empty)
                            dlgFilter.BookID = this.mstrBook;

                        dlgFilter.BeginDate = this.mdttBegDate;
                        dlgFilter.EndDate = this.mdttEndDate;

                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;
                            this.mstrBranch = dlgFilter.BranchID;
                            this.mstrBook = dlgFilter.BookID;
                            this.mstrQcBook = dlgFilter.QcBook;
                            this.mdttBegDate = dlgFilter.BeginDate.Date;
                            this.mdttEndDate = dlgFilter.EndDate.Date;

                            this.mstrDefaSect = App.ActiveCorp.DefaultSectID;
                            this.mstrDefaJob = App.ActiveCorp.DefaultJobID;

                            //this.lblTitle.Text = this.mstrRefTypeName.TrimEnd();

                            //this.mstrRefTypeName = dlgFilter.RefTypeName;
                            pobjSQLUtil.SetPara(new object[1] { this.mstrBranch });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select * from Branch where fcSkid = ?", ref strErrorMsg))
                            {
                                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                                //string strWHField = (this.mstrSaleOrBuy == "S" ? "fcWHSale" : "fcWHBuy");
                                string strWHField = "fcWHSale";
                                this.mstrDefaWHouse = dtrBranch[strWHField].ToString();
                                this.mdttLastClose = (!Convert.IsDBNull(dtrBranch["fdLastClos"]) ? Convert.ToDateTime(dtrBranch["fdLastClos"]) : DBEnum.NullDate);

                                pobjSQLUtil.SetPara(new object[1] { this.mstrBook });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select * from Book where fcSkid = ?", ref strErrorMsg))
                                {
                                    DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];

                                    this.mstrBookRunCodeType = dtrBook["fcDocRunFM"].ToString().TrimEnd();
                                    this.mstrRunCodeStyle = dtrBook["fcDocRunFM"].ToString().TrimEnd();

                                    if (dtrBook["fcPrefix"].ToString().Trim() != string.Empty)
                                    {
                                        this.mstrBookPrefix = dtrBook["fcPrefix"].ToString().TrimEnd();
                                        this.mintBookRunCodeLen = (Convert.ToInt32(dtrBook["fnRunLen"]) == 0 ? 7 : Convert.ToInt32(dtrBook["fnRunLen"]));
                                    }
                                    else
                                    {
                                        this.mstrBookPrefix = this.mstrRefType + this.mstrQcBook + "/";
                                        this.mintBookRunCodeLen = 7;
                                    }

                                }
                            }

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();
                        }
                    }
                    break;
                case "REFTO":

                    switch (this.mDocType)
                    {
                        case DocumentType.FR:

                            using (Common.dlgGetRefToPO dlgRefTo = new Common.dlgGetRefToPO("PO", this.mstrBranch, this.txtQcCoor.Tag.ToString(), ""))
                            {

                                dlgRefTo.BindData(this.dtsDataEnv.Tables[this.mstrTemRefTo]);
                                dlgRefTo.ShowDialog();
                                if (dlgRefTo.DialogResult == DialogResult.OK)
                                {
                                    this.pmLoadRefToOrder(dlgRefTo.RefTable);
                                    this.pmRecalTotPd();
                                }


                            }

                            break;
                        case DocumentType.GD:

                            using (Common.dlgGetRefToStm dlgRefTo = new Common.dlgGetRefToStm("Q1", this.mstrBranch, this.txtQcCoor.Tag.ToString(), ""))
                            {

                                dlgRefTo.BindData(this.dtsDataEnv.Tables[this.mstrTemRefTo]);
                                dlgRefTo.ShowDialog();
                                if (dlgRefTo.DialogResult == DialogResult.OK)
                                {
                                    this.pmLoadRefToOrder2(dlgRefTo.RefTable);
                                    this.pmRecalTotPd();
                                }


                            }

                            break;
                    }
                    break;
                case "IMP_XLS1":
                    using (Common.dlgGetXLS2 dlgRefTo = new Common.dlgGetXLS2())
                    {
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            pmLoadRefToOrderXML("PO", dlgRefTo.Load_DocNo, dlgRefTo.dtsLoadXML);
                        }
                    }

                    //using (Common.dlgGetXLS1 dlgRefTo = new Common.dlgGetXLS1())
                    //{
                    //    dlgRefTo.ShowDialog();
                    //    if (dlgRefTo.DialogResult == DialogResult.OK)
                    //    {
                    //        foreach (string strCode in dlgRefTo.lstCode.Lines)
                    //        {
                    //            //MessageBox.Show(strCode);
                    //            this.pmImport_XLS1(strCode);
                    //        }
                    //        //this.pmLoadRefToOrder(dlgRefTo.RefTable);
                    //        this.pmRecalTotPd();
                    //    }
                    //}

                    break;
                case "REFTOXXX":
                    using (Common.MRP.dlgGetRefToWO dlgRefTo = new Common.MRP.dlgGetRefToWO(this.mstrRefToTab, this.mstrRefToRefType, this.mstrBranch, this.mstrPlant, this.mstrRefToBook, this.mstrRefToRowID))
                    {
                        dlgRefTo.IsGetPlant = true;
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            if (dlgRefTo.RefToDocumentID != string.Empty)
                            {
                                if (this.mstrRefToRowID != dlgRefTo.RefToDocumentID)
                                {

                                    this.pmClearRefTo();
                                    this.mstrRefToBook = dlgRefTo.RefToBookID;
                                    this.mstrRefToRowID = dlgRefTo.RefToDocumentID;
                                    this.txtRefTo.Text = dlgRefTo.RefToDocumentCode.TrimEnd();
                                    this.mstrPlant = dlgRefTo.RefToPlantID;
                                    //this.txtQcSect.Tag=dlgRefTo.Ref

                                    //this.pmLoadOPSeq();

                                    pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRowID });
                                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderH3", "WORDERH", "select * from " + QMFWOrderHDInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                                    {

                                        DataRow dtrWOrderH = this.dtsDataEnv.Tables["QWOrderH3"].Rows[0];

                                        this.txtQcSect.Tag = dtrWOrderH["cSect"].ToString();
                                        pobjSQLUtil.SetPara(new object[1] { this.txtQcSect.Tag });
                                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from Sect where fcSkid = ?", ref strErrorMsg))
                                        {
                                            this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                                        }

                                        this.txtQcJob.Tag = dtrWOrderH["cJob"].ToString();
                                        pobjSQLUtil.SetPara(new object[1] { this.txtQcJob.Tag });
                                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from Job where fcSkid = ?", ref strErrorMsg))
                                        {
                                            this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                                        }

                                    }

                                    using (Common.MRP.dlgGetRefToRoute dlg = new Common.MRP.dlgGetRefToRoute())
                                    {
                                        this.pmLoadFormOPSeq();
                                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                                        dlg.BindData(this.dtsDataEnv);
                                        dlg.ShowDialog();
                                        if (dlg.DialogResult == DialogResult.OK
                                            && dlg.SelectedRow() > -1)
                                        {
                                            this.pmRetrieveOPSeqVal(dlg.SelectedRow());
                                        }

                                        this.txtFrQcWHouse.Focus();

                                    }
                                }
                            }
                            else
                            {
                                this.pmClearRefTo();
                            }
                        }
                    }
                    break;
                case "GET_OPSEQ":
                    using (Common.MRP.dlgGetRefToRoute dlg = new Common.MRP.dlgGetRefToRoute())
                    {
                        this.pmLoadFormOPSeq();
                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                        dlg.BindData(this.dtsDataEnv);
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK
                            && dlg.SelectedRow() > -1)
                        {
                            this.pmRetrieveOPSeqVal(dlg.SelectedRow());
                        }

                        this.txtFrQcWHouse.Focus();

                    }
                    break;
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        //this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd = new DatabaseForms.frmProd(FormActiveMode.PopUp);
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PDTYPE":
                    if (this.pofrmGetProdType == null)
                    {
                        this.pofrmGetProdType = new DialogForms.dlgGetProdType();
                        this.pofrmGetProdType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProdType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WKCTR":
                    if (this.pofrmGetWkCtr == null)
                    {
                        this.pofrmGetWkCtr = new frmMFWorkCenter(FormActiveMode.PopUp);
                        this.pofrmGetWkCtr.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWkCtr.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "GET_OPSEQ2":

                    if (this.pofrmGetOPSeq == null)
                    {
                        this.pofrmGetOPSeq = new Transaction.Common.MRP.dlgGetRefToRoute();
                        this.pofrmGetOPSeq.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOPSeq.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }

                    this.pmLoadFormOPSeq();
                    this.pofrmGetOPSeq.BindData(this.dtsDataEnv);
                    
                    break;
                case "STDOP":
                    if (this.pofrmGetStdOP == null)
                    {
                        this.pofrmGetStdOP = new frmMFStdOpr(FormActiveMode.PopUp);
                        this.pofrmGetStdOP.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetStdOP.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "UM":
                    if (this.pofrmGetUM == null)
                    {
                        this.pofrmGetUM = new DialogForms.dlgGetUM();
                        this.pofrmGetUM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetUM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "UOMCONVERT":
                    using (DatabaseForms.Common.dlgUOMConvert dlg = new DatabaseForms.Common.dlgUOMConvert())
                    {
                        //dlg.InitForm(this.txtQnStdUM.Text.TrimEnd(), this.txtQnUM.Text.ToString());
                        //dlg.UOMQty = this.mdecUMQty;
                        //dlg.ShowDialog();
                        //if (dlg.DialogResult == DialogResult.OK)
                        //{
                        //    this.mdecUMQty = dlg.UOMQty;
                        //    this.pmSetUMQtyMsg();
                        //}
                    }
                    break;
                case "WHOUSE":
                    if (this.pofrmGetWareHouse == null)
                    {
                        this.pofrmGetWareHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWareHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWareHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "SHOW_PACK":    
                    DataRow dtrTemPack = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);
                    using (Transaction.Common.dlgShowPack dlg = new Transaction.Common.dlgShowPack())
                    {
                        dlg.BindData(dtrTemPack["cQcProd"].ToString(), this.dtsDataEnv, this.dtsDataEnv.Tables[this.mstrTemRefTo_Pack]);
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                        }
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

                        //dlg.CheckUM = "FNUMQTY1"; //หน่วยซื้อส่วยใหญ่
                        dlg.CheckUM = "FNUMQTY2"; //หน่วยขายส่วยใหญ่
                        dlg.ProdID = dtrTemPd["cProd"].ToString();
                        dlg.Row = dtrTemPd;
                        dlg.SetParentForm(this);

                        dlg.Qty = decQty;
                        dlg.UMQty = decUOMQty;
                        dlg.UM = strUOM;
                        dlg.StdUM = strUOMStd;
                        dlg.StQty = decStkQty;
                        dlg.StUMQty = decUOMQty;
                        dlg.StUM = strUOM;
                        dlg.StStdUM = strUOMStd;
                        //กรณีลดหนี้/เพิ่มหนี้ที่เลือก Option ไม่ลด/เพิ่มจำนวน
                        //dlg.IsFixUMQty = !this.mbllIsCrDrChgQty;

                        strErrorMsg = "";
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
                            //dtrTemPd["nPrice"] = (Convert.IsDBNull(dtrTemPd["nPrice"]) ? this.pmGetPrice(dtrTemPd["cProd"].ToString(), this.txtQcCoor.Tag.ToString(), dlg.Qty, dlg.UMQty) : dtrTemPd["nPrice"]);
                            dtrTemPd["nLastPrice"] = Convert.ToDecimal(dtrTemPd["nPrice"]);

                            decQty = dlg.Qty;
                            decimal decAmt = decQty * Convert.ToDecimal(dtrTemPd["nPrice"]);
                            decimal decDiscAmt = BizRule.CalDiscAmtFromDiscStr(dtrTemPd["cDiscStr"].ToString(), decAmt, decQty, 0);
                            decimal decPrice = BizRule.LimitPrcCost(Convert.ToDecimal(dtrTemPd["nPrice"]));
                            dtrTemPd["nDiscAmt"] = decDiscAmt;
                            //dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);

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

                            //this.gridView2.SetFocusedRowCellValue(this.gridView2.Columns["nQty"], decQty);
                            //this.gridView2.RefreshRowCell(this.gridView2.FocusedRowHandle, this.gridView2.Columns["nQty"]);
                            this.mdecCurrQty = decQty;
                        }
                        else
                        {
                            dtrTemPd["nQty"] = Convert.ToDecimal(dtrTemPd["nLastQty"]);
                            this.mdecCurrQty = Convert.ToDecimal(dtrTemPd["nLastQty"]);
                            return false;
                        }
                    }
                    break;
                
                case "LOT_ITEM":
                    using (Transaction.Common.dlgGetLotWHouse2 dlg = new Transaction.Common.dlgGetLotWHouse2())
                    {
                        DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                        if (dtrTemPd == null)
                        {
                            dtrTemPd = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                            this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrTemPd);
                        }

                        int intActRow = Convert.ToInt32(dtrTemPd["nActRow"]);

                        dlg.SetParentForm(this);

                        dlg.ProdID = dtrTemPd["cProd"].ToString();
                        //dlg.IsGetPdSer = false;
                        dlg.IsGetPdSer = this.mbllIsGetPdSer;
                        dlg.SaleOrBuy = this.mstrSaleOrBuyForPdSer;

                        string strWHouse = this.txtFrQcWHouse.Tag.ToString();
                        string strWHLoca = "";

                        switch (this.mDocType)
                        {
                            case DocumentType.WR:
                            case DocumentType.WX:
                                strWHouse = this.txtFrQcWHouse.Tag.ToString();
                                break;
                            case DocumentType.RW:
                            case DocumentType.RX:
                                strWHouse = this.txtToQcWHouse.Tag.ToString();
                                break;
                            case DocumentType.FR:
                                strWHouse = this.txtToQcWHouse.Tag.ToString();
                                break;
                            case DocumentType.TR:
                                strWHouse = this.txtFrQcWHouse.Tag.ToString();
                                break;
                        } 

                        //dlg.BindData(this.dtsDataEnv, intActRow, this.mstrBranch, strWHouse, dtrTemPd["cLot"].ToString());
                        dlg.BindData(this.dtsDataEnv, this.mPdSer, intActRow, this.mstrBranch, strWHouse, strWHLoca, dtrTemPd["cLot"].ToString());
                        dlg.txtExpDate.Properties.MinValue = this.txtDate.DateTime;

                        dlg.txtAgeLong.Value = Convert.ToDecimal(dtrTemPd["fnAgeLong"]);
                        if (Convert.IsDBNull(dtrTemPd["fdExpire"]))
                        {
                            dlg.txtExpDate.EditValue = null;
                        }
                        else
                        {
                            dlg.txtExpDate.DateTime = Convert.ToDateTime(dtrTemPd["fdExpire"]);
                        }

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            dtrTemPd["cLot"] = dlg.Lot;

                            dtrTemPd["fnAgeLong"] = Convert.ToDecimal(dlg.txtAgeLong.Value);
                            dtrTemPd["fdExpire"] = dlg.txtExpDate.DateTime;

                            this.mActiveGrid.UpdateCurrentRow();
                        }
                    }
                    break;
                case "REMARK_H":
                    using (Transaction.Common.dlgGetRemark dlg = new Transaction.Common.dlgGetRemark())
                    {

                        dlg.Desc1 = this.txtRemark.Text.TrimEnd();
                        dlg.Desc2 = this.pARemark[0];
                        dlg.Desc3 = this.pARemark[1];
                        dlg.Desc4 = this.pARemark[2];
                        dlg.Desc5 = this.pARemark[3];
                        dlg.Desc6 = this.pARemark[4];
                        dlg.Desc7 = this.pARemark[5];
                        dlg.Desc8 = this.pARemark[6];
                        dlg.Desc9 = this.pARemark[7];
                        dlg.Desc10 = this.pARemark[8];

                        dlg.SetLabelText(new string[] {
                            UIBase.GetAppUIText(new string[] {"หมายเหตุ 1 :","Remark 1 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 2 :","Remark 2 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 3 :","Remark 3 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 4 :","Remark 4 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 5 :","Remark 5 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 6 :","Remark 6 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 7 :","Remark 7 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 8 :","Remark 8 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 9 :","Remark 9 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 10 :","Remark 10 :"}) });

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {

                            this.txtRemark.Text = dlg.Desc1;
                            this.pARemark[0] = dlg.Desc2;
                            this.pARemark[1] = dlg.Desc3;
                            this.pARemark[2] = dlg.Desc4;
                            this.pARemark[3] = dlg.Desc5;
                            this.pARemark[4] = dlg.Desc6;
                            this.pARemark[5] = dlg.Desc7;
                            this.pARemark[6] = dlg.Desc8;
                            this.pARemark[7] = dlg.Desc9;
                            this.pARemark[8] = dlg.Desc10;

                            //this.mActiveGrid.UpdateCurrentRow();

                        }
                    }
                    break;
                
                case "REMARK":
                    using (Transaction.Common.dlgGetRemark dlg = new Transaction.Common.dlgGetRemark())
                    {
                        //dlg.DescReadOnly = true;
                        //DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                        DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                        if (dtrTemPd == null)
                        {
                            dtrTemPd = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                            this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrTemPd);
                        }

                        dlg.Desc1 = dtrTemPd["cRemark1"].ToString().TrimEnd();
                        dlg.Desc2 = dtrTemPd["cRemark2"].ToString().TrimEnd();
                        dlg.Desc3 = dtrTemPd["cRemark3"].ToString().TrimEnd();
                        dlg.Desc4 = dtrTemPd["cRemark4"].ToString().TrimEnd();
                        dlg.Desc5 = dtrTemPd["cRemark5"].ToString().TrimEnd();
                        dlg.Desc6 = dtrTemPd["cRemark6"].ToString().TrimEnd();
                        dlg.Desc7 = dtrTemPd["cRemark7"].ToString().TrimEnd();
                        dlg.Desc8 = dtrTemPd["cRemark8"].ToString().TrimEnd();
                        dlg.Desc9 = dtrTemPd["cRemark9"].ToString().TrimEnd();
                        dlg.Desc10 = dtrTemPd["cRemark10"].ToString().TrimEnd();

                        dlg.SetLabelText(new string[] {
                            UIBase.GetAppUIText(new string[] {"หมายเหตุ 1 :","Remark 1 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 2 :","Remark 2 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 3 :","Remark 3 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 4 :","Remark 4 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 5 :","Remark 5 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 6 :","Remark 6 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 7 :","Remark 7 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 8 :","Remark 8 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 9 :","Remark 9 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ 10 :","Remark 10 :"}) });

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            dtrTemPd["cRemark1"] = dlg.Desc1;
                            dtrTemPd["cRemark2"] = dlg.Desc2;
                            dtrTemPd["cRemark3"] = dlg.Desc3;
                            dtrTemPd["cRemark4"] = dlg.Desc4;
                            dtrTemPd["cRemark5"] = dlg.Desc5;
                            dtrTemPd["cRemark6"] = dlg.Desc6;
                            dtrTemPd["cRemark7"] = dlg.Desc7;
                            dtrTemPd["cRemark8"] = dlg.Desc8;
                            dtrTemPd["cRemark9"] = dlg.Desc9;
                            dtrTemPd["cRemark10"] = dlg.Desc10;

                            this.mActiveGrid.UpdateCurrentRow();

                        }
                    }
                    break;

                case "CUST":
                    if (this.pofrmGetCust == null)
                    {
                        this.pofrmGetCust = new DatabaseForms.frmCust(FormActiveMode.PopUp);
                        this.pofrmGetCust.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCust.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "SUPPL":
                    if (this.pofrmGetSuppl == null)
                    {
                        //this.pofrmGetSuppl = new DialogForms.dlgGetCoor(this.mCoorType);
                        this.pofrmGetSuppl = new DatabaseForms.frmEMSuppl(FormActiveMode.PopUp);
                        this.pofrmGetSuppl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSuppl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "SECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new DialogForms.dlgGetSect();
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "JOB":
                    if (this.pofrmGetJob == null)
                    {
                        this.pofrmGetJob = new DialogForms.dlgGetJob();
                        //this.pofrmGetJob.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetJob.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WHLOCA":
                    if (this.pofrmGetWHLoca == null)
                    {
                        this.pofrmGetWHLoca = new frmWHLocation(FormActiveMode.PopUp);
                        //this.pofrmGetWHLoca.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWHLoca.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
            return true;
        }

        private void pmRetrieveOPSeqVal(int inRow)
        {

            if (inRow <= this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Count)
            {

                DataRow dtrSelOP = this.dtsDataEnv.Tables[this.mstrTemRoute].Rows[inRow];

                if (this.mstrActiveOP == dtrSelOP["cFrOPSeq"].ToString().TrimEnd())
                {
                    return;
                }
                else
                {
                    this.mstrActiveOP = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                }

                this.txtFrQnOP.Tag = dtrSelOP["cFrMOPR"].ToString();
                this.txtFrOPSeq.Text = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                this.txtFrQnOP.Text = dtrSelOP["cQnFrMOPR"].ToString().TrimEnd();
                this.mstrFrWkCtrH = dtrSelOP["cFrWkCtr"].ToString();

                if (this.mstrRefToMOrderOP != dtrSelOP["cRowID"].ToString())
                {
                    this.mstrRefToMOrderOP = dtrSelOP["cRowID"].ToString();

                    switch (this.mDocType)
                    { 
                        case DocumentType.WR:
                        case DocumentType.WX:
                        case DocumentType.RW:
                        case DocumentType.RX:
                            this.pmLoadRMFormOPSeq();
                            break;
                        case DocumentType.FR:
                            this.pmLoadSemiFormOPSeq(dtrSelOP["cFrOPSeq"].ToString(), dtrSelOP["cToOPSeq"].ToString(), dtrSelOP["cToWkCtr"].ToString());
                            break;
                    }
                }
            }

        }

        private void pmClearRefTo()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.txtRefTo.Text = "";
            this.mstrPlant = "";

            this.txtFrQnOP.Tag = "";
            this.txtFrOPSeq.Text = "";
            this.txtFrQnOP.Text = "";
            this.mstrFrWkCtrH = "";

        }

        private void pmLoadFormOPSeq()
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            string strFld = "WORDEROP.CROWID, WORDEROP.COPSEQ, WORDEROP.CNEXTOP, WORDEROP.CWKCTRH, WKCTRH.CCODE as QcWkCtr , WKCTRH.CNAME as QnWkCtr";
            strFld += " , MFSTDOPR.CROWID as cFrMOPR , MFSTDOPR.CCODE as QcStdOP , MFSTDOPR.CNAME as QnStdOP ";
            strSQLStr = "select " + strFld + " from MFWORDERIT_STDOP WORDEROP ";
            strSQLStr += " left join MFWKCTRHD WKCTRH on WKCTRH.CROWID = WORDEROP.CWKCTRH ";
            strSQLStr += " left join MFSTDOPR on MFSTDOPR.CROWID = WORDEROP.CMOPR ";
            //strSQLStr += " where WORDEROP.CWORDERH = ? ";
            strSQLStr += " where WORDEROP.CWORDERH = ? ";
            switch (this.mstrRefType)
            {
                case "WR":
                    strSQLStr += " and WORDEROP.CSTEP <> '" + SysDef.gc_REF_OPSTEP_FINISH + "'";
                    break;
                case "RW":
                case "FR":
                    //strSQLStr += " and WORDEROP.CSTEP <> '" + SysDef.gc_REF_OPSTEP_FINISH + "'";
                    break;
            }

            strSQLStr += " order by WORDEROP.COPSEQ";

            string strSQLStr2 = "";
            string strFld2 = "WORDEROP.CROWID, WORDEROP.COPSEQ, WORDEROP.CNEXTOP, WORDEROP.CWKCTRH, WKCTRH.CCODE as QcWkCtr , WKCTRH.CNAME as QnWkCtr";
            strFld2 += " , MFSTDOPR.CROWID as cFrMOPR , MFSTDOPR.CCODE as QcStdOP , MFSTDOPR.CNAME as QnStdOP ";
            strSQLStr2 = "select " + strFld + " from MFWORDERIT_STDOP WORDEROP ";
            strSQLStr2 += " left join MFWKCTRHD WKCTRH on WKCTRH.CROWID = WORDEROP.CWKCTRH ";
            strSQLStr2 += " left join MFSTDOPR on MFSTDOPR.CROWID = WORDEROP.CMOPR ";
            //strSQLStr2 += " where WORDEROP.CWORDERH = ? ";
            strSQLStr2 += " where WORDEROP.CWORDERH = ? and WORDEROP.COPSEQ = ? and WORDEROP.CSTEP <> ? ";
            strSQLStr2 += " order by WORDEROP.COPSEQ";

            DataRow dtrLast = null;
            DataRow dtrTemRoute = null;

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq2", "WORDEROP", strSQLStr, ref strErrorMsg))
            {
                this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                foreach (DataRow dtrOPSeq in this.dtsDataEnv.Tables["QOPSeq2"].Rows)
                {
                    dtrTemRoute = this.dtsDataEnv.Tables[this.mstrTemRoute].NewRow();
                    dtrTemRoute["cRowID"] = dtrOPSeq["cRowID"].ToString();
                    dtrTemRoute["cFrOPSeq"] = dtrOPSeq["cOPSeq"].ToString();
                    dtrTemRoute["cFrWkCtr"] = dtrOPSeq["cWKCtrH"].ToString();
                    dtrTemRoute["cQcFrWkCtr"] = dtrOPSeq["QcWkCtr"].ToString();
                    dtrTemRoute["cQnFrWkCtr"] = dtrOPSeq["QnWkCtr"].ToString();
                    dtrTemRoute["cToOPSeq"] = dtrOPSeq["cNextOP"].ToString();

                    dtrTemRoute["cFrMOPR"] = dtrOPSeq["cFrMOPR"].ToString();
                    dtrTemRoute["cQcFrMOPR"] = dtrOPSeq["QcStdOP"].ToString();
                    dtrTemRoute["cQnFrMOPR"] = dtrOPSeq["QnStdOP"].ToString();

                    pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, dtrOPSeq["cNextOP"].ToString(), SysDef.gc_REF_OPSTEP_FINISH });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QNextOP", "WORDEROP", strSQLStr2, ref strErrorMsg))
                    {
                        DataRow dtrNextOP = this.dtsDataEnv.Tables["QNextOP"].Rows[0];
                        dtrTemRoute["cToMOPR"] = dtrNextOP["cFrMOPR"].ToString();
                        dtrTemRoute["cQcToMOPR"] = dtrNextOP["QcStdOP"].ToString();
                        dtrTemRoute["cQnToMOPR"] = dtrNextOP["QnStdOP"].ToString();

                        dtrTemRoute["cToWkCtr"] = dtrNextOP["cWKCtrH"].ToString();
                        dtrTemRoute["cQcToWkCtr"] = dtrNextOP["QcWkCtr"].ToString();
                        dtrTemRoute["cQnToWkCtr"] = dtrNextOP["QnWkCtr"].ToString();

                    }

                    this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Add(dtrTemRoute);
                }
            }
        }

        private void pmLoadSemiFormOPSeq(string inFrOPSeq1, string inToOPSeq1, string inToOPSeq)
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from MFWORDERIT_PD WORDERI ";
            strSQLStr += " where WORDERI.CWORDERH = ? ";
            strSQLStr += " and WORDERI.COPSEQ = ? ";
            if (inFrOPSeq1.Trim() == inToOPSeq1.Trim())
            {
                //pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
                //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWOrderH3", "WORDERH", "select * from " + QMFWOrderHDInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                //{

                //    DataRow dtrWOrderH = this.dtsDataEnv.Tables["QWOrderH3"].Rows[0];
                //    dtrWOrderI = this.dtsDataEnv.Tables["QOPSeq3"].NewRow();

                //    pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", "select * from MFWORDERIT_PD WORDERI where 0=1", ref strErrorMsg);
                //    DataRow dtrWOrderI = this.dtsDataEnv.Tables["QOPSeq3"].NewRow();
                //    dtrWOrderI["cProd"] = dtrWOrderH[QMFWOrderHDInfo.Field.MfgProdID];
                //    this.pmRepl1RecTemWcTranI2(this.mstrTemPd, 0, dtrWOrderI);
                //}

                strSQLStr = "";
                strSQLStr = "select * from MFWORDERIT_PD WORDERI ";
                strSQLStr += " where WORDERI.CWORDERH = ? ";
                strSQLStr += " and WORDERI.CIOTYPE = 'I' ";

                pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", strSQLStr, ref strErrorMsg))
                {
                    //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                    this.pmClearTemToBlank2();
                    int intRecNo = 0;
                    foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
                    {
                        this.pmRepl1RecTemWcTranI2(this.mstrTemPd, intRecNo++, dtrWOrderI);
                    }
                }

            }
            else
            {
                strSQLStr += " and WORDERI.CPRODTYPE in ( '1', '5') ";
            }
            strSQLStr += " order by WORDERI.COPSEQ, WORDERI.CSEQ";

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, inToOPSeq });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", strSQLStr, ref strErrorMsg))
            {
                //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                this.pmClearTemToBlank2();
                int intRecNo = 0;
                foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
                {
                    this.pmRepl1RecTemWcTranI2(this.mstrTemPd, intRecNo++, dtrWOrderI);
                }
            }

        }

        private void pmLoadRMFormOPSeq()
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from MFWORDERIT_PD WORDERI ";
            strSQLStr += " where WORDERI.CWORDERH = ? ";
            strSQLStr += " and WORDERI.COPSEQ = ? ";
            //strSQLStr += " and WORDERI.CPRODTYPE in ( '1', '5') ";
            strSQLStr += " order by WORDERI.COPSEQ, WORDERI.CSEQ";

            using (Common.MRP.dlgGetMO_Items dlg = new Common.MRP.dlgGetMO_Items())
            {

                string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                string strProdTab = strFMDBName + ".dbo.PROD";
                string strUMTab = strFMDBName + ".dbo.UM";

                string strSQLStr2 = "select WORDERI.CROWID , PROD.FCCODE as QCPROD , PROD.FCNAME as QNPROD, WORDERI.NQTY , UM.FCNAME as QNUM from MFWORDERIT_PD WORDERI";
                strSQLStr2 += " left join " + strProdTab + " PROD on PROD.FCSKID = WORDERI.CPROD ";
                strSQLStr2 += " left join " + strUMTab + " UM on UM.FCSKID = WORDERI.CUOM";
                strSQLStr2 += " where WORDERI.CWORDERH = ? ";
                strSQLStr2 += " and WORDERI.COPSEQ = ? ";
                strSQLStr2 += " order by WORDERI.COPSEQ, WORDERI.CSEQ";

                dlg.SetBrowView(strSQLStr2, new object[] { this.mstrRefToRowID, this.txtFrOPSeq.Text.TrimEnd() }, QMFWOrderHDInfo.TableName, 20, 100);
                dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                //dlg.BindData(this.dtsDataEnv);
                dlg.ShowDialog();
                if (dlg.PopUpResult)
                {
                    dlg.LoadTagValue(ref pATagCode);
                    string strList = "(" + this.pmGetRngCode() + ")";

                    pobjSQLUtil.NotUpperSQLExecString = true;
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", "select * from MFWORDERIT_PD WORDERI where CROWID in " + strList, ref strErrorMsg))
                    {
                        //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                        this.pmClearTemToBlank2();
                        //int intRecNo = this.gridView2.RowCount;
                        int intRecNo = 0;
                        foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
                        {
                            this.pmRepl1RecTemWcTranI2(this.mstrTemPd, intRecNo++, dtrWOrderI);
                        }
                    }
                
                }

            }

            //pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, this.txtFrOPSeq.Text.TrimEnd() });
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", strSQLStr, ref strErrorMsg))
            //{
            //    //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
            //    this.pmClearTemToBlank2();
            //    //int intRecNo = this.gridView2.RowCount;
            //    int intRecNo = 0;
            //    foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
            //    {
            //        this.pmRepl1RecTemWcTranI2(this.mstrTemPd, intRecNo++, dtrWOrderI);
            //    }
            //}

        }

        private void pmLoadRefToOrderXML(string inRefType, string inRefNo, DataSet inSource)
        {
            string strCoor = "";
            string strProd = "";
            decimal decSumSOQty = 0;
            string strSORefNo = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = " select ";
            strSQLStr += " BOOK.FCCODE as QCBOOK ";
            strSQLStr += " , ORDERH.FCREFNO ";
            strSQLStr += " , ORDERH.FCCODE ";
            strSQLStr += " , ORDERH.FDDATE ";
            strSQLStr += " , COOR.FCCODE as QCCOOR ";
            strSQLStr += " , COOR.FCSNAME as QNCOOR ";
            strSQLStr += " , ORDERH.FCSKID as FCORDERH , ORDERH.FCCOOR ";
            strSQLStr += " from ORDERH ";
            strSQLStr += " left join COOR on COOR.FCSKID = ORDERH.FCCOOR ";
            strSQLStr += " left join BOOK on BOOK.FCSKID = ORDERH.FCBOOK ";
            strSQLStr += " where ORDERH.FCCORP = ? ";
            strSQLStr += " and ORDERH.FCBRANCH = ? ";
            strSQLStr += " and ORDERH.FCREFTYPE = ? ";
            strSQLStr += " and ORDERH.FCREFNO = ? ";
            strSQLStr += " and ORDERH.FCSTEP <> 'P' ";
            strSQLStr += " and ORDERH.FCSTEP <> 'L' ";
            strSQLStr += " and ORDERH.FCSTAT <> 'C' ";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, inRefType, inRefNo });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderH", "ORDERH", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrOrderH = this.dtsDataEnv.Tables["QOrderH"].Rows[0];

                this.txtQcCoor.Tag = dtrOrderH["FCCOOR"].ToString();
                this.txtQcCoor.Text = dtrOrderH["QCCOOR"].ToString().TrimEnd();
                this.txtQnCoor.Text = dtrOrderH["QNCOOR"].ToString().TrimEnd();

                pobjSQLUtil.SetPara(new object[1] { dtrOrderH["FCORDERH"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI_Ref1", "ORDERI", "select PROD.FCSKID as FCPROD, PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD,ORDERI.* from ORDERI left join PROD on PROD.FCSKID = ORDERI.FCPROD where ORDERI.fcOrderH = ? and ORDERI.FCSTEP <> 'P' and ORDERI.FCSTEP <> 'L' and ORDERI.FNBACKQTY <> 0 order by PROD.FCCODE", ref strErrorMsg))
                {

                    foreach (DataRow dtrOrderI1 in this.dtsDataEnv.Tables["QOrderI_Ref1"].Rows)
                    {

                    }

                }
                foreach (DataRow dtrOrderI2 in inSource.Tables["ScanItem"].Rows)
                {
                    string strQcProd = dtrOrderI2["QcProd"].ToString().TrimEnd();
                    string strLot = dtrOrderI2["Lot"].ToString().TrimEnd();
                    //string strLot = "";
                    DataRow[] d1 = this.dtsDataEnv.Tables["QOrderI_Ref1"].Select("QcProd = '" + strQcProd + "' and FNBACKQTY > 0");
                    if (d1.Length > 0)
                    {
                        bool bllAddDiffRow = false;
                        decimal decBackQty = 0;
                        decimal decDiffQty = 0;
                        if (Convert.ToDecimal(dtrOrderI2["Qty"]) >= Convert.ToDecimal(d1[0]["FNBACKQTY"]))
                        {
                            decBackQty = Convert.ToDecimal(d1[0]["FNBACKQTY"]);
                            d1[0]["FNBACKQTY"] = 0;
                            dtrOrderI2["Qty"] = Convert.ToDecimal(dtrOrderI2["Qty"]) - decBackQty;
                            decDiffQty = Convert.ToDecimal(dtrOrderI2["Qty"]);
                            bllAddDiffRow = true;
                        }
                        else
                        {
                            d1[0]["FNBACKQTY"] = Convert.ToDecimal(d1[0]["FNBACKQTY"]) - Convert.ToDecimal(dtrOrderI2["Qty"]);
                            decBackQty = Convert.ToDecimal(d1[0]["FNBACKQTY"]);
                        }


                        DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemRefTo].NewRow();
                        dtrNewSO["cOrderI"] = d1[0]["fcSkid"].ToString();
                        dtrNewSO["cOrderH"] = dtrOrderH["fcOrderH"].ToString();
                        dtrNewSO["cCoor"] = dtrOrderH["fcCoor"].ToString();
                        dtrNewSO["cProd"] = d1[0]["fcProd"].ToString();
                        dtrNewSO["fcCode"] = dtrOrderH["fcCode"].ToString();
                        dtrNewSO["fcRefNo"] = dtrOrderH["fcRefNo"].ToString();
                        dtrNewSO["fdDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]);
                        dtrNewSO["fnBackQty"] = Convert.ToDecimal(d1[0]["fnBackQty"]);
                        //dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);

                        dtrNewSO["QcProd"] = d1[0]["QcProd"].ToString();
                        dtrNewSO["QnProd"] = d1[0]["QnProd"].ToString();

                        //decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                        this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrNewSO);
                        if (!strSORefNo.Contains(dtrNewSO["fcRefNo"].ToString().Trim()))
                        {
                            strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";
                        }

                        pobjSQLUtil.SetPara(new object[1] { d1[0]["fcSkid"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "ORDERI", "select * from ORDERI where fcSkid = ?", ref strErrorMsg))
                        {
                            string strLot2 = "";
                            if (this.dtsDataEnv.Tables["QOrderI"].Rows[0]["fcLot"].ToString().Trim() != "")
                            {
                                strLot2 = this.dtsDataEnv.Tables["QOrderI"].Rows[0]["fcLot"].ToString().Trim();
                            }
                            else
                            {
                                strLot2 = dtrOrderI2["Lot"].ToString();
                            }
                            pmRepl1RecTemRefProd_Ref(1, this.dtsDataEnv.Tables["QOrderI"].Rows[0], decBackQty, strLot2, "");
                        }
                        if (bllAddDiffRow)
                        {
                            pmImport_XLS1(strQcProd, strLot, decDiffQty);
                        }

                    }
                    else
                    {
                        pmImport_XLS1(strQcProd, strLot, Convert.ToDecimal(dtrOrderI2["Qty"]));
                    }
                }



                this.txtRefTo2.Text = inRefNo;

            }
            else
            {
            }


        }

        private void pmLoadRefToOrder(DataTable inRefTo)
        {
            string strCoor = "";
            string strProd = "";
            decimal decSumSOQty = 0;
            string strSORefNo = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            foreach (DataRow dtrTemSO in inRefTo.Rows)
            {
                DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemRefTo].Select("cOrderI = '" + dtrTemSO["cOrderI"].ToString() + "'");
                if (da.Length == 0)
                {
                    DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemRefTo].NewRow();
                    dtrNewSO["cOrderI"] = dtrTemSO["cOrderI"].ToString();
                    dtrNewSO["cOrderH"] = dtrTemSO["cOrderH"].ToString();
                    dtrNewSO["cCoor"] = dtrTemSO["cCoor"].ToString();
                    dtrNewSO["cProd"] = dtrTemSO["cProd"].ToString();
                    dtrNewSO["fcCode"] = dtrTemSO["fcCode"].ToString();
                    dtrNewSO["fcRefNo"] = dtrTemSO["fcRefNo"].ToString();
                    dtrNewSO["fdDate"] = Convert.ToDateTime(dtrTemSO["fdDate"]);
                    dtrNewSO["fnBackQty"] = Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                    dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);

                    dtrNewSO["QcProd"] = dtrTemSO["QcProd"].ToString();
                    dtrNewSO["QnProd"] = dtrTemSO["QnProd"].ToString();

                    decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrNewSO);
                    if (!strSORefNo.Contains(dtrNewSO["fcRefNo"].ToString().Trim()))
                    {
                        strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";
                    }

                    pobjSQLUtil.SetPara(new object[1] { dtrTemSO["cOrderI"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "ORDERI", "select * from ORDERI where fcSkid = ?", ref strErrorMsg))
                    {
                        pmRepl1RecTemRefProd_Ref(1, this.dtsDataEnv.Tables["QOrderI"].Rows[0], Convert.ToDecimal(this.dtsDataEnv.Tables["QOrderI"].Rows[0]["FNBACKQTY"]), this.dtsDataEnv.Tables["QOrderI"].Rows[0]["fcLot"].ToString(), "");
                    }
                }
                else
                {
                    da[0]["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    strSORefNo += dtrTemSO["fcRefNo"].ToString().TrimEnd() + ",";
                    decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                }

                strCoor = dtrTemSO["cCoor"].ToString();
                strProd = dtrTemSO["cProd"].ToString();

            }

            this.txtRefTo2.Text = strSORefNo;

        }

        private void pmLoadRefToOrder2(DataTable inRefTo)
        {
            string strCoor = "";
            string strProd = "";
            decimal decSumSOQty = 0;
            string strSORefNo = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            foreach (DataRow dtrTemSO in inRefTo.Rows)
            {
                DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemRefTo].Select("cOrderI = '" + dtrTemSO["cOrderI"].ToString() + "'");
                if (da.Length == 0)
                {
                    DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemRefTo].NewRow();
                    dtrNewSO["cOrderI"] = dtrTemSO["cOrderI"].ToString();
                    dtrNewSO["cOrderH"] = dtrTemSO["cOrderH"].ToString();
                    dtrNewSO["cCoor"] = dtrTemSO["cCoor"].ToString();
                    dtrNewSO["cProd"] = dtrTemSO["cProd"].ToString();
                    dtrNewSO["fcCode"] = dtrTemSO["fcCode"].ToString();
                    dtrNewSO["fcRefNo"] = dtrTemSO["fcRefNo"].ToString();
                    dtrNewSO["fdDate"] = Convert.ToDateTime(dtrTemSO["fdDate"]);
                    dtrNewSO["fnBackQty"] = Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                    dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);

                    dtrNewSO["QcProd"] = dtrTemSO["QcProd"].ToString();
                    dtrNewSO["QnProd"] = dtrTemSO["QnProd"].ToString();

                    decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrNewSO);
                    if (!strSORefNo.Contains(dtrNewSO["fcRefNo"].ToString().Trim()))
                    {
                        strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";
                    }

                    pobjSQLUtil.SetPara(new object[1] { dtrTemSO["cOrderI"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "ORDERI", "select * from STMREQI where fcSkid = ?", ref strErrorMsg))
                    {
                        pmRepl1RecTemRefProd_Ref2(1, this.dtsDataEnv.Tables["QOrderI"].Rows[0]);
                    }
                }
                else
                {
                    da[0]["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    strSORefNo += dtrTemSO["fcRefNo"].ToString().TrimEnd() + ",";
                    decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                }

                strCoor = dtrTemSO["cCoor"].ToString();
                strProd = dtrTemSO["cProd"].ToString();

            }

            this.txtRefTo2.Text = strSORefNo;

        }

        private DataRow pmRepl1RecTemRefProd_Ref(int inRecNo, DataRow inOrderI, decimal inBackQty, string inLot, string inWHLoca)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            dtrTemPd["cRowID"] = "";
            //dtrTemPd["nRecNo"] = inRecNo;

            dtrTemPd["cXRefToProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cXRefToRefType"] = inOrderI["fcRefType"].ToString();
            dtrTemPd["cXRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cXRefToHRowID"] = inOrderI["fcOrderH"].ToString();
            dtrTemPd["cRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cRefToHRowID"] = inOrderI["fcOrderH"].ToString();

            dtrTemPd["cProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cLastProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cLastRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cFormula"] = inOrderI["fcFormulas"].ToString();
            dtrTemPd["cLastFormula"] = inOrderI["fcFormulas"].ToString();
            dtrTemPd["cPFormula"] = inOrderI["fcPFormula"].ToString();
            dtrTemPd["cRootSeq"] = inOrderI["fcRootSeq"].ToString();
            dtrTemPd["cPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cLastPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cDept"] = inOrderI["fcSect"].ToString();
            dtrTemPd["cDivision"] = inOrderI["fcDept"].ToString();
            dtrTemPd["cJob"] = inOrderI["fcJob"].ToString();
            //dtrTemPd["cWHLoca"] = inOrderI["fcWHLoca"].ToString();

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
            //dtrTemPd["nQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            //dtrTemPd["nOldQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inOrderI["fnQty"]);

            //dtrTemPd["nQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);
            //dtrTemPd["nOldQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);

            dtrTemPd["nQty"] = inBackQty;
            dtrTemPd["nOldQty"] = inBackQty;
            dtrTemPd["nLastQty"] = inBackQty;

            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inOrderI["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(inOrderI["fnUmQty"]));
            dtrTemPd["nPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);

            if (inLot.TrimEnd() != "")
            {
                dtrTemPd["cLot"] = inLot;
            }
            else
            {
                dtrTemPd["cLot"] = inOrderI["fcLot"].ToString().TrimEnd();
            }

            if (inWHLoca.TrimEnd() != "")
            {
                dtrTemPd["cWHLoca"] = inWHLoca;
            }
            else
            {
                string strWHouse = "";
                switch (this.mDocType)
                {
                    case DocumentType.WR:
                    case DocumentType.GD:
                    case DocumentType.WX:
                        strWHouse = this.txtFrQcWHouse.Tag.ToString();
                        break;
                    case DocumentType.RW:
                    case DocumentType.RX:
                    case DocumentType.FR:
                        strWHouse = this.txtToQcWHouse.Tag.ToString();
                        break;
                    case DocumentType.TR:
                        strWHouse = this.txtToQcWHouse.Tag.ToString();
                        break;
                }

                if (strWHouse != string.Empty
                    && pobjSQLUtil.SetPara(new object[] { strWHouse })
                    && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHOUSE", "WHOUSE", "select * from WHOUSE where fcSkid = ? ", ref strErrorMsg))
                {
                    string strWHLoca = this.dtsDataEnv.Tables["QWHOUSE"].Rows[0]["FCWHLOCA_STOCK"].ToString();
                    pobjSQLUtil.SetPara(new object[1] { strWHLoca });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCSKID,FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
                    {
                        dtrTemPd["cWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcSkid"].ToString();
                        dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    //inTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                }
                else
                {
                    dtrTemPd["cQcWHLoca"] = "";
                }
            }
            
            dtrTemPd["cWHouse"] = inOrderI["fcWHouse"].ToString();


            //dtrTemPd["fnAgeLong"] = Convert.ToDecimal(inOrderI["fnAgeLong"]);
            //if (Convert.IsDBNull(inOrderI["fdExpire"]))
            //{
            //    dtrTemPd["fdExpire"] = Convert.DBNull;
            //}
            //else
            //{
            //    dtrTemPd["fdExpire"] = Convert.ToDateTime(inOrderI["fdExpire"]);
            //}

            dtrTemPd["nOQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
            dtrTemPd["nOStQty"] = Convert.ToDecimal(dtrTemPd["nStQty"]);
            dtrTemPd["nOUMQty"] = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
            dtrTemPd["nOStUMQty"] = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);

            //pobjSQLUtil.SetPara(new object[1] {dtrTemPd["cProd"].ToString()});
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
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
                {
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

            //pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHLoca"].ToString() });
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
            //{
            //    dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
            //}

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

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            return dtrTemPd;
        }

        private DataRow pmRepl1RecTemRefProd_Ref2(int inRecNo, DataRow inOrderI)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            dtrTemPd["cRowID"] = "";
            //dtrTemPd["nRecNo"] = inRecNo;

            dtrTemPd["cXRefToProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cXRefToRefType"] = inOrderI["fcRefType"].ToString();
            dtrTemPd["cXRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cXRefToHRowID"] = inOrderI["fcStmReqH"].ToString();
            dtrTemPd["cRefToRowID"] = inOrderI["fcSkid"].ToString();
            dtrTemPd["cRefToHRowID"] = inOrderI["fcStmReqH"].ToString();

            dtrTemPd["cProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cLastProd"] = inOrderI["fcProd"].ToString();
            dtrTemPd["cRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cLastRefPdType"] = inOrderI["fcRefPdTyp"].ToString();
            dtrTemPd["cFormula"] = inOrderI["fcFormulas"].ToString();
            dtrTemPd["cLastFormula"] = inOrderI["fcFormulas"].ToString();
            dtrTemPd["cPFormula"] = inOrderI["fcPFormula"].ToString();
            dtrTemPd["cRootSeq"] = inOrderI["fcRootSeq"].ToString();
            dtrTemPd["cPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cLastPdType"] = inOrderI["fcProdType"].ToString();
            dtrTemPd["cDept"] = inOrderI["fcSect"].ToString();
            dtrTemPd["cDivision"] = inOrderI["fcDept"].ToString();
            dtrTemPd["cJob"] = inOrderI["fcJob"].ToString();
            dtrTemPd["cWHLoca"] = inOrderI["fcWHLoca"].ToString();

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
            //dtrTemPd["nQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            //dtrTemPd["nOldQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inOrderI["fnQty"]);
            dtrTemPd["nQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);
            dtrTemPd["nOldQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inOrderI["fnBackQty"]);

            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inOrderI["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(inOrderI["fnUmQty"]));
            dtrTemPd["nPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);

            dtrTemPd["cLot"] = inOrderI["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inOrderI["fcWHouse"].ToString();


            //dtrTemPd["fnAgeLong"] = Convert.ToDecimal(inOrderI["fnAgeLong"]);
            //if (Convert.IsDBNull(inOrderI["fdExpire"]))
            //{
            //    dtrTemPd["fdExpire"] = Convert.DBNull;
            //}
            //else
            //{
            //    dtrTemPd["fdExpire"] = Convert.ToDateTime(inOrderI["fdExpire"]);
            //}

            dtrTemPd["nOQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
            dtrTemPd["nOUMQty"] = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
            dtrTemPd["nOStQty"] = Convert.ToDecimal(dtrTemPd["nStQty"]);
            dtrTemPd["nOStUMQty"] = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);

            //pobjSQLUtil.SetPara(new object[1] {dtrTemPd["cProd"].ToString()});
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
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
                {
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

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHLoca"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
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

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            return dtrTemPd;
        }

        private void pmImport_XLS1(string inQcProd, string inLot, decimal inQty)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = null;
            if (inQcProd != "")
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inQcProd });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    this.pmRetrieveProductVal(ref dtrTemPd, this.dtsDataEnv.Tables["QProd"].Rows[0]);

                }
            }
            else
            {
                return;
            }

            dtrTemPd["cRowID"] = "";
            //dtrTemPd["nRecNo"] = inRecNo;

            //dtrTemPd["cXRefToProd"] = inOrderI["fcProd"].ToString();
            //dtrTemPd["cXRefToRefType"] = inOrderI["fcRefType"].ToString();
            //dtrTemPd["cXRefToRowID"] = inOrderI["fcSkid"].ToString();
            //dtrTemPd["cXRefToHRowID"] = inOrderI["fcOrderH"].ToString();
            //dtrTemPd["cRefToRowID"] = inOrderI["fcSkid"].ToString();
            //dtrTemPd["cRefToHRowID"] = inOrderI["fcOrderH"].ToString();

            //dtrTemPd["cProd"] = inOrderI["fcProd"].ToString();
            //dtrTemPd["cLastProd"] = inOrderI["fcProd"].ToString();
            //dtrTemPd["cRefPdType"] = "";
            //dtrTemPd["cLastRefPdType"] = "";
            //dtrTemPd["cFormula"] = "";
            //dtrTemPd["cLastFormula"] = "";
            //dtrTemPd["cPFormula"] = "";
            //dtrTemPd["cRootSeq"] = "";
            //dtrTemPd["cPdType"] = "";
            //dtrTemPd["cLastPdType"] = "";
            //dtrTemPd["cDept"] = "";
            //dtrTemPd["cDivision"] = "";
            //dtrTemPd["cJob"] = "";
            //dtrTemPd["cWHLoca"] = inOrderI["fcWHLoca"].ToString();

            //dtrTemPd["cUOM"] = inOrderI["fcUm"].ToString();
            dtrTemPd["nQty"] = inQty;
            dtrTemPd["nOldQty"] = inQty;
            dtrTemPd["nUOMQty"] = 1;
            dtrTemPd["nLastQty"] = inQty;
            //dtrTemPd["nPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);

            dtrTemPd["cLot"] = inLot;

            //dtrTemPd["cWHouse"] = this.txtToQcWHouse.Tag.ToString();

            string strWHouse = "";
            switch (this.mDocType)
            {
                case DocumentType.WR:
                case DocumentType.GD:
                case DocumentType.WX:
                    strWHouse = this.txtFrQcWHouse.Tag.ToString();
                    break;
                case DocumentType.RW:
                case DocumentType.RX:
                case DocumentType.FR:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
                case DocumentType.TR:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
            }

            dtrTemPd["cWHouse"] = strWHouse;

            dtrTemPd["nOQty"] = 1;
            dtrTemPd["nOUMQty"] = 1;
            dtrTemPd["nOStQty"] = 1;
            dtrTemPd["nOStUMQty"] = 1;

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
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
                {
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

            //pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHLoca"].ToString() });
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
            //{
            //    dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
            //}

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

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            //return dtrTemPd;
        }

        private string pmGetRngCode()
        {
            string strTagStr = "";
            for (int intCnt = 0; intCnt < this.pATagCode.Count; intCnt++)
            {
                strTagStr += "'" + this.pATagCode[intCnt].ToString() + "', ";
            }
            strTagStr = (strTagStr.Length > 2 ? AppUtil.StringHelper.Left(strTagStr, strTagStr.Length - 2) : "");
            return strTagStr;
        }

        private void pmRepl1RecTemWcTranI2(string inAlias, int inRecNo, DataRow inWcTranI)
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            cDBMSAgent pobjSQLUtil2 = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            bool bllIsNewRow = false;
            DataRow dtrTemPd = this.pmNewItem(inRecNo, ref bllIsNewRow);

            dtrTemPd["nRecNo"] = inRecNo + 1;
            //dtrTemPd["cOPSeq"] = inWcTranI["cOPSeq"].ToString().TrimEnd();

            //dtrTemPd["cWOrderI"] = inWcTranI["cRowID"].ToString().TrimEnd();

            dtrTemPd["cRefToHRowID"] = this.mstrRefToRowID;
            dtrTemPd["cRefToRowID"] = inWcTranI["cRowID"].ToString();
            
            dtrTemPd["cProd"] = inWcTranI["cProd"].ToString();
            dtrTemPd["cLastProd"] = inWcTranI["cProd"].ToString();
            dtrTemPd["cPdType"] = inWcTranI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cLastPdType"] = inWcTranI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cRemark1"] = (Convert.IsDBNull(inWcTranI["cReMark1"]) ? "" : inWcTranI["cReMark1"].ToString().TrimEnd());
            dtrTemPd["cRemark2"] = (Convert.IsDBNull(inWcTranI["cReMark2"]) ? "" : inWcTranI["cReMark2"].ToString().TrimEnd());
            dtrTemPd["cRemark3"] = (Convert.IsDBNull(inWcTranI["cReMark3"]) ? "" : inWcTranI["cReMark3"].ToString().TrimEnd());
            dtrTemPd["cRemark4"] = (Convert.IsDBNull(inWcTranI["cReMark4"]) ? "" : inWcTranI["cReMark4"].ToString().TrimEnd());
            dtrTemPd["cRemark5"] = (Convert.IsDBNull(inWcTranI["cReMark5"]) ? "" : inWcTranI["cReMark5"].ToString().TrimEnd());
            dtrTemPd["cRemark6"] = (Convert.IsDBNull(inWcTranI["cReMark6"]) ? "" : inWcTranI["cReMark6"].ToString().TrimEnd());
            dtrTemPd["cRemark7"] = (Convert.IsDBNull(inWcTranI["cReMark7"]) ? "" : inWcTranI["cReMark7"].ToString().TrimEnd());
            dtrTemPd["cRemark8"] = (Convert.IsDBNull(inWcTranI["cReMark8"]) ? "" : inWcTranI["cReMark8"].ToString().TrimEnd());
            dtrTemPd["cRemark9"] = (Convert.IsDBNull(inWcTranI["cReMark9"]) ? "" : inWcTranI["cReMark9"].ToString().TrimEnd());
            dtrTemPd["cRemark10"] = (Convert.IsDBNull(inWcTranI["cReMark10"]) ? "" : inWcTranI["cReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inWcTranI["cUOM"].ToString().TrimEnd();
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWcTranI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWcTranI["nUOMQty"]));
            dtrTemPd["cLot"] = inWcTranI["cLot"].ToString().TrimEnd();

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString().TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            //Load MO Qty

            decimal decMOQty = 0;
            decimal decStMoveQty = 0;
            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cRefToRowID"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, "select NQTY from " + QMFWOrderIT_PdInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["nMOQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["NQTY"]);
                decMOQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["NQTY"]);
            }

            dtrTemPd["nQty"] = 0;
            switch (this.mDocType)
            {
                case DocumentType.WR:
                case DocumentType.WX:
                    pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRefType, dtrTemPd["cRefToRowID"].ToString(), this.mstrRefType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
                    {
                        if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                        {
                            dtrTemPd["nStMoveQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                            decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                        }
                    }
                    //if (decMOQty - decStMoveQty > 0)
                    //{
                    //    dtrTemPd["nQty"] = decMOQty - decStMoveQty;
                    //}
                    break;
                case DocumentType.RW:
                case DocumentType.RX:
                    pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRefType, dtrTemPd["cRefToRowID"].ToString(), DocumentType.WR.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
                    {
                        if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                        {
                            dtrTemPd["nStMoveQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                            decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                        }
                    }
                    break;
                case DocumentType.FR:
                    break;
                case DocumentType.TR:
                    break;
            }

            if (bllIsNewRow)
                this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private void pmLoadSectByWorkCenter(string inWkCtr)
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            cDBMSAgent pobjSQLUtil2 = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //this.mbllIsNoLoss = false;

            pobjSQLUtil.SetPara(new object[1] { inWkCtr });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QStdOPR", "WKCTRH", "select cSect,cJob from MFSTDOPR where cRowID = ?", ref strErrorMsg))
            {
                this.txtQcSect.Tag = this.dtsDataEnv.Tables["QStdOPR"].Rows[0]["cSect"].ToString();
                pobjSQLUtil2.SetPara(new object[1] { this.txtQcSect.Tag.ToString() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode, fcName, fcDept from Sect where fcSkid = ?", ref strErrorMsg))
                {
                    this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    //this.mstrDept = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString();
                }

                this.txtQcJob.Tag = this.dtsDataEnv.Tables["QStdOPR"].Rows[0]["cJob"].ToString();
                pobjSQLUtil2.SetPara(new object[1] { this.txtQcJob.Tag.ToString() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode, fcName, fcProj from Job where fcSkid = ?", ref strErrorMsg))
                {
                    this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                }

                //this.mbllIsNoLoss = (this.dtsDataEnv.Tables["QStdOPR"].Rows[0]["cIsNoLoss"].ToString().Trim() == "Y" ? false : true);
            }

        }

        private void pmLoadOPSeq()
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq", "WORDEROP", "select COPSEQ from WORDEROP where CWORDERH = ? group by COPSEQ order by COPSEQ", ref strErrorMsg))
            {
                //this.txtFrOPSeq.DataSource = this.dtsDataEnv.Tables["QOPSeq"];
                //this.txtFrOPSeq.DisplayMember = "COPSEQ";

                //this.cmbToOPSeq.DataSource = this.dtsDataEnv.Tables["QOPSeq"];
                //this.cmbToOPSeq.DisplayMember = "COPSEQ";
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strTagButton = (e.Button.Tag != null ? e.Button.Tag.ToString() : "");
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), strTagButton, txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), "", txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonClick(string inTextbox, string inTag , string inPara1)
        {
            string strPrefix = "";
            switch (inTextbox)
            {
                case "TXTREFTO2":
                    this.pmInitPopUpDialog("REFTO");
                    break;
                case "TXTREFTO":
                    this.pmInitPopUpDialog("REFTO");
                    break;
                case "TXTQCPROD":
                case "TXTQNPROD":
                    this.pmInitPopUpDialog("PROD");
                    strPrefix = (inTextbox == "TXTQCPROD" ? "CCODE" : "CNAME");
                    this.pofrmGetProd.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCCOOR":
                case "TXTQNCOOR":

                    if (this.mCoorType == CoorType.Customer)
                    {
                        this.pmInitPopUpDialog("CUST");
                        strPrefix = (inTextbox == "TXTQCCOOR" ? "CCODE" : "CNAME");
                        this.pofrmGetCust.ValidateField(inPara1, strPrefix, true);
                        if (this.pofrmGetCust.PopUpResult)
                        {
                            this.pmRetrievePopUpVal(inTextbox);
                        }
                    }
                    else 
                    {
                        this.pmInitPopUpDialog("SUPPL");
                        strPrefix = (inTextbox == "TXTQCCOOR" ? "FCCODE" : "FCNAME");
                        this.pofrmGetSuppl.ValidateField(inPara1, strPrefix, true);
                        if (this.pofrmGetSuppl.PopUpResult)
                        {
                            this.pmRetrievePopUpVal(inTextbox);
                        }
                    }
                    break;
                case "TXTQCBOM":
                case "TXTQNBOM":
                    this.pmInitPopUpDialog("BOM");
                    strPrefix = (inTextbox == "TXTQCBOM" ? "CCODE" : "CNAME");
                    this.pofrmGetBOM.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetBOM.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQNMFGUM":
                    this.pmInitPopUpDialog("UM");
                    strPrefix = (inTextbox == "TXTQCMFGUM" ? "FCCODE" : "FCNAME");
                    this.pofrmGetUM.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetUM.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCSECT":
                case "TXTQNSECT":
                    this.pmInitPopUpDialog("SECT");
                    strPrefix = (inTextbox == "TXTQCSECT" ? "FCCODE" : "FCNAME");
                    this.pofrmGetSect.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetSect.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCJOB":
                case "TXTQNJOB":
                    this.pmInitPopUpDialog("JOB");
                    strPrefix = (inTextbox == "TXTQCJOB" ? "FCCODE" : "FCNAME");
                    this.pofrmGetJob.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetJob.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTFROPSEQ":
                    this.pmInitPopUpDialog("GET_OPSEQ");
                    break;
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    this.pmInitPopUpDialog("WHOUSE");
                    strPrefix = ("TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(inTextbox) > -1 ? "FCCODE" : "FCNAME");
                    this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", "", strPrefix, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTOQCWHOUSE":
                case "TXTTOQNWHOUSE":
                    this.pmInitPopUpDialog("WHOUSE");
                    strPrefix = ("TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(inTextbox) > -1 ? "FCCODE" : "FCNAME");
                    this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrToWhType) + ")", "", strPrefix, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTREMARK":
                    this.pmInitPopUpDialog("REMARK_H");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            //App.ActivateMainScreen();
            //App.SetForegroundWindow(this.Handle);

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrGetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "TXTQCCOOR":
                case "TXTQNCOOR":

                    if (this.mCoorType == CoorType.Customer)
                    {
                        dtrGetVal = this.pofrmGetCust.RetrieveValue();
                    }
                    else
                        dtrGetVal = this.pofrmGetSuppl.RetrieveValue();
                    {
                    }
                    
                    if (dtrGetVal != null)
                    {
                        this.pmRetrieveCoorVal(dtrGetVal);
                    }
                    else
                    {
                        this.txtQcCoor.Tag = "";
                        this.txtQcCoor.Text = "";
                        this.txtQnCoor.Text = "";
                    }
                    break;

                case "TXTQCSECT":
                case "TXTQNSECT":

                    dtrGetVal = this.pofrmGetSect.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        this.txtQcSect.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcSect.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        //this.txtQnSect.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcSect.Tag = "";
                        this.txtQcSect.Text = "";
                        //this.txtQnSect.Text = "";
                    }
                    break;

                case "TXTQCJOB":
                case "TXTQNJOB":

                    dtrGetVal = this.pofrmGetJob.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        this.txtQcJob.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcJob.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        //this.txtQnJob.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcJob.Tag = "";
                        this.txtQcJob.Text = "";
                        //this.txtQnJob.Text = "";
                    }
                    break;

                case "GRDVIEW2_CPDTYPE":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                        this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetProdType = this.pofrmGetProdType.RetrieveValue();
                    if (dtrGetProdType != null)
                    {
                        //
                        if (dtrGetVal["cPdType"].ToString().TrimEnd() != dtrGetProdType["fcCode"].ToString().TrimEnd())
                        {
                            this.pmClr1TemPd();
                        }

                        dtrGetVal["cPdType"] = dtrGetProdType["fcCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cPdType"] = "";
                        this.pmClr1TemPd();
                    }

                    this.mActiveGrid.UpdateCurrentRow();

                    break;
                case "GRDVIEW2_CQCPROD":
                case "GRDVIEW2_CQNPROD":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                        this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetProd = this.pofrmGetProd.RetrieveValue();
                    if (dtrGetProd != null)
                    {
                        //dtrGetVal["cProd"] = dtrGetProd["fcSkid"].ToString();
                        //dtrGetVal["cPdType"] = dtrGetProd["fcType"].ToString().TrimEnd();
                        //dtrGetVal["cQcProd"] = dtrGetProd["fcCode"].ToString().TrimEnd();
                        //dtrGetVal["cQnProd"] = dtrGetProd["fcName"].ToString().TrimEnd();
                        //dtrGetVal["cUOM"] = dtrGetProd["fcUM"].ToString().TrimEnd();
                        //dtrGetVal["cUOMStd"] = dtrGetProd["fcUM"].ToString().TrimEnd();
                        this.pmRetrieveProductVal(ref dtrGetVal, dtrGetProd);

                    }
                    else
                    {
                        this.pmClr1TemPd();
                    }

                    this.mActiveGrid.UpdateCurrentRow();
                    this.pmLoadRMDetail();

                    break;
                case "TXTFROPSEQ":

                    if (this.pofrmGetOPSeq.SelectedRow() > -1)
                    {

                        this.pmRetrieveOPSeqVal(this.pofrmGetOPSeq.SelectedRow());

                    }
                    else
                    {
                        this.pmClearRefTo();
                    }
                    break;
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    if (this.pofrmGetWareHouse != null)
                    {
                        dtrGetVal = this.pofrmGetWareHouse.RetrieveValue();
                        if (dtrGetVal != null)
                        {
                            if (this.txtFrQcWHouse.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                            {
                            }
                            this.txtFrQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtFrQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                            this.txtFrQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtFrQcWHouse.Tag = "";
                            this.txtFrQcWHouse.Text = "";
                            this.txtFrQnWHouse.Text = "";
                        }
                    }
                    break;
                case "TXTTOQCWHOUSE":
                case "TXTTOQNWHOUSE":
                    if (this.pofrmGetWareHouse != null)
                    {
                        dtrGetVal = this.pofrmGetWareHouse.RetrieveValue();
                        if (dtrGetVal != null)
                        {

                            if (this.txtToQcWHouse.Tag != dtrGetVal["fcSkid"].ToString())
                            {
                            }
                            this.txtToQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtToQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                            this.txtToQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();

                        }
                        else
                        {
                            this.txtToQcWHouse.Tag = "";
                            this.txtToQcWHouse.Text = "";
                            this.txtToQnWHouse.Text = "";
                        }
                    }
                    break;
                case "GRDVIEW2_CQCWHLOCA":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                        this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetWHLoca = this.pofrmGetWHLoca.RetrieveValue();
                    if (dtrGetWHLoca != null)
                    {
                        dtrGetVal["cWHLoca"] = dtrGetWHLoca["fcSkid"].ToString().TrimEnd();
                        dtrGetVal["cQcWHLoca"] = dtrGetWHLoca["fcCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cWHLoca"] = "";
                        dtrGetVal["cQcWHLoca"] = "";
                    }

                    this.mActiveGrid.UpdateCurrentRow();

                    break;
            }
        }

        private void pmRetrieveCoorVal(DataRow inCoor)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.txtQcCoor.Tag = inCoor["fcSkid"].ToString();
            this.txtQcCoor.Text = inCoor["fcCode"].ToString().TrimEnd();
            this.txtQnCoor.Text = inCoor["fcSName"].ToString().TrimEnd();
            //this.mstrInvDetail = inCoor["fcName"].ToString().TrimEnd();

            //this.mstrLastQcCoor = this.txtQcCoor.Text;
            //this.mstrLastQsCoor = this.txtQsCoor.Text;

        }

        private void pmClearTemToBlank()
        {
            this.mstrActiveTem = this.mstrTemFG;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
            {
                DataRow dtrTem = dtrTemPd;
                this.pmClr1TemPd(ref dtrTem);
            }

        }

        private void pmClearTemToBlank2()
        {
            this.mstrActiveTem = this.mstrTemPd;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                DataRow dtrTem = dtrTemPd;
                this.pmClr1TemPd(ref dtrTem);
            }

        }

        private void pmRetrieveProductVal(ref DataRow inTemPd, DataRow inProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //string strUOM = inProd["fcUm"].ToString();
            //string strStkUOM = inProd["fcStUm"].ToString();
            //decimal decUOMQty = 1;
            //decimal decStkUOMQty = 1;

            string strUOM = inProd["fcUm2"].ToString();
            string strStkUOM = inProd["fcStUm"].ToString();
            decimal decUOMQty = 1;
            decimal decStkUOMQty = 1;

            strUOM = (inProd["fcUm2"].ToString().TrimEnd() != string.Empty ? inProd["fcUm2"].ToString() : inProd["fcUm"].ToString());
            strStkUOM = (inProd["fcUm2"].ToString().TrimEnd() != string.Empty ? inProd["fcStUm2"].ToString() : inProd["fcUm"].ToString());
            decUOMQty = Convert.ToDecimal(inProd["fnUmQty2"]);
            decStkUOMQty = Convert.ToDecimal(inProd["fnStUmQty2"]);

            inTemPd["cProd"] = inProd["fcSkid"].ToString();
            inTemPd["cFormula"] = "";
            inTemPd["cRefPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
            inTemPd["cLastRefPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
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
            inTemPd["cWHouse"] = this.txtFrQcWHouse.Tag;
            //inTemPd["cWHouse"] = this.txtQcWHouse.RowID;

            inTemPd["fnAgeLong"] = Convert.ToDecimal(inProd["fnAgeLong"]);
            if (Convert.ToDecimal(inProd["fnAgeLong"]) > 0)
            {
                inTemPd["fdExpire"] = this.txtDate.DateTime.AddDays(Convert.ToDouble(inProd["fnAgeLong"]));
            }
            else
            {
                inTemPd["fdExpire"] = Convert.DBNull;
            }

            inTemPd["nPrice"] = this.pmGetCost(inTemPd["cProd"].ToString(), this.txtFrQcWHouse.Tag.ToString(), inTemPd["cLot"].ToString(), decUOMQty);

            if (!Convert.IsDBNull(inProd["fmPicName"]))
            {
                if (inProd["fmPicName"].ToString().Trim() != "...")
                {
                    inTemPd["cAttachFile"] = inProd["fmPicName"].ToString().TrimEnd();
                }
            }

            ////inTemPd["nQty"] = 1;
            //inTemPd["nQty"] = 0;
            //inTemPd["nUOMQty"] = 1;

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

            string strWHouse = "";
            switch (this.mDocType)
            {
                case DocumentType.WR:
                case DocumentType.GD:
                case DocumentType.WX:
                    strWHouse = this.txtFrQcWHouse.Tag.ToString();
                    break;
                case DocumentType.RW:
                case DocumentType.RX:
                case DocumentType.FR:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
                case DocumentType.TR:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
            }

            if (strWHouse != string.Empty
                && pobjSQLUtil.SetPara(new object[] { strWHouse })
                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHOUSE", "WHOUSE", "select * from WHOUSE where fcSkid = ? ", ref strErrorMsg))
            {
                string strWHLoca = this.dtsDataEnv.Tables["QWHOUSE"].Rows[0]["FCWHLOCA_STOCK"].ToString();
                pobjSQLUtil.SetPara(new object[1] { strWHLoca });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCSKID,FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
                {
                    inTemPd["cWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcSkid"].ToString();
                    inTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
                }

                //inTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            else
            {
                inTemPd["cQcWHLoca"] = "";
            }

            //this.grdTemPd.SetValue("cQcProd", inProd["fcCode"].ToString().TrimEnd());
            //this.grdTemPd.SetValue("cQnProd", inProd["fcName"].ToString().TrimEnd());
            //this.grdTemPd.SetValue("cQnUOM", inTemPd["cQnUOM"].ToString().TrimEnd());
            //this.grdTemPd.UpdateData();
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
            //inTemPd["cWHouse"] = this.txtQcWHouse.RowID;

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

                            dtrTemPd["nRecNo"] = intRecNo;
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

                            dtrTemPd["nRecNo"] = intRecNo;
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

        private decimal pmGetCost(string inProd, string inWHouse, string inLot, decimal inUmQty)
        {
            string strErrorMsg = "";
            decimal decPrice = 0;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fnStdCost from Prod where fcSkid = ? ", ref strErrorMsg))
            {
                decPrice = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["fnStdCost"]);
            }
            if (decPrice == 0)
            {
                pobjSQLUtil.SetPara(new object[] { inProd, this.mstrBranch, inWHouse });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdBranch", "PDBRANCH", "select fnAvgCost from PdBranch where fcProd = ? and fcBranch = ? and fcWHouse = ? ", ref strErrorMsg))
                {
                    decPrice = Convert.ToDecimal(this.dtsDataEnv.Tables["QPdBranch"].Rows[0]["fnAvgCost"]);
                }
            }
            return decPrice;
        }

        private void pmClr1TemPd()
        {

            if (this.mActiveGrid.FocusedRowHandle < 0)
                return;

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrActiveTem].Rows[this.mActiveGrid.FocusedRowHandle];
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
            dtrTemPd["nPlanQty"] = 0;
            dtrTemPd["nPrice"] = 0;
            dtrTemPd["cDiscStr"] = "";
            //dtrTemPd["nAmt"] = 0;

            dtrTemPd["cRefToRowID"] = "";
            dtrTemPd["cRefToCode"] = "";

            dtrTemPd["fnAgeLong"] = 0;
            dtrTemPd["fdExpire"] = Convert.DBNull;

            this.mbllRecalTotPd = true;
            this.pmRecalTotPd();
        }

        private void pmClr1TemPd(ref DataRow inTemPd)
        {
            DataRow dtrTemPd = inTemPd;
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
            inTemPd["nPlanQty"] = 0;
            inTemPd["nPrice"] = 0;
            inTemPd["nLastPrice"] = 0;
            inTemPd["cDiscStr"] = "";
            //inTemPd["nAmt"] = 0;

            inTemPd["nMOQty"] = 0;
            inTemPd["nStmoveQty"] = 0;

            inTemPd["cRefToRowID"] = "";
            inTemPd["cRefToCode"] = "";

            this.mbllRecalTotPd = true;
            this.pmRecalTotPd();
        }


        private decimal pmCalOutputQty(string inOPSeq)
        {

            decimal decOutputQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {

                if ("1,5".IndexOf(dtrTemPd["cPdType"].ToString()) > -1 && dtrTemPd["cOPSeq"].ToString() == inOPSeq)
                {
                    //inOPSeq = 0;
                    decOutputQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                }

            }

            return decOutputQty;
        }

        private void pmSetUMQtyMsg()
        {
            //if (this.txtQnUM.Text.ToString().TrimEnd() != string.Empty && this.txtQnStdUM.Text.ToString().TrimEnd() != string.Empty
            //    && this.txtQnStdUM.Text.ToString().TrimEnd() != this.txtQnUM.Text.ToString().TrimEnd())
            //    this.lblUMQty.Text = "1 " + this.txtQnUM.Text.ToString().TrimEnd() + " = " + this.mdecUMQty.ToString("##,##0.0000") + " " + this.txtQnStdUM.Text.TrimEnd();
            //else
            //{
            //    this.lblUMQty.Text = "";
            //    this.mdecUMQty = 1;
            //}
        }

        private void pmRecalTotPd()
        {

            decimal decSumQty = 0;

            if (this.mbllRecalTotPd)
            {
                foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
                {
                    decSumQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                }
                this.mbllRecalTotPd = false;
            }

        }

        private void txtQcSect_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSECT" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcSect.Tag = "";
                this.txtQcSect.Text = "";
                //this.txtQnSect.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("SECT");
                e.Cancel = !this.pofrmGetSect.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetSect.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcJob_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCJOB" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcJob.Tag = "";
                this.txtQcJob.Text = "";
                //this.txtQnJob.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("JOB");
                e.Cancel = !this.pofrmGetJob.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetJob.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtFrQcWHouse_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(txtPopUp.Name.ToUpper()) > -1 ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtFrQcWHouse.Tag = "";
                this.txtFrQcWHouse.Text = "";
                this.txtFrQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WHOUSE");
                e.Cancel = !this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWareHouse.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtToQcWHouse_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(txtPopUp.Name.ToUpper()) > -1 ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtToQcWHouse.Tag = "";
                this.txtToQcWHouse.Text = "";
                this.txtToQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WHOUSE");
                e.Cancel = !this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrToWhType) + ")", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWareHouse.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtFrOPSeq_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CFROPSEQ";

            if (txtPopUp.Text == "")
            {
                txtPopUp.Tag = "";
                this.txtFrQnOP.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("GET_OPSEQ2");
                e.Cancel = !this.pofrmGetOPSeq.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetOPSeq.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCCOOR" ? "FCCODE" : "FCSNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcCoor.Tag = "";
                this.txtQcCoor.Text = "";
                this.txtQnCoor.Text = "";
            }
            else
            {

                if (this.mCoorType == CoorType.Customer)
                {
                    strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCCOOR" ? "CCODE" : "CSNAME";
                    this.pmInitPopUpDialog("CUST");
                    e.Cancel = !this.pofrmGetCust.ValidateField(txtPopUp.Text, strOrderBy, false);
                    if (this.pofrmGetCust.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(txtPopUp.Name);
                    }
                    else
                    {
                        txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                    }
                }
                else
                {
                    this.pmInitPopUpDialog("SUPPL");
                    e.Cancel = !this.pofrmGetSuppl.ValidateField(txtPopUp.Text, strOrderBy, false);
                    if (this.pofrmGetSuppl.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(txtPopUp.Name);
                    }
                    else
                    {
                        txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                    }

                }
            }

        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "RPT_DESIGN":
                        pmEditReportForm();
                        break;
                    case "PRN_BARCODE":
                        break;
                    case "ON_KEYB":
                        this.ribbonControl1.Visible = !this.ribbonControl1.Visible;
                        break;
                    case "IMP_XLS1":
                        //
                        this.pmInitPopUpDialog("IMP_XLS1");
                        break;
                    case "GENPR":

                        bool bllResult = true;

                        if (pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
                        {
                            bllResult = this.pmSaveData();
                        }

                        if (bllResult)
                        {
                            string strErrorMsg = "";
                            string strRowID = "";
                            if (this.mstrSaveRowID == string.Empty)
                            {
                                DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                                if (this.gridView1.RowCount == 0 || dtrBrow == null)
                                    return;

                                strRowID = dtrBrow["cRowID"].ToString();
                            }

                            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                            DataRow dtrLoadHead = null;
                            objSQLHelper.SetPara(new object[] { strRowID });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ? ", ref strErrorMsg))
                            {
                                dtrLoadHead = this.dtsDataEnv.Tables["QLoadPR"].Rows[0];
                            }
                            
                            this.pmInitPopUpDialog("GENPR");

                            //if (!this.pmCheckHasPR(this.mstrEditRowID, dtrLoadHead[QMFStmoveHDInfo.Field.RefNo].ToString(), ref strErrorMsg))
                            //else
                            //    MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                        break;
                    default:
                        WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                        switch (oToolButton)
                        {
                            case WsToolBar.Enter:
                                this.pmEnterForm();
                                break;
                            case WsToolBar.Insert:
                                //if (App.PermissionManager.CheckPermission(AuthenType.CanInsert, App.AppUserID, TASKNAME))
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanInsert, App.AppUserName, App.AppUserID))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Insert;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการเพิ่มข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Update:
                                //if (App.PermissionManager.CheckPermission(AuthenType.CanEdit, App.AppUserID, TASKNAME))
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Edit;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Delete:
                                //if (App.PermissionManager.CheckPermission(AuthenType.CanDelete, App.AppUserID, TASKNAME))
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                                {
                                    this.pmDeleteData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการลบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            case WsToolBar.Print:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanPrint, App.AppUserName, App.AppUserID))
                                {
                                    this.pmPrintData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการพิมพ์ข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case WsToolBar.Cancel:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                                {
                                    //this.pmCancelData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            case WsToolBar.Close:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                                {
                                    //this.pmCloseData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            case WsToolBar.Search:
                                this.pmSearchData();
                                break;
                            case WsToolBar.Undo:
                                this.pmKeyboard_Esc();
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

                string[] strADir = null;
                string strFormPath = Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\";
                if (System.IO.Directory.Exists(strFormPath))
                {
                    strADir = System.IO.Directory.GetFiles(strFormPath);
                    dlg.LoadRPT(strFormPath);
                }

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    //App.mSave_hWnd = App.GetForegroundWindow();
                    string strRPTFileName = "";
                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);
                }
            }
        }

        ArrayList pAHideFMItem = new ArrayList();
        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";

            string strIOType = (this.mstrFrWhType == SysDef.gc_WHOUSE_TYPE_NORMAL ? "O" : "I");
            strIOType = (this.mstrRefType == DocumentType.TR.ToString() ? "I" : strIOType);
            string strSQLStrOrderI = "select * from " + this.mstrITable + " where fcGLRef = ? and fcIOType = ? order by fcSeq";


            Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new Report.LocalDataSet.FORM2PRINT();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //object[] pAPara = new object[3] { App.ActiveCorp.CorpID, inBegCode, inEndCode};

            Report.Agents.PrintField oPrintField = new Report.Agents.PrintField(pobjSQLUtil, pobjSQLUtil2, App.ActiveCorp);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrHTable + " where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode between ? and ? order by fcCode";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, inBegCode, inEndCode });

            //string strRPTFileName = this.pmLoadWsFormH(this.mstrBook);
            string strRPTFileName = inRPTFileName;
            if (strRPTFileName != string.Empty)
            {
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, PrintField.xd_PRN_FORM_H, this.mstrHTable, strSQLStr, ref strErrorMsg))
                {
                    DataRow dtrLastRow = null;
                    this.pAHideFMItem.Clear();
                    foreach (DataRow dtrPFormH in this.dtsDataEnv.Tables[PrintField.xd_PRN_FORM_H].Rows)
                    {

                        string strLastPrnPrefixValue = "";
                        int intRowCount = 0;
                        dtrLastRow = null;

                        pobjSQLUtil.SetPara(new object[] { dtrPFormH["fcSkid"].ToString(), strIOType });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, PrintField.xd_PRN_FORM_I, "OrderI", strSQLStrOrderI, ref strErrorMsg))
                        {

                            string strShowCompo = "";
                            foreach (DataRow dtrPFormI in this.dtsDataEnv.Tables[PrintField.xd_PRN_FORM_I].Rows)
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

                                    //strShowCompo = (dtrFormula["fcShowComp"].ToString() == "0" || dtrFormula["fcShowComp"].ToString().TrimEnd() == "" ? App.ActiveCorp.ShowFormulaCompo : dtrFormula["fcShowComp"].ToString());
                                    //26/03/2008 แก้ให้เพิ่ม Option ในการพิมพ์ว่าจะพิมพ์รายการในชุดสินค้าหรือไม่ ?
                                    //strShowCompo
                                    //1 พิมพ์
                                    //2 ไม่พิมพ์
                                    //3 พิมพ์ชุดและรายการย่อย
                                    //this.mstrPFormulaStype = ดูตามฐานข้อมูลชุดสินค้า

                                    if (this.mstrPFormulaStype == "0")
                                    {
                                        strShowCompo = (dtrFormula["fcShowComp"].ToString() == "0" || dtrFormula["fcShowComp"].ToString().TrimEnd() == "" ? App.ActiveCorp.ShowFormulaCompo : dtrFormula["fcShowComp"].ToString());
                                    }
                                    else
                                    {
                                        //strShowCompo = (this.mstrPFormulaStype == 0 ? "1" : "2");
                                        strShowCompo = this.mstrPFormulaStype;
                                    }

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

                                if (dtrPFormI["fcRefPdTyp"].ToString() == SysDef.gc_REFPD_TYPE_FORMULA
                                    && strShowCompo == "1")
                                {
                                    continue;
                                }

                                oPrintField.LoadFieldValue(Report.PrintFieldType.RefProdField, dtrPFormI, ref dtrPrnData);
                                //dtsPrintPreview.FORMOBJ.Rows.Add(dtrPrnData);

                                string strQcProd = dtrPrnData["I2021"].ToString();
                                string strQcUM = dtrPrnData["I2031"].ToString();
                                if (this.mbllIsPrintGroup)
                                {
                                    //if (strLastPrnPrefixValue == (strQcProd + "," + strQcUM))
                                    if (strLastPrnPrefixValue == (dtrPFormI["fcRefPdTyp"].ToString() + "," + strQcProd + "," + strQcUM))
                                    {
                                        //จำนวน
                                        dtrLastRow["I2041"] = Convert.ToDecimal(dtrLastRow["I2041"]) + Convert.ToDecimal(dtrPrnData["I2041"]);
                                        //มูลค่า
                                        dtrLastRow["I2060"] = Convert.ToDecimal(dtrLastRow["I2060"]) + Convert.ToDecimal(dtrPrnData["I2060"]);
                                    }
                                    else
                                    {

                                        //strLastPrnPrefixValue = (strQcProd + "," + strQcUM);
                                        strLastPrnPrefixValue = (dtrPFormI["fcRefPdTyp"].ToString() + "," + strQcProd + "," + strQcUM);

                                        if (dtrLastRow != null)
                                        {
                                            dtsPrintPreview.FORMOBJ.Rows.Add(dtrLastRow);
                                            dtrLastRow = null;
                                        }

                                        dtrLastRow = dtsPrintPreview.FORMOBJ.NewRow();
                                        DataSetHelper.CopyDataRow(dtrPrnData, ref dtrLastRow);
                                    }

                                    if (intRowCount == this.dtsDataEnv.Tables[PrintField.xd_PRN_FORM_I].Rows.Count)
                                    {
                                        if (dtrLastRow != null)
                                        {
                                            dtsPrintPreview.FORMOBJ.Rows.Add(dtrLastRow);
                                            dtrLastRow = null;
                                        }
                                    }
                                }
                                else
                                {
                                    dtsPrintPreview.FORMOBJ.Rows.Add(dtrPrnData);
                                }

                            }
                        }
                    }

                    //26/03/2008 แก้ BUG เรื่องการพิมพ์ชุดสินค้าในรายการสุดท้ายแล้วไม่ออก
                    if (dtrLastRow != null)
                    {
                        dtsPrintPreview.FORMOBJ.Rows.Add(dtrLastRow);
                        dtrLastRow = null;
                    }

                    if (dtsPrintPreview.FORMOBJ.Rows.Count > 0)
                        this.pmPreviewReport(dtsPrintPreview, strRPTFileName);

                }
            }
        }


        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName)
        {
            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(inRPTFileName))
            //{
            //    MessageBox.Show("File not found " + inRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //rptPreviewReport.Load(inRPTFileName);
            //rptPreviewReport.SetDataSource(inData);

            //App.PreviewReport(this, false, rptPreviewReport);
            //xrPSTK_Template01.cs

            Report.RPT.xrPSTK_Template01 oprn = new Report.RPT.xrPSTK_Template01();

            string strRPTFileName1 = inRPTFileName;
            if (System.IO.File.Exists(strRPTFileName1))
                oprn.LoadLayout(strRPTFileName1);
            
            oprn.DataSource = inData;
            oprn.CreateDocument();
            frmDXPreviewReport oPreview = new frmDXPreviewReport();

            oPreview.printControl1.PrintingSystem = oprn.PrintingSystem;

            oPreview.Show();
        }

        private void pmEditReportForm()
        {

            Report.RPT.xrPSTK_Template01 report = new Report.RPT.xrPSTK_Template01();

            //string[] strADir = new string[] { Application.StartupPath + "\\RPT\\PFORM_WHTTAX_01.REPX" };

            string pstrRPTName = "";
            
            //string[] strADir = null;
            //string strFormPath = Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\";

            pstrRPTName = Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\PFORM_" + this.mstrRefType + "_01.REPX";
            using (Transaction.Common.frmRPTSelect dlg = new Transaction.Common.frmRPTSelect())
            {

                string[] strADir = null;
                string strFormPath = Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\";
                if (System.IO.Directory.Exists(strFormPath))
                {
                    strADir = System.IO.Directory.GetFiles(strFormPath);
                    dlg.LoadRPT(strFormPath);
                }

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    pstrRPTName = strADir[dlg.lstRPT.SelectedIndex];
                    //this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);

                    if (System.IO.File.Exists(pstrRPTName))
                        report.LoadLayout(pstrRPTName);

                    frmDXReportDesign designForm = new frmDXReportDesign();

                    try
                    {
                        designForm.OpenReport(report);
                        designForm.FileName = pstrRPTName;
                        designForm.ShowDesignerForm(this);
                        //this.pmShowDesignerForm(designForm, this);
                        if (designForm.FileName != pstrRPTName && System.IO.File.Exists(designForm.FileName))
                            System.IO.File.Copy(designForm.FileName, pstrRPTName, true);
                    }
                    finally
                    {
                        designForm.Dispose();
                        report.Dispose();
                    }
                
                }
            }

        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = dtrBrow[QMFStmoveHDInfo.Field.Code].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFStmoveHDInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFStmoveHDInfo.Field.Code].ToString(), "", ref strErrorMsg))
                    {
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
                    }
                }
            }
            else
            {
                if (strErrorMsg != "")
                    MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.mstrEditRowID = "";
        }

        private bool pmCanEdit(DataRow inChkRow, DocEditType inEditType, bool inShowMsg)
        {
            this.mstrCanEditMsg = "";
            string strMsg1 = "แก้ไข";
            string strMsg2 = "";
            switch (inEditType)
            {
                case DocEditType.Edit:
                    strMsg1 = UIBase.GetAppUIText(new string[] { "แก้ไข", "edit" });
                    break;
                case DocEditType.Delete:
                    strMsg1 = UIBase.GetAppUIText(new string[] { "ลบ", "delete" });
                    break;
            }
            bool bllResult = true;

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            bool bllHasUsed = false;
            if (objSQLHelper.SetPara(new object[] { this.mstrRefType, inChkRow["fcSkid"].ToString() })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select MFWORDERHD.CMSTEP, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERHD where MFWORDERHD.CROWID in (select REFDOC.CCHILDH from REFDOC where CMASTERTYP = ? and REFDOC.CMASTERH = ?) ", ref strErrorMsg))
            {
                DataRow dtrMO = this.dtsDataEnv.Tables["QHasUsed"].Rows[0];
                strMsg2 = dtrMO["cRefNo"].ToString().TrimEnd() + ", " + Convert.ToDateTime(dtrMO["dDate"]).ToString("dd/MM/yy");
                if (dtrMO["cMStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                {
                    bllHasUsed = true;
                }
                else
                {
                    bllHasUsed = false;
                }
            }

            if (bllHasUsed)
            {
                this.mstrCanEditMsg = UIBase.GetAppUIText(new string[] { "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากปิดการผลิตแล้ว" + "\r\nเอกสารใบปิดการผลิตเลขที่/วันที่ : " + strMsg2, "This document has refer to #MC : " + strMsg2 + ". Can not " + strMsg1 });
                bllResult = false;
            }
            else if (inEditType == DocEditType.Edit && inChkRow[QMFStmoveHDInfo.Field.Stat].ToString() == "C")
            {
                this.mstrCanEditMsg = UIBase.GetAppUIText(new string[] { "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากถูกยกเลิกแล้ว", "This document has CANCEL. Can not " + strMsg1 });
                bllResult = false;
            }

            if (!bllResult && inShowMsg)
            {
                MessageBox.Show(this.mstrCanEditMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return bllResult;
        }

        private bool pmCheckHasUsed(string inRowID, string inCode, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strMsg1 = UIBase.GetAppUIText(new string[] { "ไม่สามารถลบได้เนื่องจาก", "Cannot delete because" });
            string strMsg2 = "";
            bool bllHasUsed = false;

            if (objSQLHelper.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select MFWORDERHD.CMSTEP, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERHD where MFWORDERHD.CROWID in (select REFDOC.CCHILDH from REFDOC where CMASTERTYP = ? and REFDOC.CMASTERH = ?) ", ref strErrorMsg))
            {
                DataRow dtrMO = this.dtsDataEnv.Tables["QHasUsed"].Rows[0];
                strMsg2 = dtrMO["cRefNo"].ToString().TrimEnd() + " " + Convert.ToDateTime(dtrMO["dDate"]).ToString("dd/MM/yy");
                if (dtrMO["cMStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                {
                    ioErrorMsg = UIBase.GetAppUIText(new string[] { strMsg1 + " #MO : " + strMsg2 + " ปิดการผลิตแล้ว", strMsg1 + " #MO : " + strMsg2 + " has ref to #MC" });
                    bllHasUsed = true;
                }
                else
                {
                    bllHasUsed = false;
                }
            }

            return bllHasUsed;
        }

        private bool pmDeleteRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
        {
            bool bllIsCommit = false;
            bool bllResult = false;

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

                string strErrorMsg = "";
                object[] pAPara = null;

                //Save NoteCut ไว้เพื่อไล่ Update Order Step ในภายหลัง
                //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                //this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", "select cChildH from REFDOC where CMASTERTYP = ? and CMASTERH = ? group by cChildH", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut2", "NoteCut", "select cChildH from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? group by cChildH", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                ////Delete Note Cut
                //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                //this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC where CMASTERTYP = ? and CMASTERH = ? ", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                //Delete RefDoc_Stmove
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? ", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                //ต้องลบ NoteCut ออกก่อนจึงค่อยวนลูป Update Step จึงจะถูกต้อง
                foreach (DataRow dtrChildH in this.dtsDataEnv.Tables["QNoteCut2"].Rows)
                {
                    //this.pmUpdateOrderHStep(dtrChildH["cChildH"].ToString());
                    switch (this.mDocType)
                    {
                        case DocumentType.FR:
                            this.pmUpdateOrderHStep(dtrChildH["cChildH"].ToString());
                            break;
                        case DocumentType.GD:
                            this.pmUpdateOrderHStep_Stm(dtrChildH["cChildH"].ToString());
                            break;
                    }
                }

                //Update Stock
                this.pmChgItemStat(this.mstrEditRowID, ref strErrorMsg);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where fcGLRef = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

                bllResult = true;
            }
            catch (Exception ex)
            {
                ioErrorMsg = ex.Message;
                bllResult = false;

                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                    this.mdbTran2.Rollback();
                }
                App.WriteEventsLog(ex);
            }
            finally
            {
                this.mdbConn.Close();
            }
            return bllResult;
        }

        private void pmSearchData()
        {
            if (this.gridView1.OptionsView.ShowAutoFilterRow)
            {
                this.gridView1.OptionsView.ShowAutoFilterRow = false;
                this.gridView1.ClearColumnsFilter();
            }
            else
            {
                this.gridView1.OptionsView.ShowAutoFilterRow = true;
            }
        }

        private void pmEnterForm()
        {
            if ((this.mFormActiveMode == FormActiveMode.PopUp || this.mFormActiveMode == FormActiveMode.Report)
                && this.pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
            {
                this.mbllPopUpResult = true;
                this.Hide();
            }
        }

        private int pmGetRowID(string inTag)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[intCnt]["fcSkid"].ToString().TrimEnd() == inTag)
                    return intCnt;
            }
            return -1;
        }

        private bool pmSaveData()
        {
            bool bllResult = false;
            string strErrorMsg = "";
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");

                if (this.mFormActiveMode == FormActiveMode.Edit && this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    this.pmInsertLoop();
                }
                else
                {
                    this.pmGotoBrowPage();
                }

                this.pmPrintData2();

                UIBase.WaitClear();
                bllResult = true;
            }

            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bllResult = false;
            }
            return bllResult;
        }

        /// <summary>
        /// พิมพ์หลังจากที่ Save
        /// </summary>
        private void pmPrintData2()
        {

            if (MessageBox.Show(this, UIBase.GetAppUIText(new string[] {"ต้องการที่จะพิมพ์เลยหรือไม่ ?","Do you want to print ?"}), "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strCode = this.mstrPDocCode;
                using (Transaction.Common.dlgPRange dlg2 = new Transaction.Common.dlgPRange())
                {
                    dlg2.BeginCode = strCode;
                    dlg2.EndCode = strCode;

                    string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\");
                    dlg2.LoadRPT(Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\");
                    
                    dlg2.ShowDialog();
                    if (dlg2.DialogResult == DialogResult.OK)
                    {
                        this.pmPrintRangeData(dlg2.BeginCode, dlg2.EndCode, strADir[dlg2.cmbPForm.SelectedIndex]);
                    }
                }
            }

        }

        private bool pmValidBeforeSave(ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strMsg = "";
            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                strMsg = UIBase.GetAppUIText(new string[] {
                    "ยังไม่ได้ระบุเลขที่ " + this.mstrFormMenuName + " ต้องการให้เครื่อง Running ให้หรือไม่ ?"
                    ,this.mstrFormMenuName + " Code is not define ! " + "Do you want to Auto Running Code ?"});

                if (MessageBox.Show(strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    this.pmRunCode();

                    //if (this.mstrBookRunCodeType.Trim() == string.Empty)
                    //{
                    //    this.pmRunCode();
                    //}
                    //else
                    //{
                    //    this.pmRunCode2();
                    //}

                }
            }

            DateTime dttCorpStartDate = App.ActiveCorp.StartAppDate;
            if (this.mFormEditMode == UIHelper.AppFormState.Edit && this.mbllCanEdit == false)
            {
                ioErrorMsg = this.mstrCanEditMsg;
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุเลขที่เอกสาร", "Document Code is not define ! " });
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtRefNo.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุเลขที่อ้างอิง", "Reference No. is not define ! " });
                this.txtRefNo.Focus();
                return false;
            }
            else if (this.txtQcSect.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุแผนก", "Section is not define ! " });
                this.txtQcSect.Focus();
                return false;
            }
            else if (this.txtQcJob.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุโครงการ", "Job is not define ! " });
                this.txtQcJob.Focus();
                return false;
            }
            else if (dttCorpStartDate.CompareTo(this.txtDate.DateTime.Date) > 0)
            {
                ioErrorMsg = "วันที่เอกสาร" + this.mstrRefTypeName.TrimEnd() + " ไม่ควรเป็นวันที่ก่อนเริ่มใช้ระบบ";
                this.txtDate.Focus();
                return false;
            }
            else if (this.txtFrQcWHouse.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุคลังต้นทาง";
                this.txtFrQcWHouse.Focus();
                return false;
            }
            else if (this.txtToQcWHouse.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุคลังปลายทาง";
                this.txtToQcWHouse.Focus();
                return false;
            }
            else if (this.txtToQcWHouse.Text.Trim() == this.txtFrQcWHouse.Text.Trim())
            {
                ioErrorMsg = "คลังต้นทางกับปลายทางต้องเป็นคนละคลังสินค้ากัน";
                this.txtFrQcWHouse.Focus();
                return false;
            }
            else if (!this.pmChkStockBeforeSave(ref ioErrorMsg))
            {
                this.gridView2.Focus();
                return false;
            }
            else if (!this.pmChkItemOK(ref ioErrorMsg))
            {
                this.gridView2.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "เลขที่เอกสารซ้ำ", "Duplicate Document Code !" });
                this.txtCode.Focus();
                return false;
            }
            else
                bllResult = true;

            return bllResult;
        }

        private bool pmChkItemOK(ref string ioErrorMsg)
        {
            bool bllResult = false;
            bool bllHasFrWHLoca = true;
            bool bllHasNoWHLoca = false;
            bool bllHasNoQty = false;

            DataRow dtrErrRow = null;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                if (dtrTemPd["cProd"].ToString() != string.Empty && Convert.ToDecimal(dtrTemPd["nQty"]) == 0)
                {
                    bllHasNoQty = true;
                    dtrErrRow = dtrTemPd;
                    break;
                }
                if (dtrTemPd["cWHLoca"].ToString().Trim() == string.Empty)
                {
                    bllHasNoWHLoca = true;
                    dtrErrRow = dtrTemPd;
                    break;
                }
            }

            if (bllHasNoQty)
            {
                ioErrorMsg = "รายการที่ " + Convert.ToInt32(dtrErrRow["nRecNo"]).ToString("###") + " ยังไม่ได้ระบุจำนวน save ไม่ได้";
            }
            else if (bllHasNoWHLoca)
            {
                ioErrorMsg = "รายการที่ " + Convert.ToInt32(dtrErrRow["nRecNo"]).ToString("###") + " ยังไม่ได้ระบุที่เก็บสินค้า";
            }
            else
            {
                bllResult = true;
            }

            return bllResult;
        }

        private bool pmChkStockBeforeSave(ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            bool bllResult = true;
            string strWHouse = "";
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;
            dv.Sort = "cQcProd, cLot";

            switch (this.mDocType)
            {
                case DocumentType.WR:
                case DocumentType.WX:
                    strWHouse = this.txtFrQcWHouse.Tag.ToString();
                    break;
                case DocumentType.RW:
                case DocumentType.RX:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
                case DocumentType.FR:
                    strWHouse = this.txtToQcWHouse.Tag.ToString();
                    break;
                case DocumentType.TR:
                    strWHouse = this.txtFrQcWHouse.Tag.ToString();
                    break;
            } 

            string strProdLot = "";
            decimal decStockBal = 0;
            decimal decStockBal1 = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                DataRowView dtrTemPd = dv[i];

                string lcCtrlStock = BizRule.GetProdCtrlStock(pobjSQLUtil, dtrTemPd["cProd"].ToString(), App.ActiveCorp.SCtrlStock);
                switch (this.mDocType)
                {
                    case DocumentType.RW:
                    case DocumentType.RX:
                    case DocumentType.FR:
                        lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                        break;
                } 

                decimal decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                if (strProdLot != dtrTemPd["cQcProd"].ToString().TrimEnd() + dtrTemPd["cLot"].ToString().TrimEnd())
                {
                    strProdLot = dtrTemPd["cQcProd"].ToString().TrimEnd() + dtrTemPd["cLot"].ToString().TrimEnd();
                    decStockBal = this.mStockAgent.GetStockBalance(this.mstrBranch, dtrTemPd["cProd"].ToString(), strWHouse, "", dtrTemPd["cLot"].ToString());
                    //TODO: Next กรณีเปลี่ยน Lot และรหัสสินค้า
                    decStockBal = decStockBal + Convert.ToDecimal(dtrTemPd["nOldQty"]);
                    decStockBal1 = decStockBal;
                }

                if (lcCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE
                    && decStockBal - decQty < 0)
                {
                    decimal decTotQty = this.pmSumQtyByQcProd(dtrTemPd["cQcProd"].ToString(), dtrTemPd["cLot"].ToString());
                    strErrorMsg += (strErrorMsg.Trim() == "" ? "" : "\r\n") + UIBase.GetAppUIText(new string[] { "สินค้า : (", "Product/RawMat : (" }) + dtrTemPd["cQcProd"].ToString().TrimEnd() + ") " + dtrTemPd["cQnProd"].ToString().TrimEnd();
                    strErrorMsg += "\r\n" + UIBase.GetAppUIText(new string[] { "ล๊อต : ", "Lot : " }) + dtrTemPd["cLot"].ToString();
                    strErrorMsg += "\r\n" + UIBase.GetAppUIText(new string[] { "จำนวนรวม : ", "Total Qty. : " }) + decTotQty.ToString("#,###,###,##0.00");
                    strErrorMsg += "\r\n" + UIBase.GetAppUIText(new string[] { "จำนวนคงเหลือ : ", "Balance Qty. : " }) + decStockBal1.ToString("#,###,###,##0.00");
                    strErrorMsg += "\r\n------------------------------------------------------------------";
                    bllResult = false;
                }
                decStockBal = decStockBal - decQty;
            }
            ioErrorMsg = strErrorMsg;

            dv.Sort = "";
            return bllResult;
        }

        private decimal pmSumQtyByQcProd(string inQcProd, string inLot)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cQcProd = '" + inQcProd + "' and cLot = '" + inLot + "'");
            decimal decSumQty = 0;
            for (int i = 0; i < dtrSel.Length; i++)
            {
                decSumQty += Convert.ToDecimal(dtrSel[i]["nQty"]);
            }
            return decSumQty;
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
            int intYear = this.txtDate.DateTime.Year + 543;
            string strDateTime1 = AppUtil.StringHelper.Right(intYear.ToString("0000"), 2) + this.txtDate.DateTime.ToString("MM");
            string strDateTime2 = AppUtil.StringHelper.Right(intYear.ToString("0000"), 2) + this.txtDate.DateTime.ToString("MMdd");

            switch (this.mstrRunCodeStyle)
            {
                case "1":
                    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook };
                    strSQLStr = "select top 1 fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";
                    break;
                case "2":
                    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, strDateTime1 + "%" };
                    strSQLStr = "select top 1 fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and GLREF.FCCODE like ? and fcCode < ':' order by fcCode desc";
                    break;
                case "3":
                    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, strDateTime2 + "%" };
                    strSQLStr = "select top 1 fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and GLREF.FCCODE like ? and fcCode < ':' order by fcCode desc";
                    break;
                default:
                    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook };
                    strSQLStr = "select top 1 fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";
                    break;
            }

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
                intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : this.txtCode.Properties.MaxLength);
                this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));

            }
            else
            {
                switch (this.mstrRunCodeStyle)
                {
                    case "":
                    case "1":
                        intRunCode++;
                        intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : this.txtCode.Properties.MaxLength);
                        this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
                        break;
                    case "2":
                        strLastRunCode = strDateTime1 + "0000";
                        intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                        this.txtCode.Text = intRunCode.ToString();
                        break;
                    case "3":
                        strLastRunCode = strDateTime2 + "0000";
                        intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                        this.txtCode.Text = intRunCode.ToString();
                        break;
                    default:
                        intRunCode++;
                        intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : this.txtCode.Properties.MaxLength);
                        this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
                        break;
                }
            }

            if (this.txtRefNo.Text.TrimEnd() == string.Empty)
            {
                string strRefNo = this.mstrBookPrefix + this.txtCode.Text.TrimEnd();
                this.txtRefNo.Text = strRefNo;
                //this.txtRefNo.Text = this.mstrRefType + this.mstrQcBook + "/" + this.txtCode.Text.TrimEnd();
            }
            return true;
        }

        private bool SAV_pmRunCode()
        {

            //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch }, this.txtCode.MaxLength);

            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            long intRunCode = 1;
            int inMaxLength = this.txtCode.Properties.MaxLength;

            string strSQLStr = "select fcCode from " + this.mstrRefTable + " where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[4] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook };
            objSQLHelper.SetPara(pAPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
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
            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : 7);
            //intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length-3 : this.txtCode.MaxLength-3);

            this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            if (this.txtRefNo.Text.TrimEnd() == string.Empty)
            {
                this.txtRefNo.Text = this.mstrRefType + this.mstrQcBook + "/" + this.txtCode.Text.TrimEnd();
            }
            return true;
        }

        private bool pmRunCode2()
        {

            //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch }, this.txtCode.MaxLength);

            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            long intRunCode = 1;
            int inMaxLength = this.mintBookRunCodeLen;

            string strCodePreFix = "";
            int intYear = this.txtDate.DateTime.Year;
            int intMonth = this.txtDate.DateTime.Month;
            if (this.mstrBookRunCodeType == "1")
            {
                intYear = intYear + 543;
            }

            strCodePreFix = StringHelper.Right(intYear.ToString("0000"), 2) + intMonth.ToString("00");

            string strSQLStr = "select fcCode from " + this.mstrRefTable + " where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and year(fdDate) = ? and month(fdDate) = ? and fcCode < ':' order by fcCode desc";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, this.txtDate.DateTime.Year, this.txtDate.DateTime.Month };
            objSQLHelper.SetPara(pAPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["fcCode"].ToString().Trim();
                try
                {
                    intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode = Convert.ToInt32(strCodePreFix + intRunCode.ToString(StringHelper.Replicate("0", mintBookRunCodeLen)));
                    //intRunCode++;
                }
            }
            else
            {
                intRunCode = Convert.ToInt32(strCodePreFix + intRunCode.ToString(StringHelper.Replicate("0", mintBookRunCodeLen)));
            }

            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : intCodeLen);
            //intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length-3 : this.txtCode.MaxLength-3);

            this.txtCode.Text = intRunCode.ToString();
            if (this.txtRefNo.Text.TrimEnd() == string.Empty)
            {
                this.txtRefNo.Text = this.mstrBookPrefix.Trim() + this.txtCode.Text.TrimEnd();
            }
            return true;
        }

        private bool pmIsValidateCode(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cCode = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateName(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cName = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private void pmUpdateRecord()
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            this.mstrPDocCode = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
            DataRow dtrSaveInfo = null;
            //if (this.mFormEditMode == UIHelper.AppFormState.Insert
            //    || (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
            //    && !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcSkid from " + this.mstrRefTable + " where fcSkid = ?", ref strErrorMsg)))
            //2012-10-4 By Yod Optimize Save Speed
            if (this.mFormEditMode == UIHelper.AppFormState.Insert)
            {
                objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcSkid from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);
                bllIsNewRow = true;
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
                if (this.mstrEditRowID == string.Empty)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
                }
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateAp] = App.AppID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateBy] = App.FMAppUserID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Datetime] = objSQLHelper.GetDBServerDateTime();
                this.dtsDataEnv.Tables[this.mstrRefTable].Rows.Add(dtrSaveInfo);
            }
            else
            {
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
            }

			string strCode = this.txtCode.Text.TrimEnd();
			if (strCode.Length > 3)
				strCode = StringHelper.Left(strCode, strCode.Length-3);

            this.mstrSaveRowID = this.mstrEditRowID;
            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[QMFStmoveHDInfo.Field.RowID] = this.mstrEditRowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.CorrectBy] = App.FMAppUserID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.LastUpd] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFStmoveHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Rftype] = this.mstrRfType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BookID] = this.mstrBook;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Step] = this.mstrStep;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.RefNo] = this.txtRefNo.Text.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.Date] = this.txtDate.DateTime.Date;
            dtrSaveInfo[QMFStmoveHDInfo.Field.FrWHouse] = this.txtFrQcWHouse.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.ToWHouse] = this.txtToQcWHouse.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            //dtrSaveInfo[QMFStmoveHDInfo.Field.DeptID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.JobID] = this.txtQcJob.Tag.ToString();
            //dtrSaveInfo[QMFStmoveHDInfo.Field.ProjID] = this.txtQcJob.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.LUpdApp] = App.AppID;
            dtrSaveInfo["fcDataser"] = "";	//fcDataser
            dtrSaveInfo["fcEAfterR"] = "E";

            dtrSaveInfo[QMFStmoveHDInfo.Field.CoorID] = this.txtQcCoor.Tag.ToString();

            string gcTemStr01 = BizRule.SetMemData(this.txtRemark.Text.TrimEnd(), "Rem");
            gcTemStr01 += BizRule.SetMemData(pARemark[0], "Rm2");
            gcTemStr01 += BizRule.SetMemData(pARemark[1], "Rm3");
            gcTemStr01 += BizRule.SetMemData(pARemark[2], "Rm4");
            gcTemStr01 += BizRule.SetMemData(pARemark[3], "Rm5");
            gcTemStr01 += BizRule.SetMemData(pARemark[4], "Rm6");
            gcTemStr01 += BizRule.SetMemData(pARemark[5], "Rm7");
            gcTemStr01 += BizRule.SetMemData(pARemark[6], "Rm8");
            gcTemStr01 += BizRule.SetMemData(pARemark[7], "Rm9");
            gcTemStr01 += BizRule.SetMemData(pARemark[8], "RmA");

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata] = gcTemStr02;
            if (DataSetHelper.HasField("fmMemData2", dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata2] = gcTemStr03;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata3] = gcTemStr04;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata4] = gcTemStr05;
            }

            if (DataSetHelper.HasField(QMFStmoveHDInfo.Field.Memdata5, dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata5] = gcTemStr06;
            }

            this.mstrPDocCode = this.txtCode.Text.TrimEnd();

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

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mstrRefToWOrderI = "";

                dtrSaveInfo["ftLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();

                decimal decDiscAmtI = 0;
                this.pmSaveRefProd(this.mstrEditRowID, ref decDiscAmtI);

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    this.pmUpdateStockForInsert();
                }

                dtrSaveInfo["fnDiscAmtI"] = decDiscAmtI;

                switch (this.mDocType)
                {
                    case DocumentType.FR:
                        this.pmSaveTemRefTo();
                        break;
                    case DocumentType.GD:
                        this.pmSaveTemRefTo_Stm();
                        break;
                }

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    if (this.mstrOldCode == this.txtCode.Text && this.mstrOldRefNo == this.txtRefNo.Text)
                    {
                        KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);
                    }
                    else 
                    {
                        KeepLogAgent.KeepLogChgValue(objSQLHelper2, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldRefNo);
                    }
                }

			}
			catch (Exception ex)
			{
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                    this.mdbTran2.Rollback();
                }
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

        private bool pmSaveRefProd(string ioErrorMsg, ref decimal ioDiscAmtI)
        {
            bool bllResult = true;
            string strRowID = "";
            string strOutRowID = "";
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

                if ((dtrTemPd["cFormula"].ToString().TrimEnd() != string.Empty || dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                    && Convert.ToDecimal(dtrTemPd["nQty"]) != 0)
                //&& (Convert.ToDecimal(dtrTemPd["nQty"]) != 0 ? true : MessageBox.Show("สินค้า " + dtrTemPd["cQcProd"].ToString().TrimEnd() + "\nยังไม่ได้ระบุจำนวนต้องการ Save ด้วยหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    //if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                    //	|| (pobjSQLUtil.SetPara( new object[1] { dtrTemPd["cRowID"].ToString() } ) 
                    //	&& !pobjSQLUtil.SQLExec(ref this.dtsDataEnv,"QChkRefProd", "RefProd", "select fcSkid from RefProd where fcSkid = ?", ref strErrorMsg)))
                    //{

                    decimal decQty = 0;
                    decimal decUmQty = 0;
                    decimal decStQty = 0;
                    string strWHouse = "";
                    string strSaveRefDocRowID = "";

                    DataRow dtrRRefProd = null;

                    #region "Save รายการฝั่ง Out"
                    //if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                    //    & (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        strRowID = App.mRunRowID("RefProd");
                        bllIsNewRow = true;
                        decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                        decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                        decStQty = 0;
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        bllIsNewRow = false;
                        strRowID = dtrTemPd["cRowID"].ToString();
                        dtrRRefProd = this.dtsDataEnv.Tables["RRefProd"].Rows[0];
                        strWHouse = (dtrRRefProd["fcIOType"].ToString() == "I" ? this.txtToQcWHouse.Tag.ToString() : this.txtFrQcWHouse.Tag.ToString());

                        if (dtrTemPd["cProd"].ToString() + strWHouse + dtrTemPd["cLot"].ToString() == dtrRRefProd["fcProd"].ToString() + dtrRRefProd["fcWHouse"].ToString() + dtrRRefProd["fcLot"].ToString())
                        {

                            decQty = (Convert.ToDecimal(dtrTemPd["nUOMQty"]) == 0 ? 0 : ((Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nUOMQty"])) - (Convert.ToDecimal(dtrRRefProd["fnQty"]) * Convert.ToDecimal(dtrRRefProd["fnUmQty"]))) / Convert.ToDecimal(dtrTemPd["nUOMQty"]));
                            decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                            decStQty = 0;
                        }
                        else
                        {
                            decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                            decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                            decStQty = 0;
                            //this.pmRetStock(dtrRRefProd, this.txtFrQcWHLoca.RowID, true, false, "", ref strErrorMsg);

                            //TODO: Test Update Stock
                            //this.pmRetStock(dtrRRefProd, dtrTemPd["cFrWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                        }
                    }

                    //รายการฝั่ง Out
                    dtrTemPd["cRowID"] = strRowID;
                    strOutRowID = strRowID;
                    DataRow dtrRefProd = null;
                    this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, false, decQty, decUmQty, 0, 0);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);


                    #region "Save xaRefProd"

                    //if ((dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    //    && (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "xaRefProd", "xaRefProd", "select cRowID from xaRefProd where cRefProd = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2)))
                    //{
                    //    strRowID = this.dtsDataEnv.Tables["xaRefProd"].Rows[0]["cRowID"].ToString();
                    //    bllIsNewRow = false;
                    //}
                    //else
                    //{
                    //    strRowID = App.mRunRowID("xaRefProd");
                    //    bllIsNewRow = true;
                    //}

                    //DataRow dtrXaRefProd = null;
                    //this.pmReplRecordXaRefProd(bllIsNewRow, strRowID, dtrTemPd["cRowID"].ToString(), dtrRefProd["fcWHouse"].ToString(), "O", dtrTemPd, ref dtrXaRefProd);

                    //strSQLUpdateStr = "";
                    //cDBMSAgent.GenUpdateSQLString(dtrXaRefProd, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    //this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    #endregion


                    #endregion

                    if (this.mstrRefType == "WR" || this.mstrRefType == "GD")
                    {
                        strSaveRefDocRowID = strRowID;
                    }

                    #region "Save รายการฝั่ง In"

                    //if ((dtrTemPd["cRowID2"].ToString().TrimEnd() == string.Empty)
                    //    & (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    if (dtrTemPd["cRowID2"].ToString().TrimEnd() == string.Empty)
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        strRowID = App.mRunRowID("RefProd");
                        bllIsNewRow = true;
                        decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                        decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                        decStQty = 0;
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        bllIsNewRow = false;
                        strRowID = dtrTemPd["cRowID2"].ToString();
                        dtrRRefProd = this.dtsDataEnv.Tables["RRefProd"].Rows[0];
                        strWHouse = (dtrRRefProd["fcIOType"].ToString() == "I" ? this.txtToQcWHouse.Tag.ToString() : this.txtFrQcWHouse.Tag.ToString());

                        if (dtrTemPd["cProd"].ToString() + strWHouse + dtrTemPd["cLot"].ToString() == dtrRRefProd["fcProd"].ToString() + dtrRRefProd["fcWHouse"].ToString() + dtrRRefProd["fcLot"].ToString())
                        {

                            decQty = (Convert.ToDecimal(dtrTemPd["nUOMQty"]) == 0 ? 0 : ((Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nUOMQty"])) - (Convert.ToDecimal(dtrRRefProd["fnQty"]) * Convert.ToDecimal(dtrRRefProd["fnUmQty"]))) / Convert.ToDecimal(dtrTemPd["nUOMQty"]));
                            decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                            decStQty = 0;
                        }
                        else
                        {
                            decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                            decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                            decStQty = 0;
                            //this.pmRetStock(dtrRRefProd, this.txtToQcWHLoca.RowID, true, false, "", ref strErrorMsg);

                            //TODO: Test Update Stock
                            //this.pmRetStock(dtrRRefProd, dtrTemPd["cToWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                        }
                    }

                    //รายการฝั่ง IN
                    dtrTemPd["cRowID2"] = strRowID;
                    dtrRefProd = null;
                    this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, true, decQty, decUmQty, 0, 0);

                    strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    #region "Save xaRefProd"

                    //if ((dtrTemPd["cRowID2"].ToString().TrimEnd() != string.Empty)
                    //    && (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "xaRefProd", "xaRefProd", "select cRowID from xaRefProd where cRefProd = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2)))
                    //{
                    //    bllIsNewRow = false;
                    //    strRowID = this.dtsDataEnv.Tables["xaRefProd"].Rows[0]["cRowID"].ToString();
                    //}
                    //else
                    //{
                    //    strRowID = App.mRunRowID("xaRefProd");
                    //    bllIsNewRow = true;
                    //}

                    //dtrXaRefProd = null;
                    //this.pmReplRecordXaRefProd(bllIsNewRow, strRowID, dtrTemPd["cRowID2"].ToString(), dtrRefProd["fcWHouse"].ToString(), "I", dtrTemPd, ref dtrXaRefProd);

                    //strSQLUpdateStr = "";
                    //cDBMSAgent.GenUpdateSQLString(dtrXaRefProd, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    //this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    #endregion


                    #endregion

                    if (this.mstrRefType == "FR"
                        || this.mstrRefType == "RW")
                    {
                        strSaveRefDocRowID = strRowID;
                    }

                    this.pmSaveRefDoc(dtrTemPd, strSaveRefDocRowID);

                    ////Save RefPdX3
                    if (this.mbllIsGetPdSer)
                    {
                        this.mPdSer.SetFromWHouse(this.txtFrQcWHouse.Tag.ToString(), dtrTemPd["cFrWHLoca"].ToString());
                        this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                        //this.mPdSer.SaveRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID2"].ToString(), this.txtToQcWHouse.Tag.ToString(), this.txtToQcWHLoca.RowID, dtrTemPd["cLot"].ToString());
                        this.mPdSer.SaveRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID2"].ToString(), this.txtToQcWHouse.Tag.ToString(), dtrTemPd["cToWHLoca"].ToString(), dtrTemPd["cLot"].ToString());
                    }

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
                            this.pmRetStock(dtrRefProd2, "", false, false, "", ref strErrorMsg);
                        }

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from RefProd where FCSKID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    }

                    if (dtrTemPd["cRowID2"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[1] { dtrTemPd["cRowID2"].ToString() };
                        if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefProd", this.mstrITable, "select * from " + this.mstrITable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                        {
                            DataRow dtrRefProd2 = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                            this.pmRetStock(dtrRefProd2, "", false, false, "", ref strErrorMsg);
                        }

                        pAPara = new object[] { dtrTemPd["cRowID2"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from RefProd where FCSKID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                    //Delete RefPdX3
                    if (this.mbllIsGetPdSer)
                    {
                        this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                        //this.mPdSer.DelItemRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID"].ToString(), this.txtFrQcWHouse.Tag.ToString(), this.txtFrQcWHLoca.RowID, dtrTemPd["cLot"].ToString());
                        this.mPdSer.DelItemRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID"].ToString(), this.txtFrQcWHouse.Tag.ToString(), dtrTemPd["cFrWHLoca"].ToString(), dtrTemPd["cLot"].ToString());
                    }

                }
            }
            return true;
        }

        private bool pmReplRecordRefProd
            (
            bool inState, DataRow inTemPd, ref DataRow ioRefProd, string inRowID,
            bool inToWHouse, decimal inQty, decimal inUmQty, decimal inStQty, decimal inStUmQty
            )
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrRefProd;
            if (bllIsNewRec)
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].NewRow();
                dtrRefProd["fcSkid"] = inRowID;
                dtrRefProd["fcCreateAp"] = App.AppID;
            }
            else
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
            }

            string strWHouse = "";
            string strIOType = "";
            if (inToWHouse)
            {
                strWHouse = txtToQcWHouse.Tag.ToString();
                strIOType = "I";
            }
            else
            {
                strWHouse = this.txtFrQcWHouse.Tag.ToString();
                strIOType = "O";
            }

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

            dtrRefProd["fcCorp"] = App.ActiveCorp.RowID;
            dtrRefProd["fcBranch"] = this.mstrBranch;
            dtrRefProd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
            dtrRefProd["fcRefType"] = dtrGLRef["fcRefType"].ToString();
            dtrRefProd["fcRfType"] = dtrGLRef["fcRfType"].ToString();
            dtrRefProd["fcStat"] = dtrGLRef["fcStat"].ToString();
            dtrRefProd["fdDate"] = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;
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
            dtrRefProd["fcWhouse"] = strWHouse;
            dtrRefProd["fcIoType"] = strIOType;
            //dtrRefProd["fcSect"] = (inTemPd["cDept"].ToString() != string.Empty ? inTemPd["cDept"].ToString() : dtrGLRef["fcSect"].ToString());
            //dtrRefProd["fcDept"] = (inTemPd["cDivision"].ToString() != string.Empty ? inTemPd["cDivision"].ToString() : dtrGLRef["fcDept"].ToString());
            //dtrRefProd["fcJob"] = (inTemPd["cJob"].ToString() != string.Empty ? inTemPd["cJob"].ToString() : dtrGLRef["fcJob"].ToString());
            //dtrRefProd["fcProj"] = (inTemPd["cProject"].ToString() != string.Empty ? inTemPd["cProject"].ToString() : dtrGLRef["fcProj"].ToString());

            dtrRefProd["fcSect"] = dtrGLRef["fcSect"].ToString();
            dtrRefProd["fcDept"] = dtrGLRef["fcDept"].ToString();
            dtrRefProd["fcJob"] = dtrGLRef["fcJob"].ToString();
            dtrRefProd["fcProj"] = dtrGLRef["fcProj"].ToString();

            dtrRefProd["fcWHLoca"] = inTemPd["cWHLoca"].ToString();

            dtrRefProd["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrRefProd["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrRefProd["fnXRate"] = 1;
            dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmUpdateStock(inQty, inUmQty, inStQty, inStUmQty, ref dtrRefProd, inTemPd);
                string strWHLoca = "";
                if (!bllIsNewRec)
                    this.pmRetStock(dtrRefProd, strWHLoca, true, false, "", ref strErrorMsg);
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

            dtrRefProd["fnAgeLong"] = Convert.ToDecimal(inTemPd["fnAgeLong"]);

            if (Convert.IsDBNull(inTemPd["fdExpire"]))
            {
                dtrRefProd["fdExpire"] = Convert.DBNull;
            }
            else
            {
                dtrRefProd["fdExpire"] = Convert.ToDateTime(inTemPd["fdExpire"]);
            }

            ioRefProd = dtrRefProd;
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
                //รายการฝั่ง Out
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

                dtrRefProd = null;
                //รายการฝั่ง In
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

            }
            return llSucc;
        }

        private bool pmUpdateStock(decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, ref DataRow ioRefProd, DataRow inTemPd)
        {
            string strErrorMsg = "";
            bool bllSucc = false;
            decimal lnQty = tninQty;
            decimal lnUmQty = tninUmQty;
            decimal lnStQty = tninStQty;
            decimal lnStUmQty = tninStUmQty;
            decimal lnAvgCost = 0;
            decimal lnCostAmt = 0;

            DataRow dtrRefProd = ioRefProd;
            DataRow dtrTemPd = inTemPd;

            decimal lnUpdStockSign = (dtrRefProd["fcIoType"].ToString() == "I" ? 1 : -1);
            string lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);

            if (dtrRefProd["fcIoType"].ToString() == "I"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrToWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }
            else if (dtrRefProd["fcIoType"].ToString() == "O"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }

            decimal decOutStockQty = 0;
            decimal decOutWHLocaQty = 0;

            string strWHLoca = "";

            dtrRefProd["fnCostAmt"] = (Convert.IsDBNull(dtrRefProd["fnCostAmt"]) ? 0 : this.pmToDecimal(dtrRefProd["fnCostAmt"]));

            if (this.mstrFrWhType == SysDef.gc_WHOUSE_TYPE_NORMAL
                && this.mstrRefType != SysDef.gc_REFTYPE_TRANSFER)
            {
                if (dtrRefProd["fcIoType"].ToString() == "O")
                {
                    lnCostAmt = 0;

                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
                        lnUpdStockSign
                        , this.mstrBranch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
                    );

                    lnCostAmt = Math.Round(lnAvgCost * this.pmToDecimal(dtrRefProd["fnQty"]) * this.pmToDecimal(dtrRefProd["fnUmQty"]), 4);
                }
                else
                {
                    lnCostAmt = this.pmToDecimal(dtrTemPd["nCostAmt"]);
                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
                        lnUpdStockSign
                        , this.mstrBranch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
                        );

                    lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nCostAmt"]), 4);

                }
                if (bllSucc)
                {
                    dtrRefProd["fnPrice"] = (this.pmToDecimal(dtrRefProd["fnQty"]) != 0 ? Math.Round(lnCostAmt / this.pmToDecimal(dtrRefProd["fnQty"]), 4) : 0);
                }
            }
            else
            {
                decimal lnUpdCostAmt = Math.Round(this.pmToDecimal(dtrRefProd["fnPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4) - this.pmToDecimal(dtrRefProd["fnCostAmt"]);	//กรณีแก้ไข เพิ่ม/ลด จำนวน จะ Update เฉพาะจำนวนที่ เพิ่ม/ลด เท่านั้น เหมือน Qty
                this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                bllSucc = this.mStockAgent.UpdateStock
                    (
                #region "UpdateStock Parameter"
                    lnUpdStockSign
                    , this.mstrBranch
                    , dtrRefProd["fcProdType"].ToString()
                    , dtrRefProd["fcProd"].ToString()
                    , dtrRefProd["fcWhouse"].ToString()
                    , strWHLoca
                    , dtrRefProd["fcLot"].ToString()
                    , lcCtrlStock
                    , lnQty
                    , lnUmQty
                    , dtrTemPd["cUOM"].ToString()
                    , dtrTemPd["cQnUOM"].ToString()
                    , lnUpdCostAmt
                    , ref lnAvgCost
                    , lnStQty * lnStUmQty
                    , ref decOutStockQty
                    , ref decOutWHLocaQty
                    , false
                    , false
                    , ""
                    , dtrRefProd["fcIoType"].ToString() == "I"
                    , 0
                    , false
                    , false
                    , ""
                    , ref strErrorMsg
                #endregion
                    );

                lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4);
            }
            if (bllSucc)
            {
                if (dtrRefProd["fcIOType"].ToString() == "O")
                {
                    dtrTemPd["nCostAmt"] = lnCostAmt;
                }
                dtrRefProd["fnCostAmt"] = lnCostAmt;
                dtrRefProd["fnPriceKe"] = dtrRefProd["fnPrice"];
                dtrRefProd["fnXRate"] = 1;
            }

            return bllSucc;
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

                if (inRefProd["fcIoType"].ToString() == "I"
                    && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrToWhType) > -1))
                {
                    lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                }
                else if (inRefProd["fcIoType"].ToString() == "O"
                    && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1))
                {
                    lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                }

                decimal lnUpdSign = (inRefProd["fcIoType"].ToString() == "I" ? -1 : 1);

                decimal decAvgCost = 0;
                decimal decOutStockQty = 0;
                decimal decOutWHLocaQty = 0;
                string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	// ระดับหน่วยคุมสต็อค

                this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                llSucc = this.mStockAgent.UpdateStock
                    (
                #region "UpdateStock Parameter"
                      lnUpdSign
                    , this.mstrBranch
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
            object[] pAPara = null;

            //RefItem ของ RefPdX3 ตอนลบต้องดูจากขา In (fcIOType = 'I') เสมอ
            //แต่ WHouse, WHLoca ต้องดูจากขา Out
            //Ex. เบิกจากคลัง A=>B
            //Out = A;In = B
            //RefPdX3 เก็บ Current WHouse คือ B
            //ตอนลบก็ต้อง Delete RefPdX3 ที่ WHouse คือ B (Out) แล้ว Update WHouse เป็น A (In)
            string strRefItem = "";

            DataRow dtrGLRef = null;
            this.mSaveDBAgent.SetPara(new object[] { inGLRef });
            if (this.mSaveDBAgent.SQLExec(ref this.dtsDataEnv, this.mstrHTable, "GLRef", "select * from GLRef where fcSkid = ?", ref strErrorMsg))
            {
                dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
            }

            string strSQLStrRefProd = "select * from RefProd where RefProd.fcGLRef = ? order by RefProd.fcSeq, RefProd.fcIOType ";
            this.mSaveDBAgent.SetPara(new object[] { inGLRef });
            if (this.mSaveDBAgent.SQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", strSQLStrRefProd, ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {

                    decimal lnUpdSign = -1 * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                    if (dtrRefProd["fcProd"].ToString() == string.Empty)
                    {
                        continue;
                    }

                    string lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);
                    string lcStCtrlStock = App.ActiveCorp.SStCtrlStock;	// ระดับหน่วยคุมสต็อค

                    if (dtrRefProd["fcIoType"].ToString() == "I"
                        && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrToWhType) > -1))
                    {
                        lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                    }
                    else if (dtrRefProd["fcIoType"].ToString() == "O"
                        && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1))
                    {
                        lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                    }

                    if (dtrRefProd["fcIoType"].ToString() == "I")
                    {
                        strRefItem = dtrRefProd["fcSkid"].ToString();
                    }

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

                    string strWHLoca = "";

                    if (dtrRefProd["fcIOType"].ToString() == "O")
                    {
                        strWHLoca = dtrRefProd["FCWHLOCA"].ToString();
                        this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                        this.mPdSer.DelItemRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrRefProd["fcProd"].ToString(), strRefItem, dtrGLRef["fcFrWhouse"].ToString(), strWHLoca, dtrRefProd["fcLot"].ToString());
                    }

                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    llSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
                          lnUpdSign
                        , this.mstrBranch
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

            decimal lnUpdSign = -1;
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
                    pobjSQLUtil2.SetPara(new object[1] { dtrRefProd["fcSkid"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QXaRefProd", "xaRefProd", "select cWHLoca from xaRefProd where cRefProd = ?", ref strErrorMsg))
                    {
                        strWHLoca = this.dtsDataEnv.Tables["QXaRefProd"].Rows[0]["cWHLoca"].ToString();
                    }

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
                                , this.mstrBranch
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
                                , this.mstrBranch
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

        private void pmSaveRefDoc(DataRow inTemPd, string inMasterI)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != inTemPd["cXRefToRowID"].ToString().TrimEnd()
                && inTemPd["cXRefToRowID"].ToString().TrimEnd() != string.Empty)
            {
                //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString() };
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inMasterI, inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString() };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != string.Empty
                && Convert.ToDecimal(inTemPd["nQty"]) != 0)
            {
                bool bllIsNewRow_RefDoc = false;
                DataRow dtrRefDoc = null;
                string strRowID = "";
                //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), this.mstrRefToRefType, inTemPd["cRefToHRowID"].ToString(), inTemPd["cRefToRowID"].ToString() };
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inMasterI, this.mstrRefToRefType, inTemPd["cRefToHRowID"].ToString(), inTemPd["cRefToRowID"].ToString() };
                string strRefDoc = "select * from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?";
                if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefDoc_Stmove, MapTable.Table.RefDoc_Stmove, strRefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                {
                    strRowID = App.mRunRowID(MapTable.Table.RefDoc_Stmove);
                    bllIsNewRow_RefDoc = true;
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].NewRow();
                    dtrRefDoc["nUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
                    dtrRefDoc["cRowID"] = strRowID;
                    dtrRefDoc["tDatetime"] = this.mSaveDBAgent2.GetDBServerDateTime();
                    //dtrRefDoc["cCreateAp"] = App.AppID;
                }
                else
                {
                    strRowID = inTemPd["cRefToRowID"].ToString();
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].Rows[0];
                }
                dtrRefDoc["cMastertyp"] = this.mstrRefType;
                dtrRefDoc["cMasterH"] = this.mstrEditRowID;
                //dtrRefDoc["cMasterI"] = inTemPd["cRowID"].ToString();
                dtrRefDoc["cMasterI"] = inMasterI;
                dtrRefDoc["cCorp"] = App.ActiveCorp.RowID;
                dtrRefDoc["cBranch"] = this.mstrBranch;
                dtrRefDoc["cPlant"] = this.mstrPlant;
                dtrRefDoc["cChildType"] = this.mstrRefToRefType;
                dtrRefDoc["cChildH"] = inTemPd["cRefToHRowID"].ToString();
                dtrRefDoc["cChildI"] = inTemPd["cRefToRowID"].ToString();
                dtrRefDoc["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
                dtrRefDoc["nUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);

                string strSQLUpdateStr_RefDoc = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefDoc, "cRowID", bllIsNewRow_RefDoc, ref strSQLUpdateStr_RefDoc, ref pAPara);
                this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr_RefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                //pobjSQLUtil.SetPara(pAPara);
                //pobjSQLUtil.SQLExec(strSQLUpdateStr_RefDoc, ref strErrorMsg);
            }
            else
            {
                //Delete Note Cut
                //pobjSQLUtil.SetPara(new object[] {this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString()});
                //pobjSQLUtil.SQLExec("delete from REFDOC_STMOVE where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", ref strErrorMsg);

                //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString() };
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inMasterI };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
        }

        private void pmSaveTemRefTo()
        {
            if (this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Count > 0)
            {
                foreach (DataRow dtrRefToRow in this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows)
                {
                    if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                        this.pmUpdateOrderHStep(dtrRefToRow["cOrderH"].ToString());
                }
            }
        }

        private void pmSaveTemRefTo_Stm()
        {
            if (this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Count > 0)
            {
                foreach (DataRow dtrRefToRow in this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows)
                {
                    if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                        this.pmUpdateOrderHStep_Stm(dtrRefToRow["cOrderH"].ToString());
                }
            }
        }

        private void pmUpdateDeleteTemRefTo() { }

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
            string strFoxSQLStrREFDOC_STMOVE = "select * from REFDOC_STMOVE where REFDOC_STMOVE.cChildtype = ? and REFDOC_STMOVE.cChildH = ? and REFDOC_STMOVE.cChildI = ?";
            pAPara = new object[] { inRefType, inChildH, inChildI };
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QREFDOC_STMOVE", "REFDOC_STMOVE", strFoxSQLStrREFDOC_STMOVE, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrREFDOC_STMOVE in this.dtsDataEnv.Tables["QREFDOC_STMOVE"].Rows)
                {
                    //pobjSQLUtil.SetPara(new object[] { dtrREFDOC_STMOVE["fcMasterH"].ToString() });
                    pAPara = new object[] { dtrREFDOC_STMOVE["cChildH"].ToString() };
                    //if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select fcStat from GLREF where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select fcStat from ORDERH where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        if (this.dtsDataEnv.Tables["QRefTo"].Rows[0]["fcStat"].ToString() != "C")
                            decCutQty += Convert.ToDecimal(dtrREFDOC_STMOVE["nQty"]) * Convert.ToDecimal(dtrREFDOC_STMOVE["nUmQty"]) / (inUOMQty != 0 ? inUOMQty : 1);
                    }
                }
            }
            return decCutQty;
        }

        private void pmUpdateOrderHStep_Stm(string inOrderH)
        {
            string strErrorMsg = "";
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFoxSQLStrOrderI = "select * from StmReqI where StmReqI.fcStmReqH = ? and FCIOTYPE = 'I' ";

            string strHStep = SysDef.gc_REF_STEP_PAY;
            //pobjSQLUtil.SetPara(new object[] { inOrderH });
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefToI", "ORDERI", strFoxSQLStrOrderI, new object[] { inOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefToI"].Rows)
                {
                    decimal decCutQty = 0;
                    decimal decQty = Convert.ToDecimal(dtrOrderI["fnQty"]);
                    //Sum Cut Qty
                    decCutQty = this.pmSumCutQty_Stm(dtrOrderI["fcRefType"].ToString(), inOrderH, dtrOrderI["fcSkid"].ToString(), Convert.ToDecimal(dtrOrderI["fnUMQty"]));
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
                    string strSQLUpdate_OrderI = "update STMREQI set fcStep = ?, fnBackQty = ? where fcSkid = ?";
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdate_OrderI, new object[] { strOrderIStep, decBackQty, dtrOrderI["fcSkid"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    //pobjSQLUtil.SetPara(new object[] { strOrderIStep, decBackQty, dtrOrderI["fcSkid"].ToString() });
                    //pobjSQLUtil.SQLExec(strSQLUpdate_OrderI, ref strErrorMsg);
                }
                //Update RefTo OrderH step
                this.mSaveDBAgent.BatchSQLExec("update STMREQH set fcStep = ? where fcSkid = ?", new object[] { strHStep, inOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //pobjSQLUtil.SetPara(new object[] { strHStep, inOrderH});
                //pobjSQLUtil.SQLExec("update ORDERH set fcStep = ? where fcSkid = ?", ref strErrorMsg);

            }
        }

        private decimal pmSumCutQty_Stm(string inRefType, string inChildH, string inChildI, decimal inUOMQty)
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            decimal decCutQty = 0;
            string strFoxSQLStrREFDOC_STMOVE = "select * from REFDOC_STMOVE where REFDOC_STMOVE.cChildtype = ? and REFDOC_STMOVE.cChildH = ? and REFDOC_STMOVE.cChildI = ?";
            pAPara = new object[] { inRefType, inChildH, inChildI };
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QREFDOC_STMOVE", "REFDOC_STMOVE", strFoxSQLStrREFDOC_STMOVE, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrREFDOC_STMOVE in this.dtsDataEnv.Tables["QREFDOC_STMOVE"].Rows)
                {
                    //pobjSQLUtil.SetPara(new object[] { dtrREFDOC_STMOVE["fcMasterH"].ToString() });
                    pAPara = new object[] { dtrREFDOC_STMOVE["cChildH"].ToString() };
                    if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select fcStat from STMREQH where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        if (this.dtsDataEnv.Tables["QRefTo"].Rows[0]["fcStat"].ToString() != "C")
                            decCutQty += Convert.ToDecimal(dtrREFDOC_STMOVE["nQty"]) * Convert.ToDecimal(dtrREFDOC_STMOVE["nUmQty"]) / (inUOMQty != 0 ? inUOMQty : 1);
                    }
                }
            }
            return decCutQty;
        }

        private bool XXXpmSaveRefTo()
        {

            bool bllResult = true;
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;

            if (this.mstrRefToRowID != this.mstrOldRefToRowID)
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[4] { this.mstrRefType, this.mstrOldRefToRowID, this.mstrRefToRefType, this.mstrRefToRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? and cChildH = ?", ref strErrorMsg);
            }

            if (this.mstrRefToRowID != string.Empty)
            {
                bool bllIsNewRow_RefTo = false;
                string strRowID_RefTo = "";
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "REFDOC", "REFDOC", "select * from REFDOC where 0=1", ref strErrorMsg);
                DataRow dtrREFDOC = this.dtsDataEnv.Tables["REFDOC"].NewRow();

                if (pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID })
                    && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefTo", "REFDOC", "select cRowID from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
                {
                    strRowID_RefTo = App.mRunRowID("REFDOC");
                    bllIsNewRow_RefTo = true;
                }
                else
                {
                    strRowID_RefTo = this.dtsDataEnv.Tables["QChkRefTo"].Rows[0]["cRowID"].ToString();
                }

                dtrREFDOC["cRowID"] = strRowID_RefTo;
                dtrREFDOC["cCorp"] = App.ActiveCorp.RowID;
                dtrREFDOC["cBranch"] = this.mstrBranch;
                dtrREFDOC["cPlant"] = this.mstrPlant;
                dtrREFDOC["cMasterTyp"] = this.mstrRefType;
                dtrREFDOC["cMasterH"] = this.mstrEditRowID;
                dtrREFDOC["cChildType"] = this.mstrRefToRefType;
                dtrREFDOC["cChildH"] = this.mstrRefToRowID;
                dtrREFDOC["cChildI"] = this.mstrRefToMOrderOP;

                string strSQLUpdateStr_RefTo = "";
                DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
                pobjSQLUtil.SetPara(pAPara);
                bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);

            }
            else
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg);

            }

            //this.pmUpdateWOrderOPStep(this.mstrRefToRowID);
            return bllResult;
        }


        private bool pmReplRecordTemPd(string inAlias, bool inState, string inType, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrWOrderI = ioRefProd;
            DataRow dtrWOrderH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

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

            dtrWOrderI["cCorp"] = App.ActiveCorp.RowID;
            dtrWOrderI["cBranch"] = this.mstrBranch;
            dtrWOrderI["cPlant"] = this.mstrPlant;
            dtrWOrderI["cWOrderH"] = dtrWOrderH["cRowID"].ToString();
            dtrWOrderI["cRefType"] = dtrWOrderH["cRefType"].ToString();
            //dtrWOrderI["fcRfType"] = dtrWOrderH["fcRfType"].ToString();
            dtrWOrderI["cStat"] = dtrWOrderH["cStat"].ToString();
            dtrWOrderI["dDate"] = Convert.ToDateTime(dtrWOrderH["dDate"]).Date;
            dtrWOrderI["cCoor"] = dtrWOrderH["cCoor"].ToString();
            dtrWOrderI["cRefPdType"] = "P";
            dtrWOrderI["cOPSeq"] = inTemPd["cOPSeq"].ToString().TrimEnd();
            dtrWOrderI["cIsMainPd"] = inTemPd["cIsMainPd"].ToString().TrimEnd();
            dtrWOrderI["cProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrWOrderI["cProd"] = inTemPd["cProd"].ToString();
            dtrWOrderI["cReMark1"] = strRemark1;
            dtrWOrderI["cReMark2"] = strRemark2;
            dtrWOrderI["cReMark3"] = strRemark3;
            dtrWOrderI["cReMark4"] = strRemark4;
            dtrWOrderI["cReMark5"] = strRemark5;
            dtrWOrderI["cReMark6"] = strRemark6;
            dtrWOrderI["cReMark7"] = strRemark7;
            dtrWOrderI["cReMark8"] = strRemark8;
            dtrWOrderI["cReMark9"] = strRemark9;
            dtrWOrderI["cReMark10"] = strRemark10;
            dtrWOrderI["cLot"] = inTemPd["cLot"].ToString().TrimEnd();
            dtrWOrderI["cWhouse"] = inTemPd["cWHouse"].ToString().TrimEnd();
            dtrWOrderI["cSect"] = dtrWOrderH["cSect"].ToString();
            dtrWOrderI["cDept"] = dtrWOrderH["cDept"].ToString();
            dtrWOrderI["cJob"] = dtrWOrderH["cJob"].ToString();
            dtrWOrderI["cProj"] = dtrWOrderH["cProj"].ToString();
            dtrWOrderI["cIOType"] = (inAlias == this.mstrTemFG ? "I" : "O");
            dtrWOrderI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrWOrderI["cBOMIT_PD"] = inTemPd["cPdStI"].ToString();
            dtrWOrderI["cProcure"] = inTemPd["cProcure"].ToString();

            dtrWOrderI["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrWOrderI["nStQty"] = Convert.ToDecimal(inTemPd["nStkQty"]);
            dtrWOrderI["nUOMQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrWOrderI["cUOM"] = inTemPd["cUOM"].ToString().TrimEnd();
            dtrWOrderI["cUOMStd"] = inTemPd["cUOMStd"].ToString().TrimEnd();

            dtrWOrderI["nStQty"] = Convert.ToDecimal(inTemPd["nStkQty"]);
            dtrWOrderI["nStUOMQty"] = Convert.ToDecimal(inTemPd["nStkUOMQty"]);
            dtrWOrderI["cStUOM"] = inTemPd["cUOMStd"].ToString().TrimEnd();
            dtrWOrderI["cStUOMStd"] = inTemPd["cUOMStd"].ToString().TrimEnd();

            dtrWOrderI["nBackQty1"] = Convert.ToDecimal(inTemPd["nBackQty1"]);
            dtrWOrderI["nBackQty2"] = Convert.ToDecimal(inTemPd["nBackQty2"]);

            dtrWOrderI["cScrap"] = inTemPd["cScrap"].ToString().TrimEnd();

            dtrWOrderI["cStep1"] = inTemPd["cStep1"].ToString();
            dtrWOrderI["cStep2"] = inTemPd["cStep2"].ToString();

            ioRefProd = dtrWOrderI;

            return true;
        }

        private void pmCreateTem()
        {

            this.pmCreateTemPd(this.mstrTemPd);

            DataTable dtbTemRoute = new DataTable(this.mstrTemRoute);
            dtbTemRoute.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRoute.Columns.Add("cFrOPSeq", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cFrWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQcFrWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQnFrWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cFrMOPR", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQcFrMOPR", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQnFrMOPR", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cToOPSeq", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cToWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQcToWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQnToWkCtr", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cToMOPR", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQcToMOPR", System.Type.GetType("System.String"));
            dtbTemRoute.Columns.Add("cQnToMOPR", System.Type.GetType("System.String"));

            dtbTemRoute.Columns["cFrOPSeq"].DefaultValue = "";
            dtbTemRoute.Columns["cFrWkCtr"].DefaultValue = "";
            dtbTemRoute.Columns["cQcFrWkCtr"].DefaultValue = "";
            dtbTemRoute.Columns["cQnFrWkCtr"].DefaultValue = "";
            dtbTemRoute.Columns["cFrMOPR"].DefaultValue = "";
            dtbTemRoute.Columns["cQcFrMOPR"].DefaultValue = "";
            dtbTemRoute.Columns["cQnFrMOPR"].DefaultValue = "";
            dtbTemRoute.Columns["cToOPSeq"].DefaultValue = "";
            dtbTemRoute.Columns["cToWkCtr"].DefaultValue = "";
            dtbTemRoute.Columns["cQcToWkCtr"].DefaultValue = "";
            dtbTemRoute.Columns["cQnToWkCtr"].DefaultValue = "";

            this.dtsDataEnv.Tables.Add(dtbTemRoute);


            DataTable dtbTemRefTo = new DataTable(this.mstrTemRefTo);

            dtbTemRefTo.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRefTo.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("QnProd", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cOld_OrderH", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cOld_OrderI", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cOrderI", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cOrderH", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("fcCode", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("fcRefNo", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("fdDate", System.Type.GetType("System.DateTime"));
            dtbTemRefTo.Columns.Add("fnBackQty", System.Type.GetType("System.Decimal"));
            dtbTemRefTo.Columns.Add("fnMOQty", System.Type.GetType("System.Decimal"));
            dtbTemRefTo.Columns.Add("IsDelete", System.Type.GetType("System.Boolean"));

            dtbTemRefTo.Columns["cRowID"].DefaultValue = "";
            dtbTemRefTo.Columns["IsDelete"].DefaultValue = false;
            dtbTemRefTo.Columns["cGUID"].DefaultValue = "";
            dtbTemRefTo.Columns["cProd"].DefaultValue = "";
            dtbTemRefTo.Columns["QcProd"].DefaultValue = "";
            dtbTemRefTo.Columns["QnProd"].DefaultValue = "";
            dtbTemRefTo.Columns["cCoor"].DefaultValue = "";
            dtbTemRefTo.Columns["cOld_OrderH"].DefaultValue = "";
            dtbTemRefTo.Columns["cOld_OrderI"].DefaultValue = "";
            dtbTemRefTo.Columns["cOrderI"].DefaultValue = "";
            dtbTemRefTo.Columns["cOrderH"].DefaultValue = "";
            dtbTemRefTo.Columns["fcCode"].DefaultValue = "";
            dtbTemRefTo.Columns["fcRefNo"].DefaultValue = "";
            dtbTemRefTo.Columns["fnBackQty"].DefaultValue = 0;
            dtbTemRefTo.Columns["fnMOQty"].DefaultValue = 0;

            dtbTemRefTo.TableNewRow += new DataTableNewRowEventHandler(TemRefTo_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemRefTo);



            DataTable dtbTemPack = new DataTable(this.mstrTemPack);
            dtbTemPack.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPack.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPack.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPack.Columns.Add("cShowPack", System.Type.GetType("System.String"));

            dtbTemPack.Columns["cRowID"].DefaultValue = "";
            dtbTemPack.Columns["cProd"].DefaultValue = "";
            dtbTemPack.Columns["cQcProd"].DefaultValue = "";
            dtbTemPack.Columns["cQnProd"].DefaultValue = "";
            dtbTemPack.Columns["cShowPack"].DefaultValue = "";

            dtbTemPack.TableNewRow += new DataTableNewRowEventHandler(TemPack_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPack);

            DataTable dtbTemRef2Pack = new DataTable(this.mstrTemRefTo_Pack);
            dtbTemRef2Pack.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRef2Pack.Columns.Add("cParent_GUID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cShowPack", System.Type.GetType("System.String"));

            dtbTemRef2Pack.Columns.Add("nPackSize", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack.Columns.Add("cPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQnPackStyle", System.Type.GetType("System.String"));

            dtbTemRef2Pack.Columns["cRowID"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQcProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQnProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cShowPack"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cPackStyle"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQnPackStyle"].DefaultValue = "";
            dtbTemRef2Pack.Columns["nPackSize"].DefaultValue = 0;

            dtbTemPack.TableNewRow += new DataTableNewRowEventHandler(TemRef2Pack_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemRef2Pack);

            DataTable dtbTemRef2Pack_Det = new DataTable(this.mstrTemRefTo_Pack_Det);
            dtbTemRef2Pack_Det.Columns.Add("cIsDel", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRef2Pack_Det.Columns.Add("cRefTo_Pack", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cPackCode", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack_Det.Columns.Add("nPackQty", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack_Det.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cShowPack", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cPack_Seq", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("fcRefProd", System.Type.GetType("System.String"));

            dtbTemRef2Pack_Det.Columns.Add("nPackSize", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack_Det.Columns.Add("cPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cQcPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack_Det.Columns.Add("cQnPackStyle", System.Type.GetType("System.String"));

            dtbTemRef2Pack_Det.Columns["cIsDel"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cRowID"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cProd"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cQcProd"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cQnProd"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cShowPack"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cPackStyle"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["cQnPackStyle"].DefaultValue = "";
            dtbTemRef2Pack_Det.Columns["nPackSize"].DefaultValue = 0;
            dtbTemRef2Pack_Det.Columns["fcRefProd"].DefaultValue = "";

            dtbTemRef2Pack_Det.TableNewRow += new DataTableNewRowEventHandler(TemRef2Pack_Det_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemRef2Pack_Det);

            this.dtsDataEnv.Tables.Add(this.mPdSer.CreateTemPdSer());

            DataTable dtb1PdSer = this.mPdSer.CreateTemPdSer();
            dtb1PdSer.TableName = xd_Alias_Tem1PdSer;
            this.dtsDataEnv.Tables.Add(dtb1PdSer);

        }
        
        private string mstrTemPack = "TemPack";

        private void pmCreateTemPd(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID2", System.Type.GetType("System.String"));
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
            dtbTemPd.Columns.Add("nOldQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQnWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQnWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cAttachFile", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderI", System.Type.GetType("System.String"));

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

            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"), "((nPrice-nDiscAmt1) * nQty) - nDiscAmt");
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
            dtbTemPd.Columns.Add("nPlanQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefTo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIsDelete", System.Type.GetType("System.String"));

            dtbTemPd.Columns.Add("nMOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStMoveQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns["nActRow"].AutoIncrement = true;

            //เรื่อง ชุดสินค้า
            dtbTemPd.Columns.Add("cRootSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQtyPerMFm", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastRefPdType", System.Type.GetType("System.String"));

            //เรื่อง Location
            dtbTemPd.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns["cWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cQcWHLoca"].DefaultValue = "";

            dtbTemPd.Columns.Add("fnAgeLong", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("fdExpire", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns["fnAgeLong"].DefaultValue = 0;

            dtbTemPd.Columns["cRootSeq"].DefaultValue = "";
            dtbTemPd.Columns["cQcPFormula"].DefaultValue = "";
            dtbTemPd.Columns["cPFormula"].DefaultValue = "";
            dtbTemPd.Columns["nQtyPerMFm"].DefaultValue = 0;
            dtbTemPd.Columns["cLastFormula"].DefaultValue = "";
            dtbTemPd.Columns["cLastRefPdType"].DefaultValue = "";

            dtbTemPd.Columns["cRefPdType"].DefaultValue = SysDef.gc_REFPD_TYPE_PRODUCT;
            dtbTemPd.Columns["cStep"].DefaultValue = SysDef.gc_STEP_CREATED;
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOldQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nStQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nLastPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nMOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStMoveQty"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cDiscStr"].DefaultValue = "";
            dtbTemPd.Columns["nDiscAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cDiscStr1"].DefaultValue = "";
            dtbTemPd.Columns["nDiscAmt1"].DefaultValue = 0;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
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
            dtbTemPd.Columns["cFrWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cFrQcWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cFrQnWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cToWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cToQcWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cToQnWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["nBackQty"].DefaultValue = 0;
            dtbTemPd.Columns["nBackDOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nDOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nPlanQty"].DefaultValue = 0;
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
            dtbTemPd.Columns["nCostAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cAttachFile"].DefaultValue = "";
            dtbTemPd.Columns["cWOrderI"].DefaultValue = "";

            dtbTemPd.Columns["nMOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStMoveQty"].DefaultValue = 0;

            //TODO: เรื่องการ Update Stock
            //"nLQty" ==> nLastQty
            dtbTemPd.Columns.Add("cCtrlStoc", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOUmQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStUmQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cCtrlStoc"].DefaultValue = "0";
            dtbTemPd.Columns["nOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOUmQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStUmQty"].DefaultValue = 0;
            //TODO: เรื่องการ Update Stock

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            
            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void TemRefTo_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void TemPack_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }


        private void TemRef2Pack_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void TemRef2Pack_Det_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void pmLoadEditPage()
        {
            this.pmCalcColWidth2();

            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            this.txtCode.Focus();
            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmLoadFormData();
                this.mbllCanEdit = this.pmCanEdit(this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0], DocEditType.Edit, true);
            }

        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.mstrSaveRowID = "";
            this.mstrEditRowID = "";
            this.mstrActiveOP = "";

            this.txtCode.Text = "";
            this.txtRefNo.Text = "";
            this.txtDate.DateTime = DateTime.Now;

            this.mstrFrWkCtrH = "";
            this.mstrToWkCtrH = "";
            this.txtFrOPSeq.Tag = "";
            this.txtFrOPSeq.Text = "";
            this.txtFrQnOP.Text = "";
            this.mstrPlant = "";

            this.mstrStep = SysDef.gc_STEP_IGNORE;
            this.mstrAtStep = "";//SysDef.gc_ATSTEP_VOUCHER_WAIT;

            this.txtFrQcWHouse.Tag = "";
            this.txtFrQcWHouse.Text = "";
            this.txtFrQnWHouse.Text = "";
            this.txtToQcWHouse.Tag = "";
            this.txtToQcWHouse.Text = "";
            this.txtToQnWHouse.Text = "";

            this.txtRemark.Text = "";

            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQcJob.Tag = "";
            this.txtQcJob.Text = "";

            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.mdecRefToAmt = 0;
            this.txtRefTo.Text = "";
            this.mstrOldRefToRowID = "";
            this.mstrOldRefToMOrderOP = "";
            this.mstrRefToMOrderOP = "";
            this.mstrRefToWOrderI = "";
            this.txtRefTo2.Text = "";

            this.mdecSumMOQty = 0;
            this.mdecSumGoodQty = 0;
            this.mdecSumWasteQty = 0;
            this.mdecSumLossQty = 0;

            this.txtQcCoor.Tag = "";
            this.txtQcCoor.Text = "";
            this.txtQnCoor.Text = "";

            this.txtQcSect.Tag = this.mstrDefaSect;
            this.txtQcSect.Text = "";
            //this.mstrDivision = "";
            pobjSQLUtil2.SetPara(new object[1] { this.txtQcSect.Tag.ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode, fcName, fcDept from Sect where fcSkid = ?", ref strErrorMsg))
            {
                this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                //this.mstrDivision = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString();
            }

            this.txtQcJob.Tag = this.mstrDefaJob;
            //this.mstrProj = App.ActiveCorp.DefaultProjectID;
            pobjSQLUtil2.SetPara(new object[1] { this.txtQcJob.Tag.ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode, fcName, fcProj from JOB where fcSkid = ?", ref strErrorMsg))
            {
                this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                //this.mstrProj = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString();
            }

            switch (this.mDocType)
            {
                case DocumentType.WR:
                case DocumentType.GD:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_WIP;

                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrFrWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtFrQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtFrQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtFrQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrToWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtToQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtToQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtToQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }

                    break;
                case DocumentType.WX:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY;
                    break;
                case DocumentType.RW:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrFrWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtFrQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtFrQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtFrQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrToWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtToQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtToQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtToQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }
                    break;
                case DocumentType.RX:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    break;
                case DocumentType.FR:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_WIP;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrFrWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtFrQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtFrQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtFrQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrToWhType });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QW1", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
                    {
                        DataRow dtrW1 = this.dtsDataEnv.Tables["QW1"].Rows[0];
                        this.txtToQcWHouse.Tag = dtrW1["fcSkid"].ToString();
                        this.txtToQcWHouse.Text = dtrW1["fcCode"].ToString().TrimEnd();
                        this.txtToQnWHouse.Tag = dtrW1["fcName"].ToString().TrimEnd();
                    }
                    break;
                case DocumentType.TR:
                    //this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    //this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
                    break;
            }
            this.txtRemark.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Clear();

            this.dtsDataEnv.Tables[this.mstrTemPack].Rows.Clear();

            this.dtsDataEnv.Tables[this.mstrTemRefTo_Pack].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemRefTo_Pack_Det].Rows.Clear();

            this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Rows.Clear();
            this.dtsDataEnv.Tables[xd_Alias_Tem1PdSer].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cPdType"];

            this.pgfEditItem.SelectedTabPageIndex = 0;

            pARemark[0] = "";
            pARemark[1] = "";
            pARemark[2] = "";
            pARemark[3] = "";
            pARemark[4] = "";
            pARemark[5] = "";
            pARemark[6] = "";
            pARemark[7] = "";
            pARemark[8] = "";

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

                DataRow dtrRetVal = null;

                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ?", ref strErrorMsg))
                {
                
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

                    this.mstrStep = dtrLoadForm[QMFStmoveHDInfo.Field.Step].ToString();

                    this.txtCode.Text = dtrLoadForm[QMFStmoveHDInfo.Field.Code].ToString().TrimEnd();
                    this.txtRefNo.Text = dtrLoadForm[QMFStmoveHDInfo.Field.RefNo].ToString().TrimEnd();
                    this.txtDate.DateTime = Convert.ToDateTime(dtrLoadForm[QMFStmoveHDInfo.Field.Date]).Date;

                    //this.txtFrOPSeq.Text = dtrLoadForm[QMFStmoveHDInfo.Field.FrOPSeq].ToString().TrimEnd();
                    //this.txtFrQnOP.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.FrMOPR].ToString();
                    //pobjSQLUtil.SetPara(new object[1] { this.txtFrQnOP.Tag });
                    //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode,cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    //{
                    //    this.txtFrQnOP.Text = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                    //}

                    this.txtQcCoor.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.CoorID].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcCoor.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcCode,fcName from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcCoor.Text = this.dtsDataEnv.Tables["QCoor"].Rows[0]["fcCode"].ToString().TrimEnd();
                        this.txtQnCoor.Text = this.dtsDataEnv.Tables["QCoor"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    this.txtQcSect.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.SectID].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcSect.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.JobID].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcJob.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from Job where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtFrQcWHouse.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.FrWHouse].ToString();
                    pobjSQLUtil.SetPara(new object[1] { dtrLoadForm[QMFStmoveHDInfo.Field.FrWHouse].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcCode, fcName from " + MapTable.Table.WHouse + " where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtFrQcWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                        this.txtFrQnWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    this.txtToQcWHouse.Tag = dtrLoadForm[QMFStmoveHDInfo.Field.ToWHouse].ToString();
                    pobjSQLUtil.SetPara(new object[1] { dtrLoadForm[QMFStmoveHDInfo.Field.ToWHouse].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcCode, fcName from " + MapTable.Table.WHouse + " where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtToQcWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                        this.txtToQnWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
                    }
                    
                    //this.txtRemark.Text = dtrLoadForm[QMFStmoveHDInfo.Field.Remark].ToString().TrimEnd();

                    //this.pmLoadRefTo();

                    this.pmLoadRemark(dtrLoadForm);
                    this.pmLoadProdTran();

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadProdTran()
        {
            int intRecNo = 0;
            string strErrorMsg = "";
            string strSQLStrRefProd = "select * from RefProd where RefProd.fcGLRef = ? order by RefProd.fcSeq, RefProd.fcIOType ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSeq = "";
            string strIOType = "";
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", strSQLStrRefProd, ref strErrorMsg))
            {
                for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrITable].Rows.Count; intCnt++)
                {
                    DataRow dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[intCnt];

                    strSeq = dtrRefProd["fcSeq"].ToString();
                    strIOType = dtrRefProd["fcIOType"].ToString();

                    intRecNo++;
                    DataRow dtrTemPd = this.pmRepl1RecTemRefProd(intRecNo, dtrRefProd);
                    intCnt++;

                    if (intCnt < this.dtsDataEnv.Tables[this.mstrITable].Rows.Count)
                    {
                        dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[intCnt];
                    }

                    if (intCnt >= this.dtsDataEnv.Tables[this.mstrITable].Rows.Count
                        | strIOType != "I" | dtrRefProd["fcSeq"].ToString() != strSeq | dtrRefProd["fcIOType"].ToString() != "O")
                    {
                        dtrTemPd["cRowID2"] = dtrTemPd["cRowID"].ToString();
                        dtrTemPd["cRowID"] = "";
                    }
                    else
                    {
                        dtrTemPd["cRowID2"] = dtrTemPd["cRowID"].ToString();
                        dtrTemPd["cRowID"] = dtrRefProd["fcSkid"].ToString();
                    }


                    string strRefToNoteCut = "";
                    //Out
                    if (this.mstrRefType == "WR" || this.mstrRefType == "GD")
                    {
                        strRefToNoteCut = dtrTemPd["cRowID"].ToString();
                    }

                    //In
                    if (this.mstrRefType == "FR"
                        || this.mstrRefType == "RW")
                    {
                        strRefToNoteCut = dtrTemPd["cRowID2"].ToString();
                    }
                    
                    //Load NoteCut
                    string strNoteCut = "select * from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ?";
                    //if ((pobjSQLUtil2.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID2"].ToString() })
                    if ((pobjSQLUtil2.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, strRefToNoteCut })
                        && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg)))
                    {

                        DataRow[] dtaRefToRow = this.dtsDataEnv.Tables[this.mstrTemRefTo].Select("cOrderI = '" + this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildI"].ToString() + "'");
                        if (dtaRefToRow.Length == 0)
                        {

                            string strFldList = "";
                            string strSQLRefToStr = "";
                            //Out
                            if (this.mstrRefType == "WR" || this.mstrRefType == "GD")
                            {
                                //string strFldList = "{0}.fcRefNo as fcRefNo, {0}.fdDate as fdDate";
                                strFldList = "ORDERH.fcSkid as fcOrderH, ORDERH.fcRefNo as fcRefNo, ORDERH.fcCode as fcCode, ORDERH.fdDate as fdDate, OrderH.fcCoor";
                                strFldList += ", Book.fcSkid as fcBook, Book.fcRefType as fcRefType, Book.fcCode as fcQcBook";
                                strFldList += ", OrderI.fcSkid as fcOrderI, OrderI.fcProd, OrderI.fnBackQty";
                                strFldList += ", Prod.fcCode as QcProd, Prod.fcName as QnProd";

                                strSQLRefToStr = "select " + strFldList + " from STMREQH ORDERH, Book, STMREQI OrderI,Prod where ORDERI.fcSkid = ? and ORDERH.fcBook = Book.fcSkid and ORDERH.fcSkid = OrderI.fcStmReqH and OrderI.fcProd = Prod.fcSkid";
                            }

                            //In
                            if (this.mstrRefType == "FR" || this.mstrRefType == "RW")
                            {
                                strFldList = "ORDERH.fcSkid as fcOrderH, ORDERH.fcRefNo as fcRefNo, ORDERH.fcCode as fcCode, ORDERH.fdDate as fdDate, OrderH.fcCoor";
                                strFldList += ", Book.fcSkid as fcBook, Book.fcRefType as fcRefType, Book.fcCode as fcQcBook";
                                strFldList += ", OrderI.fcSkid as fcOrderI, OrderI.fcProd, OrderI.fnBackQty";
                                strFldList += ", Prod.fcCode as QcProd, Prod.fcName as QnProd";

                                strSQLRefToStr = "select " + strFldList + " from ORDERH, Book, OrderI,Prod where ORDERI.fcSkid = ? and ORDERH.fcBook = Book.fcSkid and ORDERH.fcSkid = OrderI.fcOrderH and OrderI.fcProd = Prod.fcSkid";
                            }

                            string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildI"].ToString();
                            if ((pobjSQLUtil.SetPara(new object[] { strRefToHRowID })
                                && pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, strSQLRefToStr, ref strErrorMsg)))
                            {
                                DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                                DataRow dtrTemRefTo = this.dtsDataEnv.Tables[this.mstrTemRefTo].NewRow();

                                dtrTemRefTo["cRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cRowID"].ToString();
                                dtrTemRefTo["cOrderI"] = dtrRefTo["fcOrderI"].ToString();
                                dtrTemRefTo["cOrderH"] = dtrRefTo["fcOrderH"].ToString();
                                dtrTemRefTo["cCoor"] = dtrRefTo["fcCoor"].ToString();
                                dtrTemRefTo["cProd"] = dtrRefTo["fcProd"].ToString();
                                dtrTemRefTo["fcCode"] = dtrRefTo["fcCode"].ToString();
                                dtrTemRefTo["fcRefNo"] = dtrRefTo["fcRefNo"].ToString();
                                dtrTemRefTo["fdDate"] = Convert.ToDateTime(dtrRefTo["fdDate"]);
                                dtrTemRefTo["fnBackQty"] = Convert.ToDecimal(dtrRefTo["fnBackQty"]);
                                dtrTemRefTo["QcProd"] = dtrRefTo["QcProd"].ToString();
                                dtrTemRefTo["QnProd"] = dtrRefTo["QnProd"].ToString();

                                //this.mstrRefToRefType = dtrRefTo["fcRefType"].ToString();
                                ////this.mRefToDocType = BusinessEnum.GetDocEnum(this.mstrRefToRefType);

                                //dtrTemRefTo["cRowID"] = strRefToHRowID;
                                //dtrTemRefTo["cRefType"] = this.mstrRefToRefType;
                                //dtrTemRefTo["cBook"] = dtrRefTo["fcBook"].ToString();
                                //dtrTemRefTo["cQcBook"] = dtrRefTo["fcQcBook"].ToString();
                                ////dtrTemRefTo["cCode"] = dtrRefTo["fcRefNo"].ToString();
                                //dtrTemRefTo["cCode"] = dtrRefTo["fcCode"].ToString();
                                //dtrTemRefTo["dDate"] = Convert.ToDateTime(dtrRefTo["fdDate"]);
                                this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrTemRefTo);

                                if (!this.txtRefTo2.Text.Contains(dtrRefTo["fcRefNo"].ToString().Trim()))
                                {
                                    this.txtRefTo2.Text += dtrRefTo["fcRefNo"].ToString().Trim() + ",";
                                }
                            }
                        }

                        dtrTemPd["cXRefToProd"] = dtrTemPd["cProd"].ToString();
                        dtrTemPd["cXRefToRefType"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildType"].ToString();
                        dtrTemPd["cXRefToRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildI"].ToString();
                        dtrTemPd["cXRefToHRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildH"].ToString();
                        dtrTemPd["cRefToRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildI"].ToString();
                        dtrTemPd["cRefToHRowID"] = this.dtsDataEnv.Tables["QNoteCut"].Rows[0]["cChildH"].ToString();

                        //TODO: Load เลขที่อ้างอิงมาโชว์ที่ราย Items
                        //dtrTemPd["cRefToCode"] = this.pmGetRefToCode(dtrTemPd["cRefToHRowID"].ToString());

                        //Load MO Qty
                        pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cRefToRowID"].ToString() });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, "select NQTY from " + QMFWOrderIT_PdInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            dtrTemPd["nMOQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["NQTY"]);
                        }

                    }

                    string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                    string strProdTab = strFMDBName + ".dbo.PROD";
                    string strRefProdTab = strFMDBName + ".dbo.REFPROD";

                    string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                    strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                    strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                    strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                    strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                    switch (this.mDocType)
                    {
                        case DocumentType.WR:
                        case DocumentType.WX:

                            //pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRefType, dtrTemPd["cRefToRowID"].ToString(), this.mstrRefType });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
                            pobjSQLUtil2.NotUpperSQLExecString = true;
                            pobjSQLUtil2.SetPara(new object[] { this.mstrRefType, this.mstrRefToRefType, this.mstrRefToRowID, "O", dtrTemPd["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    dtrTemPd["nStMoveQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }
                            pobjSQLUtil2.NotUpperSQLExecString = false;
                            break;
                        case DocumentType.RW:
                        case DocumentType.RX:
                            //pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRefType, dtrTemPd["cRefToRowID"].ToString(), DocumentType.WR.ToString() });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
                            pobjSQLUtil2.NotUpperSQLExecString = true;
                            pobjSQLUtil2.SetPara(new object[] { this.mstrRefType, this.mstrRefToRefType, this.mstrRefToRowID, "I", dtrTemPd["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    dtrTemPd["nStMoveQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }
                            pobjSQLUtil2.NotUpperSQLExecString = false;
                            break;
                    }

                    //Load RefPdX3
                    if (this.mbllIsGetPdSer)
                    {
                        //this.mPdSer.LoadItemTRefPdX3(pobjSQLUtil, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, dtrTemPd["cRowID2"].ToString(), dtrTemPd["cProd"].ToString(), false, "", "", this.txtFrQcWHouse.RowID);
                        this.mPdSer.LoadItemTRefPdX3(pobjSQLUtil, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, dtrTemPd["cRowID2"].ToString(), dtrTemPd["cProd"].ToString(), false, "", "", this.txtToQcWHouse.Tag.ToString());
                    }

                }
                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();
                if (this.gridView2.RowCount > 0)
                {
                    this.gridView2.FocusedRowHandle = 0;
                }
            }
        }

        private DataRow pmRepl1RecTemRefProd(int inRecNo, DataRow inRefProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            dtrTemPd["cRowID"] = inRefProd["fcSkid"].ToString();
            dtrTemPd["nRecNo"] = inRecNo;

            //			dtrTemPd["cProd"] = inRefProd["fcProd"].ToString();
            //			dtrTemPd["cLastProd"] = inRefProd["fcProd"].ToString();
            //			dtrTemPd["cPdType"] = inRefProd["fcProdType"].ToString();
            //			dtrTemPd["cLastPdType"] = inRefProd["fcProdType"].ToString();
            //			dtrTemPd["cDept"] = inRefProd["fcSect"].ToString();
            //			dtrTemPd["cDivision"] = inRefProd["fcDept"].ToString();
            //			dtrTemPd["cJob"] = inRefProd["fcJob"].ToString();
            //			dtrTemPd["cProject"] = inRefProd["fcProj"].ToString();

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
            dtrTemPd["cWHLoca"] = inRefProd["fcWHLoca"].ToString();

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
            dtrTemPd["nOldQty"] = Convert.ToDecimal(inRefProd["fnQty"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inRefProd["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(inRefProd["fnUmQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inRefProd["fnQty"]);
            dtrTemPd["nPrice"] = Convert.ToDecimal(inRefProd["fnPriceKe"]);
            dtrTemPd["cDiscStr"] = inRefProd["fcDiscStr"].ToString().TrimEnd();
            dtrTemPd["nDiscAmt"] = Convert.ToDecimal(inRefProd["fnDiscAmt"]);

            dtrTemPd["cLot"] = inRefProd["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inRefProd["fcWHouse"].ToString();

                        
            dtrTemPd["fnAgeLong"] = Convert.ToDecimal(inRefProd["fnAgeLong"]);
            if (Convert.IsDBNull(inRefProd["fdExpire"]))
            {
                dtrTemPd["fdExpire"] = Convert.DBNull;
            }
            else
            {
                dtrTemPd["fdExpire"] = Convert.ToDateTime(inRefProd["fdExpire"]);
            }

            dtrTemPd["nOQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
            dtrTemPd["nOUMQty"] = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
            dtrTemPd["nOStQty"] = Convert.ToDecimal(dtrTemPd["nStQty"]);
            dtrTemPd["nOStUMQty"] = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);

            //pobjSQLUtil.SetPara(new object[1] {dtrTemPd["cProd"].ToString()});
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
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
                {
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

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHLoca"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
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

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            return dtrTemPd;
        }

        private void pmLoadRefTo()
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mstrRefToMOrderOP = "";

            //Load Reference Document
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QRefDoc"].Rows[0];
                pobjSQLUtil.SetPara(new object[1] { dtrRefDoc["cChildH"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
                {

                    DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                    
                    this.mstrPlant = dtrRefTo["cPlant"].ToString();
                    this.mstrRefToBook = dtrRefTo["cMfgBook"].ToString();
                    this.mstrRefToRowID = dtrRefTo["cRowID"].ToString();
                    this.mstrRefToMOrderOP = dtrRefDoc["cChildI"].ToString();
                    this.mstrOldRefToRowID = this.mstrRefToRowID;
                    this.mstrOldRefToMOrderOP = this.mstrRefToMOrderOP;
                    this.txtRefTo.Text = dtrRefTo["cRefNo"].ToString().TrimEnd();

                    pobjSQLUtil.SetPara(new object[1] { this.mstrRefToMOrderOP });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefToOP", QMFWOrderIT_OPInfo.TableName, "select * from " + QMFWOrderIT_OPInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrOPSeq = this.dtsDataEnv.Tables["QRefToOP"].Rows[0];
                        this.txtFrOPSeq.Text = dtrOPSeq["cOPSeq"].ToString().TrimEnd();
                        this.txtFrQnOP.Tag = dtrOPSeq["cMOPR"].ToString();
                        pobjSQLUtil.SetPara(new object[1] { this.txtFrQnOP.Tag.ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode,cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            this.txtFrQnOP.Text = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                        }

                    }
                    //

                    //this.pmLoadOPSeq();
                    this.pmLoadFormOPSeq();
                    //this.dataGrid1.DataSource=this.dtsDataEnv;
                }
            }
        }

        private void pmLoadRefTo2(string inWOrderH, ref string ioRefNo, ref string ioOPSeq)
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //Load Reference Document
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, inWOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QRefDoc"].Rows[0];
                pobjSQLUtil.SetPara(new object[1] { dtrRefDoc["cChildH"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
                {

                    DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];

                    ioRefNo = dtrRefTo["cRefNo"].ToString().TrimEnd();

                    pobjSQLUtil.SetPara(new object[1] { dtrRefDoc["cChildI"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefToOP", QMFWOrderIT_OPInfo.TableName, "select * from " + QMFWOrderIT_OPInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrOPSeq = this.dtsDataEnv.Tables["QRefToOP"].Rows[0];
                        ioOPSeq = dtrOPSeq["cOPSeq"].ToString().TrimEnd();
                        //ioQnMOPR = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                    }
                    //

                }
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldRefNo = this.txtRefNo.Text;
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.pmEnterForm();
                    break;
                case Keys.PageUp:
                    if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_EDIT1 : this.pgfMainEdit.SelectedTabPageIndex + 1);
                    break;
                case Keys.PageDown:
                    if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex - 1 <= xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);
                    break;
                case Keys.Escape:
                    this.pmKeyboard_Esc();
                    break;
            }
        
        }

        private void pmKeyboard_Esc()
        {
            if (this.pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ต้องการออกจากหน้าจอ ?", "Do you want to exit ?" }), "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmGotoBrowPage();
                }
            }
            else
            {
                this.mbllPopUpResult = false;
                if (this.mFormActiveMode == FormActiveMode.Edit)
                {
                    this.mbllFilterResult = false;
                    this.pmFilterForm();

                    this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
                    if (!this.mbllFilterResult)
                    {
                        this.Close();
                    }
                    else
                    {
                        this.pmGotoBrowPage();
                    }
                    //this.Close();
                }
                else
                    this.Hide();
            }
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
                this.txtFooter.Text = UIBase.GetAppUIText(new string[] { "สร้างโดย LOGIN : ", "Create By LOGIN : " }) + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + UIBase.GetAppUIText(new string[] { "วันที่/เวลา : ", " Date/Time : " }) + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += UIBase.GetAppUIText(new string[] { "\r\nแก้ไขล่าสุดโดย LOGIN : ", "\r\nLast update by LOGIN : " }) + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + UIBase.GetAppUIText(new string[] { "วันที่/เวลา : ", " Date/Time : " }) + Convert.ToDateTime(dtrBrow["dLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");
            }
            else 
            {
                this.txtFooter.Text = "";
            }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.pmLoadRMDetail();
        }

        private void pmLoadRMDetail()
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            if (dtrTemPd != null)
            {
                this.txtTemPdFooter.Text = "";

                string strWHouse = "";
                string strQnWHouse = "";
                switch (this.mDocType)
                {
                    case DocumentType.WR:
                    case DocumentType.WX:
                        strWHouse = this.txtFrQcWHouse.Tag.ToString();
                        strQnWHouse = this.txtFrQcWHouse.Text.Trim();
                        break;
                    case DocumentType.RW:
                    case DocumentType.RX:
                        strWHouse = this.txtToQcWHouse.Tag.ToString();
                        strQnWHouse = this.txtToQcWHouse.Text.Trim();
                        break;
                    case DocumentType.FR:
                        strWHouse = this.txtToQcWHouse.Tag.ToString();
                        strQnWHouse = this.txtToQcWHouse.Text.Trim();
                        break;
                    case DocumentType.TR:
                        strWHouse = this.txtFrQcWHouse.Tag.ToString();
                        strQnWHouse = this.txtFrQcWHouse.Text.Trim();
                        break;
                }
                string strMsg = UIBase.GetAppUIText(new string[] { "สินค้า : ", "RM/Product : " }) + dtrTemPd["cQcProd"].ToString().TrimEnd() + " (" + dtrTemPd["cQnProd"].ToString().TrimEnd() + ")";

                decimal decStkQty = this.GetStockBalance(dtrTemPd["cProd"].ToString(), strWHouse, dtrTemPd["cLot"].ToString());
                if (Convert.ToDecimal(dtrTemPd["nUOMQty"]) != 0)
                    decStkQty = decStkQty / Convert.ToDecimal(dtrTemPd["nUOMQty"]);

                string strQtyFormat = "#,###,###,##0.00";
                decimal decPdBrQty = 0; decimal decMinQty = 0; decimal decMaxQty = 0;

                this.mStockAgent.GetStockPdBranch(dtrTemPd["cProd"].ToString(), this.mstrBranch, strWHouse, ref decPdBrQty, ref decMinQty, ref decMaxQty);

                decimal decPdBrQty2 = decPdBrQty;
                if (Convert.ToDecimal(dtrTemPd["nUOMQty"]) != 0)
                    decPdBrQty2 = decPdBrQty2 / Convert.ToDecimal(dtrTemPd["nUOMQty"]);

                strMsg += UIBase.GetAppUIText(new string[] { "\r\nคลัง : ", "\r\nWarehouse : " }) + strQnWHouse + UIBase.GetAppUIText(new string[] { "   คงเหลือ : ", "   Balance : " }) + decStkQty.ToString(strQtyFormat) + " " + dtrTemPd["cQnUOM"].ToString().TrimEnd();
                strMsg += "   Minimum Stock : " + decMinQty.ToString(strQtyFormat) + " Maximun Stock : " + decMaxQty.ToString(strQtyFormat) + " /รวมทุกคลังสินค้า : " + decPdBrQty2.ToString(strQtyFormat);
                this.txtTemPdFooter.Text = strMsg;
            }
            else
            {
                this.txtTemPdFooter.Text = "";
            }
        }

        private void gridView2_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this.gridView2.UpdateCurrentRow();
            //this.pmSumRMQty();
        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == QMFStmoveHDInfo.Field.Code)
                inSearchStr = inSearchStr.PadRight(this.txtCode.Properties.MaxLength);
            else
                inSearchStr = inSearchStr.PadRight(this.txtRefNo.Properties.MaxLength);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false)
            {
                string strSearch = inSearchStr.TrimEnd();
                if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
                    strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
                {
                    strSearch = "";
                }

                this.mstrSearchStr = "%" + strSearch + "%";
                this.pmRefreshBrowView();
            }

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {

                this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

                this.gridView1.MoveLast();
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
                {
                    string strSeekNear = BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFStmoveHDInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtRefNo.Properties.MaxLength);
                    this.gridView1.StartIncrementalSearch(strSeekNear);
                }

                this.ShowDialog();
                this.mbllIsShowDialog = true;
            }
            else
            {
                this.gridView1.FocusedRowHandle = intSeekVal;
                this.mbllPopUpResult = true;
            }

            return this.mbllPopUpResult;
        }

        public DataRow RetrieveValue()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null) return null;
            
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["cRowID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            DataRow dtrTemPd = null;
            string strOrderBy = "";
            switch (inKeyField.ToUpper())
            {
                case "GRDVIEW2_CQCPROD":
                case "GRDVIEW2_CQNPROD":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCPROD" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("PROD");

                    dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    string strRefPdType = this.mstrPdType;

                    if (dtrTemPd != null && dtrTemPd["cPdType"].ToString().Trim() != string.Empty)
                    {
                        strRefPdType = this.pmSplitToSQLStr(dtrTemPd["cPdType"].ToString());
                    }

                    this.pofrmGetProd.ValidateField(strRefPdType, "", strOrderBy, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                case "GRDVIEW2_CLOT":
                    this.pmInitPopUpDialog("LOT_ITEM");
                    break;

                case "GRDVIEW2_CQCWHLOCA":
                    strOrderBy = "FCCODE";
                    this.pmInitPopUpDialog("WHLOCA");

                    dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    this.pofrmGetWHLoca.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetWHLoca.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                
                case "GRDVIEW2_CREMARK1":
                    this.pmInitPopUpDialog("REMARK");
                    break;

                case "GRDVIEW2_NQTY":
                    this.pmInitPopUpDialog("QTY_ITEM", true);
                    break;

                case "GRDVIEW2_VIEWFILE":
                    this.pmInitPopUpDialog("VIEWATTACH2");
                    break;

            }
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            //string strValue = "";
            string strValue = e.Value.ToString();
            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

            string strRefPdType = this.mstrPdType;

            if (dtrTemPd["cPdType"].ToString().Trim() != string.Empty)
            {
                strRefPdType = this.pmSplitToSQLStr(dtrTemPd["cPdType"].ToString());
            }
            string strProdID = dtrTemPd["cProd"].ToString();

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
                        e.Valid = !this.pofrmGetProdType.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetProdType.PopUpResult)
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

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cProd"] = "";
                        dtrTemPd["cQcProd"] = "";
                        dtrTemPd["cQnProd"] = "";
                        this.pmClr1TemPd();
                    }
                    else
                    {

                        this.pmInitPopUpDialog("PROD");
                        string strOrderBy = (strCol.ToUpper() == "CQCPROD" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetProd.ValidateField(strRefPdType, strValue, strOrderBy, false);

                        if (this.pofrmGetProd.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCPROD" ? dtrTemPd["cQcProd"].ToString().TrimEnd() : dtrTemPd["cQnProd"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cProd"] = "";
                            dtrTemPd["cQcProd"] = "";
                            dtrTemPd["cQnProd"] = "";
                            this.pmClr1TemPd();
                        }
                    }
                    break;
                case "CLOT":
                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cLot"] = "";
                    }
                    else
                    {
                        dtrTemPd["cLot"] = strValue;
                        this.pmInitPopUpDialog("LOT_ITEM");
                    }
                    break;
                case "CQCWHLOCA":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cWHLoca"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("WHLOCA");
                        string strOrderBy = "FCCODE";
                        e.Valid = !this.pofrmGetWHLoca.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetWHLoca.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = dtrTemPd["cQcWHLoca"].ToString().TrimEnd();
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cWHLoca"] = "";
                        }
                    }
                    break;
                case "NQTY":
                    strValue = (strValue != string.Empty ? strValue : "0");
                    decimal decQty = Convert.ToDecimal(strValue);
                    decimal decLastQty = Convert.ToDecimal(dtrTemPd["nLastQty"]);

                    if (decQty != decLastQty)
                    {

                        //if (this.pmChkReferItem("E"))
                        //{
                        //    this.gridView2.SetFocusedValue(decLastQty);
                        //    return;
                        //}

                        if (decQty > 0 && strProdID == string.Empty)
                        {
                            this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nQty", 0);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nBackQty", 0);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nBackDOQty", 0);
                            //this.gridView2.SetRowCellValue(this.gridView2.FocusedRowHandle, "nDOQty", 0);
                            this.gridView2.UpdateCurrentRow();
                            MessageBox.Show("กรุณาระบุสินค้าก่อนระบุจำนวนสินค้า/บริการ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            this.gridView2.SetFocusedValue(decQty);
                            if (this.pmValidQtyCol())
                            {
                                this.mbllRecalTotPd = true;
                                e.Value = this.mdecCurrQty;
                            }
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
            }

        }

        private bool pmValidQtyCol()
        {
            return this.pmInitPopUpDialog("QTY_ITEM", true);
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

        private void grcGridColumn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Tag != null)
                this.pmGridPopUpButtonClick(e.Button.Tag.ToString());
        }

        private void grcGridColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    string strPrefix = (this.mActiveGrid.Name.ToUpper() == "GRIDVIEW2" ? "GRDVIEW2_" : "GRDVIEW2_");
                    this.pmGridPopUpButtonClick(strPrefix + this.mActiveGrid.FocusedColumn.FieldName);
                    //this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
                    break;
            }
        }

        private void pmUpdateWOrderOPStep(string inWOrderH)
        {
            string strErrorMsg = "";
            string strSQLStr = "select * from MFWORDERIT_STDOP where CWORDERH = ? and CTYPE = ' ' order by COPSEQ ";
            string strOPStep = SysDef.gc_REF_OPSTEP_CREATE;
            string strHOPStep = SysDef.gc_REF_OPSTEP_FINISH;

            if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QRefToOP", "MFWCTRANIT", strSQLStr, new object[] { inWOrderH }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
            {
                int intRowCount = this.dtsDataEnv.Tables["QRefToOP"].Rows.Count;
                int intStep_Create = 0;
                int intStep_Start = 0;
                int intStep_Finish = 0;

                foreach (DataRow dtrWcTranI in this.dtsDataEnv.Tables["QRefToOP"].Rows)
                {
                    strOPStep = this.pmGetOPStep(inWOrderH, dtrWcTranI["cRowID"].ToString());
                    //Update RefTo OrderI step
                    string strSQLUpdate_OrderI = "update MFWORDERIT_STDOP set cStep = ? where cRowID = ?";
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdate_OrderI, new object[] { strOPStep, dtrWcTranI["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    if (strOPStep == SysDef.gc_REF_OPSTEP_CREATE) intStep_Create++;
                    else if (strOPStep == SysDef.gc_REF_OPSTEP_START) intStep_Start++;
                    else if (strOPStep == SysDef.gc_REF_OPSTEP_FINISH) intStep_Finish++;
                }
                if (intStep_Create == intRowCount) strHOPStep = SysDef.gc_REF_OPSTEP_CREATE;
                else if (intStep_Finish == intRowCount) strHOPStep = SysDef.gc_REF_OPSTEP_FINISH;
                else strHOPStep = SysDef.gc_REF_OPSTEP_START;

                //Update RefTo OrderH step
                this.mSaveDBAgent2.BatchSQLExec("update MFWORDERHD set cOPStep = ? where cRowID = ?", new object[] { strHOPStep, inWOrderH }, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

            }
        }

        private string pmGetOPStep(string inWOrderH, string inWOrderOP)
        {
            string strRetStep = SysDef.gc_REF_OPSTEP_CREATE;
            string strErrorMsg = "";
            string strSQLStr = "select MFWCTRANHD.CMSTEP from MFWCTRANHD where MFWCTRANHD.CROWID in ";
            strSQLStr += " (select REFDOC.CMASTERH from REFDOC ";
            strSQLStr += " where CMASTERTYP = 'RJ' and CCHILDTYPE = 'MO'  ";
            strSQLStr += " and CCHILDH = ? and CCHILDI = ?) and MFWCTRANHD.CMSTEP = ? ";

            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QChkFinish", "MFWCTRANIT", strSQLStr, new object[] { inWOrderH, inWOrderOP, "F" }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                strRetStep = SysDef.gc_REF_OPSTEP_FINISH;
            }
            else if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QChkFinish", "MFWCTRANIT", strSQLStr, new object[] { inWOrderH, inWOrderOP, " " }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                strRetStep = SysDef.gc_REF_OPSTEP_START;
            }
            else
            {
                strRetStep = SysDef.gc_REF_OPSTEP_CREATE;
            }
            return strRetStep;
        }

        private void btnUMConvert_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("UOMCONVERT");
        }

        private void btnViewAttach_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("VIEWATTACH");
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

        private void btnGetOPSeq_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("GET_OPSEQ");
        }

        public bool ValidateQty(bool inSetError, ref string ioErrorMsg)
        {
            return this.pmValidQty(inSetError, ref ioErrorMsg);
        }

        public decimal GetStockBalance(string inProd, string inWHouse, string inLot)
        {
            return this.mStockAgent.GetStockBalance(this.mstrBranch, inProd, inWHouse, "", inLot);
        }

        public void QueryTemLot(string inProd, string inWHouse)
        {
            if (this.dtsDataEnv.Tables[StockAgent.xd_Alias_TemLot] != null)
            {
                this.dtsDataEnv.Tables.Remove(StockAgent.xd_Alias_TemLot);
            }
            this.dtsDataEnv.Tables.Add(this.mStockAgent.GetLotTable(this.mstrBranch, inProd, inWHouse, ""));
        }

        private bool pmValidQty(bool inSetError, ref string ioErrorMsg)
        {
            bool bllResult = true;

            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

            decimal decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
            string lcCtrlStock = BizRule.GetProdCtrlStock(dtrTemPd["cCtrlStoc"].ToString(), App.ActiveCorp.SCtrlStock);

            if (lcCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE && decQty > 0)
            {
                ioErrorMsg = "";
                if ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1)
                {
                    lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                }

                decimal lnUpdSign = -1;
                decimal decMinQty = 0;
                decimal decStMinQty = 0;
                //decimal decRetStQty = 0;
                bool bllQtySucc = false;
                bool bllStQtySucc = false;
                string strWHLoca = "";

                this.mStockAgent.TestUpdateStock
                    (
                #region "Stock Parameter"
                      lnUpdSign
                    , this.mstrBranch
                    , dtrTemPd["cPdType"].ToString()
                    , dtrTemPd["cProd"].ToString()
                    , this.txtFrQcWHouse.Tag.ToString()
                    , strWHLoca
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
            return bllResult;
        }
        
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

		private void pmethChgMFMPrice(string tcinRootSeq , bool inChgQtyPerMfm)
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

        #region "On Screen Keyboard"
        private void barButtonItemNum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send(e.Item.Caption);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{F3}");
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{Enter}");
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{LEFT}");
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{RIGHT}");
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{UP}");
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{DOWN}");
        }

        #endregion

        private void grcShowPack_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmInitPopUpDialog("SHOW_PACK");
        }

    }

}
