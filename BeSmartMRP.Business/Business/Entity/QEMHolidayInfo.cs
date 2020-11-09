using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMHolidayInfo
    {
        public static string TableName = MapTable.Table.EMHoliday;

        public struct Field
        {

            public static string RowID = MapTable.ShareField.RowID;
            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Year = "NYEAR";
            public static string Date = "DDATE";

        }

    }
}
