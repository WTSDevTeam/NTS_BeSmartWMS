using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common.Order
{
    public partial class dlgOrdGetRefTo : UIHelper.frmBase
    {

        public dlgOrdGetRefTo(DocumentType inDocType)
        {
            InitializeComponent();

            this.mDocType = inDocType;
            this.mstrRefTypeList = this.mDocType.ToString();
            this.pmInitForm();
        
        }

        public dlgOrdGetRefTo(DocumentType inDocType, string inRefTypeList)
        {
            InitializeComponent();

            this.mDocType = inDocType;
            this.mstrRefTypeList = inRefTypeList;
            this.pmInitForm();
        
        }
        
        private DataSet dtsDataEnv = new DataSet();
        private DocumentType mDocType = DocumentType.SO;
        private BeSmartMRP.Business.Entity.CoorType mCoorType = CoorType.Customer;

        private DialogForms.dlgGetBook pofrmGetBook = null;
        private DialogForms.dlgGetOrder pofrmGetDoc = null;
        //private DialogForms.dlgGetRefType pofrmGetRefType = null;

        private string mstrBranchID = "";
        private string mstrCoorID = "";
        private bool mbllShowAllStep = false;
        private string mstrLastCode = "";

        private string mstrRefTypeList = "";

        public void SetTitle(string inTitle)
        {
            this.lblTitle.Text = inTitle;
        }

        public string BranchID
        {
            get { return this.mstrBranchID; }
            set
            {
                this.mstrBranchID = value;
                this.pmDefaultOption();
            }
        }

        public string CoorID
        {
            get { return this.mstrCoorID; }
            set { this.mstrCoorID = value; }
        }

        public DocumentType RefDocumentType
        {
            get { return this.mDocType; }
            set { this.mDocType = value; }
        }

        public string RefTypeList
        {
            get { return this.mstrRefTypeList; }
            set { this.mstrRefTypeList = value; }
        }

        public BeSmartMRP.Business.Entity.CoorType CoorType
        {
            get { return this.mCoorType; }
            set { this.mCoorType = value; }
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

        public string BookCode
        {
            get { return this.txtQcBook.Text.ToString(); }
        }

        public string BookName
        {
            get { return this.txtQnBook.Text.ToString(); }
        }

        private string mstrSaveDocID = "";
        public string RefToDocumentID
        {
            get { return this.txtCode.Tag.ToString(); }
            set
            {
                this.txtCode.Tag = value;
                this.mstrSaveDocID = this.txtCode.Tag.ToString();
                this.pmLoadCode();
            }
        }

        public bool IsShowAllStep
        {
            set { this.mbllShowAllStep = value; }
        }

        private void pmInitForm()
        {
            this.pmInitializeComponent();
            this.pmDefaultOption();
        }

        private void pmInitializeComponent()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mDocType.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefType", "REFTYPE", "select * from RefType where fcSkid = ?", ref strErrorMsg))
            {
                DataRow dtrRefType = this.dtsDataEnv.Tables["QRefType"].Rows[0];
                this.txtQcRefType.Tag = dtrRefType["fcCode"].ToString().TrimEnd();
                this.txtQcRefType.Text = dtrRefType["fcCode"].ToString().TrimEnd();
                this.txtQnRefType.Text = dtrRefType["fcName"].ToString().TrimEnd();
            }

            this.txtQcBook.Properties.MaxLength = 4;
        }

        private void pmLoadBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.txtQcBook.Tag });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select fcSkid, fcCode, fcName from Book where fcSkid = ?", ref strErrorMsg))
            {
                this.txtQcBook.Tag = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();
                this.txtQcBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcName"].ToString().TrimEnd();
            }
        }

        private void pmLoadCode()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { this.txtCode.Tag });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QOrder", MapTable.Table.OrderH, "select fcCode, fcRefNo from OrderH where fcSkid = ?", ref strErrorMsg))
            {
                this.txtCode.Text = this.dtsDataEnv.Tables["QOrder"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtRefNo.Text = this.dtsDataEnv.Tables["QOrder"].Rows[0]["fcRefNo"].ToString().TrimEnd();
                this.mstrLastCode = this.txtCode.Text;
            }
        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.txtQcRefType.Tag });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select top 1 fcSKid, fcCode, fcName from Book where fcCorp = ? and fcBranch = ? and fcRefType = ? order by fcCode", ref strErrorMsg))
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
                case "REFTYPE":
                    //if (this.pofrmGetRefType == null)
                    //{
                    //    this.pofrmGetRefType = new DialogForms.dlgGetRefType(this.mstrRefTypeList);
                    //    this.pofrmGetRefType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetRefType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    //}
                    //break;
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetBook();
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "DOC":
                    if (this.pofrmGetDoc == null)
                    {
                        this.pofrmGetDoc = new DialogForms.dlgGetOrder();
                        this.pofrmGetDoc.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        this.pofrmGetDoc.CoorType = this.mCoorType;
                        this.pofrmGetDoc.BranchID = this.mstrBranchID;
                        this.pofrmGetDoc.RefType = this.mDocType.ToString();
                        this.pofrmGetDoc.CoorID = this.mstrCoorID;
                        this.pofrmGetDoc.BookID = this.txtQcBook.Tag.ToString();
                        this.pofrmGetDoc.CoorType = this.mCoorType;
                        this.pofrmGetDoc.IsShowAllStep = this.mbllShowAllStep;
                        this.pofrmGetDoc.RefreshBrowView();
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "BOOK":
                    if (this.pofrmGetBook != null)
                    {
                        DataRow dtrPopup = this.pofrmGetBook.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            if (this.txtQcBook.Tag != dtrPopup["fcSkid"].ToString())
                            {
                                this.txtCode.Tag = "";
                                this.txtCode.Text = "";
                                this.txtRefNo.Text = "";
                            }

                            this.txtQcBook.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtQcBook.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtQnBook.Text = dtrPopup["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";

                            this.txtCode.Tag = "";
                            this.txtCode.Text = "";
                            this.txtRefNo.Text = "";
                        }
                        this.mstrLastCode = this.txtCode.Text;
                    }
                    break;
                case "TXTCODE":
                case "TXTREFNO":
                    if (this.pofrmGetDoc != null)
                    {
                        DataRow dtrPopup = this.pofrmGetDoc.RetrieveValue();
                        if (dtrPopup != null)
                        {
                            this.txtCode.Tag = dtrPopup["fcSkid"].ToString();
                            this.txtCode.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                            this.txtRefNo.Text = dtrPopup["fcRefNo"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtCode.Tag = "";
                            this.txtCode.Text = "";
                            this.txtRefNo.Text = "";
                        }
                        this.mstrLastCode = this.txtCode.Text;
                    }
                    break;
                //case "REFTYPE":
                    //if (this.pofrmGetRefType != null)
                    //{
                    //    DataRow dtrPopup = this.pofrmGetRefType.RetrieveValue();
                    //    if (dtrPopup != null)
                    //    {
                    //        if (this.txtQcRefType.Tag != dtrPopup["fcSkid"].ToString())
                    //        {
                    //            this.txtQcRefType.Tag = dtrPopup["fcSkid"].ToString();
                    //            this.pmDefaultOption();
                    //        }
                    //        this.txtQcRefType.Text = dtrPopup["fcCode"].ToString().TrimEnd();
                    //        this.txtQnRefType.Text = dtrPopup["fcName"].ToString().TrimEnd();
                    //        this.mDocType = BusinessEnum.GetDocEnum(this.txtQcRefType.Text);
                    //    }
                    //    else
                    //    {
                    //        this.txtQcRefType.Tag = "";
                    //        this.txtQcRefType.Text = "";
                    //        this.txtQnRefType.Text = "";

                    //        this.txtQcBook.Tag = "";
                    //        this.txtQcBook.Text = "";
                    //        this.txtQnBook.Text = "";

                    //        this.txtCode.Tag = "";
                    //        this.txtCode.Text = "";
                    //        this.txtRefNo.Text = "";
                    //        this.mstrLastCode = this.txtCode.Text;
                    //    }
                    //}
                    //break;
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

        private void pmPopUpButtonClick(string inTextbox, string inValue)
        {
            switch (inTextbox)
            {
                //case "TXTQCREFTYPE":
                //    this.pmInitPopUpDialog("REFTYPE");
                //    this.pofrmGetRefType.ValidateField("", "FCCODE", true);
                //    if (this.pofrmGetRefType.PopUpResult)
                //    {
                //        this.pmRetrievePopUpVal("REFTYPE");
                //    }
                //    break;
                case "TXTQCBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    this.pofrmGetBook.ValidateField(this.mstrBranchID, this.txtQcRefType.Tag.ToString(), "", "fcCode", true);
                    if (this.pofrmGetBook.PopUpResult)
                        this.pmRetrievePopUpVal("BOOK");
                    break;
                case "TXTCODE":
                case "TXTREFNO":
                    this.pmInitPopUpDialog("DOC");
                    this.pofrmGetDoc.ValidateField(this.txtQcBook.Tag.ToString(), "", (inTextbox == "TXTCODE" ? "FCQCORDERH" : "FCREFNO"), true);
                    if (this.pofrmGetDoc.PopUpResult)
                        this.pmRetrievePopUpVal(inTextbox);
                    break;
            }
        }

        private void txtQcBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";
            if (txtPopup.Text == "")
            {
                this.txtQcBook.Tag = "";
                this.txtQcBook.Text = "";
                this.txtQnBook.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BOOK");
                e.Cancel = !this.pofrmGetBook.ValidateField(this.mstrBranchID, this.txtQcRefType.Tag.ToString(), txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetBook.PopUpResult)
                {
                    this.pmRetrievePopUpVal("BOOK");
                }
            }
        }

        private void txtCode_Validating(object sender, CancelEventArgs e)
        {
            if (this.mstrLastCode == this.txtCode.Text)
                return;

            DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopup.Name.ToUpper() == "TXTCODE" ? "FCQCORDERH" : "FCREFNO");
            if (txtPopup.Text == "")
            {
                this.txtCode.Tag = "";
                this.txtCode.Text = "";
                this.txtRefNo.Text = "";
                this.mstrLastCode = this.txtCode.Text;
            }
            else
            {
                this.pmInitPopUpDialog("DOC");
                e.Cancel = !this.pofrmGetDoc.ValidateField(this.txtQcBook.Tag.ToString(), txtPopup.Text, strOrderBy, false);
                if (this.pofrmGetDoc.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopup.Name);
                }
            }
        }

    }
}
