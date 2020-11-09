using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class QMFStmReqHDInfo
    {

        public static string TableName = MapTable.Table.StmReqH;
        public static string TableItem = MapTable.Table.StmReqI;

        public struct Field
        {

            //Head
            public static string RowID = "FCSKID";
            public static string LUpdApp = "FCLUPDAPP";
            public static string Rftype = "FCRFTYPE";
            public static string Reftype = "FCREFTYPE";
            public static string CorpID = "FCCORP";
            public static string BranchID = "FCBRANCH";
            public static string DeptID = "FCDEPT";
            public static string SectID = "FCSECT";
            public static string JobID = "FCJOB";
            public static string Stat = "FCSTAT";
            public static string Step = "FCSTEP";
            public static string ACStep = "FCACSTEP";
            public static string GLHead = "FCGLHEAD";
            public static string GLHeadCancel = "FCGLHEADCA";
            public static string Date = "FDDATE";
            public static string ReceDate = "FDRECEDATE";
            public static string DueDate = "FDDUEDATE";
            public static string LayDate = "FDLAYDATE";
            public static string DebtDate = "FDDEBTDATE";
            public static string DebtCode = "FCDEBTCODE";
            public static string BookID = "FCBOOK";
            public static string Code = "FCCODE";
            public static string RefNo = "FCREFNO";
            public static string DiscAmti = "FNDISCAMTI";
            public static string DiscAmt1 = "FNDISCAMT1";
            public static string DiscAmt2 = "FNDISCAMT2";
            public static string DiscAmtA = "FNDISCAMTA";
            public static string DiscPcn1 = "FNDISCPCN1";
            public static string DiscPcn2 = "FNDISCPCN2";
            public static string DiscPcn3 = "FNDISCPCN3";
            public static string Amt = "FNAMT";
            public static string PayAmt = "FNPAYAMT";
            public static string SPayAmt = "FNSPAYAMT";
            public static string BefoAmt = "FNBEFOAMT";
            public static string VATIsOut = "FCVATISOUT";
            public static string VATType = "FCVATTYPE";
            public static string VATRate = "FNVATRATE";
            public static string VATAmt = "FNVATAMT";
            public static string Amt2 = "FNAMT2";
            public static string AmtNoVAT = "FNAMTNOVAT";
            public static string CoorID = "FCCOOR";
            public static string EmplID = "FCEMPL";
            public static string CredTerm = "FNCREDTERM";
            public static string IsCash = "FCISCASH";
            public static string HasRet = "FCHASRET";
            public static string VATDue = "FCVATDUE";
            public static string DebtZero = "FNDEBTZERO";
            public static string LayH = "FCLAYH";
            public static string LaySeq = "FCLAYSEQ";
            public static string FrWHouse = "FCFRWHOUSE";
            public static string ToWHouse = "FCTOWHOUSE";
            public static string CreateBy = "FCCREATEBY";
            public static string CorrectBy = "FCCORRECTB";
            public static string CancelBy = "FCCANCELBY";
            public static string Memdata = "FMMEMDATA";
            public static string Seq = "FCSEQ";
            public static string Promote = "FCPROMOTE";
            public static string VATCoor = "FCVATCOOR";
            public static string RecvMan = "FCRECVMAN";
            public static string Stepx1 = "FCSTEPX1";
            public static string Stepx2 = "FCSTEPX2";
            public static string CreateTy = "FCCREATETY";
            public static string AtStep = "FCATSTEP";
            public static string VATPort = "FNVATPORT";
            public static string VATPortA = "FNVATPORTA";
            public static string Machine = "FCMACHINE";
            public static string Periods = "FCPERIODS";
            public static string Datetime = "FTDATETIME";
            public static string Cashier = "FCCASHIER";
            public static string ApproveBy = "FCAPPROVEB";
            public static string FullFill = "FDFULLFILL";
            public static string ProjID = "FCPROJ";
            public static string Discstr = "FCDISCSTR";
            public static string Currency = "FCCURRENCY";
            public static string XRate = "FNXRATE";
            public static string Amtke = "FNAMTKE";
            public static string VATAmtke = "FNVATAMTKE";
            public static string PayAmtke = "FNPAYAMTKE";
            public static string DiscAmtk = "FNDISCAMTK";
            public static string BPayInv = "FNBPAYINV";
            public static string VATSeq = "FCVATSEQ";
            public static string TimeStamp = "FCTIMESTAM";
            public static string TranWhy = "FCTRANWHY";
            public static string DeliCoor = "FCDELICOOR";
            public static string PlantID = "FCPLANT";
            public static string LastUpd = "FTLASTUPD";
            public static string DocOwner = "FCDOCOWNER";
            public static string RefDO = "FCREFDO";
            public static string Memdata2 = "FMMEMDATA2";
            public static string Memdata3 = "FMMEMDATA3";
            public static string Memdata4 = "FMMEMDATA4";
            public static string Memdata5 = "FMMEMDATA5";
            public static string ShowPled = "FCSHOWPLED";
            public static string PPayAmt = "FNPPAYAMT";
            public static string PPayAmtk = "FNPPAYAMTK";
            public static string VATDate = "FDVATDATE";
            public static string Locked = "FNLOCKED";
            public static string RetenAmt = "FNRETENAMT";
            public static string Tradetrm = "FCTRADETRM";
            public static string DeliEmpl = "FCDELIEMPL";
            public static string Laymethd = "FCLAYMETHD";
            public static string Consign = "FCCONSIGN";
            public static string StepD = "FCSTEPD";
            public static string Receive = "FTRECEIVE";
            public static string Payterm = "FCPAYTERM";
            public static string CreateAp = "FCCREATEAP";

        }


    }
}
