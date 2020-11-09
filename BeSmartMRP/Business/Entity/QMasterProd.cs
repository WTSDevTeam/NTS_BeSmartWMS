
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

    public class QMasterProd
    {

        private string mstrRefTable = MapTable.Table.MasterProd;
        private string mstrRefChildTable = MapTable.Table.Product;

        private string mstrConnectionString = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;

        private DataSet dtsDataEnv = new DataSet(MapTable.Table.MasterProd);

        private string mstrSQLPrefix = "";
        private string mstrSQLPrefix2 = " fcCorp = ? and fcCode = ? ";

        private DataRow mdtrCurrCorp = null;

        private string mstrMemoS1 = "";
        private string mstrMemoS2 = "";
        private string mstrMemoS3 = "";
        private string mstrMemoS4 = "";
        private string mstrMemoS5 = "";

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

        public QMasterProd(string inConnectionString, DBMSType inDataBaseReside)
        {
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

            strPrefixSQL = (this.mstrFixCorp != string.Empty ? "cCorp = ? and " : "") + this.mstrSQLPrefix;

            strSQLStr = "select " + strAppUserFldList + " from " + MapTable.Table.MasterProd + " where " + this.mstrSQLPrefix
                + " and " + (inKey == "CCODE" ? "CCODE" : "CNAME") + " like ?";

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            objSQLHelper.SetPara(inPrefixPara);
            objSQLHelper.SQLExec(ref this.dtsDataEnv, strBrowViewAlias, MapTable.Table.MasterProd, strSQLStr, ref strErrorMsg);
            return this.dtsDataEnv.Tables[strBrowViewAlias].Copy();
        }

        public bool SaveChildTable(DataRow inMaster, string inOldCode
            , string inMemoS1
            , string inMemoS2
            , string inMemoS3
            , string inMemoS4
            , string inMemoS5
            , string inQcPdGrp
            , string inQcSuppl
            , string inQcUM
            , string inQcUM1
            , string inQcUM2
            , string inQcStUm
            , string inQcStUm1
            , string inQcStUm2
            , string inAccCode1
            , string inAccCode2
            , string inAccCode3
            , string inAccCode4)
        {
            return this.pmSaveChildTable(inMaster, inOldCode, inMemoS1, inMemoS2, inMemoS3, inMemoS4, inMemoS5, inQcPdGrp, inQcSuppl, inQcUM, inQcUM1, inQcUM2, inQcStUm, inQcStUm1, inQcStUm2, inAccCode1, inAccCode2, inAccCode3, inAccCode4);
        }

        public bool DeleteChildTable(string inMasterCode)
        {
            return this.pmDeleteChildTable(inMasterCode);
        }

        public bool HasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            return this.pmHasUsedChildTable(inSQLHelper, inMasterCode, ref ioErrorMsg);
        }

        private bool pmSaveChildTable(DataRow inMaster, string inOldCode
            , string inMemoS1
            , string inMemoS2
            , string inMemoS3
            , string inMemoS4
            , string inMemoS5
            , string inQcPdGrp
            , string inQcSuppl
            , string inQcUM
            , string inQcUM1
            , string inQcUM2
            , string inQcStUm
            , string inQcStUm1
            , string inQcStUm2
            , string inAccCode1
            , string inAccCode2
            , string inAccCode3
            , string inAccCode4)
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
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), strSeekCode.TrimEnd() };
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

                    string strAccBCash = "";
                    string strAccBCred = "";
                    string strAccSCash = "";
                    string strAccSCred = "";
                    string strPdGrp = "";
                    string strCoor = "";
                    string strUM = "";
                    string strUM1 = "";
                    string strUM2 = "";
                    string strStUm = "";
                    string strStUm1 = "";
                    string strStUm2 = "";

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode1.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccBCash = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode2.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccBCred = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode3.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccSCash = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode4.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccSCred = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcPdGrp.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QPdGrp", "PDGRP", "select fcSkid from PDGRP where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strPdGrp = this.dtsDataEnv.Tables["QPdGrp"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcSuppl.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid from COOR where fcCorp = ? and fcIsSupp = 'Y' and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcUM.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcUM1.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strUM1 = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcUM2.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strUM2 = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcStUm.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strStUm = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcStUm1.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strStUm1 = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inQcStUm2.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strStUm2 = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                    }
                    
                    dtrSaveInfo["fcSkid"] = strEditRowID;
                    dtrSaveInfo["fcCorp"] = dtrCorp["fcSkid"].ToString();
                    dtrSaveInfo["fcType"] = inMaster["cType"].ToString().TrimEnd();
                    dtrSaveInfo["fcCode"] = inMaster["cCode"].ToString().TrimEnd();
                    dtrSaveInfo["fcName"] = inMaster["cName"].ToString().TrimEnd();
                    dtrSaveInfo["fcSName"] = inMaster["cSName"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChr"] = inMaster["cFChr"].ToString().TrimEnd();
                    dtrSaveInfo["fcName2"] = inMaster["cName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcSName2"] = inMaster["cSName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcVatIsOut"] = inMaster["cVatIsOut"].ToString().TrimEnd();
                    dtrSaveInfo["fcCtrlStoc"] = inMaster["cCtrlStoc"].ToString().TrimEnd();
                    dtrSaveInfo["fnIsConsum"] = Convert.ToInt32(inMaster["nIsConsum"]);
                    dtrSaveInfo["fnStdCost"] = Convert.ToDecimal(inMaster["nStdCost"]);

                    dtrSaveInfo["fcStatus"] = inMaster["cStatus"].ToString();
                    dtrSaveInfo["fdInActive"] = (Convert.IsDBNull(inMaster["dInActive"]) ? Convert.DBNull : Convert.ToDateTime(inMaster["dInActive"]).Date);

                    dtrSaveInfo["fmPicName"] = inMaster["cPicName"].ToString();

                    dtrSaveInfo["fcUM"] = strUM;
                    dtrSaveInfo["fcUM1"] = strUM1;
                    dtrSaveInfo["fcUM2"] = strUM2;
                    dtrSaveInfo["fcStUm"] = strStUm;
                    dtrSaveInfo["fcStUm1"] = strStUm1;
                    dtrSaveInfo["fcStUm2"] = strStUm2;

                    dtrSaveInfo["fnUmQty1"] = Convert.ToDecimal(inMaster["nUmQty1"]);
                    dtrSaveInfo["fnUmQty2"] = Convert.ToDecimal(inMaster["nUmQty2"]);
                    dtrSaveInfo["fnStUmQty1"] = Convert.ToDecimal(inMaster["nStUmQty1"]);
                    dtrSaveInfo["fnStUmQty2"] = Convert.ToDecimal(inMaster["nStUmQty2"]);

                    dtrSaveInfo["fcPdGrp"] = strPdGrp;
                    dtrSaveInfo["fcSupp"] = strCoor;

                    dtrSaveInfo["fcAccBCash"] = strAccBCash;
                    dtrSaveInfo["fcAccBCred"] = strAccBCred;
                    dtrSaveInfo["fcAccSCash"] = strAccSCash;
                    dtrSaveInfo["fcAccSCred"] = strAccSCred;

                    //dtrSaveInfo["fcAccSCost"] = strAccSCost;
                    //dtrSaveInfo["fcAccItem"] = strAccItem;

                    dtrSaveInfo["fcCreateAp"] = this.mSaveDBAgent2.AppID;
                    dtrSaveInfo["ftLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime(); ;
                    dtrSaveInfo["fcEAfterR"] = "E";

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "fcSkid", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    this.mstrMemoS1 = inMemoS1;
                    this.mstrMemoS2 = inMemoS2;
                    this.mstrMemoS3 = inMemoS3;
                    this.mstrMemoS4 = inMemoS4;
                    this.mstrMemoS5 = inMemoS5;
                    this.pmSaveProdX4(strEditRowID);

                }
            }
            return true;
        }

        private bool pmSaveProdX4(string inProd)
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            string strRowID = "";
            object[] pAPara = null;

            DataRow dtrRProdX1 = null;
            if (this.mstrMemoS1.Trim() + this.mstrMemoS2.Trim() + this.mstrMemoS3.Trim() + this.mstrMemoS4.Trim() + this.mstrMemoS5.Trim() != string.Empty)
            {
                if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "PRODX4", "PRODX4", "select * from PRODX4 where fcProd = ?", new object[1] { inProd }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                {

                    bllIsNewRow = true;

                    strRowID = App.mRunRowID("PRODX4");
                    dtrRProdX1 = this.dtsDataEnv.Tables["PRODX4"].NewRow();
                    dtrRProdX1["fcSkid"] = strRowID;
                    //dtrRProdX1["cCreateBy"] = App.FMAppUserID;
                }
                else
                {
                    bllIsNewRow = false;
                    strRowID = this.dtsDataEnv.Tables["PRODX4"].Rows[0]["fcSkid"].ToString();
                    dtrRProdX1 = this.dtsDataEnv.Tables["PRODX4"].Rows[0];
                }

                dtrRProdX1["fcCorp"] = App.gcCorp;
                dtrRProdX1["fcProd"] = inProd;
                dtrRProdX1["fmMemData"] = this.mstrMemoS1;
                dtrRProdX1["fmMemData2"] = this.mstrMemoS2;
                dtrRProdX1["fmMemData3"] = this.mstrMemoS3;
                dtrRProdX1["fmMemData4"] = this.mstrMemoS4;
                dtrRProdX1["fmMemData5"] = this.mstrMemoS5;
                //dtrRProdX1["cLastUpdBy"] = App.FMAppUserID;
                //dtrRProdX1["dLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime();

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrRProdX1, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
            }
            else
            {

                pAPara = new object[] { inProd };
                this.mSaveDBAgent2.BatchSQLExec("delete from PRODX4 where FCPROD = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

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

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inMasterCode };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid from " + MapTable.Table.Product + " where " + this.mstrSQLPrefix2, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        string strProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSkid"].ToString();
                        pAPara = new object[] { strProd };
                        this.mSaveDBAgent2.BatchSQLExec("delete from PRODX4 where fcProd = ? ", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                    }
                    
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inMasterCode };
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
                    objSQLHelper.SetPara(new object[] { dtrCorp["fcSkid"].ToString(), inMasterCode.TrimEnd() });
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
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.RefProd, "select GLRef.fcSkid, GLRef.fcCode, GLRef.fcRefNo from RefProd left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF where RefProd.fcProd = ?", ref strErrorMsg))
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
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.OrderI, "select OrderH.fcSkid, OrderH.fcCode, OrderH.fcRefNo from OrderI left join ORDERH on OrderH.FCSKID = OrderI.fcOrderH where OrderI.fcProd = ?", ref strErrorMsg))
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
