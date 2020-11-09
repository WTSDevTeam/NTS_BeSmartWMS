
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
    public class QMasterCrGrp
    {

        private string mstrRefTable = MapTable.Table.MasterCrGrp;
        private string mstrRefChildTable = MapTable.Table.CoorGroup;

        private string mstrConnectionString = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;

        private DataSet dtsDataEnv = new DataSet(MapTable.Table.MasterCrGrp);

        private string mstrSQLPrefix = "";
        private string mstrSQLPrefix2 = " fcCorp = ? and fcCoorType = ? and fcCode = ? ";

        private DataRow mdtrCurrCorp = null;
        private CoorType mCoorType = CoorType.Supplier;
        private string mstrCoorType = "S";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
		System.Data.IDbTransaction mdbTran =null;

		WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
		System.Data.IDbConnection mdbConn2 = null;
		System.Data.IDbTransaction mdbTran2 =null;

        private string mstrFixCorp = "";
        public string FixCorpID
        {
            get { return this.mstrFixCorp; }
            set { this.mstrFixCorp = value; }
        }

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

        public QMasterCrGrp(CoorType inCoorType, string inConnectionString, DBMSType inDataBaseReside)
        {
            this.mCoorType = inCoorType;
            if (this.mCoorType == CoorType.Customer)
            {
                this.mstrCoorType = "C";
            }
            else
            {
                this.mstrCoorType = "S";
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

            strPrefixSQL = (this.mstrFixCorp != string.Empty ? "cCorp = ? and " : "") + this.mstrSQLPrefix;

            strSQLStr = "select " + strAppUserFldList + " from " + MapTable.Table.MasterCrGrp + " where " + this.mstrSQLPrefix
                + " and " + (inKey == "CCODE" ? "CCODE" : "CNAME") + " like ?";

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            objSQLHelper.SetPara(inPrefixPara);
            objSQLHelper.SQLExec(ref this.dtsDataEnv, strBrowViewAlias, MapTable.Table.MasterCrGrp, strSQLStr, ref strErrorMsg);
            return this.dtsDataEnv.Tables[strBrowViewAlias].Copy();
        }

        public bool SaveChildTable(DataRow inMaster, string inOldCode, string inAccCode)
        {
            return this.pmSaveChildTable(inMaster, inOldCode, inAccCode);
        }

        public bool DeleteChildTable(string inMasterCode)
        {
            return this.pmDeleteChildTable(inMasterCode);
        }

        public bool HasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            return this.pmHasUsedChildTable(inSQLHelper, inMasterCode, ref ioErrorMsg);
        }

        private bool pmSaveChildTable(DataRow inMaster, string inOldCode, string inAccCode)
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
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), this.mstrCoorType, strSeekCode.TrimEnd() };
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

                    string strAcChart = "";
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), inAccCode.TrimEnd() };
                    if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAcChart", "ACCHART", "select fcSkid from ACCHART where fcCorpChar = ? and fcCode = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        strAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0]["fcSkid"].ToString();
                    }

                    dtrSaveInfo["fcSkid"] = strEditRowID;
                    dtrSaveInfo["fcCorp"] = dtrCorp["fcSkid"].ToString();
                    dtrSaveInfo["fcCoorType"] = inMaster["cCoorType"].ToString().TrimEnd();
                    dtrSaveInfo["fcCode"] = inMaster["cCode"].ToString().TrimEnd();
                    dtrSaveInfo["fcName"] = inMaster["cName"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChr"] = inMaster["cFChr"].ToString().TrimEnd();
                    dtrSaveInfo["fcName2"] = inMaster["cName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcAcChart"] = strAcChart;
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
                    UIBase.WaitWind("กำลังลบข้อมูลบริษัท (" + dtrCorp["fcSkid"].ToString().TrimEnd() + ")");
                    pAPara = new object[] { dtrCorp["fcSkid"].ToString(), this.mstrCoorType, inMasterCode };
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
                    objSQLHelper.SetPara(new object[] { dtrCorp["fcSkid"].ToString(), this.mstrCoorType, inMasterCode.TrimEnd() });
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
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.Department, "select fcSkid, fcCode, fcName from COOR where fcCrGrp = ?", ref strErrorMsg))
            {
                string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                }
                ioErrorMsg = strCorpStr + "\r\nผู้จำหน่าย" + strRefMsg+ "\r\n";
                return true;
            }
            return bllResult;
        }

    }

}
