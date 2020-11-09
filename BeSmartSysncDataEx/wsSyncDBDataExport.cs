
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;

using WS.Data;
using WS.Data.Agents;
using AppUtil;
using BeSmartSysncData.Agents;


namespace BeSmartSysncDataEx
{
    public class wsSyncDBDataExport : ServiceBase
    {

        private DataSet dtsDataEnv = new DataSet();

        private Timer _timer = null;
        //private Timer _timer2 = null;

        #region "Constant Field"

        public const string xdCMRem = "Rem";
        public const string xdCMRem2 = "Rm2";
        public const string xdCMRem3 = "Rm3";
        public const string xdCMRem4 = "Rm4";
        public const string xdCMRem5 = "Rm5";
        public const string xdCMRem6 = "Rm6";
        public const string xdCMRem7 = "Rm7";
        public const string xdCMRem8 = "Rm8";
        public const string xdCMRem9 = "Rm9";
        public const string xdCMRem10 = "RmA";

        public const string x_CMemDetail = "Det";
        public const string x_CMem2Detail = "Dt2";
        public const string x_CMRem = "Rem";
        public const string x_CMRem2 = "Rm2";
        public const string x_CMAd11 = "A11";
        public const string x_CMAd21 = "A21";
        public const string x_CMAd31 = "A31";
        public const string x_CMAd12 = "A12";
        public const string x_CMAd22 = "A22";
        public const string x_CMAd32 = "A32";

        public const string xdCMCtAd11 = "C11";
        public const string xdCMCtAd21 = "C21";
        public const string xdCMCtAd31 = "C31";
        public const string xdCMCtAd12 = "C12";
        public const string xdCMCtAd22 = "C22";
        public const string xdCMCtAd32 = "C32";

        public const string x_CMCtZip = "CZp";
        public const string x_CMCtTel = "CTl";
        public const string x_CMCtFax = "CFx";
        public const string x_CMTBusi = "TBu";
        public const string x_PrcSCoor = "A";		//Sale Coor ส่วนลดประจำตัว
        public const string x_CMWebSite = "Web";
        public const string x_CMEmail = "Ema";

        public const string x_CMTel = "Tel";
        public const string x_CMFax = "Fax";
        public const string x_CMTxDesti = "Des";
        public const string x_CMTaxId = "Tax";
        public const string x_CMRemLayH = "Lay";

        public const string x_CMCtNam2 = "CN2";
        public const string x_CMCtPos2 = "CP2";
        public const string x_CMCtNam3 = "CN3";
        public const string x_CMCtPos3 = "CP3";

        public const string x_CMCauseBlkLst = "BLK";
        public const string x_CMMoBileTel = "MTl";
        public const string xdCMMId = "MId";		//25/03/47 By Pic

        #endregion

        public wsSyncDBDataExport()
        {

            this.AutoLog = true;
            this.ServiceName = "BeSmart SyncData Export Service";

            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            //Sync Customer
            _timer = new Timer(); //   ทุก ๆ 5 นาที
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);

            ////Process PrintData
            //_timer2 = new Timer( 1.5 * 60000);  // 1.5 นาที
            //_timer2.Elapsed += new System.Timers.ElapsedEventHandler(_timer2_Elapsed);

        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            //System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
            //appLog.Source = "BeSmart SyncData Service";
            //appLog.WriteEntry("Start Export Service");
            this.WriteAppLog("Start Load Config [Start]");
            
            App.FileConfig = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            string strProcessFile = AppDomain.CurrentDomain.BaseDirectory + "process.chk";

            if (System.IO.File.Exists(App.FileConfig))
            {
                App.LoadAppConfig();

                try
                {
                    this.WriteAppLog("Start Process [Start]");

                    BeSmartSysncData.Agents.SyncFMData oSyncFMData = new BeSmartSysncData.Agents.SyncFMData();
                    oSyncFMData.ProcessExport(App.mstrExportFolder);
                    this.WriteAppLog("Finish Process [Start]");
                }
                catch (Exception ex)
                {
                    this.WriteAppLog(ex.Message);
                }
                finally
                {

                    this.WriteAppLog("Finish Process [Interval]");
                    decimal intInterval = App.mintInterval * 60000;
                    _timer.Interval = (double)intInterval;
                    _timer.Start();
                }

                //this.pm1Process();
                
                //_timer2.Start();
            }
            else
            {
                EventLog evt = new EventLog();
                evt.Source = "BeSmart SyncData Service";
                evt.WriteEntry("Cannot Load Config File", EventLogEntryType.Error);
            }

        }

        protected override void OnStop()
        {
            base.OnStop();
            _timer.Stop();
            //_timer2.Stop();
        }

        #region "Service Events"

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _timer.Stop();
            //_timer2.Stop();
        }

        /// <summary>
        /// OnContinue: Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
            
            App.LoadAppConfig();

            _timer.Start();
            //_timer2.Start();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
            _timer.Stop();
            //_timer2.Stop();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets,         ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);

            base.OnCustomCommand(command);
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcase Status (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnSessionChange(): To handle a change event from a Terminal Server session.
        ///   Useful if you need to determine when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }

        #endregion

        protected void _timer_Elapsed(object sender, ElapsedEventArgs e) 
        {
            //this.pm1Process();

            this.WriteAppLog("Start Process");
            try
            {
                BeSmartSysncData.Agents.SyncFMData oSyncFMData = new BeSmartSysncData.Agents.SyncFMData();
                oSyncFMData.ProcessExport(App.mstrExportFolder);
            }
            catch (Exception ex)
            {
                this.WriteAppLog(ex.Message);
            }
            this.WriteAppLog("Finish Process");
        }

        private void WriteAppLog(string e)
        {
#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\App.Log";
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Log Message :" + e);
            //Trace.WriteLine(e);
            //Trace.WriteLine("Exception StackTrace :");
            //Trace.WriteLine(e.StackTrace.ToString());
            //Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            //Trace.WriteLine("User Name : " + System.Environment.UserName);
            //Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
            //myLog.Close();
            //myLog.Dispose();
        }

        private void InitializeComponent()
        {
            // 
            // wsSyncDBData
            // 
            this.ServiceName = "BeSmart SyncData Export Service";

        }

    }

}
