
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
    public partial class frmEMSuppl : UIHelper.frmBase, UIHelper.IfrmDBBase
    {

        public static string TASKNAME = "ESUPPL";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QCoorInfo.TableName;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QCoorInfo.Field.Code;

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

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

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmEMSuppl()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmEMSuppl(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmEMSuppl mInstanse = null;

        public static frmEMSuppl GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmEMSuppl();
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

            UIBase.SetDefaultChildAppreance(this);

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

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Name);
            //this.txtAddr11.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Addr1);
            //this.txtAddr12.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Addr2);
            //this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Name2);
            //this.txtAddr21.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Addr12);
            //this.txtAddr22.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.Addr22);
            //this.txtTel.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.TelNo);
            //this.txtFax.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QCoorInfo.Field.FaxNo);
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

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            //string strSQLExec = "select EMCOOR.CROWID, EMCOOR.CCODE, EMCOOR.CNAME, EMCOOR.CNAME2, EMCOOR.DCREATE, EMCOOR.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} EMCOOR ";
            //strSQLExec += " left join {1} EM1 ON EM1.fcSkid = EMCOOR.CCREATEBY ";
            //strSQLExec += " left join {1} EM2 ON EM2.fcSkid = EMCOOR.CLASTUPDBY ";
            string strSQLExec = "select EMCOOR.FCSKID, EMCOOR.FCCODE, EMCOOR.FCNAME, EMCOOR.FCNAME2, EMCOOR.FTDATETIME as FDCREATE, EMCOOR.FTLASTUPD as FDLASTUPDBY from {0} EMCOOR ";
            strSQLExec += " where EMCOOR.FCCORP = ? and FCISSUPP = 'Y'";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            pobjSQLUtil.SetPara(new object[] {App.ActiveCorp.RowID});
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "EMCOOR", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QCoorInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QCoorInfo.Field.Code].Visible = true;
            this.gridView1.Columns[QCoorInfo.Field.Name].Visible = true;
            this.gridView1.Columns[QCoorInfo.Field.Name2].Visible = true;

            this.gridView1.Columns[QCoorInfo.Field.Code].Caption = "รหัสผู้จำหน่าย";
            this.gridView1.Columns[QCoorInfo.Field.Name].Caption = "ชื่อผู้จำหน่าย";
            this.gridView1.Columns[QCoorInfo.Field.Name2].Caption = "ชื่อผู้จำหน่ายภาษา 2";

            this.gridView1.Columns[QCoorInfo.Field.Code].Width = 15;
            this.gridView1.Columns[QCoorInfo.Field.Name2].Width = 25;

            this.gridView1.Columns[QCoorInfo.Field.Code].VisibleIndex = 0;
            this.gridView1.Columns[QCoorInfo.Field.Name].VisibleIndex = 1;
            this.gridView1.Columns[QCoorInfo.Field.Name2].VisibleIndex = 2;

            this.pmSetSortKey(QCoorInfo.Field.Code, true);
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

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QCoorInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QCoorInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QCoorInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลผู้จำหน่าย : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QCoorInfo.Field.Code].ToString(), dtrBrow[QCoorInfo.Field.Name].ToString(), ref strErrorMsg))
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
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID , this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสผู้จำหน่ายซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
               && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อผู้จำหน่ายซ้ำ";
                this.txtName.Focus();
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
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QCoorInfo.Field.Code].ToString().Trim();
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
                dtrSaveInfo["FTDATETIME"] = objSQLHelper.GetDBServerDateTime();
                dtrSaveInfo["FTLASTUPD"] = objSQLHelper.GetDBServerDateTime();
            }

            // Control Field ที่ทุก Form ต้องมี
            //dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            //dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            //dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            dtrSaveInfo[MapTable.ShareField.fcSkid] = this.mstrEditRowID;
            dtrSaveInfo["FTLASTUPD"] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QCoorInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QCoorInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QCoorInfo.Field.Name] = this.txtName.Text.TrimEnd();
            //dtrSaveInfo[QCoorInfo.Field.Addr1] = this.txtAddr11.Text.TrimEnd();
            //dtrSaveInfo[QCoorInfo.Field.Addr2] = this.txtAddr12.Text.TrimEnd();
            dtrSaveInfo[QCoorInfo.Field.Name2] = this.txtName2.Text.TrimEnd();

            dtrSaveInfo["fcSName"] = this.txtSName.Text.TrimEnd();
            dtrSaveInfo["fcSName2"] = this.txtSName2.Text.TrimEnd();

            //dtrSaveInfo[QCoorInfo.Field.Addr12] = this.txtAddr21.Text.TrimEnd();
            //dtrSaveInfo[QCoorInfo.Field.Addr22] = this.txtAddr22.Text.TrimEnd();
            dtrSaveInfo[QCoorInfo.Field.TelNo] = this.txtTel.Text.TrimEnd();
            //dtrSaveInfo[QCoorInfo.Field.FaxNo] = this.txtFax.Text.TrimEnd();

            string gcTemStr01 = BizRule.SetMemData(this.txtAddr11.Text.Trim(), x_CMAd11);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr21.Text.Trim(), x_CMAd21);
            //gcTemStr01 += BizRule.SetMemData(this.txtAddr31.Text.Trim(), x_CMAd31);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr12.Text.Trim(), x_CMAd12);
            gcTemStr01 += BizRule.SetMemData(this.txtAddr22.Text.Trim(), x_CMAd22);
            //gcTemStr01 += BizRule.SetMemData(this.txtAddr32.Text.Trim(), x_CMAd32);
            gcTemStr01 += BizRule.SetMemData(this.txtTel.Text.Trim(), x_CMTel);
            gcTemStr01 += BizRule.SetMemData(this.txtFax.Text.Trim(), x_CMFax);
            gcTemStr01 += BizRule.SetMemData(this.txtTaxID.Text.Trim(), x_CMTaxId);

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo["fmMemData"] = gcTemStr02;
            dtrSaveInfo["fmMemData2"] = gcTemStr03;
            dtrSaveInfo["fmMemData3"] = gcTemStr04;
            dtrSaveInfo["fmMemData4"] = gcTemStr05;
            dtrSaveInfo["fmMemData5"] = gcTemStr06;
            dtrSaveInfo["fcIsSupp"] = "Y";
            dtrSaveInfo["fcZip"] = this.txtZip.Text.TrimEnd();

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
            this.txtSName.Text = "";
            this.txtSName2.Text = "";
            this.txtAddr11.Text = "";
            this.txtAddr12.Text = "";
            this.txtAddr21.Text = "";
            this.txtAddr22.Text = "";
            this.txtTel.Text = "";
            this.txtFax.Text = "";
            this.txtZip.Text = "";

            this.txtTaxID.Text = "";

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

                    this.txtCode.Text = dtrLoadForm["fcCode"].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm["fcName"].ToString().TrimEnd();
                    this.txtSName.Text = dtrLoadForm["fcSName"].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm["fcName2"].ToString().TrimEnd();
                    this.txtSName2.Text = dtrLoadForm["fcSName2"].ToString().TrimEnd();

                    //this.txtAddr11.Text = dtrLoadForm[QCoorInfo.Field.Addr1].ToString().TrimEnd();
                    //this.txtAddr12.Text = dtrLoadForm[QCoorInfo.Field.Addr2].ToString().TrimEnd();

                    //this.txtAddr21.Text = dtrLoadForm[QCoorInfo.Field.Addr12].ToString().TrimEnd();
                    //this.txtAddr22.Text = dtrLoadForm[QCoorInfo.Field.Addr22].ToString().TrimEnd();
                    this.txtTel.Text = dtrLoadForm[QCoorInfo.Field.TelNo].ToString().TrimEnd();
                    //this.txtFax.Text = dtrLoadForm[QCoorInfo.Field.FaxNo].ToString().TrimEnd();

                    string gcTemStr01 = (Convert.IsDBNull(dtrLoadForm["fmMemData"]) ? "" : dtrLoadForm["fmMemData"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData2"]) ? "" : dtrLoadForm["fmMemData2"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData3"]) ? "" : dtrLoadForm["fmMemData3"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData4"]) ? "" : dtrLoadForm["fmMemData4"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData5"]) ? "" : dtrLoadForm["fmMemData5"].ToString().TrimEnd());

                    this.txtAddr11.Text = BizRule.GetMemData(gcTemStr01, x_CMAd11);
                    this.txtAddr21.Text = BizRule.GetMemData(gcTemStr01, x_CMAd21);
                    //this.txtAddr31.Text = BizRule.GetMemData(gcTemStr01, x_CMAd31);
                    this.txtAddr12.Text = BizRule.GetMemData(gcTemStr01, x_CMAd12);
                    this.txtAddr22.Text = BizRule.GetMemData(gcTemStr01, x_CMAd22);
                    //this.txtAddr32.Text = BizRule.GetMemData(gcTemStr01, x_CMAd32);
                    this.txtFax.Text = BizRule.GetMemData(gcTemStr01, x_CMFax);

                    this.txtZip.Text = dtrLoadForm["fcZip"].ToString().TrimEnd();
                    this.txtTel.Text = BizRule.GetMemData(gcTemStr01, x_CMTel);

                    this.txtTaxID.Text = BizRule.GetMemData(gcTemStr01, x_CMTaxId);


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
            if (inOrderBy.ToUpper() == QCoorInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QCoorInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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

    }
}
