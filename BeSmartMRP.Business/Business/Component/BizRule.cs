
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WS.Data;
using WS.Data.Agents;
using AppUtil;
using AppUtil.SecureHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Entity.Info;

namespace BeSmartMRP.Business.Component
{

    public class BizRule
    {
        public BizRule() { }

        public static decimal ToDecimal(object inValue)
        {
            decimal decRetVal = 0;
            if (!Convert.IsDBNull(inValue))
            {
                decRetVal = Convert.ToDecimal(inValue);
            }
            return decRetVal;
        }

        public static string GetVatSeqPrefix(DateTime inDate)
        {
            return inDate.Year.ToString("0000") + inDate.Month.ToString("00");
        }

        public static decimal GetChequeAmt(WS.Data.Agents.cDBMSAgent inSQLHelper, string inGetType, string inCoor)
        {

            decimal decPayAmt = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            DataSet dtsDataEnv = new DataSet();

            string strCriteria1 = "";
            string strStep = "";
            string strStat = "";
            switch (inGetType)
            {
                // Type 1 : เช็คในมือระบุใบเสร็จ
                case "1":
                    strStep = " ";
                    strStat = " ";
                    strCriteria1 = " and FCSKID in (select FCPAYMENT from BILPAY where FCCOOR = ?) ";
                    pobjSQLUtil.SetPara(new object[] { inCoor, strStep, strStat, inCoor });
                    break;
                // Type 2 : เช็คในมือไม่ระบุใบเสร็จ
                case "2":
                    strStep = " ";
                    strStat = " ";
                    strCriteria1 = " and FCSKID not in (select FCPAYMENT from BILPAY where FCCOOR = ?) ";
                    pobjSQLUtil.SetPara(new object[] { inCoor, strStep, strStat, inCoor });
                    break;
                // Type 3 : เช็คยังไม่ผ่าน
                case "3":
                    strStep = "B";
                    strStat = " ";
                    pobjSQLUtil.SetPara(new object[] { inCoor, strStep, strStat });
                    break;
                // Type 4 : เช็คคืน
                case "4":
                    strStep = "B";
                    strStat = "R";
                    pobjSQLUtil.SetPara(new object[] { inCoor, strStep, strStat });
                    break;
            }

            string strSQLStr = "select sum(FNAMT) as fnAmt from PAYMENT ";
            strSQLStr += " where FCCOOR = ? ";
            strSQLStr += " and FCPAYTYPE = 'HC' ";
            strSQLStr += " and FCSTEP = ? ";
            strSQLStr += " and FCSTAT = ? ";
            strSQLStr += strCriteria1;

            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QPayment", "PAYMENT", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrQPayment = dtsDataEnv.Tables["QPayment"].Rows[0];
                if (!Convert.IsDBNull(dtrQPayment["fnAmt"]))
                {
                    decPayAmt = Convert.ToDecimal(dtrQPayment["fnAmt"]);
                }
            }

            return decPayAmt;
        }

        public static bool GetPrice(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , ref decimal ioPrice
            , ref decimal ioUmQty
            , ref string ioDiscStr
            , string inCorp
            , string inProd
            , string inCoor
            , string inPPolicy
            , string inPContrac
            , ref decimal inQty
            , ref decimal inUmQty
            , ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            DataSet dtsDataEnv = new DataSet();

            decimal tPrice = 0;
            decimal tUmQty1 = 0;
            string tDiscStr = "";

            pobjSQLUtil.SetPara(new object[] { inCoor });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "GetPrice_Coor", "COOR", "select * from Coor where fcSkid = ? ", ref strErrorMsg))
            {
                DataRow dtrQCoor = dtsDataEnv.Tables["GetPrice_Coor"].Rows[0];
                if (dtrQCoor["fcPolicyPr"].ToString().Trim() != string.Empty)
                {
                    pmChk1Price(inSQLHelper, BusinessEnum.gc_PRICE_POLICY_SALE, "P", ref tPrice, ref tUmQty1, ref tDiscStr, inCorp, inProd, "        ", dtrQCoor["fcPolicyPr"].ToString(), "", ref inQty, ref inUmQty, ref ioErrorMsg);
                }
                if (dtrQCoor["fcPolicyDi"].ToString().Trim() != string.Empty)
                {
                    decimal tPriceX = 0;
                    pmChk1Price(inSQLHelper, BusinessEnum.gc_PRICE_POLICY_SALE, "D", ref tPriceX, ref tUmQty1, ref tDiscStr, inCorp, inProd, "        ", dtrQCoor["fcPolicyDi"].ToString(), "", ref inQty, ref inUmQty, ref ioErrorMsg);
                }

            }

            ioPrice = tPrice;
            ioUmQty = tUmQty1;
            ioDiscStr = tDiscStr;

            return true;
        }

        public static bool pmChk1Price(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inSystem
            , string inChkType
            , ref decimal ioPrice
            , ref decimal ioUmQty
            , ref string ioDiscStr
            , string inCorp
            , string inProd
            , string inCoor
            , string inPPolicy
            , string inPContrac
            , ref decimal inQty
            , ref decimal inUmQty
            , ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            DataSet dtsDataEnv = new DataSet();

            bool llSuccPpolicy = true;
            string strSQLStr = "select * from PPolicy where fcSkid = ? and fcStat = 'W' ";
            pobjSQLUtil.SetPara(new object[] { inPPolicy });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QPolicy", "PPOLICY", strSQLStr, ref strErrorMsg))
            {
                llSuccPpolicy = false;
            }

            bool tChkPrice = (inChkType == "P");
            bool tChkDisc = (inChkType == "D");
            bool tPriceOk = false;
            bool tDiscOk = false;

            strSQLStr = "select * from PRICE where FCCORP = ? and FCSYSTEM = ? and FCPCONTRAC = ? and FCPPOLICY = ? and FCCOOR = ? and FCPROD = ? and FCLEVEL = '' order by FCLEVEL , FCPRICENO , FCUM";
            pobjSQLUtil.SetPara(new object[] { inCorp, inSystem, inPContrac, inPPolicy, inCoor, inProd });
            if (llSuccPpolicy && pobjSQLUtil.SQLExec(ref dtsDataEnv, "GetPrice_Price", "PPOLICY", strSQLStr, ref strErrorMsg))
            {
                decimal tStdQty = inQty * inUmQty;
                bool tHasFree = false;
                foreach (DataRow dtrGetPrice in dtsDataEnv.Tables["GetPrice_Price"].Rows)
                {
                    if ((Convert.ToDecimal(dtrGetPrice["fnUmQty"]) * Convert.ToDecimal(dtrGetPrice["fnLvQty"])) <= tStdQty)
                    {
                        if (tChkPrice)
                        {
                            ioPrice = Convert.ToDecimal(dtrGetPrice["fnAmt"]);
                            ioUmQty = (Convert.ToDecimal(dtrGetPrice["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(dtrGetPrice["fnUmQty"]));
                            tPriceOk = true;
                        }

                        if (tChkDisc)
                        {
                            ioDiscStr = dtrGetPrice["fcDiscStr"].ToString();
                            tDiscOk = true;
                            pmRefUtil(ref ioDiscStr, inQty, Convert.ToDecimal(dtrGetPrice["fnLvQty"]));
                        }

                    }
                }
            }
            return tPriceOk || tDiscOk;
        }

        private static void pmRefUtil(ref string ioDiscStr, decimal inQty, decimal inLvQty)
        {
            decimal decDiscStr = 0;
            if (("%".IndexOf(ioDiscStr) > -1 || "@".IndexOf(ioDiscStr) > -1) && inQty != 0)
            {
                try
                {
                    decDiscStr = Convert.ToDecimal(inQty) * (inLvQty == 0 ? inQty : Convert.ToDecimal(inQty / inLvQty));
                    ioDiscStr = (decDiscStr == 0 ? "" : (Math.Round(decDiscStr, 2, MidpointRounding.AwayFromZero).ToString("00000000.00")));
                }
                catch
                {
                    ioDiscStr = "";
                }
            }

        }

        public static bool ValidCreditLimit(
            string inType
            , WS.Data.Agents.cDBMSAgent inSQLHelper
            , object[] inPara
            , string inCorp
            , string inCoor
            , string inContactName
            , decimal inDocAmt
            , ref decimal ioCredLim
            , ref decimal ioDebtAmt
            , ref decimal ioBackOrdAmt
            , ref decimal ioOverCredAmt
            , ref string ioErrorMsg)
        {

            //ยอดหนี้รวม = ยอดหนี้ Invoice + SOค้างส่ง

            string strErrorMsg = "";

            DataSet dtsDataEnv = new DataSet();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            decimal decCoorBal = 0;
            decimal decCoorCredLim = 0;

            //Sum ยอดหนี้ปัจจุบัน
            //string strSQLStr = "select fcSkid, fcCorp, fcBranch, fcCoorType, fcCoor, fnAmt from CoorBal ";
            string strSQLStr = "select sum(fnAmt) as fnAmt from CoorBal ";
            strSQLStr += " where CoorBal.fcCorp = ? and CoorBal.fcBranch = ? and CoorBal.fcCoorType = ? ";

            if (inType == "G")
            {
                strSQLStr += " and CoorBal.fcCoor in (select fcSkid from COOR where COOR.FCCONTACTN = ? and COOR.FCCONTACTN <> '') ";
            }
            else
            {
                strSQLStr += " and CoorBal.fcCoor = ? ";
            }

            pobjSQLUtil.SetPara(inPara);
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QCoorBal", "COORBAL", strSQLStr, ref strErrorMsg))
            {
                if (!Convert.IsDBNull(dtsDataEnv.Tables["QCoorBal"].Rows[0]["fnAmt"]))
                {
                    decCoorBal = Convert.ToDecimal(dtsDataEnv.Tables["QCoorBal"].Rows[0]["fnAmt"]);
                }
            }

            //Sum ยอดวงเงินเครดิตจากลูกค้า
            string strSQLText = "";
            if (inType == "G")
            {
                pobjSQLUtil.SetPara(new object[] { inCorp, inContactName });
                strSQLStr = " select sum(FNCREDLIM) as FNCREDLIM from COOR where fcCorp = ? and fcIsCust = 'Y' and FCCONTACTN = ? and FCCONTACTN <> '' ";
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { inCoor });
                strSQLStr = " select sum(FNCREDLIM) as FNCREDLIM from COOR where FCSKID = ? ";
            }

            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QCoorCred", "COOR", strSQLStr, ref strErrorMsg))
            {
                if (!Convert.IsDBNull(dtsDataEnv.Tables["QCoorCred"].Rows[0]["FNCREDLIM"]))
                {
                    decCoorCredLim = Convert.ToDecimal(dtsDataEnv.Tables["QCoorCred"].Rows[0]["FNCREDLIM"]);
                }
            }

            string strPrefix = "";
            if (inType == "G")
            {
                strPrefix = inContactName;
            }
            else
            {
                strPrefix = inCoor;
            }

            decimal decBakOrdAmt = pmSumBakOrdAmt(inType, inSQLHelper, new object[] { inCorp, inPara[1], "SO", strPrefix }, ref strSQLStr);

            decBakOrdAmt = decBakOrdAmt + inDocAmt;

            bool bllIsOverCredit = ((decCoorBal + decBakOrdAmt) > decCoorCredLim);
            decimal decNetDebt = decCoorBal + decBakOrdAmt;
            decimal decOverAmt = 0;
            if (bllIsOverCredit)
                decOverAmt = (decCoorCredLim - decNetDebt) * -1;

            ioCredLim = decCoorCredLim;
            ioDebtAmt = decCoorBal;
            ioBackOrdAmt = decBakOrdAmt;
            ioOverCredAmt = decOverAmt;

            return true;
        }

        private static decimal pmSumBakOrdAmt(string inType, WS.Data.Agents.cDBMSAgent inSQLHelper, object[] inPara, ref string ioErrorMsg)
        {

            decimal decOrdAmt = 0;
            decimal decInvAmt = 0;
            string strErrorMsg = "";

            string strSQLStr = "";
            strSQLStr = "select distinct  ";
            strSQLStr += "ORDERH.FNAMT+ORDERH.FNVATAMT-ORDERH.FNDISCAMTK  as OrdAmt ";
            strSQLStr += ", GLREF.FNAMT+GLREF.FNVATAMT-GLREF.FNDISCAMTK as InvAmt ";
            strSQLStr += " from ORDERH ";
            strSQLStr += " left join NOTECUT on ORDERH.FCSKID = NOTECUT.FCCHILDH and NOTECUT.FCCHILDTYP = 'SO' ";
            strSQLStr += " left join GLREF on GLREF.FCSKID = NOTECUT.FCMASTERH and GLREF.FCSTAT <> 'C' ";
            strSQLStr += " left join COOR on COOR.FCSKID = ORDERH.FCCOOR ";
            strSQLStr += " where ";
            strSQLStr += " ORDERH .FCCORP = ? ";
            strSQLStr += " and ORDERH .FCBRANCH = ? ";
            strSQLStr += " and ORDERH .FCREFTYPE = ? ";
            strSQLStr += " and ORDERH.FCSTEP <> 'P'  ";
            strSQLStr += " and ORDERH.FCSTEP <> 'L'  ";
            strSQLStr += " and ORDERH.FCSTAT <> 'C' ";

            if (inType == "G")
            {
                strSQLStr += " and COOR.FCCONTACTN = ? and COOR.FCCONTACTN <> '' ";
            }
            else
            {
                strSQLStr += " and COOR.FCSKID = ? ";
            }

            DataSet dtsTem = new DataSet();

            inSQLHelper.SetPara(inPara);
            if (inSQLHelper.SQLExec(ref dtsTem, "QSumOrd", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrSum in dtsTem.Tables["QSumOrd"].Rows)
                {
                    decOrdAmt += (Convert.IsDBNull(dtrSum["OrdAmt"]) ? 0 : Convert.ToDecimal(dtrSum["OrdAmt"]));
                    decInvAmt += (Convert.IsDBNull(dtrSum["InvAmt"]) ? 0 : Convert.ToDecimal(dtrSum["InvAmt"]));
                }
            }
            return decOrdAmt - decInvAmt;

        }

        public static string RunVatSeq(WS.Data.Agents.cDBMSAgent inSQLHelper, object[] inPara, string inPrefix, ref string ioErrorMsg)
        {
            string strRunVal = "";
            string strErrorMsg = "";
            string strSQLStr = "";

            int intMaxLength = 11;
            string strMax = AppUtil.StringHelper.Replicate("9", intMaxLength - inPrefix.Length);
            long intRunCode = 0;
            long lngMaxValue = Convert.ToInt64(strMax);

            DataSet dtsTem = new DataSet();

            string strPrefix = (inPrefix != string.Empty ? " and fcVatSeq like '" + inPrefix + "%' " : "");
            strSQLStr = "select fcVatSeq from GLRef where fcCorp = ? and fcBranch = ? and fcVatType = ? " + strPrefix + " and fcVatSeq < ':' order by fcVatSeq desc";
            inSQLHelper.SetPara(inPara);
            if (inSQLHelper.SQLExec(ref dtsTem, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {

                string strLastRunCode = dtsTem.Tables["QRunCode"].Rows[0]["fcVatSeq"].ToString().Trim();
                strLastRunCode = strLastRunCode.Substring(inPrefix.Length, intMaxLength - inPrefix.Length);
                try
                {
                    intRunCode = Convert.ToInt64(strLastRunCode) + 1;

                    if (intRunCode > lngMaxValue)
                    {
                        ioErrorMsg = "ลำดับที่เต็ม";
                    }

                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
                strRunVal = inPrefix + intRunCode.ToString(AppUtil.StringHelper.Replicate("0", strMax.Length));
            }
            else
            {
                intRunCode = 1;
                strRunVal = inPrefix + intRunCode.ToString(AppUtil.StringHelper.Replicate("0", strMax.Length));
            }

            return strRunVal;
        }

        public static bool ChkDupVatSeq(WS.Data.Agents.cDBMSAgent inSQLHelper, object[] inPara, ref string ioRefNo)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            DataSet dtsTem = new DataSet();

            strSQLStr = "select fcRefNo, fcCode from GLRef where fcCorp = ? and fcBranch = ? and fcVatType = ? and fcVatSeq = ?";
            inSQLHelper.SetPara(inPara);
            if (inSQLHelper.SQLExec(ref dtsTem, "QDupVatSeq", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                ioRefNo = dtsTem.Tables["QDupVatSeq"].Rows[0]["fcRefNo"].ToString().TrimEnd();
                //MessageBox.Show("เลขที่เอกสารซ้ำ", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bllResult = true;
            }
            return bllResult;
        }

        /// <summary>
        /// Get step ซื้อล่าสุดของ serial นี้
        /// </summary>
        /// <param name="inSQLHelper"></param>
        /// <param name="inPdSer"></param>
        /// <param name="inForRecalChkDelRefPdX3"></param>
        /// <returns></returns>
        public static string GetPdSerPStep(WS.Data.Agents.cDBMSAgent inSQLHelper, System.Data.IDbConnection inDbConn, System.Data.IDbTransaction inDbTran, string inPdSer, bool inForRecalChkDelRefPdX3)
        {
            string strErrorMsg = "";

            string xdRefPInv = DocumentType.BI.ToString() + "," + DocumentType.BQ.ToString() + "," + DocumentType.BR.ToString() + "," + DocumentType.BU.ToString() + "," + DocumentType.BV + "," + DocumentType.BY.ToString() + ",";
            string xdRefPDrNote = DocumentType.BD.ToString() + "," + DocumentType.BX.ToString() + "," + DocumentType.BB.ToString() + "," + DocumentType.BZ.ToString() + ",";
            string xdRefPCrNote = DocumentType.BC.ToString() + "," + DocumentType.BA.ToString() + "," + DocumentType.BM.ToString() + "," + DocumentType.BN.ToString() + ",";

            string strRetPStep = BusinessEnum.gc_PDSER_PSTEP_FREE;
            DataSet dtsTem = new DataSet();

            //inSQLHelper.SetPara(new object[] {inPdSer});
            //if (inSQLHelper.SQLExec(ref dtsTem, "QRefPdX3", MapTable.Table.RefPdX3, "select * from "+MapTable.Table.RefPdX3+" where fcPdSer = ? " , ref strErrorMsg))
            if (inSQLHelper.BatchSQLExec(ref dtsTem, "QRefPdX3", MapTable.Table.RefPdX3, "select * from " + MapTable.Table.RefPdX3 + " where fcPdSer = ? ", new object[] { inPdSer }, ref strErrorMsg, inDbConn, inDbTran))
            {
                foreach (DataRow dtrRefPdX3 in dtsTem.Tables["QRefPdX3"].Rows)
                {
                    if (inForRecalChkDelRefPdX3)
                    {
                    }
                    else
                    {
                        int intCmp = -1;
                        string strRefType = dtrRefPdX3["fcRefType"].ToString();
                        if (DocumentType.PO.ToString().IndexOf(strRefType) > -1)
                        {
                            //strRetPStep = max( strRetPStep , gc_PDSER_PSTEP_ORDER );
                            //strRetPStep = BusinessEnum.gc_PDSER_PSTEP_ORDER;
                            intCmp = BusinessEnum.gc_PDSER_PSTEP_ORDER.CompareTo(strRetPStep);
                            strRetPStep = (intCmp > -1 ? BusinessEnum.gc_PDSER_PSTEP_ORDER : strRetPStep);
                        }
                        else if ((xdRefPInv + xdRefPDrNote + DocumentType.TR.ToString() + ",").IndexOf(strRefType) > -1)
                        {
                            //strRetPStep = max( strRetPStep , gc_PDSER_PSTEP_INV );
                            //strRetPStep = BusinessEnum.gc_PDSER_PSTEP_INV;
                            intCmp = BusinessEnum.gc_PDSER_PSTEP_INV.CompareTo(strRetPStep);
                            strRetPStep = (intCmp > -1 ? BusinessEnum.gc_PDSER_PSTEP_INV : strRetPStep);
                        }
                        else if ((xdRefPCrNote).IndexOf(strRefType) > -1)
                        {
                            //strRetPStep = max( strRetPStep , gc_PDSER_PSTEP_RETURN );
                            //strRetPStep = BusinessEnum.gc_PDSER_PSTEP_RETURN;
                            intCmp = BusinessEnum.gc_PDSER_PSTEP_RETURN.CompareTo(strRetPStep);
                            strRetPStep = (intCmp > -1 ? BusinessEnum.gc_PDSER_PSTEP_RETURN : strRetPStep);
                        }
                    }

                }
            }
            return strRetPStep;
        }

        /// <summary>
        /// Get step ขายล่าสุดของ serial นี้
        /// </summary>
        /// <param name="inSQLHelper"></param>
        /// <param name="inPdSer"></param>
        /// <param name="inForRecalChkDelRefPdX3"></param>
        /// <returns></returns>
        public static string GetPdSerSStep(WS.Data.Agents.cDBMSAgent inSQLHelper, System.Data.IDbConnection inDbConn, System.Data.IDbTransaction inDbTran, string inPdSer, bool inForRecalChkDelRefPdX3)
        {
            string strErrorMsg = "";
            DateTime ldMaxDate = DateTime.MinValue;

            string xdRefSInv = DocumentType.SI.ToString() + "," + DocumentType.SQ.ToString() + "," + DocumentType.SR.ToString() + "," + DocumentType.SU.ToString() + "," + DocumentType.SV.ToString() + "," + DocumentType.SY.ToString() + "," + DocumentType.SG.ToString() + ",";
            string xdRefSDrNote = DocumentType.SD.ToString() + "," + DocumentType.SX.ToString() + "," + DocumentType.SB.ToString() + "," + DocumentType.SZ.ToString() + ",";
            string xdRefSCrNote = DocumentType.SC.ToString() + "," + DocumentType.SA.ToString() + "," + DocumentType.SM.ToString() + "," + DocumentType.SN.ToString() + ",";

            string strRetSStep = BusinessEnum.gc_PDSER_SSTEP_FREE;
            DataSet dtsTem = new DataSet();
            //inSQLHelper.SetPara(new object[] {inPdSer});
            //if (inSQLHelper.SQLExec(ref dtsTem, "QRefPdX3", MapTable.Table.RefPdX3, "select * from "+MapTable.Table.RefPdX3+" where fcPdSer = ? " , ref strErrorMsg))
            if (inSQLHelper.BatchSQLExec(ref dtsTem, "QRefPdX3", MapTable.Table.RefPdX3, "select * from " + MapTable.Table.RefPdX3 + " where fcPdSer = ? ", new object[] { inPdSer }, ref strErrorMsg, inDbConn, inDbTran))
            {
                foreach (DataRow dtrRefPdX3 in dtsTem.Tables["QRefPdX3"].Rows)
                {
                    if (inForRecalChkDelRefPdX3)
                    {
                    }
                    else
                    {
                        string strRefType = dtrRefPdX3["fcRefType"].ToString();
                        string strRefItem = dtrRefPdX3["fcRefItem"].ToString();

                        //เอกสาร Invoice ขาย กับ ใบเพิ่มหนี้ขาย
                        if ((xdRefSInv + xdRefSDrNote).IndexOf(strRefType) > -1
                            && inSQLHelper.BatchSQLExec(ref dtsTem, "QRefProd", MapTable.Table.RefProd, "select fdDate from " + MapTable.Table.RefProd + " where fcSkid = ? ", new object[] { strRefItem }, ref strErrorMsg, inDbConn, inDbTran)
                            && Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]).Date.CompareTo(ldMaxDate.Date) > -1)
                        {
                            ldMaxDate = Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]);
                            strRetSStep = BusinessEnum.gc_PDSER_SSTEP_INV;
                        }
                        //เอกสาร ใบลดหนี้ขาย
                        else if ((xdRefSCrNote).IndexOf(strRefType) > -1
                            && inSQLHelper.BatchSQLExec(ref dtsTem, "QRefProd", MapTable.Table.RefProd, "select fdDate from " + MapTable.Table.RefProd + " where fcSkid = ? ", new object[] { strRefItem }, ref strErrorMsg, inDbConn, inDbTran)
                            && Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]).Date.CompareTo(ldMaxDate.Date) > -1)
                        {
                            ldMaxDate = Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]);
                            strRetSStep = BusinessEnum.gc_PDSER_SSTEP_FREE;
                        }
                        //เอกสาร ใบยืม
                        else if ((DocumentType.SS.ToString()).IndexOf(strRefType) > -1
                            && inSQLHelper.BatchSQLExec(ref dtsTem, "QOrderI", MapTable.Table.OrderI, "select fdDate from " + MapTable.Table.OrderI + " where fcSkid = ? ", new object[] { strRefItem }, ref strErrorMsg, inDbConn, inDbTran)
                            && Convert.ToDateTime(dtsTem.Tables["QOrderI"].Rows[0]["fdDate"]).Date.CompareTo(ldMaxDate.Date) > -1)
                        {
                            ldMaxDate = Convert.ToDateTime(dtsTem.Tables["QOrderI"].Rows[0]["fdDate"]);
                            strRetSStep = BusinessEnum.gc_PDSER_SSTEP_INV;
                        }
                        //เอกสาร SO
                        else if ((DocumentType.SO.ToString()).IndexOf(strRefType) > -1
                            && inSQLHelper.BatchSQLExec(ref dtsTem, "QOrderI", MapTable.Table.OrderI, "select fdDate from " + MapTable.Table.OrderI + " where fcSkid = ? ", new object[] { strRefItem }, ref strErrorMsg, inDbConn, inDbTran)
                            && Convert.ToDateTime(dtsTem.Tables["QOrderI"].Rows[0]["fdDate"]).Date.CompareTo(ldMaxDate.Date) > -1)
                        {
                            ldMaxDate = Convert.ToDateTime(dtsTem.Tables["QOrderI"].Rows[0]["fdDate"]);
                            strRetSStep = BusinessEnum.gc_PDSER_SSTEP_ORDER;
                        }
                        //เอกสารปรับยอดลด (AJ)
                        else if ((DocumentType.AJ.ToString()).IndexOf(strRefType) > -1
                            && inSQLHelper.BatchSQLExec(ref dtsTem, "QRefProd", MapTable.Table.RefProd, "select fcIOType, fdDate from " + MapTable.Table.RefProd + " where fcSkid = ? ", new object[] { strRefItem }, ref strErrorMsg, inDbConn, inDbTran)
                            && dtsTem.Tables["QRefProd"].Rows[0]["fcIOType"].ToString() == "O"
                            && Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]).Date.CompareTo(ldMaxDate.Date) > -1)
                        {
                            ldMaxDate = Convert.ToDateTime(dtsTem.Tables["QRefProd"].Rows[0]["fdDate"]);
                            strRetSStep = BusinessEnum.gc_PDSER_SSTEP_INV;
                        }

                    }
                }
            }
            return strRetSStep;
        }

        public static decimal CalDiscAmtFromDiscStr(string ioDiscStr, decimal inAmt, decimal inQty, decimal inFullQty)
        {
            decimal tCrAmt, tDiscAmt, tRetAmt;
            decimal tCrDiscAmt = 0;
            string tFilterStr = StringHelper.ChrTran(ioDiscStr, "0123456789+%@.", "");
            int tRoundAt = 4;
            ioDiscStr = StringHelper.ChrTran(ioDiscStr, tFilterStr, "");
            string tDiscStr = ioDiscStr;
            string delimStr = " +";
            char[] delimiter = delimStr.ToCharArray();
            string[] tCut = tDiscStr.Split(delimiter);

            decimal tQty = inQty;
            decimal tFullQty = (inFullQty >= 0 ? inFullQty : tQty);
            tDiscAmt = 0;
            decimal lnPriceKe = (tQty == 0 ? 0 : inAmt / tQty);
            for (int intCnt = 0; intCnt < tCut.Length; intCnt++)
            {
                string strDiscSplit = tCut[intCnt].ToString();
                if (strDiscSplit != string.Empty)
                {
                    tCrAmt = inAmt - tDiscAmt;
                    if (strDiscSplit.IndexOf("%") > -1)
                    {
                        tCrDiscAmt = Math.Round(tCrAmt * Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "%", "")) / 100, 6, MidpointRounding.AwayFromZero);
                    }
                    else if (strDiscSplit.IndexOf("@") > -1)
                    {
                        tCrDiscAmt = Math.Round(tCrAmt * Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "@", "")) / 100, 6, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        tCrDiscAmt = Convert.ToDecimal(strDiscSplit);
                        if ((tQty != tFullQty) && (tFullQty != 0))
                            tCrDiscAmt = Math.Round(tCrDiscAmt * tQty / tFullQty, tRoundAt, MidpointRounding.AwayFromZero);
                    }
                }
                tDiscAmt = tDiscAmt + tCrDiscAmt;
            }
            tRetAmt = Math.Round(tDiscAmt, 4, MidpointRounding.AwayFromZero);
            return tRetAmt;
        }

        public static decimal CalScrapQty(string inScrap, decimal inAmt, decimal inQty, decimal inFullQty)
        {
            decimal tCrAmt, tDiscAmt, tRetAmt;
            decimal tCrDiscAmt = 0;
            string tFilterStr = StringHelper.ChrTran(inScrap, "0123456789+%@.", "");
            int tRoundAt = 4;
            inScrap = StringHelper.ChrTran(inScrap, tFilterStr, "");
            string tDiscStr = inScrap;
            string delimStr = " +";
            char[] delimiter = delimStr.ToCharArray();
            string[] tCut = tDiscStr.Split(delimiter);

            decimal tQty = inQty;
            decimal tFullQty = (inFullQty >= 0 ? inFullQty : tQty);
            tDiscAmt = 0;
            decimal lnPriceKe = (tQty == 0 ? 0 : inAmt / tQty);
            for (int intCnt = 0; intCnt < tCut.Length; intCnt++)
            {
                string strDiscSplit = tCut[intCnt].ToString();
                if (strDiscSplit != string.Empty)
                {
                    tCrAmt = inAmt - tDiscAmt;
                    if (strDiscSplit.IndexOf("%") > -1)
                    {

                        tCrDiscAmt = Math.Round(tCrAmt * Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "%", "")) / 100, 6, MidpointRounding.AwayFromZero);
                    }
                    else if (strDiscSplit.IndexOf("@") > -1)
                    {
                        tCrDiscAmt = Math.Round(tCrAmt * Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "@", "")) / 100, 6, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        tCrDiscAmt = Convert.ToDecimal(strDiscSplit);
                        if ((tQty != tFullQty) && (tFullQty != 0))
                            tCrDiscAmt = Math.Round(tCrDiscAmt * tQty / tFullQty, tRoundAt, MidpointRounding.AwayFromZero);
                    }
                }
                tDiscAmt = tDiscAmt + tCrDiscAmt;
            }
            tRetAmt = Math.Round(tDiscAmt, 4, MidpointRounding.AwayFromZero);
            return tRetAmt;
        }

        /// <summary>
        /// คำนวณ Scrap แบบหาร
        /// </summary>
        /// <param name="inScrap"></param>
        /// <param name="inAmt"></param>
        /// <param name="inQty"></param>
        /// <param name="inFullQty"></param>
        /// <returns></returns>
        public static decimal CalScrapQty2(string inScrap, decimal inAmt, decimal inQty, decimal inFullQty)
        {

            decimal tCrAmt, tDiscAmt, tRetAmt;
            decimal tCrDiscAmt = 0;
            string tFilterStr = StringHelper.ChrTran(inScrap, "0123456789+%@.", "");
            int tRoundAt = 4;
            inScrap = StringHelper.ChrTran(inScrap, tFilterStr, "");
            string tDiscStr = inScrap;
            string delimStr = " +";
            char[] delimiter = delimStr.ToCharArray();
            string[] tCut = tDiscStr.Split(delimiter);

            decimal tQty = inQty;
            decimal tFullQty = (inFullQty >= 0 ? inFullQty : tQty);
            tDiscAmt = inAmt;
            decimal lnPriceKe = (tQty == 0 ? 0 : inAmt / tQty);
            for (int intCnt = 0; intCnt < tCut.Length; intCnt++)
            {
                string strDiscSplit = tCut[intCnt].ToString();
                if (strDiscSplit != string.Empty)
                {
                    //tCrAmt = inAmt - tDiscAmt;
                    tCrAmt = tDiscAmt;
                    if (strDiscSplit.IndexOf("%") > -1)
                    {
                        decimal decCalPcn = 1 - Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "%", "")) / 100;
                        if (decCalPcn != 0)
                        {
                            tDiscAmt = Math.Round(tCrAmt / decCalPcn, 6, MidpointRounding.AwayFromZero);
                        }
                    }
                    else if (strDiscSplit.IndexOf("@") > -1)
                    {
                        //tCrDiscAmt = Math.Round(tCrAmt * Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "@", "")) / 100, 6, MidpointRounding.AwayFromZero);
                        decimal decCalPcn = 1 - Convert.ToDecimal(StringHelper.ChrTran(strDiscSplit, "@", "")) / 100;
                        if (decCalPcn != 0)
                        {
                            tDiscAmt = Math.Round(tCrAmt / decCalPcn, 6, MidpointRounding.AwayFromZero);
                        }
                    }
                    else
                    {
                        tCrDiscAmt = Convert.ToDecimal(strDiscSplit);
                        if ((tQty != tFullQty) && (tFullQty != 0))
                            tCrDiscAmt = Math.Round(tCrDiscAmt * tQty / tFullQty, tRoundAt, MidpointRounding.AwayFromZero);
                        tDiscAmt = tCrAmt + tCrDiscAmt;
                    }
                }
                //tDiscAmt = tDiscAmt + tCrDiscAmt;
            }
            tRetAmt = Math.Round(tDiscAmt, 4, MidpointRounding.AwayFromZero);
            return tRetAmt;
        }

        public static string GetMemData(string inMemData, string inFieldCode)
        {
            int tStart = StringHelper.At(Convert.ToChar(127).ToString() + inFieldCode, inMemData) + inFieldCode.Length + 1;
            int tLen = StringHelper.At(inFieldCode + Convert.ToChar(127).ToString(), inMemData) - tStart;
            return (tLen > 0 ? StringHelper.SubStr(inMemData, tStart, tLen) : "");
        }

        public static string SetMemData(string inSetData, string inFieldCode)
        {
            return (inSetData.Length > 0 ? StringHelper.Chr(127) + inFieldCode + inSetData + inFieldCode + StringHelper.Chr(127) : "");
        }

        public static string GetProdCtrlStock(WS.Data.Agents.cDBMSAgent inSQLHelper, string inProd, string inCorpCtrl)
        {
            string strErrorMsg = "";
            DataSet dtsDataEnv = new DataSet();
            string strCtrlStk = inCorpCtrl;
            inSQLHelper.SetPara(new object[] { inProd });
            if (inSQLHelper.SQLExec(ref dtsDataEnv, "QProd", "PROD", "select * from Prod where fcSkid = ?", ref strErrorMsg))
            {
                string strType = dtsDataEnv.Tables["QProd"].Rows[0]["fcCtrlStoc"].ToString();
                strCtrlStk = ("0 ".IndexOf(strType) > -1 ? inCorpCtrl : strType);
            }
            return strCtrlStk;
        }

        public static string GetProdCtrlStock(string inType, string inCorpCtrl)
        {
            string strCtrlStk = ("0 ".IndexOf(inType) > -1 ? inCorpCtrl : inType);
            return strCtrlStk;
        }

        /// <summary>
        /// Change Qty in Um1 to Um2
        /// </summary>
        public static decimal GetQtyInUm2(decimal inQtyInUm1, decimal inUmQty1, decimal inUmQty2)
        {
            return inQtyInUm1 * inUmQty1 / inUmQty2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioPara1">QtyStd</param>
        /// <param name="ioPara2">PriceStd</param>
        /// <param name="ioPara3">UmQty</param>
        /// <param name="ioPara4">QtyInUm</param>
        /// <param name="ioPara5">PriceInUm</param>
        /// <returns></returns>
        public static bool GetQtyPriceInUm(decimal ioPara1, decimal ioPara2, decimal ioPara3, ref decimal ioPara4, ref decimal ioPara5)
        {
            decimal tUmQty = ioPara3;
            if (tUmQty == 0)
            {
                tUmQty = 1;
            }
            ioPara4 = ioPara1 / tUmQty;	// QtyInUm
            ioPara5 = ioPara2 * tUmQty;	// PriceInUm
            return true;
        }

        /// <summary>
        /// Change Price in Um1 to Um2
        /// </summary>
        /// <param name="inPriceInUm1"></param>
        /// <param name="inUmQty1"></param>
        /// <param name="inUmQty2"></param>
        /// <returns></returns>
        public static decimal GetQtyPriceInUm2(decimal inPriceInUm1, decimal inUmQty1, decimal inUmQty2)
        {
            return inPriceInUm1 * inUmQty2 / inUmQty1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioPara1">QtyInUm</param>
        /// <param name="ioPara2">PriceInUm</param>
        /// <param name="ioPara3">UmQty</param>
        /// <param name="ioPara4">QtyInUm</param>
        /// <param name="ioPara5">PriceInUm</param>
        /// <returns></returns>
        public static bool GetQtyPriceStd(decimal ioPara1, decimal ioPara2, decimal ioPara3, ref decimal ioPara4, ref decimal ioPara5)
        {
            decimal tUmQty = ioPara3;
            if (tUmQty == 0)
            {
                tUmQty = 1;
            }
            ioPara4 = ioPara1 * tUmQty;	//QtyInUm * UmQty
            ioPara5 = ioPara2 / tUmQty;	// PriceInUm / UmQty
            return true;
        }

        public static bool IsRootFormula(string inFormula)
        {
            return (inFormula == "$$$$$$$$");
        }

        private static WS.Data.Agents.cDBMSAgent toSQLHelper = null;
        private static QFormulaCollection tmFormula = null;
        private static DataSet dtsTem = null;
        private static decimal mRefQty = 1;
        private static string mRootFM = "";
        private static string mCurrFM = "";
        private static string mCurrQcFM = "";

        public static QFormulaCollection GetItemofFormula(WS.Data.Agents.cDBMSAgent inSQLHelper, string inFormula)
        {
            toSQLHelper = inSQLHelper;
            tmFormula = new QFormulaCollection();
            dtsTem = new DataSet();
            mRootFM = inFormula;
            mCurrFM = "";
            mCurrQcFM = "";
            pmGetItemofFormula(inFormula);
            return tmFormula;
        }

        private static void pmGetItemofFormula(string inFormula)
        {
            string strErrorMsg = "";
            mCurrFM = inFormula;

            toSQLHelper.SetPara(new object[] { inFormula });
            if (toSQLHelper.SQLExec(ref dtsTem, "QFormula", MapTable.Table.Formula, "select * from " + MapTable.Table.Formula + " where FCSKID = ? ", ref strErrorMsg))
            {
                mCurrQcFM = dtsTem.Tables["QFormula"].Rows[0]["fcCode"].ToString();
            }

            toSQLHelper.SetPara(new object[] { "C", inFormula });
            if (toSQLHelper.SQLExec(ref dtsTem, "QPdStruct", MapTable.Table.PdStruct, "select * from " + MapTable.Table.PdStruct + " where FCTYPE = ? and FCFORMULAS = ? order by FCSEQ", ref strErrorMsg))
            {
                foreach (DataRow dtrTemPdSt in dtsTem.Tables["QPdStruct"].Rows)
                {
                    FormulaInfo mInfo = tmFormula.NewRow();

                    mInfo.PdStruct = dtrTemPdSt["fcSkid"].ToString();
                    mInfo.RefPdType = dtrTemPdSt["fcProdOrFo"].ToString();
                    mInfo.PdOrFM = dtrTemPdSt["fcCompo"].ToString();
                    mInfo.Qty = Convert.ToDecimal(dtrTemPdSt["fnQty"]) * mRefQty;
                    mInfo.UM = dtrTemPdSt["fcUm"].ToString();
                    mInfo.UMQty = Convert.ToDecimal(dtrTemPdSt["fnUmQty"]);
                    mInfo.RootFM = mRootFM;
                    mInfo.ParentFM = mCurrFM;
                    mInfo.ParentQcFM = mCurrQcFM;

                    tmFormula.Add(mInfo);

                    mRefQty = Convert.ToDecimal(dtrTemPdSt["fnQty"]);

                    if (mInfo.RefPdType == "F")
                    {
                        string strSavCurr = mCurrFM;
                        decimal decSaveQty = mRefQty;
                        pmGetItemofFormula(mInfo.PdOrFM);
                        mCurrFM = strSavCurr;
                        mRefQty = decSaveQty;
                    } // if
                } // foreach
            } //if

        }

        public static decimal LimitPrcCost(decimal tninPriceKe)
        {
            return (CommonHelper.Between(tninPriceKe, -999999999999, 999999999999) ? tninPriceKe : 0);
        }

        public static string GetNearString(DataTable inTable, string inSeek, string inFld)
        {
            string strResult = "";
            string strSeekVal = inSeek.TrimEnd();

            for (int i = 0; i < inTable.Rows.Count; i++)
            {
                DataRow dtrSeek = inTable.Rows[i];
                int intCmp = string.Compare(dtrSeek[inFld].ToString().TrimEnd(), strSeekVal);
                if (intCmp > -1)
                {
                    strResult = dtrSeek[inFld].ToString().TrimEnd();
                    break;
                }
            }

            return strResult;
        }

        public static void ViewFile(string inFileName, ref string ioErrorMsg)
        {
            if (inFileName != string.Empty)
            {
                try
                {
                    System.Diagnostics.Process Trig = new System.Diagnostics.Process();
                    Trig.StartInfo.FileName = inFileName.Trim();
                    Trig.Start();
                }
                catch (Exception ex)
                {
                    ioErrorMsg = ex.Message;
                }
            }
        }

        public static void PrintPdfFile(string inFileName, string inPrinterName, ref string ioErrorMsg)
        {
            if (inFileName != string.Empty)
            {
                try
                {
                    System.Diagnostics.Process Trig = new System.Diagnostics.Process();
                    string strParaFile = "\"" + inFileName.Trim() + "\"" + " " + "\"" + inPrinterName.Trim() + "\"";
                    Trig.StartInfo.FileName = "AcroRd32.exe";
                    Trig.StartInfo.Arguments = "/t " + strParaFile;
                    Trig.Start();

                    //Trig.CloseMainWindow();
                    //Trig.Close();
                    Trig.Dispose();
                    Trig = null;
                }
                catch (Exception ex)
                {
                    ioErrorMsg = ex.Message;
                }
            }
        }

    }


}
