using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QCoorInfo
    {
        public static string TableName = MapTable.Table.Coor;

        public struct Field
        {

            public static string CorpID = "FCCORP";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Addr1 = "FCADDR1";
            public static string Addr2 = "FCADDR2";
            public static string Name2 = "FCNAME2";
            public static string Addr12 = "FCADDR12";
            public static string Addr22 = "FCADDR22";
            public static string TelNo = "FCTEL";
            public static string FaxNo = "FCFAX";
        }
    
    }
}
