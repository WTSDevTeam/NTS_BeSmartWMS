
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
    /// ไม่ปันส่วน
    /// PBGOUT1_07     7. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามงบประมาณทั้งหมด (รายเดือน)
    /// PBGOUT1_08     8. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามงบลงทุน (รายเดือน)
    /// PBGOUT1_09     9. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามงบประมาณทั้งหมด (รายไตรมาส)
    /// ปันส่วน
    /// PBGOUT1_19	    19. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามงบประมาณทั้งหมด (รายเดือน)
    /// PBGOUT1_20	    20. รายงานแผนงบประมาณรายจ่ายประจำปี-จำแนกตามงบประมาณทั้งหมด (รายไตรมาส)
    /// ================
    /// รวมทั้งสิ้น
    /// 5 รายงาน
    /// ================
    /// </summary>

    public partial class frmOption07_9 : UIHelper.frmBase
    {

        public frmOption07_9(string inForm, bool inIsAlloc)
        {
            InitializeComponent();
            this.mbllIsAlloc = inIsAlloc;
            this.mstrForm = inForm;
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;
        }

        private DatabaseForms.frmBGYear pofrmGetBGYear = null;
        private DatabaseForms.frmEMProj pofrmGetProj = null;

        private bool mbllIsAlloc = false;
        private string mstrForm = "";
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
            dtbTemPdVer.Columns.Add("cRunNo", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcPrProj", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQcProj", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnProj", System.Type.GetType("System.String"));
            
            dtbTemPdVer.Columns.Add("nAmt01", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt02", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt03", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt04", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt05", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt06", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt07", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt08", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt09", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt10", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt11", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nAmt12", System.Type.GetType("System.Decimal"));

            dtbTemPdVer.Columns.Add("nPcn01", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn02", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn03", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn04", System.Type.GetType("System.Decimal"));
            dtbTemPdVer.Columns.Add("nPcn05", System.Type.GetType("System.Decimal"));
            
            dtbTemPdVer.Columns.Add("nTotal", System.Type.GetType("System.Decimal"), "nAmt01+nAmt02+nAmt03+nAmt04+nAmt05");
            dtbTemPdVer.Columns.Add("nPcnTotal", System.Type.GetType("System.Decimal"));

            dtbTemPdVer.Columns["nLevel"].DefaultValue = 0;
            dtbTemPdVer.Columns["nRun"].DefaultValue = 0;
            dtbTemPdVer.Columns["cSeq"].DefaultValue = "";
            dtbTemPdVer.Columns["cGroup"].DefaultValue = "";
            dtbTemPdVer.Columns["cRunNo"].DefaultValue = "";
            dtbTemPdVer.Columns["cType"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcPrProj"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcProj"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnProj"].DefaultValue = "";

            dtbTemPdVer.Columns["nAmt01"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt02"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt03"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt04"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt05"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt06"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt07"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt08"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt09"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt10"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt11"].DefaultValue = 0;
            dtbTemPdVer.Columns["nAmt12"].DefaultValue = 0;

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
            this.pmAllocPercent();

            Report.LocalDataSet.DTSBUDGET01 dtsPreviewReport = new Report.LocalDataSet.DTSBUDGET01();

            decimal[] decAmt = new decimal[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            this.pmSumBGTotal(ref decAmt);

            DataRow dtrPreview = null;

            string strMsg2 = "";
            switch (this.mstrForm)
            {
                case "FORM1":
                    strMsg2 = ""; 
                    break;
                case "FORM2":
                    strMsg2 = "    (ค่าใช้จ่ายลงทุน)";
                    break;
                case "FORM3":
                    strMsg2 = "";
                    break;
            }

            string strPYear = (this.mintYear + 543).ToString("0000");
            dtrPreview = dtsPreviewReport.XRBUDGET2.NewRow();
            dtrPreview["nLevel"] = 0;
            dtrPreview["nRun"] = 0;
            dtrPreview["cGroup"] = "";
            dtrPreview["cSeq"] = "";
            dtrPreview["cRunNo"] = "";
            dtrPreview["cType"] = "H";
            dtrPreview["cQcPrProj"] = "";
            dtrPreview["cQcProj"] = "";
            dtrPreview["cQnProj"] = "รวมงบประมาณรายจ่ายของปีงบประมาณ " + strPYear + strMsg2;
            dtrPreview["nAmt01"] = decAmt[0];
            dtrPreview["nAmt02"] = decAmt[1];
            dtrPreview["nAmt03"] = decAmt[2];
            dtrPreview["nAmt04"] = decAmt[3];
            dtrPreview["nAmt05"] = decAmt[4];
            dtrPreview["nAmt06"] = decAmt[5];
            dtrPreview["nAmt07"] = decAmt[6];
            dtrPreview["nAmt08"] = decAmt[7];
            dtrPreview["nAmt09"] = decAmt[8];
            dtrPreview["nAmt10"] = decAmt[9];
            dtrPreview["nAmt11"] = decAmt[10];
            dtrPreview["nAmt12"] = decAmt[11];

            dtsPreviewReport.XRBUDGET2.Rows.Add(dtrPreview);

            foreach (DataRow dtrTemBg in this.dtsDataEnv.Tables[this.mstrTemBg].Rows)
            {
                dtrPreview = dtsPreviewReport.XRBUDGET2.NewRow();
                dtrPreview["nLevel"] = Convert.ToInt32(dtrTemBg["nLevel"]);
                dtrPreview["nRun"] = Convert.ToInt32(dtrTemBg["nRun"]);
                dtrPreview["cGroup"] = dtrTemBg["cGroup"].ToString();
                dtrPreview["cSeq"] = dtrTemBg["cSeq"].ToString();
                dtrPreview["cRunNo"] = dtrTemBg["cRunNo"].ToString();
                dtrPreview["cType"] = dtrTemBg["cType"].ToString();
                dtrPreview["cQcPrProj"] = dtrTemBg["cQcPrProj"].ToString();
                dtrPreview["cQcProj"] = dtrTemBg["cQcProj"].ToString();
                dtrPreview["cQnProj"] = dtrTemBg["cQnProj"].ToString();
                dtrPreview["nAmt01"] = Convert.ToDecimal(dtrTemBg["nAmt01"]);
                dtrPreview["nAmt02"] = Convert.ToDecimal(dtrTemBg["nAmt02"]);
                dtrPreview["nAmt03"] = Convert.ToDecimal(dtrTemBg["nAmt03"]);
                dtrPreview["nAmt04"] = Convert.ToDecimal(dtrTemBg["nAmt04"]);
                dtrPreview["nAmt05"] = Convert.ToDecimal(dtrTemBg["nAmt05"]);
                dtrPreview["nAmt06"] = Convert.ToDecimal(dtrTemBg["nAmt06"]);
                dtrPreview["nAmt07"] = Convert.ToDecimal(dtrTemBg["nAmt07"]);
                dtrPreview["nAmt08"] = Convert.ToDecimal(dtrTemBg["nAmt08"]);
                dtrPreview["nAmt09"] = Convert.ToDecimal(dtrTemBg["nAmt09"]);
                dtrPreview["nAmt10"] = Convert.ToDecimal(dtrTemBg["nAmt10"]);
                dtrPreview["nAmt11"] = Convert.ToDecimal(dtrTemBg["nAmt11"]);
                dtrPreview["nAmt12"] = Convert.ToDecimal(dtrTemBg["nAmt12"]);

                dtsPreviewReport.XRBUDGET2.Rows.Add(dtrPreview);

            }
            if (dtsPreviewReport.XRBUDGET2.Rows.Count != 0)
            {
                this.dataGridView1.DataSource = dtsPreviewReport.XRBUDGET2;
                this.pmPreviewReport(dtsPreviewReport);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    dtrPLine["cGroup"] = inGroup;
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
                int intNRun = 0;
                foreach (DataRow dtrJob in this.dtsDataEnv.Tables["QJob"].Rows)
                {

                    decimal[] decAmt = new decimal[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    decimal decTotalAmt = this.pmSumBGAmt(this.mintYear, dtrJob["cRowID"].ToString(), ref decAmt);
                    if (decTotalAmt != 0)
                    {
                        DataRow dtrPLine = this.dtsDataEnv.Tables[this.mstrTemBg].NewRow();
                        
                        intNRun += 1;
                        dtrPLine["nLevel"] = inLevel + 1;
                        dtrPLine["nRun"] = intNRun;
                        dtrPLine["cGroup"] = inGroup;
                        //dtrPLine["cSeq"] = inRunNo + "." + intNRun.ToString();
                        dtrPLine["cRunNo"] = inRunNo + "." + intNRun.ToString();
                        dtrPLine["cType"] = "D";
                        dtrPLine["cQcPrProj"] = inQcProj;
                        dtrPLine["cQcProj"] = dtrJob["cCode"].ToString().TrimEnd();
                        dtrPLine["cQnProj"] = dtrJob["cName"].ToString().TrimEnd();
                        dtrPLine["nAmt01"] = decAmt[0];
                        dtrPLine["nAmt02"] = decAmt[1];
                        dtrPLine["nAmt03"] = decAmt[2];
                        dtrPLine["nAmt04"] = decAmt[3];
                        dtrPLine["nAmt05"] = decAmt[4];
                        dtrPLine["nAmt06"] = decAmt[5];
                        dtrPLine["nAmt07"] = decAmt[6];
                        dtrPLine["nAmt08"] = decAmt[7];
                        dtrPLine["nAmt09"] = decAmt[8];
                        dtrPLine["nAmt10"] = decAmt[9];
                        dtrPLine["nAmt11"] = decAmt[10];
                        dtrPLine["nAmt12"] = decAmt[11];

                        this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Add(dtrPLine);

                    }
                }
            }
        }

        private decimal pmSumBGAmt(int inBGYear, string inJob, ref decimal[] ioAmt)
        {
            decimal decAmt = 0;
            string strSQLText = "";

            strSQLText = " SELECT ";
            strSQLText += " SUM(BGTRANIT.NAMT1) as NAMT01";
            strSQLText += " , SUM(BGTRANIT.NAMT2) as NAMT02";
            strSQLText += " , SUM(BGTRANIT.NAMT3) as NAMT03";
            strSQLText += " , SUM(BGTRANIT.NAMT4) as NAMT04";
            strSQLText += " , SUM(BGTRANIT.NAMT5) as NAMT05";
            strSQLText += " , SUM(BGTRANIT.NAMT6) as NAMT06";
            strSQLText += " , SUM(BGTRANIT.NAMT7) as NAMT07";
            strSQLText += " , SUM(BGTRANIT.NAMT8) as NAMT08";
            strSQLText += " , SUM(BGTRANIT.NAMT9) as NAMT09";
            strSQLText += " , SUM(BGTRANIT.NAMT10) as NAMT10";
            strSQLText += " , SUM(BGTRANIT.NAMT11) as NAMT11";
            strSQLText += " , SUM(BGTRANIT.NAMT12) as NAMT12";
            strSQLText += " FROM BGTRANIT";
            strSQLText += " left join BGTRANHD on BGTRANHD.CROWID = BGTRANIT.CBGTRANHD";
            strSQLText += " left join BGCHARTHD on BGCHARTHD.CROWID = BGTRANIT.CBGCHARTHD";
            strSQLText += " WHERE ";
            strSQLText += " BGTRANHD.CCORP = ? ";
            strSQLText += " AND BGTRANHD.NBGYEAR = ? ";
            strSQLText += " AND BGTRANHD.CJOB = ? ";
            strSQLText += " AND  BGTRANHD.CISPOST = 'T'";

            string strCateg = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, inBGYear, inJob });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGTranHd", "BGTRANHD", strSQLText, ref strErrorMsg))
            {
                foreach (DataRow strHd in this.dtsDataEnv.Tables["QBGTranHd"].Rows)
                {
                    ioAmt[0] = (Convert.IsDBNull(strHd["nAmt01"]) ? 0 : Convert.ToDecimal(strHd["nAmt01"]));
                    ioAmt[1] = (Convert.IsDBNull(strHd["nAmt02"]) ? 0 : Convert.ToDecimal(strHd["nAmt02"]));
                    ioAmt[2] = (Convert.IsDBNull(strHd["nAmt03"]) ? 0 : Convert.ToDecimal(strHd["nAmt03"]));
                    ioAmt[3] = (Convert.IsDBNull(strHd["nAmt04"]) ? 0 : Convert.ToDecimal(strHd["nAmt04"]));
                    ioAmt[4] = (Convert.IsDBNull(strHd["nAmt05"]) ? 0 : Convert.ToDecimal(strHd["nAmt05"]));
                    ioAmt[5] = (Convert.IsDBNull(strHd["nAmt06"]) ? 0 : Convert.ToDecimal(strHd["nAmt06"]));
                    ioAmt[6] = (Convert.IsDBNull(strHd["nAmt07"]) ? 0 : Convert.ToDecimal(strHd["nAmt07"]));
                    ioAmt[7] = (Convert.IsDBNull(strHd["nAmt08"]) ? 0 : Convert.ToDecimal(strHd["nAmt08"]));
                    ioAmt[8] = (Convert.IsDBNull(strHd["nAmt09"]) ? 0 : Convert.ToDecimal(strHd["nAmt09"]));
                    ioAmt[9] = (Convert.IsDBNull(strHd["nAmt10"]) ? 0 : Convert.ToDecimal(strHd["nAmt10"]));
                    ioAmt[10] = (Convert.IsDBNull(strHd["nAmt11"]) ? 0 : Convert.ToDecimal(strHd["nAmt11"]));
                    ioAmt[11] = (Convert.IsDBNull(strHd["nAmt12"]) ? 0 : Convert.ToDecimal(strHd["nAmt12"]));
                }//end For Each

            } // End If
            return ioAmt[0] + ioAmt[1] + ioAmt[2] + ioAmt[3] + ioAmt[4] + ioAmt[5] + ioAmt[6] + ioAmt[7] + ioAmt[8] + ioAmt[9] + ioAmt[10] + ioAmt[11];
        }

        private void pmSumHirachy()
        {
            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cType = 'D' ", "cQcProj");
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
                    this.pmSumPrProj(dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
                }
            }
        }

        private void pmSumPrProj(string inQcProj , decimal inAmt1, decimal inAmt2, decimal inAmt3, decimal inAmt4, decimal inAmt5)
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
                    this.pmSumPrProj(dtrProj["cQcPrProj"].ToString(), decSumBG1, decSumBG2, decSumBG3, decSumBG4, decSumBG5);
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

        private void pmSumBGTotal(ref decimal[] ioAmt)
        {

            DataRow[] dtASum1 = this.dtsDataEnv.Tables[this.mstrTemBg].Select("cQcPrProj = '' ");
            for (int i = 0; i < dtASum1.Length; i++)
            {
                DataRow dtrProj = dtASum1[i];
                ioAmt[0] += Convert.ToDecimal(dtrProj["nAmt01"]);
                ioAmt[1] += Convert.ToDecimal(dtrProj["nAmt02"]);
                ioAmt[2] += Convert.ToDecimal(dtrProj["nAmt03"]);
                ioAmt[3] += Convert.ToDecimal(dtrProj["nAmt04"]);
                ioAmt[4] += Convert.ToDecimal(dtrProj["nAmt05"]);
                ioAmt[5] += Convert.ToDecimal(dtrProj["nAmt06"]);
                ioAmt[6] += Convert.ToDecimal(dtrProj["nAmt07"]);
                ioAmt[7] += Convert.ToDecimal(dtrProj["nAmt08"]);
                ioAmt[8] += Convert.ToDecimal(dtrProj["nAmt09"]);
                ioAmt[9] += Convert.ToDecimal(dtrProj["nAmt10"]);
                ioAmt[10] += Convert.ToDecimal(dtrProj["nAmt11"]);
                ioAmt[11] += Convert.ToDecimal(dtrProj["nAmt12"]);

            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //switch (this.mstrForm)
            //{ 
            //    case "FORM1":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSUMBG07.rpt";
            //        break;
            //    case "FORM2":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSUMBG08.rpt";
            //        break;
            //    case "FORM3":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSUMBG09.rpt";
            //        break;
            //}

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
            //string strPIsAlloc = (this.mbllIsAlloc ? "   (กรณีปันส่วน)" : "   (กรณีไม่ปันส่วน)");

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text + " ประจำปี " + strPYear + strPIsAlloc);
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
