
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.Linq;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using BeSmartMRP.DatabaseForms;

namespace BeSmartMRP.Transaction.Common.MRP
{

    public class cGenFG
    {

        public cGenFG(string inWOrderH, string inWOrderI, string inWOrderOP, string inBranch, string inPlant, string inBook, string inWHouse, string inProd, decimal inQty, string inMfgUM, decimal inUOMQty, string inLot, string inSect, string inJob, DateTime inDate)
        {

            this.mstrWOrderH = inWOrderH;
            this.mstrWOrderI = inWOrderI;
            this.mstrWOrderOP = inWOrderOP;

            this.mstrGenFG_Branch = inBranch;
            this.mstrGenFG_Plant = inPlant;
            this.mstrGenFG_Book = inBook;
            this.mstrGenFG_WHouse = inWHouse;
            this.mstrGenFG_Lot = inLot;
            this.mdttGenFG_Date = inDate;
            this.mstrGenFG_Prod = inProd;
            this.mstrGenFG_MfgUM = inMfgUM;
            this.mdecGenFG_Qty = inQty;
            this.mdecGenFG_UOMQty = inUOMQty;

            //Sect , Job ดูตาม Work Center ของแต่ละ OP
            this.mstrGenFG_Sect = inSect;
            this.mstrGenFG_Job = inJob;

            this.dtsDataEnv.CaseSensitive = true;
            this.pmCreateTem();

        }

        private string mstrSaveGLRef = "";
        private string mstrSaveRefProd = "";
        private bool mbllGenComplete = false;

        public bool GenComplete
        {
            get { return this.mbllGenComplete; }
        }

        public string SaveGLRef
        {
            get { return this.mstrSaveGLRef; }
        }

        public string SaveRefProd
        {
            get { return this.mstrSaveRefProd; }
        }

        private string mstrWOrderH = "";
        private string mstrWOrderI = "";
        private string mstrWOrderOP = "";
        private string mstrWOrderOP_Seq = "";
        private string mstrEditRowID_PR = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrRefType = DocumentType.FR.ToString();
        private string mstrRfType = "G";

        private string mstrStep = SysDef.gc_REF_STEP_CUT_STOCK;

        private DataSet dtsDataEnv = new DataSet();

        private StockAgent mStockAgent = new StockAgent(App.ERPConnectionString, App.ConnectionString, App.DatabaseReside);

        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        private string mstrHTable = MapTable.Table.GLRef;
        private string mstrEditRowID = "";

        private string mstrTemPd_GenIssue1 = "TemPd_GenIssue1";
        private string mstrTemPd_GenIssue = "TemPd_GenIssue";
        private string mstrTemPd = "TemPd";
        private string mstrTemStock = "TemStock";

        public static string xd_TemHistory_BakRev = "TemBackRev";
        public static string xd_TemHistory_WHBal = "TemWHBal";

        private string mstrGenFG_Branch = "";
        private string mstrGenFG_Plant = "";
        private string mstrGenFG_Book = "";
        private string mstrGenFG_QcBook = "";
        private string mstrGenFG_WHouse = "";
        private string mstrGenFG_ToWHouse = "";
        private string mstrGenFG_Lot = "";
        private string mstrGenFG_Coor = "";
        private string mstrGenFG_Code = "";
        private string mstrGenFG_RefNo = "";
        private string mstrGenFG_Sect = "";
        private string mstrGenFG_Dept = "";
        private string mstrGenFG_Job = "";
        private string mstrGenFG_Proj = "";
        private string mstrGenFG_Prod = "";
        private string mstrGenFG_MfgUM = "";
        private decimal mdecGenFG_Qty = 0;
        private decimal mdecGenFG_UOMQty = 0;

        private string mstrRefToRefType = DocumentType.MO.ToString();
        private string mstrRefToTab = QMFWOrderHDInfo.TableName;
        private string mstrRefToBook = "";
        private string mstrRefToRowID = "";
        private decimal mdecRefToAmt = 0;
        private string mstrRefToMOrderOP = "";
        private string mstrRefToWOrderI = "";
        private string mstrOldRefToRowID = "";
        private string mstrOldRefToMOrderOP = "";

        private DateTime mdttGenFG_Date = DateTime.Now;
        private int mintGenFG_CredTerm = 0;
        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private string mstrCalBook_PR = "";
        private string mstrCalBook_PO = "";

        private string mstrLastRunCode = "";
        private long intRunCode = 0;
        private string mstrCurrQcCoor = "";
        private string mstrCurrCode = "";

        private string mstrFrWHouse = "";
        private string mstrToWHouse = "";
        private string mstrWHouse_RM = "";

        private string mstrFrWhType = "";
        private string mstrToWhType = "";

        private string mstrFrWhType2 = "";
        private string mstrToWhType2 = "";

        private string mstrSaleOrBuyForPdSer = "P";

        private decimal mdecAmt = 0;
        private decimal mdecVatAmt = 0;
        private decimal mdecDiscountAmt = 0;
        private decimal mdecXRate = 1;
        private decimal mdecTotPdQty = 0;
        private decimal mdecTotPdAmt = 0;
        private decimal mdecVatRate = 0;
        private decimal mdecGrossAmt = 0;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemPd_GenIssue);
            dtbTemPd.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cFrQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cToQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nCostAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToHRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cXRefToRefType", System.Type.GetType("System.String"));

            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nCostAmt"].DefaultValue = 0;
            dtbTemPd.Columns["nPrice"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemStk = new DataTable(this.mstrTemStock);
            dtbTemStk.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("nUseQty", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("nBalQty", System.Type.GetType("System.Decimal"));

            dtbTemStk.Columns["cRowID"].DefaultValue = "";
            dtbTemStk.Columns["cQcProd"].DefaultValue = "";
            dtbTemStk.Columns["cQcWHouse"].DefaultValue = "";
            dtbTemStk.Columns["cLot"].DefaultValue = "";
            dtbTemStk.Columns["nQty"].DefaultValue = 0;
            dtbTemStk.Columns["nUseQty"].DefaultValue = 0;
            dtbTemStk.Columns["nBalQty"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemStk);

            DataTable dtbTemGen1Issue = new DataTable(this.mstrTemPd_GenIssue1);
            dtbTemGen1Issue.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cFrWHouse", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cToWHouse", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemGen1Issue.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));
            
            this.dtsDataEnv.Tables.Add(dtbTemGen1Issue);

        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private bool mbllStockOnly = false;
        public void GenFG()
        {

            //this.mbllStockOnly = inStockOnly;

            this.mstrFrWhType = SysDef.gc_WHOUSE_TYPE_WIP;
            this.mstrToWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;
            this.mstrSaleOrBuyForPdSer = "B";

            this.mbllGenComplete = false;
            this.pmGenFG();
        }

        private void pmGenFG()
        {

            this.mStockAgent.CorpID = App.ActiveCorp.RowID;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFrWHouse = "";
            string strToWHouse = "";
            string strFrQcWHouse = "";
            string strToQcWHouse = "";

            objSQLHelper2.SetPara(new object[] { this.mstrGenFG_Book });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QBook", MapTable.Table.WHouse, "select * from BOOK where fcSkid = ?", ref strErrorMsg))
            {
                this.mstrGenFG_QcBook = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
            }

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrGenFG_Branch, SysDef.gc_WHOUSE_TYPE_WIP });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QWHouse", MapTable.Table.WHouse, "select * from " + MapTable.Table.WHouse + " where fcCorp = ? and fcBranch = ? and fcType = ? order by fcCode", ref strErrorMsg))
            {
                strFrWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                strFrQcWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.mstrFrWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper2.SetPara(new object[] { this.mstrGenFG_WHouse });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QWHouse", MapTable.Table.WHouse, "select * from " + MapTable.Table.WHouse + " where fcSkid = ? ", ref strErrorMsg))
            {
                strToWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                strToQcWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.mstrToWHouse = strToWHouse;
            }

            DataRow dtrLoadHead = null;

            string strQcProd = "";
            string strPdType = "";
            string strUM = "";

            objSQLHelper2.SetPara(new object[] { this.mstrGenFG_Prod });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QProd", MapTable.Table.Product, "select * from " + MapTable.Table.Product + " where fcSkid = ? ", ref strErrorMsg))
            {
                strPdType = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcType"].ToString().TrimEnd();
                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                strUM = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcUM"].ToString();
            }

            this.pmRunCode_FG();
            this.pmInsertRow(this.mstrWOrderI, this.mstrWOrderOP, this.mstrGenFG_Prod, strQcProd, strPdType, this.mstrGenFG_MfgUM, strUM, this.mstrWOrderOP_Seq, StringHelper.ConvertToBase64(1, 2), strFrWHouse, strFrQcWHouse, strToWHouse, strToQcWHouse, this.mstrGenFG_Lot, this.mdecGenFG_Qty, this.mdecGenFG_UOMQty, this.mstrGenFG_Sect, this.mstrGenFG_Job);

            string strErr2 = "";
            if (this.pmSaveData(ref strErr2))
            {
                this.mbllGenComplete = true;
                MessageBox.Show("Gen Finish Good Receive Complete !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.mbllGenComplete = false;
                if (strErr2 != "")
                    MessageBox.Show(strErr2, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool pmRunCode_FG()
        {
            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            pAPara = new object[4] { App.ActiveCorp.RowID, this.mstrGenFG_Branch, this.mstrRefType, this.mstrGenFG_Book };
            strSQLStr = "select fcCode from GLREF where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";

            if (mstrLastRunCode == "")
            {
                pobjSQLUtil.SetPara(pAPara);
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
                {
                    strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["fcCode"].ToString().Trim();
                    try
                    {
                        intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                    }
                    catch (Exception ex)
                    {
                        strErrorMsg = ex.Message.ToString();
                        intRunCode++;
                    }
                }
                else
                {
                    intRunCode++;
                    strLastRunCode = this.mstrLastRunCode;
                }

            }
            else
            {

                try
                {
                    intRunCode = Convert.ToInt64(this.mstrLastRunCode) + 1;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    intRunCode++;
                }
                
            }

            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : 7);
            this.mstrGenFG_Code = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            this.mstrGenFG_RefNo = this.mstrRefType + this.mstrGenFG_QcBook + "/" + intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            this.mstrLastRunCode = this.mstrGenFG_Code;

            return true;
        }

        private bool pmSaveData(ref string ioErrorMsg)
        {
            bool bllSucc = false;
            ioErrorMsg = "";
            string strErrorMsg = "";
            this.mstrCurrQcCoor = "";
            if (this.pmChkDupCode(this.mstrGenFG_Code))
            {
                ioErrorMsg = UIBase.GetAppUIText(new string[] { "เลขที่เอกสารซ้ำ : ", "Duplicate Document Code : " }) + this.mstrGenFG_Code;
                MessageBox.Show(ioErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bllSucc = false;
            }
            else
            {
                this.mstrCurrCode = this.mstrGenFG_Code;
                this.mstrRefToRowID = this.mstrWOrderH;
                this.mstrRefToMOrderOP = this.mstrWOrderOP;

                this.pmSaveGLRef(this.mstrFrWHouse, this.mstrToWHouse, this.mstrGenFG_Sect, this.mstrGenFG_Job);
                bllSucc = true;
            }
            return bllSucc;
        }

        private bool pmChkDupCode(string inCode)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrGenFG_Branch, "WR", this.mstrGenFG_Book, inCode };
            strSQLStr = "select fcCode from OrderH where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                bllResult = true;
            }
            return bllResult;
        }

        private void pmSaveGLRef(string inFrWHouse, string inToWHouse, string inSect, string inJob)
        {

            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where 0=1", ref strErrorMsg);
            DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrHTable].NewRow();

            if (true)
            {
                bllIsNewRow = true;
                WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                this.mstrEditRowID = objConn.RunRowID(this.mstrHTable);
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateAp] = App.AppID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.CreateBy] = App.FMAppUserID;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Datetime] = objSQLHelper.GetDBServerDateTime();
                this.dtsDataEnv.Tables[this.mstrHTable].Rows.Add(dtrSaveInfo);
            }

            string strCode = this.mstrGenFG_Code.TrimEnd();
            if (strCode.Length > 3)
                strCode = StringHelper.Left(strCode, strCode.Length - 3);

            string strDept = "";
            string strProj = "";

            objSQLHelper.SetPara(new object[] { inSect });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from SECT where fcSkid = ?", ref strErrorMsg))
            {
                strDept = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString();
            }

            objSQLHelper.SetPara(new object[] { inJob });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
            {
                strProj = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString();
            }

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[QMFStmoveHDInfo.Field.RowID] = this.mstrEditRowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.CorrectBy] = App.FMAppUserID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.LastUpd] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QMFStmoveHDInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BranchID] = this.mstrGenFG_Branch;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Reftype] = this.mstrRefType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Rftype] = this.mstrRfType;
            dtrSaveInfo[QMFStmoveHDInfo.Field.BookID] = this.mstrGenFG_Book;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Step] = SysDef.gc_STEP_IGNORE;
            dtrSaveInfo[QMFStmoveHDInfo.Field.Code] = this.mstrGenFG_Code.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.RefNo] = this.mstrGenFG_RefNo.TrimEnd();
            dtrSaveInfo[QMFStmoveHDInfo.Field.Date] = this.mdttGenFG_Date.Date;
            dtrSaveInfo[QMFStmoveHDInfo.Field.FrWHouse] = inFrWHouse;
            dtrSaveInfo[QMFStmoveHDInfo.Field.ToWHouse] = inToWHouse;
            dtrSaveInfo[QMFStmoveHDInfo.Field.SectID] = inSect;
            dtrSaveInfo[QMFStmoveHDInfo.Field.DeptID] = strDept;
            dtrSaveInfo[QMFStmoveHDInfo.Field.JobID] = inJob;
            dtrSaveInfo[QMFStmoveHDInfo.Field.ProjID] = strProj;
            dtrSaveInfo[QMFStmoveHDInfo.Field.LUpdApp] = App.AppID;
            dtrSaveInfo["fcDataser"] = "";	//fcDataser
            dtrSaveInfo["fcEAfterR"] = "E";

            string gcTemStr01 = BizRule.SetMemData("", "Rem");
            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata] = gcTemStr02;
            if (DataSetHelper.HasField("fmMemData2", dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata2] = gcTemStr03;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata3] = gcTemStr04;
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata4] = gcTemStr05;
            }

            if (DataSetHelper.HasField(QMFStmoveHDInfo.Field.Memdata5, dtrSaveInfo))
            {
                dtrSaveInfo[QMFStmoveHDInfo.Field.Memdata5] = gcTemStr06;
            }

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent2.AppID = App.AppID;
            this.mdbConn2 = this.mSaveDBAgent2.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.mdbConn2.Open();
                this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mstrRefToWOrderI = "";

                decimal decDiscAmtI = 0;

                this.pmSaveRefProd(this.mstrEditRowID, ref decDiscAmtI);

                this.pmUpdateStockForInsert();

                dtrSaveInfo["fnDiscAmtI"] = decDiscAmtI;

                this.pmSaveRefTo();

                this.mdbTran.Commit();
                this.mdbTran2.Commit();
                bllIsCommit = true;

                //KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Insert, "", this.txtCode.Text, this.txtRefNo.Text, App.FMAppUserID, App.AppUserName);

            }
            catch (Exception ex)
            {
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                    this.mdbTran2.Rollback();
                }
                App.WriteEventsLog(ex);

#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif

            }

            finally
            {
                this.mdbConn.Close();
            }

        }

        private bool pmSaveRefProd(string ioErrorMsg, ref decimal ioDiscAmtI)
        {
            bool bllResult = true;
            string strRowID = "";
            string strOutRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Select("cCode = '" + this.mstrCurrCode + "' and nQty > 0");
            for (int i = 0; i < dtrSel.Length; i++)
            {

                DataRow dtrTemPd = dtrSel[i];

                bool bllIsNewRow = false;

                decimal decQty = 0;
                decimal decUmQty = 0;
                decimal decStQty = 0;
                string strWHouse = "";
                DataRow dtrRRefProd = null;

                #region "Save รายการฝั่ง Out"

                strRowID = App.mRunRowID("RefProd");
                bllIsNewRow = true;
                decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                decStQty = 0;

                //รายการฝั่ง Out
                dtrTemPd["cRowID"] = strRowID;
                strOutRowID = strRowID;
                DataRow dtrRefProd = null;
                this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, false, decQty, decUmQty, 0, 0, i);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                #endregion

                #region "Save รายการฝั่ง In"

                strRowID = App.mRunRowID("RefProd");
                bllIsNewRow = true;
                decQty = Convert.ToDecimal(dtrTemPd["nQty"]);
                decUmQty = Convert.ToDecimal(dtrTemPd["nUOMQty"]);
                decStQty = 0;

                //รายการฝั่ง IN
                dtrTemPd["cRowID2"] = strRowID;
                dtrRefProd = null;
                this.pmReplRecordRefProd(bllIsNewRow, dtrTemPd, ref dtrRefProd, strRowID, true, decQty, decUmQty, 0, 0, i);

                strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                #endregion

                this.pmSaveRefDoc(dtrTemPd);

            }

            return true;
        }

        private bool pmReplRecordRefProd
            (
            bool inState, DataRow inTemPd, ref DataRow ioRefProd, string inRowID,
            bool inToWHouse, decimal inQty, decimal inUmQty, decimal inStQty, decimal inStUmQty, Int32 inRun
            )
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrRefProd;
            if (bllIsNewRec)
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].NewRow();
                dtrRefProd["fcSkid"] = inRowID;
                dtrRefProd["fcCreateAp"] = App.AppID;
            }
            else
            {
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
            }

            string strWHouse = "";
            string strIOType = "";
            if (inToWHouse)
            {
                strWHouse = this.mstrToWHouse;
                strIOType = "I";
            }
            else
            {
                //strWHouse = this.txtFrQcWHouse.Tag.ToString();
                strWHouse = this.mstrFrWHouse;
                strIOType = "O";
            }

            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];

            if (strIOType == "I")
            {
                this.mstrSaveGLRef = dtrGLRef["fcSkid"].ToString();
                this.mstrSaveRefProd = dtrRefProd["fcSkid"].ToString();
            }

            //string strRemark1 = inTemPd["cRemark1"].ToString().TrimEnd();
            //string strRemark2 = inTemPd["cRemark2"].ToString().TrimEnd();
            //string strRemark3 = inTemPd["cRemark3"].ToString().TrimEnd();
            //string strRemark4 = inTemPd["cRemark4"].ToString().TrimEnd();
            //string strRemark5 = inTemPd["cRemark5"].ToString().TrimEnd();
            //string strRemark6 = inTemPd["cRemark6"].ToString().TrimEnd();
            //string strRemark7 = inTemPd["cRemark7"].ToString().TrimEnd();
            //string strRemark8 = inTemPd["cRemark8"].ToString().TrimEnd();
            //string strRemark9 = inTemPd["cRemark9"].ToString().TrimEnd();
            //string strRemark10 = inTemPd["cRemark10"].ToString().TrimEnd();

            dtrRefProd["fcCorp"] = App.ActiveCorp.RowID;
            dtrRefProd["fcBranch"] = this.mstrGenFG_Branch;
            dtrRefProd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
            dtrRefProd["fcRefType"] = dtrGLRef["fcRefType"].ToString();
            dtrRefProd["fcRfType"] = dtrGLRef["fcRfType"].ToString();
            dtrRefProd["fcStat"] = dtrGLRef["fcStat"].ToString();
            dtrRefProd["fdDate"] = Convert.ToDateTime(dtrGLRef["fdDate"]).Date;
            dtrRefProd["fcRefPdTyp"] = "P";
            dtrRefProd["fcProdType"] = inTemPd["cPdType"].ToString().TrimEnd();
            dtrRefProd["fcRootSeq"] = "";
            dtrRefProd["fcShowComp"] = "";
            dtrRefProd["fcPformula"] = "";
            dtrRefProd["fcFormulas"] = "";
            dtrRefProd["fcProd"] = inTemPd["cProd"].ToString();
            //dtrRefProd["fmRemark"] = strRemark1;
            //dtrRefProd["fmRemark2"] = strRemark2;
            //dtrRefProd["fmRemark3"] = strRemark3;
            //dtrRefProd["fmRemark4"] = strRemark4;
            //dtrRefProd["fmRemark5"] = strRemark5;
            //dtrRefProd["fmRemark6"] = strRemark6;
            //dtrRefProd["fmRemark7"] = strRemark7;
            //dtrRefProd["fmRemark8"] = strRemark8;
            //dtrRefProd["fmRemark9"] = strRemark9;
            //dtrRefProd["fmRemark10"] = strRemark10;
            dtrRefProd["fcUm"] = inTemPd["cUOM"].ToString();
            dtrRefProd["fcLot"] = inTemPd["cLot"].ToString().TrimEnd();
            dtrRefProd["fcWhouse"] = strWHouse;
            dtrRefProd["fcIoType"] = strIOType;
            dtrRefProd["fcSect"] = dtrGLRef["fcSect"].ToString();
            dtrRefProd["fcDept"] = dtrGLRef["fcDept"].ToString();
            dtrRefProd["fcJob"] = dtrGLRef["fcJob"].ToString();
            dtrRefProd["fcProj"] = dtrGLRef["fcProj"].ToString();
            //dtrRefProd["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]);
            //dtrRefProd["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            //dtrRefProd["fnXRate"] = 1;
            //dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrRefProd["fcSeq"] = StringHelper.ConvertToBase64(inRun, 2);

            //if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            //{
            //    this.pmUpdateStock(inQty, inUmQty, inStQty, inStUmQty, ref dtrRefProd, inTemPd);
            //    string strWHLoca = "";
            //    if (!bllIsNewRec)
            //        this.pmRetStock(dtrRefProd, strWHLoca, true, false, "", ref strErrorMsg);
            //}

            dtrRefProd["fcLUpdApp"] = App.AppID;
            dtrRefProd["fcDataser"] = "";
            dtrRefProd["fcEAfterR"] = 'E';

            dtrRefProd["fnQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrRefProd["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrRefProd["fcUmStd"] = inTemPd["cUOMStd"].ToString();

            //dtrRefProd["fnStQty"] = Convert.ToDecimal(inTemPd["nStQty"]);
            //dtrRefProd["fcStUm"] = inTemPd["cUOMStd"].ToString();
            //dtrRefProd["fnStUmQty"] = Convert.ToDecimal(inTemPd["nStUOMQty"]);
            //dtrRefProd["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

            ioRefProd = dtrRefProd;
            return true;
        }

        private void pmSaveRefDoc(DataRow inTemPd)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != inTemPd["cXRefToRowID"].ToString().TrimEnd()
                && inTemPd["cXRefToRowID"].ToString().TrimEnd() != string.Empty)
            {
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), inTemPd["cXRefToRefType"].ToString(), inTemPd["cXRefToHRowID"].ToString(), inTemPd["cXRefToRowID"].ToString() };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }

            if (inTemPd["cRefToRowID"].ToString().TrimEnd() != string.Empty
                && Convert.ToDecimal(inTemPd["nQty"]) != 0)
            {
                bool bllIsNewRow_RefDoc = false;
                DataRow dtrRefDoc = null;
                string strRowID = "";
                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString(), this.mstrRefToRefType, inTemPd["cRefToHRowID"].ToString(), inTemPd["cRefToRowID"].ToString() };
                string strRefDoc = "select * from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ?";
                if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefDoc_Stmove, MapTable.Table.RefDoc_Stmove, strRefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                {
                    strRowID = App.mRunRowID(MapTable.Table.RefDoc_Stmove);
                    bllIsNewRow_RefDoc = true;
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].NewRow();
                    dtrRefDoc["cRowID"] = strRowID;
                    //dtrRefDoc["cCreateAp"] = App.AppID;
                }
                else
                {
                    strRowID = inTemPd["cRefToRowID"].ToString();
                    dtrRefDoc = this.dtsDataEnv.Tables[MapTable.Table.RefDoc_Stmove].Rows[0];
                }
                dtrRefDoc["cMastertyp"] = this.mstrRefType;
                dtrRefDoc["cMasterH"] = this.mstrEditRowID;
                dtrRefDoc["cMasterI"] = inTemPd["cRowID"].ToString();
                dtrRefDoc["cCorp"] = App.ActiveCorp.RowID;
                dtrRefDoc["cBranch"] = this.mstrGenFG_Branch;
                dtrRefDoc["cPlant"] = this.mstrGenFG_Plant;
                dtrRefDoc["cChildType"] = this.mstrRefToRefType;
                dtrRefDoc["cChildH"] = inTemPd["cRefToHRowID"].ToString();
                dtrRefDoc["cChildI"] = inTemPd["cRefToRowID"].ToString();
                dtrRefDoc["nQty"] = Convert.ToDecimal(inTemPd["nQty"]);
                dtrRefDoc["nUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);

                string strSQLUpdateStr_RefDoc = "";
                cDBMSAgent.GenUpdateSQLString(dtrRefDoc, "cRowID", bllIsNewRow_RefDoc, ref strSQLUpdateStr_RefDoc, ref pAPara);
                this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr_RefDoc, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                //pobjSQLUtil.SetPara(pAPara);
                //pobjSQLUtil.SQLExec(strSQLUpdateStr_RefDoc, ref strErrorMsg);
            }
            else
            {
                //Delete Note Cut
                //pobjSQLUtil.SetPara(new object[] {this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString()});
                //pobjSQLUtil.SQLExec("delete from REFDOC_STMOVE where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", ref strErrorMsg);

                pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, inTemPd["cRowID"].ToString() };
                this.mSaveDBAgent2.BatchSQLExec("delete from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? and CMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
        }

        private bool pmSaveRefTo()
        {

            bool bllResult = true;
            string strErrorMsg = "";
            cDBMSAgent pobjSQLUtil = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            object[] pAPara = null;

            if (this.mstrRefToRowID != this.mstrOldRefToRowID)
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[4] { this.mstrRefType, this.mstrOldRefToRowID, this.mstrRefToRefType, this.mstrRefToRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ? and cChildType = ? and cChildH = ?", ref strErrorMsg);
            }

            if (this.mstrRefToRowID != string.Empty)
            {
                bool bllIsNewRow_RefTo = false;
                string strRowID_RefTo = "";
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "REFDOC", "REFDOC", "select * from REFDOC where 0=1", ref strErrorMsg);
                DataRow dtrREFDOC = this.dtsDataEnv.Tables["REFDOC"].NewRow();

                if (pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID })
                    && !pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QChkRefTo", "REFDOC", "select cRowID from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
                {
                    strRowID_RefTo = App.mRunRowID("REFDOC");
                    bllIsNewRow_RefTo = true;
                }
                else
                {
                    strRowID_RefTo = this.dtsDataEnv.Tables["QChkRefTo"].Rows[0]["cRowID"].ToString();
                }

                dtrREFDOC["cRowID"] = strRowID_RefTo;
                dtrREFDOC["cCorp"] = App.ActiveCorp.RowID;
                dtrREFDOC["cBranch"] = this.mstrGenFG_Branch;
                dtrREFDOC["cPlant"] = this.mstrGenFG_Plant;
                dtrREFDOC["cMasterTyp"] = this.mstrRefType;
                dtrREFDOC["cMasterH"] = this.mstrEditRowID;
                dtrREFDOC["cChildType"] = this.mstrRefToRefType;
                dtrREFDOC["cChildH"] = this.mstrRefToRowID;
                dtrREFDOC["cChildI"] = this.mstrRefToMOrderOP;

                string strSQLUpdateStr_RefTo = "";
                DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
                pobjSQLUtil.SetPara(pAPara);
                bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);

            }
            else
            {
                //delete REFDOC
                pobjSQLUtil.SetPara(new object[2] { this.mstrRefType, this.mstrEditRowID });
                pobjSQLUtil.SQLExec("delete from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg);

            }

            //this.pmUpdateWOrderOPStep(this.mstrRefToRowID);
            return bllResult;
        }

        private bool pmUpdateStockForInsert()
        {
            string strErrorMsg = "";
            string strSQLUpdateStr = "";
            DataRow dtrGLRef = this.dtsDataEnv.Tables[this.mstrHTable].Rows[0];
            DataRow dtrRefProd = null;
            bool llSucc = true;
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Select("cCode = '" + this.mstrCurrCode + "' and nQty > 0");
            for (int i = 0; i < dtrSel.Length; i++)
            {

                DataRow dtrTemPd = dtrSel[i];

                dtrRefProd = null;
                //รายการฝั่ง Out
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

                dtrRefProd = null;
                //รายการฝั่ง In
                if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefProd, "RefProd", "select * from " + MapTable.Table.RefProd + " where fcSkid = ?", new object[1] { dtrTemPd["cRowID2"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    dtrRefProd = this.dtsDataEnv.Tables[MapTable.Table.RefProd].Rows[0];
                }
                if (dtrRefProd != null)
                {
                    object[] pAPara = null;

                    llSucc = this.pmUpdateStock(Convert.ToDecimal(dtrRefProd["fnQty"]), Convert.ToDecimal(dtrRefProd["fnUmQty"]), Convert.ToDecimal(dtrRefProd["fnStQty"]), Convert.ToDecimal(dtrRefProd["fnStUmQty"]), ref dtrRefProd, dtrTemPd);
                    cDBMSAgent.GenUpdateSQLString(dtrRefProd, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                }

            }
            return llSucc;
        }

        private bool pmUpdateStock(decimal tninQty, decimal tninUmQty, decimal tninStQty, decimal tninStUmQty, ref DataRow ioRefProd, DataRow inTemPd)
        {
            string strErrorMsg = "";
            bool bllSucc = false;
            decimal lnQty = tninQty;
            decimal lnUmQty = tninUmQty;
            decimal lnStQty = tninStQty;
            decimal lnStUmQty = tninStUmQty;
            decimal lnAvgCost = 0;
            decimal lnCostAmt = 0;

            DataRow dtrRefProd = ioRefProd;
            DataRow dtrTemPd = inTemPd;

            decimal lnUpdStockSign = (dtrRefProd["fcIoType"].ToString() == "I" ? 1 : -1);
            string lcCtrlStock = BizRule.GetProdCtrlStock(this.mSaveDBAgent, dtrRefProd["fcProd"].ToString(), App.ActiveCorp.SCtrlStock);

            if (dtrRefProd["fcIoType"].ToString() == "I"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrToWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }
            else if (dtrRefProd["fcIoType"].ToString() == "O"
                && ((SysDef.gc_WHOUSE_TYPE_WIP + SysDef.gc_WHOUSE_TYPE_OFFICE_SUPPLY).IndexOf(this.mstrFrWhType) > -1))
            {
                lcCtrlStock = BusinessEnum.gc_PROD_CTRL_STOCK_NEGATIVE_OK;
            }

            decimal decOutStockQty = 0;
            decimal decOutWHLocaQty = 0;

            string strWHLoca = "";

            dtrRefProd["fnCostAmt"] = (Convert.IsDBNull(dtrRefProd["fnCostAmt"]) ? 0 : this.pmToDecimal(dtrRefProd["fnCostAmt"]));

            if (this.mstrFrWhType == SysDef.gc_WHOUSE_TYPE_NORMAL
                && this.mstrRefType != SysDef.gc_REFTYPE_TRANSFER)
            {
                if (dtrRefProd["fcIoType"].ToString() == "O")
                {
                    lnCostAmt = 0;

                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
lnUpdStockSign
                        , this.mstrGenFG_Branch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
);

                    lnCostAmt = Math.Round(lnAvgCost * this.pmToDecimal(dtrRefProd["fnQty"]) * this.pmToDecimal(dtrRefProd["fnUmQty"]), 4);
                }
                else
                {
                    lnCostAmt = this.pmToDecimal(dtrTemPd["nCostAmt"]);
                    this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                    this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                    bllSucc = this.mStockAgent.UpdateStock
                        (
                    #region "UpdateStock Parameter"
                        lnUpdStockSign
                        , this.mstrGenFG_Branch
                        , dtrRefProd["fcProdType"].ToString()
                        , dtrRefProd["fcProd"].ToString()
                        , dtrRefProd["fcWhouse"].ToString()
                        , strWHLoca
                        , dtrRefProd["fcLot"].ToString()
                        , lcCtrlStock
                        , lnQty
                        , lnUmQty
                        , dtrTemPd["cUOM"].ToString()
                        , dtrTemPd["cQnUOM"].ToString()
                        , lnCostAmt
                        , ref lnAvgCost
                        , lnStQty * lnStUmQty
                        , ref decOutStockQty
                        , ref decOutWHLocaQty
                        , false
                        , false
                        , ""
                        , dtrRefProd["fcIoType"].ToString() == "I"
                        , 0
                        , false
                        , false
                        , ""
                        , ref strErrorMsg
                    #endregion
                        );

                    lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nCostAmt"]), 4);

                }
                if (bllSucc)
                {
                    dtrRefProd["fnPrice"] = (this.pmToDecimal(dtrRefProd["fnQty"]) != 0 ? Math.Round(lnCostAmt / this.pmToDecimal(dtrRefProd["fnQty"]), 4) : 0);
                }
            }
            else
            {
                decimal lnUpdCostAmt = Math.Round(this.pmToDecimal(dtrRefProd["fnPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4) - this.pmToDecimal(dtrRefProd["fnCostAmt"]);	//กรณีแก้ไข เพิ่ม/ลด จำนวน จะ Update เฉพาะจำนวนที่ เพิ่ม/ลด เท่านั้น เหมือน Qty
                this.mStockAgent.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
                this.mStockAgent.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
                bllSucc = this.mStockAgent.UpdateStock
                    (
                #region "UpdateStock Parameter"
                      lnUpdStockSign
                    , this.mstrGenFG_Branch
                    , dtrRefProd["fcProdType"].ToString()
                    , dtrRefProd["fcProd"].ToString()
                    , dtrRefProd["fcWhouse"].ToString()
                    , strWHLoca
                    , dtrRefProd["fcLot"].ToString()
                    , lcCtrlStock
                    , lnQty
                    , lnUmQty
                    , dtrTemPd["cUOM"].ToString()
                    , dtrTemPd["cQnUOM"].ToString()
                    , lnUpdCostAmt
                    , ref lnAvgCost
                    , lnStQty * lnStUmQty
                    , ref decOutStockQty
                    , ref decOutWHLocaQty
                    , false
                    , false
                    , ""
                    , dtrRefProd["fcIoType"].ToString() == "I"
                    , 0
                    , false
                    , false
                    , ""
                    , ref strErrorMsg
                #endregion
);

                lnCostAmt = Math.Round(this.pmToDecimal(dtrTemPd["nPrice"]) * this.pmToDecimal(dtrRefProd["fnQty"]), 4);
            }
            if (bllSucc)
            {
                if (dtrRefProd["fcIOType"].ToString() == "O")
                {
                    dtrTemPd["nCostAmt"] = lnCostAmt;
                }
                dtrRefProd["fnCostAmt"] = lnCostAmt;
                dtrRefProd["fnPriceKe"] = dtrRefProd["fnPrice"];
                dtrRefProd["fnXRate"] = 1;
            }

            return bllSucc;
        }

        private Decimal pmToDecimal(object inConvert)
        {
            Decimal decRetVal = 0;
            //decRetVal = (Convert.IsDBNull(inConvert) ? 0 : Convert.ToDecimal(inConvert));
            if (Convert.IsDBNull(inConvert))
            {
                decRetVal = 0;
            }
            else
            {
                decRetVal = Convert.ToDecimal(inConvert);
            }
            return decRetVal;
        }

        private void pmInsertRow(string inWOrderI, string inWOrderOP, string inProd, string inQcProd, string inPdType, string inMfgUM, string inUM, string inOPSeq, string inSeq, string inFrWHouse, string inFrQcWHouse, string inToWHouse, string inToQcWHouse, string inLot, decimal inQty, decimal inUOMQty, string inSect, string inJob)
        {

            DataRow dtbTemPd = this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].NewRow();

            dtbTemPd["cRefToHRowID"] = this.mstrWOrderH;
            dtbTemPd["cCode"] = this.mstrGenFG_Code;
            dtbTemPd["cRefToRowID"] = inWOrderI;
            dtbTemPd["cWOrderOP"] = inWOrderOP;
            dtbTemPd["cFrWHouse"] = inFrWHouse;
            dtbTemPd["cFrQcWHouse"] = inFrQcWHouse;
            dtbTemPd["cToWHouse"] = inToWHouse;
            dtbTemPd["cToQcWHouse"] = inToQcWHouse;
            dtbTemPd["cProd"] = inProd;
            dtbTemPd["cPdType"] = inPdType;
            dtbTemPd["cQcProd"] = inQcProd;
            dtbTemPd["nQty"] = inQty;
            dtbTemPd["nUOMQty"] = inUOMQty;
            dtbTemPd["cOPSeq"] = inOPSeq;
            dtbTemPd["cSeq"] = inSeq;
            dtbTemPd["cSect"] = inSect;
            dtbTemPd["cJob"] = inJob;
            dtbTemPd["cUOM"] = inMfgUM;
            dtbTemPd["cUOMStd"] = inUM;
            dtbTemPd["cLot"] = inLot;

            this.dtsDataEnv.Tables[this.mstrTemPd_GenIssue].Rows.Add(dtbTemPd);
        }

    }

}
