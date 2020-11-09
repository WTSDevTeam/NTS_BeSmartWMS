
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

    public partial class dlgHisOpt1 : UIHelper.frmBase
    {

        public dlgHisOpt1()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private DataSet dtsDataEnv = new DataSet();

        private DialogForms.dlgGetProd pofrmGetProd = null;

        public bool IsGetProd
        {
            set 
            { 
                this.pnlProd.Visible = value;
                if (this.pnlProd.Visible == false)
                {
                    this.Size = new Size(414, 170);
                }
            }
        }

        public string BegQcProd
        {
            get { return this.txtBegQcProd.Text; }
        }

        public string EndQcProd
        {
            get { return this.txtEndQcProd.Text; }
        }

        public DateTime BegDate
        {
            get { return this.txtBegDate.DateTime.Date; }
        }

        public DateTime EndDate
        {
            get { return this.txtEndDate.DateTime.Date; }
        }

        private void pmInitForm()
        {
            this.pmDefaultOption();
        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcCode from Prod where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.txtBegQcProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
            }

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcCode from Prod where fcCorp = ? order by fcCode desc", ref strErrorMsg))
            {
                this.txtEndQcProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
            }
            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "PRODUCT":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    if (this.pofrmGetProd != null)
                    {
                        DataRow dtrPopup = this.pofrmGetProd.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPROD")
                                this.txtBegQcProd.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            else
                                this.txtEndQcProd.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                        }
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }
        }

        private void pmPopUpButtonClick(string inTextbox, string inPara1)
        {
            switch (inTextbox)
            {
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    this.pmInitPopUpDialog("PRODUCT");
                    this.pofrmGetProd.ValidateField("", "FCCODE", false);

                    if (this.pofrmGetProd.PopUpResult)
                        this.pmRetrievePopUpVal(inTextbox);
                    break;
            }
        }

        private void txtBegQcProd_WsOnValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "fcCode";
            if (txtPopup.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("PRODUCT");
                e.Cancel = !this.pofrmGetProd.ValidateField(txtPopup.Text, strOrderBy, false);

                if (this.pofrmGetProd.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopup.Name);
                }
            }

        }

    }
}
