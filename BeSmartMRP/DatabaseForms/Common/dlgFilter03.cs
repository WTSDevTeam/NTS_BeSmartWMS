
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

    public partial class dlgFilter03 : UIHelper.frmBase
    {

        public dlgFilter03()
        {
            InitializeComponent();
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

        public string SectID
        {
            get { return this.txtQcSect.Tag.ToString(); }
        }

        public string SectCode
        {
            get { return this.txtQcSect.Text; }
        }

        public string SectName
        {
            get { return this.txtQnSect.Text; }
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
        //private DatabaseForms.frmEMSect pofrmGetSect = null;
        //private DatabaseForms.frmBGYear pofrmGetBGYear = null;


        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetBGYear = null;
        
        //private DialogForms.dlgGetSect pofrmGetSect = null;
        private int mintYear = 0;
        
        private string mstrRefType = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcSect.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QEMSectInfo.TableName, QEMSectInfo.Field.Code);
            this.txtQnSect.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QEMSectInfo.TableName, QEMSectInfo.Field.Name);
            this.txtYear.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QBGYearInfo.TableName, QBGYearInfo.Field.Code);

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "EMBRANCH", "select cRowID, cCode, cName from EMBRANCH where cCorp = ? order by CCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["cRowID"].ToString();
                this.txtQcBranch.Text = dtrBranch["cCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["cName"].ToString().TrimEnd();

            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select EMSECT.CROWID, EMSECT.CCODE, EMSECT.CNAME from EMSECT where CCORP = ? by cCode", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtQcSect.Tag = dtrBook["CROWID"].ToString();
                this.txtQcSect.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnSect.Text = dtrBook["cName"].ToString().TrimEnd();

            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGYear", "BGYEAR", "select * from BGYEAR where CCORP = ? order by NYEAR desc", ref strErrorMsg))
            {
                DataRow dtrBGYear = this.dtsDataEnv.Tables["QBGYear"].Rows[0];
                this.txtYear.Text = dtrBGYear["cCode"].ToString().TrimEnd();
                this.mintYear = Convert.ToInt32(dtrBGYear["nYear"]);
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
                case "EMSECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new frmEMSect(FormActiveMode.PopUp);
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGYEAR":
                    if (this.pofrmGetBGYear == null)
                    {
                        this.pofrmGetBGYear = new frmBGYear(FormActiveMode.PopUp);
                        //this.pofrmGetBGYear.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGYear.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                    this.pofrmGetBranch.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCSECT":
                case "TXTQNSECT":
                    this.pmInitPopUpDialog("EMSECT");
                    string strPrefix = (inTextbox == "TXTQCSECT" ? "CCODE" : "CNAME");
                    this.pofrmGetSect.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetSect.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTYEAR":
                    this.pmInitPopUpDialog("BGYEAR");
                    this.pofrmGetBGYear.ValidateField("", "CCODE", true);
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
                            this.txtQcBranch.Tag = dtrBranch["cRowID"].ToString();
                            this.txtQcBranch.Text = dtrBranch["cCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrBranch["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";
                        }
                    }
                    break;
                case "TXTQCSECT":
                case "TXTQNSECT":
                    if (this.pofrmGetSect != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetSect.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcSect.Tag = dtrPDGRP["CROWID"].ToString();
                            this.txtQcSect.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnSect.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcSect.Tag = "";
                            this.txtQcSect.Text = "";
                            this.txtQnSect.Text = "";
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

        private void txtQcSect_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSECT" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcSect.Tag = "";
                this.txtQcSect.Text = "";
                this.txtQnSect.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("EMSECT");
                e.Cancel = !this.pofrmGetSect.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetSect.PopUpResult)
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
