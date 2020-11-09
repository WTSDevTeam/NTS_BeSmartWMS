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

namespace mBudget.Report
{
    public partial class frmPDept : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static frmPDept mInstanse = null;

        private frmDept pofrmGetDept = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "cCode";

        public static frmPDept GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmPDept();
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
        
        public frmPDept()
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

            this.cmbPOrderBy.Properties.Items.AddRange(new object[] { "รหัส", "ชื่อ" });
            this.cmbPOrderBy.SelectedIndex = 0;
            this.pmSetOrder();

            this.cmbWRng.Properties.Items.AddRange(new object[] { "ระบุเป็นช่วง", "ระบุเป็นรายตัว" });
            this.cmbWRng.SelectedIndex = 0;

            this.pmDefaultOption();

        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select top 1 cCode, cName from " + MapTable.Table.MasterDept + " order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegCode.Text = this.dtsDataEnv.Tables["QDept"].Rows[0][this.mstrOrderBy].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", "select top 1 cCode , cName from " + MapTable.Table.MasterDept + " order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndCode.Text = this.dtsDataEnv.Tables["QDept"].Rows[0][this.mstrOrderBy].ToString();
            }
            this.pmSetRngStatus();
        }

        private void cmbWRng_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetRngStatus();
        }

        private void pmSetRngStatus()
        {
            this.pnlRngCode.Enabled = (this.cmbWRng.SelectedIndex == 0);
            this.pnlTagCode.Enabled = (this.cmbWRng.SelectedIndex == 1);
        }

        private void cmbPOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetOrder();
        }

        private void pmSetOrder()
        {
            this.mstrOrderBy = (this.cmbPOrderBy.SelectedIndex == 0 ? "cCode" : "cName");

            if (this.mstrOrderBy == "cCode")
            {
                this.txtBegCode.Properties.MaxLength = frmDept.MAXLENGTH_CODE;
                this.txtEndCode.Properties.MaxLength = frmDept.MAXLENGTH_CODE;
            }
            else
            {
                this.txtBegCode.Properties.MaxLength = frmDept.MAXLENGTH_NAME;
                this.txtEndCode.Properties.MaxLength = frmDept.MAXLENGTH_NAME;
            }
            this.pmDefaultOption();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "DEPT":
                    if (this.pofrmGetDept == null)
                    {
                        this.pofrmGetDept = new DatabaseForms.frmDept(FormActiveMode.Report);
                        this.pofrmGetDept.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetDept.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "TAG_CRGRP":
                    using (DialogForms.dlgTagItems dlg = new mBudget.DialogForms.dlgTagItems())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.CROWID, {0}.CCODE, {0}.CNAME, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
                        strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
                        strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";

                        string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                        string strEmplRTab = strFMDBName + ".dbo.EMPLR";
                        strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.MasterDept, strEmplRTab });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.MasterDept, frmDept.MAXLENGTH_CODE, frmDept.MAXLENGTH_NAME);
                        dlg.ShowDialog();
                        if (dlg.PopUpResult)
                        {
                            dlg.LoadTagValue(ref this.pATagCode);
                            this.txtTagCode.Text = this.pmGetRngCode();
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
                case "TXTBEGCODE":
                case "TXTENDCODE":
                    this.pmInitPopUpDialog("DEPT");
                    this.pofrmGetDept.ValidateField(inPara1, this.mstrOrderBy, true);
                    if (this.pofrmGetDept.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_CRGRP");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGCODE":
                case "TXTENDCODE":
                    if (this.pofrmGetDept != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetDept.RetrieveValue();

                        if (dtrAcChart != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGCODE")
                            {
                                this.txtBegCode.Text = dtrAcChart[this.mstrOrderBy].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndCode.Text = dtrAcChart[this.mstrOrderBy].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGCODE")
                            {
                                this.txtBegCode.Text = "";
                            }
                            else
                            {
                                this.txtEndCode.Text = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void txtQcCrGrp_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = this.mstrOrderBy;
            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("DEPT");
                e.Cancel = !this.pofrmGetDept.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetDept.PopUpResult)
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
            for (int intCnt= 0;intCnt<this.pATagCode.Count;intCnt++)
            {
                strTagStr += "'" + this.pATagCode[intCnt].ToString() + "', ";
            }
            strTagStr = (strTagStr.Length > 2 ? AppUtil.StringHelper.Left(strTagStr, strTagStr.Length-2) : "");
            return strTagStr;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.pmPrintData();
        }

        private void pmPrintData()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strCriteria = "";
            string strOrderBy = "";
            string strBegValue = ""; string strEndValue = "";
            strOrderBy = "Master_Dept." + this.mstrOrderBy;
            if (this.cmbWRng.SelectedIndex == 0)
            {
                strCriteria = strOrderBy + " between ? and ?";
                objSQLHelper.SetPara(new object[] { this.txtBegCode.Text.TrimEnd(), this.txtEndCode.Text.TrimEnd() });
            }
            else
            {
                strCriteria = " Master_Dept.cCode in (" + this.pmGetRngCode() + ")";
            }

            Report.LocalDataSet.DTSACCHART dtsPrintPreview = new Report.LocalDataSet.DTSACCHART();

            string strSQLStr = "select Master_Dept.* from " + MapTable.Table.MasterDept;
            strSQLStr += " where ";
            strSQLStr += strCriteria + " order by " + strOrderBy;
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QDept", "DEPT", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPData in this.dtsDataEnv.Tables["QDept"].Rows)
                {
                    DataRow dtrPreview = dtsPrintPreview.XRPACCHART.NewRow();
                    dtrPreview["cCode"] = dtrPData["cCode"].ToString();
                    dtrPreview["cName"] = dtrPData["cName"].ToString();
                    dtrPreview["cName2"] = dtrPData["cName2"].ToString();

                    dtsPrintPreview.XRPACCHART.Rows.Add(dtrPreview);
                }
                if (dtsPrintPreview.XRPACCHART.Rows.Count > 0)
                    this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRPDEPT.rpt";

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rptPreviewReport.Load(strRPTFileName);
            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "รหัสฝ่าย " + this.txtBegCode.Text.TrimEnd() + " ถึง " + this.txtEndCode.Text.TrimEnd());
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);
        }



    }
}
