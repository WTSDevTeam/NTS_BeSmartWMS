
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

    public partial class dlgFilter01 : UIHelper.frmBase
    {
    
        public dlgFilter01(string inRefType)
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
            get { return this.txtQcBranch.Tag.ToString(); }
        }

        public string BranchCode
        {
            get { return this.txtQcBranch.Text; }
        }

        public string BGBookID
        {
            get { return this.txtQcBGBook.Tag.ToString(); }
        }

        public string BGBookCode
        {
            get { return this.txtQcBGBook.Text; }
        }

        public string BGBookName
        {
            get { return this.txtQnBGBook.Text; }
        }

        public string mYear
        {
            get { return this.txtYear.Text.ToString(); }
        }

        public int nYear
        {
            get { return this.mintYear; }
        }

        private DatabaseForms.frmBranch pofrmGetBranch = null;
        private DatabaseForms.frmBGBook pofrmGetBGBook = null;
        private DatabaseForms.frmBGYear pofrmGetBGYear = null;

        private string mstrRefType = "";
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBGBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Code);
            this.txtQnBGBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Name);

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "EMBRANCH", "select cRowID, cCode, cName from EMBRANCH where cCorp = ? order by CCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["cRowID"].ToString();
                this.txtQcBranch.Text = dtrBranch["cCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["cName"].ToString().TrimEnd();

                this.pmDefaultBook();

            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGYear", "BGYEAR", "select * from BGYEAR where CCORP = ? order by NYEAR desc", ref strErrorMsg))
            {
                DataRow dtrBGYear = this.dtsDataEnv.Tables["QBGYear"].Rows[0];
                this.txtYear.Text = dtrBGYear["cCode"].ToString().TrimEnd();
                this.mintYear = Convert.ToInt32(dtrBGYear["nYear"]);
            }

        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.mstrRefType });
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
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new frmBranch(FormActiveMode.PopUp);
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGBOOK":
                    if (this.pofrmGetBGBook == null)
                    {
                        //this.pofrmGetBGBook = new DialogForms.dlgGetBGBook(SysDef.gc_REFTYPE_BUDALLOC);
                        this.pofrmGetBGBook = new frmBGBook(FormActiveMode.PopUp, SysDef.gc_REFTYPE_BUDALLOC);
                        this.pofrmGetBGBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGYEAR":
                    if (this.pofrmGetBGYear == null)
                    {
                        this.pofrmGetBGYear = new frmBGYear(FormActiveMode.PopUp);
                        this.pofrmGetBGYear.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGYear.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCBGBOOK":
                case "TXTQNBGBOOK":
                    this.pmInitPopUpDialog("BGBOOK");
                    strOrderBy = (inTextbox == "TXTQCBGBOOK" ? "CCODE" : "CNAME");
                    this.pofrmGetBGBook.ValidateField(this.txtQcBranch.Tag.ToString(), "", strOrderBy, true);
                    if (this.pofrmGetBGBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTYEAR":
                    this.pmInitPopUpDialog("BGYEAR");
                    strOrderBy = "CCODE";
                    this.pofrmGetBGYear.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetBGYear.PopUpResult)
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
                                this.pmDefaultBook();
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
                        }
                    }
                    break;
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
                case "TXTYEAR":
                    if (this.pofrmGetBGYear != null)
                    {
                        DataRow dtrYear = this.pofrmGetBGYear.RetrieveValue();

                        if (dtrYear != null)
                        {
                            this.txtYear.Tag = dtrYear["cRowID"].ToString();
                            this.txtYear.Text = dtrYear["cCode"].ToString().TrimEnd();
                            this.mintYear = Convert.ToInt32(dtrYear["nYear"]);
                        }
                        else
                        {
                            this.txtYear.Tag = "";
                            this.txtYear.Text = "";
                            this.mintYear = 0;
                        }
                    }
                    break;
            }
        }

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBRANCH" ? "CCODE" : "CNAME";

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
                e.Cancel = !this.pofrmGetBGBook.ValidateField(this.txtQcBranch.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                this.txtYear.Tag = "";
                this.txtYear.Text = "";
                this.mintYear = 0;
            }
            else
            {
                this.pmInitPopUpDialog("BGYEAR");
                e.Cancel = !this.pofrmGetBGYear.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBGYear.PopUpResult)
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
