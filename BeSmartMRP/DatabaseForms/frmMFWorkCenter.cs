
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

    public partial class frmMFWorkCenter : UIHelper.frmBase, IfrmDBBase
    {

        public static string TASKNAME = "EMFWKCTR";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWorkCenterInfo.TableName;
        private string mstrITable = MapTable.Table.MFWorkCenterItem_Res;
        private string mstrCostLineTable = MapTable.Table.CostLine;

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrTemMachine = "TemMachine";
        private string mstrTemTool = "TemTool";
        private string mstrTemCost = "TemCost";


        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QMFWorkCenterInfo.Field.Code;
        private bool mbllFilterResult = false;
        private string mstrFixType = "";
        private string mstrFixPlant = "";

        private string mstrEditRowID = "";
        private string mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;
        private string mstrPlant = "";
        private string mstrFormMenuName = "Work Center";

        private UIHelper.AppFormState mFormEditMode;

        private MfgResourceType mResourceType = MfgResourceType.Machine;

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        private DatabaseForms.frmMFResource pofrmResource = null;
        private DatabaseForms.frmMFResource pofrmResource2 = null;

        private DatabaseForms.frmMFResType pofrmGetResType = null;
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

        public frmMFWorkCenter()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFWorkCenter(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmMFWorkCenter mInstanse_1 = null;

        public static frmMFWorkCenter GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFWorkCenter();
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

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmCreateTem();
            this.pmInitGridProp_TemMachine();
            this.pmInitGridProp_TemTool();
            this.pmInitGridProp_TemCost();

            this.pmInitializeComponent();
            this.pmFilterForm();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();

            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
            if (!this.mbllFilterResult)
            {
                this.Close();
                //frmBudAllocate.pmClearInstanse();
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
            this.pmSetFormUI();
            this.pmMapEvent();

            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetFormUI()
        {

            this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
            this.lblJob.Text = UIBase.GetAppUIText(new string[] { "โครงการ :", "Job :" });

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWorkCenterInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWorkCenterInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QMFWorkCenterInfo.Field.Name2);

            this.cmbType.Properties.Items.Clear();
            this.cmbType.Properties.Items.AddRange(new object[] { "M = Machine", "G = GROUP" });
            this.cmbType.SelectedIndex = 0;

        }

        private void pmMapEvent()
        {

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);

            this.grdTemPd.Resize += new EventHandler(grdTemPd_Resize);
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            this.gridView2.ColumnWidthChanged += new ColumnEventHandler(gridView2_ColumnWidthChanged);
            this.gridView2.GotFocus += new EventHandler(gridView2_GotFocus);

            this.grdTemPd2.Resize += new EventHandler(grdTemPd2_Resize);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);
            this.gridView3.ColumnWidthChanged += new ColumnEventHandler(gridView3_ColumnWidthChanged);
            this.gridView3.GotFocus += new EventHandler(gridView3_GotFocus);

            this.grcQcMachine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcMachine.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.grcQcTool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcTool.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select MFWkCtrHd.CROWID, MFWkCtrHd.CCODE, MFWkCtrHd.CNAME, MFWkCtrHd.CNAME2, MFWkCtrHd.DCREATE, MFWkCtrHd.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} MFWkCtrHd ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = MFWkCtrHd.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = MFWkCtrHd.CLASTUPDBY ";
            //strSQLExec += " where MFWkCtrHd.CCORP = ? and MFWkCtrHd.CPLANT = ? ";
            strSQLExec += " where MFWkCtrHd.CCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            //pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant });
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "MFWkCtrHd", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QMFWorkCenterInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QMFWorkCenterInfo.Field.Code].Visible = true;
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name].Visible = true;
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name2].Visible = true;

            this.gridView1.Columns[QMFWorkCenterInfo.Field.Code].Caption = "WorkCenter Code";
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name].Caption = "WorkCenter Name";
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name2].Caption = "WorkCenter Name 2";

            this.gridView1.Columns[QMFWorkCenterInfo.Field.Code].Width = 15;
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name2].Width = 25;

            this.gridView1.Columns[QMFWorkCenterInfo.Field.Code].VisibleIndex = 0;
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name].VisibleIndex = 1;
            this.gridView1.Columns[QMFWorkCenterInfo.Field.Name2].VisibleIndex = 2;

            this.pmSetSortKey(QMFWorkCenterInfo.Field.Code, true);
        }

        private void pmInitGridProp_TemMachine()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemMachine].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemMachine];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            this.gridView2.Columns["nRecNo"].VisibleIndex = 0;
            this.gridView2.Columns["cQcMachine"].VisibleIndex = 1;
            this.gridView2.Columns["cQnMachine"].VisibleIndex = 2;
            this.gridView2.Columns["cRemark1"].VisibleIndex = 3;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cQcMachine"].Caption = "Machine Code";
            this.gridView2.Columns["cQnMachine"].Caption = "Machine Name";
            this.gridView2.Columns["cRemark1"].Caption = "Remark";

            this.gridView2.Columns["nRecNo"].Width = 50;
            this.gridView2.Columns["cQcMachine"].Width = 120;
            this.gridView2.Columns["cQnMachine"].Width = 250;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.grcQcMachine.Buttons[0].Tag = "GRDVIEW2_CQCMACHINE";

            this.grcQcMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Code);
            this.grcQnMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Name);
            this.grcRemark.MaxLength = 150;

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQnMachine"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cQnMachine"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cQnMachine"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQcMachine"].ColumnEdit = this.grcQcMachine;
            this.gridView2.Columns["cQnMachine"].ColumnEdit = this.grcQnMachine;
            this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark;
            this.pmCalcColWidth();
        }

        private void pmInitGridProp_TemTool()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemTool].DefaultView;

            this.grdTemPd2.DataSource = this.dtsDataEnv.Tables[this.mstrTemTool];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            this.gridView3.Columns["nRecNo"].VisibleIndex = 0;
            this.gridView3.Columns["cQcMachine"].VisibleIndex = 1;
            this.gridView3.Columns["cQnMachine"].VisibleIndex = 2;
            this.gridView3.Columns["cRemark1"].VisibleIndex = 3;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["cQcMachine"].Caption = "Tool Code";
            this.gridView3.Columns["cQnMachine"].Caption = "Tool Name";
            this.gridView3.Columns["cRemark1"].Caption = "Remark";

            this.gridView3.Columns["nRecNo"].Width = 50;
            this.gridView3.Columns["cQcMachine"].Width = 120;
            this.gridView3.Columns["cQnMachine"].Width = 250;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.grcQcTool.Buttons[0].Tag = "GRDVIEW3_CQCMACHINE";
            this.grcQcTool.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Code);
            this.grcQnTool.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Name);
            //this.grcRemark.MaxLength = 150;

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQnMachine"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cQnMachine"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cQnMachine"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cQcMachine"].ColumnEdit = this.grcQcTool;
            this.gridView3.Columns["cQnMachine"].ColumnEdit = this.grcQnTool;
            this.gridView3.Columns["cRemark1"].ColumnEdit = this.grcRemark;
            this.pmCalcColWidth2();
        }

        private void pmInitGridProp_TemCost()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemCost].DefaultView;

            this.grdTemCost.DataSource = this.dtsDataEnv.Tables[this.mstrTemCost];

            for (int intCnt = 0; intCnt < this.gridView4.Columns.Count; intCnt++)
            {
                this.gridView4.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView4.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView4.Columns["cCostType"].VisibleIndex = i++;
            this.gridView4.Columns["cCostName"].VisibleIndex = i++;
            this.gridView4.Columns["nAmt"].VisibleIndex = i++;
            this.gridView4.Columns["cCostBy"].VisibleIndex = i++;

            this.gridView4.Columns["nRecNo"].Caption = "No.";
            this.gridView4.Columns["cCostType"].Caption = "Cost Type";
            this.gridView4.Columns["cCostName"].Caption = "Cost Name";
            this.gridView4.Columns["nAmt"].Caption = "Amt";
            this.gridView4.Columns["cCostBy"].Caption = "Amt. Per";

            this.gridView4.Columns["nRecNo"].Width = 30;
            this.gridView4.Columns["cCostType"].Width = 80;
            this.gridView4.Columns["cCostName"].Width = 200;
            this.gridView4.Columns["nAmt"].Width = 80;
            this.gridView4.Columns["cCostBy"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView4.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView4.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cCostType"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cCostType"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cCostType"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cCostName"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView4.Columns["cCostName"].OptionsColumn.AllowFocus = false;
            this.gridView4.Columns["cCostName"].OptionsColumn.ReadOnly = true;

            this.gridView4.Columns["cCostBy"].ColumnEdit = this.grcCostBy;
            this.gridView4.Columns["nAmt"].ColumnEdit = this.grcAmt;

            //this.pmCalcColWidth();
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView2.Columns["nRecNo"].Width
                                    + this.gridView2.Columns["cQcMachine"].Width
                                    + this.gridView2.Columns["cQnMachine"].Width;

            int intNewWidth = this.Width - intColWidth - 80;
            this.gridView2.Columns["cRemark1"].Width = (intNewWidth > 0 ? intNewWidth : 70);
        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView3.Columns["nRecNo"].Width
                                    + this.gridView3.Columns["cQcMachine"].Width
                                    + this.gridView3.Columns["cQnMachine"].Width;

            int intNewWidth = this.Width - intColWidth - 80;
            this.gridView3.Columns["cRemark1"].Width = (intNewWidth > 0 ? intNewWidth : 70);
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

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);
        }

        private void pmFilterForm()
        {
            this.mbllFilterResult = true;
            //this.pmInitPopUpDialog("FILTER");
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
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Common.dlgMfgFilter01 dlgFilter = new Common.dlgMfgFilter01(""))
                    {

                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrPlant = dlgFilter.PlantID;

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();

                        }
                    }
                    break;
                case "RESOURCE":
                    if (this.pofrmResource == null)
                    {
                        this.pofrmResource = new frmMFResource(FormActiveMode.PopUp);
                        //this.pofrmGetResType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetResType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    this.pofrmResource.PlantID = this.mstrPlant;
                    this.pofrmResource.ResourceType = MfgResourceType.Machine;
                    this.pofrmResource.ReInitForm();
                    break;
                case "RESOURCE2":
                    if (this.pofrmResource2 == null)
                    {
                        this.pofrmResource2 = new frmMFResource(FormActiveMode.PopUp);
                        //this.pofrmGetResType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetResType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    this.pofrmResource2.PlantID = this.mstrPlant;
                    this.pofrmResource2.ResourceType = MfgResourceType.Tool;
                    this.pofrmResource2.ReInitForm();
                    break;
                case "RESTYPE":
                    if (this.pofrmGetResType == null)
                    {
                        this.pofrmGetResType = new frmMFResType(FormActiveMode.PopUp);
                        //this.pofrmGetResType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetResType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    this.pofrmGetResType.ResourceType = this.mResourceType;
                    this.pofrmGetResType.ReInitForm();
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

        private void pmPopUpButtonClick(string inTextbox, string inPara1)
        {
            string strPrefix = "";
            switch (inTextbox)
            {
                case "TXTQCMCTYPE":
                case "TXTQNMCTYPE":
                    this.pmInitPopUpDialog("RESTYPE");
                    strPrefix = (inTextbox == "TXTQCMCTYPE" ? "CCODE" : "CNAME");
                    this.pofrmGetResType.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetResType.PopUpResult)
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
                        this.txtQnSect.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcSect.Tag = "";
                        this.txtQcSect.Text = "";
                        this.txtQnSect.Text = "";
                    }
                    break;

                case "TXTQCJOB":
                case "TXTQNJOB":

                    dtrGetVal = this.pofrmGetJob.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        this.txtQcJob.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcJob.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnJob.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcJob.Tag = "";
                        this.txtQcJob.Text = "";
                        this.txtQnJob.Text = "";
                    }
                    break;

                case "GRDVIEW2_CQCMACHINE":
                case "GRDVIEW2_CQNMACHINE":
                    dtrGetVal = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemMachine].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemMachine].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrResource = this.pofrmResource.RetrieveValue();
                    if (dtrResource != null)
                    {
                        dtrGetVal["cMachine"] = dtrResource[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcMachine"] = dtrResource[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                        dtrGetVal["cQnMachine"] = dtrResource[QEMAcChartInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cMachine"] = "";
                        dtrGetVal["cQcMachine"] = "";
                        dtrGetVal["cQnMachine"] = "";
                    }

                    this.gridView2.UpdateCurrentRow();
                    break;

                case "GRDVIEW3_CQCMACHINE":
                case "GRDVIEW3_CQNMACHINE":
                    dtrGetVal = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemTool].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemTool].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrResource2 = this.pofrmResource2.RetrieveValue();
                    if (dtrResource2 != null)
                    {
                        dtrGetVal["cMachine"] = dtrResource2[MapTable.ShareField.RowID].ToString();
                        dtrGetVal["cQcMachine"] = dtrResource2[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                        dtrGetVal["cQnMachine"] = dtrResource2[QEMAcChartInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cMachine"] = "";
                        dtrGetVal["cQcMachine"] = "";
                        dtrGetVal["cQnMachine"] = "";
                    }

                    this.gridView3.UpdateCurrentRow();
                    break;
            
            }
        }

        private void pmClr1TemPd()
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrTemMachine].Rows[this.gridView2.FocusedRowHandle];
            DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemMachine].NewRow();

            string strRowID = dtrCurrRow["cRowID"].ToString();
            int intRecNo = Convert.ToInt32(dtrCurrRow["nRecNo"]);
            DataSetHelper.CopyDataRow(dtrNewRow, ref dtrCurrRow);
            dtrCurrRow["cRowID"] = strRowID;
            dtrCurrRow["nRecNo"] = intRecNo;
            //dtrTemPd["cIsDel"] = "Y";
        }

        private void txtQcSect_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSECT" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcSect.Tag = "";
                this.txtQcSect.Text = "";
                this.txtQnSect.Text = "";
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
                this.txtQnJob.Text = "";
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

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QMFWorkCenterInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QMFWorkCenterInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFWorkCenterInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูล" + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFWorkCenterInfo.Field.Code].ToString(), dtrBrow[QMFWorkCenterInfo.Field.Name].ToString(), ref strErrorMsg))
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

                object[] pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWkCtrH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //Delete CostLine
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrRefTable, this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrCostLineTable + " where cCorp = ? and cRefTab = ? and cMasterH = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrType, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { this.mstrFormMenuName + " ซ้ำ", "Duplicate " + this.mstrFormMenuName + " !" });
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
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QMFWorkCenterInfo.Field.Code].ToString().Trim();
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cCode = ?", ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cName = ?", ref strErrorMsg))
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

            dtrSaveInfo[QMFWorkCenterInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFWorkCenterInfo.Field.PlantID] = this.mstrPlant;
            dtrSaveInfo[QMFWorkCenterInfo.Field.Type] = this.mstrType;
            dtrSaveInfo[QMFWorkCenterInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QMFWorkCenterInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QMFWorkCenterInfo.Field.Name2] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo[QMFWorkCenterInfo.Field.Department] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFWorkCenterInfo.Field.Job] = this.txtQcJob.Tag.ToString();

            dtrSaveInfo[QMFWorkCenterInfo.Field.Capacity] = Convert.ToDecimal(this.txtCapacity.Value);
            dtrSaveInfo[QMFWorkCenterInfo.Field.Load_Maximum] = Convert.ToDecimal(this.txtOverLoad.Value);
            dtrSaveInfo[QMFWorkCenterInfo.Field.Load_Minimum] = Convert.ToDecimal(this.txtUnderLoad.Value);

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

                this.pmUpdateWorkCenterIT(this.mstrTemMachine);
                this.pmUpdateWorkCenterIT(this.mstrTemTool);

                this.pmSaveCostLine();

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

        private void pmUpdateWorkCenterIT(string inAlias)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {

                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cMachine"].ToString().TrimEnd() != string.Empty)
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

                    this.pmReplRecordWkCtrIT(bllIsNewRow, (inAlias == this.mstrTemMachine ? SysDef.gc_RESOURCE_TYPE_MACHINE : SysDef.gc_RESOURCE_TYPE_TOOL), dtrTemPd, ref dtrBudCI);

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

        private bool pmReplRecordWkCtrIT(bool inState, string inType, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];


            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cPlant"] = this.mstrPlant;
            dtrBudCI["cType"] = inType;
            dtrBudCI["cWkCtrH"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cSect"] = dtrBudCH["cSect"].ToString();
            dtrBudCI["cJob"] = dtrBudCH["cJob"].ToString();
            dtrBudCI["cResource"] = inTemPd["cMachine"].ToString();
            dtrBudCI["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            //dtrBudCI["cType"] = inTemPd["cType"].ToString();
            dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmSaveCostLine()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemCost in this.dtsDataEnv.Tables[this.mstrTemCost].Rows)
            {

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

                    this.pmReplRecordCostLine(bllIsNewRow, dtrTemCost, ref dtrCostLine);

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

        private bool pmReplRecordCostLine(bool inState, DataRow inTemPd, ref DataRow ioCostLine)
        {

            bool bllIsNewRec = inState;

            DataRow dtrCostLine = ioCostLine;
            //DataRow dtrMasterH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrCostLine["cCorp"] = App.ActiveCorp.RowID;
            dtrCostLine["cCostType"] = inTemPd["cCostType"].ToString();
            dtrCostLine["cRefTab"] = this.mstrRefTable.ToUpper();
            dtrCostLine["cMasterH"] = this.mstrEditRowID;
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
            this.pmCreate1Tem(this.mstrTemMachine);
            this.pmCreate1Tem(this.mstrTemTool);

            DataTable dtbTemPd = new DataTable(this.mstrTemCost);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cCostType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCostName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cCostBy", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCostName"].DefaultValue = "";
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cCostBy"].DefaultValue = "HOUR";
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemCost_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);
        
        }

        private void pmCreate1Tem(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cMCType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcMCType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcMCType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cMachine", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcMachine", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnMachine", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcMachine", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnMachine", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cMCType"].DefaultValue = "";
            dtbTemPd.Columns["cQcMCType"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcMCType"].DefaultValue = "";
            dtbTemPd.Columns["cMachine"].DefaultValue = "";
            dtbTemPd.Columns["cQcMachine"].DefaultValue = "";
            dtbTemPd.Columns["cQnMachine"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcMachine"].DefaultValue = "";
            dtbTemPd.Columns["cLastQnMachine"].DefaultValue = "";
            dtbTemPd.Columns["cRemark1"].DefaultValue = "";
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);
        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private void dtbTemCost_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private void pmLoadEditPage()
        {
            this.pmCalcColWidth();
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
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.mstrEditRowID = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtName2.Text = "";
            this.txtName2.Text = "";

            this.txtCapacity.Value = 0;
            this.txtOverLoad.Value = 100;
            this.txtUnderLoad.Value = 0;

            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQnSect.Text = "";

            this.txtQcJob.Tag = "";
            this.txtQcJob.Text = "";
            this.txtQnJob.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemMachine].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemTool].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcMachine"];
            this.gridView3.FocusedColumn = this.gridView2.Columns["cQcMachine"];
            this.pgfEditItem.SelectedTabPageIndex = 0;

            this.dtsDataEnv.Tables[this.mstrTemCost].Rows.Clear();
            this.pmInsertTemCost();

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
                    this.txtCode.Text = dtrLoadForm[QMFWorkCenterInfo.Field.Code].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm[QMFWorkCenterInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QMFWorkCenterInfo.Field.Name2].ToString().TrimEnd();

                    this.txtCapacity.Value = Convert.ToDecimal(dtrLoadForm[QMFWorkCenterInfo.Field.Capacity]);
                    this.txtOverLoad.Value = Convert.ToDecimal(dtrLoadForm[QMFWorkCenterInfo.Field.Load_Maximum]);
                    this.txtUnderLoad.Value = Convert.ToDecimal(dtrLoadForm[QMFWorkCenterInfo.Field.Load_Minimum]);

                    this.txtQcSect.Tag = dtrLoadForm[QMFWorkCenterInfo.Field.Department].ToString();
                    pobjSQLUtil2.SetPara(new object[] { this.txtQcSect.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", QEMSectInfo.TableName, "select * from " + QEMSectInfo.TableName + " where " + QEMSectInfo .Field.RowID+ " = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcSect.Text = dtrRetVal[QEMSectInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnSect.Text = dtrRetVal[QEMSectInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QMFWorkCenterInfo.Field.Job].ToString();
                    pobjSQLUtil2.SetPara(new object[] { this.txtQcJob.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", QEMJobInfo.TableName, "select * from " + QEMJobInfo.TableName + " where " + QEMJobInfo.Field.RowID + " = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcJob.Text = dtrRetVal[QEMJobInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnJob.Text = dtrRetVal[QEMJobInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.pmLoadWkCtrIT(this.mstrTemMachine);
                    this.pmLoadWkCtrIT(this.mstrTemTool);
                    this.pmLoadCostLine();
                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldName = this.txtName.Text;
        }

        private void pmLoadWkCtrIT(string inAlias)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            string strType = (inAlias == this.mstrTemMachine ? SysDef.gc_RESOURCE_TYPE_MACHINE : SysDef.gc_RESOURCE_TYPE_TOOL);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, strType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWkCtrIT", "WkCtrIT", "select * from " + this.mstrITable + " where cWkCtrH = ? and cType = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrWkCtrIT in this.dtsDataEnv.Tables["QWkCtrIT"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();

                    intRow++;
                    //dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrWkCtrIT["cRowID"].ToString();
                    dtrNewRow["cMachine"] = dtrWkCtrIT["cResource"].ToString();
                    dtrNewRow["cRemark1"] = dtrWkCtrIT["cRemark1"].ToString().TrimEnd();

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cMachine"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFResourceInfo.TableName, "select * from " + QMFResourceInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcMachine"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Code].ToString().TrimEnd();
                        dtrNewRow["cQnMachine"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);

                }
            }

        }

        private void pmLoadCostLine()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrRefTable, this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCostLine", this.mstrCostLineTable, "select * from " + this.mstrCostLineTable + " where cCorp = ? and cRefTab = ? and cMasterH = ?", ref strErrorMsg);
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
                this.pmUpdateTemCost(dtrCostLine["cCostType"].ToString(), "", dtrCostLine["cRowID"].ToString(), Convert.ToDecimal(dtrCostLine["nAmt"]), strCostBy2);
            }
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void pmInsertTemCost()
        {
            string strCostName_FIX = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) : "Fix Cost");
            string strCostName_VAR1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) : "Variable Cost per Man-hour 1");
            string strCostName_VAR2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) : "Variable Cost per Man-hour 2");
            string strCostName_VAR3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) : "Variable Cost per Man-hour 3");
            string strCostName_VAR4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) : "Variable Cost per Man-hour 4");
            string strCostName_VAR5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) : "Variable Cost per Man-hour 5");
            string strCostName_VARP1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) : "Variable Cost per Product output 1");
            string strCostName_VARP2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) : "Variable Cost per Product output 2");
            string strCostName_VARP3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) : "Variable Cost per Product output 3");
            string strCostName_VARP4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) : "Variable Cost per Product output 4");
            string strCostName_VARP5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) : "Variable Cost per Product output 5");

            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_FIX, strCostName_FIX, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARM1, strCostName_VAR1, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARM2, strCostName_VAR2, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARM3, strCostName_VAR3, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARM4, strCostName_VAR4, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARM5, strCostName_VAR5, "", 0, SysDef.gc_COST_UNIT_HOUR);

            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARP1, strCostName_VARP1, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARP2, strCostName_VARP2, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARP3, strCostName_VARP3, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARP4, strCostName_VARP4, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(SysDef.gc_REF_OP_COSTTYPE_VARP5, strCostName_VARP5, "", 0, SysDef.gc_COST_UNIT_NONE);

        }

        private void pmUpdateTemCost(string inType, string inCostName, string inRowID, decimal inAmt, string inCostPer)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemCost].Select("cCostType='" + inType + "'");
            DataRow dtrNewRow = null;
            if (dtrSel.Length == 0)
            {
                dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemCost].NewRow();
                this.dtsDataEnv.Tables[this.mstrTemCost].Rows.Add(dtrNewRow);
            }
            else
            {
                dtrNewRow = dtrSel[0];
            }
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
            if (inOrderBy.ToUpper() == QMFWorkCenterInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QMFWorkCenterInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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
                case "GRDVIEW2_CQCMACHINE":
                case "GRDVIEW2_CQNMACHINE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE");
                    this.pofrmResource.ValidateField("", strOrderBy, true);
                    if (this.pofrmResource.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
                case "GRDVIEW3_CQCMACHINE":
                case "GRDVIEW3_CQNMACHINE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW3_CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE2");
                    this.pofrmResource2.ValidateField("", strOrderBy, true);
                    if (this.pofrmResource2.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;
            }
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCMACHINE":
                case "CQNMACHINE":

                    string strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cMachine"] = "";
                        dtrTemPd["cQcMachine"] = "";
                        dtrTemPd["cQnMachine"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("RESOURCE");
                        string strOrderBy = (strCol.ToUpper() == "CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                        e.Valid = !this.pofrmResource.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmResource.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCMACHINE" ? dtrTemPd["cQcMachine"].ToString().TrimEnd() : dtrTemPd["cQnMachine"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cMachine"] = "";
                            dtrTemPd["cQcMachine"] = "";
                            dtrTemPd["cQnMachine"] = "";
                        }
                    }
                    break;
            }

        }

        private void gridView3_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd2.MainView as ColumnView;

            string strCol = gridView3.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCMACHINE":
                case "CQNMACHINE":

                    string strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cMachine"] = "";
                        dtrTemPd["cQcMachine"] = "";
                        dtrTemPd["cQnMachine"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("RESOURCE2");
                        string strOrderBy = (strCol.ToUpper() == "CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                        e.Valid = !this.pofrmResource2.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmResource2.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView3.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW3_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCMACHINE" ? dtrTemPd["cQcMachine"].ToString().TrimEnd() : dtrTemPd["cQnMachine"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cMachine"] = "";
                            dtrTemPd["cQcMachine"] = "";
                            dtrTemPd["cQnMachine"] = "";
                        }
                    }
                    break;
            }

        }

        private void grcGridColumn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmGridPopUpButtonClick(e.Button.Tag.ToString());
        }

        private void grcGridColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
                    break;
            }
        }

        private void grdTemPd_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void grdTemPd2_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth2();
        }

        private void gridView2_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView3_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth2();
        }


        private void gridView2_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemMachine;
            this.mActiveGrid = this.gridView2;
            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcMachine"];
        }

        private void gridView3_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemTool;
            this.mActiveGrid = this.gridView3;
            this.gridView3.FocusedColumn = this.gridView3.Columns["cQcMachine"];
        }

        private void grbCapacity_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
