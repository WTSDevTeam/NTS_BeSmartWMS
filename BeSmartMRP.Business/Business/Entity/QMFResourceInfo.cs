
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QMFResourceInfo
    {

        public static string TableName = MapTable.Table.MFResource;

        public struct Field
        {

            public static string RowID = MapTable.ShareField.RowID;
            public static string Type = "CTYPE";
            public static string IsActive = "CACTIVE";
            public static string ActiveDate = "DACTIVE";
            public static string CorpID = "CCORP";
            public static string Code = "CCODE";
            public static string Name = "CNAME";
            public static string Name2 = "CNAME2";
            public static string InstallDate = "DINSTALL";
            public static string SerialNo = "CSERIALNO";
            public static string Model = "CMODEL";
            public static string PlantID = "CPLANT";
            public static string MCType = "CMCTYPE";
            public static string Department = "CSECT";
            public static string Job = "CJOB";
            public static string MCGroup = "CMCGROUP";
            public static string Capacity = "NCAPACITY";
            public static string Load_Maximum = "NPCN_MAXLD";
            public static string Load_Minimum = "NPCN_MINLD";
            public static string LeadTime_SetUp = "NT_SETUP";
            public static string LeadTime_Process = "NT_PROCESS";
            public static string LeadTime_Tear = "NT_TEAR";

        }

    }

}
