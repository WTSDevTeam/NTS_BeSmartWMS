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
    public partial class XRREPORT01 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT01 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        public static XRREPORT01 GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT01();
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

        public XRREPORT01()
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
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            string strSQLStr = "SELECT GLREF.FCSKID, GLREF.FCREFTYPE, GLREF.FNAMT, GLREF.FNVATAMT, GLREF.FNVATRATE, GLREF.FCRFTYPE, GLREF.FCREFNO, GLREF.FCCODE, GLREF.FCVATISOUT, GLREF.FNDISCAMT1, GLREF.FNDISCAMT2 ";
            strSQLStr = strSQLStr + ", REFPROD.FDDATE, REFPROD.FNQTY, REFPROD.FNPRICEKE, REFPROD.FNUMQTY, REFPROD.FNXRATE , REFPROD.FNDISCAMT, REFPROD.FNCOSTAMT, REFPROD.FNCOSTADJ, REFPROD.FNVATAMT as FNVATAMT_I ";
            strSQLStr = strSQLStr + ", PROD.FCCODE as CQCPROD, PROD.FCNAME as CQNPROD ";
            strSQLStr = strSQLStr + ", PDGRP.FCSKID as CPDGRP, PDGRP.FCCODE as CQCPDGRP, PDGRP.FCNAME as CQNPDGRP, PDGRP.FCNAME2 as CQNPDGRP2 ";
            strSQLStr = strSQLStr + ", BOOK.FCSKID as CBOOK, BOOK.FCCODE as CQCBOOK, BOOK.FCNAME as CQNBOOK ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join GLREF on REFPROD.FCGLREF = GLREF.FCSKID ";
            strSQLStr = strSQLStr + " left join PROD on REFPROD.FCPROD = PROD.FCSKID ";
            strSQLStr = strSQLStr + " left join PDGRP on PROD.FCPDGRP = PDGRP.FCSKID ";
            strSQLStr = strSQLStr + " left join BOOK on GLREF.FCBOOK = BOOK.FCSKID ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and PDGRP.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and REFPROD.FCREFPDTYP = 'P' ";
            strSQLStr = strSQLStr + " and GLREF.FCRFTYPE in ('S', 'E', 'F') ";

            objSQLHelper.SetPara(new object[] { App.gcCorp, dttBegDate, dttEndDate, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd() });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    DateTime dttDate = Convert.ToDateTime(dtrGLRef["fdDate"]);

                    DataRow dtrSumGLRef = this.dtsDataEnv.Tables[this.mstrTemSaleSum].NewRow();

                    dtrSumGLRef["cBook"] = dtrGLRef["cBook"].ToString();
                    dtrSumGLRef["cQcBook"] = dtrGLRef["cQcBook"].ToString();
                    dtrSumGLRef["cQnBook"] = dtrGLRef["cQnBook"].ToString();

                    dtrSumGLRef["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                    dtrSumGLRef["cQcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                    dtrSumGLRef["cQnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();

                    this.dtsDataEnv.Tables[this.mstrTemSaleSum].Rows.Add(dtrSumGLRef);

                    decimal decSign = (dtrGLRef["fcRfType"].ToString() == "F" ? -1 : 1);

                    decimal decAmt = this.pmCalAmt(dtrGLRef) * decSign;

                    decimal decUMQty = Convert.ToDecimal(dtrGLRef["fnUMQty"]);
                    decUMQty = (decUMQty != 0 ? decUMQty : 1);
                    decimal decQty = Convert.ToDecimal(dtrGLRef["fnQty"]) * decUMQty * decSign;

                    decimal decSum = Convert.ToDecimal(dtrSumGLRef["nSale_Amt"]);
                    decimal decSumQty = Convert.ToDecimal(dtrSumGLRef["nSale_Qty"]);

                    dtrSumGLRef["nSale_Amt"] = decSum + decAmt;
                    dtrSumGLRef["nSale_Qty"] = decSumQty + decQty;


                    DataRow dtrPreview = dtsPrintPreview.SALESUM01.NewRow();

                    dtrPreview["cBook"] = dtrGLRef["cBook"].ToString();
                    dtrPreview["QcBook"] = dtrGLRef["cQcBook"].ToString();
                    dtrPreview["QnBook"] = dtrGLRef["cQnBook"].ToString();
                    //dtrPreview["cPdGrp"] = dtrGLRef["cPdGrp"].ToString();
                    dtrPreview["QcPdGrp"] = dtrGLRef["cQcPdGrp"].ToString();
                    dtrPreview["QnPdGrp"] = dtrGLRef["cQnPdGrp"].ToString();
                    dtrPreview["Amt_Sale"] = Convert.ToDecimal(dtrSumGLRef["nSale_Amt"]);
                    dtrPreview["Qty_Sale"] = Convert.ToDecimal(dtrSumGLRef["nSale_Qty"]);

                    dtsPrintPreview.SALESUM01.Rows.Add(dtrPreview);

                }
                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        private decimal pmCalAmt(DataRow inSource)
        {
            decimal decValue = 0;
            decimal decAmt = 0;
            decimal decUMQty = Convert.ToDecimal(inSource["fnUMQty"]);
            decUMQty = (decUMQty != 0 ? decUMQty : 1);

            decAmt = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnDiscAmt1"]) + Convert.ToDecimal(inSource["fnDiscAmt2"]);
            if (inSource["fcVatisOut"].ToString() == "N")
            {
                decAmt = decAmt + Convert.ToDecimal(inSource["fnVatAmt"]);
            }
            decValue = (Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) * Convert.ToDecimal(inSource["fnQty"]) * decUMQty - Convert.ToDecimal(inSource["fnDiscAmt"])) * (decAmt == 0 ? 1 : (Convert.ToDecimal(inSource["fnAmt"]) / decAmt));
            return Math.Round(decValue, 4);
        }

        private decimal pmToDecimal(DataRow inSource, string inFld)
        {
            return (Convert.IsDBNull(inSource[inFld]) ? 0 : Convert.ToDecimal(inSource[inFld]));
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM01.rpt";

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
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "MAC_XRREPORT01");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
