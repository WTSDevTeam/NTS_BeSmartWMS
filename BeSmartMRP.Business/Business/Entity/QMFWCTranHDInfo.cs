using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    
    public class QMFWCTranHDInfo
    {

        public static string TableName = MapTable.Table.MFWCTranHead;

        public struct Field
        {

            //Head
            public static string RowID = MapTable.ShareField.RowID;
            public static string Rftype = "CRFTYPE";
            public static string Reftype = "CREFTYPE";
            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string PlantID = "CPLANT";
            public static string DeptID = "CDEPT";
            public static string SectID = "CSECT";
            public static string JobID = "CJOB";
            public static string ProjID = "CPROJ";
            public static string Stat = "CSTAT";
            public static string Step = "CSTEP";
            public static string MStep = "CMSTEP";
            public static string CoorID = "CCOOR";
            public static string Code = "CCODE";
            public static string RefNo = "CREFNO";
            public static string Date = "DDATE";
            public static string DueDate = "DDUEDATE";
            public static string Priority = "CPRIORITY";
            public static string MfgBookID = "CMFGBOOK";
            public static string Machine = "CMACHINE";
            public static string FrWkCtrH = "CFRWKCTRH";
            public static string ToWkCtrH = "CTOWKCTRH";
            public static string FrOPSeq = "CFROPSEQ";
            public static string FrSubOP = "CFRSUBOP";
            public static string ToOPSeq = "CTOOPSEQ";
            public static string ToSubOP = "CTOSUBOP";
            public static string StartDate = "DSTART";
            public static string FinishDate = "DFINISH";
            public static string Memdata = "CMEMDATA";
            public static string Memdata2 = "CMEMDATA2";
            public static string Memdata3 = "CMEMDATA3";
            public static string Memdata4 = "CMEMDATA4";
            public static string Memdata5 = "CMEMDATA5";
            public static string Locked = "NLOCKED";
            public static string ReceiveQty = "NRECEIVEQT";
            public static string CreateBy = "CCREATEBY";
            public static string CreateDate = "DCREATE";
            public static string LastupdBy = "CLASTUPDBY";
            public static string CancleBy = "CCANCLEBY";
            public static string CancelDate = "DCANCEL";

            public static string FrMOPR = "CFRMOPR";
            public static string ToMOPR = "CTOMOPR";

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


        }

    }

}
