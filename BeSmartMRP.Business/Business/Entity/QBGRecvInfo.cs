using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QBGRecvInfo
    {

        public static string TableName = MapTable.Table.BudRecvHD;
        public static string TableNameItem1 = MapTable.Table.BudRecvIT;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string SectID = "CSECT";
            public static string DeptID = "CDEPT";
            public static string JobID = "CJOB";
            public static string ProjID = "CPROJ";
            public static string BudGetYear = "NBGYEAR";
            public static string RefType = "CREFTYPE";
            public static string BGBookID = "CBGBOOK";

            public static string Code = "CCODE";
            public static string Date = "DDATE";
            public static string Step = "CSTEP";
            public static string Status = "CSTAT";
            public static string Revise = "CREVISE";
            public static string IsClose = "CISCLOSE";
            public static string Amount = "NAMT";
            public static string RefNo = "CREFNO";
            public static string Desc1 = "CDESC1";
            public static string Desc2 = "CDESC2";
            public static string Desc3 = "CDESC3";
            public static string Desc4 = "CDESC4";
            public static string Desc5 = "CDESC5";
            public static string CancelRemark = "CCANCELDES";
            public static string LastUpdate = "DLASTUPDBY";
            public static string LastUpdateBy = "CLASTUPDBY";
            public static string CancelDate = "DCANCEL";
            public static string CancelBy = "CCANCELBY";

            public static string IsRevise = "CISREV";
            public static string RefToRefType = "CREFTOTYPE";
            public static string RefToBook = "CREFTOBOOK";
            public static string RefToRowID = "CREFTOROW";

        }

    }
}
