using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{
    public partial class frmRPTSelect : UIHelper.frmBase
    {
        public frmRPTSelect()
        {
            InitializeComponent();
        }

        public void LoadRPT(string inPath)
        {
            int intLen1 = (inPath).Length;
            this.lstRPT.Items.Clear();
            string[] strADir = System.IO.Directory.GetFiles(inPath);
            for (int i = 0; i < strADir.Length; i++)
            {
                string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
                this.lstRPT.Items.Add(strFormName);
            }
            if (this.lstRPT.Items.Count > 0)
            {
                this.lstRPT.SelectedIndex = 0;
            }
        }

    }
}
