﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartSysncData
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

            Agents.SyncFMData oSyncFMData = new Agents.SyncFMData();
            oSyncFMData.ProcessExport(App.mstrExportFolder);
            MessageBox.Show("Complete !");
            oSyncFMData = null;
        }
    }
}
