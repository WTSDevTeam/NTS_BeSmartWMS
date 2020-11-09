
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
    public partial class GENSTKSUM : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();

        private string mstrTemPd = "TemPd";
        private string mstrTemPd2 = "TemPd2";

        private string mstrBranch = "";
        private string mstrRefTable = "STOCK01";
        private string mstrRefTable2 = "STOCK02";

        public GENSTKSUM()
        {
            InitializeComponent();
            this.pmCreateTem();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            this.pmCalStock01();
            this.pmUpdateProcess_S1();

            this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Clear();
            this.pmCalStock02_Stk();
            this.pmCalStock02_Sale();
            this.pmUpdateProcess_S2();

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd2];
        
            MessageBox.Show("Update Complete", "Application Message");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemPd);

            dtbTemPd.Columns.Add("cPdGrp", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcPdGrp", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnPdGrp", System.Type.GetType("System.String"));

            dtbTemPd.Columns.Add("cMonth", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cYear", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("dDate", System.Type.GetType("System.DateTime"));

            dtbTemPd.Columns.Add("nQty_IN", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nQty_Out", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nQty_Ret", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt_IN", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt_Out", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt_Ret", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cPdGrp"].DefaultValue = "";
            dtbTemPd.Columns["cQcPdGrp"].DefaultValue = "";
            dtbTemPd.Columns["cQnPdGrp"].DefaultValue = "";

            dtbTemPd.Columns["cMonth"].DefaultValue = "";
            dtbTemPd.Columns["cYear"].DefaultValue = "";

            dtbTemPd.Columns["nQty_IN"].DefaultValue = 0;
            dtbTemPd.Columns["nQty_Out"].DefaultValue = 0;
            dtbTemPd.Columns["nQty_Ret"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt_IN"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt_Out"].DefaultValue = 0;
            dtbTemPd.Columns["nAmt_Ret"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd);


            DataTable dtbTemPd2 = new DataTable(this.mstrTemPd2);

            dtbTemPd2.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cMonth", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cYear", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd2.Columns.Add("nStdPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd2.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd2.Columns.Add("nSaleQty", System.Type.GetType("System.Decimal"));

            dtbTemPd2.Columns["cProd"].DefaultValue = "";
            dtbTemPd2.Columns["cQcProd"].DefaultValue = "";
            dtbTemPd2.Columns["cQnProd"].DefaultValue = "";

            dtbTemPd2.Columns["cMonth"].DefaultValue = "";
            dtbTemPd2.Columns["cYear"].DefaultValue = "";

            dtbTemPd2.Columns["nStdPrice"].DefaultValue = 0;
            dtbTemPd2.Columns["nQty"].DefaultValue = 0;
            dtbTemPd2.Columns["nSaleQty"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd2);

        }

        private void pmCalStock01()
        {

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            string strFld = " REFPROD.FCPROD , REFPROD.FCREFTYPE , REFPROD.FCIOTYPE , year(REFPROD.FDDATE) as CYEAR , month(REFPROD.FDDATE) as CMONTH , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD , REFPROD.FCREFTYPE , REFPROD.FCIOTYPE , year(REFPROD.FDDATE) , month(REFPROD.FDDATE) ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //stop = false;
            //thread = new Thread(new ThreadStart(StartPrint));
            //thread.Start();

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            //this.pmCreateBFStock();
            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date });

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                string strRefType = "";

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    //if (App.MoreProcess == false) break;

                    string strQcPdGrp = "";

                    objSQLHelper.SetPara(new object[] { dtrGLRef["fcProd"].ToString() });
                    string strSQLExec_Prod = "select Prod.fcCode, Prod.fcName, Prod.fcSName, UM.fcName as cQnUM, PdGrp.fcSkid as cPdGrp, PdGrp.fcCode as cQcPdGrp, PdGrp.fcName as cQnPdGrp from Prod  ";
                    strSQLExec_Prod += " left join UM on UM.fcSkid = Prod.fcUm ";
                    strSQLExec_Prod += " left join PDGRP on PDGRP.fcSkid = Prod.fcPdGrp ";
                    strSQLExec_Prod += " where Prod.fcSkid = ? ";

                    DataRow dtrProd = null;
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", strSQLExec_Prod, ref strErrorMsg))
                    {
                        dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        if (Convert.IsDBNull(dtrProd["cQcPdGrp"]))
                        {
                            strQcPdGrp = "ZZZZZZZZZ";
                        }
                        else
                        {
                            strQcPdGrp = dtrProd["cQcPdGrp"].ToString();
                            //strQcPdGrp = dtrProd["fcCode"].ToString();
                        }
                    }
                    else
                    {
                        continue;
                    }

                    DataRow dtrSumGLRef = null;
                    DateTime dttDate = new DateTime(Convert.ToInt32(dtrGLRef["cYear"]), Convert.ToInt32(dtrGLRef["cMonth"]), 1);

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcPdGrp = '{2}'", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), strQcPdGrp });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemPd].Select(strFilterStr);

                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                        //strQcProd = dtrProd["fcCode"].ToString();
                        if (Convert.IsDBNull(dtrProd["cQcPdGrp"]))
                        {
                            dtrSumGLRef["cQcPdGrp"] = "ZZZZZZZZZ";
                            dtrSumGLRef["cQnPdGrp"] = "Not Specific";
                        }
                        else
                        {
                            dtrSumGLRef["cPdGrp"] = dtrProd["cPdGrp"].ToString();
                            dtrSumGLRef["cQcPdGrp"] = dtrProd["cQcPdGrp"].ToString();
                            dtrSumGLRef["cQnPdGrp"] = dtrProd["cQnPdGrp"].ToString();

                            //dtrSumGLRef["cQcPdGrp"] = dtrProd["fcCode"].ToString();
                            //dtrSumGLRef["cQnPdGrp"] = dtrProd["fcName"].ToString();

                        }

                        dtrSumGLRef["dDate"] = dttDate;
                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    strRefType = dtrGLRef["fcRefType"].ToString();
                    switch (strRefType)
                    {
                        case "AJ":
                        //case "TR":
                        case "CS":
                            if (dtrGLRef["fcIOType"].ToString() == "I")
                            {
                                dtrSumGLRef["nQty_IN"] = Convert.ToDecimal(dtrSumGLRef["nQty_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                                dtrSumGLRef["nAmt_IN"] = Convert.ToDecimal(dtrSumGLRef["nAmt_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            }
                            else
                            {
                                dtrSumGLRef["nQty_Out"] = Convert.ToDecimal(dtrSumGLRef["nQty_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                                dtrSumGLRef["nAmt_Out"] = Convert.ToDecimal(dtrSumGLRef["nAmt_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            }
                            break;
                        case "BI":
                        case "BR":
                        case "BV":
                        case "RW":
                        case "RX":
                        case "FR":
                            dtrSumGLRef["nQty_IN"] = Convert.ToDecimal(dtrSumGLRef["nQty_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrSumGLRef["nAmt_IN"] = Convert.ToDecimal(dtrSumGLRef["nAmt_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                        case "WR":
                        case "WX":
                        case "SI":
                        case "SR":
                        case "SD":
                            dtrSumGLRef["nQty_Out"] = Convert.ToDecimal(dtrSumGLRef["nQty_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrSumGLRef["nAmt_Out"] = Convert.ToDecimal(dtrSumGLRef["nAmt_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                        case "SA":
                        case "SC":
                        case "SM":
                        case "SN":
                            dtrSumGLRef["nQty_Ret"] = Convert.ToDecimal(dtrSumGLRef["nQty_Ret"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrSumGLRef["nAmt_Ret"] = Convert.ToDecimal(dtrSumGLRef["nAmt_Ret"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                    }

                }
                this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];
            }

        }

        private void pmUpdateProcess_S1()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from Stock01 where cCorp = ? and cPdGrp = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
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
                dtrSaveInfo["Qty_IN"] = Convert.ToDecimal(dtrSaleSum["nQty_IN"]);
                dtrSaveInfo["Qty_Out"] = Convert.ToDecimal(dtrSaleSum["nQty_Out"]);
                dtrSaveInfo["Qty_Ret"] = Convert.ToDecimal(dtrSaleSum["nQty_Ret"]);

                dtrSaveInfo["Amt_IN"] = Convert.ToDecimal(dtrSaleSum["nAmt_IN"]);
                dtrSaveInfo["Amt_Out"] = Convert.ToDecimal(dtrSaleSum["nAmt_Out"]);
                dtrSaveInfo["Amt_Ret"] = Convert.ToDecimal(dtrSaleSum["nAmt_Ret"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmCalStock02_Stk()
        {


            string strFld = " REFPROD.FCPROD , REFPROD.FCIOTYPE , year(REFPROD.FDDATE) as CYEAR , month(REFPROD.FDDATE) as CMONTH , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD , REFPROD.FCIOTYPE , year(REFPROD.FDDATE) , month(REFPROD.FDDATE) ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //stop = false;
            //thread = new Thread(new ThreadStart(StartPrint));
            //thread.Start();

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            //this.pmCreateBFStock();
            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date });

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                string strRefType = "";

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    //if (App.MoreProcess == false) break;

                    string strQcProd = "";

                    objSQLHelper.SetPara(new object[] { dtrGLRef["fcProd"].ToString() });
                    string strSQLExec_Prod = "select Prod.fcSkid, Prod.fcCode, Prod.fcName, Prod.fcSName, Prod.fnPrice as fnStdPrice, UM.fcName as cQnUM, PdGrp.fcSkid as cPdGrp, PdGrp.fcCode as cQcPdGrp, PdGrp.fcName as cQnPdGrp from Prod  ";
                    strSQLExec_Prod += " left join UM on UM.fcSkid = Prod.fcUm ";
                    strSQLExec_Prod += " left join PDGRP on PDGRP.fcSkid = Prod.fcPdGrp ";
                    strSQLExec_Prod += " where Prod.fcSkid = ? ";

                    DataRow dtrProd = null;
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", strSQLExec_Prod, ref strErrorMsg))
                    {
                        dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        strQcProd = dtrProd["fcCode"].ToString();
                    }
                    else
                    {
                        continue;
                    }

                    DataRow dtrSumGLRef = null;
                    DateTime dttDate = new DateTime(Convert.ToInt32(dtrGLRef["cYear"]), Convert.ToInt32(dtrGLRef["cMonth"]), 1);

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcProd = '{2}'", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), strQcProd });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemPd2].Select(strFilterStr);

                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemPd2].NewRow();

                        dtrSumGLRef["cProd"] = dtrProd["fcSkid"].ToString();
                        dtrSumGLRef["cQcProd"] = dtrProd["fcCode"].ToString();
                        dtrSumGLRef["cQnProd"] = dtrProd["fcName"].ToString();
                        dtrSumGLRef["nStdPrice"] = Convert.ToDecimal(dtrProd["fnStdPrice"]);

                        dtrSumGLRef["dDate"] = dttDate;
                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    decimal decSign = (dtrGLRef["fcIOType"].ToString() == "I" ? 1 : -1);
                    decimal decQty = Convert.ToDecimal(dtrGLRef["fnSumQty"]) * decSign;

                    dtrSumGLRef["nQty"] = Convert.ToDecimal(dtrSumGLRef["nQty"]) + decQty;

                }

            }

        }

        private void pmCalStock02_Sale()
        {

            string strSQLStr = "SELECT REFPROD.FCREFTYPE, REFPROD.FNQTY , REFPROD.FNUMQTY, GLREF.FDDATE, GLREF.FCRFTYPE ";
            strSQLStr = strSQLStr + ", PROD.FCSKID as CPROD, PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD, PROD.FNPRICE AS FNSTDPRICE ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //stop = false;
            //thread = new Thread(new ThreadStart(StartPrint));
            //thread.Start();

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            //this.pmCreateBFStock();
            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date });

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                string strRefType = "";

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    //if (App.MoreProcess == false) break;

                    string strQcProd = dtrGLRef["cQcProd"].ToString();

                    DataRow dtrSumGLRef = null;
                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    string strFilterStr = string.Format("cYear = '{0}' and cMonth = '{1}' and cQcProd = '{2}'", new string[] { dttDate.Year.ToString("0000"), dttDate.Month.ToString("00"), strQcProd });
                    DataRow[] dtaSelect = this.dtsDataEnv.Tables[this.mstrTemPd2].Select(strFilterStr);

                    if (dtaSelect.Length == 0)
                    {
                        dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemPd2].NewRow();

                        dtrSumGLRef["cProd"] = dtrGLRef["cProd"].ToString();
                        dtrSumGLRef["cQcProd"] = dtrGLRef["cQcProd"].ToString();
                        dtrSumGLRef["cQnProd"] = dtrGLRef["cQnProd"].ToString();
                        dtrSumGLRef["nStdPrice"] = Convert.ToDecimal(dtrGLRef["fnStdPrice"]);

                        dtrSumGLRef["dDate"] = dttDate;
                        dtrSumGLRef["cYear"] = dttDate.Year.ToString("0000");
                        dtrSumGLRef["cMonth"] = dttDate.Month.ToString("00");

                        this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Add(dtrSumGLRef);

                    }
                    else
                    {
                        dtrSumGLRef = dtaSelect[0];
                    }

                    decimal decSign = (dtrGLRef["fcRFType"].ToString() == "F" ? -1 : 1);
                    decimal decUmQty = Convert.ToDecimal(dtrGLRef["fnUMQty"]);
                    decUmQty = (decUmQty == 0 ? 1 : decUmQty);
                    decimal decQty = Convert.ToDecimal(dtrGLRef["fnQty"]) * decUmQty * decSign;

                    dtrSumGLRef["nSaleQty"] = Convert.ToDecimal(dtrSumGLRef["nSaleQty"]) + decQty;

                }

            }

        }

        private void pmUpdateProcess_S2()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from Stock02 where cCorp = ? and cProd = ? and dDate = ?";

            foreach (DataRow dtrSaleSum in this.dtsDataEnv.Tables[this.mstrTemPd2].Rows)
            {
                string strRowID = "";
                bool bllIsNewRow = false;

                DataRow dtrSaveInfo = null;
                DateTime dttDate = Convert.ToDateTime(dtrSaleSum["dDate"]);
                objSQLHelper.SetPara(new object[] { App.gcCorp, dtrSaleSum["cProd"].ToString(), dttDate.Date });
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
                dtrSaveInfo["cProd"] = dtrSaleSum["cProd"].ToString();

                dtrSaveInfo["QcProd"] = dtrSaleSum["cQcProd"].ToString();
                dtrSaveInfo["QnProd"] = dtrSaleSum["cQnProd"].ToString();

                dtrSaveInfo["StdPrice"] = Convert.ToDecimal(dtrSaleSum["nStdPrice"]);
                dtrSaveInfo["Qty"] = Convert.ToDecimal(dtrSaleSum["nQty"]);
                dtrSaveInfo["Sale_Qty"] = Convert.ToDecimal(dtrSaleSum["nSaleQty"]);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);
            }
        }

        private void pmGetBFStock(string inQcProd, string inQcSect, ref decimal ioQty, ref decimal ioAmt)
        {
            decimal decQty = 0;
            decimal decAmt = 0;
            //DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cQcProd = '" +inQcProd+"' and cQcSect = '" +inQcSect+"' ");
            DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cQcProd = '" + inQcProd + "'");
            if (dtaSel.Length > 0)
            {
                for (int intCnt = 0; intCnt < dtaSel.Length; intCnt++)
                {
                    decQty += (Convert.ToDecimal(dtaSel[intCnt]["fnSumQty"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                    decAmt += (Convert.ToDecimal(dtaSel[intCnt]["fnSumCost"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                }
            }
            ioQty = decQty;
            ioAmt = decAmt;
        }

        private void pmCreateBFStock()
        {
            string strFld = " PROD.FCCODE as cQcProd , REFPROD.FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " and PDGRP.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " group by PROD.FCCODE , REFPROD.FCIOTYPE";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd() });

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg);

        }

        private void pmCreateBFStock2(string inProd)
        {
            string strFld = " PROD.FCCODE as cQcProd , REFPROD.FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCPROD = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by PROD.FCCODE , REFPROD.FCIOTYPE";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //objSQLHelper.SetPara(new object[] { inProd, this.txtBegDate.DateTime.Date });

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg);

        }
    
    }
}