
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using AppUtil;

namespace BeSmartMRP.Transaction
{

    public partial class frmRecalFGPrice : UIHelper.frmBase
    {

        public frmRecalFGPrice()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;
        }

        private string mstrHTable = QMFWCloseHDInfo.TableName;
        private string mstrRefTable = QMFWCloseHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWCloseIT_OP;
        private string mstrITable2 = MapTable.Table.MFWCloseIT_PD;

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;

        Report.LocalDataSet.DTSPMCLIST dtsPrintPreview = new Report.LocalDataSet.DTSPMCLIST();

        private decimal mdecSumBOMOPAmt = 0;
        private decimal mdecSumBOMAmt = 0;
        private decimal mdecSumBOMAmt_Actual = 0;
        private decimal mdecSumBOMOverHeadAmt = 0;

        private decimal mdecSumFGQty = 0;

        private DateTime mdttCurrDate = DateTime.MinValue;

        private decimal mdec_MC_COSTDL = 0;
        private decimal mdec_MC_COSTOH = 0;
        private decimal mdec_MC_COSTADJ1 = 0;
        private decimal mdec_MC_COSTADJ2 = 0;
        private decimal mdec_MC_COSTADJ3 = 0;
        private decimal mdec_MC_COSTADJ4 = 0;
        private decimal mdec_MC_COSTADJ5 = 0;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = DocumentType.MC.ToString();
        private int mintYear = 0;
        private string mstrSect = "";
        private string mstrTemPd = "TemPd";
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
                this.txtBegQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select top 1 fcSkid, fcCode, fcName from SECT where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtEndQcSect.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtEndQnSect.Text = dtrValue["fcName"].ToString().TrimEnd();
            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
                this.txtBegQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtBegQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select top 1 fcSkid, fcCode, fcName from JOB where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QJob"].Rows[0];
                this.txtEndQcJob.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtEndQnJob.Text = dtrValue["fcName"].ToString().TrimEnd();
            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcSkid, fcCode, fcName from PROD where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.txtBegQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                this.txtBegQnProd.Text = dtrProd["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcSkid, fcCode, fcName from PROD where fcCorp = ? order by FCCODE desc", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                this.txtEndQcProd.Text = dtrProd["fcCode"].ToString().TrimEnd();
                this.txtEndQnProd.Text = dtrProd["fcName"].ToString().TrimEnd();
            }

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);
            pmCreateTem();
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

            DataTable dtTemIn = new DataTable("TemIn");

            dtTemIn.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dtTemIn.Columns.Add(new DataColumn("Qty", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("BalQty", Type.GetType("System.Decimal")));
            dtTemIn.Columns.Add(new DataColumn("IsBF", Type.GetType("System.String")));
            dtTemIn.Columns.Add(new DataColumn("RefNo", Type.GetType("System.String")));

            this.dtsDataEnv.Tables.Add(dtTemIn);
            dtTemIn.TableNewRow += new DataTableNewRowEventHandler(dtTemIn_TableNewRow);
            DataTable dtTemOut = new DataTable("TemOut");
            dtTemOut.Columns.Add(new DataColumn("Qty", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            //dtTemOut.Columns.Add(new DataColumn("BalQty", Type.GetType("System.Decimal")));
            dtTemOut.Columns.Add(new DataColumn("RefNoSeq", Type.GetType("System.String")));
            //dtTemOut.Columns.Add(new DataColumn("cSeq", Type.GetType("System.String")));
            this.dtsDataEnv.Tables.Add(dtTemOut);

            DataTable dtTemPrint = new DataTable("TemPrint");
            dtTemPrint.Columns.Add(new DataColumn("InQty", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("OutQty", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("Cost", Type.GetType("System.Decimal")));
            dtTemPrint.Columns.Add(new DataColumn("Amt", Type.GetType("System.Decimal")));
            this.dtsDataEnv.Tables.Add(dtTemPrint);


        }

        private void dtTemIn_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["ID"] = e.Row.Table.Rows.Count + 1;
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

            this.lblRngSect.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงแผนก", "Select Range Section" });
            this.lblFrSect.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToSect.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngJob.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงโครงการ", "Select Range Job" });
            this.lblFrJob.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToJob.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblRngProd.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วงสินค้า/วัตถุดิบ", "Select Range RM/Product" });
            this.lblFrProd.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToProd.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

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
                        this.pofrmGetPlant = new frmEMPlant(FormActiveMode.Report);
                        this.pofrmGetPlant.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPlant.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "MFGBOOK":
                    if (this.pofrmGetMfgBook == null)
                    {
                        this.pofrmGetMfgBook = new frmMfgBook(FormActiveMode.Report, this.mstrRefType);
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

                case "TXTBEGQCSECT":
                case "TXTENDQCSECT":
                    DataRow dtrSect = this.pofrmGetSect.RetrieveValue();
                    if (dtrSect != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCSECT")
                        {
                            this.txtBegQcSect.Text = dtrSect["fcCode"].ToString().TrimEnd();
                            this.txtBegQnSect.Text = dtrSect["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcSect.Text = dtrSect["fcCode"].ToString().TrimEnd();
                            this.txtEndQnSect.Text = dtrSect["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCSECT")
                        {
                            this.txtBegQcSect.Text = "";
                            this.txtBegQnSect.Text = "";
                        }
                        else
                        {
                            this.txtEndQcSect.Text = "";
                            this.txtEndQnSect.Text = "";
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
                            this.txtBegQnJob.Text = dtrJob["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcJob.Text = dtrJob["fcCode"].ToString().TrimEnd();
                            this.txtEndQnJob.Text = dtrJob["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCJOB")
                        {
                            this.txtBegQcJob.Text = "";
                            this.txtBegQnJob.Text = "";
                        }
                        else
                        {
                            this.txtEndQcJob.Text = "";
                            this.txtEndQnJob.Text = "";
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
                    this.txtBegQnSect.Text = "";
                }
                else
                {
                    this.txtEndQnSect.Text = "";
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
                    this.txtBegQnJob.Text = "";
                }
                else
                {
                    this.txtEndQnJob.Text = "";
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

            dtsPrintPreview.PMCLIST.Rows.Clear();

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strSectTab = strFMDBName + ".dbo.SECT";
            string strJobTab = strFMDBName + ".dbo.JOB";

            string strSQLStr = "";
            strSQLStr = "select ";
            strSQLStr += " MFWCLOSEHD.CROWID, MFWCLOSEHD.CCODE, MFWCLOSEHD.CREFNO, MFWCLOSEHD.DDATE, MFWCLOSEHD.CMFGPROD, MFWCLOSEHD.CBOMHD , MFWCLOSEHD.NMFGQTY ";
            strSQLStr += " ,MFWCLOSEHD.NCOSTDL, MFWCLOSEHD.NCOSTOH, MFWCLOSEHD.NCOSTADJ1, MFWCLOSEHD.NCOSTADJ2, MFWCLOSEHD.NCOSTADJ3, MFWCLOSEHD.NCOSTADJ4, MFWCLOSEHD.NCOSTADJ5 ";
            strSQLStr += " ,MFWCLOSEIT_STDOP.NT_QUEUE, MFWCLOSEIT_STDOP.NT_SETUP , MFWCLOSEIT_STDOP.NT_PROCESS, MFWCLOSEIT_STDOP.NT_TEAR, MFWCLOSEIT_STDOP.NT_WAIT, MFWCLOSEIT_STDOP.NT_MOVE ";
            strSQLStr += " ,MFWCLOSEIT_STDOP.COPSEQ, MFWCLOSEIT_STDOP.CROWID as CSTDOP ";
            strSQLStr += ",MFBOMHD.CCODE as QCBOM, MFBOMHD.CNAME as QNBOM";
            strSQLStr += ",MFSTDOPR.CCODE as QCSTDOPR, MFSTDOPR.CNAME as QNSTDOPR";
            strSQLStr += ",MFWKCTRHD.CCODE as QCWKCTR, MFWKCTRHD.CNAME as QNWKCTR";
            strSQLStr += ",MFRESOURCE.CCODE as QCRESOURCE, MFRESOURCE.CNAME as QNRESOURCE";
            strSQLStr += " from MFWCLOSEHD ";
            strSQLStr += " left join " + strSectTab + " SECT on SECT.FCSKID = MFWCLOSEHD.CSECT ";
            strSQLStr += " left join " + strJobTab + " JOB on JOB.FCSKID = MFWCLOSEHD.CJOB ";
            strSQLStr += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWCLOSEHD.CMFGPROD ";
            strSQLStr += " left join MFBOMHD on MFBOMHD.CROWID = MFWCLOSEHD.CBOMHD ";
            strSQLStr += " left join MFWCLOSEIT_STDOP on MFWCLOSEIT_STDOP.CWCLOSEH = MFWCLOSEHD.CROWID ";
            strSQLStr += " left join MFSTDOPR on MFSTDOPR.CROWID = MFWCLOSEIT_STDOP.CMOPR ";
            strSQLStr += " left join MFWKCTRHD on MFWKCTRHD.CROWID = MFWCLOSEIT_STDOP.CWKCTRH ";
            strSQLStr += " left join MFRESOURCE on MFRESOURCE.CROWID = MFWCLOSEIT_STDOP.CRESOURCE ";
            strSQLStr += " where MFWCLOSEHD.CCORP = ? ";
            strSQLStr += " and MFWCLOSEHD.CBRANCH = ? ";
            strSQLStr += " and MFWCLOSEHD.CPLANT = ? ";
            strSQLStr += " and MFWCLOSEHD.CREFTYPE = ? ";
            strSQLStr += " and MFWCLOSEHD.CMFGBOOK = ? ";
            strSQLStr += " and MFWCLOSEHD.DDATE between ? and ? ";
            strSQLStr += " and SECT.FCCODE between ? and ? ";
            strSQLStr += " and JOB.FCCODE between ? and ? ";
            strSQLStr += " and PROD.FCCODE between ? and ? ";
            strSQLStr += " order by MFWCLOSEHD.CCODE, MFWCLOSEIT_STDOP.COPSEQ";

            pobjSQLUtil2.NotUpperSQLExecString = true;
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

            DataRow dtrLastRow = null;
            string strCurrCode = "";
            int i = 0;
            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMH", QMFBOMInfo.TableName, strSQLStr, ref strErrorMsg);
            foreach (DataRow dtrBOM in this.dtsDataEnv.Tables["QBOMH"].Rows)
            {

                if (i == 0) 
                { 
                    strCurrCode = dtrBOM["cRefNo"].ToString();
                    dtrLastRow = dtrBOM;
                }
                i++;

                if (strCurrCode != dtrBOM["cRefNo"].ToString())
                {

                    this.mdecSumFGQty = this.pmSumOutputQty(dtrLastRow["cRowID"].ToString(), dtrLastRow["cMfgProd"].ToString());
                    this.pmPrintOutput(dtrLastRow, dtrLastRow["cRowID"].ToString(), dtrLastRow["cMfgProd"].ToString());
                    
                    strCurrCode = dtrBOM["cRefNo"].ToString();
                    dtrLastRow = dtrBOM;

                    dtsPrintPreview.PMCLIST.Rows.Clear();
                }

                this.pmPrintBOM_PD(dtrBOM, dtrBOM["cRowID"].ToString(), dtrBOM["cBOMHD"].ToString(), dtrBOM["cOPSEQ"].ToString().TrimEnd());
            }

            if (dtrLastRow != null)
            {
                this.mdecSumFGQty = this.pmSumOutputQty(dtrLastRow["cRowID"].ToString(), dtrLastRow["cMfgProd"].ToString());
                this.pmPrintOutput(dtrLastRow, dtrLastRow["cRowID"].ToString(), dtrLastRow["cMfgProd"].ToString());
            }

            MessageBox.Show("Finish Calculate !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.dataGridView1.DataSource = dtsPrintPreview.PMCLIST;
        }

        private decimal pmSumOutputQty(string inWCloseH, string inProd)
        {

            decimal decSumFGQty = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            string strSQLStr = "";
            strSQLStr = "select sum(NQTY) as NMFGQTY from MFWCLOSEIT_PD where CWCLOSEH = ? and CIOTYPE = 'I' and CPROD = ? ";
            pobjSQLUtil.SetPara(new object[] { inWCloseH, inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSumFG", "MFWCLOSEHD", strSQLStr, ref strErrorMsg))
            {
                DataRow dtrSumFG = this.dtsDataEnv.Tables["QSumFG"].Rows[0];
                if (!Convert.IsDBNull(dtrSumFG["nMfgQty"]))
                {
                    decSumFGQty = Convert.ToDecimal(dtrSumFG["nMfgQty"]);
                }
            }
            return decSumFGQty;
        }

        private void pmPrintOutput(DataRow inSource, string inWCloseH, string inProd)
        {

            this.mdec_MC_COSTDL = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostDL]);
            this.mdec_MC_COSTOH = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostOH]);
            this.mdec_MC_COSTADJ1 = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostAdj1]);
            this.mdec_MC_COSTADJ2 = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostAdj2]);
            this.mdec_MC_COSTADJ3 = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostAdj3]);
            this.mdec_MC_COSTADJ4 = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostAdj4]);
            this.mdec_MC_COSTADJ5 = Convert.ToDecimal(inSource[QMFWCloseHDInfo.Field.CostAdj5]);

            string strCurrOP = "";
            decimal decSumCost_RM = 0;
            DataView dv = dtsPrintPreview.PMCLIST.DefaultView;
            dv.Sort = "OP_OPSEQ";
            for (int i = 0; i < dv.Count; i++)
            {
                DataRowView dtrOPSeq = dv[i];
                if (strCurrOP != dtrOPSeq["OP_OPSEQ"].ToString())
                {
                    strCurrOP = dtrOPSeq["OP_OPSEQ"].ToString();
                }
                //decSumCost_RM = decSumCost_RM + (Convert.ToDecimal(dtrOPSeq["RM_QTY"]) * Convert.ToDecimal(dtrOPSeq["RM_COST"]));
                decSumCost_RM = decSumCost_RM + Convert.ToDecimal(dtrOPSeq["RM_COST"]);
            }

            decimal decFGCost = this.mdec_MC_COSTDL
                                            + this.mdec_MC_COSTOH
                                            + this.mdec_MC_COSTADJ1
                                            + this.mdec_MC_COSTADJ2
                                            + this.mdec_MC_COSTADJ3
                                            + this.mdec_MC_COSTADJ4
                                            + this.mdec_MC_COSTADJ5
                                            + decSumCost_RM;

            decimal decFGPrice = (this.mdecSumFGQty != 0 ? decFGCost / this.mdecSumFGQty : 0);
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            string strSQLStr = "";
            strSQLStr = "select * from MFWCLOSEIT_PD where CWCLOSEH = ? and CIOTYPE = 'I' and CPROD = ? ";
            pobjSQLUtil.SetPara(new object[] { inWCloseH, inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QUpdRef1", "MFWCLOSEHD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrSumFG in this.dtsDataEnv.Tables["QUpdRef1"].Rows)
                {
                    decimal decCostAmt = decFGPrice * Convert.ToDecimal(dtrSumFG["nQty"]);
                    pobjSQLUtil2.SetPara(new object[] { decFGPrice, decFGPrice, decCostAmt, dtrSumFG["cRef2Item"].ToString() });
                    pobjSQLUtil2.SQLExec("update RefProd set fnPrice = ? , fnPriceKe = ? , fnCostAmt = ? where fcSkid = ? ", ref strErrorMsg);
                }
            }
        
        
        }

        private void pmPrintBOM_PD(DataRow inSource, string inWCloseH, string inBOMHD, string inOPSeq)
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strUMTab = strFMDBName + ".dbo.UM";
            string strRefProdTab = strFMDBName + ".dbo.REFPROD";
            string strGLRefTab = strFMDBName + ".dbo.GLREF";

            string strSQLStr = "";
            strSQLStr = "select ";
            strSQLStr += " MFWCLOSEIT_PD.CROWID, MFWCLOSEIT_PD.NQTY, MFWCLOSEIT_PD.CPROCURE, MFWCLOSEIT_PD.COPSEQ, MFWCLOSEIT_PD.CPROD, MFWCLOSEIT_PD.CMFGBOMHD , MFWCLOSEIT_PD.CPROCURE ";
            strSQLStr += " ,PROD.FCCODE as QCPROD,PROD.FCNAME as QNPROD, PROD.FNSTDCOST as NSTDCOST ";
            strSQLStr += " ,REFPROD.FDDATE as DDATE_RF, REFPROD.FCSEQ, GLREF.FCREFNO as REFNO_GLREF, GLREF.FCSKID as FCGLREF, REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY as QTY_REFPD, REFPROD.FCSEQ as SEQ_REFPD ";
            strSQLStr += " from MFWCLOSEIT_PD ";
            strSQLStr += " left join " + strProdTab + " PROD on PROD.FCSKID = MFWCLOSEIT_PD.CPROD ";
            strSQLStr += " left join " + strRefProdTab + " REFPROD on REFPROD.FCSKID = MFWCLOSEIT_PD.CREF2ITEM ";
            strSQLStr += " left join " + strGLRefTab + " GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " where MFWCLOSEIT_PD.CWCLOSEH = ? and MFWCLOSEIT_PD.CIOTYPE = 'O' and MFWCLOSEIT_PD.COPSEQ = ? ";
            strSQLStr += " order by MFWCLOSEIT_PD.CSEQ ";

            DataRow dtrSource = dtsPrintPreview.PMCLIST.NewRow();
            dtrSource["MC_CODE"] = inSource["CCODE"].ToString().TrimEnd();
            dtrSource["MC_REFNO"] = inSource["CREFNO"].ToString().TrimEnd();
            dtrSource["MC_DATE"] = Convert.ToDateTime(inSource["DDATE"]);
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

            dtrSource["MC_MFGQTY"] = Convert.ToDecimal(inSource["NMFGQTY"]);
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
            pobjSQLUtil2.SetPara(new object[] { inWCloseH, inOPSeq });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMIT", "MFWCLOSEIT_PD", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrBOMItem in this.dtsDataEnv.Tables["QBOMIT"].Rows)
                {
                    DataRow dtrPrnData = dtsPrintPreview.PMCLIST.NewRow();

                    DataSetHelper.CopyDataRow(dtrSource, ref dtrPrnData);

                    this.pmClearCost(dtrSource);

                    dtrPrnData["RM_PROD_CODE"] = dtrBOMItem["QCPROD"].ToString().TrimEnd();
                    dtrPrnData["RM_PROD_NAME"] = dtrBOMItem["QNPROD"].ToString().TrimEnd();
                    dtrPrnData["RM_QTY"] = Convert.ToDecimal(dtrBOMItem["NQTY"]);

                    //22/2/55 
                    //if (dtrBOMItem["CPROCURE"].ToString().TrimEnd() == "M")
                    if (false)
                    {
                        this.mdecSumBOMAmt = 0;
                        this.mdecSumBOMOPAmt = 0;
                        this.mdecSumBOMAmt_Actual = 0;
                        this.mdecSumBOMOverHeadAmt = 0;

                        dtrPrnData["RM_COST"] = this.pmGetBOMCost(inWCloseH, dtrBOMItem["cMfgBOMHD"].ToString());

                        //this.pmGetBOMCost(inWCloseH, dtrBOMItem["cMfgBOMHD"].ToString());

                        //if (Convert.ToDecimal(dtrBOMItem["NQTY"]) != 0)
                        //{
                        //    decimal decRMPrice = (this.mdecSumBOMOPAmt + this.mdecSumBOMAmt_Actual + this.mdecSumBOMOverHeadAmt) / Convert.ToDecimal(dtrBOMItem["NQTY"]);
                        //    dtrPrnData["RM_COST"] = decRMPrice;
                        //}
                    }
                    else
                    {
                        DateTime dttDate = Convert.ToDateTime(inSource["dDate"]);
                        if (!Convert.IsDBNull(dtrBOMItem["dDate_RF"]))
                        {
                            dttDate = Convert.ToDateTime(dtrBOMItem["dDate_RF"]);
                        }
                        //TODO: เปลี่ยนให้รองรับ costแบบ FIFO
                        //dtrPrnData["RM_COST"] = this.pmGetAVGPrice(dtrBOMItem["cProd"].ToString(), dttDate);
                        if (App.ActiveCorp.CostMethod_Rawmat == "A")
                        {
                            dtrPrnData["RM_COST"] = this.pmGetAVGPrice(dtrBOMItem["cProd"].ToString(), dttDate);
                        }
                        else
                        {


                            string strProd = dtrBOMItem["cProd"].ToString();
                            //decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), dttDate);
                            decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), dttDate.AddDays(-1));

                            decimal decAmt = 0;
                            decimal decSedQty = 0;
                            int intSedRecn = 0;
                            decimal decSedCost = 0;
                            decimal decSedQty2 = 0;
                            string strSedRefNo = "";
                            decimal decFIFOCost = 0;

                            GLHelper.PrintBFQTYLine(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);

                            this.dtsDataEnv.Tables["TemOut"].Rows.Clear();
                            this.dtsDataEnv.Tables["TemIn"].Rows.Clear();

                            foreach (DataRow dtrBFFIFO in GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows)
                            {
                                decimal tCostAmt1 = Math.Abs(Convert.ToDecimal(dtrBFFIFO["fnCostAmt"]) + Convert.ToDecimal(dtrBFFIFO["fnCostAdj"]));
                                decimal decPrice = (Convert.ToDecimal(dtrBFFIFO["fnQty"]) != 0 ? tCostAmt1 / Convert.ToDecimal(dtrBFFIFO["fnQty"]) : 0);
                                pmAddRowIn(Convert.ToDecimal(dtrBFFIFO["fnQty"]), Convert.ToDecimal(dtrBFFIFO["fnQty"]), decPrice, dtrBFFIFO["fcRefNo"].ToString());
                            }

                            if (decStkQty == 0)
                            {
                                this.pmPrint1Prod_Add(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);
                            }
                            this.pmPrint1Prod(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);

                            string strRefProdSeq = dtrBOMItem["fcSeq"].ToString();
                            DataRow[] dtrFIFOCost = this.dtsDataEnv.Tables["TemOut"].Select("RefNoSeq = '" + dtrBOMItem["RefNo_GLREF"].ToString() + strRefProdSeq + "'");
                            decFIFOCost = 0;
                            for (int i = 0; i < dtrFIFOCost.Length; i++)
                            {
                                decFIFOCost += Convert.ToDecimal(dtrFIFOCost[i]["Amt"]);
                            }

                            dtrPrnData["RM_COST"] = decFIFOCost;

                            decimal decRefPdQty = Convert.ToDecimal(dtrBOMItem["QTY_REFPD"]);
                            decimal decPriceKe = (decRefPdQty != 0 ? decFIFOCost / decRefPdQty : 0);

                            //REFPROD.FCSEQ as SEQ_REFPD
                            //strSQLStr += " ,REFPROD.FDDATE as DDATE_RF, REFPROD.FCSEQ, GLREF.FCREFNO as REFNO_GLREF, GLREF.FCSKID as FCGLREF, REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY as QTY_REFPD, REFPROD.FCSEQ as SEQ_REFPD ";

                            pobjSQLUtil.SetPara(new object[] { dtrBOMItem["fcGLRef"].ToString(), dtrBOMItem["SEQ_REFPD"].ToString(), "I" });

                            if (dtrBOMItem["CPROCURE"].ToString().TrimEnd() == "M")
                            {
                                //2/6/2013 ตัดออก
                                //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef1", "REFPROD", "select * from REFPROD where FCGLREF = ? and FCSEQ = ? and FCIOTYPE = ?", ref strErrorMsg))
                                //{
                                //    DataRow dtrRefProd1 = this.dtsDataEnv.Tables["QRef1"].Rows[0];
                                //    pobjSQLUtil.SetPara(new object[] { decPriceKe, decPriceKe, Math.Round(decRefPdQty * decPriceKe, 4, MidpointRounding.AwayFromZero), dtrRefProd1["fcSkid"].ToString() });
                                //    pobjSQLUtil.SQLExec("update REFPROD set FNPRICE = ? , FNPRICEKE = ? , FNCOSTAMT = ? where FCSKID = ?", ref strErrorMsg);
                                //}
                            }

                            #region "2013-6-2 Old FIFO"

                            //string strProd = dtrBOMItem["cProd"].ToString();
                            ////decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), dttDate);
                            //decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), dttDate.AddDays(-1));

                            //decimal decAmt = 0;
                            //decimal decSedQty = 0;
                            //int intSedRecn = 0;
                            //decimal decSedCost = 0;
                            //decimal decSedQty2 = 0;
                            //string strSedRefNo = "";
                            //decimal decFIFOCost = 0;

                            //GLHelper.PrintBFQTYLine(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);

                            //this.dtsDataEnv.Tables["TemOut"].Rows.Clear();
                            //this.dtsDataEnv.Tables["TemIn"].Rows.Clear();

                            //foreach (DataRow dtrBFFIFO in GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows)
                            //{

                            //    decimal tCostAmt1 = Math.Abs(Convert.ToDecimal(dtrBFFIFO["fnCostAmt"]) + Convert.ToDecimal(dtrBFFIFO["fnCostAdj"]));
                            //    decimal decPrice = (Convert.ToDecimal(dtrBFFIFO["fnQty"]) != 0 ? tCostAmt1 / Convert.ToDecimal(dtrBFFIFO["fnQty"]) : 0);
                            //    pmAddRowIn(Convert.ToDecimal(dtrBFFIFO["fnQty"]), Convert.ToDecimal(dtrBFFIFO["fnQty"]), decPrice);

                            //}

                            //this.pmPrint1Prod(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);


                            //#region "รายการฝั่งจ่าย"

                            ////decimal tRefPdQty = Convert.ToDecimal(dtrBOMItem["nQty"]);  // * (dtrOrderI["cIOType"].ToString() == "I" ? 1 : -1);
                            ////decimal tAbsQty = Math.Abs(tRefPdQty);
                            ////decimal tCostAmt = 0;    //Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                            ////decimal tRefCost = 0;

                            ////decimal tAmt = 0;
                            ////decimal tAmt1 = 0;
                            ////decimal tQty1 = 0;
                            ////decimal tQty2 = 0;
                            ////decimal tCost1 = 0;
                            ////decimal tCostAdj = 0;
                            ////decimal tInAmount = 0;

                            //////รายการฝั่งจ่าย
                            ////tCostAdj = tCostAmt;
                            ////tRefCost = decSedCost;

                            ////bool tFstPSed = true;
                            ////decimal tPCost = 0;
                            ////if (decSedQty - tAbsQty >= 0)
                            ////{
                            ////    tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                            ////    tAmt = tRefCost * tAbsQty;
                            ////    //P1Line(dtrGLRef, tAbsQty, tPCost, tAmt, ref strSedRefNo);
                            ////    decFIFOCost += tAmt;
                            ////    decSedQty = decSedQty - tAbsQty;
                            ////    tFstPSed = false;
                            ////}
                            ////else
                            ////{

                            ////    decimal tAdj = 0;
                            ////    decimal tOutAmt = 0;
                            ////    decimal tOutAmt1 = 0;
                            ////    decimal tOutAmt2 = 0;
                            ////    decimal tOQty = tAbsQty;
                            ////    bool bllIsNegativeStock = false;
                            ////    if (decSedQty > 0)
                            ////    {
                            ////        while ((decSedQty - tOQty < 0) && (intSedRecn > -1))
                            ////        {
                            ////            tOQty = tOQty - decSedQty;
                            ////            tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                            ////            tAmt = tRefCost * decSedQty;
                            ////            tOutAmt1 = tAmt;
                            ////            tAdj = tCostAdj - tAmt;
                            ////            tOutAmt = tAdj + tAmt;
                            ////            //P1Line(dtrGLRef, decSedQty, tPCost, tAmt, ref strSedRefNo);
                            ////            decFIFOCost += tAmt;
                            ////            SumSed(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                            ////            tFstPSed = false;
                            ////            tRefCost = decSedCost;
                            ////            if (decSedQty == 0 && tOQty > 0 && intSedRecn > -1)
                            ////            {
                            ////                bllIsNegativeStock = true;
                            ////                tRefCost = tPCost;
                            ////                break;
                            ////            }


                            ////        }
                            ////    }
                            ////    else
                            ////    {
                            ////        //กรณี stock ติดลบ
                            ////        decimal decNegStock = decSedQty;
                            ////        SumSedNegative(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                            ////        //decSedQty = decSedQty + decNegStock;
                            ////        tRefCost = decSedCost;
                            ////    }

                            ////    if (tOQty > 0)
                            ////    {

                            ////        if (bllIsNegativeStock)
                            ////        {
                            ////            decSedQty = decSedQty - tOQty;
                            ////        }
                            ////        else
                            ////        {
                            ////            decSedQty = decSedQty - tOQty;
                            ////            decSedQty = (decSedQty > 0 ? decSedQty : 0);
                            ////        }
                            ////        //decSedQty = max( decSedQty , 0 )
                            ////        tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                            ////        tAmt = tRefCost * tOQty;
                            ////        tOutAmt2 = tOutAmt1 + tAmt;
                            ////        ////= P1Line( iif( tFstPSed , tDate , {//} ) , iif( tFstPSed , tRefNo , '' ) , tOQty , tPCost , tAmt , 'O' , .T. , @xaYokMaQty , @xaYokMaAmt , iif( tFstPSed , RefProd.fcTimeStam , '' ) , tCostAdj , tOutAmt2 , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            ////        //P1Line(dtrGLRef, tOQty, tPCost, tAmt, ref strSedRefNo);
                            ////        decFIFOCost += tAmt;
                            ////    }
                            ////}
                            ////if ((decSedQty == 0) && (intSedRecn != -1))
                            ////{
                            ////    SumSed(ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo);
                            ////}


                            //#endregion
                            //string strRefProdSeq = dtrBOMItem["fcSeq"].ToString();
                            //DataRow[] dtrFIFOCost = this.dtsDataEnv.Tables["TemOut"].Select("RefNoSeq = '" + dtrBOMItem["RefNo_GLREF"].ToString() + strRefProdSeq + "'");
                            //decFIFOCost = 0;
                            //for (int i = 0; i < dtrFIFOCost.Length; i++)
                            //{
                            //    decFIFOCost += Convert.ToDecimal(dtrFIFOCost[i]["Amt"]);
                            //}

                            //dtrPrnData["RM_COST"] = decFIFOCost;

                            //decimal decRefPdQty = Convert.ToDecimal(dtrBOMItem["QTY_REFPD"]);
                            //decimal decPriceKe = (decRefPdQty != 0 ? decFIFOCost / decRefPdQty : 0);

                            ////REFPROD.FCSEQ as SEQ_REFPD
                            ////strSQLStr += " ,REFPROD.FDDATE as DDATE_RF, REFPROD.FCSEQ, GLREF.FCREFNO as REFNO_GLREF, GLREF.FCSKID as FCGLREF, REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY as QTY_REFPD, REFPROD.FCSEQ as SEQ_REFPD ";

                            //pobjSQLUtil.SetPara(new object[] { dtrBOMItem["fcGLRef"].ToString(), dtrBOMItem["SEQ_REFPD"].ToString(), "I" });

                            //if (dtrBOMItem["CPROCURE"].ToString().TrimEnd() == "M")
                            //{
                            //    //2/6/2013 ตัดออก
                            //    //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QRef1", "REFPROD", "select * from REFPROD where FCGLREF = ? and FCSEQ = ? and FCIOTYPE = ?", ref strErrorMsg))
                            //    //{
                            //    //    DataRow dtrRefProd1 = this.dtsDataEnv.Tables["QRef1"].Rows[0];
                            //    //    pobjSQLUtil.SetPara(new object[] { decPriceKe, decPriceKe, Math.Round(decRefPdQty * decPriceKe, 4, MidpointRounding.AwayFromZero), dtrRefProd1["fcSkid"].ToString() });
                            //    //    pobjSQLUtil.SQLExec("update REFPROD set FNPRICE = ? , FNPRICEKE = ? , FNCOSTAMT = ? where FCSKID = ?", ref strErrorMsg);
                            //    //}
                            //}

                            #endregion

                        }

                    }
                    dtsPrintPreview.PMCLIST.Rows.Add(dtrPrnData);
                }
            }
            else
            {
                DataRow dtrPrnData = dtsPrintPreview.PMCLIST.NewRow();
                DataSetHelper.CopyDataRow(dtrSource, ref dtrPrnData);
                dtsPrintPreview.PMCLIST.Rows.Add(dtrPrnData);
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

        private decimal pmGetBOMCost(string inParent, string inBOMHD)
        {

            decimal decSemiPrice = 0;
            decimal decSemiAmt = 0;
            decimal decSemiQty = 0;
            decimal decOverHeadAmt = 0;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //TODO: Sum OP Cost Here

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strRefProdTab = strFMDBName + ".dbo.REFPROD";
            string strGLRefTab = strFMDBName + ".dbo.GLREF";

            string strSQLStr = "";

            strSQLStr = "select ";
            strSQLStr += " MFWCLOSEHD.CROWID, MFWCLOSEHD.CCODE, MFWCLOSEHD.CREFNO, MFWCLOSEHD.DDATE, MFWCLOSEHD.CMFGPROD, MFWCLOSEHD.CBOMHD , MFWCLOSEHD.NMFGQTY ";
            strSQLStr += " ,MFWCLOSEHD.NCOSTDL, MFWCLOSEHD.NCOSTOH, MFWCLOSEHD.NCOSTADJ1, MFWCLOSEHD.NCOSTADJ2, MFWCLOSEHD.NCOSTADJ3, MFWCLOSEHD.NCOSTADJ4, MFWCLOSEHD.NCOSTADJ5 ";
            strSQLStr += " ,MFWCLOSEIT_PD.NQTY, MFWCLOSEIT_PD.CPROD, MFWCLOSEIT_PD.CMFGBOMHD, MFWCLOSEIT_PD.CPROCURE ";
            //strSQLStr += " ,REFPROD.FDDATE as DDATE_RF ";
            //strSQLStr += " ,REFPROD.FDDATE as DDATE_RF, REFPROD.FCSEQ, GLREF.FCREFNO as REFNO_GLREF, REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY as QTY_REFPD ";
            strSQLStr += " ,REFPROD.FDDATE as DDATE_RF, REFPROD.FCSEQ, GLREF.FCREFNO as REFNO_GLREF, GLREF.FCSKID as FCGLREF, REFPROD.FCSKID as FCREFPROD, REFPROD.FNQTY as QTY_REFPD, REFPROD.FCSEQ as SEQ_REFPD ";
            strSQLStr += " from MFWCLOSEHD ";
            strSQLStr += " left join MFWCLOSEIT_PD on MFWCLOSEIT_PD.CWCLOSEH = MFWCLOSEHD.CROWID ";
            strSQLStr += " left join " + strRefProdTab + " REFPROD on REFPROD.FCSKID = MFWCLOSEIT_PD.CREF2ITEM ";
            strSQLStr += " left join " + strGLRefTab + " GLREF on GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLStr += " where MFWCLOSEHD.CPARENT = ? ";
            strSQLStr += " and MFWCLOSEHD.CBOMHD = ? ";
            strSQLStr += " and MFWCLOSEIT_PD.CIOTYPE = 'O' ";
            strSQLStr += " order by MFWCLOSEHD.CCODE, MFWCLOSEIT_PD.CSEQ";

            int intRun = 0;
            DataRow dtrBomH = null;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { inParent, inBOMHD });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QBOMI", "BOMIT", strSQLStr, ref strErrorMsg);

            #region "Print RM Item"
            foreach (DataRow dtrBOMItem in this.dtsDataEnv.Tables["QBOMI"].Rows)
            {

                if (intRun == 0)
                {

                    decSemiQty = this.pmSumOutputQty(dtrBOMItem["cRowID"].ToString(), dtrBOMItem["cMfgProd"].ToString());

                    decOverHeadAmt = Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostDL])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostOH])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostAdj1])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostAdj2])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostAdj3])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostAdj4])
                        + Convert.ToDecimal(dtrBOMItem[QMFWCloseHDInfo.Field.CostAdj5]);

                    this.mdecSumBOMOverHeadAmt += decOverHeadAmt;

                }

                intRun++;
                //inMfgQty
                decimal decMfgQty = Convert.ToDecimal(dtrBOMItem["nQty"]);
                decimal decPrice = 0;
                decimal decPrice_Actual = 0;

                if (dtrBOMItem["cProcure"].ToString() == "M")
                {
                    decSemiAmt = decSemiAmt + (decMfgQty * this.pmGetBOMCost(dtrBOMItem["cRowID"].ToString(), dtrBOMItem["cMfgBOMHD"].ToString()));
                }
                else
                {
                    DateTime dttDate = Convert.ToDateTime(dtrBOMItem["dDate"]);
                    if (!Convert.IsDBNull(dtrBOMItem["dDate_RF"]))
                    {
                        dttDate = Convert.ToDateTime(dtrBOMItem["dDate_RF"]);
                    }

                    //TODO: เปลี่ยนให้รองรับ costแบบ FIFO
                    if (App.ActiveCorp.CostMethod_Rawmat == "A")
                    {
                        decPrice_Actual = this.pmGetAVGPrice(dtrBOMItem["cProd"].ToString(), dttDate);
                    }
                    else
                    {
                        string strProd = dtrBOMItem["cProd"].ToString();
                        decimal decStkQty = this.pmPrintStock(dtrBOMItem["cProd"].ToString(), dttDate);
                        decimal decAmt = 0;
                        decimal decSedQty = 0;
                        int intSedRecn = 0;
                        decimal decSedCost = 0;
                        decimal decSedQty2 = 0;
                        string strSedRefNo = "";
                        decimal decFIFOCost = 0;

                        GLHelper.PrintBFQTYLine(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, true);

                        //DataView dv = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].DefaultView;
                        //dv.Sort = "fdDate asc";
                        //for (int i = 0; i < dv.Count; i++)
                        //{
                        //    decFIFOCost += Convert.ToDecimal(dv[i]["fnCostAmt"]) + Convert.ToDecimal(dv[i]["fnCostAdj"]);
                        //    //this.pmAddProd2(dtrStock["cProd"].ToString(), dtrStock["QcProd"].ToString(), dtrStock["QnProd"].ToString(), dtrStock["QnUM"].ToString(), Convert.ToDecimal(dv[i]["fnQty"]), Convert.ToDecimal(dv[i]["fnCostAmt"]) + Convert.ToDecimal(dv[i]["fnCostAdj"]));
                        //}
                    
                        //decPrice_Actual = (decStkQty != 0 ? decFIFOCost / decStkQty : 0);

                        this.dtsDataEnv.Tables["TemOut"].Rows.Clear();
                        this.dtsDataEnv.Tables["TemIn"].Rows.Clear();

                        foreach (DataRow dtrBFFIFO in GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows)
                        {

                            decimal tCostAmt1 = Math.Abs(Convert.ToDecimal(dtrBFFIFO["fnCostAmt"]) + Convert.ToDecimal(dtrBFFIFO["fnCostAdj"]));
                            decimal decPrice2 = (Convert.ToDecimal(dtrBFFIFO["fnQty"]) != 0 ? tCostAmt1 / Convert.ToDecimal(dtrBFFIFO["fnQty"]) : 0);
                            pmAddRowIn(Convert.ToDecimal(dtrBFFIFO["fnQty"]), Convert.ToDecimal(dtrBFFIFO["fnQty"]), decPrice2, dtrBFFIFO["fcRefNo"].ToString());

                        }

                        this.pmPrint1Prod(dtrBOMItem["cProd"].ToString(), this.txtQcBranch.Tag.ToString(), dttDate, dttDate, decStkQty, ref decAmt, ref decSedQty, ref intSedRecn, ref decSedCost, ref decSedQty2, ref strSedRefNo, false);

                        string strRefProdSeq = dtrBOMItem["fcSeq"].ToString();
                        DataRow[] dtrFIFOCost = this.dtsDataEnv.Tables["TemOut"].Select("RefNoSeq = '" + dtrBOMItem["RefNo_GLREF"].ToString() + strRefProdSeq + "'");
                        decFIFOCost = 0;
                        for (int i = 0; i < dtrFIFOCost.Length; i++)
                        {
                            decFIFOCost += Convert.ToDecimal(dtrFIFOCost[i]["Amt"]);
                        }
                        
                        decPrice_Actual = (decStkQty != 0 ? decFIFOCost / decStkQty : 0);

                        decimal decRefPdQty = Convert.ToDecimal(dtrBOMItem["QTY_REFPD"]);
                        decimal decPriceKe = (decRefPdQty != 0 ? decFIFOCost / decRefPdQty : 0);

                        pobjSQLUtil2.SetPara(new object[] { dtrBOMItem["fcGLRef"].ToString(), dtrBOMItem["SEQ_REFPD"].ToString(), "I" });
                        if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QRef1", "REFPROD", "select * from REFPROD where FCGLREF = ? and FCSEQ = ? and FCIOTYPE = ?", ref strErrorMsg))
                        {
                            DataRow dtrRefProd1 = this.dtsDataEnv.Tables["QRef1"].Rows[0];
                            pobjSQLUtil2.SetPara(new object[] { decPriceKe, decPriceKe, Math.Round(decRefPdQty * decPriceKe, 4, MidpointRounding.AwayFromZero), dtrRefProd1["fcSkid"].ToString() });
                            pobjSQLUtil2.SQLExec("update REFPROD set FNPRICE = ? , FNPRICEKE = ? , FNCOSTAMT = ? where FCSKID = ?", ref strErrorMsg);
                        }

                    }
                    decPrice = decPrice_Actual;

                    decSemiAmt = decSemiAmt + (decMfgQty * decPrice_Actual);
                    this.mdecSumBOMAmt_Actual += decMfgQty * decPrice_Actual;
                    this.mdecSumBOMAmt += decMfgQty * decPrice;
                }

            }
            #endregion

            return (decSemiQty != 0 ? (decSemiAmt + decOverHeadAmt) / decSemiQty : 0);
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

        private string pmConvertTime2Text(decimal inHour, decimal inMin, decimal inSec)
        {
            return inHour.ToString("00") + ":" + inMin.ToString("00") + ":" + inSec.ToString("00");
        }

        #region "FIFO Method"
        private void P1Line(DataRow inSource, decimal inQty, decimal inCost, decimal inAmt, ref string ioSedRefNo)
        {

            //DataRow dtrTemFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].NewRow();
            DataRow dtrTemFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].NewRow();

            dtrTemFIFO["QcBook"] = inSource["QcBook"].ToString();
            dtrTemFIFO["SedRefNo"] = ioSedRefNo;
            dtrTemFIFO["fcRfType"] = inSource["fcRfType"].ToString();
            dtrTemFIFO["fcRefType"] = inSource["fcRefType"].ToString();
            dtrTemFIFO["fcRefNo"] = inSource["fcRefNo"].ToString();
            dtrTemFIFO["fdDate"] = Convert.ToDateTime(inSource["fdDate"]);
            dtrTemFIFO["cDate"] = Convert.ToDateTime(inSource["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(inSource["fdDate"]).ToString("MMdd") + inSource["fcRefNo"].ToString() + inSource["fcSkid"].ToString();
            //dtrTemFIFO["QcPdGrp"] = inSource["QcPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp"] = inSource["QnPdGrp"].ToString();
            //dtrTemFIFO["QnPdGrp2"] = inSource["QnPdGrp2"].ToString();
            //dtrTemFIFO["QcProd"] = inSource["QcProd"].ToString();
            //dtrTemFIFO["QnProd"] = inSource["QnProd"].ToString();
            //dtrTemFIFO["QnUM"] = inSource["QnUM"].ToString();
            //dtrTemFIFO["QcSect"] = inSource["QcSect"].ToString();
            //dtrTemFIFO["QnSect"] = inSource["QnSect"].ToString();
            //dtrTemFIFO["QcJob"] = inSource["QcJob"].ToString();
            //dtrTemFIFO["QnJob"] = inSource["QnJob"].ToString();
            //dtrTemFIFO["fcIOType"] = inSource["fcIOType"].ToString();
            dtrTemFIFO["fcIOType"] = "I";
            dtrTemFIFO["fnQty"] = inQty;
            dtrTemFIFO["fnUMQty"] = 1;
            dtrTemFIFO["fnPrice"] = inCost;
            dtrTemFIFO["fnCostAmt"] = inAmt;

            //switch (inSource["fcRfType"].ToString())
            //{
            //    case "S":
            //    case "E":
            //    case "F":
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnPriceKe"]);
            //        break;
            //    default:
            //        dtrTemFIFO["fnSellPrice"] = Convert.ToDecimal(inSource["fnSellPrice"]);
            //        break;
            //}

            //if (ioSedRefNo.Trim() == string.Empty && inSource["fcIOType"].ToString().Trim() == "I")
            if (ioSedRefNo.Trim() == string.Empty)
            {
                ioSedRefNo = dtrTemFIFO["cDate"].ToString();
            }
            //this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Rows.Add(dtrTemFIFO);
            //GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Rows.Add(dtrTemFIFO);

        }

        private void SumSed(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");
            DataRow[] dtrTemBFFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Select("fcIOType = 'I' ", "nRecNo desc");
            ioSedQty = 0;
            ioSedCost = 0;
            bool tFstIn = true;
            int intCurrRecn = 0;
            if (ioSedRecn >= 0 && ioSedRecn < dtrTemBFFIFO.Length)
            {
                intCurrRecn = ioSedRecn;
                if (ioSedQty2 > 0)
                {
                    ioSedQty = ioSedQty2;
                    decimal decBFAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                    decimal decBFQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                    ioSedCost = CostSeq2(decBFAmt, decBFQty);
                    tFstIn = false;
                }
                intCurrRecn++;
            }
            ioSedQty2 = 0;
            //By Yod : 7/6/54 
            //if (ioSedQty == 0) ioSedRecn = 0;

            while (intCurrRecn < dtrTemBFFIFO.Length)
            {
                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                decimal tQty1 = QtySeq1(tCostAmt, tAbsQty);
                decimal tCost1 = CostSeq1(tCostAmt, tAbsQty);
                decimal tCost2 = CostSeq2(tCostAmt, tAbsQty);
                if (tFstIn)
                {
                    tFstIn = false;
                    ioSedCost = tCost1;
                }
                if (ioSedCost == tCost1)
                {
                    ioSedQty = ioSedQty + tQty1;
                    ioSedRecn = intCurrRecn;
                    ioSedRefNo = dtrTemBFFIFO[intCurrRecn]["cDate"].ToString();
                    if ((tCost1 != tCost2) || (Math.Abs(tQty1) != tAbsQty))
                    {
                        ioSedQty2 = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"])) - tQty1;
                        break;
                    }

                }
                else
                {
                    break;
                }
                intCurrRecn++;
            }
        }

        private void SumSedNegative(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");
            DataRow[] dtrTemBFFIFO = GLHelper.dtsDataEnv.Tables["TemBFFIFO"].Select("fcIOType = 'I' ", "nRecNo");
            //ioSedQty = 0;
            ioSedCost = 0;
            bool tFstIn = true;
            int intCurrRecn = 0;
            if (ioSedRecn >= 0 && ioSedRecn < dtrTemBFFIFO.Length)
            {
                intCurrRecn = ioSedRecn;
                if (ioSedQty2 > 0)
                {
                    ioSedQty = ioSedQty2;
                    decimal decBFAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                    decimal decBFQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                    ioSedCost = CostSeq2(decBFAmt, decBFQty);
                    tFstIn = false;
                }
                intCurrRecn++;
            }
            ioSedQty2 = 0;
            //By Yod : 7/6/54 
            if (ioSedQty == 0) intCurrRecn = intCurrRecn - 1;

            while (intCurrRecn < dtrTemBFFIFO.Length)
            {
                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                decimal tQty1 = QtySeq1(tCostAmt, tAbsQty);
                decimal tCost1 = CostSeq1(tCostAmt, tAbsQty);
                decimal tCost2 = CostSeq2(tCostAmt, tAbsQty);
                if (tFstIn)
                {
                    tFstIn = false;
                    ioSedCost = tCost1;
                }
                if (ioSedCost == tCost1)
                {
                    ioSedQty = ioSedQty + tQty1;
                    ioSedRecn = intCurrRecn;
                    ioSedRefNo = dtrTemBFFIFO[intCurrRecn]["cDate"].ToString();
                    if ((tCost1 != tCost2) || (Math.Abs(tQty1) != tAbsQty))
                    {
                        ioSedQty2 = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"])) - tQty1;
                        break;
                    }

                }
                else
                {
                    break;
                }
                intCurrRecn++;
            }
        }

        private decimal QtySeq1(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            decimal tAmt = (inCostAmt);
            decimal tCost = CostSeq1(inCostAmt, inQty);
            //decimal tQty = Math.Round(inQty, 4);
            decimal tQty = inQty;
            return tQty;
        }

        private decimal CostSeq1(decimal inCostAmt, decimal inQty)
        {
            //return Math.Round(Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt) / inQty, App.ActiveCorp.RoundPriceAt);
            return (inQty != 0 ? inCostAmt / inQty : 0);
        }

        private decimal CostSeq2(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            //decimal tCost2 = Math.Round(tAmt / inQty, App.ActiveCorp.RoundPriceAt);
            decimal tAmt = inCostAmt;
            decimal tCost2 = (inQty != 0 ? tAmt / inQty : 0);
            return tCost2;
        }


        private void pmPrint1Prod(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;
            decimal xaSedQty = ioSedQty;
            string xaSedRefNo = ioSedRefNo;
            decimal xaSedQty2 = ioSedQty2;
            decimal xaSedCost = ioSedCost;
            int xaSedRecn = ioSedRecn;
            bool tFstPSed = false;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot,RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM, Prod.fnPrice as fnSellPrice";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp , PdGrp.fcName2 as QnPdGrp2";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
            //if (this.txtTagWHouse.Text.TrimEnd() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.TrimEnd() + " )";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef  , RefProd.fcSkid ";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;


            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XABEGDATE", inBegDate.Date);
            cmd.Parameters.AddWithValue("@XAENDDATE", inEndDate.Date);

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                //while (dtrRefProd.Read() && tQty > 0)
                while (dtrRefProd.Read())
                {

                    if (VRefProd3(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()))
                    {

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        decimal tRefCost = 0;

                        decimal tAmt = 0;
                        decimal tAmt1 = 0;
                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCostAdj = 0;
                        decimal tInAmount = 0;
                        if (tRefPdQty >= 0)
                        {

                            #region "รายการฝั่งรับ"
                            //รายการฝั่งรับ
                            tAmt = Math.Round(tCostAmt, 4, MidpointRounding.AwayFromZero);
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);

                            tAmt1 = tCost1 * tQty1;
                            tCostAdj = tCostAmt;
                            tInAmount = tAmt1 + (tAmt - tAmt1);

                            //= P1Line( tDate , tRefNo , tQty1 , tCost1 , tAmt1 , 'I' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tInAmount , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            //P1Line(dtrRefProd, tQty1, tCost1, tAmt1, ref xaSedRefNo);
                            
                            //2/6/2013 ตัดออก
                            //pmAddRowIn(tAbsQty, tAbsQty, tCostAmt / tAbsQty);

                            tQty2 = tAbsQty - tQty1;

                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }
                            #endregion

                        }
                        else
                        {

                            #region "รายการฝั่งจ่าย"

                            pmCutFIFO(dtrRefProd, Convert.ToDecimal(dtrRefProd["fnQty"]), dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSeq"].ToString());
                            //P1Line(dtrRefProd, xaSedQty, tPCost, tAmt, xaSedRefNo);

                            #endregion

                        }

                    }
                }
            }

            conn.Close();

        }

        private void pmPrint1Prod_Add(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;
            decimal xaSedQty = ioSedQty;
            string xaSedRefNo = ioSedRefNo;
            decimal xaSedQty2 = ioSedQty2;
            decimal xaSedCost = ioSedCost;
            int xaSedRecn = ioSedRecn;
            bool tFstPSed = false;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot,RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM, Prod.fnPrice as fnSellPrice";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp , PdGrp.fcName2 as QnPdGrp2";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
            //if (this.txtTagWHouse.Text.TrimEnd() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.TrimEnd() + " )";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            //lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef  , RefProd.fcSkid ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;


            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XABEGDATE", inBegDate.Date);
            cmd.Parameters.AddWithValue("@XAENDDATE", inEndDate.Date);

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                //while (dtrRefProd.Read() && tQty > 0)
                while (dtrRefProd.Read())
                {

                    if (VRefProd3(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()))
                    {

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        decimal tRefCost = 0;

                        decimal tAmt = 0;
                        decimal tAmt1 = 0;
                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCostAdj = 0;
                        decimal tInAmount = 0;
                        if (tRefPdQty >= 0)
                        {

                            #region "รายการฝั่งรับ"
                            //รายการฝั่งรับ
                            tAmt = Math.Round(tCostAmt, 4, MidpointRounding.AwayFromZero);
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);

                            tAmt1 = tCost1 * tQty1;
                            tCostAdj = tCostAmt;
                            tInAmount = tAmt1 + (tAmt - tAmt1);

                            //= P1Line( tDate , tRefNo , tQty1 , tCost1 , tAmt1 , 'I' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tInAmount , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            //P1Line(dtrRefProd, tQty1, tCost1, tAmt1, ref xaSedRefNo);

                            //For Test 16/1/2013
                            pmAddRowIn(tAbsQty, tAbsQty, tCostAmt / tAbsQty, dtrRefProd["fcRefNo"].ToString());

                            tQty2 = tAbsQty - tQty1;

                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }
                            #endregion

                        }
                        else
                        {

                            #region "รายการฝั่งจ่าย"

                            //pmCutFIFO(dtrRefProd, Convert.ToDecimal(dtrRefProd["fnQty"]), dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSeq"].ToString());
                            //P1Line(dtrRefProd, xaSedQty, tPCost, tAmt, xaSedRefNo);

                            #endregion

                        }

                    }
                }
            }

            conn.Close();

        }

        private void pmAddRowIn(decimal inQty, decimal inBalQty, decimal inPrice, string inRefNo)
        {
            DataRow dtrN1 = this.dtsDataEnv.Tables["TemIn"].NewRow();
            dtrN1["Qty"] = inQty;
            dtrN1["Cost"] = inPrice;
            dtrN1["BalQty"] = inBalQty;
            dtrN1["Amt"] = inPrice * inQty;
            dtrN1["RefNo"] = inRefNo;
            this.dtsDataEnv.Tables["TemIn"].Rows.Add(dtrN1);

            //DataRow dtrN1 = this.dtsDataEnv.Tables["TemIn"].NewRow();
            //dtrN1["Qty"] = inQty;
            //dtrN1["Cost"] = inPrice;
            //dtrN1["BalQty"] = inBalQty;
            //dtrN1["Amt"] = inPrice * inQty;
            //this.dtsDataEnv.Tables["TemIn"].Rows.Add(dtrN1);
        }

        private void pmAddRowOut(decimal inQty, string inRefNoSeq, decimal inPrice)
        {
            DataRow dtrN1 = this.dtsDataEnv.Tables["TemOut"].NewRow();
            dtrN1["Qty"] = inQty;
            dtrN1["Cost"] = inPrice;
            dtrN1["RefNoSeq"] = inRefNoSeq;
            dtrN1["Amt"] = inPrice * inQty;
            this.dtsDataEnv.Tables["TemOut"].Rows.Add(dtrN1);
        }

        private void pmCutFIFO(SqlDataReader inSource, decimal inCutQty, string inRefNo)
        {
            decimal decCutQty = inCutQty;
            DataRow[] dtrInRow = this.dtsDataEnv.Tables["TemIn"].Select("BalQty > 0", "ID desc");
            for (int i = 0; i < dtrInRow.Length; i++)
            {
                decimal decBalQty = Convert.ToDecimal(dtrInRow[i]["BalQty"]);
                decimal decCost = Convert.ToDecimal(dtrInRow[i]["Cost"]);

                if (decBalQty > decCutQty)
                {
                    pmAddRowOut(decCutQty, inRefNo, decCost);
                    dtrInRow[i]["BalQty"] = decBalQty - decCutQty;
                    decCutQty = 0;
                    break;
                }
                else
                {
                    pmAddRowOut(decBalQty, inRefNo, decCost);
                    decCutQty = decCutQty - decBalQty;
                    dtrInRow[i]["BalQty"] = 0;
                }

            }

            if (decCutQty > 0)
            {
                pmAddRowOut(decCutQty, inRefNo, 0);
            }

        }

        private string xaXtraType = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;

        private bool VRefProd3(string inRfType, string inIOType)
        {
            int intSearch = xaXtraType.IndexOf(inRfType);
            return (!(intSearch > -1)
                || (inRfType == SysDef.gc_RFTYPE_ISSUE_PD && inIOType == "O")
                || (inRfType == SysDef.gc_RFTYPE_PRODUCE_PD && inIOType == "I")
                || (inRfType == SysDef.gc_RFTYPE_RETURN_ISSUE && inIOType == "I"));
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            using (Common.MRP.dlgTem2 dlg = new BeSmartMRP.Transaction.Common.MRP.dlgTem2())
            {
                dlg.grdBrowView.DataSource = dtsPrintPreview.PMCLIST;
                dlg.ShowDialog();
            }

        }

    }
}
