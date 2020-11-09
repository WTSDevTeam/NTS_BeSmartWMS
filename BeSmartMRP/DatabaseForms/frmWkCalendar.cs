
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;

using DevExpress.XtraGrid.Views.Base;

namespace BeSmartMRP.DatabaseForms
{

    public partial class frmWkCalendar : UIHelper.frmBase
    {

        public static string TASKNAME = "EMFWKCTR";

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();

        private string mstrRefTable = MapTable.Table.EMWorkCalendar;
        private string mstrITable = MapTable.Table.EMWorkCalendarItem;
        private string mstrITable2 = MapTable.Table.EMWorkCalendarItem_Range;
        private string mstrCostLineTable = MapTable.Table.CostLine;

        private string mstrActiveTem = "";
        private DevExpress.XtraGrid.Views.Grid.GridView mActiveGrid = null;

        private string mstrTemWorkHr = "TemWorkHr";
        private string mstrTemWorkHr_Range = "TemWorkHr_Range";

        private string mstrSortKey = QMFWorkCenterInfo.Field.Code;
        private bool mbllFilterResult = false;

        private string mstrEditRowID = "";
        private string mstrType = SysDef.gc_RESOURCE_TYPE_MACHINE;

        private string mstrPlant = "";
        private string mstrWkHour = "";
        private string mstrHoliday = "";
        private int mintYear = 2000;

        private string mstrFormMenuName = "Work Center";

        private UIHelper.AppFormState mFormEditMode;

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmWkCalendar()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmWkCalendar(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmWkCalendar mInstanse_1 = null;

        public static frmWkCalendar GetInstanse()
        {
            if (mInstanse_1 == null)
            {
                mInstanse_1 = new frmWkCalendar();
            }
            return mInstanse_1;
        }

        private static void pmClearInstanse()
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

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmCreateTem();
            this.pmInitGridProp_TemWorkHour();

            this.pmInitializeComponent();
            this.pmFilterForm();
            
            UIBase.WaitClear();

            this.DialogResult = (this.mbllFilterResult == true ? DialogResult.OK : DialogResult.Cancel);
            if (!this.mbllFilterResult)
            {
                this.Close();
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

            //UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            this.pmSetMaxLength();
            this.pmSetFormUI();
            this.pmMapEvent();

            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmSetFormUI()
        {
            //this.lblSect.Text = UIBase.GetAppUIText(new string[] { "แผนก :", "Section :" });
        }

        private void pmSetMaxLength()
        {
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
        }

        private void pmMapEvent()
        {
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);
            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
        }

        private void pmSetBrowView()
        {
        }

        private void pmInitGridProp_TemWorkHour()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemWorkHr].DefaultView;

            this.grdTemWkHour.DataSource = this.dtsDataEnv.Tables[this.mstrTemWorkHr];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].OptionsColumn.ReadOnly = true;
            }

            int i = 0;
            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cDayName"].VisibleIndex = i++;
            this.gridView1.Columns["dDate"].VisibleIndex = i++;
            this.gridView1.Columns["nWorkHR"].VisibleIndex = i++;
            this.gridView1.Columns["nOTHR"].VisibleIndex = i++;
            this.gridView1.Columns["nTotHour"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = "No.";
            this.gridView1.Columns["cDayName"].Caption = "วัน";
            this.gridView1.Columns["dDate"].Caption = "วันที่";
            this.gridView1.Columns["nWorkHR"].Caption = "ชั่วโมงทำงาน";
            this.gridView1.Columns["nOTHR"].Caption = "O.T.";
            this.gridView1.Columns["nTotHour"].Caption = "ชั่วโมง";

            this.gridView1.Columns["nRecNo"].Width = 50;
            this.gridView1.Columns["cDayName"].Width = 100;
            this.gridView1.Columns["dDate"].Width = 100;
            this.gridView1.Columns["nWorkHR"].Width = 100;
            this.gridView1.Columns["nOTHR"].Width = 100;
            this.gridView1.Columns["nTotHour"].Width = 100;

            this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["nWorkHR"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nWorkHR"].DisplayFormat.FormatString = "#,###,###,##0.00";

            this.gridView1.Columns["nOTHR"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nOTHR"].DisplayFormat.FormatString = "#,###,###,##0.00";

            this.gridView1.Columns["nTotHour"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nTotHour"].DisplayFormat.FormatString = "#,###,###,##0.00";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.grcQcMachine.Buttons[0].Tag = "GRDVIEW2_CQCMACHINE";

            //this.grcQcMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Code);
            //this.grcQnMachine.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QMFResourceInfo.TableName, QMFResourceInfo.Field.Name);
            //this.grcRemark.MaxLength = 150;

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView1.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            //this.gridView1.Columns["cQnMachine"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            //this.gridView1.Columns["cQnMachine"].OptionsColumn.AllowFocus = false;
            //this.gridView1.Columns["cQnMachine"].OptionsColumn.ReadOnly = true;

            //this.gridView1.Columns["cQcMachine"].ColumnEdit = this.grcQcMachine;
            //this.gridView1.Columns["cQnMachine"].ColumnEdit = this.grcQnMachine;
            //this.gridView1.Columns["cRemark1"].ColumnEdit = this.grcRemark;
            //this.pmCalcColWidth();
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["nRecNo"].Width
                                    + this.gridView1.Columns["cQcMachine"].Width
                                    + this.gridView1.Columns["cQnMachine"].Width;

            int intNewWidth = this.Width - intColWidth - 80;
            this.gridView1.Columns["cRemark1"].Width = (intNewWidth > 0 ? intNewWidth : 70);
        }

        private void pmFilterForm()
        {
            this.mbllFilterResult = true;
            this.pmInitPopUpDialog("FILTER");
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "FILTER":
                    using (Common.dlgMfgFilter02 dlgFilter = new Common.dlgMfgFilter02())
                    {

                        dlgFilter.SetTitle(this.Text, "", TASKNAME);
                        dlgFilter.ShowDialog();
                        if (dlgFilter.DialogResult == DialogResult.OK)
                        {
                            this.mbllFilterResult = true;

                            this.mstrPlant = dlgFilter.PlantID;
                            this.mstrHoliday = dlgFilter.HolidayID;
                            this.mstrWkHour = dlgFilter.StdWorkHrID;
                            this.mintYear = dlgFilter.Year;

                            this.pmLoadFormData();
                        }
                    }
                    break;

            }
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName.ToUpper())
                {
                    case "EDIT":
                        DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                        if (dtrBrow != null)
                        {
                            string strEditRowID = dtrBrow["cRowID"].ToString();
                            DateTime dttDate = Convert.ToDateTime(dtrBrow["dDate"]);

                            using (Common.dlgSetWorkHour dlg = new DatabaseForms.Common.dlgSetWorkHour(this.dtsDataEnv, strEditRowID, MRPAgent.AEngVShortDate[dttDate.DayOfWeek.GetHashCode()], dttDate))
                            {
                                //dlgFilter.SetTitle(this.Text, "", TASKNAME);
                                dlg.ShowDialog();
                                if (dlg.DialogResult == DialogResult.OK)
                                {
                                    decimal decWorkHr = 0;
                                    decimal decOTHr = 0;
                                    dlg.SaveData(ref this.dtsDataEnv, ref decWorkHr,ref decOTHr);
                                    dtrBrow["nWorkHR"] = decWorkHr;
                                    dtrBrow["nOTHR"] = decOTHr;

                                }
                            }
                        }
                        break;
                    case "SAVE":
                        this.pmSaveData();
                        break;
                    case "EXIT":
                        break;
                }

                //WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                //switch (oToolButton)
                //{
                //    case WsToolBar.Search:
                //        this.pmSearchData();
                //        break;
                //    case WsToolBar.Save:
                //        this.pmSaveData();
                //        break;
                //    case WsToolBar.Refresh:
                //        break;
                //}

            }
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
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cWkCtrH = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                //Delete CostLine
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrRefTable, this.mstrEditRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrCostLineTable + " where cCorp = ? and cRefTab = ? and cMasterH = ? ", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
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
        { }

        private void pmSaveData()
        {

            string strErrorMsg = "";
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");

                UIBase.WaitClear();
                this.pmBlankFormData();
                this.pmLoadFormData();
                MessageBox.Show("Save Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            return bllResult;
        }

        private void pmUpdateRecord()
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
            DataRow dtrSaveInfo = null;
            if (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
                && objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ?", ref strErrorMsg))
            {
                bllIsNewRow = false;
                dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
            }

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            //Sum work hour and OT hour
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemWorkHr].AsEnumerable()
                                    select new { TotHour = p1["nWorkHR"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemWorkHr].AsEnumerable()
                                  select new { TotHour = p1["nOTHR"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            dtrSaveInfo["NTOTWORKHR"] = decSumWorkHr;
            dtrSaveInfo["NTOTOTHR"] = decSumOTHr;
            dtrSaveInfo["CWORKHOUR"] = this.mstrWkHour;
            dtrSaveInfo["CHOLIDAY"] = this.mstrHoliday;

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.pmUpdateWorkCenterIT(this.mstrTemWorkHr);

                this.mdbTran.Commit();
                bllIsCommit = true;

               
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

        private void pmUpdateWorkCenterIT(string inAlias)
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[inAlias].Rows)
            {

                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (Convert.ToDecimal(dtrTemPd["nTotHour"]) != 0)
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrBudCI["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrBudCI["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    //var ps = from p1 in dtsDataEnv.Tables[this.mstrTemWorkHr_Range].AsEnumerable()
                    //         where Convert.ToDateTime(p1["DDATE"]).Date == Convert.ToDateTime(dtrTemPd["dDate"]).Date
                    //         select new { cRowID = p1["CROWID"], nWorkHr = p1["nWorkHr"], nOTHR = p1["nOTHR"], dDate = p1["DDATE"] };

                    this.pmReplRecordWkCtrIT(bllIsNewRow, dtrTemPd, ref dtrBudCI);
                    this.pmSaveWkCalIT_Range(bllIsNewRow, Convert.ToDateTime(dtrTemPd["dDate"]).Date, strRowID);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrBudCI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
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

        private bool pmReplRecordWkCtrIT(bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrWorkHourIT = ioRefProd;
            DateTime dttNewDate = Convert.ToDateTime(inTemPd["dDate"]);
            string strDayName = MRPAgent.AEngVShortDate[dttNewDate.DayOfWeek.GetHashCode()];


            dtrWorkHourIT["cCorp"] = App.ActiveCorp.RowID;
            dtrWorkHourIT["cWorkCalH"] = this.mstrEditRowID;
            dtrWorkHourIT["dDate"] = dttNewDate;
            dtrWorkHourIT["cDay"] = strDayName;
            dtrWorkHourIT["nWorkHr"] = Convert.ToDecimal(inTemPd["nWorkHR"]);
            dtrWorkHourIT["nOTHr"] = Convert.ToDecimal(inTemPd["nOTHR"]);
            dtrWorkHourIT["cPlant"] = this.mstrPlant;

            return true;
        }

        private bool pmSaveWkCalIT_Range(bool inState, DateTime inWorkDate, string inWkCalIT)
        {

            string strErrorMsg = "";
            object[] pAPara = null;

            var lsGrpWorkHourDate = from p1 in this.dtsDataEnv.Tables[this.mstrTemWorkHr_Range].AsEnumerable()
                                    where Convert.ToDateTime(p1["dDate"]) == inWorkDate
                                    select new { cRowID = p1["cRowID"], dDate = p1["dDate"], dBegTime = p1["dBegTime"], dEndTime = p1["dEndTime"], cType = p1["cType"], nTotHour = p1["nTotHour"], nRecNo = p1["nRecNo"] };

            foreach (var liGrpHourDate in lsGrpWorkHourDate.ToList())
            {

                //this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                //DataRow dtrWorkCalendarIT_Rng = this.dtsDataEnv.Tables[this.mstrITable2].NewRow();
                DataRow dtrWorkCalendarIT_Rng = null;
                string strRowID = "";
                bool bllIsNewRow = false;
                if ((liGrpHourDate.cRowID.ToString().TrimEnd() == string.Empty)
                    && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable2 + " where cRowID = ?", new object[1] { liGrpHourDate.cRowID.ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                {
                    this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    dtrWorkCalendarIT_Rng = this.dtsDataEnv.Tables[this.mstrITable2].NewRow();

                    strRowID = App.mRunRowID(this.mstrITable);
                    bllIsNewRow = true;
                    dtrWorkCalendarIT_Rng["cRowID"] = strRowID;
                }
                else
                {
                    this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where cRowID = ?", new object[1] { liGrpHourDate.cRowID.ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    dtrWorkCalendarIT_Rng = this.dtsDataEnv.Tables[this.mstrITable2].Rows[0];

                    strRowID = dtrWorkCalendarIT_Rng["cRowID"].ToString();
                    bllIsNewRow = false;
                }


                string strDayName = "";
                strDayName = MRPAgent.AEngVShortDate[inWorkDate.DayOfWeek.GetHashCode()];

                dtrWorkCalendarIT_Rng["cRowID"] = strRowID;
                dtrWorkCalendarIT_Rng["cCorp"] = App.ActiveCorp.RowID;
                dtrWorkCalendarIT_Rng["cWorkCalH"] = this.mstrEditRowID;
                dtrWorkCalendarIT_Rng["cWorkCalIT"] = inWkCalIT;
                dtrWorkCalendarIT_Rng["cDay"] = strDayName;
                dtrWorkCalendarIT_Rng["dDate"] = inWorkDate;
                dtrWorkCalendarIT_Rng["dBegTime"] = Convert.ToDateTime(liGrpHourDate.dBegTime);
                dtrWorkCalendarIT_Rng["dEndTime"] = Convert.ToDateTime(liGrpHourDate.dEndTime);
                dtrWorkCalendarIT_Rng["cType"] = liGrpHourDate.cType.ToString();
                dtrWorkCalendarIT_Rng["cPlant"] = this.mstrPlant;
                dtrWorkCalendarIT_Rng["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(liGrpHourDate.nRecNo), 2);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrWorkCalendarIT_Rng, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }


            return true;
        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemWorkHr);
            //dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cDayName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDayName2", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("nWorkHR", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nOTHR", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"), "nWorkHR+nOTHR");
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cDayName"].DefaultValue = "";
            dtbTemPd.Columns["cDayName2"].DefaultValue = "";
            dtbTemPd.Columns["nWorkHR"].DefaultValue = 0;
            dtbTemPd.Columns["nOTHR"].DefaultValue = 0;
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemPd2 = new DataTable(this.mstrTemWorkHr_Range);
            dtbTemPd2.Columns.Add("cIsDel", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cWkHourRowID", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd2.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd2.Columns.Add("cDay", System.Type.GetType("System.String"));
            dtbTemPd2.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemPd2.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemPd2.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemPd2.Columns["cIsDel"].DefaultValue = "";
            dtbTemPd2.Columns["cRowID"].DefaultValue = "";
            dtbTemPd2.Columns["nTotHour"].DefaultValue = 0;
            this.dtsDataEnv.Tables.Add(dtbTemPd2);

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemWorkHr_Range];

        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private void pmLoadEditPage()
        {
        }

        private void pmBlankFormData()
        {
            this.dtsDataEnv.Tables[this.mstrTemWorkHr].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemWorkHr_Range].Rows.Clear();
        }

        private void pmLoadFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "select EMWORKCALENDAR.CROWID as CWORKCALH_ROW,EMWORKCALENDARIT.* from EMWORKCALENDAR";
            strSQLStr += " left join EMWORKCALENDARIT on EMWORKCALENDARIT.CWORKCALH = EMWORKCALENDAR.CROWID";
            strSQLStr += " where EMWORKCALENDAR.CCORP = ? and EMWORKCALENDAR.CPLANT = ? ";
            strSQLStr += " and EMWORKCALENDAR.CWORKHOUR = ? and EMWORKCALENDAR.CHOLIDAY = ? and EMWORKCALENDAR.NYEAR = ?";
            strSQLStr += " order by EMWORKCALENDARIT.DDATE";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrPlant, this.mstrWkHour, this.mstrHoliday, this.mintYear });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWorkHourIT", this.mstrRefTable, strSQLStr, ref strErrorMsg);

            //if (this.dtsDataEnv.Tables["QWorkHourIT"].Rows[0])

            if (this.dtsDataEnv.Tables["QWorkHourIT"].Rows.Count > 0)
            {
                this.mstrEditRowID = this.dtsDataEnv.Tables["QWorkHourIT"].Rows[0]["CWORKCALH_ROW"].ToString();
            }

            DateTime dttBegDate = new DateTime(this.mintYear, 1, 1);
            DateTime dttEndDate = new DateTime(this.mintYear, 12, 31);
            TimeSpan tsYear = dttEndDate - dttBegDate;
            double intYearDay = tsYear.TotalDays;
            for (int i = 0; i < Convert.ToInt32(intYearDay); i++)
            {
                DateTime dttNewDate = dttBegDate.AddDays(i);
                var ps = from p1 in dtsDataEnv.Tables["QWorkHourIT"].AsEnumerable()
                         where Convert.ToDateTime(p1["DDATE"]).Date == dttNewDate.Date
                         select new { cRowID = p1["CROWID"], nWorkHr = p1["nWorkHr"], nOTHR = p1["nOTHR"], dDate = p1["DDATE"] };

                DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemWorkHr].NewRow();

                if (ps.ToList().Count > 0)
                {
                    foreach (var psWorkRng in ps.ToList())
                    {
                        dtrNewRow["cRowID"] = psWorkRng.cRowID;
                        dtrNewRow["nWorkHR"] = Convert.ToDecimal(psWorkRng.nWorkHr);
                        dtrNewRow["nOTHR"] = Convert.ToDecimal(psWorkRng.nOTHR);
                    }
                }

                dtrNewRow["dDate"] = dttNewDate;
                string strDayName = "";
                string strDayName2 = "";
                strDayName = MRPAgent.AThaiDate[dttNewDate.DayOfWeek.GetHashCode()];
                strDayName2 = MRPAgent.AEngDate[dttNewDate.DayOfWeek.GetHashCode()];

                dtrNewRow["cDayName"] = strDayName;
                dtrNewRow["cDayName2"] = strDayName2;

                this.dtsDataEnv.Tables[this.mstrTemWorkHr].Rows.Add(dtrNewRow);

            }
            this.gridView1.FocusedRowHandle = 0;
        }

        private void pmLoadOldVar()
        {
        }

        private void pmLoadWorkCalendarIT()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intRow = 0;
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID, "" });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWkCtrIT", "WkCtrIT", "select * from " + this.mstrITable + " where cWkCtrH = ? and cType = ? order by cSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrWkCtrIT in this.dtsDataEnv.Tables["QWkCtrIT"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemWorkHr].NewRow();

                    intRow++;
                    //dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrWkCtrIT["cRowID"].ToString();
                    dtrNewRow["cMachine"] = dtrWkCtrIT["cResource"].ToString();

                    this.dtsDataEnv.Tables[this.mstrTemWorkHr].Rows.Add(dtrNewRow);

                }
            }

        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //this.pmGotoBrowPage();
                        this.Close();
                    }
                    break;
            }
        
        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            string strOrderBy = "";
            switch (inKeyField.ToUpper())
            {
                case "GRDVIEW2_CQCMACHINE":
                case "GRDVIEW2_CQNMACHINE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW2_CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE");
                    //this.pofrmResource.ValidateField("", strOrderBy, true);
                    //if (this.pofrmResource.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inKeyField);
                    //}
                    break;
                case "GRDVIEW3_CQCMACHINE":
                case "GRDVIEW3_CQNMACHINE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW3_CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                    this.pmInitPopUpDialog("RESOURCE2");
                    //this.pofrmResource2.ValidateField("", strOrderBy, true);
                    //if (this.pofrmResource2.PopUpResult)
                    //{
                    //    this.pmRetrievePopUpVal(inKeyField);
                    //}
                    break;
            }
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemWkHour.MainView as ColumnView;

            string strCol = gridView1.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCMACHINE":
                case "CQNMACHINE":

                    string strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cMachine"] = "";
                        dtrTemPd["cQcMachine"] = "";
                        dtrTemPd["cQnMachine"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("RESOURCE");
                        string strOrderBy = (strCol.ToUpper() == "CQCMACHINE" ? QMFResourceInfo.Field.Code : QMFResourceInfo.Field.Name);
                        //e.Valid = !this.pofrmResource.ValidateField(strValue, strOrderBy, false);

                        //if (this.pofrmResource.PopUpResult)
                        //{
                        //    e.Valid = true;
                        //    this.gridView2.UpdateCurrentRow();
                        //    this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                        //    e.Value = (strCol.ToUpper() == "CQCMACHINE" ? dtrTemPd["cQcMachine"].ToString().TrimEnd() : dtrTemPd["cQnMachine"].ToString().TrimEnd());
                        //}
                        //else
                        //{
                        //    e.Value = "";
                        //    dtrTemPd["cMachine"] = "";
                        //    dtrTemPd["cQcMachine"] = "";
                        //    dtrTemPd["cQnMachine"] = "";
                        //}
                    }
                    break;
            }

        }

        private void grcGridColumn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmGridPopUpButtonClick(e.Button.Tag.ToString());
        }

        private void grcGridColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    this.pmGridPopUpButtonClick(gridView1.FocusedColumn.FieldName);
                    break;
            }
        }

        private void grdTemPd_Resize(object sender, EventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void grdTemPd2_Resize(object sender, EventArgs e)
        {
            //this.pmCalcColWidth2();
        }

        private void gridView2_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            this.pmCalcColWidth();
        }

        private void gridView3_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            //this.pmCalcColWidth2();
        }


        private void gridView2_GotFocus(object sender, EventArgs e)
        {
            this.mstrActiveTem = this.mstrTemWorkHr;
            this.mActiveGrid = this.gridView1;
            this.gridView1.FocusedColumn = this.gridView1.Columns["cQcMachine"];
        }


    }
}
