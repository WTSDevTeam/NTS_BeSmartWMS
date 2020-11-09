
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

namespace BeSmartMRP.DialogForms
{

    public partial class dlgGetCoor : UIHelper.frmBase
    {

        public static int MAXLENGTH_CODE = 20;
        public static int MAXLENGTH_NAME = 70;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = "COOR";
        private string mstrSortKey = "FCCODE";

        //private CoorType mCoorType = CoorType.Customer;
        private CoorType mCoorType = CoorType.Supplier;
        private string mstrCoorMsg = "";
        private string mstrCoorFld = "";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetCoor()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public dlgGetCoor(CoorType inCoorType)
        {
            InitializeComponent();

            this.mCoorType = inCoorType;
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");

            this.pmInitializeComponent();
            this.pmSetBrowView();

            this.pmInitGridProp();
            //this.gridView1.MoveLast();

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetCoor_KeyDown);
            if (this.mCoorType == CoorType.Customer)
            {
                this.mstrCoorMsg = UIBase.GetAppUIText(new string[] { "ลูกค้า", "Customer" });
                this.mstrCoorFld = "FCISCUST";
            }
            else
            {
                this.mstrCoorMsg = UIBase.GetAppUIText(new string[] { "ผู้จำหน่าย", "Supplier" });
                this.mstrCoorFld = "FCISSUPP";
            }
            this.Text = UIBase.GetAppUIText(new string[] { "ระบุ", "Select " }) + this.mstrCoorMsg;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mintMaxLen_CODE = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, "FCCODE");
            this.mintMaxLen_NAME = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, "FCNAME");
            this.mintMaxLen_SNAME = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, "FCSNAME");
        
        }

        private int mintMaxLen_CODE = 20;
        private int mintMaxLen_NAME = 70;
        private int mintMaxLen_SNAME = 70;

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            //object[] pAPara = new object[] { App.gcCorp };
            object[] pAPara = null;
            string strSQLExec = "select COOR.FCSKID, COOR.FCCODE, COOR.FCNAME, COOR.FCSNAME from COOR where FCCORP = ? and " + this.mstrCoorFld + " = 'Y' ";
            if (this.mstrSearchStr != string.Empty)
            {
                strSQLExec += " and " + this.mstrSortKey + " like ? ";
                pAPara = new object[] { App.gcCorp, this.mstrSearchStr };
            }
            else
            {
                strSQLExec += "";
                pAPara = new object[] { App.gcCorp };
            }

            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "FCCODE";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["FCCODE"].VisibleIndex = i++;
            this.gridView1.Columns["FCNAME"].VisibleIndex = i++;

            this.gridView1.Columns["FCCODE"].Caption = UIBase.GetAppUIText(new string[] { "รหัส" + this.mstrCoorMsg, this.mstrCoorMsg + " Code" });
            this.gridView1.Columns["FCNAME"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อ" + this.mstrCoorMsg, this.mstrCoorMsg + " Name" });
            this.gridView1.Columns["FCSNAME"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อย่อ", "Short Name" }); ;

            this.gridView1.Columns["FCCODE"].Width = 15;
            this.gridView1.Columns["FCSNAME"].Width = 30;

            this.pmSetSortKey("FCCODE", true);
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
                        this.Hide();
                        break;
                    case WsToolBar.Refresh:
                        this.pmRefreshBrowView();
                        break;
                }

            }
        }

        private void dlgGetCoor_KeyDown(object sender, KeyEventArgs e)
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
                    this.Hide();
                    break;
            }

        }

        private void pmRefreshBrowView()
        {
            this.pmSetBrowView();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            if (this.gridView1.RowCount > 0)
                this.gridView1.FocusedRowHandle = 0;
        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                //this.gridView1.RefreshData();
            }

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
            }
            this.mstrSortKey = inColumn.ToUpper();
            this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
            this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void pmEnterForm()
        {
            this.mbllPopUpResult = true;
            this.Hide();
        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "FCCODE")
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);
            else
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_NAME);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false)
            {
                string strSearch = inSearchStr.TrimEnd();
                if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
                    strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
                {
                    strSearch = "";
                }

                this.mstrSearchStr = "%" + strSearch + "%";
                this.pmRefreshBrowView();
            }

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                this.ShowDialog();
                this.mbllIsShowDialog = true;
            }
            else
            {
                this.gridView1.FocusedRowHandle = intSeekVal;
                this.mbllPopUpResult = true;
            }
            return this.mbllPopUpResult;
        }

        public DataRow RetrieveValue()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null) return null;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["fcSkid"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }




    }
}
