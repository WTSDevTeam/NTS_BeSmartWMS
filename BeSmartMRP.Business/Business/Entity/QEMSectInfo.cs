using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    
    public class QEMSectInfo
    {

        public static string TableName = MapTable.Table.EMSect;

        public struct Field
        {

            public static string RowID = "FCSKID";
            public static string CorpID = "FCCORP";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Name2 = "FCNAME2";
            public static string DeptID = "FCDEPT";
            public static string PRSectID = "FCPRSECT";
            public static string Type = "FCTYPE";

        }

    }
}
