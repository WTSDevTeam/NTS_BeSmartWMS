
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;
using AppUtil;

namespace BeSmartMRP.Report
{

    public partial class XRSTCARD01 : UIHelper.frmBase
    {


        public XRSTCARD01(string inForm)
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
        private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DialogForms.dlgGetBook pofrmGetBook = null;

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

        private string mstrBr_Addr11 = "";
        private string mstrBr_Addr12 = "";

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);
            this.txtQnBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Name);

            this.txtQcBranch.Tag = "";
            this.txtQcPlant.Tag = "";
            this.txtQcBook.Tag = "";

            this.txtQcBook.Text = "";
            this.txtQnBook.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select * from BRANCH where fcCorp = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

                mstrBr_Addr11 = dtrBranch[QBranchInfo.Field.Addr1].ToString().TrimEnd();
                mstrBr_Addr12 = dtrBranch[QBranchInfo.Field.Addr2].ToString().TrimEnd();

            }

            objSQLHelper2.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPlant", "PLANT", "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " where cCorp = ? order by CCODE", ref strErrorMsg))
            {
                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant["cRowID"].ToString();
                this.txtQcPlant.Text = dtrPlant["cCode"].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant["cName"].ToString().TrimEnd();
            }

            this.pmDefaultBookCode();

            this.txtBegDate.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.txtEndDate.DateTime = DateTime.Now;

            switch (this.mstrPForm)
            {
                case "FORM10":
                case "FORM23":
                    this.pnlCoor.Visible = true;
                    this.txtBegDate.DateTime = DateTime.Now;
                    this.txtEndDate.DateTime = DateTime.Now;
                    break;
                case "FORM11":
                case "FORM12":
                    this.txtBegDate.DateTime = DateTime.Now;
                    this.txtEndDate.DateTime = DateTime.Now;
                    this.pnlBook.Visible = true;
                    break;
                case "FORM12XXX":
                    this.txtBegDate2.DateTime = DateTime.Now;
                    this.txtEndDate2.DateTime = DateTime.Now;
                    this.pnlDate.Visible = false;
                    this.pnlDate2.Visible = true;
                    this.pnlBook.Visible = true;
                    break;
                case "FORM21":
                    this.txtBegDate.DateTime = DateTime.Now;
                    this.txtEndDate.Visible = false;
                    this.pnlBook.Visible = false;
                    this.pnlCoor.Visible = false;
                    this.pnlProd.Visible = false;
                    this.lblToDate.Visible = false;
                    this.cmbIsDetail.Visible = true;
                    this.pnlDate.Location = new Point(20, 120);
                    this.cmbIsDetail.Location = new Point(180, 176);
                    //this.btnPrint.Location = new Point(180, 150);
                    //this.btnCancel.Location = new Point(250, 150);
                    
                    this.btnPrint.Location = new Point(180, 250);
                    this.btnCancel.Location = new Point(250, 250);

                    break;
                case "FORM22":
                    this.txtBegDate.DateTime = DateTime.Now;
                    this.pnlBook.Visible = false;
                    this.pnlCoor.Visible = false;
                    this.pnlProd.Visible = false;
                    this.cmbIsDetail.Visible = false;
                    this.pnlDate.Location = new Point(20, 120);
                    this.btnPrint.Location = new Point(180, 150);
                    this.btnCancel.Location = new Point(250, 150);

                    break;
            }
                
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


            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select top 1 fcSkid, fcCode, fcName from COOR where fcCorp = ? and FCISCUST = 'Y' order by FCCODE", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                this.txtBegQcCoor.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtBegQnCoor.Text = dtrValue["fcName"].ToString().TrimEnd();
            }
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select top 1 fcSkid, fcCode, fcName from COOR where fcCorp = ? and FCISCUST = 'Y'  order by FCCODE desc", ref strErrorMsg))
            {
                dtrValue = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                this.txtEndQcCoor.Text = dtrValue["fcCode"].ToString().TrimEnd();
                this.txtEndQnCoor.Text = dtrValue["fcName"].ToString().TrimEnd();
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

            if (this.mstrPForm == "FORM21")
            {
                this.cmbIsDetail.Visible = true;
                this.cmbIsDetail.Properties.Items.Clear();
                this.cmbIsDetail.Properties.Items.AddRange(new object[] { 
                UIBase.GetAppUIText(new string[] { "พิมพ์กระดาษ Slip", "พิมพ์กระดาษ Slip" })
                , UIBase.GetAppUIText(new string[] { "พิมพ์กระดาษ A4" , "พิมพ์กระดาษ A4" })});

                this.cmbIsDetail.SelectedIndex = 0;
            }

            this.pmSetRngState();

            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);

            this.pmCreateTem();

            switch (this.mstrPForm)
            {
                case "FORM7":
                    //this.lblTaskName.Text = "";
                    this.lblTopN.Visible = true;
                    this.txtTopN.Visible = true;
                    this.txtTopN.Value = 10;
                    this.cmbIsDetail.SelectedIndex = 1;
                    this.cmbIsDetail.Visible = false;
                    break;
            }
        
        }

        private void pmMapEvent()
        {
            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new CancelEventHandler(txtQcBranch_Validating);

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            this.txtQcBook.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBook.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBook.Validating += new CancelEventHandler(txtQcBook_Validating);

            this.txtBegQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);
            this.txtEndQcProd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcProd.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcProd.Validating += new CancelEventHandler(txtQcProd_Validating);


            this.txtBegQcCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcCoor.Validating += new CancelEventHandler(txtQcCoor_Validating);
            this.txtEndQcCoor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcCoor.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcCoor.Validating += new CancelEventHandler(txtQcCoor_Validating);

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

        private void pmDefaultBookCode()
        {
            string strBookID = "";

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.AppUserID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEmplR", "EMPLR", "select * from APPLOGIN where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrEmpl = this.dtsDataEnv.Tables["QEmplR"].Rows[0];
                strBookID = dtrEmpl["FCBOOK"].ToString();
            }
            
            
            string strSQLExec_Book = "select top 1 fcSkid, fcCode, fcName from Book where fcCorp = ? and fcBranch = ? and fcRefType = ? order by fcCode";
            if (strBookID == "")
            {
                objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtQcBranch.Tag.ToString(), "SR" });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", strSQLExec_Book, ref strErrorMsg))
                {
                    this.txtQcBook.Tag = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();
                    this.txtQcBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
                    this.txtQnBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcName"].ToString().TrimEnd();
                }
            }
            else
            {
                objSQLHelper.SetPara(new object[] { strBookID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select * from Book where FCSKID  = ?", ref strErrorMsg))
                {
                    this.txtQcBook.Tag = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();
                    this.txtQcBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcCode"].ToString().TrimEnd();
                    this.txtQnBook.Text = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcName"].ToString().TrimEnd();
                }
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
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetBook();
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor(CoorType.Customer);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), "SR", inPara1, (inTextbox == "TXTQCBOOK" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetBook.PopUpResult)
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
                case "TXTBEGQCCOOR":
                case "TXTENDQCCOOR":
                    this.pmInitPopUpDialog("COOR");
                    strPrefix = "FCCODE";
                    this.pofrmGetCoor.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetCoor.PopUpResult)
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
                    this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), DocumentType.MO.ToString(), this.txtQcBook.Tag.ToString(), "", strOrderBy, true);
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
                                this.pmDefaultBookCode();
                            }

                            this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

                            mstrBr_Addr11 = dtrBranch[QBranchInfo.Field.Addr1].ToString().TrimEnd();
                            mstrBr_Addr12 = dtrBranch[QBranchInfo.Field.Addr2].ToString().TrimEnd();
                        
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";

                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";

                            mstrBr_Addr11 = "";
                            mstrBr_Addr12 = "";


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
                            this.pmDefaultBookCode();
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

                        this.txtQcBook.Tag = "";
                        this.txtQcBook.Text = "";
                        this.txtQnBook.Text = "";


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



                case "TXTBEGQCCOOR":
                case "TXTENDQCCOOR":
                    DataRow dtrCoor = this.pofrmGetCoor.RetrieveValue();
                    if (dtrCoor != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCCOOR")
                        {
                            this.txtBegQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                            this.txtBegQnCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                            this.txtEndQnCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCCOOR")
                        {
                            this.txtBegQcCoor.Text = "";
                            this.txtBegQnCoor.Text = "";
                        }
                        else
                        {
                            this.txtEndQcCoor.Text = "";
                            this.txtEndQnCoor.Text = "";
                        }
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

        private void txtQcBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTQCBOOK")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTQCBOOK")
                {
                    this.txtQcBook.Tag = "";
                    this.txtQcBook.Text = "";
                    this.txtQnBook.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("BOOK");
                e.Cancel = !this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), "SR", txtPopUp.Text, strOrderBy, false);
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
                e.Cancel = !this.pofrmGetDoc.ValidateField(this.txtQcBranch.Tag.ToString(), this.txtQcPlant.Tag.ToString(), DocumentType.MO.ToString(), this.txtQcBook.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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

        private void txtQcCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "FCCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCCOOR")
                {
                    this.txtBegQnCoor.Text = "";
                }
                else
                {
                    this.txtEndQnCoor.Text = "";
                }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            switch (this.mstrPForm)
            {
                case "FORM21":
                    this.pmPrintData2();
                    break;
                case "FORM22":
                    this.pmPrintData3();
                    break;
                default:
                    this.pmPrintData();
                    break;
            }

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


            switch (this.mstrPForm)
            {
                case "FORM10":
                case "FORM11":
                case "FORM12":
                case "FORM23":
                    break;
                default:
                    this.pmCreateBFStock();
                    break;
            }

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strAppDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;

            string strProdTab = strFMDBName + ".dbo.PROD";
            string strWHouseTab = strFMDBName + ".dbo.WHOUSE";

            //Select Field
            string strSQLExec = "";

            string strFld = "GLREF.FCREFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FDDATE,(select SUM(FNQTY*FNUMQTY) from REFPROD RF where RF.FCGLREF = GLREF.FCSKID) as IQTY,GLREF.FCDISCSTR,GLREF.FNDISCAMTK,GLREF.FNAMT,GLREF.FNVATAMT,GLREF.FCVATISOUT";
            strFld = strFld + ", REFPROD.FNQTY*REFPROD.FNUMQTY AS FNQTY, REFPROD.FCIOTYPE, REFPROD.FCLOT,REFPROD.FNPRICE,REFPROD.FNDISCAMTK as FNDISCAMTK_I";
            strFld = strFld + ",GLREF.FMMEMDATA,GLREF.FMMEMDATA2,GLREF.FMMEMDATA3,GLREF.FMMEMDATA4,GLREF.FMMEMDATA5";
            //strFld = strFld + ", PROD.FCCODE AS QCPROD, PROD.FCNAME AS QNPROD ";
            strFld = strFld + ", PROD.FCCODE AS QCPROD, PROD.FCSNAME AS QNPROD ";
            strFld = strFld + ", PDGRP.FCCODE AS QCPDGRP, PDGRP.FCNAME AS QNPDGRP ";
            strFld = strFld + ", COOR.FCCODE AS QCCOOR, COOR.FCNAME AS QNCOOR ";
            strFld = strFld + ", WHOUSE.FCCODE AS QCWHOUSE, WHOUSE.FCNAME AS QNWHOUSE ";
            strFld = strFld + ", WHLOCATION.FCCODE AS QCWHLOCA, WHLOCATION.FCNAME AS QNWHLOCA ";
            strFld = strFld + ", BOOK.FCCODE AS QCBOOK,BOOK.FCNAME AS QNBOOK ";
            strFld = strFld + ", UM.FCNAME AS QNUM ";

            strSQLExec = "select " + strFld + " from REFPROD ";
            strSQLExec = strSQLExec + " inner join GLREF ON GLREF.FCSKID = REFPROD.FCGLREF ";
            strSQLExec = strSQLExec + " inner join BOOK ON BOOK.FCSKID = GLREF.FCBOOK ";
            if (this.mstrPForm == "FORM11" || this.mstrPForm == "FORM12")
            {
                if (this.txtQcBook.Text.Trim() != "")
                {
                    strSQLExec = strSQLExec + " and BOOK.FCCODE = '" + this.txtQcBook.Text.TrimEnd() + "'";
                }
            }
            strSQLExec = strSQLExec + " left join PROD ON PROD.FCSKID = REFPROD.FCPROD ";
            strSQLExec = strSQLExec + " left join PDGRP ON PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLExec = strSQLExec + " left join UM ON UM.FCSKID = REFPROD.FCUM ";
            strSQLExec = strSQLExec + " left join WHOUSE ON WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLExec = strSQLExec + " left join WHLOCATION ON WHLOCATION.FCSKID = REFPROD.FCWHLOCA ";
            strSQLExec = strSQLExec + " left join COOR ON COOR.FCSKID = GLREF.FCCOOR ";
            strSQLExec = strSQLExec + " where GLREF.FCCORP = ? AND GLREF.FCBRANCH = ? ";
            //if ()
            //"FORM12"
            if (this.mstrPForm == "FORM12XXX")
            {
                strSQLExec = strSQLExec + "AND GLREF.FTDATETIME between ? and ? ";
                strSQLExec = strSQLExec + "AND GLREF.FCRFTYPE in ('S','E','F') ";
            }
            else
            {
                strSQLExec = strSQLExec + "AND GLREF.FDDATE between ? and ? ";
            }

            strSQLExec = strSQLExec + " and GLREF.FCSTAT <> 'C' ";
            strSQLExec = strSQLExec + " and PROD.FCCODE between ? and ? ";
            //strSQLExec = strSQLExec + " and PROD.FCTYPE = ? AND PROD.FCCODE between ? and ? ";
            strSQLExec = strSQLExec + " and WHOUSE.FCTYPE = ' ' ";

            if (this.mstrPForm == "FORM10"
                || this.mstrPForm == "FORM23")
            {
                strSQLExec = strSQLExec + " and COOR.FCCODE between ? and ? ";
            }

            switch (this.mstrPForm)
            {
                case "FORM10":
                case "FORM11":
                case "FORM12":
                case "FORM23":
                    strSQLExec = strSQLExec + "AND ( GLREF.FCRFTYPE in ('S','E','F') or (GLREF.FCREFTYPE = 'GD') )";
                    break;
            }
            
            //strSQLExec = strSQLExec + " and WHOUSE.FCTYPE = ' ' " + (this.cmbWHouse.SelectedIndex == 0 ? "and WHOUSE.FCCODE between '" + this.txtBegQcWHouse.Text.TrimEnd() + "' and '" + this.txtEndQcWHouse.Text.TrimEnd() + "' " : "");
            strSQLExec = strSQLExec + " order by WHOUSE.FCCODE , PROD.FCCODE, GLREF.FDDATE, REFPROD.FCIOTYPE ";

            LocalDataSet.DTSPSTK01 dtsPrintPreview = new Report.LocalDataSet.DTSPSTK01();

            int intRun = 0;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.Timeout_Exec = 3000;
            if (this.mstrPForm == "FORM10" || this.mstrPForm == "FORM23")
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd(), this.txtBegQcCoor.Text.TrimEnd(), this.txtEndQcCoor.Text.TrimEnd() });
            }
            else if (this.mstrPForm == "FORM12XXX")
            {
                DateTime dttBeg = new DateTime(this.txtBegDate2.DateTime.Year, this.txtBegDate2.DateTime.Month, this.txtBegDate2.DateTime.Day, this.txtBegDate2.DateTime.Hour, this.txtBegDate2.DateTime.Minute, 00);
                DateTime dttEnd = new DateTime(this.txtEndDate2.DateTime.Year, this.txtEndDate2.DateTime.Month, this.txtEndDate2.DateTime.Day, this.txtEndDate2.DateTime.Hour, this.txtEndDate2.DateTime.Minute, 00);

                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, dttBeg, dttEnd, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            }
            else
            {
                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd() });
            }
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLREF", strSQLExec, ref strErrorMsg))
            {
                strCurrPrefix = "";
                decimal decBFQty = 0; decimal decBFAmt = 0;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {
                    #region "Print Detail"
                    intRun++;
                    DataRow dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                    if (strCurrPrefix != dtrGLRef["QcWHouse"].ToString() + dtrGLRef["QcProd"].ToString())
                    {
                        App.AppMessage = "กำลังพิมพ์ข้อมูล สินค้า : " + "(" + dtrGLRef["QcProd"].ToString().TrimEnd() + ")" + dtrGLRef["QnProd"].ToString().TrimEnd();
                        strCurrPrefix = dtrGLRef["QcWHouse"].ToString() + dtrGLRef["QcProd"].ToString();

                        switch (this.mstrPForm)
                        {
                            case "FORM10":
                            case "FORM11":
                            case "FORM12":
                            case "FORM23":
                                break;
                            default:
                                this.pmGetBFStock(dtrGLRef["QcWHouse"].ToString(), dtrGLRef["QcProd"].ToString(), ref decBFQty, ref decBFAmt);
                                break;
                        }
                    }

                    dtrPreview["CREFNO"] = dtrGLRef["fcRefType"].ToString() + dtrGLRef["QcBook"].ToString() + "/" + dtrGLRef["fcCode"].ToString();
                    dtrPreview["CQCBOOK"] = dtrGLRef["QcBook"].ToString().TrimEnd();
                    dtrPreview["CQNBOOK"] = dtrGLRef["QnBook"].ToString().TrimEnd();
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

                    string strRemark = (Convert.IsDBNull(dtrGLRef["fmMemData"]) ? "" : dtrGLRef["fmMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData2"]) ? "" : dtrGLRef["fmMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData3"]) ? "" : dtrGLRef["fmMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData4"]) ? "" : dtrGLRef["fmMemData4"].ToString().TrimEnd());
                    if (dtrGLRef["fmMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData5"]) ? "" : dtrGLRef["fmMemData5"].ToString().TrimEnd());

                    string strRemark_Prn = BizRule.GetMemData(strRemark, "Rem");

                    dtrPreview["DDATE"] = Convert.ToDateTime(dtrGLRef["fdDate"]);
                    //dtrPreview["CQCPDTYPE"] = "";
                    //dtrPreview["CQNPDTYPE"] = "";
                    dtrPreview["REMARK1"] = strRemark_Prn;
                    dtrPreview["REFTYPE"] = dtrGLRef["FCREFTYPE"].ToString();
                    dtrPreview["CQCPROD"] = dtrGLRef["QcProd"].ToString();
                    dtrPreview["CQNPROD"] = dtrGLRef["QnProd"].ToString();
                    dtrPreview["CQCWHOUSE"] = dtrGLRef["QcWHouse"].ToString();
                    dtrPreview["CQNWHOUSE"] = dtrGLRef["QnWHouse"].ToString();
                    dtrPreview["CLOT"] = dtrGLRef["fcLot"].ToString();
                    dtrPreview["CIOTYPE"] = dtrGLRef["fcIOType"].ToString();
                    dtrPreview["CQNUM"] = dtrGLRef["QNUM"].ToString();
                    dtrPreview["NBFQTY"] = decBFQty;
                    dtrPreview["NBFAMT"] = 0;
                    dtrPreview["NAMT_IN"] = 0;
                    dtrPreview["NAMT_OUT"] = 0;
                    dtrPreview["CQCPDGRP"] = dtrGLRef["QcPdGrp"].ToString();
                    dtrPreview["CQNPDGRP"] = dtrGLRef["QnPdGrp"].ToString();

                    dtrPreview["NQTY_IN"] = 0;
                    dtrPreview["NQTY_OUT"] = 0;

                    dtrPreview["QCWHLOCA"] = dtrGLRef["QCWHLOCA"].ToString();
                    dtrPreview["QNWHLOCA"] = dtrGLRef["QNWHLOCA"].ToString();
                    dtrPreview["AGL_01"] = Convert.ToDecimal(dtrGLRef["fnPrice"]);

                    decBFQty = 0;

                    if (!Convert.IsDBNull(dtrGLRef["QcCoor"]))
                    {
                        dtrPreview["CQCSUBCOOR"] = dtrGLRef["QcCoor"].ToString();
                        dtrPreview["CQNSUBCOOR"] = dtrGLRef["QnCoor"].ToString();
                    }

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

                    dtrPreview["FCVATISOUT"] = dtrGLRef["FCVATISOUT"].ToString();
                    dtrPreview["FNVATAMT"] = Convert.ToDecimal(dtrGLRef["FNVATAMT"]);
                    dtrPreview["FNVATAMT"] = Convert.ToDecimal(dtrGLRef["FNVATAMT"]);
                    dtrPreview["FNAMT"] = Convert.ToDecimal(dtrGLRef["FNAMT"]);

                    decimal decTotQty = Convert.ToDecimal(dtrGLRef["IQTY"]);
                    decimal decDiscAmt = Convert.ToDecimal(dtrGLRef["FNDISCAMTK"]);
                    decimal decAllocDiscAmt = 0;
                    if (decTotQty != 0)
                        decAllocDiscAmt = Math.Round(decQty / decTotQty * decDiscAmt, 4, MidpointRounding.AwayFromZero);

                    switch (this.mstrPForm)
                    {
                        case "FORM11":
                            dtrPreview["DISCAMTI"] = Convert.ToDecimal(dtrGLRef["FNDISCAMTK_I"]);
                            dtrPreview["DISCAMT"] = Convert.ToDecimal(dtrGLRef["FNDISCAMTK"]);
                            dtrPreview["DISCSTR"] = dtrGLRef["FCDISCSTR"].ToString().TrimEnd();
                            break;
                        default:
                            dtrPreview["DISCAMTI"] = decAllocDiscAmt + Convert.ToDecimal(dtrGLRef["FNDISCAMTK_I"]);
                            dtrPreview["DISCAMT"] = Convert.ToDecimal(dtrGLRef["FNDISCAMTK"]);
                            dtrPreview["DISCSTR"] = dtrGLRef["FCDISCSTR"].ToString().TrimEnd();
                            break;
                    }


                    dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);
                    #endregion
                }
            }
            //this.dataGrid1.DataSource = this.dtsDataEnv;


            switch (this.mstrPForm)
            {
                case "FORM10":
                case "FORM12":
                case "FORM23":
                    break;
                case "FORM11":
                    this.pmPrintData_PYM(dtsPrintPreview.XRPSTMOVE);
                    break;
                default:

                    //พิมพ์เฉพาะยอดที่ยกมาไม่มีเคลื่อนไหว
                    #region "Print BF"
                    DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cIsPrint = 'N' ", "cQcWHouse , cQcProd ");
                    if (dtaSel.Length > 0)
                    {

                        decimal decQty = 0;
                        decimal decAmt = 0;

                        string strCurrProd = "";
                        string strCurrWHouse = "";

                        DataRow dtrPreview = null;
                        DataRow dtrCurr = null;

                        strCurrPrefix = dtaSel[0]["cQcWHouse"].ToString() + dtaSel[0]["cQcProd"].ToString();

                        for (int intCnt = 0; intCnt < dtaSel.Length; intCnt++)
                        {

                            if (strCurrPrefix != dtaSel[intCnt]["cQcWHouse"].ToString() + dtaSel[intCnt]["cQcProd"].ToString())
                            {
                                strCurrPrefix = dtaSel[intCnt]["cQcWHouse"].ToString() + dtaSel[intCnt]["cQcProd"].ToString();

                                #region "Insert Print Row"
                                string strQnProd = "";
                                string strQnWHouse = "";

                                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, strCurrWHouse });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "GLREF", "select fcName from WHOUSE where FCCORP = ? and FCBRANCH = ? and FCCODE = ?", ref strErrorMsg))
                                {
                                    strQnWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString();
                                }

                                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, strCurrProd });
                                if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "GLREF", "select fcName from PROD where FCCORP = ? and FCCODE = ?", ref strErrorMsg))
                                {
                                    strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString();
                                }

                                App.AppMessage = "กำลังพิมพ์ข้อมูล สินค้า : " + "(" + dtaSel[intCnt]["cQcProd"].ToString() + ")" + strQnProd.TrimEnd();

                                dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                                dtrPreview["CREFNO"] = "ยอดยกมา";
                                dtrPreview["DDATE"] = this.txtBegDate.DateTime.Date;
                                //dtrPreview["CQCPDTYPE"] = "";
                                //dtrPreview["CQNPDTYPE"] = "";
                                dtrPreview["CQCPROD"] = strCurrProd;
                                dtrPreview["CQNPROD"] = strQnProd;
                                dtrPreview["CQCWHOUSE"] = strCurrWHouse;
                                dtrPreview["CQNWHOUSE"] = strQnWHouse;
                                dtrPreview["CQNUM"] = "";
                                dtrPreview["NBFQTY"] = decQty;
                                dtrPreview["NBFAMT"] = 0;
                                dtrPreview["NAMT_IN"] = 0;
                                dtrPreview["NAMT_OUT"] = 0;

                                dtrPreview["NQTY_IN"] = 0;
                                dtrPreview["NQTY_OUT"] = 0;

                                dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                                #endregion

                                decQty = 0;
                                decAmt = 0;
                                dtrCurr = null;
                            }

                            decQty += (Convert.ToDecimal(dtaSel[intCnt]["nSumQty"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                            decAmt += (Convert.ToDecimal(dtaSel[intCnt]["nSumCost"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));

                            strCurrWHouse = dtaSel[intCnt]["cQcWHouse"].ToString();
                            strCurrProd = dtaSel[intCnt]["cQcProd"].ToString();
                            dtrCurr = dtaSel[intCnt];

                        }

                        if (dtrCurr != null)
                        {
                            #region "Insert Print Row"
                            string strQnProd = "";
                            string strQnWHouse = "";

                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, strCurrWHouse });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "GLREF", "select fcName from WHOUSE where FCCORP = ? and FCBRANCH = ? and FCCODE = ?", ref strErrorMsg))
                            {
                                strQnWHouse = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcName"].ToString();
                            }

                            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, strCurrProd });
                            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "GLREF", "select fcName from PROD where FCCORP = ? and FCCODE = ?", ref strErrorMsg))
                            {
                                strQnProd = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString();
                            }

                            App.AppMessage = "กำลังพิมพ์ข้อมูล สินค้า : " + "(" + strCurrProd + ")" + strQnProd.TrimEnd();

                            dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                            dtrPreview["CREFNO"] = "ยอดยกมา";
                            dtrPreview["DDATE"] = this.txtBegDate.DateTime.Date;
                            //dtrPreview["CQCPDTYPE"] = "";
                            //dtrPreview["CQNPDTYPE"] = "";
                            dtrPreview["CQCPROD"] = strCurrProd;
                            dtrPreview["CQNPROD"] = strQnProd;
                            dtrPreview["CQCWHOUSE"] = strCurrWHouse;
                            dtrPreview["CQNWHOUSE"] = strQnWHouse;
                            dtrPreview["CQNUM"] = "";
                            dtrPreview["NBFQTY"] = decQty;
                            dtrPreview["NBFAMT"] = 0;
                            dtrPreview["NAMT_IN"] = 0;
                            dtrPreview["NAMT_OUT"] = 0;

                            dtrPreview["NQTY_IN"] = 0;
                            dtrPreview["NQTY_OUT"] = 0;

                            dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                            #endregion
                        }

                    }


                    #endregion

                    break;
            }

            if (dtsPrintPreview.XRPSTMOVE.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dtsPrintPreview.XRPSTMOVE;
                this.pmPreviewReport(dtsPrintPreview);
            }
            else
            {
                //this.EndReport();
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pmPrintData_PYM(DataTable inSource)
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


            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strAppDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;

            string strProdTab = strFMDBName + ".dbo.PROD";
            string strWHouseTab = strFMDBName + ".dbo.WHOUSE";

            //Select Field
            string strSQLExec = "";

            strSQLExec = "select 'HOLD' as CGRP ";

            strSQLExec = "select pym.CDOCOWNER,pym.CPOSPAYTYPE ";
            strSQLExec += " ,PM.CVCTYPE_CODE,PM.CCREDCARD_TYPE,PM.NAMT,PM.CREFCODE ";
            strSQLExec += " ,INV.FCCODE,INV.FCREFNO,INV.FDDATE,INV.FNTR_CHGAMT ";
            strSQLExec += " ,INV.FCREFTYPE,BK.FCCODE as QCBOOK ";
            strSQLExec += " from TRANS_PAYMENT pym ";
            strSQLExec += " left join POSPAYMENT PM on PM.CROWID = pym.CPOSPAYMENT ";
            strSQLExec += " left join GLREF INV on INV.FCSKID = pym.CREF_INV ";
            strSQLExec += " left join BOOK BK on BK.FCSKID = INV.FCBOOK ";
            strSQLExec += " where pym.CCORP = ? and pym.CBRANCH = ? ";
            strSQLExec += " and pym.CDOCOWNER = 'INV' ";
            strSQLExec += " and pym.DDATE between ? and ? ";
            strSQLExec += " and INV.FCCORP = ? and INV.FCBRANCH = ? and INV.FCRFTYPE in ('S','E','F') and INV.FCSTAT <> 'C' ";
            strSQLExec += " and INV.FCISHOLD = '' ";


            LocalDataSet.DTSSALESUM01 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM01();

            int intRun = 0;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.Timeout_Exec = 3000;

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, App.ActiveCorp.RowID, this.txtQcBranch.Tag });

            DataRow dtrPreview = null;

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef1", "GLREF", strSQLExec, ref strErrorMsg))
            {

                foreach (DataRow dtrPym in this.dtsDataEnv.Tables["QGLRef1"].Rows)
                {

                    decimal decPymAmt = 0;
                    //
                    if (dtrPym["CPOSPAYTYPE"].ToString().Trim() == "CASH")
                    {
                        decPymAmt = Convert.ToDecimal(dtrPym["NAMT"]) - Convert.ToDecimal(dtrPym["FNTR_CHGAMT"]);
                    }
                    else
                    {
                        decPymAmt = Convert.ToDecimal(dtrPym["NAMT"]);
                    }

                    //dtrPreview["CREFNO"] = dtrPym["fcRefType"].ToString() + dtrPym["QcBook"].ToString() + "/" + dtrPym["fcCode"].ToString();

                    pmAddRow_Pym(dtsPrintPreview.SALESUM22
                        , dtrPym["CPOSPAYTYPE"].ToString().Trim()
                        , dtrPym["CVCTYPE_CODE"].ToString().Trim()
                        , dtrPym["CCREDCARD_TYPE"].ToString().Trim()
                        , dtrPym["CREFCODE"].ToString().Trim()
                        , dtrPym["FCCODE"].ToString().Trim()
                        , dtrPym["FCREFNO"].ToString().Trim()
                        , Convert.ToDateTime(dtrPym["FDDATE"])
                        , decPymAmt
                        );

                }


            }

            if (dtsPrintPreview.SALESUM22.Rows.Count > 0)
            {
                //this.pmPreviewReport(dtsPrintPreview);

                var groupedData = from b in inSource.AsEnumerable()
                                  group b by b.Field<string>("CREFNO") into g
                                  let list = g.ToList()
                                  select new
                                  {
                                      FCREFNO = g.Key,
                                  };
                foreach (var row in groupedData)
                {
                    string strRefNo = row.FCREFNO;
                    DataRow[] dta1 = dtsPrintPreview.SALESUM22.Select("FCREFNO = '" + strRefNo + "'");
                    foreach (DataRow dtrPym in dta1)
                    {
                        DataRow dtrPymR = inSource.NewRow();
                        dtrPymR["CREFNO"] = strRefNo;
                        dtrPymR["GRP"] = "P";
                        dtrPymR["CPOSPAYTYPE"] = dtrPym["CPOSPAYTYPE"].ToString();
                        dtrPymR["GRP"] = "P";
                        dtrPymR["CIOTYPE"] = "O";
                        dtrPymR["REFTYPE"] = "SR";

                        dtrPymR["CCREDCARD_TYPE"] = dtrPym["CCREDCARD_TYPE"].ToString();
                        dtrPymR["CVCTYPE_CODE"] = dtrPym["CVCTYPE_CODE"].ToString();
                        dtrPymR["CCREDCARD_REFNO"] = dtrPym["CCREDCARD_REFNO"].ToString();
                        dtrPymR["Amt"] = Convert.ToDecimal(dtrPym["Amt"]);

                        inSource.Rows.Add(dtrPymR);
                    }

                }

            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pmPrintData2()
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


            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strAppDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;

            string strProdTab = strFMDBName + ".dbo.PROD";
            string strWHouseTab = strFMDBName + ".dbo.WHOUSE";

            //Select Field
            string strSQLExec = "";

            strSQLExec = "select 'HOLD' as CGRP ";
            strSQLExec += " , (select WK.FCCODE from WORKSTATION WK where WK.FCSKID = GLREF.FCWKSTAT) as QCWKSTAT ";
            strSQLExec += " ,COUNT(*) as QTY,SUM(GLREF.FNAMT) as AMT,SUM(GLREF.FNVATAMT) as VATAMT,SUM(GLREF.FNDISCAMTI) as DISCI,SUM(GLREF.FNDISCAMTK) as DISC,sum(GLREF.FNTR_CHGAMT) as CHGAMT from GLREF where ";
            strSQLExec += " GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCRFTYPE = 'S' and GLREF.FCSTAT <> 'C' ";
            strSQLExec += " and FCISHOLD = 'Y' ";
            strSQLExec += " and convert(varchar,FDDATE,112) = ? ";
            //strSQLExec += " --and GLREF.FCPOSS_SESS = '' ";
            strSQLExec += " group by GLREF.FCWKSTAT ";
            strSQLExec += " union ";
            strSQLExec += " select 'VOID' as CGRP  ";
            strSQLExec += " , (select WK.FCCODE from WORKSTATION WK where WK.FCSKID = GLREF.FCWKSTAT) as QCWKSTAT ";
            strSQLExec += " ,COUNT(*) as QTY,SUM(GLREF.FNAMT) as AMT,SUM(GLREF.FNVATAMT) as VATAMT,SUM(GLREF.FNDISCAMTI) as DISCI,SUM(GLREF.FNDISCAMTK) as DISC,sum(GLREF.FNTR_CHGAMT) as CHGAMT from GLREF where ";
            strSQLExec += " GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCRFTYPE = 'S' and FCSTAT = 'C' ";
            strSQLExec += " and convert(varchar,FDDATE,112) = ? ";
            //strSQLExec += " --and GLREF.FCPOSS_SESS = '' ";
            strSQLExec += " group by GLREF.FCWKSTAT ";
            strSQLExec += " union ";
            strSQLExec += " select 'SALE' as CGRP ";
            strSQLExec += " , (select WK.FCCODE from WORKSTATION WK where WK.FCSKID = GLREF.FCWKSTAT) as QCWKSTAT ";
            strSQLExec += " ,COUNT(*) as QTY,SUM(GLREF.FNAMT) as AMT,SUM(GLREF.FNVATAMT) as VATAMT,SUM(GLREF.FNDISCAMTI) as DISCI,SUM(GLREF.FNDISCAMTK) as DISC,sum(GLREF.FNTR_CHGAMT) as CHGAMT from GLREF where ";
            strSQLExec += " GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCRFTYPE = 'S' and GLREF.FCSTAT <> 'C' ";
            strSQLExec += " and FCISHOLD = '' ";
            strSQLExec += " and convert(varchar,FDDATE,112) = ? ";
            //strSQLExec += " --and GLREF.FCPOSS_SESS = '' ";
            strSQLExec += " group by GLREF.FCWKSTAT ";
            strSQLExec += " union ";
            strSQLExec += " select 'CR' as CGRP ";
            strSQLExec += " , (select WK.FCCODE from WORKSTATION WK where WK.FCSKID = GLREF.FCWKSTAT) as QCWKSTAT ";
            strSQLExec += " ,COUNT(*) as QTY,SUM(GLREF.FNAMT) as AMT,SUM(GLREF.FNVATAMT) as VATAMT,SUM(GLREF.FNDISCAMTI) as DISCI,SUM(GLREF.FNDISCAMTK) as DISC,sum(GLREF.FNTR_CHGAMT) as CHGAMT from GLREF where ";
            strSQLExec += " GLREF.FCCORP = ? and GLREF.FCBRANCH = ? and GLREF.FCRFTYPE = 'E' and GLREF.FCSTAT <> 'C' ";
            strSQLExec += " and FCISHOLD = '' ";
            strSQLExec += " and convert(varchar,FDDATE,112) = ? ";
            //strSQLExec += " --and GLREF.FCPOSS_SESS = '' ";
            strSQLExec += " group by GLREF.FCWKSTAT ";

            LocalDataSet.DTSSALESUM01 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM01();

            int intRun = 0;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.Timeout_Exec = 3000;

            string strPDate = this.txtBegDate.DateTime.Date.Year.ToString("0000") + this.txtBegDate.DateTime.Date.ToString("MMdd");

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate
                                                                ,App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate
                                                                ,App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate
                                                                ,App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate });

            DataRow dtrPreview = null;

            decimal decQty_Sum = 0; decimal decAmt_Sum = 0; decimal decVatAmt_Sum = 0; decimal decChgAmt = 0;
            decimal decDiscAmt_Sum = 0; decimal decDiscAmtI_Sum = 0;decimal decDiscAmtK_Sum = 0;
            decimal decQty = 0; decimal decAmt = 0; decimal decVatAmt = 0; decimal decDiscAmt = 0; decimal decDiscAmtI = 0;

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef1", "GLREF", strSQLExec, ref strErrorMsg))
            {


                //ยกเลิกบิล
                //พักบิล
                //ยอดคืนสินค้า
                //ยอดขาย
                //รวมเป็นเงิน
                
                //ยอดขายเงินสด
                //ยอดคืนเงินสด
                //ยอดรวมเงินสด
                //ยอดขายบัตรเครดิต
                //ยอดคืนบัตรเครดิต
                //ยอดรวมบัตรเครดิต
                //ยอดขาย Voucher
                //ยอดคืน Voucher
                //ยอดรวม Voucher
                //ยอดรวมส่วนลด (บิลขาย)
                //ยอดรวมส่วนลด (บิลคืน)
                //ส่วนลดท้ายบิล
                //Total Amount
                //VAT
                //The amount before taxes

                this.pmSumINVAmt("VOID", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt, ref decVatAmt, ref decDiscAmt, ref decDiscAmtI, ref decChgAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "01","01", "INV_VOID", "ยกเลิกบิล", decQty, 0);
                //decQty_Sum += (decQty * -1); decAmt_Sum += (decAmt * -1);

                this.pmSumINVAmt("HOLD", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt, ref decVatAmt, ref decDiscAmt, ref decDiscAmtI, ref decChgAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "01","02", "INV_HOLD", "พักบิล", decQty, 0);
                //decQty_Sum += (decQty * -1); decAmt_Sum += (decAmt * -1);

                decQty = 0; decAmt = 0; decVatAmt = 0; decDiscAmt = 0; decDiscAmtI = 0;
                pmAddRow(dtsPrintPreview.SALESUM21, "01","03", "INV_CR", "ยอดคืนสินค้า", decQty, decAmt);
                decQty_Sum += (decQty * -1); decAmt_Sum += (decAmt * -1);

                this.pmSumINVAmt("SALE", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt, ref decVatAmt, ref decDiscAmt, ref decDiscAmtI, ref decChgAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "01","04", "INV_SALE", "ยอดขาย", decQty, decAmt + decVatAmt);
                decQty_Sum += (decQty * 1); decAmt_Sum += ((decAmt + decVatAmt) * 1);
                //decQty_Sum += (decQty * -1); decAmt_Sum += (decAmt * -1);

                pmAddRow(dtsPrintPreview.SALESUM21, "09","16", "SUM_DISCI", "ยอดรวมส่วนลด (บิลขาย)", 0, decDiscAmtI);
                pmAddRow(dtsPrintPreview.SALESUM21, "09","17", "SUM_DISCI_CR", "ยอดรวมส่วนลด (บิลคืน)", 0, 0);
                pmAddRow(dtsPrintPreview.SALESUM21, "10","18", "SUM_DISC", "ส่วนลดท้ายบิล", 0, decDiscAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "12", "20", "TOT_VATAMT", "VAT", 0, decVatAmt);
                
                //pmAddRow(dtsPrintPreview.SALESUM21, "10", "19", "TOT_RETAMT", "VAT", 0, decVatAmt);
                decDiscAmtK_Sum = decDiscAmt;
                //รวมเป็นเงิน	
                //dtrPreview = dtsPrintPreview.SALESUM21.NewRow();
                //decQty = 0; decAmt = 0; decVatAmt = 0; decDiscAmt = 0; decDiscAmtI = 0;
                pmAddRow(dtsPrintPreview.SALESUM21, "02","05", "INV_TOT", "รวมเป็นเงิน", 0, decAmt_Sum);
                pmAddRow(dtsPrintPreview.SALESUM21, "12","21", "TOT_BTAX", "The amount before taxes", 0, decAmt_Sum - decVatAmt);

                //Total Amount	TOT_AMT
                //The amount before taxes	TOT_BTAX


            }

            strSQLExec = "select pym.CDOCOWNER,pym.CPOSPAYTYPE,count(*) as Qty,SUM(pym.NPAYAMT) as PYM_AMT ";
            strSQLExec += " from TRANS_PAYMENT pym ";
            strSQLExec += " left join GLREF on GLREF.FCSKID = pym.CREF_INV ";
            strSQLExec += " where pym.CCORP = ? and pym.CBRANCH = ? ";
            strSQLExec += " and pym.CDOCOWNER = 'INV' ";
            strSQLExec += " and convert(varchar,pym.DDATE,112) = ? ";
            strSQLExec += " and GLREF.FCSTAT <> 'C' and GLREF.FCISHOLD <> 'Y' ";
            strSQLExec += " group by pym.CDOCOWNER,pym.CPOSPAYTYPE ";
            strSQLExec += " union ";
            strSQLExec += " select pym.CDOCOWNER,pym.CPOSPAYTYPE,count(*) as Qty,SUM(pym.NPAYAMT) as PYM_AMT  ";
            strSQLExec += " from TRANS_PAYMENT pym ";
            strSQLExec += " left join GLREF on GLREF.FCSKID = pym.CREF_INV ";
            strSQLExec += " where pym.CCORP = ? and pym.CBRANCH = ? ";
            strSQLExec += " and pym.CDOCOWNER = 'INV_CR' ";
            strSQLExec += " and convert(varchar,pym.DDATE,112) = ? ";
            strSQLExec += " and GLREF.FCSTAT <> 'C' and GLREF.FCISHOLD <> 'Y' ";
            strSQLExec += " group by pym.CDOCOWNER,pym.CPOSPAYTYPE ";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate
                                                                ,App.ActiveCorp.RowID, this.txtQcBranch.Tag, strPDate });

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef1", "GLREF", strSQLExec, ref strErrorMsg))
            {
                decimal decSumAmt = 0;
                this.pmSumINVPymAmt("INV", "CASH", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "03","06", "PYM_CASH", "ยอดขายเงินสด", 0, decAmt);
                decSumAmt += decAmt;

                pmAddRow(dtsPrintPreview.SALESUM21, "04", "09", "PYM_CASH", "ยอดรวมเงินสด", 0, decAmt - decChgAmt);

                this.pmSumINVPymAmt("INV_CR", "CASH", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "03","07", "PYM_CASH_CR", "ยอดคืนเงินสด", 0, decAmt);
                decSumAmt += (decAmt * -1);

                pmAddRow(dtsPrintPreview.SALESUM21, "03", "08", "TOT_CHGAMT", "เงินทอน", 0, decChgAmt);
                decSumAmt += (decChgAmt * -1);

                this.pmSumINVPymAmt("INV", "CREDIT", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "05","10", "PYM_CRED", "ยอดขายบัตรเครดิต", 0, decAmt);
                decSumAmt += (decAmt * 1);

                pmAddRow(dtsPrintPreview.SALESUM21, "06", "12", "PYM_CRED_TOT", "ยอดรวมบัตรเครดิต", 0, decAmt);

                this.pmSumINVPymAmt("INV_R", "CREDIT", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "05", "11", "PYM_CRED", "ยอดคืนบัตรเครดิต", 0, decAmt);
                decSumAmt += (decAmt * -1);

                ///
                this.pmSumINVPymAmt("INV", "VOUCHER", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "07", "13", "PYM_VC", "ยอดขาย Voucher", 0, decAmt);
                decSumAmt += (decAmt * 1);

                pmAddRow(dtsPrintPreview.SALESUM21, "08", "15", "PYM_VC_TOT", "ยอดรวม Voucher", 0, decAmt);

                this.pmSumINVPymAmt("INV_R", "VOUCHER", this.dtsDataEnv.Tables["QGLRef1"], ref decQty, ref decAmt);
                pmAddRow(dtsPrintPreview.SALESUM21, "07", "14", "PYM_VC_CR", "ยอดคืน Voucher", 0, decAmt);
                decSumAmt += (decAmt * -1);

                pmAddRow(dtsPrintPreview.SALESUM21, "11","19", "TOT_AMT", "Total Amount", 0, decSumAmt);


            }

            if (dtsPrintPreview.SALESUM21.Rows.Count > 0)
            {
                this.pmPreviewReport(dtsPrintPreview);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pmAddRow(DataTable inSource, string inSeq_Grp, string inSeq, string inPara, string inDesc, decimal inQty, decimal inAmt)
        {
            DataRow dtrPreview = inSource.NewRow();
            dtrPreview["SEQ_GRP"] = inSeq_Grp;
            dtrPreview["SEQ"] = inSeq;
            dtrPreview["PARA"] = inPara;
            dtrPreview["DESC"] = inDesc;
            dtrPreview["Qty"] = inQty;
            dtrPreview["Amt"] = inAmt;
            inSource.Rows.Add(dtrPreview);

        }

        private void pmSumINVAmt(string inPara, DataTable inSource, ref decimal decQty, ref decimal decAmt, ref decimal decVatAmt, ref decimal decDiscAmt, ref decimal decDiscAmtI, ref decimal decChgAmt)
        {

            decQty = 0;
            decAmt = 0;
            decVatAmt = 0;
            decDiscAmt = 0;
            decDiscAmtI = 0;
            decChgAmt = 0;

            decQty = (from p1 in inSource.AsEnumerable()
                      where p1["CGRP"].ToString() == inPara
                      select new { Amt = p1["QTY"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decAmt = (from p1 in inSource.AsEnumerable()
                      where p1["CGRP"].ToString() == inPara
                      select new { Amt = p1["AMT"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decVatAmt = (from p1 in inSource.AsEnumerable()
                         where p1["CGRP"].ToString() == inPara
                         select new { Amt = p1["VATAMT"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decDiscAmtI = (from p1 in inSource.AsEnumerable()
                           where p1["CGRP"].ToString() == inPara
                           select new { Amt = p1["DISCI"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decDiscAmt = (from p1 in inSource.AsEnumerable()
                          where p1["CGRP"].ToString() == inPara
                          select new { Amt = p1["DISC"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decChgAmt = (from p1 in inSource.AsEnumerable()
                         where p1["CGRP"].ToString() == inPara
                         select new { Amt = p1["CHGAMT"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

        }

        private void pmSumINVPymAmt(string inPara, string inPaymentType,DataTable inSource, ref decimal decQty, ref decimal decAmt)
        {

            decQty = 0;
            decAmt = 0;

            decQty = (from p1 in inSource.AsEnumerable()
                      where p1["CDOCOWNER"].ToString().Trim() == inPara && p1["CPOSPAYTYPE"].ToString().Trim() == inPaymentType
                      select new { Amt = p1["QTY"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

            decAmt = (from p1 in inSource.AsEnumerable()
                      where p1["CDOCOWNER"].ToString().Trim() == inPara && p1["CPOSPAYTYPE"].ToString().Trim() == inPaymentType
                      select new { Amt = p1["PYM_AMT"] }).Sum(p1 => Convert.ToDecimal(p1.Amt));

        }

        private void pmPrintData3()
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


            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strAppDBName = App.ConfigurationManager.ConnectionInfo.DBMSName;

            string strProdTab = strFMDBName + ".dbo.PROD";
            string strWHouseTab = strFMDBName + ".dbo.WHOUSE";

            //Select Field
            string strSQLExec = "";

            strSQLExec = "select 'HOLD' as CGRP ";

            strSQLExec = "select pym.CDOCOWNER,pym.CPOSPAYTYPE ";
            strSQLExec += " ,PM.CVCTYPE_CODE,PM.CCREDCARD_TYPE,PM.NAMT,PM.CREFCODE ";
            strSQLExec += " ,INV.FCCODE,INV.FCREFNO,INV.FDDATE,INV.FNTR_CHGAMT ";

            strSQLExec += " ,(select sum(PM2.NAMT) from TRANS_PAYMENT p2";
            strSQLExec += " left join POSPAYMENT PM2 on PM2.CROWID = p2.CPOSPAYMENT ";
            strSQLExec += " where INV.FCSKID = p2.CREF_INV) as SumPymAmt";

            strSQLExec += " from TRANS_PAYMENT pym ";
            strSQLExec += " left join POSPAYMENT PM on PM.CROWID = pym.CPOSPAYMENT ";
            strSQLExec += " left join GLREF INV on INV.FCSKID = pym.CREF_INV ";
            strSQLExec += " where pym.CCORP = ? and pym.CBRANCH = ? ";
            strSQLExec += " and pym.CDOCOWNER = 'INV' ";
            strSQLExec += " and pym.DDATE between ? and ? ";
            strSQLExec += " and INV.FCCORP = ? and INV.FCBRANCH = ? and INV.FCRFTYPE in ('S','E','F') and INV.FCSTAT <> 'C' ";
            strSQLExec += " and INV.FCISHOLD = '' ";


            LocalDataSet.DTSSALESUM01 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM01();

            int intRun = 0;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.Timeout_Exec = 30000;

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, App.ActiveCorp.RowID, this.txtQcBranch.Tag });

            DataRow dtrPreview = null;

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QGLRef1", "GLREF", strSQLExec, ref strErrorMsg))
            {

                foreach (DataRow dtrPym in this.dtsDataEnv.Tables["QGLRef1"].Rows)
                {

                    decimal decPymAmt = 0;
                    //
                    if (dtrPym["CPOSPAYTYPE"].ToString().Trim() == "CASH")
                    {
                        //decPymAmt = Convert.ToDecimal(dtrPym["NAMT"]) - Convert.ToDecimal(dtrPym["FNTR_CHGAMT"]);
                        decimal decSumPymAmt = Convert.ToDecimal(dtrPym["SumPymAmt"]);
                        decPymAmt = Convert.ToDecimal(dtrPym["NAMT"]) - Math.Round(Convert.ToDecimal(dtrPym["NAMT"]) / decSumPymAmt * Convert.ToDecimal(dtrPym["FNTR_CHGAMT"]), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        decPymAmt = Convert.ToDecimal(dtrPym["NAMT"]);
                    }
                    pmAddRow_Pym(dtsPrintPreview.SALESUM22
                        , dtrPym["CPOSPAYTYPE"].ToString().Trim()
                        , dtrPym["CVCTYPE_CODE"].ToString().Trim()
                        , dtrPym["CCREDCARD_TYPE"].ToString().Trim()
                        , dtrPym["CREFCODE"].ToString().Trim()
                        , dtrPym["FCCODE"].ToString().Trim()
                        , dtrPym["FCREFNO"].ToString().Trim()
                        , Convert.ToDateTime(dtrPym["FDDATE"])
                        , decPymAmt
                        );

                }


            }

            if (dtsPrintPreview.SALESUM22.Rows.Count > 0)
            {
                this.pmPreviewReport(dtsPrintPreview);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pmAddRow_Pym(DataTable inSource, string inPayType, string inVCCode, string inCardType, string inCardCode, string inCode, string inRefNo, DateTime inDate, decimal inAmt)
        {

            string strUpdCardCode = "";
            if (inCardCode.Length > 4)
            {
                strUpdCardCode = "XXXXX" + AppUtil.StringHelper.Right(inCardCode, 4);
            }
            else
            {
                if (inCardCode.Length > 2)
                {
                    strUpdCardCode = "XXXXX" + AppUtil.StringHelper.Right(inCardCode, 2);
                }
                else
                {
                    strUpdCardCode = inCardCode;
                }
            }
            DataRow dtrPreview = inSource.NewRow();
            dtrPreview["CPOSPAYTYPE"] = inPayType;
            dtrPreview["CVCTYPE_CODE"] = inVCCode;
            dtrPreview["CCREDCARD_TYPE"] = inCardType;
            dtrPreview["CCREDCARD_REFNO"] = strUpdCardCode;
            dtrPreview["FCCODE"] = inCode;
            dtrPreview["FCREFNO"] = inRefNo;
            dtrPreview["FDDATE"] = inDate;
            dtrPreview["Amt"] = inAmt;
            inSource.Rows.Add(dtrPreview);
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
            strFld += " ,PDGRP.FCCODE as CQCPDGRP";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            //strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' " + (this.cmbWHouse.SelectedIndex == 0 ? "and WHOUSE.FCCODE between '" + this.txtBegQcWHouse.Text.TrimEnd() + "' and '" + this.txtEndQcWHouse.Text.TrimEnd() + "' " : "");
            //strSQLStr = strSQLStr + " and PROD.FCTYPE = ? and PROD.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " and PROD.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " group by WHOUSE.FCCODE , PROD.FCCODE , PDGRP.FCCODE,REFPROD.FCIOTYPE";

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

            string strRPTFileName = "";

            switch (this.mstrPForm)
            {
                case "FORM1":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD01.RPT";
                    break;
                case "FORM2":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD02.RPT";
                    break;
                case "FORM4":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD04.RPT";
                    break;
                case "FORM5":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD05.RPT";
                    break;
                case "FORM6":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD06.RPT";
                    break;
                case "FORM7":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE01.RPT";
                    break;
                case "FORM8":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE02.RPT";
                    break;
                case "FORM9":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE03.RPT";
                    break;
                case "FORM10":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE04.RPT";
                    //strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE04-02.RPT";
                    break;
                case "FORM11":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE05.RPT";
                    break;
                case "FORM12":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE06.RPT";
                    break;
                case "FORM21":
                    if (this.cmbIsDetail.SelectedIndex == 0)
                        strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE21.RPT";
                    else
                        strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE21_A4.RPT";

                    break;
                case "FORM22":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE22.RPT";
                    break;
                case "FORM23":
                    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTSALE04-02.RPT";
                    break;
            }

            //if (this.mstrPForm == "FORM1")
            //{
            //    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD01.RPT";
            //}
            //else
            //{
            //    strRPTFileName = Application.StartupPath + "\\RPT\\XRPSTCARD02.RPT";
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
            //switch (this.mstrPForm)
            //{
            //    case "FORM21":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQnBranch.Text.TrimEnd() + "\r\n" + mstrBr_Addr11 + "\r\n" + mstrBr_Addr12);
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ปิดรอบวันที่ " + this.txtBegDate.DateTime.ToString("dd/MM/yy"));
            //        break;
            //    case "FORM22":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์ตั้งแต่วันที่ " + this.txtBegDate.DateTime.ToString("dd/MM/yy") + " ถึง " + this.txtEndDate.DateTime.ToString("dd/MM/yy"));
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQnBranch.Text.TrimEnd() + "\r\n" + mstrBr_Addr11 + "\r\n" + mstrBr_Addr12);
            //        break;
            //    default:
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์สินค้ารหัสตั้งแต่ " + this.txtBegQcProd.Text.TrimEnd() + " ถึง " + this.txtEndQcProd.Text.TrimEnd());
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์ตั้งแต่วันที่ " + this.txtBegDate.DateTime.ToString("dd/MM/yy") + " ถึง " + this.txtEndDate.DateTime.ToString("dd/MM/yy"));
            //        break;
            //}

            //if (this.mstrPForm == "FORM7")
            //{
            //    AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "N");
            //    AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtTopN.Value);
            //}
            //else
            //{
            //    AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbIsDetail.SelectedIndex == 0 ? "Y" : "N");
            //}
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "Y");

            //int i = 0;
            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["pfShowDetail"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //if (this.mstrPForm == "FORM7")
            //{
            //    prmCRPara["ffTopN"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //}

            //switch (this.mstrPForm)
            //{
            //    case "FORM10":
            //    case "FORM11":
            //    case "FORM21":
            //    case "FORM22":
            //    case "FORM23":
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQcBranch.Text.Trim());
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQnBranch.Text.TrimEnd());
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrBr_Addr11);
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrBr_Addr12);
            //        AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.TaxID);

            //        prmCRPara["pfQcBranch"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //        prmCRPara["pfQnBranch"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //        prmCRPara["pfBrAddr1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //        prmCRPara["pfBrAddr2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //        prmCRPara["pfTaxID"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);

            //        break;
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
            strResult += this.cmbIsDetail.Text.TrimEnd() + "|";
            strResult += this.txtBegDate.DateTime.ToString("dd/MM/yy") + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtEndDate.DateTime.ToString("dd/MM/yy") + "|";
            strResult += this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtBegQnProd.Text.TrimEnd()
                                + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtBegQcProd.Text.TrimEnd() + " : " + this.txtEndQnProd.Text.TrimEnd() + "|";

            //strResult += this.cmbWHCost.Text.TrimEnd() + "|";

            return strResult;
        }

    }
}
