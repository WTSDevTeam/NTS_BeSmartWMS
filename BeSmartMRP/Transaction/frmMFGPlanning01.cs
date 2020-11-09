
//#define xd_RUNMODE_DEBUG

using System;
using System.Collections;
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

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using BeSmartMRP.DatabaseForms;

namespace BeSmartMRP.Transaction
{
    public partial class frmMFGPlanning01 : UIHelper.frmBase
    {

        public static string TASKNAME = "EMORDER";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        
        private bool mbllFilterResult = false;

        private string mstrEditRowID = "";
        private string mstrSaveRowID = "";

        private string mstrRefType = DocumentType.MO.ToString();
        private string mstrRefToRefType = DocumentType.SO.ToString();
        private string mstrRfType = "M";
        private DocumentType mRefType = DocumentType.MO;

        private string mstrPlant = "";
        private string mstrBranch = "";
        private string mstrBook = "";
        private string mstrQcBook = "";

        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private DateTime mdttPlan_BegDate = DateTime.Now;
        private DateTime mdttPlan_EndDate = DateTime.Now;

        private int mintDayCount = 0;
        
        private string mstrDefaSQL_MOrderH = "";
        private string mstrTemMOrderOP = "TemMOrderOP";
        private string mstrTemWorkCal = "TemWorkCal";
        private string mstrTemWorkCenter = "TemWorkCenter";
        private string mstrTemWkCtrCap = "TemWkCtrCap";
        private string mstrTemOPPlanCap = "TemOPPlanCap";

        private string mstrTemMapField = "TemMapField";

        public frmMFGPlanning01()
        {
            InitializeComponent();
            this.pmCreateTem();
            this.pmInitForm();
        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
            {
                base.OnClosing(ce);
                App.ActivateMainScreen();
            }
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "Do you want to close this form ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");

            this.mbllFilterResult = false;

            this.mstrDefaSQL_MOrderH = "select CROWID from MFWORDERHD";
            this.mstrDefaSQL_MOrderH += " where MFWORDERHD.CCORP = ?";
            this.mstrDefaSQL_MOrderH += " and MFWORDERHD.CBRANCH = ? ";
            this.mstrDefaSQL_MOrderH += " and MFWORDERHD.CPLANT = ? ";
            this.mstrDefaSQL_MOrderH += " and MFWORDERHD.CREFTYPE = ? ";
            this.mstrDefaSQL_MOrderH += " and MFWORDERHD.CMFGBOOK = ? ";
            this.mstrDefaSQL_MOrderH += " and MFWORDERHD.DDATE between ? and ? ";

            this.pmInitializeComponent();
            this.pmFilterForm();
            //this.pmGotoBrowPage();

            //this.pmInitGridProp();
            //this.gridView1.MoveLast();

            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            //UIHelper.UIBase.CreateTransactionToolbar(this.barMainEdit, this.barMain);
            //this.pmSetMaxLength();
            //this.pmMapEvent();
            this.pmSetFormUI();


            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetFormUI()
        {

            //this.mstrFormMenuName = UIBase.GetAppUIText(new string[] { "เอกสารใบสั่งผลิต [MO]", "MANUFACTURING ORDER [MO]" });
            //this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            //this.lblCode.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Doc. Code" }) });
            //this.lblRefNo.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." }) });
            //this.lblDate.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Doc. Date" }) });

        }

        private void pmCreateTem()
        {

            DataTable dtbTemWorkCal = new DataTable(this.mstrTemWorkCal);

            dtbTemWorkCal.Columns.Add("CTYPE", System.Type.GetType("System.String"));
            dtbTemWorkCal.Columns.Add("DDATE", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DBEGTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DENDTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("DCURRTIME", System.Type.GetType("System.DateTime"));
            dtbTemWorkCal.Columns.Add("TotWkHour", System.Type.GetType("System.Decimal"));
            dtbTemWorkCal.Columns.Add("BalWkHour", System.Type.GetType("System.Decimal"));
            dtbTemWorkCal.Columns.Add("CDATE", System.Type.GetType("System.String"));

            dtsDataEnv.Tables.Add(dtbTemWorkCal);

            //Table Mapping Filed ระหว่าง date กับ column

        }

        private void pmCreateTem_Planning()
        {

            if (dtsDataEnv.Tables[this.mstrTemMOrderOP] != null)
            {
                dtsDataEnv.Tables.Remove(this.mstrTemMOrderOP);
            }

            if (dtsDataEnv.Tables[this.mstrTemMapField] != null)
            {
                dtsDataEnv.Tables.Remove(this.mstrTemMapField);
            }

            if (dtsDataEnv.Tables[this.mstrTemWkCtrCap] != null)
            {
                dtsDataEnv.Tables.Remove(this.mstrTemWkCtrCap);
            }

            if (dtsDataEnv.Tables[this.mstrTemOPPlanCap] != null)
            {
                dtsDataEnv.Tables.Remove(this.mstrTemOPPlanCap);
            }

            DataTable dtbTemMapField = new DataTable(this.mstrTemMapField);
            dtbTemMapField.Columns.Add("cField", System.Type.GetType("System.String"));
            dtbTemMapField.Columns.Add("cDate", System.Type.GetType("System.String"));
            dtbTemMapField.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtsDataEnv.Tables.Add(dtbTemMapField);

            DataTable dtbTemPd = new DataTable(this.mstrTemMOrderOP);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("LineType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QcWorkCtr", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("Code_MO", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("StartDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("DueDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("OPSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("OPName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("WkCtrH", System.Type.GetType("System.String"));

            DataTable dtbTemWkCtrCap = new DataTable(this.mstrTemWkCtrCap);
            dtbTemWkCtrCap.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemWkCtrCap.Columns.Add("QcWorkCtr", System.Type.GetType("System.String"));
            dtbTemWkCtrCap.Columns.Add("QnWorkCtr", System.Type.GetType("System.String"));
            dtbTemWkCtrCap.Columns.Add("CapPerHour", System.Type.GetType("System.Decimal"));

            DataTable dtbTemOPPlanCap = new DataTable(this.mstrTemOPPlanCap);
            dtbTemOPPlanCap.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemOPPlanCap.Columns.Add("QcWorkCtr", System.Type.GetType("System.String"));
            dtbTemOPPlanCap.Columns.Add("Code_MO", System.Type.GetType("System.String"));
            dtbTemOPPlanCap.Columns.Add("StartDate", System.Type.GetType("System.DateTime"));
            dtbTemOPPlanCap.Columns.Add("DueDate", System.Type.GetType("System.DateTime"));
            dtbTemOPPlanCap.Columns.Add("OPSeq", System.Type.GetType("System.String"));
            dtbTemOPPlanCap.Columns.Add("OPName", System.Type.GetType("System.String"));
            dtbTemOPPlanCap.Columns.Add("WkCtrH", System.Type.GetType("System.String"));

            DateTime dttStartTime = this.mdttPlan_BegDate;
            string strMthDay = "Day_";
            for (int i = 0; i < this.mintDayCount; i++)
            {
                string strFieldName = strMthDay + i.ToString("00");
                dttStartTime = this.mdttPlan_BegDate.AddDays(i);
                dtbTemPd.Columns.Add(strFieldName, System.Type.GetType("System.Decimal"));
                dtbTemWkCtrCap.Columns.Add(strFieldName, System.Type.GetType("System.Decimal"));
                dtbTemOPPlanCap.Columns.Add(strFieldName, System.Type.GetType("System.Decimal"));

                dtbTemPd.Columns[strFieldName].DefaultValue = 0;
                dtbTemWkCtrCap.Columns[strFieldName].DefaultValue = 0;
                dtbTemOPPlanCap.Columns[strFieldName].DefaultValue = 0;

                this.pmMapField(strFieldName, dttStartTime);
            }

            dtsDataEnv.Tables.Add(dtbTemOPPlanCap);
            dtsDataEnv.Tables.Add(dtbTemWkCtrCap);
            dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void pmMapField(string inFieldName, DateTime inDate)
        {
            DataRow dtrMapField = dtsDataEnv.Tables[this.mstrTemMapField].NewRow();
            dtrMapField["cField"] = inFieldName;
            dtrMapField["dDate"] = inDate;
            dtrMapField["cDate"] = inDate.ToString("yyyyMMdd");
            dtsDataEnv.Tables[this.mstrTemMapField].Rows.Add(dtrMapField);
        }

        private void pmFilterForm()
        {
            this.pmInitPopUpDialog("FILTER");
        }

        private void pmInitPopUpDialog(string inDialogName)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrGetVal = null;
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Common.dlgFilter02 dlgFilter = new Common.dlgFilter02(this.mstrRefType))
                    {

                        dlgFilter.SetTitle(this.Text, TASKNAME);
                        dlgFilter.BegDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dlgFilter.EndDate = dlgFilter.BegDate.AddMonths(1).AddDays(-1);
                        
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrPlant = dlgFilter.PlantID;
                            this.mstrBranch = dlgFilter.BranchID;
                            this.mstrBook = dlgFilter.BookID;
                            this.mstrQcBook = dlgFilter.BookCode;
                            this.mdttBegDate = dlgFilter.BegDate.Date;
                            this.mdttEndDate = dlgFilter.EndDate.Date;

                            this.pmLoadPlanningTable();
                            
                            this.grdBrowView.Focus();

                        }
                    }
                    break;
            }
        }

        private void pmLoadPlanningTable()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QMfgBook", "BOOK", "select * from MFGBOOK where cRowID = ? ", ref strErrorMsg))
            //{ }

            this.mdttPlan_BegDate = pmLoadRangeDate("MIN");
            this.mdttPlan_EndDate = pmLoadRangeDate("MAX");

            TimeSpan ts = this.mdttPlan_EndDate - this.mdttPlan_BegDate;
            this.mintDayCount = Convert.ToInt32(ts.TotalDays);

            this.pmLoadWorkCalendar(this.mdttPlan_BegDate, this.mdttPlan_EndDate);
            this.pmCreateTem_Planning();

            this.pmLoadWorkCenterItem();
            this.pmLoadOPPlan();

            //Load TemMOrderOP จาก mstrTemOPPlanCap และ WkCtr Item เพื่อแสดงผลหน้าจอ
            
            //this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrTemMOrderOP];
            this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Clear();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrTemMOrderOP];


            DataView dv = this.dtsDataEnv.Tables[this.mstrTemOPPlanCap].DefaultView;
            dv.Sort = "QcWorkCtr";

            if (dv.Count > 0)
            {
                string strMthDay = "";
                string strCurrWkCtr = dv[0]["QcWorkCtr"].ToString();
                //int[] sumArray
                DataRow dtrSumLine = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                foreach (DataRowView dtrOPCap in dv)
                {
                    //dtrSumLine = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();

                    if (strCurrWkCtr != dtrOPCap["QcWorkCtr"].ToString())
                    {
                        //Insert Sum Line

                        DataRow dtrSumLine_All = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                        DataRow dtrSumLine_Bal = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                        dtrSumLine["LineType"] = "S2";
                        dtrSumLine["QcWorkCtr"] = strCurrWkCtr;
                        dtrSumLine["Code_MO"] = "SubTotal Cap : (" + strCurrWkCtr + ")";

                        dtrSumLine_All["LineType"] = "S1";
                        dtrSumLine_All["QcWorkCtr"] = strCurrWkCtr;
                        dtrSumLine_All["Code_MO"] = "Available : (" + strCurrWkCtr + ")";

                        dtrSumLine_Bal["LineType"] = "S3";
                        dtrSumLine_Bal["QcWorkCtr"] = strCurrWkCtr;
                        dtrSumLine_Bal["Code_MO"] = "Diff : (" + strCurrWkCtr + ")";

                        strMthDay = "Day_";
                        for (int i = 0; i < this.mintDayCount; i++)
                        {
                            string strFieldName = strMthDay + i.ToString("00");
                            DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemWkCtrCap].Select("QcWorkCtr = '" + strCurrWkCtr + "'");
                            if (da.Length > 0)
                            {
                                dtrSumLine_All[strFieldName] = Convert.ToDecimal(da[0][strFieldName]);
                            }
                            dtrSumLine_Bal[strFieldName] = Convert.ToDecimal(dtrSumLine_All[strFieldName]) - Convert.ToDecimal(dtrSumLine[strFieldName]);
                        }

                        this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine_All);
                        this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine);
                        this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine_Bal);

                        dtrSumLine = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();

                        strCurrWkCtr = dtrOPCap["QcWorkCtr"].ToString();

                    }

                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                    dtrNewRow["LineType"] = "D1";
                    dtrNewRow["QcWorkCtr"] = dtrOPCap["QcWorkCtr"].ToString();
                    dtrNewRow["Code_MO"] = dtrOPCap["Code_MO"].ToString();
                    dtrNewRow["StartDate"] = Convert.ToDateTime(dtrOPCap["StartDate"]);
                    dtrNewRow["DueDate"] = Convert.ToDateTime(dtrOPCap["DueDate"]);
                    this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrNewRow);

                    DateTime dttStartTime = this.mdttPlan_BegDate;
                    strMthDay = "Day_";
                    for (int i = 0; i < this.mintDayCount; i++)
                    {
                        string strFieldName = strMthDay + i.ToString("00");
                        dtrNewRow[strFieldName] = Convert.ToDecimal(dtrOPCap[strFieldName]);
                        dtrSumLine[strFieldName] = Convert.ToDecimal(dtrSumLine[strFieldName]) + Convert.ToDecimal(dtrOPCap[strFieldName]);
                        //dttStartTime = this.mdttPlan_BegDate.AddDays(i);
                        //this.pmMapField(strFieldName, dttStartTime);
                    }


                }
                if (dtrSumLine != null)
                {
                    DataRow dtrSumLine_All = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                    DataRow dtrSumLine_Bal = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                    dtrSumLine["LineType"] = "S2";
                    dtrSumLine["QcWorkCtr"] = strCurrWkCtr;
                    dtrSumLine["Code_MO"] = "SubTotal Cap : (" + strCurrWkCtr + ")";

                    dtrSumLine_All["LineType"] = "S1";
                    dtrSumLine_All["QcWorkCtr"] = strCurrWkCtr;
                    dtrSumLine_All["Code_MO"] = "Available : (" + strCurrWkCtr + ")";

                    dtrSumLine_Bal["LineType"] = "S3";
                    dtrSumLine_Bal["QcWorkCtr"] = strCurrWkCtr;
                    dtrSumLine_Bal["Code_MO"] = "Diff : (" + strCurrWkCtr + ")";

                    strMthDay = "Day_";
                    for (int i = 0; i < this.mintDayCount; i++)
                    {
                        string strFieldName = strMthDay + i.ToString("00");
                        DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemWkCtrCap].Select("QcWorkCtr = '" + strCurrWkCtr + "'");
                        if (da.Length > 0)
                        {
                            dtrSumLine_All[strFieldName] = Convert.ToDecimal(da[0][strFieldName]);
                        }
                        dtrSumLine_Bal[strFieldName] = Convert.ToDecimal(dtrSumLine_All[strFieldName]) - Convert.ToDecimal(dtrSumLine[strFieldName]);
                    }

                    this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine_All);
                    this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine);
                    this.dtsDataEnv.Tables[this.mstrTemMOrderOP].Rows.Add(dtrSumLine_Bal);

                    //dtrSumLine = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].NewRow();
                }

            }


            pmInitGridProp_TemCap();


            //this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemMapField];
            //this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemWorkCal];
            //this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemOPPlanCap];
            //this.dataGridView2.DataSource = this.dtsDataEnv.Tables[this.mstrTemWorkCal];
            //this.dataGridView3.DataSource = this.dtsDataEnv.Tables[this.mstrTemWkCtrCap];

        }

        private void pmInitGridProp_TemCap()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemMOrderOP].DefaultView;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrTemMOrderOP];

            this.gridView1.OptionsView.AllowCellMerge = true;

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gridView1.Columns[intCnt].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            this.gridView1.Columns["QcWorkCtr"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;

            this.gridView1.Columns["QcWorkCtr"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView1.Columns["Code_MO"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView1.Columns["StartDate"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridView1.Columns["DueDate"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            int i = 0;
            //this.gridView1.Columns["QcWorkCtr"].
            this.gridView1.Columns["QcWorkCtr"].VisibleIndex = i++;
            this.gridView1.Columns["Code_MO"].VisibleIndex = i++;
            this.gridView1.Columns["StartDate"].VisibleIndex = i++;
            this.gridView1.Columns["DueDate"].VisibleIndex = i++;

            this.gridView1.Columns["StartDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["StartDate"].DisplayFormat.FormatString = "dd/MM/yy";
            this.gridView1.Columns["DueDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["DueDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["QcWorkCtr"].Caption = "W/C";
            this.gridView1.Columns["Code_MO"].Caption = "MO";
            this.gridView1.Columns["StartDate"].Caption = "Start Date";
            this.gridView1.Columns["DueDate"].Caption = "Due Date";

            DateTime dttStartTime = this.mdttPlan_BegDate;
            string strMthDay = "Day_";
            for (int j = 0; j < this.mintDayCount; j++)
            {
                string strFieldName = strMthDay + j.ToString("00");
                this.gridView1.Columns[strFieldName].VisibleIndex = i++;
                dttStartTime = this.mdttPlan_BegDate.AddDays(j);
                this.gridView1.Columns[strFieldName].Caption = dttStartTime.ToString("dd/MM");
                this.gridView1.Columns[strFieldName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                this.gridView1.Columns[strFieldName].DisplayFormat.FormatString = "###,###,##00.00;-###,###,##00.00; ";
            
            }

              this.gridView1.Columns["Code_MO"].Width = 120;

            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            styleFormatCondition1.Appearance.BackColor = Color.Green;
            styleFormatCondition1.Appearance.ForeColor = Color.White;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Column = this.gridView1.Columns["LineType"];
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "S1";

            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            styleFormatCondition2.Appearance.BackColor = Color.Yellow;
            styleFormatCondition2.Appearance.ForeColor = Color.Black;
            styleFormatCondition2.Appearance.Options.UseBackColor = true;
            styleFormatCondition2.Appearance.Options.UseForeColor = true;
            styleFormatCondition2.Column = this.gridView1.Columns["LineType"];
            styleFormatCondition2.ApplyToRow = true;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = "S2";

            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            styleFormatCondition3.Appearance.BackColor = Color.Blue;
            styleFormatCondition3.Appearance.ForeColor = Color.White;
            styleFormatCondition3.Appearance.Options.UseBackColor = true;
            styleFormatCondition3.Appearance.Options.UseForeColor = true;
            styleFormatCondition3.Column = this.gridView1.Columns["LineType"];
            styleFormatCondition3.ApplyToRow = true;
            styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition3.Value1 = "S3";

            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] { styleFormatCondition1, styleFormatCondition2, styleFormatCondition3 });

        }

        private void pmLoadOPPlan()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select ";
            strSQLStr += " MFWORDERIT_OPPLAN.* ";
            strSQLStr += " , MFGBOOK.CCODE as QCBOOK";
            strSQLStr += " , MFWKCTRHD.CCODE as QCWORKCTR, MFWKCTRHD.CNAME as QNWORKCTR";
            strSQLStr += " , MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DSTART, MFWORDERHD.DDUEDATE ";
            strSQLStr += " from MFWORDERIT_OPPLAN ";
            strSQLStr += " left join MFWORDERHD on MFWORDERHD.CROWID = MFWORDERIT_OPPLAN.CWORDERH ";
            strSQLStr += " left join MFGBOOK on MFGBOOK.CROWID = MFWORDERHD.CMFGBOOK ";
            strSQLStr += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWORDERIT_OPPLAN.CWKCTRH ";
            strSQLStr += "where MFWORDERIT_OPPLAN.CWORDERH in (" + this.mstrDefaSQL_MOrderH + " ) ";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadOPPlan", "MFWORDERIT_OPPLAN", strSQLStr, ref strErrorMsg))
            {
                //Loop for Load Planning
                foreach (DataRow dtrWkCtr in this.dtsDataEnv.Tables["QLoadOPPlan"].Rows)
                {
                    this.pmAddRow_OPPlanCap(dtrWkCtr);
                }
            }

        
        }

        private DateTime pmLoadRangeDate(string inOrder)
        {
            DateTime dttRetVal = DateTime.Now;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select top 1 DDATE from MFWORDERIT_OPPLAN where CWORDERH in (" + this.mstrDefaSQL_MOrderH + " ) order by " + (inOrder == "MIN" ? "DDATE asc" : "DDATE desc");

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGetDate", "MFWORDERIT_OPPLAN", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrOPPlan = this.dtsDataEnv.Tables["QGetDate"].Rows[0];
                DateTime dttCalDate = Convert.ToDateTime(dtrOPPlan["DDATE"]);
                int intDay = 1;
                if (inOrder == "MIN")
                {
                    intDay = (dttCalDate.Day < 15 ? 1 : 15);
                    dttRetVal = new DateTime(dttCalDate.Year, dttCalDate.Month, intDay);
                }
                else
                {
                    dttRetVal = new DateTime(dttCalDate.Year, dttCalDate.Month, 1).AddMonths(1).AddDays(-1);
                }

            }

            return dttRetVal;
        }

        private void pmLoadWorkCenterItem()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "select * from MFWKCTRHD where CROWID in (select CWKCTRH from MFWORDERIT_OPPLAN where CWORDERH in (" + this.mstrDefaSQL_MOrderH + " ) group by CWKCTRH)";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QLoadWkCtr", "MFWKCTRHD", strSQLStr, ref strErrorMsg))
            {
                //Loop for Load WorkCenter Cap
                foreach (DataRow dtrWkCtr in this.dtsDataEnv.Tables["QLoadWkCtr"].Rows)
                {
                    decimal decCapPerHour = Convert.ToDecimal(dtrWkCtr["NCAPACITY"]);

                    DataRow dtrNewWkCtr = this.dtsDataEnv.Tables[this.mstrTemWkCtrCap].NewRow();
                    dtrNewWkCtr["QcWorkCtr"] = dtrWkCtr["cCode"].ToString();
                    dtrNewWkCtr["QnWorkCtr"] = dtrWkCtr["cName"].ToString();
                    dtrNewWkCtr["CapPerHour"] = decCapPerHour;
                    this.dtsDataEnv.Tables[this.mstrTemWkCtrCap].Rows.Add(dtrNewWkCtr);

                    foreach (DataRow dtrWorkCal in this.dtsDataEnv.Tables[this.mstrTemWorkCal].Rows)
                    {
                        decimal decWorkHour = Convert.ToDecimal(dtrWorkCal["TotWkHour"]);
                        decimal decCapPerDay = decCapPerHour * decWorkHour;

                        //DataRow[] dtaSel = this.dtsDataEnv.Tables[mstrTemMapField].Select("CDATE = '" + dtrWorkCal["CDATE"].ToString()+ "'");
                        string strFldName = this.pmGetMapField(dtrWorkCal["CDATE"].ToString());
                        if (strFldName.Trim() != string.Empty)
                        {
                            //string strFldName = dtaSel[0]["cField"].ToString();
                            dtrNewWkCtr[strFldName] = Convert.ToDecimal(dtrNewWkCtr[strFldName]) + decCapPerDay;
                        }

                    }
                }

            }

        }

        private string pmGetMapField(string inCDate)
        {
            string strFldName = "";
            DataRow[] dtaSel = this.dtsDataEnv.Tables[mstrTemMapField].Select("CDATE = '" + inCDate + "'");
            if (dtaSel.Length > 0)
            {
                strFldName = dtaSel[0]["cField"].ToString();
            }
            return strFldName;
        }

        private void pmLoadWorkCalendar(DateTime inBegDate, DateTime inEndDate)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            int intYear = inBegDate.Year;

            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.mstrPlant });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select * from EMPLANT where cRowID = ? ", ref strErrorMsg))
            {

                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                string strWorkHour = dtrPlant["cWorkHour"].ToString();
                string strHoliday = dtrPlant["cHoliday"].ToString();

                string strSQLStr = "select * from EMWORKCALENDAR";
                strSQLStr += " where EMWORKCALENDAR.CCORP = ? and EMWORKCALENDAR.CPLANT = ? ";
                strSQLStr += " and EMWORKCALENDAR.CWORKHOUR = ? and EMWORKCALENDAR.NYEAR = ?";

                string strSQLStr2 = "select * from EMWORKCALENDARIT_RANGE where CWORKCALH = ? and DDATE between ? and ? order by DDATE,CSEQ";

                //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant, strWorkHour, strHoliday, intYear });
                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant, strWorkHour, intYear });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWorkCal", "EMWORKCALENDAR", strSQLStr, ref strErrorMsg))
                {
                    DataRow dtrWorkCal = this.dtsDataEnv.Tables["QWorkCal"].Rows[0];
                    objSQLHelper.SetPara(new object[] { dtrWorkCal["cRowID"], inBegDate, inEndDate });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWorkCalIT_Range", "EMWORKCALENDARIT_RANGE", strSQLStr2, ref strErrorMsg))
                    {
                        //DataRow dtrWorkCal = this.dtsDataEnv.Tables["QWorkCal"].Rows[0];
                        foreach (DataRow dtrWorkCal_Range in this.dtsDataEnv.Tables["QWorkCalIT_Range"].Rows)
                        {
                            pmAddRow_WkHour(dtrWorkCal_Range["cType"].ToString(), Convert.ToDateTime(dtrWorkCal_Range["dDate"]), Convert.ToDateTime(dtrWorkCal_Range["dBegTime"]), Convert.ToDateTime(dtrWorkCal_Range["dEndTime"]));
                        }

                    }
                }


            }

        }

        private void pmAddRow_WkHour(string inType, DateTime inDate, DateTime inBegDate, DateTime inEndDate)
        {

            DataRow dr = this.dtsDataEnv.Tables[mstrTemWorkCal].NewRow();
            dr["CTYPE"] = inType;
            dr["DDATE"] = inDate;
            dr["DBEGTIME"] = inBegDate;
            dr["DENDTIME"] = inEndDate;
            dr["DCURRTIME"] = inEndDate;
            dr["CDATE"] = inDate.ToString("yyyyMMdd");

            TimeSpan ts = inEndDate - inBegDate;
            dr["TotWkHour"] = Convert.ToDecimal(ts.TotalHours);
            dr["BalWkHour"] = Convert.ToDecimal(ts.TotalHours);

            this.dtsDataEnv.Tables[mstrTemWorkCal].Rows.Add(dr);
        }

        private void pmAddRow_OPPlanCap(DataRow inSource)
        {

            DataRow dr = this.dtsDataEnv.Tables[this.mstrTemOPPlanCap].NewRow();
            dr["QcWorkCtr"] = inSource["QcWorkCtr"].ToString();
            dr["Code_MO"] = "MO" + inSource["QcBook"].ToString().Trim() + "/" + inSource["CCODE"].ToString();
            dr["StartDate"] = Convert.ToDateTime(inSource["dStart"]);
            dr["DueDate"] = Convert.ToDateTime(inSource["dDueDate"]);

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            string strSQLStr = "select * from MFWKCTRHD where CROWID = ?";

            //CWKCTRH
            decimal decCapPerHour = 0;
            objSQLHelper.SetPara(new object[] { inSource["CWKCTRH"].ToString() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWkCtr", "MFWKCTRHD", strSQLStr, ref strErrorMsg))
            {
                //Loop for Load WorkCenter Cap
                foreach (DataRow dtrWkCtr in this.dtsDataEnv.Tables["QWkCtr"].Rows)
                {
                    decCapPerHour = Convert.ToDecimal(dtrWkCtr["NCAPACITY"]);
                }
            }

            string strFldName = this.pmGetMapField(Convert.ToDateTime(inSource["DDATE"]).ToString("yyyyMMdd"));
            if (strFldName.Trim() != string.Empty)
            {
                dr[strFldName] = decCapPerHour * Convert.ToDecimal(inSource["nUseHour"]);
            }

            this.dtsDataEnv.Tables[mstrTemOPPlanCap].Rows.Add(dr);
        }

    }
}
