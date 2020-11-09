
//#define xd_DEBUG

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
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgFilterOption1 : UIHelper.frmBase
    {
 
        private DataSet dtsDataEnv = new DataSet();
        private static dlgFilterOption1 mInstanse = null;

        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DialogForms.dlgGetBook pofrmGetBook = null;
        private DialogForms.dlgGetCrZone pofrmGetCrZone = null;
        private UIHelper.IfrmDBBase pofrmGetWkStat = null;

        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";
        private string mstrCrZone = "";
        private string mstrRefType = "SI";
        private string mstrWkStat = "";
        private bool mbllIsValidateWorkStation = false;

        public dlgFilterOption1(string inRefType)
        {
            InitializeComponent();

            this.mstrRefType = inRefType;
            this.pmInitForm();
        }

        public void SetTitle(string inTitle1, string inTaskName)
        {
            this.lblTitle.Text = inTitle1;
            this.lblTaskName.Text = "TASK NAME : " + inTaskName;
        }

        public string BranchID
        {
            get { return this.txtQcBranch.Tag.ToString(); }
            set 
            { 
                this.txtQcBranch.Tag = value;
                this.pmLoadBranch();
            }
        }

        public string BookID
        {
            get { return this.txtQcBook.Tag.ToString(); }
            set 
            { 
                this.txtQcBook.Tag = value;
                this.pmLoadBook();
            }
        }

        public string QcBook
        {
            get { return this.txtQcBook.Text.TrimEnd(); }
        }

        public string QnBook
        {
            get { return this.txtQnBook.Text.TrimEnd(); }
        }

        public DateTime BeginDate
        {
            get { return this.txtBegDate.DateTime; }
            set { this.txtBegDate.DateTime = value; }
        }

        public DateTime EndDate
        {
            get { return this.txtEndDate.DateTime; }
            set { this.txtEndDate.DateTime = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            this.txtQcBranch.Properties.MaxLength = DialogForms.dlgGetBranch.MAXLENGTH_CODE;
            this.txtQnBranch.Properties.MaxLength = DialogForms.dlgGetBranch.MAXLENGTH_NAME;

            this.txtQcBook.Properties.MaxLength = DialogForms.dlgGetBook.MAXLENGTH_CODE;
            this.txtQnBook.Properties.MaxLength = DialogForms.dlgGetBook.MAXLENGTH_NAME;

            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "Date between :" });
            this.lblToDate.Text = UIBase.GetAppUIText(new string[] { "ถึง", "To :" });

            this.pmDefaultOption();

            if (this.mstrRefType == "SR")
            {
                this.txtQcWkStat.Enabled = false;
                this.pnlWorkStation.Visible = true;
                this.mbllIsValidateWorkStation = true;
            }
            else
            {
                this.mbllIsValidateWorkStation = false;
            }

            if (App.AppUserName.TrimEnd() == "BIGBOSS")
            {
                this.txtQcWkStat.Enabled = true;
            }


        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                this.txtQcBranch.Tag = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                this.txtQcBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcCode"].ToString();
                this.txtQnBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcName"].ToString();
            }

            this.pmDefaultBookCode();

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;

            objSQLHelper.SetPara(new object[] { App.gcCorp, System.Environment.MachineName });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWkStat", "WORKSTATION", "select * from WORKSTATION where fcCorp = ? and fcName = ? ", ref strErrorMsg))
            {
                this.mstrWkStat = this.dtsDataEnv.Tables["QWkStat"].Rows[0]["fcSkid"].ToString();
                this.txtQcWkStat.Tag = this.dtsDataEnv.Tables["QWkStat"].Rows[0]["fcSkid"].ToString();
                this.txtQcWkStat.Text = this.dtsDataEnv.Tables["QWkStat"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnWkStat.Text = this.dtsDataEnv.Tables["QWkStat"].Rows[0]["fcName"].ToString().TrimEnd();
            }
            else
            {
                if (this.mbllIsValidateWorkStation)
                {
                    MessageBox.Show("ไม่พบเครื่อง " + System.Environment.MachineName + " ในระบบ !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.txtQcWkStat.Tag = "";
                this.txtQcWkStat.Text = "";
                this.txtQnWkStat.Text = "";
            }

            UIBase.SetDefaultChildAppreance(this);
        
        }

        private void pmDefaultBookCode()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            string strSQLExec_Book = "select top 1 fcSkid, fcCode, fcName from Book where fcCorp = ? and fcBranch = ? and fcRefType = ? order by fcCode";
            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.mstrRefType});
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", strSQLExec_Book, ref strErrorMsg))
            {
                this.txtQcBook.Tag = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();  
                this.txtQcBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcName"].ToString().TrimEnd();
            }

        }

        private void pmLoadBranch()
        {
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.txtQcBranch.Tag });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from " + MapTable.Table.Branch + " where fcSkid = ?", ref strErrorMsg))
            {
                this.txtQcBranch.Tag = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                this.txtQcBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcName"].ToString().TrimEnd();
            }
        }

        private void pmLoadBook()
        {
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = new cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.txtQcBook.Tag });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select fcSkid, fcCode, fcName from BOOK where fcSkid = ?", ref strErrorMsg))
            {
                this.txtQcBook.Tag = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();
                this.txtQcBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcName"].ToString().TrimEnd();
            }
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
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetBook();
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "CRZONE":
                    if (this.pofrmGetCrZone == null)
                    {
                        this.pofrmGetCrZone = new DialogForms.dlgGetCrZone();
                        this.pofrmGetCrZone.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCrZone.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WKGRP":
                    if (this.pofrmGetWkStat == null)
                    {
                        this.pofrmGetWkStat = new frmWorkStation(FormActiveMode.PopUp);
                        //this.pofrmGetWkStat.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWkStat.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            switch (inTextbox)
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    this.pmInitPopUpDialog("BRANCH");
                    this.pofrmGetBranch.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCCRZONE" ? "FCCODE" : "FCNAME") , true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    this.pofrmGetBook.ValidateField(this.mstrBranch, this.mstrRefType, inPara1, (inTextbox == "TXTQCBOOK" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_PDGRP");
                    break;
                case "TXTQCDEPT":
                case "TXTQNDEPT":
                    this.pmInitPopUpDialog("WKGRP");
                    string strPrefix = (inTextbox == "TXTQCPROJ" ? "FCCODE" : "FCNAME");
                    this.pofrmGetWkStat.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetWkStat.PopUpResult)
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
                        DataRow dtrCrZone = this.pofrmGetBranch.RetrieveValue();

                        if (dtrCrZone != null)
                        {
                            if (this.mstrBranch != dtrCrZone["fcSkid"].ToString())
                            {
                                this.mstrBranch = dtrCrZone["fcSkid"].ToString();
                                this.pmDefaultBookCode();
                            }

                            this.txtQcBranch.Tag = dtrCrZone["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrCrZone["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";
                        }
                    }
                    break;
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    if (this.pofrmGetBook != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetBook.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcBook.Tag = dtrPDGRP["fcSkid"].ToString();
                            this.txtQcBook.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                            this.txtQnBook.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";
                        }
                    }
                    break;
                case "TXTQCWKSTAT":
                case "TXTQNWKSTAT":

                    if (this.pofrmGetWkStat != null)
                    {
                        DataRow dtrJob = this.pofrmGetWkStat.RetrieveValue();
                        this.txtQcWkStat.Tag = dtrJob["fcSkid"].ToString();
                        this.txtQcWkStat.Text = dtrJob["fcCode"].ToString().TrimEnd();
                        this.txtQnWkStat.Text = dtrJob["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcWkStat.Tag = "";
                        this.txtQcWkStat.Text = "";
                        this.txtQnWkStat.Text = "";
                    }
                    break;
            }
        }

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";

            if (txtPopUp.Name.ToUpper() == "TXTQCBRANCH")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                this.txtQcBranch.Tag = "";
                this.txtQcBranch.Text = "";
                this.txtQnBranch.Text = "";

                this.txtQcBook.Tag = "";
                this.txtQcBook.Text = "";
                this.txtQnBook.Text = "";
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

        private void txtQcBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTQCBOOK" )
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTQCBOOK")
                {
                    this.txtQcBook.Tag = "";
                    this.txtQcBook.Text = "";
                    this.txtQnBook.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("BOOK");
                e.Cancel = !this.pofrmGetBook.ValidateField(this.mstrBranch, this.mstrRefType, txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBook.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcProj_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCDEPT" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcWkStat.Tag = "";
                this.txtQcWkStat.Text = "";
                this.txtQnWkStat.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WKGRP");
                e.Cancel = !this.pofrmGetWkStat.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWkStat.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.mbllIsValidateWorkStation)
            {
                if (this.txtQcWkStat.Text.Trim() == "")
                {
                    MessageBox.Show("WORK STATION ไม่ถูกต้อง !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}
