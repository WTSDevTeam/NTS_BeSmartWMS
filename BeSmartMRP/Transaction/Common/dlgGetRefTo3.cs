
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


namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetRefTo3 : UIHelper.frmBase
    {

        public dlgGetRefTo3(string inRefType)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.pmInitForm();
        }

        public void SetTitle(string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
        }

        public string BranchID
        {
            set 
            { 
                this.mstrBranch = value;
                this.pmDefaultBook();
            }
        }

        public string BGBookID
        {
            get { return this.txtQcBGBook.Tag.ToString(); }
        }

        public string BGBookCode
        {
            get { return this.txtQcBGBook.Text; }
        }

        public string DocID
        {
            get { return this.txtCode.Tag.ToString(); }
        }

        public string DocCode
        {
            get { return this.txtCode.Text.TrimEnd(); }
        }

        public string BGBookName
        {
            get { return this.txtQnBGBook.Text; }
        }

        public int nYear
        {
            get { return this.mintYear; }
            set { this.mintYear = value; }
        }

        public string JobID
        {
            set { this.mstrJob = value; }
        }

        private DatabaseForms.frmBGBook pofrmGetBGBook = null;
        private DialogForms.dlgGetDoc2 pofrmGetDoc1 = null;

        private string mstrRefType = "";
        private int mintYear = 0;
        private string mstrBranch = "";
        private string mstrSect = "";
        private string mstrJob = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBGBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Code);
            this.txtQnBGBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Name);

            this.txtQcBGBook.Tag = "";
            this.txtQcBGBook.Text = "";
            this.txtQnBGBook.Text = "";

            int intYear = DateTime.Now.Year;

            this.pmDefaultBook();

        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BGBook", "select cRowID, cCode, cName from BGBOOK where cCorp = ? and cBranch = ? and cRefType  = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcBGBook.Tag = dtrBook["cRowID"].ToString();
                this.txtQcBGBook.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnBGBook.Text = dtrBook["cName"].ToString().TrimEnd();

            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BGBOOK":
                    if (this.pofrmGetBGBook == null)
                    {
                        this.pofrmGetBGBook = new frmBGBook(FormActiveMode.PopUp, this.mstrRefType);
                        this.pofrmGetBGBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "RECVDOC":
                    if (this.pofrmGetDoc1 == null)
                    {
                        this.pofrmGetDoc1 = new DialogForms.dlgGetDoc2();
                        this.pofrmGetDoc1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCBGBOOK":
                case "TXTQNBGBOOK":
                    this.pmInitPopUpDialog("BGBOOK");
                    strOrderBy = (inTextbox == "TXTQCBGBOOK" ? "CCODE" : "CNAME");
                    this.pofrmGetBGBook.ValidateField(this.mstrBranch, "", strOrderBy, true);
                    if (this.pofrmGetBGBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTCODE":
                    this.pmInitPopUpDialog("RECVDOC");
                    this.pofrmGetDoc1.ValidateField(this.mstrBranch, this.mstrRefType, this.txtQcBGBook.Tag.ToString(), this.mstrJob, this.mintYear, "", "CCODE", true);
                    if (this.pofrmGetDoc1.PopUpResult)
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
                case "TXTQCBGBOOK":
                case "TXTQNBGBOOK":
                    if (this.pofrmGetBGBook != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetBGBook.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcBGBook.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcBGBook.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnBGBook.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBGBook.Tag = "";
                            this.txtQcBGBook.Text = "";
                            this.txtQnBGBook.Text = "";
                        }
                    }
                    break;
                case "TXTCODE":
                    if (this.pofrmGetDoc1 != null)
                    {
                        DataRow dtrDoc1 = this.pofrmGetDoc1.RetrieveValue();

                        if (dtrDoc1 != null)
                        {
                            this.txtCode.Tag = dtrDoc1["cRowID"].ToString();
                            this.txtCode.Text = dtrDoc1["cCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtCode.Tag = "";
                            this.txtCode.Text = "";
                        }
                    }
                    break;
            }
        }

        private void txtQcBGBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBGBOOK" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBGBook.Tag = "";
                this.txtQcBGBook.Text = "";
                this.txtQnBGBook.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BGBOOK");
                e.Cancel = !this.pofrmGetBGBook.ValidateField(this.mstrBranch, txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBGBook.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtCode_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                this.txtCode.Tag = "";
            }
            else
            {
                this.pmInitPopUpDialog("RECVDOC");
                e.Cancel = !this.pofrmGetDoc1.ValidateField(this.mstrBranch, this.mstrRefType, this.txtQcBGBook.Tag.ToString(), this.mstrJob, this.mintYear, this.txtCode.Text, strOrderBy, false);
                if (this.pofrmGetDoc1.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

    }
}
