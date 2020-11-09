using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMDeptInfo
    {

        public static string TableName = MapTable.Table.EMDept;

        public struct Field
        {

            public static string CorpID = "FCCORP";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Name2 = "FCNAME2";
            public static string Type = "FCTYPE";
            public static string PRProj = "FCPRPROJ";
        }

    }
}
