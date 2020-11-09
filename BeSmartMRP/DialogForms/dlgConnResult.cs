using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.DialogForms
{
    public partial class dlgConnResult : Form
    {
        public dlgConnResult()
        {
            InitializeComponent();
            this.lstResult.Items.Clear();
        }

        public void WriteMessage(string inMsg)
        {
            this.lstResult.Items.Add(inMsg);
        }
    
    }
}