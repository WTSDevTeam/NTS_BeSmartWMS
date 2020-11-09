using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QMFPDCapInfo
    {

        public static string TableName = MapTable.Table.PDCapHD;

        public struct Field
        {

            public static string Rowid = "CROWID";
            public static string Active = "CACTIVE";
            public static string ActiveDate = "DACTIVE";
            public static string CorpID = "CCORP";
            public static string Type = "CTYPE";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string SectID = "CSECT";
            public static string WKCtrH = "CWKCTRH";
            public static string WKHour = "NWKHOUR";

        }
    }
}
