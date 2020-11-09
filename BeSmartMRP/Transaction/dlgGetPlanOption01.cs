
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

namespace BeSmartMRP.Transaction
{
    public partial class dlgGetPlanOption01 : UIHelper.frmBase
    {


        public dlgGetPlanOption01(string inBranch, string inPlant, string inMOBook)
        {
            InitializeComponent();
            //this.mstrRefType = inRefType;
            this.mstrMOBook = inMOBook;
            this.mstrPlant = inPlant;
            this.mstrBranch = inBranch;

            this.pmInitForm();
        }

        public void SetTitle(string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
        }

        public string OrderBy
        {
            get 
            {
                string strResult = "";
                strResult = (this.cmbOrderBy.SelectedIndex == 0 ? "DDATE" : "CCODE");
                return strResult; 
            }
        }

        public string MO_BegCode
        {
            get { return this.txtBegDoc.Text.TrimEnd(); }
        }

        public string MO_EndCode
        {
            get { return this.txtEndDoc.Text.TrimEnd(); }
        }

        public DateTime MO_BegDate
        {
            get { return this.txtMOBegDate.DateTime.Date; }
        }

        public DateTime MO_EndDate
        {
            get { return this.txtMOEndDate.DateTime.Date; }
        }

        private DialogForms.dlgGetDocMO pofrmGetDoc = null;

        private string mstrRefType = "PR";

        private string mstrBranch = "";
        private string mstrPlant = "";
        private string mstrMOBook = "";

        private string mstrSect = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            this.cmbOrderBy.SelectedIndexChanged += new EventHandler(cmbOrderBy_SelectedIndexChanged);

            this.txtBegDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            this.txtEndDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";

            this.txtMOBegDate.DateTime = DateTime.Now;
            this.txtMOEndDate.DateTime = DateTime.Now;

            this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "ระบุ Option การสร้าง ตารางการผลิต", "Select Generate Planning Option" });

            this.cmbOrderBy.Properties.Items.Clear();
            this.cmbOrderBy.Properties.Items.AddRange(new object[] { 
                                                                                UIBase.GetAppUIText(new string[] { "ช่วงวันที่", "By Date" })
                                                                                ,UIBase.GetAppUIText(new string[] { "ช่วงเลขที่", "By MO Code" }) });

            this.cmbOrderBy.SelectedIndex = 0;

            this.lblRng.Text = UIBase.GetAppUIText(new string[] { "เลือกสร้างเอกสารตาม :", "Generate P/R Order :" });
            this.lblRngDate_MO.Text = UIBase.GetAppUIText(new string[] { "ระบุช่วงวันที่เอกสาร MO", "Specify range MO date" });
            this.lblRngDate_Beg.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "From Date :" });
            this.lblRngDate_End.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngDoc_MO.Text = UIBase.GetAppUIText(new string[] { "ระบุช่วงเลขที่เอกสาร MO", "Specify range MO Code" });
            this.lblRngDoc_Beg.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่เลขที่ :", "From Code :" });
            this.lblRngDoc_End.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            UIBase.SetDefaultChildAppreance(this);
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "DOC_MO":
                    if (this.pofrmGetDoc == null)
                    {
                        this.pofrmGetDoc = new DialogForms.dlgGetDocMO();
                        this.pofrmGetDoc.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                        this.pofrmGetDoc.FIlterStep = " and MFWORDERHD.CMSTEP <> 'P' and MFWORDERHD.CSTAT <> 'C' ";
                    }
                    break;
            }
        }

        private void pmSetRngBy()
        {
            if (this.cmbOrderBy.SelectedIndex == 0)
            {
                this.pnlRngDate.Enabled = true;
                this.pnlRngDoc.Enabled = false;
            }
            else
            {
                this.pnlRngDate.Enabled = false;
                this.pnlRngDoc.Enabled = true;
            }
        }

        private void cmbOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetRngBy();
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
                case "TXTBEGDOC":
                case "TXTENDDOC":
                    this.pmInitPopUpDialog("DOC_MO");
                    strOrderBy = "CCODE";
                    this.pofrmGetDoc.ValidateField(this.mstrBranch, this.mstrPlant, DocumentType.MO.ToString(), this.mstrMOBook, "", strOrderBy, true);
                    if (this.pofrmGetDoc.PopUpResult)
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
                case "TXTBEGDOC":
                case "TXTENDDOC":
                    if (this.pofrmGetDoc != null)
                    {
                        DataRow dtrMO = this.pofrmGetDoc.RetrieveValue();

                        if (dtrMO != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGDOC")
                            {
                                this.txtBegDoc.Text = dtrMO["cCode"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndDoc.Text = dtrMO["cCode"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGDOC")
                            {
                                this.txtBegDoc.Text = "";
                            }
                            else
                            {
                                this.txtEndDoc.Text = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void txtBegDoc_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("DOC_MO");
                e.Cancel = !this.pofrmGetDoc.ValidateField(this.mstrBranch, this.mstrPlant, DocumentType.MO.ToString(), this.mstrMOBook, txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetDoc.PopUpResult)
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
            if (pmValidOption())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool pmValidOption()
        {
            bool bllResult = false;
            string strErrorMsg = "";
            bllResult = true;

            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            return bllResult;
        }

    }
}
