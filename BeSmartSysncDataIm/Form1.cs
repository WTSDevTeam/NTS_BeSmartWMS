using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartSysncDataIm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            App.FileConfig = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            string strProcessFile = AppDomain.CurrentDomain.BaseDirectory + "process.chk";

            if (System.IO.File.Exists(App.FileConfig))
            {
                App.LoadAppConfig();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BeSmartSysncData.Agents.SyncFMData oSyncFMData = new BeSmartSysncData.Agents.SyncFMData();
            oSyncFMData.ProcessImport(App.mstrImportFolder, App.mstrBakFolder);
            MessageBox.Show("Complete !");
            oSyncFMData = null;
        }
    }
}
