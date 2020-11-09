using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    
    public class QBGBookInfo
    {

        public static string TableName = MapTable.Table.BudgetBook;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string RefType = "CREFTYPE";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Type = "CTYPE";
        }


    }
}
