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

    public partial class XRREPORT08 : UIHelper.frmBase
    {
        
        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT08 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        public static XRREPORT08 GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT08();
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

        public XRREPORT08()
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

            //this.cmbPOrderBy.Properties.Items.AddRange(new object[] { "รหัสกลุ่มสินค้า", "ชื่อกลุ่มสินค้า" });
            //this.cmbPOrderBy.SelectedIndex = 0;
            //this.pmSetOrder();

            //this.cmbWRng.Properties.Items.AddRange(new object[] { "ระบุเป็นช่วง", "ระบุเป็นรายตัว" });
            //this.cmbWRng.SelectedIndex = 0;

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
                this.txtBegPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0][this.mstrOrderBy].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPDGRP", "PDGRP", "select top 1 fcCode, fcName from " + MapTable.Table.ProdGroup + " where fcCorp = ? and fcType = 'G' order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndPdGrp.Text = this.dtsDataEnv.Tables["QPDGRP"].Rows[0][this.mstrOrderBy].ToString();
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

            if (this.mstrOrderBy == "fcCode")
            {
                this.txtBegPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
                this.txtEndPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_CODE;
            }
            else
            {
                this.txtBegPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;
                this.txtEndPdGrp.Properties.MaxLength = DialogForms.dlgGetPdGrp.MAXLENGTH_NAME;
            }
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
                case "TXTBEGPDGRP":
                case "TXTENDPDGRP":
                    this.pmInitPopUpDialog("PDGRP");
                    this.pofrmGetPdGrp.ValidateField(inPara1, this.mstrOrderBy, true);
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
                case "TXTBEGPDGRP":
                case "TXTENDPDGRP":
                    if (this.pofrmGetPdGrp != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetPdGrp.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGPDGRP")
                            {
                                this.txtBegPdGrp.Text = dtrPDGRP[this.mstrOrderBy].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndPdGrp.Text = dtrPDGRP[this.mstrOrderBy].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGPDGRP")
                            {
                                this.txtBegPdGrp.Text = "";
                            }
                            else
                            {
                                this.txtEndPdGrp.Text = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void txtQcPdGrp_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = this.mstrOrderBy;
            if (txtPopUp.Text == "")
            {
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

            string strFld = " * ";
            string strSQLStr = "select * from SALESUM02 order by cRefTask, dDate";

            Report.LocalDataSet.DTSSALESUM02 dtsPrintPreview = new Report.LocalDataSet.DTSSALESUM02();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            //objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, App.gcCorp, this.txtBegPdGrp.Text.TrimEnd(), this.txtEndPdGrp.Text.TrimEnd() });

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    dtrPreview = dtsPrintPreview.SALESUM02.NewRow();

                    dtrPreview["cID"] = "01";
                    string strTaskName = dtrGLRef["cRefTask"].ToString().TrimEnd();
                    string strTaskName2 = "";
                    string strGroup = "";

                    switch (strTaskName)
                    {
                        case "1_S1":
                            strTaskName2 = "Net Sales";
                            strGroup = "A";
                            break;
                        case "2_ACCU_S1":
                            strTaskName2 = "Accum Net Sales";
                            strGroup = "A";
                            break;
                        case "3_C1":
                            strTaskName2 = "Collection";
                            strGroup = "B";
                            break;
                        case "4_ACCU_C1":
                            strTaskName2 = "Accum Collection";
                            strGroup = "B";
                            break;
                        case "5_ACC_PCN":
                            strTaskName2 = "Acc. Collection/Acc. Net Sales";
                            strGroup = "C";
                            break;
                    }

                    int intDiv = (strGroup == "C" ? 1 : 1000);
                    dtrPreview["cRefTask"] = strTaskName;
                    dtrPreview["cTaskName"] = strTaskName2;
                    dtrPreview["cGroup"] = strGroup;
                    dtrPreview["dDate"] = Convert.ToDateTime(dtrGLRef["dDate"]);
                    dtrPreview["nAmt"] = Convert.ToDecimal(dtrGLRef["nAmt"])/intDiv;

                    DateTime dttXDate = Convert.ToDateTime(dtrGLRef["dDate"]);
                    int intPrefix = 0;
                    string strMthColumn = "nAmt" + Convert.ToString(dttXDate.Month + intPrefix).TrimEnd();
                    dtrPreview[strMthColumn] = Convert.ToDecimal(dtrGLRef["nAmt"])/intDiv;

                    dtsPrintPreview.SALESUM02.Rows.Add(dtrPreview);

                }


                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(Report.LocalDataSet.DTSSALESUM02 inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM02.rpt";

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rptPreviewReport.Load(strRPTFileName);

            this.pmOpenSubReport(inData, rptPreviewReport);

            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "MAC_XRREPORT08");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

        private void pmOpenSubReport(Report.LocalDataSet.DTSSALESUM02 inData, ReportDocument inMasterReport)
        {
            this.dataGridView1.DataSource = inData.SALESUM02;
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
