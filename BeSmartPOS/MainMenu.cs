//#define xd_VERSION_DEMO

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;

using WS.Data;
using WS.Data.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.DialogForms;
using System.Threading;

namespace BeSmartPOS
{

    public class BeSmartPOS
    {

        public const string xd_EVENTS_FILENAME = "Event.Log";

        public static string AppVersion = "16/08/07";
        public static string AppModule = "MIS_SYS";

        public static string AppID = "$%";

        public static string gcCorp = "$%";
        public static string gcCorpName = "";

        public static string AppUserRoleList = "";
        public static string AppUserID = "";
        public static string AppUserName = "";
        public static string FMAppUserID = "";

        public static bool MoreProcess = true;
        public static string AppMessage = "";
        public static string LastAppMessage = "";

        public static string xd_Access_ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + "\\" + "AppResource.mdb; Jet OLEDB:Database Password=x2y2";

        public static BeSmartMRP.Business.Agents.SecurityAgent PermissionManager = null;

        public static BeSmartMRP.Business.Agents.ConfigurationAgent ConfigurationManager = null;

        private static string mstrConnectionString = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula_ta;User ID=fm1234;Password=x2y2;";
        private static string mstrERPConnectionString = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula_ta;User ID=fm1234;Password=x2y2;";

        public static BeSmartMRP.frmMainMenuPOS1 ofrmMainMenu;
        public static BeSmartMRP.frmReportPreview ofrmReportPreview = null;

        private static DBMSType pnDatabaseReside = DBMSType.MSSQLServer;

        private static cConn oConnDB = new cConn();
        public static string mRunRowID(string inTableName)
        {
            return oConnDB.RunRowID(inTableName);
        }

        public static string ConnectionString
        {
            get { return pmLoadAppConnStr(); }
        }

        public static string ERPConnectionString
        {
            get { return pmLoadERPAppConnStr(); }
        }

        public static DBMSType DatabaseReside
        {
            get { return pnDatabaseReside; }
        }

        private static string pmLoadAppConnStr()
        {
            string[] strPara = new string[] { BeSmartPOS.ConfigurationManager.ConnectionInfo.ServerName, BeSmartPOS.ConfigurationManager.ConnectionInfo.DBMSName };
            BeSmartPOS.mstrConnectionString = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return BeSmartPOS.mstrConnectionString;
        }

        private static string pmLoadERPAppConnStr()
        {
            string[] strPara = new string[] { BeSmartPOS.ConfigurationManager.ConnectionInfo.ERPServerName, BeSmartPOS.ConfigurationManager.ConnectionInfo.ERPDBMSName };
            BeSmartPOS.mstrERPConnectionString = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return BeSmartPOS.mstrERPConnectionString;
        }

        private void pmLoadConfig()
        {
            if (System.IO.File.Exists(Application.StartupPath + @"\CONFIG.XML") == false)
            {
                BeSmartPOS.SetAppConfig();
            }
            BeSmartPOS.ConfigurationManager.Load(Application.StartupPath);
            if (BeSmartPOS.PermissionManager != null)
            {
                //BeSmartPOS.PermissionManager.ConnectionString2 = BeSmartPOS.xd_Access_ConnectionString;
                //BeSmartPOS.PermissionManager.ConnectionString = BeSmartPOS.ERPConnectionString;
                //BeSmartPOS.PermissionManager.DataBaseReside = BeSmartPOS.DatabaseReside;
            }
        }

        #region "SetActive Corp"
        public static void SetActiveCorp(DataRow inCorp)
        {
            BeSmartPOS.gcCorp = inCorp["fcSkid"].ToString();
            BeSmartPOS.gcCorpName = inCorp["fcName"].ToString().TrimEnd();
            //System.Data.DataSet dtsCorp = new System.Data.DataSet();
            //string strErrorMsg = "";
            //cDBMSAgent objSQLHelper = new cDBMSAgent(BeSmartPOS.ERPConnectionString, BeSmartPOS.DatabaseReside);

            //ActiveCorp.RowID = inCorp["fcSkid"].ToString();
            //ActiveCorp.Code = inCorp["fcCode"].ToString();
            //ActiveCorp.Name = inCorp["fcName"].ToString().TrimEnd();
            //ActiveCorp.StartAppDate = Convert.ToDateTime(inCorp["fdCalDate"]);
            //ActiveCorp.Address1 = inCorp["fcAddr1"].ToString().TrimEnd();
            //ActiveCorp.Address2 = inCorp["fcAddr2"].ToString().TrimEnd();
            //ActiveCorp.TelNo = inCorp["fcTel"].ToString().TrimEnd();
            //ActiveCorp.FaxNo = inCorp["fcFax"].ToString().TrimEnd();
            //ActiveCorp.TaxID = inCorp["fcTaxID"].ToString().TrimEnd();
            //ActiveCorp.SaleVATIsOut = (inCorp["fcVatIsOut"].ToString() != string.Empty ? inCorp["fcVatIsOut"].ToString() : "Y");
            //ActiveCorp.SaleVATType = (inCorp["fcVatType"].ToString() != string.Empty ? inCorp["fcVatType"].ToString() : "1");
            //ActiveCorp.SCtrlStock = (inCorp["fcCtrlStoc"].ToString().TrimEnd() != string.Empty ? inCorp["fcCtrlStoc"].ToString().TrimEnd() : "1");

            //objSQLHelper.SetPara(new object[1] { inCorp["fcSect"].ToString() });
            //if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Sect", "Sect", "select * from Sect where fcSkid = ? ", ref strErrorMsg))
            //{
            //    BeSmartPOS.ActiveCorp.DefaultSectID = dtsCorp.Tables["QVFLD_Sect"].Rows[0]["fcSkid"].ToString();
            //    objSQLHelper.SetPara(new object[1] { dtsCorp.Tables["QVFLD_Sect"].Rows[0]["fcDept"].ToString() });
            //    if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Dept", "Dept", "select * from Dept where fcSkid = ?", ref strErrorMsg))
            //        BeSmartPOS.ActiveCorp.DefaultDeptID = dtsCorp.Tables["QVFLD_Dept"].Rows[0]["fcSkid"].ToString();
            //}
            //objSQLHelper.SetPara(new object[1] { inCorp["fcProj"].ToString() });
            //if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Proj", "Proj", "select * from Proj where fcSkid = ? ", ref strErrorMsg))
            //{
            //    BeSmartPOS.ActiveCorp.DefaultProjectID = dtsCorp.Tables["QVFLD_Proj"].Rows[0]["fcSkid"].ToString();
            //}

            //objSQLHelper.SetPara(new object[1] { inCorp["fcJob"].ToString() });
            //if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Job", "Job", "select * from Job where fcSkid = ? ", ref strErrorMsg))
            //{
            //    BeSmartPOS.ActiveCorp.DefaultJobID = dtsCorp.Tables["QVFLD_Job"].Rows[0]["fcSkid"].ToString();
            //}

            //BeSmartPOS.ActiveCorp.MMQtyFormatString = "#,###,###.0000";
            //BeSmartPOS.ActiveCorp.PriceFormatString = "#,###,###.0000";
        }
        #endregion

        public static void SetAppConfig()
        {
            using (BeSmartMRP.DialogForms.dlgAppConfig dlg = new BeSmartMRP.DialogForms.dlgAppConfig())
            {
                dlg.ShowDialog();
            }
        }

        public static void GetLogin(bool inIsStartUp)
        {
            using (frmLogin dlg = new frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (inIsStartUp)
                        Application.Run(BeSmartPOS.ofrmMainMenu);
                    //else
                    //BeSmartPOS.ofrmMainMenu.MainStatusBar.Panels["USERNAME"].Text = "User : " + BeSmartPOS.AppUserName;
                }
                else
                {
                    if (inIsStartUp)
                        Application.Exit();
                }
            }
        }

        #region "Exception Manager"
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif

            string strFileName = Application.StartupPath + "\\" + xd_EVENTS_FILENAME;
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Exception Message :");
            Trace.WriteLine(e.Exception.Message.ToString());
            Trace.WriteLine("Exception StackTrace :");
            Trace.WriteLine(e.Exception.StackTrace.ToString());
            Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            Trace.WriteLine("User Name : " + System.Environment.UserName);
            Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
        }

        public static void WriteEventsLog(Exception ex)
        {

#if xd_RUNMODE_DEBUG
			MessageBox.Show(ex.TargetSite.ToString() + "\n" + ex.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif

            string strFileName = Application.StartupPath + "\\" + xd_EVENTS_FILENAME;
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Exception Message :");
            Trace.WriteLine(ex.Message.ToString());
            Trace.WriteLine("Exception StackTrace :");
            Trace.WriteLine(ex.StackTrace.ToString());
            Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            Trace.WriteLine("User Name : " + System.Environment.UserName);
            Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
        }

        #endregion

        #region "Start Application"


        public BeSmartPOS()
        {
            //BeSmartPOS.ofrmMainMenu = new BeSmartMRP.frmMainmenu();
        }

        private DateTime XD_EXPIREDATE = new DateTime(2008, 01, 31);
        public static BeSmartMRP.App oBeSmartMRP = new BeSmartMRP.App("POS");

        public void StartApplication()
        {

            oBeSmartMRP.StartConfig();

            BeSmartPOS.ConfigurationManager = new BeSmartMRP.Business.Agents.ConfigurationAgent();
#if !xd_RUNMODE_DEBUG
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
#endif

#if !xd_RUNMODE_DEBUG
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
#endif

            this.pmLoadConfig();

            BeSmartPOS.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent();
            //BeSmartPOS.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent(BeSmartPOS.ConnectionString, BeSmartPOS.DatabaseReside);
            //BeSmartPOS.PermissionManager.ModuleID = BeSmartPOS.AppID;
            //BeSmartPOS.PermissionManager.ConnectionString2 = BeSmartPOS.xd_Access_ConnectionString;

            //            BeSmartPOS.ofrmMainMenu = new BeSmartMRP.frmMainmenu();

            //#if xd_VERSION_DEMO
            //            if (XD_EXPIREDATE.CompareTo(DateTime.Now) < 0)
            //            {
            //                MessageBox.Show("Database is Access Deny", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                Application.Exit();
            //            }
            //            else
            //            {
            //                using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
            //                {
            //                    if (dlg.ShowDialog() == DialogResult.OK)
            //                    {
            //                        Application.Run(BeSmartPOS.ofrmMainMenu);
            //                        //Application.Run(new Form1());
            //                    }
            //                    else
            //                    {
            //                        Application.Exit();
            //                    }
            //                }
            //            }
            //#else
            //            using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
            //            {
            //                if (dlg.ShowDialog() == DialogResult.OK)
            //                {
            //                    Application.Run(BeSmartPOS.ofrmMainMenu);
            //                }
            //                else
            //                {
            //                    Application.Exit();
            //                }
            //            }
            //#endif
        }

        #endregion

        private void Application_ApplicationExit(object sender, EventArgs e)
        {

            //if (ofrmReportPreview != null)
            //{
            //    ofrmReportPreview.Dispose();
            //    ofrmReportPreview = null;
            //}

            if (oConnDB != null)
            {
                oConnDB = null;
            }

            if (PermissionManager != null)
            {
                PermissionManager = null;
            }
            //if (ActiveCorp != null)
            //{
            //    ActiveCorp = null;
            //}
        }



    }


    static class MainMenu
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //MessageBox.Show("Hello");
            //for Vista or Window7
            if (System.Environment.OSVersion.Version.Major > 5)
            {
                DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint(); //แก้ปัญหาเรื่องภาษาไทยที่ Toolbar ของ Window Vista
            }

            //DevExpress.Utils.WXPaint.WXPPainter.Default.UseWindowsXPThemeColors = 0;
            //DevExpress.Utils.WXPaint.WXPPainter.Default.UseWindowsXPThemeColors = 2;

            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMainmenu());
            BeSmartPOS app = new BeSmartPOS();
            app.StartApplication();
        }
    }

}
