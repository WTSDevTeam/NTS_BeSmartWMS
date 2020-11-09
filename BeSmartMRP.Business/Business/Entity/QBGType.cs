using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QBGTypeInfo
    {

        public static string TableName = MapTable.Table.BudgetType;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string IsActive = "CACTIVE";
            public static string ActiveDate = "DACTIVE";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Type = "CTYPE";
            public static string Category = "CCATEG";

        }

    }
}
