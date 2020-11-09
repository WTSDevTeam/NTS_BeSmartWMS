
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

    public partial class dlgShowCredit : UIHelper.frmBase
    {

        private cShowPaymentDetail oShowPymDet = null;
        private string mstrBranchID = "";
        private string mstrProd = "";
        private string mstrCoor = "";
        private string mstrCoorTN = "";

        public dlgShowCredit(string inCoor, string inCoorTN)
        {
            InitializeComponent();

            this.mstrBranchID = "";
            this.mstrCoor = inCoor;
            this.mstrCoorTN = inCoorTN;
            this.oShowPymDet = new cShowPaymentDetail("S");
        }

        public void ShowCreditDetail(string inCoor)
        {
            this.mstrBranchID = "";
            this.mstrCoor = inCoor;
        }

        private void btnShowDet_Click(object sender, EventArgs e)
        {
            this.oShowPymDet.ShowPaymentDetail("", this.mstrCoor, this.mstrCoorTN);
        }

    }
}
