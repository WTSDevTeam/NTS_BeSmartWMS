using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using System.IO;

namespace BeSmartMRP.Transaction
{
    public partial class frmExTran01 : UIHelper.frmBase
    {
        public frmExTran01()
        {
            InitializeComponent();

            App.FileConfig = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            string strProcessFile = AppDomain.CurrentDomain.BaseDirectory + "process.chk";

            if (System.IO.File.Exists(App.FileConfig))
            {
                App.LoadAppConfig();
            }
            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SyncFMData oSyncFMData = new SyncFMData();
            oSyncFMData.ProcessExport(App.mstrExportFolder, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date);
            MessageBox.Show("Complete !");
            oSyncFMData = null;
        }
    }
}
