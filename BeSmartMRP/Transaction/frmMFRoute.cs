
#define xd_RUNMODE_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using BeSmartMRP.DatabaseForms;

namespace BeSmartMRP.Transaction
{

    public partial class frmMFRoute : UIHelper.frmBase
    {

        public static string TASKNAME = "EROUTECD";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;
        private const int xd_PAGE_EDIT2 = 2;
        private const int xd_PAGE_EDIT3 = 3;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWCTranHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWCTranIT;

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrActiveOP = "";
        private string mstrSortKey = QMFWCTranHDInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrTemFG = "TemFG";
        private string mstrTemPd = "TemPd";
        private string mstrTemOP = "TemOP";
        private string mstrTemPdX1 = "TemPdX1";
        private string mstrTemRoute = "TemRoute";

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
        private string mstrRefType = DocumentType.RJ.ToString();
        private string mstrRfType = "O";
        private DocumentType mRefType = DocumentType.RJ;
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
        private string mstrRefToRefType = DocumentType.MO.ToString();
        private string mstrRefToTab = QMFWOrderHDInfo.TableName;
        private string mstrRefToTab2 = QMFWOrderIT_OPInfo.TableName;
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
        private string mstrStep = "";

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

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        private DatabaseForms.frmMFResType pofrmGetResType = null;
        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DatabaseForms.frmMFWorkCenter pofrmGetWkCtr = null;
        private DatabaseForms.frmMFStdOpr pofrmGetStdOP = null;
        private DatabaseForms.frmMFResource pofrmGetResource = null;

        private DialogForms.dlgGetProdType pofrmGetProdType = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetUM pofrmGetUM = null;

        private Common.MRP.dlgGetRefToRoute pofrmGetOPSeq = null;
        private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DialogForms.dlgGetBOM pofrmGetBOM = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;

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

        public frmMFRoute()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFRoute(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
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
        
        private static frmMFRoute mInstanse_1 = null;

        public static frmMFRoute GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFRoute();
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

        private void frmMFRoute_Load(object sender, EventArgs e)
        {

            //this.WindowState = FormWindowState.Minimized;
            //this.WindowState = FormWindowState.Maximized;

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
            this.pmInitGridProp_TemFG();
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

            UIHelper.UIBase.CreateTransactionToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();
            this.pmSetFormUI();

            this.mstrPdType = this.pmSplitToSQLStr(this.pmGetRefProdType());

            UIBase.SetDefaultChildAppreance(this);

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

            this.mstrFormMenuName = UIBase.GetAppUIText(new string[] { "ใบส่งมอบงานระหว่าง W/C", "ROUTE CARD JOURNAL" });
            this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            this.lblCode.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Doc. Code" }) });
            this.lblRefNo.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." }) });
            this.lblDate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Doc. Date" }) });

            this.lblRefer.Text = UIBase.GetAppUIText(new string[] { "#อ้างอิง M/O :", "#Refer M/O :" });
            this.lblStartDate.Text = UIBase.GetAppUIText(new string[] { "วันที่เริ่มต้น :", "Start Date/Time :" });
            this.lblDueDate.Text = UIBase.GetAppUIText(new string[] { "วันที่สิ้นสุด :", "Finish Date/Time :" });
            this.lblPlanDueDate.Text = UIBase.GetAppUIText(new string[] { "วันที่ผลิตเสร็จตามแผน :", "Due Date/Time :" });
            this.lblOPTime.Text = UIBase.GetAppUIText(new string[] { "เวลาในการผลิต :", "Opr. Time :" });

            //this.lblFrWkCtr.Text = UIBase.GetAppUIText(new string[] { "รหัส W/C :", "W/C :" });
            this.lblFrOP.Text = UIBase.GetAppUIText(new string[] { "OP Seq. :", "OP. Seq :" });
            //this.lblToWkCtr.Text = UIBase.GetAppUIText(new string[] { "โอนไป W/C :", "Next W/C :" });
            this.lblToOP.Text = UIBase.GetAppUIText(new string[] { "ไป OP Seq. :", "Next OP. Seq :" });

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });

            this.btnGetOPSeq.Text = UIBase.GetAppUIText(new string[] { "ระบุขั้นตอนการผลิต", "Select OP. Seq" });
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWCTranHDInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWCTranHDInfo.Field.RefNo);

            this.txtRemark1.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark1);
            this.txtRemark2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark2);
            this.txtRemark3.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark3);
            this.txtRemark4.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark4);
            this.txtRemark5.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark5);
            this.txtRemark6.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark6);
            this.txtRemark7.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark7);
            this.txtRemark8.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark8);
            this.txtRemark9.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark9);
            this.txtRemark10.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCTranHDInfo.TableName, QMFWCTranHDInfo.Field.Remark10);

        }

        private void pmMapEvent()
        {
            this.Resize += new EventHandler(frmMFRoute_Resize);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.grdBrowView.Resize += new EventHandler(grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new ColumnEventHandler(gridView1_ColumnWidthChanged);

            this.grdTemPd.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView3.ColumnWidthChanged += new ColumnEventHandler(gridView3_ColumnWidthChanged);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);
            this.gridView3.GotFocus += new EventHandler(gridView3_GotFocus);

            this.grdTemFG.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView4.ColumnWidthChanged += new ColumnEventHandler(gridView4_ColumnWidthChanged);
            this.gridView4.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView4_ValidatingEditor);
            this.gridView4.GotFocus += new EventHandler(gridView4_GotFocus);

            this.grcQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);


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

            this.txtStartDate.Validating += new CancelEventHandler(txtStartDate_Validating);

        }

        private void frmMFRoute_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
            this.pmCalcColWidth2();
            this.pmCalcColWidth3();
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";

            string strStep = this.pmGenOrderStep();

            string strSQLExec = "";

            strSQLExec = "select WCTRANH.CROWID, WCTRANH.CSTAT, " + strStep + ", WCTRANH.CCODE, WCTRANH.CREFNO, WCTRANH.DDATE, WORDERH.CREFNO as REFNO_WO , WCTRANH.CFROPSEQ , WCTRANH.CTOOPSEQ ";
            strSQLExec += ",FRMOPR.CNAME AS QNFRMOPR ";
            strSQLExec += ",TOMOPR.CNAME AS QNTOMOPR ";
            strSQLExec += ",SECT.FCCODE AS QCSECT ";
            strSQLExec += ",JOB.FCCODE AS QCJOB ";
            strSQLExec += " , WCTRANH.DCREATE, WCTRANH.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} WCTRANH ";
            //strSQLExec += " left join " + strProdTab + " PROD on PROD.FCSKID = WCTRANH.CMFGPROD ";
            strSQLExec += " left join MFSTDOPR FRMOPR on FRMOPR.CROWID = WCTRANH.CFRMOPR ";
            strSQLExec += " left join MFSTDOPR TOMOPR on TOMOPR.CROWID = WCTRANH.CTOMOPR ";
            strSQLExec += " left join REFDOC on REFDOC.CMASTERTYP = 'RJ' and REFDOC.CMASTERH = WCTRANH.CROWID ";
            strSQLExec += " left join MFWORDERHD WORDERH on WORDERH.CROWID = REFDOC.CCHILDH ";
            strSQLExec += " left join " + strSectTab + " SECT on SECT.FCSKID = WCTRANH.CSECT ";
            strSQLExec += " left join " + strJobTab + " JOB on JOB.FCSKID = WCTRANH.CJOB ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = WCTRANH.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = WCTRANH.CLASTUPDBY ";
            strSQLExec += " where WCTRANH.CCORP = ? and WCTRANH.CBRANCH = ? and WCTRANH.CPLANT = ? and WCTRANH.CREFTYPE = ? and WCTRANH.CMFGBOOK = ? and WCTRANH.DDATE between ? and ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "WCTRANH", strSQLExec, ref strErrorMsg);

        }

        private string pmGenOrderStep()
        {
            string strStep = "";
            strStep = "CSTEP = case WCTRANH.CSTEP when 'P' then 'FINISH' when 'L' then 'CLOSE' when '1' then ' ' end";
            return strStep;
        }

        private void pmInitGridProp_TemFG()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemFG].DefaultView;

            this.grdTemFG.DataSource = this.dtsDataEnv.Tables[this.mstrTemFG];

            for (int intCnt = 0; intCnt < this.gridView4.Columns.Count; intCnt++)
            {
                this.gridView4.Columns[intCnt].Visible = false;
            }

            int i = 0;

            this.gridView4.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView4.Columns["cPdType"].VisibleIndex = i++;
            this.gridView4.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView4.Columns["cQnProd"].VisibleIndex = i++;
            //this.gridView4.Columns["dFinish"].VisibleIndex = i++;
            this.gridView4.Columns["nMOQty"].VisibleIndex = i++;
            //this.gridView4.Columns["nBFQty"].VisibleIndex = i++;
            this.gridView4.Columns["nQty"].VisibleIndex = i++;
            this.gridView4.Columns["nWasteQty"].VisibleIndex = i++;
            this.gridView4.Columns["nLossQty1"].VisibleIndex = i++;
            //this.gridView4.Columns["nHoldQty"].VisibleIndex = i++;
            this.gridView4.Columns["cQnUOM"].VisibleIndex = i++;

            this.gridView4.Columns["nRecNo"].Caption = "No.";
            this.gridView4.Columns["cPdType"].Caption = "T";
            this.gridView4.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView4.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView4.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน MO", "MO" });
            this.gridView4.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ของดี", "Good" });
            this.gridView4.Columns["nWasteQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ของเสีย", "N/G" });
            this.gridView4.Columns["nLossQty1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "สูญเสีย", "Loss" });
            this.gridView4.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });

            this.gridView4.Columns["nMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nMOQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["nWasteQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nWasteQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["nLossQty1"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nLossQty1"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["nRecNo"].Width = 50;
            this.gridView4.Columns["cPdType"].Width = 50;
            this.gridView4.Columns["cQcProd"].Width = 130;
            this.gridView4.Columns["nMOQty"].Width = 80;
            this.gridView4.Columns["nQty"].Width = 80;
            this.gridView4.Columns["nWasteQty"].Width = 80;
            this.gridView4.Columns["nLossQty1"].Width = 80;
            this.gridView4.Columns["cQnUOM"].Width = 80;
            //this.gridView4.Columns["cSubSti"].Width = 5;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW3_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW3_CPDTYPE";

            this.gridView4.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["nMOQty"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["nMOQty"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["nMOQty"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView4.Columns["nWasteQty"].ColumnEdit = this.grcQty;
            this.gridView4.Columns["nLossQty1"].ColumnEdit = this.grcQty;

            this.gridView4.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView4.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            //this.gridView4.Columns["cRemark1"].ColumnEdit = this.grcRemark2;

            this.pmCalcColWidth3();
        }

        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView3.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView3.Columns["cPdType"].VisibleIndex = i++;
            this.gridView3.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView3.Columns["cQnProd"].VisibleIndex = i++;
            //this.gridView3.Columns["dFinish"].VisibleIndex = i++;
            //this.gridView3.Columns["cRemark1"].VisibleIndex = i++;
            //this.gridView3.Columns["cLot"].VisibleIndex = i++;
            //this.gridView3.Columns["nMfgQty"].VisibleIndex = i++;
            this.gridView3.Columns["nMOQty"].VisibleIndex = i++;
            this.gridView3.Columns["nQty"].VisibleIndex = i++;
            //this.gridView3.Columns["nGoodQty"].VisibleIndex = i++;
            //this.gridView3.Columns["nWasteQty"].VisibleIndex = i++;
            //this.gridView3.Columns["nLossQty1"].VisibleIndex = i++;
            this.gridView3.Columns["cQnUOM"].VisibleIndex = i++;


            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["cPdType"].Caption = "T";
            this.gridView3.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView3.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView3.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน M/O", "MO. Qty." });
            this.gridView3.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView3.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });

            this.gridView3.Columns["nRecNo"].Width = 50;
            this.gridView3.Columns["cPdType"].Width = 50;
            this.gridView3.Columns["cQcProd"].Width = 130;
            this.gridView3.Columns["nMOQty"].Width = 80;
            this.gridView3.Columns["nQty"].Width = 80;
            this.gridView3.Columns["cQnUOM"].Width = 80;

            this.gridView3.Columns["nMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nMOQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView3.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW3_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW3_CPDTYPE";
            //this.grcRemark2.Buttons[0].Tag = "GRDVIEW3_CREMARK1";

            this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");
            //this.grcRemark.MaxLength = 150;
            //this.grcRemark2.MaxLength = 150;

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nMOQty"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nMOQty"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nMOQty"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView3.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView3.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            //this.gridView3.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            //this.gridView3.Columns["cScrap"].ColumnEdit = this.grcScrap;

            this.pmCalcColWidth2();
        }

        private void pmCalcColWidth1()
        {

            int intColWidth = this.gridView1.Columns[QMFWCTranHDInfo.Field.Stat].Width
                                    + this.gridView1.Columns[QMFWCTranHDInfo.Field.Step].Width
                                    + this.gridView1.Columns[QMFWCTranHDInfo.Field.Code].Width
                                    + this.gridView1.Columns[QMFWCTranHDInfo.Field.RefNo].Width
                                    + this.gridView1.Columns[QMFWCTranHDInfo.Field.Date].Width;

            //int intNewWidth = this.Width - intColWidth - 60;
            //this.gridView3.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView3.Columns["nRecNo"].Width
                                    + this.gridView3.Columns["cPdType"].Width
                                    + this.gridView3.Columns["cQcProd"].Width
                                    + this.gridView3.Columns["nQty"].Width
                                    + this.gridView3.Columns["nMOQty"].Width
                                    + this.gridView3.Columns["cQnUOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView3.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth3()
        {

            int intColWidth = this.gridView4.Columns["nRecNo"].Width
                                    + this.gridView4.Columns["cPdType"].Width
                                    + this.gridView4.Columns["cQcProd"].Width
                                    + this.gridView4.Columns["nMOQty"].Width
                                    + this.gridView4.Columns["nQty"].Width
                                    + this.gridView4.Columns["nWasteQty"].Width
                                    + this.gridView4.Columns["nLossQty1"].Width
                                    + this.gridView4.Columns["cQnUOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView4.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFWCTranHDInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Stat].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Step].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.RefNo].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Date].VisibleIndex = i++;
            this.gridView1.Columns["REFNO_WO"].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.FrOPSeq].VisibleIndex = i++;
            this.gridView1.Columns["QNFRMOPR"].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.ToOPSeq].VisibleIndex = i++;
            this.gridView1.Columns["QCSECT"].VisibleIndex = i++;
            this.gridView1.Columns["QCJOB"].VisibleIndex = i++;

            //this.gridView1.Columns["QCPROD"].VisibleIndex = i++;

            //this.gridView1.Columns[QMFWCTranHDInfo.Field.IsApprove].Caption = "Status";

            this.gridView1.Columns[QMFWCTranHDInfo.Field.Stat].Width = 30;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Step].Width = 75;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Code].Width = 90;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.RefNo].Width = 100;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Date].Width = 80;
            this.gridView1.Columns["REFNO_WO"].Width = 120;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.FrOPSeq].Width = 50;
            this.gridView1.Columns["QNFRMOPR"].Width = 90;
            this.gridView1.Columns[QMFWCTranHDInfo.Field.ToOPSeq].Width = 90;
            this.gridView1.Columns["QCSECT"].Width = 50;
            this.gridView1.Columns["QCJOB"].Width = 50;
            //this.gridView1.Columns["QCPROD"].Width = 120;

            this.gridView1.Columns[QMFWCTranHDInfo.Field.Stat].Caption = "C";
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Step].Caption = "STEP";
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Code].Caption = UIBase.GetAppUIText(new string[] {"เลขที่ภายใน","DOC. CODE"});
            this.gridView1.Columns[QMFWCTranHDInfo.Field.RefNo].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });
            this.gridView1.Columns[QMFWCTranHDInfo.Field.Date].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            this.gridView1.Columns["REFNO_WO"].Caption = UIBase.GetAppUIText(new string[] { "#MO", "#MO" });
            this.gridView1.Columns[QMFWCTranHDInfo.Field.FrOPSeq].Caption = UIBase.GetAppUIText(new string[] { "OP Seq.", "OP Seq." });
            this.gridView1.Columns["QNFRMOPR"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อ OP", "OP Name" });
            this.gridView1.Columns[QMFWCTranHDInfo.Field.ToOPSeq].Caption = UIBase.GetAppUIText(new string[] { "ไปยัง OP", "Next OP" });
            this.gridView1.Columns["QCSECT"].Caption = UIBase.GetAppUIText(new string[] { "แผนก", "Section" });
            this.gridView1.Columns["QCJOB"].Caption = UIBase.GetAppUIText(new string[] { "โครงการ", "Job" });
            //this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "PROD. CODE" });

            this.pmSetSortKey(QMFWCTranHDInfo.Field.Code, true);
        
        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void gridView4_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemFG;
            this.mActiveGrid = this.gridView4;
            //this.gridView2.FocusedColumn = this.gridView2.Columns["cOPSeq"];
        }

        private void grdTemFG_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth3();
        }

        private void gridView4_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth3();
        }

        private void grdTemPd_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth2();
        }

        private void gridView3_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth2();
        }

        private void gridView3_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemPd;
            this.mActiveGrid = this.gridView3;
            //this.gridView3.FocusedColumn = this.gridView3.Columns["cOPSeq"];
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

        private void pmInitPopUpDialog(string inDialogName)
        {

            string strErrorMsg = "";
            DataRow dtrGetVal = null;
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Common.dlgFilter02 dlgFilter = new Common.dlgFilter02(this.mstrRefType))
                    {

                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrPlant = dlgFilter.PlantID;
                            this.mstrBranch = dlgFilter.BranchID;
                            this.mstrBook = dlgFilter.BookID;
                            this.mstrQcBook = dlgFilter.BookCode;
                            this.mdttBegDate = dlgFilter.BegDate.Date;
                            this.mdttEndDate = dlgFilter.EndDate.Date;

                            //this.mstrDefaSect = (oMfgBookInfo.Department.Tag.TrimEnd() != string.Empty ? oMfgBookInfo.Department.Tag : (oPlantInfo.Department.RowID.TrimEnd() != string.Empty) ? oPlantInfo.Department.RowID : (App.ActiveCorp.DefaultSectID.TrimEnd() != string.Empty) ? App.ActiveCorp.DefaultSectID.TrimEnd() : "");
                            //this.mstrDefaJob = (oMfgBookInfo.Job.Tag.TrimEnd() != string.Empty ? oMfgBookInfo.Job.Tag : (oPlantInfo.Job.RowID.TrimEnd() != string.Empty) ? oPlantInfo.Job.RowID : (App.ActiveCorp.DefaultJobID.TrimEnd() != string.Empty) ? App.ActiveCorp.DefaultJobID.TrimEnd() : "");

                            this.mstrDefaSect = App.ActiveCorp.DefaultSectID;
                            this.mstrDefaJob = App.ActiveCorp.DefaultJobID;

                            this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" }) + "\\" + dlgFilter.PlantCode + "\\" + dlgFilter.BookCode;

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();

                        }
                    }
                    break;
                case "REFTO":
                    using (Common.MRP.dlgGetRefToWO dlgRefTo = new Common.MRP.dlgGetRefToWO(this.mstrRefToTab, this.mstrRefToRefType, this.mstrBranch, this.mstrPlant, this.mstrRefToBook, this.mstrRefToRowID))
                    {
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

                                    //this.pmLoadOPSeq();
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
                                            //DataRow dtrSelOP = this.dtsDataEnv.Tables[this.mstrTemRoute].Rows[dlg.SelectedRow()];
                                            //this.mstrRefToMOrderOP = dtrSelOP["cRowID"].ToString();
                                            //this.txtFrOPSeq.Text = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                                            //this.txtFrQnOP.Tag = dtrSelOP["cFrMOPR"].ToString().TrimEnd();
                                            //this.txtFrQnOP.Text = dtrSelOP["cQnFrMOPR"].ToString().TrimEnd();
                                            //this.txtToOPSeq.Text = dtrSelOP["cToOPSeq"].ToString().TrimEnd();
                                            //this.txtToQnOP.Tag = dtrSelOP["cToMOPR"].ToString().TrimEnd();
                                            //this.txtToQnOP.Text = dtrSelOP["cQnToMOPR"].ToString().TrimEnd();

                                            //this.pmLoadSemiFormOPSeq();
                                            //this.pmLoadRMFormOPSeq();
                                            //this.pmLoadSectByWorkCenter(this.txtFrQnOP.Tag.ToString());

                                        }
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
                            //DataRow dtrSelOP = this.dtsDataEnv.Tables[this.mstrTemRoute].Rows[dlg.SelectedRow()];
                            //this.mstrRefToMOrderOP = dtrSelOP["cRowID"].ToString();
                            //this.txtFrOPSeq.Text = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                            //this.txtFrQnOP.Tag = dtrSelOP["cFrMOPR"].ToString().TrimEnd();
                            //this.txtFrQnOP.Text = dtrSelOP["cQnFrMOPR"].ToString().TrimEnd();
                            //this.txtToOPSeq.Text = dtrSelOP["cToOPSeq"].ToString().TrimEnd();
                            //this.txtToQnOP.Tag = dtrSelOP["cToMOPR"].ToString().TrimEnd();
                            //this.txtToQnOP.Text = dtrSelOP["cQnToMOPR"].ToString().TrimEnd();

                            //this.pmLoadSemiFormOPSeq();
                            //this.pmLoadRMFormOPSeq();
                            //this.pmLoadSectByWorkCenter(this.txtFrQnOP.Tag.ToString());

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
                case "RESOURCE":
                    if (this.pofrmGetResource == null)
                    {
                        this.pofrmGetResource = new frmMFResource(FormActiveMode.PopUp);
                        this.pofrmGetResource.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetResource.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    this.pofrmGetResource.PlantID = this.mstrPlant;
                    this.pofrmGetResource.ResourceType = MfgResourceType.Tool;
                    this.pofrmGetResource.ReInitForm();
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
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor();
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "BOM":
                    if (this.pofrmGetBOM == null)
                    {
                        this.pofrmGetBOM = new DialogForms.dlgGetBOM();
                        this.pofrmGetBOM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBOM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            }
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
                this.txtToOPSeq.Text = dtrSelOP["cToOPSeq"].ToString().TrimEnd();
                this.txtToQnOP.Tag = dtrSelOP["cToMOPR"].ToString();
                this.txtToQnOP.Text = dtrSelOP["cQnToMOPR"].ToString().TrimEnd();
                this.mstrFrWkCtrH = dtrSelOP["cFrWkCtr"].ToString();
                this.mstrToWkCtrH = dtrSelOP["cToWkCtr"].ToString();

                if (this.mstrRefToMOrderOP != dtrSelOP["cRowID"].ToString())
                {
                    this.mstrRefToMOrderOP = dtrSelOP["cRowID"].ToString();

                    this.pmLoadSemiFormOPSeq();
                    this.pmLoadRMFormOPSeq();
                    this.pmLoadSectByWorkCenter(this.txtFrQnOP.Tag.ToString());

                }
            }

        }

        private void pmClearRefTo()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.txtRefTo.Text = "";

            this.txtFrOPSeq.Text = "";
            this.txtFrQnOP.Text = "";

            this.txtToOPSeq.Text = "";
            this.txtToQnOP.Text = "";

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
            strSQLStr += " where WORDEROP.CWORDERH = ? and WORDEROP.CSTEP <> ? ";
            strSQLStr += " order by WORDEROP.COPSEQ";

            string strSQLStr2 = "";
            string strFld2 = "WORDEROP.CROWID, WORDEROP.COPSEQ, WORDEROP.CNEXTOP, WORDEROP.CWKCTRH, WKCTRH.CCODE as QcWkCtr , WKCTRH.CNAME as QnWkCtr";
            strFld2 += " , WORDEROP.NT_QUEUE, WORDEROP.NT_SETUP, WORDEROP.NT_PROCESS, WORDEROP.NT_TEAR, WORDEROP.NT_WAIT ";
            strFld2 += " , MFSTDOPR.CROWID as cFrMOPR , MFSTDOPR.CCODE as QcStdOP , MFSTDOPR.CNAME as QnStdOP ";
            strSQLStr2 = "select " + strFld + " from MFWORDERIT_STDOP WORDEROP ";
            strSQLStr2 += " left join MFWKCTRHD WKCTRH on WKCTRH.CROWID = WORDEROP.CWKCTRH ";
            strSQLStr2 += " left join MFSTDOPR on MFSTDOPR.CROWID = WORDEROP.CMOPR ";
            //strSQLStr2 += " where WORDEROP.CWORDERH = ? ";
            strSQLStr2 += " where WORDEROP.CWORDERH = ? and WORDEROP.COPSEQ = ? and WORDEROP.CSTEP <> ? ";
            strSQLStr2 += " order by WORDEROP.COPSEQ";

            DataRow dtrLast = null;
            DataRow dtrTemRoute = null;

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, SysDef.gc_REF_OPSTEP_FINISH });
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

                    this.pmCalPlanDueDate(this.mstrRefToMOrderOP);

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

        private void pmLoadSemiFormOPSeq()
        {

            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from MFWORDERIT_PD WORDERI ";
            strSQLStr += " where WORDERI.CWORDERH = ? ";
            strSQLStr += " and WORDERI.COPSEQ = ? ";
            if (this.txtToOPSeq.Text.Trim() == string.Empty)
            {
            }
            else
            {
                strSQLStr += " and WORDERI.CPRODTYPE in ( '1', '5') ";
            }
            strSQLStr += " order by WORDERI.COPSEQ, WORDERI.CSEQ";

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, this.txtToOPSeq.Text.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", strSQLStr, ref strErrorMsg))
            {
                //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                this.pmClearTemToBlank();
                int intRecNo = this.gridView4.RowCount;
                foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
                {
                    this.pmRepl1RecTemWcTranI2(this.mstrTemFG, intRecNo++, dtrWOrderI);
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

            pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID, this.txtFrOPSeq.Text.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOPSeq3", "WORDERI", strSQLStr, ref strErrorMsg))
            {
                //this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();
                this.pmClearTemToBlank2();
                int intRecNo = this.gridView3.RowCount;
                foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QOPSeq3"].Rows)
                {
                    this.pmRepl1RecTemWcTranI2(this.mstrTemPd, intRecNo++, dtrWOrderI);
                }
            }

        }

        private void pmRepl1RecTemWcTranI2(string inAlias, int inRecNo, DataRow inWcTranI)
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            cDBMSAgent pobjSQLUtil2 = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            //dtrTemPd["cRowID"] = inWcTranI["cRowID"].ToString().TrimEnd();
            dtrTemPd["cWOrderI"] = inWcTranI["cRowID"].ToString().TrimEnd();
            dtrTemPd["nRecNo"] = inRecNo;
            //dtrTemPd["cOPSeq"] = inWcTranI["cOPSeq"].ToString().TrimEnd();
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
            //dtrTemPd["nMOQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
            //dtrTemPd["nGoodQty"] = Convert.ToDecimal(inWcTranI["nGoodQty"]);
            //dtrTemPd["nWasteQty"] = Convert.ToDecimal(inWcTranI["nWasteQty"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWcTranI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWcTranI["nUOMQty"]));
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
            dtrTemPd["cLot"] = inWcTranI["cLot"].ToString().TrimEnd();

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString().TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            //if (Convert.ToDecimal(dtrTemPd["nMOQty"]) != 0)
            //{
            //decimal decLossQty = Convert.ToDecimal(dtrTemPd["nMOQty"]) - Convert.ToDecimal(dtrTemPd["nGoodQty"]) - Convert.ToDecimal(dtrTemPd["nWasteQty"]);

            //			try
            //			{
            //				dtrTemPd["dFinish"] = Convert.ToDateTime(inWcTranI["dFinish"]).Date;
            //			}
            //			catch
            //			{
            //				dtrTemPd["dFinish"] = this.txtDate.Value.Date;
            //			}
            //			
            //			decimal decRMQty = this.pmSumRMQty(Convert.ToDateTime(dtrTemPd["dFinish"]));
            //			decimal decLossQty = decRMQty - Convert.ToDecimal(dtrTemPd["nHoldQty"]) - Convert.ToDecimal(dtrTemPd["nGoodQty"]) - Convert.ToDecimal(dtrTemPd["nWasteQty"]);
            //			//dtrTemPd["nLossQty1"] = decLossQty;
            //			if (decRMQty != 0)
            //			{
            //				//dtrTemPd["nLossQty1"] = (this.mbllIsNoLoss == false ? 0 : decLossQty);
            //				dtrTemPd["nLossQty1"] = (this.mbllIsNoLoss == false ? 0 : (decLossQty < 0 ? 0 : decLossQty));
            //			}
            //			else
            //			{
            //				dtrTemPd["nLossQty1"] = 0;
            //			}
            //			//dtrTemPd["nUnKnowQty"] = (this.mbllIsNoLoss == false ? 0 : decLossQty);
            //			//dtrTemPd["nLossQty1"] = (this.mbllIsNoLoss == false ? 0 : decLossQty - Convert.ToDecimal(dtrTemPd["nUnKnowQty"]));

            //}

            //if (inAlias==this.mstrTemPd)
            //{
            dtrTemPd["dFinish"] = this.txtDate.DateTime.Date;
            //}

            if (inAlias == this.mstrTemPd
                && "1,5".IndexOf(inWcTranI["CPRODTYPE"].ToString()) > -1)
            {
                string strSQLStr2 = "select ";
                //strSQLStr2 += " REFDOC.CCHILDH , WCTRANH.CFROPSEQ, WCTRANI.CPROD, WCTRANI.NQTY, WCTRANI.NGOODQTY ";
                strSQLStr2 += " WCTRANH.CFROPSEQ, sum(WCTRANI.NQTY) as NQTY, sum(WCTRANI.NGOODQTY) as NGOODQTY";
                strSQLStr2 += " from REFDOC  ";
                strSQLStr2 += " left join WCTRANH on WCTRANH.CROWID = REFDOC.CMASTERH ";
                strSQLStr2 += " left join WCTRANI on WCTRANI.CWCTRANH = REFDOC.CMASTERH ";
                strSQLStr2 += " where REFDOC.CCHILDTYPE = 'WO' and REFDOC.CCHILDH = ? ";
                strSQLStr2 += " and WCTRANI.CIOTYPE = 'I' and WCTRANH.CFROPSEQ < ?  ";
                strSQLStr2 += " group by WCTRANH.CFROPSEQ ";
                strSQLStr2 += " order by WCTRANH.CFROPSEQ desc  ";

                pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRowID, this.txtFrOPSeq.Text.TrimEnd() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOPSeq4", "WORDERI", strSQLStr2, ref strErrorMsg))
                {
                    string strOPSeq = "";
                    string strOPSeq2 = "";
                    decimal decMOQty = 0;
                    DataRow dtrSum1 = this.dtsDataEnv.Tables["QOPSeq4"].Rows[0];
                    dtrTemPd["nMOQty"] = decMOQty;
                    if (!Convert.IsDBNull(dtrSum1["nQty"]))
                    {
                        decMOQty = Convert.ToDecimal(dtrSum1["nQty"]);
                        dtrTemPd["nMOQty"] = decMOQty;
                    }
                }
                else
                {
                    dtrTemPd["nMOQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
                }

            }
            else
            {
                dtrTemPd["nMOQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
            }

            decimal decIssueQty = 0;
            decimal decReturnQty = 0;
            //Sum ยอดเบิก
            pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), dtrTemPd["cWOrderI"].ToString(), DocumentType.WR.ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                {
                    decIssueQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                }
            }

            //Sum ยอดรับคืน
            pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), dtrTemPd["cWOrderI"].ToString(), DocumentType.RW.ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                {
                    decReturnQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                }
            }

            dtrTemPd["nQty"] = decIssueQty - decReturnQty;

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
                case "TXTREFTO":
                    this.pmInitPopUpDialog("REFTO");
                    break;
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

                case "GRDVIEW3_CPDTYPE":
                case "GRDVIEW4_CPDTYPE":

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
                case "GRDVIEW3_CQCPROD":
                case "GRDVIEW3_CQNPROD":
                case "GRDVIEW4_CQCPROD":
                case "GRDVIEW4_CQNPROD":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrActiveTem].NewRow();
                        this.dtsDataEnv.Tables[this.mstrActiveTem].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetProd = this.pofrmGetProd.RetrieveValue();
                    if (dtrGetProd != null)
                    {
                        dtrGetVal["cProd"] = dtrGetProd["fcSkid"].ToString();
                        dtrGetVal["cPdType"] = dtrGetProd["fcType"].ToString().TrimEnd();
                        dtrGetVal["cQcProd"] = dtrGetProd["fcCode"].ToString().TrimEnd();
                        dtrGetVal["cQnProd"] = dtrGetProd["fcName"].ToString().TrimEnd();
                        dtrGetVal["cUOM"] = dtrGetProd["fcUM"].ToString().TrimEnd();
                        dtrGetVal["cUOMStd"] = dtrGetProd["fcUM"].ToString().TrimEnd();

                        //if (!Convert.IsDBNull(dtrGetProd["fmPicName"]))
                        //{
                        //    if (dtrGetProd["fmPicName"].ToString().Trim() != "...")
                        //    {
                        //        dtrGetVal["cAttachFile"] = dtrGetProd["fmPicName"].ToString().TrimEnd();
                        //    }
                        //}
                        dtrGetVal["nQty"] = 1;
                        dtrGetVal["nUOMQty"] = 1;

                        if (objSQLHelper.SetPara(new object[] { dtrGetProd["fcUM"].ToString() })
                                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid , fcCode, fcName from UM where fcSkid = ? ", ref strErrorMsg))
                        {
                            dtrGetVal["cQnUOM"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }
                    else
                    {
                        this.pmClr1TemPd();
                    }

                    this.mActiveGrid.UpdateCurrentRow();

                    break;
                case "TXTFROPSEQ":

                    if (this.pofrmGetOPSeq.SelectedRow() > -1)
                    {

                        this.pmRetrieveOPSeqVal(this.pofrmGetOPSeq.SelectedRow());

                        //DataRow dtrSelOP = this.dtsDataEnv.Tables[this.mstrTemRoute].Rows[this.pofrmGetOPSeq.SelectedRow()];

                        //if (this.mstrRefToMOrderOP != dtrSelOP["cRowID"].ToString())
                        //{
                        //    this.mstrRefToMOrderOP = dtrSelOP["cRowID"].ToString();
                        //    this.txtFrQnOP.Tag = dtrSelOP["cFrMOPR"].ToString().TrimEnd();
                        //    this.txtToQnOP.Tag = dtrSelOP["cToMOPR"].ToString().TrimEnd();

                        //    this.txtFrOPSeq.Text = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                        //    this.txtToOPSeq.Text = dtrSelOP["cToOPSeq"].ToString().TrimEnd();
                            
                        //    this.pmLoadSemiFormOPSeq();
                        //    this.pmLoadRMFormOPSeq();
                        //    this.pmLoadSectByWorkCenter(this.txtFrQnOP.Tag.ToString());
                        //}

                        //this.txtFrOPSeq.Text = dtrSelOP["cFrOPSeq"].ToString().TrimEnd();
                        //this.txtFrQnOP.Text = dtrSelOP["cQnFrMOPR"].ToString().TrimEnd();
                        //this.txtToOPSeq.Text = dtrSelOP["cToOPSeq"].ToString().TrimEnd();
                        //this.txtToQnOP.Text = dtrSelOP["cQnToMOPR"].ToString().TrimEnd();

                    }
                    else
                    {
                        this.pmClearRefTo();
                    }

                    //dtrGetVal = this.pofrmGetOPSeq.RetrieveValue();
                    //if (dtrGetVal != null)
                    //{
                    //    this.txtQcJob.Tag = dtrGetVal["fcSkid"].ToString();
                    //    this.txtQcJob.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                    //    //this.txtQnJob.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    //}
                    //else
                    //{
                    //    this.txtQcJob.Tag = "";
                    //    this.txtQcJob.Text = "";
                    //    //this.txtQnJob.Text = "";
                    //} 
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

        private void pmClr1TemPd()
        {
            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrActiveTem].Rows[this.mActiveGrid.FocusedRowHandle];
            dtrTemPd["cWOrderI"] = "";
            dtrTemPd["cProd"] = "";
            dtrTemPd["cLastProd"] = "";
            dtrTemPd["cPdType"] = "";
            dtrTemPd["cQcProd"] = "";
            dtrTemPd["cQnProd"] = "";
            dtrTemPd["cLastQcProd"] = "";
            dtrTemPd["cLastQnProd"] = "";
            dtrTemPd["cUOM"] = "";
            dtrTemPd["cQnUOM"] = "";
            dtrTemPd["nQty"] = 0;
            dtrTemPd["nMOQty"] = 0;
            dtrTemPd["nLastQty"] = 0;
            dtrTemPd["nGoodQty"] = 0;
            dtrTemPd["nWasteQty"] = 0;
            dtrTemPd["nLossQty1"] = 0;
            dtrTemPd["nHoldQty"] = 0;
            dtrTemPd["nBackQty"] = 0;
            dtrTemPd["dFinish"] = DBEnum.NullDate;

            this.mbllRecalTotPd = true;
            this.pmRecalTotPd();
        }

        private void pmClr1TemPd(ref DataRow inTemPd)
        {
            DataRow dtrTemPd = inTemPd;
            dtrTemPd["cWOrderI"] = "";
            dtrTemPd["cProd"] = "";
            dtrTemPd["cLastProd"] = "";
            dtrTemPd["cPdType"] = "";
            dtrTemPd["cQcProd"] = "";
            dtrTemPd["cQnProd"] = "";
            dtrTemPd["cLastQcProd"] = "";
            dtrTemPd["cLastQnProd"] = "";
            dtrTemPd["cUOM"] = "";
            dtrTemPd["cQnUOM"] = "";
            dtrTemPd["nQty"] = 0;
            dtrTemPd["nMOQty"] = 0;
            dtrTemPd["nLastQty"] = 0;
            dtrTemPd["nGoodQty"] = 0;
            dtrTemPd["nWasteQty"] = 0;
            dtrTemPd["nLossQty1"] = 0;
            dtrTemPd["nHoldQty"] = 0;
            dtrTemPd["nBackQty"] = 0;
            dtrTemPd["dFinish"] = DBEnum.NullDate;

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

            decimal decMOQty = 0;
            decimal decSumQty = 0;
            decimal decSumGoodQty = 0;
            decimal decSumWasteQty = 0;
            decimal decSumLossQty = 0;

            if (this.mbllRecalTotPd)
            {
                foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
                {
                    decSumQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                    decSumGoodQty += Convert.ToDecimal(dtrTemPd["nGoodQty"]);
                    decSumWasteQty += Convert.ToDecimal(dtrTemPd["nWasteQty"]);

                    if (decMOQty == 0)
                    {
                        decMOQty = Convert.ToDecimal(dtrTemPd["nMOQty"]);
                    }
                }
                this.mbllRecalTotPd = false;
                //this.txtTotPdQty.Value = decSumQty;
                //this.txtTotGoodQty.Value = decSumGoodQty;
                //this.txtTotWasteQty.Value = decSumWasteQty;
                ////this.txtTotPdAmt.Value = decSumAmt;
            }

            decimal decBFQty1 = 0;
            decimal decBFQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
            {
                decBFQty1 = Convert.ToDecimal(dtrTemPd["nHoldQty"]);
                dtrTemPd["nBFQty"] = decBFQty;
                decBFQty = decBFQty1;
            }

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
            {
                decBFQty = Convert.ToDecimal(dtrTemPd["nBFQty"]);
                //if (decBFQty > 0)

                //if (dtrTemPd["cQcProd"].ToString().Trim() != "" && decBFQty > 0)
                if (dtrTemPd["cQcProd"].ToString().Trim() != "")
                {

                    //กรณีไม่คีย์น้ำหนักดีย์ส่งต่อให้ default ไปที่ ไม่เสร็จ
                    //if (Convert.ToDecimal(dtrTemPd["nGoodQty"]) == 0)
                    //{
                    //	dtrTemPd["nHoldQty"] = decBFQty - Convert.ToDecimal(dtrTemPd["nWasteQty"]);
                    //}

                    decimal decLossQty = decBFQty - Convert.ToDecimal(dtrTemPd["nHoldQty"]) - Convert.ToDecimal(dtrTemPd["nGoodQty"]) - Convert.ToDecimal(dtrTemPd["nWasteQty"]);
                    if (decLossQty < 0)
                    {

                        try
                        {
                            dtrTemPd["dFinish"] = Convert.ToDateTime(dtrTemPd["dFinish"]).Date;
                        }
                        catch
                        {
                            dtrTemPd["dFinish"] = this.txtDate.DateTime.Date;
                        }

                        decimal decRMQty = this.pmSumRMQty(Convert.ToDateTime(dtrTemPd["dFinish"]));
                        decLossQty = decRMQty - Convert.ToDecimal(dtrTemPd["nHoldQty"]) - Convert.ToDecimal(dtrTemPd["nGoodQty"]) - Convert.ToDecimal(dtrTemPd["nWasteQty"]);
                    }

                    //08/07/09 : By Yod แสดงค่าLossเฉพาะเมื่อมีการคีย์ดีส่งต่อเท่านั้น
                    if (Convert.ToDecimal(dtrTemPd["nGoodQty"]) != 0)
                    {
                        //dtrTemPd["nLossQty1"] = (decLossQty < 0 ? 0 : decLossQty);
                        //dtrTemPd["nLossQty1"] = (this.mbllIsNoLoss == false ? 0 : (decLossQty < 0 ? 0 : decLossQty));
                    }
                    //dtrTemPd["nLossQty1"] = (decLossQty < 0 ? 0 : decLossQty);
                }
                decSumLossQty += Convert.ToDecimal(dtrTemPd["nLossQty1"]);
            }

            //Cal รายการวัตถุดิบ
            decBFQty1 = 0;
            decBFQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                decBFQty1 = Convert.ToDecimal(dtrTemPd["nQty"]);
                dtrTemPd["nBFQty"] = decBFQty;
                decBFQty = decBFQty1;
            }

            this.mdecSumMOQty = decMOQty;
            this.mdecSumGoodQty = decSumGoodQty;
            this.mdecSumWasteQty = decSumWasteQty;
            this.mdecSumLossQty = decSumLossQty;

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

        private void txtFrOPSeq_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CFROPSEQ";

            if (txtPopUp.Text == "")
            {
                txtPopUp.Tag = "";
                this.txtFrQnOP.Text = "";
                this.txtToOPSeq.Tag = "";
                this.txtToOPSeq.Text = "";
                this.txtToQnOP.Text = "";
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

        private void txtStartDate_Validating(object sender, CancelEventArgs e)
        {
            this.pmCalPlanDueDate(this.mstrRefToMOrderOP);
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
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

                            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

                            DataRow dtrLoadHead = null;
                            objSQLHelper.SetPara(new object[] { strRowID });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
                            {
                                dtrLoadHead = this.dtsDataEnv.Tables["QLoadPR"].Rows[0];
                            }
                            
                            this.pmInitPopUpDialog("GENPR");

                            //if (!this.pmCheckHasPR(this.mstrEditRowID, dtrLoadHead[QMFWCTranHDInfo.Field.RefNo].ToString(), ref strErrorMsg))
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
                                if (App.PermissionManager.CheckPermission(App.AppUserName, AuthenType.CanPrint, App.AppUserID, TASKNAME))
                                {
                                    this.pmPrintData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการพิมพ์ข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case WsToolBar.Cancel:
                                if (App.PermissionManager.CheckPermission(App.AppUserName, AuthenType.CanEdit, App.AppUserID, TASKNAME))
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

            string strCode = dtrBrow["cCode"].ToString().TrimEnd();
            using (Transaction.Common.dlgPRange dlg = new Transaction.Common.dlgPRange())
            {

                dlg.BeginCode = strCode;
                dlg.EndCode = strCode;

                string[] strADir = null;

                if (System.IO.Directory.Exists(Application.StartupPath + "\\RPT\\FORM_RJ\\"))
                {
                    strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_RJ\\");
                    dlg.LoadRPT(Application.StartupPath + "\\RPT\\FORM_RJ\\");
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

        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";
            string strSQLStrOrderI = "select * from " + this.mstrITable + " where cWCTranH = ? and cIOType = 'O' order by cSeq";

            Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new Report.LocalDataSet.FORM2PRINT();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cCode between ? and ? order by cCode";
            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, inBegCode, inEndCode });

            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                {

                    string strQcBook = "";
                    string strQnBook = "";
                    pobjSQLUtil2.SetPara(new object[1] { dtrOrderH["cMfgBook"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBook", "MFGBOOK", "select * from MfgBook where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcSect = "";
                    string strQnSect = "";
                    string strQcDept = "";
                    string strQnDept = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cSect"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select * from DEPT where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }

                    string strQcMfgProd = "";
                    string strQnMfgProd = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cMfgProd"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal decMfgQty = Convert.ToDecimal(dtrOrderH["nMfgQty"]);

                    string strStartDate = "";
                    string strDueDate = "";
                    if (!Convert.IsDBNull(dtrOrderH[QMFWCTranHDInfo.Field.StartDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWCTranHDInfo.Field.StartDate]).Date;
                        strStartDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                    }

                    if (!Convert.IsDBNull(dtrOrderH[QMFWCTranHDInfo.Field.DueDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWCTranHDInfo.Field.DueDate]).Date;
                        strDueDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                    }

                    string strQcJob = "";
                    string strQnJob = "";
                    string strQcProj = "";
                    string strQnProj = "";

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cJob"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProj", "PROJ", "select * from PROJ where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcName"].ToString().TrimEnd();
                        }
                    }


                    string strQcCoor = ""; string strQnCoor = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cCoor"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        strQcCoor = dtrCoor["fcCode"].ToString().TrimEnd();
                        strQnCoor = dtrCoor["fcSName"].ToString().TrimEnd();
                    }

                    string strQcWHouse = "";
                    string strQnWHouse = "";
                    //pobjSQLUtil.SetPara(new object[1] {dtrOrderH["cFrWHouse"].ToString()});
                    //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv,"QWHouse", "WHOUSE", "select * from WHOUSE where fcSkid = ?", ref strErrorMsg))
                    //{
                    //	strQcWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                    //	strQnWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
                    //}

                    string strRemark = (Convert.IsDBNull(dtrOrderH["cMemData"]) ? "" : dtrOrderH["cMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData2"]) ? "" : dtrOrderH["cMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData3"]) ? "" : dtrOrderH["cMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData4"]) ? "" : dtrOrderH["cMemData4"].ToString().TrimEnd());
                    if (dtrOrderH["cMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrOrderH["cMemData5"]) ? "" : dtrOrderH["cMemData5"].ToString().TrimEnd());

                    //string strRemark1 = BizRule.GetMemData(strRemark, "Rem");
                    //string strRemark2 = BizRule.GetMemData(strRemark, "Rm2");
                    //string strRemark3 = BizRule.GetMemData(strRemark, "Rm3");
                    //string strRemark4 = BizRule.GetMemData(strRemark, "Rm4");
                    //string strRemark5 = BizRule.GetMemData(strRemark, "Rm5");
                    //string strRemark6 = BizRule.GetMemData(strRemark, "Rm6");
                    //string strRemark7 = BizRule.GetMemData(strRemark, "Rm7");
                    //string strRemark8 = BizRule.GetMemData(strRemark, "Rm8");
                    //string strRemark9 = BizRule.GetMemData(strRemark, "Rm9");
                    //string strRemark10 = BizRule.GetMemData(strRemark, "RmA");

                    string strRemark1 = dtrOrderH["cReMark1"].ToString();
                    string strRemark2 = dtrOrderH["cReMark2"].ToString();
                    string strRemark3 = dtrOrderH["cReMark3"].ToString();
                    string strRemark4 = dtrOrderH["cReMark4"].ToString();
                    string strRemark5 = dtrOrderH["cReMark5"].ToString();
                    string strRemark6 = dtrOrderH["cReMark6"].ToString();
                    string strRemark7 = dtrOrderH["cReMark7"].ToString();
                    string strRemark8 = dtrOrderH["cReMark8"].ToString();
                    string strRemark9 = dtrOrderH["cReMark9"].ToString();
                    string strRemark10 = dtrOrderH["cReMark10"].ToString();

                    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                    {
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                            }

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUOM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            string strRefQcBook = "";
                            string strRefQnBook = "";
                            string strRefCode = "";
                            string strRefRefNo = "";
                            string strRefDate = "";
                            //this.pmLoadRefToCode(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PFORM_MO.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;

                            //if (!Convert.IsDBNull(dtrOrderH["dApprove"]))
                            //{
                            //	dtrPrnData["CAPPROVEDATE"] = Convert.ToDateTime(dtrOrderH["dApprove"]).ToString("dd/MM/yy");
                            //}

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["CREMARK1_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                            dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            //dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            //dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            //dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PFORM_MO.Rows.Add(dtrPrnData);

                        }
                    }
                }
                this.pmPreviewReport(dtsPrintPreview, inRPTFileName, false);
                //dlg.WaitClear();
            }
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName, bool inPrnDiscount)
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
        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = dtrBrow[QMFWCTranHDInfo.Field.Code].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFWCTranHDInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFWCTranHDInfo.Field.Code].ToString(), "", ref strErrorMsg))
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

        private bool pmCheckHasUsed(string inRowID, string inCode, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strMsg1 = UIBase.GetAppUIText(new string[] { "ไม่สามารถลบได้เนื่องจาก", "Cannot delete because" });
            string strMsg2 = "";
            bool bllHasUsed = true;

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

            if (bllHasUsed == false)
            {
                objSQLHelper.SetPara(new object[] { this.mstrEditRowID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed2", this.mstrRefTable, "select * from " + this.mstrRefTable + " where CROWID = ? ", ref strErrorMsg))
                {
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables["QHasUsed2"].Rows[0];
                    if (dtrLoadForm[QMFWCTranHDInfo.Field.FrOPSeq].ToString().TrimEnd() != dtrLoadForm[QMFWCTranHDInfo.Field.ToOPSeq].ToString().TrimEnd())
                    {
                        bllHasUsed = this.pmCheckHasNextOP(ref ioErrorMsg);
                    }
                }
            }

            return bllHasUsed;
        }

        private bool pmCheckHasNextOP(ref string ioErrorMsg)
        {
            string strErrorMsg = "";

            bool bllResult = false;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = true;
            objSQLHelper.SetPara(new object[] { this.mstrEditRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                string strToSeq = this.dtsDataEnv.Tables["QHasUsed"].Rows[0][QMFWCTranHDInfo.Field.ToOPSeq].ToString();
                objSQLHelper.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefDoc", this.mstrRefTable, "select * from REFDOC where CMASTERTYP = ? and CMASTERH = ?", ref strErrorMsg))
                {
                    string strWOrderH = this.dtsDataEnv.Tables["QRefDoc"].Rows[0]["cChildH"].ToString();
                    string strSQLStr = "";
                    strSQLStr = "select MFWCTRANHD.CCODE,MFWCTRANHD.CREFNO from MFWCTRANHD ";
                    strSQLStr += " left join REFDOC on REFDOC.CMASTERTYP = 'RJ' and REFDOC.CMASTERH = MFWCTRANHD.CROWID ";
                    strSQLStr += " left join MFWORDERHD WORDERH on WORDERH.CROWID = REFDOC.CCHILDH ";
                    strSQLStr += " where WORDERH.CROWID = ? and MFWCTRANHD.CFROPSEQ = ? ";

                    objSQLHelper.SetPara(new object[] { strWOrderH, strToSeq });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, strSQLStr, ref strErrorMsg))
                    {
                        ioErrorMsg = "Cannot delete because has Tranfer next OP";
                        bllResult = true;
                    }
                }
            }
            return bllResult;
        }

        private bool pmDeleteRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
        {
            bool bllIsCommit = false;
            bool bllResult = false;
            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strErrorMsg = "";
                object[] pAPara = null;

                //Save NoteCut ไว้เพื่อไล่ Update Order Step ในภายหลัง
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", "select cChildH from REFDOC where CMASTERTYP = ? and CMASTERH = ? group by cChildH", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //Delete Note Cut
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from REFDOC where CMASTERTYP = ? and CMASTERH = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //ต้องลบ NoteCut ออกก่อนจึงค่อยวนลูป Update Step จึงจะถูกต้อง
                foreach (DataRow dtrChildH in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                {
                    this.pmUpdateWOrderOPStep(dtrChildH["cChildH"].ToString());
                }

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWCTranH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
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
                if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[intCnt]["cRowID"].ToString().TrimEnd() == inTag)
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

                    string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_MO\\");
                    dlg2.LoadRPT(Application.StartupPath + "\\RPT\\FORM_MO\\");
                    
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
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
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
            //else if (this.txtTotMfgQty.Value == 0)
            //{
            //    ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุจำนวนที่สั่งผลิต", "Mfg. Qty is not define ! " });
            //    this.txtTotMfgQty.Focus();
            //    return false;
            //}
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "เลขที่เอกสารซ้ำ", "Duplicate Document Code !" });
                this.txtCode.Focus();
                return false;
            }
            else
                bllResult = true;

            return bllResult;
        }

        private bool pmRunCode()
        {
            //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch }, this.txtCode.MaxLength);

            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            long intRunCode = 1;
            int inMaxLength = this.txtCode.Properties.MaxLength;

            string strSQLStr = "select cCode from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cCode < ':' order by cCode desc";
            
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = new object[5] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook };
            objSQLHelper.SetPara(pAPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["cCode"].ToString().Trim();
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

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
            if (this.mFormEditMode == UIHelper.AppFormState.Insert
                || (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cRowID from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg)))
            {
                bllIsNewRow = true;
                if (this.mstrEditRowID == string.Empty)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
                }
                dtrSaveInfo[MapTable.ShareField.CreateBy] = App.FMAppUserID;
                dtrSaveInfo[MapTable.ShareField.CreateDate] = objSQLHelper.GetDBServerDateTime();
                this.dtsDataEnv.Tables[this.mstrRefTable].Rows.Add(dtrSaveInfo);
            }

			string strCode = this.txtCode.Text.TrimEnd();
			if (strCode.Length > 3)
				strCode = StringHelper.Left(strCode, strCode.Length-3);

            this.mstrSaveRowID = this.mstrEditRowID;
            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFWCTranHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFWCTranHDInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QMFWCTranHDInfo.Field.PlantID] = this.mstrPlant;

            dtrSaveInfo[QMFWCTranHDInfo.Field.Step] = this.mstrStep;
            dtrSaveInfo[QMFWCTranHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFWCTranHDInfo.Field.Rftype] = this.mstrRfType;
            dtrSaveInfo[QMFWCTranHDInfo.Field.MStep] = (this.chkIsFinish.Checked ? "F" : "");

            dtrSaveInfo[QMFWCTranHDInfo.Field.MfgBookID] = this.mstrBook;
            dtrSaveInfo[QMFWCTranHDInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.RefNo] = this.txtRefNo.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Date] = this.txtDate.DateTime.Date;

            this.mstrPDocCode = this.txtCode.Text.TrimEnd();

            dtrSaveInfo[QMFWCTranHDInfo.Field.StartDate] = this.txtStartDate.DateTime;
            dtrSaveInfo[QMFWCTranHDInfo.Field.DueDate] = this.txtDueDate.DateTime;

            dtrSaveInfo[QMFWCTranHDInfo.Field.FrWkCtrH] = this.mstrFrWkCtrH;
            dtrSaveInfo[QMFWCTranHDInfo.Field.ToWkCtrH] = this.mstrToWkCtrH;
            dtrSaveInfo[QMFWCTranHDInfo.Field.FrOPSeq] = this.txtFrOPSeq.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.ToOPSeq] = this.txtToOPSeq.Text.TrimEnd();

            dtrSaveInfo[QMFWCTranHDInfo.Field.FrMOPR] = this.txtFrQnOP.Tag.ToString();
            dtrSaveInfo[QMFWCTranHDInfo.Field.ToMOPR] = this.txtToQnOP.Tag.ToString();

            dtrSaveInfo[QMFWCTranHDInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFWCTranHDInfo.Field.JobID] = this.txtQcJob.Tag.ToString();

            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark1] = this.txtRemark1.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark2] = this.txtRemark2.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark3] = this.txtRemark3.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark4] = this.txtRemark4.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark5] = this.txtRemark5.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark6] = this.txtRemark6.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark7] = this.txtRemark7.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark8] = this.txtRemark8.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark9] = this.txtRemark9.Text.TrimEnd();
            dtrSaveInfo[QMFWCTranHDInfo.Field.Remark10] = this.txtRemark10.Text.TrimEnd();

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mstrRefToWOrderI = "";
                this.pmSaveWcTranI(ref strErrorMsg, this.mstrTemFG);
                this.pmSaveWcTranI(ref strErrorMsg, this.mstrTemPd);
                this.pmSaveRefTo();

                if (this.chkIsFinish.Checked)
                { 
                    //เปลี่ยน status ของ report as finish เป็นแค่เอกสารเดียวเท่านั้น
                    string strSQLUpdateMStep = "";
                    strSQLUpdateMStep = " update MFWCTRANHD set CMSTEP = ? where MFWCTRANHD.CROWID ";
                    strSQLUpdateMStep += " in (select CMASTERH from REFDOC where CMASTERTYP = 'RJ' and CCHILDTYPE = 'MO' and CCHILDI = ?)";
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateMStep, new object[] { "", this.mstrRefToMOrderOP }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    this.mSaveDBAgent.BatchSQLExec("update MFWCTRANHD set CMSTEP = ? where MFWCTRANHD.CROWID = ?", new object[] { "F", this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

                this.mdbTran.Commit();
                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    if (this.mstrOldCode == this.txtCode.Text && this.mstrOldRefNo == this.txtRefNo.Text)
                    {
                        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);
                    }
                    else 
                    {
                        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldRefNo);
                    }
                }

			}
			catch (Exception ex)
			{
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
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

        private void pmUpdateBomIT_Pd(string inAlias)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrBudCI["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrBudCI["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    this.pmReplRecordTemPd(inAlias, bllIsNewRow, this.mstrTemOP, dtrTemPd, ref dtrBudCI);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrBudCI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
        }

        private bool pmSaveWcTranI(ref string ioErrorMsg, string inAlias)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            int intRun = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {

                intRun++;
                bool bllIsNewRow = false;
                decimal decHoldQty = Convert.ToDecimal(dtrTemPd["nBFQty"]) - Convert.ToDecimal(dtrTemPd["nHoldQty"]);
                decimal decChkQty = Convert.ToDecimal(dtrTemPd["nQty"])
                                                        + Convert.ToDecimal(dtrTemPd["nGoodQty"])
                                                        + Convert.ToDecimal(dtrTemPd["nWasteQty"])
                                                        + decHoldQty;

                bool bllIsSaveRow = false;
                if (inAlias == this.mstrTemPd)
                {
                    bllIsSaveRow = (decChkQty > 0 || intRun == 1);
                }
                else
                {
                    bllIsSaveRow = true;
                }

                //+ Convert.ToDecimal(dtrTemPd["nLossQty1"])

                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                //	&& bllIsSaveRow)
                //&& (Convert.ToDecimal(dtrTemPd["nQty"]) != 0 ? true : MessageBox.Show("สินค้า " + dtrTemPd["cQcProd"].ToString().TrimEnd() + "\nยังไม่ได้ระบุจำนวนต้องการ Save ด้วยหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        || (pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cRowID"].ToString().TrimEnd() })
                        && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefProd", this.mstrITable, "select cRowID from " + this.mstrITable + " where cRowID = ?", ref strErrorMsg)))
                    {
                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                    }
                    else
                    {
                        strRowID = dtrTemPd["cRowID"].ToString().TrimEnd();
                    }
                    dtrTemPd["cRowID"] = strRowID;
                    DataRow dtrWcTranI = null;
                    this.pmReplRecordWcTranI(inAlias, bllIsNewRow, dtrTemPd, ref dtrWcTranI);

                    string strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrWcTranI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    pobjSQLUtil.SetPara(pAPara);
                    bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr, ref strErrorMsg);

                }
                else
                {
                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {
                        pobjSQLUtil.SetPara(new object[] { dtrTemPd["cRowID"].ToString() });
                        bllResult = pobjSQLUtil.SQLExec("delete from " + this.mstrITable + " where cRowID = ?", ref strErrorMsg);
                    }
                }
            }
            return true;
        }

        private bool pmReplRecordWcTranI(string inAlias, bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrWcTranI;
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (bllIsNewRec)
            {
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", ref strErrorMsg);
                dtrWcTranI = this.dtsDataEnv.Tables[this.mstrITable].NewRow();
                dtrWcTranI["cRowID"] = inTemPd["cRowID"].ToString();
            }
            else
            {
                pobjSQLUtil.SetPara(new object[1] { inTemPd["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", ref strErrorMsg);
                dtrWcTranI = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
            }
            
            DataRow dtrWcTranH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

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

            dtrWcTranI["cCorp"] = App.ActiveCorp.RowID;
            dtrWcTranI["cBranch"] = this.mstrBranch;
            dtrWcTranI["cPlant"] = this.mstrPlant;
            dtrWcTranI["cWcTranH"] = dtrWcTranH["cRowID"].ToString();
            dtrWcTranI["cRefType"] = dtrWcTranH["cRefType"].ToString();
            dtrWcTranI["cStat"] = dtrWcTranH["cStat"].ToString();
            dtrWcTranI["dDate"] = Convert.ToDateTime(dtrWcTranH["dDate"]).Date;

            //dtrWcTranI["cFrWkCtrH"] = this.txtQcFrWkCtr.Tag.ToString();
            //dtrWcTranI["cToWkCtrH"] = this.txtQcFrWkCtr.Tag.ToString();

            dtrWcTranI["cFrOPSeq"] = this.txtFrOPSeq.Text.TrimEnd();
            dtrWcTranI["cFrWkCtrH"] = this.mstrFrWkCtrH;
            dtrWcTranI["cFrOPSeq"] = this.txtFrOPSeq.Text.TrimEnd();
            dtrWcTranI["cToOPSeq"] = this.txtToOPSeq.Text.TrimEnd();
            dtrWcTranI["cToWkCtrH"] = this.mstrToWkCtrH;
            dtrWcTranI["cToOPSeq"] = this.txtToOPSeq.Text.TrimEnd();

            dtrWcTranI["cProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrWcTranI["cProd"] = inTemPd["cProd"].ToString();
            dtrWcTranI["cReMark1"] = strRemark1;
            dtrWcTranI["cReMark2"] = strRemark2;
            dtrWcTranI["cReMark3"] = strRemark3;
            dtrWcTranI["cReMark4"] = strRemark4;
            dtrWcTranI["cReMark5"] = strRemark5;
            dtrWcTranI["cReMark6"] = strRemark6;
            dtrWcTranI["cReMark7"] = strRemark7;
            dtrWcTranI["cReMark8"] = strRemark8;
            dtrWcTranI["cReMark9"] = strRemark9;
            dtrWcTranI["cReMark10"] = strRemark10;
            dtrWcTranI["cUOM"] = inTemPd["cUOM"].ToString().TrimEnd();
            dtrWcTranI["cSect"] = this.txtQcSect.Tag.ToString();
            //dtrWcTranI["cDept"] = this.mstrDivision;
            dtrWcTranI["cJob"] = this.txtQcJob.Tag.ToString();
            //dtrWcTranI["cProj"] = this.mstrJob;
            dtrWcTranI["cIoType"] = (inAlias == this.mstrTemPd ? "I" : "O");
            dtrWcTranI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrWcTranI["nUOMQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrWcTranI["cUOMStd"] = inTemPd["cUOMStd"].ToString().TrimEnd();
            dtrWcTranI["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrWcTranI["nGoodQty"] = Convert.ToDecimal(inTemPd["nGoodQty"]);
            dtrWcTranI["nWasteQty"] = Convert.ToDecimal(inTemPd["nWasteQty"]);
            dtrWcTranI["nLossQty1"] = Convert.ToDecimal(inTemPd["nLossQty1"]);
            dtrWcTranI["nHoldQty"] = Convert.ToDecimal(inTemPd["nHoldQty"]);

            dtrWcTranI["cWOrderI"] = inTemPd["cWOrderI"].ToString();
            if (inAlias == this.mstrTemPd)
            {
                if (this.mstrRefToWOrderI.Trim() == "")
                {
                    this.mstrRefToWOrderI = inTemPd["cWOrderI"].ToString();
                }

                if (dtrWcTranI["cWOrderI"].ToString().Trim() == "")
                {
                    dtrWcTranI["cWOrderI"] = this.mstrRefToWOrderI;
                }
            }

            //			if (inAlias == this.mstrTemPd)
            //			{
            try
            {
                dtrWcTranI["dFinish"] = Convert.ToDateTime(inTemPd["dFinish"]).Date;
            }
            catch
            {
                dtrWcTranI["dFinish"] = this.txtDate.DateTime.Date;
            }
            //			}
            //			else
            //			{
            //				dtrWcTranI["dFinish"] = this.txtDate.Value.Date;
            //			}

            ioRefProd = dtrWcTranI;
            return true;
        }

        private bool pmSaveRefTo()
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

            this.pmUpdateWOrderOPStep(this.mstrRefToRowID);
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
            this.pmCreateTemPd(this.mstrTemFG);

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
        }

        private void pmCreateTemPd(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("dFinish", System.Type.GetType("System.DateTime"));
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
            dtbTemPd.Columns.Add("nMfgQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nMOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBFQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nGoodQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nWasteQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLossQty1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nHoldQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUnknowQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nRefQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPortion", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStkQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStkUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRefToQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cWOrderI", System.Type.GetType("System.String"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cOPSeq"].DefaultValue = "";
            dtbTemPd.Columns["nBFQty"].DefaultValue = 0;
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nGoodQty"].DefaultValue = 0;
            dtbTemPd.Columns["nWasteQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLossQty1"].DefaultValue = 0;
            dtbTemPd.Columns["nHoldQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUnknowQty"].DefaultValue = 0;
            dtbTemPd.Columns["nRefQty"].DefaultValue = 1;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nPortion"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nStkQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStkUOMQty"].DefaultValue = 1;
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
            dtbTemPd.Columns["nBackQty"].DefaultValue = 0;
            dtbTemPd.Columns["cDept"].DefaultValue = "";
            dtbTemPd.Columns["cQcDept"].DefaultValue = "";
            dtbTemPd.Columns["cQnDept"].DefaultValue = "";
            dtbTemPd.Columns["cJob"].DefaultValue = "";
            dtbTemPd.Columns["cQcJob"].DefaultValue = "";
            dtbTemPd.Columns["cQnJob"].DefaultValue = "";
            dtbTemPd.Columns["cLastPdType"].DefaultValue = "";
            dtbTemPd.Columns["cLastProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQnProd"].DefaultValue = "";
            dtbTemPd.Columns["cRefToRowID"].DefaultValue = "";
            dtbTemPd.Columns["nRefToQty"].DefaultValue = 0;
            dtbTemPd.Columns["cWOrderI"].DefaultValue = "";
            dtbTemPd.Columns["nMOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nMfgQty"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            
            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }



        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
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
            }

        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.mstrSaveRowID = "";
            this.mstrEditRowID = "";
            this.mstrActiveOP = "";

            this.txtCode.Text = "";
            this.txtRefNo.Text = "";
            this.txtDate.DateTime = DateTime.Now;

            this.mstrFrWkCtrH = "";
            this.mstrToWkCtrH = "";
            this.txtFrOPSeq.Tag = "";
            this.txtFrQnOP.Tag = "";
            this.txtToOPSeq.Tag = "";
            this.txtToQnOP.Tag = "";

            this.txtFrOPSeq.Text = "";
            this.txtFrQnOP.Text = "";
            this.txtToOPSeq.Text = "";
            this.txtToQnOP.Text = "";
            //this.chkIsFinish.Checked = false;
            this.chkIsFinish.Checked = true;

            this.mstrStep = (this.bllWaitApprove == true ? SysDef.gc_REF_STEP_WAIT_APPROVE : SysDef.gc_REF_STEP_CUT_STOCK);

            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQcJob.Text = "";
            this.txtQcJob.Text = "";

            this.txtStartDate.EditValue = DateTime.Now.Date;
            this.txtDueDate.EditValue = DateTime.Now.Date;
            this.txtPlanDueDate.EditValue = null;

            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.mdecRefToAmt = 0;
            this.txtRefTo.Text = "";
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

            this.txtRemark1.Text = "";
            this.txtRemark2.Text = "";
            this.txtRemark3.Text = "";
            this.txtRemark4.Text = "";
            this.txtRemark5.Text = "";
            this.txtRemark6.Text = "";
            this.txtRemark7.Text = "";
            this.txtRemark8.Text = "";
            this.txtRemark9.Text = "";
            this.txtRemark10.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemFG].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemRoute].Rows.Clear();

            this.gridView4.FocusedColumn = this.gridView4.Columns["cPdType"];
            //this.gridView3.FocusedColumn = this.gridView3.Columns["cOPSeq"];

        }

        private void pmLoadFormData()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.mstrEditRowID = dtrBrow["cRowID"].ToString();

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                DataRow dtrRetVal = null;

                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
                {
                
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

                    this.mstrStep = dtrLoadForm[QMFWCTranHDInfo.Field.Step].ToString();

                    this.txtCode.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Code].ToString().TrimEnd();
                    this.txtRefNo.Text = dtrLoadForm[QMFWCTranHDInfo.Field.RefNo].ToString().TrimEnd();
                    this.txtDate.DateTime = Convert.ToDateTime(dtrLoadForm[QMFWCTranHDInfo.Field.Date]).Date;

                    if (!Convert.IsDBNull(dtrLoadForm[QMFWCTranHDInfo.Field.StartDate]))
                    {
                        this.txtStartDate.EditValue = Convert.ToDateTime(dtrLoadForm[QMFWCTranHDInfo.Field.StartDate]);
                    }
                    this.txtDueDate.EditValue = (Convert.IsDBNull(dtrLoadForm[QMFWCTranHDInfo.Field.DueDate]) ? this.txtDate.DateTime : Convert.ToDateTime(dtrLoadForm[QMFWCTranHDInfo.Field.DueDate]));
                    //this.cmbPriotiry.SelectedValue = dtrLoadForm["cPriority"].ToString();

                    //this.mdttReceDate = Convert.ToDateTime(dtrLoadForm["dReceDate"]).Date;

                    this.txtFrOPSeq.Text = dtrLoadForm[QMFWCTranHDInfo.Field.FrOPSeq].ToString().TrimEnd();
                    this.txtToOPSeq.Text = dtrLoadForm[QMFWCTranHDInfo.Field.ToOPSeq].ToString().TrimEnd();
                    this.mstrFrWkCtrH = dtrLoadForm[QMFWCTranHDInfo.Field.FrWkCtrH].ToString();
                    this.mstrToWkCtrH = dtrLoadForm[QMFWCTranHDInfo.Field.ToWkCtrH].ToString();
                    this.mstrActiveOP = this.txtFrOPSeq.Text;

                    this.chkIsFinish.Checked = (dtrLoadForm[QMFWCTranHDInfo.Field.MStep].ToString() == "F" ? true : false);

                    this.txtFrQnOP.Tag = dtrLoadForm[QMFWCTranHDInfo.Field.FrMOPR].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtFrQnOP.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode,cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        this.txtFrQnOP.Text = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.txtToQnOP.Tag = dtrLoadForm[QMFWCTranHDInfo.Field.ToMOPR].ToString();
                    pobjSQLUtil.SetPara(new object[1] { this.txtToQnOP.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode,cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        this.txtToQnOP.Text = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.txtQcSect.Tag = dtrLoadForm[QMFWCTranHDInfo.Field.SectID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcSect.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QMFWCTranHDInfo.Field.JobID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcJob.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from Job where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtRemark1.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark1].ToString().TrimEnd();
                    this.txtRemark2.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark2].ToString().TrimEnd();
                    this.txtRemark3.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark3].ToString().TrimEnd();
                    this.txtRemark4.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark4].ToString().TrimEnd();
                    this.txtRemark5.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark5].ToString().TrimEnd();
                    this.txtRemark6.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark6].ToString().TrimEnd();
                    this.txtRemark7.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark7].ToString().TrimEnd();
                    this.txtRemark8.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark8].ToString().TrimEnd();
                    this.txtRemark9.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark9].ToString().TrimEnd();
                    this.txtRemark10.Text = dtrLoadForm[QMFWCTranHDInfo.Field.Remark10].ToString().TrimEnd();

                    //this.pmLoadBOMIT_Pd(this.mstrTemFG);
                    //this.pmLoadBOMIT_Pd(this.mstrTemPd);
                    this.pmLoadRefTo();

                    this.pmLoadWcTranI(this.mstrTemFG);
                    this.pmLoadWcTranI(this.mstrTemPd);

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadRefTo()
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mstrRefToMOrderOP = "";
            //Load Reference Document
            //pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef2WOrderOP", "WORDEROP", "select * from WORDEROP where cWCTranH = ?", ref strErrorMsg))
            //{
            //    DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QRef2WOrderOP"].Rows[0];
            //    this.mstrRefToMOrderOP = dtrWOrderOP["cRowID"].ToString();
            //}

            //Load Reference Document
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QRefDoc"].Rows[0];
                pobjSQLUtil.SetPara(new object[1] { dtrRefDoc["cChildH"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
                {
                    DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                    this.mstrRefToBook = dtrRefTo["cMfgBook"].ToString().TrimEnd();
                    this.mstrRefToRowID = dtrRefTo["cRowID"].ToString().TrimEnd();
                    this.mstrRefToMOrderOP = dtrRefDoc["cChildI"].ToString();
                    this.mstrOldRefToRowID = this.mstrRefToRowID;
                    this.mstrOldRefToMOrderOP = this.mstrRefToMOrderOP;
                    this.txtRefTo.Text = dtrRefTo["cRefNo"].ToString().TrimEnd();

                    this.pmCalPlanDueDate(this.mstrRefToMOrderOP);
                    //this.pmLoadOPSeq();
                    this.pmLoadFormOPSeq();
                    //this.dataGrid1.DataSource=this.dtsDataEnv;
                }
            }
        }

        private void pmCalPlanDueDate(string inWOrderOP)
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { inWOrderOP });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefToOP", this.mstrRefToTab2, "select * from " + this.mstrRefToTab2 + " where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrRefDoc2 = this.dtsDataEnv.Tables["QRefToOP"].Rows[0];
                decimal decOPTime = Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_Queue])
                        + Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_SetUp])
                        + (Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_Process]))
                        + Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_Tear])
                        + Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_Wait])
                        + Convert.ToDecimal(dtrRefDoc2[QMFBOMInfo.Field.LeadTime_Move]);

                decimal intHour = 0; decimal intMin = 0; decimal intSec = 0;
                AppUtil.CommonHelper.ConvertSecToHMS(decOPTime, out intHour, out intMin, out intSec);
                this.txtOPTime.Text = this.pmConvertTime2Text(intHour, intMin, intSec);

                this.txtPlanDueDate.EditValue = this.txtStartDate.DateTime.AddSeconds(Convert.ToDouble(decOPTime));
            }
        }

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        private void pmLoadBOMIT_Pd(string inAlias)
        {
            this.dtsDataEnv.Tables[inAlias].Rows.Clear();

            int intRecNo = 0;
            string strErrorMsg = "";
            string strIOType = (inAlias == this.mstrTemFG ? "I" : "O");
            string strSQLStr = "select * from " + this.mstrITable + " where cWCTranH = ? and cIOType = ? order by cSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, strIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    intRecNo++;
                    this.pmRepl1RecTemPd(inAlias, intRecNo, dtrPdStI);
                }
            }
        }

        private void pmRepl1RecTemPd(string inAlias, int inRecNo, DataRow inWOrderI)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            dtrTemPd["cRowID"] = inWOrderI["cRowID"].ToString().TrimEnd();
            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inWOrderI["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inWOrderI["cProd"].ToString();
            dtrTemPd["cLastProd"] = inWOrderI["cProd"].ToString();
            dtrTemPd["cPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cLastPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cRemark1"] = (Convert.IsDBNull(inWOrderI["cReMark1"]) ? "" : inWOrderI["cReMark1"].ToString().TrimEnd());
            dtrTemPd["cRemark2"] = (Convert.IsDBNull(inWOrderI["cReMark2"]) ? "" : inWOrderI["cReMark2"].ToString().TrimEnd());
            dtrTemPd["cRemark3"] = (Convert.IsDBNull(inWOrderI["cReMark3"]) ? "" : inWOrderI["cReMark3"].ToString().TrimEnd());
            dtrTemPd["cRemark4"] = (Convert.IsDBNull(inWOrderI["cReMark4"]) ? "" : inWOrderI["cReMark4"].ToString().TrimEnd());
            dtrTemPd["cRemark5"] = (Convert.IsDBNull(inWOrderI["cReMark5"]) ? "" : inWOrderI["cReMark5"].ToString().TrimEnd());
            dtrTemPd["cRemark6"] = (Convert.IsDBNull(inWOrderI["cReMark6"]) ? "" : inWOrderI["cReMark6"].ToString().TrimEnd());
            dtrTemPd["cRemark7"] = (Convert.IsDBNull(inWOrderI["cReMark7"]) ? "" : inWOrderI["cReMark7"].ToString().TrimEnd());
            dtrTemPd["cRemark8"] = (Convert.IsDBNull(inWOrderI["cReMark8"]) ? "" : inWOrderI["cReMark8"].ToString().TrimEnd());
            dtrTemPd["cRemark9"] = (Convert.IsDBNull(inWOrderI["cReMark9"]) ? "" : inWOrderI["cReMark9"].ToString().TrimEnd());
            dtrTemPd["cRemark10"] = (Convert.IsDBNull(inWOrderI["cReMark10"]) ? "" : inWOrderI["cReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inWOrderI["cUOM"].ToString().TrimEnd();
            dtrTemPd["nQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWOrderI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWOrderI["nUOMQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["cLot"] = inWOrderI["cLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inWOrderI["cWHouse"].ToString().TrimEnd();
            dtrTemPd["cPdStI"] = inWOrderI["cBOMIT_PD"].ToString();
            dtrTemPd["cScrap"] = inWOrderI["cScrap"].ToString().TrimEnd();

            dtrTemPd["cProcure"] = inWOrderI["cProcure"].ToString().TrimEnd();
            dtrTemPd["cStep1"] = inWOrderI["cStep1"].ToString();
            dtrTemPd["cStep2"] = inWOrderI["cStep1"].ToString();
            dtrTemPd["nBackQty1"] = Convert.ToDecimal(inWOrderI["nBackQty1"]);
            dtrTemPd["nBackQty2"] = Convert.ToDecimal(inWOrderI["nBackQty2"]);

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString().TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWHouse"].ToString().TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select FCCODE,FCNAME from WHouse where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cPdStI"].ToString().TrimEnd() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStI", "PDSTI", "select nQty from MFBOMIT_PD where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["nRefQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QPdStI"].Rows[0]["nQty"]);
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private void pmLoadWcTranI(string inAlias)
        {
            int intRecNo = 0;
            string strErrorMsg = "";

            string strSQLStrRefProd = "select * from " + this.mstrITable + " where cWcTranH = ? and cIOType = ? order by cSeq ";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strIOType = (inAlias == this.mstrTemPd ? "I" : "O");
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, strIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, strSQLStrRefProd, ref strErrorMsg))
            {
                foreach (DataRow dtrWcTranI in this.dtsDataEnv.Tables[this.mstrITable].Rows)
                {
                    intRecNo++;
                    this.pmRepl1RecTemWcTranI(inAlias, intRecNo, dtrWcTranI);
                }
                this.mbllRecalTotPd = true;
                this.pmRecalTotPd();

            }
        }

        private void pmRepl1RecTemWcTranI(string inAlias, int inRecNo, DataRow inWcTranI)
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            cDBMSAgent pobjSQLUtil2 = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            dtrTemPd["cRowID"] = inWcTranI["cRowID"].ToString().TrimEnd();
            dtrTemPd["nRecNo"] = inRecNo;
            //dtrTemPd["cOPSeq"] = inWcTranI["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inWcTranI["cProd"].ToString();
            dtrTemPd["cLastProd"] = inWcTranI["cProd"].ToString();
            dtrTemPd["cPdType"] = inWcTranI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cLastPdType"] = inWcTranI["cProdType"].ToString().TrimEnd();
            //dtrTemPd["cRemark1"] = (Convert.IsDBNull(inWcTranI["cReMark1"]) ? "" : inWcTranI["cReMark1"].ToString().TrimEnd());
            //dtrTemPd["cRemark2"] = (Convert.IsDBNull(inWcTranI["cReMark2"]) ? "" : inWcTranI["cReMark2"].ToString().TrimEnd());
            //dtrTemPd["cRemark3"] = (Convert.IsDBNull(inWcTranI["cReMark3"]) ? "" : inWcTranI["cReMark3"].ToString().TrimEnd());
            //dtrTemPd["cRemark4"] = (Convert.IsDBNull(inWcTranI["cReMark4"]) ? "" : inWcTranI["cReMark4"].ToString().TrimEnd());
            //dtrTemPd["cRemark5"] = (Convert.IsDBNull(inWcTranI["cReMark5"]) ? "" : inWcTranI["cReMark5"].ToString().TrimEnd());
            //dtrTemPd["cRemark6"] = (Convert.IsDBNull(inWcTranI["cReMark6"]) ? "" : inWcTranI["cReMark6"].ToString().TrimEnd());
            //dtrTemPd["cRemark7"] = (Convert.IsDBNull(inWcTranI["cReMark7"]) ? "" : inWcTranI["cReMark7"].ToString().TrimEnd());
            //dtrTemPd["cRemark8"] = (Convert.IsDBNull(inWcTranI["cReMark8"]) ? "" : inWcTranI["cReMark8"].ToString().TrimEnd());
            //dtrTemPd["cRemark9"] = (Convert.IsDBNull(inWcTranI["cReMark9"]) ? "" : inWcTranI["cReMark9"].ToString().TrimEnd());
            //dtrTemPd["cRemark10"] = (Convert.IsDBNull(inWcTranI["cReMark10"]) ? "" : inWcTranI["cReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inWcTranI["cUOM"].ToString().TrimEnd();
            dtrTemPd["nQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
            dtrTemPd["nGoodQty"] = Convert.ToDecimal(inWcTranI["nGoodQty"]);
            dtrTemPd["nWasteQty"] = Convert.ToDecimal(inWcTranI["nWasteQty"]);
            dtrTemPd["nLossQty1"] = Convert.ToDecimal(inWcTranI["nLossQty1"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWcTranI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWcTranI["nUOMQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inWcTranI["nQty"]);
            dtrTemPd["cLot"] = inWcTranI["cLot"].ToString().TrimEnd();
            dtrTemPd["cWOrderI"] = inWcTranI["cWOrderI"].ToString();
            dtrTemPd["nHoldQty"] = Convert.ToDecimal(inWcTranI["nHoldQty"]);

            dtrTemPd["dFinish"] = DBEnum.NullDate;
            //if (inAlias == this.mstrTemPd)
            //{
            try
            {
                dtrTemPd["dFinish"] = Convert.ToDateTime(inWcTranI["dFinish"]).Date;
            }
            catch
            {
                dtrTemPd["dFinish"] = this.txtDate.DateTime.Date;
            }
            //}

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString().TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUOM", "UOM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUOM"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            //			pobjSQLUtil2.SetPara(new object[1] {dtrTemPd["cWOrderI"].ToString().TrimEnd()});
            //			if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv,"QWOrderI", "WORDERI", "select * from WORDERI where cRowID = ?", ref strErrorMsg))
            //			{
            //				dtrTemPd["nMOQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["nQty"]);
            //			}

            if (inAlias == this.mstrTemPd
                && "XXXXX".IndexOf(inWcTranI["CPRODTYPE"].ToString()) > -1)
            //&& "1,5".IndexOf(inWcTranI["CPRODTYPE"].ToString()) > -1)
            {
                string strSQLStr2 = "select ";
                //strSQLStr2 += " REFDOC.CCHILDH , WCTRANH.CFROPSEQ, WCTRANI.CPROD, WCTRANI.NQTY, WCTRANI.NGOODQTY ";
                strSQLStr2 += " WCTRANH.CFROPSEQ, sum(WCTRANI.NQTY) as NQTY, sum(WCTRANI.NGOODQTY) as NGOODQTY";
                strSQLStr2 += " from REFDOC  ";
                strSQLStr2 += " left join MFWCTRANHD WCTRANH on WCTRANH.CROWID = REFDOC.CMASTERH ";
                strSQLStr2 += " left join MFWCTRANIT WCTRANI on WCTRANI.CWCTRANH = REFDOC.CMASTERH ";
                strSQLStr2 += " where REFDOC.CCHILDTYPE = 'WO' and REFDOC.CCHILDH = ? ";
                strSQLStr2 += " and WCTRANI.CIOTYPE = 'I' and WCTRANH.CFROPSEQ < ?  ";
                strSQLStr2 += " group by WCTRANH.CFROPSEQ ";
                strSQLStr2 += " order by WCTRANH.CFROPSEQ desc  ";

                pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRowID, this.txtFrOPSeq.Text.TrimEnd() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOPSeq4", "WORDERI", strSQLStr2, ref strErrorMsg))
                {
                    string strOPSeq = "";
                    string strOPSeq2 = "";
                    decimal decMOQty = 0;
                    DataRow dtrSum1 = this.dtsDataEnv.Tables["QOPSeq4"].Rows[0];
                    dtrTemPd["nMOQty"] = decMOQty;
                    if (!Convert.IsDBNull(dtrSum1["nQty"]))
                    {
                        decMOQty = Convert.ToDecimal(dtrSum1["nQty"]);
                        dtrTemPd["nMOQty"] = decMOQty;
                    }

                }
                else
                {
                    pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cWOrderI"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", "WORDERI", "select * from MFWORDERIT_PD where cRowID = ?", ref strErrorMsg))
                    {
                        dtrTemPd["nMOQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["nQty"]);
                    }
                }
            }
            else
            {
                pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cWOrderI"].ToString() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", "WORDERI", "select * from MFWORDERIT_PD where cRowID = ?", ref strErrorMsg))
                {
                    dtrTemPd["nMOQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["nQty"]);
                }
            }

            if (inAlias == this.mstrTemPd)
            {
                //dtrTemPd["nMOQty"] = this.pmSumMOQty();
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private decimal pmSumMOQty()
        {

            decimal decOutputQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                //if ("1,5".IndexOf(dtrTemPd["cPdType"].ToString()) > -1)
                if (true)
                {
                    //inOPSeq = 0;
                    decOutputQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                }

            }

            return decOutputQty;
        }

        private decimal pmSumRMQty(DateTime inDFinish)
        {

            decimal decOutputQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                //if ("1,5".IndexOf(dtrTemPd["cPdType"].ToString()) > -1)
                if (dtrTemPd["cQcProd"].ToString().Trim() != string.Empty)
                {
                    //if (true)
                    DateTime dttFinish = Convert.ToDateTime(dtrTemPd["dFinish"]);
                    if (inDFinish.CompareTo(dttFinish) > -1)
                    {
                        //inOPSeq = 0;
                        decOutputQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                    }
                }

            }

            return decOutputQty;
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

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == QMFWCTranHDInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFWCTranHDInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtRefNo.Properties.MaxLength);
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
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["cRowID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
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
            string strOrderBy = "";
            switch (inKeyField.ToUpper())
            {
                case "GRDVIEW3_CQCPROD":
                case "GRDVIEW3_CQNPROD":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW3_CQCPROD" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("PROD");

                    DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

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
                case "GRDVIEW3_CREMARK1":
                    this.pmInitPopUpDialog("REMARK");
                    break;

                case "GRDVIEW3_VIEWFILE":
                    this.pmInitPopUpDialog("VIEWATTACH2");
                    break;

            }
        }

        private void gridView3_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strValue = "";
            string strCol = gridView3.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

            string strRefPdType = this.mstrPdType;

            if (dtrTemPd["cPdType"].ToString().Trim() != string.Empty)
            {
                strRefPdType = this.pmSplitToSQLStr(dtrTemPd["cPdType"].ToString());
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
                        e.Valid = !this.pofrmGetProdType.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetProdType.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView3.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW3_" + strCol);
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
                            this.gridView3.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW3_" + strCol);
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
            }

        }

        private void gridView4_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strValue = "";
            string strCol = gridView4.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

            string strRefPdType = this.mstrPdType;

            if (dtrTemPd["cPdType"].ToString().Trim() != string.Empty)
            {
                strRefPdType = this.pmSplitToSQLStr(dtrTemPd["cPdType"].ToString());
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
                        e.Valid = !this.pofrmGetProdType.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetProdType.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView4.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW4_" + strCol);
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
                            this.gridView4.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW4_" + strCol);
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
            }

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
                    string strPrefix = (this.mActiveGrid.Name.ToUpper() == "GRIDVIEW2" ? "GRDVIEW2_" : "GRDVIEW3_");
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

            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefToOP", "MFWCTRANIT", strSQLStr, new object[] { inWOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran))
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
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdate_OrderI, new object[] { strOPStep, dtrWcTranI["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    if (strOPStep == SysDef.gc_REF_OPSTEP_CREATE) intStep_Create++;
                    else if (strOPStep == SysDef.gc_REF_OPSTEP_START) intStep_Start++;
                    else if (strOPStep == SysDef.gc_REF_OPSTEP_FINISH) intStep_Finish++;
                }
                if (intStep_Create == intRowCount) strHOPStep = SysDef.gc_REF_OPSTEP_CREATE;
                else if (intStep_Finish == intRowCount) strHOPStep = SysDef.gc_REF_OPSTEP_FINISH;
                else strHOPStep = SysDef.gc_REF_OPSTEP_START;

                //Update RefTo OrderH step
                this.mSaveDBAgent.BatchSQLExec("update MFWORDERHD set cOPStep = ? where cRowID = ?", new object[] { strHOPStep, inWOrderH }, ref strErrorMsg, this.mdbConn, this.mdbTran);

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


    }
}
