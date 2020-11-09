using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QMFWOrderHDInfo
    {

        public static string TableName = MapTable.Table.MFWOrderHead;

        public struct Field
        {

            //Head
            public static string RowID = MapTable.ShareField.RowID;
            public static string RFType = "CRFTYPE";
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
            public static string PRStep = "CPRSTEP";
            public static string CoorID = "CCOOR";
            public static string Code = "CCODE";
            public static string RefNo = "CREFNO";
            public static string Date = "DDATE";
            public static string DueDate = "DDUEDATE";
            public static string Priority = "CPRIORITY";
            public static string MfgBookID = "CMFGBOOK";
            public static string MfgProdID = "CMFGPROD";
            public static string BOMID = "CBOMHD";
            public static string MfgQty = "NMFGQTY";
            public static string RefQty = "NREFQTY";
            public static string Scrap = "CSCRAP";
            public static string ScrapQty = "NSCRAPQTY";
            public static string Qty = "NQTY";
            public static string MfgUMID = "CMFGUM";
            public static string MfgUMQty = "NMUMQTY";
            public static string StartDate = "DSTART";
            public static string FinishDate = "DFINISH";
            public static string ResourceID = "CMACHINE";
            public static string WkCtrID = "CWKCTRH";
            public static string Memdata = "CMEMDATA";
            public static string Memdata2 = "CMEMDATA2";
            public static string Memdata3 = "CMEMDATA3";
            public static string Memdata4 = "CMEMDATA4";
            public static string Memdata5 = "CMEMDATA5";
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
            public static string Locked = "NLOCKED";
            public static string ReceiveQty = "NRECEIVEQTY";
            public static string BatchNo = "CBATCHNO";
            public static string BatchRun = "CBATCHRUN";
            public static string Level = "CLEVEL";
            public static string MasterID = "CMASTER";
            public static string ParentID = "CPARENT";
            public static string IsApprove = "CISAPPROVE";
            public static string ApproveBy = "CAPPROVEBY";
            public static string ApproveDate = "DAPPROVE";
            public static string OPStep = "COPSTEP";
            public static string RoundCtrl = "CROUNDCTRL";
            public static string IsPlan = "CISPLAN";

            public static string BOMOutputQty = "NBOMOUTQTY";
            public static string RMQtyFactor1 = "NRMQTYFAC1";
            public static string RMQtyFactor2 = "NRMQTYFAC2";

        }
    
    }

}
