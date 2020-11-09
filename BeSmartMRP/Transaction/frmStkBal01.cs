
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;


namespace BeSmartMRP.Transaction
{
    public partial class frmStkBal01 : UIHelper.frmBase
    {
        
        private DataSet dtsDataEnv = new DataSet();
        private static frmStkBal01 mInstanse = null;
        private static frmStkBal01 mInstanse_3 = null;

        private string mstrTemPd = "TemPd";
        private string mstrTemPd2 = "TemPd2";
        private string mstrTemStk = "TemStk";

        private string mstrTemPd_Local = "TemPd_Local";
        private string mstrTemStk_Local = "TemStk_Local";

        private string mstrTemQProd = "TemQProd";

        private int mintRowIndex = -1;
        private int mintRowCount = -1;
        private string mstrProd = "";

        private string mstrRepName = "";
        private string mstrTaskName = "";

        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;

        private ArrayList pATagCode = new ArrayList();

        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private string mstrProdPict = "";
        private static bool mbllIsShow3 = false;

        public static frmStkBal01 GetInstanse(bool IsShow3)
        {
            if (IsShow3)
            {
                if (mInstanse_3 == null)
                {
                    mInstanse_3 = new frmStkBal01(IsShow3);
                }
                return mInstanse_3;
            }
            else
            {
                if (mInstanse == null)
                {
                    mInstanse = new frmStkBal01(IsShow3);
                }
                return mInstanse;
            }
        }

        private static void pmClearInstanse()
        {

            if (mbllIsShow3)
            {
                if (mInstanse_3 != null)
                {
                    mInstanse_3 = null;
                }
            }
            else
            {
                if (mInstanse != null)
                {
                    mInstanse = null;
                }
            }
        }

        public frmStkBal01(bool InShow3)
        {
            InitializeComponent();
            mbllIsShow3 = InShow3;
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");

            if (mbllIsShow3)
            {
                //pAWH = new string[] { "0001", "SR-1", "SR-2", "SR-3", "SR-7" };
                pAWH = new string[] { "001", "ST01", "ST02", "ST03", "ST04" };
                pAWH2 = new string[] { "WT-1", "WT-2", "WT-3", "WT-7" };
                //mstrWHList = "'0001','SR-1', 'SR-2', 'SR-2', 'SR-7' , 'WT-1' , 'WT-2' , 'WT-3' , 'WT-7'";
                //mstrWHList = "'001'";
                mstrWHList = "'001','ST01','ST02','ST03','ST04'";
            }
            else
            {
                //pAWH = new string[] { "0001", "SR-1", "SR-2", "SR-7" };
                //pAWH2 = new string[] { "WT-1", "WT-2", "WT-7" };
                //mstrWHList = "'0001','SR-1', 'SR-2', 'SR-7' , 'WT-1' , 'WT-2' , 'WT-7'";
                //pAWH = new string[] { "001" };
                pAWH = new string[] { "001", "ST01", "ST02", "ST03", "ST04" };
                pAWH2 = new string[] { "WT-1", "WT-2", "WT-7" };
                //mstrWHList = "'001'";
                mstrWHList = "'001','ST01','ST02','ST03','ST04'";
            }

            this.pmInitializeComponent();
            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            this.dtsDataEnv.CaseSensitive = true;

            this.pmCreateTem();
            this.pmInitGridProp();
            
            this.pmDefaultOption();

        }

        private void pmDefaultOption()
        {
            
            this.pmBlankFormData();

            this.pmLoadDefaultOption();

        }

        private void pmLoadDefaultOption()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            object[] pAPara = new object[] { App.gcCorp };
            objSQLHelper.SetPara(pAPara);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from " + MapTable.Table.Branch + " where FCCORP = ? order by FCCODE", ref strErrorMsg))
            {
            }

            this.mintRowIndex = -1;
            this.mintRowCount = 0;

            pAPara = new object[] { App.gcCorp };
            objSQLHelper.SetPara(pAPara);
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrTemQProd, "PROD", "select fcSkid, fcCode from " + MapTable.Table.Product + " where FCCORP = ? order by FCCODE", ref strErrorMsg))
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrTemQProd, "PROD", "select fcSkid, fcCode from " + MapTable.Table.Product + " where FCCORP = ? and FCSTATUS <> 'I' order by FCCODE", ref strErrorMsg))
            {
                this.mintRowCount = this.dtsDataEnv.Tables[this.mstrTemQProd].Rows.Count;
            }
        }

        private void pmBlankFormData()
        {

            this.mstrProd = "";
            //this.mintRowCount = 0;
            //this.mintRowIndex = -1;

            this.mstrProdPict = "";
            this.txtQcProd.Tag = "";
            this.txtQcProd.Text = "";
            this.txtQnProd.Text = "";
            this.txtQsProd.Text = "";

            this.txtQty_PO.Value = 0;
            this.txtQty_SO.Value = 0;
            this.txtQty_Bal.Value = 0;

            this.txtPrice1.Value = 0;
            this.txtPrice2.Value = 0;

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Clear();

            this.dtspstK021.XRPSTK02.Rows.Clear();

            this.lblRowIdx.Text = this.mintRowIndex.ToString();

        }

        private void pmLoadFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);


            string strCurrBranch = "";
            string strQueryProd = "select PROD.FCSKID,PROD.FCCODE,PROD.FCNAME,PROD.FCSNAME,PROD.FMPICNAME,PROD.FNPRICE,UM.FCNAME as QNUM  from PROD left join UM on UM.FCSKID = PROD.FCUM where PROD.FCSKID = ?";
            objSQLHelper.SetPara(new object[] { this.mstrProd });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", strQueryProd, ref strErrorMsg))
            {

                DataRow dtrRow = this.dtsDataEnv.Tables["QProd"].Rows[0];

                //this.pmRequery(this.mstrProd, dtrRow["FCCODE"].ToString().TrimEnd());

                this.txtQcProd.Tag = this.mstrProd;
                this.txtQcProd.Text = dtrRow["FCCODE"].ToString().TrimEnd();
                this.txtQnProd.Text = dtrRow["FCNAME"].ToString().TrimEnd();
                this.txtQsProd.Text = dtrRow["FCSNAME"].ToString().TrimEnd();

                this.pmRequery(this.mstrProd, dtrRow["FCCODE"].ToString().TrimEnd());

                this.txtPrice1.Value = Convert.ToDecimal(dtrRow["FNPRICE"]);

                this.lblQnUM1.Text = dtrRow["QNUM"].ToString().TrimEnd();
                this.lblQnUM2.Text = this.lblQnUM1.Text;
                this.lblQnUM3.Text = this.lblQnUM1.Text;

                //objSQLHelper.SetPara(new object[] { dtrRow["FCUM"].ToString() });
                //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select FCNAME from UM where FCSKID = ?", ref strErrorMsg))
                //{
                //    this.lblQnUM1.Text = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().Trim();
                //    this.lblQnUM2.Text = this.lblQnUM1.Text;
                //    this.lblQnUM3.Text = this.lblQnUM1.Text;
                //}

                this.mstrProdPict = "";
                if (!Convert.IsDBNull(dtrRow["FMPICNAME"]))
                {
                    //this.mstrProdPict = BizRule.GetMemData2(dtrRow["FMPICNAME"].ToString().TrimEnd(), "<P1>");
                }

            }

            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void pmCreateTem()
        {

            this.pmCreatemPd(this.mstrTemPd);
            this.pmCreatemPd(this.mstrTemPd2);

            DataTable dtbTemStk = new DataTable(this.mstrTemStk);
            dtbTemStk.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("CreateDate", System.Type.GetType("System.DateTime"));
            dtbTemStk.Columns.Add("QcBranch", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("QnBranch", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("QcWHouse", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("QcWHouse2", System.Type.GetType("System.String"));
            dtbTemStk.Columns.Add("QTY1", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("QTY2", System.Type.GetType("System.Decimal"));
            dtbTemStk.Columns.Add("Total", System.Type.GetType("System.Decimal"), "QTY1+QTY2");

            dtbTemStk.Columns["QcProd"].DefaultValue = "";
            dtbTemStk.Columns["QcBranch"].DefaultValue = "";
            dtbTemStk.Columns["QnBranch"].DefaultValue = "";
            dtbTemStk.Columns["QcWHouse"].DefaultValue = "";
            dtbTemStk.Columns["QcWHouse2"].DefaultValue = "";
            dtbTemStk.Columns["QTY1"].DefaultValue = 0;
            dtbTemStk.Columns["QTY2"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemStk);

            DataTable dtbTemPd_Local = new DataTable(this.mstrTemPd_Local);
            dtbTemPd_Local.Columns.Add("PO_dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd_Local.Columns.Add("PO_cRefNo", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("PO_nQTY", System.Type.GetType("System.Decimal"));
            dtbTemPd_Local.Columns.Add("PO_dDeliDate", System.Type.GetType("System.DateTime"));

            dtbTemPd_Local.Columns.Add("SO_dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd_Local.Columns.Add("SO_cRefNo", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("SO_QnCoor", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("SO_QnEmpl", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("SO_nQTY", System.Type.GetType("System.Decimal"));
            dtbTemPd_Local.Columns.Add("SO_dDeliDate", System.Type.GetType("System.DateTime"));

            dtbTemPd_Local.Columns.Add("nStdPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd_Local.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd_Local.Columns.Add("nPriceInVAT", System.Type.GetType("System.Decimal"));

            dtbTemPd_Local.Columns.Add("RefType", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemPd_Local.Columns.Add("Load_Date", System.Type.GetType("System.DateTime"));

            dtbTemPd_Local.Columns["RefType"].DefaultValue = "";
            dtbTemPd_Local.Columns["QcProd"].DefaultValue = "";
            dtbTemPd_Local.Columns["PO_cRefNo"].DefaultValue = "";
            dtbTemPd_Local.Columns["PO_nQTY"].DefaultValue = 0;
            dtbTemPd_Local.Columns["SO_cRefNo"].DefaultValue = "";
            dtbTemPd_Local.Columns["SO_QnCoor"].DefaultValue = "";
            dtbTemPd_Local.Columns["SO_QnEmpl"].DefaultValue = "";
            dtbTemPd_Local.Columns["SO_nQTY"].DefaultValue = 0;
            dtbTemPd_Local.Columns["nStdPrice"].DefaultValue = 0;
            dtbTemPd_Local.Columns["nPrice"].DefaultValue = 0;
            dtbTemPd_Local.Columns["nPriceInVAT"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd_Local);

            DataTable dtbTemStk_Local = new DataTable(this.mstrTemStk_Local);
            dtbTemStk_Local.Columns.Add("QcBranch", System.Type.GetType("System.String"));
            dtbTemStk_Local.Columns.Add("QnBranch", System.Type.GetType("System.String"));
            dtbTemStk_Local.Columns.Add("QTY1", System.Type.GetType("System.Decimal"));
            dtbTemStk_Local.Columns.Add("QTY2", System.Type.GetType("System.Decimal"));
            dtbTemStk_Local.Columns.Add("Total", System.Type.GetType("System.Decimal"), "QTY1+QTY2");

            dtbTemStk_Local.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemStk_Local.Columns.Add("Load_dDate", System.Type.GetType("System.DateTime"));

            dtbTemStk_Local.Columns["QcProd"].DefaultValue = "";
            dtbTemStk_Local.Columns["QcBranch"].DefaultValue = "";
            dtbTemStk_Local.Columns["QnBranch"].DefaultValue = "";
            dtbTemStk_Local.Columns["QTY1"].DefaultValue = 0;
            dtbTemStk_Local.Columns["QTY2"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemStk_Local);
        
        }

        private void pmCreatemPd(string inTableName)
        {
            DataTable dtbTemPd = new DataTable(inTableName);
            dtbTemPd.Columns.Add("PO_dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("PO_cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("PO_nQTY", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("PO_dDeliDate", System.Type.GetType("System.DateTime"));

            dtbTemPd.Columns.Add("SO_dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("SO_cRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("SO_QnCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("SO_QnEmpl", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("SO_nQTY", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("SO_dDeliDate", System.Type.GetType("System.DateTime"));

            dtbTemPd.Columns.Add("nStdPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPrice", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nPriceInVAT", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns.Add("RefType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("Load_Date", System.Type.GetType("System.DateTime"));

            dtbTemPd.Columns["PO_cRefNo"].DefaultValue = "";
            dtbTemPd.Columns["PO_nQTY"].DefaultValue = 0;
            dtbTemPd.Columns["SO_cRefNo"].DefaultValue = "";
            dtbTemPd.Columns["SO_QnCoor"].DefaultValue = "";
            dtbTemPd.Columns["SO_QnEmpl"].DefaultValue = "";
            dtbTemPd.Columns["SO_nQTY"].DefaultValue = 0;
            dtbTemPd.Columns["nStdPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nPrice"].DefaultValue = 0;
            dtbTemPd.Columns["nPriceInVAT"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemPd);
        
        }

        private void pmInitGridProp()
        {

            #region "Grid TemPd"

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];


            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns["PO_dDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["PO_cRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns["PO_cRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridView1.Columns["PO_dDeliDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns["SO_dDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["SO_cRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns["SO_cRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridView1.Columns["SO_dDeliDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["PO_dDate"].Caption = "วันที่ PO";
            this.gridView1.Columns["PO_dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["PO_dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["PO_cRefNo"].Caption = "เลขที่ PO";
            this.gridView1.Columns["PO_nQTY"].Caption = "จำนวนสั่งซื้อ";
            this.gridView1.Columns["PO_nQTY"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["PO_nQTY"].DisplayFormat.FormatString = "#,###,###";
            //this.gridView1.Columns["PO_nQTY"].DisplayFormat.FormatString = "n2";

            this.gridView1.Columns["PO_dDeliDate"].Caption = "สินค้าเข้าประมาณ";
            this.gridView1.Columns["PO_dDeliDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["PO_dDeliDate"].DisplayFormat.FormatString = "dd/MM/yy";


            this.gridView1.Columns["SO_dDate"].Caption = "วันที่ SO";
            this.gridView1.Columns["SO_dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["SO_dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["SO_cRefNo"].Caption = "เลขที่ SO";
            this.gridView1.Columns["SO_QnCoor"].Caption = "ลูกค้า";
            this.gridView1.Columns["SO_QnEmpl"].Caption = "พนักงานขาย";
            this.gridView1.Columns["SO_nQTY"].Caption = "จำนวนสั่งขาย";
            this.gridView1.Columns["SO_nQTY"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["SO_nQTY"].DisplayFormat.FormatString = "#,###,###";
            //this.gridView1.Columns["SO_nQTY"].DisplayFormat.FormatString = "n2";

            this.gridView1.Columns["SO_dDeliDate"].Caption = "วันที่ส่งสินค้า";
            this.gridView1.Columns["SO_dDeliDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["SO_dDeliDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["nStdPrice"].Caption = "ราคาขาย STD.";
            this.gridView1.Columns["nStdPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["nStdPrice"].DisplayFormat.FormatString = "#,###,###.00";
            this.gridView1.Columns["nStdPrice"].DisplayFormat.FormatString = "n2";

            this.gridView1.Columns["nPrice"].Caption = "ราคาขายก่อน VAT";
            this.gridView1.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPrice"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["nPriceInVAT"].Caption = "ราคาขายรวม VAT";
            this.gridView1.Columns["nPriceInVAT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["nPriceInVAT"].DisplayFormat.FormatString = "#,###,###.00";
            this.gridView1.Columns["nPriceInVAT"].DisplayFormat.FormatString = "n2";

            this.gridView1.Columns["nStdPrice"].Visible = false;
            this.gridView1.Columns["nPrice"].Visible = false;
            this.gridView1.Columns["nPriceInVAT"].Visible = false;

            this.gridView1.Columns["QcProd"].Visible = false;
            this.gridView1.Columns["RefType"].Visible = false;
            this.gridView1.Columns["Load_Date"].Visible = false;

            #endregion

            //this.grdStock.DataSource = this.dtsDataEnv.Tables[this.mstrTemStk];

            //this.gridView2.Columns["QcBranch"].Visible = false;

            //this.gridView2.Columns["QnBranch"].Caption = "Mfg. Location";
            //this.gridView2.Columns["QTY1"].Caption = "Office";
            //this.gridView2.Columns["QTY2"].Caption = "Main";
            //this.gridView2.Columns["Total"].Caption = "Total";

            //this.gridView2.Columns["QTY1"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView2.Columns["QTY1"].DisplayFormat.FormatString = "#,###,###";

            //this.gridView2.Columns["QTY2"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView2.Columns["QTY2"].DisplayFormat.FormatString = "#,###,###";

            //this.gridView2.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView2.Columns["Total"].DisplayFormat.FormatString = "#,###,###";

            //this.gridView2.Columns["QTY1"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum,"");
            //this.gridView2.Columns["QTY2"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "");
            //this.gridView2.Columns["Total"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "");

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new DialogForms.dlgGetBranch();
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "SHOWPICT":
                    //using (Common.dlgShowProdPict dlg = new MISCenter.Report.Common.dlgShowProdPict())
                    //{
                    //    dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    //    dlg.pmRefreshPic(this.mstrProdPict);
                    //    dlg.ShowDialog();
                    //}
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonCick(string inTextbox, string inPara1)
        {
            string strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
            string strTxtPrefix2 = StringHelper.Left(inTextbox.ToUpper(), 8);

            switch (inTextbox)
            {
                case "TXTQCPROD":
                case "TXTQNPROD":
                case "TXTQSPROD":
                    this.pmInitPopUpDialog("PROD");
                    this.pofrmGetProd.ValidateField("", (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTQCPROD":
                case "TXTQNPROD":
                case "TXTQSPROD":
                    if (this.pofrmGetProd != null)
                    {
                        DataRow dtrRow = this.pofrmGetProd.RetrieveValue();

                        if (dtrRow != null)
                        {
                            if (this.txtQcProd.Tag.ToString() != dtrRow["FCSKID"].ToString())
                            {
                                this.mstrProd = dtrRow["FCSKID"].ToString();
                                this.pmLoadFormData();
                                this.mintRowIndex = this.pmGetRowID(this.txtQcProd.Text.TrimEnd());
                            }
                            this.txtQcProd.Text = dtrRow["fcCode"].ToString().TrimEnd();
                            this.txtQnProd.Text = dtrRow["fcName"].ToString().TrimEnd();
                            this.txtQsProd.Text = dtrRow["fcSName"].ToString().TrimEnd();

                            //switch (inPopupForm.TrimEnd().ToUpper())
                            //{
                            //    case "TXTQCPROD":
                            //        this.txtQcProd.Focus();
                            //        break;
                            //    case "TXTQNPROD":
                            //        this.txtQnProd.Focus();
                            //        break;
                            //    case "TXTQSPROD":
                            //        this.txtQsProd.Focus();
                            //        break;
                            //}
                        }
                        else
                        {
                            this.pmBlankFormData();
                        }
                    }
                    break;
            }
        }

        private void txtQcProd_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            switch (txtPopUp.Name.ToUpper())
            {
                case "TXTQCPROD":
                    strOrderBy = "fcCode";
                    break;
                case "TXTQNPROD":
                    strOrderBy = "fcName";
                    break;
                case "TXTQSPROD":
                    strOrderBy = "fcSName";
                    break;
            }

            if (txtPopUp.Text == "")
            {
                this.txtQcProd.Tag = "";
                this.txtQcProd.Text = "";
                this.txtQnProd.Text = "";
                this.txtQsProd.Text = "";
                this.txtPrice1.Value = 0;
                this.txtPrice2.Value = 0;
                this.pmBlankFormData();
            }
            else
            {
                this.pmInitPopUpDialog("PROD");
                e.Cancel = !this.pofrmGetProd.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetProd.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }
        }

        private void frmStkBal01_Resize(object sender, EventArgs e)
        {
            //this.grdStock.Width = this.Width - this.grdStock.Left - 15;
            //this.grdTemPd.Width = this.Width - this.grdTemPd.Left - 15;
            //this.grdTemPd.Height = this.Height - this.grdTemPd.Top - 60;
        }

        private void btnRequery_Click(object sender, EventArgs e)
        {
            this.pmRequery(this.mstrProd, this.txtQcProd.Text.TrimEnd());
        }

        private int pmGetRowID(string inTag)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrTemQProd].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[this.mstrTemQProd].Rows[intCnt]["fcCode"].ToString().TrimEnd() == inTag.TrimEnd())
                    return intCnt;
            }
            return -1;
        }

        private void pmRequery(string inProd, string inQcProd)
        {

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Clear();
            //this.dtsDataEnv.Tables[this.mstrTemStk].Rows.Clear();

            this.dtspstK021.XRPSTK02.Rows.Clear();

            decimal decOrdQty_PO = 0;
            decimal decOrdQty_SO = 0;
            decimal decStkQty = 0;

            this.pmRequeryStk(inQcProd, ref decStkQty, false);

            this.pmQueryOrd2(inQcProd, "PO", ref decOrdQty_PO, false);
            this.txtQty_PO.Value = decOrdQty_PO;

            //this.pmQueryOrd(inQcProd, "SO", ref decOrdQty_SO, false);
            this.pmQueryOrd2(inQcProd, "SO", ref decOrdQty_SO, false);
            this.txtQty_SO.Value = decOrdQty_SO;

            //this.txtQty_Bal.Value = decStkQty + decOrdQty_PO - decOrdQty_SO;
            this.txtQty_Bal.Value = decStkQty - decOrdQty_SO;

            //this.dtsDataEnv.Tables[this.mstrTemStk].Rows.Clear();

        }

        private string[] pAWH = new string[] { "0001", "SR-1", "SR-2", "SR-7", "001", "ST01", "ST02", "ST03" };
        private string[] pAWH2 = new string[] { "WT-1", "WT-2", "WT-7" };
        private string mstrWHList = "'0001','SR-1', 'SR-2', 'SR-7' , 'WT-1' , 'WT-2' , 'WT-7'";

        private void pmRequeryStk(string inProd, ref decimal ioBal,bool inIsBuffer)
        {

            ioBal = 0;
            for (int intWH = 0; intWH < this.pAWH.Length; intWH++)
            {
                DataRow dtrTemWH = this.dtspstK021.XRPSTK02.NewRow();
                //DataRow dtrTemWH = this.dtsDataEnv.Tables[0].NewRow();

                dtrTemWH["QcWH1"] = this.pAWH[intWH];
                //dtrTemWH["QcWH2"] = this.pAWH2[intWH];

                #region "Buffer Process"

                IEnumerable<DataRow> query = from product in this.dtsDataEnv.Tables[this.mstrTemStk].AsEnumerable()
                                             where product.Field<string>("QcProd").TrimEnd() == inProd.TrimEnd()
                                                 && product.Field<string>("QcWHouse").TrimEnd() == this.pAWH[intWH].TrimEnd()
                                             select product;

                bool bllIsReLoad_Qty1 = false;
                
                decimal decQty1 = 0;    //Stock ของสาขา HO
                decimal decQty2 = 0;    //Stock ของสาขา วัดตึก

                DataRow dtrTemStk = null;
                //กรณีเจอ Stock ที่เคย Load มาแล้ว
                if (query.Count() > 0)
                {
                    TimeSpan ts = DateTime.Now - (Convert.ToDateTime(query.ToList()[0]["CreateDate"]));
                    DataRow[] sel1 = this.dtsDataEnv.Tables[this.mstrTemStk].Select("QcProd = '" + inProd + "' and QcWHouse = '" + this.pAWH[intWH] + "'");
                    dtrTemStk = sel1[0];
                    if (ts.Minutes > App.mint_BufferTimeout)
                    {
                        bllIsReLoad_Qty1 = true;
                    }
                    else
                    {
                        bllIsReLoad_Qty1 = false;
                        decQty1 = Convert.ToDecimal(dtrTemStk["Qty1"]);
                        decQty2 = Convert.ToDecimal(dtrTemStk["Qty2"]);
                    }
                }
                else
                {

                    //กรณีไม่เจอ Stock ที่เคย Load มาแล้ว
                    bllIsReLoad_Qty1 = true;

                    dtrTemStk = this.dtsDataEnv.Tables[this.mstrTemStk].NewRow();
                    dtrTemStk["QcProd"] = inProd;
                    dtrTemStk["QcWHouse"] = this.pAWH[intWH];
                    //dtrTemStk["QcWHouse2"] = this.pAWH2[intWH];
                    dtrTemStk["CreateDate"] = DateTime.Now;
                    this.dtsDataEnv.Tables[this.mstrTemStk].Rows.Add(dtrTemStk);
                }
                
                if (bllIsReLoad_Qty1)
                {
                    decQty1 = this.pmGetStkQty(this.pAWH[intWH], inProd);
                    //decQty2 = this.pmGetStkQty(this.pAWH2[intWH], inProd);
                }

                //dtrTemStk เป็น Buffer ที่ LocalDataSet
                dtrTemStk["Qty1"] = decQty1;
                dtrTemStk["Qty2"] = decQty2;

                //dtrTemWH เป็น Record ที่แสดงผลที่หน้าจอ
                dtrTemWH["Qty1"] = decQty1;
                dtrTemWH["Qty2"] = decQty2;

                #endregion

                if (!inIsBuffer)
                {
                    this.dtspstK021.XRPSTK02.Rows.Add(dtrTemWH);
                }
                
                ioBal += Convert.ToDecimal(dtrTemWH["Qty1"]) + Convert.ToDecimal(dtrTemWH["Qty2"]);

#if MY_DEBUG
                MessageBox.Show("After Query Stock\r\n" + this.pAWH[intWH] + "\r\n" + this.pAWH2[intWH]);
#endif
            }

        }

        private decimal pmGetStkQty(string inQcWHouse, string inProd)
        {
            decimal decBalQty = 0;

            //WS.Data.Agents.cDBMSAgent objSQLHelper = inSQLHelper;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(App.ERPConnectionString2);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "PMGETSTKQTY";

            cmd.CommandType = CommandType.StoredProcedure;

            string strQcCorp = App.ActiveCorp.Code.PadRight(15, ' ');
            string strQcWHouse = inQcWHouse.PadRight(App.xd_LEN_WHOUSE_CODE, ' ');
            string strQcProd = inProd.PadRight(App.xd_LEN_PROD_CODE, ' ');

            cmd.Parameters.AddWithValue("@inQcCorp", strQcCorp);
            cmd.Parameters.AddWithValue("@inQcWHouse", strQcWHouse);
            cmd.Parameters.AddWithValue("@inQcProd", strQcProd);
            cmd.Parameters.Add("@ioBalQty", System.Data.SqlDbType.Decimal).Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                //MessageBox.Show("Open Connection");
                int i = cmd.ExecuteNonQuery();
                if (!Convert.IsDBNull(cmd.Parameters["@ioBalQty"].Value))
                {
                    decBalQty = Convert.ToDecimal(cmd.Parameters["@ioBalQty"].Value);
                }
                cmd.Dispose();
                cmd = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return decBalQty;
        }

        private decimal pmGetStkBal(string inBranch, string inProd)
        { 

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //string strSQLExec = "select sum(PDBRANCH.FNQTY) as FNSUMQTY from PDBRANCH left join WHOUSE on WHOUSE.FCSKID = PDBRANCH.FCWHOUSE where PDBRANCH.FCBRANCH = ? and PDBRANCH.FCPROD = ? and WHOUSE.FCTYPE = ' ' ";
            //string strSQLExec = "select sum(PDBRANCH.FNQTY) as FNSUMQTY from PDBRANCH left join WHOUSE on WHOUSE.FCSKID = PDBRANCH.FCWHOUSE where PDBRANCH.FCPROD = ? and WHOUSE.FCTYPE = ' ' ";

            string strSQLExec = "select sum(PDBRANCH.FNQTY) as FNSUMQTY from PDBRANCH left join WHOUSE on WHOUSE.FCSKID = PDBRANCH.FCWHOUSE where PDBRANCH.FCPROD = ? " + " and WHOUSE.FCCODE in (" + this.mstrWHList + ") ";

            decimal decBalQty = 0;
            //objSQLHelper.SetPara(new object[] { inBranch, inProd });
            objSQLHelper.SetPara(new object[] { inProd });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPdBranch", "PDBRANCH", strSQLExec, ref strErrorMsg))
            {
                DataRow dtrValue = this.dtsDataEnv.Tables["QPdBranch"].Rows[0];
                if (!Convert.IsDBNull(dtrValue["fnSumQty"]))
                {
                    decBalQty = Convert.ToDecimal(dtrValue["fnSumQty"]);
                }
            }
            return decBalQty;
        }

        private decimal pmGetStkBal2(string inBranch, string inProd)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec = "select sum(PDBRANCH.FNQTY) as FNSUMQTY from PDBRANCH left join WHOUSE on WHOUSE.FCSKID = PDBRANCH.FCWHOUSE where PDBRANCH.FCBRANCH = ? and PDBRANCH.FCPROD = ? and WHOUSE.FCTYPE = ' ' ";

            decimal decBalQty = 0;
            objSQLHelper.SetPara(new object[] { inBranch, inProd });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPdBranch", "PDBRANCH", strSQLExec, ref strErrorMsg))
            {
                DataRow dtrValue = this.dtsDataEnv.Tables["QPdBranch"].Rows[0];
                if (!Convert.IsDBNull(dtrValue["fnSumQty"]))
                {
                    decBalQty = Convert.ToDecimal(dtrValue["fnSumQty"]);
                }
            }
            return decBalQty;
        }

        private void pmQueryOrd(string inQcProd, string inRefType, ref decimal ioQty, bool inIsBuffer)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            
            string strFld = "Book.fcCode, OrderH.fcSkid, OrderH.fdDate, OrderH.fcCode, OrderH.fcRefNo, OrderH.fdReceDate, OrderH.fnVatRate, OrderI.fdDelivery ";
            //strFld += " ,OrderI.fnQty * OrderI.fnUMQty as fnOrdQty, OrderI.fnPriceKe";
            strFld += " ,OrderI.fnBackQty * OrderI.fnUMQty as fnOrdQty, OrderI.fnPriceKe";
            strFld += " ,Coor.fcCode as QcCoor, Coor.fcName as QnCoor ";
            strFld += " ,Empl.fcCode as QcEmpl, Empl.fcName as QnEmpl ";
            strFld += " ,Prod.fcCode as QcProd, Prod.fcName as QnProd, Prod.fnPrice as fnStdPrice ";
            //strFld += " ,PdGrp.fcCode as QcPdGrp, PdGrp.fcName as QnPdGrp ";
            strFld += " ,UM.fcCode as QcUM, UM.fcName as QnUM ";

            string strSQLExec = "select " + strFld + " from OrderH ";
            strSQLExec += " left join OrderI on OrderI.fcOrderH = OrderH.fcSkid";
            strSQLExec += " left join Book on OrderH.fcBook = Book.fcSkid";
            strSQLExec += " left join Coor on OrderH.fcCoor = Coor.fcSkid";
            strSQLExec += " left join Empl on OrderH.fcEmpl = Empl.fcSkid";
            strSQLExec += " left join Prod on OrderI.fcProd = Prod.fcSkid";
            strSQLExec += " left join UM on OrderI.fcUM = UM.fcSkid";
            strSQLExec += " where OrderH.fcCorp = ?";
            //strSQLExec += " and OrderH.fcBranch = ?";
            strSQLExec += " and OrderH.fcRefType = ?";
            strSQLExec += " and OrderH.fcStat = ?";
            //15/9/54
            strSQLExec += " and OrderI.fcCorp = ?";
            strSQLExec += " and OrderI.fcRefType = ?";
            strSQLExec += " and OrderI.fcStat = ?";
            
            strSQLExec += " and OrderI.fcStep <> ? ";
            strSQLExec += " and OrderI.fcStep <> 'L' ";
            strSQLExec += " and OrderI.fcProd = ? ";
            strSQLExec += " order by OrderH.fdDate, OrderH.fcRefNo";

            ioQty = 0;
            decimal decOrdQty = 0;

            IEnumerable<DataRow> query = from product in this.dtsDataEnv.Tables[this.mstrTemPd_Local].AsEnumerable()
                                         where product.Field<string>("QcProd").TrimEnd() == inQcProd.TrimEnd()
                                             && product.Field<string>("RefType") == inRefType
                                         select product;

            bool bllIsReLoad_Qty1 = false;
            //กรณีเจอ Stock ที่เคย Load มาแล้ว
            if (query.Count() > 0)
            {
                TimeSpan ts = DateTime.Now - (Convert.ToDateTime(query.ToList()[0]["Load_Date"]));
                DataRow[] sel1 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].Select("QcProd = '" + inQcProd + "' and RefType = '" + inRefType + "'");
                if (ts.Minutes > App.mint_BufferTimeout)
                {
                    bllIsReLoad_Qty1 = true;
                    foreach (DataRow dtrDel in sel1)
                    {
                        this.dtsDataEnv.Tables[this.mstrTemPd_Local].Rows.Remove(dtrDel);
                    }
                }
                else
                {
                    bllIsReLoad_Qty1 = false;
                }
            }
            else
            {
                //กรณีไม่เจอ Stock ที่เคย Load มาแล้ว
                bllIsReLoad_Qty1 = true;
            }

            //objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtQcBranch.Tag.ToString(), inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_CUT_STOCK, this.txtQcProd.Tag.ToString() });

            string strProd = "";
            objSQLHelper.SetPara(new object[] { App.gcCorp, inQcProd });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLProd", "PROD", "select FCSKID from PROD where FCCORP = ? and FCCODE = ?", ref strErrorMsg))
            {
                strProd = this.dtsDataEnv.Tables["QLProd"].Rows[0]["FCSKID"].ToString();
            }

            if (bllIsReLoad_Qty1)
            {
                //15/9/54
                //objSQLHelper.SetPara(new object[] { App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_PAY, strProd });
                objSQLHelper.SetPara(new object[] { App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_PAY, strProd });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOrderH", "ORDERH", strSQLExec, ref strErrorMsg))
                {
                    //MessageBox.Show("Found Transaction " + inRefType + " row = " + this.dtsDataEnv.Tables["QOrderH"].Rows.Count.ToString());
                    foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                    {
                        DataRow dtrRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        dtrRow["RefType"] = inRefType;
                        dtrRow["QcProd"] = inQcProd;

                        if (inRefType == "PO")
                        {
                            dtrRow["PO_cRefNo"] = dtrOrderH["fcRefNo"].ToString();
                            dtrRow["PO_dDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]);
                            dtrRow["PO_nQty"] = Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                            if (!Convert.IsDBNull(dtrOrderH["fdDelivery"]))
                            {
                                dtrRow["PO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdDelivery"]);
                            }
                            else
                            {
                                dtrRow["PO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdReceDate"]);
                            }
                        }
                        else
                        {
                            dtrRow["SO_cRefNo"] = dtrOrderH["fcRefNo"].ToString().Trim();
                            dtrRow["SO_QnCoor"] = dtrOrderH["QnCoor"].ToString().Trim();
                            dtrRow["SO_QnEmpl"] = dtrOrderH["QnEmpl"].ToString().Trim();
                            dtrRow["SO_dDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]);
                            dtrRow["SO_nQty"] = Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                            decimal decVatRate = Convert.ToDecimal(dtrOrderH["fnVatRate"]);
                            decimal decPrice = Convert.ToDecimal(dtrOrderH["fnPriceKe"]);
                            decimal decStdPrice = Convert.ToDecimal(dtrOrderH["fnStdPrice"]);

                            dtrRow["nPrice"] = decPrice;
                            dtrRow["nPriceinVAT"] = Math.Round(decPrice + (decPrice * decVatRate / 100), 2);
                            dtrRow["nStdPrice"] = decStdPrice;

                            if (!Convert.IsDBNull(dtrOrderH["fdDelivery"]))
                            {
                                dtrRow["SO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdDelivery"]);
                            }
                            else
                            {
                                dtrRow["SO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdReceDate"]);
                            }

                        }
                        decOrdQty += Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                        if (inIsBuffer)
                        {
                            DataRow dtrRow2 = this.dtsDataEnv.Tables[this.mstrTemPd2].NewRow();
                            mCopyDataRow(dtrRow, ref dtrRow2);
                            this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Add(dtrRow2);
                        }
                        else
                        {
                            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrRow);
                        }
                    
                    }
                }

            }
            //if (strErrorMsg != string.Empty)
            //{
            //    MessageBox.Show(strErrorMsg);
            //}

            if (bllIsReLoad_Qty1)
            {
                DataRow[] sel2 = null;
                if (inIsBuffer)
                {
                    sel2 = this.dtsDataEnv.Tables[this.mstrTemPd2].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                }
                else
                {
                    sel2 = this.dtsDataEnv.Tables[this.mstrTemPd].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                }

                foreach (DataRow dtrSource in sel2)
                {
                    DataRow dtr1 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].NewRow();
                    mCopyDataRow(dtrSource, ref dtr1);
                    dtr1["RefType"] = inRefType;
                    dtr1["QcProd"] = inQcProd;
                    dtr1["Load_Date"] = DateTime.Now;
                    this.dtsDataEnv.Tables[this.mstrTemPd_Local].Rows.Add(dtr1);
                }
            }
            else
            {
                DataRow[] sel2 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                foreach (DataRow dtrSource in sel2)
                {
                    DataRow dtr1 = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    mCopyDataRow(dtrSource, ref dtr1);
                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtr1);
                    decOrdQty += Convert.ToDecimal(dtr1[(inRefType == "SO" ? "SO_nQty" : "PO_nQty")]);
                }
            }
            
            ioQty = decOrdQty;

        }

        private void pmQueryOrd2(string inQcProd, string inRefType, ref decimal ioQty, bool inIsBuffer)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLExec = "exec PMGETORDER ?,? ";

            ioQty = 0;
            decimal decOrdQty = 0;

            IEnumerable<DataRow> query = from product in this.dtsDataEnv.Tables[this.mstrTemPd_Local].AsEnumerable()
                                         where product.Field<string>("QcProd").TrimEnd() == inQcProd.TrimEnd()
                                             && product.Field<string>("RefType") == inRefType
                                         select product;

            bool bllIsReLoad_Qty1 = false;
            //กรณีเจอ Stock ที่เคย Load มาแล้ว
            if (query.Count() > 0)
            {
                TimeSpan ts = DateTime.Now - (Convert.ToDateTime(query.ToList()[0]["Load_Date"]));
                DataRow[] sel1 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].Select("QcProd = '" + inQcProd + "' and RefType = '" + inRefType + "'");
                if (ts.Minutes > App.mint_BufferTimeout)
                {
                    bllIsReLoad_Qty1 = true;
                    foreach (DataRow dtrDel in sel1)
                    {
                        this.dtsDataEnv.Tables[this.mstrTemPd_Local].Rows.Remove(dtrDel);
                    }
                }
                else
                {
                    bllIsReLoad_Qty1 = false;
                }
            }
            else
            {
                //กรณีไม่เจอ Stock ที่เคย Load มาแล้ว
                bllIsReLoad_Qty1 = true;
            }

            //objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtQcBranch.Tag.ToString(), inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_CUT_STOCK, this.txtQcProd.Tag.ToString() });

            string strProd = "";
            objSQLHelper.SetPara(new object[] { App.gcCorp, inQcProd });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLProd", "PROD", "select FCSKID from PROD where FCCORP = ? and FCCODE = ?", ref strErrorMsg))
            {
                strProd = this.dtsDataEnv.Tables["QLProd"].Rows[0]["FCSKID"].ToString();
            }

            if (bllIsReLoad_Qty1)
            {
                //15/9/54
                //objSQLHelper.SetPara(new object[] { App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_PAY, strProd });
                //objSQLHelper.SetPara(new object[] { App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, App.gcCorp, inRefType, SysDef.gc_STAT_NORMAL, SysDef.gc_REF_STEP_PAY, strProd });
                objSQLHelper.SetPara(new object[] { strProd, inRefType });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOrderH", "ORDERH", strSQLExec, ref strErrorMsg))
                {
                    //MessageBox.Show("Found Transaction " + inRefType + " row = " + this.dtsDataEnv.Tables["QOrderH"].Rows.Count.ToString());
                    foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                    {
                        DataRow dtrRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        dtrRow["RefType"] = inRefType;
                        dtrRow["QcProd"] = inQcProd;

                        if (inRefType == "PO")
                        {
                            dtrRow["PO_cRefNo"] = dtrOrderH["fcRefNo"].ToString();
                            dtrRow["PO_dDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]);
                            dtrRow["PO_nQty"] = Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                            if (!Convert.IsDBNull(dtrOrderH["fdDelivery"]))
                            {
                                dtrRow["PO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdDelivery"]);
                            }
                            else
                            {
                                dtrRow["PO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdReceDate"]);
                            }
                        }
                        else
                        {
                            dtrRow["SO_cRefNo"] = dtrOrderH["fcRefNo"].ToString().Trim();
                            dtrRow["SO_QnCoor"] = dtrOrderH["QnCoor"].ToString().Trim();
                            dtrRow["SO_QnEmpl"] = dtrOrderH["QnEmpl"].ToString().Trim();
                            dtrRow["SO_dDate"] = Convert.ToDateTime(dtrOrderH["fdDate"]);
                            dtrRow["SO_nQty"] = Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                            decimal decVatRate = Convert.ToDecimal(dtrOrderH["fnVatRate"]);
                            decimal decPrice = Convert.ToDecimal(dtrOrderH["fnPriceKe"]);
                            decimal decStdPrice = Convert.ToDecimal(dtrOrderH["fnStdPrice"]);

                            dtrRow["nPrice"] = decPrice;
                            dtrRow["nPriceinVAT"] = Math.Round(decPrice + (decPrice * decVatRate / 100), 2);
                            dtrRow["nStdPrice"] = decStdPrice;

                            if (!Convert.IsDBNull(dtrOrderH["fdDelivery"]))
                            {
                                dtrRow["SO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdDelivery"]);
                            }
                            else
                            {
                                dtrRow["SO_dDeliDate"] = Convert.ToDateTime(dtrOrderH["fdReceDate"]);
                            }

                        }
                        decOrdQty += Convert.ToDecimal(dtrOrderH["fnOrdQty"]);

                        if (inIsBuffer)
                        {
                            DataRow dtrRow2 = this.dtsDataEnv.Tables[this.mstrTemPd2].NewRow();
                            mCopyDataRow(dtrRow, ref dtrRow2);
                            this.dtsDataEnv.Tables[this.mstrTemPd2].Rows.Add(dtrRow2);
                        }
                        else
                        {
                            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrRow);
                        }

                    }
                }

            }
            //if (strErrorMsg != string.Empty)
            //{
            //    MessageBox.Show(strErrorMsg);
            //}

            if (bllIsReLoad_Qty1)
            {
                DataRow[] sel2 = null;
                if (inIsBuffer)
                {
                    sel2 = this.dtsDataEnv.Tables[this.mstrTemPd2].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                }
                else
                {
                    sel2 = this.dtsDataEnv.Tables[this.mstrTemPd].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                }

                foreach (DataRow dtrSource in sel2)
                {
                    DataRow dtr1 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].NewRow();
                    mCopyDataRow(dtrSource, ref dtr1);
                    dtr1["RefType"] = inRefType;
                    dtr1["QcProd"] = inQcProd;
                    dtr1["Load_Date"] = DateTime.Now;
                    this.dtsDataEnv.Tables[this.mstrTemPd_Local].Rows.Add(dtr1);
                }
            }
            else
            {
                DataRow[] sel2 = this.dtsDataEnv.Tables[this.mstrTemPd_Local].Select("QcProd = '" + inQcProd.TrimEnd() + "' and RefType = '" + inRefType + "'");
                foreach (DataRow dtrSource in sel2)
                {
                    DataRow dtr1 = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    mCopyDataRow(dtrSource, ref dtr1);
                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtr1);
                    decOrdQty += Convert.ToDecimal(dtr1[(inRefType == "SO" ? "SO_nQty" : "PO_nQty")]);
                }
            }

            ioQty = decOrdQty;

        }

        private void mCopyDataRow(DataRow inSource, ref DataRow inDest)
        {
            for (int intCnt = 0; intCnt < inSource.Table.Columns.Count; intCnt++)
            {

                if (intCnt > inDest.Table.Columns.Count - 1)
                    break;

                DataColumn dtcUpdStruc = inSource.Table.Columns[intCnt];
                DataColumn dtcDestCol = inDest.Table.Columns[intCnt];
                if (dtcDestCol.ReadOnly == true)
                    continue;

                switch (dtcUpdStruc.DataType.ToString().ToUpper())
                {
                    case "SYSTEM.STRING":
                        inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? "" : inSource[dtcUpdStruc].ToString());
                        break;
                    case "SYSTEM.INT16":
                        inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : (int)inSource[dtcUpdStruc]);
                        break;
                    case "SYSTEM.INT32":
                        inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToInt32(inSource[dtcUpdStruc]));
                        break;
                    case "SYSTEM.INT64":
                        inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToInt64(inSource[dtcUpdStruc]));
                        break;
                    case "SYSTEM.DECIMAL":
                        inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToDecimal(inSource[dtcUpdStruc]));
                        break;
                    case "SYSTEM.DATETIME":
                        if (Convert.IsDBNull(inSource[dtcUpdStruc]) == false)
                            inDest[dtcDestCol] = Convert.ToDateTime(inSource[dtcUpdStruc]);
                        break;
                }
            }
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "Print":
                        //this.pmPrintData();
                        break;
                    case "Search":
                        this.pmPopUpButtonCick("TXTQCPROD", "");
                        break;
                    case "Exit":
                        this.Close();
                        break;
                    case "Requery":
                        this.pmRequery(this.mstrProd, this.txtQcProd.Text.TrimEnd());
                        break;
                    case "Go_Next":
                        this.pmBlankFormData();
                        this.pmGoRecord(1);
                        break;
                    case "Go_Prev":
                        this.pmBlankFormData();
                        this.pmGoRecord(-1);
                        break;
                    case "ShowPict":
                        this.pmInitPopUpDialog("ShowPict");
                        break;
                    case "Clear":
                        this.pmBlankFormData();
                        break;
                }
            }
        }

        private void pmGoRecord(int inRec)
        {
            if (this.mintRowIndex + inRec > this.mintRowCount-1)
            {
                this.mintRowIndex = 0;
            }
            else if (this.mintRowIndex + inRec < 0)
            {
                this.mintRowIndex = this.mintRowCount-1;
            }
            else
            {
                this.mintRowIndex = this.mintRowIndex + inRec;
            }

            if (this.mintRowIndex > this.dtsDataEnv.Tables[this.mstrTemQProd].Rows.Count
                || this.mintRowIndex < 0)
            {
                this.mintRowIndex = this.dtsDataEnv.Tables[this.mstrTemQProd].Rows.Count;
            }

            this.mstrProd = this.dtsDataEnv.Tables[this.mstrTemQProd].Rows[this.mintRowIndex]["fcSkid"].ToString();
            this.pmLoadFormData();
        }

        private void frmStkBal01_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F9:
                    this.pmBlankFormData();
                    break;
                case Keys.Escape:
                    this.pmBlankFormData();
                    this.Close();
                    break;
            }
        }

        private void btnShowPict_Click(object sender, EventArgs e)
        {
            this.pmInitPopUpDialog("ShowPict");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int intRowProc = this.mintRowIndex + 5;
            for (int i = this.mintRowIndex; i < intRowProc; i++)
            {
                if (i < this.dtsDataEnv.Tables[this.mstrTemQProd].Rows.Count - 1 && i >= 0)
                {
                    string strQcProd = this.dtsDataEnv.Tables[this.mstrTemQProd].Rows[i]["fcCode"].ToString();

                    decimal decOrdQty_PO = 0;
                    decimal decOrdQty_SO = 0;
                    decimal decStkQty = 0;
                    this.pmRequeryStk(strQcProd, ref decStkQty, true);
                    //this.pmQueryOrd(strQcProd, "PO", ref decOrdQty_PO, true);
                    //this.pmQueryOrd(strQcProd, "SO", ref decOrdQty_SO, true);
                
                }
            }
        }

    }
}
