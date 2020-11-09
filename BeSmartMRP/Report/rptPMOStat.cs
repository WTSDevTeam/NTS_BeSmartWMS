
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

    public partial class rptPMOStat : UIHelper.frmBase
    {

        public rptPMOStat(string inRefType,string inForm)
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

            string strPath = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\";
            int intLen1 = (strPath).Length;
            this.cmbPForm.Properties.Items.Clear();
            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            string[] strADir = System.IO.Directory.GetFiles(strPath);
            for (int i = 0; i < strADir.Length; i++)
            {
                string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
                this.cmbPForm.Properties.Items.Add(strFormName);
            }
            if (this.cmbPForm.Properties.Items.Count > 0)
            {
                this.cmbPForm.SelectedIndex = 0;
            }
        
        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetDocMO pofrmGetDoc = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = "";
        private int mintYear = 0;
        private string mstrSect = "";
        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcMfgBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);
            this.txtQnMfgBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Name);

            this.txtQcBranch.Tag = "";
            this.txtQcPlant.Tag = "";
            this.txtQcMfgBook.Tag = "";

            this.txtQcMfgBook.Text = "";
            this.txtQnMfgBook.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

            }

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cCorp = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant["cRowID"].ToString();
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }

            this.pmDefaultBook();

            DataRow dtrValue = null;
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select top 1 fcSkid, fcCode, fcName from SECT where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtBegQcSect.Text = dtrValue["fcCode"].ToString().TrimEnd();
                //this.txtBegQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select top 1 fcSkid, fcCode, fcName from SECT where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtEndQcSect.Text = dtrValue["fcCode"].ToString().TrimEnd();
                //this.txtEndQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
                this.txtBegQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
                //this.txtBegQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
                this.txtEndQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
                //this.txtEndQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            }

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;

            this.cmbOption1.Properties.Items.Clear();
            this.cmbOption1.Properties.Items.AddRange(new object[] { 
                                                                                          UIBase.GetAppUIText(new string[] { "1 = ทุก Step", "1 = Print All Step" })
                                                                                        , UIBase.GetAppUIText(new string[] { "2 = เฉพาะที่ยังไม่ Approve", "2 = Print Un-Approve" })
                                                                                        , UIBase.GetAppUIText(new string[] { "3 = เฉพาะที่ยังไม่ปิดการผลิต", "3 = Print Not MC Step" }) });

            this.cmbOption2.Properties.Items.Clear();
            this.cmbOption2.Properties.Items.AddRange(new object[] { 
                                                                                          UIBase.GetAppUIText(new string[] { "พิมพ์แบบสรุป", "Print Summary" })
                                                                                        , UIBase.GetAppUIText(new string[] { "พิมพ์แบบแสดงรายละเอียด", "Print Detail" }) });


            this.cmbOption1.SelectedIndex = 0;
            this.cmbOption2.SelectedIndex = 0;

            this.pnlOption1.Visible = (this.mstrPForm == "FORM1" || this.mstrPForm == "FORM3");
            this.pnlProd.Visible = this.mstrPForm == "FORM3";
            this.cmbOption2.Visible = this.mstrPForm != "FORM3";

            switch (this.mstrPForm)
            {
                case "FORM1":
                case "FORM2":
                    this.cmbPForm.Visible = false;
                    break;
                case "FORM3":
                    this.cmbPForm.Visible = true;
                    break;
            }

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

            this.txtBegQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);
            this.txtEndQcSect.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcSect.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcSect.Validating += new CancelEventHandler(txtQcSect_Validating);

            this.txtBegQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);
            this.txtEndQcJob.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcJob.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcJob.Validating += new CancelEventHandler(txtQcJob_Validating);

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
            this.lblPOption1.Text = UIBase.GetAppUIText(new string[] { "พิมพ์ MO Step ? :", "Print MO Step ? :" });
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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "MFGBOOK", "select cRowID, cCode, cName from MfgBook where cCorp = ? and cBranch = ? and cPlant = ? and cRefType  = ? order by CCODE", ref strErrorMsg))
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
                case "SECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new DialogForms.dlgGetSect();
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "JOB":
                    if (this.pofrmGetJob == null)
                    {
                        this.pofrmGetJob = new DialogForms.dlgGetJob();
                        //this.pofrmGetJob.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetJob.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "DOC_MO":
                    if (this.pofrmGetDoc == null)
                    {
                        this.pofrmGetDoc = new DialogForms.dlgGetDocMO();
                        this.pofrmGetDoc.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                        this.pofrmGetDoc.FIlterStep = " and MFWORDERHD.CSTAT <> 'C' ";
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
            string strPrefix = "";
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
                case "TXTBEGQCSECT":
                case "TXTENDQCSECT":
                    this.pmInitPopUpDialog("SECT");
                    strPrefix = "FCCODE";
                    this.pofrmGetSect.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetSect.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCJOB":
                case "TXTENDQCJOB":
                    this.pmInitPopUpDialog("JOB");
                    strPrefix = "FCCODE";
                    this.pofrmGetJob.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetJob.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    this.pmInitPopUpDialog("PROD");
                    strPrefix = "FCCODE";
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

                            this.txtBegDoc.Text = "";
                            this.txtEndDoc.Text = "";

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
                case "TXTBEGQCSECT":
                case "TXTENDQCSECT":
                    DataRow dtrSect = this.pofrmGetSect.RetrieveValue();
                    if (dtrSect != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCSECT")
                        {
                            this.txtBegQcSect.Text = dtrSect["fcCode"].ToString().TrimEnd();
                            //this.txtBegQnSect.Text = dtrSect["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcSect.Text = dtrSect["fcCode"].ToString().TrimEnd();
                            //this.txtEndQnSect.Text = dtrSect["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCSECT")
                        {
                            this.txtBegQcSect.Text = "";
                            //this.txtBegQnSect.Text = "";
                        }
                        else
                        {
                            this.txtEndQcSect.Text = "";
                            //this.txtEndQnSect.Text = "";
                        }
                    }
                    break;

                case "TXTBEGQCJOB":
                case "TXTENDQCJOB":
                    DataRow dtrJob = this.pofrmGetJob.RetrieveValue();
                    if (dtrJob != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCJOB")
                        {
                            this.txtBegQcJob.Text = dtrJob["fcCode"].ToString().TrimEnd();
                            //this.txtBegQnJob.Text = dtrJob["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcJob.Text = dtrJob["fcCode"].ToString().TrimEnd();
                            //this.txtEndQnJob.Text = dtrJob["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCJOB")
                        {
                            this.txtBegQcJob.Text = "";
                            //this.txtBegQnJob.Text = "";
                        }
                        else
                        {
                            this.txtEndQcJob.Text = "";
                            //this.txtEndQnJob.Text = "";
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

        private void txtQcSect_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCSECT")
                {
                    //this.txtBegQnSect.Text = "";
                }
                else
                {
                    //this.txtEndQnSect.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("SECT");
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

        private void txtQcJob_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCJOB")
                {
                    //this.txtBegQnJob.Text = "";
                }
                else
                {
                    //this.txtEndQnJob.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("JOB");
                e.Cancel = !this.pofrmGetJob.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetJob.PopUpResult)
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
            //WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            this.pmPrintData();
        }

        private string mstrHTable = QMFWOrderHDInfo.TableName;
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        private void pmPrintData()
        {

            string strErrorMsg = "";

            string strFilStep = "";
            switch (this.cmbOption1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strFilStep = " and CISAPPROVE <> 'A' ";
                    break;
                case 2:
                    strFilStep = " and CMSTEP <> 'P' ";
                    break;
            }

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strUMTab = strFMDBName + ".dbo.UM";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";
            string strRefProdTab = strFMDBName + ".dbo.REFPROD";
            string strRefStmoveHTab = strDBName + ".dbo.REFDOC_STMOVE";
            string strRefDocTab = strDBName + ".dbo.REFDOC";

            string strSQLStrOrderI = "select MFWORDERIT_PD.NQTY from MFWORDERIT_PD ";
            strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
            strSQLStrOrderI += " left join " + strUMTab + " UM on UM.FCSKID = MFWORDERIT_PD.CUOM";
            strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
            if (this.pnlProd.Visible)
            {
                strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            }

            string strRngCodeStr = "";
            if (this.txtBegDoc.Text.Trim() != string.Empty && this.txtEndDoc.Text.Trim() != string.Empty)
            {
                strRngCodeStr = " and CCODE between '{0}' and '{1}' ";
                strRngCodeStr = string.Format(strRngCodeStr, new string[] { this.txtBegDoc.Text, this.txtEndDoc.Text });
            }

            strSQLStrOrderI += " order by MFWORDERIT_PD.COPSEQ";

            Report.LocalDataSet.DTSPMOSTAT dtsPrintPreview = new Report.LocalDataSet.DTSPMOSTAT();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select MFWORDERHD.* from MFWORDERHD ";
            strSQLStr += " left join " + strSectTab + " SECT on SECT.FCSKID = MFWORDERHD.CSECT ";
            strSQLStr += " left join " + strJobTab + " JOB on JOB.FCSKID = MFWORDERHD.CJOB ";
            strSQLStr += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? and MFWORDERHD.DDATE between ? and ? " + strFilStep + strRngCodeStr;
            strSQLStr += " and SECT.FCCODE between ? and ? ";
            strSQLStr += " and JOB.FCCODE between ? and ? ";
            strSQLStr += " order by MFWORDERHD.CCODE";

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID
                                                                , this.txtQcBranch.Tag.ToString()
                                                                , this.txtQcPlant.Tag.ToString()
                                                                , this.mstrRefType
                                                                , this.txtQcMfgBook.Tag.ToString()
                                                                , this.txtBegDate.DateTime.Date
                                                                , this.txtEndDate.DateTime.Date
                                                                , this.txtBegQcSect.Text.TrimEnd()
                                                                , this.txtEndQcSect.Text.TrimEnd()
                                                                , this.txtBegQcJob.Text.TrimEnd()
                                                                , this.txtEndQcJob.Text.TrimEnd()});

            pobjSQLUtil2.NotUpperSQLExecString = true;
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                pobjSQLUtil2.NotUpperSQLExecString = false;
                foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                {

                    string strQcBook = "";
                    string strQnBook = "";
                    pobjSQLUtil2.SetPara(new object[1] { dtrOrderH["cMfgBook"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBook", "MFGBOOK", "select * from MfgBook where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    pobjSQLUtil.NotUpperSQLExecString = false;

                    string strQcSect = "";
                    string strQnSect = "";
                    string strQcDept = "";
                    string strQnDept = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cSect"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select * from DEPT where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }

                    string strQcMfgProd = "";
                    string strQnMfgProd = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cMfgProd"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal decMfgQty = Convert.ToDecimal(dtrOrderH["nMfgQty"]);

                    string strStartDate = "";
                    string strDueDate = "";
                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]).Date;
                        //strStartDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strStartDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]).Date;
                        //strDueDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strDueDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    string strQcBOM = "";
                    string strQnBOM = "";
                    string strBOM = dtrOrderH[QMFWOrderHDInfo.Field.BOMID].ToString();
                    pobjSQLUtil2.SetPara(new object[1] { strBOM });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode , cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcJob = "";
                    string strQnJob = "";
                    string strQcProj = "";
                    string strQnProj = "";

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cJob"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProj", "PROJ", "select * from PROJ where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcName"].ToString().TrimEnd();
                        }
                    }


                    string strQcCoor = ""; string strQnCoor = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cCoor"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        strQcCoor = dtrCoor["fcCode"].ToString().TrimEnd();
                        strQnCoor = dtrCoor["fcSName"].ToString().TrimEnd();
                    }

                    decimal decRecieveQty = 0;
                    pobjSQLUtil2.SetPara(new object[] { DocumentType.FR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? ", ref strErrorMsg))
                    {
                        if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                        {
                            decRecieveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                        }
                    }

                    string strQcWHouse = "";
                    string strQnWHouse = "";

                    //CSTEP = case WORDERH.CMSTEP when '1' then '' when 'P' then 'MC' when 'L' then 'CLOSED' end
                    string strStep = "";
                    switch (dtrOrderH["CMSTEP"].ToString())
                    {
                        case "P":
                            strStep = "MC";
                            break;
                        case "L":
                            strStep = "CLOSE";
                            break;
                        default:
                            break;
                    }

                    string strRemark = (Convert.IsDBNull(dtrOrderH["cMemData"]) ? "" : dtrOrderH["cMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData2"]) ? "" : dtrOrderH["cMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData3"]) ? "" : dtrOrderH["cMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData4"]) ? "" : dtrOrderH["cMemData4"].ToString().TrimEnd());
                    if (dtrOrderH["cMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrOrderH["cMemData5"]) ? "" : dtrOrderH["cMemData5"].ToString().TrimEnd());

                    string strRemark1 = dtrOrderH["cReMark1"].ToString();
                    string strRemark2 = dtrOrderH["cReMark2"].ToString();
                    string strRemark3 = dtrOrderH["cReMark3"].ToString();
                    string strRemark4 = dtrOrderH["cReMark4"].ToString();
                    string strRemark5 = dtrOrderH["cReMark5"].ToString();
                    string strRemark6 = dtrOrderH["cReMark6"].ToString();
                    string strRemark7 = dtrOrderH["cReMark7"].ToString();
                    string strRemark8 = dtrOrderH["cReMark8"].ToString();
                    string strRemark9 = dtrOrderH["cReMark9"].ToString();
                    string strRemark10 = dtrOrderH["cReMark10"].ToString();

                    string strRefQcBook = "";
                    string strRefQnBook = "";
                    string strRefCode = "";
                    string strRefRefNo = "";
                    string strRefDate = "";
                    this.pmLoadRefToCode(this.mstrRefType, dtrOrderH["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);

                    string strSQLStrOrderI2 = "select REFPROD.* ";
                    strSQLStrOrderI2 += " , PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD, PROD.FCSNAME as QSPROD ";
                    strSQLStrOrderI2 += " from REFPROD ";
                    strSQLStrOrderI2 += " left join PROD on PROD.FCSKID = REFPROD.FCPROD";
                    strSQLStrOrderI2 += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE";
                    strSQLStrOrderI2 += " where REFPROD.FCGLREF in (";
                    strSQLStrOrderI2 += " select FCSKID from GLREF where FCSKID in (select REFDOC.CMASTERH from ";
                    strSQLStrOrderI2 += strRefDocTab + " REFDOC where REFDOC.CCHILDTYPE = 'MO' and REFDOC.CCHILDH = ? ) ) ";
                    strSQLStrOrderI2 += " and REFPROD.FCREFTYPE in ('WR','WX','RW','RX') ";
                    strSQLStrOrderI2 += " and WHOUSE.FCTYPE = ' ' ";
                    //strSQLStrOrderI2 += " and REFPROD.FCIOTYPE = 'O' ";

                    if (this.pnlProd.Visible)
                    {
                        strSQLStrOrderI2 += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
                    }

                    decimal decMOQty = 0;

                    //strSQLStrOrderI = "select sum(MFWORDERIT_PD.NQTY) as NQTY ";
                    strSQLStrOrderI = "select MFWORDERIT_PD.NQTY ";
                    strSQLStrOrderI += " , MFWORDERIT_PD.CPROD ";
                    strSQLStrOrderI += " , MFWORDERIT_PD.COPSEQ";
                    strSQLStrOrderI += " , MFWORDERIT_PD.CSEQ";
                    //strSQLStrOrderI += " , UM.FCCODE as QcUM,UM.FCNAME as QnUM ";
                    strSQLStrOrderI += " from MFWORDERIT_PD ";
                    strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
                    //strSQLStrOrderI += " left join " + strUMTab + " UM on UM.FCSKID = MFWORDERIT_PD.CUOM";
                    strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
                    if (this.pnlProd.Visible)
                    {
                        strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
                    }

                    //strSQLStrOrderI += " group by MFWORDERIT_PD.CPROD";
                    strSQLStrOrderI += " order by MFWORDERIT_PD.COPSEQ";

                    int intRecNo = 0;
                    pobjSQLUtil2.NotUpperSQLExecString = true;
                    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                    {
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            #region "MO Item"
                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            //string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                            }

                            //strQcUM = dtrOrderI["QcUM"].ToString().TrimEnd();
                            //strQnUM = dtrOrderI["QnProd"].ToString().TrimEnd();

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;
                            dtrPrnData["CQCBOM"] = strQcBOM;
                            dtrPrnData["CQNBOM"] = strQnBOM;

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            //dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            dtrPrnData["NUMQTY"] = 1;
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            dtrPrnData["CSTEP"] = strStep;
                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            dtrPrnData["NMOQTY"] = decMfgQty;
                            //XXX
                            dtrPrnData["CSEQ"] = dtrOrderI["CSEQ"].ToString();

                            intRecNo++;

                            decimal decStMoveQty = 0;
                            pobjSQLUtil2.NotUpperSQLExecString = true;

                            string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                            strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                            strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                            strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                            strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            pobjSQLUtil2.SetPara(new object[] { DocumentType.WR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "O", dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }

                            //pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "I", dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    decStMoveQty = decStMoveQty - Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }

                            pobjSQLUtil2.NotUpperSQLExecString = false;

                            dtrPrnData["NFGQTY"] = decRecieveQty;
                            dtrPrnData["NISSUEQTY"] = decStMoveQty;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            //dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            //dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            //dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                            #endregion

                        }
                    }


                    //พิมพ์รายการสินค้าเพิ่มเติมจาก RefProd
                    pobjSQLUtil.NotUpperSQLExecString = true;
                    pobjSQLUtil.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI2, ref strErrorMsg))
                    {

                        pobjSQLUtil.NotUpperSQLExecString = false;

                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            #region "MO Item"
                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = "";
                            //string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());


                            strQcProd = dtrOrderI["QcProd"].ToString().TrimEnd();
                            strQnProd = dtrOrderI["QnProd"].ToString().TrimEnd();
                            strQsProd = dtrOrderI["QsProd"].ToString().TrimEnd();

                            DataRow[] da = dtsPrintPreview.PMOSTAT.Select("CQCPROD = '" + strQcProd + "'");
                            if (da.Length > 0)
                            {
                                continue;
                            }

                            pobjSQLUtil2.SetPara(new object[2] { dtrOrderI["fcRefType"].ToString().TrimEnd(), dtrOrderI["fcSkid"].ToString().TrimEnd() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select * from REFDOC_STMOVE where REFDOC_STMOVE.CMASTERTYP = ? and REFDOC_STMOVE.CMASTERI = ?", ref strErrorMsg))
                            {
                                pobjSQLUtil2.SetPara(new object[1] { this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["cChildI"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", "MFWORDERIT_PD", "select * from MFWORDERIT_PD where CROWID = ?", ref strErrorMsg))
                                {
                                    decMOQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["nQty"]);
                                }
                            }


                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["fcUM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;
                            dtrPrnData["CQCBOM"] = strQcBOM;
                            dtrPrnData["CQNBOM"] = strQnBOM;

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;

                            //dtrPrnData["CLOT"] = dtrOrderI["fcLot"].ToString();

                            dtrPrnData["NQTY"] = decMOQty;
                            dtrPrnData["NUMQTY"] = 1;
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            dtrPrnData["CSTEP"] = strStep;
                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            dtrPrnData["NMOQTY"] = decMfgQty;

                            decimal decStMoveQty = 0;

                            pobjSQLUtil2.NotUpperSQLExecString = false;

                            decimal decUMQty = (Convert.ToDecimal(dtrOrderI["fnUMQty"]) == 0 ? 1 : Convert.ToDecimal(dtrOrderI["fnUMQty"]));
                            decStMoveQty = Convert.ToDecimal(dtrOrderI["fnQty"]) * decUMQty;

                            dtrPrnData["NFGQTY"] = decRecieveQty;
                            dtrPrnData["NISSUEQTY"] = decStMoveQty;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            
                            //dtrPrnData["CQCPROD"] = strQcProd
                            //strCurrSeq = dtrOrderI["CSEQ"].ToString();

                            intRecNo++;

                            string strOPSeq = this.pmGetOPSeq(dtsPrintPreview, dtrPrnData["CQCPROD"].ToString());
                            if (strOPSeq.Trim() != string.Empty)
                            {
                                dtrPrnData["CSEQ"] = strOPSeq;
                                dtrPrnData["COPSEQ"] = strOPSeq;

                            }
                            else
                            {
                                dtrPrnData["CSEQ"] = StringHelper.ConvertToBase64(intRecNo, 2);
                                dtrPrnData["COPSEQ"] = StringHelper.ConvertToBase64(intRecNo, 2);
                            }
                            
                            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);

                            //dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            
                            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                            #endregion

                        }
                    }

                    #region "Un Used"

                    //else
                    //{

                    //    #region "กรณีลูกค้าไม่ได้ทำ ใบเบิกเลย"
                    //    //กรณีลูกค้าไม่ได้ทำ ใบเบิกเลย
                    //    strSQLStrOrderI = "select MFWORDERIT_PD.* from MFWORDERIT_PD ";
                    //    strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
                    //    strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
                    //    if (this.pnlProd.Visible)
                    //    {
                    //        strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
                    //    }

                    //    strSQLStrOrderI += " order by MFWORDERIT_PD.CSEQ";

                    //    pobjSQLUtil2.NotUpperSQLExecString = true;
                    //    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    //    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                    //    {
                    //        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                    //        {

                    //            #region "MO Item"
                    //            string strQcProd = "";
                    //            string strQnProd = "";
                    //            string strQsProd = "";
                    //            string strQcUM = "";
                    //            string strQnUM = "";
                    //            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                    //            pobjSQLUtil.NotUpperSQLExecString = false;
                    //            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                    //            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                    //            {
                    //                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                    //                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    //                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                    //            }

                    //            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUOM"].ToString().TrimEnd() });
                    //            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                    //            {
                    //                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                    //                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                    //            }

                    //            string strRefQcBook2 = "";
                    //            string strRefQnBook2 = "";
                    //            string strRefCode2 = "";
                    //            string strRefRefNo2 = "";
                    //            string strRefDate2 = "";
                    //            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                    //            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                    //            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                    //            dtrPrnData["CQCBOOK"] = strQcBook;
                    //            dtrPrnData["CQNBOOK"] = strQnBook;
                    //            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                    //            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                    //            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                    //            dtrPrnData["CQCCOOR"] = strQcCoor;
                    //            dtrPrnData["CQNCOOR"] = strQnCoor;
                    //            dtrPrnData["CQCBOM"] = strQcBOM;
                    //            dtrPrnData["CQNBOM"] = strQnBOM;

                    //            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                    //            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                    //            dtrPrnData["CREFDOC_REFTYPE"] = "";
                    //            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                    //            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                    //            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                    //            dtrPrnData["CSTARTDATE"] = strStartDate;
                    //            dtrPrnData["CFINISHDATE"] = strDueDate;

                    //            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                    //            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                    //            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                    //            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                    //            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                    //            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                    //            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                    //            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                    //            dtrPrnData["CQCSECT"] = strQcSect;
                    //            dtrPrnData["CQNSECT"] = strQnSect;
                    //            dtrPrnData["CQCDEPT"] = strQcDept;
                    //            dtrPrnData["CQNDEPT"] = strQnDept;
                    //            dtrPrnData["CQCJOB"] = strQcJob;
                    //            dtrPrnData["CQNJOB"] = strQnJob;
                    //            dtrPrnData["CQCPROJ"] = strQcProj;
                    //            dtrPrnData["CQNPROJ"] = strQnProj;
                    //            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                    //            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                    //            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CQCPROD"] = strQcProd;
                    //            dtrPrnData["CQNPROD"] = strQnProd;
                    //            dtrPrnData["CQSPROD"] = strQsProd;
                    //            dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                    //            dtrPrnData["CREMARK1_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK2_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK3_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK4_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK5_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK6_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK7_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK8_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK9_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["CREMARK10_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                    //            dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                    //            dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                    //            dtrPrnData["CQCUM"] = strQcUM;
                    //            dtrPrnData["CQNUM"] = strQnUM;

                    //            dtrPrnData["CSTEP"] = strStep;
                    //            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                    //            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                    //            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                    //            dtrPrnData["NMOQTY"] = decMfgQty;

                    //            decimal decStMoveQty = 0;
                    //            pobjSQLUtil2.NotUpperSQLExecString = true;

                    //            string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                    //            strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                    //            strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                    //            strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                    //            strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                    //            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                    //            pobjSQLUtil2.SetPara(new object[] { DocumentType.WR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "O", dtrOrderI["cProd"].ToString() });
                    //            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                    //            {
                    //                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                    //                {
                    //                    decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                    //                }
                    //            }

                    //            //pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                    //            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                    //            pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "I", dtrOrderI["cProd"].ToString() });
                    //            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                    //            {
                    //                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                    //                {
                    //                    decStMoveQty = decStMoveQty - Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                    //                }
                    //            }

                    //            pobjSQLUtil2.NotUpperSQLExecString = false;

                    //            dtrPrnData["NFGQTY"] = decRecieveQty;
                    //            dtrPrnData["NISSUEQTY"] = decStMoveQty;

                    //            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                    //            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                    //            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                    //            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                    //            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                    //            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                    //            #endregion

                    //        }
                    //    }

                    //    #endregion

                    //}
                    
                    #endregion

                }

                if (dtsPrintPreview.PMOSTAT.Rows.Count > 0)
                {
                    this.pmPreviewReport(dtsPrintPreview);
                }
                else
                {
                    MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private string pmGetOPSeq(Report.LocalDataSet.DTSPMOSTAT inSource, string inQcProd)
        {
            string strOPSeq = "";
            foreach (DataRow dtrSearch in inSource.PMOSTAT.Rows)
            {
                if (dtrSearch["CQCPROD"].ToString() == inQcProd)
                {
                    strOPSeq = dtrSearch["CSEQ"].ToString();
                }
            }
            return strOPSeq;
        }

        private void XXX2_pmPrintData()
        {

            string strErrorMsg = "";

            string strFilStep = "";
            switch (this.cmbOption1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strFilStep = " and CISAPPROVE <> 'A' ";
                    break;
                case 2:
                    strFilStep = " and CMSTEP <> 'P' ";
                    break;
            }

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";
            string strRefProdTab = strFMDBName + ".dbo.REFPROD";
            string strRefStmoveHTab = strDBName + ".dbo.REFDOC_STMOVE";
            string strRefDocTab = strDBName + ".dbo.REFDOC";

            string strSQLStrOrderI = "select MFWORDERIT_PD.* from MFWORDERIT_PD ";
            strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
            strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
            if (this.pnlProd.Visible)
            {
                strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            }

            string strRngCodeStr = "";
            if (this.txtBegDoc.Text.Trim() != string.Empty && this.txtEndDoc.Text.Trim() != string.Empty)
            {
                strRngCodeStr = " and CCODE between '{0}' and '{1}' ";
                strRngCodeStr = string.Format(strRngCodeStr, new string[] { this.txtBegDoc.Text, this.txtEndDoc.Text });
            }

            strSQLStrOrderI += " order by MFWORDERIT_PD.CSEQ";

            Report.LocalDataSet.DTSPMOSTAT dtsPrintPreview = new Report.LocalDataSet.DTSPMOSTAT();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select MFWORDERHD.* from MFWORDERHD ";
            strSQLStr += " left join " + strSectTab + " SECT on SECT.FCSKID = MFWORDERHD.CSECT ";
            strSQLStr += " left join " + strJobTab + " JOB on JOB.FCSKID = MFWORDERHD.CJOB ";
            strSQLStr += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? and MFWORDERHD.DDATE between ? and ? " + strFilStep + strRngCodeStr;
            strSQLStr += " and SECT.FCCODE between ? and ? ";
            strSQLStr += " and JOB.FCCODE between ? and ? ";
            strSQLStr += " order by MFWORDERHD.CCODE";

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID
                                                                , this.txtQcBranch.Tag.ToString()
                                                                , this.txtQcPlant.Tag.ToString()
                                                                , this.mstrRefType
                                                                , this.txtQcMfgBook.Tag.ToString()
                                                                , this.txtBegDate.DateTime.Date
                                                                , this.txtEndDate.DateTime.Date
                                                                , this.txtBegQcSect.Text.TrimEnd()
                                                                , this.txtEndQcSect.Text.TrimEnd()
                                                                , this.txtBegQcJob.Text.TrimEnd()
                                                                , this.txtEndQcJob.Text.TrimEnd()});

            pobjSQLUtil2.NotUpperSQLExecString = true;
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                pobjSQLUtil2.NotUpperSQLExecString = false;
                foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                {

                    string strQcBook = "";
                    string strQnBook = "";
                    pobjSQLUtil2.SetPara(new object[1] { dtrOrderH["cMfgBook"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBook", "MFGBOOK", "select * from MfgBook where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcSect = "";
                    string strQnSect = "";
                    string strQcDept = "";
                    string strQnDept = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cSect"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select * from DEPT where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }

                    string strQcMfgProd = "";
                    string strQnMfgProd = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cMfgProd"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal decMfgQty = Convert.ToDecimal(dtrOrderH["nMfgQty"]);

                    string strStartDate = "";
                    string strDueDate = "";
                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]).Date;
                        //strStartDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strStartDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]).Date;
                        //strDueDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strDueDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    string strQcBOM = "";
                    string strQnBOM = "";
                    string strBOM = dtrOrderH[QMFWOrderHDInfo.Field.BOMID].ToString();
                    pobjSQLUtil2.SetPara(new object[1] { strBOM });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode , cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcJob = "";
                    string strQnJob = "";
                    string strQcProj = "";
                    string strQnProj = "";

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cJob"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProj", "PROJ", "select * from PROJ where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcName"].ToString().TrimEnd();
                        }
                    }


                    string strQcCoor = ""; string strQnCoor = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cCoor"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        strQcCoor = dtrCoor["fcCode"].ToString().TrimEnd();
                        strQnCoor = dtrCoor["fcSName"].ToString().TrimEnd();
                    }

                    decimal decRecieveQty = 0;
                    pobjSQLUtil2.SetPara(new object[] { DocumentType.FR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? ", ref strErrorMsg))
                    {
                        if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                        {
                            decRecieveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                        }
                    }

                    string strQcWHouse = "";
                    string strQnWHouse = "";

                    //CSTEP = case WORDERH.CMSTEP when '1' then '' when 'P' then 'MC' when 'L' then 'CLOSED' end
                    string strStep = "";
                    switch (dtrOrderH["CMSTEP"].ToString())
                    {
                        case "P":
                            strStep = "MC";
                            break;
                        case "L":
                            strStep = "CLOSE";
                            break;
                        default:
                            break;
                    }

                    string strRemark = (Convert.IsDBNull(dtrOrderH["cMemData"]) ? "" : dtrOrderH["cMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData2"]) ? "" : dtrOrderH["cMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData3"]) ? "" : dtrOrderH["cMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData4"]) ? "" : dtrOrderH["cMemData4"].ToString().TrimEnd());
                    if (dtrOrderH["cMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrOrderH["cMemData5"]) ? "" : dtrOrderH["cMemData5"].ToString().TrimEnd());

                    string strRemark1 = dtrOrderH["cReMark1"].ToString();
                    string strRemark2 = dtrOrderH["cReMark2"].ToString();
                    string strRemark3 = dtrOrderH["cReMark3"].ToString();
                    string strRemark4 = dtrOrderH["cReMark4"].ToString();
                    string strRemark5 = dtrOrderH["cReMark5"].ToString();
                    string strRemark6 = dtrOrderH["cReMark6"].ToString();
                    string strRemark7 = dtrOrderH["cReMark7"].ToString();
                    string strRemark8 = dtrOrderH["cReMark8"].ToString();
                    string strRemark9 = dtrOrderH["cReMark9"].ToString();
                    string strRemark10 = dtrOrderH["cReMark10"].ToString();

                    string strRefQcBook = "";
                    string strRefQnBook = "";
                    string strRefCode = "";
                    string strRefRefNo = "";
                    string strRefDate = "";
                    this.pmLoadRefToCode(this.mstrRefType, dtrOrderH["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);

                    string strSQLStrOrderI2 = "select REFPROD.* ";
                    strSQLStrOrderI2 += " , PROD.FCCODE as QCPROD, PROD.FCNAME as QNPROD, PROD.FCSNAME as QSPROD ";
                    strSQLStrOrderI2 += " from REFPROD ";
                    strSQLStrOrderI2 += " left join PROD on PROD.FCSKID = REFPROD.FCPROD";
                    strSQLStrOrderI2 += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE";
                    strSQLStrOrderI2 += " where REFPROD.FCGLREF in (";
                    strSQLStrOrderI2 += " select FCSKID from GLREF where FCSKID in (select REFDOC.CMASTERH from ";
                    strSQLStrOrderI2 += strRefDocTab + " REFDOC where REFDOC.CCHILDTYPE = 'MO' and REFDOC.CCHILDH = ? ) ) ";
                    strSQLStrOrderI2 += " and REFPROD.FCREFTYPE in ('WR','WX','RW','RX') ";
                    strSQLStrOrderI2 += " and WHOUSE.FCTYPE = ' ' ";
                    //strSQLStrOrderI2 += " and REFPROD.FCIOTYPE = 'O' ";

                    if (this.pnlProd.Visible)
                    {
                        strSQLStrOrderI2 += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
                    }

                    decimal decMOQty = 0;

                    pobjSQLUtil.NotUpperSQLExecString = true;
                    pobjSQLUtil.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI2, ref strErrorMsg))
                    {
                        
                        pobjSQLUtil.NotUpperSQLExecString = false;

                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            #region "MO Item"
                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = "";
                            //string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            strQcProd = dtrOrderI["QcProd"].ToString().TrimEnd();
                            strQnProd = dtrOrderI["QnProd"].ToString().TrimEnd();
                            strQsProd = dtrOrderI["QsProd"].ToString().TrimEnd();

                            //pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            //{
                            //    strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                            //    strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                            //    strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                            //}

                            //select * from REFDOC_STMOVE where REFDOC_STMOVE.CCHILDTYPE = 'MO' and REFDOC_STMOVE.CCHILDH = 'g1Mx0003'
                            pobjSQLUtil2.SetPara(new object[2] { dtrOrderI["fcRefType"].ToString().TrimEnd(), dtrOrderI["fcSkid"].ToString().TrimEnd() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select * from REFDOC_STMOVE where REFDOC_STMOVE.CMASTERTYP = ? and REFDOC_STMOVE.CMASTERI = ?", ref strErrorMsg))
                            {
                                pobjSQLUtil2.SetPara(new object[1] { this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["cChildI"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWOrderI", "MFWORDERIT_PD", "select * from MFWORDERIT_PD where CROWID = ?", ref strErrorMsg))
                                {
                                    decMOQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QWOrderI"].Rows[0]["nQty"]);
                                }
                            }


                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["fcUM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;
                            dtrPrnData["CQCBOM"] = strQcBOM;
                            dtrPrnData["CQNBOM"] = strQnBOM;

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            dtrPrnData["CLOT"] = dtrOrderI["fcLot"].ToString();
                            //dtrPrnData["CREMARK1_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK2_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK3_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK4_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK5_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK6_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK7_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK8_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK9_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            //dtrPrnData["CREMARK10_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());

                            dtrPrnData["NQTY"] = decMOQty;
                            dtrPrnData["NUMQTY"] = 1;
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            dtrPrnData["CSTEP"] = strStep;
                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            dtrPrnData["NMOQTY"] = decMfgQty;

                            decimal decStMoveQty = 0;
                            //pobjSQLUtil2.NotUpperSQLExecString = true;

                            //string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                            //strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                            //strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                            //strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                            //strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                            ////if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            //pobjSQLUtil2.SetPara(new object[] { DocumentType.WR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "O", dtrOrderI["cProd"].ToString() });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            //{
                            //    if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                            //    {
                            //        decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                            //    }
                            //}

                            ////pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                            ////if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            //pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "I", dtrOrderI["cProd"].ToString() });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            //{
                            //    if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                            //    {
                            //        decStMoveQty = decStMoveQty - Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                            //    }
                            //}

                            pobjSQLUtil2.NotUpperSQLExecString = false;

                            decimal decUMQty = (Convert.ToDecimal(dtrOrderI["fnUMQty"]) == 0 ? 1 : Convert.ToDecimal(dtrOrderI["fnUMQty"]));
                            decStMoveQty = Convert.ToDecimal(dtrOrderI["fnQty"]) * decUMQty;

                            dtrPrnData["NFGQTY"] = decRecieveQty;
                            dtrPrnData["NISSUEQTY"] = decStMoveQty;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            //dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                            #endregion

                        }
                    }
                    else
                    {

                        #region "กรณีลูกค้าไม่ได้ทำ ใบเบิกเลย"
                        //กรณีลูกค้าไม่ได้ทำ ใบเบิกเลย
                        strSQLStrOrderI = "select MFWORDERIT_PD.* from MFWORDERIT_PD ";
                        strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
                        strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
                        if (this.pnlProd.Visible)
                        {
                            strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
                        }

                        strSQLStrOrderI += " order by MFWORDERIT_PD.CSEQ";

                        pobjSQLUtil2.NotUpperSQLExecString = true;
                        pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                        {
                            foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                            {

                                #region "MO Item"
                                string strQcProd = "";
                                string strQnProd = "";
                                string strQsProd = "";
                                string strQcUM = "";
                                string strQnUM = "";
                                string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                                pobjSQLUtil.NotUpperSQLExecString = false;
                                pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                                {
                                    strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                    strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                    strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                                }

                                pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUOM"].ToString().TrimEnd() });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                                {
                                    strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                    strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                                }

                                string strRefQcBook2 = "";
                                string strRefQnBook2 = "";
                                string strRefCode2 = "";
                                string strRefRefNo2 = "";
                                string strRefDate2 = "";
                                //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                                DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                                dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                                dtrPrnData["CQCBOOK"] = strQcBook;
                                dtrPrnData["CQNBOOK"] = strQnBook;
                                dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                                dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                                dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                                dtrPrnData["CQCCOOR"] = strQcCoor;
                                dtrPrnData["CQNCOOR"] = strQnCoor;
                                dtrPrnData["CQCBOM"] = strQcBOM;
                                dtrPrnData["CQNBOM"] = strQnBOM;

                                dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                                dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                                dtrPrnData["CREFDOC_REFTYPE"] = "";
                                dtrPrnData["CREFDOC_CODE"] = strRefCode;
                                dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                                dtrPrnData["CREFDOC_DATE"] = strRefDate;

                                dtrPrnData["CSTARTDATE"] = strStartDate;
                                dtrPrnData["CFINISHDATE"] = strDueDate;

                                dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                                dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                                dtrPrnData["CREFDOC_REFTYPE2"] = "";
                                dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                                dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                                dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                                dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                                dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                                dtrPrnData["CQCSECT"] = strQcSect;
                                dtrPrnData["CQNSECT"] = strQnSect;
                                dtrPrnData["CQCDEPT"] = strQcDept;
                                dtrPrnData["CQNDEPT"] = strQnDept;
                                dtrPrnData["CQCJOB"] = strQcJob;
                                dtrPrnData["CQNJOB"] = strQnJob;
                                dtrPrnData["CQCPROJ"] = strQcProj;
                                dtrPrnData["CQNPROJ"] = strQnProj;
                                dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                                dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                                dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CQCPROD"] = strQcProd;
                                dtrPrnData["CQNPROD"] = strQnProd;
                                dtrPrnData["CQSPROD"] = strQsProd;
                                dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                                dtrPrnData["CREMARK1_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK2_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK3_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK4_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK5_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK6_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK7_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK8_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK9_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["CREMARK10_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                                dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                                dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                                dtrPrnData["CQCUM"] = strQcUM;
                                dtrPrnData["CQNUM"] = strQnUM;

                                dtrPrnData["CSTEP"] = strStep;
                                dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                                dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                                dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                                dtrPrnData["NMOQTY"] = decMfgQty;

                                decimal decStMoveQty = 0;
                                pobjSQLUtil2.NotUpperSQLExecString = true;

                                string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                                strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                                strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                                strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                                strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                                //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                                pobjSQLUtil2.SetPara(new object[] { DocumentType.WR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "O", dtrOrderI["cProd"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                                {
                                    if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                    {
                                        decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                    }
                                }

                                //pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                                //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                                pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "I", dtrOrderI["cProd"].ToString() });
                                if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                                {
                                    if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                    {
                                        decStMoveQty = decStMoveQty - Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                    }
                                }

                                pobjSQLUtil2.NotUpperSQLExecString = false;

                                dtrPrnData["NFGQTY"] = decRecieveQty;
                                dtrPrnData["NISSUEQTY"] = decStMoveQty;

                                string strQcWKCtrH = ""; string strQnWKCtrH = "";
                                //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                                dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                                dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                                dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                                dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                                #endregion

                            }
                        }

                        #endregion

                    }

                }

                if (dtsPrintPreview.PMOSTAT.Rows.Count > 0)
                {
                    this.pmPreviewReport(dtsPrintPreview);
                }
                else
                {
                    MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void XXXpmPrintData()
        {

            string strErrorMsg = "";

            string strFilStep = "";
            switch (this.cmbOption1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strFilStep = " and CISAPPROVE <> 'A' ";
                    break;
                case 2:
                    strFilStep = " and CMSTEP <> 'P' ";
                    break;
            }

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";
            string strRefProdTab = strFMDBName + ".dbo.REFPROD";
            string strRefStmoveHTab = strDBName + ".dbo.REFDOC_STMOVE";

            string strSQLStrOrderI = "select MFWORDERIT_PD.* from MFWORDERIT_PD ";
            strSQLStrOrderI += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD";
            strSQLStrOrderI += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.CIOTYPE = 'O' ";
            if (this.pnlProd.Visible)
            {
                strSQLStrOrderI += string.Format(" and PROD.FCCODE between '{0}' and '{1}'", new object[] { this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            }

            string strRngCodeStr = "";
            if (this.txtBegDoc.Text.Trim() != string.Empty && this.txtEndDoc.Text.Trim() != string.Empty)
            {
                strRngCodeStr = " and CCODE between '{0}' and '{1}' ";
                strRngCodeStr = string.Format(strRngCodeStr, new string[] { this.txtBegDoc.Text, this.txtEndDoc.Text });
            }

            strSQLStrOrderI += " order by MFWORDERIT_PD.CSEQ";

            Report.LocalDataSet.DTSPMOSTAT dtsPrintPreview = new Report.LocalDataSet.DTSPMOSTAT();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select MFWORDERHD.* from MFWORDERHD ";
            strSQLStr += " left join " + strSectTab + " SECT on SECT.FCSKID = MFWORDERHD.CSECT ";
            strSQLStr += " left join " + strJobTab + " JOB on JOB.FCSKID = MFWORDERHD.CJOB ";
            strSQLStr += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? and MFWORDERHD.DDATE between ? and ? " + strFilStep + strRngCodeStr;
            strSQLStr += " and SECT.FCCODE between ? and ? ";
            strSQLStr += " and JOB.FCCODE between ? and ? ";
            strSQLStr += " order by MFWORDERHD.CCODE";

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID
                                                                , this.txtQcBranch.Tag.ToString()
                                                                , this.txtQcPlant.Tag.ToString()
                                                                , this.mstrRefType
                                                                , this.txtQcMfgBook.Tag.ToString()
                                                                , this.txtBegDate.DateTime.Date
                                                                , this.txtEndDate.DateTime.Date
                                                                , this.txtBegQcSect.Text.TrimEnd()
                                                                , this.txtEndQcSect.Text.TrimEnd()
                                                                , this.txtBegQcJob.Text.TrimEnd()
                                                                , this.txtEndQcJob.Text.TrimEnd()});

            pobjSQLUtil2.NotUpperSQLExecString = true;
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderH", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                pobjSQLUtil2.NotUpperSQLExecString = false;
                foreach (DataRow dtrOrderH in this.dtsDataEnv.Tables["QOrderH"].Rows)
                {

                    string strQcBook = "";
                    string strQnBook = "";
                    pobjSQLUtil2.SetPara(new object[1] { dtrOrderH["cMfgBook"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QMfgBook", "MFGBOOK", "select * from MfgBook where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBook = this.dtsDataEnv.Tables["QMfgBook"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcSect = "";
                    string strQnSect = "";
                    string strQcDept = "";
                    string strQnDept = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cSect"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from Sect where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnSect = this.dtsDataEnv.Tables["QSect"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QSect"].Rows[0]["fcDept"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select * from DEPT where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnDept = this.dtsDataEnv.Tables["QDept"].Rows[0]["fcName"].ToString().TrimEnd();
                        }

                    }

                    string strQcMfgProd = "";
                    string strQnMfgProd = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cMfgProd"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid, fcCode,fcName,fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnMfgProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                    }

                    decimal decMfgQty = Convert.ToDecimal(dtrOrderH["nMfgQty"]);

                    string strStartDate = "";
                    string strDueDate = "";
                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.StartDate]).Date;
                        //strStartDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strStartDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    if (!Convert.IsDBNull(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrOrderH[QMFWOrderHDInfo.Field.DueDate]).Date;
                        //strDueDate = dttDate.ToString("dd/MM/") + AppUtil.StringHelper.Right((dttDate.Year + 543).ToString("0000"), 2);
                        strDueDate = UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(dttDate, "dd/MM/yy"), dttDate.ToString("dd/MM/yy") });
                    }

                    string strQcBOM = "";
                    string strQnBOM = "";
                    string strBOM = dtrOrderH[QMFWOrderHDInfo.Field.BOMID].ToString();
                    pobjSQLUtil2.SetPara(new object[1] { strBOM });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdStH", "PDSTH", "select cRowID, cCode , cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        strQcBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cCode"].ToString().TrimEnd();
                        strQnBOM = this.dtsDataEnv.Tables["QPdStH"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    string strQcJob = "";
                    string strQnJob = "";
                    string strQcProj = "";
                    string strQnProj = "";

                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cJob"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select * from JOB where fcSkid = ?", ref strErrorMsg))
                    {
                        strQcJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcCode"].ToString().TrimEnd();
                        strQnJob = this.dtsDataEnv.Tables["QJob"].Rows[0]["fcName"].ToString().TrimEnd();

                        pobjSQLUtil.SetPara(new object[1] { this.dtsDataEnv.Tables["QJob"].Rows[0]["fcProj"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProj", "PROJ", "select * from PROJ where fcSkid = ?", ref strErrorMsg))
                        {
                            strQcProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcCode"].ToString().TrimEnd();
                            strQnProj = this.dtsDataEnv.Tables["QProj"].Rows[0]["fcName"].ToString().TrimEnd();
                        }
                    }


                    string strQcCoor = ""; string strQnCoor = "";
                    pobjSQLUtil.SetPara(new object[1] { dtrOrderH["cCoor"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName, fcSName, fcDeliCoor from Coor where fcSkid = ?", ref strErrorMsg))
                    {
                        DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                        strQcCoor = dtrCoor["fcCode"].ToString().TrimEnd();
                        strQnCoor = dtrCoor["fcSName"].ToString().TrimEnd();
                    }

                    decimal decRecieveQty = 0;
                    pobjSQLUtil2.SetPara(new object[] { DocumentType.FR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? ", ref strErrorMsg))
                    {
                        if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                        {
                            decRecieveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                        }
                    }

                    string strQcWHouse = "";
                    string strQnWHouse = "";

                    //CSTEP = case WORDERH.CMSTEP when '1' then '' when 'P' then 'MC' when 'L' then 'CLOSED' end
                    string strStep = "";
                    switch (dtrOrderH["CMSTEP"].ToString())
                    { 
                        case "P":
                            strStep = "MC";
                            break;
                        case "L":
                            strStep = "CLOSE";
                            break;
                        default:
                            break;
                    }

                    string strRemark = (Convert.IsDBNull(dtrOrderH["cMemData"]) ? "" : dtrOrderH["cMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData2"]) ? "" : dtrOrderH["cMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData3"]) ? "" : dtrOrderH["cMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrOrderH["cMemData4"]) ? "" : dtrOrderH["cMemData4"].ToString().TrimEnd());
                    if (dtrOrderH["cMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrOrderH["cMemData5"]) ? "" : dtrOrderH["cMemData5"].ToString().TrimEnd());

                    string strRemark1 = dtrOrderH["cReMark1"].ToString();
                    string strRemark2 = dtrOrderH["cReMark2"].ToString();
                    string strRemark3 = dtrOrderH["cReMark3"].ToString();
                    string strRemark4 = dtrOrderH["cReMark4"].ToString();
                    string strRemark5 = dtrOrderH["cReMark5"].ToString();
                    string strRemark6 = dtrOrderH["cReMark6"].ToString();
                    string strRemark7 = dtrOrderH["cReMark7"].ToString();
                    string strRemark8 = dtrOrderH["cReMark8"].ToString();
                    string strRemark9 = dtrOrderH["cReMark9"].ToString();
                    string strRemark10 = dtrOrderH["cReMark10"].ToString();

                    string strRefQcBook = "";
                    string strRefQnBook = "";
                    string strRefCode = "";
                    string strRefRefNo = "";
                    string strRefDate = "";
                    this.pmLoadRefToCode(this.mstrRefType, dtrOrderH["cRowID"].ToString(), ref strRefQcBook, ref strRefQnBook, ref strRefCode, ref strRefRefNo, ref strRefDate);

                    pobjSQLUtil2.NotUpperSQLExecString = true;
                    pobjSQLUtil2.SetPara(new object[] { dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QOrderI", "OrderI", strSQLStrOrderI, ref strErrorMsg))
                    {
                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QOrderI"].Rows)
                        {

                            #region "MO Item"
                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = (Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd());

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                            }

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["cUOM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;
                            dtrPrnData["CQCBOM"] = strQcBOM;
                            dtrPrnData["CQNBOM"] = strQnBOM;

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CREMARK1"] = StringHelper.ChrTran(strRemark1, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2"] = StringHelper.ChrTran(strRemark2, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3"] = StringHelper.ChrTran(strRemark3, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4"] = StringHelper.ChrTran(strRemark4, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5"] = StringHelper.ChrTran(strRemark5, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6"] = StringHelper.ChrTran(strRemark6, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7"] = StringHelper.ChrTran(strRemark7, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8"] = StringHelper.ChrTran(strRemark8, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9"] = StringHelper.ChrTran(strRemark9, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10"] = StringHelper.ChrTran(strRemark10, "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            dtrPrnData["CLOT"] = dtrOrderI["cLot"].ToString();
                            dtrPrnData["CREMARK1_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark1"]) ? "" : dtrOrderI["cReMark1"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK2_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark2"]) ? "" : dtrOrderI["cReMark2"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK3_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark3"]) ? "" : dtrOrderI["cReMark3"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK4_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark4"]) ? "" : dtrOrderI["cReMark4"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK5_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark5"]) ? "" : dtrOrderI["cReMark5"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK6_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark6"]) ? "" : dtrOrderI["cReMark6"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK7_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark7"]) ? "" : dtrOrderI["cReMark7"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK8_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark8"]) ? "" : dtrOrderI["cReMark8"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK9_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark9"]) ? "" : dtrOrderI["cReMark9"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["CREMARK10_ITEM"] = StringHelper.ChrTran((Convert.IsDBNull(dtrOrderI["cReMark10"]) ? "" : dtrOrderI["cReMark10"].ToString().TrimEnd()), "|", Convert.ToChar(13).ToString());
                            dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                            dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            dtrPrnData["CSTEP"] = strStep;
                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            dtrPrnData["NMOQTY"] = decMfgQty;

                            decimal decStMoveQty = 0;
                            pobjSQLUtil2.NotUpperSQLExecString = true;

                            string strSQLSumRef = "select sum(REFPROD.FNQTY) as SumQty from " + strRefProdTab + " REFPROD ";
                            strSQLSumRef += " left join " + strProdTab + " PROD on PROD.FCSKID = REFPROD.FCPROD";
                            strSQLSumRef += " where REFPROD.FCGLREF in (SELECT CMASTERH FROM REFDOC WHERE ";
                            strSQLSumRef += " CMASTERTYP =? and CCHILDTYPE = ? AND CCHILDH = ? )";
                            strSQLSumRef += " and REFPROD.FCIOTYPE = ? and PROD.FCSKID = ? ";

                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            pobjSQLUtil2.SetPara(new object[] { DocumentType.WR.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "O", dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    decStMoveQty = Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }

                            //pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString() });
                            //if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", "select sum(NQTY) as SUMQTY from REFDOC_STMOVE where CMASTERTYP = ? and CCHILDTYPE = ? and CCHILDH = ? and CCHILDI = ? ", ref strErrorMsg))
                            pobjSQLUtil2.SetPara(new object[] { DocumentType.RW.ToString(), DocumentType.MO.ToString(), dtrOrderH["cRowID"].ToString(), "I", dtrOrderI["cProd"].ToString() });
                            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc2", "REFDOC_STMOVE", strSQLSumRef, ref strErrorMsg))
                            {
                                if (!Convert.IsDBNull(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]))
                                {
                                    decStMoveQty = decStMoveQty - Convert.ToDecimal(this.dtsDataEnv.Tables["QRefDoc2"].Rows[0]["SUMQTY"]);
                                }
                            }

                            pobjSQLUtil2.NotUpperSQLExecString = false;

                            dtrPrnData["NFGQTY"] = decRecieveQty;
                            dtrPrnData["NISSUEQTY"] = decStMoveQty;

                            string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            //this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                            #endregion

                        }
                    }

                    //TODO: ทำพิมพ์สินค้าที่ไม่มีใน MO
                    string strSQLSumRef2 = "";
                    strSQLSumRef2 = "select * from REFPROD ";
                    strSQLSumRef2 += " where FCGLREF in (select FCSKID from GLREF where FCSKID in (select REFDOC_STMOVE.CMASTERH from " + strRefStmoveHTab + " REFDOC_STMOVE where REFDOC_STMOVE.CCHILDTYPE = 'MO' and REFDOC_STMOVE.CCHILDH = ?))";
                    //strSQLSumRef2 += " and REFPROD.FCIOTYPE = 'O'";
                    strSQLSumRef2 += " and REFPROD.FCSKID not in (select REFDOC_STMOVE.CMASTERI from " + strRefStmoveHTab + " REFDOC_STMOVE where REFDOC_STMOVE.CCHILDTYPE = 'MO' and REFDOC_STMOVE.CCHILDH = ?)";
                    pobjSQLUtil.NotUpperSQLExecString = true;
                    pobjSQLUtil.SetPara(new object[] { dtrOrderH["cRowID"].ToString(), dtrOrderH["cRowID"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strSQLSumRef2, ref strErrorMsg))
                    {
                        pobjSQLUtil.NotUpperSQLExecString = false;

                        foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefProd"].Rows)
                        {

                            #region "พิมพ์สินค้าที่ไม่มีใน MO"

                            string strQcProd = "";
                            string strQnProd = "";
                            string strQsProd = "";
                            string strQcUM = "";
                            string strQnUM = "";
                            string strPdRemark1 = "";

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["fcProd"].ToString() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcCode, fcName, fcSName from PROD where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString().TrimEnd();
                                strQsProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSName"].ToString().TrimEnd();
                            }

                            pobjSQLUtil.SetPara(new object[1] { dtrOrderI["fcUM"].ToString().TrimEnd() });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcCode, fcName from UM where fcSkid = ?", ref strErrorMsg))
                            {
                                strQcUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcCode"].ToString().TrimEnd();
                                strQnUM = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
                            }

                            string strRefQcBook2 = "";
                            string strRefQnBook2 = "";
                            string strRefCode2 = "";
                            string strRefRefNo2 = "";
                            string strRefDate2 = "";
                            //this.pmLoadRefToCode2(this.mstrRefType, dtrOrderH["cRowID"].ToString(), dtrOrderI["cRowID"].ToString(), ref strRefQcBook2, ref strRefQnBook2, ref strRefCode, ref strRefRefNo2, ref strRefDate2);

                            DataRow dtrPrnData = dtsPrintPreview.PMOSTAT.NewRow();

                            dtrPrnData["CREFTYPE"] = dtrOrderH["cRefType"].ToString();
                            dtrPrnData["CQCBOOK"] = strQcBook;
                            dtrPrnData["CQNBOOK"] = strQnBook;
                            dtrPrnData["CCODE"] = dtrOrderH["cCode"].ToString();
                            dtrPrnData["CREFNO"] = dtrOrderH["cRefNo"].ToString();
                            dtrPrnData["DDATE"] = Convert.ToDateTime(dtrOrderH["dDate"]);

                            dtrPrnData["CQCCOOR"] = strQcCoor;
                            dtrPrnData["CQNCOOR"] = strQnCoor;
                            dtrPrnData["CQCBOM"] = strQcBOM;
                            dtrPrnData["CQNBOM"] = strQnBOM;

                            dtrPrnData["CREFDOC_QCBOOK"] = strRefQcBook;
                            dtrPrnData["CREFDOC_QNBOOK"] = strRefQnBook;
                            dtrPrnData["CREFDOC_REFTYPE"] = "";
                            dtrPrnData["CREFDOC_CODE"] = strRefCode;
                            dtrPrnData["CREFDOC_REFNO"] = strRefRefNo;
                            dtrPrnData["CREFDOC_DATE"] = strRefDate;

                            dtrPrnData["CSTARTDATE"] = strStartDate;
                            dtrPrnData["CFINISHDATE"] = strDueDate;

                            dtrPrnData["CREFDOC_QCBOOK2"] = strRefQcBook2;
                            dtrPrnData["CREFDOC_QNBOOK2"] = strRefQnBook2;
                            dtrPrnData["CREFDOC_REFTYPE2"] = "";
                            dtrPrnData["CREFDOC_CODE2"] = strRefCode2;
                            dtrPrnData["CREFDOC_REFNO2"] = strRefRefNo2;
                            dtrPrnData["CREFDOC_DATE2"] = strRefDate2;

                            dtrPrnData["CQCMFGPROD"] = strQcMfgProd;
                            dtrPrnData["CQNMFGPROD"] = strQnMfgProd;
                            dtrPrnData["CQCSECT"] = strQcSect;
                            dtrPrnData["CQNSECT"] = strQnSect;
                            dtrPrnData["CQCDEPT"] = strQcDept;
                            dtrPrnData["CQNDEPT"] = strQnDept;
                            dtrPrnData["CQCJOB"] = strQcJob;
                            dtrPrnData["CQNJOB"] = strQnJob;
                            dtrPrnData["CQCPROJ"] = strQcProj;
                            dtrPrnData["CQNPROJ"] = strQnProj;
                            dtrPrnData["CQCWHOUSE"] = strQcWHouse;
                            dtrPrnData["CQNWHOUSE"] = strQnWHouse;

                            dtrPrnData["CQCPROD"] = strQcProd;
                            dtrPrnData["CQNPROD"] = strQnProd;
                            dtrPrnData["CQSPROD"] = strQsProd;
                            dtrPrnData["CLOT"] = dtrOrderI["fcLot"].ToString();
                            //dtrPrnData["NQTY"] = Convert.ToDecimal(dtrOrderI["nQty"]);
                            //dtrPrnData["NUMQTY"] = Convert.ToDecimal(dtrOrderI["nUOMQty"]);
                            dtrPrnData["NQTY"] = 0;
                            dtrPrnData["NUMQTY"] = 1;
                            dtrPrnData["CQCUM"] = strQcUM;
                            dtrPrnData["CQNUM"] = strQnUM;

                            dtrPrnData["CSTEP"] = strStep;
                            dtrPrnData["CSCRAP"] = dtrOrderH[QMFWOrderHDInfo.Field.Scrap].ToString();
                            dtrPrnData["NSCRAPQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.ScrapQty]);
                            dtrPrnData["NMFGQTY"] = Convert.ToDecimal(dtrOrderH[QMFWOrderHDInfo.Field.Qty]);
                            dtrPrnData["NMOQTY"] = decMfgQty;

                            dtrPrnData["NFGQTY"] = decRecieveQty;
                            dtrPrnData["NISSUEQTY"] = Convert.ToDecimal(dtrOrderI["fnQty"]);

                            //string strQcWKCtrH = ""; string strQnWKCtrH = "";
                            ////this.pmLoadWkCtrByOP(dtrOrderH["cRowID"].ToString(), dtrOrderI["cOPSeq"].ToString(), ref strQcWKCtrH, ref strQnWKCtrH);
                            //dtrPrnData["COPSEQ"] = dtrOrderI["cOPSeq"].ToString();
                            //dtrPrnData["CQCWKCTR"] = strQcWKCtrH;
                            //dtrPrnData["CQNWKCTR"] = strQnWKCtrH;

                            dtsPrintPreview.PMOSTAT.Rows.Add(dtrPrnData);

                            #endregion

                        }
                    }
                }

                if (dtsPrintPreview.PMOSTAT.Rows.Count > 0)
                {
                    this.pmPreviewReport(dtsPrintPreview);
                }
                else
                {
                    MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void pmLoadRefToCode(string inRefType, string inWOrderH, ref string ioQcBook, ref string ioQnBook, ref string ioCode, ref string ioRefNo, ref string ioDate)
        { 
            string strQcBook="";
            string strQnBook = "";
            string strCode = ""; 
            string strRefNo = ""; 
            string strDate = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLStr = "";
            strSQLStr = "select CCHILDH , CMASTERH from REFDOC where CCHILDTYPE = 'SO' and CMASTERTYP = 'MO' and CMASTERH = ? ";

            string strSQLStr2 = "";
            strSQLStr2 = "select ORDERH.FCREFTYPE, ORDERH.FCCODE, BOOK.FCCODE as QCBOOK from ORDERH left join BOOK on BOOK.FCSKID = ORDERH.FCBOOK where ORDERH.FCSKID = ?";
            
            pobjSQLUtil2.SetPara(new object[] { inWOrderH });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrRefDoc in this.dtsDataEnv.Tables["QRefDoc"].Rows)
                {
                    pobjSQLUtil.SetPara(new object[] { dtrRefDoc["cChildH"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef2SO", "ORDERH", strSQLStr2, ref strErrorMsg))
                    {
                        DataRow dtrSO = this.dtsDataEnv.Tables["QRef2SO"].Rows[0];
                        strCode += "SO" + dtrSO["QcBook"].ToString().Trim() + "/" + dtrSO["fcCode"].ToString().TrimEnd() + "\r\n";
                    }
                }
            }
            ioCode = strCode;
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\XRPMOSTAT.rpt";

            //switch (this.mstrPForm)
            //{
            //    case "FORM1":
            //        if (this.cmbOption2.SelectedIndex == 0)
            //        {
            //            strRPTFileName = Application.StartupPath + @"\RPT\XRPMOSTAT.rpt";
            //        }
            //        else
            //        {
            //            strRPTFileName = Application.StartupPath + @"\RPT\XRPMOSTAT_DET.rpt";
            //        }
            //        break;
            //    case "FORM2":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRPMOLST1.rpt";
            //        break;
            //    case "FORM3":
            //        //strRPTFileName = Application.StartupPath + @"\RPT\XRPMOLST2.rpt";
            //        strRPTFileName = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\" + this.cmbPForm.Text.TrimEnd();
            //        break;
            //}

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
            //                                    +UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtBegDate.DateTime, "dd/MM/yy"), this.txtBegDate.DateTime.ToString("dd/MM/yy") })
            //                                    + UIBase.GetAppReportText(new string[] { " ถึง ", " to " })
            //                                    + UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtEndDate.DateTime, "dd/MM/yy"), this.txtEndDate.DateTime.ToString("dd/MM/yy") });

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text + (this.cmbOption2.SelectedIndex == 0 ? "" : " (Detail)"));
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPDate);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName + (this.cmbOption2.SelectedIndex == 0 ? "" : "_DET"));
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
            strResult += this.txtBegDate.DateTime.ToString("dd/MM/yy") + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtEndDate.DateTime.ToString("dd/MM/yy") + "|";

            strResult += this.cmbOption1.Text.TrimEnd() + "|";
            strResult += this.cmbOption2.Text.TrimEnd() + "|";
            strResult += this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtBegQnProd.Text.TrimEnd()
                                + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtEndQnProd.Text.TrimEnd() + "|";

            return strResult;
        }

    }
}
