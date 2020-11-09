using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QAppEmplInfo
    {

        public static string TableName = MapTable.Table.AppEmpl;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Addr1 = "CADDR1";
            public static string Addr2 = "CADDR2";
            public static string Name2 = "CNAME2";
            public static string Addr12 = "CADDR12";
            public static string Addr22 = "CADDR22";
            public static string TelNo = "CTEL";
            public static string FaxNo = "CFAX";
            public static string LoginName = "CRCODE";

        }

    }

}
