using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QCostLineInfo
    {

        public static string TableName = MapTable.Table.CostLine;

        public struct Field
        {

            public static string RowID = MapTable.ShareField.RowID;
            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string CostType = "CCOSTTYPE";
            public static string CostBy = "CCOSTBY";
            public static string Type = "CTYPE";
            public static string Amt = "NAMT";

        }

        public decimal Cost_Fix = 0;
        
        public decimal Cost_Var_ManHour1 = 0;
        public decimal Cost_Var_ManHour2 = 0;
        public decimal Cost_Var_ManHour3 = 0;
        public decimal Cost_Var_ManHour4 = 0;
        public decimal Cost_Var_ManHour5 = 0;

        public decimal Cost_Var_ByOutput1 = 0;
        public decimal Cost_Var_ByOutput2 = 0;
        public decimal Cost_Var_ByOutput3 = 0;
        public decimal Cost_Var_ByOutput4 = 0;
        public decimal Cost_Var_ByOutput5 = 0;

        public string Cost_Var_ManHour1_Unit = "";
        public string Cost_Var_ManHour2_Unit = "";
        public string Cost_Var_ManHour3_Unit = "";
        public string Cost_Var_ManHour4_Unit = "";
        public string Cost_Var_ManHour5_Unit = "";

    }
}
