
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
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;


namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetLotWHouse : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = null;

        private string mstrBranch = "";
        private int mintParentID = -1;

        private string mstrProdID = "";
        private string mstrWHouse = "";

        private IfrmStockBase ofrmParent = null;

        private dlgSelLot podlgGetLot = null;
        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;

        private bool mbllIsQInsertPdSer = false;

        private string mstrSaleOrBuy = "S";
        public string SaleOrBuy
        {
            set { this.mstrSaleOrBuy = value; }
        }

        public string ProdID
        {
            get { return this.mstrProdID; }
            set { this.mstrProdID = value; }
        }

        public string Lot
        {
            get { return this.txtLot.Text; }
        }

        public void SetParentForm(IfrmStockBase inParent)
        {
            this.ofrmParent = inParent;
        }

        private void pmInitForm()
        {
            this.pmInitializeComponent();
        }

        private void pmInitializeComponent()
        {
            this.txtLot.Properties.MaxLength = 20;
            UIBase.SetDefaultChildAppreance(this);
        }

        public void BindData(DataSet inDataEnv, int inParent, string inBranch, string inWHouse, string inLot)
        {
            this.mintParentID = inParent;
            this.dtsDataEnv = inDataEnv;
            this.mstrBranch = inBranch;
            this.mstrWHouse = inWHouse;
            this.txtLot.Text = inLot;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtFrQcWHouse.Tag = inWHouse;
            pobjSQLUtil.SetPara(new object[1] { inWHouse });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcCode, fcName from " + MapTable.Table.WHouse + " where fcSkid = ?", ref strErrorMsg))
            {
                this.txtFrQcWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtFrQnWHouse.Text = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString().TrimEnd();
            }

            this.pmSetStkBal();
        }

        private void pmSetStkBal()
        {
            if (this.ofrmParent != null)
            {
                //decimal decStkQty = this.ofrmParent.GetStockBalance(this.mstrProdID, this.mstrWHouse, this.txtLot.Text.TrimEnd());
                decimal decStkQty = this.ofrmParent.GetStockBalance(this.mstrProdID, this.txtFrQcWHouse.Tag.ToString(), this.txtLot.Text.TrimEnd());
                this.txtStkBal.Text = decStkQty.ToString("#,###,###,##0.00");
            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "WHOUSE":
                    if (this.pofrmGetWareHouse == null)
                    {
                        this.pofrmGetWareHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWareHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWareHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "LOT":
                    if (this.podlgGetLot == null)
                    {
                        this.podlgGetLot = new dlgSelLot();
                        this.podlgGetLot.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.podlgGetLot.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

            }
        }


        private void pmRetrievePopUpVal(string inPopupForm)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "LOT":
                    string strLot = this.podlgGetLot.RetrieveValue();

                    this.txtLot.Text = strLot;

                    this.pmSetStkBal();
                    break;
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    if (this.pofrmGetWareHouse != null)
                    {
                        DataRow dtrGetVal = this.pofrmGetWareHouse.RetrieveValue();
                        if (dtrGetVal != null)
                        {
                            if (this.txtFrQcWHouse.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                            {
                            }
                            this.txtFrQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtFrQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                            this.txtFrQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtFrQcWHouse.Tag = "";
                            this.txtFrQcWHouse.Text = "";
                            this.txtFrQnWHouse.Text = "";
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTLOT":
                    this.pmInitPopUpDialog("LOT");
                    if (this.ofrmParent != null)
                    {
                        this.ofrmParent.QueryTemLot(this.mstrProdID, this.mstrWHouse);
                        this.podlgGetLot.BindData(this.dtsDataEnv, StockAgent.xd_Alias_TemLot);
                    }

                    this.podlgGetLot.ShowDialog();
                    if (this.podlgGetLot.PopUpResult)
                        this.pmRetrievePopUpVal("LOT");

                    break;
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    this.pmInitPopUpDialog("WHOUSE");
                    string strPrefix = ("TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(inTextbox) > -1 ? "FCCODE" : "FCNAME");
                    this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL) + ")", "", strPrefix, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void txtLot_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.pmSetStkBal();
        }

        private void txtFrQcWHouse_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "TXTFRQCWHOUSE,TXTTOQCWHOUSE".IndexOf(txtPopUp.Name.ToUpper()) > -1 ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtFrQcWHouse.Tag = "";
                this.txtFrQcWHouse.Text = "";
                this.txtFrQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WHOUSE");
                e.Cancel = !this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(SysDef.gc_WHOUSE_TYPE_NORMAL) + ")", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWareHouse.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private string pmSplitToSQLStr(string inStr)
        {
            string strResult = "";
            string[] staSQL = inStr.Split(",".ToCharArray());
            if (staSQL.Length > 0)
            {
                for (int intCnt = 0; intCnt < staSQL.Length; intCnt++)
                {
                    strResult += "'" + staSQL[intCnt] + (intCnt == staSQL.Length - 1 ? "'" : "',");
                }
            }
            return strResult;
        }

    }
}
