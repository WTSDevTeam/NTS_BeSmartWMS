
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

    public partial class rptPCost02 : UIHelper.frmBase
    {


        public rptPCost02(string inForm)
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

            string strPath = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\";
            int intLen1 = (strPath).Length;
            this.cmbPForm.Properties.Items.Clear();
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

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;

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

            this.cmbOrderBy.Properties.Items.Clear();
            this.cmbOrderBy.Properties.Items.AddRange(new object[] { 
                UIBase.GetAppUIText(new string[] { "พิมพ์เรียงตามวันที่", "Order by MO Date" })
                , UIBase.GetAppUIText(new string[] { "พิมพ์เรียงตามเลขที่เอกสาร" , "Order by MO Code" })});

            this.cmbOrderBy.SelectedIndex = 0;

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

            this.txtBegDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

            this.txtEndDoc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndDoc.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndDoc.Validating += new CancelEventHandler(txtBegDoc_Validating);

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

            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);


            this.cmbOrderBy.SelectedIndexChanged += new EventHandler(cmbOrderBy_SelectedIndexChanged);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

        }

        private void cmbOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetRngState();
        }

        private void pmCreateTem()
        {

            //รายการสินค้า
            this.dtsDataEnv.CaseSensitive = true;

            DataTable dtbTemPd = new DataTable(this.mstrTemPd);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QnProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QcUM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QnUM", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("Qty", System.Type.GetType("System.Decimal"));

            this.dtsDataEnv.Tables.Add(dtbTemPd);

        }

        private void pmSetRngState()
        {
            this.pnlDate.Enabled = this.cmbOrderBy.SelectedIndex == 0;
            this.pnlRngCode.Enabled = this.cmbOrderBy.SelectedIndex == 1;
        }

        private void pmSetFormUI()
        {
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่วันที่ :", "Date between :" });
            this.lblToDate.Text = UIBase.GetAppUIText(new string[] { "ถึง ", "To" });

            this.lblRngCode.Text = UIBase.GetAppUIText(new string[] { "เลือกเอกสารรหัสตั้งแต่", "Select #MO Code From :" });
            this.lblRngCode2.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngSect.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงแผนก", "Select Range Section" });
            this.lblFrSect.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToSect.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngJob.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงโครงการ", "Select Range Job" });
            this.lblFrJob.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToJob.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

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

                        this.txtBegDoc.Text = "";
                        this.txtEndDoc.Text = "";

                    }
                    break;
                case "TXTQCMFGBOOK":
                case "TXTQNMFGBOOK":
                    if (this.pofrmGetMfgBook != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetMfgBook.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (this.txtQcMfgBook.Tag.ToString() != dtrPDGRP["cRowID"].ToString())
                            {
                                this.txtBegDoc.Text = "";
                                this.txtEndDoc.Text = "";
                            }

                            this.txtQcMfgBook.Tag = dtrPDGRP["cRowID"].ToString();
                            this.txtQcMfgBook.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnMfgBook.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcMfgBook.Tag = "";
                            this.txtQcMfgBook.Text = "";
                            this.txtQnMfgBook.Text = "";

                            this.txtBegDoc.Text = "";
                            this.txtEndDoc.Text = "";
                        
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

        private void pmPrintData()
        {
            string strErrorMsg = "";

            dtsPrintPreview.PCOST_BOM.Rows.Clear();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";

            string strSQLStr = "";
            strSQLStr = "select ";

            strSQLStr += " MFWORDERHD.CROWID, MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE, MFWORDERHD.CMFGPROD, MFWORDERHD.CBOMHD , MFWORDERHD.NMFGQTY ";
            strSQLStr += " ,MFWORDERIT_STDOP.NT_QUEUE, MFWORDERIT_STDOP.NT_SETUP , MFWORDERIT_STDOP.NT_PROCESS, MFWORDERIT_STDOP.NT_TEAR, MFWORDERIT_STDOP.NT_WAIT, MFWORDERIT_STDOP.NT_MOVE ";
            strSQLStr += " ,MFWORDERIT_STDOP.COPSEQ, MFWORDERIT_STDOP.CROWID as CSTDOP ";
            strSQLStr += ",MFBOMHD.CCODE as QCBOM, MFBOMHD.CNAME as QNBOM";
            strSQLStr += ",MFSTDOPR.CCODE as QCSTDOPR, MFSTDOPR.CNAME as QNSTDOPR";
            strSQLStr += ",MFWKCTRHD.CCODE as QCWKCTR, MFWKCTRHD.CNAME as QNWKCTR";
            strSQLStr += ",MFRESOURCE.CCODE as QCRESOURCE, MFRESOURCE.CNAME as QNRESOURCE";
            strSQLStr += " from MFWORDERHD ";
            strSQLStr += " left join " + strSectTab + " SECT on SECT.FCSKID = MFWORDERHD.CSECT ";
            strSQLStr += " left join " + strJobTab + " JOB on JOB.FCSKID = MFWORDERHD.CJOB ";
            strSQLStr += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERHD.CMFGPROD ";
            strSQLStr += " left join MFBOMHD on MFBOMHD.CROWID = MFWORDERHD.CBOMHD ";
            strSQLStr += " left join MFWORDERIT_STDOP on MFWORDERIT_STDOP.CWORDERH = MFWORDERHD.CROWID ";
            strSQLStr += " left join MFSTDOPR on MFSTDOPR.CROWID = MFWORDERIT_STDOP.CMOPR ";
            strSQLStr += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWORDERIT_STDOP.CWKCTRH ";
            strSQLStr += " left join MFRESOURCE on MFRESOURCE.CROWID = MFWORDERIT_STDOP.CRESOURCE ";
            strSQLStr += " where MFWORDERHD.CCORP = ? ";
            strSQLStr += " and MFWORDERHD.CBRANCH = ? ";
            strSQLStr += " and MFWORDERHD.CPLANT = ? ";
            strSQLStr += " and MFWORDERHD.CREFTYPE = ? ";
            strSQLStr += " and MFWORDERHD.CMFGBOOK = ? ";
            if (this.cmbOrderBy.SelectedIndex == 0)
            {
                strSQLStr += " and MFWORDERHD.DDATE between ? and ? ";
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
                , this.txtEndQcJob.Text.TrimEnd()
                , this.txtBegQcProd.Text.TrimEnd()
                , this.txtEndQcProd.Text.TrimEnd() });
            }
            else
            {
                strSQLStr += " and MFWORDERHD.CCODE between ? and ? ";
                pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID
                , this.txtQcBranch.Tag.ToString()
                , this.txtQcPlant.Tag.ToString()
                , this.mstrRefType
                , this.txtQcMfgBook.Tag.ToString()
                , this.txtBegDoc.Text.TrimEnd()
                , this.txtEndDoc.Text.TrimEnd()
                , this.txtBegQcSect.Text.TrimEnd()
                , this.txtEndQcSect.Text.TrimEnd()
                , this.txtBegQcJob.Text.TrimEnd()
                , this.txtEndQcJob.Text.TrimEnd()
                , this.txtBegQcProd.Text.TrimEnd()
                , this.txtEndQcProd.Text.TrimEnd() });
            }

            strSQLStr += " and SECT.FCCODE between ? and ? ";
            strSQLStr += " and JOB.FCCODE between ? and ? ";
            strSQLStr += " and PROD.FCCODE between ? and ? ";
            strSQLStr += " order by MFWORDERHD.CCODE, MFWORDERIT_STDOP.COPSEQ";

            pobjSQLUtil2.NotUpperSQLExecString = true;

            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMH", QMFBOMInfo.TableName, strSQLStr, ref strErrorMsg);
            foreach (DataRow dtrBOM in this.dtsDataEnv.Tables["QBOMH"].Rows)
            {
                this.pmPrintBOM_PD(dtrBOM, dtrBOM["cRowID"].ToString(), dtrBOM["cBOMHD"].ToString(), dtrBOM["cOPSEQ"].ToString().TrimEnd());
            }
            this.pmPreviewReport(dtsPrintPreview);
        }

        private void pmPrintBOM_PD(DataRow inSource, string inWOrderH, string inBOMHD, string inOPSeq)
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strUMTab = strFMDBName + ".dbo.UM";

            string strSQLStr = "";
            strSQLStr = "select ";
            strSQLStr += " MFWORDERIT_PD.CROWID, MFWORDERIT_PD.NQTY, MFWORDERIT_PD.CPROCURE, MFWORDERIT_PD.COPSEQ, MFWORDERIT_PD.CPROD, MFWORDERIT_PD.CMFGBOMHD ";
            strSQLStr += " ,PROD.FCCODE as QCPROD,PROD.FCNAME as QNPROD, PROD.FNSTDCOST as NSTDCOST ";
            strSQLStr += " from MFWORDERIT_PD ";
            strSQLStr += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWORDERIT_PD.CPROD ";
            strSQLStr += " where MFWORDERIT_PD.CWORDERH = ? and MFWORDERIT_PD.COPSEQ = ? ";
            strSQLStr += " order by MFWORDERIT_PD.CSEQ ";

            DataRow dtrSource = dtsPrintPreview.PCOST_BOM.NewRow();
            dtrSource["MO_CODE"] = inSource["CCODE"].ToString().TrimEnd();
            dtrSource["MO_REFNO"] = inSource["CREFNO"].ToString().TrimEnd();
            dtrSource["MO_DATE"] = Convert.ToDateTime(inSource["DDATE"]);
            dtrSource["BOM_NAME"] = inSource["QNBOM"].ToString().TrimEnd();
            dtrSource["BOM_OUTPUT_PROD_CODE"] = "";
            dtrSource["BOM_OUTPUT_PROD_NAME"] = "";

            pobjSQLUtil.SetPara(new object[] { inSource["CMFGPROD"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMfgProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QMfgProd"].Rows[0];
                dtrSource["BOM_OUTPUT_PROD_CODE"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrSource["BOM_OUTPUT_PROD_NAME"] = dtrProd["fcName"].ToString().TrimEnd();
            }

            string strStdOP = "";
            pobjSQLUtil2.SetPara(new object[] { inBOMHD, inOPSeq });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOM_OP", "MFBOMIT_STDOP", "select CROWID from MFBOMIT_STDOP where CBOMHD = ? and COPSEQ = ? ", ref strErrorMsg))
            {
                DataRow dtrBOMOP = this.dtsDataEnv.Tables["QBOM_OP"].Rows[0];
                strStdOP = dtrBOMOP["cRowID"].ToString();
            }

            dtrSource["MO_MFGQTY"] = Convert.ToDecimal(inSource["NMFGQTY"]);
            dtrSource["OP_OPSEQ"] = inOPSeq;
            dtrSource["OP_OPR_CODE"] = inSource["QCSTDOPR"].ToString().TrimEnd();
            dtrSource["OP_OPR_NAME"] = inSource["QNSTDOPR"].ToString().TrimEnd();
            dtrSource["OP_WC_CODE"] = inSource["QCWKCTR"].ToString().TrimEnd();
            dtrSource["OP_WC_NAME"] = inSource["QNWKCTR"].ToString().TrimEnd();
            dtrSource["OP_TOOL_CODE"] = inSource["QCRESOURCE"].ToString().TrimEnd();
            dtrSource["OP_TOOL_NAME"] = inSource["QNRESOURCE"].ToString().TrimEnd();

            //Print OP COST
            decimal intHour = 0; decimal intMin = 0; decimal intSec = 0;
            decimal decTimeFactor = 1;
            decimal decOPTime = Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Queue])
                + Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_SetUp])
                + (Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor)
                + Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Tear])
                + Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Wait])
                + Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Move]);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Queue]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_QUEUE"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_SetUp]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_SETUP"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS((Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_PROCESS"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Tear]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_TEARDOWN"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Wait]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_WAIT"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_MOVE"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(Convert.ToDecimal(inSource[QMFBOMInfo.Field.LeadTime_Move]), out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_MOVE"] = this.pmConvertTime2Text(intHour, intMin, intSec);

            AppUtil.CommonHelper.ConvertSecToHMS(decOPTime, out intHour, out intMin, out intSec);
            dtrSource["OP_TIME_TOTAL"] = this.pmConvertTime2Text(intHour, intMin, intSec);
            dtrSource["OP_TIME_TOTAL2"] = decOPTime;

            //Get OP Cost from COSTLINE Table
            QCostLineInfo oCostLine = new QCostLineInfo();

            //Load Cost by OP
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, strStdOP, "O");

            dtrSource["OP_COST_O_FIX"] = oCostLine.Cost_Fix;
            dtrSource["OP_COST_O_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
            dtrSource["OP_COST_O_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
            dtrSource["OP_COST_O_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
            dtrSource["OP_COST_O_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
            dtrSource["OP_COST_O_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
            dtrSource["OP_COST_O_VP1"] = oCostLine.Cost_Var_ByOutput1;
            dtrSource["OP_COST_O_VP2"] = oCostLine.Cost_Var_ByOutput2;
            dtrSource["OP_COST_O_VP3"] = oCostLine.Cost_Var_ByOutput3;
            dtrSource["OP_COST_O_VP4"] = oCostLine.Cost_Var_ByOutput4;
            dtrSource["OP_COST_O_VP5"] = oCostLine.Cost_Var_ByOutput5;

            //Load Cost by WorkCenter
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, strStdOP, "W");
            dtrSource["OP_COST_W_FIX"] = oCostLine.Cost_Fix;
            dtrSource["OP_COST_W_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
            dtrSource["OP_COST_W_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
            dtrSource["OP_COST_W_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
            dtrSource["OP_COST_W_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
            dtrSource["OP_COST_W_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
            dtrSource["OP_COST_W_VP1"] = oCostLine.Cost_Var_ByOutput1;
            dtrSource["OP_COST_W_VP2"] = oCostLine.Cost_Var_ByOutput2;
            dtrSource["OP_COST_W_VP3"] = oCostLine.Cost_Var_ByOutput3;
            dtrSource["OP_COST_W_VP4"] = oCostLine.Cost_Var_ByOutput4;
            dtrSource["OP_COST_W_VP5"] = oCostLine.Cost_Var_ByOutput5;

            //Load Cost by Tool (Resource)
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, strStdOP, "T");
            dtrSource["OP_COST_T_FIX"] = oCostLine.Cost_Fix;
            dtrSource["OP_COST_T_VM1"] = oCostLine.Cost_Var_ManHour1 * decOPTime;
            dtrSource["OP_COST_T_VM2"] = oCostLine.Cost_Var_ManHour2 * decOPTime;
            dtrSource["OP_COST_T_VM3"] = oCostLine.Cost_Var_ManHour3 * decOPTime;
            dtrSource["OP_COST_T_VM4"] = oCostLine.Cost_Var_ManHour4 * decOPTime;
            dtrSource["OP_COST_T_VM5"] = oCostLine.Cost_Var_ManHour5 * decOPTime;
            dtrSource["OP_COST_T_VP1"] = oCostLine.Cost_Var_ByOutput1;
            dtrSource["OP_COST_T_VP2"] = oCostLine.Cost_Var_ByOutput2;
            dtrSource["OP_COST_T_VP3"] = oCostLine.Cost_Var_ByOutput3;
            dtrSource["OP_COST_T_VP4"] = oCostLine.Cost_Var_ByOutput4;
            dtrSource["OP_COST_T_VP5"] = oCostLine.Cost_Var_ByOutput5;

            pobjSQLUtil2.NotUpperSQLExecString = true;
            pobjSQLUtil2.SetPara(new object[] { inWOrderH, inOPSeq });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMIT", "MFWORDERIT_PD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBOMItem in this.dtsDataEnv.Tables["QBOMIT"].Rows)
                {
                    DataRow dtrPrnData = dtsPrintPreview.PCOST_BOM.NewRow();

                    DataSetHelper.CopyDataRow(dtrSource, ref dtrPrnData);

                    this.pmClearCost(dtrSource);

                    dtrPrnData["RM_PROD_CODE"] = dtrBOMItem["QCPROD"].ToString().TrimEnd();
                    dtrPrnData["RM_PROD_NAME"] = dtrBOMItem["QNPROD"].ToString().TrimEnd();
                    dtrPrnData["RM_QTY"] = Convert.ToDecimal(dtrBOMItem["NQTY"]);

                    if (dtrBOMItem["CPROCURE"].ToString().TrimEnd() == "M")
                    {
                        this.mdecSumBOMAmt = 0;
                        this.mdecSumBOMOPAmt = 0;
                        this.mdecSumBOMAmt_Actual = 0;
                        this.mdttCurrDate = Convert.ToDateTime(inSource["dDate"]).Date;
                        //this.pmGetBOMCost(0, dtrBOMItem["cMfgBOMHD"].ToString(), Convert.ToDecimal(dtrBOMItem["NQTY"]));
                        //dtrPrnData["RM_COST"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt;
                        this.pmGetBOMCost(0, dtrBOMItem["cMfgBOMHD"].ToString(), 1);
                        //this.pmGetBOMCost(0, dtrBOMItem["cMfgBOMHD"].ToString(), 1);
                        //dtrPrnData["RM_COST"] = (this.mdecSumBOMOPAmt + this.mdecSumBOMAmt) * Convert.ToDecimal(dtrBOMItem["NQTY"]);
                        dtrPrnData["RM_COST"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt;
                        dtrPrnData["RM_COST_ACTUAL"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt_Actual;
                    }
                    else
                    {
                        if (this.cmbWHCost.SelectedIndex == 0)
                        {
                            dtrPrnData["RM_COST"] = Convert.ToDecimal(dtrBOMItem["NSTDCOST"]);
                        }
                        else
                        {
                            dtrPrnData["RM_COST"] = this.pmGetBuyPrice(dtrBOMItem["cProd"].ToString(), this.cmbWHCost.SelectedIndex);
                        }

                        //TODO: เปลี่ยนให้รองรับ costแบบ FIFO
                        if (App.ActiveCorp.CostMethod_Rawmat == "A")
                        {
                            dtrPrnData["RM_COST_ACTUAL"] = this.pmGetAVGPrice(dtrBOMItem["cProd"].ToString(), Convert.ToDateTime(inSource["dDate"]));
                        }
                        else
                        {
                            string strProd = dtrBOMItem["cProd"].ToString();
                            decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), Convert.ToDateTime(inSource["dDate"]));
                            decimal decAmt = 0;
                            decimal decSedQty = 0;
                            int intSedRecn = 0;
                            decimal decSedCost = 0;
                            decimal decSedQty2 = 0;
                            string strSedRefNo = "";
                            decimal decFIFOCost = 0;
                            //dtrStock["StockQty"] = decStkQty;

                            //App.AppMessage = "คำนวณราคาทุน : " + "\r\n(" + dtrStock["QcProd"].ToString().TrimEnd() + ")" + dtrStock["QnProd"].ToString().TrimEnd();

                            GLHelper.PrintBFQTYLine(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), Convert.ToDateTime(inSource["dDate"]), Convert.ToDateTime(inSource["dDate"]), decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);
                            DataView dv = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].DefaultView;
                            dv.Sort = "fdDate asc";
                            for (int i = 0; i < dv.Count; i++)
                            {
                                decFIFOCost += Convert.ToDecimal(dv[i]["fnCostAmt"]) + Convert.ToDecimal(dv[i]["fnCostAdj"]);
                                //this.pmAddProd2(dtrStock["cProd"].ToString(), dtrStock["QcProd"].ToString(), dtrStock["QnProd"].ToString(), dtrStock["QnUM"].ToString(), Convert.ToDecimal(dv[i]["fnQty"]), Convert.ToDecimal(dv[i]["fnCostAmt"]) + Convert.ToDecimal(dv[i]["fnCostAdj"]));
                            }
                            dtrPrnData["RM_COST_ACTUAL"] = decFIFOCost;

                        }
                    
                    }

                    dtsPrintPreview.PCOST_BOM.Rows.Add(dtrPrnData);
                }
            }
            else
            {
                DataRow dtrPrnData = dtsPrintPreview.PCOST_BOM.NewRow();
                DataSetHelper.CopyDataRow(dtrSource, ref dtrPrnData);
                dtsPrintPreview.PCOST_BOM.Rows.Add(dtrPrnData);
            }
        }

        private void pmClearCost(DataRow inSource)
        {
            inSource["OP_COST_O_FIX"] = 0;
            inSource["OP_COST_O_VM1"] = 0;
            inSource["OP_COST_O_VM2"] = 0;
            inSource["OP_COST_O_VM3"] = 0;
            inSource["OP_COST_O_VM4"] = 0;
            inSource["OP_COST_O_VM5"] = 0;
            inSource["OP_COST_O_VP1"] = 0;
            inSource["OP_COST_O_VP2"] = 0;
            inSource["OP_COST_O_VP3"] = 0;
            inSource["OP_COST_O_VP4"] = 0;
            inSource["OP_COST_O_VP5"] = 0;

            //Load Cost by WorkCenter
            inSource["OP_COST_W_FIX"] = 0;
            inSource["OP_COST_W_VM1"] = 0;
            inSource["OP_COST_W_VM2"] = 0;
            inSource["OP_COST_W_VM3"] = 0;
            inSource["OP_COST_W_VM4"] = 0;
            inSource["OP_COST_W_VM5"] = 0;
            inSource["OP_COST_W_VP1"] = 0;
            inSource["OP_COST_W_VP2"] = 0;
            inSource["OP_COST_W_VP3"] = 0;
            inSource["OP_COST_W_VP4"] = 0;
            inSource["OP_COST_W_VP5"] = 0;

            //Load Cost by Tool (Resource)
            inSource["OP_COST_T_FIX"] = 0;
            inSource["OP_COST_T_VM1"] = 0;
            inSource["OP_COST_T_VM2"] = 0;
            inSource["OP_COST_T_VM3"] = 0;
            inSource["OP_COST_T_VM4"] = 0;
            inSource["OP_COST_T_VM5"] = 0;
            inSource["OP_COST_T_VP1"] = 0;
            inSource["OP_COST_T_VP2"] = 0;
            inSource["OP_COST_T_VP3"] = 0;
            inSource["OP_COST_T_VP4"] = 0;
            inSource["OP_COST_T_VP5"] = 0;
        }

        private void pmGetBOMCost(int inLevel, string inBOMHD, decimal inMfgQty)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strSQLStrBOMIT_Pd = "select * from " + MapTable.Table.MFBOMIT_PD + " where cBOMHD = ? order by cSeq";

            //TODO: Sum OP Cost Here

            string strSQLStr = "";
            strSQLStr = "select ";

            strSQLStr += " MFBOMHD.CROWID, MFBOMHD.CCODE, MFBOMHD.CNAME, MFBOMHD.CMFGPROD, MFBOMHD.NMFGQTY ";
            strSQLStr += " ,MFBOMIT_STDOP.NT_QUEUE, MFBOMIT_STDOP.NT_SETUP , MFBOMIT_STDOP.NT_PROCESS, MFBOMIT_STDOP.NT_TEAR, MFBOMIT_STDOP.NT_WAIT, MFBOMIT_STDOP.NT_MOVE ";
            strSQLStr += " ,MFBOMIT_STDOP.COPSEQ, MFBOMIT_STDOP.CROWID as CSTDOP ";
            strSQLStr += ",MFSTDOPR.CCODE as QCSTDOPR, MFSTDOPR.CNAME as QNSTDOPR";
            strSQLStr += ",MFWKCTRHD.CCODE as QCWKCTR, MFWKCTRHD.CNAME as QNWKCTR";
            strSQLStr += ",MFRESOURCE.CCODE as QCRESOURCE, MFRESOURCE.CNAME as QNRESOURCE";
            strSQLStr += " from MFBOMHD ";
            strSQLStr += " left join MFBOMIT_STDOP on MFBOMIT_STDOP.CBOMHD = MFBOMHD.CROWID ";
            strSQLStr += " left join MFSTDOPR on MFSTDOPR.CROWID = MFBOMIT_STDOP.CMOPR ";
            strSQLStr += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFBOMIT_STDOP.CWKCTRH ";
            strSQLStr += " left join MFRESOURCE on MFRESOURCE.CROWID = MFBOMIT_STDOP.CRESOURCE ";
            strSQLStr += " where MFBOMHD.CROWID = ? ";
            strSQLStr += " order by MFBOMIT_STDOP.COPSEQ";

            decimal decFactorQty = 0;
            DataRow dtrBomH = null;
            pobjSQLUtil.SetPara(new object[] { inBOMHD });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOMHD", "MFBOMHD", strSQLStr, ref strErrorMsg))
            {
                dtrBomH = this.dtsDataEnv.Tables["QBOMHD"].Rows[0];
                decFactorQty = inMfgQty / Convert.ToDecimal(dtrBomH[QMFBOMInfo.Field.MfgQty]);

                foreach (DataRow dtrPBOMHD in this.dtsDataEnv.Tables["QBOMHD"].Rows)
                {
                    //Print OP COST
                    decimal decTimeFactor = 1;
                    decimal decOPTime = Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Queue])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_SetUp])
                        + (Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Process]) * decTimeFactor)
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Tear])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Wait])
                        + Convert.ToDecimal(dtrPBOMHD[QMFBOMInfo.Field.LeadTime_Move]);

                    //Get OP Cost from COSTLINE Table
                    QCostLineInfo oCostLine = new QCostLineInfo();

                    //Load Cost by OP
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "O");

                    decimal decOPCost_OP = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    //Load Cost by WorkCenter
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "W");
                    decimal decOPCost_WC = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    //Load Cost by Tool (Resource)
                    MRPAgent.GetCostLine(pobjSQLUtil, ref oCostLine, MapTable.Table.MFBOMIT_OP, dtrPBOMHD["cStdOP"].ToString(), "T");
                    decimal decOPCost_Tool = oCostLine.Cost_Fix
                                                        + oCostLine.Cost_Var_ManHour1 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour2 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour3 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour4 * decOPTime
                                                        + oCostLine.Cost_Var_ManHour5 * decOPTime
                                                        + oCostLine.Cost_Var_ByOutput1
                                                        + oCostLine.Cost_Var_ByOutput2
                                                        + oCostLine.Cost_Var_ByOutput3
                                                        + oCostLine.Cost_Var_ByOutput4
                                                        + oCostLine.Cost_Var_ByOutput5;

                    this.mdecSumBOMOPAmt += (decFactorQty * (decOPCost_OP + decOPCost_WC + decOPCost_Tool));
                }
            }

            pobjSQLUtil.SetPara(new object[] { inBOMHD });

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOMI", "BOMIT", strSQLStrBOMIT_Pd, ref strErrorMsg);

            #region "Print RM Item"
            foreach (DataRow dtrBOMItem in this.dtsDataEnv.Tables["QBOMI"].Rows)
            {

                int intLevel = inLevel + 1;

                //inMfgQty
                decimal decMfgQty = decFactorQty * Convert.ToDecimal(dtrBOMItem["nQty"]);
                decimal decPrice = 0;
                decimal decPrice_Actual = 0;

                if (this.cmbWHCost.SelectedIndex == 0)
                {
                    pobjSQLUtil2.SetPara(new object[] { dtrBOMItem["cProd"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ? ", ref strErrorMsg))
                    {
                        decPrice = Convert.ToDecimal(this.dtsDataEnv.Tables["QProd"].Rows[0]["FNSTDCOST"]);
                    }
                }
                else
                {
                    decPrice = this.pmGetBuyPrice(dtrBOMItem["cProd"].ToString(), this.cmbWHCost.SelectedIndex);
                }
                //TODO: เปลี่ยนให้รองรับ costแบบ FIFO
                decPrice_Actual = this.pmGetAVGPrice(dtrBOMItem["cProd"].ToString(), this.mdttCurrDate);

                this.mdecSumBOMAmt_Actual += decMfgQty * decPrice_Actual;
                this.mdecSumBOMAmt += decMfgQty * decPrice;

                if (dtrBOMItem["cProcure"].ToString() == "M")
                {
                    this.pmGetBOMCost(intLevel, dtrBOMItem["cMfgBOMHD"].ToString(), decMfgQty);
                }

            }
            #endregion

        }

        private string mstrSaleOrBuy = "P";

        private decimal pmGetBuyPrice(string inProd, int inWhPrice)
        {

            //inWhPrice = 1; ราคาซื้อล่าสุด
            //inWhPrice = 2; ราคาซื้อต่ำสุด
            //inWhPrice = 3; ราคาซื้อสูงสุด

            decimal decRetVal = 0;
            string strMsg = "";

            string strSaleOrBuy = (this.mstrSaleOrBuy == "S" ? "ราคาขาย" : "ราคาซื้อ");
            string strMaxRef = "";
            string strMinRef = "";
            string strLastRef = "";
            decimal decLastAmt = 0;
            decimal decMaxAmt = 0;
            decimal decMinAmt = Convert.ToDecimal(999999999.99);
            DateTime dttLastDate = DateTime.MinValue;
            DateTime dttCurDate = DateTime.Now;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            string strProd = inProd;
            if (strProd != "" && this.mstrSaleOrBuy != "S")
            {
                pobjSQLUtil.SetPara(new object[] { strProd, this.txtQcBranch.Tag.ToString() });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", "select * from RefProd where fcProd = ? and fcBranch = ? ", ref strErrorMsg))
                {
                    foreach (DataRow dtrOrderI in this.dtsDataEnv.Tables["QRefProd"].Rows)
                    {
                        if ((SysDef.gc_RFTYPE_INV_BUY + SysDef.gc_RFTYPE_DR_BUY).IndexOf(dtrOrderI["fcRfType"].ToString()) > -1)
                        {
                            decimal decUmQty = (Convert.ToDecimal(dtrOrderI["fnUmQty"]) == 0 ? 1 : Convert.ToDecimal(dtrOrderI["fnUmQty"]));
                            #region "Cal Min&Max Buy Price"
                            if (Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty >= decMaxAmt)
                            {
                                //ราคาซื้อสูงสุด
                                decMaxAmt = Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty;
                                strMaxRef = dtrOrderI["fcSkid"].ToString();
                            }
                            if (Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty <= decMinAmt && Convert.ToDecimal(dtrOrderI["fnPrice"]) != 0)
                            {
                                //ราคาซื้อต่ำสุด
                                decMinAmt = Convert.ToDecimal(dtrOrderI["fnPrice"]) / decUmQty;
                                strMinRef = dtrOrderI["fcSkid"].ToString();
                            }

                            if (Convert.ToDateTime(dtrOrderI["fdDate"]).Date.CompareTo(dttLastDate.Date) > 0
                                || (Convert.ToDateTime(dtrOrderI["fdDate"]).Date.CompareTo(dttLastDate.Date) == 0 && Convert.ToDateTime(dtrOrderI["ftDateTime"]).CompareTo(dttCurDate) > 0)
                                && Convert.ToDecimal(dtrOrderI["fnPrice"]) != 0)
                            {
                                dttLastDate = Convert.ToDateTime(dtrOrderI["fdDate"]);
                                dttCurDate = Convert.ToDateTime(dtrOrderI["ftDateTime"]);
                                strLastRef = dtrOrderI["fcSkid"].ToString();
                            }
                            #endregion
                        }
                    }
                }
            }

            string strDate = "วันที่ [";
            string strLastAmt = "";
            string strMaxAmt = "";
            string strMinAmt = "";
            string strRefProdCmd = "select REFPROD.FDDATE, REFPROD.FNPRICE, UM.FCNAME as QnUM, CURRENCY.FCNAME as QnCurrency from REFPROD ";
            strRefProdCmd += " left join UM on UM.FCSKID = REFPROD.FCUM ";
            strRefProdCmd += " left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strRefProdCmd += " left join CURRENCY on CURRENCY.FCSKID = GLREF.FCCURRENCY ";
            strRefProdCmd += " where REFPROD.FCSKID = ?";
            DataRow dtrTem = null;

            if (strLastRef != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[] { strLastRef });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                {
                    dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                    string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                    decLastAmt = Convert.ToDecimal(dtrTem["fnPrice"]);
                    strLastAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                }
            }

            if (strMaxRef != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[] { strMaxRef });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                {
                    dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                    string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                    strMaxAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                }
            }

            if (strMinRef != string.Empty)
            {
                pobjSQLUtil.SetPara(new object[] { strMinRef });
                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRefProd", "REFPROD", strRefProdCmd, ref strErrorMsg))
                {
                    dtrTem = this.dtsDataEnv.Tables["QRefProd"].Rows[0];
                    string strCurr = (Convert.IsDBNull(dtrTem["QnCurrency"]) ? "" : "]" + dtrTem["QnCurrency"].ToString().TrimEnd() + "[");
                    strMinAmt = Convert.ToDecimal(dtrTem["fnPrice"]).ToString("#,###,###,##0.00") + strCurr + dtrTem["QnUm"].ToString().TrimEnd() + strDate + Convert.ToDateTime(dtrTem["fdDate"]).ToString("dd/MM/yy");
                }
            }
            strMsg += "\r\n" + strSaleOrBuy + "ล่าสุด [" + strLastAmt + "]";
            strMsg += "\r\n" + strSaleOrBuy + "สูงสุด [" + strMaxAmt + "]";
            strMsg += "\r\n" + strSaleOrBuy + "ต่ำสุด [" + strMinAmt + "]";

            switch (inWhPrice)
            {
                case 1:
                    decRetVal = decLastAmt;
                    break;
                case 2:
                    decRetVal = decMinAmt;
                    break;
                case 3:
                    decRetVal = decMaxAmt;
                    break;
            }
            return decRetVal;
        }

        private decimal pmGetAVGPrice(string inProd, DateTime inDate)
        {

            string strErrorMsg = "";
            
            decimal decPrice = 0;

            decimal tSign = 1;
            decimal tCostAmt = 0;
            decimal tRefCost = 0;
            decimal tQty = 0;
            decimal tAmt = 0;
            decimal outAmt = 0;
            decimal outQty = 0;


            string strFldLst = " PROD.FCCODE as QCPROD, RefProd.fcIOType, RefProd.fcRfType, RefProd.fcIOType, RefProd.FNQTY, RefProd.FNUMQTY , RefProd.fnCostAmt, RefProd.fnCostAdj ";
            string strSQLStr = "select " + strFldLst + " from REFPROD ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE";
            strSQLStr = strSQLStr + " where REFPROD.FCPROD = ?";
            strSQLStr = strSQLStr + " and REFPROD.FCBRANCH = ?";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE <= ?";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIoType";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { inProd, this.txtQcBranch.Tag.ToString(), inDate.Date });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCalPrice", "REFPROD", strSQLStr, ref strErrorMsg);
            foreach (DataRow drBFQty in this.dtsDataEnv.Tables["QCalPrice"].Rows)
            {

                tSign = (drBFQty["fcIoType"].ToString() == "I" ? 1 : -1);
                tQty = Convert.ToDecimal(drBFQty["fnQty"]) * Convert.ToDecimal(drBFQty["fnUmQty"]) * tSign;
                tCostAmt = Math.Abs(Convert.ToDecimal(drBFQty["fnCostAmt"]) + Convert.ToDecimal(drBFQty["fnCostAdj"]));

                if (tQty >= 0)
                {

                    tRefCost = (tQty != 0 ? tCostAmt / tQty : 0);
                    if (drBFQty["fcRfType"].ToString() == "F")
                    {
                        tRefCost = (outQty != 0 ? outAmt / outQty : 0);
                    }

                    tAmt = Math.Round(tQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    if (this.faPRefProd(drBFQty))
                    {

                        outAmt = outAmt + tAmt;
                        outQty = outQty + tQty;

                        if (outQty <= 0)
                            outAmt = 0;
                        else if (outQty < tQty)
                            //outAmt = outQty * tRefCost;
                            outAmt = Math.Round(outQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                        else if (outAmt < 0)
                            outAmt = 0;
                    }
                }
                else
                {

                    if (outQty != 0)
                    {
                        tRefCost = outAmt / outQty;
                        tAmt = Math.Round(tQty * tRefCost, App.ActiveCorp.RoundAmtAt, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        tRefCost = 0;
                        tAmt = 0;
                    }

                    if (this.faPRefProd(drBFQty))
                    {

                        outAmt = outAmt + tAmt;
                        outQty = outQty + tQty;

                        if (outQty <= 0)
                            outAmt = 0;
                        else if (outAmt < 0)
                            outAmt = 0;
                    }

                }

            }

            return (outQty != 0 ? outAmt / outQty : 0);
        }

        private string pstrXtraType = SysDef.gc_RFTYPE_TRAN_PD + SysDef.gc_RFTYPE_ISSUE_PD + SysDef.gc_RFTYPE_PRODUCE_PD + SysDef.gc_RFTYPE_RETURN_ISSUE;

        private bool faPRefProd(DataRow inRow)
        {
            decimal tSign = (inRow["fcIoType"].ToString() == "I" ? 1 : -1);
            decimal tQty = Convert.ToDecimal(inRow["fnQty"]) * Convert.ToDecimal(inRow["fnUmQty"]) * tSign;

            bool bllResult = (pstrXtraType.IndexOf(inRow["fcRfType"].ToString().Trim()) < 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_ISSUE_PD && tQty < 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_PRODUCE_PD && tQty >= 0
                                            || inRow["fcRfType"].ToString() == SysDef.gc_RFTYPE_RETURN_ISSUE && tQty >= 0);

            return bllResult;
        }

        private decimal pmPrintStock(string inProd, DateTime inDate)
        {

            string strSQLStr = "select ";
            strSQLStr = strSQLStr + " REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " , (select PROD.FCCODE from PROD where PROD.FCSKID = REFPROD.FCPROD) as QcProd";
            strSQLStr = strSQLStr + " , (select PROD.FCNAME from PROD where PROD.FCSKID = REFPROD.FCPROD) as QnProd";
            strSQLStr = strSQLStr + " , (select UM.FCNAME from UM where UM.FCSKID = REFPROD.FCUM) as QnUM";
            strSQLStr = strSQLStr + " , REFPROD.FCIOTYPE as IOType , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as SumQty ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " left join UM on UM.FCSKID = PROD.FCUM ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = @xaCorp and REFPROD.FCBRANCH = @xaBranch and REFPROD.FCPROD = @xaProd and REFPROD.FDDATE <= @xaDate and REFPROD.FCSTAT <> 'C'  ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD, REFPROD.FCIOTYPE, REFPROD.FCUM ";
            strSQLStr = strSQLStr + " order by QcProd ";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(App.ERPConnectionString2);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQLStr, conn);
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@xaCorp", App.gcCorp);
            cmd.Parameters.AddWithValue("@xaBranch", this.txtQcBranch.Tag.ToString());
            cmd.Parameters.AddWithValue("@xaProd", inProd);
            cmd.Parameters.AddWithValue("@xaDate", inDate.Date);

            decimal decSumQty = 0;
            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();
            try
            {
                conn.Open();
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        decimal decQty = Convert.ToDecimal(dr["SumQty"]) * (dr["IOType"].ToString() == "I" ? 1 : -1);
                        decSumQty += decQty;
                        this.pmAddProd(dr["fcProd"].ToString(), dr["QcProd"].ToString(), dr["QnProd"].ToString(), dr["QnUM"].ToString(), decQty);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                App.WriteEventsLog(ex);
            }
            finally
            {
                conn.Close();
            }
            return decSumQty;
        }

        private void pmAddProd(string inProd, string inQcProd, string inQnProd, string inQnUM, decimal inQty)
        {
            string strFilter = string.Format("QcProd = '{0}' ", new string[] { inQcProd });
            DataRow[] strSel = this.dtsDataEnv.Tables[this.mstrTemPd].Select(strFilter);
            if (strSel.Length == 0)
            {
                DataRow dtrPreview = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                dtrPreview["cProd"] = inProd;
                dtrPreview["QcProd"] = inQcProd;
                dtrPreview["QnProd"] = inQnProd;
                dtrPreview["QnUM"] = inQnUM;
                dtrPreview["Qty"] = inQty;
                this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrPreview);
            }
            else
            {
                strSel[0]["Qty"] = Convert.ToDecimal(strSel[0]["Qty"]) + inQty;
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\" + this.cmbPForm.Text.TrimEnd();

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
            ////string strPDate = UIBase.GetAppReportText(new string[] { "ตั้งแต่วันที่ ", "Date between " })
            ////                                    + UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtBegDate.DateTime, "dd/MM/yy"), this.txtBegDate.DateTime.ToString("dd/MM/yy") })
            ////                                    + UIBase.GetAppReportText(new string[] { " ถึง ", " to " })
            ////                                    + UIBase.GetAppReportText(new string[] { UIBase.GetThaiDate(this.txtEndDate.DateTime, "dd/MM/yy"), this.txtEndDate.DateTime.ToString("dd/MM/yy") });

            //string strCostName_FIX = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) : "Fix Cost");
            //string strCostName_VAR1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) : "Variable Cost per Man-hour 1");
            //string strCostName_VAR2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) : "Variable Cost per Man-hour 2");
            //string strCostName_VAR3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) : "Variable Cost per Man-hour 3");
            //string strCostName_VAR4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) : "Variable Cost per Man-hour 4");
            //string strCostName_VAR5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) : "Variable Cost per Man-hour 5");
            //string strCostName_VARP1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) : "Variable Cost per Product output 1");
            //string strCostName_VARP2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) : "Variable Cost per Product output 2");
            //string strCostName_VARP3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) : "Variable Cost per Product output 3");
            //string strCostName_VARP4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) : "Variable Cost per Product output 4");
            //string strCostName_VARP5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) : "Variable Cost per Product output 5");

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strPDate);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_FIX);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VAR1);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VAR2);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VAR3);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VAR4);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VAR5);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VARP1);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VARP2);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VARP3);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VARP4);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strCostName_VARP5);

            //int i = 0;
            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_FIX"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VAR1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VAR2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VAR3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VAR4"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VAR5"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VARP1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VARP2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VARP3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VARP4"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pf_CostName_VARP5"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);

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

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        private string pmGetPOption()
        {

            string strResult = "";
            strResult += this.txtQcBranch.Text.TrimEnd() + " : " + this.txtQnBranch.Text.TrimEnd() + "|";
            strResult += this.txtQcPlant.Text.TrimEnd() + " : " + this.txtQnPlant.Text.TrimEnd() + "|";
            strResult += this.txtQcMfgBook.Text.TrimEnd() + " : " + this.txtQnMfgBook.Text.TrimEnd() + "|";
            strResult += this.cmbOrderBy.Text.TrimEnd() + "|";
            strResult += this.txtBegDate.DateTime.ToString("dd/MM/yy") + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtEndDate.DateTime.ToString("dd/MM/yy") + "|";
            strResult += this.txtBegQcSect.Text.TrimEnd() + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcSect.Text.TrimEnd() + "|";
            strResult += this.txtBegQcJob.Text.TrimEnd() + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcJob.Text.TrimEnd() + "|";
            strResult += this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtBegQnProd.Text.TrimEnd()
                                + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtEndQnProd.Text.TrimEnd() + "|";

            strResult += this.cmbWHCost.Text.TrimEnd() + "|";

            return strResult;
        }

    }
}
