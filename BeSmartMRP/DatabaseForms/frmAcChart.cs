
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

using mBudget.Business.Entity;
using mBudget.UIHelper;
using mBudget.Business.Agents;

namespace mBudget.DatabaseForms
{
    public partial class frmAcChart : UIHelper.frmBase
    {

         /// <summary>
        /// Class for Search Dialog
        /// </summary>
        #region "Search Dialog Class"
        private class cfrmSearchData : DialogForms.cfrmSearchBase
        {

            private QMasterAcChart mobjTabRefer = null;
            private string mstrRowID = "";
            private string mstrCode = "";

            private bool mbllIsPopUpMode = false;

            public cfrmSearchData(bool inPopUp)
            {
                this.mbllIsPopUpMode = inPopUp;
            }

            override protected void OnInitComponent()
            {
                this.mobjTabRefer = new QMasterAcChart(App.ConnectionString, App.DatabaseReside);
                this.mobjTabRefer.IsPopUp = this.mbllIsPopUpMode;

                this.AddSearchKey("����", "CCODE");
                this.AddSearchKey("����", "CNAME");
                base.OnInitComponent();

                this.mstrSearchVal = "";
                this.mstrSearchKey = "CCODE";
                this.pmSetBrowView();
                this.pmInitGridProp();
            }

            override protected void OnSearch(string inSearchVal, string inKey)
            {
                //this.mstrSearchVal = inSearchVal;
                this.mstrSearchVal = (inSearchVal != string.Empty ? "%" + inSearchVal + "%" : inSearchVal);
                this.mstrSearchKey = inKey;

                this.pmSetBrowView();
                this.pmRefreshBrowView();
                base.OnSearch(inSearchVal, inKey);
            }

            override protected void OnSelected()
            {
                base.OnSelected();
                this.pmSetSelectedItem();
            }

            private void pmSetSelectedItem()
            {
                DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                if (dtrBrow != null)
                {
                    this.mstrRowID = dtrBrow["cRowID"].ToString();
                    this.mstrCode = dtrBrow["cCode"].ToString();
                }
            }

            public string RowID
            {
                get { return this.mstrRowID; }
            }

            public string Code
            {
                get { return this.mstrCode; }
            }

            public string SearchKey
            {
                get { return this.mstrSearchKey; }
            }

            private void pmSetBrowView()
            {
                DataTable dtrBrowView = new DataTable();
                dtrBrowView = this.mobjTabRefer.QueryData(new object[] { this.mstrSearchVal }, this.mstrSearchKey, this.mstrBrowViewAlias);
                if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias] != null)
                    this.dtsDataEnv.Tables.Remove(this.mstrBrowViewAlias);

                this.dtsDataEnv.Tables.Add(dtrBrowView);
            }

            private void pmInitGridProp()
            {
                this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSearchKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.Columns["CROWID"].Visible = false;
                this.gridView1.Columns["CCODE"].Caption = "����";
                this.gridView1.Columns["CNAME"].Caption = "����";
                this.gridView1.Columns["CNAME2"].Caption = "�������� 2";

                this.gridView1.Columns["CCODE"].Width = 30;
            }

            private void pmRefreshBrowView()
            {
                this.pmSetBrowView();
                this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
                if (this.gridView1.RowCount > 0)
                    this.gridView1.FocusedRowHandle = 0;
            }
        }
        #endregion


        public static string TASKNAME = "EACCHART";

        public static int MAXLENGTH_CODE = 11;
        public static int MAXLENGTH_NAME = 60;
        public static int MAXLENGTH_NAME2 = 60;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = MapTable.Table.MasterAcChart;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";

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

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        public frmAcChart()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmAcChart(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmAcChart mInstanse = null;

        public static frmAcChart GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmAcChart();
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
            this.txtCode.Properties.MaxLength = frmAcChart.MAXLENGTH_CODE;
            this.txtName.Properties.MaxLength = frmAcChart.MAXLENGTH_NAME;
            this.txtName2.Properties.MaxLength = frmAcChart.MAXLENGTH_NAME2;
            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }

            this.cmbCateg.Properties.Items.AddRange(new object[] { "G = ������ѭ��(�������ʹ)", "D = �ѭ������" });
            this.cmbCateg.SelectedIndex = 0;

            this.cmbType.Properties.Items.AddRange(new object[] { 
                "A = �Թ��Ѿ�� (Asset)"
                , "L = ˹���Թ (Liability)"
                , "C = �ع (Capital)"
                , "R = ����� (Revenue)"
                , "X = �������� (Expense)"
                , "M = �������¡�ü�Ե (Manu.Expense)" });
            this.cmbType.SelectedIndex = 0;

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strSQLExec = "select {0}.CROWID, {0}.CCATEG, {0}.CCODE, {0}.CNAME, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
            strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                strSQLExec += " where {0}.CCATEG = 'D' ";
            }
            
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, strEmplRTab });

            //pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "Master_Dept", strSQLExec, ref strErrorMsg);

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
            this.gridView1.Columns["CCATEG"].Visible = (this.mFormActiveMode == FormActiveMode.Edit ? true : false);

            this.gridView1.Columns["CCODE"].Caption = "����";
            this.gridView1.Columns["CNAME"].Caption = "����";
            this.gridView1.Columns["CCATEG"].Caption = "";

            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["CCODE"].Width = 15;
            this.gridView1.Columns["CCATEG"].MinWidth = 5;
            this.gridView1.Columns["CCATEG"].Width = 5;
            //this.gridView1.Columns["CNAME"].Width = 100;

            this.gridView1.OptionsView.ColumnAutoWidth = true;
            //this.gridView1.Columns["CCATEG"].MinWidth = 5;
            //this.gridView1.Columns["CCATEG"].Width = 5;

            //this.gridView1.RefreshData();
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
                            MessageBox.Show("������Է���㹡������������ !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Update:
                        if (App.PermissionManager.CheckPermission(AuthenType.CanEdit, App.AppUserID, TASKNAME))
                        {
                            this.mFormEditMode = UIHelper.AppFormState.Edit;
                            this.pmLoadEditPage();
                        }
                        else
                            MessageBox.Show("������Է���㹡����䢢����� !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Delete:
                        if (App.PermissionManager.CheckPermission(AuthenType.CanDelete, App.AppUserID, TASKNAME))
                        {
                            this.pmDeleteData();
                        }
                        else
                            MessageBox.Show("������Է���㹡��ź������ !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Search:
                        this.pmSearchData();
                        break;
                    case WsToolBar.Undo:
                        if (MessageBox.Show("¡��ԡ������ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
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
                if (MessageBox.Show("�׹�ѹ���ź�����żѧ�ѭ�� : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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
            Business.Entity.QMasterAcChart QRefChild = new QMasterAcChart(App.ConnectionString, App.DatabaseReside);
            bool bllHasUsed = QRefChild.HasUsedChildTable(objSQLHelper, inCode, ref ioErrorMsg);
            if (bllHasUsed)
            {
                ioErrorMsg = "�������öź�����������ͧ�ҡ�ա����ҧ�ԧ�֧� " + ioErrorMsg;
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
                Business.Entity.QMasterAcChart QRefChild = new QMasterAcChart(App.ConnectionString, App.DatabaseReside);
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
            bool bllIsPopUp = (this.mFormActiveMode == FormActiveMode.PopUp || this.mFormActiveMode == FormActiveMode.Report);
            using (cfrmSearchData dlg = new cfrmSearchData(bllIsPopUp))
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
                UIBase.WaitWind("���ѧ�ѹ�֡������...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "�ѹ�֡���º����", "Save Complete" }), 500);
                UIBase.WaitWind("�ѹ�֡���º����");
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
                if (MessageBox.Show("�ѧ������к����ʼѧ�ѭ�� ��ͧ����������ͧ Running ���������� ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "�ѧ������к� ���ʼѧ�ѭ��";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "��س��к� ���ͼѧ�ѭ��";
                this.txtName.Focus();
                return false;
            }
            if (this.txtCode.Text.TrimEnd().Length ==1 && this.cmbCateg.SelectedIndex != 0)
            {
                ioErrorMsg = "   ����������ʺѭ�� 1 ��ѡ ��ͧ�繡�����ѭ����ҹ��  ";
                this.txtCode.Focus();
                return false;
            }
            //if (StringHelper.Left(this.txtCode.Text.TrimEnd(), 1) == "0")
            //{
            //    ioErrorMsg = "   ����������ʺѭ�� 1 ��ѡ ��ͧ�繡�����ѭ����ҹ��  ";
            //    this.txtCode.Focus();
            //    return false;
            //}
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "���ʼѧ�ѭ�ի��";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
                && !this.pmIsValidateName(new object[] { this.txtName.Text.TrimEnd() }))
            {
                ioErrorMsg = "���ͼѧ�ѭ�ի��";
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
            //objSQLHelper.SetPara(inPrefixPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCode < ':' order by cCode desc", ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCode = ?", ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cName = ?", ref strErrorMsg))
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

            string strGroup = "";
            switch (this.cmbType.SelectedIndex)
            {
                case 0 :
                    strGroup = "1";
                    break;
                case 1:
                    strGroup = "2";
                    break;
                case 2:
                    strGroup = "3";
                    break;
                case 3:
                    strGroup = "4";
                    break;
                case 4:
                    strGroup = "5";
                    break;
                case 5:
                    strGroup = "6";
                    break;
            }

            string strCateg = "";
            switch (this.cmbCateg.SelectedIndex)
            {
                case 0:
                    strCateg = "G";
                    break;
                case 1:
                    strCateg = "D";
                    break;
            }

            dtrSaveInfo["cRowID"] = this.mstrEditRowID;
            dtrSaveInfo["cCode"] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo["cName"] = this.txtName.Text.TrimEnd();
            dtrSaveInfo["cFChr"] = AppUtil.StringHelper.GetFChr(this.txtName.Text.TrimEnd());
            dtrSaveInfo["cName2"] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo["cGroup"] = strGroup;
            dtrSaveInfo["cCateg"] = strCateg;
            dtrSaveInfo["nLevel"] = this.txtCode.Text.TrimEnd().Length;
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

                Business.Entity.QMasterAcChart QRefChild = new QMasterAcChart(App.ConnectionString, App.DatabaseReside);
                QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                QRefChild.SaveChildTable(dtrSaveInfo, this.mstrOldCode);

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
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtName2.Text = "";

            this.cmbType.SelectedIndex = 0;
            this.cmbCateg.SelectedIndex = 1;
        }

        private void pmLoadFormData()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.mstrEditRowID = dtrBrow["cRowID"].ToString();

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
                {
                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
                    this.txtCode.Text = dtrLoadForm["cCode"].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm["cName"].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm["cName2"].ToString().TrimEnd();

                    string strGroup = dtrLoadForm["cGroup"].ToString();
                    int intGroup = 0;
                    switch (strGroup)
                    {
                        case "1":
                            intGroup = 0; 
                            break;
                        case "2":
                            intGroup = 1;
                            break;
                        case "3":
                            intGroup = 2;
                            break;
                        case "4":
                            intGroup = 3;
                            break;
                        case "5":
                            intGroup = 4;
                            break;
                        case "6":
                            intGroup = 5;
                            break;
                    }

                    this.cmbType.SelectedIndex = intGroup;

                    string strCateg = dtrLoadForm["cCateg"].ToString();
                    int intCateg = 0;
                    switch (strCateg)
                    {
                        case "G":
                            intCateg = 0;
                            break;
                        case "D":
                            intCateg = 1;
                            break;
                    }

                    this.cmbCateg.SelectedIndex = intCateg;
                
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

        private void frmAcChart_KeyDown(object sender, KeyEventArgs e)
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
                        if (MessageBox.Show("¡��ԡ������ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
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
                this.txtFooter.Text = "���ҧ�� LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " �ѹ��� : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\n�������ش�� LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " �ѹ��� : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
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
