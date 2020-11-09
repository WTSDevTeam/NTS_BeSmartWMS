using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgUOMConvert : Form
    {
        public dlgUOMConvert()
        {
            InitializeComponent();
        }

        public void InitForm(string inUnit1, string inUnit2)
        {
            this.lblTitle.Text = "หน่วยนับมาตรฐาน = " + inUnit1;
            //this.lblUnit1.Text = "1 " + inUnit1 + " = ";
            //this.lblUnit2.Text = inUnit2;
            this.lblUnit2.Text = inUnit1;
            this.lblUnit1.Text = "1 " + inUnit2 + " = ";
        }

        public decimal UOMQty
        {
            get { return Convert.ToDecimal(this.txtUOMQty.Value); }
            set { this.txtUOMQty.Value = value; }
        }

    }
}