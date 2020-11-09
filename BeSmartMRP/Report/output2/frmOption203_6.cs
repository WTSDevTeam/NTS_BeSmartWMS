
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

namespace BeSmartMRP.Report.output2
{

    public partial class frmOption203_6 : UIHelper.frmBase
    {

        /// <summary>
        /// ใช้ในรายงาน output 2 หัวข้อ
        /// PBGOUT2_03	    3. รายงานการเบิกจ่ายงบประมาณ-จำแนกตามผลผลิต/หน่วยงาน (แสดงงบรายจ่าย) – รายเดือน
        /// PBGOUT2_04	    4. รายงานการเบิกจ่ายงบประมาณ-จำแนกตามผลผลิต/กิจกรรมหลัก (แสดงงบรายจ่าย) – รายเดือน
        /// PBGOUT2_05	    5. รายงานการเบิกจ่ายงบประมาณ-จำแนกตามผลผลิต/หน่วยงาน (แสดงงบรายจ่าย) – รายไตรมาส
        /// PBGOUT2_06	    6. รายงานการเบิกจ่ายงบประมาณ-จำแนกตามผลผลิต/กิจกรรมหลัก (แสดงงบรายจ่าย) – รายไตรมาส
        /// ================
        /// รวมทั้งสิ้น
        /// 4 รายงาน
        /// ================
        /// โครงสร้างรายงาน            |
        /// ================
        /// FORM1 : ดูตามหน่วยงาน  |
        /// ================
        ///    - Group 1 : ผลผลิต (แผนงาน = OrgChart Level 1)
        ///        - Group 2 : ประเภทงบประมาณ (งบบุคลากร = 1+3)
        ///            - Group 4 : หมวด (แผนงานย่อย = OrgChart Level 3)
        ///                - Group 4 : ฝ่าย
        ///                    - Group 5 : งาน (โครงการ = OrgChart Level 4)
        ///
        /// ================
        /// FORM2 : ดูตามกิจกรรมหลัก (แผนงาน = OrgChart Level 2)
        /// ================
        ///    - Group 1 : ผลผลิต (แผนงาน = OrgChart Level 1)
        ///        - Group 2 : ประเภทงบประมาณ (งบบุคลากร = 1+3)
        ///            - Group 3 : กิจกรรมหลัก (แผนงาน = OrgChart Level 2)
        ///                - Group 4 : หมวด (แผนงานย่อย = OrgChart Level 3)
        ///                    - Group 5 : งาน (โครงการ = OrgChart Level 4)
        /// ================
        /// </summary>
        
        public frmOption203_6(string inForm, bool inIsAlloc)
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

            this.pnlMth.Visible= (this.mstrForm == "FORM1");
            this.pnlQuarter.Visible = (this.mstrForm == "FORM2");

            this.txtBegDate.DateTime = DateTime.Now;

            this.cmbQuarter.Properties.Items.Clear();
            this.cmbQuarter.Properties.Items.AddRange(new object[] { "1", "2", "3", "4" });
            this.cmbQuarter.SelectedIndex = 0;

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

        private void btnPrint_Click(object sender, EventArgs e)
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            
            this.pmPrintData();

        }

        private void pmPrintData()
        {

            this.dtsDataEnv.Tables[this.mstrTemBg].Rows.Clear();

            string strSQLProj1 = "select ";
            strSQLProj1 += " BGRECVHD.CREFTYPE , BGRECVHD.CCODE , BGRECVHD.CREFNO , BGRECVHD.DDATE ";
            strSQLProj1 += " , BGBOOK.CCODE as QCBOOK ";
            strSQLProj1 += " , BGRECVIT.NAMT ";
            strSQLProj1 += " , BGCHARTHD.CCODE as QCBGCHART, BGCHARTHD.CNAME as QNBGCHART";
            strSQLProj1 += " , PR.CCODE as QCPRBGCHART, PR.CNAME  as QNPRBGCHART";
            strSQLProj1 += " , BGUSEIT.NAMT as NUSEAMT, BGUSEIT.DDATE as DUSEDATE ";
            strSQLProj1 += " , EMJOB.CCODE as QCJOB, EMJOB.CNAME as QNJOB";
            //strSQLProj1 += " , (select sum(BGUSEIT.NAMT) from BGUSEIT where BGUSEIT.CBGRECVIT = BGRECVIT.CROWID and BGUSEIT.CSTAT <> 'C') as nUseAmt";
            strSQLProj1 += " from BGRECVIT ";
            strSQLProj1 += " left join BGRECVHD on BGRECVHD.CROWID = BGRECVIT.CBGRECVHD";
            strSQLProj1 += " left join BGCHARTHD on BGCHARTHD.CROWID = BGRECVIT.CBGCHARTHD ";
            strSQLProj1 += " left join BGCHARTHD PR on PR.CROWID = BGCHARTHD.CPRBGCHART ";
            strSQLProj1 += " left join BGBOOK on BGBOOK.CROWID = BGRECVHD.CBGBOOK ";
            strSQLProj1 += " left join EMJOB on EMJOB.CROWID = BGRECVHD.CJOB ";
            strSQLProj1 += " left join BGUSEIT on BGUSEIT.CBGRECVIT = BGRECVIT.CROWID ";
            strSQLProj1 += " where BGRECVHD.CCORP = ? and BGRECVHD.NBGYEAR = ? and BGRECVHD.DDATE between ? and ? and EMJOB.CCODE between ? and ?";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Report.LocalDataSet.DTSOUTPUT2 dtsPreviewReport = new Report.LocalDataSet.DTSOUTPUT2();

            DateTime dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            DateTime dttEndDate = dttBegDate;

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mintYear, dttBegDate, dttEndDate, this.txtBegQcProj1.Text.TrimEnd(), this.txtEndQcProj1.Text.TrimEnd() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProj1", "EMPROJ", strSQLProj1, ref strErrorMsg))
            {
                foreach (DataRow dtrRecvHD in this.dtsDataEnv.Tables["QProj1"].Rows)
                {
                    DataRow dtrPreview = dtsPreviewReport.XRBUDGET202.NewRow();

                    dtrPreview["cCode"] = dtrRecvHD["cRefType"].ToString().TrimEnd() + dtrRecvHD["QcBook"].ToString().TrimEnd() + "/" + dtrRecvHD["cCode"].ToString().TrimEnd();
                    dtrPreview["dDate"] = Convert.ToDateTime(dtrRecvHD["dDate"]);
                    dtrPreview["nRecvAmt"] = Convert.ToDecimal(dtrRecvHD["nAmt"]);
                    dtrPreview["cQcPRBGChart"] = dtrRecvHD["QcPRBGChart"].ToString();
                    dtrPreview["cQnPRBGChart"] = dtrRecvHD["QnPRBGChart"].ToString();
                    dtrPreview["cQcJob"] = dtrRecvHD["QcJob"].ToString();
                    dtrPreview["cQnJob"] = dtrRecvHD["QnJob"].ToString();

                    if (!Convert.IsDBNull(dtrRecvHD["nUseAmt"]))
                    {
                        dtrPreview["nUseAmt"] = (Convert.IsDBNull(dtrRecvHD["nUseAmt"]) ? 0 : Convert.ToDecimal(dtrRecvHD["nUseAmt"]));
                        dtrPreview["dUseDate"] = Convert.ToDateTime(dtrRecvHD["dUseDate"]);
                    }

                    dtsPreviewReport.XRBUDGET202.Rows.Add(dtrPreview);

                }
            }

            if (dtsPreviewReport.XRBUDGET202.Rows.Count > 0)
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

            //string strRPTFileName = "";

            //if (this.cmbQuarter.SelectedIndex == 0)
            //{
            //    strRPTFileName = Application.StartupPath + @"\RPT\PBGOUT2_02.rpt";
            //}
            //else
            //{
            //    strRPTFileName = Application.StartupPath + @"\RPT\PBGOUT2_02_DET.rpt";
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
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ตั้งแต่เดือน/ปี " + this.txtBegDate.DateTime.ToString("MM/yy"));
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPYear);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            ////prmCRPara["PFYEAR"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[4]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }


    }
}
