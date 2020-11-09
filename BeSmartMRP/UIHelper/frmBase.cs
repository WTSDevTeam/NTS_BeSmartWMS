using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.UIHelper
{
    public partial class frmBase : DevExpress.XtraEditors.XtraForm
    {
        public frmBase()
        {
            InitializeComponent();
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            
            //UIHelper.KeyControl.Send(this.ActiveControl, e);
            UIHelper.KeyControl.Send(this.ActiveControl, e);

        }

        //protected override void OnClosing(CancelEventArgs ce)
        //{
        //    if (this.pmClosingApp())
        //    {
        //        base.OnClosing(ce);
        //        Application.Exit();
        //    }
        //    else
        //        ce.Cancel = true;
        //}

        //private bool pmClosingApp()
        //{
        //    return (MessageBox.Show(this, "ต้องการที่จะปิดหน้าจอหรือไม่ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        //}
    
    }
}