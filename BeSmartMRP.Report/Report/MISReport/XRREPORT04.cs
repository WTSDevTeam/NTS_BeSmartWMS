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
    public partial class XRREPORT04 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT04 mInstanse = null;

        private DialogForms.dlgGetProd pofrmGetProd = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        private string mstrForm = "";

        public static XRREPORT04 GetInstanse(string inForm)
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT04(inForm);
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

        public XRREPORT04(string inForm)
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
                    this.Text = "XRREPORT04";
                    this.lblTitle.Text = "พิมพ์ข้อมูลยอดขายและ Stock";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT04";
                    break;
                case "FORM2":
                    this.Text = "XRREPORT20";
                    this.lblTitle.Text = "พิมพ์ข้อมูล Slow Stock";
                    this.lblTaskName.Text = "รหัสรายงาน : XRREPORT20";
                    break;
            }
            
            this.txtBegQcProd.Properties.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_CODE;
            this.txtEndQcProd.Properties.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_CODE;
            this.txtBegQnProd.Properties.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_NAME;
            this.txtEndQnProd.Properties.MaxLength = DialogForms.dlgGetProd.MAXLENGTH_NAME;

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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcCode, fcName from " + MapTable.Table.Product + " where fcCorp = ? order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegQcProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString();
                this.txtBegQnProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select top 1 fcCode, fcName from " + MapTable.Table.Product + " where fcCorp = ? order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndQcProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcCode"].ToString();
                this.txtEndQnProd.Text = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcName"].ToString();
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
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DialogForms.dlgGetProd();
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.ProdGroup, DialogForms.dlgGetProd.MAXLENGTH_CODE, DialogForms.dlgGetProd.MAXLENGTH_NAME);
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
                case "TXTBEGQCPROD":
                case "TXTENDQCPROD":
                    this.pmInitPopUpDialog("PROD");
                    this.pofrmGetProd.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGQNPROD":
                case "TXTENDQNPROD":
                    this.pmInitPopUpDialog("PROD");
                    this.pofrmGetProd.ValidateField(inPara1, "FCNAME", true);
                    if (this.pofrmGetProd.PopUpResult)
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
                case "TXTBEGQCPROD":
                case "TXTBEGQNPROD":
                case "TXTENDQCPROD":
                case "TXTENDQNPROD":
                    if (this.pofrmGetProd != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetProd.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPROD" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPROD")
                            {
                                this.txtBegQcProd.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtBegQnProd.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndQcProd.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                                this.txtEndQnProd.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGQCPROD" || inPopupForm.TrimEnd().ToUpper() == "TXTBEGQNPROD")
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
                    }
                    break;

            }
        }

        private void txtQcProd_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            if (txtPopUp.Name.ToUpper() == "TXTBEGQCPROD" || txtPopUp.Name.ToUpper() == "TXTENDQCPROD")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                if (txtPopUp.Name.ToUpper() == "TXTBEGQCPROD" || txtPopUp.Name.ToUpper() == "TXTBEGQNPROD")
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

            string strSQLStr = "select cProd, sum(Sale_Qty) as SumQty from STOCK02 where cCorp = ? and QcProd between ? and ? and dDate between ? and ? group by cProd ";

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = this.txtBegDate.DateTime;// new DateTime(this.txtBegDate.DateTime.Year, 1, 1);
            dttEndDate = this.txtEndDate.DateTime;   // new DateTime(this.txtEndDate.DateTime.Year, 12, 13);

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcProd.Text.TrimEnd(), this.txtEndQcProd.Text.TrimEnd(), dttBegDate.Date, dttEndDate.Date });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    objSQLHelper2.SetPara(new object[] { dtrGLRef["cProd"].ToString() });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
                    {

                        DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                        dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                        decimal decBfQty = 0;
                        this.pmGetStockAtDate(dtrGLRef["cProd"].ToString(), this.txtEndDate.DateTime.Date, ref decBfQty);

                        dtrPreview["cQcProd"] = dtrProd["fcCode"].ToString().TrimEnd();
                        dtrPreview["cQnProd"] = dtrProd["fcName"].ToString().TrimEnd();

                        dtrPreview["nQty"] = decBfQty;
                        dtrPreview["nStdPrice"] = this.pmToDecimal(dtrProd, "fnPrice");
                        dtrPreview["nSaleQty"] = this.pmToDecimal(dtrGLRef, "SumQty");

                        dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                    }


                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        private void pmGetStockAtDate(string inProd, DateTime inDate, ref decimal ioQty)
        {
            ioQty = 0;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { inProd, inDate });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "STOCK02", "select sum(QTY) as SumQty from STOCK02 where CPROD = ? and DDATE <= ? ", ref strErrorMsg))
            {
                ioQty = this.pmToDecimal(this.dtsDataEnv.Tables["QBFStock"].Rows[0], "SumQty");
            }
        }

        private decimal pmToDecimal(DataRow inSource, string inFld)
        {
            return (Convert.IsDBNull(inSource[inFld]) ? 0 : Convert.ToDecimal(inSource[inFld]));
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = "";
            string strReportTitle = "";
            string strRPTName = "";
            switch (this.mstrForm)
            {
                case "FORM1":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM04.rpt";
                    strReportTitle = "ข้อมูลยอดขายและ Stock ณ วันที่ ";
                    strRPTName = "MAC_XRREPORT04";
                    break;
                case "FORM2":
                    strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM20.rpt";
                    strReportTitle = "ข้อมูล Slow Stock ณ วันที่ ";
                    strRPTName = "MAC_XRREPORT20";
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

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strReportTitle + this.txtEndDate.DateTime.ToString("dd MMMM yyyy"));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtBegDate.DateTime.ToString("dd MMMM "));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtEndDate.DateTime.ToString("dd MMMM "));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, strRPTName);

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
