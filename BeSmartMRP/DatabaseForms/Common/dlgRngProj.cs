using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgRngProj : UIHelper.frmBase
    {

        public dlgRngProj()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private UIHelper.IfrmDBBase pofrmGetOption1 = null;
        
        private DataSet dtsDataEnv = new DataSet();

        private string mstrSort = "CCODE";

        public string BegQcJob
        {
            get { return this.txtBegQcProj1.Text.TrimEnd(); }
            set { this.txtBegQcProj1.Text = value; }
        }

        public string EndQcJob
        {
            get { return this.txtEndQcProj1.Text.TrimEnd(); }
            set { this.txtEndQcProj1.Text = value; }
        }

        public bool IsPrintAllSect
        {
            get { return this.chkIsAll.Checked; }
        }

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strErrorMsg = "";

            this.txtBegQcProj1.Text = "";
            this.txtEndQcProj1.Text = "";

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "EMJOB":
                    if (this.pofrmGetOption1 == null)
                    {
                        this.pofrmGetOption1 = new frmEMJob(UIHelper.FormActiveMode.Report);
                        //this.pofrmGetOption1.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetOption1.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTBEGQCPROJ1":
                case "TXTENDQCPROJ1":
                    this.pmInitPopUpDialog("EMJOB");
                    this.pofrmGetOption1.ValidateField(inPara1, this.mstrSort, true);
                    if (this.pofrmGetOption1.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            DataRow dtrRetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGQCPROJ1":
                    dtrRetVal = this.pofrmGetOption1.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtBegQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtBegQcProj1.Text = "";
                    }
                    break;
                case "TXTENDQCPROJ1":
                    dtrRetVal = this.pofrmGetOption1.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtEndQcProj1.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtEndQcProj1.Text = "";
                    }
                    break;
            }
        }

        private void txtQcProj1_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = this.mstrSort;

            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("EMJOB");
                e.Cancel = !this.pofrmGetOption1.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetOption1.PopUpResult)
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
