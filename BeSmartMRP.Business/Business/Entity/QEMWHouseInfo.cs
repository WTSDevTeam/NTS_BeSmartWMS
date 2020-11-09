using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMWHouseInfo
    {

        public static string TableName = MapTable.Table.WHouse;

        public struct Field
        {

            public static string CorpID = "FCCORP";
            public static string BranchID = "FCBRANCH";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Name2 = "FCNAME2";
            public static string Type = "FCTYPE";

            public static string Location_Input = "FCWHLOCA_INPUT";
            public static string Location_Output = "FCWHLOCA_OUTPUT";
            public static string Location_Stock = "FCWHLOCA_STOCK";

        }

    }
}
