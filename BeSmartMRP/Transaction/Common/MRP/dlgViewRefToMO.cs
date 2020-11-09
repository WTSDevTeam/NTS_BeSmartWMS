
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
using BeSmartMRP.Report;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common.MRP
{
    public partial class dlgViewRefToMO : UIHelper.frmBase
    {
        
        public dlgViewRefToMO()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";

        private void pmInitForm()
        {
        }

        public void BindData(DataSet inDataEnv, string inAlias)
        {
            this.mstrAlias = inAlias;
            this.dtsDataEnv = inDataEnv;
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
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["cCode"].Group();

            this.gridView1.Columns["cCode"].VisibleIndex = i++;
            this.gridView1.Columns["cRefNo"].VisibleIndex = i++;
            this.gridView1.Columns["cQcCoor"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnCoor"].VisibleIndex = i++;
            this.gridView1.Columns["cQcProd"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnProd"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_MO"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_PR"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_PO"].VisibleIndex = i++;
            this.gridView1.Columns["nStkQty"].VisibleIndex = i++;
            this.gridView1.Columns["nIssueQty"].VisibleIndex = i++;
            this.gridView1.Columns["nRefQty"].VisibleIndex = i++;
            this.gridView1.Columns["nQty"].VisibleIndex = i++;
            this.gridView1.Columns["nPrice"].VisibleIndex = i++;

            this.gridView1.Columns["cCode"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "Document Code" });
            this.gridView1.Columns["cRefNo"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "#Ref. No." });
            this.gridView1.Columns["cQcCoor"].Caption = UIBase.GetAppUIText(new string[] { "รหัสผู้จำหน่าย", "Supplier Code" });
            this.gridView1.Columns["cQnCoor"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อผู้จำหน่าย", "Supplier Name" });
            this.gridView1.Columns["cQcProd"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Code" });
            this.gridView1.Columns["cQnProd"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า", "Product Name" });
            this.gridView1.Columns["nBakQty_MO"].Caption = UIBase.GetAppUIText(new string[] { "ยอดจอง MO", "Booking\r\nM/O. Qty." });
            this.gridView1.Columns["nBakQty_PR"].Caption = UIBase.GetAppUIText(new string[] { "PR ค้างสั่ง", "Outstanding\r\nP/R. Qty." });
            this.gridView1.Columns["nBakQty_PO"].Caption = UIBase.GetAppUIText(new string[] { "PO ค้างรับ", "Outstanding\r\nP/O. Qty" });
            this.gridView1.Columns["nStkQty"].Caption = UIBase.GetAppUIText(new string[] { "สินค้าคงหลือ", "Stock\r\nQty." });
            this.gridView1.Columns["nIssueQty"].Caption = UIBase.GetAppUIText(new string[] { "เบิก/รับคืน", "Issue\r\nQty." });
            this.gridView1.Columns["nRefQty"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน", "M/O\r\nQty." });
            this.gridView1.Columns["nQty"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน ญฑ", "Gen P/R\r\nQty." });
            this.gridView1.Columns["nPrice"].Caption = UIBase.GetAppUIText(new string[] { "ราคา", "Price" });


            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["cRefNo"].Width = 120;
            this.gridView1.Columns["cQcCoor"].Width = 90;
            //this.gridView1.Columns["cQnCoor"].Width = 40;
            this.gridView1.Columns["cQcProd"].Width = 100;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["nBakQty_MO"].Width = 80;
            this.gridView1.Columns["nBakQty_PR"].Width = 80;
            this.gridView1.Columns["nStkQty"].Width = 80;
            this.gridView1.Columns["nIssueQty"].Width = 80;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["nRefQty"].Width = 80;
            this.gridView1.Columns["nPrice"].Width = 80;

            string strQtyFormat = "#,###,###.00;(#,###,###.00);-";
            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPrice"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nStkQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nStkQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nIssueQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nIssueQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_MO"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_MO"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_PR"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_PR"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_PO"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_PO"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nPrice"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nStkQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nBakQty_PR"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nBakQty_PO"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nRefQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nIssueQty"].AppearanceCell.BackColor = Color.LemonChiffon;

            //this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView1.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;

            //this.gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "nAmt", this.gridView1.Columns["nAmt"], "{0:n2}");

            this.grdBrowView.Focus();

            this.gridView1.ExpandAllGroups();

        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag.ToString())
            {
                case "CANCEL":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgViewRefToMO_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

    
    }
}
