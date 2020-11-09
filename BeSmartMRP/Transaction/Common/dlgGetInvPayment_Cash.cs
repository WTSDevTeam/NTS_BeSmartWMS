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
    public partial class dlgGetInvPayment_Cash : UIHelper.frmBase
    {
        public dlgGetInvPayment_Cash()
        {
            InitializeComponent();
        }

        private decimal mdecTotAmt = 0;
        public void pmInitForm(decimal inTotAmt)
        {
            this.mdecTotAmt = inTotAmt;
            this.txtInvAmt.Value = this.mdecTotAmt;
            this.txtRecvAmt.SelectAll();
            this.txtRecvAmt.Focus();
        }

        public decimal Amt
        {
            get { return this.txtRecvAmt.Value; }
        }

        private void txtRecvAmt_ValueChanged(object sender, EventArgs e)
        {
            //if (this.txtRecvAmt.Value != 0)
            this.txtChangeAmt.Value = this.txtRecvAmt.Value - this.txtInvAmt.Value;
            if (this.txtRecvAmt.Value - this.txtInvAmt.Value < 0) this.txtChangeAmt.Value = 0;
        }

        private void txtRecvAmt_Validated(object sender, EventArgs e)
        {
            //if (this.txtRecvAmt.Value != 0)
            this.txtChangeAmt.Value = this.txtRecvAmt.Value - this.txtInvAmt.Value;
            if (this.txtRecvAmt.Value - this.txtInvAmt.Value < 0) this.txtChangeAmt.Value = 0;
        }

        private bool pmValidateRecvAmt()
        {
            bool bllResult = false;
            //if (this.txtRecvAmt.Value > this.txtInvAmt.Value) { }
            if (this.txtRecvAmt.Value == 0) { }
            else
            {
                bllResult = true;
            }
            return bllResult;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (pmValidateRecvAmt())
            {
                this.DialogResult = DialogResult.OK;
            }
            else {
                MessageBox.Show("ระบุยอดชำระเงินไม่ถูกต้อง");
            }
        }

    }
}
