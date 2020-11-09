﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.Linq;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using BeSmartMRP.DatabaseForms;

namespace BeSmartMRP.Transaction.Common.MRP
{

    public class cGenIssue
    {

        public cGenIssue(string inWOrderH, string inBranch, string inPlant, string inBook, DateTime inDate)
        {

            this.mstrWOrderH = inWOrderH;
            this.mstrGenIssue_Branch = inBranch;
            this.mstrGenIssue_Plant = inPlant;
            this.mstrGenIssue_Book = inBook;
            this.mdttGenIssue_Date = inDate;

            //Sect , Job ดูตาม Work Center ของแต่ละ OP
            //this.mstrGenIssue_Sect = inSect;
            //this.mstrGenIssue_Job = inJob;

            this.dtsDataEnv.CaseSensitive = true;
            this.pmCreateTem();

        }

        private string mstrWOrderH = "";
        private string mstrEditRowID_PR = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrRefType = DocumentType.WR.ToString();
        private string mstrRfType = "W";

        private string mstrDefaSect = "";
        private string mstrDefaJob = "";
        private string mstrStep = SysDef.gc_REF_STEP_CUT_STOCK;

        private DataSet dtsDataEnv = new DataSet();

        private StockAgent mStockAgent = new StockAgent(App.ERPConnectionString, App.ConnectionString, App.DatabaseReside);

        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        private string mstrHTable = MapTable.Table.GLRef;
        private string mstrEditRowID = "";

        private string mstrTemPd_GenIssue1 = "TemPd_GenIssue1";
        private string mstrTemPd_GenIssue = "TemPd_GenIssue";
        private string mstrTemPd = "TemPd";
        private string mstrTemStock = "TemStock";

        public static string xd_TemHistory_BakRev = "TemBackRev";
        public static string xd_TemHistory_WHBal = "TemWHBal";

        private string mstrGenIssue_Branch = "";
        private string mstrGenIssue_Plant = "";
        private string mstrGenIssue_Book = "";
        private string mstrGenIssue_Coor = "";
        private string mstrGenIssue_Code = "";
        private string mstrGenIssue_RefNo = "";
        private string mstrGenIssue_Sect = "";
        private string mstrGenIssue_Dept = "";
        private string mstrGenIssue_Job = "";
        private string mstrGenIssue_Proj = "";

        private string mstrRefToRefType = DocumentType.MO.ToString();
        private string mstrRefToTab = QMFWOrderHDInfo.TableName;
        private string mstrRefToBook = "";
        private string mstrRefToRowID = "";
        private decimal mdecRefToAmt = 0;
        private string mstrRefToMOrderOP = "";
        private string mstrRefToWOrderI = "";
        private string mstrOldRefToRowID = "";
        private string mstrOldRefToMOrderOP = "";

        private DateTime mdttGenIssue_Date = DateTime.Now;
        private int mintGenIssue_CredTerm = 0;
        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private string mstrCalBook_PR = "";
        private string mstrCalBook_PO = "";

        private string mstrLastRunCode = "";
        private long intRunCode = 0;
        private string mstrCurrQcCoor = "";
        private string mstrCurrCode = "";

        private string mstrFrWHouse = "";
        private string mstrToWHouse = "";
        private string mstrWHouse_RM = "";

        private string mstrFrWhType = "";
        private string mstrToWhType = "";

        private string mstrFrWhType2 = "";
        private string mstrToWhType2 = "";

        private string mstrSaleOrBuyForPdSer = "P";

        private decimal mdecAmt = 0;
        private decimal mdecVatAmt = 0;
        private decimal mdecDiscountAmt = 0;
        private decimal mdecXRate = 1;
        private decimal mdecTotPdQty = 0;
        private decimal mdecTotPdAmt = 0;
        private decimal mdecVatRate = 0;
        private decimal mdecGrossAmt = 0;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemPd_GenIssue);
            dtbTemPd.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nCostAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRefType", System.Type.GetType("System.String"));

            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nCostAmt"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemStk = new DataTable(this.mstrTemStock);
            dtbTemStk.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("nUseQty", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("nBalQty", System.Type.GetType("System.Decimal"));

            dtbTemStk.Columns["cRowID"].DefaultValue = "";
            dtbTemStk.Columns["cQcProd"].DefaultValue = "";
            dtbTemStk.Columns["cQcWHouse"].DefaultValue = "";
            dtbTemStk.Columns["cLot"].DefaultValue = "";
            dtbTemStk.Columns["nQty"].DefaultValue = 0;
            dtbTemStk.Columns["nUseQty"].DefaultValue = 0;
            dtbTemStk.Columns["nBalQty"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemStk);

            DataTable dtbTemGen1Issue = new DataTable(this.mstrTemPd_GenIssue1);
            dtbTemGen1Issue.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cFrWHouse", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cToWHouse", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));
            
            this.dtsDataEnv.Tables.Add(dtbTemGen1Issue);

        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private bool mbllStockOnly = false;
        public void GenIssue(bool inStockOnly)
        {

            this.mbllStockOnly = inStockOnly;
            
            this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
            this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_WIP;
            this.mstrSaleOrBuyForPdSer = "S";

            this.pmGenIssue();
        }

        private void pmGenIssue()
        {

            this.mStockAgent.CorpID = App.ActiveCorp.RowID;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //TODO: Next เรื่องการ Gen Issue Slip
            //=========================
            //Gen Issue Step
            //=========================
            //1) สร้าง WHouse List จาก Book ของ MO
            //2) select from WORDERIT_PD เพื่อเอา QTY, Prod และ Sect, Job จาก OPSEQ
            //3) แตก WHouse , Lot ลง TemPd จาก Stock ที่มีอยู่จริงและ Valid Stock ด้วยถ้าไม่มีให้แจ้งเตือนแล้วไม่สามารถ gen issue ได้
            //4) Loop TemPd Order by OPSeq, WHouse เพื่อสร้าง Issue Slip
            objSQLHelper.SetPara(new object[] { this.mstrWOrderH });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWOrderH", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
            {

                DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderH"].Rows[0];
                objSQLHelper.SetPara(new object[] { dtrWOrderOP[QMFWOrderHDInfo.Field.MfgBookID].ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QMfgBook", QMfgBookInfo.TableName, "select * from " + QMfgBookInfo.TableName + " where cRowID = ? ", ref strErrorMsg))
                {
                    this.mstrWHouse_RM = this.dtsDataEnv.Tables["QMfgBook"].Rows[0][QMfgBookInfo.Field.WHouse_RM].ToString().TrimEnd();
                    if (this.mstrWHouse_RM.Trim() != string.Empty)
                    {
                        this.mstrWHouse_RM = "(" + this.mstrWHouse_RM + ")";
                    }
                }

            }

            string strDefaFrWHouse = "";
            string strDefaToWHouse = "";
            string strDefaFrQcWHouse = "";
            string strDefaToQcWHouse = "";

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrGenIssue_Branch, SysDef.gc_WHOUSE_TYPE_NORMAL });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QWHouse", MapTable.Table.WHouse, "select * from " + MapTable.Table.WHouse + " where fcCorp = ? and fcBranch = ? and fcType = ? order by fcCode", ref strErrorMsg))
            {
                strDefaFrWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                strDefaFrQcWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
            }

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrGenIssue_Branch, SysDef.gc_WHOUSE_TYPE_WIP });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QWHouse", MapTable.Table.WHouse, "select * from " + MapTable.Table.WHouse + " where fcCorp = ? and fcBranch = ? and fcType = ? order by fcCode", ref strErrorMsg))
            {
                strDefaToWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                strDefaToQcWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
            }

            string strSQLStr_OP = " select MFWORDERIT_STDOP.CROWID , MFWORDERIT_STDOP.CWKCTRH , MFWKCTRHD.CSECT , MFWKCTRHD.CJOB from MFWORDERIT_STDOP ";
            strSQLStr_OP += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWORDERIT_STDOP.CWKCTRH ";
            strSQLStr_OP += " where MFWORDERIT_STDOP.CWORDERH = ? and MFWORDERIT_STDOP.COPSEQ = ? ";

            string strDefaSect = "";
            string strDefaJob = "";
            objSQLHelper.SetPara(new object[] { this.mstrWOrderH });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWOrderHD", MapTable.Table.MFWOrderHead, "select * from " + QMFWOrderHDInfo.TableName + " where CROWID = ?", ref strErrorMsg))
            {
                DataRow dtrWOrderHD = this.dtsDataEnv.Tables["QWOrderHD"].Rows[0];
                strDefaSect = dtrWOrderHD["cSect"].ToString();
                strDefaJob = dtrWOrderHD["cJob"].ToString();
            }

            DataRow dtrLoadHead = null;
            objSQLHelper.SetPara(new object[] { this.mstrWOrderH });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWOrderI", this.mstrITable2, "select * from " + this.mstrITable2 + " where cWOrderH = ? and cIOType = 'O' order by cOPSeq, cSeq", ref strErrorMsg);
            foreach (DataRow dtrWOrderI in this.dtsDataEnv.Tables["QWOrderI"].Rows)
            {

                string strOPSeq = dtrWOrderI["cOPSeq"].ToString().TrimEnd();
                string strProd = dtrWOrderI["cProd"].ToString();

                string strFrWHouse = "";
                string strFrQcWHouse = "";
                string strToWHouse = strDefaToWHouse;
                string strToQcWHouse = strDefaToQcWHouse;
                string strLot = "";
                string strSect = "";
                string strJob = "";
                string strQcProd = "";
                string strPdType = "";
                string strWOrderOP = "";

                decimal decQty = Convert.ToDecimal(dtrWOrderI["nQty"]);
                decimal decStkBalQty = this.pmLoadStockQty(strProd);

                objSQLHelper.SetPara(new object[] { this.mstrWOrderH, strOPSeq });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWOrderOP", MapTable.Table.RefProd, strSQLStr_OP, ref strErrorMsg))
                {
                    DataRow dtrWOrderOP = this.dtsDataEnv.Tables["QWOrderOP"].Rows[0];
                    strWOrderOP = dtrWOrderOP["cRowID"].ToString();
                    strSect = dtrWOrderOP["cSect"].ToString();
                    strJob = dtrWOrderOP["cJob"].ToString();
                }

                strSect = (strSect.Trim() == string.Empty ? strDefaSect : strSect);
                //strJob = (strSect.Trim() == string.Empty ? strDefaJob : strJob);
                strJob = strDefaJob;

                objSQLHelper2.SetPara(new object[] { dtrWOrderI["cProd"].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QProd", MapTable.Table.Product, "select * from " + MapTable.Table.Product + " where fcSkid = ? ", ref strErrorMsg))
                {
                    strProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSkid"].ToString();
                    strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                    strPdType = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcType"].ToString().TrimEnd();
                }

                decimal decStMoveQty = this.pmSumStMoveQty(objSQLHelper, objSQLHelper2, this.mstrRefToRefType, dtrWOrderI["cRowID"].ToString(), this.mstrRefType);
                if (decStMoveQty != 0)
                {
                    decQty = (decQty - decStMoveQty > 0 ? decQty - decStMoveQty : 0);
                }

                //objSQLHelper.SetPara(new object[] { this.mstrRefToRefType, dtrWOrderI["cRowID"].ToString(), this.mstrRefType });
                //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
                //{
                //    if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                //    {
                //        decimal decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                //        decQty = (decQty - decStMoveQty > 0 ? decQty - decStMoveQty : 0);
                //    }
                //}

                //Cut Stock
                decimal decCurrQty = decQty;

                if (decQty > 0)
                {
                    string strSel = "cQcProd = '{0}' and nBalQty > 0";
                    strSel = String.Format(strSel, new object[] { strQcProd });
                    DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemStock].Select(strSel);

                    //foreach (DataRow dtrTemStock in this.dtsDataEnv.Tables[this.mstrTemStock].Rows)
                    for (int i = 0; i < dtrSel.Length; i++)
                    {
                        DataRow dtrTemStock = dtrSel[i];

                        decimal decStkQty = Convert.ToDecimal(dtrTemStock["nBalQty"]);

                        strFrWHouse = dtrTemStock["cWHouse"].ToString();
                        strFrQcWHouse = dtrTemStock["cQcWHouse"].ToString();
                        strLot = dtrTemStock["cLot"].ToString();

                        if (decStkQty >= decCurrQty)
                        {

                            this.pmInsertRow(dtrWOrderI["cRowID"].ToString(), strWOrderOP, strProd, strQcProd, strPdType, dtrWOrderI["cUOM"].ToString(), dtrWOrderI["cOPSeq"].ToString(), dtrWOrderI["cSeq"].ToString(), strFrWHouse, strFrQcWHouse, strToWHouse, strToQcWHouse, strLot, decCurrQty, strSect, strJob);

                            decStkQty = decStkQty - decCurrQty;
                            decCurrQty = 0;
                            dtrTemStock["nUseQty"] = Convert.ToDecimal(dtrTemStock["nUseQty"]) + decQty;
                            dtrTemStock["nBalQty"] = Convert.ToDecimal(dtrTemStock["nBalQty"]) - Convert.ToDecimal(dtrTemStock["nUseQty"]);

                            break;
                        }
                        else
                        {
                            decCurrQty = decCurrQty - decStkQty;
                            dtrTemStock["nUseQty"] = Convert.ToDecimal(dtrTemStock["nUseQty"]) + decStkQty;
                            dtrTemStock["nBalQty"] = Convert.ToDecimal(dtrTemStock["nBalQty"]) - Convert.ToDecimal(dtrTemStock["nUseQty"]);

                            this.pmInsertRow(dtrWOrderI["cRowID"].ToString(), strWOrderOP, strProd, strQcProd, strPdType, dtrWOrderI["cUOM"].ToString(), dtrWOrderI["cOPSeq"].ToString(), dtrWOrderI["cSeq"].ToString(), strFrWHouse, strFrQcWHouse, strToWHouse, strToQcWHouse, strLot, decStkQty, strSect, strJob);

                            decStkQty = 0;
                        }

                    }
                
                }
                //ยอดคงเหลือไม่พอตัด Stock ถ้า Stock ไม่ให้ติดลบให้ขึ้น Error ยกเว้นสินค้า Type 5 ยอมให้ stock ติดลบได้
                if (decCurrQty > 0 && !this.mbllStockOnly)
                {
                    strFrWHouse = strDefaFrWHouse;
                    strFrQcWHouse = strDefaFrQcWHouse;
                    strLot = "";
                    this.pmInsertRow(dtrWOrderI["cRowID"].ToString(), strWOrderOP, strProd, strQcProd, strPdType, dtrWOrderI["cUOM"].ToString(), dtrWOrderI["cOPSeq"].ToString(), dtrWOrderI["cSeq"].ToString(), strFrWHouse, strFrQcWHouse, strToWHouse, strToQcWHouse, strLot, decCurrQty, strSect, strJob);
                    decCurrQty = 0;
                }



            }


            this.pmRunAllCode();

            string strErr2 = "";
            if (this.pmSaveAllOrder(ref strErr2))
            {
                MessageBox.Show("Gen Issue Slip Complete !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (strErr2 != "")
                    MessageBox.Show(strErr2, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private decimal pmSumStMoveQty(WS.Data.Agents.cDBMSAgent inSQLHelper, WS.Data.Agents.cDBMSAgent inSQLHelper2, string inRefToRefType, string inChildI, string inMasterType)
        {
            decimal decQty = 0;
            string strErrorMsg = "";
            string strSQLStr = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = inSQLHelper2;

            string strSQL_ChkValid = "select REFPROD.FCSKID from REFPROD";
            strSQL_ChkValid += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF";
            strSQL_ChkValid += " where REFPROD.FCSKID = ? and GLREF.FCSKID = ?";
         
            objSQLHelper.SetPara(new object[] { inRefToRefType, inChildI, inMasterType });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefDoc_Sum1", "REFDOC_STMOVE", "select * from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg);
            foreach (DataRow dtrSumStQty in this.dtsDataEnv.Tables["QRefDoc_Sum1"].Rows)
            {
                objSQLHelper2.SetPara(new object[] { dtrSumStQty["cMasterI"].ToString(), dtrSumStQty["cMasterH"].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk01", "REFPROD", strSQL_ChkValid, ref strErrorMsg))
                {
                    decQty = decQty + Convert.ToDecimal(dtrSumStQty["nQty"]);
                }
            }

            return decQty;
        }

        private void pmRunAllCode()
        {
            //DataView dv = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].DefaultView;
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Select(" nQty > 0", "cOPSeq, cFrQcWHouse, cQcProd, cLot, cSeq");
            //dv.Sort = "cOPSeq, cFrQcWHouse, cQcProd, cLot, cSeq";
            //dv.Sort = "cOPSeq, cFrQcWHouse";

            string strKey = "";
            for (int i = 0; i < dtrSel.Length; i++)
            {
                //DataRowView dtrRunCode = dtrSel[i];
                DataRow dtrRunCode = dtrSel[i];

                if (strKey != dtrRunCode["cOPSeq"].ToString().Trim() + dtrRunCode["cFrQcWHouse"].ToString().Trim())
                {
                    strKey = dtrRunCode["cOPSeq"].ToString().Trim() + dtrRunCode["cFrQcWHouse"].ToString().Trim();
                    this.pmRunCode_Issue();
                    DataRow[] dtrChk = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue1].Select("cCode = '" + this.mstrGenIssue_Code + "'");
                    if (dtrChk.Length == 0)
                    {
                        this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue1].Rows.Add(new object[] { this.mstrGenIssue_Code, this.mstrGenIssue_RefNo, dtrRunCode["cFrWHouse"].ToString(), dtrRunCode["cToWHouse"].ToString(), dtrRunCode["cSect"].ToString(), dtrRunCode["cJob"].ToString(), dtrRunCode["cWOrderOP"].ToString() });
                    }

                }
                dtrSel[i]["cCode"] = this.mstrGenIssue_Code;
                dtrSel[i]["cRefNo"] = this.mstrGenIssue_RefNo;

            }

        }

        private bool pmRunCode_Issue()
        {
            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            pAPara = new object[4] { App.ActiveCorp.RowID, this.mstrGenIssue_Branch, "WR", this.mstrGenIssue_Book };
            strSQLStr = "select fcCode from GLREF where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";

            if (mstrLastRunCode == "")
            {
                pobjSQLUtil.SetPara(pAPara);
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
                {
                    strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["fcCode"].ToString().Trim();
                    try
                    {
                        intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                    }
                    catch (Exception ex)
                    {
                        strErrorMsg = ex.Message.ToString();
                        intRunCode++;
                    }
                }
                else
                {
                    intRunCode++;
                    strLastRunCode = this.mstrLastRunCode;
                }

            }
            else
            {

                try
                {
                    intRunCode = Convert.ToInt64(this.mstrLastRunCode) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
                
            }

            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : 7);
            this.mstrGenIssue_Code = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            this.mstrLastRunCode = this.mstrGenIssue_Code;

            return true;
        }

        private bool pmSaveAllOrder(ref string ioErrorMsg)
        {
            ioErrorMsg = "";
            string strErrorMsg = "";
            this.mstrCurrQcCoor = "";
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].DefaultView;
            dv.Sort = "cCode";
            bool bllSucc = true;
            foreach (DataRow dtrRunCode in this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue1].Rows)
            {

                //this.pmRunCode_PR();
                if (dtrRunCode["cCode"].ToString().Trim() != string.Empty)
                {
                    if (this.pmChkDupCode(dtrRunCode["cCode"].ToString()))
                    {
                        ioErrorMsg = UIBase.GetAppUIText(new string[] { "เลขที่เอกสารซ้ำ : ", "Duplicate Document Code : " }) + dtrRunCode["cCode"].ToString().Trim();
                        MessageBox.Show(ioErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bllSucc = false;
                        break;
                    }
                    else
                    {
                        this.mstrCurrCode = dtrRunCode["cCode"].ToString();
                        this.mstrRefToRowID = this.mstrWOrderH;
                        this.mstrRefToMOrderOP = dtrRunCode["cWOrderOP"].ToString();

                        this.pmSaveGLRef(dtrRunCode["cCode"].ToString(), dtrRunCode["cCode"].ToString(), dtrRunCode["cFrWHouse"].ToString(), dtrRunCode["cToWHouse"].ToString(), dtrRunCode["cSect"].ToString(), dtrRunCode["cJob"].ToString());
                        //this.pmReplRecord_PR(dtrRunCode["cCoor"].ToString(), dtrRunCode["cCode"].ToString(), dtrRunCode["cRefNo"].ToString(), ref strErrorMsg);
                    }
                }
            }
            return bllSucc;
        }

        private bool pmChkDupCode(string inCode)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrGenIssue_Branch, "WR", this.mstrGenIssue_Book, inCode };
            strSQLStr = "select fcCode from OrderH where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                bllResult = true;
            }
            return bllResult;
        }

        private void pmSaveGLRef(string inCode, string inRefNo, string inFrWHouse, string inToWHouse, string inSect, string inJob)
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where 0=1", ref strErrorMsg);
            DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrHTable].NewRow();

            if (true)
            {
                bllIsNewRow = true;
                WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                this.mstrEditRowID = objConn.RunRowID(this.mstrHTable);
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateAp] = App.AppID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateBy] = App.FMAppUserID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Datetime] = objSQLHelper.GetDBServerDateTime();
                this.dtsDataEnv.Tables[this.mstrHTable].Rows.Add(dtrSaveInfo);
            }

            string strCode = inCode.TrimEnd();
            if (strCode.Length > 3)
                strCode = StringHelper.Left(strCode, strCode.Length - 3);

            string strDept = "";
            string strProj = "";

            objSQLHelper.SetPara(new object[] { inSect });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from SECT where fcSkid = ?", ref strErrorMsg))
            {
                strDept = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString();
            }

            objSQLHelper.SetPara(new object[] { inJob });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
            {
                strProj = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString();
            }

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[QMFStmoveHDInfo.Field.RowID] = this.mstrEditRowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.CorrectBy] = App.FMAppUserID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.LastUpd] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFStmoveHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BranchID] = this.mstrGenIssue_Branch;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Rftype] = this.mstrRfType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BookID] = this.mstrGenIssue_Book;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Step] = SysDef.gc_STEP_IGNORE;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Code] = inCode.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.RefNo] = inRefNo.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.Date] = this.mdttGenIssue_Date;
            dtrSaveInfo[QMFStmoveHDInfo.Field.FrWHouse] = inFrWHouse;
            dtrSaveInfo[QMFStmoveHDInfo.Field.ToWHouse] = inToWHouse;
            dtrSaveInfo[QMFStmoveHDInfo.Field.SectID] = inSect;
            dtrSaveInfo[QMFStmoveHDInfo.Field.DeptID] = strDept;
            dtrSaveInfo[QMFStmoveHDInfo.Field.JobID] = inJob;
            dtrSaveInfo[QMFStmoveHDInfo.Field.ProjID] = strProj;
            dtrSaveInfo[QMFStmoveHDInfo.Field.LUpdApp] = App.AppID;
            dtrSaveInfo["fcDataser"] = "";	//fcDataser
            dtrSaveInfo["fcEAfterR"] = "E";

            string gcTemStr01 = BizRule.SetMemData("", "Rem");
            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata] = gcTemStr02;
            if (DataSetHelper.HasField("fmMemData2", dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata2] = gcTemStr03;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata3] = gcTemStr04;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata4] = gcTemStr05;
            }

            if (DataSetHelper.HasField(QMFStmoveHDInfo.Field.Memdata5, dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata5] = gcTemStr06;
            }

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
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

                this.mstrRefToWOrderI = "";

                decimal decDiscAmtI = 0;

                mstrFrWHouse = inFrWHouse;
                mstrToWHouse = inToWHouse;

                this.pmSaveRefProd(this.mstrEditRowID, ref decDiscAmtI);

                this.pmUpdateStockForInsert();

                dtrSaveInfo["fnDiscAmtI"] = decDiscAmtI;

                this.pmSaveRefTo();

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
                bllIsCommit = true;

                //KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Insert, "", this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);

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
            }

        }

        private bool pmSaveRefProd(string ioErrorMsg, ref decimal ioDiscAmtI)
        {
            bool bllResult = true;
            string strRowID = "";
            string strOutRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Select("cCode = '" + this.mstrCurrCode + "' and nQty > 0");
            for (int i = 0; i < dtrSel.Length; i++)
            {

                DataRow dtrTemPd = dtrSel[i];

                bool bllIsNewRow = false;

                decimal decQty = 0;
                decimal decUmQty = 0;
                decimal decStQty = 0;
                string strWHouse = "";
                DataRow dtrRRefProd = null;

                #region "Save รายการฝั่ง Out"

                strRowID = App.mRunRowID("RefProd");
                bllIsNewRow = true;
                decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                decStQty = 0;

                //รายการฝั่ง Out
                dtrTemPd["cRowID"] = strRowID;
                strOutRowID = strRowID;
                DataRow dtrRefProd = null;
                this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, false, decQty, decUmQty, 0, 0, i);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                #endregion

                #region "Save รายการฝั่ง In"

                strRowID = App.mRunRowID("RefProd");
                bllIsNewRow = true;
                decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                decStQty = 0;

                //รายการฝั่ง IN
                dtrTemPd["cRowID2"] = strRowID;
                dtrRefProd = null;
                this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, true, decQty, decUmQty, 0, 0, i);

                strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                #endregion

                this.pmSaveRefDoc(dtrTemPd);

            }

            return true;
        }

        private bool pmReplRecordRefProd
            (
            bool inState, DataRow inTemPd, ref DataRow ioRefProd, string inRowID,
            bool inToWHouse, decimal inQty, decimal inUmQty, decimal inStQty, decimal inStUmQty, Int32 inRun
            )
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrRefProd;
            if (bllIsNewRec)
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].NewRow();
                dtrRefProd["fcSkid"] = inRowID;
                dtrRefProd["fcCreateAp"] = App.AppID;
            }
            else
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
            }

            string strWHouse = "";
            string strIOType = "";
            if (inToWHouse)
            {
                strWHouse = this.mstrToWHouse;
                strIOType = "I";
            }
            else
            {
                //strWHouse = this.txtFrQcWHouse.Tag.ToString();
                strWHouse = this.mstrFrWHouse;
                strIOType = "O";
            }

            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];

            //string strRemark1 = inTemPd["cRemark1"].ToString().TrimEnd();
            //string strRemark2 = inTemPd["cRemark2"].ToString().TrimEnd();
            //string strRemark3 = inTemPd["cRemark3"].ToString().TrimEnd();
            //string strRemark4 = inTemPd["cRemark4"].ToString().TrimEnd();
            //string strRemark5 = inTemPd["cRemark5"].ToString().TrimEnd();
            //string strRemark6 = inTemPd["cRemark6"].ToString().TrimEnd();
            //string strRemark7 = inTemPd["cRemark7"].ToString().TrimEnd();
            //string strRemark8 = inTemPd["cRemark8"].ToString().TrimEnd();
            //string strRemark9 = inTemPd["cRemark9"].ToString().TrimEnd();
            //string strRemark10 = inTemPd["cRemark10"].ToString().TrimEnd();

            dtrRefProd["fcCorp"] = App.ActiveCorp.RowID;
            dtrRefProd["fcBranch"] = this.mstrGenIssue_Branch;
            dtrRefProd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
            dtrRefProd["fcRefType"] = dtrGLRef["fcRefType"].ToString();
            dtrRefProd["fcRfType"] = dtrGLRef["fcRfType"].ToString();
            dtrRefProd["fcStat"] = dtrGLRef["fcStat"].ToString();
            dtrRefProd["fdDate"] = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;
            dtrRefProd["fcRefPdTyp"] = "P";
            dtrRefProd["fcProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrRefProd["fcRootSeq"] = "";
            dtrRefProd["fcShowComp"] = "";
            dtrRefProd["fcPformula"] = "";
            dtrRefProd["fcFormulas"] = "";
            dtrRefProd["fcProd"] = inTemPd["cProd"].ToString();
            //dtrRefProd["fmRemark"] = strRemark1;
            //dtrRefProd["fmRemark2"] = strRemark2;
            //dtrRefProd["fmRemark3"] = strRemark3;
            //dtrRefProd["fmRemark4"] = strRemark4;
            //dtrRefProd["fmRemark5"] = strRemark5;
            //dtrRefProd["fmRemark6"] = strRemark6;
            //dtrRefProd["fmRemark7"] = strRemark7;
            //dtrRefProd["fmRemark8"] = strRemark8;
            //dtrRefProd["fmRemark9"] = strRemark9;
            //dtrRefProd["fmRemark10"] = strRemark10;
            dtrRefProd["fcUm"] = inTemPd["cUOM"].ToString();
            dtrRefProd["fcLot"] = inTemPd["cLot"].ToString().TrimEnd();
            dtrRefProd["fcWhouse"] = strWHouse;
            dtrRefProd["fcIoType"] = strIOType;
            dtrRefProd["fcSect"] = dtrGLRef["fcSect"].ToString();
            dtrRefProd["fcDept"] = dtrGLRef["fcDept"].ToString();
            dtrRefProd["fcJob"] = dtrGLRef["fcJob"].ToString();
            dtrRefProd["fcProj"] = dtrGLRef["fcProj"].ToString();
            //dtrRefProd["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]);
            //dtrRefProd["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            //dtrRefProd["fnXRate"] = 1;
            //dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(inRun, 2);

            //if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            //{
            //    this.pmUpdateStock(inQty, inUmQty, inStQty, inStUmQty, ref dtrRefProd, inTemPd);
            //    string strWHLoca = "";
            //    if (!bllIsNewRec)
            //        this.pmRetStock(dtrRefProd, strWHLoca, true, false, "", ref strErrorMsg);
            //}

            dtrRefProd["fcLUpdApp"] = App.AppID;
            dtrRefProd["fcDataser"] = "";
            dtrRefProd["fcEAfterR"] = 'E';

            dtrRefProd["fnQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrRefProd["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrRefProd["fcUmStd"] = inTemPd["cUOMStd"].ToString();

            //dtrRefProd["fnStQty"] = Convert.ToDecimal(inTemPd["nStQty"]);
            //dtrRefProd["fcStUm"] = inTemPd["cUOMStd"].ToString();
            //dtrRefProd["fnStUmQty"] = Convert.ToDecimal(inTemPd["nStUOMQty"]);
            //dtrRefProd["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

            ioRefProd = dtrRefProd;
            return true;
        }

        private void pmSaveRefDoc(DataRow inTemPd)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != inTemPd["cXRefToRowID"].ToString().TrimEnd()
                && inTemPd["cXRefToRowID"].ToString().TrimEnd() != string.Empty)
            {
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString() };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != string.Empty
                && Convert.ToDecimal(inTemPd["nQty"]) != 0)
            {
                bool bllIsNewRow_RefDoc = false;
                DataRow dtrRefDoc = null;
                string strRowID = "";
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), this.mstrRefToRefType, inTemPd["cRefToHRowID"].ToString(), inTemPd["cRefToRowID"].ToString() };
                string strRefDoc = "select * from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?";
                if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefDoc_Stmove, MapTable.Table.RefDoc_Stmove, strRefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                {
                    strRowID = App.mRunRowID(MapTable.Table.RefDoc_Stmove);
                    bllIsNewRow_RefDoc = true;
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].NewRow();
                    dtrRefDoc["cRowID"] = strRowID;
                    //dtrRefDoc["cCreateAp"] = App.AppID;
                }
                else
                {
                    strRowID = inTemPd["cRefToRowID"].ToString();
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].Rows[0];
                }
                dtrRefDoc["cMastertyp"] = this.mstrRefType;
                dtrRefDoc["cMasterH"] = this.mstrEditRowID;
                dtrRefDoc["cMasterI"] = inTemPd["cRowID"].ToString();
                dtrRefDoc["cCorp"] = App.ActiveCorp.RowID;
                dtrRefDoc["cBranch"] = this.mstrGenIssue_Branch;
                dtrRefDoc["cPlant"] = this.mstrGenIssue_Plant;
                dtrRefDoc["cChildType"] = this.mstrRefToRefType;
                dtrRefDoc["cChildH"] = inTemPd["cRefToHRowID"].ToString();
                dtrRefDoc["cChildI"] = inTemPd["cRefToRowID"].ToString();
                dtrRefDoc["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
                dtrRefDoc["nUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);

                string strSQLUpdateStr_RefDoc = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefDoc, "cRowID", bllIsNewRow_RefDoc, ref strSQLUpdateStr_RefDoc, ref pAPara);
                this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr_RefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                //pobjSQLUtil.SetPara(pAPara);
                //pobjSQLUtil.SQLExec(strSQLUpdateStr_RefDoc, ref strErrorMsg);
            }
            else
            {
                //Delete Note Cut
                //pobjSQLUtil.SetPara(new object[] {this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString()});
                //pobjSQLUtil.SQLExec("delete from REFDOC_STMOVE where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", ref strErrorMsg);

                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString() };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
        }

        private bool pmSaveRefTo()
        {

            bool bllResult = true;
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;

            if (this.mstrRefToRowID != this.mstrOldRefToRowID)
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[4] { this.mstrRefType, this.mstrOldRefToRowID, this.mstrRefToRefType, this.mstrRefToRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? and cChildH = ?", ref strErrorMsg);
            }

            if (this.mstrRefToRowID != string.Empty)
            {
                bool bllIsNewRow_RefTo = false;
                string strRowID_RefTo = "";
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "REFDOC", "REFDOC", "select * from REFDOC where 0=1", ref strErrorMsg);
                DataRow dtrREFDOC = this.dtsDataEnv.Tables["REFDOC"].NewRow();

                if (pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID })
                    && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefTo", "REFDOC", "select cRowID from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
                {
                    strRowID_RefTo = App.mRunRowID("REFDOC");
                    bllIsNewRow_RefTo = true;
                }
                else
                {
                    strRowID_RefTo = this.dtsDataEnv.Tables["QChkRefTo"].Rows[0]["cRowID"].ToString();
                }

                dtrREFDOC["cRowID"] = strRowID_RefTo;
                dtrREFDOC["cCorp"] = App.ActiveCorp.RowID;
                dtrREFDOC["cBranch"] = this.mstrGenIssue_Branch;
                dtrREFDOC["cPlant"] = this.mstrGenIssue_Plant;
                dtrREFDOC["cMasterTyp"] = this.mstrRefType;
                dtrREFDOC["cMasterH"] = this.mstrEditRowID;
                dtrREFDOC["cChildType"] = this.mstrRefToRefType;
                dtrREFDOC["cChildH"] = this.mstrRefToRowID;
                dtrREFDOC["cChildI"] = this.mstrRefToMOrderOP;

                string strSQLUpdateStr_RefTo = "";
                DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
                pobjSQLUtil.SetPara(pAPara);
                bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);

            }
            else
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg);

            }

            //this.pmUpdateWOrderOPStep(this.mstrRefToRowID);
            return bllResult;
        }

        private bool pmUpdateStockForInsert()
        {
            string strErrorMsg = "";
            string strSQLUpdateStr = "";
            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
            DataRow dtrRefProd = null;
            bool llSucc = true;
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Select("cCode = '" + this.mstrCurrCode + "' and nQty > 0");
            for (int i = 0; i < dtrSel.Length; i++)
            {

                DataRow dtrTemPd = dtrSel[i];

                dtrRefProd = null;
                //รายการฝั่ง Out
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

                dtrRefProd = null;
                //รายการฝั่ง In
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

            }
            return llSucc;
        }

        private bool pmUpdateStock(decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, ref DataRow ioRefProd, DataRow inTemPd)
        {
            string strErrorMsg = "";
            bool bllSucc = false;
            decimal lnQty = tninQty;
            decimal lnUmQty = tninUmQty;
            decimal lnStQty = tninStQty;
            decimal lnStUmQty = tninStUmQty;
            decimal lnAvgCost = 0;
            decimal lnCostAmt = 0;

            DataRow dtrRefProd = ioRefProd;
            DataRow dtrTemPd = inTemPd;

            decimal lnUpdStockSign = (dtrRefProd["fcIoType"].ToString() == "I" ? 1 : -1);
            string lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);

            if (dtrRefProd["fcIoType"].ToString() == "I"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrToWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }
            else if (dtrRefProd["fcIoType"].ToString() == "O"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }

            decimal decOutStockQty = 0;
            decimal decOutWHLocaQty = 0;

            string strWHLoca = "";

            dtrRefProd["fnCostAmt"] = (Convert.IsDBNull(dtrRefProd["fnCostAmt"]) ? 0 : this.pmToDecimal(dtrRefProd["fnCostAmt"]));

            if (this.mstrFrWhType == SysDef.gc_WHOUSE_TYPE_NORMAL
                && this.mstrRefType != SysDef.gc_REFTYPE_TRANSFER)
            {
                if (dtrRefProd["fcIoType"].ToString() == "O")
                {
                    lnCostAmt = 0;

                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
lnUpdStockSign
                        , this.mstrGenIssue_Branch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
);

                    lnCostAmt = Math.Round(lnAvgCost * this.pmToDecimal(dtrRefProd["fnQty"]) * this.pmToDecimal(dtrRefProd["fnUmQty"]), 4);
                }
                else
                {
                    lnCostAmt = this.pmToDecimal(dtrTemPd["nCostAmt"]);
                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
                        lnUpdStockSign
                        , this.mstrGenIssue_Branch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
                        );

                    lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nCostAmt"]), 4);

                }
                if (bllSucc)
                {
                    dtrRefProd["fnPrice"] = (this.pmToDecimal(dtrRefProd["fnQty"]) != 0 ? Math.Round(lnCostAmt / this.pmToDecimal(dtrRefProd["fnQty"]), 4) : 0);
                }
            }
            else
            {
                decimal lnUpdCostAmt = Math.Round(this.pmToDecimal(dtrRefProd["fnPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4) - this.pmToDecimal(dtrRefProd["fnCostAmt"]);	//กรณีแก้ไข เพิ่ม/ลด จำนวน จะ Update เฉพาะจำนวนที่ เพิ่ม/ลด เท่านั้น เหมือน Qty
                this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                bllSucc = this.mStockAgent.UpdateStock
                    (
                #region "UpdateStock Parameter"
                      lnUpdStockSign
                    , this.mstrGenIssue_Branch
                    , dtrRefProd["fcProdType"].ToString()
                    , dtrRefProd["fcProd"].ToString()
                    , dtrRefProd["fcWhouse"].ToString()
                    , strWHLoca
                    , dtrRefProd["fcLot"].ToString()
                    , lcCtrlStock
                    , lnQty
                    , lnUmQty
                    , dtrTemPd["cUOM"].ToString()
                    , dtrTemPd["cQnUOM"].ToString()
                    , lnUpdCostAmt
                    , ref lnAvgCost
                    , lnStQty * lnStUmQty
                    , ref decOutStockQty
                    , ref decOutWHLocaQty
                    , false
                    , false
                    , ""
                    , dtrRefProd["fcIoType"].ToString() == "I"
                    , 0
                    , false
                    , false
                    , ""
                    , ref strErrorMsg
                #endregion
);

                lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4);
            }
            if (bllSucc)
            {
                if (dtrRefProd["fcIOType"].ToString() == "O")
                {
                    dtrTemPd["nCostAmt"] = lnCostAmt;
                }
                dtrRefProd["fnCostAmt"] = lnCostAmt;
                dtrRefProd["fnPriceKe"] = dtrRefProd["fnPrice"];
                dtrRefProd["fnXRate"] = 1;
            }

            return bllSucc;
        }

        private Decimal pmToDecimal(object inConvert)
        {
            Decimal decRetVal = 0;
            //decRetVal = (Convert.IsDBNull(inConvert) ? 0 : Convert.ToDecimal(inConvert));
            if (Convert.IsDBNull(inConvert))
            {
                decRetVal = 0;
            }
            else
            {
                decRetVal = Convert.ToDecimal(inConvert);
            }
            return decRetVal;
        }

        private void pmInsertRow(string inWOrderI, string inWOrderOP, string inProd, string inQcProd, string inPdType, string inUM, string inOPSeq, string inSeq, string inFrWHouse, string inFrQcWHouse, string inToWHouse, string inToQcWHouse, string inLot, decimal inQty, string inSect, string inJob)
        {

            DataRow dtbTemPd = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].NewRow();

            dtbTemPd["cRefToHRowID"] = this.mstrWOrderH;
            dtbTemPd["cRefToRowID"] = inWOrderI;
            dtbTemPd["cWOrderOP"] = inWOrderOP;
            dtbTemPd["cFrWHouse"] = inFrWHouse;
            dtbTemPd["cFrQcWHouse"] = inFrQcWHouse;
            dtbTemPd["cToWHouse"] = inToWHouse;
            dtbTemPd["cToQcWHouse"] = inToQcWHouse;
            dtbTemPd["cProd"] = inProd;
            dtbTemPd["cPdType"] = inPdType;
            dtbTemPd["cQcProd"] = inQcProd;
            dtbTemPd["nQty"] = inQty;
            dtbTemPd["cOPSeq"] = inOPSeq;
            dtbTemPd["cSeq"] = inSeq;
            dtbTemPd["cSect"] = inSect;
            dtbTemPd["cJob"] = inJob;
            dtbTemPd["cUOM"] = inUM;
            dtbTemPd["cUOMStd"] = inUM;
            dtbTemPd["cLot"] = inLot;

            this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Rows.Add(dtbTemPd);
        }

        private decimal pmLoadStockQty(string inProd)
        {
            decimal decStockBal = 0;
            this.pmCreateTemStock(inProd, ref decStockBal);
            return decStockBal;
        }

        private void pmCreateTemStock(string inProd, ref decimal ioStockBal)
        {

            decimal decRetVal = 0;
            decimal decSumQty = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFld = " REFPROD.FCPROD,REFPROD.FCWHOUSE,REFPROD.FCLOT,REFPROD.FCIOTYPE ";
            strFld += ",(select PD.FCCODE from PROD PD where PD.FCSKID = REFPROD.FCPROD) as QcProd ";
            strFld += ",(select WH.FCCODE from WHOUSE WH where WH.FCSKID = REFPROD.FCWHOUSE) as QcWHouse ";
            strFld += ",sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as fnSumQty ";

            string strSQLCmd = "select " + strFld + " from RefProd ";
            strSQLCmd += " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse";
            strSQLCmd += " where RefProd.fcProd = ? ";
            strSQLCmd += " and RefProd.fcBranch = ? ";
            strSQLCmd += " and RefProd.fcStat <> 'C' ";
            strSQLCmd += " and RefProd.fnQty <> 0 ";
            strSQLCmd += " and WHouse.fcType = ' ' ";
            strSQLCmd += (this.mstrWHouse_RM.Trim() != string.Empty ? " and WHOUSE.FCCODE in " + this.mstrWHouse_RM : "");
            strSQLCmd += " group by REFPROD.FCPROD,REFPROD.FCWHOUSE,REFPROD.FCLOT,REFPROD.FCIOTYPE ";
            strSQLCmd += " order by QCWHOUSE , REFPROD.FCLOT , REFPROD.FCIOTYPE ";

            string strQcProd = "";
            string strQcWHouse = "";
            string strWHouse = "";
            string strLot = "";

            string strKey = "";
            pobjSQLUtil.SetPara(new object[] { inProd, this.mstrGenIssue_Branch });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "RefProd", strSQLCmd, ref strErrorMsg);

            DataRow dtrLastRow = null;

            decRetVal = 0;
            if (this.dtsDataEnv.Tables["QRefProd"].Rows.Count > 0)
            {
                DataRow dtr1Row = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                strKey = dtr1Row["QcWHouse"].ToString() + dtr1Row["fcLot"].ToString();
                decimal decQty = (!Convert.IsDBNull(dtr1Row["fnSumQty"]) ? Convert.ToDecimal(dtr1Row["fnSumQty"]) : 0) * (dtr1Row["fcIOType"].ToString() == "I" ? 1 : -1);
                dtrLastRow = dtr1Row;
            }

            foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QRefProd"].Rows)
            {

                if (strKey != dtrRefProd["QcWHouse"].ToString() + dtrRefProd["fcLot"].ToString())
                {
                    strKey = dtrRefProd["QcWHouse"].ToString() + dtrRefProd["fcLot"].ToString();

                    strQcProd = dtrLastRow["QcProd"].ToString();
                    strQcWHouse = dtrLastRow["QcWHouse"].ToString();
                    strWHouse = dtrLastRow["fcWHouse"].ToString();
                    strLot = dtrLastRow["fcLot"].ToString();

                    string strSel = "cQcProd = '{0}' and cQcWHouse = '{1}' and cLot = '{2}' ";
                    strSel = String.Format(strSel, new object[] { strQcProd, strQcWHouse, strLot });
                    DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemStock].Select(strSel);
                    if (dtrSel.Length == 0)
                    {

                        DataRow dtrNewStock = this.dtsDataEnv.Tables[this.mstrTemStock].NewRow();
                        dtrNewStock["cQcProd"] = strQcProd;
                        dtrNewStock["cQcWHouse"] = strQcWHouse;
                        dtrNewStock["cWHouse"] = strWHouse;
                        dtrNewStock["cLot"] = strLot;
                        dtrNewStock["nQty"] = decRetVal;
                        dtrNewStock["nUseQty"] = 0;
                        dtrNewStock["nBalQty"] = decRetVal;
                        this.dtsDataEnv.Tables[this.mstrTemStock].Rows.Add(dtrNewStock);

                        dtrLastRow = dtrRefProd;
                        decRetVal = 0;
                        //dtrLastRow = null;
                    }
                }

                decimal decQty = (!Convert.IsDBNull(dtrRefProd["fnSumQty"]) ? Convert.ToDecimal(dtrRefProd["fnSumQty"]) : 0) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                decRetVal += decQty;
                decSumQty += decQty;

            }

            if (dtrLastRow != null)
            {
                strQcProd = dtrLastRow["QcProd"].ToString();
                strQcWHouse = dtrLastRow["QcWHouse"].ToString();
                strWHouse = dtrLastRow["fcWHouse"].ToString();
                strLot = dtrLastRow["fcLot"].ToString();
                string strSel = "cQcProd = '{0}' and cQcWHouse = '{1}' and cLot = '{2}' ";
                strSel = String.Format(strSel, new object[] { strQcProd, strQcWHouse, strLot });
                DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemStock].Select(strSel);
                if (dtrSel.Length == 0)
                {
                    DataRow dtrNewStock = this.dtsDataEnv.Tables[this.mstrTemStock].NewRow();
                    dtrNewStock["cQcProd"] = strQcProd;
                    dtrNewStock["cQcWHouse"] = strQcWHouse;
                    dtrNewStock["cWHouse"] = strWHouse;
                    dtrNewStock["cLot"] = strLot;
                    dtrNewStock["nQty"] = decRetVal;
                    dtrNewStock["nUseQty"] = 0;
                    dtrNewStock["nBalQty"] = decRetVal;
                    this.dtsDataEnv.Tables[this.mstrTemStock].Rows.Add(dtrNewStock);
                    dtrLastRow = null;
                }
            }

            ioStockBal = decSumQty;

        }


    }

}