using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using AppUtil;
using WS.Data;
using WS.Data.Agents;

using BeSmartMRP.Business.Component;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP.UIHelper
{
    public class BudgetHelper
    {

        private static DataSet dtsDataEnv = new DataSet();

        public static string GetAuthSectList()
        {
            return pmGetSectList();
        }

        private static string pmGetSectList()
        {
            
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strResult = "";
            string strErrorMsg = "";
            string strSQLExec = "select EMSECT.CROWID, EMSECT.CCODE, EMSECT.CNAME, EMSECT.CNAME2, EMSECT.DCREATE, EMSECT.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from EMSECT ";
            strSQLExec += " left join APPLOGIN EM1 ON EM1.CROWID = EMSECT.CCREATEBY ";
            strSQLExec += " left join APPLOGIN EM2 ON EM2.CROWID = EMSECT.CLASTUPDBY ";
            strSQLExec += " where EMSECT.CCORP = ? ";

            pobjSQLUtil.SetPara(new object[1] { App.ActiveCorp.RowID });

            pobjSQLUtil.SQLExec(ref dtsDataEnv, "QBrow", "EMSECT", strSQLExec, ref strErrorMsg);

            foreach (DataRow dtrBrow in dtsDataEnv.Tables["QBrow"].Rows)
            {
                if (App.PermissionManager.CheckPermissionBySect(AuthenType.CanAccess, App.AppUserName, App.AppUserID, dtrBrow["cRowID"].ToString()))
                {
                    strResult += "'" + dtrBrow[QEMSectInfo.Field.Code].ToString().TrimEnd() + "' ,";
                }
            }
            if (strResult.Trim() != string.Empty)
            {
                strResult = AppUtil.StringHelper.Left(strResult, StringHelper.RAt(",", strResult) - 1);
                strResult = "(" + strResult + ")";
            }

            return strResult;
        }

        public static string GetShortThaiDate(DateTime inDate)
        {
            string strResult = inDate.ToString("dd/MM/") + (inDate.Year+543).ToString("00");
            return strResult;
        }

        public static string GetLongThaiDate(DateTime inDate)
        {
            string strResult = inDate.ToString("dd MMMM ") + (inDate.Year + 543).ToString("0000");
            return strResult;
        }

        public static DocumentType GetDocumentType(string inRefType)
        {
            DocumentType oResult = DocumentType.A1;

            if (inRefType == SysDef.gc_REFTYPE_BGTRAN1)
                oResult = DocumentType.B1;
            else if (inRefType == SysDef.gc_REFTYPE_BGTRAN2)
                oResult = DocumentType.B2;
            else if (inRefType == SysDef.gc_REFTYPE_BGTRAN3)
                oResult = DocumentType.B3;

            
            else if (inRefType == SysDef.gc_REFTYPE_RECV1)
                oResult = DocumentType.R1;
            else if (inRefType == SysDef.gc_REFTYPE_RECV2)
                oResult = DocumentType.R2;
            else if (inRefType == SysDef.gc_REFTYPE_RECV3)
                oResult = DocumentType.R3;


            else if (inRefType == SysDef.gc_REFTYPE_ADV1)
                oResult = DocumentType.R4;
            else if (inRefType == SysDef.gc_REFTYPE_ADV2)
                oResult = DocumentType.R5;
            else if (inRefType == SysDef.gc_REFTYPE_ADV3)
                oResult = DocumentType.R6;

            else if (inRefType == SysDef.gc_REFTYPE_ADJ1)
                oResult = DocumentType.A1;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ2)
                oResult = DocumentType.A2;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ3)
                oResult = DocumentType.A3;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ4)
                oResult = DocumentType.A4;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ5)
                oResult = DocumentType.A5;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ6)
                oResult = DocumentType.A6;

            else if (inRefType == SysDef.gc_REFTYPE_ADJ7)
                oResult = DocumentType.A7;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ8)
                oResult = DocumentType.A8;
            else if (inRefType == SysDef.gc_REFTYPE_ADJ9)
                oResult = DocumentType.A9;
            else if (inRefType == SysDef.gc_REFTYPE_ADJA)
                oResult = DocumentType.AA;
            else if (inRefType == SysDef.gc_REFTYPE_ADJB)
                oResult = DocumentType.AB;
            else if (inRefType == SysDef.gc_REFTYPE_ADJC)
                oResult = DocumentType.AC;

            else if (inRefType == SysDef.gc_REFTYPE_PAY1)
                oResult = DocumentType.P1;
            else if (inRefType == SysDef.gc_REFTYPE_PAY2)
                oResult = DocumentType.P2;
            else if (inRefType == SysDef.gc_REFTYPE_PAY3)
                oResult = DocumentType.P3;
            else if (inRefType == SysDef.gc_REFTYPE_ADC1)
                oResult = DocumentType.P4;
            else if (inRefType == SysDef.gc_REFTYPE_ADC2)
                oResult = DocumentType.P5;
            else if (inRefType == SysDef.gc_REFTYPE_ADC3)
                oResult = DocumentType.P6;

            return oResult;
        }

        public static BudgetStep GetBudgetStep(string inKey)
        {
            BudgetStep oResult = BudgetStep.Prepare;

            if (inKey == SysDef.gc_BGTRAN_STEP_PREPARE)
                oResult = BudgetStep.Prepare;
            else if (inKey == SysDef.gc_BGTRAN_STEP_APPROVE)
                oResult = BudgetStep.Approve;
            else if (inKey == SysDef.gc_BGTRAN_STEP_POST)
                oResult = BudgetStep.Post;
            else if (inKey == SysDef.gc_BGTRAN_STEP_REVISE)
                oResult = BudgetStep.Revise;

            return oResult;
        }

        public static ApproveStep GetApproveStep(string inKey)
        {
            ApproveStep oResult = ApproveStep.Wait;

            if (inKey == SysDef.gc_APPROVE_STEP_WAIT)
                oResult = ApproveStep.Wait;
            else if (inKey == SysDef.gc_APPROVE_STEP_APPROVE)
                oResult = ApproveStep.Approve;
            else if (inKey == SysDef.gc_APPROVE_STEP_POST)
                oResult = ApproveStep.Post;
            else if (inKey == SysDef.gc_APPROVE_STEP_REJECT)
                oResult = ApproveStep.Reject;
            else if (inKey == SysDef.gc_APPROVE_STEP_REVISE)
                oResult = ApproveStep.Revise;

            return oResult;
        }

        public static string GetApproveStepText(ApproveStep inStep)
        {
            string strResult = "";
            switch (inStep)
            {
                case ApproveStep.Wait:
                    strResult = "WAIT"; 
                    break;
                case ApproveStep.Approve:
                    strResult = "APPROVE"; 
                    break;
                case ApproveStep.Reject:
                    strResult = "REJECT";
                    break;
                case ApproveStep.Pass:
                    strResult = "PASS";
                    break;
                case ApproveStep.Post:
                    strResult = "POST";
                    break;
            } 
            return strResult;
        }

        public static void SuBeSmartMRP(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , ref decimal ioAmt, ref decimal ioRecvAmt, ref decimal ioUseAmt, ref decimal ioBalAmt)
        {
            pmSuBeSmartMRP(inSQLHelper, inCorp, inBranch, inType, inBGYear, inSect, inJob, ref ioAmt, ref ioRecvAmt, ref ioUseAmt, ref ioBalAmt);
        }

        public static void SuBeSmartMRPGroup(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , string inQcBGChart
            , ref decimal ioAmt, ref decimal ioRecvAmt, ref decimal ioUseAmt, ref decimal ioBalAmt)
        {
            pmSuBeSmartMRPGroup(inSQLHelper, inCorp, inBranch, inType, inBGYear, inSect, inJob, inQcBGChart, ref ioAmt, ref ioRecvAmt, ref ioUseAmt, ref ioBalAmt);
        }

        public static void SuBeSmartMRPBeforeDate(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , DateTime inBeforeDate
            , ref decimal ioAmt)
        {
            pmSuBeSmartMRPBeforeDate(inSQLHelper, inCorp, inBranch, inType, inBGYear, inSect, inJob, inBeforeDate, ref ioAmt);
        }

        private static void pmSuBeSmartMRP(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , ref decimal ioAmt, ref decimal ioRecvAmt, ref decimal ioUseAmt, ref decimal ioBalAmt)
        {

            string strErrorMsg = "";
            string strSQLText = "select ";
            strSQLText += " sum(BGBALANCE.NAMT) as AMT ";
            strSQLText += " ,sum(BGBALANCE.NRECVAMT) as RECVAMT ";
            strSQLText += " ,sum(BGBALANCE.NUSEAMT) as USEAMT ";
            strSQLText += " ,sum(BGBALANCE.NAMT - BGBALANCE.NRECVAMT) as BALAMT ";
            strSQLText += " from " + QBGBalanceInfo.TableName;
            strSQLText += " where CCORP = ? AND CBRANCH = ? AND CTYPE = ? AND NBGYEAR = ? AND CSECT = ? AND CJOB = ? ";

            ioAmt = 0; ioRecvAmt = 0; ioUseAmt = 0; ioBalAmt = 0;
            WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            objSQLHelper.SetPara(new object[] { inCorp, inBranch, inType, inBGYear, inSect, inJob });
            if (objSQLHelper.SQLExec(ref BudgetHelper.dtsDataEnv, "QSumBG", QBGBalanceInfo.TableName, strSQLText, ref strErrorMsg))
            {
                DataRow dtrBGBal = BudgetHelper.dtsDataEnv.Tables["QSumBG"].Rows[0];
                ioAmt = Convert.IsDBNull(dtrBGBal["AMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["AMT"]);
                ioRecvAmt = Convert.IsDBNull(dtrBGBal["RECVAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["RECVAMT"]);
                ioUseAmt = Convert.IsDBNull(dtrBGBal["USEAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["USEAMT"]);
                ioBalAmt = Convert.IsDBNull(dtrBGBal["BALAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["BALAMT"]);
            }
        }

        private static void pmSuBeSmartMRPBeforeDate(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , DateTime inBeforeDate
            , ref decimal ioAmt)
        {

            string strErrorMsg = "";
            string strSQLText = "select ";
            strSQLText += " sum(BGRECVIT.NAMT) as AMT ";
            strSQLText += " from BGRECVIT ";
            strSQLText += " where CCORP = ? AND CBRANCH = ? AND CREFTYPE = ? AND NBGYEAR = ? AND CSECT = ? AND CJOB = ? and CSTAT <> 'C' and CISREV <> 'Y' and DDATE < ? ";

            ioAmt = 0;
            WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            objSQLHelper.SetPara(new object[] { inCorp, inBranch, inType, inBGYear, inSect, inJob, inBeforeDate });
            if (objSQLHelper.SQLExec(ref BudgetHelper.dtsDataEnv, "QSumBG", QBGBalanceInfo.TableName, strSQLText, ref strErrorMsg))
            {
                DataRow dtrBGBal = BudgetHelper.dtsDataEnv.Tables["QSumBG"].Rows[0];
                ioAmt = Convert.IsDBNull(dtrBGBal["AMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["AMT"]);
            }
        }

        private static void pmSuBeSmartMRPGroup(
            WS.Data.Agents.cDBMSAgent inSQLHelper
            , string inCorp
            , string inBranch
            , string inType
            , int inBGYear
            , string inSect
            , string inJob
            , string inQcBGChart
            , ref decimal ioAmt, ref decimal ioRecvAmt, ref decimal ioUseAmt, ref decimal ioBalAmt)
        {

            string strSumCode = inQcBGChart.TrimEnd() + "%";
            string strErrorMsg = "";
            string strSQLText = "select ";
            strSQLText += " sum(BGBALANCE.NAMT) as AMT ";
            strSQLText += " ,sum(BGBALANCE.NRECVAMT) as RECVAMT ";
            strSQLText += " ,sum(BGBALANCE.NUSEAMT) as USEAMT ";
            strSQLText += " ,sum(BGBALANCE.NAMT - BGBALANCE.NRECVAMT) as BALAMT ";
            strSQLText += " from " + QBGBalanceInfo.TableName;
            strSQLText += " left join BGCHARTHD on BGCHARTHD.CROWID = BGBALANCE.CBGCHARTHD ";
            strSQLText += " left join BGCHARTHD PRCHART on PRCHART.CROWID = BGCHARTHD.CPRBGCHART ";
            strSQLText += " where BGBALANCE.CCORP = ? AND BGBALANCE.CBRANCH = ? AND BGBALANCE.CTYPE = ? AND BGBALANCE.NBGYEAR = ? AND BGBALANCE.CSECT = ? AND BGBALANCE.CJOB = ? ";
            strSQLText += " AND BGCHARTHD.CCODE like ? ";

            ioAmt = 0; ioRecvAmt = 0; ioUseAmt = 0; ioBalAmt = 0;
            WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            objSQLHelper.SetPara(new object[] { inCorp, inBranch, inType, inBGYear, inSect, inJob, strSumCode });
            if (objSQLHelper.SQLExec(ref BudgetHelper.dtsDataEnv, "QSumBG", QBGBalanceInfo.TableName, strSQLText, ref strErrorMsg))
            {
                foreach (DataRow dtrBGBal in BudgetHelper.dtsDataEnv.Tables["QSumBG"].Rows)
                {
                    ioAmt += Convert.IsDBNull(dtrBGBal["AMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["AMT"]);
                    ioRecvAmt += Convert.IsDBNull(dtrBGBal["RECVAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["RECVAMT"]);
                    ioUseAmt += Convert.IsDBNull(dtrBGBal["USEAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["USEAMT"]);
                    ioBalAmt += Convert.IsDBNull(dtrBGBal["BALAMT"]) ? 0 : Convert.ToDecimal(dtrBGBal["BALAMT"]);
                }
            }
        }


    }
}
