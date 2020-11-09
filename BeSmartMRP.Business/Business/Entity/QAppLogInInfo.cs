using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QAppLogInInfo
    {

        public static string TableName = MapTable.Table.AppLogIn;

        public struct Field
        {

            public static string Active = "CACTIVE";
            public static string LoginName = "CLOGIN";
            public static string RCode = "CRCODE";
            public static string Password = "CPWD";
            public static string HPwd = "CHPWD";

            public static string Locale_UI = "CLOCALE_UI";
            public static string Locale_Report = "CLOCALE_RP";

        }

    }

}
