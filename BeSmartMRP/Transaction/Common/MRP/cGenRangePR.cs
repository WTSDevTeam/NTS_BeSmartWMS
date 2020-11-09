
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
    public class cGenRangePR
    {

        public cGenRangePR(string inBranch, string inPlant, string inBook, string inCoor, DateTime inDate)
        {

            this.mstrGenPR_Branch = inBranch;
            this.mstrGenPR_Plant = inPlant;
            this.mstrGenPR_Book = inBook;
            this.mstrGenPR_Coor = inCoor;
            this.mdttGenPR_Date = inDate;

            this.dtsDataEnv.CaseSensitive = true;
            this.pmCreateTem();

        }

        public string GenPR_MOBook
        {
            set { this.mstrGenPR_MOBook = value; }
        }

        public string GenPR_OrderBy
        {
            set { this.mstrGenPR_OrderBy = value; }
        }

        public string GenPR_BegCode
        {
            set { this.mstrGenPR_BegCode = value; }
        }

        public string GenPR_EndCode
        {
            set { this.mstrGenPR_EndCode = value; }
        }

        public DateTime GenPR_BegDate
        {
            set { this.mdttGenPR_BegDate = value; }
        }

        public DateTime GenPR_EndDate
        {
            set { this.mdttGenPR_EndDate = value; }
        }

        private string mstrWOrderH = "";
        private string mstrEditRowID_PR = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrDefaSect = "";
        private string mstrDefaJob = "";
        private string mstrStep = SysDef.gc_REF_STEP_CUT_STOCK;

        private DataSet dtsDataEnv = new DataSet();

        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        private string mstrTemPd_GenPR1 = "TemPd_GenPR1";
        private string mstrTemPd_GenPR = "TemPd_GenPR";

        public static string xd_TemHistory_BakRev = "TemBackRev";
        public static string xd_TemHistory_WHBal = "TemWHBal";

        private string mstrGenPR_Branch = "";
        private string mstrGenPR_Plant = "";
        private string mstrGenPR_Book = "";
        private string mstrGenPR_Coor = "";
        private string mstrGenPR_Code = "";
        private string mstrGenPR_RefNo = "";
        private string mstrGenPR_Sect = "";
        private string mstrGenPR_Dept = "";
        private string mstrGenPR_Job = "";
        private string mstrGenPR_Proj = "";

        private string mstrCurrWOrderH = "";
        private string mstrSave_RefNo = "";

        private string mstrQcBOM = "";
        private string mstrQnBOM = "";

        private DateTime mdttGenPR_Date = DateTime.Now;
        private string mstrGenPR_MOBook = "";
        private string mstrGenPR_OrderBy = "DDATE";
        private DateTime mdttGenPR_BegDate = DateTime.Now;
        private DateTime mdttGenPR_EndDate = DateTime.Now;
        private string mstrGenPR_BegCode = "";
        private string mstrGenPR_EndCode = "";

        private int mintGenPR_CredTerm = 0;
        private string mstrIsCash = "";
        private string mstrHasReturn = "Y";
        private string mstrVatDue = "";

        private string mstrCalBook_PR = "";
        private string mstrCalBook_PO = "";

        private string mstrLastRunCode = "";
        private long intRunCode = 1;
        private string mstrCurrQcCoor = "";

        private decimal mdecAmt = 0;
        private decimal mdecVatAmt = 0;
        private decimal mdecDiscountAmt = 0;
        private decimal mdecXRate = 1;
        private decimal mdecTotPdQty = 0;
        private decimal mdecTotPdAmt = 0;
        private decimal mdecVatRate = 0;
        private decimal mdecGrossAmt = 0;

        private bool mbllCanGenPR = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private void pmCreateTem()
        {

            this.pmCreateTemPd(this.mstrTemPd_GenPR);

            DataTable dtbTemPd = new DataTable(this.mstrTemPd_GenPR1);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderH", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnBOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProj", System.Type.GetType("System.String"));

            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemBakRev = new DataTable(xd_TemHistory_BakRev);
            dtbTemBakRev.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemBakRev.Columns.Add("cRefType", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemBakRev.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemBakRev.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemBakRev.Columns.Add("dDueDate", System.Type.GetType("System.DateTime"));
            this.dtsDataEnv.Tables.Add(dtbTemBakRev);

            DataTable dtbTemWHBal = new DataTable(xd_TemHistory_WHBal);
            dtbTemWHBal.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemWHBal.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("cQnUM", System.Type.GetType("System.String"));
            dtbTemWHBal.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemWHBal.Columns.Add("nMinStock", System.Type.GetType("System.Decimal"));
            dtbTemWHBal.Columns.Add("nMaxStock", System.Type.GetType("System.Decimal"));
            this.dtsDataEnv.Tables.Add(dtbTemWHBal);

        }

        #region "Create Tab"
        private void pmCreateTemPd(string inAlias)
        {
            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cIsMainPd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cAttachFile", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark3", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark4", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark5", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark6", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark7", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark8", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark9", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRemark10", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cScrap", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nRefQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBakQty_MO", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBakQty_PO", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBakQty_PR", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPortion", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStkQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nStkUOMQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkUOMStd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStkQnUOM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnDept", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQnJob", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty1", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBackQty2", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cLastPdType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cLastQnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefToRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRefToQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cPdStI", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProcure", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cSubSti", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStep1", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cStep2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderI", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWOrderH", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRefNo_MO", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nIssueQty", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cWOrderI"].DefaultValue = "";
            dtbTemPd.Columns["cWOrderH"].DefaultValue = "";
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQcCoor"].DefaultValue = "";
            dtbTemPd.Columns["cQnCoor"].DefaultValue = "";
            dtbTemPd.Columns["cOPSeq"].DefaultValue = "";
            dtbTemPd.Columns["cIsMainPd"].DefaultValue = "";
            dtbTemPd.Columns["nQty"].DefaultValue = 0;
            dtbTemPd.Columns["nRefQty"].DefaultValue = 1;
            dtbTemPd.Columns["nLastQty"].DefaultValue = 0;
            dtbTemPd.Columns["nPortion"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nStkQty"].DefaultValue = 0;
            dtbTemPd.Columns["nStkUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPd.Columns["nUOMQty"].DefaultValue = 1;
            dtbTemPd.Columns["cUOMStd"].DefaultValue = "";
            dtbTemPd.Columns["cRemark1"].DefaultValue = "";
            dtbTemPd.Columns["cRemark2"].DefaultValue = "";
            dtbTemPd.Columns["cRemark3"].DefaultValue = "";
            dtbTemPd.Columns["cRemark4"].DefaultValue = "";
            dtbTemPd.Columns["cRemark5"].DefaultValue = "";
            dtbTemPd.Columns["cRemark6"].DefaultValue = "";
            dtbTemPd.Columns["cRemark7"].DefaultValue = "";
            dtbTemPd.Columns["cRemark8"].DefaultValue = "";
            dtbTemPd.Columns["cRemark9"].DefaultValue = "";
            dtbTemPd.Columns["cRemark10"].DefaultValue = "";
            dtbTemPd.Columns["cLot"].DefaultValue = "";
            dtbTemPd.Columns["cWHouse"].DefaultValue = "";
            dtbTemPd.Columns["cQcWHouse"].DefaultValue = "";
            dtbTemPd.Columns["cQnWHouse"].DefaultValue = "";
            dtbTemPd.Columns["nBackQty1"].DefaultValue = 0;
            dtbTemPd.Columns["nBackQty2"].DefaultValue = 0;
            dtbTemPd.Columns["cDept"].DefaultValue = "";
            dtbTemPd.Columns["cQcDept"].DefaultValue = "";
            dtbTemPd.Columns["cQnDept"].DefaultValue = "";
            dtbTemPd.Columns["cJob"].DefaultValue = "";
            dtbTemPd.Columns["cQcJob"].DefaultValue = "";
            dtbTemPd.Columns["cQnJob"].DefaultValue = "";
            dtbTemPd.Columns["cLastPdType"].DefaultValue = "";
            dtbTemPd.Columns["cLastProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQcProd"].DefaultValue = "";
            dtbTemPd.Columns["cLastQnProd"].DefaultValue = "";
            dtbTemPd.Columns["cRefToRowID"].DefaultValue = "";
            dtbTemPd.Columns["cPdStI"].DefaultValue = "";
            dtbTemPd.Columns["cSubSti"].DefaultValue = "";
            dtbTemPd.Columns["nRefToQty"].DefaultValue = 0;
            dtbTemPd.Columns["cProcure"].DefaultValue = SysDef.gc_PROCURE_TYPE_PURCHASE;
            dtbTemPd.Columns["cStep1"].DefaultValue = SysDef.gc_REF_STEP_CUT_STOCK;
            dtbTemPd.Columns["cStep2"].DefaultValue = SysDef.gc_REF_STEP_CUT_STOCK;
            dtbTemPd.Columns["nIssueQty"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd);
        }
        #endregion

        public void GenPR()
        {
            this.pmGenPR();
        }

        private void pmGenPR()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strWHouse_RM = "";
            bool bllCanGenDupMO = false;

            object[] pAPara = null;
            string strSQLExec = " select * from " + this.mstrRefTable;
            strSQLExec += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? ";
            if (this.mstrGenPR_OrderBy == "CCODE")
            {
                strSQLExec += " and MFWORDERHD.CCODE between ? and ? ";
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrGenPR_Branch, this.mstrGenPR_Plant, DocumentType.MO.ToString(), this.mstrGenPR_MOBook, this.mstrGenPR_BegCode, this.mstrGenPR_EndCode };
            }
            else
            {
                strSQLExec += " and MFWORDERHD.DDATE between ? and ? ";
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrGenPR_Branch, this.mstrGenPR_Plant, DocumentType.MO.ToString(), this.mstrGenPR_MOBook, this.mdttGenPR_BegDate, this.mdttGenPR_EndDate };
            }

            strSQLExec += " and MFWORDERHD.CMSTEP <> 'P' and MFWORDERHD.CSTAT <> 'C' and MFWORDERHD.CPRSTEP = ' ' ";

            objSQLHelper2.SetPara(pAPara);
            objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QLoadPR", this.mstrRefTable, strSQLExec, ref strErrorMsg);
            foreach (DataRow dtrLoadHead in this.dtsDataEnv.Tables["QLoadPR"].Rows)
            {

                objSQLHelper2.SetPara(new object[] { dtrLoadHead[QMFWOrderHDInfo.Field.MfgBookID].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QMfgBook", QMfgBookInfo.TableName, "select * from " + QMfgBookInfo.TableName + " where cRowID = ? ", ref strErrorMsg))
                {
                    DataRow dtrMfgBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                    strWHouse_RM = dtrMfgBook[QMfgBookInfo.Field.WHouse_RM].ToString().TrimEnd();
                    if (strWHouse_RM.Trim() != string.Empty)
                    {
                        strWHouse_RM = "(" + strWHouse_RM + ")";
                    }

                    this.mstrCalBook_PR = dtrMfgBook[QMfgBookInfo.Field.MBook_PR].ToString().TrimEnd();
                    if (this.mstrCalBook_PR.Trim() != string.Empty)
                    {
                        this.mstrCalBook_PR = "(" + this.mstrCalBook_PR + ")";
                    }

                    this.mstrCalBook_PO = dtrMfgBook[QMfgBookInfo.Field.MBook_PO].ToString().TrimEnd();
                    if (this.mstrCalBook_PO.Trim() != string.Empty)
                    {
                        this.mstrCalBook_PO = "(" + this.mstrCalBook_PO + ")";
                    }

                    bllCanGenDupMO = (dtrMfgBook[QMfgBookInfo.Field.IsGenDupPR].ToString() == "Y" ? true : false);
                }

                this.mstrQcBOM = "";
                this.mstrQnBOM = "";
                objSQLHelper2.SetPara(new object[1] { dtrLoadHead[QMFWOrderHDInfo.Field.BOMID].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode , cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                {
                    this.mstrQcBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                    this.mstrQnBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                }

                this.mstrCurrWOrderH = dtrLoadHead[QMFWOrderHDInfo.Field.RowID].ToString();
                this.mstrGenPR_Sect = dtrLoadHead[QMFWOrderHDInfo.Field.SectID].ToString();
                this.mstrGenPR_Job = dtrLoadHead[QMFWOrderHDInfo.Field.JobID].ToString();

                objSQLHelper.SetPara(new object[] { this.mstrGenPR_Sect });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from SECT where fcSkid = ? ", ref strErrorMsg))
                {
                    this.mstrGenPR_Dept = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString();
                }

                objSQLHelper.SetPara(new object[] { this.mstrGenPR_Job });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ? ", ref strErrorMsg))
                {
                    this.mstrGenPR_Proj = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString();
                }

                bool bllResult = false;
                if (bllCanGenDupMO)
                {
                    this.mbllCanGenPR = true;
                }
                else
                {
                    //ไม่อนุญาติให้ Gen PR ซ้ำอีกรอบ
                    this.mbllCanGenPR = !this.pmCheckHasPR(dtrLoadHead[QMFWOrderHDInfo.Field.RowID].ToString(), dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString(), ref strErrorMsg);
                }

                this.pmGen1PRDoc(dtrLoadHead[QMFWOrderHDInfo.Field.RowID].ToString(), strWHouse_RM, dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString().TrimEnd(), ref strErrorMsg);

            }

            dlgPreviewOrder dlg = new dlgPreviewOrder();

            //dlg.IsSave = this.mbllCanGenPR;

            dlg.BindData(this.dtsDataEnv, this.mstrTemPd_GenPR);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                //DataRow[] dtrChk = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("nQty <> 0");
                //if (dtrChk.Length > 0)
                if (true)
                {
                    if (this.pmSaveAllOrder(ref strErrorMsg))
                    {
                        MessageBox.Show("Generate Complete !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No Data !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //TODO: จัดนี้ต้อง Loop เพื่อ Update MFWORDERHD.CPRSTEP เป็น gen PR แล้ว
            }

            //if (true)
            //{
            //    this.pmGen1PRDoc(strRowID, strWHouse_RM, dtrLoadHead[QMFWOrderHDInfo.Field.RefNo].ToString().TrimEnd(), ref strErrorMsg);
            //}
            //else
            //{
            //    if (strErrorMsg != "")
            //        MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

        }

        private bool pmCheckHasPR(string inRowID, string inRefNo, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = false;

            objSQLHelper.SetPara(new object[] { inRowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed1", "REFDOC", "select * from REFDOC where CCHILDTYPE = 'MO' and CCHILDH = ? and CMASTERTYP = 'PR' ", ref strErrorMsg))
            {
                DataRow dtrRefDoc = this.dtsDataEnv.Tables["QHasUsed1"].Rows[0];
                objSQLHelper2.SetPara(new object[] { dtrRefDoc["cMasterH"].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QHasUsed", "ORDERH", "select * from ORDERH where fcSkid = ? and fcStat <> 'C' ", ref strErrorMsg))
                {
                    ioErrorMsg = UIBase.GetAppUIText(new string[] { "มีการสร้างไปเป็นเอกสาร PR แล้ว !", "Document has generated to PR Document !" });
                    bllHasUsed = true;
                }
                else
                {
                    ioErrorMsg = "";
                }

            }

            return bllHasUsed;
        }

        private void pmGen1PRDoc(string inWorderH, string inWHouse_RM, string inRefNo, ref string ioErrorMsg)
        {

            bool bllSucc = false;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Rows.Clear();
            objSQLHelper.SetPara(new object[] { inWorderH, SysDef.gc_PROCURE_TYPE_PURCHASE, "O" });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCreateTemPD", this.mstrITable2, "select * from " + this.mstrITable2 + " where cWOrderH = ? and cProcure = ? and cIOType = ? order by cSeq", ref strErrorMsg))
            {
                int i = 0;
                DataRow dtrTemVal = null;
                foreach (DataRow dtrGenTem in this.dtsDataEnv.Tables["QCreateTemPD"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].NewRow();


                    dtrNewRow["cRefNo_MO"] = inRefNo;
                    dtrNewRow["cWOrderI"] = dtrGenTem["cRowID"].ToString();

                    dtrNewRow["cWOrderH"] = dtrGenTem["cWOrderH"].ToString();

                    dtrNewRow["cPdType"] = dtrGenTem["cProdType"].ToString();
                    dtrNewRow["cProd"] = dtrGenTem["cProd"].ToString();
                    dtrNewRow["cUOM"] = dtrGenTem["cUOM"].ToString();
                    dtrNewRow["nRefQty"] = Convert.ToDecimal(dtrGenTem["nQty"]);
                    dtrNewRow["nQty"] = Convert.ToDecimal(dtrGenTem["nQty"]);
                    dtrNewRow["nUOMQty"] = Convert.ToDecimal(dtrGenTem["nUOMQty"]);
                    dtrNewRow["nRecNo"] = i++;
                    dtrNewRow["nPrice"] = 0;

                    objSQLHelper2.SetPara(new object[] { dtrGenTem["cProd"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid,fcCode,fcName,fcSupp,fnStdCost from PROD where fcSKid = ?", ref strErrorMsg))
                    {
                        dtrTemVal = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        dtrNewRow["cQcProd"] = dtrTemVal["fcCode"].ToString();
                        dtrNewRow["cQnProd"] = dtrTemVal["fcName"].ToString();
                        dtrNewRow["cCoor"] = dtrTemVal["fcSupp"].ToString();
                        dtrNewRow["nPrice"] = Convert.ToDecimal(dtrTemVal["fnStdCost"]);

                        decimal decPOQty = this.pmQueryProdHistory_BakRev(dtrGenTem["cProd"].ToString(), "PO");
                        decimal decPRQty = this.pmQueryProdHistory_BakRev(dtrGenTem["cProd"].ToString(), "PR");
                        decimal decStkQty = this.pmQueryProdHistory_BalStk(dtrGenTem["cProd"].ToString(), inWHouse_RM);

                        decimal decBookMOQty = this.pmQueryBookingMO(dtrGenTem["cProd"].ToString(), dtrGenTem["cRowID"].ToString());
                        decimal decIssueQty = this.pmQueryMOIssueQty(dtrGenTem["cRowID"].ToString());
                        decimal decOnHandQty = decPOQty + decPRQty + decStkQty - decBookMOQty + decIssueQty;
                        dtrNewRow["nBakQty_MO"] = decBookMOQty;
                        dtrNewRow["nBakQty_PO"] = decPOQty;
                        dtrNewRow["nBakQty_PR"] = decPRQty;
                        dtrNewRow["nStkQty"] = decStkQty;
                        dtrNewRow["nIssueQty"] = decIssueQty;

                        if (Convert.ToDecimal(dtrGenTem["nQty"]) > decOnHandQty)
                        {
                            dtrNewRow["nQty"] = Convert.ToDecimal(dtrGenTem["nQty"]) - decOnHandQty;
                        }
                        else
                        {
                            dtrNewRow["nQty"] = 0;
                        }

                    }

                    objSQLHelper2.SetPara(new object[] { dtrGenTem["cUOM"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid,fcCode,fcName from UM where fcSKid = ?", ref strErrorMsg))
                    {
                        dtrTemVal = this.dtsDataEnv.Tables["QUM"].Rows[0];
                        dtrNewRow["cQnUOM"] = dtrTemVal["fcName"].ToString();
                    }

                    dtrNewRow["cCoor"] = (dtrNewRow["cCoor"].ToString().Trim() == string.Empty ? this.mstrGenPR_Coor : dtrNewRow["cCoor"].ToString());
                    objSQLHelper2.SetPara(new object[] { dtrNewRow["cCoor"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select * from COOR where fcSKid = ?", ref strErrorMsg))
                    {
                        dtrTemVal = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        dtrNewRow["cQcCoor"] = dtrTemVal["fcCode"].ToString();
                        dtrNewRow["cQnCoor"] = dtrTemVal["fcName"].ToString();
                        this.pmPushCoor(inRefNo, dtrNewRow["cCoor"].ToString(), dtrNewRow["cQcCoor"].ToString(), dtrNewRow["cQnCoor"].ToString(), this.mstrQnBOM);
                    }

                    this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Rows.Add(dtrNewRow);
                    bllSucc = true;
                }
            }

            if (bllSucc)
            {

                this.mstrGenPR_RefNo = inRefNo;

                if (this.mbllCanGenPR)
                {
                    this.pmRunAllCode(inRefNo);
                }
                //this.pmRunCode_PR();
                //this.pmReplRecord_PR(ref strErrorMsg);

            }

        }

        private void pmPushCoor(string inRefNo, string inCoor, string inCode, string inName, string inQnBOM)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].Select(" cRefNo = '" + inRefNo + "' and cQcCoor = '" + inCode + "'");
            if (dtrSel.Length == 0)
            {
                DataRow dtrCoor = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].NewRow();
                dtrCoor["cCoor"] = inCoor;
                dtrCoor["cQcCoor"] = inCode;
                dtrCoor["cQnCoor"] = inName;
                dtrCoor["cRefNo"] = inRefNo;
                dtrCoor["cQnBOM"] = inQnBOM;

                dtrCoor["cWOrderH"] = this.mstrCurrWOrderH;
                dtrCoor["cSect"] = this.mstrGenPR_Sect;
                dtrCoor["cDept"] = this.mstrGenPR_Dept;
                dtrCoor["cJob"] = this.mstrGenPR_Job;
                dtrCoor["cProj"] = this.mstrGenPR_Proj;

                this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].Rows.Add(dtrCoor);
            }
        }

        private void pmRunAllCode(string inMORefNo)
        {
            //DataView dv = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].DefaultView;
            //dv.Sort = "cRefNo, cQcCoor";

            DataRow[] dtrSel1 = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].Select("cRefNo = '" + inMORefNo + "'", "cQcCoor");
            for (int i = 0; i < dtrSel1.Length; i++)
            {

                DataRow dtrRunCode = dtrSel1[i];

                string strQcCoor = dtrRunCode["cQcCoor"].ToString();
                DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("cRefNo_MO = '" + inMORefNo + "' and cQcCoor = '" + strQcCoor + "' and nQty > 0");
                if (dtrSel.Length > 0)
                {
                    this.pmRunCode_PR();

                    dtrRunCode["cCode"] = this.mstrGenPR_Code;
                    dtrRunCode["cRefNo"] = inMORefNo;

                    for (int j = 0; j < dtrSel.Length; j++)
                    {
                        dtrSel[j]["cCode"] = this.mstrGenPR_Code;
                        dtrSel[j]["cRefNo"] = inMORefNo;
                    }
                }
            }

        }

        private bool pmSaveAllOrder(ref string ioErrorMsg)
        {
            ioErrorMsg = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mstrCurrQcCoor = "";
            DataView dv = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR1].DefaultView;
            dv.Sort = "cRefNo, cQcCoor";
            bool bllSucc = true;
            for (int i = 0; i < dv.Count; i++)
            {
                DataRowView dtrRunCode = dv[i];
                //this.pmRunCode_PR();
                if (dtrRunCode["cCode"].ToString().Trim() != string.Empty)
                {
                    if (this.pmChkDupCode(dtrRunCode["cCode"].ToString()))
                    {
                        ioErrorMsg = UIBase.GetAppUIText(new string[] { "เลขที่เอกสารซ้ำ : ", "Duplicate Document Code : " }) + dtrRunCode["cCode"].ToString().Trim();
                        //MessageBox.Show(, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bllSucc = false;
                        break;
                    }
                    else
                    {
                        this.mstrCurrQcCoor = dtrRunCode["cQcCoor"].ToString();
                        //this.pmReplRecord_PR(dtrRunCode["cCoor"].ToString(), dtrRunCode["cCode"].ToString(), dtrRunCode["cRefNo"].ToString(), dtrRunCode["cQnBOM"].ToString(), ref strErrorMsg);
                        this.pmReplRecord_PR(dtrRunCode, ref strErrorMsg);
                        //Update WORDERH.CPRSTEP = 'P'

                        pobjSQLUtil.SetPara(new object[] { dtrRunCode["cWOrderH"].ToString() });
                        pobjSQLUtil.SQLExec("update MFWORDERHD set CPRSTEP = 'P' where CROWID = ?", ref strErrorMsg);
                        pobjSQLUtil.SQLExec("update MFWORDERIT_PD set CPRSTEP = 'P' where CWORDERH = ?", ref strErrorMsg);

                    }
                }
                else
                { 
                    //Update ถึงแม้ว่าจะไม่ได้ GEN PR
                    //Update WORDERH.CPRSTEP = 'P'
                    pobjSQLUtil.SetPara(new object[] { dtrRunCode["cWOrderH"].ToString() });
                    pobjSQLUtil.SQLExec("update MFWORDERHD set CPRSTEP = 'L' where CROWID = ?", ref strErrorMsg);
                    pobjSQLUtil.SQLExec("update MFWORDERIT_PD set CPRSTEP = 'L' where CWORDERH = ?", ref strErrorMsg);
                }
            }
            return bllSucc;
        }

        private bool pmChkDupCode(string inCode)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { App.ActiveCorp.RowID, this.mstrGenPR_Branch, "PR", this.mstrGenPR_Book, inCode };
            strSQLStr = "select fcCode from OrderH where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                bllResult = true;
            }
            return bllResult;
        }

        private bool pmRunCode_PR()
        {
            string strErrorMsg = "";
            string strLastRunCode = "";
            int intCodeLen = 0;
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            pAPara = new object[4] { App.ActiveCorp.RowID, this.mstrGenPR_Branch, "PR", this.mstrGenPR_Book };
            strSQLStr = "select fcCode from OrderH where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode < ':' order by fcCode desc";

            if (mstrLastRunCode == "")
            {
                pobjSQLUtil.SetPara(pAPara);
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRunCode", "GLTRAN", strSQLStr, ref strErrorMsg))
                {
                    strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["fcCode"].ToString().Trim();
                    try
                    {
                        intRunCode = Convert.ToInt64(strLastRunCode) + 1;
                        this.mstrLastRunCode = strLastRunCode;
                    }
                    catch (Exception ex)
                    {
                        strErrorMsg = ex.Message.ToString();
                        intRunCode++;
                    }
                }
            }
            else
            {
                intRunCode++;
                strLastRunCode = this.mstrLastRunCode;
            }

            intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : 7);
            this.mstrGenPR_Code = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));
            return true;
        }

        //private bool pmReplRecord_PR(string inCoor, string inCode, string inRefNo, string inQnBOM, ref string ioErrorMsg)
        private bool pmReplRecord_PR(DataRowView inSource, ref string ioErrorMsg)
        {
            bool bllResult = true;
            bool bllIsNewRow = false;
            string strErrorMsg = "";
            string strRowID = "";

            DataRow dtrCurrRow = null;
            DataRow dtrOrderH = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //if (this.mFormEditMode == UIHelper.AppFormState.Insert)
            if (true)
            {
                strRowID = App.mRunRowID("ORDERH");
                this.mstrEditRowID_PR = strRowID;
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "ORDERH", "ORDERH", "select * from ORDERH where 0=1", ref strErrorMsg);
                dtrOrderH = this.dtsDataEnv.Tables["ORDERH"].NewRow();
                this.dtsDataEnv.Tables["ORDERH"].Rows.Add(dtrOrderH);
                bllIsNewRow = true;
                dtrOrderH["fcCreateAp"] = "$/";
                dtrOrderH["fcCreateBy"] = App.FMAppUserID;
            }
            else
            {
                dtrOrderH = this.dtsDataEnv.Tables["ORDERH"].Rows[0];
                bllIsNewRow = false;
            }

            string gcTemStr01 = "";

            this.mstrSave_RefNo = inSource["cRefNo"].ToString();
            this.pmRecalTotPd();

            //string gcTemStr01 = BizRule.SetMemData(this.mstrRemarkH1.TrimEnd(), "Rem")
            //    + BizRule.SetMemData(this.mstrInvDetail, "Det")
            //    + BizRule.SetMemData(this.mstrRemarkH2.TrimEnd(), "Rm2")
            //    + BizRule.SetMemData(this.mstrRemarkH3.TrimEnd(), "Rm3")
            //    + BizRule.SetMemData(this.mstrRemarkH4.TrimEnd(), "Rm4")
            //    + BizRule.SetMemData(this.mstrRemarkH5.TrimEnd(), "Rm5")
            //    + BizRule.SetMemData(this.mstrRemarkH6.TrimEnd(), "Rm6")
            //    + BizRule.SetMemData(this.mstrRemarkH7.TrimEnd(), "Rm7")
            //    + BizRule.SetMemData(this.mstrRemarkH8.TrimEnd(), "Rm8")
            //    + BizRule.SetMemData(this.mstrRemarkH9.TrimEnd(), "Rm9")
            //    + BizRule.SetMemData(this.mstrRemarkH10.TrimEnd(), "RmA");


            gcTemStr01 = BizRule.SetMemData("", "Rem")
                + BizRule.SetMemData("", "Det")
                + BizRule.SetMemData("", "Rm2")
                + BizRule.SetMemData("", "Rm3")
                + BizRule.SetMemData("", "Rm4")
                + BizRule.SetMemData("", "Rm5")
                + BizRule.SetMemData("", "Rm6")
                + BizRule.SetMemData("", "Rm7")
                + BizRule.SetMemData("", "Rm8")
                + BizRule.SetMemData("", "Rm9")
                + BizRule.SetMemData(inSource["cQnBOM"].ToString().TrimEnd(), "RmA");

            //string strRefNo = this.mstrGenPR_RefNo;
            //string strRefNo = inRefNo;

            //string strVatSeq = "";
            decimal decVatRate = 0;
            string strVATType = "";
            pobjSQLUtil.SetPara(new object[1] { App.ActiveCorp.SaleVATType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVatType", "VatType", "select fcCode,fnRate from VatType where fcCode = ?", ref strErrorMsg))
            {
                DataRow dtrTemRow = this.dtsDataEnv.Tables["QVatType"].Rows[0];
                decVatRate = Convert.ToDecimal(dtrTemRow["fnRate"]);
                this.mdecVatRate = Convert.ToDecimal(dtrTemRow["fnRate"]);
                strVATType = App.ActiveCorp.SaleVATType;
            }

            mstrDefaWHouseBuy = "";
            pobjSQLUtil.SetPara(new object[1] { this.mstrGenPR_Branch });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcWHSale,fcWHBuy from BRANCH where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrTemRow = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                mstrDefaWHouseBuy = dtrTemRow["fcWHBuy"].ToString();
            }

            pobjSQLUtil.SetPara(new object[] { "PR" });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefType", "REFTYPE", "select * from RefType where fcSkid = ?", ref strErrorMsg);
            DataRow dtrRefType = this.dtsDataEnv.Tables["QRefType"].Rows[0];

            this.mstrIsCash = dtrRefType["fcIsCash"].ToString();
            this.mstrVatDue = dtrRefType["fcVatDue"].ToString();

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = null;
            strRowID = this.mstrEditRowID_PR;

            dtrOrderH["fcSkid"] = this.mstrEditRowID_PR;
            dtrOrderH["fcCorp"] = App.ActiveCorp.RowID;
            dtrOrderH["fcBranch"] = this.mstrGenPR_Branch;
            dtrOrderH["fcStep"] = SysDef.gc_REF_STEP_CUT_STOCK;
            dtrOrderH["fcRefType"] = "PR";
            dtrOrderH["fcRfType"] = "w";
            dtrOrderH["fcBook"] = this.mstrGenPR_Book;
            dtrOrderH["fcCode"] = inSource["cCode"].ToString().TrimEnd();
            dtrOrderH["fcRefNo"] = inSource["cRefNo"].ToString().TrimEnd();
            dtrOrderH["fdDate"] = this.mdttGenPR_Date;
            //dtrOrderH["fdReceDate"] = this.mdttSendDate;
            dtrOrderH["fdReceDate"] = this.mdttGenPR_Date;
            dtrOrderH["fdDueDate"] = this.mdttGenPR_Date;
            dtrOrderH["fcSect"] = inSource["cSect"].ToString();
            dtrOrderH["fcDept"] = inSource["cDept"].ToString();
            dtrOrderH["fcJob"] = inSource["cJob"].ToString();
            dtrOrderH["fcProj"] = inSource["cProj"].ToString();
            dtrOrderH["fcVatIsOut"] = App.ActiveCorp.SaleVATIsOut;
            dtrOrderH["fcVatType"] = strVATType;
            dtrOrderH["fnVatrate"] = decVatRate;
            dtrOrderH["fcCoor"] = inSource["cCoor"].ToString();

            dtrOrderH["fcEmpl"] = "";
            dtrOrderH["fcRecvMan"] = "";	// fcRecvMan
            dtrOrderH["fcDeliCoor"] = inSource["cCoor"].ToString();
            //dtrOrderH["fcStepX1"] = this.txtStepX1.Text;
            dtrOrderH["fnCredTerm"] = this.mintGenPR_CredTerm;
            dtrOrderH["fcIsCash"] = this.mstrIsCash;
            dtrOrderH["fcHasRet"] = this.mstrHasReturn;
            dtrOrderH["fcVatDue"] = this.mstrVatDue;
            dtrOrderH["fcDiscStr"] = "";
            dtrOrderH["fcCurrency"] = "";
            dtrOrderH["fnXRate"] = 0;
            dtrOrderH["fnAmtKe"] = this.mdecAmt;
            dtrOrderH["fnVatAmtKe"] = this.mdecVatAmt;
            dtrOrderH["fnDiscAmtK"] = this.mdecDiscountAmt;
            dtrOrderH["fnAmt"] = Convert.ToDecimal(this.mdecAmt) * Convert.ToDecimal(mdecXRate);
            dtrOrderH["fnVatAmt"] = Convert.ToDecimal(this.mdecVatAmt) * Convert.ToDecimal(mdecXRate);
            dtrOrderH["fnDiscAmt1"] = Convert.ToDecimal(this.mdecDiscountAmt) * Convert.ToDecimal(mdecXRate);

            //dtrOrderH["fnAmtKe"] = 0;
            //dtrOrderH["fnVatAmtKe"] = 0;
            //dtrOrderH["fnDiscAmtK"] = 0;
            //dtrOrderH["fnAmt"] = Convert.ToDecimal(0) * Convert.ToDecimal(1);
            //dtrOrderH["fnVatAmt"] = Convert.ToDecimal(0) * Convert.ToDecimal(1);
            //dtrOrderH["fnDiscAmt1"] = Convert.ToDecimal(0) * Convert.ToDecimal(1);

            dtrOrderH["fnAmt2"] = 0;	//fnAmt2
            //dtrOrderH["fcCreateBy"] = "";	//fcCreateBy
            //dtrOrderH["fcCorrectB"] = "";	//fcCorrectB
            dtrOrderH["fcCorrectB"] = "";
            dtrOrderH["fcApproveB"] = ""; //fcApproveB
            dtrOrderH["fcLUpdApp"] = "$/";	//fcLUpdApp
            dtrOrderH["fcDataser"] = "";	//fcDataser
            dtrOrderH["fcEAfterR"] = "E";

            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrOrderH["fmMemData"] = gcTemStr02;
            if (DataSetHelper.HasField("fmMemData2", dtrOrderH))
            {
                dtrOrderH["fmMemData2"] = gcTemStr03;
                dtrOrderH["fmMemData3"] = gcTemStr04;
                dtrOrderH["fmMemData4"] = gcTemStr05;
            }

            if (DataSetHelper.HasField("fmMemData5", dtrOrderH))
            {
                dtrOrderH["fmMemData5"] = gcTemStr06;
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

                if (this.mstrStep == SysDef.gc_REF_STEP_CUT_STOCK)
                {
                    //dtrOrderH["fcApproveB"] = App.FMAppUserID; //fcApproveB
                    //dtrOrderH["fdApprove"] = this.mSaveDBAgent.GetDBServerDateTime();
                }

                decimal decDiscAmtI = 0;
                this.pmSaveOrderI(this.mstrEditRowID_PR, ref decDiscAmtI);

                dtrOrderH["fnDiscAmtI"] = decDiscAmtI;

                //Update GLRef
                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrOrderH, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                this.mdbTran2.Commit();

                this.dtsDataEnv.Tables["ORDERH"].Rows.Clear();

            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                this.mdbTran2.Rollback();
                App.WriteEventsLog(ex);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                this.mdbConn.Close();
                this.mdbConn2.Close();
            }

            return bllResult;
        }

        private bool pmSaveOrderI(string ioErrorMsg, ref decimal ioDiscAmtI)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("cQcCoor = '" + this.mstrCurrQcCoor + "' and nQty <> 0");
            DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("cRefNo_MO = '" + this.mstrSave_RefNo + "' and cQcCoor = '" + this.mstrCurrQcCoor + "' and nQty <> 0");
            //if (dtrSel.Length == 0)

            for (int i = 0; i < dtrSel.Length; i++)
            {
                DataRow dtrTemPd = dtrSel[i];
                bool bllIsNewRow = false;

                //if (dtrTemPd["cStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                //	continue;
                //else if (dtrTemPd["cDOStep"].ToString() == SysDef.gc_REF_STEP_PAY)
                //	continue;

                if (dtrTemPd["cProd"].ToString().TrimEnd() != string.Empty)
                {

                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select fcSkid from OrderI where fcSkid = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        strRowID = App.mRunRowID("OrderI");
                        bllIsNewRow = true;
                    }
                    else
                    {
                        bllIsNewRow = false;
                        strRowID = dtrTemPd["cRowID"].ToString();
                    }
                    dtrTemPd["cRowID"] = strRowID;
                    DataRow dtrOrderI = null;
                    this.pmReplRecordOrderI(bllIsNewRow, dtrTemPd, ref dtrOrderI);
                    ioDiscAmtI += Convert.ToDecimal(dtrOrderI["fnDiscAmt"]);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrOrderI, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {
                    //Delete OrderI
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        ////Delete Note Cut and OrderI
                        //pAPara = new object[] { this.mstrRefType, this.mstrEditRowID, dtrTemPd["cRowID"].ToString() };
                        //this.mSaveDBAgent.BatchSQLExec("delete from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? and FCMASTERI = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                        //pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        //this.mSaveDBAgent.BatchSQLExec("delete from OrderI where FCSKID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }
                }
            }
            return true;
        }

        private bool pmReplRecordOrderI(bool inState, DataRow inTemPd, ref DataRow ioOrderI)
        {
            bool bllIsNewRec = inState;
            string strErrorMsg = "";
            DataRow dtrOrderI;
            if (bllIsNewRec)
            {
                //pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "ORDERI", "select * from " + this.mstrITable + " where 0=1", ref strErrorMsg);
                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "ORDERI", "ORDERI", "select * from ORDERI where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrOrderI = this.dtsDataEnv.Tables["ORDERI"].NewRow();
                dtrOrderI["fcSkid"] = inTemPd["cRowID"].ToString();
                dtrOrderI["fcCreateAp"] = "$/";
            }
            else
            {
                //pobjSQLUtil.SetPara(new object[1] {inTemPd["cRowID"].ToString()});
                //pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, "ORDERI", "select * from " + this.mstrITable + " where fcSkid = ?", ref strErrorMsg);

                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, "ORDERI", "select * from ORDERI where fcSkid = ?", new object[1] { inTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                dtrOrderI = this.dtsDataEnv.Tables["ORDERI"].Rows[0];
            }

            DataRow dtrOrderH = this.dtsDataEnv.Tables["ORDERH"].Rows[0];

            string strRemark1 = inTemPd["cRemark1"].ToString().TrimEnd();
            string strRemark2 = inTemPd["cRemark2"].ToString().TrimEnd();
            string strRemark3 = inTemPd["cRemark3"].ToString().TrimEnd();
            string strRemark4 = inTemPd["cRemark4"].ToString().TrimEnd();
            string strRemark5 = inTemPd["cRemark5"].ToString().TrimEnd();
            string strRemark6 = inTemPd["cRemark6"].ToString().TrimEnd();
            string strRemark7 = inTemPd["cRemark7"].ToString().TrimEnd();
            string strRemark8 = inTemPd["cRemark8"].ToString().TrimEnd();
            string strRemark9 = inTemPd["cRemark9"].ToString().TrimEnd();
            string strRemark10 = inTemPd["cRemark10"].ToString().TrimEnd();

            dtrOrderI["fcStep"] = dtrOrderH["fcStep"].ToString();
            dtrOrderI["fcCorp"] = App.ActiveCorp.RowID;
            dtrOrderI["fcBranch"] = this.mstrGenPR_Branch;
            dtrOrderI["fcOrderH"] = dtrOrderH["fcSkid"].ToString();
            dtrOrderI["fcRefType"] = dtrOrderH["fcRefType"].ToString();
            //dtrOrderI["fcRfType"] = dtrOrderH["fcRfType"].ToString();
            dtrOrderI["fcStat"] = dtrOrderH["fcStat"].ToString();
            dtrOrderI["fdDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]).Date;
            dtrOrderI["fcCoor"] = dtrOrderH["fcCoor"].ToString();
            dtrOrderI["fcRefPdTyp"] = SysDef.gc_REFPD_TYPE_PRODUCT;
            dtrOrderI["fcRootSeq"] = "";
            dtrOrderI["fcShowComp"] = "";
            dtrOrderI["fcPformula"] = "";
            dtrOrderI["fcFormulas"] = "";
            dtrOrderI["fcProdType"] = inTemPd["cPdType"].ToString();
            dtrOrderI["fcProd"] = inTemPd["cProd"].ToString();
            dtrOrderI["fmRemark"] = strRemark1;
            dtrOrderI["fmRemark2"] = strRemark2;
            dtrOrderI["fmRemark3"] = strRemark3;
            dtrOrderI["fmRemark4"] = strRemark4;
            dtrOrderI["fmRemark5"] = strRemark5;
            dtrOrderI["fmRemark6"] = strRemark6;
            dtrOrderI["fmRemark7"] = strRemark7;
            dtrOrderI["fmRemark8"] = strRemark8;
            dtrOrderI["fmRemark9"] = strRemark9;
            dtrOrderI["fmRemark10"] = strRemark10;
            dtrOrderI["fcUm"] = inTemPd["cUOM"].ToString();
            dtrOrderI["fcLot"] = inTemPd["cLot"].ToString().TrimEnd();
            dtrOrderI["fcWhouse"] = this.mstrDefaWHouseBuy;
            dtrOrderI["fcSect"] = dtrOrderH["fcSect"].ToString();
            dtrOrderI["fcDept"] = dtrOrderH["fcDept"].ToString();
            dtrOrderI["fcJob"] = dtrOrderH["fcJob"].ToString();
            dtrOrderI["fcProj"] = dtrOrderH["fcProj"].ToString();
            dtrOrderI["fcIoType"] = "I";
            dtrOrderI["fnPriceKe"] = Convert.ToDecimal(inTemPd["nPrice"]);
            dtrOrderI["fnDiscAmtK"] = 0;
            dtrOrderI["fnPrice"] = Convert.ToDecimal(inTemPd["nPrice"]) * Convert.ToDecimal(1);
            //dtrOrderI["fnDiscAmt"] = Convert.ToDecimal(inTemPd["nDiscAmt"]) * Convert.ToDecimal(1);
            dtrOrderI["fnDiscAmt"] = 0;
            dtrOrderI["fnXRate"] = Convert.ToDecimal(1);
            dtrOrderI["fcDiscStr"] = "";

            //dtrOrderI["fnPriceKe"] = 0;
            //dtrOrderI["fnDiscAmtK"] = 0;
            //dtrOrderI["fnPrice"] = Convert.ToDecimal(0) * Convert.ToDecimal(1);
            //dtrOrderI["fnDiscAmt"] = Convert.ToDecimal(0) * Convert.ToDecimal(1);
            //dtrOrderI["fnXRate"] = Convert.ToDecimal(1);
            //dtrOrderI["fcDiscStr"] = "";

            dtrOrderI["fcSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);
            dtrOrderI["fnUmQty"] = Convert.ToDecimal(inTemPd["nUOMQty"]);
            dtrOrderI["fcUmStd"] = inTemPd["cUOMStd"].ToString();
            dtrOrderI["fnQty"] = Convert.ToDecimal(inTemPd["nQty"]);
            dtrOrderI["fnBackQty"] = Convert.ToDecimal(inTemPd["nQty"]);

            //
            DateTime dttDueDate = Convert.ToDateTime(dtrOrderH["fdRecedate"]);
            //if (!Convert.IsDBNull(inTemPd["dDueDate"]))
            //{
            //    dttDueDate = Convert.ToDateTime(inTemPd["dDueDate"]);
            //}
            //dtrOrderI["fdDelivery"] = dttDueDate;

            dtrOrderI["fcLUpdApp"] = App.AppID;
            dtrOrderI["fcDataser"] = "";
            dtrOrderI["fcEAfterR"] = 'E';

            //dtrOrderI["fnStQty"] = Convert.ToDecimal(inTemPd["nStQty"]);
            //dtrOrderI["fcStUm"] = inTemPd["cUOMStd"].ToString();
            //dtrOrderI["fnStUmQty"] = Convert.ToDecimal(inTemPd["nStUOMQty"]);
            //dtrOrderI["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

            decimal decVatInAmt = (dtrOrderH["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrOrderH["fnVatAmt"]) : 0);
            decimal decItemAmt = 0;
            decimal decItemVAT = 0;
            decimal decDiscAmt2 = (Convert.IsDBNull(dtrOrderH["fnDiscAmt2"]) ? 0 : Convert.ToDecimal(dtrOrderH["fnDiscAmt2"]));
            if ((Convert.ToDecimal(dtrOrderH["fnAmt"]) + decVatInAmt + Convert.ToDecimal(dtrOrderH["fnDiscAmt1"]) + decDiscAmt2) == 0)
            {
                decItemAmt = (Convert.ToDecimal(dtrOrderI["fnQty"]) * Convert.ToDecimal(dtrOrderI["fnPriceKe"]) * Convert.ToDecimal(dtrOrderI["fnXRate"])
                    - Convert.ToDecimal(dtrOrderI["fnDiscAmt"]));
            }
            else
            {
                decItemAmt = (Convert.ToDecimal(dtrOrderI["fnQty"]) * Convert.ToDecimal(dtrOrderI["fnPriceKe"]) * Convert.ToDecimal(dtrOrderI["fnXRate"]) - Convert.ToDecimal(dtrOrderI["fnDiscAmt"]))
                    * ((Convert.ToDecimal(dtrOrderH["fnAmt"]) + decVatInAmt) / (Convert.ToDecimal(dtrOrderH["fnAmt"]) + decVatInAmt + Convert.ToDecimal(dtrOrderH["fnDiscAmt1"]) + decDiscAmt2));
            }
            dtrOrderI["fcVatIsOut"] = dtrOrderH["fcVatIsOut"].ToString();
            dtrOrderI["fcVatType"] = dtrOrderH["fcVatType"].ToString();
            dtrOrderI["fnVatRate"] = Convert.ToDecimal(dtrOrderH["fnVatRate"]);
            decItemVAT = decItemAmt * Convert.ToDecimal(dtrOrderH["fnVatRate"]) / (100 + (dtrOrderH["fcVatIsOut"].ToString() == "N" ? Convert.ToDecimal(dtrOrderH["fnVatRate"]) : 0));

            dtrOrderI["fnVatAmt"] = Math.Round(decItemVAT, 4);

            this.pmSaveRefDoc(dtrOrderI["fcOrderH"].ToString(), dtrOrderI["fcSkid"].ToString(), inTemPd["cWOrderH"].ToString(), inTemPd["cWOrderI"].ToString(), Convert.ToDecimal(inTemPd["nQty"]));

            ioOrderI = dtrOrderI;
            return true;
        }

        private bool mbllRecalTotPd = true;

        private bool pmSaveRefDoc(string inOrderH, string inOrderI, string inWOrderH, string inWOrderI, decimal inQty)
        {
            bool bllResult = true;
            string strErrorMsg = "";
            object[] pAPara = null;

            bool bllIsNewRow_RefTo = false;
            string strRowID_RefTo = "";
            this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "REFDOC", "REFDOC", "select * from REFDOC where 0=1", null, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            DataRow dtrREFDOC = this.dtsDataEnv.Tables["REFDOC"].NewRow();

            strRowID_RefTo = App.mRunRowID("REFDOC");
            bllIsNewRow_RefTo = true;

            dtrREFDOC["cRowID"] = strRowID_RefTo;
            dtrREFDOC["cCorp"] = App.ActiveCorp.RowID;
            dtrREFDOC["cBranch"] = this.mstrGenPR_Branch;
            dtrREFDOC["cPlant"] = this.mstrGenPR_Plant;
            dtrREFDOC["cMasterTyp"] = DocumentType.PR.ToString();
            dtrREFDOC["cMasterH"] = inOrderH;
            dtrREFDOC["cMasterI"] = inOrderI;
            dtrREFDOC["cChildType"] = DocumentType.MO.ToString();
            dtrREFDOC["cChildH"] = inWOrderH;
            dtrREFDOC["cChildI"] = inWOrderI;
            dtrREFDOC["nQty"] = inQty;

            string strSQLUpdateStr_RefTo = "";
            DataSetHelper.GenUpdateSQLString(dtrREFDOC, "CROWID", bllIsNewRow_RefTo, ref strSQLUpdateStr_RefTo, ref pAPara);
            //pobjSQLUtil.SetPara(pAPara);
            //bllResult = pobjSQLUtil.SQLExec(strSQLUpdateStr_RefTo, ref strErrorMsg);
            bllResult = this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr_RefTo, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

            //string strSQLUpdate_OrderI = "update MFWORDERIT_PD set cStep2 = ? where cRowID = ?";
            //this.mSaveDBAgent2.BatchSQLExec(strSQLUpdate_OrderI, new object[] { SysDef.gc_STEP_PAY, inWOrderI }, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

            return bllResult;
        }

        private void pmRecalTotPd()
        {
            decimal decSumQty = 0;
            decimal decSumAmtKe = 0;
            //decimal decSumAmtStd = 0;
            //decimal decXRate = 1;
            if (this.mbllRecalTotPd)
            {
                //if (dtrSel.Length == 0)

                //DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("cQcCoor = '" + this.mstrCurrQcCoor + "'");
                DataRow[] dtrSel = this.dtsDataEnv.Tables[this.mstrTemPd_GenPR].Select("cRefNo_MO = '" + this.mstrSave_RefNo + "' and cQcCoor = '" + this.mstrCurrQcCoor + "'");
                for (int i = 0; i < dtrSel.Length; i++)
                {
                    DataRow dtrTemPd = dtrSel[i];
                    //decXRate = (Convert.ToDecimal(dtrTemPd["nXRate"]) == 0 ? 1 : Convert.ToDecimal(dtrTemPd["nXRate"]));

                    //decSumQty += Convert.ToDecimal(dtrTemPd["nQty"]);
                    //decSumAmtKe += Convert.ToDecimal(dtrTemPd["nAmt"]);

                    //if (dtrTemPd["cRefPdType"].ToString() == SysDef.gc_REFPD_TYPE_PRODUCT)
                    if (true)
                    {
                        decimal decQty = 0;
                        decQty = (Convert.IsDBNull(dtrTemPd["nQty"]) ? 0 : Convert.ToDecimal(dtrTemPd["nQty"]));
                        decSumQty += decQty;
                        decimal decAmt = decQty * Convert.ToDecimal(dtrTemPd["nPrice"]);
                        //decimal decDiscAmtI = BizRule.CalDiscAmtFromDiscStr(dtrTemPd["cDiscStr"].ToString(), decAmt, decQty, 0);
                        decimal decDiscAmtI = 0;
                        decimal decPrice = BizRule.LimitPrcCost(Convert.ToDecimal(dtrTemPd["nPrice"]));
                        //dtrTemPd["nDiscAmt"] = decDiscAmtI;
                        //dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - Convert.ToDecimal(dtrTemPd["nDiscAmt"]);

                        dtrTemPd["nAmt"] = (Convert.ToDecimal(dtrTemPd["nQty"]) * Convert.ToDecimal(dtrTemPd["nPrice"])) - 0;

                        decSumAmtKe += Math.Round((Convert.IsDBNull(dtrTemPd["nAmt"]) ? 0 : Convert.ToDecimal(dtrTemPd["nAmt"])), App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    }

                    //decSumAmtStd += Convert.ToDecimal(dtrTemPd["nAmt"]) * decXRate;
                }

                //this.mbllRecalTotPd = false;
                this.mdecTotPdQty = decSumQty;
                this.mdecTotPdAmt = decSumAmtKe;

                decimal decDiscAmt = 0; // BizRule.CalDiscAmtFromDiscStr(this.txtDiscount.Text, Convert.ToDecimal(this.mdecTotPdAmt), 0, 0);
                if (decDiscAmt != Convert.ToDecimal(this.mdecDiscountAmt))
                    this.mdecDiscountAmt = decDiscAmt;
            }
            if (App.ActiveCorp.SaleVATIsOut == "Y")
            {
                this.mdecAmt = Convert.ToDecimal(this.mdecTotPdAmt) - Convert.ToDecimal(this.mdecDiscountAmt);
                this.mdecVatAmt = Convert.ToDecimal(this.mdecAmt) * this.mdecVatRate / 100;
                this.mdecGrossAmt = Convert.ToDecimal(this.mdecAmt) + Convert.ToDecimal(this.mdecVatAmt);
            }
            else
            {
                this.mdecGrossAmt = Convert.ToDecimal(this.mdecTotPdAmt) - Convert.ToDecimal(this.mdecDiscountAmt);
                this.mdecVatAmt = Convert.ToDecimal(this.mdecGrossAmt) * (this.mdecVatRate / (100 + this.mdecVatRate));
                this.mdecAmt = Convert.ToDecimal(this.mdecGrossAmt) - Convert.ToDecimal(this.mdecVatAmt);
            }
        }

        private decimal pmQueryProdHistory_BakRev(string inProd, string inRefType)
        {

            decimal decRetVal = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFld = " OrderI.fdDate ,OrderI.fnBackQty ,OrderI.fcLot , OrderI.fdDelivery ";
            strFld += ",OrderH.fcCode as QcOrder, OrderH.fdRecedate ";
            strFld += ",Coor.fcCode as cQcCoor, Coor.fcName as cQnCoor";
            strFld += ",UM.fcName as cQnUM";
            strFld += ",Book.fcCode as cQcBook";

            string strSQLCmd = "select " + strFld + " from OrderI ";
            strSQLCmd += " left join OrderH on OrderH.fcSkid = OrderI.fcOrderH";
            strSQLCmd += " left join Book on Book.fcSkid = OrderH.fcBook";
            strSQLCmd += " left join Coor on Coor.fcSkid = OrderI.fcCoor";
            strSQLCmd += " left join UM on UM.fcSkid = OrderI.fcUM";
            strSQLCmd += " where OrderI.fcCorp = ? ";
            strSQLCmd += " and OrderI.fcBranch = ? ";
            strSQLCmd += " and OrderI.fcRefType = ? ";
            strSQLCmd += " and OrderI.fcStat = ? ";
            strSQLCmd += " and OrderI.fcStep = ? ";
            strSQLCmd += " and OrderI.fcProd = ? ";
            strSQLCmd += " and OrderI.fnBackQty > 0 ";

            if (inRefType == "PR")
            {
                strSQLCmd += (this.mstrCalBook_PR.Trim() != string.Empty ? " and BOOK.FCCODE in " + this.mstrCalBook_PR : "");
            }
            else if (inRefType == "PO")
            {
                strSQLCmd += (this.mstrCalBook_PO.Trim() != string.Empty ? " and BOOK.FCCODE in " + this.mstrCalBook_PO : "");
            }
            strSQLCmd += " order by OrderI.fdDate, OrderH.fcRefNo ";

            string strRefType = SysDef.gc_REFTYPE_P_ORDER;
            //if (this.mstrSaleOrBuy == "S")
            //    strRefType = SysDef.gc_REFTYPE_S_ORDER;
            //else
            //    strRefType = SysDef.gc_REFTYPE_P_ORDER;

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrGenPR_Branch, inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, inProd });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemHistory_BakRev].NewRow();
                dtrTemHis["cRefType"] = inRefType;
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrOrderI["fdDate"]);
                dtrTemHis["cRefNo"] = dtrOrderI["cQcBook"].ToString().TrimEnd() + "/" + dtrOrderI["QcOrder"].ToString();
                dtrTemHis["cQnCoor"] = dtrOrderI["cQnCoor"].ToString();
                dtrTemHis["cQnUM"] = dtrOrderI["cQnUM"].ToString();
                dtrTemHis["nQty"] = Convert.ToDecimal(dtrOrderI["fnBackQty"]);
                dtrTemHis["cLot"] = dtrOrderI["fcLot"].ToString();
                decRetVal = decRetVal + Convert.ToDecimal(dtrOrderI["fnBackQty"]);
                if (!Convert.IsDBNull(dtrOrderI["fdDelivery"]))
                {
                    dtrTemHis["dDueDate"] = Convert.ToDateTime(dtrOrderI["fdDelivery"]);
                }
                else
                {
                    if (!Convert.IsDBNull(dtrOrderI["fdReceDate"]))
                    {
                        dtrTemHis["dDueDate"] = Convert.ToDateTime(dtrOrderI["fdReceDate"]);
                    }
                }

                this.dtsDataEnv.Tables[xd_TemHistory_BakRev].Rows.Add(dtrTemHis);
            }
            return decRetVal;
        }

        private decimal pmQueryProdHistory_BalStk(string inProd, string inWHouse_RM)
        {

            decimal decRetVal = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFld = " REFPROD.FCPROD,REFPROD.FCIOTYPE ";
            strFld += ",sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as fnSumQty ";

            string strSQLCmd = "select " + strFld + " from RefProd ";
            strSQLCmd += " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse";
            strSQLCmd += " where RefProd.fcProd = ? ";
            strSQLCmd += " and RefProd.fcBranch = ? ";
            strSQLCmd += " and RefProd.fcStat <> 'C' ";
            strSQLCmd += " and RefProd.fnQty <> 0 ";
            strSQLCmd += " and WHouse.fcType = ' ' ";
            strSQLCmd += (inWHouse_RM.Trim() != string.Empty ? " and WHOUSE.FCCODE in " + inWHouse_RM : "");
            strSQLCmd += " group by REFPROD.FCPROD,REFPROD.FCIOTYPE ";

            string strRefType = SysDef.gc_REFTYPE_P_ORDER;
            //if (this.mstrSaleOrBuy == "S")
            //    strRefType = SysDef.gc_REFTYPE_S_ORDER;
            //else
            //    strRefType = SysDef.gc_REFTYPE_P_ORDER;

            pobjSQLUtil.SetPara(new object[] { inProd, this.mstrGenPR_Branch });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {
                decimal decQty = (!Convert.IsDBNull(dtrOrderI["fnSumQty"]) ? Convert.ToDecimal(dtrOrderI["fnSumQty"]) : 0) * (dtrOrderI["fcIOType"].ToString() == "I" ? 1 : -1);
                decRetVal += decQty;
            }
            return decRetVal;
        }

        private decimal pmQueryBookingMO(string inProd, string inWOrderI)
        {

            decimal decRetVal = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFld = " MFWORDERHD.CMSTEP, REFDOC.CMASTERTYP , MFWORDERIT_PD.NQTY , REFDOC.CMASTERI , REFDOC.CCHILDI ";

            string strSQLCmd = "select " + strFld + " from REFDOC ";
            strSQLCmd += " left join MFWORDERIT_PD on REFDOC.CMASTERTYP = 'PR' and MFWORDERIT_PD.CROWID = REFDOC.CCHILDI ";
            strSQLCmd += " left join MFWORDERHD on MFWORDERHD.CROWID = MFWORDERIT_PD.CWORDERH ";
            strSQLCmd += " where MFWORDERIT_PD.CPROD = ? ";
            strSQLCmd += " and MFWORDERHD.CMSTEP <> 'P' ";
            strSQLCmd += " and REFDOC.CCHILDI <> ? ";

            pobjSQLUtil.SetPara(new object[] { inProd, inWOrderI });
            //pobjSQLUtil.SetPara(new object[] { inProd });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLCmd, ref strErrorMsg);
            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
            {

                pobjSQLUtil2.SetPara(new object[] { dtrOrderI["cMasterI"].ToString() });
                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", "select fcSKid from OrderI where fcSkid = ?", ref strErrorMsg))
                {
                    decimal decQty = (!Convert.IsDBNull(dtrOrderI["NQTY"]) ? Convert.ToDecimal(dtrOrderI["NQTY"]) : 0);
                    decimal decIssueQty = this.pmQueryMOIssueQty(dtrOrderI["cChildI"].ToString());
                    decRetVal += decQty - decIssueQty;
                }

            }
            return decRetVal;
        }

        private decimal pmQueryMOIssueQty(string inWOrderI)
        {

            decimal decRetVal = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);


            decimal decIssueQty = 0;
            decimal decReturnQty = 0;

            pobjSQLUtil.SetPara(new object[] { "MO", inWOrderI, "WR" });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                {
                    decIssueQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                }
            }

            pobjSQLUtil.SetPara(new object[] { "MO", inWOrderI, "RW" });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CCHILDTYPE = ? and CCHILDI = ? and CMASTERTYP = ?", ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                {
                    decReturnQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                }
            }
            decRetVal = decIssueQty - decReturnQty;

            return decRetVal;
        }

    }
}
