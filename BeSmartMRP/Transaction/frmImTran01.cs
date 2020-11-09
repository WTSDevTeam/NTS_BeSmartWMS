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
    public partial class frmImTran01 : UIHelper.frmBase
    {
        public frmImTran01()
        {
            InitializeComponent();

            App.FileConfig = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            string strProcessFile = AppDomain.CurrentDomain.BaseDirectory + "process.chk";

            if (System.IO.File.Exists(App.FileConfig))
            {
                App.LoadAppConfig();
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SyncFMData oSyncFMData = new SyncFMData();
            oSyncFMData.ProcessImport(App.mstrImportFolder, App.mstrBakFolder);
            MessageBox.Show("Complete !");
            oSyncFMData = null;

        }
    }
}
