﻿
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
    public partial class frmEMSect : UIHelper.frmBase, IfrmDBBase
    {

        public static string TASKNAME = "ESECT";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QEMSectInfo.TableName;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QEMSectInfo.Field.Code;

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

        //private DatabaseForms.frmEMDept pofrmGetDept = null;
        private UIHelper.IfrmDBBase pofrmGetDept = null;

        public frmEMSect()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmEMSect(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmEMSect mInstanse = null;

        public static frmEMSect GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmEMSect();
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

            this.pmCreateBrowView();

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMSectInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMSectInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMSectInfo.Field.Name2);

            this.cmbType.Properties.Items.Clear();
            this.cmbType.Properties.Items.AddRange(new object[] { "D = DETAIL", "G = GROUP" });
            this.cmbType.SelectedIndex = 0;
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

            string strSQLExec = "select EMSECT.FCSKID, EMSECT.FCCODE, EMSECT.FCNAME, EMSECT.FCNAME2, EMSECT.FTLASTUPD as FDCREATE, EMSECT.FTDATETIME as FDLASTUPDBY from {0} EMSECT ";
            //strSQLExec += " left join {1} EM1 ON EM1.fcSkid = EMSECT.CCREATEBY ";
            //strSQLExec += " left join {1} EM2 ON EM2.fcSkid = EMSECT.CLASTUPDBY ";
            strSQLExec += " where EMSECT.FCCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            pobjSQLUtil.SetPara(new object[] {App.ActiveCorp.RowID});
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "EMSECT", strSQLExec, ref strErrorMsg);

        }

        private void pmSetBrowViewByAuth()
        {
            string strErrorMsg = "";
            string strSQLStr = "";
            string[] staPara = { this.mstrRefTable };
            string strSearchStr = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select EMSECT.FCSKID, EMSECT.FCCODE, EMSECT.FCNAME, EMSECT.FCNAME2, EMSECT.FTLASTUPD as FDCREATE, EMSECT.FTDATETIME as FDLASTUPDBY from {0} EMSECT ";
            //strSQLExec += " left join {1} EM1 ON EM1.fcSkid = EMSECT.CCREATEBY ";
            //strSQLExec += " left join {1} EM2 ON EM2.fcSkid = EMSECT.CLASTUPDBY ";
            strSQLExec += " where EMSECT.FCCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });
            
            pobjSQLUtil.SetPara(new object[1] { App.ActiveCorp.RowID });

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBrow", "EMSECT", strSQLExec, ref strErrorMsg);

            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Clear();
            foreach (DataRow dtrBrow in this.dtsDataEnv.Tables["QBrow"].Rows)
            {
                if (App.PermissionManager.CheckPermissionBySect(AuthenType.CanAccess, App.AppUserName, App.AppUserID, dtrBrow["fcSkid"].ToString()))
                {
                    DataRow dtrBrowView = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();
                    dtrBrowView[MapTable.ShareField.RowID] = dtrBrow[MapTable.ShareField.RowID].ToString();
                    dtrBrowView[QEMSectInfo.Field.Code] = dtrBrow[QEMSectInfo.Field.Code].ToString();
                    dtrBrowView[QEMSectInfo.Field.Name] = dtrBrow[QEMSectInfo.Field.Name].ToString();
                    dtrBrowView[QEMSectInfo.Field.Name2] = dtrBrow[QEMSectInfo.Field.Name2].ToString();

                    //if (!Convert.IsDBNull(dtrBrow["DCREATE"]))
                    //{
                    //    dtrBrowView["DCREATE"] = Convert.ToDateTime(dtrBrow["DCREATE"]);
                    //}

                    //if (!Convert.IsDBNull(dtrBrow["DLASTUPDBY"]))
                    //{
                    //    dtrBrowView["DLASTUPDBY"] = Convert.ToDateTime(dtrBrow["DLASTUPDBY"]);
                    //}
                    
                    //dtrBrowView["CLOGIN_ADD"] = (Convert.IsDBNull(dtrBrow["CLOGIN_ADD"]) ? "" : dtrBrow["CLOGIN_ADD"].ToString());
                    //dtrBrowView["CLOGIN_UPD"] = (Convert.IsDBNull(dtrBrow["CLOGIN_UPD"]) ? "" : dtrBrow["CLOGIN_UPD"].ToString());
                    
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrBrowView);
                }
            }
            //dlg.WaitClear();
        }

        private void pmCreateBrowView()
        {
            DataTable dtbTemPd = new DataTable(this.mstrBrowViewAlias);
            dtbTemPd.Columns.Add(MapTable.ShareField.RowID, System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add(QEMSectInfo.Field.Code, System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add(QEMSectInfo.Field.Name, System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add(QEMSectInfo.Field.Name2, System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("DCREATE", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("DLASTUPDBY", System.Type.GetType("System.DateTime"));
            //dtbTemPd.Columns.Add("CLOGIN_ADD", System.Type.GetType("System.String"));
            //dtbTemPd.Columns.Add("CLOGIN_UPD", System.Type.GetType("System.String"));

            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QEMSectInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QEMSectInfo.Field.Code].Visible = true;
            this.gridView1.Columns[QEMSectInfo.Field.Name].Visible = true;
            this.gridView1.Columns[QEMSectInfo.Field.Name2].Visible = true;

            this.gridView1.Columns[QEMSectInfo.Field.Code].Caption = "รหัสแผนก";
            this.gridView1.Columns[QEMSectInfo.Field.Name].Caption = "ชื่อแผนก";
            this.gridView1.Columns[QEMSectInfo.Field.Name2].Caption = "ชื่อแผนกภาษา 2";

            this.gridView1.Columns[QEMSectInfo.Field.Code].Width = 15;
            this.gridView1.Columns[QEMSectInfo.Field.Name2].Width = 25;

            this.gridView1.Columns[QEMSectInfo.Field.Code].VisibleIndex = 0;
            this.gridView1.Columns[QEMSectInfo.Field.Name].VisibleIndex = 1;
            this.gridView1.Columns[QEMSectInfo.Field.Name2].VisibleIndex = 2;

            this.pmSetSortKey(QEMSectInfo.Field.Code, true);
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
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.pmSetBrowViewByAuth();
            }
            else
            {
                this.pmSetBrowView();
            }
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
                case "DEPT":
                    if (this.pofrmGetDept == null)
                    {
                        this.pofrmGetDept = new frmEMDept(FormActiveMode.PopUp);
                        //this.pofrmGetDept.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDept.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCDEPT":
                case "TXTQNDEPT":
                    this.pmInitPopUpDialog("DEPT");
                    strPrefix = (inTextbox == "TXTQCPROJ" ? "FCCODE" : "FCNAME");
                    this.pofrmGetDept.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetDept.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            DataRow dtrTemPd = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "TXTQCDEPT":
                case "TXTQNDEPT":

                    if (this.pofrmGetDept != null)
                    {
                        DataRow dtrJob = this.pofrmGetDept.RetrieveValue();
                        this.txtQcDept.Tag = dtrJob["fcSkid"].ToString();
                        this.txtQcDept.Text = dtrJob["fcCode"].ToString().TrimEnd();
                        this.txtQnDept.Text = dtrJob["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcDept.Tag = "";
                        this.txtQcDept.Text = "";
                        this.txtQnDept.Text = "";
                    }
                    break;
            }
        }

        private void txtQcProj_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCDEPT" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcDept.Tag = "";
                this.txtQcDept.Text = "";
                this.txtQnDept.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("DEPT");
                e.Cancel = !this.pofrmGetDept.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetDept.PopUpResult)
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

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QEMSectInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QEMSectInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QEMSectInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลแผนก : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QEMSectInfo.Field.Code].ToString(), dtrBrow[QEMSectInfo.Field.Name].ToString(), ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from " + MapTable.Table.BudTranHD + " where cSect = ? ", ref strErrorMsg))
            {
                //strMsg2 = "(" + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"].ToString().TrimEnd() + ") " + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cName"].ToString().TrimEnd();
                ioErrorMsg = strMsg1 + "มี Transaction อ้างถึงแล้ว";
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
                if (MessageBox.Show("ยังไม่ได้ระบุรหัสแผนก ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสแผนก";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุ ชื่อแผนก";
                this.txtName.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID , this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสแผนกซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "ชื่อแผนกซ้ำ";
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
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QEMSectInfo.Field.Code].ToString().Trim();
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

            dtrSaveInfo[QEMSectInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QEMSectInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QEMSectInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QEMSectInfo.Field.Name2] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo[QEMSectInfo.Field.DeptID] = this.txtQcDept.Tag.ToString();

            //string strLevel = "";
            //switch (this.cmbType.SelectedIndex)
            //{
            //    case 0:
            //        strLevel = "D";
            //        break;
            //    case 1:
            //        strLevel = "G";
            //        break;
            //}
            //dtrSaveInfo[QEMSectInfo.Field.Type] = strLevel;

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

            this.txtQcDept.Tag = ""; this.txtQcDept.Text = "";
            this.txtQnDept.Text = "";

            this.cmbType.SelectedIndex = 0;
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
                    this.txtCode.Text = dtrLoadForm[QEMSectInfo.Field.Code].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm[QEMSectInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QEMSectInfo.Field.Name2].ToString().TrimEnd();

                    this.txtQcDept.Tag = dtrLoadForm[QEMSectInfo.Field.DeptID].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcDept.Tag });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProj", QEMDeptInfo.TableName, "select * from " + QEMDeptInfo.TableName + " where fcSkid = ?", ref strErrorMsg))
                    {
                        this.txtQcDept.Text = this.dtsDataEnv.Tables["QProj"].Rows[0][QEMDeptInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnDept.Text = this.dtsDataEnv.Tables["QProj"].Rows[0][QEMDeptInfo.Field.Name].ToString().TrimEnd();
                    }

                    //string strLevel = dtrLoadForm[QEMSectInfo.Field.Type].ToString().TrimEnd();
                    //int intSel = -1;
                    //switch (strLevel)
                    //{
                    //    case "D":
                    //        intSel = 0;
                    //        break;
                    //    case "G":
                    //        intSel = 1;
                    //        break;
                    //}
                    //this.cmbType.SelectedIndex = intSel;

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
            if (inOrderBy.ToUpper() == QEMSectInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMSectInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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