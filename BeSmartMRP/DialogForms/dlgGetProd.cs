
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
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.DialogForms
{

    public partial class dlgGetProd : UIHelper.frmBase
    {

        //public static int MAXLENGTH_CODE = 20;
        public static int MAXLENGTH_CODE = 40;
        public static int MAXLENGTH_NAME = 70;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = "PROD";
        private string mstrSortKey = "FCCODE";

        private string mstrPdType = "";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;
        private int mintFocusRow = -1;

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetProd()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public dlgGetProd(bool inShowPrice)
        {
            InitializeComponent();
            this.mbllShowPrice = inShowPrice;
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            this.pmSetBrowView(true);

            this.pmInitGridProp();
            //this.gridView1.MoveLast();

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetProd_KeyDown);
            this.grdBrowView.Resize += new EventHandler(grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(gridView1_ColumnWidthChanged);
            this.Resize += new EventHandler(dlgGetProd_Resize);
            this.Shown += new EventHandler(dlgGetProd_Shown);
        }

        private void pmSetBrowView(bool inIsInit)
        {
            UIBase.WaitWind("Loading รายการสินค้า...");
            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind();
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //this.grdBrowView.DataSource = null;
            //this.grdBrowView.RefreshDataSource();
            //this.gridView1.RefreshEditor(true);

            string[] staPara = { this.mstrRefTable, this.mstrSortKey, "%" + this.mstrSearchStr + "%" };
            string strSearchStr = (this.mstrSearchStr != string.Empty ? string.Format(" and {0}.{1} like ?", staPara) : "");

            object[] pAPara = null;
            if (inIsInit)
            {
                strSQLStr = "select fcSkid, fcType, fcCode, fcName, fcSName,fnPrice from Prod where 0=1";
            }
            else
            {
                if (this.mstrPdType != string.Empty)
                {
                    if (this.mstrSearchStr != string.Empty)
                        pAPara = new object[] { App.ActiveCorp.RowID, "%" + this.mstrSearchStr + "%" };
                    else
                        pAPara = new object[] { App.ActiveCorp.RowID };
                    strSQLStr = "select fcSkid, fcType, fcCode, fcName, fcSName,fnPrice from Prod where fcCorp = ? and fcType in (" + this.mstrPdType + ") " + strSearchStr + " and FCSTATUS <> 'I' order by " + this.mstrSortKey;
                }
                else
                {
                    if (this.mstrSearchStr != string.Empty)
                        pAPara = new object[] { App.ActiveCorp.RowID, "%" + this.mstrSearchStr + "%" };
                    else
                        pAPara = new object[] { App.ActiveCorp.RowID };
                    strSQLStr = "select fcSkid, fcType, fcCode, fcName, fcSName,fnPrice from Prod where fcCorp = ? " + strSearchStr + " and FCSTATUS <> 'I' order by " + this.mstrSortKey;
                }
                pobjSQLUtil.SetPara(pAPara);
            }

            //strSQLStr = string.Format("select {0}.fcSkid, {0}.fcCode, {0}.fcName, {0}.fcName2 from {0} " +strSearchStr+ " order by " + " {0}.{1}", staPara);
            if (!pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {

                if (this.mstrPdType != string.Empty)
                {
                    pAPara = new object[] { App.ActiveCorp.RowID };
                    strSQLStr = "select fcSkid, fcType, fcCode, fcName, fcSName,fnPrice from Prod where fcCorp = ? and fcType in (" + this.mstrPdType + ") and FCSTATUS <> 'I' order by " + this.mstrSortKey;
                }
                else
                {
                    pAPara = new object[] { App.ActiveCorp.RowID };
                    strSQLStr = "select fcSkid, fcType, fcCode, fcName, fcSName,fnPrice from Prod where fcCorp = ? and FCSTATUS <> 'I' order by " + this.mstrSortKey;
                }
                pobjSQLUtil.SetPara(pAPara);
                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLStr, ref strErrorMsg);
            }

            //this.gridView1.RefreshData();
            //this.grdBrowView.RefreshDataSource();

            //this.grdBrowView.ForceInitialize();

            //dlg.WaitClear();
            //this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            UIBase.WaitClear();
        }

        private bool mbllShowPrice = false;
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
            this.gridView1.Columns["FCTYPE"].VisibleIndex = i++;
            this.gridView1.Columns["FCCODE"].VisibleIndex = i++;
            this.gridView1.Columns["FCNAME"].VisibleIndex = i++;
            if (mbllShowPrice)
                this.gridView1.Columns["FNPRICE"].VisibleIndex = i++;

            this.gridView1.Columns["FCTYPE"].Caption = "T";
            this.gridView1.Columns["FCCODE"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Code" });
            this.gridView1.Columns["FCNAME"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า", "Product Name" });
            this.gridView1.Columns["FNPRICE"].Caption = UIBase.GetAppUIText(new string[] { "ราคาขาย", "Price" });
            //this.gridView1.Columns["CSNAME"].Caption = "ชื่อย่อ";

            this.gridView1.Columns["FCTYPE"].Width = 30;
            this.gridView1.Columns["FCCODE"].Width = 120;

            this.gridView1.Columns["FNPRICE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["FNPRICE"].DisplayFormat.FormatString = "#,###,###.00";
            this.gridView1.OptionsView.ColumnAutoWidth = false;

            this.pmSetSortKey("FCCODE", true);
            this.pmCalcColWidth();

        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["FCTYPE"].Width
                                    + this.gridView1.Columns["FCCODE"].Width;

            if (mbllShowPrice)
            {
                intColWidth += this.gridView1.Columns["FNPRICE"].Width;
            }

            int intNewWidth = this.Width - intColWidth - 70;
            this.gridView1.Columns["FCNAME"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void dlgGetProd_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
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

        private void dlgGetProd_KeyDown(object sender, KeyEventArgs e)
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
            this.pmSetBrowView(false);
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            //this.grdBrowView.RefreshDataSource();
            //this.grdBrowView.ForceInitialize();
            
            //if (this.gridView1.RowCount > 0)
            //    this.gridView1.FocusedRowHandle = 0;
            //this.grdBrowView.RefreshDataSource();
            //this.grdBrowView.ForceInitialize();
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

            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;

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

            if (this.mstrSearchStr != inSearchStr 
                || this.mstrPdType.Trim() != ""
                || this.mbllIsFormQuery == false)
            {
                string strSearch = inSearchStr.TrimEnd();
                if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
                    strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
                {
                    strSearch = "";
                }

             
                this.mstrPdType = "";

                this.mstrSearchStr = "%" + strSearch + "%";
                this.pmRefreshBrowView();
            }

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {
                //this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                //this.ShowDialog();
                //this.mbllIsShowDialog = true;

                this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

                this.gridView1.MoveLast();
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
                {
                    string strSeekNear = BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMJobInfo.Field.Code ? MAXLENGTH_CODE : MAXLENGTH_NAME);
                    this.gridView1.StartIncrementalSearch(strSeekNear);
                }

                this.mintFocusRow = this.gridView1.FocusedRowHandle;

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

        public bool ValidateField(string inPdType, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "FCCODE")
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);
            else
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_NAME);

            //if (inOrderBy.ToUpper() == "FCCODE")
            //    inSearchStr = inSearchStr.TrimEnd();
            //else
            //    inSearchStr = inSearchStr.TrimEnd();

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr
                || inPdType != this.mstrPdType
                || this.mbllIsFormQuery == false)
            {
                string strSearch = inSearchStr.TrimEnd();
                if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
                    strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
                {
                    strSearch = "";
                }

                //this.mstrSearchStr = "%" + strSearch + "%";
                this.mstrSearchStr = strSearch;
                this.mstrPdType = inPdType;
                this.pmRefreshBrowView();
            }

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {
                //this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                //this.ShowDialog();
                //this.mbllIsShowDialog = true;

                this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

                this.gridView1.MoveLast();
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
                {
                    string strSeekNear = BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMJobInfo.Field.Code ? MAXLENGTH_CODE : MAXLENGTH_NAME);
                    this.gridView1.StartIncrementalSearch(strSeekNear);
                }

                this.mintFocusRow = this.gridView1.FocusedRowHandle;

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

        private bool mbllHasForce = false;
        private void dlgGetProd_Shown(object sender, EventArgs e)
        {

            //this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            if (!mbllHasForce)
            {
                mbllHasForce = true;
                this.grdBrowView.RefreshDataSource();
                this.grdBrowView.ForceInitialize();
            }

            this.gridView1.MoveFirst();
            this.gridView1.MoveLastVisible();
            this.gridView1.FocusedRowHandle = this.mintFocusRow;

        }


    }
}
