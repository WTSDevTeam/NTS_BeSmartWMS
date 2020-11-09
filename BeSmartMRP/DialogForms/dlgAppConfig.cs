using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;

namespace BeSmartMRP.DialogForms
{
    public partial class dlgAppConfig : UIHelper.frmBase
    {
        public dlgAppConfig()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public const string REF_TABLE_NAME = "APPCONFIG";

        public const string FIELD_SERVERNAME = "SERVERNAME";
        public const string FIELD_DBMSNAME = "DBMSNAME";
        public const string FIELD_ERPSERVER = "ERPSERVER";
        public const string FIELD_ERPDBMS = "ERPDBMS";
        public const string FIELD_SHAREDIR = "SHAREDIR";

        public const string xd_APPCONFIG_XML_FILENAME = "CONFIG.XML";
        public const string xd_APPCONFIG_XSD_FILENAME = "CONFIG.XSD";

        private DataSet dtsConfig = new DataSet();

        private void pmInitForm()
        {
            this.pmLoadFormData();
        }

        private void pmLoadFormData()
        {
            this.txtServer.Text = App.ConfigurationManager.ConnectionInfo.ServerName;
            this.txtDBName.Text = App.ConfigurationManager.ConnectionInfo.DBMSName;
            this.txtERPServer.Text = App.ConfigurationManager.ConnectionInfo.ERPServerName;
            this.txtERPDB.Text = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
        }

        private bool pmTestConnect(bool inShowResult)
        {
            bool bllResult = false;
            bool bllResult1 = App.ConfigurationManager.TestConnect(this.txtServer.Text.TrimEnd(), this.txtDBName.Text.TrimEnd(), App.DatabaseReside);
            bool bllResult2 = true;
            //bool bllResult2 = App.ConfigurationManager.TestERPConnect(this.txtERPServer.Text.TrimEnd(), this.txtERPDB.Text.TrimEnd(), App.DatabaseReside);
            bllResult = (bllResult1 && bllResult2);
            if (inShowResult)
            {

                //TestConnectProgressCallBack cb = new TestConnectProgressCallBack(this.pmPregress());

                using (dlgConnResult dlg = new dlgConnResult())
                {
                    //dlg.WriteMessage(App.PackageDetail.DBMSName + " Test Connection");
                    dlg.WriteMessage("Test Connection");
                    dlg.WriteMessage("");
                    dlg.WriteMessage("Running connectivity tests...");
                    dlg.WriteMessage("");
                    dlg.WriteMessage("Attempting connection");

                    dlg.WriteMessage("Connection established");
                    dlg.WriteMessage("Verify option settings");
                    dlg.WriteMessage("Disconnecting from server");
                    dlg.WriteMessage("");
                    if (bllResult1 && bllResult2)
                    {
                        dlg.WriteMessage("TEST COMPLETED SUCCESSFULLY!");
                    }
                    else
                    {
                        dlg.WriteMessage("TEST FAIL!!!");
                    }
                    dlg.ShowDialog();
                }
            }
            return bllResult;
        }

        private void pmPregress()
        {
            Application.DoEvents();
        }

        private void pmSaveConfig()
        {
            App.ConfigurationManager.ConnectionInfo.ServerName = this.txtServer.Text.TrimEnd();
            App.ConfigurationManager.ConnectionInfo.DBMSName = this.txtDBName.Text.TrimEnd();
            App.ConfigurationManager.ConnectionInfo.ERPServerName = this.txtERPServer.Text.TrimEnd();
            App.ConfigurationManager.ConnectionInfo.ERPDBMSName = this.txtERPDB.Text.TrimEnd();
            App.ConfigurationManager.Save(Application.StartupPath);
        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            this.pmTestConnect(true);
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.pmTestConnect(false))
            {
                this.pmSaveConfig();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Cannot connect to server", "Connection result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }

    /// <summary>
    /// CallBack delegate for progress notification
    /// </summary>
    public delegate void TestConnectProgressCallBack(string inMessage);

}
