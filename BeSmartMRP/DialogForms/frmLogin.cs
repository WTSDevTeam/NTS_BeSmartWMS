using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.Business.Entity;
using AppUtil.SecureHelper;
using System.Diagnostics;

namespace BeSmartMRP.DialogForms
{
    public partial class frmLogin : UIHelper.frmBase
    {


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public frmLogin()
        {
            InitializeComponent();
            this.pmInitForm();
            //this.Size = new Size(950, 650);
        }

        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {
            switch (App.AppModule)
            {
                case "POS1_SYS":
                    this.lblTitle.Text = "WeBusiness Point of Sale";
                    break;
                default:
                    //this.lblTitle.Text = "WeBusiness ERP for Enterprise";
                    this.lblTitle.Text = "ระบุ Username/Password เพื่อเข้าสู่ระบบ";
                    break;
            }

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.lblVersion.Text = $"version { versionInfo.FileVersion }";

        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (this.pmAppChkLogin())
            {
                this.DialogResult = DialogResult.OK;
                //App.ofrmMainMenu.statusStrip1.Items[2].Text = "LOGIN : " + App.AppUserName.TrimEnd();
                //App.ofrmMainMenu.statusStrip1.Items[2].Text = "LOGIN : " + App.AppUserName.TrimEnd();
            }
        }

        private bool pmAppChkLogin()
        {
            bool bllResult = false;
            int intOSeed = -1;
            string strAppLogin = "";
            string strLoginErrorMsg = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            if (App.PermissionManager.CheckLogin(pobjSQLUtil, this.txtUserName.Text, this.txtPwd.Text, ref strAppLogin, ref strLoginErrorMsg, ref intOSeed))
            {
                pobjSQLUtil.SetPara(new object[] { strAppLogin });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from AppLogin where cRowID = ? ", ref strErrorMsg))
                {
                    bllResult = true;
                    DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                    App.FMAppUserID = dtrEmpl["cRowID"].ToString();
                    App.AppUserID = dtrEmpl["cRowID"].ToString();
                    App.AppUserName = dtrEmpl["cLogin"].ToString();

                    App.AppActiveDate = pobjSQLUtil.GetDBServerDateTime();
                    if (App.PermissionManager.CheckPermission_Add(SysDef.gc_AUTH_RIG_CHGTRANDATE, AuthenType.IsAllow, App.AppUserName, App.AppUserID))
                    {
                        App.AppAllowChangeDate = true;
                    }
                    else
                    {
                        App.AppAllowChangeDate = false;
                    }
                
                }
            }
            else
            {
                MessageBox.Show(this, strLoginErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPwd.Text = "";
                this.txtPwd.Focus();
            }
            return bllResult;
        }

        private bool pmChkLogin()
        {

            bool bllResult = false;
            int intOSeed = -1;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.txtUserName.Text.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from EMPLR where FCLOGIN = ? ", ref strErrorMsg))
            {
                foreach (DataRow dtrEmpl in this.dtsDataEnv.Tables["QEmplR"].Rows)
                {
                    string strHPwd = dtrEmpl["fcHPw"].ToString();
                    string strPwd = FMEncryptor.pmethDecryptStr(FMEncryptor.gn_INCLUDE_ENCRYPT_STR_TYPE, strHPwd, 0, ref intOSeed);
                    if (this.txtPwd.Text.TrimEnd() == strPwd.TrimEnd())
                    {
                        bllResult = true;
                        App.FMAppUserID = dtrEmpl["fcSkid"].ToString();
                        App.AppUserName = dtrEmpl["fcLogin"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(this, "Invalid password !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                        this.txtPwd.Text = "";
                        this.txtPwd.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Not found User name !");
            }
            return bllResult;
        }

        private bool pmChkLogin2()
        {

            bool bllResult = false;
            int intOSeed = -1;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.txtUserName.Text.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from AppLogin where CLOGIN = ? ", ref strErrorMsg))
            {
                foreach (DataRow dtrEmpl in this.dtsDataEnv.Tables["QEmplR"].Rows)
                {
                    string strHPwd = dtrEmpl["cPwd"].ToString();
                    bool bllIsOK = App.PermissionManager.CompareCipherText(pobjSQLUtil, this.txtPwd.Text.ToUpper().TrimEnd(), dtrEmpl["cRowID"].ToString());
                    if (bllIsOK)
                    {
                        bllResult = true;
                        App.FMAppUserID = dtrEmpl["cRowID"].ToString();
                        App.AppUserName = dtrEmpl["cLogin"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(this, "Invalid password !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txtPwd.Text = "";
                        this.txtPwd.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Not found User name !");
            }
            return bllResult;
        }

        private void btnSetting_Click(object sender, System.EventArgs e)
        {
            App.SetAppConfig();
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {

            Control oSender = this.ActiveControl;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (oSender is System.Windows.Forms.TextBox)
                    {
                        System.Windows.Forms.TextBox oActive = (System.Windows.Forms.TextBox)oSender;
                        if (oActive.Multiline == false)
                            System.Windows.Forms.SendKeys.Send("{TAB}");
                    }
                    break;
                default:
                    //UIHelper.KeyControl.Send(this.ActiveControl, e);
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            frmMainmenu2 ofrm = new frmMainmenu2();
            ofrm.Show();

            //MessageBox.Show("124    ".Trim()+"y");

            //string strSQLStr = "select GLREF.*";
            //strSQLStr += ",COOR.FCCODE as QCCOOR ";
            //strSQLStr += ",PROD.FCCODE as QCPROD ";
            //strSQLStr += ",UM.FCCODE,UM.FCNAME ";
            //strSQLStr += ",REFPROD.FNQTY,REFPROD.FNPRICE ";
            //strSQLStr += "from REFPROD ";
            //strSQLStr += "left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            //strSQLStr += "left join COOR on COOR.FCSKID = GLREF.FCCOOR ";
            //strSQLStr += "left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            //strSQLStr += "left join UM on UM.FCSKID = REFPROD.FCUM ";
            //strSQLStr += "where GLREF.FCREFTYPE = 'SI' and GLREF.FDDATE between '6-1-2010' and '12-31-2010'";

            //DataSet ds = new DataSet("INVOICE");
            //string strErrorMsg = "";
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //if (pobjSQLUtil.SQLExec(ref ds, "QInvoice", "GLREF", strSQLStr, ref strErrorMsg))
            //{
            //    ds.WriteXmlSchema("C:\\temp\\QINV.XSD");
            //    ds.WriteXml("C:\\temp\\QINV.XML");
            //}


            //string strSQLStr = "select PROD.*";
            //strSQLStr += ",UM.FCCODE,UM.FCNAME ";
            //strSQLStr += ",PDGRP.FCCODE as QCPDGRP,PDGRP.FCNAME as QnPdGrp";
            //strSQLStr += " from PROD ";
            //strSQLStr += " left join UM on UM.FCSKID = PROD.FCUM ";
            //strSQLStr += " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            ////strSQLStr += "where GLREF.FCREFTYPE = 'SI' and GLREF.FDDATE between '6-1-2010' and '12-31-2010'";

            //DataSet ds = new DataSet("INVOICE");
            //string strErrorMsg = "";
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //if (pobjSQLUtil.SQLExec(ref ds, "QInvoice", "GLREF", strSQLStr, ref strErrorMsg))
            //{
            //    ds.WriteXmlSchema("C:\\temp\\QPROD.XSD");
            //    ds.WriteXml("C:\\temp\\QPROD.XML");
            //}

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
