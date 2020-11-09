
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WS.Data;
using WS.Data.Agents;
using AppUtil;
using mBudget.UIHelper;

namespace mBudget.Business.Entity
{
    public class QMasterCoor
    {

        private string mstrRefTable = MapTable.Table.MasterCoor;
        private string mstrRefChildTable = MapTable.Table.Coor;

        private string mstrConnectionString = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;

        private DataSet dtsDataEnv = new DataSet(MapTable.Table.MasterCoor);

        private string mstrSQLPrefix = "cIsSupp = ? ";
        private string mstrSQLPrefix2 = " fcCorp = ? and fcIsSupp = ? and fcCode = ? ";

        private DataRow mdtrCurrCorp = null;

        private string mstrMemoS1 = "";
        private string mstrMemoS2 = "";

        private string mstrCoorType = "C";
        private string mstrFixCorp = "";

        public string FixCorpID
        {
            get { return this.mstrFixCorp; }
            set { this.mstrFixCorp = value; }
        }

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        public void SetSaveDBAgent(WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran)
        {
            this.mSaveDBAgent = inSaveDBAgent;
            this.mdbConn = inDbConn;
            this.mdbTran = inDbTran;
        }

        public void SetSaveDBAgent2(WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran)
        {
            this.mSaveDBAgent2 = inSaveDBAgent;
            this.mdbConn2 = inDbConn;
            this.mdbTran2 = inDbTran;
        }

        public QMasterCoor(string inConnectionString, DBMSType inDataBaseReside)
        {
            this.mstrConnectionString = inConnectionString;
            this.mDataBaseReside = inDataBaseReside;
            this.pmInitComponent();
        }

        public QMasterCoor(string inCoorType, string inConnectionString, DBMSType inDataBaseReside)
        {
            this.mstrCoorType = inCoorType;

            if (this.mstrCoorType == "C")
            {
                this.mstrSQLPrefix = "cIsCust = ? ";
                this.mstrSQLPrefix2 = " fcCorp = ? and fcIsCust = ? and fcCode = ? ";
            }
            else
            {
                this.mstrSQLPrefix = "cIsSupp = ? ";
                this.mstrSQLPrefix2 = " fcCorp = ? and fcIsSupp = ? and fcCode = ? ";
            }

            this.mstrConnectionString = inConnectionString;
            this.mDataBaseReside = inDataBaseReside;
            this.pmInitComponent();
        }

        private void pmInitComponent()
        {
        }

        public DataTable QueryData(object[] inPrefixPara, string inKey, string inAlias)
        {
            return this.pmSearchData(inPrefixPara, inKey, inAlias);
        }

        private DataTable pmSearchData(object[] inPrefixPara, string inKey, string inAlias)
        {
            string strAppUserFldList = "cRowID, cCode, cName, cName2";

            string strErrorMsg = "";
            string strBrowViewAlias = inAlias;
            string strSQLStr = "";
            string strPrefixSQL = "";

            strPrefixSQL = (this.mstrFixCorp != string.Empty ? "cCorp = ? and " : "" ) + this.mstrSQLPrefix;

            strSQLStr = "select " + strAppUserFldList + " from " + MapTable.Table.MasterCoor + " where " + strPrefixSQL
                + " and " + (inKey == "CCODE" ? "CCODE" : "CNAME") + " like ?";

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            objSQLHelper.SetPara(inPrefixPara);
            objSQLHelper.SQLExec(ref this.dtsDataEnv, strBrowViewAlias, MapTable.Table.MasterCoor, strSQLStr, ref strErrorMsg);
            return this.dtsDataEnv.Tables[strBrowViewAlias].Copy();
        }

        public bool SaveChildTable(DataRow inMaster, string inOldCode, string inMemoS1, string inMemoS2, string inQcCrGrp, string inQcAcChart)
        {
            return this.pmSaveChildTable(inMaster, inOldCode, inMemoS1, inMemoS2, inQcCrGrp, inQcAcChart);
        }

        public bool DeleteChildTable(string inMasterCode)
        {
            return this.pmDeleteChildTable(inMasterCode);
        }

        public bool HasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            return this.pmHasUsedChildTable(inSQLHelper, inMasterCode, ref ioErrorMsg);
        }

        private bool pmSaveChildTable(DataRow inMaster, string inOldCode, string inMemoS1, string inMemoS2, string inQcCrGrp, string inQcAcChart)
        {
            string strErrorMsg = "";
            string strEditRowID = "";

            object[] pAPara = null;
            bool bllIsNewRow = false;
            DataRow dtrSaveInfo = null;

            if (this.mstrFixCorp != string.Empty)
            {
                this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp + " where fcSkid = ? ", new object[] { this.mstrFixCorp }, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
            else
            {
                this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, null, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }

            if (this.dtsDataEnv.Tables["QCorp"].Rows.Count > 0)
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    UIBase.WaitWind("กำลังบันทึกข้อมูลบริษัท (" + dtrCorp["fcSkid"].ToString().TrimEnd() + ")");
                    string strSeekCode = (inOldCode.TrimEnd() == "" ? inMaster["cCode"].ToString() : inOldCode);
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), "Y", strSeekCode.TrimEnd() };
                    if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, this.mstrRefChildTable, this.mstrRefChildTable, "select * from " + this.mstrRefChildTable + " where " + this.mstrSQLPrefix2, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        //Insert Child Record
                        bllIsNewRow = true;
                        WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                        strEditRowID = objConn.RunRowID(this.mstrRefChildTable);
                        dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefChildTable].NewRow();
                        //dtrSaveInfo["fcCreateBy"] = App.FMAppUserID;
                    }
                    else
                    {
                        //Update Child Record
                        dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefChildTable].Rows[0];
                        strEditRowID = dtrSaveInfo["fcSkid"].ToString();
                        bllIsNewRow = false;
                    }

                    string strCrGrp = "";
                    string strAcChart = "";

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcAcChart.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), this.mstrCoorType, inQcCrGrp.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCrGrp", "CRGRP", "select fcSkid from CRGRP where fcCorp = ? and fcCoorType = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strCrGrp = this.dtsDataEnv.Tables["QCrGrp"].Rows[0]["fcSkid"].ToString();
                    }

                    dtrSaveInfo["fcSkid"] = strEditRowID;
                    dtrSaveInfo["fcCorp"] = dtrCorp["fcSkid"].ToString();

                    if (this.mstrCoorType == "C")
                    {
                        dtrSaveInfo["fcIsCust"] = "Y";
                        dtrSaveInfo["fcIsSupp"] = "";
                    }
                    else
                    {
                        dtrSaveInfo["fcIsCust"] = "";
                        dtrSaveInfo["fcIsSupp"] = "Y";
                    }

                    dtrSaveInfo["fcCode"] = inMaster["cCode"].ToString().TrimEnd();
                    dtrSaveInfo["fcName"] = inMaster["cName"].ToString().TrimEnd();
                    dtrSaveInfo["fcName2"] = inMaster["cName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChr"] = inMaster["cFChr"].ToString().TrimEnd();
                    dtrSaveInfo["fcSName"] = inMaster["cSName"].ToString().TrimEnd();
                    dtrSaveInfo["fcSName2"] = inMaster["cSName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChrS"] = inMaster["cFChrS"].ToString().TrimEnd();
                    dtrSaveInfo["fcZip"] = inMaster["cZip"].ToString();
                    dtrSaveInfo["fcStatus"] = inMaster["cStatus"].ToString();
                    dtrSaveInfo["fdInActive"] = (Convert.IsDBNull(inMaster["dInActive"]) ? Convert.DBNull : Convert.ToDateTime(inMaster["dInActive"]).Date);
                    dtrSaveInfo["fdFstCont"] = (Convert.IsDBNull(inMaster["dFstCont"]) ? Convert.DBNull : Convert.ToDateTime(inMaster["dFstCont"]).Date);

                    dtrSaveInfo["fcPersonty"] = inMaster["cPersonty"].ToString();
                    dtrSaveInfo["fcContactN"] = inMaster["cContactN"].ToString().TrimEnd();
                    dtrSaveInfo["fcCrGrp"] = inMaster["cCrGrp"].ToString();
                    dtrSaveInfo["fcCrZone"] = inMaster["cCrZone"].ToString();
                    dtrSaveInfo["fcEmpl"] = inMaster["cEmpl"].ToString();
                    dtrSaveInfo["fcDeliEmpl"] = inMaster["cDeliEmpl"].ToString();
                    dtrSaveInfo["fcLayEmpl"] = inMaster["cLayEmpl"].ToString();
                    dtrSaveInfo["fnCredTerm"] = Convert.ToInt32(inMaster["nCredTerm"]);
                    dtrSaveInfo["fnCredLim"] = Convert.ToDecimal(inMaster["nCredLim"]);
                    dtrSaveInfo["fnBlack"] = Convert.ToInt32(inMaster["nBlack"]);
                    dtrSaveInfo["fcPriceNo"] = inMaster["cPriceNo"].ToString();
                    dtrSaveInfo["fcGrade"] = inMaster["cGrade"].ToString();

                    dtrSaveInfo["fcVatCoor"] = inMaster["cVatCoor"].ToString();
                    dtrSaveInfo["fcDeliCoor"] = inMaster["cDeliCoor"].ToString();
                    dtrSaveInfo["fcColCoor"] = inMaster["cColCoor"].ToString();
                    dtrSaveInfo["fcPolicyPr"] = inMaster["cPolicyPr"].ToString();
                    dtrSaveInfo["fcPolicyDi"] = inMaster["cPolicyDi"].ToString();
                    dtrSaveInfo["fcVatType"] = inMaster["cVatType"].ToString();
                    dtrSaveInfo["fcBank"] = inMaster["cBank"].ToString();
                    dtrSaveInfo["fcDiscStr"] = inMaster["cDiscStr"].ToString();
                    dtrSaveInfo["fcCurrency"] = inMaster["cCurrency"].ToString();
                    
                    //dtrSaveInfo["fcCrGrp"] = strCrGrp;
                    dtrSaveInfo["fcAcChart"] = strAcChart;

                    dtrSaveInfo["fcBankNo"] = inMaster["cBankNo"].ToString().TrimEnd();
                    dtrSaveInfo["fcBBranch"] = inMaster["cBBranch"].ToString().TrimEnd();

                    dtrSaveInfo["fmMapName"] = inMaster["cMapName"].ToString();

                    dtrSaveInfo["fmMemData"] = inMaster["cMemData"].ToString();
                    dtrSaveInfo["fmMemData2"] = inMaster["cMemData2"].ToString();
                    dtrSaveInfo["fmMemData3"] = inMaster["cMemData3"].ToString();
                    dtrSaveInfo["fmMemData4"] = inMaster["cMemData4"].ToString();
                    dtrSaveInfo["fmMemData5"] = inMaster["cMemData5"].ToString();

                    dtrSaveInfo["fcCreateAp"] = this.mSaveDBAgent2.AppID;
                    dtrSaveInfo["ftLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime(); ;
                    dtrSaveInfo["fcEAfterR"] = "E";

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "fcSkid", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    this.mstrMemoS1 = inMemoS1;
                    this.mstrMemoS2 = inMemoS2;
                    this.pmSaveCoorX5(strEditRowID);

                }
            }
            return true;
        }

        private bool pmSaveCoorX5(string inCoor)
        {

            string strErrorMsg = "";
            bool bllIsNewRow = false;
            string strRowID = "";
            object[] pAPara = null;

            DataRow dtrRProdX1 = null;
            if (this.mstrMemoS1.Trim() + this.mstrMemoS2.Trim() != string.Empty)
            {
                if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "COORX5", "COORX5", "select * from COORX5 where fcCoor = ?", new object[1] { inCoor }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                {

                    bllIsNewRow = true;

                    strRowID = App.mRunRowID("COORX5");
                    dtrRProdX1 = this.dtsDataEnv.Tables["COORX5"].NewRow();
                    dtrRProdX1["fcSkid"] = strRowID;
                    //dtrRProdX1["cCreateBy"] = App.FMAppUserID;
                }
                else
                {
                    bllIsNewRow = false;
                    strRowID = this.dtsDataEnv.Tables["COORX5"].Rows[0]["fcSkid"].ToString();
                    dtrRProdX1 = this.dtsDataEnv.Tables["COORX5"].Rows[0];
                }

                dtrRProdX1["fcCorp"] = App.gcCorp;
                dtrRProdX1["fcCoor"] = inCoor;
                dtrRProdX1["fmMemData"] = this.mstrMemoS1;
                dtrRProdX1["fmMemData2"] = this.mstrMemoS2;
                //dtrRProdX1["cLastUpdBy"] = App.FMAppUserID;
                //dtrRProdX1["dLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime();

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRProdX1, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
            else
            {

                pAPara = new object[] { inCoor };
                this.mSaveDBAgent2.BatchSQLExec("delete from COORX5 where FCCOOR = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

            }
            return true;

        }

        private bool pmDeleteChildTable(string inMasterCode)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            if (this.mstrFixCorp != string.Empty)
            {
                this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp + " where fcSkid = ? ", new object[] { this.mstrFixCorp }, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
            else
            {
                this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, null, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }

            if (this.dtsDataEnv.Tables["QCorp"].Rows.Count > 0)
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    UIBase.WaitWind("กำลังลบข้อมูลบริษัท (" + dtrCorp["fcSkid"].ToString().TrimEnd() + ")");

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), "Y", inMasterCode };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid from " + MapTable.Table.Coor + " where " + this.mstrSQLPrefix2, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        string strCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0]["fcSkid"].ToString();
                        pAPara = new object[] { strCoor };
                        this.mSaveDBAgent2.BatchSQLExec("delete from COORX5 where fcCoor = ? ", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), "Y", inMasterCode };
                    this.mSaveDBAgent2.BatchSQLExec("delete from " + this.mstrRefChildTable + " where " + this.mstrSQLPrefix2, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                }
            }
            return true;
        }

        private bool pmHasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            bool bllResult = false;
            bool bllChkResult = false;
            string strRefErrorMsg = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            if (this.mstrFixCorp != string.Empty)
            {
                objSQLHelper.SetPara(new object[] { this.mstrFixCorp });
                objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp + " where fcSkid = ? ", ref strErrorMsg);
            }
            else
            {
                objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, ref strErrorMsg);
            }

            if (this.dtsDataEnv.Tables["QCorp"].Rows.Count > 0)
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    this.mdtrCurrCorp = dtrCorp;
                    objSQLHelper.SetPara(new object[] { dtrCorp["fcSkid"].ToString(), "Y", inMasterCode.TrimEnd() });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefChild", this.mstrRefChildTable, "select fcSkid, fcCode, fcName from " + this.mstrRefChildTable + " where " + this.mstrSQLPrefix2, ref strErrorMsg))
                    {
                        DataRow dtrChildTab = this.dtsDataEnv.Tables["QRefChild"].Rows[0];
                        bllChkResult = this.pmHasUsedChild1Corp(objSQLHelper, dtrChildTab["fcSkid"].ToString(), ref ioErrorMsg);
                        if (bllChkResult)
                            strRefErrorMsg += "\r\n" + ioErrorMsg;

                        bllResult = (bllResult || bllChkResult);
                    }
                }
            }
            ioErrorMsg = strRefErrorMsg;
            return bllResult;
        }

        private bool pmHasUsedChild1Corp(cDBMSAgent inSQLHelper, string inRowID, ref string ioErrorMsg)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strRefMsg = "";

            inSQLHelper.SetPara(new object[] { inRowID });
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.Department, "select fcSkid, fcCode, fcRefNo from " + MapTable.Table.GLRef + " where fcCoor = ?", ref strErrorMsg))
            {
                string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    //strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                    strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
                }
                ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
                return true;
            }
            inSQLHelper.SetPara(new object[] { inRowID });
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.OrderH, "select fcSkid, fcCode, fcRefNo from " + MapTable.Table.OrderH + " where fcCoor = ?", ref strErrorMsg))
            {
                string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    //strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                    strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
                }
                ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
                return true;
            }
            return bllResult;
        }

    }

}
