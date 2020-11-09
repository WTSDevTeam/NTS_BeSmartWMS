
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

namespace BeSmartMRP.Report
{
    public partial class frmPBGStat : UIHelper.frmBase
    {

        public frmPBGStat()
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

        private string mstrForm = "";
        private string mstrTaskName = "";
        private int mintYear = 0;
        private DataSet dtsDataEnv = new DataSet();
        private string mstrBranch = "";

        System.Data.OleDb.OleDbConnection conn = null;
        System.Data.OleDb.OleDbConnection conn2 = null;

        private DatabaseForms.frmBranch pofrmGetBranch = null;
        private UIHelper.IfrmDBBase pofrmGetSect = null;
        private UIHelper.IfrmDBBase pofrmGetBGYear = null;
        private UIHelper.IfrmDBBase pofrmGetJob = null;

        private void pmInitForm()
        {

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcSect.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QEMSectInfo.TableName, QEMSectInfo.Field.Code);
            this.txtQnSect.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QEMSectInfo.TableName, QEMSectInfo.Field.Name);
            this.txtYear.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, QBGYearInfo.TableName, QBGYearInfo.Field.Code);

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "EMBRANCH", "select cRowID, cCode, cName from EMBRANCH order by CCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.mstrBranch = dtrBranch["cRowID"].ToString();
                //this.txtQcBranch.Tag = dtrBranch["cRowID"].ToString();
                //this.txtQcBranch.Text = dtrBranch["cCode"].ToString().TrimEnd();
                //this.txtQnBranch.Text = dtrBranch["cName"].ToString().TrimEnd();

            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select EMSECT.CROWID, EMSECT.CCODE, EMSECT.CNAME from EMSECT where CCORP = ? by cCode", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QSect"].Rows[0];
                this.txtQcSect.Tag = dtrBook["CROWID"].ToString();
                this.txtQcSect.Text = dtrBook["cCode"].ToString().TrimEnd();
                this.txtQnSect.Text = dtrBook["cName"].ToString().TrimEnd();

            }

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGYear", "BGYEAR", "select * from BGYEAR where CCORP = ? order by NYEAR desc", ref strErrorMsg))
            {
                DataRow dtrBGYear = this.dtsDataEnv.Tables["QBGYear"].Rows[0];
                this.txtYear.Text = dtrBGYear["cCode"].ToString().TrimEnd();
                this.mintYear = Convert.ToInt32(dtrBGYear["nYear"]);
            }

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new frmBranch(FormActiveMode.PopUp);
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMSECT":
                    if (this.pofrmGetSect == null)
                    {
                        this.pofrmGetSect = new frmEMSect(FormActiveMode.PopUp);
                        //this.pofrmGetSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BGYEAR":
                    if (this.pofrmGetBGYear == null)
                    {
                        this.pofrmGetBGYear = new frmBGYear(FormActiveMode.PopUp);
                        //this.pofrmGetBGYear.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBGYear.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMJOB":
                    if (this.pofrmGetJob == null)
                    {
                        this.pofrmGetJob = new frmEMJob(UIHelper.FormActiveMode.Report);
                        //this.pofrmGetJob.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetJob.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
            switch (inTextbox)
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    this.pmInitPopUpDialog("BRANCH");
                    this.pofrmGetBranch.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCSECT":
                case "TXTQNSECT":
                    this.pmInitPopUpDialog("EMSECT");
                    string strPrefix = (inTextbox == "TXTQCSECT" ? "CCODE" : "CNAME");
                    this.pofrmGetSect.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetSect.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTYEAR":
                    this.pmInitPopUpDialog("BGYEAR");
                    this.pofrmGetBGYear.ValidateField("", "CCODE", true);
                    if (this.pofrmGetBGYear.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQCPROJ":
                case "TXTENDQCPROJ":
                    this.pmInitPopUpDialog("EMJOB");
                    this.pofrmGetJob.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetJob.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            DataRow dtrRetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    if (this.pofrmGetBranch != null)
                    {
                        DataRow dtrBranch = this.pofrmGetBranch.RetrieveValue();

                        //if (dtrBranch != null)
                        //{
                        //    this.txtQcBranch.Tag = dtrBranch["cRowID"].ToString();
                        //    this.txtQcBranch.Text = dtrBranch["cCode"].ToString().TrimEnd();
                        //    this.txtQnBranch.Text = dtrBranch["cName"].ToString().TrimEnd();
                        //}
                        //else
                        //{
                        //    this.txtQcBranch.Tag = "";
                        //    this.txtQcBranch.Text = "";
                        //    this.txtQnBranch.Text = "";
                        //}
                    }
                    break;
                case "TXTQCSECT":
                case "TXTQNSECT":
                    if (this.pofrmGetSect != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetSect.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcSect.Tag = dtrPDGRP["CROWID"].ToString();
                            this.txtQcSect.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                            this.txtQnSect.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcSect.Tag = "";
                            this.txtQcSect.Text = "";
                            this.txtQnSect.Text = "";
                        }
                    }
                    break;
                case "TXTYEAR":
                    if (this.pofrmGetBGYear != null)
                    {
                        DataRow dtrYear = this.pofrmGetBGYear.RetrieveValue();

                        if (dtrYear != null)
                        {
                            this.txtYear.Tag = dtrYear["cRowID"].ToString();
                            this.txtYear.Text = dtrYear["cCode"].ToString().TrimEnd();
                            this.mintYear = Convert.ToInt32(dtrYear["nYear"]);
                        }
                        else
                        {
                            this.txtYear.Tag = "";
                            this.txtYear.Text = "";
                            this.mintYear = 0;
                        }
                    }
                    break;
                case "TXTBEGQCPROJ":
                    dtrRetVal = this.pofrmGetJob.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtBegQcProj.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtBegQcProj.Text = "";
                    }
                    break;
                case "TXTENDQCPROJ":
                    dtrRetVal = this.pofrmGetJob.RetrieveValue();
                    if (dtrRetVal != null)
                    {
                        this.txtEndQcProj.Text = dtrRetVal["cCode"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtEndQcProj.Text = "";
                    }
                    break;

            }
        }

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtQcSect_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCSECT" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcSect.Tag = "";
                this.txtQcSect.Text = "";
                this.txtQnSect.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("EMSECT");
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

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
                this.txtYear.Tag = "";
                this.txtYear.Text = "";
                this.mintYear = 0;
            }
            else
            {
                this.pmInitPopUpDialog("BGYEAR");
                e.Cancel = !this.pofrmGetBGYear.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBGYear.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }
        }

        private void txtQcProj_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "CCODE";

            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("EMJOB");
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Print, this.mstrTaskName, "", "", App.FMAppUserID, App.AppUserName);
            this.pmPrintData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmPrintData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strFld = "BGTRANHD.CROWID, BGTRANHD.NAMT ";
            strFld += " , BGTRANHD.CISAPPROVE, BGTRANHD.CAPVSTAT, BGTRANHD.CISAPPROV2 , BGTRANHD.CISPOST ";
            strFld += " , BGTRANHD.CISCORRECT ";
            strFld += " , EMSECT.CCODE as QCSECT, EMSECT.CNAME as QNSECT ";
            strFld += " , EMJOB.CCODE as QCJOB, EMJOB.CNAME as QNJOB ";
            strFld += " , BGTRANHD.DCREATE, BGTRANHD.DLASTUPDBY ";
            strFld += " , EM1.CLOGIN as CLOGIN_ADD";
            strFld += " , EM2.CLOGIN as CLOGIN_UPD";
            strFld += " , EM3.CLOGIN as CLOGIN_APV ";
            strFld += " , EM4.CLOGIN as CLOGIN_CORRECT ";
            strFld += " , EM5.CLOGIN as CLOGIN_APVBY ";
            strFld += " , EM6.CLOGIN as CLOGIN_POST ";
            strFld += " , BGTRANHD.DAPPROVE ";
            strFld += " , BGTRANHD.DISCORRECT ";
            strFld += " , BGTRANHD.DAPV ";
            strFld += " , BGTRANHD.DPOST ";

            string strSQLExec = "select " + strFld + " from {0} BGTRANHD ";
            strSQLExec += " left join EMSECT ON EMSECT.CROWID = BGTRANHD.CSECT";
            strSQLExec += " left join EMJOB ON EMJOB.CROWID = BGTRANHD.CJOB ";
            strSQLExec += " left join {1} EM1 ON EM1.CROWID = BGTRANHD.CCREATEBY ";
            strSQLExec += " left join {1} EM2 ON EM2.CROWID = BGTRANHD.CLASTUPDBY ";
            strSQLExec += " left join {1} EM3 ON EM3.CROWID = BGTRANHD.CAPPROVEBY ";
            strSQLExec += " left join {1} EM4 ON EM4.CROWID = BGTRANHD.CCORRECTBY ";
            strSQLExec += " left join {1} EM5 ON EM5.CROWID = BGTRANHD.CAPVBY ";
            strSQLExec += " left join {1} EM6 ON EM6.CROWID = BGTRANHD.CPOSTBY ";
            strSQLExec += " where BGTRANHD.CCORP = ? and BGTRANHD.CBRANCH = ? and BGTRANHD.CSECT = ? and BGTRANHD.NBGYEAR = ? ";
            strSQLExec += " and EMJOB.CCODE between ? and ? ";

            strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.BudTranHD, "APPLOGIN" });

            Report.LocalDataSet.DTSPBGSTAT dtsPreviewReport = new Report.LocalDataSet.DTSPBGSTAT();

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.txtQcSect.Tag.ToString(), this.mintYear, this.txtBegQcProj.Text.TrimEnd(), this.txtEndQcProj.Text.TrimEnd() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBGTran", "BGTRANHD", strSQLExec, ref strErrorMsg))
            {
                string strLogin1 = ""; string strStat1 = "";
                string strLogin2 = ""; string strStat2 = "";
                string strLogin3 = ""; string strStat3 = "";
                string strLogin4 = ""; string strStat4 = "";
                foreach (DataRow dtrPBGTran in this.dtsDataEnv.Tables["QBGTran"].Rows)
                {
                    DataRow dtrPreview = dtsPreviewReport.XRPBGSTAT.NewRow();

                    strLogin1 = ""; strStat1 = "";
                    strLogin2 = ""; strStat2 = "";
                    strLogin3 = ""; strStat3 = "";
                    strLogin4 = ""; strStat4 = "";

                    dtrPreview["cQcJob"] = dtrPBGTran["QcJob"].ToString().TrimEnd();
                    dtrPreview["cQnJob"] = dtrPBGTran["QnJob"].ToString().TrimEnd();
                    //
                    if (dtrPBGTran["CISAPPROVE"].ToString() != SysDef.gc_APPROVE_STEP_WAIT)
                    {
                        strLogin1 = dtrPBGTran["CLOGIN_APV"].ToString();
                        strStat1 = "APPROVE";
                        dtrPreview["dApprove1"] = Convert.ToDateTime(dtrPBGTran["DAPPROVE"]);

                        strLogin2 = "-";
                        strStat2 = "WAIT";
                    }
                    else
                    {
                        strLogin1 = "-";
                        strStat1 = "WAIT";
                    }

                    if (dtrPBGTran["CISCORRECT"].ToString() != SysDef.gc_APPROVE_STEP_WAIT)
                    {
                        strLogin2 = dtrPBGTran["CLOGIN_CORRECT"].ToString();
                        strStat2 = "APPROVE";
                        dtrPreview["dApprove2"] = Convert.ToDateTime(dtrPBGTran["DISCORRECT"]);
                        strLogin3 = "-";
                        strStat3 = "WAIT";
                    }
                    else
                    {
                        //strLogin2 = "";
                        //strStat2 = "";
                    }

                    if (dtrPBGTran["CAPVSTAT"].ToString() != SysDef.gc_APPROVE_STEP_WAIT)
                    {
                        strLogin3 = dtrPBGTran["CLOGIN_APVBY"].ToString();
                        strStat3 = BudgetHelper.GetApproveStepText(BudgetHelper.GetApproveStep(dtrPBGTran["CAPVSTAT"].ToString()));

                        if (dtrPBGTran["CAPVSTAT"].ToString() == SysDef.gc_APPROVE_STEP_APPROVE)
                        {
                            strStat3 = "PASS";
                        }
                        dtrPreview["dApprove3"] = Convert.ToDateTime(dtrPBGTran["DAPV"]);
                        strLogin4 = "-";
                        strStat4 = "WAIT";
                    }
                    else
                    {
                        //strLogin3 = "";
                        //strStat3 = "";
                    }

                    if (dtrPBGTran["CISPOST"].ToString() != SysDef.gc_APPROVE_STEP_WAIT)
                    {
                        strLogin4 = dtrPBGTran["CLOGIN_POST"].ToString();
                        strStat4 = "POST";
                        dtrPreview["dApprove4"] = Convert.ToDateTime(dtrPBGTran["DPOST"]);
                    }
                    else
                    {
                        //strLogin4 = "";
                        //strStat4 = "";
                    }

                    dtrPreview["cLogIn1"] = strLogin1;
                    dtrPreview["cLogIn2"] = strLogin2;
                    dtrPreview["cLogIn3"] = strLogin3;
                    dtrPreview["cLogIn4"] = strLogin4;
                    dtrPreview["cStat1"] = strStat1;
                    dtrPreview["cStat2"] = strStat2;
                    dtrPreview["cStat3"] = strStat3;
                    dtrPreview["cStat4"] = strStat4;

                    dtsPreviewReport.XRPBGSTAT.Rows.Add(dtrPreview);

                }
            }

            if (dtsPreviewReport.XRPBGSTAT.Rows.Count > 0)
            {
                this.pmPreviewReport(dtsPreviewReport);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\XRPBGSTAT01.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.lblTitle.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrTaskName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
