
using System;
using System.Data;
using WS.Data;

namespace BeSmartMRP.Business.Agents
{

    public class ConfigurationAgent
    {

        private const string xd_CONFIG_XML_FILENAME = "CONFIG.XML";
        private const string xd_CONFIG_XSD_FILENAME = "CONFIG.XSD";

        public const string REF_TABLE_NAME = "APPCONFIG";

        public const string FIELD_SERVERNAME = "CSERVERNAME";
        public const string FIELD_DBMSNAME = "CDBMSNAME";
        public const string FIELD_ERPSERVERNAME = "CERPSERVER";
        public const string FIELD_ERPDBMSNAME = "CERPDBMS";
        public const string FIELD_COMMONPATH = "CCOMMONPATH";

        private BeSmartMRP.Business.Entity.Info.ConnectionInfo mConnInfo = null;
        private string mstrCommonPath = "";

        private DataSet dtsConfig = new DataSet();

        public ConfigurationAgent()
        {
            this.mConnInfo = new BeSmartMRP.Business.Entity.Info.ConnectionInfo();
            this.pmLoadConfigSchemaTab();
        }

        public BeSmartMRP.Business.Entity.Info.ConnectionInfo ConnectionInfo
        {
            get { return this.mConnInfo; }
        }

        public string CommonPath
        {
            get { return this.mstrCommonPath; }
            set { this.mstrCommonPath = value; }
        }

        public bool TestConnect(string inServerName, string inDBName, WS.Data.DBMSType inDBReside)
        {
            //return true;
            string strConnStr = "PROVIDER=SQLOLEDB; Data Source=" + inServerName + "; Initial Catalog=" + inDBName + ";User ID=fm1234;Password=x2y2;";
            return this.pmTestConnect(strConnStr, inDBReside, "EMCORP");
        }

        public bool TestERPConnect(string inServerName, string inDBName, WS.Data.DBMSType inDBReside)
        {
            string strConnStr = "PROVIDER=SQLOLEDB; Data Source=" + inServerName + "; Initial Catalog=" + inDBName + ";User ID=fm1234;Password=x2y2;";
            return this.pmTestConnect(strConnStr, inDBReside, "CORP");
        }

        private bool pmTestConnect(string inConnectStr, WS.Data.DBMSType inDBReside, string inTestTab)
        {
            string strErrorMsg = string.Empty;
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(inConnectStr, inDBReside);
            objSQLHelper.SQLExec("select * from " + inTestTab + " where 0=1", ref strErrorMsg);
            return (strErrorMsg == string.Empty);
        }

        public void Load(string inExecutePath)
        {
            if (System.IO.File.Exists(inExecutePath + "\\" + xd_CONFIG_XML_FILENAME) == false)
            {
                this.pmLoadDefaultAppConfig(inExecutePath);
            }
            else
            {
                this.pmLoadAppConfig(inExecutePath);
            }
        }

        public void Save(string inExecutePath)
        {
            this.pmSaveAppConifg(inExecutePath);
        }

        private void pmLoadConfigSchemaTab()
        {
            DataTable dtbTemPd = new DataTable(REF_TABLE_NAME);

            dtbTemPd.Columns.Add(FIELD_SERVERNAME, Type.GetType("System.String"));
            dtbTemPd.Columns.Add(FIELD_DBMSNAME, Type.GetType("System.String"));
            dtbTemPd.Columns.Add(FIELD_ERPSERVERNAME, Type.GetType("System.String"));
            dtbTemPd.Columns.Add(FIELD_ERPDBMSNAME, Type.GetType("System.String"));
            dtbTemPd.Columns.Add(FIELD_COMMONPATH, Type.GetType("System.String"));

            this.dtsConfig.Tables.Add(dtbTemPd);
            DataRow dtrNewRow = this.dtsConfig.Tables[REF_TABLE_NAME].NewRow();
            this.dtsConfig.Tables[REF_TABLE_NAME].Rows.Add(dtrNewRow);
        }

        private void pmLoadDefaultAppConfig(string inExecutePath)
        {
            this.mConnInfo.ServerName = "(local)";
            this.mConnInfo.DBMSName = "DBSMRP";
            this.mConnInfo.ERPServerName = "(local)";
            this.mConnInfo.ERPDBMSName = "formula";
            this.mstrCommonPath = inExecutePath + "\\Common\\";
        }

        private void pmLoadAppConfig(string inExecutePath)
        {
            try
            {
                if (this.dtsConfig.Tables[REF_TABLE_NAME].Rows.Count > 0)
                {
                    this.dtsConfig.Tables[REF_TABLE_NAME].Rows.Clear();
                }
                this.dtsConfig.ReadXmlSchema(inExecutePath + "\\" + xd_CONFIG_XSD_FILENAME);
                this.dtsConfig.ReadXml(inExecutePath + "\\" + xd_CONFIG_XML_FILENAME);
                DataRow dtrConfig = this.dtsConfig.Tables[REF_TABLE_NAME].Rows[0];

                this.mConnInfo.ServerName = dtrConfig[FIELD_SERVERNAME].ToString();
                this.mConnInfo.DBMSName = dtrConfig[FIELD_DBMSNAME].ToString();
                this.mConnInfo.ERPServerName = dtrConfig[FIELD_ERPSERVERNAME].ToString();
                this.mConnInfo.ERPDBMSName = dtrConfig[FIELD_ERPDBMSNAME].ToString();
                this.mstrCommonPath = dtrConfig[FIELD_COMMONPATH].ToString();
            }
            //catch (Exception ex)
            catch
            {
                this.pmLoadDefaultAppConfig(inExecutePath);
            }
        }

        private void pmSaveAppConifg(string inExecutePath)
        {
            DataRow dtrConfig = this.dtsConfig.Tables[REF_TABLE_NAME].Rows[0];
            this.pmUpdateConfigRow(ref dtrConfig);

            this.dtsConfig.WriteXmlSchema(inExecutePath + "\\" + xd_CONFIG_XSD_FILENAME);
            this.dtsConfig.WriteXml(inExecutePath + "\\" + xd_CONFIG_XML_FILENAME);
        }

        private void pmUpdateConfigRow(ref DataRow inAppConfig)
        {
            inAppConfig[FIELD_SERVERNAME] = this.mConnInfo.ServerName;
            inAppConfig[FIELD_DBMSNAME] = this.mConnInfo.DBMSName;
            inAppConfig[FIELD_ERPSERVERNAME] = this.mConnInfo.ERPServerName;
            inAppConfig[FIELD_ERPDBMSNAME] = this.mConnInfo.ERPDBMSName;
            inAppConfig[FIELD_COMMONPATH] = this.mstrCommonPath;
        }

    }


}
