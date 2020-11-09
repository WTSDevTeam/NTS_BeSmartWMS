using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QBGYearInfo
    {

        public static string TableName = MapTable.Table.BudgetYear;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Year = "NYEAR";
            public static string Desc = "CDESC";
            public static string Desc2 = "CDESC2";
            public static string IsActive = "CACTIVE";
            public static string ActiveDate = "DACTIVE";
            public static string IsLock = "CISLOCK";
            public static string LockDate = "DLOCKED";

        }

    }
}
