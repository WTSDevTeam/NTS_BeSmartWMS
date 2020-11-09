using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetCancleRem : UIHelper.frmBase
    {
        public dlgGetCancleRem()
        {
            InitializeComponent();
        }

        public string Remark
        {
            get { return this.txtRemark.Text.TrimEnd(); }
        }

    }
}
