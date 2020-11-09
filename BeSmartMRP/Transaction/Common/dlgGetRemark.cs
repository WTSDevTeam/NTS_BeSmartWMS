using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetRemark : UIHelper.frmBase
    {
        public dlgGetRemark()
        {
            InitializeComponent();
            UIHelper.UIBase.SetDefaultChildAppreance(this);
        }

        public bool DescReadOnly
        {
            set
            {
                this.txtDesc1.Properties.ReadOnly = value;
                this.txtDesc2.Properties.ReadOnly = value;
                this.txtDesc3.Properties.ReadOnly = value;
                this.txtDesc4.Properties.ReadOnly = value;
                this.txtDesc5.Properties.ReadOnly = value;
                this.txtDesc6.Properties.ReadOnly = value;
                this.txtDesc7.Properties.ReadOnly = value;
                this.txtDesc8.Properties.ReadOnly = value;
                this.txtDesc9.Properties.ReadOnly = value;
                this.txtDesc10.Properties.ReadOnly = value;
            }
        }

        public void SetLabelText(string[] inText)
        {
            if (inText.Length > 0)
            {
                this.lblRemark1.Text = inText[0];
            }
            if (inText.Length > 1)
            {
                this.lblRemark2.Text = inText[1];
            }
            if (inText.Length > 2)
            {
                this.lblRemark3.Text = inText[2];
            }
            if (inText.Length > 3)
            {
                this.lblRemark4.Text = inText[3];
            }
            if (inText.Length > 4)
            {
                this.lblRemark5.Text = inText[4];
            }
            if (inText.Length > 5)
            {
                this.lblRemark6.Text = inText[5];
            }
            if (inText.Length > 6)
            {
                this.lblRemark7.Text = inText[6];
            }
            if (inText.Length > 7)
            {
                this.lblRemark8.Text = inText[7];
            }
            if (inText.Length > 8)
            {
                this.lblRemark9.Text = inText[8];
            }
            if (inText.Length > 9)
            {
                this.lblRemark10.Text = inText[9];
            }
        }

        public string Desc1
        {
            get { return this.txtDesc1.Text.TrimEnd(); }
            set { this.txtDesc1.Text = value; }
        }
        public string Desc2
        {
            get { return this.txtDesc2.Text.TrimEnd(); }
            set { this.txtDesc2.Text = value; }
        }
        
        public string Desc3
        {
            get { return this.txtDesc3.Text.TrimEnd(); }
            set { this.txtDesc3.Text = value; }
        }

        public string Desc4
        {
            get { return this.txtDesc4.Text.TrimEnd(); }
            set { this.txtDesc4.Text = value; }
        }

        public string Desc5
        {
            get { return this.txtDesc5.Text.TrimEnd(); }
            set { this.txtDesc5.Text = value; }
        }

        public string Desc6
        {
            get { return this.txtDesc6.Text.TrimEnd(); }
            set { this.txtDesc6.Text = value; }
        }

        public string Desc7
        {
            get { return this.txtDesc7.Text.TrimEnd(); }
            set { this.txtDesc7.Text = value; }
        }

        public string Desc8
        {
            get { return this.txtDesc8.Text.TrimEnd(); }
            set { this.txtDesc8.Text = value; }
        }

        public string Desc9
        {
            get { return this.txtDesc9.Text.TrimEnd(); }
            set { this.txtDesc9.Text = value; }
        }

        public string Desc10
        {
            get { return this.txtDesc10.Text.TrimEnd(); }
            set { this.txtDesc10.Text = value; }
        }
    
    }
}
