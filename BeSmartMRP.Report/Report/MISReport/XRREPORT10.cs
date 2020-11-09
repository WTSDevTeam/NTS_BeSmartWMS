
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
    public partial class XRREPORT10 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT10 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        private string mstrForm = "";

        public static XRREPORT10 GetInstanse(string inForm)
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT10(inForm);
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

        public XRREPORT10(string inForm)
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
                    this.Text = "XRREPORT10";
                    this.lblTitle.Text = "วิเคราะห์ยอดขายสุทธิและยอดเก็บเงิน";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT10";
                    break;
                case "FORM2":
                    this.Text = "XRREPORT11";
                    this.lblTitle.Text = "รายงาน Stock และ ยอดขาย";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT11";
                    break;
                case "FORM3":
                    this.Text = "XRREPORT14";
                    this.lblTitle.Text = "รายงานการับคืนแยกผลิตภัณฑ์";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT14";
                    break;
            }

            this.txtBegQcPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
            this.txtEndQcPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
            this.txtBegQnPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;
            this.txtEndQnPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;

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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPDGRP", "PDGRP", "select top 1 fcCode, fcName from " + MapTable.Table.ProdGroup + " where fcCorp = ? and fcType = 'G' order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegQcPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcCode"].ToString();
                this.txtBegQnPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcName"].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPDGRP", "PDGRP", "select top 1 fcCode, fcName from " + MapTable.Table.ProdGroup + " where fcCorp = ? and fcType = 'G' order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndQcPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcCode"].ToString();
                this.txtEndQnPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcName"].ToString();
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
                case "PDGRP":
                    if (this.pofrmGetPdGrp == null)
                    {
                        this.pofrmGetPdGrp = new DialogForms.dlgGetPdGrp();
                        this.pofrmGetPdGrp.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdGrp.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTBEGQCPDGRP":
                case "TXTENDQCPDGRP":
                    this.pmInitPopUpDialog("PDGRP");
                    this.pofrmGetPdGrp.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetPdGrp.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQNPDGRP":
                case "TXTENDQNPDGRP":
                    this.pmInitPopUpDialog("PDGRP");
                    this.pofrmGetPdGrp.ValidateField(inPara1, "FCNAME", true);
                    if (this.pofrmGetPdGrp.PopUpResult)
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
                case "TXTBEGQCPDGRP":
                case "TXTBEGQNPDGRP":
                case "TXTENDQCPDGRP":
                case "TXTENDQNPDGRP":
                    if (this.pofrmGetPdGrp != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetPdGrp.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDGRP" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDGRP")
                            {
                                this.txtBegQcPdGrp.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtBegQnPdGrp.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcPdGrp.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtEndQnPdGrp.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDGRP" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDGRP")
                            {
                                this.txtBegQcPdGrp.Text = "";
                                this.txtBegQnPdGrp.Text = "";
                            }
                            else
                            {
                                this.txtEndQcPdGrp.Text = "";
                                this.txtEndQnPdGrp.Text = "";
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
            if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDGRP" || txtPopUp.Name.ToUpper() == "TXTENDQCPDGRP")
            {
                strOrderBy = "FCCODE";
            }
            else 
            {
                strOrderBy = "FCNAME";
            }
            
            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDGRP" || txtPopUp.Name.ToUpper() == "TXTBEGQNPDGRP")
                {
                    this.txtBegQcPdGrp.Text = "";
                    this.txtBegQnPdGrp.Text = "";
                }
                else
                {
                    this.txtEndQcPdGrp.Text = "";
                    this.txtEndQnPdGrp.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PDGRP");
                e.Cancel = !this.pofrmGetPdGrp.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPdGrp.PopUpResult)
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

            string strSQLStr = "select * from SALESUM01 where cCorp = ? and QcPdGrp between ? and ? and dDate between ? and ? order by QcPdGrp, dDate";

            Report.LocalDataSet.DTSSALESUM10 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM10();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, 1, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, 12, 13);

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd(), dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["QcPdGrp"] = dtrGLRef["QcPdGrp"].ToString().TrimEnd();
                    dtrPreview["QnPdGrp"] = dtrGLRef["QnPdGrp"].ToString().TrimEnd();
                    dtrPreview["dDate"] = Convert.ToDateTime(dtrGLRef["dDate"]);
                    dtrPreview["cYear"] = dtrGLRef["cYear"].ToString().TrimEnd();

                    dtrPreview["Amt_SO"] = Convert.ToDecimal(dtrGLRef["Amt_SO"]);
                    dtrPreview["Amt_Sale"] = Convert.ToDecimal(dtrGLRef["Amt_Sale"]);
                    dtrPreview["Amt_Sale_A"] = Convert.ToDecimal(dtrGLRef["Amt_Sale_A"]);
                    dtrPreview["Amt_Ret"] = Convert.ToDecimal(dtrGLRef["Amt_Ret"]);
                    dtrPreview["Amt_Ret_A"] = Convert.ToDecimal(dtrGLRef["Amt_Ret_A"]);
                    dtrPreview["Amt_Bill"] = Convert.ToDecimal(dtrGLRef["Amt_Bill"]);
                    dtrPreview["Amt_Bill_A"] = Convert.ToDecimal(dtrGLRef["Amt_Bill_A"]);
                    dtrPreview["Cost1"] = Convert.ToDecimal(dtrGLRef["Cost1"]);
                    dtrPreview["Cost2"] = Convert.ToDecimal(dtrGLRef["Cost2"]);

                    decimal decFF1 = 0;
                    if (Convert.ToDecimal(dtrGLRef["Amt_Sale_A"]) != 0)
                    {
                        decFF1 = Convert.ToDecimal(dtrGLRef["Amt_Bill_A"]) / Convert.ToDecimal(dtrGLRef["Amt_Sale_A"]) * 100;
                    }
                    dtrPreview["AMT_FF1"] = decFF1;

                    dtsPrintPreview.SALESUM01.Rows.Add(dtrPreview);

                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(Report.LocalDataSet.DTSSALESUM10 inData)
        {

            string strRPTFileName = "";
            string strRPTName = "";

            switch (this.mstrForm)
            {
                case "FORM1":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM10.rpt";
                    strRPTName = "MAC_XRREPORT10";
                    break;
                case "FORM2":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM11.rpt";
                    strRPTName = "MAC_XRREPORT11";
                    break;
                case "FORM3":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM14.rpt";
                    strRPTName = "MAC_XRREPORT14";
                    break;
            }

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rptPreviewReport.Load(strRPTFileName);

            //this.pmOpenSubReport(inData, rptPreviewReport);

            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.cmbPOrderBy.SelectedIndex == 0 ? "รหัส" : "ชื่อ") + "ผังกลุ่มสินค้า " + this.txtBegQcPdGrp.Text.TrimEnd() + " ถึง " + this.txtEndQcPdGrp.Text.TrimEnd());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.gcCorpName);
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ณ " + this.txtBegDate.DateTime.ToString("yyyy") + " - " + this.txtEndDate.DateTime.ToString("yyyy"));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strRPTName);

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

        private void pmOpenSubReport(Report.LocalDataSet.DTSSALESUM01 inData, ReportDocument inMasterReport)
        {
            this.dataGridView1.DataSource = inData.SALESUM01;
            Sections sections = inMasterReport.ReportDefinition.Sections;
            foreach (Section section in sections)
            {
                ReportObjects reportObjects = section.ReportObjects;
                foreach (ReportObject reportObject in reportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        subReportDocument.SetDataSource(inData);
                    }
                }
            }
        }

    }
}
