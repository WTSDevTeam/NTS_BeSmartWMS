//#define XD_TEST_SERVICE

using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;


namespace BeSmartSysncDataIm
{

    static class MainMenu
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

#if XD_TEST_SERVICE
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new wsSyncDBDataImport() };
            ServiceBase.Run(ServicesToRun);
#endif

        }

        public const string xd_EVENTS_FILENAME = "Error_Event.Log";
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + xd_EVENTS_FILENAME;
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Exception Message :");
            //Trace.WriteLine(e.Exception.Message.ToString());
            Trace.WriteLine("Exception StackTrace :");
            //Trace.WriteLine(e.Exception.StackTrace.ToString());
            Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            Trace.WriteLine("User Name : " + System.Environment.UserName);
            Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();

            myLog.Close();
            myLog.Dispose();

        }

    }


    public class App
    {


        private static cConn oConnDB = new cConn();
        public static string mRunRowID(string inTableName)
        {
            return oConnDB.RunRowID(inTableName);
        }

        private static string mstrERPConnectionString = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula;User ID=fm1234;Password=x2y2;";
        private static string mstrERPConnectionString2 = "PROVIDER=SQLOLEDB; Data Source=(local); Initial Catalog=formula;User ID=fm1234;Password=x2y2;";
        private static DBMSType pnDatabaseReside = DBMSType.MSSQLServer;

        public static string ERPConnectionString
        {
            get { return pmLoadERPAppConnStr(); }
        }

        public static DBMSType DatabaseReside
        {
            get { return pnDatabaseReside; }
        }

        private static string pmLoadERPAppConnStr()
        {
            string[] strPara = new string[] { App.mstrHostName, App.mstrDBMSName };
            App.mstrERPConnectionString = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return App.mstrERPConnectionString;
        }

        /// <summary>
        /// /
        /// </summary>

        public static string ERPConnectionString2
        {
            get { return pmLoadERPAppConnStr2(); }
        }

        private static string pmLoadERPAppConnStr2()
        {
            string[] strPara = new string[] { App.mstrHostName2, App.mstrDBMSName2 };
            App.mstrERPConnectionString2 = String.Format("PROVIDER=SQLOLEDB; Data Source={0}; Initial Catalog={1};User ID=fm1234;Password=x2y2;", strPara);
            return App.mstrERPConnectionString2;
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

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
        }


    }
}
