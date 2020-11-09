
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
    public partial class dlgGetInvPayment : UIHelper.frmBase
    {
        public dlgGetInvPayment(decimal inTotalAmt)
        {
            InitializeComponent();

            this.txtInvAmt.Value = inTotalAmt;
            this.txtBackAmt.Value = inTotalAmt;
            this.txtRecvAmt.Focus();

            //this.pmSetPaymentType("CASH");

        }

        private string mstrTemPym = "TemPym";

        private DataSet dtsDataEnv = new DataSet();

        public void BindData(DataSet inSource)
        {
            dtsDataEnv = inSource;
            pmInitGridProp();
            this.pmSumPayment();
        }

        private void pmInitGridProp()
        {
            //this.grdBrowView.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias], "");
            //this.grdBrowView.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPym].DefaultView;
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrTemPym];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            //this.gridView1.Columns["cType"].Group();

            this.gridView1.Columns["CPOSPAYTYPE"].VisibleIndex = i++;
            this.gridView1.Columns["CCODE"].VisibleIndex = i++;
            this.gridView1.Columns["CREFCODE"].VisibleIndex = i++;
            this.gridView1.Columns["NPAYAMT"].VisibleIndex = i++;

            this.gridView1.Columns["CPOSPAYTYPE"].Caption = "ประเภท";
            this.gridView1.Columns["CCODE"].Caption = "รหัส";
            this.gridView1.Columns["CREFCODE"].Caption = "เลขที่บัตร/อ้างอิง";
            this.gridView1.Columns["NPAYAMT"].Caption = "จำนวนเงิน";

            this.gridView1.Columns["CPOSPAYTYPE"].Width = 60;
            this.gridView1.Columns["CCODE"].Width = 40;
            //this.gridView1.Columns["CREFCODE"].Width = 60;
            this.gridView1.Columns["NPAYAMT"].Width = 50;

            this.gridView1.Columns["NPAYAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NPAYAMT"].DisplayFormat.FormatString = "#,###,###.00";

            //this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView1.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;

            //this.gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "nAmt", this.gridView1.Columns["nAmt"], "{0:n2}");

            //this.grdBrowView.Focus();
            //this.gridView1.ExpandAllGroups();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtRecvAmt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
        }

        private void txtRecvAmt_EnabledChanged(object sender, EventArgs e)
        {
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
            if (this.txtBackAmt.Value != 0) { }
            //else if (this.txtRecvAmt.Value == 0) {}
            else
            {
                bllResult = true;
            }
            return bllResult;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.pmInitPaymentDialog("SUBMIT");
        }

        private void pmInitPaymentDialog(string inPayType)
        {
            switch (inPayType)
            { 
                case "SUBMIT":
                    if (pmValidateRecvAmt())
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        this.txtRecvAmt.Focus();
                        this.txtRecvAmt.SelectAll();
                        MessageBox.Show("ระบุยอดชำระเงินไม่ถูกต้อง");
                    }

                    break;
                case "CASH":
                    using (Common.dlgGetInvPayment_Cash dlg1 = new dlgGetInvPayment_Cash())
                    {
                        dlg1.pmInitForm(this.txtBackAmt.Value);
                        dlg1.ShowDialog();
                        if (dlg1.DialogResult == DialogResult.OK)
                        {
                            this.pmInsertPayment("CASH", "CASH", "", dlg1.Amt);
                        }
                    }
                    break;
                case "CRED":
                    using (Common.dlgGetInvPayment_CredCard dlg1 = new dlgGetInvPayment_CredCard())
                    {
                        dlg1.pmInitForm(this.txtBackAmt.Value);
                        dlg1.ShowDialog();
                        if (dlg1.DialogResult == DialogResult.OK)
                        {
                            this.pmInsertPayment("CREDIT", dlg1.Code, dlg1.RefCode, dlg1.Amt);
                        }
                    }
                    break;
                case "VC":
                    using (Common.dlgGetInvPayment_GiftCard dlg1 = new dlgGetInvPayment_GiftCard())
                    {
                        dlg1.pmInitForm(this.txtBackAmt.Value);
                        dlg1.ShowDialog();
                        if (dlg1.DialogResult == DialogResult.OK)
                        {
                            this.pmInsertPayment("VOUCHER", dlg1.Code, dlg1.RefCode, dlg1.Amt);
                        }
                    }
                    break;
            }
        }

        private void btn_Pym1_Click(object sender, EventArgs e)
        {
            this.pmInitPaymentDialog("CASH");
        }

        private void btn_Pym2_Click(object sender, EventArgs e)
        {
            this.pmInitPaymentDialog("CRED");
        }

        private void btn_Pym3_Click(object sender, EventArgs e)
        {
            this.pmInitPaymentDialog("VC");
        }

        private void pmInsertPayment(string inPayType, string inPayCode, string inPayRefCode, decimal inPayAmt)
        {
            DataRow dtrPym = this.dtsDataEnv.Tables[this.mstrTemPym].NewRow();
            dtrPym["CPOSPAYTYPE"] = inPayType;
            dtrPym["CCODE"] = inPayCode;
            dtrPym["CREFCODE"] = inPayRefCode;
            dtrPym["NPAYAMT"] = inPayAmt;
            this.dtsDataEnv.Tables[this.mstrTemPym].Rows.Add(dtrPym);
            this.pmSumPayment();

            this.gridView1.SelectRow(this.gridView1.RowCount);

        }

        private string mstrPymType = "CASH";
        public string TranPayType 
        {
            get { return this.mstrPymType; }
        }

        private void pmSetPaymentType(string inPymType) 
        {
            this.mstrPymType = inPymType;
            switch (inPymType)
            { 
                case "CASH":
                    this.txtPayType.Text = "CASH";
                    break;
                case "CRED":
                    this.txtPayType.Text = "CREDIT";
                    this.txtRecvAmt.Value = this.txtInvAmt.Value;
                    this.txtRecvAmt.SelectAll();
                    break;
                case "VC":
                    this.txtRecvAmt.SelectAll();
                    break;
            }
            this.txtRecvAmt.Focus();
        }

        private void pmSumPayment()
        {
            decimal decTotAmt = 0;
            foreach (DataRow dtrSum in this.dtsDataEnv.Tables[this.mstrTemPym].Rows) 
            {
                decTotAmt += Convert.ToDecimal(dtrSum["NPAYAMT"]);
            }

            this.txtRecvAmt.Value = decTotAmt;
            this.txtBackAmt.Value = this.txtInvAmt.Value - decTotAmt;
            if (decTotAmt > this.txtInvAmt.Value)
            {
                this.txtChangeAmt.Value = decTotAmt - this.txtInvAmt.Value;
                this.txtBackAmt.Value = 0;
            }

        }

        private void btnDel_Pym_Click(object sender, EventArgs e)
        {
            this.pmDeletePayment();
        }

        private void pmDeletePayment()
        {
            if (this.gridView1.GetFocusedDataRow() != null)
            {
                DataRow dtrPym = this.gridView1.GetFocusedDataRow();
                dtrPym["CPOSPAYTYPE"] = "";
                dtrPym["CCODE"] = "";
                dtrPym["CREFCODE"] = "";
                dtrPym["NPAYAMT"] = 0;
            }
            this.pmSumPayment();
        }

        private void dlgGetInvPayment_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.F1:
                    this.pmInitPaymentDialog("CASH");
                    break;
                case Keys.F2:
                    this.pmInitPaymentDialog("CRED");
                    break;
                case Keys.F3:
                    this.pmInitPaymentDialog("VC");
                    break;
                case Keys.F10:
                    this.pmInitPaymentDialog("SUBMIT");
                    break;
                case Keys.Delete:
                    this.pmDeletePayment();
                    break;
            }
        }

    }
}

