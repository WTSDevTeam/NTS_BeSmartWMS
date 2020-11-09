using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QMFStdOprInfo
    {
        public static string TableName = MapTable.Table.MFStdOpr;

        public struct Field
        {

            public static string RowID = MapTable.ShareField.RowID;
            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";

            public static string MCType = "CMCTYPE";
            public static string Department = "CSECT";
            public static string Job = "CJOB";

            public static string Remark1 = "CREMARK1";
            public static string Remark2 = "CREMARK2";
            public static string Remark3 = "CREMARK3";

        }

    }
}
