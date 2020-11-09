using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QMFWCloseIT_OPInfo
    {
        public static string TableName = MapTable.Table.MFWCloseIT_OP;

        public struct Field
        {

            public static string Rowid = "CROWID";
            public static string Corp = "CCORP";
            public static string Plant = "CPLANT";
            public static string Type = "CTYPE";
            public static string WorderH = "CWORDERH";
            public static string MOPR = "CMOPR";
            public static string WkctrH = "CWKCTRH";
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
            public static string Opseq = "COPSEQ";
            public static string Subop = "CSUBOP";
            public static string Nextop = "CNEXTOP";
            public static string Seq = "CSEQ";
            public static string LeadTime_Queue = "NT_QUEUE"; //รอคิวก่อนเข้าผลิต
            public static string LeadTime_SetUp = "NT_SETUP"; //เวลาตั้งเครื่อง
            public static string LeadTime_Process = "NT_PROCESS"; //เวลาในการผลิต
            public static string LeadTime_Tear = "NT_TEAR"; //แกะชิดงานออกจากเครื่องจักร
            public static string LeadTime_Wait = "NT_WAIT"; //
            public static string LeadTime_Move = "NT_MOVE"; //เวลาในการเคลื่อนย้ายงานไปยัง Process ถัดไป

        }

    }

}
