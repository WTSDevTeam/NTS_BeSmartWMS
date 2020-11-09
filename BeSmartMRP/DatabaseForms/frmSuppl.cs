
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

namespace BeSmartMRP.DatabaseForms
{
    public partial class frmSuppl : UIHelper.frmBase, UIHelper.IfrmDBBase
    {

        public const string x_CMemDetail = "Det";
        public const string x_CMem2Detail = "Dt2";
        public const string x_CMRem = "Rem";
        public const string x_CMRem2 = "Rm2";
        public const string x_CMAd11 = "A11";
        public const string x_CMAd21 = "A21";
        public const string x_CMAd31 = "A31";
        public const string x_CMAd12 = "A12";
        public const string x_CMAd22 = "A22";
        public const string x_CMAd32 = "A32";

        public const string xdCMCtAd11 = "C11";
        public const string xdCMCtAd21 = "C21";
        public const string xdCMCtAd31 = "C31";
        public const string xdCMCtAd12 = "C12";
        public const string xdCMCtAd22 = "C22";
        public const string xdCMCtAd32 = "C32";

        public const string x_CMCtZip = "CZp";
        public const string x_CMCtTel = "CTl";
        public const string x_CMCtFax = "CFx";
        public const string x_CMTBusi = "TBu";
        public const string x_PrcSCoor = "A";		//Sale Coor ส่วนลดประจำตัว
        public const string x_CMEmail = "Ema";

        public const string x_CMTel = "Tel";
        public const string x_CMFax = "Fax";
        public const string x_CMTxDesti = "Des";
        public const string x_CMTaxId = "Tax";
        public const string x_CMRemLayH = "Lay";
        public const string xdCMMId = "MId";		//25/03/47 By Pic

        public static string TASKNAME = "ESUPPL";

        public static int MAXLENGTH_CODE = 20;
        public static int MAXLENGTH_NAME = 70;
        public static int MAXLENGTH_NAME2 = 70;
        public static int MAXLENGTH_SNAME = 70;
        public static int MAXLENGTH_SNAME2 = 70;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = MapTable.Table.MasterCoor;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";
        private string mstrOldSName = "";
        private string mstrOldName2 = "";
        private string mstrOldSName2 = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        //private frmAcChart pofrmGetAcChart = null;
        //private frmCrGrp pofrmGetCrGrp = null;
        //private DialogForms.dlgGetVatType pofrmGetVatType = null;
        //private DialogForms.dlgGetBank pofrmGetBank = null;

        public frmSuppl()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmSuppl(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmSuppl mInstanse = null;

        public static frmSuppl GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmSuppl();
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
            this.txtCode.Properties.MaxLength = frmSuppl.MAXLENGTH_CODE;
            this.txtName.Properties.MaxLength = frmSuppl.MAXLENGTH_NAME;
            this.txtName2.Properties.MaxLength = frmSuppl.MAXLENGTH_NAME2;
            this.txtSName.Properties.MaxLength = frmSuppl.MAXLENGTH_SNAME;
            this.txtSName2.Properties.MaxLength = frmSuppl.MAXLENGTH_SNAME2;

            this.txtQcAcChart.Properties.MaxLength = frmAcChart.MAXLENGTH_CODE;
            this.txtQnAcChart.Properties.MaxLength = frmAcChart.MAXLENGTH_NAME;

            this.txtQcCrGrp.Properties.MaxLength = frmCrGrp.MAXLENGTH_CODE;
            this.txtQnCrGrp.Properties.MaxLength = frmCrGrp.MAXLENGTH_NAME;

            this.txtQcVatType.Properties.MaxLength = DialogForms.dlgGetVatType.MAXLENGTH_CODE;

            this.cmbPersonType.Properties.Items.AddRange(new object[] { "Y = ใช่", "N = ไม่ใช่" });
            this.cmbPersonType.SelectedIndex = 0;
            this.pmSetPersonStatus();

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strSQLExec = "select {0}.CROWID, {0}.CCODE, {0}.CSNAME, {0}.CNAME, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
            strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, strEmplRTab });

            //pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "Master_CrGrp", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "CCODE";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["CCODE"].Visible = true;
            this.gridView1.Columns["CNAME"].Visible = true;
            //this.gridView1.Columns["CSNAME"].Visible = true;

            this.gridView1.Columns["CCODE"].Caption = "รหัส";
            this.gridView1.Columns["CNAME"].Caption = "ชื่อ";
            //this.gridView1.Columns["CSNAME"].Caption = "ชื่อย่อ";

            this.gridView1.Columns["CCODE"].Width = 15;
            //this.gridView1.Columns["CSNAME"].Width = 70;

            this.pmSetSortKey("cCode", true);
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
                    case WsToolBar.Search:
                        this.pmSearchData();
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

            }
        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cName"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลผู้จำหน่าย : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow["cCode"].ToString(), dtrBrow["cName"].ToString(), ref strErrorMsg))
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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            bool bllHasUsed = false;// QRefChild.HasUsedChildTable(objSQLHelper, inCode, ref ioErrorMsg);
            if (bllHasUsed)
            {
                ioErrorMsg = "ไม่สามารถลบข้อมูลได้เนื่องจากมีการอ้างอิงถึงใน " + ioErrorMsg;
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

            this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mSaveDBAgent2.AppID = App.AppID;
            this.mdbConn2 = this.mSaveDBAgent2.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.mdbConn2.Open();
                this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strErrorMsg = "";

                //Delete Child Table
                //Business.Entity.QMasterCoor QRefChild = new QMasterCoor(App.ConnectionString, App.DatabaseReside);
                QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                QRefChild.DeleteChildTable(inCode);

                object[] pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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
                this.mdbConn2.Close();
            }
            return bllResult;
        }

        private void pmSearchData()
        {
            using (cfrmSearchData dlg = new cfrmSearchData())
            {
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK
                    && dlg.Code != null)
                {

                    this.pmSetSortKey(dlg.SearchKey, true);

                    int intSeekRow = this.gridView1.LocateByValue(0, this.gridView1.Columns["CCODE"], dlg.Code);
                    if (intSeekRow > -1 && intSeekRow <= this.gridView1.RowCount)
                    {
                        this.gridView1.FocusedRowHandle = intSeekRow;
                    }
                }
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
                if (MessageBox.Show("ยังไม่ได้ระบุรหัสผู้จำหน่าย ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสผู้จำหน่าย";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุ ชื่อผู้จำหน่าย";
                this.txtName.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสผู้จำหน่ายซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
                && !this.pmIsValidateName(new object[] { this.txtName.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อผู้จำหน่ายซ้ำ";
                this.txtName.Focus();
                return false;
            }
            else if (this.txtSName.Text.TrimEnd() != string.Empty
               && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldSName.TrimEnd() != this.txtSName.Text.TrimEnd())
               && !this.pmIsValidateSName(new object[] { this.txtSName.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อย่อผู้จำหน่ายซ้ำ";
                this.txtSName.Focus();
                return false;
            }
            else if (this.txtName2.Text.TrimEnd() != string.Empty
                && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName2.TrimEnd() != this.txtName2.Text.TrimEnd())
               && !this.pmIsValidateName2(new object[] { this.txtName2.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อผู้จำหน่ายภาษา 2 ซ้ำ";
                this.txtName2.Focus();
                return false;
            }
            else if (this.txtSName2.Text.TrimEnd() != string.Empty
                && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldSName2.TrimEnd() != this.txtSName2.Text.TrimEnd())
                && !this.pmIsValidateSName2(new object[] { this.txtSName2.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อย่อผู้จำหน่ายภาษา 2 ซ้ำ";
                this.txtSName2.Focus();
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
            //objSQLHelper.SetPara(inPrefixPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["CCODE"].ToString().Trim();
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cCode = ?", ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cName = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateName2(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cName2 = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateSName(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cSName = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateSName2(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cIsSupp = 'Y' and cSName2 = ?", ref strErrorMsg))
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
                dtrSaveInfo["cCreateBy"] = App.FMAppUserID;
            }

            string gcTemStr01 = BizRule.SetMemData(this.txtAddr11.Text.Trim(), x_CMAd11);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr21.Text.Trim(), x_CMAd21);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr31.Text.Trim(), x_CMAd31);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr12.Text.Trim(), x_CMAd12);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr22.Text.Trim(), x_CMAd22);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr32.Text.Trim(), x_CMAd32);
            gcTemStr01 += BizRule.SetMemData(this.txtTel.Text.Trim(), x_CMTel);
            gcTemStr01 += BizRule.SetMemData(this.txtFax.Text.Trim(), x_CMFax);
            gcTemStr01 += BizRule.SetMemData(this.txtRemark.Text.Trim(), x_CMRem);
            gcTemStr01 += BizRule.SetMemData(this.txtRemark2.Text.Trim(), x_CMRem2);
            gcTemStr01 += BizRule.SetMemData(this.txtTaxID.Text.Trim(), x_CMTaxId);

            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr11.Text.Trim(), xdCMCtAd11);
            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr21.Text.Trim(), xdCMCtAd21);
            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr31.Text.Trim(), xdCMCtAd31);
            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr12.Text.Trim(), xdCMCtAd12);
            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr22.Text.Trim(), xdCMCtAd22);
            gcTemStr01 += BizRule.SetMemData(this.txtCTAddr32.Text.Trim(), xdCMCtAd32);
            gcTemStr01 += BizRule.SetMemData(this.txtCTZip.Text.Trim(), x_CMCtZip);
            gcTemStr01 += BizRule.SetMemData(this.txtRemarkLayH.Text.Trim(), x_CMRemLayH);
            gcTemStr01 += BizRule.SetMemData(this.txtMTaxID.Text.Trim(), xdCMMId);
            gcTemStr01 += BizRule.SetMemData(this.txtEMail.Text.Trim(), x_CMEmail);

            dtrSaveInfo["cRowID"] = this.mstrEditRowID;
            dtrSaveInfo["cCode"] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo["cName"] = this.txtName.Text.TrimEnd();
            dtrSaveInfo["cName2"] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo["cFChr"] = AppUtil.StringHelper.GetFChr(this.txtName.Text.TrimEnd());
            dtrSaveInfo["cSName"] = this.txtSName.Text.TrimEnd();
            dtrSaveInfo["cSName2"] = this.txtSName2.Text.TrimEnd();
            dtrSaveInfo["cZip"] = this.txtZip.Text.TrimEnd();
            dtrSaveInfo["cIsSupp"] = "Y";
            if (this.txtSName.Text.TrimEnd().Length == 0)
            {
                dtrSaveInfo["cFChrS"] = "";
            }
            else
            {
                dtrSaveInfo["cFChrS"] = AppUtil.StringHelper.GetFChr(this.txtSName.Text.TrimEnd());
            }

            dtrSaveInfo["cPersonty"] = (this.cmbPersonType.SelectedIndex == 0 ? "Y" : "N");
            dtrSaveInfo["cCrGrp"] = this.txtQcCrGrp.Tag.ToString();
            dtrSaveInfo["cAcChart"] = this.txtQcAcChart.Tag.ToString();
            dtrSaveInfo["cVatType"] = this.txtQcVatType.Text.TrimEnd();
            dtrSaveInfo["cBank"] = this.txtQnBank.Tag.ToString();
            dtrSaveInfo["nCredTerm"] = this.txtCredTerm.Value;
            dtrSaveInfo["nCredLim"] = this.txtCredLim.Value;
            dtrSaveInfo["cContactN"] = this.txtCTName.Text.TrimEnd();
            dtrSaveInfo["cBankNo"] = this.txtBankNo.Text.TrimEnd();
            dtrSaveInfo["cBBranch"] = this.txtBBranch.Text.TrimEnd();

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo["cMemData"] = gcTemStr02;
            dtrSaveInfo["cMemData2"] = gcTemStr03;
            dtrSaveInfo["cMemData3"] = gcTemStr04;
            dtrSaveInfo["cMemData4"] = gcTemStr05;
            dtrSaveInfo["cMemData5"] = gcTemStr06;

            dtrSaveInfo["cLastUpdBy"] = App.FMAppUserID;
            dtrSaveInfo["dLastUpd"] = objSQLHelper.GetDBServerDateTime(); ;

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
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
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                Business.Entity.QMasterCoor QRefChild = new QMasterCoor(App.ConnectionString, App.DatabaseReside);
                QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                QRefChild.SaveChildTable(dtrSaveInfo, this.mstrOldCode, "", "", this.txtQcCrGrp.Text, this.txtQcAcChart.Text);

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
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
                this.mdbConn2.Close();
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

            //Page Edit 1
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtSName.Text = "";
            this.txtAddr11.Text = "";
            this.txtAddr21.Text = "";
            this.txtAddr31.Text = "";
            this.txtName2.Text = "";
            this.txtSName2.Text = "";
            this.txtAddr12.Text = "";
            this.txtAddr22.Text = "";
            this.txtAddr32.Text = "";
            this.txtZip.Text = "";
            this.txtTel.Text = "";
            this.txtFax.Text = "";

            //Page Edit 2
            this.cmbPersonType.SelectedIndex = 0;
            this.txtTaxID.Text = "";
            this.txtMTaxID.Text = "";
            this.txtQcAcChart.Tag = "";
            this.txtQcAcChart.Text = "";
            this.txtQnAcChart.Text = "";
            this.txtCTAddr11.Text = "";
            this.txtCTAddr21.Text = "";
            this.txtCTAddr31.Text = "";
            this.txtCTAddr12.Text = "";
            this.txtCTAddr22.Text = "";
            this.txtCTAddr32.Text = "";
            this.txtCTZip.Text = "";
            this.txtRemarkLayH.Text = "";
            this.txtRemark.Text = "";
            this.txtRemark2.Text = "";
            this.txtEMail.Text = "";

            //Page Edit 3
            this.txtCredTerm.Value = 0;
            this.txtCredLim.Value = 0;
            this.txtCTName.Text = "";
            this.txtQcVatType.Tag = "";
            this.txtQcVatType.Text = "";
            this.txtQnVatType.Text = "";
            this.txtQcCrGrp.Tag = "";
            this.txtQcCrGrp.Text = "";
            this.txtQnCrGrp.Text = "";

            this.txtBankNo.Text = "";
            this.txtQnBank.Text = "";
            this.txtQnBank.Tag = "";
            this.txtBBranch.Text = "";

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
                    this.txtCode.Text = dtrLoadForm["cCode"].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm["cName"].ToString().TrimEnd();
                    this.txtSName.Text = dtrLoadForm["cSName"].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm["cName2"].ToString().TrimEnd();
                    this.txtSName2.Text = dtrLoadForm["cSName2"].ToString().TrimEnd();

                    string gcTemStr01 = (Convert.IsDBNull(dtrLoadForm["cMemData"]) ? "" : dtrLoadForm["cMemData"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["cMemData2"]) ? "" : dtrLoadForm["cMemData2"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["cMemData3"]) ? "" : dtrLoadForm["cMemData3"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["cMemData4"]) ? "" : dtrLoadForm["cMemData4"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["cMemData5"]) ? "" : dtrLoadForm["cMemData5"].ToString().TrimEnd());

                    this.txtAddr11.Text = BizRule.GetMemData(gcTemStr01, x_CMAd11);
                    this.txtAddr21.Text = BizRule.GetMemData(gcTemStr01, x_CMAd21);
                    this.txtAddr31.Text = BizRule.GetMemData(gcTemStr01, x_CMAd31);
                    this.txtAddr12.Text = BizRule.GetMemData(gcTemStr01, x_CMAd12);
                    this.txtAddr22.Text = BizRule.GetMemData(gcTemStr01, x_CMAd22);
                    this.txtAddr32.Text = BizRule.GetMemData(gcTemStr01, x_CMAd32);

                    this.txtZip.Text = dtrLoadForm["cZip"].ToString().TrimEnd();
                    this.txtTel.Text = BizRule.GetMemData(gcTemStr01, x_CMTel);
                    this.txtFax.Text = BizRule.GetMemData(gcTemStr01, x_CMFax);

                    this.cmbPersonType.SelectedIndex = (dtrLoadForm["cPersonTy"].ToString() == "Y" ? 0 : 1);

                    this.txtRemark.Text = BizRule.GetMemData(gcTemStr01, x_CMRem);
                    this.txtRemark2.Text = BizRule.GetMemData(gcTemStr01, x_CMRem2);
                    this.txtTaxID.Text = BizRule.GetMemData(gcTemStr01, x_CMTaxId);

                    this.txtCTAddr11.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd11);
                    this.txtCTAddr21.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd21);
                    this.txtCTAddr31.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd31);

                    this.txtCTAddr12.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd12);
                    this.txtCTAddr22.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd22);
                    this.txtCTAddr32.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd32);
                    this.txtCTZip.Text = BizRule.GetMemData(gcTemStr01, x_CMCtZip);

                    this.txtRemarkLayH.Text = BizRule.GetMemData(gcTemStr01, x_CMRemLayH);
                    this.txtMTaxID.Text = BizRule.GetMemData(gcTemStr01, xdCMMId);
                    this.txtEMail.Text = BizRule.GetMemData(gcTemStr01, x_CMEmail);
                    
                    
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["cAcChart"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrRefTable, "select * from " + MapTable.Table.MasterAcChart + " where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
                        this.txtQcAcChart.Tag = dtrLoadForm["cAcChart"].ToString();
                        this.txtQcAcChart.Text = dtrAcChart["cCode"].ToString().TrimEnd();
                        this.txtQnAcChart.Text = dtrAcChart["cName"].ToString().TrimEnd();
                    }

                    this.txtCredTerm.Value = Convert.ToInt32(dtrLoadForm["nCredTerm"]);
                    this.txtCredLim.Value = Convert.ToDecimal(dtrLoadForm["nCredLim"]);
                    this.txtCTName.Text = dtrLoadForm["cContactN"].ToString().TrimEnd();
                    this.txtBankNo.Text = dtrLoadForm["cBankNo"].ToString().TrimEnd();
                    this.txtBBranch.Text = dtrLoadForm["cBBranch"].ToString().TrimEnd();

                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["cCrGrp"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCrGrp", "CRGRP", "select * from " + MapTable.Table.MasterCrGrp + " where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrCrGrp = this.dtsDataEnv.Tables["QCrGrp"].Rows[0];
                        this.txtQcCrGrp.Tag = dtrLoadForm["cCrGrp"].ToString();
                        this.txtQcCrGrp.Text = dtrCrGrp["cCode"].ToString().TrimEnd();
                        this.txtQnCrGrp.Text = dtrCrGrp["cName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["cVatType"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVatType", "VATTYPE", "select * from " + MapTable.Table.VatType + " where fcCode = ?", ref strErrorMsg))
                    {
                        DataRow dtrVatType = this.dtsDataEnv.Tables["QVatType"].Rows[0];
                        this.txtQcVatType.Tag = dtrLoadForm["cVatType"].ToString();
                        this.txtQcVatType.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                        this.txtQnVatType.Text = dtrVatType["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["cBank"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBank", "BANK", "select * from " + MapTable.Table.Bank + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrBank = this.dtsDataEnv.Tables["QBank"].Rows[0];
                        this.txtQnBank.Tag = dtrLoadForm["cBank"].ToString();
                        this.txtQnBank.Text = dtrBank["fcName"].ToString().TrimEnd();
                    }

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldName = this.txtName.Text;
            this.mstrOldSName = this.txtSName.Text;
            this.mstrOldName2 = this.txtName2.Text;
            this.mstrOldSName2 = this.txtSName2.Text;
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void frmSuppl_KeyDown(object sender, KeyEventArgs e)
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
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex - 1 <= xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);
                    break;
                case Keys.PageDown:
                    if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_EDIT1 : this.pgfMainEdit.SelectedTabPageIndex + 1);
                    break;
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
                this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
            }
            else
            {
                this.txtFooter.Text = "";
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void cmbPersonType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetPersonStatus();
        }

        private void pmSetPersonStatus()
        {
            this.txtTaxID.Enabled = (this.cmbPersonType.SelectedIndex == 0);
            this.txtMTaxID.Enabled = (this.cmbPersonType.SelectedIndex == 1);
        }
        
        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "ACCHART":
                    if (this.pofrmGetAcChart == null)
                    {
                        this.pofrmGetAcChart = new DatabaseForms.frmAcChart(FormActiveMode.PopUp);
                        this.pofrmGetAcChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetAcChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "CRGRP":
                    if (this.pofrmGetCrGrp == null)
                    {
                        this.pofrmGetCrGrp = new DatabaseForms.frmCrGrp(FormActiveMode.PopUp);
                        this.pofrmGetCrGrp.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCrGrp.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "VATTYPE":
                    if (this.pofrmGetVatType == null)
                    {
                        this.pofrmGetVatType = new mBudget.DialogForms.dlgGetVatType();
                        this.pofrmGetVatType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetVatType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BANK":
                    if (this.pofrmGetBank == null)
                    {
                        this.pofrmGetBank = new mBudget.DialogForms.dlgGetBank();
                        this.pofrmGetBank.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBank.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonCick(string inTextbox, string inPara1)
        {
            switch (inTextbox)
            {
                case "TXTQCACCHART":
                case "TXTQNACCHART":
                    string strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
                    this.pmInitPopUpDialog("ACCHART");
                    this.pofrmGetAcChart.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME"), true);
                    if (this.pofrmGetAcChart.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCCRGRP":
                case "TXTQNCRGRP":
                    this.pmInitPopUpDialog("CRGRP");
                    this.pofrmGetCrGrp.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCCRGRP" ? "CCODE" : "CNAME"), true);
                    if (this.pofrmGetCrGrp.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCVATTYPE":
                    this.pmInitPopUpDialog("VATTYPE");
                    this.pofrmGetVatType.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCVATTYPE" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetVatType.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCBANK":
                case "TXTQNBANK":
                    this.pmInitPopUpDialog("BANK");
                    this.pofrmGetBank.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCBANK" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetBank.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = null;

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTQCACCHART":
                case "TXTQNACCHART":
                    if (this.pofrmGetAcChart != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

                        if (dtrAcChart != null)
                        {
                            this.txtQcAcChart.Tag = dtrAcChart["cRowID"].ToString();
                            this.txtQcAcChart.Text = dtrAcChart["cCode"].ToString().TrimEnd();
                            this.txtQnAcChart.Text = dtrAcChart["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcAcChart.Tag = "";
                            this.txtQcAcChart.Text = "";
                            this.txtQnAcChart.Text = "";
                        }
                    }
                    break;
                case "TXTQCCRGRP":
                case "TXTQNCRGRP":
                    if (this.pofrmGetCrGrp != null)
                    {
                        DataRow dtrCrGrp = this.pofrmGetCrGrp.RetrieveValue();

                        if (dtrCrGrp != null)
                        {
                            this.txtQcCrGrp.Tag = dtrCrGrp["cRowID"].ToString();
                            this.txtQcCrGrp.Text = dtrCrGrp["cCode"].ToString().TrimEnd();
                            this.txtQnCrGrp.Text = dtrCrGrp["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcCrGrp.Tag = "";
                            this.txtQcCrGrp.Text = "";
                            this.txtQnCrGrp.Text = "";
                        }
                    }
                    break;
                case "TXTQCVATTYPE":
                case "TXTQNVATTYPE":
                    if (this.pofrmGetVatType != null)
                    {
                        DataRow dtrVatType = this.pofrmGetVatType.RetrieveValue();

                        if (dtrVatType != null)
                        {
                            this.txtQcVatType.Tag = dtrVatType["fcSkid"].ToString();
                            this.txtQcVatType.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                            this.txtQnVatType.Text = dtrVatType["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcVatType.Tag = "";
                            this.txtQcVatType.Text = "";
                            this.txtQnVatType.Text = "";
                        }
                    }
                    break;
                case "TXTQCBANK":
                case "TXTQNBANK":
                    if (this.pofrmGetBank != null)
                    {
                        DataRow dtrBank = this.pofrmGetBank.RetrieveValue();

                        if (dtrBank != null)
                        {
                            this.txtQnBank.Tag = dtrBank["fcSkid"].ToString();
                            this.txtQnBank.Text = dtrBank["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQnBank.Tag = "";
                            this.txtQnBank.Text = "";
                        }
                    }
                    break;
            }
        }

        private void txtQcAcChart_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCACCHART" ? "cCode" : "cName");
            if (txtPopUp.Text == "")
            {
                this.txtQcAcChart.Tag = "";
                this.txtQcAcChart.Text = "";
                this.txtQnAcChart.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("ACCHART");
                e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetAcChart.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcCrGrp_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCCRGRP" ? "cCode" : "cName");
            if (txtPopUp.Text == "")
            {
                this.txtQcCrGrp.Tag = "";
                this.txtQcCrGrp.Text = "";
                this.txtQnCrGrp.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("CRGRP");
                e.Cancel = !this.pofrmGetCrGrp.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCrGrp.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcVatType_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCVATTYPE" ? "FCCODE" : "FCNAME");
            if (txtPopUp.Text == "")
            {
                this.txtQcVatType.Tag = "";
                this.txtQcVatType.Text = "";
                this.txtQnVatType.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("VATTYPE");
                e.Cancel = !this.pofrmGetVatType.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetVatType.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQnBank_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCBANK" ? "FCCODE" : "FCNAME");
            if (txtPopUp.Text == "")
            {
                this.txtQnBank.Tag = "";
                this.txtQnBank.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BANK");
                e.Cancel = !this.pofrmGetBank.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBank.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "CCODE")
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
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
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


    }
}
