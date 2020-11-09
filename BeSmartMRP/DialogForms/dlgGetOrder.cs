
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

using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.DialogForms
{
    public partial class dlgGetOrder : UIHelper.frmBase, IfrmDBBase
    {

        public static int MAXLENGTH_CODE = 7;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrBrowViewTable = MapTable.Table.OrderH;
        private string mstrRefTable = "ORDERH";
        private string mstrSortKey = "FCQCORDERH";
        private CoorType mCoorType = CoorType.Customer;

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private bool mbllPRReferSO_PR = false;
        private string mstrBranch = "";
        private string mstrRefType = "";
        private string mstrBook = "";
        private string mstrCoor = "";
        private string mstrCoorStr = "";
        private bool mbllShowAllStep = false;

        public string BranchID
        {
            get { return this.mstrBranch; }
            set { this.mstrBranch = value; }
        }

        public string CoorID
        {
            get { return this.mstrCoor; }
            set { this.mstrCoor = value; }
        }

        public BeSmartMRP.Business.Entity.CoorType CoorType
        {
            get { return this.mCoorType; }
            set
            {
                this.mCoorType = value;
                this.mstrCoorStr = (this.mCoorType == BeSmartMRP.Business.Entity.CoorType.Customer ? "ลูกค้า" : "ผู้จำหน่าย");
            }
        }

        public string BookID
        {
            get { return this.mstrBook; }
            set { this.mstrBook = value; }
        }

        public string RefType
        {
            get { return this.mstrRefType; }
            set { this.mstrRefType = value; }
        }

        public bool IsShowAllStep
        {
            set { this.mbllShowAllStep = value; }
        }

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetOrder()
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

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetOrder_KeyDown);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
        }

        private void pmSetBrowView()
        {
            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind();
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLCmd = "";
            if (this.mstrCoor == "")
            {
                strSQLCmd = "select 00000 as fnIsSelect , '   ' as fcSelectNo";
                strSQLCmd += ", OrderH.fcSkid , OrderH.fcBook , Book.fcCode as fcQcBook , OrderH.fcCode as fcQcOrderH , OrderH.fcRefNo , OrderH.fdDate ";
                strSQLCmd += ", OrderH.fnAmtKe+OrderH.fnVatAmtKe as nAmt, Coor.fcFChr as fcFChrCoor , Coor.fcSName as fcQnCoor ";

                strSQLCmd += " from OrderH , Book , Coor ";
                strSQLCmd += " where OrderH.fcBook = Book.fcSkid ";
                strSQLCmd += " and OrderH.fcCoor = Coor.fcSkid ";
                strSQLCmd += " and OrderH.fcCorp = ? ";
                strSQLCmd += " and OrderH.fcBranch = ?";
                strSQLCmd += " and OrderH.fcRefType = ?";
                strSQLCmd += " and OrderH.fcStat = ?";

                if (this.mstrBook != "")
                {
                    if (this.mbllPRReferSO_PR)
                    {
                        strSQLCmd += " and OrderH.fcBook = ?";
                        strSQLCmd += " and OrderH.fcStep2 = ?";
                        strSQLCmd += " and OrderH.fcApproveB <> ?";

                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL, this.mstrBook, SysDef.gc_REF_STEP2_WAIT });
                    }
                    else
                    {
                        if (this.mbllShowAllStep)
                        {
                            strSQLCmd += " and OrderH.fcBook = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL, this.mstrBook });
                        }
                        else
                        {
                            strSQLCmd += " and OrderH.fcStep = ?";
                            strSQLCmd += " and OrderH.fcBook = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, this.mstrBook });
                        }
                    }
                    strSQLCmd += " order by fcQcBook , fcQcOrderH";
                }
                else
                {
                    if (this.mbllPRReferSO_PR)
                    {
                        strSQLCmd += " and OrderH.fcStep2 = ?";
                        strSQLCmd += " and OrderH.fcApproveB <> ?";
                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP2_WAIT });
                    }
                    else
                    {
                        if (this.mbllShowAllStep)
                        {
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL });
                        }
                        else
                        {
                            strSQLCmd += " and OrderH.fcStep = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED });
                        }
                    }
                    strSQLCmd += " order by fcQcBook , fcQcOrderH";
                }
            }
            else
            {
                strSQLCmd = "select 00000 as fnIsSelect , '   ' as fcSelectNo";
                strSQLCmd += ",OrderH.fcSkid , OrderH.fcBook , Book.fcCode as fcQcBook , OrderH.fcCode as fcQcOrderH , OrderH.fcRefNo , OrderH.fdDate ";

                strSQLCmd += " from OrderH , Book ";
                strSQLCmd += " where OrderH.fcBook = Book.fcSkid";
                strSQLCmd += " and OrderH.fcCorp = ?";
                strSQLCmd += " and OrderH.fcBranch = ?";
                strSQLCmd += " and OrderH.fcRefType = ?";
                strSQLCmd += " and OrderH.fcCoor = ?";
                strSQLCmd += " and OrderH.fcStat = ?";
                if (this.mstrBook != "")
                {
                    if (this.mbllPRReferSO_PR)
                    {
                        strSQLCmd += " and OrderH.fcBook = ?";
                        strSQLCmd += " and OrderH.fcStep2 = ?";
                        strSQLCmd += " and OrderH.fcApproveB <> ?";

                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL, this.mstrBook, SysDef.gc_REF_STEP2_WAIT });
                    }
                    else
                    {
                        if (this.mbllShowAllStep)
                        {
                            strSQLCmd += " and OrderH.fcBook = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL, this.mstrBook });
                        }
                        else
                        {
                            strSQLCmd += " and OrderH.fcStep = ?";
                            strSQLCmd += " and OrderH.fcBook = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED, this.mstrBook });
                        }
                    }
                    strSQLCmd += " order by fcQcBook , fcQcOrderH";
                }
                else
                {
                    if (this.mbllPRReferSO_PR)
                    {
                        strSQLCmd += " and OrderH.fcStep2 = ?";
                        strSQLCmd += " and OrderH.fcApproveB <> ?";
                        pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP2_WAIT });
                    }
                    else
                    {
                        if (this.mbllShowAllStep)
                        {
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL });
                        }
                        else
                        {
                            strSQLCmd += " and OrderH.fcStep = ?";
                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, SysDef.gc_STAT_NORMAL, SysDef.gc_STEP_CREATED });
                        }
                    }
                    strSQLCmd += " order by fcQcBook , fcQcOrderH";
                }
            }

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrBrowViewTable, strSQLCmd, ref strErrorMsg);
            this.mbllIsFormQuery = true;
            this.pmInitGridProp();
            //dlg.WaitClear();
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "FCQCORDERH";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["FCQCORDERH"].VisibleIndex = 0;
            this.gridView1.Columns["FCREFNO"].VisibleIndex = 1;
            this.gridView1.Columns["FDDATE"].VisibleIndex = 2;
            this.gridView1.Columns["NAMT"].VisibleIndex = 3;

            this.gridView1.Columns["FCQCORDERH"].Visible = true;
            this.gridView1.Columns["FCREFNO"].Visible = true;
            this.gridView1.Columns["FDDATE"].Visible = true;
            this.gridView1.Columns["NAMT"].Visible = true;

            this.gridView1.Columns["FCQCORDERH"].Width = 50;
            this.gridView1.Columns["FDDATE"].Width = 50;
            this.gridView1.Columns["FCREFNO"].Width = 50;
            this.gridView1.Columns["NAMT"].Width = 50;

            this.gridView1.Columns["FCQCORDERH"].Caption = "เลขที่ภายใน";
            this.gridView1.Columns["FCREFNO"].Caption = "เลขที่อ้างอิง";
            this.gridView1.Columns["FDDATE"].Caption = "วันที่";
            this.gridView1.Columns["NAMT"].Caption = "มูลค่า";

            this.gridView1.Columns["NAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NAMT"].DisplayFormat.FormatString = "#,###,###.00";
            
            //this.grdBrowView.ColumnAutoResize = true;

            if (this.mstrCoor == string.Empty)
            {
                this.gridView1.Columns["FCQNCOOR"].VisibleIndex = 4;
                this.gridView1.Columns["FCQNCOOR"].Visible = true;
                this.gridView1.Columns["FCQNCOOR"].Caption = this.mstrCoorStr;

                this.gridView1.Columns["FCQCORDERH"].Width = 50;
                this.gridView1.Columns["FDDATE"].Width = 50;
                this.gridView1.Columns["FCREFNO"].Width = 50;
                //this.gridView1.Columns["fcQnCoor"].AutoSize();
            }
            else
            {
                this.gridView1.Columns["FCQCORDERH"].Width = 50;
                this.gridView1.Columns["FDDATE"].Width = 50;
                this.gridView1.Columns["FCREFNO"].Width = 50;
            }

            this.pmSetSortKey("FCQCORDERH", true);
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["FCQCORDERH"].Width
                                    + this.gridView1.Columns["FDDATE"].Width
                                    + this.gridView1.Columns["FCREFNO"].Width
                                    + this.gridView1.Columns["NAMT"].Width;

            int intNewWidth = this.grdBrowView.Width - intColWidth - 35;
            this.gridView1.Columns["FCQNCOOR"].Width = (intNewWidth > 0 ? intNewWidth : 70);
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

        public void RefreshBrowView()
        {
            this.pmRefreshBrowView();
        }

        private void dlgGetOrder_KeyDown(object sender, KeyEventArgs e)
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
            if (inOrderBy.ToUpper() == "FCQCORDERH")
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);

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

        public bool ValidateField(string inBook, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "FCQCORDERH")
                inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr
                || this.mstrBook != inBook
                || this.mbllIsFormQuery == false)
            {
                this.mstrBook = inBook;
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

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void dlgGetOrder_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }




    }
}
