
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
    
    public partial class rptPCost01 : UIHelper.frmBase
    {

        public rptPCost01(string inForm)
        {
            InitializeComponent();

            this.mstrPForm = inForm.ToUpper();
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;

            string strPath = Application.StartupPath + "\\RPT\\" + this.mstrTaskName + "\\";

            if (System.IO.Directory.Exists(strPath))
            {
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

        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;
        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DatabaseForms.frmMfgBook pofrmGetMfgBook = null;
        private DialogForms.dlgGetBOM pofrmGetBOM = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;
        private DialogForms.dlgGetProd pofrmGetProd = null;

        private string mstrPForm = "FORM1";
        private string mstrTaskName = "";
        private string mstrRefType = "MO";
        private string mstrRPTFileName = "";

        private DataSet dtsDataEnv = new DataSet();

        private decimal mdecSumBOMOPAmt = 0;
        private decimal mdecSumBOMAmt = 0;

        private string mstrHTable = QMFWOrderHDInfo.TableName;
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrITable = MapTable.Table.MFWOrderIT_OP;
        private string mstrITable2 = MapTable.Table.MFWOrderIT_PD;

        Report.LocalDataSet.DTSPCOST dtsPrintPreview = new Report.LocalDataSet.DTSPCOST();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBranch.Tag = "";
            this.txtQcPlant.Tag = "";

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

            this.cmbWHCost.Properties.Items.Clear();
            this.cmbWHCost.Properties.Items.AddRange(new object[] { 
                UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน", "Standard Cost" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อล่าสุด" , "Last purchasing price" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อต่ำสุด" , "Minimum purchasing price" })
                , UIBase.GetAppUIText(new string[] { "ราคาซื้อสูงสุด" , "Maximum purchasing price" }) });

            this.cmbWHCost.SelectedIndex = 0;

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

            this.txtBegQcBOM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtBegQcBOM.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtBegQcBOM.Validating += new CancelEventHandler(txtQcBOM_Validating);
            this.txtEndQcBOM.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtEndQcBOM.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtEndQcBOM.Validating += new CancelEventHandler(txtQcBOM_Validating);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

        }

        private void pmSetFormUI()
        {
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblPlant.Text = UIBase.GetAppUIText(new string[] { "ระบุโรงงาน :", "Plant Code :" });

            this.lblRngBOM.Text = UIBase.GetAppUIText(new string[] { "เลือกช่วง BOM", "Select range BOM" });
            this.lblFrBOM.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToBOM.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            this.lblWhCost.Text = UIBase.GetAppUIText(new string[] { "ใช้ราคาทุนจาก :", "Use Cost from :" });

            this.btnPrint.Text = UIBase.GetAppUIText(new string[] { "ตกลง", "OK" });
            this.btnCancel.Text = UIBase.GetAppUIText(new string[] { "ยกเลิก", "Cancel" });
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
                case "BOM":
                    if (this.pofrmGetBOM == null)
                    {
                        this.pofrmGetBOM = new DialogForms.dlgGetBOM();
                        this.pofrmGetBOM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBOM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                //case "SECT":
                //    if (this.pofrmGetSect == null)
                //    {
                //        this.pofrmGetSect = new DialogForms.dlgGetSect();
                //        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                //    }
                //    break;

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
                case "TXTBEGQCBOM":
                case "TXTENDQCBOM":
                    this.pmInitPopUpDialog("BOM");
                    this.pofrmGetBOM.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetBOM.PopUpResult)
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
                            this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";

                        }
                    }
                    break;
                case "TXTQCPLANT":
                case "TXTQNPLANT":

                    DataRow dtrSource = this.pofrmGetPlant.RetrieveValue();
                    if (dtrSource != null)
                    {
                        this.txtQcPlant.Tag = dtrSource[QEMPlantInfo.Field.RowID].ToString();
                        this.txtQcPlant.Text = dtrSource[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                        this.txtQnPlant.Text = dtrSource[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcPlant.Tag = "";
                        this.txtQcPlant.Text = "";
                        this.txtQnPlant.Text = "";

                    }
                    break;

                case "TXTBEGQCBOM":
                case "TXTENDQCBOM":
                    DataRow dtrSect = this.pofrmGetBOM.RetrieveValue();
                    if (dtrSect != null)
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCBOM")
                        {
                            this.txtBegQcBOM.Text = dtrSect["cCode"].ToString().TrimEnd();
                            this.txtBegQnBOM.Text = dtrSect["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtEndQcBOM.Text = dtrSect["cCode"].ToString().TrimEnd();
                            this.txtEndQnBOM.Text = dtrSect["cName"].ToString().TrimEnd();
                        }
                    }
                    else
                    {
                        if ((inPopupForm.TrimEnd().ToUpper()) == "TXTBEGQCBOM")
                        {
                            this.txtBegQcBOM.Text = "";
                            this.txtBegQnBOM.Text = "";
                        }
                        else
                        {
                            this.txtEndQcBOM.Text = "";
                            this.txtEndQnBOM.Text = "";
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

        private void txtQcBOM_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCBOM")
                {
                    this.txtBegQnBOM.Text = "";
                }
                else
                {
                    this.txtEndQnBOM.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("BOM");
                e.Cancel = !this.pofrmGetBOM.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBOM.PopUpResult)
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
            strSQLStr += " where MFBOMHD.CCORP = ? and MFBOMHD.CCODE between ? and ? ";
            strSQLStr += " order by MFBOMHD.CCODE, MFBOMIT_STDOP.COPSEQ";

            pobjSQLUtil2.SetPara(new object[] { App.ActiveCorp.RowID, this.txtBegQcBOM.Text.TrimEnd(), this.txtEndQcBOM.Text.TrimEnd() });
            pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMH", QMFBOMInfo.TableName, strSQLStr, ref strErrorMsg);
            foreach (DataRow dtrBOM in this.dtsDataEnv.Tables["QBOMH"].Rows)
            {
                this.pmPrintBOM_PD(dtrBOM, dtrBOM["cRowID"].ToString(), dtrBOM["cOPSEQ"].ToString().TrimEnd());
            }
            this.pmPreviewReport(dtsPrintPreview);
        }

        private void pmPrintBOM_PD(DataRow inSource, string inBOMH, string inOPSeq)
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strProdTab = strFMDBName + ".dbo.PROD";
            string strUMTab = strFMDBName + ".dbo.UM";

            string strSQLStr = "";
            strSQLStr = "select ";
            strSQLStr += " MFBOMIT_PD.CROWID, MFBOMIT_PD.NQTY, MFBOMIT_PD.CPROCURE, MFBOMIT_PD.COPSEQ, MFBOMIT_PD.CPROD, MFBOMIT_PD.CMFGBOMHD ";
            strSQLStr += " ,PROD.FCCODE as QCPROD,PROD.FCNAME as QNPROD, PROD.FNSTDCOST as NSTDCOST ";
            strSQLStr += " from MFBOMIT_PD ";
            strSQLStr += " left join " + strProdTab + " PROD on PROD.FCSKID = MFBOMIT_PD.CPROD ";
            strSQLStr += " where MFBOMIT_PD.CBOMHD = ? and MFBOMIT_PD.COPSEQ = ? ";
            strSQLStr += " order by MFBOMIT_PD.CSEQ ";

            DataRow dtrSource = dtsPrintPreview.PCOST_BOM.NewRow();
            dtrSource["BOM_CODE"] = inSource["CCODE"].ToString().TrimEnd();
            dtrSource["BOM_NAME"] = inSource["CNAME"].ToString().TrimEnd();
            dtrSource["BOM_OUTPUT_PROD_CODE"] = "";
            dtrSource["BOM_OUTPUT_PROD_NAME"] = "";

            pobjSQLUtil.SetPara(new object[] { inSource["CMFGPROD"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QMfgProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
            {
                DataRow dtrProd = this.dtsDataEnv.Tables["QMfgProd"].Rows[0];
                dtrSource["BOM_OUTPUT_PROD_CODE"] = dtrProd["fcCode"].ToString().TrimEnd();
                dtrSource["BOM_OUTPUT_PROD_NAME"] = dtrProd["fcName"].ToString().TrimEnd();
            }

            dtrSource["BOM_OUTPUT_QTY"] = Convert.ToDecimal(inSource["NMFGQTY"]);
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
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, inSource["cStdOP"].ToString(), "O");

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
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, inSource["cStdOP"].ToString(), "W");
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
            MRPAgent.GetCostLine(pobjSQLUtil2, ref oCostLine, MapTable.Table.MFBOMIT_OP, inSource["cStdOP"].ToString(), "T");
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
            pobjSQLUtil2.SetPara(new object[] { inBOMH, inOPSeq });
            if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QBOMIT", "MFBOMIT_PD", strSQLStr, ref strErrorMsg))
            {
                //กรณี OP นั้น ๆ มีสินค้า
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
                        //this.pmGetBOMCost(0, dtrBOMItem["cMfgBOMHD"].ToString(), Convert.ToDecimal(dtrBOMItem["NQTY"]));
                        //dtrPrnData["RM_COST"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt;
                        this.pmGetBOMCost(0, dtrBOMItem["cMfgBOMHD"].ToString(), 1);
                        //dtrPrnData["RM_COST"] = (this.mdecSumBOMOPAmt + this.mdecSumBOMAmt) * Convert.ToDecimal(dtrBOMItem["NQTY"]);
                        dtrPrnData["RM_COST"] = this.mdecSumBOMOPAmt + this.mdecSumBOMAmt;
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
                    }

                    dtsPrintPreview.PCOST_BOM.Rows.Add(dtrPrnData);
                }
            }
            else
            {
                //กรณี OP นั้น ๆ ไม่มีสินค้า
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
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "");
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

            strResult += this.txtBegQcBOM.Text.TrimEnd() + " : " + this.txtBegQnBOM.Text.TrimEnd()
                                + UIBase.GetAppUIText(new string[] { "  ถึง  ", "  to  " }) + this.txtEndQcBOM.Text.TrimEnd() + " : " + this.txtEndQnBOM.Text.TrimEnd() + "|";

            strResult += this.cmbWHCost.Text + "|";

            return strResult;
        }

    }
}
