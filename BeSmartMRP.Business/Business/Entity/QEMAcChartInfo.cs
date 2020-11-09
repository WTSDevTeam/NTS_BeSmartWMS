using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QEMAcChartInfo
    {

        public static string TableName = MapTable.Table.EMAcChart;

        public struct Field
        {

            public static string CorpID = "CCORPCHAR";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string Group = "CGROUP";
            public static string Level = "NLEVEL";
            public static string Categ = "CCATEG";
            public static string Categ2 = "CCATEG2";
            public static string PRAcChart = "CPRACCHART";

        }
    }

}
