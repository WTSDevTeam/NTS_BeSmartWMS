
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Report
{

        /// <summary>
    /// ใช้ในรายงาน output 1 หัวข้อ
    /// PBGOUT1_06	    6. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามหมวดค่าใช้จ่าย 
    /// ================
    /// รวมทั้งสิ้น
    /// 1 รายงาน
    /// ================
    /// </summary>

    public partial class frmOption06 : UIHelper.frmBase
    {

        public frmOption06()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;
        }

        public string mYear
        {
            get { return this.txtYear.Text.ToString(); }
        }

        public int nYear
        {
            get { return this.mintYear; }
        }

        public string BegQcProj1
        {
            get { return this.txtBegQcProj1.Text.TrimEnd(); }
        }

        public string EndQcProj1
        {
            get { return this.txtEndQcProj1.Text.TrimEnd(); }
        }

        public string BegQcProj2
        {
            get { return this.txtBegQcProj2.Text.TrimEnd(); }
        }

        public string EndQcProj2
        {
            get { return this.txtEndQcProj2.Text.TrimEnd(); }
        }

        private DatabaseForms.frmBGYear pofrmGetBGYear = null;
        private DatabaseForms.frmEMProj pofrmGetProj = null;

        private string mstrTaskName = "";
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();

        System.Data.OleDb.OleDbConnection conn = null;
        System.Data.OleDb.OleDbConnection conn2 = null;

        private string mstrTemBg = "TemBg";

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intYear = DateTime.Now.Year;
            
            this.pmCreateTem();

            string strErrorMsg = "";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGYear", "BGYEAR", "select * from BGYEAR where CCORP = ? order by NYEAR desc", ref strErrorMsg))
            {
                DataRow dtrBGYear = this.dtsDataEnv.Tables["QBGYear"].Rows[0];
                this.txtYear.Text = dtrBGYear["cCode"].ToString().TrimEnd();
                this.mintYear = Convert.ToInt32(dtrBGYear["nYear"]);
            }

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemBg];

        }

        private void pmCreateTem()
        {

            DataTable dtbTemPdVer = new DataTable(this.mstrTemBg);

            dtbTemPdVer.Columns.Add("nLevel", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("nRun", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cGroup", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cGroup2", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cRunNo", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cType2", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcPrProj", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcProj", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnProj", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nAmt01", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn01", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt02", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn02", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt03", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn03", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt04", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn04", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt05", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn05", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nTotal", System.Type.GetType("System.Decimal"), "nAmt01+nAmt02+nAmt03+nAmt04+nAmt05");
            dtbTemPdVer.Columns.Add("nPcnTotal", System.Type.GetType("System.Decimal"));

            dtbTemPdVer.Columns["nLevel"].DefaultValue = 0;
            dtbTemPdVer.Columns["nRun"].DefaultValue = 0;
            dtbTemPdVer.Columns["cSeq"].DefaultValue = "";
            dtbTemPdVer.Columns["cGroup"].DefaultValue = "";
            dtbTemPdVer.Columns["cGroup2"].DefaultValue = "";
            dtbTemPdVer.Columns["cRunNo"].DefaultValue = "";
            dtbTemPdVer.Columns["cType"].DefaultValue = "";
            dtbTemPdVer.Columns["cType2"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcPrProj"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcProj"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnProj"].DefaultValue = "";
            dtbTemPdVer.Columns["nAmt01"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt02"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt03"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt04"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt05"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcn01"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcn02"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcn03"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcn04"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcn05"].DefaultValue = 0;
            dtbTemPdVer.Columns["nPcnTotal"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPdVer);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BGYEAR":
                    if (this.pofrmGetBGYear == null)
                    {
                        this.pofrmGetBGYear = new frmBGYear(FormActiveMode.Report);
                        this.pofrmGetBGYear.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGYear.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PROJ":
                    if (this.pofrmGetProj == null)
                    {
                        this.pofrmGetProj = new frmEMProj(FormActiveMode.Report);
                        this.pofrmGetProj.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProj.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            string strOrderBy = "";

            switch (inTextbox)
            {
                case "TXTYEAR":
                    this.pmInitPopUpDialog("BGYEAR");
                    strOrderBy = "CCODE";
                    this.pofrmGetBGYear.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetBGYear.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROJ1":
                case "TXTENDQCPROJ1":
                    this.pmInitPopUpDialog("PROJ");
                    this.pofrmGetProj.ValidateField("LEVEL1", inPara1, "CCODE", true);
                    if (this.pofrmGetProj.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROJ2":
                case "TXTENDQCPROJ2":
                    this.pmInitPopUpDialog("PROJ");
                    this.pofrmGetProj.ValidateField("LEVEL2", inPara1, "CCODE", true);
                    if (this.pofrmGetProj.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            DataRow dtrRetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTYEAR":
                    dtrRetVal = this.pofrmGetBGYear.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtYear.Tag = dtrRetVal["cRowID"].ToString();
                        this.txtYear.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                        this.mintYear = Convert.ToInt32(dtrRetVal["nYear"]);
                    }
                    else
                    {
                        this.txtYear.Tag = "";
                        this.txtYear.Text = "";
                        this.mintYear = 0;
                    }
                    break;
                case "TXTBEGQCPROJ1":
                    dtrRetVal = this.pofrmGetProj.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtBegQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                        this.txtBegQnProj1.Text = dtrRetVal["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtBegQcProj1.Text = "";
                        this.txtBegQnProj1.Text = "";
                    }
                    break;
                case "TXTENDQCPROJ1":
                    dtrRetVal = this.pofrmGetProj.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtEndQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                        this.txtEndQnProj1.Text = dtrRetVal["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtEndQcProj1.Text = "";
                        this.txtEndQnProj1.Text = "";
                    }
                    break;
                case "TXTBEGQCPROJ2":
                    dtrRetVal = this.pofrmGetProj.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtBegQcProj2.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                        this.txtBegQnProj2.Text = dtrRetVal["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtBegQcProj2.Text = "";
                        this.txtBegQnProj2.Text = "";
                    }
                    break;
                case "TXTENDQCPROJ2":
                    dtrRetVal = this.pofrmGetProj.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtEndQcProj2.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                        this.txtEndQnProj2.Text = dtrRetVal["cName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtEndQcProj2.Text = "";
                        this.txtEndQnProj2.Text = "";
                    }
                    break;
            
            }
        }

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                this.txtYear.Tag = "";
                this.txtYear.Text = "";
                this.mintYear = 0;
            }
            else
            {
                this.pmInitPopUpDialog("BGYEAR");
                e.Cancel = !this.pofrmGetBGYear.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBGYear.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }
        }


        private void txtQcProj1_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name == "TXTBEGQCPROJ1")
                {
                    this.txtBegQnProj1.Text = "";
                }
                else
                {
                    this.txtEndQnProj1.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PROJ");
                e.Cancel = !this.pofrmGetProj.ValidateField("LEVEL1", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetProj.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcProj2_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name == "TXTBEGQCPROJ2")
                {
                    this.txtBegQnProj2.Text = "";
                }
                else
                {
                    this.txtEndQnProj2.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PROJ");
                e.Cancel = !this.pofrmGetProj.ValidateField("LEVEL2", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetProj.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            
            this.pmPrintData();
            this.pmSumHirachy();
            this.pmSumHirachy2();
            this.pmAllocPercent();

            Report.LocalDataSet.DTSBUDGET01 dtsPreviewReport = new Report.LocalDataSet.DTSBUDGET01();

            decimal decSumBG1 = 0;
            decimal decSumBG2 = 0;
            decimal decSumBG3 = 0;
            decimal decSumBG4 = 0;
            decimal decSumBG5 = 0;

            decimal decPcn1 = 0;
            decimal decPcn2 = 0;
            decimal decPcn3 = 0;
            decimal decPcn4 = 0;
            decimal decPcn5 = 0;


            this.pmSumBGTotal(
                ref decSumBG1
                ,ref decSumBG2
                , ref decSumBG3
                , ref decSumBG4
                , ref decSumBG5
                , ref decPcn1
                , ref decPcn2
                , ref decPcn3
                , ref decPcn4
                , ref decPcn5);

            DataRow dtrPreview = null;

            string strPYear = (this.mintYear + 543).ToString("0000");
            dtrPreview = dtsPreviewReport.XRBUDGET1.NewRow();
            dtrPreview["nLevel"] = 0;
            dtrPreview["nRun"] = 0;
            dtrPreview["cGroup"] = "";
            dtrPreview["cGroup2"] = "";
            dtrPreview["cSeq"] = "";
            dtrPreview["cRunNo"] = "";
            dtrPreview["cType"] = "H";
            dtrPreview["cQcPrProj"] = "";
            dtrPreview["cQcProj"] = "";
            dtrPreview["cQnProj"] = "รวมงบประมาณรายจ่ายประจำปี " + strPYear;
            dtrPreview["nAmt01"] = decSumBG1;
            dtrPreview["nAmt02"] = decSumBG2;
            dtrPreview["nAmt03"] = decSumBG3;
            dtrPreview["nAmt04"] = decSumBG4;
            dtrPreview["nAmt05"] = decSumBG5;
            dtrPreview["nPcn01"] = decPcn1;
            dtrPreview["nPcn02"] = decPcn2;
            dtrPreview["nPcn03"] = decPcn3;
            dtrPreview["nPcn04"] = decPcn4;
            dtrPreview["nPcn05"] = decPcn5;

            dtsPreviewReport.XRBUDGET1.Rows.Add(dtrPreview);

            foreach (DataRow dtrTemBg in this.dtsDataEnv.Tables[this.mstrTemBg].Rows)
            {
                dtrPreview = dtsPreviewReport.XRBUDGET1.NewRow();
                dtrPreview["nLevel"] = Convert.ToInt32(dtrTemBg["nLevel"]);
                dtrPreview["nRun"] = Convert.ToInt32(dtrTemBg["nRun"]);
                dtrPreview["cGroup"] = dtrTemBg["cGroup"].ToString();
                dtrPreview["cGroup2"] = dtrTemBg["cGroup2"].ToString();
                dtrPreview["cSeq"] = dtrTemBg["cSeq"].ToString();
                dtrPreview["cRunNo"] = dtrTemBg["cRunNo"].ToString();
                dtrPreview["cType"] = dtrTemBg["cType"].ToString();
                dtrPreview["cType2"] = dtrTemBg["cType2"].ToString();
                dtrPreview["cQcPrProj"] = dtrTemBg["cQcPrProj"].ToString();
                dtrPreview["cQcProj"] = dtrTemBg["cQcProj"].ToString();
                dtrPreview["cQnProj"] = dtrTemBg["cQnProj"].ToString();
                dtrPreview["nAmt01"] = Convert.ToDecimal(dtrTemBg["nAmt01"]);
                dtrPreview["nAmt02"] = Convert.ToDecimal(dtrTemBg["nAmt02"]);
                dtrPreview["nAmt03"] = Convert.ToDecimal(dtrTemBg["nAmt03"]);
                dtrPreview["nAmt04"] = Convert.ToDecimal(dtrTemBg["nAmt04"]);
                dtrPreview["nAmt05"] = Convert.ToDecimal(dtrTemBg["nAmt05"]);
                dtrPreview["nPcn01"] = Convert.ToDecimal(dtrTemBg["nPcn01"]);
                dtrPreview["nPcn02"] = Convert.ToDecimal(dtrTemBg["nPcn02"]);
                dtrPreview["nPcn03"] = Convert.ToDecimal(dtrTemBg["nPcn03"]);
                dtrPreview["nPcn04"] = Convert.ToDecimal(dtrTemBg["nPcn04"]);
                dtrPreview["nPcn05"] = Convert.ToDecimal(dtrTemBg["nPcn05"]);

                dtsPreviewReport.XRBUDGET1.Rows.Add(dtrPreview);

            }
            if (dtsPreviewReport.XRBUDGET1.Rows.Count != 0)
            {
                this.dataGridView1.DataSource = dtsPreviewReport.XRBUDGET1;
                this.pmPreviewReport(dtsPreviewReport);
            }
        }

        private void pmPrintData()
        {

            this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Clear();

            string strSQLProj1 = "";
            strSQLProj1 = "select * from EMPROJ where CCORP = ? and CTYPE = 'G' and CCODE between ? and ? and CPRPROJ = '' ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtBegQcProj1.Text.TrimEnd(), this.txtEndQcProj1.Text.TrimEnd() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProj1", "EMPROJ", strSQLProj1, ref strErrorMsg))
            {
                int intNRun = 0;
                foreach (DataRow dtrProj in this.dtsDataEnv.Tables["QProj1"].Rows)
                {
                    DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();
                    intNRun += 1;

                    dtrPLine["nLevel"] = 0;
                    dtrPLine["nRun"] = intNRun;
                    dtrPLine["cType"] = "G";
                    dtrPLine["cType2"] = "X";
                    dtrPLine["cGroup"] = dtrProj["cCode"].ToString().TrimEnd();
                    dtrPLine["cQcProj"] = dtrProj["cCode"].ToString().TrimEnd();
                    dtrPLine["cQnProj"] = dtrProj["cName"].ToString().TrimEnd();
                    this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);

                    this.pmCalChild(dtrProj["cCode"].ToString().TrimEnd(), 0, intNRun.ToString(), dtrProj["cRowID"].ToString(), dtrProj["cCode"].ToString().TrimEnd());
                }
            }
        }

        private void pmCalChild(string inGroup, int inLevel, string inRunNo, string inPRProj, string inQcProj)
        {
            string strSQLProj1 = "";
            strSQLProj1 = "select * from EMPROJ where CPRPROJ = ? and CCODE >= ? and CCODE <= ?";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            DataSet ds = new DataSet();

            // + ":" เพื่อให้ได้ Detail Proj ที่ครบ
            string strEndCode = this.txtEndQcProj2.Text.TrimEnd() + ":";
            objSQLHelper.SetPara(new object[] { inPRProj, this.txtBegQcProj2.Text.TrimEnd(), strEndCode });
            if (objSQLHelper.SQLExec(ref ds, "QProjChild", "EMPROJ", strSQLProj1, ref strErrorMsg))
            {
                int intNLevel = inLevel + 1;
                int intNRun = 0;
                string strRunNo = "";
                foreach (DataRow dtrProj in ds.Tables["QProjChild"].Rows)
                {
                    DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();

                    intNRun += 1;
                    dtrPLine["nLevel"] = intNLevel;
                    dtrPLine["nRun"] = intNRun;
                    dtrPLine["cType"] = "G";
                    dtrPLine["cType2"] = "X";
                    dtrPLine["cGroup"] = inGroup;
                    dtrPLine["cGroup2"] = inQcProj;
                    //dtrPLine["cSeq"] = inRunNo + "." + intNRun.ToString();
                    if (intNLevel > 1)
                    {
                        strRunNo = inRunNo + "." + intNRun.ToString();
                        dtrPLine["cRunNo"] = strRunNo;
                    }
                    dtrPLine["cQcPrProj"] = inQcProj;
                    dtrPLine["cQcProj"] = dtrProj["cCode"].ToString().TrimEnd();
                    dtrPLine["cQnProj"] = dtrProj["cName"].ToString().TrimEnd();
                    this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);

                    this.pmPJob(inGroup, intNLevel, strRunNo, dtrProj["cRowID"].ToString(), dtrProj["cCode"].ToString().TrimEnd());
                    this.pmCalChild(inGroup, intNLevel, inRunNo, dtrProj["cRowID"].ToString(), dtrProj["cCode"].ToString().TrimEnd());
                }
            }
        }

        private void pmPJob(string inGroup, int inLevel, string inRunNo, string inProj, string inQcProj)
        {
            string strSQLProj1 = "";
            strSQLProj1 = "select * from EMJOB where CPROJ = ? ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { inProj });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "EMPROJ", strSQLProj1, ref strErrorMsg))
            {
                int intNLevel = inLevel + 1;
                int intNRun = 0;
                string strRunNo = "";
                foreach (DataRow dtrJob in this.dtsDataEnv.Tables["QJob"].Rows)
                {

                    DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();
                    
                    intNRun += 1;
                    dtrPLine["nLevel"] = inLevel + 1;
                    dtrPLine["nRun"] = intNRun;
                    dtrPLine["cGroup"] = inGroup;
                    dtrPLine["cGroup2"] = dtrJob["cCode"].ToString().TrimEnd();
                    //dtrPLine["cSeq"] = inRunNo + "." + intNRun.ToString();
                    //dtrPLine["cRunNo"] = inRunNo + "." + intNRun.ToString();
                    if (intNLevel > 1)
                    {
                        strRunNo = inRunNo + "." + intNRun.ToString();
                        dtrPLine["cRunNo"] = strRunNo;
                    }
                    dtrPLine["cType"] = "G";
                    dtrPLine["cType2"] = "D";
                    dtrPLine["cQcPrProj"] = inQcProj;
                    dtrPLine["cQcProj"] = dtrJob["cCode"].ToString().TrimEnd();
                    dtrPLine["cQnProj"] = dtrJob["cName"].ToString().TrimEnd();
                    this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);

                    this.pmGrpBGChart(inGroup, intNLevel, strRunNo, dtrJob["cRowID"].ToString(), dtrJob["cCode"].ToString().TrimEnd(), inQcProj);

                }
            }
        }

        private void pmGrpBGChart(string inGroup, int inLevel, string inRunNo, string inJob, string inQcJob, string inPRQcJob)
        {
            decimal decAmt = 0;
            string strSQLText = "";
            //strSQLText = "select sum(BGTranHD.nAmt) as SumAmt from BGTranHD where nBGYear = ? and cJob = ? and cIsPost = 'T' ";

            strSQLText = " select ";
            strSQLText += " PRBGCHARTHD.CROWID ";
            strSQLText += " , (select CS.CCODE from BGCHARTHD CS where CS.CROWID = PRBGCHARTHD.CROWID ) as QcBGChart";
            strSQLText += " , (select CS.CNAME from BGCHARTHD CS where CS.CROWID = PRBGCHARTHD.CROWID ) as QnBGChart";
            strSQLText += " ,sum(BGTRANIT.NAMT1 + BGTRANIT.NAMT2 + BGTRANIT.NAMT3 + BGTRANIT.NAMT4 + BGTRANIT.NAMT5 + BGTRANIT.NAMT6";
            strSQLText += " +BGTRANIT.NAMT7 + BGTRANIT.NAMT8 + BGTRANIT.NAMT9 + BGTRANIT.NAMT10 + BGTRANIT.NAMT11 + BGTRANIT.NAMT12) as SumAmt";
            strSQLText += " from BGTRANIT ";
            strSQLText += " left join BGTRANHD on BGTRANHD.CROWID = BGTRANIT.CBGTRANHD";
            strSQLText += " left join BGCHARTHD on BGCHARTHD.CROWID = BGTRANIT.CBGCHARTHD";
            strSQLText += " left join BGCHARTHD PRBGCHARTHD on PRBGCHARTHD.CROWID = BGCHARTHD.CPRBGCHART";
            strSQLText += " left join EMJOB on EMJOB.CROWID = BGTRANIT.CJOB";
            strSQLText += " where ";
            strSQLText += " BGTRANHD.CCORP = ?";
            strSQLText += " and BGTRANHD.CJOB = ?";
            strSQLText += " and BGTRANHD.NBGYEAR = ?";
            strSQLText += " and BGTRANHD.CISPOST = ?";
            strSQLText += " group by PRBGCHARTHD.CROWID";

            string strCateg = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, inJob, this.mintYear, SysDef.gc_APPROVE_STEP_POST });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGTranHd", "BGTRANHD", strSQLText, ref strErrorMsg))
            {
                int intNLevel = inLevel + 1;
                int intNRun = 0;
                string strRunNo = "";
                foreach (DataRow dtrPBGChart in this.dtsDataEnv.Tables["QBGTranHd"].Rows)
                {
                    decAmt = (Convert.IsDBNull(dtrPBGChart["SumAmt"]) ? 0 : Convert.ToDecimal(dtrPBGChart["SumAmt"]));

                    DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();

                    intNRun += 1;
                    dtrPLine["nLevel"] = inLevel + 1;
                    dtrPLine["nRun"] = intNRun;
                    dtrPLine["cGroup"] = inGroup;
                    dtrPLine["cGroup2"] = inQcJob;
                    //dtrPLine["cSeq"] = inRunNo + "." + intNRun.ToString();
                    //dtrPLine["cRunNo"] = inRunNo + "." + intNRun.ToString();
                    if (intNLevel > 1)
                    {
                        strRunNo = inRunNo + "." + intNRun.ToString();
                        dtrPLine["cRunNo"] = strRunNo;
                    }
                    dtrPLine["cType"] = "G";
                    dtrPLine["cType2"] = "X";
                    dtrPLine["cQcPrProj"] = inQcJob;
                    dtrPLine["cQcProj"] = dtrPBGChart["QcBGChart"].ToString().TrimEnd();
                    dtrPLine["cQnProj"] = "   " + dtrPBGChart["QnBGChart"].ToString().TrimEnd();
                    //dtrPLine["nAmt01"] = decAmt;
                    //dtrPLine["nAmt02"] = decAmt2;
                    //dtrPLine["nAmt03"] = decAmt3;
                    //dtrPLine["nAmt04"] = decAmt4;
                    //dtrPLine["nAmt05"] = decAmt5;
                    this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);

                    this.pmPBGChartChild(inGroup, intNLevel, strRunNo, inJob, inQcJob, dtrPBGChart["cRowID"].ToString(), dtrPBGChart["QcBGChart"].ToString().TrimEnd());
                
                }//end For Each

            } // End If
        }

        private void pmPBGChartChild(string inGroup, int inLevel, string inRunNo, string inJob, string inQcJob, string inPRChart, string inQcPRChart)
        {

            string strSQLText = " select ";
            strSQLText += " BGCHARTHD.CROWID ";
            strSQLText += " , (select CS.CCODE from BGCHARTHD CS where CS.CROWID = BGCHARTHD.CROWID ) as QcBGChart";
            strSQLText += " , (select CS.CNAME from BGCHARTHD CS where CS.CROWID = BGCHARTHD.CROWID ) as QnBGChart";
            strSQLText += " ,sum(BGTRANIT.NAMT1 + BGTRANIT.NAMT2 + BGTRANIT.NAMT3 + BGTRANIT.NAMT4 + BGTRANIT.NAMT5 + BGTRANIT.NAMT6";
            strSQLText += " +BGTRANIT.NAMT7 + BGTRANIT.NAMT8 + BGTRANIT.NAMT9 + BGTRANIT.NAMT10 + BGTRANIT.NAMT11 + BGTRANIT.NAMT12) as SumAmt";
            strSQLText += " from BGTRANIT ";
            strSQLText += " left join BGTRANHD on BGTRANHD.CROWID = BGTRANIT.CBGTRANHD";
            strSQLText += " left join BGCHARTHD on BGCHARTHD.CROWID = BGTRANIT.CBGCHARTHD";
            strSQLText += " left join BGCHARTHD PRBGCHARTHD on PRBGCHARTHD.CROWID = BGCHARTHD.CPRBGCHART";
            strSQLText += " where ";
            strSQLText += " BGTRANHD.CCORP = ?";
            strSQLText += " and BGTRANHD.CJOB = ?";
            strSQLText += " and BGTRANHD.NBGYEAR = ?";
            strSQLText += " and BGTRANHD.CISPOST = ?";
            strSQLText += " and PRBGCHARTHD.CROWID = ?";
            strSQLText += " group by BGCHARTHD.CROWID";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, inJob, this.mintYear, SysDef.gc_APPROVE_STEP_POST, inPRChart });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGTranHd", "BGTRANHD", strSQLText, ref strErrorMsg))
            {
                int intNRun = 0;
                decimal decAmt = 0;
                foreach (DataRow dtrPBGChart in this.dtsDataEnv.Tables["QBGTranHd"].Rows)
                {
                    decAmt = (Convert.IsDBNull(dtrPBGChart["SumAmt"]) ? 0 : Convert.ToDecimal(dtrPBGChart["SumAmt"]));

                    DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();

                    intNRun += 1;
                    dtrPLine["nLevel"] = inLevel + 1;
                    dtrPLine["nRun"] = intNRun;
                    dtrPLine["cGroup"] = inGroup;
                    dtrPLine["cGroup2"] = inQcJob;
                    //dtrPLine["cSeq"] = inRunNo + "." + intNRun.ToString();
                    dtrPLine["cRunNo"] = inRunNo + "." + intNRun.ToString();
                    dtrPLine["cType"] = "D";
                    dtrPLine["cType2"] = "X";
                    dtrPLine["cQcPrProj"] = inQcPRChart;
                    dtrPLine["cQcProj"] = dtrPBGChart["QcBGChart"].ToString().TrimEnd();
                    dtrPLine["cQnProj"] = "     - " + dtrPBGChart["QnBGChart"].ToString().TrimEnd();
                    dtrPLine["nAmt01"] = decAmt;
                    //dtrPLine["nAmt02"] = decAmt2;
                    //dtrPLine["nAmt03"] = decAmt3;
                    //dtrPLine["nAmt04"] = decAmt4;
                    //dtrPLine["nAmt05"] = decAmt5;
                    this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);


                }//end For Each

            } // End If
        }

        private void pmSumHirachy()
        {
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cType = 'D' ", "cGroup2,cQcProj");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                if (dtrProj["cQcPrProj"].ToString().Trim() != string.Empty)
                {
                    decimal decSumBG1 = Convert.ToDecimal(dtrProj["nAmt01"]);
                    decimal decSumBG2 = Convert.ToDecimal(dtrProj["nAmt02"]);
                    decimal decSumBG3 = Convert.ToDecimal(dtrProj["nAmt03"]);
                    decimal decSumBG4 = Convert.ToDecimal(dtrProj["nAmt04"]);
                    decimal decSumBG5 = Convert.ToDecimal(dtrProj["nAmt05"]);
                    this.pmSumPrProj(dtrProj["cGroup2"].ToString(), dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
                }
            }
        }

        private void pmSumPrProj(string inGroup2, string inQcProj, decimal inAmt1, decimal inAmt2, decimal inAmt3, decimal inAmt4, decimal inAmt5)
        {
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cGroup2 = '" + inGroup2 + "' and cQcProj = '" + inQcProj + "' ");
            if (dtASum1.Length > 0)
            {
                DataRow dtrProj = dtASum1[0];
                dtrProj["nAmt01"] = Convert.ToDecimal(dtrProj["nAmt01"]) + inAmt1;
                dtrProj["nAmt02"] = Convert.ToDecimal(dtrProj["nAmt02"]) + inAmt2;
                dtrProj["nAmt03"] = Convert.ToDecimal(dtrProj["nAmt03"]) + inAmt3;
                dtrProj["nAmt04"] = Convert.ToDecimal(dtrProj["nAmt04"]) + inAmt4;
                dtrProj["nAmt05"] = Convert.ToDecimal(dtrProj["nAmt05"]) + inAmt5;
                if (dtrProj["cQcPrProj"].ToString().Trim() != string.Empty)
                {
                    decimal decSumBG1 = inAmt1;
                    decimal decSumBG2 = inAmt2;
                    decimal decSumBG3 = inAmt3;
                    decimal decSumBG4 = inAmt4;
                    decimal decSumBG5 = inAmt5;
                    this.pmSumPrProj(dtrProj["cGroup2"].ToString(), dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
                }

            }
        }

        private void pmSumHirachy2()
        {
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cType2 = 'D' ", "cQcProj");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                if (dtrProj["cQcPrProj"].ToString().Trim() != string.Empty)
                {
                    decimal decSumBG1 = Convert.ToDecimal(dtrProj["nAmt01"]);
                    decimal decSumBG2 = Convert.ToDecimal(dtrProj["nAmt02"]);
                    decimal decSumBG3 = Convert.ToDecimal(dtrProj["nAmt03"]);
                    decimal decSumBG4 = Convert.ToDecimal(dtrProj["nAmt04"]);
                    decimal decSumBG5 = Convert.ToDecimal(dtrProj["nAmt05"]);
                    this.pmSumPrProj2(dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
                }
            }
        }

        private void pmSumPrProj2(string inQcProj, decimal inAmt1, decimal inAmt2, decimal inAmt3, decimal inAmt4, decimal inAmt5)
        {
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cQcProj = '" + inQcProj + "' ");
            if (dtASum1.Length > 0)
            {
                DataRow dtrProj = dtASum1[0];
                dtrProj["nAmt01"] = Convert.ToDecimal(dtrProj["nAmt01"]) + inAmt1;
                dtrProj["nAmt02"] = Convert.ToDecimal(dtrProj["nAmt02"]) + inAmt2;
                dtrProj["nAmt03"] = Convert.ToDecimal(dtrProj["nAmt03"]) + inAmt3;
                dtrProj["nAmt04"] = Convert.ToDecimal(dtrProj["nAmt04"]) + inAmt4;
                dtrProj["nAmt05"] = Convert.ToDecimal(dtrProj["nAmt05"]) + inAmt5;
                if (dtrProj["cQcPrProj"].ToString().Trim() != string.Empty)
                {
                    decimal decSumBG1 = inAmt1;
                    decimal decSumBG2 = inAmt2;
                    decimal decSumBG3 = inAmt3;
                    decimal decSumBG4 = inAmt4;
                    decimal decSumBG5 = inAmt5;
                    this.pmSumPrProj2(dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
                }

            }
        }

        private void pmAllocPercent()
        {
            this.pmAllocLv1();
        }

        private void pmAllocLv1()
        {
            decimal decSumTotalBg = 0;
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cQcPrProj = '' ");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                decSumTotalBg += Convert.ToDecimal(dtrProj["nTotal"]);
            }

            if (decSumTotalBg != 0)
            {
                decimal decSumPcn1 = 0;
                decimal decSumPcn2 = 0;
                decimal decSumPcn3 = 0;
                decimal decSumPcn4 = 0;
                decimal decSumPcn5 = 0;

                decimal decAmt1 = 0;
                decimal decAmt2 = 0;
                decimal decAmt3 = 0;
                decimal decAmt4 = 0;
                decimal decAmt5 = 0;
                
                for (int i = 0; i < dtASum1.Length; i++)
                {
                    DataRow dtrProj = dtASum1[i];

                    decAmt1 = Convert.ToDecimal(dtrProj["nAmt01"]);
                    decAmt2 = Convert.ToDecimal(dtrProj["nAmt02"]);
                    decAmt3 = Convert.ToDecimal(dtrProj["nAmt03"]);
                    decAmt4 = Convert.ToDecimal(dtrProj["nAmt04"]);
                    decAmt5 = Convert.ToDecimal(dtrProj["nAmt05"]);

                    decSumPcn1 = Math.Round(100 * decAmt1 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);
                    decSumPcn2 = Math.Round(100 * decAmt2 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);
                    decSumPcn3 = Math.Round(100 * decAmt3 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);
                    decSumPcn4 = Math.Round(100 * decAmt4 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);
                    decSumPcn5 = Math.Round(100 * decAmt5 / decSumTotalBg, 2, MidpointRounding.AwayFromZero);

                    dtrProj["nPcn01"] = decSumPcn1;
                    dtrProj["nPcn02"] = decSumPcn2;
                    dtrProj["nPcn03"] = decSumPcn3;
                    dtrProj["nPcn04"] = decSumPcn4;
                    dtrProj["nPcn05"] = decSumPcn5;

                    this.pmAllocLv2(dtrProj["cQcProj"].ToString()
                                            , decAmt1, decAmt2, decAmt3, decAmt4, decAmt5
                                            , decSumPcn1, decSumPcn2, decSumPcn3, decSumPcn4, decSumPcn5);

                }
            }

        }

        private void pmAllocLv2(string inQcProj
            , decimal inPrAmt1
            , decimal inPrAmt2
            , decimal inPrAmt3
            , decimal inPrAmt4
            , decimal inPrAmt5
            , decimal inAllocPcn1
            , decimal inAllocPcn2
            , decimal inAllocPcn3
            , decimal inAllocPcn4
            , decimal inAllocPcn5)
        {
            decimal decSumTotalBg = 0;
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cQcPrProj = '" + inQcProj + "' ");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                decSumTotalBg += Convert.ToDecimal(dtrProj["nTotal"]);
            }

            if (decSumTotalBg != 0)
            {
                
                decimal decSumPcn1 = 0;
                decimal decSumPcn2 = 0;
                decimal decSumPcn3 = 0;
                decimal decSumPcn4 = 0;
                decimal decSumPcn5 = 0;

                decimal decAmt1 = 0;
                decimal decAmt2 = 0;
                decimal decAmt3 = 0;
                decimal decAmt4 = 0;
                decimal decAmt5 = 0;

                for (int i = 0; i < dtASum1.Length; i++)
                {

                    DataRow dtrProj = dtASum1[i];

                    decAmt1 = Convert.ToDecimal(dtrProj["nAmt01"]);
                    decAmt2 = Convert.ToDecimal(dtrProj["nAmt02"]);
                    decAmt3 = Convert.ToDecimal(dtrProj["nAmt03"]);
                    decAmt4 = Convert.ToDecimal(dtrProj["nAmt04"]);
                    decAmt5 = Convert.ToDecimal(dtrProj["nAmt05"]);

                    decSumPcn1 = (inPrAmt1 != 0 ? Math.Round(inAllocPcn1 / inPrAmt1 * decAmt1, 2, MidpointRounding.AwayFromZero) : 0);
                    decSumPcn2 = (inPrAmt2 != 0 ? Math.Round(inAllocPcn2 / inPrAmt2 * decAmt2, 2, MidpointRounding.AwayFromZero) : 0);
                    decSumPcn3 = (inPrAmt3 != 0 ? Math.Round(inAllocPcn3 / inPrAmt3 * decAmt3, 2, MidpointRounding.AwayFromZero) : 0);
                    decSumPcn4 = (inPrAmt4 != 0 ? Math.Round(inAllocPcn4 / inPrAmt4 * decAmt4, 2, MidpointRounding.AwayFromZero) : 0);
                    decSumPcn5 = (inPrAmt5 != 0 ? Math.Round(inAllocPcn5 / inPrAmt5 * decAmt5, 2, MidpointRounding.AwayFromZero) : 0);

                    dtrProj["nPcn01"] = decSumPcn1;
                    dtrProj["nPcn02"] = decSumPcn2;
                    dtrProj["nPcn03"] = decSumPcn3;
                    dtrProj["nPcn04"] = decSumPcn4;
                    dtrProj["nPcn05"] = decSumPcn5;

                    this.pmAllocLv2(dtrProj["cQcProj"].ToString()
                                            , decAmt1, decAmt2, decAmt3, decAmt4, decAmt5
                                            , decSumPcn1, decSumPcn2, decSumPcn3, decSumPcn4, decSumPcn5);

                }
            }

        }

        private void pmSumBGTotal(
            ref decimal inPrAmt1
            , ref decimal inPrAmt2
            , ref decimal inPrAmt3
            , ref decimal inPrAmt4
            , ref decimal inPrAmt5
            , ref decimal inAllocPcn1
            , ref decimal inAllocPcn2
            , ref decimal inAllocPcn3
            , ref decimal inAllocPcn4
            , ref decimal inAllocPcn5)
        {

            decimal decSumBG1 = 0;
            decimal decSumBG2 = 0;
            decimal decSumBG3 = 0;
            decimal decSumBG4 = 0;
            decimal decSumBG5 = 0;

            decimal decPcn1 = 0;
            decimal decPcn2 = 0;
            decimal decPcn3 = 0;
            decimal decPcn4 = 0;
            decimal decPcn5 = 0;

            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cQcPrProj = '' ");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                decSumBG1 += Convert.ToDecimal(dtrProj["nAmt01"]);
                decSumBG2 += Convert.ToDecimal(dtrProj["nAmt02"]);
                decSumBG3 += Convert.ToDecimal(dtrProj["nAmt03"]);
                decSumBG4 += Convert.ToDecimal(dtrProj["nAmt04"]);
                decSumBG5 += Convert.ToDecimal(dtrProj["nAmt05"]);

                decPcn1 += Convert.ToDecimal(dtrProj["nPcn01"]);
                decPcn2 += Convert.ToDecimal(dtrProj["nPcn02"]);
                decPcn3 += Convert.ToDecimal(dtrProj["nPcn03"]);
                decPcn4 += Convert.ToDecimal(dtrProj["nPcn04"]);
                decPcn5 += Convert.ToDecimal(dtrProj["nPcn05"]);

            }

            inPrAmt1 = decSumBG1;
            inPrAmt2 = decSumBG2;
            inPrAmt3 = decSumBG3;
            inPrAmt4 = decSumBG4;
            inPrAmt5 = decSumBG5;

            inAllocPcn1 = decPcn1;
            inAllocPcn2 = decPcn2;
            inAllocPcn3 = decPcn3;
            inAllocPcn4 = decPcn4;
            inAllocPcn5 = decPcn5;

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\XRSUMBG06.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //string strPYear = (this.mintYear + 543).ToString("0000");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text + " ประจำปี " + strPYear);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPYear);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFYEAR"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[4]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }


    }

}
