using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WS.Data;
using WS.Data.Agents;
using AppUtil;
using mBudget.UIHelper;
using mBudget.Business.Entity;

namespace mBudget.Business.Agents
{
    public class KeepLogAgent
    {


        private static DataSet dtsDataEnv = new DataSet();
        private static string mstrRefTable = Entity.MapTable.Table.KeepLog;

        public KeepLogAgent()
        { }

        public static void KeepLog(WS.Data.Agents.cDBMSAgent inSQLHelper, Entity.KeepLogType inType, string inTaskName, string inCode, string inName, string inAppUser, string inLoginName)
        {
            KeepLogAgent.pmSaveKeepLog(inSQLHelper, inType, inTaskName, inCode, inName, inAppUser, inLoginName, "", "");
        }

        public static void KeepLogChgValue(WS.Data.Agents.cDBMSAgent inSQLHelper, Entity.KeepLogType inType, string inTaskName, string inCode, string inName, string inAppUser, string inLoginName, string inOldCode, string inOldName)
        {
            KeepLogAgent.pmSaveKeepLog(inSQLHelper, inType, inTaskName, inCode, inName, inAppUser, inLoginName, inOldCode, inOldName);
        }

        private static void pmSaveKeepLog(WS.Data.Agents.cDBMSAgent inSQLHelper, Entity.KeepLogType inType, string inTaskName, string inCode, string inName, string inAppUser, string inLoginName, string inOldCode, string inOldName)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            objSQLHelper.SetPara(new object[] { inTaskName });
            if (objSQLHelper.SQLExec(ref KeepLogAgent.dtsDataEnv, "QAppObj", MapTable.Table.AppObj, "select * from " + MapTable.Table.AppObj + " where cTaskName = ?", ref strErrorMsg))
            {

                DataRow dtrAppObj = KeepLogAgent.dtsDataEnv.Tables["QAppObj"].Rows[0];
                string strMenuName = dtrAppObj["cText"].ToString();
                string strUpdType = "";
                switch (inType)
                {
                    case Entity.KeepLogType.Insert:
                        strUpdType = BusinessEnum.gc_KEEPLOG_TYPE_INSERT;
                        break;
                    case Entity.KeepLogType.Update:
                        strUpdType = BusinessEnum.gc_KEEPLOG_TYPE_UPDATE;
                        break;
                    case Entity.KeepLogType.Delete:
                        strUpdType = BusinessEnum.gc_KEEPLOG_TYPE_DELETE;
                        break;
                    case Entity.KeepLogType.LogIn:
                        strUpdType = BusinessEnum.gc_KEEPLOG_TYPE_LOG_IN;
                        break;
                    case Entity.KeepLogType.LogOut:
                        strUpdType = BusinessEnum.gc_KEEPLOG_TYPE_LOG_OUT;
                        break;
                }

                DataRow dtrSaveInfo = null;
                objSQLHelper.SQLExec(ref KeepLogAgent.dtsDataEnv, mstrRefTable, mstrRefTable, "select * from " + mstrRefTable + " where 0=1", ref strErrorMsg);

                dtrSaveInfo = KeepLogAgent.dtsDataEnv.Tables[mstrRefTable].NewRow();
                WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();

                dtrSaveInfo["cRowID"] = objConn.RunRowID(mstrRefTable);
                dtrSaveInfo["cType"] = strUpdType;
                dtrSaveInfo["cTaskName"] = inTaskName;
                dtrSaveInfo["cMenuName"] = strMenuName.TrimEnd();
                dtrSaveInfo["cCode"] = inCode;
                dtrSaveInfo["cName"] = inName;
                dtrSaveInfo["cOldCode"] = inOldCode;
                dtrSaveInfo["cOldName"] = inOldName;
                dtrSaveInfo["cLoginName"] = inLoginName;
                dtrSaveInfo["cCreateBy"] = inAppUser;

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", true, ref strSQLUpdateStr, ref pAPara);

                objSQLHelper.SetPara(pAPara);
                objSQLHelper.SQLExec(strSQLUpdateStr, ref strErrorMsg);

            }
        }

    }


}
