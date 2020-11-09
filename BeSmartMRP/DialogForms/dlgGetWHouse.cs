
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
    public partial class dlgGetWHouse : UIHelper.frmBase, IfrmDBBase
    {
 
        //public static int this.mint_MAXLENGTH_CODE = 6;
        public static int MAXLENGTH_CODE = 10;
        public static int MAXLENGTH_NAME = 30;

        private int mint_MAXLENGTH_CODE = 10;
        private int mint_MAXLENGTH_NAME = 30;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = "WHOUSE";
        private string mstrSortKey = "FCCODE";

        private string mstrBranch = "";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private string mstrFixType = SysDef.gc_WHOUSE_TYPE_NORMAL;
        private string mstrTypeList = "";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetWHouse()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            this.pmSetBrowView();

            this.pmInitGridProp();
            //this.gridView1.MoveLast();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.mint_MAXLENGTH_CODE = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, "FCCODE");
            this.mint_MAXLENGTH_NAME = MapTable.GetMaxLength(pobjSQLUtil, this.mstrRefTable, "FCNAME");

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetWHouse_KeyDown);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec = "select WHOUSE.FCSKID, WHOUSE.FCCODE, WHOUSE.FCNAME from WHOUSE where FCCORP = ? and FCBRANCH = ? " + (this.mstrFixType.Trim() != string.Empty ? "and FCTYPE in " + this.mstrFixType : "");
            object[] pAPara = new object[] { App.gcCorp, this.mstrBranch };
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

            this.gridView1.Columns["FCCODE"].Visible = true;
            this.gridView1.Columns["FCNAME"].Visible = true;

            this.gridView1.Columns["FCCODE"].Caption = "���ʤ�ѧ�Թ���";
            this.gridView1.Columns["FCNAME"].Caption = "���ͤ�ѧ�Թ���";

            this.gridView1.Columns["FCCODE"].Width = 15;

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

        private void dlgGetWHouse_KeyDown(object sender, KeyEventArgs e)
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
            //if (this.gridView1.RowCount > 0)
            //    this.gridView1.FocusedRowHandle = 0;
        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.RefreshData();
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
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_CODE);
            else
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_NAME);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr
                || this.mbllIsFormQuery == false)
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

        public bool ValidateField(string inBranch, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "FCCODE")
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_CODE);
            else
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_NAME);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr 
                || this.mstrBranch != inBranch
                || this.mbllIsFormQuery == false)
            {
                this.mstrBranch = inBranch;
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

        public bool ValidateField(string inBranch, string inType, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "FCCODE")
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_CODE);
            else
                inSearchStr = inSearchStr.PadRight(this.mint_MAXLENGTH_NAME);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr
                || this.mstrBranch != inBranch
                || this.mstrFixType != inType
                || this.mbllIsFormQuery == false)
            {
                this.mstrBranch = inBranch;
                this.mstrFixType = inType;
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
