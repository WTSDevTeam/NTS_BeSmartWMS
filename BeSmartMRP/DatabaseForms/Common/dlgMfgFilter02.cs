using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;


namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgMfgFilter02 : UIHelper.frmBase
    {

        public dlgMfgFilter02()
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

        public string PlantID
        {
            get { return this.txtQcPlant.Tag.ToString(); }
        }

        public string PlantCode
        {
            get { return this.txtQcPlant.Text; }
        }

        public string PlantName
        {
            get { return this.txtQnPlant.Text; }
        }

        public string HolidayID
        {
            get { return this.txtQcHoliday.Tag.ToString(); }
        }

        public string StdWorkHrID
        {
            get { return this.txtQcStdWorkHr.Tag.ToString(); }
        }

        public int Year
        {
            get { return this.txtDate.DateTime.Year; }
        }

        private string mstrHTable = MapTable.Table.EMWorkCalendar;
        private string mstrRefTable = MapTable.Table.EMWorkCalendar;
        private string mstrITable = MapTable.Table.EMWorkCalendarItem;
        private string mstrITable2 = MapTable.Table.EMWorkCalendarItem_Range;

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DatabaseForms.frmEMHoliday pofrmEMHoliday = null;
        private DatabaseForms.frmEMStdWorkHour pofrmEMStdWorkHour = null;

        private DateTime mdttCurrDate = DateTime.MinValue;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = DocumentType.MC.ToString();
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcHoliday.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);

            this.txtQcPlant.Tag = "";
            this.txtQcHoliday.Tag = "";
            this.txtQcStdWorkHr.Tag = "";
            this.txtQcHoliday.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";

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

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmMapEvent()
        {

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            this.txtQcHoliday.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcHoliday.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcHoliday.Validating += new CancelEventHandler(txtQcHoliday_Validating);

            this.txtQcStdWorkHr.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcStdWorkHr.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcStdWorkHr.Validating += new CancelEventHandler(txtQcStdWorkHr_Validating);

            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            //this.btnOK.Click += new EventHandler(btnPrint_Click);

        }

        private void pmSetFormUI()
        {
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ประจำปี :", "Year :" });
            this.lblHoliday.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลวันหยุดประจำปี :", "Select Holiday and NoworkingDay :" });
            this.lblStdWorkHr.Text = UIBase.GetAppUIText(new string[] { "ระบุข้อมูลเวลาทำงานมาตรฐาน :", "Select Stadard Work Hour :" });

            this.btnOK.Text = UIBase.GetAppUIText(new string[] { "ตกลง", "OK" });
            this.btnCancel.Text = UIBase.GetAppUIText(new string[] { "ยกเลิก", "Cancel" });
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
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

    }
}

