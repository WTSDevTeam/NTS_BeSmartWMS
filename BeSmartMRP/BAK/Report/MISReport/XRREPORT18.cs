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

    public partial class XRREPORT18 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT18 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;
        private frmPdContent pofrmGetPdCon = null;
        private frmPdClass pofrmGetPdClass = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        public static XRREPORT18 GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT18();
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

        public XRREPORT18()
        {
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

            this.txtBegQcPdCon.Properties.MaxLength = DatabaseForms.frmPdContent.MAXLENGTH_CODE;
            this.txtEndQcPdCon.Properties.MaxLength = DatabaseForms.frmPdContent.MAXLENGTH_CODE;
            this.txtBegQnPdCon.Properties.MaxLength = DatabaseForms.frmPdContent.MAXLENGTH_NAME;
            this.txtEndQnPdCon.Properties.MaxLength = DatabaseForms.frmPdContent.MAXLENGTH_NAME;

            this.txtBegQcPdClass.Properties.MaxLength = DatabaseForms.frmPdClass.MAXLENGTH_CODE;
            this.txtEndQcPdClass.Properties.MaxLength = DatabaseForms.frmPdClass.MAXLENGTH_CODE;
            this.txtBegQnPdClass.Properties.MaxLength = DatabaseForms.frmPdClass.MAXLENGTH_NAME;
            this.txtEndQnPdClass.Properties.MaxLength = DatabaseForms.frmPdClass.MAXLENGTH_NAME;

            this.pmDefaultOption();

        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
            }

            objSQLHelper2.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPDCon", "PDCONTENT", "select top 1 cCode, cName from " + MapTable.Table.PdContent + " where cCorp = ? order by cCode", ref strErrorMsg))
            {
                this.txtBegQcPdCon.Text = this.dtsDataEnv.Tables["QPDCon"].Rows[0]["cCode"].ToString().TrimEnd();
                this.txtBegQnPdCon.Text = this.dtsDataEnv.Tables["QPDCon"].Rows[0]["cName"].ToString().TrimEnd();
            }
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPDCon", "PDCONTENT", "select top 1 cCode, cName from " + MapTable.Table.PdContent + " where cCorp = ? order by cCode desc ", ref strErrorMsg))
            {
                this.txtEndQcPdCon.Text = this.dtsDataEnv.Tables["QPDCon"].Rows[0]["cCode"].ToString().TrimEnd();
                this.txtEndQnPdCon.Text = this.dtsDataEnv.Tables["QPDCon"].Rows[0]["cName"].ToString().TrimEnd();
            }

            objSQLHelper2.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPDClass", "PDCLASS", "select top 1 cCode, cName from " + MapTable.Table.PdClass + " where cCorp = ? order by cCode", ref strErrorMsg))
            {
                this.txtBegQcPdClass.Text = this.dtsDataEnv.Tables["QPDClass"].Rows[0]["cCode"].ToString().TrimEnd();
                this.txtBegQnPdClass.Text = this.dtsDataEnv.Tables["QPDClass"].Rows[0]["cName"].ToString().TrimEnd();
            }
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QPDClass", "PDCLASS", "select top 1 cCode, cName from " + MapTable.Table.PdClass + " where cCorp = ? order by cCode desc ", ref strErrorMsg))
            {
                this.txtEndQcPdClass.Text = this.dtsDataEnv.Tables["QPDClass"].Rows[0]["cCode"].ToString().TrimEnd();
                this.txtEndQnPdClass.Text = this.dtsDataEnv.Tables["QPDClass"].Rows[0]["cName"].ToString().TrimEnd();
            }

            this.pmCreateTem();

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
                case "PDCLASS":
                    if (this.pofrmGetPdClass == null)
                    {
                        this.pofrmGetPdClass = new DatabaseForms.frmPdClass(FormActiveMode.Report);
                        this.pofrmGetPdClass.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdClass.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "PDCONTENT":
                    if (this.pofrmGetPdCon == null)
                    {
                        this.pofrmGetPdCon = new DatabaseForms.frmPdContent(FormActiveMode.Report);
                        this.pofrmGetPdCon.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdCon.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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

                case "TXTBEGQCPDCON":
                case "TXTENDQCPDCON":
                    this.pmInitPopUpDialog("PDCONTENT");
                    this.pofrmGetPdCon.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetPdCon.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTBEGQNPDCON":
                case "TXTENDQNPDCON":
                    this.pmInitPopUpDialog("PDCONTENT");
                    this.pofrmGetPdCon.ValidateField(inPara1, "CNAME", true);
                    if (this.pofrmGetPdCon.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTBEGQCPDCLASS":
                case "TXTENDQCPDCLASS":
                    this.pmInitPopUpDialog("PDCLASS");
                    this.pofrmGetPdClass.ValidateField(inPara1, "CCODE", true);
                    if (this.pofrmGetPdClass.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

                case "TXTBEGQNPDCLASS":
                case "TXTENDQNPDCLASS":
                    this.pmInitPopUpDialog("PDCLASS");
                    this.pofrmGetPdClass.ValidateField(inPara1, "CNAME", true);
                    if (this.pofrmGetPdClass.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;

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
                case "TXTBEGQCPDCLASS":
                case "TXTBEGQNPDCLASS":
                case "TXTENDQCPDCLASS":
                case "TXTENDQNPDCLASS":
                    if (this.pofrmGetPdClass != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetPdClass.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDCLASS" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDCLASS")
                            {
                                this.txtBegQcPdClass.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                                this.txtBegQnPdClass.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcPdClass.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                                this.txtEndQnPdClass.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDCLASS" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDCLASS")
                            {
                                this.txtBegQcPdClass.Text = "";
                                this.txtBegQnPdClass.Text = "";
                            }
                            else
                            {
                                this.txtEndQcPdClass.Text = "";
                                this.txtEndQnPdClass.Text = "";
                            }
                        }
                    }
                    break;

                case "TXTBEGQCPDCON":
                case "TXTBEGQNPDCON":
                case "TXTENDQCPDCON":
                case "TXTENDQNPDCON":
                    if (this.pofrmGetPdCon != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetPdCon.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDCON" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDCON")
                            {
                                this.txtBegQcPdCon.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                                this.txtBegQnPdCon.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcPdCon.Text = dtrPDGRP["cCode"].ToString().TrimEnd();
                                this.txtEndQnPdCon.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPDCON" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPDCON")
                            {
                                this.txtBegQcPdCon.Text = "";
                                this.txtBegQnPdCon.Text = "";
                            }
                            else
                            {
                                this.txtEndQcPdCon.Text = "";
                                this.txtEndQnPdCon.Text = "";
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
                    this.txtBegQcPdCon.Text = "";
                    this.txtBegQnPdCon.Text = "";
                }
                else
                {
                    this.txtEndQcPdCon.Text = "";
                    this.txtEndQnPdCon.Text = "";
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

        private void txtQcPdCon_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDCON" || txtPopUp.Name.ToUpper() == "TXTENDQCPDCON")
            {
                strOrderBy = "CCODE";
            }
            else
            {
                strOrderBy = "CNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDCON" || txtPopUp.Name.ToUpper() == "TXTBEGQNPDCON")
                {
                    this.txtBegQcPdCon.Text = "";
                    this.txtBegQnPdCon.Text = "";
                }
                else
                {
                    this.txtEndQcPdCon.Text = "";
                    this.txtEndQnPdCon.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PDCONTENT");
                e.Cancel = !this.pofrmGetPdCon.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPdCon.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcPdClass_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDCLASS" || txtPopUp.Name.ToUpper() == "TXTENDQCPDCLASS")
            {
                strOrderBy = "CCODE";
            }
            else
            {
                strOrderBy = "CNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCPDCLASS" || txtPopUp.Name.ToUpper() == "TXTBEGQNPDCLASS")
                {
                    this.txtBegQcPdClass.Text = "";
                    this.txtBegQnPdClass.Text = "";
                }
                else
                {
                    this.txtEndQcPdClass.Text = "";
                    this.txtEndQnPdClass.Text = "";
                }
            }
            else
            {
                this.pmInitPopUpDialog("PDCLASS");
                e.Cancel = !this.pofrmGetPdClass.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPdClass.PopUpResult)
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
            this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Clear();
            this.pmPrintData();
            this.dataGridView1.DataSource = this.dtsDataEnv.Tables[this.mstrTemSaleSum];
        }

        private string mstrTemSaleSum = "TemSaleSum01";

        private void pmCreateTem()
        {

            DataTable dtbTemSaleSum = new DataTable(this.mstrTemSaleSum);

            dtbTemSaleSum.Columns.Add("cPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcPdGrp", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnPdGrp", System.Type.GetType("System.String"));

            dtbTemSaleSum.Columns.Add("cBook", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQcBook", System.Type.GetType("System.String"));
            dtbTemSaleSum.Columns.Add("cQnBook", System.Type.GetType("System.String"));

            //Sale Sum Field
            dtbTemSaleSum.Columns.Add("nSale_Amt", System.Type.GetType("System.Decimal"));
            dtbTemSaleSum.Columns.Add("nSale_Qty", System.Type.GetType("System.Decimal"));

            dtbTemSaleSum.Columns["cPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcPdGrp"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnPdGrp"].DefaultValue = "";

            dtbTemSaleSum.Columns["cBook"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQcBook"].DefaultValue = "";
            dtbTemSaleSum.Columns["cQnBook"].DefaultValue = "";

            dtbTemSaleSum.Columns["nSale_Amt"].DefaultValue = 0;
            dtbTemSaleSum.Columns["nSale_Qty"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemSaleSum);

        }

        private void pmPrintData()
        {

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
            DateTime dttXDate = new DateTime(this.txtEndDate.DateTime.Year, 12, 13);
            dttEndDate = dttXDate.AddMonths(1).AddDays(-1);

            string strSQLStr = "select * from SALESUM06 where cCorp = ? and QcPdCon between ? and ? and QcPdClass between ? and ? and dDate between ? and ? ";
            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcPdCon.Text.TrimEnd(), this.txtEndQcPdCon.Text.TrimEnd(), this.txtBegQcPdClass.Text.TrimEnd(), this.txtEndQcPdClass.Text.TrimEnd(), dttBegDate, dttEndDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["QcSaleZone"] = dtrGLRef["QcSaleZone"].ToString().TrimEnd();
                    dtrPreview["QnSaleZone"] = dtrGLRef["QnSaleZone"].ToString().TrimEnd();

                    dtrPreview["QcPdCon"] = dtrGLRef["QcPdCon"].ToString().TrimEnd();
                    dtrPreview["QnPdCon"] = dtrGLRef["QnPdCon"].ToString().TrimEnd();

                    dtrPreview["QcPdClass"] = dtrGLRef["QcPdClass"].ToString().TrimEnd();
                    dtrPreview["QnPdClass"] = dtrGLRef["QnPdClass"].ToString().TrimEnd();

                    dtrPreview["QcCrGrp"] = dtrGLRef["MQcCrGrp"].ToString().TrimEnd();
                    dtrPreview["QnCrGrp"] = dtrGLRef["MQnCrGrp"].ToString().TrimEnd();
                    dtrPreview["QcSEmpl"] = dtrGLRef["QcSEmpl"].ToString().TrimEnd();
                    dtrPreview["QnSEmpl"] = dtrGLRef["QnSEmpl"].ToString().TrimEnd();
                    dtrPreview["dDate"] = Convert.ToDateTime(dtrGLRef["dDate"]);
                    dtrPreview["cYear"] = dtrGLRef["cYear"].ToString().TrimEnd();

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

            string strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM18.rpt";

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rptPreviewReport.Load(strRPTFileName);
            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.gcCorpName);
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์ ตั้งแต่วันที่ : " + this.txtBegDate.DateTime.ToString("dd/MM/yy") + " ถึงวันที่ : " + this.txtEndDate.DateTime.ToString("dd/MM/yy"));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "MAC_XRREPORT18");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
