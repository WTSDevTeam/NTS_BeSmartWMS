using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QEMStdWorkHourInfo
    {

        public static string TableName = MapTable.Table.EMWorkHour;
        public static string WorkHourItem = MapTable.Table.EMWorkHourItem;
        public static string WorkHourItem_Range = MapTable.Table.EMWorkHourItem_Range;

        public struct Field
        {

            public static string RowID = MapTable.ShareField.RowID;
            public static string CorpID = "CCORP";
            public static string PlantID = "CPLANT";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Year = "NYEAR";
            public static string Date = "DDATE";

        }

    }
}
