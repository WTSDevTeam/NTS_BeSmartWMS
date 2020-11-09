
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
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetQty : UIHelper.frmBase
    {
        public dlgGetQty()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private DialogForms.dlgGetUM ofrmUOM = null;
        private DataSet dtsDataEnv = new DataSet();
        private DataRow mdtrActiveRow = null;

        //private frmOrder ofrmParent_Inv = null;
        //private frmOrder ofrmParent_Ord = null;
        private IfrmStockBase ofrmParent = null;

        private bool mbllIsFixUMQty = false;
        private string mstrProd = "";
        private string mstrChkUM = "FNUMQTY1";

        //public void SetParentForm(frmInvoice inParent)
        //{
        //    this.ofrmParent_Inv = inParent;
        //}

        public void SetParentForm(IfrmStockBase inParent)
        {
            this.ofrmParent = inParent;
        }

        public DataRow Row
        {
            get { return this.mdtrActiveRow; }
            set
            {
                this.mdtrActiveRow = value;
            }
        }

        public bool IsFixUMQty
        {
            get { return this.mbllIsFixUMQty; }
            set
            {
                this.mbllIsFixUMQty = value;
                this.txtQnUM.Enabled = !this.mbllIsFixUMQty;
                this.txtUMQty.Enabled = !this.mbllIsFixUMQty;
                this.txtQnStUM.Enabled = !this.mbllIsFixUMQty;
                this.txtStUMQty.Enabled = !this.mbllIsFixUMQty;
            }
        }

        #region "หน่วยนับ"
        /// <summary>
        /// จำนวน
        /// </summary>
        public decimal Qty
        {
            get { return Convert.ToDecimal(this.txtQty.Value); }
            set { this.txtQty.Value = value; }
        }

        /// <summary>
        /// อัตราแปลงหน่วย
        /// </summary>
        public decimal UMQty
        {
            get { return Convert.ToDecimal(this.txtUMQty.Value); }
            set { this.txtUMQty.Value = value; }
        }

        /// <summary>
        /// RowID ของหน่วยนับ
        /// </summary>
        public string UM
        {
            get { return this.txtQnUM.Tag.ToString(); }
            set
            {
                this.txtQnUM.Tag = value;
                this.txtQnUM.Text = this.pmLoadQnUm(this.txtQnUM.Tag.ToString());
                this.txtQnUM2.Text = this.txtQnUM.Text;

                if (!this.mbllIsFixUMQty)
                    this.txtUMQty.Enabled = (this.txtQnUM.Tag != this.txtQnStdUM.Tag);
            }
        }

        /// <summary>
        /// ชื่อของหน่วยนับ
        /// </summary>
        public string UMName
        {
            get { return this.txtQnUM.Text; }
        }

        /// <summary>
        /// RowID ของหน่วยนับมาตรฐาน
        /// </summary>
        public string StdUM
        {
            get { return this.txtQnStdUM.Tag.ToString(); }
            set
            {
                this.txtQnStdUM.Tag = value;
                this.txtQnStdUM.Text = this.pmLoadQnUm(this.txtQnStdUM.Tag.ToString());
                if (!this.mbllIsFixUMQty)
                    this.txtUMQty.Enabled = (this.txtQnUM.Tag != this.txtQnStdUM.Tag.ToString());
            }
        }

        /// <summary>
        /// ชื่อของหน่วยนับมาตรฐาน
        /// </summary>
        public string StdUMName
        {
            get { return this.txtQnStdUM.Text; }
        }

        #endregion

        #region "หน่วยคุมสต็อค"
        /// <summary>
        /// จำนวน ตามหน่วยคุมสต็อค
        /// </summary>
        public decimal StQty
        {
            get { return Convert.ToDecimal(this.txtStQty.Value); }
            set { this.txtStQty.Value = value; }
        }

        /// <summary>
        /// อัตราแปลงหน่วยคุมสต็อค
        /// </summary>
        public decimal StUMQty
        {
            get { return Convert.ToDecimal(this.txtStUMQty.Value); }
            set { this.txtStUMQty.Value = value; }
        }

        /// <summary>
        /// RowID ของหน่วยคุมสต็อค
        /// </summary>
        public string StUM
        {
            get { return this.txtQnStUM.Tag.ToString(); }
            set
            {
                this.txtQnStUM.Tag = value;
                this.txtQnStUM.Text = this.pmLoadQnUm(this.txtQnStUM.Tag.ToString());
                this.txtQnStUM2.Text = this.txtQnStUM.Text;
                this.txtStUMQty.Enabled = (this.txtQnStUM.Tag != this.txtQnStdStUM.Tag);
            }
        }

        /// <summary>
        /// ชื่อของหน่วยนับคุมสต็อค
        /// </summary>
        public string StUMName
        {
            get { return this.txtQnStUM.Text; }
        }

        /// <summary>
        /// RowID ของหน่วยนับมาตรฐาน หน่วยคุมสต็อค
        /// </summary>
        public string StStdUM
        {
            get { return this.txtQnStdStUM.Tag.ToString(); }
            set
            {
                this.txtQnStdStUM.Tag = value;
                this.txtQnStdStUM.Text = this.pmLoadQnUm(this.txtQnStdStUM.Tag.ToString());
                this.txtStUMQty.Enabled = (this.txtQnStUM.Tag != this.txtQnStdStUM.Tag);
            }
        }

        /// <summary>
        /// ชื่อของหน่วยนับคุมสต็อคมาตรฐาน
        /// </summary>
        public string StStdUMName
        {
            get { return this.txtQnStdStUM.Text; }
        }

        #endregion

        public string ProdID
        {
            set { this.mstrProd = value; }
            get { return this.mstrProd; }
        }

        public string CheckUM
        {
            set { this.mstrChkUM = value; }
            get { return this.mstrChkUM; }
        }

        private void pmInitForm()
        {
            this.pmInitializeComponent();
        }

        private void pmInitializeComponent()
        {
            UIBase.SetDefaultChildAppreance(this);
            this.pmSetFormUI();
        }

        private void pmSetFormUI()
        {

            this.Text = UIBase.GetAppUIText(new string[] { "ระบุจำนวนสินค้า และ หน่วยนับ", "Specifiy Qty and Unit of Meterial" });
            this.lblQty11.Text = UIBase.GetAppUIText(new string[] { "จำนวนหน่วยนับมาตรฐาน :", "Qty :" });
            this.lblQty13.Text = UIBase.GetAppUIText(new string[] { "=", "=" });

            this.lblQty21.Text = UIBase.GetAppUIText(new string[] { "จำนวนหน่วยคุมสต็อค :", "Stock Qty :" });
            this.lblQty23.Text = UIBase.GetAppUIText(new string[] { "=", "=" });

            this.txtQty.Properties.EditMask = App.ActiveCorp.QtyFormatString;
            this.txtQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtQty.Properties.DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.txtUMQty.Properties.EditMask = App.ActiveCorp.QtyFormatString;
            this.txtUMQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtUMQty.Properties.DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.txtStQty.Properties.EditMask = App.ActiveCorp.QtyFormatString;
            this.txtStQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStQty.Properties.DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.txtStUMQty.Properties.EditMask = App.ActiveCorp.QtyFormatString;
            this.txtStUMQty.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStUMQty.Properties.DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "UOM":
                    if (this.ofrmUOM == null)
                    {
                        this.ofrmUOM = new DialogForms.dlgGetUM();
                        this.ofrmUOM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.ofrmUOM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            DataRow dtrPopup = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "UOM":
                    dtrPopup = this.ofrmUOM.RetrieveValue();
                    if (dtrPopup != null)
                    {
                        this.txtQnUM.Tag = dtrPopup["fcSkid"].ToString();
                        this.txtQnUM.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        this.txtQnUM2.Text = this.txtQnUM.Text;

                        string strErrorMsg = "";
                        WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                        //
                        if (this.txtUMQty.Value == 1)
                        {
                            pobjSQLUtil.SetPara(new object[1] { this.mstrProd });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                                decimal decUMQty = Convert.ToDecimal(dtrProd[this.mstrChkUM]);
                                this.txtUMQty.Value = (decUMQty == 0 ? 1 : decUMQty);
                            }
                        }

                    }
                    else
                    {
                        this.txtQnUM.Tag = "";
                        this.txtQnUM.Text = "";
                        this.txtQnUM2.Text = "";
                    }
                    if (!this.mbllIsFixUMQty)
                        this.txtUMQty.Enabled = (this.txtQnUM.Tag != this.txtQnStdUM.Tag);
                    break;
                case "STUOM":
                    dtrPopup = this.ofrmUOM.RetrieveValue();
                    if (dtrPopup != null)
                    {
                        this.txtQnStUM.Tag = dtrPopup["fcSkid"].ToString();
                        this.txtQnStUM.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        this.txtQnStUM2.Text = this.txtQnStUM.Text;
                    }
                    else
                    {
                        this.txtQnStUM.Tag = "";
                        this.txtQnStUM.Text = "";
                        this.txtQnStUM2.Text = "";
                    }
                    this.txtStUMQty.Enabled = (this.txtQnStUM.Tag != this.txtQnStdStUM.Tag);
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper());
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper());
                    break;
            }

        }

        private void pmPopUpButtonClick(string inTextbox)
        {
            switch (inTextbox)
            {
                case "TXTQNUM":
                    this.pmInitPopUpDialog("UOM");
                    this.ofrmUOM.ValidateField("", "fcName", true);
                    if (this.ofrmUOM.PopUpResult)
                        this.pmRetrievePopUpVal("UOM");
                    break;
                case "TXTQNSTUM":
                    this.pmInitPopUpDialog("STUOM");
                    this.ofrmUOM.ValidateField("", "fcName", true);
                    if (this.ofrmUOM.PopUpResult)
                        this.pmRetrievePopUpVal("STUOM");
                    break;
            }
        }

        private void txtQnUM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCNAME";
            if (txtPopUp.Text == "")
            {
                this.txtQnUM.Tag = "";
                this.txtQnUM.Text = "";
                this.txtQnUM2.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("UOM");
                e.Cancel = !this.ofrmUOM.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.ofrmUOM.PopUpResult)
                {
                    this.pmRetrievePopUpVal("UOM");
                }
            }
        }

        private void txtQnStUM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCNAME";
            if (txtPopUp.Text == "")
            {
                this.txtQnStUM.Tag = "";
                this.txtQnStUM.Text = "";
                this.txtQnStUM2.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("UOM");
                e.Cancel = !this.ofrmUOM.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.ofrmUOM.PopUpResult)
                {
                    this.pmRetrievePopUpVal("STUOM");
                }
            }
        }

        private string pmLoadQnUm(string inUOM)
        {
            string strResult = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { inUOM });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select FCCODE,FCNAME from UM where fcSkid = ?", ref strErrorMsg))
            {
                strResult = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            return strResult;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            string strErrorMsg = "";
            if (this.ofrmParent != null)
            {

                string lcOUm = this.mdtrActiveRow["cUOM"].ToString();
                string lcOQnUm = this.mdtrActiveRow["cQnUOm"].ToString();
                decimal lnOUmQty = Convert.ToDecimal(this.mdtrActiveRow["nUOMQty"]);
                decimal lnOQty = Convert.ToDecimal(this.mdtrActiveRow["nQty"].ToString());
                string lcOStUm = this.mdtrActiveRow["cStUOM"].ToString();
                string lcOStQnUm = this.mdtrActiveRow["cStQnUOM"].ToString();
                decimal lnOStUmQty = Convert.ToDecimal(this.mdtrActiveRow["nStUOMQty"]);
                decimal lnOStQty = Convert.ToDecimal(this.mdtrActiveRow["nStQty"].ToString());
                decimal lnOPrice = Convert.ToDecimal(this.mdtrActiveRow["nPrice"].ToString());
                decimal lnODiscAmt = Convert.ToDecimal(this.mdtrActiveRow["nDiscAmt"].ToString());

                this.txtUMQty.Value = (Convert.ToDecimal(this.txtUMQty.Value) == 0 ? 1 : Convert.ToDecimal(this.txtUMQty.Value));
                this.txtStUMQty.Value = (Convert.ToDecimal(this.txtStUMQty.Value) == 0 ? 1 : Convert.ToDecimal(this.txtStUMQty.Value));

                BizRule.GetQtyPriceInUm2(Convert.ToDecimal(this.mdtrActiveRow["nPrice"]), Convert.ToDecimal(this.mdtrActiveRow["nUOMQty"]), Convert.ToDecimal(this.txtUMQty.Value));

                this.mdtrActiveRow["nQty"] = Convert.ToDecimal(this.txtQty.Value);
                this.mdtrActiveRow["cUOM"] = this.txtQnUM.Tag.ToString();
                this.mdtrActiveRow["cQnUom"] = this.txtQnUM.Text.TrimEnd();
                this.mdtrActiveRow["nUOMQty"] = Convert.ToDecimal(this.txtUMQty.Value);
                this.mdtrActiveRow["nStQty"] = Convert.ToDecimal(this.txtStQty.Value);
                this.mdtrActiveRow["cStUOM"] = this.txtQnStUM.Tag.ToString();
                this.mdtrActiveRow["nStUOMQty"] = Convert.ToDecimal(this.txtStUMQty.Value);

                bool bllSucc = false;
                if (this.ofrmParent != null)
                {
                    bllSucc = this.ofrmParent.ValidateQty(true, ref strErrorMsg);
                }

                if (bllSucc)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.mdtrActiveRow["cUOM"] = lcOUm;
                    this.mdtrActiveRow["cQnUom"] = lcOQnUm;
                    this.mdtrActiveRow["nUOMQty"] = lnOUmQty;
                    this.mdtrActiveRow["nQty"] = lnOQty;
                    this.mdtrActiveRow["nPrice"] = lnOPrice;
                    this.mdtrActiveRow["nDiscAmt"] = lnODiscAmt;
                    this.mdtrActiveRow["cStUOM"] = lcOStUm;
                    this.mdtrActiveRow["cStQnUOM"] = lcOStQnUm;
                    this.mdtrActiveRow["nStUOMQty"] = lnOStUmQty;
                    this.mdtrActiveRow["nStQty"] = lnOStQty;
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

        }

    }
}
