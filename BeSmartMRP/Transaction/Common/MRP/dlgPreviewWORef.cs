
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Report;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common.MRP
{

    public partial class dlgPreviewWORef : UIHelper.frmBase
    {

        public dlgPreviewWORef()
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

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias].DefaultView;
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["cRefType"].Group();

            int i = 0;
            this.gridView1.Columns["cRefType"].VisibleIndex = i++;
            this.gridView1.Columns["dDate"].VisibleIndex = i++;
            this.gridView1.Columns["cCode"].VisibleIndex = i++;
            this.gridView1.Columns["cRefNo"].VisibleIndex = i++;

            this.gridView1.Columns["cRefType"].Caption = " ";
            this.gridView1.Columns["dDate"].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "DOC. DATE" });
            this.gridView1.Columns["cCode"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "DOC. CODE" });
            this.gridView1.Columns["cRefNo"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "REF. CODE" });

            this.gridView1.Columns["cRefType"].Width = 60;
            this.gridView1.Columns["dDate"].Width = 40;
            this.gridView1.Columns["cCode"].Width = 60;
            this.gridView1.Columns["cRefNo"].Width = 50;

            this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

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
                case "ENTER":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "EXIT":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgPreviewWORef_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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