
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
    public partial class dlgGetPROption02 : UIHelper.frmBase
    {

        public dlgGetPROption02(string inRefType, string inPlant, string inMOBook)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.mstrMOBook = inMOBook;
            this.mstrPlant = inPlant;

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
            set 
            { 
                this.txtQcBranch.Tag = value;

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                objSQLHelper.SetPara(new object[] { this.txtQcBranch.Tag.ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where FCSKID = ? ", ref strErrorMsg))
                {

                    DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                    this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                    this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                    this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();
                    this.pmDefaultBook();
                }
            }
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

        public string BranchCode
        {
            get { return this.txtQcBranch.Text; }
        }

        public string BranchName
        {
            get { return this.txtQnBranch.Text; }
        }

        public string CoorID
        {
            get { return this.txtQcCoor.Tag.ToString(); }
        }

        public string CoorCode
        {
            get { return this.txtQcCoor.Text; }
        }

        public string CoorName
        {
            get { return this.txtQnCoor.Text; }
        }

        public string BookID
        {
            get { return this.txtQcBook.Tag.ToString(); }
        }

        public string BookCode
        {
            get { return this.txtQcBook.Text; }
        }

        public string BookName
        {
            get { return this.txtQnBook.Text; }
        }

        public DateTime DocDate
        {
            get { return this.txtBegDate.DateTime.Date; }
        }

        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DialogForms.dlgGetFMBook pofrmGetBook = null;
        private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DialogForms.dlgGetDocMO pofrmGetDoc = null;

        private string mstrRefType = "PR";

        private string mstrPlant = "";
        private string mstrMOBook = "";

        private string mstrSect = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            this.cmbOrderBy.SelectedIndexChanged += new EventHandler(cmbOrderBy_SelectedIndexChanged);

            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new CancelEventHandler(txtQcBranch_Validating);

            this.txtQcCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcCoor.Validating += new CancelEventHandler(txtQcCoor_Validating);

            this.txtQcBook.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBook.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBook.Validating += new CancelEventHandler(txtQcMfgBook_Validating);

            this.txtBegDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            this.txtEndDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);
            this.txtQnBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Name);

            this.txtQcBook.Tag = "";
            this.txtQcBook.Text = "";
            this.txtQnBook.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where FCCORP = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

            }

            this.txtBegDate.DateTime = DateTime.Now;

            this.txtMOBegDate.DateTime = DateTime.Now;
            this.txtMOEndDate.DateTime = DateTime.Now;

            this.pmDefaultBook();

            this.txtBegDate.DateTime = DateTime.Now;

            this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "ระบุ Option การสร้าง PR", "Select Generate PR Option" });

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

            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblCoor.Text = UIBase.GetAppUIText(new string[] { "ระบุผู้จำหน่าย :", "Supplier Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "Document Date :" });

            UIBase.SetDefaultChildAppreance(this);
        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select fcSkid, fcCode, fcName from Book where fcCorp = ? and fcBranch = ? and fcRefType  = ? order by FCCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcBook.Tag = dtrBook["fcSkid"].ToString();
                this.txtQcBook.Text = dtrBook["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = dtrBook["fcName"].ToString().TrimEnd();

            }
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
                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor(CoorType.Supplier);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetFMBook(this.mstrRefType);
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "DOC_MO":
                    if (this.pofrmGetDoc == null)
                    {
                        this.pofrmGetDoc = new DialogForms.dlgGetDocMO();
                        this.pofrmGetDoc.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                        this.pofrmGetDoc.FIlterStep = " and MFWORDERHD.CMSTEP <> 'P' and MFWORDERHD.CSTAT <> 'C' and MFWORDERHD.CPRSTEP = ' ' ";
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
                case "TXTQCCOOR":
                case "TXTQNCOOR":
                    this.pmInitPopUpDialog("COOR");
                    strOrderBy = (inTextbox == "TXTQCCOOR" ? "FCCODE" : "FCNAME");
                    this.pofrmGetCoor.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetCoor.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    strOrderBy = (inTextbox == "TXTQCBOOK" ? "FCCODE" : "FCNAME");
                    this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), "", strOrderBy, true);
                    if (this.pofrmGetBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGDOC":
                case "TXTENDDOC":
                    this.pmInitPopUpDialog("DOC_MO");
                    strOrderBy = "CCODE";
                    this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.mstrPlant, DocumentType.MO.ToString(), this.mstrMOBook, "", strOrderBy, true);
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

                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";
                        
                        }
                    }
                    break;
                case "TXTQCCOOR":
                case "TXTQNCOOR":

                    DataRow dtrGetVal = this.pofrmGetCoor.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        this.txtQcCoor.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcCoor.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnCoor.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcCoor.Tag = "";
                        this.txtQcCoor.Text = "";
                        this.txtQnCoor.Text = "";

                        this.txtQcBook.Tag = "";
                        this.txtQcBook.Text = "";
                        this.txtQnBook.Text = "";
                    
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

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBRANCH" ? "FCCODE" : "FCNAME";

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

        private void txtQcCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCCOOR" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcCoor.Tag = "";
                this.txtQcCoor.Text = "";
                this.txtQnCoor.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcMfgBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBOOK" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBook.Tag = "";
                this.txtQcBook.Text = "";
                this.txtQnBook.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BOOK");
                e.Cancel = !this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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
                e.Cancel = !this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.mstrPlant, DocumentType.MO.ToString(), this.mstrMOBook, txtPopUp.Text, strOrderBy, false);
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
            if (this.txtQcBranch.Text.Trim() == "")
            {
                strErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุสาขา !", "Branch Code is not define !" });
                this.txtQcBranch.Focus();
            }
            else if (this.txtQcBook.Text.Trim() == "")
            {
                strErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุเล่มเอกสาร !", "Book Code is not define !" });
                this.txtQcBook.Focus();
            }
            else if (this.txtQcCoor.Text.Trim() == "")
            {
                strErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุผู้จำหน่าย !", "Supplier Code is not define !" });
                this.txtQcCoor.Focus();
            }
            else
            {
                bllResult = true;
            }

            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            return bllResult;
        }

    }
}
