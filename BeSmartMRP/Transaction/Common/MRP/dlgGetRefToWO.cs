
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

namespace BeSmartMRP.Transaction.Common.MRP
{

    public partial class dlgGetRefToWO : UIHelper.frmBase
    {

        public dlgGetRefToWO(string inRefTab, string inRefType, string inBranchID, string inPlantID, string inBookID, string inRefID)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.pmInitForm(inRefTab, inRefType, inBranchID, inPlantID, inBookID, inRefID);
        }

        public dlgGetRefToWO(string inRefTab, string inRefType, string inBranchID, string inPlantID, string inCoor, string inBookID, string inRefID)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.mstrCoorID = inCoor;
            this.pmInitForm(inRefTab, inRefType, inBranchID, inPlantID, inBookID, inRefID);
        }

        public void SetTitle(string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
        }

        public string RefToBookID
        {
            get { return this.txtQcMfgBook.Tag.ToString(); }
        }

        public string RefToPlantID
        {
            get { return this.txtQcPlant.Tag.ToString(); }
        }

        public string RefToDocumentID
        {
            get { return this.txtCode.Tag.ToString(); }
        }

        public string RefToDocumentCode
        {
            get { return this.mstrRefType + this.txtQcMfgBook.Text.TrimEnd() + "/" + this.txtCode.Text.ToString().TrimEnd(); }
        }

        private string mstrRefStepField = "COPSTEP";
        public string RefStepField
        {
            get { return this.mstrRefStepField; }
            set { this.mstrRefStepField = value; }
        }

        public bool IsGetPlant
        {
            get { return this.pnlPlant.Visible; }
            set { this.pnlPlant.Visible = value; }
        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private Common.MRP.dlgGetDoc_MO pofrmGetDoc1 = null;

        private string mstrRefToTab = "";
        private string mstrRefType = "";
        private string mstrBranchID = "";
        private string mstrPlantID = "";
        private string mstrCoorID = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm(string inRefTab, string inRefType, string inBranchID, string inPlantID, string inBookID, string inRefID)
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.txtQcMfgBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Code);
            //this.txtQnMfgBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGBookInfo.TableName, QBGBookInfo.Field.Name);

            this.txtQcMfgBook.Tag = "";
            this.txtQcMfgBook.Text = "";
            this.txtQnMfgBook.Text = "";
            this.txtCode.Text = "";

            this.mstrRefToTab = inRefTab;
            this.mstrRefType = inRefType;
            this.mstrBranchID = inBranchID;
            this.mstrPlantID = inPlantID;
            this.txtQcMfgBook.Tag = inBookID;
            this.txtCode.Tag = inRefID;
            this.txtQcPlant.Tag = inPlantID;

            this.pnlPlant.Visible = false;
            //this.pnlPlant.Visible = (inPlantID == string.Empty);

            if (inPlantID == string.Empty || inBookID == string.Empty)
            {
                this.pmDefaultOption();
            }
            else
            {
                this.pmLoadOption();
            }

            //this.pmDefaultBook();

            UIBase.SetDefaultChildAppreance(this);
            this.pmSetFormUI();
        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cCorp = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant["cRowID"].ToString();
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }

            this.pmDefaultBook();

        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "MFGBOOK", "select cRowID, cCode, cName from MfgBook where cCorp = ? and cBranch = ? and cPlant = ? and cRefType  = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcMfgBook.Tag = dtrBook["cRowID"].ToString();
                this.txtQcMfgBook.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnMfgBook.Text = dtrBook["cName"].ToString().TrimEnd();

            }
        }

        private void pmLoadOption()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { this.txtQcPlant.Tag.ToString() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cRowID = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }
            
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "MFGBook", "select cRowID, cCode, cName from " + QMfgBookInfo.TableName + " where cCorp = ? and cBranch = ? and cPlant = ? and cRefType  = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcMfgBook.Tag = dtrBook["cRowID"].ToString();
                this.txtQcMfgBook.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnMfgBook.Text = dtrBook["cName"].ToString().TrimEnd();
            }

            objSQLHelper.SetPara(new object[] { this.txtCode.Tag.ToString() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefTo", this.mstrRefToTab, "select cRowID, cCode, cRefNo from " + this.mstrRefToTab + " where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                this.txtCode.Text = dtrRefTo["cCode"].ToString().TrimEnd();
                this.txtRefNo.Text = dtrRefTo["cRefNo"].ToString().TrimEnd();
            }
        
        }

        private void pmSetFormUI()
        {

            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblCode.Text = UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร :", "Doc. Code :" });
            this.lblRefNo.Text = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง :", "Ref No. :" });

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "PLANT":
                    if (this.pofrmGetPlant == null)
                    {
                        this.pofrmGetPlant = new frmEMPlant(FormActiveMode.PopUp);
                        this.pofrmGetPlant.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPlant.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "MFGBOOK":
                    if (this.pofrmGetMfgBook == null)
                    {
                        this.pofrmGetMfgBook = new frmMfgBook(FormActiveMode.PopUp, this.mstrRefType);
                        this.pofrmGetMfgBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetMfgBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "RECDOC":
                    if (this.pofrmGetDoc1 == null)
                    {
                        this.pofrmGetDoc1 = new dlgGetDoc_MO(this.mstrRefStepField);
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
                case "TXTQCMFGBOOK":
                case "TXTQNMFGBOOK":
                    this.pmInitPopUpDialog("MFGBOOK");
                    strOrderBy = (inTextbox == "TXTQCMFGBOOK" ? "CCODE" : "CNAME");
                    this.pofrmGetMfgBook.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), "", strOrderBy, true);
                    if (this.pofrmGetMfgBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTCODE":
                case "TXTREFNO":
                    string strSortBy = (inTextbox.ToUpper() == "TXTCODE" ? "CCODE" : "CREFNO");
                    this.pmInitPopUpDialog("RECDOC");
                    if (this.mstrCoorID.Trim() != string.Empty)
                    {
                        this.pofrmGetDoc1.ValidateField2(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrCoorID, this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), "", strSortBy, true);
                    }
                    else
                    {
                        this.pofrmGetDoc1.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), "", strSortBy, true);
                    }
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
                case "TXTQCPLANT":
                case "TXTQNPLANT":

                    DataRow dtrGetVal = this.pofrmGetPlant.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        if (this.txtQcPlant.Tag.ToString() != dtrGetVal["cRowID"].ToString())
                        {
                            this.txtQcPlant.Tag = dtrGetVal["cRowID"].ToString();
                            this.pmDefaultBook();
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

                        this.txtQcMfgBook.Tag = "";
                        this.txtQcMfgBook.Text = "";
                        this.txtQnMfgBook.Text = "";

                        this.txtRefNo.Text = "";

                    }
                    break;
                case "TXTQCMFGBOOK":
                case "TXTQNMFGBOOK":
                    if (this.pofrmGetMfgBook != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetMfgBook.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcMfgBook.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcMfgBook.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnMfgBook.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcMfgBook.Tag = "";
                            this.txtQcMfgBook.Text = "";
                            this.txtQnMfgBook.Text = "";

                            this.txtRefNo.Text = "";
                        }
                    }
                    break;
                case "TXTCODE":
                case "TXTREFNO":
                    if (this.pofrmGetDoc1 != null)
                    {
                        DataRow dtrDoc1 = this.pofrmGetDoc1.RetrieveValue();

                        if (dtrDoc1 != null)
                        {
                            this.txtCode.Tag = dtrDoc1["cRowID"].ToString();
                            this.txtCode.Text = dtrDoc1["cCode"].ToString().TrimEnd();
                            this.txtRefNo.Text = dtrDoc1["cRefNo"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtCode.Tag = "";
                            this.txtCode.Text = "";
                            this.txtRefNo.Text = "";
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
                this.txtQcMfgBook.Tag = "";
                this.txtQcMfgBook.Text = "";
                this.txtQnMfgBook.Text = "";
                this.txtRefNo.Text = "";
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

        private void txtQcMfgBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCMFGBOOK" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcMfgBook.Tag = "";
                this.txtQcMfgBook.Text = "";
                this.txtQnMfgBook.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("MFGBOOK");
                e.Cancel = !this.pofrmGetMfgBook.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetMfgBook.PopUpResult)
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
                this.pmInitPopUpDialog("RECDOC");
                //e.Cancel = !this.pofrmGetDoc1.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), this.txtCode.Text.TrimEnd(), strOrderBy, false);
                if (this.mstrCoorID.Trim() != string.Empty)
                {
                    e.Cancel = !this.pofrmGetDoc1.ValidateField2(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrCoorID, this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), this.txtCode.Text.TrimEnd(), strOrderBy, false);
                }
                else
                {
                    e.Cancel = !this.pofrmGetDoc1.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), this.txtCode.Text.TrimEnd(), strOrderBy, false);
                }

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

        private void txtRefNo_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CREFNO";

            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("RECDOC");
                //e.Cancel = !this.pofrmGetDoc1.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), this.txtRefNo.Text.TrimEnd(), strOrderBy, false);
                if (this.mstrCoorID.Trim() != string.Empty)
                {
                    e.Cancel = !this.pofrmGetDoc1.ValidateField2(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrCoorID, this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), txtPopUp.Text.TrimEnd(), strOrderBy, false);
                }
                else
                {
                    e.Cancel = !this.pofrmGetDoc1.ValidateField(this.mstrBranchID, this.txtQcPlant.Tag.ToString(), this.mstrRefType, this.txtQcMfgBook.Tag.ToString(), txtPopUp.Text.TrimEnd(), strOrderBy, false);
                }
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
