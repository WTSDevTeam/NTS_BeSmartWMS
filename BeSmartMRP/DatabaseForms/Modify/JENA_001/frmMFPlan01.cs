
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
using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;


namespace BeSmartMRP.DatabaseForms.Modify.JENA_001
{
    public partial class frmMFPlan01 : UIHelper.frmBase
    {


        public static string TASKNAME = "ERESTYPE";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";

        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrHTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFPlanItem;
        //private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = QMFResTypeInfo.Field.Code;
        private string mstrFixType = "";
        private string mstrRefType = DocumentType.MO.ToString();

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrTemCap = "TemCap";
        private string mstrTemAllPdCap = "TemAllPdCap";
        private string mstrTemPlan = "TemPlan";

        private string mstrOldCode = "";
        private string mstrOldName = "";

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;
        private bool mbllFilterResult = false;

        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private DatabaseForms.frmMFWorkCenter pofrmGetWkCtr = null;

        private string mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;
        private MfgResourceType mResourceType = MfgResourceType.Machine;
        private string mstrFormMenuName = "";

        private string mstrPlant = "";
        private string mstrBranch = "";
        private string mstrBook = "";
        private string mstrQcBook = "";
        private string mstrPdType = "";
        private string mstrStdUM = "";
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        private string mstrDefaFGWHouse = "";
        private string mstrDefaWRWHouse = "";
        private string mstrDefaRWWHouse = "";
        private string mstrDefaWHouseBuy = "";

        private string mstrDefaSect = "";
        private string mstrDefaJob = "";

        private string mstrBook_WHouse_FG = "";
        private string mstrBook_WHouse_RM = "";

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmMFPlan01()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmMFPlan01(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmMFPlan01 mInstanse_1 = null;

        public static frmMFPlan01 GetInstanse(MfgResourceType inResourceType)
        {

            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmMFPlan01();
            }
            return mInstanse_1;
        }

        private static void pmClearInstanse(MfgResourceType inResourceType)
        {
            if (mInstanse_1 != null)
            {
                mInstanse_1 = null;
            }
        }

        public FormActiveMode ActiveMode
        {
            get { return this.mFormActiveMode; }
            set { this.mFormActiveMode = value; }
        }

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public MfgResourceType ResourceType
        {
            get { return this.mResourceType; }
            set { this.mResourceType = value; }
        }

        public void ReInitForm()
        {
            this.pmSetFormUI();
            this.pmInitGridProp();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");

            this.mbllFilterResult = false;

            this.pmCreateTem();
            this.pmInitGridProp_TemCap();
            this.pmInitGridProp_TemPlan();

            this.pmInitializeComponent();
            this.pmFilterForm();

            this.pmGotoBrowPage();

            this.mFormEditMode = UIHelper.AppFormState.Edit;
            this.pmLoadEditPage();

            this.pmInitGridProp();
            this.gridView1.MoveLast();
            
            UIBase.WaitClear();

            if (this.mFormActiveMode == FormActiveMode.Edit)
            {
                this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
                if (!this.mbllFilterResult)
                {
                    this.Close();
                    //frmBudAllocate.pmClearInstanse();
                }
            }

        }

        private void pmInitializeComponent()
        {

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                this.ShowInTaskbar = false;
                this.ControlBox = false;
            }

            UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmMapEvent();

            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetFormUI()
        {
            //this.lblCode.Text = this.mstrFormMenuName + " Code :";
            //this.lblName.Text = this.mstrFormMenuName + " Name :";
            //this.lblName2.Text = this.mstrFormMenuName + " Name 2 :";

            //this.Text = this.mstrFormMenuName + " Items";
            //TASKNAME = "ERESTYPE_" + this.mstrType;
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

        }

        private void pmMapEvent()
        {
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.pgfMainEdit.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.pgfMainEdit_SelectedPageChanged);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.gridView3.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView3_FocusedRowChanged);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView3_ValidatingEditor);

        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemCap);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cQcWkCtr", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("cDate", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDesc", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nStdCap", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nReqCap", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nBalCap", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nResCount", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemPd.Columns["nStdCap"].DefaultValue = 0;
            dtbTemPd.Columns["nReqCap"].DefaultValue = 0;
            //dtbTemPd.Columns["nBalCap"].DefaultValue = 0;
            dtbTemPd.Columns["nBalCap"].Expression = "nStdCap-nReqCap";
            dtbTemPd.Columns["nResCount"].DefaultValue = 0;

            //dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemCost_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemAllPdCap = new DataTable(this.mstrTemAllPdCap);
            dtbTemAllPdCap.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemAllPdCap.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemAllPdCap.Columns.Add("cSeq", System.Type.GetType("System.String"));
            dtbTemAllPdCap.Columns.Add("cQcWkCtr", System.Type.GetType("System.String"));
            dtbTemAllPdCap.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemAllPdCap.Columns.Add("cDate", System.Type.GetType("System.String"));
            dtbTemAllPdCap.Columns.Add("cDesc", System.Type.GetType("System.String"));
            dtbTemAllPdCap.Columns.Add("nStdCap", System.Type.GetType("System.Decimal"));
            dtbTemAllPdCap.Columns.Add("nReqCap", System.Type.GetType("System.Decimal"));
            dtbTemAllPdCap.Columns.Add("nBalCap", System.Type.GetType("System.Decimal"));
            dtbTemAllPdCap.Columns.Add("nResCount", System.Type.GetType("System.Decimal"));
            dtbTemAllPdCap.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemAllPdCap.Columns["cRowID"].DefaultValue = "";
            dtbTemAllPdCap.Columns["nDummyFld"].DefaultValue = 0;
            dtbTemAllPdCap.Columns["nStdCap"].DefaultValue = 0;
            dtbTemAllPdCap.Columns["nReqCap"].DefaultValue = 0;
            dtbTemAllPdCap.Columns["nBalCap"].DefaultValue = 0;
            dtbTemAllPdCap.Columns["nResCount"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemAllPdCap);
            
            DataTable dtbTemPlan = new DataTable(this.mstrTemPlan);
            dtbTemPlan.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("dMO_Date", System.Type.GetType("System.DateTime"));
            dtbTemPlan.Columns.Add("cMO_Code", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("nMO_Qty", System.Type.GetType("System.Decimal"));
            dtbTemPlan.Columns.Add("cWOrderH", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cWOrderOP", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcRM", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cRemark1", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cRemark2", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cRemark3", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("nBalStk", System.Type.GetType("System.Decimal"));
            dtbTemPlan.Columns.Add("dDueDate", System.Type.GetType("System.DateTime"));
            dtbTemPlan.Columns.Add("cOPSeq", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cOPRDetail", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cMOPR", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcMOPR", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQnMOPR", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cWkCtrH", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcWkCtrH", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQnWkCtrH", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cQcOPSeq", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("cStartDate", System.Type.GetType("System.String"));
            dtbTemPlan.Columns.Add("dStartDate", System.Type.GetType("System.DateTime"));
            dtbTemPlan.Columns.Add("dFinishDate", System.Type.GetType("System.DateTime"));
            dtbTemPlan.Columns.Add("nQty1", System.Type.GetType("System.Decimal"));
            dtbTemPlan.Columns.Add("nOldQty1", System.Type.GetType("System.Decimal"));
            dtbTemPlan.Columns.Add("nQty2", System.Type.GetType("System.Decimal"));
            dtbTemPlan.Columns.Add("nTotQty", System.Type.GetType("System.Decimal"));

            dtbTemPlan.Columns["nQty1"].DefaultValue = 0;
            dtbTemPlan.Columns["nOldQty1"].DefaultValue = 0;
            dtbTemPlan.Columns["nQty2"].DefaultValue = 0;
            dtbTemPlan.Columns["cStartDate"].DefaultValue = "";

            this.dtsDataEnv.Tables.Add(dtbTemPlan);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select MFMCType.CROWID, MFMCType.CCODE, MFMCType.CNAME, MFMCType.CNAME2, MFMCType.DCREATE, MFMCType.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} MFMCType ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = MFMCType.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = MFMCType.CLASTUPDBY ";
            strSQLExec += " where MFMCType.CCORP = ? and MFMCType.CTYPE = ? and 0=1";
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            if (this.mstrFixType != string.Empty)
            {
                this.mstrType = this.mstrFixType;
            }

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrType });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "MFMCType", strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
        }

        private void pmInitGridProp_TemCap()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemCap].DefaultView;

            this.grdTemCap.DataSource = this.dtsDataEnv.Tables[this.mstrTemCap];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
                this.gridView2.Columns[intCnt].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gridView2.Columns[intCnt].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            }

            int i = 0;
            this.gridView2.Columns["cDesc"].VisibleIndex = i++;
            this.gridView2.Columns["nStdCap"].VisibleIndex = i++;
            this.gridView2.Columns["nReqCap"].VisibleIndex = i++;
            this.gridView2.Columns["nBalCap"].VisibleIndex = i++;
            this.gridView2.Columns["nResCount"].VisibleIndex = i++;

            this.gridView2.Columns["cDesc"].Caption = " ";
            this.gridView2.Columns["nStdCap"].Caption = "มาตรฐาน";
            this.gridView2.Columns["nReqCap"].Caption = "ต้องการ";
            this.gridView2.Columns["nBalCap"].Caption = "คงเหลือ";
            this.gridView2.Columns["nResCount"].Caption = "จำนวน\r\n(เครื่อง)";

            this.gridView2.Columns["cDesc"].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 204, 153);

            this.gridView2.Columns["nStdCap"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nReqCap"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nBalCap"].ColumnEdit = this.grcCapQty;
            this.gridView2.Columns["nResCount"].ColumnEdit = this.grcCapQty;

            this.gridView2.Columns["cDesc"].Width = 85;

            //this.pmCalcColWidth();

            //this.pmAddRowTemCap("OT4 ทุ่ม", 0, 0, 0, 0);
            //this.pmAddRowTemCap("OT3 ทุ่ม", 285, 3056, -2771, 0);
            //this.pmAddRowTemCap("OT2 ทุ่ม", 255, 3056, -2801, 0);
            //this.pmAddRowTemCap("OT1 ทุ่ม", 225, 3056, -2831, 0);
            //this.pmAddRowTemCap("8 ชม.", 180, 3056, -2876, 0);

        }

        private void pmAddRowTemCap(string inSeq, string inQcWkCtr, string inDesc, decimal inStdQty, decimal inReqCap, decimal inBalCap, decimal inResCnt)
        {
            DataRow dtrTemCap = this.dtsDataEnv.Tables[this.mstrTemCap].NewRow();
            dtrTemCap["cSeq"] = inSeq;
            dtrTemCap["cQcWkCtr"] = inQcWkCtr;
            dtrTemCap["cDesc"] = inDesc;
            dtrTemCap["nStdCap"] = inStdQty;
            dtrTemCap["nReqCap"] = inReqCap;
            dtrTemCap["nBalCap"] = inBalCap;
            dtrTemCap["nResCount"] = inResCnt;
            this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Add(dtrTemCap);
        }

        private void pmAddRowTemAllCap(string inSeq, string inQcWkCtr, string inDesc, decimal inStdQty, decimal inReqCap, decimal inBalCap, decimal inResCnt)
        {
            DataRow dtrTemCap = this.dtsDataEnv.Tables[this.mstrTemAllPdCap].NewRow();
            dtrTemCap["cSeq"] = inSeq;
            dtrTemCap["cQcWkCtr"] = inQcWkCtr;
            dtrTemCap["cDesc"] = inDesc;
            dtrTemCap["nStdCap"] = inStdQty;
            dtrTemCap["nReqCap"] = inReqCap;
            dtrTemCap["nBalCap"] = inBalCap;
            dtrTemCap["nResCount"] = inResCnt;
            this.dtsDataEnv.Tables[this.mstrTemAllPdCap].Rows.Add(dtrTemCap);
        }

        private void pmInitGridProp_TemPlan()
        {
            
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPlan].DefaultView;

            this.grdTemPlan.DataSource = this.dtsDataEnv.Tables[this.mstrTemPlan];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
                this.gridView3.Columns[intCnt].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gridView3.Columns[intCnt].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
                this.gridView3.Columns[intCnt].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                this.gridView3.Columns[intCnt].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
                this.gridView3.Columns[intCnt].OptionsColumn.AllowEdit = false;
            }

            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            styleFormatCondition1.Appearance.BackColor = Color.WhiteSmoke;
            styleFormatCondition1.Appearance.ForeColor = Color.WhiteSmoke;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Column = this.gridView3.Columns["cRemark1"];
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "$$$$$$$";

            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            styleFormatCondition2.Appearance.BackColor = Color.Yellow;
            //styleFormatCondition2.Appearance.ForeColor = Color.WhiteSmoke;
            styleFormatCondition2.Appearance.Options.UseBackColor = true;
            styleFormatCondition2.Appearance.Options.UseForeColor = true;
            styleFormatCondition2.Column = this.gridView3.Columns["dStartDate"];
            styleFormatCondition2.ApplyToRow = false;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = null;

            this.gridView3.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] { styleFormatCondition1, styleFormatCondition2 });

            int i = 0;
            this.gridView3.Columns["dMO_Date"].VisibleIndex = i++;
            this.gridView3.Columns["cMO_Code"].VisibleIndex = i++;
            this.gridView3.Columns["cQcCoor"].VisibleIndex = i++;
            this.gridView3.Columns["cQcProd"].VisibleIndex = i++;
            this.gridView3.Columns["nMO_Qty"].VisibleIndex = i++;
            this.gridView3.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView3.Columns["nBalStk"].VisibleIndex = i++;
            this.gridView3.Columns["dDueDate"].VisibleIndex = i++;
            //this.gridView3.Columns["cRemark2"].VisibleIndex = i++;
            //this.gridView3.Columns["cQcRM"].VisibleIndex = i++;
            this.gridView3.Columns["cRemark3"].VisibleIndex = i++;
            this.gridView3.Columns["cOPSeq"].VisibleIndex = i++;
            this.gridView3.Columns["cQcMOPR"].VisibleIndex = i++;
            this.gridView3.Columns["cQcWkCtrH"].VisibleIndex = i++;
            this.gridView3.Columns["dStartDate"].VisibleIndex = i++;
            this.gridView3.Columns["dFinishDate"].VisibleIndex = i++;
            this.gridView3.Columns["nQty1"].VisibleIndex = i++;
            //this.gridView3.Columns["nQty2"].VisibleIndex = i++;
            //this.gridView3.Columns["nTotQty"].VisibleIndex = i++;

            this.gridView3.Columns["dMO_Date"].Caption = "วันที่ MO";
            this.gridView3.Columns["cMO_Code"].Caption = "เลขที่ MO";
            this.gridView3.Columns["cQcCoor"].Caption = "รหัสลูกค้า";
            this.gridView3.Columns["cQcProd"].Caption = "รหัสสินค้า";
            this.gridView3.Columns["nMO_Qty"].Caption = "MO Q'ty";
            this.gridView3.Columns["cRemark1"].Caption = "หมายเหตุ";
            this.gridView3.Columns["nBalStk"].Caption = "ของเหลือ\r\nในคลัง";
            this.gridView3.Columns["dDueDate"].Caption = "กำหนดส่งจริง";
            this.gridView3.Columns["cRemark2"].Caption = "STATUS";
            this.gridView3.Columns["cQcRM"].Caption = "วัตถุดิบ";
            this.gridView3.Columns["cOPSeq"].Caption = "OP";
            this.gridView3.Columns["cQcMOPR"].Caption = "OPR";
            this.gridView3.Columns["cQcWkCtrH"].Caption = "W/C";
            this.gridView3.Columns["dStartDate"].Caption = "เวลาเริ่ม";
            this.gridView3.Columns["dFinishDate"].Caption = "เวลาเสร็จ";
            this.gridView3.Columns["cRemark3"].Caption = "หมายเหตุ";
            this.gridView3.Columns["nQty1"].Caption = "Cap.\r\nConsume";
            //this.gridView3.Columns["nQty2"].Caption = "Q'ty 2";
            //this.gridView3.Columns["nTotQty"].Caption = "Total";

            this.gridView3.Columns["dMO_Date"].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridView3.Columns["cMO_Code"].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;

            this.gridView3.Columns["cOPSeq"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["cQcMOPR"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["cQcWkCtrH"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["dStartDate"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["dFinishDate"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["cRemark3"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["nQty1"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["nQty2"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.Columns["nTotQty"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.OptionsView.AllowCellMerge = true;

            this.gridView3.Columns["dStartDate"].OptionsColumn.AllowEdit = true;
            this.gridView3.Columns["dFinishDate"].OptionsColumn.AllowEdit = true;

            this.gridView3.Columns["dStartDate"].ColumnEdit = this.grcStartDate;
            this.gridView3.Columns["dFinishDate"].ColumnEdit = this.grcFinishDate;

            this.gridView3.Columns["dStartDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["dStartDate"].DisplayFormat.FormatString = "dd/MM/yy";
            this.gridView3.Columns["dFinishDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["dFinishDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView3.Columns["dMO_Date"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["dMO_Date"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView3.Columns["nMO_Qty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nMO_Qty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;
            this.gridView3.Columns["nBalStk"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nBalStk"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView3.Columns["nQty1"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty1"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;
            this.gridView3.Columns["nQty2"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nQty2"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

            this.gridView3.Columns["nTotQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nTotQty"].DisplayFormat.FormatString = App.ActiveCorp.QtyFormatString;

        }

        private void pmAddRowTemPlan(
            string inRowID
            , string inWorderH
            , string inWorderOP
            , object inMO_Date
            , string inMO_Code
            , string inQcCoor
            , string inQcProd
            , decimal inMO_Qty
            , decimal inOPQty
            , string inRemark1
            , decimal inBalStk
            , object inDueDate
            , string inRemark2
            , string inQcRM
            , string inOPSeq
            , string inQcMOPR
            , string inWkCtrH
            , string inQcWkCtrH
            , DateTime inStartDate
            , DateTime inFinishDate
            , string inRemark3
            , string inPlanID
            )
        {

            DataRow dtrTemPlan = this.dtsDataEnv.Tables[this.mstrTemPlan].NewRow();

            if (inMO_Date != null)
            {
                dtrTemPlan["dMO_Date"] = Convert.ToDateTime(inMO_Date);
            }
            else
            {
                dtrTemPlan["dMO_Date"] = Convert.DBNull;
            }
            dtrTemPlan["cRowID"] = inRowID;
            dtrTemPlan["cWOrderH"] = inWorderH;
            dtrTemPlan["cWOrderOP"] = inWorderOP;
            dtrTemPlan["cMO_Code"] = inMO_Code;
            dtrTemPlan["cQcCoor"] = inQcCoor;
            dtrTemPlan["cQcProd"] = inQcProd;
            dtrTemPlan["nMO_Qty"] = inMO_Qty;
            dtrTemPlan["nQty1"] = inOPQty;

            if (inRowID.Trim() != string.Empty)
            {
                dtrTemPlan["nOldQty1"] = inOPQty;
            }

            dtrTemPlan["cRemark1"] = inRemark1;
            dtrTemPlan["nBalStk"] = inBalStk;

            if (inDueDate != null)
            {
                dtrTemPlan["dDueDate"] = Convert.ToDateTime(inDueDate);
            }
            else
            {
                dtrTemPlan["dDueDate"] = Convert.DBNull;
            }
            
            dtrTemPlan["cRemark2"] = inRemark2;
            dtrTemPlan["cQcRM"] = inQcRM;
            dtrTemPlan["cOPSeq"] = inOPSeq;
            dtrTemPlan["cQcMOPR"] = inQcMOPR;
            dtrTemPlan["cWkCtrH"] = inWkCtrH;
            dtrTemPlan["cQcWkCtrH"] = inQcWkCtrH;

            if (inStartDate == DateTime.MinValue)
            {
                dtrTemPlan["dStartDate"] = Convert.DBNull;
                dtrTemPlan["cStartDate"] = "";
            }
            else
            {
                dtrTemPlan["dStartDate"] = inStartDate;
                dtrTemPlan["cStartDate"] = inStartDate.ToString("yyyyMMdd");
            }

            if (inFinishDate == DateTime.MinValue)
            {
                dtrTemPlan["dFinishDate"] = Convert.DBNull;
            }
            else
            {
                dtrTemPlan["dFinishDate"] = inFinishDate;
            }
            
            dtrTemPlan["cRemark3"] = inRemark3;

            this.dtsDataEnv.Tables[this.mstrTemPlan].Rows.Add(dtrTemPlan);

        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.RefreshData();
            }

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count;intCnt++ )
            {
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
            }
            this.mstrSortKey = inColumn.ToUpper();
            this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
            this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
        }

        private void pgfMainEdit_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            this.pmSetToolbarState(pgfMainEdit.SelectedTabPageIndex);

            if (this.mFormActiveMode == FormActiveMode.PopUp
                || this.mFormActiveMode == FormActiveMode.Report)
            {
                if (pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
                {
                    this.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.Width - 20, AppUtil.CommonHelper.SysMetric(9) + 5);
                }
                else 
                {
                    this.Location = new Point(Convert.ToInt16((AppUtil.CommonHelper.SysMetric(1) - this.Width) / 2), Convert.ToInt16((AppUtil.CommonHelper.SysMetric(2) - this.Height) / 2));
                }
            }

        }

        private void pmSetToolbarState(int inActivePage)
        {
            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = (this.mFormActiveMode == FormActiveMode.PopUp ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);

            if (this.mFormActiveMode == FormActiveMode.Report)
            {
                this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Update.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
            this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);

            this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
            this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);
        }

        private void pmFilterForm()
        {
            this.pmInitPopUpDialog("FILTER");
        }

        private void pmGotoBrowPage()
        {
            this.pmBlankFormData();
            this.pmRefreshBrowView();

            this.pmSetPageStatus(false);
            this.pgfMainEdit.TabPages[0].PageEnabled = true;
            this.pgfMainEdit.SelectedTabPageIndex = 0;
            this.grdBrowView.Focus();
        }

        private void pmRefreshBrowView()
        {
            this.mintSaveBrowViewRowIndex = this.gridView1.FocusedRowHandle;
            this.pmSetBrowView();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            if (this.gridView1.RowCount > this.mintSaveBrowViewRowIndex)
                this.gridView1.FocusedRowHandle = this.mintSaveBrowViewRowIndex;

        }

        private void pmSetPageStatus(bool inIsEnable)
        {
            for (int intCnt = 0; intCnt < this.pgfMainEdit.TabPages.Count; intCnt++)
            {
                this.pgfMainEdit.TabPages[intCnt].PageEnabled = inIsEnable;
            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrGetVal = null;

            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Transaction.Common.dlgFilter02 dlgFilter = new Transaction.Common.dlgFilter02(DocumentType.MO.ToString()))
                    {

                        dlgFilter.SetTitle(this.Text, TASKNAME);
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

                            //this.mstrDefaSect = (oMfgBookInfo.Department.Tag.TrimEnd() != string.Empty ? oMfgBookInfo.Department.Tag : (oPlantInfo.Department.RowID.TrimEnd() != string.Empty) ? oPlantInfo.Department.RowID : (App.ActiveCorp.DefaultSectID.TrimEnd() != string.Empty) ? App.ActiveCorp.DefaultSectID.TrimEnd() : "");
                            //this.mstrDefaJob = (oMfgBookInfo.Job.Tag.TrimEnd() != string.Empty ? oMfgBookInfo.Job.Tag : (oPlantInfo.Job.RowID.TrimEnd() != string.Empty) ? oPlantInfo.Job.RowID : (App.ActiveCorp.DefaultJobID.TrimEnd() != string.Empty) ? App.ActiveCorp.DefaultJobID.TrimEnd() : "");

                            this.mstrDefaSect = App.ActiveCorp.DefaultSectID;
                            this.mstrDefaJob = App.ActiveCorp.DefaultJobID;

                            //this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" }) + "\\" + dlgFilter.PlantCode + "\\" + dlgFilter.BookCode;

                            objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                            objSQLHelper.SetPara(new object[] { this.mstrBook });
                            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QMfgBook", "BOOK", "select * from MFGBOOK where cRowID = ? ", ref strErrorMsg))
                            {
                                DataRow dtrMfgBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                                this.mstrBook_WHouse_FG = dtrMfgBook[QMfgBookInfo.Field.WHouse_FG].ToString().TrimEnd();
                                this.mstrBook_WHouse_RM = dtrMfgBook[QMfgBookInfo.Field.WHouse_RM].ToString().TrimEnd();
                            }

                            this.pmRefreshBrowView();
                            this.grdBrowView.Focus();

                        }
                    }
                    break;

                case "SECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new DialogForms.dlgGetSect();
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WKCTR":
                    if (this.pofrmGetWkCtr == null)
                    {
                        this.pofrmGetWkCtr = new frmMFWorkCenter(FormActiveMode.PopUp);
                        this.pofrmGetWkCtr.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWkCtr.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonClick(string inTextbox, string inPara1)
        {
            string strPrefix = "";
            switch (inTextbox)
            {
                case "TXTQCSECT":
                case "TXTQNSECT":
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            DataRow dtrGetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "TXTQCSECT":
                case "TXTQNSECT":

                    break;

            }
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
                        this.pmEnterForm();
                        break;
                    case WsToolBar.Insert:
                        //if (App.PermissionManager.CheckPermission(AuthenType.CanInsert, App.AppUserID, TASKNAME))
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanInsert, App.AppUserName, App.AppUserID))
                        {
                            this.mFormEditMode = UIHelper.AppFormState.Insert;
                            this.pmLoadEditPage();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการเพิ่มข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Update:
                        //if (App.PermissionManager.CheckPermission(AuthenType.CanEdit, App.AppUserID, TASKNAME))
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
                        {
                            this.mFormEditMode = UIHelper.AppFormState.Edit;
                            this.pmLoadEditPage();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Delete:
                        //if (App.PermissionManager.CheckPermission(AuthenType.CanDelete, App.AppUserID, TASKNAME))
                        if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                        {
                            this.pmDeleteData();
                        }
                        else
                            MessageBox.Show("ไม่มีสิทธิ์ในการลบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        break;
                    case WsToolBar.Search:
                        this.pmSearchData();
                        break;
                    case WsToolBar.Undo:
                        //if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        //    this.pmGotoBrowPage();
                        this.pmKeyboard_Esc();
                        break;
                    case WsToolBar.Save:
                        this.pmSaveData();
                        break;
                    case WsToolBar.Refresh:
                        this.pmRefreshBrowView();
                        break;
                }

            }
        }

        private void pmDeleteData()
        {
            string strErrorMsg = "";
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (this.gridView1.RowCount == 0 || dtrBrow == null)
                return;

            this.mstrEditRowID = dtrBrow["cRowID"].ToString();
            string strDeleteDesc = "(" + dtrBrow[QMFResTypeInfo.Field.Code].ToString().TrimEnd() + ") " + dtrBrow[QMFResTypeInfo.Field.Name].ToString().TrimEnd();
            if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow[QMFResTypeInfo.Field.Code].ToString(), ref strErrorMsg))
            {
                if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow[QMFResTypeInfo.Field.Code].ToString(), dtrBrow[QMFResTypeInfo.Field.Name].ToString(), ref strErrorMsg))
                    {
                        this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
                    }
                }
            }
            else
            {
                if (strErrorMsg != "")
                    MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.mstrEditRowID = "";
        }

        private bool pmCheckHasUsed(string inRowID, string inCode, ref string ioErrorMsg)
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strMsg1 = "ไม่สามารถลบได้เนื่องจาก";
            string strMsg2 = "";
            bool bllHasUsed = true;
            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QHasUsed", this.mstrRefTable, "select cCode, cName from " + QEMSectInfo.TableName + " where cDept = ? ", ref strErrorMsg))
            {
                strMsg2 = "(" + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cCode"].ToString().TrimEnd() + ") " + this.dtsDataEnv.Tables["QHasUsed"].Rows[0]["cName"].ToString().TrimEnd();
                ioErrorMsg = strMsg1 + "มีแผนก " + strMsg2 + " อ้างถึงแล้ว";
            }
            else
            {
                bllHasUsed = false;
            }
            return bllHasUsed;
        }

        private bool pmDeleteRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
        {
            bool bllIsCommit = false;
            bool bllResult = false;
            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strErrorMsg = "";
                
                object[] pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

                bllResult = true;
            }
            catch (Exception ex)
            {
                ioErrorMsg = ex.Message;
                bllResult = false;

                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                }
                App.WriteEventsLog(ex);
            }
            finally
            {
                this.mdbConn.Close();
            }
            return bllResult;
        }

        private void pmSearchData()
        {
            if (this.gridView1.OptionsView.ShowAutoFilterRow)
            {
                this.gridView1.OptionsView.ShowAutoFilterRow = false;
                this.gridView1.ClearColumnsFilter();
            }
            else
            {
                this.gridView1.OptionsView.ShowAutoFilterRow = true;
            }
        }

        private void pmEnterForm()
        {
            if ((this.mFormActiveMode == FormActiveMode.PopUp || this.mFormActiveMode == FormActiveMode.Report)
                && this.pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
            {
                this.mbllPopUpResult = true;
                this.Hide();
            }
        }

        private int pmGetRowID(string inTag)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[intCnt]["cRowID"].ToString().TrimEnd() == inTag)
                    return intCnt;
            }
            return -1;
        }

        private void pmSaveData()
        {
            string strErrorMsg = "";
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");

                MessageBox.Show(UIBase.GetAppUIText(new string[] { "บันทึกเรียบร้อย", "Save Complete" }), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //if (this.mFormActiveMode == FormActiveMode.Edit && this.mFormEditMode == UIHelper.AppFormState.Insert)
                //{
                //    this.pmInsertLoop();
                //}
                //else
                //{
                //    this.pmGotoBrowPage();
                //}

                UIBase.WaitClear();
            }
			if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool pmValidBeforeSave(ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strMsg = "";

            //if (this.txtQcSect.Text.TrimEnd() == string.Empty)
            //{
            //    ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code", this.mstrFormMenuName + " Code is not define ! " });
            //    this.txtQcSect.Focus();
            //    return false;
            //}
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
            //    && !this.pmIsValidateCode(new object[] { App.ActiveCorp.RowID, this.mstrType, this.txtCode.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = UIBase.GetAppUIText(new string[] { this.mstrFormMenuName + " ซ้ำ", "Duplicate " + this.mstrFormMenuName + " !" });
            //    this.txtCode.Focus();
            //    return false;
            //}
            //else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
            //   && !this.pmIsValidateName(new object[] { App.ActiveCorp.RowID , this.txtName.Text.TrimEnd() }))
            //{
            //    ioErrorMsg = "ชื่อประเภทเครื่องจักรซ้ำ";
            //    this.txtName.Focus();
            //    return false;
            //}
            //else
            //    bllResult = true;

            return bllResult;
        }

        //private bool pmRunCode()
        //{
        //    //this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranchID }, this.txtCode.MaxLength);

        //    string strErrorMsg = "";
        //    string strLastRunCode = "";
        //    int intCodeLen = 0;
        //    int intRunCode = 1;
        //    int inMaxLength = this.txtCode.Properties.MaxLength;
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrType });
        //    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType =? and cCode < ':' order by cCode desc", ref strErrorMsg))
        //    {
        //        strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0][QMFResTypeInfo.Field.Code].ToString().Trim();
        //        try
        //        {
        //            intRunCode = Convert.ToInt32(strLastRunCode) + 1;
        //        }
        //        catch (Exception ex)
        //        {
        //            strErrorMsg = ex.Message.ToString();
        //            intRunCode++;
        //        }
        //    }
        //    intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : inMaxLength);
        //    this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));

        //    return true;
        //}

        //private bool pmIsValidateCode(object[] inPrefixPara)
        //{
        //    bool bllResult = true;
        //    string strErrorMsg = "";
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    if (objSQLHelper.SetPara(inPrefixPara)
        //        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cCode = ?", ref strErrorMsg))
        //    {
        //        bllResult = false;
        //    }
        //    return bllResult;
        //}

        //private bool pmIsValidateName(object[] inPrefixPara)
        //{
        //    bool bllResult = true;
        //    string strErrorMsg = "";
        //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        //    if (objSQLHelper.SetPara(inPrefixPara)
        //        && objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select cCode from " + this.mstrRefTable + " where cCorp = ? and cType = ? and cName = ?", ref strErrorMsg))
        //    {
        //        bllResult = false;
        //    }
        //    return bllResult;
        //}

        private void pmUpdateRecord()
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.pmUpdatePdCapIT();


                this.mdbTran.Commit();
                bllIsCommit = true;

                //if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                //{
                //    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                //}
                //else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                //{
                //    if (this.mstrOldCode == this.txtCode.Text && this.mstrOldName == this.txtName.Text)
                //    {
                //        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
                //    }
                //    else 
                //    {
                //        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
                //    }
                //}

			}
			catch (Exception ex)
			{
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                }
                App.WriteEventsLog(ex);

#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif

            }

			finally
			{
				this.mdbConn.Close();
			}

        }

        private void pmUpdatePdCapIT()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPlan].Rows)
            {

                bool bllIsNewRow = false;

                DataRow dtrPdCapIT = null;
                if (!Convert.IsDBNull(dtrTemPd["dStartDate"]))
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        //|| (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                        || (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))

                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrPdCapIT = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrPdCapIT["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrPdCapIT["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrPdCapIT = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    this.pmReplRecordPdCapIT(bllIsNewRow, dtrTemPd, ref dtrPdCapIT);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrPdCapIT, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
        }

        private bool pmReplRecordPdCapIT(bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrPdCapIT = ioRefProd;

            dtrPdCapIT["cCorp"] = App.ActiveCorp.RowID;
            dtrPdCapIT["cPlant"] = this.mstrPlant;
            dtrPdCapIT["cBranch"] = this.mstrBranch;
            dtrPdCapIT["cMfgBook"] = this.mstrBook;

            dtrPdCapIT["cWOrderH"] = inTemPd["cWOrderH"].ToString();
            dtrPdCapIT["cWOrderOP"] = inTemPd["cWOrderOP"].ToString();
            dtrPdCapIT["cRemark1"] = inTemPd["cRemark1"].ToString().TrimEnd();
            dtrPdCapIT["nQty1"] = Convert.ToDecimal(inTemPd["nQty1"]);

            if (!Convert.IsDBNull(inTemPd["dStartDate"]))
            {
                dtrPdCapIT["dStart"] = Convert.ToDateTime(inTemPd["dStartDate"]);
            }
            else
            {
                dtrPdCapIT["dStart"] = Convert.DBNull;
            }

            if (!Convert.IsDBNull(inTemPd["dFinishDate"]))
            {
                dtrPdCapIT["dFinish"] = Convert.ToDateTime(inTemPd["dFinishDate"]);
            }
            else
            {
                dtrPdCapIT["dFinish"] = Convert.DBNull;
            }

            return true;
        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            this.grdTemPlan.Focus();
            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmLoadFormData();
            }

        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

            this.mstrEditRowID = "";
            //this.txtQcSect.Tag = "";
            //this.txtQcSect.Text = "";
            //this.txtQnSect.Text = "";
            //this.txtQcWkCtr.Tag = "";
            //this.txtQcWkCtr.Text = "";
            //this.txtQnWkCtr.Text = "";
            //this.txtWkHour.Value = 6;

            this.dtsDataEnv.Tables[this.mstrTemPlan].Rows.Clear();
        }

        private void pmLoadFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strCoorTab = strFMDBName + ".dbo.COOR";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";

            string strStep = "CSTEP = case WORDERH.CMSTEP when '1' then '' when 'P' then 'MC' when 'L' then 'CLOSED' end";
            string strOPStep = "COPSTEP = case WORDERH.COPSTEP when '' then '' when '5' then 'START' when '9' then 'COMPLETE' end";
            string strPRStep = "CPRSTEP = case WORDERH.CPRSTEP when ' ' then '' when 'P' then 'PR' when 'L' then 'X' end";

            string strIsApprove = "CISAPPROVE = case ";
            strIsApprove += " when WORDERH.CISAPPROVE = 'A' then 'APPROVE' ";
            strIsApprove += " when WORDERH.CISAPPROVE = ' ' then '' ";
            strIsApprove += " end ";

            string strSQLExec = "";

            strSQLExec = "select WORDERH.CROWID, WORDERH.CSTAT, " + strIsApprove + "," + strStep + "," + strOPStep + "," + strPRStep + ", WORDERH.CCODE, WORDERH.CREFNO, WORDERH.DDATE ";
            strSQLExec += " , PROD.FCCODE AS QCPROD , PROD.FCNAME AS QNPROD ";
            strSQLExec += " , SECT.FCCODE AS QCSECT , JOB.FCCODE AS QCJOB ";
            strSQLExec += " , COOR.FCCODE AS QCCOOR ";
            strSQLExec += " , MFBOMHD.NMFGQTY as NBOMOUTQTY ";
            strSQLExec += " , WORDERH.NQTY, WORDERH.DDUEDATE, WORDERH.CMFGPROD, WORDERH.DCREATE, WORDERH.DLASTUPDBY, EM1.CLOGIN as CLOGIN_ADD, EM2.CLOGIN as CLOGIN_UPD from {0} WORDERH ";
            strSQLExec += " left join " + strProdTab + " PROD on PROD.FCSKID = WORDERH.CMFGPROD ";
            strSQLExec += " left join " + strSectTab + " SECT on SECT.FCSKID = WORDERH.CSECT ";
            strSQLExec += " left join " + strJobTab + " JOB on JOB.FCSKID = WORDERH.CJOB ";
            strSQLExec += " left join " + strCoorTab + " COOR on COOR.FCSKID = WORDERH.CCOOR ";
            strSQLExec += " left join MFBOMHD on MFBOMHD.CROWID = WORDERH.CBOMHD ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = WORDERH.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = WORDERH.CLASTUPDBY ";
            strSQLExec += " where WORDERH.CCORP = ? and WORDERH.CBRANCH = ? and WORDERH.CPLANT = ? and WORDERH.CREFTYPE = ? and WORDERH.CMFGBOOK = ? and WORDERH.DDATE between ? and ? ";
            strSQLExec += " and WORDERH.CMSTEP = ' ' and WORDERH.CSTAT <> 'C' ";
            strSQLExec += " order by WORDERH.DDATE";

            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable, "APPLOGIN" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook, this.mdttBegDate, this.mdttEndDate });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "WORDERH", strSQLExec, ref strErrorMsg))
            {

                string strFld2 = "MFWORDERIT_STDOP.*";
                strFld2 += " ,MFSTDOPR.CCODE as QCSTDOP, MFSTDOPR.CNAME as QNSTDOP";
                strFld2 += " ,MFWKCTRHD.CCODE as QCWKCTR, MFWKCTRHD.CNAME as QNWKCTR";

                string strSQLStr_StdOP = "select " + strFld2 + " from MFWORDERIT_STDOP ";
                strSQLStr_StdOP += " left join MFSTDOPR on MFSTDOPR.CROWID = MFWORDERIT_STDOP.cMOPR ";
                strSQLStr_StdOP += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWORDERIT_STDOP.cWkCtrH ";
                strSQLStr_StdOP += " where MFWORDERIT_STDOP.cWOrderH = ? order by MFWORDERIT_STDOP.cSeq ";
                pobjSQLUtil.NotUpperSQLExecString = false;
                foreach (DataRow dtrWOrder in this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows)
                {

                    pobjSQLUtil.SetPara(new object[] { dtrWOrder["cRowID"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QStdOP", "MFWORDERIT_STDOP", strSQLStr_StdOP, ref strErrorMsg))
                    {

                        string strRowID = "";
                        string strRemark1 = "";
                        string strRemark2 = "";
                        decimal decBalFGQty = this.pmGetStockBal(dtrWOrder["cMfgProd"].ToString(), this.mstrBook_WHouse_FG);
                        decimal decMfgQty = Convert.ToDecimal(dtrWOrder["nQty"]);
                        decimal decBOMOutputQty = Convert.ToDecimal(dtrWOrder["nBOMOutQty"]);
                        DateTime dttStart = DateTime.MinValue;
                        DateTime dttFinish = DateTime.MinValue;

                        foreach (DataRow dtrWOrderOP in this.dtsDataEnv.Tables["QStdOP"].Rows)
                        {
                            decimal decCapComsume = Convert.ToDecimal(dtrWOrderOP["nCapFactor1"]);
                            decimal decOPQty = decMfgQty / decBOMOutputQty * decCapComsume;

                            dttStart = DateTime.MinValue;
                            dttFinish = DateTime.MinValue;
                            pobjSQLUtil.SetPara(new object[] { dtrWOrder["cRowID"].ToString(), dtrWOrderOP["cRowID"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPlanItem", "MFPLANITEM", "select * from MFPLANITEM where cWOrderH = ? and cWOrderOP = ? ", ref strErrorMsg))
                            {

                                DataRow dtrPlanItem = this.dtsDataEnv.Tables["QPlanItem"].Rows[0];
                                strRowID = dtrPlanItem["cRowID"].ToString();
                                strRemark2 = dtrPlanItem["cRemark1"].ToString().TrimEnd();
                                if (!Convert.IsDBNull(dtrPlanItem["dStart"]))
                                {
                                    dttStart = Convert.ToDateTime(dtrPlanItem["dStart"]);
                                }
                                else
                                {
                                    dttStart = DateTime.MinValue;
                                }

                                if (!Convert.IsDBNull(dtrPlanItem["dFinish"]))
                                {
                                    dttFinish = Convert.ToDateTime(dtrPlanItem["dFinish"]);
                                }
                                else
                                {
                                    dttFinish = DateTime.MinValue;
                                }

                            }
                            this.pmAddRowTemPlan(strRowID, dtrWOrder["cRowID"].ToString(), dtrWOrderOP["cRowID"].ToString(), Convert.ToDateTime(dtrWOrder["dDate"]), dtrWOrder["cCode"].ToString(), dtrWOrder["QcCoor"].ToString(), dtrWOrder["QcProd"].ToString(), Convert.ToDecimal(dtrWOrder["nQty"]), decOPQty, strRemark1, decBalFGQty, Convert.ToDateTime(dtrWOrder["dDueDate"]), "", "", dtrWOrderOP["QcStdOP"].ToString(), dtrWOrderOP["QnStdOP"].ToString(), dtrWOrderOP["cWkCtrH"].ToString(), dtrWOrderOP["QcWkCtr"].ToString(), dttStart, dttFinish, "", "");
                        }
                        this.pmAddRowTemPlan("", "", "", null, "$$$$$$$", "$$$$$$$", "$$$$$$$", -1, -1, "$$$$$$$", -1, null, "$$$$$$$", "$$$$$$$", "$$$$$$$", "$$$$$$$", "$$$$$$$", "$$$$$$$", DateTime.MinValue, DateTime.MinValue, "", "");
                    }

                }
            }

        
        }

        private void pmLoadOldVar()
        {
            //this.mstrOldCode = this.txtCode.Text;
            //this.mstrOldName = this.txtName.Text;
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.pmEnterForm();
                    break;
                case Keys.PageUp:
                    if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_EDIT1 : this.pgfMainEdit.SelectedTabPageIndex + 1);
                    break;
                case Keys.PageDown:
                    if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
                        this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex - 1 <= xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);
                    break;
                case Keys.Escape:
                    this.pmKeyboard_Esc();
                    break;
            }
        
        }

        private void pmKeyboard_Esc()
        {
            //if (this.pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
            //{
            //    if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ต้องการออกจากหน้าจอ ?", "Do you want to exit ?" }), "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        this.pmGotoBrowPage();
            //    }
            //}
            //else
            //{
                this.mbllPopUpResult = false;
                if (this.mFormActiveMode == FormActiveMode.Edit)
                {
                    this.mbllFilterResult = false;
                    this.pmFilterForm();

                    this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
                    if (!this.mbllFilterResult)
                    {
                        this.Close();
                    }
                    else
                    {
                        this.mFormEditMode = UIHelper.AppFormState.Edit;
                        this.pmLoadEditPage();

                        //this.pmGotoBrowPage();
                    }
                    //this.Close();
                }
                else
                    this.Hide();
            //}
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            this.pmSetSortKey(this.gridView1.SortedColumns[0].FieldName, false);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.txtFooter.Text = UIBase.GetAppUIText(new string[] { "สร้างโดย LOGIN : ", "Create By LOGIN : " }) + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + UIBase.GetAppUIText(new string[] { "วันที่/เวลา : ", " Date/Time : " }) + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
                this.txtFooter.Text += UIBase.GetAppUIText(new string[] { "\r\nแก้ไขล่าสุดโดย LOGIN : ", "\r\nLast update by LOGIN : " }) + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + UIBase.GetAppUIText(new string[] { "วันที่/เวลา : ", " Date/Time : " }) + Convert.ToDateTime(dtrBrow["dLastUpdBy"]).ToString("dd/MM/yy hh:mm:ss");
            }
            else 
            {
                this.txtFooter.Text = "";
            }
        }

        public DataRow RetrieveValue()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null) return null;
            
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["cRowID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        private void grcDate_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (sender is DevExpress.XtraEditors.DateEdit)
                    {
                        DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
                        if (oSender != null && oSender.Properties.AllowNullInput == DevExpress.Utils.DefaultBoolean.True)
                        {
                            oSender.EditValue = null;
                            e.Handled = true;
                        }
                    }
                    break;
            }

        }

        private void grcDate_EditValueChanged(object sender, EventArgs e)
        {

            DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
            if (oSender != null)
            {
                //oSender.EditValue = null;
                //MessageBox.Show("Update Tem Cap");
            }
            
        }

        private decimal pmGetStockBal(string inProd, string inWHouse)
        {

            decimal decBalStk = 0;
            string strBook_WHouse = inWHouse;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = " select REFPROD.FCPROD , REFPROD.FCIOTYPE , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNQTY ";
            strSQLStr += " from REFPROD ";
            strSQLStr += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " where REFPROD.FCPROD = ? ";
            strSQLStr += " and WHOUSE.FCCORP = ? and WHOUSE.FCTYPE = ' ' ";
            strSQLStr += (strBook_WHouse.Trim() != string.Empty ? " and WHOUSE.FCCODE in (" + strBook_WHouse + ") " : "");
            strSQLStr += " and GLREF.FCSTAT <> 'C' ";
            strSQLStr += " group by REFPROD.FCPROD,REFPROD.FCIOTYPE ";
            pobjSQLUtil.SetPara(new object[] { inProd, App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBal", "REFPROD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBal in this.dtsDataEnv.Tables["QBal"].Rows)
                {
                    decimal decSign = (dtrBal["fcIOType"].ToString().Trim() == "I" ? 1 : -1);
                    decBalStk = decBalStk + (Convert.ToDecimal(dtrBal["fnQty"]) * decSign);
                }
            }
            //this.txtFGBalQty.Value = decBalStk;
            return decBalStk;
        }

        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.pmUpdCapTableView();

            //if (this.txtPlanDate.Tag != null)
            //{
            //    this.pmUpdCapTableView(this.txtPlanDate.Tag.ToString());
            //}
            //else
            //{
            //    this.pmUpdCapTableView("");
            //}
        }

        private void gridView3_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPlan.MainView as ColumnView;

            //string strValue = "";
            string strValue = e.Value.ToString();
            string strCol = gridView3.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);


            switch (strCol.ToUpper())
            {
                case "DSTARTDATE":

                    this.txtQcPlanWkCtr.Text = dtrTemPd["cQcWkCtrH"].ToString();
                    if (e.Value != null && !Convert.IsDBNull(e.Value))
                    {
                        DateTime dttStartDate = Convert.ToDateTime(e.Value);
                        this.txtPlanDate.Text = Convert.ToDateTime(e.Value).ToString("dd/MM/yy");
                        this.txtPlanDate.Tag = Convert.ToDateTime(e.Value).ToString("yyyyMMdd");
                        dtrTemPd["dStartDate"] = Convert.ToDateTime(e.Value);
                        dtrTemPd["cStartDate"] = this.txtPlanDate.Tag.ToString();
                        this.gridView3.UpdateCurrentRow();
                        this.pmUpdCapTableView();
                        //this.pmUpdCapTableView(this.txtPlanDate.Tag.ToString());
                    }
                    break;
            }

        }

        private void pmUpdCapTableView()
        {
            DataRow dtrBrow = this.gridView3.GetDataRow(this.gridView3.FocusedRowHandle);
            if (dtrBrow != null)
            {
                this.txtQcPlanWkCtr.Text = dtrBrow["cQcWkCtrH"].ToString();
                this.txtQcPlanWkCtr.Tag = dtrBrow["cWkCtrH"].ToString();
                if (!Convert.IsDBNull(dtrBrow["dStartDate"]))
                {
                    this.txtPlanDate.Text = Convert.ToDateTime(dtrBrow["dStartDate"]).ToString("dd/MM/yy");
                    dtrBrow["cStartDate"] = Convert.ToDateTime(dtrBrow["dStartDate"]).ToString("yyyyMMdd");
                    this.pmCalCapUse(dtrBrow["cStartDate"].ToString());
                }
                else
                {
                    this.txtPlanDate.Text = "";
                    dtrBrow["cStartDate"] = "";
                    this.pmCalCapUse("");
                }
            }
            else
            {
                this.txtQcPlanWkCtr.Text = "";
                this.txtPlanDate.Text = "";
            }

        }

        private void pmCalCapUse(string inDate)
        {
            //Cap from this TemPlan OK
            //Cap from MFPLANITEM TODO
            this.pmLoadTemProdCap(this.txtQcPlanWkCtr.Tag.ToString(), this.txtQcPlanWkCtr.Text.TrimEnd(), inDate);

            //for (int i = this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Count; i < 0; i--)
            //{
            //    DataRow dtrTemCap = this.dtsDataEnv.Tables[this.mstrTemCap].Rows[i];
            //    decimal decCapReq = Convert.ToDecimal(dtrTemCap["nReqCap"]);
            //    decimal decCapBal = Convert.ToDecimal(dtrTemCap["nStdCap"]) - decCapReq;
            //}

        }

        private void pmLoadTemProdCap(string inWkCtr, string inQcWkCtr, string inDate)
        {

            //DataRow[] aSel = this.dtsDataEnv.Tables[this.mstrTemAllPdCap].Select("cQcWkCtr = '" + inQcWkCtr + "' and cDate = '" + inDate.ToString("yyyyMMdd") + "'", "cSeq");
            decimal decSumCapUse = this.pmSumCapUse(inQcWkCtr, inDate);
            DataRow[] aSel = this.dtsDataEnv.Tables[this.mstrTemAllPdCap].Select("cQcWkCtr = '" + inQcWkCtr + "'", "cSeq");
            if (aSel.Length == 0)
            {
                this.pmAddTemCap(inWkCtr, inQcWkCtr);
            }

            this.dtsDataEnv.Tables[this.mstrTemCap].Rows.Clear();
            for (int i = 0; i < aSel.Length; i++)
            {
                this.pmAddRowTemCap(aSel[i]["cSeq"].ToString(), aSel[i]["cQcWkCtr"].ToString(), aSel[i]["cDesc"].ToString(), Convert.ToDecimal(aSel[i]["nStdCap"]), decSumCapUse, Convert.ToDecimal(aSel[i]["nBalCap"]), Convert.ToDecimal(aSel[i]["nResCount"]));
            }

        }

        private void pmAddTemCap(string inWkCtr, string inQcWkCtr)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select sum(MFPDCAPIT.NCAP1DAY) as NCAP1DAY,count(*) as NCNT  ";
            strSQLExec += " ,sum(MFPDCAPIT.NOT1) as OT1,sum(MFPDCAPIT.NOT2) as OT2 ";
            strSQLExec += " ,sum(MFPDCAPIT.NOT3) as OT3,sum(MFPDCAPIT.NOT4) as OT4 ";
            strSQLExec += " ,sum(MFPDCAPIT.NOT5) as OT5 ";
            strSQLExec += " from MFPDCAPIT ";
            strSQLExec += " left join MFPDCAPHD on MFPDCAPHD.CROWID = MFPDCAPIT.CPDCAPH ";
            strSQLExec += " where MFPDCAPHD.CWKCTRH = ? ";
            
            pobjSQLUtil.SetPara(new object[] { inWkCtr });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdCap", "MFPDCAPHD", strSQLExec, ref strErrorMsg))
            {
                DataRow dtrPdCap = this.dtsDataEnv.Tables["QPdCap"].Rows[0];
                this.pmAddRowTemAllCap("1",inQcWkCtr, "OT 22.00", this.pmXConvetToDecimal(dtrPdCap, "OT5"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));
                this.pmAddRowTemAllCap("2", inQcWkCtr, "OT 21.00", this.pmXConvetToDecimal(dtrPdCap, "OT4"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));
                this.pmAddRowTemAllCap("3", inQcWkCtr, "OT 20.00", this.pmXConvetToDecimal(dtrPdCap, "OT3"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));
                this.pmAddRowTemAllCap("4", inQcWkCtr, "OT 19.00", this.pmXConvetToDecimal(dtrPdCap, "OT2"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));
                this.pmAddRowTemAllCap("5", inQcWkCtr, "OT 18.00", this.pmXConvetToDecimal(dtrPdCap, "OT1"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));
                this.pmAddRowTemAllCap("6", inQcWkCtr, "8 ชม.", this.pmXConvetToDecimal(dtrPdCap, "nCap1Day"), 0, 0, this.pmXConvetToDecimal(dtrPdCap, "nCnt"));

            }
        }

        private decimal pmXConvetToDecimal(DataRow inRow, string inFld)
        {
            return (Convert.IsDBNull(inRow[inFld]) ? 0 : Convert.ToDecimal(inRow[inFld]));
        }

        private decimal pmSumCapUse(string inQcWkCtr, string inDate)
        {
            decimal decSumCapUse = 0;
            decimal decCapUse = 0;
            decimal decOldCapUse = 0;
            if (inDate == "")
            {
            }
            else
            {
                foreach (DataRow dtrTemPlan in this.dtsDataEnv.Tables[this.mstrTemPlan].Rows)
                {
                    if (dtrTemPlan["cQcWkCtrH"].ToString().TrimEnd() == inQcWkCtr
                        && dtrTemPlan["cStartDate"].ToString().TrimEnd() == inDate)
                    {
                        decCapUse = this.pmXConvetToDecimal(dtrTemPlan, "nQty1");
                        //TODO: 
                        //decOldCapUse = this.pmXConvetToDecimal(dtrTemPlan, "nOldQty1");
                        decSumCapUse += decCapUse - decOldCapUse;
                    }
                }
            }
            return decSumCapUse;
        }

    }
}
