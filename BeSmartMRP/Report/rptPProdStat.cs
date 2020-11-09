
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
using BeSmartMRP.Business.Agents;
using AppUtil;


namespace BeSmartMRP.Report
{
    public partial class rptPProdStat : UIHelper.frmBase
    {

        public rptPProdStat(string inRefType,string inForm)
        {
            InitializeComponent();

            this.mstrPForm = inForm.ToUpper();
            this.mstrRefType = inRefType;
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;
        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetDocMO pofrmGetDoc = null;

        private string mstrRefTable = MapTable.Table.Product;
        private string mstrBook_WHouse_FG = "";

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = "";
        private int mintYear = 0;
        private string mstrSect = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcMfgBook.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);
            this.txtQnMfgBook.Properties.MaxLength = MapTable.GetMaxLength(pobjSQLUtil2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Name);

            this.txtQcBranch.Tag = "";
            this.txtQcPlant.Tag = "";
            this.txtQcMfgBook.Tag = "";

            this.txtQcMfgBook.Text = "";
            this.txtQnMfgBook.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

            }

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cCorp = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant["cRowID"].ToString();
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }

            this.pmDefaultBook();

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;

            this.cmbOption1.Properties.Items.Clear();
            this.cmbOption1.Properties.Items.AddRange(new object[] { 
                                                                                          UIBase.GetAppUIText(new string[] { "1 = ทุก Step", "1 = Print All Step" })
                                                                                        , UIBase.GetAppUIText(new string[] { "2 = เฉพาะที่ Approve แล้วเท่านั้น", "2 = Print Approve only" }) });

            this.cmbOption2.Properties.Items.Clear();
            this.cmbOption2.Properties.Items.AddRange(new object[] { 
                                                                                          UIBase.GetAppUIText(new string[] { "พิมพ์แบบสรุป", "Print Summary" })
                                                                                        , UIBase.GetAppUIText(new string[] { "พิมพ์แบบแสดงรายละเอียด", "Print Detail" }) });


            this.cmbOption1.SelectedIndex = 0;
            this.cmbOption2.SelectedIndex = 0;

            this.cmbOption2.Visible = false;
            //this.pnlProd.Visible = this.mstrPForm == "FORM3";

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmMapEvent()
        {
            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new CancelEventHandler(txtQcBranch_Validating);

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            this.txtQcMfgBook.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcMfgBook.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcMfgBook.Validating += new CancelEventHandler(txtQcMfgBook_Validating);

            this.txtBegDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            this.txtEndDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

        }

        private void pmSetFormUI()
        {
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "Date between :" });
            this.lblToDate.Text = UIBase.GetAppUIText(new string[] { "ถึง", "To :" });
            this.lblPOption1.Text = UIBase.GetAppUIText(new string[] { "พิมพ์ SO Step ? :", "Print SO Step ? :" });
            this.lblRngProd.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงสินค้า/วัตถุดิบ", "Select Range RM/Product" });
            this.lblFrProd.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToProd.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngCode.Text = UIBase.GetAppUIText(new string[] { "เลือกเอกสารรหัสตั้งแต่", "Select #MO Code From :" });
            this.lblRngCode2.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.btnPrint.Text = UIBase.GetAppUIText(new string[] { "ตกลง", "OK" });
            this.btnCancel.Text = UIBase.GetAppUIText(new string[] { "ยกเลิก", "Cancel" });
        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), this.mstrRefType });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBook", "MFGBOOK", "select cRowID, cCode, cName from MfgBook where cCorp = ? and cBranch = ? and cPlant = ? and cRefType  = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcMfgBook.Tag = dtrBook["cRowID"].ToString();
                this.txtQcMfgBook.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnMfgBook.Text = dtrBook["cName"].ToString().TrimEnd();

                this.txtBegDoc.Text = "";
                this.txtEndDoc.Text = "";
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
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                    this.pofrmGetMfgBook.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), "", strOrderBy, true);
                    if (this.pofrmGetMfgBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    this.pmInitPopUpDialog("PROD");
                    string strPrefix = "FCCODE";
                    this.pofrmGetProd.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGDOC":
                case "TXTENDDOC":
                    this.pmInitPopUpDialog("DOC_MO");
                    strOrderBy = "CCODE";
                    this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), DocumentType.MO.ToString(), this.txtQcMfgBook.Tag.ToString(), "", strOrderBy, true);
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

                            this.txtQcMfgBook.Tag = "";
                            this.txtQcMfgBook.Text = "";
                            this.txtQnMfgBook.Text = "";

                        }
                    }
                    break;
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
                        }
                    }
                    break;
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    DataRow dtrProd = this.pofrmGetProd.RetrieveValue();
                    if (dtrProd != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCPROD")
                        {
                            this.txtBegQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                            this.txtBegQnProd.Text = dtrProd["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                            this.txtEndQnProd.Text = dtrProd["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCPROD")
                        {
                            this.txtBegQcProd.Text = "";
                            this.txtBegQnProd.Text = "";
                        }
                        else
                        {
                            this.txtEndQcProd.Text = "";
                            this.txtEndQnProd.Text = "";
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
                e.Cancel = !this.pofrmGetMfgBook.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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

        private void txtQcProd_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCPROD")
                {
                    this.txtBegQnProd.Text = "";
                }
                else
                {
                    this.txtEndQnProd.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PROD");
                e.Cancel = !this.pofrmGetProd.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetProd.PopUpResult)
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
                e.Cancel = !this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), DocumentType.MO.ToString(), this.txtQcMfgBook.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //KeepLogAgent.KeepLog(pobjSQLUtil, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            this.pmPrintData();
        }

        private void pmPrintData()
        {

            string strErrorMsg = "";

            Report.LocalDataSet.DTSPRODSTAT dtsPrintPreview = new Report.LocalDataSet.DTSPRODSTAT();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { this.txtQcMfgBook.Tag.ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMfgBook", "BOOK", "select * from MFGBOOK where cRowID = ? ", ref strErrorMsg))
            {
                DataRow dtrMfgBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                this.mstrBook_WHouse_FG = dtrMfgBook[QMfgBookInfo.Field.WHouse_FG].ToString().TrimEnd();
            }

            string strSQLStr = "";
            strSQLStr = "select * from Prod where Prod.fcCorp = ? and Prod.fcCode between ? and ? ";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {

                decimal decSOQty = 0;
                decimal decMOQty = 0;
                decimal decStkQty = 0;
                decimal decMinStock = 0;
                decimal decMaxStock = 0;
                foreach (DataRow dtrProd in this.dtsDataEnv.Tables["QProd"].Rows)
                {
                    //Sum SO Qty**
                    //รายละเอียดสินค้า**
                    //คงคลัง     **
                    //Maximum Stock       
                    //Minimustock    
                    //กำลังผลิต    
                    //จะต้องสั่งผลิตเพิ่ม
                    decSOQty = this.pmSumSOQty(dtrProd["fcSkid"].ToString());
                    decMOQty = this.pmSumMOQty(dtrProd["fcSkid"].ToString());
                    decStkQty = this.pmSumStockBal(dtrProd["fcSkid"].ToString(), this.txtEndDate.DateTime.Date);
                    decMinStock = 0;
                    decMaxStock = 0;
                    this.pmGetMinMaxStock(this.txtQcBranch.Tag.ToString(), dtrProd["fcSkid"].ToString(), ref decMinStock, ref decMaxStock);

                    DataRow dtrPreview = dtsPrintPreview.PPRODSTAT.NewRow();
                    dtrPreview["QcProd"] = dtrProd["fcCode"].ToString();
                    dtrPreview["QnProd"] = dtrProd["fcName"].ToString();
                    dtrPreview["QTY_SO"] = decSOQty;
                    dtrPreview["QTY_STOCK"] = decStkQty;
                    dtrPreview["QTY_MAXSTOCK"] = decMaxStock;
                    dtrPreview["QTY_MINSTOCK"] = decMinStock;
                    dtrPreview["QTY_MO"] = decMOQty;
                    dtsPrintPreview.PPRODSTAT.Rows.Add(dtrPreview);
                }

                if (dtsPrintPreview.PPRODSTAT.Rows.Count > 0)
                {
                    this.pmPreviewReport(dtsPrintPreview);
                }
                else
                {
                    MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private decimal pmSumSOQty(string inProd)
        {

            string strErrorMsg = "";
            decimal decSOQty = 0;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = " select sum(ORDERI.FNBACKQTY) as fnBackQty from ORDERI  ";
            strSQLStr += " where ORDERI.FCPROD = ? and ORDERI.FCSTEP not in ( 'P', 'L') and ORDERI.FNBACKQTY > 0 and ORDERI.FCSTAT <> 'C' ";
            strSQLStr += (this.cmbOption1.SelectedIndex == 1 ? " and ORDERI.FCAPPROVEB <> '' " : "");

            pobjSQLUtil.SetPara(new object[] { inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumSO", "ORDERH", strSQLStr, ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QSumSO"].Rows[0]["fnBackQty"]))
                {
                    decSOQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QSumSO"].Rows[0]["fnBackQty"]);
                }
            }
            return decSOQty;
        }

        private decimal pmSumMOQty(string inProd)
        {

            string strErrorMsg = "";
            decimal decMOQty = 0;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";

            string strRngCodeStr = "";
            if (this.txtBegDoc.Text.Trim() != string.Empty && this.txtEndDoc.Text.Trim() != string.Empty)
            {
                strRngCodeStr = " and MFWORDERHD.CCODE between '{0}' and '{1}' ";
                strRngCodeStr = string.Format(strRngCodeStr, new string[] { this.txtBegDoc.Text, this.txtEndDoc.Text });
            }

            strSQLStr = " select sum(MFWORDERHD.NQTY) as nQty from MFWORDERHD ";
            strSQLStr += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? and MFWORDERHD.DDATE between ? and ? " + strRngCodeStr;
            strSQLStr += " and MFWORDERHD.CMFGPROD = ? and MFWORDERHD.CSTAT <> 'C' and MFWORDERHD.CMSTEP <> 'P' ";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), DocumentType.MO.ToString(), this.txtQcMfgBook.Tag.ToString(), this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumSO", "ORDERH", strSQLStr, ref strErrorMsg))
            {
                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QSumSO"].Rows[0]["nQty"]))
                {
                    decMOQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QSumSO"].Rows[0]["nQty"]);
                }
            }
            return decMOQty;
        }

        private decimal pmSumStockBal(string inProd, DateTime inDate)
        {

            decimal decBalStk = 0;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = " select REFPROD.FCPROD , REFPROD.FCIOTYPE , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNQTY ";
            strSQLStr += " from REFPROD ";
            strSQLStr += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr += " where REFPROD.FCPROD = ? and REFPROD.FDDATE <= ? ";
            strSQLStr += " and WHOUSE.FCCORP = ? and WHOUSE.FCTYPE = ' ' ";
            strSQLStr += (this.mstrBook_WHouse_FG.Trim() != string.Empty ? " and WHOUSE.FCCODE in (" + this.mstrBook_WHouse_FG + ") " : "");
            strSQLStr += " and GLREF.FCSTAT <> 'C' ";
            strSQLStr += " group by REFPROD.FCPROD,REFPROD.FCIOTYPE ";
            pobjSQLUtil.SetPara(new object[] { inProd, inDate, App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBal", "REFPROD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBal in this.dtsDataEnv.Tables["QBal"].Rows)
                {
                    decimal decSign = (dtrBal["fcIOType"].ToString().Trim() == "I" ? 1 : -1);
                    decBalStk = decBalStk + (Convert.ToDecimal(dtrBal["fnQty"]) * decSign);
                }
            }
            return decBalStk;
        }

        private void pmGetMinMaxStock(string inBranch, string inProd, ref decimal ioMinStock, ref decimal ioMaxStock)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStr1 = "";
            strSQLStr1 = "select top 1 PDBR.FNMINSTOCK from PDBR  ";
            strSQLStr1 += " left join WHOUSE on WHOUSE.FCSKID = PDBR.FCWHOUSE ";
            strSQLStr1 += " where PDBR.FCPROD = ? and PDBR.FCBRANCH = ? ";
            strSQLStr1 += " and WHOUSE.FCCORP = ? ";
            strSQLStr1 += (this.mstrBook_WHouse_FG.Trim() != string.Empty ? " and WHOUSE.FCCODE in (" + this.mstrBook_WHouse_FG + ") " : "");
            strSQLStr1 += " order by PDBR.FNMINSTOCK ";

            pobjSQLUtil.SetPara(new object[] { inProd, inBranch, App.ActiveCorp.RowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdbr", "PDBR", strSQLStr1, ref strErrorMsg))
            {
                ioMinStock = Convert.ToDecimal(this.dtsDataEnv.Tables["QPdbr"].Rows[0]["fnMinStock"]);
            }

            strSQLStr1 = "select top 1 PDBR.FNMAXSTOCK from PDBR  ";
            strSQLStr1 += " left join WHOUSE on WHOUSE.FCSKID = PDBR.FCWHOUSE ";
            strSQLStr1 += " where PDBR.FCPROD = ? and PDBR.FCBRANCH = ? ";
            strSQLStr1 += " and WHOUSE.FCCORP = ? ";
            strSQLStr1 += (this.mstrBook_WHouse_FG.Trim() != string.Empty ? " and WHOUSE.FCCODE in (" + this.mstrBook_WHouse_FG + ") " : "");
            strSQLStr1 += " order by PDBR.FNMAXSTOCK desc";
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdbr", "PDBR", strSQLStr1, ref strErrorMsg))
            {
                ioMaxStock = Convert.ToDecimal(this.dtsDataEnv.Tables["QPdbr"].Rows[0]["fnMaxStock"]);
            }
            
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\XRPPRODSTAT.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //string strPDate = UIBase.GetAppReportText(new string[] { "ตั้งแต่วันที่ ", "Date between " })
            //                                    + UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtBegDate.DateTime, "dd/MM/yy"), this.txtBegDate.DateTime.ToString("dd/MM/yy") })
            //                                    + UIBase.GetAppReportText(new string[] { " ถึง ", " to " })
            //                                    + UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtEndDate.DateTime, "dd/MM/yy"), this.txtEndDate.DateTime.ToString("dd/MM/yy") });

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPDate);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //int i = 0;
            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);

            //for (int j = 0; j < prmCRPara.Count; j++)
            //{
            //    if (prmCRPara[j].Name.ToUpper() == "PFPOPTION")
            //    {
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.pmGetPOption());
            //        prmCRPara[j].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //    }
            //}

            //App.PreviewReport(this, false, rptPreviewReport);

        }

        private string pmGetPOption()
        {
            string strResult = "";
            strResult += this.txtQcBranch.Text.TrimEnd() + " : " + this.txtQnBranch.Text.TrimEnd() + "|";
            strResult += this.txtQcPlant.Text.TrimEnd() + " : " + this.txtQnPlant.Text.TrimEnd() + "|";
            strResult += this.txtQcMfgBook.Text.TrimEnd() + " : " + this.txtQnMfgBook.Text.TrimEnd() + "|";
            strResult += this.txtBegDate.DateTime.ToString("dd/MM/yy") + " " + this.lblToDate.Text + " " + this.txtEndDate.DateTime.ToString("dd/MM/yy") + "|";
            strResult += this.cmbOption1.Text.TrimEnd() + "|";
            strResult += this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtBegQnProd.Text.TrimEnd()
                                + " " + this.lblToDate.Text + " " + this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtEndQnProd.Text.TrimEnd() + "|";

            return strResult;
        }

    }
}
