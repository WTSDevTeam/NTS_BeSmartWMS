using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMProjInfo
    {

        public static string TableName = MapTable.Table.EMProj;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Type = "CTYPE";
            public static string PRProj = "CPRPROJ";
            public static string IsAlloc = "CISALLOC";

        }
    
    }
}
