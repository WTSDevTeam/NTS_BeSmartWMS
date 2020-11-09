
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.Transaction.Common
{
    public partial class frmHistory_Prod : UIHelper.frmBase
    {

        public frmHistory_Prod(string inSaleOrBuy)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.mstrSaleOrBuy = inSaleOrBuy;
            this.pmInitForm();
        }
        
        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrAlias2 = "";
        private string mstrAlias3 = "";
        private string mstrSortKey = "";

        private string mstrSaleOrBuy = "S";

        private void pmInitForm()
        {
        }

        public void BindData(DataSet inDataEnv, string inAlias, string inAlias2, string inAlias3)
        {
            this.mstrAlias = inAlias;
            this.mstrAlias2 = inAlias2;
            this.mstrAlias3 = inAlias3;
            this.dtsDataEnv = inDataEnv;
            this.pmBindData();
        }

        private void pmBindData()
        {
            this.pmInitGridProp();
            this.pmInitGridProp2();
            this.pmInitGridProp3();
        }

        private void pmInitGridProp()
        {
            //this.grdBrowView.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias], "");
            //this.grdBrowView.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias].DefaultView;
            dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            this.gridView2.Columns["dDate"].VisibleIndex = 0;
            this.gridView2.Columns["cRefNo"].VisibleIndex = 1;
            this.gridView2.Columns["cLot"].VisibleIndex = 2;
            this.gridView2.Columns["nQty"].VisibleIndex = 3;
            this.gridView2.Columns["cQnUM"].VisibleIndex = 4;
            this.gridView2.Columns["nPrice"].VisibleIndex = 5;
            this.gridView2.Columns["cDiscStr"].VisibleIndex = 6;
            this.gridView2.Columns["cQnCoor"].VisibleIndex = 7;

            this.gridView2.Columns["dDate"].Visible = true;
            this.gridView2.Columns["cRefNo"].Visible = true;
            this.gridView2.Columns["cLot"].Visible = true;
            this.gridView2.Columns["nQty"].Visible = true;
            this.gridView2.Columns["cQnUM"].Visible = true;
            this.gridView2.Columns["nPrice"].Visible = true;
            this.gridView2.Columns["cDiscStr"].Visible = true;
            this.gridView2.Columns["cQnCoor"].Visible = true;

            this.gridView2.Columns["dDate"].Caption = "วันที่";
            this.gridView2.Columns["cRefNo"].Caption = "เลขที่อ้างอิง";
            this.gridView2.Columns["nQty"].Caption = "จำนวน";
            this.gridView2.Columns["cQnUM"].Caption = "หน่วยนับ";
            this.gridView2.Columns["nPrice"].Caption = "ราคา";
            this.gridView2.Columns["cDiscStr"].Caption = "ส่วนลด";
            this.gridView2.Columns["cLot"].Caption = "ล๊อต";
            this.gridView2.Columns["cQnCoor"].Caption = (this.mstrSaleOrBuy == "S" ? "ชื่อลูกค้า" : "ชื่อผู้จำหน่าย");

            //this.grdBrowView.ColumnAutoResize = true;
            this.gridView2.Columns["dDate"].Width = 40;
            this.gridView2.Columns["cRefNo"].Width = 65;
            this.gridView2.Columns["nQty"].Width = 50;
            this.gridView2.Columns["cQnUM"].Width = 60;
            this.gridView2.Columns["nPrice"].Width = 50;
            this.gridView2.Columns["cDiscStr"].Width = 35;
            this.gridView2.Columns["cLot"].Width = 30;

            this.gridView2.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nPrice"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.grdBrowView.RootTable.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView2.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.grdBrowView.RootTable.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView2.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView2.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;
            this.gridView2.Focus();
        }

        private void pmInitGridProp2()
        {
            //this.grdBakRev.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias2], "");
            //this.grdBakRev.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias2].DefaultView;
            dvBrow.Sort = "dDate, cRefNo";

            this.grdBakRev.DataSource = this.dtsDataEnv.Tables[this.mstrAlias2];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["cQnCoor"].VisibleIndex = 0;
            this.gridView1.Columns["cRefNo"].VisibleIndex = 1;
            this.gridView1.Columns["dDate"].VisibleIndex = 2;
            this.gridView1.Columns["cLot"].VisibleIndex = 3;
            this.gridView1.Columns["cQnUM"].VisibleIndex = 4;
            this.gridView1.Columns["nQty"].VisibleIndex = 5;
            this.gridView1.Columns["dDueDate"].VisibleIndex = 6;

            this.gridView1.Columns["cQnCoor"].Visible = true;
            this.gridView1.Columns["cRefNo"].Visible = true;
            this.gridView1.Columns["dDate"].Visible = true;
            this.gridView1.Columns["cLot"].Visible = true;
            this.gridView1.Columns["cQnUM"].Visible = true;
            this.gridView1.Columns["nQty"].Visible = true;
            this.gridView1.Columns["dDueDate"].Visible = true;

            this.gridView1.Columns["cQnCoor"].Caption = (this.mstrSaleOrBuy == "S" ? "ชื่อลูกค้า" : "ชื่อผู้จำหน่าย");
            this.gridView1.Columns["cRefNo"].Caption = "เลขที่ภายใน";
            this.gridView1.Columns["dDate"].Caption = "วันที่";
            this.gridView1.Columns["cLot"].Caption = "ล็อต";
            this.gridView1.Columns["nQty"].Caption = "จำนวนค้าง" + (this.mstrSaleOrBuy == "S" ? "ส่ง" : "รับ");
            this.gridView1.Columns["cQnUM"].Caption = "หน่วยนับ";
            this.gridView1.Columns["dDueDate"].Caption = "วันที่ส่งของ";

            this.gridView1.Columns["cRefNo"].Width = 65;
            this.gridView1.Columns["dDate"].Width = 40;
            this.gridView1.Columns["cLot"].Width = 35;
            this.gridView1.Columns["nQty"].Width = 50;
            this.gridView1.Columns["cQnUM"].Width = 60;
            this.gridView1.Columns["dDueDate"].Width = 40;

            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["dDueDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDueDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.grdBakRev.RootTable.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.grdBakRev.RootTable.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));

        }

        private void pmInitGridProp3()
        {
            //this.grdWHBal.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias3], "");
            //this.grdWHBal.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias3].DefaultView;
            //dvBrow.Sort = "FCCODE";

            this.grdWHBal.DataSource = this.dtsDataEnv.Tables[this.mstrAlias3];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            this.gridView3.Columns["cQnWHouse"].VisibleIndex = 0;
            this.gridView3.Columns["nQty"].VisibleIndex = 1;
            this.gridView3.Columns["cQnUM"].VisibleIndex = 2;
            this.gridView3.Columns["nMinStock"].VisibleIndex = 3;
            this.gridView3.Columns["nMaxStock"].VisibleIndex = 4;

            this.gridView3.Columns["cQnWHouse"].Visible = true;
            this.gridView3.Columns["cQnUM"].Visible = true;
            this.gridView3.Columns["nQty"].Visible = true;
            this.gridView3.Columns["nMinStock"].Visible = true;
            this.gridView3.Columns["nMaxStock"].Visible = true;

            this.gridView3.Columns["cQnWHouse"].Caption = "คลังสินค้า";
            this.gridView3.Columns["cQnUM"].Caption = "หน่วยนับ";
            this.gridView3.Columns["nQty"].Caption = "จำนวน";
            this.gridView3.Columns["nMinStock"].Caption = "Minimum Stock";
            this.gridView3.Columns["nMaxStock"].Caption = "Maximum Stock";

            this.gridView3.Columns["cQnUM"].Width = 50;
            this.gridView3.Columns["nQty"].Width = 50;
            this.gridView3.Columns["nMinStock"].Width = 50;
            this.gridView3.Columns["nMaxStock"].Width = 50;

            this.gridView3.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView3.Columns["nMinStock"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nMinStock"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView3.Columns["nMaxStock"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nMaxStock"].DisplayFormat.FormatString = "#,###,###.00";

        }

        private void frmHistory_Prod_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

    }
}