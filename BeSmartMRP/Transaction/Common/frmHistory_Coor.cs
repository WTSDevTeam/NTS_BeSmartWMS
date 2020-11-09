
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
    public partial class frmHistory_Coor : UIHelper.frmBase
    {
        public frmHistory_Coor()
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
            dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["dDate"].VisibleIndex = 0;
            this.gridView1.Columns["cRefNo"].VisibleIndex = 1;
            this.gridView1.Columns["cQcProd"].VisibleIndex = 2;
            this.gridView1.Columns["cQnProd"].VisibleIndex = 3;
            this.gridView1.Columns["cLot"].VisibleIndex = 4;
            this.gridView1.Columns["nQty"].VisibleIndex = 5;
            this.gridView1.Columns["cQnUM"].VisibleIndex = 6;
            this.gridView1.Columns["nPrice"].VisibleIndex = 7;
            this.gridView1.Columns["cDiscStr"].VisibleIndex = 8;

            this.gridView1.Columns["dDate"].Visible = true;
            this.gridView1.Columns["cRefNo"].Visible = true;
            this.gridView1.Columns["cQcProd"].Visible = true;
            this.gridView1.Columns["cQnProd"].Visible = true;
            this.gridView1.Columns["nQty"].Visible = true;
            this.gridView1.Columns["cQnUM"].Visible = true;
            this.gridView1.Columns["nPrice"].Visible = true;
            this.gridView1.Columns["cDiscStr"].Visible = true;
            this.gridView1.Columns["cLot"].Visible = true;

            this.gridView1.Columns["dDate"].Caption = "วันที่";
            this.gridView1.Columns["cRefNo"].Caption = "เลขที่อ้างอิง";
            this.gridView1.Columns["cQcProd"].Caption = "รหัสสินค้า";
            this.gridView1.Columns["cQnProd"].Caption = "ชื่อสินค้า";
            this.gridView1.Columns["nQty"].Caption = "จำนวน";
            this.gridView1.Columns["cQnUM"].Caption = "หน่วยนับ";
            this.gridView1.Columns["nPrice"].Caption = "ราคา";
            this.gridView1.Columns["cDiscStr"].Caption = "ส่วนลด";
            this.gridView1.Columns["cLot"].Caption = "ล๊อต";

            this.gridView1.Columns["dDate"].Width = 40;
            this.gridView1.Columns["cRefNo"].Width = 60;
            this.gridView1.Columns["cQcProd"].Width = 60;
            this.gridView1.Columns["nQty"].Width = 50;
            this.gridView1.Columns["cQnUM"].Width = 60;
            this.gridView1.Columns["nPrice"].Width = 50;
            this.gridView1.Columns["cDiscStr"].Width = 30;
            this.gridView1.Columns["cLot"].Width = 30;

            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPrice"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.gridView1.Columns["dDate"].FormatString = "dd/MM/yy";
            //this.gridView1.Columns["nQty"].FormatString = "##,###,###.00";
            //this.gridView1.Columns["nPrice"].FormatString = "##,###,###.00";

            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView1.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;
            this.grdBrowView.Focus();
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

        private void frmHistory_Coor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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