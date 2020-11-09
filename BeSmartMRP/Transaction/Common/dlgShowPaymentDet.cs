
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
    public partial class dlgShowPaymentDet : UIHelper.frmBase
    {
        public dlgShowPaymentDet()
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
            this.pmInitGridProp2();
            //this.gridView2.MoveLast();
            //this.gridView2.FocusedRowHandle = this.gridView2.GetRowHandle(this.gridView2.RowCount - 1);
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

            this.gridView1.Columns["cType"].Group();

            this.gridView1.Columns["cType"].VisibleIndex = 0;
            this.gridView1.Columns["dDate"].VisibleIndex = 1;
            this.gridView1.Columns["cQcBank"].VisibleIndex = 2;
            this.gridView1.Columns["cCode"].VisibleIndex = 3;
            this.gridView1.Columns["nAmt"].VisibleIndex = 4;

            this.gridView1.Columns["cType"].Visible = true;
            this.gridView1.Columns["dDate"].Visible = true;
            this.gridView1.Columns["cQcBank"].Visible = true;
            this.gridView1.Columns["cCode"].Visible = true;
            this.gridView1.Columns["nAmt"].Visible = true;

            this.gridView1.Columns["cType"].Caption = "";
            this.gridView1.Columns["dDate"].Caption = "วันที่ในเช็ค";
            this.gridView1.Columns["cQcBank"].Caption = "ธนาคาร";
            this.gridView1.Columns["cCode"].Caption = "เลขที่เช็ค";
            this.gridView1.Columns["nAmt"].Caption = "จำนวนเงิน";

            this.gridView1.Columns["cType"].Width = 60;
            this.gridView1.Columns["dDate"].Width = 40;
            this.gridView1.Columns["cQcBank"].Width = 60;
            this.gridView1.Columns["cCode"].Width = 60;
            this.gridView1.Columns["nAmt"].Width = 50;

            this.gridView1.Columns["nAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nAmt"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView1.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;

            this.gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "nAmt", this.gridView1.Columns["nAmt"], "{0:n2}");
            
            this.grdBrowView.Focus();

            this.gridView1.ExpandAllGroups();
        
        }

        private void pmInitGridProp2()
        {
            //this.grdBrowView.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias], "");
            //this.grdBrowView.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables["TemCredit"].DefaultView;
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowCredit.DataSource = this.dtsDataEnv.Tables["TemCredit"];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            this.gridView2.Columns["cYrMth"].VisibleIndex = 0;
            this.gridView2.Columns["cQcCoor"].VisibleIndex = 1;
            this.gridView2.Columns["cQnCoor"].VisibleIndex = 2;
            this.gridView2.Columns["NBUYSELL"].VisibleIndex = 3;
            this.gridView2.Columns["NPAYAMT"].VisibleIndex = 4;
            this.gridView2.Columns["NCRNOTE"].VisibleIndex = 5;
            this.gridView2.Columns["NDRNOTE"].VisibleIndex = 6;
            this.gridView2.Columns["NBALAMT2"].VisibleIndex = 7;

            //this.gridView2.Columns["cYrMth"].Visible = true;
            //this.gridView2.Columns["cQcCoor"].Visible = true;
            //this.gridView2.Columns["cQnCoor"].Visible = true;
            //this.gridView2.Columns["NBUYSELL"].Visible = true;
            //this.gridView2.Columns["NPAYAMT"].Visible = true;
            //this.gridView2.Columns["NCRNOTE"].Visible = true;
            //this.gridView2.Columns["NDRNOTE"].Visible = true;
            //this.gridView2.Columns["NBALAMT"].Visible = true;

            this.gridView2.Columns["cYrMth"].Caption = "ปี/เดือน";
            this.gridView2.Columns["cQcCoor"].Caption = "รหัสลูกค้า";
            this.gridView2.Columns["cQnCoor"].Caption = "ชื่อลูกค้า";
            this.gridView2.Columns["NBUYSELL"].Caption = "ขาย";
            this.gridView2.Columns["NPAYAMT"].Caption = "รับชำระ";
            this.gridView2.Columns["NCRNOTE"].Caption = "ลดหนี้";
            this.gridView2.Columns["NDRNOTE"].Caption = "เพิ่มหนี้";
            this.gridView2.Columns["NBALAMT2"].Caption = "คงเหลือ";

            this.gridView2.Columns["NBUYSELL"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["NBUYSELL"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["NPAYAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["NPAYAMT"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["NCRNOTE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["NCRNOTE"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["NDRNOTE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["NDRNOTE"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["NBALAMT2"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["NBALAMT2"].DisplayFormat.FormatString = "#,###,###.00";

            //this.gridView2.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView2.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            ////this.gridView2.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView2.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView2.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView2.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            //////this.gridView2.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;

            //this.gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "nAmt", this.gridView2.Columns["nAmt"], "{0:n2}");

            //this.grdBrowView.Focus();

            //this.gridView2.ExpandAllGroups();
            //this.gridView2.MoveFirst();
            //this.gridView2.MoveLast();
            //this.gridView2.FocusedRowHandle = this.gridView2.RowCount;

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

        private void dlgShowPaymentDet_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void dlgShowPaymentDet_Load(object sender, EventArgs e)
        {
        }

        private void dlgShowPaymentDet_Shown(object sender, EventArgs e)
        {

            this.grdBrowCredit.RefreshDataSource();
            this.grdBrowCredit.ForceInitialize();
            this.gridView2.MoveLastVisible();

            this.grdBrowView.RefreshDataSource();
            this.grdBrowView.ForceInitialize();
            this.gridView1.MoveLastVisible();
        
        }

    }
}