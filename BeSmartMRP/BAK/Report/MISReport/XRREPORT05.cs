using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

using mBudget.Business.Entity;
using mBudget.DatabaseForms;
using mBudget.UIHelper;

namespace mBudget.Report.MISReport
{
    public partial class XRREPORT05 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT05 mInstanse = null;

        private string mstrForm = "";

        private DialogForms.dlgGetEmplSM pofrmGetEmpl = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        public static XRREPORT05 GetInstanse(string inForm)
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT05(inForm);
            }
            return mInstanse;
        }

        private static void pmClearInstanse()
        {
            if (mInstanse != null)
            {
                mInstanse = null;
            }
        }

        public XRREPORT05(string inForm)
        {
            this.mstrForm = inForm;
            InitializeComponent();
            this.pmInitForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            switch (this.mstrForm)
            {
                case "FORM1":
                    this.Text = "XRREPORT05";
                    this.lblTitle.Text = "รายงานสินค้ารวม";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT05";
                    break;
                case "FORM2":
                    this.Text = "XRREPORT06";
                    this.lblTitle.Text = "รายละเอียดกลุ่มลูกค้ารวม";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT06";
                    break;
            }

            this.txtBegQcEmpl.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
            this.txtEndQcEmpl.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
            this.txtBegQnEmpl.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;
            this.txtEndQnEmpl.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;

            this.pmDefaultOption();

        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEMPL", "SEMPL", "select top 1 fcCode, fcName from " + MapTable.Table.Employee + " where fcCorp = ? and fcType = 'S' order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegQcEmpl.Text = this.dtsDataEnv.Tables["QEMPL"].Rows[0]["fcCode"].ToString();
                this.txtBegQnEmpl.Text = this.dtsDataEnv.Tables["QEMPL"].Rows[0]["fcName"].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEMPL", "SEMPL", "select top 1 fcCode, fcName from " + MapTable.Table.Employee + " where fcCorp = ? and fcType = 'S' order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndQcEmpl.Text = this.dtsDataEnv.Tables["QEMPL"].Rows[0]["fcCode"].ToString();
                this.txtEndQnEmpl.Text = this.dtsDataEnv.Tables["QEMPL"].Rows[0]["fcName"].ToString();
            }

            this.pmSetRngStatus();
        }

        private void cmbWRng_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetRngStatus();
        }

        private void pmSetRngStatus()
        {
            //this.pnlRngCode.Enabled = (this.cmbWRng.SelectedIndex == 0);
            //this.pnlTagCode.Enabled = (this.cmbWRng.SelectedIndex == 1);
        }

        private void cmbPOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetOrder();
        }

        private void pmSetOrder()
        {
            //this.mstrOrderBy = (this.cmbPOrderBy.SelectedIndex == 0 ? "cCode" : "cName");
            this.mstrOrderBy = "fcCode";
            this.pmDefaultOption();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "SEMPL":
                    if (this.pofrmGetEmpl == null)
                    {
                        this.pofrmGetEmpl = new DialogForms.dlgGetEmplSM("S");
                        this.pofrmGetEmpl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmpl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "TAG_PDGRP":
                    using (DialogForms.dlgTagItems dlg = new mBudget.DialogForms.dlgTagItems())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.FCSKID, {0}.FCCODE, {0}.FCNAME from {0} ";

                        string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                        string strEmplRTab = strFMDBName + ".dbo.EMPLR";
                        strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.ProdGroup, strEmplRTab });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.ProdGroup, DialogForms.dlgGetPdGrp.MAXLENGTH_CODE, DialogForms.dlgGetPdGrp.MAXLENGTH_NAME);
                        dlg.ShowDialog();
                        if (dlg.PopUpResult)
                        {
                            dlg.LoadTagValue(ref this.pATagCode);
                            //this.txtTagCode.Text = this.pmGetRngCode();
                        }
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonCick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonCick(string inTextbox, string inPara1)
        {
            switch (inTextbox)
            {
                case "TXTBEGQCEMPL":
                case "TXTENDQCEMPL":
                    this.pmInitPopUpDialog("SEMPL");
                    this.pofrmGetEmpl.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetEmpl.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQNEMPL":
                case "TXTENDQNEMPL":
                    this.pmInitPopUpDialog("SEMPL");
                    this.pofrmGetEmpl.ValidateField(inPara1, "FCNAME", true);
                    if (this.pofrmGetEmpl.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_PDGRP");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGQCEMPL":
                case "txtBegQnEmpl":
                case "txtEndQcEmpl":
                case "txtEndQnEmpl":
                    if (this.pofrmGetEmpl != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetEmpl.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCEMPL" || inPopupForm.TrimEnd().ToUpper() == "txtBegQnEmpl")
                            {
                                this.txtBegQcEmpl.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtBegQnEmpl.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcEmpl.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtEndQnEmpl.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCEMPL" || inPopupForm.TrimEnd().ToUpper() == "txtBegQnEmpl")
                            {
                                this.txtBegQcEmpl.Text = "";
                                this.txtBegQnEmpl.Text = "";
                            }
                            else
                            {
                                this.txtEndQcEmpl.Text = "";
                                this.txtEndQnEmpl.Text = "";
                            }
                        }
                    }
                    break;

            }
        }

        private void txtQcPdGrp_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTBEGQCEMPL" || txtPopUp.Name.ToUpper() == "TXTENDQCEMPL")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCEMPL" || txtPopUp.Name.ToUpper() == "TXTBEGQNEMPL")
                {
                    this.txtBegQcEmpl.Text = "";
                    this.txtBegQnEmpl.Text = "";
                }
                else
                {
                    this.txtEndQcEmpl.Text = "";
                    this.txtEndQnEmpl.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("SEMPL");
                e.Cancel = !this.pofrmGetEmpl.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetEmpl.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private string pmGetRngCode()
        {
            string strTagStr = "";
            for (int intCnt = 0; intCnt < this.pATagCode.Count; intCnt++)
            {
                strTagStr += "'" + this.pATagCode[intCnt].ToString() + "', ";
            }
            strTagStr = (strTagStr.Length > 2 ? AppUtil.StringHelper.Left(strTagStr, strTagStr.Length - 2) : "");
            return strTagStr;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.pmPrintData();
        }

        private void pmPrintData()
        {

            string strSQLStr = "select * from SALESUM03 where cCorp = ? and QcSEmpl between ? and ? and dDate between ? and ? order by QcSEmpl, dDate";

            Report.LocalDataSet.DTSSALESUM10 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM10();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcEmpl.Text.TrimEnd(), this.txtEndQcEmpl.Text.TrimEnd(), this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["QcSEmpl"] = dtrGLRef["QcSEmpl"].ToString().TrimEnd();
                    dtrPreview["QnSEmpl"] = dtrGLRef["QnSEmpl"].ToString().TrimEnd();
                    dtrPreview["QcProd"] = dtrGLRef["QcProd"].ToString().TrimEnd();
                    dtrPreview["QnProd"] = dtrGLRef["QnProd"].ToString().TrimEnd();
                    dtrPreview["QcCoor"] = dtrGLRef["QcCoor"].ToString().TrimEnd();
                    dtrPreview["QnCoor"] = dtrGLRef["QnCoor"].ToString().TrimEnd();

                    dtrPreview["Amt_Sale"] = Convert.ToDecimal(dtrGLRef["Amt_Sale"]);
                    dtrPreview["Amt_Ret"] = Convert.ToDecimal(dtrGLRef["Amt_Ret"]);

                    dtsPrintPreview.SALESUM01.Rows.Add(dtrPreview);

                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = "";
            string strRPTName = "";

            switch (this.mstrForm)
            {
                case "FORM1":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM05.rpt";
                    strRPTName = "MAC_XRREPORT05";
                    break;
                case "FORM2":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM06.rpt";
                    strRPTName = "MAC_XRREPORT06";
                    break;
            }

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rptPreviewReport.Load(strRPTFileName);

            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strRPTName);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.cmbPOrderBy.SelectedIndex == 0 ? "รหัส" : "ชื่อ") + "ผังกลุ่มสินค้า " + this.txtBegPdGrp.Text.TrimEnd() + " ถึง " + this.txtEndPdGrp.Text.TrimEnd());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
