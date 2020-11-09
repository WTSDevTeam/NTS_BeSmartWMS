
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgSelLot : UIHelper.frmBase
    {
 
        public dlgSelLot()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;
        private int mintFocusRow = -1;

        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

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
            dvBrow.Sort = "cLot";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["cLot"].VisibleIndex = 0;
            this.gridView1.Columns["nQty"].VisibleIndex = 1;

            this.gridView1.Columns["cLot"].Caption = "Lot";
            this.gridView1.Columns["nQty"].Caption = "Qty";

            this.gridView1.Columns["nQty"].Width = 80;

            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = "#,###,###.00";

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

        private void dlgSelLot_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DialogResult = DialogResult.OK;
                    break;
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void grdBrowView_SortKeysChanged(object sender, System.EventArgs e)
        {
        }

        public string RetrieveValue()
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null) return "";

            return dtrBrow["cLot"].ToString();
        }
    
    }
}