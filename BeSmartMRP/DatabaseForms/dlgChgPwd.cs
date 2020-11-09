
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;
using AppUtil.SecureHelper;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.DatabaseForms
{
    public partial class dlgChgPwd : UIHelper.frmBase
    {

        public static string TASKNAME = "CHGPWD";

        private DataSet dtsDataEnv = new DataSet();

        public dlgChgPwd()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.pmValidForm())
            {
                this.pmUpdateRecord();
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool pmValidForm()
        {
            bool bllResult = false;
            int intOSeed = -1;
            string strAppLogin = "";
            string strLoginErrorMsg = "";

            string strErrorMsg = "";
            if (this.txtPwd1.Text.Trim() != this.txtPwd2.Text.Trim())
            {
                MessageBox.Show(this, "รหัสผ่านไม่ถูกต้อง", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPwd1.Text = "";
                this.txtPwd2.Text = "";
                this.txtPwd1.Focus();
                return false;
            }
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (App.PermissionManager.CheckLogin(objSQLHelper, App.AppUserName, this.txtCurrPwd.Text, ref strAppLogin, ref strLoginErrorMsg, ref intOSeed))
            {
                bllResult = true;
            }
            else
            {
                MessageBox.Show(this, "รหัสผ่านเดิมไม่ถูกต้อง !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bllResult = false;
            }
            return bllResult;
        }

        private void pmUpdateRecord()
        {
            string strErrorMsg = "";
            string strPwd = "";

            App.PermissionManager.CreateCipherText(this.txtPwd1.Text.TrimEnd(), ref strPwd);
            
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { strPwd , App.FMAppUserID });
            objSQLHelper.SQLExec("update AppLogin set cPwd = ? where cRowID = ? ", ref strErrorMsg);
        }

    }
}
