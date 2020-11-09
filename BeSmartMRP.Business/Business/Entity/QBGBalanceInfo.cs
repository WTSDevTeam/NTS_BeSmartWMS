using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QBGBalanceInfo
    {

        public static string TableName = MapTable.Table.BudgetBalance;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string Type = "CTYPE";
            public static string BudGetYear = "NBGYEAR";
            public static string SectID = "CSECT";
            public static string DeptID = "CDEPT";
            public static string JobID = "CJOB";
            public static string ProjID = "CPROJ";
            public static string BGChart = "CBGCHARTHD";
            public static string Amt = "NAMT";
            public static string RecvAmt = "NRECVAMT";
            public static string UseAmt = "NUSEAMT";

            //public static System.Data.DataColumn dt1 = new System.Data.DataColumn("");

        }

    }
}
