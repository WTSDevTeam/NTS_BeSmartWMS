using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BeSmartMRP.Business.Entity
{

    public class MapTable
    {

        public MapTable() { }

        private static DataSet dtsDataEnv = new DataSet();
        private static DataTable dtrSchema = null;
        private static string mCurrTabName = "";

        public static int GetMaxLength(WS.Data.Agents.cDBMSAgent inSQLUtil , string inTable , string inField)
        {

            int intMaxLength = 1;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLUtil;

            if (mCurrTabName != inTable)
            {
                mCurrTabName = inTable;
                pobjSQLUtil.SQLExec(ref dtsDataEnv, "QSchema", inTable, "select * from " + inTable + " where 0=1", true, ref strErrorMsg);
            }
            if (dtsDataEnv.Tables["QSchema"] != null)
            {
                dtrSchema = dtsDataEnv.Tables["QSchema"];
                intMaxLength = dtrSchema.Columns[inField].MaxLength;
            }
            return intMaxLength;

        }

        public struct ShareField
        {
            public static string fcSkid = "FCSKID";
            public static string RowID = "CROWID";
            public static string Active = "CACTIVE";
            public static string ActiveDate = "DACTIVE";
            public static string CreateBy = "CCREATEBY";
            public static string CreateDate = "DCREATE";
            public static string LastUpdateBy = "CLASTUPDBY";
            public static string LastUpdate = "DLASTUPDBY";
        }

        public struct Table
        {

            public static string Corp = "CORP";
            public static string Branch = "BRANCH";
            public static string Department = "BGSECT";
            public static string Division = "BGDEPT";
            public static string Job = "BGJOB";
            public static string Project = "BGPROJ";
            public static string Coor = "COOR";
            public static string CoorGroup = "CRGRP";
            public static string ProdGroup = "PDGRP";
            public static string FMBook = "BOOK";
            public static string Currency = "CURRENCY";
            public static string VatType = "VATTYPE";
            public static string ProdType = "PRODTYPE";
            public static string Product = "PROD";
            public static string UOM = "UM";
            public static string Employee = "EMPL";
            public static string RefPdX3 = "REFPDX3";
            public static string PdSer = "PDSER";
            public static string RefType = "REFTYPE";
            public static string Stock = "STOCK";
            public static string PdBranch = "PDBRANCH";
            public static string AsGrp = "ASGRP";
            public static string AcChart = "ACCHART";
            public static string Bank = "BANK";
            public static string CrZone = "CRZONE";
            public static string Formula = "FORMULA";
            public static string PdStruct = "PDSTRUCT";
            public static string EmplR = "EMPLR";

            public static string xaStock = "XASTOCK";
            public static string xaPdSer = "XAPDSER";
            public static string xaRefPdX3 = "XAREFPDX3";

            public static string OrderH = "ORDERH";
            public static string OrderI = "ORDERI";
            public static string NoteCut = "NOTECUT";

            public static string GLRef = "GLREF";
            public static string RefProd = "REFPROD";

            public static string StmReqH = "STMREQH";
            public static string StmReqI = "STMREQI";

            public static string WHouse = "WHOUSE";
            public static string WHPack = "WHPACK";
            public static string PdBarCode = "PDBARCODE";

            public static string AppObj = "APPOBJ";
            public static string KeepLog = "KEEPLOG";
            public static string MasterDept = "BGDEPT";
            public static string MasterSect = "BGSECT";
            public static string MasterUM = "MASTER_UM";
            public static string MasterPdGrp = "MASTER_PDGRP";
            public static string MasterProd = "MASTER_PROD";
            public static string MasterCoor = "MASTER_COOR";
            public static string MasterCrGrp = "MASTER_CRGRP";
            public static string MasterAcChart = "MASTER_ACCHART";

            public static string MasterCoorX1 = "MASTER_COORX1";
            public static string MasterProdX1 = "MASTER_PRODX1";

            public static string PdCateg = "PDCATEG";
            public static string PdClass = "PDCLASS";
            public static string PdContent = "PDCONTENT";

            public static string BizType = "BIZTYPE";
            public static string Province = "PROVINCE";
            public static string Amphur = "AMPHUR";
            public static string Tumbon = "TUMBON";

            public static string WsBook = "WSBOOK";

            public static string EMCorp = "CORP";
            public static string EMBranch = "BRANCH";
            public static string EMPlant = "EMPLANT";
            public static string EMProj = "PROJ";
            public static string EMJob = "JOB";
            public static string EMDept = "DEPT";
            public static string EMSect = "SECT";
            public static string EMAcChart = "ACCHART";
            public static string EMUOM = "UM";
            public static string EMWHouseLocation = "WHLOCATION";

            public static string MfgBook = "MFGBOOK";

            public static string MFStdOpr = "MFSTDOPR";
            public static string MFResType = "MFRESTYPE";
            public static string MFResource = "MFRESOURCE";
            public static string MFWorkCenter = "MFWKCTRHD";
            public static string MFWorkCenterItem_Res = "MFWKCTRIT_RES";

            public static string MFBOMHead = "MFBOMHD";
            public static string MFBOMIT_PD = "MFBOMIT_PD";
            public static string MFBOMIT_OP = "MFBOMIT_STDOP";

            public static string MFWOrderHead = "MFWORDERHD";
            public static string MFWOrderIT_PD = "MFWORDERIT_PD";
            public static string MFWOrderIT_OP = "MFWORDERIT_STDOP";
            public static string MFWOrderIT_OPPlan = "MFWORDERIT_OPPLAN";

            public static string MFWCTranHead = "MFWCTRANHD";
            public static string MFWCTranIT = "MFWCTRANIT";

            public static string MFWCloseHead = "MFWCLOSEHD";
            public static string MFWCloseIT_PD = "MFWCLOSEIT_PD";
            public static string MFWCloseIT_OP = "MFWCLOSEIT_STDOP";

            public static string AppRole = "APPROLE";
            public static string AppEmpl = "APPEMPL";
            public static string AppLogIn = "APPLOGIN";
            public static string AppEmRole = "APPEMROLE";
            public static string AppAuthX = "APPAUTHX";
            public static string AppAuthDet = "APPAUTHDET";

            public static string BudgetBalance = "BGBALANCE";
            public static string BudgetBook = "BGBOOK";
            public static string BudgetYear = "BGYEAR";
            public static string BudgetType = "BGTYPE";
            public static string BudgetChart = "BGCHARTHD";
            public static string BudgetChartIT = "BGCHARTIT";

            public static string BudAllocate = "BGALLOCHD";
            public static string BudAllocateIT = "BGALLOCIT";

            public static string BudTranHD = "BGTRANHD";
            public static string BudTranIT = "BGTRANIT";

            public static string BudRecvHD = "BGRECVHD";
            public static string BudRecvIT = "BGRECVIT";

            public static string BudUseHD = "BGUSEHD";
            public static string BudUseIT = "BGUSEIT";

            public static string RefDoc = "REFDOC";
            public static string RefDoc_Stmove = "REFDOC_STMOVE";

            public static string CostLine = "COSTLINE";

            public static string PDCapHD = "MFPDCAPHD";
            public static string PDCapIT = "MFPDCAPIT";

            public static string MFPlanItem= "MFPLANITEM";

            public static string EMHoliday = "EMHOLIDAY";
            public static string EMHolidayItem = "EMHOLIDAYIT";

            public static string EMWorkHour = "EMWORKHOUR";
            public static string EMWorkHourItem = "EMWORKHOURIT";
            public static string EMWorkHourItem_Range = "EMWORKHOURIT_RANGE";

            public static string EMWorkCalendar = "EMWORKCALENDAR";
            public static string EMWorkCalendarItem = "EMWORKCALENDARIT";
            public static string EMWorkCalendarItem_Range = "EMWORKCALENDARIT_RANGE";

        }


    }


}
