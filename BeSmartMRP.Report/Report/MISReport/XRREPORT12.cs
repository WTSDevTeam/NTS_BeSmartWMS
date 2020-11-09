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
    public partial class XRREPORT12 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT12 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;
        private DialogForms.dlgGetCrZone pofrmGetCrZone = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        private string mstrForm = "";

        public static XRREPORT12 GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT12();
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

        public XRREPORT12()
        {
            InitializeComponent();
            this.pmInitForm();
        }


        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {

            this.txtBegQcCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_CODE;
            this.txtEndQcCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_CODE;
            this.txtBegQnCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_NAME;
            this.txtEndQnCrZone.Properties.MaxLength = DialogForms.dlgGetCrZone.MAXLENGTH_NAME;

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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPDGRP", "PDGRP", "select top 1 fcCode, fcName from " + MapTable.Table.CrZone + " where fcCorp = ? and fcCoorType = 'C' order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegQcCrZone.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcCode"].ToString();
                this.txtBegQnCrZone.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcName"].ToString();
            }

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPDGRP", "PDGRP", "select top 1 fcCode, fcName from " + MapTable.Table.CrZone + " where fcCorp = ? and fcCoorType = 'C' order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndQcCrZone.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcCode"].ToString();
                this.txtEndQnCrZone.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0]["fcName"].ToString();
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
                case "CRZONE":
                    if (this.pofrmGetCrZone == null)
                    {
                        this.pofrmGetCrZone = new DialogForms.dlgGetCrZone();
                        this.pofrmGetCrZone.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCrZone.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTBEGQCCRZONE":
                case "TXTENDQCCRZONE":
                    this.pmInitPopUpDialog("CRZONE");
                    this.pofrmGetCrZone.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetCrZone.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQNCRZONE":
                case "TXTENDQNCRZONE":
                    this.pmInitPopUpDialog("CRZONE");
                    this.pofrmGetCrZone.ValidateField(inPara1, "FCNAME", true);
                    if (this.pofrmGetCrZone.PopUpResult)
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
                case "TXTBEGQCCRZONE":
                case "TXTBEGQNCRZONE":
                case "TXTENDQCCRZONE":
                case "TXTENDQNCRZONE":
                    if (this.pofrmGetCrZone != null)
                    {
                        DataRow dtrCrZone = this.pofrmGetCrZone.RetrieveValue();

                        if (dtrCrZone != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCCRZONE" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNCRZONE")
                            {
                                this.txtBegQcCrZone.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
                                this.txtBegQnCrZone.Text = dtrCrZone["fcName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcCrZone.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
                                this.txtEndQnCrZone.Text = dtrCrZone["fcName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCCRZONE" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNCRZONE")
                            {
                                this.txtBegQcCrZone.Text = "";
                                this.txtBegQnCrZone.Text = "";
                            }
                            else
                            {
                                this.txtEndQcCrZone.Text = "";
                                this.txtEndQnCrZone.Text = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void txtQcCrZone_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";

            if (txtPopUp.Name.ToUpper() == "TXTBEGQCCRZONE" || txtPopUp.Name.ToUpper() == "TXTENDQCCRZONE")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCCRZONE" || txtPopUp.Name.ToUpper() == "TXTBEGQNCRZONE")
                {
                    this.txtBegQcCrZone.Text = "";
                    this.txtBegQnCrZone.Text = "";
                }
                else
                {
                    this.txtEndQcCrZone.Text = "";
                    this.txtEndQnCrZone.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("CRZONE");
                e.Cancel = !this.pofrmGetCrZone.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCrZone.PopUpResult)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pmPrintData()
        {

            Report.LocalDataSet.DTSSALESUM10 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM10();

            this.dataGridView1.DataSource = dtsPrintPreview.SALESUM01;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;

            App.AppMessage = "Start Extract Data...";
            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            if (this.mstrForm == "FORM1")
            {
                dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            }
            else
            {
                dttBegDate = new DateTime(this.txtBegDate.DateTime.Year - 1, 1, 1);
            }
            DateTime dttXDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1);
            dttEndDate = dttXDate.AddMonths(1).AddDays(-1);

            string strSQLStr = "select * from SALESUM05 where cCorp = ? and QcSaleZone between ? and ? and dDate between ? and ? ";
            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcCrZone.Text.TrimEnd(), this.txtEndQcCrZone.Text.TrimEnd(), dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["QcSaleZone"] = dtrGLRef["QcSaleZone"].ToString().TrimEnd();
                    dtrPreview["QnSaleZone"] = dtrGLRef["QnSaleZone"].ToString().TrimEnd();
                    dtrPreview["cCrGrpType"] = dtrGLRef["cCrGrpType"].ToString().TrimEnd();
                    dtrPreview["QcPdGrp"] = dtrGLRef["QcPdGrp"].ToString().TrimEnd();
                    dtrPreview["QnPdGrp"] = dtrGLRef["QnPdGrp"].ToString().TrimEnd();                    
                    dtrPreview["QcCrGrp"] = dtrGLRef["QcCrGrp"].ToString().TrimEnd();
                    dtrPreview["QnCrGrp"] = dtrGLRef["QnCrGrp"].ToString().TrimEnd();
                    dtrPreview["QcSEmpl"] = dtrGLRef["QcSEmpl"].ToString().TrimEnd();
                    dtrPreview["QnSEmpl"] = dtrGLRef["QnSEmpl"].ToString().TrimEnd();
                    dtrPreview["dDate"] = Convert.ToDateTime(dtrGLRef["dDate"]);
                    dtrPreview["cYear"] = dtrGLRef["cYear"].ToString().TrimEnd();

                    decimal decLastYrSale = this.pmGetLastYrSale(dtrGLRef["cCrGrp"].ToString(), dtrGLRef["cPdGrp"].ToString());
                    decimal decLastYrSale2 = this.pmGetLastYrSale2(dtrGLRef["cSaleZone"].ToString(), dtrGLRef["cPdGrp"].ToString());

                    dtrPreview["LastYrSale"] = decLastYrSale;
                    dtrPreview["LastYrSale2"] = decLastYrSale2;
                    dtrPreview["SLTarget"] = Convert.ToDecimal(dtrGLRef["SLTarget"]);
                    dtrPreview["Amt_Sale"] = Convert.ToDecimal(dtrGLRef["Amt_Sale"]);
                    dtrPreview["Amt_Ret"] = Convert.ToDecimal(dtrGLRef["Amt_Ret"]);

                    dtsPrintPreview.SALESUM01.Rows.Add(dtrPreview);

                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        private decimal pmGetLastYrSale(string inCrGrp, string inPdGrp)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            decimal decSumQty = 0;

            DateTime dttBegDate = new DateTime(this.txtBegDate.DateTime.Year - 1, 1, 1);
            DateTime dttEndDate = new DateTime(this.txtBegDate.DateTime.Year - 1, 12, 13);

            string strSQLStr = "select sum(AMT_SALE) as SumAmt from SALESUM05 where cCorp = ? and cCrGrp = ? and cPdGrp = ? and dDate between ? and ? ";
            objSQLHelper.SetPara(new object[] { App.gcCorp, inCrGrp, inPdGrp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSumLY", "SALESUM05", strSQLStr, ref strErrorMsg))
            {
                decSumQty = (Convert.IsDBNull(this.dtsDataEnv.Tables["QSumLY"].Rows[0]["SumAmt"]) ? 0 : Convert.ToDecimal(this.dtsDataEnv.Tables["QSumLY"].Rows[0]["SumAmt"]));
            }
            return decSumQty;
        }

        private decimal pmGetLastYrSale2(string inCrZone, string inPdGrp)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            decimal decSumQty = 0;

            DateTime dttBegDate = new DateTime(this.txtBegDate.DateTime.Year - 1, 1, 1);
            DateTime dttEndDate = new DateTime(this.txtBegDate.DateTime.Year - 1, 12, 13);

            string strSQLStr = "select sum(AMT_SALE) as SumAmt from SALESUM05 where cCorp = ? and cSaleZone = ? and cPdGrp = ? and dDate between ? and ? ";
            objSQLHelper.SetPara(new object[] { App.gcCorp, inCrZone, inPdGrp, dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSumLY", "SALESUM05", strSQLStr, ref strErrorMsg))
            {
                decSumQty = (Convert.IsDBNull(this.dtsDataEnv.Tables["QSumLY"].Rows[0]["SumAmt"]) ? 0 : Convert.ToDecimal(this.dtsDataEnv.Tables["QSumLY"].Rows[0]["SumAmt"]));

                if (decSumQty > 0)
                {
                    string strX = "";
                }
            }
            return decSumQty;
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(Report.LocalDataSet.DTSSALESUM10 inData)
        {

            string strRPTFileName = "";
            strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM12.rpt";
            DateTime dttXDate = DateTime.Now;
            string strRPTName = "";
            string strTitle2 = "";

            strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM12.rpt";
            strRPTName = "MAC_XRREPORT12";

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

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.gcCorpName);
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "");
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strRPTName);

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);

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
