using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QMfgBookInfo
    {

        public static string TableName = MapTable.Table.MfgBook;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string PlantID = "CPLANT";
            public static string RefType = "CREFTYPE";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Type = "CTYPE";

            public static string WHouse_FG = "CWHOUSE_FG";
            public static string WHouse_RM = "CWHOUSE_RM";

            public static string MBook_PR = "CMBOOK_PR";
            public static string MBook_PO = "CMBOOK_PO";

            public static string IsGenDupPR = "CGENDUPPR";

        }
    
    }

}
