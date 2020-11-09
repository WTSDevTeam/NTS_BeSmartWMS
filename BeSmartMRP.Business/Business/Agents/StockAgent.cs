using System;
using System.Text;
using System.Data;

using AppUtil;
using WS.Data;
using WS.Data.Agents;

using BeSmartMRP.Business.Component;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP.Business.Agents
{

    /// <summary>
    /// Summary description for StockAgent.
    /// </summary>
    public class StockAgent
    {

        public const string xd_Alias_TemPd = "TemPd";
        public const string xd_Alias_TemLot = "TemLot";

        public const string xdPHBigQtyPict = "#,###,###.####";			// 12,4

        public static string Config_StockType = "0";

        private string mstrConnectionString = "";
        private string mstrConnectionString2 = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;
        private DataSet dtsDataEnv = new DataSet("StockAgent");

        private string mstrCorp = "";
        private bool mbllIsCrSell = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        //private string mstrParentID = "";
        //private string mstrErrorMsg = "";

        public void SetSaveDBAgent(WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran)
        {
            this.mSaveDBAgent = inSaveDBAgent;
            this.mdbConn = inDbConn;
            this.mdbTran = inDbTran;
        }

        public void SetSaveDBAgent2(WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran)
        {
            this.mSaveDBAgent2 = inSaveDBAgent;
            this.mdbConn2 = inDbConn;
            this.mdbTran2 = inDbTran;
        }

        public StockAgent() { }

        public string CorpID
        {
            set { this.mstrCorp = value; }
        }

        public bool IsCrSell
        {
            get { return this.mbllIsCrSell; }
            set { this.mbllIsCrSell = value; }
        }

        public StockAgent(string inConnectionString, DBMSType inDataBaseReside)
        {
            this.mstrConnectionString = inConnectionString;
            this.mDataBaseReside = inDataBaseReside;
        }

        public void GetStockPdBranch(string inProd, string inBranch, string inWHouse, ref decimal ioQty, ref decimal ioMinStock, ref decimal ioMaxStock)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            string strFld = "PdBranch.fcWHouse, PdBranch.fnMinStock ,PdBranch.fnMaxStock ,PdBranch.fnQty, WHouse.fcCode as cQcWHouse, WHouse.fcName as cQnWHouse";

            string strSQLCmd = "select " + strFld + " from PdBranch ";
            strSQLCmd += " left join WHouse on WHouse.fcSkid = PdBranch.fcWHouse";
            strSQLCmd += " where PdBranch.fcProd = ? and PdBranch.fcBranch = ? and WHouse.fcType = ' ' ";

            ioQty = 0;
            ioMinStock = 0;
            ioMaxStock = 0;
            pobjSQLUtil.SetPara(new object[] { inProd, inBranch });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdBranch", "PdBranch", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrPdBranch in this.dtsDataEnv.Tables["QPdBranch"].Rows)
            {
                ioQty += Convert.ToDecimal(dtrPdBranch["fnQty"]);
            }

            strSQLCmd = "select * from PdBr where PdBr.fcProd = ? and PdBr.fcWHouse = ?";
            pobjSQLUtil.SetPara(new object[] { inProd, inWHouse });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdBr", "PdBr", strSQLCmd, ref strErrorMsg))
            {
                DataRow dtrPdBr = this.dtsDataEnv.Tables["QPdBr"].Rows[0];
                ioMinStock = Convert.ToDecimal(dtrPdBr["fnMinStock"]);
                ioMaxStock = Convert.ToDecimal(dtrPdBr["fnMaxStock"]);
            }

        }

        public DataTable GetLotTable(string inBranch, string inProd, string inWHouse, string inWHLoca)
        {

            string strErrorMsg = "";

            DataSet dtsDataEnv = new DataSet();
            DataTable dtbTemLot = new DataTable(xd_Alias_TemLot);
            dtbTemLot.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemLot.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemLot.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemLot.Columns.Add("nQty", System.Type.GetType("System.Decimal"));

            //decimal tStockQty = 0;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString2, this.mDataBaseReside);

            bool bllFoundStock = false;
            pobjSQLUtil.SetPara(new object[] { this.mstrCorp, inBranch, inWHouse, inProd });
            bllFoundStock = pobjSQLUtil.SQLExec(ref dtsDataEnv, "QStkUtil_Stock", "xaStock", "select * from Stock where fcCorp = ? and fcBranch = ? and fcWHouse = ? and fcProd = ? order by fcLot", ref strErrorMsg);

            if (bllFoundStock)
            {
                foreach (DataRow dtrStock in dtsDataEnv.Tables["QStkUtil_Stock"].Rows)
                {
                    decimal decQty = Convert.ToDecimal(dtrStock["fnQty"]);
                    if (decQty != 0)
                    {
                        DataRow dtrNewRow = dtbTemLot.NewRow();
                        dtrNewRow["cLot"] = dtrStock["fcLot"].ToString();
                        //dtrNewRow["cWHLoca"] = dtrStock["cWHLoca"].ToString();
                        //dtrNewRow["cQcWHLoca"] = dtrStock["cQcWHLoca"].ToString();
                        dtrNewRow["nQty"] = decQty;
                        dtbTemLot.Rows.Add(dtrNewRow);
                    }
                }
            }
            return dtbTemLot.Copy();
        }


        /// <summary>
        /// Agent สำหรับการเชื่อมต่อกับสต๊อค
        /// </summary>
        /// <param name="inConnectionString">Connection for Formula DB</param>
        /// <param name="inConnectionString2">Connection for ERPSys DB</param>
        /// <param name="inDataBaseReside">DBMS Type</param>
        public StockAgent(string inConnectionString, string inConnectionString2, DBMSType inDataBaseReside)
        {
            this.mstrConnectionString = inConnectionString;
            this.mstrConnectionString2 = inConnectionString2;
            this.mDataBaseReside = inDataBaseReside;
        }

        //		public void BindData(DataSet inDataEnv)
        //		{
        //			this.dtsDataEnv = inDataEnv;
        //		}

        public decimal GetStockBalance(string inBranch, string inProd, string inWHouse, string inWHLoca, string inLot)
        {

            string strErrorMsg = "";

            decimal tStockQty = 0;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);

            bool llFoundStock = false;
            pobjSQLUtil.SetPara(new object[] { this.mstrCorp, inBranch, inWHouse, inProd, inLot });
            llFoundStock = pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QStkUtil_Stock", "xaStock", "select * from Stock where fcCorp = ? and fcBranch = ? and fcWHouse = ? and fcProd = ? and fcLot = ? order by fcLot", ref strErrorMsg);

            DataRow dtrStock = null;
            if (!llFoundStock)
            {
                tStockQty = 0;
            }
            else
            {
                dtrStock = this.dtsDataEnv.Tables["QStkUtil_Stock"].Rows[0];
                tStockQty = Convert.ToDecimal(dtrStock["fnQty"]);
            }

            return tStockQty;
        }

        public decimal GetStockBalance(string inCorp, string inBranch, string inWHouse, string inWHLoca)
        {
            return 0;
        }

        public decimal GetStockBalance(string inCorp, string inBranch)
        {
            return 0;
        }

        public decimal GetStockBalance(string inCorp)
        {
            return 0;
        }

        public bool UpdateStock
            (
        #region "UpdateStock Parameter"
              decimal inUpdSign
            , string inBranch
            , string inProdType
            , string inProd
            , string inWHouse
            , string inWHLoca
            , string inLot

            , string inCtrlStock
            , decimal inQtyInUm
            , decimal inUmQty
            , string inUm
            , string inQnUm
            , decimal inCostAmt
            , ref decimal outAvgCost

            , decimal inStQtyStd
            , ref decimal outStockQty
            , ref decimal outWHLocaQty

            , bool inForRollBak
            , bool inIsAlert
            , string inAlertMsg
            , bool inForceCalCost
            , decimal inProdStdCost
            , bool inNoAlertCost
            , bool inNoAlertQty
            , string inQcPass
            , ref string outErrorMsg
        #endregion
            )
        {

            string strErrorMsg = "";
            bool llSucc = false;
            //string lcMsg = "";
            //string lcQnProd = "";
            //string lcQnWHouse = "";
            string lcProdType = "";
            decimal lnCostAmt = 0;
            decimal lnStQtyStd = 0;
            decimal lnBefoUpdStockQty = 0;
            bool llNoStockRecc = false;
            bool bllIsNewStock = false;
            bool bllIsNewPdBranch = false;

            DateTime ldDate = DBEnum.NullDate;
            bool llCutStockOk = false;
            DateTime ldMfgDate = DBEnum.NullDate;
            DateTime ldExpire = DBEnum.NullDate;
            decimal tAvgCost = 0;

            string gcStockMsg = "";
            string lcFoxSQLStr = "";
            string lcLot = inLot;

            //bool tlUsedGAG = false;

            llSucc = true;
            llCutStockOk = false;
            lnCostAmt = inCostAmt * inUpdSign;
            lnStQtyStd = inUpdSign * inStQtyStd;

            llCutStockOk = true;
            bool llFoundStock = false;

            lcFoxSQLStr = "select * from Stock where fcCorp = ? and fcBranch = ? and fcWhouse = ? and fcProd = ? and fcLot= ?";

            object[] pAPara = null;
            pAPara = new object[] { this.mstrCorp, inBranch, inWHouse, inProd, lcLot };
            llFoundStock = this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.Stock, MapTable.Table.Stock, lcFoxSQLStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

            DataRow dtrRStock = null;
            DataRow dtrRPdBranch = null;

            if (llFoundStock)
            {
                dtrRStock = this.dtsDataEnv.Tables[MapTable.Table.Stock].Rows[0];
                lnBefoUpdStockQty = Convert.ToDecimal(dtrRStock["fnQty"]);
                llNoStockRecc = false;
            }
            else
            {
                if (inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT || lnCostAmt != 0)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    string strRowID = objConn.RunRowID(MapTable.Table.Stock);
                    dtrRStock = this.dtsDataEnv.Tables[MapTable.Table.Stock].NewRow();

                    dtrRStock["fcSkid"] = strRowID;
                    dtrRStock["fcCorp"] = this.mstrCorp;
                    dtrRStock["fcBranch"] = inBranch;
                    dtrRStock["fdDate"] = DateTime.Now;
                    dtrRStock["fcWHouse"] = inWHouse;
                    dtrRStock["fcProd"] = inProd;
                    dtrRStock["fcLot"] = lcLot;
                    dtrRStock["fcEAfterR"] = "E";

                    dtrRStock["fnQty"] = 0;
                    dtrRStock["fnAvgCost"] = 0;
                    dtrRStock["fnStQtyStd"] = 0;

                    bllIsNewStock = true;
                }
                lnBefoUpdStockQty = 0;
                llNoStockRecc = true;
            }

            lcFoxSQLStr = "select * from PdBranch where fcCorp = ? and fcBranch = ? and fcWhouse = ? and fcProd = ?";
            pAPara = new object[] { this.mstrCorp, inBranch, inWHouse, inProd };
            if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdBranch, MapTable.Table.PdBranch, lcFoxSQLStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
            {
                if (inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT || lnCostAmt != 0)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    string strRowID = objConn.RunRowID(MapTable.Table.PdBranch);
                    dtrRPdBranch = this.dtsDataEnv.Tables[MapTable.Table.PdBranch].NewRow();

                    dtrRPdBranch["fcSkid"] = strRowID;
                    dtrRPdBranch["fcCorp"] = this.mstrCorp;
                    dtrRPdBranch["fcBranch"] = inBranch;
                    dtrRPdBranch["fcWHouse"] = inWHouse;
                    dtrRPdBranch["fcProd"] = inProd;
                    dtrRPdBranch["fcEAfterR"] = "E";

                    dtrRPdBranch["fnQty"] = 0;
                    dtrRPdBranch["fnAvgCost"] = 0;
                    dtrRPdBranch["fnStQtyStd"] = 0;
                    dtrRPdBranch["fnStdCost"] = 0;

                    bllIsNewPdBranch = true;
                }
            }
            else
            {
                dtrRPdBranch = this.dtsDataEnv.Tables[MapTable.Table.PdBranch].Rows[0];
                bllIsNewPdBranch = false;
            }

            if (inProdType != null)
            {
                lcProdType = inProdType;
            }
            else
            {
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QStkUtil_Prod", MapTable.Table.Product, "select * from Prod where fcSkid = ?", new object[] { inProd }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    lcProdType = this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fcType"].ToString();
                }
            }

            decimal tQtyStd = 0;

            bool tChkCost = (BusinessEnum.gc_PROD_TYPE_SET_GOODS + BusinessEnum.gc_PROD_TYPE_SET_RAW + BusinessEnum.gc_PROD_TYPE_SET_SEMI).IndexOf(lcProdType) > -1;
            bool tChkStock = (BusinessEnum.gc_PROD_TYPE_SET_GOODS + BusinessEnum.gc_PROD_TYPE_SET_RAW + BusinessEnum.gc_PROD_TYPE_SET_SEMI).IndexOf(lcProdType) > -1;

            if (tChkCost == false)
            {
                #region "กรณีสินค้าไม่ต้อง Update ราคาทุน"
                tQtyStd = 0;
                decimal decPrice = 0;
                BizRule.GetQtyPriceStd(inQtyInUm, 0, inUmQty, ref tQtyStd, ref decPrice);
                tQtyStd = inUpdSign * tQtyStd;
                outAvgCost = Math.Round((tQtyStd != 0 ? Math.Abs(lnCostAmt / tQtyStd) : 0), 4,MidpointRounding.AwayFromZero);

                if (inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT)
                {

                    dtrRStock["fnQty"] = Math.Round(Convert.ToDecimal(dtrRStock["fnQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRStock["fnStQtyStd"] = Convert.ToDecimal(dtrRStock["fnStQtyStd"]) + lnStQtyStd;
                    dtrRStock["fcLUpdApp"] = this.mSaveDBAgent.AppID;
                    dtrRStock["fcEAfterR"] = "E";

                    #region "Not Used"
                    // QcPass = 0  ยังไม่ผ่าน
                    // QcPass = 6  ผ่านบางส่วน ถือว่าผ่านหมด
                    //QcPass = ' ' ผ่านทั้งหมด
                    //if type( 'inQcPass' ) = 'C' .and. inQcPass $ '0' .and. type( 'dtrRStock["nUnQcQty' ) = 'N'
                    //	dtrRStock["nUnQcQty with round( dtrRStock["nUnQcQty + tQtyStd , 4 )
                    //endif
                    //if gocstStkUtilPara.plpropUpdUnDOQty
                    //	dtrRStock["nUnDOQty with round( dtrRStock["nUnDOQty - tQtyStd , 4 )
                    //endif
                    //if gocstStkUtilPara.plpropUpdSoAllocQty .and. type( 'dtrRStock["nSOAllocQ' ) = 'N'	&& 16/07/45 By Ser
                    //	dtrRStock["nSOAllocQ with round( dtrRStock["nSOAllocQ + tQtyStd  , 4 )
                    //endif
                    //= UpdUtil( "UpdDtAndLastUpd" ,'Stock' ,'RStock' ) && 03/01/48 By Khamrop							
                    //= xTablUpd( 1 , .T. , 'RStock' )
                    #endregion

                    dtrRPdBranch["fnQty"] = Math.Round(Convert.ToDecimal(dtrRPdBranch["fnQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRPdBranch["fnStQtyStd"] = Convert.ToDecimal(dtrRPdBranch["fnStQtyStd"]) + lnStQtyStd;
                    dtrRPdBranch["fcLUpdApp"] = this.mSaveDBAgent.AppID;
                    dtrRPdBranch["fcEAfterR"] = "E";
                }
                #endregion
            }
            else if (tChkCost == true && inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT)
            {
                #region "กรณีสินค้านับสต๊อค"
                tQtyStd = 0;
                decimal decPrice = 0;
                BizRule.GetQtyPriceStd(inQtyInUm, 0, inUmQty, ref tQtyStd, ref decPrice);
                tQtyStd = inUpdSign * tQtyStd;
                if (inCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE && tChkStock)
                {
                    if (lnBefoUpdStockQty + tQtyStd < 0)
                    {
                        if (tQtyStd > 0)	//เพิ่มยอด stock
                        {
                            llSucc = true;
                            llCutStockOk = true;
                        }
                        else if (this.mbllIsCrSell)	//ใบลดหนี้ขาย รับเข้าไม่ต้องเช็ค
                        {
                            llSucc = true;
                            llCutStockOk = true;
                        }
                        else
                        {
                            llSucc = false;
                            llCutStockOk = false;
                            if (inIsAlert)
                            {
                                //*	= ShowMsg( (inAlertMsg) , .T. , .F., .F., .F., .F., 'ALR' )
                                //	wait wind inAlertMsg time 3
                            }
                        }
                    }
                }

                if (llSucc)
                {
                    #region "Update Stock and PdBranch Process"
                    decimal decAvgCost = Convert.ToDecimal(dtrRStock["fnAvgCost"]);
                    decimal tStockAmt = 0;
                    decimal tCrQty = 0;
                    if (decAvgCost >= -999999999 && decAvgCost <= 9999999999)
                    {
                        tStockAmt = Convert.ToDecimal(dtrRStock["fnAvgCost"]) * Convert.ToDecimal(dtrRStock["fnQty"]);
                    }
                    else
                    {
                        if (BusinessEnum.gc_PROD_TYPE_SET_GOODS.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else if (BusinessEnum.gc_PROD_TYPE_SET_SEMI.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else
                        {
                            //tCostType = goCorp.pcpropSRawCost;
                        }
                        tStockAmt = 0;
                        tCrQty = 0;
                        if (tCrQty != 0)
                        {
                            dtrRStock["fnAvgCost"] = Math.Round(tStockAmt / tCrQty, 4, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dtrRStock["fnAvgCost"] = 0;
                        }
                    }

                    dtrRStock["fnQty"] = Math.Round(Convert.ToDecimal(dtrRStock["fnQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRStock["fnStQtyStd"] = Convert.ToDecimal(dtrRStock["fnStQtyStd"]) + lnStQtyStd;
                    dtrRStock["fcLUpdApp"] = this.mSaveDBAgent.AppID;
                    dtrRStock["fcEAfterR"] = "E";

                    #region "Not Used"
                    //if type( 'inQcPass' ) = 'C' .and. inQcPass $ '0' .and. type( 'RStock.fnUnQcQty' ) = 'N'
                    //	repl RStock.fnUnQcQty with round( RStock.fnUnQcQty + tQtyStd , 4 )
                    //endif
                    //if gocstStkUtilPara.plpropUpdUnDOQty
                    //	repl RStock.fnUnDOQty with round( RStock.fnUnDOQty - tQtyStd , 4 )
                    //endif
                    //if gocstStkUtilPara.plpropUpdSoAllocQty .and. type( 'RStock.fnSOAllocQ' ) = 'N'
                    //	repl RStock.fnSOAllocQ with round( RStock.fnSOAllocQ + tQtyStd , 4 )
                    //endif
                    #endregion

                    outAvgCost = Convert.ToDecimal(dtrRStock["fnAvgCost"]);

                    if (Convert.ToDecimal(dtrRStock["fnQty"]) > 0)
                    {
                        if (inForceCalCost || tQtyStd > 0)		// รับเข้าต้องคำนวณต้นทุนเฉลี่ยใหม่
                        {
                            outAvgCost = (tStockAmt + lnCostAmt) / Convert.ToDecimal(dtrRStock["fnQty"]);
                            if (outAvgCost < 0)
                            {
                                outAvgCost = 0;
                            }
                            dtrRStock["fnAvgCost"] = Math.Round(outAvgCost, 4, MidpointRounding.AwayFromZero);
                            dtrRStock["fcEAfterR"] = "E";
                        }
                        else
                        {
                            outAvgCost = Convert.ToDecimal(dtrRStock["fnAvgCost"]);
                        }
                    }
                    else if (Convert.ToDecimal(dtrRStock["fnQty"]) < 0)
                    {
                        outAvgCost = 1;
                        if (inForRollBak == false && tQtyStd < 0 && inNoAlertCost == false)	// จ่ายออกขณะคงคลังติดลบ
                        {
                            string strQnProd = this.pmGetProdName(inProd, this.mSaveDBAgent);
                            string strQnWHouse = this.pmGetWHouseName(inWHouse, this.mSaveDBAgent);

                            gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                                + "คลัง : " + strQnWHouse + "\r\n"
                                + "ล๊อต : " + inLot + "\r\n"
                                + "ยอดคงคลังติดลบ เครื่องจะคิดต้นทุนให้เป็น 1 ";
                        }
                    }
                    else if ((Convert.ToDecimal(dtrRStock["fnQty"]) == 0)
                        && (Convert.ToDecimal(dtrRStock["fnQty"]) == 0)
                        && (Convert.ToDecimal(dtrRStock["fnQty"]) == 0))
                    {
                        pAPara = new object[] { dtrRStock["fcSkid"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.Stock + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                    //Update Stock
                    string strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrRStock, "FCSKID", bllIsNewStock, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    //Update PdBranch
                    decAvgCost = Convert.ToDecimal(dtrRPdBranch["fnAvgCost"]);
                    decimal tPdBrAmt = 0;
                    tCrQty = 0;
                    if (decAvgCost >= -999999999 && decAvgCost <= 9999999999)
                    {
                        tPdBrAmt = Convert.ToDecimal(dtrRPdBranch["fnAvgCost"]) * Convert.ToDecimal(dtrRPdBranch["fnQty"]);
                    }
                    else
                    {
                        if (BusinessEnum.gc_PROD_TYPE_SET_GOODS.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else if (BusinessEnum.gc_PROD_TYPE_SET_SEMI.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else
                        {
                            //tCostType = goCorp.pcpropSRawCost;
                        }
                        tPdBrAmt = 0;
                        tCrQty = 0;
                        if (tCrQty != 0)
                        {
                            dtrRPdBranch["fnAvgCost"] = Math.Round(tPdBrAmt / tCrQty, 4, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dtrRPdBranch["fnAvgCost"] = 0;
                        }
                    }

                    dtrRPdBranch["fnQty"] = Math.Round(Convert.ToDecimal(dtrRPdBranch["fnQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRPdBranch["fnStQtyStd"] = Convert.ToDecimal(dtrRPdBranch["fnStQtyStd"]) + lnStQtyStd;
                    dtrRPdBranch["fcLUpdApp"] = this.mSaveDBAgent.AppID;
                    dtrRPdBranch["fcEAfterR"] = "E";

                    if (Convert.ToDecimal(dtrRPdBranch["fnQty"]) > 0)
                    {
                        if (inForceCalCost || tQtyStd > 0)		//รับเข้าต้องคำนวณต้นทุนเฉลี่ยใหม่
                        {
                            tAvgCost = (tPdBrAmt + lnCostAmt) / Convert.ToDecimal(dtrRPdBranch["fnQty"]);
                            dtrRPdBranch["fnAvgCost"] = Math.Round(tAvgCost, 4, MidpointRounding.AwayFromZero);
                            dtrRPdBranch["fcEAfterR"] = "E";
                        }
                    }
                    else if (Convert.ToDecimal(dtrRPdBranch["fnQty"]) <= 0)
                    {
                    }

                    //Update PdBranch
                    strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrRPdBranch, "FCSKID", bllIsNewPdBranch, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    if (tQtyStd < 0 && outAvgCost == 0)		// ตัด Stock
                    {
                        outAvgCost = 1;
                        if (inForRollBak == false && inNoAlertCost == false)
                        {
                            string strQnProd = this.pmGetProdName(inProd, this.mSaveDBAgent);
                            gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                                + " ต้นทุน = 0 เครื่องจะคิดต้นทุนให้เป็น 1 ";
                            if (tChkStock)
                            { }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region "กรณีสินค้าไม่นับสต๊อค"
                if (dtrRPdBranch != null && Convert.ToDecimal(dtrRPdBranch["fnStdCost"]) > 0)
                {
                    outAvgCost = Convert.ToDecimal(dtrRPdBranch["fnStdCost"]);
                }
                else if (inProdStdCost > 0)
                {
                    outAvgCost = inProdStdCost;
                }
                else if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QStkUtil_Prod", MapTable.Table.Product, "select * from Prod where fcSkid = ?", new object[] { inProd }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    outAvgCost = Convert.ToDecimal(this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fnStdCost"]);
                }
                #endregion
            }

            if (!llCutStockOk)
            {
                string strQnProd = this.pmGetProdName(inProd, this.mSaveDBAgent);
                string strQnWHouse = this.pmGetWHouseName(inWHouse, this.mSaveDBAgent);
                if (llNoStockRecc)
                {
                    if (inNoAlertQty == false)
                    {
                        gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                            + "คลัง : " + strQnWHouse + "\r\n"
                            + "ล๊อต : " + inLot + "\r\n"
                            + "ไม่มีในสต๊อค";
                    }
                    outStockQty = 0;
                }
                else
                {
                    string tQnUm = "";
                    if (inQnUm.TrimEnd() != string.Empty)
                    {
                        tQnUm = inQnUm;
                    }
                    else if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QStkUtil_Um", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", new object[] { inUm }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        tQnUm = this.dtsDataEnv.Tables["QStkUtil_Um"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal tQtyInUm = 0;
                    decimal decPriceInUm = 0;
                    bool tQtyInUmOk = BizRule.GetQtyPriceInUm(lnBefoUpdStockQty, 0, inUmQty, ref tQtyInUm, ref decPriceInUm);

                    if (inNoAlertQty == false)
                    {
                        gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                            + "คลัง : " + strQnWHouse + "\r\n"
                            + "ล๊อต : " + inLot + "\r\n"
                            + "ยอดในสต๊อคมีเพียง " + tQtyInUm.ToString(xdPHBigQtyPict) + " " + tQnUm + "\r\n"
                            + "ไม่พอให้ตัดยอด โปรดระบุจำนวนใหม่ หรืออนุญาตให้ยอดสต๊อคติดลบได้";
                    }
                    outStockQty = tQtyInUm;
                }

            }

            //llSucc = llSucc && this.UpdateStockOnWHLoca
            //    (
            //    inUpdSign
            //    , inBranch
            //    , inProdType
            //    , inProd
            //    , inWHouse
            //    , inWHLoca
            //    , inLot
            //    , inCtrlStock
            //    , inQtyInUm
            //    , inUmQty
            //    , inUm
            //    , inQnUm
            //    , inStQtyStd
            //    , ref outWHLocaQty
            //    , inNoAlertQty
            //    , ref outErrorMsg);

            outErrorMsg = gcStockMsg;

            return llSucc;
        }


        public bool UpdateStockOnWHLoca
            (
        #region "UpdateStock Parameter"
decimal inUpdSign

            , string inBranch
            , string inProdType
            , string inProd
            , string inWHouse
            , string inWHLoca
            , string inLot

            , string inCtrlStock
            , decimal inQtyInUm
            , decimal inUmQty
            , string inUm
            , string inQnUm
            , decimal inStQtyStd
            , ref decimal outStockQty
            , bool inNoAlertQty
            , ref string outErrorMsg
        #endregion
)
        {

            string strErrorMsg = "";
            bool llSucc = false;
            //string lcMsg = "";
            //string lcQnProd = "";
            //string lcQnWHouse = "";
            string lcProdType = "";
            //decimal lnCostAmt = 0;
            decimal lnStQtyStd = 0;
            decimal lnBefoUpdStockQty = 0;
            bool llNoStockRecc = false;
            bool bllIsNewStock = false;
            //bool bllIsNewPdBranch = false;

            bool llCutStockOk = false;
            DateTime ldDate = DBEnum.NullDate;

            string gcStockMsg = "";
            string lcFoxSQLStr = "";
            string lcLot = inLot;

            llSucc = true;
            //bool tlUsedGAG = false;
            llCutStockOk = false;
            lnStQtyStd = inUpdSign * inStQtyStd;

            llCutStockOk = true;
            bool llFoundStock = false;

            lcFoxSQLStr = "select * from xaStock where cCorp = ? and cBranch = ? and cWhouse = ? and cWHLoca = ? and cProd = ? and cLot= ?";

            object[] pAPara = null;
            pAPara = new object[] { this.mstrCorp, inBranch, inWHouse, inWHLoca, inProd, lcLot };
            llFoundStock = this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.xaStock, MapTable.Table.xaStock, lcFoxSQLStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

            DataRow dtrRStock = null;

            if (llFoundStock)
            {
                dtrRStock = this.dtsDataEnv.Tables[MapTable.Table.xaStock].Rows[0];
                lnBefoUpdStockQty = Convert.ToDecimal(dtrRStock["nQty"]);
                llNoStockRecc = false;
            }
            else
            {
                if (inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT)
                {
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    string strRowID = objConn.RunRowID(MapTable.Table.xaStock);
                    dtrRStock = this.dtsDataEnv.Tables[MapTable.Table.xaStock].NewRow();

                    dtrRStock["cRowID"] = strRowID;
                    dtrRStock["cCorp"] = this.mstrCorp;
                    dtrRStock["cBranch"] = inBranch;
                    dtrRStock["dDate"] = DateTime.Now;
                    dtrRStock["cWHouse"] = inWHouse;
                    dtrRStock["cWHLoca"] = inWHLoca;
                    dtrRStock["cProd"] = inProd;
                    dtrRStock["cLot"] = lcLot;
                    dtrRStock["nQty"] = 0;
                    dtrRStock["nAvgCost"] = 0;
                    dtrRStock["nStQtyStd"] = 0;

                    bllIsNewStock = true;
                }
                lnBefoUpdStockQty = 0;
                llNoStockRecc = true;
            }

            if (inProdType != null)
            {
                lcProdType = inProdType;
            }
            else
            {
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QStkUtil_Prod", MapTable.Table.Product, "select * from Prod where fcSkid = ?", new object[] { inProd }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    lcProdType = this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fcType"].ToString();
                }
            }

            decimal tQtyStd = 0;

            bool tChkCost = (BusinessEnum.gc_PROD_TYPE_SET_GOODS + BusinessEnum.gc_PROD_TYPE_SET_RAW + BusinessEnum.gc_PROD_TYPE_SET_SEMI).IndexOf(lcProdType) > -1;
            bool tChkStock = (BusinessEnum.gc_PROD_TYPE_SET_GOODS + BusinessEnum.gc_PROD_TYPE_SET_RAW + BusinessEnum.gc_PROD_TYPE_SET_SEMI).IndexOf(lcProdType) > -1;

            if (tChkCost == false)
            {
                #region "กรณีสินค้าไม่ต้อง Update ราคาทุน"
                tQtyStd = 0;
                decimal decPrice = 0;
                BizRule.GetQtyPriceStd(inQtyInUm, 0, inUmQty, ref tQtyStd, ref decPrice);
                tQtyStd = inUpdSign * tQtyStd;

                if (inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT)
                {

                    dtrRStock["nQty"] = Math.Round(Convert.ToDecimal(dtrRStock["nQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRStock["nStQtyStd"] = Convert.ToDecimal(dtrRStock["nStQtyStd"]) + lnStQtyStd;

                }
                #endregion
            }
            else if (tChkCost == true && inCtrlStock != BusinessEnum.gc_PROD_CTRL_STOCK_NO_COUNT)
            {
                #region "กรณีสินค้านับสต๊อค"
                tQtyStd = 0;
                decimal decPrice = 0;
                BizRule.GetQtyPriceStd(inQtyInUm, 0, inUmQty, ref tQtyStd, ref decPrice);
                tQtyStd = inUpdSign * tQtyStd;
                if (inCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE && tChkStock)
                {
                    if (lnBefoUpdStockQty + tQtyStd < 0)
                    {
                        if (tQtyStd > 0)	//เพิ่มยอด stock
                        {
                            llSucc = true;
                            llCutStockOk = true;
                        }
                        else if (this.mbllIsCrSell)	//ใบลดหนี้ขาย รับเข้าไม่ต้องเช็ค
                        {
                            llSucc = true;
                            llCutStockOk = true;
                        }
                        else
                        {
                            llSucc = false;
                            llCutStockOk = false;
                        }
                    }
                }

                if (llSucc)
                {
                    #region "Update xaStock"
                    decimal decAvgCost = Convert.ToDecimal(dtrRStock["nAvgCost"]);
                    decimal tStockAmt = 0;
                    decimal tCrQty = 0;
                    if (decAvgCost >= -999999999 && decAvgCost <= 9999999999)
                    {
                        tStockAmt = Convert.ToDecimal(dtrRStock["nAvgCost"]) * Convert.ToDecimal(dtrRStock["nQty"]);
                    }
                    else
                    {
                        if (BusinessEnum.gc_PROD_TYPE_SET_GOODS.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else if (BusinessEnum.gc_PROD_TYPE_SET_SEMI.IndexOf(lcProdType) > -1)
                        {
                            //tCostType = goCorp.pcpropSGoodsCost;
                        }
                        else
                        {
                            //tCostType = goCorp.pcpropSRawCost;
                        }
                        tStockAmt = 0;
                        tCrQty = 0;
                        if (tCrQty != 0)
                        {
                            dtrRStock["nAvgCost"] = Math.Round(tStockAmt / tCrQty, 4, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dtrRStock["fnAvgCost"] = 0;
                        }
                    }

                    dtrRStock["nQty"] = Math.Round(Convert.ToDecimal(dtrRStock["nQty"]) + tQtyStd, 4, MidpointRounding.AwayFromZero);
                    dtrRStock["nStQtyStd"] = Convert.ToDecimal(dtrRStock["nStQtyStd"]) + lnStQtyStd;

                    if (Convert.ToDecimal(dtrRStock["nQty"]) == 0)
                    {
                        pAPara = new object[] { dtrRStock["cRowID"].ToString() };
                        this.mSaveDBAgent2.BatchSQLExec("delete from " + MapTable.Table.xaStock + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                    }

                    //Update Stock
                    string strSQLUpdateStr = "";
                    DataSetHelper.GenUpdateSQLString(dtrRStock, "CROWID", bllIsNewStock, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    #endregion
                }
                #endregion
            }

            if (!llCutStockOk)
            {
                string strQnProd = this.pmGetProdName(inProd, this.mSaveDBAgent);
                string strQnWHouse = this.pmGetWHouseName(inWHouse, this.mSaveDBAgent);
                string strQnWHLoca = this.pmGetWHouseName(inWHLoca, this.mSaveDBAgent2);
                if (llNoStockRecc)
                {
                    if (inNoAlertQty == false)
                    {
                        gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                            + "คลัง : " + strQnWHouse + "\r\n"
                            + "ที่เก็บ : " + strQnWHLoca + "\r\n"
                            + "ล๊อต : " + inLot + "\r\n"
                            + "ไม่มีในสต๊อค";
                    }
                    outStockQty = 0;
                }
                else
                {
                    string tQnUm = "";
                    if (inQnUm.TrimEnd() != string.Empty)
                    {
                        tQnUm = inQnUm;
                    }
                    else if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QStkUtil_Um", MapTable.Table.UOM, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", new object[] { inUm }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        tQnUm = this.dtsDataEnv.Tables["QStkUtil_Um"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal tQtyInUm = 0;
                    decimal decPriceInUm = 0;
                    bool tQtyInUmOk = BizRule.GetQtyPriceInUm(lnBefoUpdStockQty, 0, inUmQty, ref tQtyInUm, ref decPriceInUm);

                    if (inNoAlertQty == false)
                    {
                        gcStockMsg = gcStockMsg + strQnProd + "\r\n"
                            + "คลัง : " + strQnWHouse + "\r\n"
                            + "ที่เก็บ : " + strQnWHLoca + "\r\n"
                            + "ล๊อต : " + inLot + "\r\n"
                            + "ยอดในสต๊อคมีเพียง " + tQtyInUm.ToString(xdPHBigQtyPict) + " " + tQnUm + "\r\n"
                            + "ไม่พอให้ตัดยอด โปรดระบุจำนวนใหม่ หรืออนุญาตให้ยอดสต๊อคติดลบได้";
                    }

                    outStockQty = tQtyInUm;
                }
            }
            outErrorMsg = gcStockMsg;
            return llSucc;
        }

        public bool TestUpdateStock
            (
        #region "Parameter"
decimal inUpdSign,

            string inBranch,
            string inProdType,
            string inProd,
            string inWHouse,
            string inWHLoca,
            string inLot,

            string inCtrlStock,
            decimal inQtyInUm,
            decimal inUmQty,
            decimal inOQtyInUm,
            decimal inOUmQty,
            string inQnUm,
            ref decimal outMinQty,
            ref bool outQtySucc,

            string inStCtrlStock,
            decimal inStQtyInUm,
            decimal inStUmQty,
            decimal inOStQtyInUm,
            decimal inOStUmQty,
            string inStQnUm,
            ref decimal outStMinQty,
            ref bool outStQtySucc,

            bool inSetError,
            bool inIsSaleType,
            bool inShortErr,
            ref string outErrorMsg
        #endregion
)
        {

            decimal tStockQty = 0;
            decimal tUnQcQty = 0;
            bool llNoStockRecc = true;
            decimal tStockStQty = 0;
            decimal tSOAllocQty = 0;
            bool llApprove = false;
            bool tUTSucc = true;

            outQtySucc = true;
            outStQtySucc = true;

            string tMsg = "";
            string tMsgStr = "";
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(this.mstrConnectionString2, this.mDataBaseReside);

            //pobjSQLUtil2.SetPara(new object[] { this.mstrCorp, inBranch, inWHouse, inWHLoca, inProd, inLot });
            //bool llFoundStock = pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QStkUtil_Stock", "xaStock", "select * from xaStock where cCorp = ? and cBranch = ? and cWHouse = ? and cWHLoca = ? and cProd = ? and cLot = ?", ref strErrorMsg);
            pobjSQLUtil.SetPara(new object[] { this.mstrCorp, inBranch, inWHouse, inProd, inLot });
            bool llFoundStock = pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QStkUtil_Stock", "Stock", "select * from Stock where fcCorp = ? and fcBranch = ? and fcWHouse = ? and fcProd = ? and fcLot = ?", ref strErrorMsg);

            string strSQLStr = "select ";
            strSQLStr += " sum(ORDERI.FNBACKQTY/ORDERI.FNUMQTY) as BACKQTY ";
            strSQLStr += " from ORDERI ";
            strSQLStr += " left join ORDERH on ORDERH.FCSKID = ORDERI.FCORDERH ";
            strSQLStr += " left join PROD on PROD.FCSKID = ORDERI.FCPROD ";
            strSQLStr += " where ";
            strSQLStr += " ORDERH.FCCORP = ? ";
            strSQLStr += " and ORDERH.FCBRANCH = ? ";
            strSQLStr += " and ORDERH.FCREFTYPE = 'SO' ";
            strSQLStr += " and ORDERI.FCWHOUSE = ? ";
            strSQLStr += " and ORDERI.FCPROD = ? ";
            strSQLStr += " and ORDERI.FCLOT = ? ";
            strSQLStr += " and ORDERI.FCSTEP = '1' ";
            strSQLStr += " and ORDERI.FCSTAT <> 'C' ";
            strSQLStr += " and ORDERI.FNBACKQTY > 0  ";
            strSQLStr += " group by PROD.FCCODE ";

            
            //pobjSQLUtil2.SetPara(new object[] { this.mstrCorp, inBranch, inWHouse, inProd, inLot });
            //bool llFoundStockSO = pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QStkUtil_Stock2", "Stock", strSQLStr, ref strErrorMsg);
            DataRow dtrStock = null; DataRow dtrStock2 = null;
            if (!llFoundStock)
            {
                tStockQty = 0;
                tUnQcQty = 0;
                llNoStockRecc = true;
                tStockStQty = 0;
                tSOAllocQty = 0;
            }
            else
            {
                dtrStock = this.dtsDataEnv.Tables["QStkUtil_Stock"].Rows[0];
                tStockQty = Convert.ToDecimal(dtrStock["fnQty"]);

                //if (this.dtsDataEnv.Tables["QStkUtil_Stock2"].Rows.Count > 0)
                //{
                //    dtrStock2 = this.dtsDataEnv.Tables["QStkUtil_Stock2"].Rows[0];

                //    if (Config_StockType == "2")
                //    {
                //        if (!Convert.IsDBNull(dtrStock2["BackQty"]))
                //            tSOAllocQty = Convert.ToDecimal(dtrStock2["BackQty"]);
                //    }
                //    else
                //    {
                //        tSOAllocQty = 0;
                //    }
                //}

            }

            decimal tQtyStd = inUpdSign * inQtyInUm * inUmQty;
            decimal tOQtyStd = inUpdSign * BizRule.GetQtyInUm2(inOQtyInUm, inOUmQty, inUmQty) * inUmQty;
            //decimal tMinQty = tStockQty - tOQtyStd;
            decimal tMinQty = tStockQty - tSOAllocQty - tOQtyStd;

            decimal decPriceInUm = 0;
            bool tQtyInUmOk = BizRule.GetQtyPriceInUm(tMinQty, 0, inUmQty, ref outMinQty, ref decPriceInUm);
            decimal tStQtyStd = inUpdSign * inStQtyInUm * inStUmQty;
            decimal tOStQtyStd = inUpdSign * BizRule.GetQtyInUm2(inOStQtyInUm, inOStUmQty, inStUmQty) * inStUmQty;
            decimal tStMinQty = tStockStQty - tOStQtyStd;
            bool tStQtyInUmOk = BizRule.GetQtyPriceInUm(tStMinQty, 0, inStUmQty, ref outStMinQty, ref decPriceInUm);

            string lcProdType = "";
            if (inProdType != null)
            {
                lcProdType = inProdType;
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { inProd });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QStkUtil_Prod", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
                {
                    lcProdType = this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fcType"].ToString();
                }
            }

            bool tChkStock = (BusinessEnum.gc_PROD_TYPE_SET_GOODS + BusinessEnum.gc_PROD_TYPE_SET_RAW + BusinessEnum.gc_PROD_TYPE_SET_SEMI).IndexOf(lcProdType) > -1;
            //if (Config_StockType == "0")
            //{
            //    tChkStock = false;
            //}

            if (this.mbllIsCrSell)
            {
            }
            else if (inQtyInUm != 0
                && inCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE
                && tStockQty + tQtyStd - tOQtyStd - tSOAllocQty < 0
                //&& tStockQty + tQtyStd - tOQtyStd < 0
                && tChkStock)
            {
                tUTSucc = false;
                outQtySucc = false;

                decimal lnMinQty = tMinQty;
                if (lnMinQty >= 0)
                {
                    tMsgStr = "สต๊อคมี";
                }
                else
                {
                    tMsgStr = "ต้องไม่น้อยกว่า";
                }

                if (inShortErr)
                {
                    tMsg = tMsgStr + outMinQty.ToString(xdPHBigQtyPict);
                }
                else
                {
                    string strQnProd = this.pmGetProdName(inProd, pobjSQLUtil);
                    string strQnWHouse = this.pmGetWHouseName(inWHouse, pobjSQLUtil);
                    //string strQnWHLoca = this.pmGetWHLocaName(inWHLoca, pobjSQLUtil2);

                    tMsg = strQnProd + "\r\n"
                        + "คลัง : " + strQnWHouse + "\r\n"
                        //+ "ที่เก็บ : " + strQnWHLoca + "\r\n"
                        + "ล๊อต : " + inLot + "\r\n"
                        //+ "SO ค้างส่ง : " + tSOAllocQty.ToString(xdPHBigQtyPict) + "\r\n"
                        + tMsgStr + " " + lnMinQty.ToString(xdPHBigQtyPict) + " " + inQnUm;

                }
                BizRule.GetQtyPriceInUm(Math.Abs(tMinQty), 0, inUmQty, ref outMinQty, ref decPriceInUm);
                if (outMinQty > inQtyInUm)
                {
                    outMinQty = inQtyInUm;
                }

            }
            else if (inStQtyInUm != 0
                && inStCtrlStock == BusinessEnum.gc_PROD_CTRL_STOCK_NOT_NEGATIVE
                && tStockStQty + tStQtyStd - tOStQtyStd < 0
                && tChkStock)
            {
                outStQtySucc = false;

                string tMsgStr2 = "จำนวนในระบบหน่วยคุมสต๊อค\r\n";
                if (tStMinQty >= 0)
                {
                    tMsgStr = "สต๊อคมี";
                }
                else
                {
                    tMsgStr = "ต้องไม่น้อยกว่า";
                }
                if (inShortErr)
                {
                    tMsg = tMsgStr2 + tMsgStr + outStMinQty.ToString(xdPHBigQtyPict);
                }
                else
                {
                    string strQnProd = this.pmGetProdName(inProd, pobjSQLUtil);
                    string strQnWHouse = this.pmGetWHouseName(inWHouse, pobjSQLUtil);
                    string strQnWHLoca = this.pmGetWHLocaName(inWHLoca, pobjSQLUtil2);

                    tMsg = tMsgStr2 + strQnProd + "\r\n"
                        + "คลัง : " + strQnWHouse + "\r\n"
                        //+ "ที่เก็บ : " + strQnWHLoca + "\r\n"
                        + "ล๊อต : " + inLot + "\r\n"
                        //+ "SO ค้างส่ง : " + tSOAllocQty.ToString(xdPHBigQtyPict) + "\r\n"
                        + tMsgStr + " " + outStMinQty.ToString(xdPHBigQtyPict) + " " + inStQnUm;
                }
            }

            outErrorMsg = tMsg;
            tUTSucc = (outQtySucc && outStQtySucc) || llApprove;
            return tUTSucc;
        }

        private string pmGetProdName(string inProd, WS.Data.Agents.cDBMSAgent inSQLHelper)
        {
            string strErrorMsg = "";
            string strQcProd = "";
            string strQnProd = "";
            inSQLHelper.SetPara(new object[] { inProd });
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QStkUtil_Prod", "PROD", "select fcCode, fcName from Prod where fcSkid = ?", ref strErrorMsg))
            {
                strQcProd = this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fcCode"].ToString().TrimEnd();
                strQnProd = this.dtsDataEnv.Tables["QStkUtil_Prod"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            return "สินค้า : " + strQcProd + "[ " + strQnProd + "]";
        }

        private string pmGetWHouseName(string inWHouse, WS.Data.Agents.cDBMSAgent inSQLHelper)
        {
            string strQnWHouse = "";
            string strErrorMsg = "";
            inSQLHelper.SetPara(new object[] { inWHouse });
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QStkUtil_WHouse", "WHOUSE", "select fcCode, fcName from WHouse where fcSkid = ?", ref strErrorMsg))
            {
                strQnWHouse = this.dtsDataEnv.Tables["QStkUtil_WHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            return strQnWHouse;
        }

        private string pmGetWHLocaName(string inWHLoca, WS.Data.Agents.cDBMSAgent inSQLHelper)
        {
            string strQnWHLoca = "";
            string strErrorMsg = "";
            inSQLHelper.SetPara(new object[] { inWHLoca });
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QStkUtil_WHLoca", "WHLOCA", "select cName from WHLoca where cRowID = ?", ref strErrorMsg))
            {
                strQnWHLoca = this.dtsDataEnv.Tables["QStkUtil_WHLoca"].Rows[0]["cName"].ToString().TrimEnd();
            }
            return strQnWHLoca;
        }

    }

}
