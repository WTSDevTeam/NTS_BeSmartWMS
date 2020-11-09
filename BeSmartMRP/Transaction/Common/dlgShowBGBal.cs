
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraGrid.Views.Base;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Transaction.Common
{

    public partial class dlgShowBGBal : UIHelper.frmBase
    {

        public static int MAXLENGTH_CODE = 4;
        public static int MAXLENGTH_NAME = 30;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = "UM";
        private string mstrSortKey = "QcBGChart";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private string mstrBranch ="";
        private string mstrType = "";
        private int mintBGYear;
        private string mstrSect = "";
        private string mstrJob = "";
        private string mstrBGChartGrp = "";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgShowBGBal()
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgShowBGBal_KeyDown);
            this.grdBrowView.Resize += new System.EventHandler(this.grdBrowView_Resize);
            this.gridView1.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.gridView1_ColumnWidthChanged);

            this.pmCreateTemBrow();
        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select * from BGChartHD where cPRBGChart = ? order by BGChartHD.cCode";

            if (this.mstrBGChartGrp != string.Empty)
            {
                this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Clear();

                //"select cPRBGChart from BGChartHD where cPRBGChart = ? order by BGChartHD.cCode"

                decimal decAmt = 0; decimal decRecvAmt = 0; decimal decUseAmt = 0; decimal decBalAmt = 0;
                DataRow dtrBGChart = null;
                string strPRChart = "";
                pobjSQLUtil.SetPara(new object[] { this.mstrBGChartGrp });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGrp1", "BGCHARTHD", "select * from BGChartHD where cRowID = ? ", ref strErrorMsg))
                {
                    dtrBGChart = this.dtsDataEnv.Tables["QGrp1"].Rows[0];
                    strPRChart = dtrBGChart["cPRBGChart"].ToString();
                    if (strPRChart.Trim() == string.Empty)
                    {
                        //Parent Chart เป็นว่าง ๆ แสดงว่าเป็นประเภทหมวดซึ่ง Sum ได้เลย
                        BudgetHelper.SuBeSmartMRPGroup(pobjSQLUtil
                            , App.ActiveCorp.RowID, this.mstrBranch, this.mstrType, this.mintBGYear, this.mstrSect, this.mstrJob, dtrBGChart["cCode"].ToString()
                            , ref decAmt, ref decRecvAmt, ref decUseAmt, ref decBalAmt);

                        DataRow dtrBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();
                        dtrBrow["Type"] = "1";
                        dtrBrow["QcBGChart"] = dtrBGChart["cCode"].ToString();
                        dtrBrow["QnBGChart"] = dtrBGChart["cName"].ToString();
                        dtrBrow["nBGAmt"] = decAmt;
                        dtrBrow["nUseAmt"] = decRecvAmt;
                        dtrBrow["nBalAmt"] = decBalAmt;
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrBrow);
                    }
                    else 
                    {

                        pobjSQLUtil.SetPara(new object[] { strPRChart });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGrp1", "BGCHARTHD", "select * from BGChartHD where cRowID = ? ", ref strErrorMsg))
                        {
                            dtrBGChart = this.dtsDataEnv.Tables["QGrp1"].Rows[0];
                            BudgetHelper.SuBeSmartMRPGroup(pobjSQLUtil
                                , App.ActiveCorp.RowID, this.mstrBranch, this.mstrType, this.mintBGYear, this.mstrSect, this.mstrJob, dtrBGChart["cCode"].ToString()
                                , ref decAmt, ref decRecvAmt, ref decUseAmt, ref decBalAmt);

                            DataRow dtrBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();
                            dtrBrow["Type"] = "1";
                            dtrBrow["QcBGChart"] = dtrBGChart["cCode"].ToString();
                            dtrBrow["QnBGChart"] = dtrBGChart["cName"].ToString();
                            dtrBrow["nBGAmt"] = decAmt;
                            dtrBrow["nUseAmt"] = decRecvAmt;
                            dtrBrow["nBalAmt"] = decBalAmt;
                            this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrBrow);
                        }

                        //กรณีมี Parent จะดึงรหัสใน Parent เดียวกันมาแสดง
                        pobjSQLUtil.SetPara(new object[] { strPRChart });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGrp1", "BGCHARTHD", "select * from BGChartHD where cPRBGChart = ? ", ref strErrorMsg))
                        {
                            foreach (DataRow dtrP1Brow in this.dtsDataEnv.Tables["QGrp1"].Rows)
                            {

                                dtrBGChart = dtrP1Brow;
                                BudgetHelper.SuBeSmartMRPGroup(pobjSQLUtil
                                    , App.ActiveCorp.RowID, this.mstrBranch, this.mstrType, this.mintBGYear, this.mstrSect, this.mstrJob, dtrBGChart["cCode"].ToString()
                                    , ref decAmt, ref decRecvAmt, ref decUseAmt, ref decBalAmt);

                                DataRow dtrBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].NewRow();
                                dtrBrow["Type"] = "2";
                                dtrBrow["QcBGChart"] = dtrBGChart["cCode"].ToString();
                                dtrBrow["QnBGChart"] = dtrBGChart["cName"].ToString();
                                dtrBrow["nBGAmt"] = decAmt;
                                dtrBrow["nUseAmt"] = decRecvAmt;
                                dtrBrow["nBalAmt"] = decBalAmt;
                                this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Add(dtrBrow);

                            
                            }
                        }
                    }
                }

            }

        }

        private void pmCreateTemBrow()
        {


            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataTable dtbTemBrow = new DataTable(this.mstrBrowViewAlias);

            dtbTemBrow.Columns.Add("Type", System.Type.GetType("System.String"));
            dtbTemBrow.Columns.Add("QcBGChart", System.Type.GetType("System.String"));
            dtbTemBrow.Columns.Add("QnBGChart", System.Type.GetType("System.String"));
            dtbTemBrow.Columns.Add("nBGAmt", System.Type.GetType("System.Decimal"));
            dtbTemBrow.Columns.Add("nUseAmt", System.Type.GetType("System.Decimal"));
            dtbTemBrow.Columns.Add("nBalAmt", System.Type.GetType("System.Decimal"));

            dtbTemBrow.Columns["Type"].DefaultValue = "";
            dtbTemBrow.Columns["QcBGChart"].DefaultValue = "";
            dtbTemBrow.Columns["QnBGChart"].DefaultValue = "";
            dtbTemBrow.Columns["nBGAmt"].DefaultValue = 0;
            dtbTemBrow.Columns["nUseAmt"].DefaultValue = 0;
            dtbTemBrow.Columns["nBalAmt"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemBrow);
        
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "Type,QcBGChart";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                //this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["Type"].Visible = false;
            this.gridView1.Columns["QcBGChart"].Caption = "รหัสงบประมาณ";
            this.gridView1.Columns["QnBGChart"].Caption = "ชื่องบประมาณ";
            this.gridView1.Columns["QnBGChart"].Caption = "ชื่องบประมาณ";
            this.gridView1.Columns["nBGAmt"].Caption = "งบประมาณ";
            this.gridView1.Columns["nUseAmt"].Caption = "ใช้ไป";
            this.gridView1.Columns["nBalAmt"].Caption = "คงเหลือ";

            this.gridView1.Columns["nBGAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBGAmt"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["nUseAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nUseAmt"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["nBalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBalAmt"].DisplayFormat.FormatString = "#,###,###,###.00";

            this.gridView1.Columns["QcBGChart"].Width = 95;
            this.gridView1.Columns["nBGAmt"].Width = 150;
            this.gridView1.Columns["nUseAmt"].Width = 150;
            this.gridView1.Columns["nBalAmt"].Width = 150;

            this.pmRecalColWidth();

            this.pmSetSortKey("QcBGChart", true);
        }

        private void pmRecalColWidth()
        {

            int intSumColWidth = this.gridView1.Columns["QcBGChart"].Width
                + this.gridView1.Columns["nBGAmt"].Width
                + this.gridView1.Columns["nUseAmt"].Width
                + this.gridView1.Columns["nBalAmt"].Width;

            int intColWidth = this.grdBrowView.Width - intSumColWidth - 30;
            this.gridView1.Columns["QnBGChart"].Width = (intColWidth < 120 ? 120 : intColWidth);

        }

        private void grdBrowView_Resize(object sender, EventArgs e)
        {
            this.pmRecalColWidth();
        }

        private void gridView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            this.pmRecalColWidth();
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

        private void dlgShowBGBal_KeyDown(object sender, KeyEventArgs e)
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
            //this.mstrSortKey = inColumn.ToUpper();
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

        public bool ValidateField(string inBranch, string inType, string inSect, string inJob, int inBGYear, string inBGChart, string inOrderBy, bool inForcePopUp)
        {
            this.mbllPopUpResult = false;

            if (this.mstrSearchStr != inBGChart || this.mbllIsFormQuery == false)
            {

                this.mstrBranch = inBranch;
                this.mstrType = inType;
                this.mintBGYear = inBGYear;
                this.mstrSect = inSect;
                this.mstrJob = inJob;

                string strSearch = inBGChart.TrimEnd();
                this.mstrSearchStr = strSearch;
                this.mstrBGChartGrp = this.mstrSearchStr;
                this.pmRefreshBrowView();
            }

            this.ShowDialog();
            this.mbllIsShowDialog = true;
            return this.mbllPopUpResult;
        }

        public DataRow RetrieveValue()
        {
            return null;
        }




    }
}
