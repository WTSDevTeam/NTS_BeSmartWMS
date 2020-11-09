
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
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;

namespace BeSmartMRP.Transaction.Common.MRP
{

    public partial class dlgGetMO_Items : UIHelper.frmBase
    {

        private int miniMaxLength_Code = 4;
        private int miniMaxLength_Name = 50;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = "MASTER_ACCHART";
        private string mstrSortKey = "QCPROD";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private object[] mAPara = null;

        private string mstrBrowViewSQLStr = "";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetMO_Items()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public void SetBrowView(string inSQLStr, object[] inPara, string inRefTable, int inMaxCode, int inMaxName)
        {
            this.mstrBrowViewSQLStr = inSQLStr;
            this.mAPara = inPara;
            this.mstrRefTable = inRefTable;
            this.miniMaxLength_Code = inMaxCode;
            this.miniMaxLength_Name = inMaxName;
            this.pmSetBrowView();
            this.pmInitGridProp();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetMO_Items_KeyDown);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = this.mstrBrowViewSQLStr;
            //strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            if (this.mAPara != null && this.mAPara.Length > 0)
                pobjSQLUtil.SetPara(this.mAPara);

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLExec, ref strErrorMsg);

            DataColumn dtcTagValue = new DataColumn("CTAGVALUE", Type.GetType("System.Boolean"));
            dtcTagValue.DefaultValue = false;
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Columns.Add(dtcTagValue);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "QCPROD";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowEdit = false;
            }

            int i = 0;
            this.gridView1.Columns["CTAGVALUE"].VisibleIndex = i++;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;
            this.gridView1.Columns["QNPROD"].VisibleIndex = i++;
            this.gridView1.Columns["NQTY"].VisibleIndex = i++;
            this.gridView1.Columns["QNUM"].VisibleIndex = i++;

            this.gridView1.Columns["CTAGVALUE"].OptionsColumn.AllowSort = DefaultBoolean.False;
            this.gridView1.Columns["CTAGVALUE"].ImageIndex = 0;
            this.gridView1.Columns["CTAGVALUE"].ImageAlignment = StringAlignment.Center;
            this.gridView1.Columns["CTAGVALUE"].OptionsColumn.AllowEdit = true;

            this.gridView1.Columns["CTAGVALUE"].Visible = true;
            this.gridView1.Columns["QCPROD"].Visible = true;
            this.gridView1.Columns["QNPROD"].Visible = true;

            this.gridView1.Columns["CTAGVALUE"].Caption = " ";
            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView1.Columns["QNPROD"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView1.Columns["NQTY"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView1.Columns["QNUM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วยนับ", "Unit" });

            this.gridView1.Columns["NQTY"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NQTY"].DisplayFormat.FormatString = "#,###,##0.0000";

            this.gridView1.Columns["CTAGVALUE"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["CTAGVALUE"].Width = 40;
            this.gridView1.Columns["QCPROD"].Width = 120;
            this.gridView1.Columns["NQTY"].Width = 90;
            this.gridView1.Columns["QNUM"].Width = 90;

            this.pmCalcColWidth();

            this.pmSetSortKey("QCPROD", true);
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

        private void dlgGetMO_Items_KeyDown(object sender, KeyEventArgs e)
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

            this.gridView1.FocusedColumn = this.gridView1.Columns["QCPROD"];
            this.gridView1.FocusedColumn = this.gridView1.Columns["QNPROD"];
            this.gridView1.FocusedColumn = this.gridView1.Columns["QCPROD"];

            //if (this.gridView1.RowCount > 0)
            //{
            //    this.gridView1.FocusedRowHandle = 0;
            //    this.statusStrip1.Focus();
            //}
            this.mbllPopUpResult = true;
            this.Hide();
        }

        public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            if (inOrderBy.ToUpper() == "QCPROD")
                inSearchStr = inSearchStr.PadRight(this.miniMaxLength_Code);
            else
                inSearchStr = inSearchStr.PadRight(this.miniMaxLength_Name);

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
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["cRowID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }

        public void LoadTagValue(ref ArrayList ioTagValue)
        {
            this.pmLoadTagValue(ref ioTagValue);
        }

        private void pmLoadTagValue(ref ArrayList ioTagValue)
        {
            ioTagValue.Clear();
            foreach (DataRow dtrValue in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
            {
                if (Convert.ToBoolean(dtrValue["CTAGVALUE"]))
                {
                    ioTagValue.Add(dtrValue["CROWID"].ToString().TrimEnd());
                }
            }
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["CTAGVALUE"].Width
                        + this.gridView1.Columns["QCPROD"].Width
                        + this.gridView1.Columns["NQTY"].Width
                        + this.gridView1.Columns["QNUM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView1.Columns["QNPROD"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        
        }

        private void dlgGetMO_Items_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private int GetColumnIndex(GridViewInfo info, GridColumn column)
        {
            for (int i = 0; i < info.ColumnsInfo.Count; i++)
                if (info.ColumnsInfo[i].Column == column)
                    return i;
            return -1;
        }

        private void gridView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            GridView view = sender as GridView;
            GridHitInfo hInfo = view.CalcHitInfo(e.X, e.Y);
            if (hInfo.InColumn)
            {
                GridViewInfo info = view.GetViewInfo() as GridViewInfo;
                int columnIndex = GetColumnIndex(info, hInfo.Column);
                for (int i = 0; i < info.ColumnsInfo[columnIndex].InnerElements.Count; i++)
                    if (info.ColumnsInfo[columnIndex].InnerElements[i].ElementInfo is DevExpress.Utils.Drawing.GlyphElementInfoArgs)
                        if (info.ColumnsInfo[columnIndex].InnerElements[i].ElementInfo.Bounds.Contains(e.X, e.Y))
                        {
                            if (Convert.ToInt32(hInfo.Column.Tag) == 0)
                            {
                                hInfo.Column.ImageIndex = 1;
                                hInfo.Column.Tag = 1;
                                this.pmCheckAll(view, hInfo.Column, true);
                            }
                            else
                            {
                                hInfo.Column.ImageIndex = 0;
                                hInfo.Column.Tag = 0;
                                this.pmCheckAll(view, hInfo.Column, false);
                            }
                            DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                        }
            }
        }

        private void pmCheckAll(GridView view, GridColumn column, bool val)
        {
            view.BeginDataUpdate();
            for (int i = 0; i < view.DataRowCount; i++)
            {
                view.SetRowCellValue(i, column, val);
            }
            view.EndDataUpdate();

        }

    }
}
