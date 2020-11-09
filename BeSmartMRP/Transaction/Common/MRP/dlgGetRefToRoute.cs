
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

namespace BeSmartMRP.Transaction.Common.MRP
{
    public partial class dlgGetRefToRoute : UIHelper.frmBase
    {

        public dlgGetRefToRoute()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public static int MAXLENGTH_CODE = 20;
        
        private bool mbllPopUpResult = false;
        private string mstrAlias = "TemRoute";
        private DataSet dtsDataEnv = null;
        private string mstrSortKey = "CFROPSEQ";

        public void BindData(DataSet inDataEnv)
        {
            this.dtsDataEnv = inDataEnv;
            this.pmBindData();
        }

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        private void pmInitForm()
        {
            this.pmInitializeComponent();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetRefToRoute_KeyDown);

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
            this.gridView1.Columns["cFrOPSeq"].VisibleIndex = i++;
            this.gridView1.Columns["cQcFrMOPR"].VisibleIndex = i++;
            this.gridView1.Columns["cQnFrMOPR"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnFrWkCtr"].VisibleIndex = i++;
            this.gridView1.Columns["cToOPSeq"].VisibleIndex = i++;
            this.gridView1.Columns["cQnToMOPR"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnToWkCtr"].VisibleIndex = i++;

            this.gridView1.Columns["cFrOPSeq"].Caption = UIBase.GetAppUIText(new string[] { "OP Seq.", "OP Seq." });
            this.gridView1.Columns["cQcFrMOPR"].Caption = UIBase.GetAppUIText(new string[] { "รหัส OPR", "OPR Code" });
            this.gridView1.Columns["cQnFrMOPR"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อ OPR", "OPR Name" });
            this.gridView1.Columns["cQnFrWkCtr"].Caption = UIBase.GetAppUIText(new string[] { "โอนจาก W/C", "From W/C" });
            this.gridView1.Columns["cToOPSeq"].Caption = UIBase.GetAppUIText(new string[] { "Next OP.", "Next OP.." });
            this.gridView1.Columns["cQnToMOPR"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อ OPR", "OPR Name" });
            this.gridView1.Columns["cQnToWkCtr"].Caption = UIBase.GetAppUIText(new string[] { "โอนเข้า W/C", "To W/C" });

            //this.gridView1.Columns["cType"].Width = 60;
            //this.gridView1.Columns["dDate"].Width = 40;
            //this.gridView1.Columns["cQcBank"].Width = 60;
            //this.gridView1.Columns["cCode"].Width = 60;
            //this.gridView1.Columns["nAmt"].Width = 50;

            //this.gridView1.Columns["nAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["nAmt"].DisplayFormat.FormatString = "#,###,###.00";

            //this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.grdBrowView.Focus();

        }

        public int SelectedRow()
        {
            return this.gridView1.FocusedRowHandle;
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                switch (oToolButton)
                {
                    case WsToolBar.Enter:
                        this.mbllPopUpResult = true;
                        this.pmEnterForm();
                        break;
                    case WsToolBar.Exit:
                        //this.DialogResult = DialogResult.Cancel;
                        this.mbllPopUpResult = false;
                        this.DialogResult = DialogResult.Cancel;
                        //this.Hide();
                        break;
                    case WsToolBar.Refresh:
                        this.pmRefreshBrowView();
                        break;
                }

            }
        }

        private void pmRefreshBrowView()
        {
            //this.pmSetBrowView();
            //this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            //if (this.gridView1.RowCount > 0)
            //    this.gridView1.FocusedRowHandle = 0;
        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            this.mstrSortKey = "cFrOPSeq";
            inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);

            //if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false)
            //{
            //    string strSearch = inSearchStr.TrimEnd();
            //    if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
            //        strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
            //    {
            //        strSearch = "";
            //    }

            //    this.mstrSearchStr = "%" + strSearch + "%";
            //    //this.pmRefreshBrowView();
            //}

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            //this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                this.ShowDialog();
                //this.mbllIsShowDialog = true;
            }
            else
            {
                this.gridView1.FocusedRowHandle = intSeekVal;
                this.mbllPopUpResult = true;
            }
            return this.mbllPopUpResult;
        }

        private void dlgGetRefToRoute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.pmEnterForm();
                    break;
                case Keys.Escape:
                    this.mbllPopUpResult = false;
                    this.DialogResult = DialogResult.Cancel;
                    //this.Hide();
                    break;
            }

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void pmEnterForm()
        {
            this.mbllPopUpResult = true;
            this.DialogResult = DialogResult.OK;
            //this.Hide();
        }

    }
}
