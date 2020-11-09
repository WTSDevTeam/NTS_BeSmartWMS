﻿
//#define xd_RUNMODE_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

namespace BeSmartMRP.Transaction
{

    //TODO: ยังไม่ได้คำนวณ Process time ตามจำนวนที่สั่งผลิต

    /// <summary>
    /// Create By : Thanapon Rauntongjarat
    /// Create Date : 01/12/2009
    /// Update Log Format :
    ///        Change Date | By : XXX | JOBNO_YYMMXXX | Type | Description;
    /// Update Log :
    /// Update Log End :
    /// </summary>
    public partial class frmMFWOrder : UIHelper.frmBase
    {

        public static string TASKNAME = "EMORDER";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;
        private const int xd_PAGE_EDIT2 = 2;
        private const int xd_PAGE_EDIT3 = 3;
        private const int xd_PAGE_EDIT4 = 4;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrHTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;
        private string mstrITable3 = MapTable.Table.MFWOrderIT_OPPlan;

        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrCanEditMsg = "";

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QMFWOrderHDInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";

        private string mstrBook_WHouse_FG = "";
        private string mstrBook_WHouse_RM = "";

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrActiveAlias = "TemPd";
        private string mstrTemFG = "TemFG";
        private string mstrTemPd = "TemPd";
        private string mstrTemOP = "TemOP";
        private string mstrTemPdX1 = "TemPdX1";
        private string mstrTemRefTo = "TemRefTo";

        //For Cal Planning
        private string mstrTemMOrderOP = "TemMOrderOP";
        private string mstrTemWorkCal = "TemWorkCal";
        private string mstrTemUsage = "TemUsage";

        private string mstrTemPd_GenPR = "TemPd_GenPR";
        private string mstrGenPR_Branch = "";
        private string mstrGenPR_Book = "";
        private string mstrGenPR_Coor = "";
        private string mstrGenPR_Code = "";
        private string mstrGenPR_RefNo = "";
        private DateTime mdttGenPR_Date = DateTime.Now;
        private int mintGenPR_CredTerm = 0;

        private string mstrGenRngPR_OrderBy = "CCODE";
        private string mstrGenRngPR_BegCode = "";
        private string mstrGenRngPR_EndCode = "";
        private DateTime mdttGenRngPR_BegDate = DateTime.Now;
        private DateTime mdttGenRngPR_EndDate = DateTime.Now;

        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private StockAgent mStockAgent = new StockAgent(App.ERPConnectionString, App.ConnectionString, App.DatabaseReside);

        private string mstrTemGenFG = "TemGenFG";
        private string mstrTemGenPd = "TemGenPd";
        private string mstrTemGenOP = "TemGenOP";
        private string mstrTemGenMulti = "TemGenMulti";
        private string mstrTemGenMulti2 = "TemGenMulti2";

        private string mstrEditRowID = "";
        private string mstrSaveRowID = "";
        private string mstrEditRowID_PR = "";
        private string mstrRefType = DocumentType.MO.ToString();
        private string mstrRefToRefType = DocumentType.SO.ToString();
        private string mstrRfType = "M";
        private DocumentType mRefType = DocumentType.MO;
        private string mstrPDocCode = "";
        private string mstrPlant = "";
        private string mstrBranch = "";
        private string mstrBook = "";
        private string mstrQcBook = "";
        private string mstrPdType = "";
        private string mstrStdUM = "";
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private string mstrDefaFGWHouse = "";
        private string mstrDefaWRWHouse = "";
        private string mstrDefaRWWHouse = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrDefaSect = "";
        private string mstrDefaJob = "";

        private bool bllWaitApprove = false;
        private string mstrStep = "";

        private string mstrChildWORowID = "";
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

        private decimal mdecUMQty = 0;
        bool mbllIsReviseBOMOnly = false;

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

        public frmMFWOrder()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFWOrder(FormActiveMode inMode)
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
        
        private static frmMFWOrder mInstanse_1 = null;

        public static frmMFWOrder GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFWOrder();
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

        private void frmMFWOrder_Load(object sender, EventArgs e)
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
            this.pmInitGridProp_TemOP();

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[mstrTemWorkCal];
            this.dataGridView2.DataSource = this.dtsDataEnv.Tables[mstrTemMOrderOP];
            this.dataGridView3.DataSource = this.dtsDataEnv.Tables[this.mstrTemUsage];

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

            this.cmbPriority.Properties.Items.Clear();
            this.cmbPriority.Properties.Items.AddRange(new object[] { "0=LOW", "1=NORMAL", "2=HIGH", "3=VERY HIGH" });
            this.cmbPriority.SelectedIndex = 1;

            this.cmdRoundCtrl.Properties.Items.Clear();
            this.cmdRoundCtrl.Properties.Items.AddRange(new object[] { 
                                                                                " "
                                                                                , "1 = " + UIBase.GetAppUIText(new string[] { "ปัดขึ้นเสมอ", "Round up" })
                                                                                , "2 = " + UIBase.GetAppUIText(new string[] { "ปัดลงเสมอ", "Round down" }) });
            this.cmdRoundCtrl.SelectedIndex = 0;

            this.mStockAgent.CorpID = App.ActiveCorp.RowID;

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

            this.mstrFormMenuName = UIBase.GetAppUIText(new string[] { "เอกสารใบสั่งผลิต [MO]", "MANUFACTURING ORDER [MO]" });
            this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            this.lblCode.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Doc. Code" }) });
            this.lblRefNo.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." }) });
            this.lblDate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Doc. Date" }) });

            this.lblApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "อนุมัติโดย", "Approve By" }) });
            this.lblDApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่", "Date" }) });
            this.lblLastUpdate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "แก้ไขล่าสุด", "Last Update " }) });
            this.lblLastUpdBy.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "โดย", "By" }) });

            this.lblMfgProd.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "สินค้าที่สั่งผลิต", "Product Output" }) });
            this.lblMfgUM.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "หน่วยนับ", "Unit" }) });
            this.lblMfgQty.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "จำนวนที่สั่งผลิต", "MO. Qty." }) });
            this.lblTotMfgQty.Text = UIBase.GetAppUIText(new string[] { "รวมจำนวนที่สั่งผลิต :", "Mfg. Qty. :" });
            this.lblQcCoor.Text = UIBase.GetAppUIText(new string[] { "รหัสลูกค้า :", "Customer Code :" });
            this.lblQnCoor.Text = UIBase.GetAppUIText(new string[] { "ชื่อลูกค้า :", "Name :" });
            this.lblRefer.Text = UIBase.GetAppUIText(new string[] { "#อ้างอิง S/O :", "#Refer S/O :" });
            this.lblStartDate.Text = UIBase.GetAppUIText(new string[] { "วันที่เริ่มต้น :", "Start Date :" });
            this.lblDueDate.Text = UIBase.GetAppUIText(new string[] { "วันที่สิ้นสุด :", "Due Date :" });

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });
            this.lblBalFG.Text = UIBase.GetAppUIText(new string[] { "คงคลัง :", "Stock :" });
            this.lblSOQty.Text = UIBase.GetAppUIText(new string[] { "จำนวน SO :", "SO Qty :" });

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWOrderHDInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWOrderHDInfo.Field.RefNo);

            this.txtRemark1.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark1);
            this.txtRemark2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark2);
            this.txtRemark3.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark3);
            this.txtRemark4.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark4);
            this.txtRemark5.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark5);
            this.txtRemark6.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark6);
            this.txtRemark7.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark7);
            this.txtRemark8.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark8);
            this.txtRemark9.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark9);
            this.txtRemark10.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWOrderHDInfo.TableName, QMFWOrderHDInfo.Field.Remark10);

        }

        private void pmMapEvent()
        {
            this.Resize += new EventHandler(frmMFWOrder_Resize);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.grdBrowView.Resize += new EventHandler(grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new ColumnEventHandler(gridView1_ColumnWidthChanged);

            this.grdTemOP.Resize += new EventHandler(grdTemOP_Resize);
            this.gridView2.ColumnWidthChanged += new ColumnEventHandler(gridView2_ColumnWidthChanged);
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            this.gridView2.GotFocus += new EventHandler(gridView2_GotFocus);

            this.grdTemPd.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView3.ColumnWidthChanged += new ColumnEventHandler(gridView3_ColumnWidthChanged);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);
            this.gridView3.GotFocus += new EventHandler(gridView3_GotFocus);
            this.gridView3.CellValueChanged += new CellValueChangedEventHandler(gridView3_CellValueChanged);


            this.grdTemFG.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView4.ColumnWidthChanged += new ColumnEventHandler(gridView4_ColumnWidthChanged);
            this.gridView4.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);
            this.gridView4.GotFocus += new EventHandler(gridView4_GotFocus);

            this.grcQcWkCtr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcWkCtr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcMOPR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcMOPR.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcOPTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcOPTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcBOM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcBOM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcRemark2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcRemark2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcRemark.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcRef2Doc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcRef2Doc_ButtonClick);

            this.txtQcCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcCoor.Validating += new CancelEventHandler(txtCoor_Validating);

            this.txtQnCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQnCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQnCoor.Validating += new CancelEventHandler(txtCoor_Validating);

            this.txtQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);

            this.txtQcBOM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBOM.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBOM.Validating += new CancelEventHandler(txtQcBOM_Validating);

            this.txtQnMfgUM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQnMfgUM.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQnMfgUM.Validating += new CancelEventHandler(txtQnMfgUM_Validating);

            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);

            this.txtRefTo.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtRefTo.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);

        }

        private void frmMFWOrder_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
            this.pmCalcColWidth1();
            this.pmCalcColWidth2();
            this.pmCalcColWidth3();
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strCoorTab = strFMDBName + ".dbo.COOR";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";

            string strStep = "CSTEP = case WORDERH.CMSTEP when '1' then '' when 'P' then 'MC' when 'L' then 'CLOSED' end";
            string strOPStep = "COPSTEP = case WORDERH.COPSTEP when '' then '' when '5' then 'START' when '9' then 'COMPLETE' end";
            string strPRStep = "CPRSTEP = case WORDERH.CPRSTEP when ' ' then '' when 'P' then 'PR' when 'L' then 'X' end";
            string strPlanStep = "CISPLAN = case WORDERH.CISPLAN when ' ' then '' when 'Y' then 'PLAN' end";

            string strIsApprove = "CISAPPROVE = case ";
            strIsApprove += " when WORDERH.CISAPPROVE = 'A' then 'APPROVE' ";
            strIsApprove += " when WORDERH.CISAPPROVE = ' ' then '' ";
            strIsApprove += " end ";

            string strSQLExec = "";

            strSQLExec = "select WORDERH.CROWID, WORDERH.CSTAT, " + strIsApprove + "," + strStep + "," + strOPStep + "," + strPRStep + "," + strPlanStep + ", WORDERH.CCODE, WORDERH.CREFNO, WORDERH.DDATE ";
            //NMFGQTY
            strSQLExec += " , WORDERH.NMFGQTY ";
            strSQLExec += " , PROD.FCCODE AS QCPROD , PROD.FCNAME AS QNPROD ";
            strSQLExec += " , SECT.FCCODE AS QCSECT , JOB.FCCODE AS QCJOB ";
            strSQLExec += " , WORDERH.DCREATE, WORDERH.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} WORDERH ";
            strSQLExec += " left join " + strProdTab + " PROD on PROD.FCSKID = WORDERH.CMFGPROD ";
            strSQLExec += " left join " + strSectTab + " SECT on SECT.FCSKID = WORDERH.CSECT ";
            strSQLExec += " left join " + strJobTab + " JOB on JOB.FCSKID = WORDERH.CJOB ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = WORDERH.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = WORDERH.CLASTUPDBY ";
            strSQLExec += " where WORDERH.CCORP = ? and WORDERH.CBRANCH = ? and WORDERH.CPLANT = ? and WORDERH.CREFTYPE = ? and WORDERH.CMFGBOOK = ? and WORDERH.DDATE between ? and ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "WORDERH", strSQLExec, ref strErrorMsg);

            DataColumn dtcRef2Doc = new DataColumn("CREF2DOC", Type.GetType("System.String"));
            dtcRef2Doc.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRef2Doc);

            DataColumn dtcRef2PR = new DataColumn("CREF2PR", Type.GetType("System.String"));
            dtcRef2PR.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRef2PR);

            DataColumn dtcRef2WR = new DataColumn("CREF2WR", Type.GetType("System.String"));
            dtcRef2WR.DefaultValue = "";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcRef2WR);

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                dtrBrow["CREF2PR"] = this.pmGetRefPR(dtrBrow["CROWID"].ToString());
            }

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                dtrBrow["CREF2WR"] = this.pmGetRefWR(dtrBrow["CROWID"].ToString());
            }
        
        }

        private string pmGetRefPR(string inWOrderH)
        {

            string strRef2PR = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strOrderHTab = strFMDBName + ".dbo.ORDERH";
            string strBookTab = strFMDBName + ".dbo.BOOK";

            string strSQLExec = "";

            strSQLExec = "select REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH  ";
            //strSQLExec += " ,BOOK.FCCODE as QCBOOK, ORDERH.FCCODE as QCPRCODE ";
            strSQLExec += " ,(select ORDERH.FCCODE from "+strOrderHTab+" ORDERH where ORDERH.FCSKID = REFDOC.CMASTERH) as QCPRCODE ";
            strSQLExec += " ,(select ORDERH.FCBOOK from " + strOrderHTab + " ORDERH where ORDERH.FCSKID = REFDOC.CMASTERH) as QCBOOK ";

            strSQLExec += " from REFDOC  ";
            strSQLExec += " left join " + strOrderHTab + " ORDERH on ORDERH.FCSKID = REFDOC.CMASTERH ";
            //strSQLExec += " left join " + strBookTab + " BOOK on BOOK.FCSKID = ORDERH.FCBOOK ";
            strSQLExec += " where REFDOC.CMASTERTYP = 'PR' and REFDOC.CCHILDTYPE = 'MO' and REFDOC.CCHILDH = ? ";
            strSQLExec += " and ORDERH.FCSTAT <> 'C' ";
            strSQLExec += " group by REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH ";

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { inWOrderH });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef2PR", "WORDERH", strSQLExec, ref strErrorMsg);

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables["QRef2PR"].Rows)
            {
                if (!Convert.IsDBNull(dtrBrow["QCPRCODE"]))
                {
                    pobjSQLUtil2.SetPara(new object[] { dtrBrow["QCBOOK"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select * from BOOK where FCSKID = ?", ref strErrorMsg))
                    {
                        DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                        strRef2PR += "PR" + dtrBook["FCCODE"].ToString().TrimEnd() + "/" + dtrBrow["QCPRCODE"].ToString().TrimEnd() + ", ";
                    }

                }
            }
            if (strRef2PR.Length > 0)
            {
                strRef2PR = strRef2PR.Substring(0, strRef2PR.Length - 2);
            }
            return strRef2PR;
        }

        private string pmGetRefWR(string inWOrderH)
        {

            string strRef2WR = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strGLRefTab = strFMDBName + ".dbo.GLREF";
            string strBookTab = strFMDBName + ".dbo.BOOK";

            string strSQLExec = "";

            strSQLExec = "select REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH  ";
            strSQLExec += " ,(select GLREF.FCCODE from " + strGLRefTab + " GLREF where GLREF.FCSKID = REFDOC.CMASTERH) as QCWRCODE ";
            strSQLExec += " ,(select GLREF.FCBOOK from " + strGLRefTab + " GLREF where GLREF.FCSKID = REFDOC.CMASTERH) as QCBOOK ";

            strSQLExec += " from REFDOC  ";
            strSQLExec += " left join " + strGLRefTab + " GLREF on GLREF.FCSKID = REFDOC.CMASTERH ";
            //strSQLExec += " left join " + strBookTab + " BOOK on BOOK.FCSKID = GLREF.FCBOOK ";
            strSQLExec += " where REFDOC.CMASTERTYP = 'WR' and REFDOC.CCHILDTYPE = 'MO' and REFDOC.CCHILDH = ? ";
            strSQLExec += " and GLREF.FCSTAT <> 'C' ";
            strSQLExec += " group by REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH ";

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { inWOrderH });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef2PR", "WORDERH", strSQLExec, ref strErrorMsg);

            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables["QRef2PR"].Rows)
            {
                if (!Convert.IsDBNull(dtrBrow["QCWRCODE"]))
                {
                    pobjSQLUtil2.SetPara(new object[] { dtrBrow["QCBOOK"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select * from BOOK where FCSKID = ?", ref strErrorMsg))
                    {
                        DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                        strRef2WR += "WR" + dtrBook["FCCODE"].ToString().TrimEnd() + "/" + dtrBrow["QCWRCODE"].ToString().TrimEnd() + ", ";
                    }

                }
            }
            if (strRef2WR.Length > 0)
            {
                strRef2WR = strRef2WR.Substring(0, strRef2WR.Length - 2);
            }
            return strRef2WR;
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
            this.gridView4.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView4.Columns["nQty"].VisibleIndex = i++;
            this.gridView4.Columns["nStdCost"].VisibleIndex = i++;
            this.gridView4.Columns["cQnUOM"].VisibleIndex = i++;
            //this.gridView4.Columns["cScrap"].VisibleIndex = i++;

            this.gridView4.Columns["nRecNo"].Caption = "No.";
            this.gridView4.Columns["cPdType"].Caption = "T";
            this.gridView4.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView4.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView4.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView4.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView4.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView4.Columns["nStdCost"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ต้นทุน", "Std.Cost" });

            this.gridView4.Columns["nRecNo"].Width = 50;
            this.gridView4.Columns["cPdType"].Width = 50;
            this.gridView4.Columns["cQcProd"].Width = 130;
            this.gridView4.Columns["cRemark1"].Width = 80;
            this.gridView4.Columns["nQty"].Width = 80;
            this.gridView4.Columns["nStdCost"].Width = 80;
            this.gridView4.Columns["cQnUOM"].Width = 80;
            //this.gridView4.Columns["cSubSti"].Width = 5;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW3_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW3_CPDTYPE";
            this.grcRemark2.Buttons[0].Tag = "GRDVIEW3_CREMARK1";

            //this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");
            ////this.grcRemark.MaxLength = 150;
            //this.grcRemark2.MaxLength = 150;
            //this.grcScrap.MaxLength = 20;

            //this.gridView4.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView4.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView4.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView4.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView4.Columns["cRemark1"].ColumnEdit = this.grcRemark2;

            this.gridView4.Columns["nStdCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nStdCost"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView4.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nQty"].DisplayFormat.FormatString = "#,###,###,##0.0000";

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
            this.gridView3.Columns["cOPSeq"].VisibleIndex = i++;
            this.gridView3.Columns["cPdType"].VisibleIndex = i++;
            this.gridView3.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView3.Columns["cQnProd"].VisibleIndex = i++;
            this.gridView3.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView3.Columns["nRefQty"].VisibleIndex = i++;
            this.gridView3.Columns["nQty"].VisibleIndex = i++;
            this.gridView3.Columns["nStdCost"].VisibleIndex = i++;
            this.gridView3.Columns["nStdCostAmt"].VisibleIndex = i++;
            this.gridView3.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView3.Columns["cProcure"].VisibleIndex = i++;
            this.gridView3.Columns["cScrap"].VisibleIndex = i++;
            this.gridView3.Columns["cRoundCtrl"].VisibleIndex = i++;
            this.gridView3.Columns["cQcBOM"].VisibleIndex = i++;
            //this.gridView3.Columns["cSubSti"].VisibleIndex = i++;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView3.Columns["cPdType"].Caption = "T";
            this.gridView3.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView3.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView3.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView3.Columns["nRefQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน BOM", "BOM Qty." });
            this.gridView3.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView3.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView3.Columns["cScrap"].Caption = "Scrap";
            this.gridView3.Columns["cProcure"].Caption = "Procure";
            this.gridView3.Columns["cSubSti"].Caption = "S";
            this.gridView3.Columns["cQcBOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BOM", "BOM" });
            this.gridView3.Columns["cRoundCtrl"].Caption = "Round";
            this.gridView3.Columns["nStdCost"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ราคาทุน", "Std. Cost" });
            this.gridView3.Columns["nStdCostAmt"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "มูลค่าทุน", "Cost Amt." });

            this.gridView3.Columns["nRecNo"].Width = 50;
            this.gridView3.Columns["cOPSeq"].Width = 80;
            this.gridView3.Columns["cPdType"].Width = 50;
            this.gridView3.Columns["cQcProd"].Width = 130;
            this.gridView3.Columns["cRemark1"].Width = 80;
            this.gridView3.Columns["nRefQty"].Width = 80;
            this.gridView3.Columns["nQty"].Width = 80;
            this.gridView3.Columns["cQnUOM"].Width = 80;
            this.gridView3.Columns["cScrap"].Width = 80;
            this.gridView3.Columns["cProcure"].Width = 60;
            this.gridView3.Columns["cQcBOM"].Width = 130;
            //this.gridView3.Columns["cSubSti"].Width = 5;
            this.gridView3.Columns["cRoundCtrl"].Width = 60;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW3_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW3_CPDTYPE";
            this.grcRemark2.Buttons[0].Tag = "GRDVIEW3_CREMARK1";
            this.grcQcBOM.Buttons[0].Tag = "GRDVIEW3_CQCBOM";

            this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");
            //this.grcRemark.MaxLength = 150;
            this.grcRemark2.MaxLength = 150;
            this.grcScrap.MaxLength = 20;

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nRefQty"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRefQty"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRefQty"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nRefQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nRefQty"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView3.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nStdCost"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nStdCost"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nStdCost"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nStdCostAmt"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nStdCostAmt"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nStdCostAmt"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView3.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView3.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView3.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView3.Columns["cProcure"].ColumnEdit = this.grcProcure;
            this.gridView3.Columns["cScrap"].ColumnEdit = this.grcScrap;
            this.gridView3.Columns["cQcBOM"].ColumnEdit = this.grcQcBOM;
            this.gridView3.Columns["cRoundCtrl"].ColumnEdit = this.grcRoundCtrl;

            this.gridView3.Columns["nQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView3.Columns["nRefQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nRefQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView3.Columns["nStdCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nStdCost"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView3.Columns["nStdCostAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nStdCostAmt"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView4.Columns["nStdCostAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nStdCostAmt"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView3.Columns["nStdCostAmt"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nStdCostAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.pmCalcColWidth2();
        }

        private void pmInitGridProp_TemOP()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemOP].DefaultView;

            this.grdTemOP.DataSource = this.dtsDataEnv.Tables[this.mstrTemOP];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView2.Columns["cOPSeq"].VisibleIndex = i++;
            this.gridView2.Columns["cNextOP"].VisibleIndex = i++;
            this.gridView2.Columns["cPopGetTime"].VisibleIndex = i++;
            this.gridView2.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView2.Columns["cQcRemark1"].VisibleIndex = i++;
            this.gridView2.Columns["cQcMOPR"].VisibleIndex = i++;
            this.gridView2.Columns["cQcWkCtrH"].VisibleIndex = i++;
            this.gridView2.Columns["cQnWkCtrH"].VisibleIndex = i++;
            this.gridView2.Columns["cQcResource"].VisibleIndex = i++;
            //this.gridView2.Columns["nScrapQty1"].VisibleIndex = i++;
            //this.gridView2.Columns["nLossQty1"].VisibleIndex = i++;
            //this.gridView2.Columns["cQcWHouse2"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView2.Columns["cNextOP"].Caption = "Next OP.";
            this.gridView2.Columns["cPopGetTime"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "เวลา", "Time" });
            this.gridView2.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView2.Columns["cQcRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ QC", "QC. Remark" });
            this.gridView2.Columns["cQcMOPR"].Caption = "OPR.";
            this.gridView2.Columns["cQcWkCtrH"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัส W/C", "W/C Code" });
            this.gridView2.Columns["cQnWkCtrH"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อ W/C", "W/C Name" });
            this.gridView2.Columns["cQcResource"].Caption = "Tool";
            //this.gridView2.Columns["nScrapQty1"].Caption = "% Scrap";
            //this.gridView2.Columns["nLossQty1"].Caption = "% Loss";
            //this.gridView2.Columns["cQcWHouse2"].Caption = "คลัง Scrap";

            this.gridView2.Columns["nRecNo"].Width = 50;
            this.gridView2.Columns["cOPSeq"].Width = 80;
            this.gridView2.Columns["cNextOP"].Width = 80;
            this.gridView2.Columns["cRemark1"].Width = 60;
            this.gridView2.Columns["cQcRemark1"].Width = 100;
            this.gridView2.Columns["cQcMOPR"].Width = 100;
            this.gridView2.Columns["cQcWkCtrH"].Width = 120;
            this.gridView2.Columns["cQnWkCtrH"].Width = 100;
            this.gridView2.Columns["cPopGetTime"].Width = 50;
            this.gridView2.Columns["cQcResource"].Width = 120;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.grcQcMOPR.Buttons[0].Tag = "GRDVIEW2_CQCMOPR";
            this.grcQcWkCtr.Buttons[0].Tag = "GRDVIEW2_CQCWKCTRH";
            this.grcOPTime.Buttons[0].Tag = "GRDVIEW2_CPOPGETTIME";
            this.grcRemark2.Buttons[0].Tag = "GRDVIEW2_CREMARK1";
            this.grcQcRemark.Buttons[0].Tag = "GRDVIEW2_CQCREMARK1";
            this.grcQcResource.Buttons[0].Tag = "GRDVIEW2_QCRESOURCE";

            this.grcQcMOPR.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFStdOprInfo.TableName, QMFStdOprInfo.Field.Code);
            this.grcQcWkCtr.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWorkCenterInfo.TableName, QMFWorkCenterInfo.Field.Code);
            this.grcOPSeq.MaxLength = 20;
            this.grcNextOPSeq.MaxLength = 20;
            this.grcRemark2.MaxLength = 150;

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQnWkCtrH"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cQnWkCtrH"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnWkCtrH"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cOPSeq"].ColumnEdit = this.grcOPSeq;
            this.gridView2.Columns["cNextOP"].ColumnEdit = this.grcNextOPSeq;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView2.Columns["cQcRemark1"].ColumnEdit = this.grcQcRemark;
            this.gridView2.Columns["cQcMOPR"].ColumnEdit = this.grcQcMOPR;
            this.gridView2.Columns["cQcWkCtrH"].ColumnEdit = this.grcQcWkCtr;
            this.gridView2.Columns["cPopGetTime"].ColumnEdit = this.grcOPTime;
            this.gridView2.Columns["cQcResource"].ColumnEdit = this.grcQcResource;
            this.pmCalcColWidth();

        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView2.Columns["nRecNo"].Width
                                    + this.gridView2.Columns["cOPSeq"].Width
                                    + this.gridView2.Columns["cNextOP"].Width
                                    + this.gridView2.Columns["cPopGetTime"].Width
                                    + this.gridView2.Columns["cQcMOPR"].Width
                                    + this.gridView2.Columns["cQcResource"].Width
                                    + this.gridView2.Columns["cQcRemark1"].Width
                                    + this.gridView2.Columns["cQcWkCtrH"].Width
                                    + this.gridView2.Columns["cQnWkCtrH"].Width;

            int intNewWidth = this.Width - intColWidth - 50;
            this.gridView2.Columns["cRemark1"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth1()
        {

            int intColWidth = this.gridView1.Columns[QMFWOrderHDInfo.Field.IsApprove].Width
                                    + this.gridView1.Columns[QMFWOrderHDInfo.Field.Stat].Width
                                    + this.gridView1.Columns[QMFWOrderHDInfo.Field.Step].Width
                                    + this.gridView1.Columns[QMFWOrderHDInfo.Field.Code].Width
                                    + this.gridView1.Columns[QMFWOrderHDInfo.Field.RefNo].Width
                                    + this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].Width
                                    +this.gridView1.Columns["CREF2DOC"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView1.Columns["QNPROD"].Width = (intNewWidth > 70 ? intNewWidth : 70);

        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView3.Columns["nRecNo"].Width
                                    +this.gridView3.Columns["cOPSeq"].Width
                                    +this.gridView3.Columns["cPdType"].Width
                                    +this.gridView3.Columns["cQcProd"].Width
                                    +this.gridView3.Columns["cRemark1"].Width
                                    + this.gridView3.Columns["cScrap"].Width
                                    + this.gridView3.Columns["nQty"].Width
                                    +this.gridView3.Columns["cQnUOM"].Width
                                    +this.gridView3.Columns["cProcure"].Width
                                    + this.gridView3.Columns["cRoundCtrl"].Width
                                    + this.gridView3.Columns["cQcBOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView3.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth3()
        {

            int intColWidth = this.gridView4.Columns["nRecNo"].Width
                                    + this.gridView4.Columns["cPdType"].Width
                                    + this.gridView4.Columns["cQcProd"].Width
                                    + this.gridView4.Columns["cRemark1"].Width
                                    + this.gridView4.Columns["nQty"].Width
                                    + this.gridView4.Columns["nStdCost"].Width
                                    + this.gridView4.Columns["cQnUOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView4.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFWOrderHDInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowEdit = false;
            }

            int i = 0;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Stat].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.IsApprove].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.OPStep].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Step].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.IsPlan].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.RefNo].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].VisibleIndex = i++;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.MfgQty].VisibleIndex = i++;
            this.gridView1.Columns["QCSECT"].VisibleIndex = i++;
            this.gridView1.Columns["QCJOB"].VisibleIndex = i++;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.PRStep].VisibleIndex = i++;
            this.gridView1.Columns["CREF2PR"].VisibleIndex = i++;
            this.gridView1.Columns["CREF2WR"].VisibleIndex = i++;
            //this.gridView1.Columns["CREF2DOC"].VisibleIndex = i++;

            //this.gridView1.Columns[QMFWOrderHDInfo.Field.IsApprove].Caption = "Status";

            this.gridView1.Columns[QMFWOrderHDInfo.Field.Stat].Width = 30;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.IsApprove].Width = 75;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Step].Width = 75;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.OPStep].Width = 60;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Code].Width = 120;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.RefNo].Width = 150;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].Width = 90;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.MfgQty].Width = 90;
            this.gridView1.Columns["QCSECT"].Width = 60;
            this.gridView1.Columns["QCJOB"].Width = 60;
            this.gridView1.Columns["QCPROD"].Width = 120;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.PRStep].Width = 30;
            this.gridView1.Columns["CREF2DOC"].Width = 30;
            this.gridView1.Columns["CREF2PR"].Width = 120;
            this.gridView1.Columns["CREF2WR"].Width = 120;

            this.gridView1.Columns[QMFWOrderHDInfo.Field.IsApprove].Caption = "#APV.";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Stat].Caption = " ";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Step].Caption = "STEP";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.OPStep].Caption = "OP. STEP";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.IsPlan].Caption = "Plan";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Code].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "DOC. CODE" });
            this.gridView1.Columns[QMFWOrderHDInfo.Field.RefNo].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            this.gridView1.Columns[QMFWOrderHDInfo.Field.MfgQty].Caption = "MO.Qty";
            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "PROD. CODE" });
            this.gridView1.Columns["CREF2DOC"].Caption = " ";
            this.gridView1.Columns[QMFWOrderHDInfo.Field.PRStep].Caption = " ";
            this.gridView1.Columns["CREF2PR"].Caption = "#PR";
            this.gridView1.Columns["CREF2WR"].Caption = "#Issue";
            this.gridView1.Columns["QCSECT"].Caption = UIBase.GetAppUIText(new string[] { "แผนก", "Section" });
            this.gridView1.Columns["QCJOB"].Caption = UIBase.GetAppUIText(new string[] { "โครงการ", "Job" });

            this.gridView1.Columns["CREF2DOC"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["CREF2DOC"].ColumnEdit = this.grcRef2Doc;

            this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.Date].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns[QMFWOrderHDInfo.Field.MfgQty].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns[QMFWOrderHDInfo.Field.MfgQty].DisplayFormat.FormatString = "n2";

            this.pmSetSortKey(QMFWOrderHDInfo.Field.Code, true);
            //this.pmCalcColWidth1();

        }

        private void grcRef2Doc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //MessageBox.Show(e.Button.Caption);
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                //this.mstrEditRowID = dtrBrow["cRowID"].ToString();
                //MessageBox.Show(dtrBrow["cCode"].ToString());
                this.pmInitPopUpDialog("VIEWREFTO");
            }
        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth1();
        }

        private void grdTemOP_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView2_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView2_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemOP;
            this.mActiveGrid = this.gridView2;
            //this.gridView2.FocusedColumn = this.gridView2.Columns["cOPSeq"];
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

            this.barMainEdit.Items["barGenIssue"].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items["barGenPR"].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items["barGenPR_Rng"].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items["barUpdatePRStep"].Enabled = (inActivePage == 0 ? true : false);

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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

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

                            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                            objSQLHelper.SetPara(new object[] { this.mstrBook });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QMfgBook", "BOOK", "select * from MFGBOOK where cRowID = ? ", ref strErrorMsg))
                            {
                                DataRow dtrMfgBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                                this.mstrBook_WHouse_FG = dtrMfgBook[QMfgBookInfo.Field.WHouse_FG].ToString().TrimEnd();
                                this.mstrBook_WHouse_RM = dtrMfgBook[QMfgBookInfo.Field.WHouse_RM].ToString().TrimEnd();
                            }

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();

                        }
                    }
                    break;

                case "REFTO":
                    using (Common.dlgGetRefToSO dlgRefTo = new Common.dlgGetRefToSO("SO", this.mstrBranch, this.txtQcCoor.Tag.ToString(), this.txtQcProd.Tag.ToString()))
                    {

                        dlgRefTo.BindData(this.dtsDataEnv.Tables[this.mstrTemRefTo]);
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            string strCoor = "";
                            string strProd = "";
                            decimal decSumSOQty = 0;
                            string strSORefNo = "";
                            foreach (DataRow dtrTemSO in dlgRefTo.RefTable.Rows)
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
                                    decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                                    this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrNewSO);

                                    strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";
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

                            this.txtRefTo.Text = strSORefNo;
                            this.txtQcCoor.Tag = strCoor;
                            this.txtSOQty.Value = decSumSOQty;

                            objSQLHelper.SetPara(new object[1] { this.txtQcCoor.Tag.ToString() });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                            {
                                DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                                this.txtQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                                this.txtQnCoor.Text = dtrCoor["fcSName"].ToString().TrimEnd();
                            }

                            if (this.txtQcProd.Tag.ToString() != strProd)
                            {

                                this.txtQcCoor.Enabled = false;
                                this.txtQnCoor.Enabled = false;
                                this.txtQcProd.Enabled = false;

                                this.txtQcProd.Tag = strProd;
                                objSQLHelper.SetPara(new object[] { this.txtQcProd.Tag.ToString() });
                                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ? ", ref strErrorMsg))
                                {
                                    DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                                    this.txtQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                                    this.txtFGBalQty.Value = this.pmGetStockBal(dtrProd["fcSkid"].ToString(), this.mstrBook_WHouse_FG);

                                    if (objSQLHelper.SetPara(new object[] { dtrProd["fcUM"].ToString() })
                                        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                                    {
                                        this.mstrStdUM = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                                    }
                                    else
                                    {
                                        this.mstrStdUM = "";
                                    }
                                }
                            }

                        }

                        this.pmRecalTotPd();

                    }
                    break;
                case "VIEWREFTO":
                    using (Common.MRP.dlgViewRefToMO dlg = new Common.MRP.dlgViewRefToMO())
                    {
                        //dlg.BindData(this.dtsDataEnv.Tables[this.mstrTemRefTo]);
                        dlg.ShowDialog();
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
                case "GETOPTIME":
                    using (DatabaseForms.Common.dlgGetOPTime dlg = new DatabaseForms.Common.dlgGetOPTime())
                    {
                        dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                        if (dtrGetVal == null)
                        {
                            dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                        }

                        decimal intHour = 0; decimal intMin = 0; decimal intSec = 0;

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]), out intHour, out intMin, out intSec);
                        dlg.SetTime("QUEUE", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]), out intHour, out intMin, out intSec);
                        dlg.SetTime("SETUP", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Process]), out intHour, out intMin, out intSec);
                        dlg.SetTime("PROCESS", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]), out intHour, out intMin, out intSec);
                        dlg.SetTime("TEAR", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]), out intHour, out intMin, out intSec);
                        dlg.SetTime("WAIT", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
                        dlg.SetTime("MOVE", intHour, intMin, intSec);
                        
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {

                            dlg.GetTime("QUEUE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("SETUP", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("PROCESS", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("TEAR", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("WAIT", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("MOVE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

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

                case "QCREMARK":
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

                        dlg.Desc1 = dtrTemPd["cQcRemark1"].ToString().TrimEnd();
                        dlg.Desc2 = dtrTemPd["cQcRemark2"].ToString().TrimEnd();
                        dlg.Desc3 = dtrTemPd["cQcRemark3"].ToString().TrimEnd();
                        dlg.Desc4 = dtrTemPd["cQcRemark4"].ToString().TrimEnd();
                        dlg.Desc5 = dtrTemPd["cQcRemark5"].ToString().TrimEnd();
                        dlg.Desc6 = dtrTemPd["cQcRemark6"].ToString().TrimEnd();
                        dlg.Desc7 = dtrTemPd["cQcRemark7"].ToString().TrimEnd();
                        dlg.Desc8 = dtrTemPd["cQcRemark8"].ToString().TrimEnd();
                        dlg.Desc9 = dtrTemPd["cQcRemark9"].ToString().TrimEnd();
                        dlg.Desc10 = dtrTemPd["cQcRemark10"].ToString().TrimEnd();

                        dlg.SetLabelText(new string[] {
                            UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 1 :","QC Remark 1 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 2 :","QC Remark 2 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 3 :","QC Remark 3 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 4 :","QC Remark 4 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 5 :","QC Remark 5 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 6 :","QC Remark 6 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 7 :","QC Remark 7 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 8 :","QC Remark 8 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 9 :","QC Remark 9 :"})
                            ,UIBase.GetAppUIText(new string[] {"หมายเหตุ QC 10 :","QC Remark 10 :"}) });

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            dtrTemPd["cQcRemark1"] = dlg.Desc1;
                            dtrTemPd["cQcRemark2"] = dlg.Desc2;
                            dtrTemPd["cQcRemark3"] = dlg.Desc3;
                            dtrTemPd["cQcRemark4"] = dlg.Desc4;
                            dtrTemPd["cQcRemark5"] = dlg.Desc5;
                            dtrTemPd["cQcRemark6"] = dlg.Desc6;
                            dtrTemPd["cQcRemark7"] = dlg.Desc7;
                            dtrTemPd["cQcRemark8"] = dlg.Desc8;
                            dtrTemPd["cQcRemark9"] = dlg.Desc9;
                            dtrTemPd["cQcRemark10"] = dlg.Desc10;

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
                case "GENPR":
                    using (Common.dlgGetPROption01 dlgPR = new Common.dlgGetPROption01("PR"))
                    {
                        this.mstrGenPR_Branch = "";
                        this.mstrGenPR_Book = "";
                        this.mstrGenPR_Coor = "";
                        dlgPR.BranchID = this.mstrBranch;
                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenPR_Branch = dlgPR.BranchID;
                            this.mstrGenPR_Book = dlgPR.BookID;
                            this.mstrGenPR_Coor = dlgPR.CoorID;
                            this.mdttGenPR_Date = dlgPR.DocDate.Date;
                            this.pmGenPR(this.mstrSaveRowID);
                        }
                    }
                    break;
                case "GENPR_RNG":
                    using (Common.dlgGetPROption02 dlgPR = new Common.dlgGetPROption02("PR", this.mstrPlant, this.mstrBook))
                    {
                        this.mstrGenPR_Branch = "";
                        this.mstrGenPR_Book = "";
                        this.mstrGenPR_Coor = "";
                        dlgPR.BranchID = this.mstrBranch;
                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenPR_Branch = dlgPR.BranchID;
                            this.mstrGenPR_Book = dlgPR.BookID;
                            this.mstrGenPR_Coor = dlgPR.CoorID;
                            this.mdttGenPR_Date = dlgPR.DocDate.Date;

                            this.mstrGenRngPR_OrderBy = dlgPR.OrderBy;
                            this.mstrGenRngPR_BegCode = dlgPR.MO_BegCode;
                            this.mstrGenRngPR_EndCode = dlgPR.MO_EndCode;
                            this.mdttGenRngPR_BegDate = dlgPR.MO_BegDate.Date;
                            this.mdttGenRngPR_EndDate = dlgPR.MO_EndDate.Date;

                            this.pmGenRangePR();
                        }
                    }
                    break;
                case "GENISSUE":
                    using (Common.MRP.dlgGetWROption01 dlgPR = new Common.MRP.dlgGetWROption01("WR"))
                    {

                        this.mstrGenPR_Branch = "";
                        this.mstrGenPR_Book = "";
                        dlgPR.BranchID = this.mstrBranch;
                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenPR_Branch = dlgPR.BranchID;
                            this.mstrGenPR_Book = dlgPR.BookID;
                            this.mdttGenPR_Date = dlgPR.DocDate.Date;
                            this.pmGenIssue(dlgPR.IsValidStock);
                        }
                    }
                    break;
                case "GENPLAN_RNG":
                    using (dlgGetPlanOption01 dlgPR = new dlgGetPlanOption01(this.mstrBranch, this.mstrPlant, this.mstrBook))
                    {

                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenRngPR_OrderBy = dlgPR.OrderBy;
                            this.mstrGenRngPR_BegCode = dlgPR.MO_BegCode;
                            this.mstrGenRngPR_EndCode = dlgPR.MO_EndCode;
                            this.mdttGenRngPR_BegDate = dlgPR.MO_BegDate.Date;
                            this.mdttGenRngPR_EndDate = dlgPR.MO_EndDate.Date;

                            this.pmCreatePlanningTable();
                            MessageBox.Show("Generate Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;
            }
        }

        private void pmCreatePlanningTable()
        {

            string strErrorMsg = "";

            string strSQLStrOrderI_OP = "select MFWORDERIT_STDOP.*, MFWORDERIT_STDOP.NT_QUEUE+MFWORDERIT_STDOP.NT_SETUP+MFWORDERIT_STDOP.NT_TEAR+MFWORDERIT_STDOP.NT_WAIT+MFWORDERIT_STDOP.NT_MOVE as FIX_Proc_Time ";
            strSQLStrOrderI_OP += " from " + this.mstrITable;
            strSQLStrOrderI_OP += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWORDERIT_STDOP.CWKCTRH ";
            strSQLStrOrderI_OP += " where MFWORDERIT_STDOP.cWOrderH = ? order by MFWORDERIT_STDOP.cSeq ";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? ";
            if (this.mstrGenRngPR_OrderBy == "CCODE")
            {
                strSQLStr += " and cCode between ? and ? order by cCode";
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mstrGenRngPR_BegCode, this.mstrGenRngPR_EndCode });
            }
            else
            {
                strSQLStr += " and dDate between ? and ? order by dDate";
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttGenRngPR_BegDate, this.mdttGenRngPR_EndDate });
            }


            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrMOrderH in this.dtsDataEnv.Tables["QMOrderH"].Rows)
                {

                    decimal decMfgFactor = (Convert.ToDecimal(dtrMOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(dtrMOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);

                    this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Clear();
                    this.dtsDataEnv.Tables[this.mstrTemUsage].Rows.Clear();
                    this.dtsDataEnv.Tables[this.mstrTemWorkCal].Rows.Clear();
                    //Create Work Hour
                    this.pmLoadWorkCalendar(Convert.ToDateTime(dtrMOrderH[QMFWOrderHDInfo.Field.DueDate]));
                    //Load OP
                    pobjSQLUtil.SetPara(new object[] { dtrMOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOrderOP", this.mstrITable, strSQLStrOrderI_OP, ref strErrorMsg))
                    {

                        foreach (DataRow dtrMOrderOP in this.dtsDataEnv.Tables["QMOrderOP"].Rows)
                        {
                            if (Convert.ToDecimal(dtrMOrderOP["FIX_Proc_Time"]) + Convert.ToDecimal(dtrMOrderOP["NT_PROCESS"]) > 0)
                            {
                                this.pmAddRow_MOrderOP(dtrMOrderOP["cRowID"].ToString(), dtrMOrderOP["COPSEQ"].ToString(), dtrMOrderOP["COPSEQ"].ToString(), dtrMOrderOP["CWKCTRH"].ToString(), decMfgFactor, Convert.ToDecimal(dtrMOrderOP["NT_PROCESS"]), Convert.ToDecimal(dtrMOrderOP["FIX_Proc_Time"]));
                            }
                        }

                        DataView dv = dtsDataEnv.Tables[this.mstrTemMOrderOP].DefaultView;
                        dv.Sort = "OPSeq desc";
                        DateTime dttCurrDate = Convert.ToDateTime(dtrMOrderH[QMFWOrderHDInfo.Field.DueDate]);
                        dttCurrDate = dttCurrDate.AddDays(-1);
                        foreach (DataRowView dtrView in dv)
                        {
                            this.pmReleaseHour(dtrView["cRowID"].ToString(), dtrView["OPSeq"].ToString(), dtrView["WkCtrH"].ToString(), ref dttCurrDate, Convert.ToDecimal(dtrView["HourUsage"]));
                        }
                        //Save To Planning Table
                        //
                        this.pmSavePlanningTable(dtrMOrderH, ref strErrorMsg);

                    }

                }
            }


        }

        private void pmSavePlanningTable(DataRow inMOrderH, ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            bool bllIsCommit = false;
            bool bllResult = false;
            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.mSaveDBAgent.BatchSQLExec("delete from MFWORDERIT_OPPLAN where CWORDERH = ? ", new object[1] { inMOrderH["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.pmUpdateWOrderIT_Plan(inMOrderH);

                this.mSaveDBAgent.BatchSQLExec("update MFWORDERHD set CISPLAN = 'Y' where CROWID = ? ", new object[1] { inMOrderH["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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
                    this.pofrmGetBOM.ValidateField(this.txtQcProd.Tag.ToString(), inPara1, strPrefix, true);
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

                    dtrGetVal = this.pofrmGetCoor.RetrieveValue();
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
                case "TXTQCBOM":
                case "TXTQNBOM":

                    DataRow dtrPopup = this.pofrmGetBOM.RetrieveValue();

						if (dtrPopup != null)
						{
							this.mdecUMQty = Convert.ToDecimal(dtrPopup["nMfgUmQty"]);
							this.mdecUMQty = (this.mdecUMQty == 0 ? 1 : this.mdecUMQty);

							objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                            if (this.txtQcProd.Tag.ToString().TrimEnd() == string.Empty)
                            {
                                if (objSQLHelper.SetPara(new object[] { dtrPopup["cMfgProd"].ToString() })
                                    && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ? ", ref strErrorMsg))
                                {
                                    DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                                    this.txtQcProd.Tag = dtrProd["fcSkid"].ToString().TrimEnd();
                                    this.txtQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                                    this.txtFGBalQty.Value = this.pmGetStockBal(dtrProd["fcSkid"].ToString(), this.mstrBook_WHouse_FG);

                                    if (objSQLHelper.SetPara(new object[] { dtrProd["fcUM"].ToString() })
                                        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                                    {
                                        this.mstrStdUM = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                                    }
                                    else
                                    {
                                        this.mstrStdUM = "";
                                    }

                                }
                            }
                            else
                            {
                                //this.mbllIsReviseBOMOnly = (this.txtQcProd.Tag.ToString().TrimEnd() == dtrPopup["cMfgProd"].ToString());
                                //TODO: For test
                                this.mbllIsReviseBOMOnly = false;
                                //this.mbllIsReviseBOMOnly = true;
                            }

                            if (this.txtQnMfgUM.Tag.ToString().Trim() == string.Empty)
                            {
                                if (objSQLHelper.SetPara(new object[] { dtrPopup["cMfgUM"].ToString() })
                                    && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                                {
                                    this.txtQnMfgUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                                    this.txtQnMfgUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                                }
                                else
                                {
                                    this.txtQnMfgUM.Tag = "";
                                    this.txtQnMfgUM.Text = "";
                                }
                            }

							//Load Detail
                            if (this.txtQcBOM.Tag.ToString() != dtrPopup["cRowID"].ToString())
							{
                                this.txtQcBOM.Tag = dtrPopup["cRowID"].ToString();

                                string strRound = dtrPopup[QMFBOMInfo.Field.RoundCtrl].ToString().TrimEnd();
                                switch (strRound)
                                {
                                    case "1":
                                        this.cmdRoundCtrl.SelectedIndex = 1;
                                        break;
                                    case "2":
                                        this.cmdRoundCtrl.SelectedIndex = 2;
                                        break;
                                    default:
                                        this.cmdRoundCtrl.SelectedIndex = 0;
                                        break;
                                }

								this.mdecPdStOutput = (Convert.ToDecimal(dtrPopup["nMfgQty"]) > 0 ? Convert.ToDecimal(dtrPopup["nMfgQty"]) : 1);
                                this.txtPdStOutput.Value = this.mdecPdStOutput;

                                if (this.txtMfgQty.Value == 0)
                                {
                                    this.txtMfgQty.EditValue = (Convert.ToDecimal(dtrPopup["nMfgQty"]) > 0 ? Convert.ToDecimal(dtrPopup["nMfgQty"]) : 1);
                                }
                                this.txtTotMfgQty.EditValue = Convert.ToDecimal(this.txtMfgQty.EditValue) + this.mdecScrapQty;

                                this.txtScrap.Text = dtrPopup["cScrap"].ToString().TrimEnd();
                                //this.mdecScrapQty = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);

                                decimal decScrapQty1 = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);
                                if (decScrapQty1 != 0)
                                {
                                    this.mdecScrapQty = decScrapQty1 - Convert.ToDecimal(this.txtMfgQty.EditValue);
                                }

                                this.mdecLastMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
                                this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);

                                this.pmLoadPdStDetail();

                                if (this.txtSOQty.Value != 0)
                                {
                                    this.txtMfgQty.Value = this.txtSOQty.Value;
                                    decScrapQty1 = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);
                                    if (decScrapQty1 != 0)
                                    {
                                        this.mdecScrapQty = decScrapQty1 - Convert.ToDecimal(this.txtMfgQty.EditValue);
                                    }
                                }
                                this.txtTotMfgQty.EditValue = Convert.ToDecimal(this.txtMfgQty.EditValue) + this.mdecScrapQty;
                                this.pmAdjustMfgQty();

                                this.mdecLastMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
                                this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);

							}
                            this.txtQcBOM.Text = dtrPopup["cCode"].ToString().TrimEnd();
                            this.txtQnBOM.Text = dtrPopup["cName"].ToString().TrimEnd();

						}
						else
						{
							this.txtQcBOM.Tag = "";
							this.txtQcBOM.Text = "";
							this.txtQnBOM.Text = "";
							this.txtQnMfgUM.Tag = "";
							this.txtQnMfgUM.Text = "";
						}
						this.pmSetUMQtyMsg();
                        this.pmRecalTotPd();
                        break;   
                case "TXTQCPROD":
                case "TXTQNPROD":

                    dtrGetVal = this.pofrmGetProd.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        if (this.txtQcProd.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                        {
                            this.txtQcProd.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtFGBalQty.Value = this.pmGetStockBal(dtrGetVal["fcSkid"].ToString(), this.mstrBook_WHouse_FG);
                        }

                        this.txtQcProd.Text = dtrGetVal["fcCode"].ToString().TrimEnd();

                        if (objSQLHelper.SetPara(new object[] { dtrGetVal["fcUM"].ToString() })
                            && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid , fcCode, fcName from UM where fcSkid = ? ", ref strErrorMsg))
                        {
                            this.txtQnMfgUM.Tag = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            this.txtQnMfgUM.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            this.mstrStdUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            //this.txtQnStdUM.Tag = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            //this.txtQnStdUM.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }
                    else
                    {
                        this.txtQcProd.Tag = "";
                        this.txtQcProd.Text = "";

                        this.txtQnMfgUM.Tag = "";
                        this.txtQnMfgUM.Text = "";

                        this.mstrStdUM = "";
                        this.txtFGBalQty.Value = 0;
                        //this.txtQnStdUM.Tag = "";
                        //this.txtQnStdUM.Text = "";

                    }
                    break;
                case "TXTQNMFGUM":

                    dtrGetVal = this.pofrmGetUM.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        if (this.txtQnMfgUM.Tag.ToString() != dtrGetVal["fcSkid"].ToString()
                                && dtrGetVal["fcSkid"].ToString() != this.mstrStdUM)
                        {
                            this.pmInitPopUpDialog("UOMCONVERT");
                        }

                        this.txtQnMfgUM.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQnMfgUM.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQnMfgUM.Tag = "";
                        this.txtQnMfgUM.Text = "";
                    }
                    this.pmSetUMQtyMsg();
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
                
                case "GRDVIEW2_CQCWKCTRH":
                case "GRDVIEW2_CQNWKCTRH":
                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetWkCtr = this.pofrmGetWkCtr.RetrieveValue();
                    if (dtrGetWkCtr != null)
                    {

                        if (dtrGetVal["cWkCtrH"].ToString() != dtrGetWkCtr[MapTable.ShareField.RowID].ToString())
                        {
                            dtrGetVal["cResource"] = "";
                            dtrGetVal["cQcResource"] = "";
                            dtrGetVal["cQnResource"] = "";
                        }
                        
                        dtrGetVal["cWkCtrH"] = dtrGetWkCtr[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcWkCtrH"] = dtrGetWkCtr[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                        dtrGetVal["cQnWkCtrH"] = dtrGetWkCtr[QEMAcChartInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cWkCtrH"] = "";
                        dtrGetVal["cQcWkCtrH"] = "";
                        dtrGetVal["cQnWkCtrH"] = "";

                        dtrGetVal["cResource"] = "";
                        dtrGetVal["cQcResource"] = "";
                        dtrGetVal["cQnResource"] = "";
                    
                    }

                    this.mActiveGrid.UpdateCurrentRow();

                    break;

                case "GRDVIEW2_CQCMOPR":
                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetOP = this.pofrmGetStdOP.RetrieveValue();
                    if (dtrGetOP != null)
                    {
                        dtrGetVal["cMOPR"] = dtrGetOP[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcMOPR"] = dtrGetOP[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cMOPR"] = "";
                        dtrGetVal["cQcMOPR"] = "";
                    }

                    this.mActiveGrid.UpdateCurrentRow();
                    break;

                case "GRDVIEW2_CQCRESOURCE":
                    dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetRes = this.pofrmGetResource.RetrieveValue();
                    if (dtrGetRes != null)
                    {
                        dtrGetVal["cResource"] = dtrGetRes[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcResource"] = dtrGetRes[QMFResourceInfo.Field.Code].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cResource"] = "";
                        dtrGetVal["cQcResource"] = "";
                    }

                    this.gridView2.UpdateCurrentRow();
                    break;
                
                case "GRDVIEW3_CPDTYPE":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrGetVal);
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

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrGetVal);
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
                        dtrGetVal["nStdCost"] = Convert.ToDecimal(dtrGetProd["fnStdCost"]);
                        dtrGetVal["nStdCostAmt"] = Convert.ToDecimal(dtrGetVal["nStdCost"]) * Convert.ToDecimal(dtrGetVal["nQty"]);

                        if (!Convert.IsDBNull(dtrGetProd["fmPicName"]))
                        {
                            if (dtrGetProd["fmPicName"].ToString().Trim() != "...")
                            {
                                dtrGetVal["cAttachFile"] = dtrGetProd["fmPicName"].ToString().TrimEnd();
                            }
                        }
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
                    this.pmRecalTotPd();

                    break;

                case "GRDVIEW3_CQCBOM":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetBOM = this.pofrmGetBOM.RetrieveValue();
                    if (dtrGetBOM != null)
                    {
                        dtrGetVal["cMfgBOMHD"] = dtrGetBOM["cRowID"].ToString();
                        dtrGetVal["cQcBOM"] = dtrGetBOM["cCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cMfgBOMHD"] = "";
                        dtrGetVal["cQcBOM"] = "";
                    }

                    this.mActiveGrid.UpdateCurrentRow();

                    break;
            }
        }

        private decimal pmGetStockBal(string inProd, string inWHouse)
        {

            decimal decBalStk = 0;
            string strBook_WHouse = inWHouse;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = " select REFPROD.FCPROD , REFPROD.FCIOTYPE , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNQTY ";
            strSQLStr += " from REFPROD ";
            strSQLStr += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " where REFPROD.FCPROD = ? ";
            strSQLStr += " and WHOUSE.FCCORP = ? and WHOUSE.FCTYPE = ' ' ";
            strSQLStr += (strBook_WHouse.Trim() != string.Empty ? " and WHOUSE.FCCODE in (" + strBook_WHouse + ") " : "");
            strSQLStr += " and GLREF.FCSTAT <> 'C' ";
            strSQLStr += " group by REFPROD.FCPROD,REFPROD.FCIOTYPE ";
            pobjSQLUtil.SetPara(new object[] { inProd, App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBal", "REFPROD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBal in this.dtsDataEnv.Tables["QBal"].Rows)
                {
                    decimal decSign = (dtrBal["fcIOType"].ToString().Trim() == "I" ? 1 : -1);
                    decBalStk = decBalStk + (Convert.ToDecimal(dtrBal["fnQty"]) * decSign);
                }
            }
            //this.txtFGBalQty.Value = decBalStk;
            return decBalStk;
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

        private void pmLoadPdStDetail()
        {
            int intRecNo = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //TODO: มาทำต่อแก้ BUG
            if (this.mbllIsReviseBOMOnly)
            { 
            }
            else
            {
                this.pmClearTemToBlank();
            }

            if (this.txtQcProd.Tag.ToString().TrimEnd() != string.Empty)
                this.pmReplTemFG();

            pobjSQLUtil.SetPara(new object[] { this.txtQcBOM.Tag.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStI", "PDSTI", "select * from MFBOMIT_PD where CBOMHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables["QPdStI"].Rows)
                {
                    intRecNo++;
                    this.pmReplTemPdStI(intRecNo, dtrPdStI);
                }
            }

            intRecNo = 0;
            pobjSQLUtil.SetPara(new object[] { this.txtQcBOM.Tag.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMFBOMIT_STDOP", "MFBOMIT_STDOP", "select * from MFBOMIT_STDOP where CBOMHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrMFBOMIT_STDOP in this.dtsDataEnv.Tables["QMFBOMIT_STDOP"].Rows)
                {
                    intRecNo++;
                    this.pmReplTemMFBOMIT_STDOP(intRecNo, dtrMFBOMIT_STDOP);
                }
            }

        }

        private void pmReplTemFG()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemFG].NewRow();
            dtrTemPd["nRecNo"] = 1;
            dtrTemPd["cProd"] = this.txtQcProd.Tag.ToString();
            dtrTemPd["cLastProd"] = this.txtQcProd.Tag.ToString();
            //dtrTemPd["cUOM"] = this.txtQnUM.Tag.ToString();
            //dtrTemPd["nQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            //dtrTemPd["nRefQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            //dtrTemPd["nUOMQty"] = 1;
            //dtrTemPd["nLastQty"] = 1;

            dtrTemPd["cUOM"] = this.txtQnMfgUM.Tag.ToString();
            dtrTemPd["nQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            dtrTemPd["nSaveQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            dtrTemPd["nRefQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            dtrTemPd["nUOMQty"] = this.mdecUMQty;
            dtrTemPd["nLastQty"] = Convert.ToDecimal(this.txtMfgQty.EditValue);

            //dtrTemPd["cWHouse"] = this.mstrDefaWHouse;
            dtrTemPd["cWHouse"] = this.mstrDefaFGWHouse;

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                dtrTemPd["cPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cLastPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = dtrProd["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
                dtrTemPd["nStdCost"] = Convert.ToDecimal(dtrProd["fnStdCost"]);
                dtrTemPd["nStdCostAmt"] = Convert.ToDecimal(dtrTemPd["nStdCost"]) * Convert.ToDecimal(dtrTemPd["nQty"]);
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

            this.dtsDataEnv.Tables[this.mstrTemFG].Rows.Add(dtrTemPd);
        }

        //TODO: มาทำต่อแก้ BUG
        private DataRow pmNewRow(ref bool ioIsNew, string inSeq, string inProd)
        {
            DataRow dtrTemPd = null;
            if (this.mbllIsReviseBOMOnly)
            {
                foreach (DataRow dtrNewRow in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
                {
                    if (dtrNewRow["cOPSeq"].ToString().Trim() == inSeq && dtrNewRow["cProd"].ToString() == inProd)
                    {
                        //Found match Row
                        dtrTemPd = dtrNewRow;
                        ioIsNew = false;
                        break;
                    }
                }
                if (dtrTemPd == null)
                {
                    dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    ioIsNew = true;
                }
            }
            else
            {
                dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                ioIsNew = true;
            }
            return dtrTemPd;
        }

        private void pmReplTemPdStI(int inRecNo, DataRow inTemPd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //TODO: มาทำต่อแก้ BUG
            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            bool bllIsNew = false;
            DataRow dtrTemPd = this.pmNewRow(ref bllIsNew, inTemPd["cOPSeq"].ToString().TrimEnd(), inTemPd["cProd"].ToString());
            if (bllIsNew)
            {
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
            }

            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inTemPd["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inTemPd["cProd"].ToString();
            dtrTemPd["cLastProd"] = inTemPd["cProd"].ToString();
            dtrTemPd["cUOM"] = inTemPd["cUM"].ToString().TrimEnd();
            dtrTemPd["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrTemPd["nRefQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrTemPd["nSaveQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inTemPd["nUMQty"]) == 0 ? 1 : Convert.ToDecimal(inTemPd["nUMQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrTemPd["nBackQty1"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrTemPd["nBackQty2"] = Convert.ToDecimal(inTemPd["nQty"]);

            //dtrTemPd["cWHouse"] = this.mstrDefaWHouse;
            dtrTemPd["cWHouse"] = this.mstrDefaWRWHouse;

            dtrTemPd["cProcure"] = inTemPd["cProcure"].ToString().TrimEnd();
            dtrTemPd["cScrap"] = inTemPd["cScrap"].ToString().TrimEnd();
            dtrTemPd["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString().TrimEnd();
            dtrTemPd["cPdStI"] = inTemPd["cRowID"].ToString();
            dtrTemPd["cMfgBOMHD"] = inTemPd["cMfgBOMHD"].ToString();

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                dtrTemPd["cPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cLastPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = dtrProd["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();

                dtrTemPd["nStdCost"] = Convert.ToDecimal(dtrProd["fnStdCost"]);
                dtrTemPd["nStdCostAmt"] = Convert.ToDecimal(dtrTemPd["nStdCost"]) * Convert.ToDecimal(dtrTemPd["nQty"]);

            }
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cUOM"].ToString().TrimEnd() });
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

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", QMFBOMInfo.TableName, "select CCODE from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            dtrTemPd["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrTemPd["cRemark2"] = inTemPd["cRemark2"].ToString().TrimEnd();
            dtrTemPd["cRemark3"] = inTemPd["cRemark3"].ToString().TrimEnd();
            dtrTemPd["cRemark4"] = inTemPd["cRemark4"].ToString().TrimEnd();
            dtrTemPd["cRemark5"] = inTemPd["cRemark5"].ToString().TrimEnd();
            dtrTemPd["cRemark6"] = inTemPd["cRemark6"].ToString().TrimEnd();
            dtrTemPd["cRemark7"] = inTemPd["cRemark7"].ToString().TrimEnd();
            dtrTemPd["cRemark8"] = inTemPd["cRemark8"].ToString().TrimEnd();
            dtrTemPd["cRemark9"] = inTemPd["cRemark9"].ToString().TrimEnd();
            dtrTemPd["cRemark10"] = inTemPd["cRemark10"].ToString().TrimEnd();

            //this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
        }

        private void pmReplTemMFBOMIT_STDOP(int inRecNo, DataRow inTemOP)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inTemOP["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cNextOP"] = inTemOP["cNextOP"].ToString().TrimEnd();
            dtrTemPd["cMOPR"] = inTemOP["cMOPR"].ToString().TrimEnd();
            dtrTemPd["cWkCtrH"] = inTemOP["cWkCtrH"].ToString().TrimEnd();
            dtrTemPd["cResource"] = inTemOP["cResource"].ToString().TrimEnd();
            dtrTemPd["cRemark1"] = inTemOP["cRemark1"].ToString().TrimEnd();
            dtrTemPd["cRemark2"] = inTemOP["cRemark2"].ToString().TrimEnd();
            dtrTemPd["cRemark3"] = inTemOP["cRemark3"].ToString().TrimEnd();
            dtrTemPd["cRemark4"] = inTemOP["cRemark4"].ToString().TrimEnd();
            dtrTemPd["cRemark5"] = inTemOP["cRemark5"].ToString().TrimEnd();
            dtrTemPd["cRemark6"] = inTemOP["cRemark6"].ToString().TrimEnd();
            dtrTemPd["cRemark7"] = inTemOP["cRemark7"].ToString().TrimEnd();
            dtrTemPd["cRemark8"] = inTemOP["cRemark8"].ToString().TrimEnd();
            dtrTemPd["cRemark9"] = inTemOP["cRemark9"].ToString().TrimEnd();
            dtrTemPd["cRemark10"] = inTemOP["cRemark10"].ToString().TrimEnd();

            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Process]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Move]);

            dtrTemPd["cQcRemark1"] = inTemOP["cQcRemark1"].ToString().TrimEnd();
            dtrTemPd["cQcRemark2"] = inTemOP["cQcRemark2"].ToString().TrimEnd();
            dtrTemPd["cQcRemark3"] = inTemOP["cQcRemark3"].ToString().TrimEnd();
            dtrTemPd["cQcRemark4"] = inTemOP["cQcRemark4"].ToString().TrimEnd();
            dtrTemPd["cQcRemark5"] = inTemOP["cQcRemark5"].ToString().TrimEnd();
            dtrTemPd["cQcRemark6"] = inTemOP["cQcRemark6"].ToString().TrimEnd();
            dtrTemPd["cQcRemark7"] = inTemOP["cQcRemark7"].ToString().TrimEnd();
            dtrTemPd["cQcRemark8"] = inTemOP["cQcRemark8"].ToString().TrimEnd();
            dtrTemPd["cQcRemark9"] = inTemOP["cQcRemark9"].ToString().TrimEnd();
            dtrTemPd["cQcRemark10"] = inTemOP["cQcRemark10"].ToString().TrimEnd();

            //By Yod 22/02/09
            dtrTemPd["cMFBOMIT_STDOP"] = inTemOP["cRowID"].ToString().TrimEnd();
            //By Yod 03/03/09
            decimal decSemiQty = this.pmCalOutputQty(dtrTemPd["cOPSeq"].ToString());
            dtrTemPd["nRefQty1"] = decSemiQty;
            dtrTemPd["nRefQty2"] = 0;

            //decimal decQty1 = decSemiQty * Convert.ToDecimal(inTemOP["nScrapQty1"]) / 100;
            //decimal decQty2 = decSemiQty * Convert.ToDecimal(inTemOP["nLossQty1"]) / 100;
            //dtrTemPd["nScrapQty1"] = decQty1;
            //dtrTemPd["nLossQty1"] = decQty2;

            //dtrTemPd["nScrapQty1"] = Convert.ToDecimal(inTemOP["nScrapQty1"]);
            //dtrTemPd["nLossQty1"] = Convert.ToDecimal(inTemOP["nLossQty1"]);

            //dtrTemPd["cWHouse2"] = inTemOP["cWHouse2"].ToString();
            //pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cWHouse2"].ToString() });
            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcCode, fcName from WHOUSE where fcSkid = ?", ref strErrorMsg))
            //{
            //    dtrTemPd["cQcWHouse2"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
            //    dtrTemPd["cQnWHouse2"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            //}

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cMOPR"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode, cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcMOPR"] = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cWkCtrH"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWkCtrH", "WKCTRH", "select cCode, cName from " + QMFWorkCenterInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcWkCtrH"] = this.dtsDataEnv.Tables["QWkCtrH"].Rows[0]["cCode"].ToString().TrimEnd();
                dtrTemPd["cQnWkCtrH"] = this.dtsDataEnv.Tables["QWkCtrH"].Rows[0]["cName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[] { dtrTemPd["cResource"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFResourceInfo.TableName, "select * from " + QMFResourceInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcResource"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Code].ToString().TrimEnd();
            }

            dtrTemPd["nCapFactor1"] = Convert.ToDecimal(inTemOP["nCapFactor1"]);
            dtrTemPd["nCapPress"] = Convert.ToDecimal(inTemOP["nCapPress"]);

            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrTemPd);
        }

        private void pmClearTemToBlank()
        {
            foreach (DataRow dtrTemFG in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
            {
                DataRow dtrTem = dtrTemFG;
                this.pmClr1TemPd(ref dtrTem);
            }

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                DataRow dtrTem = dtrTemPd;
                this.pmClr1TemPd(ref dtrTem);
            }

            foreach (DataRow dtrTemOP in this.dtsDataEnv.Tables[this.mstrTemOP].Rows)
            {
                DataRow dtrTem = dtrTemOP;
                this.pmClr1TemOP(ref dtrTem);
            }

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

        private void pmClr1TemPd(ref DataRow inTemPd)
        {
            DataRow dtrTemPd = inTemPd;
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
            dtrTemPd["nBackQty1"] = 0;
            dtrTemPd["nBackQty2"] = 0;
        }

        private void pmClr1TemOP(ref DataRow inTemOP)
        {
            DataRow dtrTemOP = inTemOP;

            dtrTemOP["cOPSeq"] = "";
            dtrTemOP["cMOPR"] = "";
            dtrTemOP["cQcMOPR"] = "";
            dtrTemOP["cWkCtrH"] = "";
            dtrTemOP["cQcWkCtrH"] = "";
            dtrTemOP["cQnWkCtrH"] = "";
            dtrTemOP["cRemark1"] = "";
            dtrTemOP["cWHouse2"] = "";
            dtrTemOP["cQcWHouse2"] = "";
            dtrTemOP["cQnWHouse2"] = "";
            dtrTemOP["nScrapQty1"] = 0;
            dtrTemOP["nLossQty1"] = 0;
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
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcProd_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCPROD" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcProd.Tag = "";
                this.txtQcProd.Text = "";
                this.txtFGBalQty.Value = 0;
            }
            else
            {
                this.pmInitPopUpDialog("PROD");
                e.Cancel = !this.pofrmGetProd.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetProd.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcBOM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBOM" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBOM.Tag = "";
                this.txtQcBOM.Text = "";
                this.txtQnBOM.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BOM");
                e.Cancel = !this.pofrmGetBOM.ValidateField(this.txtQcProd.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBOM.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
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
        
        private void txtQnMfgUM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCMFGUM" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQnMfgUM.Tag = "";
                this.txtQnMfgUM.Text = "";
                this.pmSetUMQtyMsg();
            }
            else
            {
                this.pmInitPopUpDialog("UM");
                e.Cancel = !this.pofrmGetUM.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetUM.PopUpResult)
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

            string strErrorMsg = "";
            string strRowID = "";
            bool bllResult = false;
            DataRow dtrBrow = null;
            WS.Data.Agents.cDBMSAgent objSQLHelper = null;

            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "PRNPDF":

                        dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                        if (this.gridView1.RowCount == 0 || dtrBrow == null)
                            return;
                        
                        printDialog1.ShowDialog();
                        string strPrinterName = printDialog1.PrinterSettings.PrinterName;
                        strErrorMsg = "";
                        string strCode = dtrBrow["cCode"].ToString().TrimEnd();
                        WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                        WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

                        string strSQLStr = "";
                        strSQLStr = "select * from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cCode between ? and ? order by cCode";
                        pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, strCode, strCode });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
                        {

                            string strBOM = this.dtsDataEnv.Tables["QOrderH"].Rows[0][QMFWOrderHDInfo.Field.BOMID].ToString();
                            pobjSQLUtil2.SetPara(new object[1] { strBOM });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select * from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                            {
                                string strBOM_AttachFile = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cFileAttn"].ToString();
                                BizRule.PrintPdfFile(strBOM_AttachFile, strPrinterName, ref strErrorMsg);
                                if (strErrorMsg != string.Empty)
                                {
                                    MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }


                        break;

                    case "GENPR_RNG":
                        this.pmInitPopUpDialog("GENPR_RNG");
                        break;
                    case "GENPR":

                        bllResult = true;

                        if (pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
                        {
                            bllResult = this.pmSaveData();
                        }

                        if (bllResult)
                        {
                            strErrorMsg = "";
                            strRowID = "";
                            if (this.mstrSaveRowID == string.Empty)
                            {
                                dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                                if (this.gridView1.RowCount == 0 || dtrBrow == null)
                                    return;

                                strRowID = dtrBrow["cRowID"].ToString();
                            }

                            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

                            DataRow dtrLoadHead = null;
                            objSQLHelper.SetPara(new object[] { strRowID });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
                            {
                                dtrLoadHead = this.dtsDataEnv.Tables["QLoadPR"].Rows[0];
                            }

                            this.pmInitPopUpDialog("GENPR");

                        }
                        break;

                    case "UPD_PRSTEP":

                        dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                        if (this.gridView1.RowCount == 0 || dtrBrow == null)
                            return;

                        dtrBrow["cPRStep"] = this.pmUpdPRStep(dtrBrow["cRowID"].ToString(), dtrBrow["cRefNo"].ToString().Trim(), dtrBrow["cPRStep"].ToString().Trim());

                        break;

                    case "GENISSUE":
                        this.pmInitPopUpDialog("GENISSUE");
                        break;
                    case "GENPLANNING":
                        this.pmInitPopUpDialog("GENPLAN_RNG");
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
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
                                {
                                    this.pmCancelData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            case WsToolBar.Close:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
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

        private string pmUpdPRStep(string inWOrderH, string inRefNo, string inPRStep)
        {
            string strRetStep = inPRStep;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            string strMessage = "";
            string strRowID = inWOrderH;
            string strRefNo = inRefNo;
            string strPRStep = inPRStep;
            switch (strPRStep)
            {
                case "":
                    if (this.pmCheckHasPR(strRowID, strRefNo, ref strErrorMsg))
                    {
                        strMessage = UIBase.GetAppUIText(new string[] { "Step P/R ไม่ถูกต้องต้องการปรับปรุงให้ถูกต้องหรือไม่ ?", "Invaild PR Step!\r\nDo you want to update P/R Step ?" });
                        if (MessageBox.Show(strMessage, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objSQLHelper.SetPara(new object[] { strRowID });
                            objSQLHelper.SQLExec("update " + this.mstrRefTable + " set CPRSTEP = 'P' where cRowID = ? ", ref strErrorMsg);
                            objSQLHelper.SQLExec("update MFWORDERIT_PD set CPRSTEP = 'P' where CWORDERH = ?", ref strErrorMsg);
                            strRetStep ="PR";
                        }
                    }
                    else
                    {
                        //กรณีไม่เอกสาร PR ให้ถามว่าจะ Close Gen PR ?
                        strMessage = UIBase.GetAppUIText(new string[] { "ต้องการที่จะปิดการ Gen เอกสาร P/R หรือไม่ ?", "Do you want to Close Gen P/R Step ?" });
                        if (MessageBox.Show(strMessage, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objSQLHelper.SetPara(new object[] { strRowID });
                            objSQLHelper.SQLExec("update " + this.mstrRefTable + " set CPRSTEP = 'L' where cRowID = ? ", ref strErrorMsg);
                            objSQLHelper.SQLExec("update MFWORDERIT_PD set CPRSTEP = 'L' where CWORDERH = ?", ref strErrorMsg);
                            strRetStep ="X";
                        }
                    }
                    break;
                case "PR":
                    if (!this.pmCheckHasPR(strRowID, strRefNo, ref strErrorMsg))
                    {
                        strMessage = UIBase.GetAppUIText(new string[] { "Step P/R ไม่ถูกต้องต้องการปรับปรุงให้ถูกต้องหรือไม่ ?", "Invaild PR Step!\r\nDo you want to update P/R Step ?" });
                        if (MessageBox.Show(strMessage, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objSQLHelper.SetPara(new object[] { strRowID });
                            objSQLHelper.SQLExec("delete from REFDOC where CCHILDTYPE = 'MO' and CCHILDH = ? and CMASTERTYP = 'PR' ", ref strErrorMsg);

                            objSQLHelper.SetPara(new object[] { strRowID });
                            objSQLHelper.SQLExec("update " + this.mstrRefTable + " set CPRSTEP = ' ' where cRowID = ? ", ref strErrorMsg);
                            objSQLHelper.SQLExec("update MFWORDERIT_PD set CPRSTEP = ' ' where CWORDERH = ?", ref strErrorMsg);
                            strRetStep ="";
                        }
                    }
                    break;
                case "X":
                    if (this.pmCheckHasPR(strRowID, strRefNo, ref strErrorMsg))
                    {
                        objSQLHelper.SetPara(new object[] { strRowID });
                        objSQLHelper.SQLExec("update " + this.mstrRefTable + " set CPRSTEP = 'P' where cRowID = ? ", ref strErrorMsg);
                        strRetStep ="PR";
                    }
                    else
                    {
                        //กรณีไม่เอกสาร PR ให้ถามว่าจะ Close Gen PR ?
                        strMessage = UIBase.GetAppUIText(new string[] { "ต้องการที่จะคืนสถานะ Gen เอกสาร P/R หรือไม่ ?", "Do you want to Un-Close Gen P/R Step ?" });
                        if (MessageBox.Show(strMessage, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objSQLHelper.SetPara(new object[] { strRowID });
                            objSQLHelper.SQLExec("update " + this.mstrRefTable + " set CPRSTEP = ' ' where cRowID = ? ", ref strErrorMsg);
                            objSQLHelper.SQLExec("update MFWORDERIT_PD set CPRSTEP = ' ' where CWORDERH = ?", ref strErrorMsg);
                            strRetStep ="";
                        }
                    }
                    break;
            }
            return strRetStep;
        }

        private bool pmCheckHasPR(string inRowID, string inRefNo, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = false;

            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed1", "REFDOC", "select * from REFDOC where CCHILDTYPE = 'MO' and CCHILDH = ? and CMASTERTYP = 'PR' ", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QHasUsed1"].Rows[0];
                objSQLHelper2.SetPara(new object[] { dtrRefDoc["cMasterH"].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QHasUsed", "ORDERH", "select * from ORDERH where fcSkid = ? and fcStat <> 'C' ", ref strErrorMsg))
                {
                    ioErrorMsg = UIBase.GetAppUIText(new string[] { "มีการสร้างไปเป็นเอกสาร PR แล้ว !", "Document has generated to PR Document !" });
                    bllHasUsed = true;
                }
                else
                {
                    ioErrorMsg = "";
                }

            }

            return bllHasUsed;
        }

        private bool pmCheckHasWR(string inRowID, string inRefNo, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = false;

            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed1", "REFDOC", "select * from REFDOC where CCHILDTYPE = 'MO' and CCHILDH = ? and CMASTERTYP = 'WR' ", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QHasUsed1"].Rows[0];
                objSQLHelper2.SetPara(new object[] { dtrRefDoc["cMasterH"].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QHasUsed", "ORDERH", "select * from GLREF where fcSkid = ? and fcStat <> 'C' ", ref strErrorMsg))
                {
                    ioErrorMsg = UIBase.GetAppUIText(new string[] { "มีการสร้างไปเป็นเอกสาร WR แล้ว !", "Document has generated to WR Document !" });
                    bllHasUsed = true;
                }
                else
                {
                    ioErrorMsg = "";
                }

            }

            return bllHasUsed;
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

                string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_MO\\");
                dlg.LoadRPT(Application.StartupPath + "\\RPT\\FORM_MO\\");

                //int intLen1 = (Application.StartupPath + "\\RPT\\FORM_MO\\").Length;
                //dlg.cmbPForm.Properties.Items.Clear();
                //for (int i = 0; i < strADir.Length; i++)
                //{
                //    string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
                //    dlg.cmbPForm.Properties.Items.Add(strFormName);
                //}
                //if (dlg.cmbPForm.Properties.Items.Count > 0)
                //{
                //    dlg.cmbPForm.SelectedIndex = 0;
                //}

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    //App.mSave_hWnd = App.GetForegroundWindow();
                    string strRPTFileName = "";
                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);
                }
            }
        }

        Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new Report.LocalDataSet.FORM2PRINT();

        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";
            string strSQLStrOrderI = "select * from " + this.mstrITable2 + " where cWOrderH = ? and cIOType = 'O' order by cSeq";
            string strSQLStrOrderI_OP = "select * from " + this.mstrITable + " where cWOrderH = ? order by cSeq";

            dtsPrintPreview.PFORM_MO.Rows.Clear();

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

                    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                    {

                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            DataRow dtrPrnData = dtsPrintPreview.PFORM_MO.NewRow();
                            this.pmPrintMO_Head(dtrOrderH, dtrPrnData);
                            dtsPrintPreview.PFORM_MO.Rows.Add(dtrPrnData);

                            #region "Print RM Items"
                            string strProdType = "";
                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcType, fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strProdType = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcType"].ToString().TrimEnd();
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

                            dtrPrnData["MO_ITEM_TYPE"] = "2_RM";                            
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

                            //dtrWOrderI["nRefQty"] = Convert.ToDecimal(inTemPd["nRefQty"]);

                            dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);

                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);

                            dtrPrnData["RM_PROCURE"] = dtrOrderI["CPROCURE"].ToString();
                            dtrPrnData["RM_REFQTY"] = Convert.ToDecimal(dtrOrderI[QMFWOrderHDInfo.Field.RefQty]);

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtrPrnData["RM_LOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["RM_PRODTYPE"] = strProdType;
                            pobjSQLUtil2.SetPara(new object[] { this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QDocChain01", "DOCCHAIN", "select * from DOCCHAIN where cMasterTyp = ? and cMasterH = ? and cMasterI = ?", ref strErrorMsg))
                            {
                                DataRow dtrDocChain01 = this.dtsDataEnv.Tables["QDocChain01"].Rows[0];
                                pobjSQLUtil2.SetPara(new object[] { dtrDocChain01["cChildH"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QChildMO", "MFWORDERHD", "select MFGBOOK.CCODE as QcBook, MFWORDERHD.CCODE,MFWORDERHD.CREFNO,MFWORDERHD.DDATE from MFWORDERHD left join MFGBOOK on MFGBOOK.CROWID = MFWORDERHD.CMFGBOOK where MFWORDERHD.CROWID = ?", ref strErrorMsg))
                                {
                                    DataRow dtrChildMO = this.dtsDataEnv.Tables["QChildMO"].Rows[0];
                                    dtrPrnData["RM_SUBMO_CODE"] = "MO" + dtrChildMO["QcBook"].ToString().TrimEnd() + "/" + dtrChildMO["cCode"].ToString().TrimEnd();
                                    dtrPrnData["RM_SUBMO_REFNO"] = dtrChildMO["cRefNo"].ToString().TrimEnd();
                                    dtrPrnData["RM_SUBMO_DATE"] = Convert.ToDateTime(dtrChildMO["dDate"]).ToString("dd/MM/yy");
                                }
                            }

                            #endregion

                        }

                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI_OP, ref strErrorMsg))
                    {

                        #region "Print OP Item"
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            DataRow dtrPrnData = dtsPrintPreview.PFORM_MO.NewRow();
                            this.pmPrintMO_Head(dtrOrderH, dtrPrnData);
                            dtsPrintPreview.PFORM_MO.Rows.Add(dtrPrnData);

                            #region "OP"

                            dtrPrnData["MO_ITEM_TYPE"] = "1_OP";
                            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString().TrimEnd();
                            dtrPrnData["OP_OPSEQ"] = dtrOrderI["cOPSeq"].ToString().TrimEnd();
                            dtrPrnData["OP_NEXTOP"] = dtrOrderI["cNextOP"].ToString().TrimEnd();

                            pobjSQLUtil2.SetPara(new object[] { dtrOrderI["cMOPR"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRes", QMFStdOprInfo.TableName, "select * from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                            {
                                dtrPrnData["OP_OPR_CODE"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFStdOprInfo.Field.Code].ToString().TrimEnd();
                                dtrPrnData["OP_OPR_NAME"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFStdOprInfo.Field.Name].ToString().TrimEnd();
                            }

                            pobjSQLUtil2.SetPara(new object[] { dtrOrderI["cWkCtrH"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRes", QMFWorkCenterInfo.TableName, "select * from " + QMFWorkCenterInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                            {
                                dtrPrnData["OP_WC_CODE"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFWorkCenterInfo.Field.Code].ToString().TrimEnd();
                                dtrPrnData["OP_WC_NAME"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFWorkCenterInfo.Field.Name].ToString().TrimEnd();
                            }

                            pobjSQLUtil2.SetPara(new object[] { dtrOrderI["cResource"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRes", QMFResourceInfo.TableName, "select * from " + QMFResourceInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                            {
                                dtrPrnData["OP_TOOL_CODE"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Code].ToString().TrimEnd();
                                dtrPrnData["OP_TOOL_NAME"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Name].ToString().TrimEnd();
                            }

                            dtrPrnData["OP_REMARK1"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK2"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK3"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK4"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK5"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK6"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK7"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK8"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK9"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_REMARK10"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());

                            dtrPrnData["OP_QCREMARK1"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark1"]) ? "" : dtrOrderI["cQcReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK2"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark2"]) ? "" : dtrOrderI["cQcReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK3"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark3"]) ? "" : dtrOrderI["cQcReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK4"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark4"]) ? "" : dtrOrderI["cQcReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK5"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark5"]) ? "" : dtrOrderI["cQcReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK6"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark6"]) ? "" : dtrOrderI["cQcReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK7"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark7"]) ? "" : dtrOrderI["cQcReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK8"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark8"]) ? "" : dtrOrderI["cQcReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK9"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark9"]) ? "" : dtrOrderI["cQcReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["OP_QCREMARK10"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cQcReMark10"]) ? "" : dtrOrderI["cQcReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());

                            //Print OP COST
                            decimal intHour = 0; decimal intMin = 0; decimal intSec = 0;
                            decimal decTimeFactor = 1;
                            decimal decOPTime = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Queue])
                                + Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_SetUp])
                                + (Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor)
                                + Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Tear])
                                + Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Wait])
                                + Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Move]);

                            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Queue]), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_QUEUE"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_QUEUE_2"] = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Queue]);

                            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_SetUp]), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_SETUP"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_SETUP_2"] = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_SetUp]);

                            AppUtil.CommonHelper.ConvertSecToHMS((Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_PROCESS"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_PROCESS_2"] = (Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor);

                            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Tear]), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_TEARDOWN"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_TEARDOWN_2"] = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Tear]);

                            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Wait]), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_WAIT"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_WAIT_2"] = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Wait]);

                            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_MOVE"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_MOVE_2"] = Convert.ToDecimal(dtrOrderI[QMFBOMInfo.Field.LeadTime_Move]);

                            AppUtil.CommonHelper.ConvertSecToHMS(decOPTime, out intHour, out intMin, out intSec);
                            dtrPrnData["OP_TIME_TOTAL"] = this.pmConvertTime2Text(intHour, intMin, intSec);
                            dtrPrnData["OP_TIME_TOTAL2"] = decOPTime;

                            //TODO COST ITEM
                            //Get OP Cost from COSTLINE Table
                            QCostLineInfo oCostLine = new QCostLineInfo();

                            #region "Un Used"
                            //Load Cost by OP
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "O");

                            //dtrPrnData["OP_COST_O_FIX"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_O_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            //dtrPrnData["OP_COST_O_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            //dtrPrnData["OP_COST_O_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            //dtrPrnData["OP_COST_O_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            //dtrPrnData["OP_COST_O_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            //dtrPrnData["OP_COST_O_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_O_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_O_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_O_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_O_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            ////ค่าตามที่คีย์ใน BOM
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "O", true);
                            //dtrPrnData["OP_COST_O_FIX1"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_O_VM11"] = oCostLine.Cost_Var_ManHour1;
                            //dtrPrnData["OP_COST_O_VM21"] = oCostLine.Cost_Var_ManHour2;
                            //dtrPrnData["OP_COST_O_VM31"] = oCostLine.Cost_Var_ManHour3;
                            //dtrPrnData["OP_COST_O_VM41"] = oCostLine.Cost_Var_ManHour4;
                            //dtrPrnData["OP_COST_O_VM51"] = oCostLine.Cost_Var_ManHour5;
                            //dtrPrnData["OP_COST_O_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_O_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_O_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_O_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_O_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            //dtrPrnData["OP_COST_O_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            //dtrPrnData["OP_COST_O_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            //dtrPrnData["OP_COST_O_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            //dtrPrnData["OP_COST_O_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            //dtrPrnData["OP_COST_O_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;

                            ////Load Cost by WorkCenter
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "W");
                            //dtrPrnData["OP_COST_W_FIX"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_W_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            //dtrPrnData["OP_COST_W_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            //dtrPrnData["OP_COST_W_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            //dtrPrnData["OP_COST_W_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            //dtrPrnData["OP_COST_W_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            //dtrPrnData["OP_COST_W_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_W_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_W_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_W_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_W_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            ////ค่าตามที่คีย์ใน BOM
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "W", true);
                            //dtrPrnData["OP_COST_W_FIX1"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_W_VM11"] = oCostLine.Cost_Var_ManHour1;
                            //dtrPrnData["OP_COST_W_VM21"] = oCostLine.Cost_Var_ManHour2;
                            //dtrPrnData["OP_COST_W_VM31"] = oCostLine.Cost_Var_ManHour3;
                            //dtrPrnData["OP_COST_W_VM41"] = oCostLine.Cost_Var_ManHour4;
                            //dtrPrnData["OP_COST_W_VM51"] = oCostLine.Cost_Var_ManHour5;
                            //dtrPrnData["OP_COST_W_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_W_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_W_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_W_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_W_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            //dtrPrnData["OP_COST_W_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            //dtrPrnData["OP_COST_W_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            //dtrPrnData["OP_COST_W_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            //dtrPrnData["OP_COST_W_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            //dtrPrnData["OP_COST_W_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;


                            ////Load Cost by Tool (Resource)
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "T");
                            //dtrPrnData["OP_COST_T_FIX"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_T_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            //dtrPrnData["OP_COST_T_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            //dtrPrnData["OP_COST_T_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            //dtrPrnData["OP_COST_T_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            //dtrPrnData["OP_COST_T_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            //dtrPrnData["OP_COST_T_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_T_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_T_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_T_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_T_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            ////ค่าตามที่คีย์ใน BOM
                            //MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "T", true);
                            //dtrPrnData["OP_COST_T_FIX1"] = oCostLine.Cost_Fix;
                            //dtrPrnData["OP_COST_T_VM11"] = oCostLine.Cost_Var_ManHour1;
                            //dtrPrnData["OP_COST_T_VM21"] = oCostLine.Cost_Var_ManHour2;
                            //dtrPrnData["OP_COST_T_VM31"] = oCostLine.Cost_Var_ManHour3;
                            //dtrPrnData["OP_COST_T_VM41"] = oCostLine.Cost_Var_ManHour4;
                            //dtrPrnData["OP_COST_T_VM51"] = oCostLine.Cost_Var_ManHour5;
                            //dtrPrnData["OP_COST_T_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            //dtrPrnData["OP_COST_T_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            //dtrPrnData["OP_COST_T_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            //dtrPrnData["OP_COST_T_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            //dtrPrnData["OP_COST_T_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            //dtrPrnData["OP_COST_T_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            //dtrPrnData["OP_COST_T_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            //dtrPrnData["OP_COST_T_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            //dtrPrnData["OP_COST_T_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            //dtrPrnData["OP_COST_T_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;

                            #endregion

                            #endregion

                        }
                        #endregion
                    }

                }
                this.pmPreviewReport(dtsPrintPreview, inRPTFileName, false);
                //dlg.WaitClear();
            }
        }

        private void pmPrintMO_Head(DataRow inSource, DataRow inDest)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrWOrderH = inSource;
            DataRow dtrPrnData = inDest;
            //dtsPrintPreview.PFORM_MO.Rows.Add(dtrPrnData);

            string strRefQcBook = "";
            string strRefQnBook = "";
            string strRefCode = "";
            string strRefRefNo = "";
            string strRefDate = "";
            //this.pmLoadRefToCode(this.mstrRefType, dtrWOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);

            string strRefQcBook2 = "";
            string strRefQnBook2 = "";
            string strRefCode2 = "";
            string strRefRefNo2 = "";
            string strRefDate2 = "";
            //this.pmLoadRefToCode2(this.mstrRefType, dtrWOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

            string strQcBook = "";
            string strQnBook = "";
            pobjSQLUtil2.SetPara(new object[1] { dtrWOrderH["cMfgBook"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBook", "MFGBOOK", "select * from MfgBook where cRowID = ?", ref strErrorMsg))
            {
                strQcBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cCode"].ToString().TrimEnd();
                strQnBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cName"].ToString().TrimEnd();
            }

            string strQcSect = "";
            string strQnSect = "";
            string strQcDept = "";
            string strQnDept = "";
            pobjSQLUtil.SetPara(new object[1] { dtrWOrderH["cSect"].ToString() });
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
            pobjSQLUtil.SetPara(new object[1] { dtrWOrderH["cMfgProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName from PROD where fcSkid = ?", ref strErrorMsg))
            {
                strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            decimal decMfgQty = Convert.ToDecimal(dtrWOrderH["nMfgQty"]);

            string strStartDate = "";
            string strDueDate = "";

            DateTime dttStartDate = DateTime.MinValue;
            DateTime dttDueDate = DateTime.MinValue;

            if (!Convert.IsDBNull(dtrWOrderH[QMFWOrderHDInfo.Field.StartDate]))
            {
                DateTime dttDate = Convert.ToDateTime(dtrWOrderH[QMFWOrderHDInfo.Field.StartDate]).Date;
                strStartDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                dttStartDate = dttDate;
            }

            if (!Convert.IsDBNull(dtrWOrderH[QMFWOrderHDInfo.Field.DueDate]))
            {
                DateTime dttDate = Convert.ToDateTime(dtrWOrderH[QMFWOrderHDInfo.Field.DueDate]).Date;
                strDueDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                dttDueDate = dttDate;
            }

            string strQcBOM = "";
            string strQnBOM = "";
            string strBOM_AttachFile = "";
            string strBOM_Remark1 = "";
            string strBOM_Remark2 = "";
            string strBOM_Remark3 = "";
            string strBOM_Remark4 = "";
            string strBOM_Remark5 = "";
            string strBOM_Remark6 = "";
            string strBOM_Remark7 = "";
            string strBOM_Remark8 = "";
            string strBOM_Remark9 = "";
            string strBOM_Remark10 = "";

            string strBOM = dtrWOrderH[QMFWOrderHDInfo.Field.BOMID].ToString();
            pobjSQLUtil2.SetPara(new object[1] { strBOM });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select * from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                strQcBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                strQnBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                strBOM_AttachFile = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cFileAttn"].ToString();

                strBOM_Remark1 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark1].ToString();
                strBOM_Remark2 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark2].ToString();
                strBOM_Remark3 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark3].ToString();
                strBOM_Remark4 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark4].ToString();
                strBOM_Remark5 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark5].ToString();
                strBOM_Remark6 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark6].ToString();
                strBOM_Remark7 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark7].ToString();
                strBOM_Remark8 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark8].ToString();
                strBOM_Remark9 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark9].ToString();
                strBOM_Remark10 = this.dtsDataEnv.Tables["QPdStH"].Rows[0][QMFBOMInfo.Field.Remark10].ToString();
            
            }

            string strQcJob = "";
            string strQnJob = "";
            string strQcProj = "";
            string strQnProj = "";

            pobjSQLUtil.SetPara(new object[1] { dtrWOrderH["cJob"].ToString() });
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
            pobjSQLUtil.SetPara(new object[1] { dtrWOrderH["cCoor"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                strQcCoor = dtrCoor["fcCode"].ToString().TrimEnd();
                strQnCoor = dtrCoor["fcSName"].ToString().TrimEnd();
            }

            string strRemark = (Convert.IsDBNull(dtrWOrderH["cMemData"]) ? "" : dtrWOrderH["cMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrWOrderH["cMemData2"]) ? "" : dtrWOrderH["cMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrWOrderH["cMemData3"]) ? "" : dtrWOrderH["cMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrWOrderH["cMemData4"]) ? "" : dtrWOrderH["cMemData4"].ToString().TrimEnd());
            if (dtrWOrderH["cMemData5"] != null)
                strRemark += (Convert.IsDBNull(dtrWOrderH["cMemData5"]) ? "" : dtrWOrderH["cMemData5"].ToString().TrimEnd());

            string strRemark1 = dtrWOrderH["cReMark1"].ToString();
            string strRemark2 = dtrWOrderH["cReMark2"].ToString();
            string strRemark3 = dtrWOrderH["cReMark3"].ToString();
            string strRemark4 = dtrWOrderH["cReMark4"].ToString();
            string strRemark5 = dtrWOrderH["cReMark5"].ToString();
            string strRemark6 = dtrWOrderH["cReMark6"].ToString();
            string strRemark7 = dtrWOrderH["cReMark7"].ToString();
            string strRemark8 = dtrWOrderH["cReMark8"].ToString();
            string strRemark9 = dtrWOrderH["cReMark9"].ToString();
            string strRemark10 = dtrWOrderH["cReMark10"].ToString();

            dtrPrnData["CREFTYPE"] = dtrWOrderH["cRefType"].ToString();
            dtrPrnData["CQCBOOK"] = strQcBook;
            dtrPrnData["CQNBOOK"] = strQnBook;
            dtrPrnData["CCODE"] = dtrWOrderH["cCode"].ToString();
            dtrPrnData["CREFNO"] = dtrWOrderH["cRefNo"].ToString();
            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrWOrderH["dDate"]);

            dtrPrnData["CQCCOOR"] = strQcCoor;
            dtrPrnData["CQNCOOR"] = strQnCoor;
            dtrPrnData["CQCBOM"] = strQcBOM;
            dtrPrnData["CQNBOM"] = strQnBOM;

            //if (!Convert.IsDBNull(dtrWOrderH["dApprove"]))
            //{
            //	dtrPrnData["CAPPROVEDATE"] = Convert.ToDateTime(dtrWOrderH["dApprove"]).ToString("dd/MM/yy");
            //}

            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
            dtrPrnData["CREFDOC_REFTYPE"] = "";
            dtrPrnData["CREFDOC_CODE"] = strRefCode;
            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
            dtrPrnData["CREFDOC_DATE"] = strRefDate;

            dtrPrnData["CSTARTDATE"] = strStartDate;
            dtrPrnData["CFINISHDATE"] = strDueDate;

            dtrPrnData["DSTARTDATE"] = dttStartDate;
            dtrPrnData["DFINISHDATE"] = dttDueDate;

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
            //dtrPrnData["CQCWHOUSE"] = strQcWHouse;
            //dtrPrnData["CQNWHOUSE"] = strQnWHouse;

            //this.txtTotMfgQty.Value = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.Qty]);
            //this.txtMfgQty.Value = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty]);
            dtrPrnData["NMOQTY"] = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty]);
            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.Qty]);
            dtrPrnData["CSCRAP"] = dtrWOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();

            this.mdecPdStOutput = (Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.BOMOutputQty]) != 0 ? Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.BOMOutputQty]) : 1);

            dtrPrnData["NSTOCKQTY"] = this.pmGetStockBal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgProdID].ToString(), this.mstrBook_WHouse_FG);
            dtrPrnData["NFGRATIO1"] = (this.mdecPdStOutput != 0 ? this.txtTotMfgQty.Value / this.mdecPdStOutput : 0);
            dtrPrnData["NFGRATIO2"] = (Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);

            //this.txtRatio1.Value = (this.mdecPdStOutput != 0 ? this.txtTotMfgQty.Value / this.mdecPdStOutput : 0);
            //this.txtRatio2.Value = (Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);

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

            dtrPrnData["CFG_ATTACHFILE"] = strBOM_AttachFile;
            dtrPrnData["BOM_REMARK1"] = strBOM_Remark1;
            dtrPrnData["BOM_REMARK2"] = strBOM_Remark2;
            dtrPrnData["BOM_REMARK3"] = strBOM_Remark3;
            dtrPrnData["BOM_REMARK4"] = strBOM_Remark4;
            dtrPrnData["BOM_REMARK5"] = strBOM_Remark5;
            dtrPrnData["BOM_REMARK6"] = strBOM_Remark6;
            dtrPrnData["BOM_REMARK7"] = strBOM_Remark7;
            dtrPrnData["BOM_REMARK8"] = strBOM_Remark8;
            dtrPrnData["BOM_REMARK9"] = strBOM_Remark9;
            dtrPrnData["BOM_REMARK10"] = strBOM_Remark10;

        }

        private void pmLoadWkCtrByOP(string inWOrderH, string inOPSeq, ref string ioQcWkCtrH, ref string ioQnWkCtrH)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select ";
            strSQLStr += " WkCtrH.cCode as QcWkCtrH , WkCtrH.cName as QnWkCtrH ";
            strSQLStr += " from " + QMFWOrderIT_OPInfo.TableName + " WOrderOP ";
            strSQLStr += " left join " + QMFWorkCenterInfo.TableName + " WkCtrH on WkCtrH.cRowID = WOrderOP.cWkCtrH ";
            strSQLStr += " where WOrderOP.cWOrderH = ? ";
            strSQLStr += " and WOrderOP.cOPSeq = ? ";

            pobjSQLUtil.SetPara(new object[2] { inWOrderH, inOPSeq });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWkCtr", "WKCTRH", strSQLStr, ref strErrorMsg))
            {
                ioQcWkCtrH = this.dtsDataEnv.Tables["QWkCtr"].Rows[0]["QcWkCtrH"].ToString();
                ioQnWkCtrH = this.dtsDataEnv.Tables["QWkCtr"].Rows[0]["QnWkCtrH"].ToString();
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

        private void pmGenIssue(bool inStockOnly)
        {

            string strErrorMsg = "";
            string strRowID = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            strRowID = dtrBrow["cRowID"].ToString();

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrLoadHead = null;
            
            Common.MRP.cGenIssue oGenIssue = new BeSmartMRP.Transaction.Common.MRP.cGenIssue(strRowID, this.mstrGenPR_Branch, this.mstrPlant, this.mstrGenPR_Book, this.mdttGenPR_Date);
            oGenIssue.GenIssue(inStockOnly);

        }

        private void pmGenPR(string inRowID)
        {

            string strErrorMsg = "";
            string strRowID = inRowID;
            if (inRowID == string.Empty)
            {
                DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                if (this.gridView1.RowCount == 0 || dtrBrow == null)
                    return;

                strRowID = dtrBrow["cRowID"].ToString();
            }

            //Common.MRP.cGenPR oGenPR = new BeSmartMRP.Transaction.Common.MRP.cGenPR(strRowID, this.mstrGenPR_Branch, this.mstrGenPR_Book, this.mstrGenPR_Coor, this.mdttGenPR_Date);

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrLoadHead = null;
            objSQLHelper.SetPara(new object[] { strRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                dtrLoadHead = this.dtsDataEnv.Tables["QLoadPR"].Rows[0];
                Common.MRP.cGenPR oGenPR = new BeSmartMRP.Transaction.Common.MRP.cGenPR(strRowID, this.mstrGenPR_Branch, this.mstrPlant, this.mstrGenPR_Book, this.mstrGenPR_Coor, dtrLoadHead["cSect"].ToString(), dtrLoadHead["cJob"].ToString(), this.mdttGenPR_Date);
                oGenPR.GenPR();
            }

            //WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //DataRow dtrLoadHead = null;
            //objSQLHelper.SetPara(new object[] { strRowID });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            //{
            //    dtrLoadHead = this.dtsDataEnv.Tables["QLoadPR"].Rows[0];
            //}


            //Common.frmPreviewOrder ofrm = new BeSmartMRP.Transaction.Common.frmPreviewOrder("PR");
            //ofrm.BindData(this.mstrBranch, this.mstrGenPR_Book, this.mstrGenPR_Coor, dtrLoadHead[QMFWOrderHDInfo.Field.SectID].ToString(), dtrLoadHead[QMFWOrderHDInfo.Field.JobID].ToString(), dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString(), this.mdttGenPR_Date, new DataTable());
            //ofrm.ShowDialog();
            ////if (!this.pmCheckHasPR(this.mstrEditRowID, dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString(), ref strErrorMsg))
            //if (true)
            //{
            //    this.pmGen1PRDoc(strRowID, dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString().TrimEnd(), ref strErrorMsg);
            //    MessageBox.Show("Complete !");
            //}
            //else
            //{
            //    if (strErrorMsg != "")
            //        MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

        }

        private void pmGenRangePR()
        {

            string strErrorMsg = "";
            string strRowID = "";

            Common.MRP.cGenRangePR oGenPR = new BeSmartMRP.Transaction.Common.MRP.cGenRangePR(this.mstrGenPR_Branch, this.mstrPlant, this.mstrGenPR_Book, this.mstrGenPR_Coor, this.mdttGenPR_Date);

            oGenPR.GenPR_MOBook = this.mstrBook;
            oGenPR.GenPR_OrderBy = this.mstrGenRngPR_OrderBy;
            oGenPR.GenPR_BegCode = this.mstrGenRngPR_BegCode;
            oGenPR.GenPR_EndCode = this.mstrGenRngPR_EndCode;
            oGenPR.GenPR_BegDate = this.mdttGenRngPR_BegDate;
            oGenPR.GenPR_EndDate = this.mdttGenRngPR_EndDate;

            oGenPR.GenPR();

        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = dtrBrow[QMFWOrderHDInfo.Field.Code].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFWOrderHDInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFWOrderHDInfo.Field.Code].ToString(), "", ref strErrorMsg))
                    {
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
                        //if (this.pmGetRowID(this.mstrEditRowID) > 0)
                        //{
                        //    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
                        //}
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

        private void pmCancelData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = dtrBrow[QMFWOrderHDInfo.Field.Code].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFWOrderHDInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการ Cancel ข้อมูล", "Do you want to Cancel " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmCancelRow(this.mstrEditRowID, dtrBrow[QMFWOrderHDInfo.Field.Code].ToString(), "", ref strErrorMsg))
                    {
                        //this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
                        dtrBrow[QMFWOrderHDInfo.Field.Stat] = "C";
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

            if (inChkRow[QMFWOrderHDInfo.Field.MStep].ToString() == SysDef.gc_REF_STEP_PAY)
            {
                this.mstrCanEditMsg = UIBase.GetAppUIText(new string[] { "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากปิดการผลิตแล้ว", "This document has refer to #MC. Can not " + strMsg1 });
                bllResult = false;
            }
            if (inChkRow[QMFWOrderHDInfo.Field.OPStep].ToString() !=SysDef.gc_REF_OPSTEP_CREATE)
            {
                this.mstrCanEditMsg = UIBase.GetAppUIText(new string[] { "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากถูกค้นไปทำเอกสาร Route Card แล้ว", "This document has refer to ROUTE CARD. Can not " + strMsg1 });
                bllResult = false;
            }
            
            if (inEditType == DocEditType.Edit && inChkRow[QMFWOrderHDInfo.Field.Stat].ToString() == "C")
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

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = false;
            bool bllCanEdit = false;
            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                DataRow dtrChkRow = this.dtsDataEnv.Tables["QHasUsed"].Rows[0];

                //bllCanEdit = this.pmCanEdit(dtrChkRow, DocEditType.Delete, false);
                if (this.pmCanEdit(dtrChkRow, DocEditType.Delete, false))
                {
                    bllCanEdit = this.pmCanDeleteChildWO(inRowID, ref ioErrorMsg);
                    bllCanEdit = bllCanEdit && !this.pmCheckHasPR(inRowID, dtrChkRow["cRefNo"].ToString(), ref ioErrorMsg);
                    bllCanEdit = bllCanEdit && !this.pmCheckHasWR(inRowID, dtrChkRow["cRefNo"].ToString(), ref ioErrorMsg);
                }
                else
                {
                    ioErrorMsg = this.mstrCanEditMsg;
                }

            }
            bllHasUsed = !bllCanEdit;
            return bllHasUsed;
        }

        private bool pmCanDeleteChildWO(string inParent, ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from " + this.mstrRefTable + " where cParent = ? order by cCode", ref strErrorMsg))
            {
                foreach (DataRow dtrChk in this.dtsDataEnv.Tables["QRefDoc"].Rows)
                {
                    if (this.pmCanEdit(dtrChk, DocEditType.Delete, false) == false)
                    {
                        ioErrorMsg = this.mstrCanEditMsg;
                        bllResult = false;
                        break;
                    }
                }
            }
            return bllResult;
        }

        private bool pmDeleteChildWO(string inParent, ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            ///WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //pobjSQLUtil.SetPara(new object[] { inParent });
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from " + this.mstrRefTable + " where cParent = ? order by cCode", ref strErrorMsg))
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefDoc", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cParent = ? order by cCode", new object[1] { inParent }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrChk in this.dtsDataEnv.Tables["QRefDoc"].Rows)
                {
                    this.pmDeleteChildWO(dtrChk["cRowID"].ToString(), ref strErrorMsg);
                    this.pmDelete1ChildWO(dtrChk["cRowID"].ToString());
                }
            }

            return bllResult;
        }

        private void pmDelete1ChildWO(string inRowID)
        {
            string strErrorMsg = "";
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            // ลบ DocChain
            //pobjSQLUtil.SetPara(new object[1] { inRowID });
            //pobjSQLUtil.SQLExec("delete from DOCCHAIN where cChildH = ?", ref strErrorMsg);
            this.mSaveDBAgent.BatchSQLExec("delete from DOCCHAIN where cChildH = ? ", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

            // ลบ WOrderI
            //pobjSQLUtil.SetPara(new object[1] { inRowID });
            //pobjSQLUtil.SQLExec("delete from " + this.mstrITable2 + " where cWOrderH = ?", ref strErrorMsg);
            this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable2 + " where cWOrderH = ? ", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

            // ลบ WOrderOP
            //pobjSQLUtil.SetPara(new object[1] { inRowID });
            //pobjSQLUtil.SQLExec("delete from " + this.mstrITable + " where cWOrderH = ?", ref strErrorMsg);
            this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWOrderH = ? ", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

            //pobjSQLUtil.SetPara(new object[1] { inRowID });
            //pobjSQLUtil.SQLExec("delete from " + this.mstrHTable + " where cRowID = ?", ref strErrorMsg);
            this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrHTable + " where cRowID = ? ", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID2(this.mstrBrowViewAlias, "cRowID", inRowID));
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

                this.pmDeleteChildWO(this.mstrEditRowID, ref strErrorMsg);
                // ลบ DocChain
                this.mSaveDBAgent.BatchSQLExec("delete from DOCCHAIN where cChildH = ? ", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[] { this.mstrRefType, inRowID, this.mstrRefToRefType };
                this.mSaveDBAgent.BatchSQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWOrderH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable2 + " where cWOrderH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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

        private bool pmCancelRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
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

                //this.pmDeleteChildWO(this.mstrEditRowID, ref strErrorMsg);
                // ลบ DocChain
                this.mSaveDBAgent.BatchSQLExec("delete from DOCCHAIN where cChildH = ? ", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[] { this.mstrRefType, inRowID, this.mstrRefToRefType };
                this.mSaveDBAgent.BatchSQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrITable2 + " set CSTAT = 'C' where cWOrderH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set CSTAT = 'C' where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Cancel, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

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
                this.pmGenChildWO(this.mstrLevel);
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

        private bool pmGenChildWO(string inLevel)
        {

            this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemGenMulti2].Rows.Clear();

            bool bllResult = true;
            int intRowID = 0;
            this.mstrMasterWO = this.mstrEditRowID;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                intRowID++;
                if (dtrTemPd["cMfgBOMHD"].ToString().TrimEnd() != string.Empty
                    && Convert.ToDecimal(dtrTemPd["nQty"]) > 0
                    && dtrTemPd["cProcure"].ToString().TrimEnd() == SysDef.gc_PROCURE_TYPE_MANUFACURING)
                {

                    string strMsg = UIBase.GetAppUIText(new string[] { "วัตถุดิบ : (", "SEMI : (" }) + dtrTemPd["cQcProd"].ToString().TrimEnd() + ") " + dtrTemPd["cQnProd"].ToString() + UIBase.GetAppUIText(new string[] { " ต้องมีการสั่งผลิต\r\nจะสั่งสร้างเอกสาร #MO ย่อยเลยหรือไม่ ?", " require production\r\nDo you want to create SUB #MO ?" });
                    if (!this.pmHasChildWO(this.mstrEditRowID, dtrTemPd["cRowID"].ToString())
                        && MessageBox.Show(this, strMsg, "Application confirm message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string strAppErrorMsg = "";
                        this.mstrChildWORowID = "";
                        this.mstrBatchRun = "";
                        if (this.pmSaveMulti_Gen1ChildWO(this.mstrEditRowID, inLevel, this.txtQcCoor.Tag.ToString(), dtrTemPd["cMfgBOMHD"].ToString(), dtrTemPd["cProd"].ToString(), dtrTemPd["cQcProd"].ToString().TrimEnd(), Convert.ToDecimal(dtrTemPd["nQty"]), ref strAppErrorMsg)
                            && this.mstrChildWORowID.TrimEnd() != string.Empty)
                        {
                            this.pmUpdateDocChain(this.mstrEditRowID, dtrTemPd["cRowID"].ToString(), this.mstrChildWORowID, Convert.ToDecimal(dtrTemPd["nQty"]));
                            //this.dtsDataEnv.Tables[this.mstrTemGenFG].Rows.Clear();
                        }
                        else
                        {
                            MessageBox.Show(strAppErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows.Count > 0)
            {
                this.pmGenChildWO2(inLevel);
            }

            return bllResult;
        }

        private bool pmGenChildWO2(string inLevel)
        {
            bool bllResult = true;
            int intRowID = 0;

            //Copy Full Row for Loop
            this.dtsDataEnv.Tables[this.mstrTemGenMulti2].Rows.Clear();
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows)
            {
                DataRow dtrGen2 = this.dtsDataEnv.Tables[this.mstrTemGenMulti2].NewRow();
                DataSetHelper.CopyDataRow(dtrTemPd, ref dtrGen2);
                this.dtsDataEnv.Tables[this.mstrTemGenMulti2].Rows.Add(dtrGen2);
            }

            this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows.Clear();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemGenMulti2].Rows)
            {
                intRowID++;
                if (dtrTemPd["cMfgBOMHD"].ToString().TrimEnd() != string.Empty
                    && Convert.ToDecimal(dtrTemPd["nQty"]) > 0
                    && dtrTemPd["cProcure"].ToString().TrimEnd() == SysDef.gc_PROCURE_TYPE_MANUFACURING)
                {
                    string strMsg = UIBase.GetAppUIText(new string[] { "วัตถุดิบ : (", "SEMI : (" }) + dtrTemPd["cQcProd"].ToString().TrimEnd() + ") " + dtrTemPd["cQnProd"].ToString() + UIBase.GetAppUIText(new string[] { " ต้องมีการสั่งผลิต\r\nจะสั่งสร้างเอกสาร #MO ย่อยเลยหรือไม่ ?", " require production\r\nDo you want to create SUB #MO ?" });
                    if (!this.pmHasChildWO(dtrTemPd["cWOrderH"].ToString(), dtrTemPd["cRowID"].ToString())
                        && MessageBox.Show(this, strMsg, "Application confirm message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string strAppErrorMsg = "";
                        this.mstrChildWORowID = "";
                        this.mstrBatchRun = "";
                        if (this.pmSaveMulti_Gen1ChildWO(dtrTemPd["cWOrderH"].ToString(), inLevel, this.txtQcCoor.Tag.ToString(), dtrTemPd["cMfgBOMHD"].ToString(), dtrTemPd["cProd"].ToString(), dtrTemPd["cQcProd"].ToString().TrimEnd(), Convert.ToDecimal(dtrTemPd["nQty"]), ref strAppErrorMsg)
                            && this.mstrChildWORowID.TrimEnd() != string.Empty)
                        {
                            this.pmUpdateDocChain(dtrTemPd["cWOrderH"].ToString(), dtrTemPd["cRowID"].ToString(), this.mstrChildWORowID, Convert.ToDecimal(dtrTemPd["nQty"]));
                        }
                        else
                        {
                            MessageBox.Show(strAppErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

            if (this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows.Count > 0)
            {
                this.pmGenChildWO2(inLevel);
            }
            return bllResult;
        }

        private bool pmUpdateDocChain(string inMasterH, string inMasterI, string inChildH, decimal inQty)
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            bool bllIsNewRow_NoteCut = false;
            DataRow dtrNoteCut = null;
            string strRowID = "";
            string strNoteCut = "select * from DOCCHAIN where 0=1";
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "DOCCHAIN", "DOCCHAIN", strNoteCut, ref strErrorMsg);
            strRowID = App.mRunRowID("DOCCHAIN");
            bllIsNewRow_NoteCut = true;
            dtrNoteCut = this.dtsDataEnv.Tables["DOCCHAIN"].NewRow();

            dtrNoteCut["cRowID"] = strRowID;
            dtrNoteCut["cMastertyp"] = this.mstrRefType;
            dtrNoteCut["cMasterH"] = inMasterH;
            dtrNoteCut["cMasterI"] = inMasterI;
            dtrNoteCut["cCorp"] = App.ActiveCorp.RowID;
            dtrNoteCut["cBranch"] = this.mstrBranch;
            dtrNoteCut["cPlant"] = this.mstrPlant;
            dtrNoteCut["cChildType"] = this.mstrRefType;
            dtrNoteCut["cChildH"] = inChildH;
            dtrNoteCut["cChildI"] = "";
            dtrNoteCut["nQty"] = inQty;
            dtrNoteCut["nUmQty"] = 1;

            string strSQLUpdateStr_NoteCut = "";
            DataSetHelper.GenUpdateSQLString(dtrNoteCut, "CROWID", bllIsNewRow_NoteCut, ref strSQLUpdateStr_NoteCut, ref pAPara);
            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(strSQLUpdateStr_NoteCut, ref strErrorMsg);
            return true;
        }

        private bool pmHasChildWO(string inMasterH, string inMasterI)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, inMasterH, inMasterI });
            bllResult = pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkWO", "DOCCHAIN", "select * from DOCCHAIN where cMasterTyp = ? and cMasterH = ? and cMasterI = ?", ref strErrorMsg);
            return bllResult;
        }

        private bool pmSaveMulti_Gen1ChildWO(string inParent, string inLevel, string inCoor, string inBOMHD, string inProd, string inQcProd, decimal inQty, ref string ioErrorMsg)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inBOMHD });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select * from MFBOMHD where CROWID = ? ", ref strErrorMsg))
            {
                DataRow dtrPdStH = this.dtsDataEnv.Tables["QPdStH"].Rows[0];
                decimal decQtyFactor = inQty / (Convert.ToDecimal(dtrPdStH["nMfgQty"]) > 0 ? Convert.ToDecimal(dtrPdStH["nMfgQty"]) : 1);
                decimal decMfgQty = Convert.ToDecimal(dtrPdStH["nMfgQty"]);
                decimal decMfgUMQty = (Convert.ToDecimal(dtrPdStH["nMfgUMQty"]) > 0 ? Convert.ToDecimal(dtrPdStH["nMfgUMQty"]) : 1);
                bllResult = this.pmSaveMulti_SaveChildWO(inLevel, inParent, this.mstrBatchNo, inCoor, inProd, inQcProd, dtrPdStH["cRowID"].ToString(), dtrPdStH["cMfgUM"].ToString(), decMfgQty, inQty, decMfgUMQty, decQtyFactor, ref strErrorMsg);
                ioErrorMsg = strErrorMsg;
            }
            else
            {
                ioErrorMsg = "ไม่พบ BOM สำหรับผลิตสินค้ารายการนี้.\r\nโปรดตรวจสอบสูตรการผลิตที่เมนู BOM\\เพิ่ม/แก้ไขฐานข้อมูล BOM";
            }
            return bllResult;
        }

        private bool pmSaveMulti_SaveChildWO(string inLevel, string inParentWO, string inBatchNo, string inCoor, string inProd, string inQcProd, string inPdStH, string inMfgUM, decimal inBOMOutQty, decimal inQty, decimal inMfgUMQty, decimal inQtyFactor, ref string ioErrorMsg)
        {

            bool bllResult = true;
            bool bllIsNewRow = false;
            string strErrorMsg = "";
            string strRowID = "";
            string gcTemStr01 = "";

            DataRow dtrWOrderH = null;
            DataRow dtrCurrRow = null;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where 0=1", ref strErrorMsg);
            strRowID = App.mRunRowID(this.mstrHTable);

            this.mstrChildWORowID = strRowID;
            dtrWOrderH = this.dtsDataEnv.Tables[this.mstrHTable].NewRow();
            bllIsNewRow = true;

            int intLevel = Convert.ToInt32(inLevel) + 1;
            string strBatchNo = inBatchNo;
            string strCode = "";
            this.pmRunCode2(strBatchNo, intLevel.ToString("00"), ref strCode);
            string strRefNo = this.mstrRefType + this.mstrQcBook + "/" + strCode;

            object[] pAPara = null;

            dtrWOrderH[MapTable.ShareField.CreateBy] = App.FMAppUserID;
            dtrWOrderH[MapTable.ShareField.CreateDate] = pobjSQLUtil.GetDBServerDateTime();
            dtrWOrderH[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrWOrderH[MapTable.ShareField.LastUpdate] = pobjSQLUtil.GetDBServerDateTime();

            dtrWOrderH["cRowID"] = strRowID;
            dtrWOrderH["cCorp"] = App.ActiveCorp.RowID;
            dtrWOrderH["cPlant"] = this.mstrPlant;
            dtrWOrderH["cBranch"] = this.mstrBranch;
            dtrWOrderH["cStep"] = this.mstrStep;
            dtrWOrderH["cRefType"] = this.mstrRefType;
            dtrWOrderH["cRfType"] = this.mstrRfType;
            dtrWOrderH["cMfgBook"] = this.mstrBook;
            dtrWOrderH["cCode"] = strCode;
            dtrWOrderH["cRefNo"] = strRefNo;
            dtrWOrderH["dDate"] = this.txtDate.DateTime.Date;
            dtrWOrderH["dStart"] = this.txtStartDate.DateTime.Date;
            dtrWOrderH["dDueDate"] = this.txtDueDate.DateTime.Date;
            dtrWOrderH["cSect"] = this.txtQcSect.Tag.ToString();
            //dtrWOrderH["cDept"] = this.mstrDivision;
            dtrWOrderH["cJob"] = (this.txtQcJob.Tag != null ? this.txtQcJob.Tag.ToString() : "");
            //dtrWOrderH["cProj"] = this.mstrProj;
            dtrWOrderH["cCoor"] = inCoor;
            dtrWOrderH["cMfgProd"] = inProd;
            //dtrWOrderH["cPdStH"] = inPdStH;
            dtrWOrderH[QMFWOrderHDInfo.Field.BOMID] = inPdStH;
            
            dtrWOrderH[QMFWOrderHDInfo.Field.BOMOutputQty] = inBOMOutQty;
            dtrWOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1] = inQtyFactor;

            dtrWOrderH["cMfgUm"] = inMfgUM;
            dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty] = inQty;
            dtrWOrderH[QMFWOrderHDInfo.Field.Qty] = inQty;
            dtrWOrderH["nMUMQty"] = inMfgUMQty;

            dtrWOrderH["cBatchNo"] = strBatchNo;
            dtrWOrderH["cBatchRun"] = this.mstrBatchRun;
            dtrWOrderH["cLevel"] = intLevel.ToString("00");
            dtrWOrderH["cMaster"] = this.mstrMasterWO;
            dtrWOrderH["cParent"] = inParentWO;

            dtrWOrderH["cMemData"] = "";	//cMemData
            dtrWOrderH["cMemData2"] = "";	//cMemData
            dtrWOrderH["cMemData3"] = "";	//cMemData
            dtrWOrderH["cMemData4"] = "";	//cMemData
            dtrWOrderH["cMemData5"] = "";	//cMemData

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : " ");
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : null);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : null);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : null);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : null);

            dtrWOrderH["cMemData"] = gcTemStr02;
            if (dtrWOrderH["cMemData2"] != null)
            {
                dtrWOrderH["cMemData2"] = gcTemStr03;
                dtrWOrderH["cMemData3"] = gcTemStr04;
                dtrWOrderH["cMemData4"] = gcTemStr05;
            }

            if (dtrWOrderH["cMemData5"] != null)
            {
                dtrWOrderH["cMemData5"] = gcTemStr06;
            }

            switch (BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()))
            {
                case ApproveStep.Wait:
                    dtrWOrderH[QMFWOrderHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrWOrderH[QMFWOrderHDInfo.Field.ApproveBy] = "";
                    dtrWOrderH[QMFWOrderHDInfo.Field.ApproveDate] = Convert.DBNull;
                    break;
                case ApproveStep.Approve:
                    dtrWOrderH[QMFWOrderHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrWOrderH[QMFWOrderHDInfo.Field.ApproveBy] = App.AppUserID;
                    dtrWOrderH[QMFWOrderHDInfo.Field.ApproveDate] = this.txtDApprove.DateTime;
                    break;
            }

            //Prepare Data For GenWO
            this.pmSaveMulti_LoadPdStDetail(dtrWOrderH, inProd, inPdStH, inQtyFactor, inMfgUM, inMfgUMQty);

            this.pmSaveMulti_SaveWOrderI(dtrWOrderH, this.mstrTemGenFG, ref strErrorMsg);
            this.pmSaveMulti_SaveWOrderI(dtrWOrderH, this.mstrTemGenPd, ref strErrorMsg);
            this.pmSaveMulti_SaveWOrderOP(dtrWOrderH, this.mstrTemGenOP, ref strErrorMsg);

            //Update GLRef
            string strSQLUpdateStr = "";
            DataSetHelper.GenUpdateSQLString(dtrWOrderH, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
            pobjSQLUtil.SetPara(pAPara);
            bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr, ref strErrorMsg);

            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //	objDBConn.Close();
            //}

            //if (bllResult)
            //{
            //    dtrCurrRow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();

            //    dtrCurrRow["cRowID"] = strRowID;
            //    //dtrCurrRow["cStep"] = this.mstrStep;
            //    dtrCurrRow["cStep"] = (this.mstrStep == SysDef.gc_REF_STEP_CLOSED ? "CLOSED" : "");
            //    dtrCurrRow["cCode"] = strCode;
            //    dtrCurrRow["cRefNo"] = strRefNo;
            //    dtrCurrRow["dDate"] = this.txtDate.DateTime.Date;
            //    dtrCurrRow["QcProd"] = StringHelper.PadR(inQcProd, this.txtQcProd.Properties.MaxLength);
            //    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrCurrRow);
            //}

            return bllResult;
        }

        private void pmSaveMulti_LoadPdStDetail(DataRow inMOrderH, string inProd, string inPdStH, decimal inQtyFactor, string inMfgUM, decimal inMfgUMQty)
        {
            int intRecNo = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.pmClearTemToBlank2();

            this.pmReplTemFG2(inProd, inQtyFactor, inMfgUM, inMfgUMQty);

            pobjSQLUtil.SetPara(new object[] { inPdStH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStI", "PDSTI", "select * from MFBOMIT_PD where CBOMHD = ? order by cSeq", ref strErrorMsg))
            {

                decimal decRatio2 = (Convert.ToDecimal(inMOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(inMOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);
                
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables["QPdStI"].Rows)
                {
                    intRecNo++;


                    //decimal decQty = inQtyFactor * decRatio2;
                    decimal decQty = Convert.ToDecimal(dtrPdStI["nQty"]) * decRatio2;

                    decimal decScrapQty1 = BizRule.CalScrapQty2(dtrPdStI["cScrap"].ToString(), decQty, 0, 0);
                    decimal decScrapQty = 0;
                    if (decScrapQty1 != 0)
                    {
                        decScrapQty = BizRule.CalScrapQty2(dtrPdStI["cScrap"].ToString(), decQty, 0, 0) - decQty;
                    }

                    decimal decSaveQty = decQty;
                    switch (dtrPdStI["cRoundCtrl"].ToString())
                    {
                        case "1":
                            decQty = Math.Ceiling(decQty);
                            break;
                        case "2":
                            decQty = Math.Floor(decQty);
                            break;
                        default:
                            break;
                    }

                    //this.pmReplTemPdStI2(intRecNo, dtrPdStI, inQtyFactor);
                    this.pmReplTemPdStI2(intRecNo, dtrPdStI, decQty + decScrapQty);
                }
            }

            intRecNo = 0;
            pobjSQLUtil.SetPara(new object[] { inPdStH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStOP", "PDSTOP", "select * from MFBOMIT_STDOP where CBOMHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrPdStOP in this.dtsDataEnv.Tables["QPdStOP"].Rows)
                {
                    intRecNo++;
                    this.pmReplTemPdStOP2(intRecNo, dtrPdStOP);
                }
            }

        }

        private void pmReplTemFG2(string inProd, decimal inQtyFactor, string inMfgUM, decimal inMfgUMQty)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemGenFG].NewRow();
            dtrTemPd["nRecNo"] = 1;
            dtrTemPd["cProd"] = inProd;
            dtrTemPd["cUOM"] = inMfgUM;
            dtrTemPd["nQty"] = inQtyFactor;
            dtrTemPd["nUOMQty"] = inMfgUMQty;

            dtrTemPd["cWHouse"] = this.mstrDefaFGWHouse;

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                dtrTemPd["cPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cLastPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = dtrProd["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[this.mstrTemGenFG].Rows.Add(dtrTemPd);
        }

        private void pmReplTemPdStI2(int inRecNo, DataRow inTemPd, decimal inQtyFactor)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemGenPd].NewRow();
            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inTemPd["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inTemPd["cProd"].ToString();
            dtrTemPd["cLastProd"] = inTemPd["cProd"].ToString();
            dtrTemPd["cUOM"] = inTemPd["cUM"].ToString().TrimEnd();
            dtrTemPd["nRefQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            //dtrTemPd["nQty"] = Convert.ToDecimal(inTemPd["nQty"]) * inQtyFactor;
            dtrTemPd["nQty"] = inQtyFactor;
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inTemPd["nUMQty"]) == 0 ? 1 : Convert.ToDecimal(inTemPd["nUMQty"]));
            dtrTemPd["nBackQty1"] = Convert.ToDecimal(inTemPd["nQty"]) * inQtyFactor;
            dtrTemPd["nBackQty2"] = Convert.ToDecimal(inTemPd["nQty"]) * inQtyFactor;

            //dtrTemPd["cWHouse"] = this.mstrDefaWHouse;
            dtrTemPd["cWHouse"] = this.mstrDefaWRWHouse;

            dtrTemPd["cProcure"] = inTemPd["cProcure"].ToString().TrimEnd();
            dtrTemPd["cScrap"] = inTemPd["cScrap"].ToString().TrimEnd();
            dtrTemPd["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString().TrimEnd();
            dtrTemPd["cPdStI"] = inTemPd["cRowID"].ToString();
            dtrTemPd["cMfgBOMHD"] = inTemPd["cMfgBOMHD"].ToString();

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                dtrTemPd["cPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cLastPdType"] = dtrProd["fcType"].ToString().TrimEnd();
                dtrTemPd["cQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = dtrProd["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = dtrProd["fcUM"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[this.mstrTemGenPd].Rows.Add(dtrTemPd);
        }

        private void pmReplTemPdStOP2(int inRecNo, DataRow inTemOP)
        {
            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemGenOP].NewRow();
            dtrTemPd["nRecNo"] = inRecNo;
            //dtrTemPd["cOPSeq"] = inTemOP["cOPSeq"].ToString().TrimEnd();
            //dtrTemPd["cMOPR"] = inTemOP["cMOPR"].ToString().TrimEnd();
            //dtrTemPd["cWkCtrH"] = inTemOP["cWkCtrH"].ToString().TrimEnd();
            //dtrTemPd["cRemark1"] = inTemOP["cRemark1"].ToString().TrimEnd();

            dtrTemPd["cOPSeq"] = inTemOP["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cNextOP"] = inTemOP["cNextOP"].ToString().TrimEnd();
            dtrTemPd["cMOPR"] = inTemOP["cMOPR"].ToString().TrimEnd();
            dtrTemPd["cWkCtrH"] = inTemOP["cWkCtrH"].ToString().TrimEnd();
            dtrTemPd["cResource"] = inTemOP["cResource"].ToString().TrimEnd();
            dtrTemPd["cRemark1"] = inTemOP["cRemark1"].ToString().TrimEnd();
            dtrTemPd["cRemark2"] = inTemOP["cRemark2"].ToString().TrimEnd();
            dtrTemPd["cRemark3"] = inTemOP["cRemark3"].ToString().TrimEnd();
            dtrTemPd["cRemark4"] = inTemOP["cRemark4"].ToString().TrimEnd();
            dtrTemPd["cRemark5"] = inTemOP["cRemark5"].ToString().TrimEnd();
            dtrTemPd["cRemark6"] = inTemOP["cRemark6"].ToString().TrimEnd();
            dtrTemPd["cRemark7"] = inTemOP["cRemark7"].ToString().TrimEnd();
            dtrTemPd["cRemark8"] = inTemOP["cRemark8"].ToString().TrimEnd();
            dtrTemPd["cRemark9"] = inTemOP["cRemark9"].ToString().TrimEnd();
            dtrTemPd["cRemark10"] = inTemOP["cRemark10"].ToString().TrimEnd();

            dtrTemPd["cQcRemark1"] = inTemOP["cQcRemark1"].ToString().TrimEnd();
            dtrTemPd["cQcRemark2"] = inTemOP["cQcRemark2"].ToString().TrimEnd();
            dtrTemPd["cQcRemark3"] = inTemOP["cQcRemark3"].ToString().TrimEnd();
            dtrTemPd["cQcRemark4"] = inTemOP["cQcRemark4"].ToString().TrimEnd();
            dtrTemPd["cQcRemark5"] = inTemOP["cQcRemark5"].ToString().TrimEnd();
            dtrTemPd["cQcRemark6"] = inTemOP["cQcRemark6"].ToString().TrimEnd();
            dtrTemPd["cQcRemark7"] = inTemOP["cQcRemark7"].ToString().TrimEnd();
            dtrTemPd["cQcRemark8"] = inTemOP["cQcRemark8"].ToString().TrimEnd();
            dtrTemPd["cQcRemark9"] = inTemOP["cQcRemark9"].ToString().TrimEnd();
            dtrTemPd["cQcRemark10"] = inTemOP["cQcRemark10"].ToString().TrimEnd();

            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Process]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]);
            dtrTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemOP[QMFWOrderIT_OPInfo.Field.LeadTime_Move]);

            //By Yod 22/02/09
            dtrTemPd["cMFBOMIT_STDOP"] = inTemOP["cRowID"].ToString().TrimEnd(); 

            this.dtsDataEnv.Tables[this.mstrTemGenOP].Rows.Add(dtrTemPd);
        }

        private void pmClearTemToBlank2()
        {
            this.dtsDataEnv.Tables[this.mstrTemGenFG].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemGenPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemGenOP].Rows.Clear();
        }

        private bool pmSaveMulti_SaveWOrderI(DataRow inParentRow, string inAlias, ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                bool bllIsNewRow = false;
                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                //&& (Convert.ToDecimal(dtrTemPd["nQty"]) != 0 ? true : MessageBox.Show("สินค้า " + dtrTemPd["cQcProd"].ToString().TrimEnd() + "\nยังไม่ได้ระบุจำนวนต้องการ Save ด้วยหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        || (pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cRowID"].ToString().TrimEnd() })
                        && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefProd", this.mstrITable2, "select cRowID from " + this.mstrITable2 + " where cRowID = ?", ref strErrorMsg)))
                    {
                        strRowID = App.mRunRowID(this.mstrITable2);
                        bllIsNewRow = true;
                    }
                    else
                    {
                        strRowID = dtrTemPd["cRowID"].ToString().TrimEnd();
                    }
                    dtrTemPd["cRowID"] = strRowID;
                    DataRow dtrWOrderI = null;
                    this.pmSaveMulti_ReplRecordWOrderI(inParentRow, inAlias, bllIsNewRow, dtrTemPd, ref dtrWOrderI);

                    string strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrWOrderI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    pobjSQLUtil.SetPara(pAPara);
                    bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr, ref strErrorMsg);

                    if (dtrTemPd["cProcure"].ToString().TrimEnd() == SysDef.gc_PROCURE_TYPE_MANUFACURING)
                    {
                        DataRow dtrGen1 = this.dtsDataEnv.Tables[this.mstrTemGenMulti].NewRow();

                        DataSetHelper.CopyDataRow(dtrTemPd, ref dtrGen1);

                        this.dtsDataEnv.Tables[this.mstrTemGenMulti].Rows.Add(dtrGen1);

                    }

                }
                else
                {
                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {
                        pobjSQLUtil.SetPara(new object[] { dtrTemPd["cRowID"].ToString() });
                        bllResult = pobjSQLUtil.SQLExec("delete from " + this.mstrITable2 + " where cRowID = ?", ref strErrorMsg);
                    }
                }
            }
            return true;
        }

        private bool pmSaveMulti_ReplRecordWOrderI(DataRow inParentRow, string inAlias, bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrWOrderI;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (bllIsNewRec)
            {
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where 0=1", ref strErrorMsg);
                dtrWOrderI = this.dtsDataEnv.Tables[this.mstrITable2].NewRow();
                dtrWOrderI["cRowID"] = inTemPd["cRowID"].ToString();
            }
            else
            {
                pobjSQLUtil.SetPara(new object[1] { inTemPd["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where cRowID = ?", ref strErrorMsg);
                dtrWOrderI = this.dtsDataEnv.Tables[this.mstrITable2].Rows[0];
            }

            DataRow dtrWOrderH = inParentRow;

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
            dtrWOrderI["cMfgBOMHD"] = inTemPd["cMfgBOMHD"].ToString();

            //สำหรับ Save Child MO ใน Level ถัด ๆ ไป
            inTemPd["cWOrderH"] = dtrWOrderH["cRowID"].ToString();

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
            dtrWOrderI["cIoType"] = (inAlias == this.mstrTemGenFG ? "I" : "O");
            dtrWOrderI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrWOrderI["cBOMIT_PD"] = inTemPd["cPdStI"].ToString();
            dtrWOrderI["cProcure"] = inTemPd["cProcure"].ToString();

            dtrWOrderI["cScrap"] = inTemPd["cScrap"].ToString().TrimEnd();
            dtrWOrderI["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString().TrimEnd();

            dtrWOrderI["nRefQty"] = Convert.ToDecimal(inTemPd["nRefQty"]);
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
            dtrWOrderI["cStep1"] = inTemPd["cStep1"].ToString();
            dtrWOrderI["cStep2"] = inTemPd["cStep2"].ToString();

            ioRefProd = dtrWOrderI;
            return true;
        }


        private bool pmSaveMulti_SaveWOrderOP(DataRow inParentRow, string inAlias, ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            foreach (DataRow dtrTemOP in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                bool bllIsNewRow = false;
                if (dtrTemOP["cMOPR"].ToString().TrimEnd() != string.Empty)
                {
                    if ((dtrTemOP["cRowID"].ToString().TrimEnd() == string.Empty)
                        || (pobjSQLUtil.SetPara(new object[1] { dtrTemOP["cRowID"].ToString().TrimEnd() })
                        && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkWOrderOP", this.mstrITable, "select cRowID from " + this.mstrITable + " where cRowID = ?", ref strErrorMsg)))
                    {
                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                    }
                    else
                    {
                        strRowID = dtrTemOP["cRowID"].ToString().TrimEnd();
                    }
                    dtrTemOP["cRowID"] = strRowID;
                    DataRow dtrWOrderOP = null;
                    this.pmReplWOrderOP2(inParentRow, bllIsNewRow, dtrTemOP, ref dtrWOrderOP);

                    string strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrWOrderOP, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    pobjSQLUtil.SetPara(pAPara);
                    bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr, ref strErrorMsg);
                }
                else
                {
                    //Delete WOrderOP
                    if (dtrTemOP["cRowID"].ToString().TrimEnd() != string.Empty)
                    {
                        pobjSQLUtil.SetPara(new object[] { dtrTemOP["cRowID"].ToString() });
                        bllResult = pobjSQLUtil.SQLExec("delete from " + this.mstrITable + " where CROWID = ?", ref strErrorMsg);
                    }
                }
            }
            return true;
        }

        private bool pmReplWOrderOP2(DataRow inParentRow, bool inState, DataRow inTemPd, ref DataRow ioPdStI)
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrWOrderOP;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (bllIsNewRec)
            {
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", ref strErrorMsg);
                dtrWOrderOP = this.dtsDataEnv.Tables[this.mstrITable].NewRow();
                dtrWOrderOP["cRowID"] = inTemPd["cRowID"].ToString();
            }
            else
            {
                pobjSQLUtil.SetPara(new object[1] { inTemPd["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", ref strErrorMsg);
                dtrWOrderOP = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
            }

            DataRow dtrWOrderH = inParentRow;

            dtrWOrderOP["cCorp"] = App.ActiveCorp.RowID;
            dtrWOrderOP["cPlant"] = this.mstrPlant;
            dtrWOrderOP["cWOrderH"] = dtrWOrderH["cRowID"].ToString();
            dtrWOrderOP["cMOPR"] = inTemPd["cMOPR"].ToString();
            dtrWOrderOP["cWkCtrH"] = inTemPd["cWkCtrH"].ToString();
            dtrWOrderOP["cResType"] = SysDef.gc_RESOURCE_TYPE_TOOL;
            dtrWOrderOP["cResource"] = inTemPd["cResource"].ToString();
            dtrWOrderOP["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrWOrderOP["cNextOP"] = inTemPd["cNextOP"].ToString();
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]);
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]);
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Process]);
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]);
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]);
            dtrWOrderOP[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Move]);
            dtrWOrderOP["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrWOrderOP["cRemark2"] = inTemPd["cRemark2"].ToString().TrimEnd();
            dtrWOrderOP["cRemark3"] = inTemPd["cRemark3"].ToString().TrimEnd();
            dtrWOrderOP["cRemark4"] = inTemPd["cRemark4"].ToString().TrimEnd();
            dtrWOrderOP["cRemark5"] = inTemPd["cRemark5"].ToString().TrimEnd();
            dtrWOrderOP["cRemark6"] = inTemPd["cRemark6"].ToString().TrimEnd();
            dtrWOrderOP["cRemark7"] = inTemPd["cRemark7"].ToString().TrimEnd();
            dtrWOrderOP["cRemark8"] = inTemPd["cRemark8"].ToString().TrimEnd();
            dtrWOrderOP["cRemark9"] = inTemPd["cRemark9"].ToString().TrimEnd();
            dtrWOrderOP["cRemark10"] = inTemPd["cRemark10"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark1"] = inTemPd["cQcRemark1"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark2"] = inTemPd["cQcRemark2"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark3"] = inTemPd["cQcRemark3"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark4"] = inTemPd["cQcRemark4"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark5"] = inTemPd["cQcRemark5"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark6"] = inTemPd["cQcRemark6"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark7"] = inTemPd["cQcRemark7"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark8"] = inTemPd["cQcRemark8"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark9"] = inTemPd["cQcRemark9"].ToString().TrimEnd();
            dtrWOrderOP["cQcRemark10"] = inTemPd["cQcRemark10"].ToString().TrimEnd();

            dtrWOrderOP["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            dtrWOrderOP["nCapFactor1"] = Convert.ToDecimal(inTemPd["nCapFactor1"]);
            dtrWOrderOP["nCapPress"] = Convert.ToDecimal(inTemPd["nCapPress"]);

            if (this.mFormEditMode == UIHelper.AppFormState.Insert)
            {
                dtrWOrderOP["cCreateBy"] = App.AppUserID;
            }
            dtrWOrderOP["dModify"] = pobjSQLUtil.GetDBServerDateTime();
            dtrWOrderOP["cModifyBy"] = App.AppUserID;

            ioPdStI = dtrWOrderOP;
            return true;
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
            else if (this.txtQcCoor.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุลูกค้า", "Customer is not define ! " });
                this.txtQcCoor.Focus();
                return false;
            }
            else if (this.txtQcBOM.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุ BOM", "BOM is not define ! " });
                this.txtQcCoor.Focus();
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
            else if (this.txtTotMfgQty.Value == 0)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุจำนวนที่สั่งผลิต", "Mfg. Qty is not define ! " });
                this.txtTotMfgQty.Focus();
                return false;
            }
            else if (this.txtSOQty.Value != 0 && this.txtMfgQty.Value < this.txtSOQty.Value)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "จำนวนที่สั่งผลิตต้องมากกว่าจำนวนตาม S/O", "Mfg. Qty must greater than S/O Qty ! " });
                this.txtMfgQty.Focus();
                return false;
            }
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

            string strSQLStr = "select cBatchNo as cCode from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cBatchNo < ':' order by cBatchNo desc";
            
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
            string strBatchNo = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            this.mstrBatchNo = strBatchNo;
            this.mstrBatchRun = "01";

            this.txtCode.Text = strBatchNo + "-" + this.mstrBatchRun;
            if (this.txtRefNo.Text.TrimEnd() == string.Empty)
            {
                this.txtRefNo.Text = this.mstrRefType + this.mstrQcBook + "/" + this.txtCode.Text.TrimEnd();
            }
            return true;
        }

        private bool pmRunCode2(string inBatchNo, string inLevel, ref string ioCode)
        {
            string strErrorMsg = "";
            long intRunCode = 2;
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            //pAPara = new object[7] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, inBatchNo, inLevel };
            //strSQLStr = "select cCode from "+this.mstrHTable+" where cCorp = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cCode < ':' order by cCode desc";
            //strSQLStr = "select cBatchRun from " + this.mstrHTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cBatchNo = ? and cLevel = ? and cBatchRun < ':' order by cBatchRun desc ";
            pAPara = new object[6] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, inBatchNo };
            strSQLStr = "select cBatchRun from " + this.mstrHTable + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType = ? and cMfgBook = ? and cBatchNo = ? and cBatchRun < ':' order by cBatchRun desc ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                try
                {
                    intRunCode = Convert.ToInt64(this.dtsDataEnv.Tables["QRunCode"].Rows[0]["cBatchRun"]) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
            }
            this.mstrBatchRun = intRunCode.ToString("00");
            ioCode = inBatchNo + "-" + this.mstrBatchRun;

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

            string strMasterWO = (this.mstrLevel == "01" ? this.mstrEditRowID : this.mstrMasterWO);
			string strCode = this.txtCode.Text.TrimEnd();
			if (strCode.Length > 3)
				strCode = StringHelper.Left(strCode, strCode.Length-3);

            string strBatchNo = (this.mstrBatchNo.TrimEnd() == string.Empty ? strCode : this.mstrBatchNo);

            this.mstrSaveRowID = this.mstrEditRowID;
            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFWOrderHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFWOrderHDInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QMFWOrderHDInfo.Field.PlantID] = this.mstrPlant;

            dtrSaveInfo[QMFWOrderHDInfo.Field.Step] = this.mstrStep;
            dtrSaveInfo[QMFWOrderHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFWOrderHDInfo.Field.RFType] = this.mstrRfType;

            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgBookID] = this.mstrBook;
            dtrSaveInfo[QMFWOrderHDInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.RefNo] = this.txtRefNo.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Date] = this.txtDate.DateTime.Date;

            this.mstrPDocCode = this.txtCode.Text.TrimEnd();

            dtrSaveInfo[QMFWOrderHDInfo.Field.StartDate] = this.txtStartDate.DateTime.Date;
            dtrSaveInfo[QMFWOrderHDInfo.Field.DueDate] = this.txtDueDate.DateTime.Date;

            dtrSaveInfo[QMFWOrderHDInfo.Field.CoorID] = this.txtQcCoor.Tag.ToString();
            dtrSaveInfo[QMFWOrderHDInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFWOrderHDInfo.Field.JobID] = this.txtQcJob.Tag.ToString();
            dtrSaveInfo[QMFWOrderHDInfo.Field.BOMID] = this.txtQcBOM.Tag.ToString();

            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgProdID] = this.txtQcProd.Tag.ToString();
            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgQty] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgUMID] = this.txtQnMfgUM.Tag.ToString();
            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgUMQty] = this.mdecUMQty;

            dtrSaveInfo[QMFWOrderHDInfo.Field.BOMOutputQty] = this.txtPdStOutput.Value;
            dtrSaveInfo[QMFWOrderHDInfo.Field.RMQtyFactor1] = this.txtRatio2.Value;

            dtrSaveInfo[QMFWOrderHDInfo.Field.Scrap] = this.txtScrap.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.ScrapQty] = this.mdecScrapQty;
            dtrSaveInfo[QMFWOrderHDInfo.Field.MfgQty] = this.txtMfgQty.Value;
            dtrSaveInfo[QMFWOrderHDInfo.Field.Qty] = this.txtTotMfgQty.Value;

            string strRound = SysDef.gc_ROUND_TYPE_NORMAL;
            switch (this.cmdRoundCtrl.SelectedIndex)
            {
                case 0:
                    strRound = SysDef.gc_ROUND_TYPE_NORMAL;
                    break;
                case 1:
                    strRound = SysDef.gc_ROUND_TYPE_UP;
                    break;
                case 2:
                    strRound = SysDef.gc_ROUND_TYPE_DOWN;
                    break;
            }
            dtrSaveInfo[QMFWOrderHDInfo.Field.RoundCtrl] = strRound;

            string strPriority = "";
            switch (this.cmbPriority.SelectedIndex)
            {
                case 0:
                    strPriority = "0";
                    break;
                case 1:
                    strPriority = "3";
                    break;
                case 2:
                    strPriority = "7";
                    break;
                case 3:
                    strPriority = "9";
                    break;
            }

            dtrSaveInfo[QMFWOrderHDInfo.Field.Priority] = strPriority;

            dtrSaveInfo[QMFWOrderHDInfo.Field.BatchNo] = strBatchNo;
            dtrSaveInfo[QMFWOrderHDInfo.Field.BatchRun] = this.mstrBatchRun;
            dtrSaveInfo[QMFWOrderHDInfo.Field.Level] = this.mstrLevel;
            dtrSaveInfo[QMFWOrderHDInfo.Field.MasterID] = strMasterWO;
            dtrSaveInfo[QMFWOrderHDInfo.Field.ParentID] = this.mstrParentWO;

            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark1] = this.txtRemark1.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark2] = this.txtRemark2.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark3] = this.txtRemark3.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark4] = this.txtRemark4.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark5] = this.txtRemark5.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark6] = this.txtRemark6.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark7] = this.txtRemark7.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark8] = this.txtRemark8.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark9] = this.txtRemark9.Text.TrimEnd();
            dtrSaveInfo[QMFWOrderHDInfo.Field.Remark10] = this.txtRemark10.Text.TrimEnd();

            switch (BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()))
            {
                case ApproveStep.Wait:
                    dtrSaveInfo[QMFWOrderHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFWOrderHDInfo.Field.ApproveBy] = "";
                    dtrSaveInfo[QMFWOrderHDInfo.Field.ApproveDate] = Convert.DBNull;
                    break;
                case ApproveStep.Approve:
                    dtrSaveInfo[QMFWOrderHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFWOrderHDInfo.Field.ApproveBy] = App.AppUserID;
                    dtrSaveInfo[QMFWOrderHDInfo.Field.ApproveDate] = this.txtDApprove.DateTime;
                    break;
            }

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

                this.pmUpdateBomIT_StdOP(this.mstrTemOP);
                this.pmUpdateBomIT_Pd(this.mstrTemFG);
                this.pmUpdateBomIT_Pd(this.mstrTemPd);

                this.pmSaveTemRefTo();

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

        private void pmUpdateBomIT_StdOP(string inAlias)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cOPSeq"].ToString().TrimEnd() != string.Empty)
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

                    this.pmReplRecordStdOP(bllIsNewRow, this.mstrTemOP, dtrTemPd, ref dtrBudCI);

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

        private bool pmReplRecordStdOP(bool inState, string inType, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cPlant"] = this.mstrPlant;
            dtrBudCI["cRefType"] = dtrBudCH["cRefType"].ToString();
            //dtrBudCI["cType"] = "O";
            dtrBudCI["cWOrderH"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cMOPR"] = inTemPd["cMOPR"].ToString();
            dtrBudCI["cWkCtrH"] = inTemPd["cWkCtrH"].ToString();
            dtrBudCI["cResType"] = SysDef.gc_RESOURCE_TYPE_TOOL;
            dtrBudCI["cResource"] = inTemPd["cResource"].ToString();
            dtrBudCI["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrBudCI["cNextOP"] = inTemPd["cNextOP"].ToString();
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]);
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]);
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Process]);
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]);
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]);
            dtrBudCI[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemPd[QMFWOrderIT_OPInfo.Field.LeadTime_Move]);
            dtrBudCI["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrBudCI["cRemark2"] = inTemPd["cRemark2"].ToString().TrimEnd();
            dtrBudCI["cRemark3"] = inTemPd["cRemark3"].ToString().TrimEnd();
            dtrBudCI["cRemark4"] = inTemPd["cRemark4"].ToString().TrimEnd();
            dtrBudCI["cRemark5"] = inTemPd["cRemark5"].ToString().TrimEnd();
            dtrBudCI["cRemark6"] = inTemPd["cRemark6"].ToString().TrimEnd();
            dtrBudCI["cRemark7"] = inTemPd["cRemark7"].ToString().TrimEnd();
            dtrBudCI["cRemark8"] = inTemPd["cRemark8"].ToString().TrimEnd();
            dtrBudCI["cRemark9"] = inTemPd["cRemark9"].ToString().TrimEnd();
            dtrBudCI["cRemark10"] = inTemPd["cRemark10"].ToString().TrimEnd();
            dtrBudCI["cQcRemark1"] = inTemPd["cQcRemark1"].ToString().TrimEnd();
            dtrBudCI["cQcRemark2"] = inTemPd["cQcRemark2"].ToString().TrimEnd();
            dtrBudCI["cQcRemark3"] = inTemPd["cQcRemark3"].ToString().TrimEnd();
            dtrBudCI["cQcRemark4"] = inTemPd["cQcRemark4"].ToString().TrimEnd();
            dtrBudCI["cQcRemark5"] = inTemPd["cQcRemark5"].ToString().TrimEnd();
            dtrBudCI["cQcRemark6"] = inTemPd["cQcRemark6"].ToString().TrimEnd();
            dtrBudCI["cQcRemark7"] = inTemPd["cQcRemark7"].ToString().TrimEnd();
            dtrBudCI["cQcRemark8"] = inTemPd["cQcRemark8"].ToString().TrimEnd();
            dtrBudCI["cQcRemark9"] = inTemPd["cQcRemark9"].ToString().TrimEnd();
            dtrBudCI["cQcRemark10"] = inTemPd["cQcRemark10"].ToString().TrimEnd();

            dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            dtrBudCI["nCapFactor1"] = Convert.ToDecimal(inTemPd["nCapFactor1"]);
            dtrBudCI["nCapPress"] = Convert.ToDecimal(inTemPd["nCapPress"]);

            return true;
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
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable2 + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable2].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable2);
                        bllIsNewRow = true;
                        //dtrBudCI["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrBudCI["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable2].Rows[0];

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
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable2 + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
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
            dtrWOrderI["cMfgBOMHD"] = inTemPd["cMfgBOMHD"].ToString();
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
            dtrWOrderI["nRefQty"] = Convert.ToDecimal(inTemPd["nRefQty"]);
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
            dtrWOrderI["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString().TrimEnd();

            dtrWOrderI["cStep1"] = inTemPd["cStep1"].ToString();
            dtrWOrderI["cStep2"] = inTemPd["cStep2"].ToString();

            ioRefProd = dtrWOrderI;

            return true;
        }

        private void pmUpdateWOrderIT_Plan(DataRow inMOrderH)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemUsage].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cOPSeq"].ToString().TrimEnd() != string.Empty)
                {

                    string strRowID = "";

                    this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable3, this.mstrITable3, "select * from " + this.mstrITable3 + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable3].NewRow();

                    strRowID = App.mRunRowID(this.mstrITable3);
                    bllIsNewRow = true;
                    dtrTemPd["cRowID"] = strRowID;
                    dtrBudCI["cRowID"] = dtrTemPd["cRowID"].ToString();

                    this.pmReplRecordOP_Plan(bllIsNewRow, inMOrderH, dtrTemPd, ref dtrBudCI);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrBudCI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    ////Delete RefProd
                    //if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    //{
                    //    pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                    //    this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable3 + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    //}

                }

            }
        }

        private bool pmReplRecordOP_Plan(bool inState, DataRow inMorderH, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            DataRow dtrBudCH = inMorderH;

            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cPlant"] = this.mstrPlant;
            dtrBudCI["cBranch"] = dtrBudCH["cBranch"].ToString();
            dtrBudCI["cWOrderH"] = dtrBudCH["cRowID"].ToString();

            dtrBudCI["cWOrderI_OP"] = inTemPd["CWORDERI_OP"].ToString();
            dtrBudCI["cWkCtrH"] = inTemPd["cWkCtrH"].ToString();
            dtrBudCI["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrBudCI["cType"] = inTemPd["cType"].ToString();
            dtrBudCI["dDate"] = Convert.ToDateTime(inTemPd["dDate"]);
            dtrBudCI["dBegTime"] = Convert.ToDateTime(inTemPd["dBegTime"]);
            dtrBudCI["dEndTime"] = Convert.ToDateTime(inTemPd["dEndTime"]);
            dtrBudCI["nUseHour"] = Convert.ToDecimal(inTemPd["nUseHour"]);

            //dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmCreateTem()
        {

            this.dtsDataEnv.CaseSensitive = true;

            this.pmCreateTemPd(this.mstrTemFG);
            this.pmCreateTemPd(this.mstrTemPd);
            this.pmCreateTemOP(this.mstrTemOP);

            this.pmCreateTemPd(this.mstrTemGenFG);
            this.pmCreateTemPd(this.mstrTemGenPd);
            this.pmCreateTemOP(this.mstrTemGenOP);
            this.pmCreateTemPd(this.mstrTemPd_GenPR);

            this.pmCreateTemPd(this.mstrTemGenMulti);
            this.pmCreateTemPd(this.mstrTemGenMulti2);

            DataTable dtbTemPdX1 = new DataTable(this.mstrTemPdX1);
            dtbTemPdX1.Columns.Add("nParentID", System.Type.GetType("System.Int32"));
            dtbTemPdX1.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPdX1.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdX1.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX1.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX1.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX1.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPdX1.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPdX1.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));

            //dtbTemPdX1.Columns["nActRow"].AutoIncrement = true;
            dtbTemPdX1.Columns["nActRow"].DefaultValue = -1;
            dtbTemPdX1.Columns["cRowID"].DefaultValue = "";
            dtbTemPdX1.Columns["cUOMStd"].DefaultValue = "";
            dtbTemPdX1.Columns["cRemark1"].DefaultValue = "";
            dtbTemPdX1.Columns["nQty"].DefaultValue = 0;
            dtbTemPdX1.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPdX1.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPdX1.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPdX1.Columns["nUOMQty"].DefaultValue = 1;

            this.dtsDataEnv.Tables.Add(dtbTemPdX1);

            DataTable dtbTemRefTo = new DataTable(this.mstrTemRefTo);

            dtbTemRefTo.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRefTo.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemRefTo.Columns.Add("cProd", System.Type.GetType("System.String"));
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

            dtbTemRefTo.Columns["cRowID"].DefaultValue = "";
            dtbTemRefTo.Columns["cGUID"].DefaultValue = "";
            dtbTemRefTo.Columns["cProd"].DefaultValue = "";
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

            DataTable dtbTemPd = new DataTable(this.mstrTemMOrderOP);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("ProcTime", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("SetUpTime", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("HourUsage", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("BegTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("EndTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("OPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("OPName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("WkCtrH", System.Type.GetType("System.String"));

            dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemWorkCal = new DataTable(this.mstrTemWorkCal);

            dtbTemWorkCal.Columns.Add("CTYPE", System.Type.GetType("System.String"));
            dtbTemWorkCal.Columns.Add("DDATE", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DBEGTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DENDTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DCURRTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("TotWkHour", System.Type.GetType("System.Decimal"));
            dtbTemWorkCal.Columns.Add("BalWkHour", System.Type.GetType("System.Decimal"));
            dtbTemWorkCal.Columns.Add("CDATE", System.Type.GetType("System.String"));

            dtsDataEnv.Tables.Add(dtbTemWorkCal);


            DataTable dtbTemUsage = new DataTable(this.mstrTemUsage);

            dtbTemUsage.Columns.Add("CROWID", System.Type.GetType("System.String"));
            dtbTemUsage.Columns.Add("CWORDERI_OP", System.Type.GetType("System.String"));
            dtbTemUsage.Columns.Add("COPSEQ", System.Type.GetType("System.String"));
            dtbTemUsage.Columns.Add("CWKCTRH", System.Type.GetType("System.String"));
            dtbTemUsage.Columns.Add("CTYPE", System.Type.GetType("System.String"));
            dtbTemUsage.Columns.Add("DDATE", System.Type.GetType("System.DateTime"));
            dtbTemUsage.Columns.Add("DBEGTIME", System.Type.GetType("System.DateTime"));
            dtbTemUsage.Columns.Add("DENDTIME", System.Type.GetType("System.DateTime"));
            dtbTemUsage.Columns.Add("NUSEHOUR", System.Type.GetType("System.Decimal"));
            dtbTemUsage.Columns.Add("CDATE", System.Type.GetType("System.String"));

            dtsDataEnv.Tables.Add(dtbTemUsage);

        }

        private void TemRefTo_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void pmCreateTemPd(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIsMainPd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cAttachFile", System.Type.GetType("System.String"));
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
            dtbTemPd.Columns.Add("cScrap", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRoundCtrl", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nRefQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nScrapQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nSaveQty", System.Type.GetType("System.Decimal"));
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
            dtbTemPd.Columns.Add("nBackQty1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty2", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRefToQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cPdStI", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProcure", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSubSti", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStep1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStep2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cMfgBOMHD", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcBOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderH", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nCost", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBalQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStdCost", System.Type.GetType("System.Decimal"));
            //dtbTemPd.Columns.Add("nStdCostAmt", System.Type.GetType("System.Decimal"), "nStdCost*nQty");
            dtbTemPd.Columns.Add("nStdCostAmt", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQcCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQnCoor"].DefaultValue = "";
            dtbTemPd.Columns["cOPSeq"].DefaultValue = "";
            dtbTemPd.Columns["cIsMainPd"].DefaultValue = "";
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nScrapQty"].DefaultValue = 0;
            dtbTemPd.Columns["nSaveQty"].DefaultValue = 0;
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
            dtbTemPd.Columns["nBackQty1"].DefaultValue = 0;
            dtbTemPd.Columns["nBackQty2"].DefaultValue = 0;
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
            dtbTemPd.Columns["cPdStI"].DefaultValue = "";
            dtbTemPd.Columns["cSubSti"].DefaultValue = "";
            dtbTemPd.Columns["nRefToQty"].DefaultValue = 0;
            dtbTemPd.Columns["cProcure"].DefaultValue = SysDef.gc_PROCURE_TYPE_PURCHASE;
            dtbTemPd.Columns["cStep1"].DefaultValue = SysDef.gc_REF_STEP_CUT_STOCK;
            dtbTemPd.Columns["cStep2"].DefaultValue = SysDef.gc_REF_STEP_CUT_STOCK;
            dtbTemPd.Columns["cMfgBOMHD"].DefaultValue = "";
            dtbTemPd.Columns["cQcBOM"].DefaultValue = "";
            dtbTemPd.Columns["nCost"].DefaultValue = 0;
            //dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt"].Expression = "nQty*nCost";
            dtbTemPd.Columns["nBalQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStdCost"].DefaultValue = 0;
            //dtbTemPd.Columns["nStdCostAmt"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void pmCreateTemOP(string inAlias)
        {
            //Opr Seq.
            DataTable dtbTemOP = new DataTable(inAlias);
            dtbTemOP.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemOP.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cNextOP", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cPopGetTime", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cMOPR", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcMOPR", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cWkCtrH", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcWkCtrH", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQnWkCtrH", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nScrapQty1", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nLossQty1", System.Type.GetType("System.Decimal"));
            //dtbTemOP.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            //dtbTemOP.Columns.Add("cUOM", System.Type.GetType("System.String"));
            //dtbTemOP.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            //dtbTemOP.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cWHouse2", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcWHouse2", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQnWHouse2", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark2", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark3", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark4", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark5", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark6", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark7", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark8", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark9", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRemark10", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nRefQty1", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nRefQty2", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("cMFBOMIT_STDOP", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQnResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_Queue, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_SetUp, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_Process, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_Tear, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_Wait, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWOrderIT_OPInfo.Field.LeadTime_Move, System.Type.GetType("System.Decimal"));

            dtbTemOP.Columns.Add("cQcRemark1", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark2", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark3", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark4", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark5", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark6", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark7", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark8", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark9", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcRemark10", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nCapFactor1", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nCapPress", System.Type.GetType("System.Decimal"));

            dtbTemOP.Columns["cRowID"].DefaultValue = "";
            dtbTemOP.Columns["cOPSeq"].DefaultValue = "";
            dtbTemOP.Columns["cNextOP"].DefaultValue = "";
            dtbTemOP.Columns["cPopGetTime"].DefaultValue = "";
            dtbTemOP.Columns["cMOPR"].DefaultValue = "";
            dtbTemOP.Columns["cQcMOPR"].DefaultValue = "";
            dtbTemOP.Columns["cWkCtrH"].DefaultValue = "";
            dtbTemOP.Columns["cQcWkCtrH"].DefaultValue = "";
            dtbTemOP.Columns["cQnWkCtrH"].DefaultValue = "";
            dtbTemOP.Columns["cResource"].DefaultValue = "";
            dtbTemOP.Columns["cQcResource"].DefaultValue = "";
            dtbTemOP.Columns["cQnResource"].DefaultValue = "";
            dtbTemOP.Columns["cRemark1"].DefaultValue = "";
            dtbTemOP.Columns["cRemark2"].DefaultValue = "";
            dtbTemOP.Columns["cRemark3"].DefaultValue = "";
            dtbTemOP.Columns["cRemark4"].DefaultValue = "";
            dtbTemOP.Columns["cRemark5"].DefaultValue = "";
            dtbTemOP.Columns["cRemark6"].DefaultValue = "";
            dtbTemOP.Columns["cRemark7"].DefaultValue = "";
            dtbTemOP.Columns["cRemark8"].DefaultValue = "";
            dtbTemOP.Columns["cRemark9"].DefaultValue = "";
            dtbTemOP.Columns["cRemark10"].DefaultValue = "";
            dtbTemOP.Columns["cWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["cQcWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["cQnWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["nScrapQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nLossQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nRefQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nRefQty2"].DefaultValue = 0;
            dtbTemOP.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemOP.Columns["cMFBOMIT_STDOP"].DefaultValue = "";
            dtbTemOP.Columns["nCapFactor1"].DefaultValue = 1;
            dtbTemOP.Columns["nCapPress"].DefaultValue = 0;

            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_Queue].DefaultValue = 0;
            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp].DefaultValue = 0;
            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_Process].DefaultValue = 0;
            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_Tear].DefaultValue = 0;
            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_Wait].DefaultValue = 0;
            dtbTemOP.Columns[QMFWOrderIT_OPInfo.Field.LeadTime_Move].DefaultValue = 0;

            dtbTemOP.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);

            this.dtsDataEnv.Tables.Add(dtbTemOP);
        }


        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            DataRow dtrTemPd = e.Row;
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;

            if (dtrTemPd.Table.TableName == this.mstrTemPd)
            {
                decimal decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                decimal decStdCost = (Convert.IsDBNull(dtrTemPd["nStdCost"]) ? 0 : Convert.ToDecimal(dtrTemPd["nStdCost"]));
                dtrTemPd["nStdCostAmt"] = (decQty * decStdCost);
            }

        }

        private void pmLoadEditPage()
        {
            this.pmCalcColWidth();
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

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.mstrSaveRowID = "";
            this.mstrEditRowID = "";
            
            this.mbllIsReviseBOMOnly = false;
            
            this.txtCode.Text = "";
            this.txtRefNo.Text = "";
            this.txtRefTo.Text = "";
            this.txtDate.DateTime = DateTime.Now;

            this.txtQcCoor.Enabled = true;
            this.txtQnCoor.Enabled = true;
            this.txtQcProd.Enabled = true;

            this.mstrStep = (this.bllWaitApprove == true ? SysDef.gc_REF_STEP_WAIT_APPROVE : SysDef.gc_REF_STEP_CUT_STOCK);

            this.txtQcCoor.Tag = "";
            this.txtQcCoor.Text = "";
            this.txtQnCoor.Text = "";

            this.txtQcProd.Tag = "";
            this.txtQcProd.Text = "";
            this.txtSOQty.Value = 0;
            this.txtFGBalQty.Value = 0;

            this.txtQcBOM.Tag = "";
            this.txtQcBOM.Text = "";
            this.txtQnBOM.Text = "";

            this.txtQnMfgUM.Tag = "";
            this.txtQnMfgUM.Text = "";

            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQcJob.Text = "";
            this.txtQcJob.Text = "";

            this.txtStartDate.EditValue = DateTime.Now;
            this.txtDueDate.EditValue = DateTime.Now;

            this.cmbPriority.SelectedIndex = 1;
            this.txtIsApprove.Text = "";
            this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_WAIT;
            this.txtApproveBy.Text = "";
            this.txtLastUpd.Text = "";
            this.txtQnLastUpd.Text = "";
            this.txtDApprove.EditValue = null;
            //this.txtMfgQty.EditValue = 1;
            //this.txtTotMfgQty.EditValue = 1;

            this.txtMfgQty.EditValue = 0;
            this.txtTotMfgQty.EditValue = 0;
            
            this.mdecUMQty = 1;
            this.mdecLastMfgQty = 0;
            this.mdecLastTotMfgQty = 0;

            this.cmdRoundCtrl.SelectedIndex = 0;
            this.mdecPdStOutput = 1;
            this.txtPdStOutput.Value = this.mdecPdStOutput;
            this.txtRatio1.Value = this.txtTotMfgQty.Value;
            this.txtRatio2.Value = this.txtTotMfgQty.Value;

            this.txtQnMfgUM.Tag = "";
            this.txtQnMfgUM.Text = "";

            this.mdecScrapQty = 0;
            this.txtScrap.Text = "";
            this.mstrLastScrap = "";

            //this.txtQnStdUM.Tag = "";
            //this.txtQnStdUM.Text = "";

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

            this.pmSetUMQtyMsg();

            this.mstrMasterWO = "";
            this.mstrParentWO = "";
            this.mstrLevel = "01";
            this.mstrBatchNo = "";
            this.mstrBatchRun = "";

            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemFG].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPdX1].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Clear();

            this.gridView4.FocusedColumn = this.gridView4.Columns["cPdType"];
            this.gridView3.FocusedColumn = this.gridView3.Columns["cOPSeq"];
            this.gridView2.FocusedColumn = this.gridView2.Columns["cOPSeq"];

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

                    this.mstrStep = dtrLoadForm[QMFWOrderHDInfo.Field.Step].ToString();

                    this.mstrMasterWO = dtrLoadForm[QMFWOrderHDInfo.Field.MasterID].ToString().TrimEnd();
                    this.mstrParentWO = dtrLoadForm[QMFWOrderHDInfo.Field.ParentID].ToString().TrimEnd();
                    this.mstrLevel = dtrLoadForm[QMFWOrderHDInfo.Field.Level].ToString().TrimEnd();
                    this.mstrBatchNo = dtrLoadForm[QMFWOrderHDInfo.Field.BatchNo].ToString().TrimEnd();
                    this.mstrBatchRun = dtrLoadForm[QMFWOrderHDInfo.Field.BatchRun].ToString().TrimEnd();
                    this.txtCode.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Code].ToString().TrimEnd();
                    this.txtRefNo.Text = dtrLoadForm[QMFWOrderHDInfo.Field.RefNo].ToString().TrimEnd();
                    this.txtDate.DateTime = Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.Date]).Date;

                    if (!Convert.IsDBNull(dtrLoadForm[QMFWOrderHDInfo.Field.StartDate]))
                    {
                        this.txtStartDate.EditValue = Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.StartDate]).Date;
                    }
                    else
                    {
                        this.txtStartDate.EditValue = this.txtDate.DateTime;
                    }
                    this.txtDueDate.EditValue = (Convert.IsDBNull(dtrLoadForm[QMFWOrderHDInfo.Field.DueDate]) ? this.txtDate.DateTime.Date : Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.DueDate]).Date);
                    //this.cmbPriotiry.SelectedValue = dtrLoadForm["cPriority"].ToString();

                    //this.mdttReceDate = Convert.ToDateTime(dtrLoadForm["dReceDate"]).Date;
                    this.txtQcSect.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.SectID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcSect.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.JobID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcJob.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from Job where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcCoor.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.CoorID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcCoor.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.txtQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                        this.txtQnCoor.Text = dtrCoor["fcSName"].ToString().TrimEnd();
                        //this.mstrLastQcCoor = this.txtQcCoor.Text;
                        //this.mstrLastQsCoor = this.txtQnCoor.Text;
                    }

                    //this.txtReceDate.Value = (Convert.IsDBNull(dtrLoadForm["fdRecedate"]) ? this.txtDate.Value : Convert.ToDateTime(dtrLoadForm["fdRecedate"]));

                    this.txtQcProd.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.MfgProdID].ToString();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcProd.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode, fcUM from Prod where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        this.txtQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                        this.txtFGBalQty.Value = this.pmGetStockBal(dtrProd["fcSkid"].ToString(), this.mstrBook_WHouse_FG);

                        //if (pobjSQLUtil2.SetPara(new object[] { dtrProd["fcUM"].ToString() })
                        //    && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                        //{
                        //    this.txtQnUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                        //    this.txtQnUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                        //}

                    }

                    string strRound = dtrLoadForm[QMFWOrderHDInfo.Field.RoundCtrl].ToString().TrimEnd();
                    switch (strRound)
                    {
                        case "1":
                            this.cmdRoundCtrl.SelectedIndex = 1;
                            break;
                        case "2":
                            this.cmdRoundCtrl.SelectedIndex = 2;
                            break;
                        default:
                            this.cmdRoundCtrl.SelectedIndex = 0;
                            break;
                    }

                    this.txtQcBOM.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.BOMID].ToString().TrimEnd();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcBOM.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode, cName , nMfgQty from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0];
                        this.txtQcBOM.Text = dtrBOM["cCode"].ToString().TrimEnd();
                        this.txtQnBOM.Text = dtrBOM["cName"].ToString().TrimEnd();
                        //this.mstrLastQcPdStH = this.txtQcBOM.Text;
                    }
                    this.mdecPdStOutput = (Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.BOMOutputQty]) != 0 ? Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.BOMOutputQty]) : 1);
                    this.txtPdStOutput.Value = this.mdecPdStOutput;

                    this.txtQnMfgUM.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.MfgUMID].ToString();
                    if (pobjSQLUtil2.SetPara(new object[] { this.txtQnMfgUM.Tag.ToString() })
                        && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                    {
                        this.txtQnMfgUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                        this.txtQnMfgUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    this.txtTotMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.Qty]);
                    this.txtMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgQty]);
                    this.mdecLastMfgQty = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgQty]);
                    this.mdecUMQty = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgUMQty]);

                    this.txtRatio1.Value = (this.mdecPdStOutput != 0 ? this.txtTotMfgQty.Value / this.mdecPdStOutput : 0);
                    this.txtRatio2.Value = (Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);

                    this.pmSetUMQtyMsg();

                    this.txtScrap.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Scrap].ToString().TrimEnd();

                    this.txtRemark1.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark1].ToString().TrimEnd();
                    this.txtRemark2.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark2].ToString().TrimEnd();
                    this.txtRemark3.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark3].ToString().TrimEnd();
                    this.txtRemark4.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark4].ToString().TrimEnd();
                    this.txtRemark5.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark5].ToString().TrimEnd();
                    this.txtRemark6.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark6].ToString().TrimEnd();
                    this.txtRemark7.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark7].ToString().TrimEnd();
                    this.txtRemark8.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark8].ToString().TrimEnd();
                    this.txtRemark9.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark9].ToString().TrimEnd();
                    this.txtRemark10.Text = dtrLoadForm[QMFWOrderHDInfo.Field.Remark10].ToString().TrimEnd();

                    this.txtApproveBy.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.ApproveBy].ToString();
                    string strIsApprove = dtrLoadForm[QMFWOrderHDInfo.Field.IsApprove].ToString().Trim();
                    string strApprove = "";
                    switch (BudgetHelper.GetApproveStep(strIsApprove))
                    {
                        case ApproveStep.Wait:
                            strApprove = " ";
                            this.txtDApprove.EditValue = null;
                            break;
                        case ApproveStep.Approve:
                            strApprove = "/";
                            this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.ApproveDate]);
                            break;
                    }
                    this.txtIsApprove.Text = strApprove;
                    //Load ชื่อ Login ที่อนุมัติ
                    pobjSQLUtil.SetPara(new object[] { this.txtApproveBy.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        this.txtApproveBy.Text = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }

                    //Load ชื่อ Login ที่แก้ไขล่าสุด
                    this.txtQnLastUpd.Tag = dtrLoadForm[QBGTranInfo.Field.LastUpdateBy].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQnLastUpd.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        this.txtQnLastUpd.Text = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }
                    if (!Convert.IsDBNull(dtrLoadForm[QBGTranInfo.Field.LastUpdate]))
                    {
                        DateTime dttLastUpd = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.LastUpdate]);
                        this.txtLastUpd.Text = BudgetHelper.GetShortThaiDate(dttLastUpd) + " " + dttLastUpd.ToString("HH:mm:ss");
                    }

                    this.pmLoadBOMIT_StdOP(this.mstrTemOP);
                    this.pmLoadBOMIT_Pd(this.mstrTemFG);
                    this.pmLoadBOMIT_Pd(this.mstrTemPd);

                    this.mstrLastScrap = this.txtScrap.Text.Trim();
                    this.mdecLastMfgQty = Convert.ToDecimal(this.txtMfgQty.EditValue);
                    this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);

                    this.pmLoadTemRefTo();

                }
                this.pmLoadOldVar();

                this.pmRecalTotPd();
            
            }
        }

        private void pmLoadBOMIT_StdOP(string inAlias)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBomIT_StdOP", "WkCtrIT", "select * from " + this.mstrITable + " where cWOrderH = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBomIT in this.dtsDataEnv.Tables["QBomIT_StdOP"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrBomIT["cRowID"].ToString();
                    dtrNewRow["cOPSeq"] = dtrBomIT["cOPSeq"].ToString().TrimEnd();
                    dtrNewRow["cNextOP"] = dtrBomIT["cNextOP"].ToString().TrimEnd();
                    dtrNewRow["cMOPR"] = dtrBomIT["cMOPR"].ToString().TrimEnd();
                    dtrNewRow["cWkCtrH"] = dtrBomIT["cWkCtrH"].ToString().TrimEnd();
                    dtrNewRow["cResource"] = dtrBomIT["cResource"].ToString().TrimEnd();
                    dtrNewRow["cRemark1"] = dtrBomIT["cRemark1"].ToString().TrimEnd();
                    dtrNewRow["cRemark2"] = dtrBomIT["cRemark2"].ToString().TrimEnd();
                    dtrNewRow["cRemark3"] = dtrBomIT["cRemark3"].ToString().TrimEnd();
                    dtrNewRow["cRemark4"] = dtrBomIT["cRemark4"].ToString().TrimEnd();
                    dtrNewRow["cRemark5"] = dtrBomIT["cRemark5"].ToString().TrimEnd();
                    dtrNewRow["cRemark6"] = dtrBomIT["cRemark6"].ToString().TrimEnd();
                    dtrNewRow["cRemark7"] = dtrBomIT["cRemark7"].ToString().TrimEnd();
                    dtrNewRow["cRemark8"] = dtrBomIT["cRemark8"].ToString().TrimEnd();
                    dtrNewRow["cRemark9"] = dtrBomIT["cRemark9"].ToString().TrimEnd();
                    dtrNewRow["cRemark10"] = dtrBomIT["cRemark10"].ToString().TrimEnd();

                    dtrNewRow["cQcRemark1"] = dtrBomIT["cQcRemark1"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark2"] = dtrBomIT["cQcRemark2"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark3"] = dtrBomIT["cQcRemark3"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark4"] = dtrBomIT["cQcRemark4"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark5"] = dtrBomIT["cQcRemark5"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark6"] = dtrBomIT["cQcRemark6"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark7"] = dtrBomIT["cQcRemark7"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark8"] = dtrBomIT["cQcRemark8"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark9"] = dtrBomIT["cQcRemark9"].ToString().TrimEnd();
                    dtrNewRow["cQcRemark10"] = dtrBomIT["cQcRemark10"].ToString().TrimEnd();

                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_Queue]);
                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_SetUp]);
                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_Process]);
                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_Tear]);
                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_Wait]);
                    dtrNewRow[QMFWOrderIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(dtrBomIT[QMFWOrderIT_OPInfo.Field.LeadTime_Move]);

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cMOPR"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFStdOprInfo.TableName, "select * from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcMOPR"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFStdOprInfo.Field.Code].ToString().TrimEnd();
                    }

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cWkCtrH"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFWorkCenterInfo.TableName, "select * from " + QMFWorkCenterInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcWkCtrH"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFWorkCenterInfo.Field.Code].ToString().TrimEnd();
                        dtrNewRow["cQnWkCtrH"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFWorkCenterInfo.Field.Name].ToString().TrimEnd();
                    }

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cResource"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFResourceInfo.TableName, "select * from " + QMFResourceInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcResource"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Code].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);

                }
            }

        }

        private void pmLoadBOMIT_Pd(string inAlias)
        {
            this.dtsDataEnv.Tables[inAlias].Rows.Clear();

            int intRecNo = 0;
            string strErrorMsg = "";
            string strIOType = (inAlias == this.mstrTemFG ? "I" : "O");
            string strSQLStr = "select * from " + this.mstrITable2 + " where cWOrderH = ? and cIOType = ? order by cSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, strIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables[this.mstrITable2].Rows)
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
            dtrTemPd["cMfgBOMHD"] = inWOrderI["cMfgBOMHD"].ToString().TrimEnd();
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
            dtrTemPd["nRefQty"] = Convert.ToDecimal(inWOrderI["nRefQty"]);
            dtrTemPd["nSaveQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWOrderI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWOrderI["nUOMQty"]));
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["cLot"] = inWOrderI["cLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inWOrderI["cWHouse"].ToString().TrimEnd();
            dtrTemPd["cPdStI"] = inWOrderI["cBOMIT_PD"].ToString();
            dtrTemPd["cScrap"] = inWOrderI["cScrap"].ToString().TrimEnd();
            dtrTemPd["cRoundCtrl"] = inWOrderI["cRoundCtrl"].ToString().TrimEnd();

            dtrTemPd["cProcure"] = inWOrderI["cProcure"].ToString().TrimEnd();
            dtrTemPd["cStep1"] = inWOrderI["cStep1"].ToString();
            dtrTemPd["cStep2"] = inWOrderI["cStep1"].ToString();
            dtrTemPd["nBackQty1"] = Convert.ToDecimal(inWOrderI["nBackQty1"]);
            dtrTemPd["nBackQty2"] = Convert.ToDecimal(inWOrderI["nBackQty2"]);

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select FCCODE,FCNAME,FCUM,FNSTDCOST from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = dtrTemPd["cQcProd"].ToString();
                dtrTemPd["cLastQnProd"] = dtrTemPd["cQnProd"].ToString();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["cStkUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString().TrimEnd();
                dtrTemPd["nStdCost"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["fnStdCost"]);
                dtrTemPd["nStdCostAmt"] = Convert.ToDecimal(dtrTemPd["nStdCost"]) * Convert.ToDecimal(dtrTemPd["nQty"]);
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

            //pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cPdStI"].ToString().TrimEnd() });
            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStI", "PDSTI", "select nQty from MFBOMIT_PD where cRowID = ?", ref strErrorMsg))
            //{
            //    dtrTemPd["nRefQty"] = Convert.ToDecimal(this.dtsDataEnv.Tables["QPdStI"].Rows[0]["nQty"]);
            //}

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", "BOM", "select cCode from " + MapTable.Table.MFBOMHead + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

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
                        //this.pmGotoBrowPage();
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
            if (inOrderBy.ToUpper() == QMFWOrderHDInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFWOrderHDInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtRefNo.Properties.MaxLength);
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
                case "GRDVIEW2_CQCWKCTRH":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCWKCTRH" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("WKCTR");
                    this.pofrmGetWkCtr.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetWkCtr.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                case "GRDVIEW2_CQCMOPR":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCMOPR" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("STDOP");
                    this.pofrmGetStdOP.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetStdOP.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                case "GRDVIEW2_CQCRESOURCE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCRESOURCE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE");

                    DataRow dtrTemOP = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemOP != null && dtrTemOP["cWkCtrH"].ToString().Trim() != string.Empty)
                    {
                        this.pofrmGetResource.ValidateField(dtrTemOP["cWkCtrH"].ToString(), "", strOrderBy, true);
                        if (this.pofrmGetResource.PopUpResult)
                        {
                            this.pmRetrievePopUpVal(inKeyField);
                        }
                    }

                    break;
                case "GRDVIEW2_CPOPGETTIME":
                    this.pmInitPopUpDialog("GETOPTIME");
                    break;
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
                case "GRDVIEW3_CQCBOM":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW3_CQCBOM" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("BOM");

                    dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);
                    string strProd = "";

                    if (dtrTemPd != null && dtrTemPd["cProd"].ToString().Trim() != string.Empty)
                    {
                        strProd = dtrTemPd["cProd"].ToString();
                    }

                    this.pofrmGetBOM.ValidateField(strProd, "", strOrderBy, true);
                    if (this.pofrmGetBOM.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                case "GRDVIEW2_CREMARK1":
                case "GRDVIEW3_CREMARK1":
                    this.pmInitPopUpDialog("REMARK");
                    break;
                
                case "GRDVIEW2_CQCREMARK1":
                    this.pmInitPopUpDialog("QCREMARK");
                    break;

                case "GRDVIEW3_VIEWFILE":
                    this.pmInitPopUpDialog("VIEWATTACH2");
                    break;

            }
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemOP.MainView as ColumnView;

            string strValue = "";
            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCWKCTRH":
                case "CQNWKCTRH":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cWkCtrH"] = "";
                        dtrTemPd["cQcWkCtrH"] = "";
                        dtrTemPd["cQnWkCtrH"] = "";

                        dtrTemPd["cResource"] = "";
                        dtrTemPd["cQcResource"] = "";
                        dtrTemPd["cQnResource"] = "";
                    
                    }
                    else
                    {
                        this.pmInitPopUpDialog("WKCTR");
                        string strOrderBy = (strCol.ToUpper() == "CQCWKCTRH" ? QMFWorkCenterInfo.Field.Code : QMFWorkCenterInfo.Field.Name);
                        e.Valid = !this.pofrmGetWkCtr.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetWkCtr.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCWKCTRH" ? dtrTemPd["cQcWkCtrH"].ToString().TrimEnd() : dtrTemPd["cQnWkCtrH"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cWkCtrH"] = "";
                            dtrTemPd["cQcWkCtrH"] = "";
                            dtrTemPd["cQnWkCtrH"] = "";

                            dtrTemPd["cResource"] = "";
                            dtrTemPd["cQcResource"] = "";
                            dtrTemPd["cQnResource"] = "";
                        
                        }
                    }
                    break;

                case "CQCMOPR":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cMOpr"] = "";
                        dtrTemPd["cQcMOpr"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("STDOP");
                        string strOrderBy = (strCol.ToUpper() == "CQCMOPR" ? QMFWorkCenterInfo.Field.Code : QMFWorkCenterInfo.Field.Name);
                        e.Valid = !this.pofrmGetStdOP.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetStdOP.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCMOPR" ? dtrTemPd["cQcMOPR"].ToString().TrimEnd() : "");
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cMOpr"] = "";
                            dtrTemPd["cQcMOpr"] = "";
                        }
                    }
                    break;

                case "CQCRESOURCE":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cResource"] = "";
                        dtrTemPd["cQcResource"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("RESOURCE");
                        string strOrderBy = (strCol.ToUpper() == "CQCRESOURCE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);

                        e.Valid = !this.pofrmGetResource.ValidateField(dtrTemPd["cWkCtrH"].ToString(), strValue, strOrderBy, false);
                        if (this.pofrmGetResource.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCRESOURCE" ? dtrTemPd["cQcResource"].ToString().TrimEnd() : "");
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cResource"] = "";
                            dtrTemPd["cQcResource"] = "";
                        }
                    }
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
                        view.UpdateCurrentRow();
                        this.pmRecalTotPd();
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
                    view.UpdateCurrentRow();
                    break;
            }
            this.pmRecalTotPd();

        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[e.RowHandle];
                if (Convert.IsDBNull(dtrTemPd["nQty"]))
                {
                    dtrTemPd["nQty"] = 0;
                }
                decimal decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                decimal decStdCost = (Convert.IsDBNull(dtrTemPd["nStdCost"]) ? 0 : Convert.ToDecimal(dtrTemPd["nStdCost"]));

                dtrTemPd["nStdCostAmt"] = (decQty * decStdCost);
                //this.gridView3.SetRowCellValue(this.gridView3.FocusedRowHandle, "nStdCostAmt", decQty * decStdCost);
                
                this.gridView3.UpdateSummary();

                this.pmRecalTotPd();
            
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

        private void btnUMConvert_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("UOMCONVERT");
        }

        private void btnViewAttach_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("VIEWATTACH");
        }

        private void pmClr1TemPd()
        {
            //DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.grdTemPd.Row];
            DataRow dtrTemPd = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

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

        private void txtIsApprove_Enter(object sender, EventArgs e)
        {
            this.txtIsApprove.SelectAll();
        }

        private void txtIsApprove_EditValueChanged(object sender, EventArgs e)
        {
            this.pmToggleApprove();
        }

        private void txtIsApprove_Click(object sender, EventArgs e)
        {
            if (this.txtIsApprove.Text.Trim() != string.Empty)
            {
                this.txtIsApprove.Text = "";
            }
            else
            {
                this.txtIsApprove.Text = "/";
            }
            //this.pmToggleApprove();
        }

        private void pmToggleApprove()
        {

            if (this.txtIsApprove.Text.Trim() != string.Empty)
            {
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                this.txtDApprove.DateTime = pobjSQLUtil.GetDBServerDateTime();

                this.txtApproveBy.Text = App.AppUserName;
                this.txtIsApprove.Text = "/";
                this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_APPROVE;
            }
            else
            {

                this.txtApproveBy.Text = "";
                this.txtIsApprove.Text = " ";
                this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_WAIT;
                this.txtDApprove.EditValue = null;
            }

            //this.txtStat.Text = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()));
            this.txtIsApprove.SelectAll();
            //this.pmSetGrbApproveStat();
        }

        private void txtStartDate_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
            //    if (oSender != null)
            //    {
            //        oSender.EditValue = null;
            //        e.Handled = true;
            //    }
            //}
        }

        private void txtMfgQty_Validated(object sender, EventArgs e)
        {
            if (this.mdecLastMfgQty != Convert.ToDecimal(this.txtMfgQty.EditValue)
                || this.mdecLastTotMfgQty != Convert.ToDecimal(this.txtTotMfgQty.EditValue)
                || this.mstrLastScrap != this.txtScrap.Text.Trim())
            {

                //this.mdecScrapQty = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);

                decimal decScrapQty1 = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);
                if (decScrapQty1 != 0)
                {
                    this.mdecScrapQty = decScrapQty1 - Convert.ToDecimal(this.txtMfgQty.EditValue);
                }
                
                this.txtTotMfgQty.EditValue = Convert.ToDecimal(this.txtMfgQty.EditValue) + this.mdecScrapQty;

                this.pmAdjustQty(this.mstrTemFG);
                this.pmAdjustQty(this.mstrTemPd);
                this.pmAdjustQty2(this.mstrTemOP);

                //this.mdecScrapQty = BizRule.CalScrapQty2(this.txtScrap.Text, Convert.ToDecimal(this.txtMfgQty.EditValue), 0, 0);
                //this.txtTotMfgQty.EditValue = Convert.ToDecimal(this.txtMfgQty.EditValue) + this.mdecScrapQty;
                this.mstrLastScrap = this.txtScrap.Text.Trim();
                this.mdecLastMfgQty = Convert.ToDecimal(this.txtMfgQty.EditValue);
                this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            }
        }

        private void txtTotMfgQty_Validated(object sender, EventArgs e)
        {
            if (this.mdecLastTotMfgQty != Convert.ToDecimal(this.txtTotMfgQty.EditValue))
            {
                this.pmAdjustMfgQty();
                this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            }
        }

        private void txtTotMfgQty_EditValueChanged(object sender, EventArgs e)
        {
            this.txtRatio1.Value = this.txtTotMfgQty.Value / this.txtPdStOutput.Value;

            decimal decQty = this.txtRatio1.Value;
            switch (this.cmdRoundCtrl.SelectedIndex)
            {
                case 1:
                    decQty = Math.Ceiling(decQty);
                    break;
                case 2:
                    decQty = Math.Floor(decQty);
                    break;
                default:
                    break;
            }
            this.txtRatio2.Value = decQty;
        }

        private void pmAdjustMfgQty()
        {
            this.pmAdjustQty(this.mstrTemFG);
            this.pmAdjustQty(this.mstrTemPd);
            this.pmAdjustQty2(this.mstrTemOP);

            this.pmRecalTotPd();
        
        }

        private void pmAdjustQty(string inAlias)
        {
            decimal decMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty
                    && this.mdecLastTotMfgQty != 0)
                {

                    decimal decScrapQty1 = 0;
                    decimal decScrapQty = 0;
                    decimal decSaveQty = 0;
                    //decimal decQty = Math.Round(decQty1 / this.mdecLastTotMfgQty * decMfgQty, App.ActiveCorp.RoundQtyAt, MidpointRounding.AwayFromZero);
                    //decimal decQty1 = Convert.ToDecimal(dtrTemPd["nQty"]) - Convert.ToDecimal(dtrTemPd["nScrapQty"]);
                    decimal decQty1 = Convert.ToDecimal(dtrTemPd["nRefQty"]);

                    //08/02/10 ปรับให้รองรับการปัดเศษที่หัว BOM
                    //decimal decQty = decQty1 / this.mdecLastTotMfgQty * decMfgQty;

                    decimal decQty = 0;
                    if (inAlias == this.mstrTemFG)
                    {
                        //decQty = decQty1 / this.mdecLastTotMfgQty * decMfgQty;
                        decQty = decMfgQty;
                    }
                    else
                    {

                        decQty = decQty1 * this.txtRatio2.Value;

                        decScrapQty1 = BizRule.CalScrapQty2(dtrTemPd["cScrap"].ToString(), decQty, 0, 0);
                        if (decScrapQty1 != 0)
                        {
                            decScrapQty = BizRule.CalScrapQty2(dtrTemPd["cScrap"].ToString(), decQty, 0, 0) - decQty;
                        }

                        decSaveQty = decQty;
                        switch (dtrTemPd["cRoundCtrl"].ToString())
                        {
                            case "1":
                                decQty = Math.Ceiling(decQty);
                                break;
                            case "2":
                                decQty = Math.Floor(decQty);
                                break;
                            default:
                                break;
                        }
                    }

                    dtrTemPd["nQty"] = decQty + decScrapQty;
                    dtrTemPd["nScrapQty"] = decScrapQty;
                    dtrTemPd["nSaveQty"] = decSaveQty;
                    dtrTemPd["nLastQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                    dtrTemPd["nBackQty1"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                    dtrTemPd["nBackQty2"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                }

            }
        }

        private void xxxpmAdjustQty(string inAlias)
        {
            decimal decMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty
                    && this.mdecLastTotMfgQty != 0)
                {

                    decimal decScrapQty1 = 0;
                    decimal decScrapQty = 0;

                    //decimal decQty = Math.Round(decQty1 / this.mdecLastTotMfgQty * decMfgQty, App.ActiveCorp.RoundQtyAt, MidpointRounding.AwayFromZero);
                    //decimal decQty1 = Convert.ToDecimal(dtrTemPd["nQty"]) - Convert.ToDecimal(dtrTemPd["nScrapQty"]);
                    decimal decQty1 = Convert.ToDecimal(dtrTemPd["nSaveQty"]);

                    //08/02/10 ปรับให้รองรับการปัดเศษที่หัว BOM
                    //decimal decQty = decQty1 / this.mdecLastTotMfgQty * decMfgQty;

                    decimal decQty = 0;

                    if (inAlias == this.mstrTemFG)
                    {
                        decQty = decQty1 / this.mdecLastTotMfgQty * decMfgQty;
                    }
                    else
                    {
                        decimal decRatio1 = decMfgQty / this.mdecLastTotMfgQty;
                        decimal decRatio2 = decRatio1;
                        switch (this.cmdRoundCtrl.SelectedIndex)
                        {
                            case 1:
                                decRatio2 = Math.Ceiling(decRatio1);
                                break;
                            case 2:
                                decRatio2 = Math.Floor(decRatio1);
                                break;
                            default:
                                break;
                        }
                        decQty = decQty1 * decRatio2;
                    }
                    decScrapQty1 = BizRule.CalScrapQty2(dtrTemPd["cScrap"].ToString(), decQty, 0, 0);
                    if (decScrapQty1 != 0)
                    {
                        decScrapQty = BizRule.CalScrapQty2(dtrTemPd["cScrap"].ToString(), decQty, 0, 0) - decQty;
                    }

                    decimal decSaveQty = decQty;
                    switch (dtrTemPd["cRoundCtrl"].ToString())
                    { 
                        case "1":
                            decQty = Math.Ceiling(decQty);
                            break;
                        case "2":
                            decQty = Math.Floor(decQty);
                            break;
                        default:
                            break;
                    }

                    dtrTemPd["nQty"] = decQty + decScrapQty;
                    dtrTemPd["nScrapQty"] = decScrapQty;
                    dtrTemPd["nSaveQty"] = decSaveQty;
                    dtrTemPd["nLastQty"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                    dtrTemPd["nBackQty1"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                    dtrTemPd["nBackQty2"] = Convert.ToDecimal(dtrTemPd["nQty"]);
                }

            }
        }

        private void pmAdjustQty2(string inAlias)
        {
            decimal decMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {
                if (dtrTemPd["cWkCtrH"].ToString().TrimEnd() != string.Empty
                    && this.mdecLastMfgQty != 0)
                {
                    //dtrTemPd["nScrapQty1"] = MyMath.Round(Convert.ToDecimal(dtrTemPd["nScrapQty1"]) / this.mdecLastTotMfgQty * decMfgQty, App.ActiveCorp.RoundQtyAt);
                    //dtrTemPd["nLossQty1"] = MyMath.Round(Convert.ToDecimal(dtrTemPd["nLossQty1"]) / this.mdecLastTotMfgQty * decMfgQty, App.ActiveCorp.RoundQtyAt);
                }

            }
        }

        private void pmLoadTemRefTo()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            decimal decSumSOQty = 0;
            string strSORefNo = "";

            objSQLHelper.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, this.mstrRefToRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? ", ref strErrorMsg))
            {

                string strOrderSQLStr = "";
                strOrderSQLStr = "select ORDERH.FCCODE, ORDERH.FCREFNO , ORDERH.FDDATE , ORDERI.FCCOOR , ORDERI.FCPROD , ORDERI.FNBACKQTY from ORDERI ";
                strOrderSQLStr += " left join ORDERH on ORDERH.FCSKID = ORDERI.FCORDERH ";
                strOrderSQLStr += " where ORDERI.FCSKID = ? ";

                foreach (DataRow dtrRefDoc in this.dtsDataEnv.Tables["QRefDoc"].Rows)
                {
                    DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemRefTo].NewRow();
                    dtrNewSO["cOrderI"] = dtrRefDoc["cChildI"].ToString();
                    dtrNewSO["cOrderH"] = dtrRefDoc["cChildH"].ToString();

                    objSQLHelper2.SetPara(new object[] { dtrRefDoc["cChildI"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRef2OrderI", "ORDERI", strOrderSQLStr, ref strErrorMsg))
                    {
                        DataRow dtrOrderI = this.dtsDataEnv.Tables["QRef2OrderI"].Rows[0];
                        dtrNewSO["cCoor"] = dtrOrderI["fcCoor"].ToString();
                        dtrNewSO["cProd"] = dtrOrderI["fcProd"].ToString();
                        dtrNewSO["fcCode"] = dtrOrderI["fcCode"].ToString();
                        dtrNewSO["fcRefNo"] = dtrOrderI["fcRefNo"].ToString();
                        dtrNewSO["fdDate"] = Convert.ToDateTime(dtrOrderI["fdDate"]);
                        dtrNewSO["fnBackQty"] = Convert.ToDecimal(dtrOrderI["fnBackQty"]);
                        dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrRefDoc["nQty"]);

                        decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrOrderI["fnBackQty"]);
                        strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";

                        this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Add(dtrNewSO);

                    }

                }

                this.txtRefTo.Text = strSORefNo;
                this.txtSOQty.Value = decSumSOQty;
                
                this.txtQcCoor.Enabled = false;
                this.txtQnCoor.Enabled = false;
                this.txtQcProd.Enabled = false;

            }

        }

        private int pmGetRowID2(string inAlias, string inSearchField, string inSearchValue)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[inAlias].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[inAlias].Rows[intCnt][inSearchField].ToString().TrimEnd() == inSearchValue.TrimEnd())
                    return intCnt;
            }
            return -1;
        }

        private void pmSaveTemRefTo()
        {
            if (this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows.Count > 0)
            {
                foreach (DataRow dtrRefToRow in this.dtsDataEnv.Tables[this.mstrTemRefTo].Rows)
                {
                    //if (Convert.ToBoolean(dtrRefToRow["IsDelete"]) == false)
                    if (true)
                    {
                        this.pmSaveRefToOrderI(dtrRefToRow["cOrderH"].ToString(), dtrRefToRow["cOrderI"].ToString(), dtrRefToRow["cOld_OrderH"].ToString(), dtrRefToRow["cOld_OrderI"].ToString(), Convert.ToDecimal(dtrRefToRow["fnMOQty"]));
                    }
                }
            }
        }

        private bool pmSaveRefToOrderI(string inOrderH, string inOrderI, string inOldOrderH, string inOldOrderI, decimal inQty)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;

            if (inOldOrderI.Trim() != string.Empty 
                && inOldOrderI != inOrderI)
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, this.mstrRefToRefType, inOldOrderH, inOldOrderI });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? and cChildH = ? and cChildI = ?", ref strErrorMsg);
            }

            bool bllIsNewRow_RefTo = false;
            string strRowID_RefTo = "";
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "REFDOC", "REFDOC", "select * from REFDOC where 0=1", ref strErrorMsg);
            DataRow dtrREFDOC = this.dtsDataEnv.Tables["REFDOC"].NewRow();

            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrEditRowID, this.mstrRefToRefType, inOrderH, inOrderI });
            if (!pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefTo", "REFDOC", "select cRowID from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? and cChildH = ? and cChildI = ? ", ref strErrorMsg))
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
            dtrREFDOC["cChildH"] = inOrderH;
            dtrREFDOC["cChildI"] = inOrderI;
            dtrREFDOC["nQty"] = inQty;
            dtrREFDOC["nUMQty"] = 1;

            string strSQLUpdateStr_RefTo = "";
            DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
            pobjSQLUtil.SetPara(pAPara);
            bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);

            //pmUpdateWOrderOPStep(this.mstrRefToRefType, this.mstrRefToRowID, this.mstrRefToMOrderOP);
            return bllResult;
        }

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            this.pmReleaseMO();
        }

        private void pmReleaseMO()
        {

            //Common.dlgBOMViewCost dlg2 = new BeSmartMRP.DatabaseForms.Common.dlgBOMViewCost();
            //dlg2.BindData(this.dtsDataEnv, this.mstrTemPd);
            //dlg2.ShowDialog();

            using (Transaction.Common.MRP.dlgBrowReleaseMO dlg = new Transaction.Common.MRP.dlgBrowReleaseMO())
            {
                dlg.BranchID = this.mstrBranch;
                dlg.BindData(this.dtsDataEnv, this.mstrTemPd);
                dlg.ShowDialog();
            }

            //using (Transaction.Common.dlgSelOption01 dlg = new Transaction.Common.dlgSelOption01())
            //{

            //    dlg.lsbOption.Visible = true;
            //    dlg.lsbOption.Items.Clear();
            //    dlg.lsbOption.Items.AddRange(new object[] { 
            //                                                                  UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
            //                                                                , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
            //                                                                , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
            //                                                                , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });
            //    dlg.lsbOption.SelectedIndex = 0;

            //    dlg.ShowDialog();
            //    if (dlg.DialogResult == DialogResult.OK)
            //    {
            //        this.mintPrint_RMWHCost = dlg.lsbOption.SelectedIndex;
            //        this.pmPSumBOMCost();
            //        Common.dlgBOMViewCost dlg2 = new BeSmartMRP.DatabaseForms.Common.dlgBOMViewCost();
            //        dlg2.BindData(this.dtsDataEnv, this.mstrTemPd);
            //        dlg2.ShowDialog();
            //    }
            //}
        }

        private void btnSumCost_Click(object sender, EventArgs e)
        {
            this.pmCalCost();
        }

        private void pmCalCost()
        {
            using (Transaction.Common.dlgSelOption01 dlg = new Transaction.Common.dlgSelOption01())
            {

                dlg.lsbOption.Visible = true;
                dlg.lsbOption.Items.Clear();
                dlg.lsbOption.Items.AddRange(new object[] { 
                                                                              UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });
                dlg.lsbOption.SelectedIndex = 0;

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    this.mintPrint_RMWHCost = dlg.lsbOption.SelectedIndex;
                    this.pmPSumBOMCost();
                    DatabaseForms.Common.dlgBOMViewCost dlg2 = new DatabaseForms.Common.dlgBOMViewCost();
                    dlg2.BindData(this.dtsDataEnv, this.mstrTemPd, true);
                    dlg2.ShowDialog();
                }
            }
        }

        private decimal mdecSumBOMAmt = 0;
        private decimal mdecSumBOMOPAmt = 0;
        private int mintPrint_RMWHCost = 0;

        private void pmPSumBOMCost()
        {


            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            decimal decStdCost = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {

                decimal decStkQty = this.pmGetStockBal(dtrTemPd["cProd"].ToString(), this.mstrBook_WHouse_RM);
                //decimal decStkQty = this.GetStockBalance(dtrTemPd["cProd"].ToString(), dtrTemPd["cWHouse"].ToString(), dtrTemPd["cLot"].ToString());
                dtrTemPd["nBalQty"] = decStkQty;

                switch (this.mintPrint_RMWHCost)
                {
                    case 0:
                        pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName, fnStdCost from PROD where fcSkid = ?", ref strErrorMsg))
                        {
                            decStdCost = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["fnStdCost"]);
                        }
                        dtrTemPd["nCost"] = decStdCost;
                        break;
                    default:
                        dtrTemPd["nCost"] = this.pmGetBuyPrice(dtrTemPd["cProd"].ToString(), this.mintPrint_RMWHCost);
                        break;
                }

                if (dtrTemPd["cProcure"].ToString() == "M")
                {
                    this.mdecSumBOMAmt = 0;
                    this.mdecSumBOMOPAmt = 0;
                    this.pmGetBOMCost(0, dtrTemPd["cMfgBOMHD"].ToString(), 1);
                    dtrTemPd["nCost"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt;
                }
            }
        }

        public decimal GetStockBalance(string inProd, string inWHouse, string inLot)
        {
            return this.mStockAgent.GetStockBalance(this.mstrBranch, inProd, inWHouse, "", inLot);
        }

        private void pmGetBOMCost(int inLevel, string inBOMHD, decimal inMfgQty)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStrBOMIT_Pd = "select * from " + MapTable.Table.MFBOMIT_PD + " where cBOMHD = ? order by cSeq";

            //TODO: Sum OP Cost Here

            string strSQLStr = "";
            strSQLStr = "select ";

            strSQLStr += " MFBOMHD.CROWID, MFBOMHD.CCODE, MFBOMHD.CNAME, MFBOMHD.CMFGPROD, MFBOMHD.NMFGQTY ";
            strSQLStr += " ,MFBOMIT_STDOP.NT_QUEUE, MFBOMIT_STDOP.NT_SETUP , MFBOMIT_STDOP.NT_PROCESS, MFBOMIT_STDOP.NT_TEAR, MFBOMIT_STDOP.NT_WAIT, MFBOMIT_STDOP.NT_MOVE ";
            strSQLStr += " ,MFBOMIT_STDOP.COPSEQ, MFBOMIT_STDOP.CROWID as CSTDOP ";
            strSQLStr += ",MFSTDOPR.CCODE as QCSTDOPR, MFSTDOPR.CNAME as QNSTDOPR";
            strSQLStr += ",MFWKCTRHD.CCODE as QCWKCTR, MFWKCTRHD.CNAME as QNWKCTR";
            strSQLStr += ",MFRESOURCE.CCODE as QCRESOURCE, MFRESOURCE.CNAME as QNRESOURCE";
            strSQLStr += " from MFBOMHD ";
            strSQLStr += " left join MFBOMIT_STDOP on MFBOMIT_STDOP.CBOMHD = MFBOMHD.CROWID ";
            strSQLStr += " left join MFSTDOPR on MFSTDOPR.CROWID = MFBOMIT_STDOP.CMOPR ";
            strSQLStr += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFBOMIT_STDOP.CWKCTRH ";
            strSQLStr += " left join MFRESOURCE on MFRESOURCE.CROWID = MFBOMIT_STDOP.CRESOURCE ";
            strSQLStr += " where MFBOMHD.CROWID = ? ";
            strSQLStr += " order by MFBOMIT_STDOP.COPSEQ";

            decimal decFactorQty = 0;
            DataRow dtrBomH = null;
            pobjSQLUtil.SetPara(new object[] { inBOMHD });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOMHD", "MFBOMHD", strSQLStr, ref strErrorMsg))
            {
                dtrBomH = this.dtsDataEnv.Tables["QBOMHD"].Rows[0];
                decFactorQty = inMfgQty / this.pmToDecimal(dtrBomH[QMFBOMInfo.Field.MfgQty]);

                foreach (DataRow dtrPBOMHD in this.dtsDataEnv.Tables["QBOMHD"].Rows)
                {
                    //Print OP COST
                    decimal decTimeFactor = 1;
                    decimal decOPTime = this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Queue])
                        + this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_SetUp])
                        + (this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor)
                        + this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Tear])
                        + this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Wait])
                        + this.pmToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Move]);

                    //Get OP Cost from COSTLINE Table
                    QCostLineInfo oCostLine = new QCostLineInfo();

                    //Load Cost by OP
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "O");

                    decimal decOPCost_OP = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    //Load Cost by WorkCenter
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "W");
                    decimal decOPCost_WC = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    //Load Cost by Tool (Resource)
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "T");
                    decimal decOPCost_Tool = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    this.mdecSumBOMOPAmt += (decFactorQty * (decOPCost_OP + decOPCost_WC + decOPCost_Tool));
                }
            }

            pobjSQLUtil.SetPara(new object[] { inBOMHD });

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOMI", "BOMIT", strSQLStrBOMIT_Pd, ref strErrorMsg);

            #region "Print RM Item"
            foreach (DataRow dtrBOMItem in this.dtsDataEnv.Tables["QBOMI"].Rows)
            {

                int intLevel = inLevel + 1;

                //inMfgQty
                decimal decMfgQty = decFactorQty * this.pmToDecimal(dtrBOMItem["nQty"]);
                decimal decPrice = 0;

                if (this.mintPrint_RMWHCost == 0)
                {
                    pobjSQLUtil2.SetPara(new object[] { dtrBOMItem["cProd"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ? ", ref strErrorMsg))
                    {
                        decPrice = this.pmToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["FNSTDCOST"]);
                    }
                }
                else
                {
                    decPrice = this.pmGetBuyPrice(dtrBOMItem["cProd"].ToString(), this.mintPrint_RMWHCost);
                }

                this.mdecSumBOMAmt += decMfgQty * decPrice;

                if (dtrBOMItem["cProcure"].ToString() == "M")
                {
                    this.pmGetBOMCost(intLevel, dtrBOMItem["cMfgBOMHD"].ToString(), decMfgQty);
                }

            }
            #endregion

        }

        private string mstrSaleOrBuy = "P";

        private decimal pmGetBuyPrice(string inProd, int inWhPrice)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //inWhPrice = 1; ราคาซื้อล่าสุด
            //inWhPrice = 2; ราคาซื้อต่ำสุด
            //inWhPrice = 3; ราคาซื้อสูงสุด

            decimal decRetVal = 0;
            string strMsg = "";

            string strSaleOrBuy = (this.mstrSaleOrBuy == "S" ? "ราคาขาย" : "ราคาซื้อ");
            string strMaxRef = "";
            string strMinRef = "";
            string strLastRef = "";
            decimal decLastAmt = 0;
            decimal decMaxAmt = 0;
            decimal decMinAmt = Convert.ToDecimal(999999999.99);
            DateTime dttLastDate = DateTime.MinValue;
            DateTime dttCurDate = DateTime.Now;

            string strBranch = "";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select top 1 * from Branch where fcCorp = ? order by fcCode ", ref strErrorMsg))
            {
                strBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            string strProd = inProd;
            if (strProd != "" && this.mstrSaleOrBuy != "S")
            {
                pobjSQLUtil.SetPara(new object[] { strProd, strBranch });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from RefProd where fcProd = ? and fcBranch = ? ", ref strErrorMsg))
                {
                    foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefProd"].Rows)
                    {
                        if ((SysDef.gc_RFTYPE_INV_BUY + SysDef.gc_RFTYPE_DR_BUY).IndexOf(dtrOrderI["fcRfType"].ToString()) > -1)
                        {
                            decimal decUmQty = (Convert.ToDecimal(dtrOrderI["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(dtrOrderI["fnUmQty"]));
                            #region "Cal Min&Max Buy Price"
                            if (Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty >= decMaxAmt)
                            {
                                //ราคาซื้อสูงสุด
                                decMaxAmt = Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty;
                                strMaxRef = dtrOrderI["fcSkid"].ToString();
                            }
                            if (Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty <= decMinAmt && Convert.ToDecimal(dtrOrderI["fnPrice"]) != 0)
                            {
                                //ราคาซื้อต่ำสุด
                                decMinAmt = Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty;
                                strMinRef = dtrOrderI["fcSkid"].ToString();
                            }

                            if (Convert.ToDateTime(dtrOrderI["fdDate"]).Date.CompareTo(dttLastDate.Date) > 0
                                || (Convert.ToDateTime(dtrOrderI["fdDate"]).Date.CompareTo(dttLastDate.Date) == 0 && Convert.ToDateTime(dtrOrderI["ftDateTime"]).CompareTo(dttCurDate) > 0)
                                && Convert.ToDecimal(dtrOrderI["fnPrice"]) != 0)
                            {
                                dttLastDate = Convert.ToDateTime(dtrOrderI["fdDate"]);
                                dttCurDate = Convert.ToDateTime(dtrOrderI["ftDateTime"]);
                                strLastRef = dtrOrderI["fcSkid"].ToString();
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
                    decLastAmt = Convert.ToDecimal(dtrTem["fnPrice"]);
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

            switch (inWhPrice)
            {
                case 1:
                    decRetVal = decLastAmt;
                    break;
                case 2:
                    decRetVal = decMinAmt;
                    break;
                case 3:
                    decRetVal = decMaxAmt;
                    break;
            }
            return decRetVal;
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

        private void pmRecalTotPd()
        {

            decimal decSumRMCost = 0;
            decimal decSumFGQty = 0;

            //this.mstrTemFG
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {

                //if (dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
                if (true)
                {

                    //nStdCost
                    decimal decQty = 0; decimal decStdCost = 0;
                    decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                    decStdCost = (Convert.IsDBNull(dtrTemPd["nStdCost"]) ? 0 : Convert.ToDecimal(dtrTemPd["nStdCost"]));
                    decSumRMCost += (decQty * decStdCost);
                }
            }

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemFG].Rows)
            {

                if (true)
                {
                    decimal decQty = 0;
                    decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                    decSumFGQty += decQty;
                }
            }

            if (decSumFGQty != 0)
            {
                this.txtSumStdCost.Value = decSumRMCost / decSumFGQty;
            }

        }

        private void pmLoadWorkCalendar(DateTime inMODate)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            int intYear = inMODate.Year;

            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrPlant });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select * from EMPLANT where cRowID = ? ", ref strErrorMsg))
            {

                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                string strWorkHour = dtrPlant["cWorkHour"].ToString();
                string strHoliday = dtrPlant["cHoliday"].ToString();

                string strSQLStr = "select * from EMWORKCALENDAR";
                strSQLStr += " where EMWORKCALENDAR.CCORP = ? and EMWORKCALENDAR.CPLANT = ? ";
                strSQLStr += " and EMWORKCALENDAR.CWORKHOUR = ? and EMWORKCALENDAR.NYEAR = ?";

                string strSQLStr2 = "select * from EMWORKCALENDARIT_RANGE where CWORKCALH = ? order by DDATE,CSEQ";

                //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant, strWorkHour, strHoliday, intYear });
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant, strWorkHour, intYear });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWorkCal", "EMWORKCALENDAR", strSQLStr, ref strErrorMsg))
                {
                    DataRow dtrWorkCal = this.dtsDataEnv.Tables["QWorkCal"].Rows[0];
                    objSQLHelper.SetPara(new object[] { dtrWorkCal["cRowID"] });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWorkCalIT_Range", "EMWORKCALENDARIT_RANGE", strSQLStr2, ref strErrorMsg))
                    {
                        //DataRow dtrWorkCal = this.dtsDataEnv.Tables["QWorkCal"].Rows[0];
                        //pmAddRow_WkHour
                        foreach (DataRow dtrWorkCal_Range in this.dtsDataEnv.Tables["QWorkCalIT_Range"].Rows)
                        {
                            pmAddRow_WkHour(dtrWorkCal_Range["cType"].ToString(), Convert.ToDateTime(dtrWorkCal_Range["dDate"]), Convert.ToDateTime(dtrWorkCal_Range["dBegTime"]), Convert.ToDateTime(dtrWorkCal_Range["dEndTime"]));
                        }

                    }
                }
                

            }

        }

        private void pmAddRow_MOrderOP(string inRowID, string inOPSeq, string inOPName, string inWkCtrH, decimal inFactorQty, decimal inProcTime, decimal inSetUpTime)
        {
            DataRow dr = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
            dr["cRowID"] = inRowID;
            dr["ProcTime"] = inProcTime;
            dr["SetUpTime"] = inSetUpTime;
            dr["OPSeq"] = inOPSeq;
            dr["OPName"] = inOPName;
            dr["WkCtrH"] = inWkCtrH;

            //dr["HourUsage"] = (inSetUpTime + (inProcTime * inFactorQty)) / 60;
            dr["HourUsage"] = (inSetUpTime + (inProcTime * inFactorQty)) / 3600;

            this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dr);
        }

        private void pmAddRow_WkHour(string inType, DateTime inDate, DateTime inBegDate, DateTime inEndDate)
        {

            DataRow dr = this.dtsDataEnv.Tables[mstrTemWorkCal].NewRow();
            dr["CTYPE"] = inType;
            dr["DDATE"] = inDate;
            dr["DBEGTIME"] = inBegDate;
            dr["DENDTIME"] = inEndDate;
            dr["DCURRTIME"] = inEndDate;
            dr["CDATE"] = inDate.ToString("yyyyMMdd");

            TimeSpan ts = inEndDate - inBegDate;
            dr["TotWkHour"] = Convert.ToDecimal(ts.TotalHours);
            dr["BalWkHour"] = Convert.ToDecimal(ts.TotalHours);

            this.dtsDataEnv.Tables[mstrTemWorkCal].Rows.Add(dr);
        }

        private void pmAddRow_UsageHour(string inWOrderOP, string inOPSeq, string inWkCtrH, string inType, decimal inUseTime, DateTime inDate, DateTime inBegDate, DateTime inEndDate)
        {

            DataRow dr = this.dtsDataEnv.Tables[this.mstrTemUsage].NewRow();
            dr["CWORDERI_OP"] = inWOrderOP;
            dr["COPSEQ"] = inOPSeq;
            dr["CWKCTRH"] = inWkCtrH;
            dr["CTYPE"] = inType;
            dr["DDATE"] = inDate;
            dr["CDATE"] = inDate.ToString("yyyyMMdd");
            dr["DBEGTIME"] = inBegDate;
            dr["DENDTIME"] = inEndDate;
            dr["NUSEHOUR"] = inUseTime;
            this.dtsDataEnv.Tables["TemUsage"].Rows.Add(dr);

        }

        private DataRow pmReleaseHour(string inRowID, string inOPSeq, string inWkCtrH, ref DateTime ioDueDate, decimal inUseHour)
        {
            DataRow dtrRetRow = null;
            decimal decUseHour = inUseHour;
            //DataRow[] dtrSel = dtsDataEnv.Tables[this.mstrTemWorkCal].Select("cDate <= '" + ioDueDate.ToString("yyyyMMdd") + " and BalWkHour > 0'", "dDate desc,cType desc");
            DataRow[] dtrSel = dtsDataEnv.Tables[this.mstrTemWorkCal].Select("cDate <= '" + ioDueDate.ToString("yyyyMMdd") + " and BalWkHour > 0'", "dDate desc");

            for (int i = 0; i < dtrSel.Length; i++)
            {
                DataRow dtrWorkCal = dtrSel[i];

                decimal decBalWkHor = Convert.ToDecimal(dtrWorkCal["BalWkHour"]);
                if (decBalWkHor >= decUseHour)
                {
                    dtrWorkCal["BalWkHour"] = Convert.ToDecimal(dtrWorkCal["BalWkHour"]) - decUseHour;
                    DateTime dttSaveEndDate = Convert.ToDateTime(dtrWorkCal["DCURRTIME"]);
                    dtrWorkCal["DCURRTIME"] = Convert.ToDateTime(dtrWorkCal["DCURRTIME"]).AddHours((double)(decUseHour * -1));
                    this.pmAddRow_UsageHour(inRowID, inOPSeq, inWkCtrH, dtrWorkCal["cType"].ToString(), decUseHour, Convert.ToDateTime(dtrWorkCal["DDATE"]), Convert.ToDateTime(dtrWorkCal["DCURRTIME"]), dttSaveEndDate);
                    ioDueDate = Convert.ToDateTime(dtrWorkCal["DDATE"]);
                    dtrRetRow = dtrWorkCal;
                    break;
                }
                else
                {
                    this.pmAddRow_UsageHour(inRowID, inOPSeq, inWkCtrH, dtrWorkCal["cType"].ToString(), Convert.ToDecimal(dtrWorkCal["BalWkHour"]), Convert.ToDateTime(dtrWorkCal["DDATE"]), Convert.ToDateTime(dtrWorkCal["DBEGTIME"]), Convert.ToDateTime(dtrWorkCal["DCURRTIME"]));
                    decUseHour = decUseHour - Convert.ToDecimal(dtrWorkCal["BalWkHour"]);
                    dtrWorkCal["DCURRTIME"] = Convert.ToDateTime(dtrWorkCal["DBEGTIME"]);
                    dtrWorkCal["BalWkHour"] = 0;

                    if (i + 1 < dtrSel.Length)
                    {
                        ioDueDate = Convert.ToDateTime(dtrSel[i + 1]["DDATE"]);
                    }
                }

            }
            return dtrRetRow;

        }

    }
}
