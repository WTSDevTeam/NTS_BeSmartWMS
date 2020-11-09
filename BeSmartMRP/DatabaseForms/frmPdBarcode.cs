
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


namespace BeSmartMRP.DatabaseForms
{
    public partial class frmPdBarcode : UIHelper.frmBase, UIHelper.IfrmDBBase
    {
        
        public static string TASKNAME = "EPDBARCODE";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QEMPDBarcodeInfo.TableName;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QEMPDBarcodeInfo.Field.Code;

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

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

        private DatabaseForms.frmEMProj pofrmGetProj = null;

        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetUM pofrmGetUM = null;

        ArrayList paTxtStdUM = new ArrayList();

        public frmPdBarcode()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmPdBarcode(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmPdBarcode mInstanse = null;

        public static frmPdBarcode GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmPdBarcode();
            }
            return mInstanse;
        }

        private static void pmClearInstanse()
        {
            if (mInstanse != null)
            {
                mInstanse = null;
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

            #region "Add Standard UM"
            this.paTxtStdUM.Add(this.txtQnStdUm1);
            this.paTxtStdUM.Add(this.txtQnStdUm2);
            this.paTxtStdUM.Add(this.txtQnStdUm3);
            this.paTxtStdUM.Add(this.txtQnStdUm4);
            #endregion

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPDBarcodeInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPDBarcodeInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPDBarcodeInfo.Field.Name2);

        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //string strSQLExec = "select PDBARCODE.FCSKID, PDBARCODE.FCCODE, PDBARCODE.FCNAME, PDBARCODE.FCNAME2, PDBARCODE.DCREATE, PDBARCODE.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} PDBARCODE ";
            //strSQLExec += " left join {1} EM1 ON EM1.fcSkid = PDBARCODE.CCREATEBY ";
            //strSQLExec += " left join {1} EM2 ON EM2.fcSkid = PDBARCODE.CLASTUPDBY ";
            string strSQLExec = "select PDBARCODE.FCSKID, PDBARCODE.FCCODE, PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD, PDBARCODE.FTLASTUPD as FDCREATE, PDBARCODE.FTDATETIME as FDLASTUPDBY from {0} PDBARCODE ";
            strSQLExec += " left join PROD on PDBARCODE.FCPROD = PROD.FCSKID ";
            strSQLExec += " where PDBARCODE.FCCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            pobjSQLUtil.SetPara(new object[] {App.ActiveCorp.RowID});
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "PDBARCODE", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QEMPDBarcodeInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns[QEMPDBarcodeInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;
            this.gridView1.Columns["QNPROD"].VisibleIndex = i++;

            this.gridView1.Columns[QEMPDBarcodeInfo.Field.Code].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BarCode", "BarCode" });
            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "Product Code" });
            this.gridView1.Columns["QNPROD"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "Product Name" });

            this.gridView1.Columns[QEMPDBarcodeInfo.Field.Code].Width = 15;
            this.gridView1.Columns["QCPROD"].Width = 25;

            this.pmSetSortKey(QEMPDBarcodeInfo.Field.Code, true);
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
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Print.ToString()].Enabled = (inActivePage == 0 ? true : false);

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
                case "PROJ":
                    if (this.pofrmGetProj == null)
                    {
                        this.pofrmGetProj = new frmEMProj(FormActiveMode.PopUp);
                        this.pofrmGetProj.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProj.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "UM":
                    if (this.pofrmGetUM == null)
                    {
                        //this.pofrmGetUM = new DatabaseForms.frmUM(FormActiveMode.PopUp);
                        this.pofrmGetUM = new DialogForms.dlgGetUM();
                        this.pofrmGetUM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetUM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

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

                    case WsToolBar.Print:

                        this.pmPrintData();
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

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QEMPDBarcodeInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QEMPDBarcodeInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QEMPDBarcodeInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลBarCode : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QEMPDBarcodeInfo.Field.Code].ToString(), dtrBrow[QEMPDBarcodeInfo.Field.Name].ToString(), ref strErrorMsg))
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
            //TODO: Check Corp Has Used
            return false;
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
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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
                    //App.mSave_hWnd = App.GetForegroundWindow();
                    string strRPTFileName = "";
                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, "");
                }
            }
        }

        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName)
        {
            string strErrorMsg = "";

            Report.RPT.dtsPBarCode dtsPrintPreview = new Report.RPT.dtsPBarCode();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //object[] pAPara = new object[3] { App.ActiveCorp.CorpID, inBegCode, inEndCode};

            //Report.Agents.PrintField oPrintField = new Report.Agents.PrintField(pobjSQLUtil, pobjSQLUtil2, App.ActiveCorp);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrRefTable + " where fcCorp = ? and fcCode between ? and ? order by fcCode";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inBegCode, inEndCode });

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPrn", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPFormH in this.dtsDataEnv.Tables["QPrn"].Rows)
                {

                    DataRow dtrLastRow = dtsPrintPreview.DTSBARCODE.NewRow();
                    //จำนวน
                    dtrLastRow["Barcode"] = dtrPFormH["fcCode"].ToString().Trim();
                    //มูลค่า
                    pobjSQLUtil.SetPara(new object[] { dtrPFormH[QEMPDBarcodeInfo.Field.ProdID].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
                    {
                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        dtrLastRow["QcProd"] = dtrProd[QEMWHouseInfo.Field.Code].ToString().TrimEnd();
                        dtrLastRow["QnProd"] = dtrProd[QEMWHouseInfo.Field.Name].ToString().TrimEnd();
                    }

                    dtsPrintPreview.DTSBARCODE.Rows.Add(dtrLastRow);

                }

                pmPreviewReport(dtsPrintPreview,"");
            }
        }


        //ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName)
        {
            Report.RPT.xrPdBarCode01 oprn = new Report.RPT.xrPdBarCode01();
            //oprn.LoadLayout(
            oprn.DataSource = inData;
            oprn.CreateDocument();
            frmDXPreviewReport oPreview = new frmDXPreviewReport();

            oPreview.printControl1.PrintingSystem = oprn.PrintingSystem;

            oPreview.Show();

            //DevExpress.XtraReports.for
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

        private void pmSaveData()
        {
            string strErrorMsg = "";
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");
                if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                    this.pmGotoBrowPage();
                else
                    this.pmInsertLoop();

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
            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                if (MessageBox.Show("ยังไม่ได้ระบุรหัส Barcode ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสBarCode";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtQcProd.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุสินค้า";
                this.txtQcProd.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID , this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสBarCodeซ้ำ";
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
            int intRunCode = 1;
            int inMaxLength = this.txtCode.Properties.MaxLength;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] {App.ActiveCorp.RowID});
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QEMPDBarcodeInfo.Field.Code].ToString().Trim();
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
                && !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcSkid from " + this.mstrRefTable + " where fcSkid = ?", ref strErrorMsg)))
            {
                bllIsNewRow = true;
                if (this.mstrEditRowID == string.Empty)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
                }
                //dtrSaveInfo[MapTable.ShareField.CreateBy] = App.FMAppUserID;
                //dtrSaveInfo[MapTable.ShareField.CreateDate] = objSQLHelper.GetDBServerDateTime();
                dtrSaveInfo["FTLASTUPD"] = objSQLHelper.GetDBServerDateTime();
            }

            // Control Field ที่ทุก Form ต้องมี
            //dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            //dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            //dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            dtrSaveInfo[MapTable.ShareField.fcSkid] = this.mstrEditRowID;
            dtrSaveInfo["FTLASTUPD"] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QEMPDBarcodeInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QEMPDBarcodeInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QEMPDBarcodeInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QEMPDBarcodeInfo.Field.Name2] = this.txtName2.Text.TrimEnd();

            dtrSaveInfo[QEMPDBarcodeInfo.Field.ProdID] = this.txtQcProd.Tag.ToString();

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "fcSkid", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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

        private void pmLoadEditPage()
        {
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
            this.txtStoreSize.Value = 0;

            this.txtQcProd.Tag = "";
            this.txtQcProd.Text = "";
            this.txtQnProd.Text = "";

            //Page Edit 2
            //this.txtQcUM.Enabled = true;
            //this.txtQcStUm.Enabled = true;
            //this.txtQnUM.Enabled = true;
            //this.txtQnStUm.Enabled = true;

            this.txtQcUM.Tag = ""; this.txtQcUM.Text = ""; this.txtQnUM.Text = "";
            this.txtQcUM1.Tag = ""; this.txtQcUM1.Text = ""; this.txtQnUM1.Text = "";
            this.txtQcUM2.Tag = ""; this.txtQcUM2.Text = ""; this.txtQnUM2.Text = "";
            this.txtQcStUm.Tag = ""; this.txtQcStUm.Text = ""; this.txtQnStUm.Text = "";
            this.txtQcStUm1.Tag = ""; this.txtQcStUm1.Text = ""; this.txtQnStUm1.Text = "";
            this.txtQcStUm2.Tag = ""; this.txtQcStUm2.Text = ""; this.txtQnStUm2.Text = "";

            this.txtUm1Qty.Value = 1; this.txtUm2Qty.Value = 1;
            this.txtStUm1Qty.Value = 1; this.txtStUm2Qty.Value = 1;

        }

        private void pmLoadFormData()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ?", ref strErrorMsg))
                {

                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
                    this.txtCode.Text = dtrLoadForm[QEMPDBarcodeInfo.Field.Code].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm[QEMPDBarcodeInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QEMPDBarcodeInfo.Field.Name2].ToString().TrimEnd();

                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm[QEMPDBarcodeInfo.Field.ProdID].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
                    {
                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        this.txtQcProd.Tag = dtrProd["fcSkid"].ToString();
                        this.txtQcProd.Text = dtrProd[QEMWHouseInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnProd.Text = dtrProd[QEMWHouseInfo.Field.Name].ToString().TrimEnd();
                    }

                    #region "Load UM"

                    bool bllHasUsed = this.pmHasUsedChild1Corp(this.mstrEditRowID, ref strErrorMsg);
                    if (bllHasUsed)
                    {
                        //ioErrorMsg = UIBase.GetAppUIText(new string[] { "ไม่สามารถแก้ไขหน่วยนับมาตรฐานได้เนื่องจากมีการอ้างอิงถึงใน ", "Can not delete because has refer to " }) + ioErrorMsg;
                        this.txtQcUM.Enabled = false;
                        this.txtQcStUm.Enabled = false;
                        this.txtQnUM.Enabled = false;
                        this.txtQnStUm.Enabled = false;
                    }

                    this.txtUm1Qty.Value = Convert.ToDecimal(dtrLoadForm["fnUmQty1"]);
                    this.txtUm2Qty.Value = Convert.ToDecimal(dtrLoadForm["fnUmQty2"]);
                    this.txtStUm1Qty.Value = Convert.ToDecimal(dtrLoadForm["fnStUmQty1"]);
                    this.txtStUm2Qty.Value = Convert.ToDecimal(dtrLoadForm["fnStUmQty2"]);

                    this.txtQcUM.Tag = dtrLoadForm["fcUM"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcUM"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcUM.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnUM.Text = dtrUM["fcName"].ToString().TrimEnd();

                        this.pmSetStdQnUM(this.txtQcUM.Tag.ToString(), this.txtQnUM.Text);

                    }

                    this.txtQcUM1.Tag = dtrLoadForm["fcUM1"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcUM1"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcUM1.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnUM1.Text = dtrUM["fcName"].ToString().TrimEnd();
                    }

                    this.txtQcUM2.Tag = dtrLoadForm["fcUM2"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcUM2"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcUM2.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnUM2.Text = dtrUM["fcName"].ToString().TrimEnd();
                    }

                    this.txtQcStUm.Tag = dtrLoadForm["fcStUm"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcStUm"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcStUm.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnStUm.Text = dtrUM["fcName"].ToString().TrimEnd();
                    }

                    this.txtQcStUm1.Tag = dtrLoadForm["fcStUm1"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcStUm1"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcStUm1.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnStUm1.Text = dtrUM["fcName"].ToString().TrimEnd();
                    }

                    this.txtQcStUm2.Tag = dtrLoadForm["fcStUm2"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcStUm2"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        this.txtQcStUm2.Text = dtrUM["fcCode"].ToString().TrimEnd();
                        this.txtQnStUm2.Text = dtrUM["fcName"].ToString().TrimEnd();
                    }

                    #endregion

                }
                this.pmLoadOldVar();
            }
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
                //this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                //this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text = "สร้างโดยวันที่ : " + Convert.ToDateTime(dtrBrow["fdCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดวันที่ : " + Convert.ToDateTime(dtrBrow["fdLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");
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
            if (inOrderBy.ToUpper() == QEMPDBarcodeInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMPDBarcodeInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["fcSkid"].ToString() });
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
                case "TXTQCPROD":
                case "TXTQCWHLOCA2":
                case "TXTQCWHLOCA3":
                    this.pmInitPopUpDialog("WHLOCA");
                    //strPrefix = (inTextbox == "TXTQCPROJ" ? "CCODE" : "CNAME");
                    strPrefix = (inTextbox.StartsWith("TXTQCPROD") ? "FCCODE" : "FCNAME");
                    this.pofrmGetProd.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCUM":
                case "TXTQNUM":
                case "TXTQCUM1":
                case "TXTQNUM1":
                case "TXTQCUM2":
                case "TXTQNUM2":
                case "TXTQCSTUM":
                case "TXTQNSTUM":
                case "TXTQCSTUM1":
                case "TXTQNSTUM1":
                case "TXTQCSTUM2":
                case "TXTQNSTUM2":

                case "TXTQNPRICE1":
                case "TXTQNPRICE2":
                case "TXTQNPRICE3":
                case "TXTQNPRICE4":
                case "TXTQNPRICE5":
                case "TXTQNPRICEA1":
                case "TXTQNPRICEA2":
                case "TXTQNPRICEA3":
                case "TXTQNPRICEA4":
                case "TXTQNPRICEA5":
                case "TXTQNPRICEB1":
                case "TXTQNPRICEB2":
                case "TXTQNPRICEB3":
                case "TXTQNPRICEB4":
                case "TXTQNPRICEB5":
                case "TXTQNPRICEC1":
                case "TXTQNPRICEC2":
                case "TXTQNPRICEC3":
                case "TXTQNPRICEC4":
                case "TXTQNPRICEC5":
                case "TXTQNPRICED1":
                case "TXTQNPRICED2":
                case "TXTQNPRICED3":
                case "TXTQNPRICED4":
                case "TXTQNPRICED5":

                    string strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
                    this.pmInitPopUpDialog("UM");
                    //this.pofrmGetUM.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME"), true);
                    this.pofrmGetUM.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetUM.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrTemPd = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "TXTQCPROD":
                case "TXTQNPROD":

                    if (this.pofrmGetProd != null)
                    {
                        DataRow dtrJob = this.pofrmGetProd.RetrieveValue();
                        this.txtQcProd.Tag = dtrJob["fcSkid"].ToString();
                        this.txtQcProd.Text = dtrJob["fcCode"].ToString().TrimEnd();
                        this.txtQnProd.Text = dtrJob["fcName"].ToString().TrimEnd();

                        string strUOM = dtrJob["fcUm2"].ToString();
                        string strStkUOM = dtrJob["fcStUm"].ToString();
                        decimal decUOMQty = 1;
                        decimal decStkUOMQty = 1;

                        strUOM = (dtrJob["fcUm2"].ToString().TrimEnd() != string.Empty ? dtrJob["fcUm2"].ToString() : dtrJob["fcUm"].ToString());
                        strStkUOM = (dtrJob["fcUm2"].ToString().TrimEnd() != string.Empty ? dtrJob["fcStUm2"].ToString() : dtrJob["fcUm"].ToString());
                        decUOMQty = Convert.ToDecimal(dtrJob["fnUmQty2"]);
                        decStkUOMQty = Convert.ToDecimal(dtrJob["fnStUmQty2"]);

                        //string strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });
                        //pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
                        //pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "PDBARCODE", strSQLExec, ref strErrorMsg);

                        //string strUOM = strUOM;
                        //decimal decUOMQty  = decUOMQty;
                        string strUOMStd = dtrJob["fcUm"].ToString();
                        string strStUOM = strStkUOM;
                        string strStUOMStd = dtrJob["fcStUm"].ToString();
                        decimal decStUOMQty = decUOMQty;

                    }
                    else
                    {
                        this.txtQcProd.Tag = "";
                        this.txtQcProd.Text = "";
                        this.txtQnProd.Text = "";
                    }
                    break;

                case "TXTQCUM":
                case "TXTQNUM":
                case "TXTQCUM1":
                case "TXTQNUM1":
                case "TXTQCUM2":
                case "TXTQNUM2":
                case "TXTQCSTUM":
                case "TXTQNSTUM":
                case "TXTQCSTUM1":
                case "TXTQNSTUM1":
                case "TXTQCSTUM2":
                case "TXTQNSTUM2":
                case "TXTQNPRICE1":
                case "TXTQNPRICE2":
                case "TXTQNPRICE3":
                case "TXTQNPRICE4":
                case "TXTQNPRICE5":
                case "TXTQNPRICEA1":
                case "TXTQNPRICEA2":
                case "TXTQNPRICEA3":
                case "TXTQNPRICEA4":
                case "TXTQNPRICEA5":
                case "TXTQNPRICEB1":
                case "TXTQNPRICEB2":
                case "TXTQNPRICEB3":
                case "TXTQNPRICEB4":
                case "TXTQNPRICEB5":
                case "TXTQNPRICEC1":
                case "TXTQNPRICEC2":
                case "TXTQNPRICEC3":
                case "TXTQNPRICEC4":
                case "TXTQNPRICEC5":
                case "TXTQNPRICED1":
                case "TXTQNPRICED2":
                case "TXTQNPRICED3":
                case "TXTQNPRICED4":
                case "TXTQNPRICED5":
                    if (this.pofrmGetUM != null)
                    {
                        DataRow dtrUM = this.pofrmGetUM.RetrieveValue();

                        if (dtrUM != null)
                        {
                            //string strUM = dtrUM["cRowID"].ToString();
                            //string strQcUm = dtrUM["cCode"].ToString().TrimEnd();
                            //string strQnUm = dtrUM["cName"].ToString().TrimEnd();

                            string strUM = dtrUM["fcSkid"].ToString();
                            string strQcUm = dtrUM["fcCode"].ToString().TrimEnd();
                            string strQnUm = dtrUM["fcName"].ToString().TrimEnd();

                            switch (inPopupForm.TrimEnd().ToUpper())
                            {

                                #region "Page Edit2"
                                case "TXTQCUM":
                                case "TXTQNUM":
                                    this.txtQcUM.Tag = strUM;
                                    this.txtQcUM.Text = strQcUm;
                                    this.txtQnUM.Text = strQnUm;

                                    this.pmSetDefaUM();
                                    this.pmSetStdQnUM(strUM, strQnUm);

                                    break;
                                case "TXTQCUM1":
                                case "TXTQNUM1":
                                    this.txtQcUM1.Tag = strUM;
                                    this.txtQcUM1.Text = strQcUm;
                                    this.txtQnUM1.Text = strQnUm;
                                    break;
                                case "TXTQCUM2":
                                case "TXTQNUM2":
                                    this.txtQcUM2.Tag = strUM;
                                    this.txtQcUM2.Text = strQcUm;
                                    this.txtQnUM2.Text = strQnUm;
                                    break;
                                case "TXTQCSTUM":
                                case "TXTQNSTUM":
                                    this.txtQcStUm.Tag = strUM;
                                    this.txtQcStUm.Text = strQcUm;
                                    this.txtQnStUm.Text = strQnUm;
                                    break;
                                case "TXTQCSTUM1":
                                case "TXTQNSTUM1":
                                    this.txtQcStUm1.Tag = strUM;
                                    this.txtQcStUm1.Text = strQcUm;
                                    this.txtQnStUm1.Text = strQnUm;
                                    break;
                                case "TXTQCSTUM2":
                                case "TXTQNSTUM2":
                                    this.txtQcStUm2.Tag = strUM;
                                    this.txtQcStUm2.Text = strQcUm;
                                    this.txtQnStUm2.Text = strQnUm;
                                    break;
                                #endregion

                            }

                        }
                        else
                        {


                            switch (inPopupForm.TrimEnd().ToUpper())
                            {

                                #region "Page Edit2"
                                case "TXTQCUM":
                                case "TXTQNUM":
                                    this.txtQcUM.Tag = "";
                                    this.txtQcUM.Text = "";
                                    this.txtQnUM.Text = "";
                                    break;
                                case "TXTQCUM1":
                                case "TXTQNUM1":
                                    this.txtQcUM1.Tag = "";
                                    this.txtQcUM1.Text = "";
                                    this.txtQnUM1.Text = "";
                                    break;
                                case "TXTQCUM2":
                                case "TXTQNUM2":
                                    this.txtQcUM2.Tag = "";
                                    this.txtQcUM2.Text = "";
                                    this.txtQnUM2.Text = "";
                                    break;
                                case "TXTQCSTUM":
                                case "TXTQNSTUM":
                                    this.txtQcStUm.Tag = "";
                                    this.txtQcStUm.Text = "";
                                    this.txtQnStUm.Text = "";
                                    break;
                                case "TXTQCSTUM1":
                                case "TXTQNSTUM1":
                                    this.txtQcStUm1.Tag = "";
                                    this.txtQcStUm1.Text = "";
                                    this.txtQnStUm1.Text = "";
                                    break;
                                case "TXTQCSTUM2":
                                case "TXTQNSTUM2":
                                    this.txtQcStUm2.Tag = "";
                                    this.txtQcStUm2.Text = "";
                                    this.txtQnStUm2.Text = "";
                                    break;
                                #endregion


                            }

                        }
                    }
                    break;
            }
        }

        private void txtQcProd_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper().StartsWith("TXTQCPROD") ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                //this.txtQcProj.Tag = "";
                //this.txtQcProj.Text = "";
                //this.txtQnProj.Text = "";
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
        private bool pmHasUsedChild1Corp(string inRowID, ref string ioErrorMsg)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strRefMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.RefProd, "select GLRef.fcSkid, GLRef.fcCode, GLRef.fcRefNo from RefProd left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF where RefProd.fcProd = ?", ref strErrorMsg))
            {
                //string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    //strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                    strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
                }
                //ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
                ioErrorMsg = "Document : " + strRefMsg + "\r\n";
                return true;
            }

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.OrderI, "select OrderH.fcSkid, OrderH.fcCode, OrderH.fcRefNo from OrderI left join ORDERH on OrderH.FCSKID = OrderI.fcOrderH where OrderI.fcProd = ?", ref strErrorMsg))
            {
                //string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    //strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                    strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
                }
                //ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
                ioErrorMsg = "Document : " + strRefMsg + "\r\n";
                return true;
            }

            objSQLHelper2.SetPara(new object[] { inRowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.MFBOMHead, "select cCode, cName from " + MapTable.Table.MFBOMHead + " where cMfgProd = ?", ref strErrorMsg))
            {
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["cCode"].ToString().TrimEnd() + ") " + dtrChildTab["cName"].ToString().TrimEnd();
                }
                ioErrorMsg = "BOM : " + strRefMsg + "\r\n";
                return true;
            }

            objSQLHelper2.SetPara(new object[] { inRowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.MFBOMHead, "select MFBOMHD.cCode, MFBOMHD.cName from MFBOMIT_PD left join MFBOMHD on MFBOMHD.CROWID = MFBOMIT_PD.CBOMHD where MFBOMIT_PD.cProd = ?", ref strErrorMsg))
            {
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["cCode"].ToString().TrimEnd() + ") " + dtrChildTab["cName"].ToString().TrimEnd();
                }
                ioErrorMsg = "BOM : " + strRefMsg + "\r\n";
                return true;
            }

            objSQLHelper2.SetPara(new object[] { inRowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", "MFWORDERHD", "select MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERHD where MFWORDERHD.CMFGPROD = ? ", ref strErrorMsg))
            {
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["cRefNo"].ToString().TrimEnd() + ") " + " Date : " + Convert.ToDateTime(dtrChildTab["dDate"]).ToString("dd/MM/yy");
                }
                ioErrorMsg = "#MO " + strRefMsg + "\r\n";
                return true;
            }

            objSQLHelper2.SetPara(new object[] { inRowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", "MFWORDERHD", "select MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERIT_PD left join MFWORDERHD on MFWORDERHD.CROWID = MFWORDERIT_PD.CWORDERH where MFWORDERIT_PD.CPROD = ? ", ref strErrorMsg))
            {
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["cRefNo"].ToString().TrimEnd() + ") " + " Date : " + Convert.ToDateTime(dtrChildTab["dDate"]).ToString("dd/MM/yy");
                }
                ioErrorMsg = "#MO " + strRefMsg + "\r\n";
                return true;
            }

            return bllResult;
        }

        private void pmSetStdQnUM(string inUM, string inQnUmStd)
        {
            for (int intCnt = 0; intCnt < this.paTxtStdUM.Count; intCnt++)
            {
                DevExpress.XtraEditors.ButtonEdit oTxtQnUm = (DevExpress.XtraEditors.ButtonEdit)this.paTxtStdUM[intCnt];
                oTxtQnUm.Tag = inUM;
                oTxtQnUm.Text = inQnUmStd;
            }
        }

        private void pmSetDefaUM()
        {
            if (this.txtQcUM1.Text.Trim() == string.Empty)
            {
                this.txtUm1Qty.Value = 1;
                this.txtQcUM1.Tag = this.txtQcUM.Tag.ToString();
                this.txtQcUM1.Text = this.txtQcUM.Text;
                this.txtQnUM1.Text = this.txtQnUM.Text;
            }

            if (this.txtQcUM2.Text.Trim() == string.Empty)
            {
                this.txtUm2Qty.Value = 1;
                this.txtQcUM2.Tag = this.txtQcUM.Tag.ToString();
                this.txtQcUM2.Text = this.txtQcUM.Text;
                this.txtQnUM2.Text = this.txtQnUM.Text;
            }

            if (this.txtQcStUm.Text.Trim() == string.Empty)
            {
                this.txtQcStUm.Tag = this.txtQcUM.Tag.ToString();
                this.txtQcStUm.Text = this.txtQcUM.Text;
                this.txtQnStUm.Text = this.txtQnUM.Text;
            }

            if (this.txtQcStUm1.Text.Trim() == string.Empty)
            {
                this.txtStUm1Qty.Value = 1;
                this.txtQcStUm1.Tag = this.txtQcUM.Tag.ToString();
                this.txtQcStUm1.Text = this.txtQcUM.Text;
                this.txtQnStUm1.Text = this.txtQnUM.Text;
            }

            if (this.txtQcStUm2.Text.Trim() == string.Empty)
            {
                this.txtStUm2Qty.Value = 1;
                this.txtQcStUm2.Tag = this.txtQcUM.Tag.ToString();
                this.txtQcStUm2.Text = this.txtQcUM.Text;
                this.txtQnStUm2.Text = this.txtQnUM.Text;
            }

        }

    }

}
