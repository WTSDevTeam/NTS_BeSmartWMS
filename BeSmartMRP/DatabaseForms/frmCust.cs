
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
    public partial class frmCust : UIHelper.frmBase, UIHelper.IfrmDBBase
    {

        
        /// <summary>
        /// Class for Search Dialog
        /// </summary>
        #region "Search Dialog Class"
        private class cfrmSearchData : DialogForms.cfrmSearchBase
        {

            //private QMasterCoor mobjTabRefer = null;
            private string mstrRowID = "";
            private string mstrCode = "";

            public cfrmSearchData() { }

            override protected void OnInitComponent()
            {
                //this.mobjTabRefer = new QMasterCoor("C",App.ConnectionString, App.DatabaseReside);

                //this.mobjTabRefer.FixCorpID = App.gcCorp;

                this.AddSearchKey("รหัส", "FCCODE");
                this.AddSearchKey("ชื่อ", "FCNAME");
                base.OnInitComponent();

                this.mstrSearchVal = "";
                this.mstrSearchKey = "FCCODE";
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
                //DataTable dtrBrowView = new DataTable();
                //dtrBrowView = this.mobjTabRefer.QueryData(new object[] { App.gcCorp, "Y", this.mstrSearchVal }, this.mstrSearchKey, this.mstrBrowViewAlias);
                //if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias] != null)
                //    this.dtsDataEnv.Tables.Remove(this.mstrBrowViewAlias);

                //this.dtsDataEnv.Tables.Add(dtrBrowView);
            }

            private void pmInitGridProp()
            {
                this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

                //this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSearchKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.Columns["FCSKID"].Visible = false;
                this.gridView1.Columns["CCODE"].Caption = "รหัส";
                this.gridView1.Columns["CNAME"].Caption = "ชื่อ";
                this.gridView1.Columns["CNAME2"].Caption = "ชื่อภาษา 2";

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

        public const string xdCMRem = "Rem";
        public const string xdCMRem2 = "Rm2";
        public const string xdCMRem3 = "Rm3";
        public const string xdCMRem4 = "Rm4";
        public const string xdCMRem5 = "Rm5";
        public const string xdCMRem6 = "Rm6";
        public const string xdCMRem7 = "Rm7";
        public const string xdCMRem8 = "Rm8";
        public const string xdCMRem9 = "Rm9";
        public const string xdCMRem10 = "RmA";
        
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
        public const string x_CMWebSite = "Web";
        public const string x_CMEmail = "Ema";

        public const string x_CMTel = "Tel";
        public const string x_CMFax = "Fax";
        public const string x_CMTxDesti = "Des";
        public const string x_CMTaxId = "Tax";
        public const string x_CMRemLayH = "Lay";

        public const string x_CMCtNam2 = "CN2";
        public const string x_CMCtPos2 = "CP2";
        public const string x_CMCtNam3 = "CN3";
        public const string x_CMCtPos3 = "CP3";

        public const string x_CMCauseBlkLst = "BLK";
        public const string x_CMMoBileTel = "MTl";
        public const string xdCMMId = "MId";		//25/03/47 By Pic

        public static string TASKNAME = "ECOOR";

        public static int MAXLENGTH_CODE = 20;
        public static int MAXLENGTH_NAME = 70;
        public static int MAXLENGTH_NAME2 = 70;
        public static int MAXLENGTH_SNAME = 70;
        public static int MAXLENGTH_SNAME2 = 70;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrMasterTable = MapTable.Table.Coor;
        private string mstrRefTable = MapTable.Table.Coor;
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";

        private bool mbllAddNew = false;

        private string mstrMasterRowID = "";
        private string mstrEditRowID = "";
        private string mstrSaveRowID = "";

        private string mstrMemoS1 = "";
        private string mstrMemoS2 = "";

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
        //private frmBizType pofrmGetBizType = null;
        //private frmCrGrp pofrmGetCrGrp = null;
        private DialogForms.dlgGetCrGrp pofrmGetCrGrp = null;
        //private DialogForms.dlgGetPPolicy pofrmGetPPolicy = null;
        private DialogForms.dlgGetCurrency pofrmGetCurrency = null;

        private DialogForms.dlgGetVatType pofrmGetVatType = null;
        //private DialogForms.dlgGetBank pofrmGetBank = null;
        private DialogForms.dlgGetCrZone pofrmGetCrZone = null;

        private DialogForms.dlgGetCoor pofrmGetCoor = null;

        private DialogForms.dlgGetEmplSM pofrmGetEmpl = null;
        private DialogForms.dlgGetEmplSM pofrmGetDeliEmpl = null;
        private DialogForms.dlgGetEmplSM pofrmGetColEmpl = null;

        public frmCust()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmCust(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmCust mInstanse = null;

        public static frmCust GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmCust();
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

            this.pmInitMaxLength();

            this.cmbActive.Properties.Items.Clear();
            this.cmbActive.Properties.Items.AddRange(new object[] { "ว่าง = ACTIVE", "I = INACTIVE" });
            this.cmbActive.SelectedIndex = 0;

            this.cmbPersonType.Properties.Items.Clear();
            this.cmbPersonType.Properties.Items.AddRange(new object[] { "Y = ใช่", "N = ไม่ใช่" });
            this.cmbPersonType.SelectedIndex = 0;
            this.pmSetPersonStatus();

            this.cmbBlackList.Properties.Items.Clear();
            this.cmbBlackList.Properties.Items.AddRange(new object[] { "", "Y = ใช่" });
            this.cmbBlackList.SelectedIndex = 0;

            this.cmbPriceNo.Properties.Items.Clear();
            this.cmbPriceNo.Properties.Items.AddRange(new object[] { "", "A", "B", "C", "D", "E" });
            this.cmbPriceNo.SelectedIndex = 0;

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }
        }

        private void pmInitMaxLength()
        {

            //Page Edit 1

            this.txtCode.Properties.MaxLength = frmCust.MAXLENGTH_CODE;
            this.txtName.Properties.MaxLength = frmCust.MAXLENGTH_NAME;
            this.txtSName.Properties.MaxLength = frmCust.MAXLENGTH_SNAME;

            this.txtAddr11.Properties.MaxLength = 70;
            this.txtAddr21.Properties.MaxLength = 70;
            this.txtAddr31.Properties.MaxLength = 70;

            this.txtName2.Properties.MaxLength = frmCust.MAXLENGTH_NAME;
            this.txtSName2.Properties.MaxLength = frmCust.MAXLENGTH_SNAME;
            this.txtAddr12.Properties.MaxLength = 70;
            this.txtAddr22.Properties.MaxLength = 70;
            this.txtAddr32.Properties.MaxLength = 70;

            //this.txtQnTumbon.Properties.MaxLength = frmTumbon.MAXLENGTH_NAME;
            //this.txtQnAmphur.Properties.MaxLength = frmAmphur.MAXLENGTH_NAME;
            //this.txtQnProvince.Properties.MaxLength = frmProvince.MAXLENGTH_NAME;

            this.txtZip.Properties.MaxLength = 5;
            this.txtTel.Properties.MaxLength = 40;
            this.txtMTel.Properties.MaxLength = 13;
            this.txtFax.Properties.MaxLength = 40;

            //Page Edit 2
            this.txtTaxID.Properties.MaxLength = 15;
            this.txtMTaxID.Properties.MaxLength = 17;

            //this.txtQcAcChart.Properties.MaxLength = frmAcChart.MAXLENGTH_CODE;
            //this.txtQnAcChart.Properties.MaxLength = frmAcChart.MAXLENGTH_NAME;

            this.txtCTName.Properties.MaxLength = 70;
            this.txtCTAddr11.Properties.MaxLength = 40;
            this.txtCTAddr21.Properties.MaxLength = 40;
            this.txtCTAddr31.Properties.MaxLength = 35;
            this.txtCTAddr12.Properties.MaxLength = 40;
            this.txtCTAddr22.Properties.MaxLength = 40;
            this.txtCTAddr32.Properties.MaxLength = 35;
            this.txtCTZip.Properties.MaxLength = 5;
            this.txtCTTel.Properties.MaxLength = 20;
            this.txtCTFax.Properties.MaxLength = 20;

            this.txtRemarkLayH.Properties.MaxLength = 100;
            this.txtRemark.Properties.MaxLength = 100;
            this.txtRemark2.Properties.MaxLength = 100;
            this.txtWebSite.Properties.MaxLength = 50;
            this.txtEMail.Properties.MaxLength = 100;

            //Page Edit 3

            //this.txtQcCrGrp.Properties.MaxLength = frmCrGrp.MAXLENGTH_CODE; this.txtQnCrGrp.Properties.MaxLength = frmCrGrp.MAXLENGTH_NAME;

            //this.txtQcBizType.Properties.MaxLength = frmBizType.MAXLENGTH_CODE; this.txtQnBizType.Properties.MaxLength = frmBizType.MAXLENGTH_NAME;

            this.txtQcCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_CODE; this.txtQcCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_NAME;
            this.txtQcEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_CODE; this.txtQcEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_NAME;
            this.txtQcDeliEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_CODE; this.txtQnDeliEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_NAME;
            this.txtQcLayEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_CODE; this.txtQcLayEmpl.Properties.MaxLength = DialogForms.dlgGetEmplSM.MAXLENGTH_NAME;

            this.txtQcVatType.Properties.MaxLength = DialogForms.dlgGetVatType.MAXLENGTH_CODE;

            this.txtCause.Properties.MaxLength = 100;
            this.txtGrade.Properties.MaxLength = 1;

            //Page Edit 4
            this.txtQcVatCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_CODE; this.txtQnVatCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_NAME;
            this.txtQcDeliCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_CODE; this.txtQnDeliCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_NAME;
            this.txtQcColCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_CODE; this.txtQnColCoor.Properties.MaxLength = DialogForms.dlgGetCoor.MAXLENGTH_NAME;
            
            this.txtDiscStr.Properties.MaxLength = 20;
            this.txtDiscPcn.Properties.MaxLength = 20;

            //this.txtQcPricePolicy.Properties.MaxLength = DialogForms.dlgGetPPolicy.MAXLENGTH_CODE; this.txtQnPricePolicy.Properties.MaxLength = DialogForms.dlgGetPPolicy.MAXLENGTH_NAME;
            //this.txtQcDiscPolicy.Properties.MaxLength = DialogForms.dlgGetPPolicy.MAXLENGTH_CODE; this.txtQnDiscPolicy.Properties.MaxLength = DialogForms.dlgGetPPolicy.MAXLENGTH_NAME;
            this.txtQcCurrency.Properties.MaxLength = DialogForms.dlgGetCurrency.MAXLENGTH_CODE; this.txtQnCurrency.Properties.MaxLength = DialogForms.dlgGetCurrency.MAXLENGTH_NAME;

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strStat = "CSTATUS = case {0}.FCSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";

            //string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME, {0}.FCNAME as CNAME, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FTDATETIME as DCREATE, {0}.FDLASTUPD as DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
            //strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
            //strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";
            //strSQLExec += " where {0}.CCORP = ? and {0}.CISCUST = 'Y' ";

            string strSQLExec = "select {0}.FCSKID as FCSKID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME, {0}.FCNAME as CNAME, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD from {0} ";
            strSQLExec += " where {0}.FCCORP = ? and {0}.FCISCUST = 'Y' ";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrMasterTable });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.gcCorp });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "Master_Coor", strSQLExec, ref strErrorMsg);

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

            int i = 0;
            this.gridView1.Columns["CSTATUS"].VisibleIndex = i++;
            this.gridView1.Columns["DINACTIVE"].VisibleIndex = i++;
            this.gridView1.Columns["CCODE"].VisibleIndex = i++;
            this.gridView1.Columns["CNAME"].VisibleIndex = i++;
            this.gridView1.Columns["CSNAME"].VisibleIndex = i++;

            this.gridView1.Columns["CCODE"].Visible = true;
            this.gridView1.Columns["CNAME"].Visible = true;
            this.gridView1.Columns["CSNAME"].Visible = true;

            this.gridView1.Columns["CSTATUS"].Caption = "สถานะ";
            this.gridView1.Columns["DINACTIVE"].Caption = "วันที่ InActive";
            this.gridView1.Columns["CCODE"].Caption = "รหัส";
            this.gridView1.Columns["CNAME"].Caption = "ชื่อ";
            this.gridView1.Columns["CSNAME"].Caption = "ชื่อย่อ";

            this.gridView1.Columns["CSTATUS"].Width = 15;
            this.gridView1.Columns["DINACTIVE"].Width = 15;
            this.gridView1.Columns["CCODE"].Width = 40;
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
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanInsert, App.AppUserName, App.AppUserID))
                        {
                            this.mFormEditMode = UIHelper.AppFormState.Insert;
                            this.pmLoadEditPage();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการเพิ่มข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Update:
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
                        {
                            this.mFormEditMode = UIHelper.AppFormState.Edit;
                            this.pmLoadEditPage();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Delete:
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                        {
                            this.pmDeleteData();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการลบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Search:
                        if (this.gridView1.OptionsView.ShowAutoFilterRow)
                        {
                            this.gridView1.OptionsView.ShowAutoFilterRow = false;
                            this.gridView1.ClearColumnsFilter();
                        }
                        else
                        {
                            this.gridView1.OptionsView.ShowAutoFilterRow = true;
                        }
                        //this.pmSearchData();
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

            this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cName"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลลูกค้า : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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
            //Business.Entity.QMasterCoor QRefChild = new QMasterCoor("C",App.ConnectionString, App.DatabaseReside);
            //bool bllHasUsed = QRefChild.HasUsedChildTable(objSQLHelper, inCode, ref ioErrorMsg);
            bool bllHasUsed = false;
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
                //Business.Entity.QMasterCoor QRefChild = new QMasterCoor("C", App.ConnectionString, App.DatabaseReside);
                //QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                //QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                //QRefChild.DeleteChildTable(inCode);

                string strRefRowID = "";
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where FCSKID = ?", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QMaster_Coor", MapTable.Table.MasterCoor, "select * from " + MapTable.Table.MasterCoor + " where cCorp = ? and cCode = ?", new object[2] { App.gcCorp, inCode.TrimEnd() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                //{
                //    strRefRowID = this.dtsDataEnv.Tables["QMaster_Coor"].Rows[0]["cRowID"].ToString();

                //    object[] pAPara = new object[1] { strRefRowID };
                //    this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.MasterCoorX1 + " where cCoor = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //    this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //}

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
                if (MessageBox.Show("ยังไม่ได้ระบุรหัสลูกค้า ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสลูกค้า";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == string.Empty)
            {
                ioErrorMsg = "กรุณาระบุ ชื่อลูกค้า";
                this.txtName.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.gcCorp, this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสลูกค้าซ้ำ";
                this.txtCode.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
               && !this.pmIsValidateName(new object[] { App.gcCorp, this.txtName.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อลูกค้าซ้ำ";
                this.txtName.Focus();
                return false;
            }
            else if (this.txtSName.Text.TrimEnd() != string.Empty
               && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldSName.TrimEnd() != this.txtSName.Text.TrimEnd())
               && !this.pmIsValidateSName(new object[] { App.gcCorp, this.txtSName.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อย่อลูกค้าซ้ำ";
                this.txtSName.Focus();
                return false;
            }
            else if (this.txtName2.Text.TrimEnd() != string.Empty
                && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName2.TrimEnd() != this.txtName2.Text.TrimEnd())
                && !this.pmIsValidateName2(new object[] { App.gcCorp, this.txtName2.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อลูกค้าภาษา 2 ซ้ำ";
                this.txtName2.Focus();
                return false;
            }
            else if (this.txtSName2.Text.TrimEnd() != string.Empty
                && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldSName2.TrimEnd() != this.txtSName2.Text.TrimEnd())
                && !this.pmIsValidateSName2(new object[] { App.gcCorp, this.txtSName2.Text.TrimEnd() }))
            {
                ioErrorMsg = "ชื่อย่อลูกค้าภาษา 2 ซ้ำ";
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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcCode < ':' order by fcCode desc", ref strErrorMsg))
            {
                strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["FCCODE"].ToString().Trim();
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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcCode = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateName(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcName = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateName2(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcName2 = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateSName(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcSName = ?", ref strErrorMsg))
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmIsValidateSName2(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcIsCust = 'Y' and fcSName2 = ?", ref strErrorMsg))
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

            DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrMasterTable].NewRow();
            if (this.mFormEditMode == UIHelper.AppFormState.Insert
                || (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcSkid from " + this.mstrMasterTable + " where fcSkid = ?", ref strErrorMsg)))
            {
                bllIsNewRow = true;
                if (this.mstrEditRowID == string.Empty)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
                }
                //dtrSaveInfo["fcCreateBy"] = App.FMAppUserID;
            }

            this.mstrSaveRowID = this.mstrEditRowID;

            string gcTemStr01 = BizRule.SetMemData(this.txtAddr11.Text.Trim(), x_CMAd11);
            //gcTemStr01 += BizRule.SetMemData(this.txtQnTumbon.Text.TrimEnd() + " " + this.txtQnAmphur.Text.TrimEnd(), x_CMAd21);
            //gcTemStr01 += BizRule.SetMemData(this.txtQnProvince.Text.TrimEnd(), x_CMAd31);
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
            gcTemStr01 += BizRule.SetMemData(this.txtCause.Text.Trim(), x_CMCauseBlkLst);
            gcTemStr01 += BizRule.SetMemData(this.txtMTel.Text.Trim(), x_CMMoBileTel);
            gcTemStr01 += BizRule.SetMemData(this.txtMTaxID.Text.Trim(), xdCMMId);
            gcTemStr01 += BizRule.SetMemData(this.txtWebSite.Text.Trim(), x_CMWebSite);
            gcTemStr01 += BizRule.SetMemData(this.txtEMail.Text.Trim(), x_CMEmail);
            gcTemStr01 += BizRule.SetMemData(this.txtCTTel.Text.Trim(), x_CMCtTel);
            gcTemStr01 += BizRule.SetMemData(this.txtCTFax.Text.Trim(), x_CMCtFax);

            dtrSaveInfo["fcSkid"] = this.mstrEditRowID;
            dtrSaveInfo["fcCorp"] = App.gcCorp;
            
            //Page Edit 1
            dtrSaveInfo["fcCode"] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo["fcName"] = this.txtName.Text.TrimEnd();
            dtrSaveInfo["fcName2"] = this.txtName2.Text.TrimEnd();
            dtrSaveInfo["fcFChr"] = AppUtil.StringHelper.GetFChr(this.txtName.Text.TrimEnd());
            dtrSaveInfo["fcSName"] = this.txtSName.Text.TrimEnd();
            dtrSaveInfo["fcSName2"] = this.txtSName2.Text.TrimEnd();

            //dtrSaveInfo["cTumbon"] = this.txtQnTumbon.Tag.ToString();
            //dtrSaveInfo["cAmphur"] = this.txtQnAmphur.Tag.ToString();
            //dtrSaveInfo["cprovince"] = this.txtQnProvince.Tag.ToString();
            //dtrSaveInfo["cBizType"] = this.txtQcBizType.Tag.ToString();

            dtrSaveInfo["fcZip"] = this.txtZip.Text.TrimEnd();
            dtrSaveInfo["fcIsCust"] = "Y";
            dtrSaveInfo["fcIsSupp"] = "";
            if (this.txtSName.Text.TrimEnd().Length == 0)
            {
                dtrSaveInfo["fcFChrS"] = "";
            }
            else
            {
                dtrSaveInfo["fcFChrS"] = AppUtil.StringHelper.GetFChr(this.txtSName.Text.TrimEnd());
            }

            dtrSaveInfo["fcStatus"] = (this.cmbActive.SelectedIndex == 0 ? " " : "I");
            if (this.cmbActive.SelectedIndex == 0)
            {
                dtrSaveInfo["fdInactive"] = Convert.DBNull;
            }
            else
            {
                dtrSaveInfo["fdInactive"] = this.txtDInActive.DateTime.Date;
            }

            if (this.txtFirstContact.EditValue != null)
                dtrSaveInfo["fdFstCont"] = this.txtFirstContact.DateTime;
            else
                dtrSaveInfo["fdFstCont"] = Convert.DBNull;

            //Page Edit 2
            dtrSaveInfo["fcPersonty"] = (this.cmbPersonType.SelectedIndex == 0 ? "Y" : "N");
            dtrSaveInfo["fcContactN"] = this.txtCTName.Text.TrimEnd();
            dtrSaveInfo["fcAcChart"] = this.txtQcAcChart.Tag.ToString();

            //Page Edit 3
            dtrSaveInfo["fcCrGrp"] = this.txtQcCrGrp.Tag.ToString();
            dtrSaveInfo["fcCrZone"] = this.txtQcCrZone.Tag.ToString();
            dtrSaveInfo["fcEmpl"] = this.txtQcEmpl.Tag.ToString();
            dtrSaveInfo["fcDeliEmpl"] = this.txtQcDeliEmpl.Tag.ToString();
            dtrSaveInfo["fcLayEmpl"] = this.txtQcLayEmpl.Tag.ToString();
            dtrSaveInfo["fnCredLim"] = this.txtCredLim.Value;
            dtrSaveInfo["fnCredTerm"] = this.txtCredTerm.Value;
            dtrSaveInfo["fnBlack"] = (this.cmbBlackList.SelectedIndex == 0 ? 0 : 1);
            dtrSaveInfo["fcDiscStr"] = this.txtDiscPcn.Text.TrimEnd();
            dtrSaveInfo["fcCurrency"] = this.txtQcCurrency.Tag.ToString();

            string strPriceNo = "";
            switch (this.cmbPriceNo.SelectedIndex)
            {
                case 0:
                    strPriceNo = " ";
                    break;
                case 1:
                    strPriceNo = "1";
                    break;
                case 2:
                    strPriceNo = "2";
                    break;
                case 3:
                    strPriceNo = "3";
                    break;
                case 4:
                    strPriceNo = "4";
                    break;
            }
            dtrSaveInfo["fcPriceNo"] = strPriceNo;
            dtrSaveInfo["fcGrade"] = this.txtGrade.Text.TrimEnd();

            //Page Edit 4
            dtrSaveInfo["fcVatCoor"] = this.txtQcVatCoor.Tag.ToString();
            dtrSaveInfo["fcDeliCoor"] = this.txtQcDeliCoor.Tag.ToString();
            dtrSaveInfo["fcColCoor"] = this.txtQcColCoor.Tag.ToString();
            dtrSaveInfo["fcPolicyPr"] = this.txtQcPricePolicy.Tag.ToString();
            dtrSaveInfo["fcPolicyDi"] = this.txtQcDiscPolicy.Tag.ToString();

            dtrSaveInfo["fcVatType"] = this.txtQcVatType.Text.TrimEnd();
            dtrSaveInfo["fcBank"] = this.txtQnBank.Tag.ToString();
            dtrSaveInfo["fcBankNo"] = this.txtBankNo.Text.TrimEnd();
            dtrSaveInfo["fcBBranch"] = this.txtBBranch.Text.TrimEnd();


            //dtrSaveInfo["fcMapName"] = this.lblImgFileName.Text;

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

            //dtrSaveInfo["fcLastUpdBy"] = App.FMAppUserID;
            //dtrSaveInfo["fdLastUpd"] = objSQLHelper.GetDBServerDateTime(); ;

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
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mstrMemoS1 = BizRule.SetMemData(this.txtNRemark1.Text.Trim(), xdCMRem)
                    + BizRule.SetMemData(this.txtNRemark2.Text.Trim(), xdCMRem2)
                    + BizRule.SetMemData(this.txtNRemark3.Text.Trim(), xdCMRem3)
                    + BizRule.SetMemData(this.txtNRemark4.Text.Trim(), xdCMRem4)
                    + BizRule.SetMemData(this.txtNRemark5.Text.Trim(), xdCMRem5);

                this.mstrMemoS2 = BizRule.SetMemData(this.txtNRemark6.Text.Trim(), xdCMRem6)
                    + BizRule.SetMemData(this.txtNRemark7.Text.Trim(), xdCMRem7)
                    + BizRule.SetMemData(this.txtNRemark8.Text.Trim(), xdCMRem8)
                    + BizRule.SetMemData(this.txtNRemark9.Text.Trim(), xdCMRem9)
                    + BizRule.SetMemData(this.txtNRemark10.Text.Trim(), xdCMRem10);

                //this.pmUpdateCoorX1(ref strErrorMsg);

                //Business.Entity.QMasterCoor QRefChild = new QMasterCoor("C",App.ConnectionString, App.DatabaseReside);

                //QRefChild.FixCorpID = App.gcCorp;

                //QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                //QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                //QRefChild.SaveChildTable(dtrSaveInfo, this.mstrOldCode, this.mstrMemoS1, this.mstrMemoS2, this.txtQcCrGrp.Text, this.txtQcAcChart.Text);

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

        private bool pmUpdateCoorX1(ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            bool bllIsNewRow = false;
            string strRowID = "";
            object[] pAPara = null;

            DataRow dtrRCoorX1 = null;
            if (this.mstrMemoS1.Trim() + this.mstrMemoS2.Trim() != string.Empty)
            {
                if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.MasterCoorX1, MapTable.Table.MasterCoorX1, "select * from " + MapTable.Table.MasterCoorX1 + " where cCoor = ?", new object[1] { this.mstrSaveRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {

                    bllIsNewRow = true;

                    strRowID = App.mRunRowID(MapTable.Table.MasterCoorX1);
                    dtrRCoorX1 = this.dtsDataEnv.Tables[MapTable.Table.MasterCoorX1].NewRow();
                    dtrRCoorX1["cRowID"] = strRowID;
                    dtrRCoorX1["cCreateBy"] = App.FMAppUserID;
                }
                else
                {
                    bllIsNewRow = false;
                    strRowID = this.dtsDataEnv.Tables[MapTable.Table.MasterCoorX1].Rows[0]["cRowID"].ToString();
                    dtrRCoorX1 = this.dtsDataEnv.Tables[MapTable.Table.MasterCoorX1].Rows[0];
                }

                dtrRCoorX1["cCorp"] = App.gcCorp;
                dtrRCoorX1["cCoor"] = this.mstrSaveRowID;
                dtrRCoorX1["cMemData"] = this.mstrMemoS1;
                dtrRCoorX1["cMemData2"] = this.mstrMemoS2;
                dtrRCoorX1["cLastUpdBy"] = App.FMAppUserID;
                dtrRCoorX1["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRCoorX1, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }
            else
            {

                pAPara = new object[] { this.mstrSaveRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.MasterCoorX1 + " where CCOOR = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

            }
            return true;
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

            this.mbllAddNew = true;

            this.mstrMasterRowID = "";
            this.mstrEditRowID = "";

            this.mstrMemoS1 = "";
            this.mstrMemoS2 = "";

            #region "Page Edit 1"

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

            this.txtQnTumbon.Tag = ""; this.txtQnTumbon.Text = "";
            this.txtQnAmphur.Tag = ""; this.txtQnAmphur.Text = "";
            this.txtQnProvince.Tag = ""; this.txtQnProvince.Text = "";

            this.txtZip.Text = "";
            this.txtTel.Text = "";
            this.txtMTel.Text = "";
            this.txtFax.Text = "";
            //this.txtFirstContact.DateTime = DateTime.Now;
            this.txtFirstContact.EditValue = null;
            this.cmbActive.SelectedIndex = 0;
            this.txtDInActive.EditValue = null;
            //this.txtDInActive.DateTime = DateTime.Now;

            #endregion

            #region "Page Edit 2"

            this.cmbPersonType.SelectedIndex = 0;
            this.txtTaxID.Text = "";
            this.txtMTaxID.Text = "";
            this.txtQcAcChart.Tag = "";
            this.txtQcAcChart.Text = "";
            this.txtQnAcChart.Text = "";

            this.txtCTName.Text = "";
            this.txtCTAddr11.Text = "";
            this.txtCTAddr21.Text = "";
            this.txtCTAddr31.Text = "";
            this.txtCTAddr12.Text = "";
            this.txtCTAddr22.Text = "";
            this.txtCTAddr32.Text = "";
            this.txtCTZip.Text = "";
            this.txtCTTel.Text = "";
            this.txtCTFax.Text = "";
            this.txtRemarkLayH.Text = "";
            this.txtRemark.Text = "";
            this.txtRemark2.Text = "";
            this.txtWebSite.Text = "";
            this.txtEMail.Text = "";
                
            #endregion

            #region "Page Edit 3"

            this.txtQcCrGrp.Tag = ""; this.txtQcCrGrp.Text = ""; this.txtQnCrGrp.Text = "";
            this.txtQcBizType.Tag = ""; this.txtQcBizType.Text = ""; this.txtQnBizType.Text = "";
            this.txtQcCrZone.Tag = ""; this.txtQcCrZone.Text = ""; this.txtQnCrZone.Text = "";
            this.txtQcEmpl.Tag = ""; this.txtQcEmpl.Text = ""; this.txtQnEmpl.Text = "";
            this.txtQcDeliEmpl.Tag = ""; this.txtQcDeliEmpl.Text = ""; this.txtQnDeliEmpl.Text = "";
            this.txtQcLayEmpl.Tag = ""; this.txtQcLayEmpl.Text = ""; this.txtQnLayEmpl.Text = "";

            this.txtCredTerm.Value = 0;
            this.txtCredLim.Value = 0;
            this.txtQcVatType.Tag = "";
            this.txtQcVatType.Text = "";
            this.txtQnVatType.Text = "";

            this.cmbBlackList.SelectedIndex = 0;
            this.txtCause.Text = "";
            this.cmbPriceNo.SelectedIndex = 0;
            this.txtGrade.Text = "";

            this.txtBankNo.Text = "";
            this.txtQnBank.Text = "";
            this.txtQnBank.Tag = "";
            this.txtBBranch.Text = "";
            
            #endregion

            #region "Page Edit 4"
            
            this.txtQcVatCoor.Tag = ""; this.txtQcVatCoor.Text = ""; this.txtQnVatCoor.Text = "";
            this.txtQcDeliCoor.Tag = ""; this.txtQcDeliCoor.Text = ""; this.txtQnDeliCoor.Text = "";
            this.txtQcColCoor.Tag = ""; this.txtQcColCoor.Text = ""; this.txtQnColCoor.Text = "";
            this.txtQcPricePolicy.Tag = "";this.txtQcPricePolicy.Text = "";this.txtQnPricePolicy.Text = "";
            this.txtQcDiscPolicy.Tag = "";this.txtQcDiscPolicy.Text = "";this.txtQnDiscPolicy.Text = "";
            this.txtDiscStr.Text = "";
            this.txtDiscPcn.Text = "";
            this.txtQcVatType.Tag = "";this.txtQcVatType.Text = "";this.txtQnVatType.Text = "";
            this.txtQcCurrency.Tag = "";this.txtQcCurrency.Text = "";this.txtQnCurrency.Text = "";
            
            #endregion

            #region "Page Edit 5"

            this.txtNRemark1.Text = "";
            this.txtNRemark2.Text = "";
            this.txtNRemark3.Text = "";
            this.txtNRemark4.Text = "";
            this.txtNRemark5.Text = "";
            this.txtNRemark6.Text = "";
            this.txtNRemark7.Text = "";
            this.txtNRemark8.Text = "";
            this.txtNRemark9.Text = "";
            this.txtNRemark10.Text = "";
            #endregion

            //Page Edit 6
            this.lblImgFileName.Text = "";
            this.pcbPdImage.Image = null;
            this.pmLoadOldVar();

        }

        private void pmLoadFormData()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {

                //this.mstrMasterRowID = dtrBrow["fcSkid"].ToString();
                this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                pobjSQLUtil2.SetPara(new object[] { this.mstrEditRowID });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, this.mstrMasterTable, this.mstrMasterTable, "select * from " + this.mstrMasterTable + " where fcSkid = ?", ref strErrorMsg))
                {

                    DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrMasterTable].Rows[0];

                    #region "Load Page Edit 1"

                    this.txtCode.Text = dtrLoadForm["fcCode"].ToString().TrimEnd();
                    this.txtName.Text = dtrLoadForm["fcName"].ToString().TrimEnd();
                    this.txtSName.Text = dtrLoadForm["fcSName"].ToString().TrimEnd();
                    this.txtName2.Text = dtrLoadForm["fcName2"].ToString().TrimEnd();
                    this.txtSName2.Text = dtrLoadForm["fcSName2"].ToString().TrimEnd();

                    string gcTemStr01 = (Convert.IsDBNull(dtrLoadForm["fmMemData"]) ? "" : dtrLoadForm["fmMemData"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData2"]) ? "" : dtrLoadForm["fmMemData2"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData3"]) ? "" : dtrLoadForm["fmMemData3"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData4"]) ? "" : dtrLoadForm["fmMemData4"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrLoadForm["fmMemData5"]) ? "" : dtrLoadForm["fmMemData5"].ToString().TrimEnd());

                    this.txtAddr11.Text = BizRule.GetMemData(gcTemStr01, x_CMAd11);
                    this.txtAddr21.Text = BizRule.GetMemData(gcTemStr01, x_CMAd21);
                    this.txtAddr31.Text = BizRule.GetMemData(gcTemStr01, x_CMAd31);
                    this.txtAddr12.Text = BizRule.GetMemData(gcTemStr01, x_CMAd12);
                    this.txtAddr22.Text = BizRule.GetMemData(gcTemStr01, x_CMAd22);
                    this.txtAddr32.Text = BizRule.GetMemData(gcTemStr01, x_CMAd32);

                    this.txtZip.Text = dtrLoadForm["fcZip"].ToString().TrimEnd();
                    this.txtTel.Text = BizRule.GetMemData(gcTemStr01, x_CMTel);
                    this.txtFax.Text = BizRule.GetMemData(gcTemStr01, x_CMFax);
                    this.txtMTel.Text = BizRule.GetMemData(gcTemStr01, x_CMMoBileTel);

                    if (!Convert.IsDBNull(dtrLoadForm["fdFstCont"]))
                    {
                        this.txtFirstContact.DateTime = Convert.ToDateTime(dtrLoadForm["fdFstCont"]);
                    }

                    this.cmbActive.SelectedIndex = (dtrLoadForm["fcStatus"].ToString() == "I" ? 1 : 0);
                    if (!Convert.IsDBNull(dtrLoadForm["fdInActive"]))
                    {
                        this.txtDInActive.DateTime = Convert.ToDateTime(dtrLoadForm["fdInActive"]);
                    }

                    #endregion

                    #region "Load Page Edit 2"

                    this.cmbPersonType.SelectedIndex = (dtrLoadForm["fcPersonTy"].ToString() == "Y" ? 0 : 1);
                    this.txtMTaxID.Text = BizRule.GetMemData(gcTemStr01, xdCMMId);
                    this.txtTaxID.Text = BizRule.GetMemData(gcTemStr01, x_CMTaxId);

                    this.txtCTName.Text = dtrLoadForm["fcContactN"].ToString().TrimEnd();
                    this.txtCTAddr11.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd11);
                    this.txtCTAddr21.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd21);
                    this.txtCTAddr31.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd31);

                    this.txtCTAddr12.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd12);
                    this.txtCTAddr22.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd22);
                    this.txtCTAddr32.Text = BizRule.GetMemData(gcTemStr01, xdCMCtAd32);
                    this.txtCTZip.Text = BizRule.GetMemData(gcTemStr01, x_CMCtZip);
                    this.txtCTTel.Text = BizRule.GetMemData(gcTemStr01, x_CMCtTel);
                    this.txtCTFax.Text = BizRule.GetMemData(gcTemStr01, x_CMCtFax);

                    this.txtRemark.Text = BizRule.GetMemData(gcTemStr01, x_CMRem);
                    this.txtRemark2.Text = BizRule.GetMemData(gcTemStr01, x_CMRem2);
                    this.txtRemarkLayH.Text = BizRule.GetMemData(gcTemStr01, x_CMRemLayH);
                    this.txtWebSite.Text = BizRule.GetMemData(gcTemStr01, x_CMWebSite);
                    this.txtEMail.Text = BizRule.GetMemData(gcTemStr01, x_CMEmail);

                    #endregion

                    #region "Load Page Edit 3"

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcCrGrp"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCrGrp", "CRGRP", "select * from " + MapTable.Table.CoorGroup + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCrGrp = this.dtsDataEnv.Tables["QCrGrp"].Rows[0];
                        this.txtQcCrGrp.Tag = dtrLoadForm["fcCrGrp"].ToString();
                        this.txtQcCrGrp.Text = dtrCrGrp["fcCode"].ToString().TrimEnd();
                        this.txtQnCrGrp.Text = dtrCrGrp["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcCrZone"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCrZone", "CRZONE", "select * from CRZone where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCrZone = this.dtsDataEnv.Tables["QCrZone"].Rows[0];
                        this.txtQcCrZone.Tag = dtrLoadForm["fcCrZone"].ToString();
                        this.txtQcCrZone.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
                        this.txtQnCrZone.Text = dtrCrZone["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcEmpl"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QEmpl", "EMPL", "select fcCode, fcName from EMPL where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmpl"].Rows[0];
                        this.txtQcEmpl.Tag = dtrLoadForm["fcEmpl"].ToString();
                        this.txtQcEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
                        this.txtQnEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcDeliEmpl"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QEmpl", "EMPL", "select fcCode, fcName from EMPL where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmpl"].Rows[0];
                        this.txtQcDeliEmpl.Tag = dtrLoadForm["fcDeliEmpl"].ToString();
                        this.txtQcDeliEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
                        this.txtQnDeliEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcLayEmpl"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QEmpl", "EMPL", "select fcCode, fcName from EMPL where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmpl"].Rows[0];
                        this.txtQcLayEmpl.Tag = dtrLoadForm["fcLayEmpl"].ToString();
                        this.txtQcLayEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
                        this.txtQnLayEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
                    }

                    this.txtCredTerm.Value = Convert.ToInt32(dtrLoadForm["fnCredTerm"]);
                    this.txtCredLim.Value = Convert.ToDecimal(dtrLoadForm["fnCredLim"]);

                    this.cmbBlackList.SelectedIndex = (Convert.ToInt32(dtrLoadForm["fnBlack"]) == 0 ? 0 : 1);
                    this.txtCause.Text = BizRule.GetMemData(gcTemStr01, x_CMCauseBlkLst);
                    int intPriceNo = 0;
                    string strPriceNo = dtrLoadForm["fcPriceNo"].ToString();
                    switch (strPriceNo)
                    {
                        case "1":
                            intPriceNo = 1;
                            break;
                        case "2":
                            intPriceNo = 2;
                            break;
                        case "3":
                            intPriceNo = 3;
                            break;
                        case "4":
                            intPriceNo = 4;
                            break;
                    }
                    this.cmbPriceNo.SelectedIndex = intPriceNo;
                    this.txtGrade.Text = dtrLoadForm["fcGrade"].ToString();

                    //pobjSQLUtil.SetPara(new object[] { dtrLoadForm["fcAcChart"].ToString() });
                    //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrRefTable, "select * from " + MapTable.Table.MasterAcChart + " where cRowID = ?", ref strErrorMsg))
                    //{
                    //    DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
                    //    this.txtQcAcChart.Tag = dtrLoadForm["fcAcChart"].ToString();
                    //    this.txtQcAcChart.Text = dtrAcChart["cCode"].ToString().TrimEnd();
                    //    this.txtQnAcChart.Text = dtrAcChart["cName"].ToString().TrimEnd();
                    //}

                    #endregion

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcVatCoor"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.txtQcVatCoor.Tag = dtrLoadForm["fcVatCoor"].ToString();
                        this.txtQcVatCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                        this.txtQnVatCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcDeliCoor"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.txtQcDeliCoor.Tag = dtrLoadForm["fcDeliCoor"].ToString();
                        this.txtQcDeliCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                        this.txtQnDeliCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcColCoor"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from " + MapTable.Table.Coor + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        this.txtQcColCoor.Tag = dtrLoadForm["fcColCoor"].ToString();
                        this.txtQcColCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                        this.txtQnColCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcPolicyPr"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPPolicy", "PPOLICY", "select fcCode, fcName from PPOLICY where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrPolicy = this.dtsDataEnv.Tables["QPPolicy"].Rows[0];
                        this.txtQcPricePolicy.Tag = dtrLoadForm["fcPolicyPr"].ToString();
                        this.txtQcPricePolicy.Text = dtrPolicy["fcCode"].ToString().TrimEnd();
                        this.txtQnPricePolicy.Text = dtrPolicy["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcPolicyDi"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPPolicy", "PPOLICY", "select fcCode, fcName from PPOLICY where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrPolicy = this.dtsDataEnv.Tables["QPPolicy"].Rows[0];
                        this.txtQcDiscPolicy.Tag = dtrLoadForm["fcPolicyDi"].ToString();
                        this.txtQcDiscPolicy.Text = dtrPolicy["fcCode"].ToString().TrimEnd();
                        this.txtQnDiscPolicy.Text = dtrPolicy["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcVatType"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QVatType", "VATTYPE", "select * from " + MapTable.Table.VatType + " where fcCode = ?", ref strErrorMsg))
                    {
                        DataRow dtrVatType = this.dtsDataEnv.Tables["QVatType"].Rows[0];
                        this.txtQcVatType.Tag = dtrLoadForm["fcVatType"].ToString();
                        this.txtQcVatType.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                        this.txtQnVatType.Text = dtrVatType["fcName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcCurrency"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QCurrency", "CURRENCY", "select * from Currency where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCurr = this.dtsDataEnv.Tables["QCurrency"].Rows[0];
                        this.txtQcCurrency.Tag = dtrLoadForm["fcCurrency"].ToString();
                        this.txtQcCurrency.Text = dtrCurr["fcCode"].ToString().TrimEnd();
                        this.txtQnCurrency.Text = dtrCurr["fcName"].ToString().TrimEnd();
                    }

                    this.txtDiscPcn.Text = dtrLoadForm["fcDiscStr"].ToString().TrimEnd();
                    this.txtBankNo.Text = dtrLoadForm["fcBankNo"].ToString().TrimEnd();
                    this.txtBBranch.Text = dtrLoadForm["fcBBranch"].ToString().TrimEnd();

                    pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcBank"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBank", "BANK", "select * from " + MapTable.Table.Bank + " where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrBank = this.dtsDataEnv.Tables["QBank"].Rows[0];
                        this.txtQnBank.Tag = dtrLoadForm["fcBank"].ToString();
                        this.txtQnBank.Text = dtrBank["fcName"].ToString().TrimEnd();
                    }

                    this.lblImgFileName.Text = dtrLoadForm["fmMapName"].ToString();
                    if (this.lblImgFileName.Text.TrimEnd() != string.Empty)
                    {
                        if (System.IO.File.Exists(this.lblImgFileName.Text))
                        {
                            this.pcbPdImage.Image = System.Drawing.Image.FromFile(this.lblImgFileName.Text);
                        }
                        else
                        {
                            MessageBox.Show(this, "ไม่พบไฟลฺ์รูปภาพ " + this.lblImgFileName.Text + " !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    this.pmLoadRemark();

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

        private void pmLoadRemark()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            pobjSQLUtil.SetPara(new object[] { this.mstrMasterRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoorX5", "CoorX5", "select * from CoorX5 where fcCoor = ?", ref strErrorMsg))
            {
                DataRow dtrCoorX5 = this.dtsDataEnv.Tables["QCoorX5"].Rows[0];

                string gcTemStr01 = (Convert.IsDBNull(dtrCoorX5["fmMemData"]) ? "" : dtrCoorX5["fmMemData"].ToString().TrimEnd());
                string gcTemStr02 = (Convert.IsDBNull(dtrCoorX5["fmMemData2"]) ? "" : dtrCoorX5["fmMemData2"].ToString().TrimEnd());

                this.txtNRemark1.Text = BizRule.GetMemData(gcTemStr01, xdCMRem);
                this.txtNRemark2.Text = BizRule.GetMemData(gcTemStr01, xdCMRem2);
                this.txtNRemark3.Text = BizRule.GetMemData(gcTemStr01, xdCMRem3);
                this.txtNRemark4.Text = BizRule.GetMemData(gcTemStr01, xdCMRem4);
                this.txtNRemark5.Text = BizRule.GetMemData(gcTemStr01, xdCMRem5);
                this.txtNRemark6.Text = BizRule.GetMemData(gcTemStr02, xdCMRem6);
                this.txtNRemark7.Text = BizRule.GetMemData(gcTemStr02, xdCMRem7);
                this.txtNRemark8.Text = BizRule.GetMemData(gcTemStr02, xdCMRem8);
                this.txtNRemark9.Text = BizRule.GetMemData(gcTemStr02, xdCMRem9);
                this.txtNRemark10.Text = BizRule.GetMemData(gcTemStr02, xdCMRem10);

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

        private void frmCust_KeyDown(object sender, KeyEventArgs e)
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
                //this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                //this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text = "วันที่สร้าง : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดวันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
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
            //    case "TUMBON":
            //        if (this.pofrmGetTumbon == null)
            //        {
            //            this.pofrmGetTumbon = new DatabaseForms.frmTumbon(FormActiveMode.PopUp);
            //            this.pofrmGetTumbon.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetTumbon.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

            //    case "AMPHUR":
            //        if (this.pofrmGetAmphur == null)
            //        {
            //            this.pofrmGetAmphur = new DatabaseForms.frmAmphur(FormActiveMode.PopUp);
            //            this.pofrmGetAmphur.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetAmphur.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

            //    case "PROVINCE":
            //        if (this.pofrmGetProvince == null)
            //        {
            //            this.pofrmGetProvince = new DatabaseForms.frmProvince(FormActiveMode.PopUp);
            //            this.pofrmGetProvince.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProvince.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
                
            //    case "ACCHART":
            //        if (this.pofrmGetAcChart == null)
            //        {
            //            this.pofrmGetAcChart = new DatabaseForms.frmAcChart(FormActiveMode.PopUp);
            //            this.pofrmGetAcChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetAcChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
                case "CRGRP":
                    if (this.pofrmGetCrGrp == null)
                    {
                        //this.pofrmGetCrGrp = new DatabaseForms.frmCrGrp(FormActiveMode.PopUp);
                        this.pofrmGetCrGrp = new DialogForms.dlgGetCrGrp();
                        this.pofrmGetCrGrp.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCrGrp.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            //    case "BIZTYPE":
            //        if (this.pofrmGetBizType == null)
            //        {
            //            this.pofrmGetBizType = new DatabaseForms.frmBizType(FormActiveMode.PopUp);
            //            this.pofrmGetBizType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBizType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
            //    case "PPOLICY":
            //        if (this.pofrmGetPPolicy == null)
            //        {
            //            this.pofrmGetPPolicy = new DialogForms.dlgGetPPolicy();
            //            this.pofrmGetPPolicy.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPPolicy.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
            //    case "CURRENCY":
            //        if (this.pofrmGetCurrency == null)
            //        {
            //            this.pofrmGetCurrency = new DialogForms.dlgGetCurrency();
            //            this.pofrmGetCurrency.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCurrency.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor(CoorType.Customer);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                
            //    case "CRZONE":
            //        if (this.pofrmGetCrZone == null)
            //        {
            //            this.pofrmGetCrZone = new mBudget.DialogForms.dlgGetCrZone();
            //            this.pofrmGetCrZone.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCrZone.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
            //    case "SEMPL":
            //        if (this.pofrmGetEmpl == null)
            //        {
            //            this.pofrmGetEmpl = new mBudget.DialogForms.dlgGetEmplSM("S");
            //            this.pofrmGetEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

            //    case "DELIEMPL":
            //        if (this.pofrmGetDeliEmpl == null)
            //        {
            //            this.pofrmGetDeliEmpl = new mBudget.DialogForms.dlgGetEmplSM("D");
            //            this.pofrmGetDeliEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDeliEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

            //    case "COLEMPL":
            //        if (this.pofrmGetColEmpl == null)
            //        {
            //            this.pofrmGetColEmpl = new mBudget.DialogForms.dlgGetEmplSM("R");
            //            this.pofrmGetColEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetColEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;

            //    case "VATTYPE":
            //        if (this.pofrmGetVatType == null)
            //        {
            //            this.pofrmGetVatType = new mBudget.DialogForms.dlgGetVatType();
            //            this.pofrmGetVatType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetVatType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
            //        break;
            //    case "BANK":
            //        if (this.pofrmGetBank == null)
            //        {
            //            this.pofrmGetBank = new mBudget.DialogForms.dlgGetBank();
            //            this.pofrmGetBank.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBank.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
            //        }
                    //break;
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
            string strTxtPrefix = "";
            strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
            switch (inTextbox)
            {
                case "TXTQCACCHART":
                case "TXTQNACCHART":
                    //strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
                    //this.pmInitPopUpDialog("ACCHART");
                    //this.pofrmGetAcChart.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME"), true);
                    //if (this.pofrmGetAcChart.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;
                case "TXTQCCRGRP":
                case "TXTQNCRGRP":
                    this.pmInitPopUpDialog("CRGRP");
                    this.pofrmGetCrGrp.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCCRGRP" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetCrGrp.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTQCBIZTYPE":
                case "TXTQNBIZTYPE":
                    //this.pmInitPopUpDialog("BIZTYPE");
                    //this.pofrmGetBizType.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCBIZTYPE" ? "CCODE" : "CNAME"), true);
                    //if (this.pofrmGetBizType.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;

                case "TXTQCCURRENCY":
                case "TXTQNCURRENCY":
                    this.pmInitPopUpDialog("CURRENCY");
                    this.pofrmGetCurrency.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCCURRENCY" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetCurrency.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTQCPRICEPOLICY":
                case "TXTQNPRICEPOLICY":
                case "TXTQCDISCPOLICY":
                case "TXTQNDISCPOLICY":
                    //strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
                    //this.pmInitPopUpDialog("PPOLICY");
                    //this.pofrmGetPPolicy.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
                    //if (this.pofrmGetPPolicy.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;

                case "TXTQNTUMBON":

                    //this.pmInitPopUpDialog("TUMBON");
                    //this.pofrmGetTumbon.ValidateField(inPara1, "CNAME" , true);
                    //if (this.pofrmGetTumbon.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;

                case "TXTQNAMPHUR":

                    //this.pmInitPopUpDialog("AMPHUR");
                    //this.pofrmGetAmphur.ValidateField(inPara1, "CNAME", true);
                    //if (this.pofrmGetAmphur.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;

                case "TXTQNPROVINCE":

                    //this.pmInitPopUpDialog("PROVINCE");
                    //this.pofrmGetProvince.ValidateField(inPara1, "CNAME", true);
                    //if (this.pofrmGetProvince.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;
                
                case "TXTQCVATCOOR":
                case "TXTQNVATCOOR":
                case "TXTQCDELICOOR":
                case "TXTQNDELICOOR":
                case "TXTQCCOLCOOR":
                case "TXTQNCOLCOOR":

                    strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
                    this.pmInitPopUpDialog("COOR");
                    this.pofrmGetCoor.ValidateField("", (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetCoor.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                
                case "TXTQCCRZONE":
                case "TXTQNCRZONE":
                    this.pmInitPopUpDialog("CRZONE");
                    this.pofrmGetCrZone.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCCRZONE" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetCrZone.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTQCEMPL":
                case "TXTQNEMPL":
                    this.pmInitPopUpDialog("SEMPL");
                    this.pofrmGetEmpl.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCEMPL" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetEmpl.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTQCDELIEMPL":
                case "TXTQNDELIEMPL":
                    this.pmInitPopUpDialog("DELIEMPL");
                    this.pofrmGetDeliEmpl.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCDELIEMPL" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetDeliEmpl.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTQCLAYEMPL":
                case "TXTQNLAYEMPL":
                    this.pmInitPopUpDialog("COLEMPL");
                    this.pofrmGetColEmpl.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCLAYEMPL" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetColEmpl.PopUpResult)
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
                    //this.pmInitPopUpDialog("BANK");
                    //this.pofrmGetBank.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCBANK" ? "FCCODE" : "FCNAME"), true);
                    //if (this.pofrmGetBank.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = null;

            switch (inPopupForm.TrimEnd().ToUpper())
            {
            //    case "TXTQNTUMBON":
            //        if (this.pofrmGetTumbon != null)
            //        {
            //            DataRow dtrVal = this.pofrmGetTumbon.RetrieveValue();

            //            if (dtrVal != null)
            //            {
            //                this.txtQnTumbon.Tag = dtrVal["cRowID"].ToString();
            //                this.txtQnTumbon.Text = dtrVal["cName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQnTumbon.Tag = "";
            //                this.txtQnTumbon.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQNAMPHUR":
            //        if (this.pofrmGetAmphur != null)
            //        {
            //            DataRow dtrVal = this.pofrmGetAmphur.RetrieveValue();

            //            if (dtrVal != null)
            //            {
            //                this.txtQnAmphur.Tag = dtrVal["cRowID"].ToString();
            //                this.txtQnAmphur.Text = dtrVal["cName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQnAmphur.Tag = "";
            //                this.txtQnAmphur.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQNPROVINCE":
            //        if (this.pofrmGetProvince != null)
            //        {
            //            DataRow dtrVal = this.pofrmGetProvince.RetrieveValue();

            //            if (dtrVal != null)
            //            {
            //                this.txtQnProvince.Tag = dtrVal["cRowID"].ToString();
            //                this.txtQnProvince.Text = dtrVal["cName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQnProvince.Tag = "";
            //                this.txtQnProvince.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQCACCHART":
            //    case "TXTQNACCHART":
            //        if (this.pofrmGetAcChart != null)
            //        {
            //            DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

            //            if (dtrAcChart != null)
            //            {
            //                this.txtQcAcChart.Tag = dtrAcChart["cRowID"].ToString();
            //                this.txtQcAcChart.Text = dtrAcChart["cCode"].ToString().TrimEnd();
            //                this.txtQnAcChart.Text = dtrAcChart["cName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcAcChart.Tag = "";
            //                this.txtQcAcChart.Text = "";
            //                this.txtQnAcChart.Text = "";
            //            }
            //        }
            //        break;
                case "TXTQCCRGRP":
                case "TXTQNCRGRP":
                    if (this.pofrmGetCrGrp != null)
                    {
                        DataRow dtrCrGrp = this.pofrmGetCrGrp.RetrieveValue();

                        if (dtrCrGrp != null)
                        {
                            this.txtQcCrGrp.Tag = dtrCrGrp["fcSkid"].ToString();
                            this.txtQcCrGrp.Text = dtrCrGrp["fcCode"].ToString().TrimEnd();
                            this.txtQnCrGrp.Text = dtrCrGrp["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcCrGrp.Tag = "";
                            this.txtQcCrGrp.Text = "";
                            this.txtQnCrGrp.Text = "";
                        }
                    }
                    break;
            //    case "TXTQCBIZTYPE":
            //    case "TXTQNBIZTYPE":
            //        if (this.pofrmGetBizType != null)
            //        {
            //            DataRow dtrBiz = this.pofrmGetBizType.RetrieveValue();

            //            if (dtrBiz != null)
            //            {
            //                this.txtQcBizType.Tag = dtrBiz["cRowID"].ToString();
            //                this.txtQcBizType.Text = dtrBiz["cCode"].ToString().TrimEnd();
            //                this.txtQnBizType.Text = dtrBiz["cName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcBizType.Tag = "";
            //                this.txtQcBizType.Text = "";
            //                this.txtQnBizType.Text = "";
            //            }
            //        }
            //        break;
            //    case "TXTQCCRZONE":
            //    case "TXTQNCRZONE":
            //        if (this.pofrmGetCrZone != null)
            //        {
            //            DataRow dtrCrZone = this.pofrmGetCrZone.RetrieveValue();

            //            if (dtrCrZone != null)
            //            {
            //                this.txtQcCrZone.Tag = dtrCrZone["fcSkid"].ToString();
            //                this.txtQcCrZone.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
            //                this.txtQnCrZone.Text = dtrCrZone["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcCrZone.Tag = "";
            //                this.txtQcCrZone.Text = "";
            //                this.txtQnCrZone.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQCCURRENCY":
            //    case "TXTQNCURRENCY":
            //        if (this.pofrmGetCurrency != null)
            //        {
            //            DataRow dtrCurr = this.pofrmGetCurrency.RetrieveValue();

            //            if (dtrCurr != null)
            //            {
            //                this.txtQcCurrency.Tag = dtrCurr["fcSkid"].ToString();
            //                this.txtQcCurrency.Text = dtrCurr["fcCode"].ToString().TrimEnd();
            //                this.txtQnCurrency.Text = dtrCurr["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcCurrency.Tag = "";
            //                this.txtQcCurrency.Text = "";
            //                this.txtQnCurrency.Text = "";
            //            }
            //        }
            //        break;
            //    case "TXTQCEMPL":
            //    case "TXTQNEMPL":
            //        if (this.pofrmGetEmpl != null)
            //        {
            //            DataRow dtrEmpl = this.pofrmGetEmpl.RetrieveValue();

            //            if (dtrEmpl != null)
            //            {
            //                this.txtQcEmpl.Tag = dtrEmpl["fcSkid"].ToString();
            //                this.txtQcEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
            //                this.txtQnEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcEmpl.Tag = "";
            //                this.txtQcEmpl.Text = "";
            //                this.txtQnEmpl.Text = "";
            //            }
            //        }
            //        break;
                
            //    case "TXTQCDELIEMPL":
            //    case "TXTQNDELIEMPL":
            //        if (this.pofrmGetDeliEmpl != null)
            //        {
            //            DataRow dtrEmpl = this.pofrmGetDeliEmpl.RetrieveValue();

            //            if (dtrEmpl != null)
            //            {
            //                this.txtQcDeliEmpl.Tag = dtrEmpl["fcSkid"].ToString();
            //                this.txtQcDeliEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
            //                this.txtQnDeliEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcDeliEmpl.Tag = "";
            //                this.txtQcDeliEmpl.Text = "";
            //                this.txtQnDeliEmpl.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQCLAYEMPL":
            //    case "TXTQNLAYEMPL":
            //        if (this.pofrmGetColEmpl != null)
            //        {
            //            DataRow dtrEmpl = this.pofrmGetColEmpl.RetrieveValue();

            //            if (dtrEmpl != null)
            //            {
            //                this.txtQcLayEmpl.Tag = dtrEmpl["fcSkid"].ToString();
            //                this.txtQcLayEmpl.Text = dtrEmpl["fcCode"].ToString().TrimEnd();
            //                this.txtQnLayEmpl.Text = dtrEmpl["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcLayEmpl.Tag = "";
            //                this.txtQcLayEmpl.Text = "";
            //                this.txtQnLayEmpl.Text = "";
            //            }
            //        }
            //        break;

            //    case "TXTQCVATTYPE":
            //    case "TXTQNVATTYPE":
            //        if (this.pofrmGetVatType != null)
            //        {
            //            DataRow dtrVatType = this.pofrmGetVatType.RetrieveValue();

            //            if (dtrVatType != null)
            //            {
            //                this.txtQcVatType.Tag = dtrVatType["fcSkid"].ToString();
            //                this.txtQcVatType.Text = dtrVatType["fcCode"].ToString().TrimEnd();
            //                this.txtQnVatType.Text = dtrVatType["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcVatType.Tag = "";
            //                this.txtQcVatType.Text = "";
            //                this.txtQnVatType.Text = "";
            //            }
            //        }
            //        break;

                case "TXTQCVATCOOR":
                case "TXTQNVATCOOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrCoor = this.pofrmGetCoor.RetrieveValue();

                        if (dtrCoor != null)
                        {
                            this.txtQcVatCoor.Tag = dtrCoor["fcSkid"].ToString();
                            this.txtQcVatCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                            this.txtQnVatCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcVatCoor.Tag = "";
                            this.txtQcVatCoor.Text = "";
                            this.txtQnVatCoor.Text = "";
                        }
                    }
                    break;

                case "TXTQCCOLCOOR":
                case "TXTQNCOLCOOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrCoor = this.pofrmGetCoor.RetrieveValue();

                        if (dtrCoor != null)
                        {
                            this.txtQcColCoor.Tag = dtrCoor["fcSkid"].ToString();
                            this.txtQcColCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                            this.txtQnColCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcColCoor.Tag = "";
                            this.txtQcColCoor.Text = "";
                            this.txtQnColCoor.Text = "";
                        }
                    }
                    break;

                case "TXTQCDELICOOR":
                case "TXTQNDELICOOR":
                    if (this.pofrmGetCoor != null)
                    {
                        DataRow dtrCoor = this.pofrmGetCoor.RetrieveValue();

                        if (dtrCoor != null)
                        {
                            this.txtQcDeliCoor.Tag = dtrCoor["fcSkid"].ToString();
                            this.txtQcDeliCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                            this.txtQnDeliCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcDeliCoor.Tag = "";
                            this.txtQcDeliCoor.Text = "";
                            this.txtQnDeliCoor.Text = "";
                        }
                    }
                    break;

            //    case "TXTQCPRICEPOLICY":
            //    case "TXTQNPRICEPOLICY":
            //        if (this.pofrmGetPPolicy != null)
            //        {
            //            DataRow dtrPrice = this.pofrmGetPPolicy.RetrieveValue();

            //            if (dtrPrice != null)
            //            {
            //                this.txtQcPricePolicy.Tag = dtrPrice["fcSkid"].ToString();
            //                this.txtQcPricePolicy.Text = dtrPrice["fcCode"].ToString().TrimEnd();
            //                this.txtQnPricePolicy.Text = dtrPrice["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcPricePolicy.Tag = "";
            //                this.txtQcPricePolicy.Text = "";
            //                this.txtQnPricePolicy.Text = "";
            //            }
            //        }
            //        break;
            //    case "TXTQCDISCPOLICY":
            //    case "TXTQNDISCPOLICY":

            //        if (this.pofrmGetPPolicy != null)
            //        {
            //            DataRow dtrPrice = this.pofrmGetPPolicy.RetrieveValue();

            //            if (dtrPrice != null)
            //            {
            //                this.txtQcDiscPolicy.Tag = dtrPrice["fcSkid"].ToString();
            //                this.txtQcDiscPolicy.Text = dtrPrice["fcCode"].ToString().TrimEnd();
            //                this.txtQnDiscPolicy.Text = dtrPrice["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQcDiscPolicy.Tag = "";
            //                this.txtQcDiscPolicy.Text = "";
            //                this.txtQnDiscPolicy.Text = "";
            //            }
            //        }
            //        break;
            //    case "TXTQCBANK":
            //    case "TXTQNBANK":
            //        if (this.pofrmGetBank != null)
            //        {
            //            DataRow dtrBank = this.pofrmGetBank.RetrieveValue();

            //            if (dtrBank != null)
            //            {
            //                this.txtQnBank.Tag = dtrBank["fcSkid"].ToString();
            //                this.txtQnBank.Text = dtrBank["fcName"].ToString().TrimEnd();
            //            }
            //            else
            //            {
            //                this.txtQnBank.Tag = "";
            //                this.txtQnBank.Text = "";
            //            }
            //        }
            //        break;
            }
        }

        private void txtQnTumbon_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "cName";
            if (txtPopUp.Text == "")
            {
                this.txtQnTumbon.Tag = "";
                this.txtQnTumbon.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("TUMBON");
                //e.Cancel = !this.pofrmGetTumbon.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetTumbon.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
            }

        }

        private void txtQnAmphur_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "cName";
            if (txtPopUp.Text == "")
            {
                this.txtQnAmphur.Tag = "";
                this.txtQnAmphur.Text = "";
            }
            else
            {
                //this.pmInitPopUpDialog("AMPHUR");
                //e.Cancel = !this.pofrmGetAmphur.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetAmphur.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
            }

        }

        private void txtQnProvince_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "cName";
            if (txtPopUp.Text == "")
            {
                this.txtQnProvince.Tag = "";
                this.txtQnProvince.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PROVINCE");
                //e.Cancel = !this.pofrmGetProvince.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetProvince.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
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
                //e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetAcChart.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
            }

        }

        private void txtQcCrGrp_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCCRGRP" ? "fcCode" : "fcName");
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

        private void txtQcBizType_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCBIZTYPE" ? "cCode" : "cName");
            if (txtPopUp.Text == "")
            {
                this.txtQcBizType.Tag = "";
                this.txtQcBizType.Text = "";
                this.txtQnBizType.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BIZTYPE");
                //e.Cancel = !this.pofrmGetBizType.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetBizType.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
            }

        }

        private void txtQcCrZone_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCCRZONE" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcCrZone.Tag = "";
                this.txtQcCrZone.Text = "";
                this.txtQnCrZone.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("CRZONE");
                e.Cancel = !this.pofrmGetCrZone.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCrZone.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcVatCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCVATCOOR" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcVatCoor.Tag = "";
                this.txtQcVatCoor.Text = "";
                this.txtQnVatCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcDeliCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCDELICOOR" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcDeliCoor.Tag = "";
                this.txtQcDeliCoor.Text = "";
                this.txtQnDeliCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcColCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCCOLCOOR" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcColCoor.Tag = "";
                this.txtQcColCoor.Text = "";
                this.txtQnColCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcEmpl_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCEMPL" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcEmpl.Tag = "";
                this.txtQcEmpl.Text = "";
                this.txtQnEmpl.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("SEMPL");
                e.Cancel = !this.pofrmGetEmpl.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetEmpl.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcDeliEmpl_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCDELIEMPL" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcDeliEmpl.Tag = "";
                this.txtQcDeliEmpl.Text = "";
                this.txtQnDeliEmpl.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("DELIEMPL");
                e.Cancel = !this.pofrmGetDeliEmpl.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetDeliEmpl.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcLayEmpl_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCLAYEMPL" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcLayEmpl.Tag = "";
                this.txtQcLayEmpl.Text = "";
                this.txtQnLayEmpl.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COLEMPL");
                e.Cancel = !this.pofrmGetColEmpl.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetColEmpl.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcCurrency_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCCURRENCY" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcCurrency.Tag = "";
                this.txtQcCurrency.Text = "";
                this.txtQnCurrency.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("CURRENCY");
                e.Cancel = !this.pofrmGetCurrency.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCurrency.PopUpResult)
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

        private void txtQcDiscPolicy_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;

            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCDISCPOLICY" ? "fcCode" : "fcName");
            if (txtPopUp.Text == "")
            {
                this.txtQcDiscPolicy.Tag = "";
                this.txtQcDiscPolicy.Text = "";
                this.txtQnDiscPolicy.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PPOLICY");
                //e.Cancel = !this.pofrmGetPPolicy.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetPPolicy.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
            }

        }

        private void txtQcPricePolicy_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;

            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCPRICEPOLICY" ? "fcCode" : "fcName");

            if (txtPopUp.Text == "")
            {
                this.txtQcPricePolicy.Tag = "";
                this.txtQcPricePolicy.Text = "";
                this.txtQnPricePolicy.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PPOLICY");
                //e.Cancel = !this.pofrmGetPPolicy.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetPPolicy.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
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
                //e.Cancel = !this.pofrmGetBank.ValidateField(txtPopUp.Text, strOrderBy, false);
                //if (this.pofrmGetBank.PopUpResult)
                //{
                //    this.pmRetrievePopUpVal(txtPopUp.Name);
                //}
                //else
                //{
                //    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                //}
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
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["FCSKID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where FCSKID = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }

        private void pmLoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "ระบุรูปภาพลูกค้า";
            dlg.Filter = "Image Files (JPEG, GIF, BMP, etc.)|"
                + "*.jpg;*.jpeg;*.gif;*.bmp;"
                + "*.tif;*.tiff;*.png|"
                + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
                + "GIF files (*.gif)|*.gif|"
                + "BMP files (*.bmp)|*.bmp|"
                + "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|"
                + "PNG files (*.png)|*.png";

            dlg.InitialDirectory = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.pmClearImage();
                this.lblImgFileName.Text = dlg.FileName;
                this.pcbPdImage.Image = System.Drawing.Image.FromFile(dlg.FileName);
                this.pcbPdImage.Invalidate();
            }
            dlg.Dispose();
        }

        private void pmClearImage()
        {
            this.lblImgFileName.Text = "";
            this.pcbPdImage.Image = null;
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            this.pmLoadImage();
        }

        private void btnClearImg_Click(object sender, EventArgs e)
        {
            this.pmClearImage();
        }

        private void cmbActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetDActive();
        }

        private void pmSetDActive()
        {
            this.txtDInActive.Enabled = (this.cmbActive.SelectedIndex == 1);
        }


    }
}
