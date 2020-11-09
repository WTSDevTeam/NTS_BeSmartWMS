
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
    public partial class frmEMPlant : UIHelper.frmBase, IfrmDBBase
    {

        public static string TASKNAME = "EPLANT";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QEMPlantInfo.TableName;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QEMPlantInfo.Field.Code;
        private string mstrFixType = "";
        private string mstrFormMenuName = UIBase.GetAppUIText(new string[] { "โรงงาน", "Plant" });

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private DatabaseForms.frmEMHoliday pofrmEMHoliday = null;
        private DatabaseForms.frmEMStdWorkHour pofrmEMStdWorkHour = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmEMPlant()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmEMPlant(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmEMPlant mInstanse = null;

        public static frmEMPlant GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmEMPlant();
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

            this.cmbType.Enabled = true;

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;

                this.cmbType.Enabled = false;
            }

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();
            this.pmSetFormUI();

            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPlantInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPlantInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMPlantInfo.Field.Name2);

            this.cmbType.Properties.Items.Clear();
            this.cmbType.Properties.Items.AddRange(new object[] { "D = DETAIL", "G = GROUP" });
            this.cmbType.SelectedIndex = 0;
        }

        private void pmSetFormUI()
        {
            this.lblHoliday.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลวันหยุดประจำปี :", "Select Holiday and NoworkingDay :" });
            this.lblStdWorkHr.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลเวลาทำงานมาตรฐาน :", "Select Stadard Work Hour :" });

        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.txtQcHoliday.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcHoliday.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcHoliday.Validating += new CancelEventHandler(txtQcHoliday_Validating);

            this.txtQcStdWorkHr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcStdWorkHr.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcStdWorkHr.Validating += new CancelEventHandler(txtQcStdWorkHr_Validating);
        
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "HOLIDAY":
                    if (this.pofrmEMHoliday == null)
                    {
                        this.pofrmEMHoliday = new frmEMHoliday(FormActiveMode.Report);
                        this.pofrmEMHoliday.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmEMHoliday.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "STDWORKHOUR":
                    if (this.pofrmEMStdWorkHour == null)
                    {
                        this.pofrmEMStdWorkHour = new frmEMStdWorkHour(FormActiveMode.Report);
                        this.pofrmEMStdWorkHour.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmEMStdWorkHour.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strTagButton = (e.Button.Tag != null ? e.Button.Tag.ToString() : "");
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCHOLIDAY":
                    this.pmInitPopUpDialog("HOLIDAY");
                    strOrderBy = (inTextbox == "TXTQCHOLIDAY" ? "CCODE" : "CNAME");
                    this.pofrmEMHoliday.ValidateField("", strOrderBy, true);
                    if (this.pofrmEMHoliday.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCSTDWORKHR":
                    this.pmInitPopUpDialog("STDWORKHOUR");
                    strOrderBy = (inTextbox == "TXTQCSTDWORKHR" ? "CCODE" : "CNAME");
                    this.pofrmEMStdWorkHour.ValidateField("", strOrderBy, true);
                    if (this.pofrmEMStdWorkHour.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTQCHOLIDAY":
                    if (this.pofrmEMHoliday != null)
                    {
                        DataRow dtrPDGRP = this.pofrmEMHoliday.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcHoliday.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcHoliday.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcHoliday.Tag = "";
                            this.txtQcHoliday.Text = "";
                        }
                    }
                    break;

                case "TXTQCSTDWORKHR":
                    if (this.pofrmEMStdWorkHour != null)
                    {
                        DataRow dtrPDGRP = this.pofrmEMStdWorkHour.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcStdWorkHr.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcStdWorkHr.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcStdWorkHr.Tag = "";
                            this.txtQcStdWorkHr.Text = "";
                        }
                    }
                    break;

            }
        }
        
        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select EMPlant.CROWID, EMPlant.CCODE, EMPlant.CNAME, EMPlant.CNAME2, EMPlant.DCREATE, EMPlant.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} EMPlant ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = EMPlant.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = EMPlant.CLASTUPDBY ";
            strSQLExec += " where EMPlant.CCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.SetPara(new object[] {App.ActiveCorp.RowID});
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "EMPlant", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QEMPlantInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QEMPlantInfo.Field.Code].Visible = true;
            this.gridView1.Columns[QEMPlantInfo.Field.Name].Visible = true;
            this.gridView1.Columns[QEMPlantInfo.Field.Name2].Visible = true;

            this.gridView1.Columns[QEMPlantInfo.Field.Code].Caption = "รหัสโรงงาน";
            this.gridView1.Columns[QEMPlantInfo.Field.Name].Caption = "ชื่อโรงงาน";
            this.gridView1.Columns[QEMPlantInfo.Field.Name2].Caption = "ชื่อโรงงานภาษา 2";

            this.gridView1.Columns[QEMPlantInfo.Field.Code].Width = 15;
            this.gridView1.Columns[QEMPlantInfo.Field.Name2].Width = 25;

            this.gridView1.Columns[QEMPlantInfo.Field.Code].VisibleIndex = 0;
            this.gridView1.Columns[QEMPlantInfo.Field.Name].VisibleIndex = 1;
            this.gridView1.Columns[QEMPlantInfo.Field.Name2].VisibleIndex = 2;

            this.pmSetSortKey(QEMPlantInfo.Field.Code, true);
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

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QEMPlantInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QEMPlantInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QEMPlantInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QEMPlantInfo.Field.Code].ToString(), dtrBrow[QEMPlantInfo.Field.Name].ToString(), ref strErrorMsg))
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
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID , this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "รหัส " +this.mstrFormMenuName + " ซ้ำ", "Duplicate " + this.mstrFormMenuName + " Code !" });
                this.txtCode.Focus();
                return false;
            }
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "ชื่อโรงงานซ้ำ";
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
            objSQLHelper.SetPara(new object[] {App.ActiveCorp.RowID});
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QEMPlantInfo.Field.Code].ToString().Trim();
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
            }

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QEMPlantInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QEMPlantInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QEMPlantInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QEMPlantInfo.Field.Name2] = this.txtName2.Text.TrimEnd();

            dtrSaveInfo["CWORKHOUR"] = this.txtQcStdWorkHr.Tag.ToString();
            dtrSaveInfo["CHOLIDAY"] = this.txtQcHoliday.Tag.ToString();
            dtrSaveInfo["CWORKCAL"] = "";
            dtrSaveInfo["CACTIVE"] = "";
            dtrSaveInfo["NCAPACITY"] = 0;

            dtrSaveInfo["CJOB"] = "";
            dtrSaveInfo["CPROJ"] = "";

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
            this.cmbType.SelectedIndex = 0;

            this.txtQcStdWorkHr.Tag = "";
            this.txtQcStdWorkHr.Text = "";

            this.txtQcHoliday.Tag = "";
            this.txtQcHoliday.Text = "";
        
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
                    this.txtCode.Text = dtrLoadForm[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QEMPlantInfo.Field.Name2].ToString().TrimEnd();

                    DataRow dtrLoadFormVar = null;
                    this.txtQcStdWorkHr.Tag = dtrLoadForm["CWORKHOUR"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["CWORKHOUR"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QLoadVar", QEMStdWorkHourInfo.TableName, "select * from " + QEMStdWorkHourInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrLoadFormVar = this.dtsDataEnv.Tables["QLoadVar"].Rows[0];
                        this.txtQcStdWorkHr.Text = dtrLoadFormVar["cCode"].ToString().TrimEnd();
                    }

                    this.txtQcHoliday.Tag = dtrLoadForm["CHOLIDAY"].ToString();
                    pobjSQLUtil.SetPara(new object[] { dtrLoadForm["CHOLIDAY"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QLoadVar", QEMHolidayInfo.TableName, "select * from " + QEMHolidayInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrLoadFormVar = this.dtsDataEnv.Tables["QLoadVar"].Rows[0];
                        this.txtQcHoliday.Text = dtrLoadFormVar["cCode"].ToString().TrimEnd();
                    }

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
            if (inOrderBy.ToUpper() == QEMPlantInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMPlantInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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

        private void txtQcHoliday_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCHOLIDAY" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcHoliday.Tag = "";
                this.txtQcHoliday.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("HOLIDAY");
                e.Cancel = !this.pofrmEMHoliday.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmEMHoliday.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcStdWorkHr_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSTDWORKHR" ? "CCODE" : "CNAME";


            if (txtPopUp.Text == "")
            {
                this.txtQcStdWorkHr.Tag = "";
                this.txtQcStdWorkHr.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("STDWORKHOUR");
                e.Cancel = !this.pofrmEMStdWorkHour.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmEMStdWorkHour.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

    }
}
