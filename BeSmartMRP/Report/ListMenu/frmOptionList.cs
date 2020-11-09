
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

namespace BeSmartMRP.Report.ListMenu
{

    public partial class frmOptionList : UIHelper.frmBase
    {

        public frmOptionList(string inTableName, string inText)
        {
            InitializeComponent();

            this.mstrTableName = inTableName;
            this.mstrTableText = inText;
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.lblText1.Text = "ระบุช่วงรหัส" + this.mstrTableText + "ในการออกรายงาน :";
            this.Text = inText;
            this.mstrTaskName = inTaskName;
        }

        private UIHelper.IfrmDBBase pofrmGetOption1 = null;
        
        private bool mbllIsAlloc = false;
        private string mstrForm = "";
        private string mstrTaskName = "";
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();

        private string mstrTemBg = "TemBg";

        private string mstrTableName = "";
        private string mstrTableText = "";
        private string mstrSort = "CCODE";

        private string mstrPrefixFld = "CCORP";

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strErrorMsg = "";

            this.txtBegQcProj1.Text = "";
            this.txtBegQnProj1.Text = "";
            this.txtEndQcProj1.Text = "";
            this.txtEndQnProj1.Text = "";

            switch (this.mstrTableName)
            {
                case "EMACCHART":
                    this.mstrPrefixFld = "CCORPCHAR";
                    break;
                case "APPLOGIN":
                    this.mstrSort = "CRCODE";
                    break;
            }

            if (this.mstrTableName == MapTable.Table.EMCorp)
            {
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOpt", "CORP", "select top 1 * from " + this.mstrTableName + " order by cCode", ref strErrorMsg))
                {
                    this.txtBegQcProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.txtBegQnProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cName"].ToString().TrimEnd();
                }
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOpt", "CORP", "select top 1 * from " + this.mstrTableName + " order by cCode desc", ref strErrorMsg))
                {
                    this.txtEndQcProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.txtEndQnProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cName"].ToString().TrimEnd();
                }
            }
            else 
            {
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOpt", "CORP", "select top 1 * from " + this.mstrTableName + " where " + this.mstrPrefixFld + " = ? order by cCode", ref strErrorMsg))
                {
                    this.txtBegQcProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.txtBegQnProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cName"].ToString().TrimEnd();
                }

                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOpt", "CORP", "select top 1 * from " + this.mstrTableName + " where " + this.mstrPrefixFld + " = ? order by cCode desc", ref strErrorMsg))
                {
                    this.txtEndQcProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.txtEndQnProj1.Text = this.dtsDataEnv.Tables["QOpt"].Rows[0]["cName"].ToString().TrimEnd();
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "EMCORP":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmCorp(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMBRANCH":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmBranch(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMDEPT":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMDept(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMSECT":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMSect(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMPROJ":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMProj(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMJOB":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMJob(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGTYPE":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmBudType(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGCHARTHD":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmBudChart(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMACCHART":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMAcChart(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMUOM":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMUOM(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "APPEMPL":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmAppEmpl(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "APPLOGIN":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmAppLogin(FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTBEGQCPROJ1":
                case "TXTENDQCPROJ1":
                    this.pmInitPopUpDialog(this.mstrTableName);
                    this.pofrmGetOption1.ValidateField(inPara1, this.mstrSort, true);
                    if (this.pofrmGetOption1.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROJ2":
                case "TXTENDQCPROJ2":
                    this.pmInitPopUpDialog(this.mstrTableName);
                    this.pofrmGetOption1.ValidateField(inPara1, this.mstrSort, true);
                    if (this.pofrmGetOption1.PopUpResult)
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
                case "TXTBEGQCPROJ1":
                    dtrRetVal = this.pofrmGetOption1.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        if (this.mstrTableName == "APPLOGIN")
                        {
                            this.txtBegQcProj1.Text = dtrRetVal["cRCode"].ToString().TrimEnd();
                            this.txtBegQnProj1.Text = dtrRetVal["cLogin"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtBegQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                            this.txtBegQnProj1.Text = dtrRetVal["cName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        this.txtBegQcProj1.Text = "";
                        this.txtBegQnProj1.Text = "";
                    }
                    break;
                case "TXTENDQCPROJ1":
                    dtrRetVal = this.pofrmGetOption1.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        if (this.mstrTableName == "APPLOGIN")
                        {
                            this.txtEndQcProj1.Text = dtrRetVal["cRCode"].ToString().TrimEnd();
                            this.txtEndQnProj1.Text = dtrRetVal["cLogin"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                            this.txtEndQnProj1.Text = dtrRetVal["cName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        this.txtEndQcProj1.Text = "";
                        this.txtEndQnProj1.Text = "";
                    }
                    break;
            }
        }

        private void txtQcProj1_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = this.mstrSort;

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
                this.pmInitPopUpDialog(this.mstrTableName);
                e.Cancel = !this.pofrmGetOption1.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetOption1.PopUpResult)
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

            string strErrorMsg = "";
            string strSQLText = "";
            string strJoinTable = "";
            string strJoinFld = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            if (this.mstrTableName == MapTable.Table.EMCorp
                || this.mstrTableName == "APPLOGIN")
            {
                objSQLHelper.SetPara(new object[] { this.txtBegQcProj1.Text.TrimEnd(), this.txtEndQcProj1.Text.TrimEnd() });
                if (this.mstrTableName == "APPLOGIN")
                {
                    strSQLText = "select * from " + this.mstrTableName + " where CLOGIN between ? and ? ";
                }
                else
                {
                    strSQLText = "select * from " + this.mstrTableName + " where CCODE between ? and ? ";
                }
            }
            else
            {

                switch (this.mstrTableName)
                {
                    case "EMSECT":
                        strJoinTable = "EMDEPT";
                        strJoinFld = "CDEPT";
                        break;
                    case "EMJOB":
                        strJoinTable = "EMPROJ";
                        strJoinFld = "CPROJ";
                        break;
                    case "BGCHARTHD":
                        strJoinTable = "BGTYPE";
                        strJoinFld = "CBGTYPE";
                        break;
                }
                
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtBegQcProj1.Text.TrimEnd(), this.txtEndQcProj1.Text.TrimEnd() });

                string strFld = "MASTER.*";
                if (strJoinTable != string.Empty) strFld += ", J1.CCODE as QCPR01, J1.CNAME as QNPR01";
                strSQLText = " select " + strFld + " from " + this.mstrTableName + " MASTER ";
                if (strJoinTable != string.Empty) strSQLText += " left join " + strJoinTable + " J1 on J1.CROWID = MASTER." + strJoinFld;
                strSQLText += " where MASTER." + this.mstrPrefixFld + " = ? and MASTER." + this.mstrSort + " between ? and ? order by MASTER." + this.mstrSort;
            }

            Report.LocalDataSet.DTSLIST01 dtsPreviewReport = new Report.LocalDataSet.DTSLIST01();

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QList", this.mstrTableName, strSQLText, ref strErrorMsg);
            foreach (DataRow dtrProj in this.dtsDataEnv.Tables["QList"].Rows)
            {
                DataRow dtrPreview = dtsPreviewReport.XRLIST01.NewRow();

                switch (this.mstrTableName)
                {
                    case "APPLOGIN":
                        dtrPreview["cCode"] = dtrProj["cRCode"].ToString().TrimEnd();
                        dtrPreview["cName"] = dtrProj["cLogin"].ToString().TrimEnd();
                        break;
                    case "APPEMPL":
                        dtrPreview["cCode"] = dtrProj["cCode"].ToString().TrimEnd();
                        dtrPreview["cName"] = dtrProj["cName"].ToString().TrimEnd();
                        dtrPreview["cPRCode1"] = dtrProj["cRCode"].ToString().TrimEnd();
                        break;
                    default:
                        dtrPreview["cCode"] = dtrProj["cCode"].ToString().TrimEnd();
                        dtrPreview["cName"] = dtrProj["cName"].ToString().TrimEnd();
                        break;
                }

                switch (this.mstrTableName)
                {
                    case "EMJOB":
                    case "EMPROJ":
                    case "BGCHARTHD":
                        dtrPreview["cType"] = dtrProj["cType"].ToString().TrimEnd();
                        break;
                }

                if (strJoinTable != string.Empty)
                {
                    dtrPreview["cPRCode1"] = dtrProj["QCPR01"].ToString().TrimEnd();
                    dtrPreview["cPRName1"] = dtrProj["QNPR01"].ToString().TrimEnd();
                }
                dtsPreviewReport.XRLIST01.Rows.Add(dtrPreview);

            }
        
            if (dtsPreviewReport.XRLIST01.Rows.Count != 0)
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

            //strRPTFileName = Application.StartupPath + @"\RPT\XRLIST01.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //if (this.mstrTableName == MapTable.Table.EMCorp)
            //{
            //    AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "");
            //}
            //else 
            //{
            //    AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //}
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์รหัส" + this.mstrTableText + "ตั้งแต่ : " + this.txtBegQcProj1.Text + " ถึง " + this.txtEndQcProj1.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTableText.Trim());

            //switch (this.mstrTableName)
            //{
            //    case "EMSECT":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "รหัสฝ่าย");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ชื่อฝ่าย");
            //        break;
            //    case "EMPROJ":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ประเภท");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        break;
            //    case "BGCHARTHD":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ประเภท");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "รหัสประเภท");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ชื่อประเภท");
            //        break;
            //    case "EMJOB":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "รหัสโครงการ");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        break;
            //    case "APPEMPL":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "Login");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        break;
            //    default:
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "X");
            //        break;
            //}

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[4]);
            //prmCRPara["PFTABNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[5]);
            //prmCRPara["PFH1"].ApplyCurrentValues((ParameterValues)this.pACrPara[6]);
            //prmCRPara["PFH2"].ApplyCurrentValues((ParameterValues)this.pACrPara[7]);
            //prmCRPara["PFH3"].ApplyCurrentValues((ParameterValues)this.pACrPara[8]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }


    }
}
