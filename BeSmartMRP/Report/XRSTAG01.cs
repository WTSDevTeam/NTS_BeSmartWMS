
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
    public partial class XRSTAG01 : UIHelper.frmBase
    {
        
        public XRSTAG01(string inForm)
        {
            InitializeComponent();

            this.mstrPForm = inForm.ToUpper();
            //this.mstrRefType = inRefType;
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;

            //string strPath = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\";
            //int intLen1 = (strPath).Length;
            //this.cmbPForm.Properties.Items.Clear();
            //string[] strADir = System.IO.Directory.GetFiles(strPath);
            //for (int i = 0; i < strADir.Length; i++)
            //{
            //    string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
            //    this.cmbPForm.Properties.Items.Add(strFormName);
            //}
            //if (this.cmbPForm.Properties.Items.Count > 0)
            //{
            //    this.cmbPForm.SelectedIndex = 0;
            //}
        
        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;
        private DialogForms.dlgGetDocMO pofrmGetDoc = null;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = "MO";
        private string mstrRPTFileName = "";
        private string mstrTemPd = "TemPd";

        private DataSet dtsDataEnv = new DataSet();

        private decimal mdecSumBOMOPAmt = 0;
        private decimal mdecSumBOMAmt = 0;
        private decimal mdecSumBOMAmt_Actual = 0;
        private DateTime mdttCurrDate = DateTime.MinValue;

        private string mstrHTable = QMFWOrderHDInfo.TableName;
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        Report.LocalDataSet.DTSPCOST dtsPrintPreview = new Report.LocalDataSet.DTSPCOST();

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

            this.txtBegDate.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.txtEndDate.DateTime = DateTime.Now;

            DataRow dtrValue = null;
            //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select top 1 fcSkid, fcCode, fcName from SECT where fcCorp = ? order by FCCODE", ref strErrorMsg))
            //{
            //    dtrValue = this.dtsDataEnv.Tables["QSect"].Rows[0];
            //    this.txtBegQcSect.Text = dtrValue["fcCode"].ToString().TrimEnd();
            //    //this.txtBegQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            //}
            //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select top 1 fcSkid, fcCode, fcName from SECT where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            //{
            //    dtrValue = this.dtsDataEnv.Tables["QSect"].Rows[0];
            //    this.txtEndQcSect.Text = dtrValue["fcCode"].ToString().TrimEnd();
            //    //this.txtEndQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            //}

            //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE", ref strErrorMsg))
            //{
            //    dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
            //    this.txtBegQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
            //    //this.txtBegQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            //}
            //objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            //{
            //    dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
            //    this.txtEndQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
            //    //this.txtEndQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            //}

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcSkid, fcCode, fcName from PROD where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.txtBegQcProd.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtBegQnProd.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcSkid, fcCode, fcName from PROD where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.txtEndQcProd.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtEndQnProd.Text = dtrValue["fcName"].ToString().TrimEnd();
            }

            this.cmbWHCost.Properties.Items.Clear();
            this.cmbWHCost.Properties.Items.AddRange(new object[] { 
                UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });

            this.cmbWHCost.SelectedIndex = 0;

            //this.cmbOrderBy.Properties.Items.Clear();
            //this.cmbOrderBy.Properties.Items.AddRange(new object[] { 
            //    UIBase.GetAppUIText(new string[] { "พิมพ์เรียงตามวันที่", "Order by MO Date" })
            //    , UIBase.GetAppUIText(new string[] { "พิมพ์เรียงตามเลขที่เอกสาร" , "Order by MO Code" })});

            this.cmbIsDetail.Properties.Items.Clear();
            this.cmbIsDetail.Properties.Items.AddRange(new object[] { 
                UIBase.GetAppUIText(new string[] { "พิมพ์รายละเอียด", "Order by MO Date" })
                , UIBase.GetAppUIText(new string[] { "ไม่พิมพ์รายละเอียด" , "Order by MO Code" })});

            this.cmbIsDetail.SelectedIndex = 0;

            this.txtAge1.Value = 15;
            this.txtAge2.Value = 30;

            this.pmSetRngState();

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

            this.pmCreateTem();
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

            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);


            this.cmbIsDetail.SelectedIndexChanged += new EventHandler(cmbOrderBy_SelectedIndexChanged);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

        }

        private void cmbOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetRngState();
        }

        private void pmSetRngState()
        {
            //this.pnlDate.Enabled = this.cmbOrderBy.SelectedIndex == 0;
        }

        private void pmSetFormUI()
        {
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "Date between :" });
            this.lblToDate.Text = UIBase.GetAppUIText(new string[] { "ถึง ", "To" });

            //this.lblRngCode.Text = UIBase.GetAppUIText(new string[] { "เลือกเอกสารรหัสตั้งแต่", "Select #MO Code From :" });
            //this.lblRngCode2.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            //this.lblRngSect.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงแผนก", "Select Range Section" });
            //this.lblFrSect.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            //this.lblToSect.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            //this.lblRngJob.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงโครงการ", "Select Range Job" });
            //this.lblFrJob.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            //this.lblToJob.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngProd.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงสินค้า/วัตถุดิบ", "Select Range RM/Product" });
            this.lblFrProd.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToProd.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblWhCost.Text = UIBase.GetAppUIText(new string[] { "ใช้ราคาทุนจาก :", "Use Cost from :" });

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
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "DOC_MO":
                    if (this.pofrmGetDoc == null)
                    {
                        this.pofrmGetDoc = new DialogForms.dlgGetDocMO();
                        this.pofrmGetDoc.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDoc.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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

        private string mstrTemPdSer = "TemPdSer";

        private void pmCreateTem()
        {

            DataTable dtbTemPdSer = new DataTable(this.mstrTemPdSer);
            dtbTemPdSer.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQcPdSer", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cRfType", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cRefType", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cRefNo", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("dRefDate", System.Type.GetType("System.DateTime"));
            dtbTemPdSer.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQnWHouse", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cQnWHLoca", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cPdSer", System.Type.GetType("System.String"));
            dtbTemPdSer.Columns.Add("cLot", System.Type.GetType("System.String"));

            dtbTemPdSer.Columns["cQcProd"].DefaultValue = "";
            dtbTemPdSer.Columns["cQnProd"].DefaultValue = "";
            dtbTemPdSer.Columns["cQcPdSer"].DefaultValue = "";
            dtbTemPdSer.Columns["cRefType"].DefaultValue = "";
            dtbTemPdSer.Columns["cRefNo"].DefaultValue = "";
            dtbTemPdSer.Columns["cQcCoor"].DefaultValue = "";
            dtbTemPdSer.Columns["cQnCoor"].DefaultValue = "";
            dtbTemPdSer.Columns["cQcWHouse"].DefaultValue = "";
            dtbTemPdSer.Columns["cQnWHouse"].DefaultValue = "";
            dtbTemPdSer.Columns["cQcWHLoca"].DefaultValue = "";
            dtbTemPdSer.Columns["cQnWHLoca"].DefaultValue = "";
            dtbTemPdSer.Columns["cPdSer"].DefaultValue = "";
            dtbTemPdSer.Columns["cLot"].DefaultValue = "";

            this.dtsDataEnv.Tables.Add(dtbTemPdSer);

        }

        private void pmPrintData()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strCurrPrefix = "";

            //stop = false;
            //thread = new Thread(new ThreadStart(StartPrint));
            //thread.Start();

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.AppMessage = "Start Extract Data...";

            //this.pmCreateBFStock();

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strAppDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;

            string strProdTab = strFMDBName + ".dbo.PROD";
            string strWHouseTab = strFMDBName + ".dbo.WHOUSE";

            //Select Field
            string strSQLExec = "";

            string strFld = "GLREF.FCREFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FDDATE";
            strFld = strFld + ", REFPROD.FNQTY*REFPROD.FNUMQTY AS FNQTY, REFPROD.FCIOTYPE, REFPROD.FCLOT ";
            //strFld = strFld + ", PROD.FCCODE AS QCPROD, PROD.FCNAME AS QNPROD ";
            strFld = strFld + ", PROD.FCCODE AS QCPROD, PROD.FCSNAME AS QNPROD ";
            strFld = strFld + ", WHOUSE.FCCODE AS QCWHOUSE, WHOUSE.FCNAME AS QNWHOUSE ";
            strFld = strFld + ", BOOK.FCCODE AS QCBOOK ";
            strFld = strFld + ", BOOK.FCCODE AS QCBOOK ";
            strFld = strFld + ", datediff(DAY,REFPROD.FDDATE,?) as DayRun ";
            strSQLExec = "select " + strFld + " from REFPROD ";
            strSQLExec = strSQLExec + " inner join GLREF ON GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLExec = strSQLExec + " inner join BOOK ON BOOK.FCSKID = GLREF.FCBOOK ";
            strSQLExec = strSQLExec + " inner join PROD ON PROD.FCSKID = REFPROD.FCPROD and PROD.FNAGELONG > 0";
            strSQLExec = strSQLExec + " inner join WHOUSE ON WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLExec = strSQLExec + " where GLREF.FCCORP = ? AND GLREF.FCBRANCH = ? AND GLREF.FDDATE <= ? ";
            strSQLExec = strSQLExec + " and GLREF.FCSTAT <> 'C' ";
            strSQLExec = strSQLExec + " and PROD.FCCODE between ? and ? ";
            //strSQLExec = strSQLExec + " and PROD.FCTYPE = ? AND PROD.FCCODE between ? and ? ";
            strSQLExec = strSQLExec + " and WHOUSE.FCTYPE = ' ' ";
            strSQLExec = strSQLExec + " and REFPROD.FCIOTYPE = 'I' ";
            //strSQLExec = strSQLExec + " and WHOUSE.FCTYPE = ' ' " + (this.cmbWHouse.SelectedIndex == 0 ? "and WHOUSE.FCCODE between '" + this.txtBegQcWHouse.Text.TrimEnd() + "' and '" + this.txtEndQcWHouse.Text.TrimEnd() + "' " : "");
            strSQLExec = strSQLExec + " order by WHOUSE.FCCODE , PROD.FCCODE, GLREF.FDDATE, REFPROD.FCIOTYPE ";

            LocalDataSet.DTSPSTK01 dtsPrintPreview = new Report.LocalDataSet.DTSPSTK01();

            int intRun = 0;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.Timeout_Exec = 3000;
            pobjSQLUtil.SetPara(new object[] { this.txtBegDate.DateTime.Date, App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", strSQLExec, ref strErrorMsg))
            {
                strCurrPrefix = "";
                decimal decBFQty = 0; decimal decBFAmt = 0;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    intRun++;
                    DataRow dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                    if (strCurrPrefix != dtrGLRef["QcWHouse"].ToString() + dtrGLRef["QcProd"].ToString())
                    {
                        App.AppMessage = "กำลังพิมพ์ข้อมูล สินค้า : " + "(" + dtrGLRef["QcProd"].ToString().TrimEnd() + ")" + dtrGLRef["QnProd"].ToString().TrimEnd();
                        //strCurrPrefix = dtrGLRef["QcWHouse"].ToString() + dtrGLRef["QcProd"].ToString();
                        //this.pmGetBFStock(dtrGLRef["QcWHouse"].ToString(), dtrGLRef["QcProd"].ToString(), ref decBFQty, ref decBFAmt);
                    }

                    dtrPreview["CREFNO"] = dtrGLRef["fcRefType"].ToString() + dtrGLRef["QcBook"].ToString() + "/" + dtrGLRef["fcCode"].ToString();
                    ////04/12/08 By Yod
                    //if (this.cmbWhCode.SelectedIndex == 0)
                    //{
                    //    dtrPreview["CREFNO"] = dtrGLRef["cRefNo"].ToString();
                    //}
                    //else
                    //{
                    //    dtrPreview["CREFNO"] = dtrGLRef["cRefType"].ToString() + dtrGLRef["QcBook"].ToString() + "/" + dtrGLRef["cCode"].ToString();
                    //}

                    //dtrPreview["CREFNO"] = dtrGLRef["cRefNo"].ToString();
                    dtrPreview["DDATE"] = Convert.ToDateTime(dtrGLRef["fdDate"]);
                    //dtrPreview["CQCPDTYPE"] = "";
                    //dtrPreview["CQNPDTYPE"] = "";
                    dtrPreview["CQCPROD"] = dtrGLRef["QcProd"].ToString();
                    dtrPreview["CQNPROD"] = dtrGLRef["QnProd"].ToString();
                    dtrPreview["CQCWHOUSE"] = dtrGLRef["QcWHouse"].ToString();
                    dtrPreview["CQNWHOUSE"] = dtrGLRef["QnWHouse"].ToString();
                    dtrPreview["CLOT"] = dtrGLRef["fcLot"].ToString();
                    dtrPreview["CIOTYPE"] = dtrGLRef["fcIOType"].ToString();
                    dtrPreview["CQNUM"] = "";
                    dtrPreview["NBFQTY"] = decBFQty;
                    dtrPreview["NBFAMT"] = 0;
                    dtrPreview["NAMT_IN"] = 0;
                    dtrPreview["NAMT_OUT"] = 0;

                    dtrPreview["NQTY_IN"] = 0;
                    dtrPreview["NQTY_OUT"] = 0;

                    decBFQty = 0;

                    //dtrPreview["cSerialNo"] = strSerialNO;

                    decimal decSign = (dtrGLRef["fcIOType"].ToString() == "I" ? 1 : -1);
                    decimal decQty = Convert.ToDecimal(dtrGLRef["fnQty"]);	// * Convert.ToDecimal(dtrGLRef["fnUmQty"]);
                    if (decSign > 0)
                    {
                        dtrPreview["NQTY_IN"] = decQty;
                    }
                    else
                    {
                        dtrPreview["NQTY_OUT"] = decQty;
                    }

                    int iDayRun = Convert.ToInt32(dtrGLRef["DayRun"]);
                    if (iDayRun < this.txtAge1.Value)
                    {
                        dtrPreview["AGL_01"] = decQty;
                    }
                    else if (iDayRun >= this.txtAge1.Value && iDayRun < this.txtAge2.Value)
                    {
                        dtrPreview["AGL_02"] = decQty;
                    }
                    else if (iDayRun > this.txtAge2.Value)
                    {
                        dtrPreview["AGL_03"] = decQty;
                    }


                    dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);
                }

            }

            if (dtsPrintPreview.XRPSTMOVE.Rows.Count > 0)
            {
                this.pmPreviewReport(dtsPrintPreview);
            }
            else
            {
                //this.EndReport();
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pmGetBFStock(string inQcWHouse, string inQcProd, ref decimal ioQty, ref decimal ioAmt)
        {
            decimal decQty = 0;
            decimal decAmt = 0;
            DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cQcWHouse = '" + inQcWHouse + "' and cQcProd = '" + inQcProd + "'");
            if (dtaSel.Length > 0)
            {
                for (int intCnt = 0; intCnt < dtaSel.Length; intCnt++)
                {
                    decQty += (Convert.ToDecimal(dtaSel[intCnt]["nSumQty"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                    decAmt += (Convert.ToDecimal(dtaSel[intCnt]["nSumCost"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                    dtaSel[intCnt]["cIsPrint"] = "Y";
                }
            }
            ioQty = decQty;
            ioAmt = decAmt;
        }

        private void pmCreateBFStock()
        {

            string strFld = " WHOUSE.FCCODE as cQcWHouse , PROD.FCCODE as cQcProd , REFPROD.FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as NSUMQTY , sum(REFPROD.FNCOSTAMT) as NSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            //strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' " + (this.cmbWHouse.SelectedIndex == 0 ? "and WHOUSE.FCCODE between '" + this.txtBegQcWHouse.Text.TrimEnd() + "' and '" + this.txtEndQcWHouse.Text.TrimEnd() + "' " : "");
            //strSQLStr = strSQLStr + " and PROD.FCTYPE = ? and PROD.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " and PROD.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " group by WHOUSE.FCCODE , PROD.FCCODE , REFPROD.FCIOTYPE";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.NotUpperSQLExecString = true;
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg);

            DataColumn dtcPrint = new DataColumn("cIsPrint", System.Type.GetType("System.String"));
            dtcPrint.DefaultValue = "N";
            this.dtsDataEnv.Tables["QBFStock"].Columns.Add(dtcPrint);

        }


        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //if (this.mstrPForm == "FORM1")
            //{
            //    strRPTFileName = Application.StartupPath + "\\RPT\\XRPAG01.RPT";
            //}
            //else
            //{
            //    strRPTFileName = Application.StartupPath + "\\RPT\\XRPAG02.RPT";
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

            //string strPDate = "";

            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPDate);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์สินค้ารหัสตั้งแต่ " + this.txtBegQcProd.Text.TrimEnd() + " ถึง " + this.txtEndQcProd.Text.TrimEnd());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์ตั้งแต่วันที่ " + this.txtBegDate.DateTime.ToString("dd/MM/yy") + " ถึง " + this.txtEndDate.DateTime.ToString("dd/MM/yy"));
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbIsDetail.SelectedIndex == 0 ? "Y" : "N");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtAge1.Value.ToString());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtAge2.Value.ToString());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, " ");
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "Y");

            //int i = 0;
            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pfShowDetail"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pfInAgeL1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pfInAgeL2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pfInAgeL3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        private string pmGetPOption()
        {

            string strResult = "";
            strResult += this.txtQcBranch.Text.TrimEnd() + " : " + this.txtQnBranch.Text.TrimEnd() + "|";
            strResult += this.cmbIsDetail.Text.TrimEnd() + "|";
            strResult += this.txtBegDate.DateTime.ToString("dd/MM/yy") + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtEndDate.DateTime.ToString("dd/MM/yy") + "|";
            strResult += this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtBegQnProd.Text.TrimEnd()
                                + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtEndQnProd.Text.TrimEnd() + "|";

            //strResult += this.cmbWHCost.Text.TrimEnd() + "|";

            return strResult;
        }

    }
}
