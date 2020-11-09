
#define xd_RUNMODE_DEBUG

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

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.DatabaseForms
{

    /// <summary>
    /// Input การตั้งงบประมาณ
    /// RefType (mstrRefType) มี 3 ประเภทเอกสารดังนี้ :
    ///     B1 หมายถึง ตอนตั้งงบประมาณ
    ///     B2 หมายถึง ตอนตั้งงบประมาณเหลื่อมปี (โหลดโครงการเดียวกันที่เหลือจ่ายมาตั้ง)
    ///     B3 หมายถึง ตอนตั้งงบประมาณเหลือจ่าย  (โหลดโครงการใด ๆ ที่เหลือจ่ายมาตั้ง)
    /// 
    /// BudgetStep (BeSmartMRPStep) มี 4 Step ของเอกสารดังนี้ :
    ///     BudgetStep.Prepare หมายถึง ตอนตั้งงบประมาณ
    ///     BudgetStep.Revise หมายถึง ตอนแก้ไขการตั้งงบประมาณ
    ///     BudgetStep.Approve หมายถึง ตอนพิจารณาการตั้งงบประมาณ
    ///     BudgetStep.Post หมายถึง ตอนPOST รายการการตั้งงบประมาณ
    /// 
    /// การอนุมัติเอกสารใน Filed ต่าง ๆ ดังนี้ :
    ///     Approve ในขั้น BudgetStep.Prepare :
    ///     Approve ในขั้น BudgetStep.Revise :
    ///        CISAPPROVE => อนุมัติจาก Role หัวหน้า Staff
    ///        CAPPROVEBY
    ///        DAPPROVE
    ///     
    ///     Approve ในขั้น BudgetStep.Approve :
    ///     CISCORRECT => อนุมัติขั้นต้นจาก Role เจ้าหน้าที่งบประมาณ/หัวหน้างบประมาณ
    ///     CCORRECTBY
    ///     DISCORRECT
    ///     
    ///     CAPVSTAT => อนุมัติขั้นต้นจาก Role หัวหน้างบประมาณ
    ///     CAPVREMARK
    ///     CAPVBY
    ///     DAPV
    ///     
    ///     CISAPPROV2 => ผ่านหน่วยงานภายนอกจาก Role หัวหน้างบประมาณ
    ///     CAPPROVEB2
    ///     DAPPROVE2
    ///     
    ///     CISPOST => POST จาก Role หัวหน้างบประมาณ
    ///     CPOSYBY
    ///     DPOST
    /// </summary>

    public partial class frmBudTran : UIHelper.frmBase
    {
        
        public static string TASKNAME = "EBGPRE_";

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

        private string mstrRefTable = MapTable.Table.BudTranHD;
        private string mstrITable = MapTable.Table.BudTranIT;

        private string mstrTemPd = "TemPd";
        private string mstrMenuName = "บันทึกงบประมาณก่อนการอนุมัติ";

        private string mstrBranch = "";
        private string mstrSect = "";
        private string mstrBGYear = "";
        private int mintBGYear = 0;

        private decimal pmTotAmt = 0;
        private string mstrCurFilterStat = "X";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldJob = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;
        private bool mbllCanEdit = true;
        private bool mbllCanEditHeadOnly = false;
        private string mstrCanEditMsg = "";
        private decimal mdecUpdBudSign = -1;

        private string mstrRefType = "B1";

        private string mstrApproveStep = "";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private BudgetAgent BeSmartMRPAgent = new BudgetAgent();

        private DatabaseForms.frmBudType pofrmGetBudType = null;
        private DatabaseForms.frmBudChart pofrmGetBudChart = null;
        private DatabaseForms.frmEMJob pofrmGetJob = null;
        private DatabaseForms.frmEMDept pofrmGetDept = null;
        private DatabaseForms.frmEMUOM pofrmGetUM = null;
        private DatabaseForms.frmAppEmpl pofrmGetEmpl = null;

        #region "Grid Action Method"
        private void btnInsertRow_Click(object sender, System.EventArgs e)
        {
            this.pmInsertGridRow();
        }

        private void btnRemoveRow_Click(object sender, System.EventArgs e)
        {
            this.pmClr1TemPd();
            //DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            //dtrTemPd["cIsDel"] = "Y";
        }

        private void btnMoveRowUp_Click(object sender, System.EventArgs e)
        {
            this.pmMoveGridRow(1);
        }

        private void btnMoveRowDown_Click(object sender, System.EventArgs e)
        {
            this.pmMoveGridRow(-1);
        }

        private void pmInsertGridRow()
        {
            if (this.gridView2.FocusedRowHandle < 0
                || this.gridView2.FocusedRowHandle == this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count)
                return;

            int intFocus = this.gridView2.FocusedRowHandle;
            DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            DataRow dtrOldRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            for (int intCnt = this.gridView2.FocusedRowHandle; intCnt < this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count; intCnt++)
            {
                DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[intCnt];
                dtrNewRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]);
                dtrCurrRow["nRecNo"] = Convert.ToInt32(dtrCurrRow["nRecNo"]) + 1;

                DataSetHelper.CopyDataRow(dtrCurrRow, ref dtrOldRow);
                DataSetHelper.CopyDataRow(dtrNewRow, ref dtrCurrRow);
                DataSetHelper.CopyDataRow(dtrOldRow, ref dtrNewRow);
            }
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);
            this.gridView2.FocusedRowHandle = intFocus;
            //this.gridView2.Refetch();
            //this.gridView2.Refresh();
        }

        private void pmMoveGridRow(int inDirection)
        {
            if ((inDirection == 1 && this.gridView2.FocusedRowHandle == 0)
                || inDirection == -1 && this.gridView2.FocusedRowHandle == this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Count - 1)
                return;

            DataRow dtrSwapRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle];
            DataRow dtrUpRow = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle - inDirection];

            int intSwapRecNo = Convert.ToInt32(dtrCurrRow["nRecNo"]);
            dtrCurrRow["nRecNo"] = Convert.ToInt32(dtrUpRow["nRecNo"]);
            dtrUpRow["nRecNo"] = intSwapRecNo;

            DataSetHelper.CopyDataRow(dtrCurrRow, ref dtrSwapRow);
            DataSetHelper.CopyDataRow(dtrUpRow, ref dtrCurrRow);
            DataSetHelper.CopyDataRow(dtrSwapRow, ref dtrUpRow);

            this.gridView2.FocusedRowHandle = this.gridView2.FocusedRowHandle - inDirection;
            //this.gridView2.Refetch();
            //this.gridView2.Refresh();
        }
        #endregion

        public frmBudTran(string inRefType, string inStep)
        {
            InitializeComponent();

            this.mstrRefType = inRefType;
            this.mstrBudgetStep = inStep;
            this.BeSmartMRPStep = BudgetHelper.GetBudgetStep(inStep);

            this.pmInitForm();
        }

        private static frmBudTran mInstanse_1 = null;
        private static frmBudTran mInstanse_2 = null;
        private static frmBudTran mInstanse_3 = null;
        private static frmBudTran mInstanse_4 = null;

        public static frmBudTran GetInstanse(string inRefType, string inStep)
        {
            BudgetStep oResult = BudgetHelper.GetBudgetStep(inStep);
            switch (oResult)
            {
                case BudgetStep.Prepare:
                    if (mInstanse_1 == null)
                    {
                        mInstanse_1 = new frmBudTran(inRefType, inStep);
                    }
                    return mInstanse_1;
                    break;
                case BudgetStep.Approve:
                    if (mInstanse_2 == null)
                    {
                        mInstanse_2 = new frmBudTran(inRefType, inStep);
                    }
                    return mInstanse_2;
                    break;
                case BudgetStep.Post:
                    if (mInstanse_3 == null)
                    {
                        mInstanse_3 = new frmBudTran(inRefType, inStep);
                    }
                    return mInstanse_3;
                    break;
                case BudgetStep.Revise:
                    if (mInstanse_4 == null)
                    {
                        mInstanse_4 = new frmBudTran(inRefType, inStep);
                    }
                    return mInstanse_4;
                    break;
            }
            return null;
        }

        private static void pmClearInstanse(string inStep)
        {

            if (mInstanse_1 != null
                && inStep == SysDef.gc_APPROVE_STEP_WAIT)
            {
                mInstanse_1 = null;
            }

            if (mInstanse_2 != null
                && inStep == SysDef.gc_APPROVE_STEP_APPROVE)
            {
                mInstanse_2 = null;
            }

            if (mInstanse_3 != null
                && inStep == SysDef.gc_APPROVE_STEP_POST)
            {
                mInstanse_3 = null;
            }

            if (mInstanse_4 != null
                && inStep == SysDef.gc_APPROVE_STEP_REVISE)
            {
                mInstanse_4 = null;
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
            this.pmFilterForm();
            this.pmGotoBrowPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();

            UIBase.WaitClear();

            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
            if (!this.mbllFilterResult)
            {
                this.Close();
                //frmBudTran.pmClearInstanse();
            }
        }

        private void pmInitializeComponent()
        {
    
            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }

            this.grcApprove.Items.Clear();
            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:

                    this.grcApprove.Items.AddRange(new object[] { " ", "/" });
                    
                    this.mstrMenuName = "เมนูบันทึกงบประมาณก่อนการอนุมัติ";
                    this.grbApprove.Text = "ผู้อนุมัติเบื้องต้น (หัวหน้าหน่วยงานผู้เสนองบประมาณ)";
                    this.lblApprove.Text = "อนุมัติ :";

                    this.grbAPVStat.Visible = false;
                    this.pnlGridControl.Visible = true;

                switch (this.mstrRefType)
                    { 
                        case "B1":
                            TASKNAME = "EBGPRE_PP";
                            break;
                        case "B2":
                            TASKNAME = "EBGPRE_PP_V";
                            break;
                        case "B3":
                            TASKNAME = "EBGPRE_PP_R";
                            break;
                    }

                    break;
                case BudgetStep.Approve:

                    //this.grcApprove.Items.AddRange(new object[] { " ", "/", "R" });
                    this.grcApprove.Items.AddRange(new object[] { " ", "/" });
                    
                    this.mstrMenuName = "เมนูพิจารณางบประมาณ";
                    this.grbApprove.Text = "ผู้อนุมัติเบื้องต้น (เจ้าหน้าที่หน่วยงานงบประมาณ)";
                    this.lblApprove.Text = "อนุมัติ :";

                    this.txtQcJob.Enabled = false;
                    this.txtQcEmpl.Enabled = false;
                    this.grbAPVStat.Visible = true;
                    this.pnlGridControl.Visible = false;
                    //19/01/09 By Yod
                    //this.txtIsApprove.Enabled = false;

                    switch (this.mstrRefType)
                    {
                        case "B1":
                            TASKNAME = "EBGPRE_AP";
                            break;
                        case "B2":
                            TASKNAME = "EBGPRE_AP_V";
                            break;
                        case "B3":
                            TASKNAME = "EBGPRE_AP_R";
                            break;
                    }

                    break;
                case BudgetStep.Post:

                    this.grcApprove.Items.AddRange(new object[] { " ", "/" });
                    
                    this.mstrMenuName = "เมนู POST งบประมาณ";
                    this.lblApprove.Text = "POST :";
                    this.grbApprove.Text = "";

                    this.txtQcJob.Enabled = false;
                    this.txtQcEmpl.Enabled = false;
                    this.grbAPVStat.Visible = false;
                    this.pnlGridControl.Visible = false;

                    switch (this.mstrRefType)
                    {
                        case "B1":
                            TASKNAME = "EBGPRE_PS";
                            break;
                        case "B2":
                            TASKNAME = "EBGPRE_PS_V";
                            break;
                        case "B3":
                            TASKNAME = "EBGPRE_PS_R";
                            break;
                    }

                    break;
                case BudgetStep.Revise:

                    this.grcApprove.Items.AddRange(new object[] { " ", "/" });
                    
                    this.mstrMenuName = "เมนูปรับแก้ไขงบประมาณ";
                    this.grbApprove.Text = "ผู้อนุมัติเบื้องต้น (หัวหน้าหน่วยงานผู้เสนองบประมาณ)";
                    this.lblApprove.Text = "อนุมัติ :";

                    this.txtQcJob.Enabled = false;
                    this.txtQcEmpl.Enabled = false;
                    this.grbAPVStat.Visible = false;
                    this.pnlGridControl.Visible = true;

                    switch (this.mstrRefType)
                    {
                        case "B1":
                            TASKNAME = "EBGPRE_RV";
                            break;
                        case "B2":
                            TASKNAME = "EBGPRE_RV_V";
                            break;
                        case "B3":
                            TASKNAME = "EBGPRE_RV_R";
                            break;
                    }

                    break;
            }

            this.tbrType.EditValue = "";
            this.pmSetMaxLength();
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcJob.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QEMJobInfo.TableName, QEMJobInfo.Field.Code);
            this.txtQcEmpl.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QAppEmplInfo.TableName, QAppEmplInfo.Field.Code);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;

            string strJoinApprove1 = "";
            string strJoinApprove2 = "";

            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                case BudgetStep.Revise:
                    strJoinApprove1 = " left join {1} EM3 ON EM3.CROWID = BGTRANHD.CAPPROVEBY ";
                    strJoinApprove2 = ", BGTRANHD.DAPPROVE as DAPPROVE ";
                    break;
                case BudgetStep.Approve:
                    strJoinApprove1 = " left join {1} EM3 ON EM3.CROWID = BGTRANHD.CCORRECTBY ";
                    strJoinApprove1 += " left join {1} EM4 ON EM4.CROWID = BGTRANHD.CAPVBY ";
                    strJoinApprove2 = ", BGTRANHD.DISCORRECT as DAPPROVE ";
                    strJoinApprove2 += ", BGTRANHD.DAPV as DAPV , EM4.CLOGIN as CLOGIN_APV2 ";
                    break;
                case BudgetStep.Post:
                    strJoinApprove1 = " left join {1} EM3 ON EM3.CROWID = BGTRANHD.CAPVBY ";
                    strJoinApprove1 += " left join {1} EM4 ON EM4.CROWID = BGTRANHD.CPOSTBY ";
                    strJoinApprove2 = ", BGTRANHD.DAPV as DAPPROVE ";
                    strJoinApprove2 += ", BGTRANHD.DPOST as DPOST , EM4.CLOGIN as CLOGIN_POST ";
                    break;
            }

            string strFld = "BGTRANHD.CROWID, BGTRANHD.NAMT ";
            strFld += " , BGTRANHD.CISAPPROVE, BGTRANHD.CAPVSTAT, BGTRANHD.CISAPPROV2 , BGTRANHD.CISPOST ";
            strFld += " , BGTRANHD.CISCORRECT ";
            strFld += " , BGTRANHD.CISAPPROVE as CISAPPROVE_S, BGTRANHD.CAPVSTAT as CAPVSTAT_S, BGTRANHD.CISAPPROV2 as CISAPPROV2_S ";
            strFld += " , BGTRANHD.CISCORRECT as CISCORRECT_S";
            strFld += " , EMSECT.CCODE as QCSECT, EMSECT.CNAME as QNSECT ";
            strFld += " , EMJOB.CCODE as QCJOB, EMJOB.CNAME as QNJOB ";
            strFld += " , BGTRANHD.DCREATE, BGTRANHD.DLASTUPDBY ";
            strFld += " , EM1.CLOGIN as CLOGIN_ADD";
            strFld += " , EM2.CLOGIN as CLOGIN_UPD";
            strFld += " , EM3.CLOGIN as CLOGIN_APV " + strJoinApprove2;

            string strSQLExec = "select " + strFld + " from {0} BGTRANHD ";
            strSQLExec += " left join EMSECT ON EMSECT.CROWID = BGTRANHD.CSECT";
            strSQLExec += " left join EMJOB ON EMJOB.CROWID = BGTRANHD.CJOB ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = BGTRANHD.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = BGTRANHD.CLASTUPDBY ";
            strSQLExec += strJoinApprove1;

            bool bllIsFilterSect = true;
            if (this.BeSmartMRPStep == BudgetStep.Post && this.mstrSect.Trim() == string.Empty)
            {
                bllIsFilterSect = false;
            }

            strSQLExec += " where BGTRANHD.CCORP = ? and BGTRANHD.CBRANCH = ? " + (bllIsFilterSect ? " and BGTRANHD.CSECT = ? " : "") + " and BGTRANHD.NBGYEAR = ? ";

            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    break;
                case BudgetStep.Approve:
                    strSQLExec += " and BGTRANHD.CISAPPROVE = '" + SysDef.gc_APPROVE_STEP_APPROVE + "'";
                    break;
                case BudgetStep.Post:
                    strSQLExec += " and BGTRANHD.CISAPPROV2 = '" + SysDef.gc_APPROVE_STEP_APPROVE + "'";
                    break;
                case BudgetStep.Revise:
                    strSQLExec += " and (BGTRANHD.CISAPPROVE = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CISCORRECT = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CAPVSTAT = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CISAPPROV2 = '" + SysDef.gc_APPROVE_STEP_REJECT + "' )";
                    break;
            }

            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            //pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.NotUpperSQLExecString = true;
            //pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrSect, this.mintBGYear });
            if (bllIsFilterSect)
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrSect, this.mintBGYear });
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mintBGYear });
            }
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "BGAllocate", strSQLExec, ref strErrorMsg);

            //DataColumn dtcApprove = new DataColumn("BAPPROVE", Type.GetType("System.Boolean"));
            //dtcApprove.DefaultValue = false;
            DataColumn dtcApprove = new DataColumn("BAPPROVE", Type.GetType("System.String"));
            dtcApprove.DefaultValue = "";
            DataColumn dtcApprove2 = new DataColumn("CSTAT", Type.GetType("System.String"));
            dtcApprove2.DefaultValue = false;
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcApprove);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcApprove2);

            DataColumn dtcAPVStat = new DataColumn("BAPVSTAT2", Type.GetType("System.String"));
            dtcAPVStat.DefaultValue = "";
            DataColumn dtcApvStat2 = new DataColumn("CAPVSTAT2", Type.GetType("System.String"));
            dtcApvStat2.DefaultValue = false;
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcAPVStat);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcApvStat2);

            DataColumn dtcIsOut = new DataColumn("BISOUT", Type.GetType("System.Boolean"));
            dtcIsOut.DefaultValue = false;
            DataColumn dtcIsOut2 = new DataColumn("CISOUT", Type.GetType("System.String"));
            dtcIsOut2.DefaultValue = "N";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcIsOut);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcIsOut2);

            DataColumn dtcIsPost = new DataColumn("BISPOST", Type.GetType("System.Boolean"));
            dtcIsPost.DefaultValue = false;
            DataColumn dtcIsPost2 = new DataColumn("CISPOST2", Type.GetType("System.String"));
            dtcIsPost2.DefaultValue = "N";
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcIsPost);
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcIsPost2);

            foreach (DataRow dtrBrowView in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                dtrBrowView["bApprove"] = "";
                dtrBrowView["cStat"] = "";

                dtrBrowView["BAPVSTAT2"] = "";
                dtrBrowView["CAPVSTAT2"] = "";

                dtrBrowView["bIsOut"] = false;
                dtrBrowView["cIsOut"] = "N";

                dtrBrowView["bIsPost"] = false;
                dtrBrowView["cIsPost2"] = "WAIT";

                string strIsApprove = "";
                string strApprove = "";

                switch (this.BeSmartMRPStep)
                {
                    case BudgetStep.Prepare:
                        strIsApprove = dtrBrowView[QBGTranInfo.Field.IsApprove].ToString().Trim();
                        strApprove = "";
                        switch (BudgetHelper.GetApproveStep(strIsApprove))
                        {
                            case ApproveStep.Wait:
                                strApprove = " ";
                                break;
                            case ApproveStep.Approve:
                                strApprove = "/";
                                break;
                            case ApproveStep.Reject:
                                strApprove = "R";
                                break;
                        }

                        dtrBrowView["bApprove"] = strApprove;
                        dtrBrowView["cStat"] = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsApprove));
                        break;
                    case BudgetStep.Approve:
                        strIsApprove = dtrBrowView[QBGTranInfo.Field.IsCorrect].ToString().Trim();
                        strApprove = "";
                        switch (BudgetHelper.GetApproveStep(strIsApprove))
                        {
                            case ApproveStep.Wait:
                                strApprove = " ";
                                break;
                            case ApproveStep.Approve:
                                strApprove = "/";
                                break;
                            case ApproveStep.Reject:
                                strApprove = "R";
                                break;
                        }

                        dtrBrowView["bApprove"] = strApprove;
                        dtrBrowView["cStat"] = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsApprove));
                        ////////////////

                        string strAPVStat = dtrBrowView[QBGTranInfo.Field.APVStatus].ToString().Trim();
                        string strAPVStat2 = "";
                        string strApproveStatText = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strAPVStat));
                        switch (BudgetHelper.GetApproveStep(strAPVStat))
                        {
                            case ApproveStep.Wait:
                                strAPVStat2 = " ";
                                break;
                            case ApproveStep.Approve:
                                strAPVStat2 = "/";
                                strApproveStatText = "PASS";
                                break;
                            case ApproveStep.Reject:
                                strAPVStat2 = "R";
                                break;
                        }

                        dtrBrowView["BAPVSTAT2"] = strAPVStat2;
                        dtrBrowView["CAPVSTAT2"] = strApproveStatText;

                        string strIsOut = dtrBrowView[QBGTranInfo.Field.IsApprove2].ToString().Trim();
                        string strIsOut2 = "";
                        if (strIsOut == SysDef.gc_APPROVE_STEP_APPROVE)
                        {
                            dtrBrowView["bIsOut"] = true;
                            dtrBrowView["cIsOut"] = "Y";
                        }
                        break;
                    case BudgetStep.Post:
                        string strIsPost = dtrBrowView[QBGTranInfo.Field.IsPost].ToString().Trim();
                        bool bllIsPost = false;
                        switch (BudgetHelper.GetApproveStep(strIsPost))
                        {
                            case ApproveStep.Wait:
                                bllIsPost = false;
                                break;
                            case ApproveStep.Post:
                                bllIsPost = true;
                                break;
                        }

                        dtrBrowView["bIsPost"] = bllIsPost;
                        dtrBrowView["cIsPost2"] = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsPost));
                        break;
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

            //this.gridView1.Columns["BAPPROVE"].OptionsColumn.AllowEdit = (this.BeSmartMRPStep == BudgetStep.Approve ? false : true);
            this.gridView1.Columns["BAPPROVE"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["BAPVSTAT2"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["BISOUT"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["BISPOST"].OptionsColumn.AllowEdit = true;

            if (this.BeSmartMRPStep == BudgetStep.Post)
            {
                this.gridView1.Columns["BISPOST"].Visible = true;
                this.gridView1.Columns["CISPOST2"].Visible = true;
            }
            else
            {
                this.gridView1.Columns["BAPPROVE"].Visible = true;
                this.gridView1.Columns["CSTAT"].Visible = true;
            }

            this.gridView1.Columns["QCSECT"].Visible = true;
            this.gridView1.Columns["QNSECT"].Visible = true;
            this.gridView1.Columns["QCJOB"].Visible = true;
            this.gridView1.Columns["QNJOB"].Visible = true;
            this.gridView1.Columns["NAMT"].Visible = true;

            this.gridView1.Columns["BAPVSTAT2"].Visible = (this.BeSmartMRPStep == BudgetStep.Approve);
            this.gridView1.Columns["CAPVSTAT2"].Visible = (this.BeSmartMRPStep == BudgetStep.Approve);
            this.gridView1.Columns["BISOUT"].Visible = (this.BeSmartMRPStep == BudgetStep.Approve);
            this.gridView1.Columns["CISOUT"].Visible = (this.BeSmartMRPStep == BudgetStep.Approve);

            if (this.BeSmartMRPStep == BudgetStep.Post)
            {
                this.gridView1.Columns["BISPOST"].VisibleIndex = 0;
                this.gridView1.Columns["CISPOST2"].VisibleIndex = 1;
            }
            else
            {
                this.gridView1.Columns["BAPPROVE"].VisibleIndex = 0;
                this.gridView1.Columns["CSTAT"].VisibleIndex = 1;
            }

            this.gridView1.Columns["QCSECT"].VisibleIndex = 2;
            this.gridView1.Columns["QNSECT"].VisibleIndex = 3;
            this.gridView1.Columns["QCJOB"].VisibleIndex = 4;
            this.gridView1.Columns["QNJOB"].VisibleIndex = 5;
            this.gridView1.Columns["NAMT"].VisibleIndex = 6;

            if (this.BeSmartMRPStep == BudgetStep.Approve)
            {
                this.gridView1.Columns["BAPVSTAT2"].VisibleIndex = 7;
                this.gridView1.Columns["CAPVSTAT2"].VisibleIndex = 8;
                this.gridView1.Columns["BISOUT"].VisibleIndex = 9;
                this.gridView1.Columns["CISOUT"].VisibleIndex = 10;
            }

            this.gridView1.Columns["BAPPROVE"].Caption = "#อนุมัติ";
            this.gridView1.Columns["CSTAT"].Caption = "สถานะ";

            this.gridView1.Columns["BISPOST"].Caption = "#POST";
            this.gridView1.Columns["CISPOST2"].Caption = "สถานะ";

            this.gridView1.Columns["QCSECT"].Caption = "รหัสแผนก";
            this.gridView1.Columns["QNSECT"].Caption = "ชื่อแผนก";
            this.gridView1.Columns["QCJOB"].Caption = "รหัสโครงการ";
            this.gridView1.Columns["QNJOB"].Caption = "ชื่อโครงการ";
            this.gridView1.Columns["NAMT"].Caption = "งบประมาณรวม";
            this.gridView1.Columns["BAPVSTAT2"].Caption = "#พิจารณา";
            this.gridView1.Columns["CAPVSTAT2"].Caption = "สถานะ";
            this.gridView1.Columns["BISOUT"].Caption = "#Y/N";
            this.gridView1.Columns["CISOUT"].Caption = "สถานะ";

            this.gridView1.Columns["NAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NAMT"].DisplayFormat.FormatString = "#,###,###,###.00";

            //this.gridView1.Columns["BAPPROVE"].ColumnEdit = this.grcApprove;

            this.grcApprove.ReadOnly = !App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove, App.AppUserName, App.AppUserID);
            this.grcIsPost.ReadOnly = !App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove, App.AppUserName, App.AppUserID);
            this.grcAPVStat.ReadOnly = !App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove2, App.AppUserName, App.AppUserID);
            this.grcIsOut.ReadOnly = !App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove2, App.AppUserName, App.AppUserID);

            this.gridView1.Columns["BAPPROVE"].ColumnEdit = this.grcApprove;
            this.gridView1.Columns["BAPVSTAT2"].ColumnEdit = this.grcAPVStat;
            this.gridView1.Columns["BISOUT"].ColumnEdit = this.grcIsOut;
            this.gridView1.Columns["BISPOST"].ColumnEdit = this.grcIsPost;

            this.gridView1.Columns["BAPPROVE"].Width = 50;
            this.gridView1.Columns["CSTAT"].Width = 70;
            this.gridView1.Columns["BISPOST"].Width = 50;
            this.gridView1.Columns["CISPOST2"].Width = 70;
            this.gridView1.Columns["QCSECT"].Width = 70;
            this.gridView1.Columns["QNSECT"].Width = 150;
            this.gridView1.Columns["QCJOB"].Width = 100;
            this.gridView1.Columns["NAMT"].Width = 100;
            this.gridView1.Columns["BAPVSTAT2"].Width = 70;
            this.gridView1.Columns["CAPVSTAT2"].Width = 70;
            this.gridView1.Columns["BISOUT"].Width = 50;
            this.gridView1.Columns["CISOUT"].Width = 70;

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

        private void pmRecalColWidth()
        {

            int intSumColWidth = this.gridView1.Columns["BAPPROVE"].Width
                + this.gridView1.Columns["CSTAT"].Width
                + this.gridView1.Columns["QCSECT"].Width
                + this.gridView1.Columns["QNSECT"].Width
                + this.gridView1.Columns["QCJOB"].Width
                + this.gridView1.Columns["NAMT"].Width;

            if (this.BeSmartMRPStep == BudgetStep.Approve)
            {
                intSumColWidth += this.gridView1.Columns["BAPVSTAT2"].Width
                + this.gridView1.Columns["CAPVSTAT2"].Width
                + this.gridView1.Columns["BISOUT"].Width
                + this.gridView1.Columns["CISOUT"].Width;
            }
            int intColWidth = this.grdBrowView.Width - intSumColWidth - 30;
            this.gridView1.Columns["QNJOB"].Width = (intColWidth < 120 ? 120 : intColWidth);

        }

        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = true;
                switch (this.BeSmartMRPStep)
                {
                    case BudgetStep.Prepare:
                    case BudgetStep.Revise:
                        break;
                    case BudgetStep.Approve:
                    case BudgetStep.Post:
                        this.gridView2.Columns[intCnt].OptionsColumn.ReadOnly = true;
                        break;
                }

                //if (this.BeSmartMRPStep != BudgetStep.Prepare
                //    || this.BeSmartMRPStep != BudgetStep.Revise)
                //{
                //    this.gridView2.Columns[intCnt].OptionsColumn.ReadOnly = true;
                //}
            }

            this.gridView2.Columns["cRowID"].Visible = false;
            this.gridView2.Columns["nRecNo"].Visible = false;
            this.gridView2.Columns["cBGType"].Visible = false;
            this.gridView2.Columns["cBGChartHD"].Visible = false;
            this.gridView2.Columns["cQnBGType"].Visible = false;

            this.gridView2.Columns["cQcBGType"].VisibleIndex = 0;
            this.gridView2.Columns["cQcBGChart"].VisibleIndex = 1;
            this.gridView2.Columns["cQnBGChart"].VisibleIndex = 2;
            this.gridView2.Columns["cRemark"].VisibleIndex = 3;

            this.gridView2.Columns["nAmt10"].VisibleIndex = 4;
            this.gridView2.Columns["nAmt11"].VisibleIndex = 5;
            this.gridView2.Columns["nAmt12"].VisibleIndex = 6;

            this.gridView2.Columns["nAmt1"].VisibleIndex = 7;
            this.gridView2.Columns["nAmt2"].VisibleIndex = 8;
            this.gridView2.Columns["nAmt3"].VisibleIndex = 9;
            this.gridView2.Columns["nAmt4"].VisibleIndex = 10;
            this.gridView2.Columns["nAmt5"].VisibleIndex = 11;
            this.gridView2.Columns["nAmt6"].VisibleIndex = 12;
            this.gridView2.Columns["nAmt7"].VisibleIndex = 13;
            this.gridView2.Columns["nAmt8"].VisibleIndex = 14;
            this.gridView2.Columns["nAmt9"].VisibleIndex = 15;
            this.gridView2.Columns["nTotAmt"].VisibleIndex = 16;

            this.gridView2.Columns["nTotAmt"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nTotAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nTotAmt"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView2.Columns["cQcBGType"].Caption = "หมวด";
            this.gridView2.Columns["cQcBGChart"].Caption = "รหัสค่าใช้จ่าย";
            this.gridView2.Columns["cQnBGChart"].Caption = "ชื่อค่าใช้จ่าย";
            this.gridView2.Columns["cRemark"].Caption = "รายการ";

            this.gridView2.Columns["cQcBGType"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView2.Columns["cQcBGChart"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView2.Columns["cQnBGChart"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView2.Columns["cRemark"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //this.gridView2.Columns["nAmt10"].Caption = "ต.ค.";
            //this.gridView2.Columns["nAmt11"].Caption = "พ.ย.";
            //this.gridView2.Columns["nAmt12"].Caption = "ธ.ค.";
            //this.gridView2.Columns["nAmt9"].Caption = "ม.ค.";
            //this.gridView2.Columns["nAmt8"].Caption = "ก.พ.";
            //this.gridView2.Columns["nAmt7"].Caption = "มี.ค.";
            //this.gridView2.Columns["nAmt6"].Caption = "เม.ษ.";
            //this.gridView2.Columns["nAmt5"].Caption = "พ.ค.";
            //this.gridView2.Columns["nAmt4"].Caption = "มิ.ย.";
            //this.gridView2.Columns["nAmt3"].Caption = "ก.ค.";
            //this.gridView2.Columns["nAmt2"].Caption = "ส.ค.";
            //this.gridView2.Columns["nAmt1"].Caption = "ก.ย.";

            this.gridView2.Columns["nAmt10"].Caption = "ต.ค.";
            this.gridView2.Columns["nAmt11"].Caption = "พ.ย.";
            this.gridView2.Columns["nAmt12"].Caption = "ธ.ค.";

            this.gridView2.Columns["nAmt1"].Caption = "ม.ค.";
            this.gridView2.Columns["nAmt2"].Caption = "ก.พ.";
            this.gridView2.Columns["nAmt3"].Caption = "มี.ค.";
            this.gridView2.Columns["nAmt4"].Caption = "เม.ษ.";
            this.gridView2.Columns["nAmt5"].Caption = "พ.ค.";
            this.gridView2.Columns["nAmt6"].Caption = "มิ.ย.";
            this.gridView2.Columns["nAmt7"].Caption = "ก.ค.";
            this.gridView2.Columns["nAmt8"].Caption = "ส.ค.";
            this.gridView2.Columns["nAmt9"].Caption = "ก.ย.";

            this.gridView2.Columns["nTotAmt"].Caption = "รวม";

            this.gridView2.Columns["cQcBGType"].Width = 50;
            this.gridView2.Columns["cQcBGChart"].Width = 90;
            this.gridView2.Columns["cQnBGChart"].Width = 120;
            this.gridView2.Columns["cRemark"].Width = 120;

            int intDefaColWidth = 60;
            this.gridView2.Columns["nAmt1"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt2"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt3"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt4"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt5"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt6"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt7"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt8"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt9"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt10"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt11"].Width = intDefaColWidth;
            this.gridView2.Columns["nAmt12"].Width = intDefaColWidth;
            this.gridView2.Columns["nTotAmt"].Width = intDefaColWidth+40;

            this.gridView2.Columns["nTotAmt"].AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridView2.Columns["nTotAmt"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;

            this.gridView2.Columns["nAmt1"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt2"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt3"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt4"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt5"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt6"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt7"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt8"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt9"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt10"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt11"].ColumnEdit = this.grcAmt;
            this.gridView2.Columns["nAmt12"].ColumnEdit = this.grcAmt;

            //this.grcQcJob.MaxLength = DialogForms.dlgGetJob.MAXLENGTH_CODE;
            //this.grcQnJob.MaxLength = DialogForms.dlgGetJob.MAXLENGTH_NAME;
            //this.grcQnUM.MaxLength = DialogForms.dlgGetUM.MAXLENGTH_NAME;
            //this.grcQcDept.MaxLength = DialogForms.dlgGetDept.MAXLENGTH_CODE;

            //this.grcRemark.MaxLength = 150;

            //this.gridView2.Columns["cQcJob"].ColumnEdit = this.grcQcJob;
            //this.gridView2.Columns["cQnJob"].ColumnEdit = this.grcQnJob;
            //this.gridView2.Columns["cRemark"].ColumnEdit = this.grcRemark;
            //this.gridView2.Columns["nAllocPcn"].ColumnEdit = this.grcAllocPcn;
            //this.gridView2.Columns["nQty"].ColumnEdit = this.grcQty;
            //this.gridView2.Columns["cQcDept"].ColumnEdit = this.grcQcDept;
            //this.gridView2.Columns["cQnUM"].ColumnEdit = this.grcQnUM;
            //this.gridView2.Columns["cType"].ColumnEdit = this.grcType;

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


            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    break;
                case BudgetStep.Approve:
                case BudgetStep.Post:
                case BudgetStep.Revise:
                    this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.barMainEdit.Items["tbrTXCopy"].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;
            } 

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);

            this.barMainEdit.Items["tbrTXCopy"].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items["tbrType"].Enabled = (inActivePage == 0 ? true : false);
        
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

            this.pmSetFooterText();

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
                case "FILTER":
                    using (Common.dlgFilter03 dlgFilter = new Common.dlgFilter03())
                    {
                        dlgFilter.SetTitle(this.mstrMenuName, TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrBranch = dlgFilter.BranchID;
                            this.mstrSect = dlgFilter.SectID;
                            this.txtYear.Text = dlgFilter.mYear;

                            this.mstrBGYear = dlgFilter.mYear;
                            this.mintBGYear = dlgFilter.nYear;
                            this.txtQcSect.Text = dlgFilter.SectCode;
                            this.txtQnSect.Text = dlgFilter.SectName;

                            //this.Text = this.mstrMenuName + "/ปีงบประมาณ : " + this.mstrBGYear + "/แผนก : " + this.txtQnSect.Text;
                            this.Text = this.mstrMenuName + "\\ปีงบประมาณ : " + this.mstrBGYear;

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();
                        }
                    }
                    break;
                case "BUDTYPE":
                    if (this.pofrmGetBudType == null)
                    {
                        this.pofrmGetBudType = new frmBudType(FormActiveMode.PopUp);
                        this.pofrmGetBudType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBudType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "BUDCHART":
                    if (this.pofrmGetBudChart == null)
                    {
                        //this.pofrmGetBudChart = new DialogForms.dlgGetBudChart();
                        this.pofrmGetBudChart = new frmBudChart(FormActiveMode.PopUp, "D");
                        this.pofrmGetBudChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBudChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "JOB":
                    if (this.pofrmGetJob == null)
                    {
                        this.pofrmGetJob = new frmEMJob(FormActiveMode.PopUp);
                        this.pofrmGetJob.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetJob.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "DEPT":
                    if (this.pofrmGetDept == null)
                    {
                        //this.pofrmGetDept = new DialogForms.dlgGetDept();
                        this.pofrmGetDept = new frmEMDept(FormActiveMode.PopUp);
                        this.pofrmGetDept.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDept.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "SEMPL":
                    if (this.pofrmGetEmpl == null)
                    {
                        //this.pofrmGetEmpl = new DialogForms.dlgGetEmplSM("S");
                        this.pofrmGetEmpl = new frmAppEmpl(FormActiveMode.PopUp);
                        this.pofrmGetEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "UM":
                    if (this.pofrmGetUM == null)
                    {
                        //this.pofrmGetUM = new DialogForms.dlgGetUM();
                        this.pofrmGetUM = new frmEMUOM(FormActiveMode.PopUp);
                        this.pofrmGetUM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetUM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PRINTOPT":
                    DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                    if (this.gridView1.RowCount == 0 || dtrBrow == null)
                        return;

                    string strQcProj = dtrBrow["QcJob"].ToString().TrimEnd();
                    using (Common.dlgRngProj dlg = new Common.dlgRngProj())
                    {
                        dlg.BegQcJob = strQcProj;
                        dlg.EndQcJob = strQcProj;
                        //dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            this.pmPrintData(dlg.IsPrintAllSect, dlg.BegQcJob, dlg.EndQcJob);
                        }
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
                case "TXTQCJOB":
                case "TXTQNJOB":
                    this.pmInitPopUpDialog("JOB");
                    strPrefix = (inTextbox == "TXTQCJOB" ? "CCODE" : "CNAME");
                    this.pofrmGetJob.ValidateField(this.mstrSect, inPara1, strPrefix, true);
                    if (this.pofrmGetJob.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCEMPL":
                case "TXTQNEMPL":
                    this.pmInitPopUpDialog("SEMPL");
                    strPrefix = (inTextbox == "TXTQCEMPL" ? "CCODE" : "CNAME");
                    this.pofrmGetEmpl.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetEmpl.PopUpResult)
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

                case "TXTQCJOB":
                case "TXTQNJOB":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (this.pofrmGetJob != null)
                    {
                        DataRow dtrJob = this.pofrmGetJob.RetrieveValue();
                        if (dtrJob != null && this.txtQcJob.Tag != dtrJob["cRowID"].ToString()
                            && (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldJob != this.txtQcJob.Tag.ToString())
                            && !this.pmIsValidateJob(new object[] { App.ActiveCorp.RowID, this.mintBGYear, dtrJob["cRowID"].ToString() }))
                        {
                            string strPCode = "(" + dtrJob["cCode"].ToString().TrimEnd() + ")" + dtrJob["cName"].ToString().TrimEnd();
                            MessageBox.Show("โครงการ " + strPCode + "\r\nมีการอ้างอิงไปตั้งงบประมาณแล้ว", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.txtQcJob.Tag = "";
                            this.txtQcJob.Text = "";
                            this.txtQnJob.Text = "";
                        }
                        else
                        {
                            if (dtrJob != null)
                            {
                                this.txtQcJob.Tag = dtrJob["cRowID"].ToString();
                                this.txtQcJob.Text = dtrJob["cCode"].ToString().TrimEnd();
                                this.txtQnJob.Text = dtrJob["cName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtQcJob.Tag = "";
                                this.txtQcJob.Text = "";
                                this.txtQnJob.Text = "";
                            }
                        }
                    }
                    else
                    {
                        this.txtQcJob.Tag = "";
                        this.txtQcJob.Text = "";
                        this.txtQnJob.Text = "";
                    }
                    break;

                case "TXTQCEMPL":
                case "TXTQNEMPL":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (this.pofrmGetEmpl != null)
                    {
                        DataRow dtrEmpl = this.pofrmGetEmpl.RetrieveValue();
                        this.txtQcEmpl.Tag = dtrEmpl["cRowID"].ToString();
                        this.txtQcEmpl.Text = dtrEmpl["cCode"].ToString().TrimEnd();
                        this.txtQnEmpl.Text = dtrEmpl["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcEmpl.Tag = "";
                        this.txtQcEmpl.Text = "";
                        this.txtQnEmpl.Text = "";
                    }
                    break;

                case "GRDVIEW2_CQCBGTYPE":
                case "GRDVIEW2_CQNBGTYPE":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetBudType != null)
                    {
                        DataRow dtrJob = this.pofrmGetBudType.RetrieveValue();
                        dtrTemPd["cBGType"] = dtrJob["cRowID"].ToString();
                        dtrTemPd["cQcBGType"] = dtrJob["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnBGType"] = dtrJob["cName"].ToString().TrimEnd();

                        this.gridView2.UpdateCurrentRow();
                    }
                    break;

                case "GRDVIEW2_CQCBGCHART":
                case "GRDVIEW2_CQNBGCHART":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    DataRow dtrBGChart = this.pofrmGetBudChart.RetrieveValue();
                    if (dtrBGChart != null)
                    {
                        dtrTemPd["cBGChartHD"] = dtrBGChart["cRowID"].ToString();
                        dtrTemPd["cQcBGChart"] = dtrBGChart["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnBGChart"] = dtrBGChart["cName"].ToString().TrimEnd();

                        string strErrorMsg = "";
                        WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                        objSQLHelper.SetPara(new object[] { dtrBGChart["cBGType"].ToString() });
                        if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGType", QBGTypeInfo.TableName, "select * from " + QBGTypeInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            DataRow dtrBGType = this.dtsDataEnv.Tables["QBGType"].Rows[0];
                            dtrTemPd["cBGType"] = dtrBGType["cRowID"].ToString();
                            dtrTemPd["cQcBGType"] = dtrBGType["cCode"].ToString().TrimEnd();
                            dtrTemPd["cQnBGType"] = dtrBGType["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            dtrTemPd["cBGType"] = "";
                            dtrTemPd["cQcBGType"] = "";
                            dtrTemPd["cQnBGType"] = "";
                        }
                        this.gridView2.UpdateCurrentRow();
                    }
                    break;


                case "GRDVIEW2_CQCJOB":
                case "GRDVIEW2_CQNJOB":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetJob != null)
                    {
                        DataRow dtrJob2 = this.pofrmGetJob.RetrieveValue();
                        dtrTemPd["cJob"] = dtrJob2["cRowID"].ToString();
                        dtrTemPd["cQcJob"] = dtrJob2["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnJob"] = dtrJob2["cName"].ToString().TrimEnd();

                        this.gridView2.UpdateCurrentRow();
                    }
                    break;

                case "GRDVIEW2_CQCDEPT":
                case "GRDVIEW2_CQNDEPT":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetDept != null)
                    {
                        DataRow dtrDept = this.pofrmGetDept.RetrieveValue();
                        dtrTemPd["cDept"] = dtrDept["cRowID"].ToString();
                        dtrTemPd["cQcDept"] = dtrDept["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnDept"] = dtrDept["cName"].ToString().TrimEnd();

                        this.gridView2.UpdateCurrentRow();
                    }
                    break;

                case "GRDVIEW2_CQNUM":
                    dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetUM != null)
                    {
                        DataRow dtrUM = this.pofrmGetUM.RetrieveValue();
                        dtrTemPd["cUOM"] = dtrUM["cRowID"].ToString();
                        dtrTemPd["cQcUM"] = dtrUM["cCode"].ToString().TrimEnd();
                        dtrTemPd["cQnUM"] = dtrUM["cName"].ToString().TrimEnd();

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
                switch (strMenuName)
                { 
                    case "TXCOPY":
                        //TODO: เพิ่ม Function Copy เอกสาร
                        MessageBox.Show("เพิ่ม Function Copy เอกสาร");
                        //this.pmCopyDoc();
                        break;
                    case "PRINT":
                        this.pmInitPopUpDialog("PRINTOPT");
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
                                //if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
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

        private void pmClr1TemPd()
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
            DataRow dtrCurrRow = this.dtsDataEnv.Tables[this.mstrTemPd].Rows[this.gridView2.FocusedRowHandle];
            DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

            string strRowID = dtrCurrRow["cRowID"].ToString();
            DataSetHelper.CopyDataRow(dtrNewRow, ref dtrCurrRow);
            dtrCurrRow["cRowID"] = strRowID;
            //dtrTemPd["cIsDel"] = "Y";
            this.pmCalTotAmt();
        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow["QcJob"].ToString().TrimEnd() + ") " + dtrBrow["QnJob"].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["QcJob"].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show("ยืนยันการลบข้อมูลการตั้งงบประมาณ : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow["QcJob"].ToString(), ref strErrorMsg))
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
            bool bllCanEdit = false;
            bool bllHasUsed = false;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrChkRow = this.dtsDataEnv.Tables["QHasUsed"].Rows[0];
                bllCanEdit = this.pmCanEdit(dtrChkRow, DocEditType.Delete, false);
                ioErrorMsg = this.mstrCanEditMsg;
            }
            bllHasUsed = !bllCanEdit;
            return bllHasUsed;
        }

        private bool pmDeleteRow(string inRowID, string inCode, ref string ioErrorMsg)
        {
            object[] pAPara = null;
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

                //this.mdbConn2.Open();
                //this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strErrorMsg = "";

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cBGTranHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                //this.mdbTran2.Commit();
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
                    //this.mdbTran2.Rollback();
                }
                App.WriteEventsLog(ex);
            }
            finally
            {
                this.mdbConn.Close();
                //this.mdbConn2.Close();
            }
            return bllResult;
        }

        private void pmSearchData()
        {
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

                switch (this.BeSmartMRPStep)
                {
                    case BudgetStep.Prepare:
                    case BudgetStep.Revise:
                        this.pmUpdateRecord();
                        break;
                    case BudgetStep.Approve:
                        this.pmApproveStep2(ref strErrorMsg);
                        break;
                    case BudgetStep.Post:
                        this.pmApproveStep3(ref strErrorMsg);
                        break;
                }

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

            if (this.mFormEditMode == UIHelper.AppFormState.Edit
                && this.mbllCanEdit == false)
            {
                ioErrorMsg = this.mstrCanEditMsg;
                this.txtQcJob.Focus();
                return false;
            }
            else if (this.txtQcJob.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุโครงการ";
                this.txtQcJob.Focus();
                return false;
            }
            else if (this.txtQcEmpl.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุผู้เสนองบประมาณ";
                this.txtQcEmpl.Focus();
                return false;
            }
            else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldJob != this.txtQcJob.Tag.ToString())
                && !this.pmIsValidateJob(new object[] { App.ActiveCorp.RowID, this.mintBGYear, this.txtQcJob.Tag.ToString() }))
            {
                ioErrorMsg = "โครงการนี้นำมาบันทึกซ้ำ";
                this.txtQcJob.Focus();
                return false;
            }
            else
                bllResult = true;

            return bllResult;
        }

        private bool pmIsValidateJob(object[] inPrefixPara)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SetPara(inPrefixPara)
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and nBGYear = ? and cJob = ?", ref strErrorMsg))
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
                string strRevise = dtrSaveInfo[QBGTranInfo.Field.Revise].ToString();
                intRevise = (strRevise.Trim() == "" ? 0 : Convert.ToInt32(strRevise));

            }

            intRevise++;

            DateTime dttDate = new DateTime(this.mintBGYear, 1, 1);

            dtrSaveInfo[QBGTranInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QBGTranInfo.Field.BranchID] = this.mstrBranch;
            dtrSaveInfo[QBGTranInfo.Field.RefType] = this.mstrRefType;
            dtrSaveInfo[QBGTranInfo.Field.SectID] = this.mstrSect;
            dtrSaveInfo[QBGTranInfo.Field.JobID] = this.txtQcJob.Tag.ToString();
            dtrSaveInfo[QBGTranInfo.Field.AppEmplID] = this.txtQcEmpl.Tag.ToString();
            dtrSaveInfo[QBGTranInfo.Field.BudGetYear] = this.mintBGYear;
            dtrSaveInfo[QBGTranInfo.Field.Date] = dttDate.Date;
            dtrSaveInfo[QBGTranInfo.Field.Revise] = intRevise.ToString("####");
            dtrSaveInfo[QBGTranInfo.Field.Amount] = this.pmTotAmt;

            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                case BudgetStep.Revise:

                    switch (BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()))
                    {
                        case ApproveStep.Wait:
                            dtrSaveInfo[QBGTranInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                            dtrSaveInfo[QBGTranInfo.Field.ApproveBy] = "";
                            dtrSaveInfo[QBGTranInfo.Field.ApproveDate] = Convert.DBNull;
                            break;
                        case ApproveStep.Approve:
                            dtrSaveInfo[QBGTranInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                            dtrSaveInfo[QBGTranInfo.Field.ApproveBy] = App.FMAppUserID;
                            dtrSaveInfo[QBGTranInfo.Field.ApproveDate] = this.txtDApprove.DateTime;

                            if (this.BeSmartMRPStep == BudgetStep.Revise)
                            {

                                dtrSaveInfo[QBGTranInfo.Field.IsCorrect] = "";
                                dtrSaveInfo[QBGTranInfo.Field.CorrectBy] = "";
                                dtrSaveInfo[QBGTranInfo.Field.CorrectDate] = Convert.DBNull;
                                
                                dtrSaveInfo[QBGTranInfo.Field.IsApprove2] = "";
                                dtrSaveInfo[QBGTranInfo.Field.ApproveBy2] = "";
                                dtrSaveInfo[QBGTranInfo.Field.ApproveDate2] = Convert.DBNull;

                                dtrSaveInfo[QBGTranInfo.Field.APVStatus] = "";
                                dtrSaveInfo[QBGTranInfo.Field.APVBy] = "";
                                dtrSaveInfo[QBGTranInfo.Field.APVDate] = Convert.DBNull;
                            }

                            break;
                        case ApproveStep.Reject:
                            dtrSaveInfo[QBGTranInfo.Field.IsApprove] = this.txtIsApprove.Tag.ToString();
                            dtrSaveInfo[QBGTranInfo.Field.ApproveBy] = App.FMAppUserID;
                            dtrSaveInfo[QBGTranInfo.Field.ApproveDate] = this.txtDApprove.DateTime;
                            break;
                    }

                    break;

                case BudgetStep.Approve:
                    dtrSaveInfo[QBGTranInfo.Field.IsCorrect] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QBGTranInfo.Field.CorrectBy] = App.FMAppUserID;
                    dtrSaveInfo[QBGTranInfo.Field.CorrectDate] = this.txtDApprove.DateTime;
                    break;
                case BudgetStep.Post:
                    dtrSaveInfo[QBGTranInfo.Field.IsPost] = this.txtIsApprove.Tag.ToString();
                    dtrSaveInfo[QBGTranInfo.Field.PostBy] = App.FMAppUserID;
                    dtrSaveInfo[QBGTranInfo.Field.PostDate] = this.txtDApprove.DateTime;
                    break;
            }

            dtrSaveInfo["cLastUpdBy"] = App.FMAppUserID.TrimEnd();
            dtrSaveInfo["dLastUpdBy"] = objSQLHelper.GetDBServerDateTime();

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

                this.pmUpdateBudAllocIT();

                this.mdbTran.Commit();

                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, "", "", App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, "", "", App.FMAppUserID, App.AppUserName);
                    //if (this.mstrOldJob == this.txtCode.Text )
                    //{
                    //    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, "", "", App.FMAppUserID, App.AppUserName);
                    //}
                    //else 
                    //{
                    //    KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, "", "", App.FMAppUserID, App.AppUserName, this.mstrOldJob, this.mstrOldName);
                    //}
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

        private void pmUpdateBudAllocIT()
        {
            string strErrorMsg = "";
            object[] pAPara = null;
            int intRecNo = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cBGType"].ToString().TrimEnd() != string.Empty)
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

                    intRecNo++;
                    this.pmReplRecordBudAllocIT(bllIsNewRow, intRecNo, dtrTemPd, ref dtrBudCI);

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

        private bool pmReplRecordBudAllocIT(bool inState, int inRecNo, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cBranch"] = this.mstrBranch;
            dtrBudCI["nBGYear"] = this.mintBGYear;
            dtrBudCI["cRefType"] = this.mstrRefType;
            dtrBudCI["cBGTranHD"] = dtrBudCH["cRowID"].ToString();
            dtrBudCI["cBGType"] = inTemPd["cBGType"].ToString();
            dtrBudCI["cBGChartHD"] = inTemPd["cBGChartHD"].ToString();
            dtrBudCI["cSect"] = dtrBudCH[QBGTranInfo.Field.SectID].ToString();
            dtrBudCI["cJob"] = dtrBudCH[QBGTranInfo.Field.JobID].ToString();
            dtrBudCI["cRemark"] = inTemPd["cRemark"].ToString().TrimEnd();

            dtrBudCI["nAmt1"] = BizRule.ToDecimal(inTemPd["nAmt1"]);
            dtrBudCI["nAmt2"] = BizRule.ToDecimal(inTemPd["nAmt2"]);
            dtrBudCI["nAmt3"] = BizRule.ToDecimal(inTemPd["nAmt3"]);
            dtrBudCI["nAmt4"] = BizRule.ToDecimal(inTemPd["nAmt4"]);
            dtrBudCI["nAmt5"] = BizRule.ToDecimal(inTemPd["nAmt5"]);
            dtrBudCI["nAmt6"] = BizRule.ToDecimal(inTemPd["nAmt6"]);
            dtrBudCI["nAmt7"] = BizRule.ToDecimal(inTemPd["nAmt7"]);
            dtrBudCI["nAmt8"] = BizRule.ToDecimal(inTemPd["nAmt8"]);
            dtrBudCI["nAmt9"] = BizRule.ToDecimal(inTemPd["nAmt9"]);
            dtrBudCI["nAmt10"] = BizRule.ToDecimal(inTemPd["nAmt10"]);
            dtrBudCI["nAmt11"] = BizRule.ToDecimal(inTemPd["nAmt11"]);
            dtrBudCI["nAmt12"] = BizRule.ToDecimal(inTemPd["nAmt12"]);

            dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(inRecNo , 2);

            return true;
        }


        private void pmCreateTem()
        {

            DataTable dtbTemPdVer = new DataTable(this.mstrTemPd);

            dtbTemPdVer.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("cBGType", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcBGType", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnBGType", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cBGChartHD", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcBGChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnBGChart", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cRemark", System.Type.GetType("System.String"));

            dtbTemPdVer.Columns.Add("nAmt10", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt11", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt12", System.Type.GetType("System.Decimal"));

            dtbTemPdVer.Columns.Add("nAmt1", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt2", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt3", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt4", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt5", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt6", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt7", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt8", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt9", System.Type.GetType("System.Decimal"));

            dtbTemPdVer.Columns.Add("nTotAmt", System.Type.GetType("System.Decimal"), "nAmt1+nAmt2+nAmt3+nAmt4+nAmt5+nAmt6+nAmt7+nAmt8+nAmt9+nAmt10+nAmt11+nAmt12");
            //dtbTemPdVer.Columns.Add("cIsDel", System.Type.GetType("System.String"));

            dtbTemPdVer.Columns["cRowID"].DefaultValue = "";
            dtbTemPdVer.Columns["nRecNo"].DefaultValue = 0;
            dtbTemPdVer.Columns["cBGType"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcBGType"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnBGType"].DefaultValue = "";
            dtbTemPdVer.Columns["cBGChartHD"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcBGChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnBGChart"].DefaultValue = "";
            dtbTemPdVer.Columns["cRemark"].DefaultValue = "";
            //dtbTemPdVer.Columns["cIsDel"].DefaultValue = "";

            dtbTemPdVer.Columns["nAmt1"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt2"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt3"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt4"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt5"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt6"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt7"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt8"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt9"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt10"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt11"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt12"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPdVer);

        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();

            if (this.BeSmartMRPStep == BudgetStep.Prepare
                || this.BeSmartMRPStep == BudgetStep.Revise)
            {
                this.txtQcJob.Focus();
            }
            else
            {
                this.txtIsApprove.Focus();
            }

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

            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    this.mstrBudgetStep = SysDef.gc_APPROVE_STEP_WAIT;
                    break;
                case BudgetStep.Approve:
                    this.mstrBudgetStep = SysDef.gc_APPROVE_STEP_APPROVE;
                    break;
                case BudgetStep.Post:
                    this.mstrBudgetStep = SysDef.gc_APPROVE_STEP_POST;
                    break;
                case BudgetStep.Revise:
                    this.mstrBudgetStep = SysDef.gc_APPROVE_STEP_REVISE;
                    break;
            }
                
            this.txtQcJob.Tag = "";
            this.txtQcJob.Text = "";
            this.txtQnJob.Text = "";
            this.txtRevise.Text = "";
            this.txtQcEmpl.Tag = "";
            this.txtQcEmpl.Text = "";
            this.txtQnEmpl.Text = "";

            //
            if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove, App.AppUserName, App.AppUserID))
            {
                this.txtIsApprove.Enabled = true;
            }
            else
            {
                this.txtIsApprove.Enabled = false;
            }

            if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove2, App.AppUserName, App.AppUserID))
            {
                this.grbAPVStat.Enabled = true;
            }
            else
            {
                this.grbAPVStat.Enabled = false;
            }

            this.rdoAPVStat.SelectedIndex = -1;
            this.txtIsApprove.Text = "";
            this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_WAIT;
            this.txtApproveBy.Text = "";
            //this.txtDApprove.Properties.Is
            this.txtStat.Text = "WAIT";
            this.txtLastUpd.Text = "";
            this.txtQnLastUpd.Text = "";
            this.txtTotAmt.Text = "";

            //this.txtIsOut.Enabled = false;

            this.pmClearIsOutStep();

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

                    if (dtrLoadForm[QBGTranInfo.Field.Revise].ToString().Trim() != string.Empty)
                    {
                        this.txtRevise.Text = Convert.ToInt32(dtrLoadForm[QBGTranInfo.Field.Revise].ToString()).ToString();
                    }

                    this.txtQcEmpl.Tag = dtrLoadForm[QBGTranInfo.Field.AppEmplID].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcEmpl.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmpl", "EMPL", "select * from APPEMPL where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmpl"].Rows[0];
                        this.txtQcEmpl.Text = dtrEmpl["cCode"].ToString().TrimEnd();
                        this.txtQnEmpl.Text = dtrEmpl["cName"].ToString().TrimEnd();
                    }

                    this.txtQcJob.Tag = dtrLoadForm[QBGTranInfo.Field.JobID].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQcJob.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from EMJOB where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrJob = this.dtsDataEnv.Tables["QJob"].Rows[0];
                        this.txtQcJob.Text = dtrJob["cCode"].ToString().TrimEnd();
                        this.txtQnJob.Text = dtrJob["cName"].ToString().TrimEnd();
                    }

                    string strIsApprove = "";
                    string strApprove = "";
                    switch (this.BeSmartMRPStep)
                    {
                        case BudgetStep.Prepare:
                            strIsApprove = dtrLoadForm[QBGTranInfo.Field.IsApprove].ToString().Trim();
                            strApprove = "";
                            switch (BudgetHelper.GetApproveStep(strIsApprove))
                            {
                                case ApproveStep.Wait:
                                    strApprove = " ";
                                    this.txtDApprove.EditValue = null;
                                    break;
                                case ApproveStep.Approve:
                                    strApprove = "/";
                                    this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.ApproveDate]);
                                    break;
                                case ApproveStep.Reject:
                                    strApprove = "R";
                                    this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.ApproveDate]);
                                    break;
                            }

                            this.txtApproveBy.Tag = dtrLoadForm[QBGTranInfo.Field.ApproveBy].ToString();
                            this.txtIsApprove.Text = strApprove;
                            this.txtStat.Text = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsApprove));
                            break;
                        case BudgetStep.Approve:
                            strIsApprove = dtrLoadForm[QBGTranInfo.Field.IsCorrect].ToString().Trim();
                            strApprove = "";

                            switch (BudgetHelper.GetApproveStep(strIsApprove))
                            {
                                case ApproveStep.Wait:
                                    strApprove = " ";
                                    this.txtDApprove.EditValue = null;
                                    break;
                                case ApproveStep.Approve:
                                    strApprove = "/";
                                    this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.CorrectDate]);
                                    break;
                                case ApproveStep.Reject:
                                    strApprove = "R";
                                    this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.CorrectDate]);
                                    break;
                            }

                            this.txtApproveBy.Tag = dtrLoadForm[QBGTranInfo.Field.CorrectBy].ToString();
                            this.txtIsApprove.Text = strApprove;
                            this.txtStat.Text = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsApprove));

                            if (this.BeSmartMRPStep == BudgetStep.Approve)
                            {
                                strIsApprove = dtrLoadForm[QBGTranInfo.Field.APVStatus].ToString().Trim();
                                string strIsOut = dtrLoadForm[QBGTranInfo.Field.IsApprove2].ToString().Trim();
                                int intAPV = -1;
                                switch (BudgetHelper.GetApproveStep(strIsApprove))
                                { 
                                    case ApproveStep.Wait:
                                        break;
                                    case ApproveStep.Approve:
                                        intAPV = 1;
                                        break;
                                    case ApproveStep.Reject:
                                        intAPV = 0;
                                        break;
                                }
                                this.rdoAPVStat.SelectedIndex = intAPV;
                                this.txtAPVRemark.Text = dtrLoadForm[QBGTranInfo.Field.APVRemark].ToString().TrimEnd();
                                //this.txtIsOut.Text = (strIsOut != string.Empty ? "Y" : "");
                                //this.txtIsOut.Enabled = (this.rdoAPVStat.SelectedIndex != -1 ? true : false);
                                this.chkIsOut.Checked = (strIsOut != string.Empty ? true : false);
                            }

                            break;

                        case BudgetStep.Post:
                            string strIsPost = dtrLoadForm[QBGTranInfo.Field.IsPost].ToString().Trim();
                            string strPost = "";
                            switch (BudgetHelper.GetApproveStep(strIsPost))
                            {
                                case ApproveStep.Wait:
                                    strPost = " ";
                                    this.txtDApprove.EditValue = null;
                                    break;
                                case ApproveStep.Post:
                                    strPost = "/";
                                    this.txtDApprove.DateTime = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.PostDate]);
                                    break;
                            }

                            this.txtApproveBy.Tag = dtrLoadForm[QBGTranInfo.Field.PostBy].ToString();
                            this.txtIsApprove.Text = strPost;
                            this.txtStat.Text = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strIsPost)); 
                            break;
                        case BudgetStep.Revise:
                            this.txtApproveBy.Tag = "";
                            this.txtApproveBy.Text = "";
                            this.txtIsApprove.Text = "";
                            this.txtStat.Text = "WAIT";
                            break;
                    }

                    //Load ชื่อ Login ที่อนุมัติ
                    pobjSQLUtil.SetPara(new object[] { this.txtApproveBy.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        this.txtApproveBy.Text = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }

                    //Load ชื่อ Login ที่แก้ไขล่าสุด
                    this.txtQnLastUpd.Tag = dtrLoadForm[QBGTranInfo.Field.LastUpdateBy].ToString();
                    pobjSQLUtil.SetPara(new object[] { this.txtQnLastUpd.Tag.ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
                    {
                        DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                        this.txtQnLastUpd.Text = dtrEmpl["cLogin"].ToString().TrimEnd();
                    }
                    if (!Convert.IsDBNull(dtrLoadForm[QBGTranInfo.Field.LastUpdate]))
                    {
                        DateTime dttLastUpd = Convert.ToDateTime(dtrLoadForm[QBGTranInfo.Field.LastUpdate]);
                        this.txtLastUpd.Text = BudgetHelper.GetShortThaiDate(dttLastUpd) + " " + dttLastUpd.ToString("HH:mm:ss");
                    }

                    this.pmLoadBudAllocIT();
                    this.pmCalTotAmt();
                }
                this.pmSetTxtApproveStat();
                this.pmLoadOldVar();
            }
        }

        private void pmLoadOldVar()
        {
            this.mstrOldJob = this.txtQcJob.Tag.ToString();
        }

        private void pmSetTxtApproveStat()
        {

            if (this.rdoAPVStat.SelectedIndex != -1)
            {
                this.txtIsApprove.Enabled = false;
                this.chkIsOut.Enabled = true;
            }
            else
            {
                this.chkIsOut.Enabled = false;
                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove, App.AppUserName, App.AppUserID))
                {
                    this.txtIsApprove.Enabled = true;
                }
                else
                {
                    this.txtIsApprove.Enabled = false;
                }
            }

        }

        private void pmSetGrbApproveStat()
        {

            if (this.txtIsApprove.Text.Trim() == string.Empty)
            {
                this.grbAPVStat.Enabled = false;
            }
            else
            {
                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanApprove2, App.AppUserName, App.AppUserID))
                {
                    this.grbAPVStat.Enabled = true;
                }
                else
                {
                    this.grbAPVStat.Enabled = false;
                }
            }

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
            }
            bool bllResult = true;
            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    ApproveStep oApprove = BudgetHelper.GetApproveStep(inBgTran[QBGTranInfo.Field.IsApprove].ToString());
                    bool bllIsCorrect = (inBgTran[QBGTranInfo.Field.IsCorrect].ToString().Trim() == SysDef.gc_APPROVE_STEP_APPROVE ? true : false);
                    switch (oApprove)
                    {
                        case ApproveStep.Wait:
                            bllResult = true;
                            break;
                        case ApproveStep.Approve:
                            this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากผ่านการอนุมัติขั้นต้นแล้ว";
                            bllResult = false;
                            break;
                        case ApproveStep.Reject:
                            this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากผ่านการอนุมัติขั้นต้น แต่ไม่ผ่านการอนุมัติ\r\nหากต้องการแก้ไขให้ไปที่เมนู ปรับแก้ไขงบประมาณ";
                            bllResult = false;
                            break;
                    }

                    if (inBgTran[QBGTranInfo.Field.IsPost].ToString() == SysDef.gc_APPROVE_STEP_POST)
                    {
                        this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากผ่านการ POST แล้ว";
                        bllResult = false;
                    }
                    else if (bllIsCorrect)
                    {
                        this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากผ่านการอนุมัติจากหน่วยงานงบประมาณแล้ว !";
                        bllResult = false;
                    }

                    if (!bllResult && inShowMsg)
                    {
                        MessageBox.Show(this.mstrCanEditMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                case BudgetStep.Approve:
                    if (inBgTran[QBGTranInfo.Field.IsPost].ToString() == SysDef.gc_APPROVE_STEP_POST)
                    {
                        this.mstrCanEditMsg = "ไม่อนุญาตให้" + strMsg1 + "เนื่องจากผ่านการ POST แล้ว";
                        bllResult = false;
                    }
                    if (!bllResult && inShowMsg)
                    {
                        MessageBox.Show(this.mstrCanEditMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                case BudgetStep.Post:
                    ////
                    ////
                    ////
                    ////
                    break;
            }
            
            return bllResult;
        }

        private void pmLoadBudAllocIT()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBudCI", "BUDCI", "select * from " + this.mstrITable + " where cBGTranHD = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrBudCI in this.dtsDataEnv.Tables["QBudCI"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrBudCI["cRowID"].ToString();
                    dtrNewRow["cBGType"] = dtrBudCI["cBGType"].ToString();
                    dtrNewRow["cBGChartHD"] = dtrBudCI["cBGChartHD"].ToString();
                    dtrNewRow["cRemark"] = dtrBudCI["cRemark"].ToString().TrimEnd();

                    dtrNewRow["nAmt1"] = BizRule.ToDecimal(dtrBudCI["nAmt1"]);
                    dtrNewRow["nAmt2"] = BizRule.ToDecimal(dtrBudCI["nAmt2"]);
                    dtrNewRow["nAmt3"] = BizRule.ToDecimal(dtrBudCI["nAmt3"]);
                    dtrNewRow["nAmt4"] = BizRule.ToDecimal(dtrBudCI["nAmt4"]);
                    dtrNewRow["nAmt5"] = BizRule.ToDecimal(dtrBudCI["nAmt5"]);
                    dtrNewRow["nAmt6"] = BizRule.ToDecimal(dtrBudCI["nAmt6"]);
                    dtrNewRow["nAmt7"] = BizRule.ToDecimal(dtrBudCI["nAmt7"]);
                    dtrNewRow["nAmt8"] = BizRule.ToDecimal(dtrBudCI["nAmt8"]);
                    dtrNewRow["nAmt9"] = BizRule.ToDecimal(dtrBudCI["nAmt9"]);
                    dtrNewRow["nAmt10"] = BizRule.ToDecimal(dtrBudCI["nAmt10"]);
                    dtrNewRow["nAmt11"] = BizRule.ToDecimal(dtrBudCI["nAmt11"]);
                    dtrNewRow["nAmt12"] = BizRule.ToDecimal(dtrBudCI["nAmt12"]);

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cBGType"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBGType", "BGTYPE", "select * from BGType where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcBGType"] = this.dtsDataEnv.Tables["QBGType"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnBGType"] = this.dtsDataEnv.Tables["QBGType"].Rows[0]["cName"].ToString().TrimEnd();
                    }

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
        
        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void frmBudTran_KeyDown(object sender, KeyEventArgs e)
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
            this.pmSetFooterText();
        }

        private void pmSetFooterText()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");

                if (!Convert.IsDBNull(dtrBrow["CLOGIN_APV"])
                    && dtrBrow["CLOGIN_APV"].ToString().Trim() != string.Empty)
                {
                    string strAPV1 = "";
                    string strAPV2 = "";
                    switch (this.BeSmartMRPStep)
                    {
                        case BudgetStep.Prepare:
                        case BudgetStep.Revise:
                            strAPV1 = "\r\nอนุมัติโดย LOGIN : ";
                            break;
                        case BudgetStep.Approve:
                            strAPV1 = "\r\nอนุมัติขั้นต้นโดย LOGIN : ";

                            if (!Convert.IsDBNull(dtrBrow["CLOGIN_APV2"])
                                && dtrBrow["CLOGIN_APV2"].ToString().Trim() != string.Empty)
                            {
                                strAPV2 = "\r\nPass/Reject โดย LOGIN : " + dtrBrow["CLOGIN_APV2"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dAPV"]).ToString("dd/MM/yy hh:mm:ss");
                            }
                            break;
                        case BudgetStep.Post:
                            strAPV1 = "\r\nPASS รายการโดย LOGIN : ";

                            if (!Convert.IsDBNull(dtrBrow["CLOGIN_POST"])
                                && dtrBrow["CLOGIN_POST"].ToString().Trim() != string.Empty)
                            {
                                strAPV2 = "\r\nPOST โดย LOGIN : " + dtrBrow["CLOGIN_POST"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dPost"]).ToString("dd/MM/yy hh:mm:ss");
                            }
                            break;
                    }
                    this.txtFooter.Text += strAPV1 + dtrBrow["CLOGIN_APV"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dApprove"]).ToString("dd/MM/yy hh:mm:ss") + strAPV2;
                }
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
                case "CQCBGTYPE":
                case "CQNBGTYPE":

                    strOrderBy = (inKeyField.ToUpper() == "CQCBGTYPE" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("BUDTYPE");
                    this.pofrmGetBudType.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetBudType.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_" + inKeyField);
                    }
                    break;

                case "CQCBGCHART":
                case "CQNBGCHART":

                    strOrderBy = (inKeyField.ToUpper() == "CQCBGCHART" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("BUDCHART");
                    this.pofrmGetBudChart.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetBudChart.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_" + inKeyField);
                    }
                    break;

                case "CQCJOB":
                case "CQNJOB":

                    strOrderBy = (inKeyField.ToUpper() == "CQCJOB" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("JOB");
                    this.pofrmGetJob.ValidateField(this.mstrSect, "", strOrderBy, true);
                    if (this.pofrmGetJob.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_"+inKeyField);
                    }
                    break;

                case "CQCDEPT":
                case "CQNDEPT":
                    
                    strOrderBy = (inKeyField.ToUpper() == "CQCDEPT" ? "CCODE" : "CNAME");
                    this.pmInitPopUpDialog("DEPT");
                    this.pofrmGetDept.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetDept.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_" + inKeyField);
                    }
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
                case "CQCBGTYPE":
                case "CQNBGTYPE":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cBGType"] = "";
                        dtrTemPd["cQcBGType"] = "";
                        dtrTemPd["cQnBGType"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("BUDTYPE");
                        string strOrderBy = (strCol.ToUpper() == "CQCBGTYPE" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetBudType.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetBudType.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCBGTYPE" ? dtrTemPd["cQcBGType"].ToString().TrimEnd() : dtrTemPd["cQnBGType"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cBGType"] = "";
                            dtrTemPd["cQcBGType"] = "";
                            dtrTemPd["cQnBGType"] = "";
                        }
                    }
                    break;

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
                        string strBGType = dtrTemPd["cBGType"].ToString();
                        e.Valid = !this.pofrmGetBudChart.ValidateField(strBGType , strValue, strOrderBy, false);

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
                        }
                    }
                    break;

                case "CQCJOB":
                case "CQNJOB":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cJob"] = "";
                        dtrTemPd["cQcJob"] = "";
                        dtrTemPd["cQnJob"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("JOB");
                        string strOrderBy = (strCol.ToUpper() == "CQCJOB" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetJob.ValidateField(this.mstrSect, strValue, strOrderBy, false);

                        if (this.pofrmGetJob.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCJOB" ? dtrTemPd["cQcJob"].ToString().TrimEnd() : dtrTemPd["cQnJob"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cJob"] = "";
                            dtrTemPd["cQcJob"] = "";
                            dtrTemPd["cQnJob"] = "";
                        }
                    }
                    break;

                case "CQCDEPT":
                case "CQNDEPT":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cDept"] = "";
                        dtrTemPd["cQcDept"] = "";
                        dtrTemPd["cQnDept"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("DEPT");
                        string strOrderBy = (strCol.ToUpper() == "CQCDEPT" ? "CCODE" : "CNAME");
                        e.Valid = !this.pofrmGetDept.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetDept.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCDEPT" ? dtrTemPd["cQcDept"].ToString().TrimEnd() : dtrTemPd["cQnDept"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cDept"] = "";
                            dtrTemPd["cQcDept"] = "";
                            dtrTemPd["cQnDept"] = "";
                        }
                    }
                    break;

            }

        }

        private void txtQcJob_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCJOB" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcJob.Tag = "";
                this.txtQcJob.Text = "";
                this.txtQnJob.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("JOB");
                e.Cancel = !this.pofrmGetJob.ValidateField(this.mstrSect, txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetJob.PopUpResult)
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
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCEMPL" ? "CCODE" : "CNAME";

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

        private void txtIsApprove_Enter(object sender, EventArgs e)
        {
            this.txtIsApprove.SelectAll();
        }

        private void txtIsApprove_EditValueChanged(object sender, EventArgs e)
        {
            this.pmToggleApprove();
        }

        private void pmToggleApprove()
        {

            if (this.txtIsApprove.Text.Trim() != string.Empty)
            {
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                this.txtDApprove.DateTime = pobjSQLUtil.GetDBServerDateTime();

                this.txtApproveBy.Text = App.AppUserName;
                if (this.BeSmartMRPStep == BudgetStep.Post)
                {
                    this.txtIsApprove.Text = "/";
                    this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_POST;
                }
                else
                {
                    this.txtIsApprove.Text = "/";
                    this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_APPROVE;
                    //if ("rRพฑ".IndexOf(this.txtIsApprove.Text) > -1)
                    //{
                    //    this.txtIsApprove.Text = "R";
                    //    this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_REJECT;
                    //}
                    //else
                    //{
                    //    this.txtIsApprove.Text = "/";
                    //    this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_APPROVE;
                    //}
                }

            }
            else
            {

                this.txtApproveBy.Text = "";
                this.txtIsApprove.Text = " ";
                this.txtIsApprove.Tag = SysDef.gc_APPROVE_STEP_WAIT;
                this.txtDApprove.EditValue = null;
            }

            this.txtStat.Text = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(this.txtIsApprove.Tag.ToString()));
            this.txtIsApprove.SelectAll();
            this.pmSetGrbApproveStat();
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
                decTotAmt += BizRule.ToDecimal(dtrTemPd["nTotAmt"]);
            }
            decTotAmt = Math.Round(decTotAmt, 2, MidpointRounding.AwayFromZero);
            this.pmTotAmt = decTotAmt;
            //decPcn1 = Math.Round(100 * decSumBG1 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);
            this.txtTotAmt.Text = decTotAmt.ToString("#,###,###,###.00");
        }

        private bool pmApproveStep1(DataRow inSource, string inApproveStat , ref string ioErrorMsg)
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
                //bool bllIsApprove = Convert.ToBoolean(inSource["bApprove"]);
                string strApprove = inApproveStat;
                string strApproveBy = (strApprove.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();
                pAPara = new object[] { strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID , this.mSaveDBAgent.GetDBServerDateTime() , inSource["cRowID"].ToString() };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprove = ?, cApproveBy = ? , dApprove =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        private bool pmCorrectStep2(DataRow inSource, string inCorrectStat, ref string ioErrorMsg)
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
                //bool bllIsCorrect = Convert.ToBoolean(inSource["bCorrect"]);
                string strCorrect = inCorrectStat;
                string strCorrectBy = (strCorrect.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttCorrect = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strCorrect, strCorrectBy, (strCorrect.Trim() == "" ? Convert.DBNull : dttCorrect), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), inSource["cRowID"].ToString() };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsCorrect = ?, cCorrectBy = ? , dIsCorrect =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strCorrect, strCorrectBy, (strCorrect.Trim() == "" ? Convert.DBNull : dttCorrect), inSource["cRowID"].ToString() };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsCorrect = ?, cCorrectBy = ? , dIsCorrect =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        private bool pmApproveStep2(ref string ioErrorMsg)
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
                //bool bllIsApprove = Convert.ToBoolean(inSource["bApprove"]);
                string strApprove = "";

                DataRow dtrBGTranHD = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

                //Update เรื่องการ Approve Status
                if (this.rdoAPVStat.SelectedIndex != -1)
                {
                    strApprove = (this.rdoAPVStat.SelectedIndex == 0 ? SysDef.gc_APPROVE_STEP_REJECT : SysDef.gc_APPROVE_STEP_APPROVE);
                }

                string strApproveBy = "";
                if (strApprove.Trim() == "")
                {
                    strApproveBy = "";
                }
                else
                {
                    strApproveBy = (dtrBGTranHD["cAPVBy"].ToString().Trim() == "" ? App.FMAppUserID : dtrBGTranHD["cAPVBy"].ToString());
                }
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();

                //19/01/09 By Yod : Update เรื่องการ Correct
                string strIsCorrect = "";
                if (this.txtIsApprove.Text.Trim() != string.Empty)
                {
                    strIsCorrect = this.txtIsApprove.Tag.ToString();
                }

                string strIsCorrectBy = "";
                if (strIsCorrect.Trim() == "")
                {
                    strIsCorrectBy = "";
                }
                else
                {
                    strIsCorrectBy = (dtrBGTranHD["cCorrectBy"].ToString().Trim() == "" ? App.FMAppUserID : dtrBGTranHD["cCorrectBy"].ToString());
                }

                DateTime dttCorrect = this.mSaveDBAgent.GetDBServerDateTime();
                
                //pAPara = new object[] {
                //    strIsCorrect, strIsCorrectBy, (strIsCorrect.Trim() == "" ? Convert.DBNull : dttCorrect)
                //    , strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), this.txtAPVRemark.Text.Trim()
                //    , App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime()
                //    , this.mstrEditRowID };

                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsCorrect = ?, cCorrectBy = ? , dIsCorrect =  ? , cAPVStat = ?, cAPVBy = ? , dAPV =  ? , cAPVRemark = ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[] {
                    strIsCorrect, strIsCorrectBy, (strIsCorrect.Trim() == "" ? Convert.DBNull : dttCorrect)
                    , strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), this.txtAPVRemark.Text.Trim()
                    , this.mstrEditRowID };

                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsCorrect = ?, cCorrectBy = ? , dIsCorrect =  ? , cAPVStat = ?, cAPVBy = ? , dAPV =  ? , cAPVRemark = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                //มีการอนุมัติ

                string strApprove2 = "";
                string strApproveBy2 = "";

                //if (this.txtIsOut.Text.Trim() != string.Empty)
                if (this.chkIsOut.Checked)
                {
                    strApprove2 = SysDef.gc_APPROVE_STEP_APPROVE;
                }
                else
                {
                    strApprove2 = SysDef.gc_APPROVE_STEP_WAIT;
                }

                //อนุมัติไม่ผ่านหรือ REJECT
                if (this.rdoAPVStat.SelectedIndex != 1)
                {
                    strApprove2 = "";
                }

                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                strApproveBy2 = (strApprove2.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove2 = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strApprove2, strApproveBy2, (strApprove2.Trim() == "" ? Convert.DBNull : dttApprove2), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), this.mstrEditRowID };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprov2 = ?, cApproveB2 = ? , dApprove2 =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[] { strApprove2, strApproveBy2, (strApprove2.Trim() == "" ? Convert.DBNull : dttApprove2), this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprov2 = ?, cApproveB2 = ? , dApprove2 =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        private bool pmApproveStep2B(string inApproveStat, ref string ioErrorMsg)
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
                //bool bllIsApprove = Convert.ToBoolean(inSource["bApprove"]);
                string strApprove = "";
                bool bllIsApprove = false;
                if (inApproveStat.Trim() != "")
                {
                    strApprove = inApproveStat;
                    if (strApprove == SysDef.gc_APPROVE_STEP_APPROVE)
                    {
                        bllIsApprove = true;
                    }
                }

                string strApproveBy = (strApprove.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), this.mstrEditRowID };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cAPVStat = ?, cAPVBy = ? , dAPV =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cAPVStat = ?, cAPVBy = ? , dAPV =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                //อนุมัติไม่ผ่านหรือ REJECT ต้องคืนสถานะ IsOut ออก
                if (!bllIsApprove)
                {
                    this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                    pAPara = new object[] { "", "", Convert.DBNull, this.mstrEditRowID };
                    this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprov2 = ?, cApproveB2 = ? , dApprove2 =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    this.mdbTran.Commit();
                    bllIsCommit = true;
                }

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

        //Save จากหน้า Browse แก้ค่าผ่าน พรบ ?
        private bool pmApproveStep2C(bool inIsOut, ref string ioErrorMsg)
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

                string strIsOut = (inIsOut ? SysDef.gc_APPROVE_STEP_APPROVE : SysDef.gc_APPROVE_STEP_WAIT);
                string strApproveBy = (strIsOut.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();

                //pAPara = new object[] { strIsOut, strApproveBy, (strIsOut.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), this.mstrEditRowID };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprov2 = ?, cApproveB2 = ? , dApprove2 =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strIsOut, strApproveBy, (strIsOut.Trim() == "" ? Convert.DBNull : dttApprove), this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprov2 = ?, cApproveB2 = ? , dApprove2 =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;
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

        /// <summary>
        /// Post รายการสร้าง แล้ว Insert into BGBalance
        /// </summary>
        /// <param name="ioErrorMsg"></param>
        /// <returns></returns>
        private bool pmApproveStep3(ref string ioErrorMsg)
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
                string strIsPost = "";

                if (this.txtIsApprove.Text.Trim() != string.Empty)
                {
                    strIsPost = SysDef.gc_APPROVE_STEP_POST;
                }

                string strIsPostBy = (strIsPost.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strIsPost, strIsPostBy, (strIsPost.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), this.mstrEditRowID };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsPost = ?, cPostBy = ? , dPost =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strIsPost, strIsPostBy, (strIsPost.Trim() == "" ? Convert.DBNull : dttApprove), this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsPost = ?, cPostBy = ? , dPost =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        /// <summary>
        /// Post รายการสร้าง แล้ว Insert into BGBalance
        /// </summary>
        /// <param name="ioErrorMsg"></param>
        /// <returns></returns>
        private bool pmApproveStep3B(bool inIsPost, ref string ioErrorMsg)
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
                string strIsPost = SysDef.gc_APPROVE_STEP_WAIT;
                bool bllIsPost = false;
                if (inIsPost)
                {
                    strIsPost = SysDef.gc_APPROVE_STEP_POST;
                    bllIsPost = true;
                }

                string strIsPostBy = (strIsPost.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strIsPost, strIsPostBy, (strIsPost.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), this.mstrEditRowID };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsPost = ?, cPostBy = ? , dPost =  ? , cLastUpdBy = ? , dLastUpdBy = ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strIsPost, strIsPostBy, (strIsPost.Trim() == "" ? Convert.DBNull : dttApprove), this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsPost = ?, cPostBy = ? , dPost =  ? where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                decimal decSign = (bllIsPost ? 1 : -1);
                this.pmPostBudget(decSign);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        private void pmPostBudget(decimal inUpdSign)
        {
            string strErrorMsg = "";
            if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QSaveMBal", this.mstrITable, "select * from " + this.mstrITable + " where cBGTranHD = ? order by cSeq", new object[1] { this.mstrEditRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {

                foreach (DataRow dtrBudI in this.dtsDataEnv.Tables["QSaveMBal"].Rows)
                {

                    decimal decBGAmt = BizRule.ToDecimal(dtrBudI["nAmt1"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt2"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt3"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt4"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt5"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt6"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt7"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt8"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt9"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt10"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt11"])
                                                    + BizRule.ToDecimal(dtrBudI["nAmt12"]);

                    this.BeSmartMRPAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.BeSmartMRPAgent.UpdateBudget(inUpdSign, App.ActiveCorp.RowID, this.mstrBranch, " ", this.mintBGYear, dtrBudI["cSect"].ToString(), dtrBudI["cJob"].ToString(), dtrBudI["cBGChartHD"].ToString(), decBGAmt, ref strErrorMsg);

                }
            }
            
        }

        private bool pmApproveStepRevise(DataRow inSource, string inApproveStat, ref string ioErrorMsg)
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
                //bool bllIsApprove = Convert.ToBoolean(inSource["bApprove"]);
                string strApprove = inApproveStat;
                string strApproveBy = (strApprove.Trim() == "" ? "" : App.FMAppUserID);
                DateTime dttApprove = this.mSaveDBAgent.GetDBServerDateTime();
                //pAPara = new object[] { strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), App.FMAppUserID, this.mSaveDBAgent.GetDBServerDateTime(), inSource["cRowID"].ToString() };
                //this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprove = ?, cApproveBy = ? , dApprove =  ? , cLastUpdBy = ? , dLastUpdBy = ? , cAPVStat = '' , cIsApprov2 = '' where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                pAPara = new object[] { strApprove, strApproveBy, (strApprove.Trim() == "" ? Convert.DBNull : dttApprove), inSource["cRowID"].ToString() };
                this.mSaveDBAgent.BatchSQLExec("update " + this.mstrRefTable + " set cIsApprove = ?, cApproveBy = ? , dApprove =  ? , CISCORRECT = '' , CCORRECTBY = '' , cAPVStat = '' , cIsApprov2 = '' where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

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

        private void grcApprove_SelectedValueChanged(object sender, EventArgs e)
        {
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

            if (this.BeSmartMRPStep != BudgetStep.Revise)
            {
                bool bllIsCorrect = (dtrTemPd[QBGTranInfo.Field.IsCorrect].ToString().Trim() == SysDef.gc_APPROVE_STEP_APPROVE ? true : false);
                bool bllIsPassReject = (dtrTemPd[QBGTranInfo.Field.APVStatus].ToString().Trim() != string.Empty ? true : false);
                if (dtrTemPd["cIsPost"].ToString() == SysDef.gc_APPROVE_STEP_POST)
                {
                    //dtrTemPd["bApprove"] = dtrTemPd["bApprove"].ToString();
                    //dtrTemPd["bApprove"] = "/";
                    this.gridView1.SetFocusedRowCellValue(this.gridView1.Columns["BAPPROVE"], "/");
                    MessageBox.Show("เอกสาร POST แล้วไม่สามารถแก้ไขได้ !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (bllIsCorrect)
                {
                    //dtrTemPd["bApprove"] = "/";
                    if (this.BeSmartMRPStep != BudgetStep.Approve)
                    {
                        this.gridView1.SetFocusedRowCellValue(this.gridView1.Columns["BAPPROVE"], "/");
                        MessageBox.Show("ไม่อนุญาตให้แก้ไขได้เนื่องจากผ่านการอนุมัติจากหน่วยงานงบประมาณแล้ว !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (this.BeSmartMRPStep == BudgetStep.Approve && bllIsPassReject)
                    {
                        this.gridView1.SetFocusedRowCellValue(this.gridView1.Columns["BAPPROVE"], "/");
                        MessageBox.Show("ไม่อนุญาตให้แก้ไขได้เนื่องจากผ่านการ Pass/Reject แล้ว !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
            }

            if (sender is DevExpress.XtraEditors.ComboBoxEdit)
            {
                DevExpress.XtraEditors.ComboBoxEdit oSender = sender as DevExpress.XtraEditors.ComboBoxEdit;
                string strApprove = "";
                string strApproveText = "";
                string strAPVStat = "";
                switch (oSender.SelectedIndex)
                {
                    case 0:
                        strApprove = " ";
                        strAPVStat = SysDef.gc_APPROVE_STEP_WAIT;
                        break;
                    case 1:
                        strApprove = "/";
                        strAPVStat = SysDef.gc_APPROVE_STEP_APPROVE;
                        //strAPVStat = SysDef.gc_APPROVE_STEP_PASS;
                        break;
                    case 2:
                        strApprove = "R";
                        strAPVStat = SysDef.gc_APPROVE_STEP_REJECT;
                        break;
                }

                strApproveText = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strAPVStat));
                dtrTemPd["bApprove"] = strApprove;

                string strErrorMsg = "";
                switch (this.BeSmartMRPStep)
                {
                    case BudgetStep.Prepare:
                        this.pmApproveStep1(dtrTemPd, strAPVStat, ref strErrorMsg);
                        dtrTemPd["cStat"] = strApproveText;
                        break;
                    case BudgetStep.Revise:
                        this.pmApproveStepRevise(dtrTemPd, strAPVStat, ref strErrorMsg);
                        dtrTemPd["cStat"] = strApproveText;
                        break;
                    case BudgetStep.Approve:
                        this.pmCorrectStep2(dtrTemPd, strAPVStat, ref strErrorMsg);
                        dtrTemPd["cStat"] = strApproveText;
                        break;
                    case BudgetStep.Post:
                        //this.pmApproveStep3();
                        break;
                }
                //this.pmRefreshBrowView();
            }

        }

        private void grcAPVStat_SelectedValueChanged(object sender, EventArgs e)
        {

            string strErrorMsg = "";
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            this.mstrEditRowID = dtrTemPd["cRowID"].ToString();

            if (dtrTemPd["cIsPost"].ToString() == SysDef.gc_APPROVE_STEP_POST)
            {
                //dtrTemPd["BAPVSTAT2"] = dtrTemPd["bApprove"].ToString();
                this.gridView1.SetFocusedRowCellValue(this.gridView1.Columns["BAPVSTAT2"], dtrTemPd["bApprove"].ToString());
                MessageBox.Show("เอกสาร POST แล้วไม่สามารถแก้ไขได้ !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtrTemPd["bApprove"].ToString() == SysDef.gc_APPROVE_STEP_WAIT)
            {
                //dtrTemPd["BAPVSTAT2"] = dtrTemPd["bApprove"].ToString();
                this.gridView1.SetFocusedRowCellValue(this.gridView1.Columns["BAPVSTAT2"], dtrTemPd["bApprove"].ToString());
                MessageBox.Show("ไม่สามารถ Pass/Reject เอกสารได้\r\nเนื่องจากยังไม่ผ่านการอนุมัติขั้นต้น !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is DevExpress.XtraEditors.ComboBoxEdit)
            {
                DevExpress.XtraEditors.ComboBoxEdit oSender = sender as DevExpress.XtraEditors.ComboBoxEdit;
                string strApprove = "";
                string strApproveText = "";
                string strAPVStat = "";
                switch (oSender.SelectedIndex)
                {
                    case 0:
                        strApprove = " ";
                        strAPVStat = SysDef.gc_APPROVE_STEP_WAIT;
                        dtrTemPd["BISOUT"] = false;
                        dtrTemPd["CISOUT"] = "";
                        break;
                    case 1:
                        strApprove = "/";
                        strAPVStat = SysDef.gc_APPROVE_STEP_APPROVE;
                        break;
                    case 2:
                        strApprove = "R";
                        strAPVStat = SysDef.gc_APPROVE_STEP_REJECT;

                        dtrTemPd["BISOUT"] = false;
                        dtrTemPd["CISOUT"] = "";
                        
                        break;
                }

                strApproveText = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(strAPVStat));
                switch (oSender.SelectedIndex)
                {
                    case 1:
                        strApproveText = "PASS";
                        break;
                }
                dtrTemPd["BAPVSTAT2"] = strApprove;
                dtrTemPd["CAPVSTAT2"] = strApproveText;

                this.pmApproveStep2B(strAPVStat, ref strErrorMsg);
                //this.pmRefreshBrowView();
            }
            this.mstrEditRowID = "";

        }

        private void grcIsOut_CheckedChanged(object sender, EventArgs e)
        {
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            DevExpress.XtraEditors.CheckEdit oSender = sender as DevExpress.XtraEditors.CheckEdit;

            if (dtrTemPd["BAPVSTAT2"].ToString() != "/")
            {
                oSender.Checked = false;
                return;
            }

            this.mstrEditRowID = dtrTemPd["cRowID"].ToString();
            bool bllIsApprove = false;
            string strErrorMsg = "";
            bllIsApprove = oSender.Checked;
            //dtrTemPd["bIsOut"] = bllIsApprove;
            dtrTemPd["CISOUT"] = (bllIsApprove ? "Y" : "N");
            this.pmApproveStep2C(bllIsApprove, ref strErrorMsg);

            this.mstrEditRowID = "";
        }

        private void grcIsPost_CheckedChanged(object sender, EventArgs e)
        {
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            DevExpress.XtraEditors.CheckEdit oSender = sender as DevExpress.XtraEditors.CheckEdit;

            this.mstrEditRowID = dtrTemPd["cRowID"].ToString();
            bool bllIsPost = false;
            string strErrorMsg = "";
            bllIsPost = oSender.Checked;
            dtrTemPd["CISPOST2"] = (bllIsPost ? "POST" : "WAIT");
            this.pmApproveStep3B(bllIsPost, ref strErrorMsg);

            this.mstrEditRowID = "";
        }

        private void tbcFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is DevExpress.XtraEditors.ComboBoxEdit)
            {
                DevExpress.XtraEditors.ComboBoxEdit oSender = sender as DevExpress.XtraEditors.ComboBoxEdit;
                string strApprove = "";
                switch (oSender.SelectedIndex)
                {
                    case 0:
                        strApprove = "X";
                        break;
                    case 1:
                        strApprove = " ";
                        break;
                    case 2:
                        strApprove = "/";
                        break;
                    case 3:
                        strApprove = "R";
                        break;
                }

                switch (this.BeSmartMRPStep)
                {
                    case BudgetStep.Prepare:
                        this.mstrCurFilterStat = strApprove;
                        break;
                    case BudgetStep.Approve:
                        this.mstrCurFilterStat = strApprove;
                        break;
                    case BudgetStep.Post:
                        break;
                }
                this.gridView1.RefreshData();
            }
        }

        private void gridView1_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            //DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;

            if (dvBrow.Count > 0)
            {
                DataRow dtrTemPd = dvBrow[e.ListSourceRow].Row;

                if (dtrTemPd == null) return;

                if (this.mstrCurFilterStat == "X")
                {
                    e.Handled = false;
                }
                else
                {
                    switch (this.BeSmartMRPStep)
                    {
                        case BudgetStep.Prepare:
                            e.Visible = (dtrTemPd["bApprove"].ToString() == this.mstrCurFilterStat ? true : false);
                            break;
                        case BudgetStep.Approve:
                            e.Visible = (dtrTemPd["bAPVStat2"].ToString() == this.mstrCurFilterStat ? true : false);
                            break;
                        case BudgetStep.Post:
                            break;
                    }
                    e.Handled = true;
                }
            }

        }

        private void gridView2_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            //DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            if (dvBrow.Count > 0)
            {
                DataRow dtrTemPd = dvBrow[e.ListSourceRow].Row;

                if (dtrTemPd == null) return;

                //e.Visible = (dtrTemPd["cIsDel"].ToString() == "Y" ? false : true);
                e.Handled = true;
            }

        }

        private void rdoAPVStat_DoubleClick(object sender, EventArgs e)
        {
            this.pmSetTxtApproveStat();
            this.pmClearIsOutStep();
        }

        private void rdoAPVStat_Properties_EditValueChanged(object sender, EventArgs e)
        {
            this.pmSetTxtApproveStat();
        }

        private void pmClearIsOutStep()
        {
            this.rdoAPVStat.SelectedIndex = -1;
            this.chkIsOut.Checked = false;
        }

        private void txtIsOut_Enter(object sender, EventArgs e)
        {
            this.txtIsApprove.SelectAll();
        }

        private void txtIsApprove_Click(object sender, EventArgs e)
        {
            if (this.txtIsApprove.Text.Trim() != string.Empty)
            {
                this.txtIsApprove.Text = "";
            }
            else
            {
                this.txtIsApprove.Text = "/";
            }
            //this.pmToggleApprove();
        }


        private void pmPrintData(bool inPrintAllSect , string inBegCode, string inEndCode)
        {

            string strErrorMsg = "";
            string strSQLText = "";
            string strJoinTable = "";
            string strJoinFld = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSectList = BudgetHelper.GetAuthSectList();

            string strFld = "BGTRANHD.CROWID, BGTRANHD.CREVISE , BGTRANHD.NBGYEAR , BGTRANHD.NAMT ";
            strFld += " , BGTRANHD.CISAPPROVE, BGTRANHD.CAPVSTAT, BGTRANHD.CISAPPROV2 , BGTRANHD.CISPOST ";
            strFld += " , BGTRANHD.CISAPPROVE as CISAPPROVE_S, BGTRANHD.CAPVSTAT as CAPVSTAT_S, BGTRANHD.CISAPPROV2 as CISAPPROV2_S ";
            strFld += " , EMSECT.CCODE as QCSECT, EMSECT.CNAME as QNSECT ";
            strFld += " , EMJOB.CCODE as QCJOB, EMJOB.CNAME as QNJOB ";
            strFld += " , EMPROJ.CCODE as QCPROJ, EMPROJ.CNAME as QNPROJ ";
            strFld += " , APPEMPL.CCODE as QCEMPL, APPEMPL.CNAME as QNEMPL ";
            strFld += " , BGTRANHD.DCREATE, BGTRANHD.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD, EM3.CLOGIN as CLOGIN_APV ";
            strFld += " , BGCHARTHD.CCODE as QCBGCHART, BGCHARTHD.CNAME as QNBGCHART ";
            strFld += " , BGTYPE.CCODE as QCBGTYPE, BGTYPE.CNAME as QNBGTYPE ";
            strFld += " , BGTRANIT.CREMARK";
            strFld += " , BGTRANIT.NAMT1 as NAMT01";
            strFld += " , BGTRANIT.NAMT2 as NAMT02";
            strFld += " , BGTRANIT.NAMT3 as NAMT03";
            strFld += " , BGTRANIT.NAMT4 as NAMT04";
            strFld += " , BGTRANIT.NAMT5 as NAMT05";
            strFld += " , BGTRANIT.NAMT6 as NAMT06";
            strFld += " , BGTRANIT.NAMT7 as NAMT07";
            strFld += " , BGTRANIT.NAMT8 as NAMT08";
            strFld += " , BGTRANIT.NAMT9 as NAMT09";
            strFld += " , BGTRANIT.NAMT10 as NAMT10";
            strFld += " , BGTRANIT.NAMT11 as NAMT11";
            strFld += " , BGTRANIT.NAMT12 as NAMT12";

            string strSQLExec = "select " + strFld + " from {0} BGTRANHD ";
            strSQLExec += " left join BGTRANIT ON BGTRANHD.CROWID = BGTRANIT.CBGTRANHD ";
            strSQLExec += " left join BGCHARTHD ON BGCHARTHD.CROWID = BGTRANIT.CBGCHARTHD ";
            strSQLExec += " left join BGTYPE ON BGTYPE.CROWID = BGTRANIT.CBGTYPE ";
            strSQLExec += " left join EMSECT ON EMSECT.CROWID = BGTRANHD.CSECT";
            strSQLExec += " left join EMJOB ON EMJOB.CROWID = BGTRANHD.CJOB ";
            strSQLExec += " left join EMPROJ ON EMPROJ.CROWID = EMJOB.CPROJ ";
            strSQLExec += " left join APPEMPL ON APPEMPL.CROWID = BGTRANHD.CEMPL ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = BGTRANHD.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = BGTRANHD.CLASTUPDBY ";
            strSQLExec += " left join {1} EM3 ON EM3.CROWID = BGTRANHD.CAPPROVEBY ";
            strSQLExec += " where BGTRANHD.CCORP = ? and BGTRANHD.CBRANCH = ? " + (inPrintAllSect ? " and EMSECT.CCODE in " + strSectList : " and BGTRANHD.CSECT = ? ");
            strSQLExec += " and BGTRANHD.NBGYEAR = ? and EMJOB.CCODE between ? and ? ";

            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    break;
                case BudgetStep.Approve:
                    strSQLExec += " and BGTRANHD.CISAPPROVE = '" + SysDef.gc_APPROVE_STEP_APPROVE + "'";
                    break;
                case BudgetStep.Post:
                    strSQLExec += " and BGTRANHD.CISAPPROV2 = '" + SysDef.gc_APPROVE_STEP_APPROVE + "'";
                    break;
                case BudgetStep.Revise:
                    strSQLExec += " and (BGTRANHD.CISAPPROVE = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CISCORRECT = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CAPVSTAT = '" + SysDef.gc_APPROVE_STEP_REJECT + "'";
                    strSQLExec += " or BGTRANHD.CISAPPROV2 = '" + SysDef.gc_APPROVE_STEP_REJECT + "' )";
                    break;
            }

            Report.LocalDataSet.DTSLIST01 dtsPreviewReport = new Report.LocalDataSet.DTSLIST01();

            strSQLExec = string.Format(strSQLExec, new string[] { "BGTRANHD", "APPLOGIN" });

            if (inPrintAllSect)
            {
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mintBGYear, inBegCode, inEndCode });
            }
            else
            {
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrSect, this.mintBGYear, inBegCode, inEndCode });
            }
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QList", "BGTRANHD", strSQLExec, ref strErrorMsg);
            foreach (DataRow dtrList in this.dtsDataEnv.Tables["QList"].Rows)
            {
                DataRow dtrPreview = dtsPreviewReport.XRLIST02.NewRow();

                dtrPreview["cBGYear"] = (Convert.ToInt32(dtrList["nBGYear"])+543).ToString("0000");
                //dtrPreview["cSeq"] = dtrList[""].ToString();
                if (dtrList[QBGTranInfo.Field.Revise].ToString().Trim() != string.Empty)
                {
                    dtrPreview["cRevise"] = Convert.ToInt32(dtrList[QBGTranInfo.Field.Revise].ToString()).ToString();
                }
                
                dtrPreview["cQcSect"] = dtrList["QcSect"].ToString();
                dtrPreview["cQnSect"] = dtrList["QnSect"].ToString();
                //dtrPreview["cQcPrProj"] = dtrList["QcProj"].ToString();
                dtrPreview["cQcJob"] = dtrList["QcJob"].ToString();
                dtrPreview["cQnJob"] = dtrList["QnJob"].ToString();
                dtrPreview["cQcProj"] = dtrList["QcProj"].ToString();
                dtrPreview["cQnProj"] = dtrList["QnProj"].ToString();
                dtrPreview["cQcEmpl"] = dtrList["QcEmpl"].ToString();
                dtrPreview["cQnEmpl"] = dtrList["QnEmpl"].ToString();
                //dtrPreview["cIsApprove"] = dtrList[""].ToString();
                //dtrPreview["cApproveBy"] = dtrList[""].ToString();
                //dtrPreview["dApprove"] = dtrList[""].ToString();
                //dtrPreview["cStatus"] = dtrList[""].ToString();
                //dtrPreview["cLastUpd"] = dtrList[""].ToString();
                //dtrPreview["cLastUpdBy"] = dtrList[""].ToString();
                dtrPreview["cQcBGType"] = dtrList["QcBGType"].ToString();
                dtrPreview["cQnBGType"] = dtrList["QnBGType"].ToString();
                dtrPreview["cQcBGChart"] = dtrList["QcBGChart"].ToString();
                dtrPreview["cQnBGChart"] = dtrList["QnBGChart"].ToString();
                dtrPreview["cDesc"] = dtrList["cRemark"].ToString();
                dtrPreview["nAmt01"] = BizRule.ToDecimal(dtrList["NAMT01"]);
                dtrPreview["nAmt02"] = BizRule.ToDecimal(dtrList["NAMT02"]);
                dtrPreview["nAmt03"] = BizRule.ToDecimal(dtrList["NAMT03"]);
                dtrPreview["nAmt04"] = BizRule.ToDecimal(dtrList["NAMT04"]);
                dtrPreview["nAmt05"] = BizRule.ToDecimal(dtrList["NAMT05"]);
                dtrPreview["nAmt06"] = BizRule.ToDecimal(dtrList["NAMT06"]);
                dtrPreview["nAmt07"] = BizRule.ToDecimal(dtrList["NAMT07"]);
                dtrPreview["nAmt08"] = BizRule.ToDecimal(dtrList["NAMT08"]);
                dtrPreview["nAmt09"] = BizRule.ToDecimal(dtrList["NAMT09"]);
                dtrPreview["nAmt10"] = BizRule.ToDecimal(dtrList["NAMT10"]);
                dtrPreview["nAmt11"] = BizRule.ToDecimal(dtrList["NAMT11"]);
                dtrPreview["nAmt12"] = BizRule.ToDecimal(dtrList["NAMT12"]);

                dtsPreviewReport.XRLIST02.Rows.Add(dtrPreview);

            }

            if (dtsPreviewReport.XRLIST02.Rows.Count != 0)
            {
                this.pmPreviewReport(dtsPreviewReport);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = "";

            strRPTFileName = Application.StartupPath + @"\RPT\XRLSTBGTRAN01.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.Text);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์รหัส" + this.mstrTableText + "ตั้งแต่ : " + this.txtBegQcProj1.Text + " ถึง " + this.txtEndQcProj1.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, TASKNAME);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            ////prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

        private void grcApprove_CheckedChanged(object sender, EventArgs e)
        {
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            bool bllIsApprove = false;
            string strErrorMsg = "";
            switch (this.BeSmartMRPStep)
            {
                case BudgetStep.Prepare:
                    //this.pmApproveStep1(dtrTemPd, ref strErrorMsg);

                    bllIsApprove = Convert.ToBoolean(dtrTemPd["bApprove"]);
                    dtrTemPd["cStat"] = (bllIsApprove ? "WAIT" : "APPROVE");
                    break;
                case BudgetStep.Approve:
                    //this.pmApproveStep1();
                    break;
                case BudgetStep.Post:
                    //this.pmApproveStep3();
                    break;
            }
            this.pmRefreshBrowView();
            //this.gridView1.UpdateCurrentRow();
        }


    }
}
