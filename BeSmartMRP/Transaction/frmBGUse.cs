
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

namespace BeSmartMRP.Transaction
{

    public partial class frmBGUse : UIHelper.frmBase
    {

        public static string TASKNAME = "EBUDUSE";

        public static int MAXLENGTH_CODE = 15;
        public static int MAXLENGTH_DESC = 150;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";
        private bool mbllFilterResult = false;
        private string mstrBudgetStep = SysDef.gc_APPROVE_STEP_WAIT;
        BudgetStep BeSmartMRPStep = BudgetStep.Prepare;

        private string mstrRefTable = QBGUseInfo.TableName;
        private string mstrITable = QBGUseInfo.TableNameItem1;

        private string mstrTemPd = "TemPd";

        private string mstrBranch = "";
        private string mstrJob = "";
        private string mstrSect = "";
        private string mstrDept = "";
        private string mstrBGBook = "";
        private string mstrBGYear = "";
        private int mintBGYear = 0;
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private decimal pmTotAmt = 0;
        private string mstrCurFilterStat = "X";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;
        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrCanEditMsg = "";

        private string mstrRefType = SysDef.gc_REFTYPE_PAY1;
        private DocumentType mDocType = DocumentType.P1;
        private string mstrQnRefType = "";
        private string mstrQnRefType2 = "";
        private bool mbllIsRevise = false;

        private string mstrRef2RefType = SysDef.gc_REFTYPE_ADJ1;
        private string mstrRef2RefTypeDefa = SysDef.gc_REFTYPE_ADJ1;
        private string mstrRef2BookID = "";
        private string mstrRef2QcBook = "";
        private string mstrRef2QnBook = "";

        //จะมีค่าเสมอ ใช้ในกรณี อ้างอิง เอกสาร ใบจองงบ
        private string mstrRef2BGRecvHD = "";
        //ใช้ในกรณี Revise เอกสาร
        //จะมีค่าแต่ตอน แก้ไขเอกสาร
        private string mstrRef2RowID = "";
        private string mstrRef2RowID_Old = "";

        private BudgetAgent BeSmartMRPAgent = new BudgetAgent();

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        //private DatabaseForms.frmBudChart pofrmGetBudChart = null;
        private DialogForms.dlgGetBudChart pofrmGetBudChart = null;

        public frmBGUse(string inRefType, string inStep)
        {
            InitializeComponent();

            this.mstrRefType = inRefType;
            this.mstrBudgetStep = inStep;
            this.mDocType = BudgetHelper.GetDocumentType(this.mstrRefType);
            this.BeSmartMRPStep = BudgetHelper.GetBudgetStep(inStep);
            this.pmInitForm();
        }

        private static frmBGUse mInstanse_1 = null;
        private static frmBGUse mInstanse_2 = null;
        private static frmBGUse mInstanse_3 = null;
        private static frmBGUse mInstanse_4 = null;
        private static frmBGUse mInstanse_5 = null;
        private static frmBGUse mInstanse_6 = null;

        private static frmBGUse mInstanse_A7 = null;
        private static frmBGUse mInstanse_A8 = null;
        private static frmBGUse mInstanse_A9 = null;
        private static frmBGUse mInstanse_AA = null;
        private static frmBGUse mInstanse_AB = null;
        private static frmBGUse mInstanse_AC = null;

        public static frmBGUse GetInstanse(string inRefType, string inStep)
        {
            DocumentType oResult = BudgetHelper.GetDocumentType(inRefType);
            switch (oResult)
            {
                case DocumentType.P1:
                    if (mInstanse_1 == null)
                    {
                        mInstanse_1 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_1;
                case DocumentType.P2:
                    if (mInstanse_2 == null)
                    {
                        mInstanse_2 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_2;
                case DocumentType.P3:
                    if (mInstanse_3 == null)
                    {
                        mInstanse_3 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_3;
                case DocumentType.P4:
                    if (mInstanse_4 == null)
                    {
                        mInstanse_4 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_4;
                case DocumentType.P5:
                    if (mInstanse_5 == null)
                    {
                        mInstanse_5 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_5;
                case DocumentType.P6:
                    if (mInstanse_6 == null)
                    {
                        mInstanse_6 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_6;

                case DocumentType.A7:
                    if (mInstanse_A7 == null)
                    {
                        mInstanse_A7 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_A7;
                case DocumentType.A8:
                    if (mInstanse_A8 == null)
                    {
                        mInstanse_A8 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_A8;
                case DocumentType.A9:
                    if (mInstanse_A9 == null)
                    {
                        mInstanse_A9 = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_A9;
                case DocumentType.AA:
                    if (mInstanse_AA == null)
                    {
                        mInstanse_AA = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_AA;
                case DocumentType.AB:
                    if (mInstanse_AB == null)
                    {
                        mInstanse_AB = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_AB;
                case DocumentType.AC:
                    if (mInstanse_AC == null)
                    {
                        mInstanse_AC = new frmBGUse(inRefType, inStep);
                    }
                    return mInstanse_AC;
            }
            return null;
        }

        private static void pmClearInstanse(DocumentType inDocType)
        {

            if (mInstanse_1 != null
                && inDocType == DocumentType.P1)
            {
                mInstanse_1 = null;
            }

            if (mInstanse_2 != null
                && inDocType == DocumentType.P2)
            {
                mInstanse_2 = null;
            }

            if (mInstanse_3 != null
                && inDocType == DocumentType.P3)
            {
                mInstanse_3 = null;
            }

            if (mInstanse_4 != null
                && inDocType == DocumentType.P4)
            {
                mInstanse_4 = null;
            }

            if (mInstanse_5 != null
                && inDocType == DocumentType.P5)
            {
                mInstanse_5 = null;
            }

            if (mInstanse_6 != null
                && inDocType == DocumentType.P6)
            {
                mInstanse_6 = null;
            }

            if (mInstanse_A7 != null
                && inDocType == DocumentType.A7)
            {
                mInstanse_A7 = null;
            }

            if (mInstanse_A8 != null
                && inDocType == DocumentType.A8)
            {
                mInstanse_A8 = null;
            }

            if (mInstanse_A9 != null
                && inDocType == DocumentType.A9)
            {
                mInstanse_A9 = null;
            }

            if (mInstanse_AA != null
                && inDocType == DocumentType.AA)
            {
                mInstanse_AA = null;
            }

            if (mInstanse_AB != null
                && inDocType == DocumentType.AB)
            {
                mInstanse_AB = null;
            }

            if (mInstanse_AC != null
                && inDocType == DocumentType.AC)
            {
                mInstanse_AC = null;
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

            this.pmCreateTem();
            this.pmInitGridProp_TemPd();

            this.pmFilterForm();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();

            UIBase.WaitClear();

            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
            if (!this.mbllFilterResult)
            {
                this.Close();
                //frmBGUse.pmClearInstanse();
            }
        }

        private void pmInitializeComponent()
        {

            UIHelper.UIBase.CreateTransactionToolbar(this.barMainEdit, this.barMain);
            
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefType", "DOCTYPE", "select * from DOCTYPE where cCode = ?", ref strErrorMsg))
            {
                DataRow dtrRefType = this.dtsDataEnv.Tables["QRefType"].Rows[0];
                this.mstrQnRefType = dtrRefType["cName"].ToString().TrimEnd();
            }

            this.mstrQnRefType2 = "เมนูบันทึก" + this.mstrQnRefType;
            this.Text = this.mstrQnRefType2;
            TASKNAME = "EBGUSE_" + this.mstrRefType;

            switch (this.mDocType)
            {
                case DocumentType.P1:
                    this.mstrRef2RefType = "'" + SysDef.gc_REFTYPE_RECV1 + "'" + "," + "'" + SysDef.gc_REFTYPE_ADJ1 + "'";
                    this.mstrRef2RefTypeDefa = SysDef.gc_REFTYPE_RECV1;
                    break;
                case DocumentType.P2:
                    this.mstrRef2RefType = "'" + SysDef.gc_REFTYPE_RECV2 + "'" + "," + "'" + SysDef.gc_REFTYPE_ADJ2 + "'";
                    this.mstrRef2RefTypeDefa = SysDef.gc_REFTYPE_RECV2;
                    break;
                case DocumentType.P3:
                    this.mstrRef2RefType = "'" + SysDef.gc_REFTYPE_RECV3 + "'" + "," + "'" + SysDef.gc_REFTYPE_ADJ3 + "'";
                    this.mstrRef2RefTypeDefa = SysDef.gc_REFTYPE_RECV3;
                    break;
                case DocumentType.A7:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_PAY1;
                    this.mbllIsRevise = true;
                    break;
                case DocumentType.A8:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_PAY2;
                    this.mbllIsRevise = true;
                    break;
                case DocumentType.A9:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_PAY3;
                    this.mbllIsRevise = true;
                    break;
                case DocumentType.AA:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_ADV1;
                    this.mbllIsRevise = true;
                    break;
                case DocumentType.AB:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_ADV2;
                    this.mbllIsRevise = true;
                    break;
                case DocumentType.AC:
                    this.mstrRef2RefType = SysDef.gc_REFTYPE_ADV3;
                    this.mbllIsRevise = true;
                    break;
            }

            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;

            this.pmSetMaxLength();
            this.pmMapEvent();
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtCode.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QBGUseInfo.TableName, QBGUseInfo.Field.Code);
            this.txtRefNo.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QBGUseInfo.TableName, QBGUseInfo.Field.RefNo);
            this.txtDesc1.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QBGUseInfo.TableName, QBGUseInfo.Field.Desc1);
        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);

            this.grdBrowView.Resize += new System.EventHandler(this.grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView1_ColumnWidthChanged);
            this.gridView1.EndSorting += new System.EventHandler(this.gridView1_EndSorting);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            //this.gridView1.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gridView1_CustomRowFilter);

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.grdTemPd.Resize += new System.EventHandler(this.grdTemPd_Resize);
            this.gridView2.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView2_ColumnWidthChanged);
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            this.grcAmt.Leave += new EventHandler(this.grcAmt_Leave);

            this.txtRefTo.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPopUp_ButtonClick);
            this.txtRefTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPopUp_KeyDown);

            this.grcQcBGChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcColumn_KeyDown);
            this.grcQcBGChart.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcColumn_ButtonClick);

            this.grcQnBGChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcColumn_KeyDown);
            this.grcQnBGChart.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcColumn_ButtonClick);

            this.grcRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcColumn_KeyDown);
            this.grcRemark.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcColumn_ButtonClick);
        
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;

            string strFld = "BGUSEHD.CROWID, BGUSEHD.NAMT ";
            strFld += " , BGUSEHD.CSTAT, BGUSEHD.CSTEP, BGUSEHD.CISCLOSE, BGUSEHD.CISREV, BGUSEHD.CCODE, BGUSEHD.DDATE, BGUSEHD.CDESC1, BGUSEHD.CREFNO, BGUSEHD.NAMT, BGUSEHD.CCANCELDES ";
            strFld += " , BGUSEHD.DCREATE, BGUSEHD.DLASTUPDBY, BGUSEHD.DCANCEL, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD, EM3.CLOGIN as CLOGIN_CAN ";

            string strSQLExec = "select " + strFld + " from {0} BGUSEHD ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = BGUSEHD.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = BGUSEHD.CLASTUPDBY ";
            strSQLExec += " left join {1} EM3 ON EM3.CROWID = BGUSEHD.CCANCELBY ";
            strSQLExec += " where BGUSEHD.CCORP = ? and BGUSEHD.CBRANCH = ? and BGUSEHD.CREFTYPE = ? and BGUSEHD.CBGBOOK = ? and BGUSEHD.CJOB = ? and BGUSEHD.NBGYEAR = ? and BGUSEHD.DDATE between ? and ? ";
            strSQLExec += " order by BGUSEHD.CCODE ";

            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            //pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBGBook, this.mstrJob, this.mintBGYear, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "BGAllocate", strSQLExec, ref strErrorMsg);

            //DataColumn dtcIsClose2 = new DataColumn("CISCLOSE2", Type.GetType("System.String"));
            DataColumn dtcAmt1 = new DataColumn("NBGTOTAMT", Type.GetType("System.Decimal"));
            DataColumn dtcAmt2 = new DataColumn("NBGBALAMT", Type.GetType("System.Decimal"));

            //dtcIsClose2.DefaultValue = "";
            dtcAmt1.DefaultValue = 0;
            dtcAmt2.DefaultValue = 0;

            //this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcIsClose2);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcAmt1);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcAmt2);

            decimal decAmt = 0; decimal decRecvAmt = 0; decimal decUseAmt = 0; decimal decBalAmt = 0;

            BudgetHelper.SuBeSmartMRP(pobjSQLUtil
                , App.ActiveCorp.RowID, this.mstrBranch, "", this.mintBGYear, this.mstrSect, this.mstrJob
                , ref decAmt, ref decRecvAmt, ref decUseAmt, ref decBalAmt);

            decimal decCurrAmt = decAmt;
            foreach (DataRow dtrBrowView in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                if (dtrBrowView[QBGUseInfo.Field.IsClose].ToString() == SysDef.gc_REF_STEP_CLOSED)
                {
                    //dtrBrowView["CISCLOSE2"] = "Closed";
                    dtrBrowView["CSTEP"] = "Closed";
                }
                if (dtrBrowView[QBGUseInfo.Field.IsRevise].ToString() == "Y")
                {
                    dtrBrowView["CSTEP"] = "Revise";
                }

                if (dtrBrowView[QBGUseInfo.Field.Status].ToString() != SysDef.gc_STAT_CANCEL
                    && dtrBrowView[QBGUseInfo.Field.IsRevise].ToString() != "Y")
                {
                    decCurrAmt = decCurrAmt - Convert.ToDecimal(dtrBrowView["NAMT"]);
                    dtrBrowView["NBGTOTAMT"] = decAmt;
                    dtrBrowView["NBGBALAMT"] = decCurrAmt;
                }
                else
                {
                    dtrBrowView["NBGTOTAMT"] = 0;
                    dtrBrowView["NBGBALAMT"] = 0;
                }

            }
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            //dvBrow.Sort = "CCODE";

            //this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            this.grdBrowView.DataSource = dvBrow;
            this.grdBrowView.MainView.PopulateColumns();

            this.gridView1.OptionsBehavior.Editable = true;
            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].OptionsColumn.AllowEdit = false;
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["CSTAT"].Visible = true;
            //this.gridView1.Columns["CSTEP"].Visible = true;
            this.gridView1.Columns["CSTEP"].Visible = true;
            this.gridView1.Columns["CCODE"].Visible = true;
            this.gridView1.Columns["DDATE"].Visible = true;
            this.gridView1.Columns["CDESC1"].Visible = true;
            this.gridView1.Columns["NAMT"].Visible = true;
            //this.gridView1.Columns["NBGTOTAMT"].Visible = true;
            //this.gridView1.Columns["NBGBALAMT"].Visible = true;
            this.gridView1.Columns["CREFNO"].Visible = true;

            this.gridView1.Columns["CSTAT"].VisibleIndex = 0;
            this.gridView1.Columns["CSTEP"].VisibleIndex = 1;
            this.gridView1.Columns["CCODE"].VisibleIndex = 2;
            this.gridView1.Columns["DDATE"].VisibleIndex = 3;
            this.gridView1.Columns["CDESC1"].VisibleIndex = 4;
            this.gridView1.Columns["NAMT"].VisibleIndex = 5;
            //this.gridView1.Columns["NBGTOTAMT"].VisibleIndex = 6;
            //this.gridView1.Columns["NBGBALAMT"].VisibleIndex = 7;
            this.gridView1.Columns["CREFNO"].VisibleIndex = 6;

            this.gridView1.Columns["CSTAT"].Caption = "C";
            this.gridView1.Columns["CSTEP"].Caption = "Step";
            this.gridView1.Columns["CCODE"].Caption = "เลขที่";
            this.gridView1.Columns["DDATE"].Caption = "วันที่";
            this.gridView1.Columns["CDESC1"].Caption = "รายละเอียด";
            this.gridView1.Columns["NAMT"].Caption = "จำนวนเงิน";
            this.gridView1.Columns["NBGTOTAMT"].Caption = "วงเงินงบประมาณ";
            this.gridView1.Columns["NBGBALAMT"].Caption = "งบประมาณคงเหลือ";
            this.gridView1.Columns["CREFNO"].Caption = "เอกสารอ้างอิง";


            this.gridView1.Columns["DDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["DDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["NAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NAMT"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["NBGTOTAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NBGTOTAMT"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["NBGBALAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NBGBALAMT"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["CSTAT"].Width = 30;
            this.gridView1.Columns["CSTEP"].Width = 70;
            this.gridView1.Columns["CCODE"].Width = 80;
            this.gridView1.Columns["DDATE"].Width = 70;
            this.gridView1.Columns["NAMT"].Width = 100;
            this.gridView1.Columns["NBGTOTAMT"].Width = 100;
            this.gridView1.Columns["NBGBALAMT"].Width = 100;
            this.gridView1.Columns["CREFNO"].Width = 100;

            this.pmRecalColWidth();

        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmRecalColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            this.pmRecalColWidth();
        }

        private void grdTemPd_Resize(object sender, EventArgs e)
        {
            this.pmRecalColWidth2();
        }

        private void gridView2_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            this.pmRecalColWidth2();
        }

        private void pmRecalColWidth()
        {

            this.gridView1.Columns["NBGTOTAMT"].Width = 100;
            this.gridView1.Columns["NBGBALAMT"].Width = 100;
            this.gridView1.Columns["CREFNO"].Width = 100;

            int intSumColWidth = this.gridView1.Columns["CSTAT"].Width
                + this.gridView1.Columns["CSTEP"].Width
                + this.gridView1.Columns["CCODE"].Width
                + this.gridView1.Columns["DDATE"].Width
                + this.gridView1.Columns["NAMT"].Width
                + this.gridView1.Columns["CREFNO"].Width;

                //+ this.gridView1.Columns["NBGTOTAMT"].Width
                //+ this.gridView1.Columns["NBGTOTAMT"].Width
                //+this.gridView1.Columns["NBGTOTAMT"].Width;

            int intColWidth = this.grdBrowView.Width - intSumColWidth - 30;
            this.gridView1.Columns["CDESC1"].Width = (intColWidth < 120 ? 120 : intColWidth);

        }

        private void pmRecalColWidth2()
        {

            int intSumColWidth = this.gridView2.Columns["cQcBGChart"].Width
                + this.gridView2.Columns["cQnBGChart"].Width
                + this.gridView2.Columns["nAmt"].Width;

            if (this.mbllIsRevise)
                intSumColWidth += this.gridView2.Columns["nAmt"].Width;

            int intColWidth = this.grdBrowView.Width - intSumColWidth - 30;
            if (intColWidth < 120)
            {
                intColWidth = 120;
            }
            else if (intColWidth > 500)
            {
                intColWidth = 500;
            }
            this.gridView2.Columns["cDesc1"].Width = intColWidth;

        }

        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            this.gridView2.Columns["cDesc1"].Visible = true;
            this.gridView2.Columns["cQcBGChart"].Visible = true;
            this.gridView2.Columns["cQnBGChart"].Visible = true;
            this.gridView2.Columns["nAmt"].Visible = true;
            this.gridView2.Columns["nRevAmt"].Visible = this.mbllIsRevise;

            this.gridView2.Columns["cDesc1"].VisibleIndex = 0;
            this.gridView2.Columns["cQcBGChart"].VisibleIndex = 1;
            this.gridView2.Columns["cQnBGChart"].VisibleIndex = 2;
            this.gridView2.Columns["nAmt"].VisibleIndex = 3;
            if (this.mbllIsRevise) this.gridView2.Columns["nRevAmt"].VisibleIndex = 4;

            this.gridView2.Columns["nAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nAmt"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView2.Columns["nRevAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nRevAmt"].DisplayFormat.FormatString = "#,###,###,###.00";


            this.gridView2.Columns["cDesc1"].Caption = "รายการ";
            this.gridView2.Columns["cQcBGChart"].Caption = "รหัสค่าใช้จ่าย";
            this.gridView2.Columns["cQnBGChart"].Caption = "ชื่อค่าใช้จ่าย";
            this.gridView2.Columns["nAmt"].Caption = "จำนวนเงิน";
            this.gridView2.Columns["nRevAmt"].Caption = "จำนวนเงินปรับแก้ไขเป็น";

            this.gridView2.Columns["cDesc1"].Width = 200;
            this.gridView2.Columns["cQcBGChart"].Width = 90;
            this.gridView2.Columns["cQnBGChart"].Width = 150;
            this.gridView2.Columns["nAmt"].Width = 80;
            this.gridView2.Columns["nRevAmt"].Width = 150;

            //this.gridView2.Columns["cQnBGChart"].OptionsColumn.AllowEdit = false;
            this.gridView2.Columns["cQnBGChart"].AppearanceCell.BackColor = Color.Silver;

            this.grcRemark.ReadOnly = true;
            this.grcQnBGChart.ReadOnly = true;

            this.gridView2.Columns["cDesc1"].ColumnEdit = this.grcRemark;
            this.gridView2.Columns["cQcBGChart"].ColumnEdit = this.grcQcBGChart;
            this.gridView2.Columns["cQnBGChart"].ColumnEdit = this.grcQnBGChart;

            this.gridView2.Columns["cDesc1"].AppearanceCell.BackColor = Color.Silver;
            this.gridView2.Columns["cQcBGChart"].AppearanceCell.BackColor = Color.Silver;
            this.gridView2.Columns["cQnBGChart"].AppearanceCell.BackColor = Color.Silver;

            //this.gridView2.Columns["cDesc1"].OptionsColumn.AllowEdit = false;
            this.gridView2.Columns["cQcBGChart"].OptionsColumn.AllowEdit = false;

            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;

            if (this.mbllIsRevise)
            {
                this.gridView2.Columns["nAmt"].AppearanceCell.BackColor = Color.Silver;
                this.gridView2.Columns["nAmt"].OptionsColumn.AllowEdit = false;
                this.gridView2.Columns["nRevAmt"].ColumnEdit = this.grcAmt;
            }
            else
            {
                this.gridView2.Columns["nAmt"].ColumnEdit = this.grcAmt;
            }

            this.pmRecalColWidth2();
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

            this.barMainEdit.Items[WsToolBar.Close.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Cancel.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Close.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);

            //this.barMainEdit.Items["tbrTXCopy"].Enabled = (inActivePage == 0 ? true : false);
            //this.barMainEdit.Items["tbrType"].Enabled = (inActivePage == 0 ? true : false);
        
        }

        private void pmFilterForm()
        {
            this.pmInitPopUpDialog("FILTER");
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
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = null;
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Common.dlgFilter01 dlgFilter = new Common.dlgFilter01(this.mstrRefType))
                    {
                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrBranch = dlgFilter.BranchID;
                            this.txtQnBranch.Text = dlgFilter.BranchName;

                            this.txtQnBook.Text = dlgFilter.BGBookCode.Trim() + " : " + dlgFilter.BGBookName.Trim();
                            //this.mstrSect = dlgFilter.SectID;
                            this.txtYear.Text = dlgFilter.mYear;
                            this.mstrBGYear = dlgFilter.mYear;
                            this.mintBGYear = dlgFilter.nYear;
                            this.mstrBGBook = dlgFilter.BGBookID;
                            this.mstrJob = dlgFilter.JobID;
                            this.txtQcJob.Text = dlgFilter.JobCode;
                            this.txtQnJob.Text = dlgFilter.JobName;
                            this.mstrSect = "";

                            this.txtQnBook2.Text = this.txtQnBook.Text;
                            this.txtQcJob2.Text = this.txtQcJob.Text;
                            this.txtQnJob2.Text = this.txtQnJob.Text;

                            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrJob, this.mintBGYear, SysDef.gc_APPROVE_STEP_POST });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGTran", "BGTRANHD", "select CSECT from BGTRANHD where CCORP = ? and CJOB = ? and NBGYEAR = ? and CISPOST = ?", ref strErrorMsg))
                            {
                                this.mstrSect = this.dtsDataEnv.Tables["QBGTran"].Rows[0]["cSect"].ToString();
                            }

                            this.mdttBegDate = dlgFilter.BegDate;
                            this.mdttEndDate = dlgFilter.EndDate;

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();
                        }
                    }
                    break;
                //ค้นใบจองมาตัดจ่าย
                case "REFTO":
                    using (Common.dlgGetRefTo2 dlg = new Common.dlgGetRefTo2(this.mstrRef2RefType, this.mstrRef2RefTypeDefa))
                    {
                        dlg.BranchID = this.mstrBranch;
                        dlg.JobID = this.mstrJob;
                        dlg.nYear = this.mintBGYear;
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            if (this.mstrRef2BGRecvHD != dlg.DocID)
                            {
                                this.mstrRef2BookID = dlg.BGBookID;
                                this.mstrRef2QcBook = dlg.BGBookCode;
                                this.mstrRef2BGRecvHD = dlg.DocID;
                                this.pmLoadRefToItem();
                            }
                        }
                    }
                    break;
                 //ค้นใบจ่าย Revise เอกสาร
                case "REFTO_BGUSE":
                    using (Common.dlgGetRefTo3 dlg = new Common.dlgGetRefTo3(this.mstrRef2RefType))
                    {
                        dlg.BranchID = this.mstrBranch;
                        dlg.JobID = this.mstrJob;
                        dlg.nYear = this.mintBGYear;
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            if (this.mstrRef2RowID != dlg.DocID)
                            {
                                this.mstrRef2BookID = dlg.BGBookID;
                                this.mstrRef2QcBook = dlg.BGBookCode;
                                this.mstrRef2RowID = dlg.DocID;
                                this.pmLoadRefToItem2();
                            }
                        }
                    }
                    break;
                case "BGBAL":
                    using (Common.dlgShowBGBal dlg = new Common.dlgShowBGBal())
                    {
                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                        DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                        if (dtrTemPd != null)
                        {
                            //dlg.Desc1 = dtrTemPd["cDesc1"].ToString().TrimEnd();
                            string strBGChartHD = dtrTemPd["cBGChartHD"].ToString();
                            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                            objSQLHelper.SetPara(new object[] { strBGChartHD });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGChart", "BGCHARTHD", "select * from BGCHARTHD where CROWID = ?", ref strErrorMsg))
                            {
                                string strPRBGChartHD = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cPRBGChart"].ToString();
                                dlg.ValidateField(this.mstrBranch, "", this.mstrSect, this.mstrJob, this.mintBGYear, strPRBGChartHD, "QcBGChart", true);
                            }

                        }
                    }

                    break;

                case "REMARK":
                    using (Common.dlgGetRemark dlg = new Common.dlgGetRemark())
                    {
                        dlg.DescReadOnly = true;
                        DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                        if (dtrTemPd == null)
                        {
                            dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                        }

                        dlg.Desc1 = dtrTemPd["cDesc1"].ToString().TrimEnd();
                        dlg.Desc2 = dtrTemPd["cDesc2"].ToString().TrimEnd();
                        dlg.Desc3 = dtrTemPd["cDesc3"].ToString().TrimEnd();
                        dlg.Desc4 = dtrTemPd["cDesc4"].ToString().TrimEnd();
                        dlg.Desc5 = dtrTemPd["cDesc5"].ToString().TrimEnd();
                        dlg.Desc6 = dtrTemPd["cDesc6"].ToString().TrimEnd();
                        dlg.Desc7 = dtrTemPd["cDesc7"].ToString().TrimEnd();
                        dlg.Desc8 = dtrTemPd["cDesc8"].ToString().TrimEnd();
                        dlg.Desc9 = dtrTemPd["cDesc9"].ToString().TrimEnd();
                        dlg.Desc10 = dtrTemPd["cDesc10"].ToString().TrimEnd();

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            dtrTemPd["cDesc1"] = dlg.Desc1;
                            dtrTemPd["cDesc2"] = dlg.Desc2;
                            dtrTemPd["cDesc3"] = dlg.Desc3;
                            dtrTemPd["cDesc4"] = dlg.Desc4;
                            dtrTemPd["cDesc5"] = dlg.Desc5;
                            dtrTemPd["cDesc6"] = dlg.Desc6;
                            dtrTemPd["cDesc7"] = dlg.Desc7;
                            dtrTemPd["cDesc8"] = dlg.Desc8;
                            dtrTemPd["cDesc9"] = dlg.Desc9;
                            dtrTemPd["cDesc10"] = dlg.Desc10;

                            this.gridView2.UpdateCurrentRow();

                        }
                    }
                    break;

                case "BUDCHART":
                    if (this.pofrmGetBudChart == null)
                    {
                        //this.pofrmGetBudChart = new DialogForms.dlgGetBudChart();
                        //this.pofrmGetBudChart = new DatabaseForms.frmBudChart(FormActiveMode.Report);
                        this.pofrmGetBudChart = new DialogForms.dlgGetBudChart();
                        this.pofrmGetBudChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBudChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void pmLoadRefToItem()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrRef2BGRecvHD });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGRecvHD", "BGRECVHD", "select * from BGRECVHD where CROWID = ?", ref strErrorMsg))
            {
                DataRow dtrRecvH = this.dtsDataEnv.Tables["QBGRecvHD"].Rows[0];
                //this.txtDate.DateTime = Convert.ToDateTime(dtrRecvH["dDate"]);
                this.txtDesc1.Text = dtrRecvH["cDesc1"].ToString().TrimEnd();
                this.txtRefNo.Text = dtrRecvH["cRefNo"].ToString().TrimEnd();
                this.txtRefTo.Text = this.mstrRef2QcBook.Trim() + "/" + dtrRecvH["cCode"].ToString().TrimEnd();
                this.pmLoadItem2(this.mstrRef2BGRecvHD);

                this.pmCalTotAmt();
            }

        }

        /// <summary>
        /// Load รายการอ้างอิงจากเอกสารการจองงบ/เงินทดรอง
        /// </summary>
        /// <param name="intRowID"></param>
        private void pmLoadItem2(string intRowID)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { intRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudItem", "BUDCI", "select * from BGRECVIT where cBGRECVHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBudItem in this.dtsDataEnv.Tables["QBudItem"].Rows)
                {

                    if (dtrBudItem["cStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                        continue;

                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cBGRecvHD"] = intRowID;
                    dtrNewRow["cBGRecvIT"] = dtrBudItem["cRowID"].ToString();
                    dtrNewRow["cBGChartHD"] = dtrBudItem["cBGChartHD"].ToString();
                    dtrNewRow["cDesc1"] = dtrBudItem["cDesc1"].ToString().TrimEnd();
                    dtrNewRow["cDesc2"] = dtrBudItem["cDesc2"].ToString().TrimEnd();
                    dtrNewRow["cDesc3"] = dtrBudItem["cDesc3"].ToString().TrimEnd();
                    dtrNewRow["cDesc4"] = dtrBudItem["cDesc4"].ToString().TrimEnd();
                    dtrNewRow["cDesc5"] = dtrBudItem["cDesc5"].ToString().TrimEnd();
                    dtrNewRow["cDesc6"] = dtrBudItem["cDesc6"].ToString().TrimEnd();
                    dtrNewRow["cDesc7"] = dtrBudItem["cDesc7"].ToString().TrimEnd();
                    dtrNewRow["cDesc8"] = dtrBudItem["cDesc8"].ToString().TrimEnd();
                    dtrNewRow["cDesc9"] = dtrBudItem["cDesc9"].ToString().TrimEnd();
                    dtrNewRow["cDesc10"] = dtrBudItem["cDesc10"].ToString().TrimEnd();

                    decimal decBackAmt = Convert.ToDecimal(dtrBudItem["nAmt"]) - Convert.ToDecimal(dtrBudItem["nUseAmt"]);
                    dtrNewRow["nAmt"] = decBackAmt;
                    dtrNewRow["nRevAmt"] = 0;
                    dtrNewRow["nUseAmt"] = 0;

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cBGChartHD"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBGChart", "BGChart", "select * from BGChartHD where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);

                }
            }

        }

        private void pmLoadRefToItem2()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrRef2RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGRecvHD", "BGRECVHD", "select * from BGUSEHD where CROWID = ?", ref strErrorMsg))
            {
                DataRow dtrRecvH = this.dtsDataEnv.Tables["QBGRecvHD"].Rows[0];
                //this.txtDate.DateTime = Convert.ToDateTime(dtrRecvH["dDate"]);
                this.txtDesc1.Text = dtrRecvH["cDesc1"].ToString().TrimEnd();
                this.txtRefNo.Text = dtrRecvH["cRefNo"].ToString().TrimEnd();
                this.txtRefTo.Text = this.mstrRef2QcBook.Trim() + "/" + dtrRecvH["cCode"].ToString().TrimEnd();
                this.pmLoadItem3(this.mstrRef2RowID);

                this.pmCalTotAmt();
            }

        }

        /// <summary>
        /// Load รายการอ้างอิงใบ จ่ายงบ/เคลียย์เงินเงินทดรอง
        /// </summary>
        /// <param name="intRowID"></param>
        private void pmLoadItem3(string intRowID)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;

            pobjSQLUtil.SetPara(new object[] { intRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBGUseHD", "BGUSEHD", "select cBGRecvHD from BGUSEHD where cRowID = ?", ref strErrorMsg))
            {
                this.mstrRef2BGRecvHD = this.dtsDataEnv.Tables["QBGUseHD"].Rows[0]["cBGRecvHD"].ToString();
            }

            pobjSQLUtil.SetPara(new object[] { intRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudItem", "BUDCI", "select * from BGUSEIT where cBGUSEHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBudItem in this.dtsDataEnv.Tables["QBudItem"].Rows)
                {

                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRefToHD"] = intRowID;
                    dtrNewRow["cRefToIT"] = dtrBudItem["cRowID"].ToString();
                    dtrNewRow["cBGRecvHD"] = dtrBudItem["cBGRecvHD"].ToString();
                    dtrNewRow["cBGRecvIT"] = dtrBudItem["cBGRecvIT"].ToString();
                    dtrNewRow["cBGChartHD"] = dtrBudItem["cBGChartHD"].ToString();
                    dtrNewRow["cDesc1"] = dtrBudItem["cDesc1"].ToString().TrimEnd();
                    dtrNewRow["cDesc2"] = dtrBudItem["cDesc2"].ToString().TrimEnd();
                    dtrNewRow["cDesc3"] = dtrBudItem["cDesc3"].ToString().TrimEnd();
                    dtrNewRow["cDesc4"] = dtrBudItem["cDesc4"].ToString().TrimEnd();
                    dtrNewRow["cDesc5"] = dtrBudItem["cDesc5"].ToString().TrimEnd();
                    dtrNewRow["cDesc6"] = dtrBudItem["cDesc6"].ToString().TrimEnd();
                    dtrNewRow["cDesc7"] = dtrBudItem["cDesc7"].ToString().TrimEnd();
                    dtrNewRow["cDesc8"] = dtrBudItem["cDesc8"].ToString().TrimEnd();
                    dtrNewRow["cDesc9"] = dtrBudItem["cDesc9"].ToString().TrimEnd();
                    dtrNewRow["cDesc10"] = dtrBudItem["cDesc10"].ToString().TrimEnd();

                    dtrNewRow["nAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);
                    dtrNewRow["nRevAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);
                    dtrNewRow["nUseAmt"] = Convert.ToDecimal(dtrBudItem["nUseAmt"]);
                    dtrNewRow["nOAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cBGChartHD"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBGChart", "BGChart", "select * from BGChartHD where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);

                }
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
                case "TXTQCJOB":
                case "TXTQNJOB":
                    //this.pmInitPopUpDialog("JOB");
                    //strPrefix = (inTextbox == "TXTQCJOB" ? "CCODE" : "CNAME");
                    //this.pofrmGetJob.ValidateField(inPara1, strPrefix, true);
                    //if (this.pofrmGetJob.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inTextbox);
                    //}
                    break;
                case "TXTREFTO":
                    if (this.mbllIsRevise)
                    {
                        this.pmInitPopUpDialog("REFTO_BGUSE");
                    }
                    else
                    {
                        this.pmInitPopUpDialog("REFTO");
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            DataRow dtrTemPd = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "GRDVIEW2_CQCBGCHART":
                case "GRDVIEW2_CQNBGCHART":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    DataRow dtrJob = this.pofrmGetBudChart.RetrieveValue();
                    if (dtrJob != null)
                    {
                        dtrTemPd["cBGChartHD"] = dtrJob["cRowID"].ToString();
                        dtrTemPd["cQcBGChart"] = dtrJob["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnBGChart"] = dtrJob["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrTemPd["cBGChartHD"] ="";
                        dtrTemPd["cQcBGChart"] = "";
                        dtrTemPd["cQnBGChart"] = "";
                    }
                    this.gridView2.UpdateCurrentRow();
                    break;

            }
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                { 
                    case "TXCOPY":
                        //TODO: เพิ่ม Function Copy เอกสาร
                        MessageBox.Show("เพิ่ม Function Copy เอกสาร");
                        //this.pmCopyDoc();
                        break;
                    default:

                        WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                        #region "Toolbar Events"

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
                            case WsToolBar.Cancel:
                                this.pmCancelData();
                                break;
                            case WsToolBar.Close:
                                this.pmCloseData();
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

                        #endregion
                        
                        break;
                }

            }
        }

        private void pmCancelData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cDesc1"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), DocEditType.Cancel, ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการยกเลิกเอกสารเลขที่ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string strCanCleRem = "";
                    using (Common.dlgGetCancleRem dlg = new Common.dlgGetCancleRem())
                    {
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            strCanCleRem = dlg.Remark;
                        }
                    }
                    if (this.pmCancelDoc(this.mstrEditRowID, dtrBrow["cCode"].ToString(), strCanCleRem, ref strErrorMsg))
                    {
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[this.pmGetRowID(this.mstrEditRowID)]["CSTAT"] = "C";
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

        private void pmCloseData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();

            string strIsClose = dtrBrow["cIsClose"].ToString();
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cDesc1"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), DocEditType.Close, ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการ CLOSE เอกสารเลขที่ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmCloseDoc(this.mstrEditRowID, dtrBrow["cCode"].ToString(), strIsClose , ref strErrorMsg))
                    {
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[this.pmGetRowID(this.mstrEditRowID)]["CSTEP"] = (strIsClose == SysDef.gc_REF_STEP_CLOSED ? "" : "Closed");
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[this.pmGetRowID(this.mstrEditRowID)]["CISCLOSE"] = (strIsClose == SysDef.gc_REF_STEP_CLOSED ? SysDef.gc_REF_STEP_CREATE : SysDef.gc_REF_STEP_CLOSED);
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

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cDesc1"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), DocEditType.Delete, ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบเอกสารเลขที่ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow["cCode"].ToString(), ref strErrorMsg))
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

        private bool pmCheckHasUsed(string inRowID, string inCode, DocEditType inChkType, ref string ioErrorMsg)
        {
            string strErrorMsg = "";
            bool bllCanEdit = false;
            bool bllHasUsed = false;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrChkRow = this.dtsDataEnv.Tables["QHasUsed"].Rows[0];
                bllCanEdit = this.pmCanEdit(dtrChkRow, inChkType, false);
                ioErrorMsg = this.mstrCanEditMsg;
            }

            bllHasUsed = !bllCanEdit;
            return bllHasUsed;
        }

        private bool pmCancelDoc(string inRowID, string inCode, string inCancelRem , ref string ioErrorMsg)
        {
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

                string strErrorMsg = "";

                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QBGRecvH", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    //if (this.mbllIsRevise)
                    //{
                    //    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.RefToRowID].ToString();
                    //}
                    //else
                    //{
                    //    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.BGRecvHD].ToString();
                    //}

                    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.RefToRowID].ToString();
                    this.mstrRef2BGRecvHD = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.BGRecvHD].ToString();
                
                }

                this.pmUpdBudget(-1);

                pAPara = new object[2] { SysDef.gc_STAT_CANCEL, inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrITable + " set cStat = ? where cBGUSEHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[5] { SysDef.gc_STAT_CANCEL, inCancelRem, App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cStat = ? , cCancelDes = ? , cCancelBy = ? , dCancel = ?  where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                if (this.mbllIsRevise)
                {
                    this.pmSaveRefTo(false, this.mstrRef2RowID);
                }

                this.pmUpdateRecvHStep(false, this.mstrRef2BGRecvHD);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Cancel, TASKNAME, inCode, "", App.FMAppUserID, App.AppUserName);

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

        private bool pmCloseDoc(string inRowID, string inCode, string inIsClose, ref string ioErrorMsg)
        {
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

                string strErrorMsg = "";

                string strIsClose = (inIsClose == SysDef.gc_REF_STEP_CLOSED ? SysDef.gc_REF_STEP_CREATE : SysDef.gc_REF_STEP_CLOSED);

                //if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QBGRecvH", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                //{
                //    strIsClose = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.IsClose].ToString();
                //}

                pAPara = new object[2] { strIsClose, inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrITable + " set cIsClose = ? where cBGUSEHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[2] { strIsClose, inRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsClose = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Close, TASKNAME, inCode, "", App.FMAppUserID, App.AppUserName);

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

        private bool pmDeleteRow(string inRowID, string inCode, ref string ioErrorMsg)
        {
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

                string strErrorMsg = "";
                string strStatus = SysDef.gc_STAT_NORMAL;
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QBGRecvH", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    strStatus = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.Status].ToString();
                    //if (this.mbllIsRevise)
                    //{
                    //    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.RefToRowID].ToString();
                    //}
                    //else
                    //{
                    //    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.BGRecvHD].ToString();
                    //}
                    this.mstrRef2RowID = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.RefToRowID].ToString();
                    this.mstrRef2BGRecvHD = this.dtsDataEnv.Tables["QBGRecvH"].Rows[0][QBGUseInfo.Field.BGRecvHD].ToString();
                }

                if (strStatus != SysDef.gc_STAT_CANCEL)
                {
                    this.pmUpdBudget(-1);
                }

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cBGUSEHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                if (strStatus != SysDef.gc_STAT_CANCEL)
                {
                    if (this.mbllIsRevise)
                    {
                        this.pmSaveRefTo(false, this.mstrRef2RowID);
                    }

                    this.pmUpdateRecvHStep(false, this.mstrRef2BGRecvHD);
                }

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, "", App.FMAppUserID, App.AppUserName);

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
                if (MessageBox.Show("ยังไม่ได้ระบุเลขที่เอกสาร ต้องการให้เครื่อง Running ให้หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pmRunCode();
                }
            }

            if (this.mbllCanEdit == false)
            {
                ioErrorMsg = "ไม่สามารถแก้ไขเอกสารได้ !";
                this.txtCode.Focus();
                return false;
            }
            else if (this.txtCode.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสการปันส่วนงบประมาณ";
                this.txtCode.Focus();
                return false;
            }
            //else if (this.txtDate.DateTime.Year != this.mintBGYear)
            //{
            //    ioErrorMsg = "ระบุวันที่ไม่สอดคล้องกับปีงบประมาณ กรุณาตรวจสอบอีกครั้ง !";
            //    this.txtDate.Focus();
            //    return false;
            //}
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
                && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBGBook, this.mstrJob, this.mintBGYear,this.txtCode.Text.TrimEnd() }))
            {
                ioErrorMsg = "รหัสการปันส่วนงบประมาณซ้ำ";
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
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBGBook, this.mstrJob, this.mintBGYear });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cRefType = ? and cBGBook = ? and cJob = ? and nBGYear = ? and cCode < ':' order by cCode desc", ref strErrorMsg))
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
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cBranch = ? and cRefType = ? and cBGBook = ? and cJob = ? and nBGYear = ? and cCode = ?", ref strErrorMsg))
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
            int intRevise = 0;

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
                intRevise = -1;
            }
            else
            {
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
            }

            intRevise++;

            dtrSaveInfo[QBGUseInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QBGUseInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QBGUseInfo.Field.RefType] = this.mstrRefType;
            dtrSaveInfo[QBGUseInfo.Field.BGBookID] = this.mstrBGBook;
            dtrSaveInfo[QBGUseInfo.Field.SectID] = this.mstrSect;
            dtrSaveInfo[QBGUseInfo.Field.JobID] = this.mstrJob;
            dtrSaveInfo[QBGUseInfo.Field.BudGetYear] = this.mintBGYear;
            dtrSaveInfo[QBGUseInfo.Field.Code] = this.txtCode.Text.TrimEnd();
            dtrSaveInfo[QBGUseInfo.Field.Date] = this.txtDate.DateTime.Date;
            dtrSaveInfo[QBGUseInfo.Field.RefNo] = this.txtRefNo.Text.TrimEnd();
            dtrSaveInfo[QBGUseInfo.Field.Desc1] = this.txtDesc1.Text.TrimEnd();
            dtrSaveInfo[QBGUseInfo.Field.Amount] = this.pmTotAmt;

            dtrSaveInfo["cLastUpdBy"] = App.FMAppUserID.TrimEnd();
            dtrSaveInfo["dLastUpdBy"] = objSQLHelper.GetDBServerDateTime();

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                dtrSaveInfo[QBGUseInfo.Field.BGRecvHD] = this.mstrRef2BGRecvHD;

                if (this.mbllIsRevise)
                {
                    dtrSaveInfo[QBGRecvInfo.Field.RefToRefType] = this.mstrRef2RefType;
                    dtrSaveInfo[QBGRecvInfo.Field.RefToBook] = this.mstrRef2BookID;
                    dtrSaveInfo[QBGRecvInfo.Field.RefToRowID] = this.mstrRef2RowID;
                    if (this.mstrRef2RowID != this.mstrRef2RowID_Old)
                    {
                        this.pmSaveRefTo(true, this.mstrRef2RowID);
                    }
                }
                
                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.pmUpdateItem();

                this.pmUpdateRecvHStep(true, this.mstrRef2BGRecvHD);

                this.mdbTran.Commit();

                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtCode.Text, "", App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    if (this.mstrOldCode == this.txtCode.Text)
                    {
                        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, "", App.FMAppUserID, App.AppUserName);
                    }
                    else
                    {
                        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, "", App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
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

        private void pmSaveRefTo(bool IsRevise, string inRefToHD)
        {
            string strErrorMsg = "";
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QSaveItem", this.mstrITable, "select * from " + this.mstrITable + " where cBGUseHD = ?", new object[1] { inRefToHD }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                decimal decUpdSign = (IsRevise ? -1 : 1);
                foreach (DataRow dtrSaveItem in this.dtsDataEnv.Tables["QSaveItem"].Rows)
                {
                    this.pmUpdBudgetItem(decUpdSign, this.mstrJob, this.mstrSect, this.mintBGYear, dtrSaveItem["cBGChartHD"].ToString(), Convert.ToDecimal(dtrSaveItem["nAmt"]));
                }
                string strIsRev = (IsRevise ? "Y" : "");
                object[] pAPara = new object[] { strIsRev, inRefToHD };
                this.mSaveDBAgent.BatchSQLExec("update BGUSEIT set CISREV = ? where CBGUSEHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                this.mSaveDBAgent.BatchSQLExec("update BGUSEHD set CISREV = ? where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }
        }

        private void pmUpdateRecvHStep(bool IsRevise, string inRefToHD)
        {
            string strErrorMsg = "";

            string strFoxSQLStrOrderI = "select * from BGRecvIT where BGRecvIT.cBGRecvHD = ?";

            string strHStep = SysDef.gc_REF_STEP_PAY;
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefToI", "ORDERI", strFoxSQLStrOrderI, new object[] { inRefToHD }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefToI"].Rows)
                {
                    decimal decUseAmt = 0;
                    decimal decAmt = Convert.ToDecimal(dtrOrderI["nAmt"]);
                    //Sum Cut Amt
                    decUseAmt = this.pmSumCutAmt(dtrOrderI["cRowID"].ToString());
                    string strOrderIStep = SysDef.gc_REF_STEP_PAY;
                    decimal decBackAmt = 0;
                    if (decUseAmt != 0 && decUseAmt >= decAmt)
                    {
                        if (decAmt < 0) { decBackAmt = decAmt - decUseAmt; }
                    }
                    else
                    {
                        //if (dtrOrderI["fcRefPdTyp"].ToString() == "P") { strHStep = SysDef.gc_REF_STEP_CREATE; }
                        strHStep = SysDef.gc_REF_STEP_CREATE;
                        strOrderIStep = SysDef.gc_REF_STEP_CREATE;
                        decBackAmt = decAmt - decUseAmt;
                    }
                    //Update RefTo OrderI step
                    string strSQLUpdate_OrderI = "update BGRECVIT set cStep = ?, nUseAmt = ? where cRowID = ?";
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdate_OrderI, new object[] { strOrderIStep, decUseAmt, dtrOrderI["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }
                //Update RefTo OrderH step
                this.mSaveDBAgent.BatchSQLExec("update BGRECVHD set cStep = ? where cRowID = ?", new object[] { strHStep, inRefToHD }, ref strErrorMsg, this.mdbConn, this.mdbTran);

            }
        }

        private decimal pmSumCutAmt(string inChildI)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            decimal decCutQty = 0;
            string strFoxSQLStrNoteCut = "select * from BGUseIT where BGUseIT.cBGRecvIT = ? and BGUseIT.cIsRev <> 'Y' ";
            pAPara = new object[] { inChildI };
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QNoteCut", "NOTECUT", strFoxSQLStrNoteCut, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                foreach (DataRow dtrNoteCut in this.dtsDataEnv.Tables["QNoteCut"].Rows)
                {
                    pAPara = new object[] { dtrNoteCut["cBGUseHD"].ToString() };
                    if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", "select cStat from BGUSEHD where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        if (this.dtsDataEnv.Tables["QRefTo"].Rows[0]["cStat"].ToString() != "C")
                            decCutQty += Convert.ToDecimal(dtrNoteCut["nAmt"]);
                    }
                }
            }
            return decCutQty;
        }

        private void pmUpdateItem()
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            int intRecNo = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudItem = null;
                if (dtrTemPd["cBGChartHD"].ToString().TrimEnd() != string.Empty)
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudItem = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrBudItem["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrBudItem["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudItem = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;

                        decimal decAmt = Convert.ToDecimal(dtrBudItem["nAmt"]);
                        this.pmUpdBudgetItem(-1, this.mstrJob, this.mstrSect, this.mintBGYear, dtrBudItem["cBGChartHD"].ToString(), decAmt);

                    }

                    intRecNo++;
                    this.pmReplRecordItem(bllIsNewRow, intRecNo, dtrTemPd, ref dtrBudItem);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrBudItem, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty
                        && this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {

                        dtrBudItem = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];
                        this.pmUpdBudgetItem(-1, this.mstrJob, this.mstrSect, this.mintBGYear, dtrBudItem["cBGChartHD"].ToString(), Convert.ToDecimal(dtrBudItem["nAmt"]));

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
        }

        private bool pmReplRecordItem(bool inState, int inRecNo, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudItem = ioRefProd;
            DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrBudItem["cCorp"] = App.ActiveCorp.RowID;
            dtrBudItem["cBranch"] = this.mstrBranch;
            dtrBudItem["cRefType"] = this.mstrRefType;
            dtrBudItem["nBGYear"] = this.mintBGYear;
            dtrBudItem["cBGUSEHD"] = dtrBudCH["cRowID"].ToString();
            dtrBudItem["dDate"] = Convert.ToDateTime(dtrBudCH["dDate"]);
            dtrBudItem["cBGChartHD"] = inTemPd["cBGChartHD"].ToString();
            dtrBudItem["cSect"] = dtrBudCH["cSect"].ToString();
            dtrBudItem["cDept"] = dtrBudCH["cDept"].ToString();
            dtrBudItem["cJob"] = dtrBudCH["cJob"].ToString();
            dtrBudItem["cProj"] = dtrBudCH["cProj"].ToString();
            dtrBudItem["cDesc1"] = inTemPd["cDesc1"].ToString().TrimEnd();
            dtrBudItem["cDesc2"] = inTemPd["cDesc2"].ToString().TrimEnd();
            dtrBudItem["cDesc3"] = inTemPd["cDesc3"].ToString().TrimEnd();
            dtrBudItem["cDesc4"] = inTemPd["cDesc4"].ToString().TrimEnd();
            dtrBudItem["cDesc5"] = inTemPd["cDesc5"].ToString().TrimEnd();
            dtrBudItem["cDesc6"] = inTemPd["cDesc6"].ToString().TrimEnd();
            dtrBudItem["cDesc7"] = inTemPd["cDesc7"].ToString().TrimEnd();
            dtrBudItem["cDesc8"] = inTemPd["cDesc8"].ToString().TrimEnd();
            dtrBudItem["cDesc9"] = inTemPd["cDesc9"].ToString().TrimEnd();
            dtrBudItem["cDesc10"] = inTemPd["cDesc10"].ToString().TrimEnd();
            dtrBudItem["nAmt"] = Convert.ToDecimal(inTemPd["nAmt"]);
            dtrBudItem["cSeq"] = StringHelper.ConvertToBase64(inRecNo , 2);
            dtrBudItem["cBGRecvHD"] = inTemPd["cBGRecvHD"].ToString().TrimEnd();
            dtrBudItem["cBGRecvIT"] = inTemPd["cBGRecvIT"].ToString().TrimEnd();

            if (this.mbllIsRevise)
            {
                dtrBudItem["nRefAmt"] = Convert.ToDecimal(inTemPd["nAmt"]);
                dtrBudItem["nAmt"] = Convert.ToDecimal(inTemPd["nRevAmt"]);
                dtrBudItem["cRefToHD"] = inTemPd["cRefToHD"].ToString().TrimEnd();
                dtrBudItem["cRefToIT"] = inTemPd["cRefToIT"].ToString().TrimEnd();
            }

            decimal decAmt = Convert.ToDecimal(dtrBudItem["nAmt"]);
            //เพิ่มยอดจองงบประมาณ
            this.pmUpdBudgetItem(1, this.mstrJob, this.mstrSect, this.mintBGYear, inTemPd["cBGChartHD"].ToString(), decAmt);

            return true;
        }

        private void pmUpdBudgetItem(decimal inUpdSign, string inJob, string inSect , int inBGYear, string inBGChartHD, decimal inAmt)
        {
            string strErrorMsg = "";
            this.BeSmartMRPAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
            this.BeSmartMRPAgent.UpdateUseBudget(inUpdSign, App.ActiveCorp.RowID, this.mstrBranch, " ", this.mintBGYear, inSect, inJob, inBGChartHD, inAmt, ref strErrorMsg);
        }

        private void pmUpdBudget(decimal inUpdSign)
        {
            string strErrorMsg = "";
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QSaveMBal", this.mstrITable, "select * from " + this.mstrITable + " where cBGUSEHD = ? order by cSeq", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {

                foreach (DataRow dtrBudI in this.dtsDataEnv.Tables["QSaveMBal"].Rows)
                {
                    this.pmUpdBudgetItem(-1, dtrBudI["cJob"].ToString(), dtrBudI["cSect"].ToString(), this.mintBGYear, dtrBudI["cBGChartHD"].ToString(), Convert.ToDecimal(dtrBudI["nAmt"]));
                }
            }
        }

        private void pmCreateTem()
        {

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataTable dtbTemPdVer = new DataTable(this.mstrTemPd);

            dtbTemPdVer.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("cBGChartHD", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcBGChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnBGChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc1", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc2", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc3", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc4", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc5", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc6", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc7", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc8", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc9", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cDesc10", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nOAmt", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nRevAmt", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nUseAmt", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("cRefToHD", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cRefToIT", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cBGRecvHD", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cBGRecvIT", System.Type.GetType("System.String"));

            dtbTemPdVer.Columns["cRowID"].DefaultValue = "";
            dtbTemPdVer.Columns["nRecNo"].DefaultValue = 0;
            dtbTemPdVer.Columns["cBGChartHD"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcBGChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnBGChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc1"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc2"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc3"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc4"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc5"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc6"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc7"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc8"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc9"].DefaultValue = "";
            dtbTemPdVer.Columns["cDesc10"].DefaultValue = "";
            dtbTemPdVer.Columns["cRefToIT"].DefaultValue = "";
            dtbTemPdVer.Columns["cRefToHD"].DefaultValue = "";
            dtbTemPdVer.Columns["cBGRecvHD"].DefaultValue = "";
            dtbTemPdVer.Columns["cBGRecvIT"].DefaultValue = "";
            
            dtbTemPdVer.Columns["nAmt"].DefaultValue = 0;
            dtbTemPdVer.Columns["nOAmt"].DefaultValue = 0;
            dtbTemPdVer.Columns["nRevAmt"].DefaultValue = 0;
            dtbTemPdVer.Columns["nUseAmt"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPdVer);

        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();

            this.txtRefNo.Focus();

            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmLoadFormData();
                this.mbllCanEdit = this.pmCanEdit(this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0], DocEditType.Edit, true);
            }
        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);
            
            this.mstrEditRowID = "";
            this.mstrRef2BookID = "";
            this.mstrRef2QcBook = "";
            this.mstrRef2RowID = "";
            this.mstrRef2BGRecvHD = "";
            this.mstrRef2RowID_Old = this.mstrRef2RowID;

            this.txtCode.Text = "";
            this.txtDate.DateTime = DateTime.Now;
            this.txtRefNo.Text = "";
            this.txtRefTo.Text = "";
            this.txtDesc1.Text = "";
            this.txtTotAmt.Text = "";
            this.txtBalAmt.Text = "";

            decimal decAmt = 0; decimal decRecvAmt = 0; decimal decUseAmt = 0; decimal decBalAmt = 0;

            BudgetHelper.SuBeSmartMRP(pobjSQLUtil
                , App.ActiveCorp.RowID, this.mstrBranch, "", this.mintBGYear, this.mstrSect, this.mstrJob
                , ref decAmt, ref decRecvAmt, ref decUseAmt, ref decBalAmt);

            this.txtTotAmt.Text = decAmt.ToString("#,###,###,###.00");
            this.txtBalAmt.Text = decBalAmt.ToString("#,###,###,###.00");

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcJob"];

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
                    this.txtDate.DateTime = Convert.ToDateTime(dtrLoadForm["dDate"]);
                    this.txtRefNo.Text = dtrLoadForm["cRefNo"].ToString().TrimEnd();
                    this.txtDesc1.Text = dtrLoadForm["cDesc1"].ToString().TrimEnd();

                    this.txtQcJob.Tag = dtrLoadForm["cJob"].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcJob.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from EMJOB where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrJob = this.dtsDataEnv.Tables["QJob"].Rows[0];
                        this.txtQcJob.Text = dtrJob["cCode"].ToString().TrimEnd();
                        this.txtQnJob.Text = dtrJob["cName"].ToString().TrimEnd();

                        this.txtQcJob2.Text = this.txtQcJob.Text;
                        this.txtQnJob2.Text = this.txtQnJob.Text;
                    
                    }

                    this.mstrRef2BookID = dtrLoadForm[QBGUseInfo.Field.RefToBook].ToString();
                    if (this.mbllIsRevise)
                    {
                        this.mstrRef2RowID = dtrLoadForm[QBGUseInfo.Field.RefToRowID].ToString();
                        pobjSQLUtil.SetPara(new object[] { this.mstrRef2RowID });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", "BGUSEHD", "select BGBOOK.CCODE as QCBOOK, BGUSEHD.CCODE, BGUSEHD.CBGRECVHD from BGUSEHD left join BGBOOK on BGBOOK.CROWID = BGUSEHD.cBGBook where BGUSEHD.cRowID = ?", ref strErrorMsg))
                        {
                            DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                            this.txtRefTo.Text = dtrRefTo["QcBook"].ToString().TrimEnd() + "/" + dtrRefTo["cCode"].ToString().TrimEnd();
                            this.mstrRef2BGRecvHD = dtrRefTo["cBGRecvHD"].ToString();
                        }
                    }
                    else
                    {
                        this.mstrRef2RowID = "";
                        this.mstrRef2BGRecvHD = dtrLoadForm[QBGUseInfo.Field.BGRecvHD].ToString();
                        pobjSQLUtil.SetPara(new object[] { this.mstrRef2BGRecvHD });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefTo", "BGUSEHD", "select BGBOOK.CCODE as QCBOOK, BGRECVHD.CCODE from BGRECVHD left join BGBOOK on BGBOOK.CROWID = BGRECVHD.cBGBook where BGRECVHD.cRowID = ?", ref strErrorMsg))
                        {
                            DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                            this.txtRefTo.Text = dtrRefTo["QcBook"].ToString().TrimEnd() + "/" + dtrRefTo["cCode"].ToString().TrimEnd();
                        }
                    }

                    this.mstrRef2RowID_Old = this.mstrRef2RowID;
                    this.pmLoadItem();
                
                    this.pmCalTotAmt();

                }
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtCode.Text;
        }

        private bool pmCanEdit(DataRow inBgTran, DocEditType inEditType, bool inShowMsg)
        {
            this.mstrCanEditMsg = "";
            string strMsg1 = "แก้ไข";
            switch (inEditType)
            {
                case DocEditType.Edit:
                    strMsg1 = "แก้ไข";
                    break;
                case DocEditType.Delete:
                    strMsg1 = "ลบ";
                    break;
                case DocEditType.Close:
                    strMsg1 = " CLOSE ";
                    break;
            }
            bool bllResult = true;

            if (inBgTran[QBGUseInfo.Field.Step].ToString() == SysDef.gc_REF_STEP_COMPLETE)
            {
                this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากเอกสารถูกอ้างอิงไปหมดแล้ว";
                bllResult = false;
            }
            else if (inBgTran[QBGUseInfo.Field.IsRevise].ToString() == "Y")
            {
                this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากเอกสารถูกอ้างอิงไปปรับแก้ไขแล้ว";
                bllResult = false;
            }
            else if (inEditType != DocEditType.Delete && inBgTran[QBGUseInfo.Field.Status].ToString() == SysDef.gc_STAT_CANCEL)
            {
                this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากเอกสารถูก CANCEL แล้ว";
                bllResult = false;
            }
            else if (inEditType == DocEditType.Edit
                && inBgTran[QBGUseInfo.Field.Step].ToString() == SysDef.gc_REF_STEP_CLOSED)
            {
                this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากเอกสารถูก CLOSED แล้ว";
                bllResult = false;
            }

            if (!bllResult && inShowMsg)
            {
                MessageBox.Show(this.mstrCanEditMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return bllResult;
        }

        private void pmLoadItem()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudItem", "BUDCI", "select * from " + this.mstrITable + " where cBGUSEHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBudItem in this.dtsDataEnv.Tables["QBudItem"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrBudItem["cRowID"].ToString();
                    dtrNewRow["cBGChartHD"] = dtrBudItem["cBGChartHD"].ToString();
                    dtrNewRow["cDesc1"] = dtrBudItem["cDesc1"].ToString().TrimEnd();
                    dtrNewRow["cDesc2"] = dtrBudItem["cDesc2"].ToString().TrimEnd();
                    dtrNewRow["cDesc3"] = dtrBudItem["cDesc3"].ToString().TrimEnd();
                    dtrNewRow["cDesc4"] = dtrBudItem["cDesc4"].ToString().TrimEnd();
                    dtrNewRow["cDesc5"] = dtrBudItem["cDesc5"].ToString().TrimEnd();
                    dtrNewRow["cDesc6"] = dtrBudItem["cDesc6"].ToString().TrimEnd();
                    dtrNewRow["cDesc7"] = dtrBudItem["cDesc7"].ToString().TrimEnd();
                    dtrNewRow["cDesc8"] = dtrBudItem["cDesc8"].ToString().TrimEnd();
                    dtrNewRow["cDesc9"] = dtrBudItem["cDesc9"].ToString().TrimEnd();
                    dtrNewRow["cDesc10"] = dtrBudItem["cDesc10"].ToString().TrimEnd();

                    dtrNewRow["nAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);
                    dtrNewRow["nOAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);
                    dtrNewRow["nUseAmt"] = Convert.ToDecimal(dtrBudItem["nUseAmt"]);

                    dtrNewRow["cBGRecvHD"] = dtrBudItem["cBGRecvHD"].ToString();
                    dtrNewRow["cBGRecvIT"] = dtrBudItem["cBGRecvIT"].ToString();

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cBGChartHD"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBGChart", "BGChart", "select * from BGChartHD where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnBGChart"] = this.dtsDataEnv.Tables["QBGChart"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    if (this.mbllIsRevise)
                    {
                        dtrNewRow["nAmt"] = Convert.ToDecimal(dtrBudItem["nRefAmt"]);
                        dtrNewRow["nRevAmt"] = Convert.ToDecimal(dtrBudItem["nAmt"]);
                        dtrNewRow["cRefToHD"] = dtrBudItem["cRefToHD"].ToString().TrimEnd();
                        dtrNewRow["cRefToIT"] = dtrBudItem["cRefToIT"].ToString().TrimEnd();
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
                            this.mbllFilterResult = false;
                            this.pmInitPopUpDialog("FILTER");
                            if (!this.mbllFilterResult)
                            {
                                this.Close();
                            }
                            //this.Close();
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
                if (dtrBrow["CSTAT"].ToString() == SysDef.gc_STAT_CANCEL)
                {
                    this.txtFooter.Text += "\r\nยกเลิกโดย LOGIN : " + dtrBrow["CLOGIN_CAN"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCancel"]).ToString("dd/MM/yy hh:mm:ss");
                    this.txtFooter.Text += "\r\nสาเหตุการยกเลิก : " + dtrBrow["CCANCELDES"].ToString().TrimEnd();
                }
            }
        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "CCODE")
                inSearchStr = inSearchStr.PadRight(10);

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

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            string strOrderBy = "CCODE";
            switch (inKeyField.ToUpper())
            {

                case "CDESC1":
                    this.pmInitPopUpDialog("REMARK");
                    break;
                case "CQCBGCHART":
                    strOrderBy = (inKeyField.ToUpper() == "CQCBGCHART" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("BUDCHART");
                    this.pofrmGetBudChart.ValidateField(this.mintBGYear, this.mstrJob, "", strOrderBy, true);
                    if (this.pofrmGetBudChart.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_" + inKeyField);
                    }
                    break;
                case "CQNBGCHART":
                    this.pmInitPopUpDialog("BGBAL");
                    break;
            }
        }

        private void grcColumn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
        }

        private void grcColumn_KeyDown(object sender, KeyEventArgs e)
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

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strValue = "";
            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCBGCHART":
                case "CQNBGCHART":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cBGChartHD"] = "";
                        dtrTemPd["cQcBGChart"] = "";
                        dtrTemPd["cQnBGChart"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("BUDCHART");
                        string strOrderBy = (strCol.ToUpper() == "CQCBGCHART" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetBudChart.ValidateField(this.mintBGYear, this.mstrJob, strValue, strOrderBy, false);

                        if (this.pofrmGetBudChart.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCBGCHART" ? dtrTemPd["cQcBGChart"].ToString().TrimEnd() : dtrTemPd["cQnBGChart"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cBGChartHD"] = "";
                            dtrTemPd["cQcBGChart"] = "";
                            dtrTemPd["cQnBGChart"] = "";
                            dtrTemPd["nAmt"] = 0;
                        }
                    }
                    break;
            }

        }

        private void grcAmt_Leave(object sender, EventArgs e)
        {
            this.gridView2.UpdateCurrentRow();
            this.pmCalTotAmt();
        }


        private void pmCalTotAmt()
        {
            decimal decTotAmt = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                if (this.mbllIsRevise)
                {
                    decTotAmt += Convert.ToDecimal(dtrTemPd["nRevAmt"]);
                }
                else
                {
                    decTotAmt += Convert.ToDecimal(dtrTemPd["nAmt"]);
                }
            }
            this.pmTotAmt = decTotAmt;
            //this.txtTotAmt.Text = decTotAmt.ToString("#,###,###,###.00");
        }

        private void grcAmt_Validating(object sender, CancelEventArgs e)
        {

            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            DevExpress.XtraEditors.SpinEdit oSender = sender as DevExpress.XtraEditors.SpinEdit;

            if (dtrTemPd != null && oSender != null)
            {

                decimal decRevAmt = 0;
                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                pobjSQLUtil.SetPara(new object[] { dtrTemPd["cBGRecvIT"].ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudItem", "BUDCI", "select nAmt,nUseAmt from BGRECVIT where cRowID = ? ", ref strErrorMsg))
                {
                    DataRow dtrBudItem = this.dtsDataEnv.Tables["QBudItem"].Rows[0];
                    decRevAmt = Convert.ToDecimal(dtrTemPd["nOAmt"]) + Convert.ToDecimal(dtrBudItem["nAmt"]) - Convert.ToDecimal(dtrBudItem["nUseAmt"]);
                }
                decimal decAmt = oSender.Value;
                if (decAmt > decRevAmt)
                {
                    e.Cancel = true;
                }
            }

        }


    }
}
