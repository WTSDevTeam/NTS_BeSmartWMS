using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QEMWorkCalendarInfo
    {

        public static string TableName = MapTable.Table.EMWorkCalendar;
        public static string WorkCalendarItemItem = MapTable.Table.EMWorkCalendarItem;
        public static string WorkCalendarItemItem_Range = MapTable.Table.EMWorkCalendarItem_Range;

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
