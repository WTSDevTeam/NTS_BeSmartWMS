
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
    public class QMasterAcChart
    {
     
        private string mstrRefTable = MapTable.Table.MasterAcChart;
        private string mstrRefChildTable = MapTable.Table.AcChart;

        private string mstrConnectionString = "";
        private DBMSType mDataBaseReside = DBMSType.MSSQLServer;

        private DataSet dtsDataEnv = new DataSet(MapTable.Table.MasterAcChart);

        private string mstrSQLPrefix = "";
        private string mstrSQLPrefix2 = " fcCorpChar = ? and fcCode = ? ";

        private DataRow mdtrCurrCorp = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
		System.Data.IDbTransaction mdbTran =null;

		WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
		System.Data.IDbConnection mdbConn2 = null;
		System.Data.IDbTransaction mdbTran2 =null;

        private bool mbllIsPopUp = false;
        public bool IsPopUp
        {
            get { return this.mbllIsPopUp; }
            set { this.mbllIsPopUp = value; }
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

        public QMasterAcChart(string inConnectionString, DBMSType inDataBaseReside)
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

            this.mstrSQLPrefix = (this.mbllIsPopUp ? " cCateg = 'D' and " : "");
            strSQLStr = "select " + strAppUserFldList + " from " + MapTable.Table.MasterAcChart + " where " + this.mstrSQLPrefix
                + " " + (inKey == "CCODE" ? "CCODE" : "CNAME") + " like ?";

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDataBaseReside);
            objSQLHelper.SetPara(inPrefixPara);
            objSQLHelper.SQLExec(ref this.dtsDataEnv, strBrowViewAlias, MapTable.Table.MasterAcChart, strSQLStr, ref strErrorMsg);
            return this.dtsDataEnv.Tables[strBrowViewAlias].Copy();
        }

        public bool SaveChildTable(DataRow inMaster, string inOldCode)
        {
            return this.pmSaveChildTable(inMaster, inOldCode);
        }

        public bool DeleteChildTable(string inMasterCode)
        {
            return this.pmDeleteChildTable(inMasterCode);
        }

        public bool HasUsedChildTable(cDBMSAgent inSQLHelper, string inMasterCode, ref string ioErrorMsg)
        {
            return this.pmHasUsedChildTable(inSQLHelper, inMasterCode, ref ioErrorMsg);
        }

        private bool pmSaveChildTable(DataRow inMaster, string inOldCode)
        {
            string strErrorMsg = "";
            string strEditRowID = "";
            bool bllHasChgCode = false;

            object[] pAPara = null;
            bool bllIsNewRow = false;
            DataRow dtrSaveInfo = null;

            //เพื่อให้รองรับการใช้ FCCORPCHAR ร่วมกัน
            //if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcSkid, fcCode, fcName from " + MapTable.Table.Corp, null, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
            if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select fcCorpChar from " + MapTable.Table.Corp + " group by fcCorpChar", null, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
            {
                foreach (DataRow dtrCorp in this.dtsDataEnv.Tables["QCorp"].Rows)
                {
                    UIBase.WaitWind("กำลังบันทึกข้อมูลบริษัท (" + dtrCorp["fcCorpChar"].ToString().TrimEnd() + ")");
                    string strSeekCode = (inOldCode.TrimEnd() == "" ? inMaster["cCode"].ToString() : inOldCode);
                    pAPara = new object[] { dtrCorp["fcCorpChar"].ToString(), strSeekCode.TrimEnd() };
                    if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, this.mstrRefChildTable, this.mstrRefChildTable, "select * from " + this.mstrRefChildTable + " where " + this.mstrSQLPrefix2, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                    {
                        //Insert Child Record
                        bllIsNewRow = true;
                        bllHasChgCode = false;
                        WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                        strEditRowID = objConn.RunRowID(this.mstrRefChildTable);
                        dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefChildTable].NewRow();
                        dtrSaveInfo["fcCreateBy"] = App.FMAppUserID;
                    }
                    else
                    {
                        //Update Child Record
                        dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefChildTable].Rows[0];
                        strEditRowID = dtrSaveInfo["fcSkid"].ToString();
                        bllIsNewRow = false;

                        if (dtrSaveInfo["fcCode"].ToString().TrimEnd() != inMaster["cCode"].ToString().TrimEnd())
                        {
                            bllHasChgCode = true;
                        }
                    }

                    dtrSaveInfo["fcSkid"] = strEditRowID;
                    dtrSaveInfo["fcCorpchar"] = dtrCorp["fcCorpChar"].ToString();
                    dtrSaveInfo["fcGroup"] = inMaster["cGroup"].ToString();
                    dtrSaveInfo["fcCode"] = inMaster["cCode"].ToString().TrimEnd();
                    dtrSaveInfo["fcName"] = inMaster["cName"].ToString().TrimEnd();
                    dtrSaveInfo["fcFChr"] = inMaster["cFChr"].ToString().TrimEnd();
                    dtrSaveInfo["fcName2"] = inMaster["cName2"].ToString().TrimEnd();
                    dtrSaveInfo["fcCateg"] = (inMaster["cCateg"].ToString().TrimEnd() == "G" ? "กลุ่ม" : "บัญชี");
                    dtrSaveInfo["fcCateg2"] = (inMaster["cCateg"].ToString().TrimEnd() == "G" ? "GENERAL" : "DETAIL");
                    dtrSaveInfo["fnLevel"] = inMaster["cCode"].ToString().TrimEnd().Length;
                    dtrSaveInfo["fcCreateAp"] = this.mSaveDBAgent2.AppID;
                    dtrSaveInfo["ftLastUpd"] = this.mSaveDBAgent2.GetDBServerDateTime(); ;
                    dtrSaveInfo["fcEAfterR"] = "E";

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "fcSkid", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

                    if (bllHasChgCode)
                    {
                        pAPara = new object[] { strEditRowID };
                        if (this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, "QAsset", "ASSET", "select fcSkid from Asset where Asset.fcAcChart = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
                        {
                            pAPara = new object[] { inMaster["cCode"].ToString(), strEditRowID };
                            this.mSaveDBAgent2.BatchSQLExec("Update Asset set fcQcAcChar = ? where Asset.fcAcChart = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
                        }
                    }

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
            if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.Department, "select fcSkid, fcCode, fcName from " + MapTable.Table.Department + " where fcDept = ?", ref strErrorMsg))
            {
                string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
                foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
                {
                    strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
                }
                ioErrorMsg = strCorpStr + "\r\nแผนก" + strRefMsg+ "\r\n";
                return true;
            }
            return bllResult;
        }

    }

}
