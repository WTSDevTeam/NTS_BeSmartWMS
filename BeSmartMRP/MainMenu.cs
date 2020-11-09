//#define xd_VERSION_DEMO

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Globalization;

using WS.Data;
using WS.Data.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.DialogForms;
using System.Threading;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Component;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP
{

    public class App
    {

        public const string xd_EVENTS_FILENAME = "Event.Log";

        //public static string AppVersion = "11/03/2010";
        //public static string AppDBVersion = "11/03/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 3, 11);

        //public static string AppVersion = "17/03/2010";
        //public static string AppDBVersion = "17/03/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 3, 17);

        //public static string AppVersion = "22/03/2010";
        //public static string AppDBVersion = "22/03/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 3, 22);

        //public static string AppVersion = "24/03/2010";
        //public static string AppDBVersion = "24/03/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 3, 24);

        //public static string AppVersion = "02/04/2010";
        //public static string AppDBVersion = "02/04/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 4, 2);

        //public static string AppVersion = "10/04/2010";
        //public static string AppDBVersion = "10/04/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 4, 10);

        //public static string AppVersion = "19/04/2010";
        //public static string AppDBVersion = "19/04/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 4, 19);

        //public static string AppVersion = "24/04/2010";
        //public static string AppDBVersion = "24/04/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 4, 24);

        //public static string AppVersion = "06/05/2010";
        //public static string AppDBVersion = "06/05/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 5, 6);

        //public static string AppVersion = "11/05/2010";
        //public static string AppDBVersion = "11/05/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 5, 11);

        //public static string AppVersion = "25/05/2010";
        //public static string AppDBVersion = "25/05/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 5, 25);

        //public static string AppVersion = "01/06/2010";
        //public static string AppDBVersion = "01/06/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 6, 1);

        //public static string AppVersion = "23/06/2010";
        //public static string AppDBVersion = "23/06/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 6, 23);

        //public static string AppVersion = "04/08/2010";
        //public static string AppDBVersion = "04/08/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 8, 4);

        //public static string AppVersion = "16/08/2010";
        //public static string AppDBVersion = "16/08/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 8, 16);

        //public static string AppVersion = "01/09/2010";
        //public static string AppDBVersion = "01/09/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 9, 1);

        //public static string AppVersion = "28/09/2010";
        //public static string AppDBVersion = "28/09/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 9, 28);

        //public static string AppVersion = "08/10/2010";
        //public static string AppDBVersion = "08/10/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 10, 8);

        //public static string AppVersion = "15/10/2010";
        //public static string AppDBVersion = "15/10/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 10, 15);

        //public static string AppVersion = "19/10/2010";
        //public static string AppDBVersion = "19/10/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 10, 19);

        //public static string AppVersion = "05/11/2010";
        //public static string AppDBVersion = "05/11/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 11, 05);

        //public static string AppVersion = "26/11/2010";
        //public static string AppDBVersion = "26/11/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 11, 26);

        //public static string AppVersion = "14/12/2010";
        //public static string AppDBVersion = "14/12/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 12, 14);

        //public static string AppVersion = "16/12/2010";
        //public static string AppDBVersion = "16/12/2010";
        //public static DateTime dAppVersion = new DateTime(2010, 12, 16);

        //public static string AppVersion = "26/01/2011";
        //public static string AppDBVersion = "26/01/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 1, 26);

        //public static string AppVersion = "16/03/2011";
        //public static string AppDBVersion = "16/03/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 3, 16);

        //public static string AppVersion = "04/04/2011";
        //public static string AppDBVersion = "04/04/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 4, 4);

        //public static string AppVersion = "20/04/2011";
        //public static string AppDBVersion = "20/04/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 4, 20);

        //public static string AppVersion = "12/05/2011";
        //public static string AppDBVersion = "12/05/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 4, 20);

        //public static string AppVersion = "14/05/2011";
        //public static string AppDBVersion = "14/05/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 5, 14);

        //public static string AppVersion = "06/07/2011";
        //public static string AppDBVersion = "06/07/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 7, 6);

        //public static string AppVersion = "23/08/2011";
        //public static string AppDBVersion = "23/08/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 8, 23);

        //public static string AppVersion = "06/09/2011";
        //public static string AppDBVersion = "06/09/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 9, 6);

        //public static string AppVersion = "03/10/2011";
        //public static string AppDBVersion = "03/10/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 10, 3);

        //public static string AppVersion = "11/10/2011";
        //public static string AppDBVersion = "11/10/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 10, 11);
        
        //public static string AppVersion = "25/10/2011";
        //public static string AppDBVersion = "25/10/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 10, 25);

        //public static string AppVersion = "18/11/2011";
        //public static string AppDBVersion = "18/11/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 11, 18);


        //public static string AppVersion = "14/12/2011";
        //public static string AppDBVersion = "14/12/2011";
        //public static DateTime dAppVersion = new DateTime(2011, 12, 14);

        //public static string AppVersion = "21/6/2012";
        //public static string AppDBVersion = "21/6/2012";
        //public static DateTime dAppVersion = new DateTime(2012, 6, 21);

        //public static string AppVersion = "19/1/2013";
        //public static string AppDBVersion = "19/1/2013";
        //public static DateTime dAppVersion = new DateTime(2013, 1, 19);

        public static string AppVersion = "09/09/2020";
        public static string AppDBVersion = "09/09/2020";
        public static DateTime dAppVersion = new DateTime(2020, 9, 9);

        public static int mint_BufferTimeout = 2;
        
        public static string AppModule = "MRPI_SYS";

        public static string AppID = "M2";
        public static string FMAppID = "$%";

        public static string gcCorp = "$%";
        public static string gcCorpName = "";
        public static string gcCorpName2 = "";
        public static string gcCorpSaleVATType = "1";
        public static DateTime gdStartCorpDate = DateTime.MinValue;

        public static string Config_StockType = "0";
        public static string Config_CredLimit = "0";

        public static string AppUserRoleList = "";
        public static string AppUserID = "";
        public static string AppUserName = "";
        public static string FMAppUserID = "";

        public static bool AppAllowChangeDate = false;
        public static DateTime AppActiveDate = DateTime.Now;

        public static int xd_LEN_PROD_CODE = 20;
        public static int xd_LEN_PROD_NAME = 70;
        public static int xd_LEN_WHOUSE_CODE = 10;
        public static int xd_LEN_WHOUSE_NAME = 30;
        

        public static bool MoreProcess = true;
        public static string AppMessage = "";
        public static string LastAppMessage = "";

        public static AppLocale mLocale_UI = AppLocale.en_US;
        public static AppLocale mLocale_Report = AppLocale.en_US;

        public static string xd_Access_ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + "\\" + "AppResource.mdb; Jet OLEDB:Database Password=x2y2";

        public static BeSmartMRP.Business.Agents.SecurityAgent PermissionManager = null;

        public static BeSmartMRP.Business.Agents.ConfigurationAgent ConfigurationManager = null;

        private static string mstrConnectionString = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula_ta;User ID=fm1234;Password=x2y2;";
        private static string mstrERPConnectionString = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula_ta;User ID=fm1234;Password=x2y2;";

        //public static frmMainmenu ofrmMainMenu;
        public static DevExpress.XtraEditors.XtraForm ofrmMainMenu;
        public static frmReportPreview ofrmReportPreview = null;

        public static Business.Entity.CorpInfo ActiveCorp = new Business.Entity.CorpInfo();

        private static DBMSType pnDatabaseReside = DBMSType.MSSQLServer;

        [DllImportAttribute("User32.dll")]
        public static extern int FindWindow(String ClassName, String WindowName);

        //Import the SetForeground API to activate it
        [DllImportAttribute("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static IntPtr mSave_hWnd;

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

        public static string ERPConnectionString2
        {
            get { return pmLoadERPAppConnStr2(); }
        }

        public static string ConnectionString2
        {
            get { return pmLoadAppConnStr2(); }
        }

        public static DBMSType DatabaseReside
        {
            get { return pnDatabaseReside; }
        }

        public static void ActivateMainScreen()
        {
            App.SetForegroundWindow(App.ofrmMainMenu.Handle);
            if (App.ofrmMainMenu.WindowState == FormWindowState.Minimized)
            {
                App.ofrmMainMenu.WindowState = FormWindowState.Maximized;
            }
        }

        private static string pmLoadAppConnStr()
        {
            string[] strPara = new string[] { App.ConfigurationManager.ConnectionInfo.ServerName, App.ConfigurationManager.ConnectionInfo.DBMSName };
            App.mstrConnectionString = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return App.mstrConnectionString;
        }

        private static string pmLoadERPAppConnStr()
        {
            string[] strPara = new string[] { App.ConfigurationManager.ConnectionInfo.ERPServerName, App.ConfigurationManager.ConnectionInfo.ERPDBMSName };
            App.mstrERPConnectionString = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return App.mstrERPConnectionString;
        }

        private static string pmLoadERPAppConnStr2()
        {
            string[] strPara = new string[] { App.ConfigurationManager.ConnectionInfo.ERPServerName, App.ConfigurationManager.ConnectionInfo.ERPDBMSName };
            return String.Format("Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
        }

        private static string pmLoadAppConnStr2()
        {
            string[] strPara = new string[] { App.ConfigurationManager.ConnectionInfo.ServerName, App.ConfigurationManager.ConnectionInfo.DBMSName };
            return String.Format("Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
        }

        private void pmLoadConfig()
        {
            if (System.IO.File.Exists(Application.StartupPath + @"\CONFIG.XML") == false)
            {
                App.SetAppConfig();
            }
            App.ConfigurationManager.Load(Application.StartupPath);
            if (App.PermissionManager != null)
            {
                //App.PermissionManager.ConnectionString2 = App.xd_Access_ConnectionString;
                //App.PermissionManager.ConnectionString = App.ERPConnectionString;
                //App.PermissionManager.DataBaseReside = App.DatabaseReside;
            }
        }

        public static void AppDBChkUpdate()
        {
            if (System.IO.File.Exists(Application.StartupPath + @"\UPDATE.CHK"))
            {
                cDBMSAgent objSQLHelper = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);

                BeSmartMRP.Data.DBChkUpdate mDBChkUpdate = new BeSmartMRP.Data.DBChkUpdate(App.ConnectionString, App.DatabaseReside);

                if (mDBChkUpdate.CheckUpdate())
                {
                    System.IO.File.Delete(Application.StartupPath + @"\UPDATE.CHK");
                    MessageBox.Show("Update Database Structure Complete !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Update Database Structure Fail !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public static string mstrHostName = "(local)";
        public static string mstrDBMSName = "formula";

        public static string mstrHostName2 = "(local)";
        public static string mstrDBMSName2 = "formula";
        public static decimal mintInterval = 1;

        public static string mstrExportFolder = "";
        public static string mstrImportFolder = "";
        public static string mstrBakFolder = "";

        public static string mstrMSG_From = "";
        public static string mstrMSG_FromName = "";
        public static string mstrMSG_To = "";
        public static string mstrMSG_SMTPServerName = "";
        public static int mstrMSG_SMTPServerPort = 25;
        public static string mstrMSG_SMTPUserName = "";
        public static string mstrMSG_SMTPPwd = "";
        public static bool mstrMSG_SMTPIsSSL = true;

        public static string FileConfig = "";

        public static void LoadAppConfig()
        {

            //Clear Tem Before Import

            string strFileName = App.FileConfig;
            string strLine = "";
            string[] aLine = null;

            System.IO.StreamReader sr = new StreamReader(strFileName, System.Text.Encoding.Default);
            while ((strLine = sr.ReadLine()) != null)
            {

                aLine = strLine.Split("=".ToCharArray());

                if (aLine.Length <= 0)
                    continue;

                switch (aLine[0].ToString().Trim().ToUpper())
                {
                    case "SOURCENAME":
                        App.mstrHostName = aLine[1].ToString().Trim();
                        break;
                    case "DBMSNAME":
                        App.mstrDBMSName = aLine[1].ToString().Trim();
                        break;
                    case "DESTNAME":
                        App.mstrHostName2 = aLine[1].ToString().Trim();
                        break;
                    case "DBMSNAME2":
                        App.mstrDBMSName2 = aLine[1].ToString().Trim();
                        break;
                    case "INTERVAL":
                        try
                        {
                            App.mintInterval = Convert.ToDecimal(aLine[1].ToString().Trim());
                        }
                        catch
                        {
                            App.mintInterval = 1;
                        }
                        break;
                    case "EXPORT_DIR":
                        App.mstrExportFolder = aLine[1].ToString().Trim();
                        break;
                    case "IMPORT_DIR":
                        App.mstrImportFolder = aLine[1].ToString().Trim();
                        break;
                    case "BAK_DIR":
                        App.mstrBakFolder = aLine[1].ToString().Trim();
                        break;
                }

            }

            sr.Close();

        }

        #region "SetActive Corp"

        public static void SetActiveCorp(DataRow inCorp)
        {

            App.gcCorp = inCorp["fcSkid"].ToString();
            App.gcCorpName = inCorp["fcName"].ToString().TrimEnd();
            App.gcCorpName2 = inCorp["fcName2"].ToString().TrimEnd();
            App.gcCorpSaleVATType = (inCorp["fcVatType"].ToString() != string.Empty ? inCorp["fcVatType"].ToString() : "1");
            App.gdStartCorpDate = Convert.ToDateTime(inCorp["fdCalDate"]);

            BeSmartMRP.Business.Agents.KeepLogAgent.CORPID = inCorp["fcSkid"].ToString();

            System.Data.DataSet dtsCorp = new System.Data.DataSet();
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper2.SetPara(new object[] { App.AppUserID });
            if (objSQLHelper2.SQLExec(ref dtsCorp, "QEmplR", "EMPLR", "select * from AppLogin where cRowID = ? ", ref strErrorMsg))
            {
                DataRow dtrEmpl = dtsCorp.Tables["QEmplR"].Rows[0];
                App.mLocale_UI = UIHelper.AppEnum.GetLocaleEnum(dtrEmpl[QAppLogInInfo.Field.Locale_UI].ToString().TrimEnd());
                App.mLocale_Report = UIHelper.AppEnum.GetLocaleEnum(dtrEmpl[QAppLogInInfo.Field.Locale_Report].ToString().TrimEnd());

                string strLocale = "en-US";
                switch (App.mLocale_UI)
                { 
                    case AppLocale.th_TH:
                        strLocale = "th-TH"; 
                        break;
                    case AppLocale.en_US:
                        strLocale = "en-US"; 
                        break;
                }

                pmSetAppCulture(strLocale);

            }

            ActiveCorp.RowID = inCorp["fcSkid"].ToString();
            ActiveCorp.CorpChar = inCorp["fcCorpChar"].ToString();
            ActiveCorp.Code = inCorp["fcCode"].ToString();
            ActiveCorp.Name = inCorp["fcName"].ToString().TrimEnd();
            ActiveCorp.Name2 = inCorp["fcName2"].ToString().TrimEnd();
            ActiveCorp.StartAppDate = Convert.ToDateTime(inCorp["fdCalDate"]);
            ActiveCorp.Address1 = inCorp["fcAddr1"].ToString().TrimEnd();
            ActiveCorp.Address2 = inCorp["fcAddr2"].ToString().TrimEnd();
            ActiveCorp.Address12 = inCorp["fcAddr12"].ToString().TrimEnd();
            ActiveCorp.Address22 = inCorp["fcAddr22"].ToString().TrimEnd();
            ActiveCorp.ShowFormulaCompo = (inCorp["fcShowComp"].ToString().TrimEnd() != string.Empty ? inCorp["fcShowComp"].ToString() : "1");
            ActiveCorp.TelNo = inCorp["fcTel"].ToString().TrimEnd();
            ActiveCorp.FaxNo = inCorp["fcFax"].ToString().TrimEnd();
            //ActiveCorp.TaxID = inCorp["fcTaxID"].ToString().TrimEnd();
            ActiveCorp.TaxID = inCorp["FCTRADENO"].ToString().TrimEnd();
            ActiveCorp.SaleVATIsOut = (inCorp["fcVatIsOut"].ToString() != string.Empty ? inCorp["fcVatIsOut"].ToString() : "Y");
            ActiveCorp.SaleVATType = (inCorp["fcVatType"].ToString() != string.Empty ? inCorp["fcVatType"].ToString() : "1");
            ActiveCorp.SCtrlStock = (inCorp["fcCtrlStoc"].ToString().TrimEnd() != string.Empty ? inCorp["fcCtrlStoc"].ToString().TrimEnd() : "1");
            ActiveCorp.CostMethod_Goods = (inCorp["fcGoodsCos"].ToString().TrimEnd() != string.Empty ? inCorp["fcGoodsCos"].ToString().TrimEnd() : "A");
            ActiveCorp.CostMethod_Rawmat = (inCorp["fcRawCost"].ToString().TrimEnd() != string.Empty ? inCorp["fcRawCost"].ToString().TrimEnd() : "A");

            objSQLHelper.SetPara(new object[1] { inCorp["fcSect"].ToString() });
            if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Sect", "Sect", "select * from Sect where fcSkid = ? ", ref strErrorMsg))
            {
                App.ActiveCorp.DefaultSectID = dtsCorp.Tables["QVFLD_Sect"].Rows[0]["fcSkid"].ToString();
                objSQLHelper.SetPara(new object[1] { dtsCorp.Tables["QVFLD_Sect"].Rows[0]["fcDept"].ToString() });
                if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Dept", "Dept", "select * from Dept where fcSkid = ?", ref strErrorMsg))
                    App.ActiveCorp.DefaultDeptID = dtsCorp.Tables["QVFLD_Dept"].Rows[0]["fcSkid"].ToString();
            }
            objSQLHelper.SetPara(new object[1] { inCorp["fcProj"].ToString() });
            if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Proj", "Proj", "select * from Proj where fcSkid = ? ", ref strErrorMsg))
            {
                App.ActiveCorp.DefaultProjectID = dtsCorp.Tables["QVFLD_Proj"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper.SetPara(new object[1] { inCorp["fcJob"].ToString() });
            if (objSQLHelper.SQLExec(ref dtsCorp, "QVFLD_Job", "Job", "select * from Job where fcSkid = ? ", ref strErrorMsg))
            {
                App.ActiveCorp.DefaultJobID = dtsCorp.Tables["QVFLD_Job"].Rows[0]["fcSkid"].ToString();
            }

            App.ActiveCorp.MMQtyFormatString = "#,###,###.0000";
            App.ActiveCorp.PriceFormatString = "#,###,###.0000";

            string strQtyPict = inCorp["fcQtyPict"].ToString();
            string[] aText = strQtyPict.Split(".".ToCharArray());

            if (aText.Length > 1)
            {
                App.ActiveCorp.QtyFormatString = AppUtil.StringHelper.ChrTran(aText[0].Trim(), "9", "#") + "." + AppUtil.StringHelper.ChrTran(aText[1].Trim(), "9", "0");
            }
            else
            {
                App.ActiveCorp.QtyFormatString = AppUtil.StringHelper.ChrTran(aText[0].Trim(), "9", "#");
            }

            string strRemark = (Convert.IsDBNull(inCorp["fmMemData"]) ? "" : inCorp["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inCorp["fmMemData2"]) ? "" : inCorp["fmMemData2"].ToString().TrimEnd());

            string strCorpVouCentMode = BizRule.GetMemData(strRemark, "VRM");
            string strCorpVouRunLen = BizRule.GetMemData(strRemark, "VLN");
            int intCorpVouRunLen = 4;
            try
            {
                intCorpVouRunLen = Convert.ToInt32(strCorpVouRunLen);
            }
            catch { }

            //App.ActiveCorp.CorpVouCentMode = (strCorpVouCentMode == "" ? "1" : strCorpVouCentMode);
            //App.ActiveCorp.CorpVouRunLen = (intCorpVouRunLen < 4 ? 4 : intCorpVouRunLen);

        }

        private static void pmSetAppCulture(string inLocale)
        {
            CultureInfo objCI = new CultureInfo(inLocale);
            Thread.CurrentThread.CurrentCulture = objCI;
            Thread.CurrentThread.CurrentUICulture = objCI;

            DateTimeFormatInfo dtsTimeDisplay = new DateTimeFormatInfo();
            //dtsTimeDisplay.
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat=new DateTimeFormatInfo
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
                    App.AppDBChkUpdate();

                    if (inIsStartUp)
                        Application.Run(App.ofrmMainMenu);
                    //else
                        //App.ofrmMainMenu.MainStatusBar.Panels["USERNAME"].Text = "User : " + App.AppUserName;
                }
                else
                {
                    if (inIsStartUp)
                        Application.Exit();
                }
            }
        }

        #region "Report Manager"
        public static void PreviewReport(object sender)
        {
            //pmPreviewReport(sender, true, inReport);
        }

        public static void PreviewReport(object sender, bool inShowGroupTree)
        {
            //pmPreviewReport(sender, inShowGroupTree, inReport);
        }

        public static void PreviewReport()
        {
            //pmPreviewReport(null, false, inReport);
        }

        private static void pmPreviewReport(object sender, bool inShowGroupTree)
        {
            if (ofrmReportPreview != null)
            {
                ofrmReportPreview.Dispose();
            }
            ofrmReportPreview = new frmReportPreview();
            //ofrmReportPreview.DisplayGroupTree = inShowGroupTree;
            ofrmReportPreview.InitReport(sender);
            if (sender != null)
            {
                //ofrmReportPreview.TopMost = true;
                ofrmReportPreview.Show();
            }
        }
        #endregion

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


        public App()
        {
            //App.ofrmMainMenu = new BeSmartMRP.frmMainmenu();
        }

        public string mstrModule = "";
        public App(string inModule)
        {
            mstrModule = inModule;
            //App.ofrmMainMenu = new BeSmartMRP.frmMainmenu();
        }

        public void StartConfig()
        {

            App.ConfigurationManager = new BeSmartMRP.Business.Agents.ConfigurationAgent();
#if !xd_RUNMODE_DEBUG
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
#endif

#if !xd_RUNMODE_DEBUG
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
#endif

            this.pmLoadConfig();

            //this.pmDBChkUpdate();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            App.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent();
            App.PermissionManager.SQLHelper = pobjSQLUtil;
            //App.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent(App.ConnectionString, App.DatabaseReside);
            //App.PermissionManager.ModuleID = App.AppID;
            //App.PermissionManager.ConnectionString2 = App.xd_Access_ConnectionString;

            switch (this.mstrModule) { 
                case "POS":
                    App.ofrmMainMenu = new BeSmartMRP.frmMainMenuPOS1();
                    App.AppModule = "POS1_SYS";
                    break;
                default:
                    App.ofrmMainMenu = new BeSmartMRP.frmMainmenu();
                    App.AppModule = "MRPI_SYS";
                    break;
            }

#if xd_VERSION_DEMO
			if (XD_EXPIREDATE.CompareTo(DateTime.Now) < 0)
			{
				MessageBox.Show("Database is Access Deny", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			else
			{
				using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						Application.Run(App.ofrmMainMenu);
                        //Application.Run(new Form1());
                    }
					else
					{
						Application.Exit();
					}
				}
			}
#else
            using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    
                    App.AppDBChkUpdate();

                    Application.Run(App.ofrmMainMenu);
                }
                else
                {
                    Application.Exit();
                }
            }
#endif
        }

        public static bool pmAppChkVersion(ref string ioMsg)
        {

            bool bllIsOK = true;

            string strMsg = "";
            System.Data.DataSet dtsAppVer = new System.Data.DataSet();
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = new cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (!objSQLHelper.SQLExec(ref dtsAppVer, "QAppConfig", "APPCONFIG", "select * from SMAPPCONFIG ", ref strErrorMsg))
            {
                objSQLHelper.SetPara(new object[] { "$$$$0001", App.AppID, "N/A", App.AppVersion, App.AppDBVersion });
                objSQLHelper.SQLExec("insert into SMAPPCONFIG (cRowID , cRegMod , cLicNo , cAppVersion , cDBVersion ) values (?,?,?,?,?)", ref strErrorMsg);
            }

            if (objSQLHelper.SQLExec(ref dtsAppVer, "QAppConfig", "APPCONFIG", "select * from SMAPPCONFIG ", ref strErrorMsg))
            {
                string strAppVersion = dtsAppVer.Tables["QAppConfig"].Rows[0]["cAppVersion"].ToString().Trim();
                string[] aDate = strAppVersion.Split('/');
                try
                {
                    string strDateMsg = "";
                    strDateMsg += "\r\n Version โปรแกรม             : " + App.AppVersion;
                    strDateMsg += "\r\n Version โปรแกรมในระบบ : " + strAppVersion;

                    int intYear = Convert.ToInt32(aDate[2]);
                    //if (DateTime.Now.ToString("gg") == "พ.ศ.")
                    //{
                    //    intYear = intYear - 543;
                    //}

                    DateTime dttServerAppVersion = new DateTime(intYear, Convert.ToInt32(aDate[1]), Convert.ToInt32(aDate[0]));
                    int intCmp = dttServerAppVersion.CompareTo(App.dAppVersion);
                    switch (intCmp)
                    {
                        case -1:
                            strMsg += " ต้องการเปลี่ยน Version โปรแกรมในระบบ ให้ตรงกับ Version โปรแกรมล่าสุดหรือไม่ ?";
                            strMsg += " \r\n";
                            strMsg += " \r\n เนื่องจาก Version โปรแกรม นี้ ใหม่กว่า Version โปรแกรมในระบบ";
                            strMsg += strDateMsg;

                            if (MessageBox.Show(strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                objSQLHelper.SetPara(new object[] { App.AppVersion, App.AppDBVersion });
                                objSQLHelper.SQLExec("update SMAPPCONFIG set cAppVersion = ? , cDBVersion = ? ", ref strErrorMsg);
                                MessageBox.Show("Update Version โปรแกรม เป็นวันที่ " + App.AppVersion + " เสร็จสมบูรณ์", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("ไม่ Update Version โปรแกรมในระบบ ", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                bllIsOK = false;
                            }

                            break;
                        case 0:
                            strMsg = "";
                            break;
                        case 1:
                            strMsg = " ไม่สามารถเข้าทำงานได้ ! ";
                            strMsg += " \r\n";
                            strMsg += " \r\n เนื่องจากVersion โปรแกรม นี้ เก่ากว่า Version โปรแกรมในระบบ ";
                            strMsg += strDateMsg;
                            bllIsOK = false;
                            break;
                    }

                }
                catch
                {
                    bllIsOK = false;
                    strMsg = "Consistency Error ! ";
                }

            }

            ioMsg = strMsg;
            return bllIsOK;
        }

        private DateTime XD_EXPIREDATE = new DateTime(2008, 01, 31);
        public void StartApplication()
        {

            App.ConfigurationManager = new BeSmartMRP.Business.Agents.ConfigurationAgent();
#if !xd_RUNMODE_DEBUG
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
#endif

#if !xd_RUNMODE_DEBUG
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
#endif

            this.pmLoadConfig();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            App.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent();
            App.PermissionManager.SQLHelper = pobjSQLUtil;
            //App.PermissionManager = new BeSmartMRP.Business.Agents.SecurityAgent(App.ConnectionString, App.DatabaseReside);
            //App.PermissionManager.ModuleID = App.AppID;
            //App.PermissionManager.ConnectionString2 = App.xd_Access_ConnectionString;

#if xd_VERSION_DEMO
			if (XD_EXPIREDATE.CompareTo(DateTime.Now) < 0)
			{
				MessageBox.Show("Database is Access Deny", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			else
			{
				using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						Application.Run(App.ofrmMainMenu);
                        //Application.Run(new Form1());
                    }
					else
					{
						Application.Exit();
					}
				}
			}
#else
            using (BeSmartMRP.DialogForms.frmLogin dlg = new BeSmartMRP.DialogForms.frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(App.ofrmMainMenu);
                }
                else
                {
                    Application.Exit();
                }
            }
#endif
        }

#endregion

        private void Application_ApplicationExit(object sender, EventArgs e)
        {

            if (ofrmReportPreview != null)
            {
                ofrmReportPreview.Dispose();
                ofrmReportPreview = null;
            }

            if (oConnDB != null)
            {
                oConnDB = null;
            }

            if (PermissionManager != null)
            {
                PermissionManager = null;
            }

            if (ActiveCorp != null)
            {
                ActiveCorp = null;
            }
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMainmenu());
            App app = new App();
            app.StartApplication();
        }
    }
}
