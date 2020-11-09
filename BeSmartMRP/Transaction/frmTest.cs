using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction
{
    public partial class frmTest : UIHelper.frmBase
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("1");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("2");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{F1}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ribbonControl1.Visible = !ribbonControl1.Visible;
        }
    }
}
