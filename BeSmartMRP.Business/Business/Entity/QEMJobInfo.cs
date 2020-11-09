using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMJobInfo
    {

        public static string TableName = MapTable.Table.EMJob;

        public struct Field
        {

            public static string RowID = "FCSKID";
            public static string CorpID = "FCCORP";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Name2 = "FCNAME2";
            public static string ProjID = "FCPROJ";
            public static string PRJobID = "FCPRJOB";
            public static string Type = "FCTYPE";
            public static string SectID = "FCSECT";
            public static string IsAlloc = "FCISALLOC";

        }

    }
}
