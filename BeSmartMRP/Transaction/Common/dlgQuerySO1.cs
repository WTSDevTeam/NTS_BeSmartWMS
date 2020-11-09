
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
using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;

using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgQuerySO1 : UIHelper.frmBase
    {

        public dlgQuerySO1(string inRefType, string inBranch, string inBook, string inCoor, string inProd)
        {
            InitializeComponent();

            this.mstrRefType = inRefType;
            this.mstrBranch = inBranch;
            this.mstrBook = inBook;
            this.mstrCoor = inCoor;
            this.mstrProd = inProd;
            this.pmInitForm();
        }

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_OrderH";
        private string mstrBrowViewAlias2 = "Brow_OrderH1";
        private string mstrRefTable = "ORDERI";

        private string mstrAlias = "";
        private string mstrSortKey = "";
        private string mstrBranch = "";
        private string mstrBook = "";
        private string mstrCoor = "";
        private string mstrProd = "";
        private string mstrRefType = "";

        public string RefToKey
        {
            get { return this.mstrCoor + this.mstrProd; }
        }

        public decimal RefToQty
        {
            get { return this.txtTotMfgQty.Value; }
        }

        public DataTable RetVal
        {
            get { return this.dtsDataEnv.Tables[this.mstrBrowViewAlias2]; }
        }

        private void pmInitForm()
        {

            UIBase.WaitWind("Loading Form...");
            //this.pmInitializeComponent();
            this.pmSetBrowView();
            this.pmInitGridProp();
            this.pmSetFormUI();

            this.pmSetActivePage(0);

            UIBase.WaitClear();
        }

        private void pmSetFormUI()
        {
            //this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });
            this.lblTitle1.Text = UIBase.GetAppUIText(new string[] { "ขั้นตอนที่ 1 : ระบุสินค้าที่จะดึงไปผลิต", "Step 1 : Select Product Items from S/O" });
            this.lblTitle2.Text = UIBase.GetAppUIText(new string[] { "ขั้นตอนที่ 2 :ระบุเอกสาร " + mstrRefType + " ที่จะดึงไปผลิต", "Step 2 : Select S/O for Production" });

            string strCoorMsg = (mstrRefType == "SO" ? UIBase.GetAppUIText(new string[] { "ลูกค้า", "Customer" }) : UIBase.GetAppUIText(new string[] { "ผู้จำหน่าย", "Supplier" }));
            this.lblCoor.Text = string.Format("{0} :", new string[] { strCoorMsg });
            this.lblProd.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "สินค้า", "Product" }) });
            this.lblTotMfgQty.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "รวมจำนวนที่สั่งผลิต", "Total Qty." }) });
        
        }

        private void pmSetActivePage(int inActivePage)
        {

            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.TabPages[1].PageEnabled = false;

            this.pgfMainEdit.SelectedTabPageIndex = inActivePage;
            this.pgfMainEdit.TabPages[inActivePage].PageEnabled = true;

            this.pmSetToolbarState(inActivePage);

            if (inActivePage == 1)
            {
                this.pmLoadOrdItem();
            }
        }

        private void pmSetToolbarState(int inActivePage)
        {
            this.barManager1.Items["btnNext"].Enabled = (inActivePage == 0 ? true : false);
            this.barManager1.Items["btnPrev"].Enabled = (inActivePage == 1 ? true : false);
            this.barManager1.Items["btnOK2"].Enabled = (inActivePage == 1 ? true : false);
        }

        private void pmSetBrowView()
        {

            UIBase.WaitWind("Loading รายการสินค้า...");
            string strErrorMsg = " ";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            object[] pAPara = null;
            if (this.mstrCoor != string.Empty && this.mstrProd != string.Empty)
            {
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, this.mstrProd, this.mstrBook };
            }
            else if (this.mstrCoor != string.Empty && this.mstrProd == string.Empty)
            {
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, this.mstrBook };
            }
            else if (this.mstrCoor == string.Empty && this.mstrProd != string.Empty)
            {
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrProd, this.mstrBook };
            }
            else
            {
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook };
            }
            strSQLStr = " ";
            
            strSQLStr += " select ";
            strSQLStr += " (select P.FCCODE from PROD P where P.FCSKID = ORDERI.FCPROD) as QCPROD ";
            strSQLStr += " , (select P.FCNAME from PROD P where P.FCSKID = ORDERI.FCPROD) as QNPROD ";
            strSQLStr += " , (select C.FCCODE from COOR C where C.FCSKID = ORDERI.FCCOOR) as QCCOOR ";
            strSQLStr += " , (select C.FCNAME from COOR C where C.FCSKID = ORDERI.FCCOOR) as QNCOOR ";
            strSQLStr += " , sum(ORDERI.FNBACKQTY) as FNBACKQTY ";
            strSQLStr += " , ORDERI.FCCOOR, ORDERI.FCPROD ";
            strSQLStr += " from ORDERI ";
            strSQLStr += " left join PROD on PROD.FCSKID = ORDERI.FCPROD ";
            strSQLStr += " left join COOR on COOR.FCSKID = ORDERI.FCCOOR ";
            strSQLStr += " left join ORDERH on ORDERH.FCSKID = ORDERI.FCORDERH ";
            strSQLStr += " where ORDERI.FCCORP = ? ";
            strSQLStr += " and ORDERI.FCBRANCH = ? ";
            strSQLStr += " and ORDERI.FCREFTYPE = ? ";
            strSQLStr += " and ORDERI.FCSTEP <> 'P' ";
            strSQLStr += " and ORDERI.FCSTEP <> 'L' ";
            strSQLStr += " and ORDERI.FNBACKQTY <> 0 ";
            if (this.mstrCoor != string.Empty && this.mstrProd != string.Empty)
            {
                strSQLStr += " and ORDERH.FCCOOR = ? and ORDERI.FCPROD = ? ";
            }
            else if (this.mstrCoor != string.Empty && this.mstrProd == string.Empty)
            {
                strSQLStr += " and ORDERH.FCCOOR = ? ";
            }
            else if (this.mstrCoor == string.Empty && this.mstrProd != string.Empty)
            {
                strSQLStr += " and ORDERI.FCPROD = ? ";
            }
            strSQLStr += " and ORDERH.FCBOOK = ? ";
            strSQLStr += " group by ORDERI.FCCOOR, ORDERI.FCPROD ";
            strSQLStr += " order by QCPROD,QCCOOR ";

            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLStr, ref strErrorMsg);
            for (int i = 0; i < this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count; i++)
            {
                DataRow dtrBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[i];
                decimal decCutQty = pmSumCutQty1(dtrBrow["FCCOOR"].ToString(), dtrBrow["FCPROD"].ToString());
                decimal decBackQty = Convert.ToDecimal(dtrBrow["FNBACKQTY"]);
                decimal decMOQty = decBackQty - decCutQty;
                dtrBrow["FNBACKQTY"] = decMOQty;
                if (decMOQty <= 0)
                {
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[i].Delete();
                    //this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(i);
                }

            }
            UIBase.WaitClear();

        }

        private decimal pmSumCutQty1(string inCoor, string inProd)
        {
            string strErrorMsg = " ";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strSectTab = strFMDBName + ".dbo.ORDERI";

            object[] pAPara = new object[] { inProd, inCoor };
            strSQLStr = " select ORDERI.FCPROD,ORDERI.FCCOOR , REFDOC.NQTY from " + strSectTab + " ORDERI ";
            strSQLStr += " left join REFDOC on REFDOC.CCHILDTYPE = 'SO' and REFDOC.CCHILDI = ORDERI.FCSKID ";
            strSQLStr += " where ORDERI.FCPROD = ? and ORDERI.FCCOOR = ? ";

            decimal decSumQty = 0;
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QORDERI", "ORDERI", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrSum in this.dtsDataEnv.Tables["QORDERI"].Rows)
                {
                    if (!Convert.IsDBNull(dtrSum["NQTY"]))
                    {
                        decSumQty = decSumQty + Convert.ToDecimal(dtrSum["NQTY"]);
                    }
                }

            }
            return decSumQty;
        }

        private decimal pmSumCutQty2(string inOrderI)
        {
            string strErrorMsg = " ";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strSectTab = strFMDBName + ".dbo.ORDERI";

            object[] pAPara = new object[] { inOrderI };
            strSQLStr = " select ORDERI.FCPROD,ORDERI.FCCOOR , REFDOC.NQTY from " + strSectTab + " ORDERI ";
            strSQLStr += " left join REFDOC on REFDOC.CCHILDTYPE = 'SO' and REFDOC.CCHILDI = ORDERI.FCSKID ";
            strSQLStr += " where ORDERI.FCSKID = ? ";

            decimal decSumQty = 0;
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QORDERI", "ORDERI", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrSum in this.dtsDataEnv.Tables["QORDERI"].Rows)
                {
                    if (!Convert.IsDBNull(dtrSum["NQTY"]))
                    {
                        decSumQty = decSumQty + Convert.ToDecimal(dtrSum["NQTY"]);
                    }
                }

            }
            return decSumQty;
        }

        private void pmSetBrowView2()
        {
            UIBase.WaitWind("Loading รายการสินค้า...");
            string strErrorMsg = " ";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            object[] pAPara = null;
            pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrCoor, this.mstrProd, this.mstrBook };
            strSQLStr = " ";
            
            strSQLStr += " select ";
            strSQLStr += " (select H.FCREFNO from ORDERH H where H.FCSKID = ORDERI.FCORDERH) as FCREFNO ";
            strSQLStr += " , (select H.FCCODE from ORDERH H where H.FCSKID = ORDERI.FCORDERH) as FCCODE ";
            strSQLStr += " , (select H.FDDATE from ORDERH H where H.FCSKID = ORDERI.FCORDERH) as FDDATE ";
            strSQLStr += " , (select P.FCCODE from PROD P where P.FCSKID = ORDERI.FCPROD) as QCPROD ";
            strSQLStr += " , (select P.FCNAME from PROD P where P.FCSKID = ORDERI.FCPROD) as QNPROD ";
            strSQLStr += " , (select C.FCCODE from COOR C where C.FCSKID = ORDERI.FCCOOR) as QCCOOR ";
            strSQLStr += " , (select C.FCNAME from COOR C where C.FCSKID = ORDERI.FCCOOR) as QNCOOR ";
            strSQLStr += " , sum(ORDERI.FNBACKQTY) as FNBACKQTY ";
            strSQLStr += " , ORDERI.FCSKID, ORDERI.FCORDERH , ORDERI.FCCOOR, ORDERI.FCPROD";
            strSQLStr += " from ORDERI ";
            strSQLStr += " left join PROD on PROD.FCSKID = ORDERI.FCPROD ";
            strSQLStr += " left join COOR on COOR.FCSKID = ORDERI.FCCOOR ";
            strSQLStr += " left join ORDERH on ORDERH.FCSKID = ORDERI.FCORDERH ";
            //strSQLStr += " left join BOOK on BOOK.FCSKID = BOOK.FCBOOK ";
            strSQLStr += " where ORDERI.FCCORP = ? ";
            strSQLStr += " and ORDERI.FCBRANCH = ? ";
            strSQLStr += " and ORDERI.FCREFTYPE = ? ";
            strSQLStr += " and ORDERI.FCSTEP <> 'P' ";
            strSQLStr += " and ORDERI.FCSTEP <> 'L' ";
            strSQLStr += " and ORDERI.FCCOOR = ? ";
            strSQLStr += " and ORDERI.FCPROD = ? ";
            strSQLStr += " and ORDERI.FNBACKQTY <> 0 ";
            strSQLStr += " and ORDERH.FCBOOK = ? ";
            strSQLStr += " group by ORDERI.FCSKID, ORDERI.FCORDERH , ORDERI.FCCOOR, ORDERI.FCPROD ";
            strSQLStr += " order by FCREFNO,QCPROD,QCCOOR ";

            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias2, this.mstrRefTable, strSQLStr, ref strErrorMsg);
            for (int i = 0; i < this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Rows.Count; i++)
            {
                DataRow dtrBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Rows[i];
                decimal decCutQty = pmSumCutQty2(dtrBrow["FCSKID"].ToString());
                decimal decBackQty = Convert.ToDecimal(dtrBrow["FNBACKQTY"]);
                decimal decMOQty = decBackQty - decCutQty;
                dtrBrow["FNBACKQTY"] = decMOQty;
                if (decMOQty <= 0)
                {
                    //this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Rows[i].Delete();
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Rows.RemoveAt(i);
                }

            }

            DataColumn dtcTagValue = new DataColumn("CTAGVALUE", Type.GetType("System.Boolean"));
            dtcTagValue.DefaultValue = false;
            this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Columns.Add(dtcTagValue);

            UIBase.WaitClear();
        }

        private void pmInitGridProp()
        {

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["QCPROD"].VisibleIndex = i++;
            this.gridView1.Columns["QNPROD"].VisibleIndex = i++;
            this.gridView1.Columns["QCCOOR"].VisibleIndex = i++;
            this.gridView1.Columns["QNCOOR"].VisibleIndex = i++;
            this.gridView1.Columns["FNBACKQTY"].VisibleIndex = i++;

            this.gridView1.Columns["QCPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Code" });
            this.gridView1.Columns["QNPROD"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Name" });
            this.gridView1.Columns["QCCOOR"].Caption = UIBase.GetAppUIText(new string[] { "รหัสลูกค้า", "Customer Code" });
            this.gridView1.Columns["QNCOOR"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อลูกค้า", "Customer Name" });
            this.gridView1.Columns["FNBACKQTY"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน", "Qty." });

            this.gridView1.Columns["QCPROD"].Width = 60;
            this.gridView1.Columns["QCCOOR"].Width = 60;
            this.gridView1.Columns["QNCOOR"].Width = 80;
            this.gridView1.Columns["FNBACKQTY"].Width = 80;

            this.gridView1.Columns["FNBACKQTY"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["FNBACKQTY"].DisplayFormat.FormatString = "#,###,###.00";
            
            this.grdBrowView.Focus();
        }

        private void pmInitGridProp2()
        {

            //System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias].DefaultView;
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias2];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
                this.gridView2.Columns[intCnt].OptionsColumn.AllowEdit = false;
            }

            int i = 0;
            this.gridView2.Columns["CTAGVALUE"].VisibleIndex = i++;
            this.gridView2.Columns["FCCODE"].VisibleIndex = i++;
            this.gridView2.Columns["FCREFNO"].VisibleIndex = i++;
            this.gridView2.Columns["FDDATE"].VisibleIndex = i++;
            this.gridView2.Columns["FNBACKQTY"].VisibleIndex = i++;

            this.gridView2.Columns["CTAGVALUE"].Caption = " ";
            this.gridView2.Columns["FCCODE"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "Doc. Code" });
            this.gridView2.Columns["FCREFNO"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No" });
            this.gridView2.Columns["FDDATE"].Caption = UIBase.GetAppUIText(new string[] { "วันที่", "Doc. Date" });
            this.gridView2.Columns["FNBACKQTY"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน", "Qty." });

            this.gridView2.Columns["CTAGVALUE"].OptionsColumn.AllowSort = DefaultBoolean.False;
            this.gridView2.Columns["CTAGVALUE"].ImageIndex = 0;
            this.gridView2.Columns["CTAGVALUE"].ImageAlignment = StringAlignment.Center;
            this.gridView2.Columns["CTAGVALUE"].OptionsColumn.AllowEdit = true;

            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.Columns["CTAGVALUE"].Width = 40;
            this.gridView2.Columns["FCCODE"].Width = 85;
            this.gridView2.Columns["FDDATE"].Width = 85;
            this.gridView2.Columns["FNBACKQTY"].Width = 80;

            this.gridView2.Columns["FNBACKQTY"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["FNBACKQTY"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView2.Columns["FDDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["FDDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.pmCalcColWidth();
            this.gridView2.Columns["CTAGVALUE"].ColumnEdit = this.chkTag;
            this.grdTemPd.Focus();
        }

        private void pmCalcColWidth()
        {
            int intColWidth = this.gridView2.Columns["CTAGVALUE"].Width
                                    + this.gridView2.Columns["FCCODE"].Width
                                    + this.gridView2.Columns["FDDATE"].Width
                                    + this.gridView2.Columns["FNBACKQTY"].Width;

            int intNewWidth = this.Width - intColWidth - 50;
            this.gridView2.Columns["FCREFNO"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private void dlgQuerySO1_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView2_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
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
                case "PREV":
                    this.pmSetActivePage(0);
                    break;
                case "NEXT":
                    this.pmSetActivePage(1);
                    break;
            }

        }

        private void pmLoadOrdItem()
        {
        
            DataRow dtrBrowView = this.gridView1.GetFocusedDataRow();

            this.txtQcCoor.Text = dtrBrowView["QcCoor"].ToString();
            this.txtQnCoor.Text = dtrBrowView["QnCoor"].ToString();
            this.txtQcProd.Text = dtrBrowView["QcProd"].ToString();
            this.txtQnProd.Text = dtrBrowView["QnProd"].ToString();

            this.mstrCoor = dtrBrowView["fcCoor"].ToString();
            this.mstrProd = dtrBrowView["fcProd"].ToString();
            this.pmSetBrowView2();
            this.pmInitGridProp2();

        }

        private void dlgQuerySO1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (this.pgfMainEdit.SelectedTabPageIndex == 0)
                    {
                        this.pmSetActivePage(1);
                    }
                    break;
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void grdBrowView_SortKeysChanged(object sender, System.EventArgs e)
        {
        }

        private int GetColumnIndex(GridViewInfo info, GridColumn column)
        {
            for (int i = 0; i < info.ColumnsInfo.Count; i++)
                if (info.ColumnsInfo[i].Column == column)
                    return i;
            return -1;
        }

        private void gridView2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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
            this.pmSumMfgQty();

        }

        private void pmSumMfgQty()
        {
            decimal decSumQty = 0;
            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrBrowViewAlias2].Rows)
            {
                if (Convert.ToBoolean(dtrTemPd["CTAGVALUE"]))
                {
                    decSumQty = decSumQty + Convert.ToDecimal(dtrTemPd["FNBACKQTY"]);
                }
            }
            this.txtTotMfgQty.Value = decSumQty;
        }

        private void chkTag_CheckedChanged(object sender, EventArgs e)
        {
            DataRow dtrTemPd = this.gridView2.GetFocusedDataRow();
            DevExpress.XtraEditors.CheckEdit oSender = sender as DevExpress.XtraEditors.CheckEdit;
            dtrTemPd["CTAGVALUE"] = oSender.Checked;
            this.pmSumMfgQty();
        }

    
    }
}
