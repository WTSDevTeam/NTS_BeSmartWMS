
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

    public partial class frmPKeepLog : UIHelper.frmBase
    {

        public frmPKeepLog()
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

        private string mstrForm = "";
        private string mstrTaskName = "";
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();

        System.Data.OleDb.OleDbConnection conn = null;
        System.Data.OleDb.OleDbConnection conn2 = null;

        private void pmInitForm()
        {
            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            this.pmPrintData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmPrintData()
        {

            string strSQLText = "";
            string strFld = " KEEPLOG.DCREATE , KEEPLOG.CTYPE , APPOBJ.CDESC , KEEPLOG.CTASKNAME , KEEPLOG.CLOGINNAME , KEEPLOG.CMACHINE ";
            strFld += ", KEEPLOG.COLDCODE , KEEPLOG.CCODE";

            strSQLText = "select " + strFld + " from KEEPLOG ";
            strSQLText += " left join APPOBJ on APPOBJ.CTASKNAME = KEEPLOG.CTASKNAME ";
            strSQLText += " where KEEPLOG.CCORP = ? and KEEPLOG.DCREATE > ? and KEEPLOG.DCREATE < ? order by KEEPLOG.DCREATE desc ";

            string strErrorMsg = "";
            
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Report.LocalDataSet.DTSKEEPLOG dtsPreviewReport = new Report.LocalDataSet.DTSKEEPLOG();

            DateTime dttBegDate = this.txtBegDate.DateTime.Date.AddMinutes(-1);
            DateTime dttEndDate = this.txtEndDate.DateTime.Date.AddDays(1);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QKeepLog", "KEEPLOG", strSQLText, ref strErrorMsg))
            {
                foreach (DataRow dtrKeepLog in this.dtsDataEnv.Tables["QKeepLog"].Rows)
                {
                    DataRow dtrPreview = dtsPreviewReport.XRPKEEPLOG.NewRow();

                    dtrPreview["dCreate"] = Convert.ToDateTime(dtrKeepLog["dCreate"]);
                    dtrPreview["cType"] = dtrKeepLog["cType"].ToString().TrimEnd();
                    dtrPreview["cDesc"] = dtrKeepLog["cDesc"].ToString().TrimEnd() + (dtrKeepLog["cCode"].ToString().TrimEnd() != string.Empty ? " [" + dtrKeepLog["cCode"].ToString().TrimEnd() + "]" : "");
                    //dtrPreview["cDesc"] = dtrKeepLog["cDesc"].ToString().TrimEnd();
                    dtrPreview["cLogInName"] = dtrKeepLog["cLogInName"].ToString().TrimEnd();
                    dtrPreview["cMachine"] = dtrKeepLog["cMachine"].ToString().TrimEnd();

                    dtsPreviewReport.XRPKEEPLOG.Rows.Add(dtrPreview);

                }
            }

            if (dtsPreviewReport.XRPKEEPLOG.Rows.Count > 0)
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

            //strRPTFileName = Application.StartupPath + @"\RPT\XRPKEEPLOG.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
