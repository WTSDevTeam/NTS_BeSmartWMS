#define xd_RUNMODE_DEBUG

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

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;

using DevExpress.XtraGrid.Views.Base;

namespace BeSmartMRP.DatabaseForms
{
    
    public partial class frmMFBOM : UIHelper.frmBase, IfrmDBBase
    {

        public static string TASKNAME = "EMFBOM";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;
        private const int xd_PAGE_EDIT2 = 2;
        private const int xd_PAGE_EDIT3 = 3;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFBOMInfo.TableName;
        private string mstrITable = MapTable.Table.MFBOMIT_OP;
        private string mstrITable2 = MapTable.Table.MFBOMIT_PD;
        private string mstrCostLineTable = MapTable.Table.CostLine;

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QMFBOMInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";

        public const string xdCMRem = "Rem";
        public const string xdCMRem2 = "Rm2";
        public const string xdCMRem3 = "Rm3";
        public const string xdCMRem4 = "Rm4";
        public const string xdCMRem5 = "Rm5";
        public const string xdCMRem6 = "Rm6";
        public const string xdCMRem7 = "Rm7";
        public const string xdCMRem8 = "Rm8";
        public const string xdCMRem9 = "Rm9";
        public const string xdCMRem10 = "RmA";

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrTemPd = "TemPd";
        private string mstrTemOP = "TemOP";
        private string mstrTemPdX = "TemPdX";
        private string mstrTemPdX1 = "TemPdX1";

        private string mstrTemCost = "TemCost";
        private string mstrTemCostW = "TemCostW";
        private string mstrTemCostT = "TemCostT";
        private string mstrTem1Cost = "Tem1Cost";
        private string mstrTem1CostW = "Tem1CostW";
        private string mstrTem1CostT = "Tem1CostT";

        private string mstrEditRowID = "";
        private string mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;
        private string mstrPlant = "";
        private string mstrPdType = "";

        private string mstrFormMenuName = UIBase.GetAppUIText(new string[] { "BOM", "BOM" });

        private UIHelper.AppFormState mFormEditMode;

        private MfgResourceType mResourceType = MfgResourceType.Machine;

        private string mstrOldCode = "";
        private string mstrOldName = "";

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

        public frmMFBOM()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFBOM(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmMFBOM mInstanse_1 = null;

        public static frmMFBOM GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFBOM();
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

        private void frmMFBOM_Load(object sender, EventArgs e)
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
            this.pmInitGridProp_TemPd();
            this.pmInitGridProp_TemOP();

            this.pmInitializeComponent();
            this.pmFilterForm();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();

            this.mbllFilterResult = true;
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

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();
            this.pmSetFormUI();

            this.cmbType.Properties.Items.Clear();
            this.cmbType.Properties.Items.AddRange(new object[] { "M = Mfg. BOM", "P = Packaging" });
            this.cmbType.SelectedIndex = 0;

            this.cmdRoundCtrl.Properties.Items.Clear();
            this.cmdRoundCtrl.Properties.Items.AddRange(new object[] { 
                                                                                " "
                                                                                , "1 = " + UIBase.GetAppUIText(new string[] { "ปัดขึ้นเสมอ", "Round up" })
                                                                                , "2 = " + UIBase.GetAppUIText(new string[] { "ปัดลงเสมอ", "Round down" }) });
            this.cmdRoundCtrl.SelectedIndex = 0;

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
            switch (this.mResourceType)
            {
                case MfgResourceType.Machine:
                    this.mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;
                    break;
                case MfgResourceType.Tool:
                    this.mstrType = SysDef.gc_RESOURCE_TYPE_TOOL;
                    break;
            }


            this.lblCode.Text = string.Format("{0} {1} :", new string[] { 
                                                             UIBase.GetAppUIText(new string[] { "รหัส", this.mstrFormMenuName })
                                                            , UIBase.GetAppUIText(new string[] { this.mstrFormMenuName, "CODE" }) });

            this.lblName.Text = string.Format("{0} {1} :", new string[] { 
                                                            UIBase.GetAppUIText(new string[] { "ชื่อ", this.mstrFormMenuName })
                                                            , UIBase.GetAppUIText(new string[] { this.mstrFormMenuName, "NAME" }) });

            this.lblName2.Text = string.Format("{0} {1} :", new string[] { 
                                                            UIBase.GetAppUIText(new string[] { "ชื่อ", this.mstrFormMenuName })
                                                            , UIBase.GetAppUIText(new string[] { this.mstrFormMenuName, "NAME 2" })});

            this.lblApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "อนุมัติโดย", "Approve By" }) });
            this.lblDApprove.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่", "Date" }) });
            this.lblLastUpdate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "แก้ไขล่าสุด", "Last Update " }) });
            this.lblLastUpdBy.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "โดย", "By" }) });

            this.lblMfgProd.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "สินค้าที่ผลิตได้", this.mstrFormMenuName + " Output" }) });
            this.lblMfgQty.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "จำนวนที่ผลิตได้", "BOM Output Qty." }) });
            this.lblMfgUM.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "หน่วยนับผลิต", "Mfg. Unit of Measure" }) });
            this.lblStdUM.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "หน่วยนับมาตรฐาน", "Std. Unit of Measure" }) });
            this.btnUMConvert.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "ระบุอัตราแปลงหน่วย", "Unit Conversion" }) });

            this.lblRound.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "ปัดเศษ", "Rounding" }) });

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Name2);

            this.txtRemark1.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark1);
            this.txtRemark2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark2);
            this.txtRemark3.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark3);
            this.txtRemark4.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark4);
            this.txtRemark5.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark5);
            this.txtRemark6.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark6);
            this.txtRemark7.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark7);
            this.txtRemark8.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark8);
            this.txtRemark9.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark9);
            this.txtRemark10.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFBOMInfo.Field.Remark10);

        }

        private void pmMapEvent()
        {
            this.Resize += new EventHandler(frmMFBOM_Resize);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.grdTemOP.Resize += new EventHandler(grdTemOP_Resize);
            this.gridView2.ColumnWidthChanged += new ColumnEventHandler(gridView2_ColumnWidthChanged);
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            this.gridView2.GotFocus += new EventHandler(gridView2_GotFocus);

            this.grdTemPd.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView3.ColumnWidthChanged += new ColumnEventHandler(gridView3_ColumnWidthChanged);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);
            this.gridView3.GotFocus += new EventHandler(gridView3_GotFocus);
            this.gridView3.CellValueChanged += new CellValueChangedEventHandler(this.gridView3_CellValueChanged);

            this.grcQcWkCtr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcWkCtr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcResource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcResource.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcMOPR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcMOPR.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcOPTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcOPTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcOPCost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcOPCost.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcProd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcBOM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcBOM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcRemark2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcRemark2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcRemark.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            this.txtQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);

            this.txtQcUM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcUM.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQnUM.Validating += new CancelEventHandler(txtQcUM_Validating);

            this.txtAttachFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtAttachFile.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);

        }

        private void frmMFBOM_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strCoorTab = strFMDBName + ".dbo.COOR";

            string strStep = "CISAPPROVE = case ";
            strStep += " when MFResource.CISAPPROVE = 'A' then 'APPROVE' ";
            strStep += " when MFResource.CISAPPROVE = ' ' then '' ";
            strStep += " end ";

            string strSQLExec = "select MFResource.CROWID, " + strStep + " , MFResource.CCODE, MFResource.CNAME, MFResource.CNAME2, MFResource.DCREATE, MFResource.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD ";
            strSQLExec += " , PROD.FCCODE AS QCPROD , PROD.FCNAME AS QNPROD ";
            strSQLExec += " from {0} MFResource ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = MFResource.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = MFResource.CLASTUPDBY ";
            strSQLExec += " left join {2} PROD on PROD.FCSKID = MFResource.CMFGPROD ";
            strSQLExec += " where MFResource.CCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN", strProdTab });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "MFResource", strSQLExec, ref strErrorMsg);

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
            this.gridView3.Columns["nQty"].VisibleIndex = i++;
            this.gridView3.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView3.Columns["cProcure"].VisibleIndex = i++;
            this.gridView3.Columns["cScrap"].VisibleIndex = i++;
            this.gridView3.Columns["cRoundCtrl"].VisibleIndex = i++;
            this.gridView3.Columns["IsMRP"].VisibleIndex = i++;
            this.gridView3.Columns["IsCost"].VisibleIndex = i++;
            this.gridView3.Columns["IsOverHead"].VisibleIndex = i++;
            this.gridView3.Columns["IsIssue"].VisibleIndex = i++;
            this.gridView3.Columns["cQcBOM"].VisibleIndex = i++;
            //this.gridView3.Columns["cSubSti"].VisibleIndex = i++;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView3.Columns["cPdType"].Caption = "T";
            this.gridView3.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView3.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView3.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView3.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView3.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView3.Columns["cScrap"].Caption = "Scrap";
            this.gridView3.Columns["IsMRP"].Caption = "MRP";
            this.gridView3.Columns["IsCost"].Caption = "Cost";
            this.gridView3.Columns["IsOverHead"].Caption = "Over Hd.";
            this.gridView3.Columns["IsIssue"].Caption = "Issue";
            this.gridView3.Columns["cProcure"].Caption = "Procure";
            this.gridView3.Columns["cRoundCtrl"].Caption = "Round";
            this.gridView3.Columns["cSubSti"].Caption = "S";
            this.gridView3.Columns["cQcBOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BOM", "BOM" });

            this.gridView3.Columns["nRecNo"].Width = 50;
            this.gridView3.Columns["cOPSeq"].Width = 80;
            this.gridView3.Columns["cPdType"].Width = 50;
            this.gridView3.Columns["cQcProd"].Width = 130;
            this.gridView3.Columns["cRemark1"].Width = 80;
            this.gridView3.Columns["nQty"].Width = 80;
            this.gridView3.Columns["cQnUOM"].Width = 80;
            this.gridView3.Columns["cScrap"].Width = 80;
            this.gridView3.Columns["IsMRP"].Width = 50;
            this.gridView3.Columns["IsCost"].Width = 50;
            this.gridView3.Columns["IsOverHead"].Width = 60;
            this.gridView3.Columns["IsIssue"].Width = 50;
            this.gridView3.Columns["cProcure"].Width = 60;
            this.gridView3.Columns["cRoundCtrl"].Width = 60;
            this.gridView3.Columns["cQcBOM"].Width = 130;
            //this.gridView3.Columns["cSubSti"].Width = 5;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW3_CQCPROD";
            this.grcQcBOM.Buttons[0].Tag = "GRDVIEW3_CQCBOM";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW3_CPDTYPE";
            this.grcRemark2.Buttons[0].Tag = "GRDVIEW3_CREMARK1";

            this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");
            this.grcRemark.MaxLength = 150;
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

            this.gridView3.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView3.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView3.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            this.gridView3.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView3.Columns["cProcure"].ColumnEdit = this.grcProcure;
            this.gridView3.Columns["cRoundCtrl"].ColumnEdit = this.grcRoundCtrl;
            //
            this.gridView3.Columns["cScrap"].ColumnEdit = this.grcScrap;
            this.gridView3.Columns["cQcBOM"].ColumnEdit = this.grcQcBOM;

            this.gridView3.Columns["nQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

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
            this.gridView2.Columns["cPopGetCost"].VisibleIndex = i++;
            this.gridView2.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView2.Columns["cQcRemark1"].VisibleIndex = i++;
            this.gridView2.Columns["cQcMOPR"].VisibleIndex = i++;
            this.gridView2.Columns["cQnMOPR"].VisibleIndex = i++;
            this.gridView2.Columns["cQcWkCtrH"].VisibleIndex = i++;
            this.gridView2.Columns["cQnWkCtrH"].VisibleIndex = i++;
            this.gridView2.Columns["cQcResource"].VisibleIndex = i++;
            this.gridView2.Columns["nCapFactor1"].VisibleIndex = i++;
            this.gridView2.Columns["nCapPress"].VisibleIndex = i++;
            //this.gridView2.Columns["nScrapQty1"].VisibleIndex = i++;
            //this.gridView2.Columns["nLossQty1"].VisibleIndex = i++;
            //this.gridView2.Columns["cQcWHouse2"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView2.Columns["cNextOP"].Caption = "Next OP.";
            this.gridView2.Columns["cPopGetTime"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "เวลา", "Time" });
            this.gridView2.Columns["cPopGetCost"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ต้นทุน", "Cost" });
            //this.gridView2.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            //this.gridView2.Columns["cQcRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ QC", "QC. Remark" });
            this.gridView2.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "OPR. Detail" });
            this.gridView2.Columns["cQcRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ QC", "QC. Detail" });
            this.gridView2.Columns["cQcMOPR"].Caption = "OPR.";
            this.gridView2.Columns["cQnMOPR"].Caption = "OPR. Name";
            this.gridView2.Columns["cQcWkCtrH"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัส W/C", "W/C Code" });
            this.gridView2.Columns["cQnWkCtrH"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อ W/C", "W/C Name" });
            this.gridView2.Columns["cQcResource"].Caption = "Tool";
            this.gridView2.Columns["nCapFactor1"].Caption = "Cap Cons.";
            this.gridView2.Columns["nCapPress"].Caption = "Press";
            //this.gridView2.Columns["nScrapQty1"].Caption = "% Scrap";
            //this.gridView2.Columns["nLossQty1"].Caption = "% Loss";
            //this.gridView2.Columns["cQcWHouse2"].Caption = "คลัง Scrap";

            this.gridView2.Columns["nRecNo"].Width = 50;
            this.gridView2.Columns["cOPSeq"].Width = 80;
            this.gridView2.Columns["cNextOP"].Width = 80;
            this.gridView2.Columns["cRemark1"].Width = 60;
            this.gridView2.Columns["cQcRemark1"].Width = 100;
            this.gridView2.Columns["cQcMOPR"].Width = 100;
            this.gridView2.Columns["cQnMOPR"].Width = 120;
            this.gridView2.Columns["cQcWkCtrH"].Width = 100;
            this.gridView2.Columns["cQnWkCtrH"].Width = 120;
            this.gridView2.Columns["cPopGetTime"].Width = 50;
            this.gridView2.Columns["cPopGetCost"].Width = 50;
            this.gridView2.Columns["cQcResource"].Width = 120;
            this.gridView2.Columns["nCapFactor1"].Width = 50;
            this.gridView2.Columns["nCapPress"].Width = 50;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.grcQcMOPR.Buttons[0].Tag = "GRDVIEW2_CQCMOPR";
            this.grcQcWkCtr.Buttons[0].Tag = "GRDVIEW2_CQCWKCTRH";
            this.grcOPTime.Buttons[0].Tag = "GRDVIEW2_CPOPGETTIME";
            this.grcOPCost.Buttons[0].Tag = "GRDVIEW2_CPOPGETCOST";
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

            this.gridView2.Columns["cQnMOPR"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cQnMOPR"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnMOPR"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cOPSeq"].ColumnEdit = this.grcOPSeq;
            this.gridView2.Columns["cNextOP"].ColumnEdit = this.grcNextOPSeq;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView2.Columns["cQcRemark1"].ColumnEdit = this.grcQcRemark;
            this.gridView2.Columns["cQcMOPR"].ColumnEdit = this.grcQcMOPR;
            this.gridView2.Columns["cQcWkCtrH"].ColumnEdit = this.grcQcWkCtr;
            this.gridView2.Columns["cPopGetTime"].ColumnEdit = this.grcOPTime;
            this.gridView2.Columns["cPopGetCost"].ColumnEdit = this.grcOPCost;
            this.gridView2.Columns["cQcResource"].ColumnEdit = this.grcQcResource;
            this.gridView2.Columns["nCapFactor1"].ColumnEdit = this.grcCapFactor1;
            this.gridView2.Columns["nCapPress"].ColumnEdit = this.grcCapPress1;
            
            this.pmCalcColWidth();

        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView2.Columns["nRecNo"].Width
                                    + this.gridView2.Columns["cOPSeq"].Width
                                    + this.gridView2.Columns["cNextOP"].Width
                                    + this.gridView2.Columns["cPopGetTime"].Width
                                    + this.gridView2.Columns["cPopGetCost"].Width
                                    + this.gridView2.Columns["cQcMOPR"].Width
                                    + this.gridView2.Columns["cQnMOPR"].Width
                                    + this.gridView2.Columns["cQcWkCtrH"].Width
                                    + this.gridView2.Columns["cQcResource"].Width
                                    + this.gridView2.Columns["cQcRemark1"].Width
                                    + this.gridView2.Columns["cQnWkCtrH"].Width;

            int intNewWidth = this.Width - intColWidth - 50;
            this.gridView2.Columns["cRemark1"].Width = (intNewWidth > 70 ? intNewWidth : 70);
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
                                    +this.gridView3.Columns["IsMRP"].Width
                                    +this.gridView3.Columns["IsCost"].Width
                                    +this.gridView3.Columns["IsOverHead"].Width
                                    +this.gridView3.Columns["IsIssue"].Width
                                    +this.gridView3.Columns["cProcure"].Width
                                    + this.gridView3.Columns["cRoundCtrl"].Width
                                    + this.gridView3.Columns["cQcBOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView3.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFBOMInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QMFBOMInfo.Field.IsApprove].Caption = "#APV.";
            this.gridView1.Columns[QMFBOMInfo.Field.Code].Caption = this.mstrFormMenuName + " Code";
            this.gridView1.Columns[QMFBOMInfo.Field.Name].Caption = this.mstrFormMenuName + " Name";
            this.gridView1.Columns[QMFBOMInfo.Field.Name2].Caption = this.mstrFormMenuName + " Name 2";
            this.gridView1.Columns[QMFBOMInfo.Field.Name2].Caption = this.mstrFormMenuName + " Name 2";
            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "PROD. CODE" });

            this.gridView1.Columns[QMFBOMInfo.Field.IsApprove].Width = 10;
            //this.gridView1.Columns[QMFBOMInfo.Field.IsApprove].BestFit();
            this.gridView1.Columns[QMFBOMInfo.Field.Code].Width = 40;
            this.gridView1.Columns[QMFBOMInfo.Field.Name2].Width = 40;
            this.gridView1.Columns["QCPROD"].Width = 120;

            int i = 0;
            this.gridView1.Columns[QMFBOMInfo.Field.IsApprove].VisibleIndex = i++;
            this.gridView1.Columns[QMFBOMInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QMFBOMInfo.Field.Name].VisibleIndex = i++;
            this.gridView1.Columns[QMFBOMInfo.Field.Name2].VisibleIndex = i++;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;

            this.pmSetSortKey(QMFBOMInfo.Field.Code, true);
        
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

            this.barMainEdit.Items[WsToolBar.Print.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Print.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);
        }

        private void pmFilterForm()
        {
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
                case "PLANT":
                    if (this.pofrmGetPlant == null)
                    {
                        this.pofrmGetPlant = new frmEMPlant(FormActiveMode.PopUp);
                        this.pofrmGetPlant.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPlant.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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

                case "BOM":
                    if (this.pofrmGetBOM == null)
                    {
                        this.pofrmGetBOM = new DialogForms.dlgGetBOM();
                        this.pofrmGetBOM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBOM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                
                case "UOMCONVERT":
                    using (Common.dlgUOMConvert dlg = new Common.dlgUOMConvert())
                    {
                        dlg.InitForm(this.txtQnStdUM.Text.TrimEnd(), this.txtQnUM.Text.ToString());
                        dlg.UOMQty = this.mdecUMQty;
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            this.mdecUMQty = dlg.UOMQty;
                            this.pmSetUMQtyMsg();
                        }
                    }
                    break;
                case "VIEWATTACH":

                    strErrorMsg = "";
                    BizRule.ViewFile(this.txtAttachFile.Text.Trim(), ref strErrorMsg);
                    if (strErrorMsg != string.Empty)
                    {
                        MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;
                case "VIEWATTACH2":

                    dtrGetVal = this.mActiveGrid.GetDataRow(this.mActiveGrid.FocusedRowHandle);

                    if (dtrGetVal != null)
                    {
                        strErrorMsg = "";
                        BizRule.ViewFile(dtrGetVal["cAttachFile"].ToString(), ref strErrorMsg);
                        if (strErrorMsg != string.Empty)
                        {
                            MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                    break;
                case "ATTATCH":
                    dlgGetFile.ShowDialog();
                    this.txtAttachFile.Text = dlgGetFile.FileName.TrimEnd();
                    break;
                case "GETOPTIME":
                    using (Common.dlgGetOPTime dlg = new Common.dlgGetOPTime())
                    {

                        dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                        if (dtrGetVal == null)
                        {
                            dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                        }

                        decimal intHour = 0; decimal intMin = 0; decimal intSec = 0;

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_Queue]), out intHour, out intMin, out intSec);
                        dlg.SetTime("QUEUE", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_SetUp]), out intHour, out intMin, out intSec);
                        dlg.SetTime("SETUP", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_Process]), out intHour, out intMin, out intSec);
                        dlg.SetTime("PROCESS", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_Tear]), out intHour, out intMin, out intSec);
                        dlg.SetTime("TEAR", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_Wait]), out intHour, out intMin, out intSec);
                        dlg.SetTime("WAIT", intHour, intMin, intSec);

                        AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(dtrGetVal[QMFBOMInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
                        dlg.SetTime("MOVE", intHour, intMin, intSec);
                        
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {

                            dlg.GetTime("QUEUE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_Queue] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("SETUP", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_SetUp] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("PROCESS", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_Process] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("TEAR", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_Tear] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("WAIT", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_Wait] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            dlg.GetTime("MOVE", ref intHour, ref intMin, ref intSec);
                            dtrGetVal[QMFBOMInfo.Field.LeadTime_Move] = AppUtil.CommonHelper.ConvertHMSToSec(intHour, intMin, intSec);

                            this.gridView2.UpdateCurrentRow();

                        }
                    }
                    break;
                case "GETOPCOST":
                    //MessageBox.Show("Get COST");
                    using (Common.dlgGetBOMOPCost dlg = new Common.dlgGetBOMOPCost())
                    {

                        dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                        if (dtrGetVal == null)
                        {
                            dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                        }

                        dlg.BindData(this.dtsDataEnv, dtrGetVal["cGUID"].ToString());
                        dlg.ShowDialog();
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
                case "TXTQCPLANT":
                case "TXTQNPLANT":
                    this.pmInitPopUpDialog("PLANT");
                    strPrefix = (inTextbox == "TXTQCPLANT" ? QEMPlantInfo.Field.Code : QEMPlantInfo.Field.Name);
                    this.pofrmGetPlant.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetPlant.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
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
                case "TXTQCUM":
                    this.pmInitPopUpDialog("UM");
                    strPrefix = (inTextbox == "TXTQCUM" ? "FCCODE" : "FCNAME");
                    this.pofrmGetUM.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetUM.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTATTACHFILE":

                    switch (inTag.ToUpper())
                    { 
                        case "DEL":
                            this.txtAttachFile.Text = "";
                            break;
                        case "VIEW":
                            this.pmInitPopUpDialog("VIEWATTACH");
                            break;
                        default:
                            this.pmInitPopUpDialog("ATTATCH");
                            break;
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrGetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "TXTQCPLANT":
                case "TXTQNPLANT":

                    dtrGetVal = this.pofrmGetPlant.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        this.txtQcPlant.Tag = dtrGetVal[QEMPlantInfo.Field.RowID].ToString();
                        this.txtQcPlant.Text = dtrGetVal[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnPlant.Text = dtrGetVal[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcPlant.Tag = "";
                        this.txtQcPlant.Text = "";
                        this.txtQnPlant.Text = "";
                    }
                    break;

                case "TXTQCPROD":
                case "TXTQNPROD":

                    dtrGetVal = this.pofrmGetProd.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        this.txtQcProd.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcProd.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnProd.Text = dtrGetVal["fcName"].ToString().TrimEnd();

                        if (objSQLHelper.SetPara(new object[] { dtrGetVal["fcUM"].ToString() })
                            && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid , fcCode, fcName from UM where fcSkid = ? ", ref strErrorMsg))
                        {
                            this.txtQcUM.Tag = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            this.txtQcUM.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                            this.txtQnUM.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();

                            this.txtQnStdUM.Tag = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                            this.txtQnStdUM.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                        if (!Convert.IsDBNull(dtrGetVal["fmPicName"]))
                        {
                            if (dtrGetVal["fmPicName"].ToString().Trim() != "...")
                            {
                                this.txtAttachFile.Text = dtrGetVal["fmPicName"].ToString().TrimEnd();
                            }
                        }

                    }
                    else
                    {
                        this.txtQcProd.Tag = "";
                        this.txtQcProd.Text = "";
                        this.txtQnProd.Text = "";
                        this.txtAttachFile.Text = "";

                        this.txtQcUM.Tag = "";
                        this.txtQcUM.Text = "";
                        this.txtQnUM.Text = "";

                        this.txtQnStdUM.Tag = "";
                        this.txtQnStdUM.Text = "";
                    
                    }
                    break;
                case "TXTQCUM":

                    dtrGetVal = this.pofrmGetUM.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        if (this.txtQcUM.Tag.ToString() != dtrGetVal["fcSkid"].ToString()
                                && dtrGetVal["fcSkid"].ToString() != this.txtQnStdUM.Tag.ToString())
                        {
                            this.txtQcUM.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtQcUM.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                            this.txtQnUM.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                            this.pmInitPopUpDialog("UOMCONVERT");
                        }

                        this.txtQcUM.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcUM.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnUM.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcUM.Tag = "";
                        this.txtQcUM.Text = "";
                        this.txtQnUM.Text = "";
                    }
                    this.pmSetUMQtyMsg();
                    break;

                case "GRDVIEW2_CQCWKCTRH":
                case "GRDVIEW2_CQNWKCTRH":
                    dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

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

                        if (dtrGetVal["cWkCtrH"].ToString() != dtrGetWkCtr[MapTable.ShareField.RowID].ToString())
                        {
                            this.pmLoad1CostLine2(QMFWorkCenterInfo.TableName, dtrGetWkCtr[MapTable.ShareField.RowID].ToString(), dtrGetVal["cGUID"].ToString(), "W", this.mstrTemCostW);
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

                    this.gridView2.UpdateCurrentRow();

                    break;

                case "GRDVIEW2_CQCMOPR":
                    dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetOP = this.pofrmGetStdOP.RetrieveValue();
                    if (dtrGetOP != null)
                    {

                        if (dtrGetVal["cMOPR"].ToString() != dtrGetOP[MapTable.ShareField.RowID].ToString())
                        {
                            this.pmLoad1CostLine2(QMFStdOprInfo.TableName, dtrGetOP[MapTable.ShareField.RowID].ToString(), dtrGetVal["cGUID"].ToString(), "O", this.mstrTemCost);
                        }

                        dtrGetVal["cMOPR"] = dtrGetOP[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcMOPR"] = dtrGetOP[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                        dtrGetVal["cQnMOPR"] = dtrGetOP[QEMAcChartInfo.Field.Name].ToString().TrimEnd();

                    }
                    else
                    {
                        dtrGetVal["cMOPR"] = "";
                        dtrGetVal["cQcMOPR"] = "";
                        dtrGetVal["cQnMOPR"] = "";
                    }

                    this.gridView2.UpdateCurrentRow();
                    break;

                case "GRDVIEW2_QCRESOURCE":
                    dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemOP].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetRes = this.pofrmGetResource.RetrieveValue();
                    if (dtrGetRes != null)
                    {

                        if (dtrGetVal["cResource"].ToString() != dtrGetRes[MapTable.ShareField.RowID].ToString())
                        {
                            this.pmLoad1CostLine2(QMFResourceInfo.TableName, dtrGetRes[MapTable.ShareField.RowID].ToString(), dtrGetVal["cGUID"].ToString(), "T", this.mstrTemCostT);
                        }

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

                    dtrGetVal = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

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

                    this.gridView3.UpdateCurrentRow();

                    break;
                case "GRDVIEW3_CQCPROD":
                case "GRDVIEW3_CQNPROD":

                    dtrGetVal = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

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

                        string[] staRemark = new string[] { "", "", "", "", "", "", "", "", "", "" };
                        this.pmLoadPdRemark(dtrGetProd["fcSkid"].ToString(), ref staRemark);
                        dtrGetVal["cRemark1"] = staRemark[0];
                        dtrGetVal["cRemark2"] = staRemark[1];
                        dtrGetVal["cRemark3"] = staRemark[2];
                        dtrGetVal["cRemark4"] = staRemark[3];
                        dtrGetVal["cRemark5"] = staRemark[4];
                        dtrGetVal["cRemark6"] = staRemark[5];
                        dtrGetVal["cRemark7"] = staRemark[6];
                        dtrGetVal["cRemark8"] = staRemark[7];
                        dtrGetVal["cRemark9"] = staRemark[8];
                        dtrGetVal["cRemark10"] = staRemark[9];
                    }
                    else
                    {
                        this.pmClr1TemPd();
                    }

                    this.gridView3.UpdateCurrentRow();

                    break;

                case "GRDVIEW3_CQCBOM":

                    dtrGetVal = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

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

                    this.gridView3.UpdateCurrentRow();

                    break;
                case "LOAD_BOM":
                    DataRow dtrGetBOM2 = this.pofrmGetBOM.RetrieveValue();
                    if (dtrGetBOM2 != null)
                    {
                        this.pmLoadFormData(true, dtrGetBOM2["cRowID"].ToString());
                    }
                    break;
            }
        }

        private void pmLoadPdRemark(string inProd, ref string[] ioRemark)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProdX4", "PRODX4", "select * from ProdX4 where fcProd = ?", ref strErrorMsg))
            {
                DataRow dtrProdX4 = this.dtsDataEnv.Tables["QProdX4"].Rows[0];

                string gcTemStr01 = (Convert.IsDBNull(dtrProdX4["fmMemData"]) ? "" : dtrProdX4["fmMemData"].ToString().TrimEnd());
                string gcTemStr02 = (Convert.IsDBNull(dtrProdX4["fmMemData2"]) ? "" : dtrProdX4["fmMemData2"].ToString().TrimEnd());
                string gcTemStr03 = (Convert.IsDBNull(dtrProdX4["fmMemData3"]) ? "" : dtrProdX4["fmMemData3"].ToString().TrimEnd());
                string gcTemStr04 = (Convert.IsDBNull(dtrProdX4["fmMemData4"]) ? "" : dtrProdX4["fmMemData4"].ToString().TrimEnd());
                string gcTemStr05 = (Convert.IsDBNull(dtrProdX4["fmMemData5"]) ? "" : dtrProdX4["fmMemData5"].ToString().TrimEnd());

                ioRemark[0] = BizRule.GetMemData(gcTemStr01, xdCMRem);
                ioRemark[1] = BizRule.GetMemData(gcTemStr01, xdCMRem2);
                ioRemark[2] = BizRule.GetMemData(gcTemStr02, xdCMRem3);
                ioRemark[3] = BizRule.GetMemData(gcTemStr02, xdCMRem4);
                ioRemark[4] = BizRule.GetMemData(gcTemStr03, xdCMRem5);
                ioRemark[5] = BizRule.GetMemData(gcTemStr03, xdCMRem6);
                ioRemark[6] = BizRule.GetMemData(gcTemStr04, xdCMRem7);
                ioRemark[7] = BizRule.GetMemData(gcTemStr04, xdCMRem8);
                ioRemark[8] = BizRule.GetMemData(gcTemStr05, xdCMRem9);
                ioRemark[9] = BizRule.GetMemData(gcTemStr05, xdCMRem10);

            }

        }

        private void pmSetUMQtyMsg()
        {
            if (this.txtQnUM.Text.ToString().TrimEnd() != string.Empty && this.txtQnStdUM.Text.ToString().TrimEnd() != string.Empty
                && this.txtQnStdUM.Text.ToString().TrimEnd() != this.txtQnUM.Text.ToString().TrimEnd())
                this.lblUMQty.Text = "1 " + this.txtQnUM.Text.ToString().TrimEnd() + " = " + this.mdecUMQty.ToString("##,##0.0000") + " " + this.txtQnStdUM.Text.TrimEnd();
            else
            {
                this.lblUMQty.Text = "";
                this.mdecUMQty = 1;
            }

        }

        private void txtQcPlant_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCPLANT" ? QEMPlantInfo.Field.Code : QEMPlantInfo.Field.Name;

            if (txtPopUp.Text == "")
            {
                this.txtQcPlant.Tag = "";
                this.txtQcPlant.Text = "";
                this.txtQnPlant.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PLANT");
                e.Cancel = !this.pofrmGetPlant.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPlant.PopUpResult)
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
                this.txtQnProd.Text = "";
                this.txtAttachFile.Text = "";
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

        private void txtQcUM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCUM" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcUM.Tag = "";
                this.txtQcUM.Text = "";
                this.txtQnUM.Text = "";
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
                    case WsToolBar.Search:
                        this.pmSearchData();
                        break;
                    case WsToolBar.Undo:
                        if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                            this.pmGotoBrowPage();
                        break;
                    case WsToolBar.Save:
                        this.pmSaveData();
                        break;
                    case WsToolBar.Refresh:
                        this.pmRefreshBrowView();
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

                if (System.IO.Directory.Exists(Application.StartupPath + "\\RPT\\FORM_BOM\\"))
                {
                    strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_BOM\\");
                    dlg.LoadRPT(Application.StartupPath + "\\RPT\\FORM_BOM\\");
                }

                dlg.cmbPOption1.Visible = true;
                dlg.cmbPOption1.Properties.Items.Clear();
                dlg.cmbPOption1.Properties.Items.AddRange(new object[] { 
                                                                              UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
                                                                            , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });
                dlg.cmbPOption1.SelectedIndex = 0;

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    this.mintPrint_RMWHCost = dlg.cmbPOption1.SelectedIndex;
                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);
                }
            }
        }

        private int mintPrint_RMWHCost = 0;
        Report.LocalDataSet.FORM2PRINT dtsPrintPreview = new Report.LocalDataSet.FORM2PRINT();

        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";
            string strSQLStrOrderI_OP = "select * from " + MapTable.Table.MFBOMIT_OP + " where cBOMHD = ? order by cSeq";
            string strSQLStrOrderI_Pd = "select * from " + MapTable.Table.MFBOMIT_PD + " where cBOMHD = ? order by cSeq";

            dtsPrintPreview.PFORM_BOM.Rows.Clear();
            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrRefTable + " where cCorp = ? and cCode between ? and ? order by cCode";
            
            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, inBegCode, inEndCode });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBOMH in this.dtsDataEnv.Tables["QBOMH"].Rows)
                {

                    string strQcMfgProd = "";
                    string strQnMfgProd = "";
                    string strQnMfgProd2 = "";
                    string strQcMfgUM = "";
                    string strQnMfgUM = "";
                    string strQcStdMfgUM = "";
                    string strQnStdMfgUM = "";
                    string strApproveBy = "";
                    string strApproveDate = "";
                    string strLastUpdBy = "";
                    string strLastUpd = "";

                    pobjSQLUtil.SetPara(new object[1] { dtrBOMH[QMFBOMInfo.Field.MfgProdID].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName,fcName2,fcUM from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                        strQnMfgProd2 = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName2"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid, fcCode,fcName,fcName2 from UM where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcStdMfgUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnStdMfgUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                        }
                    
                    }

                    pobjSQLUtil.SetPara(new object[1] { dtrBOMH[QMFBOMInfo.Field.MfgUMID].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid, fcCode,fcName,fcName2 from UM where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal decMfgQty = Convert.ToDecimal(dtrBOMH[QMFBOMInfo.Field.MfgQty]);

                    //Load ชื่อ Login ที่อนุมัติ
                    pobjSQLUtil2.SetPara(new object[] { dtrBOMH[QMFBOMInfo.Field.ApproveBy].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        strApproveBy = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }

                    //Load ชื่อ Login ที่แก้ไขล่าสุด
                    pobjSQLUtil2.SetPara(new object[] { dtrBOMH[QBGTranInfo.Field.LastUpdateBy].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        strLastUpdBy = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }
                    if (!Convert.IsDBNull(dtrBOMH[QBGTranInfo.Field.LastUpdate]))
                    {
                        DateTime dttLastUpd = Convert.ToDateTime(dtrBOMH[QBGTranInfo.Field.LastUpdate]);
                        strLastUpd = BudgetHelper.GetShortThaiDate(dttLastUpd) + " " + dttLastUpd.ToString("HH:mm:ss");
                    }

                    if (!Convert.IsDBNull(dtrBOMH[QBGTranInfo.Field.ApproveDate]))
                    {
                        DateTime dttLastUpd = Convert.ToDateTime(dtrBOMH[QBGTranInfo.Field.ApproveDate]);
                        strApproveDate = BudgetHelper.GetShortThaiDate(dttLastUpd) + " " + dttLastUpd.ToString("HH:mm:ss");
                    }

                    string strRemark1 = dtrBOMH[QMFBOMInfo.Field.Remark1].ToString();
                    string strRemark2 = dtrBOMH[QMFBOMInfo.Field.Remark2].ToString();
                    string strRemark3 = dtrBOMH[QMFBOMInfo.Field.Remark3].ToString();
                    string strRemark4 = dtrBOMH[QMFBOMInfo.Field.Remark4].ToString();
                    string strRemark5 = dtrBOMH[QMFBOMInfo.Field.Remark5].ToString();
                    string strRemark6 = dtrBOMH[QMFBOMInfo.Field.Remark6].ToString();
                    string strRemark7 = dtrBOMH[QMFBOMInfo.Field.Remark7].ToString();
                    string strRemark8 = dtrBOMH[QMFBOMInfo.Field.Remark8].ToString();
                    string strRemark9 = dtrBOMH[QMFBOMInfo.Field.Remark9].ToString();
                    string strRemark10 = dtrBOMH[QMFBOMInfo.Field.Remark10].ToString();

                    pobjSQLUtil2.SetPara(new object[] { dtrBOMH[QMFBOMInfo.Field.RowID].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI_Pd, ref strErrorMsg))
                    {
                        #region "Print RM Item"
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            decimal decStdCost = 0;
                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());
                            string strQcRM_MfgBOM = "";
                            string strQnRM_MfgBOM = "";

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName, fnStdCost from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                                decStdCost = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["fnStdCost"]);
                            }

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            DataRow dtrPrnData = dtsPrintPreview.PFORM_BOM.NewRow();

                            #region "BOM"

                            dtrPrnData["BOM_CODE"] = dtrBOMH[QMFBOMInfo.Field.Code].ToString();
                            dtrPrnData["BOM_NAME"] = dtrBOMH[QMFBOMInfo.Field.Name].ToString();
                            dtrPrnData["BOM_NAME2"] = dtrBOMH[QMFBOMInfo.Field.Name].ToString();
                            dtrPrnData["BOM_APPROVE_BY"] = strApproveBy;
                            dtrPrnData["BOM_APPROVE_BY_DATE"] = strApproveDate;
                            dtrPrnData["BOM_LAST_UPDATE"] = strLastUpd;
                            dtrPrnData["BOM_LAST_UPDATE_BY"] = strLastUpdBy;
                            dtrPrnData["BOM_OUTPUT_PROD_CODE"] = strQcMfgProd;
                            dtrPrnData["BOM_OUTPUT_PROD_NAME"] = strQnMfgProd;
                            dtrPrnData["BOM_OUTPUT_PROD_NAME2"] = strQnMfgProd2;
                            dtrPrnData["BOM_OUTPUT_QTY"] = Convert.ToDecimal(dtrBOMH[QMFBOMInfo.Field.MfgQty]);
                            dtrPrnData["BOM_MFG_UMCODE"] = strQcMfgUM;
                            dtrPrnData["BOM_MFG_UMNAME"] = strQnMfgUM;
                            dtrPrnData["BOM_STD_UMCODE"] = strQcStdMfgUM;
                            dtrPrnData["BOM_STD_UMNAME"] = strQnStdMfgUM;
                            dtrPrnData["BOM_SCRAP"] = dtrBOMH[QMFBOMInfo.Field.Scrap].ToString();

                            dtrPrnData["BOM_REMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());

                            #endregion

                            #region "RM"

                            int intLevel = 0;
                            dtrPrnData["BOM_ITEM_TYPE"] = "2_RM";
                            dtrPrnData["RM_PROD_LEVEL"] = intLevel;
                            dtrPrnData["RM_PROD_CODE"] = strQcProd;
                            dtrPrnData["RM_PROD_NAME"] = strQnProd;
                            dtrPrnData["RM_PROD_SNAME"] = strQsProd;
                            dtrPrnData["RM_REMARK1"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK2"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK3"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK4"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK5"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK6"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK7"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK8"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK9"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_REMARK10"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["RM_QTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                            //dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                            dtrPrnData["RM_UMCODE"] = strQcUM;
                            dtrPrnData["RM_UMNAME"] = strQnUM;
                            dtrPrnData["RM_PROCURE"] = dtrOrderI["cProcure"].ToString();
                            dtrPrnData["RM_SCRAP"] = dtrOrderI["cScrap"].ToString();

                            switch (this.mintPrint_RMWHCost)
                            { 
                                case 0:
                                    dtrPrnData["RM_COST"] = decStdCost;
                                    break;
                                default:
                                    dtrPrnData["RM_COST"] = this.pmGetBuyPrice(dtrOrderI["cProd"].ToString(), this.mintPrint_RMWHCost);
                                    break;
                            }

                            dtsPrintPreview.PFORM_BOM.Rows.Add(dtrPrnData);

                            if (dtrOrderI["cProcure"].ToString() == "M")
                            {

                                pobjSQLUtil2.SetPara(new object[1] { dtrOrderI["cMfgBOMHD"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBOM", "PROD", "select cCode, cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                                {
                                    dtrPrnData["RM_MFGBOM_CODE"] = this.dtsDataEnv.Tables["QMfgBOM"].Rows[0]["cCode"].ToString().TrimEnd();
                                    dtrPrnData["RM_MFGBOM_NAME"] = this.dtsDataEnv.Tables["QMfgBOM"].Rows[0]["cName"].ToString().TrimEnd();
                                }

                                dtrPrnData["RM_MFGBOM_CODE"] = dtrBOMH[QMFBOMInfo.Field.Code].ToString();
                                dtrPrnData["RM_MFGBOM_NAME"] = dtrBOMH[QMFBOMInfo.Field.Name].ToString();

                                this.pmPrintBOM_Item(dtrPrnData, intLevel, dtrOrderI["cMfgBOMHD"].ToString(), Convert.ToDecimal(dtrOrderI["nQty"]));
                            }

                            #endregion

                        }
                        #endregion
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrBOMH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI_OP, ref strErrorMsg))
                    {
                        #region "Print OP Item"
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            DataRow dtrPrnData = dtsPrintPreview.PFORM_BOM.NewRow();

                            #region "BOM"

                            dtrPrnData["BOM_CODE"] = dtrBOMH[QMFBOMInfo.Field.Code].ToString();
                            dtrPrnData["BOM_NAME"] = dtrBOMH[QMFBOMInfo.Field.Name].ToString();
                            dtrPrnData["BOM_NAME2"] = dtrBOMH[QMFBOMInfo.Field.Name].ToString();
                            dtrPrnData["BOM_APPROVE_BY"] = strApproveBy;
                            dtrPrnData["BOM_APPROVE_BY_DATE"] = strApproveDate;
                            dtrPrnData["BOM_LAST_UPDATE"] = strLastUpd;
                            dtrPrnData["BOM_LAST_UPDATE_BY"] = strLastUpdBy;
                            dtrPrnData["BOM_OUTPUT_PROD_CODE"] = strQcMfgProd;
                            dtrPrnData["BOM_OUTPUT_PROD_NAME"] = strQnMfgProd;
                            dtrPrnData["BOM_OUTPUT_PROD_NAME2"] = strQnMfgProd2;
                            dtrPrnData["BOM_OUTPUT_QTY"] = Convert.ToDecimal(dtrBOMH[QMFBOMInfo.Field.MfgQty]);
                            dtrPrnData["BOM_MFG_UMCODE"] = strQcMfgUM;
                            dtrPrnData["BOM_MFG_UMNAME"] = strQnMfgUM;
                            dtrPrnData["BOM_STD_UMCODE"] = strQcStdMfgUM;
                            dtrPrnData["BOM_STD_UMNAME"] = strQnStdMfgUM;
                            dtrPrnData["BOM_SCRAP"] = dtrBOMH[QMFBOMInfo.Field.Scrap].ToString();

                            dtrPrnData["BOM_REMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["BOM_REMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());

                            #endregion

                            #region "OP"

                            dtrPrnData["BOM_ITEM_TYPE"] = "1_OP";
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

                            //Get OP Cost from COSTLINE Table
                            QCostLineInfo oCostLine = new QCostLineInfo();

                            //Load Cost by OP
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "O");

                            dtrPrnData["OP_COST_O_FIX"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_O_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            dtrPrnData["OP_COST_O_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            dtrPrnData["OP_COST_O_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            dtrPrnData["OP_COST_O_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            dtrPrnData["OP_COST_O_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            dtrPrnData["OP_COST_O_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_O_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_O_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_O_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_O_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            //ค่าตามที่คีย์ใน BOM
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "O", true);
                            dtrPrnData["OP_COST_O_FIX1"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_O_VM11"] = oCostLine.Cost_Var_ManHour1;
                            dtrPrnData["OP_COST_O_VM21"] = oCostLine.Cost_Var_ManHour2;
                            dtrPrnData["OP_COST_O_VM31"] = oCostLine.Cost_Var_ManHour3;
                            dtrPrnData["OP_COST_O_VM41"] = oCostLine.Cost_Var_ManHour4;
                            dtrPrnData["OP_COST_O_VM51"] = oCostLine.Cost_Var_ManHour5;
                            dtrPrnData["OP_COST_O_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_O_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_O_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_O_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_O_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            dtrPrnData["OP_COST_O_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            dtrPrnData["OP_COST_O_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            dtrPrnData["OP_COST_O_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            dtrPrnData["OP_COST_O_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            dtrPrnData["OP_COST_O_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;

                            //Load Cost by WorkCenter
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "W");
                            dtrPrnData["OP_COST_W_FIX"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_W_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            dtrPrnData["OP_COST_W_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            dtrPrnData["OP_COST_W_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            dtrPrnData["OP_COST_W_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            dtrPrnData["OP_COST_W_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            dtrPrnData["OP_COST_W_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_W_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_W_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_W_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_W_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            //ค่าตามที่คีย์ใน BOM
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "W", true);
                            dtrPrnData["OP_COST_W_FIX1"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_W_VM11"] = oCostLine.Cost_Var_ManHour1;
                            dtrPrnData["OP_COST_W_VM21"] = oCostLine.Cost_Var_ManHour2;
                            dtrPrnData["OP_COST_W_VM31"] = oCostLine.Cost_Var_ManHour3;
                            dtrPrnData["OP_COST_W_VM41"] = oCostLine.Cost_Var_ManHour4;
                            dtrPrnData["OP_COST_W_VM51"] = oCostLine.Cost_Var_ManHour5;
                            dtrPrnData["OP_COST_W_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_W_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_W_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_W_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_W_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            dtrPrnData["OP_COST_W_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            dtrPrnData["OP_COST_W_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            dtrPrnData["OP_COST_W_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            dtrPrnData["OP_COST_W_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            dtrPrnData["OP_COST_W_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;


                            //Load Cost by Tool (Resource)
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "T");
                            dtrPrnData["OP_COST_T_FIX"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_T_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
                            dtrPrnData["OP_COST_T_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
                            dtrPrnData["OP_COST_T_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
                            dtrPrnData["OP_COST_T_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
                            dtrPrnData["OP_COST_T_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
                            dtrPrnData["OP_COST_T_VP1"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_T_VP2"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_T_VP3"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_T_VP4"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_T_VP5"] = oCostLine.Cost_Var_ByOutput5;

                            //ค่าตามที่คีย์ใน BOM
                            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrOrderI["cRowID"].ToString(), "T", true);
                            dtrPrnData["OP_COST_T_FIX1"] = oCostLine.Cost_Fix;
                            dtrPrnData["OP_COST_T_VM11"] = oCostLine.Cost_Var_ManHour1;
                            dtrPrnData["OP_COST_T_VM21"] = oCostLine.Cost_Var_ManHour2;
                            dtrPrnData["OP_COST_T_VM31"] = oCostLine.Cost_Var_ManHour3;
                            dtrPrnData["OP_COST_T_VM41"] = oCostLine.Cost_Var_ManHour4;
                            dtrPrnData["OP_COST_T_VM51"] = oCostLine.Cost_Var_ManHour5;
                            dtrPrnData["OP_COST_T_VP11"] = oCostLine.Cost_Var_ByOutput1;
                            dtrPrnData["OP_COST_T_VP21"] = oCostLine.Cost_Var_ByOutput2;
                            dtrPrnData["OP_COST_T_VP31"] = oCostLine.Cost_Var_ByOutput3;
                            dtrPrnData["OP_COST_T_VP41"] = oCostLine.Cost_Var_ByOutput4;
                            dtrPrnData["OP_COST_T_VP51"] = oCostLine.Cost_Var_ByOutput5;

                            dtrPrnData["OP_COST_T_VM11_UNIT"] = oCostLine.Cost_Var_ManHour1_Unit;
                            dtrPrnData["OP_COST_T_VM21_UNIT"] = oCostLine.Cost_Var_ManHour2_Unit;
                            dtrPrnData["OP_COST_T_VM31_UNIT"] = oCostLine.Cost_Var_ManHour3_Unit;
                            dtrPrnData["OP_COST_T_VM41_UNIT"] = oCostLine.Cost_Var_ManHour4_Unit;
                            dtrPrnData["OP_COST_T_VM51_UNIT"] = oCostLine.Cost_Var_ManHour5_Unit;

                            dtsPrintPreview.PFORM_BOM.Rows.Add(dtrPrnData);
                            #endregion

                        }
                        #endregion
                    }

                }
                this.pmPreviewReport(dtsPrintPreview, inRPTFileName, false);
                //dlg.WaitClear();
            }
        }

        private void pmPrintBOM_Item(DataRow inSource, int inLevel, string inBOMHD, decimal inMfgQty)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStrOrderI_Pd = "select * from " + MapTable.Table.MFBOMIT_PD + " where cBOMHD = ? order by cSeq";

            pobjSQLUtil2.SetPara(new object[] { inBOMHD });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI_Pd, ref strErrorMsg))
            {
                #region "Print RM Item"
                foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                {

                    string strQcProd = "";
                    string strQnProd = "";
                    string strQsProd = "";
                    string strQcUM = "";
                    string strQnUM = "";
                    decimal decStdCost = 0;
                    string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                    decimal decFactorQty = 0;
                    DataRow dtrBomH = null;
                    pobjSQLUtil2.SetPara(new object[] { inBOMHD });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMHD", "MFBOMHD", "select * from " + QMFBOMInfo.TableName + " where cRowID = ? ", ref strErrorMsg))
                    {
                        dtrBomH = this.dtsDataEnv.Tables["QBOMHD"].Rows[0];
                        decFactorQty = inMfgQty / Convert.ToDecimal(dtrBomH[QMFBOMInfo.Field.MfgQty]);
                    }

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName, fnStdCost from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                        strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                        decStdCost = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["fnStdCost"]);
                    }

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUM"].ToString().TrimEnd() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    DataRow dtrPrnData = dtsPrintPreview.PFORM_BOM.NewRow();

                    DataSetHelper.CopyDataRow(inSource, ref dtrPrnData);

                    #region "RM"

                    int intLevel = inLevel + 1;
                    dtrPrnData["BOM_ITEM_TYPE"] = "2_RM";
                    dtrPrnData["RM_PROD_LEVEL"] = intLevel;
                    dtrPrnData["RM_PROD_CODE"] = StringHelper.Space(intLevel*2) + strQcProd;
                    dtrPrnData["RM_PROD_NAME"] = strQnProd;
                    dtrPrnData["RM_PROD_SNAME"] = strQsProd;
                    dtrPrnData["RM_REMARK1"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK2"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK3"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK4"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK5"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK6"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK7"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK8"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK9"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    dtrPrnData["RM_REMARK10"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());

                    //inMfgQty
                    decimal decMfgQty = decFactorQty * Convert.ToDecimal(dtrOrderI["nQty"]);
                    dtrPrnData["RM_QTY"] = decMfgQty;
                    //dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                    dtrPrnData["RM_UMCODE"] = strQcUM;
                    dtrPrnData["RM_UMNAME"] = strQnUM;
                    dtrPrnData["RM_PROCURE"] = dtrOrderI["cProcure"].ToString();
                    dtrPrnData["RM_SCRAP"] = dtrOrderI["cScrap"].ToString();
                    
                    //dtrPrnData["RM_MFGBOM_CODE"] = "";
                    //dtrPrnData["RM_MFGBOM_NAME"] = "";

                    dtrPrnData["RM_MFGBOM_CODE"] = inSource["RM_MFGBOM_CODE"].ToString();
                    dtrPrnData["RM_MFGBOM_NAME"] = inSource["RM_MFGBOM_NAME"].ToString();

                    switch (this.mintPrint_RMWHCost)
                    {
                        case 0:
                            dtrPrnData["RM_COST"] = decStdCost;
                            break;
                        default:
                            dtrPrnData["RM_COST"] = this.pmGetBuyPrice(dtrOrderI["cProd"].ToString(), this.mintPrint_RMWHCost);
                            break;
                    }

                    dtsPrintPreview.PFORM_BOM.Rows.Add(dtrPrnData);

                    if (dtrOrderI["cProcure"].ToString() == "M")
                    {
                        pobjSQLUtil2.SetPara(new object[1] { dtrOrderI["cMfgBOMHD"].ToString() });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBOM", "PROD", "select cCode, cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            dtrPrnData["RM_MFGBOM_CODE"] = this.dtsDataEnv.Tables["QMfgBOM"].Rows[0]["cCode"].ToString().TrimEnd();
                            dtrPrnData["RM_MFGBOM_NAME"] = this.dtsDataEnv.Tables["QMfgBOM"].Rows[0]["cName"].ToString().TrimEnd();
                        }

                        this.pmPrintBOM_Item(dtrPrnData, intLevel, dtrOrderI["cMfgBOMHD"].ToString(), decMfgQty);
                    }

                    #endregion

                }
                #endregion
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
            string strDeleteDesc = "(" + dtrBrow[QMFBOMInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QMFBOMInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFBOMInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFBOMInfo.Field.Code].ToString(), dtrBrow[QMFBOMInfo.Field.Name].ToString(), ref strErrorMsg))
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

            string strMsg1 = UIBase.GetAppUIText(new string[] { "ไม่สามารถลบได้เนื่องจาก", "Can not delete because " });
            string strMsg2 = "";
            bool bllHasUsed = false;
            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select cRefNo from " + QMFWOrderHDInfo.TableName + " where CBOMHD = ? ", ref strErrorMsg))
            {
                strMsg2 = this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cRefNo"].ToString().TrimEnd();
                ioErrorMsg += "\r\n" + UIBase.GetAppUIText(new string[] { "มีเอกสาร MO เลขที่ " + strMsg2 + " อ้างถึงแล้ว", "has refer by #MO : " + strMsg2 + " " });
                bllHasUsed = true;
            }

            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select MFBOMHD.CCODE from MFBOMIT_PD left join MFBOMHD on MFBOMHD.CROWID = MFBOMIT_PD.CBOMHD where MFBOMIT_PD.CMFGBOMHD = ? ", ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"]))
                {
                    strMsg2 = this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"].ToString().TrimEnd();
                    ioErrorMsg += "\r\n" + UIBase.GetAppUIText(new string[] { "มีเอกสาร BOM รหัส " + strMsg2 + " อ้างถึงแล้ว", "has refer by BOM : " + strMsg2 + " " });
                    bllHasUsed = true;
                }
            }
            ioErrorMsg = strMsg1 + ioErrorMsg;
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

                //Delete Cost Line
                pAPara = new object[2] { this.mstrITable, inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from COSTLINE where CREFTAB = ? and CMASTERH in (select CROWID from MFBOMIT_STDOP where CBOMHD = ?)", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cBOMHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable2 + " where cBOMHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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

        private void pmSaveData()
        {
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

                UIBase.WaitClear();
            }
			if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool pmValidBeforeSave(ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strMsg = "";
            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                strMsg = UIBase.GetAppUIText(new string[] {
                    "ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code ต้องการให้เครื่อง Running ให้หรือไม่ ?"
                    ,this.mstrFormMenuName + " Code is not define ! " + "Do you want to Auto Running Code ?"});

                if (MessageBox.Show(strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code", this.mstrFormMenuName + " Code is not define ! " });
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code", this.mstrFormMenuName + " Name is not define ! " });
                this.txtName.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "รหัส " + this.mstrFormMenuName + " ซ้ำ", "Duplicate " + this.mstrFormMenuName + " Code !" });
                this.txtCode.Focus();
                return false;
            }
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "OPR. Nameซ้ำ";
            //    this.txtName.Focus();
            //    return false;
            //}
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
            int intRunCode = 1;
            int inMaxLength = this.txtCode.Properties.MaxLength;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QMFBOMInfo.Field.Code].ToString().Trim();
                try
                {
                    intRunCode = Convert.ToInt32(strLastRunCode) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
            }
            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : inMaxLength);
            this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));

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

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFBOMInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFBOMInfo.Field.PlantID] = this.txtQcPlant.Tag.ToString();
            dtrSaveInfo[QMFBOMInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Name2] = this.txtName2.Text.TrimEnd();

            dtrSaveInfo[QMFBOMInfo.Field.MfgProdID] = this.txtQcProd.Tag.ToString();
            dtrSaveInfo[QMFBOMInfo.Field.MfgQty] = Convert.ToDecimal(this.txtMfgQty.Value);
            dtrSaveInfo[QMFBOMInfo.Field.MfgUMID] = this.txtQcUM.Tag.ToString();
            dtrSaveInfo[QMFBOMInfo.Field.MfgUMQty] = this.mdecUMQty;
            dtrSaveInfo[QMFBOMInfo.Field.IsActive] = (this.chkIsActive.Checked ? "A" : "");

            dtrSaveInfo[QMFBOMInfo.Field.Scrap] = this.txtScrap.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.FileAttach] = this.txtAttachFile.Text.TrimEnd();

            dtrSaveInfo[QMFBOMInfo.Field.Remark1] = this.txtRemark1.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark2] = this.txtRemark2.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark3] = this.txtRemark3.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark4] = this.txtRemark4.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark5] = this.txtRemark5.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark6] = this.txtRemark6.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark7] = this.txtRemark7.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark8] = this.txtRemark8.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark9] = this.txtRemark9.Text.TrimEnd();
            dtrSaveInfo[QMFBOMInfo.Field.Remark10] = this.txtRemark10.Text.TrimEnd();

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
            dtrSaveInfo[QMFBOMInfo.Field.RoundCtrl] = strRound;

            switch (BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()))
            {
                case ApproveStep.Wait:
                    dtrSaveInfo[QMFBOMInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFBOMInfo.Field.ApproveBy] = "";
                    dtrSaveInfo[QMFBOMInfo.Field.ApproveDate] = Convert.DBNull;
                    break;
                case ApproveStep.Approve:
                    dtrSaveInfo[QMFBOMInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QMFBOMInfo.Field.ApproveBy] = App.AppUserID;
                    dtrSaveInfo[QMFBOMInfo.Field.ApproveDate] = this.txtDApprove.DateTime;
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
                this.pmUpdateBomIT_Pd(this.mstrTemPd);

                this.mdbTran.Commit();
                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    if (this.mstrOldCode == this.txtCode.Text && this.mstrOldName == this.txtName.Text)
                    {
                        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                    }
                    else 
                    {
                        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
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

                    this.pmSaveCostLine(this.mstrTemCost, dtrTemPd["cRowID"].ToString(), dtrTemPd["cGUID"].ToString(), "O");
                    this.pmSaveCostLine(this.mstrTemCostW, dtrTemPd["cRowID"].ToString(), dtrTemPd["cGUID"].ToString(), "W");
                    this.pmSaveCostLine(this.mstrTemCostT, dtrTemPd["cRowID"].ToString(), dtrTemPd["cGUID"].ToString(), "T");

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[2] { this.mstrITable, dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from COSTLINE where CREFTAB = ? and CMASTERH = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        //TODO: Delete Cost Line
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
            dtrBudCI["cPlant"] = this.txtQcPlant.Tag.ToString();
            //dtrBudCI["cType"] = "O";
            //dtrBudCI["cBOMHD"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cBOMHD"] = this.mstrEditRowID;
            dtrBudCI["cMOPR"] = inTemPd["cMOPR"].ToString();
            dtrBudCI["cWkCtrH"] = inTemPd["cWkCtrH"].ToString();
            dtrBudCI["cResType"] = SysDef.gc_RESOURCE_TYPE_TOOL;
            dtrBudCI["cResource"] = inTemPd["cResource"].ToString();
            dtrBudCI["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrBudCI["cNextOP"] = inTemPd["cNextOP"].ToString();
            dtrBudCI[QMFBOMInfo.Field.LeadTime_Queue] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_Queue]);
            dtrBudCI[QMFBOMInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_SetUp]);
            dtrBudCI[QMFBOMInfo.Field.LeadTime_Process] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_Process]);
            dtrBudCI[QMFBOMInfo.Field.LeadTime_Tear] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_Tear]);
            dtrBudCI[QMFBOMInfo.Field.LeadTime_Wait] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_Wait]);
            dtrBudCI[QMFBOMInfo.Field.LeadTime_Move] = Convert.ToDecimal(inTemPd[QMFBOMInfo.Field.LeadTime_Move]);
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
            dtrBudCI["nCapFactor1"] = Convert.ToDecimal(inTemPd["nCapFactor1"]);
            dtrBudCI["nCapPress"] = Convert.ToDecimal(inTemPd["nCapPress"]);
            
            dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

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
                if (dtrTemPd["cOPSeq"].ToString().TrimEnd() != string.Empty)
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

                    this.pmReplRecordTemPd(bllIsNewRow, this.mstrTemOP, dtrTemPd, ref dtrBudCI);

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

        private bool pmReplRecordTemPd(bool inState, string inType, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrPdStI = ioRefProd;
            DataRow dtrPdStH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrPdStI["cCorp"] = App.ActiveCorp.RowID;
            dtrPdStI["cPlant"] = this.txtQcPlant.Tag.ToString();
            //dtrPdStI["cBOMHD"] = dtrPdStH["cRowID"].ToString();
            dtrPdStI["cBOMHD"] = this.mstrEditRowID;
            dtrPdStI["cOPSeq"] = inTemPd["cOPSeq"].ToString();
            dtrPdStI["cProd"] = inTemPd["cProd"].ToString();
            dtrPdStI["cMfgBOMHD"] = inTemPd["cMfgBOMHD"].ToString();
            dtrPdStI["cUM"] = inTemPd["cUOM"].ToString();
            dtrPdStI["cProcure"] = inTemPd["cProcure"].ToString();
            dtrPdStI["cRoundCtrl"] = inTemPd["cRoundCtrl"].ToString();
            dtrPdStI["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrPdStI["cRemark2"] = inTemPd["cRemark2"].ToString().TrimEnd();
            dtrPdStI["cRemark3"] = inTemPd["cRemark3"].ToString().TrimEnd();
            dtrPdStI["cRemark4"] = inTemPd["cRemark4"].ToString().TrimEnd();
            dtrPdStI["cRemark5"] = inTemPd["cRemark5"].ToString().TrimEnd();
            dtrPdStI["cRemark6"] = inTemPd["cRemark6"].ToString().TrimEnd();
            dtrPdStI["cRemark7"] = inTemPd["cRemark7"].ToString().TrimEnd();
            dtrPdStI["cRemark8"] = inTemPd["cRemark8"].ToString().TrimEnd();
            dtrPdStI["cRemark9"] = inTemPd["cRemark9"].ToString().TrimEnd();
            dtrPdStI["cRemark10"] = inTemPd["cRemark10"].ToString().TrimEnd();
            dtrPdStI["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrPdStI["nUMQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrPdStI["cScrap"] = inTemPd["cScrap"].ToString().TrimEnd();
            dtrPdStI["cIsMRP"] = (Convert.ToBoolean(inTemPd["IsMRP"]) ? "Y" : "N");
            dtrPdStI["cIsCost"] = (Convert.ToBoolean(inTemPd["IsCost"]) ? "Y" : "N");
            dtrPdStI["cIsOverHd"] = (Convert.ToBoolean(inTemPd["IsOverHead"]) ? "Y" : "N");
            dtrPdStI["cIsIssue"] = (Convert.ToBoolean(inTemPd["IsIssue"]) ? "Y" : "N");

            dtrPdStI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmSaveCostLine(string inAlias, string inMasterH, string inParent, string inType)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            DataRow[] dtrSel = this.dtsDataEnv.Tables[inAlias].Select("cParent = '" + inParent + "'");
            for (int i = 0; i < dtrSel.Length; i++)
            {
                DataRow dtrTemCost = dtrSel[i];

                bool bllIsNewRow = false;

                DataRow dtrCostLine = null;
                if (Convert.ToDecimal(dtrTemCost["nAmt"]) != 0)
                {

                    string strRowID = "";
                    if ((dtrTemCost["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrCostLineTable + " where cRowID = ?", new object[1] { dtrTemCost["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrCostLineTable, this.mstrCostLineTable, "select * from " + this.mstrCostLineTable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrCostLine = this.dtsDataEnv.Tables[this.mstrCostLineTable].NewRow();

                        strRowID = App.mRunRowID(this.mstrCostLineTable);
                        bllIsNewRow = true;
                        //dtrCostLine["cCreateAp"] = App.AppID;
                        dtrTemCost["cRowID"] = strRowID;
                        dtrCostLine["cRowID"] = dtrTemCost["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrCostLineTable, this.mstrCostLineTable, "select * from " + this.mstrCostLineTable + " where cRowID = ?", new object[1] { dtrTemCost["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrCostLine = this.dtsDataEnv.Tables[this.mstrCostLineTable].Rows[0];

                        strRowID = dtrTemCost["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    this.pmReplRecordCostLine(bllIsNewRow, inMasterH, inType, dtrTemCost, ref dtrCostLine);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrCostLine, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemCost["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[] { dtrTemCost["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrCostLineTable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
        }

        private bool pmReplRecordCostLine(bool inState, string inMasterH, string inType, DataRow inTemPd, ref DataRow ioCostLine)
        {

            bool bllIsNewRec = inState;

            DataRow dtrCostLine = ioCostLine;
            //DataRow dtrMasterH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrCostLine["cCorp"] = App.ActiveCorp.RowID;
            dtrCostLine["cType"] = inType;
            dtrCostLine["cCostType"] = inTemPd["cCostType"].ToString();
            dtrCostLine["cRefTab"] = this.mstrITable;
            dtrCostLine["cMasterH"] = inMasterH;
            dtrCostLine["nAmt"] = Convert.ToDecimal(inTemPd["nAmt"]);

            string strCostBy = inTemPd["cCostBy"].ToString().Trim().ToUpper();
            string strCostBy2 = "";
            switch (strCostBy)
            {
                case "-":
                    strCostBy2 = " ";
                    break;
                case "SEC":
                    strCostBy2 = "S";
                    break;
                case "MIN":
                    strCostBy2 = "M";
                    break;
                case "HOUR":
                    strCostBy2 = "H";
                    break;
                case "DAY":
                    strCostBy2 = "D";
                    break;
            }
            dtrCostLine["cCostBy"] = strCostBy2;

            return true;
        }

        private void pmCreateTem()
        {

            //รายการสินค้า
            DataTable dtbTemPd = new DataTable(this.mstrTemPd);
            dtbTemPd.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cBOMHD", System.Type.GetType("System.String"));
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
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("IsMRP", System.Type.GetType("System.Boolean"));
            dtbTemPd.Columns.Add("IsCost", System.Type.GetType("System.Boolean"));
            dtbTemPd.Columns.Add("IsOverHead", System.Type.GetType("System.Boolean"));
            dtbTemPd.Columns.Add("IsIssue", System.Type.GetType("System.Boolean"));
            dtbTemPd.Columns.Add("cProcure", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSubSti", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cMfgBOMHD", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcBOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRoundCtrl", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nCost", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBalQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));

            dtbTemPd.Columns["nActRow"].AutoIncrement = true;
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cBOMHD"].DefaultValue = "";
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
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["IsMRP"].DefaultValue = true;
            dtbTemPd.Columns["IsCost"].DefaultValue = true;
            dtbTemPd.Columns["IsOverHead"].DefaultValue = false;
            dtbTemPd.Columns["IsIssue"].DefaultValue = true;
            dtbTemPd.Columns["cProcure"].DefaultValue = SysDef.gc_PROCURE_TYPE_PURCHASE;
            dtbTemPd.Columns["cMfgBOMHD"].DefaultValue = "";
            dtbTemPd.Columns["cQcBOM"].DefaultValue = "";
            dtbTemPd.Columns["cRoundCtrl"].DefaultValue = SysDef.gc_ROUND_TYPE_NORMAL;
            dtbTemPd.Columns["nCost"].DefaultValue = 0;
            //dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt"].Expression = "nQty*nCost";
            dtbTemPd.Columns["nBalQty"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemPdX = new DataTable(this.mstrTemPdX);
            dtbTemPdX.Columns.Add("nParentID", System.Type.GetType("System.Int32"));
            dtbTemPdX.Columns.Add("nActRow", System.Type.GetType("System.Int32"));
            dtbTemPdX.Columns.Add("nActRow2", System.Type.GetType("System.Int32"));
            dtbTemPdX.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdX.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPdX.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPdX.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPdX.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));

            dtbTemPdX.Columns["nActRow"].DefaultValue = -1;
            dtbTemPdX.Columns["nActRow2"].AutoIncrement = true;
            dtbTemPdX.Columns["cRowID"].DefaultValue = "";
            dtbTemPdX.Columns["cUOMStd"].DefaultValue = "";
            dtbTemPdX.Columns["cRemark1"].DefaultValue = "";
            dtbTemPdX.Columns["nQty"].DefaultValue = 0;
            dtbTemPdX.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPdX.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPdX.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPdX.Columns["nUOMQty"].DefaultValue = 1;

            this.dtsDataEnv.Tables.Add(dtbTemPdX);

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

            //Opr Seq.
            DataTable dtbTemOP = new DataTable(this.mstrTemOP);
            dtbTemOP.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemOP.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cNextOP", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cMOPR", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcMOPR", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQnMOPR", System.Type.GetType("System.String"));
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
            
            dtbTemOP.Columns.Add("cPopGetTime", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cPopGetCost", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQcResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("cQnResource", System.Type.GetType("System.String"));
            dtbTemOP.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_Queue, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_SetUp, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_Process, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_Tear, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_Wait, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add(QMFBOMInfo.Field.LeadTime_Move, System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nCapFactor1", System.Type.GetType("System.Decimal"));
            dtbTemOP.Columns.Add("nCapPress", System.Type.GetType("System.Decimal"));

            dtbTemOP.Columns["cRowID"].DefaultValue = "";
            dtbTemOP.Columns["cOPSeq"].DefaultValue = "";
            dtbTemOP.Columns["cNextOP"].DefaultValue = "";
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

            dtbTemOP.Columns["cQcRemark1"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark2"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark3"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark4"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark5"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark6"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark7"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark8"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark9"].DefaultValue = "";
            dtbTemOP.Columns["cQcRemark10"].DefaultValue = "";
            
            dtbTemOP.Columns["cWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["cQcWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["cQnWHouse2"].DefaultValue = "";
            dtbTemOP.Columns["nScrapQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nLossQty1"].DefaultValue = 0;
            dtbTemOP.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemOP.Columns["nCapFactor1"].DefaultValue = 1;
            dtbTemOP.Columns["nCapPress"].DefaultValue = 0;

            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_Queue].DefaultValue = 0;
            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_SetUp].DefaultValue = 0;
            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_Process].DefaultValue = 0;
            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_Tear].DefaultValue = 0;
            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_Wait].DefaultValue = 0;
            dtbTemOP.Columns[QMFBOMInfo.Field.LeadTime_Move].DefaultValue = 0;

            dtbTemOP.TableNewRow += new DataTableNewRowEventHandler(dtbTemOP_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemOP);

            this.pmCreateTemCost(this.mstrTemCost);
            this.pmCreateTemCost(this.mstrTemCostW);
            this.pmCreateTemCost(this.mstrTemCostT);

            this.pmCreateTemCost(this.mstrTem1Cost);
            this.pmCreateTemCost(this.mstrTem1CostW);
            this.pmCreateTemCost(this.mstrTem1CostT);

        }

        private void pmCreateTemCost(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cParent", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cCostType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCostName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cCostBy", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cParent"].DefaultValue = "";
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCostName"].DefaultValue = "";
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cCostBy"].DefaultValue = "HOUR";
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemCost_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);
        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private void dtbTemOP_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void dtbTemCost_TableNewRow(object sender, DataTableNewRowEventArgs e)
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
                this.pmLoadFormData(false, "");
            }

        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.btnLoad.Enabled = true;

            this.mstrEditRowID = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtName2.Text = "";
            this.txtName2.Text = "";
            this.chkIsActive.Checked = true;

            this.txtQcPlant.Tag = "";
            this.txtQcPlant.Text = "";
            this.txtQnPlant.Text = "";

            this.txtQcProd.Tag = "";
            this.txtQcProd.Text = "";
            this.txtQnProd.Text = "";
            this.txtAttachFile.Text = "";

            this.cmbType.SelectedIndex = 0;
            this.cmdRoundCtrl.SelectedIndex = 0;

            this.txtIsApprove.Text = "";
            this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_WAIT;
            this.txtApproveBy.Text = "";
            this.txtLastUpd.Text = "";
            this.txtQnLastUpd.Text = "";
            this.txtDApprove.EditValue = null;
            this.txtMfgQty.Value = 1;
            this.mdecUMQty = 1;

            this.txtQcUM.Tag = "";
            this.txtQcUM.Text = "";
            this.txtQnUM.Text = "";

            this.txtQnStdUM.Tag = "";
            this.txtQnStdUM.Text = "";

            this.txtScrap.Text = "";
            this.txtAttachFile.Text = "";

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
            this.dtsDataEnv.Tables[this.mstrTemOP].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPdX].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPdX1].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cOPSeq"];
            this.gridView3.FocusedColumn = this.gridView3.Columns["cOPSeq"];

            this.dtsDataEnv.Tables[this.mstrTemCost].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemCostW].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemCostT].Rows.Clear();

            this.dtsDataEnv.Tables[this.mstrTem1Cost].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTem1CostW].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTem1CostT].Rows.Clear();

        }

        private void pmLoadFormData(bool inIsCopy, string inBOMH)
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null || inIsCopy == true)
            {

                this.btnLoad.Enabled = false;
                if (inIsCopy)
                {
                    this.mstrEditRowID = inBOMH;
                }
                else
                {
                    this.mstrEditRowID = dtrBrow["cRowID"].ToString();
                }

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                DataRow dtrRetVal = null;

                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
                {
                
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

                    if (!inIsCopy)
                    {
                        this.txtCode.Text = dtrLoadForm[QMFBOMInfo.Field.Code].ToString().TrimEnd();
                    }

                    this.txtName.Text = dtrLoadForm[QMFBOMInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QMFBOMInfo.Field.Name2].ToString().TrimEnd();
                    this.txtMfgQty.Value = Convert.ToDecimal(dtrLoadForm[QMFBOMInfo.Field.MfgQty]);

                    this.chkIsActive.Checked = (dtrLoadForm[QMFBOMInfo.Field.IsActive].ToString().TrimEnd() == "A" ? true : false);

                    this.txtQcPlant.Tag = dtrLoadForm[QMFBOMInfo.Field.PlantID].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcPlant.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", QEMPlantInfo.TableName, "select * from " + QEMPlantInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcPlant.Text = dtrRetVal[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnPlant.Text = dtrRetVal[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.txtQcProd.Tag = dtrLoadForm[QMFBOMInfo.Field.MfgProdID].ToString();
                    pobjSQLUtil2.SetPara(new object[] { this.txtQcProd.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcProd.Text = dtrRetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnProd.Text = dtrRetVal["fcName"].ToString().TrimEnd();

                        this.txtQnStdUM.Tag = dtrRetVal["fcUM"].ToString();
                        pobjSQLUtil2.SetPara(new object[] { this.txtQnStdUM.Tag.ToString() });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                        {
                            dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                            this.txtQnStdUM.Text = dtrRetVal["fcname"].ToString().TrimEnd();
                        }
                    
                    }

                    this.txtQcUM.Tag = dtrLoadForm[QMFBOMInfo.Field.MfgUMID].ToString();
                    pobjSQLUtil2.SetPara(new object[] { this.txtQcUM.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcUM.Text = dtrRetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnUM.Text = dtrRetVal["fcname"].ToString().TrimEnd();
                    }

                    this.mdecUMQty = Convert.ToDecimal(dtrLoadForm[QMFBOMInfo.Field.MfgUMQty]);

                    this.pmSetUMQtyMsg();

                    //string strRound = this.cmdRoundCtrl.SelectedIndex = 0;
                    string strRound = dtrLoadForm[QMFBOMInfo.Field.RoundCtrl].ToString().TrimEnd();
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

                    this.txtScrap.Text = dtrLoadForm[QMFBOMInfo.Field.Scrap].ToString().TrimEnd();
                    this.txtAttachFile.Text = dtrLoadForm[QMFBOMInfo.Field.FileAttach].ToString().TrimEnd();

                    this.txtRemark1.Text = dtrLoadForm[QMFBOMInfo.Field.Remark1].ToString().TrimEnd();
                    this.txtRemark2.Text = dtrLoadForm[QMFBOMInfo.Field.Remark2].ToString().TrimEnd();
                    this.txtRemark3.Text = dtrLoadForm[QMFBOMInfo.Field.Remark3].ToString().TrimEnd();
                    this.txtRemark4.Text = dtrLoadForm[QMFBOMInfo.Field.Remark4].ToString().TrimEnd();
                    this.txtRemark5.Text = dtrLoadForm[QMFBOMInfo.Field.Remark5].ToString().TrimEnd();
                    this.txtRemark6.Text = dtrLoadForm[QMFBOMInfo.Field.Remark6].ToString().TrimEnd();
                    this.txtRemark7.Text = dtrLoadForm[QMFBOMInfo.Field.Remark7].ToString().TrimEnd();
                    this.txtRemark8.Text = dtrLoadForm[QMFBOMInfo.Field.Remark8].ToString().TrimEnd();
                    this.txtRemark9.Text = dtrLoadForm[QMFBOMInfo.Field.Remark9].ToString().TrimEnd();
                    this.txtRemark10.Text = dtrLoadForm[QMFBOMInfo.Field.Remark10].ToString().TrimEnd();

                    this.txtApproveBy.Tag = dtrLoadForm[QMFBOMInfo.Field.ApproveBy].ToString();
                    string strIsApprove = dtrLoadForm[QMFBOMInfo.Field.IsApprove].ToString().Trim();
                    string strApprove = "";
                    switch (BudgetHelper.GetApproveStep(strIsApprove))
                    {
                        case ApproveStep.Wait:
                            strApprove = " ";
                            this.txtDApprove.EditValue = null;
                            break;
                        case ApproveStep.Approve:
                            strApprove = "/";
                            this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QMFBOMInfo.Field.ApproveDate]);
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

                    this.pmLoadBOMIT_StdOP(inIsCopy, this.mstrTemOP);
                    this.pmLoadBOMIT_Pd(inIsCopy);

                }
                this.pmLoadOldVar();
            }

            if (inIsCopy)
            {
                this.mstrEditRowID = "";
            }
            this.pmSumRMQty();
        }

        private void pmLoadCostLine(bool inIsCopy, string inMasterH, string inParent)
        {
            this.pmLoad1CostLine(inIsCopy, inMasterH, inParent, "O", this.mstrTemCost);
            this.pmLoad1CostLine(inIsCopy, inMasterH, inParent, "W", this.mstrTemCostW);
            this.pmLoad1CostLine(inIsCopy, inMasterH, inParent, "T", this.mstrTemCostT);
        }

        private void pmLoad1CostLine(bool inIsCopy, string inMasterH, string inParent, string inType, string inAlias)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrITable, inMasterH, inType });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCostLine", this.mstrCostLineTable, "select * from " + this.mstrCostLineTable + " where cCorp = ? and cRefTab = ? and cMasterH = ? and cType = ? ", ref strErrorMsg);
            foreach (DataRow dtrCostLine in this.dtsDataEnv.Tables["QCostLine"].Rows)
            {
                string strCostBy = dtrCostLine["cCostBy"].ToString().Trim();
                string strCostBy2 = "";
                switch (strCostBy)
                {
                    case "S":
                        strCostBy2 = SysDef.gc_COST_UNIT_SEC;
                        break;
                    case "M":
                        strCostBy2 = SysDef.gc_COST_UNIT_MIN;
                        break;
                    case "H":
                        strCostBy2 = SysDef.gc_COST_UNIT_HOUR;
                        break;
                    case "D":
                        strCostBy2 = SysDef.gc_COST_UNIT_DAY;
                        break;
                    default:
                        strCostBy2 = SysDef.gc_COST_UNIT_NONE;
                        break;
                }

                string strRowID = (inIsCopy ? "" : dtrCostLine["cRowID"].ToString());
                this.pmUpdateTemCost(inAlias, inParent, dtrCostLine["cCostType"].ToString(), "", strRowID, Convert.ToDecimal(dtrCostLine["nAmt"]), strCostBy2);
            }
        }

        private void pmLoad1CostLine2(string inRefTab, string inMasterH, string inParent, string inType, string inAlias)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inRefTab, inMasterH });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCostLine", this.mstrCostLineTable, "select * from " + this.mstrCostLineTable + " where cCorp = ? and cRefTab = ? and cMasterH = ? ", ref strErrorMsg);
            foreach (DataRow dtrCostLine in this.dtsDataEnv.Tables["QCostLine"].Rows)
            {
                string strCostBy = dtrCostLine["cCostBy"].ToString().Trim();
                string strCostBy2 = "";
                switch (strCostBy)
                {
                    case "S":
                        strCostBy2 = SysDef.gc_COST_UNIT_SEC;
                        break;
                    case "M":
                        strCostBy2 = SysDef.gc_COST_UNIT_MIN;
                        break;
                    case "H":
                        strCostBy2 = SysDef.gc_COST_UNIT_HOUR;
                        break;
                    case "D":
                        strCostBy2 = SysDef.gc_COST_UNIT_DAY;
                        break;
                    default:
                        strCostBy2 = SysDef.gc_COST_UNIT_NONE;
                        break;
                }
                this.pmUpdateTemCost(inAlias, inParent, dtrCostLine["cCostType"].ToString(), "", "", Convert.ToDecimal(dtrCostLine["nAmt"]), strCostBy2);
            }
        }

        private void pmLoadBOMIT_StdOP(bool inIsCopy, string inAlias)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBomIT_StdOP", "WkCtrIT", "select * from " + this.mstrITable + " where cBOMHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBomIT in this.dtsDataEnv.Tables["QBomIT_StdOP"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();

                    intRow++;
                    //dtrNewRow["nRecNo"] = intRow;

                    string strRowID = "";
                    if (inIsCopy)
                    {
                        dtrNewRow["cRowID"] = "";
                    }
                    else
                    {
                        dtrNewRow["cRowID"] = dtrBomIT["cRowID"].ToString();
                    }
                    
                    strRowID = dtrBomIT["cRowID"].ToString();

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

                    dtrNewRow[QMFBOMInfo.Field.LeadTime_Queue] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_Queue]);
                    dtrNewRow[QMFBOMInfo.Field.LeadTime_SetUp] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_SetUp]);
                    dtrNewRow[QMFBOMInfo.Field.LeadTime_Process] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_Process]);
                    dtrNewRow[QMFBOMInfo.Field.LeadTime_Tear] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_Tear]);
                    dtrNewRow[QMFBOMInfo.Field.LeadTime_Wait] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_Wait]);
                    dtrNewRow[QMFBOMInfo.Field.LeadTime_Move] = Convert.ToDecimal(dtrBomIT[QMFBOMInfo.Field.LeadTime_Move]);

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cMOPR"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFStdOprInfo.TableName, "select * from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcMOPR"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFStdOprInfo.Field.Code].ToString().TrimEnd();
                        dtrNewRow["cQnMOPR"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFStdOprInfo.Field.Name].ToString().TrimEnd();
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

                    dtrNewRow["nCapFactor1"] = Convert.ToDecimal(dtrBomIT["nCapFactor1"]);
                    dtrNewRow["nCapPress"] = Convert.ToDecimal(dtrBomIT["nCapPress"]);

                    this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);

                    this.pmLoadCostLine(inIsCopy, strRowID, dtrNewRow["cGUID"].ToString());

                }
            }

        }

        private void pmLoadBOMIT_Pd(bool inIsCopy)
        {
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            int intRecNo = 0;
            string strErrorMsg = "";
            string strSQLStr = "select * from " + this.mstrITable2 + " where cBOMHD = ? order by cSeq ";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPdStI in this.dtsDataEnv.Tables[this.mstrITable2].Rows)
                {
                    intRecNo++;
                    this.pmRepl1RecTemPd(inIsCopy, intRecNo, dtrPdStI);
                }
            }
        }

        private void pmRepl1RecTemPd(bool inIsCopy, int inRecNo, DataRow inPdStI)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

            if (inIsCopy)
            {
                dtrTemPd["cRowID"] = "";
            }
            else
            {
                dtrTemPd["cRowID"] = inPdStI["cRowID"].ToString().TrimEnd();
            }

            //dtrTemPd["nRecNo"] = inRecNo;
            dtrTemPd["cOPSeq"] = inPdStI["cOPSeq"].ToString().TrimEnd();
            dtrTemPd["cProd"] = inPdStI["cProd"].ToString();
            dtrTemPd["cMfgBOMHD"] = inPdStI["cMfgBOMHD"].ToString();
            dtrTemPd["cUOM"] = inPdStI["cUM"].ToString();
            dtrTemPd["cRemark1"] = inPdStI["cRemark1"].ToString().TrimEnd();
            dtrTemPd["cRemark2"] = inPdStI["cRemark2"].ToString().TrimEnd();
            dtrTemPd["cRemark3"] = inPdStI["cRemark3"].ToString().TrimEnd();
            dtrTemPd["cRemark4"] = inPdStI["cRemark4"].ToString().TrimEnd();
            dtrTemPd["cRemark5"] = inPdStI["cRemark5"].ToString().TrimEnd();
            dtrTemPd["cRemark6"] = inPdStI["cRemark6"].ToString().TrimEnd();
            dtrTemPd["cRemark7"] = inPdStI["cRemark7"].ToString().TrimEnd();
            dtrTemPd["cRemark8"] = inPdStI["cRemark8"].ToString().TrimEnd();
            dtrTemPd["cRemark9"] = inPdStI["cRemark9"].ToString().TrimEnd();
            dtrTemPd["cRemark10"] = inPdStI["cRemark10"].ToString().TrimEnd();
            dtrTemPd["cScrap"] = inPdStI["cScrap"].ToString().TrimEnd();
            dtrTemPd["nQty"] = Convert.ToDecimal(inPdStI["nQty"]);
            dtrTemPd["nLastQty"] = Convert.ToDecimal(inPdStI["nQty"]);
            dtrTemPd["nUOMQty"] = Convert.ToDecimal(inPdStI["nUMQty"]);

            dtrTemPd["IsMRP"] = (inPdStI["cIsMRP"].ToString() == "Y" ? true : false);
            dtrTemPd["IsCost"] = (inPdStI["cIsCost"].ToString() == "Y" ? true : false);
            dtrTemPd["IsOverHead"] = (inPdStI["cIsOverHd"].ToString() == "Y" ? true : false);
            dtrTemPd["IsIssue"] = (inPdStI["cIsIssue"].ToString() == "Y" ? true : false);

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cProd"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcType, fcCode, fcName, fcUm, fmPicName from PROD where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cPdType"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcType"].ToString().TrimEnd();
                dtrTemPd["cLastPdType"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcType"].ToString().TrimEnd();
                dtrTemPd["cQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cLastQcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                dtrTemPd["cLastQnProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                dtrTemPd["cUOMStd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QProd"].Rows[0]["fmPicName"]))
                {
                    if (this.dtsDataEnv.Tables["QProd"].Rows[0]["fmPicName"].ToString().Trim() != "...")
                    {
                        dtrTemPd["cAttachFile"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fmPicName"].ToString();
                    }
                }
            }

            pobjSQLUtil2.SetPara(new object[1] { dtrTemPd["cUOM"].ToString() });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcName from UM where fcSkid = ?", ref strErrorMsg))
            {
                dtrTemPd["cQnUOM"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[1] { dtrTemPd["cMfgBOMHD"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOM", "BOM", "select cCode from " + MapTable.Table.MFBOMHead + " where cRowID = ?", ref strErrorMsg))
            {
                dtrTemPd["cQcBOM"] = this.dtsDataEnv.Tables["QBOM"].Rows[0]["cCode"].ToString().TrimEnd();
            }

            dtrTemPd["cProcure"] = inPdStI["cProcure"].ToString().TrimEnd();
            dtrTemPd["cRoundCtrl"] = inPdStI["cRoundCtrl"].ToString().TrimEnd();

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);

        }


        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldName = this.txtName.Text;
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void pmUpdateTemCost(string inAlias, string inParent, string inType, string inCostName, string inRowID, decimal inAmt, string inCostPer)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[inAlias].Select("cCostType='" + inType + "' and cParent = '" + inParent + "' ");
            DataRow dtrNewRow = null;
            if (dtrSel.Length == 0)
            {
                dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();
                this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);
            }
            else
            {
                dtrNewRow = dtrSel[0];
            }

            dtrNewRow["cParent"] = inParent;
            dtrNewRow["cRowID"] = inRowID;
            dtrNewRow["cCostType"] = inType;
            if (inCostName.Trim() != string.Empty)
            {
                dtrNewRow["cCostName"] = inCostName;
            }
            dtrNewRow["nAmt"] = inAmt;
            dtrNewRow["cCostBy"] = inCostPer;
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
                    if (this.pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
                    {
                        if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.pmGotoBrowPage();
                        }
                    }
                    else
                    {
                        this.mbllPopUpResult = false;
                        if (this.mFormActiveMode == FormActiveMode.Edit)
                        {
                            this.Close();
                        }
                        else
                            this.Hide();
                    }
                    break;
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
            if (inOrderBy.ToUpper() == QMFBOMInfo.Field.Code)
                inSearchStr = inSearchStr.PadRight(this.txtCode.Properties.MaxLength);
            else
                inSearchStr = inSearchStr.PadRight(this.txtName.Properties.MaxLength);

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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFBOMInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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
            DataRow dtrTemPd = null;
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
                case "GRDVIEW2_QCRESOURCE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_QCRESOURCE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE");

                    DataRow dtrTemOP = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemOP != null && dtrTemOP["cWkCtrH"].ToString().Trim() != string.Empty)
                    {
                        //this.pofrmGetResource.ValidateField(dtrTemOP["cWkCtrH"].ToString(), "", strOrderBy, true);
                        this.pofrmGetResource.ValidateField("", strOrderBy, true);
                        if (this.pofrmGetResource.PopUpResult)
                        {
                            this.pmRetrievePopUpVal(inKeyField);
                        }
                    }
                    
                    break;
                case "GRDVIEW2_CPOPGETTIME":
                    this.pmInitPopUpDialog("GETOPTIME");
                    break;
                case "GRDVIEW2_CPOPGETCOST":
                    this.pmInitPopUpDialog("GETOPCOST");
                    break;
                case "GRDVIEW3_CQCPROD":
                case "GRDVIEW3_CQNPROD":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW3_CQCPROD" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("PROD");

                    dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

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
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

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
                        dtrTemPd["cQnMOpr"] = "";
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
                            dtrTemPd["cQnMOpr"] = "";
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

                        //e.Valid = !this.pofrmGetResource.ValidateField(dtrTemPd["cWkCtrH"].ToString(), strValue, strOrderBy, false);
                        e.Valid = !this.pofrmGetResource.ValidateField(strValue, strOrderBy, false);
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
            DataRow dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

            string strRefPdType = this.mstrPdType;
            string strProd = "";

            if (dtrTemPd["cPdType"].ToString().Trim() != string.Empty)
            {
                strRefPdType = this.pmSplitToSQLStr(dtrTemPd["cPdType"].ToString());
            }

            if (dtrTemPd["cProd"].ToString().Trim() != string.Empty)
            {
                strProd = dtrTemPd["cProd"].ToString();
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
                case "CQCBOM":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cMfgBOMHD"] = "";
                        dtrTemPd["cQcBOM"] = "";
                    }
                    else
                    {

                        this.pmInitPopUpDialog("BOM");
                        string strOrderBy = (strCol.ToUpper() == "CQCBOM" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetBOM.ValidateField(strProd, strValue, strOrderBy, false);

                        if (this.pofrmGetBOM.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView3.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW3_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCBOM" ? dtrTemPd["cQcBOM"].ToString().TrimEnd() : dtrTemPd["cQnBOM"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cMfgBOMHD"] = "";
                            dtrTemPd["cQcBOM"] = "";
                        }
                    }
                    break;
            }
            //this.gridView3.CloseEditor();
            //this.gridView3.UpdateCurrentRow();
            //this.pmSumRMQty();
        }

        private void pmSumRMQty()
        {
            decimal decSumQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                if (!Convert.IsDBNull(dtrTemPd["nQty"]))
                {
                    decSumQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                }
            }
            this.txtSumRMQty.Value = decSumQty;
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
            DataRow dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

            dtrTemPd["cProd"] = "";
            dtrTemPd["cLastProd"] = "";
            dtrTemPd["cPdType"] = "";
            dtrTemPd["cQcProd"] = "";
            dtrTemPd["cQnProd"] = "";
            dtrTemPd["cLastQcProd"] = "";
            dtrTemPd["cLastQnProd"] = "";
            dtrTemPd["cUOM"] = "";
            dtrTemPd["cQnUOM"] = "";
            dtrTemPd["cMfgBOMHD"] = "";
            dtrTemPd["cQcBOM"] = "";
            dtrTemPd["nQty"] = 0;
        }

        private void btnC1_Click(object sender, EventArgs e)
        {
            if (this.pnlRoute.PanelVisibility == DevExpress.XtraEditors.SplitPanelVisibility.Both)
            {
                this.pnlRoute.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                this.btnC1.Text = "-";
            }
            else
            {
                this.pnlRoute.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                this.btnC1.Text = "+";
            }
        }

        private void btnC2_Click(object sender, EventArgs e)
        {
            if (this.pnlRoute.PanelVisibility == DevExpress.XtraEditors.SplitPanelVisibility.Both)
            {
                this.pnlRoute.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                this.btnC2.Text = "-";
            }
            else
            {
                this.pnlRoute.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                this.btnC2.Text = "+";
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.pmCopyBOM();
        }

        private void pmCopyBOM()
        {
            bool bllResult = false;
            this.pmInitPopUpDialog("BOM");
            bllResult = this.pofrmGetBOM.ValidateField("", "CCODE", true);
            if (this.pofrmGetBOM.PopUpResult)
            {
                this.pmRetrievePopUpVal("LOAD_BOM");
            }
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

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        private void gridView3_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this.gridView3.UpdateCurrentRow();
            this.pmSumRMQty();
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
                    Common.dlgBOMViewCost dlg2 = new BeSmartMRP.DatabaseForms.Common.dlgBOMViewCost();
                    dlg2.BindData(this.dtsDataEnv, this.mstrTemPd, false);
                    dlg2.ShowDialog();
                }
            }
        }

        private decimal mdecSumBOMAmt = 0;
        private decimal mdecSumBOMOPAmt = 0;

        private void pmPSumBOMCost()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            decimal decStdCost = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
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
                decFactorQty = inMfgQty / Convert.ToDecimal(dtrBomH[QMFBOMInfo.Field.MfgQty]);

                foreach (DataRow dtrPBOMHD in this.dtsDataEnv.Tables["QBOMHD"].Rows)
                {
                    //Print OP COST
                    decimal decTimeFactor = 1;
                    decimal decOPTime = Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Queue])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_SetUp])
                        + (Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor)
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Tear])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Wait])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Move]);

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
                decimal decMfgQty = decFactorQty * Convert.ToDecimal(dtrBOMItem["nQty"]);
                decimal decPrice = 0;

                if (this.mintPrint_RMWHCost == 0)
                {
                    pobjSQLUtil2.SetPara(new object[] { dtrBOMItem["cProd"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ? ", ref strErrorMsg))
                    {
                        decPrice = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["FNSTDCOST"]);
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


    }
}
