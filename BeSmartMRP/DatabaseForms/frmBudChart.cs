
#define xd_RUNMODE_DEBUG

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

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.DatabaseForms
{

    public partial class frmBudChart : UIHelper.frmBase , UIHelper.IfrmDBBase
    {
 
        public static string TASKNAME = "EBGCHART";

        public static int MAXLENGTH_CODE = 11;
        public static int MAXLENGTH_NAME = 200;
        public static int MAXLENGTH_NAME2 = 200;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";

        private string mstrRefTable = MapTable.Table.BudgetChart;
        private string mstrITable = MapTable.Table.BudgetChartIT;

        private string mstrTemPd = "TemPd";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";
        private int mstrOldType = -1;

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        //private string mstrFixType = "D";
        private string mstrFixType = "";
        private string mstrGetBrowLevel = "";

        private string mstrBGType = "";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        //private DatabaseForms.frmBudType pofrmGetBudType = null;
        //private DialogForms.dlgGetAcChart pofrmGetAcChart = null;

        private DatabaseForms.frmBudType pofrmGetBudType = null;
        private DatabaseForms.frmEMAcChart pofrmGetAcChart = null;
        private DatabaseForms.frmBudChart pofrmGetBudChart = null;

        public frmBudChart()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmBudChart(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        public frmBudChart(FormActiveMode inMode, string inFixType)
        {
            InitializeComponent();

            this.mstrFixType = inFixType;
            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmBudChart mInstanse = null;

        public static frmBudChart GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmBudChart();
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
            this.pmInitGridProp_TemPd();

            this.pmInitializeComponent();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            this.txtCode.Properties.MaxLength = MAXLENGTH_CODE;
            this.txtName.Properties.MaxLength = MAXLENGTH_NAME;
            this.txtName2.Properties.MaxLength = MAXLENGTH_NAME2;
            this.txtRemark.Properties.MaxLength = 150;

            this.cmbType.Properties.Items.Clear();
            this.cmbType.Properties.Items.AddRange(new object[] { "D = รายการย่อย", "G = กลุ่ม" });
            this.cmbType.SelectedIndex = 0;

            //this.txtQcBudType.Properties.MaxLength = frmBudType.MAXLENGTH_CODE;
            //this.txtQnBudType.Properties.MaxLength = frmBudType.MAXLENGTH_NAME;
    
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

            //string strCriteria = (this.mFormActiveMode == FormActiveMode.Edit || this.mFormActiveMode == FormActiveMode.Report ? "" : " and BGCHARTHD.CTYPE = '" + this.mstrFixType + "' and BGCHARTHD.CCODE like ? ");
            string strCriteria = "";

            if (this.mstrFixType != string.Empty)
            {
                strCriteria = " and BGCHARTHD.CTYPE = '" + this.mstrFixType + "' and BGCHARTHD.CCODE like ? ";
            }
            else
            {
                strCriteria = (this.mFormActiveMode == FormActiveMode.Edit || this.mFormActiveMode == FormActiveMode.Report ? "" : " BGCHARTHD.CCODE like ? ");
            }

            string strSQLExec = "select BGCHARTHD.CROWID, BGCHARTHD.CTYPE, BGCHARTHD.CCODE, BGCHARTHD.CNAME, BGCHARTHD.DCREATE, BGCHARTHD.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} BGCHARTHD";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = BGCHARTHD.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = BGCHARTHD.CLASTUPDBY ";
            strSQLExec += " where {0}.CCORP = ? " + strCriteria;
            if (this.mstrBGType != string.Empty)
            {
                strSQLExec += " and {0}.CBGTYPE = ? ";
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBGType , this.mstrSearchStr });
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrSearchStr });
            }


            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            if (this.mFormActiveMode == FormActiveMode.Edit)
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            }
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "BudType", strSQLExec, ref strErrorMsg))
            {
            }
            else
            {
                this.mstrSearchStr = "%%";
                if (this.mstrBGType != string.Empty)
                {
                    pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBGType, this.mstrSearchStr });
                }
                else
                {
                    pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrSearchStr });
                }

                if (this.mFormActiveMode == FormActiveMode.Edit)
                {
                    pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
                }
                
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "BudType", strSQLExec, ref strErrorMsg);
            }

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

            this.gridView1.Columns["CTYPE"].Visible = true;
            this.gridView1.Columns["CCODE"].Visible = true;
            this.gridView1.Columns["CNAME"].Visible = true;

            this.gridView1.Columns["CTYPE"].Caption = "";
            this.gridView1.Columns["CCODE"].Caption = "รหัสประเภทงบ";
            this.gridView1.Columns["CNAME"].Caption = "ชื่อประเภทงบ";
            //this.gridView1.Columns["CSNAME"].Caption = "ชื่อย่อ";

            this.gridView1.Columns["CTYPE"].Width = 25;
            this.gridView1.Columns["CCODE"].Width = 70;

            this.gridView1.Columns["CTYPE"].VisibleIndex = 0;
            this.gridView1.Columns["CCODE"].VisibleIndex = 1;
            this.gridView1.Columns["CNAME"].VisibleIndex = 2;

            this.pmSetSortKey("cCode", true);

            if (this.mFormActiveMode == FormActiveMode.PopUp || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.gridView1.Columns["CCODE"].Width = 120;
            }

            this.pmRecalColWidth();
        
        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmRecalColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmRecalColWidth();
        }

        private void pmRecalColWidth()
        {
            int intSumColWidth = this.gridView1.Columns["CTYPE"].Width
                + this.gridView1.Columns["CCODE"].Width;

            int intColWidth = this.grdBrowView.Width - intSumColWidth - 30;
            this.gridView1.Columns["CNAME"].Width = (intColWidth < 120 ? 120 : intColWidth);
        }
        
        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = true;
            }

            this.gridView2.Columns["nRecNo"].Visible = false;
            this.gridView2.Columns["cRowID"].Visible = false;
            this.gridView2.Columns["cAcChart"].Visible = false;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cQcAcChart"].Caption = "รหัสผังบัญชี";
            this.gridView2.Columns["cQnAcChart"].Caption = "ชื่อผังบัญชี";
            this.gridView2.Columns["cRemark"].Caption = "หมายเหตุ";

            this.gridView2.Columns["nRecNo"].Width = 5;
            this.gridView2.Columns["cQcAcChart"].Width = 15;
            this.gridView2.Columns["cQnAcChart"].Width = 45;

            this.grcQcAcChart.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_CODE;
            this.grcQnAcChart.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_NAME;
            this.grcRemark.MaxLength = 150;

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQcAcChart"].ColumnEdit = this.grcQcAcChart;
            this.gridView2.Columns["cQnAcChart"].ColumnEdit = this.grcQnAcChart;
            this.gridView2.Columns["cRemark"].ColumnEdit = this.grcRemark;

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
                case "BUDTYPE":
                    if (this.pofrmGetBudType == null)
                    {
                        this.pofrmGetBudType = new DatabaseForms.frmBudType(FormActiveMode.Report);
                        this.pofrmGetBudType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBudType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BUDCHART":
                    if (this.pofrmGetBudChart == null)
                    {
                        this.pofrmGetBudChart = new DatabaseForms.frmBudChart(FormActiveMode.Report, "G");
                        this.pofrmGetBudChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBudChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "ACCHART":
                    if (this.pofrmGetAcChart == null)
                    {
                        //this.pofrmGetAcChart = new DialogForms.dlgGetAcChart();
                        this.pofrmGetAcChart = new frmEMAcChart(FormActiveMode.PopUp);
                        this.pofrmGetAcChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetAcChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            switch (inTextbox)
            {
                case "TXTQCBUDTYPE":
                case "TXTQNBUDTYPE":
                    this.pmInitPopUpDialog("BUDTYPE");
                    string strPrefix = (inTextbox == "TXTQCBUDTYPE" ? "CCODE" : "CNAME");
                    this.pofrmGetBudType.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetBudType.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCBUDCHART":
                case "TXTQNBUDCHART":
                    this.pmInitPopUpDialog("BUDCHART");
                    this.pofrmGetBudChart.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetBudChart.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "QCACCHART":
                case "QNACCHART":
                    this.pmInitPopUpDialog("ACCHART");
                    strPrefix = (inTextbox == "TXTQCBUDTYPE" ? QEMAcChartInfo.Field.Code : QEMAcChartInfo.Field.Name);
                    this.pofrmGetAcChart.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetAcChart.PopUpResult)
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
                case "TXTQCBUDTYPE":
                case "TXTQNBUDTYPE":
                    if (this.pofrmGetBudType != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetBudType.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcBudType.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcBudType.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnBudType.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBudType.Tag = "";
                            this.txtQcBudType.Text = "";
                            this.txtQnBudType.Text = "";
                        }
                    }
                    break;
                case "TXTQCBUDCHART":
                case "TXTQNBUDCHART":
                    if (this.pofrmGetBudChart != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetBudChart.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcBudChart.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcBudChart.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnBudChart.Text = dtrPDGRP["cName"].ToString().TrimEnd();

                            if (this.txtCode.Text.Trim() == string.Empty)
                            {
                                this.txtCode.Text = this.txtQcBudChart.Text;
                            }
                        
                        }
                        else
                        {
                            this.txtQcBudChart.Tag = "";
                            this.txtQcBudChart.Text = "";
                            this.txtQnBudChart.Text = "";
                        }
                    }
                    break;
                case "GRDVIEW2_CQCACCHART":
                case "GRDVIEW2_CQNACCHART":
                    DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetAcChart != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();
                        if (dtrAcChart != null)
                        {
                            dtrTemPd["cAcChart"] = dtrAcChart[MapTable.ShareField.RowID].ToString();
                            dtrTemPd["cQcAcChart"] = dtrAcChart[QEMAcChartInfo.Field.Code].ToString().TrimEnd();
                            dtrTemPd["cQnAcChart"] = dtrAcChart[QEMAcChartInfo.Field.Name].ToString().TrimEnd();
                        }
                        else
                        {
                            dtrTemPd["cAcChart"] = "";
                            dtrTemPd["cQcAcChart"] = "";
                            dtrTemPd["cQnAcChart"] = "";
                        }

                        this.gridView2.UpdateCurrentRow();
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
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cName"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลผังงบประมาณ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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
            //TODO: Check Corp Has Used
            bool bllHasChild = false;

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = true;
            if (this.pmHasChild())
            {
                ioErrorMsg = strMsg1 + "มีการอ้างอิงไปเป็นบัญชีคุมแล้ว";
            }
            else if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from BGTRANIT where cBGChartHD = ? ", ref strErrorMsg))
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
            
            string strErrorMsg = "";

            object[] pAPara = null;
            bool bllIsCommit = false;
            bool bllResult = false;
            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cBGChartHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

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
                if (MessageBox.Show("ยังไม่ได้ระบุรหัสผังงบประมาณ ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (this.txtQcBudChart.Text.Trim() == string.Empty)
                    {
                        this.pmRunCode();
                    }
                    else
                    {
                        this.pmRunCode2();
                    }
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสผังงบประมาณ";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุ ชื่อผังงบประมาณ";
                this.txtName.Focus();
                return false;
            }
            else if (this.txtQcBudType.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุ ประเภทงบประมาณ";
                this.txtQcBudType.Focus();
                return false;
            }
            else if (this.mFormEditMode == UIHelper.AppFormState.Edit
                && this.mstrEditRowID == this.txtQcBudChart.Tag.ToString())
            {
                ioErrorMsg = "ไม่สามารถระบุรหัสแผนงานคุม เป็นตัวเดียวกันกับรหัสแผนงานได้";
                this.txtName.Focus();
                return false;
            }
            else if (this.pmChkCodeHirachy(ref ioErrorMsg) == false)
            {
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสผังงบประมาณซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Edit && this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmCanChangeCode())
            {
                ioErrorMsg = "ไม่สามารถแก้ไขรหัสแผนงานได้ เนื่องจากมีการอ้างอิงไปเป็นบัญชีคุม";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Edit && this.mstrOldType != this.cmbType.SelectedIndex)
                    && !this.pmCanChangeType())
            {
                ioErrorMsg = "ไม่สามารถแก้ไขชนิดแผนงานได้ เนื่องจากมีการอ้างอิงไปเป็นบัญชีคุม";
                this.cmbType.Focus();
                return false;
            }


            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID, this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "ชื่อผังงบประมาณซ้ำ";
            //    this.txtName.Focus();
            //    return false;
            //}
            else
                bllResult = true;

            return bllResult;
        }

        private bool pmChkCodeHirachy(ref string ioErrorMsg)
        {
            bool bllResult = false;
            if (this.txtQcBudChart.Text.Trim() != string.Empty)
            {
                if (this.txtCode.Text.TrimEnd().Length <= this.txtQcBudChart.Text.TrimEnd().Length)
                {
                    ioErrorMsg = "รหัสต้องยาวไม่น้อยกว่า " + this.txtQcBudChart.Text.TrimEnd().Length.ToString() + " ตัวอักษร";
                }
                else if (this.txtCode.Text.TrimEnd().Length > this.txtQcBudChart.Text.TrimEnd().Length)
                {
                    string strCodePrefix = StringHelper.SubStr(this.txtCode.Text.TrimEnd(), 1, this.txtQcBudChart.Text.TrimEnd().Length);
                    if (strCodePrefix != this.txtQcBudChart.Text.TrimEnd())
                    {
                        ioErrorMsg = "รหัส " + this.txtQcBudChart.Text.TrimEnd().Length.ToString() + " หลักแรกต้องเป็น " + this.txtQcBudChart.Text.TrimEnd() + " เท่านั้น !";
                    }
                    else
                    {
                        bllResult = true;
                    }
                }
                else
                {
                    bllResult = true;
                }
            }
            else
            {
                bllResult = true;
            }
            return bllResult;
        }

        private bool pmCanChangeCode()
        {
            bool bllCanChg = true;
            bool bllHasChild = this.pmHasChild();
            if (bllHasChild)
            {
                bllCanChg = false;
            }
            return bllCanChg;
        }

        private bool pmCanChangeType()
        {
            bool bllCanChg = true;
            bool bllHasChild = this.pmHasChild();
            if (bllHasChild)
            {
                bllCanChg = false;
            }
            return bllCanChg;
        }

        private bool pmHasChild()
        {
            bool bllHasChild = false;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrEditRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasChild", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cPRBGChart = ? ", ref strErrorMsg))
            {
                bllHasChild = true;
            }
            return bllHasChild;
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

        private bool pmRunCode2()
        {
            //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranchID }, this.txtCode.MaxLength);

            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            int intRunCode = 1;
            int inMaxLength = this.txtCode.Properties.MaxLength;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strType = "";
            switch (this.cmbType.SelectedIndex)
            {
                case 0:
                    strType = "D";
                    break;
                case 1:
                    strType = "G";
                    break;
            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, strType, this.txtQcBudChart.Text.TrimEnd() + ":" });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cCode < ? order by cCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QEMProjInfo.Field.Code].ToString().Trim();
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

            //DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
            DataRow dtrSaveInfo = null;
            if (this.mFormEditMode == UIHelper.AppFormState.Insert
                || (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cRowID from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg)))
            {
                bllIsNewRow = true;
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
                if (this.mstrEditRowID == string.Empty)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
                }
                dtrSaveInfo["cRowID"] = this.mstrEditRowID;
                dtrSaveInfo["cCreateBy"] = App.FMAppUserID;

                this.dtsDataEnv.Tables[this.mstrRefTable].Rows.Add(dtrSaveInfo);
            }
            else
            {
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
            }

            dtrSaveInfo["cCorp"] = App.ActiveCorp.RowID;
            dtrSaveInfo["cType"] = (this.cmbType.SelectedIndex == 0 ? "D" : "G");
            dtrSaveInfo["cPrBgChart"] = this.txtQcBudChart.Tag.ToString();
            dtrSaveInfo["cCode"] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo["cName"] = this.txtName.Text.TrimEnd();
            dtrSaveInfo["cFChr"] = AppUtil.StringHelper.GetFChr(this.txtName.Text.TrimEnd());
            dtrSaveInfo["cName2"] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo["cBgType"] = this.txtQcBudType.Tag.ToString();
            dtrSaveInfo["cRemark"] = this.txtRemark.Text.TrimEnd();
            dtrSaveInfo["cLastUpdBy"] = App.FMAppUserID.TrimEnd();
            dtrSaveInfo["dLastUpdBy"] = objSQLHelper.GetDBServerDateTime(); ;

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

                this.pmUpdateBudChartI();

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

        private void pmUpdateBudChartI()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cAcChart"].ToString().TrimEnd() != string.Empty)
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

                    this.pmReplRecordBudCI(bllIsNewRow, dtrTemPd, ref dtrBudCI);

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

        private bool pmReplRecordBudCI(bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cBGChartHD"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cAcChart"] = inTemPd["cAcChart"].ToString();
            dtrBudCI["cRemark"] = inTemPd["cRemark"].ToString().TrimEnd();

            //dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }


        private void pmCreateTem()
        {

            DataTable dtbTemPdVer = new DataTable(this.mstrTemPd);

            dtbTemPdVer.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cAcChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("cQcAcChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnAcChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cRemark", System.Type.GetType("System.String"));

            dtbTemPdVer.Columns["cRowID"].DefaultValue = "";
            dtbTemPdVer.Columns["cAcChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcAcChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnAcChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cRemark"].DefaultValue = "";

            this.dtsDataEnv.Tables.Add(dtbTemPdVer);

        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            //this.txtCode.Focus();
            this.txtQcBudType.Focus();
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

            this.txtQcBudChart.Tag = "";this.txtQcBudChart.Text = "";this.txtQnBudChart.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtName2.Text = "";
            this.txtQcBudType.Tag = "";this.txtQcBudType.Text = "";this.txtQnBudType.Text = "";
            this.cmbType.SelectedIndex = 0;
            this.txtRemark.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcAcChart"];

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
                    
                    this.txtQcBudChart.Tag = dtrLoadForm["cPrBgChart"].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcBudChart.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudChart", "BUDCHART", "select * from BGCHARTHD where CROWID = ?", ref strErrorMsg))
                    {
                        DataRow dtrBudChart = this.dtsDataEnv.Tables["QBudChart"].Rows[0];
                        this.txtQcBudChart.Text = dtrBudChart["cCode"].ToString().TrimEnd();
                        this.txtQnBudChart.Text = dtrBudChart["cName"].ToString().TrimEnd();
                    }

                    this.txtCode.Text = dtrLoadForm["cCode"].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm["cName"].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm["cName2"].ToString().TrimEnd();
                    this.cmbType.SelectedIndex = (dtrLoadForm["cType"].ToString() == "D" ? 0 : 1);
                    this.txtRemark.Text = dtrLoadForm["cRemark"].ToString().TrimEnd();

                    this.txtQcBudType.Tag = dtrLoadForm["cBgType"].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcBudType.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudType", "BUDTYPE", "select * from BGTYPE where CROWID = ?", ref strErrorMsg))
                    {
                        DataRow dtrBudType = this.dtsDataEnv.Tables["QBudType"].Rows[0];
                        this.txtQcBudType.Text = dtrBudType["cCode"].ToString().TrimEnd();
                        this.txtQnBudType.Text = dtrBudType["cName"].ToString().TrimEnd();
                    }

                    this.pmLoadBudCI();
                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
            this.mstrOldName = this.txtName.Text;
            this.mstrOldType = this.cmbType.SelectedIndex;
        }

        private void pmLoadBudCI()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudCI", "BUDCI", "select * from "+this.mstrITable+" where cBGChartHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBudCI in this.dtsDataEnv.Tables["QBudCI"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrBudCI["cRowID"].ToString();
                    dtrNewRow["cRemark"] = dtrBudCI["cRemark"].ToString().TrimEnd();
                    dtrNewRow["cAcChart"] = dtrBudCI["cAcChart"].ToString();

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cAcChart"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select * from EMACCHART where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcAcChart"] = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnAcChart"] = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);
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

        private void frmPdCateg_KeyDown(object sender, KeyEventArgs e)
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
                this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");
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

            this.mstrBGType = "";

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

        public bool ValidateField(string inBGType, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "CCODE")
                inSearchStr = inSearchStr.PadRight(this.txtCode.Properties.MaxLength);
            else
                inSearchStr = inSearchStr.PadRight(this.txtName.Properties.MaxLength);

            if (this.mstrSortKey != inOrderBy
                || this.mstrBGType != inBGType)
            {
                this.mstrSortKey = inOrderBy;
                this.mstrBGType = inBGType;
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

        private void txtQcBudType_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBUDTYPE" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBudType.Tag = "";
                this.txtQcBudType.Text = "";
                this.txtQnBudType.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BUDTYPE");
                e.Cancel = !this.pofrmGetBudType.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBudType.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcBudChart_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                this.txtQcBudChart.Tag = "";
                this.txtQcBudChart.Text = "";
                this.txtQnBudChart.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BUDCHART");
                e.Cancel = !this.pofrmGetBudChart.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBudChart.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            switch (inKeyField.ToUpper())
            {
                case "CQCACCHART":
                case "CQNACCHART":

                    string strOrderBy = (inKeyField.ToUpper() == "CQCACCHART" ? QEMAcChartInfo.Field.Code : QEMAcChartInfo.Field.Name);
                    this.pmInitPopUpDialog("ACCHART");
                    this.pofrmGetAcChart.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetAcChart.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_"+inKeyField);
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
                case "CQCACCHART":
                case "CQNACCHART":

                    string strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cAcChart"] = "";
                        dtrTemPd["cQcAcChart"] = "";
                        dtrTemPd["cQnAcChart"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("ACCHART");
                        string strOrderBy = (strCol.ToUpper() == "CQCACCHART" ? QEMAcChartInfo.Field.Code : QEMAcChartInfo.Field.Name);
                        e.Valid = !this.pofrmGetAcChart.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetAcChart.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCACCHART" ? dtrTemPd["cQcAcChart"].ToString().TrimEnd() : dtrTemPd["cQnAcChart"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cAcChart"] = "";
                            dtrTemPd["cQcAcChart"] = "";
                            dtrTemPd["cQnAcChart"] = "";
                        }
                    }
                    break;
            }

        }

        private void grcAcChart_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
        }

        private void grcAcChart_KeyDown(object sender, KeyEventArgs e)
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

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowIndex = e.RowHandle;
            if (rowIndex >= 0)
            {
                rowIndex++;
                e.Info.DisplayText = rowIndex.ToString();
                DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                if (dtrTemPd != null)
                {
                    dtrTemPd["nRecNo"] = rowIndex.ToString();
                }

            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }
    
    }
}
