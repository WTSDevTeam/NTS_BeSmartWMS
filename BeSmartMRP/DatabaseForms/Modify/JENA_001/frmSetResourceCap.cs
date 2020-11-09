
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

namespace BeSmartMRP.DatabaseForms.Modify.JENA_001
{
    public partial class frmSetResourceCap : UIHelper.frmBase
    {

        public static string TASKNAME = "ERESTYPE";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFPDCapInfo.TableName;
        private string mstrITable = MapTable.Table.PDCapIT;
        
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "QCSECT";
        private string mstrFixType = "";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrTemCap = "TemCap";

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private DatabaseForms.frmMFWorkCenter pofrmGetWkCtr = null;

        private string mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;
        private MfgResourceType mResourceType = MfgResourceType.Machine;
        private string mstrFormMenuName = "";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmSetResourceCap()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmSetResourceCap(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmSetResourceCap mInstanse_1 = null;

        public static frmSetResourceCap GetInstanse(MfgResourceType inResourceType)
        {

            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmSetResourceCap();
            }
            return mInstanse_1;
        }

        private static void pmClearInstanse(MfgResourceType inResourceType)
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

        public MfgResourceType ResourceType
        {
            get { return this.mResourceType; }
            set { this.mResourceType = value; }
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
            this.pmInitGridProp_TemMachine();
            
            this.pmInitializeComponent();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();
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
            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetFormUI()
        {
            //this.lblCode.Text = this.mstrFormMenuName + " Code :";
            //this.lblName.Text = this.mstrFormMenuName + " Name :";
            //this.lblName2.Text = this.mstrFormMenuName + " Name 2 :";

            //this.Text = this.mstrFormMenuName + " Items";
            //TASKNAME = "ERESTYPE_" + this.mstrType;
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);

            this.txtQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtQcWkCtr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcWkCtr.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcWkCtr.Validating += new CancelEventHandler(txtQcWkCtr_Validating);

        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemCap);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cResType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcResType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnResType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cResource", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcResource", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnResource", System.Type.GetType("System.String"));

            //nCap1Hour = กำลังการผลิตต่อชั่วโมง
            dtbTemPd.Columns.Add("nCap1Hour", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            //nCap1Day = กำลังการผลิตต่อวัน
            //nCap1Day = nCap1Hour * this.txtWkHour.Value
            dtbTemPd.Columns.Add("nCap1Day", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nResCount", System.Type.GetType("System.Decimal"));
            //nCap1Hour2 = nCap1Hour * nResCount คือเอาจำนวนเครื่องจักร * กำลังการผลิตต่อชั่วโมง
            dtbTemPd.Columns.Add("nCap1Hour2", System.Type.GetType("System.Decimal"));
            //nCap1Day2 = nCap1Day * nResCount คือเอาจำนวนเครื่องจักร * กำลังการผลิตต่อวัน
            dtbTemPd.Columns.Add("nCap1Day2", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOT1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOT2", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOT3", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOT4", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOT5", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cResType"].DefaultValue = "";
            dtbTemPd.Columns["cQcResType"].DefaultValue = "";
            dtbTemPd.Columns["cQnResType"].DefaultValue = "";
            dtbTemPd.Columns["cResource"].DefaultValue = "";
            dtbTemPd.Columns["cQcResource"].DefaultValue = "";
            dtbTemPd.Columns["cQnResource"].DefaultValue = "";
            dtbTemPd.Columns["nCap1Hour"].DefaultValue = 0;
            dtbTemPd.Columns["cQnUM"].DefaultValue = "";
            dtbTemPd.Columns["nCap1Day"].DefaultValue = 0;
            dtbTemPd.Columns["nResCount"].DefaultValue = 0;
            dtbTemPd.Columns["nCap1Hour2"].DefaultValue = 0;
            dtbTemPd.Columns["nCap1Day2"].DefaultValue = 0;
            dtbTemPd.Columns["nCap1Hour2"].DefaultValue = 0;
            dtbTemPd.Columns["nOT1"].DefaultValue = 0;
            dtbTemPd.Columns["nOT2"].DefaultValue = 0;
            dtbTemPd.Columns["nOT3"].DefaultValue = 0;
            dtbTemPd.Columns["nOT4"].DefaultValue = 0;
            dtbTemPd.Columns["nOT5"].DefaultValue = 0;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            //dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemCost_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strSectTab = strFMDBName + ".dbo.SECT";

            string strSQLExec = "select MFPDCAPH.CROWID, SECT.FCCODE as QCSECT, MFWKCTRHD.CCODE as QCWKCTR, MFPDCAPH.NWKHOUR, MFPDCAPH.DCREATE, MFPDCAPH.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} MFPDCAPH ";
            strSQLExec += " left join " + strSectTab + " SECT on SECT.FCSKID = MFPDCAPH.CSECT ";
            strSQLExec += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFPDCAPH.CWKCTRH ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = MFPDCAPH.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = MFPDCAPH.CLASTUPDBY ";
            strSQLExec += " where MFPDCAPH.CCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "MFPDCAPH", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "QCSECT";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["QCSECT"].VisibleIndex = i++;
            this.gridView1.Columns["QCWKCTR"].VisibleIndex = i++;
            this.gridView1.Columns["NWKHOUR"].VisibleIndex = i++;

            this.gridView1.Columns["QCSECT"].Caption = "Department";
            this.gridView1.Columns["QCWKCTR"].Caption = "W/C";
            this.gridView1.Columns["NWKHOUR"].Caption = "Work Hour";

            this.gridView1.Columns["QCSECT"].Width = 120;
            this.gridView1.Columns["QCWKCTR"].Width = 120;
            this.gridView1.Columns["NWKHOUR"].Width = 70;

            this.pmSetSortKey("QCSECT", true);
        }

        private void pmInitGridProp_TemMachine()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemCap].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemCap];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
                this.gridView2.Columns[intCnt].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gridView2.Columns[intCnt].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;

                this.gridView2.Columns[intCnt].AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
                this.gridView2.Columns[intCnt].OptionsColumn.AllowFocus = true;
                this.gridView2.Columns[intCnt].OptionsColumn.ReadOnly = true;
            
            }

            int i = 0;
            this.gridView2.Columns["cQnResType"].VisibleIndex = i++;
            this.gridView2.Columns["cQcResource"].VisibleIndex = i++;
            this.gridView2.Columns["cQnResource"].VisibleIndex = i++;
            this.gridView2.Columns["cQnUM"].VisibleIndex = i++;
            this.gridView2.Columns["nCap1Hour"].VisibleIndex = i++;
            this.gridView2.Columns["nCap1Hour2"].VisibleIndex = i++;
            this.gridView2.Columns["nCap1Day"].VisibleIndex = i++;
            //this.gridView2.Columns["nResCount"].VisibleIndex = i++;
            //this.gridView2.Columns["nCap1Day2"].VisibleIndex = i++;
            this.gridView2.Columns["nOT1"].VisibleIndex = i++;
            this.gridView2.Columns["nOT2"].VisibleIndex = i++;
            this.gridView2.Columns["nOT3"].VisibleIndex = i++;
            this.gridView2.Columns["nOT4"].VisibleIndex = i++;
            this.gridView2.Columns["nOT5"].VisibleIndex = i++;

            this.gridView2.Columns["cQnResType"].Caption = "MACHINE TYPE";
            this.gridView2.Columns["cQcResource"].Caption = "MACHINE CODE";
            this.gridView2.Columns["cQnResource"].Caption = "MACHINE NAME";
            this.gridView2.Columns["cQnUM"].Caption = "UNIT";
            this.gridView2.Columns["nCap1Hour"].Caption = "PRESS";
            this.gridView2.Columns["nCap1Hour2"].Caption = "PRESS/HR.";
            this.gridView2.Columns["nCap1Day"].Caption = "PRESS/DAY";
            //this.gridView2.Columns["nResCount"].Caption = "จำนวนเครื่อง\r\n(C)";
            //this.gridView2.Columns["nCap1Day2"].Caption = "วัน\r\n(E=BxC)";
            this.gridView2.Columns["nOT1"].Caption = "OT 18.00";
            this.gridView2.Columns["nOT2"].Caption = "OT 19.00";
            this.gridView2.Columns["nOT3"].Caption = "OT 20.00";
            this.gridView2.Columns["nOT4"].Caption = "OT 21.00";
            this.gridView2.Columns["nOT5"].Caption = "OT 22.00";

            this.gridView2.Columns["nCap1Hour"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nCap1Hour"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nCap1Hour"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["cQnUM"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["cQnUM"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["cQnUM"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["nOT1"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nOT1"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nOT1"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["nOT2"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nOT2"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nOT2"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["nOT3"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nOT3"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nOT3"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["nOT4"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nOT4"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nOT4"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["nOT5"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView2.Columns["nOT5"].OptionsColumn.AllowFocus = true;
            this.gridView2.Columns["nOT5"].OptionsColumn.ReadOnly = false;

            this.gridView2.Columns["cQnResType"].Width = 90;
            //this.gridView2.Columns["nRecNo"].Width = 50;
            //this.gridView2.Columns["cQcMachine"].Width = 120;
            //this.gridView2.Columns["cQnMachine"].Width = 250;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.grcQcMachine.Buttons[0].Tag = "GRDVIEW2_CQCMACHINE";

            //this.grcQcMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Code);
            //this.grcQnMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Name);
            //this.grcRemark.MaxLength = 150;

            ////this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            //this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            //this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            //this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            //this.gridView2.Columns["cQnMachine"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            //this.gridView2.Columns["cQnMachine"].OptionsColumn.AllowFocus = false;
            //this.gridView2.Columns["cQnMachine"].OptionsColumn.ReadOnly = true;

            //this.gridView2.Columns["cQcMachine"].ColumnEdit = this.grcQcMachine;
            //this.gridView2.Columns["cQnMachine"].ColumnEdit = this.grcQnMachine;
            //this.gridView2.Columns["cRemark1"].ColumnEdit = this.grcRemark;

            this.gridView2.Columns["nCap1Hour"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nOT1"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT1"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT1"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT2"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT3"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT4"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nOT5"].ColumnEdit = this.grcCapQty;

            this.gridView2.Columns["nCap1Day"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nResCount"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nCap1Hour2"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nCap1Day2"].ColumnEdit = this.grcCapQty;

            //this.pmCalcColWidth();
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
                case "SECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new DialogForms.dlgGetSect();
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WKCTR":
                    if (this.pofrmGetWkCtr == null)
                    {
                        this.pofrmGetWkCtr = new frmMFWorkCenter(FormActiveMode.PopUp);
                        this.pofrmGetWkCtr.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWkCtr.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCWKCTR":
                case "TXTQNWKCTR":
                    this.pmInitPopUpDialog("WKCTR");
                    strPrefix = (inTextbox == "TXTQCWKCTR" ? "CCODE" : "CNAME");
                    this.pofrmGetWkCtr.ValidateField("", strPrefix, true);
                    if (this.pofrmGetWkCtr.PopUpResult)
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

                case "TXTQCWKCTR":
                case "TXTQNWKCTR":

                    dtrGetVal = this.pofrmGetWkCtr.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        if (this.txtQcWkCtr.Tag.ToString() != dtrGetVal["cRowID"].ToString())
                        {
                            this.txtQcWkCtr.Tag = dtrGetVal["cRowID"].ToString();
                            this.pmGenCapDetail();
                        }
                        this.txtQcWkCtr.Text = dtrGetVal["cCode"].ToString().TrimEnd();
                        this.txtQnWkCtr.Text = dtrGetVal["cName"].ToString().TrimEnd();

                    }
                    else
                    {
                        this.txtQcWkCtr.Tag = "";
                        this.txtQcWkCtr.Text = "";
                        this.txtQnWkCtr.Text = "";
                        this.pmClearCapDetail();
                    }
                    break;

            }
        }

        private void pmGenCapDetail()
        {
            this.pmClearCapDetail();
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select MFRESTYPE.CROWID as cResType, MFRESTYPE.CNAME as QnResType, MFRESOURCE.CROWID as cResource, MFRESOURCE.CCODE as QcResource, MFRESOURCE.CNAME as QnResource from MFWKCTRIT_RES";
            strSQLExec += " left join MFRESOURCE on  MFRESOURCE.CROWID = MFWKCTRIT_RES.CRESOURCE";
            strSQLExec += " left join MFRESTYPE on  MFRESTYPE.CROWID = MFRESOURCE.CMCTYPE";
            strSQLExec += " where MFWKCTRIT_RES.CWKCTRH = ?";
            strSQLExec += " order by MFWKCTRIT_RES.CTYPE ,MFWKCTRIT_RES.CSEQ";

            pobjSQLUtil.SetPara(new object[] { this.txtQcWkCtr.Tag.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGenCap", "MFMCType", strSQLExec, ref strErrorMsg))
            {
                foreach (DataRow dtrCap in this.dtsDataEnv.Tables["QGenCap"].Rows)
                {
                    DataRow dtrTemCap = this.dtsDataEnv.Tables[this.mstrTemCap].NewRow();
                    dtrTemCap["cResource"] = dtrCap["cResource"].ToString().TrimEnd();
                    dtrTemCap["cQcResource"] = dtrCap["QcResource"].ToString().TrimEnd();
                    dtrTemCap["cQnResource"] = dtrCap["QnResource"].ToString().TrimEnd();
                    dtrTemCap["cResType"] = dtrCap["cResType"].ToString().TrimEnd();
                    dtrTemCap["cQnResType"] = dtrCap["QnResType"].ToString().TrimEnd();
                    dtrTemCap["nResCount"] = 1;
                    this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Add(dtrTemCap);
                }
            }

        }

        private void xxxpmGenCapDetail()
        {
            this.pmClearCapDetail();
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select MFRESTYPE.CNAME as QnResType,count(*) as ResCnt from MFWKCTRIT_RES";
            strSQLExec += " left join MFRESOURCE on  MFRESOURCE.CROWID = MFWKCTRIT_RES.CRESOURCE";
            strSQLExec += " left join MFRESTYPE on  MFRESTYPE.CROWID = MFRESOURCE.CMCTYPE";
            strSQLExec += " where MFWKCTRIT_RES.CWKCTRH = ?";
            strSQLExec += " group by MFRESTYPE.CNAME";

            pobjSQLUtil.SetPara(new object[] { this.txtQcWkCtr.Tag.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGenCap", "MFMCType", strSQLExec, ref strErrorMsg))
            {
                foreach (DataRow dtrCap in this.dtsDataEnv.Tables["QGenCap"].Rows)
                {
                    DataRow dtrTemCap = this.dtsDataEnv.Tables[this.mstrTemCap].NewRow();
                    dtrTemCap["cQnResType"] = dtrCap["QnResType"].ToString().TrimEnd();
                    dtrTemCap["nResCount"] = Convert.ToDecimal(dtrCap["ResCnt"]);
                    this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Add(dtrTemCap);
                }
            }

        }

        private void pmClearCapDetail()
        {
            this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Clear();
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

        private void txtQcWkCtr_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCWKCTR" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcWkCtr.Tag = "";
                this.txtQcWkCtr.Text = "";
                this.txtQnWkCtr.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WKCTR");
                e.Cancel = !this.pofrmGetWkCtr.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWkCtr.PopUpResult)
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
            string strDeleteDesc = "(" + dtrBrow["QCSECT"].ToString().TrimEnd() + ") " + dtrBrow["QCWKCTR"].ToString().TrimEnd();
            //if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFPDCapInfo.Field.Code].ToString(), ref strErrorMsg))
            if (true)
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, ref strErrorMsg))
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

        private bool pmDeleteRow(string inRowID, ref string ioErrorMsg)
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
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cPdCapH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                //KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

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

            if (this.txtQcSect.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code", this.mstrFormMenuName + " Code is not define ! " });
                this.txtQcSect.Focus();
                return false;
            }
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
            //    && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrType, this.txtCode.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = UIBase.GetAppUIText(new string[] { this.mstrFormMenuName + " ซ้ำ", "Duplicate " + this.mstrFormMenuName + " !" });
            //    this.txtCode.Focus();
            //    return false;
            //}
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "ชื่อประเภทเครื่องจักรซ้ำ";
            //    this.txtName.Focus();
            //    return false;
            //}
            else
                bllResult = true;

            return bllResult;
        }

        //private bool pmRunCode()
        //{
        //    //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranchID }, this.txtCode.MaxLength);

        //    string strErrorMsg = "";
        //    string strLastRunCode = "";
        //    int intCodeLen = 0;
        //    int intRunCode = 1;
        //    int inMaxLength = this.txtCode.Properties.MaxLength;
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrType });
        //    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType =? and cCode < ':' order by cCode desc", ref strErrorMsg))
        //    {
        //        strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QMFPDCapInfo.Field.Code].ToString().Trim();
        //        try
        //        {
        //            intRunCode = Convert.ToInt32(strLastRunCode) + 1;
        //        }
        //        catch (Exception ex)
        //        {
        //            strErrorMsg = ex.Message.ToString();
        //            intRunCode++;
        //        }
        //    }
        //    intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : inMaxLength);
        //    this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));

        //    return true;
        //}

        //private bool pmIsValidateCode(object[] inPrefixPara)
        //{
        //    bool bllResult = true;
        //    string strErrorMsg = "";
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    if (objSQLHelper.SetPara(inPrefixPara)
        //        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cCode = ?", ref strErrorMsg))
        //    {
        //        bllResult = false;
        //    }
        //    return bllResult;
        //}

        //private bool pmIsValidateName(object[] inPrefixPara)
        //{
        //    bool bllResult = true;
        //    string strErrorMsg = "";
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    if (objSQLHelper.SetPara(inPrefixPara)
        //        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cName = ?", ref strErrorMsg))
        //    {
        //        bllResult = false;
        //    }
        //    return bllResult;
        //}

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

            dtrSaveInfo[QMFPDCapInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFPDCapInfo.Field.SectID] = this.txtQcSect.Tag.ToString();
            dtrSaveInfo[QMFPDCapInfo.Field.WKCtrH] = this.txtQcWkCtr.Tag.ToString();
            dtrSaveInfo[QMFPDCapInfo.Field.WKHour] = Convert.ToDecimal(this.txtWkHour.Value);

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

                this.pmUpdatePdCapIT();

                this.mdbTran.Commit();
                bllIsCommit = true;

                //if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                //{
                //    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                //}
                //else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                //{
                //    if (this.mstrOldCode == this.txtCode.Text && this.mstrOldName == this.txtName.Text)
                //    {
                //        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                //    }
                //    else
                //    {
                //        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
                //    }
                //}

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

        private void pmUpdatePdCapIT()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemCap].Rows)
            {

                bool bllIsNewRow = false;

                DataRow dtrPdCapIT = null;
                if (dtrTemPd["cResource"].ToString().TrimEnd() != string.Empty)
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        //| (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                        || (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrPdCapIT = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrPdCapIT["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrPdCapIT["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrPdCapIT = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    this.pmReplRecordPdCapIT(bllIsNewRow, dtrTemPd, ref dtrPdCapIT);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrPdCapIT, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
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

        private bool pmReplRecordPdCapIT(bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrPdCapIT = ioRefProd;
            DataRow dtrPdCapHD = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrPdCapIT["cCorp"] = App.ActiveCorp.RowID;
            dtrPdCapIT["cPdCapH"] = dtrPdCapHD["cRowID"].ToString();
            dtrPdCapIT["cSect"] = dtrPdCapHD["cSect"].ToString();
            dtrPdCapIT["cResType"] = inTemPd["cResType"].ToString();
            dtrPdCapIT["cResource"] = inTemPd["cResource"].ToString();
            //dtrPdCapIT["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrPdCapIT["NCAP1HOUR"] = Convert.ToDecimal(inTemPd["NCAP1HOUR"]);
            dtrPdCapIT["CQNUM"] = inTemPd["cQnUM"].ToString().TrimEnd();
            dtrPdCapIT["NCAP1DAY"] = Convert.ToDecimal(inTemPd["NCAP1DAY"]);
            dtrPdCapIT["NRESCOUNT"] = Convert.ToDecimal(inTemPd["NRESCOUNT"]);
            dtrPdCapIT["NOT1"] = Convert.ToDecimal(inTemPd["NOT1"]);
            dtrPdCapIT["NOT2"] = Convert.ToDecimal(inTemPd["NOT2"]);
            dtrPdCapIT["NOT3"] = Convert.ToDecimal(inTemPd["NOT3"]);
            dtrPdCapIT["NOT4"] = Convert.ToDecimal(inTemPd["NOT4"]);
            dtrPdCapIT["NOT5"] = Convert.ToDecimal(inTemPd["NOT5"]);

            //dtrPdCapIT["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            this.txtQcSect.Focus();
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
            this.txtQcSect.Tag = "";
            this.txtQcSect.Text = "";
            this.txtQnSect.Text = "";
            this.txtQcWkCtr.Tag = "";
            this.txtQcWkCtr.Text = "";
            this.txtQnWkCtr.Text = "";
            this.txtWkHour.Value = 6;

            this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Clear();
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
                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
                {
                
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
                    DataRow dtrRetVal = null;

                    this.txtQcSect.Tag = dtrLoadForm[QMFPDCapInfo.Field.SectID].ToString();
                    pobjSQLUtil2.SetPara(new object[] { this.txtQcSect.Tag.ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefTo", QEMSectInfo.TableName, "select * from " + QEMSectInfo.TableName + " where " + QEMSectInfo.Field.RowID + " = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcSect.Text = dtrRetVal[QEMSectInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnSect.Text = dtrRetVal[QEMSectInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.txtQcWkCtr.Tag = dtrLoadForm[QMFPDCapInfo.Field.WKCtrH].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcWkCtr.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", QMFWorkCenterInfo.TableName, "select * from " + QMFWorkCenterInfo.TableName + " where " + QMFWorkCenterInfo.Field.RowID + " = ?", ref strErrorMsg))
                    {
                        dtrRetVal = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        this.txtQcWkCtr.Text = dtrRetVal[QMFWorkCenterInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnWkCtr.Text = dtrRetVal[QMFWorkCenterInfo.Field.Name].ToString().TrimEnd();
                    }

                    this.txtWkHour.Value = Convert.ToDecimal(dtrLoadForm[QMFPDCapInfo.Field.WKHour]);
                    this.pmLoadWkCtrIT("TemMachine");
                    this.pmLoadWkCtrIT("TemTool");

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            //this.mstrOldCode = this.txtCode.Text;
            //this.mstrOldName = this.txtName.Text;
        }

        private void pmLoadWkCtrIT(string inAlias)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            string strType = (inAlias == "TemMachine" ? SysDef.gc_RESOURCE_TYPE_MACHINE : SysDef.gc_RESOURCE_TYPE_TOOL);
            pobjSQLUtil.SetPara(new object[] { this.txtQcWkCtr.Tag.ToString(), strType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWkCtrIT", "WkCtrIT", "select * from " + MapTable.Table.MFWorkCenterItem_Res + " where cWkCtrH = ? and cType = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrWkCtrIT in this.dtsDataEnv.Tables["QWkCtrIT"].Rows)
                {


                    pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, dtrWkCtrIT["cResource"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdCapIT", this.mstrITable, "select * from " + this.mstrITable + " where cPdCapH = ? and cResource = ? ", ref strErrorMsg))
                    {

                        DataRow dtrPdCapIT = this.dtsDataEnv.Tables["QPdCapIT"].Rows[0];
                        DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemCap].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Add(dtrNewRow);

                        intRow++;
                        //dtrNewRow["nRecNo"] = intRow;
                        dtrNewRow["cRowID"] = dtrPdCapIT["cRowID"].ToString();
                        dtrNewRow["cResource"] = dtrPdCapIT["cResource"].ToString();
                        dtrNewRow["nCap1Hour"] = Convert.ToDecimal(dtrPdCapIT["nCap1Hour"]);
                        dtrNewRow["cQnUM"] = dtrPdCapIT["cQnUM"].ToString();
                        dtrNewRow["nCap1Day"] = Convert.ToDecimal(dtrPdCapIT["nCap1Day"]);
                        dtrNewRow["nResCount"] = Convert.ToDecimal(dtrPdCapIT["nResCount"]);
                        dtrNewRow["nOT1"] = Convert.ToDecimal(dtrPdCapIT["nOT1"]);
                        dtrNewRow["nOT2"] = Convert.ToDecimal(dtrPdCapIT["nOT2"]);
                        dtrNewRow["nOT3"] = Convert.ToDecimal(dtrPdCapIT["nOT3"]);
                        dtrNewRow["nOT4"] = Convert.ToDecimal(dtrPdCapIT["nOT4"]);
                        dtrNewRow["nOT5"] = Convert.ToDecimal(dtrPdCapIT["nOT5"]);

                        decimal decPress = Convert.ToDecimal(dtrNewRow["nCap1Hour"]);
                        dtrNewRow["nCap1Hour2"] = (decPress != 0 ? 3600 / decPress : 0);
                        dtrNewRow["nCap1Day"] = Convert.ToDecimal(dtrNewRow["nCap1Hour2"]) * this.txtWkHour.Value;
                        //dtrNewRow["nCap1Day2"] = Convert.ToDecimal(dtrNewRow["nCap1Day"]) * Convert.ToDecimal(dtrNewRow["nResCount"]);

                        pobjSQLUtil.SetPara(new object[] { dtrNewRow["cResource"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFResourceInfo.TableName, "select * from " + QMFResourceInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            dtrNewRow["cQcResource"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Code].ToString().TrimEnd();
                            dtrNewRow["cQnResource"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.Name].ToString().TrimEnd();
                            dtrNewRow["cResType"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResourceInfo.Field.MCType].ToString();
                            pobjSQLUtil.SetPara(new object[] { dtrNewRow["cResType"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRes", QMFResTypeInfo.TableName, "select * from " + QMFResTypeInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                            {
                                dtrNewRow["cQnResType"] = this.dtsDataEnv.Tables["QRes"].Rows[0][QMFResTypeInfo.Field.Name].ToString().TrimEnd();
                            }
                        }

                    }

                }
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

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                DataRow dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemCap].Rows[e.RowHandle];

                decimal decPress = Convert.ToDecimal(dtrTemPd["nCap1Hour"]);
                dtrTemPd["nCap1Hour2"] = (decPress != 0 ? 3600 / decPress : 0);
                dtrTemPd["nCap1Day"] = Convert.ToDecimal(dtrTemPd["nCap1Hour2"]) * this.txtWkHour.Value;

                //dtrTemPd["nCap1Day"] = Convert.ToDecimal(dtrTemPd["nCap1Hour"]) * this.txtWkHour.Value;
                //dtrTemPd["nCap1Hour2"] = Convert.ToDecimal(dtrTemPd["nCap1Hour"]) * Convert.ToDecimal(dtrTemPd["nResCount"]);
                //dtrTemPd["nCap1Day2"] = Convert.ToDecimal(dtrTemPd["nCap1Day"]) * Convert.ToDecimal(dtrTemPd["nResCount"]);
            }
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

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        private void btnFillOT_Click(object sender, EventArgs e)
        {
            foreach (DataRow dtrTemCap in this.dtsDataEnv.Tables[this.mstrTemCap].Rows)
            {
                dtrTemCap["nOT1"] = Convert.ToDecimal(dtrTemCap["nCap1Hour2"]) * (this.txtWkHour.Value + Convert.ToDecimal(0.5));
                dtrTemCap["nOT2"] = Convert.ToDecimal(dtrTemCap["nCap1Hour2"]) * (this.txtWkHour.Value + Convert.ToDecimal(1.5));
                dtrTemCap["nOT3"] = Convert.ToDecimal(dtrTemCap["nCap1Hour2"]) * (this.txtWkHour.Value + Convert.ToDecimal(2.5));
                dtrTemCap["nOT4"] = Convert.ToDecimal(dtrTemCap["nCap1Hour2"]) * (this.txtWkHour.Value + Convert.ToDecimal(3.5));
                dtrTemCap["nOT5"] = Convert.ToDecimal(dtrTemCap["nCap1Hour2"]) * (this.txtWkHour.Value + Convert.ToDecimal(4.5));
            }
        }

    }
}
