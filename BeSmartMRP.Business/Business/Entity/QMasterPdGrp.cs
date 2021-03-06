
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
    public class QMasterPdGrp
    {
     
        private string mstrRefTable = MapTable.Table.MasterPdGrp;
        private string mstrRefChildTable = MapTable.Table.ProdGroup;

        private string mstrConnectionString = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;

        private DataSet dtsDataEnv = new DataSet(MapTable.Table.MasterPdGrp);

        private string mstrSQLPrefix = "";
        private string mstrSQLPrefix2 = " fcCorp = ? and fcCode = ? ";

        private DataRow mdtrCurrCorp = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
		System.Data.IDbTransaction mdbTran =null;

		WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
		System.Data.IDbConnection mdbConn2 = null;
		System.Data.IDbTransaction mdbTran2 =null;

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

        public QMasterPdGrp(string inConnectionString, DBMSType inDataBaseReside)
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

            strSQLStr = "select " + strAppUserFldList + " from " + MapTable.Table.MasterPdGrp + " where " + this.mstrSQLPrefix
                + " " + (inKey == "CCODE" ? "CCODE" : "CNAME") + " like ?";

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            objSQLHelper.SetPara(inPrefixPara);
            objSQLHelper.SQLExec(ref this.dtsDataEnv, strBrowViewAlias, MapTable.Table.MasterPdGrp, strSQLStr, ref strErrorMsg);
            return this.dtsDataEnv.Tables[strBrowViewAlias].Copy();
        }

        public bool SaveChildTable(DataRow inMaster, string inOldCode, string inAccCode1, string inAccCode2, string inAccCode3, string inAccCode4, string inAccCode5, string inAccCode6)
        {
            return this.pmSaveChildTable(inMaster, inOldCode, inAccCode1, inAccCode2, inAccCode3, inAccCode4, inAccCode5, inAccCode6);
        }

        public bool DeleteChildTable(string inMasterCode)
        {
            return this.pmDeleteChildTable(inMasterCode);
        }

        public bool HasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            return this.pmHasUsedChildTable(inSQLHelper, inMasterCode, ref ioErrorMsg);
        }

        private bool pmSaveChildTable(DataRow inMaster, string inOldCode, string inAccCode1, string inAccCode2, string inAccCode3, string inAccCode4, string inAccCode5, string inAccCode6)
        {
            string strErrorMsg = "";
            string strEditRowID = "";

            object[] pAPara = null;
            bool bllIsNewRow = false;
            DataRow dtrSaveInfo = null;

            if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, null, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    UIBase.WaitWind("���ѧ�ѹ�֡�����ź���ѷ (" + dtrCorp["fcSkid"].ToString().TrimEnd() + ")");
                    string strSeekCode = ( inOldCode.TrimEnd() == "" ? inMaster["cCode"].ToString() : inOldCode );
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), strSeekCode.TrimEnd() };
                    if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, this.mstrRefChildTable, this.mstrRefChildTable, "select * from " + this.mstrRefChildTable + " where " + this.mstrSQLPrefix2 , pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
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
                    string strAccSCost = "";
                    string strAccItem = "";

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

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode5.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccSCost = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode6.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAccItem = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    dtrSaveInfo["fcSkid"] = strEditRowID;
                    dtrSaveInfo["fcCorp"] = dtrCorp["fcSkid"].ToString();
                    dtrSaveInfo["fcType"] = "G";
                    dtrSaveInfo["fcCode"] = inMaster["cCode"].ToString().TrimEnd();
                    dtrSaveInfo["fcName"] = inMaster["cName"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChr"] = inMaster["cFChr"].ToString().TrimEnd();
                    dtrSaveInfo["fcName2"] = inMaster["cName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcAccBCash"] = strAccBCash;
                    dtrSaveInfo["fcAccBCred"] = strAccBCred;
                    dtrSaveInfo["fcAccSCash"] = strAccSCash;
                    dtrSaveInfo["fcAccSCred"] = strAccSCred;
                    dtrSaveInfo["fcAccSCost"] = strAccSCost;
                    dtrSaveInfo["fcAccItem"] = strAccItem;
                    
                    dtrSaveInfo["fcCreateAp"] = this.mSaveDBAgent2.AppID;
                    dtrSaveInfo["ftLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime(); ;
                    dtrSaveInfo["fcEAfterR"] = "E";

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "fcSkid", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                }
            }
            return true;
        }

        private bool pmDeleteChildTable(string inMasterCode)
        {
            string strErrorMsg = "";
            object[] pAPara = null;
 
            if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, null, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    UIBase.WaitWind("���ѧź�����ź���ѷ (" + dtrCorp["fcSkid"].ToString().TrimEnd() + ")");
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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, ref strErrorMsg))
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
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.Department, "select fcSkid, fcCode, fcName from " + MapTable.Table.Product + " where fcPdGrp = ?", ref strErrorMsg))
            {
                string strCorpStr = "����ѷ (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                }
                ioErrorMsg = strCorpStr + "\r\n�Թ���" + strRefMsg+ "\r\n";
                return true;
            }
            return bllResult;
        }

    }

}
