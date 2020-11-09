
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

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

    public partial class frmEMStdWorkHour : UIHelper.frmBase , IfrmDBBase, UIHelper.IAppMenu
    {

        public static string TASKNAME = "ESTDWKHR";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QEMStdWorkHourInfo.TableName;
        private string mstrITable = MapTable.Table.EMWorkHourItem_Range;

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QEMStdWorkHourInfo.Field.Code;
        private string mstrFixType = "";
        private string mstrFormMenuName = UIBase.GetAppUIText(new string[] { "ตารางทำงานมาตรฐาน", "Standard Work Hour" });

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private string mstrTemWkHour = "TemWkHour";
        private string mstrTemOTHour = "TemOTHour";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmEMStdWorkHour()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmEMStdWorkHour(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmEMStdWorkHour mInstanse = null;

        public static frmEMStdWorkHour GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmEMStdWorkHour();
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

            this.pmCreateTem();
            this.pmInitGridProp_TemWkHour();
            this.pmInitGridProp_TemOTHour();
            this.pmInitializeComponent();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();
        }

        public void RunMenu(object inParent, object[] args)
        {
            if (args != null)
            {
            }

            this.pmInitForm();

            System.Windows.Forms.Form oParent = inParent as System.Windows.Forms.Form;
            if (oParent != null)
            {
                this.MdiParent = oParent;
            }
            this.Show();
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

            for (int i = 0; i < MRPAgent.AThaiDate.Length; i++)
            {
                this.lsbDayList.Items.Add(UIBase.GetAppUIText(new string[] { MRPAgent.AThaiDate[i], MRPAgent.AEngDate[i] }));
            }
        }

        private void pmSetFormUI()
        {

            this.lblCode.Text = UIBase.GetAppUIText(new string[] { "รหัส :", "Code :" });
            this.lblName.Text = UIBase.GetAppUIText(new string[] { "ชื่อ :", "Name :" });
            this.lblName2.Text = UIBase.GetAppUIText(new string[] { "ชื่อภาษา 2 :", "Name 2:" });

            this.Text = this.mstrFormMenuName;

        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMStdWorkHourInfo.Field.Code);
            this.txtName.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMStdWorkHourInfo.Field.Name);
            this.txtName2.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, QEMStdWorkHourInfo.Field.Name2);
        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.gridView2.CellValueChanged += new CellValueChangedEventHandler(gridView2_CellValueChanged);
            this.gridView3.CellValueChanged += new CellValueChangedEventHandler(gridView3_CellValueChanged);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select EMWorkHr.CROWID, EMWorkHr.CCODE, EMWorkHr.CNAME, EMWorkHr.CNAME2, EMWorkHr.DCREATE, EMWorkHr.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} EMWorkHr ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = EMWorkHr.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = EMWorkHr.CLASTUPDBY ";
            strSQLExec += " where EMWorkHr.CCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "EMWorkHr", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = QEMStdWorkHourInfo.Field.Code;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Code].Caption = "Code";
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Name].Caption = "Name";
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Name2].Caption = "Name 2";

            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Code].Width = 15;
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Name2].Width = 25;

            int i = 0;
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Code].VisibleIndex = i++;
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Name].VisibleIndex = i++;
            this.gridView1.Columns[QEMStdWorkHourInfo.Field.Name2].VisibleIndex = i++;

            this.pmSetSortKey(QEMStdWorkHourInfo.Field.Code, true);
        }

        private void pmInitGridProp_TemWkHour()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemWkHour].DefaultView;

            this.grdTemWorkHour.DataSource = this.dtsDataEnv.Tables[this.mstrTemWkHour];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView2.Columns["cDay"].VisibleIndex = i++;
            this.gridView2.Columns["dBegTime"].VisibleIndex = i++;
            this.gridView2.Columns["dEndTime"].VisibleIndex = i++;
            this.gridView2.Columns["nTotHour"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["dBegTime"].Caption = UIBase.GetAppUIText(new string[] { "ตั้งแต่", "Start" });
            this.gridView2.Columns["dEndTime"].Caption = UIBase.GetAppUIText(new string[] { "ถึง", "End" });
            this.gridView2.Columns["nTotHour"].Caption = UIBase.GetAppUIText(new string[] { "ชม.", "Hour" });

            this.gridView2.Columns["nRecNo"].Width = 30;
            this.gridView2.Columns["dBegTime"].Width = 80;
            this.gridView2.Columns["dEndTime"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["nTotHour"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nTotHour"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nTotHour"].OptionsColumn.ReadOnly = true;
            this.gridView2.Columns["nTotHour"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nTotHour"].DisplayFormat.FormatString = "{0:#,###,##0.0;-#,###,##0.0; }";

            this.gridView2.Columns["dBegTime"].ColumnEdit = this.grcBegTime;
            this.gridView2.Columns["dEndTime"].ColumnEdit = this.grcEndTime;

            this.gridView2.Columns["nTotHour"].SummaryItem.DisplayFormat = "{0:#,###,##0.0;-#,###,##0.0; }";
            this.gridView2.Columns["nTotHour"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //this.pmCalcColWidth();

        }

        private void pmInitGridProp_TemOTHour()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemOTHour].DefaultView;

            this.grdTemOTHour.DataSource = this.dtsDataEnv.Tables[this.mstrTemOTHour];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView3.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView3.Columns["cDay"].VisibleIndex = i++;
            this.gridView3.Columns["dBegTime"].VisibleIndex = i++;
            this.gridView3.Columns["dEndTime"].VisibleIndex = i++;
            this.gridView3.Columns["nTotHour"].VisibleIndex = i++;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["dBegTime"].Caption = UIBase.GetAppUIText(new string[] { "ตั้งแต่", "Start" });
            this.gridView3.Columns["dEndTime"].Caption = UIBase.GetAppUIText(new string[] { "ถึง", "End" });
            this.gridView3.Columns["nTotHour"].Caption = UIBase.GetAppUIText(new string[] { "ชม.", "Hour" });

            this.gridView3.Columns["nRecNo"].Width = 30;
            this.gridView3.Columns["dBegTime"].Width = 80;
            this.gridView3.Columns["dEndTime"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nTotHour"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nTotHour"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nTotHour"].OptionsColumn.ReadOnly = true;
            this.gridView3.Columns["nTotHour"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nTotHour"].DisplayFormat.FormatString = "{0:#,###,##0.0;-#,###,##0.0; }";

            this.gridView3.Columns["dBegTime"].ColumnEdit = this.grcBegTime;
            this.gridView3.Columns["dEndTime"].ColumnEdit = this.grcEndTime;

            this.gridView3.Columns["nTotHour"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nTotHour"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

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
            //switch (inDialogName.TrimEnd().ToUpper())
            //{
            //}
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
            //switch (inTextbox)
            //{
            //}
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            //DataRow dtrGetVal = null;
            //switch (inPopupForm.TrimEnd().ToUpper())
            //{

            //    //case "TXTQCMCTYPE":
            //    //case "TXTQNMCTYPE":

            //    //    dtrGetVal = this.pofrmGetMCType.RetrieveValue();
            //    //    if (dtrGetVal != null)
            //    //    {
            //    //        this.txtQcMCType.Tag = dtrGetVal["cRowID"].ToString();
            //    //        this.txtQcMCType.Text = dtrGetVal["cCode"].ToString().TrimEnd();
            //    //        this.txtQnMCType.Text = dtrGetVal["cName"].ToString().TrimEnd();
            //    //    }
            //    //    else
            //    //    {
            //    //        this.txtQcMCType.Tag = "";
            //    //        this.txtQcMCType.Text = "";
            //    //        this.txtQnMCType.Text = "";
            //    //    }
            //    //    break;
            //}
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
            string strDeleteDesc = "(" + dtrBrow[QEMStdWorkHourInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QEMStdWorkHourInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QEMStdWorkHourInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QEMStdWorkHourInfo.Field.Code].ToString(), dtrBrow[QEMStdWorkHourInfo.Field.Name].ToString(), ref strErrorMsg))
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
            bool bllHasUsed = false;
            //if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
            //    && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select cCode, cName from " + QEMSectInfo.TableName + " where cDept = ? ", ref strErrorMsg))
            //{
            //    strMsg2 = "(" + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"].ToString().TrimEnd() + ") " + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cName"].ToString().TrimEnd();
            //    ioErrorMsg = strMsg1 + "มีแผนก " + strMsg2 + " อ้างถึงแล้ว";
            //}
            //else
            //{
            //    bllHasUsed = false;
            //}
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
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWkHourH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID , this.txtCode.Text.TrimEnd() }))
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
            objSQLHelper.SetPara(new object[] {App.ActiveCorp.RowID});
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QEMStdWorkHourInfo.Field.Code].ToString().Trim();
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

            dtrSaveInfo[QEMStdWorkHourInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Name] = this.txtName.Text.TrimEnd();
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Name2] = this.txtName2.Text.TrimEnd();

            for (int i = 0; i < MRPAgent.AEngVShortDate.Length; i++)
            {
                dtrSaveInfo["HR_D" + (i + 1).ToString("0")] = this.pmGetWorkHour(this.mstrTemWkHour, MRPAgent.AEngVShortDate[i]);
                dtrSaveInfo["OT_D" + (i + 1).ToString("0")] = this.pmGetWorkHour(this.mstrTemOTHour, MRPAgent.AEngVShortDate[i]);
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

                this.pmUpdateWorkHourIT(this.mstrTemWkHour, "");
                this.pmUpdateWorkHourIT(this.mstrTemOTHour, "O");

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

        private void pmUpdateWorkHourIT(string inTem, string inType)
        {

            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inTem].Rows)
            {

                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (Convert.ToDecimal(dtrTemPd["nTotHour"]) != 0)
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

                    this.pmReplRecordWkHourIT(bllIsNewRow, dtrTemPd, ref dtrBudCI, inType);

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

        private bool pmReplRecordWkHourIT(bool inState, DataRow inTemPd, ref DataRow ioWorkHourIT, string inType)
        {

            bool bllIsNewRec = inState;

            DataRow dtrWorkHourIT = ioWorkHourIT;
            DataRow dtrStdWorkHr = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(inTemPd["dBegTime"]).Hour, Convert.ToDateTime(inTemPd["dBegTime"]).Minute, 0);
            DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(inTemPd["dEndTime"]).Hour, Convert.ToDateTime(inTemPd["dEndTime"]).Minute, 0);

            dtrWorkHourIT["cCorp"] = App.ActiveCorp.RowID;
            dtrWorkHourIT["cWkHourH"] = dtrStdWorkHr["cRowID"].ToString();
            dtrWorkHourIT["cType"] = inType;
            dtrWorkHourIT["cDay"] = inTemPd["cDay"].ToString();
            dtrWorkHourIT["dBEGTIME"] = dttBegTime;
            dtrWorkHourIT["dENDTIME"] = dttEndTime;
            dtrWorkHourIT["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemWkHour);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDay", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nTotHour"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemWkHour_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemOT = new DataTable(this.mstrTemOTHour);
            dtbTemOT.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemOT.Columns.Add("cDay", System.Type.GetType("System.String"));
            dtbTemOT.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemOT.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemOT.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemOT.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemOT.Columns["cRowID"].DefaultValue = "";
            dtbTemOT.Columns["nTotHour"].DefaultValue = 0;

            //dtbTemOT.TableNewRow += new DataTableNewRowEventHandler(dtbTemWkHour_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemOT);
        
        }

        private void dtbTemWkHour_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            //e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            //e.Row["dDate"] = new DateTime(DateTime.Now.Year, 1, 1);
        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            //this.txtCode.Focus();
            this.txtName.Focus();
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

            this.lsbDayList.SelectedIndex = 0;
            this.pmCreateTemWorkHour(false);
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
                    this.txtCode.Text = dtrLoadForm[QEMStdWorkHourInfo.Field.Code].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm[QEMStdWorkHourInfo.Field.Name].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm[QEMStdWorkHourInfo.Field.Name2].ToString().TrimEnd();

                    this.pmCreateTemWorkHour(true);

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldName = this.txtName.Text;
        }

        private void pmCreateTemWorkHour(bool inLoadVal)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemOTHour].Rows.Clear();

            for (int i = 0; i < MRPAgent.AThaiDate.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    DataRow dtrRngDate = this.dtsDataEnv.Tables[this.mstrTemWkHour].NewRow();
                    dtrRngDate["nRecNo"] = j + 1;
                    dtrRngDate["cDay"] = MRPAgent.AEngVShortDate[i];

                    if (inLoadVal)
                    {
                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrEditRowID, MRPAgent.AEngVShortDate[i], StringHelper.ConvertToBase64(Convert.ToInt32(dtrRngDate["nRecNo"]), 2) });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWKHourIT", "", "select * from " + this.mstrITable + " where cCorp = ? and cWKHourH = ? and cType = '' and cDay = ? and cSeq = ? ", ref strErrorMsg))
                        {
                            DataRow dtrWKHourIT = this.dtsDataEnv.Tables["QWKHourIT"].Rows[0];
                            dtrRngDate["cRowID"] = dtrWKHourIT["cRowID"].ToString();

                            DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Minute, 0);
                            DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Minute, 0);
                            TimeSpan tsTotHour = dttEndTime - dttBegTime;

                            dtrRngDate["dBegTime"] = dttBegTime;
                            dtrRngDate["dEndTime"] = dttEndTime;
                            dtrRngDate["nTotHour"] = tsTotHour.TotalHours;
                        
                        }

                    }

                    this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows.Add(dtrRngDate);
                }

                for (int j = 0; j < 2; j++)
                {
                    DataRow dtrRngDate = this.dtsDataEnv.Tables[this.mstrTemOTHour].NewRow();
                    dtrRngDate["nRecNo"] = j + 1;
                    dtrRngDate["cDay"] = MRPAgent.AEngVShortDate[i];

                    if (inLoadVal)
                    {
                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrEditRowID, MRPAgent.AEngVShortDate[i], StringHelper.ConvertToBase64(Convert.ToInt32(dtrRngDate["nRecNo"]), 2) });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWKHourIT", "", "select * from " + this.mstrITable + " where cCorp = ? and cWKHourH = ? and cType = 'O' and cDay = ? and cSeq = ? ", ref strErrorMsg))
                        {
                            DataRow dtrWKHourIT = this.dtsDataEnv.Tables["QWKHourIT"].Rows[0];
                            dtrRngDate["cRowID"] = dtrWKHourIT["cRowID"].ToString();

                            DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Minute, 0);
                            DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Minute, 0);
                            TimeSpan tsTotHour = dttEndTime - dttBegTime;

                            dtrRngDate["dBegTime"] = dttBegTime;
                            dtrRngDate["dEndTime"] = dttEndTime;
                            dtrRngDate["nTotHour"] = tsTotHour.TotalHours;

                        }

                    }
                    
                    this.dtsDataEnv.Tables[this.mstrTemOTHour].Rows.Add(dtrRngDate);
                }

            }

            this.pmSumWorkHour();

        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }


        private void pmLoadHolidayLine()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrEditRowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QHolidatIT", this.mstrITable, "select * from " + this.mstrITable + " where cCorp = ? and cHolidayH = ? order by dDate", ref strErrorMsg);

            int intRow = 0;
            foreach (DataRow dtrWkCtrIT in this.dtsDataEnv.Tables["QHolidatIT"].Rows)
            {
                DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemWkHour].NewRow();

                intRow++;
                //dtrNewRow["nRecNo"] = intRow;
                dtrNewRow["cRowID"] = dtrWkCtrIT["cRowID"].ToString();
                dtrNewRow["dDate"] = Convert.ToDateTime(dtrWkCtrIT["dDate"]);
                dtrNewRow["cRemark"] = dtrWkCtrIT["cRemark"].ToString().TrimEnd();

                this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows.Add(dtrNewRow);
            }
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
            if (inOrderBy.ToUpper() == QEMStdWorkHourInfo.Field.Code)
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
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMStdWorkHourInfo.Field.Code ? this.txtCode.Properties.MaxLength : this.txtName.Properties.MaxLength);
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

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        private void txtDate_Leave(object sender, EventArgs e)
        {
        }

        private void lsbDayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFilterVal = MRPAgent.AEngVShortDate[this.lsbDayList.SelectedIndex];
            this.gridView2.Columns["cDay"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo(DevExpress.XtraGrid.Columns.ColumnFilterType.Custom, null, "[cDay] = '" + strFilterVal + "' ", "");
            this.gridView3.Columns["cDay"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo(DevExpress.XtraGrid.Columns.ColumnFilterType.Custom, null, "[cDay] = '" + strFilterVal + "' ", "");
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(e.RowHandle);
            if (e.RowHandle > -1 && dtrTemPd != null)
            {
                if (!Convert.IsDBNull(dtrTemPd["dBegTime"]) && !Convert.IsDBNull(dtrTemPd["dEndTime"]))
                {
                    DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dBegTime"]).Hour, Convert.ToDateTime(dtrTemPd["dBegTime"]).Minute, 0);
                    DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dEndTime"]).Hour, Convert.ToDateTime(dtrTemPd["dEndTime"]).Minute, 0);
                    TimeSpan tsTotHour = dttEndTime - dttBegTime;
                    dtrTemPd["nTotHour"] = tsTotHour.TotalHours;

                }
                else
                {
                    dtrTemPd["nTotHour"] = 0;
                }
                this.gridView2.UpdateSummary();
                this.pmSumWorkHour();

            }
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow dtrTemPd = this.gridView3.GetDataRow(e.RowHandle);
            if (e.RowHandle > -1 && dtrTemPd != null)
            {
                if (!Convert.IsDBNull(dtrTemPd["dBegTime"]) && !Convert.IsDBNull(dtrTemPd["dEndTime"]))
                {
                    DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dBegTime"]).Hour, Convert.ToDateTime(dtrTemPd["dBegTime"]).Minute, 0);
                    DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dEndTime"]).Hour, Convert.ToDateTime(dtrTemPd["dEndTime"]).Minute, 0);
                    TimeSpan tsTotHour = dttEndTime - dttBegTime;
                    dtrTemPd["nTotHour"] = tsTotHour.TotalHours;

                }
                else
                {
                    dtrTemPd["nTotHour"] = 0;
                }
                this.gridView3.UpdateSummary();
                this.pmSumWorkHour();

            }
        }

        private void btnGrpUpdate_Click(object sender, EventArgs e)
        {
            string strFilterVal = MRPAgent.AEngVShortDate[this.lsbDayList.SelectedIndex];
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemWkHour].AsEnumerable()
                          where p1["cDay"].ToString() == strFilterVal
                          select new { TimeSpan = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TimeSpan));

            MessageBox.Show(decSumWorkHr.ToString());

        }

        private decimal pmGetWorkHour(string inTable, string inDay)
        {
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[inTable].AsEnumerable()
                                    where p1["cDay"].ToString() == inDay
                                    select new { TimeSpan = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TimeSpan));
            return decSumWorkHr;
        }

        private void pmSumWorkHour()
        {
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemWkHour].AsEnumerable()
                                    select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemOTHour].AsEnumerable()
                                  select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            this.txtSumWkHour.Value = decSumWorkHr;
            this.txtSumOTWkHour.Value = decSumOTHr;
        }

    }
}
