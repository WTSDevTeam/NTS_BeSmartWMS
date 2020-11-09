
//#define xd_RUNMODE_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

    /// <summary>
    /// Create By : Thanapon Rauntongjarat
    /// Create Date : 01/12/2009
    /// Update Log Format :
    ///        Change Date | By : XXX | JOBNO_YYMMXXX | Type | Description;
    /// Update Log :
    ///        24/05/10 | By : Yod | 1005001 | แก้ BUG | เรื่องการ Update cParent;
    /// Update Log End :
    /// </summary>
    public partial class frmMFWClose : UIHelper.frmBase
    {

        public static string TASKNAME = "EMCLOSE";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;
        private const int xd_PAGE_EDIT2 = 2;
        private const int xd_PAGE_EDIT3 = 3;
        private const int xd_PAGE_EDIT4 = 4;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWCloseHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWCloseIT_OP;
        private string mstrITable2 = MapTable.Table.MFWCloseIT_PD;

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QMFWCloseHDInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";

        private string mstrBook_WHouse_FG = "";

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrActiveAlias = "TemPd";
        private string mstrTemFG = "TemFG";
        private string mstrTemPd = "TemPd";
        private string mstrTemOP = "TemOP";
        private string mstrTemPdX1 = "TemPdX1";
        private string mstrTemRefTo = "TemRefTo";

        private string mstrTemPd_GenPR = "TemPd_GenPR";
        private string mstrGenPR_Branch = "";
        private string mstrGenPR_Book = "";
        private string mstrGenPR_Coor = "";
        private string mstrGenPR_Code = "";
        private string mstrGenPR_RefNo = "";
        private DateTime mdttGenPR_Date = DateTime.Now;
        private int mintGenPR_CredTerm = 0;

        private string mstrGenFG_Branch = "";
        private string mstrGenFG_Book = "";
        private string mstrGenFG_WHouse = "";
        private string mstrGenFG_Lot = "";
        private decimal mdecGenFG_Qty = 0;
        private DateTime mdttGenFG_Date = DateTime.Now;

        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private string mstrTemGenFG = "TemGenFG";
        private string mstrTemGenPd = "TemGenPd";
        private string mstrTemGenOP = "TemGenOP";

        private string mstrEditRowID = "";
        private string mstrSaveRowID = "";
        private string mstrEditRowID_PR = "";
        private string mstrRefToTab = QMFWOrderHDInfo.TableName;
        private string mstrRefToTabI = MapTable.Table.MFWOrderIT_OP;
        private string mstrRefToTabI2 = MapTable.Table.MFWOrderIT_PD;
        private string mstrRefType = DocumentType.MC.ToString();
        private string mstrRefToRefType = DocumentType.MO.ToString();
        private string mstrRefToBook = "";
        private string mstrRefToRowID = "";
        private string mstrOldRefToRowID = "";

        private string mstrRfType = "M";
        private DocumentType mRefType = DocumentType.MC;
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
        private decimal mdecSumFRQty = 0;
        private decimal mdecSumMOQty = 0;

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

        public frmMFWClose()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFWClose(FormActiveMode inMode)
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
        
        private static frmMFWClose mInstanse_1 = null;

        public static frmMFWClose GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFWClose();
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

        private void frmMFWClose_Load(object sender, EventArgs e)
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

            this.mstrFormMenuName = UIBase.GetAppUIText(new string[] { "เอกสารใบปิดการผลิต [MC]", "CLOSE MANUFACTURING ORDER [MC]" });
            this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            this.lblCode.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Doc. Code" }) });
            this.lblRefNo.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." }) });
            this.lblDate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Doc. Date" }) });

            this.lblApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "อนุมัติโดย", "Approve By" }) });
            this.lblDApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่", "Date" }) });
            this.lblLastUpdate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "แก้ไขล่าสุด", "Last Update " }) });
            this.lblLastUpdBy.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "โดย", "By" }) });

            this.lblMfgProd.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "สินค้าที่สั่งผลิต", "Product Output" }) });
            this.lblMfgQty.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "จำนวนที่สั่งผลิต", "MO. Qty." }) });
            this.lblMfgUM.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "หน่วยนับ", "Unit" }) });
            this.lblTotMfgQty.Text = UIBase.GetAppUIText(new string[] { "รวมจำนวนที่สั่งผลิต :", "Mfg. Qty. :" });
            this.lblQcCoor.Text = UIBase.GetAppUIText(new string[] { "รหัสลูกค้า :", "Customer Code :" });
            this.lblQnCoor.Text = UIBase.GetAppUIText(new string[] { "ชื่อลูกค้า :", "Name :" });
            this.lblRefer.Text = UIBase.GetAppUIText(new string[] { "#อ้างอิง M/O :", "#Refer M/O :" });
            this.lblStartDate.Text = UIBase.GetAppUIText(new string[] { "วันที่เริ่มต้น :", "Start Date :" });
            this.lblDueDate.Text = UIBase.GetAppUIText(new string[] { "วันที่เสร็จ :", "Finish Date :" });

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });
            this.lblBalFG.Text = UIBase.GetAppUIText(new string[] { "คงคลัง :", "Stock :" });
            this.lblSOQty.Text = UIBase.GetAppUIText(new string[] { "จำนวน SO :", "SO Qty :" });

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWCloseHDInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWCloseHDInfo.Field.RefNo);

            this.txtRemark1.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark1);
            this.txtRemark2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark2);
            this.txtRemark3.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark3);
            this.txtRemark4.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark4);
            this.txtRemark5.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark5);
            this.txtRemark6.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark6);
            this.txtRemark7.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark7);
            this.txtRemark8.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark8);
            this.txtRemark9.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark9);
            this.txtRemark10.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFWCloseHDInfo.TableName, QMFWCloseHDInfo.Field.Remark10);

        }

        private void pmMapEvent()
        {
            this.Resize += new EventHandler(frmMFWClose_Resize);
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

        private void frmMFWClose_Resize(object sender, EventArgs e)
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
            
            string strStep = "CSTEP = case WCLOSEH.CSTEP when '1' then '' when 'L' then 'CLOSED' end";

            string strIsApprove = "CISAPPROVE = case ";
            strIsApprove += " when WCLOSEH.CISAPPROVE = 'A' then 'APPROVE' ";
            strIsApprove += " when WCLOSEH.CISAPPROVE = ' ' then '' ";
            strIsApprove += " end ";

            string strSQLExec = "";

            strSQLExec = "select WCLOSEH.CROWID, WCLOSEH.CSTAT, " + strIsApprove + "," + strStep + ", WCLOSEH.CCODE, WCLOSEH.CREFNO, WCLOSEH.DDATE ";
            strSQLExec += " , PROD.FCCODE AS QCPROD , PROD.FCNAME AS QNPROD ";
            strSQLExec += " , WCLOSEH.DCREATE, WCLOSEH.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} WCLOSEH ";
            strSQLExec += " left join " + strProdTab + " PROD on PROD.FCSKID = WCLOSEH.CMFGPROD ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = WCLOSEH.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = WCLOSEH.CLASTUPDBY ";
            strSQLExec += " where WCLOSEH.CCORP = ? and WCLOSEH.CBRANCH = ? and WCLOSEH.CPLANT = ? and WCLOSEH.CREFTYPE = ? and WCLOSEH.CMFGBOOK = ? and WCLOSEH.DDATE between ? and ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "WCLOSEH", strSQLExec, ref strErrorMsg);

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

            //this.gridView4.Columns["cRef2RefTy"].VisibleIndex = i++;
            this.gridView4.Columns["cRefTo_Code"].VisibleIndex = i++;

            this.gridView4.Columns["nMOQty"].VisibleIndex = i++;
            this.gridView4.Columns["nQty"].VisibleIndex = i++;
            this.gridView4.Columns["cQnUOM"].VisibleIndex = i++;
            //this.gridView4.Columns["cScrap"].VisibleIndex = i++;

            this.gridView4.Columns["nRecNo"].Caption = "No.";
            this.gridView4.Columns["cPdType"].Caption = "T";
            this.gridView4.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView4.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView4.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView4.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "MO Qty.", "MO Qty." });
            this.gridView4.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView4.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView4.Columns["cRef2RefTy"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#RefType", "#RefType" });
            this.gridView4.Columns["cRefTo_Code"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#RefCode", "#RefCode" });

            this.gridView4.Columns["nRecNo"].Width = 50;
            this.gridView4.Columns["cPdType"].Width = 50;
            this.gridView4.Columns["cQcProd"].Width = 130;
            this.gridView4.Columns["cRemark1"].Width = 80;
            this.gridView4.Columns["cRef2RefTy"].Width = 60;
            this.gridView4.Columns["cRefTo_Code"].Width = 120;
            this.gridView4.Columns["nMOQty"].Width = 80;
            this.gridView4.Columns["nQty"].Width = 80;
            this.gridView4.Columns["cQnUOM"].Width = 80;
            //this.gridView4.Columns["cSubSti"].Width = 5;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.gridView4.Columns["nMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nMOQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView4.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView4.Columns["cRef2RefTy"].OptionsColumn.ReadOnly = true;
            this.gridView4.Columns["cRefTo_Code"].OptionsColumn.ReadOnly = true;

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

            this.gridView4.Columns["cRef2RefTy"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.gridView4.Columns["cRefTo_Code"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;

            this.gridView4.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView4.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView4.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView4.Columns["cRemark1"].ColumnEdit = this.grcRemark2;

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

            //this.gridView3.Columns["cRef2RefTy"].VisibleIndex = i++;
            this.gridView3.Columns["cRefTo_Code"].VisibleIndex = i++;

            this.gridView3.Columns["nMOQty"].VisibleIndex = i++;
            this.gridView3.Columns["nQty"].VisibleIndex = i++;
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
            this.gridView3.Columns["nMOQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "MO Qty.", "MO Qty." });
            this.gridView3.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView3.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView3.Columns["cScrap"].Caption = "Scrap";
            this.gridView3.Columns["cProcure"].Caption = "Procure";
            this.gridView3.Columns["cSubSti"].Caption = "S";
            this.gridView3.Columns["cQcBOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BOM", "BOM" });
            this.gridView3.Columns["cRoundCtrl"].Caption = "Round";

            this.gridView3.Columns["cRef2RefTy"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#RefType", "#RefType" });
            this.gridView3.Columns["cRefTo_Code"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "#RefCode", "#RefCode" });

            this.gridView3.Columns["nRecNo"].Width = 50;
            this.gridView3.Columns["cRef2RefTy"].Width = 60;
            this.gridView3.Columns["cRefTo_Code"].Width = 120;
            this.gridView3.Columns["cOPSeq"].Width = 80;
            this.gridView3.Columns["cPdType"].Width = 50;
            this.gridView3.Columns["cQcProd"].Width = 130;
            this.gridView3.Columns["cRemark1"].Width = 80;
            this.gridView3.Columns["nMOQty"].Width = 80;
            this.gridView3.Columns["nQty"].Width = 80;
            this.gridView3.Columns["cQnUOM"].Width = 80;
            this.gridView3.Columns["cScrap"].Width = 80;
            this.gridView3.Columns["cProcure"].Width = 60;
            this.gridView3.Columns["cQcBOM"].Width = 130;
            //this.gridView3.Columns["cSubSti"].Width = 5;
            this.gridView3.Columns["cRoundCtrl"].Width = 60;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.gridView3.Columns["nMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nMOQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView3.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

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

            this.gridView3.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cRef2RefTy"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.gridView3.Columns["cRefTo_Code"].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;

            this.gridView3.Columns["cRef2RefTy"].OptionsColumn.ReadOnly = true;
            this.gridView3.Columns["cRefTo_Code"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView3.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView3.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView3.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView3.Columns["cProcure"].ColumnEdit = this.grcProcure;
            this.gridView3.Columns["cScrap"].ColumnEdit = this.grcScrap;
            this.gridView3.Columns["cQcBOM"].ColumnEdit = this.grcQcBOM;
            this.gridView3.Columns["cRoundCtrl"].ColumnEdit = this.grcRoundCtrl;

            this.pmCalcColWidth2();
        }

        private void pmInitGridProp_TemOP()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemOP].DefaultView;

            this.grdTemOP.DataSource = this.dtsDataEnv.Tables[this.mstrTemOP];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
                this.gridView2.Columns[intCnt].OptionsColumn.ReadOnly = true;
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

            this.gridView2.Columns["nQty"].VisibleIndex = i++;
            this.gridView2.Columns["nWasteQty"].VisibleIndex = i++;
            this.gridView2.Columns["nLossQty1"].VisibleIndex = i++;

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
            this.gridView2.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ของดี", "Good" });
            this.gridView2.Columns["nWasteQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ของเสีย", "N/G" });
            this.gridView2.Columns["nLossQty1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "สูญเสีย", "Loss" });
            //this.gridView2.Columns["nScrapQty1"].Caption = "% Scrap";
            //this.gridView2.Columns["nLossQty1"].Caption = "% Loss";
            //this.gridView2.Columns["cQcWHouse2"].Caption = "คลัง Scrap";

            this.gridView2.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["nWasteQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nWasteQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["nLossQty1"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nLossQty1"].DisplayFormat.FormatString = "#,###,###.00";

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
            this.gridView2.Columns["nQty"].Width = 80;
            this.gridView2.Columns["nWasteQty"].Width = 80;
            this.gridView2.Columns["nLossQty1"].Width = 80;

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

            this.gridView2.Columns["cPopGetTime"].OptionsColumn.ReadOnly = false;

            this.grcQcRemark.Buttons[0].Enabled = false;
            this.grcQcMOPR.Buttons[0].Enabled = false;
            this.grcQcWkCtr.Buttons[0].Enabled = false;
            this.grcQcResource.Buttons[0].Enabled = false;
            this.grcQcResource.Buttons[0].Enabled = false;

            this.gridView2.Columns["cOPSeq"].ColumnEdit = this.grcOPSeq;
            this.gridView2.Columns["cNextOP"].ColumnEdit = this.grcNextOPSeq;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView2.Columns["cQcRemark1"].ColumnEdit = this.grcQcRemark;
            this.gridView2.Columns["cQcMOPR"].ColumnEdit = this.grcQcMOPR;
            this.gridView2.Columns["cQcWkCtrH"].ColumnEdit = this.grcQcWkCtr;
            this.gridView2.Columns["cPopGetTime"].ColumnEdit = this.grcOPTime;
            this.gridView2.Columns["cQcResource"].ColumnEdit = this.grcQcResource;

            this.gridView2.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView2.Columns["nWasteQty"].ColumnEdit = this.grcQty;
            this.gridView2.Columns["nLossQty1"].ColumnEdit = this.grcQty;
            
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
                                    + this.gridView2.Columns["nQty"].Width
                                    + this.gridView2.Columns["nWasteQty"].Width
                                    + this.gridView2.Columns["nLossQty1"].Width
                                    + this.gridView2.Columns["cQcWkCtrH"].Width
                                    + this.gridView2.Columns["cQnWkCtrH"].Width;

            int intNewWidth = this.Width - intColWidth - 50;
            this.gridView2.Columns["cRemark1"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth1()
        {

            int intColWidth = this.gridView1.Columns[QMFWCloseHDInfo.Field.IsApprove].Width
                                    + this.gridView1.Columns[QMFWCloseHDInfo.Field.Stat].Width
                                    + this.gridView1.Columns[QMFWCloseHDInfo.Field.Step].Width
                                    + this.gridView1.Columns[QMFWCloseHDInfo.Field.Code].Width
                                    + this.gridView1.Columns[QMFWCloseHDInfo.Field.RefNo].Width
                                    + this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].Width;

            //int intNewWidth = this.Width - intColWidth - 60;
            //this.gridView3.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmCalcColWidth2()
        {


            int intColWidth = this.gridView3.Columns["nRecNo"].Width
                                    //+ this.gridView3.Columns["cRef2RefTy"].Width
                                    + this.gridView3.Columns["cRefTo_Code"].Width
                                    + this.gridView3.Columns["cOPSeq"].Width
                                    + this.gridView3.Columns["cPdType"].Width
                                    + this.gridView3.Columns["cQcProd"].Width
                                    + this.gridView3.Columns["cRemark1"].Width
                                    + this.gridView3.Columns["cScrap"].Width
                                    + this.gridView3.Columns["nMOQty"].Width
                                    + this.gridView3.Columns["nQty"].Width
                                    + this.gridView3.Columns["cQnUOM"].Width
                                    + this.gridView3.Columns["cProcure"].Width
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
                                    //+ this.gridView4.Columns["cRef2RefTy"].Width
                                    + this.gridView4.Columns["cRefTo_Code"].Width
                                    + this.gridView4.Columns["nMOQty"].Width
                                    + this.gridView4.Columns["nQty"].Width
                                    + this.gridView4.Columns["cQnUOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView4.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFWCloseHDInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Stat].VisibleIndex = i++;
            //this.gridView1.Columns[QMFWCloseHDInfo.Field.IsApprove].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Step].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.RefNo].VisibleIndex = i++;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].VisibleIndex = i++;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;

            //this.gridView1.Columns[QMFWCloseHDInfo.Field.IsApprove].Caption = "Status";

            this.gridView1.Columns[QMFWCloseHDInfo.Field.Stat].Width = 50;
            //this.gridView1.Columns[QMFWCloseHDInfo.Field.IsApprove].Width = 75;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Step].Width = 75;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Code].Width = 120;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.RefNo].Width = 150;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].Width = 90;
            this.gridView1.Columns["QCPROD"].Width = 120;

            this.gridView1.Columns[QMFWCloseHDInfo.Field.IsApprove].Caption = "#APV.";
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Stat].Caption = " ";
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Step].Caption = "STEP";
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Code].Caption = UIBase.GetAppUIText(new string[] {"เลขที่ภายใน","DOC. CODE"});
            this.gridView1.Columns[QMFWCloseHDInfo.Field.RefNo].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "PROD. CODE" });

            this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns[QMFWCloseHDInfo.Field.Date].DisplayFormat.FormatString = "dd/MM/yy";

            this.pmSetSortKey(QMFWCloseHDInfo.Field.Code, true);
        
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
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);
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
                            }

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();

                        }
                    }
                    break;

                case "REFTO":
                    using (Common.MRP.dlgGetRefToWO dlgRefTo = new Common.MRP.dlgGetRefToWO(this.mstrRefToTab, this.mstrRefToRefType, this.mstrBranch, this.mstrPlant, this.txtQcCoor.Tag.ToString(), this.mstrRefToBook, this.mstrRefToRowID))
                    {
                        dlgRefTo.RefStepField = "CMSTEP";
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            if (dlgRefTo.RefToDocumentID != string.Empty)
                            {
                                if (this.mstrRefToRowID != dlgRefTo.RefToDocumentID
                                    && this.pmChkChildWOIsClose(dlgRefTo.RefToDocumentID))
                                {
                                    this.pmClearRefTo();
                                    this.mstrRefToBook = dlgRefTo.RefToBookID;
                                    this.mstrRefToRowID = dlgRefTo.RefToDocumentID;
                                    this.txtRefTo.Text = dlgRefTo.RefToDocumentCode.TrimEnd();
                                    this.pmLoadRefToMO(this.mstrRefToRowID);

                                    this.pmSetFormEnable(false);
                                }
                            }
                            else
                            {
                                this.pmClearRefTo();
                            }
                        }
                    }
                    break;

                case "GENFR":
                    using (Common.MRP.dlgGetFROption01 dlgPR = new Common.MRP.dlgGetFROption01("FR"))
                    {

                        this.mstrGenFG_Branch = "";
                        this.mstrGenFG_Book = "";
                        this.mstrGenFG_WHouse = "";
                        this.mstrGenFG_Lot = "";
                        //this.mdecGenFG_Qty = 0;

                        dlgPR.BranchID = this.mstrBranch;
                        dlgPR.Qty = this.mdecGenFG_Qty;
                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenFG_Branch = dlgPR.BranchID;
                            this.mstrGenFG_Book = dlgPR.BookID;
                            this.mstrGenFG_WHouse = dlgPR.WHouseID;
                            this.mstrGenFG_Lot = dlgPR.Lot;
                            this.mdttGenFG_Date = dlgPR.DocDate.Date;
                            this.mdecGenFG_Qty = dlgPR.Qty;
                            if (this.mdecGenFG_Qty > 0)
                            {
                                this.pmGenFGDoc();
                            }
                        }
                    }
                    break;

                case "REFTOXX":
                    using (Common.dlgGetRefToSO dlgRefTo = new Common.dlgGetRefToSO("SO",this.mstrBranch, this.txtQcCoor.Tag.ToString(), this.txtQcProd.Tag.ToString()))
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
                                    this.pmLoadStockBal(dtrProd["fcSkid"].ToString());

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

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Queue]), out intHour, out intMin, out intSec);
                        dlg.SetTime("QUEUE", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp]), out intHour, out intMin, out intSec);
                        dlg.SetTime("SETUP", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Process]), out intHour, out intMin, out intSec);
                        dlg.SetTime("PROCESS", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Tear]), out intHour, out intMin, out intSec);
                        dlg.SetTime("TEAR", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Wait]), out intHour, out intMin, out intSec);
                        dlg.SetTime("WAIT", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
                        dlg.SetTime("MOVE", intHour, intMin, intSec);
                        
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {

                            dlg.GetTime("QUEUE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Queue] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("SETUP", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("PROCESS", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Process] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("TEAR", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Tear] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("WAIT", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Wait] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("MOVE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFWCloseIT_OPInfo.Field.LeadTime_Move] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

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
                        dlgPR.ShowDialog();
                        if (dlgPR.DialogResult == DialogResult.OK)
                        {
                            this.mstrGenPR_Branch = dlgPR.BranchID;
                            this.mstrGenPR_Book = dlgPR.BookID;
                            this.mstrGenPR_Coor = dlgPR.CoorID;
                            this.mdttGenPR_Date = dlgPR.DocDate.Date;
                            //this.pmGenPR(this.mstrSaveRowID);
                        }
                    }
                    break;
            }
        }

        private void pmSetFormEnable(bool inEnable)
        {

            this.txtQcCoor.Enabled = inEnable;
            this.txtQnCoor.Enabled = inEnable;
            this.txtQcProd.Enabled = inEnable;
            this.txtQcBOM.Enabled = inEnable;

            this.txtMfgQty.Enabled = inEnable;
            this.txtScrap.Enabled = inEnable;
            this.txtQnMfgUM.Enabled = inEnable;
            this.txtTotMfgQty.Enabled = inEnable;
            this.cmbPriority.Enabled = inEnable;
            this.txtQcSect.Enabled = inEnable;
            this.txtQcJob.Enabled = inEnable;

        }

        private void pmClearRefTo()
        {
            this.mstrRefToBook = "";
            this.mstrRefToRowID = "";
            this.txtRefTo.Text = "";
        }

        private bool pmChkChildWOIsClose(string inParentWO)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inParentWO });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from " + QMFWOrderHDInfo.TableName + " where " + QMFWOrderHDInfo.Field.ParentID + " = ? order by " + QMFWOrderHDInfo.Field.Code, ref strErrorMsg))
            {
                foreach (DataRow dtrChk in this.dtsDataEnv.Tables["QRefDoc"].Rows)
                {
                    if (dtrChk[QMFWOrderHDInfo.Field.MStep].ToString() != SysDef.gc_REF_STEP_PAY)
                    {

                        string strMsg = UIBase.GetAppUIText(new string[] { 
                            "เอกสาร #WO : " + dtrChk["cRefNo"].ToString() + " ยังไม่ปิดการผลิต\r\nต้องปิดการผลิตให้เสร็จเรียบร้อยก่อน !"
                            , "#WO : " + dtrChk["cRefNo"].ToString() + " Not Close Production\r\nMust be completed before Production close !" });

                        MessageBox.Show(strMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bllResult = false;
                        break;
                    }
                }
            }
            return bllResult;
        }

        private void pmLoadRefToMO(string inWOrderH)
        {

            string strWOrderH = inWOrderH;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrRetVal = null;

            pobjSQLUtil.SetPara(new object[] { strWOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefToTab, this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
            {

                DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefToTab].Rows[0];

                //this.mstrStep = dtrLoadForm[QMFWOrderHDInfo.Field.Step].ToString();

                this.mstrMasterWO = dtrLoadForm[QMFWOrderHDInfo.Field.MasterID].ToString().TrimEnd();
                this.mstrParentWO = dtrLoadForm[QMFWOrderHDInfo.Field.ParentID].ToString().TrimEnd();

                this.mstrLevel = dtrLoadForm[QMFWOrderHDInfo.Field.Level].ToString().TrimEnd();
                this.mstrBatchNo = dtrLoadForm[QMFWOrderHDInfo.Field.BatchNo].ToString().TrimEnd();
                this.mstrBatchRun = dtrLoadForm[QMFWOrderHDInfo.Field.BatchRun].ToString().TrimEnd();

                //if (!Convert.IsDBNull(dtrLoadForm[QMFWOrderHDInfo.Field.StartDate]))
                //{
                //    this.txtStartDate.EditValue = Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.StartDate]).Date;
                //}
                //this.txtDueDate.EditValue = (Convert.IsDBNull(dtrLoadForm[QMFWOrderHDInfo.Field.DueDate]) ? this.txtDate.DateTime.Date : Convert.ToDateTime(dtrLoadForm[QMFWOrderHDInfo.Field.DueDate]).Date);

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
                }

                this.txtQcProd.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.MfgProdID].ToString();
                pobjSQLUtil2.SetPara(new object[1] { this.txtQcProd.Tag.ToString() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode, fcUM from Prod where fcSkid = ?", ref strErrorMsg))
                {
                    DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                    this.txtQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                    this.pmLoadStockBal(dtrProd["fcSkid"].ToString());
                }

                this.txtQcBOM.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.BOMID].ToString().TrimEnd();
                pobjSQLUtil.SetPara(new object[1] { this.txtQcBOM.Tag.ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode, cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                {
                    this.txtQcBOM.Text = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.txtQnBOM.Text = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                    //this.mstrLastQcPdStH = this.txtQcBOM.Text;
                }

                this.txtQnMfgUM.Tag = dtrLoadForm[QMFWOrderHDInfo.Field.MfgUMID].ToString();
                if (pobjSQLUtil2.SetPara(new object[] { this.txtQnMfgUM.Tag.ToString() })
                    && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                {
                    this.txtQnMfgUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                    this.txtQnMfgUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                }

                this.txtMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgQty]);
                this.mdecLastMfgQty = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgQty]);
                this.mdecUMQty = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.MfgUMQty]);
                this.txtTotMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWOrderHDInfo.Field.Qty]);

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

                //TODO: เรื่องการสรุปยอดการผลิตของใบปิด
                this.pmLoadBOMIT_StdOP2(strWOrderH, this.mstrTemOP);

                //style 1
                this.mdecSumFRQty = 0;
                this.mdecSumMOQty = 0;

                this.pmLoadFG(strWOrderH, this.txtQcProd.Tag.ToString(), true);
                this.pmLoadFG(strWOrderH, this.txtQcProd.Tag.ToString(), false);

                //style 2
                //this.pmLoadBOMIT_Pd2(strWOrderH, this.mstrTemFG);
                //this.pmLoadBOMIT_Pd2(strWOrderH, this.mstrTemPd);

                //style 3
                //this.pmLoadBOMIT_Pd2(strWOrderH, this.mstrTemFG);
                //this.pmLoadFG(strWOrderH, this.txtQcProd.Tag.ToString(), false);

                this.mstrLastScrap = this.txtScrap.Text.Trim();
                this.mdecLastMfgQty = Convert.ToDecimal(this.txtMfgQty.EditValue);
                this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);
            }
        }

        private void pmLoadBOMIT_StdOP2(string inWOrderH, string inAlias)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { inWOrderH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBomIT_StdOP", "WkCtrIT", "select * from " + this.mstrRefToTabI + " where cWOrderH = ? order by cSeq", ref strErrorMsg))
            {

                foreach (DataRow dtrBomIT in this.dtsDataEnv.Tables["QBomIT_StdOP"].Rows)
                {

                    DataRow dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    //dtrNewRow["cRowID"] = dtrBomIT["cRowID"].ToString();
                    dtrNewRow["cRowID"] = "";
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

                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Queue]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Process]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Tear]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Wait]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Move]);

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

                    //Sum ของดี/ของเสีย/สึกหรอ
                    decimal decGoodQty = 0; decimal decWasteQty = 0; decimal decLossQty = 0;
                    this.pmSumWCTranI(this.mstrRefToRowID, dtrBomIT["cRowID"].ToString(), "O", ref decGoodQty, ref decWasteQty, ref decLossQty);
                    dtrNewRow["nQty"] = decGoodQty;
                    dtrNewRow["nWasteQty"] = decWasteQty;
                    dtrNewRow["nLossQty1"] = decLossQty;

                    this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);

                    this.pmLoadStmoveByOP(this.mstrTemPd, DocumentType.WR.ToString(), inWOrderH, dtrBomIT["cOPSeq"].ToString());

                }
            }

        }

        private void pmLoadFG(string inWOrderH, string inMasterProd, bool inIsLoadMasterProd)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;
            string strRefDocTab = strDBName + ".dbo.REFDOC";
            string strWOrderOPTab = strDBName + ".dbo.MFWORDERIT_STDOP";

            string strSQLStr = "select GLREF.FCREFTYPE, GLREF.FCREFNO, GLREF.FCCODE , GLREF.FDDATE";
            strSQLStr += " , REFPROD.FNQTY, REFPROD.FCIOTYPE, REFPROD.FCLOT ";
            strSQLStr += " , PROD.FCTYPE as CPRODTYPE, PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD ";
            strSQLStr += " , BOOK.FCCODE as QCBOOK ";
            strSQLStr += " , WHOUSE.FCCODE as QCWHOUSE ";
            strSQLStr += " , REFPROD.FCWHOUSE, REFPROD.FCUM, REFPROD.FCPROD, REFPROD.FCSKID as FCREFPROD, REFPROD.FCGLREF ";
            strSQLStr += " from GLREF  ";
            strSQLStr += " left join REFPROD on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr += " left join BOOK on BOOK.FCSKID = GLREF.FCBOOK ";
            strSQLStr += " where GLREF.FCSKID in ( ";
            strSQLStr += " select REFDOC.CMASTERH from " + strRefDocTab + " REFDOC ";
            strSQLStr += " where REFDOC.CCHILDTYPE = ? and REFDOC.CCHILDH = ? and REFDOC.CMASTERTYP = 'FR' ";
            strSQLStr += " group by REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH ) ";
            strSQLStr += " and WHOUSE.FCTYPE = ' ' ";
            if (inIsLoadMasterProd)
            {
                strSQLStr += " and REFPROD.FCPROD = ? ";
            }
            else
            {
                strSQLStr += " and REFPROD.FCPROD <> ? ";
            }
            strSQLStr += " order by GLREF.FDDATE, REFPROD.FCIOTYPE, GLREF.FCCODE ";

            pobjSQLUtil2.NotUpperSQLExecString = true;
            pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), inWOrderH, inMasterProd });
            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QREFDOC1", "REFDOC", strSQLStr, ref strErrorMsg);

            DataRow dtrTemPd = null;
            int intRecNo = this.gridView4.RowCount - 1;
            foreach (DataRow dtrRefDoc in this.dtsDataEnv.Tables["QREFDOC1"].Rows)
            {
                intRecNo++;
                string strRefNo = dtrRefDoc["fcRefType"].ToString().Trim() + dtrRefDoc["QcBook"].ToString().Trim() + "/" + dtrRefDoc["fcCode"].ToString().Trim();
                this.pmRepl1RecTemPd3(this.mstrTemFG, intRecNo, dtrRefDoc, strRefNo, "", 1, inWOrderH, dtrRefDoc["fcGLRef"].ToString(), dtrRefDoc["fcRefProd"].ToString());
                dtrTemPd = dtrRefDoc;

                if (inIsLoadMasterProd)
                {
                    this.mdecSumFRQty += Convert.ToDecimal(dtrRefDoc["fnQty"]);
                }
            }

            bool bllNoFRQty = (this.mdecSumFRQty == 0);
            if (inIsLoadMasterProd
                && this.mdecSumFRQty != 0
                && this.mdecSumMOQty > this.mdecSumFRQty)
            {
                string strGLRef = dtrTemPd["fcGLRef"].ToString();
                string strRefProd = dtrTemPd["fcRefProd"].ToString();
                dtrTemPd["fcGLRef"] = "";
                dtrTemPd["fcRefProd"] = "";
                dtrTemPd["fnQty"] = (this.mdecSumMOQty - this.mdecSumFRQty);
                this.pmRepl1RecTemPd3(this.mstrTemFG, intRecNo + 1, dtrTemPd, "", "", 1, inWOrderH, strGLRef, strRefProd);
            }
            else if (inIsLoadMasterProd && bllNoFRQty)
            {
                //Search WORDERI
                //Search OP ล่างสุด

                string strWOrderOP = "";
                string strWOrderOPSeq = "";

                pobjSQLUtil.SetPara(new object[] { inWOrderH });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWOrderOP", QMFWOrderIT_OPInfo.TableName, "select top 1 * from " + QMFWOrderIT_OPInfo.TableName + " where CWORDERH = ? order by COPSEQ desc", ref strErrorMsg))
                {
                    DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderOP"].Rows[0];
                    strWOrderOP = dtrWOrderOP["cRowID"].ToString();
                    strWOrderOPSeq = dtrWOrderOP["cOPSeq"].ToString().TrimEnd();
                }

                pobjSQLUtil.SetPara(new object[] { inWOrderH, inMasterProd });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, "select top 1 * from " + QMFWOrderIT_PdInfo.TableName + " where CWORDERH = ? and cProd = ? order by cSeq", ref strErrorMsg))
                {
                    DataRow dtrWOrderIT = this.dtsDataEnv.Tables["QWOrderI"].Rows[0];
                    this.pmRepl1RecTemPd4(this.mstrTemFG, intRecNo + 1, dtrWOrderIT, strWOrderOPSeq, strWOrderOP);
                }
            }

        }

        private void pmLoadStmoveByOP(string inAlias, string inRefType, string inWOrderH, string inOPSeq)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;
            string strRefDocTab = strDBName + ".dbo.REFDOC";
            string strWOrderOPTab = strDBName + ".dbo.MFWORDERIT_STDOP";

            string strSQLStr = "select GLREF.FCREFTYPE, GLREF.FCREFNO, GLREF.FCCODE , GLREF.FDDATE";
            strSQLStr += " , REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY, REFPROD.FCIOTYPE, REFPROD.FCLOT ";
            strSQLStr += " , PROD.FCTYPE as CPRODTYPE, PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD ";
            strSQLStr += " , BOOK.FCCODE as QCBOOK ";
            strSQLStr += " , WHOUSE.FCCODE as QCWHOUSE ";
            strSQLStr += " , REFPROD.FCWHOUSE, REFPROD.FCUM, REFPROD.FCPROD, REFPROD.FCSKID as FCREFPROD, REFPROD.FCGLREF ";
            strSQLStr += " from GLREF  ";
            strSQLStr += " left join REFPROD on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr += " left join BOOK on BOOK.FCSKID = GLREF.FCBOOK ";
            strSQLStr += " where GLREF.FCSKID in ( ";
            strSQLStr += " select REFDOC.CMASTERH from " + strRefDocTab + " REFDOC ";
            strSQLStr += " left join " + strWOrderOPTab + " MFWORDERIT_STDOP on MFWORDERIT_STDOP.CROWID = REFDOC.CCHILDI ";
            strSQLStr += " where REFDOC.CCHILDTYPE = ? and REFDOC.CCHILDH = ? and REFDOC.CMASTERTYP in ( 'WR' , 'RW') and MFWORDERIT_STDOP.COPSEQ = ? ";
            strSQLStr += " group by REFDOC.CCHILDTYPE,REFDOC.CCHILDH,REFDOC.CMASTERTYP,REFDOC.CMASTERH ) ";
            strSQLStr += " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr += " order by GLREF.FDDATE, REFPROD.FCIOTYPE, GLREF.FCCODE ";

            //เฉพาะ WR/RW ไม่รวม FR select from REFDOC เพื่อหา ใบเบิกตาม OP นั้น ๆ
            //select from REFDOC_STMOVE GLREF/REFPROD เพื่อหาใบเบิกจริง ๆ

            //string strSQLStr = "select * from REFDOC where CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? and CMASTERTYP = ? ";
            //string strSQLStr2 = "select * from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDH = ? and CMASTERTYP = ? and CMASTERH = ? ";

            pobjSQLUtil2.NotUpperSQLExecString = true;
            pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), inWOrderH, inOPSeq });
            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QREFDOC1", "REFDOC", strSQLStr, ref strErrorMsg);

            int intRecNo = this.gridView3.RowCount - 1;
            foreach (DataRow dtrRefDoc in this.dtsDataEnv.Tables["QREFDOC1"].Rows)
            {
                intRecNo++;
                string strRefNo = dtrRefDoc["fcRefType"].ToString().Trim() + dtrRefDoc["QcBook"].ToString().Trim() + "/" + dtrRefDoc["fcCode"].ToString().Trim();
                this.pmRepl1RecTemPd3(inAlias, intRecNo, dtrRefDoc, strRefNo, inOPSeq, (dtrRefDoc["fcRefType"].ToString().Trim() == "WR" ? 1 : -1), inWOrderH, dtrRefDoc["fcGLRef"].ToString(), dtrRefDoc["fcRefProd"].ToString());
            }

        }

        private void pmLoadBOMIT_Pd2(string inWOrderH, string inAlias)
        {

            this.dtsDataEnv.Tables[inAlias].Rows.Clear();

            int intRecNo = 0;
            string strErrorMsg = "";
            string strIOType = (inAlias == this.mstrTemFG ? "I" : "O");
            string strSQLStr = "select * from " + this.mstrRefToTabI2 + " where cWOrderH = ? and cIOType = ? order by cSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { inWOrderH, strIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefToTabI2, this.mstrRefToTabI2, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables[this.mstrRefToTabI2].Rows)
                {
                    intRecNo++;
                    this.pmRepl1RecTemPd2(inAlias, intRecNo, dtrPdStI);
                }
            }
        }

        /// <summary>
        /// ใช้ตอน Load รายการสินค้าที่อ้างอิง MO แบบเก่า
        /// </summary>
        /// <param name="inAlias"></param>
        /// <param name="inRecNo"></param>
        /// <param name="inWOrderI"></param>
        private void pmRepl1RecTemPd2(string inAlias, int inRecNo, DataRow inWOrderI)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            //dtrTemPd["cRowID"] = inWOrderI["cRowID"].ToString().TrimEnd();
            dtrTemPd["cRowID"] = "";
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
            dtrTemPd["nMOQty"] = Convert.ToDecimal(inWOrderI["nQty"]);

            decimal decUseQty = 0;
            this.pmSumWCTranI2(inWOrderI["cRowID"].ToString(), (inAlias == this.mstrTemPd ? "I" : "O"), ref decUseQty);
            dtrTemPd["nQty"] = decUseQty;

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

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", "BOM", "select cCode from " + MapTable.Table.MFBOMHead + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private void pmRepl1RecTemPd3(string inAlias, int inRecNo, DataRow inWOrderI, string inRefNo, string inOPSeq, decimal inFactor, string inWOrderH, string inGLRef, string inRefProd)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            //dtrTemPd["cRowID"] = inWOrderI["cRowID"].ToString().TrimEnd();
            dtrTemPd["cRowID"] = "";
            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inOPSeq;
            dtrTemPd["cPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inWOrderI["fcProd"].ToString();

            dtrTemPd["cRef2RefTy"] = inWOrderI["fcRefType"].ToString();
            dtrTemPd["cRef2Head"] = inWOrderI["fcGLRef"].ToString();
            dtrTemPd["cRef2Item"] = inWOrderI["fcRefProd"].ToString();
            dtrTemPd["cRefTo_Code"] = inRefNo;
            
            dtrTemPd["cLastProd"] = inWOrderI["fcProd"].ToString();
            //dtrTemPd["cPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            //dtrTemPd["cLastPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            //dtrTemPd["cRemark1"] = (Convert.IsDBNull(inWOrderI["cReMark1"]) ? "" : inWOrderI["cReMark1"].ToString().TrimEnd());
            //dtrTemPd["cRemark2"] = (Convert.IsDBNull(inWOrderI["cReMark2"]) ? "" : inWOrderI["cReMark2"].ToString().TrimEnd());
            //dtrTemPd["cRemark3"] = (Convert.IsDBNull(inWOrderI["cReMark3"]) ? "" : inWOrderI["cReMark3"].ToString().TrimEnd());
            //dtrTemPd["cRemark4"] = (Convert.IsDBNull(inWOrderI["cReMark4"]) ? "" : inWOrderI["cReMark4"].ToString().TrimEnd());
            //dtrTemPd["cRemark5"] = (Convert.IsDBNull(inWOrderI["cReMark5"]) ? "" : inWOrderI["cReMark5"].ToString().TrimEnd());
            //dtrTemPd["cRemark6"] = (Convert.IsDBNull(inWOrderI["cReMark6"]) ? "" : inWOrderI["cReMark6"].ToString().TrimEnd());
            //dtrTemPd["cRemark7"] = (Convert.IsDBNull(inWOrderI["cReMark7"]) ? "" : inWOrderI["cReMark7"].ToString().TrimEnd());
            //dtrTemPd["cRemark8"] = (Convert.IsDBNull(inWOrderI["cReMark8"]) ? "" : inWOrderI["cReMark8"].ToString().TrimEnd());
            //dtrTemPd["cRemark9"] = (Convert.IsDBNull(inWOrderI["cReMark9"]) ? "" : inWOrderI["cReMark9"].ToString().TrimEnd());
            //dtrTemPd["cRemark10"] = (Convert.IsDBNull(inWOrderI["cReMark10"]) ? "" : inWOrderI["cReMark10"].ToString().TrimEnd());
            dtrTemPd["cUOM"] = inWOrderI["fcUM"].ToString().TrimEnd();
            //dtrTemPd["nMOQty"] = Convert.ToDecimal(inWOrderI["fnQty"]);

            decimal decUseQty = 0;
            //this.pmSumWCTranI2(inWOrderI["cRowID"].ToString(), (inAlias == this.mstrTemPd ? "I" : "O"), ref decUseQty);
            dtrTemPd["nQty"] = Convert.ToDecimal(inWOrderI["fnQty"]) * inFactor;

            dtrTemPd["nSaveQty"] = Convert.ToDecimal(inWOrderI["fnQty"]);
            //dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWOrderI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWOrderI["nUOMQty"]));
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["cLot"] = inWOrderI["fcLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inWOrderI["fcWHouse"].ToString().TrimEnd();

            string strSQLStr_WOrderI = "";
            strSQLStr_WOrderI = "select MFWORDERIT_PD.* from REFDOC_STMOVE";
            strSQLStr_WOrderI += " left join MFWORDERIT_PD on MFWORDERIT_PD.CROWID = REFDOC_STMOVE.CCHILDI ";
            strSQLStr_WOrderI += " where REFDOC_STMOVE.CMASTERTYP = ? and REFDOC_STMOVE.CMASTERH = ? and REFDOC_STMOVE.CMASTERI = ? ";
            //pobjSQLUtil2.SetPara(new object[] { inWOrderI["fcRefType"].ToString(), inWOrderI["fcGLRef"].ToString(), inWOrderI["fcRefProd"].ToString() });
            pobjSQLUtil2.SetPara(new object[] { inWOrderI["fcRefType"].ToString(), inGLRef, inRefProd });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, strSQLStr_WOrderI, ref strErrorMsg))
            {

                DataRow dtrWOrderI = this.dtsDataEnv.Tables["QWOrderI"].Rows[0];

                this.mdecSumMOQty = Convert.ToDecimal(dtrWOrderI["nQty"]);
                dtrTemPd["cWOrderH"] = dtrWOrderI["cWOrderH"].ToString();
                dtrTemPd["cWOrderI"] = dtrWOrderI["cRowID"].ToString();
                dtrTemPd["nMOQty"] = Convert.ToDecimal(dtrWOrderI["nQty"]);
                dtrTemPd["cPdStI"] = dtrWOrderI["cBOMIT_PD"].ToString();
                dtrTemPd["cScrap"] = dtrWOrderI["cScrap"].ToString().TrimEnd();
                dtrTemPd["cRoundCtrl"] = dtrWOrderI["cRoundCtrl"].ToString().TrimEnd();
                dtrTemPd["cProcure"] = dtrWOrderI["cProcure"].ToString().TrimEnd();
                dtrTemPd["cMfgBOMHD"] = dtrWOrderI["cMfgBOMHD"].ToString();
            }

            string strSQLStr_WOrderI_FR = "";
            strSQLStr_WOrderI_FR = "select MFWORDERIT_STDOP.cRowID from REFDOC";
            strSQLStr_WOrderI_FR += " left join MFWORDERIT_STDOP on MFWORDERIT_STDOP.CROWID = REFDOC.CCHILDI ";
            strSQLStr_WOrderI_FR += " where REFDOC.CCHILDTYPE = ? and REFDOC.CCHILDH = ? and REFDOC.CMASTERTYP = ? and REFDOC.CMASTERH = ? ";
            pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), inWOrderH, inWOrderI["fcRefType"].ToString(), inGLRef });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderOP", QMFWOrderIT_PdInfo.TableName, strSQLStr_WOrderI_FR, ref strErrorMsg))
            {
                DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderOP"].Rows[0];
                dtrTemPd["cWOrderOP"] = dtrWOrderOP["cRowID"].ToString();
            }

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

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", "BOM", "select cCode from " + MapTable.Table.MFBOMHead + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private void pmRepl1RecTemPd4(string inAlias, int inRecNo, DataRow inWOrderI, string inOPSeq, string inWOrderOP)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[inAlias].NewRow();
            //dtrTemPd["cRowID"] = inWOrderI["cRowID"].ToString().TrimEnd();
            dtrTemPd["cRowID"] = "";
            dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inOPSeq;
            dtrTemPd["cPdType"] = inWOrderI["cProdType"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inWOrderI["cProd"].ToString();
            
            dtrTemPd["cRef2RefTy"] = "";
            dtrTemPd["cRef2Head"] = "";
            dtrTemPd["cRef2Item"] = "";
            dtrTemPd["cRefTo_Code"] = "";

            dtrTemPd["cUOM"] = inWOrderI["cUOM"].ToString().TrimEnd();

            decimal decUseQty = 0;
            //this.pmSumWCTranI2(inWOrderI["cRowID"].ToString(), (inAlias == this.mstrTemPd ? "I" : "O"), ref decUseQty);
            dtrTemPd["nQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["nUOMQty"] = Convert.ToDecimal(inWOrderI["nUOMQty"]);

            dtrTemPd["nSaveQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            //dtrTemPd["nUOMQty"] = (Convert.ToDecimal(inWOrderI["nUOMQty"]) == 0 ? 1 : Convert.ToDecimal(inWOrderI["nUOMQty"]));
            //dtrTemPd["nLastQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["cLot"] = inWOrderI["cLot"].ToString().TrimEnd();
            dtrTemPd["cWHouse"] = inWOrderI["cWHouse"].ToString();

            dtrTemPd["cWOrderH"] = inWOrderI["cWOrderH"].ToString();
            dtrTemPd["cWOrderI"] = inWOrderI["cRowID"].ToString();
            dtrTemPd["nMOQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
            dtrTemPd["cPdStI"] = inWOrderI["cBOMIT_PD"].ToString();
            dtrTemPd["cScrap"] = inWOrderI["cScrap"].ToString().TrimEnd();
            dtrTemPd["cRoundCtrl"] = inWOrderI["cRoundCtrl"].ToString().TrimEnd();
            dtrTemPd["cProcure"] = inWOrderI["cProcure"].ToString().TrimEnd();
            dtrTemPd["cMfgBOMHD"] = inWOrderI["cMfgBOMHD"].ToString();

            dtrTemPd["cWOrderOP"] = inWOrderOP;

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

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", "BOM", "select cCode from " + MapTable.Table.MFBOMHead + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrTemPd);

        }

        private void pmSumWCTranI(string inWOrderH, string inWOrderI, string inIOType, ref decimal ioQty, ref decimal ioWasteQty, ref decimal ioLossQty)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select sum(NQTY) as NQTY,sum(NWASTEQTY) as NWASTEQTY,sum(NLOSSQTY1) as NLOSSQTY1 from MFWCTRANIT where MFWCTRANIT.CWCTRANH in (";
            strSQLStr += " select REFDOC.CMASTERH from REFDOC where REFDOC.CCHILDTYPE = 'MO' ";
            strSQLStr += " and REFDOC.CCHILDH = ? and REFDOC.CCHILDI = ? ) and MFWCTRANIT.CIOTYPE = ? ";

            pobjSQLUtil.SetPara(new object[] { inWOrderH, inWOrderI, inIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumWCTranI", "MFWCTRANIT", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrSum1 = this.dtsDataEnv.Tables["QSumWCTranI"].Rows[0];
                if (!Convert.IsDBNull(dtrSum1["NQTY"]))
                {
                    ioQty = Convert.ToDecimal(dtrSum1["NQTY"]);
                }

                if (!Convert.IsDBNull(dtrSum1["NWASTEQTY"]))
                {
                    ioWasteQty = Convert.ToDecimal(dtrSum1["NWASTEQTY"]);
                }

                if (!Convert.IsDBNull(dtrSum1["NLOSSQTY1"]))
                {
                    ioLossQty = Convert.ToDecimal(dtrSum1["NLOSSQTY1"]);
                }
            
            }

        }

        private void pmSumActualOPTime(string inWOrderH, string inWOrderI, ref decimal ioLT_Proc)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select sum(NQTY) as NQTY,sum(NWASTEQTY) as NWASTEQTY,sum(NLOSSQTY1) as NLOSSQTY1 from MFWCTRANIT where MFWCTRANIT.CWCTRANH in (";
            strSQLStr += " select REFDOC.CMASTERH from REFDOC where REFDOC.CCHILDTYPE = 'MO' ";
            strSQLStr += " and REFDOC.CCHILDH = ? and REFDOC.CCHILDI = ? ) and MFWCTRANIT.CIOTYPE = ? ";

            pobjSQLUtil.SetPara(new object[] { inWOrderH, inWOrderI });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumWCTranI", "MFWCTRANIT", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrSum1 = this.dtsDataEnv.Tables["QSumWCTranI"].Rows[0];
                if (!Convert.IsDBNull(dtrSum1["NQTY"]))
                {
                    //ioQty = Convert.ToDecimal(dtrSum1["NQTY"]);
                }

            }

        }

        private void pmSumWCTranI2(string inWOrderI, string inIOType, ref decimal ioQty)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            //strSQLStr = "select sum(NQTY) as NQTY,sum(NWASTEQTY) as NWASTEQTY,sum(NLOSSQTY1) as NLOSSQTY1 from MFWCTRANIT where CWORDERI = ? ";
            strSQLStr = "select sum(NQTY) as NQTY from MFWCTRANIT where CWORDERI = ? and CIOTYPE = ? ";

            pobjSQLUtil.SetPara(new object[] { inWOrderI, inIOType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumWCTranI", "MFWCTRANIT", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrSum1 = this.dtsDataEnv.Tables["QSumWCTranI"].Rows[0];
                if (!Convert.IsDBNull(dtrSum1["NQTY"]))
                {
                    ioQty = Convert.ToDecimal(dtrSum1["NQTY"]);
                }

                //if (!Convert.IsDBNull(dtrSum1["NWASTEQTY"]))
                //{
                //    ioWasteQty = Convert.ToDecimal(dtrSum1["NWASTEQTY"]);
                //}

                //if (!Convert.IsDBNull(dtrSum1["NLOSSQTY1"]))
                //{
                //    ioLossQty = Convert.ToDecimal(dtrSum1["NLOSSQTY1"]);
                //}

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
                                    this.pmLoadStockBal(dtrProd["fcSkid"].ToString());

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
								this.mdecPdStOutput = (Convert.ToDecimal(dtrPopup["nMfgQty"]) > 0 ? Convert.ToDecimal(dtrPopup["nMfgQty"]) : 1);

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
                break;
                case "TXTQCPROD":
                case "TXTQNPROD":

                    dtrGetVal = this.pofrmGetProd.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        if (this.txtQcProd.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                        {
                            this.txtQcProd.Tag = dtrGetVal["fcSkid"].ToString();
                            this.pmLoadStockBal(dtrGetVal["fcSkid"].ToString());
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

        private void pmLoadStockBal(string inProd)
        {

            decimal decBalStk = 0;
            
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = " select REFPROD.FCPROD , REFPROD.FCIOTYPE , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNQTY ";
            strSQLStr += " from REFPROD ";
            strSQLStr += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " where REFPROD.FCPROD = ? ";
            strSQLStr += " and WHOUSE.FCCORP = ? and WHOUSE.FCTYPE = ' ' ";
            strSQLStr += " and WHOUSE.FCCODE in (" + this.mstrBook_WHouse_FG + ") ";
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
            this.txtFGBalQty.Value = decBalStk;

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
            this.pmClearTemToBlank();

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

        private void pmReplTemPdStI(int inRecNo, DataRow inTemPd)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
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
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM", "WHOUSE", "select CCODE from "+QMFBOMInfo.TableName+" where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
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

                            //if (!this.pmCheckHasPR(this.mstrEditRowID, dtrLoadHead[QMFWCloseHDInfo.Field.RefNo].ToString(), ref strErrorMsg))
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

            string strCode = dtrBrow["cCode"].ToString().TrimEnd();
            using (Transaction.Common.dlgPRange dlg = new Transaction.Common.dlgPRange())
            {

                dlg.BeginCode = strCode;
                dlg.EndCode = strCode;

                string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_MC\\");
                dlg.LoadRPT(Application.StartupPath + "\\RPT\\FORM_MC\\");

                //int intLen1 = (Application.StartupPath + "\\RPT\\FORM_MC\\").Length;
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
            string strSQLStrOrderI = "select * from " + this.mstrITable2 + " where cWCloseH = ? and cIOType = 'O' order by cSeq";
            string strSQLStrOrderI_OP = "select * from " + this.mstrITable + " where cWCloseH = ? order by cSeq";

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

                            dtrPrnData["MO_ITEM_TYPE"] = "2_RM";
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["RM_LOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["RM_REFMOQTY"] = Convert.ToDecimal(dtrOrderI["nMOQty"]);

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

                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            //dtrPrnData["NMOQTY"] = decMfgQty;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;


                            string strRefProdSeq = "";
                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cRef2Head"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", "select GLREF.FCSKID , GLREF.FDDATE , BOOK.FCCODE as QCBOOK, GLREF.FCREFTYPE, GLREF.FCRFTYPE, GLREF.FCCODE, GLREF.FCREFNO  from GLREF left join BOOK on BOOK.FCSKID = GLREF.FCBOOK where GLREF.fcSkid = ?", ref strErrorMsg))
                            {

                                DataRow dtrGLRef = this.dtsDataEnv.Tables["QGLRef"].Rows[0];
                                dtrPrnData["RM_REFDOC"] = dtrGLRef["fcRefType"].ToString().TrimEnd() + dtrGLRef["QcBook"].ToString().TrimEnd() + "/" + dtrGLRef["fcCode"].ToString().TrimEnd();
                                //TODO: เปลี่ยนให้รองรับ costแบบ FIFO
                                //dtrPrnData["RM_COST_ACTUAL"] = this.pmGetAVGPrice(dtrOrderI["cProd"].ToString(), Convert.ToDateTime(dtrGLRef["fdDate"]));
                                DateTime dttDocDate = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;

                                DataRow QRefProd = null;
                                pobjSQLUtil.SetPara(new object[1] { dtrOrderI["CREF2ITEM"].ToString() });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from REFPROD where REFPROD.fcSkid = ?", ref strErrorMsg))
                                {
                                    QRefProd = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                                    strRefProdSeq = QRefProd["fcSeq"].ToString();
                                }

                                if (App.ActiveCorp.CostMethod_Rawmat == "A")
                                {
                                    dtrPrnData["RM_COST_ACTUAL"] = this.pmGetAVGPrice(dtrOrderI["cProd"].ToString(), dttDocDate);
                                }
                                else
                                {
                                    string strProd = dtrOrderI["cProd"].ToString();
                                    decimal decStkQty = this.pmPrintStock(dtrOrderI["cProd"].ToString(), dttDocDate.AddDays(-1));
                                    //decStkQty = decStkQty + (dtrOrderI["cIOType"].ToString() == "I" ? -1 : 1) * Convert.ToDecimal(dtrOrderI["nQty"]);
                                    decimal decAmt = 0;
                                    decimal decSedQty = 0;
                                    int intSedRecn = 0;
                                    decimal decSedCost = 0;
                                    decimal decSedQty2 = 0;
                                    string strSedRefNo = "";
                                    decimal decFIFOCost = 0;
                                    //dtrStock["StockQty"] = decStkQty;

                                    //App.AppMessage = "คำนวณราคาทุน : " + "\r\n(" + dtrStock["QcProd"].ToString().TrimEnd() + ")" + dtrStock["QnProd"].ToString().TrimEnd();


                                    if (strQcProd.TrimEnd() == "1ALNIK0505251H18" && dtrGLRef["fcRefNo"].ToString().Trim() == "WR01/54080003")
                                    {
                                        dataGridView1.DataSource = this.dtsDataEnv.Tables["TemIn"];
                                        dataGridView2.DataSource = this.dtsDataEnv.Tables["TemOut"];
                                    }

                                    //GLHelper.PrintBFQTYLine(dtrOrderI["cProd"].ToString(), this.mstrBranch, Convert.ToDateTime(dtrOrderI["dDate"]), Convert.ToDateTime(dtrOrderI["dDate"]), decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);
                                    GLHelper.PrintBFQTYLine(dtrOrderI["cProd"].ToString(), this.mstrBranch, dttDocDate, dttDocDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);

                                    this.dtsDataEnv.Tables["TemOut"].Rows.Clear();
                                    this.dtsDataEnv.Tables["TemIn"].Rows.Clear();

                                    foreach (DataRow dtrBFFIFO in GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows)
                                    {

                                        decimal tCostAmt1 = Math.Abs(Convert.ToDecimal(dtrBFFIFO["fnCostAmt"]) + Convert.ToDecimal(dtrBFFIFO["fnCostAdj"]));
                                        decimal decPrice = (Convert.ToDecimal(dtrBFFIFO["fnQty"]) != 0 ? tCostAmt1 / Convert.ToDecimal(dtrBFFIFO["fnQty"]) : 0);
                                        pmAddRowIn(Convert.ToDecimal(dtrBFFIFO["fnQty"]), Convert.ToDecimal(dtrBFFIFO["fnQty"]), decPrice, dtrBFFIFO["fcRefNo"].ToString());

                                    }

                                    if (decStkQty == 0)
                                    {
                                        this.pmPrint1Prod_Add(dtrOrderI["cProd"].ToString(), this.mstrBranch, dttDocDate, dttDocDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);
                                    }
                                    this.pmPrint1Prod(dtrOrderI["cProd"].ToString(), this.mstrBranch, dttDocDate, dttDocDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);

                                    //if (strQcProd.TrimEnd() == "AP-121")
                                    if (strQcProd.TrimEnd() == "1ALNIK0505251H18")
                                    {
                                        //dataGridView1.DataSource = this.dtsDataEnv.Tables["TemIn"];
                                    }


                                    #region "รายการฝั่งจ่าย UN-USED"

                                    //decimal tRefPdQty = Convert.ToDecimal(dtrOrderI["nQty"]) * (dtrOrderI["cIOType"].ToString() == "I" ? 1 : -1);
                                    //decimal tAbsQty = Math.Abs(tRefPdQty);
                                    //decimal tCostAmt = 0;    //Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                                    //decimal tRefCost = 0;

                                    //decimal tAmt = 0;
                                    //decimal tAmt1 = 0;
                                    //decimal tQty1 = 0;
                                    //decimal tQty2 = 0;
                                    //decimal tCost1 = 0;
                                    //decimal tCostAdj = 0;
                                    //decimal tInAmount = 0;


                                    ////รายการฝั่งจ่าย
                                    //tCostAdj = tCostAmt;
                                    //tRefCost = decSedCost;

                                    //bool tFstPSed = true;
                                    //decimal tPCost = 0;
                                    //if (decSedQty - tAbsQty >= 0)
                                    //{
                                    //    tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                    //    tAmt = tRefCost * tAbsQty;
                                    //    P1Line(dtrGLRef, tAbsQty, tPCost, tAmt, ref strSedRefNo);
                                    //    decFIFOCost += tAmt;
                                    //    decSedQty = decSedQty - tAbsQty;
                                    //    tFstPSed = false;
                                    //}
                                    //else
                                    //{

                                    //    decimal tAdj = 0;
                                    //    decimal tOutAmt = 0;
                                    //    decimal tOutAmt1 = 0;
                                    //    decimal tOutAmt2 = 0;
                                    //    decimal tOQty = tAbsQty;
                                    //    bool bllIsNegativeStock = false;
                                    //    if (decSedQty > 0)
                                    //    {
                                    //        while ((decSedQty - tOQty < 0) && (intSedRecn > -1))
                                    //        {
                                    //            tOQty = tOQty - decSedQty;
                                    //            tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                    //            tAmt = tRefCost * decSedQty;
                                    //            tOutAmt1 = tAmt;
                                    //            tAdj = tCostAdj - tAmt;
                                    //            tOutAmt = tAdj + tAmt;
                                    //            P1Line(dtrGLRef, decSedQty, tPCost, tAmt, ref strSedRefNo);
                                    //            decFIFOCost += tAmt;
                                    //            SumSed(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                                    //            tFstPSed = false;
                                    //            tRefCost = decSedCost;
                                    //            if (decSedQty == 0 && tOQty > 0 && intSedRecn > -1)
                                    //            {
                                    //                bllIsNegativeStock = true;
                                    //                tRefCost = tPCost;
                                    //                break;
                                    //            }


                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        //กรณี stock ติดลบ
                                    //        decimal decNegStock = decSedQty;
                                    //        SumSedNegative(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                                    //        //decSedQty = decSedQty + decNegStock;
                                    //        tRefCost = decSedCost;
                                    //    }

                                    //    if (tOQty > 0)
                                    //    {

                                    //        if (bllIsNegativeStock)
                                    //        {
                                    //            decSedQty = decSedQty - tOQty;
                                    //        }
                                    //        else
                                    //        {
                                    //            decSedQty = decSedQty - tOQty;
                                    //            decSedQty = (decSedQty > 0 ? decSedQty : 0);
                                    //        }
                                    //        //decSedQty = max( decSedQty , 0 )
                                    //        tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                    //        tAmt = tRefCost * tOQty;
                                    //        tOutAmt2 = tOutAmt1 + tAmt;
                                    //        //= P1Line( iif( tFstPSed , tDate , {//} ) , iif( tFstPSed , tRefNo , '' ) , tOQty , tPCost , tAmt , 'O' , .T. , @xaYokMaQty , @xaYokMaAmt , iif( tFstPSed , RefProd.fcTimeStam , '' ) , tCostAdj , tOutAmt2 , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                                    //        P1Line(dtrGLRef, tOQty, tPCost, tAmt, ref strSedRefNo);
                                    //        decFIFOCost += tAmt;
                                    //    }
                                    //}
                                    //if ((decSedQty == 0) && (intSedRecn != -1))
                                    //{
                                    //    SumSed(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                                    //}


                                    #endregion

                                    //decimal decPrice = (decStkQty != 0 ? decFIFOCost / decStkQty : 0);
                                    //dtrPrnData["RM_COST_ACTUAL"] = decPrice * Convert.ToDecimal(dtrOrderI["nQty"]);

                                    //dtrPrnData["RM_COST_ACTUAL"] = decFIFOCost;
                                    //pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cRef2Item"].ToString() });
                                    //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from RefProd where fcSkid = ?", ref strErrorMsg))
                                    if (QRefProd != null)
                                    {
                                        DataRow dtrRefProd = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                                        if (VRefProd3(dtrGLRef["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()) && dtrRefProd["fcIOType"].ToString() == "I")
                                        {

                                            decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                                            decimal tAbsQty = Math.Abs(tRefPdQty);
                                            decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                                            dtrPrnData["RM_COST_ACTUAL"] = tCostAmt;
                                        }
                                        else
                                        {
                                            DataRow[] dtrFIFOCost = this.dtsDataEnv.Tables["TemOut"].Select("RefNoSeq = '" + dtrGLRef["fcRefNo"].ToString() + strRefProdSeq + "'");
                                            decFIFOCost = 0;
                                            for (int i = 0; i < dtrFIFOCost.Length; i++)
                                            {
                                                decFIFOCost += Convert.ToDecimal(dtrFIFOCost[i]["Amt"]);
                                            }
                                            dtrPrnData["RM_COST_ACTUAL"] = decFIFOCost;

                                        }
                                    }


                                }


                            }

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cWHouse"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select FCCODE,FCNAME from WHouse where fcSkid = ?", ref strErrorMsg))
                            {
                                dtrPrnData["RM_QCWHOUSE"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                                dtrPrnData["RM_QNWHOUSE"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
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
            string strRefFRCode = "";
            decimal decRefFRQty = 0;
            
            this.pmLoadRefToCode(this.mstrRefType, dtrWOrderH["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);
            this.pmLoadRefToFRCode(this.mstrRefType, dtrWOrderH["cRowID"].ToString(), dtrWOrderH["cMfgProd"].ToString(), ref strRefFRCode, ref decRefFRQty);

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
            string strBOM = dtrWOrderH[QMFWOrderHDInfo.Field.BOMID].ToString();
            pobjSQLUtil2.SetPara(new object[1] { strBOM });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select * from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {

                DataRow dtrBOMHD = this.dtsDataEnv.Tables["QPdStH"].Rows[0];
                strQcBOM = dtrBOMHD["cCode"].ToString().TrimEnd();
                strQnBOM = dtrBOMHD["cName"].ToString().TrimEnd();

                this.mdecPdStOutput = (Convert.ToDecimal(dtrBOMHD[QMFBOMInfo.Field.MfgQty]) != 0 ? Convert.ToDecimal(dtrBOMHD[QMFBOMInfo.Field.MfgQty]) : 1);
            
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
            dtrPrnData["CREFDOC_FR_CODE"] = strRefFRCode;
            dtrPrnData["CREFDOC_FR_QTY"] = decRefFRQty;
            //decRefFRQty
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
            dtrPrnData["CREFDOC_CODE"] = strRefDate;

            //strRefFRCode

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

            dtrPrnData["NCOSTDL"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostDL]);
            dtrPrnData["NCOSTOH"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostOH]);
            dtrPrnData["NCOSTADJ1"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostAdj1]);
            dtrPrnData["NCOSTADJ2"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostAdj2]);
            dtrPrnData["NCOSTADJ3"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostAdj3]);
            dtrPrnData["NCOSTADJ4"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostAdj4]);
            dtrPrnData["NCOSTADJ5"] = Convert.ToDecimal(dtrWOrderH[QMFWCloseHDInfo.Field.CostAdj5]);

            //this.txtTotMfgQty.Value = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.Qty]);
            //this.txtMfgQty.Value = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty]);
            dtrPrnData["NMOQTY"] = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty]);
            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.Qty]);
            dtrPrnData["CSCRAP"] = dtrWOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();

            dtrPrnData["NSTOCKQTY"] = this.pmGetStockBal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgProdID].ToString(), this.mstrBook_WHouse_FG);
            dtrPrnData["NFGRATIO1"] = (this.mdecPdStOutput != 0 ? this.txtTotMfgQty.Value / this.mdecPdStOutput : 0);
            //dtrPrnData["NFGRATIO2"] = (Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) != 0 ? Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.RMQtyFactor1]) : 1);

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
        }

        private void pmLoadRefToCode(string inRefType, string inWCloseH, ref string strRefQcBook, ref string strRefQnBook, ref string strRefCode, ref string strRefRefNo, ref string strRefDate)
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //Load Reference Document
            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, inWCloseH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QRefDoc"].Rows[0];
                pobjSQLUtil.SetPara(new object[1] { dtrRefDoc["cChildH"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select * from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
                {
                    DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                    string strMfgBook = dtrRefTo["cMfgBook"].ToString();
                    pobjSQLUtil.SetPara(new object[1] { strMfgBook });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMfgBook", MapTable.Table.MfgBook, "select * from MFGBOOK where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrMfgBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                        strRefQcBook = dtrMfgBook["cCode"].ToString().TrimEnd();
                        strRefQnBook = dtrMfgBook["cName"].ToString().TrimEnd();
                    }
                    strRefCode = this.mstrRefToRefType + strRefQcBook + "/" + dtrRefTo["cCode"].ToString().TrimEnd();
                    strRefRefNo = dtrRefTo["cRefNo"].ToString().TrimEnd();

                }
            }
        }

        private void pmLoadRefToFRCode(string inRefType, string inWCloseH, string inMfgProd, ref string strRefCode, ref decimal ioRefQty)
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            cDBMSAgent pobjSQLUtil2 = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //Load Reference Document
            pobjSQLUtil.SetPara(new object[] { inWCloseH });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWCloseI", "WCloseI", "select * from " + this.mstrITable2 + " where cWCloseH = ? and cIOType = 'I' and cRef2Head <> '' ", ref strErrorMsg))
            {
                foreach (DataRow dtrWCloseI in this.dtsDataEnv.Tables["QWCloseI"].Rows)
                {
                    pobjSQLUtil2.SetPara(new object[1] { dtrWCloseI["cRef2Head"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", "select BOOK.FCCODE as QCBOOK, GLREF.FCREFTYPE, GLREF.FCCODE from GLREF left join BOOK on BOOK.FCSKID = GLREF.FCBOOK where GLREF.fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrGLRef = this.dtsDataEnv.Tables["QGLRef"].Rows[0];
                        strRefCode += dtrGLRef["fcRefType"].ToString().TrimEnd() + dtrGLRef["QcBook"].ToString().TrimEnd() + "/" + dtrGLRef["fcCode"].ToString().TrimEnd() + ",";
                        pobjSQLUtil2.SetPara(new object[2] { dtrWCloseI["cRef2Head"].ToString(), inMfgProd });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSumRefProd", "REFPROD", "select sum(REFPROD.FNQTY) as SumQty from REFPROD where REFPROD.FCGLREF = ? and REFPROD.FCPROD = ? and REFPROD.FCIOTYPE = 'I' ", ref strErrorMsg))
                        {
                            DataRow dtrSumRefProd = this.dtsDataEnv.Tables["QSumRefProd"].Rows[0];
                            if (!Convert.IsDBNull(dtrSumRefProd["SumQty"]))
                            {
                                ioRefQty += Convert.ToDecimal(dtrSumRefProd["SumQty"]);
                            }
                        }
                    }

                }
            }
            if (strRefCode.Length > 0)
            {
                strRefCode = strRefCode.Substring(0, strRefCode.Length - 1);
            }
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

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
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
            string strDeleteDesc = dtrBrow[QMFWCloseHDInfo.Field.Code].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFWCloseHDInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFWCloseHDInfo.Field.Code].ToString(), "", ref strErrorMsg))
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

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = true;
            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select cCode, cName from " + QEMSectInfo.TableName + " where cDept = ? ", ref strErrorMsg))
            {
                strMsg2 = "(" + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"].ToString().TrimEnd() + ") " + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cName"].ToString().TrimEnd();
                ioErrorMsg = strMsg1 + "มีแผนก " + strMsg2 + " อ้างถึงแล้ว";
            }
            else
            {
                bllHasUsed = false;
            }
            return bllHasUsed;
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
                
                pAPara = new object[] { this.mstrRefType, inRowID, this.mstrRefToRefType };
                this.mSaveDBAgent.BatchSQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //ต้องลบ NoteCut ออกก่อนจึงค่อยวนลูป Update Step จึงจะถูกต้อง
                foreach (DataRow dtrChildH in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                {
                    this.mSaveDBAgent.BatchSQLExec("update MFWORDERHD set CMSTEP = '' where cRowID = ?", new object[] { dtrChildH["cChildH"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWCloseH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable2 + " where cWCloseH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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

                    string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_MC\\");
                    dlg2.LoadRPT(Application.StartupPath + "\\RPT\\FORM_MC\\");
                    
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
            //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranchID }, this.txtCode.MaxLength);

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

            dtrSaveInfo[QMFWCloseHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFWCloseHDInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QMFWCloseHDInfo.Field.PlantID] = this.mstrPlant;

            dtrSaveInfo[QMFWCloseHDInfo.Field.Step] = this.mstrStep;
            dtrSaveInfo[QMFWCloseHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFWCloseHDInfo.Field.RFType] = this.mstrRfType;

            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgBookID] = this.mstrBook;
            dtrSaveInfo[QMFWCloseHDInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.RefNo] = this.txtRefNo.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Date] = this.txtDate.DateTime.Date;

            this.mstrPDocCode = this.txtCode.Text.TrimEnd();

            dtrSaveInfo[QMFWCloseHDInfo.Field.StartDate] = this.txtStartDate.DateTime.Date;
            dtrSaveInfo[QMFWCloseHDInfo.Field.DueDate] = this.txtDueDate.DateTime.Date;

            dtrSaveInfo[QMFWCloseHDInfo.Field.CoorID] = this.txtQcCoor.Tag.ToString();
            dtrSaveInfo[QMFWCloseHDInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFWCloseHDInfo.Field.JobID] = this.txtQcJob.Tag.ToString();
            dtrSaveInfo[QMFWCloseHDInfo.Field.BOMID] = this.txtQcBOM.Tag.ToString();

            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgProdID] = this.txtQcProd.Tag.ToString();
            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgQty] = Convert.ToDecimal(this.txtMfgQty.EditValue);
            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgUMID] = this.txtQnMfgUM.Tag.ToString();
            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgUMQty] = this.mdecUMQty;

            dtrSaveInfo[QMFWCloseHDInfo.Field.Scrap] = this.txtScrap.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.ScrapQty] = this.mdecScrapQty;
            dtrSaveInfo[QMFWCloseHDInfo.Field.MfgQty] = this.txtMfgQty.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.Qty] = this.txtTotMfgQty.Value;

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

            dtrSaveInfo[QMFWCloseHDInfo.Field.Priority] = strPriority;

            dtrSaveInfo[QMFWCloseHDInfo.Field.BatchNo] = strBatchNo;
            dtrSaveInfo[QMFWCloseHDInfo.Field.BatchRun] = this.mstrBatchRun;
            dtrSaveInfo[QMFWCloseHDInfo.Field.Level] = this.mstrLevel;
            dtrSaveInfo[QMFWCloseHDInfo.Field.MasterID] = strMasterWO;
            dtrSaveInfo[QMFWCloseHDInfo.Field.ParentID] = this.mstrParentWO;

            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark1] = this.txtRemark1.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark2] = this.txtRemark2.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark3] = this.txtRemark3.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark4] = this.txtRemark4.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark5] = this.txtRemark5.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark6] = this.txtRemark6.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark7] = this.txtRemark7.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark8] = this.txtRemark8.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark9] = this.txtRemark9.Text.TrimEnd();
            dtrSaveInfo[QMFWCloseHDInfo.Field.Remark10] = this.txtRemark10.Text.TrimEnd();

            dtrSaveInfo[QMFWCloseHDInfo.Field.CostDL] = this.txtCostDL.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostOH] = this.txtCostOH.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostAdj1] = this.txtCostAdj1.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostAdj2] = this.txtCostAdj2.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostAdj3] = this.txtCostAdj3.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostAdj4] = this.txtCostAdj4.Value;
            dtrSaveInfo[QMFWCloseHDInfo.Field.CostAdj5] = this.txtCostAdj5.Value;

            switch (BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()))
            {
                case ApproveStep.Wait:
                    dtrSaveInfo[QMFWCloseHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFWCloseHDInfo.Field.ApproveBy] = "";
                    dtrSaveInfo[QMFWCloseHDInfo.Field.ApproveDate] = Convert.DBNull;
                    break;
                case ApproveStep.Approve:
                    dtrSaveInfo[QMFWCloseHDInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFWCloseHDInfo.Field.ApproveBy] = App.AppUserID;
                    dtrSaveInfo[QMFWCloseHDInfo.Field.ApproveDate] = this.txtDApprove.DateTime;
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

                this.pmGenFG();

                this.mdbTran.Commit();

                this.pmSaveRefTo();

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
            dtrBudCI["cWCloseH"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cMOPR"] = inTemPd["cMOPR"].ToString();
            dtrBudCI["cWkCtrH"] = inTemPd["cWkCtrH"].ToString();
            dtrBudCI["cResType"] = SysDef.gc_RESOURCE_TYPE_TOOL;
            dtrBudCI["cResource"] = inTemPd["cResource"].ToString();
            dtrBudCI["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrBudCI["cNextOP"] = inTemPd["cNextOP"].ToString();
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_Queue]);
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp]);
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_Process]);
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_Tear]);
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_Wait]);
            dtrBudCI[QMFWCloseIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemPd[QMFWCloseIT_OPInfo.Field.LeadTime_Move]);
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

            dtrBudCI["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrBudCI["nWasteQty"] = Convert.ToDecimal(inTemPd["nWasteQty"]);
            dtrBudCI["nLossQty1"] = Convert.ToDecimal(inTemPd["nLossQty1"]);

            dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmGenFG()
        {
            DataRow[] dtaSel = this.dtsDataEnv.Tables[this.mstrTemFG].Select("cRef2Head = '' ");
            if (dtaSel.Length > 0)
            {

                string strMsg = UIBase.GetAppUIText(new string[] { "ต้องการสร้างเอกสารใบรับสินค้าสำเร็จรูปหรือไม่ ?", "Do you want to generate Finished Recive Goods [FR] ? " });

                if (MessageBox.Show(this, strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.mdecGenFG_Qty = Convert.ToDecimal(dtaSel[0]["nQty"]);
                    this.pmInitPopUpDialog("GENFR");
                }
            }

        }

        private void pmGenFGDoc()
        {

            string strErrorMsg = "";
            DataRow[] dtaSel = this.dtsDataEnv.Tables[this.mstrTemFG].Select("cRef2Head = '' ");
            for (int i = 0; i < dtaSel.Length; i++)
            {
                DataRow dtrGenFG = dtaSel[i];
                //Common.MRP.cGenFG oGenFG = new Transaction.Common.MRP.cGenFG(dtrGenFG["cWOrderH"].ToString(), dtrGenFG["cWOrderI"].ToString(), dtrGenFG["cWOrderOP"].ToString(), this.mstrBranch, this.mstrPlant, this.mstrGenFG_Book, this.mstrGenFG_WHouse, dtrGenFG["cProd"].ToString(), Convert.ToDecimal(dtrGenFG["nQty"]), this.mstrGenFG_Lot, this.txtQcSect.Tag.ToString(), this.txtQcJob.Tag.ToString(), this.mdttGenPR_Date);
                Common.MRP.cGenFG oGenFG = new Transaction.Common.MRP.cGenFG(dtrGenFG["cWOrderH"].ToString(), dtrGenFG["cWOrderI"].ToString(), dtrGenFG["cWOrderOP"].ToString(), this.mstrBranch, this.mstrPlant, this.mstrGenFG_Book, this.mstrGenFG_WHouse, dtrGenFG["cProd"].ToString(), this.mdecGenFG_Qty, dtrGenFG["cUOM"].ToString(), Convert.ToDecimal(dtrGenFG["nUOMQty"]), this.mstrGenFG_Lot, this.txtQcSect.Tag.ToString(), this.txtQcJob.Tag.ToString(), this.mdttGenPR_Date);
                oGenFG.GenFG();
                if (oGenFG.GenComplete)
                {
                    dtrGenFG["cRef2Head"] = oGenFG.SaveGLRef;
                    dtrGenFG["cRef2Item"] = oGenFG.SaveRefProd;

                    object[] pAPara = new object[] { dtrGenFG["cRef2Head"].ToString(), dtrGenFG["cRef2Item"].ToString(), this.mdecGenFG_Qty, dtrGenFG["cRowID"].ToString() };
                    this.mSaveDBAgent.BatchSQLExec("update " + this.mstrITable2 + " set cRef2Head = ? , cRef2Item = ? , nQty = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
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
            dtrWOrderI["cWCloseH"] = dtrWOrderH["cRowID"].ToString();
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

            dtrWOrderI["nMOQty"] = Convert.ToDecimal(inTemPd["nMOQty"]);
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
            dtrWOrderI["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString().TrimEnd();

            dtrWOrderI["cStep1"] = inTemPd["cStep1"].ToString();
            dtrWOrderI["cStep2"] = inTemPd["cStep2"].ToString();

            dtrWOrderI["cRef2RefTy"] = inTemPd["cRef2RefTy"].ToString();
            dtrWOrderI["cRef2Head"] = inTemPd["cRef2Head"].ToString();
            dtrWOrderI["cRef2Item"] = inTemPd["cRef2Item"].ToString();

            ioRefProd = dtrWOrderI;

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


            DataTable dtTemIn = new DataTable("TemIn");

            dtTemIn.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dtTemIn.Columns.Add(new DataColumn("Qty", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("BalQty", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("IsBF", Type.GetType("System.String")));
            dtTemIn.Columns.Add(new DataColumn("RefNo", Type.GetType("System.String")));
            this.dtsDataEnv.Tables.Add(dtTemIn);
            dtTemIn.TableNewRow += new DataTableNewRowEventHandler(dtTemIn_TableNewRow);

            DataTable dtTemOut = new DataTable("TemOut");
            dtTemOut.Columns.Add(new DataColumn("Qty", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            //dtTemOut.Columns.Add(new DataColumn("BalQty", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("RefNoSeq", Type.GetType("System.String")));
            //dtTemOut.Columns.Add(new DataColumn("cSeq", Type.GetType("System.String")));
            this.dtsDataEnv.Tables.Add(dtTemOut);

            DataTable dtTemPrint = new DataTable("TemPrint");
            dtTemPrint.Columns.Add(new DataColumn("InQty", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("OutQty", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            this.dtsDataEnv.Tables.Add(dtTemPrint);

        }

        private void dtTemIn_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["ID"] = e.Row.Table.Rows.Count + 1;
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
            dtbTemPd.Columns.Add("nMOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nRefQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nScrapQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nSaveQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPortion", System.Type.GetType("System.Int32"));

            dtbTemPd.Columns.Add("nGoodQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nWasteQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLossQty1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nHoldQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUnknowQty", System.Type.GetType("System.Decimal"));
            
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

            dtbTemPd.Columns.Add("cRef2Head", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRef2Item", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRef2RefTy", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefTo_Code", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderH", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderI", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQcCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQnCoor"].DefaultValue = "";
            dtbTemPd.Columns["cOPSeq"].DefaultValue = "";
            dtbTemPd.Columns["cIsMainPd"].DefaultValue = "";
            dtbTemPd.Columns["nMOQty"].DefaultValue = 0;
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nScrapQty"].DefaultValue = 0;
            dtbTemPd.Columns["nSaveQty"].DefaultValue = 0;
            dtbTemPd.Columns["nRefQty"].DefaultValue = 1;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["cWOrderH"].DefaultValue = "";
            dtbTemPd.Columns["cWOrderI"].DefaultValue = "";
            dtbTemPd.Columns["cWOrderOP"].DefaultValue = "";

            dtbTemPd.Columns["nGoodQty"].DefaultValue = 0;
            dtbTemPd.Columns["nWasteQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLossQty1"].DefaultValue = 0;
            dtbTemPd.Columns["nHoldQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUnknowQty"].DefaultValue = 0;

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
            dtbTemPd.Columns["cRef2RefTy"].DefaultValue = "";
            dtbTemPd.Columns["cRef2Head"].DefaultValue = "";
            dtbTemPd.Columns["cRef2Item"].DefaultValue = "";
            dtbTemPd.Columns["cRefTo_Code"].DefaultValue = "";

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

            dtbTemOP.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nGoodQty", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nWasteQty", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nHoldQty", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nUnknowQty", System.Type.GetType("System.Decimal"));

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
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_Queue, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_SetUp, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_Process, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_Tear, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_Wait, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFWCloseIT_OPInfo.Field.LeadTime_Move, System.Type.GetType("System.Decimal"));

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

            dtbTemOP.Columns["nQty"].DefaultValue = 0;
            dtbTemOP.Columns["nGoodQty"].DefaultValue = 0;
            dtbTemOP.Columns["nWasteQty"].DefaultValue = 0;
            dtbTemOP.Columns["nLossQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nHoldQty"].DefaultValue = 0;
            dtbTemOP.Columns["nUnknowQty"].DefaultValue = 0;
            
            dtbTemOP.Columns["nRefQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nRefQty2"].DefaultValue = 0;
            dtbTemOP.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemOP.Columns["cMFBOMIT_STDOP"].DefaultValue = "";

            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_Queue].DefaultValue = 0;
            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp].DefaultValue = 0;
            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_Process].DefaultValue = 0;
            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_Tear].DefaultValue = 0;
            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_Wait].DefaultValue = 0;
            dtbTemOP.Columns[QMFWCloseIT_OPInfo.Field.LeadTime_Move].DefaultValue = 0;

            dtbTemOP.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);

            this.dtsDataEnv.Tables.Add(dtbTemOP);
        }


        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
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
            this.mstrOldRefToRowID = "";
            this.txtCode.Text = "";
            this.txtRefNo.Text = "";
            this.txtRefTo.Text = "";
            this.txtDate.DateTime = DateTime.Now;
            this.mdecSumFRQty = 0;
            this.mdecSumMOQty = 0;

            this.pmSetFormEnable(true);

            this.pmClearRefTo();

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
            this.txtMfgQty.EditValue = 1;
            this.txtTotMfgQty.EditValue = 1;
            this.mdecUMQty = 1;
            this.mdecLastMfgQty = 0;
            this.mdecLastTotMfgQty = 0;
            this.mdecPdStOutput = 1;

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

            this.txtCostDL.Value = 0;
            this.txtCostOH.Value = 0;
            this.txtCostAdj1.Value = 0;
            this.txtCostAdj2.Value = 0;
            this.txtCostAdj3.Value = 0;
            this.txtCostAdj4.Value = 0;
            this.txtCostAdj5.Value = 0;

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

                    this.mstrStep = dtrLoadForm[QMFWCloseHDInfo.Field.Step].ToString();

                    this.mstrMasterWO = dtrLoadForm[QMFWCloseHDInfo.Field.MasterID].ToString().TrimEnd();
                    this.mstrParentWO = dtrLoadForm[QMFWCloseHDInfo.Field.ParentID].ToString().TrimEnd();
                    this.mstrLevel = dtrLoadForm[QMFWCloseHDInfo.Field.Level].ToString().TrimEnd();
                    this.mstrBatchNo = dtrLoadForm[QMFWCloseHDInfo.Field.BatchNo].ToString().TrimEnd();
                    this.mstrBatchRun = dtrLoadForm[QMFWCloseHDInfo.Field.BatchRun].ToString().TrimEnd();
                    this.txtCode.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Code].ToString().TrimEnd();
                    this.txtRefNo.Text = dtrLoadForm[QMFWCloseHDInfo.Field.RefNo].ToString().TrimEnd();
                    this.txtDate.DateTime = Convert.ToDateTime(dtrLoadForm[QMFWCloseHDInfo.Field.Date]).Date;

                    if (!Convert.IsDBNull(dtrLoadForm[QMFWCloseHDInfo.Field.StartDate]))
                    {
                        this.txtStartDate.EditValue = Convert.ToDateTime(dtrLoadForm[QMFWCloseHDInfo.Field.StartDate]).Date;
                    }
                    this.txtDueDate.EditValue = (Convert.IsDBNull(dtrLoadForm[QMFWCloseHDInfo.Field.DueDate]) ? this.txtDate.DateTime.Date : Convert.ToDateTime(dtrLoadForm[QMFWCloseHDInfo.Field.DueDate]).Date);
                    //this.cmbPriotiry.SelectedValue = dtrLoadForm["cPriority"].ToString();

                    //this.mdttReceDate = Convert.ToDateTime(dtrLoadForm["dReceDate"]).Date;
                    this.txtQcSect.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.SectID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcSect.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcCode from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcSect.Text = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.JobID].ToString().TrimEnd();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcJob.Tag });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcCode from Job where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcJob.Text = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                    }

                    this.txtQcCoor.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.CoorID].ToString().TrimEnd();
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

                    this.txtQcProd.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.MfgProdID].ToString();
                    pobjSQLUtil2.SetPara(new object[1] { this.txtQcProd.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode, fcUM from Prod where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        this.txtQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                        this.pmLoadStockBal(dtrProd["fcSkid"].ToString());

                        //if (pobjSQLUtil2.SetPara(new object[] { dtrProd["fcUM"].ToString() })
                        //    && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                        //{
                        //    this.txtQnUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                        //    this.txtQnUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                        //}

                    }

                    this.txtQcBOM.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.BOMID].ToString().TrimEnd();
                    pobjSQLUtil.SetPara(new object[1] { this.txtQcBOM.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode, cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        this.txtQcBOM.Text = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                        this.txtQnBOM.Text = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                        //this.mstrLastQcPdStH = this.txtQcBOM.Text;
                    }

                    this.txtQnMfgUM.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.MfgUMID].ToString();
                    if (pobjSQLUtil2.SetPara(new object[] { this.txtQnMfgUM.Tag.ToString() })
                        && pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVFLD_UM", "UM", "select fcSkid,fcCode,fcName from UM where fcSkid = ? ", ref strErrorMsg))
                    {
                        this.txtQnMfgUM.Tag = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcSkid"].ToString().TrimEnd();
                        this.txtQnMfgUM.Text = this.dtsDataEnv.Tables["QVFLD_UM"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    this.txtMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.MfgQty]);
                    this.mdecLastMfgQty = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.MfgQty]);
                    this.mdecUMQty = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.MfgUMQty]);
                    this.txtTotMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.Qty]);

                    this.pmSetUMQtyMsg();

                    this.txtScrap.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Scrap].ToString().TrimEnd();

                    this.txtRemark1.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark1].ToString().TrimEnd();
                    this.txtRemark2.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark2].ToString().TrimEnd();
                    this.txtRemark3.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark3].ToString().TrimEnd();
                    this.txtRemark4.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark4].ToString().TrimEnd();
                    this.txtRemark5.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark5].ToString().TrimEnd();
                    this.txtRemark6.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark6].ToString().TrimEnd();
                    this.txtRemark7.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark7].ToString().TrimEnd();
                    this.txtRemark8.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark8].ToString().TrimEnd();
                    this.txtRemark9.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark9].ToString().TrimEnd();
                    this.txtRemark10.Text = dtrLoadForm[QMFWCloseHDInfo.Field.Remark10].ToString().TrimEnd();

                    this.txtCostDL.Value= Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostDL]);
                    this.txtCostOH.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostOH]);
                    this.txtCostAdj1.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostAdj1]);
                    this.txtCostAdj2.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostAdj2]);
                    this.txtCostAdj3.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostAdj3]);
                    this.txtCostAdj4.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostAdj4]);
                    this.txtCostAdj5.Value = Convert.ToDecimal(dtrLoadForm[QMFWCloseHDInfo.Field.CostAdj5]);

                    this.txtApproveBy.Tag = dtrLoadForm[QMFWCloseHDInfo.Field.ApproveBy].ToString();
                    string strIsApprove = dtrLoadForm[QMFWCloseHDInfo.Field.IsApprove].ToString().Trim();
                    string strApprove = "";
                    switch (BudgetHelper.GetApproveStep(strIsApprove))
                    {
                        case ApproveStep.Wait:
                            strApprove = " ";
                            this.txtDApprove.EditValue = null;
                            break;
                        case ApproveStep.Approve:
                            strApprove = "/";
                            this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QMFWCloseHDInfo.Field.ApproveDate]);
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

                    this.pmLoadRefTo();

                    this.pmLoadBOMIT_StdOP(this.mstrTemOP);
                    this.pmLoadBOMIT_Pd(this.mstrTemFG);
                    this.pmLoadBOMIT_Pd(this.mstrTemPd);

                    this.mstrLastScrap = this.txtScrap.Text.Trim();
                    this.mdecLastMfgQty = Convert.ToDecimal(this.txtMfgQty.EditValue);
                    this.mdecLastTotMfgQty = Convert.ToDecimal(this.txtTotMfgQty.EditValue);

                    //this.pmLoadRefTo();

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadRefTo()
        {
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

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
                    this.mstrOldRefToRowID = this.mstrRefToRowID;
                    this.txtRefTo.Text = dtrRefTo["cRefNo"].ToString().TrimEnd();

                    this.pmSetFormEnable(false);
                
                }
            }
        }

        private void pmLoadBOMIT_StdOP(string inAlias)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBomIT_StdOP", "WkCtrIT", "select * from " + this.mstrITable + " where cWCloseH = ? order by cSeq", ref strErrorMsg))
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

                    dtrNewRow["NQTY"] = Convert.ToDecimal(dtrBomIT["NQTY"]);
                    dtrNewRow["NWASTEQTY"] = Convert.ToDecimal(dtrBomIT["NWASTEQTY"]);
                    dtrNewRow["NLOSSQTY1"] = Convert.ToDecimal(dtrBomIT["NLOSSQTY1"]);

                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Queue] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Queue]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_SetUp]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Process] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Process]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Tear] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Tear]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Wait] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Wait]);
                    dtrNewRow[QMFWCloseIT_OPInfo.Field.LeadTime_Move] = Convert.ToDecimal(dtrBomIT[QMFWCloseIT_OPInfo.Field.LeadTime_Move]);

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
            string strSQLStr = "select * from " + this.mstrITable2 + " where cWCloseH = ? and cIOType = ? order by cSeq ";
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
            dtrTemPd["nMOQty"] = Convert.ToDecimal(inWOrderI["nMOQty"]);
            dtrTemPd["nQty"] = Convert.ToDecimal(inWOrderI["nQty"]);
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

            dtrTemPd["cRef2RefTy"] = inWOrderI["cRef2RefTy"].ToString();
            dtrTemPd["cRef2Head"] = inWOrderI["cRef2Head"].ToString();
            dtrTemPd["cRef2Item"] = inWOrderI["cRef2Item"].ToString();

            if (inAlias == this.mstrTemFG
                && dtrTemPd["cRef2Head"].ToString().Trim() == "")
            {
                DataRow[] dtrSelRef = this.dtsDataEnv.Tables[this.mstrTemFG].Select("cQcProd = '" + this.txtQcProd.Text.TrimEnd() + "' and cRef2Head <> '' ");
                if (dtrSelRef.Length > 0)
                {
                    DataRow dtrRef = dtrSelRef[0];
                    string strSQLStr_WOrderI = "";
                    strSQLStr_WOrderI = "select MFWORDERIT_PD.* from REFDOC_STMOVE";
                    strSQLStr_WOrderI += " left join MFWORDERIT_PD on MFWORDERIT_PD.CROWID = REFDOC_STMOVE.CCHILDI ";
                    strSQLStr_WOrderI += " where REFDOC_STMOVE.CMASTERTYP = ? and REFDOC_STMOVE.CMASTERH = ? and REFDOC_STMOVE.CMASTERI = ? ";
                    pobjSQLUtil2.SetPara(new object[] { dtrRef["cRef2RefTy"].ToString(), dtrRef["cRef2Head"].ToString(), dtrRef["cRef2Item"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, strSQLStr_WOrderI, ref strErrorMsg))
                    {
                        DataRow dtrWOrderI = this.dtsDataEnv.Tables["QWOrderI"].Rows[0];
                        dtrTemPd["cWOrderH"] = dtrWOrderI["cWOrderH"].ToString();
                        dtrTemPd["cWOrderI"] = dtrWOrderI["cRowID"].ToString();
                        dtrTemPd["nMOQty"] = Convert.ToDecimal(dtrWOrderI["nQty"]);
                    }

                    string strSQLStr_WOrderI_FR = "";
                    strSQLStr_WOrderI_FR = "select MFWORDERIT_STDOP.cRowID from REFDOC";
                    strSQLStr_WOrderI_FR += " left join MFWORDERIT_STDOP on MFWORDERIT_STDOP.CROWID = REFDOC.CCHILDI ";
                    strSQLStr_WOrderI_FR += " where REFDOC.CCHILDTYPE = ? and REFDOC.CCHILDH = ? and REFDOC.CMASTERTYP = ? and REFDOC.CMASTERH = ? ";
                    pobjSQLUtil2.SetPara(new object[] { DocumentType.MO.ToString(), this.mstrRefToRowID, dtrRef["cRef2RefTy"].ToString(), dtrRef["cRef2Head"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderOP", QMFWOrderIT_PdInfo.TableName, strSQLStr_WOrderI_FR, ref strErrorMsg))
                    {
                        DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderOP"].Rows[0];
                        dtrTemPd["cWOrderOP"] = dtrWOrderOP["cRowID"].ToString();
                    }
                }
                else
                {

                    string strWOrderOP = "";
                    string strWOrderOPSeq = "";

                    pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRowID });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderOP", QMFWOrderIT_OPInfo.TableName, "select top 1 * from " + QMFWOrderIT_OPInfo.TableName + " where CWORDERH = ? order by COPSEQ desc", ref strErrorMsg))
                    {
                        DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderOP"].Rows[0];
                        dtrTemPd["cWOrderOP"] = dtrWOrderOP["cRowID"].ToString();
                    }

                    pobjSQLUtil2.SetPara(new object[] { this.mstrRefToRowID, this.txtQcProd.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", QMFWOrderIT_PdInfo.TableName, "select top 1 * from " + QMFWOrderIT_PdInfo.TableName + " where CWORDERH = ? and cProd = ? order by cSeq", ref strErrorMsg))
                    {
                        DataRow dtrWOrderI = this.dtsDataEnv.Tables["QWOrderI"].Rows[0];
                        dtrTemPd["cWOrderH"] = dtrWOrderI["cWOrderH"].ToString();
                        dtrTemPd["cWOrderI"] = dtrWOrderI["cRowID"].ToString();
                        dtrTemPd["nMOQty"] = Convert.ToDecimal(dtrWOrderI["nQty"]);
                    }

                }
            }
            
            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cRef2Head"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", "select BOOK.FCCODE as QCBOOK, GLREF.FCREFTYPE, GLREF.FCCODE from GLREF left join BOOK on BOOK.FCSKID = GLREF.FCBOOK where GLREF.fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrGLRef = this.dtsDataEnv.Tables["QGLRef"].Rows[0];
                dtrTemPd["cRefTo_Code"] = dtrGLRef["fcRefType"].ToString().TrimEnd() + dtrGLRef["QcBook"].ToString().TrimEnd() + "/" + dtrGLRef["fcCode"].ToString().TrimEnd();
            }

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
            if (inOrderBy.ToUpper() == QMFWCloseHDInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFWCloseHDInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtRefNo.Properties.MaxLength);
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

        private void pmAdjustMfgQty()
        {
            this.pmAdjustQty(this.mstrTemFG);
            this.pmAdjustQty(this.mstrTemPd);
            this.pmAdjustQty2(this.mstrTemOP);
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

                    //decimal decQty = Math.Round(decQty1 / this.mdecLastTotMfgQty * decMfgQty, App.ActiveCorp.RoundQtyAt, MidpointRounding.AwayFromZero);
                    //decimal decQty1 = Convert.ToDecimal(dtrTemPd["nQty"]) - Convert.ToDecimal(dtrTemPd["nScrapQty"]);
                    decimal decQty1 = Convert.ToDecimal(dtrTemPd["nSaveQty"]);
                    decimal decQty = decQty1 / this.mdecLastTotMfgQty * decMfgQty;
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

                pobjSQLUtil.SetPara(new object[] { this.mstrOldRefToRowID });
                pobjSQLUtil.SQLExec("update MFWORDERHD set CMSTEP = '' where cRowID = ?", ref strErrorMsg);
            
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
                dtrREFDOC["cChildI"] = "";

                string strSQLUpdateStr_RefTo = "";
                DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
                pobjSQLUtil.SetPara(pAPara);
                bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);

                pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
                pobjSQLUtil.SQLExec("update MFWORDERHD set CMSTEP = 'P' where cRowID = ?", ref strErrorMsg);

                //JOBNO: 1005001 : แก้ BUG เรื่องการ Update cParent
                this.pmUpdateChildMC();

            }
            else
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg);

                pobjSQLUtil.SetPara(new object[] { this.mstrRefToRowID });
                pobjSQLUtil.SQLExec("update MFWORDERHD set CMSTEP = '' where cRowID = ?", ref strErrorMsg);

            }

            return bllResult;
        }

        private void pmUpdateChildMC()
        {
            bool bllResult = true;
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;

            string strSQLStr = "select CCORP,CROWID,CMASTER,CPARENT,CCODE,CREFNO from MFWCLOSEHD where CROWID in ";
            strSQLStr += " (select CMASTERH from REFDOC where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH in (select CROWID from MFWORDERHD where CPARENT = ? ) ) ";

            pobjSQLUtil.SetPara(new object[] { this.mstrRefType, this.mstrRefToRefType, this.mstrRefToRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChildMC", "REFDOC", strSQLStr, ref strErrorMsg);
            foreach (DataRow dtrMCloseH in this.dtsDataEnv.Tables["QChildMC"].Rows)
            {
                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, dtrMCloseH["cRowID"].ToString() });
                pobjSQLUtil.SQLExec("update MFWCLOSEHD set CPARENT = ? where cRowID = ?", ref strErrorMsg);
            }
        }

        private decimal pmGetAVGPrice(string inProd, DateTime inDate)
        {

            string strErrorMsg = "";

            decimal decPrice = 0;

            decimal tSign = 1;
            decimal tCostAmt = 0;
            decimal tRefCost = 0;
            decimal tQty = 0;
            decimal tAmt = 0;
            decimal outAmt = 0;
            decimal outQty = 0;


            string strFldLst = " PROD.FCCODE as QCPROD, RefProd.fcIOType, RefProd.fcRfType, RefProd.fcIOType, RefProd.FNQTY, RefProd.FNUMQTY , RefProd.fnCostAmt, RefProd.fnCostAdj ";
            string strSQLStr = "select " + strFldLst + " from REFPROD ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE";
            strSQLStr = strSQLStr + " where REFPROD.FCPROD = ?";
            strSQLStr = strSQLStr + " and REFPROD.FCBRANCH = ?";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE <= ?";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIoType";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { inProd, this.mstrBranch, inDate.Date });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCalPrice", "REFPROD", strSQLStr, ref strErrorMsg);
            foreach (DataRow drBFQty in this.dtsDataEnv.Tables["QCalPrice"].Rows)
            {

                tSign = (drBFQty["fcIoType"].ToString() == "I" ? 1 : -1);
                tQty = Convert.ToDecimal(drBFQty["fnQty"]) * Convert.ToDecimal(drBFQty["fnUmQty"]) * tSign;
                tCostAmt = Math.Abs(Convert.ToDecimal(drBFQty["fnCostAmt"]) + Convert.ToDecimal(drBFQty["fnCostAdj"]));

                if (tQty >= 0)
                {

                    tRefCost = (tQty != 0 ? tCostAmt / tQty : 0);
                    if (drBFQty["fcRfType"].ToString() == "F")
                    {
                        tRefCost = (outQty != 0 ? outAmt / outQty : 0);
                    }

                    tAmt = Math.Round(tQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    if (this.faPRefProd(drBFQty))
                    {

                        outAmt = outAmt + tAmt;
                        outQty = outQty + tQty;

                        if (outQty <= 0)
                            outAmt = 0;
                        else if (outQty < tQty)
                            //outAmt = outQty * tRefCost;
                            outAmt = Math.Round(outQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                        else if (outAmt < 0)
                            outAmt = 0;
                    }
                }
                else
                {

                    if (outQty != 0)
                    {
                        tRefCost = outAmt / outQty;
                        tAmt = Math.Round(tQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        tRefCost = 0;
                        tAmt = 0;
                    }

                    if (this.faPRefProd(drBFQty))
                    {

                        outAmt = outAmt + tAmt;
                        outQty = outQty + tQty;

                        if (outQty <= 0)
                            outAmt = 0;
                        else if (outAmt < 0)
                            outAmt = 0;
                    }

                }

            }

            return (outQty != 0 ? outAmt / outQty : 0);
        }

        //private string pstrXtraType = SysDef.gc_RFTYPE_TRAN_PD + SysDef.gc_RFTYPE_ISSUE_PD + SysDef.gc_RFTYPE_PRODUCE_PD + SysDef.gc_RFTYPE_RETURN_ISSUE;
        private string pstrXtraType = SysDef.gc_RFTYPE_ISSUE_PD + SysDef.gc_RFTYPE_PRODUCE_PD + SysDef.gc_RFTYPE_RETURN_ISSUE;

        private bool faPRefProd(DataRow inRow)
        {
            decimal tSign = (inRow["fcIoType"].ToString() == "I" ? 1 : -1);
            decimal tQty = Convert.ToDecimal(inRow["fnQty"]) * Convert.ToDecimal(inRow["fnUmQty"]) * tSign;

            bool bllResult = (pstrXtraType.IndexOf(inRow["fcRfType"].ToString().Trim()) < 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_ISSUE_PD && tQty < 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_PRODUCE_PD && tQty >= 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_RETURN_ISSUE && tQty >= 0);

            return bllResult;
        }

        private decimal pmPrintStock(string inProd, DateTime inDate)
        {

            string strSQLStr = "select ";
            strSQLStr = strSQLStr + " REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " , (select PROD.FCCODE from PROD where PROD.FCSKID = REFPROD.FCPROD) as QcProd";
            strSQLStr = strSQLStr + " , (select PROD.FCNAME from PROD where PROD.FCSKID = REFPROD.FCPROD) as QnProd";
            strSQLStr = strSQLStr + " , (select UM.FCNAME from UM where UM.FCSKID = REFPROD.FCUM) as QnUM";
            strSQLStr = strSQLStr + " , REFPROD.FCIOTYPE as IOType , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as SumQty ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " left join UM on UM.FCSKID = PROD.FCUM ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = @xaCorp and REFPROD.FCBRANCH = @xaBranch and REFPROD.FCPROD = @xaProd and REFPROD.FDDATE <= @xaDate and REFPROD.FCSTAT <> 'C'  ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD, REFPROD.FCIOTYPE, REFPROD.FCUM ";
            strSQLStr = strSQLStr + " order by QcProd ";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(App.ERPConnectionString2);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQLStr, conn);
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@xaCorp", App.ActiveCorp.RowID);
            cmd.Parameters.AddWithValue("@xaBranch", this.mstrBranch);
            cmd.Parameters.AddWithValue("@xaProd", inProd);
            cmd.Parameters.AddWithValue("@xaDate", inDate.Date);

            decimal decSumQty = 0;
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            try
            {
                conn.Open();
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        decimal decQty = Convert.ToDecimal(dr["SumQty"]) * (dr["IOType"].ToString() == "I" ? 1 : -1);
                        decSumQty += decQty;
                        //this.pmAddProd(dr["fcProd"].ToString(), dr["QcProd"].ToString(), dr["QnProd"].ToString(), dr["QnUM"].ToString(), decQty);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                App.WriteEventsLog(ex);
            }
            finally
            {
                conn.Close();
            }
            return decSumQty;
        }

        private void pmAddProd(string inProd, string inQcProd, string inQnProd, string inQnUM, decimal inQty)
        {
            string strFilter = string.Format("QcProd = '{0}' ", new string[] { inQcProd });
            DataRow[] strSel = this.dtsDataEnv.Tables[this.mstrTemPd].Select(strFilter);
            if (strSel.Length == 0)
            {
                DataRow dtrPreview = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                dtrPreview["cProd"] = inProd;
                dtrPreview["QcProd"] = inQcProd;
                dtrPreview["QnProd"] = inQnProd;
                dtrPreview["QnUM"] = inQnUM;
                dtrPreview["Qty"] = inQty;
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrPreview);
            }
            else
            {
                strSel[0]["Qty"] = Convert.ToDecimal(strSel[0]["Qty"]) + inQty;
            }

        }

        #region "FIFO Method"
        private void P1Line(DataRow inSource, decimal inQty, decimal inCost, decimal inAmt, ref string ioSedRefNo)
        {

            //DataRow dtrTemFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].NewRow();
            DataRow dtrTemFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].NewRow();

            dtrTemFIFO["QcBook"] = inSource["QcBook"].ToString();
            dtrTemFIFO["SedRefNo"] = ioSedRefNo;
            dtrTemFIFO["fcRfType"] = inSource["fcRfType"].ToString();
            dtrTemFIFO["fcRefType"] = inSource["fcRefType"].ToString();
            dtrTemFIFO["fcRefNo"] = inSource["fcRefNo"].ToString();
            dtrTemFIFO["fdDate"] = Convert.ToDateTime(inSource["fdDate"]);
            dtrTemFIFO["cDate"] = Convert.ToDateTime(inSource["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(inSource["fdDate"]).ToString("MMdd") + inSource["fcRefNo"].ToString() + inSource["fcSkid"].ToString();
            //dtrTemFIFO["QcPdGrp"] = inSource["QcPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp"] = inSource["QnPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp2"] = inSource["QnPdGrp2"].ToString();
            //dtrTemFIFO["QcProd"] = inSource["QcProd"].ToString();
            //dtrTemFIFO["QnProd"] = inSource["QnProd"].ToString();
            //dtrTemFIFO["QnUM"] = inSource["QnUM"].ToString();
            //dtrTemFIFO["QcSect"] = inSource["QcSect"].ToString();
            //dtrTemFIFO["QnSect"] = inSource["QnSect"].ToString();
            //dtrTemFIFO["QcJob"] = inSource["QcJob"].ToString();
            //dtrTemFIFO["QnJob"] = inSource["QnJob"].ToString();
            //dtrTemFIFO["fcIOType"] = inSource["fcIOType"].ToString();
            dtrTemFIFO["fcIOType"] = "I";
            dtrTemFIFO["fnQty"] = inQty;
            dtrTemFIFO["fnUMQty"] = 1;
            dtrTemFIFO["fnPrice"] = inCost;
            dtrTemFIFO["fnCostAmt"] = inAmt;

            //switch (inSource["fcRfType"].ToString())
            //{
            //    case "S":
            //    case "E":
            //    case "F":
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnPriceKe"]);
            //        break;
            //    default:
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnSellPrice"]);
            //        break;
            //}

            //if (ioSedRefNo.Trim() == string.Empty && inSource["fcIOType"].ToString().Trim() == "I")
            if (ioSedRefNo.Trim() == string.Empty)
            {
                ioSedRefNo = dtrTemFIFO["cDate"].ToString();
            }
            //this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Rows.Add(dtrTemFIFO);
            //GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows.Add(dtrTemFIFO);

        }

        private void XXP1Line(SqlDataReader inSource, decimal inQty, decimal inCost, decimal inAmt, ref string ioSedRefNo)
        {

            //DataRow dtrTemFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].NewRow();
            DataRow dtrTemFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].NewRow();

            dtrTemFIFO["QcBook"] = inSource["QcBook"].ToString();
            dtrTemFIFO["SedRefNo"] = ioSedRefNo;
            dtrTemFIFO["fcRfType"] = inSource["fcRfType"].ToString();
            dtrTemFIFO["fcRefType"] = inSource["fcRefType"].ToString();
            dtrTemFIFO["fcRefNo"] = inSource["fcRefNo"].ToString();
            dtrTemFIFO["fdDate"] = Convert.ToDateTime(inSource["fdDate"]);
            dtrTemFIFO["cDate"] = Convert.ToDateTime(inSource["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(inSource["fdDate"]).ToString("MMdd") + inSource["fcRefNo"].ToString() + inSource["fcSkid"].ToString();
            //dtrTemFIFO["QcPdGrp"] = inSource["QcPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp"] = inSource["QnPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp2"] = inSource["QnPdGrp2"].ToString();
            //dtrTemFIFO["QcProd"] = inSource["QcProd"].ToString();
            //dtrTemFIFO["QnProd"] = inSource["QnProd"].ToString();
            //dtrTemFIFO["QnUM"] = inSource["QnUM"].ToString();
            //dtrTemFIFO["QcSect"] = inSource["QcSect"].ToString();
            //dtrTemFIFO["QnSect"] = inSource["QnSect"].ToString();
            //dtrTemFIFO["QcJob"] = inSource["QcJob"].ToString();
            //dtrTemFIFO["QnJob"] = inSource["QnJob"].ToString();
            //dtrTemFIFO["fcIOType"] = inSource["fcIOType"].ToString();
            dtrTemFIFO["fcIOType"] = "I";
            dtrTemFIFO["fnQty"] = inQty;
            dtrTemFIFO["fnUMQty"] = 1;
            dtrTemFIFO["fnPrice"] = inCost;
            dtrTemFIFO["fnCostAmt"] = inAmt;

            //switch (inSource["fcRfType"].ToString())
            //{
            //    case "S":
            //    case "E":
            //    case "F":
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnPriceKe"]);
            //        break;
            //    default:
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnSellPrice"]);
            //        break;
            //}

            //if (ioSedRefNo.Trim() == string.Empty && inSource["fcIOType"].ToString().Trim() == "I")
            if (ioSedRefNo.Trim() == string.Empty)
            {
                ioSedRefNo = dtrTemFIFO["cDate"].ToString();
            }
            //this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Rows.Add(dtrTemFIFO);
            GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows.Add(dtrTemFIFO);

        }

        private void SumSed(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");

            DataRow[] dtrTemBFFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Select("fcIOType = 'I' ", "nRecNo");
            //DataRow[] dtrTemBFFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Select("fcIOType = 'I' ", "nRecNo desc");
            
            ioSedQty = 0;
            ioSedCost = 0;
            bool tFstIn = true;
            int intCurrRecn = 0;
            if (ioSedRecn >= 0 && ioSedRecn < dtrTemBFFIFO.Length)
            {
                intCurrRecn = ioSedRecn;
                if (ioSedQty2 > 0)
                {
                    ioSedQty = ioSedQty2;
                    decimal decBFAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                    decimal decBFQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                    ioSedCost = CostSeq2(decBFAmt, decBFQty);
                    tFstIn = false;
                }
                intCurrRecn++;
            }
            ioSedQty2 = 0;
            //By Yod : 7/6/54 
            //if (ioSedQty == 0) ioSedRecn = 0;

            while (intCurrRecn < dtrTemBFFIFO.Length)
            {
                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                decimal tQty1 = QtySeq1(tCostAmt, tAbsQty);
                decimal tCost1 = CostSeq1(tCostAmt, tAbsQty);
                decimal tCost2 = CostSeq2(tCostAmt, tAbsQty);
                if (tFstIn)
                {
                    tFstIn = false;
                    ioSedCost = tCost1;
                }
                if (ioSedCost == tCost1)
                {
                    ioSedQty = ioSedQty + tQty1;
                    ioSedRecn = intCurrRecn;
                    ioSedRefNo = dtrTemBFFIFO[intCurrRecn]["cDate"].ToString();
                    if ((tCost1 != tCost2) || (Math.Abs(tQty1) != tAbsQty))
                    {
                        ioSedQty2 = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"])) - tQty1;
                        break;
                    }

                }
                else
                {
                    break;
                }
                intCurrRecn++;
            }
        }

        private void SumSedNegative(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");
            DataRow[] dtrTemBFFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Select("fcIOType = 'I' ", "nRecNo");
            //ioSedQty = 0;
            ioSedCost = 0;
            bool tFstIn = true;
            int intCurrRecn = 0;
            if (ioSedRecn >= 0 && ioSedRecn < dtrTemBFFIFO.Length)
            {
                intCurrRecn = ioSedRecn;
                if (ioSedQty2 > 0)
                {
                    ioSedQty = ioSedQty2;
                    decimal decBFAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                    decimal decBFQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                    ioSedCost = CostSeq2(decBFAmt, decBFQty);
                    tFstIn = false;
                }
                intCurrRecn++;
            }
            ioSedQty2 = 0;
            //By Yod : 7/6/54 
            if (ioSedQty == 0) intCurrRecn = intCurrRecn - 1;

            while (intCurrRecn < dtrTemBFFIFO.Length)
            {
                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                decimal tQty1 = QtySeq1(tCostAmt, tAbsQty);
                decimal tCost1 = CostSeq1(tCostAmt, tAbsQty);
                decimal tCost2 = CostSeq2(tCostAmt, tAbsQty);
                if (tFstIn)
                {
                    tFstIn = false;
                    ioSedCost = tCost1;
                }
                if (ioSedCost == tCost1)
                {
                    ioSedQty = ioSedQty + tQty1;
                    ioSedRecn = intCurrRecn;
                    ioSedRefNo = dtrTemBFFIFO[intCurrRecn]["cDate"].ToString();
                    if ((tCost1 != tCost2) || (Math.Abs(tQty1) != tAbsQty))
                    {
                        ioSedQty2 = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"])) - tQty1;
                        break;
                    }

                }
                else
                {
                    break;
                }
                intCurrRecn++;
            }
        }

        private decimal QtySeq1(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            decimal tAmt = (inCostAmt);
            decimal tCost = CostSeq1(inCostAmt, inQty);
            //decimal tQty = Math.Round(inQty, 4);
            decimal tQty = inQty;
            return tQty;
        }

        private decimal CostSeq1(decimal inCostAmt, decimal inQty)
        {
            //return Math.Round(Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt) / inQty, App.ActiveCorp.RoundPriceAt);
            return (inQty != 0 ? inCostAmt / inQty : 0);
        }

        private decimal CostSeq2(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            //decimal tCost2 = Math.Round(tAmt / inQty, App.ActiveCorp.RoundPriceAt);
            decimal tAmt = inCostAmt;
            decimal tCost2 = (inQty != 0 ? tAmt / inQty : 0);
            return tCost2;
        }


        private void pmPrint1Prod(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;
            decimal xaSedQty = ioSedQty;
            string xaSedRefNo = ioSedRefNo;
            decimal xaSedQty2 = ioSedQty2;
            decimal xaSedCost = ioSedCost;
            int xaSedRecn = ioSedRecn;
            bool tFstPSed = false;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot,RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM, Prod.fnPrice as fnSellPrice";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp , PdGrp.fcName2 as QnPdGrp2";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
            //if (this.txtTagWHouse.Text.TrimEnd() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.TrimEnd() + " )";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef  , RefProd.fcSkid ";
            //lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;


            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XABEGDATE", inBegDate.Date);
            cmd.Parameters.AddWithValue("@XAENDDATE", inEndDate.Date);

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                //while (dtrRefProd.Read() && tQty > 0)
                while (dtrRefProd.Read())
                {

                    if (VRefProd3(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()))
                    {

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        decimal tRefCost = 0;

                        decimal tAmt = 0;
                        decimal tAmt1 = 0;
                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCostAdj = 0;
                        decimal tInAmount = 0;
                        if (tRefPdQty >= 0)
                        {

                            #region "รายการฝั่งรับ"
                            //รายการฝั่งรับ
                            tAmt = Math.Round(tCostAmt, 4, MidpointRounding.AwayFromZero);
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);

                            tAmt1 = tCost1 * tQty1;
                            tCostAdj = tCostAmt;
                            tInAmount = tAmt1 + (tAmt - tAmt1);

                            //= P1Line( tDate , tRefNo , tQty1 , tCost1 , tAmt1 , 'I' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tInAmount , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            //P1Line(dtrRefProd, tQty1, tCost1, tAmt1, ref xaSedRefNo);

                            //For Test 16/1/2013
                            //pmAddRowIn(tAbsQty, tAbsQty, tCostAmt / tAbsQty, dtrRefProd["fcRefNo"].ToString());
                            
                            tQty2 = tAbsQty - tQty1;

                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }
                            #endregion

                        }
                        else
                        {

                            #region "รายการฝั่งจ่าย"

                            pmCutFIFO(dtrRefProd, Convert.ToDecimal(dtrRefProd["fnQty"]), dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSeq"].ToString());
                            //P1Line(dtrRefProd, xaSedQty, tPCost, tAmt, xaSedRefNo);

                            #endregion

                        }

                    }
                }
            }

            conn.Close();

        }

        private void pmPrint1Prod_Add(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;
            decimal xaSedQty = ioSedQty;
            string xaSedRefNo = ioSedRefNo;
            decimal xaSedQty2 = ioSedQty2;
            decimal xaSedCost = ioSedCost;
            int xaSedRecn = ioSedRecn;
            bool tFstPSed = false;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot,RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM, Prod.fnPrice as fnSellPrice";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp , PdGrp.fcName2 as QnPdGrp2";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
            //if (this.txtTagWHouse.Text.TrimEnd() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.TrimEnd() + " )";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            //lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef  , RefProd.fcSkid ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;


            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XABEGDATE", inBegDate.Date);
            cmd.Parameters.AddWithValue("@XAENDDATE", inEndDate.Date);

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                //while (dtrRefProd.Read() && tQty > 0)
                while (dtrRefProd.Read())
                {

                    if (VRefProd3(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()))
                    {

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        decimal tRefCost = 0;

                        decimal tAmt = 0;
                        decimal tAmt1 = 0;
                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCostAdj = 0;
                        decimal tInAmount = 0;
                        if (tRefPdQty >= 0)
                        {

                            #region "รายการฝั่งรับ"
                            //รายการฝั่งรับ
                            tAmt = Math.Round(tCostAmt, 4, MidpointRounding.AwayFromZero);
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);

                            tAmt1 = tCost1 * tQty1;
                            tCostAdj = tCostAmt;
                            tInAmount = tAmt1 + (tAmt - tAmt1);

                            //= P1Line( tDate , tRefNo , tQty1 , tCost1 , tAmt1 , 'I' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tInAmount , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            //P1Line(dtrRefProd, tQty1, tCost1, tAmt1, ref xaSedRefNo);

                            //For Test 16/1/2013
                            pmAddRowIn(tAbsQty, tAbsQty, tCostAmt / tAbsQty, dtrRefProd["fcRefNo"].ToString());

                            tQty2 = tAbsQty - tQty1;

                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }
                            #endregion

                        }
                        else
                        {

                            #region "รายการฝั่งจ่าย"

                            //pmCutFIFO(dtrRefProd, Convert.ToDecimal(dtrRefProd["fnQty"]), dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSeq"].ToString());
                            //P1Line(dtrRefProd, xaSedQty, tPCost, tAmt, xaSedRefNo);

                            #endregion

                        }

                    }
                }
            }

            conn.Close();

        }

        private void pmAddRowIn(decimal inQty, decimal inBalQty, decimal inPrice, string inRefNo)
        {
            DataRow dtrN1 = this.dtsDataEnv.Tables["TemIn"].NewRow();
            dtrN1["Qty"] = inQty;
            dtrN1["Cost"] = inPrice;
            dtrN1["BalQty"] = inBalQty;
            dtrN1["Amt"] = inPrice * inQty;
            dtrN1["RefNo"] = inRefNo;
            this.dtsDataEnv.Tables["TemIn"].Rows.Add(dtrN1);
        }

        private void pmAddRowOut(decimal inQty, string inRefNoSeq, decimal inPrice)
        {
            DataRow dtrN1 = this.dtsDataEnv.Tables["TemOut"].NewRow();
            dtrN1["Qty"] = inQty;
            dtrN1["Cost"] = inPrice;
            dtrN1["RefNoSeq"] = inRefNoSeq;
            dtrN1["Amt"] = inPrice * inQty;
            this.dtsDataEnv.Tables["TemOut"].Rows.Add(dtrN1);
        }

        private void pmCutFIFO(SqlDataReader inSource, decimal inCutQty, string inRefNo)
        {
            decimal decCutQty = inCutQty;
            DataRow[] dtrInRow = this.dtsDataEnv.Tables["TemIn"].Select("BalQty > 0", "ID desc");
            for (int i = 0; i < dtrInRow.Length; i++)
            {
                decimal decBalQty = Convert.ToDecimal(dtrInRow[i]["BalQty"]);
                decimal decCost = Convert.ToDecimal(dtrInRow[i]["Cost"]);

                if (decBalQty > decCutQty)
                {
                    pmAddRowOut(decCutQty, inRefNo, decCost);
                    dtrInRow[i]["BalQty"] = decBalQty - decCutQty;
                    decCutQty = 0;
                    break;
                }
                else
                {
                    pmAddRowOut(decBalQty, inRefNo, decCost);
                    decCutQty = decCutQty - decBalQty;
                    dtrInRow[i]["BalQty"] = 0;
                }

            }

            if (decCutQty > 0)
            {
                pmAddRowOut(decCutQty, inRefNo, 0);
            }

        }

        //private string xaXtraType = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;
        private string xaXtraType = SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;

        private bool VRefProd3(string inRfType, string inIOType)
        {
            int intSearch = xaXtraType.IndexOf(inRfType);
            return (!(intSearch > -1)
                || (inRfType == SysDef.gc_RFTYPE_ISSUE_PD && inIOType == "O")
                || (inRfType == SysDef.gc_RFTYPE_PRODUCE_PD && inIOType == "I")
                || (inRfType == SysDef.gc_RFTYPE_RETURN_ISSUE && inIOType == "I"));
        }

        #endregion


    }
}
