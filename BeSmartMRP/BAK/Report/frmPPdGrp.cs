
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
using mBudget.Business.Component;

namespace mBudget.Report
{
    public partial class frmPPdGrp : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static frmPPdGrp mInstanse = null;

        private frmPdGrp pofrmGetPdGrp = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "cCode";

        public static frmPPdGrp GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmPPdGrp();
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
        
        public frmPPdGrp()
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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "Coor", "select top 1 cCode, cName from " + MapTable.Table.MasterPdGrp + " order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegCode.Text = this.dtsDataEnv.Tables["QCoor"].Rows[0][this.mstrOrderBy].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "Coor", "select top 1 cCode , cName from " + MapTable.Table.MasterPdGrp + " order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndCode.Text = this.dtsDataEnv.Tables["QCoor"].Rows[0][this.mstrOrderBy].ToString();
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
                this.txtBegCode.Properties.MaxLength = frmPdGrp.MAXLENGTH_CODE;
                this.txtEndCode.Properties.MaxLength = frmPdGrp.MAXLENGTH_CODE;
            }
            else
            {
                this.txtBegCode.Properties.MaxLength = frmPdGrp.MAXLENGTH_NAME;
                this.txtEndCode.Properties.MaxLength = frmPdGrp.MAXLENGTH_NAME;
            }
            this.pmDefaultOption();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "COOR":
                    if (this.pofrmGetPdGrp == null)
                    {
                        this.pofrmGetPdGrp = new DatabaseForms.frmPdGrp(FormActiveMode.Report);
                        this.pofrmGetPdGrp.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdGrp.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "TAG_COOR":
                    using (DialogForms.dlgTagItems dlg = new mBudget.DialogForms.dlgTagItems())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.CROWID, {0}.CCODE, {0}.CNAME, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
                        strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
                        strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";

                        string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                        string strEmplRTab = strFMDBName + ".dbo.EMPLR";
                        strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.MasterPdGrp, strEmplRTab });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.MasterPdGrp, frmPdGrp.MAXLENGTH_CODE, frmPdGrp.MAXLENGTH_NAME);
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
                    this.pmInitPopUpDialog("Coor");
                    this.pofrmGetPdGrp.ValidateField(inPara1, this.mstrOrderBy, true);
                    if (this.pofrmGetPdGrp.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_Coor");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGCODE":
                case "TXTENDCODE":
                    if (this.pofrmGetPdGrp != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetPdGrp.RetrieveValue();

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

        private void txtQcCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = this.mstrOrderBy;
            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("Coor");
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
            strOrderBy = "Master_PdGrp." + this.mstrOrderBy;
            if (this.cmbWRng.SelectedIndex == 0)
            {
                strCriteria = strOrderBy + " between ? and ?";
                objSQLHelper.SetPara(new object[] { this.txtBegCode.Text.TrimEnd(), this.txtEndCode.Text.TrimEnd() });
            }
            else
            {
                strCriteria = " Master_PdGrp.cCode in (" + this.pmGetRngCode() + ")";
            }

            Report.LocalDataSet.DTSACCHART dtsPrintPreview = new Report.LocalDataSet.DTSACCHART();

            string strAcChartFld = " , AcChart1.cCode as QcAcChart, AcChart1.cName as QnAcChart ";
            strAcChartFld += " , AcChart2.cCode as QcAcChart2, AcChart2.cName as QnAcChart2 ";

            string strSQLStr = "select Master_PdGrp.* "+strAcChartFld+" from " + MapTable.Table.MasterPdGrp;
            strSQLStr += " left join Master_AcChart AcChart1 on AcChart1.cRowID = Master_PdGrp.cAccBCash ";
            strSQLStr += " left join Master_AcChart AcChart2 on AcChart2.cRowID = Master_PdGrp.cAccBCred ";
            strSQLStr += " where ";
            strSQLStr += strCriteria + " order by " + strOrderBy;
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPdGrp", "PdGrp", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPData in this.dtsDataEnv.Tables["QPdGrp"].Rows)
                {
                    DataRow dtrPreview = dtsPrintPreview.XRPACCHART.NewRow();
                    dtrPreview["cCode"] = dtrPData["cCode"].ToString();
                    dtrPreview["cName"] = dtrPData["cName"].ToString();
                    dtrPreview["cName2"] = dtrPData["cName2"].ToString();
                    dtrPreview["cQcAcChart"] = dtrPData["QcAcChart"].ToString();
                    dtrPreview["cQnAcChart"] = dtrPData["QnAcChart"].ToString();
                    dtrPreview["cQcAcChart2"] = dtrPData["QcAcChart2"].ToString();
                    dtrPreview["cQnAcChart2"] = dtrPData["QnAcChart2"].ToString();

                    dtsPrintPreview.XRPACCHART.Rows.Add(dtrPreview);
                }
                if (dtsPrintPreview.XRPACCHART.Rows.Count > 0)
                    this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRPPDGRP.rpt";

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rptPreviewReport.Load(strRPTFileName);
            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.cmbPOrderBy.SelectedIndex == 0 ? "รหัส" : "ชื่อ" ) + "ผู้จำหน่าย " + this.txtBegCode.Text.TrimEnd() + " ถึง " + this.txtEndCode.Text.TrimEnd());
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);
        }



    }
}
