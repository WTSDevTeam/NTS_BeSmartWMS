
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
    public partial class XRREPORT15 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT15 mInstanse = null;

        private DialogForms.dlgGetEmZone pofrmGetEmZone = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        private string mstrForm = "";

        public static XRREPORT15 GetInstanse(string inForm)
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT15(inForm);
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

        public XRREPORT15(string inForm)
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

            this.txtBegQcEmZone.Properties.MaxLength = DialogForms.dlgGetEmZone.MAXLENGTH_CODE;
            this.txtBegQnEmZone.Properties.MaxLength = DialogForms.dlgGetEmZone.MAXLENGTH_CODE;
            this.txtEndQcEmZone.Properties.MaxLength = DialogForms.dlgGetEmZone.MAXLENGTH_NAME;
            this.txtEndQnEmZone.Properties.MaxLength = DialogForms.dlgGetEmZone.MAXLENGTH_NAME;

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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEmZone", "EmZone", "select top 1 fcCode, fcName from EMZONE where fcCorp = ? and fcEmplType = 'S' order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegQcEmZone.Text = this.dtsDataEnv.Tables["QEmZone"].Rows[0]["fcCode"].ToString();
                this.txtBegQnEmZone.Text = this.dtsDataEnv.Tables["QEmZone"].Rows[0]["fcName"].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEmZone", "EmZone", "select top 1 fcCode, fcName from EMZONE where fcCorp = ? and fcEmplType = 'S' order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndQcEmZone.Text = this.dtsDataEnv.Tables["QEmZone"].Rows[0]["fcCode"].ToString();
                this.txtEndQnEmZone.Text = this.dtsDataEnv.Tables["QEmZone"].Rows[0]["fcName"].ToString();
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
                case "EMZONE":
                    if (this.pofrmGetEmZone == null)
                    {
                        this.pofrmGetEmZone = new DialogForms.dlgGetEmZone();
                        this.pofrmGetEmZone.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmZone.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;

                case "TAG_EMZONE":
                    using (DialogForms.dlgTagItems dlg = new mBudget.DialogForms.dlgTagItems())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.FCSKID, {0}.FCCODE, {0}.FCNAME from {0} ";

                        string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                        string strEmplRTab = strFMDBName + ".dbo.EMPLR";
                        strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.ProdGroup, strEmplRTab });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.ProdGroup, DialogForms.dlgGetEmZone.MAXLENGTH_CODE, DialogForms.dlgGetEmZone.MAXLENGTH_NAME);
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
                case "TXTBEGQCEMZONE":
                case "TXTENDQCEMZONE":
                    this.pmInitPopUpDialog("EMZONE");
                    this.pofrmGetEmZone.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetEmZone.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQNEMZONE":
                case "TXTENDQNEMZONE":
                    this.pmInitPopUpDialog("EMZONE");
                    this.pofrmGetEmZone.ValidateField(inPara1, "FCNAME", true);
                    if (this.pofrmGetEmZone.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_EmZone");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGQCEMZONE":
                case "TXTBEGQNEMZONE":
                case "TXTENDQCEMZONE":
                case "TXTENDQNEMZONE":
                    if (this.pofrmGetEmZone != null)
                    {
                        DataRow dtrEmZone = this.pofrmGetEmZone.RetrieveValue();

                        if (dtrEmZone != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCEMZONE" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNEMZONE")
                            {
                                this.txtBegQcEmZone.Text = dtrEmZone["fcCode"].ToString().TrimEnd();
                                this.txtBegQnEmZone.Text = dtrEmZone["fcName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcEmZone.Text = dtrEmZone["fcCode"].ToString().TrimEnd();
                                this.txtEndQnEmZone.Text = dtrEmZone["fcName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCEMZONE" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNEMZONE")
                            {
                                this.txtBegQcEmZone.Text = "";
                                this.txtBegQnEmZone.Text = "";
                            }
                            else
                            {
                                this.txtEndQcEmZone.Text = "";
                                this.txtEndQnEmZone.Text = "";
                            }
                        }
                    }
                    break;

            }
        }

        private void txtQcEmZone_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";

            if (txtPopUp.Name.ToUpper() == "TXTBEGQCEMZONE" || txtPopUp.Name.ToUpper() == "TXTENDQCEMZONE")
            {
                strOrderBy = "FCCODE";
            }
            else 
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCEMZONE" || txtPopUp.Name.ToUpper() == "TXTBEGQNEMZONE")
                {
                    this.txtBegQcEmZone.Text = "";
                    this.txtBegQnEmZone.Text = "";
                }
                else
                {
                    this.txtEndQcEmZone.Text = "";
                    this.txtEndQnEmZone.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("EMZONE");
                e.Cancel = !this.pofrmGetEmZone.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetEmZone.PopUpResult)
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

            string strSQLStr = "select * from SALESUM02 where cCorp = ? and QcSaleTeam between ? and ? and dDate between ? and ? ";

            Report.LocalDataSet.DTSSALESUM10 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM10();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;

            App.AppMessage = "Start Extract Data...";
            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year-1, 1, 1);
            dttEndDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcEmZone.Text.TrimEnd(), this.txtEndQcEmZone.Text.TrimEnd(), dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["QcSaleTeam"] = dtrGLRef["QcSaleTeam"].ToString().TrimEnd();
                    dtrPreview["QnSaleTeam"] = dtrGLRef["QnSaleTeam"].ToString().TrimEnd();
                    dtrPreview["QcSEmpl"] = dtrGLRef["QcSEmpl"].ToString().TrimEnd();
                    dtrPreview["QnSEmpl"] = dtrGLRef["QnSEmpl"].ToString().TrimEnd();

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

                    dtsPrintPreview.SALESUM01.Rows.Add(dtrPreview);

                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(Report.LocalDataSet.DTSSALESUM10 inData)
        {

            string strRPTFileName = "";
            strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM15.rpt";
            //switch (this.mstrForm)
            //{
            //    case "FORM1":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM13.rpt";
            //        break;
            //    case "FORM2":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM11.rpt";
            //        break;
            //    case "FORM3":
            //        strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM14.rpt";
            //        break;
            //}

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

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.cmbPOrderBy.SelectedIndex == 0 ? "รหัส" : "ชื่อ") + "ผังกลุ่มสินค้า " + this.txtBegQcEmZone.Text.TrimEnd() + " ถึง " + this.txtEndQcEmZone.Text.TrimEnd());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.gcCorpName);
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "ณ " + this.txtBegDate.DateTime.ToString("yyyy") + " - " + this.txtEndDate.DateTime.ToString("yyyy"));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "MAC_XRREPORT15");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

        private void pmOpenSubReport(Report.LocalDataSet.DTSSALESUM01 inData, ReportDocument inMasterReport)
        {
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
