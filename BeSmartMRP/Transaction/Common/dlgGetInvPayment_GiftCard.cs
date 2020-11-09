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
    public partial class dlgGetInvPayment_GiftCard : UIHelper.frmBase
    {
        public dlgGetInvPayment_GiftCard()
        {
            InitializeComponent();
        }

        private decimal mdecPymAmt = 0;

        public void pmInitForm(decimal inAmt)
        {
            mdecPymAmt = inAmt;
            this.txtInvAmt.Value = inAmt;
            //this.txtRecvAmt.Value = inAmt;
            //this.txtCardNo.Focus();
        }

        private string mstrPymType = "VISA";
        public string Code
        {
            get { return this.mstrPymType; }
        }

        public string RefCode
        {
            get { return this.txtCardNo.Text.TrimEnd(); }
        }

        public decimal Amt
        {
            get { return this.txtRecvAmt.Value; }
        }

        private void btn_Pym3_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn.Tag != null)
            {
                string strVCCode = btn.Tag.ToString();
                this.txtPayType.Text = btn.Text;
                this.mstrPymType = strVCCode;
                switch (strVCCode.ToUpper())
                {
                    case "VC100":
                        this.txtRecvAmt.Value = 100;
                        break;
                    case "VC600":
                        this.txtRecvAmt.Value = 600;
                        break;
                    case "VC1200":
                        this.txtRecvAmt.Value = 1200;
                        break;
                }
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string strErrorMsg = "";
            if (pmValidateForm(ref strErrorMsg))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(strErrorMsg);
            }

        }

        private bool pmValidateForm(ref string ioErrorMsg)
        {
            bool bllResult = true;
            return bllResult;
        }

    }
}
