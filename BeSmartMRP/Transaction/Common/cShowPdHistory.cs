
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

namespace BeSmartMRP.Transaction.Common
{

    public class cShowPdHistory
    {

        public static string xd_TemHistory_Coor = "TemHisCust";
        public static string xd_TemHistory_BakRev = "TemBackRev";
        public static string xd_TemHistory_WHBal = "TemWHBal";

        private string mstrSaleOrBuy = "S";

        public cShowPdHistory(string inSaleOrBuy)
        {
            this.mstrSaleOrBuy = inSaleOrBuy;
            this.pmCreateTem();
        }

        private DataSet dtsDataEnv = new DataSet();

        private string mstrBranchID = "";
        private string mstrProd = "";
        private string mstrCoor = "";
        private string mstrBegQcProd = "";
        private string mstrEndQcProd = "";
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        public string BranchID
        {
            get { return this.mstrBranchID; }
            set { this.mstrBranchID = value; }
        }

        private void pmCreateTem()
        {
            DataTable dtbTemHisCust = new DataTable(xd_TemHistory_Coor);
            dtbTemHisCust.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemHisCust.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemHisCust.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemHisCust.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemHisCust.Columns.Add("cDiscStr", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemHisCust.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            this.dtsDataEnv.Tables.Add(dtbTemHisCust);

            DataTable dtbTemBakRev = new DataTable(xd_TemHistory_BakRev);
            dtbTemBakRev.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemBakRev.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemBakRev.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemBakRev.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("dDueDate", System.Type.GetType("System.DateTime"));
            this.dtsDataEnv.Tables.Add(dtbTemBakRev);

            DataTable dtbTemWHBal = new DataTable(xd_TemHistory_WHBal);
            dtbTemWHBal.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemWHBal.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemWHBal.Columns.Add("nMinStock", System.Type.GetType("System.Decimal"));
            dtbTemWHBal.Columns.Add("nMaxStock", System.Type.GetType("System.Decimal"));
            this.dtsDataEnv.Tables.Add(dtbTemWHBal);

        }

        public void ShowProdHistory(string inBranch, string inProd)
        {
            if (this.mstrProd == inProd && MessageBox.Show("สินค้านี้เคยดูประวัติแล้ว\r\nต้องการประมวลผลใหม่หรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.No)
            {
                this.pmQueryProdHistory(false);
            }
            else
            {
                using (dlgHisOpt1 dlg = new dlgHisOpt1())
                {
                    dlg.IsGetProd = false;
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        this.mstrBranchID = inBranch;
                        this.mstrProd = inProd;
                        this.mdttBegDate = dlg.BegDate;
                        this.mdttEndDate = dlg.EndDate;
                        this.pmQueryProdHistory(true);
                    }
                }
            }

        }

        public void ShowCoorHistory(string inBranch, string inCoor)
        {
            using (dlgHisOpt1 dlg = new dlgHisOpt1())
            {
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    this.mstrBranchID = inBranch;
                    this.mstrCoor = inCoor;
                    this.mstrBegQcProd = dlg.BegQcProd;
                    this.mstrEndQcProd = dlg.EndQcProd;
                    this.mdttBegDate = dlg.BegDate;
                    this.mdttEndDate = dlg.EndDate;
                    this.pmQueryCoorHistory();
                }
            }
        }

        private void pmQueryProdHistory(bool isRequery)
        {
            if (isRequery)
            {
                WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                this.pmQueryProdHistory_Prod(pobjSQLUtil);
                this.pmQueryProdHistory_BakRev(pobjSQLUtil);
                this.pmQueryProdHistory_WHBal(pobjSQLUtil);
            }

            frmHistory_Prod ofrmHistory = new frmHistory_Prod(this.mstrSaleOrBuy);
            ofrmHistory.BindData(this.dtsDataEnv, xd_TemHistory_Coor, xd_TemHistory_BakRev, xd_TemHistory_WHBal);
            ofrmHistory.ShowDialog();

        }

        private void pmQueryProdHistory_Prod(WS.Data.Agents.cDBMSAgent inSQLUtil)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLUtil;

            if (this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Clear();
            }

            string strFld = "RefProd.fcCorp ,RefProd.fcCoor ,RefProd.fcBranch ,RefProd.fcGLRef ,RefProd.fcProd ,RefProd.fnPriceKe ,RefProd.fcDiscStr ,RefProd.fnQty ,RefProd.fdDate ,RefProd.fcRfType ,RefProd.fcUmStd ,RefProd.fnUmQty ,RefProd.fcLot ";
            strFld += ",GLRef.fcRefNo";
            strFld += ",Coor.fcCode as cQcCoor, Coor.fcName as cQnCoor";
            strFld += ",UM.fcName as cQnUM";

            string strSQLCmdRefProd = "select  " + strFld + " from RefProd ";
            strSQLCmdRefProd += " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            strSQLCmdRefProd += " left join Coor on Coor.fcSkid = RefProd.fcCoor ";
            strSQLCmdRefProd += " left join UM on UM.fcSkid = RefProd.fcUmStd ";
            strSQLCmdRefProd += " where RefProd.fcProd = ?";
            strSQLCmdRefProd += " and RefProd.fcBranch = ? and RefProd.fdDate Between ? and ? and RefProd.fcStat <> 'C' ";
            if (this.mstrSaleOrBuy == "S")
                strSQLCmdRefProd += " and RefProd.fcRfType in ('" + SysDef.gc_RFTYPE_INV_SELL + "', '" + SysDef.gc_RFTYPE_DR_SELL + "', '" + SysDef.gc_RFTYPE_CR_SELL + "')";
            else
                strSQLCmdRefProd += " and RefProd.fcRfType in ('" + SysDef.gc_RFTYPE_INV_BUY + "', '" + SysDef.gc_RFTYPE_DR_BUY + "', '" + SysDef.gc_RFTYPE_CR_BUY + "')";

            strSQLCmdRefProd += " order by RefProd.fdDate, GLRef.fcRefNo";

            pobjSQLUtil.SetPara(new object[] { this.mstrProd, this.mstrBranchID, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strSQLCmdRefProd, ref strErrorMsg);
            foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QRefProd"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_Coor].NewRow();
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                dtrTemHis["cRefNo"] = dtrRefProd["fcRefNo"].ToString();
                dtrTemHis["cQnCoor"] = dtrRefProd["cQnCoor"].ToString();
                dtrTemHis["cQnUM"] = dtrRefProd["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUMQty"]);
                dtrTemHis["nPrice"] = Convert.ToDecimal(dtrRefProd["fnPriceKe"]);
                dtrTemHis["cDiscStr"] = dtrRefProd["fcDiscStr"].ToString();
                dtrTemHis["cLot"] = dtrRefProd["fcLot"].ToString();
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Add(dtrTemHis);
            }

            strFld = "OrderI.fcCorp ,OrderI.fcCoor ,OrderI.fcBranch ,OrderI.fcOrderH ,OrderI.fcProd ,OrderI.fnPriceKe ,OrderI.fcDiscStr ,OrderI.fnQty ,OrderI.fdDate ,OrderI.fcUmStd ,OrderI.fnUmQty ,OrderI.fcLot ";
            strFld += ", OrderH.fcRefNo";
            strFld += ",Coor.fcCode as cQcCoor, Coor.fcName as cQnCoor";
            strFld += ",UM.fcName as cQnUM";

            string strSQLCmdOrder = "select  " + strFld + " from OrderI ";
            strSQLCmdOrder += " left join OrderH on OrderH.fcSkid = OrderI.fcOrderH ";
            strSQLCmdOrder += " left join Coor on Coor.fcSkid = OrderI.fcCoor ";
            strSQLCmdOrder += " left join UM on UM.fcSkid = OrderI.fcUmStd ";
            strSQLCmdOrder += " where OrderI.fcProd = ?";
            strSQLCmdOrder += " and OrderI.fcBranch = ? and OrderI.fdDate Between ? and ? and OrderI.fcStat <> 'C' ";
            if (this.mstrSaleOrBuy == "S")
                strSQLCmdOrder += " and OrderI.fcRefType in ('" + SysDef.gc_REFTYPE_S_ORDER + "', '" + SysDef.gc_REFTYPE_S_QUATATION + "')";
            else
                strSQLCmdOrder += " and OrderI.fcRefType in ('" + SysDef.gc_REFTYPE_P_ORDER + "', '" + SysDef.gc_REFTYPE_P_REQUEST + "')";
            strSQLCmdOrder += " order by OrderI.fdDate, OrderH.fcRefNo";

            pobjSQLUtil.SetPara(new object[] { this.mstrProd, this.mstrBranchID, this.mdttBegDate, this.mdttEndDate });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmdOrder, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_Coor].NewRow();
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrOrderI["fdDate"]);
                dtrTemHis["cRefNo"] = dtrOrderI["fcRefNo"].ToString();
                dtrTemHis["cQnCoor"] = dtrOrderI["cQnCoor"].ToString();
                dtrTemHis["cQnUM"] = dtrOrderI["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrOrderI["fnQty"]) * Convert.ToDecimal(dtrOrderI["fnUMQty"]);
                dtrTemHis["nPrice"] = Convert.ToDecimal(dtrOrderI["fnPriceKe"]);
                dtrTemHis["cDiscStr"] = dtrOrderI["fcDiscStr"].ToString();
                dtrTemHis["cLot"] = dtrOrderI["fcLot"].ToString();
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Add(dtrTemHis);
            }

        }

        private void pmQueryProdHistory_BakRev(WS.Data.Agents.cDBMSAgent inSQLUtil)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLUtil;

            if (this.dtsDataEnv.Tables[xd_TemHistory_BakRev].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemHistory_BakRev].Rows.Clear();
            }

            string strFld = " OrderI.fdDate ,OrderI.fnBackQty ,OrderI.fcLot , OrderI.fdDelivery ";
            strFld += ",OrderH.fcCode as QcOrder, OrderH.fdRecedate ";
            strFld += ",Coor.fcCode as cQcCoor, Coor.fcName as cQnCoor";
            strFld += ",UM.fcName as cQnUM";
            strFld += ",Book.fcCode as cQcBook";

            string strSQLCmd = "select " + strFld + " from OrderI ";
            strSQLCmd += " left join OrderH on OrderH.fcSkid = OrderI.fcOrderH";
            strSQLCmd += " left join Book on Book.fcSkid = OrderH.fcBook";
            strSQLCmd += " left join Coor on Coor.fcSkid = OrderI.fcCoor";
            strSQLCmd += " left join UM on UM.fcSkid = OrderI.fcUM";
            strSQLCmd += " where OrderI.fcCorp = ? ";
            strSQLCmd += " and OrderI.fcBranch = ? ";
            strSQLCmd += " and OrderI.fcRefType = ? ";
            strSQLCmd += " and OrderI.fcStat = ? ";
            strSQLCmd += " and OrderI.fcStep = ? ";
            strSQLCmd += " and OrderI.fcProd = ? ";
            strSQLCmd += " and OrderI.fnBackQty > 0 ";
            strSQLCmd += " order by OrderI.fdDate, OrderH.fcRefNo ";

            string strRefType = SysDef.gc_REFTYPE_S_ORDER;
            if (this.mstrSaleOrBuy == "S")
                strRefType = SysDef.gc_REFTYPE_S_ORDER;
            else
                strRefType = SysDef.gc_REFTYPE_P_ORDER;

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, strRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, this.mstrProd });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_BakRev].NewRow();
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrOrderI["fdDate"]);
                dtrTemHis["cRefNo"] = dtrOrderI["cQcBook"].ToString().TrimEnd() + "/" + dtrOrderI["QcOrder"].ToString();
                dtrTemHis["cQnCoor"] = dtrOrderI["cQnCoor"].ToString();
                dtrTemHis["cQnUM"] = dtrOrderI["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrOrderI["fnBackQty"]);
                dtrTemHis["cLot"] = dtrOrderI["fcLot"].ToString();

                if (!Convert.IsDBNull(dtrOrderI["fdDelivery"]))
                {
                    dtrTemHis["dDueDate"] = Convert.ToDateTime(dtrOrderI["fdDelivery"]);
                }
                else
                {
                    dtrTemHis["dDueDate"] = Convert.ToDateTime(dtrOrderI["fdReceDate"]);
                }

                this.dtsDataEnv.Tables[xd_TemHistory_BakRev].Rows.Add(dtrTemHis);
            }

        }

        private void pmQueryProdHistory_WHBal(WS.Data.Agents.cDBMSAgent inSQLUtil)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLUtil;

            if (this.dtsDataEnv.Tables[xd_TemHistory_WHBal].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemHistory_WHBal].Rows.Clear();
            }

            string strFld = "PdBranch.fnMinStock ,PdBranch.fnMaxStock ,PdBranch.fnQty, WHouse.fcCode as cQcWHouse, WHouse.fcName as cQnWHouse";

            string strSQLCmd = "select " + strFld + " from PdBranch ";
            strSQLCmd += " left join WHouse on WHouse.fcSkid = PdBranch.fcWHouse";
            strSQLCmd += " where PdBranch.fcProd = ? and PdBranch.fcBranch = ? ";

            string strQnUM = "";
            pobjSQLUtil.SetPara(new object[] { this.mstrProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "Prod", "select UM.fcName from Prod left join UM on UM.fcSkid = Prod.fcUM where Prod.fcSkid = ?", ref strErrorMsg))
            {
                strQnUM = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            pobjSQLUtil.SetPara(new object[] { this.mstrProd, this.mstrBranchID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdBranch", "PdBranch", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrPdBranch in this.dtsDataEnv.Tables["QPdBranch"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_WHBal].NewRow();
                dtrTemHis["cQcWHouse"] = dtrPdBranch["cQcWHouse"].ToString();
                dtrTemHis["cQnWHouse"] = dtrPdBranch["cQnWHouse"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrPdBranch["fnQty"]);
                dtrTemHis["nMinStock"] = Convert.ToDecimal(dtrPdBranch["fnMinStock"]);
                dtrTemHis["nMaxStock"] = Convert.ToDecimal(dtrPdBranch["fnMaxStock"]);
                dtrTemHis["cQnUM"] = strQnUM;

                this.dtsDataEnv.Tables[xd_TemHistory_WHBal].Rows.Add(dtrTemHis);
            }

        }

        private void pmQueryCoorHistory()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            if (this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Clear();
            }

            string strFld = "RefProd.fcCorp ,RefProd.fcCoor ,RefProd.fcBranch ,RefProd.fcGLRef ,RefProd.fcProd ,RefProd.fnPriceKe ,RefProd.fcDiscStr ,RefProd.fnQty ,RefProd.fdDate ,RefProd.fcRfType ,RefProd.fcUmStd ,RefProd.fnUmQty ,RefProd.fcLot ";
            strFld += ", GLRef.fcRefNo";
            strFld += ",Prod.fcCode as cQcProd, Prod.fcName as cQnProd";
            strFld += ",UM.fcName as cQnUM";

            string strSQLCmdRefProd = "select  " + strFld + " from RefProd ";
            strSQLCmdRefProd += " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            strSQLCmdRefProd += " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            strSQLCmdRefProd += " left join UM on UM.fcSkid = RefProd.fcUmStd ";
            strSQLCmdRefProd += " where RefProd.fcBranch = ?";
            strSQLCmdRefProd += " and RefProd.fcCoor = ? and RefProd.fdDate Between ? and ? and RefProd.fcStat <> 'C' ";
            if (this.mstrSaleOrBuy == "S")
                strSQLCmdRefProd += " and RefProd.fcRfType in ('" + SysDef.gc_RFTYPE_INV_SELL + "', '" + SysDef.gc_RFTYPE_DR_SELL + "', '" + SysDef.gc_RFTYPE_CR_SELL + "')";
            else
                strSQLCmdRefProd += " and RefProd.fcRfType in ('" + SysDef.gc_RFTYPE_INV_BUY + "', '" + SysDef.gc_RFTYPE_DR_BUY + "', '" + SysDef.gc_RFTYPE_CR_BUY + "')";

            strSQLCmdRefProd += " and Prod.fcCorp = ? and Prod.fcCode between ? and ?";
            strSQLCmdRefProd += " order by RefProd.fdDate, GLRef.fcRefNo";

            pobjSQLUtil.SetPara(new object[] { this.mstrBranchID, this.mstrCoor, this.mdttBegDate, this.mdttEndDate, App.ActiveCorp.RowID, this.mstrBegQcProd, this.mstrEndQcProd });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strSQLCmdRefProd, ref strErrorMsg);
            foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QRefProd"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_Coor].NewRow();
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                dtrTemHis["cRefNo"] = dtrRefProd["fcRefNo"].ToString();
                dtrTemHis["cQcProd"] = dtrRefProd["cQcProd"].ToString();
                dtrTemHis["cQnProd"] = dtrRefProd["cQnProd"].ToString();
                dtrTemHis["cQnUM"] = dtrRefProd["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUMQty"]);
                dtrTemHis["nPrice"] = Convert.ToDecimal(dtrRefProd["fnPriceKe"]);
                dtrTemHis["cDiscStr"] = dtrRefProd["fcDiscStr"].ToString();
                dtrTemHis["cLot"] = dtrRefProd["fcLot"].ToString();
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Add(dtrTemHis);
            }

            strFld = "OrderI.fcCorp ,OrderI.fcCoor ,OrderI.fcBranch ,OrderI.fcOrderH ,OrderI.fcProd ,OrderI.fnPriceKe ,OrderI.fcDiscStr ,OrderI.fnQty ,OrderI.fdDate ,OrderI.fcUmStd ,OrderI.fnUmQty ,OrderI.fcLot ";
            strFld += ", OrderH.fcRefNo";
            strFld += ",Prod.fcCode as cQcProd, Prod.fcName as cQnProd";
            strFld += ",UM.fcName as cQnUM";

            string strSQLCmdOrder = "select  " + strFld + " from OrderI ";
            strSQLCmdOrder += " left join OrderH on OrderH.fcSkid = OrderI.fcOrderH ";
            strSQLCmdOrder += " left join Prod on Prod.fcSkid = OrderI.fcProd ";
            strSQLCmdOrder += " left join UM on UM.fcSkid = OrderI.fcUmStd ";
            strSQLCmdOrder += " where OrderI.fcBranch = ?";
            strSQLCmdOrder += " and OrderI.fcCoor = ? and OrderI.fdDate Between ? and ? and OrderI.fcStat <> 'C' ";
            if (this.mstrSaleOrBuy == "S")
                strSQLCmdOrder += " and OrderI.fcRefType in ('" + SysDef.gc_REFTYPE_S_ORDER + "', '" + SysDef.gc_REFTYPE_S_QUATATION + "')";
            else
                strSQLCmdOrder += " and OrderI.fcRefType in ('" + SysDef.gc_REFTYPE_P_ORDER + "', '" + SysDef.gc_REFTYPE_P_REQUEST + "')";
            strSQLCmdOrder += " and Prod.fcCorp = ? and Prod.fcCode between ? and ?";
            strSQLCmdOrder += " order by OrderI.fdDate, OrderH.fcRefNo";

            pobjSQLUtil.SetPara(new object[] { this.mstrBranchID, this.mstrCoor, this.mdttBegDate, this.mdttEndDate, App.ActiveCorp.RowID, this.mstrBegQcProd, this.mstrEndQcProd });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmdOrder, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_Coor].NewRow();
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrOrderI["fdDate"]);
                dtrTemHis["cRefNo"] = dtrOrderI["fcRefNo"].ToString();
                dtrTemHis["cQcProd"] = dtrOrderI["cQcProd"].ToString();
                dtrTemHis["cQnProd"] = dtrOrderI["cQnProd"].ToString();
                dtrTemHis["cQnUM"] = dtrOrderI["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrOrderI["fnQty"]) * Convert.ToDecimal(dtrOrderI["fnUMQty"]);
                dtrTemHis["nPrice"] = Convert.ToDecimal(dtrOrderI["fnPriceKe"]);
                dtrTemHis["cDiscStr"] = dtrOrderI["fcDiscStr"].ToString();
                dtrTemHis["cLot"] = dtrOrderI["fcLot"].ToString();
                this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Add(dtrTemHis);
            }
            if (this.dtsDataEnv.Tables[xd_TemHistory_Coor].Rows.Count > 0)
            {
                frmHistory_Coor ofrmHistory = new frmHistory_Coor();
                ofrmHistory.BindData(this.dtsDataEnv, xd_TemHistory_Coor);
                ofrmHistory.ShowDialog();
            }

        }

    }
}
