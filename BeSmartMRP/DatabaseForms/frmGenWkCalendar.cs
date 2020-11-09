
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using WS.Data;
using WS.Data.Agents;
using AppUtil;

namespace BeSmartMRP.DatabaseForms
{

    public partial class frmGenWkCalendar : UIHelper.frmBase
    {

        public frmGenWkCalendar()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;

        }

        private string mstrHTable = MapTable.Table.EMWorkCalendar;
        private string mstrRefTable = MapTable.Table.EMWorkCalendar;
        private string mstrITable = MapTable.Table.EMWorkCalendarItem;
        private string mstrITable2 = MapTable.Table.EMWorkCalendarItem_Range;

        private string mstrCode = "";
        private string mstrName = "";

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmEMHoliday pofrmEMHoliday = null;
        private DatabaseForms.frmEMStdWorkHour pofrmEMStdWorkHour = null;

        Report.LocalDataSet.DTSPMCLIST dtsPrintPreview = new Report.LocalDataSet.DTSPMCLIST();

        private DateTime mdttCurrDate = DateTime.MinValue;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = DocumentType.MC.ToString();
        private int mintYear = 0;
        private string mstrSect = "";
        private string mstrTemPd = "TemPd";
        private DataSet dtsDataEnv = new DataSet();

        private string mstrEditRowID = "";
        private string mstrFormMenuName = "";
        private UIHelper.AppFormState mFormEditMode;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcHoliday.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);

            this.txtQcBranch.Tag = "";
            this.txtQcPlant.Tag = "";
            this.txtQcHoliday.Tag = "";

            this.txtQcHoliday.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

            }

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cCorp = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant["cRowID"].ToString();
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }

            this.txtDate.DateTime = DateTime.Now;

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID, DateTime.Now.Year });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QHoliday", QEMHolidayInfo.TableName, "select cRowID, cCode, cName from " + QEMHolidayInfo.TableName + " where cCorp = ? and nYear = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrHoliday = this.dtsDataEnv.Tables["QHoliday"].Rows[0];
                this.txtQcHoliday.Tag = dtrHoliday["cRowID"].ToString();
                this.txtQcHoliday.Text = dtrHoliday["cCode"].ToString().TrimEnd();
            }

            //this.cmbWHCost.Properties.Items.Clear();
            //this.cmbWHCost.Properties.Items.AddRange(new object[] { 
            //    UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
            //    , UIBase.GetAppUIText(new string[] { "ราคาทุนจริง" , "Actual Cost" })
            //    , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
            //    , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
            //    , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });

            //this.cmbWHCost.SelectedIndex = 0;

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

            this.mFormEditMode = AppFormState.Insert;
            this.pmCreateTem();

        }

        private static frmGenWkCalendar mInstanse = null;

        public static frmGenWkCalendar GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmGenWkCalendar();
            }
            return mInstanse;
        }

        private static void pmClearInstanse()
        {
            if (mInstanse != null)
            {
                mInstanse = null;
            }
        }

        private void pmCreateTem()
        {

            //รายการสินค้า
            this.dtsDataEnv.CaseSensitive = true;

            DataTable dtbTemPd = new DataTable(this.mstrTemPd);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("cDesc", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cWorkHr_IT", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nTotHour"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemWkHour_TableNewRow);

            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void dtbTemWkHour_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            //e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        private void pmMapEvent()
        {
            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new CancelEventHandler(txtQcBranch_Validating);

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            this.txtQcHoliday.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcHoliday.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcHoliday.Validating += new CancelEventHandler(txtQcHoliday_Validating);

            this.txtQcStdWorkHr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcStdWorkHr.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcStdWorkHr.Validating += new CancelEventHandler(txtQcStdWorkHr_Validating);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

        }

        private void pmSetFormUI()
        {
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ประจำปี :", "Year :" });
            this.lblHoliday.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลวันหยุดประจำปี :", "Select Holiday and NoworkingDay :" });
            this.lblStdWorkHr.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลเวลาทำงานมาตรฐาน :", "Select Stadard Work Hour :" });

            this.btnPrint.Text = UIBase.GetAppUIText(new string[] { "ตกลง", "OK" });
            this.btnCancel.Text = UIBase.GetAppUIText(new string[] { "ยกเลิก", "Cancel" });
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new BeSmartMRP.DialogForms.dlgGetBranch();
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PLANT":
                    if (this.pofrmGetPlant == null)
                    {
                        this.pofrmGetPlant = new frmEMPlant(FormActiveMode.Report);
                        this.pofrmGetPlant.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPlant.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "HOLIDAY":
                    if (this.pofrmEMHoliday == null)
                    {
                        this.pofrmEMHoliday = new frmEMHoliday(FormActiveMode.Report);
                        this.pofrmEMHoliday.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmEMHoliday.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "STDWORKHOUR":
                    if (this.pofrmEMStdWorkHour == null)
                    {
                        this.pofrmEMStdWorkHour = new frmEMStdWorkHour(FormActiveMode.Report);
                        this.pofrmEMStdWorkHour.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmEMStdWorkHour.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strTagButton = (e.Button.Tag != null ? e.Button.Tag.ToString() : "");
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    this.pmInitPopUpDialog("BRANCH");
                    strOrderBy = (inTextbox == "TXTQCBRANCH" ? "FCCODE" : "FCNAME");
                    this.pofrmGetBranch.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCPLANT":
                case "TXTQNPLANT":
                    this.pmInitPopUpDialog("PLANT");
                    strOrderBy = (inTextbox == "TXTQCPLANT" ? QEMPlantInfo.Field.Code : QEMPlantInfo.Field.Name);
                    this.pofrmGetPlant.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetPlant.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCHOLIDAY":
                    this.pmInitPopUpDialog("HOLIDAY");
                    strOrderBy = (inTextbox == "TXTQCHOLIDAY" ? "CCODE" : "CNAME");
                    this.pofrmEMHoliday.ValidateField("", strOrderBy, true);
                    if (this.pofrmEMHoliday.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCSTDWORKHR":
                    this.pmInitPopUpDialog("STDWORKHOUR");
                    strOrderBy = (inTextbox == "TXTQCSTDWORKHR" ? "CCODE" : "CNAME");
                    this.pofrmEMStdWorkHour.ValidateField("", strOrderBy, true);
                    if (this.pofrmEMStdWorkHour.PopUpResult)
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
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    if (this.pofrmGetBranch != null)
                    {
                        DataRow dtrBranch = this.pofrmGetBranch.RetrieveValue();

                        if (dtrBranch != null)
                        {
                            if (this.txtQcBranch.Tag.ToString() != dtrBranch["fcSkid"].ToString())
                            {
                                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                            }

                            this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";

                            this.txtQcHoliday.Tag = "";
                            this.txtQcHoliday.Text = "";

                        }
                    }
                    break;
                case "TXTQCPLANT":
                case "TXTQNPLANT":

                    DataRow dtrGetVal = this.pofrmGetPlant.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        if (this.txtQcPlant.Tag.ToString() != dtrGetVal["cRowID"].ToString())
                        {
                            this.txtQcPlant.Tag = dtrGetVal["cRowID"].ToString();
                        }

                        this.txtQcPlant.Tag = dtrGetVal[QEMPlantInfo.Field.RowID].ToString();
                        this.txtQcPlant.Text = dtrGetVal[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnPlant.Text = dtrGetVal[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcPlant.Tag = "";
                        this.txtQcPlant.Text = "";
                        this.txtQnPlant.Text = "";

                        this.txtQcHoliday.Tag = "";
                        this.txtQcHoliday.Text = "";

                    }
                    break;
                case "TXTQCHOLIDAY":
                    if (this.pofrmEMHoliday != null)
                    {
                        DataRow dtrPDGRP = this.pofrmEMHoliday.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcHoliday.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcHoliday.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcHoliday.Tag = "";
                            this.txtQcHoliday.Text = "";
                        }
                    }
                    break;

                case "TXTQCSTDWORKHR":
                    if (this.pofrmEMStdWorkHour != null)
                    {
                        DataRow dtrPDGRP = this.pofrmEMStdWorkHour.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcStdWorkHr.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcStdWorkHr.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcStdWorkHr.Tag = "";
                            this.txtQcStdWorkHr.Text = "";
                        }
                    }
                    break;

            }
        }

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBRANCH" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBranch.Tag = "";
                this.txtQcBranch.Text = "";
                this.txtQnBranch.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BRANCH");
                e.Cancel = !this.pofrmGetBranch.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBranch.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcPlant_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCPLANT" ? QEMPlantInfo.Field.Code : QEMPlantInfo.Field.Name;

            if (txtPopUp.Text == "")
            {
                this.txtQcPlant.Tag = "";
                this.txtQcPlant.Text = "";
                this.txtQnPlant.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PLANT");
                e.Cancel = !this.pofrmGetPlant.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPlant.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcHoliday_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCHOLIDAY" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcHoliday.Tag = "";
                this.txtQcHoliday.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("HOLIDAY");
                e.Cancel = !this.pofrmEMHoliday.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmEMHoliday.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcStdWorkHr_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSTDWORKHR" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcStdWorkHr.Tag = "";
                this.txtQcStdWorkHr.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("STDWORKHOUR");
                e.Cancel = !this.pofrmEMStdWorkHour.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmEMStdWorkHour.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);

            this.pmClrOldData();
            this.pmGenTem();
            this.pmSaveData();
            //MessageBox.Show("OK");
            MessageBox.Show(this, UIBase.GetAppUIText(new string[] { "ประมวลผลเสร็จเรียบร้อย", "Generate Complete" }), "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pmGenTem()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            string strSQLStr = "";
            strSQLStr += " select * from EMHOLIDAYIT where CCORP = ? and CHOLIDAYH = ? order by DDATE ";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcHoliday.Tag.ToString() });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QHoliday", QEMHolidayInfo.TableName, strSQLStr, ref strErrorMsg);

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcStdWorkHr.Tag.ToString() });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWorkHr_Rng", "EMWORKHOURIT_RANGE", "select * from EMWORKHOURIT_RANGE where CCORP = ? and CWKHOURH = ? order by CSEQ", ref strErrorMsg);
            
            //foreach (DataRow dtrHoliday in this.dtsDataEnv.Tables["QHoliday"].Rows)
            //{
            //}

            DateTime dttBegDate = new DateTime(this.txtDate.DateTime.Year, 1, 1);
            DateTime dttEndDate = new DateTime(this.txtDate.DateTime.Year, 1, 1).AddYears(1).AddDays(-1);

            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];
            TimeSpan tsYear = dttEndDate - dttBegDate;
            double intYearDay = tsYear.TotalDays;
            for (int i = 0; i < Convert.ToInt32(intYearDay); i++)
            {
                DateTime dttNewDate = dttBegDate.AddDays(i);
                var ps = from p1 in dtsDataEnv.Tables["QHoliday"].AsEnumerable()
                         where Convert.ToDateTime(p1["DDATE"]).Date == dttNewDate.Date
                          select new { dDate = p1["DDATE"] };

                if (ps.ToList().Count > 0)
                {
                }
                else
                {

                    string strDayName = "";
                    strDayName = MRPAgent.AEngVShortDate[dttNewDate.DayOfWeek.GetHashCode()];

                    var ps2 = from p1 in dtsDataEnv.Tables["QWorkHr_Rng"].AsEnumerable()
                              where p1["cDay"].ToString().Trim() == strDayName.Trim() && p1["cType"].ToString().Trim() == ""
                              select new { CWKHOURIT = p1["CROWID"], CDAY = p1["CDAY"], CTYPE = p1["CTYPE"], DBEGTIME = p1["DBEGTIME"], DENDTIME = p1["DENDTIME"] };

                    var ps3 = from p1 in dtsDataEnv.Tables["QWorkHr_Rng"].AsEnumerable()
                              where p1["cDay"].ToString().Trim() == strDayName.Trim() && p1["cType"].ToString().Trim() == "O"
                              select new { CWKHOURIT = p1["CROWID"], CDAY = p1["CDAY"], CTYPE = p1["CTYPE"], DBEGTIME = p1["DBEGTIME"], DENDTIME = p1["DENDTIME"] };

                    int intRecNo = 1;
                    foreach (var psWorkRng in ps2.ToList())
                    {

                        DataRow dtrNewDate = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                        DateTime dttBegTime = Convert.ToDateTime(psWorkRng.DBEGTIME);
                        DateTime dttEndTime = Convert.ToDateTime(psWorkRng.DENDTIME);

                        dttBegTime = new DateTime(dttNewDate.Year, dttNewDate.Month, dttNewDate.Day, dttBegTime.Hour, dttBegTime.Minute, dttBegTime.Second);
                        dttEndTime = new DateTime(dttNewDate.Year, dttNewDate.Month, dttNewDate.Day, dttEndTime.Hour, dttEndTime.Minute, dttEndTime.Second);
                        
                        TimeSpan tsTotHour = dttEndTime - dttBegTime;

                        //
                        dtrNewDate["nRecNo"] = intRecNo++;
                        dtrNewDate["cType"] = psWorkRng.CTYPE.ToString();
                        dtrNewDate["cWorkHr_IT"] = psWorkRng.CWKHOURIT.ToString();
                        dtrNewDate["cDesc"] = strDayName;
                        dtrNewDate["dDate"] = dttNewDate;
                        dtrNewDate["dBegTime"] = dttBegTime;
                        dtrNewDate["dEndTime"] = dttEndTime;
                        dtrNewDate["nTotHour"] = tsTotHour.TotalHours;
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewDate);

                    }

                    intRecNo = 1;
                    foreach (var psWorkRng in ps3.ToList())
                    {

                        DataRow dtrNewDate = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                        DateTime dttBegTime = Convert.ToDateTime(psWorkRng.DBEGTIME);
                        DateTime dttEndTime = Convert.ToDateTime(psWorkRng.DENDTIME);

                        dttBegTime = new DateTime(dttNewDate.Year, dttNewDate.Month, dttNewDate.Day, dttBegTime.Hour, dttBegTime.Minute, dttBegTime.Second);
                        dttEndTime = new DateTime(dttNewDate.Year, dttNewDate.Month, dttNewDate.Day, dttEndTime.Hour, dttEndTime.Minute, dttEndTime.Second);
                        
                        TimeSpan tsTotHour = dttEndTime - dttBegTime;

                        dtrNewDate["nRecNo"] = intRecNo++;
                        dtrNewDate["cType"] = psWorkRng.CTYPE.ToString();
                        dtrNewDate["cWorkHr_IT"] = psWorkRng.CWKHOURIT.ToString();
                        dtrNewDate["cDesc"] = strDayName;
                        dtrNewDate["dDate"] = dttNewDate;
                        dtrNewDate["dBegTime"] = dttBegTime;
                        dtrNewDate["dEndTime"] = dttEndTime;
                        dtrNewDate["nTotHour"] = tsTotHour.TotalHours;
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewDate);

                    }

                }

            }

        
        }

        private void pmSaveData()
        {
            string strErrorMsg = "";
            if (true)
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");

                UIBase.WaitClear();
            }
            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pmUpdateRecord()
        {

            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            DataRow dtrSaveInfo = null;
            objSQLHelper.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0 = 1", ref strErrorMsg);
            
            dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
            bllIsNewRow = true;
            if (this.mstrEditRowID == string.Empty)
            {
                WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
            }
            dtrSaveInfo[MapTable.ShareField.CreateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.CreateDate] = objSQLHelper.GetDBServerDateTime();
            this.dtsDataEnv.Tables[this.mstrRefTable].Rows.Add(dtrSaveInfo);

            this.mstrCode = this.txtDate.DateTime.Year.ToString("0000");
            this.mstrName = this.txtDate.DateTime.Year.ToString("0000");

            // Control Field ที่ทุก Form ต้องมี
            dtrSaveInfo[MapTable.ShareField.RowID] = this.mstrEditRowID;
            dtrSaveInfo[MapTable.ShareField.LastUpdateBy] = App.FMAppUserID;
            dtrSaveInfo[MapTable.ShareField.LastUpdate] = objSQLHelper.GetDBServerDateTime();
            // Control Field ที่ทุก Form ต้องมี

            dtrSaveInfo[QEMStdWorkHourInfo.Field.CorpID] = App.ActiveCorp.RowID;
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Code] = this.mstrCode;
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Name] = this.mstrName;
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Date] = new DateTime(this.txtDate.DateTime.Year, 1, 1);
            dtrSaveInfo[QEMStdWorkHourInfo.Field.Year] = this.txtDate.DateTime.Year;
            dtrSaveInfo[QEMStdWorkHourInfo.Field.PlantID] = this.txtQcPlant.Tag.ToString();

            //Sum work hour and OT hour
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                    where p1["cType"].ToString().Trim() == ""
                                    select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                  where p1["cType"].ToString().Trim() == "O"
                                  select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            dtrSaveInfo["NTOTWORKHR"] = decSumWorkHr;
            dtrSaveInfo["NTOTOTHR"] = decSumOTHr;
            dtrSaveInfo["CWORKHOUR"] = this.txtQcStdWorkHr.Tag.ToString();
            dtrSaveInfo["CHOLIDAY"] = this.txtQcHoliday.Tag.ToString();

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

                this.pmUpdateWorkCalIT();

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

        private void pmUpdateWorkCalIT()
        {

            string strErrorMsg = "";
            object[] pAPara = null;

            var lsGrpWorkHour = from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                        group p1 by p1["dDate"] into g
                        select new { Group = g.Key };

            bool bllIsNewRow = false;
            string strRowID = "";

            foreach (var liWorkHour in lsGrpWorkHour.ToList())
            {

                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0 = 1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                DataRow dtrWorkCalendarIT = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                strRowID = App.mRunRowID(this.mstrITable);
                bllIsNewRow = true;

                dtrWorkCalendarIT["cRowID"] = strRowID;

                this.pmReplRecordWkCalIT(bllIsNewRow, Convert.ToDateTime(liWorkHour.Group), ref dtrWorkCalendarIT);
                this.pmSaveWkCalIT_Range(bllIsNewRow, Convert.ToDateTime(liWorkHour.Group), strRowID);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrWorkCalendarIT, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

            }

        }

        private bool pmReplRecordWkCalIT(bool inState, DateTime inWorkDate, ref DataRow ioWorkHourIT)
        {

            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                  where Convert.ToDateTime(p1["dDate"]) == inWorkDate && p1["cType"].ToString().Trim() == ""
                                  select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                  where Convert.ToDateTime(p1["dDate"]) == inWorkDate && p1["cType"].ToString().Trim() == "O"
                                  select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            DataRow dtrWorkHourIT = ioWorkHourIT;

            string strDayName = "";
            strDayName = MRPAgent.AEngVShortDate[inWorkDate.DayOfWeek.GetHashCode()];
            //switch (inWorkDate.DayOfWeek)
            //{
            //    case DayOfWeek.Monday:
            //        strDayName = MRPAgent.AEngVShortDate[0];
            //        break;
            //    case DayOfWeek.Tuesday:
            //        strDayName = MRPAgent.AEngVShortDate[1];
            //        break;
            //    case DayOfWeek.Wednesday:
            //        strDayName = MRPAgent.AEngVShortDate[2];
            //        break;
            //    case DayOfWeek.Thursday:
            //        strDayName = MRPAgent.AEngVShortDate[3];
            //        break;
            //    case DayOfWeek.Friday:
            //        strDayName = MRPAgent.AEngVShortDate[4];
            //        break;
            //    case DayOfWeek.Saturday:
            //        strDayName = MRPAgent.AEngVShortDate[5];
            //        break;
            //    case DayOfWeek.Sunday:
            //        strDayName = MRPAgent.AEngVShortDate[6];
            //        break;
            //}

            dtrWorkHourIT["cCorp"] = App.ActiveCorp.RowID;
            dtrWorkHourIT["cWorkCalH"] = this.mstrEditRowID;
            dtrWorkHourIT["dDate"] = inWorkDate;
            dtrWorkHourIT["cDay"] = strDayName;
            dtrWorkHourIT["nWorkHr"] = decSumWorkHr;
            dtrWorkHourIT["nOTHr"] = decSumOTHr;
            dtrWorkHourIT["cPlant"] = this.txtQcPlant.Tag.ToString();
            return true;
        }

        private bool pmSaveWkCalIT_Range(bool inState, DateTime inWorkDate, string inWkCalIT)
        {

            string strErrorMsg = "";
            object[] pAPara = null;

            var lsGrpWorkHourDate = from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                    where Convert.ToDateTime(p1["dDate"]) == inWorkDate
                                    select new { dDate = p1["dDate"], dBegTime = p1["dBegTime"], dEndTime = p1["dEndTime"], cType = p1["cType"], nTotHour = p1["nTotHour"], nRecNo = p1["nRecNo"] };

            DataRow dtrBudCI = null;
            foreach (var liGrpHourDate in lsGrpWorkHourDate.ToList())
            {

                this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable2, this.mstrITable2, "select * from " + this.mstrITable2 + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                DataRow dtrWorkCalendarIT_Rng = this.dtsDataEnv.Tables[this.mstrITable2].NewRow();

                string strRowID = App.mRunRowID(this.mstrITable2);
                bool bllIsNewRow = true;

                string strDayName = "";
                strDayName = MRPAgent.AEngVShortDate[inWorkDate.DayOfWeek.GetHashCode()];

                DateTime dttBegTime = Convert.ToDateTime(liGrpHourDate.dBegTime);
                DateTime dttEndTime = Convert.ToDateTime(liGrpHourDate.dEndTime);

                dttBegTime = new DateTime(inWorkDate.Year, inWorkDate.Month, inWorkDate.Day, dttBegTime.Hour, dttBegTime.Minute, dttBegTime.Second);
                dttEndTime = new DateTime(inWorkDate.Year, inWorkDate.Month, inWorkDate.Day, dttEndTime.Hour, dttEndTime.Minute, dttEndTime.Second);

                dtrWorkCalendarIT_Rng["cRowID"] = strRowID;
                dtrWorkCalendarIT_Rng["cCorp"] = App.ActiveCorp.RowID;
                dtrWorkCalendarIT_Rng["cWorkCalH"] = this.mstrEditRowID;
                dtrWorkCalendarIT_Rng["cWorkCalIT"] = inWkCalIT;
                dtrWorkCalendarIT_Rng["cDay"] = strDayName;
                dtrWorkCalendarIT_Rng["dDate"] = inWorkDate;
                dtrWorkCalendarIT_Rng["dBegTime"] = dttBegTime;
                dtrWorkCalendarIT_Rng["dEndTime"] = dttEndTime;
                dtrWorkCalendarIT_Rng["cType"] = liGrpHourDate.cType.ToString();
                dtrWorkCalendarIT_Rng["cPlant"] = this.txtQcPlant.Tag.ToString();
                dtrWorkCalendarIT_Rng["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(liGrpHourDate.nRecNo), 2);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrWorkCalendarIT_Rng, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            }


            return true;
        }

        private void pmClrOldData()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            string strSQLStr = "";
            string strSQLStrDel1 = "";
            string strSQLStrDel2 = "";
            string strSQLStrDel3 = "";
            strSQLStr = "select * from EMWORKCALENDAR where CCORP = ? and CPLANT = ? and CWORKHOUR = ? and CHOLIDAY = ? and NYEAR = ?";
            strSQLStrDel1 = "delete from EMWORKCALENDAR where CROWID = ?  ";
            strSQLStrDel2 = "delete from EMWORKCALENDARIT where CWORKCALH = ? ";
            strSQLStrDel3 = "delete from EMWORKCALENDARIT_RANGE where CWORKCALH = ? ";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcPlant.Tag.ToString(), this.txtQcStdWorkHr.Tag.ToString(), this.txtQcHoliday.Tag.ToString(), this.txtDate.DateTime.Year });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDel_WorkCal", "EMWORKCALENDAR", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrWorkCal = this.dtsDataEnv.Tables["QDel_WorkCal"].Rows[0];
                pobjSQLUtil.SetPara(new object[] { dtrWorkCal["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(strSQLStrDel1, ref strErrorMsg);

                pobjSQLUtil.SetPara(new object[] { dtrWorkCal["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(strSQLStrDel2, ref strErrorMsg);

                pobjSQLUtil.SetPara(new object[] { dtrWorkCal["cRowID"].ToString() });
                pobjSQLUtil.SQLExec(strSQLStrDel3, ref strErrorMsg);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
            //                      where p1["cType"].ToString().Trim() == "O"
            //                      select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            var group = from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                        group p1 by p1["dDate"] into g
                        select new { Group = g.Key };

            MessageBox.Show(group.ToList().Count.ToString());

            this.dataGridView2.DataSource = group.ToList();

            var g2 = group.Take(3);

            foreach (var x in g2.ToList())
            {
                //MessageBox.Show(Convert.ToDateTime(x.Group).ToString());
                //decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                //                      where Convert.ToDateTime(p1["dDate"]) == Convert.ToDateTime(x.Group)
                //                      select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

                var varIT = from p1 in this.dtsDataEnv.Tables[this.mstrTemPd].AsEnumerable()
                                      where Convert.ToDateTime(p1["dDate"]) == Convert.ToDateTime(x.Group)
                                      select new { TotHour = p1["nTotHour"] };

                decimal decSumOTHr = varIT.Sum(p1 => Convert.ToDecimal(p1.TotHour));

                MessageBox.Show(decSumOTHr.ToString());
                //MessageBox.Show(varIT.ToList().Count.ToString());

            }

        }


    }
}
