
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetInvPayment_CredCard : UIHelper.frmBase
    {
        public dlgGetInvPayment_CredCard()
        {
            InitializeComponent();
        }

        private decimal mdecPymAmt = 0;

        public void pmInitForm(decimal inAmt) 
        {
            mdecPymAmt = inAmt;
            this.txtInvAmt.Value = inAmt;
            this.txtRecvAmt.Value = inAmt;
            this.txtCardNo.Focus();
            pmSetPaymentType("CRED_VISA");
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

        private void pmSetPaymentType(string inPymType)
        {
            this.mstrPymType = inPymType;
            switch (inPymType)
            {
                case "CRED_VISA":
                    this.txtPayType.Text = "VISA";
                    this.mstrPymType = "VISA";
                    break;
                case "CRED_MC":
                    this.txtPayType.Text = "MASTER CARD";
                    this.mstrPymType = "MC";
                    break;
            }
            this.txtRecvAmt.Focus();
        }

        private void btn_Pym1_Click(object sender, EventArgs e)
        {
            pmSetPaymentType("CRED_VISA");
        }

        private void btn_Pym2_Click(object sender, EventArgs e)
        {
            pmSetPaymentType("CRED_MC");
        }

        private void btn_Pym3_Click(object sender, EventArgs e)
        {
            pmSetPaymentType("PYM_VC");
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
            bool bllResult = false;
            if (this.txtCardNo.Text.Trim() == "")
            {
                ioErrorMsg = "กรุณาระบุเลขบัตรเครดิต";
                this.txtCardNo.Focus();
            }
            else if (this.txtRecvAmt.Value > this.txtInvAmt.Value)
            {
                ioErrorMsg = "ยอดรับชำระมากกว่ามูลค่าที่ต้องชำระไม่ได้";
                this.txtRecvAmt.Focus();
            }
            else
            {
                bllResult = true;
            }
            return bllResult;
        }


    }
}
