using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QBGTranInfo
    {

        public static string TableName = MapTable.Table.BudTranHD;

        public struct Field
        {

            public static string CorpID = "CCORP";
            public static string BranchID = "CBRANCH";
            public static string SectID = "CSECT";
            public static string DeptID = "CDEPT";
            public static string JobID = "CJOB";
            public static string ProjID = "CPROJ";
            public static string AppEmplID = "CEMPL";
            public static string BudGetType = "CTYPE";
            public static string BudGetYear = "NBGYEAR";

            public static string Code = "CCODE";
            public static string Date = "DDATE";
            public static string Step = "CSTEP";
            public static string Status = "CSTAT";
            public static string Revise = "CREVISE";
            public static string Amount = "NAMT";
            public static string RefType = "CREFTYPE";
            public static string LastUpdate = "DLASTUPDBY";
            public static string LastUpdateBy = "CLASTUPDBY";

            //อนุมัติจาก => หน่วยงานขั้นต้น หรือไม่ ?
            public static string IsApprove = "CISAPPROVE";
            public static string ApproveBy = "CAPPROVEBY";
            public static string ApproveDate = "DAPPROVE";

            //อนุมัติจาก => หน่วยงานงบประมาณขั้นต้น หรือไม่ ?
            public static string IsCorrect = "CISCORRECT";
            public static string CorrectBy = "CCORRECTBY";
            public static string CorrectDate = "DISCORRECT";

            //อนุมัติจาก => หน่วยงานงปม. หรือไม่ ?
            public static string APVStatus = "CAPVSTAT";
            public static string APVRemark = "CAPVREMARK";
            public static string APVBy = "CAPVBY";
            public static string APVDate = "DAPV";

            //อนุมัติจาก => หน่วยงานภายนอก หรือไม่ ?
            public static string IsApprove2 = "CISAPPROV2";
            public static string ApproveBy2 = "CAPPROVEB2";
            public static string ApproveDate2 = "DAPPROVE2";

            //สถานะการ POST รายการ
            public static string IsPost = "CISPOST";
            public static string PostBy = "CPOSTBY";
            public static string PostDate = "DPOST";

        }

    }

}
