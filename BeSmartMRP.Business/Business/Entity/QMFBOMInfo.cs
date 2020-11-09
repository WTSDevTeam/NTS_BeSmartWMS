using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QMFBOMInfo
    {
        public static string TableName = MapTable.Table.MFBOMHead;

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
            public static string PlantID = "CPLANT";

            public static string EngBOMHD = "CENGDRAW";
            public static string MfgProdID = "CMFGPROD";
            public static string MfgUMID = "CMFGUM";
            public static string MfgUMQty = "NMFGUMQTY";
            public static string MfgQty = "NMFGQTY";
            public static string StartDate = "DSTART";
            
            //Standard Manufacturing LeadTime
            //Queue LeadTime
            //SetUp LeadTime
            //Process LeadTime
            //TearDown LeadTime
            //Wait LeadTime
            //Move LeadTime

            public static string LeadTime_Queue = "NT_QUEUE"; //รอคิวก่อนเข้าผลิต
            public static string LeadTime_SetUp = "NT_SETUP"; //เวลาตั้งเครื่อง
            public static string LeadTime_Process = "NT_PROCESS"; //เวลาในการผลิต
            public static string LeadTime_Tear = "NT_TEAR"; //แกะชิดงานออกจากเครื่องจักร
            public static string LeadTime_Wait = "NT_WAIT"; //
            public static string LeadTime_Move = "NT_MOVE"; //เวลาในการเคลื่อนย้ายงานไปยัง Process ถัดไป

            public static string Scrap = "CSCRAP";
            public static string FileAttach = "CFILEATTN";
            public static string RoundCtrl = "CROUNDCTRL";

            public static string Remark1 = "CREMARK1";
            public static string Remark2 = "CREMARK2";
            public static string Remark3 = "CREMARK3";
            public static string Remark4 = "CREMARK4";
            public static string Remark5 = "CREMARK5";
            public static string Remark6 = "CREMARK6";
            public static string Remark7 = "CREMARK7";
            public static string Remark8 = "CREMARK8";
            public static string Remark9 = "CREMARK9";
            public static string Remark10 = "CREMARK10";

            public static string QcRemark1 = "CQCREMARK1";
            public static string QcRemark2 = "CQCREMARK2";
            public static string QcRemark3 = "CQCREMARK3";
            public static string QcRemark4 = "CQCREMARK4";
            public static string QcRemark5 = "CQCREMARK5";
            public static string QcRemark6 = "CQCREMARK6";
            public static string QcRemark7 = "CQCREMARK7";
            public static string QcRemark8 = "CQCREMARK8";
            public static string QcRemark9 = "CQCREMARK9";
            public static string QcRemark10 = "CQCREMARK10";

            public static string IsApprove = "CISAPPROVE";
            public static string ApproveBy = "CAPPROVEBY";
            public static string ApproveDate = "DAPPROVE";

        }

    }

}
