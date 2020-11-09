
#define xd_DEBUG

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

using mBudget.Business.Entity;
using mBudget.DatabaseForms;
using mBudget.Business.Agents;
using mBudget.UIHelper;

namespace mBudget.Report.GenReport
{
    public partial class GENSALESUM : UIHelper.frmBase
    {

        private string mstrRefTable = "SALESUM01";
        private string mstrRefTable2 = "SALESUM02";
        private string mstrRefTable3 = "SALESUM03";
        private string mstrRefTable4 = "SALESUM04";
        private string mstrRefTable5 = "SALESUM05";
        private string mstrRefTable6 = "SALESUM06";

        private string mstrTemPdGrp = "TemPdGrp";
        private string mstrTemSaleSum = "TemSaleSum01";
        private string mstrTemSaleSum2 = "TemSaleSum02";
        private string mstrTemNewCoor = "TemNewCoor";
        private string mstrTemSGrp4 = "TemSGrp4";

        private DataSet dtsDataEnv = new DataSet();
        
        public GENSALESUM()
        {
            InitializeComponent();
            this.pmCreateTem();
            this.pmCreateTem2();

#if xd_DEBUG
            this.dataGridView1.Visible = true;
#endif

        }

        private void pmCreateTem()
        {

            DataTable dtbTemPdGrp = new DataTable(this.mstrTemPdGrp);
            dtbTemPdGrp.Columns.Add("cQcPdGrp", System.Type.GetType("System.String"));
            dtbTemPdGrp.Columns.Add("cQcCrZone", System.Type.GetType("System.String"));
            dtbTemPdGrp.Columns.Add("cQcEmZone", System.Type.GetType("System.String"));
            this.dtsDataEnv.Tables.Add(dtbTemPdGrp);

            DataTable dtbTemSaleSum = new DataTable(this.mstrTemSaleSum);

            dtbTemSaleSum.Columns.Add("cRefNo", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnPdGrp", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCrZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcCrZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnCrZone", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cEmZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcEmZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnEmZone", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cEmpl", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcEmpl", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnEmpl", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnCoor", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnProd", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cMonth", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cYear", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            //Sale Sum Field
            dtbTemSaleSum.Columns.Add("nAmt_SO", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Sale", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Ret", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Bill", System.Type.GetType("System.Decimal"));
            //Accumulate Field
            dtbTemSaleSum.Columns.Add("nAmt_SO_A", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Sale_A", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Ret_A", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Bill_A", System.Type.GetType("System.Decimal"));

            dtbTemSaleSum.Columns["cPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnPdGrp"].DefaultValue = "";

            dtbTemSaleSum.Columns["cCrZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcCrZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnCrZone"].DefaultValue = "";

            dtbTemSaleSum.Columns["cEmZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcEmZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnEmZone"].DefaultValue = "";

            dtbTemSaleSum.Columns["cEmpl"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcEmpl"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnEmpl"].DefaultValue = "";

            dtbTemSaleSum.Columns["cProd"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcProd"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnProd"].DefaultValue = "";

            dtbTemSaleSum.Columns["cCoor"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcCoor"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnCoor"].DefaultValue = "";

            dtbTemSaleSum.Columns["cMonth"].DefaultValue = "";
            dtbTemSaleSum.Columns["cYear"].DefaultValue = "";

            dtbTemSaleSum.Columns["nAmt_SO"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Sale"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Ret"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Bill"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_SO_A"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Sale_A"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Ret_A"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Bill_A"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemSaleSum);

        }

        private void pmCreateTem2()
        {

            DataTable dtbTemGrp = new DataTable(this.mstrTemSGrp4);

            dtbTemGrp.Columns.Add("cQcCrZone", System.Type.GetType("System.String"));
            dtbTemGrp.Columns.Add("cCrGrpType", System.Type.GetType("System.String"));
            dtbTemGrp.Columns.Add("cQcCrGrp", System.Type.GetType("System.String"));
            dtbTemGrp.Columns.Add("cQcEmpl", System.Type.GetType("System.String"));

            this.dtsDataEnv.Tables.Add(dtbTemGrp);

            DataTable dtbTemNewCoor = new DataTable(this.mstrTemNewCoor);

            dtbTemNewCoor.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemNewCoor.Columns.Add("cIsNewCoor", System.Type.GetType("System.String"));

            this.dtsDataEnv.Tables.Add(dtbTemNewCoor);

            
            DataTable dtbTemSaleSum = new DataTable(this.mstrTemSaleSum2);

            dtbTemSaleSum.Columns.Add("cRefNo", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCrGrpType", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cMCrGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cMQcCrGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cMQnCrGrp", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCrGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcCrGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnCrGrp", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCrZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcCrZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnCrZone", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cEmZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcEmZone", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnEmZone", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cEmpl", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcEmpl", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnEmpl", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnCoor", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnProd", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnPdGrp", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cPdClass", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcPdClass", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnPdClass", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cPdContent", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcPdCon", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnPdCon", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cMonth", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cYear", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("dDate", System.Type.GetType("System.DateTime"));

            //Sale Sum Field
            dtbTemSaleSum.Columns.Add("nSTarget", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_SO", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Sale", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Ret", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nAmt_Bill", System.Type.GetType("System.Decimal"));

            dtbTemSaleSum.Columns["cCrGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcCrGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnCrGrp"].DefaultValue = "";

            dtbTemSaleSum.Columns["cCrZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcCrZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnCrZone"].DefaultValue = "";

            dtbTemSaleSum.Columns["cEmZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcEmZone"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnEmZone"].DefaultValue = "";

            dtbTemSaleSum.Columns["cEmpl"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcEmpl"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnEmpl"].DefaultValue = "";

            dtbTemSaleSum.Columns["cProd"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcProd"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnProd"].DefaultValue = "";

            dtbTemSaleSum.Columns["cPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnPdGrp"].DefaultValue = "";

            dtbTemSaleSum.Columns["cPdContent"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcPdCon"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnPdCon"].DefaultValue = "";

            dtbTemSaleSum.Columns["cPdClass"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcPdClass"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnPdClass"].DefaultValue = "";

            dtbTemSaleSum.Columns["cCoor"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcCoor"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnCoor"].DefaultValue = "";

            dtbTemSaleSum.Columns["cMonth"].DefaultValue = "";
            dtbTemSaleSum.Columns["cYear"].DefaultValue = "";

            dtbTemSaleSum.Columns["nAmt_SO"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Sale"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Ret"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nAmt_Bill"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemSaleSum);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemSaleSum2];
            //this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemNewCoor];

            ////Update SALESUM01
            //this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Clear();

            //this.pmPrintData_GLRef_S1();
            //this.pmPrintData_Order_S1();
            //this.pmCalProcess_S1();
            //this.pmUpdateProcess_S1();

            ////Update SALESUM02
            //this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Clear();
            //this.pmPrintData_GLRef_S2();
            //this.pmCalProcess_S2();
            //this.pmUpdateProcess_S2();

            ////Update SALESUM03
            //this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Clear();
            //this.pmPrintData_GLRef_S3();
            //this.pmUpdateProcess_S3();

            //this.dtsDataEnv.Tables[this.mstrTemSGrp4].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemNewCoor].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Clear();

            //this.pmPrintData_GLRef_S4();
            //this.pmCalProcess_S4();
            //this.pmUpdateProcess_S4();

            //this.dtsDataEnv.Tables[this.mstrTemSGrp4].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemNewCoor].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Clear();

            //this.pmPrintData_GLRef_S5();
            //this.pmCalProcess_S5();
            //this.pmUpdateProcess_S5();

            this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Clear();
            this.pmPrintData_GLRef_S6();
            this.pmCalProcess_S6();
            this.pmUpdateProcess_S6();

            MessageBox.Show("Update Complete", "Application Message");
        }

        #region "Update SALESUM01"
        private void pmUpdateProcess_S1()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum01 where cCorp = ? and cPdGrp = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cPdGrp"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();
                dtrSaveInfo["cPdGrp"] = dtrSaleSum["cPdGrp"].ToString();

                dtrSaveInfo["QcPdGrp"] = dtrSaleSum["cQcPdGrp"].ToString();
                dtrSaveInfo["QnPdGrp"] = dtrSaleSum["cQnPdGrp"].ToString();
                dtrSaveInfo["Amt_SO"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO"]);
                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);
                dtrSaveInfo["Amt_Bill"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill"]);

                dtrSaveInfo["Amt_SO_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO_A"]);
                dtrSaveInfo["Amt_Sale_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale_A"]);
                dtrSaveInfo["Amt_Ret_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret_A"]);
                dtrSaveInfo["Amt_Bill_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill_A"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmCalProcess_S1()
        {
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemSaleSum].DefaultView;
            //dv.Sort = "cQcPdGrp, cYear, cMonth";
            dv.Sort = "dDate";
            this.dataGridView1.DataSource = dv;
            DateTime dttStart = Convert.ToDateTime(dv[0]["dDate"]);
            int intMthDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(dttStart, this.txtEndDate.DateTime.Date, "Month")) + 1;

            string strPdGrp = dv[0]["cPdGrp"].ToString();
            string strQcPdGrp = dv[0]["cQcPdGrp"].ToString();
            string strQnPdGrp = dv[0]["cQnPdGrp"].ToString();
            
            #region "Insert Blank Month"
            foreach (DataRow dtrPdGrp in this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows)
            {
                DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select("cQcPdGrp = '" + dtrPdGrp["cQcPdGrp"].ToString() + "'", "dDate");
                if (dtaSelect.Length > 0)
                {
                    for (int intCnt = 0; intCnt < intMthDiff; intCnt++)
                    {
                        strPdGrp = dtaSelect[0]["cPdGrp"].ToString();
                        strQcPdGrp = dtaSelect[0]["cQcPdGrp"].ToString();
                        strQnPdGrp = dtaSelect[0]["cQnPdGrp"].ToString();
                        DateTime dttDate = dttStart.AddMonths(intCnt);
                        DataRow dtrSumGLRef = null;
                        string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}'", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), strQcPdGrp });
                        DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr);
                        if (dtaSelect2.Length == 0)
                        {
                            dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                            dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                            dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");
                            dtrSumGLRef["cPdGrp"] = strPdGrp;
                            dtrSumGLRef["cQcPdGrp"] = strQcPdGrp;
                            dtrSumGLRef["cQnPdGrp"] = strQnPdGrp;
                            dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                            this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                        }
                    }
                }
            }
            #endregion

            #region "Cal Acumulate Field"
            foreach (DataRow dtrPdGrp in this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows)
            {

                DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select("cQcPdGrp = '" + dtrPdGrp["cQcPdGrp"].ToString() + "'", "dDate");
                if (dtaSelect.Length > 0)
                {

                    decimal decAccumAmt_SO = 0;
                    decimal decAccumAmt_Sale = 0;
                    decimal decAccumAmt_Ret = 0;
                    decimal decAccumAmt_Bill = 0;

                    string strCurrYear = "";

                    for (int intCnt = 0; intCnt < dtaSelect.Length; intCnt++)
                    {
                        if (strCurrYear != dtaSelect[intCnt]["cYear"].ToString())
                        {
                            strCurrYear = dtaSelect[intCnt]["cYear"].ToString();
                            decAccumAmt_SO = 0;
                            decAccumAmt_Sale = 0;
                            decAccumAmt_Ret = 0;
                            decAccumAmt_Bill = 0;
                        }

                        decAccumAmt_SO += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_SO"]);
                        decAccumAmt_Sale += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Sale"]);
                        decAccumAmt_Ret += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Ret"]);
                        decAccumAmt_Bill += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Bill"]);

                        dtaSelect[intCnt]["nAmt_SO_A"] = decAccumAmt_SO;
                        dtaSelect[intCnt]["nAmt_Sale_A"] = decAccumAmt_Sale;
                        dtaSelect[intCnt]["nAmt_Ret_A"] = decAccumAmt_Ret;
                        dtaSelect[intCnt]["nAmt_Bill_A"] = decAccumAmt_Bill;

                    }

                }

            }
            #endregion

        }

        private void pmPrintData_GLRef_S1()
        {

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;
                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}'", new string[] {dttDate.Year.ToString("0000") , dttDate.Month.ToString("00") , dtrGLRef["cQcPdGrp"].ToString()});
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                        dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                        dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();
                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        //dtrSumGLRef["cRefNo"] = dtrGLRef["fcRefNo"].ToString();
                        //dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcProd"].ToString();
                        //dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnProd"].ToString();
                        //dtrSumGLRef["dDate"] = dttDate;

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                        DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemPdGrp].Select("cQcPdGrp = '" + dtrGLRef["cQcPdGrp"].ToString() + "'");
                        if (dtaSelect2.Length == 0)
                        {
                            DataRow dtrPdGrp = this.dtsDataEnv.Tables[this.mstrTemPdGrp].NewRow();
                            dtrPdGrp["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                            this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Add(dtrPdGrp);
                        }

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);
                    decimal decBillAmt = this.pmCalLibAmt(dtrGLRef["fcSkid"].ToString());

                    decimal decAllocBillAmt = this.pmAllocBillAmt(dtrGLRef, decBillAmt);
                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);
                    decimal decSumBill = Convert.ToDecimal(dtrSumGLRef["nAmt_Bill"]);

                    dtrSumGLRef[strFld] = decSum + decAmt;
                    dtrSumGLRef["nAmt_Bill"] = decSumBill + decAllocBillAmt;

                }

            }

        }

        private void pmPrintData_Order_S1()
        {

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT ORDERH.FCSKID, ORDERH.FCREFTYPE, ORDERH.FNAMT, ORDERH.FNVATAMT, ORDERH.FCRFTYPE, ORDERH.FCREFNO, ORDERH.FCCODE, ORDERH.FCVATISOUT, ORDERH.FNDISCAMT1 ";
            strSQLStr = strSQLStr + ", ORDERI.FDDATE, ORDERI.FNQTY, ORDERI.FNPRICEKE, ORDERI.FNUMQTY, ORDERI.FNXRATE , ORDERI.FNDISCAMT ";
            strSQLStr = strSQLStr + ", COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + " from ORDERI ";
            strSQLStr = strSQLStr + " left join ORDERH on ORDERI.FCORDERH = ORDERH.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on ORDERI.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on ORDERI.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " where ORDERI.FCCORP = ? ";
            strSQLStr = strSQLStr + " and ORDERI.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and ORDERI.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and ORDERI.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and ORDERH.FCREFTYPE = 'SO' ";
            strSQLStr = strSQLStr + " and ORDERI.FCSTEP <> 'L' ";
            //strSQLStr = strSQLStr + " and ORDERH.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QORDERH", "ORDERH", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrORDERH in this.dtsDataEnv.Tables["QORDERH"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrORDERH["fdDate"]);

                    DataRow dtrSumORDERH = null;
                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}'", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), dtrORDERH["cQcPdGrp"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumORDERH = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                        dtrSumORDERH["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumORDERH["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumORDERH["cPdGrp"] = dtrORDERH["cPdGrp"].ToString();
                        dtrSumORDERH["cQcPdGrp"] = dtrORDERH["cQcPdGrp"].ToString();
                        dtrSumORDERH["cQnPdGrp"] = dtrORDERH["cQnPdGrp"].ToString();
                        dtrSumORDERH["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumORDERH);

                        DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemPdGrp].Select("cQcPdGrp = '" + dtrORDERH["cQcPdGrp"].ToString() + "'");
                        if (dtaSelect2.Length == 0)
                        {
                            DataRow dtrPdGrp = this.dtsDataEnv.Tables[this.mstrTemPdGrp].NewRow();
                            dtrPdGrp["cQcPdGrp"] = dtrORDERH["cQcPdGrp"].ToString();
                            this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Add(dtrPdGrp);
                        }

                    }
                    else
                    {
                        dtrSumORDERH = dtaSelect[0];
                    }

                    decimal decAmt = this.pmCalAmt2(dtrORDERH);   //Convert.ToDecimal(dtrORDERH["fnAmt"]);
                    decimal decSum = Convert.ToDecimal(dtrSumORDERH["nAmt_SO"]);
                    dtrSumORDERH["nAmt_SO"] = decSum + decAmt;

                }

            }

        }

#endregion

        #region "Update SALESUM02"
        private void pmUpdateProcess_S2()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum02 where cCorp = ? and cPdGrp = ? and cSaleZone = ? and cSEmpl = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cPdGrp"].ToString(), dtrSaleSum["cCrZone"].ToString(), dtrSaleSum["cEmpl"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable2, this.mstrRefTable2, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable2);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable2].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable2].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();

                dtrSaveInfo["cPdGrp"] = dtrSaleSum["cPdGrp"].ToString();
                dtrSaveInfo["QcPdGrp"] = dtrSaleSum["cQcPdGrp"].ToString();
                dtrSaveInfo["QnPdGrp"] = dtrSaleSum["cQnPdGrp"].ToString();

                dtrSaveInfo["cSaleZone"] = dtrSaleSum["cCrZone"].ToString();
                dtrSaveInfo["QcSaleZone"] = dtrSaleSum["cQcCrZone"].ToString();
                dtrSaveInfo["QnSaleZone"] = dtrSaleSum["cQnCrZone"].ToString();

                dtrSaveInfo["cSaleTeam"] = dtrSaleSum["cEmZone"].ToString();
                dtrSaveInfo["QcSaleTeam"] = dtrSaleSum["cQcEmZone"].ToString();
                dtrSaveInfo["QnSaleTeam"] = dtrSaleSum["cQnEmZone"].ToString();

                dtrSaveInfo["cSEmpl"] = dtrSaleSum["cEmpl"].ToString();
                dtrSaveInfo["QcSEmpl"] = dtrSaleSum["cQcEmpl"].ToString();
                dtrSaveInfo["QnSEmpl"] = dtrSaleSum["cQnEmpl"].ToString();

                dtrSaveInfo["Amt_SO"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO"]);
                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);
                dtrSaveInfo["Amt_Bill"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill"]);

                dtrSaveInfo["Amt_SO_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO_A"]);
                dtrSaveInfo["Amt_Sale_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale_A"]);
                dtrSaveInfo["Amt_Ret_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret_A"]);
                dtrSaveInfo["Amt_Bill_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill_A"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmPrintData_GLRef_S2()
        {

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", CRZONE.FCSKID as CCRZONE, CRZONE.FCCODE as CQCCRZONE, CRZONE.FCNAME as CQNCRZONE ";
            strSQLStr = strSQLStr + ", EMZONE.FCSKID as CEMZONE, EMZONE.FCCODE as CQCEMZONE, EMZONE.FCNAME as CQNEMZONE ";
            strSQLStr = strSQLStr + ", EMPL.FCSKID as CEMPL, EMPL.FCCODE as CQCEMPL, EMPL.FCNAME as CQNEMPL ";
            
            strSQLStr = strSQLStr + " from REFPROD ";

            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";

            strSQLStr = strSQLStr + " left join CRZONE on COOR.FCCRZONE = CRZONE.FCSKID ";
            strSQLStr = strSQLStr + " left join EMPL on GLREF.FCEMPL = EMPL.FCSKID ";
            strSQLStr = strSQLStr + " left join EMZONE on EMPL.FCEMZONE = EMZONE.FCSKID ";

            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;
                    string[] paStr = new string[] { 
                        dttDate.Year.ToString("0000")
                        , dttDate.Month.ToString("00")
                        , this.pmToString(dtrGLRef, "cQcPdGrp")
                        , this.pmToString(dtrGLRef, "cQcCrZone")
                        , this.pmToString(dtrGLRef, "cQcEmZone")
                        , this.pmToString(dtrGLRef, "cQcEmpl") };

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}' and cQcCrZone = '{3}' and cQcEmZone = '{4}' and cQcEmpl = '{5}' ", paStr);
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                        dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                        dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();

                        dtrSumGLRef["cCrZone"] = this.pmToString(dtrGLRef, "cCrZone");
                        dtrSumGLRef["cQcCrZone"] = this.pmToString(dtrGLRef, "cQcCrZone");
                        dtrSumGLRef["cQnCrZone"] = this.pmToString(dtrGLRef, "cQnCrZone");

                        dtrSumGLRef["cEmZone"] = this.pmToString(dtrGLRef, "cEmZone");
                        dtrSumGLRef["cQcEmZone"] = this.pmToString(dtrGLRef, "cQcEmZone");
                        dtrSumGLRef["cQnEmZone"] = this.pmToString(dtrGLRef, "cQnEmZone");

                        dtrSumGLRef["cEmpl"] = this.pmToString(dtrGLRef, "cEmpl");
                        dtrSumGLRef["cQcEmpl"] = this.pmToString(dtrGLRef, "cQcEmpl");
                        dtrSumGLRef["cQnEmpl"] = this.pmToString(dtrGLRef, "cQnEmpl");

                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        //dtrSumGLRef["cRefNo"] = dtrGLRef["fcRefNo"].ToString();
                        //dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcProd"].ToString();
                        //dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnProd"].ToString();
                        //dtrSumGLRef["dDate"] = dttDate;

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                        string[] paStr2 = new string[] {
                            this.pmToString(dtrGLRef, "cQcPdGrp")
                        , this.pmToString(dtrGLRef, "cQcCrZone")
                        , this.pmToString(dtrGLRef, "cQcEmZone") };

                        string strFilterStr2 = string.Format("cQcPdGrp = '{0}' and cQcCRZone = '{1}' and cQcEmZone = '{2}' ", paStr2);
                        DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemPdGrp].Select(strFilterStr2);
                        if (dtaSelect2.Length == 0)
                        {
                            DataRow dtrPdGrp = this.dtsDataEnv.Tables[this.mstrTemPdGrp].NewRow();
                            dtrPdGrp["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                            dtrPdGrp["cQcCrZone"] = this.pmToString(dtrGLRef, "cQcCrZone");
                            dtrPdGrp["cQcEmZone"] = this.pmToString(dtrGLRef, "cQcEmZone");
                            this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows.Add(dtrPdGrp);
                        }

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);
                    decimal decBillAmt = this.pmCalLibAmt(dtrGLRef["fcSkid"].ToString());

                    decimal decAllocBillAmt = this.pmAllocBillAmt(dtrGLRef, decBillAmt);
                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);
                    decimal decSumBill = Convert.ToDecimal(dtrSumGLRef["nAmt_Bill"]);

                    dtrSumGLRef[strFld] = decSum + decAmt;
                    dtrSumGLRef["nAmt_Bill"] = decSumBill + decAllocBillAmt;

                }

            }

        }

        private void pmCalProcess_S2()
        {
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemSaleSum].DefaultView;
            //dv.Sort = "cQcPdGrp, cYear, cMonth";
            dv.Sort = "dDate";
            //this.dataGridView1.DataSource = dv;
            DateTime dttStart = Convert.ToDateTime(dv[0]["dDate"]);
            int intMthDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(dttStart, this.txtEndDate.DateTime.Date, "Month")) + 1;

            string strPdGrp = dv[0]["cPdGrp"].ToString();
            string strQcPdGrp = dv[0]["cQcPdGrp"].ToString();
            string strQnPdGrp = dv[0]["cQnPdGrp"].ToString();

            dv.Sort = "cQcPdGrp, cQcCRZone, cQcEMZone";
            this.dataGridView1.DataSource = dv;

            #region "Insert Blank Month"
            string strPrefix = "";
            for (int intCnt = 0; intCnt < dv.Count; intCnt++)
            {
                DataRowView dtrPdGrp = dv[intCnt];
                if (strPrefix != dtrPdGrp["cQcPdGrp"].ToString().TrimEnd() + dtrPdGrp["cQcCRZone"].ToString().TrimEnd() + dtrPdGrp["cQcEMZone"].ToString().TrimEnd())
                {
                    strPrefix = dtrPdGrp["cQcPdGrp"].ToString().TrimEnd() + dtrPdGrp["cQcCRZone"].ToString().TrimEnd() + dtrPdGrp["cQcEMZone"].ToString().TrimEnd();


                    string[] paStr = new string[] { 
                        dtrPdGrp["cQcPdGrp"].ToString()
                        , dtrPdGrp["cQcCrZone"].ToString()
                        , dtrPdGrp["cQcEmZone"].ToString() };

                    string strFilterStr = string.Format("cQcPdGrp = '{0}' and cQcCrZone = '{1}' and cQcEmZone = '{2}' ", paStr);
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr, "dDate");

                    //DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select("cQcPdGrp = '" + dtrPdGrp["cQcPdGrp"].ToString() + "'", "dDate");

                    if (dtaSelect.Length > 0)
                    {
                        for (int intCnt2 = 0; intCnt2 < intMthDiff; intCnt2++)
                        {

                            DateTime dttDate = dttStart.AddMonths(intCnt2);
                            DataRow dtrSumGLRef = null;

                            string[] paStr2 = new string[] {
                                                    dttDate.Year.ToString("0000")
                                                    , dttDate.Month.ToString("00")
                                                    , dtaSelect[0]["cQcPdGrp"].ToString()
                                                    , dtaSelect[0]["cQcCrZone"].ToString()
                                                    , dtaSelect[0]["cQcEmZone"].ToString() };

                            string strFilterStr2 = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}' and cQcCRZone = '{3}' and cQcEmZone = '{4}' ", paStr2);

                            DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr2);

                            if (dtaSelect2.Length == 0)
                            {
                                dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                                dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                                dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                                dtrSumGLRef["cPdGrp"] = dtaSelect[0]["cPdGrp"].ToString();
                                dtrSumGLRef["cQcPdGrp"] = dtaSelect[0]["cQcPdGrp"].ToString();
                                dtrSumGLRef["cQnPdGrp"] = dtaSelect[0]["cQnPdGrp"].ToString();

                                dtrSumGLRef["cCRZone"] = dtaSelect[0]["cCRZone"].ToString();
                                dtrSumGLRef["cQcCRZone"] = dtaSelect[0]["cQcCRZone"].ToString(); ;
                                dtrSumGLRef["cQnCRZone"] = dtaSelect[0]["cQnCRZone"].ToString();

                                dtrSumGLRef["cEMZone"] = dtaSelect[0]["cEMZone"].ToString();
                                dtrSumGLRef["cQcEMZone"] = dtaSelect[0]["cQcEMZone"].ToString(); ;
                                dtrSumGLRef["cQnEMZone"] = dtaSelect[0]["cQnEMZone"].ToString();

                                dtrSumGLRef["cEmpl"] = dtaSelect[0]["cEmpl"].ToString();
                                dtrSumGLRef["cQcEmpl"] = dtaSelect[0]["cQcEmpl"].ToString(); ;
                                dtrSumGLRef["cQnEmpl"] = dtaSelect[0]["cQnEmpl"].ToString();

                                dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                                this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                            }
                        }
                    }

                }
            }

            #endregion



            #region "Cal Acumulate Field"
            foreach (DataRow dtrPdGrp in this.dtsDataEnv.Tables[this.mstrTemPdGrp].Rows)
            {

                string[] paStr = new string[] { 
                        dtrPdGrp["cQcPdGrp"].ToString()
                        , dtrPdGrp["cQcCrZone"].ToString()
                        , dtrPdGrp["cQcEmZone"].ToString() };

                string strFilterStr = string.Format("cQcPdGrp = '{0}' and cQcCrZone = '{1}' and cQcEmZone = '{2}' ", paStr);
                DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr, "dDate");

                if (dtaSelect.Length > 0)
                {

                    decimal decAccumAmt_SO = 0;
                    decimal decAccumAmt_Sale = 0;
                    decimal decAccumAmt_Ret = 0;
                    decimal decAccumAmt_Bill = 0;

                    for (int intCnt = 0; intCnt < dtaSelect.Length; intCnt++)
                    {
                        decAccumAmt_SO += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_SO"]);
                        decAccumAmt_Sale += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Sale"]);
                        decAccumAmt_Ret += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Ret"]);
                        decAccumAmt_Bill += Convert.ToDecimal(dtaSelect[intCnt]["nAmt_Bill"]);

                        dtaSelect[intCnt]["nAmt_SO_A"] = decAccumAmt_SO;
                        dtaSelect[intCnt]["nAmt_Sale_A"] = decAccumAmt_Sale;
                        dtaSelect[intCnt]["nAmt_Ret_A"] = decAccumAmt_Ret;
                        dtaSelect[intCnt]["nAmt_Bill_A"] = decAccumAmt_Bill;

                    }

                }

            }
            #endregion

        }

        #endregion

        #region "Update SALESUM03"
        private void pmPrintData_GLRef_S3()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCSKID as CCOOR, COOR.FCCODE as CQCCOOR, COOR.FCNAME as CQNCOOR, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM ";
            strSQLStr = strSQLStr + ", PROD.FCSKID as CPROD, PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", CRZONE.FCSKID as CCRZONE, CRZONE.FCCODE as CQCCRZONE, CRZONE.FCNAME as CQNCRZONE ";
            strSQLStr = strSQLStr + ", EMZONE.FCSKID as CEMZONE, EMZONE.FCCODE as CQCEMZONE, EMZONE.FCNAME as CQNEMZONE ";
            strSQLStr = strSQLStr + ", EMPL.FCSKID as CEMPL, EMPL.FCCODE as CQCEMPL, EMPL.FCNAME as CQNEMPL ";

            strSQLStr = strSQLStr + " from REFPROD ";

            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";

            strSQLStr = strSQLStr + " left join CRZONE on COOR.FCCRZONE = CRZONE.FCSKID ";
            strSQLStr = strSQLStr + " left join EMPL on GLREF.FCEMPL = EMPL.FCSKID ";
            strSQLStr = strSQLStr + " left join EMZONE on EMPL.FCEMZONE = EMZONE.FCSKID ";

            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;
                    string[] paStr = new string[] { 
                        dttDate.Year.ToString("0000")
                        , dttDate.Month.ToString("00")
                        , this.pmToString(dtrGLRef, "cQcEmpl")
                        , this.pmToString(dtrGLRef, "cQcCoor")
                        , this.pmToString(dtrGLRef, "cQcProd") };

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcEmpl = '{2}' and cQcCoor = '{3}' and cQcProd = '{4}' ", paStr);
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cPdGrp"] = this.pmToString(dtrGLRef, "cPdGrp");
                        dtrSumGLRef["cQcPdGrp"] = this.pmToString(dtrGLRef, "cQcPdGrp");
                        dtrSumGLRef["cQnPdGrp"] = this.pmToString(dtrGLRef, "cQnPdGrp");

                        dtrSumGLRef["cEmpl"] = this.pmToString(dtrGLRef, "cEmpl");
                        dtrSumGLRef["cQcEmpl"] = this.pmToString(dtrGLRef, "cQcEmpl");
                        dtrSumGLRef["cQnEmpl"] = this.pmToString(dtrGLRef, "cQnEmpl");

                        dtrSumGLRef["cCoor"] = this.pmToString(dtrGLRef, "cCoor");
                        dtrSumGLRef["cQcCoor"] = this.pmToString(dtrGLRef, "cQcCoor");
                        dtrSumGLRef["cQnCoor"] = this.pmToString(dtrGLRef, "cQnCoor");

                        dtrSumGLRef["cProd"] = this.pmToString(dtrGLRef, "cProd");
                        dtrSumGLRef["cQcProd"] = this.pmToString(dtrGLRef, "cQcProd");
                        dtrSumGLRef["cQnProd"] = this.pmToString(dtrGLRef, "cQnProd");

                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        dtrSumGLRef["cRefNo"] = dtrGLRef["fcRefNo"].ToString();
                        //dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcProd"].ToString();
                        //dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnProd"].ToString();

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);
                    decimal decBillAmt = this.pmCalLibAmt(dtrGLRef["fcSkid"].ToString());

                    decimal decAllocBillAmt = this.pmAllocBillAmt(dtrGLRef, decBillAmt);
                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);
                    //decimal decSumBill = Convert.ToDecimal(dtrSumGLRef["nAmt_Bill"]);

                    dtrSumGLRef[strFld] = decSum + decAmt;
                    //dtrSumGLRef["nAmt_Bill"] = decSumBill + decAllocBillAmt;

                }

            }

        }

        private void pmUpdateProcess_S3()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum03 where cCorp = ? and cSEmpl = ? and cProd = ? and cCoor = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cEmpl"].ToString(), dtrSaleSum["cProd"].ToString(), dtrSaleSum["cCoor"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable3, this.mstrRefTable3, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable3);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable3].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable3].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();

                dtrSaveInfo["cSEmpl"] = dtrSaleSum["cEmpl"].ToString();
                dtrSaveInfo["QcSEmpl"] = dtrSaleSum["cQcEmpl"].ToString();
                dtrSaveInfo["QnSEmpl"] = dtrSaleSum["cQnEmpl"].ToString();

                dtrSaveInfo["cPdGrp"] = dtrSaleSum["cPdGrp"].ToString();
                dtrSaveInfo["QcPdGrp"] = dtrSaleSum["cQcPdGrp"].ToString();
                dtrSaveInfo["QnPdGrp"] = dtrSaleSum["cQnPdGrp"].ToString();

                dtrSaveInfo["cProd"] = dtrSaleSum["cProd"].ToString();
                dtrSaveInfo["QcProd"] = dtrSaleSum["cQcProd"].ToString();
                dtrSaveInfo["QnProd"] = dtrSaleSum["cQnProd"].ToString();

                dtrSaveInfo["cCoor"] = dtrSaleSum["cCoor"].ToString();
                dtrSaveInfo["QcCoor"] = dtrSaleSum["cQcCoor"].ToString();
                dtrSaveInfo["QnCoor"] = dtrSaleSum["cQnCoor"].ToString();

                dtrSaveInfo["Amt_SO"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO"]);
                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);
                dtrSaveInfo["Amt_Bill"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill"]);

                dtrSaveInfo["Amt_SO_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_SO_A"]);
                dtrSaveInfo["Amt_Sale_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale_A"]);
                dtrSaveInfo["Amt_Ret_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret_A"]);
                dtrSaveInfo["Amt_Bill_A"] = Convert.ToDecimal(dtrSaleSum["nAmt_Bill_A"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        #endregion

        private void pmPrintData_GLRef_S4()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCSKID as CCOOR, COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM, COOR.FCCRGRP ";
            strSQLStr = strSQLStr + ", CRGRP.FCCODE as CQCCRGRP, CRGRP.FCNAME as CQNCRGRP ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", CRZONE.FCSKID as CCRZONE, CRZONE.FCCODE as CQCCRZONE, CRZONE.FCNAME as CQNCRZONE ";
            strSQLStr = strSQLStr + ", EMZONE.FCSKID as CEMZONE, EMZONE.FCCODE as CQCEMZONE, EMZONE.FCNAME as CQNEMZONE ";
            strSQLStr = strSQLStr + ", EMPL.FCSKID as CEMPL, EMPL.FCCODE as CQCEMPL, EMPL.FCNAME as CQNEMPL ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join CRGRP on COOR.FCCRGRP = CRGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join CRZONE on COOR.FCCRZONE = CRZONE.FCSKID ";
            strSQLStr = strSQLStr + " left join EMPL on GLREF.FCEMPL = EMPL.FCSKID ";
            strSQLStr = strSQLStr + " left join EMZONE on EMPL.FCEMZONE = EMZONE.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;
                    string strIsNewCoor = this.pmChkIsNewCoor(dtrGLRef["cCoor"].ToString(), dtrGLRef["cQcCoor"].ToString());

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcCrZone = '{2}' and cCrGrpType = '{3}' and cQcCrGrp = '{4}' and cQcEmpl = '{5}' ", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), dtrGLRef["cQcCrZone"].ToString(), strIsNewCoor, dtrGLRef["cQcCrGrp"].ToString(), dtrGLRef["cQcEmpl"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cCrZone"] = dtrGLRef["cCrZone"].ToString();
                        dtrSumGLRef["cQcCrZone"] = dtrGLRef["cQcCrZone"].ToString();
                        dtrSumGLRef["cQnCrZone"] = dtrGLRef["cQnCrZone"].ToString();

                        dtrSumGLRef["cCrGrp"] = dtrGLRef["fcCrGrp"].ToString();
                        dtrSumGLRef["cQcCrGrp"] = dtrGLRef["cQcCrGrp"].ToString();
                        dtrSumGLRef["cQnCrGrp"] = dtrGLRef["cQnCrGrp"].ToString();
                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);
                        dtrSumGLRef["cCrGrpType"] = strIsNewCoor;

                        dtrSumGLRef["cEmZone"] = dtrGLRef["cEmZone"].ToString();
                        dtrSumGLRef["cQcEmZone"] = dtrGLRef["cQcEmZone"].ToString();
                        dtrSumGLRef["cQnEmZone"] = dtrGLRef["cQnEmZone"].ToString();

                        dtrSumGLRef["cEmpl"] = dtrGLRef["cEmpl"].ToString();
                        dtrSumGLRef["cQcEmpl"] = dtrGLRef["cQcEmpl"].ToString();
                        dtrSumGLRef["cQnEmpl"] = dtrGLRef["cQnEmpl"].ToString();

                        //Get Parent CRGRP
                        objSQLHelper2.SetPara(new object[] { App.gcCorp, dtrGLRef["cQcCrGrp"].ToString() });
                        if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QM_CRGRP", "CRGRP", "select * from MASTER_CRGRP where cCorp = ? and cCode = ? ", ref strErrorMsg))
                        {
                            objSQLHelper.SetPara(new object[] { this.dtsDataEnv.Tables["QM_CRGRP"].Rows[0]["cParent"].ToString() });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCrGrp", "CRGRP", "select * from CRGRP where fcSkid = ? ", ref strErrorMsg))
                            {
                                DataRow dtrMCrGrp = this.dtsDataEnv.Tables["QCrGrp"].Rows[0];
                                dtrSumGLRef["cMCrGrp"] = dtrMCrGrp["fcSkid"].ToString();
                                dtrSumGLRef["cMQcCrGrp"] = dtrMCrGrp["fcCode"].ToString();
                                dtrSumGLRef["cMQnCrGrp"] = dtrMCrGrp["fcName"].ToString();
                            }
                        }

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                        string[] paStr = new string[] {
                            this.pmToString(dtrSumGLRef, "cQcCrZone") 
                            , this.pmToString(dtrSumGLRef, "cCrGrpType") 
                            , this.pmToString(dtrSumGLRef, "cQcCrGrp")
                            , this.pmToString(dtrSumGLRef, "cQcEmpl") };

                        string strFilterStr2 = string.Format("cQcCrZone = '{0}' and cCrGrpType = '{1}' and cQcCrGrp = '{2}' and cQcEmpl = '{3}' ", new string[] { dtrSumGLRef["cQcCrZone"].ToString(), dtrSumGLRef["cCrGrpType"].ToString(), dtrSumGLRef["cQcCrGrp"].ToString(), dtrSumGLRef["cQcEmpl"].ToString() });
                        DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSGrp4].Select(strFilterStr2);
                        if (dtaSelect2.Length == 0)
                        {
                            DataRow dtrSGrp4 = this.dtsDataEnv.Tables[this.mstrTemSGrp4].NewRow();

                            dtrSGrp4["cQcCrZone"] = dtrSumGLRef["cQcCrZone"].ToString();
                            dtrSGrp4["cCrGrpType"] = dtrSumGLRef["cCrGrpType"].ToString();
                            dtrSGrp4["cQcCrGrp"] = dtrSumGLRef["cQcCrGrp"].ToString();
                            dtrSGrp4["cQcEmpl"] = dtrSumGLRef["cQcEmpl"].ToString();

                            this.dtsDataEnv.Tables[this.mstrTemSGrp4].Rows.Add(dtrSGrp4);

                        }

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);

                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);

                    dtrSumGLRef[strFld] = decSum + decAmt;

                }

            }

        }

        private void pmCalProcess_S4()
        {
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].DefaultView;

            dv.Sort = "dDate";

            if (dv.Count == 0)
            {
                return;
            }

            DateTime dttStart = Convert.ToDateTime(dv[0]["dDate"]);
            int intMthDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(dttStart, this.txtEndDate.DateTime.Date, "Month")) + 1;

            dv.Sort = "cQcCrZone, cCrGrpType, cQcCrGrp, cQcEmpl, dDate";
            this.dataGridView1.DataSource = dv;

            #region "Insert Blank Month"
            string strPrefix = "";
            for (int intCnt = 0; intCnt < dv.Count; intCnt++)
            {
                DataRowView dtrPdGrp = dv[intCnt];
                if (strPrefix != dtrPdGrp["cQcCrZone"].ToString().TrimEnd() + dtrPdGrp["cCrGrpType"].ToString().TrimEnd() + dtrPdGrp["cQcCrGrp"].ToString().TrimEnd() + dtrPdGrp["cQcEmpl"].ToString().TrimEnd())
                {
                    strPrefix = dtrPdGrp["cQcCrZone"].ToString().TrimEnd() + dtrPdGrp["cCrGrpType"].ToString().TrimEnd() + dtrPdGrp["cQcCrGrp"].ToString().TrimEnd() + dtrPdGrp["cQcEmpl"].ToString().TrimEnd();


                    string[] paStr = new string[] { 
                        dtrPdGrp["cQcCrZone"].ToString()
                        , dtrPdGrp["cCrGrpType"].ToString()
                        , dtrPdGrp["cQcCrGrp"].ToString()
                        , dtrPdGrp["cQcEmpl"].ToString() };

                    string strFilterStr = string.Format("cQcCrZone = '{0}' and cCrGrpType = '{1}' and cQcCrGrp = '{2}' and cQcEmpl = '{3}' ", new string[] { dtrPdGrp["cQcCrZone"].ToString() , dtrPdGrp["cCrGrpType"].ToString(), dtrPdGrp["cQcCrGrp"].ToString() , dtrPdGrp["cQcEmpl"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr, "dDate");

                    if (dtaSelect.Length > 0)
                    {
                        for (int intCnt2 = 0; intCnt2 < intMthDiff; intCnt2++)
                        {

                            DateTime dttDate = dttStart.AddMonths(intCnt2);
                            DataRow dtrSumGLRef = null;

                            string[] paStr2 = new string[] {
                                                    dttDate.Year.ToString("0000")
                                                    , dttDate.Month.ToString("00")
                                                    , dtaSelect[0]["cQcCrZone"].ToString()
                                                    , dtaSelect[0]["cCrGrpType"].ToString()
                                                    , dtaSelect[0]["cQcCrGrp"].ToString()
                                                    , dtaSelect[0]["cQcEmpl"].ToString() };

                            string strFilterStr2 = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcCrZone = '{2}' and cCrGrpType = '{3}' and cQcCrGrp = '{4}' and cQcEmpl = '{5}' ", paStr2);
                            DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr2);

                            if (dtaSelect2.Length == 0)
                            {
                                dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                                dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                                dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                                dtrSumGLRef["cCRZone"] = dtaSelect[0]["cCRZone"].ToString();
                                dtrSumGLRef["cQcCRZone"] = dtaSelect[0]["cQcCRZone"].ToString(); ;
                                dtrSumGLRef["cQnCRZone"] = dtaSelect[0]["cQnCRZone"].ToString();

                                dtrSumGLRef["cCrGrpType"] = dtaSelect[0]["cCrGrpType"].ToString();

                                dtrSumGLRef["cMCrGrp"] = dtaSelect[0]["cMCrGrp"].ToString();
                                dtrSumGLRef["cMQcCrGrp"] = dtaSelect[0]["cMQcCrGrp"].ToString();
                                dtrSumGLRef["cMQnCrGrp"] = dtaSelect[0]["cMQnCrGrp"].ToString();

                                dtrSumGLRef["cCrGrp"] = dtaSelect[0]["cCrGrp"].ToString();
                                dtrSumGLRef["cQcCrGrp"] = dtaSelect[0]["cQcCrGrp"].ToString();
                                dtrSumGLRef["cQnCrGrp"] = dtaSelect[0]["cQnCrGrp"].ToString();

                                dtrSumGLRef["cEMZone"] = dtaSelect[0]["cEMZone"].ToString();
                                dtrSumGLRef["cQcEMZone"] = dtaSelect[0]["cQcEMZone"].ToString(); ;
                                dtrSumGLRef["cQnEMZone"] = dtaSelect[0]["cQnEMZone"].ToString();

                                dtrSumGLRef["cEmpl"] = dtaSelect[0]["cEmpl"].ToString();
                                dtrSumGLRef["cQcEmpl"] = dtaSelect[0]["cQcEmpl"].ToString(); ;
                                dtrSumGLRef["cQnEmpl"] = dtaSelect[0]["cQnEmpl"].ToString();

                                dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                                this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                            }
                        }
                    }

                }
            }

            #endregion

        }

        private void pmUpdateProcess_S4()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum04 where cCorp = ? and cSaleZone = ? and cCrGrpType = ? and cCrGrp = ? and cSEmpl = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cCrZone"].ToString(), dtrSaleSum["cCrGrpType"].ToString(), dtrSaleSum["cCrGrp"].ToString(), dtrSaleSum["cEmpl"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable4, this.mstrRefTable4, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable4);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable4].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable4].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();

                dtrSaveInfo["cSaleZone"] = dtrSaleSum["cCrZone"].ToString();
                dtrSaveInfo["cCrGrpType"] = dtrSaleSum["cCrGrpType"].ToString();
                dtrSaveInfo["cCrGrp"] = dtrSaleSum["cCrGrp"].ToString();
                dtrSaveInfo["cMCrGrp"] = dtrSaleSum["cMCrGrp"].ToString();
                dtrSaveInfo["cSEmpl"] = dtrSaleSum["cEmpl"].ToString();
                dtrSaveInfo["cSaleTeam"] = dtrSaleSum["cEmZone"].ToString();

                dtrSaveInfo["QcSaleZone"] = dtrSaleSum["cQcCrZone"].ToString();
                dtrSaveInfo["QnSaleZone"] = dtrSaleSum["cQnCrZone"].ToString();
                dtrSaveInfo["MQcCrGrp"] = dtrSaleSum["cMQcCrGrp"].ToString();
                dtrSaveInfo["MQnCrGrp"] = dtrSaleSum["cMQnCrGrp"].ToString();
                dtrSaveInfo["QcCrGrp"] = dtrSaleSum["cQcCrGrp"].ToString();
                dtrSaveInfo["QnCrGrp"] = dtrSaleSum["cQnCrGrp"].ToString();
                dtrSaveInfo["QcSaleTeam"] = dtrSaleSum["cQcEmZone"].ToString();
                dtrSaveInfo["QnSaleTeam"] = dtrSaleSum["cQnEmZone"].ToString();
                dtrSaveInfo["QcSEmpl"] = dtrSaleSum["cQcEmpl"].ToString();
                dtrSaveInfo["QnSEmpl"] = dtrSaleSum["cQnEmpl"].ToString();

                decimal decAmtSale = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);

                //dtrSaveInfo["SLTarget"] = decAmtSale + (decAmtSale * 10 / 100);
                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmPrintData_GLRef_S5()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCSKID as CCOOR, COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM, COOR.FCCRGRP ";
            strSQLStr = strSQLStr + ", CRGRP.FCCODE as CQCCRGRP, CRGRP.FCNAME as CQNCRGRP ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", CRZONE.FCSKID as CCRZONE, CRZONE.FCCODE as CQCCRZONE, CRZONE.FCNAME as CQNCRZONE ";
            strSQLStr = strSQLStr + ", EMZONE.FCSKID as CEMZONE, EMZONE.FCCODE as CQCEMZONE, EMZONE.FCNAME as CQNEMZONE ";
            strSQLStr = strSQLStr + ", EMPL.FCSKID as CEMPL, EMPL.FCCODE as CQCEMPL, EMPL.FCNAME as CQNEMPL ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join CRGRP on COOR.FCCRGRP = CRGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join CRZONE on COOR.FCCRZONE = CRZONE.FCSKID ";
            strSQLStr = strSQLStr + " left join EMPL on GLREF.FCEMPL = EMPL.FCSKID ";
            strSQLStr = strSQLStr + " left join EMZONE on EMPL.FCEMZONE = EMZONE.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;
                    string strIsNewCoor = this.pmChkIsNewCoor(dtrGLRef["cCoor"].ToString(), dtrGLRef["cQcCoor"].ToString());

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcCrZone = '{2}' and cQcCrGrp = '{3}' and cQcPdGrp = '{4}' ", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), dtrGLRef["cQcCrZone"].ToString(), dtrGLRef["cQcCrGrp"].ToString(), dtrGLRef["cQcPdGrp"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cCrZone"] = dtrGLRef["cCrZone"].ToString();
                        dtrSumGLRef["cQcCrZone"] = dtrGLRef["cQcCrZone"].ToString();
                        dtrSumGLRef["cQnCrZone"] = dtrGLRef["cQnCrZone"].ToString();

                        dtrSumGLRef["cCrGrp"] = dtrGLRef["fcCrGrp"].ToString();
                        dtrSumGLRef["cQcCrGrp"] = dtrGLRef["cQcCrGrp"].ToString();
                        dtrSumGLRef["cQnCrGrp"] = dtrGLRef["cQnCrGrp"].ToString();
                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        dtrSumGLRef["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                        dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                        dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();

                        dtrSumGLRef["cEmZone"] = dtrGLRef["cEmZone"].ToString();
                        dtrSumGLRef["cQcEmZone"] = dtrGLRef["cQcEmZone"].ToString();
                        dtrSumGLRef["cQnEmZone"] = dtrGLRef["cQnEmZone"].ToString();

                        dtrSumGLRef["cEmpl"] = dtrGLRef["cEmpl"].ToString();
                        dtrSumGLRef["cQcEmpl"] = dtrGLRef["cQcEmpl"].ToString();
                        dtrSumGLRef["cQnEmpl"] = dtrGLRef["cQnEmpl"].ToString();

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);

                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);

                    dtrSumGLRef[strFld] = decSum + decAmt;

                }

            }

        }

        private void pmCalProcess_S5()
        {
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].DefaultView;

            dv.Sort = "dDate";

            if (dv.Count == 0)
            {
                return;
            }

            DateTime dttStart = Convert.ToDateTime(dv[0]["dDate"]);
            int intMthDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(dttStart, this.txtEndDate.DateTime.Date, "Month")) + 1;

            dv.Sort = "cQcCrZone, cQcCrGrp, cQcPdGrp, dDate";
            this.dataGridView1.DataSource = dv;

            #region "Insert Blank Month"
            string strPrefix = "";
            for (int intCnt = 0; intCnt < dv.Count; intCnt++)
            {
                DataRowView dtrPdGrp = dv[intCnt];
                if (strPrefix != dtrPdGrp["cQcCrZone"].ToString().TrimEnd() + dtrPdGrp["cQcCrGrp"].ToString().TrimEnd() + dtrPdGrp["cQcPdGrp"].ToString().TrimEnd())
                {
                    strPrefix = dtrPdGrp["cQcCrZone"].ToString().TrimEnd() + dtrPdGrp["cQcCrGrp"].ToString().TrimEnd() + dtrPdGrp["cQcPdGrp"].ToString().TrimEnd();


                    string[] paStr = new string[] { 
                        dtrPdGrp["cQcCrZone"].ToString()
                        , dtrPdGrp["cQcCrGrp"].ToString()
                        , dtrPdGrp["cQcPdGrp"].ToString() };

                    string strFilterStr = string.Format("cQcCrZone = '{0}' and cQcCrGrp = '{1}' and cQcPdGrp = '{2}' ", new string[] { dtrPdGrp["cQcCrZone"].ToString(), dtrPdGrp["cQcCrGrp"].ToString(), dtrPdGrp["cQcPdGrp"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr, "dDate");

                    if (dtaSelect.Length > 0)
                    {
                        for (int intCnt2 = 0; intCnt2 < intMthDiff; intCnt2++)
                        {

                            DateTime dttDate = dttStart.AddMonths(intCnt2);
                            DataRow dtrSumGLRef = null;

                            string[] paStr2 = new string[] {
                                                    dttDate.Year.ToString("0000")
                                                    , dttDate.Month.ToString("00")
                                                    , dtaSelect[0]["cQcCrZone"].ToString()
                                                    , dtaSelect[0]["cQcCrGrp"].ToString()
                                                    , dtaSelect[0]["cQcPdGrp"].ToString() };

                            string strFilterStr2 = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcCrZone = '{2}' and cQcCrGrp = '{3}' and cQcPdGrp = '{4}' ", paStr2);
                            DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr2);

                            if (dtaSelect2.Length == 0)
                            {
                                dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                                dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                                dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                                dtrSumGLRef["cCRZone"] = dtaSelect[0]["cCRZone"].ToString();
                                dtrSumGLRef["cQcCRZone"] = dtaSelect[0]["cQcCRZone"].ToString(); ;
                                dtrSumGLRef["cQnCRZone"] = dtaSelect[0]["cQnCRZone"].ToString();

                                dtrSumGLRef["cCrGrp"] = dtaSelect[0]["cCrGrp"].ToString();
                                dtrSumGLRef["cQcCrGrp"] = dtaSelect[0]["cQcCrGrp"].ToString();
                                dtrSumGLRef["cQnCrGrp"] = dtaSelect[0]["cQnCrGrp"].ToString();

                                dtrSumGLRef["cPdGrp"] = dtaSelect[0]["cPdGrp"].ToString();
                                dtrSumGLRef["cQcPdGrp"] = dtaSelect[0]["cQcPdGrp"].ToString();
                                dtrSumGLRef["cQnPdGrp"] = dtaSelect[0]["cQnPdGrp"].ToString();

                                dtrSumGLRef["cEMZone"] = dtaSelect[0]["cEMZone"].ToString();
                                dtrSumGLRef["cQcEMZone"] = dtaSelect[0]["cQcEMZone"].ToString(); ;
                                dtrSumGLRef["cQnEMZone"] = dtaSelect[0]["cQnEMZone"].ToString();

                                dtrSumGLRef["cEmpl"] = dtaSelect[0]["cEmpl"].ToString();
                                dtrSumGLRef["cQcEmpl"] = dtaSelect[0]["cQcEmpl"].ToString(); ;
                                dtrSumGLRef["cQnEmpl"] = dtaSelect[0]["cQnEmpl"].ToString();

                                dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                                this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                            }
                        }
                    }

                }
            }

            #endregion

        }

        private void pmUpdateProcess_S5()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum05 where cCorp = ? and cSaleZone = ? and cCrGrp = ? and cPdGrp = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cCrZone"].ToString(), dtrSaleSum["cCrGrp"].ToString(), dtrSaleSum["cPdGrp"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable5, this.mstrRefTable5, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable5);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable5].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable5].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();

                dtrSaveInfo["cSaleZone"] = dtrSaleSum["cCrZone"].ToString();
                dtrSaveInfo["cCrGrp"] = dtrSaleSum["cCrGrp"].ToString();
                dtrSaveInfo["cPdGrp"] = dtrSaleSum["cPdGrp"].ToString();
                dtrSaveInfo["cSEmpl"] = dtrSaleSum["cEmpl"].ToString();
                dtrSaveInfo["cSaleTeam"] = dtrSaleSum["cEmZone"].ToString();

                dtrSaveInfo["QcSaleZone"] = dtrSaleSum["cQcCrZone"].ToString();
                dtrSaveInfo["QnSaleZone"] = dtrSaleSum["cQnCrZone"].ToString();
                dtrSaveInfo["QcCrGrp"] = dtrSaleSum["cQcCrGrp"].ToString();
                dtrSaveInfo["QnCrGrp"] = dtrSaleSum["cQnCrGrp"].ToString();
                dtrSaveInfo["QcPdGrp"] = dtrSaleSum["cQcPdGrp"].ToString();
                dtrSaveInfo["QnPdGrp"] = dtrSaleSum["cQnPdGrp"].ToString();
                dtrSaveInfo["QcSaleTeam"] = dtrSaleSum["cQcEmZone"].ToString();
                dtrSaveInfo["QnSaleTeam"] = dtrSaleSum["cQnEmZone"].ToString();
                dtrSaveInfo["QcSEmpl"] = dtrSaleSum["cQcEmpl"].ToString();
                dtrSaveInfo["QnSEmpl"] = dtrSaleSum["cQnEmpl"].ToString();

                decimal decAmtSale = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);

                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmPrintData_GLRef_S6()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", COOR.FCSKID as CCOOR, COOR.FCCODE as CQCCOOR, COOR.FCNAME2 as CQNCOOR2, COOR.FCSNAME2 as CQSCOOR2, COOR.FNCREDTERM, COOR.FCCRGRP ";
            strSQLStr = strSQLStr + ", CRGRP.FCCODE as CQCCRGRP, CRGRP.FCNAME as CQNCRGRP ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", CRZONE.FCSKID as CCRZONE, CRZONE.FCCODE as CQCCRZONE, CRZONE.FCNAME as CQNCRZONE ";
            strSQLStr = strSQLStr + ", EMZONE.FCSKID as CEMZONE, EMZONE.FCCODE as CQCEMZONE, EMZONE.FCNAME as CQNEMZONE ";
            strSQLStr = strSQLStr + ", EMPL.FCSKID as CEMPL, EMPL.FCCODE as CQCEMPL, EMPL.FCNAME as CQNEMPL ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join COOR on REFPROD.FCCOOR = COOR.FCSKID ";
            strSQLStr = strSQLStr + " left join CRGRP on COOR.FCCRGRP = CRGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join CRZONE on COOR.FCCRZONE = CRZONE.FCSKID ";
            strSQLStr = strSQLStr + " left join EMPL on GLREF.FCEMPL = EMPL.FCSKID ";
            strSQLStr = strSQLStr + " left join EMZONE on EMPL.FCEMZONE = EMZONE.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = null;

                    string strQcPdClass = "";
                    string strQcPdCon = "";

                    //Get Prod Class and Content
                    objSQLHelper2.SetPara(new object[] { App.gcCorp, dtrGLRef["cQcProd"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QM_Prod", "MPROD", "select * from MASTER_PROD where cCorp = ? and cCode = ? ", ref strErrorMsg))
                    {
                        DataRow dtrMProd = this.dtsDataEnv.Tables["QM_Prod"].Rows[0];
                        objSQLHelper2.SetPara(new object[] { dtrMProd["cPdContent"].ToString() });
                        if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPdCon", "PDCONTENT", "select * from PDCONTENT where cRowID = ? ", ref strErrorMsg))
                        {
                            DataRow dtrPdCon = this.dtsDataEnv.Tables["QPdCon"].Rows[0];
                            strQcPdCon = dtrPdCon["cCode"].ToString();
                        }

                        objSQLHelper2.SetPara(new object[] { dtrMProd["cPdClass"].ToString() });
                        if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPdClass", "PDCLASS", "select * from PDCLASS where cRowID = ? ", ref strErrorMsg))
                        {
                            DataRow dtrPdClass = this.dtsDataEnv.Tables["QPdClass"].Rows[0];
                            strQcPdClass = dtrPdClass["cCode"].ToString();
                        }

                    }

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdCon = '{2}' and cQcPdClass = '{3}' and cQcCrZone = '{4}' ", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), strQcPdCon, strQcPdClass, dtrGLRef["cQcCrZone"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr);
                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        dtrSumGLRef["cCrZone"] = dtrGLRef["cCrZone"].ToString();
                        dtrSumGLRef["cQcCrZone"] = dtrGLRef["cQcCrZone"].ToString();
                        dtrSumGLRef["cQnCrZone"] = dtrGLRef["cQnCrZone"].ToString();

                        dtrSumGLRef["cCrGrp"] = dtrGLRef["fcCrGrp"].ToString();
                        dtrSumGLRef["cQcCrGrp"] = dtrGLRef["cQcCrGrp"].ToString();
                        dtrSumGLRef["cQnCrGrp"] = dtrGLRef["cQnCrGrp"].ToString();
                        dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                        dtrSumGLRef["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                        dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                        dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();

                        dtrSumGLRef["cEmZone"] = dtrGLRef["cEmZone"].ToString();
                        dtrSumGLRef["cQcEmZone"] = dtrGLRef["cQcEmZone"].ToString();
                        dtrSumGLRef["cQnEmZone"] = dtrGLRef["cQnEmZone"].ToString();

                        dtrSumGLRef["cEmpl"] = dtrGLRef["cEmpl"].ToString();
                        dtrSumGLRef["cQcEmpl"] = dtrGLRef["cQcEmpl"].ToString();
                        dtrSumGLRef["cQnEmpl"] = dtrGLRef["cQnEmpl"].ToString();

                        //Get Prod Class and Content
                        objSQLHelper2.SetPara(new object[] { App.gcCorp, dtrGLRef["cQcProd"].ToString() });
                        if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QM_Prod", "MPROD", "select * from MASTER_PROD where cCorp = ? and cCode = ? ", ref strErrorMsg))
                        {
                            DataRow dtrMProd = this.dtsDataEnv.Tables["QM_Prod"].Rows[0];
                            objSQLHelper2.SetPara(new object[] { dtrMProd["cPdContent"].ToString() });
                            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPdCon", "PDCONTENT", "select * from PDCONTENT where cRowID = ? ", ref strErrorMsg))
                            {
                                DataRow dtrPdCon = this.dtsDataEnv.Tables["QPdCon"].Rows[0];
                                dtrSumGLRef["cPdContent"] = dtrPdCon["cRowID"].ToString();
                                dtrSumGLRef["cQcPdCon"] = dtrPdCon["cCode"].ToString();
                                dtrSumGLRef["cQnPdCon"] = dtrPdCon["cName"].ToString();
                            }

                            objSQLHelper2.SetPara(new object[] { dtrMProd["cPdClass"].ToString() });
                            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPdClass", "PDCLASS", "select * from PDCLASS where cRowID = ? ", ref strErrorMsg))
                            {
                                DataRow dtrPdClass = this.dtsDataEnv.Tables["QPdClass"].Rows[0];
                                dtrSumGLRef["cPdClass"] = dtrPdClass["cRowID"].ToString();
                                dtrSumGLRef["cQcPdClass"] = dtrPdClass["cCode"].ToString();
                                dtrSumGLRef["cQnPdClass"] = dtrPdClass["cName"].ToString();
                            }

                        }

                        this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    string strFld = (dtrGLRef["fcRfType"].ToString() == "F" ? "nAmt_Ret" : "nAmt_Sale");

                    decimal decAmt = this.pmCalAmt(dtrGLRef);

                    decimal decSum = Convert.ToDecimal(dtrSumGLRef[strFld]);

                    dtrSumGLRef[strFld] = decSum + decAmt;

                }

            }

        }

        private void pmCalProcess_S6()
        {

            DataView dv = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].DefaultView;

            dv.Sort = "dDate";

            if (dv.Count == 0)
            {
                return;
            }

            DateTime dttStart = Convert.ToDateTime(dv[0]["dDate"]);
            int intMthDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(dttStart, this.txtEndDate.DateTime.Date, "Month")) + 1;

            dv.Sort = "cQcPdCon, cQcPdClass, cQcCrZone, dDate";
            this.dataGridView1.DataSource = dv;

            #region "Insert Blank Month"
            string strPrefix = "";
            for (int intCnt = 0; intCnt < dv.Count; intCnt++)
            {
                DataRowView dtrPdGrp = dv[intCnt];
                if (strPrefix != dtrPdGrp["cQcPdCon"].ToString().TrimEnd() + dtrPdGrp["cQcPdClass"].ToString().TrimEnd() + dtrPdGrp["cQcCrZone"].ToString().TrimEnd())
                {

                    strPrefix = dtrPdGrp["cQcPdCon"].ToString().TrimEnd() + dtrPdGrp["cQcPdClass"].ToString().TrimEnd() + dtrPdGrp["cQcCrZone"].ToString().TrimEnd();

                    string[] paStr = new string[] { 
                        dtrPdGrp["cQcPdCon"].ToString()
                        , dtrPdGrp["cQcPdClass"].ToString()
                        , dtrPdGrp["cQcCrZone"].ToString() };

                    string strFilterStr = string.Format("cQcPdCon = '{0}' and cQcPdClass = '{1}' and cQcCrZone = '{2}' ", new string[] { dtrPdGrp["cQcPdCon"].ToString(), dtrPdGrp["cQcPdClass"].ToString(), dtrPdGrp["cQcCrZone"].ToString() });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr, "dDate");

                    if (dtaSelect.Length > 0)
                    {
                        for (int intCnt2 = 0; intCnt2 < intMthDiff; intCnt2++)
                        {

                            DateTime dttDate = dttStart.AddMonths(intCnt2);
                            DataRow dtrSumGLRef = null;

                            string[] paStr2 = new string[] {
                                                    dttDate.Year.ToString("0000")
                                                    , dttDate.Month.ToString("00")
                                                    , dtaSelect[0]["cQcPdCon"].ToString()
                                                    , dtaSelect[0]["cQcPdClass"].ToString()
                                                    , dtaSelect[0]["cQcCrZone"].ToString() };

                            string strFilterStr2 = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdCon = '{2}' and cQcPdClass = '{3}' and cQcCrZone = '{4}' ", paStr2);
                            DataRow[] dtaSelect2 = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Select(strFilterStr2);

                            if (dtaSelect2.Length == 0)
                            {
                                dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum2].NewRow();

                                dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                                dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                                dtrSumGLRef["cCRZone"] = dtaSelect[0]["cCRZone"].ToString();
                                dtrSumGLRef["cQcCRZone"] = dtaSelect[0]["cQcCRZone"].ToString(); ;
                                dtrSumGLRef["cQnCRZone"] = dtaSelect[0]["cQnCRZone"].ToString();

                                dtrSumGLRef["cCrGrp"] = dtaSelect[0]["cCrGrp"].ToString();
                                dtrSumGLRef["cQcCrGrp"] = dtaSelect[0]["cQcCrGrp"].ToString();
                                dtrSumGLRef["cQnCrGrp"] = dtaSelect[0]["cQnCrGrp"].ToString();

                                dtrSumGLRef["cPdGrp"] = dtaSelect[0]["cPdGrp"].ToString();
                                dtrSumGLRef["cQcPdGrp"] = dtaSelect[0]["cQcPdGrp"].ToString();
                                dtrSumGLRef["cQnPdGrp"] = dtaSelect[0]["cQnPdGrp"].ToString();

                                dtrSumGLRef["cEMZone"] = dtaSelect[0]["cEMZone"].ToString();
                                dtrSumGLRef["cQcEMZone"] = dtaSelect[0]["cQcEMZone"].ToString(); ;
                                dtrSumGLRef["cQnEMZone"] = dtaSelect[0]["cQnEMZone"].ToString();

                                dtrSumGLRef["cEmpl"] = dtaSelect[0]["cEmpl"].ToString();
                                dtrSumGLRef["cQcEmpl"] = dtaSelect[0]["cQcEmpl"].ToString(); ;
                                dtrSumGLRef["cQnEmpl"] = dtaSelect[0]["cQnEmpl"].ToString();

                                dtrSumGLRef["cPdContent"] = dtaSelect[0]["cPdContent"].ToString();
                                dtrSumGLRef["cQcPdCon"] = dtaSelect[0]["cQcPdCon"].ToString();
                                dtrSumGLRef["cQnPdCon"] = dtaSelect[0]["cQnPdCon"].ToString();

                                dtrSumGLRef["cPdClass"] = dtaSelect[0]["cPdClass"].ToString();
                                dtrSumGLRef["cQcPdClass"] = dtaSelect[0]["cQcPdClass"].ToString();
                                dtrSumGLRef["cQnPdClass"] = dtaSelect[0]["cQnPdClass"].ToString();

                                dtrSumGLRef["dDate"] = new DateTime(dttDate.Year, dttDate.Month, 1);

                                this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows.Add(dtrSumGLRef);

                            }

                        }

                    }

                }
            }

            #endregion

        }

        private void pmUpdateProcess_S6()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from SaleSum06 where cCorp = ? and cPdContent = ? and cPdClass = ? and cSaleZone = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemSaleSum2].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cPdContent"].ToString(), dtrSaleSum["cPdClass"].ToString(), dtrSaleSum["cCrZone"].ToString(), dttDate.Date });
                if (!objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable6, this.mstrRefTable6, strSQLStr, ref strErrorMsg))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strRowID = objConn.RunRowID(this.mstrRefTable6);
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable6].NewRow();
                    dtrSaveInfo["cRowID"] = strRowID;
                }
                else
                {
                    dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable6].Rows[0];
                }

                dtrSaveInfo["cCorp"] = App.gcCorp;
                dtrSaveInfo["dDate"] = dttDate;
                dtrSaveInfo["cYear"] = dtrSaleSum["cYear"].ToString();

                dtrSaveInfo["cSaleZone"] = dtrSaleSum["cCrZone"].ToString();
                dtrSaveInfo["cCrGrp"] = dtrSaleSum["cCrGrp"].ToString();
                dtrSaveInfo["cPdGrp"] = dtrSaleSum["cPdGrp"].ToString();
                dtrSaveInfo["cSEmpl"] = dtrSaleSum["cEmpl"].ToString();
                dtrSaveInfo["cSaleTeam"] = dtrSaleSum["cEmZone"].ToString();
                dtrSaveInfo["cPdContent"] = dtrSaleSum["cPdContent"].ToString();
                dtrSaveInfo["cPdClass"] = dtrSaleSum["cPdClass"].ToString();

                dtrSaveInfo["QcSaleZone"] = dtrSaleSum["cQcCrZone"].ToString();
                dtrSaveInfo["QnSaleZone"] = dtrSaleSum["cQnCrZone"].ToString();
                dtrSaveInfo["QcCrGrp"] = dtrSaleSum["cQcCrGrp"].ToString();
                dtrSaveInfo["QnCrGrp"] = dtrSaleSum["cQnCrGrp"].ToString();
                dtrSaveInfo["QcPdGrp"] = dtrSaleSum["cQcPdGrp"].ToString();
                dtrSaveInfo["QnPdGrp"] = dtrSaleSum["cQnPdGrp"].ToString();
                dtrSaveInfo["QcSaleTeam"] = dtrSaleSum["cQcEmZone"].ToString();
                dtrSaveInfo["QnSaleTeam"] = dtrSaleSum["cQnEmZone"].ToString();
                dtrSaveInfo["QcSEmpl"] = dtrSaleSum["cQcEmpl"].ToString();
                dtrSaveInfo["QnSEmpl"] = dtrSaleSum["cQnEmpl"].ToString();

                dtrSaveInfo["QcPdClass"] = dtrSaleSum["cQcPdClass"].ToString();
                dtrSaveInfo["QnPdClass"] = dtrSaleSum["cQnPdClass"].ToString();
                dtrSaveInfo["QcPdCon"] = dtrSaleSum["cQcPdCon"].ToString();
                dtrSaveInfo["QnPdCon"] = dtrSaleSum["cQnPdCon"].ToString();

                decimal decAmtSale = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);

                dtrSaveInfo["Amt_Sale"] = Convert.ToDecimal(dtrSaleSum["nAmt_Sale"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private string pmChkIsNewCoor(string inCoor, string inQcCoor)
        {
            string strRetVal = "Y";
            string strFilterStr = string.Format("cQcCoor = '{0}' ", new string[] { inQcCoor });
            DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemNewCoor].Select(strFilterStr);
            if (dtaSelect.Length == 0)
            {

                string strSQLStr = "select top 1 FCSKID from GLREF where FCCORP = ? and FCCOOR = ? and FDDATE < ? and FCRFTYPE in ('S', 'E', 'F') and FCSTAT <> 'C' ";

                DateTime dttChkBegDate = new DateTime(this.txtEndDate.DateTime.Year, 1, 1);
                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                objSQLHelper.SetPara(new object[] { App.gcCorp, inCoor, dttChkBegDate });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkCoor", "COOR", strSQLStr, ref strErrorMsg))
                {
                    //Seek เจอให้ถือเป็นลูกค้าเก่า
                    strRetVal = "N";
                }
                else
                {
                    strRetVal = "Y";
                }

                DataRow dtrTemNewCoor = this.dtsDataEnv.Tables[this.mstrTemNewCoor].NewRow();
                dtrTemNewCoor["cQcCoor"] = inQcCoor;
                dtrTemNewCoor["cIsNewCoor"] = strRetVal;
                this.dtsDataEnv.Tables[this.mstrTemNewCoor].Rows.Add(dtrTemNewCoor);

            }
            else
            {
                strRetVal = dtaSelect[0]["cIsNewCoor"].ToString().TrimEnd();
            }

            return strRetVal;
        }

        private string pmToString(DataRow inSource, string inFld)
        {
            return (Convert.IsDBNull(inSource[inFld]) ? "" : inSource[inFld].ToString());
        }

        private decimal pmAllocBillAmt(DataRow inSource, decimal inBillAmt)
        {
            decimal decVat_I = 0;
            decimal decValue = 0;
            decimal decBillAmt = 0;
            decimal decAmt = 0;
            decimal decAmt1 = 0;
            decimal decInvAmt = 0;
            decimal decUMQty = Convert.ToDecimal(inSource["fnUMQty"]);
            decUMQty = (decUMQty != 0 ? decUMQty : 1);

            decAmt = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnDiscAmt1"]) + Convert.ToDecimal(inSource["fnDiscAmt2"]);
            decBillAmt = Math.Round(inBillAmt / (1 + ( Convert.ToDecimal(inSource["fnVatRate"]) / 100)), 4);
            if (inSource["fcVatisOut"].ToString() == "N")
            {
                decAmt = decAmt + Convert.ToDecimal(inSource["fnVatAmt"]);
            }

            decimal decAmt_Item = (Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) * Convert.ToDecimal(inSource["fnQty"]) * decUMQty - Convert.ToDecimal(inSource["fnDiscAmt"])) * (decAmt == 0 ? 1 : (Convert.ToDecimal(inSource["fnAmt"]) / decAmt));

            if (decBillAmt != 0 && decAmt != 0)
            {
                decValue = decAmt_Item / decAmt * decBillAmt;
            }
            return Math.Round(decValue, 4);
        }

        private decimal pmCalAmt(DataRow inSource)
        {
            decimal decValue = 0;
            decimal decAmt = 0;
            decimal decUMQty = Convert.ToDecimal(inSource["fnUMQty"]);
            decUMQty = (decUMQty != 0 ? decUMQty : 1);

            decAmt = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnDiscAmt1"]) + Convert.ToDecimal(inSource["fnDiscAmt2"]);
            if (inSource["fcVatisOut"].ToString() == "N")
            {
                decAmt = decAmt + Convert.ToDecimal(inSource["fnVatAmt"]);
            }
            decValue = (Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) * Convert.ToDecimal(inSource["fnQty"]) * decUMQty - Convert.ToDecimal(inSource["fnDiscAmt"])) * (decAmt == 0 ? 1 : (Convert.ToDecimal(inSource["fnAmt"]) / decAmt));
            return Math.Round(decValue, 4);
        }

        private decimal pmCalAmt2(DataRow inSource)
        {
            decimal decValue = 0;
            decimal decAmt = 0;
            decimal decUMQty = Convert.ToDecimal(inSource["fnUMQty"]);

            decAmt = Convert.ToDecimal(inSource["fnAmt"]);
            if (inSource["fcVatisOut"].ToString() == "N")
            {
                decAmt = decAmt + Convert.ToDecimal(inSource["fnVatAmt"]);
            }
            decValue = (Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) * Convert.ToDecimal(inSource["fnQty"]) * decUMQty - Convert.ToDecimal(inSource["fnDiscAmt"])) * (decAmt == 0 ? 1 : (Convert.ToDecimal(inSource["fnAmt"]) / decAmt));
            //decValue = (Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) * Convert.ToDecimal(inSource["fnQty"]) * Convert.ToDecimal(inSource["fnUMQty"]) - Convert.ToDecimal(inSource["fnDiscAmt"]));
            return Math.Round(decValue, 4);
        }

        private decimal pmCalLibAmt(string inGLRef)
        {
            decimal decAmt = 0;

            string strSQLStr = "select sum( RefPay.fnPayamt ) as fnSmPayAmt from RefPay , GLRef ";
            strSQLStr += " where RefPay.fcGLRef = GLRef.fcSkid and RefPay.fcChildGLR = ? ";
            strSQLStr += " and RefPay.fdDate <= ? and GLRef.fdDate <= ? and GLRef.fcStat <> ? ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //objSQLHelper.SetPara(new object[] { inGLRef, this.txtBegDate.DateTime.Date, this.txtBegDate.DateTime.Date, "C" });
            DateTime dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);
            objSQLHelper.SetPara(new object[] { inGLRef, dttEndDate, dttEndDate, "C" });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefPay", "REFPAY", strSQLStr, ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefPay"].Rows[0]["fnSmPayAmt"]))
                {
                    decAmt = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefPay"].Rows[0]["fnSmPayAmt"]);
                }
            }
            return decAmt;
        }

    
    
    
    }
}