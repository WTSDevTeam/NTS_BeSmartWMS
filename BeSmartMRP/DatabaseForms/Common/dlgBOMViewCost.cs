using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.UIHelper;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgBOMViewCost : UIHelper.frmBase
    {

        public dlgBOMViewCost()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";
        private bool mbllShowStock = false;

        private void pmInitForm()
        {
        }

        public void BindData(DataSet inDataEnv, string inAlias, bool inShowStock)
        {
            this.mstrAlias = inAlias;
            this.dtsDataEnv = inDataEnv;
            this.mbllShowStock = inShowStock;
            this.pmBindData();
        }

        private void pmBindData()
        {
            this.pmInitGridProp();
        }

        private void pmInitGridProp()
        {
            //this.grdBrowView.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias], "");
            //this.grdBrowView.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias].DefaultView;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cOPSeq"].VisibleIndex = i++;
            this.gridView1.Columns["cPdType"].VisibleIndex = i++;
            this.gridView1.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView1.Columns["cQnProd"].VisibleIndex = i++;
            //this.gridView1.Columns["cRemark1"].VisibleIndex = i++;
            if (this.mbllShowStock)
            {
                this.gridView1.Columns["nBalQty"].VisibleIndex = i++;
            }
            this.gridView1.Columns["nQty"].VisibleIndex = i++;
            this.gridView1.Columns["nCost"].VisibleIndex = i++;
            this.gridView1.Columns["nAmt"].VisibleIndex = i++;
            this.gridView1.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView1.Columns["cProcure"].VisibleIndex = i++;
            this.gridView1.Columns["cScrap"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = "No.";
            this.gridView1.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView1.Columns["cPdType"].Caption = "T";
            this.gridView1.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView1.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView1.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView1.Columns["nBalQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "คงเหลือ", "Stock" });
            this.gridView1.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView1.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView1.Columns["nCost"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ต้นทุน", "Cost" });
            this.gridView1.Columns["nAmt"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "มูลค่า", "Amt." });
            this.gridView1.Columns["cScrap"].Caption = "Scrap";
            //this.gridView1.Columns["IsMRP"].Caption = "MRP";
            //this.gridView1.Columns["IsCost"].Caption = "Cost";
            //this.gridView1.Columns["IsOverHead"].Caption = "Over Hd.";
            //this.gridView1.Columns["IsIssue"].Caption = "Issue";
            this.gridView1.Columns["cProcure"].Caption = "Procure";
            //this.gridView1.Columns["cRoundCtrl"].Caption = "Round";
            //this.gridView1.Columns["cSubSti"].Caption = "S";
            //this.gridView1.Columns["cQcBOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BOM", "BOM" });

            this.gridView1.Columns["nRecNo"].Width = 50;
            this.gridView1.Columns["cOPSeq"].Width = 80;
            this.gridView1.Columns["cPdType"].Width = 50;
            this.gridView1.Columns["cQcProd"].Width = 130;
            this.gridView1.Columns["cRemark1"].Width = 80;
            this.gridView1.Columns["nBalQty"].Width = 80;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["nCost"].Width = 80;
            this.gridView1.Columns["cQnUOM"].Width = 80;
            this.gridView1.Columns["cScrap"].Width = 80;
            this.gridView1.Columns["cProcure"].Width = 60;
            this.gridView1.Columns["cRoundCtrl"].Width = 60;
            //this.gridView1.Columns["cQcBOM"].Width = 130;
            //this.gridView1.Columns["cSubSti"].Width = 5;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.gridView1.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["nBalQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBalQty"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView1.Columns["nQty"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView1.Columns["nQty"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            this.gridView1.Columns["nCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nCost"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView1.Columns["nCost"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView1.Columns["nCost"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;


            this.gridView1.Columns["nAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nAmt"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView1.Columns["nAmt"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView1.Columns["nAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag.ToString())
            {
                case "ENTER":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "EXIT":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgBOMViewCost_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void grdBrowView_SortKeysChanged(object sender, System.EventArgs e)
        {
        }

    }
}