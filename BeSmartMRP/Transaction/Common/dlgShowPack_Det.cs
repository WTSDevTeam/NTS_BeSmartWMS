
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;


namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgShowPack_Det : UIHelper.frmBase
    {

        public dlgShowPack_Det()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private string mstrRefType = "SO";
        private string mstrBranch = "";
        private string mstrCoor = "";
        private string mstrProd = "";
        private string mstrReferKey = "";
        private string mstrTemPd = "dlgShowPack_TemPd";

        private bool mbllRecalTotPd = false;
        private decimal mdecCurrQty = 0;

        private UIHelper.IfrmDBBase pofrmGetWHPack = null;
        
        private DataSet dtsDataEnv = new DataSet();

        private string mstrQcProd = "";
        private decimal mdecPackSize = 0;

        public void BindData(string inQcProd,string inQcWHPack, string inQnWHPack, decimal inPackSize, DataSet inDataSet, DataTable inSource)
        {

            this.mstrQcProd = inQcProd;
            this.mdecPackSize = inPackSize;

            this.txtQcWHPack.Text = inQcWHPack;
            this.txtQnWHPack.Text = inQnWHPack;
            this.txtSumQty.Text = this.mdecPackSize.ToString("#,###,###.00");

            dtsDataEnv = inDataSet;
            
            this.dtsDataEnv.Tables["TemRefTo_Pack_Det"].Rows.Clear();
            this.pmInitGridProp();

            DataRow[] da1 = this.dtsDataEnv.Tables["TemPd"].Select("cQcProd = '" + this.mstrQcProd + "'", "cLot");
            if (da1.Length > 0)
            {

                decimal decCurrPackQty = this.mdecPackSize;
                decimal decCutQty = 0;
                int j = 1;

                for (int i = 0; i < da1.Length; i++)
                {
                    decimal decCurQty = Convert.ToDecimal(da1[i]["nQty"]);

                    while (decCurQty > 0)
                    {
                        if (decCurQty >= decCurrPackQty)
                        {
                            decCutQty = decCurrPackQty;
                        }
                        else
                        {
                            decCutQty = decCurQty;
                        }

                        DataRow dtrNewPack = this.dtsDataEnv.Tables["TemRefTo_Pack_Det"].NewRow();

                        decCurQty = decCurQty - decCutQty;
                        decCurrPackQty = decCurrPackQty - decCutQty;

                        dtrNewPack["cGUID"] = da1[i]["cGUID"].ToString();
                        //dtrNewPack["fcRefProd"] = da1[i]["fcRefProd"].ToString();
                        dtrNewPack["cLot"] = da1[i]["cLot"].ToString();
                        dtrNewPack["nQty"] = Convert.ToDecimal(da1[i]["nQty"]);
                        dtrNewPack["nPackQty"] = decCutQty;
                        dtrNewPack["cSeq"] = (i + 1).ToString("000");
                        dtrNewPack["cPack_Seq"] = j.ToString("000");

                        string strPackCode = this.txtQcWHPack.Text.Trim() + "-" + dtrNewPack["cLot"].ToString().TrimEnd() + "-" + dtrNewPack["cPack_Seq"].ToString().TrimEnd();
                        dtrNewPack["cPackCode"] = strPackCode;

                        if (decCurrPackQty == 0)
                        {
                            j++;
                            decCurrPackQty = this.mdecPackSize;
                        }

                        this.dtsDataEnv.Tables["TemRefTo_Pack_Det"].Rows.Add(dtrNewPack);
                    }
                    //decSumQty += Convert.ToDecimal(da1[i]["nQty"]);
                    //insert Pack detail
                }
            }

            //foreach (DataRow dtrTemSO in inSource.Rows)
            //{
            //    DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cParent_GUID = '" + dtrTemSO["cGUID"].ToString() + "'");
            //    if (da.Length == 0)
            //    {
            //        //DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
            //        //dtrNewSO["cParent_GUID"] = dtrTemSO["cGUID"].ToString();
            //        //this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewSO);
            //    }

            //}
        }

        private void pmInitForm()
        {

            //this.pmMapEvent();
            //this.pmSetFormUI();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            UIBase.SetDefaultChildAppreance(this);
            //this.pmCreateTem();
        }
        private void pmInitGridProp()
        {

            this.grdTemPack.DataSource = this.dtsDataEnv.Tables["TemRefTo_Pack_Det"];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                //this.gridView1.Columns[intCnt].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
                //this.gridView1.Columns[intCnt].OptionsColumn.AllowFocus = false;
                //this.gridView1.Columns[intCnt].OptionsColumn.ReadOnly = true;

            }

            int i = 0;

            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cPackCode"].VisibleIndex = i++;
            this.gridView1.Columns["cLot"].VisibleIndex = i++;
            this.gridView1.Columns["nQty"].VisibleIndex = i++;
            this.gridView1.Columns["nPackQty"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = UIBase.GetAppUIText(new string[] { "No.", "No." });
            this.gridView1.Columns["cPackCode"].Caption = UIBase.GetAppUIText(new string[] { "Storage Code", "Storage Code" });
            this.gridView1.Columns["cLot"].Caption = UIBase.GetAppUIText(new string[] { "Lot", "Lot" });
            this.gridView1.Columns["nQty"].Caption = UIBase.GetAppUIText(new string[] { "Qty", "Qty" });
            this.gridView1.Columns["nPackQty"].Caption = UIBase.GetAppUIText(new string[] { "Storage Qty", "Storage Qty" });

            this.gridView1.Columns["nRecNo"].Width = 35;
            this.gridView1.Columns["cPackCode"].Width = 120;
            this.gridView1.Columns["cLot"].Width = 85;
            this.gridView1.Columns["nQty"].Width = 85;
            this.gridView1.Columns["nPackQty"].Width = 85;

            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["nPackQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPackQty"].DisplayFormat.FormatString = "#,###,###.00";

            //this.pmCalcColWidth();
            this.grdTemPack.Focus();
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    break;
            }

        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "OK":
                        this.DialogResult = DialogResult.OK;
                        break;
                    case "EXIT":
                        this.DialogResult = DialogResult.Cancel;
                        break;
                }

            }
        }

    }
}
