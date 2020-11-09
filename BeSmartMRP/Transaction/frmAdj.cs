
//TODO: ยังไม่เสร็จเรื่อง แก้สินค้า A=>B จะ Update Stock ผิด

#define xd_RUNMODE_DEBUG

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

    public partial class frmAdj : UIHelper.frmBase, IfrmStockBase
    {

        
        public static string TASKNAME = "EADJ";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        public const string xd_ALIAS_TEMREFTO = "TemRefTo";
        public const string xd_Alias_TemRefToProd = "TemRefToReqProd";
        public const string xd_Alias_Tem1PdSer = "Tem1PdSer";

        public const string xd_ALIAS_BROW_REFTO = "TemBrowRefTo";

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrHTable = QMFStmoveHDInfo.TableName;
        private string mstrRefTable = QMFStmoveHDInfo.TableName;
        private string mstrITable = MapTable.Table.RefProd;

        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrCanEditMsg = "";

        private string mstrBookPrefix = "";
        private string mstrBookRunCodeType = "";
        private int mintBookRunCodeLen = 4;
        private string mstrRunCodeStyle = "";

        private string mstrDefaWHouse = "";
        private string mstrDefaWHouseTypeList = " ";//SysDef.gc_WAREHOUSE_TYPE_NORMAL;
        //private WHLocaInfo mDefaWHLoca = new WHLocaInfo();

        //private string mstrDefaLot = "                    ";
        private QEMWHLocationInfo mDefaWHLoca = new QEMWHLocationInfo();

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
        private DocumentType mDocType = DocumentType.AJ;
        private string mstrRefType = DocumentType.AJ.ToString();
        private string mstrRefTypeName = "";
        private string mstrRfType = "O";
        private DocumentType mRefType = DocumentType.AJ;
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
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetUM pofrmGetUM = null;
        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;

        private Common.MRP.dlgGetRefToRoute pofrmGetOPSeq = null;
        private DialogForms.dlgGetCoor pofrmGetCoor = null;
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

        public frmAdj()
        {
            InitializeComponent();
            //this.mstrRefType = inRefType;
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
        
        private static frmAdj mInstanse_1 = null;

        public static frmAdj GetInstanse()
        {

            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmAdj();
            }
            return mInstanse_1;


        }

        private static void pmClearInstanse()
        {

            if (mInstanse_1 != null)
            {
                mInstanse_1 = null;
            }

        }

        private void frmAdj_Load(object sender, EventArgs e)
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

            this.lblRemark.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ :", "Remark :" });

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFStmoveHDInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFStmoveHDInfo.Field.RefNo);


        }

        private void pmMapEvent()
        {

            this.Resize += new EventHandler(frmAdj_Resize);
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

            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);

            this.pgfEditItem.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfEditItem_SelectedPageChanged);

        }

        private void frmAdj_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
            this.pmCalcColWidth2();
        }

        private void pmSetBrowView()
        {
            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind();

            //string strRemSQL = "'" + StringHelper.Replicate(" ", 100) + "' as cRemark1 ";
            string strErrorMsg = "";
            string strSQLExec;
            object[] pAPara = null;
            string[] staPara = new string[] { MapTable.Table.GLRef };
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            strSQLExec = "SELECT {0}.FCSKID,{0}.FCSTAT,{0}.FCATSTEP,{0}.FCCODE,{0}.FDDATE,{0}.FCREFNO,{0}.FMMEMDATA";
            strSQLExec += " , GLREF.FTDATETIME as DCREATE, GLREF.FTLASTUPD as DLASTUPDBY, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD  ";
            strSQLExec += " FROM {0} GLREF ";
            strSQLExec += " left join {1} EM1 ON EM1.FCSKID = GLREF.FCCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.FCSKID = GLREF.FCCORRECTB ";
            strSQLExec += " WHERE {0}.FCCORP = ? AND {0}.FCBRANCH = ? AND {0}.FCREFTYPE = ? AND {0}.FCBOOK = ? AND {0}.FDDATE BETWEEN ? AND ? ";

            //strSQLExec = String.Format(strSQLExec, staPara);
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "EMPLR" });
            //pobjSQLUtil.NotUpperSQLExecString = true;

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrHTable, strSQLExec, ref strErrorMsg);
            DataColumn dtcRemark = new DataColumn("CREMARK", System.Type.GetType("System.String"));
            dtcRemark.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRemark);

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                dtrBrow["CREMARK"] = this.pmLoadRemark2(dtrBrow);
            }
            //dlg.WaitClear();
        }

        private void pmLoadRemark(DataRow inSource)
        {
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData2"]) ? "" : inSource["fmMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData3"]) ? "" : inSource["fmMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData4"]) ? "" : inSource["fmMemData4"].ToString().TrimEnd());
            if (inSource["fmMemData5"] != null)
                strRemark += (Convert.IsDBNull(inSource["fmMemData5"]) ? "" : inSource["fmMemData5"].ToString().TrimEnd());

            this.txtRemark.Text = BizRule.GetMemData(strRemark, "Rem");
        }

        private string pmLoadRemark2(DataRow inSource)
        {
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            return BizRule.GetMemData(strRemark, "Rem");
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

            //this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView2.Columns["cRefPdType"].VisibleIndex = i++;
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

            //this.gridView2.Columns["nQty"].VisibleIndex = i++;
            this.gridView2.Columns["nAddQty"].VisibleIndex = i++;
            this.gridView2.Columns["nSubQty"].VisibleIndex = i++;
            this.gridView2.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView2.Columns["nPrice"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            //this.gridView2.Columns["cPdType"].Caption = "P/F";
            this.gridView2.Columns["cPdType"].Caption = "T";
            this.gridView2.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView2.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView2.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView2.Columns["cLot"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ล๊อต/คลัง", "Lot/WHouse" });
            this.gridView2.Columns["cQcWHLoca"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ที่เก็บ", "Location" });
            //this.gridView2.Columns["cToQcWHLoca"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "เข้าที่เก็บ", "To Location" });
            //this.gridView2.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#MO Qty.", "#MO Qty." });

            this.gridView2.Columns["nAddQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวนเพิ่ม", "In. Qty." });
            this.gridView2.Columns["nSubQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวนลด", "Out. Qty." });
            this.gridView2.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView2.Columns["nPrice"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ราคาทุน", "Cost" });

            this.gridView2.Columns["nRecNo"].Width = 40;
            this.gridView2.Columns["cPdType"].Width = 30;
            this.gridView2.Columns["cPdType"].Width = 50;
            this.gridView2.Columns["cQcProd"].Width = 130;
            this.gridView2.Columns["nQty"].Width = 80;
            this.gridView2.Columns["cQnUOM"].Width = 80;
            this.gridView2.Columns["cRemark1"].Width = 80;
            this.gridView2.Columns["cLot"].Width = 80;
            this.gridView2.Columns["cQcWHLoca"].Width = 80;
            //this.gridView2.Columns["cToQcWHLoca"].Width = 80;
            this.gridView2.Columns["nPrice"].Width = 80;

            this.gridView2.Columns["nAddQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nAddQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView2.Columns["nSubQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nSubQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

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
            //this.gridView2.Columns["cToQcWHLoca"].ColumnEdit = this.grcQcWHLoca;

            this.gridView2.Columns["nAddQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView2.Columns["nAddQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridView2.Columns["nSubQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView2.Columns["nSubQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.pmCalcColWidth2();
        }

        private void pmCalcColWidth1()
        {

            int intColWidth = this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Width
                                    + this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Width;

            //int intNewWidth = this.Width - intColWidth - 60;
            //this.gridView2.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView2.Columns["nRecNo"].Width
                                    //+ this.gridView2.Columns["cPdType"].Width
                                    + this.gridView2.Columns["cPdType"].Width
                                    + this.gridView2.Columns["cQcProd"].Width
                                    + this.gridView2.Columns["cRemark1"].Width
                                    + this.gridView2.Columns["cLot"].Width
                                    + this.gridView2.Columns["nQty"].Width
                                    + this.gridView2.Columns["nPrice"].Width
                                    + this.gridView2.Columns["cQnUOM"].Width;

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
            //this.gridView1.Columns["QCSECT"].VisibleIndex = i++;
            //this.gridView1.Columns["QCJOB"].VisibleIndex = i++;
            this.gridView1.Columns["CREMARK"].VisibleIndex = i++;
            //this.gridView1.Columns["CREFTO_REFNO"].VisibleIndex = i++;
            //this.gridView1.Columns["CREFTO_OP"].VisibleIndex = i++;

            this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Width = 30;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Width = 90;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Width = 100;
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Width = 80;
            //this.gridView1.Columns["QCSECT"].Width = 50;
            //this.gridView1.Columns["QCJOB"].Width = 50;
            this.gridView1.Columns["CREMARK"].Width = 100;

            this.gridView1.Columns[QMFStmoveHDInfo.Field.Stat].Caption = "C";
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Code].Caption = UIBase.GetAppUIText(new string[] {"เลขที่ภายใน","DOC. CODE"});
            this.gridView1.Columns[QMFStmoveHDInfo.Field.RefNo].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });
            this.gridView1.Columns[QMFStmoveHDInfo.Field.Date].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            //this.gridView1.Columns["QCSECT"].Caption = UIBase.GetAppUIText(new string[] { "แผนก", "Section" });
            //this.gridView1.Columns["QCJOB"].Caption = UIBase.GetAppUIText(new string[] { "โครงการ", "Job" });
            this.gridView1.Columns["CREMARK"].Caption = UIBase.GetAppUIText(new string[] { "หมายเหตุ", "Remark" });

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

            //this.barMainEdit.Items["barPrnBarcode"].Enabled = (inActivePage == 0 ? true : false);
            //this.barMainEdit.Items["barImpXLS1"].Enabled = (inActivePage == 0 ? false : true);
            
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
                                    
                                    this.mstrDefaWHouse = dtrBook["FCWHOUSE"].ToString();

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

                case "IMP_XLS1":
                    using (Common.dlgGetXLS1 dlgRefTo = new Common.dlgGetXLS1())
                    {
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            foreach (string strCode in dlgRefTo.lstCode.Lines)
                            {
                                //MessageBox.Show(strCode);
                                this.pmImport_XLS1(strCode);
                            }
                            //this.pmLoadRefToOrder(dlgRefTo.RefTable);
                            this.pmRecalTotPd();
                        }
                    }

                    break;
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
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
                                    if (dtrTemPd["cPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
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
                        
                        dlg.pnlWHouse.Visible = true;
                        dlg.SetParentForm(this);

                        dlg.ProdID = dtrTemPd["cProd"].ToString();
                        //dlg.IsGetPdSer = false;
                        dlg.SaleOrBuy = this.mstrSaleOrBuyForPdSer;

                        string strWHouse = dtrTemPd["cWHouse"].ToString();
                        string strWHLoca = "";

                        dlg.BindData(this.dtsDataEnv, intActRow, this.mstrBranch, strWHouse, dtrTemPd["cLot"].ToString());
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
                            dtrTemPd["cWHouse"] = dlg.txtFrQcWHouse.Tag.ToString();
                            dtrTemPd["cLot"] = dlg.Lot;

                            dtrTemPd["fnAgeLong"] = Convert.ToDecimal(dlg.txtAgeLong.Value);
                            dtrTemPd["fdExpire"] = dlg.txtExpDate.DateTime;

                            this.mActiveGrid.UpdateCurrentRow();
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

                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor(this.mCoorType);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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

        private void pmClearRefTo()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.mstrPlant = "";

            this.mstrFrWkCtrH = "";

        }

        private void pmImport_XLS1(string inQcProd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string[] aData = inQcProd.Split(",".ToCharArray());
            DataRow dtrTemPd = null;
            if (aData[0].ToString().Trim() != "")
            {
                string strQcProd = aData[0].ToString().Trim();
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, strQcProd });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    this.pmRetrieveProductVal(ref dtrTemPd, this.dtsDataEnv.Tables["QProd"].Rows[0]);

                }
                else
                {
                    MessageBox.Show("ไม่พบสินค้ารหัส" + strQcProd);
                    return;
                }
            }
            else
            {
                return;
            }

            string strQty = aData[3].ToString().Trim();
            string strQnLoca = aData[1].ToString().Trim();
            string strLot = aData[2].ToString().Trim();
            decimal decBalQty = Convert.ToDecimal(strQty.Replace(",", ""));

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
            //dtrTemPd["cPdType"] = "";
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
            dtrTemPd["nAddQty"] = decBalQty;
            dtrTemPd["nOldQty"] = decBalQty;
            dtrTemPd["nUOMQty"] = 1;
            dtrTemPd["nLastQty"] = decBalQty;
            //dtrTemPd["nPrice"] = Convert.ToDecimal(inOrderI["fnPriceKe"]);

            dtrTemPd["cLot"] = strLot;
            dtrTemPd["cWHouse"] = "";

            dtrTemPd["nOQty"] = decBalQty;
            dtrTemPd["nOUMQty"] = 1;
            dtrTemPd["nOStQty"] = decBalQty;
            dtrTemPd["nOStUMQty"] = 1;

            if (dtrTemPd["cPFormula"].ToString().TrimEnd() != "")
            {
                pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cPFormula"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QFormula", "FORMULA", "select FCCODE,FCNAME,FCUM from FORMULAS where fcSkid = ?", ref strErrorMsg))
                {
                    dtrTemPd["cQcPFormula"] = this.dtsDataEnv.Tables["QFormula"].Rows[0]["fcCode"].ToString().TrimEnd();
                }
            }

            if (dtrTemPd["cPdType"].ToString() == SysDef.gc_REFPD_TYPE_FORMULA)
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

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, SysDef.gc_WHOUSE_TYPE_NORMAL });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcSkid,fcCode, fcName from WHOUSE where fcCorp = ? and fcBranch = ? and fcType = ? order by FCCODE", ref strErrorMsg))
            {
                dtrTemPd["cWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                dtrTemPd["cQcWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            //string strQnLoca = "XXXXX";
            if (strQnLoca.Trim() == "")
            {
                strQnLoca = "-";
            }
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, strQnLoca });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select * from WHLocation where fcCorp = ? and fcName = ?", ref strErrorMsg))
            {
                dtrTemPd["cWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcSkid"].ToString();
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
                case "TXTQCPROD":
                case "TXTQNPROD":
                    this.pmInitPopUpDialog("PROD");
                    strPrefix = (inTextbox == "TXTQCPROD" ? "FCCODE" : "FCNAME");
                    this.pofrmGetProd.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCCOOR":
                case "TXTQNCOOR":
                    this.pmInitPopUpDialog("COOR");
                    strPrefix = (inTextbox == "TXTQCCOOR" ? "FCCODE" : "FCNAME");
                    this.pofrmGetCoor.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetCoor.PopUpResult)
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
            inTemPd["cPdType"] = SysDef.gc_REFPD_TYPE_PRODUCT;
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
            inTemPd["cWHouse"] = this.mstrDefaWHouse;
            //inTemPd["cWHouse"] = this.txtFrQcWHouse.Tag;
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

            inTemPd["nPrice"] = this.pmGetCost(inTemPd["cProd"].ToString(), inTemPd["cWHouse"].ToString(), inTemPd["cLot"].ToString(), decUOMQty);

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

            string strWHouse = this.mstrDefaWHouse;
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
            //dtrTemPd["nBackDOQty"] = 0;
            //dtrTemPd["nDOQty"] = 0;
            //dtrTemPd["nPlanQty"] = 0;
            dtrTemPd["nPrice"] = 0;
            dtrTemPd["cDiscStr"] = "";
            //dtrTemPd["nAmt"] = 0;

            //dtrTemPd["cRefToRowID"] = "";
            //dtrTemPd["cRefToCode"] = "";

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
                                if (App.PermissionManager.CheckPermission(App.AppUserName, AuthenType.CanEdit, App.AppUserID, TASKNAME))
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

            //string strIOType = (this.mstrFrWhType == SysDef.gc_WHOUSE_TYPE_NORMAL ? "O" : "I");
            //strIOType = (this.mstrRefType == DocumentType.TR.ToString() ? "I" : strIOType);
            string strSQLStrOrderI = "select * from " + this.mstrITable + " where fcGLRef = ? order by fcSeq";

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

                        pobjSQLUtil.SetPara(new object[] { dtrPFormH["fcSkid"].ToString() });
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
                if (dtrTemPd["cProd"].ToString() != string.Empty && Convert.ToDecimal(dtrTemPd["nAddQty"]) == 0 && Convert.ToDecimal(dtrTemPd["nSubQty"]) == 0)
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

            string strProdLot = "";
            decimal decStockBal = 0;
            decimal decStockBal1 = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                DataRowView dtrTemPd = dv[i];

                string lcCtrlStock = BizRule.GetProdCtrlStock(pobjSQLUtil, dtrTemPd["cProd"].ToString(), App.ActiveCorp.SCtrlStock);
                switch (this.mDocType)
                {
                    case DocumentType.AJ:
                        lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
                        break;
                }

                strWHouse = dtrTemPd["cWHouse"].ToString();
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
            dtrSaveInfo[QMFStmoveHDInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            //dtrSaveInfo[QMFStmoveHDInfo.Field.DeptID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.JobID] = this.txtQcJob.Tag.ToString();
            //dtrSaveInfo[QMFStmoveHDInfo.Field.ProjID] = this.txtQcJob.Tag.ToString();
            dtrSaveInfo[QMFStmoveHDInfo.Field.LUpdApp] = App.AppID;
            dtrSaveInfo["fcDataser"] = "";	//fcDataser
            dtrSaveInfo["fcEAfterR"] = "E";

            string gcTemStr01 = BizRule.SetMemData(this.txtRemark.Text.TrimEnd(), "Rem");
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

                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
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
                    DataRow dtrRRefProd = null;

                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        & (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefProd", "RefProd", "select * from RefProd where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        strRowID = App.mRunRowID("RefProd");
                        bllIsNewRow = true;
                        decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                        decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                        decStQty = 0;
                    }
                    else
                    {
                        bllIsNewRow = false;
                        strRowID = dtrTemPd["cRowID"].ToString();
                        dtrRRefProd = this.dtsDataEnv.Tables["RRefProd"].Rows[0];
                        strWHouse = dtrTemPd["cWHouse"].ToString();

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
                            this.pmRetStock(dtrRRefProd, dtrTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);
                        }
                    }

                    dtrTemPd["cRowID"] = strRowID;
                    strOutRowID = strRowID;
                    DataRow dtrRefProd = null;

                    decimal decAddQty = Convert.ToDecimal(dtrTemPd["nAddQty"]);
                    decimal decSaveQty = decQty * (decAddQty > 0 ? 1 : -1);
                    decimal decSaveStQty = decStQty * (decAddQty > 0 ? 1 : -1);
                    this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, decSaveQty, decUmQty, decSaveStQty, decUmQty);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    ////Save RefPdX3
                    //if (this.mbllIsGetPdSer)
                    //{
                    //    this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    //    this.mPdSer.SaveRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID"].ToString(), dtrTemPd["cWHouse"].ToString(), dtrTemPd["cWHLoca"].ToString(), dtrTemPd["cLot"].ToString());
                    //}

                }
                else
                {

                    //TODO: Update Stock When Delete
                    //this.pmRetStock(dtrRRefProd, dtrTemPd["cWHLoca"].ToString(), true, false, "", ref strErrorMsg);

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[1] { dtrTemPd["cRowID"].ToString() };
                        if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefProd", this.mstrITable, "select * from " + this.mstrITable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                        {
                            DataRow dtrRefProd2 = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                            this.pmRetStock(dtrRefProd2, dtrTemPd["cWHLoca"].ToString(), false, false, "", ref strErrorMsg);
                        }

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from RefProd where FCSKID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                    ////Delete RefPdX3
                    //if (this.mbllIsGetPdSer)
                    //{
                    //    this.mstrSaleOrBuyForPdSer = (dtrTemPd["cIOType"].ToString() == "I" ? "P" : "S");
                    //    this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    //    //this.mPdSer.DelItemRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrTemPd["cProd"].ToString(), dtrTemPd["cRowID"].ToString(), dtrTemPd["cWHouse"].ToString(), dtrTemPd["cWHLoca"].ToString(), dtrTemPd["cLot"].ToString());
                    //}

                }
            }
            return true;
        }

        private bool pmReplRecordRefProd
            (
            bool inState, DataRow inTemPd, ref DataRow ioRefProd, string inRowID,
            decimal inQty, decimal inUmQty, decimal inStQty, decimal inStUmQty
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
            dtrRefProd["fcRefPdTyp"] = "P";
            dtrRefProd["fcProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrRefProd["fcRootSeq"] = "";
            dtrRefProd["fcShowComp"] = "";
            dtrRefProd["fcPformula"] = "";
            dtrRefProd["fcFormulas"] = "";
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
            dtrRefProd["fcIoType"] = (Convert.ToDecimal(inTemPd["nAddQty"]) > 0 ? "I" : "O");
            dtrRefProd["fcSect"] = (inTemPd["cDept"].ToString() != string.Empty ? inTemPd["cDept"].ToString() : dtrGLRef["fcSect"].ToString());
            dtrRefProd["fcDept"] = (inTemPd["cDivision"].ToString() != string.Empty ? inTemPd["cDivision"].ToString() : dtrGLRef["fcDept"].ToString());
            dtrRefProd["fcJob"] = (inTemPd["cJob"].ToString() != string.Empty ? inTemPd["cJob"].ToString() : dtrGLRef["fcJob"].ToString());
            dtrRefProd["fcProj"] = (inTemPd["cProject"].ToString() != string.Empty ? inTemPd["cProject"].ToString() : dtrGLRef["fcProj"].ToString());
            dtrRefProd["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrRefProd["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrRefProd["fnXRate"] = 1;
            dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrRefProd["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrRefProd["fcUmStd"] = inTemPd["cUOMStd"].ToString();
            dtrRefProd["fnQty"] = Math.Abs(Convert.ToDecimal(inTemPd["nAddQty"]) - Convert.ToDecimal(inTemPd["nSubQty"]));

            dtrRefProd["fcWHLoca"] = inTemPd["cWHLoca"].ToString();

            dtrRefProd["fcLUpdApp"] = App.AppID;
            dtrRefProd["fcDataser"] = "";
            dtrRefProd["fcEAfterR"] = 'E';

            dtrRefProd["fnStQty"] = Convert.ToDecimal(inTemPd["nStQty"]);
            dtrRefProd["fcStUm"] = inTemPd["cUOMStd"].ToString();
            dtrRefProd["fnStUmQty"] = Convert.ToDecimal(inTemPd["nStUOMQty"]);
            dtrRefProd["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmUpdateStock(inQty, inUmQty, inStQty, inStUmQty, ref dtrRefProd, inTemPd);
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
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "RefProd", "select * from " + this.mstrITable + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    //this.pmTest(false , true, dtrTemPd);
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

            decimal decOutStockQty = 0;
            decimal decOutWHLocaQty = 0;

            string strWHLoca = "";
            strWHLoca = dtrRefProd["fcWHLoca"].ToString();

            decimal lnUpdCostAmt = Math.Round(Convert.ToDecimal(dtrRefProd["fnPrice"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]) * Convert.ToDecimal(dtrRefProd["fnQty"]), 4);
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

            //lnCostAmt = Math.Round( Convert.ToDecimal(dtrTemPd["nPrice"]) * Convert.ToDecimal(dtrRefProd["fnQty"]) , 4);
            if (bllSucc)
            {
                dtrRefProd["fnCostAmt"] = lnCostAmt;
            }

            return bllSucc;
        }
        
        private bool XXXpmUpdateStockForInsert()
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

        private bool XXXpmUpdateStock(decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, ref DataRow ioRefProd, DataRow inTemPd)
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
                        //this.mSaveDBAgent2.SetPara(new object[1] { dtrRefProd["fcSkid"].ToString() });
                        //if (this.mSaveDBAgent2.SQLExec(ref this.dtsDataEnv, "QXaRefProd", "xaRefProd", "select cWHLoca from xaRefProd where cRefProd = ?", ref strErrorMsg))
                        //{
                        //    strWHLoca = this.dtsDataEnv.Tables["QXaRefProd"].Rows[0]["cWHLoca"].ToString();
                        //    this.mPdSer.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                        //    this.mPdSer.DelItemRefPdX3(this.mSaveDBAgent, this.mdbConn, this.mdbTran, this.dtsDataEnv, App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrSaleOrBuyForPdSer, dtrRefProd["fcProd"].ToString(), strRefItem, dtrGLRef["fcFrWhouse"].ToString(), strWHLoca, dtrRefProd["fcLot"].ToString());
                        //}
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
            //รายการสินค้า
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
            dtbTemPd.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOldQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastAddQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastSubQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("nAddQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nSubQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOQtyInUm", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStQtyInUm", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStUmQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStUOMQty", System.Type.GetType("System.Decimal"));
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

            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"), "nPrice * nQty");
            dtbTemPd.Columns.Add("cDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDivision", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProject", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefTo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIsDelete", System.Type.GetType("System.String"));

            dtbTemPd.Columns.Add("nQtyAtDat", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLQtyAtDat", System.Type.GetType("System.Decimal"));

            //เรื่อง ชุดสินค้า
            dtbTemPd.Columns.Add("cRootSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcPFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQtyPerMFm", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastFormula", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastRefPdType", System.Type.GetType("System.String"));

            //เรื่อง Location
            //dtbTemPd.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            //dtbTemPd.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            //dtbTemPd.Columns["cWHLoca"].DefaultValue = "";
            //dtbTemPd.Columns["cQcWHLoca"].DefaultValue = "";

            dtbTemPd.Columns.Add("fnAgeLong", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("fdExpire", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns["fnAgeLong"].DefaultValue = 0;

            dtbTemPd.Columns.Add("cAttachFile", System.Type.GetType("System.String"));

            dtbTemPd.Columns["cRootSeq"].DefaultValue = "";
            dtbTemPd.Columns["cQcPFormula"].DefaultValue = "";
            dtbTemPd.Columns["cPFormula"].DefaultValue = "";
            dtbTemPd.Columns["nQtyPerMFm"].DefaultValue = 0;
            dtbTemPd.Columns["cLastFormula"].DefaultValue = "";
            dtbTemPd.Columns["cLastRefPdType"].DefaultValue = "";

            dtbTemPd.Columns["cRefPdType"].DefaultValue = SysDef.gc_REFPD_TYPE_PRODUCT;


            //TODO: เรื่องการ Update Stock
            //"nLQty" ==> nLastQty
            dtbTemPd.Columns.Add("cCtrlStoc", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOUmQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOStQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIOType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns["nActRow"].AutoIncrement = true;

            dtbTemPd.Columns["cStep"].DefaultValue = SysDef.gc_STEP_CREATED;
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOldQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastAddQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastSubQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nStQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cDiscStr"].DefaultValue = "";
            dtbTemPd.Columns["nDiscAmt"].DefaultValue = 0;
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
            dtbTemPd.Columns["cDept"].DefaultValue = "";
            dtbTemPd.Columns["cQcDept"].DefaultValue = "";
            dtbTemPd.Columns["cQnDept"].DefaultValue = "";
            dtbTemPd.Columns["cDivision"].DefaultValue = "";
            dtbTemPd.Columns["cJob"].DefaultValue = "";
            dtbTemPd.Columns["cQcJob"].DefaultValue = "";
            dtbTemPd.Columns["cQnJob"].DefaultValue = "";
            dtbTemPd.Columns["cProject"].DefaultValue = "";
            dtbTemPd.Columns["cLastPdType"].DefaultValue = "";
            dtbTemPd.Columns["cLastProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQnProd"].DefaultValue = "";
            dtbTemPd.Columns["cWHLoca"].DefaultValue = "";
            dtbTemPd.Columns["cSeq"].DefaultValue = "";
            dtbTemPd.Columns["cIOType"].DefaultValue = "";
            dtbTemPd.Columns["cSeq"].DefaultValue = "";

            dtbTemPd.Columns["nQtyAtDat"].DefaultValue = 0;
            dtbTemPd.Columns["nQtyAtDat"].DefaultValue = 0;
            dtbTemPd.Columns["nLQtyAtDat"].DefaultValue = 0;
            dtbTemPd.Columns["nAddQty"].DefaultValue = 0;
            dtbTemPd.Columns["nSubQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOQtyInUm"].DefaultValue = 0;
            dtbTemPd.Columns["nOUmQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStQtyInUm"].DefaultValue = 0;
            dtbTemPd.Columns["nOStUmQty"].DefaultValue = 0;
            dtbTemPd.Columns["nBackQty"].DefaultValue = 0;

            dtbTemPd.Columns["cCtrlStoc"].DefaultValue = "0";
            dtbTemPd.Columns["nOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nOStQty"].DefaultValue = 0;
            //TODO: เรื่องการ Update Stock

            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemRefTo = new DataTable(xd_ALIAS_TEMREFTO);
            dtbTemRefTo.Columns.Add("cRowID", System.Type.GetType("System.String"));
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

            this.dtsDataEnv.Tables.Add(dtbTemBrowRefTo);


            //this.dtsDataEnv.Tables.Add(this.mPdSer.CreateTemPdSer());

            //DataTable dtb1PdSer = this.mPdSer.CreateTemPdSer();
            //dtb1PdSer.TableName = xd_Alias_Tem1PdSer;
            //this.dtsDataEnv.Tables.Add(dtb1PdSer);

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);

        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            //e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private string mstrTemPack = "TemPack";

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
            this.mstrPlant = "";

            this.mstrStep = SysDef.gc_STEP_IGNORE;
            this.mstrAtStep = "";//SysDef.gc_ATSTEP_VOUCHER_WAIT;

            this.txtRemark.Text = "";

            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQcJob.Tag = "";
            this.txtQcJob.Text = "";

            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.mdecRefToAmt = 0;
            this.mstrOldRefToRowID = "";
            this.mstrOldRefToMOrderOP = "";
            this.mstrRefToMOrderOP = "";
            this.mstrRefToWOrderI = "";

            this.mdecSumMOQty = 0;
            this.mdecSumGoodQty = 0;
            this.mdecSumWasteQty = 0;
            this.mdecSumLossQty = 0;

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

            this.txtRemark.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cPdType"];

            this.pgfEditItem.SelectedTabPageIndex = 0;
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

                    dtrTemPd["nQty"] = Math.Abs(Convert.ToDecimal(dtrTemPd["nQty"])) * (dtrTemPd["cIOType"].ToString() == "I" ? 1 : -1);

                    dtrTemPd["nAddQty"] = dtrTemPd["cIOType"].ToString() == "I" ? Math.Abs(Convert.ToDecimal(dtrTemPd["nQty"])) : 0;
                    dtrTemPd["nSubQty"] = dtrTemPd["cIOType"].ToString() == "I" ? 0 : Math.Abs(Convert.ToDecimal(dtrTemPd["nQty"]));
                    dtrTemPd["nOQtyInUm"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                    dtrTemPd["nOUmQty"] = Convert.ToDecimal(dtrTemPd["nUomQty"]);
                    dtrTemPd["nOStQtyInUm"] = Convert.ToDecimal(dtrTemPd["nStQty"]);
                    dtrTemPd["nOStUmQty"] = Convert.ToDecimal(dtrTemPd["nStUOMQty"]);
                    dtrTemPd["nLastAddQty"] = Convert.ToDecimal(dtrTemPd["nAddQty"]);
                    dtrTemPd["nLastSubQty"] = Convert.ToDecimal(dtrTemPd["nSubQty"]);

                    ////Load RefPdX3
                    //if (this.mbllIsGetPdSer)
                    //{
                    //    this.mPdSer.LoadItemTRefPdX3(pobjSQLUtil, this.dtsDataEnv, Convert.ToInt32(dtrTemPd["nActRow"]), App.ActiveCorp.RowID, this.mstrBranchID, this.mstrRefType, dtrTemPd["cRowID"].ToString(), dtrTemPd["cProd"].ToString(), false, "", "", dtrTemPd["cWHouse"].ToString());
                    //}

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
            dtrTemPd["cProd"] = inRefProd["fcProd"].ToString();
            dtrTemPd["cLastProd"] = inRefProd["fcProd"].ToString();
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
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inRefProd["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(inRefProd["fnUmQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inRefProd["fnQty"]);
            dtrTemPd["nPrice"] = Convert.ToDecimal(inRefProd["fnPriceKe"]);
            dtrTemPd["cDiscStr"] = inRefProd["fcDiscStr"].ToString().TrimEnd();
            dtrTemPd["nDiscAmt"] = Convert.ToDecimal(inRefProd["fnDiscAmt"]);
            dtrTemPd["cIOType"] = inRefProd["fcIOType"].ToString();
            dtrTemPd["cSeq"] = inRefProd["fcSeq"].ToString();

            dtrTemPd["nStQty"] = Convert.ToDecimal(inRefProd["fnStQty"]);
            dtrTemPd["cStUOm"] = inRefProd["fcStUm"].ToString();
            dtrTemPd["cStUOmStd"] = inRefProd["fcStUmStd"].ToString();
            dtrTemPd["nStUOmQty"] = Convert.ToDecimal(inRefProd["fnStUmQty"]);

            dtrTemPd["cLot"] = inRefProd["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inRefProd["fcWHouse"].ToString();
            dtrTemPd["cWHLoca"] = inRefProd["fcWHLoca"].ToString();

            //pobjSQLUtil.SetPara(new object[1] {dtrTemPd["cProd"].ToString().TrimEnd()});
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

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHLoca"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHOUSE", "select FCCODE,FCNAME from WHLocation where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcCode"].ToString().TrimEnd();
            }

            if (dtrTemPd["cWHLoca"].ToString().TrimEnd() == string.Empty)
            {
                //if (this.mDefaWHLoca != null)
                //{
                //    dtrTemPd["cWHLoca"] = this.mDefaWHLoca.RowID;
                //    dtrTemPd["cQcWHLoca"] = this.mDefaWHLoca.Code.TrimEnd();
                //}
            }

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            return dtrTemPd;
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
                //switch (this.mDocType)
                //{
                //    case DocumentType.AJ:
                //    case DocumentType.WX:
                //        strWHouse = this.txtFrQcWHouse.Tag.ToString();
                //        strQnWHouse = this.txtFrQcWHouse.Text.Trim();
                //        break;
                //    case DocumentType.RW:
                //    case DocumentType.RX:
                //        strWHouse = this.txtToQcWHouse.Tag.ToString();
                //        strQnWHouse = this.txtToQcWHouse.Text.Trim();
                //        break;
                //    case DocumentType.FR:
                //        strWHouse = this.txtToQcWHouse.Tag.ToString();
                //        strQnWHouse = this.txtToQcWHouse.Text.Trim();
                //        break;
                //    case DocumentType.TR:
                //        strWHouse = this.txtFrQcWHouse.Tag.ToString();
                //        strQnWHouse = this.txtFrQcWHouse.Text.Trim();
                //        break;
                //}
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
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCPROD" ? "FCCODE" : "FCNAME");
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
                        string strOrderBy = (strCol.ToUpper() == "CQCPROD" ? "FCCODE" : "FCNAME");
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
                    , dtrTemPd["cWHouse"].ToString()
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
