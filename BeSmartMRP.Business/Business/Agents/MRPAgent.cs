//TODO: ทำ function GetBuyPrice

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
    public class MRPAgent
    {

        public MRPAgent() { }

        public static string[] AThaiDate = new string[7] { "อาทิตย์", "จันทร์", "อังคาร", "พุธ", "พฤหัสบดี", "ศุกร์", "เสาร์" };
        public static string[] AEngDate = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public static string[] AEngVShortDate = new string[7] { "SU", "M", "T", "W", "TH", "F", "S" };

        public static void GetCostLine(WS.Data.Agents.cDBMSAgent inSQLUtil, ref QCostLineInfo ioInfo, string inRefTab, string inMasterH, string inType)
        {
            pmGetCostLine(inSQLUtil, ref ioInfo, inRefTab, inMasterH, inType, false);
        }

        public static void GetCostLine(WS.Data.Agents.cDBMSAgent inSQLUtil, ref QCostLineInfo ioInfo, string inRefTab, string inMasterH, string inType, bool inIsFixUnit)
        {
            pmGetCostLine(inSQLUtil, ref ioInfo, inRefTab, inMasterH, inType, inIsFixUnit);
        }

        private static void pmGetCostLine(WS.Data.Agents.cDBMSAgent inSQLUtil, ref QCostLineInfo ioInfo, string inRefTab, string inMasterH, string inType, bool inIsFixUnit)
        {

            ioInfo.Cost_Fix = 0;
            ioInfo.Cost_Var_ManHour1 = 0;
            ioInfo.Cost_Var_ManHour2 = 0;
            ioInfo.Cost_Var_ManHour3 = 0;
            ioInfo.Cost_Var_ManHour4 = 0;
            ioInfo.Cost_Var_ManHour5 = 0;
            ioInfo.Cost_Var_ByOutput1 = 0;
            ioInfo.Cost_Var_ByOutput2 = 0;
            ioInfo.Cost_Var_ByOutput3 = 0;
            ioInfo.Cost_Var_ByOutput4 = 0;
            ioInfo.Cost_Var_ByOutput5 = 0;

            DataSet dtsDataEnv = new DataSet();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLUtil;
            string strSQLStr_CostLine = "select COSTLINE.CCOSTTYPE, COSTLINE.NAMT, COSTLINE.CCOSTBY from COSTLINE where CREFTAB = ? and CMASTERH = ? and CTYPE = ? ";

            pobjSQLUtil.SetPara(new object[] { inRefTab, inMasterH, inType });
            pobjSQLUtil.SQLExec(ref dtsDataEnv, "QCostLine", QCostLineInfo.TableName, strSQLStr_CostLine, ref strErrorMsg);
            foreach (DataRow dtrCostLine in dtsDataEnv.Tables["QCostLine"].Rows)
            {
                decimal decAmt = Convert.ToDecimal(dtrCostLine[QCostLineInfo.Field.Amt]);
                string strCostBy = dtrCostLine[QCostLineInfo.Field.CostBy].ToString();
                switch (dtrCostLine[QCostLineInfo.Field.CostType].ToString())
                {
                    case "FIX":
                        ioInfo.Cost_Fix = decAmt;
                        break;
                    case "VM1":
                        ioInfo.Cost_Var_ManHour1 = (inIsFixUnit ? decAmt : pmConvert2Sec(decAmt, strCostBy));
                        ioInfo.Cost_Var_ManHour1_Unit = strCostBy;
                        break;
                    case "VM2":
                        ioInfo.Cost_Var_ManHour2 = (inIsFixUnit ? decAmt : pmConvert2Sec(decAmt, strCostBy));
                        ioInfo.Cost_Var_ManHour2_Unit = strCostBy;
                        break;
                    case "VM3":
                        ioInfo.Cost_Var_ManHour3 = (inIsFixUnit ? decAmt : pmConvert2Sec(decAmt, strCostBy));
                        ioInfo.Cost_Var_ManHour3_Unit = strCostBy;
                        break;
                    case "VM4":
                        ioInfo.Cost_Var_ManHour4 = (inIsFixUnit ? decAmt : pmConvert2Sec(decAmt, strCostBy));
                        ioInfo.Cost_Var_ManHour4_Unit = strCostBy;
                        break;
                    case "VM5":
                        ioInfo.Cost_Var_ManHour5 = (inIsFixUnit ? decAmt : pmConvert2Sec(decAmt, strCostBy));
                        ioInfo.Cost_Var_ManHour5_Unit = strCostBy;
                        break;
                    case "VP1":
                        ioInfo.Cost_Var_ByOutput1 = decAmt;
                        break;
                    case "VP2":
                        ioInfo.Cost_Var_ByOutput2 = decAmt;
                        break;
                    case "VP3":
                        ioInfo.Cost_Var_ByOutput3 = decAmt;
                        break;
                    case "VP4":
                        ioInfo.Cost_Var_ByOutput4 = decAmt;
                        break;
                    case "VP5":
                        ioInfo.Cost_Var_ByOutput5 = decAmt;
                        break;
                }
            }

        }

        private static decimal pmConvert2Sec(decimal inAmt, string inCostBy)
        {

            decimal decRetSec = 0;
            switch (inCostBy)
            {
                case "S":
                    decRetSec = inAmt;
                    break;
                case "M":
                    decRetSec = inAmt / 60;
                    break;
                case "H":
                    decRetSec = inAmt / 60 / 60;
                    break;
                case "D":
                    decRetSec = inAmt / 60 / 60 / 60;
                    break;
                default:
                    decRetSec = inAmt;
                    break;
            }
            return decRetSec;
        }

    }

}
